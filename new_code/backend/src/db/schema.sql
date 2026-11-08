-- SQLite schema for Madhushala Pro (new stack).
-- Generated from dev/db/ddl.sql by scripts/gen_schema.py - do not edit by hand.
PRAGMA foreign_keys = ON;

CREATE TABLE IF NOT EXISTS [ExizeCompany] (
  [companyCode] TEXT NOT NULL,
  [mergeCompanyCode] TEXT NOT NULL,
  PRIMARY KEY ([companyCode], [mergeCompanyCode])
);

CREATE TABLE IF NOT EXISTS [Itemgroupml] (
  [ml] INTEGER NOT NULL,
  [packing] REAL NOT NULL,
  [companycode] TEXT NOT NULL,
  PRIMARY KEY ([companycode], [packing], [ml])
);

CREATE TABLE IF NOT EXISTS [PurchaseTaxDetail] (
  [companycode] TEXT,
  [trnno] INTEGER NOT NULL,
  [schemecode] TEXT,
  [TaxCode] TEXT,
  [TaxRate] TEXT,
  [OnAmount] TEXT,
  [TaxAmount] TEXT,
  [TaxAccount] TEXT,
  [yearcode] TEXT
);

CREATE TABLE IF NOT EXISTS [TEMP] (
  [companyCode] TEXT NOT NULL,
  [yearCode] TEXT NOT NULL,
  [kflName] TEXT NOT NULL,
  [type] TEXT NOT NULL,
  [groupName] TEXT NOT NULL,
  [categoryName] TEXT NOT NULL,
  [ml] INTEGER NOT NULL,
  [qnty] REAL NOT NULL,
  [batchno] TEXT NOT NULL,
  [party] TEXT NOT NULL,
  [billno] TEXT NOT NULL,
  [tppassno] TEXT NOT NULL,
  [trndate] TEXT NOT NULL,
  PRIMARY KEY ([companyCode], [yearCode], [trndate], [billno], [batchno], [categoryName], [groupName], [kflName], [ml], [party], [qnty], [tppassno], [type])
);

CREATE TABLE IF NOT EXISTS [accesspanel] (
  [usercode] TEXT,
  [menuname] TEXT,
  [menutext] TEXT
);

CREATE TABLE IF NOT EXISTS [acgroupmst] (
  [acgroupcode] TEXT NOT NULL,
  [acgroupname] TEXT NOT NULL,
  [actype] TEXT NOT NULL,
  [sequence] INTEGER NOT NULL,
  PRIMARY KEY ([acgroupcode])
);

CREATE TABLE IF NOT EXISTS [acountname] (
  [accode] TEXT NOT NULL,
  [acgroupcode] TEXT,
  [acname] TEXT,
  [actype] INTEGER,
  [subgroupcode] TEXT,
  [companycode] TEXT NOT NULL,
  PRIMARY KEY ([companycode], [accode])
);

CREATE TABLE IF NOT EXISTS [breakagedetail] (
  [CompanyCode] TEXT NOT NULL,
  [trnno] INTEGER NOT NULL,
  [ItemCode] TEXT NOT NULL,
  [Receivd] TEXT,
  [yearcode] TEXT NOT NULL,
  [loose] REAL,
  [box] REAL,
  [quantity] REAL,
  [trndate] TEXT,
  [batchDate] TEXT NOT NULL,
  [batchNo] TEXT NOT NULL,
  PRIMARY KEY ([CompanyCode], [yearcode], [trnno], [batchDate], [batchNo], [ItemCode])
);

CREATE TABLE IF NOT EXISTS [breakagemain] (
  [CompanyCode] TEXT NOT NULL,
  [YearCode] TEXT NOT NULL,
  [TrnNo] INTEGER NOT NULL,
  [Narration] TEXT,
  [shopcode] TEXT,
  [trndate] TEXT NOT NULL,
  [ledgercode] TEXT,
  [billno] TEXT,
  [TP_pass_no] TEXT,
  [trnid] INTEGER NOT NULL,
  PRIMARY KEY ([CompanyCode], [YearCode], [trndate], [TrnNo])
);

