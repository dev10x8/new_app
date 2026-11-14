import { api } from './api.js';
import { h, table, form, field, input, select, money, num, today, kpi, delta } from './ui.js';
import { groupOfRoute, isGroupRoute } from './menu.js';
import { groupIcon, navIcon, svgIcon } from './icons.js';

// Primary-key columns per master (mirrors backend/src/services/masters.js).
const MASTER_PK = {
  company: ['companycode'],
  category: ['companycode', 'categorycode'],
  group: ['companycode', 'groupcode'],
  kfl: ['companycode', 'kflcode'],
  strength: ['companycode', 'strengthname'],
  itemgroupml: ['companycode', 'packing', 'ml'],
  storage: ['companycode', 'shopcode'],
  ledger: ['companycode', 'ledcode'],
  acgroup: ['acgroupcode'],
  acount: ['companycode', 'accode'],
  item: ['companycode', 'itemcode'],
  itemrate: ['companycode', 'itemcode', 'ratecode'],
  rateinfo: ['companycode', 'ratecode'],
  year: ['companycode', 'yearcode'],
  counter: ['companycode', 'countercode'],
  customer: ['companycode', 'custcode'],
  area: ['companycode', 'areacode'],
  manufacturer: ['companycode', 'manufcode'],
  naiunit: ['companycode', 'unitcode'],
  naigroup: ['companycode', 'groupcode'],
  naisubgroup: ['companycode', 'groupcode', 'subgroupcode'],
  naiitem: ['companycode', 'itemcode'],
  scheme: ['companycode', 'schemecode'],
  schemedetail: ['companycode', 'schemecode', 'itemcode'],
  offer: ['companycode', 'offercode'],
  taxscheme: ['companycode', 'SchemeCode', 'TaxCode'],
  exciseitem: ['companycode', 'itemcode'],
  excisemap: ['companycode', 'itemcode', 'exciseitemcode'],
  reporttagmain: ['companycode', 'reportName'],
  reporttagdetail: ['companycode', 'reportName'],
};

const humanize = (k) => String(k).replace(/([a-z])([A-Z])/g, '$1 $2').replace(/_/g, ' ').replace(/\b\w/g, (c) => c.toUpperCase());
const pkOf = (name, rows) => MASTER_PK[name] || (rows?.[0] ? Object.keys(rows[0]).filter((k) => /code|name/i.test(k)).slice(0, 2) : ['companycode']);

const formValues = (f) => Object.fromEntries(
  [...f.querySelectorAll('input,select,textarea')]
    .filter((i) => !i.closest('.lines'))
    .map((i) => [i.name, i.value]));


// --------------------------------------------------------------- login/dash
async function dashboard(ctx) {
  const d = await api.dashboard();
  const s = d.todaySession || {};
  const series = (d.salesSeries || []).map((r) => r.net);
  const trend = delta(d.yesterdaySales?.net, d.todaySales?.net);

  // ---- Navigation header (legacy frmDashboardShow top strip) --------------
  // The modules staff reach for most often, kept on the landing screen so a
  // task like a stock adjustment or a lookup never needs a trip to the sidebar.
  const navModules = [
    { label: 'Reports', route: 'reports:sales' },
    { label: 'Purchase', route: 'purchase' },
    { label: 'Stock Transfer', route: 'stock-transfer' },
    { label: 'ePOS', route: 'upi' },
    { label: 'Item Master', route: 'items' },
  ];
  const navHeader = h('div', { class: 'dash-nav' },
    h('span', { class: 'dash-nav-brand' }, 'Business Dashboard'),
    h('div', { class: 'dash-nav-links' },
      ...navModules.map((m) => h('button', { class: 'dash-nav-link', onclick: () => ctx.navigate(m.route) }, m.label))));

  // ---- Session sidebar (left) — real-time till metrics --------------------
  const metric = (label, value, opts = {}) => h('div', { class: `dash-metric${opts.strong ? ' strong' : ''}` },
    h('span', { class: 'dash-metric-k' }, label),
    h('span', { class: 'dash-metric-v' }, opts.money ? money(value) : num(value)));
  const sidebar = h('aside', { class: 'dash-side' },
    h('div', { class: 'card dash-side-card' },
      h('h3', {}, 'Today’s session'),
      h('p', { class: 'muted small' }, s.date || d.today || ''),
      metric('Bottles sold', s.bottles || 0, { strong: true }),
      metric('Cash collection', s.cash || 0, { money: true }),
      metric('Card collection', s.card || 0, { money: true }),
      metric('Credit (party)', s.credit || 0, { money: true }),
      metric('Sales returns', s.returns || 0, { money: true }),
      h('div', { class: 'dash-side-foot' },
        metric('Bills today', s.bills || 0),
        metric('Net sales', s.net || 0, { money: true }))));

  // ---- Action / control panel (legacy bt_cash / bt_card / bt_return / bt_expense)
  const actionPanel = h('div', { class: 'card dash-actions' },
    h('h3', {}, 'Quick till actions'),
    h('div', { class: 'dash-action-grid' },
      h('button', { class: 'dash-action cash', onclick: () => ctx.navigate('sale', { mode: 'CASH' }) }, 'Cash Sale'),
      h('button', { class: 'dash-action card', onclick: () => ctx.navigate('sale', { mode: 'CREDIT CARD' }) }, 'Credit Card'),
      h('button', { class: 'dash-action return', onclick: () => ctx.navigate('sale', { mode: 'RETURN' }) }, 'Sales Return'),
      h('button', { class: 'dash-action expense', onclick: () => ctx.openModal('Expenses', expenseForm(ctx, ctx.state.params || {})) }, 'Log Expense')));

  // ---- Central data grid — the primary workspace --------------------------
  const central = h('div', { class: 'dash-center' },
    h('div', { class: 'grid cols-4' },
      kpi({ label: "Today's Sales", value: d.todaySales?.net, money: true, trend, series }),
      kpi({ label: 'Bills Today', value: d.todaySales?.bills }),
      kpi({ label: 'Stock Value', value: d.stockValue, money: true }),
      kpi({ label: 'Low Stock', value: (d.lowStock || []).length })),
    actionPanel,
    h('div', { class: 'section-title' }, 'Recent bills'),
    table([
      { key: 'trnno', title: 'Bill' },
      { key: 'trndate', title: 'Date' },
      { key: 'party', title: 'Party' },
      { key: 'netamount', title: 'Net', num: true },
    ], d.recentBills || []));

  return h('div', {},
    navHeader,
    h('div', { class: 'dash-layout' }, sidebar, central),
    h('div', { class: 'section-title' }, 'Sales — last 7 days'),
    h('div', { class: 'grid cols-4' },
      kpi({ label: 'Sales Net (YTD)', value: d.sales.net, money: true }),
      kpi({ label: 'Sales Bills (YTD)', value: d.sales.bills }),
      kpi({ label: 'Purchase Net (YTD)', value: d.purchase.net, money: true }),
      kpi({ label: 'Stock Lines', value: d.stockLines })),
    h('div', { class: 'grid cols-2' },
      h('div', { class: 'card' }, h('h3', {}, 'Quick actions'),
        h('div', { class: 'list-menu' },
          h('button', { onclick: () => ctx.navigate('sale') }, '＋ New Sale'),
          h('button', { onclick: () => ctx.navigate('reports:sales') }, 'Sales Reports'),
          h('button', { onclick: () => ctx.navigate('items') }, 'Item Master'))),
      h('div', { class: 'card' },
        h('h3', {}, 'Low stock'),
        table([
          { key: 'itemname', title: 'Item' },
          { key: 'qty', title: 'Qty', num: true },
          { key: 'status', title: 'Status', badge: (r) => (r.status === 'OUT' ? 'danger' : 'warn') },
        ], d.lowStock || []))));
}

async function systemAdmin(ctx) {
  const info = await api.systemInfo();
  const entries = Object.entries(info).map(([k, v]) => h('tr', {}, h('td', {}, humanize(k)), h('td', {}, String(v == null ? '—' : v))));
  return h('div', { class: 'card' },
    h('h3', {}, 'System Information'),
    h('div', { class: 'table-wrap' }, h('table', {}, h('tbody', {}, ...entries))));
}

// --------------------------------------------------------------- masters
async function masterScreen(ctx, name) {
  const rows = await api.masters(name);
  const pk = pkOf(name, rows);
  const cols = rows.length ? Object.keys(rows[0]).slice(0, 9) : [];
  const columns = cols.map((c) => ({ key: c, title: humanize(c), num: /amount|rate|ml|packing|qnty/i.test(c) }));

  const addBtn = h('button', { class: 'btn primary' }, '＋ Add');
  addBtn.onclick = () => openMasterForm(ctx, name, rows, null);

  const wrap = h('div', {},
    h('div', { class: 'toolbar' }, addBtn, h('span', { class: 'muted small' }, `${rows.length} record(s)`)),
    rows.length
      ? withRowActions(ctx, name, rows, pk, columns)
      : h('div', { class: 'table-wrap' }, h('div', { class: 'empty' }, 'No records. Use “Add”.'))
  );
  return wrap;
}

function withRowActions(ctx, name, rows, pk, columns) {
  const t = table(columns, rows);
  const tbody = t.querySelector('tbody');
  [...tbody.rows].forEach((tr, i) => {
    const td = h('td', {},
      h('button', { class: 'btn small', onclick: () => openMasterForm(ctx, name, rows, rows[i]) }, 'Edit'),
      ' ',
      h('button', {
        class: 'btn small danger',
        onclick: async () => {
          if (!confirm('Delete this record?')) return;
          const keys = Object.fromEntries(pk.map((c) => [c, rows[i][c]]));
          try { await api.deleteMaster(name, keys); ctx.toast('Deleted', 'ok'); ctx.navigate(ctx.state.route); }
          catch (e) { ctx.toast(e.message, 'error'); }
        },
      }, 'Del'));
    tr.append(td);
  });
  const head = t.querySelector('thead tr');
  head.append(h('th', {}, ''));
  return t;
}

function openMasterForm(ctx, name, rows, existing) {
  const keys = rows.length ? Object.keys(rows[0]) : (existing ? Object.keys(existing) : ['companycode', 'code', 'name']);
  const fields = keys.map((k) => field(humanize(k), input(k, existing?.[k] ?? (k === 'companycode' ? '1' : ''))));
  const f = h('form', { class: 'form' }, ...fields.map((x) => h('div', { class: 'row' }, x)));
  f.append(h('div', { class: 'actions' },
    h('button', { class: 'btn primary', type: 'submit' }, existing ? 'Update' : 'Create')));
  f.addEventListener('submit', async (e) => {
    e.preventDefault();
    const values = formValues(f);
    try {
      if (existing) {
        const pk = pkOf(name, rows);
        const keyVals = Object.fromEntries(pk.map((c) => [c, existing[c]]));
        await api.updateMaster(name, keyVals, values);
      } else {
        await api.createMaster(name, values);
      }
      ctx.closeModal();
      ctx.toast('Saved', 'ok');
      ctx.navigate(ctx.state.route);
    } catch (ex) { ctx.toast(ex.message, 'error'); }
  });
  ctx.openModal(`${existing ? 'Edit' : 'Add'} · ${humanize(name)}`, f);
}

async function itemsScreen(ctx) {
  const rows = await api.items();
  const addBtn = h('button', { class: 'btn primary' }, '＋ Add Item');
  addBtn.onclick = () => openMasterForm(ctx, 'item', [], null);
  const t = table([
    { key: 'itemcode', title: 'Code' },
    { key: 'itemname', title: 'Name' },
    { key: 'categoryname', title: 'Category' },
    { key: 'kflname', title: 'KFL' },
    { key: 'ml', title: 'ML', num: true },
    { key: 'packing', title: 'Pack', num: true },
    { key: 'salesrate', title: 'Sale', money: true, num: true },
    { key: 'purchaseRate', title: 'Purch', money: true, num: true },
    { key: 'barcode', title: 'Barcode' },
  ], rows);
  return h('div', {},
    h('div', { class: 'toolbar' }, addBtn, h('span', { class: 'muted small' }, `${rows.length} item(s)`)),
    t);
}

async function schemesScreen(ctx) {
  const [sch, offers] = await Promise.all([api.masters('scheme'), api.masters('offer')]);
  return h('div', {},
    h('div', { class: 'section-title' }, 'Schemes'),
    table([{ key: 'schemecode', title: 'Code' }, { key: 'schemename', title: 'Name' }], sch),
    h('div', { class: 'section-title' }, 'Offers'),
    table([{ key: 'offercode', title: 'Code' }, { key: 'offername', title: 'Name' }], offers));
}

// --------------------------------------------------------------- inventory
const STOCK_BADGE = { OK: 'ok', LOW: 'warn', OUT: 'error' };

async function inventoryScreen(ctx) {
  const [summary, low, reqs] = await Promise.all([
    api.inventorySummary(), api.inventoryLowStock(), api.requisitions(),
  ]);

  const stat = (label, value, kind = '') => h('div', { class: 'card stat' },
    h('div', { class: 'k' }, label), h('div', { class: `v${kind ? ` ${kind}` : ''}` }, String(value)));
  const counters = h('div', { class: 'grid cols-4' },
    stat('Total Items', summary.totalItems),
    stat('Low Stock', summary.lowStock, summary.lowStock ? 'warn' : ''),
    stat('Out of Stock', summary.outOfStock, summary.outOfStock ? 'danger' : ''),
    stat('In Stock', summary.inStock),
    stat('Stock Units', summary.stockUnits),
    stat('Stock Value', money(summary.stockValue), 'money'),
    stat('Stores', summary.stores),
    stat('Open Requisitions', summary.openRequisitions, summary.openRequisitions ? 'warn' : ''));

  const addBtn = h('button', { class: 'btn primary' }, '＋ Add Item');
  addBtn.onclick = () => openMasterForm(ctx, 'item', [], null);

  const genBtn = h('button', { class: 'btn' }, 'Auto-raise requisitions');
  genBtn.onclick = async () => {
    try {
      const r = await api.generateRequisitions();
      ctx.toast(`${r.created} requisition(s) created, ${r.skipped} already open`, 'ok');
      ctx.navigate(ctx.state.route);
    } catch (e) { ctx.toast(e.message, 'error'); }
  };
  const refreshBtn = h('button', { class: 'btn' }, 'Refresh');
  refreshBtn.onclick = () => ctx.navigate(ctx.state.route);

  const lowTable = table([
    { key: 'itemcode', title: 'Code' }, { key: 'itemname', title: 'Item' },
    { key: 'categoryname', title: 'Category' }, { key: 'ml', title: 'ML', num: true },
    { key: 'qty', title: 'On hand', num: true }, { key: 'minlevel', title: 'Min', num: true },
    { key: 'reorderqty', title: 'Reorder', num: true },
    { key: 'status', title: 'Status', badge: (r) => STOCK_BADGE[r.status] || '' },
  ], low);

  const reqTable = table([
    { key: 'id', title: '#' }, { key: 'itemcode', title: 'Code' }, { key: 'itemname', title: 'Item' },
    { key: 'currentqty', title: 'On hand', num: true }, { key: 'minlevel', title: 'Min', num: true },
    { key: 'reorderqty', title: 'Reorder', num: true }, { key: 'rate', title: 'Rate', money: true },
    { key: 'status', title: 'Status', badge: (r) => ({ DRAFT: 'warn', ORDERED: 'ok', CANCELLED: '' }[r.status] || '') },
  ], reqs);
  [...(reqTable.querySelector('tbody')?.rows || [])].forEach((tr, i) => {
    const row = reqs[i];
    tr.append(h('td', {}, row.status !== 'DRAFT' ? '—' : h('button', { class: 'btn small', onclick: async () => {
      try { await api.updateRequisition(row.id, { status: 'ORDERED' }); ctx.toast('Marked as ordered', 'ok'); ctx.navigate(ctx.state.route); }
      catch (e) { ctx.toast(e.message, 'error'); }
    } }, 'Mark ordered')));
  });
  if (reqs.length) reqTable.querySelector('thead tr').append(h('th', {}, ''));

  return h('div', {},
    h('div', { class: 'card' },
      h('h3', {}, 'Inventory'),
      h('p', { class: 'muted' }, 'Live stock position across every store, with low-stock alerting against each item’s reorder level (itemmst.min_qnty, or the DefaultMinQty parameter).')),
    h('div', { class: 'card' },
      h('div', { class: 'section-title' }, 'Overview'),
      counters,
      h('div', { class: 'toolbar' }, addBtn, genBtn, refreshBtn)),
    h('div', { class: 'card' },
      h('div', { class: 'section-title' }, 'Low stock / out of stock'),
      lowTable),
    h('div', { class: 'card' },
      h('div', { class: 'section-title' }, 'Purchase requisitions'),
      reqTable));
}

// --------------------------------------------------------------- admin
async function paramsScreen(ctx) {
  const p = await api.parameters();
  const skip = new Set(['companycode', 'nav_mode']);
  const keys = Object.keys(p).filter((k) => !skip.has(k));
  const fields = keys.map((k) => h('div', { class: 'row' }, field(humanize(k), input(k, p[k] ?? ''))));
  const f = h('form', { class: 'form' }, ...fields);
  f.append(h('div', { class: 'actions' }, h('button', { class: 'btn primary', type: 'submit' }, 'Save Parameters')));
  f.addEventListener('submit', async (e) => {
    e.preventDefault();
    const data = formValues(f);
    try { await api.updateParameters(data); ctx.toast('Parameters saved', 'ok'); }
    catch (ex) { ctx.toast(ex.message, 'error'); }
  });

  // Navigation mode is a first-class choice (see docs/UI_NAV_DESIGN.md), not a
  // raw text field: it drives how the sidebar reveals submenus.
  const navMode = h('select', { name: 'nav_mode', class: 'nav-mode-select' },
    ...[['expand', 'Expand in sidebar — click a group to show its items inline'],
      ['cards', 'Cards in main panel — click a group to open its items as boxes']]
      .map(([v, label]) => {
        const o = h('option', { value: v }, label);
        if ((p.nav_mode || 'expand') === v) o.selected = true;
        return o;
      }));
  navMode.addEventListener('change', async () => {
    try {
      await api.updateParameters({ nav_mode: navMode.value });
      ctx.applyNavMode(navMode.value);
      ctx.toast(`Navigation set to ${navMode.value === 'cards' ? 'cards' : 'expand'} menu`, 'ok');
    } catch (ex) { ctx.toast(ex.message, 'error'); }
  });
  const navCard = h('div', { class: 'card' },
    h('h3', {}, 'Navigation'),
    h('div', { class: 'row' }, field('Sidebar menu style', navMode)),
    h('p', { class: 'muted small' }, 'Saved immediately for all users of this company.'));

  return h('div', {}, navCard, h('div', { class: 'card' }, h('h3', {}, 'Parameters'), f));
}

