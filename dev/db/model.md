# Madhushala Pro - Database Model

- **Database**: `barmanager`
- **Source**: original `Debug/Resources/barmanager.mdf` (attached to SQL Server 2022)
- **Generated**: 2026-11-08 12:09:02 UTC

## Summary

| Object type | Count |
| --- | --- |
| Tables | 45 |
| Views | 24 |
| Stored procedures | 3 |
| Functions | 11 |

## Tables and row counts

| Table | Rows | Columns | Primary key |
| --- | --- | --- | --- |
| `dbo.accesspanel` | 0 | 3 |  |
| `dbo.acgroupmst` | 0 | 4 | acgroupcode |
| `dbo.acountname` | 0 | 6 | accode, companycode |
| `dbo.breakagedetail` | 0 | 11 | CompanyCode, trnno, ItemCode, yearcode, batchDate, batchNo |
| `dbo.breakagemain` | 0 | 10 | CompanyCode, YearCode, TrnNo, trndate |
| `dbo.categorymst` | 0 | 3 | companycode, categorycode |
| `dbo.companymast` | 1 | 17 | companycode |
| `dbo.companyparamtr` | 0 | 60 | companycode |
| `dbo.ExizeCompany` | 0 | 2 | companyCode, mergeCompanyCode |
| `dbo.fine_details` | 0 | 9 | companyCode, yearCode, code, trndate, trnno |
| `dbo.groupmst` | 0 | 4 | companycode, groupcode |
| `dbo.Itemgroupml` | 0 | 3 | ml, packing, companycode |
| `dbo.itemmst` | 0 | 16 | itemcode, companycode |
| `dbo.itemrateinfo` | 1 | 3 | ratecode, companycode |
| `dbo.itemratemst` | 0 | 4 | companycode, ratecode, itemcode |
| `dbo.journaldetail` | 0 | 16 | companycode, yearcode, trnno, trndate, slno |
| `dbo.kflmst` | 0 | 3 | companycode, kflcode |
| `dbo.ledger` | 0 | 20 | ledcode, companycode |
| `dbo.ledger_opening` | 0 | 7 |  |
| `dbo.openingStockDetail` | 0 | 9 | companyCode, yearCode, shopCode, batchNo, itemCode |
| `dbo.openingstockmst` | 0 | 8 | shopcode, companycode, itemcode, yearcode |
| `dbo.payment_detail` | 0 | 9 | companycode, yearcode, vchno, trnno, vchdate |
| `dbo.payment_main` | 0 | 15 | companycode, yearcode, vchno, vchdate |
| `dbo.purchasedetail` | 0 | 17 | companycode, yearcode, itemcode, batchno, itemrate, trnno |
| `dbo.purchasemain` | 0 | 19 | companycode, yearcode, trnno |
| `dbo.PurchaseTaxDetail` | 0 | 9 |  |
| `dbo.receipt_detail` | 0 | 8 | companycode, yearcode, vchno, trnno, vchdate |
| `dbo.receipt_main` | 0 | 16 | companycode, yearcode, vchno, vchdate |
| `dbo.reportTaggingDetail` | 0 | 4 |  |
| `dbo.reportTaggingMain` | 0 | 3 |  |
| `dbo.sales_tax` | 0 | 7 | trnno, companycode, yearcode, tax_book |
| `dbo.salesbilldetail` | 0 | 16 | companycode, itemcode, trnno, yearcode, batchDate, batchNo |
| `dbo.salesbillmain` | 0 | 17 | companycode, trnno, yearcode |
| `dbo.stk_transfer_detail` | 0 | 10 | CompanyCode, ItemCode, trnno, yearcode, batchDate, batchNo |
| `dbo.stk_transfer_main` | 0 | 9 | CompanyCode, YearCode, TrnNo, TrnDate |
| `dbo.storage` | 0 | 5 | companycode, shopcode |
| `dbo.strength` | 0 | 2 | strengthname, companycode |
| `dbo.taxdetail` | 0 | 14 | companycode, SchemeCode, TaxCode |
| `dbo.TEMP` | 0 | 13 | companyCode, yearCode, kflName, type, groupName, categoryName, ml, qnty, batchno, party, billno, tppassno, trndate |
| `dbo.tempitem` | 0 | 1 |  |
| `dbo.templedger` | 0 | 1 |  |
| `dbo.trnidmst` | 0 | 5 | companycode, yearcode, trnid |
| `dbo.userrights` | 0 | 8 | usercode, companycode, modulename |
| `dbo.users` | 1 | 4 | usercode |
| `dbo.yearmst` | 1 | 5 | yearcode, companycode |

## Entity relationship diagram

