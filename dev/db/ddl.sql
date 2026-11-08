/*
 * DDL - schema for Madhushala Pro (regenerates the database from scratch)
 * Database : barmanager
 * Generated: 2026-11-08 12:08:52 UTC
 * Source   : original Debug/Resources/barmanager.mdf (attached to SQL Server 2022)
 * Tool     : SQL Server Management Objects (SMO) script engine
 */

-- ============================================================
-- 1. DATABASE
-- ============================================================
-- Created with SQL Server's default data/log directories so the
-- script runs anywhere. Sizes/growth are then aligned to the original.
IF DB_ID(N'barmanager') IS NULL
    CREATE DATABASE [barmanager];
GO
ALTER DATABASE [barmanager] MODIFY FILE (NAME = N'barmanager', SIZE = 4096KB, FILEGROWTH = 1024KB);
ALTER DATABASE [barmanager] MODIFY FILE (NAME = N'barmanager_log', SIZE = 5696KB, FILEGROWTH = 10%);
ALTER DATABASE [barmanager] SET COMPATIBILITY_LEVEL = 100;
GO
USE [barmanager];
GO

-- ============================================================
-- 3. TABLES (primary keys, defaults, checks, indexes, triggers)
--    Foreign keys are added in section 7.
-- ============================================================
-- ----- dbo.accesspanel -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[accesspanel](
	[usercode] [varchar](6) NULL,
	[menuname] [varchar](30) NULL,
	[menutext] [varchar](50) NULL
)

ALTER TABLE [dbo].[accesspanel]  WITH CHECK ADD  CONSTRAINT [FK_accesspanel_users] FOREIGN KEY([usercode])
REFERENCES [dbo].[users] ([usercode])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[accesspanel] CHECK CONSTRAINT [FK_accesspanel_users]
GO

