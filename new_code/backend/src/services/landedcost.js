import { all, get, run } from '../db/index.js';
import { money, now } from './helpers.js';

/**
 * Landing cost and margin.
 *
 * The invoice rate is not the true cost of a bottle: trade discounts, other
 * charges on the bill and free (scheme) bottles all move the real cost per
 * bottle. Landing (landed) cost folds those into a single number, and margin is
 * the difference between the selling price and that number.
 *
 *   landing cost / bottle = ( rate*qty - discount + other charges ) / (qty + free)
 *   margin / bottle       = selling price - landing cost
 *   margin %              = margin / selling price * 100
 *
 * Money columns in this rebuild are a mix of REAL and TEXT, so every figure is
 * read through `num()` (a tolerant numeric cast) before use.
 */

/** Tolerant numeric read: handles REAL, TEXT-encoded money and blanks. */
export function num(v) {
  if (v === null || v === undefined || v === '') return 0;
  const n = Number(v);
  return Number.isFinite(n) ? n : 0;
}

/**
 * Compute landing cost and margin for one purchase line.
 *
 * @param {object} line  { itemrate, itemdiscount, itemfreeqnty, itemquantity, itemmrp }
 * @param {object} [opts]
 * @param {number} [opts.otherCharges]   apportioned share of bill-level charges
 * @param {number} [opts.taxAmount]      tax on the line (tracked, not capitalised)
 * @param {number} [opts.salesrate]      selling price to compare against
 * @param {number} [opts.markupPct]      if set and no salesrate, derive salesrate
 * @returns {{qty,freeqty,grossrate,discount,othercharges,taxamount,landingcost,salesrate,marginamt,marginpct}}
 */
export function computeLine(line, opts = {}) {
  const qty = num(line.itemquantity);
  const freeqty = num(line.itemfreeqnty);
  const grossrate = num(line.itemrate);
  const discount = num(line.itemdiscount);
  const othercharges = num(opts.otherCharges);
  const taxamount = num(opts.taxAmount);

  const net = grossrate * qty - discount + othercharges;
  const totalQty = qty + freeqty;
  const landingcost = totalQty > 0 ? money(net / totalQty) : money(grossrate);

  let salesrate = num(opts.salesrate);
  if (!salesrate && opts.markupPct) salesrate = money(landingcost * (1 + num(opts.markupPct) / 100));

  const marginamt = money(salesrate - landingcost);
  const marginpct = salesrate > 0 ? money((marginamt / salesrate) * 100) : 0;

  return { qty, freeqty, grossrate, discount, othercharges, taxamount, landingcost, salesrate, marginamt, marginpct };
}

/**
 * Compute landing cost + margin for every line of a purchase, persist the
 * results in tbl_LandedCost and return them. Bill-level charges
 * (purchasemain.tottaxOth) are apportioned across lines by value.
 *
 * @param {object} p { company, year, trnno, otherCharges? }
 */
export function computePurchase(db, { company, year, trnno, otherCharges } = {}) {
  const header = get(db, 'SELECT * FROM purchasemain WHERE companycode=? AND yearcode=? AND trnno=?', [company, year, trnno]);
  if (!header) throw Object.assign(new Error(`No purchase ${trnno}`), { status: 404 });
  const lines = all(db, 'SELECT * FROM purchasedetail WHERE companycode=? AND yearcode=? AND trnno=? ORDER BY slno', [company, year, trnno]);

  const billOther = otherCharges != null ? num(otherCharges) : num(header.tottaxOth);
  const lineValue = (l) => num(l.itemrate) * num(l.itemquantity);
  const totalValue = lines.reduce((s, l) => s + lineValue(l), 0) || 1;

  const at = now();
  const out = [];
  run(db, 'DELETE FROM tbl_LandedCost WHERE companycode=? AND yearcode=? AND trnno=?', [company, year, trnno]);
  for (const l of lines) {
    const share = money((lineValue(l) / totalValue) * billOther);
    const item = get(db, 'SELECT salesrate FROM itemmst WHERE companycode=? AND itemcode=?', [company, l.itemcode]) || {};
    const c = computeLine(l, { otherCharges: share, salesrate: num(item.salesrate), taxAmount: num(l.taxable) * 0 });
    run(
      db,
      `INSERT INTO tbl_LandedCost
         (companycode, yearcode, trnno, itemcode, batchno, qty, freeqty, grossrate, discount,
          othercharges, taxamount, landingcost, salesrate, marginamt, marginpct, computedat)
       VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)`,
      [company, year, trnno, l.itemcode, l.batchno || null, c.qty, c.freeqty, c.grossrate, c.discount,
       c.othercharges, c.taxamount, c.landingcost, c.salesrate, c.marginamt, c.marginpct, at],
    );
    out.push({ ...c, itemcode: l.itemcode, itemname: item?.itemname, batchno: l.batchno });
  }
  return { trnno, lines: out };
}

