import { test } from 'node:test';
import assert from 'node:assert/strict';
import { mkdtempSync, rmSync } from 'node:fs';
import { tmpdir } from 'node:os';
import { join } from 'node:path';
import http from 'node:http';
import { openDatabase, get } from '../src/db/index.js';
import * as tally from '../src/services/tally.js';
import * as sales from '../src/services/sales.js';
import * as purchase from '../src/services/purchase.js';

const COMPANY = '1';
const YEAR = '2024';

function freshDb() {
  const dir = mkdtempSync(join(tmpdir(), 'vanijliquor-tallylive-'));
  return { db: openDatabase(join(dir, 'test.db')), dir };
}

// A real local HTTP server standing in for Tally Prime's HTTP/XML server.
// The service performs a genuine POST against it - no mocks.
function fakeTally({ reject = false } = {}) {
  const received = [];
  const server = http.createServer((req, res) => {
    let body = '';
    req.on('data', (c) => { body += c; });
    req.on('end', () => {
      received.push({ url: req.url, method: req.method, contentType: req.headers['content-type'], body });
      res.setHeader('content-type', 'text/xml');
      if (reject) {
        res.end('<ENVELOPE><BODY><DATA><ERRORS>1</ERRORS><LINEERROR>Ledger does not exist</LINEERROR></DATA></BODY></ENVELOPE>');
        return;
      }
      res.end('<ENVELOPE><BODY><DATA><CREATED>1</CREATED><ERRORS>0</ERRORS></DATA></BODY></ENVELOPE>');
    });
  });
  return new Promise((resolve) => {
    server.listen(0, '127.0.0.1', () => resolve({ server, port: server.address().port, received }));
  });
}

function seedAndQueue(db) {
  purchase.createPurchase(db, {
    company: COMPANY, year: YEAR, trndate: '2024-04-05', ptype: 'PURCHASE',
    suppliercode: 'S00001', purchaseacccode: 'L00004', shopcode: 'MAIN', docno: 'INV-1',
    lines: [{ itemcode: 'I00001', batchno: 'B1', itemrate: 200, itemquantity: 50, itemmrp: 250 }],
  });
  sales.createSale(db, {
    company: COMPANY, year: YEAR, trndate: '2024-04-06', storecode: 'CNT1',
    salestype: 'FL', ledgercode: 'C00001', lines: [{ itemcode: 'I00001', qnty: 10, rate: 250 }],
  });
  tally.saveConfig(db, COMPANY, { enabled: true, tallycompany: 'VANIJLIQUOR' });
  tally.enqueueAll(db, COMPANY, YEAR);
}

test('live push imports queued vouchers into Tally over HTTP/XML', async () => {
  const { db, dir } = freshDb();
  const { server, port, received } = await fakeTally();
  try {
    seedAndQueue(db);
    const url = `http://127.0.0.1:${port}`;
    tally.saveConfig(db, COMPANY, { tallyurl: url });

    const res = await tally.livePush(db, COMPANY, YEAR, {});
    assert.equal(res.status, 200);
    assert.ok(res.acked >= 1, `expected vouchers acked, got ${res.acked}`);
    assert.equal(res.failed, 0);

    // Every queue row that was pushed is now ACKED.
    const pending = get(db, "SELECT COUNT(*) AS n FROM tbl_TallySyncQueue WHERE companycode=? AND status<>'ACKED'", [COMPANY]);
    assert.equal(pending.n, 0);

    // The request was a real text/xml POST carrying an import envelope.
    assert.equal(received.length, 1);
    assert.equal(received[0].method, 'POST');
    assert.match(received[0].contentType, /text\/xml/);
    assert.match(received[0].body, /TALLYREQUEST>Import Data/);
    assert.match(received[0].body, /VANIJLIQUOR/);
  } finally { server.close(); db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('live push keeps vouchers for retry when Tally reports a line error', async () => {
  const { db, dir } = freshDb();
  const { server, port } = await fakeTally({ reject: true });
  try {
    seedAndQueue(db);
    tally.saveConfig(db, COMPANY, { tallyurl: `http://127.0.0.1:${port}` });
    const res = await tally.livePush(db, COMPANY, YEAR, {});
    assert.equal(res.acked, 0);
    assert.ok(res.failed >= 1);
    assert.match(res.error, /Ledger does not exist/);
    const failed = get(db, "SELECT COUNT(*) AS n FROM tbl_TallySyncQueue WHERE companycode=? AND status='FAILED'", [COMPANY]);
    assert.equal(failed.n, res.failed);
    // a retry moves them back to PENDING
    const retry = tally.retryFailed(db, COMPANY);
    assert.equal(retry.retried, res.failed);
  } finally { server.close(); db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('live push without a configured endpoint throws', async () => {
  const { db, dir } = freshDb();
  try {
    seedAndQueue(db);
    await assert.rejects(() => tally.livePush(db, COMPANY, YEAR, {}), /not configured/);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('live test probes the endpoint and records a log entry', async () => {
  const { db, dir } = freshDb();
  const { server, port } = await fakeTally();
  try {
    tally.saveConfig(db, COMPANY, { tallyurl: `http://127.0.0.1:${port}` });
    const r = await tally.liveTest(db, COMPANY);
    assert.equal(r.ok, true);
    assert.equal(r.status, 200);
    const logs = tally.logs(db, COMPANY);
    assert.ok(logs.some((l) => l.action === 'TEST'), 'probe is logged');
  } finally { server.close(); db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('saveConfig merges partial updates and preserves the live endpoint fields', () => {
  const { db, dir } = freshDb();
  try {
    tally.saveConfig(db, COMPANY, { tallyurl: 'http://127.0.0.1:9000', tallycompanyname: 'VANIJLIQUOR', tlsinsecure: true });
    tally.saveConfig(db, COMPANY, { brokerurl: 'https://broker.example' }); // partial update
    const cfg = tally.getConfig(db, COMPANY);
    assert.equal(cfg.tallyurl, 'http://127.0.0.1:9000');
    assert.equal(cfg.tallycompanyname, 'VANIJLIQUOR');
    assert.equal(cfg.tlsinsecure, 1);
    assert.equal(cfg.brokerurl, 'https://broker.example');
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});
