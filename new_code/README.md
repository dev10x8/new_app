# Madhushala Pro — new stack

A from-scratch rebuild of the Madhushala Pro bar / liquor POS & management
system on a modern, dependency-light stack:

| Layer    | Old stack (this repo)                    | New stack (`new_code/`)                 |
| -------- | ---------------------------------------- | --------------------------------------- |
| Frontend | VB.NET WinForms (MDI, TreeView menu)     | Tauri desktop app — Web UI + Rust shell |
| Backend  | VB.NET business logic + stored procedures| Node.js HTTP API, service modules       |
| Database | Microsoft SQL Server (`barmanager.mdf`)  | SQLite (`data/madhushala.db`)           |

The rebuild keeps the same business flows and features described in
`../PRODUCT_FEATURES.md` and the same data model as `../dev/db/ddl.sql`.

## Why this stack

- **SQLite** — a single file database, no server to install; the schema is
  generated directly from the original SQL Server DDL so the tables, keys and
  columns match the legacy model.
- **Node.js backend** — no external runtime dependencies. The data layer uses
  the built-in `node:sqlite` module and the API uses `node:http`. Only Node.js
  (>= 22.5) is required.
- **Tauri frontend (Web + Rust)** — the UI is ordinary HTML/CSS/JS (no
  framework) rendered in a native window by Tauri v2; the Rust layer only
  provides the window. The exact same UI also runs in a browser, which is how
  it is served by Docker. See [`tauri/`](tauri/README.md).

## Requirements

- Node.js >= 22.5.0 (for the built-in `node:sqlite` module).
- Python 3 (only to regenerate the SQLite schema from the SQL Server DDL).

## Quick start

```bash
cd new_code
npm start            # starts the backend, then opens the UI
```

Sign in with the shipped credentials:

```
User Code: A00001
Password:  admin
```

Run the pieces separately if you prefer:

```bash
npm run backend      # HTTP API on http://127.0.0.1:7100
npm run web          # web UI on http://127.0.0.1:8080 (expects the backend)
npm run tauri        # desktop app (Tauri) - see tauri/README.md
```

## Run with Docker

`docker-compose.yml` builds and runs both halves:

| Service | What it is | Default URL |
| --- | --- | --- |
| `backend` | Node.js + SQLite API | http://localhost:7100 |
| `frontend` | the web UI (the frontend of the Tauri desktop app) | http://localhost:8080 |

```bash
# from the repository root
docker compose -f new_code/docker-compose.yml up --build
```

Then open **http://localhost:8080** — that is the app UI in your browser. Log in
with `A00001` / `admin`. The API is on **http://localhost:7100**
(`/api/health`, `/api/report-groups`, ...). The web server proxies `/api` to the
backend, so there is no CORS to configure.

The SQLite file lives in the named volume `madhushala-data`, so data survives
`down`/`up`. To wipe and reseed:

```bash
docker compose -f new_code/docker-compose.yml down -v
```

Change the published ports with environment variables:

```bash
MADHUSHALA_PORT=8000 WEB_PORT=8001 \
  docker compose -f new_code/docker-compose.yml up --build
```

The images (`new_code/backend/Dockerfile`, `new_code/web/Dockerfile`) have no
third-party runtime dependencies — Node plus the source tree — so the builds are
small and fast. The backend image is also useful on its own if you only want the
API:

```bash
docker run --rm -p 7100:7100 madhushala-backend:latest
```

## Project layout

