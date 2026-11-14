import { all, get, run, tx } from '../db/index.js';
import { money, nextBillNo, nextTrnId, billPrefix, now } from './helpers.js';
import { assertPostableDate } from './clock.js';
import { postVoucher } from './accounting.js';
import { getConfig, enqueueSale } from './tally.js';
import { captureSale } from './excise.js';
import { itemTax, lineGst } from './gst.js';

const SALE_TYPES = ['FL', 'CS', 'NAI', 'CASH', 'CREDIT', 'CREDIT CARD', 'CREDIT ACCOUNT', 'RETURN'];

export function listSaleTypes() {
  return SALE_TYPES;
}

/**
 * Create a POS / counter sale.
 *
 * @param {object} sale
 * @param {string} sale.company
 * @param {string} sale.year
 * @param {string} sale.storecode
 * @param {string} sale.ledgercode     party ledger (default walk-in)
 * @param {string} sale.salestype      FL | CS | NAI | RETURN ...
 * @param {string} sale.ratecode
 * @param {string} sale.trndate
 * @param {string} sale.userId
 * @param {number} sale.discount
 * @param {Array<{itemcode:string,qnty:number,rate?:number,mrp?:number,batchNo?:string,batchDate?:string}>} sale.lines
 */
export function createSale(db, sale) {
  return tx(db, () => {
    if (!sale.lines || sale.lines.length === 0) throw new Error('Sale has no lines');

    // The date is server-owned and validated: no future dates, no posting into
    // a closed day, back-dating beyond tolerance is allowed but flagged.
    const dateCheck = assertPostableDate(db, {
      company: sale.company, year: sale.year, date: sale.trndate, lockTable: 'salesbillmain',
    });
    const trndate = dateCheck.date;
    const trnno = nextBillNo(db, sale.company, sale.year, sale.salestype);
    const trnid = nextTrnId(db, sale.company, sale.year, 'SALE');

    const params = get(db, 'SELECT * FROM companyparamtr WHERE companycode = ?', [sale.company]) || {};
    const ledgercode = sale.ledgercode || params.cashparty || 'C00001';
    const ratecode = sale.ratecode || params.ratecode || 'G00001';
    const isReturn = String(sale.salestype).toUpperCase() === 'RETURN';

    let amount = 0;
    let gstTotal = 0;
    const resolved = [];
    for (const line of sale.lines) {
      const item = get(db, 'SELECT * FROM itemmst WHERE companycode = ? AND itemcode = ?', [sale.company, line.itemcode]);
      if (!item) throw new Error(`Unknown item ${line.itemcode}`);
      const rate = money(line.rate ?? item.salesrate ?? 0);
      const qty = Number(line.qnty) || 0;
      // Per-line discount (legacy item-wise discount): a percentage, or an
      // absolute amount. It lowers the line's taxable value, so GST is charged
      // on the discounted amount - the same way the legacy grid's "TOTAL AMOUNT"
      // column already had the line discount netted out.
      const gross = money(rate * qty);
      const lineDisc = money(line.discp ? money(gross * Number(line.discp) / 100) : (Number(line.discount) || 0));
      const lineAmount = money(gross - lineDisc);
      amount = money(amount + lineAmount);
      // GST: split the line into taxable value + tax components (legacy logic).
      const tax = itemTax(db, sale.company, line.itemcode);
      const gst = lineGst(lineAmount, tax);
      gstTotal = money(gstTotal + gst.cgst + gst.sgst + gst.igst + gst.cess + gst.addcess);
      resolved.push({ item, rate, qty, lineAmount, lineDisc, gst, tax, mrp: money(line.mrp ?? item.salesrate ?? 0), ...line });
    }

    const discamount = money(sale.discount || 0);
    // Credit-card surcharge and cash rounding (legacy: txt_surchargerate /
    // txt_rnd). Both default to 0, so a plain sale is unaffected.
    const surcharge = money(sale.surcharge || 0);
    const roundoff = money(sale.roundoff || 0);
    const netamount = money(amount - discamount + surcharge + roundoff);

    run(
      db,
      `INSERT INTO salesbillmain
         (companycode, trnno, trndate, ledgercode, amount, salestype, ratecode, storecode, yearcode,
          discamount, netamount, account_head, discount_account_head, trntime, trnid, discount_reason, KFL, igst,
          surcharge, roundoff)
       VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)`,
      [sale.company, trnno, trndate, ledgercode, amount, sale.salestype, ratecode, sale.storecode, sale.year,
       discamount, netamount, params.SaleAccount_Head || 'L00003', params.discntacc || 'L00005',
       trndate, trnid, sale.discountReason || null, sale.kfl || 'none', money(gstTotal),
       surcharge, roundoff],
    );

    for (const r of resolved) {
      const g = r.gst;
      run(
        db,
        `INSERT INTO salesbilldetail
           (companycode, itemcode, tot_box, loose, qnty, itemAmount, trnno, rate, trndate, yearcode,
            large, small, westage, MRP, batchDate, batchNo,
            taxable, cgst, sgst, igst, cess, addcess, gstp)
         VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)`,
        [sale.company, r.itemcode, r.tot_box || 0, r.loose || 0, r.qty, r.lineAmount, trnno, r.rate, trndate, sale.year,
         r.large || 0, r.small || 0, r.westage || 0, r.mrp, r.batchDate || trndate, r.batchNo || '0',
         g.taxable, g.cgst, g.sgst, g.igst, g.cess, g.addcess, g.gstp],
      );
    }

    // Double-entry. Sale: Dr party (net) + Dr discount, Cr sales (gross).
    // Return reverses every leg.
    const saleLed = params.SaleAccount_Head || 'L00003';
    const discLed = params.discntacc || 'L00005';
    const sign = isReturn ? -1 : 1;
    const lines = [
      { ledcode: ledgercode, debit: sign * netamount > 0 ? netamount : 0, credit: sign * netamount < 0 ? netamount : 0, narr: `${isReturn ? 'Sale return' : 'Sale'} ${trnno}` },
      { ledcode: saleLed, debit: sign * amount < 0 ? amount : 0, credit: sign * amount > 0 ? amount : 0, narr: `${isReturn ? 'Sale return' : 'Sale'} ${trnno}` },
    ];
    if (discamount > 0) {
      lines.push({ ledcode: discLed, debit: sign > 0 ? discamount : 0, credit: sign < 0 ? discamount : 0, narr: `Discount ${trnno}` });
    }
    if (surcharge !== 0) {
      lines.push({ ledcode: params.surcharge_account_head || saleLed, debit: sign * surcharge < 0 ? Math.abs(surcharge) : 0, credit: sign * surcharge > 0 ? Math.abs(surcharge) : 0, narr: `Surcharge ${trnno}` });
    }
    if (roundoff !== 0) {
      lines.push({ ledcode: params.rndoffacc || discLed, debit: sign * roundoff < 0 ? Math.abs(roundoff) : 0, credit: sign * roundoff > 0 ? Math.abs(roundoff) : 0, narr: `Rounding ${trnno}` });
    }

    run(db, 'INSERT INTO trnidmst (companycode, yearcode, trnid, trn_type, usercode) VALUES (?,?,?,?,?)',
      [sale.company, sale.year, trnid, 'SALE', sale.userId || null]);

    postVoucher(db, {
      company: sale.company, year: sale.year, trnno, trndate,
      type: isReturn ? 'SALE RETURN' : 'SALE', trnid, lines,
    });

    if (getConfig(db, sale.company).enabled) {
      try { enqueueSale(db, sale.company, sale.year, trnno); }
      catch { /* never block billing on sync queueing */ }
    }

    // Excise compliance capture (best-effort; never blocks billing).
    try { captureSale(db, { company: sale.company, year: sale.year, trnno }); }
    catch { /* excise queueing is non-fatal */ }

    return { trnno, trnid, amount, discamount, surcharge, roundoff, netamount, gst: money(gstTotal),
      prefix: billPrefix(sale.salestype), lines: resolved.length,
      date: trndate, date_warning: dateCheck.backdated ? `back-dated by ${dateCheck.backdated_days} day(s)` : null,
      clock_rolled_back: dateCheck.clock_rolled_back };
  });
}

