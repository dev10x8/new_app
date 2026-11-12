import test from 'node:test';
import assert from 'node:assert/strict';
import { generateKeyPairSync, sign as cryptoSign } from 'node:crypto';
import { freshDb, machineA, machineB } from './helpers.mjs';
import * as onboarding from '../src/services/onboarding.js';
import * as licensing from '../src/services/licensing.js';
import * as license from '../src/services/license.js';

test('creating a business allocates a sequential code and starts as draft', () => {
  const db = freshDb();
  const a = onboarding.create(db, { legalname: 'A Wines', email: 'a@a.example' });
  const b = onboarding.create(db, { legalname: 'B Wines', email: 'b@b.example' });
  assert.match(a.business_code, /^VL-\d{4}-0001$/);
  assert.match(b.business_code, /^VL-\d{4}-0002$/);
  assert.equal(a.status, 'draft');
  assert.equal(a.onboarding_step, 1);
});

test('a business needs a legal name and a contact', () => {
  const db = freshDb();
  assert.throws(() => onboarding.create(db, { email: 'x@y.example' }), /Legal name/);
  assert.throws(() => onboarding.create(db, { legalname: 'No Contact' }), /email or phone/);
});

test('module selection replaces and de-duplicates', () => {
  const db = freshDb();
  const b = onboarding.create(db, { legalname: 'M Wines', email: 'm@m.example' });
  onboarding.setModules(db, b.id, ['EXCISE', 'EXCISE', 'TALLY']);
  let mods = onboarding.listModules(db, b.id).map((m) => m.module_code);
  assert.deepEqual(mods, ['EXCISE', 'TALLY']);
  onboarding.removeModule(db, b.id, 'EXCISE');
  mods = onboarding.listModules(db, b.id).map((m) => m.module_code);
  assert.deepEqual(mods, ['TALLY']);
});

test('entitlements are the union of plan bundle and chosen modules', () => {
  const db = freshDb();
  const b = onboarding.create(db, { legalname: 'E Wines', email: 'e@e.example', plan_code: 'basic' });
  onboarding.setModules(db, b.id, ['PARCHA']);
  const full = onboarding.getById(db, b.id);
  const ent = licensing.entitlementsFor(db, full);
  assert.ok(ent.includes('EXCISE'));
  assert.ok(ent.includes('PARCHA'));
  assert.ok(!ent.includes('TALLY'));
});

test('list filters by status and searches by name', () => {
  const db = freshDb();
  onboarding.create(db, { legalname: 'Alpha Spirits', email: 'a@a.example' });
  const beta = onboarding.create(db, { legalname: 'Beta Beers', email: 'b@b.example' });
  onboarding.update(db, beta.id, { status: 'active' });
  assert.equal(onboarding.list(db, {}).total, 2);
  assert.equal(onboarding.list(db, { status: 'active' }).total, 1);
  assert.equal(onboarding.list(db, { q: 'Alpha' }).total, 1);
});

test('stats summarise counts and MRR', () => {
  const db = freshDb();
  const b = onboarding.create(db, { legalname: 'S Wines', email: 's@s.example', plan_code: 'premium' });
  onboarding.update(db, b.id, { status: 'active' });
  const s = onboarding.stats(db);
  assert.equal(s.total, 1);
  assert.equal(s.activeBusinesses, 1);
  assert.equal(s.mrr, 1499);
});

test('issuing binds the licence to the machine and lists entitlements', () => {
  const db = freshDb();
  const b = onboarding.create(db, { legalname: 'L Wines', email: 'l@l.example', plan_code: 'standard', period_code: 'oneYear' });
  const lic = licensing.issue(db, { business_id: b.id, machine: machineA });
  assert.match(lic.license_id, /^VL-LIC-[A-Z0-9]{6}$/);
  assert.equal(lic.machine_hash.length, 64);
  assert.ok(lic.entitlements.includes('CLOUD'));
  assert.equal(lic.status, 'issued');
  assert.equal(lic.activations.length, 0);
});

test('issuing refuses a business that does not exist or an empty fingerprint', () => {
  const db = freshDb();
  assert.throws(() => licensing.issue(db, { business_id: 999, machine: machineA }), /Business not found/);
  const b = onboarding.create(db, { legalname: 'F Wines', email: 'f@f.example' });
  assert.throws(() => licensing.issue(db, { business_id: b.id, machine: {} }), /at least a MAC/);
});

test('activation succeeds on the bound machine and is refused elsewhere', () => {
  const db = freshDb();
  const b = onboarding.create(db, { legalname: 'A Wines', email: 'a@a.example' });
  const lic = licensing.issue(db, { business_id: b.id, machine: machineA });

  const activated = licensing.activate(db, lic.license_id, machineA, { ip: '10.0.0.1' });
  assert.equal(activated.status, 'active');
  assert.equal(activated.activations.length, 1);

  assert.throws(() => licensing.activate(db, lic.license_id, machineB), /different machine/);
  assert.equal(licensing.getLicense(db, lic.license_id).activations.length, 1);
});

