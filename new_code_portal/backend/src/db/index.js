import { DatabaseSync } from 'node:sqlite';
import { readFileSync, mkdirSync, existsSync } from 'node:fs';
import { dirname, join } from 'node:path';
import { fileURLToPath } from 'node:url';

const here = dirname(fileURLToPath(import.meta.url));
const schemaPath = join(here, 'schema.sql');
const seedPath = join(here, 'seed.sql');

/**
 * Open (and on first run create + seed) the portal SQLite database.
 * @param {string} dbFile absolute path to the .db file
 * @returns {DatabaseSync}
 */
export function openDatabase(dbFile) {
  const fresh = !existsSync(dbFile);
  mkdirSync(dirname(dbFile), { recursive: true });
  const db = new DatabaseSync(dbFile);
  db.exec('PRAGMA foreign_keys = ON;');
  db.exec('PRAGMA journal_mode = WAL;');
  db.exec(readFileSync(schemaPath, 'utf8'));
  migrate(db);
  if (fresh) db.exec(readFileSync(seedPath, 'utf8'));
  else seedIfEmpty(db);
  return db;
}

/**
 * Add columns introduced after the first release to databases created before
 * them. `CREATE TABLE IF NOT EXISTS` in schema.sql does not alter an existing
 * table, so upgrades are applied here. Each entry is added only when missing,
 * which keeps this safe to run on every open.
 */
function migrate(db) {
  const addColumn = (table, column, ddl) => {
    const cols = db.prepare(`PRAGMA table_info(${table})`).all().map((c) => c.name);
    if (!cols.includes(column)) db.exec(`ALTER TABLE ${table} ADD COLUMN ${column} ${ddl}`);
  };
  addColumn('licenses', 'grace_days', 'INTEGER NOT NULL DEFAULT 0');
  addColumn('licenses', 'offline_limit_days', 'INTEGER NOT NULL DEFAULT 0');
  addColumn('licenses', 'seat_limit', 'INTEGER NOT NULL DEFAULT 1');
  addColumn('licenses', 'bound_machine_hash', "TEXT NOT NULL DEFAULT ''");
  addColumn('licenses', 'surrendered_at', "TEXT NOT NULL DEFAULT ''");
  addColumn('activations', 'released_at', "TEXT NOT NULL DEFAULT ''");
}

/** Keep an existing database supplied with catalogue rows (idempotent). */
function seedIfEmpty(db) {
  const n = db.prepare('SELECT COUNT(*) AS n FROM plans').get().n;
  if (!n) db.exec(readFileSync(seedPath, 'utf8'));
}

/** Run a query returning all rows. */
export function all(db, sql, params = []) {
  return db.prepare(sql).all(...params);
}

/** Run a query returning the first row (or undefined). */
export function get(db, sql, params = []) {
  return db.prepare(sql).get(...params);
}

/** Run a statement. */
export function run(db, sql, params = []) {
  return db.prepare(sql).run(...params);
}

/** Run fn inside a transaction. */
export function tx(db, fn) {
  db.exec('BEGIN');
  try {
    const out = fn();
    db.exec('COMMIT');
    return out;
  } catch (err) {
    db.exec('ROLLBACK');
    throw err;
  }
}
