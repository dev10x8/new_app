import { all, get, run, tx } from '../db/index.js';
import { config } from '../config.js';
import { addMonths, now, today, token, daysBetween } from './helpers.js';
import { machineHash, machineDetail, hashFromDetail } from './fingerprint.js';
import { sign, verify, publicKeyBase64, usingProductionKey } from './license.js';
import * as catalogue from './catalogue.js';
import * as onboarding from './onboarding.js';
import * as audit from './audit.js';

/**
 * Licence lifecycle: issue -> activate -> verify -> (surrender) -> reactivate.
 *
 * A licence binds a business's plan + modules to a machine. It carries a signed
 * payload the desktop app can verify offline. Activation records the machine
 * that redeemed it; redeeming on a different machine is refused (anti-clone)
 * unless the licence has been explicitly surrendered first.
 *
 * Two "soft enforcement" ideas are baked into the signed payload so the desktop
 * can honour them with no network:
 *   * grace_days         - after valid_until the licence still verifies, and
 *                          reports "grace", for this many days.
 *   * offline_limit_days - days the client may run without a check-in before it
 *                          drops to limited "educational" mode.
 * `seat_limit` is how many distinct machines one licence may bind.
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
  const grace_days = Number.isFinite(Number(input.grace_days)) && input.grace_days != null
    ? Number(input.grace_days) : config.graceDays;
  const offline_limit_days = Number.isFinite(Number(input.offline_limit_days)) && input.offline_limit_days != null
    ? Number(input.offline_limit_days) : config.offlineLimitDays;
  const seat_limit = Number(input.seat_limit) > 0 ? Number(input.seat_limit) : config.seatLimit;

  const payload = {
    v: 2,
    license_id,
    business_code: business.business_code,
    legalname: business.legalname,
    gstin: business.gstin,
    plan: business.plan_code,
    period: business.period_code,
    entitlements,
    machine_hash,
    seats: seat_limit,
    grace_days,
    offline_limit_days,
    valid_from,
    valid_until,
    issued_at: now(),
    issuer: config.issuer,
  };
  const signed = sign(payload);

  return tx(db, () => {
    run(db, `INSERT INTO licenses
      (license_id, business_id, plan_code, period_code, status, machine_hash, machine_detail,
       entitlements, valid_from, valid_until, issued_at, payload, signature, public_key,
       grace_days, offline_limit_days, seat_limit, bound_machine_hash)
      VALUES (?,?,?,?, 'issued', ?,?,?,?,?,?,?,?,?,?,?,?,?)`, [
      license_id, business.id, business.plan_code, business.period_code, machine_hash,
      JSON.stringify(components), JSON.stringify(entitlements), valid_from, valid_until,
      payload.issued_at, signed.body, signed.signature, signed.publicKey,
      grace_days, offline_limit_days, seat_limit, machine_hash,
    ]);
    onboarding.update(db, business.id, { status: business.status === 'draft' ? 'onboarding' : business.status }, actor);
    audit.log(db, actor, 'license.issue', license_id, {
      business: business.business_code, machine_hash: machine_hash.slice(0, 16),
      grace_days, offline_limit_days, seat_limit,
    });
    return getLicense(db, license_id);
  });
}

/**
 * Machine(s) a licence is bound to: its primary binding (seeded from the issue
 * fingerprint, cleared on surrender) plus every additional seat activated and
 * not released.
 */
function boundMachines(db, lic) {
  const used = new Set(
    all(db, "SELECT DISTINCT machine_hash FROM activations WHERE license_id=? AND released_at=''", [lic.license_id])
      .map((r) => r.machine_hash),
  );
  if (lic.bound_machine_hash) used.add(lic.bound_machine_hash);
  return used;
}

/**
 * Redeem a licence on a machine — the anti-clone gate.
 *
 * Allowed when the machine is the one the licence was issued for, or one of the
 * seats already activated, or a free seat remains (seat_limit). After a
 * surrender the licence is unbound and the next activation re-binds it.
 */
