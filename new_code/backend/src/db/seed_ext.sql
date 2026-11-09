-- Bootstrap data for the extended tables (new stack).

INSERT OR IGNORE INTO countermst (companycode, countercode, countername, storecode, machine) VALUES
  ('1', 'C1', 'Counter 1', 'CNT1', 'POS-01'),
  ('1', 'C2', 'Counter 2', 'MAIN', 'POS-02');

INSERT OR IGNORE INTO tblArea (companycode, areacode, areaname) VALUES
  ('1', 'A1', 'Ballygunj'),
  ('1', 'A2', 'Salt Lake');

INSERT OR IGNORE INTO customerDetails (companycode, custcode, custname, phone, email, address, areacode, gstin) VALUES
  ('1', 'CU0001', 'Walk-in Customer', '', '', '', 'A1', ''),
  ('1', 'CU0002', 'Hotel Blue', '9800000000', 'blue@example.com', 'Park Street', 'A1', '19AABCU9603R1ZM');

INSERT OR IGNORE INTO ManufacturerMaster (companycode, manufcode, manufname) VALUES
  ('1', 'M00001', 'United Spirits'),
  ('1', 'M00002', 'United Breweries');

INSERT OR IGNORE INTO naiUnitMaster (companycode, unitcode, unitname) VALUES
  ('1', 'U1', 'Bottle'),
  ('1', 'U2', 'Can');

INSERT OR IGNORE INTO naiItemMainGroup (companycode, groupcode, groupname) VALUES
  ('1', 'NG1', 'Soft Drinks');

INSERT OR IGNORE INTO naiItemSubGroup (companycode, groupcode, subgroupcode, subgroupname) VALUES
  ('1', 'NG1', 'NS1', 'Cola');

INSERT OR IGNORE INTO naiItemMaster (companycode, itemcode, itemname, unitcode, groupcode, subgroupcode, salerate, gstrate, barcode) VALUES
  ('1', 'N00001', 'Cola 300ml', 'U2', 'NG1', 'NS1', 40, 28, '8902000000015');

INSERT OR IGNORE INTO tbl_scheme (companycode, schemecode, schemename, fromdate, todate, active) VALUES
  ('1', 'SC1', 'Beer 1+1', '2024-04-01', '2024-06-30', 1);

INSERT OR IGNORE INTO tbl_schemedetail (companycode, schemecode, itemcode, buyqty, freeqty, discount) VALUES
  ('1', 'SC1', 'I00003', 24, 1, 0);

INSERT OR IGNORE INTO offermst (companycode, offercode, offername, itemcode, discountpct, fromdate, todate, active) VALUES
  ('1', 'OF1', 'Whisky 5% off', 'I00001', 5, '2024-04-01', '2024-04-30', 1);

INSERT OR IGNORE INTO ExciseItemMaster (companycode, itemcode, excisecode, excisename, ml, strength) VALUES
  ('1', 'I00001', 'EX001', 'Royal Stag 750', 750, 'FULL');

INSERT OR IGNORE INTO ExciseItemMapItemMaster (companycode, itemcode, exciseitemcode) VALUES
  ('1', 'I00001', 'EX001');

INSERT OR IGNORE INTO taxdetail (companycode, SchemeCode, schemename, SrNo, TaxCode, taxname, ledcode, sig, TaxRate, taxamount, calc_on, base, after_discount, after_saletax) VALUES
  ('1', 'TS1', 'GST 28', 1, 'T1', 'GST', 'L00003', '+', 28, 0, 'P', 0, 0, 0);

INSERT OR IGNORE INTO reportTaggingMain (reportName, reportCategory, companycode) VALUES
  ('Daily Stock Book', 'Excise', '1'),
  ('Monthly Stock', 'Excise', '1');

INSERT OR IGNORE INTO reportTaggingDetail (reportName, reportCategory, groupname, companycode) VALUES
  ('Daily Stock Book', 'Excise', 'IMFL', '1');

INSERT OR IGNORE INTO Version (id, version, updatedat) VALUES
  (1, '1.0.0-new', '2024-04-01');

UPDATE itemmst SET manufcode='M00001', expirydate='2026-12-31' WHERE itemcode IN ('I00001','I00002');
UPDATE itemmst SET manufcode='M00002', expirydate='2025-06-30' WHERE itemcode='I00003';

-- excise sync: default (disabled) West Bengal config for company 1.
INSERT OR IGNORE INTO tbl_ExciseConfig
  (companycode, state, endpointbase, licenseeid, credential, autoupload, updatedat)
VALUES
  ('1', 'WB', 'https://excise.wb.gov.in/APIEODB/EODBPLA', '', '', 0, '2024-04-01');

-- ---- GST: tax ledgers, item rates and the company GSTIN ----
INSERT OR IGNORE INTO ledger (name, accode, ledcode, companycode, type) VALUES
  ('CGST Output',  'AC0002', 'L00011', '1', 'Duties & Taxes'),
  ('SGST Output',  'AC0002', 'L00012', '1', 'Duties & Taxes'),
  ('IGST Output',  'AC0002', 'L00013', '1', 'Duties & Taxes'),
  ('CGST Input',   'AC0002', 'L00014', '1', 'Duties & Taxes'),
  ('SGST Input',   'AC0002', 'L00015', '1', 'Duties & Taxes'),
  ('IGST Input',   'AC0002', 'L00016', '1', 'Duties & Taxes');

-- Item GST rates (exclusive by default; NAI item inclusive to show both modes).
UPDATE itemmst SET cgst=9,  sgst=9,  igst=0,  cess=0,  addcess=0, gsttype=0,
  CgstOtptLdgr='L00011', SgstOtptLdgr='L00012', IgstOtptLdgr='L00013',
  CgstInptLdgr='L00014', SgstInptLdgr='L00015', IgstInptLdgr='L00016'
 WHERE companycode='1' AND itemcode IN ('I00001','I00002');
UPDATE itemmst SET cgst=14, sgst=14, igst=0,  cess=0,  addcess=0, gsttype=0,
  CgstOtptLdgr='L00011', SgstOtptLdgr='L00012', IgstOtptLdgr='L00013',
  CgstInptLdgr='L00014', SgstInptLdgr='L00015', IgstInptLdgr='L00016'
 WHERE companycode='1' AND itemcode='I00003';
UPDATE naiItemMaster SET cgst=14, sgst=14, igst=0, cess=0, addcess=0, gsttype=1,
  CgstOtptLdgr='L00011', SgstOtptLdgr='L00012', IgstOtptLdgr='L00013',
  CgstInptLdgr='L00014', SgstInptLdgr='L00015', IgstInptLdgr='L00016'
 WHERE companycode='1' AND itemcode='N00001';

-- Company GSTIN (printed on the tax invoice) and filing config.
UPDATE companymast SET gstin='19AABCA1234B1Z5' WHERE companycode='1';
INSERT OR IGNORE INTO tbl_GstConfig (companycode, gstin, legalname, tradename, statecode)
VALUES ('1', '19AABCA1234B1Z5', 'ABM TECHNOMATRIX (P) LTD.', 'MADHUSHALA PRO', '19');
