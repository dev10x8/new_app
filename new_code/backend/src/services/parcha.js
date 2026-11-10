import { all, get, run } from '../db/index.js';
import { money, now } from './helpers.js';
import { inflateSync, inflateRawSync } from 'node:zlib';
import { createPurchase } from './purchase.js';
import { computePurchase, applyMarginToItem, num } from './landedcost.js';
import { parseDelimited, sniffDelimiter, rowsToObjects } from '../util/tabular.js';
import { existsSync, mkdtempSync, writeFileSync, readFileSync, rmSync } from 'node:fs';
import { tmpdir } from 'node:os';
import { join } from 'node:path';
import { spawnSync } from 'node:child_process';

/**
 * Parcha (wholesale purchase invoice) import.
 *
 * The legacy app could read a state-format purchase PDF and post it as a
 * purchase. This rebuild generalises the idea and closes the gaps the legacy
 * had: it accepts a structured invoice (JSON/CSV), a PDF invoice (text is
 * extracted from the content streams), or text from an OCR provider, matches
 * the lines to the item master, posts the purchase, computes landing cost and
 * margin for every line, can raise the selling price from a target margin, and
 * can seed opening stock - the "first Parcha" flow, in one step.
 *
 * `analyze` is a dry run (matches + landing cost preview, nothing written);
 * `apply` writes. Every run is recorded in tbl_ParchaImport.
 */

const numOr = (v, d = 0) => { const n = num(v); return v === '' || v == null ? d : n; };

// ------------------------------------------------------------------ invoice input

/** Normalise a loosely-shaped invoice object into { header, lines }. */
export function normalizeInvoice(raw = {}) {
  const header = {
    suppliername: raw.suppliername || raw.supplier || raw.party || '',
    suppliercode: raw.suppliercode || raw.partycode || null,
    docno: raw.docno || raw.invoiceno || raw.invoiceNo || raw.billno || '',
    docdate: raw.docdate || raw.invoicedate || raw.trndate || null,
    trndate: raw.trndate || raw.docdate || raw.invoicedate || null,
    tppassno: raw.tppassno || raw.tppass || '',
    shopcode: raw.shopcode || null,
    tottaxOth: numOr(raw.tottaxOth, 0),
    narration: raw.narration || null,
  };
  const rawLines = raw.lines || raw.items || raw.details || [];
  const lines = rawLines.map((l) => ({
    itemcode: l.itemcode || l.code || null,
    itemname: l.itemname || l.name || l.description || '',
    ml: l.ml != null ? Number(l.ml) : null,
    batchno: l.batchno || l.batch || '0',
    itemquantity: numOr(l.itemquantity ?? l.qty ?? l.quantity, 0),
    itemfreeqnty: numOr(l.itemfreeqnty ?? l.free ?? l.freeqty, 0),
    itemrate: numOr(l.itemrate ?? l.rate, 0),
    itemboxrate: numOr(l.itemboxrate, 0),
    itemmrp: numOr(l.itemmrp ?? l.mrp, 0),
    itemdiscount: numOr(l.itemdiscount ?? l.discount, 0),
  }));
  return { header, lines };
}

export function parseInvoiceJson(content) {
  return normalizeInvoice(typeof content === 'string' ? JSON.parse(content) : content);
}

/** CSV/TSV invoice: header keys on the first data row, one line per row. */
export function parseInvoiceCsv(content) {
  const rows = parseDelimited(String(content), sniffDelimiter(content));
  const objs = rowsToObjects(rows);
  const header = {};
  if (objs[0]) {
    header.suppliername = objs[0].supplier || objs[0].suppliername || '';
    header.docno = objs[0].docno || objs[0].invoiceno || '';
    header.trndate = objs[0].trndate || objs[0].docdate || null;
  }
  const lines = objs.filter((o) => (o.itemname || o.itemcode) && (o.qty || o.itemquantity || o.rate || o.itemrate));
  return normalizeInvoice({ ...header, lines });
}

/**
 * Best-effort text -> invoice. Handles the common invoice row shape
 * "Item name   qty   rate   amount" and labelled header fields.
 */
