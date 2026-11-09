-- Extended schema for the new stack: tables that the fuller feature set needs
-- but that were not present in the original DDL snapshot. Same conventions as
-- schema.sql (varchar -> TEXT, int -> INTEGER, float/money -> REAL).

-- ---- masters: counters, customers, areas, manufacturers ----
CREATE TABLE IF NOT EXISTS [countermst] (
  [companycode] TEXT NOT NULL,
  [countercode] TEXT NOT NULL,
  [countername] TEXT,
  [storecode] TEXT,
  [machine] TEXT,
  PRIMARY KEY ([companycode], [countercode])
);

CREATE TABLE IF NOT EXISTS [customerDetails] (
  [companycode] TEXT NOT NULL,
  [custcode] TEXT NOT NULL,
  [custname] TEXT,
  [phone] TEXT,
  [email] TEXT,
  [address] TEXT,
  [areacode] TEXT,
  [gstin] TEXT,
  PRIMARY KEY ([companycode], [custcode])
);

CREATE TABLE IF NOT EXISTS [tblArea] (
  [companycode] TEXT NOT NULL,
  [areacode] TEXT NOT NULL,
  [areaname] TEXT,
  PRIMARY KEY ([companycode], [areacode])
);

CREATE TABLE IF NOT EXISTS [ManufacturerMaster] (
  [companycode] TEXT NOT NULL,
  [manufcode] TEXT NOT NULL,
  [manufname] TEXT,
  PRIMARY KEY ([companycode], [manufcode])
);

-- ---- non-alcoholic (NAI) masters ----
CREATE TABLE IF NOT EXISTS [naiUnitMaster] (
  [companycode] TEXT NOT NULL,
  [unitcode] TEXT NOT NULL,
  [unitname] TEXT,
  PRIMARY KEY ([companycode], [unitcode])
);

CREATE TABLE IF NOT EXISTS [naiItemMainGroup] (
  [companycode] TEXT NOT NULL,
  [groupcode] TEXT NOT NULL,
  [groupname] TEXT,
  PRIMARY KEY ([companycode], [groupcode])
);

CREATE TABLE IF NOT EXISTS [naiItemSubGroup] (
  [companycode] TEXT NOT NULL,
  [groupcode] TEXT NOT NULL,
  [subgroupcode] TEXT NOT NULL,
  [subgroupname] TEXT,
  PRIMARY KEY ([companycode], [groupcode], [subgroupcode])
);

CREATE TABLE IF NOT EXISTS [naiItemMaster] (
  [companycode] TEXT NOT NULL,
  [itemcode] TEXT NOT NULL,
  [itemname] TEXT,
  [unitcode] TEXT,
  [groupcode] TEXT,
  [subgroupcode] TEXT,
  [salerate] REAL,
  [gstrate] REAL,
  [barcode] TEXT,
  PRIMARY KEY ([companycode], [itemcode])
);

-- ---- schemes, offers, excise item mapping ----
CREATE TABLE IF NOT EXISTS [tbl_scheme] (
  [companycode] TEXT NOT NULL,
  [schemecode] TEXT NOT NULL,
  [schemename] TEXT,
  [fromdate] TEXT,
  [todate] TEXT,
  [active] INTEGER DEFAULT 1,
  PRIMARY KEY ([companycode], [schemecode])
);

CREATE TABLE IF NOT EXISTS [tbl_schemedetail] (
  [companycode] TEXT NOT NULL,
  [schemecode] TEXT NOT NULL,
  [itemcode] TEXT NOT NULL,
  [buyqty] INTEGER DEFAULT 0,
  [freeqty] INTEGER DEFAULT 0,
  [discount] REAL DEFAULT 0,
  PRIMARY KEY ([companycode], [schemecode], [itemcode])
);

CREATE TABLE IF NOT EXISTS [offermst] (
  [companycode] TEXT NOT NULL,
  [offercode] TEXT NOT NULL,
  [offername] TEXT,
  [itemcode] TEXT,
  [discountpct] REAL DEFAULT 0,
  [fromdate] TEXT,
  [todate] TEXT,
  [active] INTEGER DEFAULT 1,
  PRIMARY KEY ([companycode], [offercode])
);

