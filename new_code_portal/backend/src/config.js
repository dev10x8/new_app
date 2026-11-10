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
};

export default config;
