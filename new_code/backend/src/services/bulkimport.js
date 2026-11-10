import { all, get, run, tx } from '../db/index.js';
import { money, now } from './helpers.js';
import { readTable, rowsToObjects, parseDelimited, sniffDelimiter } from '../util/tabular.js';

/**
 * Bulk master / stock import from a spreadsheet.
 *
 * The legacy app could only bulk-load offers and opening stock, through a
 * hidden Excel button. This generalises it: the shopkeeper drops in an .xlsx,
 * CSV or JSON file and every row is upserted into the chosen master (or into
 * opening stock). Column headings are matched loosely (case / spaces /
 * underscores are ignored) so a hand-made sheet usually just works.
 *
 * Two-step flow like the legacy migration: `analyze` reports what would happen,
 * `apply` writes. Every run is recorded in tbl_BulkImportBatch. Nothing is ever
 * deleted - existing rows are updated in place.
 */

// Masters that may be bulk-loaded, with the columns a template exposes and any
// columns that must be present. Everything else falls back to the live schema.
export const MASTER_TARGETS = {
  item: { table: 'itemmst', label: 'Item Master', required: ['itemcode', 'itemname'] },
  category: { table: 'categorymst', label: 'Brand / Category', required: ['categorycode', 'categoryname'] },
  kfl: { table: 'kflmst', label: 'KFL', required: ['kflcode', 'kflname'] },
  group: { table: 'groupmst', label: 'Group', required: ['groupcode', 'groupname'] },
  strength: { table: 'strength', label: 'Strength', required: ['strengthname'] },
  itemgroupml: { table: 'Itemgroupml', label: 'Measure and Packing', required: ['ml', 'packing'] },
  storage: { table: 'storage', label: 'Store', required: ['shopcode', 'shopname'] },
  customer: { table: 'customerDetails', label: 'Customer', required: ['custcode', 'custname'] },
  manufacturer: { table: 'ManufacturerMaster', label: 'Manufacturer', required: ['manufcode', 'manufname'] },
  ledger: { table: 'ledger', label: 'Ledger', required: ['ledcode', 'name'] },
  area: { table: 'tblArea', label: 'Area', required: ['areacode', 'areaname'] },
  counter: { table: 'countermst', label: 'Counter', required: ['countercode', 'countername'] },
};

const norm = (s) => String(s ?? '').toLowerCase().replace(/[\s_\-.]/g, '');

/** Column metadata (name/type/pk) for a table from the live schema. */
function columns(db, table) {
  return all(db, `PRAGMA table_info([${table}])`).map((c) => ({
    name: c.name,
    type: String(c.type || '').toUpperCase(),
    notnull: Number(c.notnull) === 1,
    pk: Number(c.pk) > 0,
    pkOrder: Number(c.pk),
    dflt: c.dflt_value,
  }));
}

function coerce(value, type) {
  if (value === null || value === undefined) return null;
  const s = String(value).trim();
  if (s === '' || /^null$/i.test(s)) return null;
  if (/INT/.test(type)) { const n = parseInt(s, 10); return Number.isNaN(n) ? value : n; }
  if (/REAL|FLOAT|DOUBLE|NUMERIC|DECIMAL|MONEY/.test(type)) { const n = Number(s); return Number.isNaN(n) ? value : n; }
  return s;
}

// Common heading aliases -> canonical column name. Hand-typed sheets (and the
// legacy Excel exports) use human headings like "Sale Rate" / "Item Code".
const ALIASES = {
  itemcode: ['itemcode', 'code', 'item'],
  itemname: ['itemname', 'name', 'item', 'description'],
  categorycode: ['category', 'brandcode'],
  categoryname: ['categoryname', 'brand', 'category'],
  kflcode: ['kflcode'],
  kflname: ['kfl', 'kflname'],
  groupcode: ['groupcode'],
  groupname: ['group', 'groupname'],
  strengthname: ['strength', 'strengthname'],
  ml: ['ml', 'size', 'volume'],
  packing: ['packing', 'pack', 'packsize', 'case'],
  barcode: ['barcode', 'gtin', 'ean'],
  salesrate: ['salesrate', 'salerate', 'mrp', 'saleprice', 'price'],
  purchaseRate: ['purchaserate', 'purchrate', 'cost', 'costprice'],
  min_qnty: ['minqnty', 'reorder', 'minlevel', 'minqty'],
  manufcode: ['manufcode'],
  manufname: ['manufacturer', 'manufname', 'company'],
  shopcode: ['shopcode', 'storecode', 'store'],
  shopname: ['shopname', 'storename', 'store'],
  custcode: ['custcode', 'customercode'],
  custname: ['custname', 'customer', 'name'],
  ledcode: ['ledcode', 'ledgercode'],
  areacode: ['areacode'],
  areaname: ['area', 'areaname'],
  countercode: ['countercode'],
  countername: ['counter', 'countername'],
  phone: ['phone', 'mobile', 'contact'],
  email: ['email'],
  address: ['address'],
  gstin: ['gstin', 'gst'],
  name: ['name', 'ledgername'],
  type: ['type'],
  accode: ['accode', 'accountcode'],
  qnty: ['qnty', 'quantity', 'qty', 'opening'],
  loose: ['loose'],
  box: ['box', 'boxes', 'case'],
  trndate: ['trndate', 'date', 'asondate'],
};

