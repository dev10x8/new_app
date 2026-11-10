import { all, get, run, tx } from '../db/index.js';
import { config } from '../config.js';
import { addMonths, now, today, token, daysBetween } from './helpers.js';
import { machineHash, machineDetail, hashFromDetail } from './fingerprint.js';
import { sign, verify, publicKeyBase64, usingProductionKey } from './license.js';
import * as catalogue from './catalogue.js';
import * as onboarding from './onboarding.js';
import * as audit from './audit.js';

/**
 * Licence lifecycle: issue -> activate -> verify.
 *
 * A licence binds a business's plan + modules to ONE machine. It carries a
 * signed payload the desktop app can verify offline. Activation records the
 * first machine that redeemed it; redeeming the same licence on a different
 * machine hash is refused (anti-clone).
 */

const clean = (v, max = 200) => String(v == null ? '' : v).trim().slice(0, max);

/** Modules a business is entitled to: bundled by plan + explicitly selected. */
export function entitlementsFor(db, business) {
  const bundled = new Set(
    catalogue.plans(db).find((p) => p.code === business.plan_code)?.modules || [],
  );
  for (const m of onboarding.listModules(db, business.id)) {
    if (m.status !== 'off') bundled.add(m.module_code);
  }
  return [...bundled].sort();
}

function nextLicenseId(db) {
  let id;
  do { id = `VL-LIC-${token(6)}`; } while (get(db, 'SELECT 1 AS x FROM licenses WHERE license_id=?', [id]));
  return id;
}

/**
 * Issue a licence bound to a machine fingerprint.
 * @param {object} input { business_code|business_id, machine:{...}, months? }
 */
export function issue(db, input, actor = 'portal') {
  const business = input.business_id
    ? onboarding.getById(db, Number(input.business_id))
    : onboarding.getByCode(db, clean(input.business_code, 40));
  if (!business) throw Object.assign(new Error('Business not found'), { status: 404 });
  if (!input.machine || typeof input.machine !== 'object') {
    throw Object.assign(new Error('Machine fingerprint is required'), { status: 400 });
  }

  const components = machineDetail(input.machine);
  if (!components.macs.length && !components.cpu && !components.disk) {
    throw Object.assign(new Error('Machine fingerprint must include at least a MAC, CPU id or disk id'), { status: 400 });
  }
  const machine_hash = machineHash(input.machine);

  const period = catalogue.periods(db).find((p) => p.code === business.period_code) || { months: config.defaultTermMonths };
  const months = Number(input.months) || period.months || config.defaultTermMonths;
  const valid_from = today();
  const valid_until = addMonths(valid_from, months);
  const entitlements = entitlementsFor(db, business);
  const license_id = nextLicenseId(db);

  const payload = {
    v: 1,
    license_id,
    business_code: business.business_code,
    legalname: business.legalname,
    gstin: business.gstin,
    plan: business.plan_code,
    period: business.period_code,
    entitlements,
    machine_hash,
    valid_from,
    valid_until,
    issued_at: now(),
    issuer: config.issuer,
  };
  const signed = sign(payload);

  return tx(db, () => {
    run(db, `INSERT INTO licenses
      (license_id, business_id, plan_code, period_code, status, machine_hash, machine_detail,
       entitlements, valid_from, valid_until, issued_at, payload, signature, public_key)
      VALUES (?,?,?,?, 'issued', ?,?,?,?,?,?,?,?,?)`, [
      license_id, business.id, business.plan_code, business.period_code, machine_hash,
      JSON.stringify(components), JSON.stringify(entitlements), valid_from, valid_until,
      payload.issued_at, signed.body, signed.signature, signed.publicKey,
    ]);
    onboarding.update(db, business.id, { status: business.status === 'draft' ? 'onboarding' : business.status }, actor);
    audit.log(db, actor, 'license.issue', license_id, { business: business.business_code, machine_hash: machine_hash.slice(0, 16) });
    return getLicense(db, license_id);
  });
}

/** Redeem a licence on a machine — the anti-clone gate. */
export function activate(db, licenseId, machine, meta = {}, actor = 'desktop') {
  const lic = getLicense(db, licenseId);
  if (!lic) throw Object.assign(new Error('Licence not found'), { status: 404 });
  if (lic.status === 'revoked') throw Object.assign(new Error('Licence has been revoked'), { status: 403 });

  const hash = machineHash(machine || {});
  if (hash !== lic.machine_hash) {
    audit.log(db, actor, 'license.activate.denied', licenseId, { reason: 'machine-mismatch' });
    throw Object.assign(new Error('Licence is bound to a different machine'), { status: 403 });
  }

  const todayStr = today();
  if (daysBetween(todayStr, lic.valid_until) < 0) {
    throw Object.assign(new Error('Licence has expired'), { status: 403 });
  }

  return tx(db, () => {
    const first = !get(db, 'SELECT 1 AS x FROM activations WHERE license_id=?', [licenseId]);
    if (first) run(db, "UPDATE licenses SET status='active' WHERE license_id=?", [licenseId]);
    run(db, 'INSERT INTO activations (license_id, machine_hash, activated_at, ip, detail) VALUES (?,?,?,?,?)',
      [licenseId, hash, now(), clean(meta.ip, 60), clean(meta.detail, 200)]);
    run(db, "UPDATE businesses SET status='active', updatedat=? WHERE id=?", [now(), lic.business_id]);
    audit.log(db, actor, 'license.activate', licenseId, { first, machine_hash: hash.slice(0, 16) });
    return getLicense(db, licenseId);
  });
}

