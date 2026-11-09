# Legacy data migration

Bring data across from the original **Madhushala Pro** (VB.NET WinForms + SQL
Server `barmanager`) into the rebuild (Node.js + SQLite), driven entirely from
the admin UI: **Tools → Data Migration**.

The importer never talks to SQL Server directly and never runs destructive
SQL. You export from the legacy machine to a portable file, load that file in
the new app, preview it, then apply.

## Where it lives

| Layer | File |
| --- | --- |
| Service | `backend/src/services/migration.js` |
| Routes | `backend/src/server.js` — `/api/migration/*` |
| UI | `web/screens.js` → `migrationScreen`, `web/menu.js` (Tools → Data Migration) |
| API client | `web/api.js` — `migration*` methods |
| Tracking tables | `backend/src/db/schema_ext.sql` — `tbl_MigrationBatch`, `tbl_MigrationLog` |
| Tests | `backend/test/migration.test.js` |

## The two-step flow

1. **Analyze (dry run)** — parses the source, compares every row against the
   target schema and reports, per table, how many rows would be *inserted*,
   *updated* or *failed*, which source columns are unknown (and therefore
   ignored) and which required columns had to be defaulted. Nothing is written.
2. **Apply** — re-parses the same source and writes it inside one transaction.
   Rows are **inserted**, or **updated in place** when their primary key already
   exists. No table is ever deleted or truncated. Rows that cannot be written
   are counted and their errors listed; the rest of the batch still commits.

Every run (analyze and apply) is recorded in `tbl_MigrationBatch` and shown in
the screen's *Migration history* table, so the import is auditable.

## Supported sources

| Source | What it is | How to produce it |
| --- | --- | --- |
| **SQL dump** | The T-SQL text `INSERT [dbo].[table] (...) VALUES (...)` statements | SQL Server Management Objects "script data", or `bcp` output — see the helper script below |
| **JSON bundle** | `{ "company": "1", "year": "2024", "tables": { "itemmst": [ { ... }, ... ] } }` | any exporter; a bare `{ "itemmst": [...] }` map also works |
| **CSV** | One table: a header row of column names, then rows | `bcp ... out table.csv -c -t,` |
| **Server folder** | A directory of `<table>.csv` (or `<table>.json`) files, or a `manifest.json` | export each table to a file in one folder on the server |

The **"Load export helper script"** button fills the box with a ready-to-run
PowerShell/bcp recipe that produces a compatible dump from the legacy server.

## What can be migrated

The `MIGRATABLE` list in `migration.js` is the allow-list, ordered
parent-first so masters load before the transactions that reference them:
company/year setup, users & rights, accounting masters, item masters, opening
balances, purchases, sales, breakage, transfers, payments/receipts/journals,
excise & reporting tables, and the extended masters the rebuild added (counter,
customer, area, manufacturer, NAI, scheme/offer, excise item).

Any table in the source that is **not** on this list — or that does not exist in
this database — is reported under *Skipped tables* and never applied.

## How a row is mapped

For each source row the importer:

1. Keeps only columns that exist in the target table; unknown columns are
   recorded and dropped.
2. Coerces values to the target column affinity (TEXT / INTEGER / REAL). T-SQL
   `N'...'`, `'...'`, `NULL`, numbers and `CAST(... AS ...)` are understood.
3. Fills `companycode` / `yearcode` from the running company/year when the
   source omits them.
4. Defaults any other `NOT NULL` column that has no value and no table default
   (`0` for numeric, `''` for text) and flags it as a *missing column*.
5. Decides insert vs. update from the primary key.

Idempotent: running the same export twice updates the same rows instead of
duplicating them.

## Notes and limits

- The login PIN is the plain-text `users.password`, so migrated users keep
  their existing PINs. The shipped admin (`A00001`) is updated in place.
- Money columns in the generated SQLite schema are `TEXT` (a property of the
  schema generator, not of the importer); the app writes and compares them the
  same way, so imported amounts behave identically.
- For very large dumps, run *Analyze* first: it tells you the row counts and any
  problematic tables before you commit.
- Backup first: **Tools → Data Back-Up** copies the SQLite file. A migration is
  additive (insert/update only), so a backup plus the history table is enough to
  reason about any change.
