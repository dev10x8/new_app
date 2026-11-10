import { all, get, run, tx } from '../db/index.js';
import { now } from './helpers.js';
import * as audit from './audit.js';

const STEPS = ['business', 'plan', 'modules', 'review', 'license'];
export const ONBOARDING_STEPS = STEPS;

const clean = (v, max = 200) => String(v == null ? '' : v).trim().slice(0, max);

/** Allocate the next business code, e.g. VL-2026-0001. */
function nextBusinessCode(db) {
  const year = new Date().getFullYear();
  const like = `VL-${year}-%`;
  const row = get(db, "SELECT business_code FROM businesses WHERE business_code LIKE ? ORDER BY business_code DESC LIMIT 1", [like]);
  const n = row ? Number(row.business_code.slice(-4)) + 1 : 1;
  return `VL-${year}-${String(n).padStart(4, '0')}`;
}

/** Create a draft business from an onboarding form submission. */
export function create(db, input, actor = 'portal') {
  const legalname = clean(input.legalname, 200);
  const email = clean(input.email, 200);
  const phone = clean(input.phone, 40);
  if (!legalname) throw Object.assign(new Error('Legal name is required'), { status: 400 });
  if (!email && !phone) throw Object.assign(new Error('An email or phone is required'), { status: 400 });

  return tx(db, () => {
    const business_code = nextBusinessCode(db);
    const info = run(db, `INSERT INTO businesses
      (business_code, legalname, tradename, ownername, email, phone, gstin, panno,
       address1, city, district, state, statecode, pincode, businesstype, outlets,
       plan_code, period_code, status, onboarding_step)
      VALUES (?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?,?, 'draft', 1)`, [
      business_code, legalname, clean(input.tradename, 200), clean(input.ownername, 200),
      email, phone, clean(input.gstin, 20).toUpperCase(), clean(input.panno, 20).toUpperCase(),
      clean(input.address1, 240), clean(input.city, 80), clean(input.district, 80),
      clean(input.state, 80), clean(input.statecode, 4), clean(input.pincode, 10),
      clean(input.businesstype, 40), Math.max(1, Number(input.outlets) || 1),
      clean(input.plan_code, 20) || 'basic', clean(input.period_code, 20) || 'monthly',
    ]);
    const id = Number(info.lastInsertRowid);
    audit.log(db, actor, 'business.create', business_code, { legalname });
    return getById(db, id);
  });
}

/** Update editable business fields. */
export function update(db, id, input, actor = 'portal') {
  const existing = getById(db, id);
  if (!existing) throw Object.assign(new Error('Business not found'), { status: 404 });
  const fields = ['legalname', 'tradename', 'ownername', 'email', 'phone', 'gstin', 'panno',
    'address1', 'city', 'district', 'state', 'statecode', 'pincode', 'businesstype'];
  const sets = [];
  const params = [];
  for (const f of fields) {
    if (input[f] !== undefined) { sets.push(`${f}=?`); params.push(clean(input[f], 240)); }
  }
  if (input.outlets !== undefined) { sets.push('outlets=?'); params.push(Math.max(1, Number(input.outlets) || 1)); }
  if (input.plan_code !== undefined) { sets.push('plan_code=?'); params.push(clean(input.plan_code, 20)); }
  if (input.period_code !== undefined) { sets.push('period_code=?'); params.push(clean(input.period_code, 20)); }
  if (input.status !== undefined) { sets.push('status=?'); params.push(clean(input.status, 20)); }
  if (input.onboarding_step !== undefined) { sets.push('onboarding_step=?'); params.push(Math.min(STEPS.length, Math.max(1, Number(input.onboarding_step) || 1))); }
  sets.push('updatedat=?'); params.push(now());
  params.push(id);
  run(db, `UPDATE businesses SET ${sets.join(', ')} WHERE id=?`, params);
  audit.log(db, actor, 'business.update', existing.business_code, input);
  return getById(db, id);
}