```mermaid
erDiagram
    accesspanel {
        varchar usercode "FK"
        varchar menuname ""
        varchar menutext ""
    }
    acgroupmst {
        varchar acgroupcode "PK"
        varchar acgroupname ""
        varchar actype ""
        int sequence ""
    }
    acountname {
        varchar accode "PK"
        varchar acgroupcode "FK"
        varchar acname ""
        bit actype ""
        varchar subgroupcode ""
        varchar companycode "PK"
    }
    breakagedetail {
        varchar CompanyCode "PK"
        int trnno "PK"
        varchar ItemCode "PK"
        varchar Receivd ""
        varchar yearcode "PK"
        float loose ""
        float box ""
        float quantity ""
        datetime trndate "FK"
        datetime batchDate "PK"
        varchar batchNo "PK"
    }
    breakagemain {
        varchar CompanyCode "PK"
        varchar YearCode "PK"
        int TrnNo "PK"
        varchar Narration ""
        varchar shopcode "FK"
        datetime trndate "PK"
        varchar ledgercode "FK"
        varchar billno ""
        varchar TP_pass_no ""
        int trnid ""
    }
    categorymst {
        varchar companycode "PK"
        varchar categorycode "PK"
        varchar categoryname ""
    }
    companymast {
        varchar companycode "PK"
        varchar companyname ""
        varchar address1 ""
        varchar address2 ""
        varchar city ""
        varchar district ""
        varchar stat ""
        varchar pin ""
        varchar phn ""
        varchar email ""
        varchar website ""
        varchar faxno ""
        varchar lstno ""
        varchar cstno ""
        varchar panno ""
        varchar vatno ""
        varchar stno ""
    }
    companyparamtr {
        varchar companycode "PK"
        int csalebillno ""
        varchar SaleAccount_Head ""
        varchar bankacc ""
        varchar cashparty ""
        varchar crcashparty ""
        varchar breakacc ""
        varchar saleretrnacc ""
        varchar csalelmt ""
        varchar printbill ""
        varchar printer ""
        varchar purchaseacchd ""
        varchar cashhd ""
        varchar discntacc ""
        varchar defshpgdn ""
        varchar ratecode ""
        varchar expenceacc ""
        varchar rndoffacc ""
        varchar autorefcurrsal ""
        varchar actposmode ""
        varchar bill_footer ""
        varchar bill_footer2 ""
        varchar bill_footer3 ""
        varchar bill_footer4 ""
        varchar on_off_shop_type ""
        varchar back_up_restore_path ""
        varchar print_mode ""
        varchar surcharge_account_head ""
        varchar Trade_discount_head ""
        float surcharge_percent ""
        varchar phone_number1 ""
        varchar email_id ""
        varchar sender_id ""
        varchar password ""
        varchar zone ""
        int screen_time ""
        bit status_show ""
        varchar language ""
        nvarchar message ""
        varchar sales_taxon_mrp ""
        float sales_taxon_mrp_rate ""
        bit mrp_change ""
        varchar sms_password ""
        bit saletax_including_free ""
        bit backup_upload ""
        bit sms_on ""
        varchar purchase_discount_type ""
        bit batchSale ""
        bit negativeSale ""
        bit batchEdit ""
        varchar excisePassword ""
        varchar exciseId ""
        bit sms_billEditDelete ""
        bit sms_PartySale ""
        bit sms_creditCardSale ""
        bit sms_payment ""
        bit sms_receipt ""
        nvarchar companyId ""
        int csLimit ""
        bit userWiseSalesStatement ""
    }
    ExizeCompany {
        varchar companyCode "PK"
        varchar mergeCompanyCode "PK"
    }
    fine_details {
        varchar companyCode "PK"
        varchar yearCode "PK"
        varchar code "PK"
        datetime trndate "PK"
        float amount ""
        varchar naration ""
        varchar type ""
        int trnno "PK"
        varchar trnid ""
    }
    groupmst {
        varchar companycode "PK"
        varchar groupcode "PK"
        varchar groupname ""
        varchar subGroupCode ""
    }
    Itemgroupml {
        int ml "PK"
        float packing "PK"
        varchar companycode "PK"
    }
    itemmst {
        varchar groupcode "FK"
        varchar itemcode "PK"
        varchar itemname ""
        varchar categorycode "FK"
        varchar kflcode "FK"
        float packing "FK"
        varchar strengthname "FK"
        int ml "FK"
        varchar companycode "PK"
        varchar barcode ""
        float salesrate ""
        float purchaseRate ""
        float purchase_discount_amount ""
        float purchase_salestaxonmrp_amount ""
        float purchase_taxscheme_amount ""
        int min_qnty ""
    }
    itemrateinfo {
        varchar ratecode "PK"
        varchar ratename ""
        varchar companycode "PK"
    }
    itemratemst {
        varchar companycode "PK"
        varchar ratecode "PK"
        varchar itemcode "PK"
        float salesrate ""
    }
    journaldetail {
        varchar companycode "PK"
        varchar yearcode "PK"
        varchar trnno "PK"
        datetime trndate "PK"
        int slno "PK"
        varchar to_by ""
        varchar ledcode "FK"
        varchar chqno ""
        datetime chqdate ""
        varchar accpay ""
        float debit ""
        float credit ""
        varchar type ""
        varchar narr ""
        int trnid ""
        float Currbal ""
    }
    kflmst {
        varchar companycode "PK"
        varchar kflcode "PK"
        varchar kflname ""
    }
    ledger {
        varchar name ""
        varchar accode "FK"
        varchar contperson ""
        varchar city ""
        varchar district ""
        varchar state ""
        varchar phone ""
        varchar email ""
        varchar www ""
        varchar address1 ""
        varchar area ""
        varchar address2 ""
        float debit ""
        varchar vatno ""
        varchar panno ""
        varchar ledcode "PK"
        varchar companycode "PK"
        float credit ""
        varchar type ""
        varchar ratecode ""
    }
    ledger_opening {
        varchar companycode "FK"
        varchar yearcode ""
        varchar ledcode "FK"
        float debit ""
        float credit ""
        varchar type ""
        datetime stdate ""
    }
    openingStockDetail {
        varchar companyCode "PK"
        varchar yearCode "PK"
        datetime trndate ""
        varchar shopCode "PK"
        varchar batchNo "PK"
        varchar itemCode "PK"
        int loose ""
        int box ""
        int qnty ""
    }
    openingstockmst {
        varchar shopcode "PK"
        varchar companycode "PK"
        varchar itemcode "PK"
        int loose ""
        int box ""
        int qnty ""
        varchar yearcode "PK"
        datetime trndate ""
    }
    payment_detail {
        varchar companycode "PK"
        varchar yearcode "PK"
        int vchno "PK"
        varchar trnno "PK"
        float due_amount ""
        float debit ""
        float credit ""
        datetime vchdate "PK"
        varchar supplierCode ""
    }
    payment_main {
        varchar companycode "PK"
        varchar yearcode "PK"
        int vchno "PK"
        varchar head_account ""
        varchar client_account ""
        varchar narration ""
        datetime vchdate "PK"
        varchar cheque_no ""
        float due ""
        float amount_paid ""
        float discount ""
        float net_due ""
        varchar Discount_account_head ""
        int trnid "FK"
        datetime chqDate ""
    }
    purchasedetail {
        varchar companycode "PK"
        varchar yearcode "PK"
        varchar trnid ""
        numeric slno ""
        varchar itemcode "PK"
        varchar batchno "PK"
        numeric itemrate "PK"
        int itembox ""
        int itemloose ""
        int itemquantity ""
        numeric itemamount ""
        int trnno "PK"
        int itemfreeqnty ""
        float itemboxrate ""
        float itemmrp ""
        float itemdiscount ""
        datetime trndate ""
    }
    purchasemain {
        varchar companycode "PK"
        varchar yearcode "PK"
        int trnid "FK"
        int trnno "PK"
        datetime trndate ""
        varchar ptype ""
        varchar purchaseacccode "FK"
        varchar suppliercode "FK"
        varchar shopcode "FK"
        varchar docno ""
        datetime docdate ""
        varchar tppassno ""
        varchar schemecode ""
        numeric totamount ""
        numeric tottaxOth ""
        numeric totnetamt ""
        varchar narration ""
        varchar checkedBy ""
        bit saletax_including_free ""
    }
    PurchaseTaxDetail {
        varchar companycode "FK"
        int trnno "FK"
        varchar schemecode "FK"
        varchar TaxCode "FK"
        numeric TaxRate ""
        numeric OnAmount ""
        numeric TaxAmount ""
        varchar TaxAccount ""
        varchar yearcode "FK"
    }
    receipt_detail {
        varchar companycode "PK"
        varchar yearcode "PK"
        int vchno "PK"
        varchar trnno "PK"
        float due_amount ""
        float debit ""
        float credit ""
        datetime vchdate "PK"
    }
    receipt_main {
        varchar companycode "PK"
        varchar yearcode "PK"
        int vchno "PK"
        varchar head_account "FK"
        varchar client_account "FK"
        varchar narration ""
        datetime vchdate "PK"
        varchar cheque_no ""
        float due ""
        float amount_paid ""
        float discount ""
        float net_due ""
        varchar Discount_account_head "FK"
        varchar receipt_type ""
        int trnid "FK"
        datetime chqDate ""
    }
    reportTaggingDetail {
        varchar reportName ""
        varchar reportCategory ""
        varchar groupname ""
        varchar companycode ""
    }
    reportTaggingMain {
        varchar reportName ""
        varchar reportCategory ""
        varchar companycode ""
    }
    sales_tax {
        varchar trnno "PK"
        varchar companycode "PK"
        varchar yearcode "PK"
        varchar cash_bank_book ""
        varchar tax_book "PK"
        float tax_amount ""
        datetime date ""
    }
    salesbilldetail {
        varchar companycode "PK"
        varchar itemcode "PK"
        int tot_box ""
        int loose ""
        int qnty ""
        float itemAmount ""
        varchar trnno "PK"
        float rate ""
        datetime trndate ""
        varchar yearcode "PK"
        int large ""
        int small ""
        int westage ""
        float MRP ""
        datetime batchDate "PK"
        varchar batchNo "PK"
    }
    salesbillmain {
        varchar companycode "PK"
        varchar trnno "PK"
        datetime trndate ""
        varchar ledgercode "FK"
        float amount ""
        varchar salestype ""
        varchar ratecode "FK"
        varchar storecode "FK"
        varchar yearcode "PK"
        float discamount ""
        float netamount ""
        varchar account_head "FK"
        varchar discount_account_head "FK"
        datetime trntime ""
        int trnid "FK"
        varchar discount_reason ""
        varchar KFL ""
    }
    stk_transfer_detail {
        varchar CompanyCode "PK"
        varchar ItemCode "PK"
        int box ""
        int loose ""
        int qnty ""
        int trnno "PK"
        datetime trndate "FK"
        varchar yearcode "PK"
        datetime batchDate "PK"
        varchar batchNo "PK"
    }
    stk_transfer_main {
        varchar CompanyCode "PK"
        varchar YearCode "PK"
        int TrnNo "PK"
        datetime TrnDate "PK"
        varchar shopname_from ""
        varchar shopname_to ""
        varchar shopcode_frm ""
        varchar shopcode_to ""
        int trnid "FK"
    }
    storage {
        varchar companycode "PK"
        varchar shopcode "PK"
        varchar shopname ""
        varchar address ""
        varchar ShopType ""
    }
    strength {
        varchar strengthname "PK"
        varchar companycode "PK"
    }
    taxdetail {
        varchar companycode "PK"
        varchar SchemeCode "PK"
        varchar schemename ""
        int SrNo ""
        varchar TaxCode "PK"
        varchar taxname ""
        varchar ledcode "FK"
        varchar sig ""
        float TaxRate ""
        float taxamount ""
        varchar calc_on ""
        int base ""
        bit after_discount ""
        bit after_saletax ""
    }
    TEMP {
        varchar companyCode "PK"
        varchar yearCode "PK"
        varchar kflName "PK"
        varchar type "PK"
        varchar groupName "PK"
        varchar categoryName "PK"
        int ml "PK"
        float qnty "PK"
        varchar batchno "PK"
        varchar party "PK"
        varchar billno "PK"
        varchar tppassno "PK"
        datetime trndate "PK"
    }
    tempitem {
        nvarchar item ""
    }
    templedger {
        nvarchar ledname ""
    }
    trnidmst {
        varchar companycode "PK"
        varchar yearcode "PK"
        int trnid "PK"
        varchar trn_type ""
        varchar usercode "FK"
    }
    userrights {
        varchar usercode "PK"
        varchar companycode "PK"
        varchar accaccess ""
        varchar modulename "PK"
        varchar perv ""
        varchar pera ""
        varchar pere ""
        varchar perd ""
    }
    users {
        varchar usercode "PK"
        varchar username ""
        varchar accesslevel ""
        varchar password ""
    }
    yearmst {
        varchar yearcode "PK"
        varchar yearrange ""
        datetime stdate ""
        datetime enddate ""
        varchar companycode "PK"
    }
    users ||--o{ accesspanel : "FK_accesspanel_users"
    acgroupmst ||--o{ acountname : "FK_acountname_acgroupmst"
    companymast ||--o{ acountname : "FK_acountname_companymast"
    breakagemain ||--o{ breakagedetail : "FK_breakagedetail_breakagemain"
    itemmst ||--o{ breakagedetail : "FK_breakagedetail_itemmst"
    ledger ||--o{ breakagemain : "FK_breakagemain_ledger"
    storage ||--o{ breakagemain : "FK_breakagemain_storage"
    companymast ||--o{ categorymst : "FK_categorymst_companymast"
    companymast ||--o{ companyparamtr : "FK_companyparamtr_companymast"
    companymast ||--o{ groupmst : "FK_groupmst_companymast"
    companymast ||--o{ Itemgroupml : "FK_Itemgroupml_companymast"
    categorymst ||--o{ itemmst : "FK_itemmst_categorymst"
    companymast ||--o{ itemmst : "FK_itemmst_companymast"
    groupmst ||--o{ itemmst : "FK_itemmst_groupmst"
    Itemgroupml ||--o{ itemmst : "FK_itemmst_Itemgroupml"
    kflmst ||--o{ itemmst : "FK_itemmst_kflmst"
    strength ||--o{ itemmst : "FK_itemmst_strength"
    companymast ||--o{ itemrateinfo : "FK_itemrateinfo_companymast"
    itemrateinfo ||--o{ itemratemst : "FK_itemratemst_itemrateinfo"
    ledger ||--o{ journaldetail : "FK_journaldetail_ledger"
    companymast ||--o{ kflmst : "FK_kflmst_companymast"
    acountname ||--o{ ledger : "FK_ledger_acountname"
    companymast ||--o{ ledger : "FK_ledger_companymast"
    ledger ||--o{ ledger_opening : "FK_ledger_opening_ledger"
    openingstockmst ||--o{ openingStockDetail : "FK_openingStockDetail_openingstockmst"
    itemmst ||--o{ openingstockmst : "FK_openingstockmst_itemmst"
    payment_main ||--o{ payment_detail : "FK_payment_detail_payment_main"
    purchasemain ||--o{ payment_detail : "FK_payment_detail_purchasemain"
    trnidmst ||--o{ payment_main : "FK_payment_main_trnidmst"
    itemmst ||--o{ purchasedetail : "FK_purchasedetail_itemmst"
    purchasemain ||--o{ purchasedetail : "FK_purchasedetail_purchasemain"
    ledger ||--o{ purchasemain : "FK_purchasemain_ledger_purchaseacccode"
    ledger ||--o{ purchasemain : "FK_purchasemain_ledger_suppliercode"
    storage ||--o{ purchasemain : "FK_purchasemain_storage"
    trnidmst ||--o{ purchasemain : "FK_purchasemain_trnidmst"
    purchasemain ||--o{ PurchaseTaxDetail : "FK_PurchaseTaxDetail_purchasemain"
    taxdetail ||--o{ PurchaseTaxDetail : "FK_PurchaseTaxDetail_taxdetail"
    receipt_main ||--o{ receipt_detail : "FK_receipt_detail_receipt_main"
    salesbillmain ||--o{ receipt_detail : "FK_receipt_detail_salesbillmain"
    ledger ||--o{ receipt_main : "FK_receipt_main_ledger_clientac"
    ledger ||--o{ receipt_main : "FK_receipt_main_ledger_discac"
    ledger ||--o{ receipt_main : "FK_receipt_main_ledger_headac"
    trnidmst ||--o{ receipt_main : "FK_receipt_main_trnidmst"
    salesbillmain ||--o{ sales_tax : "FK_sales_tax_salesbillmain"
    itemmst ||--o{ salesbilldetail : "FK_salesbilldetail_itemmst"
    salesbillmain ||--o{ salesbilldetail : "FK_salesbilldetail_salesbillmain"
    companyparamtr ||--o{ salesbillmain : "FK_salesbillmain_companyparamtr_discntacchead"
    itemrateinfo ||--o{ salesbillmain : "FK_salesbillmain_itemrateinfo"
    ledger ||--o{ salesbillmain : "FK_salesbillmain_ledger_acount_head"
    ledger ||--o{ salesbillmain : "FK_salesbillmain_ledger_discount_account"
    ledger ||--o{ salesbillmain : "FK_salesbillmain_ledger_ledgercode"
    storage ||--o{ salesbillmain : "FK_salesbillmain_storage"
    trnidmst ||--o{ salesbillmain : "FK_salesbillmain_trnidmst"
    itemmst ||--o{ stk_transfer_detail : "FK_stk_transfer_detail_itemmst"
    stk_transfer_main ||--o{ stk_transfer_detail : "FK_stk_transfer_detail_stk_transfer_main"
    trnidmst ||--o{ stk_transfer_main : "FK_stk_transfer_main_trnidmst"
    companymast ||--o{ storage : "FK_storage_companymast"
    companymast ||--o{ strength : "FK_strength_companymast"
    companymast ||--o{ taxdetail : "FK_taxdetail_companymast"
    ledger ||--o{ taxdetail : "FK_taxdetail_ledger"
    users ||--o{ trnidmst : "FK_trnidmst_users"
    companymast ||--o{ userrights : "FK_userrights_companymast"
    users ||--o{ userrights : "FK_userrights_users"
    companymast ||--o{ yearmst : "FK_yearmst_companymast"
```


