import { test } from 'node:test';
import assert from 'node:assert/strict';
import { mkdtempSync, rmSync } from 'node:fs';
import { tmpdir } from 'node:os';
import { join } from 'node:path';
import { openDatabase, get } from '../src/db/index.js';
import * as m22 from '../src/services/m22.js';
import * as excise from '../src/services/excise.js';
import * as purchase from '../src/services/purchase.js';
import * as sales from '../src/services/sales.js';
import * as stock from '../src/services/stock.js';
import { runCatalogReport } from '../src/services/reportCatalog.js';

const COMPANY = '1';
const YEAR = '2024';

function freshDb() {
  const dir = mkdtempSync(join(tmpdir(), 'vanijliquor-m22-'));
  const file = join(dir, 'test.db');
  return { db: openDatabase(file), file, dir };
}

function seed(db) {
  purchase.createPurchase(db, {
    company: COMPANY, year: YEAR, trndate: '2024-04-05', ptype: 'PURCHASE',
    suppliercode: 'S00001', purchaseacccode: 'L00004', shopcode: 'MAIN', docno: 'INV-1',
    lines: [{ itemcode: 'I00001', batchno: 'B1', itemrate: 200, itemquantity: 100, itemmrp: 250 }],
  });
  sales.createSale(db, {
    company: COMPANY, year: YEAR, trndate: '2024-04-06', storecode: 'CNT1',
    salestype: 'FL', ledgercode: 'C00001', lines: [{ itemcode: 'I00001', qnty: 30, rate: 250 }],
  });
  stock.addBreakage(db, {
    company: COMPANY, year: YEAR, trnno: 1, shopcode: 'MAIN', trndate: '2024-04-07',
    narration: 'broken in transit', lines: [{ itemcode: 'I00001', quantity: 2 }],
  });
}

test('Form M-22 tables are created by the migration', () => {
  const { db, file, dir } = freshDb();
  try {
    db.close();
    const db2 = openDatabase(file);
    for (const t of ['tbl_FormM22', 'tbl_FormM22Detail']) {
      assert.ok(get(db2, "SELECT name FROM sqlite_master WHERE type='table' AND name=?", [t]), `${t} should exist`);
    }
    db2.close();
  } finally { rmSync(dir, { recursive: true, force: true }); }
});

test('brand movements reconcile opening + received - sold - breakage = closing', () => {
  const { db, dir } = freshDb();
  try {
    seed(db);
    const rows = m22.brandMovements(db, { company: COMPANY, year: YEAR, from: '2024-04-01', to: '2024-04-30' });
    const brand = rows.find((r) => r.brandcode === 'K00001');
    assert.ok(brand, 'IMFL (K00001) row is present');
    // I00001 is KFL K00001; period opening was 0 (nothing before 2024-04-01)
    assert.equal(brand.openingqty, 0);
    assert.equal(brand.receivedqty, 100);
    assert.equal(brand.soldqty, 30);
    assert.equal(brand.breakageqty, 2);
    assert.equal(brand.closingqty, 68);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('buildFormM22 persists a statement with brand-wise detail and totals', () => {
  const { db, dir } = freshDb();
  try {
    seed(db);
    const doc = m22.buildFormM22(db, { company: COMPANY, year: YEAR, from: '2024-04-01', to: '2024-04-30' });
    assert.equal(doc.status, 'DRAFT');
    assert.equal(doc.receivedqty, 100);
    assert.equal(doc.soldqty, 30);
    assert.equal(doc.breakageqty, 2);
    assert.equal(doc.closingqty, 68);
    assert.ok(doc.details.length >= 1);
    assert.equal(m22.listFormM22(db, COMPANY).length, 1);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('buildFormM22 requires a period', () => {
  const { db, dir } = freshDb();
  try {
    assert.throws(() => m22.buildFormM22(db, { company: COMPANY, year: YEAR }), /period/i);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('fileFormM22 stamps the statement as FILED and it renders as text', () => {
  const { db, dir } = freshDb();
  try {
    seed(db);
    const doc = m22.buildFormM22(db, { company: COMPANY, year: YEAR, from: '2024-04-01', to: '2024-04-30' });
    const filed = m22.fileFormM22(db, COMPANY, doc.id);
    assert.equal(filed.status, 'FILED');
    assert.ok(filed.filedat);
    const text = m22.renderFormM22Text(db, COMPANY, doc.id);
    assert.match(text, /FORM M-22/);
    assert.match(text, /TOTAL/);
    assert.match(text, /Signature of Licensee/);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('the Form M-22 catalog report runs and returns brand rows', () => {
  const { db, dir } = freshDb();
  try {
    seed(db);
    const rows = runCatalogReport(db, 'excise.form_m22', { company: COMPANY, year: YEAR });
    assert.ok(Array.isArray(rows) && rows.length >= 1);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('excise adapters now cover many states, split into api and portal modes', () => {
  const adapters = excise.listAdapters();
  const states = adapters.map((a) => a.state);
  for (const s of ['WB', 'OD', 'UP', 'HR', 'MH', 'KA', 'RJ', 'GJ', 'TS', 'MP', 'AP', 'AS', 'KL', 'CG', 'PB', 'DL']) {
    assert.ok(states.includes(s), `missing adapter for ${s}`);
  }
  assert.ok(adapters.some((a) => a.mode === 'api'), 'at least one api-mode adapter');
  assert.ok(adapters.some((a) => a.mode === 'portal'), 'at least one portal-mode adapter');
  // every adapter exposes a default base so the config screen can prefill it
  for (const a of adapters) assert.ok(a.defaultBase, `${a.state} should have a defaultBase`);
});
