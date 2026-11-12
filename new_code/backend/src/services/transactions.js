import { all, get, run, tx } from '../db/index.js';
import { money, nextTrnNo } from './helpers.js';
import { assertPostableDate } from './clock.js';
import { postVoucher } from './accounting.js';

// ---------------------------------------------------------------- transfer sale
/** Counter / shop transfer sale: moves goods from one shop to another with value. */
export function createTransferSale(db, t) {
  return tx(db, () => {
    t = { ...t, trndate: assertPostableDate(db, { company: t.company, year: t.year, date: t.trndate }).date };
    const trnno = t.trnno || nextTrnNo(db, 'transfer_sale_main', t.company, t.year, 'trnno');
    let amount = 0;
    for (const l of t.lines) {
      const item = get(db, 'SELECT * FROM itemmst WHERE companycode=? AND itemcode=?', [t.company, l.itemcode]);
      const rate = money(l.rate ?? item?.salesrate ?? 0);
      amount = money(amount + rate * (Number(l.qnty) || 0));
    }
    run(db,
      `INSERT INTO transfer_sale_main (companycode, yearcode, trnno, trndate, shopcode_from, shopcode_to, amount, narration, trnid)
       VALUES (?,?,?,?,?,?,?,?,?)`,
      [t.company, t.year, trnno, t.trndate, t.from, t.to, amount, t.narration || null, t.trnid || 0]);
    for (const l of t.lines) {
      const item = get(db, 'SELECT * FROM itemmst WHERE companycode=? AND itemcode=?', [t.company, l.itemcode]);
      const rate = money(l.rate ?? item?.salesrate ?? 0);
      run(db,
        `INSERT INTO transfer_sale_detail (companycode, yearcode, trnno, itemcode, qnty, rate, amount)
         VALUES (?,?,?,?,?,?,?)`,
        [t.company, t.year, trnno, l.itemcode, l.qnty || 0, rate, money(rate * (Number(l.qnty) || 0))]);
    }
    return { trnno, amount };
  });
}

export function listTransferSales(db, company, year) {
  return all(db, 'SELECT * FROM transfer_sale_main WHERE companycode=? AND yearcode=? ORDER BY trndate DESC', [company, year]);
}

/** Transfer receive: records goods received from another shop. */
export function createTransferReceive(db, t) {
  return tx(db, () => {
    t = { ...t, trndate: assertPostableDate(db, { company: t.company, year: t.year, date: t.trndate }).date };
    const trnno = t.trnno || nextTrnNo(db, 'transfer_receive_main', t.company, t.year, 'trnno');
    run(db,
      `INSERT INTO transfer_receive_main (companycode, yearcode, trnno, trndate, shopcode_from, shopcode_to, narration)
       VALUES (?,?,?,?,?,?,?)`,
      [t.company, t.year, trnno, t.trndate, t.from, t.to, t.narration || null]);
    for (const l of t.lines) {
      run(db,
        `INSERT INTO transfer_receive_detail (companycode, yearcode, trnno, itemcode, qnty) VALUES (?,?,?,?,?)`,
        [t.company, t.year, trnno, l.itemcode, l.qnty || 0]);
    }
    return { trnno };
  });
}

export function listTransferReceives(db, company, year) {
  return all(db, 'SELECT * FROM transfer_receive_main WHERE companycode=? AND yearcode=? ORDER BY trndate DESC', [company, year]);
}