/** Replace the module selection for a business. */
export function setModules(db, id, moduleCodes, actor = 'portal') {
  const existing = getById(db, id);
  if (!existing) throw Object.assign(new Error('Business not found'), { status: 404 });
  const codes = [...new Set((moduleCodes || []).map((c) => clean(c, 40)).filter(Boolean))];
  return tx(db, () => {
    run(db, 'DELETE FROM business_modules WHERE business_id=?', [id]);
    for (const code of codes) {
      run(db, 'INSERT OR IGNORE INTO business_modules (business_id, module_code, status) VALUES (?,?,?)', [id, code, 'active']);
    }
    run(db, 'UPDATE businesses SET updatedat=? WHERE id=?', [now(), id]);
    audit.log(db, actor, 'business.set-modules', existing.business_code, { count: codes.length });
    return listModules(db, id);
  });
}

/** Add a single module. */
export function addModule(db, id, code, actor = 'portal') {
  const existing = getById(db, id);
  if (!existing) throw Object.assign(new Error('Business not found'), { status: 404 });
  run(db, 'INSERT OR IGNORE INTO business_modules (business_id, module_code, status) VALUES (?,?,?)', [id, clean(code, 40), 'active']);
  audit.log(db, actor, 'business.add-module', existing.business_code, { module: code });
  return listModules(db, id);
}

/** Remove a single module. */
export function removeModule(db, id, code, actor = 'portal') {
  const existing = getById(db, id);
  if (!existing) throw Object.assign(new Error('Business not found'), { status: 404 });
  run(db, 'DELETE FROM business_modules WHERE business_id=? AND module_code=?', [id, clean(code, 40)]);
  audit.log(db, actor, 'business.remove-module', existing.business_code, { module: code });
  return listModules(db, id);
}

export function listModules(db, id) {
  return all(db, 'SELECT module_code, status FROM business_modules WHERE business_id=? ORDER BY module_code', [id]);
}

/** Full business record with its module selection. */
export function getById(db, id) {
  const b = get(db, 'SELECT * FROM businesses WHERE id=?', [id]);
  if (!b) return null;
  return { ...b, modules: listModules(db, id) };
}

export function getByCode(db, code) {
  const b = get(db, 'SELECT * FROM businesses WHERE business_code=?', [code]);
  if (!b) return null;
  return { ...b, modules: listModules(db, b.id) };
}

/** Paginated / filtered list. */
export function list(db, { status, q, limit = 100, offset = 0 } = {}) {
  const where = [];
  const params = [];
  if (status) { where.push('status=?'); params.push(status); }
  if (q) { where.push('(legalname LIKE ? OR tradename LIKE ? OR email LIKE ? OR business_code LIKE ?)'); params.push(`%${q}%`, `%${q}%`, `%${q}%`, `%${q}%`); }
  const clause = where.length ? `WHERE ${where.join(' AND ')}` : '';
  const rows = all(db, `SELECT * FROM businesses ${clause} ORDER BY id DESC LIMIT ? OFFSET ?`, [...params, Number(limit) || 100, Number(offset) || 0]);
  const total = get(db, `SELECT COUNT(*) AS n FROM businesses ${clause}`, params).n;
  return { total, rows: rows.map((b) => ({ ...b, modules: listModules(db, b.id) })) };
}

/** Counts for the dashboard. */
export function stats(db) {
  const one = (sql, ...p) => get(db, sql, p).n;
  const total = one('SELECT COUNT(*) AS n FROM businesses');
  const byStatus = {};
  for (const r of all(db, 'SELECT status, COUNT(*) AS n FROM businesses GROUP BY status')) byStatus[r.status] = r.n;
  const licenses = one('SELECT COUNT(*) AS n FROM licenses');
  const activeLicenses = one("SELECT COUNT(*) AS n FROM licenses WHERE status IN ('issued','active')");
  const activeBusinesses = byStatus.active || 0;
  // Recurring monthly value: active businesses at their plan's monthly rate.
  const mrr = get(db, `SELECT COALESCE(SUM(p.monthly),0) AS n FROM businesses b
    JOIN plans p ON p.code=b.plan_code WHERE b.status IN ('active','onboarding')`).n;
  const byPlan = all(db, `SELECT plan_code, COUNT(*) AS n FROM businesses GROUP BY plan_code`);
  return { total, byStatus, activeBusinesses, licenses, activeLicenses, mrr, byPlan };
}
