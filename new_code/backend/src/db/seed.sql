-- Seed data for Madhushala Pro (new stack).
-- The first block mirrors the rows shipped in the original barmanager DB.
-- The rest is a small bootstrap dataset so every business flow is runnable.

-- ---- rows from the original database ----
INSERT OR IGNORE INTO users (usercode, username, accesslevel, password)
VALUES ('A00001', 'Admin', 'Admin', 'admin');

INSERT OR IGNORE INTO companymast
  (companycode, companyname, address1, address2, city, district, stat, pin, phn, email, website, faxno, lstno, cstno, panno, vatno, stno)
VALUES
  ('1', 'ABM TECHNOMATRIX (P) LTD.', '7A, CORNFIELD ROAD', 'BALLYGUNJ', 'KOLKATA', '', 'WEST BENGAL', '700019', '03340670384', 'info@abmtechno.com', 'www.abmtechno.com', '', '', '', '', '', '');

INSERT OR IGNORE INTO itemrateinfo (ratecode, ratename, companycode)
VALUES ('G00001', 'GENERAL', '1');

-- ---- bootstrap data ----
INSERT OR IGNORE INTO yearmst (yearcode, yearrange, stdate, enddate, companycode)
VALUES ('2024', '2024-2025', '2024-04-01', '2025-03-31', '1');

INSERT OR IGNORE INTO users (usercode, username, accesslevel, password)
VALUES ('A00002', 'Cashier', 'User', 'cashier');

-- accounting groups and heads
INSERT OR IGNORE INTO acgroupmst (acgroupcode, acgroupname, actype, sequence) VALUES
  ('AG0001', 'Current Assets', 'Assets', 1),
  ('AG0002', 'Current Liabilities', 'Liabilities', 2),
  ('AG0003', 'Income', 'Income', 3),
  ('AG0004', 'Expenses', 'Expenses', 4);

INSERT OR IGNORE INTO acountname (accode, acgroupcode, acname, actype, subgroupcode, companycode) VALUES
  ('AC0001', 'AG0001', 'Cash & Bank', 0, 'CA', '1'),
  ('AC0002', 'AG0001', 'Sundry Debtors', 0, 'CA', '1'),
  ('AC0003', 'AG0002', 'Sundry Creditors', 1, 'CL', '1'),
  ('AC0004', 'AG0003', 'Sales', 1, 'IN', '1'),
  ('AC0005', 'AG0004', 'Purchase', 0, 'EX', '1'),
  ('AC0006', 'AG0004', 'Discount', 0, 'EX', '1');

-- ledgers
INSERT OR IGNORE INTO ledger (name, accode, ledcode, companycode, type) VALUES
  ('Cash',              'AC0001', 'L00001', '1', 'Cash'),
  ('Bank',              'AC0001', 'L00002', '1', 'Bank'),
  ('Sales',             'AC0004', 'L00003', '1', 'Income'),
  ('Purchase',          'AC0005', 'L00004', '1', 'Expense'),
  ('Discount Allowed',  'AC0006', 'L00005', '1', 'Expense'),
  ('Discount Received', 'AC0006', 'L00006', '1', 'Income'),
  ('Sundry Debtors',    'AC0002', 'L00007', '1', 'Party'),
  ('Sundry Creditors',  'AC0003', 'L00008', '1', 'Party'),
  ('Sales Return',      'AC0004', 'L00009', '1', 'Income'),
  ('Breakage',          'AC0006', 'L00010', '1', 'Expense'),
  ('ABC Wines',         'AC0003', 'S00001', '1', 'Supplier'),
  ('Walk-in Customer',  'AC0002', 'C00001', '1', 'Customer');

