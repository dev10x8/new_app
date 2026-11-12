# AGENTS.md — new_code (the VanijLiquor rebuild)

Repository knowledge for the **new app**. For the repository as a whole (the
original WinForms app, `decode/`, `dev/db/` DDL) see the root `AGENTS.md`.

## What this is

**VanijLiquor** (formerly shipped as *Madhushala Pro*) — a bar / liquor retail &
wholesale POS and management system. This folder is a from-scratch rebuild of the
legacy Windows .NET VB.NET WinForms + SQL Server (`barmanager`) app.

- **Stack**: backend on Node.js `node:http` + SQLite (`node:sqlite`); frontend is
  a framework-free HTML/CSS/JS SPA that is also the frontend of the Tauri
  (Web + Rust) desktop shell. **No external runtime dependencies** — Node >= 22.5
  only (for `node:sqlite`).
- **One frontend, three hosts**: browser (`web/server.js`), Docker, and the Tauri
  desktop window all serve the same `web/` code; Tauri adds only a native window.
- The legacy app is still called *Madhushala Pro* where it is described as the
  original system, and its literal file paths (`decode/Madhushala Pro.exe/…`), the
  vendor URL `madhushalasoftware.com` and the `MadhushalaDataSync.dll` component
  name are intentionally **not** renamed.

## Layout

| Path | Contents |
| --- | --- |
| `backend/src/server.js` | HTTP router + all `/api` routes (~165 routes) |
| `backend/src/config.js` | Runtime config; DB path + env vars |
| `backend/src/db/` | `index.js` (open/migrate/seed), `schema.sql`, `schema_ext.sql`, `seed.sql`, `seed_ext.sql` |
| `backend/src/services/` | All business logic (one module per domain, see below) |
| `backend/src/util/tabular.js` | Loose Excel/CSV/JSON parsing used by imports |
| `backend/data/state-masters/` | Approved-brand lists (`ALL.json`, `OD.json`, `WB.json`) |
| `backend/test/` | `node --test` suites (111 tests, 9 files) |
| `web/` | SPA: `index.html`, `app.js` (shell/router), `menu.js` (sidebar), `screens.js` (every screen), `ui.js` (DOM helpers), `api.js` (fetch wrapper), `server.js` (static + `/api` proxy), `scripts/build_web.mjs` → `dist/` |
| `tauri/` | Tauri v2 desktop shell (Rust crate `vanijliquor`, `vanijliquor_lib`); reuses `web/` unchanged |
| `scripts/` | `start.js` (backend + web launcher), `seed_demo.js`, `reset_admin_pin.js`, `capture_screenshots.js` |
| `docs/` | Design/status docs + `build_user_guide.py` (generates the five PDFs) |
| `docker-compose.yml` | backend (7100) + frontend (8080); DB in the `vanijliquor-data` volume |

## Commands

Run from `new_code/` unless noted.

```bash
npm start                 # backend (127.0.0.1:7100) + web UI (127.0.0.1:8080), opens the browser
npm run backend           # backend only
npm run web               # web UI only (expects the backend)
npm run tauri             # desktop app (see tauri/README.md)
npm test                  # backend test suite (node --test)
npm run reset-db          # drop + reseed the SQLite file
npm run seed-demo         # reset + load a rich demo dataset (screenshots/demos)
npm run reset-admin-pin [usercode] [pin]   # forgot-PIN recovery (default A00001 1234)
npm run gen-schema        # regenerate backend/src/db/schema.sql from ../dev/db/ddl.sql (needs Python 3)
npm run screenshots       # capture docs/screenshots/*.png (needs the app running)
npm run guides            # build the five user-guide PDFs (needs reportlab + Pillow + screenshots)
```

Docker: `docker compose -f new_code/docker-compose.yml up --build`.

## Key facts

- **Default login**: user `A00001`, password `admin` (access level `Admin` = full
  rights). **PINs/passwords are stored in plaintext** in `users.password`, exactly
  as the legacy app stored them — so a forgotten admin PIN is a one-line update
  (`npm run reset-admin-pin`). Access rights live in the `userrights` table.
