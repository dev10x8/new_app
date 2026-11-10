import { test } from 'node:test';
import assert from 'node:assert/strict';
import { mkdtempSync, rmSync } from 'node:fs';
import { tmpdir } from 'node:os';
import { join } from 'node:path';
import { deflateRawSync, deflateSync } from 'node:zlib';
import { openDatabase, get, all } from '../src/db/index.js';
import * as tabular from '../src/util/tabular.js';
import * as bulkimport from '../src/services/bulkimport.js';
import * as statemaster from '../src/services/statemaster.js';
import * as landedcost from '../src/services/landedcost.js';
import * as parcha from '../src/services/parcha.js';
import { createPurchase } from '../src/services/purchase.js';

const COMPANY = '1';
const YEAR = '2024';

function freshDb() {
  const dir = mkdtempSync(join(tmpdir(), 'madhushala-import-'));
  return { db: openDatabase(join(dir, 'test.db')), dir };
}

// ---------------------------------------------------------------- zip helpers

/** CRC32 (needed only so the archives look real; the reader ignores it). */
function crc32(buf) {
  let c = ~0;
  for (let i = 0; i < buf.length; i++) {
    c ^= buf[i];
    for (let k = 0; k < 8; k++) c = (c >>> 1) ^ (0xedb88320 & -(c & 1));
  }
  return (~c) >>> 0;
}

/** Build a minimal ZIP (deflate entries) from name -> string|Buffer. */
function zip(entries) {
  const locals = [];
  const central = [];
  let offset = 0;
  for (const [name, data] of entries) {
    const raw = Buffer.isBuffer(data) ? data : Buffer.from(String(data), 'utf8');
    const comp = deflateRawSync(raw);
    const nameBuf = Buffer.from(name, 'utf8');
    const crc = crc32(raw);
    const local = Buffer.alloc(30 + nameBuf.length);
    local.writeUInt32LE(0x04034b50, 0);
    local.writeUInt16LE(20, 4);
    local.writeUInt16LE(0, 6);
    local.writeUInt16LE(8, 8); // deflate
    local.writeUInt32LE(0, 10);
    local.writeUInt32LE(crc, 14);
    local.writeUInt32LE(comp.length, 18);
    local.writeUInt32LE(raw.length, 22);
    local.writeUInt16LE(nameBuf.length, 26);
    nameBuf.copy(local, 30);
    locals.push(local, comp);

    const cen = Buffer.alloc(46 + nameBuf.length);
    cen.writeUInt32LE(0x02014b50, 0);
    cen.writeUInt16LE(20, 4);
    cen.writeUInt16LE(20, 6);
    cen.writeUInt16LE(8, 10);
    cen.writeUInt32LE(crc, 16);
    cen.writeUInt32LE(comp.length, 20);
    cen.writeUInt32LE(raw.length, 24);
    cen.writeUInt16LE(nameBuf.length, 28);
    cen.writeUInt32LE(offset, 42);
    nameBuf.copy(cen, 46);
    central.push(cen);
    offset += local.length + comp.length;
  }
  const cenBuf = Buffer.concat(central);
  const eocd = Buffer.alloc(22);
  eocd.writeUInt32LE(0x06054b50, 0);
  eocd.writeUInt16LE(central.length, 8);
  eocd.writeUInt16LE(central.length, 10);
  eocd.writeUInt32LE(cenBuf.length, 12);
  eocd.writeUInt32LE(offset, 16);
  return Buffer.concat([...locals, cenBuf, eocd]);
}

const cell = (ref, v, t) => (t ? `<c r="${ref}" t="${t}"><v>${v}</v></c>` : `<c r="${ref}"><v>${v}</v></c>`);
const cellStr = (ref, s, idx) => `<c r="${ref}" t="s"><v>${idx}</v></c>`;

