#!/usr/bin/env node
// Reset a user's login PIN in the new_code database.
//
// The PIN is stored in the `users` table (plaintext, exactly as the legacy
// Madhushala app stored it), so a forgotten admin PIN is a one-line update.
//
// Usage:
//   node scripts/reset_admin_pin.js                 # reset A00001 to 1234
//   node scripts/reset_admin_pin.js A00001 4321     # user code and new PIN
//   MADHUSHALA_DB=/tmp/demo.db node scripts/reset_admin_pin.js A00001 4321

import { resolve, dirname } from 'node:path';
import { fileURLToPath } from 'node:url';
import { openDatabase } from '../backend/src/db/index.js';
import * as auth from '../backend/src/services/auth.js';

const here = dirname(fileURLToPath(import.meta.url));
const DB_FILE = process.env.MADHUSHALA_DB || resolve(here, '..', 'data', 'madhushala.db');
const usercode = (process.argv[2] || 'A00001').toUpperCase();
const pin = process.argv[3] || '1234';

const db = openDatabase(DB_FILE);
const user = db.prepare('SELECT usercode, username FROM users WHERE usercode=?').get(usercode);
if (!user) {
  console.error(`No such user: ${usercode}`);
  process.exit(1);
}
auth.resetPin(db, usercode, pin);
db.close();
console.log(`PIN for ${usercode} (${user.username}) reset to "${pin}" in ${DB_FILE}`);
