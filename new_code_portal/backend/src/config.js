import { dirname, resolve } from 'node:path';
import { fileURLToPath } from 'node:url';

const here = dirname(fileURLToPath(import.meta.url));

export const config = {
  // new_code_portal/data/portal.db
  dbFile: process.env.PORTAL_DB || resolve(here, '..', '..', 'data', 'portal.db'),
  host: process.env.HOST || '127.0.0.1',
  port: Number(process.env.PORT || 7200),
  // Ed25519 seed (hex, 32 bytes) that signs licences. When unset, a deterministic
  // development key is derived so a fresh checkout works out of the box. In
  // production set PORTAL_LICENSE_KEY to a secret seed kept on the server only.
  licenseKey: process.env.PORTAL_LICENSE_KEY || '',
  // Human-readable vendor identity embedded in issued licences.
  vendor: process.env.PORTAL_VENDOR || 'VanijLiquor',
  issuer: process.env.PORTAL_ISSUER || 'VanijLiquor Licensing Authority',
  // How long a generated licence stays valid (months) when a plan has no term.
  defaultTermMonths: Number(process.env.PORTAL_TERM_MONTHS || 12),
  // After valid_until the licence still verifies but reports "grace" for this
  // many days before it hard-expires — so a renewal that lands a few days late
  // does not stop a paying customer's shop.
  graceDays: Number(process.env.PORTAL_GRACE_DAYS || 15),
  // How many days the desktop may run without a check-in before it drops to
  // limited "educational" mode (Tally-style). Baked into the signed payload so
  // the client can enforce it offline.
  offlineLimitDays: Number(process.env.PORTAL_OFFLINE_LIMIT_DAYS || 30),
  // How many distinct machines one licence may bind (seats).
  seatLimit: Number(process.env.PORTAL_SEAT_LIMIT || 1),
};

export default config;
