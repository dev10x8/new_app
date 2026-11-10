import http from 'node:http';
import { URL } from 'node:url';
import { openDatabase } from './db/index.js';
import { config } from './config.js';
import * as auth from './services/auth.js';
import * as masters from './services/masters.js';
import * as sales from './services/sales.js';
import * as purchase from './services/purchase.js';
import * as stock from './services/stock.js';
import * as vouchers from './services/vouchers.js';
import * as reports from './services/reports.js';
import * as catalog from './services/reportCatalog.js';
import * as admin from './services/admin.js';
import * as txn from './services/transactions.js';
import * as tools from './services/tools.js';
import * as tally from './services/tally.js';
import * as excise from './services/excise.js';
import * as gst from './services/gst.js';
import * as inventory from './services/inventory.js';
import * as m22 from './services/m22.js';
import * as migration from './services/migration.js';
import * as bulkimport from './services/bulkimport.js';
import * as statemaster from './services/statemaster.js';
import * as parcha from './services/parcha.js';
import * as landedcost from './services/landedcost.js';
import { nextTrnNo } from './services/helpers.js';

const db = openDatabase(config.dbFile);

function json(res, code, body) {
  const text = JSON.stringify(body);
  res.writeHead(code, { 'content-type': 'application/json', 'content-length': Buffer.byteLength(text) });
  res.end(text);
}

function readBody(req) {
  return new Promise((resolve, reject) => {
    const chunks = [];
    req.on('data', (c) => { chunks.push(c); });
    req.on('end', () => {
      const buf = Buffer.concat(chunks);
      if (!buf.length) return resolve({});
      const ct = String(req.headers['content-type'] || '');
      // Raw binary upload (e.g. an .xlsx dropped on the endpoint): hand the
      // handler a base64 string it can turn back into a Buffer.
      if (ct.includes('application/octet-stream') || ct.includes('multipart/form-data')) {
        return resolve({ filebase64: buf.toString('base64') });
      }
      try { resolve(JSON.parse(buf.toString('utf8'))); } catch (e) { reject(e); }
    });
    req.on('error', reject);
  });
}

const routes = [];
function route(method, path, handler) { routes.push({ method, path, handler }); }

// ---- meta ----
route('GET', '/api/health', () => ({ ok: true, company: config.company, year: config.year }));
route('GET', '/api/meta', () => ({
  company: config.company,
  year: config.year,
  masters: Object.keys(masters.MASTERS),
  saleTypes: sales.listSaleTypes(),
  reports: reports.REPORTS,
  reportGroups: catalog.reportGroups(),
}));
route('GET', '/api/system-info', () => admin.systemInfo(db, config.company));

// ---- auth ----
route('POST', '/api/login', (b) => {
  const r = auth.login(db, b);
  if (!r.ok) throw Object.assign(new Error(r.error), { status: 401 });
  return r;
});
route('GET', '/api/users', () => auth.listUsers(db));
route('POST', '/api/users', (b) => auth.createUser(db, b));
route('PUT', '/api/users/:usercode', (b, p) => auth.updateUser(db, p.usercode, b));
route('POST', '/api/users/:usercode/reset-pin', (b, p) => auth.resetPin(db, p.usercode, b.pin));
route('POST', '/api/users/:usercode/rights', (b, p) => auth.setRights(db, p.usercode, b.company || config.company, b.module, b.rights || {}));

// ---- masters ----
route('GET', '/api/masters/:name', (b, p) => masters.listMaster(db, p.name));
route('GET', '/api/masters/:name/:key', (b, p) => masters.getMaster(db, p.name, { [p.name === 'item' ? 'itemcode' : 'code']: p.key, companycode: config.company }) || masters.getMaster(db, p.name, { itemcode: p.key, companycode: config.company }));
route('POST', '/api/masters/:name', (b, p) => masters.createMaster(db, p.name, b));
route('PUT', '/api/masters/:name', (b, p) => masters.updateMaster(db, p.name, b.keys, b.values));
route('DELETE', '/api/masters/:name', (b, p) => masters.deleteMaster(db, p.name, b.keys));
route('GET', '/api/items', () => masters.listItemsDetailed(db, config.company));
route('GET', '/api/items/find', (b, p, q) => masters.findItem(db, config.company, q.get('term') || ''));