CREATE TABLE IF NOT EXISTS [ExciseItemMaster] (
  [companycode] TEXT NOT NULL,
  [itemcode] TEXT NOT NULL,
  [excisecode] TEXT,
  [excisename] TEXT,
  [ml] INTEGER DEFAULT 0,
  [strength] TEXT,
  PRIMARY KEY ([companycode], [itemcode])
);

CREATE TABLE IF NOT EXISTS [ExciseItemMapItemMaster] (
  [companycode] TEXT NOT NULL,
  [itemcode] TEXT NOT NULL,
  [exciseitemcode] TEXT NOT NULL,
  PRIMARY KEY ([companycode], [itemcode], [exciseitemcode])
);

-- ---- transactions: transfer sale / receive ----
CREATE TABLE IF NOT EXISTS [transfer_sale_main] (
  [companycode] TEXT NOT NULL,
  [yearcode] TEXT NOT NULL,
  [trnno] INTEGER NOT NULL,
  [trndate] TEXT NOT NULL,
  [shopcode_from] TEXT,
  [shopcode_to] TEXT,
  [amount] REAL DEFAULT 0,
  [narration] TEXT,
  [trnid] INTEGER DEFAULT 0,
  PRIMARY KEY ([companycode], [yearcode], [trnno])
);

CREATE TABLE IF NOT EXISTS [transfer_sale_detail] (
  [companycode] TEXT NOT NULL,
  [yearcode] TEXT NOT NULL,
  [trnno] INTEGER NOT NULL,
  [itemcode] TEXT NOT NULL,
  [qnty] INTEGER DEFAULT 0,
  [rate] REAL DEFAULT 0,
  [amount] REAL DEFAULT 0,
  PRIMARY KEY ([companycode], [yearcode], [trnno], [itemcode])
);

CREATE TABLE IF NOT EXISTS [transfer_receive_main] (
  [companycode] TEXT NOT NULL,
  [yearcode] TEXT NOT NULL,
  [trnno] INTEGER NOT NULL,
  [trndate] TEXT NOT NULL,
  [shopcode_from] TEXT,
  [shopcode_to] TEXT,
  [narration] TEXT,
  PRIMARY KEY ([companycode], [yearcode], [trnno])
);

CREATE TABLE IF NOT EXISTS [transfer_receive_detail] (
  [companycode] TEXT NOT NULL,
  [yearcode] TEXT NOT NULL,
  [trnno] INTEGER NOT NULL,
  [itemcode] TEXT NOT NULL,
  [qnty] INTEGER DEFAULT 0,
  PRIMARY KEY ([companycode], [yearcode], [trnno], [itemcode])
);

-- ---- transactions: debit / credit notes ----
CREATE TABLE IF NOT EXISTS [notes_main] (
  [companycode] TEXT NOT NULL,
  [yearcode] TEXT NOT NULL,
  [noteno] TEXT NOT NULL,
  [trndate] TEXT NOT NULL,
  [notetype] TEXT NOT NULL,
  [partycode] TEXT,
  [headaccount] TEXT,
  [amount] REAL DEFAULT 0,
  [narration] TEXT,
  [trnid] INTEGER DEFAULT 0,
  PRIMARY KEY ([companycode], [yearcode], [noteno])
);

CREATE TABLE IF NOT EXISTS [notes_detail] (
  [companycode] TEXT NOT NULL,
  [yearcode] TEXT NOT NULL,
  [noteno] TEXT NOT NULL,
  [slno] INTEGER NOT NULL,
  [ledcode] TEXT,
  [amount] REAL DEFAULT 0,
  [narration] TEXT,
  PRIMARY KEY ([companycode], [yearcode], [noteno], [slno])
);

