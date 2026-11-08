# Madhushala Pro — Product Features, Menus & Business Flows

Reverse-engineered from the decompiled sources in `decode/` (see
`decode/README.md`). This documents the running application's navigation
structure and its end-to-end business processes.

- Product: **Madhushala Pro** (aka Madhushala Ultimate) — bar / liquor retail
  and wholesale POS & management system.
- Platform: Windows .NET Framework 4.0, VB.NET WinForms (decompiled to C#).
- Database: Microsoft SQL Server (local `barmanager` + optional central server).
- Entry point: `Madhushala Pro.exe` → `MyProject.Forms.Form1` (company / year
  selection) → `frmLoginNew` (login) → `frm_main` (shell with the menu tree).

---

## 1. Application shell

`frm_main` is the MDI container. Navigation is a left `TreeView` (`tv_menu`)
with **11 top-level nodes**. A click runs `tvMenuClick(node)`, which either:

- sets `mdl_public_variables.count` and calls `frmload()` → opens the generic
  entry form `frm_Form1` in the mode for that count, or
- calls `reportFormOpen(no, title)` / `open_frm_ReportsPreview(name)` → opens a
  report viewer, or
- opens a dedicated form (e.g. `frm_sys_admin`, `frm_exciseform`).

Status bar shows company, user, counter, financial year, date/time. A top
toolbar exposes Log In / Log Out / Change Company / Help. The menu is gated by
login: `logout()` greys out everything except `Main` and `Help`.

**Top-level menu nodes**

| # | Node | Purpose |
| --- | --- | --- |
| 1 | Main | Session: Log Out, System Admin, Exit |
| 2 | Admin | Company, Parameters, User, User Rights, New Year, Excise Audit, Deleted Bill |
| 3 | Masters | All master data (30 sub-items) |
| 4 | Transactions | Sales, purchase, vouchers, stock (20 sub-items) |
| 5 | Reports | Sales, stock, purchase, excise, MIS, accounts reports (8 groups) |
| 6 | Tools | Calculator, backup/restore, Tally export, WhatsApp, Excise |
| 7 | Dash Board | Status dashboards (6 items) |
| 8 | Help | Ask for Help, About, List of Updates |
| 9 | Clean Print Garbage | Maintenance utility |
| 10 | PML Print and Reports | PML reporting |
| 11 | Data Sync | Upload/download to central server |

---

## 2. Full menu tree

### 2.1 Main
- Log Out
- System Admin → `frm_sys_admin` (AMC Updater; deployment modes POS / DELHI / WHOLESALE)
- Exit

### 2.2 Admin
- Company → `frm_companyinfo` (company profile)
- Parameters → `frm_companyparameter` (defaults, accounts heads, printing, tax, sync, POS options)
- User → `frm_userinfo` (users, `users` table; access level Admin/User)
- User Rights → `frm_userrights` (`userrights`: per-module view/add/edit/delete flags)
- New Year → `frm_yearcreation` / `frm_yearcreationPOS` / `frm_yearcreation_Delhi` (carry-forward opening balances)
- Excise Audit → `frm_ExcizeAudit`
- Deleted Bill → deleted sales-bill register (`DeleteSalesBillMain`, `DeletedSalesbilldetail`)

### 2.3 Masters
Accounting & tax:
- A/C Main Group — `MasterAccountsMain`
- A/C Sub Group — `MasterAccountsGroups`
- Ledger — `MasterAccountsLedger` (ledgers carry opening balances)
- Tax Schemes — `frm_Taxscm`
- Excise Report Tagging — `frm_tagging` / `reportTaggingMain` + `reportTaggingDetail`

Liquor product masters:
- Manufacturer — `ManufacturerMaster`
- Brand
- Category
- KFL (Kerala-style category grouping)
- Measure and Packing (bottle sizes / units per case)
- Strength (alcohol strength / ML)
- Item Master — `itemmst`
- NAI Unit / NAI Group / NAI Sub Group / NAI Master — `naiUnitMaster`, `naiItemMainGroup`, `naiItemSubGroup`, `naiItemMaster` (non-alcoholic items)
- Sales Rate — `frm_SalesRate` / `itemratemst`
- Store Master — `frm_storage`
- Area — `tblArea`
- Scheme — `frm_scheme` / `tbl_scheme`, `tbl_schemedetail`
- Offer Master — `frm_OfferMaster` / `offermst`
- Counter Master — `frm_countermst` / `countermst` (POS counters)
- Customer Master — `frm_customer` / `customerDetails`
- Customer Screen Landscape / Portrait — `frm_customer_pos` / `frm_customer_posL1` (customer display)
- Excise Item Master — `ExciseItemMaster`
- Excise Map Item Master — `ExciseItemMapItemMaster`
- Counter Tagging — `frm_CounterTagging`
- Barcode Print — `frm_itemmstadd` barcode output
- Item Merging — `ItemMerging`

### 2.4 Transactions
- Sale - POS — `frm_countersales` (counter billing)
- Counter Sale — `frm_countersalesTransfer` / counter transfer sale
- Purchase — `frm_purchase` / `purchasemain`, `purchasedetail`
- Payment Entry — `frm_conpayrec_vch` (payment voucher)
- Receipt Entry — `frm_receipt_voucher`
- Contra Entry — `frm_contra_entry`
- Journal Entry — `frm_journal_vch` / `journaldetail`
- BRS Entry — `frm_reconciliation` (bank reconciliation / `TransactionReconciliation`)
- Debit Note
- Credit Note
- Opening Stock — `openingStockDetail`
- Stock Transfer — `frm_stocktransfer` / `frm_stocktransfer2`
- Breakage — `frm_breakageentry` / `frm_breakageentryTLW`
- Excise Fine Entry — `frm_fine` / `fine_details`
- On A/C Adjustment — `frm_OnAccountAdjustment`
- A/C Replace — `frm_AccountTransfer` (ledger substitution)
- QRCode Payment Status — `frm_qrorder` (UPI/QR, `tbl_UPIRespose`)
- Transfer Sale — `frm_TransferSale`
- Transfer Receive — `frm_TransferReceive`
- Delivery — `frmDelivery` (home delivery orders)

### 2.5 Reports
Sales Reports (32):
- In Bulk Litre, Sales Statement, Sales Date Wise Statement, Bill Statement,
  Multi Bill Print, ML Wise Sale, Category-ML Wise, KFL-Category-ML,
  Item-Group Wise, Category-Brand, Item Wise Sales, Party Wise Sales,
  Sales Summary, Bill Wise Sales, Amount Wise, Sales Return, Date-Category Wise,
  Manufacturer Wise, Party Weekly Sale, Area Wise Sales, Discount Report,
  Offer Discount, NAI GST Report, Home Delivery Report, Category-Item,
  Parcha Report, Deleted Sale Report, Customer Wise Sales, KFL ML Wise Sale
  Report, Audit Report, Transfer Shop, Transfer Receive

Stock Reports (20):
- Items Wise Closing, Value Wise Closing, Value Wise Detail, Brand-Category,
  Brand-ML Wise, Category-Brand Wise, Category-ML Wise, Manufacturer Wise,
  Stock ML Wise, Stock-In Litre, Min Quantity Item, Stock-BL/LPL,
  Case-Bottle Wise, MainGroup-Sub Group, Sub Group-Item Wise,
  Stock NAI Item wise, Expiry Report, Counter Wise Stock, Daily Stock Item Wise,
  ABC Report

Purchase Reports (10):
- Supplier Details, Bill Details, Bill Summary, Tax report, Item Summary,
  Category Wise, Brand Wise, Manufacturer Wise, Supplier-Brand Wise,
  Purchase NAI GST Report

Other groups:
- Breakage Reports
- Stock Transfer Reports
- MIS Reports (12): Daily Summary, Bill Wise Payment & Receipt,
  Party Sales Discount, Daily Profit-Loss, Bank Clearance, Tax Scheme,
  Comparison Report, Kota Report, Purchase Profit, Scheme Report,
  Payment Summary, Item Wise Profit-Loss
- Excise Reports (21): Daily Stock Book, Daily Stock Online, Daily Stock Online
  IML, Weekly Stock Online, Monthly Stock Online (x2 variants), Monthly Stock
  Offline (x2 variants), Daily Stock, Monthly Stock (x2), Category Register,
  Brand Register, Monthly Stock Assam, Monthly Stock Assam Spirit,
  Date Wise Report, Date Wise Closing Report, Date Wise Excel Report,
  Lifting_IMFL_BEER, Daily Stock Online IML, Daily Stock BL
- Accounts Reports (14): Day Book, Sales Register, Sales Ret. Reg,
  Purchase Register, Pur. Return Reg, General Ledger, Group Closing,
  P/L Account, Balance Sheet, Trial Balance, Tax Report, Tax Register,
  Party Wise Outstanding, CashBook/BankBook

### 2.6 Tools
- Calculator
- Shortcut Panel
- Change Password
- Data Back-Up
- Data Restore
- Export To Tally → Export Sales, Export Purchase (`frm_tally_export`)
- WhatsApp (`frm_message` / SMS & WhatsApp sending)
- Excise → `frm_exciseform` (excise data entry / upload)

### 2.7 Dash Board
- Sales Status
- Purchase / Lifting Status
- Stock Status
- Purchase / Scheme
- Figure of Spirit / Beer
- Figure of CS

### 2.8 Help
- Ask for Help
- About Madhushala
- List of Updates

### 2.9 Other top-level
- Clean Print Garbage — clears print spool/temp artifacts
- PML Print and Reports — PML (`tbl_PML`, `tbl_PMLCAL`) reporting
- Data Sync — opens `frmDataUploadProgress`; central sync

---

## 3. Core business flows

### 3.1 Startup, company/year, login
1. App starts (`Form1` / `Form1New`). If `MultipleCompany=yes`, company selection
   form is shown.
2. `check_connection()` / `check_database()` verify SQL Server and attach/select
   the `barmanager` database; on corruption the app offers to delete/rebuild it.
3. Financial year is selected (`yearmst`, `yearcode`, book start/end dates).
4. Login (`frmLoginNew.ValidatePasswordAndLogin`):
   - joins `userrights` + `users`, or reads `users` directly;
   - compares entered password to `users.password` (plaintext);
   - if `accesslevel = "Admin"` sets `uadmin = true`;
   - on success runs `loginall()` and enables the menu.
5. `frm_main` loads company parameters, user rights, counters, and the menu tree.
   Log out sets `users.isActive=0` and greys the menu.

### 3.2 Masters maintenance
Standard create/edit/delete/clear over the master tables (`itemmst`,
`MasterAccountsLedger`, `countermst`, `customerDetails`, etc.). Masters feed all
transactions. Item master defines packing/ML/strength and links to sales rates.

### 3.3 POS / counter sale (primary revenue flow)
1. User opens **Sale - POS** (`frm_countersales`).
2. Selects counter, customer (optional), sale type (FL / CS / NAI).
3. Adds items (barcode scan or search); quantity in case/bottle/ML; rate from
   `itemratemst`; discounts/offers from `tbl_scheme`, `offermst`.
4. Bill number assigned per counter via `SP_GetBillNo_Bar` / `csalebillno`.
5. On save:
   - writes header `salesbillmain` (trnno, trndate, ledgercode, amount,
     salestype, ratecode, storecode, yearcode, discamount, netamount,
     account_head, trntime, trnid, billtype, PaymentType, userID, counterID);
   - writes lines `salesbilldetail` (itemcode, qty, rate, amount, MRP, discount,
     batch, billtype);
   - posts accounting to `TransactionMain` / `TransactionDetail` /
     `TransactionMatch` (SALE + RECEIPT entries, counter/cash/bank ledgers);
   - updates stock via `SP_SetStockAll` / `SP_SetStockAllPOSLatest`.
6. Prints bill (`frm_bedit` / KOT `frm_KotBilltPrinting`); optional customer
   display, pole display, SMS/WhatsApp, QR/UPI payment (`frm_qrorder`).
7. Sale return / refund writes a RETURN row (`salesbillmain.salestype='RETURN'`)
   and the deleted-bill tables when cancelled (`DeleteSalesBillMain`,
   `DeletedSalesbilldetail`).

### 3.4 Purchase / goods receipt
1. **Purchase** (`frm_purchase`) → `purchasemain` (supplier, invoice, type) +
   `purchasedetail` (items, qty, rate, tax).
2. Optional import from supplier file (`btnImport`) or upload services.
3. Accounting posted to `TransactionMain`/`TransactionMatch` (PURCHASE + PAYMENT).
4. Feeds stock, purchase reports, and supplier outstanding.

### 3.5 Accounts & vouchers
- Payment (`frm_conpayrec_vch`), Receipt (`frm_receipt_voucher`),
  Contra (`frm_contra_entry`), Journal (`frm_journal_vch`), Debit/Credit notes,
  BRS (`frm_reconciliation`).
- All post to `TransactionMain` / `TransactionDetail` / `TransactionMatch`;
  balances derived from `MasterAccountsLedger`, `MasterAccountsOpening`.
- Outstanding/aging via `VoucharsDueNew`, `BillWiseOutstanding`.

### 3.6 Stock
- Opening Stock (`openingStockDetail`), Stock Transfer
  (`frm_stocktransfer`), Breakage (`frm_breakageentry`), Excise Fine
  (`frm_fine`).
- Current stock maintained in `tbl_Stock` / `TempStockDetails` and refreshed by
  `SP_SetStockAll*`; per-day snapshots in `tbl_DailyItemWiseStock`.

### 3.7 Day close / stock refresh
- `frm_manualdayclose` ("Refresh Sale Summary"): picks unclosed date range from
  `salesbillmain` where `dayclose IS NULL`, then
  `update salesbillmain set dayclose = <close date> ...` per counter.
- Stock is (re)computed via `SP_SetStockAll` (or POS variant) over company/year
  and the closed date.

### 3.8 Excise compliance
- Excise masters and tagging (`ExciseItemMaster`, `ExciseItemMapItemMaster`,
  `reportTaggingMain`).
- Excise reports (daily/weekly/monthly stock, IMFL/BEER lifting, Assam variants)
  via `SP_ExciseReportAssam*`.
- Excise upload helper `ExciseSalesUploadOld.exe` downloaded from
  `ftp://13.235.37.46/ExciseUploadExe/`; `frm_exciseform` drives entry/upload.

### 3.9 Data sync (multi-branch / central)
1. `Data Sync` opens `frmDataUploadProgress`; user needs `users.saledatasync`
   and online sync enabled in `companyparamtr`.
2. Upload runs stored procedures per entity:
   `SP_Sales_Upload(_NEW)`, `SP_Purchase_Upload(_NEW)`,
   `SP_Transaction_Upload`, `SP_Transfer_Upload(_NEW)`, `SP_Breakage_Upload(_NEW)`,
   `SP_PAYREC_Upload_NEW`, `SP_Trnid_Upload(_NEW)`.
3. Download pulls master/status updates; status tracked in
   `tbl_UploadDownloadStatus`, `TBL_Audit`, `Version`.
4. Central/master DB connection from config (`conStringOnline`,
   `MasterDBMain`, `CentralizeDB`); `MadhushalaDataSync.dll` provides the
   client, `AutoDataSync.exe` is the background sync helper.

### 3.10 Backup, restore, utilities
- `Tools → Data Back-Up` (`backup()`): gzip-compresses data to the backup path
  set in Admin → Company Parameters; `Data Restore` uses `frm_restore`.
- Export to Tally (`frm_tally_export`), Clean Print Garbage, PML reporting.

---

## 4. Key data model (selected tables)

| Area | Tables |
| --- | --- |
| Company / year | `companymast`, `companyparamtr`, `yearmst` |
| Users / security | `users`, `userrights`, `WebUserMenuRights` |
| Accounts masters | `MasterAccountsMain`, `MasterAccountsGroups`, `MasterAccountsLedger`, `MasterAccountsOpening` |
| Items | `itemmst`, `itemratemst`, `ManufacturerMaster`, `ExciseItemMaster`, `ExciseItemMapItemMaster` |
| NAI | `naiUnitMaster`, `naiItemMainGroup`, `naiItemSubGroup`, `naiItemMaster` |
| Customers / counters | `customerDetails`, `countermst`, `tblArea`, `tblCust_Reminders` |
| Sales | `salesbillmain`, `salesbilldetail`, `sales_tax`, `DeleteSalesBillMain`, `DeletedSalesbilldetail` |
| Purchase | `purchasemain`, `purchasedetail` |
| Accounting engine | `TransactionMain`, `TransactionDetail`, `TransactionMatch`, `TransactionReconciliation`, `journaldetail` |
| Stock | `tbl_Stock`, `openingStockDetail`, `tbl_DailyItemWiseStock`, `TempStockDetails` |
| Schemes / offers | `tbl_scheme`, `tbl_schemedetail`, `offermst` |
| Excise | `ExizeCompany`, `reportTaggingMain`, `reportTaggingDetail`, `tbl_Pinelabe*` |
| Sync / audit | `tbl_UploadDownloadStatus`, `TBL_Audit`, `Version`, `trnidmst` |
| UPI / payments | `tbl_UPIRespose`, `TBL_RazorPayCardDetail` |

Voucher numbering uses `trnidmst` / `tblIndexId` and per-counter `csalebillno`;
bill numbers render as `FL`/`CS`/`NAI` + `trnno`.

---

## 5. Deployment / configuration notes

- Config file: `Debug/Madhushala Pro.exe.config` (`appSettings`).
  - `type` = `single` (local SQL Express) or server mode.
  - `MultipleCompany` = `yes`/`no`.
  - Local DB `barmanager`; remote via `server_crm` + `conStringOnline`.
  - SMS gateway keys (`SMS_*`), printer names, backup paths.
- `Madhushala Pro.exe` is stored in Git LFS; the tracked file is a pointer.
- Databases: `Debug/Resources/barmanager.mdf` (+ `_log.ldf`) and
  `barmanager_demo.mdf`; the app attaches `barmanager.mdf` from its startup path.

> Note: credentials and connection strings appear in plaintext in the config and
> database as shipped; this is a legacy design, documented here for
> completeness, not as a recommendation.
