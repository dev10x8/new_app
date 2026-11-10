import { all, get, run, tx } from '../db/index.js';
import { money, now } from './helpers.js';
import { existsSync, readdirSync, readFileSync } from 'node:fs';
import { dirname, join, basename } from 'node:path';
import { fileURLToPath } from 'node:url';

/**
 * Pre-configured state master list.
 *
 * The legacy app had no approved-brand catalogue - masters were typed by hand.
 * This adds one: a per-state list of approved brands, pack sizes (Quart / Pint
 * / Nip / ...) and government-mandated MRPs, shipped as JSON datasets under
 * backend/data/state-masters/ and loaded into tbl_StateMaster for a company.
 *
 * From a loaded list the shop can create the matching item masters in one step
 * (category / KFL / pack size / manufacturer rows are created as needed, MRP
 * becomes the selling price and the approved rate the purchase rate). An
 * uploaded list can replace the bundled one, so a shop can refresh MRPs when
 * the state revises them.
 */

const here = dirname(fileURLToPath(import.meta.url));
const DATA_DIR = join(here, '..', '..', 'data', 'state-masters');

/** Read one dataset file into { state, label, updated, brands }. */
function readDataset(file) {
  const raw = JSON.parse(readFileSync(file, 'utf8'));
  return {
    state: String(raw.state || basename(file, '.json')).toUpperCase(),
    label: raw.label || raw.state,
    updated: raw.updated || null,
    note: raw.note || '',
    brands: Array.isArray(raw.brands) ? raw.brands : [],
  };
}

/** The bundled datasets (one per state, plus the generic ALL fallback). */
export function datasets() {
  if (!existsSync(DATA_DIR)) return [];
  return readdirSync(DATA_DIR)
    .filter((f) => f.toLowerCase().endsWith('.json'))
    .map((f) => {
      const d = readDataset(join(DATA_DIR, f));
      const packs = d.brands.reduce((s, b) => s + (b.packs?.length || 0), 0);
      return { state: d.state, label: d.label, updated: d.updated, file: f, brands: d.brands.length, packs };
    })
    .sort((a, b) => a.state.localeCompare(b.state));
}

function datasetFor(state) {
  const file = join(DATA_DIR, `${state}.json`);
  if (existsSync(file)) return readDataset(file);
  const fallback = join(DATA_DIR, 'ALL.json');
  return existsSync(fallback) ? readDataset(fallback) : null;
}

/** Flatten a dataset into one row per brand/pack. */
function flatten(dataset, state, sourcefile) {
  const rows = [];
  for (const b of dataset.brands || []) {
    for (const p of b.packs || []) {
      rows.push({
        state,
        brandcode: b.brandcode || null,
        brandname: b.brandname,
        categoryname: b.categoryname || 'Brand',
        kflname: b.kflname || 'IMFL',
        strengthname: b.strengthname || null,
        ml: p.ml != null ? Number(p.ml) : null,
        packname: p.packname || null,
        packing: p.packing != null ? Number(p.packing) : null,
        mrp: p.mrp != null ? money(p.mrp) : null,
        rate: p.rate != null ? money(p.rate) : null,
        manufacturer: b.manufacturer || null,
        hsn: b.hsn || null,
        gstp: b.gstp != null ? Number(b.gstp) : null,
        sourcefile,
      });
    }
  }
  return rows;
}

/**
 * Load a state list into tbl_StateMaster (upsert). Source may be the bundled
 * dataset (`state`) or an uploaded JSON document (`content`).
 * @returns {{state, loaded, inserts, updates, brands}}
 */
export function loadState(db, { company, state, content = null, filename = null } = {}) {
  const st = String(state || '').toUpperCase();
  let dataset;
  let sourcefile;
  if (content) {
    const parsed = JSON.parse(String(content));
    dataset = { state: parsed.state || st, brands: parsed.brands || [] };
    sourcefile = filename || 'uploaded.json';
  } else {
    dataset = datasetFor(st);
    sourcefile = `${st}.json`;
  }
  if (!dataset) throw Object.assign(new Error(`No state dataset for "${st}"`), { status: 404 });

  const rows = flatten(dataset, st, sourcefile);
  const at = now();
  let inserts = 0;
  let updates = 0;
  tx(db, () => {
    for (const r of rows) {
      const existing = get(db,
        'SELECT id FROM tbl_StateMaster WHERE companycode=? AND state=? AND brandname=? AND IFNULL(ml,0)=IFNULL(?,0) AND IFNULL(mrp,0)=IFNULL(?,0)',
        [company, st, r.brandname, r.ml, r.mrp]);
      if (existing) {
        run(db,
          `UPDATE tbl_StateMaster SET brandcode=?, categoryname=?, kflname=?, strengthname=?, packname=?,
             packing=?, rate=?, manufacturer=?, hsn=?, gstp=?, sourcefile=?, loadedat=? WHERE id=?`,
          [r.brandcode, r.categoryname, r.kflname, r.strengthname, r.packname, r.packing, r.rate,
           r.manufacturer, r.hsn, r.gstp, r.sourcefile, at, existing.id]);
        updates++;
      } else {
        run(db,
          `INSERT INTO tbl_StateMaster
             (companycode, state, brandcode, brandname, categoryname, kflname, strengthname, ml, packname,
              packing, mrp, rate, manufacturer, hsn, gstp, sourcefile, loadedat)
           VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)`,
          [company, st, r.brandcode, r.brandname, r.categoryname, r.kflname, r.strengthname, r.ml,
           r.packname, r.packing, r.mrp, r.rate, r.manufacturer, r.hsn, r.gstp, r.sourcefile, at]);
        inserts++;
      }
    }
  });
  const brands = new Set(rows.map((r) => r.brandname)).size;
  return { state: st, loaded: rows.length, inserts, updates, brands, sourcefile };
}