```
new_code/
├── backend/
│   ├── src/
│   │   ├── db/
│   │   │   ├── index.js      # SQLite open/seed + query helpers
│   │   │   ├── schema.sql    # generated from ../dev/db/ddl.sql (45 tables)
│   │   │   ├── schema_ext.sql# extra tables for the fuller feature set
│   │   │   ├── seed.sql      # original rows + bootstrap data
│   │   │   └── seed_ext.sql  # bootstrap rows for the extra tables
│   │   ├── services/         # business logic (see below)
│   │   ├── config.js
│   │   └── server.js         # HTTP API
│   ├── scripts/
│   │   ├── gen_schema.py     # SQL Server DDL -> SQLite schema
│   │   └── reset.js          # drop + recreate + reseed the database
│   └── test/
│       ├── services.test.js  # core service tests
│       ├── features.test.js  # extended feature + report catalog tests
│       ├── excise.test.js    # state excise sync (real local HTTP fake)
│       └── gst.test.js       # GST rate card, GSTR-1/3B, filing
├── web/                      # the UI (frontend of the Tauri desktop app)
│   ├── index.html            # single page shell
│   ├── app.js                # shell, router, session
│   ├── screens.js            # every screen / flow
│   ├── menu.js               # navigation tree (mirrors legacy frm_main)
│   ├── ui.js, api.js         # DOM helpers + HTTP client
│   ├── styles.css            # design system
│   ├── server.js             # static server + /api proxy (Docker + dev)
│   └── Dockerfile            # web image
├── tauri/                    # desktop shell (Web + Rust)
│   ├── package.json          # @tauri-apps/cli
│   └── src-tauri/            # Rust crate + tauri.conf.json + icons
├── docs/                     # user guides + screenshots
│   ├── build_user_guide.py   # generates the five role PDFs
│   ├── screenshots/          # captured by scripts/capture_screenshots.js
│   └── Madhushala_Pro_User_Guide*.pdf
├── data/madhushala.db        # created on first run
└── scripts/start.js          # backend + web launcher
```

The SQLite database ends up with **90 tables**: the 45 from the original DDL
(`schema.sql`) plus 45 added for the fuller feature set (`schema_ext.sql`,
including the GST tables `tbl_GstReturn`, `tbl_GstReturnLog` and
`tbl_GstConfig`).

## Business flows implemented

| Flow (PRODUCT_FEATURES.md §) | New code |
| --- | --- |
| Startup / login / logout (§3.1) | `services/auth.js`, UI login screen |
| Masters maintenance (§3.2) | `services/masters.js` (generic CRUD over every master table, incl. counter, customer, area, manufacturer, NAI, scheme/offer, tax scheme, excise item, report tagging) |
| POS / counter sale (§3.3) | `services/sales.js` (`createSale`, returns, delete, day close) |
| Purchase / goods receipt (§3.4) | `services/purchase.js` |
| Accounts & vouchers (§3.5) | `services/vouchers.js` + `services/accounting.js` (double entry) |
| Stock (§3.6) | `services/stock.js` (opening stock, transfer, breakage, on-hand) |
| Day close (§3.7) | `sales.dayClose` + `sales.salesSummary` |
| Reports (§2.5) | `services/reportCatalog.js` — 119 reports in 9 groups |
| Dash board (§2.7) | `reports.dashboard` + catalog-backed status screens |
| Transfer sale / receive | `services/transactions.js` |
| Debit / credit notes, on-a/c, a/c replace | `services/transactions.js` |
| Excise fine, BRS, QR/UPI, home delivery | `services/transactions.js` |
| New year carry-forward, parameters, excise audit | `services/admin.js` |
| Tools (calculator, backup, tally, SMS, PML, sync, clean print) | `services/tools.js` |
| GST (item rates, per-line capture, GSTR-1/3B, filing) | `services/gst.js` |
| Remote Tally Sync (§Tally) | `services/tally.js` |
| State excise compliance sync | `services/excise.js` |

Voucher numbering keeps the legacy format: bills render as `FL` / `CS` / `NAI`
plus a running number (e.g. `FL00001`).

## Menu-by-menu, screen-by-screen mapping

Old = the VB.NET WinForms app (`frm_main` TreeView → a form, e.g.
`frm_countersales`, writing to a SQL Server table). New = this UI screen and
the service / API behind it.

Status column values: **Full** = implemented end to end; **Partial** = present
but simplified or read-only; **Not yet** = not implemented (kept out of the UI
menu, or shown as an informational screen).

