import { all } from '../db/index.js';
import { stockOnHand } from './stock.js';
import { ledgerBalances, trialBalance } from './accounting.js';
import * as gst from './gst.js';
import * as m22 from './m22.js';
import * as landedcost from './landedcost.js';

// Shared joins for sales-detail reports.
const SALES_FROM = `
  FROM salesbillmain sm
  JOIN salesbilldetail sd
    ON sd.companycode=sm.companycode AND sd.yearcode=sm.yearcode AND sd.trnno=sm.trnno
  LEFT JOIN itemmst i ON i.companycode=sd.companycode AND i.itemcode=sd.itemcode
  LEFT JOIN categorymst c ON c.companycode=i.companycode AND c.categorycode=i.categorycode
  LEFT JOIN kflmst k ON k.companycode=i.companycode AND k.kflcode=i.kflcode
  LEFT JOIN groupmst g ON g.companycode=i.companycode AND g.groupcode=i.groupcode
  LEFT JOIN ManufacturerMaster mf ON mf.companycode=i.companycode AND mf.manufcode=i.manufcode
  LEFT JOIN ledger l ON l.companycode=sm.companycode AND l.ledcode=sm.ledgercode
  LEFT JOIN customerDetails cu ON cu.companycode=sm.companycode AND cu.custcode=sm.ledgercode
  LEFT JOIN storage st ON st.companycode=sm.companycode AND st.shopcode=sm.storecode
  WHERE sm.companycode=? AND sm.yearcode=? AND sm.salestype <> 'RETURN'`;

const PURCHASE_FROM = `
  FROM purchasemain pm
  JOIN purchasedetail pd
    ON pd.companycode=pm.companycode AND pd.yearcode=pm.yearcode AND pd.trnno=pm.trnno
  LEFT JOIN itemmst i ON i.companycode=pd.companycode AND i.itemcode=pd.itemcode
  LEFT JOIN categorymst c ON c.companycode=i.companycode AND c.categorycode=i.categorycode
  LEFT JOIN kflmst k ON k.companycode=i.companycode AND k.kflcode=i.kflcode
  LEFT JOIN ManufacturerMaster mf ON mf.companycode=i.companycode AND mf.manufcode=i.manufcode
  LEFT JOIN ledger l ON l.companycode=pm.companycode AND l.ledcode=pm.suppliercode
  WHERE pm.companycode=? AND pm.yearcode=? AND pm.ptype='PURCHASE'`;

const DIM = {
  item: { sel: 'sd.itemcode AS code, i.itemname AS name', grp: 'sd.itemcode, i.itemname' },
  category: { sel: 'i.categorycode AS code, c.categoryname AS name', grp: 'i.categorycode, c.categoryname' },
  kfl: { sel: 'i.kflcode AS code, k.kflname AS name', grp: 'i.kflcode, k.kflname' },
  group: { sel: 'i.groupcode AS code, g.groupname AS name', grp: 'i.groupcode, g.groupname' },
  manufacturer: { sel: 'i.manufcode AS code, mf.manufname AS name', grp: 'i.manufcode, mf.manufname' },
  party: { sel: 'sm.ledgercode AS code, l.name AS name', grp: 'sm.ledgercode, l.name' },
  customer: { sel: 'cu.custcode AS code, cu.custname AS name', grp: 'cu.custcode, cu.custname' },
  area: { sel: 'cu.areacode AS code, cu.areacode AS name', grp: 'cu.areacode' },
  store: { sel: 'sm.storecode AS code, st.shopname AS name', grp: 'sm.storecode, st.shopname' },
  date: { sel: 'sm.trndate AS code, sm.trndate AS name', grp: 'sm.trndate' },
  ml: { sel: 'i.ml AS code, i.ml AS name', grp: 'i.ml' },
  category_ml: { sel: "i.categorycode || '/' || i.ml AS code, c.categoryname || ' / ' || i.ml AS name", grp: 'i.categorycode, c.categoryname, i.ml' },
  kfl_category_ml: { sel: "i.kflcode || '/' || i.categorycode || '/' || i.ml AS code, k.kflname || ' / ' || c.categoryname || ' / ' || i.ml AS name", grp: 'i.kflcode, k.kflname, i.categorycode, c.categoryname, i.ml' },
  category_brand: { sel: "i.categorycode || '/' || i.kflcode AS code, c.categoryname || ' / ' || k.kflname AS name", grp: 'i.categorycode, c.categoryname, i.kflcode, k.kflname' },
  group_item: { sel: 'i.groupcode || \'/\' || sd.itemcode AS code, g.groupname || \' / \' || i.itemname AS name', grp: 'i.groupcode, g.groupname, sd.itemcode, i.itemname' },
  category_item: { sel: 'i.categorycode || \'/\' || sd.itemcode AS code, c.categoryname || \' / \' || i.itemname AS name', grp: 'i.categorycode, c.categoryname, sd.itemcode, i.itemname' },
};

function range(opts, col) {
  const params = [];
  let sql = '';
  if (opts.from) { sql += ` AND ${col} >= ?`; params.push(opts.from); }
  if (opts.to) { sql += ` AND ${col} <= ?`; params.push(opts.to); }
  return { sql, params };
}

function salesDim(db, company, year, dim, opts, { litres = false } = {}) {
  const d = DIM[dim];
  const r = range(opts, 'sm.trndate');
  const extra = litres ? ', SUM(sd.qnty * i.ml / 1000.0) AS litres' : '';
  return all(db,
    `SELECT ${d.sel}, SUM(sd.qnty) AS qty, SUM(sd.itemAmount) AS amount${extra}
     ${SALES_FROM} ${r.sql}
     GROUP BY ${d.grp} ORDER BY amount DESC`,
    [company, year, ...r.params]);
}

