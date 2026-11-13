#!/usr/bin/env node
// Reset a user's PIN without touching any other data.
//
// Use when a shop is locked out because the Admin PIN was forgotten:
//
//   npm run reset-admin-pin              # defaults to A00001 / 1234
//   npm run reset-admin-pin A00001 9876  # explicit user + PIN
//   node scripts/reset-pin.js --user A00001 --pin 1234
//   node scripts/reset-pin.js --list
//
// The database defaults to config.dbFile (override with VANIJLIQUOR_DB). Unlike
// `npm run reset-db` this is NOT destructive: it only updates users.password.
import { parseArgs } from 'node:util';
import { config } from '../src/config.js';
import { openDatabase } from '../src/db/index.js';
import { listUsers, resetPin } from '../src/services/auth.js';

const { values, positionals } = parseArgs({
  options: {
    user: { type: 'string', short: 'u' },
    pin: { type: 'string', short: 'p' },
    list: { type: 'boolean', short: 'l' },
    help: { type: 'boolean', short: 'h' },
  },
  allowPositionals: true,
});

// Positional form: reset-admin-pin [usercode] [pin]
const user = values.user ?? positionals[0] ?? 'A00001';
const pin = values.pin ?? positionals[1] ?? '1234';

if (values.help) {
  console.log(`Reset a user's PIN.

  reset-admin-pin [usercode] [pin]   positional form (defaults A00001 / 1234)
  --user, -u <usercode>              user to reset
  --pin,  -p <pin>                   new PIN/password
  --list, -l                         list users and exit

Database: ${config.dbFile} (override with VANIJLIQUOR_DB)`);
  process.exit(0);
}

const db = openDatabase(config.dbFile);

if (values.list) {
  for (const u of listUsers(db)) console.log(`${u.usercode}\t${u.username}\t${u.accesslevel}`);
  db.close();
  process.exit(0);
}

if (pin == null || String(pin).length < 1) {
  console.error('The new PIN must not be empty. Use --help for usage.');
  db.close();
  process.exit(1);
}

try {
  resetPin(db, user, pin);
  console.log(`PIN reset for ${user}.`);
} catch (err) {
  console.error(err.message);
  db.close();
  process.exit(1);
}
db.close();
