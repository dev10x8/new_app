import { all, get, run, tx } from '../db/index.js';
import { money, nextTrnNo, nextTrnId } from './helpers.js';
import { postVoucher } from './accounting.js';

/**
 * Create a purchase (goods receipt) and its tax detail.
 *
 * @param {object} p
 * @param {Array<{itemcode,batchno,itemrate,itembox,itemloose,itemquantity,itemmrp,itemdiscount}>} p.lines
 */
export function createPurchase(db, p) {
  return tx(db, () => {
    if (!p.lines || p.lines.length === 0) throw new Error('Purchase has no lines');
    const trnno = p.trnno || nextTrnNo(db, 'purchasemain', p.company, p.year, 'trnno');
    const trnid = nextTrnId(db, p.company, p.year, 'PURCHASE');
    const ptype = p.ptype || 'PURCHASE';

    let totamount = 0;
    for (const l of p.lines) {
      const qty = Number(l.itemquantity) || 0;
      totamount = money(totamount + money(Number(l.itemrate) || 0) * qty);
    }
    const tottaxOth = money(p.tottaxOth || 0);
    const totnetamt = money(totamount + tottaxOth);

    run(
      db,
      `INSERT INTO purchasemain
         (companycode, yearcode, trnid, trnno, trndate, ptype, purchaseacccode, suppliercode, shopcode,
          docno, docdate, tppassno, schemecode, totamount, tottaxOth, totnetamt, narration, checkedBy, saletax_including_free)
       VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)`,
      [p.company, p.year, trnid, trnno, p.trndate, ptype, p.purchaseacccode || null, p.suppliercode || null,
       p.shopcode || null, p.docno || `DOC${trnno}`, p.docdate || p.trndate, p.tppassno || '', p.schemecode || null,
       totamount, tottaxOth, totnetamt, p.narration || null, p.checkedBy || null, p.saletaxIncludingFree ? 1 : 0],
    );

    for (const l of p.lines) {
      const qty = Number(l.itemquantity) || 0;
      run(
        db,
        `INSERT INTO purchasedetail
           (companycode, yearcode, trnid, slno, itemcode, batchno, itemrate, itembox, itemloose, itemquantity,
            itemamount, trnno, itemfreeqnty, itemboxrate, itemmrp, itemdiscount, trndate)
         VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)`,
        [p.company, p.year, String(trnid), l.slno || 1, l.itemcode, l.batchno || '0', money(l.itemrate || 0),
         l.itembox || 0, l.itemloose || 0, qty, money(money(l.itemrate || 0) * qty), trnno,
         l.itemfreeqnty || 0, l.itemboxrate || null, l.itemmrp || 0, l.itemdiscount || 0, p.trndate],
      );
    }

    // Double-entry: Dr purchase (gross), Cr supplier.
    const purchaseLed = p.purchaseacccode || 'L00004';
    const supplierLed = p.suppliercode || 'L00008';
    const sign = ptype.toUpperCase().includes('RETURN') ? -1 : 1;
    postVoucher(db, {
      company: p.company, year: p.year, trnno: `PUR${trnno}`, trndate: p.trndate,
      type: ptype, trnid,
      lines: [
        { ledcode: purchaseLed, debit: sign > 0 ? totnetamt : 0, credit: sign < 0 ? totnetamt : 0, narr: ptype },
        { ledcode: supplierLed, debit: sign < 0 ? totnetamt : 0, credit: sign > 0 ? totnetamt : 0, narr: ptype },
      ],
    });

    return { trnno, trnid, totamount, tottaxOth, totnetamt };
  });
}

export function listPurchases(db, company, year, { from, to, ptype } = {}) {
  const params = [company, year];
  let sql = 'SELECT * FROM purchasemain WHERE companycode=? AND yearcode=?';
  if (from) { sql += ' AND trndate >= ?'; params.push(from); }
  if (to) { sql += ' AND trndate <= ?'; params.push(to); }
  if (ptype) { sql += ' AND ptype = ?'; params.push(ptype); }
  sql += ' ORDER BY trndate DESC, trnno DESC';
  return all(db, sql, params);
}

export function getPurchase(db, company, year, trnno) {
  const header = get(db, 'SELECT * FROM purchasemain WHERE companycode=? AND yearcode=? AND trnno=?', [company, year, trnno]);
  if (!header) return null;
  const lines = all(db, 'SELECT * FROM purchasedetail WHERE companycode=? AND yearcode=? AND trnno=?', [company, year, trnno]);
  return { header, lines };
}