-- ---- transactions: on-a/c adjustment, a/c replace ----
CREATE TABLE IF NOT EXISTS [onaccount_adjustment] (
  [companycode] TEXT NOT NULL,
  [yearcode] TEXT NOT NULL,
  [trnno] INTEGER NOT NULL,
  [trndate] TEXT NOT NULL,
  [partycode] TEXT,
  [amount] REAL DEFAULT 0,
  [narration] TEXT,
  PRIMARY KEY ([companycode], [yearcode], [trnno])
);

CREATE TABLE IF NOT EXISTS [account_replace] (
  [companycode] TEXT NOT NULL,
  [yearcode] TEXT NOT NULL,
  [trnno] INTEGER NOT NULL,
  [trndate] TEXT NOT NULL,
  [from_ledger] TEXT,
  [to_ledger] TEXT,
  [amount] REAL DEFAULT 0,
  [narration] TEXT,
  PRIMARY KEY ([companycode], [yearcode], [trnno])
);

-- ---- transactions: bank reconciliation (BRS) ----
CREATE TABLE IF NOT EXISTS [TransactionReconciliation] (
  [companycode] TEXT NOT NULL,
  [yearcode] TEXT NOT NULL,
  [slno] INTEGER NOT NULL,
  [ledcode] TEXT,
  [trndate] TEXT,
  [chqno] TEXT,
  [chqdate] TEXT,
  [amount] REAL DEFAULT 0,
  [cleared] INTEGER DEFAULT 0,
  [cleareddate] TEXT,
  PRIMARY KEY ([companycode], [yearcode], [slno])
);

-- ---- transactions: UPI / QR payment status ----
CREATE TABLE IF NOT EXISTS [tbl_UPIRespose] (
  [companycode] TEXT NOT NULL,
  [trnno] TEXT NOT NULL,
  [trndate] TEXT,
  [amount] REAL DEFAULT 0,
  [upiid] TEXT,
  [status] TEXT DEFAULT 'PENDING',
  [refno] TEXT,
  PRIMARY KEY ([companycode], [trnno])
);

-- ---- transactions: home delivery ----
CREATE TABLE IF NOT EXISTS [delivery_orders] (
  [companycode] TEXT NOT NULL,
  [yearcode] TEXT NOT NULL,
  [orderno] TEXT NOT NULL,
  [trndate] TEXT,
  [custcode] TEXT,
  [address] TEXT,
  [phone] TEXT,
  [amount] REAL DEFAULT 0,
  [status] TEXT DEFAULT 'OPEN',
  [narration] TEXT,
  PRIMARY KEY ([companycode], [yearcode], [orderno])
);

-- ---- admin: deleted bill archive ----
CREATE TABLE IF NOT EXISTS [DeleteSalesBillMain] (
  [companycode] TEXT NOT NULL,
  [trnno] TEXT NOT NULL,
  [trndate] TEXT,
  [ledgercode] TEXT,
  [amount] REAL DEFAULT 0,
  [salestype] TEXT,
  [storecode] TEXT,
  [yearcode] TEXT NOT NULL,
  [discamount] REAL DEFAULT 0,
  [netamount] REAL DEFAULT 0,
  [deletedon] TEXT
);

CREATE TABLE IF NOT EXISTS [DeletedSalesbilldetail] (
  [companycode] TEXT NOT NULL,
  [itemcode] TEXT,
  [qnty] INTEGER DEFAULT 0,
  [itemAmount] REAL DEFAULT 0,
  [trnno] TEXT,
  [rate] REAL DEFAULT 0,
  [yearcode] TEXT NOT NULL
);

-- ---- tools / sync / audit ----
CREATE TABLE IF NOT EXISTS [tbl_MessageLog] (
  [id] INTEGER PRIMARY KEY AUTOINCREMENT,
  [companycode] TEXT,
  [channel] TEXT,
  [recipient] TEXT,
  [message] TEXT,
  [sentat] TEXT,
  [status] TEXT DEFAULT 'SENT'
);

CREATE TABLE IF NOT EXISTS [tbl_UploadDownloadStatus] (
  [companycode] TEXT NOT NULL,
  [entity] TEXT NOT NULL,
  [direction] TEXT NOT NULL,
  [lastrun] TEXT,
  [records] INTEGER DEFAULT 0,
  [status] TEXT,
  PRIMARY KEY ([companycode], [entity], [direction])
);