async function usersScreen(ctx) {
  const rows = await api.users();
  const addBtn = h('button', { class: 'btn primary' }, '＋ Add User');
  addBtn.onclick = () => {
    const f = form([
      [['User Code', input('usercode', '')], ['Name', input('username', '')]],
      [['Access Level', select('accesslevel', ['User', 'Admin'], 'User')], ['PIN', h('input', { name: 'pin', type: 'password', value: '' })]],
    ], async (d) => { try { await api.createUser(d); ctx.closeModal(); ctx.toast('User created', 'ok'); ctx.navigate(ctx.state.route); } catch (e) { ctx.toast(e.message, 'error'); } });
    ctx.openModal('Add User', f);
  };
  const t = table([
    { key: 'usercode', title: 'Code' },
    { key: 'username', title: 'Name' },
    { key: 'accesslevel', title: 'Level', badge: (r) => (r.accesslevel === 'Admin' ? 'ok' : '') },
  ], rows);
  [...t.querySelector('tbody').rows].forEach((tr, i) => {
    tr.append(h('td', {},
      h('button', { class: 'btn small', onclick: () => resetPin(ctx, rows[i]) }, 'Reset PIN'),
      ' ',
      h('button', { class: 'btn small', onclick: () => rightsFor(ctx, rows[i]) }, 'Rights')));
  });
  t.querySelector('thead tr').append(h('th', {}, ''));
  return h('div', {}, h('div', { class: 'toolbar' }, addBtn), t);
}

function resetPin(ctx, user) {
  const f = form([[['New PIN', h('input', { name: 'pin', type: 'password' })]],
    [['Confirm PIN', h('input', { name: 'pin2', type: 'password' })]]],
    async (d) => {
      if (!d.pin || d.pin !== d.pin2) return ctx.toast('PINs do not match', 'error');
      try { await api.resetPin(user.usercode, d.pin); ctx.closeModal(); ctx.toast('PIN updated', 'ok'); }
      catch (e) { ctx.toast(e.message, 'error'); }
    });
  ctx.openModal(`Reset PIN · ${user.username}`, f);
}

async function rightsFor(ctx, user) {
  const modules = ['MASTERS', 'TRANSACTIONS', 'REPORTS', 'ADMIN', 'TOOLS'];
  const body = h('div', { class: 'form' });
  for (const m of modules) {
    body.append(h('div', { class: 'row three' },
      field(`${m} · View`, select(`${m}_v`, ['Y', 'N'], 'Y')),
      field(`${m} · Add`, select(`${m}_a`, ['Y', 'N'], 'Y')),
      field(`${m} · Delete`, select(`${m}_d`, ['Y', 'N'], 'N'))));
  }
  const btn = h('button', { class: 'btn primary' }, 'Save Rights');
  btn.onclick = async () => {
    try {
      for (const m of modules) {
        await api.setRights(user.usercode, m, {
          view: document.querySelector(`[name="${m}_v"]`).value,
          add: document.querySelector(`[name="${m}_a"]`).value,
          delete: document.querySelector(`[name="${m}_d"]`).value,
        });
      }
      ctx.closeModal(); ctx.toast('Rights saved', 'ok');
    } catch (e) { ctx.toast(e.message, 'error'); }
  };
  body.append(h('div', { class: 'actions' }, btn));
  ctx.openModal(`User Rights · ${user.username}`, body);
}

async function newYearScreen(ctx) {
  const years = await api.years();
  // Next financial year (Apr 1 - Mar 31) derived from the server date.
  const [y, m] = today().split('-').map(Number);
  const startYear = (m >= 4 ? y : y - 1) + 1;
  const f = form([
    [['Year Range', input('yearrange', `${startYear}-${startYear + 1}`)], ['Start Date', input('startdate', `${startYear}-04-01`, 'date')]],
    [['End Date', input('enddate', `${startYear + 1}-03-31`, 'date')], ['Year Code', input('yearcode', String(startYear))]],
  ], async (d) => {
    try { await api.createNewYear(d); ctx.toast('New year created', 'ok'); ctx.navigate(ctx.state.route); }
    catch (e) { ctx.toast(e.message, 'error'); }
  });
  return h('div', {},
    h('div', { class: 'card' }, h('h3', {}, 'Create New Year'), f),
    h('div', { class: 'section-title' }, 'Existing Years'),
    table([{ key: 'yearcode', title: 'Code' }, { key: 'yearrange', title: 'Range' }, { key: 'startdate', title: 'From' }, { key: 'enddate', title: 'To' }], years));
}

async function exciseAuditScreen(ctx) {
  const rows = await api.exciseAudit();
  const keys = rows.length ? Object.keys(rows[0]) : [];
  return h('div', { class: 'card' }, h('h3', {}, 'Excise Audit'),
    table(keys.map((k) => ({ key: k, title: humanize(k) })), rows));
}

async function deletedBillsScreen(ctx) {
  const rows = await api.deletedBills();
  return h('div', { class: 'card' }, h('h3', {}, 'Deleted Bills'),
    table([{ key: 'trnno', title: 'Bill No' }, { key: 'trndate', title: 'Date' }, { key: 'deletedby', title: 'Deleted By' }, { key: 'deletedat', title: 'Deleted At' }], rows));
}

// --------------------------------------------------------------- reports
async function reportsGroup(ctx, group) {
  const groups = await api.reportGroups();
  const list = groups[group] || [];
  return h('div', {},
    h('div', { class: 'grid cols-3' },
      ...Object.keys(groups).map((g) => h('button', {
        class: `btn${g === group ? ' primary' : ''}`,
        onclick: () => ctx.navigate(`reports:${g}`),
      }, `${humanize(g)} (${groups[g].length})`))),
    h('div', { class: 'section-title' }, `${humanize(group)} reports`),
    h('div', { class: 'list-menu' },
      ...list.map((r) => h('button', { onclick: () => runReport(ctx, group, r) }, r.title))));
}

async function runReport(ctx, group, r) {
  const { setPage } = ctx;
  setPage(h('div', { class: 'loading' }, h('span', { class: 'spinner' }), 'Running report…'));
  try {
    const rows = await api.catalogReport(r.key);
    const keys = rows.length ? Object.keys(rows[0]) : [];
    const node = h('div', {},
      h('div', { class: 'toolbar' },
        h('button', { class: 'btn', onclick: () => ctx.navigate(`reports:${group}`) }, '← Back'),
        h('span', { class: 'muted small' }, `${rows.length} row(s)`)),
      table(keys.map((k) => ({ key: k, title: humanize(k), num: /amount|amt|rate|qnty|qty|ml|value|net|gross|tax/i.test(k) })), rows));
    setPage(node);
  } catch (e) {
    setPage(h('div', { class: 'card' }, h('h3', {}, r.title), h('p', { class: 'error' }, e.message),
      h('div', { class: 'actions' }, h('button', { class: 'btn', onclick: () => ctx.navigate(`reports:${group}`) }, '← Back'))));
  }
}

// --------------------------------------------------------------- repeater
function repeater(fields, initial = []) {
  const wrap = h('div', { class: 'lines' });
  const rows = [];
  function addRow(vals = {}) {
    const rowEl = h('div', { class: 'line-row' });
    for (const fd of fields) rowEl.append(field(fd.label, fd.options ? select(fd.name, fd.options, vals[fd.name]) : input(fd.name, vals[fd.name] ?? fd.value ?? '', fd.type || 'text')));
    const rm = h('button', { class: 'btn small danger', type: 'button' }, '✕');
    rm.onclick = () => { rows.splice(rows.indexOf(rowEl), 1); rowEl.remove(); };
    rowEl.append(h('div', { class: 'line-rm' }, rm));
    rows.push(rowEl);
    wrap.append(rowEl);
  }
  for (const v of initial) addRow(v);
  const addBtn = h('button', { class: 'btn small', type: 'button' }, '＋ Add line');
  addBtn.onclick = () => addRow();
  const container = h('div', {}, wrap, h('div', { class: 'toolbar' }, addBtn));
  const values = () => rows
    .map((r) => Object.fromEntries([...r.querySelectorAll('input,select')].map((i) => [i.name, i.value])))
    .filter((o) => Object.values(o).some((v) => v !== ''));
  return { node: container, addRow, values, rows };
}

// --------------------------------------------------------------- POS sale
// Rebuilt to mirror the legacy frm_countersales "Counter Sale" form: a dense,
// keyboard-first till with a scannable item grid, live totals
// (gross / discount / surcharge / rounding / net / tendered), sale-type action
// buttons and a party-details panel. Backed by the same createSale API, so the
// double-entry, GST split, excise capture and Tally sync all still happen.
const POS_ACTIONS = [
  { key: 'cash', label: 'Cash\nSale', type: 'CASH' },
  { key: 'party', label: 'Party\nSale', type: 'CREDIT ACCOUNT' },
  { key: 'creditcard', label: 'Credit\nCard', type: 'CREDIT CARD' },
  { key: 'return', label: 'Sales\nReturn', type: 'RETURN' },
];

async function posSale(ctx) {
  const [items, ledgers, stores, rateinfos, params] = await Promise.all([
    api.items(),
    api.masters('ledger').catch(() => []),
    api.masters('storage').catch(() => []),
    api.masters('rateinfo').catch(() => []),
    api.parameters().catch(() => ({})),
  ]);
  const byCode = new Map(items.map((i) => [String(i.itemcode).toUpperCase(), i]));
  const byBarcode = new Map(items.filter((i) => i.barcode).map((i) => [String(i.barcode).toUpperCase(), i]));

  const st = {
    salestype: ctx.state.params?.mode || 'CASH',
    lines: [],
    store: stores.find((s) => s.ShopType === 'POS')?.shopcode || stores[0]?.shopcode || 'CNT1',
    ratecode: params.ratecode || rateinfos[0]?.ratecode || 'GENERAL',
    discRate: 0, discount: 0,
    surchargeRate: Number(params.surcharge_percent) || 0, surcharge: 0,
    roundoff: 0, tendered: 0, gross: 0, net: 0,
  };

  const ledgerRowsFor = (type) => {
    const t = String(type || '').toUpperCase();
    let rows = ledgers;
    if (t === 'CASH') rows = ledgers.filter((l) => /cash/i.test(l.type || '') || l.ledcode === params.cashparty);
    else if (t === 'CREDIT CARD') rows = ledgers.filter((l) => /bank|upi/i.test(l.type || '') || l.ledcode === params.bankacc);
    else if (t === 'CREDIT ACCOUNT') rows = ledgers.filter((l) => /party|customer|debtor/i.test(l.type || ''));
    return rows.length ? rows : ledgers;
  };

  // ---- live totals (mirrors txt_gross / txt_discount / txt_nett / txt_rnd) --
  const fGross = roField('Gross Amount');
  const fDiscRate = numField('Disc %', '0', () => { st.discRate = Number(fDiscRate.input.value) || 0; if (st.discRate) st.discount = 0; recalc(); });
  const fDiscount = numField('Discount', '0', () => { st.discount = Number(fDiscount.input.value) || 0; if (st.discount) st.discRate = 0; recalc(); });
  const fReason = h('input', { name: 'discountReason', placeholder: 'Discount reason' });
  const fSurchRate = numField('Surcharge %', String(st.surchargeRate || 0), () => { st.surchargeRate = Number(fSurchRate.input.value) || 0; recalc(); });
  const fSurcharge = roField('Surcharge');
  const fRound = roField('Rounding Off');
  const fNet = roField('Net Amount', 'net');
  const fTender = numField('Tendered Amount', '0', () => { st.tendered = Number(fTender.input.value) || 0; recalc(); });
  const fChange = roField('Change');
  const fParty = select('ledgercode', ledgerRowsFor('CASH').map((l) => ({ value: l.ledcode, label: l.name })));
  const fPhone = h('input', { name: 'phone', readonly: true });
  const fOpening = roField('Opening (Dr)');
  const fPrevDue = roField('Previous Due');
  const fNettDue = roField('Nett Due');
  const fBillNo = h('input', { name: 'billNo', readonly: true, placeholder: 'auto' });
  const fDate = input('trndate', today(), 'date');
  const fStore = select('storecode', stores.map((s) => ({ value: s.shopcode, label: s.shopname })), st.store);
  const fRate = select('ratecode', (rateinfos.length ? rateinfos : [{ ratecode: 'GENERAL', ratename: 'GENERAL' }]).map((r) => ({ value: r.ratecode, label: r.ratename })), st.ratecode);
  const fBulk = h('input', { type: 'checkbox' });

  // ---- item grid ---------------------------------------------------------
  const tbody = h('tbody');
  const emptyRow = () => h('tr', { class: 'pos-empty' }, h('td', { colspan: 11 }, 'Scan a barcode or search an item to start the bill.'));
  const grid = h('table', { class: 'pos-grid' },
    h('thead', {}, h('tr', {},
      ...['Barcode', 'Item Name', 'Batch', 'Rate', 'MRP', 'Qty', 'Disc %', 'Disc Amt', 'Amount', 'ML', ''].map((t) => h('th', {}, t)))),
    tbody);

  function addLine(item, qty = 1) {
    if (!item) return;
    if (st.lines.length === 0) tbody.replaceChildren();
    const line = {
      itemcode: item.itemcode, itemname: item.itemname, barcode: item.barcode || '',
      batchNo: '0', rate: Number(item.salesrate) || 0, mrp: Number(item.salesrate) || 0,
      qnty: qty, discp: 0, discAmt: 0, amount: 0, ml: item.ml || 0,
    };
    st.lines.push(line);
    const tr = h('tr', { dataset: { code: item.itemcode } });
    const cell = (child, cls) => h('td', { class: cls || '' }, child);
    const qtyIn = numFieldRaw(String(qty), () => { line.qnty = Number(qtyIn.value) || 0; recalc(); });
    const rateIn = numFieldRaw(String(line.rate), () => { line.rate = Number(rateIn.value) || 0; recalc(); });
    const discIn = numFieldRaw('0', () => { line.discp = Number(discIn.value) || 0; recalc(); });
    const batchIn = h('input', { value: line.batchNo, size: 6, class: 'pos-cell' });
    batchIn.addEventListener('input', () => { line.batchNo = batchIn.value; });
    const mrpIn = h('input', { value: money(line.mrp), readonly: true, size: 7, class: 'pos-cell' });
    const amtCell = h('td', { class: 'num pos-amt' }, '0.00');
    const discAmtCell = h('td', { class: 'num' }, '0.00');
    const rm = h('button', { class: 'btn small danger', type: 'button', title: 'Remove line' }, '✕');
    rm.onclick = () => { st.lines.splice(st.lines.indexOf(line), 1); tr.remove(); if (!st.lines.length) tbody.append(emptyRow()); recalc(); };
    line._render = () => {
      const gross = line.rate * line.qnty;
      line.discAmt = round2(gross * (line.discp || 0) / 100);
      line.amount = round2(gross - line.discAmt);
      amtCell.textContent = money(line.amount);
      discAmtCell.textContent = money(line.discAmt);
    };
    tr.append(
      cell(item.barcode || item.itemcode, 'pos-code'),
      cell(item.itemname),
      cell(batchIn),
      cell(rateIn),
      cell(mrpIn),
      cell(qtyIn), cell(discIn), discAmtCell, amtCell, cell(String(line.ml || ''), 'num'),
      cell(rm),
    );
    tbody.append(tr);
    recalc();
  }

  function recalc() {
    for (const l of st.lines) l._render();
    st.gross = round2(st.lines.reduce((s, l) => s + l.amount, 0));
    if (st.discRate) st.discount = round2(st.gross * st.discRate / 100);
    const surcharge = st.salestype === 'CREDIT CARD' ? round2(st.gross * (st.surchargeRate || 0) / 100) : 0;
    st.surcharge = surcharge;
    const pre = round2(st.gross - (st.discount || 0) + surcharge);
    const rounded = Math.round(pre);
    st.roundoff = round2(rounded - pre);
    st.net = rounded;
    fGross.value = money(st.gross);
    // Don't rewrite the field the user is currently editing (it would fight the
    // keystrokes); update only the counterpart/computed displays.
    const active = document.activeElement;
    if (active !== fDiscount.input) fDiscount.input.value = money(st.discount || 0);
    if (active !== fDiscRate.input) fDiscRate.input.value = String(st.discRate || 0);
    fSurcharge.value = money(surcharge);
    fRound.value = `${st.roundoff >= 0 ? '+' : '-'}${money(Math.abs(st.roundoff))}`;
    fNet.value = money(st.net);
    fChange.value = st.tendered > 0 ? money(st.tendered - st.net) : '0.00';
    const showSurch = st.salestype === 'CREDIT CARD';
    fSurchRate.wrap.hidden = !showSurch;
    fSurcharge.wrap.hidden = !showSurch;
    updateActionButtons();
  }

  // ---- party details -----------------------------------------------------
  async function refreshParty() {
    const ledcode = fParty.value;
    const row = ledgers.find((l) => l.ledcode === ledcode);
    fPhone.value = row?.phone || '';
    try {
      const entries = await api.ledger(ledcode);
      const bal = entries.length ? Number(entries[entries.length - 1].balance) || 0 : 0;
      fPrevDue.value = money(Math.abs(bal));
      fNettDue.value = money(Math.abs(bal) + st.net);
      fOpening.value = money(0);
    } catch { fPrevDue.value = fNettDue.value = '0.00'; }
  }
  fParty.addEventListener('change', () => { refreshParty(); });

  // ---- sale-type buttons (legacy ToolStrip) ------------------------------
  const actionBtns = {};
  const setSaleType = (type) => {
    st.salestype = type;
    fParty.replaceChildren(...ledgerRowsFor(type).map((l) => h('option', { value: l.ledcode }, l.name)));
    if (type === 'CREDIT ACCOUNT') fParty.selectedIndex = Math.max(0, ledgerRowsFor(type).findIndex((l) => /debtor|party|customer/i.test(l.type || '')));
    if (type === 'CREDIT CARD') { fSurchRate.input.value = String(st.surchargeRate || 0); }
    if (type === 'RETURN') { /* return mode: bill qty shown as-is; net posts negative */ }
    recalc();
    refreshParty();
  };
  const actions = h('div', { class: 'pos-actions' },
    ...POS_ACTIONS.map((a) => {
      const b = h('button', { class: `pos-btn pos-${a.key}`, type: 'button' }, ...a.label.split('\n').flatMap((t, i) => (i ? [h('br'), t] : [t])));
      b.onclick = () => setSaleType(a.type);
      actionBtns[a.type] = b;
      return b;
    }),
    h('button', { class: 'pos-btn', type: 'button', onclick: () => ctx.openModal('Expenses', expenseForm(ctx, params)) }, 'Expenses'),
    h('button', { class: 'pos-btn', type: 'button', onclick: () => ctx.navigate('voucher:RECEIPT') }, 'Payment Receipt'),
    h('button', { class: 'pos-btn', type: 'button', onclick: () => ctx.navigate('tally') }, 'XML-Query'),
    h('button', { class: 'pos-btn', type: 'button', onclick: () => ctx.navigate('excise-sync') }, 'Excise'),
    h('div', { class: 'pos-spacer' }),
    h('button', { class: 'btn small', type: 'button', onclick: refreshParty }, 'Show Party Details'),
    h('button', { class: 'btn small primary', type: 'submit', form: 'pos-form' }, 'Save [F8]'),
    h('button', { class: 'btn small', type: 'button', onclick: () => printBill(st.lines) }, 'Print [F9]'),
    h('button', { class: 'btn small danger', type: 'button', onclick: () => ctx.navigate('sale') }, 'Cancel'),
  );
  function updateActionButtons() {
    for (const a of POS_ACTIONS) actionBtns[a.type]?.classList.toggle('active', st.salestype === a.type);
  }

  // ---- item search / scan ------------------------------------------------
  const search = h('input', { class: 'pos-search', placeholder: 'Scan barcode / type item code or name, then Enter', list: 'pos-items', autofocus: true });
  const searchList = h('datalist', { id: 'pos-items' }, ...items.slice(0, 800).map((i) => h('option', { value: i.barcode || i.itemcode }, `${i.itemcode} · ${i.itemname}`)));
  async function commitSearch() {
    const term = search.value.trim();
    if (!term) return;
    let item = byCode.get(term.toUpperCase()) || byBarcode.get(term.toUpperCase());
    if (!item) { try { item = await api.findItem(term); } catch { item = null; } }
    if (!item) { ctx.toast(`No item matches "${term}"`, 'error'); return; }
    addLine(item, 1);
    search.value = '';
    search.focus();
  }
  search.addEventListener('keydown', (e) => { if (e.key === 'Enter') { e.preventDefault(); commitSearch(); } });
  search.addEventListener('change', commitSearch);

  // ---- statements (Current Stocks / Current Sales) -----------------------
  const stmtBody = h('tbody');
  const stmt = h('div', { class: 'pos-stmt' },
    h('div', { class: 'pos-stmt-tabs' },
      h('button', { class: 'tab active', type: 'button', onclick: (e) => showStock(e.target) }, 'Current Stocks Statement'),
      h('button', { class: 'tab', type: 'button', onclick: (e) => showSales(e.target) }, 'Current Sales Statement')),
    h('div', { class: 'table-wrap' }, h('table', {}, h('thead', {}, h('tr', {}, h('th', {}, 'Item'), h('th', { class: 'num' }, 'Stock'))), stmtBody)));
  async function showStock(btn) {
    selectTab(btn);
    stmtBody.replaceChildren(h('tr', {}, h('td', { colspan: 2, class: 'muted' }, 'Loading…')));
    try {
      const rows = (await api.inventoryStock()).filter((r) => !st.store || r.storecode === st.store).slice(0, 60);
      stmtBody.replaceChildren(...(rows.length ? rows.map((r) => h('tr', {}, h('td', {}, r.itemname), h('td', { class: 'num' }, num(r.qty)))) : [h('tr', {}, h('td', { colspan: 2, class: 'muted' }, 'No stock movement today.'))]));
    } catch { stmtBody.replaceChildren(h('tr', {}, h('td', { colspan: 2, class: 'muted' }, 'Stock unavailable.'))); }
  }
  async function showSales(btn) {
    selectTab(btn);
    stmtBody.replaceChildren(h('tr', {}, h('td', { colspan: 2, class: 'muted' }, 'Loading…')));
    try {
      const rows = await api.salesSummary({ from: today(), to: today() });
      const totals = rows.reduce((a, r) => ({ bills: a.bills + Number(r.bills || 0), net: a.net + Number(r.net || 0) }), { bills: 0, net: 0 });
      stmtBody.replaceChildren(
        h('tr', {}, h('td', {}, 'Bills today'), h('td', { class: 'num' }, num(totals.bills))),
        h('tr', {}, h('td', {}, 'Net sales today'), h('td', { class: 'num' }, money(totals.net))),
      );
    } catch { stmtBody.replaceChildren(h('tr', {}, h('td', { colspan: 2, class: 'muted' }, 'Sales summary unavailable.'))); }
  }
  const selectTab = (btn) => { for (const t of stmt.querySelectorAll('.tab')) t.classList.toggle('active', t === btn); };

  // ---- form / save -------------------------------------------------------
  const formEl = h('form', { class: 'pos-form', id: 'pos-form' },
    h('div', { class: 'pos-top' },
      h('div', { class: 'pos-top-row' },
        field('Date', fDate), field('Bill No.', fBillNo),
        field('Store', fStore), field('Rate List', fRate),
        h('label', { class: 'field pos-bulk' }, h('span', {}, 'Bulk Sale'), fBulk)),
      h('div', { class: 'pos-top-row' },
        field('Party Name', fParty), field('Mobile', fPhone), field('Search', h('div', { class: 'pos-searchwrap' }, search, searchList)))),
    h('div', { class: 'pos-main' },
      h('div', { class: 'pos-gridwrap' },
        h('div', { class: 'pos-gridhead' }, 'Items list', h('span', { class: 'muted' }, 'F2 to search · Enter to add')),
        h('div', { class: 'pos-gridscroll' }, grid)),
      h('div', { class: 'pos-side' },
        h('div', { class: 'pos-totals' },
          fGross.wrap, fDiscRate.wrap, fDiscount.wrap, field('Discount Reason', fReason),
          fSurchRate.wrap, fSurcharge.wrap, fRound.wrap, fNet.wrap, fTender.wrap, fChange.wrap),
        h('div', { class: 'pos-party' },
          h('div', { class: 'section-title' }, 'Party Details'),
          field('Opening (Dr)', fOpening.input), field('Previous Due', fPrevDue.input), field('Nett Due', fNettDue.input),
          field('Payment', select('payment', ['Cash', 'Credit Card', 'Credit Account', 'Return'])),
          field('Bank Name', h('input', { name: 'bank' })),
          field('Chq No', h('input', { name: 'chqno' })),
          field('Ref Bill No', h('input', { name: 'refbill' }))),
        stmt)),
    h('div', { class: 'pos-foot' }, actions),
  );

  formEl.addEventListener('submit', async (e) => {
    e.preventDefault();
    if (!st.lines.length) return ctx.toast('Add at least one item', 'error');
    if (st.salestype === 'CREDIT ACCOUNT' && !fParty.value) return ctx.toast('Select a party for a party sale', 'error');
    const lines = st.lines.map((l) => ({ itemcode: l.itemcode, qnty: l.qnty, rate: l.rate, mrp: l.mrp, batchNo: l.batchNo, discp: l.discp }));
    try {
      const r = await api.createSale({
        salestype: st.salestype, storecode: fStore.value, ratecode: fRate.value,
        ledgercode: fParty.value, trndate: fDate.value,
        discount: st.discount || 0, discountReason: fReason.value || undefined,
        surcharge: st.surcharge || 0, roundoff: st.roundoff || 0,
        lines, userId: ctx.state.user.usercode,
      });
      if (r.clock_rolled_back) ctx.toast('Warning: system clock appears to be set back', 'error');
      ctx.openModal('Bill Saved', h('div', { class: 'grid cols-2' },
        statCard('Bill No', r.trnno), statCard('Gross', money(r.amount)),
        statCard('Discount', money(r.discamount)), statCard('Surcharge', money(r.surcharge || 0)),
        statCard('Rounding', money(r.roundoff || 0)), statCard('Net', money(r.netamount)),
        r.date_warning ? statCard('Date', r.date_warning) : null));
      ctx.toast(`Saved ${r.trnno}`, 'ok');
      fBillNo.value = r.trnno;
    } catch (ex) { ctx.toast(ex.message, 'error'); }
  });

  // Keyboard map (legacy: F8 save / F9 print / Esc clear; F2 search).
  formEl.addEventListener('keydown', (e) => {
    if (e.key === 'F2') { e.preventDefault(); search.focus(); search.select(); }
    else if (e.key === 'F8') { e.preventDefault(); formEl.requestSubmit(); }
    else if (e.key === 'F9') { e.preventDefault(); printBill(st.lines); }
    else if (e.key === 'Escape') { e.preventDefault(); resetBill(); }
  });
  function resetBill() {
    st.lines = []; tbody.replaceChildren(emptyRow());
    st.discRate = st.discount = st.surcharge = st.roundoff = st.tendered = 0;
    fReason.value = ''; fTender.input.value = '0';
    setSaleType('CASH'); recalc();
  }

  const screen = h('div', { class: 'card pos-card' },
    h('h3', {}, 'Counter Sale'), formEl);
  setSaleType(st.salestype);
  void showStock(stmt.querySelector('.tab.active'));
  return screen;
}