CREATE TABLE IF NOT EXISTS [categorymst] (
  [companycode] TEXT NOT NULL,
  [categorycode] TEXT NOT NULL,
  [categoryname] TEXT,
  PRIMARY KEY ([companycode], [categorycode])
);

CREATE TABLE IF NOT EXISTS [companymast] (
  [companycode] TEXT NOT NULL,
  [companyname] TEXT NOT NULL,
  [address1] TEXT,
  [address2] TEXT,
  [city] TEXT,
  [district] TEXT,
  [stat] TEXT,
  [pin] TEXT,
  [phn] TEXT,
  [email] TEXT,
  [website] TEXT,
  [faxno] TEXT,
  [lstno] TEXT,
  [cstno] TEXT,
  [panno] TEXT,
  [vatno] TEXT,
  [stno] TEXT,
  PRIMARY KEY ([companycode])
);

CREATE TABLE IF NOT EXISTS [companyparamtr] (
  [companycode] TEXT NOT NULL,
  [csalebillno] INTEGER,
  [SaleAccount_Head] TEXT NOT NULL,
  [bankacc] TEXT,
  [cashparty] TEXT,
  [crcashparty] TEXT,
  [breakacc] TEXT,
  [saleretrnacc] TEXT,
  [csalelmt] TEXT,
  [printbill] TEXT,
  [printer] TEXT,
  [purchaseacchd] TEXT NOT NULL,
  [cashhd] TEXT,
  [discntacc] TEXT NOT NULL,
  [defshpgdn] TEXT,
  [ratecode] TEXT,
  [expenceacc] TEXT,
  [rndoffacc] TEXT,
  [autorefcurrsal] TEXT,
  [actposmode] TEXT,
  [bill_footer] TEXT,
  [bill_footer2] TEXT,
  [bill_footer3] TEXT,
  [bill_footer4] TEXT,
  [on_off_shop_type] TEXT,
  [back_up_restore_path] TEXT,
  [print_mode] TEXT,
  [surcharge_account_head] TEXT,
  [Trade_discount_head] TEXT,
  [surcharge_percent] REAL,
  [phone_number1] TEXT,
  [email_id] TEXT,
  [sender_id] TEXT,
  [password] TEXT,
  [zone] TEXT,
  [screen_time] INTEGER,
  [status_show] INTEGER,
  [language] TEXT,
  [message] TEXT,
  [sales_taxon_mrp] TEXT,
  [sales_taxon_mrp_rate] REAL,
  [mrp_change] INTEGER,
  [sms_password] TEXT,
  [saletax_including_free] INTEGER,
  [backup_upload] INTEGER NOT NULL,
  [sms_on] INTEGER NOT NULL,
  [purchase_discount_type] TEXT,
  [batchSale] INTEGER,
  [negativeSale] INTEGER,
  [batchEdit] INTEGER,
  [excisePassword] TEXT,
  [exciseId] TEXT,
  [sms_billEditDelete] INTEGER NOT NULL,
  [sms_PartySale] INTEGER NOT NULL,
  [sms_creditCardSale] INTEGER NOT NULL,
  [sms_payment] INTEGER NOT NULL,
  [sms_receipt] INTEGER NOT NULL,
  [companyId] TEXT,
  [csLimit] INTEGER NOT NULL,
  [userWiseSalesStatement] INTEGER NOT NULL,
  PRIMARY KEY ([companycode])
);

CREATE TABLE IF NOT EXISTS [fine_details] (
  [companyCode] TEXT NOT NULL,
  [yearCode] TEXT NOT NULL,
  [code] TEXT NOT NULL,
  [trndate] TEXT NOT NULL,
  [amount] REAL NOT NULL,
  [naration] TEXT,
  [type] TEXT,
  [trnno] INTEGER NOT NULL,
  [trnid] TEXT NOT NULL,
  PRIMARY KEY ([companyCode], [yearCode], [trndate], [trnno], [code])
);

CREATE TABLE IF NOT EXISTS [groupmst] (
  [companycode] TEXT NOT NULL,
  [groupcode] TEXT NOT NULL,
  [groupname] TEXT,
  [subGroupCode] TEXT NOT NULL,
  PRIMARY KEY ([companycode], [groupcode])
);