export function parseInvoiceText(text) {
  const lines = [];
  const src = String(text || '');
  for (const raw of src.split('\n')) {
    const line = raw.replace(/\s+/g, ' ').trim();
    if (!line) continue;
    // a row is a name (letters) followed by 2-4 trailing numbers
    const m = line.match(/^(.*?[A-Za-z\)\.])\s+((?:\d+(?:\.\d+)?\s+){1,4}\d+(?:\.\d+)?)$/);
    if (!m) continue;
    const name = m[1].replace(/[\s.]+$/, '').trim();
    const nums = m[2].split(/\s+/).map(Number);
    if (nums.length < 2) continue;
    // last two numbers are rate and amount (or qty and rate) - take qty first,
    // rate second, treating a trailing value that equals qty*rate as the amount.
    const [a, b, c, d] = nums;
    let qty = a; let rate = b; let amount = c ?? d;
    if (nums.length >= 3 && Math.abs(a * b - (nums[nums.length - 1])) < 0.05) amount = nums[nums.length - 1];
    if (amount != null && rate && Math.abs(qty * rate - amount) > 0.05 && nums.length >= 3) {
      // first number was probably the line/sr no - shift
      qty = b; rate = c ?? b; amount = nums[nums.length - 1];
    }
    if (!name || !(qty > 0)) continue;
    lines.push({ itemname: name, itemquantity: qty, itemrate: rate, itemmrp: 0, batchno: '0' });
  }
  const header = {};
  const pick = (re) => (src.match(re) || [])[1];
  header.suppliername = pick(/supplier\s*[:\-]\s*([^\n]+)/i) || '';
  header.docno = pick(/(?:invoice|bill|doc)\s*(?:no|#)\s*[:\-]?\s*([A-Za-z0-9\-\/]+)/i) || '';
  header.trndate = (pick(/(?:date)\s*[:\-]\s*(\d{2}[\/\-]\d{2}[\/\-]\d{4}|\d{4}[\/\-]\d{2}[\/\-]\d{2})/i) || '').trim() || null;
  return normalizeInvoice({ ...header, lines });
}

/**
 * Extract text from a PDF (uncompressed or Flate-compressed content streams).
 * This is a small, dependency-free reader for the text-showing operators
 * (Tj / TJ / ') that a simple invoice PDF uses - the same job the legacy app
 * delegated to SautinSoft. Scanned/image-only PDFs yield no text and must go
 * through the OCR path instead.
 */
export function extractPdfText(buffer) {
  const buf = Buffer.isBuffer(buffer) ? buffer : Buffer.from(buffer);
  const latin = buf.toString('latin1');
  const chunks = [];
  const streamRe = /stream\r?\n?([\s\S]*?)\r?\n?endstream/g;
  let m;
  while ((m = streamRe.exec(latin)) !== null) {
    let raw = Buffer.from(m[1], 'latin1');
    let text;
    try { text = inflateSync(raw).toString('latin1'); }
    catch {
      try { text = inflateRawSync(raw).toString('latin1'); }
      catch { text = raw.toString('latin1'); }
    }
    chunks.push(text);
  }
  const out = [];
  for (const c of chunks) {
    for (const tm of c.matchAll(/\((?:[^()\\]|\\.)*\)\s*Tj|\[(?:[^\]]*)\]\s*TJ/g)) {
      const s = tm[0];
      const parts = [...s.matchAll(/\(((?:[^()\\]|\\.)*)\)/g)].map((x) => x[1]);
      const line = parts.join('')
        .replace(/\\([()\\])/g, '$1')
        .replace(/\\n/g, '\n').replace(/\\r/g, '\n').replace(/\\t/g, ' ');
      if (line.trim()) out.push(line);
    }
  }
  return out.join('\n');
}