### Main

| Old menu → form (table) | New UI screen | New service / API | Status |
| --- | --- | --- | --- |
| Main → Log Out | returns to Login | UI logout | Full |
| Main → System Admin → `frm_sys_admin` | Main > System Admin | `admin.systemInfo` `/api/system-info` | Full |
| Main → Exit | exits the app | UI exit | Full |

### Admin

| Old menu → form (table) | New UI screen | New service / API | Status |
| --- | --- | --- | --- |
| Admin → Company → `frm_companyinfo` (`companymast`) | Admin > Company | `services/masters.js` `/api/masters/company` | Full |
| Admin → Parameters → `frm_companyparameter` (`companyparamtr`) | Admin > Parameters | `admin.getParameters/updateParameters` `/api/admin/parameters` | Full |
| Admin → User → `frm_userinfo` (`users`) | Admin > User | `services/auth.js` `/api/users` | Full |
| Admin → User Rights → `frm_userrights` (`userrights`) | Admin > User Rights | `auth.setRights` `/api/users/:code/rights` | Full |
| Admin → New Year → `frm_yearcreation` (`yearmst`) | Admin > New Year | `admin.createNewYear` `/api/admin/new-year` | Full |
| Admin → Excise Audit → `frm_ExcizeAudit` | Admin > Excise Audit | `admin.exciseAudit` `/api/admin/excise-audit` | Full |
| Admin → Deleted Bill → `DeleteSalesBillMain` | Admin > Deleted Bill | `sales.listDeletedBills` `/api/admin/deleted-bills` | Full |

### Masters

| Old menu → form (table) | New UI screen | New service / API | Status |
| --- | --- | --- | --- |
| Masters → A/C Main Group (`MasterAccountsMain` → `acgroupmst`) | Masters > A/C Main Group | `/api/masters/acgroup` | Full |
| Masters → A/C Sub Group (`acountname`) | Masters > A/C Sub Group | `/api/masters/acount` | Full |
| Masters → Ledger → `MasterAccountsLedger` (`ledger`) | Masters > Ledger | `/api/masters/ledger` | Full |
| Masters → Category (`categorymst`) | Masters > Category | `/api/masters/category` | Full |
| Masters → KFL (`kflmst`) | Masters > KFL | `/api/masters/kfl` | Full |
| Masters → Strength (`strength`) | Masters > Strength | `/api/masters/strength` | Full |
| Masters → Measure and Packing (`Itemgroupml`) | Masters > Measure and Packing | `/api/masters/itemgroupml` | Full |
| Masters → Item Master → `itemmst` | Masters > Item Master | `masters.listItemsDetailed` `/api/items` | Full |
| Masters → Sales Rate → `itemratemst` | Masters > Sales Rate | `/api/masters/rateinfo` | Full |
| Masters → Store Master → `frm_storage` (`storage`) | Masters > Store Master | `/api/masters/storage` | Full |
| Masters → Counter Master (`countermst`) | Masters > Counter Master | `/api/masters/counter` | Full |
| Masters → Customer Master (`customerDetails`) | Masters > Customer Master | `/api/masters/customer` | Full |
| Masters → Manufacturer (`ManufacturerMaster`) | Masters > Manufacturer | `/api/masters/manufacturer` | Full |
| Masters → Area (`tblArea`) | Masters > Area | `/api/masters/area` | Full |
| Masters → Tax Schemes → `frm_Taxscm` (`taxdetail`) | Masters > Tax Schemes | `/api/masters/taxscheme` | Full |
| Masters → Scheme / Offer (`tbl_scheme`, `offermst`) | Masters > Scheme / Offer | `/api/masters/scheme`, `.../offer` | Full |
| Masters → Excise Item masters (`ExciseItemMaster`) | Masters > Excise Item | `/api/masters/exciseitem` | Full |
| Masters → Excise Report Tagging (`reportTaggingMain`) | Masters > Excise Report Tagging | `/api/masters/reporttagmain` | Full |
| Masters → NAI masters (unit, main group, sub group, item) | Masters > NAI Unit / Group / Sub Group / Item | `/api/masters/naiunit`, `naigroup`, `naisubgroup`, `naiitem` | Full |
| Masters → Barcode Print | (no screen; barcodes are stored on `itemmst.barcode` and used at POS) | — | Partial |
| Masters → Item Merging / Counter Tagging | — | — | Not yet |