export function getSale(db, company, year, trnno) {
  const header = get(db, 'SELECT * FROM salesbillmain WHERE companycode=? AND yearcode=? AND trnno=?', [company, year, trnno]);
  if (!header) return null;
  const lines = all(db, 'SELECT * FROM salesbilldetail WHERE companycode=? AND yearcode=? AND trnno=?', [company, year, trnno]);
  return { header, lines };
}

export function listSales(db, company, year, { from, to, salestype } = {}) {
  const params = [company, year];
  let sql = 'SELECT * FROM salesbillmain WHERE companycode=? AND yearcode=?';
  if (from) { sql += ' AND trndate >= ?'; params.push(from); }
  if (to) { sql += ' AND trndate <= ?'; params.push(to); }
  if (salestype) { sql += ' AND salestype = ?'; params.push(salestype); }
  sql += ' ORDER BY trndate DESC, trnno DESC';
  return all(db, sql, params);
}

/** Delete a sale and archive it (legacy: DeleteSalesBillMain / DeletedSalesbilldetail). */
export function deleteSale(db, company, year, trnno) {
  return tx(db, () => {
    const header = get(db, 'SELECT * FROM salesbillmain WHERE companycode=? AND yearcode=? AND trnno=?', [company, year, trnno]);
    if (!header) throw new Error('Sale not found');
    const lines = all(db, 'SELECT * FROM salesbilldetail WHERE companycode=? AND yearcode=? AND trnno=?', [company, year, trnno]);
    const when = now();
    run(
      db,
      `INSERT INTO DeleteSalesBillMain
         (companycode, trnno, trndate, ledgercode, amount, salestype, storecode, yearcode, discamount, netamount, deletedon)
       VALUES (?,?,?,?,?,?,?,?,?,?,?)`,
      [header.companycode, header.trnno, header.trndate, header.ledgercode, header.amount, header.salestype,
       header.storecode, header.yearcode, header.discamount, header.netamount, when],
    );
    for (const l of lines) {
      run(
        db,
        `INSERT INTO DeletedSalesbilldetail (companycode, itemcode, qnty, itemAmount, trnno, rate, yearcode)
         VALUES (?,?,?,?,?,?,?)`,
        [l.companycode, l.itemcode, l.qnty, l.itemAmount, l.trnno, l.rate, l.yearcode],
      );
    }
    run(db, 'DELETE FROM salesbilldetail WHERE companycode=? AND yearcode=? AND trnno=?', [company, year, trnno]);
    run(db, 'DELETE FROM salesbillmain WHERE companycode=? AND yearcode=? AND trnno=?', [company, year, trnno]);
    run(db, 'INSERT INTO TBL_Audit (companycode, tablename, action, refkey, auditat) VALUES (?,?,?,?,?)',
      [company, 'salesbillmain', 'DELETE', trnno, when]);
    return { deleted: trnno };
  });
}

