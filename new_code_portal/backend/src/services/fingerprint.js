import { createHash } from 'node:crypto';

/**
 * Hardware fingerprint normalisation + hashing.
 *
 * The desktop installer collects a handful of stable machine identifiers
 * (primary MAC address, CPU id, system-disk serial, and optionally hostname /
 * OS) and posts them here. We normalise each component, then hash the ordered
 * set with SHA-256. The resulting `machine_hash` is what a licence is bound to:
 *
 *   * It is stable across reboots and NIC renames (we normalise MAC case and
 *     punctuation and prefer the primary/first non-virtual NIC).
 *   * It is sensitive to the machine: moving the same install to another box
 *     changes at least the MAC and usually the CPU/disk id, so the hash no
 *     longer matches and the licence refuses to activate (anti-clone).
 *   * It is a one-way digest: the portal never needs the raw identifiers to
 *     validate, and the licence file leaks nothing reusable.
 *
 * Callers may send either a structured `components` object or a flat payload
 * with `mac` / `cpu` / `disk` / `hostname` / `os` fields (what the installer
 * script emits).
 */

const clean = (v) => String(v == null ? '' : v).trim();

/** Normalise a MAC address to lower-case colon-separated hex, or '' if invalid. */
export function normaliseMac(mac) {
  const hex = clean(mac).toLowerCase().replace(/[^0-9a-f]/g, '');
  if (hex.length !== 12) return '';
  return hex.match(/.{2}/g).join(':');
}

/**
 * Normalise a list of MAC addresses: drop blanks and locally-administered /
 * multicast MACs (those are unstable for virtual adapters), sort for a stable
 * order and keep the primary (first) one prominent.
 */
export function normaliseMacs(input) {
  const list = Array.isArray(input) ? input : [input];
  const macs = list.map(normaliseMac).filter(Boolean);
  // Drop multicast + locally-administered addresses (2nd-least-significant bit
  // of the first octet set) — they belong to virtual/tunnel adapters.
  const physical = macs.filter((m) => (parseInt(m.slice(0, 2), 16) & 0x02) === 0);
  const use = (physical.length ? physical : macs);
  return [...new Set(use)].sort();
}

/** Pull a normalised component set out of either structured or flat input. */
export function normaliseComponents(input = {}) {
  const c = input.components || input;
  const macs = normaliseMacs(c.macs || c.mac || c.macaddress);
  return {
    macs,
    cpu: clean(c.cpu || c.cpuid).toLowerCase(),
    disk: clean(c.disk || c.diskserial || c.diskid).toLowerCase(),
    hostname: clean(c.hostname).toLowerCase(),
    os: clean(c.os || c.osname).toLowerCase(),
  };
}

/**
 * Stable, order-independent serialisation of the components. Only the fields
 * that actually identify the box are included; hostname/os are advisory and
 * folded in only when present so a rename does not silently invalidate a
 * licence on machines that did not report them.
 */
function canonical(components) {
  const c = components;
  const parts = [
    `mac:${c.macs.join(',')}`,
    `cpu:${c.cpu}`,
    `disk:${c.disk}`,
  ];
  if (c.hostname) parts.push(`host:${c.hostname}`);
  return parts.join('|');
}

/** SHA-256 (hex) of the normalised fingerprint. */
export function machineHash(input = {}) {
  const components = normaliseComponents(input);
  return createHash('sha256').update(canonical(components)).digest('hex');
}

/** Human-readable detail to store alongside the hash (never the raw MAC list is required). */
export function machineDetail(input = {}) {
  const c = normaliseComponents(input);
  return {
    macs: c.macs,
    mac: c.macs[0] || '',
    cpu: c.cpu,
    disk: c.disk,
    hostname: c.hostname,
    os: c.os,
  };
}

/** Recompute the hash from stored detail (used to verify an activation). */
export function hashFromDetail(detail = {}) {
  return machineHash({ components: detail });
}