CREATE TABLE IF NOT EXISTS [TBL_Audit] (
  [id] INTEGER PRIMARY KEY AUTOINCREMENT,
  [companycode] TEXT,
  [tablename] TEXT,
  [action] TEXT,
  [refkey] TEXT,
  [auditat] TEXT,
  [usercode] TEXT
);

CREATE TABLE IF NOT EXISTS [Version] (
  [id] INTEGER PRIMARY KEY,
  [version] TEXT,
  [updatedat] TEXT
);

-- ---- PML print & reports ----
CREATE TABLE IF NOT EXISTS [tbl_PML] (
  [companycode] TEXT NOT NULL,
  [yearcode] TEXT NOT NULL,
  [slno] INTEGER NOT NULL,
  [trndate] TEXT,
  [itemcode] TEXT,
  [ml] INTEGER DEFAULT 0,
  [bottles] INTEGER DEFAULT 0,
  [narration] TEXT,
  PRIMARY KEY ([companycode], [yearcode], [slno])
);

CREATE TABLE IF NOT EXISTS [tbl_PMLCAL] (
  [companycode] TEXT NOT NULL,
  [yearcode] TEXT NOT NULL,
  [slno] INTEGER NOT NULL,
  [trndate] TEXT,
  [totml] REAL DEFAULT 0,
  [totbottles] INTEGER DEFAULT 0,
  PRIMARY KEY ([companycode], [yearcode], [slno])
);

-- ---- remote Tally sync (connectionless twin-agent) ----
-- Local agent settings for the shop counter.
CREATE TABLE IF NOT EXISTS [tbl_TallySyncConfig] (
  [companycode] TEXT NOT NULL,
  [enabled] INTEGER DEFAULT 0,
  [brokerurl] TEXT,
  [agenttoken] TEXT,
  [companyguid] TEXT,
  [tallycompany] TEXT,
  [agentid] TEXT,
  [lastpullat] TEXT,
  [lastpushat] TEXT,
  [updatedat] TEXT,
  PRIMARY KEY ([companycode])
);

-- Outbound queue: one row per voucher pending delivery to remote Tally.
CREATE TABLE IF NOT EXISTS [tbl_TallySyncQueue] (
  [id] INTEGER PRIMARY KEY AUTOINCREMENT,
  [companycode] TEXT NOT NULL,
  [yearcode] TEXT NOT NULL,
  [vouchertype] TEXT,
  [sourcetable] TEXT,
  [sourcetrnno] TEXT,
  [docdate] TEXT,
  [docno] TEXT,
  [amount] REAL DEFAULT 0,
  [payload] TEXT,
  [status] TEXT DEFAULT 'PENDING',
  [attempts] INTEGER DEFAULT 0,
  [lasterror] TEXT,
  [createdat] TEXT,
  [updatedat] TEXT,
  [ackedat] TEXT,
  UNIQUE ([companycode], [yearcode], [sourcetable], [sourcetrnno])
);

-- Delivery log: one row per sync attempt (for the sync report).
CREATE TABLE IF NOT EXISTS [tbl_TallySyncLog] (
  [id] INTEGER PRIMARY KEY AUTOINCREMENT,
  [queueid] INTEGER,
  [companycode] TEXT,
  [direction] TEXT,
  [action] TEXT,
  [status] TEXT,
  [message] TEXT,
  [loggedat] TEXT
);

-- String-pattern ledger / tax auto-mapping (POS pattern -> Tally master).
CREATE TABLE IF NOT EXISTS [tbl_TallyMapping] (
  [id] INTEGER PRIMARY KEY AUTOINCREMENT,
  [companycode] TEXT NOT NULL,
  [maptype] TEXT NOT NULL,
  [pattern] TEXT NOT NULL,
  [targetname] TEXT,
  [priority] INTEGER DEFAULT 0,
  [active] INTEGER DEFAULT 1
);

