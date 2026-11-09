import { all, get, run, tx } from '../db/index.js';
import { money, now } from './helpers.js';
import { createPurchase } from './purchase.js';

/**
 * State excise compliance sync.
 *
 * Track A (sales ledger): every POS sale/return line is captured into
 * tbl_ExciseQueue keyed by the item's excise identity (GTIN / Pack_Size / MRP)
 * and pushed to the state EODB API with a token.
 *
 * Track B (portal): daily closing balances are pushed back to the portal, and
 * indents / transport permits pulled from the portal are recorded for review.
 *
 * Billing never blocks on this: capture is best-effort, the network layer only
 * moves queue rows between PENDING -> ACKED / FAILED.
 */

const PENDING = 'PENDING';
const SENT = 'SENT';
const ACKED = 'ACKED';
const FAILED = 'FAILED';

// ------------------------------------------------------------- config

export function getConfig(db, company) {
  const row = get(db, 'SELECT * FROM tbl_ExciseConfig WHERE companycode=?', [company]);
  if (row) return row;
  return {
    companycode: company, state: 'WB', endpointbase: '', licenseeid: '',
    credential: '', autoupload: 0, lastuploadat: null, updatedat: null,
  };
}

export function saveConfig(db, company, cfg) {
  const cur = get(db, 'SELECT 1 FROM tbl_ExciseConfig WHERE companycode=?', [company]);
  const v = [
    cfg.state || 'WB', cfg.endpointbase || '', cfg.licenseeid || '',
    cfg.credential || '', cfg.autoupload ? 1 : 0, now(),
  ];
  if (cur) {
    run(db, `UPDATE tbl_ExciseConfig SET state=?, endpointbase=?, licenseeid=?, credential=?,
             autoupload=?, updatedat=? WHERE companycode=?`, [...v, company]);
  } else {
    run(db, `INSERT INTO tbl_ExciseConfig
             (state, endpointbase, licenseeid, credential, autoupload, updatedat, companycode)
             VALUES (?,?,?,?,?,?,?)`, [...v, company]);
  }
  return getConfig(db, company);
}

function touchUpload(db, company) {
  run(db, 'UPDATE tbl_ExciseConfig SET lastuploadat=? WHERE companycode=?', [now(), company]);
}

function log(db, queueid, company, direction, action, status, message) {
  run(db, `INSERT INTO tbl_ExciseLog (queueid, companycode, direction, action, status, message, loggedat)
           VALUES (?,?,?,?,?,?,?)`, [queueid || null, company, direction, action, status, message || '', now()]);
}

export function logs(db, company, limit = 200) {
  return all(db, 'SELECT * FROM tbl_ExciseLog WHERE companycode=? ORDER BY id DESC LIMIT ?', [company, limit]);
}

// ------------------------------------------------------------- state adapters

/**
 * West Bengal adapter (API mode). Endpoints and payload shapes are taken
 * verbatim from the legacy app (decode/.../Madhushala Pro.decompiled.cs,
 * Excise_Upload / SendRequest): authorize against CheckAuthorization, push each
 * line to InsertPosData with a Tokencode header.
 */
const WB = {
  state: 'WB',
  mode: 'api',
  label: 'West Bengal (EODB / WBSBCL)',
  defaultBase: 'https://excise.wb.gov.in/APIEODB/EODBPLA',
  authorizeUrl: (base) => `${base}/CheckAuthorization`,
  pushUrl: (base) => `${base}/InsertPosData`,
  closingUrl: (base) => `${base}/InsertClosingBalance`,
  // Legacy builds the login as "<LicenseNo>/01".
  userName: (cfg) => `${String(cfg.licenseeid ?? '').trim()}/01`,
  salesPayload: (cfg, row) => ({
    Licensee_Id_No: String(cfg.licenseeid ?? '').trim(),
    Datetime_Stamp: row.trndate,
    GTIN: row.gtin,
    Pack_Size: String(row.packsize),
    MRP: String(money(row.mrp)),
    Quantity: String(row.quantity),
  }),
  closingPayload: (cfg, row) => ({
    Licensee_Id_No: String(cfg.licenseeid ?? '').trim(),
    Datetime_Stamp: row.balancedate,
    GTIN: row.gtin,
    Pack_Size: String(row.packsize),
    MRP: String(money(row.mrp)),
    Closing_Balance: String(row.closingqty),
  }),
};

