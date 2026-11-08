import { all, get, run } from '../db/index.js';
import { money, nextTrnNo } from './helpers.js';

/**
 * Post a balanced double-entry voucher to journaldetail.
 *
 * @param {DatabaseSync} db
 * @param {object} v
 * @param {string} v.company
 * @param {string} v.year
 * @param {string} v.trnno      voucher number (string)
 * @param {string} v.trndate    'YYYY-MM-DD'
 * @param {string} v.type       voucher type label (SALE, PURCHASE, PAYMENT, ...)
 * @param {number} v.trnid
 * @param {Array<{ledcode:string,debit?:number,credit?:number,narr?:string,chqno?:string,chqdate?:string}>} v.lines
 */
export function postVoucher(db, v) {
  const debit = money(v.lines.reduce((s, l) => s + (Number(l.debit) || 0), 0));
  const credit = money(v.lines.reduce((s, l) => s + (Number(l.credit) || 0), 0));
  if (debit !== credit) {
    throw new Error(`Voucher ${v.trnno} is not balanced: debit ${debit} != credit ${credit}`);
  }
  let slno = 0;
  for (const line of v.lines) {
    slno += 1;
    run(
      db,
      `INSERT INTO journaldetail
         (companycode, yearcode, trnno, trndate, slno, to_by, ledcode, chqno, chqdate,
          accpay, debit, credit, type, narr, trnid, Currbal)
       VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)`,
      [
        v.company, v.year, String(v.trnno), v.trndate, slno,
        (Number(line.debit) || 0) > 0 ? 'Dr' : 'Cr',
        line.ledcode, line.chqno || null, line.chqdate || null,
        line.accpay || null, money(line.debit || 0), money(line.credit || 0),
        v.type, line.narr || v.narr || null, v.trnid || 0, null,
      ],
    );
  }
  return { debit, credit, lines: slno };
}

/**
 * Ledger balances (debit - credit) for a company/year.
 */
export function ledgerBalances(db, company, year) {
  return all(
    db,
    `SELECT l.ledcode, l.name, l.type,
            COALESCE(SUM(j.debit), 0)  AS debit,
            COALESCE(SUM(j.credit), 0) AS credit,
            COALESCE(SUM(j.debit), 0) - COALESCE(SUM(j.credit), 0) AS balance
       FROM ledger l
       LEFT JOIN journaldetail j
         ON j.ledcode = l.ledcode AND j.companycode = l.companycode
        AND j.yearcode = ?
      WHERE l.companycode = ?
      GROUP BY l.ledcode, l.name, l.type
      ORDER BY l.ledcode`,
    [year, company],
  );
}

/**
 * Trial balance: debit and credit columns must agree.
 */
export function trialBalance(db, company, year) {
  const rows = all(
    db,
    `SELECT l.ledcode, l.name,
            COALESCE(SUM(j.debit), 0)  AS debit,
            COALESCE(SUM(j.credit), 0) AS credit
       FROM ledger l
       LEFT JOIN journaldetail j
         ON j.ledcode = l.ledcode AND j.companycode = l.companycode AND j.yearcode = ?
      WHERE l.companycode = ?
      GROUP BY l.ledcode, l.name
      ORDER BY l.ledcode`,
    [year, company],
  );
  const totalDebit = money(rows.reduce((s, r) => s + r.debit, 0));
  const totalCredit = money(rows.reduce((s, r) => s + r.credit, 0));
  return { rows, totalDebit, totalCredit, balanced: totalDebit === totalCredit };
}

/** Next voucher sequence for a given type label. */
export function nextVoucherNo(db, company, year, type) {
  return nextTrnNo(db, 'journaldetail', company, year, 'slno') === 0
    ? 1
    : (get(
        db,
        'SELECT COUNT(DISTINCT trnno) AS n FROM journaldetail WHERE companycode=? AND yearcode=? AND type=?',
        [company, year, type],
      )?.n || 0) + 1;
}
