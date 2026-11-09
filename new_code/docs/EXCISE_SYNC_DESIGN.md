# Excise Tax Compliance Sync — Design

Status: implemented in `new_code/` (backend + web).
Scope: how the legacy Windows app performs "automatic" state excise sync, what the
marketing site claims, and how the rebuild reproduces the real behaviour in the
Node.js + SQLite stack.

---

## 1. The marketing claim

Source: <https://madhushalasoftware.com/excise.php> (fetched 2026-11).

The page describes three mechanisms:

1. **Single-Scan Dual-Action Engine** — one scan both bills the customer and
   "packages encrypted compliance data and transmits telemetry to state servers
   in the background".
2. **Proprietary Chrome Extension for 2-Way Portal Sync** — "1-click indent & TP
   import" and "closing balance sync" that "pushes verified daily sales and
   closing stock back to the portal".
3. **Zero-Downtime Hybrid Reliability** — "0.1s offline engine … all transactions
   are held in a secure local queue and sync automatically when connectivity
   returns".

Plus a **Pre-Validation Gate** ("state MRP locks and TP batch mismatch alerts"),
a **PDF Import** of depot/BEVCO invoices, and a **28-state** directory.

The FAQ adds that a single scan "routes data through two concurrent background
threads: one prints the customer receipt and updates inventory, while the other
sends encrypted regulatory compliance data directly to state excise servers".

---

## 2. What the legacy code actually does

Decompiled source: `decode/Madhushala Pro.exe/Madhushala Pro.decompiled.cs`
(referenced line numbers). Two independent, West-Bengal-specific mechanisms.

### 2.1 Track A — direct state API push (WB EODB)

Each liquor item carries an excise identity: **GTIN** (`itemmst.barcode`),
**Pack_Size** (`itemmst.ml`) and **MRP** (`itemmst.salesrate`).

At billing, `InsertExciseRecords` (line 273242) upserts one row into the local
table `tblTempSaleRecordExcise` (`Licensee_Id_No, Datetime_Stamp, GTIN,
Pack_Size, MRP, Quantity`). Returns are written with a negative quantity.

```
CREATE TABLE [dbo].[tblTempSaleRecordExcise](
  [Licensee_Id_No] varchar(20), [Datetime_Stamp] varchar(20),
  [GTIN] varchar(50), [Pack_Size] varchar(4),
  [MRP] decimal(18,0), [Quantity] int )
```

A separate helper, `ExciseSalesUpload.exe` (also shipped as
`Debug/ExciseSalesUploadOld.exe`), drains the queue in `Excise_Upload()` (line
273116):

1. `SendRequest()` (line 273492) POSTs to
   `https://excise.wb.gov.in/APIEODB/EODBPLA/CheckAuthorization` with
   `{"User_Name":"<LicenseNo>/01","Password":"<faxno>"}` and reads back a
   `TokenCode` (retries up to 5× on HTTP 429).
2. For each queued row it POSTs to
   `https://excise.wb.gov.in/APIEODB/EODBPLA/InsertPosData` with header
   `Tokencode` and body
   `{"Licensee_Id_No","Datetime_Stamp","GTIN","Pack_Size","MRP","Quantity"}`.
3. On a `"success"` reply the row is **deleted** from the queue; failures are
   collected into `Frm_FailedExciseRecords`.
4. `updateCRM()` (line 273221) posts a track record to the vendor's own server.

Credentials come from `companymast` (`LicenseNo`, `faxno`, `website`) and the
`Madhushala Pro.exe.config` app settings.

### 2.2 Track B — Selenium browser automation (WBSBCL / BEVCO portal)

The app bundles `chromedriver.exe` and `WebDriver.dll` and drives a real Chrome:

- `login()` (line 310186) opens
  `https://excise.wb.gov.in/WBSBCL/Bevco/NIC/UserLogin/Login.aspx`, fills
  `txt_username` / `txt_password` from `companymast`, screenshots the CAPTCHA
  (`Image1`), crops it and shows it to the operator to type.
