import {
  createPrivateKey, createPublicKey, publicEncrypt, privateDecrypt,
  randomBytes, createCipheriv, createDecipheriv, constants,
} from 'node:crypto';
import { config } from '../config.js';

/**
 * Licence sealing (confidentiality, at rest).
 *
 * A licence record is sealed with the vendor's RSA PUBLIC key and can only be
 * unsealed with the matching PRIVATE key, which lives on the portal server. So
 * a copy of a licence sitting on a customer's disk (or in a backup, or a
 * support bundle) is opaque to everyone but the vendor, who can recover it for
 * audit, renewal or dispute.
 *
 * RSA-OAEP can only encrypt a few hundred bytes, so this is hybrid: a random
 * AES-256-GCM key encrypts the record, and RSA-OAEP wraps that key. The sealed
 * value is a JSON envelope `{ v, alg, ek, iv, tag, ct }` (all base64).
 *
 * This is deliberately separate from the Ed25519 signature (see license.js):
 *   * the SIGNATURE gives unforgeability — the app verifies a licence offline
 *     and nobody can mint or edit one without the signing key;
 *   * the SEAL gives confidentiality — the at-rest copy cannot be read without
 *     the sealing key held by the portal.
 *
 * Key management:
 *   * Production: set PORTAL_SEAL_KEY to a base64 PKCS#8 RSA private key.
 *   * Development: a fixed key is used so a fresh checkout round-trips. It is
 *     NOT secret and must never be used in production.
 */

// Throwaway RSA-2048 key pair for development. NOT secret.
const DEV_PRIVATE_KEY = `MIIEvgIBADANBgkqhkiG9w0BAQEFAASCBKgwggSkAgEAAoIBAQDCVXsWwciNJQABXAN62Bbniaxg3ssL+9+6G8257mBFjDSTysHWZUQz1dEa02WGruP9M56hBlCdtjJl8H3lqZQGitBahsl6hIANUHHY6X/FhRa9KFXclpQHBEjelA8MI3HaWHm8CpXDNni/LPl6Jsvnsvjt/O8z+tuEVtan+AodBKkW/5GfJhjMG6/HduhQhrmpA9X5X1DbZFIYe6pncnDpojcbridcHiD8z+q+Zb+RbQOSuWnH2d200FXJf/VY+OvmVS48l2OgNq4YyW41TGuQ0PPxbEThrOhZeLqwlfDXDKANtbAjLeJB0glWzWgPeEqFE3u2mpcuNFtknp7sUVmlAgMBAAECggEAAadT486u/ocjvTSOPFuzCNytYH76KqubAhdoeb44iCmi7OWwc+WVXfziHvX7itjZdyHFph3Ko9JAEt6oA5icz4LGNm79Lz4KD5LxVQ+E7HuvGi70rsAtREivYg16efeh6WIQBHSb76GlXLhs6wgs0KzqMgQ7+NeX9JlUkZtVDV16pGvZl9yfAL1lzh9nPjBQzSqS5onzfmV/iSD17mGjH/ImRrar4yWK84necWafhi7KQKVGOGrQl4+o5FvcKImUvFjmBm6/r4zRHp9td2kiPW2pFi7u1mTonOqIURaArnk6Syq2R8OSEOO4UoKc5Oxh1cUvYEoJklwFcIjJTaMZBQKBgQD0RC7IFD4pyuEAtTXhCMmYNQGY2oDzauNY7ZU42LVzgFCLTTTj8XyYN4X9PAoTuM3/1gjQaiC+xyZWIot7jwAbUGyHVyx1dqBNP5NCtIxtG6LQHL8wzJwCirUgsCHnukxp6M/WPYjTf2u/j9CEGldBvNz/XqgheMmtu7y06aJi8wKBgQDLq0OLZXiZJhNp+V6L1Bs92Kz6C/Zfumpbk6SePCdCJ7OAQlUuYiuqPo/dn9w9ww3n/mYj/U4oFwQ5fBITsjh/hMkcuK/jKTQjl58ZhV780pE2au5oZfnxDvcP2OeEdMc0R6FmTXWzMjOKtFlvHHsevyzubkBBzmR/MWyfmiQHBwKBgQCTSxYmokZF70e0pJEDzMe7l6MJ6CDAJ+FF+6Xy7IKmTmbDSWAuX60xILIUXQL6l4Lo1cjk3Eo4oGBWqmgPrtByqBF5nzB4+Hp7em2zWKohFPwdoGgN5iPhSIEmX+9iC64OY0wItXxvhLOVhQRI1JVvV5KwDwU1gd+jesS1FvlLzwKBgQCfTB3xWnU/+8r1eUjcvxJrhhyNGmwiJZDofYslBFGEab7402kCdWBHsNUfd9hwKi6nRJApGAdYTeBNAeC1rRCJlJckMsPHPcP9Dqsu7+UCg8N2N56u4wvh4l2U9IboT9o65sezV2R2uw4bL8I/qJwXyVlgR7VUgxSRc1vNXWdOYwKBgDJIaB0xP0mZL23udCTw2buADDaarqP8y6cRjR/lqFmFsg0wxNuHvKpHx3YVpJa+fJdY87hFeVFmVLj3iMU4s/sfXN1L5H/FL9RIa48l8VrFTs17KJmcjRKJ7jUx+LvJDRzwkjwdWZL/aTy9QOHstKLkN7RDrp8x6cku5yK3s29P`;

const OAEP = { padding: constants.RSA_PKCS1_OAEP_PADDING, oaepHash: 'sha256' };

/** True when a real (non-dev) sealing key is configured. */
export function usingProductionKey() {
  return /^[A-Za-z0-9+/=]{200,}$/.test(String(config.sealKey || '').trim());
}

function privateKey() {
  const b64 = usingProductionKey() ? String(config.sealKey).trim() : DEV_PRIVATE_KEY;
  return createPrivateKey({ key: Buffer.from(b64, 'base64'), format: 'der', type: 'pkcs8' });
}

/** The RSA public key (SPKI, base64) — the "public key provided by the portal". */
export function sealPublicKey() {
  return createPublicKey(privateKey()).export({ type: 'spki', format: 'der' }).toString('base64');
}

/** Seal an object into an opaque envelope string. Only the portal can unseal it. */
export function seal(value) {
  const key = randomBytes(32);
  const iv = randomBytes(12);
  const cipher = createCipheriv('aes-256-gcm', key, iv);
  const ct = Buffer.concat([cipher.update(Buffer.from(JSON.stringify(value), 'utf8')), cipher.final()]);
  const ek = publicEncrypt({ key: privateKey(), ...OAEP }, key);
  return JSON.stringify({
    v: 1,
    alg: 'RSA-OAEP-256+A256GCM',
    ek: ek.toString('base64'),
    iv: iv.toString('base64'),
    tag: cipher.getAuthTag().toString('base64'),
    ct: ct.toString('base64'),
  });
}

/** Unseal an envelope produced by `seal` (portal-side recovery / audit). */
export function unseal(envelope) {
  const env = typeof envelope === 'string' ? JSON.parse(envelope) : envelope;
  const key = privateDecrypt({ key: privateKey(), ...OAEP }, Buffer.from(env.ek, 'base64'));
  const decipher = createDecipheriv('aes-256-gcm', key, Buffer.from(env.iv, 'base64'));
  decipher.setAuthTag(Buffer.from(env.tag, 'base64'));
  const plain = Buffer.concat([decipher.update(Buffer.from(env.ct, 'base64')), decipher.final()]);
  return JSON.parse(plain.toString('utf8'));
}
