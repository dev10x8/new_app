// Thin HTTP client for the backend API. Mirrors tui/src/lib/api.js but runs
// in the browser (and inside the Tauri webview).

// Base URL resolution:
//  - Tauri desktop: the bundled backend runs on localhost:7100.
//  - Served over HTTP (docker compose): the web server proxies /api, so a
//    same-origin (empty) base is used and there is no CORS to worry about.
//  - Overridable with window.__API_BASE__ if needed.
const isDesktop = typeof location !== 'undefined'
  && (location.protocol === 'tauri:' || location.protocol === 'file:');
const BASE = (typeof window !== 'undefined' && window.__API_BASE__)
  || (isDesktop ? 'http://127.0.0.1:7100' : '');

async function req(method, path, body) {
  const opts = { method, headers: {} };
  if (body !== undefined) {
    opts.headers['content-type'] = 'application/json';
    opts.body = JSON.stringify(body);
  }
  const res = await fetch(BASE + path, opts);
  const json = await res.json().catch(() => ({ ok: false, error: 'Invalid JSON from server' }));
  if (!json.ok) throw new Error(json.error || `HTTP ${res.status}`);
  return json.data;
}

const qs = (o = {}) => {
  const p = new URLSearchParams();
  for (const [k, v] of Object.entries(o)) if (v !== undefined && v !== null && v !== '') p.set(k, v);
  const s = p.toString();
  return s ? `?${s}` : '';
};