/**
 * Verify a licence offline-style: check the signature, the term and (when a
 * fingerprint is supplied) the machine binding. Returns a verdict object
 * rather than throwing, so both the portal UI and the desktop can show it.
 */
export function verifyLicense(db, licenseId, machine) {
  const lic = getLicense(db, licenseId);
  if (!lic) return { ok: false, reason: 'not-found' };
  const signatureOk = verify(lic.payload, lic.signature, lic.public_key || publicKeyBase64());
  const expired = daysBetween(today(), lic.valid_until) < 0;
  const result = {
    ok: signatureOk && !expired,
    license_id: lic.license_id,
    business_code: lic.business_code,
    plan: lic.plan_code,
    entitlements: lic.entitlements,
    valid_from: lic.valid_from,
    valid_until: lic.valid_until,
    days_remaining: Math.max(0, daysBetween(today(), lic.valid_until)),
    status: lic.status,
    signature_ok: signatureOk,
    expired,
    machine_ok: null,
  };
  if (machine) {
    result.machine_ok = machineHash(machine) === lic.machine_hash;
    if (!result.machine_ok) result.ok = false;
  }
  return result;
}

/** Re-sign an existing licence with the current key (e.g. after key rotation). */
export function resign(db, licenseId, actor = 'portal') {
  const lic = getLicense(db, licenseId);
  if (!lic) throw Object.assign(new Error('Licence not found'), { status: 404 });
  const signed = sign(lic.payload);
  run(db, 'UPDATE licenses SET payload=?, signature=?, public_key=? WHERE license_id=?',
    [signed.body, signed.signature, signed.publicKey, licenseId]);
  audit.log(db, actor, 'license.resign', licenseId, {});
  return getLicense(db, licenseId);
}

export function revoke(db, licenseId, actor = 'portal') {
  const lic = getLicense(db, licenseId);
  if (!lic) throw Object.assign(new Error('Licence not found'), { status: 404 });
  run(db, "UPDATE licenses SET status='revoked' WHERE license_id=?", [licenseId]);
  audit.log(db, actor, 'license.revoke', licenseId, {});
  return getLicense(db, licenseId);
}

/** Row -> API shape, parsing JSON columns. */
function shape(db, row) {
  const biz = get(db, 'SELECT business_code, legalname, plan_code FROM businesses WHERE id=?', [row.business_id]);
  return {
    license_id: row.license_id,
    business_id: row.business_id,
    business_code: biz ? biz.business_code : '',
    legalname: biz ? biz.legalname : '',
    plan_code: row.plan_code,
    period_code: row.period_code,
    status: row.status,
    machine_hash: row.machine_hash,
    machine_detail: safe(row.machine_detail, {}),
    entitlements: safe(row.entitlements, []),
    valid_from: row.valid_from,
    valid_until: row.valid_until,
    issued_at: row.issued_at,
    payload: safe(row.payload, {}),
    signature: row.signature,
    public_key: row.public_key,
    activations: all(db, 'SELECT machine_hash, activated_at, ip, detail FROM activations WHERE license_id=? ORDER BY id', [row.license_id]),
  };
}
const safe = (s, fb) => { try { return JSON.parse(s); } catch { return fb; } };

export function getLicense(db, licenseId) {
  const row = get(db, 'SELECT * FROM licenses WHERE license_id=?', [licenseId]);
  return row ? shape(db, row) : null;
}

export function list(db, { business_id, status, q, limit = 100 } = {}) {
  const where = [];
  const params = [];
  if (business_id) { where.push('l.business_id=?'); params.push(Number(business_id)); }
  if (status) { where.push('l.status=?'); params.push(status); }
  if (q) { where.push('(l.license_id LIKE ? OR b.legalname LIKE ? OR b.business_code LIKE ?)'); params.push(`%${q}%`, `%${q}%`, `%${q}%`); }
  const clause = where.length ? `WHERE ${where.join(' AND ')}` : '';
  const rows = all(db, `SELECT l.* FROM licenses l JOIN businesses b ON b.id=l.business_id ${clause} ORDER BY l.id DESC LIMIT ?`,
    [...params, Number(limit) || 100]);
  return rows.map((r) => shape(db, r));
}

/** Dashboard counts for the licence panel. */
export function stats(db) {
  const one = (sql) => get(db, sql).n;
  return {
    issued: one("SELECT COUNT(*) AS n FROM licenses WHERE status='issued'"),
    active: one("SELECT COUNT(*) AS n FROM licenses WHERE status='active'"),
    revoked: one("SELECT COUNT(*) AS n FROM licenses WHERE status='revoked'"),
    expiringSoon: one(`SELECT COUNT(*) AS n FROM licenses WHERE status IN ('issued','active') AND valid_until <= date('now','+30 day')`),
    activations: one('SELECT COUNT(*) AS n FROM activations'),
    usingProductionKey: usingProductionKey(),
    publicKey: publicKeyBase64(),
  };
}

export { hashFromDetail };
