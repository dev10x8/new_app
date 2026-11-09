import { all, get, run, tx } from '../db/index.js';
import { money, now } from './helpers.js';

// GST implementation, ported from the legacy Madhushala Pro app.
//
// The legacy app captured GST at the NAI item master (cgst/sgst/igst/cess/
// addcess rates, a gsttype flag and ten input/output tax ledgers), computed the
// per-line taxable value and tax at billing, printed a GSTIN on the tax
// invoice, and produced the rate-slab "NAI GST Report" (USP_NAIGstReport).
// This module keeps all of that and adds the return-filing layer the legacy
// app lacked: GSTR-1 (invoice-wise outward supplies), GSTR-3B (summary) and an
// offline JSON submission with a logged filing audit trail.

export const RET_TYPES = ['GSTR1', 'GSTR3B'];

const n = (v) => Number(v) || 0;

// --------------------------------------------------------------- config
export function getConfig(db, company) {
  const row = get(db, 'SELECT * FROM tbl_GstConfig WHERE companycode=?', [company]);
  if (row) return row;
  const comp = get(db, 'SELECT gstin FROM companymast WHERE companycode=?', [company]) || {};
  return {
    companycode: company, gstin: comp.gstin || '', legalname: '', tradename: '',
    statecode: '', endpointbase: '', clientid: '', clientsecret: '',
    autofile: 0, lastfiledat: null, updatedat: null,
  };
}

export function saveConfig(db, company, cfg) {
  const cur = get(db, 'SELECT 1 FROM tbl_GstConfig WHERE companycode=?', [company]);
  const v = [
    cfg.gstin || '', cfg.legalname || '', cfg.tradename || '', cfg.statecode || '',
    cfg.endpointbase || '', cfg.clientid || '', cfg.clientsecret || '',
    cfg.autofile ? 1 : 0, now(),
  ];
  if (cur) {
    run(db, `UPDATE tbl_GstConfig SET gstin=?, legalname=?, tradename=?, statecode=?,
             endpointbase=?, clientid=?, clientsecret=?, autofile=?, updatedat=?
             WHERE companycode=?`, [...v, company]);
  } else {
    run(db, `INSERT INTO tbl_GstConfig
             (gstin, legalname, tradename, statecode, endpointbase, clientid, clientsecret,
              autofile, updatedat, companycode)
             VALUES (?,?,?,?,?,?,?,?,?,?)`, [...v, company]);
  }
  if (cfg.gstin) run(db, 'UPDATE companymast SET gstin=? WHERE companycode=?', [cfg.gstin, company]);
  return getConfig(db, company);
}

export function logs(db, company, limit = 200) {
  return all(db, `SELECT * FROM tbl_GstReturnLog WHERE companycode=?
                   ORDER BY id DESC LIMIT ?`, [company, limit]);
}

function log(db, company, returnid, action, detail) {
  run(db, `INSERT INTO tbl_GstReturnLog (companycode, returnid, action, detail, createdat)
           VALUES (?,?,?,?,?)`, [company, returnid || null, action, detail || null, now()]);
}

// --------------------------------------------------- item GST rates
/**
 * GST rate card: every item's cgst/sgst/igst/cess/addcess rates and the
 * gsttype flag, drawn from both itemmst (alcohol) and naiItemMaster (NAI).
 * This is what the legacy item-master GST tab and the "Item Tax" list showed.
 */
export function listItemRates(db, company) {
  return all(db, `SELECT itemcode, itemname, source, cgst, sgst, igst, cess, addcess, gsttype,
                         cgst+sgst+igst AS gstp, baseprice
                    FROM (
                      SELECT itemcode, itemname, 'item' AS source,
                             COALESCE(cgst,0) cgst, COALESCE(sgst,0) sgst, COALESCE(igst,0) igst,
                             COALESCE(cess,0) cess, COALESCE(addcess,0) addcess,
                             COALESCE(gsttype,1) gsttype, COALESCE(baseprice,0) baseprice
                        FROM itemmst WHERE companycode=?
                      UNION ALL
                      SELECT itemcode, itemname, 'nai' AS source,
                             COALESCE(cgst,0) cgst, COALESCE(sgst,0) sgst, COALESCE(igst,0) igst,
                             COALESCE(cess,0) cess, COALESCE(addcess,0) addcess,
                             COALESCE(gsttype,1) gsttype, COALESCE(baseprice,0) baseprice
                        FROM naiItemMaster WHERE companycode=?
                    ) ORDER BY itemcode`, [company, company]);
}

