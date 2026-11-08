import { all, get } from '../db/index.js';
import { money } from './helpers.js';
import { stockOnHand } from './stock.js';
import { ledgerBalances, trialBalance } from './accounting.js';

/**
 * Report catalogue. Keys mirror the groups in PRODUCT_FEATURES.md; each report
 * returns rows so the TUI can render it generically.
 */
export const REPORTS = {
  'sales.bill': 'Bill Statement',
  'sales.item': 'Item Wise Sales',
  'sales.category': 'Category Wise Sales',
  'sales.daily': 'Sales Date Wise Statement',
  'sales.return': 'Sales Return',
  'purchase.bill': 'Purchase Bill Details',
  'purchase.item': 'Purchase Item Summary',
  'stock.closing': 'Items Wise Closing Stock',
  'accounts.ledger': 'General Ledger',
  'accounts.trial': 'Trial Balance',
  'accounts.daybook': 'Day Book',
  'mis.profit': 'Item Wise Profit-Loss',
};

export function runReport(db, key, opts) {
  const { company, year } = opts;
  switch (key) {
    case 'sales.bill': return salesBillStatement(db, company, year, opts);
    case 'sales.item': return salesItemWise(db, company, year, opts);
    case 'sales.category': return salesCategoryWise(db, company, year, opts);
    case 'sales.daily': return salesDaily(db, company, year, opts);
    case 'sales.return': return salesReturns(db, company, year, opts);
    case 'purchase.bill': return purchaseBill(db, company, year, opts);
    case 'purchase.item': return purchaseItemWise(db, company, year, opts);
    case 'stock.closing': return stockClosing(db, company, year, opts);
    case 'accounts.ledger': return ledgerBalances(db, company, year);
    case 'accounts.trial': return trialBalance(db, company, year).rows;
    case 'accounts.daybook': return dayBookAll(db, company, year, opts);
    case 'mis.profit': return itemProfit(db, company, year, opts);
    default: throw new Error(`Unknown report: ${key}`);
  }
}

function range(opts, col = 'trndate') {
  const params = [];
  let sql = '';
  if (opts.from) { sql += ` AND ${col} >= ?`; params.push(opts.from); }
  if (opts.to) { sql += ` AND ${col} <= ?`; params.push(opts.to); }
  return { sql, params };
}

function salesBillStatement(db, company, year, opts) {
  const r = range(opts);
  return all(db,
    `SELECT trnno, trndate, salestype, storecode, ledgercode, amount, discamount, netamount
       FROM salesbillmain
      WHERE companycode=? AND yearcode=? ${r.sql}
      ORDER BY trndate DESC, trnno DESC`, [company, year, ...r.params]);
}

function salesItemWise(db, company, year, opts) {
  const r = range(opts, 'sm.trndate');
  return all(db,
    `SELECT sd.itemcode, i.itemname,
            SUM(sd.qnty) AS qty, SUM(sd.itemAmount) AS amount
       FROM salesbillmain sm
       JOIN salesbilldetail sd ON sd.companycode=sm.companycode AND sd.yearcode=sm.yearcode AND sd.trnno=sm.trnno
       LEFT JOIN itemmst i ON i.companycode=sd.companycode AND i.itemcode=sd.itemcode
      WHERE sm.companycode=? AND sm.yearcode=? ${r.sql}
      GROUP BY sd.itemcode, i.itemname
      ORDER BY amount DESC`, [company, year, ...r.params]);
}

function salesCategoryWise(db, company, year, opts) {
  const r = range(opts, 'sm.trndate');
  return all(db,
    `SELECT i.categorycode, c.categoryname, SUM(sd.qnty) AS qty, SUM(sd.itemAmount) AS amount
       FROM salesbillmain sm
       JOIN salesbilldetail sd ON sd.companycode=sm.companycode AND sd.yearcode=sm.yearcode AND sd.trnno=sm.trnno
       JOIN itemmst i ON i.companycode=sd.companycode AND i.itemcode=sd.itemcode
       LEFT JOIN categorymst c ON c.companycode=i.companycode AND c.categorycode=i.categorycode
      WHERE sm.companycode=? AND sm.yearcode=? ${r.sql}
      GROUP BY i.categorycode, c.categoryname
      ORDER BY amount DESC`, [company, year, ...r.params]);
}