-- company parameters (point accounting heads at the ledgers above)
INSERT OR IGNORE INTO companyparamtr
  (companycode, csalebillno, SaleAccount_Head, bankacc, cashparty, crcashparty, breakacc, saleretrnacc,
   printbill, purchaseacchd, cashhd, discntacc, defshpgdn, ratecode, expenceacc, rndoffacc,
   actposmode, print_mode, surcharge_percent, language, backup_upload, sms_on,
   sms_billEditDelete, sms_PartySale, sms_creditCardSale, sms_payment, sms_receipt,
   csLimit, userWiseSalesStatement)
VALUES
  ('1', 0, 'L00003', 'L00002', 'C00001', 'C00001', 'L00010', 'L00009',
   'N', 'L00004', 'L00001', 'L00005', 'MAIN', 'G00001', 'L00004', 'L00005',
   'Y', 'POS', 0, 'EN', 0, 0,
   0, 0, 0, 0, 0,
   1000, 0);

-- stores / counters
INSERT OR IGNORE INTO storage (companycode, shopcode, shopname, address, ShopType) VALUES
  ('1', 'MAIN', 'Main Store', 'Kolkata', 'STO'),
  ('1', 'CNT1', 'Counter 1', 'Kolkata', 'POS');

-- product masters
INSERT OR IGNORE INTO categorymst (companycode, categorycode, categoryname) VALUES
  ('1', 'C00001', 'Whisky'),
  ('1', 'C00002', 'Beer'),
  ('1', 'C00003', 'Rum');

INSERT OR IGNORE INTO groupmst (companycode, groupcode, groupname, subGroupCode) VALUES
  ('1', 'G00001', 'IMFL', 'main'),
  ('1', 'G00002', 'Beer', 'main');

INSERT OR IGNORE INTO kflmst (companycode, kflcode, kflname) VALUES
  ('1', 'K00001', 'IMFL'),
  ('1', 'K00002', 'BEER');

INSERT OR IGNORE INTO strength (companycode, strengthname) VALUES
  ('1', 'FULL'),
  ('1', 'BEER');

INSERT OR IGNORE INTO Itemgroupml (ml, packing, companycode) VALUES
  (750, 12, '1'),
  (650, 24, '1'),
  (330, 24, '1');

INSERT OR IGNORE INTO itemmst
  (groupcode, itemcode, itemname, categorycode, kflcode, packing, strengthname, ml, companycode,
   barcode, salesrate, purchaseRate, purchase_discount_amount, purchase_salestaxonmrp_amount,
   purchase_taxscheme_amount, min_qnty)
VALUES
  ('G00001', 'I00001', 'Royal Stag 750ml',   'C00001', 'K00001', 12, 'FULL', 750, '1', '8901000000011', 250.0, 200.0, 0, 0, 0, 12),
  ('G00001', 'I00002', 'Blenders Pride 750ml','C00001', 'K00001', 12, 'FULL', 750, '1', '8901000000028', 320.0, 260.0, 0, 0, 0, 12),
  ('G00002', 'I00003', 'Kingfisher Beer 650ml','C00002', 'K00002', 24, 'BEER', 650, '1', '8901000000035', 90.0,  70.0,  0, 0, 0, 24);

INSERT OR IGNORE INTO itemratemst (companycode, ratecode, itemcode, salesrate) VALUES
  ('1', 'G00001', 'I00001', 250.0),
  ('1', 'G00001', 'I00002', 320.0),
  ('1', 'G00001', 'I00003', 90.0);

-- user rights (module level view/add/edit/delete)
INSERT OR IGNORE INTO userrights (usercode, companycode, accaccess, modulename, perv, pera, pere, perd) VALUES
  ('A00001', '1', 'Y', 'MASTERS', 'Y', 'Y', 'Y', 'Y'),
  ('A00001', '1', 'Y', 'TRANSACTIONS', 'Y', 'Y', 'Y', 'Y'),
  ('A00001', '1', 'Y', 'REPORTS', 'Y', 'Y', 'Y', 'Y'),
  ('A00002', '1', 'Y', 'TRANSACTIONS', 'Y', 'Y', 'N', 'N'),
  ('A00002', '1', 'Y', 'REPORTS', 'Y', 'N', 'N', 'N');
