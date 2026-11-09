#!/usr/bin/env node
// Seed a rich, realistic demo dataset for the user guide and for demos.
//
// Run on a FRESH database (this script resets it first):
//   node scripts/seed_demo.js
//   MADHUSHALA_DB=/tmp/demo.db node scripts/seed_demo.js
//
// It writes through the same service layer the app uses, so every number the
// guide shows (stock, ledger, reports) is internally consistent.

import { existsSync, rmSync } from 'node:fs';
import { dirname, resolve } from 'node:path';
import { fileURLToPath } from 'node:url';
import { openDatabase, all } from '../backend/src/db/index.js';
import * as masters from '../backend/src/services/masters.js';
import * as sales from '../backend/src/services/sales.js';
import * as purchase from '../backend/src/services/purchase.js';
import * as stock from '../backend/src/services/stock.js';
import * as vouchers from '../backend/src/services/vouchers.js';
import * as txn from '../backend/src/services/transactions.js';
import * as tools from '../backend/src/services/tools.js';
import * as tally from '../backend/src/services/tally.js';
import * as excise from '../backend/src/services/excise.js';
import * as gst from '../backend/src/services/gst.js';
import * as auth from '../backend/src/services/auth.js';

const here = dirname(fileURLToPath(import.meta.url));
const DB_FILE = process.env.MADHUSHALA_DB || resolve(here, '..', 'data', 'madhushala.db');
const C = '1';
const Y = '2024';

// --- start clean -----------------------------------------------------------
for (const suffix of ['', '-wal', '-shm']) {
  const f = DB_FILE + suffix;
  if (existsSync(f)) rmSync(f);
}
const db = openDatabase(DB_FILE);
const master = (name, values) => masters.createMaster(db, name, values);

console.log('Seeding demo data into', DB_FILE);

// --- reference masters the seed does not already ship ----------------------
master('category', { companycode: C, categorycode: 'C00004', categoryname: 'Vodka' });
master('category', { companycode: C, categorycode: 'C00005', categoryname: 'Country Spirit' });

master('manufacturer', { companycode: C, manufcode: 'M00003', manufname: 'Pernod Ricard' });
master('manufacturer', { companycode: C, manufcode: 'M00004', manufname: 'Radico Khaitan' });

master('area', { companycode: C, areacode: 'A3', areaname: 'Park Street' });

master('customer', {
  companycode: C, custcode: 'CU0003', custname: 'Sunset Bar & Grill',
  phone: '9830011223', email: 'accounts@sunsetbar.example', address: '24 Park Street',
  areacode: 'A3', gstin: '19AABCS1429B1ZX',
});

// a few more sellable lines so reports look alive
const NEW_ITEMS = [
  ['G00001', 'I00004', 'Absolut Vodka 750ml', 'C00004', 'K00001', 12, 'FULL', 750, '8901000000042', 1450, 1150, 36],
  ['G00001', 'I00005', 'Magic Moments 750ml', 'C00004', 'K00001', 12, 'FULL', 750, '8901000000059', 620, 480, 12],
  ['G00002', 'I00006', 'Budweiser 650ml', 'C00002', 'K00002', 24, 'BEER', 650, '8901000000066', 150, 115, 24],
];
for (const [groupcode, itemcode, itemname, categorycode, kflcode, packing, strengthname, ml, barcode, salesrate, purchaseRate, min_qnty] of NEW_ITEMS) {
  master('item', {
    companycode: C, groupcode, itemcode, itemname, categorycode, kflcode, packing, strengthname, ml,
    barcode, salesrate, purchaseRate, min_qnty,
    purchase_discount_amount: 0, purchase_salestaxonmrp_amount: 0, purchase_taxscheme_amount: 0,
  });
  master('itemrate', { companycode: C, ratecode: 'G00001', itemcode, salesrate });
}

// GST rate card for the demo items (alcohol: 18%/28%, exclusive of tax).
db.exec(`UPDATE itemmst SET cgst=9, sgst=9, igst=0, gsttype=0,
  CgstOtptLdgr='L00011', SgstOtptLdgr='L00012', IgstOtptLdgr='L00013',
  CgstInptLdgr='L00014', SgstInptLdgr='L00015', IgstInptLdgr='L00016'
  WHERE companycode='${C}' AND itemcode IN ('I00001','I00002','I00005')`);
db.exec(`UPDATE itemmst SET cgst=14, sgst=14, igst=0, gsttype=0,
  CgstOtptLdgr='L00011', SgstOtptLdgr='L00012', IgstOtptLdgr='L00013',
  CgstInptLdgr='L00014', SgstInptLdgr='L00015', IgstInptLdgr='L00016'
  WHERE companycode='${C}' AND itemcode IN ('I00003','I00004','I00006')`);