function salesDaily(db, company, year, opts) {
  const r = range(opts);
  return all(db,
    `SELECT trndate, COUNT(*) AS bills, SUM(amount) AS gross, SUM(discamount) AS discount, SUM(netamount) AS net
       FROM salesbillmain
      WHERE companycode=? AND yearcode=? ${r.sql}
      GROUP BY trndate ORDER BY trndate`, [company, year, ...r.params]);
}

function salesReturns(db, company, year, opts) {
  const r = range(opts);
  return all(db,
    `SELECT trnno, trndate, ledgercode, amount, netamount
       FROM salesbillmain
      WHERE companycode=? AND yearcode=? AND salestype='RETURN' ${r.sql}
      ORDER BY trndate DESC`, [company, year, ...r.params]);
}

function purchaseBill(db, company, year, opts) {
  const r = range(opts);
  return all(db,
    `SELECT trnno, trndate, ptype, suppliercode, docno, totamount, tottaxOth, totnetamt
       FROM purchasemain
      WHERE companycode=? AND yearcode=? ${r.sql}
      ORDER BY trndate DESC, trnno DESC`, [company, year, ...r.params]);
}

function purchaseItemWise(db, company, year, opts) {
  const r = range(opts, 'pm.trndate');
  return all(db,
    `SELECT pd.itemcode, i.itemname, SUM(pd.itemquantity) AS qty, SUM(pd.itemamount) AS amount
       FROM purchasemain pm
       JOIN purchasedetail pd ON pd.companycode=pm.companycode AND pd.yearcode=pm.yearcode AND pd.trnno=pm.trnno
       LEFT JOIN itemmst i ON i.companycode=pd.companycode AND i.itemcode=pd.itemcode
      WHERE pm.companycode=? AND pm.yearcode=? ${r.sql}
      GROUP BY pd.itemcode, i.itemname
      ORDER BY amount DESC`, [company, year, ...r.params]);
}

function stockClosing(db, company, year, opts) {
  return stockOnHand(db, { company, year, storecode: opts.storecode });
}

function dayBookAll(db, company, year, opts) {
  const r = range(opts);
  return all(db,
    `SELECT trndate, trnno, type, ledcode, narr, debit, credit
       FROM journaldetail
      WHERE companycode=? AND yearcode=? ${r.sql}
      ORDER BY trndate, trnno, slno`, [company, year, ...r.params]);
}

function itemProfit(db, company, year, opts) {
  const r = range(opts, 'sm.trndate');
  const sales = all(db,
    `SELECT sd.itemcode, i.itemname, i.purchaseRate,
            SUM(sd.qnty) AS qty, SUM(sd.itemAmount) AS salesamount
       FROM salesbillmain sm
       JOIN salesbilldetail sd ON sd.companycode=sm.companycode AND sd.yearcode=sm.yearcode AND sd.trnno=sm.trnno
       LEFT JOIN itemmst i ON i.companycode=sd.companycode AND i.itemcode=sd.itemcode
      WHERE sm.companycode=? AND sm.yearcode=? ${r.sql}
      GROUP BY sd.itemcode, i.itemname, i.purchaseRate`, [company, year, ...r.params]);
  return sales.map((s) => {
    const cost = money((s.purchaseRate || 0) * s.qty);
    return { ...s, cost, profit: money(s.salesamount - cost) };
  });
}

/** Dashboard figures (Sales / Purchase / Stock status). */
export function dashboard(db, company, year) {
  const sales = get(db,
    `SELECT COUNT(*) AS bills, COALESCE(SUM(netamount),0) AS net FROM salesbillmain WHERE companycode=? AND yearcode=?`,
    [company, year]);
  const purchase = get(db,
    `SELECT COUNT(*) AS bills, COALESCE(SUM(totnetamt),0) AS net FROM purchasemain WHERE companycode=? AND yearcode=?`,
    [company, year]);
  const items = get(db, 'SELECT COUNT(*) AS n FROM itemmst WHERE companycode=?', [company]);
  const stock = stockOnHand(db, { company, year });
  const stockValue = money(stock.reduce((s, r) => s + (r.value || 0), 0));
  return { sales, purchase, items: items.n, stockValue, stockLines: stock.length };
}
