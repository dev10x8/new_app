import { test } from 'node:test';
import assert from 'node:assert/strict';
import { mkdtempSync, rmSync, writeFileSync } from 'node:fs';
import { tmpdir } from 'node:os';
import { join } from 'node:path';
import { readFileSync } from 'node:fs';
import { openDatabase, get, all } from '../src/db/index.js';
import * as migration from '../src/services/migration.js';

const COMPANY = '1';
const YEAR = '2024';

function freshDb() {
  const dir = mkdtempSync(join(tmpdir(), 'madhushala-migration-'));
  return { db: openDatabase(join(dir, 'test.db')), dir };
}

// A representative legacy dump: several INSERTs, a multi-row VALUES list, an
// escaped quote (O''Brien) and a CAST(... AS DateTime) - all real T-SQL shapes.
const DUMP = `
USE [barmanager];
GO
INSERT [dbo].[users] ([usercode], [username], [accesslevel], [password]) VALUES (N'A00001', N'Admin', N'Admin', N'admin')
GO
INSERT [dbo].[users] ([usercode], [username], [accesslevel], [password]) VALUES (N'A00009', N'Legacy Cashier', N'User', N'legacy')
GO
INSERT [dbo].[itemmst] ([itemcode], [itemname], [packing], [ml], [companycode]) VALUES (N'I00091', N'O''Brien Rum', 12, 750, N'1'), (N'I00092', N'Old Monk', 12, 750, N'1')
GO
INSERT [dbo].[companymast] ([companycode], [companyname], [city]) VALUES (N'1', N'ABM TECHNOMATRIX (P) LTD.', N'KOLKATA')
GO
INSERT [dbo].[yearmst] ([yearcode], [yearrange], [stdate], [companycode]) VALUES (N'1', N'2014-2014', CAST(N'2014-04-01T00:00:00.000' AS DateTime), N'1')
GO
INSERT [dbo].[no_such_table] ([x]) VALUES (N'1')
GO
`;

// ------------------------------------------------------------- parsers

test('parseSqlDump reads INSERTs, multi-row VALUES, escapes and CAST', () => {
  const { tables } = migration.parseSqlDump(DUMP);
  assert.deepEqual(Object.keys(tables).sort(), ['companymast', 'itemmst', 'no_such_table', 'users', 'yearmst']);
  assert.equal(tables.users.length, 2);
  assert.deepEqual(tables.users[0], { usercode: 'A00001', username: 'Admin', accesslevel: 'Admin', password: 'admin' });
  assert.equal(tables.itemmst.length, 2);
  assert.equal(tables.itemmst[0].itemname, "O'Brien Rum");
  assert.equal(tables.itemmst[0].ml, 750);
  assert.equal(tables.yearmst[0].stdate, '2014-04-01T00:00:00.000');
});

test('parseJsonBundle accepts a wrapper and a bare map', () => {
  const wrapped = migration.parseJsonBundle(JSON.stringify({ company: '1', year: '2024', tables: { users: [{ usercode: 'X' }] } }));
  assert.deepEqual(wrapped.tables.users, [{ usercode: 'X' }]);
  assert.equal(wrapped.company, '1');
  const bare = migration.parseJsonBundle(JSON.stringify({ itemmst: [{ itemcode: 'I1' }] }));
  assert.deepEqual(bare.tables.itemmst, [{ itemcode: 'I1' }]);
});

test('parseCsv honours quotes and empty cells', () => {
  const csv = 'itemcode,itemname,ml\nI00020,"Rum, Dark",750\nI00021,Plain,\n';
  const { tables } = migration.parseCsv(csv, 'itemmst');
  assert.equal(tables.itemmst.length, 2);
  assert.equal(tables.itemmst[0].itemname, 'Rum, Dark');
  assert.equal(tables.itemmst[1].ml, null);
});