/** Build an .xlsx with one sheet from an array-of-arrays (all strings shared). */
function makeXlsx(rows) {
  const strings = [];
  const idx = (s) => { const i = strings.indexOf(s); if (i >= 0) return i; strings.push(s); return strings.length - 1; };
  const sheetRows = rows.map((r, ri) => `<row r="${ri + 1}">${r.map((v, ci) => cellStr(`${String.fromCharCode(65 + ci)}${ri + 1}`, String(v), idx(String(v)))).join('')}</row>`).join('');
  const sheet = `<?xml version="1.0"?><worksheet xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main"><sheetData>${sheetRows}</sheetData></worksheet>`;
  const sst = `<?xml version="1.0"?><sst xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main" count="${strings.length}" uniqueCount="${strings.length}">${strings.map((s) => `<si><t>${s.replace(/&/g, '&amp;').replace(/</g, '&lt;')}</t></si>`).join('')}</sst>`;
  const wb = '<?xml version="1.0"?><workbook xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main"><sheets><sheet name="Sheet1" sheetId="1" r:id="rId1"/></sheets></workbook>';
  return zip([
    ['xl/workbook.xml', wb],
    ['xl/sharedStrings.xml', sst],
    ['xl/worksheets/sheet1.xml', sheet],
  ]);
}

// ------------------------------------------------------------- tabular reader

test('parseDelimited handles quotes, commas and blank lines', () => {
  const rows = tabular.parseDelimited('a,b\n"x, y",2\n\n3,4\n');
  assert.deepEqual(rows, [['a', 'b'], ['x, y', '2'], ['3', '4']]);
});

test('readXlsx reads a real (deflated) workbook', () => {
  const buf = makeXlsx([['itemcode', 'itemname', 'ml'], ['I00901', 'Test Rum', '750']]);
  const sheets = tabular.readXlsx(buf);
  assert.equal(sheets.length, 1);
  assert.equal(sheets[0].name, 'Sheet1');
  assert.deepEqual(sheets[0].rows[0], ['itemcode', 'itemname', 'ml']);
  assert.deepEqual(sheets[0].rows[1], ['I00901', 'Test Rum', '750']);
});

// --------------------------------------------------------------- bulk import

test('bulk import: analyze then apply an item master sheet', () => {
  const { db } = freshDb();
  const csv = 'Item Code,Item Name,ML,Packing,Sale Rate\nI00910,Bulk Rum A,750,12,620\nI00911,Bulk Rum B,375,24,320\n';
  const plan = bulkimport.analyze(db, { company: COMPANY, year: YEAR, kind: 'masters', target: 'item', content: csv, source: 'csv' });
  assert.equal(plan.totals.rows, 2);
  assert.equal(plan.totals.inserts, 2);
  assert.equal(get(db, 'SELECT COUNT(*) n FROM itemmst WHERE itemcode=?', ['I00910']).n, 0, 'analyze writes nothing');

  const res = bulkimport.apply(db, { company: COMPANY, year: YEAR, kind: 'masters', target: 'item', content: csv, source: 'csv' });
  assert.equal(res.totals.inserts, 2);
  const item = get(db, 'SELECT * FROM itemmst WHERE companycode=? AND itemcode=?', [COMPANY, 'I00910']);
  assert.equal(item.itemname, 'Bulk Rum A');
  assert.equal(item.ml, 750);
  assert.equal(item.companycode, COMPANY, 'company is filled in');

  // re-apply updates in place, never duplicates
  const res2 = bulkimport.apply(db, { company: COMPANY, year: YEAR, kind: 'masters', target: 'item', content: csv, source: 'csv' });
  assert.equal(res2.totals.inserts, 0);
  assert.equal(res2.totals.updates, 2);
});