// ---- inventory (dashboard, low-stock alerts, requisitions) ----
route('GET', '/api/inventory/summary', () => inventory.inventorySummary(db, { company: config.company, year: config.year }));
route('GET', '/api/inventory/stock', () => inventory.listItemStock(db, { company: config.company, year: config.year }));
route('GET', '/api/inventory/low-stock', () => inventory.lowStock(db, { company: config.company, year: config.year }));
route('POST', '/api/inventory/items', (b) => inventory.addItem(db, config.company, b));
route('GET', '/api/inventory/requisitions', (b, p, q) => inventory.requisitions(db, config.company, { status: q.get('status') }));
route('GET', '/api/inventory/requisitions/summary', () => inventory.requisitionSummary(db, config.company));
route('POST', '/api/inventory/requisitions/generate', (b) => inventory.generateRequisitions(db, { company: config.company, year: config.year, suppliercode: b && b.suppliercode }));
route('PUT', '/api/inventory/requisitions/:id', (b, p) => inventory.updateRequisition(db, config.company, Number(p.id), b));

// ---- sales / POS ----
route('GET', '/api/sales', (b, p, q) => sales.listSales(db, config.company, config.year, { from: q.get('from'), to: q.get('to'), salestype: q.get('salestype') }));
route('GET', '/api/sales/:trnno', (b, p) => sales.getSale(db, config.company, config.year, p.trnno));
route('POST', '/api/sales', (b) => sales.createSale(db, { company: config.company, year: config.year, ...b }));
route('DELETE', '/api/sales/:trnno', (b, p) => sales.deleteSale(db, config.company, config.year, p.trnno));
route('POST', '/api/day-close', (b) => sales.dayClose(db, config.company, config.year, b));
route('GET', '/api/sales-summary', (b, p, q) => sales.salesSummary(db, config.company, config.year, { from: q.get('from'), to: q.get('to') }));

// ---- purchase ----
route('GET', '/api/purchases', (b, p, q) => purchase.listPurchases(db, config.company, config.year, { from: q.get('from'), to: q.get('to'), ptype: q.get('ptype') }));
route('GET', '/api/purchases/:trnno', (b, p) => purchase.getPurchase(db, config.company, config.year, p.trnno));
route('POST', '/api/purchases', (b) => purchase.createPurchase(db, { company: config.company, year: config.year, ...b }));

// ---- stock ----
route('GET', '/api/stock', (b, p, q) => stock.stockOnHand(db, { company: config.company, year: config.year, storecode: q.get('storecode'), itemcode: q.get('itemcode') }));
route('POST', '/api/opening-stock', (b) => stock.addOpeningStock(db, { company: config.company, year: config.year, ...b }));
route('POST', '/api/stock-transfer', (b) => stock.stockTransfer(db, { company: config.company, year: config.year, trnno: b.trnno || nextTrnNo(db, 'stk_transfer_main', config.company, config.year, 'TrnNo'), ...b }));
route('POST', '/api/breakage', (b) => stock.addBreakage(db, { company: config.company, year: config.year, trnno: b.trnno || nextTrnNo(db, 'breakagemain', config.company, config.year, 'TrnNo'), ...b }));