-- Masters pulled from corporate Tally back to the counter.
CREATE TABLE IF NOT EXISTS [tbl_TallyInbound] (
  [id] INTEGER PRIMARY KEY AUTOINCREMENT,
  [companycode] TEXT NOT NULL,
  [mtype] TEXT NOT NULL,
  [mcode] TEXT,
  [mname] TEXT NOT NULL,
  [payload] TEXT,
  [status] TEXT DEFAULT 'RECEIVED',
  [receivedat] TEXT,
  [appliedat] TEXT
);

-- ---- state excise compliance sync (WB EODB + portal) ----
-- Per-company excise settings: which state adapter, endpoints and credentials.
CREATE TABLE IF NOT EXISTS [tbl_ExciseConfig] (
  [companycode] TEXT NOT NULL,
  [state] TEXT DEFAULT 'WB',
  [endpointbase] TEXT,
  [licenseeid] TEXT,
  [credential] TEXT,
  [autoupload] INTEGER DEFAULT 0,
  [lastuploadat] TEXT,
  [updatedat] TEXT,
  PRIMARY KEY ([companycode])
);

-- Outbound sale/return ledger pushed to the state excise API. One row per
-- (sale line, GTIN, pack size, MRP); quantity is signed (negative on returns).
CREATE TABLE IF NOT EXISTS [tbl_ExciseQueue] (
  [id] INTEGER PRIMARY KEY AUTOINCREMENT,
  [companycode] TEXT NOT NULL,
  [yearcode] TEXT NOT NULL,
  [sourcetable] TEXT,
  [sourcetrnno] TEXT,
  [trndate] TEXT,
  [gtin] TEXT,
  [packsize] TEXT,
  [mrp] REAL DEFAULT 0,
  [quantity] INTEGER DEFAULT 0,
  [status] TEXT DEFAULT 'PENDING',
  [attempts] INTEGER DEFAULT 0,
  [lasterror] TEXT,
  [createdat] TEXT,
  [updatedat] TEXT,
  [ackedat] TEXT,
  UNIQUE ([companycode], [sourcetable], [sourcetrnno], [gtin], [packsize], [mrp])
);

-- One row per excise upload attempt (audit / diagnostics).
CREATE TABLE IF NOT EXISTS [tbl_ExciseLog] (
  [id] INTEGER PRIMARY KEY AUTOINCREMENT,
  [queueid] INTEGER,
  [companycode] TEXT,
  [direction] TEXT,
  [action] TEXT,
  [status] TEXT,
  [message] TEXT,
  [loggedat] TEXT
);

-- Daily closing stock pushed back to the state portal.
CREATE TABLE IF NOT EXISTS [tbl_ExciseClosingBalance] (
  [id] INTEGER PRIMARY KEY AUTOINCREMENT,
  [companycode] TEXT NOT NULL,
  [yearcode] TEXT NOT NULL,
  [balancedate] TEXT,
  [gtin] TEXT,
  [packsize] TEXT,
  [mrp] REAL DEFAULT 0,
  [closingqty] INTEGER DEFAULT 0,
  [status] TEXT DEFAULT 'PENDING',
  [attempts] INTEGER DEFAULT 0,
  [lasterror] TEXT,
  [createdat] TEXT,
  [updatedat] TEXT,
  [ackedat] TEXT,
  UNIQUE ([companycode], [balancedate], [gtin], [packsize], [mrp])
);

-- Indents / transport permits pulled from the state portal (the Selenium
-- "1-click import" output) and pending review before they hit purchases.
CREATE TABLE IF NOT EXISTS [tbl_ExcisePortalInbound] (
  [id] INTEGER PRIMARY KEY AUTOINCREMENT,
  [companycode] TEXT NOT NULL,
  [doctype] TEXT,
  [docno] TEXT,
  [docdate] TEXT,
  [tppassno] TEXT,
  [payload] TEXT,
  [status] TEXT DEFAULT 'RECEIVED',
  [receivedat] TEXT,
  [appliedat] TEXT
);

