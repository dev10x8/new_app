import { request as httpRequest } from 'node:http';
import { request as httpsRequest } from 'node:https';
import { all, get, run, tx } from '../db/index.js';
import { money, now } from './helpers.js';

const SALES = 'SALES';
const PURCHASE = 'PURCHASE';

const xesc = (s) => String(s ?? '').replace(/&/g, '&amp;').replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/"/g, '&quot;');
const attr = (s) => String(s ?? '').replace(/"/g, '&quot;');
const ymd = (d) => String(d || '').slice(0, 10).replace(/-/g, '');
const num = (n) => String(money(n));

// ------------------------------------------------------------- config

export function getConfig(db, company) {
  const row = get(db, 'SELECT * FROM tbl_TallySyncConfig WHERE companycode=?', [company]);
  if (row) return row;
  return {
    companycode: company, enabled: 0, brokerurl: '', agenttoken: '',
    companyguid: '', tallycompany: '', agentid: '', lastpullat: null, lastpushat: null,
    tallyurl: '', tallycompanyname: '', tlsinsecure: 0,
  };
}

export function saveConfig(db, company, cfg) {
  const cur = get(db, 'SELECT * FROM tbl_TallySyncConfig WHERE companycode=?', [company]);
  // Merge onto the current config so a partial update never clears other fields.
  const c = { ...(cur || getConfig(db, company)), ...cfg };
  const at = now();
  const v = [
    c.enabled ? 1 : 0,
    c.brokerurl || '', c.agenttoken || '', c.companyguid || '',
    c.tallycompany || '', c.agentid || '', c.tallyurl || '',
    c.tallycompanyname || '', c.tlsinsecure ? 1 : 0, at,
  ];
  if (cur) {
    run(db, `UPDATE tbl_TallySyncConfig SET enabled=?, brokerurl=?, agenttoken=?, companyguid=?,
             tallycompany=?, agentid=?, tallyurl=?, tallycompanyname=?, tlsinsecure=?, updatedat=?
             WHERE companycode=?`, [...v, company]);
  } else {
    run(db, `INSERT INTO tbl_TallySyncConfig
             (enabled, brokerurl, agenttoken, companyguid, tallycompany, agentid,
              tallyurl, tallycompanyname, tlsinsecure, updatedat, companycode)
             VALUES (?,?,?,?,?,?,?,?,?,?,?)`, [...v, company]);
  }
  return getConfig(db, company);
}

/** Generate a token for the shop agent (stand-in for the cloud broker handshake). */
export function newAgentToken() {
  const rnd = () => Math.random().toString(36).slice(2, 10);
  return `TALLY-${rnd()}${rnd()}`.toUpperCase();
}

/** Guard the agent-facing endpoints with the configured token. */
export function requireToken(cfg, token) {
  if (cfg.enabled && cfg.agenttoken && String(token || '') !== cfg.agenttoken) {
    throw Object.assign(new Error('Invalid agent token'), { status: 401 });
  }
}

export function touchPull(db, company) {
  run(db, 'UPDATE tbl_TallySyncConfig SET lastpullat=? WHERE companycode=?', [now(), company]);
}

export function touchPush(db, company) {
  run(db, 'UPDATE tbl_TallySyncConfig SET lastpushat=? WHERE companycode=?', [now(), company]);
}

// ------------------------------------------------------------- mapping

export function listMapping(db, company, maptype) {
  const params = [company];
  let sql = 'SELECT * FROM tbl_TallyMapping WHERE companycode=?';
  if (maptype) { sql += ' AND maptype=?'; params.push(maptype); }
  sql += ' ORDER BY maptype, priority DESC, id';
  return all(db, sql, params);
}

export function addMapping(db, m) {
  const r = run(db, `INSERT INTO tbl_TallyMapping (companycode, maptype, pattern, targetname, priority, active)
                     VALUES (?,?,?,?,?,?)`,
    [m.company, m.maptype, m.pattern, m.targetname || '', Number(m.priority) || 0, m.active === false ? 0 : 1]);
  return get(db, 'SELECT * FROM tbl_TallyMapping WHERE id=?', [r.lastInsertRowid]);
}

export function deleteMapping(db, id) {
  return run(db, 'DELETE FROM tbl_TallyMapping WHERE id=?', [id]).changes > 0;
}

/** Resolve a POS value to a Tally master using the configured string patterns. */
export function resolveMapping(db, company, maptype, value) {
  const rules = listMapping(db, company, maptype).filter((r) => r.active);
  const v = String(value ?? '').toLowerCase();
  for (const r of rules) {
    const pat = String(r.pattern || '').toLowerCase();
    if (pat && v.includes(pat)) return r.targetname;
  }
  return null;
}

// ------------------------------------------------------------- voucher payload

function salesPayload(db, company, year, row) {
  const params = get(db, 'SELECT * FROM companyparamtr WHERE companycode=?', [company]) || {};
  const salesLed = resolveMapping(db, company, 'LEDGER', params.SaleAccount_Head) || params.SaleAccount_Head || 'Sales';
  const cashLed = resolveMapping(db, company, 'LEDGER', row.ledgercode) || row.ledgercode;
  const discLed = params.discntacc || 'Discount Allowed';
  const lines = all(db, 'SELECT * FROM salesbilldetail WHERE companycode=? AND yearcode=? AND trnno=?',
    [company, year, row.trnno]);

  const legs = [];
  legs.push({ ledname: cashLed, amount: Number(row.netamount || 0), side: 'Dr' });
  legs.push({ ledname: salesLed, amount: -Number(row.amount || 0), side: 'Cr' });
  if (Number(row.discamount || 0) > 0) legs.push({ ledname: discLed, amount: Number(row.discamount || 0), side: 'Dr' });

  const inventory = lines.map((l) => {
    const item = get(db, 'SELECT * FROM itemmst WHERE companycode=? AND itemcode=?', [company, l.itemcode]) || {};
    return {
      stockitem: resolveMapping(db, company, 'ITEM', `${l.itemcode} ${item.itemname || ''}`) || item.itemname || l.itemcode,
      qty: Number(l.qnty || 0), rate: Number(l.rate || 0), amount: Number(l.itemAmount || 0),
    };
  });
  return { date: row.trndate, voucherNumber: row.trnno, partyLedger: cashLed, legs, inventory };
}

function purchasePayload(db, company, year, row) {
  const params = get(db, 'SELECT * FROM companyparamtr WHERE companycode=?', [company]) || {};
  const purchLed = params.purchaseacchd || 'Purchase';
  const partyLed = resolveMapping(db, company, 'LEDGER', row.suppliercode) || row.suppliercode;
  const lines = all(db, 'SELECT * FROM purchasedetail WHERE companycode=? AND yearcode=? AND trnno=?',
    [company, year, row.trnno]);
  const legs = [
    { ledname: purchLed, amount: Number(row.totnetamt || 0), side: 'Dr' },
    { ledname: partyLed, amount: -Number(row.totnetamt || 0), side: 'Cr' },
  ];
  const inventory = lines.map((l) => {
    const item = get(db, 'SELECT * FROM itemmst WHERE companycode=? AND itemcode=?', [company, l.itemcode]) || {};
    return {
      stockitem: resolveMapping(db, company, 'ITEM', `${l.itemcode} ${item.itemname || ''}`) || item.itemname || l.itemcode,
      qty: Number(l.itemquantity || 0), rate: Number(l.itemrate || 0), amount: Number(l.itemamount || 0),
    };
  });
  return { date: row.trndate, voucherNumber: row.trnno, partyLedger: partyLed, legs, inventory };
}

// ------------------------------------------------------------- queue

function enqueue(db, company, year, type, table, row) {
  const isSales = type === SALES;
  const amount = isSales ? row.netamount : row.totnetamt;
  const payload = isSales ? salesPayload(db, company, year, row) : purchasePayload(db, company, year, row);
  const at = now();
  const existing = get(db,
    'SELECT id FROM tbl_TallySyncQueue WHERE companycode=? AND yearcode=? AND sourcetable=? AND sourcetrnno=?',
    [company, year, table, row.trnno]);
  if (existing) return { id: existing.id, created: false };
  const r = run(db, `INSERT INTO tbl_TallySyncQueue
      (companycode, yearcode, vouchertype, sourcetable, sourcetrnno, docdate, docno, amount, payload, status, createdat, updatedat)
      VALUES (?,?,?,?,?,?,?,?,?,?,?,?)`,
    [company, year, type, table, row.trnno, row.trndate, row.trnno, Number(amount || 0),
      JSON.stringify(payload), 'PENDING', at, at]);
  log(db, r.lastInsertRowid, company, 'OUT', 'ENQUEUE', 'PENDING', `${type} ${row.trnno}`);
  return { id: r.lastInsertRowid, created: true };
}

export function enqueueSale(db, company, year, trnno) {
  const row = get(db, 'SELECT * FROM salesbillmain WHERE companycode=? AND yearcode=? AND trnno=?', [company, year, trnno]);
  if (!row) throw new Error(`Sale ${trnno} not found`);
  return enqueue(db, company, year, SALES, 'salesbillmain', row).id;
}

export function enqueuePurchase(db, company, year, trnno) {
  const row = get(db, 'SELECT * FROM purchasemain WHERE companycode=? AND yearcode=? AND trnno=?', [company, year, trnno]);
  if (!row) throw new Error(`Purchase ${trnno} not found`);
  return enqueue(db, company, year, PURCHASE, 'purchasemain', row).id;
}

/** Enqueue every sale/purchase not yet queued (the agent's sweep). */
export function enqueueAll(db, company, year) {
  const out = tx(db, () => {
    let sales = 0; let purchases = 0;
    for (const s of all(db, 'SELECT * FROM salesbillmain WHERE companycode=? AND yearcode=?', [company, year])) {
      if (enqueue(db, company, year, SALES, 'salesbillmain', s).created) sales += 1;
    }
    for (const p of all(db, 'SELECT * FROM purchasemain WHERE companycode=? AND yearcode=?', [company, year])) {
      if (enqueue(db, company, year, PURCHASE, 'purchasemain', p).created) purchases += 1;
    }
    return { sales, purchases };
  });
  return { ...out, queued: out.sales + out.purchases };
}

export function queue(db, company, { status } = {}) {
  const params = [company];
  let sql = 'SELECT * FROM tbl_TallySyncQueue WHERE companycode=?';
  if (status) { sql += ' AND status=?'; params.push(status); }
  sql += ' ORDER BY id DESC LIMIT 500';
  return all(db, sql, params);
}

export function queueSummary(db, company) {
  const rows = all(db, `SELECT status, COUNT(*) AS n FROM tbl_TallySyncQueue WHERE companycode=? GROUP BY status`, [company]);
  const out = { PENDING: 0, SENT: 0, ACKED: 0, FAILED: 0 };
  for (const r of rows) out[r.status] = r.n;
  out.total = Object.values(out).reduce((a, b) => a + b, 0);
  return out;
}

function log(db, queueid, company, direction, action, status, message) {
  run(db, `INSERT INTO tbl_TallySyncLog (queueid, companycode, direction, action, status, message, loggedat)
           VALUES (?,?,?,?,?,?,?)`, [queueid || null, company, direction, action, status, message || '', now()]);
}

export function logs(db, company, limit = 200) {
  return all(db, 'SELECT * FROM tbl_TallySyncLog WHERE companycode=? ORDER BY id DESC LIMIT ?', [company, limit]);
}

// ------------------------------------------------------------- XML

/** Render one voucher payload as Tally Prime import XML. */
export function buildVoucherXml(company, v) {
  const guid = `${company}-${v.voucherNumber}`;
  const legs = v.legs.map((l) => `        <ALLLEDGERENTRIES.LIST>
          <LEDGERNAME>${xesc(l.ledname)}</LEDGERNAME>
          <ISDEEMEDPOSITIVE>${l.amount < 0 ? 'Yes' : 'No'}</ISDEEMEDPOSITIVE>
          <AMOUNT>${num(l.amount)}</AMOUNT>
        </ALLLEDGERENTRIES.LIST>`).join('\n');
  const inv = v.inventory.length
    ? `      <INVENTORYENTRIES.LIST>\n` + v.inventory.map((i) => `        <STOCKITEMNAME>${xesc(i.stockitem)}</STOCKITEMNAME>
        <ISDEEMEDPOSITIVE>Yes</ISDEEMEDPOSITIVE>
        <RATE>${num(i.rate)}</RATE>
        <AMOUNT>${num(-i.amount)}</AMOUNT>
        <ACTUALQTY>${num(i.qty)}</ACTUALQTY>
        <BILLEDQTY>${num(i.qty)}</BILLEDQTY>`).join('\n') + `\n      </INVENTORYENTRIES.LIST>`
    : '';
  return `    <VOUCHER REMOTEID="${attr(guid)}" VCHTYPE="${attr(v.vouchertype || 'Sales')}" ACTION="Create">
      <DATE>${ymd(v.date)}</DATE>
      <VOUCHERNUMBER>${xesc(v.voucherNumber)}</VOUCHERNUMBER>
      <PARTYLEDGERNAME>${xesc(v.partyLedger)}</PARTYLEDGERNAME>
      <PERSISTEDVIEW>Invoice Voucher View</PERSISTEDVIEW>
${inv}
${legs}
    </VOUCHER>`;
}

function envelope(company, vouchers) {
  return `<?xml version="1.0" encoding="utf-8"?>
<ENVELOPE>
  <HEADER>
    <TALLYREQUEST>Import Data</TALLYREQUEST>
    <TYPE>Vouchers</TYPE>
    <SUBTYPE>Vouchers</SUBTYPE>
    <TALLYTAG></TALLYTAG>
  </HEADER>
  <BODY>
    <DESC>
      <STATICVARIABLES>
        <SVCURRENTCOMPANY>${xesc(company)}</SVCURRENTCOMPANY>
      </STATICVARIABLES>
    </DESC>
    <DATA>
${vouchers.map((v) => buildVoucherXml(company, v)).join('\n')}
    </DATA>
  </BODY>
</ENVELOPE>`;
}

/** Voucher envelope for the shop agent to submit to the cloud broker. */
export function buildVoucherXmlBatch(db, company, year, { ids, all: useAll, limit = 100, svc } = {}) {
  const params = [company];
  let sql = "SELECT * FROM tbl_TallySyncQueue WHERE companycode=? AND status IN ('PENDING','SENT')";
  if (ids && ids.length) { sql += ` AND id IN (${ids.map(() => '?').join(',')})`; params.push(...ids); }
  sql += ' ORDER BY id LIMIT ?';
  params.push(limit);
  const rows = all(db, sql, params);
  const cfg = getConfig(db, company);
  const svcCompany = svc || cfg.tallycompany || company;
  const vouchers = rows.map((r) => {
    // Rebuild from source so late mapping-rule changes are reflected.
    const src = get(db, `SELECT * FROM ${r.sourcetable} WHERE companycode=? AND yearcode=? AND trnno=?`,
      [company, r.yearcode, r.sourcetrnno]);
    const payload = !src ? JSON.parse(r.payload || '{}')
      : r.vouchertype === PURCHASE ? purchasePayload(db, company, r.yearcode, src)
        : salesPayload(db, company, r.yearcode, src);
    return { ...payload, vouchertype: r.vouchertype === SALES ? 'Sales' : 'Purchase' };
  });
  return {
    company: svcCompany,
    count: vouchers.length,
    ids: rows.map((r) => r.id),
    xml: envelope(svcCompany, vouchers),
  };
}

/** Envelope of Tally masters (the accountant pushes these back). */
export function buildMasterXml(company, masters) {
  const items = masters.map((m) => `    <MASTER NAME="${attr(m.name)}" TYPE="${attr(m.type || 'Ledger')}">
      <PARENT>${xesc(m.parent || '')}</PARENT>
      <GSTIN>${xesc(m.gstin || '')}</GSTIN>
    </MASTER>`).join('\n');
  return `<?xml version="1.0" encoding="utf-8"?>
<ENVELOPE>
  <HEADER>
    <TALLYREQUEST>Import Data</TALLYREQUEST>
    <TYPE>Masters</TYPE>
  </HEADER>
  <BODY>
    <DESC><STATICVARIABLES><SVCURRENTCOMPANY>${xesc(company)}</SVCURRENTCOMPANY></STATICVARIABLES></DESC>
    <DATA>
${items}
    </DATA>
  </BODY>
</ENVELOPE>`;
}

// ------------------------------------------------------------- processing

export function markSent(db, company, ids) {
  const list = (ids || []).map(Number).filter(Boolean);
  if (!list.length) return { sent: 0 };
  let sent = 0;
  for (const id of list) {
    const r = run(db, `UPDATE tbl_TallySyncQueue SET status='SENT', attempts=attempts+1, updatedat=?
                       WHERE id=? AND companycode=? AND status='PENDING'`, [now(), id, company]);
    if (r.changes) { sent += 1; log(db, id, company, 'OUT', 'SEND', 'SENT', 'Submitted to cloud broker'); }
  }
  return { sent };
}

/** Agent acknowledges delivery (or rejection) of queued vouchers. */
export function acknowledge(db, company, results) {
  const list = results || [];
  let acked = 0; let failed = 0;
  for (const r of list) {
    const ok = String(r.status || 'ACKED').toUpperCase() !== 'FAILED';
    if (ok) {
      const u = run(db, `UPDATE tbl_TallySyncQueue SET status='ACKED', ackedat=?, updatedat=?, lasterror=NULL
                         WHERE id=? AND companycode=?`, [now(), now(), r.id, company]);
      if (u.changes) { acked += 1; log(db, r.id, company, 'OUT', 'ACK', 'ACKED', r.message || 'Delivered to Tally'); }
    } else {
      const u = run(db, `UPDATE tbl_TallySyncQueue SET status='FAILED', attempts=attempts+1, lasterror=?, updatedat=?
                         WHERE id=? AND companycode=?`, [r.message || 'Rejected by Tally', now(), r.id, company]);
      if (u.changes) { failed += 1; log(db, r.id, company, 'OUT', 'ACK', 'FAILED', r.message || 'Rejected by Tally'); }
    }
  }
  return { acked, failed };
}

/** Move FAILED rows back to PENDING for another attempt (controlled retry). */
export function retryFailed(db, company) {
  const at = now();
  const r = run(db, `UPDATE tbl_TallySyncQueue SET status='PENDING', updatedat=? WHERE companycode=? AND status='FAILED'`, [at, company]);
  if (r.changes) log(db, null, company, 'OUT', 'RETRY', 'PENDING', `${r.changes} failed voucher(s) requeued`);
  return { retried: r.changes };
}

// ------------------------------------------------------------- inbound masters

export function listInbound(db, company, status) {
  const params = [company];
  let sql = 'SELECT * FROM tbl_TallyInbound WHERE companycode=?';
  if (status) { sql += ' AND status=?'; params.push(status); }
  sql += ' ORDER BY id DESC';
  return all(db, sql, params);
}

/** Record a master pushed from corporate Tally (pending review at the counter). */
export function receiveMaster(db, company, m) {
  const at = now();
  const code = m.code || m.name;
  const existing = get(db, 'SELECT id FROM tbl_TallyInbound WHERE companycode=? AND mtype=? AND (mcode=? OR mname=?)',
    [company, m.type, code, m.name]);
  if (existing) {
    run(db, `UPDATE tbl_TallyInbound SET mcode=?, mname=?, payload=?, status='RECEIVED', receivedat=?, appliedat=NULL WHERE id=?`,
      [code, m.name, JSON.stringify(m), at, existing.id]);
    log(db, null, company, 'IN', 'RECEIVE', 'RECEIVED', `${m.type} ${m.name}`);
    return existing.id;
  }
  const r = run(db, `INSERT INTO tbl_TallyInbound (companycode, mtype, mcode, mname, payload, status, receivedat)
                     VALUES (?,?,?,?,?,?,?)`, [company, m.type, code, m.name, JSON.stringify(m), 'RECEIVED', at]);
  log(db, null, company, 'IN', 'RECEIVE', 'RECEIVED', `${m.type} ${m.name}`);
  return r.lastInsertRowid;
}

/** Apply a received master into the local master table (upsert). */
export function applyInbound(db, company, id) {
  return tx(db, () => {
    const row = get(db, 'SELECT * FROM tbl_TallyInbound WHERE id=? AND companycode=?', [id, company]);
    if (!row) throw new Error(`Inbound master ${id} not found`);
    const m = JSON.parse(row.payload || '{}');
    if (row.mtype === 'LEDGER') {
      const led = m.code || `T${id}`;
      const exists = get(db, 'SELECT 1 FROM ledger WHERE companycode=? AND ledcode=?', [company, led]);
      if (exists) run(db, 'UPDATE ledger SET name=? WHERE companycode=? AND ledcode=?', [m.name, company, led]);
      else run(db, 'INSERT INTO ledger (name, accode, ledcode, companycode, type) VALUES (?,?,?,?,?)',
        [m.name, m.parent || 'AC0003', led, company, 'Party']);
    } else if (row.mtype === 'STOCKITEM') {
      const code = m.code || `I${id}`;
      const exists = get(db, 'SELECT 1 FROM itemmst WHERE companycode=? AND itemcode=?', [company, code]);
      if (exists) run(db, 'UPDATE itemmst SET itemname=?, salesrate=? WHERE companycode=? AND itemcode=?',
        [m.name, Number(m.rate) || 0, company, code]);
      else run(db, `INSERT INTO itemmst
          (itemcode, itemname, salesrate, packing, ml, purchase_discount_amount,
           purchase_salestaxonmrp_amount, purchase_taxscheme_amount, companycode)
          VALUES (?,?,?,?,?,?,?,?,?)`,
        [code, m.name, Number(m.rate) || 0, Number(m.packing) || 1, Number(m.ml) || 0, 0, 0, 0, company]);
    }
    run(db, `UPDATE tbl_TallyInbound SET status='APPLIED', appliedat=? WHERE id=?`, [now(), id]);
    log(db, null, company, 'IN', 'APPLY', 'APPLIED', `${row.mtype} ${row.mname}`);
    return { id, applied: true };
  });
}

// ------------------------------------------------------------- live Tally push

/**
 * POST a Tally XML envelope to the configured Tally Prime HTTP endpoint.
 *
 * Tally Prime (Release 3.0+) exposes an HTTP server (F1 > Settings > Advanced
 * Configuration > "Enable HTTP/XML") that accepts the standard import envelope
 * on its port (9000 by default) and answers with an ENVELOPE that carries
 * <LINEERROR>/<ERRORS> elements. `node:http` is used rather than `fetch` so the
 * request goes out as `text/xml`, which is what Tally expects.
 *
 * @param {string} url      full endpoint, e.g. http://127.0.0.1:9000
 * @param {string} xml      the envelope to import
 * @param {object} [opts]   { timeout = 15000, insecure = false }
 */
export function postTallyXml(url, xml, { timeout = 15000, insecure = false } = {}) {
  return new Promise((resolve, reject) => {
    let target;
    try { target = new URL(url); } catch { reject(new Error(`Invalid Tally URL "${url}"`)); return; }
    const isHttps = target.protocol === 'https:';
    const send = isHttps ? httpsRequest : httpRequest;
    const req = send({
      protocol: target.protocol,
      hostname: target.hostname,
      port: target.port || (isHttps ? 443 : 80),
      path: `${target.pathname}${target.search}` || '/',
      method: 'POST',
      headers: {
        'Content-Type': 'text/xml; charset=utf-8',
        'Content-Length': Buffer.byteLength(xml),
      },
      rejectUnauthorized: isHttps ? !insecure : undefined,
    }, (res) => {
      let body = '';
      res.setEncoding('utf8');
      res.on('data', (c) => { body += c; });
      res.on('end', () => resolve({ status: res.statusCode, body }));
    });
    req.on('error', reject);
    req.setTimeout(timeout, () => req.destroy(new Error(`Tally request timed out after ${timeout}ms`)));
    req.write(xml);
    req.end();
  });
}

const lineErrors = (body) => {
  const out = [];
  const re = /<LINEERROR>([\s\S]*?)<\/LINEERROR>/gi;
  let m = re.exec(body || '');
  while (m) { out.push(m[1].trim()); m = re.exec(body); }
  const errs = /<ERRORS>(\d+)<\/ERRORS>/i.exec(body || '');
  if (Number(errs?.[1]) > 0 && !out.length) out.push('Tally reported errors');
  return out;
};

/** A minimal "is Tally listening" probe envelope (exports the company collection). */
function probeXml() {
  return `<?xml version="1.0" encoding="utf-8"?>
<ENVELOPE>
  <HEADER><TALLYREQUEST>Export Data</TALLYREQUEST><TYPE>Collection</TYPE><ID>Company</ID></HEADER>
  <BODY><DESC><STATICVARIABLES><SVEXPORTFORMAT>$$SysName:XML</SVEXPORTFORMAT></STATICVARIABLES></DESC></BODY>
</ENVELOPE>`;
}

/** Probe the configured Tally endpoint without importing anything. */
export async function liveTest(db, company) {
  const cfg = getConfig(db, company);
  if (!cfg.tallyurl) throw new Error('Tally endpoint URL is not configured');
  const res = await postTallyXml(cfg.tallyurl, probeXml(), { insecure: !!cfg.tlsinsecure });
  const ok = res.status === 200;
  log(db, null, company, 'OUT', 'TEST', ok ? 'OK' : 'FAILED', `Probe ${cfg.tallyurl} -> HTTP ${res.status}`);
  return { ok, status: res.status, bytes: (res.body || '').length };
}

/**
 * Live push: import the pending vouchers straight into Tally Prime over HTTP.
 * On success the queue rows move to ACKED; failures are recorded with the
 * LINEERROR text and left for a retry. Billing stays offline - this only moves
 * the queue.
 */
export async function livePush(db, company, year, { limit = 100, ids } = {}) {
  const cfg = getConfig(db, company);
  if (!cfg.tallyurl) throw new Error('Tally endpoint URL is not configured');
  const batch = buildVoucherXmlBatch(db, company, year, { ids, limit });
  if (!batch.count) return { pushed: 0, acked: 0, failed: 0, message: 'Queue is empty' };

  const res = await postTallyXml(cfg.tallyurl, batch.xml, { insecure: !!cfg.tlsinsecure });
  const errors = lineErrors(res.body);
  const at = now();

  if (res.status === 200 && errors.length === 0) {
    for (const id of batch.ids) {
      run(db, `UPDATE tbl_TallySyncQueue SET status='ACKED', attempts=attempts+1, ackedat=?, updatedat=?, lasterror=NULL
               WHERE id=? AND companycode=?`, [at, at, id, company]);
    }
    log(db, null, company, 'OUT', 'LIVE', 'ACKED', `${batch.count} voucher(s) imported to Tally Prime`);
    touchPush(db, company);
    return { pushed: batch.count, acked: batch.count, failed: 0, status: res.status };
  }

  const msg = (errors.join('; ') || `HTTP ${res.status}`).slice(0, 500);
  for (const id of batch.ids) {
    run(db, `UPDATE tbl_TallySyncQueue SET status='FAILED', attempts=attempts+1, lasterror=?, updatedat=?
             WHERE id=? AND companycode=?`, [msg, at, id, company]);
  }
  log(db, null, company, 'OUT', 'LIVE', 'FAILED', msg);
  return { pushed: 0, acked: 0, failed: batch.count, status: res.status, error: msg };
}

// ------------------------------------------------------------- sync report

export function syncReport(db, company, year) {
  const summary = queueSummary(db, company);
  const byDay = all(db, `SELECT substr(createdat,1,10) AS day, COUNT(*) AS queued,
                                SUM(CASE WHEN status='ACKED' THEN 1 ELSE 0 END) AS acked
                           FROM tbl_TallySyncQueue WHERE companycode=? AND yearcode=?
                          GROUP BY substr(createdat,1,10) ORDER BY day DESC`, [company, year]);
  const inbound = all(db, `SELECT mtype, status, COUNT(*) AS n FROM tbl_TallyInbound
                           WHERE companycode=? GROUP BY mtype, status`, [company]);
  return { summary, byDay, inbound, logs: logs(db, company, 50) };
}