### Transactions

| Old menu → form (table) | New UI screen | New service / API | Status |
| --- | --- | --- | --- |
| Sale - POS → `frm_countersales` (`salesbillmain`/`detail`) | Transactions > Sale - POS | `services/sales.js` `/api/sales` | Full |
| Purchase → `frm_purchase` (`purchasemain`/`detail`) | Transactions > Purchase | `services/purchase.js` `/api/purchases` | Full |
| Counter Sale / Transfer Sale → `frm_countersales` | Transactions > Counter / Transfer Sale | `transactions.createTransferSale` `/api/transfer-sales` | Full |
| Transfer Receive | Transactions > Transfer Receive | `transactions.createTransferReceive` `/api/transfer-receives` | Full |
| Payment Entry → `frm_conpayrec_vch` (`payment_main`) | Transactions > Payment Entry | `vouchers.createPaymentReceipt` `/api/vouchers` | Full |
| Receipt Entry → `frm_receipt_voucher` (`receipt_main`) | Transactions > Receipt Entry | `vouchers.createPaymentReceipt` `/api/vouchers` | Full |
| Contra Entry → `frm_contra_entry` | Transactions > Contra Entry | `vouchers.createContra` `/api/vouchers` | Full |
| Journal Entry → `frm_journal_vch` (`journaldetail`) | Transactions > Journal Entry | `vouchers.createJournal` `/api/vouchers` | Full |
| Debit Note / Credit Note | Transactions > Debit Note / Credit Note | `transactions.createNote` `/api/notes` | Full |
| On A/C Adjustment | Transactions > On A/C Adjustment | `transactions.createOnAccountAdjustment` `/api/on-account` | Full |
| A/C Replace | Transactions > A/C Replace | `transactions.createAccountReplace` `/api/account-replace` | Full |
| Excise Fine → `frm_fine` (`fine_details`) | Transactions > Excise Fine | `transactions.createFine` `/api/fines` | Full |
| Opening Stock → `openingStockDetail` | Transactions > Opening Stock | `stock.addOpeningStock` `/api/opening-stock` | Full |
| Stock Transfer → `frm_stocktransfer` (`stk_transfer_main`) | Transactions > Stock Transfer | `stock.stockTransfer` `/api/stock-transfer` | Full |
| Breakage → `frm_breakageentry` (`breakagemain`) | Transactions > Breakage | `stock.addBreakage` `/api/breakage` | Full |
| BRS Entry → `frm_reconciliation` | Transactions > BRS Entry | `transactions.addReconciliation/clearReconciliation` `/api/reconciliation` | Full |
| QRCode Payment → `frm_qrorder` (`tbl_UPIRespose`) | Transactions > QRCode / UPI Payment | `transactions.recordUpi` `/api/upi` | Full |
| Delivery → `frmDelivery` (`delivery_orders`) | Transactions > Home Delivery | `transactions.createDelivery/setDeliveryStatus` `/api/deliveries` | Full |

### Reports