- `RecieveConsignment()` (line 310273) clicks through the portal — sets a date
  range, searches requisitions, clicks each row link and accepts the confirm
  dialog. This is the "1-click TP / indent import".
- `ExcisePdfImport/` + SautinSoft PDF libraries ingest BEVCO depot invoice PDFs
  to pull bottles and TP pass numbers.

### 2.3 Validation gate

The upload screen reconciles `tblTempSaleRecordExcise` against `itemmst` on
`barcode = GTIN AND ml = Pack_Size AND salesrate = MRP` (line 314099). Rows with
no match land in an "unmatched data" grid for manual correction before upload.

---

## 3. Claim vs. code

| Page says | Code shows |
|---|---|
| One scan → two concurrent threads, "encrypted" data to state servers | One scan writes to a local table; upload is a **later, separate** step over **plain JSON** (no encryption) |
| "Auto-sync on reconnection" | Sync is **operator-triggered** (run `ExciseSalesUpload.exe` / the upload screen); no background timer |
| "Proprietary Chrome Extension" | **Not an extension** — bundled chromedriver + Selenium inside the WinForms app |
| "Zero credential storage" | Credentials **are** stored in the DB (`companymast`) |
| "Respects standard CAPTCHA" | True — CAPTCHA image shown to a human |
| "Pre-Validation Gate, MRP lock, TP mismatch alerts" | Partial and real: GTIN+ml+MRP mismatch → unmatched grid; not the automated gate described |
| "Runs on Windows, Linux, Android, iOS" | Excise/sync components are **Windows .NET only** |
| "28 states", "100% compliant" | Page's own disclaimer says state listings don't confirm integration; all but Assam are "Details coming soon". WB-only in practice |

**Bottom line:** a West-Bengal, two-track, offline-first design — a queue filled
at billing then pushed to the WB EODB JSON API with a token, plus Selenium
automation of the WBSBCL/BEVCO portal for indents/TP and CAPTCHA entry. It
avoids double scanning, but the sync is operator-triggered, unencrypted and
state-specific.

---

## 4. Rebuild design (`new_code/`)

The rebuild keeps the same two tracks and the same offline-first guarantee, but
makes the state pluggable and the trigger schedulable. Guiding rules:

- **Never block billing.** Queueing is best-effort inside the existing sale
  transaction; failures are swallowed (as `sales.js` already does for Tally).
- **The queue is the source of truth.** The network layer only moves rows
  between `PENDING → SENT → ACKED / FAILED`.
- **One adapter per state.** West Bengal ships first; others are added without
  touching billing.
- **Deterministic, testable.** The uploader takes an injected `fetch`, so tests
  run against a real local fake server — no mocks.

### 4.1 Data model (new tables in `db/schema_ext.sql`)

- `tbl_ExciseConfig` — per company: state code, endpoint base, licensee id,
  credential secret, auth style, `autoupload` flag, `lastuploadat`.
- `tbl_ExciseQueue` — the outbound sale/return ledger:
  `(id, companycode, yearcode, sourcetable, sourcetrnno, trndate, gtin,
   packsize, mrp, quantity, status, attempts, lasterror, createdat, updatedat,
   ackedat)`, unique on `(companycode, sourcetable, sourcetrnno, gtin, packsize,
   mrp)` so a re-run cannot double-count a line.
- `tbl_ExciseLog` — one row per attempt (mirrors `tbl_TallySyncLog`).
- `tbl_ExciseClosingBalance` — daily closing stock pushed to the portal:
  `(companycode, yearcode, balancedate, gtin, packsize, mrp, closingqty,
   status, attempts, lasterror, createdat, updatedat, ackedat)`.
- `tbl_ExcisePortalInbound` — indents / transport permits pulled from the
  portal: `(id, companycode, doctype, docno, docdate, payload, status,
   receivedat, appliedat)`.

### 4.2 Billing-time capture

`services/excise.js` exports `captureSale(db, {company, year, trnno})`:

1. Read `salesbillmain` + `salesbilldetail`.
2. For each line, resolve the excise identity from `itemmst`
   (`barcode → GTIN`, `ml → Pack_Size`, `salesrate → MRP`).