export function activate(db, licenseId, machine, meta = {}, actor = 'desktop') {
  const lic = getLicense(db, licenseId);
  if (!lic) throw Object.assign(new Error('Licence not found'), { status: 404 });
  if (lic.status === 'revoked') throw Object.assign(new Error('Licence has been revoked'), { status: 403 });

  const state = statusOf(lic);
  if (state.state === 'expired') {
    audit.log(db, actor, 'license.activate.denied', licenseId, { reason: 'expired' });
    throw Object.assign(new Error('Licence has expired'), { status: 403 });
  }

  const hash = machineHash(machine || {});
  const bound = boundMachines(db, lic);
  const freeSeat = bound.size < lic.seat_limit;
  if (!bound.has(hash) && !freeSeat) {
    // Single-seat licences report the classic anti-clone reason; multi-seat
    // licences report that all seats are taken.
    const reason = lic.seat_limit > 1 ? 'seat-limit' : 'machine-mismatch';
    audit.log(db, actor, 'license.activate.denied', licenseId, { reason });
    throw Object.assign(new Error(
      reason === 'seat-limit'
        ? `Seat limit reached (${lic.seat_limit}); surrender a machine first`
        : 'Licence is bound to a different machine',
    ), { status: 403 });
  }

  return tx(db, () => {
    const first = !get(db, 'SELECT 1 AS x FROM activations WHERE license_id=? AND released_at=\'\'', [licenseId]);
    // Re-bind after a surrender: the first activation on an unbound licence.
    run(db, "UPDATE licenses SET bound_machine_hash=? WHERE license_id=? AND bound_machine_hash=''", [hash, licenseId]);
    run(db, 'INSERT INTO activations (license_id, machine_hash, activated_at, ip, detail) VALUES (?,?,?,?,?)',
      [licenseId, hash, now(), clean(meta.ip, 60), clean(meta.detail, 200)]);
    // Any live binding means the licence is in use.
    run(db, "UPDATE licenses SET status='active' WHERE license_id=? AND status!='revoked'", [licenseId]);
    run(db, "UPDATE businesses SET status='active', updatedat=? WHERE id=?", [now(), lic.business_id]);
    audit.log(db, actor, 'license.activate', licenseId, { first, machine_hash: hash.slice(0, 16), seats_used: boundMachines(db, getLicense(db, licenseId)).size });
    return getLicense(db, licenseId);
  });
}

/**
 * Release every machine binding so the licence can be moved to a new machine
 * (a hardware replacement). Tally's "surrender": explicit and audited, rather
 * than a silent re-bind. The licence keeps its term; the next activation
 * re-binds it.
 */
export function surrender(db, licenseId, actor = 'portal', note = '') {
  const lic = getLicense(db, licenseId);
  if (!lic) throw Object.assign(new Error('Licence not found'), { status: 404 });
  if (lic.status === 'revoked') throw Object.assign(new Error('Licence has been revoked'), { status: 403 });
  return tx(db, () => {
    run(db, "UPDATE activations SET released_at=? WHERE license_id=? AND released_at=''", [now(), licenseId]);
    run(db, "UPDATE licenses SET bound_machine_hash='', surrendered_at=?, status='issued' WHERE license_id=?", [now(), licenseId]);
    audit.log(db, actor, 'license.surrender', licenseId, { note: clean(note, 200) });
    return getLicense(db, licenseId);
  });
}

/**
 * The licence's state from the expiry + grace window. `severity` tells the UI
 * (and the desktop) whether the licence is fully usable (`ok`), usable with a
 * warning (`warn`), or should refuse to run (`hard`).
 */
export function statusOf(lic) {
  if (lic.status === 'revoked') return { state: 'revoked', severity: 'hard', days_remaining: 0, grace_remaining: 0 };
  const days = daysBetween(today(), lic.valid_until); // >0 = still in term
  const grace = Number(lic.grace_days) || 0;
  if (days >= 0) return { state: 'valid', severity: 'ok', days_remaining: days, grace_remaining: 0 };
  if (-days <= grace) return { state: 'grace', severity: 'warn', days_remaining: 0, grace_remaining: grace + days };
  return { state: 'expired', severity: 'hard', days_remaining: 0, grace_remaining: 0 };
}

