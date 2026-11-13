import {
  createPublicKey, publicEncrypt, randomBytes, createCipheriv, constants,
} from 'node:crypto';
import { VENDOR_SEAL_KEY } from '../license/vendor.js';

/**
 * Seal a licence record with the vendor's RSA PUBLIC key.
 *
 * This is the encrypt-only half of `new_code_portal/backend/src/services/seal.js`.
 * The desktop app can WRITE a sealed record but cannot read it back — unsealing
 * needs the vendor's private key, which only the portal holds. So the copy of a
 * licence left on a customer's machine (or in a backup, or a support bundle) is
 * opaque to everyone but the vendor.
 *
 * RSA-OAEP cannot encrypt more than a few hundred bytes, so this is hybrid: a
 * random AES-256-GCM key encrypts the record, and RSA-OAEP wraps that key. The
 * result is the same `{ v, alg, ek, iv, tag, ct }` envelope the portal unseals.
 *
 * This is confidentiality only. Unforgeability comes from the Ed25519 signature
 * (see license.js) — the app always enforces the SIGNED payload, never the seal.
 */

const OAEP = { padding: constants.RSA_PKCS1_OAEP_PADDING, oaepHash: 'sha256' };

export function seal(value) {
  const pub = createPublicKey({ key: Buffer.from(VENDOR_SEAL_KEY, 'base64'), format: 'der', type: 'spki' });
  const key = randomBytes(32);
  const iv = randomBytes(12);
  const cipher = createCipheriv('aes-256-gcm', key, iv);
  const ct = Buffer.concat([cipher.update(Buffer.from(JSON.stringify(value), 'utf8')), cipher.final()]);
  const ek = publicEncrypt({ key: pub, ...OAEP }, key);
  return JSON.stringify({
    v: 1,
    alg: 'RSA-OAEP-256+A256GCM',
    ek: ek.toString('base64'),
    iv: iv.toString('base64'),
    tag: cipher.getAuthTag().toString('base64'),
    ct: ct.toString('base64'),
  });
}
