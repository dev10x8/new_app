import { createPublicKey, verify as cryptoVerify, createHash } from 'node:crypto';
import { readFileSync } from 'node:fs';
import { dirname, join } from 'node:path';
import { fileURLToPath } from 'node:url';
import { networkInterfaces, hostname, platform, release } from 'node:os';
import { spawnSync } from 'node:child_process';
import { get, run } from '../db/index.js';
import { machineHash, machineDetail, normaliseMacs } from './fingerprint.js';
import { seal } from './seal.js';
import { VENDOR_PUBLIC_KEY, DEMO_DAYS } from '../license/vendor.js';

/**
 * Desktop licence enforcement.
 *
 * The app ships the vendor's Ed25519 PUBLIC key (compiled in from
 * `license/vendor.js`) and verifies every licence offline — no call to the
 * portal. A licence is the canonical-JSON payload the portal signed.
 *
 * Two ways a licence arrives:
 *   * DEMO  - signed by the vendor at build time, unbound and with a relative
 *             term (`demo_days`). Activating it starts the clock on this machine
 *             and binds it to the machine fingerprint.
 *   * IMPORT - a portal-issued `.vlic`, already bound to this machine's
 *             fingerprint (the customer pasted it into the portal).
 *
 * The stored licence lives in `tbl_License` (single row). Nothing is trusted
 * from the row itself: the signature is always re-checked against the pinned
 * key, so editing the row cannot extend a term.
 */

const here = dirname(fileURLToPath(import.meta.url));

// ------------------------------------------------------------ canonical json
export function canonicalJson(value) {
  if (Array.isArray(value)) return `[${value.map(canonicalJson).join(',')}]`;
  if (value && typeof value === 'object') {
    const keys = Object.keys(value).sort();
    return `{${keys.map((k) => `${JSON.stringify(k)}:${canonicalJson(value[k])}`).join(',')}}`;
  }
  return JSON.stringify(value);
}

/** Verify a signed payload against a public key (base64 SPKI DER). */
export function verify(payload, signature, publicKeyB64 = VENDOR_PUBLIC_KEY) {
  try {
    const key = createPublicKey({ key: Buffer.from(publicKeyB64, 'base64'), format: 'der', type: 'spki' });
    const body = canonicalJson(payload);
    return cryptoVerify(null, Buffer.from(body, 'utf8'), key, Buffer.from(signature, 'base64'));
  } catch {
    return false;
  }
}

// ------------------------------------------------------------ fingerprint
/** Best-effort Windows identifiers; blank on other platforms. */
function windowsIds() {
  if (platform() !== 'win32') return { cpu: '', disk: '' };
  const opts = { encoding: 'utf8', windowsHide: true, timeout: 6000 };
  const run = (cmd, args) => {
    const r = spawnSync(cmd, args, opts);
    return r.status === 0 && r.stdout ? String(r.stdout) : '';
  };
  // PowerShell CIM is the supported path; `wmic` is deprecated on recent
  // Windows and may be absent, so it is only a fallback.
  const ps = (expr) => run('powershell', ['-NoProfile', '-NonInteractive', '-Command', expr]).trim();
  const wmic = (args) => run('wmic', args)
    .split(/\r?\n/).map((l) => l.trim())
    .filter((l) => l && !/^(ProcessorId|SerialNumber)$/i.test(l))[0] || '';
  const cpu = ps('(Get-CimInstance Win32_Processor | Select-Object -First 1 -ExpandProperty ProcessorId)')
    || wmic(['cpu', 'get', 'ProcessorId', '/value']);
  const disk = ps('(Get-CimInstance Win32_DiskDrive | Where-Object { $_.Index -eq 0 } | Select-Object -First 1 -ExpandProperty SerialNumber)')
    || wmic(['diskdrive', 'where', 'index=0', 'get', 'SerialNumber', '/value']);
  return { cpu: cpu.trim(), disk: disk.trim() };
}

/** Collect the identifiers a licence binds to. */
export function collectFingerprint() {
  const macs = [];
  for (const addrs of Object.values(networkInterfaces())) {
    for (const a of addrs || []) {
      if (a.mac && a.mac !== '00:00:00:00:00:00') macs.push(a.mac);
    }
  }
  const ids = windowsIds();
  return {
    macs: normaliseMacs(macs),
    cpu: ids.cpu,
    disk: ids.disk,
    hostname: hostname(),
    os: `${platform()} ${release()}`,
  };
}

/** Human-facing fingerprint: the hash (what the portal binds) plus detail. */
export function fingerprint() {
  const components = collectFingerprint();
  return { machine_hash: machineHash(components), ...machineDetail(components) };
}

// ------------------------------------------------------------ storage
const today = () => new Date().toISOString().slice(0, 10);
const addDays = (date, days) => {
  const d = new Date(`${date}T00:00:00Z`);
  d.setUTCDate(d.getUTCDate() + days);
  return d.toISOString().slice(0, 10);
};
const daysBetween = (a, b) => Math.round((Date.parse(`${b}T00:00:00Z`) - Date.parse(`${a}T00:00:00Z`)) / 86400000);

function loadRow(db) {
  return get(db, 'SELECT * FROM tbl_License WHERE id=1');
}

/** Load and decode the demo licence bundled with the app. */
export function demoLicence() {
  const file = join(here, '..', 'license', 'demo.vlic');
  return JSON.parse(readFileSync(file, 'utf8'));
}