// second store / counter for transfer flows
master('storage', { companycode: C, shopcode: 'BAR1', shopname: 'Main Bar', address: 'Kolkata', ShopType: 'POS' });

// --- opening stock (1 April 2024) ------------------------------------------
const OPENING = [
  ['MAIN', 'I00001', 240], ['MAIN', 'I00002', 180], ['MAIN', 'I00003', 480],
  ['MAIN', 'I00004', 60], ['MAIN', 'I00005', 120], ['MAIN', 'I00006', 240],
];
for (const [shopcode, itemcode, qnty] of OPENING) {
  stock.addOpeningStock(db, { company: C, year: Y, shopcode, itemcode, qnty, trndate: '2024-04-01' });
}

// --- purchases (goods receipts) --------------------------------------------
const PURCHASES = [
  { date: '2024-04-03', docno: 'INV-8801', shopcode: 'MAIN', lines: [
    { itemcode: 'I00001', batchno: 'B1', itemrate: 200, itemquantity: 240, itemmrp: 250 },
    { itemcode: 'I00002', batchno: 'B1', itemrate: 260, itemquantity: 120, itemmrp: 320 },
  ] },
  { date: '2024-04-04', docno: 'INV-8802', shopcode: 'MAIN', lines: [
    { itemcode: 'I00003', batchno: 'B2', itemrate: 70, itemquantity: 480, itemmrp: 90 },
    { itemcode: 'I00006', batchno: 'B2', itemrate: 115, itemquantity: 240, itemmrp: 150 },
  ] },
  { date: '2024-04-05', docno: 'INV-8803', shopcode: 'MAIN', lines: [
    { itemcode: 'I00004', batchno: 'B3', itemrate: 1150, itemquantity: 60, itemmrp: 1450 },
    { itemcode: 'I00005', batchno: 'B3', itemrate: 480, itemquantity: 120, itemmrp: 620 },
  ] },
];
const purchaseNos = [];
for (const p of PURCHASES) {
  const r = purchase.createPurchase(db, {
    company: C, year: Y, trndate: p.date, ptype: 'PURCHASE', suppliercode: 'S00001',
    purchaseacccode: 'L00004', shopcode: p.shopcode, docno: p.docno, lines: p.lines,
  });
  purchaseNos.push(r.trnno || r);
}

// --- sales over several days ----------------------------------------------
const SALES = [
  { date: '2024-04-06', store: 'CNT1', type: 'FL', ledger: 'C00001', lines: [
    { itemcode: 'I00001', qnty: 12, rate: 250 }, { itemcode: 'I00003', qnty: 24, rate: 90 } ] },
  { date: '2024-04-06', store: 'CNT1', type: 'CS', ledger: 'CU0002', discount: 40, lines: [
    { itemcode: 'I00002', qnty: 6, rate: 320 }, { itemcode: 'I00006', qnty: 24, rate: 150 } ] },
  { date: '2024-04-07', store: 'BAR1', type: 'FL', ledger: 'C00001', lines: [
    { itemcode: 'I00004', qnty: 2, rate: 1450 }, { itemcode: 'I00005', qnty: 6, rate: 620 } ] },
  { date: '2024-04-07', store: 'CNT1', type: 'NAI', ledger: 'C00001', lines: [
    { itemcode: 'I00003', qnty: 48, rate: 90 } ] },
  { date: '2024-04-08', store: 'CNT1', type: 'FL', ledger: 'CU0003', discount: 100, lines: [
    { itemcode: 'I00001', qnty: 24, rate: 250 }, { itemcode: 'I00002', qnty: 12, rate: 320 } ] },
  { date: '2024-04-08', store: 'BAR1', type: 'CS', ledger: 'CU0002', lines: [
    { itemcode: 'I00005', qnty: 12, rate: 620 } ] },
  { date: '2024-04-09', store: 'CNT1', type: 'RETURN', ledger: 'C00001', lines: [
    { itemcode: 'I00003', qnty: 6, rate: 90 } ] },
];
const saleNos = [];
for (const s of SALES) {
  const r = sales.createSale(db, {
    company: C, year: Y, trndate: s.date, storecode: s.store, salestype: s.type,
    ledgercode: s.ledger, discount: s.discount || 0, lines: s.lines,
  });
  saleNos.push(r.trnno);
}

// --- stock transfer MAIN -> BAR1 -------------------------------------------
stock.stockTransfer(db, {
  company: C, year: Y, trnno: 1, trndate: '2024-04-08', from: 'MAIN', to: 'BAR1',
  fromName: 'Main Store', toName: 'Main Bar',
  lines: [{ itemcode: 'I00001', qnty: 36 }, { itemcode: 'I00003', qnty: 96 }],
});