CREATE TABLE IF NOT EXISTS [itemmst] (
  [groupcode] TEXT,
  [itemcode] TEXT NOT NULL,
  [itemname] TEXT,
  [categorycode] TEXT,
  [kflcode] TEXT,
  [packing] REAL NOT NULL,
  [strengthname] TEXT,
  [ml] INTEGER NOT NULL,
  [companycode] TEXT NOT NULL,
  [barcode] TEXT,
  [salesrate] REAL,
  [purchaseRate] REAL,
  [purchase_discount_amount] REAL NOT NULL,
  [purchase_salestaxonmrp_amount] REAL NOT NULL,
  [purchase_taxscheme_amount] REAL NOT NULL,
  [min_qnty] INTEGER,
  PRIMARY KEY ([companycode], [itemcode])
);

CREATE TABLE IF NOT EXISTS [itemrateinfo] (
  [ratecode] TEXT NOT NULL,
  [ratename] TEXT,
  [companycode] TEXT NOT NULL,
  PRIMARY KEY ([companycode], [ratecode])
);

CREATE TABLE IF NOT EXISTS [itemratemst] (
  [companycode] TEXT NOT NULL,
  [ratecode] TEXT NOT NULL,
  [itemcode] TEXT NOT NULL,
  [salesrate] REAL,
  PRIMARY KEY ([companycode], [itemcode], [ratecode])
);

CREATE TABLE IF NOT EXISTS [journaldetail] (
  [companycode] TEXT NOT NULL,
  [yearcode] TEXT NOT NULL,
  [trnno] TEXT NOT NULL,
  [trndate] TEXT NOT NULL,
  [slno] INTEGER NOT NULL,
  [to_by] TEXT NOT NULL,
  [ledcode] TEXT NOT NULL,
  [chqno] TEXT,
  [chqdate] TEXT,
  [accpay] TEXT,
  [debit] REAL,
  [credit] REAL,
  [type] TEXT NOT NULL,
  [narr] TEXT,
  [trnid] INTEGER NOT NULL,
  [Currbal] REAL,
  PRIMARY KEY ([companycode], [yearcode], [trndate], [trnno], [slno])
);

CREATE TABLE IF NOT EXISTS [kflmst] (
  [companycode] TEXT NOT NULL,
  [kflcode] TEXT NOT NULL,
  [kflname] TEXT,
  PRIMARY KEY ([companycode], [kflcode])
);

CREATE TABLE IF NOT EXISTS [ledger] (
  [name] TEXT,
  [accode] TEXT NOT NULL,
  [contperson] TEXT,
  [city] TEXT,
  [district] TEXT,
  [state] TEXT,
  [phone] TEXT,
  [email] TEXT,
  [www] TEXT,
  [address1] TEXT,
  [area] TEXT,
  [address2] TEXT,
  [debit] REAL,
  [vatno] TEXT,
  [panno] TEXT,
  [ledcode] TEXT NOT NULL,
  [companycode] TEXT NOT NULL,
  [credit] REAL,
  [type] TEXT,
  [ratecode] TEXT,
  PRIMARY KEY ([companycode], [ledcode])
);

CREATE TABLE IF NOT EXISTS [ledger_opening] (
  [companycode] TEXT NOT NULL,
  [yearcode] TEXT,
  [ledcode] TEXT NOT NULL,
  [debit] REAL,
  [credit] REAL,
  [type] TEXT,
  [stdate] TEXT
);

CREATE TABLE IF NOT EXISTS [openingStockDetail] (
  [companyCode] TEXT NOT NULL,
  [yearCode] TEXT NOT NULL,
  [trndate] TEXT,
  [shopCode] TEXT NOT NULL,
  [batchNo] TEXT NOT NULL,
  [itemCode] TEXT NOT NULL,
  [loose] INTEGER,
  [box] INTEGER,
  [qnty] INTEGER,
  PRIMARY KEY ([companyCode], [yearCode], [shopCode], [batchNo], [itemCode])
);

