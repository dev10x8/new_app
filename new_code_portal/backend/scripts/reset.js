#!/usr/bin/env node
// Drop the portal database so the next open recreates + reseeds it.
import { existsSync, rmSync } from 'node:fs';
import { config } from '../src/config.js';
import { openDatabase } from '../src/db/index.js';

for (const suffix of ['', '-wal', '-shm']) {
  const f = config.dbFile + suffix;
  if (existsSync(f)) rmSync(f);
}
const db = openDatabase(config.dbFile);
const plans = db.prepare('SELECT COUNT(*) AS n FROM plans').get().n;
const modules = db.prepare('SELECT COUNT(*) AS n FROM modules').get().n;
console.log(`Reset ${config.dbFile}: ${plans} plans, ${modules} modules`);
db.close();