/** Read-only display field (legacy read-only TextBox). */
function roField(label, cls) {
  const input = h('input', { readonly: true, class: cls || '' });
  return { wrap: field(label, input), input, get value() { return input.value; }, set value(v) { input.value = v; } };
}
/** Number field with an oninput handler; returns { wrap, input }. */
function numField(label, value, oninput) {
  const input = h('input', { type: 'number', step: '0.01', value });
  if (oninput) input.addEventListener('input', oninput);
  return { wrap: field(label, input), input };
}
function numFieldRaw(value, oninput) {
  const input = h('input', { type: 'number', step: '1', value, class: 'pos-cell' });
  if (oninput) input.addEventListener('input', oninput);
  return input;
}
function round2(n) { return Math.round((Number(n) + Number.EPSILON) * 100) / 100; }

/** Expense capture (legacy bt_expense -> frm_ExpenceForm). */
function expenseForm(ctx, params) {
  const wrap = h('div', { class: 'form' });
  const amount = h('input', { name: 'amount', type: 'number' });
  const ledger = select('ledcode', [{ value: params.expenceacc || 'L00004', label: 'Expense / Purchase' }], params.expenceacc || 'L00004');
  const pay = select('from', [{ value: params.cashhd || 'L00001', label: 'Cash' }, { value: params.bankacc || 'L00002', label: 'Bank' }]);
  const narr = h('input', { name: 'narr', placeholder: 'Narration' });
  const btn = h('button', { class: 'btn primary' }, 'Post Expense');
  btn.onclick = async () => {
    const amt = Number(amount.value) || 0;
    if (amt <= 0) return ctx.toast('Enter an amount', 'error');
    try {
      await api.createVoucher({
        type: 'PAYMENT', vchdate: today(), amount_paid: amt, head_account: ledger.value,
        client_account: pay.value, narration: narr.value || 'Counter expense', debit: 0, credit: 0,
      });
      ctx.toast('Expense posted', 'ok'); ctx.closeModal();
    } catch (ex) { ctx.toast(ex.message, 'error'); }
  };
  wrap.append(field('Amount', amount), field('Expense Head', ledger), field('Paid From', pay), field('Narration', narr),
    h('div', { class: 'actions' }, btn));
  return wrap;
}

/** Print a bill (browser print of the current lines). */
function printBill(lines) {
  if (!lines.length) return;
  const rows = lines.map((l) => `<tr><td>${l.itemname}</td><td>${l.qnty}</td><td>${money(l.rate)}</td><td>${money(l.amount)}</td></tr>`).join('');
  const w = window.open('', '_blank', 'width=380,height=600');
  if (!w) return;
  w.document.write(`<html><head><title>Bill</title><style>body{font:12px monospace;padding:8px}table{width:100%;border-collapse:collapse}td,th{border-bottom:1px dashed #999;padding:2px;text-align:left}</style></head><body><h3>VanijLiquor</h3><table><thead><tr><th>Item</th><th>Qty</th><th>Rate</th><th>Amt</th></tr></thead><tbody>${rows}</tbody></table><p>Total: ${money(lines.reduce((s, l) => s + l.amount, 0))}</p></body></html>`);
  w.document.close(); w.focus(); w.print();
}

const statCard = (k, v) => h('div', { class: 'stat' }, h('div', { class: 'k' }, k), h('div', { class: 'v' }, v));

async function salesRegister(ctx) {
  const rows = await api.sales({});
  const t = table([
    { key: 'trnno', title: 'Bill No' },
    { key: 'trndate', title: 'Date' },
    { key: 'salestype', title: 'Type' },
    { key: 'storecode', title: 'Store' },
    { key: 'ledgercode', title: 'Party' },
    { key: 'amount', title: 'Gross', money: true, num: true },
    { key: 'netamount', title: 'Net', money: true, num: true },
  ], rows);
  const tbody = t.querySelector('tbody');
  [...(tbody?.rows || [])].forEach((tr, i) => {
    tr.append(h('td', {}, h('button', {
      class: 'btn small',
      onclick: async () => {
        const inv = await api.gstInvoice(rows[i].trnno);
        if (!inv) return ctx.toast('Invoice not found', 'error');
        ctx.openModal(`Tax Invoice ${inv.invoice.trnno}`, h('div', {},
          h('p', {}, h('b', {}, inv.seller.name), ` — GSTIN ${inv.seller.gstin || '—'}`),
          h('p', { class: 'muted' }, inv.seller.address || ''),
          h('p', {}, `Buyer: ${inv.buyer.name || '—'}${inv.buyer.gstin ? ` (GSTIN ${inv.buyer.gstin})` : ''}`),
          table([
            { key: 'itemname', title: 'Item' }, { key: 'qnty', title: 'Qty', num: true },
            { key: 'taxable', title: 'Taxable', money: true, num: true },
            { key: 'gstp', title: 'Rate', num: true },
            { key: 'cgst', title: 'CGST', money: true, num: true },
            { key: 'sgst', title: 'SGST', money: true, num: true },
            { key: 'igst', title: 'IGST', money: true, num: true },
            { key: 'cess', title: 'Cess', money: true, num: true },
          ], inv.lines),
          h('div', { class: 'grid cols-3' },
            statCard('Taxable', money(inv.totals.taxable)), statCard('CGST', money(inv.totals.cgst)),
            statCard('SGST', money(inv.totals.sgst)), statCard('IGST', money(inv.totals.igst)),
            statCard('Cess', money(inv.totals.cess)), statCard('Bill Net', money(inv.invoice.netamount)))));
      },
    }, 'Tax Invoice')));
  });
  t.querySelector('thead tr').append(h('th', {}, ''));
  return h('div', {},
    h('div', { class: 'toolbar' },
      h('button', { class: 'btn', onclick: () => ctx.navigate('sale') }, '＋ New Sale'),
      h('span', { class: 'muted small' }, `${rows.length} bill(s)`)),
    t);
}

// --------------------------------------------------------------- purchase
async function purchaseScreen(ctx) {
  const items = await api.items();
  const lineEd = repeater([
    { name: 'itemcode', label: 'Item', options: items.map((i) => ({ value: i.itemcode, label: `${i.itemcode} · ${i.itemname}` })) },
    { name: 'itemquantity', label: 'Qty', type: 'number', value: '1' },
    { name: 'itemrate', label: 'Rate', type: 'number' },
    { name: 'itemmrp', label: 'MRP', type: 'number' },
    { name: 'batchno', label: 'Batch', value: 'B1' },
  ], [{ itemquantity: '1' }]);
  const f = h('form', { class: 'form' },
    h('div', { class: 'row three' },
      field('Date', input('trndate', today(), 'date')),
      field('Supplier Ledger', input('suppliercode', 'S00001')),
      field('Supplier Invoice No', input('docno', 'INV-001'))),
    h('div', { class: 'section-title' }, 'Items'),
    lineEd.node,
    h('div', { class: 'actions' }, h('button', { class: 'btn primary', type: 'submit' }, 'Save Purchase')));
  f.addEventListener('submit', async (e) => {
    e.preventDefault();
    const data = formValues(f);
    const lines = lineEd.values().map((l) => ({
      itemcode: l.itemcode, itemquantity: Number(l.itemquantity) || 1, itemrate: Number(l.itemrate) || 0,
      itemmrp: Number(l.itemmrp) || 0, batchno: l.batchno || 'B1', itemfreeqnty: 0,
    }));
    if (!lines.length) return ctx.toast('Add at least one item', 'error');
    try {
      const r = await api.createPurchase({ ...data, purchaseacccode: 'L00004', shopcode: 'MAIN', lines });
      ctx.openModal('Purchase Saved', h('div', { class: 'grid cols-2' },
        statCard('Trn No', String(r.trnno)), statCard('Net', money(r.totnetamt))));
      ctx.toast('Purchase saved', 'ok');
    } catch (ex) { ctx.toast(ex.message, 'error'); }
  });
  return h('div', { class: 'card' }, h('h3', {}, 'Purchase Entry'), f);
}

// --------------------------------------------------------------- vouchers
async function voucherScreen(ctx, type) {
  const isJournal = type === 'JOURNAL';
  const isContra = type === 'CONTRA';
  let f;
  if (isJournal) {
    const lineEd = repeater([
      { name: 'ledcode', label: 'Ledger', value: 'L00001' },
      { name: 'debit', label: 'Debit', type: 'number', value: '0' },
      { name: 'credit', label: 'Credit', type: 'number', value: '0' },
    ], [{ debit: '0', credit: '0' }]);
    f = h('form', { class: 'form' },
      field('Date', input('vchdate', today(), 'date')),
      h('div', { class: 'section-title' }, 'Lines'),
      lineEd.node,
      h('div', { class: 'actions' }, h('button', { class: 'btn primary', type: 'submit' }, 'Post Journal')));
    f.addEventListener('submit', async (e) => {
      e.preventDefault();
      const d = formValues(f);
      const lines = lineEd.values().map((l) => ({ ledcode: l.ledcode, debit: Number(l.debit) || 0, credit: Number(l.credit) || 0 }));
      try { const r = await api.createVoucher({ type, vchdate: d.vchdate, lines }); ctx.toast(`Journal ${r.trnno} posted`, 'ok'); }
      catch (ex) { ctx.toast(ex.message, 'error'); }
    });
  } else if (isContra) {
    f = form([
      [['Date', input('vchdate', today(), 'date')]],
      [['From Ledger', input('from_ledger', 'L00001')], ['To Ledger', input('to_ledger', 'L00002')]],
      [['Amount', input('amount', '0', 'number')]],
    ], async (d) => { try { await api.createVoucher({ type, ...d, amount: Number(d.amount) || 0 }); ctx.toast('Contra posted', 'ok'); } catch (e) { ctx.toast(e.message, 'error'); } });
  } else {
    f = form([
      [['Date', input('vchdate', today(), 'date')]],
      [['Head Account', input('head_account', 'L00002')], ['Client Account', input('client_account', type === 'PAYMENT' ? 'S00001' : 'C00001')]],
      [['Amount', input('amount_paid', '0', 'number')], ['Narration', input('narration', `${type} entry`)]],
    ], async (d) => { try { const r = await api.createVoucher({ type, ...d, amount_paid: Number(d.amount_paid) || 0 }); ctx.toast(`Voucher ${r.vchno} saved`, 'ok'); } catch (e) { ctx.toast(e.message, 'error'); } });
  }
  const recents = await api.vouchers(type);
  return h('div', {},
    h('div', { class: 'card' }, h('h3', {}, `${humanize(type)} Entry`), f),
    h('div', { class: 'section-title' }, 'Recent'),
    table([{ key: 'vchno', title: 'No' }, { key: 'vchdate', title: 'Date' }, { key: 'amount', title: 'Amount', money: true, num: true }, { key: 'narration', title: 'Narration' }], recents));
}

