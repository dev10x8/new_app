import { test } from 'node:test';
import assert from 'node:assert/strict';
import { mkdtempSync, rmSync } from 'node:fs';
import { tmpdir } from 'node:os';
import { join } from 'node:path';
import { openDatabase, get } from '../src/db/index.js';
import { MASTERS } from '../src/services/masters.js';
import * as txn from '../src/services/transactions.js';
import * as admin from '../src/services/admin.js';
import * as tools from '../src/services/tools.js';
import * as tally from '../src/services/tally.js';
import * as sales from '../src/services/sales.js';
import * as stock from '../src/services/stock.js';
import { CATALOG, reportGroups, runCatalogReport } from '../src/services/reportCatalog.js';
import { trialBalance } from '../src/services/accounting.js';

const COMPANY = '1';
const YEAR = '2024';

function freshDb() {
  const dir = mkdtempSync(join(tmpdir(), 'madhushala-ext-'));
  return { db: openDatabase(join(dir, 'test.db')), dir };
}

test('extended schema adds all new tables', () => {
  const { db, dir } = freshDb();
  try {
    const names = ['countermst', 'customerDetails', 'tblArea', 'ManufacturerMaster',
      'naiUnitMaster', 'naiItemMainGroup', 'naiItemSubGroup', 'naiItemMaster',
      'tbl_scheme', 'offermst', 'ExciseItemMaster', 'transfer_sale_main',
      'transfer_receive_main', 'notes_main', 'onaccount_adjustment', 'account_replace',
      'TransactionReconciliation', 'tbl_UPIRespose', 'delivery_orders',
      'DeleteSalesBillMain', 'tbl_MessageLog', 'tbl_UploadDownloadStatus',
      'TBL_Audit', 'tbl_PML', 'tbl_PMLCAL'];
    for (const n of names) {
      const r = db.prepare("SELECT name FROM sqlite_master WHERE type='table' AND name=?").get(n);
      assert.ok(r, `missing table ${n}`);
    }
    const total = db.prepare("SELECT COUNT(*) AS n FROM sqlite_master WHERE type='table'").get();
    assert.ok(total.n >= 78, `expected >=78 tables, got ${total.n}`);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('every registered master resolves to an existing table', () => {
  const { db, dir } = freshDb();
  try {
    for (const [name, def] of Object.entries(MASTERS)) {
      const t = get(db, "SELECT name FROM sqlite_master WHERE type='table' AND name=?", [def.table]);
      assert.ok(t, `master ${name} -> table ${def.table} does not exist`);
    }
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('transfer sale and receive are recorded', () => {
  const { db, dir } = freshDb();
  try {
    const s = txn.createTransferSale(db, {
      company: COMPANY, year: YEAR, trndate: '2024-04-06', from: 'MAIN', to: 'CNT1',
      lines: [{ itemcode: 'I00001', qnty: 5 }],
    });
    assert.equal(s.trnno, 1);
    assert.equal(s.amount, 1250);
    assert.equal(txn.listTransferSales(db, COMPANY, YEAR).length, 1);

    const r = txn.createTransferReceive(db, {
      company: COMPANY, year: YEAR, trndate: '2024-04-06', from: 'MAIN', to: 'CNT1',
      lines: [{ itemcode: 'I00001', qnty: 5 }],
    });
    assert.equal(r.trnno, 1);
    assert.equal(txn.listTransferReceives(db, COMPANY, YEAR).length, 1);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('debit and credit notes post balanced vouchers', () => {
  const { db, dir } = freshDb();
  try {
    const dn = txn.createNote(db, {
      company: COMPANY, year: YEAR, trndate: '2024-04-06', type: 'DEBIT',
      partycode: 'S00001', headaccount: 'L00004', amount: 100,
    });
    assert.equal(dn.noteno, 'DN00001');
    const cn = txn.createNote(db, {
      company: COMPANY, year: YEAR, trndate: '2024-04-07', type: 'CREDIT',
      partycode: 'C00001', headaccount: 'L00003', amount: 60,
    });
    assert.equal(cn.noteno, 'CN00001');
    assert.equal(txn.listNotes(db, COMPANY, YEAR).length, 2);
    assert.equal(txn.listNotes(db, COMPANY, YEAR, 'DEBIT').length, 1);
    assert.equal(trialBalance(db, COMPANY, YEAR).balanced, true);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('on-a/c adjustment and a/c replace post balanced vouchers', () => {
  const { db, dir } = freshDb();
  try {
    txn.createOnAccountAdjustment(db, { company: COMPANY, year: YEAR, trndate: '2024-04-06', partycode: 'S00001', amount: 300 });
    txn.createAccountReplace(db, { company: COMPANY, year: YEAR, trndate: '2024-04-06', from_ledger: 'L00003', to_ledger: 'L00004', amount: 150 });
    assert.equal(trialBalance(db, COMPANY, YEAR).balanced, true);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('excise fine posts a balanced voucher', () => {
  const { db, dir } = freshDb();
  try {
    const f = txn.createFine(db, { company: COMPANY, year: YEAR, trndate: '2024-04-06', amount: 500, narration: 'penalty' });
    assert.equal(f.trnno, 1);
    assert.equal(txn.listFines(db, COMPANY, YEAR).length, 1);
    assert.equal(trialBalance(db, COMPANY, YEAR).balanced, true);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('bank reconciliation add and clear', () => {
  const { db, dir } = freshDb();
  try {
    const a = txn.addReconciliation(db, { company: COMPANY, year: YEAR, ledcode: 'L00002', trndate: '2024-04-06', chqno: '123456', amount: 5000 });
    assert.equal(a.slno, 1);
    txn.clearReconciliation(db, COMPANY, YEAR, 1, '2024-04-08');
    const rows = txn.listReconciliation(db, COMPANY, YEAR);
    assert.equal(rows[0].cleared, 1);
    assert.equal(rows[0].cleareddate, '2024-04-08');
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('UPI payment is upserted by bill', () => {
  const { db, dir } = freshDb();
  try {
    txn.recordUpi(db, { company: COMPANY, trnno: 'FL00001', trndate: '2024-04-06', amount: 900, upiid: 'a@upi', status: 'PENDING' });
    txn.recordUpi(db, { company: COMPANY, trnno: 'FL00001', trndate: '2024-04-06', amount: 900, upiid: 'a@upi', status: 'SUCCESS' });
    const rows = txn.listUpi(db, COMPANY);
    assert.equal(rows.length, 1);
    assert.equal(rows[0].status, 'SUCCESS');
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('delivery order lifecycle', () => {
  const { db, dir } = freshDb();
  try {
    const d = txn.createDelivery(db, { company: COMPANY, year: YEAR, trndate: '2024-04-06', custcode: 'CU0002', address: 'Park St', amount: 900 });
    assert.equal(d.orderno, 'DLV00001');
    txn.setDeliveryStatus(db, COMPANY, YEAR, 'DLV00001', 'DELIVERED');
    const rows = txn.listDeliveries(db, COMPANY, YEAR);
    assert.equal(rows[0].status, 'DELIVERED');
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('deleted sale is archived with detail and audit row', () => {
  const { db, dir } = freshDb();
  try {
    stock.addOpeningStock(db, { company: COMPANY, year: YEAR, shopcode: 'MAIN', itemcode: 'I00001', qnty: 50, trndate: '2024-04-01' });
    sales.createSale(db, { company: COMPANY, year: YEAR, trndate: '2024-04-06', storecode: 'MAIN', salestype: 'FL', ledgercode: 'C00001', lines: [{ itemcode: 'I00001', qnty: 4, rate: 250 }] });
    sales.deleteSale(db, COMPANY, YEAR, 'FL00001');
    const archived = sales.listDeletedBills(db, COMPANY, YEAR);
    assert.equal(archived.length, 1);
    assert.equal(archived[0].trnno, 'FL00001');
    const detail = get(db, 'SELECT COUNT(*) AS n FROM DeletedSalesbilldetail WHERE companycode=? AND trnno=?', [COMPANY, 'FL00001']);
    assert.equal(detail.n, 1);
    const audit = get(db, "SELECT COUNT(*) AS n FROM TBL_Audit WHERE action='DELETE'");
    assert.equal(audit.n, 1);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('new year carries forward ledger balances and opening stock', () => {
  const { db, dir } = freshDb();
  try {
    stock.addOpeningStock(db, { company: COMPANY, year: YEAR, shopcode: 'MAIN', itemcode: 'I00001', qnty: 80, trndate: '2024-04-01' });
    txn.createFine(db, { company: COMPANY, year: YEAR, trndate: '2024-04-06', amount: 250, narration: 'penalty' });
    const r = admin.createNewYear(db, { company: COMPANY, yearcode: '2025', yearrange: '2025-2026', stdate: '2025-04-01', enddate: '2026-03-31', fromYear: YEAR });
    assert.equal(r.yearcode, '2025');
    assert.ok(r.carriedLedgers >= 1, 'should carry ledger balances');
    assert.ok(r.carriedStock >= 1, 'should carry opening stock');
    const opened = get(db, 'SELECT COALESCE(SUM(qnty),0) AS q FROM openingstockmst WHERE companycode=? AND yearcode=?', [COMPANY, '2025']);
    assert.equal(opened.q, 80);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('parameters can be read and updated', () => {
  const { db, dir } = freshDb();
  try {
    const p = admin.getParameters(db, COMPANY);
    assert.ok(p.companycode);
    admin.updateParameters(db, COMPANY, { bill_footer: 'Thanks!' });
    assert.equal(admin.getParameters(db, COMPANY).bill_footer, 'Thanks!');
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('system info reports version and flags', () => {
  const { db, dir } = freshDb();
  try {
    const s = admin.systemInfo(db, COMPANY);
    assert.equal(s.version, '1.0.0-new');
    assert.equal(typeof s.posMode, 'boolean');
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('excise audit joins excise items with stock', () => {
  const { db, dir } = freshDb();
  try {
    stock.addOpeningStock(db, { company: COMPANY, year: YEAR, shopcode: 'MAIN', itemcode: 'I00001', qnty: 40, trndate: '2024-04-01' });
    const rows = admin.exciseAudit(db, COMPANY, YEAR);
    assert.ok(rows.length >= 1);
    assert.equal(rows[0].excisecode, 'EX001');
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('tools: calculator, messages, sync, PML, clean print', () => {
  const { db, dir } = freshDb();
  try {
    assert.equal(tools.calculate(12, '*', 5), 60);
    assert.throws(() => tools.calculate(1, '/', 0), /Division by zero/);

    tools.sendMessage(db, { company: COMPANY, channel: 'SMS', recipient: '9800000000', message: 'hi' });
    assert.equal(tools.listMessages(db, COMPANY).length, 1);

    const sync = tools.dataSync(db, { company: COMPANY, entity: 'SALES', direction: 'UPLOAD' });
    assert.equal(sync.status, 'SUCCESS');
    assert.equal(tools.syncStatus(db, COMPANY).length, 1);
    tools.dataSync(db, { company: COMPANY, entity: 'SALES', direction: 'UPLOAD' });
    assert.equal(tools.syncStatus(db, COMPANY).length, 1, 'sync should upsert');

    const pml = tools.addPml(db, { company: COMPANY, year: YEAR, trndate: '2024-04-06', ml: 750, bottles: 10 });
    assert.equal(pml.totml, 750);
    tools.addPml(db, { company: COMPANY, year: YEAR, trndate: '2024-04-07', ml: 1000, bottles: 4 });
    assert.equal(tools.listPml(db, COMPANY, YEAR).length, 2);
    const totals = get(db, 'SELECT totml, totbottles FROM tbl_PMLCAL WHERE companycode=? AND yearcode=?', [COMPANY, YEAR]);
    assert.equal(totals.totml, 1750);
    assert.equal(totals.totbottles, 14);

    const cleaned = tools.cleanPrintGarbage(db, COMPANY);
    assert.equal(cleaned.messagesCleared, 1);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('backup copies the database file', () => {
  const { db, dir } = freshDb();
  try {
    const dest = join(dir, 'backup.db');
    const r = tools.backupDatabase(join(dir, 'test.db'), dest);
    assert.ok(r.size > 0);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('report catalog covers every legacy group with the expected counts', () => {
  const groups = reportGroups();
  assert.equal(groups.sales.length, 32);
  assert.equal(groups.stock.length, 20);
  assert.equal(groups.purchase.length, 10);
  assert.equal(groups.accounts.length, 14);
  assert.equal(groups.mis.length, 12);
  assert.equal(groups.breakage.length, 2);
  assert.equal(groups.transfer.length, 2);
  assert.equal(groups.excise.length, 21);
  assert.equal(Object.keys(CATALOG).length, 113);
});

test('every catalog report executes against a seeded database', () => {
  const { db, dir } = freshDb();
  try {
    for (const key of Object.keys(CATALOG)) {
      const rows = runCatalogReport(db, key, { company: COMPANY, year: YEAR });
      assert.ok(Array.isArray(rows), `${key} should return an array`);
    }
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('tally sync: config, auto-enqueue on sale, xml, ack, retry', () => {
  const { db, dir } = freshDb();
  try {
    // disabled by default -> sales are not queued
    sales.createSale(db, {
      company: COMPANY, year: YEAR, trndate: '2024-04-06', storecode: 'CNT1',
      salestype: 'FL', ledgercode: 'C00001', lines: [{ itemcode: 'I00001', qnty: 2, rate: 250 }],
    });
    assert.equal(tally.queue(db, COMPANY).length, 0, 'no queueing while disabled');

    const cfg = tally.saveConfig(db, COMPANY, { enabled: true, tallycompany: 'Madhushala Co', agenttoken: 'TOK1' });
    assert.equal(cfg.enabled, 1);
    assert.equal(cfg.tallycompany, 'Madhushala Co');

    // a sale made while enabled is queued automatically
    sales.createSale(db, {
      company: COMPANY, year: YEAR, trndate: '2024-04-06', storecode: 'CNT1',
      salestype: 'FL', ledgercode: 'C00001', lines: [{ itemcode: 'I00002', qnty: 1, rate: 320 }],
    });
    const q = tally.queue(db, COMPANY);
    assert.equal(q.length, 1);
    assert.equal(q[0].vouchertype, 'SALES');
    assert.equal(q[0].status, 'PENDING');
    assert.equal(tally.queueSummary(db, COMPANY).PENDING, 1);

    // mapping + xml
    tally.addMapping(db, { company: COMPANY, maptype: 'LEDGER', pattern: 'C00001', targetname: 'Cash', priority: 1 });
    assert.equal(tally.resolveMapping(db, COMPANY, 'LEDGER', 'C00001'), 'Cash');
    const xml = tally.buildVoucherXmlBatch(db, COMPANY, YEAR);
    assert.equal(xml.count, 1);
    assert.match(xml.xml, /<VOUCHER /);
    assert.match(xml.xml, /<LEDGERNAME>Cash<\/LEDGERNAME>/);

    // agent marks sent, then acks
    tally.markSent(db, COMPANY, xml.ids);
    assert.equal(tally.queueSummary(db, COMPANY).SENT, 1);
    tally.acknowledge(db, COMPANY, [{ id: xml.ids[0], status: 'ACKED' }]);
    assert.equal(tally.queueSummary(db, COMPANY).ACKED, 1);

    // failure + retry path
    const q2 = tally.queue(db, COMPANY)[0];
    tally.markSent(db, COMPANY, [q2.id]);
    tally.acknowledge(db, COMPANY, [{ id: q2.id, status: 'FAILED', message: 'company not loaded' }]);
    assert.equal(tally.queueSummary(db, COMPANY).FAILED, 1);
    assert.equal(tally.retryFailed(db, COMPANY).retried, 1);
    assert.equal(tally.queueSummary(db, COMPANY).PENDING, 1);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('tally sync: inbound masters are received and applied', () => {
  const { db, dir } = freshDb();
  try {
    tally.saveConfig(db, COMPANY, { enabled: true, agenttoken: 'TOK1' });
    tally.receiveMaster(db, COMPANY, { type: 'LEDGER', name: 'SBI Bank', code: 'L90001', parent: 'AC0001' });
    tally.receiveMaster(db, COMPANY, { type: 'STOCKITEM', name: 'Royal Stag 750ml', code: 'I90001', rate: 255 });
    assert.equal(tally.listInbound(db, COMPANY).length, 2);

    const rows = tally.listInbound(db, COMPANY);
    for (const r of rows) tally.applyInbound(db, COMPANY, r.id);
    assert.equal(tally.listInbound(db, COMPANY, 'APPLIED').length, 2);
    const led = get(db, 'SELECT * FROM ledger WHERE companycode=? AND ledcode=?', [COMPANY, 'L90001']);
    assert.equal(led.name, 'SBI Bank');
    const item = get(db, 'SELECT * FROM itemmst WHERE companycode=? AND itemcode=?', [COMPANY, 'I90001']);
    assert.equal(item.salesrate, 255);

    // re-receive updates instead of duplicating
    tally.receiveMaster(db, COMPANY, { type: 'LEDGER', name: 'SBI Bank Ltd', code: 'L90001' });
    assert.equal(tally.listInbound(db, COMPANY).length, 2);
    assert.equal(tally.listInbound(db, COMPANY, 'RECEIVED').length, 1);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('tally sync: token guard and sync report', () => {
  const { db, dir } = freshDb();
  try {
    const cfg = tally.saveConfig(db, COMPANY, { enabled: true, agenttoken: 'SECRET' });
    assert.doesNotThrow(() => tally.requireToken(cfg, 'SECRET'));
    assert.throws(() => tally.requireToken(cfg, 'nope'), /Invalid agent token/);

    sales.createSale(db, {
      company: COMPANY, year: YEAR, trndate: '2024-04-06', storecode: 'CNT1',
      salestype: 'FL', ledgercode: 'C00001', lines: [{ itemcode: 'I00001', qnty: 4, rate: 250 }],
    });
    const rep = tally.syncReport(db, COMPANY, YEAR);
    assert.equal(rep.summary.PENDING, 1);
    assert.ok(rep.logs.length >= 1);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});