/**
 * Odisha adapter (portal mode).
 *
 * Odisha has no published JSON API and the legacy app ships no Odisha code
 * (the vendor's excise page lists Odisha as "Details coming soon"). The real
 * system is the State Excise portal at stateexcise.odisha.gov.in — an ASP.NET
 * application reached through the login page
 * /UserLogIn/Login.aspx — and compliance is done by submitting the POS sales
 * file and the daily closing stock through the portal.
 *
 * So this adapter does not POST to a state API directly. Instead it builds the
 * exact submission batches and hands them to the shop-side portal agent (the
 * Selenium-equivalent of the WB Track-B automation), which signs in and uploads
 * them. Rows are marked SENT until the agent confirms with `confirmUploaded`.
 * The endpoint shapes below are deliberately minimal and configurable
 * (`endpointbase`) so they can be pinned to the real portal contract once it is
 * confirmed with the department.
 */
const OD = {
  state: 'OD',
  mode: 'portal',
  label: 'Odisha (State Excise portal)',
  defaultBase: 'https://stateexcise.odisha.gov.in',
  loginPath: '/UserLogIn/Login.aspx',
  portalPath: '/Portal_New_Default.aspx',
  salesBatchPath: '/writereaddata/POSDataUpload',
  closingBatchPath: '/writereaddata/ClosingBalanceUpload',
  salesPayload: (cfg, row) => ({
    Licensee_Id_No: String(cfg.licenseeid ?? '').trim(),
    Datetime_Stamp: row.trndate,
    GTIN: row.gtin,
    Pack_Size: String(row.packsize),
    MRP: String(money(row.mrp)),
    Quantity: String(row.quantity),
  }),
  closingPayload: (cfg, row) => ({
    Licensee_Id_No: String(cfg.licenseeid ?? '').trim(),
    Datetime_Stamp: row.balancedate,
    GTIN: row.gtin,
    Pack_Size: String(row.packsize),
    MRP: String(money(row.mrp)),
    Closing_Balance: String(row.closingqty),
  }),
};

export const ADAPTERS = { WB, OD };

export function listAdapters() {
  return Object.values(ADAPTERS).map((a) => ({
    state: a.state, label: a.label, mode: a.mode || 'api', defaultBase: a.defaultBase,
  }));
}

function resolveAdapter(state) {
  const a = ADAPTERS[state];
  if (!a) throw new Error(`No excise adapter for state "${state}"`);
  return a;
}

// ------------------------------------------------------------- capture

/** Upsert one queued line (idempotent per bill + excise identity). */
function upsertLine(db, company, year, { trnno, trndate, gtin, packsize, mrp, quantity }) {
  const at = now();
  run(db, `INSERT INTO tbl_ExciseQueue
      (companycode, yearcode, sourcetable, sourcetrnno, trndate, gtin, packsize, mrp, quantity,
       status, createdat, updatedat)
      VALUES (?,?,?,?,?,?,?,?,?,?,?,?)
      ON CONFLICT(companycode, sourcetable, sourcetrnno, gtin, packsize, mrp)
      DO UPDATE SET quantity=excluded.quantity, trndate=excluded.trndate, updatedat=excluded.updatedat`,
    [company, year, 'salesbillmain', trnno, trndate, gtin, String(packsize), money(mrp), quantity, PENDING, at, at]);
}

/**
 * Capture a sale (or sale return) into the excise queue.
 * Returns the number of distinct excise lines written.
 */