export function listStateMaster(db, { company, state, q } = {}) {
  const params = [company];
  let where = 'companycode=?';
  if (state) { where += ' AND state=?'; params.push(String(state).toUpperCase()); }
  if (q) { where += ' AND (brandname LIKE ? OR brandcode LIKE ?)'; params.push(`%${q}%`, `%${q}%`); }
  return all(db, `SELECT * FROM tbl_StateMaster WHERE ${where} ORDER BY brandname, ml`, params);
}

export function stateSummary(db, company) {
  return all(db,
    `SELECT state, COUNT(*) AS rows, COUNT(DISTINCT brandname) AS brands, MAX(loadedat) AS loadedat
       FROM tbl_StateMaster WHERE companycode=? GROUP BY state ORDER BY state`, [company]);
}

// --------------------------------------------------- create items from the list

function nextCode(db, table, codeCol, prefix, company, width = 5) {
  const rows = all(db, `SELECT [${codeCol}] AS c FROM [${table}] WHERE [${codeCol}] LIKE ?`, [`${prefix}%`]);
  let max = 0;
  for (const r of rows) {
    const n = parseInt(String(r.c).slice(prefix.length), 10);
    if (Number.isFinite(n) && n > max) max = n;
  }
  return prefix + String(max + 1).padStart(width, '0');
}

/** Ensure a master row exists for a name; return its code. */
function ensureCode(db, { table, codeCol, nameCol, name, company, prefix, extra = {} }) {
  const found = get(db, `SELECT [${codeCol}] AS code FROM [${table}] WHERE companycode=? AND [${nameCol}]=?`, [company, name]);
  if (found) return found.code;
  const code = nextCode(db, table, codeCol, prefix, company);
  const cols = ['companycode', codeCol, nameCol, ...Object.keys(extra)];
  const vals = [company, code, name, ...Object.values(extra)];
  run(db, `INSERT INTO [${table}] (${cols.map((c) => `[${c}]`).join(',')}) VALUES (${cols.map(() => '?').join(',')})`, vals);
  return code;
}

function upsertItem(db, company, values, pkCols = ['companycode', 'itemcode']) {
  const where = pkCols.map((c) => `[${c}]=?`).join(' AND ');
  const exists = get(db, `SELECT 1 AS x FROM itemmst WHERE ${where}`, pkCols.map((c) => values[c]));
  const names = Object.keys(values);
  if (exists) {
    const set = names.filter((n) => !pkCols.includes(n));
    run(db, `UPDATE itemmst SET ${set.map((c) => `[${c}]=?`).join(', ')} WHERE ${where}`,
      [...set.map((c) => values[c]), ...pkCols.map((c) => values[c])]);
  } else {
    run(db, `INSERT INTO itemmst (${names.map((c) => `[${c}]`).join(',')}) VALUES (${names.map(() => '?').join(',')})`,
      names.map((c) => values[c]));
  }
}

/**
 * Plan (dry run) the item masters that a loaded state list would create.
 * @returns {{state, rows, inserts, updates, samples}}
 */
export function analyzeItems(db, { company, year, state } = {}) {
  const st = String(state || '').toUpperCase();
  const rows = listStateMaster(db, { company, state: st });
  const entries = buildItemEntries(db, { company, rows });
  let inserts = 0;
  let updates = 0;
  for (const e of entries) {
    const exists = get(db, 'SELECT 1 AS x FROM itemmst WHERE companycode=? AND itemcode=?', [company, e.itemcode]);
    if (exists) updates++; else inserts++;
  }
  return {
    state: st, rows: rows.length, inserts, updates,
    samples: entries.slice(0, 10).map((e) => ({ itemcode: e.itemcode, itemname: e.itemname, ml: e.ml, salesrate: e.salesrate, purchaseRate: e.purchaseRate })),
  };
}

/**
 * Turn each state-list row into an itemmst values object. Category / KFL /
 * pack-size / manufacturer masters are resolved (and created on apply) here.
 */