-- ---- additive columns on the generated schema (SQLite ALTER ADD COLUMN) ----
ALTER TABLE [itemmst] ADD COLUMN [manufcode] TEXT;
ALTER TABLE [itemmst] ADD COLUMN [expirydate] TEXT;

-- ---- GST: item-level tax rates, ledgers and input/output tax ledgers ----
-- Mirrors the legacy naiItemMaster GST columns: cgst/sgst/igst/cess/addcess
-- rates, the gsttype flag (0 = exclusive/added on top, 1 = inclusive/built in)
-- and the ten input/output tax ledgers used to post GST in the books.
ALTER TABLE [itemmst] ADD COLUMN [cgst] REAL;
ALTER TABLE [itemmst] ADD COLUMN [sgst] REAL;
ALTER TABLE [itemmst] ADD COLUMN [igst] REAL;
ALTER TABLE [itemmst] ADD COLUMN [cess] REAL;
ALTER TABLE [itemmst] ADD COLUMN [addcess] REAL;
ALTER TABLE [itemmst] ADD COLUMN [gsttype] INTEGER DEFAULT 1;
ALTER TABLE [itemmst] ADD COLUMN [baseprice] REAL;
ALTER TABLE [itemmst] ADD COLUMN [CgstInptLdgr] TEXT;
ALTER TABLE [itemmst] ADD COLUMN [SgstInptLdgr] TEXT;
ALTER TABLE [itemmst] ADD COLUMN [CessInptLdgr] TEXT;
ALTER TABLE [itemmst] ADD COLUMN [AdCessInptLdgr] TEXT;
ALTER TABLE [itemmst] ADD COLUMN [CgstOtptLdgr] TEXT;
ALTER TABLE [itemmst] ADD COLUMN [SgstOtptLdgr] TEXT;
ALTER TABLE [itemmst] ADD COLUMN [CessOtptLdgr] TEXT;
ALTER TABLE [itemmst] ADD COLUMN [AdCessOtptLdgr] TEXT;
ALTER TABLE [itemmst] ADD COLUMN [IgstInptLdgr] TEXT;
ALTER TABLE [itemmst] ADD COLUMN [IgstOtptLdgr] TEXT;

ALTER TABLE [naiItemMaster] ADD COLUMN [cgst] REAL;
ALTER TABLE [naiItemMaster] ADD COLUMN [sgst] REAL;
ALTER TABLE [naiItemMaster] ADD COLUMN [igst] REAL;
ALTER TABLE [naiItemMaster] ADD COLUMN [cess] REAL;
ALTER TABLE [naiItemMaster] ADD COLUMN [addcess] REAL;
ALTER TABLE [naiItemMaster] ADD COLUMN [gsttype] INTEGER DEFAULT 1;
ALTER TABLE [naiItemMaster] ADD COLUMN [baseprice] REAL;
ALTER TABLE [naiItemMaster] ADD COLUMN [CgstInptLdgr] TEXT;
ALTER TABLE [naiItemMaster] ADD COLUMN [SgstInptLdgr] TEXT;
ALTER TABLE [naiItemMaster] ADD COLUMN [CessInptLdgr] TEXT;
ALTER TABLE [naiItemMaster] ADD COLUMN [AdCessInptLdgr] TEXT;
ALTER TABLE [naiItemMaster] ADD COLUMN [CgstOtptLdgr] TEXT;
ALTER TABLE [naiItemMaster] ADD COLUMN [SgstOtptLdgr] TEXT;
ALTER TABLE [naiItemMaster] ADD COLUMN [CessOtptLdgr] TEXT;
ALTER TABLE [naiItemMaster] ADD COLUMN [AdCessOtptLdgr] TEXT;
ALTER TABLE [naiItemMaster] ADD COLUMN [IgstInptLdgr] TEXT;
ALTER TABLE [naiItemMaster] ADD COLUMN [IgstOtptLdgr] TEXT;

-- ---- GST: company GSTIN + purchase GSTIN (printed on the tax invoice) ----
ALTER TABLE [companymast] ADD COLUMN [gstin] TEXT;
ALTER TABLE [purchasemain] ADD COLUMN [gstin] TEXT;

