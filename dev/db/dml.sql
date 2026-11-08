/*
 * DML - seed data for Madhushala Pro (INSERT statements)
 * Database : barmanager
 * Generated: 2026-11-08 12:09:02 UTC
 * Source   : original Debug/Resources/barmanager.mdf (attached to SQL Server 2022)
 * Tool     : SQL Server Management Objects (SMO) script engine
 */

USE [barmanager];
GO
-- FK checks are disabled while loading so table order does not matter.
EXEC sp_MSforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';
GO

-- ----- dbo.users (1 rows) -----
INSERT [dbo].[users] ([usercode], [username], [accesslevel], [password]) VALUES (N'A00001', N'Admin', N'Admin', N'admin')
GO

-- ----- dbo.accesspanel (0 rows) -----
-- (no rows)
GO

-- ----- dbo.acgroupmst (0 rows) -----
-- (no rows)
GO

-- ----- dbo.companymast (1 rows) -----
INSERT [dbo].[companymast] ([companycode], [companyname], [address1], [address2], [city], [district], [stat], [pin], [phn], [email], [website], [faxno], [lstno], [cstno], [panno], [vatno], [stno]) VALUES (N'1', N'ABM TECHNOMATRIX (P) LTD.', N'7A, CORNFIELD ROAD', N'BALLYGUNJ', N'KOLKATA', N'', N'WEST BENGAL', N'700019', N'03340670384', N'info@abmtechno.com', N'www.abmtechno.com', N'', N'', N'', N'', N'', N'')
GO

-- ----- dbo.acountname (0 rows) -----
-- (no rows)
GO

-- ----- dbo.ledger (0 rows) -----
-- (no rows)
GO

-- ----- dbo.storage (0 rows) -----
-- (no rows)
GO

-- ----- dbo.breakagemain (0 rows) -----
-- (no rows)
GO

-- ----- dbo.categorymst (0 rows) -----
-- (no rows)
GO

-- ----- dbo.groupmst (0 rows) -----
-- (no rows)
GO

-- ----- dbo.Itemgroupml (0 rows) -----
-- (no rows)
GO

-- ----- dbo.kflmst (0 rows) -----
-- (no rows)
GO

-- ----- dbo.strength (0 rows) -----
-- (no rows)
GO

-- ----- dbo.itemmst (0 rows) -----
-- (no rows)
GO

-- ----- dbo.breakagedetail (0 rows) -----
-- (no rows)
GO

-- ----- dbo.companyparamtr (0 rows) -----
-- (no rows)
GO

-- ----- dbo.ExizeCompany (0 rows) -----
-- (no rows)
GO

-- ----- dbo.fine_details (0 rows) -----
-- (no rows)
GO

-- ----- dbo.itemrateinfo (1 rows) -----
INSERT [dbo].[itemrateinfo] ([ratecode], [ratename], [companycode]) VALUES (N'G00001', N'GENERAL', N'1')
GO

-- ----- dbo.itemratemst (0 rows) -----
-- (no rows)
GO

-- ----- dbo.journaldetail (0 rows) -----
-- (no rows)
GO

-- ----- dbo.ledger_opening (0 rows) -----
-- (no rows)
GO

-- ----- dbo.openingstockmst (0 rows) -----
-- (no rows)
GO

-- ----- dbo.openingStockDetail (0 rows) -----
-- (no rows)
GO

-- ----- dbo.trnidmst (0 rows) -----
-- (no rows)
GO

-- ----- dbo.payment_main (0 rows) -----
-- (no rows)
GO

-- ----- dbo.purchasemain (0 rows) -----
-- (no rows)
GO

-- ----- dbo.payment_detail (0 rows) -----
-- (no rows)
GO

-- ----- dbo.purchasedetail (0 rows) -----
-- (no rows)
GO

-- ----- dbo.taxdetail (0 rows) -----
-- (no rows)
GO

-- ----- dbo.PurchaseTaxDetail (0 rows) -----
-- (no rows)
GO

-- ----- dbo.receipt_main (0 rows) -----
-- (no rows)
GO

-- ----- dbo.salesbillmain (0 rows) -----
-- (no rows)
GO

-- ----- dbo.receipt_detail (0 rows) -----
-- (no rows)
GO

-- ----- dbo.reportTaggingDetail (0 rows) -----
-- (no rows)
GO

-- ----- dbo.reportTaggingMain (0 rows) -----
-- (no rows)
GO

-- ----- dbo.sales_tax (0 rows) -----
-- (no rows)
GO

-- ----- dbo.salesbilldetail (0 rows) -----
-- (no rows)
GO

-- ----- dbo.stk_transfer_main (0 rows) -----
-- (no rows)
GO

-- ----- dbo.stk_transfer_detail (0 rows) -----
-- (no rows)
GO

-- ----- dbo.TEMP (0 rows) -----
-- (no rows)
GO

-- ----- dbo.tempitem (0 rows) -----
-- (no rows)
GO

-- ----- dbo.templedger (0 rows) -----
-- (no rows)
GO

-- ----- dbo.userrights (0 rows) -----
-- (no rows)
GO

-- ----- dbo.yearmst (1 rows) -----
INSERT [dbo].[yearmst] ([yearcode], [yearrange], [stdate], [enddate], [companycode]) VALUES (N'1', N'2014-2014', CAST(N'2014-04-01T00:00:00.000' AS DateTime), CAST(N'2014-03-31T00:00:00.000' AS DateTime), N'1')
GO

-- Re-enable and validate all foreign keys.
EXEC sp_MSforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL';
GO

-- Total data rows: 4