## Table details

### `dbo.accesspanel` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| usercode | VarChar(6) | yes | FK |  |
| menuname | VarChar(30) | yes |  |  |
| menutext | VarChar(50) | yes |  |  |

Foreign keys:

- `FK_accesspanel_users`: (usercode) -> `dbo.users` (usercode)

### `dbo.acgroupmst` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| acgroupcode | VarChar(6) | no | PK |  |
| acgroupname | VarChar(50) | no |  |  |
| actype | VarChar(50) | no |  |  |
| sequence | Int | no |  | ((0)) |

### `dbo.acountname` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| accode | VarChar(6) | no | PK |  |
| acgroupcode | VarChar(6) | yes | FK |  |
| acname | VarChar(50) | yes |  |  |
| actype | Bit | yes |  |  |
| subgroupcode | VarChar(6) | yes |  |  |
| companycode | VarChar(6) | no | PK | ((0)) |

Foreign keys:

- `FK_acountname_acgroupmst`: (acgroupcode) -> `dbo.acgroupmst` (acgroupcode)
- `FK_acountname_companymast`: (companycode) -> `dbo.companymast` (companycode)

### `dbo.breakagedetail` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| CompanyCode | VarChar(6) | no | PK |  |
| trnno | Int | no | PK |  |
| ItemCode | VarChar(6) | no | PK |  |
| Receivd | VarChar(100) | yes |  |  |
| yearcode | VarChar(6) | no | PK |  |
| loose | Float | yes |  |  |
| box | Float | yes |  |  |
| quantity | Float | yes |  |  |
| trndate | DateTime | yes | FK |  |
| batchDate | DateTime | no | PK | (((1)/(1))/(2013)) |
| batchNo | VarChar(50) | no | PK | ((0)) |