CREATE TABLE IF NOT EXISTS [openingstockmst] (
  [shopcode] TEXT NOT NULL,
  [companycode] TEXT NOT NULL,
  [itemcode] TEXT NOT NULL,
  [loose] INTEGER,
  [box] INTEGER,
  [qnty] INTEGER,
  [yearcode] TEXT NOT NULL,
  [trndate] TEXT,
  PRIMARY KEY ([companycode], [yearcode], [shopcode], [itemcode])
);

CREATE TABLE IF NOT EXISTS [payment_detail] (
  [companycode] TEXT NOT NULL,
  [yearcode] TEXT NOT NULL,
  [vchno] INTEGER NOT NULL,
  [trnno] TEXT NOT NULL,
  [due_amount] REAL,
  [debit] REAL,
  [credit] REAL,
  [vchdate] TEXT NOT NULL,
  [supplierCode] TEXT,
  PRIMARY KEY ([companycode], [yearcode], [vchdate], [vchno], [trnno])
);

CREATE TABLE IF NOT EXISTS [payment_main] (
  [companycode] TEXT NOT NULL,
  [yearcode] TEXT NOT NULL,
  [vchno] INTEGER NOT NULL,
  [head_account] TEXT,
  [client_account] TEXT,
  [narration] TEXT,
  [vchdate] TEXT NOT NULL,
  [cheque_no] TEXT,
  [due] REAL,
  [amount_paid] REAL,
  [discount] REAL,
  [net_due] REAL,
  [Discount_account_head] TEXT,
  [trnid] INTEGER NOT NULL,
  [chqDate] TEXT,
  PRIMARY KEY ([companycode], [yearcode], [vchdate], [vchno])
);

CREATE TABLE IF NOT EXISTS [purchasedetail] (
  [companycode] TEXT NOT NULL,
  [yearcode] TEXT NOT NULL,
  [trnid] TEXT,
  [slno] TEXT,
  [itemcode] TEXT NOT NULL,
  [batchno] TEXT NOT NULL,
  [itemrate] TEXT NOT NULL,
  [itembox] INTEGER,
  [itemloose] INTEGER,
  [itemquantity] INTEGER,
  [itemamount] TEXT,
  [trnno] INTEGER NOT NULL,
  [itemfreeqnty] INTEGER,
  [itemboxrate] REAL,
  [itemmrp] REAL,
  [itemdiscount] REAL,
  [trndate] TEXT NOT NULL,
  PRIMARY KEY ([companycode], [yearcode], [trnno], [itemcode], [batchno], [itemrate])
);

CREATE TABLE IF NOT EXISTS [purchasemain] (
  [companycode] TEXT NOT NULL,
  [yearcode] TEXT NOT NULL,
  [trnid] INTEGER NOT NULL,
  [trnno] INTEGER NOT NULL,
  [trndate] TEXT NOT NULL,
  [ptype] TEXT,
  [purchaseacccode] TEXT,
  [suppliercode] TEXT,
  [shopcode] TEXT,
  [docno] TEXT NOT NULL,
  [docdate] TEXT,
  [tppassno] TEXT NOT NULL,
  [schemecode] TEXT,
  [totamount] TEXT,
  [tottaxOth] TEXT,
  [totnetamt] TEXT,
  [narration] TEXT,
  [checkedBy] TEXT,
  [saletax_including_free] INTEGER,
  PRIMARY KEY ([companycode], [yearcode], [trnno])
);

CREATE TABLE IF NOT EXISTS [receipt_detail] (
  [companycode] TEXT NOT NULL,
  [yearcode] TEXT NOT NULL,
  [vchno] INTEGER NOT NULL,
  [trnno] TEXT NOT NULL,
  [due_amount] REAL,
  [debit] REAL,
  [credit] REAL,
  [vchdate] TEXT NOT NULL,
  PRIMARY KEY ([companycode], [yearcode], [vchdate], [vchno], [trnno])
);