// --------------------------------------------------- per-line GST computation
/**
 * Resolve an item's GST identity. GST is held on both itemmst (alcohol) and
 * naiItemMaster (non-alcoholic), matching the legacy vw_itemCodebarCode view.
 */
export function itemTax(db, company, itemcode) {
  const row = get(db, `SELECT COALESCE(i.cgst,n.cgst,0) cgst, COALESCE(i.sgst,n.sgst,0) sgst,
                              COALESCE(i.igst,n.igst,0) igst, COALESCE(i.cess,n.cess,0) cess,
                              COALESCE(i.addcess,n.addcess,0) addcess,
                              COALESCE(i.gsttype,n.gsttype,1) gsttype,
                              COALESCE(i.baseprice,n.baseprice,0) baseprice,
                              i.CgstInptLdgr i_cgstin, n.CgstInptLdgr n_cgstin,
                              i.SgstInptLdgr i_sgstin, n.SgstInptLdgr n_sgstin,
                              i.CessInptLdgr i_cessin, n.CessInptLdgr n_cessin,
                              i.AdCessInptLdgr i_adcessin, n.AdCessInptLdgr n_adcessin,
                              i.IgstInptLdgr i_igstin, n.IgstInptLdgr n_igstin,
                              i.CgstOtptLdgr i_cgstout, n.CgstOtptLdgr n_cgstout,
                              i.SgstOtptLdgr i_sgstout, n.SgstOtptLdgr n_sgstout,
                              i.CessOtptLdgr i_cessout, n.CessOtptLdgr n_cessout,
                              i.AdCessOtptLdgr i_adcessout, n.AdCessOtptLdgr n_adcessout,
                              i.IgstOtptLdgr i_igstout, n.IgstOtptLdgr n_igstout
                         FROM (SELECT 1) d
                         LEFT JOIN itemmst i ON i.companycode=? AND i.itemcode=?
                         LEFT JOIN naiItemMaster n ON n.companycode=? AND n.itemcode=?`,
    [company, itemcode, company, itemcode]) || {};
  return {
    cgst: n(row.cgst), sgst: n(row.sgst), igst: n(row.igst),
    cess: n(row.cess), addcess: n(row.addcess),
    gsttype: row.gsttype === null || row.gsttype === undefined ? 1 : Number(row.gsttype),
    baseprice: n(row.baseprice),
    ledgers: {
      cgstIn: row.i_cgstin || row.n_cgstin || null,
      sgstIn: row.i_sgstin || row.n_sgstin || null,
      cessIn: row.i_cessin || row.n_cessin || null,
      adcessIn: row.i_adcessin || row.n_adcessin || null,
      igstIn: row.i_igstin || row.n_igstin || null,
      cgstOut: row.i_cgstout || row.n_cgstout || null,
      sgstOut: row.i_sgstout || row.n_sgstout || null,
      cessOut: row.i_cessout || row.n_cessout || null,
      adcessOut: row.i_adcessout || row.n_adcessout || null,
      igstOut: row.i_igstout || row.n_igstout || null,
    },
  };
}

/**
 * Split a line amount into taxable value and the GST components.
 *
 * `gsttype` follows the legacy flag: 0 = exclusive (GST added on top of the
 * amount, the amount is the taxable value), 1 = inclusive (GST is already
 * inside the amount and is backed out).
 *
 * @returns {{taxable:number,cgst:number,sgst:number,igst:number,cess:number,addcess:number,gstp:number,total:number}}
 */