/** Load an invoice from any supported source. */
export function loadInvoice({ content, buffer, filename = '', source } = {}) {
  const ext = String(filename).toLowerCase().split('.').pop();
  if (buffer) {
    const buf = Buffer.isBuffer(buffer) ? buffer : Buffer.from(buffer);
    const isPdf = ext === 'pdf' || buf.subarray(0, 5).toString('latin1') === '%PDF-';
    if (isPdf) return { source: 'pdf', ...parseInvoiceText(extractPdfText(buf)) };
    const text = buf.toString('utf8');
    if (ext === 'json') return { source: 'json', ...parseInvoiceJson(text) };
    return { source: 'csv', ...parseInvoiceCsv(text) };
  }
  const text = String(content ?? '');
  const trimmed = text.trim();
  if (source === 'text') return { source: 'text', ...parseInvoiceText(text) };
  if (trimmed.startsWith('{') || trimmed.startsWith('[')) return { source: 'json', ...parseInvoiceJson(trimmed) };
  return { source: 'csv', ...parseInvoiceCsv(text) };
}

// ---------------------------------------------------------------- item matching

/** Match one invoice line to an item master row (code / barcode / name+ml). */
export function matchLine(db, company, line) {
  if (line.itemcode) {
    const byCode = get(db, 'SELECT itemcode, itemname, salesrate FROM itemmst WHERE companycode=? AND itemcode=?', [company, line.itemcode]);
    if (byCode) return { ...byCode, how: 'code' };
  }
  const name = String(line.itemname || '').trim();
  if (name) {
    const byBarcode = get(db, 'SELECT itemcode, itemname, salesrate FROM itemmst WHERE companycode=? AND barcode=?', [company, name]);
    if (byBarcode) return { ...byBarcode, how: 'barcode' };
    if (line.ml) {
      const byNameMl = get(db, 'SELECT itemcode, itemname, salesrate FROM itemmst WHERE companycode=? AND itemname=? AND ml=?', [company, name, line.ml]);
      if (byNameMl) return { ...byNameMl, how: 'name+ml' };
    }
    const byName = get(db, 'SELECT itemcode, itemname, salesrate FROM itemmst WHERE companycode=? AND itemname LIKE ?', [company, `%${name}%`]);
    if (byName) return { ...byName, how: 'name' };
    const fuzzy = matchByTokens(db, company, name, line.ml);
    if (fuzzy) return fuzzy;
  }
  return null;
}

const STOPWORDS = new Set(['the', 'and', 'of', 'ml', 'ltr', 'l', 'bottle', 'bottles', 'no', 'no1']);

/** Significant lowercase word tokens, ML figures stripped out. */
function tokens(text) {
  return String(text || '')
    .toLowerCase()
    .replace(/\d+\s*ml\b/g, ' ')
    .replace(/[^a-z0-9]+/g, ' ')
    .split(' ')
    .filter((t) => t && t.length > 1 && !STOPWORDS.has(t));
}

/**
 * Last-resort matcher for hand-typed parchas whose spelling differs from the
 * item master ("Royal Stag Deluxe Whisky" vs "Royal Stag"): score every item by
 * shared significant words (+ML agreement) and take a clear winner.
 */
function matchByTokens(db, company, name, ml) {
  const want = tokens(name);
  if (!want.length) return null;
  const rows = all(db, 'SELECT itemcode, itemname, salesrate, ml FROM itemmst WHERE companycode=?', [company]);
  let best = null;
  for (const r of rows) {
    const have = new Set(tokens(r.itemname));
    let score = want.filter((t) => have.has(t)).length;
    if (score === 0) continue;
    if (ml && Number(r.ml) === Number(ml)) score += 1;
    if (!best || score > best.score) best = { row: r, score };
  }
  // Require at least half the invoice's significant words to line up.
  if (best && best.score >= Math.ceil(want.length / 2)) {
    return { itemcode: best.row.itemcode, itemname: best.row.itemname, salesrate: best.row.salesrate, how: 'name~' };
  }
  return null;
}

// --------------------------------------------------------------------- planning

/**
 * Build a plan: match every line, estimate landing cost + margin. No writes.
 * @param {object} opts { company, year, invoice, createMissing, marginPct }
 */