/**
 * Set the selling price of an item from a target margin over its landing cost.
 * Writes itemmst.salesrate (and itemratemst for the item's rate codes) so the
 * POS picks the new price up.
 */
export function applyMarginToItem(db, { company, itemcode, landingcost, marginPct = null, salesrate = null }) {
  const item = get(db, 'SELECT * FROM itemmst WHERE companycode=? AND itemcode=?', [company, itemcode]);
  if (!item) throw Object.assign(new Error(`No item ${itemcode}`), { status: 404 });
  const price = salesrate != null
    ? money(salesrate)
    : money(num(landingcost) * (1 + num(marginPct) / 100));
  run(db, 'UPDATE itemmst SET salesrate=? WHERE companycode=? AND itemcode=?', [price, company, itemcode]);
  run(db, 'UPDATE itemratemst SET salesrate=? WHERE companycode=? AND itemcode=?', [price, company, itemcode]);
  return { itemcode, salesrate: price, marginamt: money(price - num(landingcost)) };
}

/**
 * Landing-cost / margin report across purchases in a date range, joined to the
 * item master. Latest computed row per (trnno, itemcode) is used.
 */
export function report(db, { company, year, from, to } = {}) {
  const params = [company];
  let where = 'lc.companycode=?';
  if (year) { where += ' AND lc.yearcode=?'; params.push(year); }
  if (from) { where += ' AND pm.trndate>=?'; params.push(from); }
  if (to) { where += ' AND pm.trndate<=?'; params.push(to); }
  return all(db,
    `SELECT lc.trnno, pm.trndate, lc.itemcode, i.itemname, lc.batchno, lc.qty, lc.freeqty,
            lc.grossrate, lc.discount, lc.othercharges, lc.landingcost, lc.salesrate,
            lc.marginamt, lc.marginpct
       FROM tbl_LandedCost lc
       LEFT JOIN purchasemain pm
         ON pm.companycode=lc.companycode AND pm.yearcode=lc.yearcode AND pm.trnno=lc.trnno
       LEFT JOIN itemmst i ON i.companycode=lc.companycode AND i.itemcode=lc.itemcode
      WHERE ${where}
      ORDER BY pm.trndate DESC, lc.trnno DESC, lc.itemcode`, params);
}

/** Item-wise summary: average landing cost, margin and margin % per item. */
export function itemSummary(db, { company, year } = {}) {
  const params = [company];
  let where = 'lc.companycode=?';
  if (year) { where += ' AND lc.yearcode=?'; params.push(year); }
  return all(db,
    `SELECT lc.itemcode, i.itemname,
            COUNT(*) AS lines,
            SUM(lc.qty) AS qty,
            ROUND(AVG(lc.landingcost),2) AS avgexcost,
            ROUND(AVG(lc.salesrate),2) AS avgsale,
            ROUND(AVG(lc.marginamt),2) AS avgmargin,
            ROUND(AVG(lc.marginpct),2) AS avgmarginpct
       FROM tbl_LandedCost lc
       LEFT JOIN itemmst i ON i.companycode=lc.companycode AND i.itemcode=lc.itemcode
      WHERE ${where}
      GROUP BY lc.itemcode, i.itemname
      ORDER BY avgmarginpct DESC`, params);
}
