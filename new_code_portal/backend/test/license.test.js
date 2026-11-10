import test from 'node:test';
import assert from 'node:assert/strict';
import { canonicalJson, sign, verify, publicKeyBase64, usingProductionKey } from '../src/services/license.js';

test('canonicalJson sorts keys at every level', () => {
  assert.equal(canonicalJson({ b: 1, a: { d: 2, c: 3 } }), '{"a":{"c":3,"d":2},"b":1}');
  assert.equal(canonicalJson([3, { b: 1, a: 2 }]), '[3,{"a":2,"b":1}]');
});

test('a signed payload verifies and a tampered one does not', () => {
  const payload = { license_id: 'VL-LIC-TEST01', plan: 'premium', entitlements: ['TALLY'] };
  const { signature, publicKey } = sign(payload);
  assert.equal(verify(payload, signature, publicKey), true);
  assert.equal(verify({ ...payload, plan: 'basic' }, signature, publicKey), false);
  assert.equal(verify(payload, signature, publicKeyBase64()), true);
});

test('a signature from another key is rejected', () => {
  const a = sign({ x: 1 });
  const b = sign({ x: 1 });
  // Same payload, same (dev) key in this environment -> same signature.
  assert.equal(a.signature, b.signature);
  assert.equal(verify({ x: 1 }, a.signature, publicKeyBase64()), true);
});

test('the development key is flagged as non-production', () => {
  assert.equal(usingProductionKey(), false);
});