// --- breakage --------------------------------------------------------------
stock.addBreakage(db, {
  company: C, year: Y, trnno: 1, trndate: '2024-04-09', shopcode: 'MAIN',
  narration: 'Bottles broken in transit', ledgercode: 'L00010', billno: 'BR-1',
  lines: [{ itemcode: 'I00003', quantity: 6 }, { itemcode: 'I00001', quantity: 1 }],
});

// --- counter / transfer sale and receive -----------------------------------
txn.createTransferSale(db, {
  company: C, year: Y, trndate: '2024-04-08', from: 'MAIN', to: 'CNT1', narration: 'Counter replenishment',
  lines: [{ itemcode: 'I00002', qnty: 12, rate: 320 }],
});
txn.createTransferReceive(db, {
  company: C, year: Y, trndate: '2024-04-08', from: 'MAIN', to: 'CNT1',
  narration: 'Received at counter', lines: [{ itemcode: 'I00002', qnty: 12 }],
});

// --- vouchers --------------------------------------------------------------
vouchers.createPaymentReceipt(db, {
  company: C, year: Y, type: 'PAYMENT', vchdate: '2024-04-06', head_account: 'L00002',
  client_account: 'L00008', amount_paid: 50000, narration: 'Payment to ABC Wines', trnno: purchaseNos[0],
});
vouchers.createPaymentReceipt(db, {
  company: C, year: Y, type: 'RECEIPT', vchdate: '2024-04-08', head_account: 'L00001',
  client_account: 'L00007', amount_paid: 25000, narration: 'Receipt from Hotel Blue',
});
vouchers.createContra(db, {
  company: C, year: Y, vchdate: '2024-04-07', from_ledger: 'L00001', to_ledger: 'L00002',
  amount: 30000, narration: 'Cash deposited to bank',
});
vouchers.createJournal(db, {
  company: C, year: Y, vchdate: '2024-04-09', narration: 'Depreciation for the month',
  lines: [
    { ledcode: 'L00005', debit: 1500, narr: 'Depreciation' },
    { ledcode: 'L00001', credit: 1500, narr: 'Depreciation' },
  ],
});

// --- notes, adjustments, fine, BRS, UPI, delivery --------------------------
txn.createNote(db, {
  company: C, year: Y, trndate: '2024-04-09', type: 'DEBIT', partycode: 'S00001',
  headaccount: 'L00004', amount: 2200, narration: 'Short supply claim',
  lines: [{ ledcode: 'L00004', amount: 2200, narration: 'Short supply' }],
});
txn.createNote(db, {
  company: C, year: Y, trndate: '2024-04-09', type: 'CREDIT', partycode: 'CU0002',
  headaccount: 'L00003', amount: 1500, narration: 'Rate difference credit',
  lines: [{ ledcode: 'L00003', amount: 1500, narration: 'Rate difference' }],
});
txn.createOnAccountAdjustment(db, {
  company: C, year: Y, trndate: '2024-04-08', partycode: 'L00007', against_ledger: 'L00003',
  amount: 5000, narration: 'On account adjustment',
});
txn.createAccountReplace(db, {
  company: C, year: Y, trndate: '2024-04-08', from_ledger: 'L00001', to_ledger: 'L00002',
  amount: 2000, narration: 'Ledger substitution',
});
txn.createFine(db, {
  company: C, year: Y, trndate: '2024-04-09', amount: 5000, narration: 'Excise fine for label mismatch',
  expense_ledger: 'L00010', payment_ledger: 'L00001',
});
txn.addReconciliation(db, {
  company: C, year: Y, ledcode: 'L00002', trndate: '2024-04-07', chqno: 'CHQ-1001',
  chqdate: '2024-04-10', amount: 30000, cleared: false,
});
txn.addReconciliation(db, {
  company: C, year: Y, ledcode: 'L00002', trndate: '2024-04-06', chqno: 'CHQ-0998',
  chqdate: '2024-04-06', amount: 12000, cleared: true, cleareddate: '2024-04-07',
});
txn.recordUpi(db, {
  company: C, trnno: saleNos[0], trndate: '2024-04-06', amount: 4680,
  upiid: 'sunset@okhdfc', status: 'SUCCESS', refno: 'UPI-77341',
});
txn.createDelivery(db, {
  company: C, year: Y, trndate: '2024-04-09', custcode: 'CU0003', address: '24 Park Street',
  phone: '9830011223', amount: 2900, status: 'OPEN', narration: 'Home delivery order',
});

