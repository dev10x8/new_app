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
import { nextTrnNo } from './services/helpers.js';

const db = openDatabase(config.dbFile);

function json(res, code, body) {
  const text = JSON.stringify(body);
  res.writeHead(code, { 'content-type': 'application/json', 'content-length': Buffer.byteLength(text) });
  res.end(text);
}

function readBody(req) {
  return new Promise((resolve, reject) => {
    let data = '';
    req.on('data', (c) => { data += c; });
    req.on('end', () => {
      if (!data) return resolve({});
      try { resolve(JSON.parse(data)); } catch (e) { reject(e); }
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
  console.log(`Madhushala Pro backend listening on http://${config.host}:${config.port}`);
  console.log(`Database: ${config.dbFile}`);
});

export { server, db, routes };