CREATE TABLE IF NOT EXISTS [receipt_main] (
  [companycode] TEXT NOT NULL,
  [yearcode] TEXT NOT NULL,
  [vchno] INTEGER NOT NULL,
  [head_account] TEXT,
  [client_account] TEXT,
  [narration] TEXT,
  [vchdate] TEXT NOT NULL,
  [cheque_no] TEXT,
  [due] REAL,
  [amount_paid] REAL,
  [discount] REAL,
  [net_due] REAL,
  [Discount_account_head] TEXT,
  [receipt_type] TEXT,
  [trnid] INTEGER NOT NULL,
  [chqDate] TEXT,
  PRIMARY KEY ([companycode], [yearcode], [vchdate], [vchno])
);

CREATE TABLE IF NOT EXISTS [reportTaggingDetail] (
  [reportName] TEXT,
  [reportCategory] TEXT,
  [groupname] TEXT,
  [companycode] TEXT
);

CREATE TABLE IF NOT EXISTS [reportTaggingMain] (
  [reportName] TEXT,
  [reportCategory] TEXT,
  [companycode] TEXT
);

CREATE TABLE IF NOT EXISTS [sales_tax] (
  [trnno] TEXT NOT NULL,
  [companycode] TEXT NOT NULL,
  [yearcode] TEXT NOT NULL,
  [cash_bank_book] TEXT,
  [tax_book] TEXT NOT NULL,
  [tax_amount] REAL,
  [date] TEXT,
  PRIMARY KEY ([companycode], [yearcode], [trnno], [tax_book])
);

CREATE TABLE IF NOT EXISTS [salesbilldetail] (
  [companycode] TEXT NOT NULL,
  [itemcode] TEXT NOT NULL,
  [tot_box] INTEGER,
  [loose] INTEGER,
  [qnty] INTEGER,
  [itemAmount] REAL,
  [trnno] TEXT NOT NULL,
  [rate] REAL,
  [trndate] TEXT,
  [yearcode] TEXT NOT NULL,
  [large] INTEGER,
  [small] INTEGER,
  [westage] INTEGER,
  [MRP] REAL NOT NULL,
  [batchDate] TEXT NOT NULL,
  [batchNo] TEXT NOT NULL,
  PRIMARY KEY ([companycode], [yearcode], [trnno], [batchDate], [batchNo], [itemcode])
);

CREATE TABLE IF NOT EXISTS [salesbillmain] (
  [companycode] TEXT NOT NULL,
  [trnno] TEXT NOT NULL,
  [trndate] TEXT NOT NULL,
  [ledgercode] TEXT NOT NULL,
  [amount] REAL,
  [salestype] TEXT,
  [ratecode] TEXT,
  [storecode] TEXT,
  [yearcode] TEXT NOT NULL,
  [discamount] REAL,
  [netamount] REAL,
  [account_head] TEXT,
  [discount_account_head] TEXT,
  [trntime] TEXT,
  [trnid] INTEGER NOT NULL,
  [discount_reason] TEXT,
  [KFL] TEXT NOT NULL,
  PRIMARY KEY ([companycode], [yearcode], [trnno])
);

CREATE TABLE IF NOT EXISTS [stk_transfer_detail] (
  [CompanyCode] TEXT NOT NULL,
  [ItemCode] TEXT NOT NULL,
  [box] INTEGER,
  [loose] INTEGER,
  [qnty] INTEGER,
  [trnno] INTEGER NOT NULL,
  [trndate] TEXT NOT NULL,
  [yearcode] TEXT NOT NULL,
  [batchDate] TEXT NOT NULL,
  [batchNo] TEXT NOT NULL,
  PRIMARY KEY ([CompanyCode], [yearcode], [trnno], [batchDate], [batchNo], [ItemCode])
);

CREATE TABLE IF NOT EXISTS [stk_transfer_main] (
  [CompanyCode] TEXT NOT NULL,
  [YearCode] TEXT NOT NULL,
  [TrnNo] INTEGER NOT NULL,
  [TrnDate] TEXT NOT NULL,
  [shopname_from] TEXT,
  [shopname_to] TEXT,
  [shopcode_frm] TEXT,
  [shopcode_to] TEXT,
  [trnid] INTEGER NOT NULL,
  PRIMARY KEY ([CompanyCode], [YearCode], [TrnDate], [TrnNo])
);

CREATE TABLE IF NOT EXISTS [storage] (
  [companycode] TEXT NOT NULL,
  [shopcode] TEXT NOT NULL,
  [shopname] TEXT,
  [address] TEXT,
  [ShopType] TEXT,
  PRIMARY KEY ([companycode], [shopcode])
);