Foreign keys:

- `FK_breakagedetail_breakagemain`: (CompanyCode, yearcode, trndate, trnno) -> `dbo.breakagemain` (CompanyCode, YearCode, trndate, TrnNo)
- `FK_breakagedetail_itemmst`: (CompanyCode, ItemCode) -> `dbo.itemmst` (companycode, itemcode)

### `dbo.breakagemain` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| CompanyCode | VarChar(6) | no | PK |  |
| YearCode | VarChar(6) | no | PK |  |
| TrnNo | Int | no | PK |  |
| Narration | VarChar(150) | yes |  |  |
| shopcode | VarChar(6) | yes | FK |  |
| trndate | DateTime | no | PK |  |
| ledgercode | VarChar(6) | yes | FK |  |
| billno | VarCharMax | yes |  |  |
| TP_pass_no | VarCharMax | yes |  |  |
| trnid | Int | no |  | ((0)) |

Foreign keys:

- `FK_breakagemain_ledger`: (CompanyCode, ledgercode) -> `dbo.ledger` (companycode, ledcode)
- `FK_breakagemain_storage`: (CompanyCode, shopcode) -> `dbo.storage` (companycode, shopcode)

### `dbo.categorymst` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| companycode | VarChar(6) | no | PK |  |
| categorycode | VarChar(6) | no | PK |  |
| categoryname | VarChar(50) | yes |  |  |

Foreign keys:

- `FK_categorymst_companymast`: (companycode) -> `dbo.companymast` (companycode)

### `dbo.companymast` (1 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| companycode | VarChar(6) | no | PK |  |
| companyname | VarChar(50) | no |  |  |
| address1 | VarChar(200) | yes |  |  |
| address2 | VarChar(200) | yes |  |  |
| city | VarChar(200) | yes |  |  |
| district | VarChar(200) | yes |  |  |
| stat | VarChar(200) | yes |  |  |
| pin | VarChar(200) | yes |  |  |
| phn | VarChar(200) | yes |  |  |
| email | VarChar(200) | yes |  |  |
| website | VarChar(200) | yes |  |  |
| faxno | VarChar(200) | yes |  |  |
| lstno | VarChar(200) | yes |  |  |
| cstno | VarChar(200) | yes |  |  |
| panno | VarChar(15) | yes |  |  |
| vatno | VarChar(15) | yes |  |  |
| stno | VarChar(15) | yes |  |  |