/** Archived (deleted) bills register. */
export function listDeletedBills(db, company, year) {
  return all(db,
    'SELECT * FROM DeleteSalesBillMain WHERE companycode=? AND yearcode=? ORDER BY deletedon DESC',
    [company, year]);
}

/**
 * Day close: stamp salesbillmain.dayclose for all unclosed rows in a range.
 * The legacy schema tracks the close in the sales summary; here we add the
 * column lazily and record the close date.
 */
export function dayClose(db, company, year, { from, to }) {
  return tx(db, () => {
    ensureDayCloseColumn(db);
    const res = run(
      db,
      `UPDATE salesbillmain SET dayclose = ?
        WHERE companycode = ? AND yearcode = ? AND dayclose IS NULL
          AND trndate >= ? AND trndate <= ?`,
      [to, company, year, from, to],
    );
    return { closed: res.changes };
  });
}

function ensureDayCloseColumn(db) {
  const cols = all(db, "PRAGMA table_info(salesbillmain)");
  if (!cols.some((c) => c.name === 'dayclose')) {
    db.exec('ALTER TABLE salesbillmain ADD COLUMN dayclose TEXT');
  }
}

/** Daily sales summary (used by dashboards and the day-close screen). */
export function salesSummary(db, company, year, { from, to } = {}) {
  const params = [company, year];
  let sql = `SELECT trndate,
                    COUNT(*) AS bills,
                    SUM(amount) AS gross,
                    SUM(discamount) AS discount,
                    SUM(netamount) AS net
               FROM salesbillmain
              WHERE companycode=? AND yearcode=?`;
  if (from) { sql += ' AND trndate >= ?'; params.push(from); }
  if (to) { sql += ' AND trndate <= ?'; params.push(to); }
  sql += ' GROUP BY trndate ORDER BY trndate';
  return all(db, sql, params);
}