test('parseDirectory reads per-table CSV files', () => {
  const dir = mkdtempSync(join(tmpdir(), 'madhushala-csv-'));
  try {
    writeFileSync(join(dir, 'categorymst.csv'), 'categorycode,categoryname,companycode\nC00009,Test Cat,1\n');
    const { tables } = migration.parseDirectory(dir);
    assert.equal(tables.categorymst.length, 1);
    assert.equal(tables.categorymst[0].categoryname, 'Test Cat');
  } finally { rmSync(dir, { recursive: true, force: true }); }
});

test('parseSqlDump handles the repository legacy dml.sql fixture', () => {
  const txt = readFileSync(new URL('../../../dev/db/dml.sql', import.meta.url), 'utf8');
  const { tables } = migration.parseSqlDump(txt);
  assert.ok(tables.users && tables.users.length === 1);
  assert.equal(tables.users[0].password, 'admin');
  assert.equal(tables.companymast[0].companyname, 'ABM TECHNOMATRIX (P) LTD.');
});

// ------------------------------------------------------------- analyze

test('analyze is a dry run that classifies inserts, updates and skips', () => {
  const { db, dir } = freshDb();
  try {
    // A00001 already ships as admin, so it is an update; A00009 is new.
    const res = migration.analyze(db, { source: 'sql', content: DUMP, company: COMPANY, year: YEAR, filename: 'legacy.sql' });
    assert.equal(res.mode, 'ANALYZE');
    const users = res.tables.find((t) => t.table === 'users');
    assert.equal(users.inserts, 1);
    assert.equal(users.updates, 1);
    const items = res.tables.find((t) => t.table === 'itemmst');
    assert.equal(items.inserts, 2);
    assert.equal(res.skippedTables.length, 1);
    assert.equal(res.skippedTables[0].table, 'no_such_table');
    // nothing written yet
    assert.equal(get(db, 'SELECT COUNT(*) AS n FROM itemmst WHERE itemcode=?', ['I00091']).n, 0);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('analyze reports source columns the target table does not have', () => {
  const { db, dir } = freshDb();
  try {
    const json = JSON.stringify({ tables: { categorymst: [{ companycode: '1', categorycode: 'C00077', categoryname: 'Sparkling', bogus_column: 'x' }] } });
    const res = migration.analyze(db, { source: 'json', content: json, company: COMPANY, year: YEAR });
    const cat = res.tables.find((t) => t.table === 'categorymst');
    assert.deepEqual(cat.unknownColumns, ['bogus_column']);
    assert.equal(cat.inserts, 1);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

// ------------------------------------------------------------- apply

test('apply writes rows and defaults the tenant columns', () => {
  const { db, dir } = freshDb();
  try {
    // itemmst row with no companycode: the importer fills it from the request.
    const json = JSON.stringify({ tables: { categorymst: [{ categorycode: 'C00078', categoryname: 'Liqueur' }] } });
    const res = migration.apply(db, { source: 'json', content: json, company: COMPANY, year: YEAR });
    assert.equal(res.status, 'APPLIED');
    assert.equal(res.totals.inserted, 1);
    const row = get(db, 'SELECT * FROM categorymst WHERE categorycode=?', ['C00078']);
    assert.equal(row.companycode, '1');
    assert.equal(row.categoryname, 'Liqueur');
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('apply is idempotent - a second run updates instead of duplicating', () => {
  const { db, dir } = freshDb();
  try {
    const first = migration.apply(db, { source: 'sql', content: DUMP, company: COMPANY, year: YEAR });
    assert.equal(first.totals.inserted, 4); // users A00009, 2 items, yearmst '1'
    assert.equal(first.totals.updated, 2);  // seeded users A00001 and companymast '1'
    const second = migration.apply(db, { source: 'sql', content: DUMP, company: COMPANY, year: YEAR });
    assert.equal(second.totals.inserted, 0);
    assert.equal(second.totals.updated, 6);
    assert.equal(get(db, 'SELECT COUNT(*) AS n FROM itemmst WHERE itemcode IN (?,?)', ['I00091', 'I00092']).n, 2);
    assert.equal(get(db, 'SELECT COUNT(*) AS n FROM users WHERE usercode=?', ['A00009']).n, 1);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('apply coerces text values into numeric columns', () => {
  const { db, dir } = freshDb();
  try {
    const csv = 'categorycode,categoryname,companycode\nC00079,CSV Cat,1\n';
    const res = migration.apply(db, { source: 'csv', table: 'categorymst', content: csv, company: COMPANY, year: YEAR });
    assert.equal(res.totals.inserted, 1);
    const row = get(db, 'SELECT * FROM categorymst WHERE categorycode=?', ['C00079']);
    assert.equal(row.categorycode, 'C00079');
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('apply imports a CSV folder from disk (legacy directory source)', () => {
  const { db, dir } = freshDb();
  const folder = mkdtempSync(join(tmpdir(), 'madhushala-legacy-'));
  try {
    writeFileSync(join(folder, 'groupmst.csv'), 'companycode,groupcode,groupname\n1,G00009,Test Group\n');
    const res = migration.apply(db, { source: 'legacy', path: folder, company: COMPANY, year: YEAR });
    assert.equal(res.totals.inserted, 1);
    assert.equal(get(db, 'SELECT groupname FROM groupmst WHERE groupcode=?', ['G00009']).groupname, 'Test Group');
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); rmSync(folder, { recursive: true, force: true }); }
});

// ------------------------------------------------------------- reporting

test('history and batch detail record every run', () => {
  const { db, dir } = freshDb();
  try {
    migration.analyze(db, { source: 'sql', content: DUMP, company: COMPANY, year: YEAR, filename: 'legacy.sql' });
    const applied = migration.apply(db, { source: 'sql', content: DUMP, company: COMPANY, year: YEAR, filename: 'legacy.sql' });
    const hist = migration.history(db, COMPANY);
    assert.equal(hist.length, 2);
    assert.equal(hist[0].mode, 'APPLY');
    assert.equal(hist[0].status, 'APPLIED');
    const detail = migration.batchDetail(db, COMPANY, applied.batchId);
    assert.ok(detail.batch);
    assert.ok(Array.isArray(detail.detail.tables));
    assert.ok(detail.detail.tables.some((t) => t.table === 'users'));
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('tablesInfo flags known and unknown tables', () => {
  const { db, dir } = freshDb();
  try {
    const info = migration.tablesInfo(db);
    const byName = Object.fromEntries(info.map((t) => [t.table, t.present]));
    assert.equal(byName.itemmst, true);
    assert.equal(byName.users, true);
    assert.equal(byName.salesbillmain, true);
    assert.equal(info.length, migration.MIGRATABLE.length);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});

test('exportSqlScript mentions the migratable tables and the importer flow', () => {
  const script = migration.exportSqlScript();
  assert.match(script, /Data Migration/);
  assert.match(script, /itemmst/);
  assert.match(script, /INSERT \[dbo\]\./);
});

// ------------------------------------------------------------- transactions

test('apply migrates a purchase header and detail together', () => {
  const { db, dir } = freshDb();
  try {
    const dump = `
INSERT [dbo].[purchasemain] ([companycode],[yearcode],[trnid],[trnno],[trndate],[ptype],[totamount],[tottaxOth],[totnetamt])
VALUES (N'1', N'2024', 9, N'77', N'2024-04-05', N'PURCHASE', 500, 0, 500)
GO
INSERT [dbo].[purchasedetail] ([companycode],[yearcode],[trnid],[slno],[itemcode],[batchno],[itemrate],[itemquantity],[trnno],[trndate])
VALUES (N'1', N'2024', 9, 1, N'I00001', N'B1', 100, 5, N'77', N'2024-04-05')
GO
`;
    const res = migration.apply(db, { source: 'sql', content: dump, company: COMPANY, year: YEAR });
    assert.equal(res.totals.inserted, 2);
    const head = get(db, 'SELECT * FROM purchasemain WHERE trnno=?', ['77']);
    assert.equal(Number(head.totamount), 500);
    const line = get(db, 'SELECT * FROM purchasedetail WHERE trnno=?', ['77']);
    assert.equal(line.itemcode, 'I00001');
    assert.equal(Number(line.itemquantity), 5);
  } finally { db.close(); rmSync(dir, { recursive: true, force: true }); }
});