CREATE TABLE IF NOT EXISTS [strength] (
  [strengthname] TEXT NOT NULL,
  [companycode] TEXT NOT NULL,
  PRIMARY KEY ([companycode], [strengthname])
);

CREATE TABLE IF NOT EXISTS [taxdetail] (
  [companycode] TEXT NOT NULL,
  [SchemeCode] TEXT NOT NULL,
  [schemename] TEXT,
  [SrNo] INTEGER,
  [TaxCode] TEXT NOT NULL,
  [taxname] TEXT,
  [ledcode] TEXT,
  [sig] TEXT,
  [TaxRate] REAL,
  [taxamount] REAL,
  [calc_on] TEXT,
  [base] INTEGER,
  [after_discount] INTEGER NOT NULL,
  [after_saletax] INTEGER NOT NULL,
  PRIMARY KEY ([companycode], [SchemeCode], [TaxCode])
);

CREATE TABLE IF NOT EXISTS [tempitem] (
  [item] TEXT
);

CREATE TABLE IF NOT EXISTS [templedger] (
  [ledname] TEXT
);

CREATE TABLE IF NOT EXISTS [trnidmst] (
  [companycode] TEXT NOT NULL,
  [yearcode] TEXT NOT NULL,
  [trnid] INTEGER NOT NULL,
  [trn_type] TEXT NOT NULL,
  [usercode] TEXT,
  PRIMARY KEY ([companycode], [yearcode], [trnid])
);

CREATE TABLE IF NOT EXISTS [userrights] (
  [usercode] TEXT NOT NULL,
  [companycode] TEXT NOT NULL,
  [accaccess] TEXT,
  [modulename] TEXT NOT NULL,
  [perv] TEXT,
  [pera] TEXT,
  [pere] TEXT,
  [perd] TEXT,
  PRIMARY KEY ([usercode], [companycode], [modulename])
);

CREATE TABLE IF NOT EXISTS [users] (
  [usercode] TEXT NOT NULL,
  [username] TEXT NOT NULL,
  [accesslevel] TEXT,
  [password] TEXT,
  PRIMARY KEY ([usercode])
);

CREATE TABLE IF NOT EXISTS [yearmst] (
  [yearcode] TEXT NOT NULL,
  [yearrange] TEXT,
  [stdate] TEXT,
  [enddate] TEXT,
  [companycode] TEXT NOT NULL,
  PRIMARY KEY ([companycode], [yearcode])
);

-- Lookup indexes for the main business flows.
CREATE INDEX IF NOT EXISTS [ix_salesbillmain_trndate] ON [salesbillmain] ([trndate]);
CREATE INDEX IF NOT EXISTS [ix_salesbillmain_storecode] ON [salesbillmain] ([storecode]);
CREATE INDEX IF NOT EXISTS [ix_salesbilldetail_trnno] ON [salesbilldetail] ([trnno]);
CREATE INDEX IF NOT EXISTS [ix_salesbilldetail_itemcode] ON [salesbilldetail] ([itemcode]);
CREATE INDEX IF NOT EXISTS [ix_purchasemain_trndate] ON [purchasemain] ([trndate]);
CREATE INDEX IF NOT EXISTS [ix_purchasedetail_trnno] ON [purchasedetail] ([trnno]);
CREATE INDEX IF NOT EXISTS [ix_purchasedetail_itemcode] ON [purchasedetail] ([itemcode]);
CREATE INDEX IF NOT EXISTS [ix_itemmst_itemname] ON [itemmst] ([itemname]);
CREATE INDEX IF NOT EXISTS [ix_itemmst_barcode] ON [itemmst] ([barcode]);
CREATE INDEX IF NOT EXISTS [ix_ledger_name] ON [ledger] ([name]);
CREATE INDEX IF NOT EXISTS [ix_ledger_opening_ledcode] ON [ledger_opening] ([ledcode]);
CREATE INDEX IF NOT EXISTS [ix_journaldetail_ledcode] ON [journaldetail] ([ledcode]);