3. Upsert into `tbl_ExciseQueue` with `Quantity` signed by the bill type
   (negative for `RETURN`), `Datetime_Stamp = trndate`.
4. Rows that fail the identity check are still queued but flagged (see 4.4).

`sales.createSale` calls `captureSale` right after the Tally enqueue, guarded so
any error is ignored — exactly the existing pattern.

### 4.3 State adapter + uploader

An adapter describes how one state's system is reached. Two **modes** ship:

| Mode | Behaviour | Example |
| --- | --- | --- |
| `api` | authorize, then POST each line to a JSON endpoint | West Bengal (EODB) |
| `portal` | stage lines as `SENT`; the shop-side portal agent signs into the state portal and uploads the batch | Odisha (State Excise portal) |

`uploadQueue(db, {company, fetchImpl, state, force})`:

- Resolve the adapter for the configured state (`state` overrides the config).
- **`api` mode:** POST `CheckAuthorization` (WB) with the licensee id and
  credential, read `TokenCode`, then POST each `PENDING` row to `InsertPosData`
  with the `Tokencode` header. On `success` mark `ACKED` (the legacy row is
  deleted; we keep it for an audit trail); on failure mark `FAILED` with
  `lasterror`. HTTP 429 is retried in-process up to 5 times, matching legacy.
- **`portal` mode:** there is no direct API, so each `PENDING` row is marked
  `SENT` and the batch is exposed via `buildPortalBatch()` for the agent. The
  agent reports results through `confirmUploaded()`, which moves rows
  `SENT → ACKED`/`FAILED`.
- `retryFailed()` moves `FAILED → PENDING` in either mode.

#### 4.3.1 West Bengal (`WB`, api mode)

Encodes the exact endpoints and payloads found in the legacy code
(`.../EODBPLA/CheckAuthorization`, `.../EODBPLA/InsertPosData`,
`.../EODBPLA/InsertClosingBalance`; login built as `<LicenseNo>/01`), so a
deployment pointed at the real WB servers behaves identically.

#### 4.3.2 Odisha (`OD`, portal mode)

Odisha ships **no Odisha code** in the legacy app (the vendor's excise page
lists Odisha as "Details coming soon"), and there is no published JSON API. The
real system is the State Excise portal at `stateexcise.odisha.gov.in`, an
ASP.NET application reached through the login page `/UserLogIn/Login.aspx`
(home `/Portal_New_Default.aspx`), where compliance is done by submitting the
POS sales file and the daily closing stock through the portal.

The adapter therefore builds the submission batch — sales lines from
`tbl_ExciseQueue` (status `SENT`) and closing-balance lines from
`tbl_ExciseClosingBalance` (status `PENDING`) — with the same
`Licensee_Id_No / Datetime_Stamp / GTIN / Pack_Size / MRP / Quantity`
(and `Closing_Balance`) payload keys as WB, and hands it to the shop-side portal
agent. `buildPortalBatch()` returns `{ portal: { base, login, home }, licensee,
sales[], closing[], count }`; `confirmUploaded()` records the agent's result.
`uploadClosingBalance()` also switches to staging in portal mode.

> The endpoint/payload contract is **configurable and deliberately minimal**
> (`endpointbase`, plus the path constants in the adapter) so it can be pinned
> to the real portal contract once confirmed with the department. The adapter
> models *where the data goes*, not an unverified wire format.

### 4.4 Pre-validation gate

`validateQueue(db, company)` joins `tbl_ExciseQueue` to `itemmst` on
`barcode = gtin AND ml = packsize AND salesrate = mrp` and returns the rows that
do **not** match (the legacy "unmatched data" grid). `uploadQueue` refuses to
push rows that fail the gate unless explicitly forced, which is the rebuild's
version of the MRP/TP mismatch guard.

### 4.5 Closing-balance sync (Track B, service side)

`buildClosingBalance(db, {company, year, date})` computes per-GTIN closing
quantity from the existing `opening_closing_stock`-style aggregation and upserts
`tbl_ExciseClosingBalance`. `uploadClosingBalance()` pushes them with the same
token. This is the "closing balance sync" the portal page advertises, done
through the service API rather than a browser.

