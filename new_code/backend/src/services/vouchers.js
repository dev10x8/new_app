import { all, get, run, tx } from '../db/index.js';
import { money } from './helpers.js';
import { postVoucher } from './accounting.js';

/** Payment / Receipt voucher. type: 'PAYMENT' | 'RECEIPT'. */
export function createPaymentReceipt(db, v) {
  return tx(db, () => {
    const isPay = v.type === 'PAYMENT';
    const table = isPay ? 'payment_main' : 'receipt_main';
    const detailTable = isPay ? 'payment_detail' : 'receipt_detail';
    const vchno = v.vchno || nextVchno(db, table, v.company, v.year);
    const amount = money(v.amount_paid || 0);

    run(
      db,
      `INSERT INTO ${table}
         (companycode, yearcode, vchno, head_account, client_account, narration, vchdate, cheque_no,
          due, amount_paid, discount, net_due, Discount_account_head, trnid, chqDate)
       VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)`,
      [v.company, v.year, vchno, v.head_account || null, v.client_account || null, v.narration || null,
       v.vchdate, v.cheque_no || null, money(v.due || 0), amount, money(v.discount || 0), money(v.net_due || 0),
       v.discount_account_head || null, v.trnid || 0, v.chqDate || null],
    );

    run(
      db,
      `INSERT INTO ${detailTable}
         (companycode, yearcode, vchno, trnno, due_amount, debit, credit, vchdate)
       VALUES (?,?,?,?,?,?,?,?)`,
      [v.company, v.year, vchno, v.trnno || '', money(v.due || 0), money(v.debit || 0), money(v.credit || 0), v.vchdate],
    );

    // Dr client account, Cr head account (payment); reverse for receipt.
    const lines = isPay
      ? [
          { ledcode: v.client_account || 'L00008', debit: amount, narr: v.narration },
          { ledcode: v.head_account || 'L00002', credit: amount, narr: v.narration },
        ]
      : [
          { ledcode: v.head_account || 'L00002', debit: amount, narr: v.narration },
          { ledcode: v.client_account || 'L00007', credit: amount, narr: v.narration },
        ];
    postVoucher(db, { company: v.company, year: v.year, trnno: `${isPay ? 'PAY' : 'REC'}${vchno}`, trndate: v.vchdate, type: v.type, lines });

    return { vchno };
  });
}

/** Contra entry: move money between two ledgers (cash <-> bank). */
export function createContra(db, v) {
  return tx(db, () => {
    const amount = money(v.amount || 0);
    postVoucher(db, {
      company: v.company, year: v.year, trnno: v.trnno || `CTR${Date.now()}`, trndate: v.vchdate,
      type: 'CONTRA',
      lines: [
        { ledcode: v.to_ledger, debit: amount, narr: v.narration || 'Contra' },
        { ledcode: v.from_ledger, credit: amount, narr: v.narration || 'Contra' },
      ],
    });
    return { ok: true, amount };
  });
}

/** Journal entry: multi-line manual voucher. */
export function createJournal(db, v) {
  return tx(db, () => {
    const trnno = v.trnno || `JRN${Date.now()}`;
    postVoucher(db, {
      company: v.company, year: v.year, trnno, trndate: v.vchdate, type: 'JOURNAL',
      lines: v.lines.map((l) => ({ ledcode: l.ledcode, debit: money(l.debit || 0), credit: money(l.credit || 0), narr: l.narr })),
    });
    return { trnno };
  });
}

function nextVchno(db, table, company, year) {
  const row = get(db, `SELECT COALESCE(MAX(vchno),0) AS n FROM ${table} WHERE companycode=? AND yearcode=?`, [company, year]);
  return (row?.n || 0) + 1;
}

export function listVouchers(db, company, year, type) {
  const table = type === 'PAYMENT' ? 'payment_main' : 'receipt_main';
  return all(db, `SELECT * FROM ${table} WHERE companycode=? AND yearcode=? ORDER BY vchdate DESC, vchno DESC`, [company, year]);
}

/** General ledger for one ledger code. */
export function generalLedger(db, company, year, ledcode) {
  const rows = all(
    db,
    `SELECT trndate, trnno, type, narr, debit, credit
       FROM journaldetail
      WHERE companycode=? AND yearcode=? AND ledcode=?
      ORDER BY trndate, trnno, slno`,
    [company, year, ledcode],
  );
  let bal = 0;
  const out = rows.map((r) => {
    bal = money(bal + r.debit - r.credit);
    return { ...r, balance: bal };
  });
  return out;
}

/** Day book: all journal lines for a date. */
export function dayBook(db, company, year, date) {
  return all(
    db,
    `SELECT trnno, type, ledcode, narr, debit, credit
       FROM journaldetail
      WHERE companycode=? AND yearcode=? AND trndate=?
      ORDER BY trnno, slno`,
    [company, year, date],
  );
}