export function lineGst(amount, t) {
  const amt = money(amount);
  const gstp = n(t.cgst) + n(t.sgst) + n(t.igst);
  const cessp = n(t.cess);
  const addcessp = n(t.addcess);
  const inclusive = Number(t.gsttype) === 1;

  let taxable;
  if (inclusive) {
    taxable = money(amt * 100 / (100 + gstp));
  } else {
    taxable = amt;
  }
  const gstAmt = money(taxable * gstp / 100);
  // Intra-state (CGST+SGST) vs inter-state (IGST): use whichever the item sets.
  const igst = n(t.igst) > 0 ? money(taxable * n(t.igst) / 100) : 0;
  const cgst = igst > 0 ? 0 : money(taxable * n(t.cgst) / 100);
  const sgst = igst > 0 ? 0 : money(taxable * n(t.sgst) / 100);
  const cess = money(taxable * cessp / 100);
  const addcess = money(taxable * addcessp / 100);
  const total = inclusive ? amt : money(amt + gstAmt + cess + addcess);
  return { taxable, cgst, sgst, igst, cess, addcess, gstp, total };
}

// --------------------------------------------------- return period helpers
/** First and last day of a 'YYYY-MM' period. */
export function periodRange(period) {
  const [y, m] = String(period).split('-').map(Number);
  const last = new Date(y, m, 0).getDate();
  const p = (x) => String(x).padStart(2, '0');
  return { from: `${y}-${p(m)}-01`, to: `${y}-${p(m)}-${p(last)}` };
}

// --------------------------------------------------- GSTR-1 (outward)
/**
 * Invoice-wise outward supplies for the period. Mirrors the legacy
 * USP_NAIGstReport detail (bill, item, qty, taxable, rate, CGST/SGST/IGST,
 * cess) but for every taxable item, not only NAI.
 */
export function gstr1(db, company, year, { from, to } = {}) {
  const params = [company, year];
  let sql = `SELECT sm.trnno AS invoiceno, sm.trndate AS invoicedate,
                    sm.salestype, sm.ledgercode, l.name AS partyname,
                    cu.gstin AS partygstin,
                    sd.itemcode, i.itemname,
                    sd.qnty, sd.rate, sd.itemAmount,
                    COALESCE(sd.taxable, sd.itemAmount) AS taxable,
                    COALESCE(sd.cgst,0) cgst, COALESCE(sd.sgst,0) sgst,
                    COALESCE(sd.igst,0) igst, COALESCE(sd.cess,0) cess,
                    COALESCE(sd.addcess,0) addcess, COALESCE(sd.gstp,0) gstp
               FROM salesbillmain sm
               JOIN salesbilldetail sd
                 ON sd.companycode=sm.companycode AND sd.yearcode=sm.yearcode AND sd.trnno=sm.trnno
               LEFT JOIN itemmst i ON i.companycode=sd.companycode AND i.itemcode=sd.itemcode
               LEFT JOIN naiItemMaster ni ON ni.companycode=sd.companycode AND ni.itemcode=sd.itemcode
               LEFT JOIN ledger l ON l.companycode=sm.companycode AND l.ledcode=sm.ledgercode
               LEFT JOIN customerDetails cu ON cu.companycode=sm.companycode AND cu.custcode=sm.ledgercode
              WHERE sm.companycode=? AND sm.yearcode=?`;
  if (from) { sql += ' AND sm.trndate >= ?'; params.push(from); }
  if (to) { sql += ' AND sm.trndate <= ?'; params.push(to); }
  sql += ' ORDER BY sm.trndate, sm.trnno, sd.itemcode';
  const rows = all(db, sql, params);
  const out = rows.map((r) => {
    const sign = String(r.salestype).toUpperCase() === 'RETURN' ? -1 : 1;
    return {
      invoiceno: r.invoiceno, invoicedate: r.invoicedate, salestype: r.salestype,
      party: r.partyname || r.ledgercode, partygstin: r.partygstin || '',
      itemcode: r.itemcode, itemname: r.itemname, qty: sign * n(r.qnty),
      rate: n(r.rate), taxable: money(sign * n(r.taxable)), gstp: n(r.gstp),
      cgst: money(sign * n(r.cgst)), sgst: money(sign * n(r.sgst)),
      igst: money(sign * n(r.igst)), cess: money(sign * (n(r.cess) + n(r.addcess))),
    };
  });
  const totals = out.reduce((a, r) => ({
    taxable: money(a.taxable + r.taxable), cgst: money(a.cgst + r.cgst),
    sgst: money(a.sgst + r.sgst), igst: money(a.igst + r.igst), cess: money(a.cess + r.cess),
  }), { taxable: 0, cgst: 0, sgst: 0, igst: 0, cess: 0 });
  return { rows: out, totals, count: out.length };
}