### `dbo.companyparamtr` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| companycode | VarChar(6) | no | PK |  |
| csalebillno | Int | yes |  |  |
| SaleAccount_Head | VarChar(6) | no |  |  |
| bankacc | VarChar(6) | yes |  |  |
| cashparty | VarChar(50) | yes |  |  |
| crcashparty | VarChar(50) | yes |  |  |
| breakacc | VarChar(6) | yes |  |  |
| saleretrnacc | VarChar(6) | yes |  |  |
| csalelmt | VarChar(50) | yes |  |  |
| printbill | VarChar(1) | yes |  |  |
| printer | VarChar(50) | yes |  |  |
| purchaseacchd | VarChar(6) | no |  |  |
| cashhd | VarChar(6) | yes |  |  |
| discntacc | VarChar(6) | no |  |  |
| defshpgdn | VarChar(50) | yes |  |  |
| ratecode | VarChar(6) | yes |  |  |
| expenceacc | VarChar(6) | yes |  |  |
| rndoffacc | VarChar(6) | yes |  |  |
| autorefcurrsal | VarChar(1) | yes |  |  |
| actposmode | VarChar(1) | yes |  |  |
| bill_footer | VarChar(20) | yes |  |  |
| bill_footer2 | VarChar(20) | yes |  |  |
| bill_footer3 | VarChar(20) | yes |  |  |
| bill_footer4 | VarChar(20) | yes |  |  |
| on_off_shop_type | VarChar(3) | yes |  |  |
| back_up_restore_path | VarCharMax | yes |  |  |
| print_mode | VarChar(7) | yes |  |  |
| surcharge_account_head | VarChar(6) | yes |  |  |
| Trade_discount_head | VarChar(6) | yes |  |  |
| surcharge_percent | Float | yes |  |  |
| phone_number1 | VarChar(10) | yes |  |  |
| email_id | VarChar(50) | yes |  |  |
| sender_id | VarChar(50) | yes |  |  |
| password | VarChar(20) | yes |  |  |
| zone | VarChar(50) | yes |  |  |
| screen_time | Int | yes |  |  |
| status_show | Bit | yes |  |  |
| language | VarChar(20) | yes |  |  |
| message | NVarChar(140) | yes |  |  |
| sales_taxon_mrp | VarChar(6) | yes |  |  |
| sales_taxon_mrp_rate | Float | yes |  |  |
| mrp_change | Bit | yes |  |  |
| sms_password | VarChar(15) | yes |  |  |
| saletax_including_free | Bit | yes |  | ((0)) |
| backup_upload | Bit | no |  | ((0)) |
| sms_on | Bit | no |  | ((0)) |
| purchase_discount_type | VarChar(6) | yes |  |  |
| batchSale | Bit | yes |  | ((0)) |
| negativeSale | Bit | yes |  | ((0)) |
| batchEdit | Bit | yes |  | ((0)) |
| excisePassword | VarChar(40) | yes |  |  |
| exciseId | VarChar(70) | yes |  |  |
| sms_billEditDelete | Bit | no |  | ((0)) |
| sms_PartySale | Bit | no |  | ((0)) |
| sms_creditCardSale | Bit | no |  | ((0)) |
| sms_payment | Bit | no |  | ((0)) |
| sms_receipt | Bit | no |  | ((0)) |
| companyId | NVarChar(20) | yes |  |  |
| csLimit | Int | no |  | ((1000)) |
| userWiseSalesStatement | Bit | no |  | ((0)) |

Foreign keys:

- `FK_companyparamtr_companymast`: (companycode) -> `dbo.companymast` (companycode)

### `dbo.ExizeCompany` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| companyCode | VarChar(6) | no | PK |  |
| mergeCompanyCode | VarChar(6) | no | PK |  |

### `dbo.fine_details` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| companyCode | VarChar(6) | no | PK |  |
| yearCode | VarChar(6) | no | PK |  |
| code | VarChar(3) | no | PK |  |
| trndate | DateTime | no | PK |  |
| amount | Float | no |  |  |
| naration | VarChar(50) | yes |  |  |
| type | VarChar(60) | yes |  |  |
| trnno | Int | no | PK |  |
| trnid | VarChar(6) | no |  |  |

### `dbo.groupmst` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| companycode | VarChar(6) | no | PK |  |
| groupcode | VarChar(6) | no | PK |  |
| groupname | VarChar(50) | yes |  |  |
| subGroupCode | VarChar(6) | no |  | ('main') |

Foreign keys:

- `FK_groupmst_companymast`: (companycode) -> `dbo.companymast` (companycode)

### `dbo.Itemgroupml` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| ml | Int | no | PK |  |
| packing | Float | no | PK |  |
| companycode | VarChar(6) | no | PK | ((0)) |

Foreign keys:

- `FK_Itemgroupml_companymast`: (companycode) -> `dbo.companymast` (companycode)

### `dbo.itemmst` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| groupcode | VarChar(6) | yes | FK |  |
| itemcode | VarChar(6) | no | PK |  |
| itemname | VarChar(50) | yes |  |  |
| categorycode | VarChar(6) | yes | FK |  |
| kflcode | VarChar(6) | yes | FK |  |
| packing | Float | no | FK |  |
| strengthname | VarChar(6) | yes | FK |  |
| ml | Int | no | FK |  |
| companycode | VarChar(6) | no | PK |  |
| barcode | VarChar(50) | yes |  |  |
| salesrate | Float | yes |  |  |
| purchaseRate | Float | yes |  |  |
| purchase_discount_amount | Float | no |  | ((0)) |
| purchase_salestaxonmrp_amount | Float | no |  | ((0)) |
| purchase_taxscheme_amount | Float | no |  | ((0)) |
| min_qnty | Int | yes |  |  |

Foreign keys:

- `FK_itemmst_categorymst`: (companycode, categorycode) -> `dbo.categorymst` (companycode, categorycode)
- `FK_itemmst_companymast`: (companycode) -> `dbo.companymast` (companycode)
- `FK_itemmst_groupmst`: (companycode, groupcode) -> `dbo.groupmst` (companycode, groupcode)
- `FK_itemmst_Itemgroupml`: (companycode, packing, ml) -> `dbo.Itemgroupml` (companycode, packing, ml)
- `FK_itemmst_kflmst`: (companycode, kflcode) -> `dbo.kflmst` (companycode, kflcode)
- `FK_itemmst_strength`: (companycode, strengthname) -> `dbo.strength` (companycode, strengthname)

### `dbo.itemrateinfo` (1 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| ratecode | VarChar(6) | no | PK |  |
| ratename | VarChar(50) | yes |  |  |
| companycode | VarChar(6) | no | PK |  |

Foreign keys:

- `FK_itemrateinfo_companymast`: (companycode) -> `dbo.companymast` (companycode)

### `dbo.itemratemst` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| companycode | VarChar(6) | no | PK |  |
| ratecode | VarChar(6) | no | PK |  |
| itemcode | VarChar(6) | no | PK |  |
| salesrate | Float | yes |  |  |

Foreign keys:

- `FK_itemratemst_itemrateinfo`: (companycode, ratecode) -> `dbo.itemrateinfo` (companycode, ratecode)

### `dbo.journaldetail` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| companycode | VarChar(6) | no | PK |  |
| yearcode | VarChar(6) | no | PK |  |
| trnno | VarChar(20) | no | PK |  |
| trndate | DateTime | no | PK |  |
| slno | Int | no | PK |  |
| to_by | VarChar(2) | no |  |  |
| ledcode | VarChar(6) | no | FK |  |
| chqno | VarChar(6) | yes |  |  |
| chqdate | DateTime | yes |  |  |
| accpay | VarChar(3) | yes |  |  |
| debit | Float | yes |  |  |
| credit | Float | yes |  |  |
| type | VarChar(8) | no |  |  |
| narr | VarChar(50) | yes |  |  |
| trnid | Int | no |  |  |
| Currbal | Float | yes |  |  |

Foreign keys:

- `FK_journaldetail_ledger`: (companycode, ledcode) -> `dbo.ledger` (companycode, ledcode)

### `dbo.kflmst` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| companycode | VarChar(6) | no | PK |  |
| kflcode | VarChar(6) | no | PK |  |
| kflname | VarChar(50) | yes |  |  |

Foreign keys:

- `FK_kflmst_companymast`: (companycode) -> `dbo.companymast` (companycode)

