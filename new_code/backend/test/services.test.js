import { test } from 'node:test';
import assert from 'node:assert/strict';
import { mkdtempSync, rmSync } from 'node:fs';
import { tmpdir } from 'node:os';
import { join } from 'node:path';
import { openDatabase } from '../src/db/index.js';
import * as auth from '../src/services/auth.js';
import * as masters from '../src/services/masters.js';
import * as sales from '../src/services/sales.js';
import * as purchase from '../src/services/purchase.js';
import * as stock from '../src/services/stock.js';
import * as vouchers from '../src/services/vouchers.js';
import * as reports from '../src/services/reports.js';
import { trialBalance } from '../src/services/accounting.js';

const COMPANY = '1';
const YEAR = '2024';

function freshDb() {
  const dir = mkdtempSync(join(tmpdir(), 'vanijliquor-'));
  const file = join(dir, 'test.db');
  const db = openDatabase(file);
  return { db, file, dir };
}

test('database seeds 45 tables, admin user and bootstrap data', () => {
  const { db, dir } = freshDb();
  try {
    const tables = db.prepare("SELECT COUNT(*) AS n FROM sqlite_master WHERE type='table'").get();
    assert.ok(tables.n >= 45, `expected >=45 tables, got ${tables.n}`);
    const admin = db.prepare("SELECT * FROM users WHERE usercode='A00001'").get();
    assert.equal(admin.password, 'admin');
    assert.equal(admin.accesslevel, 'Admin');
    const items = db.prepare('SELECT COUNT(*) AS n FROM itemmst').get();
    assert.equal(items.n, 3);
  } finally {
    db.close(); rmSync(dir, { recursive: true, force: true });
  }
});

test('login accepts correct PIN and rejects wrong PIN', () => {
  const { db, dir } = freshDb();
  try {
    assert.equal(auth.login(db, { usercode: 'A00001', password: 'admin' }).ok, true);
    assert.equal(auth.login(db, { usercode: 'A00001', password: 'wrong' }).ok, false);
    assert.equal(auth.login(db, { usercode: 'ZZZZ', password: 'admin' }).ok, false);
  } finally {
    db.close(); rmSync(dir, { recursive: true, force: true });
  }
});

test('admin PIN can be reset and used to log in', () => {
  const { db, dir } = freshDb();
  try {
    auth.resetPin(db, 'A00001', '4321');
    assert.equal(auth.login(db, { usercode: 'A00001', password: 'admin' }).ok, false);
    const r = auth.login(db, { usercode: 'A00001', password: '4321' });
    assert.equal(r.ok, true);
    assert.equal(r.user.admin, true);
  } finally {
    db.close(); rmSync(dir, { recursive: true, force: true });
  }
});

test('purchase increases stock on hand', () => {
  const { db, dir } = freshDb();
  try {
    purchase.createPurchase(db, {
      company: COMPANY, year: YEAR, trndate: '2024-04-05', ptype: 'PURCHASE',
      suppliercode: 'S00001', purchaseacccode: 'L00004', shopcode: 'MAIN', docno: 'INV-1',
      lines: [{ itemcode: 'I00001', batchno: 'B1', itemrate: 200, itemquantity: 120, itemmrp: 250 }],
    });
    const rows = stock.stockOnHand(db, { company: COMPANY, year: YEAR, storecode: 'MAIN' });
    const row = rows.find((r) => r.itemcode === 'I00001');
    assert.equal(row.qty, 120);
  } finally {
    db.close(); rmSync(dir, { recursive: true, force: true });
  }
});

test('POS sale posts bill, lines and balanced accounting', () => {
  const { db, dir } = freshDb();
  try {
    purchase.createPurchase(db, {
      company: COMPANY, year: YEAR, trndate: '2024-04-05', ptype: 'PURCHASE',
      suppliercode: 'S00001', purchaseacccode: 'L00004', shopcode: 'MAIN', docno: 'INV-1',
      lines: [{ itemcode: 'I00001', batchno: 'B1', itemrate: 200, itemquantity: 120, itemmrp: 250 }],
    });
    const res = sales.createSale(db, {
      company: COMPANY, year: YEAR, trndate: '2024-04-06', storecode: 'CNT1',
      salestype: 'FL', ledgercode: 'C00001', discount: 10,
      lines: [{ itemcode: 'I00001', qnty: 12, rate: 250 }],
    });
    assert.equal(res.trnno, 'FL00001');
    assert.equal(res.amount, 3000);
    assert.equal(res.netamount, 2990);

    const sale = sales.getSale(db, COMPANY, YEAR, 'FL00001');
    assert.equal(sale.lines.length, 1);
    assert.equal(sale.lines[0].qnty, 12);

    const tb = trialBalance(db, COMPANY, YEAR);
    assert.equal(tb.balanced, true, `trial balance off: Dr ${tb.totalDebit} Cr ${tb.totalCredit}`);
  } finally {
    db.close(); rmSync(dir, { recursive: true, force: true });
  }
});

