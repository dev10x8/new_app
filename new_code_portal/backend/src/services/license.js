import {
  createHash, createPrivateKey, createPublicKey, generateKeyPairSync,
  sign as cryptoSign, verify as cryptoVerify,
} from 'node:crypto';
import { config } from '../config.js';

/**
 * Licence signing & verification.
 *
 * A licence is a JSON payload plus a detached Ed25519 signature. The desktop
 * app ships the vendor **public key** and can therefore verify a licence
 * offline (no call to the portal). Signing uses the vendor **private key**,
 * which only the portal holds. The payload is canonicalised (sorted keys, no
 * whitespace) before signing so verification is deterministic.
 *
 * Key management:
 *   * Production: set PORTAL_LICENSE_KEY to a 32-byte hex seed (keep it secret,
 *     on the server only). Its public key is embedded in every licence.
 *   * Development: when unset, a fixed seed is used so a fresh checkout can
 *     issue licences the bundled verifier accepts. This key is NOT secret and
 *     must never be used in production.
 */

const DEV_SEED = createHash('sha256').update('vanijliquor-portal-dev-license-key').digest();

function seedBuffer() {
  const hex = String(config.licenseKey || '').trim();
  if (/^[0-9a-fA-F]{64}$/.test(hex)) return Buffer.from(hex, 'hex');
  return DEV_SEED;
}

/** Build an Ed25519 private key object from the configured seed. */
export function privateKey() {
  const seed = seedBuffer();
  // PKCS#8 wrapper for a raw Ed25519 seed (RFC 8410).
  const der = Buffer.concat([
    Buffer.from('302e020100300506032b657004220420', 'hex'),
    seed,
  ]);
  return createPrivateKey({ key: der, format: 'der', type: 'pkcs8' });
}

/** The matching public key object. */
export function publicKey() {
  return createPublicKey(privateKey());
}

/** Base64 DER (SPKI) of the public key — what the desktop app pins. */
export function publicKeyBase64() {
  return publicKey().export({ type: 'spki', format: 'der' }).toString('base64');
}

/** True when a real (non-dev) signing key is configured. */
export function usingProductionKey() {
  return /^[0-9a-fA-F]{64}$/.test(String(config.licenseKey || '').trim());
}

/** Deterministic JSON: object keys sorted recursively, no extraneous space. */
export function canonicalJson(value) {
  if (Array.isArray(value)) return `[${value.map(canonicalJson).join(',')}]`;
  if (value && typeof value === 'object') {
    const keys = Object.keys(value).sort();
    return `{${keys.map((k) => `${JSON.stringify(k)}:${canonicalJson(value[k])}`).join(',')}}`;
  }
  return JSON.stringify(value);
}

/** Sign a payload object; returns { payload, signature, publicKey }. */
export function sign(payload) {
  const body = canonicalJson(payload);
  const signature = cryptoSign(null, Buffer.from(body, 'utf8'), privateKey()).toString('base64');
  return { payload, body, signature, publicKey: publicKeyBase64() };
}

/** Verify a signed payload against a public key (base64 DER). */
export function verify(payload, signature, publicKeyB64 = publicKeyBase64()) {
  try {
    const key = createPublicKey({ key: Buffer.from(publicKeyB64, 'base64'), format: 'der', type: 'spki' });
    const body = canonicalJson(payload);
    return cryptoVerify(null, Buffer.from(body, 'utf8'), key, Buffer.from(signature, 'base64'));
  } catch {
    return false;
  }
}

/** Generate a fresh key pair (for the `npm run gen-license-key` helper). */
export function generateKeyPair() {
  const { privateKey: priv, publicKey: pub } = generateKeyPairSync('ed25519');
  const seedDer = priv.export({ type: 'pkcs8', format: 'der' });
  // The raw 32-byte seed is the last 32 bytes of the PKCS#8 DER above.
  const seed = seedDer.subarray(seedDer.length - 32).toString('hex');
  return { seed, publicKey: pub.export({ type: 'spki', format: 'der' }).toString('base64') };
}