// ---- vouchers ----
route('GET', '/api/vouchers/:type', (b, p) => vouchers.listVouchers(db, config.company, config.year, p.type.toUpperCase()));
route('POST', '/api/vouchers', (b) => {
  const type = String(b.type || '').toUpperCase();
  if (type === 'PAYMENT' || type === 'RECEIPT') return vouchers.createPaymentReceipt(db, { company: config.company, year: config.year, type, ...b });
  if (type === 'CONTRA') return vouchers.createContra(db, { company: config.company, year: config.year, ...b });
  if (type === 'JOURNAL') return vouchers.createJournal(db, { company: config.company, year: config.year, ...b });
  throw Object.assign(new Error('Unknown voucher type'), { status: 400 });
});
route('GET', '/api/ledger/:ledcode', (b, p) => vouchers.generalLedger(db, config.company, config.year, p.ledcode));

// ---- reports ----
route('GET', '/api/reports', () => reports.REPORTS);
route('GET', '/api/reports/:key', (b, p, q) => reports.runReport(db, p.key, { company: config.company, year: config.year, from: q.get('from'), to: q.get('to'), storecode: q.get('storecode') }));
route('GET', '/api/dashboard', () => reports.dashboard(db, config.company, config.year));

// ---- full report catalog ----
route('GET', '/api/report-groups', () => catalog.reportGroups());
route('GET', '/api/report-catalog/:key', (b, p, q) => catalog.runCatalogReport(db, p.key, {
  company: config.company, year: config.year, from: q.get('from'), to: q.get('to'), storecode: q.get('storecode'),
}));

// ---- admin ----
route('GET', '/api/admin/parameters', () => admin.getParameters(db, config.company));
route('PUT', '/api/admin/parameters', (b) => admin.updateParameters(db, config.company, b));
route('GET', '/api/admin/years', () => admin.listYears(db, config.company));
route('POST', '/api/admin/new-year', (b) => admin.createNewYear(db, { company: config.company, ...b }));
route('GET', '/api/admin/excise-audit', () => admin.exciseAudit(db, config.company, config.year));
route('GET', '/api/admin/deleted-bills', () => sales.listDeletedBills(db, config.company, config.year));

// ---- extended transactions ----
route('GET', '/api/transfer-sales', () => txn.listTransferSales(db, config.company, config.year));
route('POST', '/api/transfer-sales', (b) => txn.createTransferSale(db, { company: config.company, year: config.year, ...b }));
route('GET', '/api/transfer-receives', () => txn.listTransferReceives(db, config.company, config.year));
route('POST', '/api/transfer-receives', (b) => txn.createTransferReceive(db, { company: config.company, year: config.year, ...b }));
route('GET', '/api/notes', (b, p, q) => txn.listNotes(db, config.company, config.year, q.get('type')));
route('POST', '/api/notes', (b) => txn.createNote(db, { company: config.company, year: config.year, ...b }));
route('POST', '/api/on-account', (b) => txn.createOnAccountAdjustment(db, { company: config.company, year: config.year, ...b }));
route('POST', '/api/account-replace', (b) => txn.createAccountReplace(db, { company: config.company, year: config.year, ...b }));
route('GET', '/api/fines', () => txn.listFines(db, config.company, config.year));
route('POST', '/api/fines', (b) => txn.createFine(db, { company: config.company, year: config.year, ...b }));
route('GET', '/api/reconciliation', (b, p, q) => txn.listReconciliation(db, config.company, config.year, q.get('ledcode')));
route('POST', '/api/reconciliation', (b) => txn.addReconciliation(db, { company: config.company, year: config.year, ...b }));
route('POST', '/api/reconciliation/:slno/clear', (b, p) => txn.clearReconciliation(db, config.company, config.year, Number(p.slno), b.cleareddate));
route('GET', '/api/upi', () => txn.listUpi(db, config.company));
route('POST', '/api/upi', (b) => txn.recordUpi(db, { company: config.company, ...b }));
route('GET', '/api/deliveries', () => txn.listDeliveries(db, config.company, config.year));
route('POST', '/api/deliveries', (b) => txn.createDelivery(db, { company: config.company, year: config.year, ...b }));
route('PUT', '/api/deliveries/:orderno', (b, p) => txn.setDeliveryStatus(db, config.company, config.year, p.orderno, b.status));

