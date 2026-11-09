import { all, get, run } from '../db/index.js';
import { money, now } from './helpers.js';
import { stockOnHand } from './stock.js';
import { createMaster } from './masters.js';

/**
 * Inventory control: an at-a-glance dashboard (total items, low-stock counts,
 * stock value), low-stock alerting against a per-item reorder level, and draft
 * purchase requisitions for the items that need restocking.
 *
 * Stock itself is never materialised here - `stockOnHand` remains the single
 * source of truth; this module aggregates it and compares the result with the
 * item's reorder level (itemmst.min_qnty, or the company parameter
 * `DefaultMinQty` when the item does not set one).
 */

const DEFAULT_MIN = 10;

/** Reorder level for an item: itemmst.min_qnty, else the company default. */
function defaultMinQty(db, company) {
  const row = get(db, 'SELECT * FROM companyparamtr WHERE companycode=?', [company]);
  const n = row ? Number(row.defminqty) : NaN;
  return Number.isFinite(n) && n > 0 ? n : DEFAULT_MIN;
}

/** Aggregate on-hand stock per item across every store, with reorder status. */
export function listItemStock(db, { company, year } = {}) {
  const items = all(db,
    `SELECT i.itemcode, i.itemname, i.categorycode, c.categoryname, i.ml, i.packing,
            i.salesrate, i.purchaseRate, COALESCE(i.min_qnty, ?) AS minlevel
       FROM itemmst i
       LEFT JOIN categorymst c ON c.companycode=i.companycode AND c.categorycode=i.categorycode
      WHERE i.companycode=?
      ORDER BY i.itemcode`,
    [defaultMinQty(db, company), company]);

  const byItem = new Map();
  for (const row of stockOnHand(db, { company, year })) {
    byItem.set(row.itemcode, (byItem.get(row.itemcode) || 0) + Number(row.qty || 0));
  }

  return items.map((it) => {
    const qty = byItem.get(it.itemcode) || 0;
    const minlevel = Number(it.minlevel) || 0;
    const status = qty <= 0 ? 'OUT' : (minlevel > 0 && qty < minlevel) ? 'LOW' : 'OK';
    return {
      ...it, qty, minlevel, status,
      value: money(qty * Number(it.salesrate || 0)),
      reorderqty: status === 'OK' ? 0 : Math.max(0, minlevel - qty) || minlevel,
    };
  });
}

/** Headline inventory numbers for the dashboard. */
export function inventorySummary(db, { company, year } = {}) {
  const items = listItemStock(db, { company, year });
  const low = items.filter((i) => i.status === 'LOW');
  const out = items.filter((i) => i.status === 'OUT');
  const stores = all(db, 'SELECT shopcode, shopname FROM storage WHERE companycode=? ORDER BY shopcode', [company]);
  const openReqs = get(db,
    "SELECT COUNT(*) AS n FROM tbl_Requisition WHERE companycode=? AND status='DRAFT'", [company])?.n || 0;
  return {
    totalItems: items.length,
    lowStock: low.length,
    outOfStock: out.length,
    inStock: items.length - low.length - out.length,
    stockValue: money(items.reduce((s, i) => s + i.value, 0)),
    stockUnits: items.reduce((s, i) => s + i.qty, 0),
    stores: stores.length,
    openRequisitions: openReqs,
    lowStockItems: low.slice(0, 50),
  };
}

/** Items at or below their reorder level (the "low stock alert" list). */
export function lowStock(db, { company, year } = {}) {
  return listItemStock(db, { company, year })
    .filter((i) => i.status !== 'OK')
    .sort((a, b) => (a.qty - b.qty) || a.itemcode.localeCompare(b.itemcode));
}

/** Add a stock item (delegates to the generic master CRUD for full columns). */
export function addItem(db, company, values) {
  const item = { companycode: company, ...values };
  createMaster(db, 'item', item);
  return get(db, 'SELECT * FROM itemmst WHERE companycode=? AND itemcode=?', [company, item.itemcode]);
}

// ------------------------------------------------------------- requisitions

/** Draft purchase requisitions raised for the low-stock items. */
export function requisitions(db, company, { status } = {}) {
  const params = [company];
  let sql = 'SELECT * FROM tbl_Requisition WHERE companycode=?';
  if (status) { sql += ' AND status=?'; params.push(status); }
  sql += ' ORDER BY id DESC';
  return all(db, sql, params);
}

export function requisitionSummary(db, company) {
  const rows = requisitions(db, company);
  const by = { DRAFT: 0, ORDERED: 0, CANCELLED: 0 };
  for (const r of rows) by[r.status] = (by[r.status] || 0) + 1;
  return { total: rows.length, ...by };
}

/**
 * Auto-requisition: create a DRAFT purchase requisition for every low-stock item
 * that does not already have an open (DRAFT) one. Idempotent - re-running only
 * adds rows for newly-low items.
 */
export function generateRequisitions(db, { company, year, suppliercode = null } = {}) {
  const low = lowStock(db, { company, year });
  const open = new Set(all(db,
    "SELECT itemcode FROM tbl_Requisition WHERE companycode=? AND status='DRAFT'", [company])
    .map((r) => r.itemcode));
  const at = now();
  let created = 0;
  for (const item of low) {
    if (open.has(item.itemcode)) continue;
    run(db,
      `INSERT INTO tbl_Requisition
         (companycode, yearcode, itemcode, itemname, currentqty, minlevel, reorderqty, rate, suppliercode, status, createdat, updatedat)
       VALUES (?,?,?,?,?,?,?,?,?, 'DRAFT', ?, ?)`,
      [company, year, item.itemcode, item.itemname, item.qty, item.minlevel, item.reorderqty,
       money(Number(item.purchaseRate || item.salesrate || 0)), suppliercode, at, at]);
    created += 1;
  }
  return { created, skipped: low.length - created, low: low.length };
}

export function updateRequisition(db, company, id, { status, reorderqty } = {}) {
  const cur = get(db, 'SELECT * FROM tbl_Requisition WHERE companycode=? AND id=?', [company, id]);
  if (!cur) throw Object.assign(new Error(`No requisition ${id}`), { status: 404 });
  const next = status || cur.status;
  if (!['DRAFT', 'ORDERED', 'CANCELLED'].includes(next)) throw new Error(`Bad requisition status "${next}"`);
  run(db, 'UPDATE tbl_Requisition SET status=?, reorderqty=?, updatedat=? WHERE companycode=? AND id=?',
    [next, reorderqty != null ? reorderqty : cur.reorderqty, now(), company, id]);
  return get(db, 'SELECT * FROM tbl_Requisition WHERE companycode=? AND id=?', [company, id]);
}
