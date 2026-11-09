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
| Reports (Sales / Stock / Purchase / Breakage / Stock Transfer / MIS / Excise / Accounts) | Implemented | `services/reportCatalog.js` — 120 reports in 9 groups |
| Settings | Implemented | Company / Parameters / Users |
| **AI Auto Purchase Import (Parcha Import)** | **Missing** | No OCR / AI. Only a read-only `sales.parcha` report; there is no bill-image → purchase-entry path |

---

## 2. Top feature strip

| Feature | Status | Where / notes |
| --- | --- | --- |
| Excise Management — auto registers & audit-ready reports | Implemented | 16 state adapters (WB, OD, UP, HR, MH, KA, RJ, GJ, TS, MP, AP, AS, KL, CG, PB, DL), split into direct-JSON **api** and portal-agent modes; 22 excise reports incl. Category / Brand registers and the **Form M-22** brand register (`services/m22.js`, route `form-m22`) |
| Tally Integration — automation & export | Implemented | Offline XML export + connectionless twin-agent remote sync + **live Tally Prime push** (`services/tally.js` `livePush`/`liveTest` over Tally's HTTP/XML server) |
| AI Auto Purchase Import | Missing | Same as §1 |
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

## 5. Summary

**Fully covered:** billing, purchases, stock transfer, breakage, the full report
catalog, settings, offline operation, the inventory dashboard with low-stock
alerting and auto-requisitions, the excise suite (16 state adapters, Form
M-22, audit-ready registers), Tally export, remote twin-agent sync and live
Tally Prime push, and admin-run legacy data migration (SQL dump / JSON / CSV /
server folder, dry-run + idempotent apply).

**Biggest true gaps (no code at all):**

1. AI Auto Purchase Import / Parcha OCR — the flagship "AI" claim
2. Camera AI (CCTV face recognition)
3. WhatsApp Business integration (live alerts) — currently only a message log
4. Hardware / payment connectors: Pine Labs, Marg ERP, Razorpay Dynamic QR, ESC/POS thermal print
5. Customer-facing / pole display
6. Cloud multi-device POS and bar / restaurant KOT billing

**Partial:** shift / drawer reconciliation, hourly analytics.

### Suggested next steps (cheapest high value first)

1. ESC/POS print path
2. Wire `sendMessage` to a real WhatsApp / SMS gateway
3. Shift-wise cash-drawer reconciliation

The AI OCR import and Camera AI are substantially larger builds.