export function captureSale(db, { company, year, trnno }) {
  const head = get(db, 'SELECT * FROM salesbillmain WHERE companycode=? AND yearcode=? AND trnno=?',
    [company, year, trnno]);
  if (!head) throw new Error(`Sale ${trnno} not found`);
  const isReturn = String(head.salestype || '').toUpperCase() === 'RETURN';
  const lines = all(db, 'SELECT * FROM salesbilldetail WHERE companycode=? AND yearcode=? AND trnno=?',
    [company, year, trnno]);

  // aggregate per excise identity so repeated SKUs collapse into one line
  const agg = new Map();
  for (const l of lines) {
    const item = get(db, 'SELECT * FROM itemmst WHERE companycode=? AND itemcode=?', [company, l.itemcode]) || {};
    const gtin = String(item.barcode || '').trim();
    const packsize = String(item.ml ?? '');
    const mrp = money(item.salesrate ?? l.MRP ?? 0);
    const qty = (Number(l.qnty) || 0) * (isReturn ? -1 : 1);
    const key = `${gtin}|${packsize}|${mrp}`;
    agg.set(key, (agg.get(key) || 0) + qty);
  }

  let n = 0;
  for (const [key, quantity] of agg) {
    const [gtin, packsize, mrp] = key.split('|');
    upsertLine(db, company, year, { trnno, trndate: head.trndate, gtin, packsize, mrp: Number(mrp), quantity });
    n += 1;
  }
  log(db, null, company, 'OUT', 'CAPTURE', PENDING, `${trnno}: ${n} excise line(s)`);
  return n;
}

/** Capture every sale/return for a year that is not yet in the queue. */
export function captureAll(db, company, year) {
  return tx(db, () => {
    let captured = 0;
    for (const s of all(db, 'SELECT trnno FROM salesbillmain WHERE companycode=? AND yearcode=?', [company, year])) {
      captured += captureSale(db, { company, year, trnno: s.trnno });
    }
    return { captured };
  });
}

// ------------------------------------------------------------- queue reads

export function queue(db, company, { status } = {}) {
  const params = [company];
  let sql = 'SELECT * FROM tbl_ExciseQueue WHERE companycode=?';
  if (status) { sql += ' AND status=?'; params.push(status); }
  sql += ' ORDER BY id DESC LIMIT 500';
  return all(db, sql, params);
}

export function queueSummary(db, company) {
  const rows = all(db, 'SELECT status, COUNT(*) AS n FROM tbl_ExciseQueue WHERE companycode=? GROUP BY status', [company]);
  const out = { PENDING: 0, SENT: 0, ACKED: 0, FAILED: 0 };
  for (const r of rows) out[r.status] = r.n;
  out.total = out.PENDING + out.SENT + out.ACKED + out.FAILED;
  return out;
}

/**
 * Pre-validation gate: rows whose excise identity no longer matches an item
 * (barcode = GTIN, ml = Pack_Size, salesrate = MRP). Mirrors the legacy
 * "unmatched data" grid. Returns the offending queue rows.
 */
export function validateQueue(db, company) {
  return all(db,
    `SELECT q.* FROM tbl_ExciseQueue q
      WHERE q.companycode=?
        AND NOT EXISTS (
          SELECT 1 FROM itemmst i
           WHERE i.companycode = q.companycode
             AND i.barcode = q.gtin
             AND CAST(i.ml AS TEXT) = q.packsize
             AND i.salesrate = q.mrp)
      ORDER BY q.id`,
    [company]);
}

// ------------------------------------------------------------- upload (Track A)

async function authorize(adapter, base, cfg, fetchImpl) {
  const body = JSON.stringify({ User_Name: adapter.userName(cfg), Password: cfg.credential || '' });
  for (let attempt = 0; attempt < 5; attempt += 1) {
    const res = await fetchImpl(adapter.authorizeUrl(base), {
      method: 'POST', headers: { 'Content-Type': 'application/json' }, body,
    });
    if (res.status === 429) continue; // server busy - retry, like the legacy loop
    const text = await res.text();
    let token = null;
    try {
      const parsed = JSON.parse(text);
      const arr = Array.isArray(parsed) ? parsed : [parsed];
      token = arr.find((x) => x && x.TokenCode)?.TokenCode || null;
    } catch { /* non-JSON response */ }
    if (!token) throw new Error(`Excise authorization failed (${res.status}): ${text.slice(0, 200)}`);
    return token;
  }
  throw new Error('Excise server busy, please try again');
}

async function pushRow(url, token, body, fetchImpl) {
  const res = await fetchImpl(url, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json', Tokencode: token },
    body: JSON.stringify(body),
  });
  const text = await res.text();
  if (/success/i.test(text)) return { ok: true, text };
  return { ok: false, text: text || `HTTP ${res.status}` };
}