// --------------------------------------------------- GSTR-3B (summary)
/** Summary outward supplies, bucketed by GST rate (the legacy rate slabs). */
export function gstr3b(db, company, year, { from, to } = {}) {
  const g1 = gstr1(db, company, year, { from, to });
  const byRate = {};
  for (const r of g1.rows) {
    const key = r.gstp || 0;
    const b = (byRate[key] ||= { gstp: key, taxable: 0, cgst: 0, sgst: 0, igst: 0, cess: 0 });
    b.taxable = money(b.taxable + r.taxable); b.cgst = money(b.cgst + r.cgst);
    b.sgst = money(b.sgst + r.sgst); b.igst = money(b.igst + r.igst);
    b.cess = money(b.cess + r.cess);
  }
  const inward = all(db, `SELECT COALESCE(SUM(COALESCE(pd.taxable, pd.itemamount)),0) taxable,
                                 COALESCE(SUM(pd.cgst),0) cgst, COALESCE(SUM(pd.sgst),0) sgst,
                                 COALESCE(SUM(pd.igst),0) igst, COALESCE(SUM(pd.cess+pd.addcess),0) cess
                            FROM purchasemain pm JOIN purchasedetail pd
                              ON pd.companycode=pm.companycode AND pd.yearcode=pm.yearcode AND pd.trnno=pm.trnno
                           WHERE pm.companycode=? AND pm.yearcode=?` + (from ? ' AND pm.trndate>=?' : '') + (to ? ' AND pm.trndate<=?' : ''),
    [company, year, ...(from ? [from] : []), ...(to ? [to] : [])])[0] || {};
  const t = g1.totals;
  const itc = { taxable: money(inward.taxable), cgst: money(inward.cgst), sgst: money(inward.sgst), igst: money(inward.igst), cess: money(inward.cess) };
  return {
    outward: t,
    byRate: Object.values(byRate).sort((a, b) => a.gstp - b.gstp),
    itc,
    netPayable: {
      cgst: money(Math.max(0, t.cgst - itc.cgst)),
      sgst: money(Math.max(0, t.sgst - itc.sgst)),
      igst: money(Math.max(0, t.igst - itc.igst)),
      cess: money(Math.max(0, t.cess - itc.cess)),
    },
  };
}

// --------------------------------------------------- purchase GST register
/** Input-tax register: GST captured on purchase lines. */
export function purchaseGst(db, company, year, { from, to } = {}) {
  const params = [company, year];
  let sql = `SELECT pm.trnno, pm.trndate, pm.suppliercode, l.name AS suppliername, pm.gstin,
                    pd.itemcode, i.itemname, pd.itemquantity, pd.itemrate,
                    COALESCE(pd.taxable, pd.itemamount) taxable, COALESCE(pd.gstp,0) gstp,
                    COALESCE(pd.cgst,0) cgst, COALESCE(pd.sgst,0) sgst, COALESCE(pd.igst,0) igst,
                    COALESCE(pd.cess,0) cess, COALESCE(pd.addcess,0) addcess
               FROM purchasemain pm
               JOIN purchasedetail pd
                 ON pd.companycode=pm.companycode AND pd.yearcode=pm.yearcode AND pd.trnno=pm.trnno
               LEFT JOIN itemmst i ON i.companycode=pd.companycode AND i.itemcode=pd.itemcode
               LEFT JOIN ledger l ON l.companycode=pm.companycode AND l.ledcode=pm.suppliercode
              WHERE pm.companycode=? AND pm.yearcode=?`;
  if (from) { sql += ' AND pm.trndate >= ?'; params.push(from); }
  if (to) { sql += ' AND pm.trndate <= ?'; params.push(to); }
  sql += ' ORDER BY pm.trndate, pm.trnno, pd.itemcode';
  return all(db, sql, params);
}

// --------------------------------------------------- rate-slab report
/**
 * Port of the legacy USP_NAIGstReport: one row per rate slab with the taxable
 * value, CGST/SGST (intra-state) and IGST (inter-state) for each. An optional
 * subgroupcode narrows it to a single NAI sub-group (USP_NAIGstReportSubGroup).
 */
