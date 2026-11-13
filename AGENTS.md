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
| `new_code/` | **Rebuild** on Node.js + SQLite with a Tauri (Web + Rust) frontend |
| `new_code_portal/` | **Customer Portal** web app (Node.js + SQLite): onboarding, plans/modules, machine-bound licence issuance |

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

- **Stack**: Tauri (Web + Rust) UI + backend (Node.js `node:http`) + SQLite
  (`node:sqlite`). **No external runtime dependencies** — Node >= 22.5 only.
- **Schema**: generated from `../dev/db/ddl.sql` by
  `backend/scripts/gen_schema.py` → `src/db/schema.sql` (45 tables). Do not
  hand-edit `schema.sql`; edit the generator. A second, hand-written layer
  `src/db/schema_ext.sql` adds the tables the original snapshot lacked
  (counter, customer, area, manufacturer, NAI, scheme/offer, excise item,
  report tagging, transfer sale/receive, notes, on-a/c, a/c replace, BRS, UPI,
  delivery, deleted-bill archive, message log, sync status, audit, PML, version,
  remote Tally sync, state excise sync, Form M-22, inventory requisitions,
  **GST** — item GST rate columns, GST columns on sales/purchase lines,
  `tbl_GstReturn`/`tbl_GstReturnLog`/`tbl_GstConfig`, and the legacy-migration
  `tbl_MigrationBatch`/`tbl_MigrationLog`). It is applied on every
  `openDatabase` (idempotent
  `CREATE TABLE IF NOT EXISTS`; `ALTER ADD COLUMN` re-runs are ignored).
  Total **96 tables** (45 + 51; SQLite adds its internal `sqlite_sequence`).
  Seeds live in `seed.sql` and `seed_ext.sql`.
- **Services** (`backend/src/services/`): `auth`, `masters` (generic CRUD; see
  `MASTERS` registry), `sales` (POS), `purchase`, `stock`, `vouchers`,
  `accounting` (double entry), `reports`, `reportCatalog` (declarative
  reports), `transactions` (transfer/notes/adjustments/fine/BRS/UPI/delivery),
  `admin` (parameters, new-year carry-forward, excise audit), `tools`
  (calculator, backup, tally, SMS, PML, sync, clean print), `tally`
  (remote Tally Prime twin-agent sync + live HTTP/XML push), `excise`
  (state excise compliance sync + Form M-22), `inventory` (dashboard,
  low-stock alerts, auto-requisitions), `m22` (Form M-22 brand register),
  `gst`, and `migration` (legacy data import — see
  `new_code/docs/LEGACY_DATA_MIGRATION.md`).
- **Report catalog** is one declarative table in `reportCatalog.js`; the
  report menu is generated from `/api/report-groups`, not hard-coded. Counts:
  sales 32, stock 20, purchase 10, accounts 14, MIS 12, breakage 2, transfer 2,
  excise 22, GST 6.
- **Tests**: `cd new_code/backend && npm test` (126 tests across
  `test/services.test.js`, `test/features.test.js`, `test/excise.test.js`,
  `test/gst.test.js`, `test/inventory.test.js`, `test/m22.test.js`,
  `test/tallylive.test.js`, `test/migration.test.js`, `test/clock.test.js`,
  `test/imports.test.js` and `test/cors.test.js`,
  fresh temp DB each; includes a test that runs every catalog report,
  excise tests that drive a **real local HTTP fake** of the state excise API,
  a CORS test that boots the real server and drives it with `fetch()`,
  and migration tests that parse the repository's own `dev/db/dml.sql` fixture —
  no mocks).
- **Run**: `cd new_code && npm start` (backend + web UI). Login `A00001` / `admin`.
  Extra scripts: `npm run seed-demo` (reset + load a rich demo dataset for
  screenshots/demos), `npm run reset-admin-pin [usercode] [pin]` (forgot-PIN
  recovery; PINs are plaintext in `users`), `npm run screenshots` (drive headless
  Chromium over CDP to capture `docs/screenshots/*.png`; needs the app running).
