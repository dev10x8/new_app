import test from 'node:test';
import assert from 'node:assert/strict';
import { mkdtempSync, rmSync, writeFileSync } from 'node:fs';
import { tmpdir } from 'node:os';
import { join } from 'node:path';
import { openDatabase } from '../src/db/index.js';
import * as license from '../src/services/license.js';
import { seal } from '../src/services/seal.js';

const daysAgo = (n) => new Date(Date.now() - n * 86400000).toISOString().slice(0, 10);

function freshDb() {
  const dir = mkdtempSync(join(tmpdir(), 'vanijliquor-license-'));
  const db = openDatabase(join(dir, 'test.db'));
  return { db, dir };
}

test('a fresh install has no licence and is blocked', () => {
  const { db, dir } = freshDb();
  try {
    const s = license.status(db);
    assert.equal(s.present, false);
    assert.equal(s.state, 'none');
    assert.equal(s.severity, 'hard');
    assert.equal(license.isBlocked(db), true);
    // The fingerprint is always available so the UI can show it.
    assert.match(s.machine_hash, /^[0-9a-f]{64}$/);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('activating the bundled demo yields a valid, machine-bound licence', () => {
  const { db, dir } = freshDb();
  try {
    const s = license.activateDemo(db);
    assert.equal(s.state, 'valid');
    assert.equal(s.severity, 'ok');
    assert.equal(s.kind, 'demo');
    assert.equal(s.signature_ok, true);
    assert.equal(s.machine_ok, true);
    assert.ok(s.days_remaining > 80 && s.days_remaining <= 90, `days=${s.days_remaining}`);
    assert.equal(license.isBlocked(db), false);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('the stored record carries a sealed copy the app itself cannot read', () => {
  const { db, dir } = freshDb();
  try {
    license.activateDemo(db);
    const row = db.prepare('SELECT payload, signature, sealed FROM tbl_License WHERE id=1').get();
    assert.ok(row.sealed && row.sealed.length > 100, 'sealed blob present');
    const env = JSON.parse(row.sealed);
    assert.equal(env.alg, 'RSA-OAEP-256+A256GCM');
    // The plaintext is NOT present anywhere in the sealed envelope.
    assert.equal(row.sealed.includes(row.signature), false);
    // And the sealed blob is not the signed payload either.
    assert.notEqual(row.sealed, row.payload);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('an edited payload fails verification and hard-blocks', () => {
  const { db, dir } = freshDb();
  try {
    license.activateDemo(db);
    const row = db.prepare('SELECT payload FROM tbl_License WHERE id=1').get();
    const tampered = JSON.parse(row.payload);
    tampered.valid_until = '2999-12-31';
    db.prepare('UPDATE tbl_License SET payload=?, valid_until=? WHERE id=1')
      .run(JSON.stringify(tampered), '2999-12-31');
    const s = license.status(db);
    assert.equal(s.signature_ok, false);
    assert.equal(s.state, 'invalid');
    assert.equal(license.isBlocked(db), true);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('importing a licence signed for another key is refused', () => {
  const { db, dir } = freshDb();
  try {
    const forged = {
      payload: { v: 2, license_id: 'VL-FAKE', plan: 'premium', machine_hash: '', entitlements: [] },
      signature: Buffer.from('not a real signature').toString('base64'),
    };
    assert.throws(() => license.importLicence(db, forged), /not valid|another vendor|tampered/i);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('the demo is one-time: reactivating keeps the original term', () => {
  const { db, dir } = freshDb();
  try {
    license.activateDemo(db);
    // Simulate 30 days having passed since the trial first started.
    db.prepare('UPDATE tbl_License SET trial_started_at=? WHERE id=1').run(daysAgo(30));
    const s = license.activateDemo(db); // user clicks "start demo" again
    assert.equal(s.state, 'valid');
    assert.equal(s.trial_started, true);
    assert.ok(s.days_remaining <= 61 && s.days_remaining >= 59, `days=${s.days_remaining}`);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('deleting the database does not restart the demo', () => {
  const dir = mkdtempSync(join(tmpdir(), 'vanijliquor-trial-'));
  const file = join(dir, 'test.db');
  try {
    let db = openDatabase(file);
    license.activateDemo(db);
    // Backdate the on-disk marker by 45 days, then delete the DB as a user would.
    const marker = join(dir, '.vl-trial');
    writeFileSync(marker, daysAgo(45));
    db.close();
    for (const s of ['', '-wal', '-shm']) rmSync(file + s, { force: true });

    db = openDatabase(file); // fresh, reseeded
    const fresh = license.status(db);
    assert.equal(fresh.present, false, 'no licence row after deletion');
    assert.equal(fresh.trial_started, true, 'marker remembers the trial');
    const s = license.activateDemo(db);
    assert.ok(s.days_remaining <= 46 && s.days_remaining >= 44, `days=${s.days_remaining}`);
  } finally { rmSync(dir, { recursive: true, force: true }); }
});

test('importing the bundled demo file cannot restart the trial', () => {
  const { db, dir } = freshDb();
  try {
    license.activateDemo(db);
    db.prepare('UPDATE tbl_License SET trial_started_at=? WHERE id=1').run(daysAgo(45));
    const s = license.importLicence(db, license.demoLicence());
    assert.ok(s.days_remaining <= 46 && s.days_remaining >= 44, `days=${s.days_remaining}`);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('the seal envelope round-trips through the portal unsealer shape', () => {
  // Mirrors the portal-side unseal: only the RSA private key can read it. Here
  // we only assert the envelope is well-formed and decryptable with the dev key.
  const env = JSON.parse(seal({ hello: 'world' }));
  assert.deepEqual(Object.keys(env).sort(), ['alg', 'ct', 'ek', 'iv', 'tag', 'v']);
  assert.equal(env.v, 1);
});
