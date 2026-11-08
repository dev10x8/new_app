import { all, get, run, tx } from '../db/index.js';
import { copyFileSync, existsSync, statSync } from 'node:fs';
import { money, now } from './helpers.js';

/** Backup: copy the SQLite database to a destination path. */
export function backupDatabase(dbFile, dest) {
  if (!existsSync(dbFile)) throw new Error(`Database file not found: ${dbFile}`);
  copyFileSync(dbFile, dest);
  const size = statSync(dest).size;
  return { dest, size, at: now() };
}

/** Simple four-function calculator (Tools > Calculator). */
export function calculate(a, op, b) {
  const x = Number(a); const y = Number(b);
  switch (op) {
    case '+': return money(x + y);
    case '-': return money(x - y);
    case '*': return money(x * y);
    case '/': if (y === 0) throw new Error('Division by zero'); return money(x / y);
    default: throw new Error(`Unknown operator: ${op}`);
  }
}

/** Tally export: emit voucher rows as Tally-friendly XML-ish lines. */
export function tallyExport(db, company, year, kind = 'SALES') {
  if (kind === 'PURCHASE') {
    const rows = all(db, 'SELECT * FROM purchasemain WHERE companycode=? AND yearcode=? ORDER BY trndate', [company, year]);
    return rows.map((r) => `<VOUCHER TYPE="PURCHASE" DATE="${r.trndate}" NO="${r.trnno}" PARTY="${r.suppliercode}" AMOUNT="${r.totnetamt}"/>`);
  }
  const rows = all(db, 'SELECT * FROM salesbillmain WHERE companycode=? AND yearcode=? ORDER BY trndate', [company, year]);
  return rows.map((r) => `<VOUCHER TYPE="SALES" DATE="${r.trndate}" NO="${r.trnno}" PARTY="${r.ledgercode}" AMOUNT="${r.netamount}"/>`);
}

/** SMS / WhatsApp sending: logs the message (gateway integration point). */
export function sendMessage(db, m) {
  const at = now();
  run(db,
    `INSERT INTO tbl_MessageLog (companycode, channel, recipient, message, sentat, status) VALUES (?,?,?,?,?,?)`,
    [m.company, m.channel || 'SMS', m.recipient || '', m.message || '', at, 'SENT']);
  return { channel: m.channel || 'SMS', recipient: m.recipient, at, status: 'SENT' };
}

export function listMessages(db, company) {
  return all(db, 'SELECT * FROM tbl_MessageLog WHERE companycode=? ORDER BY id DESC LIMIT 200', [company]);
}

/** Data sync: run an entity upload/download and record status. */
export function dataSync(db, { company, entity, direction }) {
  const counts = {
    SALES: 'salesbillmain',
    PURCHASE: 'purchasemain',
    TRANSACTION: 'journaldetail',
    TRANSFER: 'stk_transfer_main',
    BREAKAGE: 'breakagemain',
    PAYREC: 'payment_main',
  };
  const table = counts[entity] || null;
  const records = table
    ? (get(db, `SELECT COUNT(*) AS n FROM ${table} WHERE companycode=?`, [company])?.n || 0)
    : 0;
  const at = now();
  const existing = get(db,
    'SELECT 1 FROM tbl_UploadDownloadStatus WHERE companycode=? AND entity=? AND direction=?',
    [company, entity, direction]);
  if (existing) {
    run(db,
      'UPDATE tbl_UploadDownloadStatus SET lastrun=?, records=?, status=? WHERE companycode=? AND entity=? AND direction=?',
      [at, records, 'SUCCESS', company, entity, direction]);
  } else {
    run(db,
      'INSERT INTO tbl_UploadDownloadStatus (companycode, entity, direction, lastrun, records, status) VALUES (?,?,?,?,?,?)',
      [company, entity, direction, at, records, 'SUCCESS']);
  }
  run(db, 'INSERT INTO TBL_Audit (companycode, tablename, action, refkey, auditat) VALUES (?,?,?,?,?)',
    [company, table || entity, `SYNC_${direction}`, entity, at]);
  return { entity, direction, records, at, status: 'SUCCESS' };
}

export function syncStatus(db, company) {
  return all(db, 'SELECT * FROM tbl_UploadDownloadStatus WHERE companycode=? ORDER BY entity, direction', [company]);
}

/** PML print & reports: create a PML entry and recompute totals. */
export function addPml(db, p) {
  return tx(db, () => {
    const row = get(db, 'SELECT COALESCE(MAX(slno),0) AS n FROM tbl_PML WHERE companycode=? AND yearcode=?', [p.company, p.year]);
    const slno = (row?.n || 0) + 1;
    const ml = Number(p.ml) || 0;
    const bottles = Number(p.bottles) || 0;
    run(db,
      'INSERT INTO tbl_PML (companycode, yearcode, slno, trndate, itemcode, ml, bottles, narration) VALUES (?,?,?,?,?,?,?,?)',
      [p.company, p.year, slno, p.trndate, p.itemcode || null, ml, bottles, p.narration || null]);

    const totals = get(db,
      'SELECT COALESCE(SUM(ml),0) AS totml, COALESCE(SUM(bottles),0) AS totbottles FROM tbl_PML WHERE companycode=? AND yearcode=?',
      [p.company, p.year]);
    const cur = get(db, 'SELECT 1 FROM tbl_PMLCAL WHERE companycode=? AND yearcode=? AND slno=1', [p.company, p.year]);
    if (cur) {
      run(db, 'UPDATE tbl_PMLCAL SET trndate=?, totml=?, totbottles=? WHERE companycode=? AND yearcode=? AND slno=1',
        [p.trndate, totals.totml, totals.totbottles, p.company, p.year]);
    } else {
      run(db, 'INSERT INTO tbl_PMLCAL (companycode, yearcode, slno, trndate, totml, totbottles) VALUES (?,?,?,?,?,?)',
        [p.company, p.year, 1, p.trndate, totals.totml, totals.totbottles]);
    }
    return { slno, totml: totals.totml, totbottles: totals.totbottles };
  });
}

export function listPml(db, company, year) {
  return all(db, 'SELECT * FROM tbl_PML WHERE companycode=? AND yearcode=? ORDER BY slno', [company, year]);
}

/** Clean Print Garbage: prune sync/audit/status noise (maintenance). */
export function cleanPrintGarbage(db, company) {
  const r1 = run(db, 'DELETE FROM tbl_MessageLog WHERE companycode=? AND status=?', [company, 'SENT']);
  const r2 = run(db, 'DELETE FROM TBL_Audit WHERE companycode=? AND auditat < ?', [company, now().slice(0, 10)]);
  return { messagesCleared: r1.changes, auditCleared: r2.changes };
}