function buildItemEntries(db, { company, rows }) {
  const entries = [];
  const usedCodes = new Set();
  for (const r of rows) {
    const ml = r.ml != null ? Number(r.ml) : null;
    // Reuse an existing item for the same name+ml, else mint a fresh code.
    const existing = get(db,
      'SELECT itemcode FROM itemmst WHERE companycode=? AND itemname=? AND ml=?',
      [company, `${r.brandname} ${ml}ml`, ml]);
    const itemcode = existing?.itemcode || nextItemCode(db, usedCodes);
    usedCodes.add(itemcode);
    entries.push({
      row: r,
      itemcode,
      itemname: `${r.brandname} ${ml}ml`,
      categoryname: r.categoryname,
      kflname: r.kflname,
      strengthname: r.strengthname,
      manufacturer: r.manufacturer,
      ml,
      packing: r.packing,
      hsn: r.hsn,
      gstp: r.gstp,
      salesrate: r.mrp,
      purchaseRate: r.rate,
    });
  }
  return entries;
}

function nextItemCode(db, used) {
  const rows = all(db, "SELECT itemcode FROM itemmst WHERE itemcode LIKE 'I%'");
  let max = 0;
  for (const r of rows) { const n = parseInt(String(r.itemcode).slice(1), 10); if (Number.isFinite(n) && n > max) max = n; }
  let n = max + 1;
  let code = 'I' + String(n).padStart(5, '0');
  while (used.has(code)) { n++; code = 'I' + String(n).padStart(5, '0'); }
  return code;
}

/** Apply: create/update item masters from a loaded state list. */
export function applyItems(db, { company, year, state } = {}) {
  const st = String(state || '').toUpperCase();
  const rows = listStateMaster(db, { company, state: st });
  const entries = buildItemEntries(db, { company, rows });
  let inserts = 0;
  let updates = 0;
  const created = { categories: 0, kfl: 0, packs: 0, manufacturers: 0 };
  tx(db, () => {
    for (const e of entries) {
      const categorycode = ensureCode(db, { table: 'categorymst', codeCol: 'categorycode', nameCol: 'categoryname', name: e.categoryname, company, prefix: 'C' });
      const kflcode = ensureCode(db, { table: 'kflmst', codeCol: 'kflcode', nameCol: 'kflname', name: e.kflname, company, prefix: 'K' });
      const manufcode = e.manufacturer
        ? ensureCode(db, { table: 'ManufacturerMaster', codeCol: 'manufcode', nameCol: 'manufname', name: e.manufacturer, company, prefix: 'M' })
        : null;
      if (e.ml != null && e.packing != null) {
        const pg = get(db, 'SELECT 1 AS x FROM Itemgroupml WHERE companycode=? AND ml=? AND packing=?', [company, e.ml, e.packing]);
        if (!pg) { run(db, 'INSERT INTO Itemgroupml (ml, packing, companycode) VALUES (?,?,?)', [e.ml, e.packing, company]); created.packs++; }
      }
      const values = {
        companycode: company,
        itemcode: e.itemcode,
        itemname: e.itemname,
        categorycode,
        kflcode,
        groupcode: null,
        packing: e.packing || 0,
        strengthname: e.strengthname || null,
        ml: e.ml || 0,
        barcode: null,
        salesrate: e.salesrate != null ? money(e.salesrate) : null,
        purchaseRate: e.purchaseRate != null ? money(e.purchaseRate) : null,
        purchase_discount_amount: 0,
        purchase_salestaxonmrp_amount: 0,
        purchase_taxscheme_amount: 0,
        min_qnty: 0,
        manufcode,
        gsttype: 0,
        cgst: e.gstp ? money(e.gstp / 2) : null,
        sgst: e.gstp ? money(e.gstp / 2) : null,
        igst: 0, cess: 0, addcess: 0,
      };
      const exists = get(db, 'SELECT 1 AS x FROM itemmst WHERE companycode=? AND itemcode=?', [company, e.itemcode]);
      upsertItem(db, company, values);
      if (exists) updates++; else inserts++;
    }
  });
  return { state: st, rows: rows.length, inserts, updates, created };
}

export { ensureCode };

// ---------------------------------------------------------------- pack sizes

/** Government pack-size master rows (Quart / Pint / Nip / ...). */
export function listPackSizes(db, company) {
  return all(db, 'SELECT * FROM tbl_PackSize WHERE companycode IS NULL OR companycode=? ORDER BY sortorder, ml', [company]);
}

export function createPackSize(db, company, { packname, ml, abbr = null, sortorder = 0 } = {}) {
  if (!packname || ml == null) throw new Error('packname and ml are required');
  run(db, 'INSERT INTO tbl_PackSize (companycode, packname, ml, abbr, sortorder, isactive) VALUES (?,?,?,?,?,1)',
    [company, packname, Number(ml), abbr, Number(sortorder) || 0]);
  return get(db, 'SELECT * FROM tbl_PackSize WHERE id=last_insert_rowid()');
}

export function deletePackSize(db, company, id) {
  run(db, 'DELETE FROM tbl_PackSize WHERE id=? AND companycode=?', [Number(id), company]);
  return { deleted: true };
}

/** Name the pack size for a millilitre figure (Quart / Pint / Nip / ...). */
export function packNameForMl(db, company, ml) {
  const row = get(db,
    'SELECT packname FROM tbl_PackSize WHERE ml=? AND (companycode IS NULL OR companycode=?) ORDER BY sortorder LIMIT 1',
    [Number(ml), company]);
  return row?.packname || null;
}