/**
 * Authorize and push every PENDING queue row.
 * @param {object} opts
 * @param {string} opts.company
 * @param {Function} [opts.fetchImpl] injected fetch (tests use a real local server)
 * @param {string} [opts.state] override the configured state adapter
 * @param {boolean} [opts.force] push even rows that fail the validation gate
 */
export async function uploadQueue(db, { company, fetchImpl = fetch, state, force = false } = {}) {
  const cfg = getConfig(db, company);
  const adapter = resolveAdapter(state || cfg.state);
  const base = (cfg.endpointbase || adapter.defaultBase).replace(/\/+$/, '');
  if (!cfg.licenseeid) throw new Error('Excise licensee id is not configured');

  // Rows that fail the pre-validation gate are held back (unless forced).
  const bad = new Set(validateQueue(db, company).map((r) => r.id));
  const allPending = queue(db, company, { status: PENDING });
  const rows = force ? allPending : allPending.filter((r) => !bad.has(r.id));
  const skipped = allPending.length - rows.length;

  if (adapter.mode === 'portal') {
    // No direct state API: stage the batch for the shop-side portal agent, which
    // signs into the portal and uploads. Rows stay SENT until it confirms.
    const at = now();
    for (const row of rows) {
      run(db, `UPDATE tbl_ExciseQueue SET status=?, attempts=attempts+1, lasterror=NULL, updatedat=?
               WHERE id=?`, [SENT, at, row.id]);
      log(db, row.id, company, 'OUT', 'STAGE', SENT, `${row.gtin} x${row.quantity} staged for portal agent`);
    }
    const batch = buildPortalBatch(db, company, { state: adapter.state });
    touchUpload(db, company);
    return {
      state: adapter.state, mode: 'portal', staged: rows.length, skipped,
      batch: batch.count, portal: adapter.loginPath, total: allPending.length,
    };
  }

  const token = await authorize(adapter, base, cfg, fetchImpl);
  let acked = 0; let failed = 0;

  for (const row of rows) {
    const { ok, text } = await pushRow(adapter.pushUrl(base), token, adapter.salesPayload(cfg, row), fetchImpl);
    if (ok) {
      run(db, `UPDATE tbl_ExciseQueue SET status=?, attempts=attempts+1, ackedat=?, updatedat=?, lasterror=NULL
               WHERE id=?`, [ACKED, now(), now(), row.id]);
      log(db, row.id, company, 'OUT', 'UPLOAD', ACKED, `${row.gtin} x${row.quantity}`);
      acked += 1;
    } else {
      run(db, `UPDATE tbl_ExciseQueue SET status=?, attempts=attempts+1, lasterror=?, updatedat=?
               WHERE id=?`, [FAILED, String(text).slice(0, 500), now(), row.id]);
      log(db, row.id, company, 'OUT', 'UPLOAD', FAILED, String(text).slice(0, 500));
      failed += 1;
    }
  }

  touchUpload(db, company);
  return { state: adapter.state, mode: 'api', authorized: true, acked, failed, skipped, total: allPending.length };
}

/**
 * Build the portal submission batch (Odisha / portal-mode states) for the shop
 * agent to upload after signing into the state excise portal.
 */
export function buildPortalBatch(db, company, { state, status = SENT } = {}) {
  const cfg = getConfig(db, company);
  const adapter = resolveAdapter(state || cfg.state);
  const params = [company, status];
  const sales = all(db,
    `SELECT * FROM tbl_ExciseQueue WHERE companycode=? AND status=? ORDER BY id LIMIT 500`, params);
  const closing = all(db,
    `SELECT * FROM tbl_ExciseClosingBalance WHERE companycode=? AND status=? ORDER BY id LIMIT 500`,
    [company, PENDING]);
  return {
    state: adapter.state,
    mode: adapter.mode,
    portal: { base: (cfg.endpointbase || adapter.defaultBase).replace(/\/+$/, ''), login: adapter.loginPath, home: adapter.portalPath },
    licensee: cfg.licenseeid || '',
    sales: sales.map((r) => adapter.salesPayload(cfg, r)),
    closing: closing.map((r) => adapter.closingPayload(cfg, r)),
    count: sales.length + closing.length,
  };
}

