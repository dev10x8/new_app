import { existsSync, rmSync } from 'node:fs';
import { join } from 'node:path';
import { parseArgs } from 'node:util';
import { config } from './config.js';
import { openDatabase } from './db/index.js';
import { listUsers, resetPin } from './services/auth.js';

/**
 * Recovery commands the packaged backend executable exposes as subcommands
 * (`backend.exe reset-pin ...`, `backend.exe reset-db`). Kept in the bundle so
 * the shipped app has no readable .js for a locked-out shop to read or edit.
 */

/**
 * Where the database lives. The packaged desktop app keeps it under the Tauri
 * identifier in %LOCALAPPDATA%, so when run from an install without an explicit
 * VANIJLIQUOR_DB we look there before falling back to the dev path.
 */
export function resolveDbFile() {
  if (process.env.VANIJLIQUOR_DB) return config.dbFile;
  if (existsSync(config.dbFile)) return config.dbFile;
  const local = process.env.LOCALAPPDATA || process.env.APPDATA;
  if (local) {
    const packaged = join(local, 'com.vanijliquor.pro', 'vanijliquor.db');
    if (existsSync(packaged)) return packaged;
  }
  return config.dbFile;
}

const usage = (dbFile) => `VanijLiquor backend (packaged executable).

  backend.exe                       start the API server (default)
  backend.exe reset-pin [usercode] [pin]   reset a user's PIN (defaults A00001 / 1234)
  backend.exe reset-pin --list             list users
  backend.exe reset-db                     drop and reseed the database (DESTRUCTIVE)

Database: ${dbFile} (override with VANIJLIQUOR_DB)`;

/** `reset-pin` subcommand: non-destructive PIN recovery. */
export function resetPinCommand(argv) {
  const { values, positionals } = parseArgs({
    args: argv,
    options: {
      user: { type: 'string', short: 'u' },
      pin: { type: 'string', short: 'p' },
      list: { type: 'boolean', short: 'l' },
      help: { type: 'boolean', short: 'h' },
    },
    allowPositionals: true,
  });
  const dbFile = resolveDbFile();
  if (values.help) { console.log(usage(dbFile)); return 0; }

  const db = openDatabase(dbFile);
  try {
    if (values.list) {
      for (const u of listUsers(db)) console.log(`${u.usercode}\t${u.username}\t${u.accesslevel}`);
      return 0;
    }
    const user = values.user ?? positionals[0] ?? 'A00001';
    const pin = values.pin ?? positionals[1] ?? '1234';
    if (pin == null || String(pin).length < 1) {
      console.error('The new PIN must not be empty.');
      return 1;
    }
    resetPin(db, user, pin);
    console.log(`PIN reset for ${user}.`);
    return 0;
  } catch (err) {
    console.error(err.message);
    return 1;
  } finally {
    db.close();
  }
}

/** `reset-db` subcommand: drop the database file so the next open reseeds it. */
export function resetDbCommand() {
  const dbFile = resolveDbFile();
  for (const suffix of ['', '-wal', '-shm']) {
    const f = dbFile + suffix;
    if (existsSync(f)) rmSync(f);
  }
  const db = openDatabase(dbFile);
  const tables = db.prepare("SELECT COUNT(*) AS n FROM sqlite_master WHERE type='table'").get();
  const items = db.prepare('SELECT COUNT(*) AS n FROM itemmst').get();
  const users = db.prepare('SELECT COUNT(*) AS n FROM users').get();
  console.log(`Reset ${dbFile}: ${tables.n} tables, ${items.n} items, ${users.n} users`);
  db.close();
  return 0;
}

/**
 * Run a recovery subcommand if `name` is one, else return null so the caller
 * starts the server. Exit codes are the caller's to apply.
 */
export function runRecovery(name, argv) {
  if (name === 'reset-pin' || name === 'reset-admin-pin') return resetPinCommand(argv);
  if (name === 'reset-db') return resetDbCommand();
  if (name === '--help' || name === '-h' || name === 'help') { console.log(usage(resolveDbFile())); return 0; }
  return null;
}