/** Resolve a source heading to a real target column, trying aliases too. */
function resolveColumn(k, cols, byNorm) {
  const direct = byNorm.get(norm(k));
  if (direct) return direct;
  const nk = norm(k);
  for (const [canonical, aliases] of Object.entries(ALIASES)) {
    if (aliases.includes(nk)) {
      const c = byNorm.get(norm(canonical));
      if (c) return c;
    }
  }
  return null;
}

/** Map a source row (loose header names) onto real target columns. */
function mapRow(row, cols, { company, year } = {}) {
  const byNorm = new Map(cols.map((c) => [norm(c.name), c]));
  const out = {};
  const unknown = [];
  for (const [k, v] of Object.entries(row)) {
    const c = resolveColumn(k, cols, byNorm);
    if (c) out[c.name] = coerce(v, c.type);
    else if (String(k).trim() !== '') unknown.push(k);
  }
  for (const c of cols) {
    if (norm(c.name) === 'companycode' && out.companycode == null && company != null) out.companycode = company;
    if (norm(c.name) === 'yearcode' && out.yearcode == null && year != null) out.yearcode = year;
    // Fill NOT NULL columns that have no default so an insert can succeed; the
    // primary-key columns are left alone (a missing key is a real error).
    if (!c.pk && c.notnull && out[c.name] == null && c.dflt == null) {
      out[c.name] = /INT|REAL|FLOAT|DOUBLE|NUMERIC|DECIMAL|MONEY/.test(c.type) ? 0 : '';
    }
  }
  return { values: out, unknown };
}

// ------------------------------------------------------------------ source load

/**
 * Load a source document into { format, sheetName, rows: object[] }.
 * Accepts { content } (text) or { buffer } (Buffer) with a filename.
 */
export function loadSource({ content, buffer, filename = '' } = {}) {
  if (buffer) {
    const { format, rows, sheets } = readTable(buffer, filename);
    return { format, sheetName: sheets[0]?.name, rows: rowsToObjects(rows) };
  }
  const text = String(content ?? '');
  const trimmed = text.trim();
  if (trimmed.startsWith('{') || trimmed.startsWith('[')) {
    const parsed = JSON.parse(trimmed);
    const rows = Array.isArray(parsed) ? parsed : (parsed.rows || parsed.data || []);
    return { format: 'json', rows };
  }
  const rows = parseDelimited(text, sniffDelimiter(text));
  return { format: 'text', rows: rowsToObjects(rows) };
}

// ------------------------------------------------------------------- plan/build

/**
 * Build an import plan without writing anything.
 * @param {object} opts { company, year, kind, target, rows, content, buffer, filename, source }
 */
export function buildPlan(db, opts = {}) {
  const { company, year, kind = 'masters', target } = opts;
  let rows = opts.rows;
  let filename = opts.filename;
  let source = opts.source;
  if (!rows) {
    const loaded = loadSource(opts);
    rows = loaded.rows;
    source = source || loaded.format;
  }
  const targetDef = kind === 'openingstock'
    ? { table: 'openingstockmst', label: 'Opening Stock' }
    : MASTER_TARGETS[target];
  if (!targetDef) throw new Error(`Unknown bulk target "${target}"`);
  const table = targetDef.table;
  const cols = columns(db, table);
  const pkCols = cols.filter((c) => c.pk).sort((a, b) => a.pkOrder - b.pkOrder).map((c) => c.name);

  const plan = { table, pkCols, entries: [], totals: { rows: 0, inserts: 0, updates: 0, failed: 0 } };
  const seen = new Map();
  for (let i = 0; i < rows.length; i++) {
    const { values, unknown } = mapRow(rows[i], cols, { company, year });
    const key = pkCols.map((c) => values[c]).join('\u0001');
    const missingPk = pkCols.filter((c) => values[c] == null || values[c] === '');
    if (missingPk.length) {
      plan.entries.push({ row: i + 1, action: 'FAIL', values, unknown, error: `Missing key: ${missingPk.join(', ')}` });
      plan.totals.failed++;
      plan.totals.rows++;
      continue;
    }
    const existing = seen.has(key)
      ? seen.get(key)
      : get(db, `SELECT 1 AS x FROM [${table}] WHERE ${pkCols.map((c) => `[${c}]=?`).join(' AND ')}`, pkCols.map((c) => values[c]));
    const action = existing ? 'UPDATE' : 'INSERT';
    seen.set(key, true);
    plan.entries.push({ row: i + 1, action, values, unknown });
    plan.totals.rows++;
    if (action === 'INSERT') plan.totals.inserts++; else plan.totals.updates++;
  }
  plan.filename = filename;
  plan.source = source;
  plan.kind = kind;
  return plan;
}

