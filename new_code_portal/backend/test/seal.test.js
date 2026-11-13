import test from 'node:test';
import assert from 'node:assert/strict';
import { seal, unseal, sealPublicKey, usingProductionKey } from '../src/services/seal.js';

test('seal/unseal round-trips a licence record', () => {
  const record = {
    payload: { v: 2, license_id: 'VL-LIC-TEST01', plan: 'premium', entitlements: ['TALLY', 'SALES_POS'] },
    signature: 'c2lnbmF0dXJl',
    machine_hash: 'a'.repeat(64),
    kind: 'issued',
  };
  const sealed = seal(record);
  assert.equal(typeof sealed, 'string');
  const env = JSON.parse(sealed);
  assert.equal(env.alg, 'RSA-OAEP-256+A256GCM');
  // The plaintext must not be recoverable from the ciphertext alone.
  assert.equal(sealed.includes('VL-LIC-TEST01'), false);
  assert.deepEqual(unseal(sealed), record);
});

test('tampering with the sealed ciphertext is detected', () => {
  const env = JSON.parse(seal({ a: 1 }));
  const ct = Buffer.from(env.ct, 'base64');
  ct[0] ^= 0xff;
  env.ct = ct.toString('base64');
  assert.throws(() => unseal(env));
});

test('the public seal key is exposed and the dev key is flagged', () => {
  assert.match(sealPublicKey(), /^MIIB/);
  assert.equal(usingProductionKey(), false);
});