- **Forgot-PIN recovery (installed app)**: the packaged runtime ships
  `backend/scripts/` too, so a locked-out shop can reset without a source
  checkout. The script auto-finds the DB (`VANIJLIQUOR_DB` > `new_code/data` >
  `%LOCALAPPDATA%\com.vanijliquor.pro\vanijliquor.db`):
  `"C:\Program Files\VanijLiquor\runtime\node.exe" "C:\Program Files\VanijLiquor\runtime\backend\scripts\reset-pin.js" --user A00001 --pin 1234`
  (or drop `--user/--pin` for the A00001/1234 default). Non-destructive — only
  `users.password` changes.
- **User guides**: generated by `new_code/docs/build_user_guide.py` (reportlab +
  Pillow) from one content base into five PDFs — a combined `Madhushala_Pro_
  User_Guide.pdf` plus role guides for **POS User**, **Bar User**, **Accountant**
  and **Admin**. Each embeds the screenshots and, per flow, a legacy mapping
  table (WinForms form / SQL Server table -> rebuilt screen); the GST setup,
  return-filing and GST reports flows are included. Rebuild:
  `npm run guides` (or `python3 docs/build_user_guide.py [all|pos|bar|accountant|
  admin]`). It needs the screenshots, so run `npm run screenshots` first.
- **UI**: `web/` is a framework-free SPA (HTML/CSS/JS) served by `web/server.js`
  (static + `/api` proxy). It is also the frontend of the Tauri desktop app
  (`tauri/`), which reuses `web/` unchanged and only adds a Rust window.
  `web/api.js` points at `http://127.0.0.1:7100` when it detects the Tauri
  webview (`__TAURI_INTERNALS__`/`__TAURI__`, or the `tauri:`/`file:` protocol,
  or a `tauri.localhost` host); it also retries the first request while the
  bundled backend finishes starting. The backend sends CORS headers (including
  `access-control-allow-private-network`, which Chromium's Private Network
  Access check requires for the `tauri.localhost` -> loopback call) so the
  cross-origin webview can reach it.
- **Desktop packaging** (`tauri/`): the packaged app is self-contained. The
  `beforeBuildCommand` runs `tauri/scripts/stage_runtime.mjs`, which copies the
  running Node binary and the backend source into `src-tauri/runtime/`
  (git-ignored); `tauri.conf.json` bundles that as a resource. On launch the Rust
  shell (`src-tauri/src/lib.rs`) spawns `runtime/node runtime/backend/src/server.js`,
  waits for `127.0.0.1:7100`, and kills the child on exit. The DB lives in a
  local (non-roaming) per-user data dir — `%LOCALAPPDATA%` first, since SQLite
  on a roaming/network `%APPDATA%` can hang — and the shell tees the backend's
  stdout/stderr to `backend.log` there (also next to the exe and in `%TEMP%`),
  since a packaged window has no console. The `backend_diagnostics` command
  returns the data-dir path and log tail, which the login screen shows when the
  backend is unreachable. Note: Tauri resolves resources to a verbatim
  (`\\?\C:\...`) path; Node's CJS loader cannot parse that prefix (it truncates
  the entry script to `C:` and dies with `EISDIR: lstat 'C:'`), so the shell
  strips `\\?\` / `\\?\UNC\` from the node, server and DB paths before spawning.
  Windows installers (NSIS `.exe`,
  WiX `.msi`) are built by `.github/workflows/package-windows.yml` on a `v*` tag,
  renamed to `VanijLiquor_<version>_x64-setup.exe` / `_en-US.msi`, and attached to
  a GitHub release. The version must match `tauri.conf.json` and the portal's
  `PORTAL_APP_VERSION` / `PORTAL_RELEASE_TAG`.
- **Licensing (desktop side)**: the app verifies a licence **offline** against
  the vendor Ed25519 public key in `backend/src/license/vendor.js` (generated by
  the portal's `scripts/gen_demo_license.js`). `tbl_License` holds a single row
  with the signed `payload` + `signature` (what the app enforces) and a `sealed`
  copy: the same record encrypted to the vendor's RSA **public** key. The app can
  *write* the seal but not read it — only the portal, holding the matching
  private key (`PORTAL_SEAL_KEY`), can unseal it (`POST /api/seal/unseal`). So a
  licence left on a customer's disk is opaque to everyone but the vendor. Note
  the two primitives do different jobs: the **signature** gives unforgeability,
  the **seal** gives confidentiality. Because the app must enforce offline, it
  has to keep the *signed* payload readable locally; the seal is a separate
  vendor-readable copy, not the enforced one. On first run the app activates the
  bundled 90-day demo (relative term, so it always has 90 days from activation)
  and opens the Licence screen; writes are gated with HTTP 402 until a valid
  licence is present (`services/license.js` → `isBlocked`).
- **Docker**: `docker compose -f new_code/docker-compose.yml up --build` runs the
  backend (port 7100) and the web UI (port 8080); DB in the `madhushala-data`
  volume. Ports overridable with `MADHUSHALA_PORT` / `WEB_PORT`. No third-party
  runtime deps, so the images are just Node + source.

## new_code_portal (the customer portal)

- **What it is**: a separate **web app** (branded "VanijLiquor Customer Portal")
  for self-service customer onboarding and licensing. Node.js + `node:sqlite` +
  a framework-free web UI; same conventions as `new_code`, but no desktop shell.
- **Run**: `cd new_code_portal && npm start` → backend `127.0.0.1:7200`, web
  `:8081` (served locally at `http://127.0.0.1:8081`). Extra scripts:
  `npm run seed-demo`, `npm test` (50 tests), `npm run reset-db`,
  `npm run gen-license-key`, `npm run screenshots` (CDP capture into
  `docs/screenshots/`), `npm run guides` (build the user guide PDF with
  `docs/build_user_guide.py`; needs reportlab + Pillow).