// ---- tools ----
route('POST', '/api/tools/calc', (b) => ({ result: tools.calculate(b.a, b.op, b.b) }));
route('POST', '/api/tools/backup', (b) => tools.backupDatabase(config.dbFile, b.dest));
route('GET', '/api/tools/tally', (b, p, q) => tools.tallyExport(db, config.company, config.year, q.get('kind') || 'SALES'));
route('GET', '/api/tools/messages', () => tools.listMessages(db, config.company));
route('POST', '/api/tools/messages', (b) => tools.sendMessage(db, { company: config.company, ...b }));
route('GET', '/api/tools/sync-status', () => tools.syncStatus(db, config.company));
route('POST', '/api/tools/sync', (b) => tools.dataSync(db, { company: config.company, entity: b.entity, direction: b.direction }));
route('GET', '/api/tools/pml', () => tools.listPml(db, config.company, config.year));
route('POST', '/api/tools/pml', (b) => tools.addPml(db, { company: config.company, year: config.year, ...b }));
route('POST', '/api/tools/clean-print', () => tools.cleanPrintGarbage(db, config.company));

// ---- legacy data migration (admin) ----
route('GET', '/api/migration/tables', () => migration.tablesInfo(db));
route('GET', '/api/migration/export-sql', () => ({ script: migration.exportSqlScript() }));
route('GET', '/api/migration/history', () => migration.history(db, config.company));
route('GET', '/api/migration/batches/:id', (b, p) => migration.batchDetail(db, config.company, Number(p.id)));
route('POST', '/api/migration/analyze', (b) => migration.analyze(db, { company: config.company, year: config.year, ...b }));
route('POST', '/api/migration/apply', (b) => migration.apply(db, { company: config.company, year: config.year, ...b }));

// ---- bulk master / opening-stock import (Excel / CSV / JSON) ----
// A caller may send { content } (text), { filebase64 } (raw upload) or
// { buffer } is derived from filebase64 here.
const withFile = (b) => (b && b.filebase64 ? { ...b, buffer: Buffer.from(b.filebase64, 'base64') } : b);
route('GET', '/api/bulk/targets', () => ({ targets: Object.entries(bulkimport.MASTER_TARGETS).map(([k, v]) => ({ key: k, label: v.label })), templates: bulkimport.templates() }));
route('GET', '/api/bulk/history', () => bulkimport.history(db, config.company));
route('POST', '/api/bulk/analyze', (b) => bulkimport.analyze(db, { company: config.company, year: config.year, ...withFile(b) }));
route('POST', '/api/bulk/apply', (b) => bulkimport.apply(db, { company: config.company, year: config.year, ...withFile(b) }));

// ---- pre-configured state master list (approved brands / sizes / MRPs) ----
route('GET', '/api/state-master/datasets', () => statemaster.datasets());
route('GET', '/api/state-master', (b, p, q) => statemaster.listStateMaster(db, { company: config.company, state: q.get('state'), q: q.get('q') }));
route('GET', '/api/state-master/summary', () => statemaster.stateSummary(db, config.company));
route('POST', '/api/state-master/load', (b) => statemaster.loadState(db, { company: config.company, ...b }));
route('POST', '/api/state-master/items/analyze', (b) => statemaster.analyzeItems(db, { company: config.company, year: config.year, ...b }));
route('POST', '/api/state-master/items/apply', (b) => statemaster.applyItems(db, { company: config.company, year: config.year, ...b }));

// ---- pack sizes (Quart / Pint / Nip / ...) ----
route('GET', '/api/pack-sizes', () => statemaster.listPackSizes(db, config.company));
route('POST', '/api/pack-sizes', (b) => statemaster.createPackSize(db, config.company, b));
route('DELETE', '/api/pack-sizes/:id', (b, p) => statemaster.deletePackSize(db, config.company, p.id));