function purchaseDim(db, company, year, dim, opts) {
  const map = {
    item: ['pd.itemcode AS code, i.itemname AS name', 'pd.itemcode, i.itemname'],
    category: ['i.categorycode AS code, c.categoryname AS name', 'i.categorycode, c.categoryname'],
    kfl: ['i.kflcode AS code, k.kflname AS name', 'i.kflcode, k.kflname'],
    manufacturer: ['i.manufcode AS code, mf.manufname AS name', 'i.manufcode, mf.manufname'],
    party: ['pm.suppliercode AS code, l.name AS name', 'pm.suppliercode, l.name'],
    date: ['pm.trndate AS code, pm.trndate AS name', 'pm.trndate'],
    brand: ['i.kflcode || \'/\' || pm.suppliercode AS code, k.kflname || \' / \' || l.name AS name', 'i.kflcode, k.kflname, pm.suppliercode, l.name'],
  }[dim];
  const r = range(opts, 'pm.trndate');
  return all(db,
    `SELECT ${map[0]}, SUM(pd.itemquantity) AS qty, SUM(pd.itemamount) AS amount
     ${PURCHASE_FROM} ${r.sql}
     GROUP BY ${map[1]} ORDER BY amount DESC`,
    [company, year, ...r.params]);
}

const S = (title, run) => ({ group: 'Sales', title, run });
const ST = (title, run) => ({ group: 'Stock', title, run });
const P = (title, run) => ({ group: 'Purchase', title, run });
const A = (title, run) => ({ group: 'Accounts', title, run });
const M = (title, run) => ({ group: 'MIS', title, run });
const B = (title, run) => ({ group: 'Breakage', title, run });
const T = (title, run) => ({ group: 'Transfer', title, run });
const E = (title, run) => ({ group: 'Excise', title, run });
const G = (title, run) => ({ group: 'GST', title, run });