- **Routes**: dashboard, businesses, business, licenses, onboard, plans,
  modules, issue, verify, download, key, audit (`web/menu.js` drives the sidebar).
- **Licensing**: Ed25519-signed `.vlic` files bound to a machine fingerprint
  (MAC, CPU id, disk serial, hostname). Issue a licence from a pasted
  `machine-fingerprint.json`; activation on a different machine is refused
  (`Licence is bound to a different machine`). Set `PORTAL_LICENSE_KEY` for
  production; otherwise a development signing key is used and the UI flags it.
  `GET /api/seal-key` returns the RSA public key the desktop uses to seal stored
  licences; `POST /api/seal/unseal` recovers them (portal-side only). Set
  `PORTAL_SEAL_KEY` (base64 PKCS#8 RSA private key) in production, else the dev
  key in `services/seal.js` is used.
- **Downloads**: the "Download App" screen is driven by `GET /api/downloads`
  (`backend/src/services/downloads.js`). It links to the Windows installers on a
  GitHub release (`PORTAL_APP_VERSION`, `PORTAL_RELEASE_REPO`,
  `PORTAL_RELEASE_TAG`, or `PORTAL_DOWNLOAD_BASE_URL` for a CDN) and to the
  licence-server endpoints (`PORTAL_LICENSE_SERVER_URL` / `PORTAL_PUBLIC_URL`).
  The installer filenames must match what `package-windows.yml` publishes.
- **User guide**: `docs/VanijLiquor_Customer_Portal_User_Guide.pdf`, generated
  by `docs/build_user_guide.py` from the app's own sources and screenshots in
  `docs/screenshots/` (captured by `scripts/capture_screenshots.js`).

### Conventions in new_code

- All amounts round through `money()` (2 dp) in `services/helpers.js`.
- Every voucher posts through `postVoucher()` and must balance (Dr == Cr).
- Stock is computed on the fly in `stock.stockOnHand()` (opening + purchases −
  sales + returns − transfers out + transfers in − breakage); there is no
  materialised stock table.
- The web UI is framework-free: one route table (`web/menu.js`) drives the
  sidebar; screens live in `web/screens.js`; there are no build-time deps.