| Old group (PRODUCT_FEATURES.md §2.5) | New UI screen | New service / API | Status |
| --- | --- | --- | --- |
| Sales Reports (32) | Reports > Sales Reports | `reportCatalog.js` (32 keys) `/api/report-catalog/:key` | Full |
| Stock Reports (20) | Reports > Stock Reports | `reportCatalog.js` (20 keys) | Full |
| Purchase Reports (10) | Reports > Purchase Reports | `reportCatalog.js` (10 keys) | Full |
| Accounts Reports (14) | Reports > Accounts Reports | `reportCatalog.js` (14 keys) | Full |
| MIS Reports (12) | Reports > MIS Reports | `reportCatalog.js` (12 keys) | Full |
| Breakage Reports (2) | Reports > Breakage Reports | `reportCatalog.js` (2 keys) | Full |
| Stock Transfer Reports (2) | Reports > Stock Transfer Reports | `reportCatalog.js` (2 keys) | Full |
| Excise Reports (21) | Reports > Excise Reports | `reportCatalog.js` (21 keys) | Full |
| GST Reports (6) | Reports > GST Reports | `reportCatalog.js` (6 keys) `/api/report-catalog/gst.*` | Full |

The catalog holds **119 reports** and is driven from a single declarative table
(`services/reportCatalog.js`), so the report menu is generated from the API
(`/api/report-groups`) rather than hard-coded.

### Dash Board, Tools, Help, other top-level

| Old menu → form (table) | New UI screen | New service / API | Status |
| --- | --- | --- | --- |
| Dash Board → Sales Status | Dash Board > Sales Status | `reports.dashboard` `/api/dashboard` | Full |
| Dash Board → Stock Status | Dash Board > Stock Status | `stock.items_closing` | Full |
| Dash Board → Purchase Status | Dash Board > Purchase Status | `purchase.bill_detail` | Full |
| Dash Board → Scheme / Spirit-Beer / CS figures | Dash Board > Scheme / Spirit-Beer / CS Figures | `mis.scheme`, `stock.value_closing` | Partial |
| Tools → Calculator | Tools > Calculator | `tools.calculate` `/api/tools/calc` | Full |
| Tools → Change Password | Tools > Change Password | `auth.resetPin` `/api/users/:code/reset-pin` | Full |
| Tools → Data Back-Up (`backup()`) | Tools > Data Back-Up | `tools.backupDatabase` `/api/tools/backup` | Full |
| Tools → Data Restore | Tools > Data Restore (guided) | copy backup over `data/madhushala.db` | Partial |
| Tools → Shortcut Panel | Tools > Shortcut Panel | Help screen | Full |
| Tools → Export To Tally → `frm_tally_export` | Tools > Export To Tally | `tools.tallyExport` `/api/tools/tally` | Full |
| Tools → Remote Tally Sync (connectionless twin-agent) | Tools > Remote Tally Sync | `tally.*` `/api/tally/*` | Full |
| Tools → WhatsApp / SMS (`frm_message`) | Tools > SMS / WhatsApp | `tools.sendMessage/listMessages` `/api/tools/messages` | Full |
| Tools → PML Print and Reports | Tools > PML Print and Reports | `tools.addPml/listPml` `/api/tools/pml` | Full |
| Tools → Clean Print Garbage | Tools > Clean Print Garbage | `tools.cleanPrintGarbage` `/api/tools/clean-print` | Full |
| Tools → Data Sync | Tools > Data Sync | `tools.dataSync/syncStatus` `/api/tools/sync` | Full |
| Tools → Excise Sync (state portal) | Tools > Excise Sync | `excise.*` `/api/excise/*` | Full |
| Tools → GST / Returns (rates, GSTR-1/3B, filing) | Tools > GST / Returns | `gst.*` `/api/gst/*` | Full |
| Help → About Madhushala | Help > About Madhushala | `admin.systemInfo` | Full |
| Help → Ask for Help | Help > Ask for Help | Help screen | Full |
| Help → List of Updates | Help > List of Updates | `admin.systemInfo` | Full |

## Database

The SQLite schema is **generated** from the authoritative SQL Server DDL so it
stays faithful to the original:

```bash
npm run gen-schema   # reads ../dev/db/ddl.sql, writes src/db/schema.sql
```

Reset (drop, recreate, reseed) the database at any time:

```bash
npm run reset-db
```

