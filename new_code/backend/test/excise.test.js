import { test } from 'node:test';
import assert from 'node:assert/strict';
import { mkdtempSync, rmSync } from 'node:fs';
import { tmpdir } from 'node:os';
import { join } from 'node:path';
import http from 'node:http';
import { openDatabase, get, all } from '../src/db/index.js';
import * as excise from '../src/services/excise.js';
import * as sales from '../src/services/sales.js';
import * as purchase from '../src/services/purchase.js';
import { stockOnHand } from '../src/services/stock.js';

const COMPANY = '1';
const YEAR = '2024';

function freshDb() {
  const dir = mkdtempSync(join(tmpdir(), 'vanijliquor-excise-'));
  return { db: openDatabase(join(dir, 'test.db')), dir };
}

// A real local HTTP server that stands in for the state excise API. The service
// performs genuine fetch() calls against it - no mocks.
function fakeExciseServer({ failGtin = null } = {}) {
  const received = [];
  const server = http.createServer((req, res) => {
    let body = '';
    req.on('data', (c) => { body += c; });
    req.on('end', () => {
      res.setHeader('content-type', 'application/json');
      if (req.url.endsWith('/CheckAuthorization')) {
        res.end(JSON.stringify([{ TokenCode: 'TEST-TOKEN-123' }]));
        return;
      }
      const payload = JSON.parse(body || '{}');
      received.push({ url: req.url, token: req.headers.tokencode, payload });
      if (failGtin && payload.GTIN === failGtin) {
        res.statusCode = 200;
        res.end(JSON.stringify({ status: 'error', message: 'rejected by state server' }));
        return;
      }
      res.end(JSON.stringify({ status: 'success' }));
    });
  });
  return new Promise((resolve) => {
    server.listen(0, '127.0.0.1', () => {
      const { port } = server.address();
      resolve({ server, port, base: `http://127.0.0.1:${port}`, received });
    });
  });
}

function seedSale(db, { salestype = 'FL', qnty = 12 } = {}) {
  purchase.createPurchase(db, {
    company: COMPANY, year: YEAR, trndate: '2024-04-05', ptype: 'PURCHASE',
    suppliercode: 'S00001', purchaseacccode: 'L00004', shopcode: 'MAIN', docno: 'INV-1',
    lines: [{ itemcode: 'I00001', batchno: 'B1', itemrate: 200, itemquantity: 240, itemmrp: 250 }],
  });
  return sales.createSale(db, {
    company: COMPANY, year: YEAR, trndate: '2024-04-06', storecode: 'CNT1',
    salestype, ledgercode: 'C00001', lines: [{ itemcode: 'I00001', qnty, rate: 250 }],
  });
}

