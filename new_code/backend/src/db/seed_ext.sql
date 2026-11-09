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
