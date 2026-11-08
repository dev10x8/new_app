import { all, get, run } from '../db/index.js';

/**
 * Generic master-data registry. Each entry describes the table, its primary
 * key columns and the editable columns. This mirrors the many "Master" forms
 * in the legacy app (company, category, group, item, ledger, store, ...).
 */
export const MASTERS = {
  company: { table: 'companymast', pk: ['companycode'], label: 'companyname' },
  category: { table: 'categorymst', pk: ['companycode', 'categorycode'], label: 'categoryname' },
  group: { table: 'groupmst', pk: ['companycode', 'groupcode'], label: 'groupname' },
  kfl: { table: 'kflmst', pk: ['companycode', 'kflcode'], label: 'kflname' },
  strength: { table: 'strength', pk: ['companycode', 'strengthname'], label: 'strengthname' },
  itemgroupml: { table: 'Itemgroupml', pk: ['companycode', 'packing', 'ml'], label: 'ml' },
  storage: { table: 'storage', pk: ['companycode', 'shopcode'], label: 'shopname' },
  ledger: { table: 'ledger', pk: ['companycode', 'ledcode'], label: 'name' },
  acgroup: { table: 'acgroupmst', pk: ['acgroupcode'], label: 'acgroupname' },
  acount: { table: 'acountname', pk: ['companycode', 'accode'], label: 'acname' },
  item: { table: 'itemmst', pk: ['companycode', 'itemcode'], label: 'itemname' },
  itemrate: { table: 'itemratemst', pk: ['companycode', 'itemcode', 'ratecode'], label: 'salesrate' },
  rateinfo: { table: 'itemrateinfo', pk: ['companycode', 'ratecode'], label: 'ratename' },
  year: { table: 'yearmst', pk: ['companycode', 'yearcode'], label: 'yearrange' },
  counter: { table: 'countermst', pk: ['companycode', 'countercode'], label: 'countername' },
  customer: { table: 'customerDetails', pk: ['companycode', 'custcode'], label: 'custname' },
  area: { table: 'tblArea', pk: ['companycode', 'areacode'], label: 'areaname' },
  manufacturer: { table: 'ManufacturerMaster', pk: ['companycode', 'manufcode'], label: 'manufname' },
  naiunit: { table: 'naiUnitMaster', pk: ['companycode', 'unitcode'], label: 'unitname' },
  naigroup: { table: 'naiItemMainGroup', pk: ['companycode', 'groupcode'], label: 'groupname' },
  naisubgroup: { table: 'naiItemSubGroup', pk: ['companycode', 'groupcode', 'subgroupcode'], label: 'subgroupname' },
  naiitem: { table: 'naiItemMaster', pk: ['companycode', 'itemcode'], label: 'itemname' },
  scheme: { table: 'tbl_scheme', pk: ['companycode', 'schemecode'], label: 'schemename' },
  schemedetail: { table: 'tbl_schemedetail', pk: ['companycode', 'schemecode', 'itemcode'], label: 'itemcode' },
  offer: { table: 'offermst', pk: ['companycode', 'offercode'], label: 'offername' },
  taxscheme: { table: 'taxdetail', pk: ['companycode', 'SchemeCode', 'TaxCode'], label: 'schemename' },
  exciseitem: { table: 'ExciseItemMaster', pk: ['companycode', 'itemcode'], label: 'excisename' },
  excisemap: { table: 'ExciseItemMapItemMaster', pk: ['companycode', 'itemcode', 'exciseitemcode'], label: 'exciseitemcode' },
  reporttagmain: { table: 'reportTaggingMain', pk: ['companycode', 'reportName'], label: 'reportName' },
  reporttagdetail: { table: 'reportTaggingDetail', pk: ['companycode', 'reportName'], label: 'reportName' },
};

function def(name) {
  const d = MASTERS[name];
  if (!d) throw new Error(`Unknown master: ${name}`);
  return d;
}

export function listMaster(db, name) {
  const d = def(name);
  return all(db, `SELECT * FROM [${d.table}] ORDER BY [${d.pk[d.pk.length - 1]}]`);
}

export function getMaster(db, name, keys) {
  const d = def(name);
  const where = d.pk.map((c) => `[${c}] = ?`).join(' AND ');
  return get(db, `SELECT * FROM [${d.table}] WHERE ${where}`, d.pk.map((c) => keys[c]));
}

export function createMaster(db, name, values) {
  const d = def(name);
  const cols = Object.keys(values);
  if (cols.length === 0) throw new Error('No values supplied');
  const sql = `INSERT INTO [${d.table}] (${cols.map((c) => `[${c}]`).join(',')}) VALUES (${cols.map(() => '?').join(',')})`;
  run(db, sql, cols.map((c) => values[c]));
  return values;
}

export function updateMaster(db, name, keys, values) {
  const d = def(name);
  const setCols = Object.keys(values);
  if (setCols.length === 0) throw new Error('No values supplied');
  const where = d.pk.map((c) => `[${c}] = ?`).join(' AND ');
  const sql = `UPDATE [${d.table}] SET ${setCols.map((c) => `[${c}] = ?`).join(', ')} WHERE ${where}`;
  run(db, sql, [...setCols.map((c) => values[c]), ...d.pk.map((c) => keys[c])]);
  return getMaster(db, name, keys);
}

export function deleteMaster(db, name, keys) {
  const d = def(name);
  const where = d.pk.map((c) => `[${c}] = ?`).join(' AND ');
  run(db, `DELETE FROM [${d.table}] WHERE ${where}`, d.pk.map((c) => keys[c]));
  return { deleted: true };
}

/** Items with resolved category/group/kfl names, for lookup screens. */
export function listItemsDetailed(db, company) {
  return all(
    db,
    `SELECT i.*, c.categoryname, g.groupname, k.kflname
       FROM itemmst i
       LEFT JOIN categorymst c ON c.companycode=i.companycode AND c.categorycode=i.categorycode
       LEFT JOIN groupmst    g ON g.companycode=i.companycode AND g.groupcode=i.groupcode
       LEFT JOIN kflmst      k ON k.companycode=i.companycode AND k.kflcode=i.kflcode
      WHERE i.companycode = ?
      ORDER BY i.itemcode`,
    [company],
  );
}

/** Resolve an item by barcode or code (POS scan / search). */
export function findItem(db, company, term) {
  return get(
    db,
    `SELECT * FROM itemmst
      WHERE companycode = ? AND (itemcode = ? OR barcode = ?)
      LIMIT 1`,
    [company, term, term],
  ) || get(
    db,
    `SELECT * FROM itemmst
      WHERE companycode = ? AND (itemname LIKE ? OR itemcode LIKE ?)
      LIMIT 1`,
    [company, `%${term}%`, `%${term}%`],
  );
}
