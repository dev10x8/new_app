# AGENTS.md — repository knowledge

## What this repository is

**Madhushala Pro** — a bar / liquor retail & wholesale POS and management
system. The original app is a Windows .NET Framework 4.0 VB.NET WinForms
application backed by Microsoft SQL Server (`barmanager` DB). It is shipped in
this repo as binaries under `Debug/` (Git LFS pointers) and `.dll` files.

## Layout

| Path | Contents |
| --- | --- |
| `Debug/` | Original app binaries and `Resources/barmanager.mdf` (+ `_log.ldf`) |
| `decode/` | Decompiled sources of every shipped `.dll`/`.exe` |
| `PRODUCT_FEATURES.md` | Menus, submenus and end-to-end business flows |
| `dev/` | Developer guide, decompile scripts, Docker toolchain, DB scripts |
| `dev/db/` | Authoritative DDL/DML/model generated from `barmanager.mdf` |
| `new_code/` | **Rebuild** on Node.js + SQLite with a TUI frontend |

## Key facts

- The admin PIN lives in `users.password` in **plaintext**. The shipped admin is
  `A00001` / `admin`; access level `Admin` grants full rights.
- `dev/db/ddl.sql` (45 tables, 24 views, 3 procs, 11 functions) is generated from
  the original MDF via SMO and is the authoritative schema reference.
- SQL Server DDL is regenerated with a SMO-based C# scripter under `/tmp/smo/`
  (build needs internet; run needs the `madhushala-dev_default` Docker network
  with the `mssql` container).
- Bill numbers render as `FL` / `CS` / `NAI` + running number (e.g. `FL00001`).

## new_code (the rebuild)

- **Stack**: TUI (Node.js, raw ANSI) + backend (Node.js `node:http`) + SQLite
  (`node:sqlite`). **No external runtime dependencies** — Node >= 22.5 only.
- **Schema**: generated from `../dev/db/ddl.sql` by
  `backend/scripts/gen_schema.py` → `src/db/schema.sql` (45 tables). Do not
  hand-edit `schema.sql`; edit the generator. A second, hand-written layer
  `src/db/schema_ext.sql` adds 33 tables the original snapshot lacked
  (counter, customer, area, manufacturer, NAI, scheme/offer, excise item,
  report tagging, transfer sale/receive, notes, on-a/c, a/c replace, BRS, UPI,
  delivery, deleted-bill archive, message log, sync status, audit, PML, version).
  Total **78 tables**. Seeds live in `seed.sql` and `seed_ext.sql`.
- **Services** (`backend/src/services/`): `auth`, `masters` (generic CRUD; see
  `MASTERS` registry), `sales` (POS), `purchase`, `stock`, `vouchers`,
  `accounting` (double entry), `reports`, `reportCatalog` (113 declarative
  reports), `transactions` (transfer/notes/adjustments/fine/BRS/UPI/delivery),
  `admin` (parameters, new-year carry-forward, excise audit), `tools`
  (calculator, backup, tally, SMS, PML, sync, clean print).
- **Report catalog** is one declarative table in `reportCatalog.js`; the TUI
  report menu is generated from `/api/report-groups`, not hard-coded. Counts:
  sales 32, stock 20, purchase 10, accounts 14, MIS 12, breakage 2, transfer 2,
  excise 21.
- **Tests**: `cd new_code/backend && npm test` (31 tests across
  `test/services.test.js` and `test/features.test.js`, fresh temp DB each;
  includes a test that runs every catalog report).
- **Run**: `cd new_code && npm start` (backend + TUI). Login `A00001` / `admin`.

### Conventions in new_code

- All amounts round through `money()` (2 dp) in `services/helpers.js`.
- Every voucher posts through `postVoucher()` and must balance (Dr == Cr).
- Stock is computed on the fly in `stock.stockOnHand()` (opening + purchases −
  sales + returns − transfers out + transfers in − breakage); there is no
  materialised stock table.
- The TUI input layer treats end-of-input (piped/scripted stdin) as a clean exit.