async function noteScreen(ctx, type) {
  const f = form([
    [['Date', input('trndate', today(), 'date')], ['Party Ledger', input('ledcode', 'C00001')]],
    [['Amount', input('amount', '0', 'number')], ['Narration', input('narration', `${humanize(type)} note`)]],
  ], async (d) => { try { await api.createNote({ type, ...d, amount: Number(d.amount) || 0 }); ctx.toast('Note saved', 'ok'); ctx.navigate(ctx.state.route); } catch (e) { ctx.toast(e.message, 'error'); } });
  const rows = await api.notes(type);
  return h('div', {},
    h('div', { class: 'card' }, h('h3', {}, `${humanize(type)} Note`), f),
    h('div', { class: 'section-title' }, 'Recent'),
    table([{ key: 'trnno', title: 'No' }, { key: 'trndate', title: 'Date' }, { key: 'ledcode', title: 'Party' }, { key: 'amount', title: 'Amount', money: true, num: true }], rows));
}

// --------------------------------------------------------------- stock ops
async function openingStock(ctx) {
  const f = form([
    [['Store', input('shopcode', 'MAIN')], ['Item Code', input('itemcode', '')]],
    [['Quantity', input('qnty', '0', 'number')], ['Date', input('trndate', today(), 'date')]],
  ], async (d) => { try { await api.addOpeningStock({ ...d, qnty: Number(d.qnty) || 0 }); ctx.toast('Opening stock saved', 'ok'); } catch (e) { ctx.toast(e.message, 'error'); } });
  return h('div', { class: 'card' }, h('h3', {}, 'Opening Stock'), f);
}

async function stockTransferScreen(ctx) {
  const lineEd = repeater([
    { name: 'itemcode', label: 'Item', value: '' },
    { name: 'qnty', label: 'Qty', type: 'number', value: '1' },
  ], [{ qnty: '1' }]);
  const f = h('form', { class: 'form' },
    h('div', { class: 'row three' }, field('From', input('from', 'MAIN')), field('To', input('to', 'CNT1')), field('Date', input('trndate', today(), 'date'))),
    h('div', { class: 'section-title' }, 'Items'), lineEd.node,
    h('div', { class: 'actions' }, h('button', { class: 'btn primary', type: 'submit' }, 'Save Transfer')));
  f.addEventListener('submit', async (e) => {
    e.preventDefault();
    const d = formValues(f);
    const lines = lineEd.values().map((l) => ({ itemcode: l.itemcode, qnty: Number(l.qnty) || 0 }));
    try { const r = await api.stockTransfer({ ...d, lines }); ctx.toast(`Transfer ${r.trnno} saved`, 'ok'); } catch (ex) { ctx.toast(ex.message, 'error'); }
  });
  return h('div', { class: 'card' }, h('h3', {}, 'Stock Transfer'), f);
}

async function breakageScreen(ctx) {
  const lineEd = repeater([
    { name: 'itemcode', label: 'Item', value: '' },
    { name: 'quantity', label: 'Qty', type: 'number', value: '1' },
  ], [{ quantity: '1' }]);
  const f = h('form', { class: 'form' },
    h('div', { class: 'row' }, field('Store', input('shopcode', 'MAIN')), field('Date', input('trndate', today(), 'date'))),
    h('div', { class: 'section-title' }, 'Items'), lineEd.node,
    h('div', { class: 'actions' }, h('button', { class: 'btn primary', type: 'submit' }, 'Save Breakage')));
  f.addEventListener('submit', async (e) => {
    e.preventDefault();
    const d = formValues(f);
    const lines = lineEd.values().map((l) => ({ itemcode: l.itemcode, quantity: Number(l.quantity) || 0 }));
    try { const r = await api.breakage({ ...d, lines }); ctx.toast(`Breakage ${r.trnno} saved`, 'ok'); } catch (ex) { ctx.toast(ex.message, 'error'); }
  });
  return h('div', { class: 'card' }, h('h3', {}, 'Breakage'), f);
}

async function transferSaleScreen(ctx) {
  const lineEd = repeater([
    { name: 'itemcode', label: 'Item', value: '' },
    { name: 'qnty', label: 'Qty', type: 'number', value: '1' },
  ], [{ qnty: '1' }]);
  const f = h('form', { class: 'form' },
    h('div', { class: 'row three' }, field('From', input('from', 'MAIN')), field('To', input('to', 'CNT1')), field('Date', input('trndate', today(), 'date'))),
    h('div', { class: 'section-title' }, 'Items'), lineEd.node,
    h('div', { class: 'actions' }, h('button', { class: 'btn primary', type: 'submit' }, 'Save')));
  f.addEventListener('submit', async (e) => {
    e.preventDefault();
    const d = formValues(f);
    const lines = lineEd.values().map((l) => ({ itemcode: l.itemcode, qnty: Number(l.qnty) || 0 }));
    try { const r = await api.createTransferSale({ ...d, lines }); ctx.toast(`Transfer sale ${r.trnno} saved`, 'ok'); ctx.navigate(ctx.state.route); } catch (ex) { ctx.toast(ex.message, 'error'); }
  });
  const rows = await api.transferSales();
  return h('div', {}, h('div', { class: 'card' }, h('h3', {}, 'Counter / Transfer Sale'), f),
    h('div', { class: 'section-title' }, 'Recent'),
    table([{ key: 'trnno', title: 'No' }, { key: 'trndate', title: 'Date' }, { key: 'from', title: 'From' }, { key: 'to', title: 'To' }, { key: 'amount', title: 'Amount', money: true, num: true }], rows));
}

async function transferReceiveScreen(ctx) {
  const f = form([
    [['From', input('from', 'MAIN')], ['To', input('to', 'CNT1')]],
    [['Date', input('trndate', today(), 'date')], ['Transfer No', input('trnno', '')]],
    [['Amount', input('amount', '0', 'number')]],
  ], async (d) => { try { await api.createTransferReceive({ ...d, amount: Number(d.amount) || 0 }); ctx.toast('Transfer receive saved', 'ok'); ctx.navigate(ctx.state.route); } catch (e) { ctx.toast(e.message, 'error'); } });
  const rows = await api.transferReceives();
  return h('div', {}, h('div', { class: 'card' }, h('h3', {}, 'Transfer Receive'), f),
    h('div', { class: 'section-title' }, 'Recent'),
    table([{ key: 'trnno', title: 'No' }, { key: 'trndate', title: 'Date' }, { key: 'from', title: 'From' }, { key: 'to', title: 'To' }, { key: 'amount', title: 'Amount', money: true, num: true }], rows));
}

async function onAccountScreen(ctx) {
  const f = form([
    [['Date', input('trndate', today(), 'date')], ['Ledger', input('ledcode', 'C00001')]],
    [['Amount', input('amount', '0', 'number')], ['Narration', input('narration', 'On A/C adjustment')]],
  ], async (d) => { try { await api.onAccount({ ...d, amount: Number(d.amount) || 0 }); ctx.toast('Adjustment saved', 'ok'); } catch (e) { ctx.toast(e.message, 'error'); } });
  return h('div', { class: 'card' }, h('h3', {}, 'On A/C Adjustment'), f);
}

async function accountReplaceScreen(ctx) {
  const f = form([
    [['Date', input('trndate', today(), 'date')]],
    [['From Ledger', input('from_ledcode', 'C00001')], ['To Ledger', input('to_ledcode', 'C00002')]],
    [['Amount', input('amount', '0', 'number')]],
  ], async (d) => { try { await api.accountReplace({ ...d, amount: Number(d.amount) || 0 }); ctx.toast('Replace saved', 'ok'); } catch (e) { ctx.toast(e.message, 'error'); } });
  return h('div', { class: 'card' }, h('h3', {}, 'A/C Replace'), f);
}

async function fineScreen(ctx) {
  const f = form([
    [['Date', input('trndate', today(), 'date')], ['Shop', input('shopcode', 'MAIN')]],
    [['Amount', input('amount', '0', 'number')], ['Reason', input('reason', 'Excise fine')]],
  ], async (d) => { try { await api.createFine({ ...d, amount: Number(d.amount) || 0 }); ctx.toast('Fine saved', 'ok'); ctx.navigate(ctx.state.route); } catch (e) { ctx.toast(e.message, 'error'); } });
  const rows = await api.fines();
  return h('div', {}, h('div', { class: 'card' }, h('h3', {}, 'Excise Fine'), f),
    h('div', { class: 'section-title' }, 'Recent'),
    table([{ key: 'trnno', title: 'No' }, { key: 'trndate', title: 'Date' }, { key: 'shopcode', title: 'Shop' }, { key: 'amount', title: 'Amount', money: true, num: true }, { key: 'reason', title: 'Reason' }], rows));
}

async function brsScreen(ctx) {
  const [rows, upiRows] = await Promise.all([api.reconciliation(''), api.upi()]);
  const f = form([
    [['Ledger', input('ledcode', 'L00002')], ['Amount', input('amount', '0', 'number')]],
    [['Date', input('trndate', today(), 'date')], ['Reference', input('reference', '')]],
  ], async (d) => { try { await api.addReconciliation({ ...d, amount: Number(d.amount) || 0 }); ctx.toast('BRS entry saved', 'ok'); ctx.navigate(ctx.state.route); } catch (e) { ctx.toast(e.message, 'error'); } });
  const t = table([
    { key: 'slno', title: 'Sl' }, { key: 'ledcode', title: 'Ledger' }, { key: 'amount', title: 'Amount', money: true, num: true },
    { key: 'trndate', title: 'Date' }, { key: 'cleareddate', title: 'Cleared', badge: (r) => (r.cleareddate ? 'ok' : 'warn') },
  ], rows);
  [...t.querySelector('tbody').rows].forEach((tr, i) => {
    tr.append(h('td', {}, rows[i].cleareddate ? '—' : h('button', {
      class: 'btn small', onclick: async () => { try { await api.clearReconciliation(rows[i].slno, today()); ctx.toast('Cleared', 'ok'); ctx.navigate(ctx.state.route); } catch (e) { ctx.toast(e.message, 'error'); } },
    }, 'Clear')));
  });
  t.querySelector('thead tr').append(h('th', {}, ''));
  return h('div', {},
    h('div', { class: 'card' }, h('h3', {}, 'BRS Entry (Bank Reconciliation)'), f),
    h('div', { class: 'section-title' }, 'Reconciliation'),
    t,
    h('div', { class: 'section-title' }, 'UPI Transactions'),
    table([{ key: 'trnno', title: 'No' }, { key: 'trndate', title: 'Date' }, { key: 'amount', title: 'Amount', money: true, num: true }, { key: 'refno', title: 'Ref' }], upiRows));
}

async function upiScreen(ctx) {
  const f = form([
    [['Date', input('trndate', today(), 'date')], ['Amount', input('amount', '0', 'number')]],
    [['UPI Ref', input('refno', '')], ['Party', input('ledcode', 'C00001')]],
  ], async (d) => { try { await api.recordUpi({ ...d, amount: Number(d.amount) || 0 }); ctx.toast('UPI recorded', 'ok'); ctx.navigate(ctx.state.route); } catch (e) { ctx.toast(e.message, 'error'); } });
  const rows = await api.upi();
  return h('div', {}, h('div', { class: 'card' }, h('h3', {}, 'QRCode / UPI Payment'), f),
    h('div', { class: 'section-title' }, 'Recent'),
    table([{ key: 'trnno', title: 'No' }, { key: 'trndate', title: 'Date' }, { key: 'amount', title: 'Amount', money: true, num: true }, { key: 'refno', title: 'Ref' }], rows));
}

async function deliveryScreen(ctx) {
  const f = form([
    [['Date', input('trndate', today(), 'date')], ['Customer', input('custcode', 'CUST1')]],
    [['Address', input('address', '')], ['Amount', input('amount', '0', 'number')]],
  ], async (d) => { try { await api.createDelivery({ ...d, amount: Number(d.amount) || 0 }); ctx.toast('Delivery order saved', 'ok'); ctx.navigate(ctx.state.route); } catch (e) { ctx.toast(e.message, 'error'); } });
  const rows = await api.deliveries();
  const statuses = ['PENDING', 'OUT', 'DELIVERED', 'CANCELLED'];
  const t = table([
    { key: 'orderno', title: 'Order' }, { key: 'trndate', title: 'Date' }, { key: 'custcode', title: 'Customer' },
    { key: 'amount', title: 'Amount', money: true, num: true }, { key: 'status', title: 'Status', badge: (r) => (r.status === 'DELIVERED' ? 'ok' : r.status === 'CANCELLED' ? 'danger' : 'warn') },
  ], rows);
  [...t.querySelector('tbody').rows].forEach((tr, i) => {
    const sel = select('status', statuses, rows[i].status);
    sel.style.minWidth = '120px';
    tr.append(h('td', {}, h('div', { style: 'display:flex;gap:6px' }, sel,
      h('button', { class: 'btn small', onclick: async () => { try { await api.setDeliveryStatus(rows[i].orderno, sel.value); ctx.toast('Status updated', 'ok'); ctx.navigate(ctx.state.route); } catch (e) { ctx.toast(e.message, 'error'); } } }, 'Set'))));
  });
  t.querySelector('thead tr').append(h('th', {}, 'Update'));
  return h('div', {}, h('div', { class: 'card' }, h('h3', {}, 'Home Delivery'), f),
    h('div', { class: 'section-title' }, 'Orders'), t);
}

// --------------------------------------------------------------- dashboards
async function dashGeneric(ctx, group, title) {
  const groups = await api.reportGroups();
  const list = groups[group] || [];
  const rows = list.length ? await api.catalogReport(list[0].key).catch(() => []) : [];
  const keys = rows.length ? Object.keys(rows[0]) : [];
  return h('div', {},
    h('div', { class: 'card' }, h('h3', {}, title),
      table(keys.map((k) => ({ key: k, title: humanize(k), num: /amount|amt|qnty|value|net|gross/i.test(k) })), rows)),
    h('div', { class: 'section-title' }, 'More'),
    h('div', { class: 'list-menu' }, ...list.map((r) => h('button', { onclick: () => runReport(ctx, group, r) }, r.title))));
}

async function dashScheme(ctx) {
  return dashGeneric(ctx, 'excise', 'Scheme / Spirit-Beer / CS Figures');
}

// --------------------------------------------------------------- tools
function calcScreen(ctx) {
  const out = h('input', { readonly: true, value: '0' });
  const mk = (label, val, op) => h('button', { class: 'btn', onclick: async () => {
    try { const r = await api.calc(Number(out.value) || 0, op, Number(val)); out.value = String(r.result); }
    catch (e) { ctx.toast(e.message, 'error'); }
  } }, label);
  const digits = ['1', '2', '3', '4', '5', '6', '7', '8', '9', '0'];
  return h('div', { class: 'card' },
    h('h3', {}, 'Calculator'),
    h('div', { style: 'max-width:280px' }, out,
      h('div', { class: 'list-menu', style: 'margin-top:10px' },
        ...digits.map((d) => h('button', { class: 'btn', onclick: () => { out.value = out.value === '0' ? d : out.value + d; } }, d)),
        mk('＋', 0, 'add'), mk('−', 0, 'sub'), mk('×', 1, 'mul'), mk('÷', 1, 'div'))));
}

function changePassword(ctx) {
  return h('div', { class: 'card' }, h('h3', {}, 'Change Password'),
    form([
      [['New PIN', h('input', { name: 'pin', type: 'password' })], ['Confirm PIN', h('input', { name: 'pin2', type: 'password' })]],
    ], async (d) => {
      if (!d.pin || d.pin !== d.pin2) return ctx.toast('PINs do not match', 'error');
      try { await api.resetPin(ctx.state.user.usercode, d.pin); ctx.toast('Password changed', 'ok'); }
      catch (e) { ctx.toast(e.message, 'error'); }
    }));
}

function backupScreen(ctx) {
  return h('div', { class: 'card' }, h('h3', {}, 'Data Back-Up'),
    form([[['Destination folder', input('dest', '/data/backup')]]], async (d) => {
      try { await api.backup(d.dest); ctx.toast('Backup created', 'ok'); } catch (e) { ctx.toast(e.message, 'error'); }
    }));
}

function restoreScreen(ctx) {
  return h('div', { class: 'card' }, h('h3', {}, 'Data Restore'),
    h('p', { class: 'muted' }, 'Restore reads a backup of the SQLite file from the server. Place the file on the server and give its path.'),
    form([[['Backup file path', input('src', '/data/backup/vanijliquor.db')]]], async () => {
      ctx.toast('Restore is performed server-side; not enabled in this build', 'error');
    }));
}

async function tallyScreen(ctx) {
  const kindSel = select('kind', ['SALES', 'PURCHASE', 'LEDGER'], 'SALES');
  const out = h('pre', { class: 'card', style: 'white-space:pre-wrap' }, '');
  const btn = h('button', { class: 'btn primary' }, 'Export');
  btn.onclick = async () => { try { out.textContent = JSON.stringify(await api.tally(kindSel.value), null, 2); } catch (e) { ctx.toast(e.message, 'error'); } };
  return h('div', {}, h('div', { class: 'card' }, h('h3', {}, 'Export To Tally'), h('div', { class: 'toolbar' }, field('Data', kindSel), btn)), out);
}

async function messagesScreen(ctx) {
  const rows = await api.messages();
  const f = form([[['To', input('to', '')], ['Message', input('body', '')]]], async (d) => {
    try { await api.sendMessage(d); ctx.toast('Message queued', 'ok'); ctx.navigate(ctx.state.route); } catch (e) { ctx.toast(e.message, 'error'); }
  });
  return h('div', {}, h('div', { class: 'card' }, h('h3', {}, 'SMS / WhatsApp'), f),
    h('div', { class: 'section-title' }, 'Outbox'),
    table([{ key: 'sentto', title: 'To' }, { key: 'message', title: 'Message' }, { key: 'sentat', title: 'Sent At' }], rows));
}