test('sale reduces stock; day close stamps bills', () => {
  const { db, dir } = freshDb();
  try {
    purchase.createPurchase(db, {
      company: COMPANY, year: YEAR, trndate: '2024-04-05', ptype: 'PURCHASE',
      suppliercode: 'S00001', purchaseacccode: 'L00004', shopcode: 'MAIN', docno: 'INV-1',
      lines: [{ itemcode: 'I00001', batchno: 'B1', itemrate: 200, itemquantity: 120, itemmrp: 250 }],
    });
    sales.createSale(db, {
      company: COMPANY, year: YEAR, trndate: '2024-04-06', storecode: 'MAIN',
      salestype: 'FL', ledgercode: 'C00001', lines: [{ itemcode: 'I00001', qnty: 20, rate: 250 }],
    });
    const rows = stock.stockOnHand(db, { company: COMPANY, year: YEAR, storecode: 'MAIN' });
    assert.equal(rows.find((r) => r.itemcode === 'I00001').qty, 100);

    const closed = sales.dayClose(db, COMPANY, YEAR, { from: '2024-04-01', to: '2024-04-30' });
    assert.equal(closed.closed, 1);
    const again = sales.dayClose(db, COMPANY, YEAR, { from: '2024-04-01', to: '2024-04-30' });
    assert.equal(again.closed, 0, 'already-closed bills must not be re-closed');
  } finally {
    db.close(); rmSync(dir, { recursive: true, force: true });
  }
});

test('stock transfer moves quantity between stores', () => {
  const { db, dir } = freshDb();
  try {
    stock.addOpeningStock(db, { company: COMPANY, year: YEAR, shopcode: 'MAIN', itemcode: 'I00001', qnty: 100, trndate: '2024-04-01' });
    stock.stockTransfer(db, {
      company: COMPANY, year: YEAR, trnno: 1, trndate: '2024-04-08', from: 'MAIN', to: 'CNT1',
      lines: [{ itemcode: 'I00001', qnty: 30 }],
    });
    const rows = stock.stockOnHand(db, { company: COMPANY, year: YEAR });
    assert.equal(rows.find((r) => r.storecode === 'MAIN' && r.itemcode === 'I00001').qty, 70);
    assert.equal(rows.find((r) => r.storecode === 'CNT1' && r.itemcode === 'I00001').qty, 30);
  } finally {
    db.close(); rmSync(dir, { recursive: true, force: true });
  }
});

test('breakage reduces stock', () => {
  const { db, dir } = freshDb();
  try {
    stock.addOpeningStock(db, { company: COMPANY, year: YEAR, shopcode: 'MAIN', itemcode: 'I00001', qnty: 50, trndate: '2024-04-01' });
    stock.addBreakage(db, {
      company: COMPANY, year: YEAR, trnno: 1, trndate: '2024-04-08', shopcode: 'MAIN',
      lines: [{ itemcode: 'I00001', quantity: 5 }],
    });
    const rows = stock.stockOnHand(db, { company: COMPANY, year: YEAR, storecode: 'MAIN' });
    assert.equal(rows.find((r) => r.itemcode === 'I00001').qty, 45);
  } finally {
    db.close(); rmSync(dir, { recursive: true, force: true });
  }
});

test('payment voucher posts and keeps books balanced', () => {
  const { db, dir } = freshDb();
  try {
    const r = vouchers.createPaymentReceipt(db, {
      company: COMPANY, year: YEAR, type: 'PAYMENT', vchdate: '2024-04-07',
      head_account: 'L00002', client_account: 'S00001', amount_paid: 20000, narration: 'Paid supplier',
    });
    assert.equal(r.vchno, 1);
    const tb = trialBalance(db, COMPANY, YEAR);
    assert.equal(tb.balanced, true);
  } finally {
    db.close(); rmSync(dir, { recursive: true, force: true });
  }
});