### 4.6 Portal inbound (Track B, transport side)

`receiveInbound()` records indents/TP rows (the Selenium "1-click import"
output) into `tbl_ExcisePortalInbound`; `applyInbound()` upserts them into
`purchasemain` / `purchasedetail` so stock and TP pass numbers land locally.
A companion agent (browser automation or a portal API, per state) performs the
scrape and calls these endpoints with the agent token — the same
connectionless twin-agent shape as the Tally sync.

### 4.7 Configuration surface

Mirrors `tally.getConfig` / `saveConfig`: `GET/PUT /api/excise/config`, with an
`autoupload` flag. When enabled, a scheduler (out of scope here) can call
`uploadQueue` on a timer; the endpoints make that a one-liner.

---

## 5. API surface (added to `server.js`)

| Method | Path | Purpose |
|---|---|---|
| GET | `/api/excise/adapters` | list state adapters + mode |
| GET | `/api/excise/config` | read settings |
| PUT | `/api/excise/config` | save settings |
| GET | `/api/excise/queue` | list queue (filter by status) |
| GET | `/api/excise/summary` | status counts |
| POST | `/api/excise/capture` | backfill capture for a trnno (or all) |
| POST | `/api/excise/upload` | api: push rows; portal: stage rows as `SENT` |
| POST | `/api/excise/retry` | FAILED → PENDING |
| GET | `/api/excise/portal-batch` | portal-mode submission batch for the agent |
| POST | `/api/excise/portal-confirm` | portal agent confirms `SENT → ACKED`/`FAILED` |
| GET | `/api/excise/unmatched` | pre-validation gate results |
| GET | `/api/excise/closing-balance` | build/read daily closing balance |
| POST | `/api/excise/closing-balance/upload` | api: push; portal: stage closing rows |
| GET | `/api/excise/inbound` | list portal inbound rows |
| POST | `/api/excise/inbound` | receive inbound (agent) |
| POST | `/api/excise/inbound/apply` | apply inbound into purchase |
| GET | `/api/excise/logs` | attempt log |

---

## 6. Testing

`test/excise.test.js` starts a **real local HTTP server** that implements the WB
endpoints (`CheckAuthorization`, `InsertPosData`, closing-balance push) and
points the adapter at it. No mocks: the service performs real `fetch` calls to a
real socket. Covered:

- capture on a POS sale (signed quantity, GTIN/Pack_Size/MRP mapping);
- capture on a return (negative quantity);
- re-capture is idempotent (no duplicate rows);
- validation gate flags a row whose MRP no longer matches `itemmst`;
- upload authorizes, pushes, and marks rows `ACKED`;
- a failing push marks the row `FAILED` with `lasterror`, and `retry` requeues it;
- closing-balance build + push;
- inbound receive + apply creates a purchase.

Odisha (portal mode) is covered without any network — the batch is built
locally, so it is asserted directly:

- the `OD` adapter is registered with `mode: 'portal'`;
- `uploadQueue` stages pending rows as `SENT` and `buildPortalBatch` returns the
  portal coordinates + payloads;
- `confirmUploaded` moves staged rows to `ACKED`, or to `FAILED` on rejection
  (and `retry` requeues them);
- the validation gate holds back bad lines in portal mode too;
- closing balance is staged into the batch and confirmed via `confirmUploaded`.

---

## 7. Known limitations / future work

- Two adapters ship: West Bengal (api mode) and Odisha (portal mode). Other
  states need their own adapter (the page's "28 states" is aspirational).
- The Odisha adapter models the portal workflow (stage → agent upload →
  confirm) but its endpoint/payload contract is not verified against the live
  portal (it is unreachable from the build environment); pin it to the
  department's confirmed contract before production use.
- Track B's browser automation (CAPTCHA, portal scraping) is a separate agent,
  not part of the service; the service only consumes/produces its data.
- Credentials are stored in the local DB, as in the legacy app. Encrypting the
  credential at rest and adding an audit trail is recommended before production.
- The `autoupload` scheduler is not included; the API is ready for it.
