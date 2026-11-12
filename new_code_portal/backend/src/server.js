import http from 'node:http';
import { URL } from 'node:url';
import { openDatabase } from './db/index.js';
import { config } from './config.js';
import * as catalogue from './services/catalogue.js';
import * as onboarding from './services/onboarding.js';
import * as licensing from './services/licensing.js';
import * as audit from './services/audit.js';
import * as license from './services/license.js';
import * as downloads from './services/downloads.js';

const db = openDatabase(config.dbFile);

function json(res, code, body) {
  const text = JSON.stringify(body);
  res.writeHead(code, {
    'content-type': 'application/json',
    'content-length': Buffer.byteLength(text),
    'access-control-allow-origin': '*',
  });
  res.end(text);
}

function readBody(req) {
  return new Promise((resolve, reject) => {
    const chunks = [];
    req.on('data', (c) => { chunks.push(c); });
    req.on('end', () => {
      const buf = Buffer.concat(chunks);
      if (!buf.length) return resolve({});
      try { resolve(JSON.parse(buf.toString('utf8'))); } catch (e) { reject(Object.assign(e, { status: 400 })); }
    });
    req.on('error', reject);
  });
}

const routes = [];
function route(method, path, handler) { routes.push({ method, path, handler }); }

// ---- meta ----
route('GET', '/api/health', () => ({ ok: true, app: 'VanijLiquor Customer Portal', vendor: config.vendor }));
route('GET', '/api/meta', () => ({
  app: 'VanijLiquor Customer Portal',
  vendor: config.vendor,
  issuer: config.issuer,
  onboardingSteps: onboarding.ONBOARDING_STEPS,
  plans: catalogue.plans(db).map((p) => ({ code: p.code, name: p.name, monthly: p.monthly })),
}));

// ---- catalogue ----
route('GET', '/api/catalogue', () => catalogue.catalogue(db));
route('GET', '/api/plans', () => catalogue.plans(db));
route('GET', '/api/periods', () => catalogue.periods(db));
route('GET', '/api/modules', () => catalogue.modules(db));
route('GET', '/api/addons', () => catalogue.addons(db));
route('GET', '/api/comparison', () => catalogue.comparison(db));
// Price a plan + period + optional add-on modules (no persistence).
route('POST', '/api/quote', (b) => quote(b));

// ---- onboarding / businesses ----
route('POST', '/api/businesses', (b) => onboarding.create(db, b, 'portal'));
route('GET', '/api/businesses', (b, p, q) => onboarding.list(db, {
  status: q.get('status') || undefined, q: q.get('q') || undefined,
  limit: q.get('limit') || undefined, offset: q.get('offset') || undefined,
}));
route('GET', '/api/businesses/:id', (b, p) => {
  const biz = /^\d+$/.test(p.id) ? onboarding.getById(db, Number(p.id)) : onboarding.getByCode(db, p.id);
  if (!biz) throw Object.assign(new Error('Business not found'), { status: 404 });
  return { ...biz, entitlements: licensing.entitlementsFor(db, biz) };
});
route('PUT', '/api/businesses/:id', (b, p) => onboarding.update(db, Number(p.id), b, 'portal'));
route('POST', '/api/businesses/:id/modules', (b, p) => onboarding.setModules(db, Number(p.id), b.modules || [], 'portal'));
route('POST', '/api/businesses/:id/modules/:code', (b, p) => onboarding.addModule(db, Number(p.id), p.code, 'portal'));
route('DELETE', '/api/businesses/:id/modules/:code', (b, p) => onboarding.removeModule(db, Number(p.id), p.code, 'portal'));

// ---- licensing ----
route('POST', '/api/licenses', (b) => licensing.issue(db, b, 'portal'));
route('GET', '/api/licenses', (b, p, q) => licensing.list(db, {
  business_id: q.get('business_id') || undefined, status: q.get('status') || undefined, q: q.get('q') || undefined,
}));
route('GET', '/api/licenses/:id', (b, p) => {
  const lic = licensing.getLicense(db, p.id);
  if (!lic) throw Object.assign(new Error('Licence not found'), { status: 404 });
  return lic;
});
// Download the standalone, offline-verifiable licence file (the .vlic the app imports).
route('GET', '/api/licenses/:id/file', (b, p, q) => licensing.getLicense(db, p.id) && licenseFile(db, p.id));
route('POST', '/api/licenses/:id/activate', (b, p) => licensing.activate(db, p.id, b.machine, { ip: b.ip, detail: b.detail }, 'desktop'));
route('POST', '/api/licenses/:id/verify', (b, p) => licensing.verifyLicense(db, p.id, b.machine, { daysSinceCheckin: b.days_since_checkin || 0 }));
route('POST', '/api/licenses/:id/surrender', (b, p) => licensing.surrender(db, p.id, 'portal', b.note || ''));
route('POST', '/api/licenses/:id/revoke', (b, p) => licensing.revoke(db, p.id, 'portal'));
route('POST', '/api/licenses/:id/resign', (b, p) => licensing.resign(db, p.id, 'portal'));
route('GET', '/api/licenses/:id/activations', (b, p) => licensing.getLicense(db, p.id).activations);
// The offline behaviour contract the desktop enforces: what state a licence is
// in now, and how to behave after N days without a check-in.
route('GET', '/api/licenses/:id/status', (b, p, q) => {
  const lic = licensing.getLicense(db, p.id);
  if (!lic) throw Object.assign(new Error('Licence not found'), { status: 404 });
  return {
    license_id: lic.license_id,
    state: lic.state, severity: lic.severity,
    days_remaining: lic.days_remaining, grace_days: lic.grace_days, grace_remaining: lic.grace_remaining,
    offline_limit_days: lic.offline_limit_days,
    offline: licensing.offlineState(lic, q.get('days_since_checkin') || 0),
    seats: lic.seat_limit, seats_used: licensing.getLicense(db, p.id).activations.filter((a) => !a.released_at).length,
    entitlements: lic.entitlements,
  };
});
route('GET', '/api/license-key', () => ({ public_key: license.publicKeyBase64(), production: license.usingProductionKey() }));