export const api = {
  base: BASE,
  health: () => req('GET', '/api/health'),
  meta: () => req('GET', '/api/meta'),
  systemInfo: () => req('GET', '/api/system-info'),
  login: (usercode, password) => req('POST', '/api/login', { usercode, password }),

  users: () => req('GET', '/api/users'),
  createUser: (u) => req('POST', '/api/users', u),
  updateUser: (usercode, u) => req('PUT', `/api/users/${usercode}`, u),
  resetPin: (usercode, pin) => req('POST', `/api/users/${usercode}/reset-pin`, { pin }),
  setRights: (usercode, module, rights) => req('POST', `/api/users/${usercode}/rights`, { module, rights }),

  masters: (name) => req('GET', `/api/masters/${name}`),
  createMaster: (name, values) => req('POST', `/api/masters/${name}`, values),
  updateMaster: (name, keys, values) => req('PUT', `/api/masters/${name}`, { keys, values }),
  deleteMaster: (name, keys) => req('DELETE', `/api/masters/${name}`, { keys }),
  items: () => req('GET', '/api/items'),
  findItem: (term) => req('GET', `/api/items/find${qs({ term })}`),

  sales: (q = {}) => req('GET', `/api/sales${qs(q)}`),
  sale: (trnno) => req('GET', `/api/sales/${encodeURIComponent(trnno)}`),
  createSale: (sale) => req('POST', '/api/sales', sale),
  deleteSale: (trnno) => req('DELETE', `/api/sales/${encodeURIComponent(trnno)}`),
  dayClose: (from, to) => req('POST', '/api/day-close', { from, to }),
  salesSummary: (q = {}) => req('GET', `/api/sales-summary${qs(q)}`),

  purchases: (q = {}) => req('GET', `/api/purchases${qs(q)}`),
  purchase: (trnno) => req('GET', `/api/purchases/${trnno}`),
  createPurchase: (p) => req('POST', '/api/purchases', p),

  stock: (q = {}) => req('GET', `/api/stock${qs(q)}`),
  addOpeningStock: (o) => req('POST', '/api/opening-stock', o),
  stockTransfer: (t) => req('POST', '/api/stock-transfer', t),
  breakage: (b) => req('POST', '/api/breakage', b),

  vouchers: (type) => req('GET', `/api/vouchers/${type}`),
  createVoucher: (v) => req('POST', '/api/vouchers', v),
  ledger: (ledcode) => req('GET', `/api/ledger/${ledcode}`),

  reports: () => req('GET', '/api/reports'),
  report: (key, q = {}) => req('GET', `/api/reports/${key}${qs(q)}`),
  dashboard: () => req('GET', '/api/dashboard'),

  reportGroups: () => req('GET', '/api/report-groups'),
  catalogReport: (key, q = {}) => req('GET', `/api/report-catalog/${key}${qs(q)}`),

  parameters: () => req('GET', '/api/admin/parameters'),
  updateParameters: (v) => req('PUT', '/api/admin/parameters', v),
  years: () => req('GET', '/api/admin/years'),
  createNewYear: (y) => req('POST', '/api/admin/new-year', y),
  exciseAudit: () => req('GET', '/api/admin/excise-audit'),
  deletedBills: () => req('GET', '/api/admin/deleted-bills'),

  transferSales: () => req('GET', '/api/transfer-sales'),
  createTransferSale: (t) => req('POST', '/api/transfer-sales', t),
  transferReceives: () => req('GET', '/api/transfer-receives'),
  createTransferReceive: (t) => req('POST', '/api/transfer-receives', t),
  notes: (type) => req('GET', `/api/notes${type ? `?type=${type}` : ''}`),
  createNote: (n) => req('POST', '/api/notes', n),
  onAccount: (a) => req('POST', '/api/on-account', a),
  accountReplace: (a) => req('POST', '/api/account-replace', a),
  fines: () => req('GET', '/api/fines'),
  createFine: (f) => req('POST', '/api/fines', f),
  reconciliation: (ledcode) => req('GET', `/api/reconciliation${ledcode ? `?ledcode=${ledcode}` : ''}`),
  addReconciliation: (r) => req('POST', '/api/reconciliation', r),
  clearReconciliation: (slno, cleareddate) => req('POST', `/api/reconciliation/${slno}/clear`, { cleareddate }),
  upi: () => req('GET', '/api/upi'),
  recordUpi: (u) => req('POST', '/api/upi', u),
  deliveries: () => req('GET', '/api/deliveries'),
  createDelivery: (d) => req('POST', '/api/deliveries', d),
  setDeliveryStatus: (orderno, status) => req('PUT', `/api/deliveries/${encodeURIComponent(orderno)}`, { status }),

  calc: (a, op, b) => req('POST', '/api/tools/calc', { a, op, b }),
  backup: (dest) => req('POST', '/api/tools/backup', { dest }),
  tally: (kind) => req('GET', `/api/tools/tally?kind=${kind || 'SALES'}`),
  messages: () => req('GET', '/api/tools/messages'),
  sendMessage: (m) => req('POST', '/api/tools/messages', m),
  syncStatus: () => req('GET', '/api/tools/sync-status'),
  sync: (entity, direction) => req('POST', '/api/tools/sync', { entity, direction }),
  pml: () => req('GET', '/api/tools/pml'),
  addPml: (p) => req('POST', '/api/tools/pml', p),
  cleanPrint: () => req('POST', '/api/tools/clean-print'),

  tallyConfig: () => req('GET', '/api/tally/config'),
  saveTallyConfig: (c) => req('PUT', '/api/tally/config', c),
  newTallyToken: () => req('POST', '/api/tally/token'),
  tallyQueue: (status) => req('GET', `/api/tally/queue${status ? `?status=${status}` : ''}`),
  tallySummary: () => req('GET', '/api/tally/summary'),
  tallyEnqueue: () => req('POST', '/api/tally/enqueue'),
  tallyRetry: () => req('POST', '/api/tally/retry'),
  tallyXml: (ids) => req('GET', `/api/tally/xml${ids ? `?ids=${ids}` : ''}`),
  tallyReport: () => req('GET', '/api/tally/report'),
  tallyLogs: () => req('GET', '/api/tally/logs'),
  tallyMapping: (type) => req('GET', `/api/tally/mapping${type ? `?type=${type}` : ''}`),
  addTallyMapping: (m) => req('POST', '/api/tally/mapping', m),
  deleteTallyMapping: (id) => req('DELETE', `/api/tally/mapping/${id}`),
  tallyInbound: (status) => req('GET', `/api/tally/inbound${status ? `?status=${status}` : ''}`),
  applyTallyInbound: (id) => req('POST', '/api/tally/inbound/apply', { id }),
};

export default api;