function writeRow(db, table, values, pkCols) {
  const names = Object.keys(values);
  const where = pkCols.map((c) => `[${c}]=?`).join(' AND ');
  const exists = get(db, `SELECT 1 AS x FROM [${table}] WHERE ${where}`, pkCols.map((c) => values[c]));
  if (exists) {
    const set = names.filter((n) => !pkCols.includes(n));
    if (set.length) {
      run(db, `UPDATE [${table}] SET ${set.map((c) => `[${c}]=?`).join(', ')} WHERE ${where}`,
        [...set.map((c) => values[c]), ...pkCols.map((c) => values[c])]);
    }
  } else {
    run(db, `INSERT INTO [${table}] (${names.map((c) => `[${c}]`).join(',')}) VALUES (${names.map(() => '?').join(',')})`,
      names.map((c) => values[c]));
  }
}

const nn = (v) => (v === undefined ? null : v);

function recordBatch(db, b) {
  run(db,
    `INSERT INTO tbl_BulkImportBatch
       (companycode, yearcode, kind, source, filename, mode, status, tables, inserted, updated, skipped, failed, startedat, finishedat, message, detail)
     VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)`,
    [nn(b.company), nn(b.year), nn(b.kind), nn(b.source), nn(b.filename), nn(b.mode), nn(b.status), b.tables || 0,
     b.inserted || 0, b.updated || 0, b.skipped || 0, b.failed || 0, nn(b.startedat), nn(b.finishedat), nn(b.message), nn(b.detail)]);
  return get(db, 'SELECT last_insert_rowid() AS id').id;
}

/** Dry run: build the plan, record an ANALYZED batch, write nothing. */
export function analyze(db, opts) {
  const plan = buildPlan(db, opts);
  const started = now();
  const batchId = recordBatch(db, {
    company: opts.company, year: opts.year, kind: plan.kind, source: plan.source, filename: plan.filename,
    mode: 'ANALYZE', status: 'ANALYZED', tables: 1,
    inserted: plan.totals.inserts, updated: plan.totals.updates, failed: plan.totals.failed,
    startedat: started, finishedat: now(),
    message: `Dry run: ${plan.totals.inserts} to insert, ${plan.totals.updates} to update`,
    detail: JSON.stringify({ table: plan.table, totals: plan.totals }),
  });
  return { batchId, mode: 'ANALYZE', ...plan, totals: plan.totals };
}

/** Apply: write every valid row in one transaction, record an APPLIED batch. */
export function apply(db, opts) {
  const plan = buildPlan(db, opts);
  const started = now();
  const result = tx(db, () => {
    let inserted = 0;
    let updated = 0;
    const errors = [];
    for (const e of plan.entries) {
      if (e.action === 'FAIL') { errors.push({ row: e.row, error: e.error }); continue; }
      try {
        writeRow(db, plan.table, e.values, plan.pkCols);
        if (e.action === 'INSERT') inserted++; else updated++;
      } catch (err) {
        errors.push({ row: e.row, error: err.message });
      }
    }
    return { inserted, updated, errors };
  });
  const batchId = recordBatch(db, {
    company: opts.company, year: opts.year, kind: plan.kind, source: plan.source, filename: plan.filename,
    mode: 'APPLY', status: result.errors.length && !result.inserted && !result.updated ? 'FAILED' : 'APPLIED',
    tables: 1, inserted: result.inserted, updated: result.updated, failed: result.errors.length,
    startedat: started, finishedat: now(), message: `Applied to ${plan.table}`,
    detail: JSON.stringify({ table: plan.table, inserted: result.inserted, updated: result.updated, errors: result.errors.slice(0, 50) }),
  });
  return {
    batchId, mode: 'APPLY', table: plan.table, kind: plan.kind,
    totals: { rows: plan.totals.rows, inserts: result.inserted, updates: result.updated, failed: result.errors.length },
    errors: result.errors,
  };
}

export function history(db, company) {
  return all(db, 'SELECT * FROM tbl_BulkImportBatch WHERE companycode=? ORDER BY id DESC LIMIT 50', [company]);
}

/** CSV templates (header + one sample row) for the common masters. */
export function templates() {
  const T = {
    item: 'itemcode,itemname,categorycode,kflcode,groupcode,packing,ml,strengthname,barcode,salesrate,purchaseRate,min_qnty',
    category: 'categorycode,categoryname',
    kfl: 'kflcode,kflname',
    group: 'groupcode,groupname',
    strength: 'strengthname',
    itemgroupml: 'ml,packing',
    storage: 'shopcode,shopname,address',
    customer: 'custcode,custname,phone,email,address,areacode,gstin',
    manufacturer: 'manufcode,manufname',
    ledger: 'ledcode,name,accode,type',
    openingstock: 'shopcode,itemcode,loose,box,qnty,trndate',
  };
  return Object.entries(T).map(([target, header]) => ({ target, header }));
}

export { rowsToObjects };