async function pmlScreen(ctx) {
  const rows = await api.pml();
  const f = form([[['Date', input('trndate', today(), 'date')], ['Bottles', input('bottles', '0', 'number')]], [['ML', input('ml', '0', 'number')], ['Remarks', input('remarks', '')]]],
    async (d) => { try { await api.addPml(d); ctx.toast('PML entry saved', 'ok'); ctx.navigate(ctx.state.route); } catch (e) { ctx.toast(e.message, 'error'); } });
  return h('div', {}, h('div', { class: 'card' }, h('h3', {}, 'PML Print and Reports'), f),
    h('div', { class: 'section-title' }, 'Entries'), table([{ key: 'slno', title: 'Sl' }, { key: 'trndate', title: 'Date' }, { key: 'bottles', title: 'Bottles', num: true }, { key: 'ml', title: 'ML', num: true }, { key: 'remarks', title: 'Remarks' }], rows));
}

function cleanPrintScreen(ctx) {
  return h('div', { class: 'card' }, h('h3', {}, 'Clean Print Garbage'),
    h('p', { class: 'muted' }, 'Removes orphaned print records for the current company.'),
    h('div', { class: 'actions' }, h('button', { class: 'btn danger', onclick: async () => { try { await api.cleanPrint(); ctx.toast('Print garbage cleaned', 'ok'); } catch (e) { ctx.toast(e.message, 'error'); } } }, 'Run Cleanup')));
}

// --------------------------------------------------------------- data migration
function migrationResultCard(title, res) {
  const totals = res.totals || {};
  const stat = (label, value, kind = '') => h('div', { class: 'card stat' },
    h('div', { class: 'k' }, label), h('div', { class: `v${kind ? ` ${kind}` : ''}` }, String(value ?? 0)));
  const t = table([
    { key: 'table', title: 'Table' },
    { key: 'sourceRows', title: 'Rows', num: true },
    { key: 'inserts', title: 'Insert', num: true },
    { key: 'updates', title: 'Update', num: true },
    { key: 'failed', title: 'Failed', num: true },
    { key: 'unknownColumns', title: 'Ignored columns', value: (r) => (r.unknownColumns?.length ? r.unknownColumns.join(', ') : '—') },
  ], res.tables || []);
  const problems = (res.tables || []).flatMap((tb) => (tb.samples || []).map((s) => ({ table: tb.table, ...s })));
  return h('div', { class: 'card' },
    h('div', { class: 'section-title' }, title),
    h('div', { class: 'grid cols-4' },
      stat('Tables', totals.tables),
      stat('Inserted', res.mode === 'APPLY' ? (totals.inserted ?? totals.inserts) : totals.inserts, 'ok'),
      stat('Updated', res.mode === 'APPLY' ? (totals.updated ?? totals.updates) : totals.updates, 'warn'),
      stat('Failed', res.mode === 'APPLY' ? (totals.failed ?? totals.failed) : totals.failed, totals.failed ? 'danger' : '')),
    (res.skippedTables?.length)
      ? h('p', { class: 'muted' }, `Skipped tables: ${res.skippedTables.map((s) => `${s.table} (${s.reason})`).join('; ')}`)
      : null,
    t,
    problems.length ? h('div', {}, h('div', { class: 'section-title' }, 'Row errors'),
      h('pre', { class: 'card', style: 'white-space:pre-wrap;margin:0' }, JSON.stringify(problems.slice(0, 30), null, 2))) : null);
}

async function migrationScreen(ctx) {
  const tables = await api.migrationTables().catch(() => []);
  const history = await api.migrationHistory().catch(() => []);

  const sourceSel = select('source', [
    { value: 'sql', label: 'SQL dump (.sql, legacy INSERT statements)' },
    { value: 'json', label: 'JSON bundle (tables -> rows)' },
    { value: 'csv', label: 'CSV (one table)' },
    { value: 'legacy', label: 'Server folder (CSV/JSON files per table)' },
  ], 'sql');
  const tableInput = input('table', 'itemmst');
  const pathInput = input('path', '/data/legacy-export');
  const filenameInput = input('filename', 'legacy_export.sql');

  const textarea = h('textarea', { name: 'content', rows: 10, placeholder: 'Paste the export here, or use “Choose file” below…', style: 'width:100%;font-family:monospace' });
  const fileInput = h('input', { type: 'file', accept: '.sql,.json,.csv,.txt' });
  fileInput.addEventListener('change', () => {
    const f = fileInput.files && fileInput.files[0];
    if (!f) return;
    const ext = f.name.toLowerCase().split('.').pop();
    if (ext === 'json') sourceSel.value = 'json';
    else if (ext === 'csv') sourceSel.value = 'csv';
    else sourceSel.value = 'sql';
    filenameInput.value = f.name;
    const reader = new FileReader();
    reader.onload = () => { textarea.value = String(reader.result || ''); };
    reader.readAsText(f);
    syncMode();
  });

  const fileRow = h('div', { class: 'row' }, field('Target table (CSV only)', tableInput), field('File name', filenameInput));
  const pathRow = h('div', { class: 'row' }, field('Server folder path', pathInput));
  const contentField = field('Export contents', textarea);

  function syncMode() {
    const mode = sourceSel.value;
    fileRow.hidden = mode !== 'csv';
    pathRow.hidden = mode !== 'legacy';
    contentField.hidden = mode === 'legacy';
    fileInput.hidden = mode === 'legacy';
  }
  sourceSel.addEventListener('change', syncMode);

  const out = h('div', {});
  const sourceOpts = () => {
    const mode = sourceSel.value;
    return {
      source: mode,
      table: mode === 'csv' ? tableInput.value.trim() : undefined,
      path: mode === 'legacy' ? pathInput.value.trim() : undefined,
      filename: filenameInput.value.trim() || undefined,
      content: mode === 'legacy' ? undefined : textarea.value,
    };
  };
  const run = async (which) => {
    out.replaceChildren(h('div', { class: 'loading' }, h('span', { class: 'spinner' }), 'Working…'));
    try {
      const res = which === 'apply' ? await api.migrationApply(sourceOpts()) : await api.migrationAnalyze(sourceOpts());
      out.replaceChildren(migrationResultCard(which === 'apply' ? `Applied — batch #${res.batchId}` : `Dry run — batch #${res.batchId}`, res));
      ctx.toast(which === 'apply' ? 'Migration applied' : 'Dry run complete', 'ok');
    } catch (e) {
      out.replaceChildren(h('div', { class: 'card' }, h('p', { class: 'error' }, e.message)));
      ctx.toast(e.message, 'error');
    }
  };

  const analyzeBtn = h('button', { class: 'btn', onclick: () => run('analyze') }, 'Analyze (dry run)');
  const applyBtn = h('button', { class: 'btn primary', onclick: () => {
    if (!confirm('Write the analyzed data into this database? Existing rows with the same key are updated; nothing is deleted.')) return;
    run('apply');
  } }, 'Apply migration');

  const exportBtn = h('button', { class: 'btn ghost', onclick: async () => {
    try {
      const r = await api.migrationExportSql();
      textarea.value = r.script;
      sourceSel.value = 'sql';
      syncMode();
      ctx.toast('Export helper script loaded', 'ok');
    } catch (e) { ctx.toast(e.message, 'error'); }
  } }, 'Load export helper script');

  const histTable = table([
    { key: 'id', title: '#' },
    { key: 'mode', title: 'Mode' },
    { key: 'source', title: 'Source' },
    { key: 'filename', title: 'File' },
    { key: 'status', title: 'Status', badge: (r) => ({ APPLIED: 'ok', ANALYZED: 'warn', FAILED: 'danger' }[r.status] || '') },
    { key: 'inserted', title: 'Ins', num: true },
    { key: 'updated', title: 'Upd', num: true },
    { key: 'failed', title: 'Fail', num: true },
    { key: 'finishedat', title: 'When' },
  ], history);

  syncMode();
  return h('div', {},
    h('div', { class: 'card' },
      h('h3', {}, 'Data Migration from the Legacy App'),
      h('p', { class: 'muted' }, 'Import masters and transactions exported from the original Madhushala Pro SQL Server database. Pick a source, load the file, run a dry run to preview, then apply. Rows are inserted, or updated when their primary key already exists — no table is ever deleted or truncated.'),
      h('div', { class: 'toolbar' }, field('Source', sourceSel), exportBtn),
      fileRow, pathRow, contentField,
      h('div', { class: 'row' }, field('Choose file', fileInput)),
      h('div', { class: 'actions' }, analyzeBtn, applyBtn)),
    h('div', { class: 'card' },
      h('div', { class: 'section-title' }, `Migratable tables (${tables.length})`),
      h('p', { class: 'muted small' }, tables.map((t) => t.table + (t.present ? '' : ' (n/a)')).join(' · '))),
    out,
    h('div', { class: 'card' },
      h('div', { class: 'section-title' }, 'Migration history'),
      histTable));
}

async function syncScreen(ctx) {
  const status = await api.syncStatus();
  const entitySel = select('entity', ['SALES', 'PURCHASE', 'ITEMS', 'LEDGER'], 'SALES');
  const dirSel = select('direction', ['PUSH', 'PULL'], 'PUSH');
  const btn = h('button', { class: 'btn primary' }, 'Sync now');
  btn.onclick = async () => { try { await api.sync(entitySel.value, dirSel.value); ctx.toast('Sync complete', 'ok'); ctx.navigate(ctx.state.route); } catch (e) { ctx.toast(e.message, 'error'); } };
  return h('div', {},
    h('div', { class: 'card' }, h('h3', {}, 'Data Sync'),
      h('div', { class: 'toolbar' }, field('Entity', entitySel), field('Direction', dirSel), btn)),
    h('div', { class: 'section-title' }, 'Status'),
    h('div', { class: 'card' }, h('pre', { style: 'white-space:pre-wrap;margin:0' }, JSON.stringify(status, null, 2))));
}

// --------------------------------------------------------------- remote Tally sync
const TALLY_STATUS_BADGE = { PENDING: '', SENT: 'warn', ACKED: 'ok', FAILED: 'error' };

async function tallySyncScreen(ctx) {
  const [cfg, summary, queue, mapping, inbound, report] = await Promise.all([
    api.tallyConfig(), api.tallySummary(), api.tallyQueue(),
    api.tallyMapping(), api.tallyInbound(), api.tallyReport(),
  ]);

  // --- configuration ---
  const enabled = h('input', { type: 'checkbox', name: 'enabled' });
  enabled.checked = !!cfg.enabled;
  const tlsinsecure = h('input', { type: 'checkbox', name: 'tlsinsecure' });
  tlsinsecure.checked = !!cfg.tlsinsecure;
  const cfgForm = form([
    [['Broker URL', input('brokerurl', cfg.brokerurl || 'https://broker.vanijliquor.cloud')],
     ['Tally company', input('tallycompany', cfg.tallycompany || '')]],
    [['Agent ID', input('agentid', cfg.agentid || '')],
     ['Company GUID', input('companyguid', cfg.companyguid || '')]],
    [['Agent token', input('agenttoken', cfg.agenttoken || '')],
     ['Enabled', enabled]],
    [['Tally endpoint (HTTP/XML)', input('tallyurl', cfg.tallyurl || 'http://127.0.0.1:9000')],
     ['Tally company name', input('tallycompanyname', cfg.tallycompanyname || '')]],
    [['Accept self-signed TLS', tlsinsecure]],
  ], async (d) => {
    await api.saveTallyConfig({ ...d, enabled: enabled.checked, tlsinsecure: tlsinsecure.checked });
    ctx.toast('Sync settings saved', 'ok');
    ctx.navigate(ctx.state.route);
  });
  const tokenBtn = h('button', { class: 'btn' }, 'Generate token');
  tokenBtn.onclick = async () => {
    try { await api.newTallyToken(); ctx.toast('New agent token generated', 'ok'); ctx.navigate(ctx.state.route); }
    catch (e) { ctx.toast(e.message, 'error'); }
  };

  // --- status + actions ---
  const stat = (label, value, kind = '') => h('div', { class: 'card stat' },
    h('div', { class: 'k' }, label), h('div', { class: `v${kind ? ` ${kind}` : ''}` }, String(value)));
  const stats = h('div', { class: 'grid cols-4' },
    stat('Pending', summary.PENDING, summary.PENDING ? 'warn' : ''),
    stat('Sent', summary.SENT),
    stat('Delivered', summary.ACKED),
    stat('Failed', summary.FAILED, summary.FAILED ? 'danger' : ''),
    stat('Total', summary.total));

  const act = (label, fn, cls = 'btn') => {
    const b = h('button', { class: cls }, label);
    b.onclick = async () => { try { const r = await fn(); ctx.toast(typeof r === 'string' ? r : `${label} done`, 'ok'); ctx.navigate(ctx.state.route); } catch (e) { ctx.toast(e.message, 'error'); } };
    return b;
  };
  const actions = h('div', { class: 'toolbar' },
    act('Queue pending bills', async () => { const r = await api.tallyEnqueue(); return `${r.queued} voucher(s) queued`; }, 'btn primary'),
    act('Retry failed', async () => { const r = await api.tallyRetry(); return `${r.retried} requeued`; }),
    act('Refresh', async () => ''));

  // --- live Tally Prime push ---
  const testBtn = h('button', { class: 'btn' }, 'Test connection');
  testBtn.onclick = async () => {
    try { const r = await api.tallyLiveTest(); ctx.toast(r.ok ? `Tally reachable (HTTP ${r.status})` : `No response (HTTP ${r.status})`, r.ok ? 'ok' : 'warn'); }
    catch (e) { ctx.toast(e.message, 'error'); }
  };
  const pushBtn = h('button', { class: 'btn primary' }, 'Push to Tally Prime');
  pushBtn.onclick = async () => {
    try {
      const r = await api.tallyLivePush({});
      if (r.failed) ctx.toast(`${r.failed} voucher(s) rejected: ${r.error}`, 'error');
      else ctx.toast(`${r.acked || 0} voucher(s) imported into Tally Prime`, 'ok');
      ctx.navigate(ctx.state.route);
    } catch (e) { ctx.toast(e.message, 'error'); }
  };
  const liveCard = h('div', { class: 'card' },
    h('h3', {}, 'Live Tally Prime push'),
    h('p', { class: 'muted' }, 'Import the queued vouchers straight into Tally Prime over its HTTP/XML server (enable it in Tally: F1 → Settings → Advanced Configuration → “Enable HTTP/XML”, default port 9000). Nothing is removed from the queue until Tally accepts the envelope; failures are kept for a retry.'),
    h('div', { class: 'toolbar' }, testBtn, pushBtn));

  // --- XML preview ---
  const xmlOut = h('pre', { class: 'card', style: 'white-space:pre-wrap;max-height:340px;overflow:auto;margin:0' }, '');
  const xmlBtn = h('button', { class: 'btn' }, 'Preview Tally XML');
  xmlBtn.onclick = async () => {
    try { const r = await api.tallyXml(); xmlOut.textContent = r.count ? r.xml : 'Queue is empty.'; }
    catch (e) { ctx.toast(e.message, 'error'); }
  };

  // --- queue ---
  const queueTable = table([
    { key: 'id', title: '#' }, { key: 'vouchertype', title: 'Type' }, { key: 'docno', title: 'Doc' },
    { key: 'docdate', title: 'Date' }, { key: 'amount', title: 'Amount', money: true },
    { key: 'attempts', title: 'Tries', num: true },
    { key: 'status', title: 'Status', badge: (r) => TALLY_STATUS_BADGE[r.status] || '' },
    { key: 'lasterror', title: 'Last error' },
  ], queue);

  // --- mapping ---
  const MAP_TYPES = ['LEDGER', 'ITEM', 'TAX', 'PAYMENT'];
  const mapType = select('maptype', MAP_TYPES, 'LEDGER');
  const mapForm = form([
    [['Type', mapType], ['POS pattern', input('pattern', '')], ['Tally master', input('targetname', '')]],
  ], async (d) => {
    await api.addTallyMapping({ ...d, priority: 1 });
    ctx.toast('Mapping rule added', 'ok');
    ctx.navigate(ctx.state.route);
  });
  const mappingTable = table([
    { key: 'maptype', title: 'Type' }, { key: 'pattern', title: 'POS pattern' },
    { key: 'targetname', title: 'Tally master' }, { key: 'priority', title: 'Priority', num: true },
    { key: 'active', title: 'Active', value: (r) => (r.active ? 'Yes' : 'No') },
  ], mapping);
  [...(mappingTable.querySelector('tbody')?.rows || [])].forEach((tr, i) => {
    tr.append(h('td', {}, h('button', { class: 'btn danger small', onclick: async () => {
      try { await api.deleteTallyMapping(mapping[i].id); ctx.toast('Mapping removed', 'ok'); ctx.navigate(ctx.state.route); }
      catch (e) { ctx.toast(e.message, 'error'); }
    } }, 'Delete')));
  });
  if (mapping.length) mappingTable.querySelector('thead tr').append(h('th', {}, ''));

  // --- inbound masters ---
  const inboundTable = table([
    { key: 'mtype', title: 'Type' }, { key: 'mcode', title: 'Code' }, { key: 'mname', title: 'Name' },
    { key: 'status', title: 'Status', badge: (r) => (r.status === 'APPLIED' ? 'ok' : 'warn') },
    { key: 'receivedat', title: 'Received' },
  ], inbound);
  [...(inboundTable.querySelector('tbody')?.rows || [])].forEach((tr, i) => {
    const row = inbound[i];
    tr.append(h('td', {}, row.status === 'APPLIED' ? '—' : h('button', { class: 'btn small', onclick: async () => {
      try { await api.applyTallyInbound(row.id); ctx.toast('Master applied', 'ok'); ctx.navigate(ctx.state.route); }
      catch (e) { ctx.toast(e.message, 'error'); }
    } }, 'Apply')));
  });
  if (inbound.length) inboundTable.querySelector('thead tr').append(h('th', {}, ''));

  // --- log ---
  const logTable = table([
    { key: 'loggedat', title: 'When' }, { key: 'direction', title: 'Dir' }, { key: 'action', title: 'Action' },
    { key: 'status', title: 'Status', badge: (r) => TALLY_STATUS_BADGE[r.status] || (r.status === 'APPLIED' ? 'ok' : '') },
    { key: 'message', title: 'Message' },
  ], report.logs);

  return h('div', {},
    h('div', { class: 'card' },
      h('h3', {}, 'Remote Tally Sync'),
      h('p', { class: 'muted' }, 'Connectionless twin-agent sync: the shop agent pushes sales and purchase vouchers to the cloud broker for your accountant\'s Tally Prime, and supported ledger / stock-item masters flow back to the counter. Billing and printing keep working offline; pending vouchers wait in the local queue and deliver when the connection returns.')),
    h('div', { class: 'card' },
      h('div', { class: 'section-title' }, 'Status'),
      stats,
      actions),
    liveCard,
    h('div', { class: 'card' },
      h('h3', {}, 'Agent settings'),
      cfgForm,
      h('div', { class: 'actions' }, tokenBtn),
      h('p', { class: 'muted' }, `Last pull: ${cfg.lastpullat || '—'} · Last push: ${cfg.lastpushat || '—'}`)),
    h('div', { class: 'card' },
      h('h3', {}, 'Outbound queue'),
      queueTable),
    h('div', { class: 'card' },
      h('div', { class: 'section-title' }, 'Voucher XML'),
      h('div', { class: 'toolbar' }, xmlBtn),
      xmlOut),
    h('div', { class: 'card' },
      h('h3', {}, 'Ledger / tax auto-mapping'),
      h('p', { class: 'muted' }, 'Map POS values to Tally masters by string pattern (matched against the POS code or name).'),
      mapForm, mappingTable),
    h('div', { class: 'card' },
      h('h3', {}, 'Masters received from Tally'),
      inboundTable),
    h('div', { class: 'card' },
      h('div', { class: 'section-title' }, 'Sync log'),
      logTable));
}