test('bulk import: xlsx buffer, loose headers and missing-key reporting', () => {
  const { db } = freshDb();
  const buf = makeXlsx([
    ['item_code', 'Item Name', 'ml'],
    ['I00920', 'Sheet Rum', '750'],
    ['', 'No Code Row', '750'],
  ]);
  const res = bulkimport.apply(db, { company: COMPANY, year: YEAR, kind: 'masters', target: 'item', buffer: buf, filename: 'items.xlsx' });
  assert.equal(res.totals.inserts, 1);
  assert.equal(res.totals.failed, 1);
  assert.match(res.errors[0].error, /Missing key/);
  assert.equal(get(db, 'SELECT itemname FROM itemmst WHERE itemcode=?', ['I00920']).itemname, 'Sheet Rum');
});

test('bulk import: opening stock upserts into openingstockmst', () => {
  const { db } = freshDb();
  const csv = 'shopcode,itemcode,loose,box,qnty\nMAIN,I00001,2,5,62\n';
  const res = bulkimport.apply(db, { company: COMPANY, year: YEAR, kind: 'openingstock', target: 'openingstock', content: csv, source: 'csv' });
  assert.equal(res.totals.inserts, 1);
  const row = get(db, 'SELECT * FROM openingstockmst WHERE companycode=? AND itemcode=?', [COMPANY, 'I00001']);
  assert.equal(row.qnty, 62);
  assert.equal(row.yearcode, YEAR);
});

// ------------------------------------------------------------- state master

test('state master: load a bundled dataset and create items from it', () => {
  const { db } = freshDb();
  const datasets = statemaster.datasets();
  assert.ok(datasets.find((d) => d.state === 'WB'), 'WB dataset is bundled');

  const loaded = statemaster.loadState(db, { company: COMPANY, state: 'WB' });
  assert.ok(loaded.loaded > 0);
  assert.equal(loaded.inserts, loaded.loaded);
  // reload is idempotent
  const again = statemaster.loadState(db, { company: COMPANY, state: 'WB' });
  assert.equal(again.inserts, 0);
  assert.equal(again.updates, loaded.loaded);

  const rows = statemaster.listStateMaster(db, { company: COMPANY, state: 'WB' });
  assert.ok(rows.length > 0);
  assert.ok(rows.every((r) => r.mrp > 0 && r.ml > 0));

  const plan = statemaster.analyzeItems(db, { company: COMPANY, year: YEAR, state: 'WB' });
  assert.equal(plan.inserts, rows.length);
  assert.equal(plan.updates, 0);

  const applied = statemaster.applyItems(db, { company: COMPANY, year: YEAR, state: 'WB' });
  assert.equal(applied.inserts, rows.length);
  assert.ok(get(db, 'SELECT COUNT(*) n FROM itemmst WHERE companycode=? AND salesrate IS NOT NULL AND salesrate > 0', [COMPANY]).n > 0, 'items were created');
  // category / kfl masters were created as needed
  assert.ok(get(db, 'SELECT COUNT(*) n FROM categorymst WHERE companycode=?', [COMPANY]).n > 0);
  assert.ok(get(db, 'SELECT COUNT(*) n FROM kflmst WHERE companycode=?', [COMPANY]).n > 0);
});

test('state master: pack sizes resolve Quart / Pint / Nip by ml', () => {
  const { db } = freshDb();
  const packs = statemaster.listPackSizes(db, COMPANY);
  assert.ok(packs.find((p) => p.packname === 'Quart' && p.ml === 750));
  assert.equal(statemaster.packNameForMl(db, COMPANY, 750), 'Quart');
  assert.equal(statemaster.packNameForMl(db, COMPANY, 180), 'Nip');
  assert.equal(statemaster.packNameForMl(db, COMPANY, 999), null);
});

// --------------------------------------------------------------- landing cost

test('landing cost: free bottles and charges lower the true cost per bottle', () => {
  const c = landedcost.computeLine(
    { itemrate: 200, itemquantity: 10, itemfreeqnty: 1, itemdiscount: 100 },
    { otherCharges: 500, salesrate: 250 },
  );
  // (200*10 - 100 + 500) / 11 = 2400/11 = 218.18
  assert.equal(c.landingcost, 218.18);
  assert.equal(c.marginamt, 31.82);
  assert.equal(c.marginpct, 12.73);
});

