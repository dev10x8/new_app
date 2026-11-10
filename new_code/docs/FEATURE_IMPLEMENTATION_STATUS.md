# Feature implementation status

Comparison of the features advertised on **madhushalasoftware.com** (the `#workflow`
section plus the top feature strip and the other homepage claims) against what the
rebuild in `new_code` actually implements.

Legend: **Implemented** · **Partial** (skeleton or an adjacent feature only) ·
**Missing**.

Source page: <https://madhushalasoftware.com/#workflow> (reviewed 2026-11-08).

---

## 1. The `#workflow` section (core set)

| Feature on the page | Status | Where / notes |
| --- | --- | --- |
| Billing | Implemented | `services/sales.js` (POS, `FL` / `CS` / `NAI` numbering) |
| Inventory — Add Item, low-stock counts | Implemented | Inventory dashboard (`services/inventory.js`) with Total Items / Low Stock / Out of Stock counters, low-stock alerting against `itemmst.min_qnty`, and auto-raised purchase requisitions (`tbl_Requisition`); screen at route `inventory` |
| Purchases | Implemented | `services/purchase.js` (manual entry) |
| Stock Transfer | Implemented | `services/transactions.js`, stock transfer reports |
| Breakage Entry | Implemented | `services/stock.js` breakage + reports |
| Reports (Sales / Stock / Purchase / Breakage / Stock Transfer / MIS / Excise / Accounts) | Implemented | `services/reportCatalog.js` — 122 reports in 9 groups (incl. **Landed Cost & Margin** and **Item-wise Margin**) |
| Settings | Implemented | Company / Parameters / Users |
| **AI Auto Purchase Import (Parcha Import)** | **Implemented** | Route `parcha-import` + `services/parcha.js`: import a wholesale invoice from PDF / JSON / CSV / pasted text, match lines to the item master, post the purchase, compute **landing cost + margin** per line, optionally create missing items, raise selling prices from a target margin, and seed opening stock. OCR is pluggable (stub / local `tesseract` / external HTTP). Legacy had a state-format PDF import; this generalises it and closes its gaps |

---

## 2. Top feature strip