// ---------------------------------------------------------------- notes
/** Debit / credit note. type: 'DEBIT' | 'CREDIT'. */
export function createNote(db, n) {
  return tx(db, () => {
    n = { ...n, trndate: assertPostableDate(db, { company: n.company, year: n.year, date: n.trndate }).date };
    const noteno = n.noteno || `${n.type === 'DEBIT' ? 'DN' : 'CN'}${String(nextTrnNo(db, 'notes_main', n.company, n.year, 'noteno')).padStart(5, '0')}`;
    const amount = money(n.amount || 0);
    run(db,
      `INSERT INTO notes_main (companycode, yearcode, noteno, trndate, notetype, partycode, headaccount, amount, narration, trnid)
       VALUES (?,?,?,?,?,?,?,?,?,?)`,
      [n.company, n.year, noteno, n.trndate, n.type, n.partycode || null, n.headaccount || null, amount, n.narration || null, n.trnid || 0]);
    let slno = 0;
    for (const l of n.lines || []) {
      slno += 1;
      run(db,
        `INSERT INTO notes_detail (companycode, yearcode, noteno, slno, ledcode, amount, narration) VALUES (?,?,?,?,?,?,?)`,
        [n.company, n.year, noteno, slno, l.ledcode, money(l.amount || 0), l.narration || null]);
    }
    // Debit note: Dr party, Cr purchase/expense head. Credit note: reverse.
    const sign = n.type === 'DEBIT' ? 1 : -1;
    postVoucher(db, {
      company: n.company, year: n.year, trnno: noteno, trndate: n.trndate, type: `${n.type} NOTE`,
      lines: [
        { ledcode: n.partycode || 'L00007', debit: sign > 0 ? amount : 0, credit: sign < 0 ? amount : 0, narr: n.narration },
        { ledcode: n.headaccount || 'L00004', debit: sign < 0 ? amount : 0, credit: sign > 0 ? amount : 0, narr: n.narration },
      ],
    });
    return { noteno, amount };
  });
}

export function listNotes(db, company, year, type) {
  const params = [company, year];
  let sql = 'SELECT * FROM notes_main WHERE companycode=? AND yearcode=?';
  if (type) { sql += ' AND notetype=?'; params.push(type); }
  sql += ' ORDER BY trndate DESC';
  return all(db, sql, params);
}

// ---------------------------------------------------------------- adjustments
export function createOnAccountAdjustment(db, a) {
  return tx(db, () => {
    a = { ...a, trndate: assertPostableDate(db, { company: a.company, year: a.year, date: a.trndate }).date };
    const trnno = a.trnno || nextTrnNo(db, 'onaccount_adjustment', a.company, a.year, 'trnno');
    const amount = money(a.amount || 0);
    run(db,
      `INSERT INTO onaccount_adjustment (companycode, yearcode, trnno, trndate, partycode, amount, narration)
       VALUES (?,?,?,?,?,?,?)`,
      [a.company, a.year, trnno, a.trndate, a.partycode || null, amount, a.narration || null]);
    postVoucher(db, {
      company: a.company, year: a.year, trnno: `ONA${trnno}`, trndate: a.trndate, type: 'ON A/C ADJUSTMENT',
      lines: [
        { ledcode: a.partycode || 'L00007', debit: amount, narr: 'On a/c adjustment' },
        { ledcode: a.against_ledger || 'L00003', credit: amount, narr: 'On a/c adjustment' },
      ],
    });
    return { trnno, amount };
  });
}

export function createAccountReplace(db, a) {
  return tx(db, () => {
    a = { ...a, trndate: assertPostableDate(db, { company: a.company, year: a.year, date: a.trndate }).date };
    const trnno = a.trnno || nextTrnNo(db, 'account_replace', a.company, a.year, 'trnno');
    const amount = money(a.amount || 0);
    run(db,
      `INSERT INTO account_replace (companycode, yearcode, trnno, trndate, from_ledger, to_ledger, amount, narration)
       VALUES (?,?,?,?,?,?,?,?)`,
      [a.company, a.year, trnno, a.trndate, a.from_ledger, a.to_ledger, amount, a.narration || null]);
    postVoucher(db, {
      company: a.company, year: a.year, trnno: `ACR${trnno}`, trndate: a.trndate, type: 'A/C REPLACE',
      lines: [
        { ledcode: a.to_ledger, debit: amount, narr: 'A/c replace' },
        { ledcode: a.from_ledger, credit: amount, narr: 'A/c replace' },
      ],
    });
    return { trnno, amount };
  });
}

// ---------------------------------------------------------------- excise fine
export function createFine(db, f) {
  return tx(db, () => {
    f = { ...f, trndate: assertPostableDate(db, { company: f.company, year: f.year, date: f.trndate }).date };
    const trnno = f.trnno || nextTrnNo(db, 'fine_details', f.company, f.year, 'trnno');
    const amount = money(f.amount || 0);
    run(db,
      `INSERT INTO fine_details (companyCode, yearCode, code, trndate, amount, naration, type, trnno, trnid)
       VALUES (?,?,?,?,?,?,?,?,?)`,
      [f.company, f.year, f.code || 'F1', f.trndate, amount, f.narration || null, f.type || 'Excise', trnno, String(f.trnid || 0)]);
    postVoucher(db, {
      company: f.company, year: f.year, trnno: `FINE${trnno}`, trndate: f.trndate, type: 'EXCISE FINE',
      lines: [
        { ledcode: f.expense_ledger || 'L00010', debit: amount, narr: f.narration || 'Excise fine' },
        { ledcode: f.payment_ledger || 'L00001', credit: amount, narr: f.narration || 'Excise fine' },
      ],
    });
    return { trnno, amount };
  });
}