// ---- distribution: installer + licence-server links for the download page ----
route('GET', '/api/downloads', (b, p, q) => downloads.distribution(q.get('origin') || ''));

// ---- dashboard + audit ----
route('GET', '/api/dashboard', () => ({
  businesses: onboarding.stats(db),
  licenses: licensing.stats(db),
  recent: audit.recent(db, 12),
}));
route('GET', '/api/audit', (b, p, q) => audit.recent(db, q.get('limit') || 100));

function quote(input = {}) {
  const plan = catalogue.plans(db).find((p) => p.code === input.plan_code);
  if (!plan) throw Object.assign(new Error('Unknown plan'), { status: 400 });
  const period = catalogue.periods(db).find((p) => p.code === input.period_code) || catalogue.periods(db)[0];
  const base = catalogue.planPrice(db, plan.code, period.code);
  const included = new Set(plan.modules);
  const addonCodes = [...new Set((input.modules || []).map(String))];
  const extra = catalogue.modules(db)
    .filter((m) => addonCodes.includes(m.code) && !included.has(m.code) && m.addon_price > 0)
    .map((m) => ({ code: m.code, name: m.name, price: m.addon_price }));
  const addonsTotal = extra.reduce((s, m) => s + m.price, 0);
  const subtotal = base + addonsTotal;
  const gst = Math.round(subtotal * 0.18);
  return {
    plan_code: plan.code, plan_name: plan.name, period_code: period.code, period_label: period.label,
    base, addons: extra, addons_total: addonsTotal, subtotal, gst, total: subtotal + gst,
    currency: 'INR',
  };
}

/** Assemble the downloadable licence file from a stored licence row. */
function licenseFile(db, licenseId) {
  const lic = licensing.getLicense(db, licenseId);
  return {
    format: 'vanijliquor-license',
    version: 2,
    license_id: lic.license_id,
    business: { code: lic.business_code, legalname: lic.legalname, plan: lic.plan_code, period: lic.period_code },
    machine_hash: lic.machine_hash,
    machine_detail: lic.machine_detail,
    entitlements: lic.entitlements,
    seats: lic.seat_limit,
    valid_from: lic.valid_from,
    valid_until: lic.valid_until,
    // Offline contract: the desktop runs in limited "educational" mode once it
    // has gone this many days without a check-in, and keeps working for
    // `grace_days` after expiry before refusing.
    offline_limit_days: lic.offline_limit_days,
    grace_days: lic.grace_days,
    payload: lic.payload,
    signature: lic.signature,
    public_key: lic.public_key,
    verify: 'Ed25519 over canonical JSON of payload; verify with the PINNED vendor key. SHA-256 machine fingerprint must match.',
  };
}

const server = http.createServer(async (req, res) => {
  const url = new URL(req.url, `http://${req.headers.host}`);
  const path = url.pathname;
  for (const r of routes) {
    const params = matchPath(r.path, path);
    if (r.method === req.method && params) {
      try {
        const body = ['POST', 'PUT', 'PATCH', 'DELETE'].includes(req.method) ? await readBody(req) : {};
        const out = await r.handler(body, params, url.searchParams);
        return json(res, 200, { ok: true, data: out });
      } catch (err) {
        return json(res, err.status || 400, { ok: false, error: err.message });
      }
    }
  }
  json(res, 404, { ok: false, error: `No route for ${req.method} ${path}` });
});

function matchPath(pattern, actual) {
  const pp = pattern.split('/');
  const ap = actual.split('/');
  if (pp.length !== ap.length) return null;
  const params = {};
  for (let i = 0; i < pp.length; i++) {
    if (pp[i].startsWith(':')) params[pp[i].slice(1)] = decodeURIComponent(ap[i]);
    else if (pp[i] !== ap[i]) return null;
  }
  return params;
}

server.listen(config.port, config.host, () => {
  console.log(`VanijLiquor Customer Portal backend listening on http://${config.host}:${config.port}`);
  console.log(`Database: ${config.dbFile}`);
  if (!license.usingProductionKey()) {
    console.log('WARNING: using the development licence signing key. Set PORTAL_LICENSE_KEY for production.');
  }
});

export { server, db, routes };