- **Database file**: `new_code/data/vanijliquor.db` (WAL mode). Override the path
  with `VANIJLIQUOR_DB`. On first open the DB is created and seeded; on later opens
  only additive migrations run. Env vars: `VANIJLIQUOR_DB`, `HOST`, `PORT`,
  `COMPANY`, `YEAR` (backend); `PORT`, `HOST`, `API_URL` (web).
- **Schema**: `schema.sql` (45 tables) is **generated** from `../dev/db/ddl.sql` by
  `backend/scripts/gen_schema.py` — do not hand-edit it; edit the generator. A
  second hand-written layer `schema_ext.sql` (56 tables) adds what the original
  snapshot lacked (GST, excise sync, Form M-22, migration batch/log, …). Total
  **101 tables**. `openDatabase()` applies `schema_ext.sql` idempotently on every
  open (`CREATE TABLE IF NOT EXISTS`; `ALTER ADD COLUMN` re-runs are ignored).
  Seeds: `seed.sql` + `seed_ext.sql`.
- **Services** (`backend/src/services/`): `auth`, `masters` (generic CRUD via the
  `MASTERS` registry), `sales` (POS), `purchase`, `stock`, `vouchers`,
  `accounting` (double entry), `reports`, `reportCatalog` (declarative reports),
  `transactions`, `admin`, `tools`, `tally` (remote Tally Prime twin-agent sync),
  `excise` (state excise sync + Form M-22), `inventory`, `m22`, `gst`,
  `migration` (legacy import), plus the newer `parcha` (wholesale-invoice / OCR
  import), `bulkimport` (any master Excel/CSV/JSON), `statemaster` and
  `landedcost`.
- **Report catalog** is one declarative table in `reportCatalog.js`; the report
  menu is generated from `GET /api/report-groups`, not hard-coded. Counts:
  sales 32, stock 20, purchase 12, accounts 14, MIS 12, breakage 2, transfer 2,
  excise 22, GST 6 — **122 total** across 9 groups.
- **Bill numbers** render as `FL` / `CS` / `NAI` + running number (e.g. `FL00001`).
- **Licensing is owned by the separate customer portal** (`../new_code_portal`),
  not this app. It mints an Ed25519-signed `.vlic` bound to a machine
  fingerprint. Licences carry three soft-enforcement tolerances baked into the
  signed payload — a **grace window** after expiry, an **offline limit** before
  the client drops to limited *educational* mode, and a **seat limit** — plus an
  audited **surrender** to move a licence to a new machine. The desktop app pins
  the vendor public key and verifies offline; it must never trust a key carried
  inside the licence file. See `../new_code_portal/README.md`.

## Conventions

- All amounts round through `money()` (2 dp) in `services/helpers.js`.
- Every voucher posts through `postVoucher()` and must balance (Dr == Cr).
- Stock is computed on the fly in `stock.stockOnHand()` (opening + purchases −
  sales + returns − transfers out + transfers in − breakage); there is **no**
  materialised stock table.
- The web UI is framework-free: `web/menu.js` is the single route table that
  drives the sidebar; screens live in `web/screens.js`; there are no build-time
  deps. After editing `web/*.js`/`web/index.html`, rebuild the bundle with
  `npm --prefix web run build` (writes `web/dist/`, which Tauri bundles).
- Keep branding consistent with **VanijLiquor** (app name, `VL` brand mark,
  `vanijliquor-*` image/volume/env names). Do not rename the literal legacy
  paths/URLs listed under "What this is".

## Tests

`cd backend && npm test` (`node --test`). Suites: `services`, `features`,
`imports`, `excise`, `gst`, `inventory`, `m22`, `tallylive`, `migration` — fresh
temp DB each. No mocks: excise/tally tests drive a real local HTTP fake, and the
migration tests parse the repository's own `dev/db/dml.sql` fixture.