// --- tools: SMS log, PML, data sync status ---------------------------------
tools.sendMessage(db, { company: C, channel: 'SMS', recipient: '9800000000', message: 'Bill FL00001: Rs 4680. Thank you.' });
tools.sendMessage(db, { company: C, channel: 'WhatsApp', recipient: '9830011223', message: 'Your order DLV00001 is out for delivery.' });
tools.addPml(db, { company: C, year: Y, trndate: '2024-04-09', itemcode: 'I00001', ml: 750, bottles: 60, narration: 'PML for April' });
tools.addPml(db, { company: C, year: Y, trndate: '2024-04-09', itemcode: 'I00002', ml: 750, bottles: 24, narration: 'PML for April' });
tools.dataSync(db, { company: C, entity: 'SALES', direction: 'UPLOAD' });
tools.dataSync(db, { company: C, entity: 'PURCHASE', direction: 'UPLOAD' });

// --- remote Tally sync: queue a few vouchers, deliver some -----------------
tally.saveConfig(db, C, {
  enabled: 1, brokerurl: 'https://tally-broker.example/api', agenttoken: tally.newAgentToken(),
  companyguid: 'abm-technomatrix-1', tallycompany: 'ABM TECHNOMATRIX (P) LTD.', agentid: 'SHOP-KOL-01',
});
tally.addMapping(db, { company: C, maptype: 'LEDGER', pattern: 'L00003', targetname: 'Sales @ 18%' });
tally.addMapping(db, { company: C, maptype: 'LEDGER', pattern: 'L00004', targetname: 'Purchase @ 18%' });
tally.addMapping(db, { company: C, maptype: 'LEDGER', pattern: 'S00001', targetname: 'ABC Wines (Tally)' });
tally.addMapping(db, { company: C, maptype: 'ITEM', pattern: 'I00001', targetname: 'Royal Stag 750ml' });
tally.enqueueAll(db, C, Y);
const queued = tally.queue(db, C);
tally.acknowledge(db, C, queued.slice(0, Math.max(1, queued.length - 1)).map((r) => ({ id: r.id, status: 'ACKED' })));
if (queued.length) {
  tally.acknowledge(db, C, [{ id: queued[queued.length - 1].id, status: 'FAILED', message: 'Ledger not found in Tally company' }]);
}

// --- state excise sync (West Bengal, api mode) -----------------------------
excise.saveConfig(db, C, {
  state: 'WB', endpointbase: 'https://excise.wb.gov.in/APIEODB/EODBPLA',
  licenseeid: 'WB-LIC-2024-0091', credential: 'demo-credential',
});
excise.captureAll(db, C, Y);
const exQueue = excise.queue(db, C);
excise.buildClosingBalance(db, { company: C, year: Y, date: '2024-04-09' });

// --- GST: filing settings and a generated GSTR-1 ---------------------------
gst.saveConfig(db, C, {
  gstin: '19AABCA1234B1Z5', legalname: 'ABM TECHNOMATRIX (P) LTD.',
  tradename: 'MADHUSHALA PRO', statecode: '19',
});
gst.buildReturn(db, C, Y, { rettype: 'GSTR1', period: '2024-04' });

// --- users and rights ------------------------------------------------------
auth.createUser(db, { usercode: 'A00003', username: 'Store Manager', accesslevel: 'User', password: 'store' });
for (const [module, r] of [
  ['MASTERS', { perv: 'Y', pera: 'Y', pere: 'Y', perd: 'N' }],
  ['TRANSACTIONS', { perv: 'Y', pera: 'Y', pere: 'Y', perd: 'N' }],
  ['REPORTS', { perv: 'Y', pera: 'N', pere: 'N', perd: 'N' }],
]) auth.setRights(db, 'A00003', C, module, r);

// --- report tagging --------------------------------------------------------
master('reporttagmain', { companycode: C, reportName: 'Sales Register' });
master('reporttagdetail', { companycode: C, reportName: 'Sales Register' });

// --- summary ---------------------------------------------------------------
const count = (sql) => db.prepare(sql).get().n;
console.log('Demo data ready:');
console.log(`  items          ${count('SELECT COUNT(*) n FROM itemmst')}`);
console.log(`  purchases      ${count('SELECT COUNT(*) n FROM purchasemain')}`);
console.log(`  sales bills    ${count('SELECT COUNT(*) n FROM salesbillmain')}`);
console.log(`  excise queue   ${exQueue.length}`);
console.log(`  tally queue    ${queued.length}`);
console.log(`  users          ${all(db, 'SELECT usercode FROM users').map((u) => u.usercode).join(', ')}`);
db.close();