-- ---- GST: per-line tax captured at billing / goods receipt ----
ALTER TABLE [salesbilldetail] ADD COLUMN [taxable] REAL;
ALTER TABLE [salesbilldetail] ADD COLUMN [cgst] REAL;
ALTER TABLE [salesbilldetail] ADD COLUMN [sgst] REAL;
ALTER TABLE [salesbilldetail] ADD COLUMN [igst] REAL;
ALTER TABLE [salesbilldetail] ADD COLUMN [cess] REAL;
ALTER TABLE [salesbilldetail] ADD COLUMN [addcess] REAL;
ALTER TABLE [salesbilldetail] ADD COLUMN [gstp] REAL;
ALTER TABLE [salesbillmain] ADD COLUMN [igst] REAL;

ALTER TABLE [purchasedetail] ADD COLUMN [taxable] REAL;
ALTER TABLE [purchasedetail] ADD COLUMN [cgst] REAL;
ALTER TABLE [purchasedetail] ADD COLUMN [sgst] REAL;
ALTER TABLE [purchasedetail] ADD COLUMN [igst] REAL;
ALTER TABLE [purchasedetail] ADD COLUMN [cess] REAL;
ALTER TABLE [purchasedetail] ADD COLUMN [addcess] REAL;
ALTER TABLE [purchasedetail] ADD COLUMN [gstp] REAL;
ALTER TABLE [purchasedetail] ADD COLUMN [CgstInptLdgr] TEXT;
ALTER TABLE [purchasedetail] ADD COLUMN [SgstInptLdgr] TEXT;
ALTER TABLE [purchasedetail] ADD COLUMN [CessInptLdgr] TEXT;
ALTER TABLE [purchasedetail] ADD COLUMN [AdCessInptLdgr] TEXT;
ALTER TABLE [purchasedetail] ADD COLUMN [IgstInptLdgr] TEXT;

-- ---- GST return filing: GSTR-1 (outward) / GSTR-3B (summary) runs ----
CREATE TABLE IF NOT EXISTS [tbl_GstReturn] (
  [id] INTEGER PRIMARY KEY AUTOINCREMENT,
  [companycode] TEXT NOT NULL,
  [yearcode] TEXT NOT NULL,
  [rettype] TEXT NOT NULL,             -- GSTR1 | GSTR3B
  [period] TEXT NOT NULL,              -- YYYY-MM
  [fromdate] TEXT,
  [todate] TEXT,
  [status] TEXT DEFAULT 'DRAFT',       -- DRAFT | GENERATED | FILED
  [gstin] TEXT,
  [arn] TEXT,                          -- acknowledgement reference (on filing)
  [taxable] REAL DEFAULT 0,
  [cgst] REAL DEFAULT 0,
  [sgst] REAL DEFAULT 0,
  [igst] REAL DEFAULT 0,
  [cess] REAL DEFAULT 0,
  [taxamount] REAL DEFAULT 0,
  [invoiceno] INTEGER DEFAULT 0,
  [json] TEXT,
  [createdat] TEXT,
  [filedat] TEXT
);

-- ---- GST return filing: append-only audit trail of every action ----
CREATE TABLE IF NOT EXISTS [tbl_GstReturnLog] (
  [id] INTEGER PRIMARY KEY AUTOINCREMENT,
  [companycode] TEXT NOT NULL,
  [returnid] INTEGER,
  [action] TEXT,
  [detail] TEXT,
  [createdat] TEXT
);

-- ---- GST return filing: portal / GSP connection settings ----
CREATE TABLE IF NOT EXISTS [tbl_GstConfig] (
  [companycode] TEXT NOT NULL,
  [gstin] TEXT,
  [legalname] TEXT,
  [tradename] TEXT,
  [statecode] TEXT,
  [endpointbase] TEXT,
  [clientid] TEXT,
  [clientsecret] TEXT,
  [autofile] INTEGER DEFAULT 0,
  [lastfiledat] TEXT,
  [updatedat] TEXT,
  PRIMARY KEY ([companycode])
);