test('landing cost: computePurchase persists per-line rows', () => {
  const { db } = freshDb();
  // post a real purchase (fills every NOT NULL header column)
  const purchase = createPurchase(db, {
    company: COMPANY, year: YEAR, trndate: '2024-05-01', tottaxOth: 100,
    lines: [{ itemcode: 'I00001', batchno: '0', itemrate: 200, itemquantity: 5, itemmrp: 250, itemdiscount: 0 }],
  });
  const res = landedcost.computePurchase(db, { company: COMPANY, year: YEAR, trnno: purchase.trnno });
  assert.equal(res.lines.length, 1);
  // (200*5 + 100) / 5 = 220
  assert.equal(res.lines[0].landingcost, 220);
  const saved = get(db, 'SELECT * FROM tbl_LandedCost WHERE companycode=? AND trnno=?', [COMPANY, purchase.trnno]);
  assert.equal(saved.landingcost, 220);
  assert.equal(landedcost.report(db, { company: COMPANY, year: YEAR }).length, 1);
});

test('landing cost: applyMarginToItem writes the selling price', () => {
  const { db } = freshDb();
  const r = landedcost.applyMarginToItem(db, { company: COMPANY, itemcode: 'I00001', landingcost: 200, marginPct: 25 });
  assert.equal(r.salesrate, 250);
  assert.equal(get(db, 'SELECT salesrate FROM itemmst WHERE itemcode=?', ['I00001']).salesrate, 250);
});

// ------------------------------------------------------------------- parcha

test('parcha: parse a structured JSON invoice and match item lines', () => {
  const { db } = freshDb();
  const invoice = parcha.parseInvoiceJson(JSON.stringify({
    supplier: 'United Spirits', docno: 'INV-100',
    lines: [{ itemcode: 'I00001', qty: 10, rate: 600, mrp: 750, free: 1 }],
  }));
  assert.equal(invoice.header.suppliername, 'United Spirits');
  const plan = parcha.buildPlan(db, { company: COMPANY, year: YEAR, invoice });
  assert.equal(plan.entries[0].itemcode, 'I00001');
  assert.equal(plan.entries[0].how, 'code');
  // (600*10)/11 = 545.45
  assert.equal(plan.entries[0].landingcost, 545.45);
});

test('parcha: fuzzy name matching tolerates a differently-spelled brand', () => {
  const { db } = freshDb();
  // The item master holds "Royal Stag 750ml"; a hand-typed parcha says more.
  const hit = parcha.matchLine(db, COMPANY, { itemname: 'Royal Stag Deluxe Whisky 750ml', ml: 750 });
  assert.equal(hit.itemcode, 'I00001');
  assert.equal(hit.how, 'name~');
  // An unrelated brand must not be force-matched.
  assert.equal(parcha.matchLine(db, COMPANY, { itemname: 'Absolut Vodka Sweden', ml: 750 }), null);
});

test('parcha: text extraction -> invoice lines', () => {
  const text = 'Supplier: United Spirits\nInvoice No: A-77\nDate: 01/05/2024\nRoyal Stag Deluxe Whisky 750ml   10   600.00   6000.00\nOld Monk Rum   5   500.00   2500.00\n';
  const inv = parcha.parseInvoiceText(text);
  assert.equal(inv.header.suppliername, 'United Spirits');
  assert.equal(inv.header.docno, 'A-77');
  assert.equal(inv.lines.length, 2);
  assert.equal(inv.lines[0].itemquantity, 10);
  assert.equal(inv.lines[0].itemrate, 600);
});

