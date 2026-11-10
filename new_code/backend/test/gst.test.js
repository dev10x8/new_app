import { test } from 'node:test';
import assert from 'node:assert/strict';
import { mkdtempSync, rmSync } from 'node:fs';
import { tmpdir } from 'node:os';
import { join } from 'node:path';
import { openDatabase } from '../src/db/index.js';
import * as sales from '../src/services/sales.js';
import * as purchase from '../src/services/purchase.js';
import * as gst from '../src/services/gst.js';
import { trialBalance } from '../src/services/accounting.js';

const COMPANY = '1';
const YEAR = '2024';

function freshDb() {
  const dir = mkdtempSync(join(tmpdir(), 'vanijliquor-gst-'));
  const file = join(dir, 'test.db');
  const db = openDatabase(file);
  return { db, file, dir };
}

function seedStock(db) {
  purchase.createPurchase(db, {
    company: COMPANY, year: YEAR, trndate: '2024-04-05', ptype: 'PURCHASE',
    suppliercode: 'S00001', purchaseacccode: 'L00004', shopcode: 'MAIN', docno: 'INV-1',
    lines: [{ itemcode: 'I00001', batchno: 'B1', itemrate: 200, itemquantity: 120, itemmrp: 250 }],
  });
}

test('GST columns are added to an existing database by migration', () => {
  const { db, file, dir } = freshDb();
  try {
    db.close();
    // Reopen the same file: schema_ext migration must add the GST columns.
    const db2 = openDatabase(file);
    const cols = db2.prepare('PRAGMA table_info(naiItemMaster)').all().map((c) => c.name);
    for (const c of ['cgst', 'sgst', 'igst', 'cess', 'addcess', 'gsttype', 'CgstOtptLdgr', 'IgstInptLdgr']) {
      assert.ok(cols.includes(c), `naiItemMaster should have ${c}`);
    }
    const sd = db2.prepare('PRAGMA table_info(salesbilldetail)').all().map((c) => c.name);
    for (const c of ['taxable', 'cgst', 'sgst', 'igst', 'cess', 'addcess', 'gstp']) {
      assert.ok(sd.includes(c), `salesbilldetail should have ${c}`);
    }
    db2.close();
  } finally { rmSync(dir, { recursive: true, force: true }); }
});