export function buildPlan(db, { company, year, invoice, createMissing = false, marginPct = null } = {}) {
  const { header, lines } = invoice;
  const totalValue = lines.reduce((s, l) => s + num(l.itemrate) * num(l.itemquantity), 0) || 1;
  const entries = [];
  let matched = 0;
  let missing = 0;
  for (const l of lines) {
    const hit = matchLine(db, company, l);
    const share = money((num(l.itemrate) * num(l.itemquantity) / totalValue) * num(header.tottaxOth));
    const qty = num(l.itemquantity) + num(l.itemfreeqnty);
    const net = num(l.itemrate) * num(l.itemquantity) - num(l.itemdiscount) + share;
    const landingcost = qty > 0 ? money(net / qty) : money(num(l.itemrate));
    const salesrate = numOr(l.itemmrp, numOr(hit?.salesrate, 0)) || (marginPct != null ? money(landingcost * (1 + num(marginPct) / 100)) : 0);
    const marginamt = money(salesrate - landingcost);
    const marginpct = salesrate > 0 ? money((marginamt / salesrate) * 100) : 0;
    if (hit) matched++; else missing++;
    entries.push({
      line: l, itemcode: hit?.itemcode || null, itemname: hit?.itemname || l.itemname,
      how: hit?.how || null, landingcost, salesrate, marginamt, marginpct,
      willCreate: !hit && createMissing,
    });
  }
  const totals = {
    lines: lines.length, matched, missing,
    landingcost: money(entries.reduce((s, e) => s + e.landingcost, 0) / (entries.length || 1)),
    marginamt: money(entries.reduce((s, e) => s + e.marginamt, 0) / (entries.length || 1)),
    marginpct: money(entries.reduce((s, e) => s + e.marginpct, 0) / (entries.length || 1)),
  };
  return { header, entries, totals };
}

const nn = (v) => (v === undefined ? null : v);

function recordBatch(db, b) {
  run(db,
    `INSERT INTO tbl_ParchaImport
       (companycode, yearcode, source, filename, mode, status, suppliername, docno, trnno, lines, matched,
        created_items, landingcost, marginamt, marginpct, startedat, finishedat, message, detail)
     VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?)`,
    [nn(b.company), nn(b.year), nn(b.source), nn(b.filename), nn(b.mode), nn(b.status), nn(b.suppliername),
     nn(b.docno), nn(b.trnno), b.lines || 0, b.matched || 0, b.created_items || 0, b.landingcost || 0,
     b.marginamt || 0, b.marginpct || 0, nn(b.startedat), nn(b.finishedat), nn(b.message), nn(b.detail)]);
  return get(db, 'SELECT last_insert_rowid() AS id').id;
}

/** Dry run: match + landing-cost preview, write nothing. */
export function analyze(db, opts) {
  const plan = buildPlan(db, opts);
  const started = now();
  const batchId = recordBatch(db, {
    company: opts.company, year: opts.year, source: opts.invoice.source || opts.source, filename: opts.filename,
    mode: 'ANALYZE', status: 'ANALYZED', suppliername: plan.header.suppliername, docno: plan.header.docno,
    lines: plan.totals.lines, matched: plan.totals.matched,
    landingcost: plan.totals.landingcost, marginamt: plan.totals.marginamt, marginpct: plan.totals.marginpct,
    startedat: started, finishedat: now(),
    message: `Dry run: ${plan.totals.matched}/${plan.totals.lines} lines matched`,
    detail: JSON.stringify({ entries: plan.entries.map((e) => ({ itemname: e.itemname, itemcode: e.itemcode, how: e.how, landingcost: e.landingcost, salesrate: e.salesrate, marginpct: e.marginpct })) }),
  });
  return { batchId, mode: 'ANALYZE', ...plan };
}

/** Create an item master for an unmatched invoice line. */
function createItemFromLine(db, company, line) {
  const rows = all(db, "SELECT itemcode FROM itemmst WHERE itemcode LIKE 'I%'");
  let max = 0;
  for (const r of rows) { const n = parseInt(String(r.itemcode).slice(1), 10); if (Number.isFinite(n) && n > max) max = n; }
  const itemcode = 'I' + String(max + 1).padStart(5, '0');
  run(db,
    `INSERT INTO itemmst (companycode, itemcode, itemname, packing, ml, purchase_discount_amount,
        purchase_salestaxonmrp_amount, purchase_taxscheme_amount, salesrate, purchaseRate, min_qnty)
     VALUES (?,?,?,?,?,?,?,?,?,?,?)`,
    [company, itemcode, line.itemname || itemcode, 0, line.ml || 0, 0, 0, 0,
     money(line.itemmrp || 0), money(line.itemrate || 0), 0]);
  return itemcode;
}