### `dbo.ledger` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| name | VarChar(50) | yes |  |  |
| accode | VarChar(6) | no | FK |  |
| contperson | VarChar(50) | yes |  |  |
| city | VarChar(50) | yes |  |  |
| district | VarChar(50) | yes |  |  |
| state | VarChar(50) | yes |  |  |
| phone | VarChar(50) | yes |  |  |
| email | VarChar(50) | yes |  |  |
| www | VarChar(50) | yes |  |  |
| address1 | VarChar(50) | yes |  |  |
| area | VarChar(50) | yes |  |  |
| address2 | VarChar(50) | yes |  |  |
| debit | Float | yes |  |  |
| vatno | VarChar(50) | yes |  |  |
| panno | VarChar(50) | yes |  |  |
| ledcode | VarChar(6) | no | PK |  |
| companycode | VarChar(6) | no | PK |  |
| credit | Float | yes |  |  |
| type | VarChar(50) | yes |  |  |
| ratecode | VarChar(6) | yes |  |  |

Foreign keys:

- `FK_ledger_acountname`: (companycode, accode) -> `dbo.acountname` (companycode, accode)
- `FK_ledger_companymast`: (companycode) -> `dbo.companymast` (companycode)

### `dbo.ledger_opening` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| companycode | VarChar(6) | no | FK |  |
| yearcode | VarChar(6) | yes |  |  |
| ledcode | VarChar(6) | no | FK |  |
| debit | Float | yes |  |  |
| credit | Float | yes |  |  |
| type | VarChar(50) | yes |  |  |
| stdate | DateTime | yes |  |  |

Foreign keys:

- `FK_ledger_opening_ledger`: (companycode, ledcode) -> `dbo.ledger` (companycode, ledcode)

### `dbo.openingStockDetail` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| companyCode | VarChar(6) | no | PK |  |
| yearCode | VarChar(6) | no | PK |  |
| trndate | DateTime | yes |  |  |
| shopCode | VarChar(6) | no | PK |  |
| batchNo | VarChar(50) | no | PK |  |
| itemCode | VarChar(6) | no | PK |  |
| loose | Int | yes |  |  |
| box | Int | yes |  |  |
| qnty | Int | yes |  |  |

Foreign keys:

- `FK_openingStockDetail_openingstockmst`: (companyCode, yearCode, shopCode, itemCode) -> `dbo.openingstockmst` (companycode, yearcode, shopcode, itemcode)

### `dbo.openingstockmst` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| shopcode | VarChar(6) | no | PK |  |
| companycode | VarChar(6) | no | PK |  |
| itemcode | VarChar(6) | no | PK |  |
| loose | Int | yes |  |  |
| box | Int | yes |  |  |
| qnty | Int | yes |  |  |
| yearcode | VarChar(6) | no | PK |  |
| trndate | DateTime | yes |  |  |

Foreign keys:

- `FK_openingstockmst_itemmst`: (companycode, itemcode) -> `dbo.itemmst` (companycode, itemcode)

### `dbo.payment_detail` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| companycode | VarChar(6) | no | PK |  |
| yearcode | VarChar(6) | no | PK |  |
| vchno | Int | no | PK |  |
| trnno | VarChar(20) | no | PK |  |
| due_amount | Float | yes |  |  |
| debit | Float | yes |  |  |
| credit | Float | yes |  |  |
| vchdate | DateTime | no | PK |  |
| supplierCode | VarChar(6) | yes |  |  |

Foreign keys:

- `FK_payment_detail_payment_main`: (companycode, yearcode, vchdate, vchno) -> `dbo.payment_main` (companycode, yearcode, vchdate, vchno)
- `FK_payment_detail_purchasemain`: (companycode, yearcode, trnno) -> `dbo.purchasemain` (companycode, yearcode, docno)

### `dbo.payment_main` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| companycode | VarChar(6) | no | PK |  |
| yearcode | VarChar(6) | no | PK |  |
| vchno | Int | no | PK |  |
| head_account | VarChar(6) | yes |  |  |
| client_account | VarChar(6) | yes |  |  |
| narration | VarChar(50) | yes |  |  |
| vchdate | DateTime | no | PK |  |
| cheque_no | VarChar(20) | yes |  |  |
| due | Float | yes |  |  |
| amount_paid | Float | yes |  |  |
| discount | Float | yes |  |  |
| net_due | Float | yes |  |  |
| Discount_account_head | VarChar(6) | yes |  |  |
| trnid | Int | no | FK | ((0)) |
| chqDate | DateTime | yes |  |  |

Foreign keys:

- `FK_payment_main_trnidmst`: (companycode, yearcode, trnid) -> `dbo.trnidmst` (companycode, yearcode, trnid)

### `dbo.purchasedetail` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| companycode | VarChar(6) | no | PK |  |
| yearcode | VarChar(6) | no | PK |  |
| trnid | VarChar(6) | yes |  |  |
| slno | Numeric(3,0) | yes |  |  |
| itemcode | VarChar(6) | no | PK |  |
| batchno | VarChar(50) | no | PK |  |
| itemrate | Numeric(10,2) | no | PK |  |
| itembox | Int | yes |  |  |
| itemloose | Int | yes |  |  |
| itemquantity | Int | yes |  |  |
| itemamount | Numeric(10,2) | yes |  |  |
| trnno | Int | no | PK |  |
| itemfreeqnty | Int | yes |  |  |
| itemboxrate | Float | yes |  |  |
| itemmrp | Float | yes |  |  |
| itemdiscount | Float | yes |  |  |
| trndate | DateTime | no |  | ('4/1/2013') |

Foreign keys:

- `FK_purchasedetail_itemmst`: (companycode, itemcode) -> `dbo.itemmst` (companycode, itemcode)
- `FK_purchasedetail_purchasemain`: (companycode, yearcode, trnno) -> `dbo.purchasemain` (companycode, yearcode, trnno)

### `dbo.purchasemain` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| companycode | VarChar(6) | no | PK |  |
| yearcode | VarChar(6) | no | PK |  |
| trnid | Int | no | FK |  |
| trnno | Int | no | PK |  |
| trndate | DateTime | no |  |  |
| ptype | VarChar(50) | yes |  |  |
| purchaseacccode | VarChar(6) | yes | FK |  |
| suppliercode | VarChar(6) | yes | FK |  |
| shopcode | VarChar(6) | yes | FK |  |
| docno | VarChar(20) | no |  |  |
| docdate | DateTime | yes |  |  |
| tppassno | VarChar(25) | no |  |  |
| schemecode | VarChar(6) | yes |  |  |
| totamount | Numeric(18,2) | yes |  |  |
| tottaxOth | Numeric(18,2) | yes |  |  |
| totnetamt | Numeric(18,2) | yes |  |  |
| narration | VarChar(100) | yes |  |  |
| checkedBy | VarChar(6) | yes |  |  |
| saletax_including_free | Bit | yes |  | ((0)) |

Foreign keys:

- `FK_purchasemain_ledger_purchaseacccode`: (companycode, purchaseacccode) -> `dbo.ledger` (companycode, ledcode)
- `FK_purchasemain_ledger_suppliercode`: (companycode, suppliercode) -> `dbo.ledger` (companycode, ledcode)
- `FK_purchasemain_storage`: (companycode, shopcode) -> `dbo.storage` (companycode, shopcode)
- `FK_purchasemain_trnidmst`: (companycode, yearcode, trnid) -> `dbo.trnidmst` (companycode, yearcode, trnid)