test('item GST rates resolve from both itemmst and naiItemMaster', () => {
  const { db, dir } = freshDb();
  try {
    const alc = gst.itemTax(db, COMPANY, 'I00001');
    assert.equal(alc.cgst, 9); assert.equal(alc.sgst, 9); assert.equal(alc.gsttype, 0);
    const nai = gst.itemTax(db, COMPANY, 'N00001');
    assert.equal(nai.cgst, 14); assert.equal(nai.sgst, 14); assert.equal(nai.gsttype, 1);
    const rates = gst.listItemRates(db, COMPANY);
    assert.equal(rates.length, 4);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('line GST splits exclusive and inclusive amounts correctly', () => {
  const { db, dir } = freshDb();
  try {
    // Exclusive (gsttype 0): amount is taxable, tax added on top.
    const excl = gst.lineGst(3000, gst.itemTax(db, COMPANY, 'I00001'));
    assert.equal(excl.taxable, 3000);
    assert.equal(excl.cgst, 270); assert.equal(excl.sgst, 270);
    assert.equal(excl.total, 3540);

    // Inclusive (gsttype 1): tax is backed out of the amount.
    const incl = gst.lineGst(4000, gst.itemTax(db, COMPANY, 'N00001'));
    assert.equal(incl.taxable, 3125);
    assert.equal(incl.cgst, 437.5); assert.equal(incl.sgst, 437.5);
    assert.equal(incl.total, 4000);

    // Inter-state: IGST only, no CGST/SGST.
    const igst = gst.lineGst(1000, { cgst: 0, sgst: 0, igst: 18, cess: 0, addcess: 0, gsttype: 0 });
    assert.equal(igst.igst, 180); assert.equal(igst.cgst, 0); assert.equal(igst.sgst, 0);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('a POS sale captures GST per line and books stay balanced', () => {
  const { db, dir } = freshDb();
  try {
    seedStock(db);
    const res = sales.createSale(db, {
      company: COMPANY, year: YEAR, trndate: '2024-04-06', storecode: 'CNT1',
      salestype: 'FL', ledgercode: 'C00001', lines: [{ itemcode: 'I00001', qnty: 12, rate: 250 }],
    });
    assert.equal(res.amount, 3000);
    assert.equal(res.gst, 540);

    const sale = sales.getSale(db, COMPANY, YEAR, res.trnno);
    const line = sale.lines[0];
    assert.equal(line.taxable, 3000);
    assert.equal(line.cgst, 270); assert.equal(line.sgst, 270);
    assert.equal(line.gstp, 18);
    assert.equal(sale.header.igst, 540);

    assert.equal(trialBalance(db, COMPANY, YEAR).balanced, true);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('purchase captures input tax credit and its ledger heads', () => {
  const { db, dir } = freshDb();
  try {
    purchase.createPurchase(db, {
      company: COMPANY, year: YEAR, trndate: '2024-04-05', ptype: 'PURCHASE',
      suppliercode: 'S00001', purchaseacccode: 'L00004', shopcode: 'MAIN', docno: 'INV-1', gstin: '19AAAAA0000A1Z5',
      lines: [{ itemcode: 'I00001', batchno: 'B1', itemrate: 200, itemquantity: 120, itemmrp: 250 }],
    });
    const rows = gst.purchaseGst(db, COMPANY, YEAR, {});
    assert.equal(rows.length, 1);
    assert.equal(rows[0].taxable, 24000);
    assert.equal(rows[0].cgst, 2160);
    const head = db.prepare('SELECT gstin FROM purchasemain').get();
    assert.equal(head.gstin, '19AAAAA0000A1Z5');
    const detail = db.prepare('SELECT CgstInptLdgr, IgstInptLdgr FROM purchasedetail').get();
    assert.equal(detail.CgstInptLdgr, 'L00014');
    assert.equal(detail.IgstInptLdgr, 'L00016');
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('GSTR-1 and GSTR-3B summarise outward supplies', () => {
  const { db, dir } = freshDb();
  try {
    seedStock(db);
    sales.createSale(db, {
      company: COMPANY, year: YEAR, trndate: '2024-04-06', storecode: 'CNT1',
      salestype: 'FL', ledgercode: 'C00001', lines: [{ itemcode: 'I00001', qnty: 12, rate: 250 }],
    });
    const g1 = gst.gstr1(db, COMPANY, YEAR, { from: '2024-04-01', to: '2024-04-30' });
    assert.equal(g1.count, 1);
    assert.equal(g1.totals.taxable, 3000);
    assert.equal(g1.totals.cgst, 270);
    assert.equal(g1.totals.sgst, 270);

    const g3 = gst.gstr3b(db, COMPANY, YEAR, { from: '2024-04-01', to: '2024-04-30' });
    assert.equal(g3.outward.taxable, 3000);
    assert.equal(g3.byRate.length, 1);
    assert.equal(g3.byRate[0].gstp, 18);
    assert.equal(g3.itc.taxable, 24000);
    assert.equal(g3.netPayable.cgst, 0, 'ITC exceeds output, nothing payable');
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('rate-slab report matches the legacy USP_NAIGstReport shape', () => {
  const { db, dir } = freshDb();
  try {
    seedStock(db);
    sales.createSale(db, {
      company: COMPANY, year: YEAR, trndate: '2024-04-06', storecode: 'CNT1',
      salestype: 'FL', ledgercode: 'C00001', lines: [{ itemcode: 'I00001', qnty: 12, rate: 250 }],
    });
    const rep = gst.naiGstReport(db, COMPANY, YEAR, {});
    assert.equal(rep.rows.length, 1);
    assert.equal(rep.rows[0].rate, 18);
    assert.equal(rep.rows[0].taxable, 3000);
    assert.equal(rep.rows[0].cgst, 270);
    assert.equal(rep.rows[0].qty, 12);
    assert.equal(rep.detail.length, 1);
    assert.equal(rep.detail[0].billno, 'FL00001');
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('GST return lifecycle: build, JSON payload, file, audit log', () => {
  const { db, dir } = freshDb();
  try {
    seedStock(db);
    sales.createSale(db, {
      company: COMPANY, year: YEAR, trndate: '2024-04-06', storecode: 'CNT1',
      salestype: 'FL', ledgercode: 'C00001', lines: [{ itemcode: 'I00001', qnty: 12, rate: 250 }],
    });

    const built = gst.buildReturn(db, COMPANY, YEAR, { rettype: 'GSTR1', period: '2024-04' });
    assert.equal(built.status, 'GENERATED');
    assert.equal(built.taxable, 3000);
    assert.equal(built.cgst, 270);
    assert.equal(built.invoiceno, 1);
    assert.ok(built.json.includes('ret_period'));

    const filed = gst.fileReturn(db, COMPANY, built.id, {});
    assert.equal(filed.status, 'FILED');
    assert.ok(filed.arn);
    assert.ok(filed.filedat);

    // Rebuilding does not duplicate the return row.
    gst.buildReturn(db, COMPANY, YEAR, { rettype: 'GSTR1', period: '2024-04' });
    assert.equal(gst.listReturns(db, COMPANY).filter((r) => r.rettype === 'GSTR1').length, 1);

    const logs = gst.logs(db, COMPANY);
    assert.ok(logs.some((l) => l.action === 'FILED'));

    const json = gst.returnJson(db, COMPANY, built.id);
    assert.equal(json.rettype, 'GSTR1');
    assert.equal(json.payload.ret_period, '202404');
    assert.ok(Array.isArray(json.payload.b2cs));
    assert.ok(Array.isArray(json.payload.b2b));
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('tax invoice prints the company GSTIN and tax split', () => {
  const { db, dir } = freshDb();
  try {
    seedStock(db);
    sales.createSale(db, {
      company: COMPANY, year: YEAR, trndate: '2024-04-06', storecode: 'CNT1',
      salestype: 'FL', ledgercode: 'C00001', lines: [{ itemcode: 'I00001', qnty: 12, rate: 250 }],
    });
    const inv = gst.taxInvoice(db, COMPANY, YEAR, 'FL00001');
    assert.equal(inv.seller.gstin, '19AABCA1234B1Z5');
    assert.equal(inv.lines.length, 1);
    assert.equal(inv.totals.taxable, 3000);
    assert.equal(inv.totals.cgst, 270);
    assert.equal(inv.totals.sgst, 270);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('GST settings round-trip and set the company GSTIN', () => {
  const { db, dir } = freshDb();
  try {
    const saved = gst.saveConfig(db, COMPANY, { gstin: '19XYZAB1234C1Z9', legalname: 'Test Co', autofile: true });
    assert.equal(saved.gstin, '19XYZAB1234C1Z9');
    assert.equal(saved.autofile, 1);
    const comp = db.prepare('SELECT gstin FROM companymast WHERE companycode=?').get(COMPANY);
    assert.equal(comp.gstin, '19XYZAB1234C1Z9');
    assert.equal(gst.getConfig(db, COMPANY).legalname, 'Test Co');
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});