export const CATALOG = {
  // ---------------- Sales (32) ----------------
  'sales.in_bulk_litre': S('In Bulk Litre', (db, o) => salesDim(db, o.company, o.year, 'item', o, { litres: true })),
  'sales.statement': S('Sales Statement', (db, o) => all(db,
    `SELECT sm.trnno, sm.trndate, sm.salestype, sm.ledgercode, sm.amount, sm.discamount, sm.netamount
       FROM salesbillmain sm WHERE sm.companycode=? AND sm.yearcode=? ORDER BY sm.trndate DESC`, [o.company, o.year])),
  'sales.date_wise': S('Sales Date Wise Statement', (db, o) => all(db,
    `SELECT trndate, COUNT(*) AS bills, SUM(amount) AS gross, SUM(discamount) AS discount, SUM(netamount) AS net
       FROM salesbillmain WHERE companycode=? AND yearcode=? GROUP BY trndate ORDER BY trndate`, [o.company, o.year])),
  'sales.bill': S('Bill Statement', (db, o) => all(db,
    `SELECT trnno, trndate, salestype, storecode, ledgercode, amount, discamount, netamount
       FROM salesbillmain WHERE companycode=? AND yearcode=? ORDER BY trndate DESC, trnno DESC`, [o.company, o.year])),
  'sales.multi_bill': S('Multi Bill Print', (db, o) => all(db,
    `SELECT sm.trnno, sm.trndate, sd.itemcode, i.itemname, sd.qnty, sd.rate, sd.itemAmount
       FROM salesbillmain sm
       JOIN salesbilldetail sd ON sd.companycode=sm.companycode AND sd.yearcode=sm.yearcode AND sd.trnno=sm.trnno
       LEFT JOIN itemmst i ON i.companycode=sd.companycode AND i.itemcode=sd.itemcode
      WHERE sm.companycode=? AND sm.yearcode=? ORDER BY sm.trnno, sd.itemcode`, [o.company, o.year])),
  'sales.ml_wise': S('ML Wise Sale', (db, o) => salesDim(db, o.company, o.year, 'ml', o, { litres: true })),
  'sales.category_ml': S('Category-ML Wise', (db, o) => salesDim(db, o.company, o.year, 'category_ml', o)),
  'sales.kfl_category_ml': S('KFL-Category-ML', (db, o) => salesDim(db, o.company, o.year, 'kfl_category_ml', o)),
  'sales.item_group': S('Item-Group Wise', (db, o) => salesDim(db, o.company, o.year, 'group_item', o)),
  'sales.category_brand': S('Category-Brand', (db, o) => salesDim(db, o.company, o.year, 'category_brand', o)),
  'sales.item': S('Item Wise Sales', (db, o) => salesDim(db, o.company, o.year, 'item', o)),
  'sales.party': S('Party Wise Sales', (db, o) => salesDim(db, o.company, o.year, 'party', o)),
  'sales.summary': S('Sales Summary', (db, o) => all(db,
    `SELECT COUNT(*) AS bills, SUM(amount) AS gross, SUM(discamount) AS discount, SUM(netamount) AS net
       FROM salesbillmain WHERE companycode=? AND yearcode=?`, [o.company, o.year])),
  'sales.bill_wise': S('Bill Wise Sales', (db, o) => all(db,
    `SELECT trnno, trndate, ledgercode, amount, netamount FROM salesbillmain
      WHERE companycode=? AND yearcode=? ORDER BY netamount DESC`, [o.company, o.year])),
  'sales.amount_wise': S('Amount Wise', (db, o) => all(db,
    `SELECT trnno, trndate, ledgercode, netamount FROM salesbillmain
      WHERE companycode=? AND yearcode=? ORDER BY netamount DESC`, [o.company, o.year])),
  'sales.return': S('Sales Return', (db, o) => all(db,
    `SELECT trnno, trndate, ledgercode, amount, netamount FROM salesbillmain
      WHERE companycode=? AND yearcode=? AND salestype='RETURN' ORDER BY trndate DESC`, [o.company, o.year])),
  'sales.date_category': S('Date-Category Wise', (db, o) => all(db,
    `SELECT sm.trndate, i.categorycode, c.categoryname, SUM(sd.qnty) AS qty, SUM(sd.itemAmount) AS amount
     ${SALES_FROM} GROUP BY sm.trndate, i.categorycode, c.categoryname ORDER BY sm.trndate, amount DESC`, [o.company, o.year])),
  'sales.manufacturer': S('Manufacturer Wise', (db, o) => salesDim(db, o.company, o.year, 'manufacturer', o)),
  'sales.party_weekly': S('Party Weekly Sale', (db, o) => all(db,
    `SELECT sm.ledgercode, l.name, strftime('%Y-%W', sm.trndate) AS week, SUM(sm.netamount) AS net
       FROM salesbillmain sm LEFT JOIN ledger l ON l.companycode=sm.companycode AND l.ledcode=sm.ledgercode
      WHERE sm.companycode=? AND sm.yearcode=? GROUP BY sm.ledgercode, l.name, week ORDER BY week`, [o.company, o.year])),
  'sales.area': S('Area Wise Sales', (db, o) => salesDim(db, o.company, o.year, 'area', o)),
  'sales.discount': S('Discount Report', (db, o) => all(db,
    `SELECT trndate, COUNT(*) AS bills, SUM(discamount) AS discount FROM salesbillmain
      WHERE companycode=? AND yearcode=? GROUP BY trndate ORDER BY trndate`, [o.company, o.year])),
  'sales.offer_discount': S('Offer Discount', (db, o) => all(db,
    `SELECT sd.itemcode, i.itemname, o.offername, o.discountpct,
            SUM(sd.qnty) AS qty, SUM(sd.itemAmount) AS amount,
            SUM(sd.itemAmount) * o.discountpct / 100.0 AS offerdiscount
       FROM salesbillmain sm
       JOIN salesbilldetail sd ON sd.companycode=sm.companycode AND sd.yearcode=sm.yearcode AND sd.trnno=sm.trnno
       JOIN offermst o ON o.companycode=sm.companycode AND o.itemcode=sd.itemcode AND o.active=1
       LEFT JOIN itemmst i ON i.companycode=sd.companycode AND i.itemcode=sd.itemcode
      WHERE sm.companycode=? AND sm.yearcode=? GROUP BY sd.itemcode, i.itemname, o.offername, o.discountpct`, [o.company, o.year])),
  'sales.nai_gst': S('NAI GST Report', (db, o) => gst.naiGstReport(db, o.company, o.year, { from: o.from, to: o.to }).rows),
  'sales.home_delivery': S('Home Delivery Report', (db, o) => all(db,
    `SELECT orderno, trndate, custcode, address, phone, amount, status FROM delivery_orders
      WHERE companycode=? AND yearcode=? ORDER BY trndate DESC`, [o.company, o.year])),
  'sales.category_item': S('Category-Item', (db, o) => salesDim(db, o.company, o.year, 'category_item', o)),
  'sales.parcha': S('Parcha Report', (db, o) => all(db,
    `SELECT sm.trnno, sm.trndate, sd.itemcode, i.itemname, sd.qnty, sd.rate, sd.itemAmount
       FROM salesbillmain sm
       JOIN salesbilldetail sd ON sd.companycode=sm.companycode AND sd.yearcode=sm.yearcode AND sd.trnno=sm.trnno
       LEFT JOIN itemmst i ON i.companycode=sd.companycode AND i.itemcode=sd.itemcode
      WHERE sm.companycode=? AND sm.yearcode=? ORDER BY sm.trndate, sm.trnno`, [o.company, o.year])),
  'sales.deleted': S('Deleted Sale Report', (db, o) => all(db,
    `SELECT trnno, trndate, ledgercode, amount, netamount, deletedon FROM DeleteSalesBillMain
      WHERE companycode=? AND yearcode=? ORDER BY deletedon DESC`, [o.company, o.year])),
  'sales.customer': S('Customer Wise Sales', (db, o) => salesDim(db, o.company, o.year, 'customer', o)),
  'sales.kfl_ml': S('KFL ML Wise Sale Report', (db, o) => all(db,
    `SELECT i.kflcode, k.kflname, i.ml, SUM(sd.qnty) AS qty, SUM(sd.itemAmount) AS amount, SUM(sd.qnty*i.ml/1000.0) AS litres
     ${SALES_FROM} GROUP BY i.kflcode, k.kflname, i.ml ORDER BY i.kflcode, i.ml`, [o.company, o.year])),
  'sales.audit': S('Audit Report', (db, o) => all(db,
    `SELECT id, tablename, action, refkey, auditat, usercode FROM TBL_Audit
      WHERE companycode=? ORDER BY id DESC LIMIT 500`, [o.company])),
  'sales.transfer_shop': S('Transfer Shop', (db, o) => all(db,
    `SELECT trnno, trndate, shopcode_from, shopcode_to, amount FROM transfer_sale_main
      WHERE companycode=? AND yearcode=? ORDER BY trndate DESC`, [o.company, o.year])),
  'sales.transfer_receive': S('Transfer Receive', (db, o) => all(db,
    `SELECT trnno, trndate, shopcode_from, shopcode_to FROM transfer_receive_main
      WHERE companycode=? AND yearcode=? ORDER BY trndate DESC`, [o.company, o.year])),

  // ---------------- Stock (20) ----------------
  'stock.items_closing': ST('Items Wise Closing', (db, o) => stockOnHand(db, { company: o.company, year: o.year, storecode: o.storecode })),
  'stock.value_closing': ST('Value Wise Closing', (db, o) => stockOnHand(db, { company: o.company, year: o.year, storecode: o.storecode }).sort((a, b) => b.value - a.value)),
  'stock.value_detail': ST('Value Wise Detail', (db, o) => all(db,
    `SELECT m.shopcode, m.itemcode, i.itemname, i.salesrate, SUM(m.qnty) AS qty, SUM(m.qnty)*COALESCE(i.salesrate,0) AS value
       FROM openingstockmst m LEFT JOIN itemmst i ON i.companycode=m.companycode AND i.itemcode=m.itemcode
      WHERE m.companycode=? AND m.yearcode=? GROUP BY m.shopcode, m.itemcode, i.itemname, i.salesrate ORDER BY value DESC`, [o.company, o.year])),
  'stock.brand_category': ST('Brand-Category', (db, o) => all(db,
    `SELECT i.kflcode, k.kflname, i.categorycode, c.categoryname, COUNT(*) AS items, SUM(o.qnty) AS qty
       FROM openingstockmst o JOIN itemmst i ON i.companycode=o.companycode AND i.itemcode=o.itemcode
       LEFT JOIN kflmst k ON k.companycode=i.companycode AND k.kflcode=i.kflcode
       LEFT JOIN categorymst c ON c.companycode=i.companycode AND c.categorycode=i.categorycode
      WHERE o.companycode=? AND o.yearcode=? GROUP BY i.kflcode, k.kflname, i.categorycode, c.categoryname`, [o.company, o.year])),
  'stock.brand_ml': ST('Brand-ML Wise', (db, o) => all(db,
    `SELECT i.kflcode, k.kflname, i.ml, SUM(o.qnty) AS qty FROM openingstockmst o
       JOIN itemmst i ON i.companycode=o.companycode AND i.itemcode=o.itemcode
       LEFT JOIN kflmst k ON k.companycode=i.companycode AND k.kflcode=i.kflcode
      WHERE o.companycode=? AND o.yearcode=? GROUP BY i.kflcode, k.kflname, i.ml ORDER BY i.kflcode, i.ml`, [o.company, o.year])),
  'stock.category_brand': ST('Category-Brand Wise', (db, o) => all(db,
    `SELECT i.categorycode, c.categoryname, i.kflcode, k.kflname, SUM(o.qnty) AS qty FROM openingstockmst o
       JOIN itemmst i ON i.companycode=o.companycode AND i.itemcode=o.itemcode
       LEFT JOIN categorymst c ON c.companycode=i.companycode AND c.categorycode=i.categorycode
       LEFT JOIN kflmst k ON k.companycode=i.companycode AND k.kflcode=i.kflcode
      WHERE o.companycode=? AND o.yearcode=? GROUP BY i.categorycode, c.categoryname, i.kflcode, k.kflname`, [o.company, o.year])),
  'stock.category_ml': ST('Category-ML Wise', (db, o) => all(db,
    `SELECT i.categorycode, c.categoryname, i.ml, SUM(o.qnty) AS qty FROM openingstockmst o
       JOIN itemmst i ON i.companycode=o.companycode AND i.itemcode=o.itemcode
       LEFT JOIN categorymst c ON c.companycode=i.companycode AND c.categorycode=i.categorycode
      WHERE o.companycode=? AND o.yearcode=? GROUP BY i.categorycode, c.categoryname, i.ml`, [o.company, o.year])),
  'stock.manufacturer': ST('Manufacturer Wise', (db, o) => all(db,
    `SELECT i.manufcode, mf.manufname, SUM(o.qnty) AS qty FROM openingstockmst o
       JOIN itemmst i ON i.companycode=o.companycode AND i.itemcode=o.itemcode
       LEFT JOIN ManufacturerMaster mf ON mf.companycode=i.companycode AND mf.manufcode=i.manufcode
      WHERE o.companycode=? AND o.yearcode=? GROUP BY i.manufcode, mf.manufname`, [o.company, o.year])),
  'stock.ml': ST('Stock ML Wise', (db, o) => all(db,
    `SELECT i.ml, SUM(o.qnty) AS qty, SUM(o.qnty*i.ml/1000.0) AS litres FROM openingstockmst o
       JOIN itemmst i ON i.companycode=o.companycode AND i.itemcode=o.itemcode
      WHERE o.companycode=? AND o.yearcode=? GROUP BY i.ml`, [o.company, o.year])),
  'stock.in_litre': ST('Stock-In Litre', (db, o) => all(db,
    `SELECT i.itemcode, i.itemname, SUM(o.qnty) AS qty, SUM(o.qnty*i.ml/1000.0) AS litres FROM openingstockmst o
       JOIN itemmst i ON i.companycode=o.companycode AND i.itemcode=o.itemcode
      WHERE o.companycode=? AND o.yearcode=? GROUP BY i.itemcode, i.itemname ORDER BY litres DESC`, [o.company, o.year])),
  'stock.min_qty': ST('Min Quantity Item', (db, o) => all(db,
    `SELECT i.itemcode, i.itemname, i.min_qnty, COALESCE(o.qnty,0) AS qty FROM itemmst i
       LEFT JOIN openingstockmst o ON o.companycode=i.companycode AND o.itemcode=i.itemcode AND o.yearcode=?
      WHERE i.companycode=? AND (i.min_qnty IS NOT NULL AND COALESCE(o.qnty,0) <= i.min_qnty) ORDER BY qty`, [o.year, o.company])),
  'stock.bl_lpl': ST('Stock-BL/LPL', (db, o) => all(db,
    `SELECT i.itemcode, i.itemname, SUM(o.qnty) AS qty, SUM(o.qnty/i.packing) AS cases, SUM(o.qnty*i.ml/1000.0) AS litres
       FROM openingstockmst o JOIN itemmst i ON i.companycode=o.companycode AND i.itemcode=o.itemcode
      WHERE o.companycode=? AND o.yearcode=? GROUP BY i.itemcode, i.itemname`, [o.company, o.year])),
  'stock.case_bottle': ST('Case-Bottle Wise', (db, o) => all(db,
    `SELECT i.itemcode, i.itemname, i.packing, SUM(o.qnty) AS qty,
            CAST(SUM(o.qnty)/i.packing AS INT) AS cases, SUM(o.qnty) % i.packing AS bottles
       FROM openingstockmst o JOIN itemmst i ON i.companycode=o.companycode AND i.itemcode=o.itemcode
      WHERE o.companycode=? AND o.yearcode=? GROUP BY i.itemcode, i.itemname, i.packing`, [o.company, o.year])),
  'stock.maingroup_subgroup': ST('MainGroup-Sub Group', (db, o) => all(db,
    `SELECT i.groupcode, g.groupname, i.categorycode, c.categoryname, SUM(o.qnty) AS qty
       FROM openingstockmst o JOIN itemmst i ON i.companycode=o.companycode AND i.itemcode=o.itemcode
       LEFT JOIN groupmst g ON g.companycode=i.companycode AND g.groupcode=i.groupcode
       LEFT JOIN categorymst c ON c.companycode=i.companycode AND c.categorycode=i.categorycode
      WHERE o.companycode=? AND o.yearcode=? GROUP BY i.groupcode, g.groupname, i.categorycode, c.categoryname`, [o.company, o.year])),
  'stock.subgroup_item': ST('Sub Group-Item Wise', (db, o) => all(db,
    `SELECT i.groupcode, g.groupname, i.itemcode, i.itemname, SUM(o.qnty) AS qty
       FROM openingstockmst o JOIN itemmst i ON i.companycode=o.companycode AND i.itemcode=o.itemcode
       LEFT JOIN groupmst g ON g.companycode=i.companycode AND g.groupcode=i.groupcode
      WHERE o.companycode=? AND o.yearcode=? GROUP BY i.groupcode, g.groupname, i.itemcode, i.itemname`, [o.company, o.year])),
  'stock.nai_item': ST('Stock NAI Item wise', (db, o) => all(db,
    `SELECT n.itemcode, n.itemname, n.unitcode, n.gstrate FROM naiItemMaster n WHERE n.companycode=?`, [o.company])),
  'stock.expiry': ST('Expiry Report', (db, o) => all(db,
    `SELECT itemcode, itemname, expirydate, salesrate FROM itemmst
      WHERE companycode=? AND expirydate IS NOT NULL ORDER BY expirydate`, [o.company])),
  'stock.counter_wise': ST('Counter Wise Stock', (db, o) => all(db,
    `SELECT o.shopcode, s.shopname, SUM(o.qnty) AS qty FROM openingstockmst o
       LEFT JOIN storage s ON s.companycode=o.companycode AND s.shopcode=o.shopcode
      WHERE o.companycode=? AND o.yearcode=? GROUP BY o.shopcode, s.shopname`, [o.company, o.year])),
  'stock.daily_item': ST('Daily Stock Item Wise', (db, o) => all(db,
    `SELECT sm.trndate, sd.itemcode, SUM(sd.qnty) AS qty
       FROM salesbillmain sm JOIN salesbilldetail sd
         ON sd.companycode=sm.companycode AND sd.yearcode=sm.yearcode AND sd.trnno=sm.trnno
      WHERE sm.companycode=? AND sm.yearcode=? GROUP BY sm.trndate, sd.itemcode ORDER BY sm.trndate, sd.itemcode`, [o.company, o.year])),
  'stock.abc': ST('ABC Report', (db, o) => {
    const rows = stockOnHand(db, { company: o.company, year: o.year });
    const total = rows.reduce((s, r) => s + (r.value || 0), 0) || 1;
    let cum = 0;
    return rows.sort((a, b) => b.value - a.value).map((r) => {
      cum += r.value || 0;
      const pct = cum / total * 100;
      return { ...r, class: pct <= 70 ? 'A' : pct <= 90 ? 'B' : 'C', cumpct: Math.round(pct * 100) / 100 };
    });
  }),

  // ---------------- Purchase (10) ----------------
  'purchase.supplier': P('Supplier Details', (db, o) => all(db,
    `SELECT l.ledcode, l.name, l.city, l.phone, l.email FROM ledger l
      WHERE l.companycode=? AND l.type='Supplier' ORDER BY l.name`, [o.company])),
  'purchase.bill_detail': P('Bill Details', (db, o) => all(db,
    `SELECT trnno, trndate, suppliercode, docno, totamount, tottaxOth, totnetamt FROM purchasemain
      WHERE companycode=? AND yearcode=? ORDER BY trndate DESC`, [o.company, o.year])),
  'purchase.bill_summary': P('Bill Summary', (db, o) => all(db,
    `SELECT COUNT(*) AS bills, SUM(totamount) AS gross, SUM(tottaxOth) AS tax, SUM(totnetamt) AS net
       FROM purchasemain WHERE companycode=? AND yearcode=?`, [o.company, o.year])),
  'purchase.tax': P('Tax report', (db, o) => all(db,
    `SELECT pt.schemecode, pt.TaxCode, SUM(pt.OnAmount) AS onamount, SUM(pt.TaxAmount) AS taxamount
       FROM PurchaseTaxDetail pt WHERE pt.companycode=? AND pt.yearcode=? GROUP BY pt.schemecode, pt.TaxCode`, [o.company, o.year])),
  'purchase.item': P('Item Summary', (db, o) => purchaseDim(db, o.company, o.year, 'item', o)),
  'purchase.category': P('Category Wise', (db, o) => purchaseDim(db, o.company, o.year, 'category', o)),
  'purchase.brand': P('Brand Wise', (db, o) => purchaseDim(db, o.company, o.year, 'kfl', o)),
  'purchase.manufacturer': P('Manufacturer Wise', (db, o) => purchaseDim(db, o.company, o.year, 'manufacturer', o)),
  'purchase.supplier_brand': P('Supplier-Brand Wise', (db, o) => purchaseDim(db, o.company, o.year, 'brand', o)),
  'purchase.nai_gst': P('Purchase NAI GST Report', (db, o) => gst.purchaseGst(db, o.company, o.year, { from: o.from, to: o.to })),
  'purchase.landing_cost': P('Landed Cost & Margin', (db, o) => landedcost.report(db, { company: o.company, year: o.year, from: o.from, to: o.to })),
  'purchase.item_margin': P('Item-wise Margin', (db, o) => landedcost.itemSummary(db, { company: o.company, year: o.year })),

  // ---------------- Accounts (14) ----------------
  'accounts.daybook': A('Day Book', (db, o) => all(db,
    `SELECT trndate, trnno, type, ledcode, narr, debit, credit FROM journaldetail
      WHERE companycode=? AND yearcode=? ORDER BY trndate, trnno, slno`, [o.company, o.year])),
  'accounts.sales_register': A('Sales Register', (db, o) => all(db,
    `SELECT trnno, trndate, ledgercode, salestype, amount, netamount FROM salesbillmain
      WHERE companycode=? AND yearcode=? ORDER BY trndate`, [o.company, o.year])),
  'accounts.sales_return_register': A('Sales Ret. Reg', (db, o) => all(db,
    `SELECT trnno, trndate, ledgercode, amount, netamount FROM salesbillmain
      WHERE companycode=? AND yearcode=? AND salestype='RETURN' ORDER BY trndate`, [o.company, o.year])),
  'accounts.purchase_register': A('Purchase Register', (db, o) => all(db,
    `SELECT trnno, trndate, suppliercode, docno, totnetamt FROM purchasemain
      WHERE companycode=? AND yearcode=? AND ptype='PURCHASE' ORDER BY trndate`, [o.company, o.year])),
  'accounts.purchase_return_register': A('Pur. Return Reg', (db, o) => all(db,
    `SELECT trnno, trndate, suppliercode, docno, totnetamt FROM purchasemain
      WHERE companycode=? AND yearcode=? AND ptype='PURCHASE RETURN' ORDER BY trndate`, [o.company, o.year])),
  'accounts.general_ledger': A('General Ledger', (db, o) => all(db,
    `SELECT ledcode, trndate, trnno, type, narr, debit, credit FROM journaldetail
      WHERE companycode=? AND yearcode=? ORDER BY ledcode, trndate, trnno`, [o.company, o.year])),
  'accounts.group_closing': A('Group Closing', (db, o) => all(db,
    `SELECT l.accode, a.acname, COALESCE(SUM(j.debit-j.credit),0) AS balance
       FROM ledger l LEFT JOIN acountname a ON a.companycode=l.companycode AND a.accode=l.accode
       LEFT JOIN journaldetail j ON j.companycode=l.companycode AND j.ledcode=l.ledcode AND j.yearcode=?
      WHERE l.companycode=? GROUP BY l.accode, a.acname ORDER BY l.accode`, [o.year, o.company])),
  'accounts.pl': A('P/L Account', (db, o) => {
    const rows = ledgerBalances(db, o.company, o.year);
    const income = rows.filter((r) => /income/i.test(r.type || '')).reduce((s, r) => s - r.balance, 0);
    const expense = rows.filter((r) => /expense/i.test(r.type || '')).reduce((s, r) => s + r.balance, 0);
    return [{ head: 'Total Income', amount: Math.round(income * 100) / 100 },
            { head: 'Total Expense', amount: Math.round(expense * 100) / 100 },
            { head: 'Net Profit', amount: Math.round((income - expense) * 100) / 100 }];
  }),
  'accounts.balance_sheet': A('Balance Sheet', (db, o) => all(db,
    `SELECT l.type, l.ledcode, l.name, COALESCE(SUM(j.debit-j.credit),0) AS balance
       FROM ledger l LEFT JOIN journaldetail j ON j.companycode=l.companycode AND j.ledcode=l.ledcode AND j.yearcode=?
      WHERE l.companycode=? GROUP BY l.type, l.ledcode, l.name ORDER BY l.type, l.ledcode`, [o.year, o.company])),
  'accounts.trial': A('Trial Balance', (db, o) => trialBalance(db, o.company, o.year).rows),
  'accounts.tax': A('Tax Report', (db, o) => all(db,
    `SELECT t.SchemeCode, t.schemename, t.TaxCode, t.taxname, t.TaxRate FROM taxdetail t WHERE t.companycode=?`, [o.company])),
  'accounts.tax_register': A('Tax Register', (db, o) => all(db,
    `SELECT pt.schemecode, pt.TaxCode, pt.TaxRate, SUM(pt.OnAmount) AS onamount, SUM(pt.TaxAmount) AS taxamount
       FROM PurchaseTaxDetail pt WHERE pt.companycode=? AND pt.yearcode=? GROUP BY pt.schemecode, pt.TaxCode, pt.TaxRate`, [o.company, o.year])),
  'accounts.outstanding': A('Party Wise Outstanding', (db, o) => all(db,
    `SELECT l.ledcode, l.name, COALESCE(SUM(j.debit-j.credit),0) AS outstanding
       FROM ledger l LEFT JOIN journaldetail j ON j.companycode=l.companycode AND j.ledcode=l.ledcode AND j.yearcode=?
      WHERE l.companycode=? AND l.type IN ('Customer','Supplier','Party')
      GROUP BY l.ledcode, l.name ORDER BY outstanding DESC`, [o.year, o.company])),
  'accounts.cashbank': A('CashBook/BankBook', (db, o) => all(db,
    `SELECT j.trndate, j.trnno, l.name, j.narr, j.debit, j.credit FROM journaldetail j
       JOIN ledger l ON l.companycode=j.companycode AND l.ledcode=j.ledcode
      WHERE j.companycode=? AND j.yearcode=? AND l.type IN ('Cash','Bank') ORDER BY j.trndate, j.trnno`, [o.company, o.year])),

  // ---------------- MIS (12) ----------------
  'mis.daily_summary': M('Daily Summary', (db, o) => all(db,
    `SELECT trndate, COUNT(*) AS bills, SUM(netamount) AS net FROM salesbillmain
      WHERE companycode=? AND yearcode=? GROUP BY trndate ORDER BY trndate`, [o.company, o.year])),
  'mis.bill_payment_receipt': M('Bill Wise Payment & Receipt', (db, o) => all(db,
    `SELECT 'PAYMENT' AS kind, vchno, vchdate, client_account, amount_paid FROM payment_main WHERE companycode=? AND yearcode=?
     UNION ALL
     SELECT 'RECEIPT' AS kind, vchno, vchdate, client_account, amount_paid FROM receipt_main WHERE companycode=? AND yearcode=?
     ORDER BY vchdate`, [o.company, o.year, o.company, o.year])),
  'mis.party_discount': M('Party Sales Discount', (db, o) => all(db,
    `SELECT ledgercode, SUM(discamount) AS discount, SUM(netamount) AS net FROM salesbillmain
      WHERE companycode=? AND yearcode=? GROUP BY ledgercode ORDER BY discount DESC`, [o.company, o.year])),
  'mis.daily_pl': M('Daily Profit-Loss', (db, o) => all(db,
    `SELECT sm.trndate, SUM(sd.itemAmount) AS sales,
            SUM(sd.qnty * COALESCE(i.purchaseRate,0)) AS cost,
            SUM(sd.itemAmount) - SUM(sd.qnty * COALESCE(i.purchaseRate,0)) AS profit
       FROM salesbillmain sm
       JOIN salesbilldetail sd ON sd.companycode=sm.companycode AND sd.yearcode=sm.yearcode AND sd.trnno=sm.trnno
       LEFT JOIN itemmst i ON i.companycode=sd.companycode AND i.itemcode=sd.itemcode
      WHERE sm.companycode=? AND sm.yearcode=? GROUP BY sm.trndate ORDER BY sm.trndate`, [o.company, o.year])),
  'mis.bank_clearance': M('Bank Clearance', (db, o) => all(db,
    `SELECT ledcode, trndate, chqno, chqdate, amount, cleared, cleareddate FROM TransactionReconciliation
      WHERE companycode=? AND yearcode=? ORDER BY trndate`, [o.company, o.year])),
  'mis.tax_scheme': M('Tax Scheme', (db, o) => all(db,
    `SELECT SchemeCode, schemename, COUNT(*) AS lines, SUM(TaxRate) AS totalrate FROM taxdetail
      WHERE companycode=? GROUP BY SchemeCode, schemename`, [o.company])),
  'mis.comparison': M('Comparison Report', (db, o) => all(db,
    `SELECT trndate, SUM(netamount) AS sales FROM salesbillmain WHERE companycode=? AND yearcode=?
      GROUP BY trndate ORDER BY trndate`, [o.company, o.year])),
  'mis.kota': M('Kota Report', (db, o) => all(db,
    `SELECT sm.storecode, COUNT(*) AS bills, SUM(sm.netamount) AS net FROM salesbillmain sm
      WHERE sm.companycode=? AND sm.yearcode=? GROUP BY sm.storecode`, [o.company, o.year])),
  'mis.purchase_profit': M('Purchase Profit', (db, o) => all(db,
    `SELECT i.itemcode, i.itemname, i.purchaseRate, i.salesrate,
            (COALESCE(i.salesrate,0) - COALESCE(i.purchaseRate,0)) AS margin
       FROM itemmst i WHERE i.companycode=? ORDER BY margin DESC`, [o.company])),
  'mis.scheme': M('Scheme Report', (db, o) => all(db,
    `SELECT s.schemecode, s.schemename, d.itemcode, d.buyqty, d.freeqty, d.discount
       FROM tbl_scheme s LEFT JOIN tbl_schemedetail d ON d.companycode=s.companycode AND d.schemecode=s.schemecode
      WHERE s.companycode=? ORDER BY s.schemecode`, [o.company])),
  'mis.payment_summary': M('Payment Summary', (db, o) => all(db,
    `SELECT vchdate, COUNT(*) AS vouchers, SUM(amount_paid) AS paid FROM payment_main
      WHERE companycode=? AND yearcode=? GROUP BY vchdate ORDER BY vchdate`, [o.company, o.year])),
  'mis.item_pl': M('Item Wise Profit-Loss', (db, o) => all(db,
    `SELECT sd.itemcode, i.itemname, SUM(sd.qnty) AS qty, SUM(sd.itemAmount) AS sales,
            SUM(sd.qnty * COALESCE(i.purchaseRate,0)) AS cost,
            SUM(sd.itemAmount) - SUM(sd.qnty * COALESCE(i.purchaseRate,0)) AS profit
       FROM salesbillmain sm
       JOIN salesbilldetail sd ON sd.companycode=sm.companycode AND sd.yearcode=sm.yearcode AND sd.trnno=sm.trnno
       LEFT JOIN itemmst i ON i.companycode=sd.companycode AND i.itemcode=sd.itemcode
      WHERE sm.companycode=? AND sm.yearcode=? GROUP BY sd.itemcode, i.itemname ORDER BY profit DESC`, [o.company, o.year])),

  // ---------------- Breakage (2) ----------------
  'breakage.report': B('Breakage Report', (db, o) => all(db,
    `SELECT bm.TrnNo, bm.trndate, bm.shopcode, bm.Narration, SUM(bd.quantity) AS qty
       FROM breakagemain bm JOIN breakagedetail bd
         ON bd.CompanyCode=bm.CompanyCode AND bd.yearcode=bm.YearCode AND bd.trnno=bm.TrnNo
      WHERE bm.CompanyCode=? AND bm.YearCode=? GROUP BY bm.TrnNo, bm.trndate, bm.shopcode, bm.Narration ORDER BY bm.trndate DESC`, [o.company, o.year])),
  'breakage.item': B('Breakage Item Wise', (db, o) => all(db,
    `SELECT bd.ItemCode, i.itemname, SUM(bd.quantity) AS qty FROM breakagedetail bd
       LEFT JOIN itemmst i ON i.companycode=bd.CompanyCode AND i.itemcode=bd.ItemCode
      WHERE bd.CompanyCode=? AND bd.yearcode=? GROUP BY bd.ItemCode, i.itemname ORDER BY qty DESC`, [o.company, o.year])),

  // ---------------- Stock Transfer (2) ----------------
  'transfer.shop': T('Transfer Shop', (db, o) => all(db,
    `SELECT tm.TrnNo, tm.TrnDate, tm.shopcode_frm, tm.shopcode_to, SUM(td.qnty) AS qty
       FROM stk_transfer_main tm JOIN stk_transfer_detail td
         ON td.CompanyCode=tm.CompanyCode AND td.yearcode=tm.YearCode AND td.trnno=tm.TrnNo
      WHERE tm.CompanyCode=? AND tm.YearCode=? GROUP BY tm.TrnNo, tm.TrnDate, tm.shopcode_frm, tm.shopcode_to ORDER BY tm.TrnDate DESC`, [o.company, o.year])),
  'transfer.receive': T('Transfer Receive', (db, o) => all(db,
    `SELECT trnno, trndate, shopcode_from, shopcode_to FROM transfer_receive_main
      WHERE companycode=? AND yearcode=? ORDER BY trndate DESC`, [o.company, o.year])),

  // ---------------- Excise (21) ----------------
  'excise.daily_stock_book': E('Daily Stock Book', (db, o) => exciseStock(db, o, 'day')),
  'excise.daily_stock_online': E('Daily Stock Online', (db, o) => exciseStock(db, o, 'day')),
  'excise.daily_stock_online_iml': E('Daily Stock Online IML', (db, o) => exciseStock(db, o, 'day')),
  'excise.weekly_stock_online': E('Weekly Stock Online', (db, o) => exciseStock(db, o, 'week')),
  'excise.monthly_stock_online': E('Monthly Stock Online', (db, o) => exciseStock(db, o, 'month')),
  'excise.monthly_stock_online_iml': E('Monthly Stock Online IML', (db, o) => exciseStock(db, o, 'month')),
  'excise.monthly_stock_offline': E('Monthly Stock Offline', (db, o) => exciseStock(db, o, 'month')),
  'excise.monthly_stock_offline_iml': E('Monthly Stock Offline IML', (db, o) => exciseStock(db, o, 'month')),
  'excise.daily_stock': E('Daily Stock', (db, o) => exciseStock(db, o, 'day')),
  'excise.monthly_stock': E('Monthly Stock', (db, o) => exciseStock(db, o, 'month')),
  'excise.monthly_stock_iml': E('Monthly Stock IML', (db, o) => exciseStock(db, o, 'month')),
  'excise.category_register': E('Category Register', (db, o) => all(db,
    `SELECT i.categorycode, c.categoryname, SUM(sd.qnty) AS qty FROM salesbillmain sm
       JOIN salesbilldetail sd ON sd.companycode=sm.companycode AND sd.yearcode=sm.yearcode AND sd.trnno=sm.trnno
       LEFT JOIN itemmst i ON i.companycode=sd.companycode AND i.itemcode=sd.itemcode
       LEFT JOIN categorymst c ON c.companycode=i.companycode AND c.categorycode=i.categorycode
      WHERE sm.companycode=? AND sm.yearcode=? GROUP BY i.categorycode, c.categoryname`, [o.company, o.year])),
  'excise.brand_register': E('Brand Register', (db, o) => all(db,
    `SELECT i.kflcode, k.kflname, SUM(sd.qnty) AS qty FROM salesbillmain sm
       JOIN salesbilldetail sd ON sd.companycode=sm.companycode AND sd.yearcode=sm.yearcode AND sd.trnno=sm.trnno
       LEFT JOIN itemmst i ON i.companycode=sd.companycode AND i.itemcode=sd.itemcode
       LEFT JOIN kflmst k ON k.companycode=i.companycode AND k.kflcode=i.kflcode
      WHERE sm.companycode=? AND sm.yearcode=? GROUP BY i.kflcode, k.kflname`, [o.company, o.year])),
  'excise.monthly_assam': E('Monthly Stock Assam', (db, o) => exciseStock(db, o, 'month')),
  'excise.monthly_assam_spirit': E('Monthly Stock Assam Spirit', (db, o) => exciseStock(db, o, 'month')),
  'excise.date_wise': E('Date Wise Report', (db, o) => exciseStock(db, o, 'day')),
  'excise.date_wise_closing': E('Date Wise Closing Report', (db, o) => exciseStock(db, o, 'day')),
  'excise.date_wise_excel': E('Date Wise Excel Report', (db, o) => exciseStock(db, o, 'day')),
  'excise.lifting_imfl_beer': E('Lifting_IMFL_BEER', (db, o) => all(db,
    `SELECT pm.trndate, i.kflcode, SUM(pd.itemquantity) AS qty FROM purchasemain pm
       JOIN purchasedetail pd ON pd.companycode=pm.companycode AND pd.yearcode=pm.yearcode AND pd.trnno=pm.trnno
       LEFT JOIN itemmst i ON i.companycode=pd.companycode AND i.itemcode=pd.itemcode
      WHERE pm.companycode=? AND pm.yearcode=? GROUP BY pm.trndate, i.kflcode ORDER BY pm.trndate`, [o.company, o.year])),
  'excise.daily_stock_bl': E('Daily Stock BL', (db, o) => exciseStock(db, o, 'day')),
  'excise.daily_stock_bl_iml': E('Daily Stock BL (IML)', (db, o) => exciseStock(db, o, 'day')),
  'excise.form_m22': E('Form M-22 (Brand Register)', (db, o) => m22.brandMovements(db,
    { company: o.company, year: o.year, from: o.from || '0000-01-01', to: o.to || '9999-12-31' })),

  // ---------------- GST (6) ----------------
  'gst.nai_rate_slab': G('NAI GST Report (rate slab)', (db, o) => gst.naiGstReport(db, o.company, o.year, { from: o.from, to: o.to }).rows),
  'gst.nai_detail': G('NAI GST Detail (bill-wise)', (db, o) => gst.naiGstReport(db, o.company, o.year, { from: o.from, to: o.to }).detail),
  'gst.gstr1': G('GSTR-1 (Outward Supplies)', (db, o) => gst.gstr1(db, o.company, o.year, { from: o.from, to: o.to }).rows),
  'gst.gstr3b_rate': G('GSTR-3B Summary by Rate', (db, o) => gst.gstr3b(db, o.company, o.year, { from: o.from, to: o.to }).byRate),
  'gst.purchase_register': G('GST Purchase Register (ITC)', (db, o) => gst.purchaseGst(db, o.company, o.year, { from: o.from, to: o.to })),
  'gst.returns': G('GST Returns Filed', (db, o) => gst.listReturns(db, o.company)),
};