Data lives in `data/madhushala.db`. Delete the file to start fresh; the next
backend start recreates and seeds it.

## API

The backend exposes a small JSON API (`{ ok, data }` / `{ ok, error }`):

| Method | Path | Purpose |
| --- | --- | --- |
| GET  | `/api/health` | liveness |
| POST | `/api/login` | authenticate (`{usercode,password}`) |
| GET/POST/PUT/DELETE | `/api/masters/:name` | master CRUD |
| GET  | `/api/items`, `/api/items/find?term=` | item lookup (POS scan) |
| GET/POST/DELETE | `/api/sales` | POS sales |
| POST | `/api/day-close` | day close |
| GET/POST | `/api/purchases` | purchases |
| GET/POST | `/api/stock`, `/api/opening-stock`, `/api/stock-transfer`, `/api/breakage` | stock |
| GET/POST | `/api/vouchers` | payment / receipt / contra / journal |
| GET  | `/api/reports/:key` | reports |
| GET  | `/api/report-groups` | report catalog, grouped |
| GET  | `/api/report-catalog/:key` | run a catalog report |
| GET  | `/api/dashboard` | dashboard figures |
| POST | `/api/users/:usercode/reset-pin` | reset a user PIN |
| GET/PUT | `/api/admin/parameters` | company parameters |
| GET/POST | `/api/admin/years`, `/api/admin/new-year` | years / carry-forward |
| GET  | `/api/admin/excise-audit`, `/api/admin/deleted-bills` | admin views |
| GET/POST | `/api/transfer-sales`, `/api/transfer-receives` | shop transfers |
| GET/POST | `/api/notes` | debit / credit notes |
| POST | `/api/on-account`, `/api/account-replace` | adjustments |
| GET/POST | `/api/fines` | excise fines |
| GET/POST | `/api/reconciliation`, `/api/reconciliation/:slno/clear` | BRS |
| GET/POST | `/api/upi` | QR / UPI payments |
| GET/POST/PUT | `/api/deliveries` | home delivery |
| GET  | `/api/system-info` | version + deployment flags |
| POST | `/api/tools/calc`, `/api/tools/backup`, `/api/tools/clean-print` | tools |
| GET  | `/api/tools/tally`, `/api/tools/messages`, `/api/tools/sync-status`, `/api/tools/pml` | tools |
| POST | `/api/tools/messages`, `/api/tools/sync`, `/api/tools/pml` | tools |
| GET/PUT | `/api/tally/config`, `/api/tally/mapping`, `/api/tally/inbound` | remote Tally sync setup |
| POST | `/api/tally/token`, `/api/tally/enqueue`, `/api/tally/retry`, `/api/tally/mapping`, `/api/tally/inbound/apply` | remote Tally sync actions |
| GET  | `/api/tally/queue`, `/api/tally/summary`, `/api/tally/xml`, `/api/tally/report`, `/api/tally/logs` | remote Tally sync views |
| GET  | `/api/tally/agent/pull?token=` | shop agent pulls pending voucher XML |
| POST | `/api/tally/agent/ack`, `/api/tally/inbound` | shop agent acks delivery / receives masters |

## Tests

```bash
npm test
```

60 tests (four files) cover seeding, login, PIN reset, purchases, POS sales,
stock movement (transfer, breakage, returns), day close, vouchers, reports and
master CRUD, plus the extended features: transfer sale/receive, debit/credit
notes, on-a/c and a/c replace, excise fine, BRS, UPI, delivery, deleted-bill
archiving, new-year carry-forward, parameters, excise audit, tools (calculator,
messages, sync, PML, clean print, backup), remote Tally sync (auto-enqueue,
voucher XML, ack, retry, inbound masters, token guard, sync report), the state
excise sync (driven against a real local HTTP fake of the portal API), GST
(item rate card, per-line split, GSTR-1/3B, filing), and that **all 119 catalog
reports execute**. Every test uses a fresh temporary database.

## Forgot the admin PIN?