### `dbo.PurchaseTaxDetail` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| companycode | VarChar(6) | yes | FK |  |
| trnno | Int | no | FK |  |
| schemecode | VarChar(6) | yes | FK |  |
| TaxCode | VarChar(6) | yes | FK | ('') |
| TaxRate | Numeric(18,2) | yes |  | ((0)) |
| OnAmount | Numeric(18,2) | yes |  | ((0)) |
| TaxAmount | Numeric(18,2) | yes |  | ((0)) |
| TaxAccount | VarChar(6) | yes |  | ('') |
| yearcode | VarChar(6) | yes | FK | ('') |

Foreign keys:

- `FK_PurchaseTaxDetail_purchasemain`: (companycode, yearcode, trnno) -> `dbo.purchasemain` (companycode, yearcode, trnno)
- `FK_PurchaseTaxDetail_taxdetail`: (companycode, schemecode, TaxCode) -> `dbo.taxdetail` (companycode, SchemeCode, TaxCode)

### `dbo.receipt_detail` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| companycode | VarChar(6) | no | PK |  |
| yearcode | VarChar(6) | no | PK |  |
| vchno | Int | no | PK |  |
| trnno | VarChar(20) | no | PK |  |
| due_amount | Float | yes |  |  |
| debit | Float | yes |  |  |
| credit | Float | yes |  |  |
| vchdate | DateTime | no | PK |  |

Foreign keys:

- `FK_receipt_detail_receipt_main`: (companycode, yearcode, vchdate, vchno) -> `dbo.receipt_main` (companycode, yearcode, vchdate, vchno)
- `FK_receipt_detail_salesbillmain`: (companycode, yearcode, trnno) -> `dbo.salesbillmain` (companycode, yearcode, trnno)

### `dbo.receipt_main` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| companycode | VarChar(6) | no | PK |  |
| yearcode | VarChar(6) | no | PK |  |
| vchno | Int | no | PK |  |
| head_account | VarChar(6) | yes | FK |  |
| client_account | VarChar(6) | yes | FK |  |
| narration | VarChar(50) | yes |  |  |
| vchdate | DateTime | no | PK |  |
| cheque_no | VarChar(20) | yes |  |  |
| due | Float | yes |  |  |
| amount_paid | Float | yes |  |  |
| discount | Float | yes |  |  |
| net_due | Float | yes |  |  |
| Discount_account_head | VarChar(6) | yes | FK |  |
| receipt_type | VarChar(15) | yes |  |  |
| trnid | Int | no | FK | ((0)) |
| chqDate | DateTime | yes |  |  |

Foreign keys:

- `FK_receipt_main_ledger_clientac`: (companycode, client_account) -> `dbo.ledger` (companycode, ledcode)
- `FK_receipt_main_ledger_discac`: (companycode, Discount_account_head) -> `dbo.ledger` (companycode, ledcode)
- `FK_receipt_main_ledger_headac`: (companycode, head_account) -> `dbo.ledger` (companycode, ledcode)
- `FK_receipt_main_trnidmst`: (companycode, yearcode, trnid) -> `dbo.trnidmst` (companycode, yearcode, trnid)

### `dbo.reportTaggingDetail` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| reportName | VarChar(50) | yes |  |  |
| reportCategory | VarChar(50) | yes |  |  |
| groupname | VarChar(50) | yes |  |  |
| companycode | VarChar(6) | yes |  |  |

### `dbo.reportTaggingMain` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| reportName | VarChar(50) | yes |  |  |
| reportCategory | VarChar(50) | yes |  |  |
| companycode | VarChar(6) | yes |  |  |

### `dbo.sales_tax` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| trnno | VarChar(20) | no | PK |  |
| companycode | VarChar(6) | no | PK |  |
| yearcode | VarChar(6) | no | PK |  |
| cash_bank_book | VarChar(6) | yes |  |  |
| tax_book | VarChar(6) | no | PK |  |
| tax_amount | Float | yes |  |  |
| date | DateTime | yes |  |  |

Foreign keys:

- `FK_sales_tax_salesbillmain`: (companycode, yearcode, trnno) -> `dbo.salesbillmain` (companycode, yearcode, trnno)

### `dbo.salesbilldetail` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| companycode | VarChar(6) | no | PK |  |
| itemcode | VarChar(6) | no | PK |  |
| tot_box | Int | yes |  |  |
| loose | Int | yes |  |  |
| qnty | Int | yes |  |  |
| itemAmount | Float | yes |  |  |
| trnno | VarChar(20) | no | PK |  |
| rate | Float | yes |  |  |
| trndate | DateTime | yes |  |  |
| yearcode | VarChar(6) | no | PK |  |
| large | Int | yes |  |  |
| small | Int | yes |  |  |
| westage | Int | yes |  |  |
| MRP | Float | no |  | ((0)) |
| batchDate | DateTime | no | PK | (((1)/(1))/(2013)) |
| batchNo | VarChar(50) | no | PK | ((0)) |

Foreign keys:

- `FK_salesbilldetail_itemmst`: (companycode, itemcode) -> `dbo.itemmst` (companycode, itemcode)
- `FK_salesbilldetail_salesbillmain`: (companycode, yearcode, trnno) -> `dbo.salesbillmain` (companycode, yearcode, trnno)

### `dbo.salesbillmain` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| companycode | VarChar(6) | no | PK |  |
| trnno | VarChar(20) | no | PK |  |
| trndate | DateTime | no |  |  |
| ledgercode | VarChar(6) | no | FK |  |
| amount | Float | yes |  |  |
| salestype | VarChar(20) | yes |  |  |
| ratecode | VarChar(6) | yes | FK |  |
| storecode | VarChar(6) | yes | FK |  |
| yearcode | VarChar(6) | no | PK |  |
| discamount | Float | yes |  |  |
| netamount | Float | yes |  |  |
| account_head | VarChar(6) | yes | FK |  |
| discount_account_head | VarChar(6) | yes | FK |  |
| trntime | DateTime | yes |  |  |
| trnid | Int | no | FK | ((0)) |
| discount_reason | VarChar(50) | yes |  |  |
| KFL | VarChar(60) | no |  | ('none') |

Foreign keys:

- `FK_salesbillmain_companyparamtr_discntacchead`: (companycode, discount_account_head) -> `dbo.companyparamtr` (companycode, discntacc)
- `FK_salesbillmain_itemrateinfo`: (companycode, ratecode) -> `dbo.itemrateinfo` (companycode, ratecode)
- `FK_salesbillmain_ledger_acount_head`: (companycode, account_head) -> `dbo.ledger` (companycode, ledcode)
- `FK_salesbillmain_ledger_discount_account`: (companycode, discount_account_head) -> `dbo.ledger` (companycode, ledcode)
- `FK_salesbillmain_ledger_ledgercode`: (companycode, ledgercode) -> `dbo.ledger` (companycode, ledcode)
- `FK_salesbillmain_storage`: (companycode, storecode) -> `dbo.storage` (companycode, shopcode)
- `FK_salesbillmain_trnidmst`: (companycode, yearcode, trnid) -> `dbo.trnidmst` (companycode, yearcode, trnid)