export function naiGstReport(db, company, year, { from, to, subgroupcode } = {}) {
  const params = [company, year];
  let sql = `SELECT sd.trndate, sd.trnno, COALESCE(ni.itemname, i.itemname) itemname,
                    sd.qnty, sd.itemAmount,
                    COALESCE(sd.gstp, COALESCE(ni.cgst,0)+COALESCE(ni.sgst,0)+COALESCE(ni.igst,0)) gstper,
                    COALESCE(sd.taxable, sd.itemAmount) taxable,
                    COALESCE(sd.cgst,0) cgst, COALESCE(sd.sgst,0) sgst, COALESCE(sd.igst,0) igst,
                    COALESCE(sd.cess,0) cess, COALESCE(sd.addcess,0) addcess,
                    ni.subgroupcode
               FROM salesbilldetail sd
               JOIN salesbillmain sm ON sm.companycode=sd.companycode AND sm.yearcode=sd.yearcode AND sm.trnno=sd.trnno
               LEFT JOIN itemmst i ON i.companycode=sd.companycode AND i.itemcode=sd.itemcode
               LEFT JOIN naiItemMaster ni ON ni.companycode=sd.companycode AND ni.itemcode=sd.itemcode
              WHERE sd.companycode=? AND sd.yearcode=?`;
  if (from) { sql += ' AND sd.trndate >= ?'; params.push(from); }
  if (to) { sql += ' AND sd.trndate <= ?'; params.push(to); }
  if (subgroupcode) { sql += ' AND ni.subgroupcode = ?'; params.push(subgroupcode); }
  const rows = all(db, sql, params);

  const slabs = {};
  for (const r of rows) {
    const key = n(r.gstper);
    const s = (slabs[key] ||= { rate: key, taxable: 0, cgst: 0, sgst: 0, igst: 0, cess: 0, addcess: 0, qty: 0 });
    s.taxable = money(s.taxable + n(r.taxable));
    s.cgst = money(s.cgst + n(r.cgst));
    s.sgst = money(s.sgst + n(r.sgst));
    s.igst = money(s.igst + n(r.igst));
    s.cess = money(s.cess + n(r.cess));
    s.addcess = money(s.addcess + n(r.addcess));
    s.qty = n(s.qty) + n(r.qnty);
  }
  return {
    rows: Object.values(slabs).sort((a, b) => a.rate - b.rate),
    detail: rows.map((r) => ({
      date: r.trndate, billno: r.trnno, itemname: r.itemname, qty: n(r.qnty),
      amount: money(n(r.itemAmount)), gstper: n(r.gstper), taxable: money(n(r.taxable)),
      cgst: money(n(r.cgst)), sgst: money(n(r.sgst)), igst: money(n(r.igst)),
      cess: money(n(r.cess)), addcess: money(n(r.addcess)),
    })),
  };
}

// --------------------------------------------------- return filing lifecycle
// GSTR-1 JSON sections. b2b = registered buyers (invoice-wise), b2cs = all other
// outward supplies consolidated per rate (the shape the GSTN offline tool wants).
function buildB2b(rows) {
  const byInvoice = new Map();
  for (const r of rows.filter((x) => x.partygstin)) {
    let inv = byInvoice.get(r.invoiceno);
    if (!inv) {
      inv = {
        ctin: r.partygstin, inv: [{
          inum: r.invoiceno, idt: r.invoicedate,
          val: 0, pos: r.partygstin.slice(0, 2), rchrg: 'N', inv_typ: 'R', itms: [],
        }],
      };
      byInvoice.set(r.invoiceno, inv);
    }
    const doc = inv.inv[0];
    doc.val = money(doc.val + r.taxable + r.cgst + r.sgst + r.igst + r.cess);
    doc.itms.push({
      num: doc.itms.length + 1,
      itm_det: { rt: r.gstp, txval: r.taxable, camt: r.cgst, samt: r.sgst, iamt: r.igst, csamt: r.cess },
    });
  }
  return [...byInvoice.values()];
}