// --------------------------------------------------------------- excise sync
const EXCISE_STATUS_BADGE = { PENDING: '', SENT: 'warn', ACKED: 'ok', FAILED: 'error' };

async function exciseSyncScreen(ctx) {
  const [cfg, adapters, summary, queue, unmatched, inbound, report] = await Promise.all([
    api.exciseConfig(), api.exciseAdapters(), api.exciseSummary(), api.exciseQueue(),
    api.exciseUnmatched(), api.exciseInbound(), api.exciseReport(),
  ]);

  // --- configuration ---
  const stateSel = select('state', adapters.map((a) => ({ value: a.state, label: a.label })), cfg.state || 'WB');
  const autoupload = h('input', { type: 'checkbox', name: 'autoupload' });
  autoupload.checked = !!cfg.autoupload;
  const cfgForm = form([
    [['State', stateSel], ['Licensee ID', input('licenseeid', cfg.licenseeid || '')]],
    [['Endpoint base', input('endpointbase', cfg.endpointbase || '')],
     ['Credential', h('input', { name: 'credential', type: 'password', value: cfg.credential || '' })]],
    [['Auto-upload when online', autoupload]],
  ], async (d) => {
    await api.saveExciseConfig({ ...d, autoupload: autoupload.checked });
    ctx.toast('Excise settings saved', 'ok');
    ctx.navigate(ctx.state.route);
  });

  // --- status + actions ---
  const stat = (label, value, kind = '') => h('div', { class: 'card stat' },
    h('div', { class: 'k' }, label), h('div', { class: `v${kind ? ` ${kind}` : ''}` }, String(value)));
  const stats = h('div', { class: 'grid cols-4' },
    stat('Pending', summary.PENDING, summary.PENDING ? 'warn' : ''),
    stat('Sent to portal', summary.SENT, summary.SENT ? 'warn' : ''),
    stat('Uploaded', summary.ACKED),
    stat('Failed', summary.FAILED, summary.FAILED ? 'danger' : ''));
  const stats2 = h('div', { class: 'grid cols-4' },
    stat('Unmatched', unmatched.length, unmatched.length ? 'danger' : ''),
    stat('Total', summary.total));

  const act = (label, fn, cls = 'btn') => {
    const b = h('button', { class: cls }, label);
    b.onclick = async () => {
      try { const r = await fn(); ctx.toast(typeof r === 'string' ? r : `${label} done`, 'ok'); ctx.navigate(ctx.state.route); }
      catch (e) { ctx.toast(e.message, 'error'); }
    };
    return b;
  };
  const adapter = adapters.find((a) => a.state === cfg.state) || adapters[0] || {};
  const isPortal = adapter.mode === 'portal';
  const actions = h('div', { class: 'toolbar' },
    act('Capture pending bills', async () => { const r = await api.exciseCapture(); return `${r.captured} line(s) captured`; }, 'btn primary'),
    act(isPortal ? 'Stage for portal agent' : 'Upload to portal', async () => {
      const r = await api.exciseUpload();
      return isPortal ? `${r.staged} staged for the portal agent` : `${r.acked} uploaded, ${r.failed} failed, ${r.skipped} skipped`;
    }),
    act('Retry failed', async () => { const r = await api.exciseRetry(); return `${r.retried} requeued`; }),
    act('Refresh', async () => ''));

  // --- portal batch (portal-mode states, e.g. Odisha) ---
  let portalCard = null;
  if (isPortal) {
    const dl = h('button', { class: 'btn' }, 'Export batch (JSON)');
    dl.onclick = async () => {
      try {
        const batch = await api.excisePortalBatch();
        const blob = new Blob([JSON.stringify(batch, null, 2)], { type: 'application/json' });
        const url = URL.createObjectURL(blob);
        const a = h('a', { href: url, download: `excise-portal-${cfg.state}.json` });
        a.click(); URL.revokeObjectURL(url);
        ctx.toast(`${batch.count} line(s) exported`, 'ok');
      } catch (e) { ctx.toast(e.message, 'error'); }
    };
    const conf = h('button', { class: 'btn primary' }, 'Confirm all sent as uploaded');
    conf.onclick = async () => {
      try {
        const ids = queue.filter((r) => r.status === 'SENT').map((r) => r.id);
        if (!ids.length) { ctx.toast('Nothing staged', 'warn'); return; }
        const r = await api.confirmExcisePortal({ results: ids.map((id) => ({ id, status: 'ACKED' })) });
        ctx.toast(`${r.acked} confirmed`, 'ok'); ctx.navigate(ctx.state.route);
      } catch (e) { ctx.toast(e.message, 'error'); }
    };
    portalCard = h('div', { class: 'card' },
      h('h3', {}, `Portal agent — ${adapter.label}`),
      h('p', { class: 'muted' }, `This state has no direct JSON API. Sales and closing-balance lines are staged (status SENT) and submitted through the state excise portal (${adapter.defaultBase}) by the shop-side agent, which signs in at the login page and uploads the batch. Confirm here once the portal shows the lines accepted.`),
      h('div', { class: 'toolbar' }, dl, conf));
  }

  // --- closing balance ---
  const cbDate = input('date', today(), 'date');
  const cbBtn = h('button', { class: 'btn' }, 'Build closing balance');
  cbBtn.onclick = async () => {
    try { const r = await api.buildExciseClosingBalance(cbDate.value); ctx.toast(`${r.lines} closing line(s) built`, 'ok'); ctx.navigate(ctx.state.route); }
    catch (e) { ctx.toast(e.message, 'error'); }
  };
  const cbUpBtn = h('button', { class: 'btn' }, 'Upload closing balance');
  cbUpBtn.onclick = async () => {
    try {
      const r = await api.uploadExciseClosingBalance(cbDate.value);
      const msg = r.mode === 'portal' ? `${r.staged} line(s) staged for the portal agent` : `${r.acked} uploaded, ${r.failed} failed`;
      ctx.toast(msg, 'ok'); ctx.navigate(ctx.state.route);
    }
    catch (e) { ctx.toast(e.message, 'error'); }
  };

  // --- tables ---
  const queueTable = table([
    { key: 'id', title: '#' }, { key: 'trndate', title: 'Date' }, { key: 'gtin', title: 'GTIN' },
    { key: 'packsize', title: 'Pack' }, { key: 'mrp', title: 'MRP', money: true },
    { key: 'quantity', title: 'Qty', num: true }, { key: 'attempts', title: 'Tries', num: true },
    { key: 'status', title: 'Status', badge: (r) => EXCISE_STATUS_BADGE[r.status] || '' },
    { key: 'lasterror', title: 'Last error' },
  ], queue);

  const unmatchedTable = table([
    { key: 'sourcetrnno', title: 'Bill' }, { key: 'gtin', title: 'GTIN' },
    { key: 'packsize', title: 'Pack' }, { key: 'mrp', title: 'MRP', money: true },
    { key: 'quantity', title: 'Qty', num: true },
  ], unmatched);

  const inboundTable = table([
    { key: 'doctype', title: 'Type' }, { key: 'docno', title: 'Doc' }, { key: 'docdate', title: 'Date' },
    { key: 'tppassno', title: 'TP pass' },
    { key: 'status', title: 'Status', badge: (r) => (r.status === 'APPLIED' ? 'ok' : 'warn') },
  ], inbound);
  [...(inboundTable.querySelector('tbody')?.rows || [])].forEach((tr, i) => {
    const row = inbound[i];
    tr.append(h('td', {}, row.status === 'APPLIED' ? '—' : h('button', { class: 'btn small', onclick: async () => {
      try { await api.applyExciseInbound(row.id); ctx.toast('Applied into purchase', 'ok'); ctx.navigate(ctx.state.route); }
      catch (e) { ctx.toast(e.message, 'error'); }
    } }, 'Apply')));
  });
  if (inbound.length) inboundTable.querySelector('thead tr').append(h('th', {}, ''));

  const logTable = table([
    { key: 'loggedat', title: 'When' }, { key: 'direction', title: 'Dir' }, { key: 'action', title: 'Action' },
    { key: 'status', title: 'Status', badge: (r) => EXCISE_STATUS_BADGE[r.status] || '' },
    { key: 'message', title: 'Message' },
  ], report.logs);

  return h('div', {},
    h('div', { class: 'card' },
      h('h3', {}, 'State Excise Compliance Sync'),
      h('p', { class: 'muted' }, 'Offline-first excise compliance. Every sale and return line is queued locally by its excise identity (GTIN / pack size / MRP) at billing; pending lines upload to the state EODB API when the connection is available. Daily closing balances are pushed back to the portal, and indents / transport permits pulled from the portal are reviewed and applied here. Billing and printing never depend on the network.')),
    h('div', { class: 'card' },
      h('div', { class: 'section-title' }, 'Status'),
      stats,
      stats2,
      actions),
    portalCard,
    h('div', { class: 'card' },
      h('h3', {}, 'State settings'),
      cfgForm,
      h('p', { class: 'muted' }, `Last upload: ${cfg.lastuploadat || '—'}`)),
    h('div', { class: 'card' },
      h('h3', {}, 'Outbound queue'),
      queueTable),
    h('div', { class: 'card' },
      h('h3', {}, 'Pre-validation gate — unmatched lines'),
      h('p', { class: 'muted' }, 'Lines whose GTIN + pack size + MRP no longer match the item master. These are skipped on upload until corrected.'),
      unmatchedTable),
    h('div', { class: 'card' },
      h('h3', {}, 'Closing balance'),
      h('div', { class: 'toolbar' }, field('Date', cbDate), cbBtn, cbUpBtn)),
    h('div', { class: 'card' },
      h('h3', {}, 'Portal documents received'),
      inboundTable),
    h('div', { class: 'card' },
      h('div', { class: 'section-title' }, 'Sync log'),
      logTable));
}

// --------------------------------------------------------------- Form M-22
async function formM22Screen(ctx) {
  const docs = await api.formM22();
  const from = input('from', ctx.state.meta?.yearStart || `${today().slice(0, 4)}-04-01`, 'date');
  const to = input('to', today(), 'date');
  const buildBtn = h('button', { class: 'btn primary' }, 'Build M-22');
  buildBtn.onclick = async () => {
    try {
      const doc = await api.buildFormM22(from.value, to.value);
      ctx.toast(`M-22 #${doc.id} built (${doc.details.length} brand(s))`, 'ok');
      ctx.navigate(ctx.state.route);
    } catch (e) { ctx.toast(e.message, 'error'); }
  };

  const preview = h('pre', { class: 'card', style: 'white-space:pre-wrap;max-height:400px;overflow:auto;margin:0' }, 'Select a statement to preview.');

  const docsTable = table([
    { key: 'id', title: '#' }, { key: 'periodfrom', title: 'From' }, { key: 'periodto', title: 'To' },
    { key: 'openingqty', title: 'Opening', num: true }, { key: 'receivedqty', title: 'Received', num: true },
    { key: 'soldqty', title: 'Sold', num: true }, { key: 'breakageqty', title: 'Breakage', num: true },
    { key: 'closingqty', title: 'Closing', num: true },
    { key: 'status', title: 'Status', badge: (r) => (r.status === 'FILED' ? 'ok' : 'warn') },
  ], docs);
  [...(docsTable.querySelector('tbody')?.rows || [])].forEach((tr, i) => {
    const row = docs[i];
    const show = h('button', { class: 'btn small', onclick: async () => {
      try { const r = await api.printFormM22(row.id); preview.textContent = r.text; } catch (e) { ctx.toast(e.message, 'error'); }
    } }, 'View');
    const file = h('button', { class: 'btn small', onclick: async () => {
      try { await api.fileFormM22(row.id); ctx.toast('Statement marked filed', 'ok'); ctx.navigate(ctx.state.route); }
      catch (e) { ctx.toast(e.message, 'error'); }
    } }, 'File');
    tr.append(h('td', {}, row.status === 'FILED' ? show : h('span', {}, show, ' ', file)));
  });
  if (docs.length) docsTable.querySelector('thead tr').append(h('th', {}, ''));

  return h('div', {},
    h('div', { class: 'card' },
      h('h3', {}, 'Form M-22 — Brand Register'),
      h('p', { class: 'muted' }, 'The state excise brand register: brand-wise opening balance, receipts, issues (sales) and breakage, closing with the closing balance, for the selected period. Figures are derived from the same movements as the Daily Stock Book, so the two always reconcile.')),
    h('div', { class: 'card' },
      h('div', { class: 'section-title' }, 'Build a statement'),
      h('div', { class: 'toolbar' }, field('From', from), field('To', to), buildBtn)),
    h('div', { class: 'card' },
      h('div', { class: 'section-title' }, 'Statements'),
      docsTable),
    h('div', { class: 'card' },
      h('div', { class: 'section-title' }, 'Statement preview'),
      preview));
}

// --------------------------------------------------------------- GST
async function gstScreen(ctx) {
  const [cfg, rates, returns, logs] = await Promise.all([
    api.gstConfig(), api.gstRates(), api.gstReturns(), api.gstLogs(),
  ]);

  const period = today().slice(0, 7);
  const pIn = input('period', period, 'month');
  const fromIn = input('from', '', 'date');
  const toIn = input('to', '', 'date');
  const retSel = select('rettype', ['GSTR1', 'GSTR3B'], 'GSTR1');

  const out = h('div', {});
  const runSlab = async () => {
    const rows = await api.gstRateSlab({ from: fromIn.value, to: toIn.value });
    out.replaceChildren(
      h('div', { class: 'section-title' }, 'GST rate slab (legacy USP_NAIGstReport)'),
      table([
        { key: 'rate', title: 'Rate %', num: true },
        { key: 'qty', title: 'Qty', num: true },
        { key: 'taxable', title: 'Taxable', money: true, num: true },
        { key: 'cgst', title: 'CGST', money: true, num: true },
        { key: 'sgst', title: 'SGST', money: true, num: true },
        { key: 'igst', title: 'IGST', money: true, num: true },
        { key: 'cess', title: 'Cess', money: true, num: true },
        { key: 'addcess', title: 'Add. Cess', money: true, num: true },
      ], rows));
  };
  const runGstr1 = async () => {
    const g = await api.gstr1({ from: fromIn.value, to: toIn.value });
    out.replaceChildren(
      h('div', { class: 'section-title' }, `GSTR-1 outward supplies — ${g.count} line(s)`),
      h('div', { class: 'grid cols-3' },
        statCard('Taxable', money(g.totals.taxable)), statCard('CGST', money(g.totals.cgst)),
        statCard('SGST', money(g.totals.sgst)), statCard('IGST', money(g.totals.igst)),
        statCard('Cess', money(g.totals.cess))),
      table([
        { key: 'invoiceno', title: 'Invoice' }, { key: 'invoicedate', title: 'Date' },
        { key: 'party', title: 'Party' }, { key: 'partygstin', title: 'GSTIN' },
        { key: 'itemname', title: 'Item' }, { key: 'qty', title: 'Qty', num: true },
        { key: 'taxable', title: 'Taxable', money: true, num: true },
        { key: 'gstp', title: 'Rate', num: true },
        { key: 'cgst', title: 'CGST', money: true, num: true },
        { key: 'sgst', title: 'SGST', money: true, num: true },
        { key: 'igst', title: 'IGST', money: true, num: true },
      ], g.rows));
  };
  const runGstr3b = async () => {
    const g = await api.gstr3b({ from: fromIn.value, to: toIn.value });
    out.replaceChildren(
      h('div', { class: 'section-title' }, 'GSTR-3B summary'),
      h('div', { class: 'grid cols-3' },
        statCard('Outward taxable', money(g.outward.taxable)),
        statCard('Output CGST', money(g.outward.cgst)),
        statCard('Output SGST', money(g.outward.sgst)),
        statCard('Output IGST', money(g.outward.igst)),
        statCard('Input tax credit', money(g.itc.cgst + g.itc.sgst + g.itc.igst)),
        statCard('Net CGST payable', money(g.netPayable.cgst)),
        statCard('Net SGST payable', money(g.netPayable.sgst)),
        statCard('Net IGST payable', money(g.netPayable.igst))),
      h('div', { class: 'section-title' }, 'Outward supplies by rate'),
      table([
        { key: 'gstp', title: 'Rate %', num: true },
        { key: 'taxable', title: 'Taxable', money: true, num: true },
        { key: 'cgst', title: 'CGST', money: true, num: true },
        { key: 'sgst', title: 'SGST', money: true, num: true },
        { key: 'igst', title: 'IGST', money: true, num: true },
        { key: 'cess', title: 'Cess', money: true, num: true },
      ], g.byRate));
  };

  const cfgForm = form([
    [['GSTIN', input('gstin', cfg.gstin || '')], ['State Code', input('statecode', cfg.statecode || '')]],
    [['Legal Name', input('legalname', cfg.legalname || '')], ['Trade Name', input('tradename', cfg.tradename || '')]],
    [['GSP Endpoint', input('endpointbase', cfg.endpointbase || '')], ['Client ID', input('clientid', cfg.clientid || '')]],
    [['Client Secret', input('clientsecret', cfg.clientsecret || '')], ['Auto-file', select('autofile', ['No', 'Yes'], cfg.autofile ? 'Yes' : 'No')]],
  ], async (d) => {
    try {
      await api.saveGstConfig({ ...d, autofile: d.autofile === 'Yes' });
      ctx.toast('GST settings saved', 'ok');
    } catch (e) { ctx.toast(e.message, 'error'); }
  });

  const retTable = table([
    { key: 'rettype', title: 'Return' }, { key: 'period', title: 'Period' },
    { key: 'status', title: 'Status', badge: (r) => (r.status === 'FILED' ? 'ok' : '') },
    { key: 'taxable', title: 'Taxable', money: true, num: true },
    { key: 'taxamount', title: 'Tax', money: true, num: true },
    { key: 'arn', title: 'ARN' }, { key: 'filedat', title: 'Filed' },
  ], returns);
  [...retTable.querySelectorAll('tbody tr')].forEach((tr, i) => {
    const r = returns[i];
    tr.append(h('td', {},
      h('button', {
        class: 'btn small',
        onclick: async () => { const j = await api.gstReturnJson(r.id); ctx.openModal(`JSON — ${r.rettype} ${r.period}`,
          h('pre', { class: 'json' }, JSON.stringify(j.payload, null, 2))); },
      }, 'JSON'), ' ',
      r.status === 'FILED' ? '' : h('button', {
        class: 'btn small primary',
        onclick: async () => { await api.fileGstReturn(r.id, {}); ctx.toast('Return filed', 'ok'); ctx.navigate('gst'); },
      }, 'File')));
  });

  return h('div', {},
    h('div', { class: 'card' },
      h('h3', {}, 'GST'),
      h('p', { class: 'muted' }, 'Item GST rates are captured at billing; GSTIN prints on the tax invoice. Generate GSTR-1 / GSTR-3B for a period, inspect the e-filing JSON and record the filing.')),
    h('div', { class: 'card' },
      h('h3', {}, 'Company GST settings'),
      cfgForm),
    h('div', { class: 'card' },
      h('h3', {}, 'GST reports'),
      h('div', { class: 'row three' },
        field('From', fromIn), field('To', toIn),
        h('div', { class: 'actions' },
          h('button', { class: 'btn', onclick: runSlab }, 'Rate slab'),
          ' ',
          h('button', { class: 'btn', onclick: runGstr1 }, 'GSTR-1'),
          ' ',
          h('button', { class: 'btn', onclick: runGstr3b }, 'GSTR-3B'))),
      out),
    h('div', { class: 'card' },
      h('h3', {}, 'Generate a return'),
      form([
        [['Return type', retSel], ['Period', pIn]],
      ], async (d) => {
        try {
          const r = await api.buildGstReturn({ rettype: d.rettype, period: d.period });
          ctx.toast(`${r.rettype} ${r.period} generated (tax ₹${money(r.taxamount)})`, 'ok');
          ctx.navigate('gst');
        } catch (e) { ctx.toast(e.message, 'error'); }
      }),
      h('div', { class: 'section-title' }, 'Returns'),
      retTable),
    h('div', { class: 'card' },
      h('h3', {}, 'Item GST rate card'),
      table([
        { key: 'itemcode', title: 'Code' }, { key: 'itemname', title: 'Item' },
        { key: 'source', title: 'Master' },
        { key: 'cgst', title: 'CGST %', num: true }, { key: 'sgst', title: 'SGST %', num: true },
        { key: 'igst', title: 'IGST %', num: true }, { key: 'cess', title: 'Cess %', num: true },
        { key: 'gstp', title: 'Total %', num: true },
        { key: 'gsttype', title: 'Type', value: (r) => (r.gsttype ? 'Inclusive' : 'Exclusive'), badge: (r) => (r.gsttype ? 'ok' : '') },
      ], rates)),
    h('div', { class: 'card' },
      h('h3', {}, 'Filing log'),
      table([
        { key: 'createdat', title: 'When' }, { key: 'action', title: 'Action' },
        { key: 'returnid', title: 'Return' }, { key: 'detail', title: 'Detail' },
      ], logs)));
}

