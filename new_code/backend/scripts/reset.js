import { existsSync, rmSync } from 'node:fs';
import { config } from '../src/config.js';
import { openDatabase } from '../src/db/index.js';

// Drop the database file (and WAL/SHM sidecars) so the next open reseeds it.
for (const suffix of ['', '-wal', '-shm']) {
  const f = config.dbFile + suffix;
  if (existsSync(f)) rmSync(f);
}
const db = openDatabase(config.dbFile);
const tables = db.prepare("SELECT COUNT(*) AS n FROM sqlite_master WHERE type='table'").get();
const items = db.prepare('SELECT COUNT(*) AS n FROM itemmst').get();
const users = db.prepare('SELECT COUNT(*) AS n FROM users').get();
console.log(`Reset ${config.dbFile}: ${tables.n} tables, ${items.n} items, ${users.n} users`);
db.close();