/**
 * Portal agent reports the result of a submission. `results` is
 * [{ id, status: 'ACKED'|'FAILED', message }] for sales lines; `closingIds`
 * lists closing-balance rows that were accepted.
 */
export function confirmUploaded(db, company, { results = [], closingIds = [] } = {}) {
  let acked = 0; let failed = 0;
  for (const r of results) {
    const ok = String(r.status || ACKED).toUpperCase() !== FAILED;
    if (ok) {
      const u = run(db, `UPDATE tbl_ExciseQueue SET status=?, ackedat=?, updatedat=?, lasterror=NULL
                         WHERE id=? AND companycode=?`, [ACKED, now(), now(), r.id, company]);
      if (u.changes) { acked += 1; log(db, r.id, company, 'OUT', 'CONFIRM', ACKED, r.message || 'Uploaded via portal'); }
    } else {
      const u = run(db, `UPDATE tbl_ExciseQueue SET status=?, attempts=attempts+1, lasterror=?, updatedat=?
                         WHERE id=? AND companycode=?`, [FAILED, r.message || 'Rejected by portal', now(), r.id, company]);
      if (u.changes) { failed += 1; log(db, r.id, company, 'OUT', 'CONFIRM', FAILED, r.message || 'Rejected by portal'); }
    }
  }
  let closing = 0;
  for (const id of closingIds) {
    const u = run(db, 'UPDATE tbl_ExciseClosingBalance SET status=?, ackedat=?, updatedat=? WHERE id=? AND companycode=?',
      [ACKED, now(), now(), id, company]);
    closing += u.changes;
  }
  touchUpload(db, company);
  return { acked, failed, closing };
}

/** Move FAILED rows back to PENDING for another attempt. */
export function retryFailed(db, company) {
  const r = run(db, 'UPDATE tbl_ExciseQueue SET status=?, updatedat=? WHERE companycode=? AND status=?',
    [PENDING, now(), company, FAILED]);
  if (r.changes) log(db, null, company, 'OUT', 'RETRY', PENDING, `${r.changes} failed line(s) requeued`);
  return { retried: r.changes };
}

// ------------------------------------------------------------- closing balance (Track B)

/**
 * Build the closing stock for a date, per excise identity, and upsert into
 * tbl_ExciseClosingBalance. Closing qty is net movement up to and including the
 * date (opening + purchases - sales + sale returns - transfers out + in -
 * breakage).
 */