test('parcha: extractPdfText reads a simple Flate-compressed content stream', () => {
  const content = 'BT /F1 12 Tf (Royal Stag 750) Tj (10) Tj (600) Tj ET';
  const stream = deflateSync(Buffer.from(content, 'latin1'));
  const pdf = Buffer.concat([
    Buffer.from('%PDF-1.4\n', 'latin1'),
    Buffer.from('stream\n', 'latin1'),
    stream,
    Buffer.from('\nendstream\n', 'latin1'),
  ]);
  const text = parcha.extractPdfText(pdf);
  assert.match(text, /Royal Stag 750/);
  assert.match(text, /600/);
});

test('parcha: apply posts a purchase, computes landing cost and seeds opening stock', () => {
  const { db } = freshDb();
  const invoice = parcha.parseInvoiceJson(JSON.stringify({
    supplier: 'United Spirits', docno: 'INV-200', trndate: '2024-05-02',
    lines: [
      { itemcode: 'I00001', qty: 10, rate: 600, mrp: 750, free: 1 },
      { itemname: 'Brand New Rum', qty: 4, rate: 400, mrp: 520 },
    ],
  }));
  const res = parcha.apply(db, {
    company: COMPANY, year: YEAR, invoice, createMissing: true, marginPct: 20, setOpeningStock: true, shopcode: 'MAIN',
  });
  assert.ok(res.trnno, 'a purchase number was returned');
  const purchase = get(db, 'SELECT * FROM purchasemain WHERE companycode=? AND trnno=?', [COMPANY, res.trnno]);
  assert.ok(purchase, 'purchase header written');
  assert.equal(get(db, 'SELECT COUNT(*) n FROM purchasedetail WHERE companycode=? AND trnno=?', [COMPANY, res.trnno]).n, 2);
  // the unmatched line created a new item
  const created = get(db, 'SELECT * FROM itemmst WHERE companycode=? AND itemname=?', [COMPANY, 'Brand New Rum']);
  assert.ok(created, 'missing item was created');
  // landed cost rows exist for both lines
  assert.equal(get(db, 'SELECT COUNT(*) n FROM tbl_LandedCost WHERE companycode=? AND trnno=?', [COMPANY, res.trnno]).n, 2);
  // opening stock seeded
  assert.equal(get(db, 'SELECT COUNT(*) n FROM openingstockmst WHERE companycode=? AND shopcode=?', [COMPANY, 'MAIN']).n, 2);
  // margin applied to the first item's selling price
  const priced = get(db, 'SELECT salesrate FROM itemmst WHERE companycode=? AND itemcode=?', [COMPANY, 'I00001']);
  assert.ok(priced.salesrate > 600, 'selling price raised above the purchase rate');
  assert.equal(get(db, 'SELECT COUNT(*) n FROM tbl_ParchaImport WHERE companycode=? AND status=?', [COMPANY, 'IMPORTED']).n, 1);
});

test('parcha: analyze is a dry run that writes nothing', () => {
  const { db } = freshDb();
  const invoice = parcha.parseInvoiceJson(JSON.stringify({ lines: [{ itemcode: 'I00001', qty: 2, rate: 600 }] }));
  const r = parcha.analyze(db, { company: COMPANY, year: YEAR, invoice });
  assert.equal(r.totals.lines, 1);
  assert.equal(r.totals.matched, 1);
  assert.equal(get(db, 'SELECT COUNT(*) n FROM purchasemain').n, 0);
  assert.equal(get(db, 'SELECT COUNT(*) n FROM tbl_ParchaImport WHERE status=?', ['ANALYZED']).n, 1);
});

test('parcha: ocr config round-trips and the stub provider refuses', async () => {
  const { db } = freshDb();
  const cfg = parcha.saveOcrConfig(db, COMPANY, { provider: 'stub', lang: 'eng', enabled: 1 });
  assert.equal(cfg.provider, 'stub');
  await assert.rejects(() => parcha.ocrExtract(db, COMPANY, { buffer: Buffer.from('x'), filename: 'a.png' }), /stub/);
});
