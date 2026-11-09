import { all, get, run } from '../db/index.js';
import { now } from './helpers.js';

/**
 * Form M-22 - the state excise "brand register" a retail licensee files
 * periodically. It is a brand-wise statement of the opening balance, receipts,
 * issues (sales) and breakage, closing with the closing balance, and is signed
 * and countersigned before submission to the excise office.
 *
 * The figures are derived from the same movement tables the stock ledger uses
 * (purchases, sales, breakage), grouped by the item's brand (KFL), so the form
 * always reconciles with the Daily Stock Book.
 */

const clamp = (n) => (Number.isFinite(n) ? n : 0);

/** Brand-wise quantities for a period: opening, received, sold, breakage, closing. */
export function brandMovements(db, { company, year, from, to }) {
  const brands = all(db,
    `SELECT DISTINCT COALESCE(i.kflcode,'') AS brandcode, COALESCE(k.kflname,'(unbranded)') AS brandname
       FROM itemmst i
       LEFT JOIN kflmst k ON k.companycode=i.companycode AND k.kflcode=i.kflcode
      WHERE i.companycode=? ORDER BY brandname`, [company]);

  const bucket = (sql, params) => {
    const m = new Map();
    for (const r of all(db, sql, params)) m.set(r.brandcode || '', clamp(Number(r.qty)));
    return m;
  };

  const opening = bucket(
    `SELECT COALESCE(i.kflcode,'') AS brandcode, SUM(m.qty) AS qty FROM (
       SELECT sd.itemcode, CAST(sd.qnty AS REAL) AS qty FROM openingstockmst sd
        WHERE sd.companycode=? AND sd.yearcode=? AND sd.trndate < ?
       UNION ALL
       SELECT pd.itemcode, CAST(pd.itemquantity AS REAL) FROM purchasemain pm
         JOIN purchasedetail pd ON pd.companycode=pm.companycode AND pd.yearcode=pm.yearcode AND pd.trnno=pm.trnno
        WHERE pm.companycode=? AND pm.yearcode=? AND pm.ptype='PURCHASE' AND pm.trndate < ?
       UNION ALL
       SELECT pd.itemcode, -CAST(pd.itemquantity AS REAL) FROM purchasemain pm
         JOIN purchasedetail pd ON pd.companycode=pm.companycode AND pd.yearcode=pm.yearcode AND pd.trnno=pm.trnno
        WHERE pm.companycode=? AND pm.yearcode=? AND pm.ptype='PURCHASE RETURN' AND pm.trndate < ?
       UNION ALL
       SELECT sd.itemcode, -CAST(sd.qnty AS REAL) FROM salesbillmain sm
         JOIN salesbilldetail sd ON sd.companycode=sm.companycode AND sd.yearcode=sm.yearcode AND sd.trnno=sm.trnno
        WHERE sm.companycode=? AND sm.yearcode=? AND (sm.salestype IS NULL OR sm.salestype<>'RETURN') AND sm.trndate < ?
       UNION ALL
       SELECT sd.itemcode, CAST(sd.qnty AS REAL) FROM salesbillmain sm
         JOIN salesbilldetail sd ON sd.companycode=sm.companycode AND sd.yearcode=sm.yearcode AND sd.trnno=sm.trnno
        WHERE sm.companycode=? AND sm.yearcode=? AND sm.salestype='RETURN' AND sm.trndate < ?
       UNION ALL
       SELECT bd.ItemCode, -CAST(bd.quantity AS REAL) FROM breakagemain bm
         JOIN breakagedetail bd ON bd.CompanyCode=bm.CompanyCode AND bd.yearcode=bm.YearCode AND bd.trnno=bm.TrnNo
        WHERE bm.CompanyCode=? AND bm.YearCode=? AND bm.trndate < ?
     ) m JOIN itemmst i ON i.companycode=? AND i.itemcode=m.itemcode
      GROUP BY i.kflcode`,
    [company, year, from, company, year, from, company, year, from, company, year, from,
     company, year, from, company, year, from, company]);

  const received = bucket(
    `SELECT COALESCE(i.kflcode,'') AS brandcode, SUM(pd.itemquantity) AS qty FROM purchasemain pm
       JOIN purchasedetail pd ON pd.companycode=pm.companycode AND pd.yearcode=pm.yearcode AND pd.trnno=pm.trnno
       LEFT JOIN itemmst i ON i.companycode=pd.companycode AND i.itemcode=pd.itemcode
      WHERE pm.companycode=? AND pm.yearcode=? AND pm.ptype='PURCHASE' AND pm.trndate BETWEEN ? AND ?
      GROUP BY i.kflcode`, [company, year, from, to]);

  const sold = bucket(
    `SELECT COALESCE(i.kflcode,'') AS brandcode, SUM(sd.qnty) AS qty FROM salesbillmain sm
       JOIN salesbilldetail sd ON sd.companycode=sm.companycode AND sd.yearcode=sm.yearcode AND sd.trnno=sm.trnno
       LEFT JOIN itemmst i ON i.companycode=sd.companycode AND i.itemcode=sd.itemcode
      WHERE sm.companycode=? AND sm.yearcode=? AND (sm.salestype IS NULL OR sm.salestype<>'RETURN') AND sm.trndate BETWEEN ? AND ?
      GROUP BY i.kflcode`, [company, year, from, to]);

  const breakage = bucket(
    `SELECT COALESCE(i.kflcode,'') AS brandcode, SUM(bd.quantity) AS qty FROM breakagemain bm
       JOIN breakagedetail bd ON bd.CompanyCode=bm.CompanyCode AND bd.yearcode=bm.YearCode AND bd.trnno=bm.TrnNo
       LEFT JOIN itemmst i ON i.companycode=bd.CompanyCode AND i.itemcode=bd.ItemCode
      WHERE bm.CompanyCode=? AND bm.YearCode=? AND bm.trndate BETWEEN ? AND ?
      GROUP BY i.kflcode`, [company, year, from, to]);

  return brands.map((b) => {
    const o = opening.get(b.brandcode) || 0;
    const r = received.get(b.brandcode) || 0;
    const s = sold.get(b.brandcode) || 0;
    const br = breakage.get(b.brandcode) || 0;
    return {
      brandcode: b.brandcode, brandname: b.brandname,
      openingqty: o, receivedqty: r, soldqty: s, breakageqty: br,
      closingqty: o + r - s - br,
    };
  });
}