// ---- Parcha (purchase invoice) import ----
route('GET', '/api/parcha/history', () => parcha.history(db, config.company));
route('GET', '/api/parcha/batches/:id', (b, p) => parcha.batchDetail(db, config.company, Number(p.id)));
route('POST', '/api/parcha/analyze', (b) => parcha.analyze(db, { company: config.company, year: config.year, filename: b.filename, invoice: parcha.loadInvoice(withFile(b)) }));
route('POST', '/api/parcha/apply', (b) => parcha.apply(db, { company: config.company, year: config.year, filename: b.filename, invoice: parcha.loadInvoice(withFile(b)), createMissing: !!b.createMissing, marginPct: b.marginPct, setOpeningStock: !!b.setOpeningStock, shopcode: b.shopcode }));

// ---- OCR / document capture (Parcha scan) ----
route('GET', '/api/parcha/ocr-config', () => ({ ...parcha.ocrConfig(db, config.company), tesseractAvailable: parcha.tesseractAvailable() }));
route('PUT', '/api/parcha/ocr-config', (b) => parcha.saveOcrConfig(db, config.company, b));
route('POST', '/api/parcha/ocr', async (b) => {
  const buf = Buffer.from(String((b && b.image) || ''), 'base64');
  const { provider, text } = await parcha.ocrExtract(db, config.company, { buffer: buf, filename: (b && b.filename) || 'scan.png' });
  const invoice = parcha.parseInvoiceText(text);
  return { provider, text, invoice: parcha.normalizeInvoice(invoice) };
});

// ---- landing cost / margin ----
route('GET', '/api/landedcost/report', (b, p, q) => landedcost.report(db, { company: config.company, year: config.year, from: q.get('from'), to: q.get('to') }));
route('GET', '/api/landedcost/items', () => landedcost.itemSummary(db, { company: config.company, year: config.year }));
route('POST', '/api/landedcost/compute', (b) => landedcost.computePurchase(db, { company: config.company, year: config.year, trnno: b.trnno, otherCharges: b.otherCharges }));
route('POST', '/api/landedcost/apply-margin', (b) => landedcost.applyMarginToItem(db, { company: config.company, itemcode: b.itemcode, landingcost: b.landingcost, marginPct: b.marginPct, salesrate: b.salesrate }));

// ---- remote Tally sync (connectionless twin-agent) ----
route('GET', '/api/tally/config', () => tally.getConfig(db, config.company));
route('PUT', '/api/tally/config', (b) => tally.saveConfig(db, config.company, b));
route('POST', '/api/tally/token', () => tally.saveConfig(db, config.company, { ...tally.getConfig(db, config.company), agenttoken: tally.newAgentToken() }));
route('GET', '/api/tally/queue', (b, p, q) => tally.queue(db, config.company, { status: q.get('status') }));
route('GET', '/api/tally/summary', () => tally.queueSummary(db, config.company));
route('POST', '/api/tally/enqueue', () => tally.enqueueAll(db, config.company, config.year));
route('POST', '/api/tally/retry', () => tally.retryFailed(db, config.company));
route('GET', '/api/tally/xml', (b, p, q) => {
  const ids = (q.get('ids') || '').split(',').map((x) => Number(x)).filter(Boolean);
  return tally.buildVoucherXmlBatch(db, config.company, config.year, { ids, limit: Number(q.get('limit')) || 100 });
});
route('GET', '/api/tally/report', () => tally.syncReport(db, config.company, config.year));
route('GET', '/api/tally/logs', () => tally.logs(db, config.company));
// ---- live Tally Prime push (HTTP/XML) ----
route('POST', '/api/tally/live-test', () => tally.liveTest(db, config.company));
route('POST', '/api/tally/live-push', (b) => tally.livePush(db, config.company, config.year, { limit: (b && b.limit) || 100, ids: b && b.ids }));
route('GET', '/api/tally/mapping', (b, p, q) => tally.listMapping(db, config.company, q.get('type')));
route('POST', '/api/tally/mapping', (b) => tally.addMapping(db, { company: config.company, ...b }));
route('DELETE', '/api/tally/mapping/:id', (b, p) => ({ deleted: tally.deleteMapping(db, Number(p.id)) }));
route('GET', '/api/tally/inbound', (b, p, q) => tally.listInbound(db, config.company, q.get('status')));
route('POST', '/api/tally/inbound/apply', (b) => tally.applyInbound(db, config.company, b.id));

