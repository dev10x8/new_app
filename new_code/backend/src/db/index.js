import { DatabaseSync } from 'node:sqlite';
import { readFileSync, mkdirSync, existsSync } from 'node:fs';
import { dirname, join } from 'node:path';
import { fileURLToPath } from 'node:url';

const here = dirname(fileURLToPath(import.meta.url));
const schemaPath = join(here, 'schema.sql');
const schemaExtPath = join(here, 'schema_ext.sql');
const seedPath = join(here, 'seed.sql');
const seedExtPath = join(here, 'seed_ext.sql');

/**
 * Open (and on first run create + seed) the SQLite database.
 * @param {string} dbFile absolute path to the .db file
 * @returns {DatabaseSync}
 */
export function openDatabase(dbFile) {
  const fresh = !existsSync(dbFile);
  mkdirSync(dirname(dbFile), { recursive: true });
  const db = new DatabaseSync(dbFile);
  db.exec('PRAGMA foreign_keys = ON;');
  db.exec('PRAGMA journal_mode = WAL;');
  if (fresh) {
    db.exec(readFileSync(schemaPath, 'utf8'));
    if (existsSync(schemaExtPath)) db.exec(readFileSync(schemaExtPath, 'utf8'));
    db.exec(readFileSync(seedPath, 'utf8'));
    if (existsSync(seedExtPath)) db.exec(readFileSync(seedExtPath, 'utf8'));
  }
  return db;
}

/** Run a query returning all rows. */
export function all(db, sql, params = []) {
  return db.prepare(sql).all(...params);
}

/** Run a query returning the first row (or undefined). */
export function get(db, sql, params = []) {
  return db.prepare(sql).get(...params);
}

/** Execute a statement (INSERT/UPDATE/DELETE). */
export function run(db, sql, params = []) {
  return db.prepare(sql).run(...params);
}

/** Wrap a function in a transaction. */
export function tx(db, fn) {
  db.exec('BEGIN');
  try {
    const result = fn();
    db.exec('COMMIT');
    return result;
  } catch (err) {
    db.exec('ROLLBACK');
    throw err;
  }
}