test('unbalanced journal is rejected', () => {
  const { db, dir } = freshDb();
  try {
    assert.throws(() => vouchers.createJournal(db, {
      company: COMPANY, year: YEAR, vchdate: '2024-04-07',
      lines: [{ ledcode: 'L00001', debit: 100, credit: 0 }, { ledcode: 'L00003', debit: 0, credit: 90 }],
    }), /not balanced/);
  } finally {
    db.close(); rmSync(dir, { recursive: true, force: true });
  }
});

test('reports return rows for sales, purchase, stock and accounts', () => {
  const { db, dir } = freshDb();
  try {
    purchase.createPurchase(db, {
      company: COMPANY, year: YEAR, trndate: '2024-04-05', ptype: 'PURCHASE',
      suppliercode: 'S00001', purchaseacccode: 'L00004', shopcode: 'MAIN', docno: 'INV-1',
      lines: [{ itemcode: 'I00001', batchno: 'B1', itemrate: 200, itemquantity: 120, itemmrp: 250 }],
    });
    sales.createSale(db, {
      company: COMPANY, year: YEAR, trndate: '2024-04-06', storecode: 'CNT1',
      salestype: 'FL', ledgercode: 'C00001', lines: [{ itemcode: 'I00001', qnty: 12, rate: 250 }],
    });
    const opts = { company: COMPANY, year: YEAR };
    assert.equal(reports.runReport(db, 'sales.bill', opts).length, 1);
    assert.equal(reports.runReport(db, 'sales.item', opts)[0].qty, 12);
    assert.equal(reports.runReport(db, 'purchase.bill', opts).length, 1);
    assert.ok(reports.runReport(db, 'stock.closing', opts).length >= 1);
    assert.ok(reports.runReport(db, 'accounts.trial', opts).length >= 1);
    assert.ok(reports.runReport(db, 'mis.profit', opts).length >= 1);
    const dash = reports.dashboard(db, COMPANY, YEAR);
    assert.equal(dash.sales.bills, 1);
  } finally {
    db.close(); rmSync(dir, { recursive: true, force: true });
  }
});

test('masters CRUD round-trips', () => {
  const { db, dir } = freshDb();
  try {
    masters.createMaster(db, 'category', { companycode: '1', categorycode: 'C9', categoryname: 'Gin' });
    const found = masters.getMaster(db, 'category', { companycode: '1', categorycode: 'C9' });
    assert.equal(found.categoryname, 'Gin');
    masters.updateMaster(db, 'category', { companycode: '1', categorycode: 'C9' }, { categoryname: 'Premium Gin' });
    assert.equal(masters.getMaster(db, 'category', { companycode: '1', categorycode: 'C9' }).categoryname, 'Premium Gin');
    masters.deleteMaster(db, 'category', { companycode: '1', categorycode: 'C9' });
    assert.equal(masters.getMaster(db, 'category', { companycode: '1', categorycode: 'C9' }), undefined);
  } finally {
    db.close(); rmSync(dir, { recursive: true, force: true });
  }
});

test('sale return reverses accounting and restores stock', () => {
  const { db, dir } = freshDb();
  try {
    stock.addOpeningStock(db, { company: COMPANY, year: YEAR, shopcode: 'MAIN', itemcode: 'I00001', qnty: 100, trndate: '2024-04-01' });
    sales.createSale(db, {
      company: COMPANY, year: YEAR, trndate: '2024-04-06', storecode: 'MAIN',
      salestype: 'FL', ledgercode: 'C00001', lines: [{ itemcode: 'I00001', qnty: 10, rate: 250 }],
    });
    sales.createSale(db, {
      company: COMPANY, year: YEAR, trndate: '2024-04-07', storecode: 'MAIN',
      salestype: 'RETURN', ledgercode: 'C00001', lines: [{ itemcode: 'I00001', qnty: 4, rate: 250 }],
    });
    const rows = stock.stockOnHand(db, { company: COMPANY, year: YEAR, storecode: 'MAIN' });
    assert.equal(rows.find((r) => r.itemcode === 'I00001').qty, 94);
    assert.equal(trialBalance(db, COMPANY, YEAR).balanced, true);
  } finally {
    db.close(); rmSync(dir, { recursive: true, force: true });
  }
});