function buildB2cs(rows) {
  const byRate = new Map();
  for (const r of rows.filter((x) => !x.partygstin)) {
    const b = byRate.get(r.gstp) || { sply_ty: r.igst ? 'INTER' : 'INTRA', rt: r.gstp, typ: 'OE', txval: 0, camt: 0, samt: 0, iamt: 0, csamt: 0 };
    b.txval = money(b.txval + r.taxable);
    b.camt = money(b.camt + r.cgst);
    b.samt = money(b.samt + r.sgst);
    b.iamt = money(b.iamt + r.igst);
    b.csamt = money(b.csamt + r.cess);
    byRate.set(r.gstp, b);
  }
  return [...byRate.values()].sort((a, b) => a.rt - b.rt);
}

function findReturn(db, company, rettype, period) {
  return get(db, `SELECT * FROM tbl_GstReturn
                   WHERE companycode=? AND rettype=? AND period=?`, [company, rettype, period]);
}

export function listReturns(db, company) {
  return all(db, `SELECT * FROM tbl_GstReturn WHERE companycode=? ORDER BY period DESC, id DESC`, [company]);
}

/**
 * Build (or rebuild) a return for the period and store its JSON payload.
 * @param {object} o
 * @param {'GSTR1'|'GSTR3B'} o.rettype
 * @param {string} o.period  'YYYY-MM'
 */
export function buildReturn(db, company, year, { rettype = 'GSTR1', period } = {}) {
  if (!RET_TYPES.includes(rettype)) throw new Error(`Unknown return type ${rettype}`);
  if (!/^\d{4}-\d{2}$/.test(String(period || ''))) throw new Error('period must be YYYY-MM');
  const { from, to } = periodRange(period);
  const cfg = getConfig(db, company);

  let payload, totals, invoiceCount = 0;
  if (rettype === 'GSTR1') {
    const g = gstr1(db, company, year, { from, to });
    totals = g.totals; invoiceCount = new Set(g.rows.map((r) => r.invoiceno)).size;
    payload = {
      gstin: cfg.gstin, ret_period: period.replace('-', ''),
      gt: money(g.totals.taxable + g.totals.cgst + g.totals.sgst + g.totals.igst + g.totals.cess),
      b2b: buildB2b(g.rows),
      b2cs: buildB2cs(g.rows),
      nil: { inv: [] },
    };
  } else {
    const g = gstr3b(db, company, year, { from, to });
    totals = g.outward; invoiceCount = 0;
    payload = {
      gstin: cfg.gstin, ret_period: period.replace('-', ''),
      sup_details: {
        osup_det: { txval: g.outward.taxable, camt: g.outward.cgst, samt: g.outward.sgst, iamt: g.outward.igst, csamt: g.outward.cess },
        osup_zero: { txval: 0, iamt: 0, csamt: 0 },
        osup_nil_exmp: { txval: 0 },
      },
      itc_elg: { itc_avl: [{ ty: 'OTH', iamt: g.itc.igst, camt: g.itc.cgst, samt: g.itc.sgst, csamt: g.itc.cess }] },
      by_rate: g.byRate,
      net_payable: g.netPayable,
    };
  }

  const taxamount = money(totals.cgst + totals.sgst + totals.igst + totals.cess);
  return tx(db, () => {
    const existing = findReturn(db, company, rettype, period);
    const vals = [from, to, 'GENERATED', cfg.gstin || null, money(totals.taxable),
      money(totals.cgst), money(totals.sgst), money(totals.igst), money(totals.cess),
      taxamount, invoiceCount, JSON.stringify(payload)];
    let id;
    if (existing) {
      run(db, `UPDATE tbl_GstReturn SET fromdate=?, todate=?, status=?, gstin=?, taxable=?,
               cgst=?, sgst=?, igst=?, cess=?, taxamount=?, invoiceno=?, json=?, createdat=?
               WHERE id=?`, [...vals, now(), existing.id]);
      id = existing.id;
    } else {
      run(db, `INSERT INTO tbl_GstReturn
               (companycode, yearcode, rettype, period, fromdate, todate, status, gstin,
                taxable, cgst, sgst, igst, cess, taxamount, invoiceno, json, createdat)
               VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)`,
        [company, year, rettype, period, ...vals, now()]);
      id = get(db, 'SELECT last_insert_rowid() AS id').id;
    }
    log(db, company, id, 'GENERATED', `${rettype} ${period} taxable ${money(totals.taxable)} tax ${taxamount}`);
    return get(db, 'SELECT * FROM tbl_GstReturn WHERE id=?', [id]);
  });
}