/**
 * Offline tolerance: how the desktop should behave when it has not checked in
 * for `daysSinceCheckin` days. Past offline_limit_days it drops to limited
 * "educational" mode (Tally-style) instead of stopping.
 */
export function offlineState(lic, daysSinceCheckin = 0) {
  const limit = Number(lic.offline_limit_days) || 0;
  const days = Math.max(0, Number(daysSinceCheckin) || 0);
  const educational = limit > 0 && days > limit;
  return {
    state: educational ? 'educational' : 'online',
    severity: educational ? 'warn' : 'ok',
    days_since_checkin: days,
    offline_limit_days: limit,
  };
}

/**
 * Verify a licence offline-style: check the signature, the term and (when a
 * fingerprint is supplied) the machine binding. Returns a verdict object
 * rather than throwing, so both the portal UI and the desktop can show it.
 *
 * The signature is checked against the CURRENT signing key only. The public key
 * stored in the licence row is never trusted for the decision — otherwise a
 * self-signed licence carrying its own key would validate.
 */
export function verifyLicense(db, licenseId, machine, { daysSinceCheckin = 0 } = {}) {
  const lic = getLicense(db, licenseId);
  if (!lic) return { ok: false, reason: 'not-found' };
  const signatureOk = verify(lic.payload, lic.signature);
  const state = statusOf(lic);
  const offline = offlineState(lic, daysSinceCheckin);
  const result = {
    ok: signatureOk && state.severity !== 'hard',
    license_id: lic.license_id,
    business_code: lic.business_code,
    plan: lic.plan_code,
    entitlements: lic.entitlements,
    seats: lic.seat_limit,
    valid_from: lic.valid_from,
    valid_until: lic.valid_until,
    days_remaining: state.days_remaining,
    grace_days: lic.grace_days,
    grace_remaining: state.grace_remaining,
    offline_limit_days: lic.offline_limit_days,
    status: lic.status,
    state: state.state,
    severity: state.severity,
    offline_state: offline.state,
    signature_ok: signatureOk,
    expired: state.state === 'expired',
    machine_ok: null,
  };
  if (machine) {
    const hash = machineHash(machine);
    result.machine_ok = boundMachines(db, lic).has(hash);
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
  const lic = {
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
    grace_days: row.grace_days,
    offline_limit_days: row.offline_limit_days,
    seat_limit: row.seat_limit,
    bound_machine_hash: row.bound_machine_hash || '',
    surrendered_at: row.surrendered_at || '',
    valid_from: row.valid_from,
    valid_until: row.valid_until,
    issued_at: row.issued_at,
    payload: safe(row.payload, {}),
    signature: row.signature,
    public_key: row.public_key,
    activations: all(db, 'SELECT machine_hash, activated_at, released_at, ip, detail FROM activations WHERE license_id=? ORDER BY id', [row.license_id]),
  };
  const st = statusOf(lic);
  return { ...lic, state: st.state, severity: st.severity, days_remaining: st.days_remaining, grace_remaining: st.grace_remaining };
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
    // Within the grace window: past valid_until but not past valid_until + grace_days.
    inGrace: one(`SELECT COUNT(*) AS n FROM licenses WHERE status!='revoked'
      AND valid_until < date('now') AND date('now') <= date(valid_until, '+' || grace_days || ' day')`),
    activations: one('SELECT COUNT(*) AS n FROM activations'),
    seatsUsed: one('SELECT COUNT(DISTINCT license_id || machine_hash) AS n FROM activations WHERE released_at=\'\''),
    usingProductionKey: usingProductionKey(),
    publicKey: publicKeyBase64(),
  };
}

export { hashFromDetail };