test('a licence MAC format change still activates (normalisation)', () => {
  const db = freshDb();
  const b = onboarding.create(db, { legalname: 'N Wines', email: 'n@n.example' });
  const lic = licensing.issue(db, { business_id: b.id, machine: machineA });
  const sameMac = { ...machineA, mac: '001a.2b3c.4d5e' };
  const activated = licensing.activate(db, lic.license_id, sameMac);
  assert.equal(activated.status, 'active');
});

test('verify reports signature, expiry and machine match', () => {
  const db = freshDb();
  const b = onboarding.create(db, { legalname: 'V Wines', email: 'v@v.example' });
  const lic = licensing.issue(db, { business_id: b.id, machine: machineA });
  const ok = licensing.verifyLicense(db, lic.license_id, machineA);
  assert.equal(ok.ok, true);
  assert.equal(ok.signature_ok, true);
  assert.equal(ok.machine_ok, true);
  assert.equal(ok.expired, false);
  const wrong = licensing.verifyLicense(db, lic.license_id, machineB);
  assert.equal(wrong.ok, false);
  assert.equal(wrong.machine_ok, false);
});

test('revoked licences cannot activate', () => {
  const db = freshDb();
  const b = onboarding.create(db, { legalname: 'R Wines', email: 'r@r.example' });
  const lic = licensing.issue(db, { business_id: b.id, machine: machineA });
  licensing.revoke(db, lic.license_id);
  assert.throws(() => licensing.activate(db, lic.license_id, machineA), /revoked/);
});

test('an expired licence is refused', () => {
  const db = freshDb();
  const b = onboarding.create(db, { legalname: 'X Wines', email: 'x@x.example' });
  const lic = licensing.issue(db, { business_id: b.id, machine: machineA, months: 1 });
  db.prepare("UPDATE licenses SET valid_until='2000-01-01' WHERE license_id=?").run(lic.license_id);
  const v = licensing.verifyLicense(db, lic.license_id, machineA);
  assert.equal(v.expired, true);
  assert.equal(v.ok, false);
  assert.throws(() => licensing.activate(db, lic.license_id, machineA), /expired/);
});

test('license stats count issued, active and revoked', () => {
  const db = freshDb();
  const b = onboarding.create(db, { legalname: 'C Wines', email: 'c@c.example' });
  const l1 = licensing.issue(db, { business_id: b.id, machine: machineA });
  licensing.activate(db, l1.license_id, machineA);
  licensing.issue(db, { business_id: b.id, machine: machineB });
  const s = licensing.stats(db);
  assert.equal(s.active, 1);
  assert.equal(s.issued, 1);
  assert.equal(s.activations, 1);
});

test('a licence inside its term reports the valid state', () => {
  const db = freshDb();
  const b = onboarding.create(db, { legalname: 'S Wines', email: 's@s.example' });
  const lic = licensing.issue(db, { business_id: b.id, machine: machineA, months: 12, grace_days: 15 });
  const v = licensing.verifyLicense(db, lic.license_id, machineA);
  assert.equal(v.state, 'valid');
  assert.equal(v.severity, 'ok');
  assert.equal(v.ok, true);
  assert.ok(v.days_remaining > 300);
});

test('the grace window keeps a just-expired licence usable, then it expires', () => {
  const db = freshDb();
  const b = onboarding.create(db, { legalname: 'G Wines', email: 'g@g.example' });
  const lic = licensing.issue(db, { business_id: b.id, machine: machineA, months: 1, grace_days: 15 });
  // Move the expiry 5 days into the past: inside the 15-day grace window.
  db.prepare("UPDATE licenses SET valid_until=date('now','-5 day') WHERE license_id=?").run(lic.license_id);
  const inGrace = licensing.verifyLicense(db, lic.license_id, machineA);
  assert.equal(inGrace.state, 'grace');
  assert.equal(inGrace.severity, 'warn');
  assert.equal(inGrace.ok, true);
  assert.equal(inGrace.grace_remaining, 10);
  // Grace still permits activation.
  const activated = licensing.activate(db, lic.license_id, machineA);
  assert.equal(activated.status, 'active');

  // 30 days past expiry is beyond the 15-day grace: hard-expired.
  db.prepare("UPDATE licenses SET valid_until=date('now','-30 day') WHERE license_id=?").run(lic.license_id);
  const expired = licensing.verifyLicense(db, lic.license_id, machineA);
  assert.equal(expired.state, 'expired');
  assert.equal(expired.severity, 'hard');
  assert.equal(expired.ok, false);
});