/**
 * Apply: create missing items if asked, post the purchase, compute landing cost
 * + margin, optionally raise selling prices from a target margin, and
 * optionally seed opening stock.
 */
export function apply(db, opts) {
  const { company, year, createMissing = false, marginPct = null, setOpeningStock = false, shopcode = null } = opts;
  const plan = buildPlan(db, opts);
  const started = now();

  let created = 0;
  const resolvedLines = [];
  for (const e of plan.entries) {
    let itemcode = e.itemcode;
    if (!itemcode && createMissing) { itemcode = createItemFromLine(db, company, e.line); created++; }
    if (!itemcode) continue;
    resolvedLines.push({
      itemcode, batchno: e.line.batchno || '0', itemrate: e.line.itemrate,
      itembox: 0, itemloose: 0, itemquantity: e.line.itemquantity, itemfreeqnty: e.line.itemfreeqnty,
      itemmrp: e.line.itemmrp, itemdiscount: e.line.itemdiscount, itemboxrate: e.line.itemboxrate || null,
    });
  }
  if (!resolvedLines.length) throw Object.assign(new Error('No invoice lines could be matched or created'), { status: 400 });

  const created_purchase = createPurchase(db, {
    company, year,
    trndate: plan.header.trndate || now().slice(0, 10),
    suppliercode: plan.header.suppliercode, shopcode: plan.header.shopcode || shopcode,
    docno: plan.header.docno, docdate: plan.header.docdate || plan.header.trndate,
    tppassno: plan.header.tppassno, tottaxOth: plan.header.tottaxOth,
    narration: plan.header.narration || `Parcha import (${plan.header.suppliername || 'unknown supplier'})`,
    lines: resolvedLines,
  });

  let landed = computePurchase(db, { company, year, trnno: created_purchase.trnno });
  if (marginPct != null) {
    for (const l of landed.lines) applyMarginToItem(db, { company, itemcode: l.itemcode, landingcost: l.landingcost, marginPct });
    // refresh so the returned rows reflect the new selling price / margin
    landed = computePurchase(db, { company, year, trnno: created_purchase.trnno });
  }
  if (setOpeningStock) {
    for (const l of landed.lines) {
      run(db,
        `INSERT INTO openingstockmst (shopcode, companycode, itemcode, loose, box, qnty, yearcode, trndate)
         VALUES (?,?,?,?,?,?,?,?)
         ON CONFLICT(companycode, yearcode, shopcode, itemcode)
         DO UPDATE SET loose=excluded.loose, box=excluded.box, qnty=excluded.qnty, trndate=excluded.trndate`,
        [plan.header.shopcode || shopcode || 'MAIN', company, l.itemcode, 0, 0, l.qty + l.freeqty,
         year, plan.header.trndate || now().slice(0, 10)]);
    }
  }

  const avg = (arr, k) => money(arr.reduce((s, x) => s + num(x[k]), 0) / (arr.length || 1));
  const totals = {
    lines: plan.entries.length, matched: plan.totals.matched, created,
    landingcost: avg(landed.lines, 'landingcost'), marginamt: avg(landed.lines, 'marginamt'), marginpct: avg(landed.lines, 'marginpct'),
  };
  const batchId = recordBatch(db, {
    company, year, source: opts.invoice.source || opts.source, filename: opts.filename,
    mode: 'APPLY', status: 'IMPORTED', suppliername: plan.header.suppliername, docno: plan.header.docno,
    trnno: created_purchase.trnno, lines: totals.lines, matched: totals.matched, created_items: created,
    landingcost: totals.landingcost, marginamt: totals.marginamt, marginpct: totals.marginpct,
    startedat: started, finishedat: now(), message: `Imported as purchase ${created_purchase.trnno}`,
    detail: JSON.stringify({ trnno: created_purchase.trnno, lines: landed.lines }),
  });
  return { batchId, mode: 'APPLY', trnno: created_purchase.trnno, purchase: created_purchase, landedcost: landed.lines, totals };
}

