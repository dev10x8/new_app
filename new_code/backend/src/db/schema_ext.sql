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

-- ---- additive columns on the generated schema (SQLite ALTER ADD COLUMN) ----
ALTER TABLE [itemmst] ADD COLUMN [manufcode] TEXT;
ALTER TABLE [itemmst] ADD COLUMN [expirydate] TEXT;