// --------------------------------------------------------------- help
function aboutScreen() {
  return h('div', { class: 'card' }, h('h3', {}, 'About VanijLiquor'),
    h('p', {}, 'Bar & Liquor POS — modern rebuild.'),
    h('p', { class: 'muted' }, 'Frontend: Tauri (Web + Rust). Backend: Node.js + SQLite. All business flows from the legacy Windows app are covered.'));
}

// --------------------------------------------------------------- licence
const LICENCE_STATE_LABEL = {
  none: 'No licence', valid: 'Licensed', grace: 'In grace period',
  expired: 'Expired', invalid: 'Invalid licence',
};

/** Licence status, machine fingerprint (to paste in the portal) and activation. */
async function licenseScreen(ctx) {
  const st = await api.license();
  const fp = await api.licenseFingerprint();

  const statusCard = card('Licence status',
    h('div', { class: 'licence-status' },
      h('span', { class: `badge ${st.severity === 'ok' ? 'ok' : st.severity === 'warn' ? 'warn' : 'bad'}` },
        LICENCE_STATE_LABEL[st.state] || st.state),
      st.present && st.kind === 'demo' ? h('span', { class: 'pill' }, 'Demo') : null),
    st.present ? h('div', { class: 'kv' },
      kv('Licence ID', st.license_id),
      kv('Plan', st.plan || '—'),
      kv('Valid from', st.valid_from || '—'),
      kv('Valid until', st.valid_until || '—'),
      kv('Days remaining', st.state === 'valid' ? String(st.days_remaining) : (st.state === 'grace' ? `${st.grace_remaining} (grace)` : '0')),
      kv('Signature', st.signature_ok ? 'verified' : 'INVALID'),
      kv('Encrypted at rest', st.sealed ? 'yes (vendor-readable only)' : 'no'),
      kv('Machine', st.machine_ok ? 'matches' : 'different machine')) : null,
    st.state === 'none'
      ? h('p', { class: 'muted' }, st.trial_started
        ? (st.trial_expired
          ? 'No licence is active and the 90-day demo has already been used on this machine. Import a licence issued by the customer portal to continue.'
          : `No licence is active. The demo is already running on this machine (${st.trial_days_remaining} day(s) left), or import a licence issued by the customer portal.`)
        : 'No licence is active. Start the 90-day demo below, or import a licence issued by the customer portal.')
      : null,
    st.severity !== 'ok'
      ? h('p', { class: 'error' }, st.state === 'expired'
        ? 'This licence has expired. Import a renewed licence to continue posting.'
        : st.state === 'grace'
          ? 'This licence is past its term and running in the grace window. Renew soon.'
          : st.state === 'invalid'
            ? 'This licence failed verification (tampered or issued by another vendor).'
            : 'A valid licence is required to post transactions.')
      : null);

  // Demo activation (one-time per machine).
  const used = st.trial_started;
  const left = st.trial_days_remaining;
  const demoBody = used
    ? (st.trial_expired
      ? h('p', { class: 'error' }, 'The 90-day demo has already been used on this machine and has ended. Import a licence to keep posting.')
      : h('p', { class: 'muted' }, `The demo has already been used on this machine — ${left} day(s) remaining of the original 90-day trial. Reinstalling or deleting the database will not restart it.`))
    : h('p', { class: 'muted' }, 'The bundled demo unlocks every module for 90 days. It can be started once per machine and cannot be restarted.');
  const demoCard = card('90-day demo',
    demoBody,
    h('div', { class: 'actions' }, h('button', {
      class: 'btn primary',
      disabled: used,
      onclick: async (e) => {
        e.target.disabled = true;
        try { await api.activateDemo(); ctx.toast('Demo licence activated', 'ok'); await ctx.navigate('license'); }
        catch (err) { ctx.toast(err.message, 'bad'); e.target.disabled = false; }
      },
    }, used ? (st.trial_expired ? 'Demo used up' : 'Demo already used') : 'Start the 90-day demo')));

  // Import a portal-issued licence.
  const picker = filePicker('.vlic,.json,application/json');
  const importCard = card('Import a licence file',
    h('p', { class: 'muted' }, 'Paste or choose the .vlic file the customer portal issued for this machine.'),
    h('label', { class: 'field' }, 'Licence file (.vlic)',
      picker.el),
    h('div', { class: 'actions' }, h('button', {
      class: 'btn',
      onclick: async (e) => {
        const raw = picker.state.text;
        if (!raw) { ctx.toast('Choose a licence file first', 'bad'); return; }
        e.target.disabled = true;
        try {
          let doc; try { doc = JSON.parse(raw); } catch { throw new Error('That file is not valid JSON'); }
          await api.importLicense(doc);
          ctx.toast('Licence imported', 'ok');
          await ctx.navigate('license');
        } catch (err) { ctx.toast(err.message, 'bad'); e.target.disabled = false; }
      },
    }, 'Import licence')));

  // Fingerprint to paste into the portal.
  const fpCard = card('Machine fingerprint',
    h('p', { class: 'muted' }, 'Paste this into the portal under “Issue Licence” so the licence binds to this PC.'),
    h('div', { class: 'kv' },
      kv('Machine hash', fp.machine_hash),
      kv('MAC', (fp.macs || []).join(', ') || '—'),
      kv('CPU', fp.cpu || '—'),
      kv('Disk', fp.disk || '—'),
      kv('Host', fp.hostname || '—'),
      kv('OS', fp.os || '—')),
    h('div', { class: 'actions' }, h('button', {
      class: 'btn ghost small',
      onclick: () => navigator.clipboard?.writeText(JSON.stringify(fp, null, 2)).then(() => ctx.toast('Copied fingerprint', 'ok'), () => {}),
    }, 'Copy JSON')));

  return h('div', {}, statusCard, demoCard, importCard, fpCard);
}

const kv = (k, v) => h('div', { class: 'kv-row' }, h('span', { class: 'k' }, k), h('span', { class: 'v' }, String(v == null ? '—' : v)));

function helpScreen() {
  return h('div', { class: 'card' }, h('h3', {}, 'Ask for Help'),
    h('p', {}, 'Use the sidebar to reach every screen. Key flows:'),
    h('ul', {}, h('li', {}, 'Transactions → Sale – POS for billing'),
      h('li', {}, 'Reports → any group for the full report catalog'),
      h('li', {}, 'Admin → User to manage PINs and rights')));
}
function updatesScreen() {
  return h('div', { class: 'card' }, h('h3', {}, 'List of Updates'),
    h('ul', {}, h('li', {}, 'Web/Tauri frontend replacing the terminal UI'),
      h('li', {}, '101-table schema, full report catalog (122 reports)'),
      h('li', {}, 'GST: item rates, per-line capture, GSTR-1 / GSTR-3B returns and filing'),
      h('li', {}, 'Docker Compose for backend + frontend')));
}

// --------------------------------------------------- pack sizes / state master

/** A file input that exposes its bytes as text and base64 for API calls. */
function filePicker(accept) {
  const el = h('input', { type: 'file', accept });
  const state = { name: '', text: '', base64: '' };
  el.addEventListener('change', () => {
    const f = el.files && el.files[0];
    if (!f) return;
    state.name = f.name;
    const reader = new FileReader();
    reader.onload = () => {
      const buf = new Uint8Array(reader.result);
      state.text = new TextDecoder().decode(buf);
      let bin = '';
      for (let i = 0; i < buf.length; i++) bin += String.fromCharCode(buf[i]);
      state.base64 = btoa(bin);
    };
    reader.readAsArrayBuffer(f);
  });
  return { el, state };
}

const card = (title, ...body) => h('div', { class: 'card' }, title ? h('h3', {}, title) : null, ...body);

async function packSizesScreen(ctx) {
  const rows = await api.packSizes();
  const t = table([
    { key: 'packname', title: 'Pack' },
    { key: 'ml', title: 'ML', num: true },
    { key: 'abbr', title: 'Abbr' },
    { key: 'sortorder', title: 'Order', num: true },
  ], rows);
  const tbody = t.querySelector('tbody');
  if (tbody) [...tbody.rows].forEach((tr, i) => {
    tr.append(h('td', {}, h('button', {
      class: 'btn small danger',
      onclick: async () => {
        if (!confirm(`Delete pack size ${rows[i].packname} ${rows[i].ml}ml?`)) return;
        try { await api.deletePackSize(rows[i].id); ctx.toast('Deleted', 'ok'); ctx.navigate('pack-sizes'); }
        catch (e) { ctx.toast(e.message, 'error'); }
      },
    }, 'Del')));
  });

  const nameI = input('packname', '');
  const mlI = input('ml', '');
  const abbrI = input('abbr', '');
  const add = h('button', { class: 'btn primary' }, '＋ Add pack size');
  add.onclick = async () => {
    try { await api.createPackSize({ packname: nameI.value, ml: mlI.value, abbr: abbrI.value }); ctx.toast('Added', 'ok'); ctx.navigate('pack-sizes'); }
    catch (e) { ctx.toast(e.message, 'error'); }
  };
  return h('div', {},
    card('Government pack sizes (Quart / Pint / Nip)',
      h('p', { class: 'muted' }, 'The standard Indian liquor pack sizes, keyed to their millilitre figure. Used to name a bottle’s pack size from its ML and to seed the state master list.'),
      h('div', { class: 'row three' }, field('Pack name', nameI), field('ML', mlI), field('Abbreviation', abbrI)),
      h('div', { class: 'actions' }, add)),
    card(null, t));
}

async function stateMasterScreen(ctx) {
  const datasets = await api.stateDatasets().catch(() => []);
  const stateSel = select('state', datasets.map((d) => ({ value: d.state, label: `${d.label} (${d.state}) — ${d.brands} brands` })), datasets[0]?.state);
  const viewState = input('viewstate', datasets[0]?.state || 'WB');

  const loadedRows = h('div', {});
  const summaryBox = h('div', {});
  const loadSummary = async () => {
    const summary = await api.stateSummary().catch(() => []);
    summaryBox.replaceChildren(table([
      { key: 'state', title: 'State' }, { key: 'brands', title: 'Brands', num: true },
      { key: 'rows', title: 'Rows', num: true }, { key: 'loadedat', title: 'Loaded' },
    ], summary));
  };
  const loadTable = async () => {
    const rows = await api.stateMaster({ state: viewState.value.trim() }).catch(() => []);
    loadedRows.replaceChildren(table([
      { key: 'brandname', title: 'Brand' },
      { key: 'categoryname', title: 'Category' },
      { key: 'kflname', title: 'KFL' },
      { key: 'ml', title: 'ML', num: true },
      { key: 'packname', title: 'Pack' },
      { key: 'packing', title: 'Packing', num: true },
      { key: 'mrp', title: 'MRP', money: true, num: true },
      { key: 'rate', title: 'Approved rate', money: true, num: true },
    ], rows));
  };

  const out = h('div', {});
  const load = async () => {
    out.replaceChildren(h('div', { class: 'loading' }, h('span', { class: 'spinner' }), 'Loading…'));
    try {
      const res = await api.loadStateMaster({ state: stateSel.value });
      out.replaceChildren(card('Loaded', h('p', {}, `Loaded ${res.loaded} rows (${res.inserts} new, ${res.updates} updated) for state ${res.state}.`)));
      ctx.toast('State list loaded', 'ok');
      await Promise.all([loadTable(), loadSummary()]);
    } catch (e) { out.replaceChildren(card('Error', h('p', { class: 'error' }, e.message))); ctx.toast(e.message, 'error'); }
  };

  const makeItems = (mode) => async () => {
    out.replaceChildren(h('div', { class: 'loading' }, h('span', { class: 'spinner' }), 'Working…'));
    try {
      const res = mode === 'apply' ? await api.applyStateItems({ state: viewState.value.trim() }) : await api.analyzeStateItems({ state: viewState.value.trim() });
      out.replaceChildren(card(mode === 'apply' ? 'Items created' : 'Dry run',
        h('div', { class: 'grid cols-3' },
          h('div', { class: 'card stat' }, h('div', { class: 'k' }, 'Items'), h('div', { class: 'v' }, num(res.inserts + res.updates))),
          h('div', { class: 'card stat' }, h('div', { class: 'k' }, 'New'), h('div', { class: 'v ok' }, num(res.inserts))),
          h('div', { class: 'card stat' }, h('div', { class: 'k' }, 'Updated'), h('div', { class: 'v warn' }, num(res.updates)))),
        res.samples ? table([
          { key: 'itemcode', title: 'Code' }, { key: 'itemname', title: 'Name' },
          { key: 'ml', title: 'ML', num: true }, { key: 'salesrate', title: 'MRP', money: true, num: true },
          { key: 'purchaseRate', title: 'Rate', money: true, num: true },
        ], res.samples) : null));
      ctx.toast(mode === 'apply' ? 'Item masters created' : 'Dry run complete', 'ok');
    } catch (e) { out.replaceChildren(card('Error', h('p', { class: 'error' }, e.message))); ctx.toast(e.message, 'error'); }
  };

  await loadTable();
  await loadSummary();
  return h('div', {},
    card('Pre-configured state master list',
      h('p', { class: 'muted' }, 'Load the state’s approved brand / size / MRP list in one step (the legacy app had no such catalogue — masters were typed by hand). Refresh by uploading a revised list, then create the matching item masters.'),
      h('div', { class: 'toolbar' }, field('State list', stateSel), h('button', { class: 'btn primary', onclick: load }, 'Load list')),
      h('div', { class: 'row' }, field('Show state', viewState),
        h('div', { class: 'actions' },
          h('button', { class: 'btn ghost', onclick: loadTable }, 'Refresh'),
          h('button', { class: 'btn', onclick: makeItems('analyze') }, 'Create items (dry run)'),
          h('button', { class: 'btn primary', onclick: makeItems('apply') }, 'Create item masters')))),
    out,
    card('Loaded lists', summaryBox),
    card('Approved brands', loadedRows));
}