-- ----- dbo.acgroupmst -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[acgroupmst](
	[acgroupcode] [varchar](6) NOT NULL,
	[acgroupname] [varchar](50) NOT NULL,
	[actype] [varchar](50) NOT NULL,
	[sequence] [int] NOT NULL,
 CONSTRAINT [PK_acgroupmst] PRIMARY KEY CLUSTERED 
(
	[acgroupcode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)

ALTER TABLE [dbo].[acgroupmst] ADD  DEFAULT ((0)) FOR [sequence]
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
create trigger [dbo].[acmstdel]
on [dbo].[acgroupmst]
for delete
as
begin
select 'master cannot be deleted'
rollback transaction
end

ALTER TABLE [dbo].[acgroupmst] ENABLE TRIGGER [acmstdel]
GO

-- ----- dbo.acountname -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[acountname](
	[accode] [varchar](6) NOT NULL,
	[acgroupcode] [varchar](6) NULL,
	[acname] [varchar](50) NULL,
	[actype] [bit] NULL,
	[subgroupcode] [varchar](6) NULL,
	[companycode] [varchar](6) NOT NULL,
 CONSTRAINT [PK_acountname] PRIMARY KEY CLUSTERED 
(
	[companycode] ASC,
	[accode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)

ALTER TABLE [dbo].[acountname] ADD  DEFAULT ((0)) FOR [companycode]
ALTER TABLE [dbo].[acountname]  WITH CHECK ADD  CONSTRAINT [FK_acountname_acgroupmst] FOREIGN KEY([acgroupcode])
REFERENCES [dbo].[acgroupmst] ([acgroupcode])
ALTER TABLE [dbo].[acountname] CHECK CONSTRAINT [FK_acountname_acgroupmst]
ALTER TABLE [dbo].[acountname]  WITH CHECK ADD  CONSTRAINT [FK_acountname_companymast] FOREIGN KEY([companycode])
REFERENCES [dbo].[companymast] ([companycode])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[acountname] CHECK CONSTRAINT [FK_acountname_companymast]
GO

-- ----- dbo.breakagedetail -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[breakagedetail](
	[CompanyCode] [varchar](6) NOT NULL,
	[trnno] [int] NOT NULL,
	[ItemCode] [varchar](6) NOT NULL,
	[Receivd] [varchar](100) NULL,
	[yearcode] [varchar](6) NOT NULL,
	[loose] [float] NULL,
	[box] [float] NULL,
	[quantity] [float] NULL,
	[trndate] [datetime] NULL,
	[batchDate] [datetime] NOT NULL,
	[batchNo] [varchar](50) NOT NULL,
 CONSTRAINT [PK_breakagedetail] PRIMARY KEY CLUSTERED 
(
	[CompanyCode] ASC,
	[yearcode] ASC,
	[trnno] ASC,
	[batchDate] ASC,
	[batchNo] ASC,
	[ItemCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)

ALTER TABLE [dbo].[breakagedetail] ADD  DEFAULT (((1)/(1))/(2013)) FOR [batchDate]
ALTER TABLE [dbo].[breakagedetail] ADD  DEFAULT ((0)) FOR [batchNo]
ALTER TABLE [dbo].[breakagedetail]  WITH CHECK ADD  CONSTRAINT [FK_breakagedetail_breakagemain] FOREIGN KEY([CompanyCode], [yearcode], [trndate], [trnno])
REFERENCES [dbo].[breakagemain] ([CompanyCode], [YearCode], [trndate], [TrnNo])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[breakagedetail] CHECK CONSTRAINT [FK_breakagedetail_breakagemain]
ALTER TABLE [dbo].[breakagedetail]  WITH CHECK ADD  CONSTRAINT [FK_breakagedetail_itemmst] FOREIGN KEY([CompanyCode], [ItemCode])
REFERENCES [dbo].[itemmst] ([companycode], [itemcode])
ALTER TABLE [dbo].[breakagedetail] CHECK CONSTRAINT [FK_breakagedetail_itemmst]
GO

-- ----- dbo.breakagemain -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[breakagemain](
	[CompanyCode] [varchar](6) NOT NULL,
	[YearCode] [varchar](6) NOT NULL,
	[TrnNo] [int] NOT NULL,
	[Narration] [varchar](150) NULL,
	[shopcode] [varchar](6) NULL,
	[trndate] [datetime] NOT NULL,
	[ledgercode] [varchar](6) NULL,
	[billno] [varchar](max) NULL,
	[TP_pass_no] [varchar](max) NULL,
	[trnid] [int] NOT NULL,
 CONSTRAINT [PK_breakagemain] PRIMARY KEY CLUSTERED 
(
	[CompanyCode] ASC,
	[YearCode] ASC,
	[trndate] ASC,
	[TrnNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)

ALTER TABLE [dbo].[breakagemain] ADD  DEFAULT ((0)) FOR [trnid]
ALTER TABLE [dbo].[breakagemain]  WITH CHECK ADD  CONSTRAINT [FK_breakagemain_ledger] FOREIGN KEY([CompanyCode], [ledgercode])
REFERENCES [dbo].[ledger] ([companycode], [ledcode])
ALTER TABLE [dbo].[breakagemain] CHECK CONSTRAINT [FK_breakagemain_ledger]
ALTER TABLE [dbo].[breakagemain]  WITH CHECK ADD  CONSTRAINT [FK_breakagemain_storage] FOREIGN KEY([CompanyCode], [shopcode])
REFERENCES [dbo].[storage] ([companycode], [shopcode])
ALTER TABLE [dbo].[breakagemain] CHECK CONSTRAINT [FK_breakagemain_storage]
GO

-- ----- dbo.categorymst -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[categorymst](
	[companycode] [varchar](6) NOT NULL,
	[categorycode] [varchar](6) NOT NULL,
	[categoryname] [varchar](50) NULL,
 CONSTRAINT [PK_categorymst] PRIMARY KEY CLUSTERED 
(
	[companycode] ASC,
	[categorycode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)

ALTER TABLE [dbo].[categorymst]  WITH CHECK ADD  CONSTRAINT [FK_categorymst_companymast] FOREIGN KEY([companycode])
REFERENCES [dbo].[companymast] ([companycode])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[categorymst] CHECK CONSTRAINT [FK_categorymst_companymast]
GO

-- ----- dbo.companymast -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[companymast](
	[companycode] [varchar](6) NOT NULL,
	[companyname] [varchar](50) NOT NULL,
	[address1] [varchar](200) NULL,
	[address2] [varchar](200) NULL,
	[city] [varchar](200) NULL,
	[district] [varchar](200) NULL,
	[stat] [varchar](200) NULL,
	[pin] [varchar](200) NULL,
	[phn] [varchar](200) NULL,
	[email] [varchar](200) NULL,
	[website] [varchar](200) NULL,
	[faxno] [varchar](200) NULL,
	[lstno] [varchar](200) NULL,
	[cstno] [varchar](200) NULL,
	[panno] [varchar](15) NULL,
	[vatno] [varchar](15) NULL,
	[stno] [varchar](15) NULL,
 CONSTRAINT [PK_companymast] PRIMARY KEY CLUSTERED 
(
	[companycode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)

GO

-- ----- dbo.companyparamtr -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[companyparamtr](
	[companycode] [varchar](6) NOT NULL,
	[csalebillno] [int] NULL,
	[SaleAccount_Head] [varchar](6) NOT NULL,
	[bankacc] [varchar](6) NULL,
	[cashparty] [varchar](50) NULL,
	[crcashparty] [varchar](50) NULL,
	[breakacc] [varchar](6) NULL,
	[saleretrnacc] [varchar](6) NULL,
	[csalelmt] [varchar](50) NULL,
	[printbill] [varchar](1) NULL,
	[printer] [varchar](50) NULL,
	[purchaseacchd] [varchar](6) NOT NULL,
	[cashhd] [varchar](6) NULL,
	[discntacc] [varchar](6) NOT NULL,
	[defshpgdn] [varchar](50) NULL,
	[ratecode] [varchar](6) NULL,
	[expenceacc] [varchar](6) NULL,
	[rndoffacc] [varchar](6) NULL,
	[autorefcurrsal] [varchar](1) NULL,
	[actposmode] [varchar](1) NULL,
	[bill_footer] [varchar](20) NULL,
	[bill_footer2] [varchar](20) NULL,
	[bill_footer3] [varchar](20) NULL,
	[bill_footer4] [varchar](20) NULL,
	[on_off_shop_type] [varchar](3) NULL,
	[back_up_restore_path] [varchar](max) NULL,
	[print_mode] [varchar](7) NULL,
	[surcharge_account_head] [varchar](6) NULL,
	[Trade_discount_head] [varchar](6) NULL,
	[surcharge_percent] [float] NULL,
	[phone_number1] [varchar](10) NULL,
	[email_id] [varchar](50) NULL,
	[sender_id] [varchar](50) NULL,
	[password] [varchar](20) NULL,
	[zone] [varchar](50) NULL,
	[screen_time] [int] NULL,
	[status_show] [bit] NULL,
	[language] [varchar](20) NULL,
	[message] [nvarchar](140) NULL,
	[sales_taxon_mrp] [varchar](6) NULL,
	[sales_taxon_mrp_rate] [float] NULL,
	[mrp_change] [bit] NULL,
	[sms_password] [varchar](15) NULL,
	[saletax_including_free] [bit] NULL,
	[backup_upload] [bit] NOT NULL,
	[sms_on] [bit] NOT NULL,
	[purchase_discount_type] [varchar](6) NULL,
	[batchSale] [bit] NULL,
	[negativeSale] [bit] NULL,
	[batchEdit] [bit] NULL,
	[excisePassword] [varchar](40) NULL,
	[exciseId] [varchar](70) NULL,
	[sms_billEditDelete] [bit] NOT NULL,
	[sms_PartySale] [bit] NOT NULL,
	[sms_creditCardSale] [bit] NOT NULL,
	[sms_payment] [bit] NOT NULL,
	[sms_receipt] [bit] NOT NULL,
	[companyId] [nvarchar](20) NULL,
	[csLimit] [int] NOT NULL,
	[userWiseSalesStatement] [bit] NOT NULL,
 CONSTRAINT [PK_companyparamtr] PRIMARY KEY CLUSTERED 
(
	[companycode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF),
 CONSTRAINT [IX_companyparamtr_discntacc] UNIQUE NONCLUSTERED 
(
	[companycode] ASC,
	[discntacc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF),
 CONSTRAINT [IX_companyparamtr_purchaseacchd] UNIQUE NONCLUSTERED 
(
	[companycode] ASC,
	[purchaseacchd] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF),
 CONSTRAINT [IX_companyparamtr_salesaccounthead] UNIQUE NONCLUSTERED 
(
	[companycode] ASC,
	[SaleAccount_Head] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)

ALTER TABLE [dbo].[companyparamtr] ADD  DEFAULT ((0)) FOR [saletax_including_free]
ALTER TABLE [dbo].[companyparamtr] ADD  DEFAULT ((0)) FOR [backup_upload]
ALTER TABLE [dbo].[companyparamtr] ADD  DEFAULT ((0)) FOR [sms_on]
ALTER TABLE [dbo].[companyparamtr] ADD  DEFAULT ((0)) FOR [batchSale]
ALTER TABLE [dbo].[companyparamtr] ADD  DEFAULT ((0)) FOR [negativeSale]
ALTER TABLE [dbo].[companyparamtr] ADD  DEFAULT ((0)) FOR [batchEdit]
ALTER TABLE [dbo].[companyparamtr] ADD  DEFAULT ((0)) FOR [sms_billEditDelete]
ALTER TABLE [dbo].[companyparamtr] ADD  DEFAULT ((0)) FOR [sms_PartySale]
ALTER TABLE [dbo].[companyparamtr] ADD  DEFAULT ((0)) FOR [sms_creditCardSale]
ALTER TABLE [dbo].[companyparamtr] ADD  DEFAULT ((0)) FOR [sms_payment]
ALTER TABLE [dbo].[companyparamtr] ADD  DEFAULT ((0)) FOR [sms_receipt]
ALTER TABLE [dbo].[companyparamtr] ADD  DEFAULT ((1000)) FOR [csLimit]
ALTER TABLE [dbo].[companyparamtr] ADD  DEFAULT ((0)) FOR [userWiseSalesStatement]
ALTER TABLE [dbo].[companyparamtr]  WITH CHECK ADD  CONSTRAINT [FK_companyparamtr_companymast] FOREIGN KEY([companycode])
REFERENCES [dbo].[companymast] ([companycode])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[companyparamtr] CHECK CONSTRAINT [FK_companyparamtr_companymast]
GO

-- ----- dbo.ExizeCompany -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[ExizeCompany](
	[companyCode] [varchar](6) NOT NULL,
	[mergeCompanyCode] [varchar](6) NOT NULL,
 CONSTRAINT [PK_ExizeCompany] PRIMARY KEY CLUSTERED 
(
	[companyCode] ASC,
	[mergeCompanyCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)

GO

-- ----- dbo.fine_details -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[fine_details](
	[companyCode] [varchar](6) NOT NULL,
	[yearCode] [varchar](6) NOT NULL,
	[code] [varchar](3) NOT NULL,
	[trndate] [datetime] NOT NULL,
	[amount] [float] NOT NULL,
	[naration] [varchar](50) NULL,
	[type] [varchar](60) NULL,
	[trnno] [int] NOT NULL,
	[trnid] [varchar](6) NOT NULL,
 CONSTRAINT [PK_find_details] PRIMARY KEY CLUSTERED 
(
	[companyCode] ASC,
	[yearCode] ASC,
	[trndate] ASC,
	[trnno] ASC,
	[code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)

GO

-- ----- dbo.groupmst -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[groupmst](
	[companycode] [varchar](6) NOT NULL,
	[groupcode] [varchar](6) NOT NULL,
	[groupname] [varchar](50) NULL,
	[subGroupCode] [varchar](6) NOT NULL,
 CONSTRAINT [PK_groupmst] PRIMARY KEY CLUSTERED 
(
	[companycode] ASC,
	[groupcode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)

ALTER TABLE [dbo].[groupmst] ADD  DEFAULT ('main') FOR [subGroupCode]
ALTER TABLE [dbo].[groupmst]  WITH CHECK ADD  CONSTRAINT [FK_groupmst_companymast] FOREIGN KEY([companycode])
REFERENCES [dbo].[companymast] ([companycode])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[groupmst] CHECK CONSTRAINT [FK_groupmst_companymast]
GO

-- ----- dbo.Itemgroupml -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[Itemgroupml](
	[ml] [int] NOT NULL,
	[packing] [float] NOT NULL,
	[companycode] [varchar](6) NOT NULL,
 CONSTRAINT [PK_Itemgroupml] PRIMARY KEY CLUSTERED 
(
	[companycode] ASC,
	[packing] ASC,
	[ml] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)

ALTER TABLE [dbo].[Itemgroupml] ADD  DEFAULT ((0)) FOR [companycode]
ALTER TABLE [dbo].[Itemgroupml]  WITH CHECK ADD  CONSTRAINT [FK_Itemgroupml_companymast] FOREIGN KEY([companycode])
REFERENCES [dbo].[companymast] ([companycode])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[Itemgroupml] CHECK CONSTRAINT [FK_Itemgroupml_companymast]
GO

-- ----- dbo.itemmst -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[itemmst](
	[groupcode] [varchar](6) NULL,
	[itemcode] [varchar](6) NOT NULL,
	[itemname] [varchar](50) NULL,
	[categorycode] [varchar](6) NULL,
	[kflcode] [varchar](6) NULL,
	[packing] [float] NOT NULL,
	[strengthname] [varchar](6) NULL,
	[ml] [int] NOT NULL,
	[companycode] [varchar](6) NOT NULL,
	[barcode] [varchar](50) NULL,
	[salesrate] [float] NULL,
	[purchaseRate] [float] NULL,
	[purchase_discount_amount] [float] NOT NULL,
	[purchase_salestaxonmrp_amount] [float] NOT NULL,
	[purchase_taxscheme_amount] [float] NOT NULL,
	[min_qnty] [int] NULL,
 CONSTRAINT [PK_itemmst] PRIMARY KEY CLUSTERED 
(
	[companycode] ASC,
	[itemcode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)

ALTER TABLE [dbo].[itemmst] ADD  DEFAULT ((0)) FOR [purchase_discount_amount]
ALTER TABLE [dbo].[itemmst] ADD  DEFAULT ((0)) FOR [purchase_salestaxonmrp_amount]
ALTER TABLE [dbo].[itemmst] ADD  DEFAULT ((0)) FOR [purchase_taxscheme_amount]
ALTER TABLE [dbo].[itemmst]  WITH CHECK ADD  CONSTRAINT [FK_itemmst_categorymst] FOREIGN KEY([companycode], [categorycode])
REFERENCES [dbo].[categorymst] ([companycode], [categorycode])
ALTER TABLE [dbo].[itemmst] CHECK CONSTRAINT [FK_itemmst_categorymst]
ALTER TABLE [dbo].[itemmst]  WITH CHECK ADD  CONSTRAINT [FK_itemmst_companymast] FOREIGN KEY([companycode])
REFERENCES [dbo].[companymast] ([companycode])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[itemmst] CHECK CONSTRAINT [FK_itemmst_companymast]
ALTER TABLE [dbo].[itemmst]  WITH CHECK ADD  CONSTRAINT [FK_itemmst_groupmst] FOREIGN KEY([companycode], [groupcode])
REFERENCES [dbo].[groupmst] ([companycode], [groupcode])
ALTER TABLE [dbo].[itemmst] CHECK CONSTRAINT [FK_itemmst_groupmst]
ALTER TABLE [dbo].[itemmst]  WITH CHECK ADD  CONSTRAINT [FK_itemmst_Itemgroupml] FOREIGN KEY([companycode], [packing], [ml])
REFERENCES [dbo].[Itemgroupml] ([companycode], [packing], [ml])
ALTER TABLE [dbo].[itemmst] CHECK CONSTRAINT [FK_itemmst_Itemgroupml]
ALTER TABLE [dbo].[itemmst]  WITH CHECK ADD  CONSTRAINT [FK_itemmst_kflmst] FOREIGN KEY([companycode], [kflcode])
REFERENCES [dbo].[kflmst] ([companycode], [kflcode])
ALTER TABLE [dbo].[itemmst] CHECK CONSTRAINT [FK_itemmst_kflmst]
ALTER TABLE [dbo].[itemmst]  WITH CHECK ADD  CONSTRAINT [FK_itemmst_strength] FOREIGN KEY([companycode], [strengthname])
REFERENCES [dbo].[strength] ([companycode], [strengthname])
ALTER TABLE [dbo].[itemmst] CHECK CONSTRAINT [FK_itemmst_strength]
GO

-- ----- dbo.itemrateinfo -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[itemrateinfo](
	[ratecode] [varchar](6) NOT NULL,
	[ratename] [varchar](50) NULL,
	[companycode] [varchar](6) NOT NULL,
 CONSTRAINT [PK_itemrateinfo] PRIMARY KEY CLUSTERED 
(
	[companycode] ASC,
	[ratecode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)

ALTER TABLE [dbo].[itemrateinfo]  WITH CHECK ADD  CONSTRAINT [FK_itemrateinfo_companymast] FOREIGN KEY([companycode])
REFERENCES [dbo].[companymast] ([companycode])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[itemrateinfo] CHECK CONSTRAINT [FK_itemrateinfo_companymast]
GO

-- ----- dbo.itemratemst -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[itemratemst](
	[companycode] [varchar](6) NOT NULL,
	[ratecode] [varchar](6) NOT NULL,
	[itemcode] [varchar](6) NOT NULL,
	[salesrate] [float] NULL,
 CONSTRAINT [PK_itemratemst] PRIMARY KEY CLUSTERED 
(
	[companycode] ASC,
	[itemcode] ASC,
	[ratecode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)

ALTER TABLE [dbo].[itemratemst]  WITH CHECK ADD  CONSTRAINT [FK_itemratemst_itemrateinfo] FOREIGN KEY([companycode], [ratecode])
REFERENCES [dbo].[itemrateinfo] ([companycode], [ratecode])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[itemratemst] CHECK CONSTRAINT [FK_itemratemst_itemrateinfo]
GO

-- ----- dbo.journaldetail -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[journaldetail](
	[companycode] [varchar](6) NOT NULL,
	[yearcode] [varchar](6) NOT NULL,
	[trnno] [varchar](20) NOT NULL,
	[trndate] [datetime] NOT NULL,
	[slno] [int] NOT NULL,
	[to_by] [varchar](2) NOT NULL,
	[ledcode] [varchar](6) NOT NULL,
	[chqno] [varchar](6) NULL,
	[chqdate] [datetime] NULL,
	[accpay] [varchar](3) NULL,
	[debit] [float] NULL,
	[credit] [float] NULL,
	[type] [varchar](8) NOT NULL,
	[narr] [varchar](50) NULL,
	[trnid] [int] NOT NULL,
	[Currbal] [float] NULL,
 CONSTRAINT [PK_journaldetail] PRIMARY KEY CLUSTERED 
(
	[companycode] ASC,
	[yearcode] ASC,
	[trndate] ASC,
	[trnno] ASC,
	[slno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)

ALTER TABLE [dbo].[journaldetail]  WITH CHECK ADD  CONSTRAINT [FK_journaldetail_ledger] FOREIGN KEY([companycode], [ledcode])
REFERENCES [dbo].[ledger] ([companycode], [ledcode])
ALTER TABLE [dbo].[journaldetail] CHECK CONSTRAINT [FK_journaldetail_ledger]
GO

-- ----- dbo.kflmst -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[kflmst](
	[companycode] [varchar](6) NOT NULL,
	[kflcode] [varchar](6) NOT NULL,
	[kflname] [varchar](50) NULL,
 CONSTRAINT [PK_kflmst] PRIMARY KEY CLUSTERED 
(
	[companycode] ASC,
	[kflcode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)

ALTER TABLE [dbo].[kflmst]  WITH CHECK ADD  CONSTRAINT [FK_kflmst_companymast] FOREIGN KEY([companycode])
REFERENCES [dbo].[companymast] ([companycode])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[kflmst] CHECK CONSTRAINT [FK_kflmst_companymast]
GO

-- ----- dbo.ledger -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[ledger](
	[name] [varchar](50) NULL,
	[accode] [varchar](6) NOT NULL,
	[contperson] [varchar](50) NULL,
	[city] [varchar](50) NULL,
	[district] [varchar](50) NULL,
	[state] [varchar](50) NULL,
	[phone] [varchar](50) NULL,
	[email] [varchar](50) NULL,
	[www] [varchar](50) NULL,
	[address1] [varchar](50) NULL,
	[area] [varchar](50) NULL,
	[address2] [varchar](50) NULL,
	[debit] [float] NULL,
	[vatno] [varchar](50) NULL,
	[panno] [varchar](50) NULL,
	[ledcode] [varchar](6) NOT NULL,
	[companycode] [varchar](6) NOT NULL,
	[credit] [float] NULL,
	[type] [varchar](50) NULL,
	[ratecode] [varchar](6) NULL,
 CONSTRAINT [PK_ledger] PRIMARY KEY CLUSTERED 
(
	[companycode] ASC,
	[ledcode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)

ALTER TABLE [dbo].[ledger]  WITH CHECK ADD  CONSTRAINT [FK_ledger_acountname] FOREIGN KEY([companycode], [accode])
REFERENCES [dbo].[acountname] ([companycode], [accode])
ALTER TABLE [dbo].[ledger] CHECK CONSTRAINT [FK_ledger_acountname]
ALTER TABLE [dbo].[ledger]  WITH CHECK ADD  CONSTRAINT [FK_ledger_companymast] FOREIGN KEY([companycode])
REFERENCES [dbo].[companymast] ([companycode])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[ledger] CHECK CONSTRAINT [FK_ledger_companymast]
GO

-- ----- dbo.ledger_opening -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[ledger_opening](
	[companycode] [varchar](6) NOT NULL,
	[yearcode] [varchar](6) NULL,
	[ledcode] [varchar](6) NOT NULL,
	[debit] [float] NULL,
	[credit] [float] NULL,
	[type] [varchar](50) NULL,
	[stdate] [datetime] NULL
)

ALTER TABLE [dbo].[ledger_opening]  WITH CHECK ADD  CONSTRAINT [FK_ledger_opening_ledger] FOREIGN KEY([companycode], [ledcode])
REFERENCES [dbo].[ledger] ([companycode], [ledcode])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[ledger_opening] CHECK CONSTRAINT [FK_ledger_opening_ledger]
GO

-- ----- dbo.openingStockDetail -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[openingStockDetail](
	[companyCode] [varchar](6) NOT NULL,
	[yearCode] [varchar](6) NOT NULL,
	[trndate] [datetime] NULL,
	[shopCode] [varchar](6) NOT NULL,
	[batchNo] [varchar](50) NOT NULL,
	[itemCode] [varchar](6) NOT NULL,
	[loose] [int] NULL,
	[box] [int] NULL,
	[qnty] [int] NULL,
 CONSTRAINT [PK_openingStockDetail] PRIMARY KEY CLUSTERED 
(
	[companyCode] ASC,
	[yearCode] ASC,
	[shopCode] ASC,
	[batchNo] ASC,
	[itemCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)

ALTER TABLE [dbo].[openingStockDetail]  WITH CHECK ADD  CONSTRAINT [FK_openingStockDetail_openingstockmst] FOREIGN KEY([companyCode], [yearCode], [shopCode], [itemCode])
REFERENCES [dbo].[openingstockmst] ([companycode], [yearcode], [shopcode], [itemcode])
ALTER TABLE [dbo].[openingStockDetail] CHECK CONSTRAINT [FK_openingStockDetail_openingstockmst]
GO

-- ----- dbo.openingstockmst -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[openingstockmst](
	[shopcode] [varchar](6) NOT NULL,
	[companycode] [varchar](6) NOT NULL,
	[itemcode] [varchar](6) NOT NULL,
	[loose] [int] NULL,
	[box] [int] NULL,
	[qnty] [int] NULL,
	[yearcode] [varchar](6) NOT NULL,
	[trndate] [datetime] NULL,
 CONSTRAINT [com_kk] PRIMARY KEY CLUSTERED 
(
	[companycode] ASC,
	[yearcode] ASC,
	[shopcode] ASC,
	[itemcode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)

ALTER TABLE [dbo].[openingstockmst]  WITH CHECK ADD  CONSTRAINT [FK_openingstockmst_itemmst] FOREIGN KEY([companycode], [itemcode])
REFERENCES [dbo].[itemmst] ([companycode], [itemcode])
ALTER TABLE [dbo].[openingstockmst] CHECK CONSTRAINT [FK_openingstockmst_itemmst]
GO

-- ----- dbo.payment_detail -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[payment_detail](
	[companycode] [varchar](6) NOT NULL,
	[yearcode] [varchar](6) NOT NULL,
	[vchno] [int] NOT NULL,
	[trnno] [varchar](20) NOT NULL,
	[due_amount] [float] NULL,
	[debit] [float] NULL,
	[credit] [float] NULL,
	[vchdate] [datetime] NOT NULL,
	[supplierCode] [varchar](6) NULL,
 CONSTRAINT [PK_payment_detail] PRIMARY KEY CLUSTERED 
(
	[companycode] ASC,
	[yearcode] ASC,
	[vchdate] ASC,
	[vchno] ASC,
	[trnno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)

ALTER TABLE [dbo].[payment_detail]  WITH CHECK ADD  CONSTRAINT [FK_payment_detail_payment_main] FOREIGN KEY([companycode], [yearcode], [vchdate], [vchno])
REFERENCES [dbo].[payment_main] ([companycode], [yearcode], [vchdate], [vchno])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[payment_detail] CHECK CONSTRAINT [FK_payment_detail_payment_main]
ALTER TABLE [dbo].[payment_detail]  WITH NOCHECK ADD  CONSTRAINT [FK_payment_detail_purchasemain] FOREIGN KEY([companycode], [yearcode], [trnno])
REFERENCES [dbo].[purchasemain] ([companycode], [yearcode], [docno])
ALTER TABLE [dbo].[payment_detail] NOCHECK CONSTRAINT [FK_payment_detail_purchasemain]
GO

-- ----- dbo.payment_main -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[payment_main](
	[companycode] [varchar](6) NOT NULL,
	[yearcode] [varchar](6) NOT NULL,
	[vchno] [int] NOT NULL,
	[head_account] [varchar](6) NULL,
	[client_account] [varchar](6) NULL,
	[narration] [varchar](50) NULL,
	[vchdate] [datetime] NOT NULL,
	[cheque_no] [varchar](20) NULL,
	[due] [float] NULL,
	[amount_paid] [float] NULL,
	[discount] [float] NULL,
	[net_due] [float] NULL,
	[Discount_account_head] [varchar](6) NULL,
	[trnid] [int] NOT NULL,
	[chqDate] [datetime] NULL,
 CONSTRAINT [PK_payment_main] PRIMARY KEY CLUSTERED 
(
	[companycode] ASC,
	[yearcode] ASC,
	[vchdate] ASC,
	[vchno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)

ALTER TABLE [dbo].[payment_main] ADD  DEFAULT ((0)) FOR [trnid]
ALTER TABLE [dbo].[payment_main]  WITH NOCHECK ADD  CONSTRAINT [FK_payment_main_trnidmst] FOREIGN KEY([companycode], [yearcode], [trnid])
REFERENCES [dbo].[trnidmst] ([companycode], [yearcode], [trnid])
ALTER TABLE [dbo].[payment_main] CHECK CONSTRAINT [FK_payment_main_trnidmst]
GO

-- ----- dbo.purchasedetail -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[purchasedetail](
	[companycode] [varchar](6) NOT NULL,
	[yearcode] [varchar](6) NOT NULL,
	[trnid] [varchar](6) NULL,
	[slno] [numeric](3, 0) NULL,
	[itemcode] [varchar](6) NOT NULL,
	[batchno] [varchar](50) NOT NULL,
	[itemrate] [numeric](10, 2) NOT NULL,
	[itembox] [int] NULL,
	[itemloose] [int] NULL,
	[itemquantity] [int] NULL,
	[itemamount] [numeric](10, 2) NULL,
	[trnno] [int] NOT NULL,
	[itemfreeqnty] [int] NULL,
	[itemboxrate] [float] NULL,
	[itemmrp] [float] NULL,
	[itemdiscount] [float] NULL,
	[trndate] [datetime] NOT NULL,
 CONSTRAINT [PK_purchasedetail] PRIMARY KEY NONCLUSTERED 
(
	[companycode] ASC,
	[yearcode] ASC,
	[trnno] ASC,
	[itemcode] ASC,
	[batchno] ASC,
	[itemrate] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)

ALTER TABLE [dbo].[purchasedetail] ADD  DEFAULT ('4/1/2013') FOR [trndate]
ALTER TABLE [dbo].[purchasedetail]  WITH CHECK ADD  CONSTRAINT [FK_purchasedetail_itemmst] FOREIGN KEY([companycode], [itemcode])
REFERENCES [dbo].[itemmst] ([companycode], [itemcode])
ALTER TABLE [dbo].[purchasedetail] CHECK CONSTRAINT [FK_purchasedetail_itemmst]
ALTER TABLE [dbo].[purchasedetail]  WITH CHECK ADD  CONSTRAINT [FK_purchasedetail_purchasemain] FOREIGN KEY([companycode], [yearcode], [trnno])
REFERENCES [dbo].[purchasemain] ([companycode], [yearcode], [trnno])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[purchasedetail] CHECK CONSTRAINT [FK_purchasedetail_purchasemain]
GO

-- ----- dbo.purchasemain -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[purchasemain](
	[companycode] [varchar](6) NOT NULL,
	[yearcode] [varchar](6) NOT NULL,
	[trnid] [int] NOT NULL,
	[trnno] [int] NOT NULL,
	[trndate] [datetime] NOT NULL,
	[ptype] [varchar](50) NULL,
	[purchaseacccode] [varchar](6) NULL,
	[suppliercode] [varchar](6) NULL,
	[shopcode] [varchar](6) NULL,
	[docno] [varchar](20) NOT NULL,
	[docdate] [datetime] NULL,
	[tppassno] [varchar](25) NOT NULL,
	[schemecode] [varchar](6) NULL,
	[totamount] [numeric](18, 2) NULL,
	[tottaxOth] [numeric](18, 2) NULL,
	[totnetamt] [numeric](18, 2) NULL,
	[narration] [varchar](100) NULL,
	[checkedBy] [varchar](6) NULL,
	[saletax_including_free] [bit] NULL,
 CONSTRAINT [PK_purchasemain] PRIMARY KEY CLUSTERED 
(
	[companycode] ASC,
	[yearcode] ASC,
	[trnno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF),
 CONSTRAINT [IX_purchasemain] UNIQUE NONCLUSTERED 
(
	[companycode] ASC,
	[yearcode] ASC,
	[docno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)

ALTER TABLE [dbo].[purchasemain] ADD  DEFAULT ((0)) FOR [saletax_including_free]
ALTER TABLE [dbo].[purchasemain]  WITH CHECK ADD  CONSTRAINT [FK_purchasemain_ledger_purchaseacccode] FOREIGN KEY([companycode], [purchaseacccode])
REFERENCES [dbo].[ledger] ([companycode], [ledcode])
ALTER TABLE [dbo].[purchasemain] CHECK CONSTRAINT [FK_purchasemain_ledger_purchaseacccode]
ALTER TABLE [dbo].[purchasemain]  WITH CHECK ADD  CONSTRAINT [FK_purchasemain_ledger_suppliercode] FOREIGN KEY([companycode], [suppliercode])
REFERENCES [dbo].[ledger] ([companycode], [ledcode])
ALTER TABLE [dbo].[purchasemain] CHECK CONSTRAINT [FK_purchasemain_ledger_suppliercode]
ALTER TABLE [dbo].[purchasemain]  WITH CHECK ADD  CONSTRAINT [FK_purchasemain_storage] FOREIGN KEY([companycode], [shopcode])
REFERENCES [dbo].[storage] ([companycode], [shopcode])
ALTER TABLE [dbo].[purchasemain] CHECK CONSTRAINT [FK_purchasemain_storage]
ALTER TABLE [dbo].[purchasemain]  WITH NOCHECK ADD  CONSTRAINT [FK_purchasemain_trnidmst] FOREIGN KEY([companycode], [yearcode], [trnid])
REFERENCES [dbo].[trnidmst] ([companycode], [yearcode], [trnid])
ALTER TABLE [dbo].[purchasemain] CHECK CONSTRAINT [FK_purchasemain_trnidmst]
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'dd/MM/yyyy' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'purchasemain', @level2type=N'COLUMN',@level2name=N'trndate'
GO

-- ----- dbo.PurchaseTaxDetail -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[PurchaseTaxDetail](
	[companycode] [varchar](6) NULL,
	[trnno] [int] NOT NULL,
	[schemecode] [varchar](6) NULL,
	[TaxCode] [varchar](6) NULL,
	[TaxRate] [numeric](18, 2) NULL,
	[OnAmount] [numeric](18, 2) NULL,
	[TaxAmount] [numeric](18, 2) NULL,
	[TaxAccount] [varchar](6) NULL,
	[yearcode] [varchar](6) NULL
)

ALTER TABLE [dbo].[PurchaseTaxDetail] ADD  CONSTRAINT [DF_PurchaseTaxDetail_TaxCode]  DEFAULT ('') FOR [TaxCode]
ALTER TABLE [dbo].[PurchaseTaxDetail] ADD  CONSTRAINT [DF_PurchaseTaxDetail_TaxRate]  DEFAULT ((0)) FOR [TaxRate]
ALTER TABLE [dbo].[PurchaseTaxDetail] ADD  CONSTRAINT [DF_PurchaseTaxDetail_OnAmount]  DEFAULT ((0)) FOR [OnAmount]
ALTER TABLE [dbo].[PurchaseTaxDetail] ADD  CONSTRAINT [DF_PurchaseTaxDetail_TaxAmount]  DEFAULT ((0)) FOR [TaxAmount]
ALTER TABLE [dbo].[PurchaseTaxDetail] ADD  CONSTRAINT [DF_PurchaseTaxDetail_TaxAccount]  DEFAULT ('') FOR [TaxAccount]
ALTER TABLE [dbo].[PurchaseTaxDetail] ADD  CONSTRAINT [DF_PurchaseTaxDetail_itmCode]  DEFAULT ('') FOR [yearcode]
ALTER TABLE [dbo].[PurchaseTaxDetail]  WITH CHECK ADD  CONSTRAINT [FK_PurchaseTaxDetail_purchasemain] FOREIGN KEY([companycode], [yearcode], [trnno])
REFERENCES [dbo].[purchasemain] ([companycode], [yearcode], [trnno])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[PurchaseTaxDetail] CHECK CONSTRAINT [FK_PurchaseTaxDetail_purchasemain]
ALTER TABLE [dbo].[PurchaseTaxDetail]  WITH CHECK ADD  CONSTRAINT [FK_PurchaseTaxDetail_taxdetail] FOREIGN KEY([companycode], [schemecode], [TaxCode])
REFERENCES [dbo].[taxdetail] ([companycode], [SchemeCode], [TaxCode])
ON UPDATE CASCADE
ALTER TABLE [dbo].[PurchaseTaxDetail] CHECK CONSTRAINT [FK_PurchaseTaxDetail_taxdetail]
GO

-- ----- dbo.receipt_detail -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[receipt_detail](
	[companycode] [varchar](6) NOT NULL,
	[yearcode] [varchar](6) NOT NULL,
	[vchno] [int] NOT NULL,
	[trnno] [varchar](20) NOT NULL,
	[due_amount] [float] NULL,
	[debit] [float] NULL,
	[credit] [float] NULL,
	[vchdate] [datetime] NOT NULL,
 CONSTRAINT [PK_receipt_detail] PRIMARY KEY CLUSTERED 
(
	[companycode] ASC,
	[yearcode] ASC,
	[vchdate] ASC,
	[vchno] ASC,
	[trnno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)

ALTER TABLE [dbo].[receipt_detail]  WITH CHECK ADD  CONSTRAINT [FK_receipt_detail_receipt_main] FOREIGN KEY([companycode], [yearcode], [vchdate], [vchno])
REFERENCES [dbo].[receipt_main] ([companycode], [yearcode], [vchdate], [vchno])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[receipt_detail] CHECK CONSTRAINT [FK_receipt_detail_receipt_main]
ALTER TABLE [dbo].[receipt_detail]  WITH NOCHECK ADD  CONSTRAINT [FK_receipt_detail_salesbillmain] FOREIGN KEY([companycode], [yearcode], [trnno])
REFERENCES [dbo].[salesbillmain] ([companycode], [yearcode], [trnno])
ON UPDATE CASCADE
ALTER TABLE [dbo].[receipt_detail] NOCHECK CONSTRAINT [FK_receipt_detail_salesbillmain]
GO

-- ----- dbo.receipt_main -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[receipt_main](
	[companycode] [varchar](6) NOT NULL,
	[yearcode] [varchar](6) NOT NULL,
	[vchno] [int] NOT NULL,
	[head_account] [varchar](6) NULL,
	[client_account] [varchar](6) NULL,
	[narration] [varchar](50) NULL,
	[vchdate] [datetime] NOT NULL,
	[cheque_no] [varchar](20) NULL,
	[due] [float] NULL,
	[amount_paid] [float] NULL,
	[discount] [float] NULL,
	[net_due] [float] NULL,
	[Discount_account_head] [varchar](6) NULL,
	[receipt_type] [varchar](15) NULL,
	[trnid] [int] NOT NULL,
	[chqDate] [datetime] NULL,
 CONSTRAINT [PK_receipt_main] PRIMARY KEY CLUSTERED 
(
	[companycode] ASC,
	[yearcode] ASC,
	[vchdate] ASC,
	[vchno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)

ALTER TABLE [dbo].[receipt_main] ADD  DEFAULT ((0)) FOR [trnid]
ALTER TABLE [dbo].[receipt_main]  WITH CHECK ADD  CONSTRAINT [FK_receipt_main_ledger_clientac] FOREIGN KEY([companycode], [client_account])
REFERENCES [dbo].[ledger] ([companycode], [ledcode])
ALTER TABLE [dbo].[receipt_main] CHECK CONSTRAINT [FK_receipt_main_ledger_clientac]
ALTER TABLE [dbo].[receipt_main]  WITH CHECK ADD  CONSTRAINT [FK_receipt_main_ledger_discac] FOREIGN KEY([companycode], [Discount_account_head])
REFERENCES [dbo].[ledger] ([companycode], [ledcode])
ALTER TABLE [dbo].[receipt_main] CHECK CONSTRAINT [FK_receipt_main_ledger_discac]
ALTER TABLE [dbo].[receipt_main]  WITH CHECK ADD  CONSTRAINT [FK_receipt_main_ledger_headac] FOREIGN KEY([companycode], [head_account])
REFERENCES [dbo].[ledger] ([companycode], [ledcode])
ALTER TABLE [dbo].[receipt_main] CHECK CONSTRAINT [FK_receipt_main_ledger_headac]
ALTER TABLE [dbo].[receipt_main]  WITH NOCHECK ADD  CONSTRAINT [FK_receipt_main_trnidmst] FOREIGN KEY([companycode], [yearcode], [trnid])
REFERENCES [dbo].[trnidmst] ([companycode], [yearcode], [trnid])
ALTER TABLE [dbo].[receipt_main] CHECK CONSTRAINT [FK_receipt_main_trnidmst]
GO

-- ----- dbo.reportTaggingDetail -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[reportTaggingDetail](
	[reportName] [varchar](50) NULL,
	[reportCategory] [varchar](50) NULL,
	[groupname] [varchar](50) NULL,
	[companycode] [varchar](6) NULL
)

GO

-- ----- dbo.reportTaggingMain -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[reportTaggingMain](
	[reportName] [varchar](50) NULL,
	[reportCategory] [varchar](50) NULL,
	[companycode] [varchar](6) NULL
)

GO

-- ----- dbo.sales_tax -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[sales_tax](
	[trnno] [varchar](20) NOT NULL,
	[companycode] [varchar](6) NOT NULL,
	[yearcode] [varchar](6) NOT NULL,
	[cash_bank_book] [varchar](6) NULL,
	[tax_book] [varchar](6) NOT NULL,
	[tax_amount] [float] NULL,
	[date] [datetime] NULL,
 CONSTRAINT [PK_sales_tax] PRIMARY KEY CLUSTERED 
(
	[companycode] ASC,
	[yearcode] ASC,
	[trnno] ASC,
	[tax_book] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)

ALTER TABLE [dbo].[sales_tax]  WITH CHECK ADD  CONSTRAINT [FK_sales_tax_salesbillmain] FOREIGN KEY([companycode], [yearcode], [trnno])
REFERENCES [dbo].[salesbillmain] ([companycode], [yearcode], [trnno])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[sales_tax] CHECK CONSTRAINT [FK_sales_tax_salesbillmain]
GO

-- ----- dbo.salesbilldetail -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[salesbilldetail](
	[companycode] [varchar](6) NOT NULL,
	[itemcode] [varchar](6) NOT NULL,
	[tot_box] [int] NULL,
	[loose] [int] NULL,
	[qnty] [int] NULL,
	[itemAmount] [float] NULL,
	[trnno] [varchar](20) NOT NULL,
	[rate] [float] NULL,
	[trndate] [datetime] NULL,
	[yearcode] [varchar](6) NOT NULL,
	[large] [int] NULL,
	[small] [int] NULL,
	[westage] [int] NULL,
	[MRP] [float] NOT NULL,
	[batchDate] [datetime] NOT NULL,
	[batchNo] [varchar](50) NOT NULL,
 CONSTRAINT [PK_salesbilldetail] PRIMARY KEY CLUSTERED 
(
	[companycode] ASC,
	[yearcode] ASC,
	[trnno] ASC,
	[batchDate] ASC,
	[batchNo] ASC,
	[itemcode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)

ALTER TABLE [dbo].[salesbilldetail] ADD  DEFAULT ((0)) FOR [MRP]
ALTER TABLE [dbo].[salesbilldetail] ADD  DEFAULT (((1)/(1))/(2013)) FOR [batchDate]
ALTER TABLE [dbo].[salesbilldetail] ADD  DEFAULT ((0)) FOR [batchNo]
ALTER TABLE [dbo].[salesbilldetail]  WITH CHECK ADD  CONSTRAINT [FK_salesbilldetail_itemmst] FOREIGN KEY([companycode], [itemcode])
REFERENCES [dbo].[itemmst] ([companycode], [itemcode])
ALTER TABLE [dbo].[salesbilldetail] CHECK CONSTRAINT [FK_salesbilldetail_itemmst]
ALTER TABLE [dbo].[salesbilldetail]  WITH NOCHECK ADD  CONSTRAINT [FK_salesbilldetail_salesbillmain] FOREIGN KEY([companycode], [yearcode], [trnno])
REFERENCES [dbo].[salesbillmain] ([companycode], [yearcode], [trnno])
ON UPDATE CASCADE
NOT FOR REPLICATION 
ALTER TABLE [dbo].[salesbilldetail] NOCHECK CONSTRAINT [FK_salesbilldetail_salesbillmain]
GO

-- ----- dbo.salesbillmain -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[salesbillmain](
	[companycode] [varchar](6) NOT NULL,
	[trnno] [varchar](20) NOT NULL,
	[trndate] [datetime] NOT NULL,
	[ledgercode] [varchar](6) NOT NULL,
	[amount] [float] NULL,
	[salestype] [varchar](20) NULL,
	[ratecode] [varchar](6) NULL,
	[storecode] [varchar](6) NULL,
	[yearcode] [varchar](6) NOT NULL,
	[discamount] [float] NULL,
	[netamount] [float] NULL,
	[account_head] [varchar](6) NULL,
	[discount_account_head] [varchar](6) NULL,
	[trntime] [datetime] NULL,
	[trnid] [int] NOT NULL,
	[discount_reason] [varchar](50) NULL,
	[KFL] [varchar](60) NOT NULL,
 CONSTRAINT [PK_salesbillmain] PRIMARY KEY CLUSTERED 
(
	[companycode] ASC,
	[yearcode] ASC,
	[trnno] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)

ALTER TABLE [dbo].[salesbillmain] ADD  DEFAULT ((0)) FOR [trnid]
ALTER TABLE [dbo].[salesbillmain] ADD  DEFAULT ('none') FOR [KFL]
ALTER TABLE [dbo].[salesbillmain]  WITH CHECK ADD  CONSTRAINT [FK_salesbillmain_companyparamtr_discntacchead] FOREIGN KEY([companycode], [discount_account_head])
REFERENCES [dbo].[companyparamtr] ([companycode], [discntacc])
ON UPDATE CASCADE
ALTER TABLE [dbo].[salesbillmain] CHECK CONSTRAINT [FK_salesbillmain_companyparamtr_discntacchead]
ALTER TABLE [dbo].[salesbillmain]  WITH CHECK ADD  CONSTRAINT [FK_salesbillmain_itemrateinfo] FOREIGN KEY([companycode], [ratecode])
REFERENCES [dbo].[itemrateinfo] ([companycode], [ratecode])
ALTER TABLE [dbo].[salesbillmain] CHECK CONSTRAINT [FK_salesbillmain_itemrateinfo]
ALTER TABLE [dbo].[salesbillmain]  WITH CHECK ADD  CONSTRAINT [FK_salesbillmain_ledger_acount_head] FOREIGN KEY([companycode], [account_head])
REFERENCES [dbo].[ledger] ([companycode], [ledcode])
ALTER TABLE [dbo].[salesbillmain] CHECK CONSTRAINT [FK_salesbillmain_ledger_acount_head]
ALTER TABLE [dbo].[salesbillmain]  WITH CHECK ADD  CONSTRAINT [FK_salesbillmain_ledger_discount_account] FOREIGN KEY([companycode], [discount_account_head])
REFERENCES [dbo].[ledger] ([companycode], [ledcode])
ALTER TABLE [dbo].[salesbillmain] CHECK CONSTRAINT [FK_salesbillmain_ledger_discount_account]
ALTER TABLE [dbo].[salesbillmain]  WITH CHECK ADD  CONSTRAINT [FK_salesbillmain_ledger_ledgercode] FOREIGN KEY([companycode], [ledgercode])
REFERENCES [dbo].[ledger] ([companycode], [ledcode])
ALTER TABLE [dbo].[salesbillmain] CHECK CONSTRAINT [FK_salesbillmain_ledger_ledgercode]
ALTER TABLE [dbo].[salesbillmain]  WITH CHECK ADD  CONSTRAINT [FK_salesbillmain_storage] FOREIGN KEY([companycode], [storecode])
REFERENCES [dbo].[storage] ([companycode], [shopcode])
ALTER TABLE [dbo].[salesbillmain] CHECK CONSTRAINT [FK_salesbillmain_storage]
ALTER TABLE [dbo].[salesbillmain]  WITH NOCHECK ADD  CONSTRAINT [FK_salesbillmain_trnidmst] FOREIGN KEY([companycode], [yearcode], [trnid])
REFERENCES [dbo].[trnidmst] ([companycode], [yearcode], [trnid])
ALTER TABLE [dbo].[salesbillmain] CHECK CONSTRAINT [FK_salesbillmain_trnidmst]
GO

-- ----- dbo.stk_transfer_detail -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[stk_transfer_detail](
	[CompanyCode] [varchar](6) NOT NULL,
	[ItemCode] [varchar](6) NOT NULL,
	[box] [int] NULL,
	[loose] [int] NULL,
	[qnty] [int] NULL,
	[trnno] [int] NOT NULL,
	[trndate] [datetime] NOT NULL,
	[yearcode] [varchar](6) NOT NULL,
	[batchDate] [datetime] NOT NULL,
	[batchNo] [varchar](50) NOT NULL,
 CONSTRAINT [PK_stk_transfer_detail] PRIMARY KEY CLUSTERED 
(
	[CompanyCode] ASC,
	[yearcode] ASC,
	[trnno] ASC,
	[batchDate] ASC,
	[batchNo] ASC,
	[ItemCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)

ALTER TABLE [dbo].[stk_transfer_detail] ADD  DEFAULT (((1)/(1))/(2013)) FOR [batchDate]
ALTER TABLE [dbo].[stk_transfer_detail] ADD  DEFAULT ((0)) FOR [batchNo]
ALTER TABLE [dbo].[stk_transfer_detail]  WITH CHECK ADD  CONSTRAINT [FK_stk_transfer_detail_itemmst] FOREIGN KEY([CompanyCode], [ItemCode])
REFERENCES [dbo].[itemmst] ([companycode], [itemcode])
ALTER TABLE [dbo].[stk_transfer_detail] CHECK CONSTRAINT [FK_stk_transfer_detail_itemmst]
ALTER TABLE [dbo].[stk_transfer_detail]  WITH CHECK ADD  CONSTRAINT [FK_stk_transfer_detail_stk_transfer_main] FOREIGN KEY([CompanyCode], [yearcode], [trndate], [trnno])
REFERENCES [dbo].[stk_transfer_main] ([CompanyCode], [YearCode], [TrnDate], [TrnNo])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[stk_transfer_detail] CHECK CONSTRAINT [FK_stk_transfer_detail_stk_transfer_main]
GO

-- ----- dbo.stk_transfer_main -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[stk_transfer_main](
	[CompanyCode] [varchar](6) NOT NULL,
	[YearCode] [varchar](6) NOT NULL,
	[TrnNo] [int] NOT NULL,
	[TrnDate] [datetime] NOT NULL,
	[shopname_from] [varchar](50) NULL,
	[shopname_to] [varchar](50) NULL,
	[shopcode_frm] [varchar](6) NULL,
	[shopcode_to] [varchar](6) NULL,
	[trnid] [int] NOT NULL,
 CONSTRAINT [PK_stk_transfer_main] PRIMARY KEY CLUSTERED 
(
	[CompanyCode] ASC,
	[YearCode] ASC,
	[TrnDate] ASC,
	[TrnNo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)

ALTER TABLE [dbo].[stk_transfer_main] ADD  DEFAULT ((0)) FOR [trnid]
ALTER TABLE [dbo].[stk_transfer_main]  WITH NOCHECK ADD  CONSTRAINT [FK_stk_transfer_main_trnidmst] FOREIGN KEY([CompanyCode], [YearCode], [trnid])
REFERENCES [dbo].[trnidmst] ([companycode], [yearcode], [trnid])
ALTER TABLE [dbo].[stk_transfer_main] CHECK CONSTRAINT [FK_stk_transfer_main_trnidmst]
GO

-- ----- dbo.storage -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[storage](
	[companycode] [varchar](6) NOT NULL,
	[shopcode] [varchar](6) NOT NULL,
	[shopname] [varchar](50) NULL,
	[address] [varchar](50) NULL,
	[ShopType] [varchar](3) NULL,
 CONSTRAINT [PK_storage] PRIMARY KEY CLUSTERED 
(
	[companycode] ASC,
	[shopcode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)

ALTER TABLE [dbo].[storage]  WITH CHECK ADD  CONSTRAINT [FK_storage_companymast] FOREIGN KEY([companycode])
REFERENCES [dbo].[companymast] ([companycode])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[storage] CHECK CONSTRAINT [FK_storage_companymast]
GO

-- ----- dbo.strength -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[strength](
	[strengthname] [varchar](6) NOT NULL,
	[companycode] [varchar](6) NOT NULL,
 CONSTRAINT [PK_strength] PRIMARY KEY CLUSTERED 
(
	[companycode] ASC,
	[strengthname] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)

ALTER TABLE [dbo].[strength] ADD  DEFAULT ((0)) FOR [companycode]
ALTER TABLE [dbo].[strength]  WITH CHECK ADD  CONSTRAINT [FK_strength_companymast] FOREIGN KEY([companycode])
REFERENCES [dbo].[companymast] ([companycode])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[strength] CHECK CONSTRAINT [FK_strength_companymast]
GO

-- ----- dbo.taxdetail -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[taxdetail](
	[companycode] [varchar](6) NOT NULL,
	[SchemeCode] [varchar](6) NOT NULL,
	[schemename] [varchar](50) NULL,
	[SrNo] [int] NULL,
	[TaxCode] [varchar](6) NOT NULL,
	[taxname] [varchar](50) NULL,
	[ledcode] [varchar](6) NULL,
	[sig] [varchar](1) NULL,
	[TaxRate] [float] NULL,
	[taxamount] [float] NULL,
	[calc_on] [varchar](1) NULL,
	[base] [int] NULL,
	[after_discount] [bit] NOT NULL,
	[after_saletax] [bit] NOT NULL,
 CONSTRAINT [PK_taxdetail] PRIMARY KEY CLUSTERED 
(
	[companycode] ASC,
	[SchemeCode] ASC,
	[TaxCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)

ALTER TABLE [dbo].[taxdetail] ADD  CONSTRAINT [DF_nwTaxDetail_TaxCode]  DEFAULT ((0)) FOR [TaxCode]
ALTER TABLE [dbo].[taxdetail] ADD  CONSTRAINT [DF_nwTaxDetail_AcCode]  DEFAULT ('') FOR [ledcode]
ALTER TABLE [dbo].[taxdetail] ADD  CONSTRAINT [DF_nwTaxDetail_TaxRate]  DEFAULT ((0)) FOR [TaxRate]
ALTER TABLE [dbo].[taxdetail] ADD  DEFAULT ((0)) FOR [after_discount]
ALTER TABLE [dbo].[taxdetail] ADD  DEFAULT ((0)) FOR [after_saletax]
ALTER TABLE [dbo].[taxdetail]  WITH CHECK ADD  CONSTRAINT [FK_taxdetail_companymast] FOREIGN KEY([companycode])
REFERENCES [dbo].[companymast] ([companycode])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[taxdetail] CHECK CONSTRAINT [FK_taxdetail_companymast]
ALTER TABLE [dbo].[taxdetail]  WITH CHECK ADD  CONSTRAINT [FK_taxdetail_ledger] FOREIGN KEY([companycode], [ledcode])
REFERENCES [dbo].[ledger] ([companycode], [ledcode])
ALTER TABLE [dbo].[taxdetail] CHECK CONSTRAINT [FK_taxdetail_ledger]
GO

-- ----- dbo.TEMP -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[TEMP](
	[companyCode] [varchar](6) NOT NULL,
	[yearCode] [varchar](6) NOT NULL,
	[kflName] [varchar](40) NOT NULL,
	[type] [varchar](20) NOT NULL,
	[groupName] [varchar](50) NOT NULL,
	[categoryName] [varchar](50) NOT NULL,
	[ml] [int] NOT NULL,
	[qnty] [float] NOT NULL,
	[batchno] [varchar](50) NOT NULL,
	[party] [varchar](100) NOT NULL,
	[billno] [varchar](50) NOT NULL,
	[tppassno] [varchar](50) NOT NULL,
	[trndate] [datetime] NOT NULL,
 CONSTRAINT [PK_TEMP] PRIMARY KEY CLUSTERED 
(
	[companyCode] ASC,
	[yearCode] ASC,
	[trndate] ASC,
	[billno] ASC,
	[batchno] ASC,
	[categoryName] ASC,
	[groupName] ASC,
	[kflName] ASC,
	[ml] ASC,
	[party] ASC,
	[qnty] ASC,
	[tppassno] ASC,
	[type] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)

ALTER TABLE [dbo].[TEMP] ADD  CONSTRAINT [DF_TEMP_BATCH]  DEFAULT ('') FOR [batchno]
GO

-- ----- dbo.tempitem -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[tempitem](
	[item] [nvarchar](225) NULL
)

GO

-- ----- dbo.templedger -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[templedger](
	[ledname] [nvarchar](225) NULL
)

GO

-- ----- dbo.trnidmst -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[trnidmst](
	[companycode] [varchar](6) NOT NULL,
	[yearcode] [varchar](6) NOT NULL,
	[trnid] [int] NOT NULL,
	[trn_type] [varchar](30) NOT NULL,
	[usercode] [varchar](6) NULL,
 CONSTRAINT [PK_trnidmst] PRIMARY KEY CLUSTERED 
(
	[companycode] ASC,
	[yearcode] ASC,
	[trnid] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)

ALTER TABLE [dbo].[trnidmst] ADD  DEFAULT ('old') FOR [trn_type]
ALTER TABLE [dbo].[trnidmst]  WITH CHECK ADD  CONSTRAINT [FK_trnidmst_users] FOREIGN KEY([usercode])
REFERENCES [dbo].[users] ([usercode])
ALTER TABLE [dbo].[trnidmst] CHECK CONSTRAINT [FK_trnidmst_users]
GO

-- ----- dbo.userrights -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[userrights](
	[usercode] [varchar](6) NOT NULL,
	[companycode] [varchar](6) NOT NULL,
	[accaccess] [varchar](1) NULL,
	[modulename] [varchar](50) NOT NULL,
	[perv] [varchar](5) NULL,
	[pera] [varchar](5) NULL,
	[pere] [varchar](5) NULL,
	[perd] [varchar](5) NULL,
 CONSTRAINT [PK_userrights] PRIMARY KEY CLUSTERED 
(
	[usercode] ASC,
	[companycode] ASC,
	[modulename] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)

ALTER TABLE [dbo].[userrights]  WITH CHECK ADD  CONSTRAINT [FK_userrights_companymast] FOREIGN KEY([companycode])
REFERENCES [dbo].[companymast] ([companycode])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[userrights] CHECK CONSTRAINT [FK_userrights_companymast]
ALTER TABLE [dbo].[userrights]  WITH CHECK ADD  CONSTRAINT [FK_userrights_users] FOREIGN KEY([usercode])
REFERENCES [dbo].[users] ([usercode])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[userrights] CHECK CONSTRAINT [FK_userrights_users]
GO

-- ----- dbo.users -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[users](
	[usercode] [varchar](6) NOT NULL,
	[username] [varchar](50) NOT NULL,
	[accesslevel] [varchar](6) NULL,
	[password] [varchar](10) NULL,
 CONSTRAINT [PK_users] PRIMARY KEY CLUSTERED 
(
	[usercode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)

SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
create trigger [dbo].[userdel]
on [dbo].[users]
for delete as 
begin
declare @u varchar(6)
select @u=usercode from deleted
if @u='A00001'
begin
rollback transaction
end
end

ALTER TABLE [dbo].[users] ENABLE TRIGGER [userdel]
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

create trigger [dbo].[userrights_del]
on [dbo].[users]
for delete
as
begin
declare @n varchar(50)
select @n=usercode from deleted
delete from userrights where usercode=@n
end
ALTER TABLE [dbo].[users] ENABLE TRIGGER [userrights_del]
GO

-- ----- dbo.yearmst -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE TABLE [dbo].[yearmst](
	[yearcode] [varchar](6) NOT NULL,
	[yearrange] [varchar](9) NULL,
	[stdate] [datetime] NULL,
	[enddate] [datetime] NULL,
	[companycode] [varchar](6) NOT NULL,
 CONSTRAINT [PK_yearmst] PRIMARY KEY CLUSTERED 
(
	[companycode] ASC,
	[yearcode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF)
)

ALTER TABLE [dbo].[yearmst]  WITH CHECK ADD  CONSTRAINT [FK_yearmst_companymast] FOREIGN KEY([companycode])
REFERENCES [dbo].[companymast] ([companycode])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[yearmst] CHECK CONSTRAINT [FK_yearmst_companymast]
GO

-- ============================================================
-- 4. VIEWS
-- ============================================================
-- ----- dbo.stock -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE view [dbo].[stock] with schemabinding as select itemcode,itemname,sum(bottles) as stock,companycode,storecode,yearcode,packing from (/********for adding the qunatities in the stock******/ select dbo.itemmst.itemcode,itemname,sum(qnty) as bottles,dbo.itemmst.companycode,shopcode as storecode,yearcode,packing from dbo.openingstockmst join dbo.itemmst on dbo.openingstockmst.companycode=dbo.itemmst.companycode and dbo.openingstockmst.itemcode=dbo.itemmst.itemcode group by dbo.itemmst.companycode,yearcode,shopcode,dbo.itemmst.itemcode,dbo.itemmst.itemname,packing union all select dbo.itemmst.itemcode,dbo.itemmst.itemname,(-1)*sum(qnty) as bottles,dbo.salesbillmain.companycode,storecode,dbo.salesbillmain.yearcode,packing from dbo.salesbillmain join dbo.salesbilldetail on dbo.salesbillmain.companycode=dbo.salesbilldetail.companycode and dbo.salesbillmain.yearcode=dbo.salesbilldetail.yearcode and dbo.salesbillmain.trnno=dbo.salesbilldetail.trnno join dbo.itemmst on dbo.salesbilldetail.companycode=dbo.itemmst.companycode and dbo.itemmst.itemcode=dbo.salesbilldetail.itemcode where salestype='cash' or salestype='credit card' or salestype ='credit account' group by dbo.salesbillmain.companycode,dbo.salesbillmain.yearcode,storecode,dbo.itemmst.itemcode,dbo.itemmst.itemname,packing union all select dbo.itemmst.itemcode,dbo.itemmst.itemname,sum(qnty) as bottles,dbo.salesbillmain.companycode,storecode,dbo.salesbillmain.yearcode,packing from dbo.salesbillmain join dbo.salesbilldetail on dbo.salesbillmain.companycode=dbo.salesbilldetail.companycode and dbo.salesbillmain.yearcode=dbo.salesbilldetail.yearcode and dbo.salesbillmain.trnno=dbo.salesbilldetail.trnno join dbo.itemmst on dbo.salesbilldetail.companycode=dbo.itemmst.companycode and dbo.itemmst.itemcode=dbo.salesbilldetail.itemcode where salestype='return' group by dbo.salesbillmain.companycode,dbo.salesbillmain.yearcode,storecode,dbo.itemmst.itemcode,dbo.itemmst.itemname,packing union all select dbo.itemmst.itemcode,dbo.itemmst.itemname,sum(itemquantity) as bottles,dbo.purchasemain.companycode,shopcode as storecode,dbo.purchasemain.yearcode,packing  from dbo.purchasemain join dbo.purchasedetail on dbo.purchasedetail.companycode=dbo.purchasemain.companycode and dbo.purchasedetail.yearcode=dbo.purchasemain.yearcode and dbo.purchasedetail.trnno=dbo.purchasemain.trnno  join dbo.itemmst on dbo.itemmst.companycode=dbo.purchasedetail.companycode and dbo.itemmst.itemcode=dbo.purchasedetail.itemcode where ptype='PURCHASE' group by dbo.purchasemain.companycode,dbo.purchasemain.yearcode,shopcode,dbo.itemmst.itemcode,dbo.itemmst.itemname,packing union all select dbo.itemmst.itemcode,dbo.itemmst.itemname,-1*sum(itemquantity) as bottles,dbo.purchasemain.companycode,shopcode as storecode,dbo.purchasemain.yearcode,packing from dbo.purchasemain join dbo.purchasedetail on dbo.purchasedetail.companycode=dbo.purchasemain.companycode and dbo.purchasedetail.yearcode=dbo.purchasemain.yearcode and dbo.purchasedetail.trnno=dbo.purchasemain.trnno join dbo.itemmst on dbo.itemmst.companycode=dbo.purchasedetail.companycode and dbo.itemmst.itemcode=dbo.purchasedetail.itemcode where ptype='PURCHASE RETURN' group by dbo.purchasemain.companycode,dbo.purchasemain.yearcode,shopcode,dbo.itemmst.itemcode,dbo.itemmst.itemname,packing union all /********for deducting the qunatities form the storage of the stock from where the items has been sold ******/ select dbo.itemmst.itemcode,itemname,(-1)*sum(qnty) as bottles,dbo.stk_transfer_detail.companycode,shopcode_frm as storecode,dbo.stk_transfer_detail.yearcode,packing from dbo.stk_transfer_detail join dbo.itemmst on dbo.stk_transfer_detail.companycode=dbo.itemmst.companycode and dbo.itemmst.itemcode=dbo.stk_transfer_detail.itemcode join dbo.stk_transfer_main on dbo.stk_transfer_main.companycode=dbo.stk_transfer_detail.companycode and dbo.stk_transfer_main.yearcode=dbo.stk_transfer_detail.yearcode and dbo.stk_transfer_main.trnno=dbo.stk_transfer_detail.trnno group by dbo.stk_transfer_detail.companycode,dbo.stk_transfer_detail.yearcode,shopcode_frm,dbo.itemmst.itemcode,dbo.itemmst.itemname,packing union all /********for adding the qunatities to the storage of the stock  where the items has been sold ******/ select dbo.itemmst.itemcode,itemname,sum(qnty) as bottles,dbo.stk_transfer_detail.companycode,shopcode_to as storecode,dbo.stk_transfer_detail.yearcode,packing from dbo.stk_transfer_detail join dbo.itemmst on dbo.stk_transfer_detail.companycode=dbo.itemmst.companycode and dbo.itemmst.itemcode=dbo.stk_transfer_detail.itemcode join dbo.stk_transfer_main on dbo.stk_transfer_main.companycode=dbo.stk_transfer_detail.companycode and dbo.stk_transfer_main.yearcode=dbo.stk_transfer_detail.yearcode and dbo.stk_transfer_main.trnno=dbo.stk_transfer_detail.trnno group by dbo.stk_transfer_detail.companycode,dbo.stk_transfer_detail.yearcode,shopcode_to,dbo.itemmst.itemcode,dbo.itemmst.itemname,packing )as x group by companycode,yearcode,storecode,itemcode,itemname,packing 
GO

-- ----- dbo.sale_purchase_daywise -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE view [dbo].[sale_purchase_daywise]
as
select itemcode,itemname,sum(bottles_purchased) as bottles_purchased,sum(bottles_sold) as bottles_sold,companycode,storecode,yearcode,trndate
from(
SELECT     itemmst.itemcode, itemmst.itemname,0 as bottles_purchased, SUM(salesbilldetail.qnty) AS bottles_sold, salesbillmain.companycode, salesbillmain.storecode, 
            salesbillmain.yearcode, salesbilldetail.trndate
FROM         salesbillmain INNER JOIN
                      salesbilldetail ON salesbillmain.companycode = salesbilldetail.companycode AND salesbillmain.yearcode = salesbilldetail.yearcode AND 
                      salesbillmain.trnno = salesbilldetail.trnno INNER JOIN
                      itemmst ON itemmst.itemcode = salesbilldetail.itemcode AND salesbilldetail.companycode = itemmst.companycode
WHERE     (salesbillmain.salestype = 'cash') OR
                      (salesbillmain.salestype = 'credit card') OR
                      (salesbillmain.salestype = 'credit account')
GROUP BY itemmst.itemcode, itemmst.itemname, salesbillmain.companycode, salesbillmain.storecode, salesbillmain.yearcode, salesbilldetail.trndate

union all

SELECT     itemmst.itemcode, itemmst.itemname,0 as bottles_purchased, - 1 *SUM(salesbilldetail.qnty) AS bottles_sold, salesbillmain.companycode, salesbillmain.storecode, 
                      salesbillmain.yearcode, salesbilldetail.trndate
FROM         salesbillmain INNER JOIN
                      salesbilldetail ON salesbillmain.companycode = salesbilldetail.companycode AND salesbillmain.yearcode = salesbilldetail.yearcode AND 
                      salesbillmain.trnno = salesbilldetail.trnno INNER JOIN
                      itemmst ON itemmst.itemcode = salesbilldetail.itemcode AND salesbilldetail.companycode = itemmst.companycode
WHERE     (salesbillmain.salestype = 'return')
GROUP BY itemmst.itemcode, itemmst.itemname, salesbillmain.companycode, salesbillmain.storecode, salesbillmain.yearcode, salesbilldetail.trndate

union all


SELECT     itemmst.itemcode, itemmst.itemname, SUM(purchasedetail.itemquantity) AS bottles_purchased,0 as bottles_sold, purchasemain.companycode, 
                      purchasemain.shopcode AS storecode, purchasemain.yearcode, purchasemain.trndate
FROM         purchasemain INNER JOIN
                      purchasedetail ON purchasedetail.companycode = purchasemain.companycode AND purchasedetail.trnno = purchasemain.trnno AND 
                      purchasedetail.yearcode = purchasemain.yearcode INNER JOIN
                      itemmst ON itemmst.itemcode = purchasedetail.itemcode AND itemmst.companycode = purchasedetail.companycode
WHERE     (purchasemain.ptype = 'PURCHASE')
GROUP BY itemmst.itemcode, itemmst.itemname, purchasemain.companycode, purchasemain.shopcode, purchasemain.yearcode, purchasemain.trndate

union all

SELECT     itemmst.itemcode, itemmst.itemname, - (1 * SUM(purchasedetail.itemquantity)) AS bottles_purchased,0 as bottles_sold, purchasemain.companycode, 
                      purchasemain.shopcode AS storecode, purchasemain.yearcode, purchasemain.trndate
FROM         purchasemain INNER JOIN
                      purchasedetail ON purchasedetail.companycode = purchasemain.companycode AND purchasedetail.trnno = purchasemain.trnno AND 
                      purchasedetail.yearcode = purchasemain.yearcode INNER JOIN
                      itemmst ON itemmst.itemcode = purchasedetail.itemcode AND itemmst.companycode = purchasedetail.companycode
WHERE     (purchasemain.ptype = 'PURCHASE RETURN')
GROUP BY itemmst.itemcode, itemmst.itemname, purchasemain.companycode, purchasemain.shopcode, purchasemain.yearcode, purchasemain.trndate
)x 
group by itemcode,itemname,companycode,storecode,yearcode,trndate

GO

-- ----- dbo.vw_cash_sale_daywise -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE view [dbo].[vw_cash_sale_daywise] as select itemmst.itemcode,itemmst.itemname,sum(qnty) as BOTTLES_SOLD,sum(salesbilldetail.itemamount)as amnt, salesbillmain.companycode,storecode,salesbillmain.yearcode,salesbillmain.trndate,ratecode from salesbillmain join salesbilldetail on salesbillmain.companycode=salesbilldetail.companycode and salesbillmain.yearcode=salesbilldetail.yearcode and salesbillmain.trnno=salesbilldetail.trnno join itemmst on itemmst.itemcode=salesbilldetail.itemcode and salesbilldetail.companycode=itemmst.companycode where salestype='CASH'group by itemmst.itemcode,itemmst.itemname,salesbillmain.companycode,storecode,salesbillmain.yearcode,salesbillmain.trndate,ratecode
GO

-- ----- dbo.vw_credit_sale_daywise -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE view [dbo].[vw_credit_sale_daywise] as select itemmst.itemcode,itemmst.itemname,sum(qnty) as BOTTLES_SOLD,sum(salesbilldetail.itemamount)as amnt, salesbillmain.companycode,storecode,salesbillmain.yearcode,salesbillmain.trndate,ratecode from salesbillmain join salesbilldetail on salesbillmain.companycode=salesbilldetail.companycode And salesbillmain.yearcode = salesbilldetail.yearcode And salesbillmain.trnno = salesbilldetail.trnno join itemmst on itemmst.itemcode=salesbilldetail.itemcode and salesbilldetail.companycode=itemmst.companycode where salestype='credit account' group by itemmst.itemcode,itemmst.itemname,salesbillmain.companycode,storecode,salesbillmain.yearcode,salesbillmain.trndate,ratecode
GO

-- ----- dbo.vw_creditcard_sale_daywise -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE view [dbo].[vw_creditcard_sale_daywise] as select itemmst.itemcode,itemmst.itemname,sum(qnty) as BOTTLES_SOLD,sum(salesbilldetail.itemamount)as amnt, salesbillmain.companycode,storecode,salesbillmain.yearcode,salesbillmain.trndate,ratecode from salesbillmain join salesbilldetail on salesbillmain.companycode=salesbilldetail.companycode And salesbillmain.yearcode = salesbilldetail.yearcode And salesbillmain.trnno = salesbilldetail.trnno join itemmst on itemmst.itemcode=salesbilldetail.itemcode and salesbilldetail.companycode=itemmst.companycode where salestype='credit card' group by itemmst.itemcode,itemmst.itemname,salesbillmain.companycode,storecode,salesbillmain.yearcode,salesbillmain.trndate,ratecode
GO

-- ----- dbo.vw_creditors -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
create view [dbo].[vw_creditors] as select ledcode, name,ledger.companycode from ledger join acountname on acountname.accode=ledger.accode where acountname.acname like '%CREDITORS'  or acountname.acname like 'CREDITORS%'
GO

-- ----- dbo.vw_detors -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
create view [dbo].[vw_detors] as select ledcode, name,ledger.companycode from ledger join acountname on acountname.accode=ledger.accode where acountname.acname like '%DEBTORS'  or acountname.acname like 'DEBTORS%'
GO

-- ----- dbo.vw_discount_of_the_day -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE VIEW [dbo].[vw_discount_of_the_day] AS
select sum(discamount) as DISCOUNT_OF_THE_DAY,companycode,trndate,yearcode
from salesbillmain
group by trndate,companycode,trndate,yearcode

GO

-- ----- dbo.vw_ltr_sale -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
create  view [dbo].[vw_ltr_sale] as select liquor,companycode,yearcode,trndate,groupname,kflname,sum(ltr_sale) as ltr_sale from(select 'SPIRIT' as liquor,salesbilldetail.companycode,salesbilldetail.yearcode,salesbillmain.trndate,groupname,kflname,convert(float,convert(float,ml*qnty)/1000) as Ltr_sale from salesbilldetail join itemmst on itemmst.itemcode=salesbilldetail.itemcode and itemmst.companycode=salesbilldetail.companycode join groupmst on itemmst.groupcode=groupmst.groupcode and itemmst.companycode=groupmst.companycode join kflmst on itemmst.kflcode=kflmst.kflcode and itemmst.companycode=kflmst.companycode join salesbillmain on salesbillmain.companycode=salesbilldetail.companycode and salesbillmain.yearcode=salesbilldetail.yearcode and salesbillmain.trnno=salesbilldetail.trnno where groupname  not like '%BEER' and   groupname  not like 'BEER%' and   groupname  not like '%LAB' and   groupname  not like 'LAB%' and salestype<>'RETURN' union all select 'BEER'as liquor,salesbilldetail.companycode,salesbilldetail.yearcode,salesbillmain.trndate,groupname,kflname,convert(float,convert(float,ml*qnty)/1000) as Ltr_sale from salesbilldetail join itemmst on itemmst.itemcode=salesbilldetail.itemcode and itemmst.companycode=salesbilldetail.companycode join groupmst on itemmst.groupcode=groupmst.groupcode and itemmst.companycode=groupmst.companycode join kflmst on itemmst.kflcode=kflmst.kflcode and itemmst.companycode=kflmst.companycode join salesbillmain on salesbillmain.companycode=salesbilldetail.companycode and salesbillmain.yearcode=salesbilldetail.yearcode and salesbillmain.trnno=salesbilldetail.trnno where groupname like '%BEER' and salestype<>'RETURN' or  groupname  like 'BEER%' and salestype<>'RETURN' or    groupname  like '%LAB' and salestype<>'RETURN' or groupname  like 'LAB%' and salestype<>'RETURN' union all select 'SPIRIT' as liquor,salesbilldetail.companycode,salesbilldetail.yearcode,salesbillmain.trndate,groupname,kflname,-1*convert(float,convert(float,ml*qnty)/1000) as Ltr_sale from salesbilldetail join itemmst on itemmst.itemcode=salesbilldetail.itemcode and itemmst.companycode=salesbilldetail.companycode join groupmst on itemmst.groupcode=groupmst.groupcode and itemmst.companycode=groupmst.companycode join kflmst on itemmst.kflcode=kflmst.kflcode and itemmst.companycode=kflmst.companycode join salesbillmain on salesbillmain.companycode=salesbilldetail.companycode and salesbillmain.yearcode=salesbilldetail.yearcode and salesbillmain.trnno=salesbilldetail.trnno where groupname  not like '%BEER' and   groupname  not like 'BEER%' and   groupname  not like '%LAB' and   groupname  not like 'LAB%' and salestype='RETURN' union all select 'BEER'as liquor,salesbilldetail.companycode,salesbilldetail.yearcode,salesbillmain.trndate,groupname,kflname,-1*convert(float,convert(float,ml*qnty)/1000) as Ltr_sale from salesbilldetail join itemmst on itemmst.itemcode=salesbilldetail.itemcode and itemmst.companycode=salesbilldetail.companycode join groupmst on itemmst.groupcode=groupmst.groupcode and itemmst.companycode=groupmst.companycode join kflmst on itemmst.kflcode=kflmst.kflcode and itemmst.companycode=kflmst.companycode join salesbillmain on salesbillmain.companycode=salesbilldetail.companycode and salesbillmain.yearcode=salesbilldetail.yearcode and salesbillmain.trnno=salesbilldetail.trnno where groupname like '%BEER' and salestype='RETURN' or  groupname  like 'BEER%' and salestype='RETURN' or    groupname  like '%LAB' and salestype='RETURN' or groupname  like 'LAB%' and salestype='RETURN' ) as x group by liquor,companycode,yearcode,trndate,groupname,kflname
GO

-- ----- dbo.vw_payment_detail -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
create view [dbo].[vw_payment_detail] as select trndate as date,purchasemain.companycode,yearcode,suppliercode as ledgercode,name,convert(varchar(max),docno) as billno,0 as debit,totnetamt as credit from purchasemain join ledger on ledger.ledcode=purchasemain.suppliercode and ledger.companycode=purchasemain.companycode where ptype='PURCHASE' union all select payment_detail.vchdate as date,payment_detail.companycode,payment_detail.yearcode,head_account as ledgercode,name,convert(varchar(max),trnno) as billno,payment_detail.debit as debit,0 as credit from payment_detail join  payment_main on payment_detail.companycode=payment_main.companycode and payment_detail.yearcode=payment_main.yearcode And payment_detail.vchno = payment_main.vchno join ledger on ledger.ledcode=payment_main.head_account and ledger.companycode=payment_main.companycode union all select vchdate as date,payment_main.companycode,yearcode,head_account as ledgercode,name,'Voucher no.'+convert(varchar(max),vchno) as billno,discount as debit,0 as credit from payment_main join ledger on ledger.ledcode=payment_main.head_account And ledger.companycode = payment_main.companycode where discount<>0
GO

-- ----- dbo.vw_payment_main -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE view [dbo].[vw_payment_main] as
select x.yearcode,x.companycode,vchdate,x.vchno,x.name as head_account,y.name as client_account,cheque_no,due,amount_paid,discount,net_due,narration,x.ledcode as head_account_code,y.ledcode as client_account_code
from(
select yearcode,payment_main.companycode,ledcode,name,vchdate,vchno,due,discount,amount_paid,net_due,cheque_no,narration
from payment_main
join ledger
on ledger.companycode=payment_main.companycode
and ledger.ledcode=payment_main.head_account
)x
join(
select yearcode,payment_main.companycode,vchno,name,ledcode
from payment_main
join ledger
on ledger.companycode=payment_main.companycode
and ledger.ledcode=payment_main.client_account
)y 
on x.yearcode=y.yearcode
and x.companycode=y.companycode
and x.vchno=y.vchno

GO

-- ----- dbo.vw_purchse_daywise -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE view [dbo].[vw_purchse_daywise] as select itemmst.itemcode,itemmst.itemname,sum(itemquantity) as BOTTLES_PURCHASED,isnull(sum(purchasedetail.itemamount),0) as amnt,purchasemain.companycode,shopcode,purchasemain.yearcode,purchasemain.trndate from purchasemain  join purchasedetail  on purchasedetail.companycode=purchasemain.companycode And purchasedetail.trnno = purchasemain.trnno And purchasedetail.yearcode = purchasemain.yearcode join itemmst on itemmst.itemcode=purchasedetail.itemcode And itemmst.companycode = purchasedetail.companycode where ptype='PURCHASE' group by itemmst.itemcode,itemmst.itemname,purchasemain.companycode,shopcode,purchasemain.yearcode,purchasemain.trndate 
GO

-- ----- dbo.vw_purchse_return_daywise -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE view [dbo].[vw_purchse_return_daywise] as select itemmst.itemcode,itemmst.itemname,sum(itemquantity) as BOTTLES_PURCHASED,isnull(sum(purchasedetail.itemamount),0) as amnt,purchasemain.companycode,shopcode,purchasemain.yearcode,purchasemain.trndate from purchasemain join purchasedetail  on purchasedetail.companycode=purchasemain.companycode And purchasedetail.trnno = purchasemain.trnno And purchasedetail.yearcode = purchasemain.yearcode join itemmst on itemmst.itemcode=purchasedetail.itemcode And itemmst.companycode = purchasedetail.companycode where ptype='PURCHASE RETURN' group by itemmst.itemcode,itemmst.itemname,purchasemain.companycode,shopcode,purchasemain.yearcode,purchasemain.trndate 
GO

-- ----- dbo.vw_ratemst -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
create view [dbo].[vw_ratemst] as
select companycode,ratecode,itemcode,itemname,sum(salesrate) as salesrate from(
-- ====selecting the items from the itemmst and joining 
-- ===it with the ratecodes of the itemmst with 0 as sales rate ===
select itemmst.companycode,ratecode,itemcode,itemname,'0' as salesrate
from itemmst join 
itemrateinfo
on itemrateinfo.companycode=itemmst.companycode
union all
--======selecting the saved rates from the itemratemst ====
select itemratemst.companycode,ratecode,itemratemst.itemcode,itemname,itemratemst.salesrate 
from itemratemst
join itemmst on itemratemst.itemcode=itemmst.itemcode
and itemmst.companycode=itemratemst.companycode

) as x
group by companycode,ratecode,itemcode,itemname



GO

-- ----- dbo.vw_receipt_accounts -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE view [dbo].[vw_receipt_accounts] as SELECT trnid,trndate AS date,salesbillmain.companycode,yearcode,account_head AS ledgercode, 'Sale' AS Vchtype,convert(varchar(24),trnno) as billno, ledger.name, amount AS debit, 0 AS credit, ledgercode AS other_ledgercode , '' as cheque_no FROM salesbillmain JOIN ledger ON ledger.companycode = salesbillmain.companycode AND salesbillmain.account_head = ledger.ledcode  and salestype<>'RETURN'    union all select trnid, trndate as date,salesbillmain.companycode,yearcode, ledgercode,'Sale' as Vchtype,convert(varchar(24),trnno) as billno,ledger.name,0 as debit,netamount as credit, account_head as other_ledgercode , '' as cheque_no FROM salesbillmain join ledger on ledger.ledcode=salesbillmain.ledgercode and ledger.companycode=salesbillmain.companycode  and salestype<>'RETURN'  union all  select trnid, trndate as date,salesbillmain.companycode,yearcode,discount_account_head as ledgercode,'Sale' as Vchtype,convert(varchar(24),trnno) as billno,ledger.name,0 as debit,discamount as credit, account_head as other_ledgercode , '' as cheque_no FROM salesbillmain join ledger on ledger.ledcode=salesbillmain.discount_account_head and ledger.companycode=salesbillmain.companycode where discamount<>0   and salestype<>'RETURN'   union all  select trnid, trndate as date,salesbillmain.companycode,yearcode,discount_account_head as ledgercode,'Sale' as Vchtype,convert(varchar(24),trnno) as billno,ledger.name,discamount as debit,0 as credit, account_head as other_ledgercode , '' as cheque_no FROM salesbillmain join ledger on ledger.ledcode=salesbillmain.discount_account_head and ledger.companycode=salesbillmain.companycode where discamount<>0   and salestype='RETURN'   union all  select trnid, sales_tax.date, sales_tax.companycode, sales_tax.yearcode, sales_tax.tax_book AS ledgercode, 'Sale' AS Vchtype, CONVERT(varchar(24), sales_tax.trnno) AS billno,ledger.name, ABS(sales_tax.tax_amount) AS debit, 0 AS credit, ledgercode AS other_ledgercode, '' AS cheque_no FROM sales_tax INNER JOIN ledger ON ledger.companycode = sales_tax.companycode AND ledger.ledcode = sales_tax.tax_book INNER JOIN salesbillmain ON sales_tax.companycode = salesbillmain.companycode AND sales_tax.yearcode = salesbillmain.yearcode AND sales_tax.trnno = salesbillmain.trnno WHERE (sales_tax.tax_amount > 0) union all select 0 as trnid, date,sales_tax.companycode,yearcode,tax_book as ledgercode,'Sale' as Vchtype,convert(varchar(24),trnno) as billno,ledger.name,0 as debit,abs(tax_amount) as credit, cash_bank_book as other_ledgercode , '' as cheque_no FROM sales_tax join ledger on ledger.companycode=sales_tax.companycode and ledger.ledcode=sales_tax.tax_book  where tax_amount<0   union all  select trnid, vchdate as date,receipt_main.companycode,yearcode,head_account as ledgercode,'Receipt' as Vchtype,convert(varchar(24),vchno) as billno,ledger.name,amount_paid as debit,0 as credit, client_account as other_ledgercode ,cheque_no FROM receipt_main join ledger on ledger.ledcode=receipt_main.head_account and ledger.companycode=receipt_main.companycode  union all  select trnid, vchdate as date,receipt_main.companycode,yearcode,client_account as ledgercode,'Receipt' as Vchtype,convert(varchar(24),vchno) as billno,ledger.name,0 as debit,amount_paid as credit, head_account as other_ledgercode ,cheque_no FROM receipt_main join ledger on ledger.ledcode=receipt_main.client_account and ledger.companycode=receipt_main.companycode   union all  select trnid, trndate as date,salesbillmain.companycode,yearcode,account_head as ledgercode,'Return' as Vchtype,convert(varchar(24),trnno) as billno,ledger.name,0 as debit,netamount as credit,ledgercode as  other_ledgercode , '' as cheque_no FROM salesbillmain  join ledger  on ledger.ledcode=salesbillmain.account_head  and ledger.companycode=salesbillmain.companycode where salestype='RETURN'   union all  select trnid, trndate as date,salesbillmain.companycode,yearcode,ledgercode as ledgercode,'Return' as Vchtype,convert(varchar(24),trnno) as billno,ledger.name,netamount as debit,0 as credit,account_head as  other_ledgercode , '' as cheque_no FROM salesbillmain  join ledger on  ledger.ledcode=salesbillmain.ledgercode  and ledger.companycode=salesbillmain.companycode where salestype='RETURN'   union all  select trnid, trndate as date,purchasemain.companycode,yearcode,purchaseacccode as ledgercode,'Purchase' as Vchtype,docno as billno,ledger.name,0 as debit,totamount as credit,suppliercode as ledgercode , '' as cheque_no FROM purchasemain join ledger  on ledger.ledcode=purchasemain.purchaseacccode and ledger.companycode=purchasemain.companycode where ptype='PURCHASE'   union all  select trnid, trndate as date,purchasemain.companycode,yearcode,suppliercode as ledgercode,'Purchase' as Vchtype,docno as billno,ledger.name,totnetamt as debit,0 as credit ,purchaseacccode as ledgercode , '' as cheque_no FROM purchasemain join ledger  on ledger.ledcode=purchasemain.suppliercode and ledger.companycode=purchasemain.companycode where ptype='PURCHASE'   union all  select trnid, trndate as date,purchasemain.companycode,yearcode,purchaseacccode as ledgercode,'Purchase' as Vchtype,docno as billno,ledger.name,totnetamt as debit,0 as credit,suppliercode as ledgercode , '' as cheque_no FROM purchasemain join ledger  on ledger.ledcode=purchasemain.purchaseacccode and ledger.companycode=purchasemain.companycode where ptype='RETURN'   union all  select trnid, trndate as date,purchasemain.companycode,yearcode,suppliercode as ledgercode,'Purchase' as Vchtype,docno as billno,ledger.name,0 as debit,totnetamt as credit ,purchaseacccode as ledgercode , '' as cheque_no FROM purchasemain join ledger  on ledger.ledcode=purchasemain.suppliercode and ledger.companycode=purchasemain.companycode where ptype='RETURN'   union all  select trnid, docdate as date,purchasetaxdetail.companycode,purchasemain.yearcode,taxdetail.ledcode as ledgercode,'Purchase' as vchtype,purchasemain.docno as billno,ledger.name, 0 as debit,purchasetaxdetail.taxamount as credit, purchaseacccode as other_ledgercode , '' as cheque_no FROM purchasetaxdetail join taxdetail  on taxdetail.companycode=purchasetaxdetail.companycode and taxdetail.schemecode=purchasetaxdetail.schemecode and taxdetail.taxcode=purchasetaxdetail.taxcode join purchasemain  on purchasemain.companycode=purchasetaxdetail.companycode and purchasemain.yearcode=purchasetaxdetail.yearcode and purchasemain.trnno=purchasetaxdetail.trnno join ledger  on ledger.companycode=taxdetail.companycode and ledger.ledcode=taxdetail.ledcode where sig='-'   union all  select trnid, docdate as date,purchasetaxdetail.companycode,purchasemain.yearcode,taxdetail.ledcode as ledgercode,'Purchase' as vchtype,purchasemain.docno as billno,ledger.name,purchasetaxdetail.taxamount as debit, 0 as credit, suppliercode as other_ledgercode , '' as cheque_no FROM purchasetaxdetail left join taxdetail  on taxdetail.companycode=purchasetaxdetail.companycode and taxdetail.schemecode=purchasetaxdetail.schemecode and taxdetail.taxcode=purchasetaxdetail.taxcode left join purchasemain  on purchasemain.companycode=purchasetaxdetail.companycode and purchasemain.yearcode=purchasetaxdetail.yearcode and purchasemain.trnno=purchasetaxdetail.trnno left join ledger  on ledger.companycode=taxdetail.companycode and ledger.ledcode=taxdetail.ledcode where sig='+'   union all  select trnid, docdate as date,purchasetaxdetail.companycode,purchasemain.yearcode,purchasetaxdetail.taxaccount as ledgercode,'Purchase' as vchtype,purchasemain.docno as billno, ledger.name, abs(taxamount) as debit, 0 as credit, purchaseacccode as other_ledgercode , '' as cheque_no FROM purchasetaxdetail join purchasemain  on purchasemain.companycode=purchasetaxdetail.companycode and purchasemain.yearcode=purchasetaxdetail.yearcode and purchasemain.trnno=purchasetaxdetail.trnno join ledger  on ledger.companycode=purchasetaxdetail.companycode and ledger.ledcode=purchasetaxdetail.taxaccount where purchasetaxdetail.schemecode is null and taxamount<0   union all  select trnid, docdate as date,purchasetaxdetail.companycode,purchasemain.yearcode,purchasetaxdetail.taxaccount as ledgercode,'Purchase' as vchtype,purchasemain.docno as billno, ledger.name,0 as debit, taxamount as credit, purchaseacccode as other_ledgercode , '' as cheque_no FROM purchasetaxdetail join purchasemain  on purchasemain.companycode=purchasetaxdetail.companycode and purchasemain.yearcode=purchasetaxdetail.yearcode and purchasemain.trnno=purchasetaxdetail.trnno join ledger  on ledger.companycode=purchasetaxdetail.companycode and ledger.ledcode=purchasetaxdetail.taxaccount where purchasetaxdetail.schemecode is null and taxamount>0   union all select trnid, vchdate as date,payment_main.companycode,yearcode,head_account as ledgercode,'Payment' as Vchtype,convert(varchar(24),vchno) as billno,ledger.name,0 as debit,amount_paid as credit, client_account as other_ledgercode , cheque_no FROM payment_main join ledger on ledger.ledcode=payment_main.head_account and ledger.companycode=payment_main.companycode   union all  select trnid, vchdate as date,payment_main.companycode,yearcode,client_account as ledgercode,'Payment' as Vchtype,convert(varchar(24),vchno) as billno,ledger.name,amount_paid as debit,0 as credit, head_account as other_ledgercode , cheque_no FROM payment_main join ledger on ledger.ledcode=payment_main.client_account and ledger.companycode=payment_main.companycode   union all  select journaldetail.trnid, journaldetail.trndate as date,journaldetail.companycode, journaldetail.yearcode,journaldetail.ledcode as ledgercode,journaldetail.type as vchtype,convert(varchar(24),bill) as billno,ledger.name,journaldetail.credit as debit,journaldetail.debit as credit,journaldetail_1.ledcode as other_ledgercode,journaldetail.chqno as cheque_no FROM journaldetail join (SELECT companycode, yearcode,type,trndate, trnno as bill,count(ledcode)as lcount FROM journaldetail GROUP BY companycode, yearcode,type, trndate, trnno having count(ledcode)=2) as X  on journaldetail.companycode=x.companycode and journaldetail.yearcode=x.yearcode and journaldetail.trndate=x.trndate and journaldetail.trnno=x.bill and journaldetail.type=x.type join ledger on journaldetail.companycode=ledger.companycode and journaldetail.ledcode=ledger.ledcode join journaldetail as journaldetail_1 on journaldetail.companycode=journaldetail_1.companycode and journaldetail.yearcode=journaldetail_1.yearcode and journaldetail.trndate=journaldetail_1.trndate and journaldetail.trnno=journaldetail_1.trnno and journaldetail.ledcode<>journaldetail_1.ledcode and journaldetail.type=journaldetail_1.type   union all  select journaldetail.trnid, journaldetail.trndate as date,journaldetail.companycode,journaldetail.yearcode,journaldetail.ledcode as ledgercode,journaldetail.type as vchtype,journaldetail.trnno as billno,name,case when journaldetail.to_by='To' and journaldetail_1.debit < journaldetail.credit then journaldetail_1.debit else journaldetail.credit end as debit,case when journaldetail.to_by='By' and journaldetail.debit < journaldetail_1.credit then journaldetail.debit else journaldetail_1.credit end as credit,journaldetail_1.ledcode as other_ledgercode,journaldetail.chqno as cheque_no from journaldetail join journaldetail as journaldetail_1 on journaldetail.companycode=journaldetail_1.companycode and journaldetail.yearcode=journaldetail_1.yearcode and journaldetail.trndate=journaldetail_1.trndate and journaldetail.trnno=journaldetail_1.trnno and journaldetail.ledcode<>journaldetail_1.ledcode and journaldetail.type=journaldetail_1.type and journaldetail.to_by<>journaldetail_1.to_by join (SELECT companycode, yearcode,type,trndate, trnno as bill,count(ledcode)as lount FROM journaldetail GROUP BY companycode, yearcode,type, trndate, trnno having count(ledcode)=3) as X on journaldetail.companycode=x.companycode and journaldetail.yearcode=x.yearcode and journaldetail.trndate=x.trndate and journaldetail.trnno=x.bill and journaldetail.type=x.type join ledger on journaldetail.companycode=ledger.companycode and journaldetail.ledcode=ledger.ledcode   union all  select journaldetail.trnid, journaldetail.trndate as date,journaldetail.companycode, journaldetail.yearcode,journaldetail.ledcode as ledgercode,journaldetail.type as vchtype,convert(varchar(24),bill) as billno,ledger.name,journaldetail.credit as debit,journaldetail.debit as credit,'' as other_ledgercode,journaldetail.chqno as cheque_no FROM journaldetail join (SELECT companycode, yearcode,type,trndate, trnno as bill,count(ledcode)as lcount FROM journaldetail GROUP BY companycode, yearcode,type, trndate, trnno having count(ledcode)>3 ) as X  on journaldetail.companycode=x.companycode and journaldetail.yearcode=x.yearcode and journaldetail.trndate=x.trndate and journaldetail.trnno=x.bill and journaldetail.type=x.type join ledger on journaldetail.companycode=ledger.companycode and journaldetail.ledcode=ledger.ledcode 
GO

-- ----- dbo.vw_receipt_detail -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
create view [dbo].[vw_receipt_detail] as select trndate as date,salesbillmain.companycode,yearcode,ledgercode as ledgercode,convert(varchar(max),trnno) as billno,name,netamount as debit,0 as credit from salesbillmain join ledger  on ledger.ledcode=salesbillmain.ledgercode and ledger.companycode=salesbillmain.companycode where  salestype='CREDIT ACCOUNT' union all select receipt_main.vchdate as date,receipt_main.companycode,receipt_main.yearcode,head_account as ledegercode,convert(varchar(max),trnno) as billno,name,0 as debit,receipt_detail.credit as credit from receipt_main join receipt_detail on receipt_main.vchno=receipt_detail.vchno and receipt_main.companycode=receipt_detail.companycode and receipt_main.yearcode=receipt_detail.yearcode join ledger on ledger.companycode=receipt_main.companycode and ledger.ledcode=receipt_main.head_account where receipt_type='CREDIT ACCOUNT' union all select vchdate as date,receipt_main.companycode,yearcode,head_account as ledgercode,'Voucher no.'+convert(varchar(max),vchno) as billno,name,0 as debit,discount as credit from receipt_main join ledger on ledger.ledcode=receipt_main.head_account and ledger.companycode=receipt_main.companycode where receipt_type='CREDIT ACCOUNT' and discount<>0
GO

-- ----- dbo.vw_receipt_main -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE view [dbo].[vw_receipt_main] as
select x.yearcode,x.companycode,vchdate,x.vchno,x.name as head_account,y.name as client_account,cheque_no,due,amount_paid,discount,net_due,narration,x.ledcode as head_account_code,y.ledcode as client_account_code
from(
select yearcode,receipt_main.companycode,vchdate,vchno,name,narration,cheque_no,due,discount,amount_paid,net_due,ledcode
from receipt_main
join ledger
on ledger.companycode=receipt_main.companycode
and ledger.ledcode=receipt_main.head_account
)x
join(
select yearcode,receipt_main.companycode,vchno,name,ledcode
from receipt_main
join ledger
on ledger.companycode=receipt_main.companycode
and ledger.ledcode=receipt_main.client_account
)y 
on x.yearcode=y.yearcode
and x.companycode=y.companycode
and x.vchno=y.vchno

GO

-- ----- dbo.vw_return_sale_daywise -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE view [dbo].[vw_return_sale_daywise] as select itemmst.itemcode,itemmst.itemname,sum(qnty) as BOTTLES_SOLD,sum(salesbilldetail.itemamount)as amnt, salesbillmain.companycode,storecode,salesbillmain.yearcode,salesbillmain.trndate,ratecode from salesbillmain join salesbilldetail on salesbillmain.companycode=salesbilldetail.companycode And salesbillmain.yearcode = salesbilldetail.yearcode And salesbillmain.trnno = salesbilldetail.trnno join itemmst on itemmst.itemcode=salesbilldetail.itemcode and salesbilldetail.companycode=itemmst.companycode where salestype='return' group by itemmst.itemcode,itemmst.itemname,salesbillmain.companycode,storecode,salesbillmain.yearcode,salesbillmain.trndate,ratecode
GO

-- ----- dbo.vw_sale -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE view [dbo].[vw_sale] as select salestype,sum(bottles_sold) as bottles_sold,sum(large) as large,sum(small) as small,sum(westage)as westage,sum(total_amount)as total_amount,companycode,yearcode,trndate from(select salestype,qnty as BOTTLES_SOLD,isnull(large,0) as large,isnull(small,0)as small,isnull(westage,0) as westage,itemamount as TOTAL_AMOUNT,salesbillmain.companycode,salesbillmain.yearcode,salesbillmain.trndate from salesbillmain join salesbilldetail  on salesbillmain.companycode=salesbilldetail.companycode  and salesbillmain.trnno=salesbilldetail.trnno and salesbillmain.yearcode=salesbilldetail.yearcode join itemmst on itemmst.itemcode=salesbilldetail.itemcode and salesbilldetail.companycode=itemmst.companycode union all select 'DISCOUNT' as saletype,'0' as bottles_sold,'0' as large,'0' as small , '0' as westage,discamount as total_amount,companycode,yearcode,trndate from salesbillmain where salestype<>'RETURN' and discamount<>0 union all select 'DISCOUNT' as saletype,'0' as bottles_sold,'0' as large,'0' as small , '0' as westage,(-1)*discamount as total_amount,companycode,yearcode,trndate from salesbillmain where salestype='RETURN' and discamount<>0 union all select 'EXPENCE' as saletype,'0' as BOTTLES_SOLD,'0' as large,'0' as small , '0' as westage,debit as total_amount,payment_main.companycode as companycode,payment_main.yearcode as yearcode,payment_main.vchdate as trndate from payment_main join payment_detail on payment_main.companycode=payment_detail.companycode and payment_main.yearcode=payment_detail.yearcode and payment_main.vchno=payment_detail.vchno join companyparamtr on companyparamtr.companycode=payment_main.companycode and companyparamtr.expenceacc=payment_main.head_account )as x group by salestype,companycode,yearcode,trndate
GO

-- ----- dbo.vw_sales_type_all -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE view [dbo].[vw_sales_type_all] as
select salestype,sum(qnty) as BOTTLES_SOLD,sum(itemamount)  as TOTAL_AMOUNT,salesbillmain.companycode,salesbillmain.yearcode,salesbillmain.trndate from salesbillmain
join salesbilldetail on salesbillmain.companycode=salesbilldetail.companycode and salesbillmain.trnno=salesbilldetail.trnno and salesbillmain.yearcode=salesbilldetail.yearcode 
join itemmst on itemmst.itemcode=salesbilldetail.itemcode and salesbilldetail.companycode=itemmst.companycode
group by salestype,salesbillmain.companycode,salesbillmain.yearcode,salesbillmain.trndate

GO

-- ----- dbo.vw_salesbill_main -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
create view [dbo].[vw_salesbill_main] as SELECT CONVERT(varchar(10), salesbillmain.trndate, 103) AS trndate, ledger.name, salesbillmain.trnno, salesbillmain.amount, salesbillmain.discamount,dbo.salesbillmain.trntime, ISNULL(sales_tax.tax_amount, 0) AS 'surcharge_amount', salesbillmain.netamount, salesbillmain.salestype, itemrateinfo.ratename, storage.shopname, salesbillmain.ledgercode, salesbillmain.storecode, salesbillmain.ratecode, discount_reason, salesbillmain.yearcode, salesbillmain.companycode, salesbillmain.trndate AS date, sales_tax.tax_amount,salesbillmain.trnid FROM companyparamtr JOIN sales_tax ON companyparamtr.companycode = sales_tax.companycode AND companyparamtr.surcharge_account_head = sales_tax.tax_book RIGHT OUTER JOIN salesbillmain JOIN ledger ON salesbillmain.ledgercode = ledger.ledcode AND salesbillmain.companycode = ledger.companycode JOIN itemrateinfo ON salesbillmain.ratecode = itemrateinfo.ratecode AND salesbillmain.companycode = itemrateinfo.companycode JOIN storage ON storage.shopcode = salesbillmain.storecode AND salesbillmain.companycode = storage.companycode ON sales_tax.companycode = salesbillmain.companycode AND sales_tax.yearcode = salesbillmain.yearcode AND sales_tax.trnno = salesbillmain.trnno
GO

-- ----- dbo.vw_tally_data -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
create view [dbo].[vw_tally_data] as select saletype,x.companycode,x.yearcode,x.trnno,date,head,client,itemname,itemqty,itemrate,itemamount,tottaxamt,totnetamt from(select salestype as saletype,salesbillmain.companycode,salesbillmain.yearcode,salesbillmain.trnno,salesbillmain.trndate as date,name as head,itemname,qnty as itemqty,rate as itemrate,itemamount,discamount as tottaxamt,itemamount-discamount as totnetamt from salesbillmain join ledger on ledger.ledcode=salesbillmain.account_head and ledger.companycode=salesbillmain.companycode join salesbilldetail on salesbilldetail.companycode=salesbillmain.companycode and salesbilldetail.yearcode=salesbillmain.yearcode and salesbilldetail.trnno=salesbillmain.trnno join itemmst on itemmst.companycode=salesbilldetail.companycode and itemmst.itemcode=salesbilldetail.itemcode ) as x join( select salesbillmain.companycode,salesbillmain.yearcode,salesbillmain.trnno,name as client from salesbillmain join ledger on ledger.ledcode=salesbillmain.ledgercode and ledger.companycode=salesbillmain.companycode join salesbilldetail on salesbilldetail.companycode=salesbillmain.companycode and salesbilldetail.yearcode=salesbillmain.yearcode and salesbilldetail.trnno=salesbillmain.trnno join itemmst on itemmst.companycode=salesbilldetail.companycode and itemmst.itemcode=salesbilldetail.itemcode group by salesbillmain.companycode,salesbillmain.yearcode,salesbillmain.trnno,name) as y on  x.companycode = y.companycode and x.yearcode = y.yearcode and x.trnno = y.trnno
GO

-- ----- dbo.vw_tally_purchase -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
create view [dbo].[vw_tally_purchase] as select x.trnno as no,x.companycode,x.yearcode,date,achead,supplier,itempurchased,loose,box,itemqty,itemrate,itemamount,chno,chdate,tpassno,batchno,x.ptype, x.discount from (select purchasemain.trnno,purchasemain.companycode,purchasemain.yearcode,purchasemain.trndate as date,name as achead,itemname as itempurchased,itemloose as loose,itembox as box,itemquantity as itemqty,isnull(itemrate,0) as itemrate,isnull(itemamount,0) as itemamount,docno as chno,docdate as chdate,tppassno as tpassno,batchno,ptype,ISNULL(dbo.purchasedetail.itemdiscount, 0) AS discount from purchasemain join ledger on ledger.ledcode=purchasemain.purchaseacccode and ledger.companycode=purchasemain.companycode join purchasedetail on purchasedetail.companycode=purchasemain.companycode and purchasedetail.yearcode=purchasemain.yearcode and purchasedetail.trnno=purchasemain.trnno join itemmst on itemmst.itemcode=purchasedetail.itemcode and itemmst.companycode=purchasedetail.companycode ) as x join( select trnno,purchasemain.companycode,yearcode,name as supplier from purchasemain join ledger  on ledger.companycode=purchasemain.companycode and ledger.ledcode=purchasemain.suppliercode )as y on y.companycode=x.companycode and y.yearcode=x.yearcode and y.trnno=x.trnno
GO

-- ----- dbo.vwSalesData -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
create view [dbo].[vwSalesData]  as SELECT salesbillmain.trndate as date, salesbillmain.salestype as SalesAcount, ledger.name as AcountHead, salesbillmain.trnno, itemmst.itemname,salesbilldetail.qnty as itemqty, salesbilldetail.rate as itemrate, salesbilldetail.itemAmount, (salesbilldetail.itemAmount*((salesbillmain.discamount)/(salesbillmain.netamount+salesbillmain.discamount)))as discount,(salesbilldetail.itemAmount-(salesbilldetail.itemAmount*((salesbillmain.discamount)/(salesbillmain.netamount+salesbillmain.discamount)))) as totnetamt FROM salesbillmain JOIN salesbilldetail ON salesbillmain.companycode = salesbilldetail.companycode AND salesbillmain.trndate = salesbilldetail.trndate AND salesbillmain.trnno = salesbilldetail.trnno AND salesbillmain.yearcode = salesbilldetail.yearcode JOIN ledger ON salesbillmain.companycode = ledger.companycode AND salesbillmain.ledgercode = ledger.ledcode JOIN itemmst ON salesbilldetail.companycode = itemmst.companycode AND salesbilldetail.itemcode = itemmst.itemcode where salesbillmain.companycode='1' and salesbillmain.yearcode='2' and salesbillmain.trndate>='4/1/2013 7:04:32 PM'  and salesbillmain.trndate<='8/24/2013 7:04:32 PM' group by salesbillmain.trnno,salesbillmain.companycode,salesbillmain.yearcode,salesbillmain.trndate,name,salestype,itemmst.itemname,salesbilldetail.qnty, salesbilldetail.rate, salesbilldetail.itemAmount,salesbillmain.discamount,salesbillmain.netamount
GO

-- ============================================================
-- 5. FUNCTIONS
-- ============================================================
-- ----- dbo.DateRange -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

CREATE function [dbo].[DateRange](@StartDate DATETIME , @EndDate DATETIME)returns  table as return
WITH Dates(Date) AS
(
    SELECT DATEADD(dd, 0, DATEDIFF(dd, 0, @StartDate)) AS Date
    UNION ALL
    SELECT DATEADD(day, 1, Date) AS Date
    FROM Dates
    WHERE Date < @EndDate
)
SELECT d.Date
FROM Dates d



GO

-- ----- dbo.opening_closing_stock -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE function [dbo].[opening_closing_stock](@day1 datetime,@day2 datetime,@day3 datetime) returns table as return select itemcode,itemname,sum(opening_stock) as opening_stock,sum(bottles_purchased) as bottles_purchased,sum(bottles_sold) as bottles_sold,sum(stock)+sum(opening_stock)+ sum(bottles_purchased)-sum(bottles_sold)as stock,companycode,storecode,yearcode,sum(breakage) as breakage from(select itemmst.itemcode,itemname,sum(qnty) as opening_stock,0 as bottles_purchased,0 as bottles_sold,0 as stock,itemmst.companycode,shopcode as storecode,yearcode,0 as breakage from openingstockmst join itemmst on openingstockmst.itemcode=itemmst.itemcode and openingstockmst.companycode=itemmst.companycode where openingstockmst.trndate=@day1 group by itemmst.itemcode,itemmst.itemname,itemmst.companycode,shopcode,openingstockmst.yearcode union all select itemmst.itemcode,itemmst.itemname,(-1)*sum(qnty) as opening_stock,0 as bottles_purchased,0 as bottles_sold,0 as stock,salesbillmain.companycode,storecode,salesbillmain.yearcode,0 as breakage from salesbillmain join salesbilldetail on salesbillmain.companycode=salesbilldetail.companycode and salesbillmain.yearcode=salesbilldetail.yearcode and salesbillmain.trnno=salesbilldetail.trnno join itemmst on itemmst.itemcode=salesbilldetail.itemcode and salesbilldetail.companycode=itemmst.companycode where salestype<>'RETURN' and salesbillmain.trndate >=@day1 and salesbillmain.trndate <@day2 group by itemmst.itemcode,itemmst.itemname,salesbillmain.companycode,storecode,salesbillmain.yearcode union all select itemmst.itemcode,itemmst.itemname,sum(qnty) as opening_stock,0 as bottles_purchased,0 as bottles_sold,0 as stock,salesbillmain.companycode,storecode,salesbillmain.yearcode,0 as breakage  from salesbillmain join salesbilldetail on salesbillmain.companycode=salesbilldetail.companycode and salesbillmain.yearcode=salesbilldetail.yearcode and salesbillmain.trnno=salesbilldetail.trnno join itemmst on itemmst.itemcode=salesbilldetail.itemcode and salesbilldetail.companycode=itemmst.companycode where salestype='RETURN' and salesbillmain.trndate >=@day1 and salesbillmain.trndate <@day2 group by itemmst.itemcode,itemmst.itemname,salesbillmain.companycode,storecode,salesbillmain.yearcode union all select itemmst.itemcode,itemmst.itemname,sum(itemquantity) as opening_stock,0 as bottles_purchased,0 as bottles_sold,0 as stock,purchasemain.companycode,shopcode as storecode,purchasemain.yearcode,0 as breakage  from purchasemain join purchasedetail on purchasedetail.companycode=purchasemain.companycode and purchasedetail.trnno=purchasemain.trnno and purchasedetail.yearcode=purchasemain.yearcode join itemmst on itemmst.itemcode=purchasedetail.itemcode and itemmst.companycode=purchasedetail.companycode where ptype='PURCHASE' and purchasemain.trndate >=@day1 and purchasemain.trndate <@day2 group by itemmst.itemcode,itemmst.itemname,purchasemain.companycode,shopcode,purchasemain.yearcode union all select itemmst.itemcode,itemmst.itemname,-1*sum(itemquantity) as opening_stock,0 as bottles_purchased,0 as bottles_sold,0 as stock,purchasemain.companycode,shopcode as storecode,purchasemain.yearcode,0 as breakage  from purchasemain join purchasedetail on purchasedetail.companycode=purchasemain.companycode and purchasedetail.trnno=purchasemain.trnno and purchasedetail.yearcode=purchasemain.yearcode join itemmst on itemmst.itemcode=purchasedetail.itemcode and itemmst.companycode=purchasedetail.companycode where ptype='RETURN' and purchasemain.trndate >=@day1 and purchasemain.trndate <@day2 group by itemmst.itemcode,itemmst.itemname,purchasemain.companycode,shopcode,purchasemain.yearcode union all select itemmst.itemcode,itemname,(-1)*sum(qnty) as opening_stock,0 as bottles_purchased,0 as bottles_sold,0 as stock,stk_transfer_detail.companycode,shopcode_frm as storecode,stk_transfer_detail.yearcode,0 as breakage  from stk_transfer_detail join itemmst on itemmst.itemcode=stk_transfer_detail.itemcode and stk_transfer_detail.companycode=itemmst.companycode join stk_transfer_main on stk_transfer_main.companycode=stk_transfer_detail.companycode and stk_transfer_main.yearcode=stk_transfer_detail.yearcode and stk_transfer_main.trnno=stk_transfer_detail.trnno where stk_transfer_main.trndate>=@day1 and stk_transfer_main.trndate<@day2 group by itemmst.itemcode,itemmst.itemname,stk_transfer_detail.companycode,shopcode_frm,stk_transfer_detail.yearcode union all select itemmst.itemcode,itemname,sum(qnty) as opening_stock,0 as bottles_purchased,0 as bottles_sold,0 as stock,stk_transfer_detail.companycode,shopcode_to as storecode,stk_transfer_detail.yearcode,0 as breakage  from stk_transfer_detail join itemmst on itemmst.itemcode=stk_transfer_detail.itemcode and stk_transfer_detail.companycode=itemmst.companycode join stk_transfer_main on stk_transfer_main.companycode=stk_transfer_detail.companycode and stk_transfer_main.yearcode=stk_transfer_detail.yearcode and stk_transfer_main.trnno=stk_transfer_detail.trnno where stk_transfer_main.trndate>=@day1 and stk_transfer_main.trndate<@day2 group by itemmst.itemcode,itemmst.itemname,stk_transfer_detail.companycode,shopcode_to,stk_transfer_detail.yearcode union all select breakagedetail.itemcode,itemname,-1*sum(quantity) as opening_stock,0 as bottles_purchased,0 as bottles_sold,0 as stock,breakagedetail.companycode,shopcode,breakagedetail.yearcode,0 as breakage  from breakagedetail join breakagemain on breakagedetail.companycode=breakagemain.companycode and breakagedetail.yearcode=breakagemain.yearcode and breakagedetail.trnno=breakagemain.trnno join itemmst on itemmst.itemcode=breakagedetail.itemcode and itemmst.companycode=breakagedetail.companycode where receivd='False' and breakagemain.trndate>=@day1 and breakagemain.trndate<@day2 group by breakagedetail.itemcode,itemname,breakagedetail.companycode,shopcode,breakagedetail.yearcode union all select itemmst.itemcode,itemmst.itemname,0 as opening_stock,0 as bottles_purchased,sum(qnty) as bottles_sold,0 as stock,salesbillmain.companycode,storecode,salesbillmain.yearcode,0 as breakage  from salesbillmain join salesbilldetail on salesbillmain.companycode=salesbilldetail.companycode and salesbillmain.yearcode=salesbilldetail.yearcode and salesbillmain.trnno=salesbilldetail.trnno join itemmst on itemmst.itemcode=salesbilldetail.itemcode and salesbilldetail.companycode=itemmst.companycode where salestype='cash' and salesbillmain.trndate >=@day2 and salesbillmain.trndate <=@day3 or salestype='credit card' and salesbillmain.trndate >=@day2 and salesbillmain.trndate <=@day3 or  salestype ='credit account' and salesbillmain.trndate >=@day2 and salesbillmain.trndate <=@day3 group by itemmst.itemcode,itemmst.itemname,salesbillmain.companycode,storecode,salesbillmain.yearcode union all select itemmst.itemcode,itemmst.itemname,0 as opening_stock,0 as bottles_purchased,(-1)*sum(qnty) as bottles_sold,0 as stock,salesbillmain.companycode,storecode,salesbillmain.yearcode,0 as breakage  from salesbillmain join salesbilldetail on salesbillmain.companycode=salesbilldetail.companycode and salesbillmain.yearcode=salesbilldetail.yearcode and salesbillmain.trnno=salesbilldetail.trnno join itemmst on itemmst.itemcode=salesbilldetail.itemcode and salesbilldetail.companycode=itemmst.companycode where salestype='return' and salesbillmain.trndate >=@day2 and salesbillmain.trndate <=@day3 group by itemmst.itemcode,itemmst.itemname,salesbillmain.companycode,storecode,salesbillmain.yearcode union all select itemmst.itemcode,itemmst.itemname,0 as opening_stock,sum(itemquantity) as bottles_purchased,0 as bottles_sold,0 as stock,purchasemain.companycode,shopcode as storecode,purchasemain.yearcode,0 as breakage  from purchasemain join purchasedetail on purchasedetail.companycode=purchasemain.companycode and purchasedetail.trnno=purchasemain.trnno and purchasedetail.yearcode=purchasemain.yearcode join itemmst on itemmst.itemcode=purchasedetail.itemcode and itemmst.companycode=purchasedetail.companycode where ptype='PURCHASE' and purchasemain.trndate >=@day2 and purchasemain.trndate <=@day3 group by itemmst.itemcode,itemmst.itemname,purchasemain.companycode,shopcode,purchasemain.yearcode union all select itemmst.itemcode,itemmst.itemname,0 as opening_stock,(-1)*sum(itemquantity) as bottles_purchased,0 as bottles_sold,0 as stock,purchasemain.companycode,shopcode as storecode,purchasemain.yearcode,0 as breakage  from purchasemain join purchasedetail on purchasedetail.companycode=purchasemain.companycode and purchasedetail.trnno=purchasemain.trnno and purchasedetail.yearcode=purchasemain.yearcode join itemmst on itemmst.itemcode=purchasedetail.itemcode and itemmst.companycode=purchasedetail.companycode where ptype='RETURN' and purchasemain.trndate >=@day2 and purchasemain.trndate <=@day3 group by itemmst.itemcode,itemmst.itemname,purchasemain.companycode,shopcode,purchasemain.yearcode union all select itemmst.itemcode,itemname,0 as opening_stock,0 as bottles_purchased,0 as bottles_sold,(-1)*sum(qnty) as stock,stk_transfer_detail.companycode,shopcode_frm as storecode,stk_transfer_detail.yearcode,0 as breakage  from stk_transfer_detail join itemmst on itemmst.itemcode=stk_transfer_detail.itemcode and stk_transfer_detail.companycode=itemmst.companycode join stk_transfer_main on stk_transfer_main.companycode=stk_transfer_detail.companycode and stk_transfer_main.yearcode=stk_transfer_detail.yearcode and stk_transfer_main.trnno=stk_transfer_detail.trnno where stk_transfer_main.trndate>=@day2 and stk_transfer_main.trndate<=@day3 group by itemmst.itemcode,itemmst.itemname,stk_transfer_detail.companycode,shopcode_frm,stk_transfer_detail.yearcode union all select itemmst.itemcode,itemname,0 as opening_stock,0 as bottles_purchased,0 as bottles_sold,sum(qnty) as stock,stk_transfer_detail.companycode,shopcode_to as storecode,stk_transfer_detail.yearcode,0 as breakage  from stk_transfer_detail join itemmst on itemmst.itemcode=stk_transfer_detail.itemcode and stk_transfer_detail.companycode=itemmst.companycode join stk_transfer_main on stk_transfer_main.companycode=stk_transfer_detail.companycode and stk_transfer_main.yearcode=stk_transfer_detail.yearcode and stk_transfer_main.trnno=stk_transfer_detail.trnno where stk_transfer_main.trndate>=@day2 and stk_transfer_main.trndate<=@day3 group by itemmst.itemcode,itemmst.itemname,stk_transfer_detail.companycode,shopcode_to,stk_transfer_detail.yearcode union all select breakagedetail.itemcode,itemname,0 as opening_stock,0 as bottles_purchased,0 as bottles_sold,-1*sum(quantity) as stock,breakagedetail.companycode,shopcode,breakagedetail.yearcode,sum(quantity) as breakage  from breakagedetail join breakagemain on breakagedetail.companycode=breakagemain.companycode and breakagedetail.yearcode=breakagemain.yearcode and breakagedetail.trnno=breakagemain.trnno join itemmst on itemmst.itemcode=breakagedetail.itemcode and itemmst.companycode=breakagedetail.companycode where receivd='False' and breakagemain.trndate>=@day2 and breakagemain.trndate<=@day3 group by breakagedetail.itemcode,itemname,breakagedetail.companycode,shopcode,breakagedetail.yearcode)x group by itemcode,itemname,companycode,storecode,yearcode
GO

-- ----- dbo.opening_closing_stock_batchNo -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE function [dbo].[opening_closing_stock_batchNo](@day1 datetime,@day2 datetime,@day3 datetime) returns table as return  select itemcode,itemname,sum(opening_stock) as opening_stock,sum(bottles_purchased) as bottles_purchased,sum(bottles_sold) as bottles_sold, sum(stock)+sum(opening_stock)+ sum(bottles_purchased)-sum(bottles_sold)as stock,companycode,storecode,yearcode,batchNo ,trndate,max(distinct(mrp)) as mrp from( select itemmst.itemcode,itemname,sum(qnty) as opening_stock,0 as bottles_purchased,0 as bottles_sold,0 as stock,itemmst.companycode,shopcode as storecode,yearcode, (batchNo+' ` '+convert(varchar,openingstockDetail.trndate,103)) as batchNo,openingstockDetail.trndate ,max(distinct(itemmst.purchaserate)) as mrp from openingstockDetail join itemmst on openingstockDetail.itemcode=itemmst.itemcode and openingstockDetail.companycode=itemmst.companycode where openingstockDetail.trndate=@day1 group by itemmst.itemcode,itemmst.itemname,itemmst.companycode,shopcode,openingstockDetail.yearcode,openingstockDetail.batchNO,openingstockDetail.trndate union all select itemmst.itemcode,itemmst.itemname,(-1)*sum(qnty) as opening_stock,0 as bottles_purchased,0 as bottles_sold,0 as stock,salesbillmain.companycode,storecode,salesbillmain.yearcode,salesbilldetail.batchNo,salesbilldetail.batchdate as trndate ,max(distinct(salesbilldetail.mrp)) as mrp from salesbillmain join salesbilldetail on salesbillmain.companycode=salesbilldetail.companycode and salesbillmain.yearcode=salesbilldetail.yearcode and salesbillmain.trnno=salesbilldetail.trnno  join itemmst on itemmst.itemcode=salesbilldetail.itemcode and salesbilldetail.companycode=itemmst.companycode where salestype<>'RETURN' and salesbillmain.trndate >=@day1 and salesbillmain.trndate <@day2 group by itemmst.itemcode,itemmst.itemname,salesbillmain.companycode,storecode,salesbillmain.yearcode,salesbilldetail.batchNo,salesbilldetail.batchdate union all select itemmst.itemcode,itemmst.itemname,sum(qnty) as opening_stock,0 as bottles_purchased,0 as bottles_sold,0 as stock,salesbillmain.companycode,storecode,salesbillmain.yearcode,salesbilldetail.batchNo,salesbilldetail.batchdate as trndate ,max(distinct(salesbilldetail.mrp)) as mrp from salesbillmain join salesbilldetail on salesbillmain.companycode=salesbilldetail.companycode and salesbillmain.yearcode=salesbilldetail.yearcode and salesbillmain.trnno=salesbilldetail.trnno join itemmst on itemmst.itemcode=salesbilldetail.itemcode and salesbilldetail.companycode=itemmst.companycode where salestype='RETURN' and salesbillmain.trndate >=@day1 and salesbillmain.trndate <@day2 group by itemmst.itemcode,itemmst.itemname,salesbillmain.companycode,storecode,salesbillmain.yearcode,salesbilldetail.batchNo,salesbilldetail.batchdate union all select itemmst.itemcode,itemmst.itemname,sum(itemquantity) as opening_stock,0 as bottles_purchased,0 as bottles_sold,0 as stock,purchasemain.companycode, shopcode as storecode,purchasemain.yearcode,(purchasedetail.batchNo+' ` '+convert(varchar,purchasemain.trnDate,103)) as batchNo,purchasemain.trnDate,max(distinct(purchasedetail.itemmrp)) as mrp  from purchasemain join purchasedetail on purchasedetail.companycode=purchasemain.companycode and purchasedetail.trnno=purchasemain.trnno and purchasedetail.yearcode=purchasemain.yearcode join itemmst on itemmst.itemcode=purchasedetail.itemcode and itemmst.companycode=purchasedetail.companycode where ptype='PURCHASE' and purchasemain.trndate >=@day1 and purchasemain.trndate <@day2 group by itemmst.itemcode,itemmst.itemname,purchasemain.companycode,shopcode,purchasemain.yearcode,purchasedetail.batchNo,purchasemain.trnDate union all select itemmst.itemcode,itemmst.itemname,-1*sum(itemquantity) as opening_stock,0 as bottles_purchased,0 as bottles_sold,0 as stock,purchasemain.companycode,shopcode as storecode,purchasemain.yearcode,(purchasedetail.batchNo+' ` '+convert(varchar,purchasemain.trnDate,103)) as batchNo,purchasemain.trnDate,max(distinct(purchasedetail.itemmrp))as mrp  from purchasemain join purchasedetail on purchasedetail.companycode=purchasemain.companycode and purchasedetail.trnno=purchasemain.trnno and purchasedetail.yearcode=purchasemain.yearcode join itemmst on itemmst.itemcode=purchasedetail.itemcode and itemmst.companycode=purchasedetail.companycode where ptype='RETURN' and purchasemain.trndate >=@day1 and purchasemain.trndate <@day2 group by itemmst.itemcode,itemmst.itemname,purchasemain.companycode,shopcode,purchasemain.yearcode,purchasedetail.batchNo,purchasemain.trnDate union all select itemmst.itemcode,itemname,(-1)*sum(qnty) as opening_stock,0 as bottles_purchased,0 as bottles_sold,0 as stock,stk_transfer_detail.companycode,shopcode_frm as storecode,stk_transfer_detail.yearcode,(stk_transfer_detail.batchNo+' ` '+convert(varchar,stk_transfer_detail.batchdate,103)) as batchNo,stk_transfer_detail.batchdate as trndate,max(distinct(purchasedetail.itemmrp))as mrp from stk_transfer_detail join itemmst on itemmst.itemcode=stk_transfer_detail.itemcode and stk_transfer_detail.companycode=itemmst.companycode join stk_transfer_main on stk_transfer_main.companycode=stk_transfer_detail.companycode and stk_transfer_main.yearcode=stk_transfer_detail.yearcode and stk_transfer_main.trnno=stk_transfer_detail.trnno  JOIN purchasedetail ON stk_transfer_detail.CompanyCode = purchasedetail.companycode AND stk_transfer_detail.yearcode = purchasedetail.yearcode AND stk_transfer_detail.ItemCode = purchasedetail.itemcode AND stk_transfer_detail.batchNo = purchasedetail.batchno AND stk_transfer_detail.batchDate = purchasedetail.trndate where stk_transfer_main.trndate>=@day1 and stk_transfer_main.trndate<@day2 group by itemmst.itemcode,itemmst.itemname,stk_transfer_detail.companycode,shopcode_frm,stk_transfer_detail.yearcode,stk_transfer_detail.batchNo,stk_transfer_detail.batchdate union all  select itemmst.itemcode,itemname,sum(qnty) as opening_stock,0 as bottles_purchased,0 as bottles_sold,0 as stock,stk_transfer_detail.companycode,shopcode_to as storecode,stk_transfer_detail.yearcode,(stk_transfer_detail.batchNo+' ` '+convert(varchar,stk_transfer_detail.batchdate,103))as batchNo,stk_transfer_detail.batchdate as trndate, max(distinct(purchasedetail.itemmrp))as mrp from stk_transfer_detail join itemmst on itemmst.itemcode=stk_transfer_detail.itemcode and stk_transfer_detail.companycode=itemmst.companycode join stk_transfer_main on stk_transfer_main.companycode=stk_transfer_detail.companycode and stk_transfer_main.yearcode=stk_transfer_detail.yearcode and stk_transfer_main.trnno=stk_transfer_detail.trnno  JOIN purchasedetail ON stk_transfer_detail.CompanyCode = purchasedetail.companycode AND stk_transfer_detail.yearcode = purchasedetail.yearcode AND stk_transfer_detail.ItemCode = purchasedetail.itemcode AND stk_transfer_detail.batchNo = purchasedetail.batchno AND stk_transfer_detail.batchDate = purchasedetail.trndate where stk_transfer_main.trndate>=@day1 and stk_transfer_main.trndate<@day2 group by itemmst.itemcode,itemmst.itemname,stk_transfer_detail.companycode,shopcode_to,stk_transfer_detail.yearcode,stk_transfer_detail.batchNo ,stk_transfer_detail.batchdate union all select breakagedetail.itemcode,itemname,-1*sum(quantity) as opening_stock,0 as bottles_purchased,0 as bottles_sold,0 as stock,breakagedetail.companycode,shopcode,breakagedetail.yearcode,(breakagedetail.batchNo+' ` '+convert(varchar,breakagedetail.batchdate,103)) as batchNo,breakagedetail.batchdate as trndate ,max(distinct(purchasedetail.itemmrp))as mrp  from breakagedetail join breakagemain on breakagedetail.companycode=breakagemain.companycode and breakagedetail.yearcode=breakagemain.yearcode and breakagedetail.trnno=breakagemain.trnno join itemmst on itemmst.itemcode=breakagedetail.itemcode and itemmst.companycode=breakagedetail.companycode  JOIN purchasedetail ON breakagedetail.CompanyCode = purchasedetail.companycode AND breakagedetail.yearcode = purchasedetail.yearcode AND breakagedetail.ItemCode = purchasedetail.itemcode AND breakagedetail.batchDate = purchasedetail.trndate AND breakagedetail.batchNo = purchasedetail.batchno where receivd='False' and breakagemain.trndate>=@day1 and breakagemain.trndate<@day2 group by breakagedetail.itemcode,itemname,breakagedetail.companycode,shopcode,breakagedetail.yearcode,breakagedetail.batchNo,breakagedetail.batchdate union all select itemmst.itemcode,itemmst.itemname,0 as opening_stock,0 as bottles_purchased,sum(qnty) as bottles_sold,0 as stock,salesbillmain.companycode,storecode,salesbillmain.yearcode,salesbilldetail.batchNo,salesbilldetail.batchdate as trndate ,max(distinct(salesbilldetail.mrp)) as mrp from salesbillmain join salesbilldetail on salesbillmain.companycode=salesbilldetail.companycode and salesbillmain.yearcode=salesbilldetail.yearcode and salesbillmain.trnno=salesbilldetail.trnno   join itemmst on itemmst.itemcode=salesbilldetail.itemcode and salesbilldetail.companycode=itemmst.companycode where salestype='cash' and salesbillmain.trndate >=@day2 and salesbillmain.trndate <=@day3 or salestype='credit card' and salesbillmain.trndate >=@day2 and salesbillmain.trndate <=@day3 or  salestype ='credit account' and salesbillmain.trndate >=@day2 and salesbillmain.trndate <=@day3  group by itemmst.itemcode,itemmst.itemname,salesbillmain.companycode,storecode,salesbillmain.yearcode,salesbilldetail.batchNo,salesbilldetail.batchdate union all select itemmst.itemcode,itemmst.itemname,0 as opening_stock,0 as bottles_purchased,(-1)*sum(qnty) as bottles_sold,0 as stock,salesbillmain.companycode,storecode,salesbillmain.yearcode,salesbilldetail.batchNo,salesbilldetail.batchdate as trndate ,max(distinct(salesbilldetail.mrp)) as mrp from salesbillmain join salesbilldetail on salesbillmain.companycode=salesbilldetail.companycode and salesbillmain.yearcode=salesbilldetail.yearcode and salesbillmain.trnno=salesbilldetail.trnno join itemmst on itemmst.itemcode=salesbilldetail.itemcode and salesbilldetail.companycode=itemmst.companycode where salestype='return' and salesbillmain.trndate >=@day2 and salesbillmain.trndate <=@day3 group by itemmst.itemcode,itemmst.itemname,salesbillmain.companycode,storecode,salesbillmain.yearcode,salesbilldetail.batchNo,salesbilldetail.batchdate union all select itemmst.itemcode,itemmst.itemname,0 as opening_stock,sum(itemquantity) as bottles_purchased,0 as bottles_sold,0 as stock,purchasemain.companycode,shopcode as storecode,purchasemain.yearcode,(purchasedetail.batchNo+' ` '+convert(varchar,purchasemain.trndate,103)) as batchNo,purchasemain.trndate,max(distinct(purchasedetail.itemmrp)) as mrp  from purchasemain join purchasedetail on purchasedetail.companycode=purchasemain.companycode and purchasedetail.trnno=purchasemain.trnno and purchasedetail.yearcode=purchasemain.yearcode join itemmst on itemmst.itemcode=purchasedetail.itemcode and itemmst.companycode=purchasedetail.companycode where ptype='PURCHASE' and purchasemain.trndate >=@day2 and purchasemain.trndate <=@day3 group by itemmst.itemcode,itemmst.itemname,purchasemain.companycode,shopcode,purchasemain.yearcode,purchasedetail.batchNo ,purchasemain.trndate union all select itemmst.itemcode,itemmst.itemname,0 as opening_stock,(-1)*sum(itemquantity) as bottles_purchased,0 as bottles_sold,0 as stock,purchasemain.companycode,shopcode as storecode,purchasemain.yearcode,(purchasedetail.batchNo+' ` '+convert(varchar,purchasemain.trndate,103))as batchNo ,purchasemain.trndate ,max(distinct(purchasedetail.itemmrp)) as mrp  from purchasemain join purchasedetail on purchasedetail.companycode=purchasemain.companycode and purchasedetail.trnno=purchasemain.trnno and purchasedetail.yearcode=purchasemain.yearcode join itemmst on itemmst.itemcode=purchasedetail.itemcode and itemmst.companycode=purchasedetail.companycode where ptype='RETURN' and purchasemain.trndate >=@day2 and purchasemain.trndate <=@day3 group by itemmst.itemcode,itemmst.itemname,purchasemain.companycode,shopcode,purchasemain.yearcode,purchasedetail.batchNo,purchasemain.trndate union all select itemmst.itemcode,itemname,0 as opening_stock,0 as bottles_purchased,0 as bottles_sold,(-1)*sum(qnty) as stock,stk_transfer_detail.companycode,shopcode_frm as storecode,stk_transfer_detail.yearcode,(stk_transfer_detail.batchNo+' ` '+convert(varchar,stk_transfer_detail.batchdate,103)) as batchNo ,stk_transfer_detail.batchdate as trndate,max(distinct(purchasedetail.itemmrp))as mrp from stk_transfer_detail  join itemmst on itemmst.itemcode=stk_transfer_detail.itemcode and stk_transfer_detail.companycode=itemmst.companycode join stk_transfer_main on stk_transfer_main.companycode=stk_transfer_detail.companycode and stk_transfer_main.yearcode=stk_transfer_detail.yearcode and stk_transfer_main.trnno=stk_transfer_detail.trnno JOIN purchasedetail ON stk_transfer_detail.CompanyCode = purchasedetail.companycode AND stk_transfer_detail.yearcode = purchasedetail.yearcode AND stk_transfer_detail.ItemCode = purchasedetail.itemcode AND stk_transfer_detail.batchNo = purchasedetail.batchno AND stk_transfer_detail.batchDate = purchasedetail.trndate where stk_transfer_main.trndate>=@day2 and stk_transfer_main.trndate<=@day3 group by itemmst.itemcode,itemmst.itemname,stk_transfer_detail.companycode,shopcode_frm,stk_transfer_detail.yearcode,stk_transfer_detail.batchNo,stk_transfer_detail.batchdate  union all select itemmst.itemcode,itemname,0 as opening_stock,0 as bottles_purchased,0 as bottles_sold,sum(qnty) as stock,stk_transfer_detail.companycode,shopcode_to as storecode,stk_transfer_detail.yearcode,(stk_transfer_detail.batchNo+' ` '+convert(varchar,stk_transfer_detail.batchdate,103)) as batchNo,stk_transfer_detail.batchdate as trndate ,max(distinct(purchasedetail.itemmrp))as mrp  from stk_transfer_detail join itemmst on itemmst.itemcode=stk_transfer_detail.itemcode and stk_transfer_detail.companycode=itemmst.companycode join stk_transfer_main on stk_transfer_main.companycode=stk_transfer_detail.companycode and stk_transfer_main.yearcode=stk_transfer_detail.yearcode and stk_transfer_main.trnno=stk_transfer_detail.trnno JOIN purchasedetail ON stk_transfer_detail.CompanyCode = purchasedetail.companycode AND stk_transfer_detail.yearcode = purchasedetail.yearcode AND stk_transfer_detail.ItemCode = purchasedetail.itemcode AND stk_transfer_detail.batchNo = purchasedetail.batchno AND stk_transfer_detail.batchDate = purchasedetail.trndate where stk_transfer_main.trndate>=@day2 and stk_transfer_main.trndate<=@day3 group by itemmst.itemcode,itemmst.itemname,stk_transfer_detail.companycode,shopcode_to,stk_transfer_detail.yearcode,stk_transfer_detail.batchNo ,stk_transfer_detail.batchdate union all select breakagedetail.itemcode,itemname,0 as opening_stock,0 as bottles_purchased,0 as bottles_sold,-1*sum(quantity) as stock,breakagedetail.companycode,shopcode,breakagedetail.yearcode,(breakagedetail.batchNo+' ` '+convert(varchar,breakagedetail.batchdate,103)) as batchNo ,breakagedetail.batchdate as trndate ,max(distinct(purchasedetail.itemmrp))as mrp  from breakagedetail join breakagemain on breakagedetail.companycode=breakagemain.companycode and breakagedetail.yearcode=breakagemain.yearcode and breakagedetail.trnno=breakagemain.trnno  join itemmst on itemmst.itemcode=breakagedetail.itemcode and itemmst.companycode=breakagedetail.companycode JOIN purchasedetail ON breakagedetail.CompanyCode = purchasedetail.companycode AND breakagedetail.yearcode = purchasedetail.yearcode AND breakagedetail.ItemCode = purchasedetail.itemcode AND breakagedetail.batchDate = purchasedetail.trndate AND breakagedetail.batchNo = purchasedetail.batchno where receivd='False' and breakagemain.trndate>=@day2 and breakagemain.trndate<=@day3  group by breakagedetail.itemcode,itemname,breakagedetail.companycode,shopcode,breakagedetail.yearcode,breakagedetail.batchNo,breakagedetail.batchdate  )x  group by itemcode,itemname,companycode,storecode,yearcode,batchNo,trndate 
GO

-- ----- dbo.opening_closing_stock_rep -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE function [dbo].[opening_closing_stock_rep](@day1 datetime,@day2 datetime,@day3 datetime) returns table
as
return
select itemcode,itemname,rate,sum(opening_stock) as opening_stock,sum(bottles_purchased) as bottles_purchased,sum(bottles_sold) as bottles_sold, sum(stock)+sum(opening_stock)+ sum(bottles_purchased)-sum(bottles_sold)as stock,companycode,storecode,yearcode
from(
-- PART1 >@day2 and <@day2----->
-- ==========for adding the qunatities in the stock==========
select itemmst.itemcode,itemname,0 as rate,sum(qnty) as opening_stock,0 as bottles_purchased,0 as bottles_sold,0 as stock,itemmst.companycode,shopcode as storecode,yearcode
from openingstockmst
join itemmst 
on 
openingstockmst.itemcode=itemmst.itemcode 
and openingstockmst.companycode=itemmst.companycode
where openingstockmst.trndate=@day1
group by itemmst.itemcode,itemmst.itemname,itemmst.companycode,shopcode,openingstockmst.yearcode
union all
-- ===========deducting those quantities from the stock that has been sold==========
select itemmst.itemcode,itemmst.itemname, rate,(-1)*sum(qnty) as opening_stock,0 as bottles_purchased,0 as bottles_sold,0 as stock,salesbillmain.companycode,storecode,salesbillmain.yearcode 
from salesbillmain
join salesbilldetail 
on salesbillmain.companycode=salesbilldetail.companycode and salesbillmain.yearcode=salesbilldetail.yearcode and salesbillmain.trnno=salesbilldetail.trnno
join itemmst on itemmst.itemcode=salesbilldetail.itemcode and salesbilldetail.companycode=itemmst.companycode
where salestype<>'RETURN' and salesbillmain.trndate >=@day1 and salesbillmain.trndate <@day2 
group by itemmst.itemcode,itemmst.itemname,rate,salesbillmain.companycode,storecode,salesbillmain.yearcode
union all
-- ===========adding those quantities back to the stock that has been returned back=====
select itemmst.itemcode,itemmst.itemname,rate,sum(qnty) as opening_stock,0 as bottles_purchased,0 as bottles_sold,0 as stock,salesbillmain.companycode,storecode,salesbillmain.yearcode 
from salesbillmain
join salesbilldetail 
on salesbillmain.companycode=salesbilldetail.companycode and salesbillmain.yearcode=salesbilldetail.yearcode and salesbillmain.trnno=salesbilldetail.trnno
join itemmst on itemmst.itemcode=salesbilldetail.itemcode and salesbilldetail.companycode=itemmst.companycode
where salestype='RETURN' and salesbillmain.trndate >=@day1 and salesbillmain.trndate <@day2 
group by itemmst.itemcode,itemmst.itemname,rate,salesbillmain.companycode,storecode,salesbillmain.yearcode
union all
-- ==========adding the quantities to the stock that has been purchased===========
select itemmst.itemcode,itemmst.itemname,0 as rate,sum(itemquantity) as opening_stock,0 as bottles_purchased,0 as bottles_sold,0 as stock,purchasemain.companycode,shopcode as storecode,purchasemain.yearcode 
from purchasemain
join purchasedetail 
on purchasedetail.companycode=purchasemain.companycode
and purchasedetail.trnno=purchasemain.trnno
and purchasedetail.yearcode=purchasemain.yearcode
join itemmst on itemmst.itemcode=purchasedetail.itemcode
and itemmst.companycode=purchasedetail.companycode
where ptype='PURCHASE' and purchasemain.trndate >=@day1 and purchasemain.trndate <@day2
group by itemmst.itemcode,itemmst.itemname,purchasemain.companycode,shopcode,purchasemain.yearcode
union all
-- ==========subtracting those quantities to the stock that has been purchased returned=========
select itemmst.itemcode,itemmst.itemname,0 as rate,-1*sum(itemquantity) as opening_stock,0 as bottles_purchased,0 as bottles_sold,0 as stock,purchasemain.companycode,shopcode as storecode,purchasemain.yearcode 
from purchasemain
join purchasedetail 
on purchasedetail.companycode=purchasemain.companycode
and purchasedetail.trnno=purchasemain.trnno
and purchasedetail.yearcode=purchasemain.yearcode
join itemmst on itemmst.itemcode=purchasedetail.itemcode
and itemmst.companycode=purchasedetail.companycode
where ptype='PURCHASE RETURN' and purchasemain.trndate >=@day1 and purchasemain.trndate <@day2
group by itemmst.itemcode,itemmst.itemname,purchasemain.companycode,shopcode,purchasemain.yearcode
union all
-- ==========for deducting the qunatities form the storage of the stock from where the items has been transferred from========
select itemmst.itemcode,itemname,0 as rate,(-1)*sum(qnty) as opening_stock,0 as bottles_purchased,0 as bottles_sold,0 as stock,stk_transfer_detail.companycode,shopcode_frm as storecode,stk_transfer_detail.yearcode
from stk_transfer_detail
join itemmst on itemmst.itemcode=stk_transfer_detail.itemcode and stk_transfer_detail.companycode=itemmst.companycode
join stk_transfer_main
on stk_transfer_main.companycode=stk_transfer_detail.companycode and stk_transfer_main.yearcode=stk_transfer_detail.yearcode and stk_transfer_main.trnno=stk_transfer_detail.trnno
where stk_transfer_main.trndate>=@day1 and stk_transfer_main.trndate<@day2
group by itemmst.itemcode,itemmst.itemname,stk_transfer_detail.companycode,shopcode_frm,stk_transfer_detail.yearcode
union all
-- ========for adding the qunatities to the storage of the stock  where the items has been transferred to===================
select itemmst.itemcode,itemname,0 as rate,sum(qnty) as opening_stock,0 as bottles_purchased,0 as bottles_sold,0 as stock,stk_transfer_detail.companycode,shopcode_to as storecode,stk_transfer_detail.yearcode
from stk_transfer_detail
join itemmst on itemmst.itemcode=stk_transfer_detail.itemcode and stk_transfer_detail.companycode=itemmst.companycode
join stk_transfer_main
on stk_transfer_main.companycode=stk_transfer_detail.companycode and stk_transfer_main.yearcode=stk_transfer_detail.yearcode and stk_transfer_main.trnno=stk_transfer_detail.trnno
where stk_transfer_main.trndate>=@day1 and stk_transfer_main.trndate<@day2
group by itemmst.itemcode,itemmst.itemname,stk_transfer_detail.companycode,shopcode_to,stk_transfer_detail.yearcode
union all
-- =====Query for deducting the breakage from the purchase =========
select breakagedetail.itemcode,itemname,0 as rate,-1*sum(quantity) as opening_stock,0 as bottles_purchased,0 as bottles_sold,0 as stock,breakagedetail.companycode,shopcode,breakagedetail.yearcode
from breakagedetail
join breakagemain
on breakagedetail.companycode=breakagemain.companycode
and breakagedetail.yearcode=breakagemain.yearcode
and breakagedetail.trnno=breakagemain.trnno
join itemmst 
on itemmst.itemcode=breakagedetail.itemcode
and itemmst.companycode=itemmst.companycode
where receivd='False' and breakagemain.trndate>=@day1 and breakagemain.trndate<@day2
group by breakagedetail.itemcode,itemname,breakagedetail.companycode,shopcode,breakagedetail.yearcode
union all
-- ======part 2 from >=day2 to <=day3=============
-- ==========query for getting the number of items sold=========
select itemmst.itemcode,itemmst.itemname,rate,0 as opening_stock,0 as bottles_purchased,sum(qnty) as bottles_sold,0 as stock,salesbillmain.companycode,storecode,salesbillmain.yearcode 
from salesbillmain
join salesbilldetail 
on salesbillmain.companycode=salesbilldetail.companycode and salesbillmain.yearcode=salesbilldetail.yearcode and salesbillmain.trnno=salesbilldetail.trnno
join itemmst on itemmst.itemcode=salesbilldetail.itemcode and salesbilldetail.companycode=itemmst.companycode
where salestype='cash' and salesbillmain.trndate >=@day2 and salesbillmain.trndate <=@day3 
or salestype='credit card' and salesbillmain.trndate >=@day2 and salesbillmain.trndate <=@day3 
or  salestype ='credit account' and salesbillmain.trndate >=@day2 and salesbillmain.trndate <=@day3
group by itemmst.itemcode,itemmst.itemname,rate,salesbillmain.companycode,storecode,salesbillmain.yearcode
union all
-- ========query for subtracting the returned bottles from the sold bottles=====
select itemmst.itemcode,itemmst.itemname,rate,0 as opening_stock,0 as bottles_purchased,(-1)*sum(qnty) as bottles_sold,0 as stock,salesbillmain.companycode,storecode,salesbillmain.yearcode 
from salesbillmain
join salesbilldetail 
on salesbillmain.companycode=salesbilldetail.companycode and salesbillmain.yearcode=salesbilldetail.yearcode and salesbillmain.trnno=salesbilldetail.trnno
join itemmst on itemmst.itemcode=salesbilldetail.itemcode and salesbilldetail.companycode=itemmst.companycode
where salestype='return' and salesbillmain.trndate >=@day2 and salesbillmain.trndate <=@day3 
group by itemmst.itemcode,itemmst.itemname,rate,salesbillmain.companycode,storecode,salesbillmain.yearcode
union all
-- =======query for getting the number of bottles purchased================
select itemmst.itemcode,itemmst.itemname,0 as rate,0 as opening_stock,sum(itemquantity) as bottles_purchased,0 as bottles_sold,0 as stock,purchasemain.companycode,shopcode as storecode,purchasemain.yearcode 
from purchasemain
join purchasedetail 
on purchasedetail.companycode=purchasemain.companycode
and purchasedetail.trnno=purchasemain.trnno
and purchasedetail.yearcode=purchasemain.yearcode
join itemmst on itemmst.itemcode=purchasedetail.itemcode
and itemmst.companycode=purchasedetail.companycode
where ptype='PURCHASE' and purchasemain.trndate >=@day2 and purchasemain.trndate <=@day3
group by itemmst.itemcode,itemmst.itemname,purchasemain.companycode,shopcode,purchasemain.yearcode
union all
--- ==============query for subtracting the returned bottles from the purchased bottles======
select itemmst.itemcode,itemmst.itemname,0 as rate,0 as opening_stock,(-1)*sum(itemquantity) as bottles_purchased,0 as bottles_sold,0 as stock,purchasemain.companycode,shopcode as storecode,purchasemain.yearcode 
from purchasemain
join purchasedetail 
on purchasedetail.companycode=purchasemain.companycode
and purchasedetail.trnno=purchasemain.trnno
and purchasedetail.yearcode=purchasemain.yearcode
join itemmst on itemmst.itemcode=purchasedetail.itemcode
and itemmst.companycode=purchasedetail.companycode
where ptype='PURCHASE RETURN' and purchasemain.trndate >=@day2 and purchasemain.trndate <=@day3
group by itemmst.itemcode,itemmst.itemname,purchasemain.companycode,shopcode,purchasemain.yearcode
union all
-- ==========for deducting the qunatities form the storage of the stock from where the items has been transferred from========
select itemmst.itemcode,itemname,0 as rate,0 as opening_stock,0 as bottles_purchased,0 as bottles_sold,(-1)*sum(qnty) as stock,stk_transfer_detail.companycode,shopcode_frm as storecode,stk_transfer_detail.yearcode
from stk_transfer_detail
join itemmst on itemmst.itemcode=stk_transfer_detail.itemcode and stk_transfer_detail.companycode=itemmst.companycode
join stk_transfer_main
on stk_transfer_main.companycode=stk_transfer_detail.companycode and stk_transfer_main.yearcode=stk_transfer_detail.yearcode and stk_transfer_main.trnno=stk_transfer_detail.trnno
where stk_transfer_main.trndate>=@day2 and stk_transfer_main.trndate<=@day3
group by itemmst.itemcode,itemmst.itemname,stk_transfer_detail.companycode,shopcode_frm,stk_transfer_detail.yearcode
union all
-- ========for adding the qunatities to the storage of the stock  where the items has been transferred to ===================
select itemmst.itemcode,itemname,0 as rate,0 as opening_stock,0 as bottles_purchased,0 as bottles_sold,sum(qnty) as stock,stk_transfer_detail.companycode,shopcode_to as storecode,stk_transfer_detail.yearcode
from stk_transfer_detail
join itemmst on itemmst.itemcode=stk_transfer_detail.itemcode and stk_transfer_detail.companycode=itemmst.companycode
join stk_transfer_main
on stk_transfer_main.companycode=stk_transfer_detail.companycode and stk_transfer_main.yearcode=stk_transfer_detail.yearcode and stk_transfer_main.trnno=stk_transfer_detail.trnno
where stk_transfer_main.trndate>=@day2 and stk_transfer_main.trndate<=@day3
group by itemmst.itemcode,itemmst.itemname,stk_transfer_detail.companycode,shopcode_to,stk_transfer_detail.yearcode
union all 
-- =====query for subtracting the breakageentry form the stock ==========
select breakagedetail.itemcode,itemname,0 as rate,0 as opening_stock,0 as bottles_purchased,0 as bottles_sold,-1*sum(quantity) as stock,breakagedetail.companycode,shopcode,breakagedetail.yearcode
from breakagedetail
join breakagemain
on breakagedetail.companycode=breakagemain.companycode
and breakagedetail.yearcode=breakagemain.yearcode
and breakagedetail.trnno=breakagemain.trnno
join itemmst 
on itemmst.itemcode=breakagedetail.itemcode
and itemmst.companycode=itemmst.companycode
where receivd='False' 
and breakagemain.trndate>=@day2 
and breakagemain.trndate<=@day3
group by breakagedetail.itemcode,itemname,breakagedetail.companycode,shopcode,breakagedetail.yearcode
)x 
group by itemcode,itemname,rate,companycode,storecode,yearcode

GO

-- ----- dbo.opening_stock_show -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE function [dbo].[opening_stock_show](@y varchar(6),@c varchar(6),@s varchar(6)) returns table
as
return
select @s as shopcode,itemcode,itemname,companycode,yearcode,sum(loose)as loose,sum(box) as box,sum(qnty)as qnty
from(
select 
shopcode,itemmst.itemname,itemmst.itemcode,itemmst.companycode,yearcode,sum(loose)as loose,sum(box) as box,sum(qnty)as qnty
from openingstockmst
join itemmst
on openingstockmst.companycode=itemmst.companycode
and openingstockmst.itemcode=itemmst.itemcode
where yearcode=@y and itemmst.companycode=@c and shopcode=@s
group by shopcode,itemmst.itemcode,itemname,itemmst.companycode,yearcode
union all
select @s as shopcode,itemname,itemcode,companycode,@y as yearcode,'0' as loose,'0' as box,'0' as qnty
from itemmst
where companycode=@c
group by itemcode,itemname,companycode
)x group by shopcode,itemcode,itemname,companycode,yearcode

GO

-- ----- dbo.sales_statement -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE function [dbo].[sales_statement](@day1 datetime,@day2 datetime)returns table
as
return
select itemcode,itemname,companycode,storecode,yearcode,sum(itemamount) as itemamount,rate ,ml,sum(qnty) as qnty,ratecode
from(
select itemmst.itemcode,itemmst.itemname,salesbillmain.companycode,storecode,salesbillmain.yearcode,sum(itemamount) as itemamount,rate ,ml,sum(qnty) as qnty,ratecode
from salesbillmain
join salesbilldetail 
on salesbillmain.companycode=salesbilldetail.companycode and salesbillmain.yearcode=salesbilldetail.yearcode and salesbillmain.trnno=salesbilldetail.trnno
join itemmst on itemmst.itemcode=salesbilldetail.itemcode and salesbilldetail.companycode=itemmst.companycode
where salestype='cash'  and salesbillmain.trndate >=@day1 and salesbillmain.trndate <=@day2 
or salestype='credit card' and salesbillmain.trndate >=@day1 and salesbillmain.trndate <=@day2 
or  salestype ='credit account' and salesbillmain.trndate >=@day1 and salesbillmain.trndate <=@day2 
group by itemmst.itemcode,itemmst.itemname,salesbillmain.companycode,storecode,salesbillmain.yearcode,ratecode,rate,ml
union all
select itemmst.itemcode,itemmst.itemname,salesbillmain.companycode,storecode,salesbillmain.yearcode,(-1)*sum(itemamount) as itemamount,rate ,ml,(-1)*sum(qnty) as qnty,ratecode
from salesbillmain
join salesbilldetail 
on salesbillmain.companycode=salesbilldetail.companycode and salesbillmain.yearcode=salesbilldetail.yearcode and salesbillmain.trnno=salesbilldetail.trnno
join itemmst on itemmst.itemcode=salesbilldetail.itemcode and salesbilldetail.companycode=itemmst.companycode
where salestype='return'  and salesbillmain.trndate >=@day1 and salesbillmain.trndate <=@day2  
group by itemmst.itemcode,itemmst.itemname,salesbillmain.companycode,storecode,salesbillmain.yearcode,ratecode,rate,ml
)x group by itemcode,itemname,companycode,storecode,yearcode,rate,ml,ratecode
GO

-- ----- dbo.stock_by_day -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE function [dbo].[stock_by_day](@day1 datetime ,@day2 datetime) returns table
as
return
select itemcode,itemname,sum(bottles_purchased) as bottles_purchased,sum(bottles_sold) as bottles_sold,sum(stock)+ sum(bottles_purchased) -sum(bottles_sold)as stock,companycode,storecode,yearcode
from(
-- ==========for adding the qunatities in the stock==========
select itemmst.itemcode,itemname,0 as bottles_purchased,0 as bottles_sold,sum(qnty) as stock,itemmst.companycode,shopcode as storecode,yearcode
from openingstockmst
join itemmst 
on 
openingstockmst.itemcode=itemmst.itemcode 
and openingstockmst.companycode=itemmst.companycode
group by itemmst.itemcode,itemmst.itemname,itemmst.companycode,shopcode,openingstockmst.yearcode

union all

-- ==========query for getting the number of items sold
select itemmst.itemcode,itemmst.itemname,0 as bottles_purchased,sum(qnty) as bottles_sold,0 as stock,salesbillmain.companycode,storecode,salesbillmain.yearcode 
from salesbillmain
join salesbilldetail 
on salesbillmain.companycode=salesbilldetail.companycode and salesbillmain.yearcode=salesbilldetail.yearcode and salesbillmain.trnno=salesbilldetail.trnno
join itemmst on itemmst.itemcode=salesbilldetail.itemcode and salesbilldetail.companycode=itemmst.companycode
where salestype='cash' and salesbillmain.trndate >=@day1 and salesbillmain.trndate <=@day2 or salestype='credit card' and salesbillmain.trndate >=@day1 and salesbillmain.trndate <=@day2 or  salestype ='credit account' and salesbillmain.trndate >=@day1 and salesbillmain.trndate <=@day2
group by itemmst.itemcode,itemmst.itemname,salesbillmain.companycode,storecode,salesbillmain.yearcode

union all

-- ========query for subtracting the returned bottles from the sold bottles=====
select itemmst.itemcode,itemmst.itemname,0 as bottles_purchased,(-1)*sum(qnty) as bottles_sold,0 as stock,salesbillmain.companycode,storecode,salesbillmain.yearcode 
from salesbillmain
join salesbilldetail 
on salesbillmain.companycode=salesbilldetail.companycode and salesbillmain.yearcode=salesbilldetail.yearcode and salesbillmain.trnno=salesbilldetail.trnno
join itemmst on itemmst.itemcode=salesbilldetail.itemcode and salesbilldetail.companycode=itemmst.companycode
where salestype='return' and salesbillmain.trndate >=@day1 and salesbillmain.trndate <=@day2 
group by itemmst.itemcode,itemmst.itemname,salesbillmain.companycode,storecode,salesbillmain.yearcode

union all

-- =======query for getting the number of bottles purchased================
select itemmst.itemcode,itemmst.itemname,sum(itemquantity) as bottles_purchased,0 as bottles_sold,0 as stock,purchasemain.companycode,shopcode as storecode,purchasemain.yearcode 
from purchasemain
join purchasedetail 
on purchasedetail.companycode=purchasemain.companycode
and purchasedetail.trnno=purchasemain.trnno
and purchasedetail.yearcode=purchasemain.yearcode
join itemmst on itemmst.itemcode=purchasedetail.itemcode
and itemmst.companycode=purchasedetail.companycode
where ptype='PURCHASE' and purchasemain.trndate >=@day1 and purchasemain.trndate <=@day2
group by itemmst.itemcode,itemmst.itemname,purchasemain.companycode,shopcode,purchasemain.yearcode


union all

--- ==============query for subtracting the returned bottles from the purchased bottles======
select itemmst.itemcode,itemmst.itemname,(-1)*sum(itemquantity) as bottles_purchased,0 as bottles_sold,0 as stock,purchasemain.companycode,shopcode as storecode,purchasemain.yearcode 
from purchasemain
join purchasedetail 
on purchasedetail.companycode=purchasemain.companycode
and purchasedetail.trnno=purchasemain.trnno
and purchasedetail.yearcode=purchasemain.yearcode
join itemmst on itemmst.itemcode=purchasedetail.itemcode
and itemmst.companycode=purchasedetail.companycode
where ptype='PURCHASE RETURN' and purchasemain.trndate >=@day1 and purchasemain.trndate <=@day2
group by itemmst.itemcode,itemmst.itemname,purchasemain.companycode,shopcode,purchasemain.yearcode

)x 
group by itemcode,itemname,companycode,storecode,yearcode

GO

-- ----- dbo.xcise_statement_final -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE function [dbo].[xcise_statement_final](@day1 as datetime,@day2 as datetime) returns table as return  select companycode,yearcode,kflname,type,groupname,categoryname,ml,isnull(sum(qnty),0) as 'qnty',batchno,party,billno,tppassno  from(select itemmst.companycode,yearcode,kflname,'1-Opening' as type,groupname,categoryname,ml,opening_stock as 'qnty','' as batchno,'' as party,'' as billno,'' as tppassno  from  opening_closing_stock(@day1,@day2,@day2)  join itemmst on itemmst.itemcode=opening_closing_stock.itemcode and itemmst.companycode=opening_closing_stock.companycode  join groupmst on groupmst.groupcode=itemmst.groupcode and groupmst.companycode=itemmst.companycode  join categorymst on categorymst.categorycode=itemmst.categorycode and categorymst.companycode=itemmst.companycode  join kflmst on kflmst.companycode=itemmst.companycode and kflmst.kflcode=itemmst.kflcode  where not (opening_stock=0 and bottles_purchased=0 and stock=0 and bottles_sold=0)  union all  SELECT purchasemain.companycode,purchasemain.yearcode,kflmst.kflname, '2-Purchase' AS type, groupmst.groupname, categorymst.categoryname, itemmst.ml, SUM(purchasedetail.itemquantity) AS qnty,batchno,name as party,docno as billno,tppassno FROM purchasemain JOIN purchasedetail ON purchasemain.companycode = purchasedetail.companycode AND purchasemain.yearcode = purchasedetail.yearcode AND purchasemain.docdate = purchasedetail.trndate AND purchasemain.trnno = purchasedetail.trnno JOIN ledger ON purchasemain.companycode = ledger.companycode AND purchasemain.suppliercode = ledger.ledcode JOIN itemmst JOIN kflmst ON itemmst.companycode = kflmst.companycode AND itemmst.kflcode = kflmst.kflcode JOIN groupmst ON itemmst.companycode = groupmst.companycode AND itemmst.groupcode = groupmst.groupcode JOIN categorymst ON itemmst.companycode = categorymst.companycode AND itemmst.categorycode = categorymst.categorycode ON purchasedetail.companycode = itemmst.companycode AND purchasedetail.itemcode = itemmst.itemcode WHERE (purchasemain.trndate = @day2) GROUP BY purchasemain.companycode,purchasemain.yearcode,kflmst.kflname, groupmst.groupname, categorymst.categoryname, itemmst.ml,batchno,name,docno,tppassno union all  select itemmst.companycode,yearcode,kflname,'3-Total' as type,groupname,categoryname,ml,bottles_purchased+opening_stock as 'qnty','' as batchno,'' as party,'' as billno,'' as tppassno  from opening_closing_stock(@day1,@day2,@day2)  join itemmst on itemmst.itemcode=opening_closing_stock.itemcode and itemmst.companycode=opening_closing_stock.companycode  join groupmst on groupmst.groupcode=itemmst.groupcode and groupmst.companycode=itemmst.companycode  join categorymst on categorymst.categorycode=itemmst.categorycode and categorymst.companycode=itemmst.companycode  join kflmst on kflmst.companycode=itemmst.companycode and kflmst.kflcode=itemmst.kflcode  where not (opening_stock=0 and bottles_purchased=0 and stock=0 and bottles_sold=0)  union all  select itemmst.companycode,yearcode,kflname,'4-Closing' as type,groupname,categoryname,ml,stock as 'qnty','' as batchno,'' as party,'' as billno,'' as tppassno  from  opening_closing_stock(@day1,@day2,@day2)  join itemmst on itemmst.itemcode=opening_closing_stock.itemcode and itemmst.companycode=opening_closing_stock.companycode  join groupmst on groupmst.groupcode=itemmst.groupcode and groupmst.companycode=itemmst.companycode  join categorymst on categorymst.categorycode=itemmst.categorycode and categorymst.companycode=itemmst.companycode  join kflmst on kflmst.companycode=itemmst.companycode and kflmst.kflcode=itemmst.kflcode  where not (opening_stock=0 and bottles_purchased=0 and stock=0 and bottles_sold=0)  union all  select itemmst.companycode,yearcode,kflname,'5-Difference' as type,groupname,categoryname,ml,bottles_sold as 'qnty','' as batchno,'' as party,'' as billno,'' as tppassno  from  opening_closing_stock(@day1,@day2,@day2)  join itemmst on itemmst.itemcode=opening_closing_stock.itemcode and itemmst.companycode=opening_closing_stock.companycode  join groupmst on groupmst.groupcode=itemmst.groupcode and groupmst.companycode=itemmst.companycode  join categorymst on categorymst.categorycode=itemmst.categorycode and categorymst.companycode=itemmst.companycode  join kflmst on kflmst.companycode=itemmst.companycode and kflmst.kflcode=itemmst.kflcode  where not (opening_stock=0 and bottles_purchased=0 and stock=0 and bottles_sold=0) union all  select itemmst.companycode,yearcode,kflname,'6-Difference in BL' as type,groupname,categoryname,ml,convert(float,convert(float,ml*bottles_sold)/1000) as 'qnty','' as batchno,'' as party,'' as billno,'' as tppassno  from  opening_closing_stock(@day1,@day2,@day2)  join itemmst on itemmst.itemcode=opening_closing_stock.itemcode and itemmst.companycode=opening_closing_stock.companycode  join groupmst on groupmst.groupcode=itemmst.groupcode and groupmst.companycode=itemmst.companycode  join categorymst on categorymst.categorycode=itemmst.categorycode and categorymst.companycode=itemmst.companycode  join kflmst on kflmst.companycode=itemmst.companycode and kflmst.kflcode=itemmst.kflcode  where not (opening_stock=0 and bottles_purchased=0 and stock=0 and bottles_sold=0) union all select itemmst.companycode,yearcode,kflname,'5-Difference' as type,groupname,categoryname,ml,breakage as 'qnty','' as batchno,'' as party,'' as billno,'' as tppassno  from  opening_closing_stock(@day1,@day2,@day2)  join itemmst on itemmst.itemcode=opening_closing_stock.itemcode and itemmst.companycode=opening_closing_stock.companycode  join groupmst on groupmst.groupcode=itemmst.groupcode and groupmst.companycode=itemmst.companycode  join categorymst on categorymst.categorycode=itemmst.categorycode and categorymst.companycode=itemmst.companycode  join kflmst on kflmst.companycode=itemmst.companycode and kflmst.kflcode=itemmst.kflcode  where(Not (opening_stock = 0 And bottles_purchased = 0 And stock = 0 And bottles_sold = 0)) union all select itemmst.companycode,yearcode,kflname,'6-Difference in BL' as type,groupname,categoryname,ml,convert(float,convert(float,ml*breakage)/1000) as 'qnty','' as batchno,'' as party,'' as billno,'' as tppassno  from  opening_closing_stock(@day1,@day2,@day2)  join itemmst on itemmst.itemcode=opening_closing_stock.itemcode and itemmst.companycode=opening_closing_stock.companycode  join groupmst on groupmst.groupcode=itemmst.groupcode and groupmst.companycode=itemmst.companycode  join categorymst on categorymst.categorycode=itemmst.categorycode and categorymst.companycode=itemmst.companycode  join kflmst on kflmst.companycode=itemmst.companycode and kflmst.kflcode=itemmst.kflcode  where not (opening_stock=0 and bottles_purchased=0 and stock=0 and bottles_sold=0) ) as x group by companycode,yearcode,kflname,type,groupname,categoryname,ml,batchno,party,billno,tppassno  
GO

-- ----- dbo.xcise_statement1 -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE function [dbo].[xcise_statement1](@day1 as datetime,@day2 as datetime,@day3 as datetime) returns table as return  select companycode,yearcode, storecode,kflname,type,groupname,categoryname,ml,sum(qnty) as 'qnty' , strengthName  from(   select itemmst.companycode,yearcode, storecode,kflname,'1-Opening' as type,groupname,categoryname,ml,opening_stock as 'qnty',strengthName  from opening_closing_stock(@day1,@day2,@day3 )  right outer join itemmst on itemmst.itemcode=opening_closing_stock.itemcode and itemmst.companycode=opening_closing_stock.companycode  right outer join groupmst on groupmst.groupcode=itemmst.groupcode and groupmst.companycode=itemmst.companycode  right outer join categorymst on categorymst.categorycode=itemmst.categorycode and categorymst.companycode=itemmst.companycode  right outer join kflmst on kflmst.companycode=itemmst.companycode and kflmst.kflcode=itemmst.kflcode  union all  select itemmst.companycode,yearcode, storecode,kflname,'2-Purchase' as type,groupname,categoryname,ml,bottles_purchased as 'qnty' , strengthName  from opening_closing_stock(@day1,@day2,@day3)  right outer join itemmst on itemmst.itemcode=opening_closing_stock.itemcode and itemmst.companycode=opening_closing_stock.companycode  right outer join groupmst on groupmst.groupcode=itemmst.groupcode and groupmst.companycode=itemmst.companycode  right outer join categorymst on categorymst.categorycode=itemmst.categorycode and categorymst.companycode=itemmst.companycode  right outer join kflmst on kflmst.companycode=itemmst.companycode and kflmst.kflcode=itemmst.kflcode  union all  select itemmst.companycode,yearcode, storecode,kflname,'3-Total' as type,groupname,categoryname,ml,bottles_purchased+opening_stock as 'qnty' , strengthName  from opening_closing_stock(@day1,@day2,@day3)  right outer join itemmst on itemmst.itemcode=opening_closing_stock.itemcode and itemmst.companycode=opening_closing_stock.companycode  right outer join groupmst on groupmst.groupcode=itemmst.groupcode and groupmst.companycode=itemmst.companycode  right outer join categorymst on categorymst.categorycode=itemmst.categorycode and categorymst.companycode=itemmst.companycode  right outer join kflmst on kflmst.companycode=itemmst.companycode and kflmst.kflcode=itemmst.kflcode  union all  select itemmst.companycode,yearcode, storecode,kflname,'4-Closing' as type,groupname,categoryname,ml,stock as 'qnty' , strengthName  from opening_closing_stock(@day1,@day2,@day3)  right outer join itemmst on itemmst.itemcode=opening_closing_stock.itemcode and itemmst.companycode=opening_closing_stock.companycode  right outer join groupmst on groupmst.groupcode=itemmst.groupcode and groupmst.companycode=itemmst.companycode  right outer join categorymst on categorymst.categorycode=itemmst.categorycode and categorymst.companycode=itemmst.companycode  right outer join kflmst on kflmst.companycode=itemmst.companycode and kflmst.kflcode=itemmst.kflcode  union all  select itemmst.companycode,yearcode, storecode,kflname,'5-Difference' as type,groupname,categoryname,ml,bottles_sold as 'qnty' , strengthName  from opening_closing_stock(@day1,@day2,@day3)  right outer join itemmst on itemmst.itemcode=opening_closing_stock.itemcode and itemmst.companycode=opening_closing_stock.companycode  right outer join groupmst on groupmst.groupcode=itemmst.groupcode and groupmst.companycode=itemmst.companycode  right outer join categorymst on categorymst.categorycode=itemmst.categorycode and categorymst.companycode=itemmst.companycode  right outer join kflmst on kflmst.companycode=itemmst.companycode and kflmst.kflcode=itemmst.kflcode  union all  select itemmst.companycode,yearcode, storecode,kflname,'6-Difference in BL' as type,groupname,categoryname,ml,convert(float,convert(float,ml*bottles_sold)/1000) as 'qnty' , strengthName  from opening_closing_stock(@day1,@day2,@day3)  right outer join itemmst on itemmst.itemcode=opening_closing_stock.itemcode and itemmst.companycode=opening_closing_stock.companycode  right outer join groupmst on groupmst.groupcode=itemmst.groupcode and groupmst.companycode=itemmst.companycode  right outer join categorymst on categorymst.categorycode=itemmst.categorycode and categorymst.companycode=itemmst.companycode  right outer join kflmst on kflmst.companycode=itemmst.companycode and kflmst.kflcode=itemmst.kflcode  Union all select itemmst.companycode,yearcode, storecode,kflname,'5-Difference' as type,groupname,categoryname,ml,breakage as 'qnty' , strengthName  from opening_closing_stock(@day1,@day2,@day3)  right outer join itemmst on itemmst.itemcode=opening_closing_stock.itemcode and itemmst.companycode=opening_closing_stock.companycode  right outer join groupmst on groupmst.groupcode=itemmst.groupcode and groupmst.companycode=itemmst.companycode  right outer join categorymst on categorymst.categorycode=itemmst.categorycode and categorymst.companycode=itemmst.companycode  right outer join kflmst on kflmst.companycode=itemmst.companycode and kflmst.kflcode=itemmst.kflcode  union all  select itemmst.companycode,yearcode, storecode,kflname,'6-Difference in BL' as type,groupname,categoryname,ml,convert(float,convert(float,ml*breakage)/1000) as 'qnty' , strengthName  from opening_closing_stock(@day1,@day2,@day3)  right outer join itemmst on itemmst.itemcode=opening_closing_stock.itemcode and itemmst.companycode=opening_closing_stock.companycode  right outer join groupmst on groupmst.groupcode=itemmst.groupcode and groupmst.companycode=itemmst.companycode  right outer join categorymst on categorymst.categorycode=itemmst.categorycode and categorymst.companycode=itemmst.companycode  right outer join kflmst on kflmst.companycode=itemmst.companycode and kflmst.kflcode=itemmst.kflcode  ) as x group by companycode,yearcode, storecode,kflname,type,groupname,categoryname,ml,strengthName  
GO

-- ----- dbo.xcise_statement2 -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE function [dbo].[xcise_statement2](@day1 as datetime,@day2 as datetime,@day3 as datetime) returns table as return select companycode,yearcode,kflname,type,groupname,categoryname,ml,sum(qnty) as 'qnty' from(select itemmst.companycode,yearcode,kflname,'1-Opening' as type,groupname,categoryname,ml,opening_stock as 'qnty' from  opening_closing_stock(@day1,@day2,@day3) join itemmst on itemmst.itemcode=opening_closing_stock.itemcode and itemmst.companycode=opening_closing_stock.companycode join groupmst on groupmst.groupcode=itemmst.groupcode and groupmst.companycode=itemmst.companycode join categorymst on categorymst.categorycode=itemmst.categorycode and categorymst.companycode=itemmst.companycode join kflmst on kflmst.companycode=itemmst.companycode and kflmst.kflcode=itemmst.kflcode where not (opening_stock=0 and bottles_purchased=0 and stock=0 and bottles_sold=0) union all select itemmst.companycode,yearcode,kflname,'2-Purchase' as type,groupname,categoryname,ml,bottles_purchased as 'qnty' from opening_closing_stock(@day1,@day2,@day3) join itemmst on itemmst.itemcode=opening_closing_stock.itemcode and itemmst.companycode=opening_closing_stock.companycode join groupmst on groupmst.groupcode=itemmst.groupcode and groupmst.companycode=itemmst.companycode join categorymst on categorymst.categorycode=itemmst.categorycode and categorymst.companycode=itemmst.companycode join kflmst on kflmst.companycode=itemmst.companycode and kflmst.kflcode=itemmst.kflcode where not (opening_stock=0 and bottles_purchased=0 and stock=0 and bottles_sold=0) union all select itemmst.companycode,yearcode,kflname,'3-Total' as type,groupname,categoryname,ml,bottles_purchased+opening_stock as 'qnty' from opening_closing_stock(@day1,@day2,@day3) join itemmst on itemmst.itemcode=opening_closing_stock.itemcode and itemmst.companycode=opening_closing_stock.companycode join groupmst on groupmst.groupcode=itemmst.groupcode and groupmst.companycode=itemmst.companycode join categorymst on categorymst.categorycode=itemmst.categorycode and categorymst.companycode=itemmst.companycode join kflmst on kflmst.companycode=itemmst.companycode and kflmst.kflcode=itemmst.kflcode where not (opening_stock=0 and bottles_purchased=0 and stock=0 and bottles_sold=0) union all select itemmst.companycode,yearcode,kflname,'4-Closing' as type,groupname,categoryname,ml,stock as 'qnty' from  opening_closing_stock(@day1,@day2,@day3) join itemmst on itemmst.itemcode=opening_closing_stock.itemcode and itemmst.companycode=opening_closing_stock.companycode join groupmst on groupmst.groupcode=itemmst.groupcode and groupmst.companycode=itemmst.companycode join categorymst on categorymst.categorycode=itemmst.categorycode and categorymst.companycode=itemmst.companycode join kflmst on kflmst.companycode=itemmst.companycode and kflmst.kflcode=itemmst.kflcode where not (opening_stock=0 and bottles_purchased=0 and stock=0 and bottles_sold=0) union all select itemmst.companycode,yearcode,kflname,'5-Difference' as type,groupname,categoryname,ml,bottles_sold as 'qnty' from  opening_closing_stock(@day1,@day2,@day3) join itemmst on itemmst.itemcode=opening_closing_stock.itemcode and itemmst.companycode=opening_closing_stock.companycode join groupmst on groupmst.groupcode=itemmst.groupcode and groupmst.companycode=itemmst.companycode join categorymst on categorymst.categorycode=itemmst.categorycode and categorymst.companycode=itemmst.companycode join kflmst on kflmst.companycode=itemmst.companycode and kflmst.kflcode=itemmst.kflcode where not (opening_stock=0 and bottles_purchased=0 and stock=0 and bottles_sold=0)union all select itemmst.companycode,yearcode,kflname,'6-Difference in BL' as type,groupname,categoryname,ml,convert(float,convert(float,ml*bottles_sold)/1000) as 'qnty' from  opening_closing_stock(@day1,@day2,@day3) join itemmst on itemmst.itemcode=opening_closing_stock.itemcode and itemmst.companycode=opening_closing_stock.companycode join groupmst on groupmst.groupcode=itemmst.groupcode and groupmst.companycode=itemmst.companycode join categorymst on categorymst.categorycode=itemmst.categorycode and categorymst.companycode=itemmst.companycode join kflmst on kflmst.companycode=itemmst.companycode and kflmst.kflcode=itemmst.kflcode where not (opening_stock=0 and bottles_purchased=0 and stock=0 and bottles_sold=0)union all select itemmst.companycode,yearcode,kflname,'5-Difference' as type,groupname,categoryname,ml,breakage as 'qnty' from  opening_closing_stock(@day1,@day2,@day3) join itemmst on itemmst.itemcode=opening_closing_stock.itemcode and itemmst.companycode=opening_closing_stock.companycode join groupmst on groupmst.groupcode=itemmst.groupcode and groupmst.companycode=itemmst.companycode join categorymst on categorymst.categorycode=itemmst.categorycode and categorymst.companycode=itemmst.companycode join kflmst on kflmst.companycode=itemmst.companycode and kflmst.kflcode=itemmst.kflcode where not (opening_stock=0 and bottles_purchased=0 and stock=0 and bottles_sold=0)union all select itemmst.companycode,yearcode,kflname,'6-Difference in BL' as type,groupname,categoryname,ml,convert(float,convert(float,ml*breakage)/1000) as 'qnty' from  opening_closing_stock(@day1,@day2,@day3) join itemmst on itemmst.itemcode=opening_closing_stock.itemcode and itemmst.companycode=opening_closing_stock.companycode join groupmst on groupmst.groupcode=itemmst.groupcode and groupmst.companycode=itemmst.companycode join categorymst on categorymst.categorycode=itemmst.categorycode and categorymst.companycode=itemmst.companycode join kflmst on kflmst.companycode=itemmst.companycode and kflmst.kflcode=itemmst.kflcode where not (opening_stock=0 and bottles_purchased=0 and stock=0 and bottles_sold=0)) as x group by companycode,yearcode,kflname,type,groupname,categoryname,ml
GO

-- ----- dbo.xcise_stock -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
create function [dbo].[xcise_stock](@day1 as datetime,@day2 as datetime,@day3 as datetime,@companycode as int,@yearcode as int) returns table
as
return
SELECT @day2 as trndate,groupname,categoryname,ml,qtype,qnty,party,batch from(
SELECT groupname,categoryname,ml,'0 OPENING' AS QTYPE ,SUM(opening_stock) AS qnty,'' as party,''as batch
FROM dbo.opening_closing_stock(@day1,@day2,@day3) 
join itemmst ON itemmst.itemcode = opening_closing_stock.itemcode AND itemmst.companycode = opening_closing_stock.companycode 
join groupmst ON groupmst.groupcode = itemmst.groupcode AND groupmst.companycode = itemmst.companycode 
join categorymst ON itemmst.companycode = categorymst.companycode AND itemmst.categorycode = categorymst.categorycode 
WHERE opening_closing_stock.companycode = @companycode AND opening_closing_stock.yearcode = @yearcode 
GROUP BY groupname,categoryname,ml 

union all


SELECT groupname,categoryname,ml,'1 PURCHASE' AS QTYPE ,SUM(bottles_purchased) AS qnty,
	STUFF(
       (
		select name +', ' 
		from purchasemain
		join ledger on purchasemain.companycode=ledger.companycode
		and purchasemain.suppliercode=ledger.ledcode
		WHERE purchasemain.companycode = @companycode AND purchasemain.yearcode =  @yearcode and purchasemain.trndate =@day2
		group by name
		FOR XML PATH ('')
        )
  ,1,2,'') AS party,
	STUFF(
       (
		select batchno +', ' 
		from purchasemain
		join purchasedetail on purchasemain.companycode=purchasedetail.companycode
		and purchasemain.yearcode=purchasedetail.yearcode
		and purchasemain.trnno=purchasedetail.trnno
		WHERE purchasemain.companycode = @companycode AND purchasemain.yearcode =  @yearcode and purchasemain.trndate =@day2
		group by batchno
		FOR XML PATH ('')
        )
  ,1,2,'') AS batch
FROM dbo.opening_closing_stock(@day1,@day2,@day3) 
join itemmst ON itemmst.itemcode = opening_closing_stock.itemcode AND itemmst.companycode = opening_closing_stock.companycode 
join groupmst ON groupmst.groupcode = itemmst.groupcode AND groupmst.companycode = itemmst.companycode 
join categorymst ON itemmst.companycode = categorymst.companycode AND itemmst.categorycode = categorymst.categorycode 
WHERE opening_closing_stock.companycode = @companycode AND opening_closing_stock.yearcode =  @yearcode
GROUP BY groupname,categoryname,ml


union all
SELECT groupname,categoryname,ml,'2 TOTAL' AS QTYPE ,SUM(opening_stock+bottles_purchased) AS qnty ,'' as party,''as batch
FROM dbo.opening_closing_stock(@day1,@day2,@day3) 
join itemmst ON itemmst.itemcode = opening_closing_stock.itemcode AND itemmst.companycode = opening_closing_stock.companycode 
join groupmst ON groupmst.groupcode = itemmst.groupcode AND groupmst.companycode = itemmst.companycode 
join categorymst ON itemmst.companycode = categorymst.companycode AND itemmst.categorycode = categorymst.categorycode 
WHERE opening_closing_stock.companycode =@companycode AND opening_closing_stock.yearcode = @yearcode
GROUP BY groupname,categoryname,ml 
union all
SELECT groupname,categoryname,ml,'3 CLOSING' AS QTYPE ,SUM(stock) AS qnty ,'' as party,''as batch
FROM dbo.opening_closing_stock(@day1,@day2,@day3) 
join itemmst ON itemmst.itemcode = opening_closing_stock.itemcode AND itemmst.companycode = opening_closing_stock.companycode 
join groupmst ON groupmst.groupcode = itemmst.groupcode AND groupmst.companycode = itemmst.companycode 
join categorymst ON itemmst.companycode = categorymst.companycode AND itemmst.categorycode = categorymst.categorycode 
WHERE opening_closing_stock.companycode = @companycode AND opening_closing_stock.yearcode = @yearcode
GROUP BY groupname,categoryname,ml
UNION ALL
SELECT groupname,categoryname,ml,'4 SALES' AS QTYPE ,SUM(bottles_sold) AS qnty ,'' as party,''as batch
FROM dbo.opening_closing_stock(@day1,@day2,@day3) 
join itemmst ON itemmst.itemcode = opening_closing_stock.itemcode AND itemmst.companycode = opening_closing_stock.companycode 
join groupmst ON groupmst.groupcode = itemmst.groupcode AND groupmst.companycode = itemmst.companycode 
join categorymst ON itemmst.companycode = categorymst.companycode AND itemmst.categorycode = categorymst.categorycode 
WHERE opening_closing_stock.companycode =@companycode AND opening_closing_stock.yearcode = @yearcode
GROUP BY groupname,categoryname,ml 
) as x
GO

-- ============================================================
-- 6. STORED PROCEDURES
-- ============================================================
-- ----- dbo.serialise_n_reference_sale_bills -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
create procedure [dbo].[serialise_n_reference_sale_bills] @companycode varchar(6),@yearcode varchar(6) as begin begin tran t1 declare @trnno_previous int declare @trnno int declare @diff int declare @count int set @count=0 set @trnno_previous=0 declare c cursor for select convert(int,trnno) from salesbillmain where companycode=@companycode and yearcode =@yearcode  order by convert(int,trnno) asc open c fetch c into @trnno while @@fetch_status=0 begin  set @diff=@trnno-@trnno_previous-1 if @diff>0 begin set @count=@count+1 End set @trnno_previous=@trnno fetch c into @trnno End Close c deallocate c while @count>0  begin set @trnno_previous=0 declare c cursor for select convert(int,trnno) from salesbillmain where companycode=@companycode and yearcode =@yearcode  order by convert(int,trnno) asc  open c fetch c into @trnno while @@fetch_status=0  begin set @diff=@trnno-@trnno_previous-1 if @diff>0  begin update salesbillmain set trnno=(convert(int,trnno)-@diff) where convert(int,trnno)>=@trnno and companycode=@companycode and yearcode =@yearcode  End set @trnno_previous=@trnno fetch c into @trnno  End Close c  deallocate c set @count=@count-1  End commit tran t1  End 
GO

-- ----- dbo.usp_exciseStatement -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE PROCEDURE [dbo].[usp_exciseStatement] @day1 datetime,@day2 datetime AS select companycode,yearcode,kflname,type,groupname,categoryname,ml,isnull(sum(qnty),0) as 'qnty',batchno,party,billno,tppassno ,trndate from(select itemmst.companycode,yearcode,kflname,'1-Opening' as type,groupname,categoryname,ml,opening_stock as 'qnty','' as batchno,'' as party,'' as billno,'' as tppassno ,@day2 as trnDate from  opening_closing_stock(@day1,@day2,@day2)  join itemmst on itemmst.itemcode=opening_closing_stock.itemcode and itemmst.companycode=opening_closing_stock.companycode  join groupmst on groupmst.groupcode=itemmst.groupcode and groupmst.companycode=itemmst.companycode  join categorymst on categorymst.categorycode=itemmst.categorycode and categorymst.companycode=itemmst.companycode  join kflmst on kflmst.companycode=itemmst.companycode and kflmst.kflcode=itemmst.kflcode  where not (opening_stock=0 and bottles_purchased=0 and stock=0 and bottles_sold=0)  union all  SELECT purchasemain.companycode,purchasemain.yearcode,kflmst.kflname, '2-Purchase' AS type, groupmst.groupname, categorymst.categoryname, itemmst.ml, SUM(purchasedetail.itemquantity) AS qnty,batchno,name as party,docno as billno,tppassno,@day2 as trnDate FROM purchasemain JOIN purchasedetail ON purchasemain.companycode = purchasedetail.companycode AND purchasemain.yearcode = purchasedetail.yearcode AND purchasemain.docdate = purchasedetail.trndate AND purchasemain.trnno = purchasedetail.trnno JOIN ledger ON purchasemain.companycode = ledger.companycode AND purchasemain.suppliercode = ledger.ledcode JOIN itemmst JOIN kflmst ON itemmst.companycode = kflmst.companycode AND itemmst.kflcode = kflmst.kflcode JOIN groupmst ON itemmst.companycode = groupmst.companycode AND itemmst.groupcode = groupmst.groupcode JOIN categorymst ON itemmst.companycode = categorymst.companycode AND itemmst.categorycode = categorymst.categorycode ON purchasedetail.companycode = itemmst.companycode AND purchasedetail.itemcode = itemmst.itemcode WHERE (purchasemain.trndate = @day2) GROUP BY purchasemain.companycode,purchasemain.yearcode,kflmst.kflname, groupmst.groupname, categorymst.categoryname, itemmst.ml,batchno,name,docno,tppassno,purchasemain.trndate union all  select itemmst.companycode,yearcode,kflname,'3-Total' as type,groupname,categoryname,ml,bottles_purchased+opening_stock as 'qnty','' as batchno,'' as party,'' as billno,'' as tppassno,@day2 as trnDate  from opening_closing_stock(@day1,@day2,@day2)  join itemmst on itemmst.itemcode=opening_closing_stock.itemcode and itemmst.companycode=opening_closing_stock.companycode  join groupmst on groupmst.groupcode=itemmst.groupcode and groupmst.companycode=itemmst.companycode join categorymst on categorymst.categorycode=itemmst.categorycode and categorymst.companycode=itemmst.companycode  join kflmst on kflmst.companycode=itemmst.companycode and kflmst.kflcode=itemmst.kflcode  where not (opening_stock=0 and bottles_purchased=0 and stock=0 and bottles_sold=0)  union all  select itemmst.companycode,yearcode,kflname,'4-Closing' as type,groupname,categoryname,ml,stock as 'qnty','' as batchno,'' as party,'' as billno,'' as tppassno,@day2 as trnDate  from  opening_closing_stock(@day1,@day2,@day2)  join itemmst on itemmst.itemcode=opening_closing_stock.itemcode and itemmst.companycode=opening_closing_stock.companycode  join groupmst on groupmst.groupcode=itemmst.groupcode and groupmst.companycode=itemmst.companycode  join categorymst on categorymst.categorycode=itemmst.categorycode and categorymst.companycode=itemmst.companycode  join kflmst on kflmst.companycode=itemmst.companycode and kflmst.kflcode=itemmst.kflcode  where not (opening_stock=0 and bottles_purchased=0 and stock=0 and bottles_sold=0)  union all  select itemmst.companycode,yearcode,kflname,'5-Difference' as type,groupname,categoryname,ml,bottles_sold as 'qnty','' as batchno,'' as party,'' as billno,'' as tppassno,@day2 as trnDate  from  opening_closing_stock(@day1,@day2,@day2)  join itemmst on itemmst.itemcode=opening_closing_stock.itemcode and itemmst.companycode=opening_closing_stock.companycode  join groupmst on groupmst.groupcode=itemmst.groupcode and groupmst.companycode=itemmst.companycode  join categorymst on categorymst.categorycode=itemmst.categorycode and categorymst.companycode=itemmst.companycode  join kflmst on kflmst.companycode=itemmst.companycode and kflmst.kflcode=itemmst.kflcode  where not (opening_stock=0 and bottles_purchased=0 and stock=0 and bottles_sold=0) union all  select itemmst.companycode,yearcode,kflname,'6-Difference in BL' as type,groupname,categoryname,ml,convert(float,convert(float,ml*bottles_sold)/1000) as 'qnty','' as batchno,'' as party,'' as billno,'' as tppassno ,@day2 as trnDate from  opening_closing_stock(@day1,@day2,@day2)  join itemmst on itemmst.itemcode=opening_closing_stock.itemcode and itemmst.companycode=opening_closing_stock.companycode  join groupmst on groupmst.groupcode=itemmst.groupcode and groupmst.companycode=itemmst.companycode  join categorymst on categorymst.categorycode=itemmst.categorycode and categorymst.companycode=itemmst.companycode  join kflmst on kflmst.companycode=itemmst.companycode and kflmst.kflcode=itemmst.kflcode  where not (opening_stock=0 and bottles_purchased=0 and stock=0 and bottles_sold=0) union all select itemmst.companycode,yearcode,kflname,'5-Difference' as type,groupname,categoryname,ml,breakage as 'qnty','' as batchno,'' as party,'' as billno,'' as tppassno,@day2 as trnDate  from  opening_closing_stock(@day1,@day2,@day2)  join itemmst on itemmst.itemcode=opening_closing_stock.itemcode and itemmst.companycode=opening_closing_stock.companycode  join groupmst on groupmst.groupcode=itemmst.groupcode and groupmst.companycode=itemmst.companycode  join categorymst on categorymst.categorycode=itemmst.categorycode and categorymst.companycode=itemmst.companycode  join kflmst on kflmst.companycode=itemmst.companycode and kflmst.kflcode=itemmst.kflcode  where(Not (opening_stock = 0 And bottles_purchased = 0 And stock = 0 And bottles_sold = 0)) union all select itemmst.companycode,yearcode,kflname,'6-Difference in BL' as type,groupname,categoryname,ml,convert(float,convert(float,ml*breakage)/1000) as 'qnty','' as batchno,'' as party,'' as billno,'' as tppassno,@day2 as trnDate  from  opening_closing_stock(@day1,@day2,@day2)  join itemmst on itemmst.itemcode=opening_closing_stock.itemcode and itemmst.companycode=opening_closing_stock.companycode  join groupmst on groupmst.groupcode=itemmst.groupcode and groupmst.companycode=itemmst.companycode  join categorymst on categorymst.categorycode=itemmst.categorycode and categorymst.companycode=itemmst.companycode  join kflmst on kflmst.companycode=itemmst.companycode and kflmst.kflcode=itemmst.kflcode  where not (opening_stock=0 and bottles_purchased=0 and stock=0 and bottles_sold=0) ) as x group by companycode,yearcode,kflname,type,groupname,categoryname,ml,batchno,party,billno,tppassno,trnDate  
GO

-- ----- dbo.uspGetBatchWiseStock -----
SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON
CREATE PROCEDURE [dbo].[uspGetBatchWiseStock] @day1 datetime, @day2 datetime, @day3 datetime, @companyCode varchar(6), @yearCode varchar(6), @storeCode varchar(6) ,@day4 as datetime AS select itemcode,batchno ,stock,storecode, trndate,mrp  from( select itemcode,itemname,sum(opening_stock) as opening_stock,sum(bottles_purchased) as bottles_purchased,sum(bottles_sold) as bottles_sold, sum(stock)+sum(opening_stock)+ sum(bottles_purchased)-sum(bottles_sold)as stock,companycode,storecode,yearcode,batchNo ,trndate ,max(distinct(mrp)) as mrp from( select itemmst.itemcode,itemname,sum(qnty) as opening_stock,0 as bottles_purchased,0 as bottles_sold,0 as stock,itemmst.companycode,shopcode as storecode,yearcode, (batchNo+' ` '+convert(varchar,openingstockDetail.trndate,103)) as batchNo,openingstockDetail.trndate ,max(distinct(itemmst.purchaserate)) as mrp from openingstockDetail join itemmst on openingstockDetail.itemcode=itemmst.itemcode and openingstockDetail.companycode=itemmst.companycode where openingstockDetail.trndate=@day1 group by itemmst.itemcode,itemmst.itemname,itemmst.companycode,shopcode,openingstockDetail.yearcode,openingstockDetail.batchNO,openingstockDetail.trndate union all select itemmst.itemcode,itemmst.itemname,(-1)*sum(qnty) as opening_stock,0 as bottles_purchased,0 as bottles_sold,0 as stock,salesbillmain.companycode,storecode,salesbillmain.yearcode,salesbilldetail.batchNo,salesbilldetail.batchdate as trndate ,max(distinct(salesbilldetail.mrp)) as mrp from salesbillmain join salesbilldetail on salesbillmain.companycode=salesbilldetail.companycode and salesbillmain.yearcode=salesbilldetail.yearcode and salesbillmain.trnno=salesbilldetail.trnno  join itemmst on itemmst.itemcode=salesbilldetail.itemcode and salesbilldetail.companycode=itemmst.companycode where salestype<>'RETURN' and salesbillmain.trndate >=@day1 and salesbillmain.trndate <@day2 group by itemmst.itemcode,itemmst.itemname,salesbillmain.companycode,storecode,salesbillmain.yearcode,salesbilldetail.batchNo,salesbilldetail.batchdate union all select itemmst.itemcode,itemmst.itemname,sum(qnty) as opening_stock,0 as bottles_purchased,0 as bottles_sold,0 as stock,salesbillmain.companycode,storecode,salesbillmain.yearcode,salesbilldetail.batchNo,salesbilldetail.batchdate as trndate ,max(distinct(salesbilldetail.mrp)) as mrp from salesbillmain join salesbilldetail on salesbillmain.companycode=salesbilldetail.companycode and salesbillmain.yearcode=salesbilldetail.yearcode and salesbillmain.trnno=salesbilldetail.trnno join itemmst on itemmst.itemcode=salesbilldetail.itemcode and salesbilldetail.companycode=itemmst.companycode where salestype='RETURN' and salesbillmain.trndate >=@day1 and salesbillmain.trndate <@day2 group by itemmst.itemcode,itemmst.itemname,salesbillmain.companycode,storecode,salesbillmain.yearcode,salesbilldetail.batchNo,salesbilldetail.batchdate union all select itemmst.itemcode,itemmst.itemname,sum(itemquantity) as opening_stock,0 as bottles_purchased,0 as bottles_sold,0 as stock,purchasemain.companycode, shopcode as storecode,purchasemain.yearcode,(purchasedetail.batchNo+' ` '+convert(varchar,purchasemain.trnDate,103)) as batchNo,purchasemain.trnDate,max(distinct(purchasedetail.itemmrp)) as mrp  from purchasemain join purchasedetail on purchasedetail.companycode=purchasemain.companycode and purchasedetail.trnno=purchasemain.trnno and purchasedetail.yearcode=purchasemain.yearcode join itemmst on itemmst.itemcode=purchasedetail.itemcode and itemmst.companycode=purchasedetail.companycode where ptype='PURCHASE' and purchasemain.trndate >=@day1 and purchasemain.trndate <@day2 group by itemmst.itemcode,itemmst.itemname,purchasemain.companycode,shopcode,purchasemain.yearcode,purchasedetail.batchNo,purchasemain.trnDate union all select itemmst.itemcode,itemmst.itemname,-1*sum(itemquantity) as opening_stock,0 as bottles_purchased,0 as bottles_sold,0 as stock,purchasemain.companycode,shopcode as storecode,purchasemain.yearcode,(purchasedetail.batchNo+' ` '+convert(varchar,purchasemain.trnDate,103)) as batchNo,purchasemain.trnDate,max(distinct(purchasedetail.itemmrp))as mrp  from purchasemain join purchasedetail on purchasedetail.companycode=purchasemain.companycode and purchasedetail.trnno=purchasemain.trnno and purchasedetail.yearcode=purchasemain.yearcode join itemmst on itemmst.itemcode=purchasedetail.itemcode and itemmst.companycode=purchasedetail.companycode where ptype='RETURN' and purchasemain.trndate >=@day1 and purchasemain.trndate <@day2 group by itemmst.itemcode,itemmst.itemname,purchasemain.companycode,shopcode,purchasemain.yearcode,purchasedetail.batchNo,purchasemain.trnDate union all select itemmst.itemcode,itemname,(-1)*sum(qnty) as opening_stock,0 as bottles_purchased,0 as bottles_sold,0 as stock,stk_transfer_detail.companycode,shopcode_frm as storecode,stk_transfer_detail.yearcode,(stk_transfer_detail.batchNo+' ` '+convert(varchar,stk_transfer_detail.batchdate,103)) as batchNo,stk_transfer_detail.batchdate as trndate,max(distinct(purchasedetail.itemmrp))as mrp from stk_transfer_detail join itemmst on itemmst.itemcode=stk_transfer_detail.itemcode and stk_transfer_detail.companycode=itemmst.companycode join stk_transfer_main on stk_transfer_main.companycode=stk_transfer_detail.companycode and stk_transfer_main.yearcode=stk_transfer_detail.yearcode and stk_transfer_main.trnno=stk_transfer_detail.trnno  JOIN purchasedetail ON stk_transfer_detail.CompanyCode = purchasedetail.companycode AND stk_transfer_detail.yearcode = purchasedetail.yearcode AND stk_transfer_detail.ItemCode = purchasedetail.itemcode AND stk_transfer_detail.batchNo = purchasedetail.batchno AND stk_transfer_detail.batchDate = purchasedetail.trndate where stk_transfer_main.trndate>=@day1 and stk_transfer_main.trndate<@day2 group by itemmst.itemcode,itemmst.itemname,stk_transfer_detail.companycode,shopcode_frm,stk_transfer_detail.yearcode,stk_transfer_detail.batchNo,stk_transfer_detail.batchdate union all  select itemmst.itemcode,itemname,sum(qnty) as opening_stock,0 as bottles_purchased,0 as bottles_sold,0 as stock,stk_transfer_detail.companycode,shopcode_to as storecode,stk_transfer_detail.yearcode,(stk_transfer_detail.batchNo+' ` '+convert(varchar,stk_transfer_detail.batchdate,103))as batchNo,stk_transfer_detail.batchdate as trndate, max(distinct(purchasedetail.itemmrp))as mrp from stk_transfer_detail join itemmst on itemmst.itemcode=stk_transfer_detail.itemcode and stk_transfer_detail.companycode=itemmst.companycode join stk_transfer_main on stk_transfer_main.companycode=stk_transfer_detail.companycode and stk_transfer_main.yearcode=stk_transfer_detail.yearcode and stk_transfer_main.trnno=stk_transfer_detail.trnno  JOIN purchasedetail ON stk_transfer_detail.CompanyCode = purchasedetail.companycode AND stk_transfer_detail.yearcode = purchasedetail.yearcode AND stk_transfer_detail.ItemCode = purchasedetail.itemcode AND stk_transfer_detail.batchNo = purchasedetail.batchno AND stk_transfer_detail.batchDate = purchasedetail.trndate where stk_transfer_main.trndate>=@day1 and stk_transfer_main.trndate<@day2 group by itemmst.itemcode,itemmst.itemname,stk_transfer_detail.companycode,shopcode_to,stk_transfer_detail.yearcode,stk_transfer_detail.batchNo ,stk_transfer_detail.batchdate union all select breakagedetail.itemcode,itemname,-1*sum(quantity) as opening_stock,0 as bottles_purchased,0 as bottles_sold,0 as stock,breakagedetail.companycode,shopcode,breakagedetail.yearcode,(breakagedetail.batchNo+' ` '+convert(varchar,breakagedetail.batchdate,103)) as batchNo,breakagedetail.batchdate as trndate ,max(distinct(purchasedetail.itemmrp))as mrp  from breakagedetail join breakagemain on breakagedetail.companycode=breakagemain.companycode and breakagedetail.yearcode=breakagemain.yearcode and breakagedetail.trnno=breakagemain.trnno join itemmst on itemmst.itemcode=breakagedetail.itemcode and itemmst.companycode=breakagedetail.companycode  JOIN purchasedetail ON breakagedetail.CompanyCode = purchasedetail.companycode AND breakagedetail.yearcode = purchasedetail.yearcode AND breakagedetail.ItemCode = purchasedetail.itemcode AND breakagedetail.batchDate = purchasedetail.trndate AND breakagedetail.batchNo = purchasedetail.batchno where receivd='False' and breakagemain.trndate>=@day1 and breakagemain.trndate<@day2 group by breakagedetail.itemcode,itemname,breakagedetail.companycode,shopcode,breakagedetail.yearcode,breakagedetail.batchNo,breakagedetail.batchdate union all select itemmst.itemcode,itemmst.itemname,0 as opening_stock,0 as bottles_purchased,sum(qnty) as bottles_sold,0 as stock,salesbillmain.companycode,storecode,salesbillmain.yearcode,salesbilldetail.batchNo,salesbilldetail.batchdate as trndate ,max(distinct(salesbilldetail.mrp)) as mrp from salesbillmain join salesbilldetail on salesbillmain.companycode=salesbilldetail.companycode and salesbillmain.yearcode=salesbilldetail.yearcode and salesbillmain.trnno=salesbilldetail.trnno   join itemmst on itemmst.itemcode=salesbilldetail.itemcode and salesbilldetail.companycode=itemmst.companycode where salestype='cash' and salesbillmain.trndate >=@day2 and salesbillmain.trndate <=@day3 or salestype='credit card' and salesbillmain.trndate >=@day2 and salesbillmain.trndate <=@day3 or  salestype ='credit account' and salesbillmain.trndate >=@day2 and salesbillmain.trndate <=@day3  group by itemmst.itemcode,itemmst.itemname,salesbillmain.companycode,storecode,salesbillmain.yearcode,salesbilldetail.batchNo,salesbilldetail.batchdate union all select itemmst.itemcode,itemmst.itemname,0 as opening_stock,0 as bottles_purchased,(-1)*sum(qnty) as bottles_sold,0 as stock,salesbillmain.companycode,storecode,salesbillmain.yearcode,salesbilldetail.batchNo,salesbilldetail.batchdate as trndate ,max(distinct(salesbilldetail.mrp)) as mrp from salesbillmain join salesbilldetail on salesbillmain.companycode=salesbilldetail.companycode and salesbillmain.yearcode=salesbilldetail.yearcode and salesbillmain.trnno=salesbilldetail.trnno join itemmst on itemmst.itemcode=salesbilldetail.itemcode and salesbilldetail.companycode=itemmst.companycode where salestype='return' and salesbillmain.trndate >=@day2 and salesbillmain.trndate <=@day3 group by itemmst.itemcode,itemmst.itemname,salesbillmain.companycode,storecode,salesbillmain.yearcode,salesbilldetail.batchNo,salesbilldetail.batchdate union all select itemmst.itemcode,itemmst.itemname,0 as opening_stock,sum(itemquantity) as bottles_purchased,0 as bottles_sold,0 as stock,purchasemain.companycode,shopcode as storecode,purchasemain.yearcode,(purchasedetail.batchNo+' ` '+convert(varchar,purchasemain.trndate,103)) as batchNo,purchasemain.trndate,max(distinct(purchasedetail.itemmrp)) as mrp  from purchasemain join purchasedetail on purchasedetail.companycode=purchasemain.companycode and purchasedetail.trnno=purchasemain.trnno and purchasedetail.yearcode=purchasemain.yearcode join itemmst on itemmst.itemcode=purchasedetail.itemcode and itemmst.companycode=purchasedetail.companycode where ptype='PURCHASE' and purchasemain.trndate >=@day2 and purchasemain.trndate <=@day3 group by itemmst.itemcode,itemmst.itemname,purchasemain.companycode,shopcode,purchasemain.yearcode,purchasedetail.batchNo ,purchasemain.trndate union all select itemmst.itemcode,itemmst.itemname,0 as opening_stock,(-1)*sum(itemquantity) as bottles_purchased,0 as bottles_sold,0 as stock,purchasemain.companycode,shopcode as storecode,purchasemain.yearcode,(purchasedetail.batchNo+' ` '+convert(varchar,purchasemain.trndate,103))as batchNo ,purchasemain.trndate ,max(distinct(purchasedetail.itemmrp)) as mrp  from purchasemain join purchasedetail on purchasedetail.companycode=purchasemain.companycode and purchasedetail.trnno=purchasemain.trnno and purchasedetail.yearcode=purchasemain.yearcode join itemmst on itemmst.itemcode=purchasedetail.itemcode and itemmst.companycode=purchasedetail.companycode where ptype='RETURN' and purchasemain.trndate >=@day2 and purchasemain.trndate <=@day3 group by itemmst.itemcode,itemmst.itemname,purchasemain.companycode,shopcode,purchasemain.yearcode,purchasedetail.batchNo,purchasemain.trndate union all select itemmst.itemcode,itemname,0 as opening_stock,0 as bottles_purchased,0 as bottles_sold,(-1)*sum(qnty) as stock,stk_transfer_detail.companycode,shopcode_frm as storecode,stk_transfer_detail.yearcode,(stk_transfer_detail.batchNo+' ` '+convert(varchar,stk_transfer_detail.batchdate,103)) as batchNo ,stk_transfer_detail.batchdate as trndate,max(distinct(purchasedetail.itemmrp))as mrp from stk_transfer_detail  join itemmst on itemmst.itemcode=stk_transfer_detail.itemcode and stk_transfer_detail.companycode=itemmst.companycode join stk_transfer_main on stk_transfer_main.companycode=stk_transfer_detail.companycode and stk_transfer_main.yearcode=stk_transfer_detail.yearcode and stk_transfer_main.trnno=stk_transfer_detail.trnno JOIN purchasedetail ON stk_transfer_detail.CompanyCode = purchasedetail.companycode AND stk_transfer_detail.yearcode = purchasedetail.yearcode AND stk_transfer_detail.ItemCode = purchasedetail.itemcode AND stk_transfer_detail.batchNo = purchasedetail.batchno AND stk_transfer_detail.batchDate = purchasedetail.trndate where stk_transfer_main.trndate>=@day2 and stk_transfer_main.trndate<=@day3 group by itemmst.itemcode,itemmst.itemname,stk_transfer_detail.companycode,shopcode_frm,stk_transfer_detail.yearcode,stk_transfer_detail.batchNo,stk_transfer_detail.batchdate  union all select itemmst.itemcode,itemname,0 as opening_stock,0 as bottles_purchased,0 as bottles_sold,sum(qnty) as stock,stk_transfer_detail.companycode,shopcode_to as storecode,stk_transfer_detail.yearcode,(stk_transfer_detail.batchNo+' ` '+convert(varchar,stk_transfer_detail.batchdate,103)) as batchNo,stk_transfer_detail.batchdate as trndate ,max(distinct(purchasedetail.itemmrp))as mrp  from stk_transfer_detail join itemmst on itemmst.itemcode=stk_transfer_detail.itemcode and stk_transfer_detail.companycode=itemmst.companycode join stk_transfer_main on stk_transfer_main.companycode=stk_transfer_detail.companycode and stk_transfer_main.yearcode=stk_transfer_detail.yearcode and stk_transfer_main.trnno=stk_transfer_detail.trnno JOIN purchasedetail ON stk_transfer_detail.CompanyCode = purchasedetail.companycode AND stk_transfer_detail.yearcode = purchasedetail.yearcode AND stk_transfer_detail.ItemCode = purchasedetail.itemcode AND stk_transfer_detail.batchNo = purchasedetail.batchno AND stk_transfer_detail.batchDate = purchasedetail.trndate where stk_transfer_main.trndate>=@day2 and stk_transfer_main.trndate<=@day3 group by itemmst.itemcode,itemmst.itemname,stk_transfer_detail.companycode,shopcode_to,stk_transfer_detail.yearcode,stk_transfer_detail.batchNo ,stk_transfer_detail.batchdate union all select breakagedetail.itemcode,itemname,0 as opening_stock,0 as bottles_purchased,0 as bottles_sold,-1*sum(quantity) as stock,breakagedetail.companycode,shopcode,breakagedetail.yearcode,(breakagedetail.batchNo+' ` '+convert(varchar,breakagedetail.batchdate,103)) as batchNo ,breakagedetail.batchdate as trndate ,max(distinct(purchasedetail.itemmrp))as mrp  from breakagedetail join breakagemain on breakagedetail.companycode=breakagemain.companycode and breakagedetail.yearcode=breakagemain.yearcode and breakagedetail.trnno=breakagemain.trnno  join itemmst on itemmst.itemcode=breakagedetail.itemcode and itemmst.companycode=breakagedetail.companycode JOIN purchasedetail ON breakagedetail.CompanyCode = purchasedetail.companycode AND breakagedetail.yearcode = purchasedetail.yearcode AND breakagedetail.ItemCode = purchasedetail.itemcode AND breakagedetail.batchDate = purchasedetail.trndate AND breakagedetail.batchNo = purchasedetail.batchno where receivd='False' and breakagemain.trndate>=@day2 and breakagemain.trndate<=@day3  group by breakagedetail.itemcode,itemname,breakagedetail.companycode,shopcode,breakagedetail.yearcode,breakagedetail.batchNo,breakagedetail.batchdate ) as x group by x.itemcode,x.itemname,x.companycode,x.storecode,x.yearcode,x.batchNo,x.trndate ) as y where y.companyCode = @companyCode and y.yearCode = @yearCode and y.storeCode = @storeCode and y.trndate <= @day4 and y.stock > 0 order by itemcode Asc,trndate asc, batchNo asc 
GO

-- ============================================================
-- 7. FOREIGN KEYS
-- ============================================================
ALTER TABLE [dbo].[accesspanel]  WITH CHECK ADD  CONSTRAINT [FK_accesspanel_users] FOREIGN KEY([usercode])
REFERENCES [dbo].[users] ([usercode])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[accesspanel] CHECK CONSTRAINT [FK_accesspanel_users]
GO
ALTER TABLE [dbo].[acountname]  WITH CHECK ADD  CONSTRAINT [FK_acountname_acgroupmst] FOREIGN KEY([acgroupcode])
REFERENCES [dbo].[acgroupmst] ([acgroupcode])
ALTER TABLE [dbo].[acountname] CHECK CONSTRAINT [FK_acountname_acgroupmst]
GO
ALTER TABLE [dbo].[acountname]  WITH CHECK ADD  CONSTRAINT [FK_acountname_companymast] FOREIGN KEY([companycode])
REFERENCES [dbo].[companymast] ([companycode])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[acountname] CHECK CONSTRAINT [FK_acountname_companymast]
GO
ALTER TABLE [dbo].[breakagedetail]  WITH CHECK ADD  CONSTRAINT [FK_breakagedetail_breakagemain] FOREIGN KEY([CompanyCode], [yearcode], [trndate], [trnno])
REFERENCES [dbo].[breakagemain] ([CompanyCode], [YearCode], [trndate], [TrnNo])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[breakagedetail] CHECK CONSTRAINT [FK_breakagedetail_breakagemain]
GO
ALTER TABLE [dbo].[breakagedetail]  WITH CHECK ADD  CONSTRAINT [FK_breakagedetail_itemmst] FOREIGN KEY([CompanyCode], [ItemCode])
REFERENCES [dbo].[itemmst] ([companycode], [itemcode])
ALTER TABLE [dbo].[breakagedetail] CHECK CONSTRAINT [FK_breakagedetail_itemmst]
GO
ALTER TABLE [dbo].[breakagemain]  WITH CHECK ADD  CONSTRAINT [FK_breakagemain_ledger] FOREIGN KEY([CompanyCode], [ledgercode])
REFERENCES [dbo].[ledger] ([companycode], [ledcode])
ALTER TABLE [dbo].[breakagemain] CHECK CONSTRAINT [FK_breakagemain_ledger]
GO
ALTER TABLE [dbo].[breakagemain]  WITH CHECK ADD  CONSTRAINT [FK_breakagemain_storage] FOREIGN KEY([CompanyCode], [shopcode])
REFERENCES [dbo].[storage] ([companycode], [shopcode])
ALTER TABLE [dbo].[breakagemain] CHECK CONSTRAINT [FK_breakagemain_storage]
GO
ALTER TABLE [dbo].[categorymst]  WITH CHECK ADD  CONSTRAINT [FK_categorymst_companymast] FOREIGN KEY([companycode])
REFERENCES [dbo].[companymast] ([companycode])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[categorymst] CHECK CONSTRAINT [FK_categorymst_companymast]
GO
ALTER TABLE [dbo].[companyparamtr]  WITH CHECK ADD  CONSTRAINT [FK_companyparamtr_companymast] FOREIGN KEY([companycode])
REFERENCES [dbo].[companymast] ([companycode])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[companyparamtr] CHECK CONSTRAINT [FK_companyparamtr_companymast]
GO
ALTER TABLE [dbo].[groupmst]  WITH CHECK ADD  CONSTRAINT [FK_groupmst_companymast] FOREIGN KEY([companycode])
REFERENCES [dbo].[companymast] ([companycode])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[groupmst] CHECK CONSTRAINT [FK_groupmst_companymast]
GO
ALTER TABLE [dbo].[Itemgroupml]  WITH CHECK ADD  CONSTRAINT [FK_Itemgroupml_companymast] FOREIGN KEY([companycode])
REFERENCES [dbo].[companymast] ([companycode])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[Itemgroupml] CHECK CONSTRAINT [FK_Itemgroupml_companymast]
GO
ALTER TABLE [dbo].[itemmst]  WITH CHECK ADD  CONSTRAINT [FK_itemmst_categorymst] FOREIGN KEY([companycode], [categorycode])
REFERENCES [dbo].[categorymst] ([companycode], [categorycode])
ALTER TABLE [dbo].[itemmst] CHECK CONSTRAINT [FK_itemmst_categorymst]
GO
ALTER TABLE [dbo].[itemmst]  WITH CHECK ADD  CONSTRAINT [FK_itemmst_companymast] FOREIGN KEY([companycode])
REFERENCES [dbo].[companymast] ([companycode])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[itemmst] CHECK CONSTRAINT [FK_itemmst_companymast]
GO
ALTER TABLE [dbo].[itemmst]  WITH CHECK ADD  CONSTRAINT [FK_itemmst_groupmst] FOREIGN KEY([companycode], [groupcode])
REFERENCES [dbo].[groupmst] ([companycode], [groupcode])
ALTER TABLE [dbo].[itemmst] CHECK CONSTRAINT [FK_itemmst_groupmst]
GO
ALTER TABLE [dbo].[itemmst]  WITH CHECK ADD  CONSTRAINT [FK_itemmst_Itemgroupml] FOREIGN KEY([companycode], [packing], [ml])
REFERENCES [dbo].[Itemgroupml] ([companycode], [packing], [ml])
ALTER TABLE [dbo].[itemmst] CHECK CONSTRAINT [FK_itemmst_Itemgroupml]
GO
ALTER TABLE [dbo].[itemmst]  WITH CHECK ADD  CONSTRAINT [FK_itemmst_kflmst] FOREIGN KEY([companycode], [kflcode])
REFERENCES [dbo].[kflmst] ([companycode], [kflcode])
ALTER TABLE [dbo].[itemmst] CHECK CONSTRAINT [FK_itemmst_kflmst]
GO
ALTER TABLE [dbo].[itemmst]  WITH CHECK ADD  CONSTRAINT [FK_itemmst_strength] FOREIGN KEY([companycode], [strengthname])
REFERENCES [dbo].[strength] ([companycode], [strengthname])
ALTER TABLE [dbo].[itemmst] CHECK CONSTRAINT [FK_itemmst_strength]
GO
ALTER TABLE [dbo].[itemrateinfo]  WITH CHECK ADD  CONSTRAINT [FK_itemrateinfo_companymast] FOREIGN KEY([companycode])
REFERENCES [dbo].[companymast] ([companycode])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[itemrateinfo] CHECK CONSTRAINT [FK_itemrateinfo_companymast]
GO
ALTER TABLE [dbo].[itemratemst]  WITH CHECK ADD  CONSTRAINT [FK_itemratemst_itemrateinfo] FOREIGN KEY([companycode], [ratecode])
REFERENCES [dbo].[itemrateinfo] ([companycode], [ratecode])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[itemratemst] CHECK CONSTRAINT [FK_itemratemst_itemrateinfo]
GO
ALTER TABLE [dbo].[journaldetail]  WITH CHECK ADD  CONSTRAINT [FK_journaldetail_ledger] FOREIGN KEY([companycode], [ledcode])
REFERENCES [dbo].[ledger] ([companycode], [ledcode])
ALTER TABLE [dbo].[journaldetail] CHECK CONSTRAINT [FK_journaldetail_ledger]
GO
ALTER TABLE [dbo].[kflmst]  WITH CHECK ADD  CONSTRAINT [FK_kflmst_companymast] FOREIGN KEY([companycode])
REFERENCES [dbo].[companymast] ([companycode])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[kflmst] CHECK CONSTRAINT [FK_kflmst_companymast]
GO
ALTER TABLE [dbo].[ledger]  WITH CHECK ADD  CONSTRAINT [FK_ledger_acountname] FOREIGN KEY([companycode], [accode])
REFERENCES [dbo].[acountname] ([companycode], [accode])
ALTER TABLE [dbo].[ledger] CHECK CONSTRAINT [FK_ledger_acountname]
GO
ALTER TABLE [dbo].[ledger]  WITH CHECK ADD  CONSTRAINT [FK_ledger_companymast] FOREIGN KEY([companycode])
REFERENCES [dbo].[companymast] ([companycode])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[ledger] CHECK CONSTRAINT [FK_ledger_companymast]
GO
ALTER TABLE [dbo].[ledger_opening]  WITH CHECK ADD  CONSTRAINT [FK_ledger_opening_ledger] FOREIGN KEY([companycode], [ledcode])
REFERENCES [dbo].[ledger] ([companycode], [ledcode])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[ledger_opening] CHECK CONSTRAINT [FK_ledger_opening_ledger]
GO
ALTER TABLE [dbo].[openingStockDetail]  WITH CHECK ADD  CONSTRAINT [FK_openingStockDetail_openingstockmst] FOREIGN KEY([companyCode], [yearCode], [shopCode], [itemCode])
REFERENCES [dbo].[openingstockmst] ([companycode], [yearcode], [shopcode], [itemcode])
ALTER TABLE [dbo].[openingStockDetail] CHECK CONSTRAINT [FK_openingStockDetail_openingstockmst]
GO
ALTER TABLE [dbo].[openingstockmst]  WITH CHECK ADD  CONSTRAINT [FK_openingstockmst_itemmst] FOREIGN KEY([companycode], [itemcode])
REFERENCES [dbo].[itemmst] ([companycode], [itemcode])
ALTER TABLE [dbo].[openingstockmst] CHECK CONSTRAINT [FK_openingstockmst_itemmst]
GO
ALTER TABLE [dbo].[payment_detail]  WITH CHECK ADD  CONSTRAINT [FK_payment_detail_payment_main] FOREIGN KEY([companycode], [yearcode], [vchdate], [vchno])
REFERENCES [dbo].[payment_main] ([companycode], [yearcode], [vchdate], [vchno])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[payment_detail] CHECK CONSTRAINT [FK_payment_detail_payment_main]
GO
ALTER TABLE [dbo].[payment_detail]  WITH NOCHECK ADD  CONSTRAINT [FK_payment_detail_purchasemain] FOREIGN KEY([companycode], [yearcode], [trnno])
REFERENCES [dbo].[purchasemain] ([companycode], [yearcode], [docno])
ALTER TABLE [dbo].[payment_detail] NOCHECK CONSTRAINT [FK_payment_detail_purchasemain]
GO
ALTER TABLE [dbo].[payment_main]  WITH NOCHECK ADD  CONSTRAINT [FK_payment_main_trnidmst] FOREIGN KEY([companycode], [yearcode], [trnid])
REFERENCES [dbo].[trnidmst] ([companycode], [yearcode], [trnid])
ALTER TABLE [dbo].[payment_main] CHECK CONSTRAINT [FK_payment_main_trnidmst]
GO
ALTER TABLE [dbo].[purchasedetail]  WITH CHECK ADD  CONSTRAINT [FK_purchasedetail_itemmst] FOREIGN KEY([companycode], [itemcode])
REFERENCES [dbo].[itemmst] ([companycode], [itemcode])
ALTER TABLE [dbo].[purchasedetail] CHECK CONSTRAINT [FK_purchasedetail_itemmst]
GO
ALTER TABLE [dbo].[purchasedetail]  WITH CHECK ADD  CONSTRAINT [FK_purchasedetail_purchasemain] FOREIGN KEY([companycode], [yearcode], [trnno])
REFERENCES [dbo].[purchasemain] ([companycode], [yearcode], [trnno])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[purchasedetail] CHECK CONSTRAINT [FK_purchasedetail_purchasemain]
GO
ALTER TABLE [dbo].[purchasemain]  WITH CHECK ADD  CONSTRAINT [FK_purchasemain_ledger_purchaseacccode] FOREIGN KEY([companycode], [purchaseacccode])
REFERENCES [dbo].[ledger] ([companycode], [ledcode])
ALTER TABLE [dbo].[purchasemain] CHECK CONSTRAINT [FK_purchasemain_ledger_purchaseacccode]
GO
ALTER TABLE [dbo].[purchasemain]  WITH CHECK ADD  CONSTRAINT [FK_purchasemain_ledger_suppliercode] FOREIGN KEY([companycode], [suppliercode])
REFERENCES [dbo].[ledger] ([companycode], [ledcode])
ALTER TABLE [dbo].[purchasemain] CHECK CONSTRAINT [FK_purchasemain_ledger_suppliercode]
GO
ALTER TABLE [dbo].[purchasemain]  WITH CHECK ADD  CONSTRAINT [FK_purchasemain_storage] FOREIGN KEY([companycode], [shopcode])
REFERENCES [dbo].[storage] ([companycode], [shopcode])
ALTER TABLE [dbo].[purchasemain] CHECK CONSTRAINT [FK_purchasemain_storage]
GO
ALTER TABLE [dbo].[purchasemain]  WITH NOCHECK ADD  CONSTRAINT [FK_purchasemain_trnidmst] FOREIGN KEY([companycode], [yearcode], [trnid])
REFERENCES [dbo].[trnidmst] ([companycode], [yearcode], [trnid])
ALTER TABLE [dbo].[purchasemain] CHECK CONSTRAINT [FK_purchasemain_trnidmst]
GO
ALTER TABLE [dbo].[PurchaseTaxDetail]  WITH CHECK ADD  CONSTRAINT [FK_PurchaseTaxDetail_purchasemain] FOREIGN KEY([companycode], [yearcode], [trnno])
REFERENCES [dbo].[purchasemain] ([companycode], [yearcode], [trnno])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[PurchaseTaxDetail] CHECK CONSTRAINT [FK_PurchaseTaxDetail_purchasemain]
GO
ALTER TABLE [dbo].[PurchaseTaxDetail]  WITH CHECK ADD  CONSTRAINT [FK_PurchaseTaxDetail_taxdetail] FOREIGN KEY([companycode], [schemecode], [TaxCode])
REFERENCES [dbo].[taxdetail] ([companycode], [SchemeCode], [TaxCode])
ON UPDATE CASCADE
ALTER TABLE [dbo].[PurchaseTaxDetail] CHECK CONSTRAINT [FK_PurchaseTaxDetail_taxdetail]
GO
ALTER TABLE [dbo].[receipt_detail]  WITH CHECK ADD  CONSTRAINT [FK_receipt_detail_receipt_main] FOREIGN KEY([companycode], [yearcode], [vchdate], [vchno])
REFERENCES [dbo].[receipt_main] ([companycode], [yearcode], [vchdate], [vchno])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[receipt_detail] CHECK CONSTRAINT [FK_receipt_detail_receipt_main]
GO
ALTER TABLE [dbo].[receipt_detail]  WITH NOCHECK ADD  CONSTRAINT [FK_receipt_detail_salesbillmain] FOREIGN KEY([companycode], [yearcode], [trnno])
REFERENCES [dbo].[salesbillmain] ([companycode], [yearcode], [trnno])
ON UPDATE CASCADE
ALTER TABLE [dbo].[receipt_detail] NOCHECK CONSTRAINT [FK_receipt_detail_salesbillmain]
GO
ALTER TABLE [dbo].[receipt_main]  WITH CHECK ADD  CONSTRAINT [FK_receipt_main_ledger_clientac] FOREIGN KEY([companycode], [client_account])
REFERENCES [dbo].[ledger] ([companycode], [ledcode])
ALTER TABLE [dbo].[receipt_main] CHECK CONSTRAINT [FK_receipt_main_ledger_clientac]
GO
ALTER TABLE [dbo].[receipt_main]  WITH CHECK ADD  CONSTRAINT [FK_receipt_main_ledger_discac] FOREIGN KEY([companycode], [Discount_account_head])
REFERENCES [dbo].[ledger] ([companycode], [ledcode])
ALTER TABLE [dbo].[receipt_main] CHECK CONSTRAINT [FK_receipt_main_ledger_discac]
GO
ALTER TABLE [dbo].[receipt_main]  WITH CHECK ADD  CONSTRAINT [FK_receipt_main_ledger_headac] FOREIGN KEY([companycode], [head_account])
REFERENCES [dbo].[ledger] ([companycode], [ledcode])
ALTER TABLE [dbo].[receipt_main] CHECK CONSTRAINT [FK_receipt_main_ledger_headac]
GO
ALTER TABLE [dbo].[receipt_main]  WITH NOCHECK ADD  CONSTRAINT [FK_receipt_main_trnidmst] FOREIGN KEY([companycode], [yearcode], [trnid])
REFERENCES [dbo].[trnidmst] ([companycode], [yearcode], [trnid])
ALTER TABLE [dbo].[receipt_main] CHECK CONSTRAINT [FK_receipt_main_trnidmst]
GO
ALTER TABLE [dbo].[sales_tax]  WITH CHECK ADD  CONSTRAINT [FK_sales_tax_salesbillmain] FOREIGN KEY([companycode], [yearcode], [trnno])
REFERENCES [dbo].[salesbillmain] ([companycode], [yearcode], [trnno])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[sales_tax] CHECK CONSTRAINT [FK_sales_tax_salesbillmain]
GO
ALTER TABLE [dbo].[salesbilldetail]  WITH CHECK ADD  CONSTRAINT [FK_salesbilldetail_itemmst] FOREIGN KEY([companycode], [itemcode])
REFERENCES [dbo].[itemmst] ([companycode], [itemcode])
ALTER TABLE [dbo].[salesbilldetail] CHECK CONSTRAINT [FK_salesbilldetail_itemmst]
GO
ALTER TABLE [dbo].[salesbilldetail]  WITH NOCHECK ADD  CONSTRAINT [FK_salesbilldetail_salesbillmain] FOREIGN KEY([companycode], [yearcode], [trnno])
REFERENCES [dbo].[salesbillmain] ([companycode], [yearcode], [trnno])
ON UPDATE CASCADE
NOT FOR REPLICATION 
ALTER TABLE [dbo].[salesbilldetail] NOCHECK CONSTRAINT [FK_salesbilldetail_salesbillmain]
GO
ALTER TABLE [dbo].[salesbillmain]  WITH CHECK ADD  CONSTRAINT [FK_salesbillmain_companyparamtr_discntacchead] FOREIGN KEY([companycode], [discount_account_head])
REFERENCES [dbo].[companyparamtr] ([companycode], [discntacc])
ON UPDATE CASCADE
ALTER TABLE [dbo].[salesbillmain] CHECK CONSTRAINT [FK_salesbillmain_companyparamtr_discntacchead]
GO
ALTER TABLE [dbo].[salesbillmain]  WITH CHECK ADD  CONSTRAINT [FK_salesbillmain_itemrateinfo] FOREIGN KEY([companycode], [ratecode])
REFERENCES [dbo].[itemrateinfo] ([companycode], [ratecode])
ALTER TABLE [dbo].[salesbillmain] CHECK CONSTRAINT [FK_salesbillmain_itemrateinfo]
GO
ALTER TABLE [dbo].[salesbillmain]  WITH CHECK ADD  CONSTRAINT [FK_salesbillmain_ledger_acount_head] FOREIGN KEY([companycode], [account_head])
REFERENCES [dbo].[ledger] ([companycode], [ledcode])
ALTER TABLE [dbo].[salesbillmain] CHECK CONSTRAINT [FK_salesbillmain_ledger_acount_head]
GO
ALTER TABLE [dbo].[salesbillmain]  WITH CHECK ADD  CONSTRAINT [FK_salesbillmain_ledger_discount_account] FOREIGN KEY([companycode], [discount_account_head])
REFERENCES [dbo].[ledger] ([companycode], [ledcode])
ALTER TABLE [dbo].[salesbillmain] CHECK CONSTRAINT [FK_salesbillmain_ledger_discount_account]
GO
ALTER TABLE [dbo].[salesbillmain]  WITH CHECK ADD  CONSTRAINT [FK_salesbillmain_ledger_ledgercode] FOREIGN KEY([companycode], [ledgercode])
REFERENCES [dbo].[ledger] ([companycode], [ledcode])
ALTER TABLE [dbo].[salesbillmain] CHECK CONSTRAINT [FK_salesbillmain_ledger_ledgercode]
GO
ALTER TABLE [dbo].[salesbillmain]  WITH CHECK ADD  CONSTRAINT [FK_salesbillmain_storage] FOREIGN KEY([companycode], [storecode])
REFERENCES [dbo].[storage] ([companycode], [shopcode])
ALTER TABLE [dbo].[salesbillmain] CHECK CONSTRAINT [FK_salesbillmain_storage]
GO
ALTER TABLE [dbo].[salesbillmain]  WITH NOCHECK ADD  CONSTRAINT [FK_salesbillmain_trnidmst] FOREIGN KEY([companycode], [yearcode], [trnid])
REFERENCES [dbo].[trnidmst] ([companycode], [yearcode], [trnid])
ALTER TABLE [dbo].[salesbillmain] CHECK CONSTRAINT [FK_salesbillmain_trnidmst]
GO
ALTER TABLE [dbo].[stk_transfer_detail]  WITH CHECK ADD  CONSTRAINT [FK_stk_transfer_detail_itemmst] FOREIGN KEY([CompanyCode], [ItemCode])
REFERENCES [dbo].[itemmst] ([companycode], [itemcode])
ALTER TABLE [dbo].[stk_transfer_detail] CHECK CONSTRAINT [FK_stk_transfer_detail_itemmst]
GO
ALTER TABLE [dbo].[stk_transfer_detail]  WITH CHECK ADD  CONSTRAINT [FK_stk_transfer_detail_stk_transfer_main] FOREIGN KEY([CompanyCode], [yearcode], [trndate], [trnno])
REFERENCES [dbo].[stk_transfer_main] ([CompanyCode], [YearCode], [TrnDate], [TrnNo])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[stk_transfer_detail] CHECK CONSTRAINT [FK_stk_transfer_detail_stk_transfer_main]
GO
ALTER TABLE [dbo].[stk_transfer_main]  WITH NOCHECK ADD  CONSTRAINT [FK_stk_transfer_main_trnidmst] FOREIGN KEY([CompanyCode], [YearCode], [trnid])
REFERENCES [dbo].[trnidmst] ([companycode], [yearcode], [trnid])
ALTER TABLE [dbo].[stk_transfer_main] CHECK CONSTRAINT [FK_stk_transfer_main_trnidmst]
GO
ALTER TABLE [dbo].[storage]  WITH CHECK ADD  CONSTRAINT [FK_storage_companymast] FOREIGN KEY([companycode])
REFERENCES [dbo].[companymast] ([companycode])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[storage] CHECK CONSTRAINT [FK_storage_companymast]
GO
ALTER TABLE [dbo].[strength]  WITH CHECK ADD  CONSTRAINT [FK_strength_companymast] FOREIGN KEY([companycode])
REFERENCES [dbo].[companymast] ([companycode])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[strength] CHECK CONSTRAINT [FK_strength_companymast]
GO
ALTER TABLE [dbo].[taxdetail]  WITH CHECK ADD  CONSTRAINT [FK_taxdetail_companymast] FOREIGN KEY([companycode])
REFERENCES [dbo].[companymast] ([companycode])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[taxdetail] CHECK CONSTRAINT [FK_taxdetail_companymast]
GO
ALTER TABLE [dbo].[taxdetail]  WITH CHECK ADD  CONSTRAINT [FK_taxdetail_ledger] FOREIGN KEY([companycode], [ledcode])
REFERENCES [dbo].[ledger] ([companycode], [ledcode])
ALTER TABLE [dbo].[taxdetail] CHECK CONSTRAINT [FK_taxdetail_ledger]
GO
ALTER TABLE [dbo].[trnidmst]  WITH CHECK ADD  CONSTRAINT [FK_trnidmst_users] FOREIGN KEY([usercode])
REFERENCES [dbo].[users] ([usercode])
ALTER TABLE [dbo].[trnidmst] CHECK CONSTRAINT [FK_trnidmst_users]
GO
ALTER TABLE [dbo].[userrights]  WITH CHECK ADD  CONSTRAINT [FK_userrights_companymast] FOREIGN KEY([companycode])
REFERENCES [dbo].[companymast] ([companycode])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[userrights] CHECK CONSTRAINT [FK_userrights_companymast]
GO
ALTER TABLE [dbo].[userrights]  WITH CHECK ADD  CONSTRAINT [FK_userrights_users] FOREIGN KEY([usercode])
REFERENCES [dbo].[users] ([usercode])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[userrights] CHECK CONSTRAINT [FK_userrights_users]
GO
ALTER TABLE [dbo].[yearmst]  WITH CHECK ADD  CONSTRAINT [FK_yearmst_companymast] FOREIGN KEY([companycode])
REFERENCES [dbo].[companymast] ([companycode])
ON UPDATE CASCADE
ON DELETE CASCADE
ALTER TABLE [dbo].[yearmst] CHECK CONSTRAINT [FK_yearmst_companymast]
GO

-- End of DDL