/**
 * "File" a return. There is no live GSTN/GSP integration here, so this is an
 * explicit offline submission: it stamps the return FILED with an ARN and
 * records the action. Wire `submitToPortal` to a GSP for a real e-filing.
 */
export function fileReturn(db, company, id, { arn } = {}) {
  const ret = get(db, 'SELECT * FROM tbl_GstReturn WHERE id=? AND companycode=?', [id, company]);
  if (!ret) throw new Error(`Return ${id} not found`);
  if (ret.status === 'FILED') return ret;
  if (!ret.json) throw new Error('Generate the return before filing it');
  const ref = arn || `OFFLINE-${ret.rettype}-${ret.period}-${Date.now()}`;
  return tx(db, () => {
    run(db, `UPDATE tbl_GstReturn SET status='FILED', arn=?, filedat=? WHERE id=?`, [ref, now(), id]);
    run(db, `UPDATE tbl_GstConfig SET lastfiledat=?, updatedat=? WHERE companycode=?`, [now(), now(), company]);
    log(db, company, id, 'FILED', `arn ${ref}`);
    return get(db, 'SELECT * FROM tbl_GstReturn WHERE id=?', [id]);
  });
}

/** The e-filing payload (JSON) a GSP/GSTN submission would POST. */
export function returnJson(db, company, id) {
  const ret = get(db, 'SELECT * FROM tbl_GstReturn WHERE id=? AND companycode=?', [id, company]);
  if (!ret) throw new Error(`Return ${id} not found`);
  return { rettype: ret.rettype, period: ret.period, status: ret.status, arn: ret.arn, payload: JSON.parse(ret.json || '{}') };
}

/** Print-ready tax invoice data for one sale, including the company GSTIN. */
export function taxInvoice(db, company, year, trnno) {
  const head = get(db, `SELECT sm.*, l.name partyname, cu.gstin partygstin
                          FROM salesbillmain sm
                          LEFT JOIN ledger l ON l.companycode=sm.companycode AND l.ledcode=sm.ledgercode
                          LEFT JOIN customerDetails cu ON cu.companycode=sm.companycode AND cu.custcode=sm.ledgercode
                         WHERE sm.companycode=? AND sm.yearcode=? AND sm.trnno=?`, [company, year, trnno]);
  if (!head) return null;
  const comp = get(db, 'SELECT companyname, address1, address2, city, stat, gstin FROM companymast WHERE companycode=?', [company]) || {};
  const lines = all(db, `SELECT sd.*, COALESCE(i.itemname, ni.itemname) itemname
                           FROM salesbilldetail sd
                           LEFT JOIN itemmst i ON i.companycode=sd.companycode AND i.itemcode=sd.itemcode
                           LEFT JOIN naiItemMaster ni ON ni.companycode=sd.companycode AND ni.itemcode=sd.itemcode
                          WHERE sd.companycode=? AND sd.yearcode=? AND sd.trnno=?`, [company, year, trnno]);
  const totals = lines.reduce((a, l) => ({
    taxable: money(a.taxable + n(l.taxable)), cgst: money(a.cgst + n(l.cgst)),
    sgst: money(a.sgst + n(l.sgst)), igst: money(a.igst + n(l.igst)),
    cess: money(a.cess + n(l.cess) + n(l.addcess)),
  }), { taxable: 0, cgst: 0, sgst: 0, igst: 0, cess: 0 });
  return { seller: { name: comp.companyname, address: [comp.address1, comp.address2, comp.city, comp.stat].filter(Boolean).join(', '), gstin: comp.gstin || '' },
    buyer: { name: head.partyname, gstin: head.partygstin || '' },
    invoice: { trnno: head.trnno, trndate: head.trndate, amount: money(head.amount), netamount: money(head.netamount) },
    lines: lines.map((l) => ({ itemcode: l.itemcode, itemname: l.itemname, qnty: n(l.qnty), rate: n(l.rate),
      amount: money(n(l.itemAmount)), taxable: money(n(l.taxable)), cgst: money(n(l.cgst)), sgst: money(n(l.sgst)),
      igst: money(n(l.igst)), cess: money(n(l.cess) + n(l.addcess)), gstp: n(l.gstp) })),
    totals };
}