test('POS sale is captured into the excise queue with the item identity', () => {
  const { db, dir } = freshDb();
  try {
    const sale = seedSale(db);
    const rows = excise.queue(db, COMPANY);
    assert.equal(rows.length, 1);
    assert.equal(rows[0].gtin, '8901000000011'); // itemmst.barcode of I00001
    assert.equal(rows[0].packsize, '750');       // itemmst.ml
    assert.equal(rows[0].mrp, 250);              // itemmst.salesrate
    assert.equal(rows[0].quantity, 12);
    assert.equal(rows[0].status, 'PENDING');
    assert.equal(rows[0].sourcetrnno, sale.trnno);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('sale return is captured with a negative quantity', () => {
  const { db, dir } = freshDb();
  try {
    seedSale(db, { salestype: 'RETURN', qnty: 5 });
    const rows = excise.queue(db, COMPANY);
    assert.equal(rows.length, 1);
    assert.equal(rows[0].quantity, -5);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('re-capturing the same sale does not duplicate queue rows', () => {
  const { db, dir } = freshDb();
  try {
    const sale = seedSale(db);
    excise.captureSale(db, { company: COMPANY, year: YEAR, trnno: sale.trnno });
    const rows = excise.queue(db, COMPANY);
    assert.equal(rows.length, 1, 'idempotent capture');
    assert.equal(rows[0].quantity, 12);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('validation gate flags a line whose MRP no longer matches the item', () => {
  const { db, dir } = freshDb();
  try {
    seedSale(db);
    assert.equal(excise.validateQueue(db, COMPANY).length, 0);
    // change the item's sales rate so the queued MRP no longer matches
    db.prepare("UPDATE itemmst SET salesrate=300 WHERE itemcode='I00001'").run();
    const bad = excise.validateQueue(db, COMPANY);
    assert.equal(bad.length, 1);
    assert.equal(bad[0].gtin, '8901000000011');
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('upload authorizes and pushes pending lines, marking them ACKED', async () => {
  const { db, dir } = freshDb();
  const fake = await fakeExciseServer();
  try {
    seedSale(db);
    excise.saveConfig(db, COMPANY, { state: 'WB', endpointbase: fake.base, licenseeid: 'LIC-9', credential: 'pw' });

    const res = await excise.uploadQueue(db, { company: COMPANY });
    assert.equal(res.acked, 1);
    assert.equal(res.failed, 0);

    // the fake server saw the token and the exact legacy payload shape
    const push = fake.received.find((r) => r.url.endsWith('/InsertPosData'));
    assert.equal(push.token, 'TEST-TOKEN-123');
    assert.deepEqual(push.payload, {
      Licensee_Id_No: 'LIC-9', Datetime_Stamp: '2024-04-06', GTIN: '8901000000011',
      Pack_Size: '750', MRP: '250', Quantity: '12',
    });

    const rows = excise.queue(db, COMPANY);
    assert.equal(rows[0].status, 'ACKED');
    assert.ok(rows[0].ackedat);
  } finally {
    fake.server.close(); db.close(); rmSync(dir, { recursive: true, force: true });
  }
});

test('a rejected push marks FAILED, and retry requeues it', async () => {
  const { db, dir } = freshDb();
  const fake = await fakeExciseServer({ failGtin: '8901000000011' });
  try {
    seedSale(db);
    excise.saveConfig(db, COMPANY, { state: 'WB', endpointbase: fake.base, licenseeid: 'LIC-9', credential: 'pw' });

    const res = await excise.uploadQueue(db, { company: COMPANY });
    assert.equal(res.failed, 1);
    let rows = excise.queue(db, COMPANY);
    assert.equal(rows[0].status, 'FAILED');
    assert.match(rows[0].lasterror, /rejected/);

    const retry = excise.retryFailed(db, COMPANY);
    assert.equal(retry.retried, 1);
    rows = excise.queue(db, COMPANY);
    assert.equal(rows[0].status, 'PENDING');
  } finally {
    fake.server.close(); db.close(); rmSync(dir, { recursive: true, force: true });
  }
});

test('upload skips lines that fail the validation gate unless forced', async () => {
  const { db, dir } = freshDb();
  const fake = await fakeExciseServer();
  try {
    seedSale(db);
    excise.saveConfig(db, COMPANY, { state: 'WB', endpointbase: fake.base, licenseeid: 'LIC-9', credential: 'pw' });
    db.prepare("UPDATE itemmst SET salesrate=999 WHERE itemcode='I00001'").run();

    const res = await excise.uploadQueue(db, { company: COMPANY });
    assert.equal(res.skipped, 1);
    assert.equal(res.acked, 0);
    assert.equal(fake.received.length, 0, 'nothing pushed for a failing line');

    const forced = await excise.uploadQueue(db, { company: COMPANY, force: true });
    assert.equal(forced.acked, 1);
  } finally {
    fake.server.close(); db.close(); rmSync(dir, { recursive: true, force: true });
  }
});

test('closing balance is built from stock and pushed to the portal', async () => {
  const { db, dir } = freshDb();
  const fake = await fakeExciseServer();
  try {
    seedSale(db); // purchase 240, sale 12 -> closing 228
    const built = excise.buildClosingBalance(db, { company: COMPANY, year: YEAR, date: '2024-04-30' });
    assert.equal(built.lines, 1);
    const rows = excise.listClosingBalance(db, COMPANY, '2024-04-30');
    assert.equal(rows[0].closingqty, 228);

    excise.saveConfig(db, COMPANY, { state: 'WB', endpointbase: fake.base, licenseeid: 'LIC-9', credential: 'pw' });
    const up = await excise.uploadClosingBalance(db, { company: COMPANY });
    assert.equal(up.acked, 1);
    const push = fake.received.find((r) => r.url.endsWith('/InsertClosingBalance'));
    assert.equal(push.payload.Closing_Balance, '228');
  } finally {
    fake.server.close(); db.close(); rmSync(dir, { recursive: true, force: true });
  }
});

test('portal inbound indent is received and applied into a purchase', () => {
  const { db, dir } = freshDb();
  try {
    const id = excise.receiveInbound(db, COMPANY, {
      doctype: 'TP', docno: 'TP-1001', docdate: '2024-04-07', tppassno: 'TPP-55', year: YEAR,
      shopcode: 'MAIN', lines: [{ itemcode: 'I00001', quantity: 24, rate: 200 }],
    });
    const list = excise.listInbound(db, COMPANY, 'RECEIVED');
    assert.equal(list.length, 1);

    const applied = excise.applyInbound(db, COMPANY, id);
    assert.ok(applied.trnno);
    const after = excise.listInbound(db, COMPANY, 'APPLIED');
    assert.equal(after.length, 1);

    const rows = stockOnHand(db, { company: COMPANY, year: YEAR, storecode: 'MAIN' });
    assert.equal(rows.find((r) => r.itemcode === 'I00001').qty, 24);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('sync report summarises the excise pipeline', () => {
  const { db, dir } = freshDb();
  try {
    seedSale(db);
    const rep = excise.syncReport(db, COMPANY);
    assert.equal(rep.summary.PENDING, 1);
    assert.equal(rep.unmatched, 0);
    assert.ok(Array.isArray(rep.logs));
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

// ------------------------------------------------------------- Odisha (OD)

test('Odisha adapter is registered in portal mode', () => {
  const od = excise.listAdapters().find((a) => a.state === 'OD');
  assert.ok(od, 'OD adapter present');
  assert.equal(od.mode, 'portal');
  assert.match(od.defaultBase, /stateexcise\.odisha\.gov\.in/);
});

test('Odisha upload stages pending lines as SENT and builds a portal batch', async () => {
  const { db, dir } = freshDb();
  try {
    seedSale(db);
    excise.saveConfig(db, COMPANY, { state: 'OD', endpointbase: 'https://stateexcise.odisha.gov.in', licenseeid: 'OD-LIC-1' });

    const res = await excise.uploadQueue(db, { company: COMPANY });
    assert.equal(res.mode, 'portal');
    assert.equal(res.staged, 1);
    assert.equal(res.portal, '/UserLogIn/Login.aspx');

    const rows = excise.queue(db, COMPANY);
    assert.equal(rows[0].status, 'SENT');
    assert.equal(excise.queueSummary(db, COMPANY).SENT, 1);

    const batch = excise.buildPortalBatch(db, COMPANY);
    assert.equal(batch.mode, 'portal');
    assert.equal(batch.portal.login, '/UserLogIn/Login.aspx');
    assert.match(batch.portal.base, /stateexcise\.odisha\.gov\.in/);
    assert.equal(batch.sales.length, 1);
    assert.equal(batch.sales[0].Licensee_Id_No, 'OD-LIC-1');
    assert.equal(batch.sales[0].GTIN, '8901000000011');
    assert.equal(batch.sales[0].Quantity, '12');
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('Odisha portal agent confirmation marks staged lines ACKED', async () => {
  const { db, dir } = freshDb();
  try {
    seedSale(db);
    excise.saveConfig(db, COMPANY, { state: 'OD', licenseeid: 'OD-LIC-1' });
    await excise.uploadQueue(db, { company: COMPANY });

    const [row] = excise.queue(db, COMPANY);
    const out = excise.confirmUploaded(db, COMPANY, { results: [{ id: row.id, status: 'ACKED' }] });
    assert.equal(out.acked, 1);
    assert.equal(excise.queue(db, COMPANY)[0].status, 'ACKED');
    assert.equal(excise.queueSummary(db, COMPANY).ACKED, 1);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('Odisha portal rejection marks a line FAILED and it can be retried', async () => {
  const { db, dir } = freshDb();
  try {
    seedSale(db);
    excise.saveConfig(db, COMPANY, { state: 'OD', licenseeid: 'OD-LIC-1' });
    await excise.uploadQueue(db, { company: COMPANY });
    const [row] = excise.queue(db, COMPANY);

    const out = excise.confirmUploaded(db, COMPANY, {
      results: [{ id: row.id, status: 'FAILED', message: 'GTIN not registered on portal' }],
    });
    assert.equal(out.failed, 1);
    assert.equal(excise.queue(db, COMPANY)[0].status, 'FAILED');
    assert.match(excise.queue(db, COMPANY)[0].lasterror, /not registered/);

    assert.equal(excise.retryFailed(db, COMPANY).retried, 1);
    assert.equal(excise.queue(db, COMPANY)[0].status, 'PENDING');
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('Odisha upload holds back lines failing the validation gate', async () => {
  const { db, dir } = freshDb();
  try {
    seedSale(db);
    excise.saveConfig(db, COMPANY, { state: 'OD', licenseeid: 'OD-LIC-1' });
    db.prepare("UPDATE itemmst SET salesrate=999 WHERE itemcode='I00001'").run();

    const res = await excise.uploadQueue(db, { company: COMPANY });
    assert.equal(res.staged, 0);
    assert.equal(res.skipped, 1);
    assert.equal(excise.queue(db, COMPANY)[0].status, 'PENDING', 'bad line stays PENDING');
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('Odisha closing balance is handed to the portal agent via the batch', async () => {
  const { db, dir } = freshDb();
  try {
    seedSale(db);
    excise.saveConfig(db, COMPANY, { state: 'OD', licenseeid: 'OD-LIC-1' });
    excise.buildClosingBalance(db, { company: COMPANY, year: YEAR, date: '2024-04-30' });

    const up = await excise.uploadClosingBalance(db, { company: COMPANY });
    assert.equal(up.mode, 'portal');
    assert.equal(up.staged, 1);

    const batch = excise.buildPortalBatch(db, COMPANY);
    assert.equal(batch.closing.length, 1);
    assert.equal(batch.closing[0].Closing_Balance, '228');

    const [cl] = all(db, 'SELECT * FROM tbl_ExciseClosingBalance WHERE companycode=?', [COMPANY]);
    const conf = excise.confirmUploaded(db, COMPANY, { closingIds: [cl.id] });
    assert.equal(conf.closing, 1);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});