// agent-facing endpoints (called by the shop agent / cloud broker)
route('GET', '/api/tally/agent/pull', (b, p, q) => {
  const cfg = tally.getConfig(db, config.company);
  tally.requireToken(cfg, q.get('token'));
  const out = tally.buildVoucherXmlBatch(db, config.company, config.year, { limit: Number(q.get('limit')) || 100 });
  tally.touchPull(db, config.company);
  return out;
});
route('POST', '/api/tally/agent/ack', (b) => {
  const cfg = tally.getConfig(db, config.company);
  tally.requireToken(cfg, b.token);
  const r = tally.acknowledge(db, config.company, b.results || []);
  tally.touchPush(db, config.company);
  return r;
});
route('POST', '/api/tally/inbound', (b) => {
  const cfg = tally.getConfig(db, config.company);
  tally.requireToken(cfg, b.token);
  const masters = b.masters || [];
  for (const m of masters) tally.receiveMaster(db, config.company, m);
  tally.touchPush(db, config.company);
  return { received: masters.length };
});

// ---- state excise compliance sync ----
route('GET', '/api/excise/adapters', () => excise.listAdapters());
route('GET', '/api/excise/config', () => excise.getConfig(db, config.company));
route('PUT', '/api/excise/config', (b) => excise.saveConfig(db, config.company, b));
route('GET', '/api/excise/queue', (b, p, q) => excise.queue(db, config.company, { status: q.get('status') }));
route('GET', '/api/excise/summary', () => excise.queueSummary(db, config.company));
route('POST', '/api/excise/capture', (b) => (b && b.trnno
  ? { captured: excise.captureSale(db, { company: config.company, year: config.year, trnno: b.trnno }) }
  : excise.captureAll(db, config.company, config.year)));
route('POST', '/api/excise/upload', (b) => excise.uploadQueue(db, {
  company: config.company, state: b && b.state, force: !!(b && b.force),
}));
route('POST', '/api/excise/retry', () => excise.retryFailed(db, config.company));
route('GET', '/api/excise/portal-batch', (b, p, q) => excise.buildPortalBatch(db, config.company, {
  state: q.get('state'), status: q.get('status') || undefined,
}));
route('POST', '/api/excise/portal-confirm', (b) => excise.confirmUploaded(db, config.company, b || {}));
route('GET', '/api/excise/unmatched', () => excise.validateQueue(db, config.company));
route('GET', '/api/excise/closing-balance', (b, p, q) => {
  const date = q.get('date');
  if (q.get('build') === '1' && date) excise.buildClosingBalance(db, { company: config.company, year: config.year, date });
  return excise.listClosingBalance(db, config.company, date);
});
route('POST', '/api/excise/closing-balance/build', (b) => excise.buildClosingBalance(db,
  { company: config.company, year: config.year, date: (b && b.date) }));
route('POST', '/api/excise/closing-balance/upload', (b) => excise.uploadClosingBalance(db,
  { company: config.company, state: b && b.state, date: b && b.date }));