async function bulkImportScreen(ctx) {
  const meta = await api.bulkTargets().catch(() => ({ targets: [], templates: [] }));
  const history = await api.bulkHistory().catch(() => []);
  const kindSel = select('kind', [
    { value: 'masters', label: 'Master data (item, category, KFL, …)' },
    { value: 'openingstock', label: 'Opening stock' },
  ], 'masters');
  const targetSel = select('target', meta.targets.map((t) => ({ value: t.key, label: t.label })), 'item');
  const fp = filePicker('.xlsx,.csv,.json,.txt,.tsv');
  const textarea = h('textarea', { name: 'content', rows: 8, placeholder: '…or paste CSV / JSON here', style: 'width:100%;font-family:monospace' });
  const templatePre = h('pre', { class: 'muted small', style: 'white-space:pre-wrap' }, '');
  const syncTarget = () => {
    const t = meta.templates.find((x) => x.target === targetSel.value);
    templatePre.textContent = t ? `Expected columns:\n${t.header}` : '';
  };
  targetSel.addEventListener('change', syncTarget); syncTarget();
  kindSel.addEventListener('change', () => { targetSel.disabled = kindSel.value === 'openingstock'; });

  const out = h('div', {});
  const payload = () => {
    const base = { kind: kindSel.value, target: targetSel.value, filename: fp.state.name || undefined };
    if (fp.state.base64) return { ...base, filebase64: fp.state.base64 };
    return { ...base, content: textarea.value, source: textarea.value.trim().startsWith('{') ? 'json' : 'csv' };
  };
  const run = async (which) => {
    out.replaceChildren(h('div', { class: 'loading' }, h('span', { class: 'spinner' }), 'Working…'));
    try {
      const res = which === 'apply' ? await api.bulkApply(payload()) : await api.bulkAnalyze(payload());
      out.replaceChildren(bulkResultCard(which === 'apply' ? `Applied — batch #${res.batchId}` : `Dry run — batch #${res.batchId}`, res));
      ctx.toast(which === 'apply' ? 'Import applied' : 'Dry run complete', 'ok');
    } catch (e) { out.replaceChildren(card('Error', h('p', { class: 'error' }, e.message))); ctx.toast(e.message, 'error'); }
  };

  return h('div', {},
    card('Bulk Master Upload',
      h('p', { class: 'muted' }, 'Load many masters (or opening stock) from an Excel / CSV / JSON file in one go — the legacy app could only bulk-load offers and opening stock through a hidden button. Headings are matched loosely, so “Item Code” / “itemcode” both work. Rows are inserted, or updated when their key already exists; nothing is deleted.'),
      h('div', { class: 'row three' },
        field('What to load', kindSel), field('Target master', targetSel), field('File (.xlsx / .csv / .json)', fp.el)),
      textarea, templatePre,
      h('div', { class: 'actions' },
        h('button', { class: 'btn', onclick: () => run('analyze') }, 'Analyze (dry run)'),
        h('button', { class: 'btn primary', onclick: () => { if (confirm('Write the analyzed rows into this database?')) run('apply'); } }, 'Apply import'))),
    out,
    card('Import history', table([
      { key: 'id', title: '#' }, { key: 'kind', title: 'Kind' }, { key: 'filename', title: 'File' },
      { key: 'status', title: 'Status', badge: (r) => ({ APPLIED: 'ok', ANALYZED: 'warn', FAILED: 'danger' }[r.status] || '') },
      { key: 'inserted', title: 'Ins', num: true }, { key: 'updated', title: 'Upd', num: true }, { key: 'failed', title: 'Fail', num: true },
    ], history)));
}

function bulkResultCard(title, res) {
  const totals = res.totals || {};
  const stat = (label, value, kind = '') => h('div', { class: 'card stat' }, h('div', { class: 'k' }, label), h('div', { class: `v${kind ? ` ${kind}` : ''}` }, String(value ?? 0)));
  return h('div', { class: 'card' },
    h('div', { class: 'section-title' }, `${title} — ${res.table || ''}`),
    h('div', { class: 'grid cols-4' },
      stat('Rows', totals.rows), stat('Inserted', totals.inserts, 'ok'), stat('Updated', totals.updates, 'warn'),
      stat('Failed', totals.failed, totals.failed ? 'danger' : '')),
    (res.errors && res.errors.length)
      ? h('div', {}, h('div', { class: 'section-title' }, 'Row errors'),
        h('pre', { style: 'white-space:pre-wrap;margin:0' }, JSON.stringify(res.errors.slice(0, 30), null, 2)))
      : null);
}

async function parchaImportScreen(ctx) {
  const history = await api.parchaHistory().catch(() => []);
  const ocr = await api.ocrConfig().catch(() => ({}));
  const fp = filePicker('.pdf,.json,.csv,.txt');
  const textarea = h('textarea', { name: 'content', rows: 8, placeholder: 'Paste the invoice text here (or choose a PDF / JSON / CSV above)', style: 'width:100%;font-family:monospace' });
  const createMissing = h('input', { type: 'checkbox', name: 'createMissing', checked: true });
  const setOpeningStock = h('input', { type: 'checkbox', name: 'setOpeningStock' });
  const marginPct = input('marginPct', '');
  const shopcode = input('shopcode', 'MAIN');

  const out = h('div', {});
  const payload = () => {
    const base = {
      filename: fp.state.name || undefined,
      createMissing: createMissing.checked, setOpeningStock: setOpeningStock.checked,
      marginPct: marginPct.value === '' ? null : Number(marginPct.value), shopcode: shopcode.value || 'MAIN',
    };
    if (fp.state.base64) return { ...base, filebase64: fp.state.base64 };
    const text = textarea.value.trim();
    return { ...base, content: text, source: text.startsWith('{') || text.startsWith('[') ? 'json' : 'text' };
  };
  const run = async (which) => {
    out.replaceChildren(h('div', { class: 'loading' }, h('span', { class: 'spinner' }), 'Working…'));
    try {
      const res = which === 'apply' ? await api.parchaApply(payload()) : await api.parchaAnalyze(payload());
      out.replaceChildren(parchaResultCard(which === 'apply' ? `Imported — purchase ${res.trnno || ''}` : `Dry run — batch #${res.batchId}`, res, which === 'apply'));
      ctx.toast(which === 'apply' ? 'Parcha imported' : 'Dry run complete', 'ok');
    } catch (e) { out.replaceChildren(card('Error', h('p', { class: 'error' }, e.message))); ctx.toast(e.message, 'error'); }
  };

  // OCR settings
  const provSel = select('provider', ['stub', 'tesseract', 'external'], ocr.provider || 'stub');
  const endpointI = input('endpoint', ocr.endpoint || '');
  const langI = input('lang', ocr.lang || 'eng');
  const saveOcr = async () => {
    try { await api.saveOcrConfig({ provider: provSel.value, endpoint: endpointI.value, lang: langI.value, enabled: provSel.value !== 'stub' ? 1 : 0 }); ctx.toast('OCR settings saved', 'ok'); }
    catch (e) { ctx.toast(e.message, 'error'); }
  };

  return h('div', {},
    card('Parcha Import (wholesale invoice)',
      h('p', { class: 'muted' }, 'Import a purchase invoice from a PDF, JSON or CSV (or pasted text). Lines are matched to the item master, the purchase is posted, and the landing cost + margin are computed for every line — the gaps the legacy PDF import left. Missing items can be created on the fly, selling prices can be raised from a target margin, and opening stock can be seeded in the same step.'),
      h('div', { class: 'row' }, field('Invoice file (.pdf / .json / .csv)', fp.el)),
      textarea,
      h('div', { class: 'row three' },
        field('Create missing items', h('label', { class: 'inline' }, createMissing, ' yes')),
        field('Seed opening stock', h('label', { class: 'inline' }, setOpeningStock, ' yes')),
        field('Target margin % (optional)', marginPct)),
      h('div', { class: 'row' }, field('Shop / store for opening stock', shopcode)),
      h('div', { class: 'actions' },
        h('button', { class: 'btn', onclick: () => run('analyze') }, 'Analyze (dry run)'),
        h('button', { class: 'btn primary', onclick: () => { if (confirm('Post this invoice as a purchase?')) run('apply'); } }, 'Import invoice'))),
    out,
    card('OCR / document capture',
      h('p', { class: 'muted small' }, `Provider: stub (paste text), tesseract (local CLI${ocr.tesseractAvailable ? ' — available' : ' — not installed'}), or an external HTTP OCR service.`),
      h('div', { class: 'row three' }, field('Provider', provSel), field('Endpoint (external)', endpointI), field('Language', langI)),
      h('div', { class: 'actions' }, h('button', { class: 'btn', onclick: saveOcr }, 'Save OCR settings'))),
    card('Import history', table([
      { key: 'id', title: '#' }, { key: 'source', title: 'Source' }, { key: 'suppliername', title: 'Supplier' },
      { key: 'docno', title: 'Doc' }, { key: 'trnno', title: 'Purchase' }, { key: 'lines', title: 'Lines', num: true },
      { key: 'matched', title: 'Matched', num: true },
      { key: 'status', title: 'Status', badge: (r) => ({ IMPORTED: 'ok', ANALYZED: 'warn', FAILED: 'danger' }[r.status] || '') },
      { key: 'marginpct', title: 'Margin %', num: true },
    ], history)));
}

function parchaResultCard(title, res, applied) {
  const rows = applied
    ? (res.landedcost || []).map((l) => ({ ...l, itemcode: l.itemcode }))
    : (res.entries || []).map((e) => ({ itemcode: e.itemcode, itemname: e.itemname, how: e.how, landingcost: e.landingcost, salesrate: e.salesrate, marginamt: e.marginamt, marginpct: e.marginpct, willCreate: e.willCreate }));
  return h('div', { class: 'card' },
    h('div', { class: 'section-title' }, title),
    res.totals ? h('div', { class: 'grid cols-4' },
      h('div', { class: 'card stat' }, h('div', { class: 'k' }, 'Lines'), h('div', { class: 'v' }, num(res.totals.lines))),
      h('div', { class: 'card stat' }, h('div', { class: 'k' }, 'Matched'), h('div', { class: 'v ok' }, num(res.totals.matched))),
      h('div', { class: 'card stat' }, h('div', { class: 'k' }, 'Avg landing'), h('div', { class: 'v' }, money(res.totals.landingcost))),
      h('div', { class: 'card stat' }, h('div', { class: 'k' }, 'Avg margin %'), h('div', { class: 'v' }, num(res.totals.marginpct)))) : null,
    table([
      { key: 'itemcode', title: 'Code' },
      { key: 'itemname', title: 'Item' },
      { key: 'how', title: 'Match' },
      { key: 'willCreate', title: 'New?', value: (r) => (r.willCreate ? 'yes' : '') },
      { key: 'landingcost', title: 'Landing cost', money: true, num: true },
      { key: 'salesrate', title: 'Sale rate', money: true, num: true },
      { key: 'marginamt', title: 'Margin', money: true, num: true },
      { key: 'marginpct', title: 'Margin %', num: true },
    ], rows));
}

async function landedCostScreen(ctx) {
  const from = input('from', '');
  const to = input('to', '');
  const trnnoI = input('trnno', '');
  const otherI = input('otherCharges', '');
  const itemI = input('itemcode', '');
  const landI = input('landingcost', '');
  const pctI = input('marginPct', '');
  const out = h('div', {});
  const itemsOut = h('div', {});

  const loadReport = async () => {
    out.replaceChildren(h('div', { class: 'loading' }, h('span', { class: 'spinner' }), 'Loading…'));
    try {
      const rows = await api.landedCostReport({ from: from.value, to: to.value });
      out.replaceChildren(table([
        { key: 'trndate', title: 'Date' }, { key: 'trnno', title: 'Purchase' }, { key: 'itemname', title: 'Item' },
        { key: 'qty', title: 'Qty', num: true }, { key: 'freeqty', title: 'Free', num: true },
        { key: 'grossrate', title: 'Rate', money: true, num: true },
        { key: 'landingcost', title: 'Landing cost', money: true, num: true },
        { key: 'salesrate', title: 'Sale rate', money: true, num: true },
        { key: 'marginamt', title: 'Margin', money: true, num: true },
        { key: 'marginpct', title: 'Margin %', num: true },
      ], rows));
    } catch (e) { out.replaceChildren(card('Error', h('p', { class: 'error' }, e.message))); }
  };
  const loadItems = async () => {
    const rows = await api.landedCostItems().catch(() => []);
    itemsOut.replaceChildren(table([
      { key: 'itemname', title: 'Item' }, { key: 'lines', title: 'Lines', num: true }, { key: 'qty', title: 'Qty', num: true },
      { key: 'avgexcost', title: 'Avg landing', money: true, num: true },
      { key: 'avgsale', title: 'Avg sale', money: true, num: true },
      { key: 'avgmargin', title: 'Avg margin', money: true, num: true },
      { key: 'avgmarginpct', title: 'Avg margin %', num: true },
    ], rows));
  };

  const compute = async () => {
    try {
      const r = await api.computeLandedCost(trnnoI.value.trim(), otherI.value === '' ? undefined : Number(otherI.value));
      ctx.toast(`Computed ${r.lines.length} line(s)`, 'ok'); await loadReport();
    } catch (e) { ctx.toast(e.message, 'error'); }
  };
  const applyM = async () => {
    try {
      const r = await api.applyMargin({ itemcode: itemI.value.trim(), landingcost: Number(landI.value), marginPct: pctI.value === '' ? null : Number(pctI.value) });
      ctx.toast(`New sale rate ${money(r.salesrate)}`, 'ok'); await loadItems();
    } catch (e) { ctx.toast(e.message, 'error'); }
  };

  await loadReport(); await loadItems();
  return h('div', {},
    card('Landed cost & margin',
      h('p', { class: 'muted' }, 'The true cost of a bottle once discounts, bill charges and free (scheme) bottles are folded in — and the margin against its selling price. Compute it for a posted purchase, or set an item’s selling price from a target margin.'),
      h('div', { class: 'row three' }, field('From', from), field('To', to),
        h('div', { class: 'actions' }, h('button', { class: 'btn', onclick: loadReport }, 'Refresh')))),
    out,
    card('Compute for a purchase',
      h('div', { class: 'row three' }, field('Purchase no', trnnoI), field('Bill charges (optional)', otherI),
        h('div', { class: 'actions' }, h('button', { class: 'btn primary', onclick: compute }, 'Compute landing cost')))),
    card('Set selling price from a target margin',
      h('div', { class: 'row three' }, field('Item code', itemI), field('Landing cost', landI), field('Margin %', pctI)),
      h('div', { class: 'actions' }, h('button', { class: 'btn primary', onclick: applyM }, 'Apply margin'))),
    card('Item-wise margin summary', itemsOut));
}

// ------------------------------------------------------- group launcher
/** Option B landing screen: a group's items shown as small clickable boxes. */
async function groupLauncher(ctx, route) {
  const group = groupOfRoute(route);
  if (!group) {
    return h('div', { class: 'card' }, h('h3', {}, 'Menu group not found'),
      h('p', { class: 'muted' }, `No group for "${route}".`));
  }
  const active = ctx.state.route;
  const boxes = h('div', { class: 'launcher-grid' },
    ...group.items.map((it) => h('button', {
      class: `launcher-card${it.route === active ? ' active' : ''}`,
      onclick: () => ctx.navigate(it.route),
    },
    h('span', { class: 'launcher-icon' }, navIcon(it.route, group.title)),
    h('span', { class: 'launcher-label' }, it.title),
    h('span', { class: 'launcher-go' }, svgIcon('chevron-right', 16)))));

  return h('div', {},
    h('div', { class: 'launcher-head' },
      h('span', { class: 'launcher-head-icon' }, groupIcon(group.title)),
      h('div', {},
        h('h3', {}, group.title),
        h('p', { class: 'muted small' }, `${group.items.length} item(s) — pick one to open`))),
    boxes);
}


export async function screens(route, ctx) {
  if (isGroupRoute(route)) return groupLauncher(ctx, route);
  if (route === 'dashboard') return dashboard(ctx);
  if (route === 'dash:sales') return dashboard(ctx);
  if (route === 'sysadmin') return systemAdmin(ctx);
  if (route === 'params') return paramsScreen(ctx);
  if (route === 'users') return usersScreen(ctx);
  if (route === 'rights') return usersScreen(ctx);
  if (route === 'new-year') return newYearScreen(ctx);
  if (route === 'excise-audit') return exciseAuditScreen(ctx);
  if (route === 'deleted-bills') return deletedBillsScreen(ctx);
  if (route === 'items') return itemsScreen(ctx);
  if (route === 'schemes') return schemesScreen(ctx);
  if (route === 'inventory') return inventoryScreen(ctx);
  if (route === 'pack-sizes') return packSizesScreen(ctx);
  if (route === 'state-master') return stateMasterScreen(ctx);
  if (route === 'bulk-import') return bulkImportScreen(ctx);
  if (route === 'parcha-import') return parchaImportScreen(ctx);
  if (route === 'landed-cost') return landedCostScreen(ctx);
  if (route.startsWith('master:')) return masterScreen(ctx, route.slice(7));
  if (route.startsWith('reports:')) return reportsGroup(ctx, route.slice(8));
  if (route === 'sale') return posSale(ctx);
  if (route === 'sales-register') return salesRegister(ctx);
  if (route === 'purchase') return purchaseScreen(ctx);
  if (route.startsWith('voucher:')) return voucherScreen(ctx, route.slice(8));
  if (route.startsWith('note:')) return noteScreen(ctx, route.slice(5));
  if (route === 'transfer-sale') return transferSaleScreen(ctx);
  if (route === 'transfer-receive') return transferReceiveScreen(ctx);
  if (route === 'on-account') return onAccountScreen(ctx);
  if (route === 'account-replace') return accountReplaceScreen(ctx);
  if (route === 'fine') return fineScreen(ctx);
  if (route === 'opening-stock') return openingStock(ctx);
  if (route === 'stock-transfer') return stockTransferScreen(ctx);
  if (route === 'breakage') return breakageScreen(ctx);
  if (route === 'brs') return brsScreen(ctx);
  if (route === 'upi') return upiScreen(ctx);
  if (route === 'delivery') return deliveryScreen(ctx);
  if (route === 'dash:stock') return dashGeneric(ctx, 'stock', 'Stock Status');
  if (route === 'dash:purchase') return dashGeneric(ctx, 'purchase', 'Purchase Status');
  if (route === 'dash:scheme') return dashScheme(ctx);
  if (route === 'calc') return calcScreen(ctx);
  if (route === 'change-password') return changePassword(ctx);
  if (route === 'backup') return backupScreen(ctx);
  if (route === 'restore') return restoreScreen(ctx);
  if (route === 'tally') return tallyScreen(ctx);
  if (route === 'tally-sync') return tallySyncScreen(ctx);
  if (route === 'excise-sync') return exciseSyncScreen(ctx);
  if (route === 'form-m22') return formM22Screen(ctx);
  if (route === 'gst') return gstScreen(ctx);
  if (route === 'messages') return messagesScreen(ctx);
  if (route === 'pml') return pmlScreen(ctx);
  if (route === 'clean-print') return cleanPrintScreen(ctx);
  if (route === 'migration') return migrationScreen(ctx);
  if (route === 'sync') return syncScreen(ctx);
  if (route === 'about') return aboutScreen();
  if (route === 'license') return licenseScreen(ctx);
  if (route === 'help') return helpScreen();
  if (route === 'updates') return updatesScreen();
  return h('div', { class: 'card' }, h('h3', {}, 'Not found'), h('p', { class: 'muted' }, `No screen for route "${route}".`));
}