export function listFines(db, company, year) {
  return all(db, 'SELECT * FROM fine_details WHERE companyCode=? AND yearCode=? ORDER BY trndate DESC', [company, year]);
}

// ---------------------------------------------------------------- BRS
export function addReconciliation(db, r) {
  r = { ...r, trndate: assertPostableDate(db, { company: r.company, year: r.year, date: r.trndate }).date };
  const row = get(db, 'SELECT COALESCE(MAX(slno),0) AS n FROM TransactionReconciliation WHERE companycode=? AND yearcode=?', [r.company, r.year]);
  const slno = (row?.n || 0) + 1;
  run(db,
    `INSERT INTO TransactionReconciliation (companycode, yearcode, slno, ledcode, trndate, chqno, chqdate, amount, cleared, cleareddate)
     VALUES (?,?,?,?,?,?,?,?,?,?)`,
    [r.company, r.year, slno, r.ledcode, r.trndate, r.chqno || null, r.chqdate || null, money(r.amount || 0), r.cleared ? 1 : 0, r.cleareddate || null]);
  return { slno };
}

export function listReconciliation(db, company, year, ledcode) {
  const params = [company, year];
  let sql = 'SELECT * FROM TransactionReconciliation WHERE companycode=? AND yearcode=?';
  if (ledcode) { sql += ' AND ledcode=?'; params.push(ledcode); }
  sql += ' ORDER BY trndate';
  return all(db, sql, params);
}

export function clearReconciliation(db, company, year, slno, cleareddate) {
  run(db, 'UPDATE TransactionReconciliation SET cleared=1, cleareddate=? WHERE companycode=? AND yearcode=? AND slno=?',
    [cleareddate, company, year, slno]);
  return { slno, cleared: true };
}

// ---------------------------------------------------------------- UPI / QR
export function recordUpi(db, u) {
  run(db,
    `INSERT INTO tbl_UPIRespose (companycode, trnno, trndate, amount, upiid, status, refno)
     VALUES (?,?,?,?,?,?,?)
     ON CONFLICT(companycode, trnno) DO UPDATE SET status=excluded.status, refno=excluded.refno, amount=excluded.amount`,
    [u.company, u.trnno, u.trndate, money(u.amount || 0), u.upiid || null, u.status || 'PENDING', u.refno || null]);
  return get(db, 'SELECT * FROM tbl_UPIRespose WHERE companycode=? AND trnno=?', [u.company, u.trnno]);
}

export function listUpi(db, company) {
  return all(db, 'SELECT * FROM tbl_UPIRespose WHERE companycode=? ORDER BY trndate DESC', [company]);
}

// ---------------------------------------------------------------- delivery
export function createDelivery(db, d) {
  d = { ...d, trndate: assertPostableDate(db, { company: d.company, year: d.year, date: d.trndate }).date };
  const orderno = d.orderno || `DLV${String(nextTrnNo(db, 'delivery_orders', d.company, d.year, 'orderno')).padStart(5, '0')}`;
  run(db,
    `INSERT INTO delivery_orders (companycode, yearcode, orderno, trndate, custcode, address, phone, amount, status, narration)
     VALUES (?,?,?,?,?,?,?,?,?,?)`,
    [d.company, d.year, orderno, d.trndate, d.custcode || null, d.address || null, d.phone || null, money(d.amount || 0), d.status || 'OPEN', d.narration || null]);
  return { orderno };
}

export function listDeliveries(db, company, year) {
  return all(db, 'SELECT * FROM delivery_orders WHERE companycode=? AND yearcode=? ORDER BY trndate DESC', [company, year]);
}

export function setDeliveryStatus(db, company, year, orderno, status) {
  run(db, 'UPDATE delivery_orders SET status=? WHERE companycode=? AND yearcode=? AND orderno=?', [status, company, year, orderno]);
  return { orderno, status };
}