The admin PIN lives in the `users` table (plaintext, as in the legacy app).
Reset it from the UI (**Tools → Change Password** or **Admin → User → Reset
PIN**) or directly:

```bash
curl -X POST localhost:7100/api/users/A00001/reset-pin \
  -H 'content-type: application/json' -d '{"pin":"1234"}'
```

## GST

GST is captured on the item master and applied at billing, then aggregated into
GSTR-1 / GSTR-3B returns that can be generated, inspected as JSON and filed.

**Item rates** live on both `itemmst` (alcohol) and `naiItemMaster` (non-alcohol),
mirroring the legacy NAI item master: `cgst`, `sgst`, `igst`, `cess`, `addcess`,
`baseprice`, a `gsttype` flag and the ten input/output tax ledgers. `gsttype`
follows the legacy meaning: **0 = exclusive** (GST is added on top of the line
amount) and **1 = inclusive** (GST is backed out of the amount). Intra-state
lines carry CGST+SGST, inter-state lines carry IGST.

**At billing** each sale line stores `taxable`, `cgst`, `sgst`, `igst`, `cess`,
`addcess` and `gstp`; the bill header stores the total GST in `igst`. Purchases
store the same split plus the input-tax ledger heads (`CgstInptLdgr`, ...) used
for the input tax credit, and the supplier GSTIN is stored on `purchasemain`.

**Returns** are built per `YYYY-MM` period into `tbl_GstReturn` (status DRAFT →
GENERATED → FILED) with an append-only `tbl_GstReturnLog` audit trail. GSTR-1 is
invoice-wise outward supplies; GSTR-3B is the summary by rate plus ITC and net
payable. "Filing" is an explicit offline step that stamps an ARN; wire
`gst.fileReturn` to a GSP/GSTN endpoint for a live e-filing.

| Concern | Where |
| --- | --- |
| Rate card, line split, returns, filing | `backend/src/services/gst.js` |
| Per-line capture at billing | `sales.js`, `purchase.js` |
| Tax invoice (company + party GSTIN) | `gst.taxInvoice` `/api/gst/invoice/:trnno` |
| Screen | Tools > GST / Returns (route `gst`) |
| Reports | Reports > GST Reports (`gst.*` catalog keys) |

## User guides

`docs/` holds five generated PDF guides, one content base rendered per audience.
They cover the full menu, including the GST / Returns screen and the GST Reports
group, with a screenshot and a legacy-mapping table for every flow.

| Guide | Audience |
| --- | --- |
| `Madhushala_Pro_User_Guide.pdf` | Admin and User flows (combined) |
| `Madhushala_Pro_User_Guide_POS_User.pdf` | Counter operators / cashiers |
| `Madhushala_Pro_User_Guide_Bar_User.pdf` | Bar staff / bartenders |
| `Madhushala_Pro_User_Guide_Accountant.pdf` | Accountants / back office |
| `Madhushala_Pro_User_Guide_Admin.pdf` | Administrators |

Rebuild them (needs the screenshots first):

```bash
npm run screenshots     # capture docs/screenshots/*.png (app must be running)
npm run guides          # or: python3 docs/build_user_guide.py [all|pos|bar|accountant|admin]
```

`npm run guides` needs `reportlab` and `Pillow` (`pip install reportlab pillow`).

## Notes & scope

- Almost every legacy menu leaf is now implemented end to end (see the mapping
  above). The few remaining gaps are item merging, counter tagging, barcode
  printing and the full excise form; the rest of the excise workflow is covered
  by the Excise reports, Excise fine and excise audit screens.
- The SQLite schema is the original SQL Server model (`schema.sql`) plus an
  additive layer (`schema_ext.sql`) for tables the original snapshot did not
  include. Original table and column names are preserved so the two can be
  compared directly.
- Passwords remain plaintext for behavioural parity with the original system.
  This is a legacy design carried over deliberately; hashing is recommended
  before any real deployment.