route('GET', '/api/excise/inbound', (b, p, q) => excise.listInbound(db, config.company, q.get('status')));
route('POST', '/api/excise/inbound', (b) => ({ id: excise.receiveInbound(db, config.company, b) }));
route('POST', '/api/excise/inbound/apply', (b) => excise.applyInbound(db, config.company, b.id));
route('GET', '/api/excise/logs', () => excise.logs(db, config.company));
route('GET', '/api/excise/report', () => excise.syncReport(db, config.company));

// ---- Form M-22 (brand register) ----
route('GET', '/api/excise/m22', () => m22.listFormM22(db, config.company));
route('POST', '/api/excise/m22/build', (b) => m22.buildFormM22(db, { company: config.company, year: config.year, from: b && b.from, to: b && b.to }));
route('GET', '/api/excise/m22/:id', (b, p) => m22.getFormM22(db, config.company, Number(p.id)));
route('POST', '/api/excise/m22/:id/file', (b, p) => m22.fileFormM22(db, config.company, Number(p.id)));
route('GET', '/api/excise/m22/:id/print', (b, p) => ({ text: m22.renderFormM22Text(db, config.company, Number(p.id)) }));

// ---- GST (rates, returns and filing) ----
route('GET', '/api/gst/config', () => gst.getConfig(db, config.company));
route('PUT', '/api/gst/config', (b) => gst.saveConfig(db, config.company, b || {}));
route('GET', '/api/gst/rates', () => gst.listItemRates(db, config.company));
route('GET', '/api/gst/rate-slab', (b, p, q) => gst.naiGstReport(db, config.company, config.year,
  { from: q.get('from'), to: q.get('to'), subgroupcode: q.get('subgroup') }));
route('GET', '/api/gst/gstr1', (b, p, q) => gst.gstr1(db, config.company, config.year,
  { from: q.get('from'), to: q.get('to') }));
route('GET', '/api/gst/gstr3b', (b, p, q) => gst.gstr3b(db, config.company, config.year,
  { from: q.get('from'), to: q.get('to') }));
route('GET', '/api/gst/returns', () => gst.listReturns(db, config.company));
route('POST', '/api/gst/returns/build', (b) => gst.buildReturn(db, config.company, config.year,
  { rettype: (b && b.rettype) || 'GSTR1', period: b && b.period }));
route('POST', '/api/gst/returns/:id/file', (b, p) => gst.fileReturn(db, config.company, Number(p.id), b || {}));
route('GET', '/api/gst/returns/:id/json', (b, p) => gst.returnJson(db, config.company, Number(p.id)));
route('GET', '/api/gst/logs', () => gst.logs(db, config.company));
route('GET', '/api/gst/invoice/:trnno', (b, p) => gst.taxInvoice(db, config.company, config.year, p.trnno));

const server = http.createServer(async (req, res) => {
  const url = new URL(req.url, `http://${req.headers.host}`);
  const path = url.pathname;
  for (const r of routes) {
    const params = matchPath(r.path, path);
    if (r.method === req.method && params) {
      try {
        const body = ['POST', 'PUT', 'PATCH', 'DELETE'].includes(req.method) ? await readBody(req) : {};
        const out = await r.handler(body, params, url.searchParams);
        return json(res, 200, { ok: true, data: out });
      } catch (err) {
        return json(res, err.status || 400, { ok: false, error: err.message });
      }
    }
  }
  json(res, 404, { ok: false, error: `No route for ${req.method} ${path}` });
});

function matchPath(pattern, actual) {
  const pp = pattern.split('/');
  const ap = actual.split('/');
  if (pp.length !== ap.length) return null;
  const params = {};
  for (let i = 0; i < pp.length; i++) {
    if (pp[i].startsWith(':')) params[pp[i].slice(1)] = decodeURIComponent(ap[i]);
    else if (pp[i] !== ap[i]) return null;
  }
  return params;
}

server.listen(config.port, config.host, () => {
  console.log(`VanijLiquor backend listening on http://${config.host}:${config.port}`);
  console.log(`Database: ${config.dbFile}`);
});

export { server, db, routes };