export function buildClosingBalance(db, { company, year, date }) {
  // Six movement branches, each keyed by (company, year, date).
  const cteParams = [];
  for (let i = 0; i < 6; i++) cteParams.push(company, year, date);
  const sql = `
    WITH movements AS (
      SELECT itemcode, CAST(COALESCE(qnty,0) AS INTEGER) AS qty
        FROM openingstockmst WHERE companycode=? AND yearcode=? AND trndate <= ?
      UNION ALL
      SELECT pd.itemcode, CAST(COALESCE(pd.itemquantity,0) AS INTEGER)
        FROM purchasemain pm JOIN purchasedetail pd
          ON pd.companycode=pm.companycode AND pd.yearcode=pm.yearcode AND pd.trnno=pm.trnno
       WHERE pm.companycode=? AND pm.yearcode=? AND pm.ptype='PURCHASE' AND pm.trndate <= ?
      UNION ALL
      SELECT pd.itemcode, -CAST(COALESCE(pd.itemquantity,0) AS INTEGER)
        FROM purchasemain pm JOIN purchasedetail pd
          ON pd.companycode=pm.companycode AND pd.yearcode=pm.yearcode AND pd.trnno=pm.trnno
       WHERE pm.companycode=? AND pm.yearcode=? AND pm.ptype='PURCHASE RETURN' AND pm.trndate <= ?
      UNION ALL
      SELECT sd.itemcode, -CAST(COALESCE(sd.qnty,0) AS INTEGER)
        FROM salesbillmain sm JOIN salesbilldetail sd
          ON sd.companycode=sm.companycode AND sd.yearcode=sm.yearcode AND sd.trnno=sm.trnno
       WHERE sm.companycode=? AND sm.yearcode=? AND (sm.salestype IS NULL OR sm.salestype<>'RETURN') AND sm.trndate <= ?
      UNION ALL
      SELECT sd.itemcode, CAST(COALESCE(sd.qnty,0) AS INTEGER)
        FROM salesbillmain sm JOIN salesbilldetail sd
          ON sd.companycode=sm.companycode AND sd.yearcode=sm.yearcode AND sd.trnno=sm.trnno
       WHERE sm.companycode=? AND sm.yearcode=? AND sm.salestype='RETURN' AND sm.trndate <= ?
      UNION ALL
      SELECT bd.ItemCode, -CAST(COALESCE(bd.quantity,0) AS INTEGER)
        FROM breakagemain bm JOIN breakagedetail bd
          ON bd.CompanyCode=bm.CompanyCode AND bd.yearcode=bm.YearCode AND bd.trnno=bm.TrnNo
       WHERE bm.CompanyCode=? AND bm.YearCode=? AND bm.trndate <= ?
    )
    SELECT i.barcode AS gtin, CAST(i.ml AS TEXT) AS packsize, i.salesrate AS mrp,
           SUM(m.qty) AS closingqty
      FROM movements m JOIN itemmst i
        ON i.companycode=? AND i.itemcode=m.itemcode
     WHERE i.barcode IS NOT NULL AND i.barcode<>''
     GROUP BY i.barcode, i.ml, i.salesrate`;

  const rows = all(db, sql, [...cteParams, company]);
  const at = now();
  let n = 0;
  for (const r of rows) {
    run(db, `INSERT INTO tbl_ExciseClosingBalance
        (companycode, yearcode, balancedate, gtin, packsize, mrp, closingqty, status, createdat, updatedat)
        VALUES (?,?,?,?,?,?,?,?,?,?)
        ON CONFLICT(companycode, balancedate, gtin, packsize, mrp)
        DO UPDATE SET closingqty=excluded.closingqty, status=?, updatedat=?`,
      [company, year, date, r.gtin, String(r.packsize), money(r.mrp), Number(r.closingqty) || 0, PENDING, at, at,
        PENDING, at]);
    n += 1;
  }
  return { date, lines: n };
}

export function listClosingBalance(db, company, date) {
  const params = [company];
  let sql = 'SELECT * FROM tbl_ExciseClosingBalance WHERE companycode=?';
  if (date) { sql += ' AND balancedate=?'; params.push(date); }
  sql += ' ORDER BY balancedate DESC, id DESC LIMIT 500';
  return all(db, sql, params);
}

export async function uploadClosingBalance(db, { company, fetchImpl = fetch, state, date } = {}) {
  const cfg = getConfig(db, company);
  const adapter = resolveAdapter(state || cfg.state);
  const base = (cfg.endpointbase || adapter.defaultBase).replace(/\/+$/, '');
  if (!cfg.licenseeid) throw new Error('Excise licensee id is not configured');

  const params = [company];
  let sql = 'SELECT * FROM tbl_ExciseClosingBalance WHERE companycode=? AND status=?';
  params.push(PENDING);
  if (date) { sql += ' AND balancedate=?'; params.push(date); }
  const rows = all(db, sql, params);

  // Portal mode: hand the closing stock to the agent via the batch (no direct API).
  if (adapter.mode === 'portal') {
    const batch = buildPortalBatch(db, company, { state: adapter.state });
    touchUpload(db, company);
    return { state: adapter.state, mode: 'portal', staged: rows.length, closing: batch.closing.length, total: rows.length };
  }

  const token = await authorize(adapter, base, cfg, fetchImpl);

  let acked = 0; let failed = 0;
  for (const row of rows) {
    const { ok, text } = await pushRow(adapter.closingUrl(base), token, adapter.closingPayload(cfg, row), fetchImpl);
    if (ok) {
      run(db, 'UPDATE tbl_ExciseClosingBalance SET status=?, attempts=attempts+1, ackedat=?, updatedat=? WHERE id=?',
        [ACKED, now(), now(), row.id]);
      log(db, null, company, 'OUT', 'CLOSING', ACKED, `${row.gtin} ${row.balancedate}`);
      acked += 1;
    } else {
      run(db, 'UPDATE tbl_ExciseClosingBalance SET status=?, attempts=attempts+1, lasterror=?, updatedat=? WHERE id=?',
        [FAILED, String(text).slice(0, 500), now(), row.id]);
      log(db, null, company, 'OUT', 'CLOSING', FAILED, String(text).slice(0, 500));
      failed += 1;
    }
  }
  touchUpload(db, company);
  return { state: adapter.state, mode: 'api', acked, failed, total: rows.length };
}