function exciseStock(db, o, bucket) {
  const fmt = bucket === 'month' ? '%Y-%m' : bucket === 'week' ? '%Y-%W' : '%Y-%m-%d';
  return all(db,
    `SELECT strftime('${fmt}', sm.trndate) AS period, sd.itemcode, i.itemname, i.ml,
            SUM(sd.qnty) AS qty, SUM(sd.qnty*i.ml/1000.0) AS litres
       FROM salesbillmain sm
       JOIN salesbilldetail sd ON sd.companycode=sm.companycode AND sd.yearcode=sm.yearcode AND sd.trnno=sm.trnno
       LEFT JOIN itemmst i ON i.companycode=sd.companycode AND i.itemcode=sd.itemcode
      WHERE sm.companycode=? AND sm.yearcode=? GROUP BY period, sd.itemcode, i.itemname, i.ml ORDER BY period, sd.itemcode`,
    [o.company, o.year]);
}

/** Grouped report catalog: { group: [{key,title}] } keyed by lower-case group. */
export function reportGroups() {
  const groups = {};
  for (const [key, def] of Object.entries(CATALOG)) {
    (groups[def.group.toLowerCase()] ||= []).push({ key, title: def.title });
  }
  return groups;
}

export function runCatalogReport(db, key, opts) {
  const def = CATALOG[key];
  if (!def) throw new Error(`Unknown report: ${key}`);
  return def.run(db, opts);
}