/** Activate the bundled demo on this machine: bind it and start its clock. */
export function activateDemo(db, actor = 'desktop') {
  const demo = demoLicence();
  if (!verify(demo.payload, demo.signature)) {
    throw Object.assign(new Error('The bundled demo licence failed verification'), { status: 500 });
  }
  const fp = fingerprint();
  const from = today();
  const until = addDays(from, Number(demo.demo_days) || DEMO_DAYS);
  store(db, {
    kind: 'demo',
    license_id: demo.license_id,
    payload: demo.payload,
    signature: demo.signature,
    machine_hash: fp.machine_hash,
    valid_from: from,
    valid_until: until,
  }, actor);
  return status(db);
}

/**
 * Import a portal-issued `.vlic`. The signature must verify against the pinned
 * vendor key and, when the licence is machine-bound, the fingerprint must match
 * this machine.
 */
export function importLicence(db, file, actor = 'desktop') {
  const doc = typeof file === 'string' ? JSON.parse(file) : file;
  if (!doc || !doc.payload || !doc.signature) {
    throw Object.assign(new Error('Not a VanijLiquor licence file'), { status: 400 });
  }
  if (!verify(doc.payload, doc.signature)) {
    throw Object.assign(new Error('Licence signature is not valid (issued for another vendor or tampered)'), { status: 400 });
  }
  const payload = doc.payload;
  const fp = fingerprint();
  const bound = String(payload.machine_hash || '');
  if (bound && bound !== fp.machine_hash) {
    throw Object.assign(new Error('This licence is bound to a different machine'), { status: 403 });
  }
  store(db, {
    kind: payload.kind === 'demo' ? 'demo' : 'issued',
    license_id: payload.license_id,
    payload,
    signature: doc.signature,
    machine_hash: bound || fp.machine_hash,
    valid_from: payload.valid_from || today(),
    valid_until: payload.valid_until || addDays(today(), Number(payload.demo_days) || DEMO_DAYS),
  }, actor);
  return status(db);
}

function store(db, l, actor) {
  // The row keeps two representations of the same licence:
  //   * payload + signature  - the signed copy the app enforces offline;
  //   * sealed               - the same record encrypted to the vendor's public
  //                            key, so only the portal can ever read it back.
  const sealed = seal({ payload: l.payload, signature: l.signature, machine_hash: l.machine_hash,
    kind: l.kind, valid_from: l.valid_from, valid_until: l.valid_until });
  run(db, `INSERT INTO tbl_License
    (id, kind, license_id, machine_hash, payload, signature, sealed, valid_from, valid_until, activated_at, updatedat)
    VALUES (1,?,?,?,?,?,?,?,?,?,?)
    ON CONFLICT(id) DO UPDATE SET
      kind=excluded.kind, license_id=excluded.license_id, machine_hash=excluded.machine_hash,
      payload=excluded.payload, signature=excluded.signature, sealed=excluded.sealed,
      valid_from=excluded.valid_from, valid_until=excluded.valid_until,
      activated_at=excluded.activated_at, updatedat=excluded.updatedat`,
    [l.kind, l.license_id, l.machine_hash, canonicalJson(l.payload), l.signature, sealed,
      l.valid_from, l.valid_until, l.valid_from, new Date().toISOString()]);
  void actor;
}

/** Remove the stored licence (e.g. to re-run setup). */
export function clear(db) {
  run(db, 'DELETE FROM tbl_License WHERE id=1');
}

// ------------------------------------------------------------ status
/**
 * The licence verdict the UI and the route gate act on:
 *   state    none | valid | grace | expired | invalid
 *   severity ok   | warn  | hard
 */
export function status(db) {
  const row = loadRow(db);
  const fp = fingerprint();
  if (!row) {
    return { present: false, state: 'none', severity: 'hard', days_remaining: 0, machine_hash: fp.machine_hash };
  }
  let payload;
  try { payload = JSON.parse(row.payload); } catch { payload = null; }
  const signatureOk = payload ? verify(payload, row.signature) : false;
  const base = {
    present: true,
    kind: row.kind,
    license_id: row.license_id,
    signature_ok: signatureOk,
    sealed: Boolean(row.sealed),
    valid_from: row.valid_from,
    valid_until: row.valid_until,
    activated_at: row.activated_at,
    plan: payload ? payload.plan : '',
    entitlements: payload ? payload.entitlements || [] : [],
    machine_hash: fp.machine_hash,
    machine_ok: String(payload?.machine_hash || '') === '' || payload?.machine_hash === fp.machine_hash,
  };
  if (!signatureOk) return { ...base, state: 'invalid', severity: 'hard', days_remaining: 0 };

  const remaining = daysBetween(today(), row.valid_until);
  const grace = Number(payload.grace_days) || 0;
  if (remaining >= 0) return { ...base, state: 'valid', severity: 'ok', days_remaining: remaining, grace_remaining: 0 };
  if (-remaining <= grace) return { ...base, state: 'grace', severity: 'warn', days_remaining: 0, grace_remaining: grace + remaining };
  return { ...base, state: 'expired', severity: 'hard', days_remaining: 0, grace_remaining: 0 };
}

/** True when the app should refuse to run (no licence, invalid, or expired). */
export function isBlocked(db) {
  const s = status(db);
  return s.severity === 'hard';
}

export { machineHash };