// ------------------------------------------------------------- portal inbound (Track B)

export function listInbound(db, company, status) {
  const params = [company];
  let sql = 'SELECT * FROM tbl_ExcisePortalInbound WHERE companycode=?';
  if (status) { sql += ' AND status=?'; params.push(status); }
  sql += ' ORDER BY id DESC';
  return all(db, sql, params);
}

/** Record an indent / transport permit pulled from the portal. */
export function receiveInbound(db, company, doc) {
  const at = now();
  const existing = get(db, 'SELECT id FROM tbl_ExcisePortalInbound WHERE companycode=? AND doctype=? AND docno=?',
    [company, doc.doctype, doc.docno]);
  if (existing) {
    run(db, `UPDATE tbl_ExcisePortalInbound SET docdate=?, tppassno=?, payload=?, status='RECEIVED',
             receivedat=?, appliedat=NULL WHERE id=?`,
      [doc.docdate, doc.tppassno || null, JSON.stringify(doc), at, existing.id]);
    return existing.id;
  }
  const r = run(db, `INSERT INTO tbl_ExcisePortalInbound
      (companycode, doctype, docno, docdate, tppassno, payload, status, receivedat)
      VALUES (?,?,?,?,?,?,?,?)`,
    [company, doc.doctype, doc.docno, doc.docdate, doc.tppassno || null, JSON.stringify(doc), 'RECEIVED', at]);
  log(db, null, company, 'IN', 'RECEIVE', 'RECEIVED', `${doc.doctype} ${doc.docno}`);
  return r.lastInsertRowid;
}

/**
 * Apply a received indent/TP into a purchase (stock + TP pass number).
 * `doc.lines` is [{ itemcode, quantity, rate }].
 */
export function applyInbound(db, company, id) {
  const row = get(db, 'SELECT * FROM tbl_ExcisePortalInbound WHERE id=? AND companycode=?', [id, company]);
  if (!row) throw new Error(`Inbound document ${id} not found`);
  const doc = JSON.parse(row.payload || '{}');
  const year = doc.year || get(db, 'SELECT yearcode FROM yearmst WHERE companycode=? ORDER BY yearcode DESC LIMIT 1', [company])?.yearcode;

  const result = createPurchase(db, {
    company,
    year,
    trndate: doc.docdate,
    ptype: 'PURCHASE',
    suppliercode: doc.suppliercode || null,
    purchaseacccode: doc.ledgercode || 'L00004',
    shopcode: doc.shopcode || 'MAIN',
    docno: doc.docno,
    docdate: doc.docdate,
    tppassno: doc.tppassno || '',
    narration: `Excise portal ${doc.doctype} ${doc.docno}`,
    lines: (doc.lines || []).map((l) => ({
      itemcode: l.itemcode, batchno: l.batchno || '0', itemrate: Number(l.rate) || 0,
      itemquantity: Number(l.quantity) || 0, itemmrp: l.mrp || 0,
    })),
  });
  run(db, "UPDATE tbl_ExcisePortalInbound SET status='APPLIED', appliedat=? WHERE id=?", [now(), id]);
  log(db, null, company, 'IN', 'APPLY', 'APPLIED', `${row.doctype} ${row.docno} -> purchase ${result.trnno}`);
  return { id, trnno: result.trnno, applied: true };
}

// ------------------------------------------------------------- report

export function syncReport(db, company) {
  return {
    summary: queueSummary(db, company),
    closing: all(db, `SELECT status, COUNT(*) AS n FROM tbl_ExciseClosingBalance
                      WHERE companycode=? GROUP BY status`, [company]),
    inbound: all(db, `SELECT doctype, status, COUNT(*) AS n FROM tbl_ExcisePortalInbound
                      WHERE companycode=? GROUP BY doctype, status`, [company]),
    unmatched: validateQueue(db, company).length,
    logs: logs(db, company, 50),
  };
}