### `dbo.stk_transfer_detail` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| CompanyCode | VarChar(6) | no | PK |  |
| ItemCode | VarChar(6) | no | PK |  |
| box | Int | yes |  |  |
| loose | Int | yes |  |  |
| qnty | Int | yes |  |  |
| trnno | Int | no | PK |  |
| trndate | DateTime | no | FK |  |
| yearcode | VarChar(6) | no | PK |  |
| batchDate | DateTime | no | PK | (((1)/(1))/(2013)) |
| batchNo | VarChar(50) | no | PK | ((0)) |

Foreign keys:

- `FK_stk_transfer_detail_itemmst`: (CompanyCode, ItemCode) -> `dbo.itemmst` (companycode, itemcode)
- `FK_stk_transfer_detail_stk_transfer_main`: (CompanyCode, yearcode, trndate, trnno) -> `dbo.stk_transfer_main` (CompanyCode, YearCode, TrnDate, TrnNo)

### `dbo.stk_transfer_main` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| CompanyCode | VarChar(6) | no | PK |  |
| YearCode | VarChar(6) | no | PK |  |
| TrnNo | Int | no | PK |  |
| TrnDate | DateTime | no | PK |  |
| shopname_from | VarChar(50) | yes |  |  |
| shopname_to | VarChar(50) | yes |  |  |
| shopcode_frm | VarChar(6) | yes |  |  |
| shopcode_to | VarChar(6) | yes |  |  |
| trnid | Int | no | FK | ((0)) |

Foreign keys:

- `FK_stk_transfer_main_trnidmst`: (CompanyCode, YearCode, trnid) -> `dbo.trnidmst` (companycode, yearcode, trnid)

### `dbo.storage` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| companycode | VarChar(6) | no | PK |  |
| shopcode | VarChar(6) | no | PK |  |
| shopname | VarChar(50) | yes |  |  |
| address | VarChar(50) | yes |  |  |
| ShopType | VarChar(3) | yes |  |  |

Foreign keys:

- `FK_storage_companymast`: (companycode) -> `dbo.companymast` (companycode)

### `dbo.strength` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| strengthname | VarChar(6) | no | PK |  |
| companycode | VarChar(6) | no | PK | ((0)) |

Foreign keys:

- `FK_strength_companymast`: (companycode) -> `dbo.companymast` (companycode)

### `dbo.taxdetail` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| companycode | VarChar(6) | no | PK |  |
| SchemeCode | VarChar(6) | no | PK |  |
| schemename | VarChar(50) | yes |  |  |
| SrNo | Int | yes |  |  |
| TaxCode | VarChar(6) | no | PK | ((0)) |
| taxname | VarChar(50) | yes |  |  |
| ledcode | VarChar(6) | yes | FK | ('') |
| sig | VarChar(1) | yes |  |  |
| TaxRate | Float | yes |  | ((0)) |
| taxamount | Float | yes |  |  |
| calc_on | VarChar(1) | yes |  |  |
| base | Int | yes |  |  |
| after_discount | Bit | no |  | ((0)) |
| after_saletax | Bit | no |  | ((0)) |

Foreign keys:

- `FK_taxdetail_companymast`: (companycode) -> `dbo.companymast` (companycode)
- `FK_taxdetail_ledger`: (companycode, ledcode) -> `dbo.ledger` (companycode, ledcode)

### `dbo.TEMP` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| companyCode | VarChar(6) | no | PK |  |
| yearCode | VarChar(6) | no | PK |  |
| kflName | VarChar(40) | no | PK |  |
| type | VarChar(20) | no | PK |  |
| groupName | VarChar(50) | no | PK |  |
| categoryName | VarChar(50) | no | PK |  |
| ml | Int | no | PK |  |
| qnty | Float | no | PK |  |
| batchno | VarChar(50) | no | PK | ('') |
| party | VarChar(100) | no | PK |  |
| billno | VarChar(50) | no | PK |  |
| tppassno | VarChar(50) | no | PK |  |
| trndate | DateTime | no | PK |  |

### `dbo.tempitem` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| item | NVarChar(225) | yes |  |  |

### `dbo.templedger` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| ledname | NVarChar(225) | yes |  |  |

### `dbo.trnidmst` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| companycode | VarChar(6) | no | PK |  |
| yearcode | VarChar(6) | no | PK |  |
| trnid | Int | no | PK |  |
| trn_type | VarChar(30) | no |  | ('old') |
| usercode | VarChar(6) | yes | FK |  |

Foreign keys:

- `FK_trnidmst_users`: (usercode) -> `dbo.users` (usercode)

### `dbo.userrights` (0 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| usercode | VarChar(6) | no | PK |  |
| companycode | VarChar(6) | no | PK |  |
| accaccess | VarChar(1) | yes |  |  |
| modulename | VarChar(50) | no | PK |  |
| perv | VarChar(5) | yes |  |  |
| pera | VarChar(5) | yes |  |  |
| pere | VarChar(5) | yes |  |  |
| perd | VarChar(5) | yes |  |  |

Foreign keys:

- `FK_userrights_companymast`: (companycode) -> `dbo.companymast` (companycode)
- `FK_userrights_users`: (usercode) -> `dbo.users` (usercode)

### `dbo.users` (1 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| usercode | VarChar(6) | no | PK |  |
| username | VarChar(50) | no |  |  |
| accesslevel | VarChar(6) | yes |  |  |
| password | VarChar(10) | yes |  |  |

### `dbo.yearmst` (1 rows)

| Column | Type | Null | Key | Default |
| --- | --- | --- | --- | --- |
| yearcode | VarChar(6) | no | PK |  |
| yearrange | VarChar(9) | yes |  |  |
| stdate | DateTime | yes |  |  |
| enddate | DateTime | yes |  |  |
| companycode | VarChar(6) | no | PK |  |

Foreign keys:

- `FK_yearmst_companymast`: (companycode) -> `dbo.companymast` (companycode)

## Views

- `dbo.sale_purchase_daywise`
- `dbo.stock`
- `dbo.vw_cash_sale_daywise`
- `dbo.vw_credit_sale_daywise`
- `dbo.vw_creditcard_sale_daywise`
- `dbo.vw_creditors`
- `dbo.vw_detors`
- `dbo.vw_discount_of_the_day`
- `dbo.vw_ltr_sale`
- `dbo.vw_payment_detail`
- `dbo.vw_payment_main`
- `dbo.vw_purchse_daywise`
- `dbo.vw_purchse_return_daywise`
- `dbo.vw_ratemst`
- `dbo.vw_receipt_accounts`
- `dbo.vw_receipt_detail`
- `dbo.vw_receipt_main`
- `dbo.vw_return_sale_daywise`
- `dbo.vw_sale`
- `dbo.vw_sales_type_all`
- `dbo.vw_salesbill_main`
- `dbo.vw_tally_data`
- `dbo.vw_tally_purchase`
- `dbo.vwSalesData`

## Stored procedures

- `dbo.serialise_n_reference_sale_bills`
- `dbo.usp_exciseStatement`
- `dbo.uspGetBatchWiseStock`

## Functions

- `dbo.DateRange`
- `dbo.opening_closing_stock`
- `dbo.opening_closing_stock_batchNo`
- `dbo.opening_closing_stock_rep`
- `dbo.opening_stock_show`
- `dbo.sales_statement`
- `dbo.stock_by_day`
- `dbo.xcise_statement_final`
- `dbo.xcise_statement1`
- `dbo.xcise_statement2`
- `dbo.xcise_stock`