| Feature | Status | Where / notes |
| --- | --- | --- |
| Excise Management — auto registers & audit-ready reports | Implemented | 16 state adapters (WB, OD, UP, HR, MH, KA, RJ, GJ, TS, MP, AP, AS, KL, CG, PB, DL), split into direct-JSON **api** and portal-agent modes; 22 excise reports incl. Category / Brand registers and the **Form M-22** brand register (`services/m22.js`, route `form-m22`) |
| Tally Integration — automation & export | Implemented | Offline XML export + connectionless twin-agent remote sync + **live Tally Prime push** (`services/tally.js` `livePush`/`liveTest` over Tally's HTTP/XML server) |
| AI Auto Purchase Import | Implemented | Same as §1 (route `parcha-import`) |
| Stock Transfer / Breakage Entry / Reports | Implemented | As above |
| **WhatsApp — live bill alerts & daily summaries** | Partial | `tools.sendMessage` only writes to `tbl_MessageLog` (a documented "gateway integration point"). No WhatsApp Business API and no automatic per-bill / day-end sends |

---

## 3. Other homepage claims

| Claim | Status | Where / notes |
| --- | --- | --- |
| Multi-counter parallel billing | Implemented | Per-counter bill numbering |
| Dynamic customer-facing screen (UPI QR) | Missing | No customer / pole display at all |
| Cashier-wise shift closing + cash-drawer reconciliation | Partial | Day close + `sales.salesSummary` exist; no shift-wise / drawer reconciliation |
| Low-stock notifications + auto-purchase requisitions | Implemented | `services/inventory.js` low-stock alerting against `itemmst.min_qnty` and auto-raised draft requisitions (`tbl_Requisition`), surfaced on the Inventory screen |
| Brand-wise gross margin + hourly sales trends | Partial | Item margin & scheme / profit reports exist; no hourly breakdown |
| Day-end summaries via WhatsApp | Partial | Log only (see WhatsApp row above) |
| Tally Prime Sync | Implemented | Offline XML export + remote twin-agent sync + live HTTP/XML push to Tally Prime |
| Pine Labs Smart POS | Missing | No card-terminal integration |
| Marg ERP Connector | Missing | None |
| Razorpay Dynamic QR | Missing | No Razorpay integration (UPI is recorded manually via `QRCode / UPI Payment`) |
| 80mm ESC/POS thermal printers | Missing | No thermal / ESC/POS print path |
| **Camera AI** (CCTV face detection, employee name + ID on screen) | Missing | None |
| Next-Gen **Cloud-Based** POS (multi-device, cloud) | Partial | Offline-first local SQLite + a remote Tally sync agent; no cloud hosting / multi-device web |
| Offline-first billing | Implemented | Core design |
| State excise breadth (eAbgari / BEVCO, UP IESCMS, Haryana L-14A, …) | Implemented | 16 state adapters ship (WB, OD, UP, HR, MH, KA, RJ, GJ, TS, MP, AP, AS, KL, CG, PB, DL); more are added without touching billing |
| Bar / restaurant **KOT** billing | Missing | Legacy had `frm_KotBilltPrinting`; the rebuild has no KOT screen (only a "Kota" report) |
| Historical data migration | Implemented | Admin screen **Tools → Data Migration** (`services/migration.js`): import from the legacy SQL Server DB via SQL dump / JSON bundle / CSV / server folder, with a dry-run analyze step, idempotent insert-or-update apply, per-table reporting and an audit history (`tbl_MigrationBatch`). See `new_code/docs/LEGACY_DATA_MIGRATION.md` |

---

## 4. Already-known gaps

The rebuild's own scope notes (`new_code/README.md` → "Notes & scope") already flag:

- Item merging
- Counter tagging
- Barcode printing
- The full excise form

These are consistent with the rows above.

---

## 4a. Bulk Master Upload, state master list & pack sizes (legacy-parity additions)

The legacy app had these features only partially; the rebuild now has them in full.

Verification: the legacy behaviour was read from the decompiled binary
(`decode/Madhushala Pro.exe/Madhushala Pro.decompiled.cs`) — the PDF import lives
in `ImportAnyStatePDF_Click_1` / `ExtractAllTextFromPdf` / `PostPurchaseFromBevTables`
(the "First 'Parcha' Scan" claim), while the bulk-master button and any state-master
catalogue were searched for and are absent.

| Feature | Legacy | New build | Where |
| --- | --- | --- | --- |
| Bulk master upload (any master, Excel/CSV/JSON) | Partial — Excel only, **hidden button** (`btnImportFromExcel`), only for **offers** and **opening stock** | Implemented — route `bulk-import`, `services/bulkimport.js`: load **any** master or opening stock from `.xlsx` / `.csv` / `.json`, loose header matching, dry-run + apply, per-row error reporting, audit history (`tbl_BulkImportBatch`) | `services/bulkimport.js`, `util/tabular.js` (XLSX/CSV with Node built-ins) |
| Pre-configured **state master list** | Missing — only a per-state logo download | Implemented — route `state-master`, `services/statemaster.js`: bundled datasets (`data/state-masters/{WB,OD,ALL}.json`), load a state's approved brand/size/MRP list, then generate the matching item masters (dry-run + apply) | `services/statemaster.js`, `data/state-masters/*.json` |
| **Pack sizes** (Quart / Pint / Nip) | Missing — only report headers | Implemented — route `pack-sizes`, seeded `tbl_PackSize`, ML → pack-size naming used by the state master | `db/seed_ext.sql`, `services/statemaster.js` |
| **Landed cost + margin** | Partial — legacy computed a rough cost, no per-line landing cost or margin report | Implemented — `services/landedcost.js`: fold discounts, bill charges, tax and free (scheme) bottles into a true **cost per bottle**, then report margin vs selling price; set an item's price from a target margin. Route `landed-cost` + reports `purchase.landing_cost`, `purchase.item_margin` | `services/landedcost.js` |
| **Parcha import (wholesale invoice)** — the "First 'Parcha' Scan" claim | Present — `ImportAnyStatePDF_Click_1` / "Import from PDF": extract text, detect the state (WB / MP), post a purchase from the excise tables | Implemented — `services/parcha.js`: read a purchase invoice from **PDF** (Flate content streams), JSON, CSV or pasted text; match lines to the item master (code / barcode / name / **fuzzy name+ML**), create missing items, post the purchase with **landed cost**, raise prices from a target margin and seed opening stock — all with a dry-run. Route `parcha`, `services/parcha.js` | `services/parcha.js` |
| **OCR / document capture** (scan → invoice) | Present — text extraction only, no pluggable OCR | Implemented — pluggable provider: `stub` (pasted text), `tesseract` (local CLI) or an external HTTP OCR service, configurable per company; scanned image → text → parsed invoice. Route `parcha/ocr`, `parcha/ocr-config` | `services/parcha.js` (`ocrExtract`, `saveOcrConfig`) |

**What "landing cost + margin" means:** the *landing cost* is the real cost of one
sellable bottle once everything on the bill is spread over the bottles you can
actually sell — the item rate, less the line discount, plus a share of the bill's
other charges and tax, divided by the bill quantity **including free (scheme)
bottles** (free bottles lower the true cost per bottle). The *margin* is the
selling price minus that landing cost (the amount, and as a percentage of the
selling price). See `services/landedcost.js` (`computePurchase`, `applyMarginToItem`).

---

## 5. Summary

**Fully covered:** billing, purchases, stock transfer, breakage, the full report
catalog, settings, offline operation, the inventory dashboard with low-stock
alerting and auto-requisitions, the excise suite (16 state adapters, Form
M-22, audit-ready registers), Tally export, remote twin-agent sync and live
Tally Prime push, admin-run legacy data migration (SQL dump / JSON / CSV /
server folder, dry-run + idempotent apply), the **AI / OCR Parcha purchase
import**, **bulk master upload** for any master, the **pre-configured state
master list**, **pack sizes**, and **landed cost + margin** reporting.

**Biggest true gaps (no code at all):**

1. Camera AI (CCTV face recognition)
2. WhatsApp Business integration (live alerts) — currently only a message log
3. Hardware / payment connectors: Pine Labs, Marg ERP, Razorpay Dynamic QR, ESC/POS thermal print
4. Customer-facing / pole display
5. Cloud multi-device POS and bar / restaurant KOT billing

**Partial:** shift / drawer reconciliation, hourly analytics.

### Suggested next steps (cheapest high value first)

1. ESC/POS print path
2. Wire `sendMessage` to a real WhatsApp / SMS gateway
3. Shift-wise cash-drawer reconciliation

Camera AI is a substantially larger build. The Parcha OCR import ships with a
pluggable provider (stub / local `tesseract` / external HTTP) — pointing it at a
real OCR service is a configuration step, not a new build.
