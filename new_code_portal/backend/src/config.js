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
  // RSA private key (base64 PKCS#8) that SEALS licences at rest. The desktop app
  // stores an encrypted copy it cannot itself read; only this key (on the
  // portal) can unseal it. When unset, a development key is used.
  sealKey: process.env.PORTAL_SEAL_KEY || '',
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
  // ---- distribution (downloads + licence server links) ----
  // Version of the desktop app the portal advertises and links to.
  appVersion: process.env.PORTAL_APP_VERSION || '1.0.8',
  // GitHub repository that hosts the signed Windows installers as release assets.
  releaseRepo: process.env.PORTAL_RELEASE_REPO || 'dev10x8/new_app',
  // Release tag to link to (e.g. 'v1.0.0'). Empty links to the "latest" release.
  releaseTag: process.env.PORTAL_RELEASE_TAG || '',
  // Explicit base URL for installer downloads. When set it wins over the
  // GitHub release URL, so installers can be served from a CDN or object store.
  downloadBaseUrl: process.env.PORTAL_DOWNLOAD_BASE_URL || '',
  // Absolute base URL of this portal (its licensing/activation server). Set in
  // production so issued instructions carry a reachable URL; when empty the UI
  // falls back to the browser's current origin.
  publicUrl: process.env.PORTAL_PUBLIC_URL || '',
  // URL the desktop app uses to reach the licence server. Defaults to publicUrl.
  licenseServerUrl: process.env.PORTAL_LICENSE_SERVER_URL || '',
};

export default config;
