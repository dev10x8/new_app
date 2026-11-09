import { test } from 'node:test';
import assert from 'node:assert/strict';
import { mkdtempSync, rmSync } from 'node:fs';
import { tmpdir } from 'node:os';
import { join } from 'node:path';
import { openDatabase, get } from '../src/db/index.js';
import * as inventory from '../src/services/inventory.js';
import * as purchase from '../src/services/purchase.js';
import * as sales from '../src/services/sales.js';

const COMPANY = '1';
const YEAR = '2024';

function freshDb() {
  const dir = mkdtempSync(join(tmpdir(), 'madhushala-inv-'));
  const file = join(dir, 'test.db');
  return { db: openDatabase(file), file, dir };
}

function receive(db, qty) {
  purchase.createPurchase(db, {
    company: COMPANY, year: YEAR, trndate: '2024-04-05', ptype: 'PURCHASE',
    suppliercode: 'S00001', purchaseacccode: 'L00004', shopcode: 'MAIN', docno: 'INV-1',
    lines: [{ itemcode: 'I00001', batchno: 'B1', itemrate: 200, itemquantity: qty, itemmrp: 250 }],
  });
}

test('inventory schema (tbl_Requisition) is created by the migration', () => {
  const { db, file, dir } = freshDb();
  try {
    db.close();
    const db2 = openDatabase(file);
    const t = get(db2, "SELECT name FROM sqlite_master WHERE type='table' AND name='tbl_Requisition'");
    assert.ok(t, 'tbl_Requisition should exist after migration');
    db2.close();
  } finally { rmSync(dir, { recursive: true, force: true }); }
});

test('inventory summary counts items and flags low / out of stock', () => {
  const { db, dir } = freshDb();
  try {
    receive(db, 5); // I00001 seeded with min_qnty = 12
    const s = inventory.inventorySummary(db, { company: COMPANY, year: YEAR });
    assert.ok(s.totalItems >= 3, `expected seeded items, got ${s.totalItems}`);
    assert.equal(s.lowStock, 1, 'I00001 is below its min level');
    assert.ok(s.outOfStock >= 1, 'items with no stock are OUT');
    assert.ok(s.stockValue > 0, 'stock value is computed');
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('low-stock list reports the reorder quantity', () => {
  const { db, dir } = freshDb();
  try {
    receive(db, 5);
    const low = inventory.lowStock(db, { company: COMPANY, year: YEAR });
    const it = low.find((i) => i.itemcode === 'I00001');
    assert.ok(it, 'I00001 should be low');
    assert.equal(it.minlevel, 12);
    assert.equal(it.qty, 5);
    assert.equal(it.reorderqty, 7);
    assert.equal(it.status, 'LOW');
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('an item drops out of the low-stock list once it is replenished', () => {
  const { db, dir } = freshDb();
  try {
    receive(db, 5);
    const before = inventory.lowStock(db, { company: COMPANY, year: YEAR });
    assert.ok(before.find((i) => i.itemcode === 'I00001'), 'I00001 is low at 5 units');
    receive(db, 40); // now 45 on hand, above the min of 12
    const after = inventory.lowStock(db, { company: COMPANY, year: YEAR }).find((i) => i.itemcode === 'I00001');
    assert.equal(after, undefined, 'I00001 leaves the low-stock list');
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('auto-requisition raises one draft per low item and is idempotent', () => {
  const { db, dir } = freshDb();
  try {
    receive(db, 3);
    const first = inventory.generateRequisitions(db, { company: COMPANY, year: YEAR });
    assert.equal(first.created, 3, 'I00001 (low) + I00002/I00003 (out) = 3');
    const second = inventory.generateRequisitions(db, { company: COMPANY, year: YEAR });
    assert.equal(second.created, 0);
    assert.equal(second.skipped, 3);
    const reqs = inventory.requisitions(db, COMPANY);
    assert.equal(reqs.length, 3);
    const i1 = reqs.find((r) => r.itemcode === 'I00001');
    assert.equal(i1.status, 'DRAFT');
    assert.equal(i1.reorderqty, 9);
    // transition to ORDERED and confirm the summary
    inventory.updateRequisition(db, COMPANY, i1.id, { status: 'ORDERED' });
    const sum = inventory.requisitionSummary(db, COMPANY);
    assert.equal(sum.ORDERED, 1);
    assert.equal(sum.DRAFT, 2);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('addItem records a new stock item via the master CRUD', () => {
  const { db, dir } = freshDb();
  try {
    const item = inventory.addItem(db, COMPANY, {
      itemcode: 'I09999', itemname: 'Test Bottle', packing: 1, ml: 750,
      purchase_discount_amount: 0, purchase_salestaxonmrp_amount: 0, purchase_taxscheme_amount: 0,
      salesrate: 100, purchaseRate: 80, min_qnty: 24,
    });
    assert.equal(item.itemcode, 'I09999');
    assert.equal(item.itemname, 'Test Bottle');
    const stock = inventory.listItemStock(db, { company: COMPANY, year: YEAR });
    const added = stock.find((i) => i.itemcode === 'I09999');
    assert.ok(added);
    assert.equal(added.minlevel, 24);
    assert.equal(added.status, 'OUT'); // no stock yet
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('selling stock drives an item into the low-stock alert', () => {
  const { db, dir } = freshDb();
  try {
    receive(db, 20);
    sales.createSale(db, {
      company: COMPANY, year: YEAR, trndate: '2024-04-06', storecode: 'CNT1',
      salestype: 'FL', ledgercode: 'C00001', lines: [{ itemcode: 'I00001', qnty: 12, rate: 250 }],
    });
    const low = inventory.lowStock(db, { company: COMPANY, year: YEAR }).find((i) => i.itemcode === 'I00001');
    assert.ok(low, 'after selling 12 of 20 the item is at/below the min of 12');
    assert.equal(low.qty, 8);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});