/** Build and persist a Form M-22 statement for a period (brand-wise). */
export function buildFormM22(db, { company, year, from, to }) {
  if (!from || !to) throw new Error('A period (from and to dates) is required for Form M-22');
  const rows = brandMovements(db, { company, year, from, to });
  const totals = rows.reduce((a, r) => ({
    openingqty: a.openingqty + r.openingqty, receivedqty: a.receivedqty + r.receivedqty,
    soldqty: a.soldqty + r.soldqty, breakageqty: a.breakageqty + r.breakageqty,
    closingqty: a.closingqty + r.closingqty,
  }), { openingqty: 0, receivedqty: 0, soldqty: 0, breakageqty: 0, closingqty: 0 });

  const at = now();
  const res = run(db,
    `INSERT INTO tbl_FormM22
       (companycode, yearcode, periodfrom, periodto, openingqty, receivedqty, soldqty, breakageqty, closingqty, status, createdat)
     VALUES (?,?,?,?,?,?,?,?,?, 'DRAFT', ?)`,
    [company, year, from, to, totals.openingqty, totals.receivedqty, totals.soldqty,
     totals.breakageqty, totals.closingqty, at]);
  const id = Number(res.lastInsertRowid);
  for (const r of rows) {
    run(db,
      `INSERT INTO tbl_FormM22Detail
         (m22id, companycode, brandcode, brandname, openingqty, receivedqty, soldqty, breakageqty, closingqty)
       VALUES (?,?,?,?,?,?,?,?,?)`,
      [id, company, r.brandcode, r.brandname, r.openingqty, r.receivedqty, r.soldqty, r.breakageqty, r.closingqty]);
  }
  return getFormM22(db, company, id);
}

export function listFormM22(db, company) {
  return all(db, 'SELECT * FROM tbl_FormM22 WHERE companycode=? ORDER BY id DESC', [company]);
}

export function getFormM22(db, company, id) {
  const head = get(db, 'SELECT * FROM tbl_FormM22 WHERE companycode=? AND id=?', [company, id]);
  if (!head) return null;
  const details = all(db, 'SELECT * FROM tbl_FormM22Detail WHERE companycode=? AND m22id=? ORDER BY brandname', [company, id]);
  return { ...head, details };
}

export function fileFormM22(db, company, id) {
  const head = get(db, 'SELECT * FROM tbl_FormM22 WHERE companycode=? AND id=?', [company, id]);
  if (!head) throw Object.assign(new Error(`No Form M-22 statement ${id}`), { status: 404 });
  run(db, "UPDATE tbl_FormM22 SET status='FILED', filedat=? WHERE companycode=? AND id=?", [now(), company, id]);
  return getFormM22(db, company, id);
}

/** Plain-text rendering of a filed statement (the printable document). */
export function renderFormM22Text(db, company, id) {
  const doc = getFormM22(db, company, id);
  if (!doc) throw Object.assign(new Error(`No Form M-22 statement ${id}`), { status: 404 });
  const companyRow = get(db, 'SELECT companyname FROM companymast WHERE companycode=?', [company]);
  const pad = (v, w) => String(v ?? '').padEnd(w);
  const numf = (v) => String(v ?? 0).padStart(12);
  const lines = [];
  lines.push('FORM M-22  -  BRAND REGISTER');
  lines.push(`Licensee : ${companyRow?.companyname || company}`);
  lines.push(`Period   : ${doc.periodfrom} to ${doc.periodto}`);
  lines.push(`Status   : ${doc.status}`);
  lines.push('');
  lines.push(`${pad('Brand', 28)}${pad('Opening', 12)}${pad('Received', 12)}${pad('Sold', 12)}${pad('Breakage', 12)}${pad('Closing', 12)}`);
  lines.push('-'.repeat(88));
  for (const d of doc.details) {
    lines.push(`${pad(d.brandname, 28)}${numf(d.openingqty)}${numf(d.receivedqty)}${numf(d.soldqty)}${numf(d.breakageqty)}${numf(d.closingqty)}`);
  }
  lines.push('-'.repeat(88));
  lines.push(`${pad('TOTAL', 28)}${numf(doc.openingqty)}${numf(doc.receivedqty)}${numf(doc.soldqty)}${numf(doc.breakageqty)}${numf(doc.closingqty)}`);
  lines.push('');
  lines.push('Signature of Licensee : ____________________');
  lines.push('Counter-signature     : ____________________');
  return lines.join('\n');
}