test('the grace period is baked into the signed payload', () => {
  const db = freshDb();
  const b = onboarding.create(db, { legalname: 'P Wines', email: 'p@p.example' });
  const lic = licensing.issue(db, { business_id: b.id, machine: machineA, grace_days: 21, offline_limit_days: 45, seat_limit: 3 });
  assert.equal(lic.payload.grace_days, 21);
  assert.equal(lic.payload.offline_limit_days, 45);
  assert.equal(lic.payload.seats, 3);
  assert.equal(lic.grace_days, 21);
  assert.equal(lic.seat_limit, 3);
});

test('offline tolerance switches to educational mode past the limit', () => {
  const db = freshDb();
  const b = onboarding.create(db, { legalname: 'O Wines', email: 'o@o.example' });
  const lic = licensing.issue(db, { business_id: b.id, machine: machineA, offline_limit_days: 30 });
  const fresh = licensing.verifyLicense(db, lic.license_id, machineA, { daysSinceCheckin: 5 });
  assert.equal(fresh.offline_state, 'online');
  const stale = licensing.verifyLicense(db, lic.license_id, machineA, { daysSinceCheckin: 45 });
  assert.equal(stale.offline_state, 'educational');
  assert.equal(stale.ok, true); // educational is limited, not refused
});

test('a multi-seat licence admits extra machines up to the seat limit', () => {
  const db = freshDb();
  const b = onboarding.create(db, { legalname: 'M Wines', email: 'm@m.example' });
  const machineC = { mac: 'AA:BB:CC:DD:EE:F0', cpu: 'BFEBFBFF000306A9', disk: 'WD-1234ABCD' };
  const lic = licensing.issue(db, { business_id: b.id, machine: machineA, seat_limit: 2 });
  licensing.activate(db, lic.license_id, machineA);
  const second = licensing.activate(db, lic.license_id, machineB);
  assert.equal(second.activations.filter((a) => !a.released_at).length, 2);
  assert.throws(() => licensing.activate(db, lic.license_id, machineC), /Seat limit/);
});

test('surrender releases every binding so the licence can move to a new machine', () => {
  const db = freshDb();
  const b = onboarding.create(db, { legalname: 'R Wines', email: 'r2@r.example' });
  const lic = licensing.issue(db, { business_id: b.id, machine: machineA });
  licensing.activate(db, lic.license_id, machineA);
  assert.throws(() => licensing.activate(db, lic.license_id, machineB), /different machine/);

  const released = licensing.surrender(db, lic.license_id, 'portal', 'hardware replacement');
  assert.equal(released.bound_machine_hash, '');
  assert.equal(released.status, 'issued');
  assert.ok(released.surrendered_at);
  assert.equal(released.activations.every((a) => a.released_at), true);

  const moved = licensing.activate(db, lic.license_id, machineB);
  assert.equal(moved.bound_machine_hash.length, 64);
  assert.equal(moved.activations.filter((a) => !a.released_at).length, 1);
});

test('surrender is recorded in the audit log', () => {
  const db = freshDb();
  const b = onboarding.create(db, { legalname: 'A Wines', email: 'a2@a.example' });
  const lic = licensing.issue(db, { business_id: b.id, machine: machineA });
  licensing.surrender(db, lic.license_id);
  const entry = db.prepare("SELECT * FROM audit_log WHERE action='license.surrender' ORDER BY id DESC").get();
  assert.ok(entry);
  assert.equal(entry.entity, lic.license_id);
});

test('a licence re-signed with an attacker key is rejected (pinned key only)', () => {
  const db = freshDb();
  const b = onboarding.create(db, { legalname: 'K Wines', email: 'k@k.example' });
  const lic = licensing.issue(db, { business_id: b.id, machine: machineA });

  // The attacker mints their own Ed25519 key, signs a doctored payload with it,
  // and stores their public key in the licence row — the old verifier trusted
  // that column, so this used to validate.
  const { publicKey: attackerPub, privateKey: attackerPriv } = generateKeyPairSync('ed25519');
  const doctored = { ...lic.payload, valid_until: '2099-12-31' };
  const body = license.canonicalJson(doctored);
  const forgedSig = cryptoSign(null, Buffer.from(body, 'utf8'), attackerPriv).toString('base64');
  db.prepare('UPDATE licenses SET payload=?, signature=?, public_key=? WHERE license_id=?')
    .run(body, forgedSig, attackerPub.export({ type: 'spki', format: 'der' }).toString('base64'), lic.license_id);

  const v = licensing.verifyLicense(db, lic.license_id, machineA);
  assert.equal(v.signature_ok, false);
  assert.equal(v.ok, false);
});