export function history(db, company) {
  return all(db, 'SELECT * FROM tbl_ParchaImport WHERE companycode=? ORDER BY id DESC LIMIT 50', [company]);
}

export function batchDetail(db, company, id) {
  const batch = get(db, 'SELECT * FROM tbl_ParchaImport WHERE companycode=? AND id=?', [company, id]);
  if (!batch) return null;
  const detail = batch.detail ? JSON.parse(batch.detail) : null;
  return { batch, detail };
}

// -------------------------------------------------------------------------- OCR

export function ocrConfig(db, company) {
  return get(db, 'SELECT * FROM tbl_OcrConfig WHERE companycode=?', [company])
    || { companycode: company, provider: 'stub', endpoint: '', apikey: '', lang: 'eng', enabled: 0 };
}

export function saveOcrConfig(db, company, cfg = {}) {
  const cur = ocrConfig(db, company);
  run(db,
    `INSERT INTO tbl_OcrConfig (companycode, provider, endpoint, apikey, lang, enabled, updatedat)
     VALUES (?,?,?,?,?,?,?)
     ON CONFLICT(companycode) DO UPDATE SET provider=excluded.provider, endpoint=excluded.endpoint,
       apikey=excluded.apikey, lang=excluded.lang, enabled=excluded.enabled, updatedat=excluded.updatedat`,
    [company, cfg.provider ?? cur.provider, cfg.endpoint ?? cur.endpoint, cfg.apikey ?? cur.apikey,
     cfg.lang ?? cur.lang, cfg.enabled != null ? (cfg.enabled ? 1 : 0) : cur.enabled, now()]);
  return ocrConfig(db, company);
}

/** True if the `tesseract` CLI is on PATH. */
export function tesseractAvailable() {
  const r = spawnSync('tesseract', ['--version'], { encoding: 'utf8' });
  return !r.error && r.status === 0;
}

/**
 * Run OCR on an image buffer via the configured provider.
 *  - stub      : no real OCR; caller should pass text instead (throws).
 *  - tesseract : local `tesseract` CLI (needs the binary installed).
 *  - external  : POST the image (base64) to `endpoint`, expect { text }.
 */
export async function ocrExtract(db, company, { buffer, filename = 'scan.png' } = {}) {
  const cfg = ocrConfig(db, company);
  const provider = cfg.provider || 'stub';
  if (provider === 'tesseract') {
    if (!tesseractAvailable()) throw new Error('tesseract CLI not installed on the server');
    const dir = mkdtempSync(join(tmpdir(), 'vanijliquor-ocr-'));
    try {
      const img = join(dir, filename);
      writeFileSync(img, buffer);
      const r = spawnSync('tesseract', [img, 'stdout', '-l', cfg.lang || 'eng'], { encoding: 'utf8' });
      if (r.error) throw r.error;
      if (r.status !== 0) throw new Error(r.stderr || 'tesseract failed');
      return { provider, text: r.stdout };
    } finally { rmSync(dir, { recursive: true, force: true }); }
  }
  if (provider === 'external') {
    if (!cfg.endpoint) throw new Error('OCR endpoint not configured');
    const res = await fetch(cfg.endpoint, {
      method: 'POST',
      headers: { 'content-type': 'application/json', ...(cfg.apikey ? { authorization: `Bearer ${cfg.apikey}` } : {}) },
      body: JSON.stringify({ image: Buffer.from(buffer).toString('base64'), lang: cfg.lang || 'eng' }),
    });
    if (!res.ok) throw new Error(`OCR provider returned HTTP ${res.status}`);
    const data = await res.json();
    return { provider, text: data.text || '' };
  }
  throw new Error('OCR provider is "stub": paste the invoice text instead, or configure tesseract / an external provider');
}
