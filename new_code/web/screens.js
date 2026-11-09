import { api } from './api.js';
import { h, table, form, field, input, select, money, num } from './ui.js';

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
  const stat = (k, v, isMoney) => h('div', { class: 'card stat' },
    h('div', { class: 'k' }, k),
    h('div', { class: `v${isMoney ? ' money' : ''}` }, isMoney ? money(v) : num(v)));
  return h('div', {},
    h('div', { class: 'grid cols-4' },
      stat('Sales Bills', d.sales.bills),
      stat('Sales Net', d.sales.net, true),
      stat('Purchase Bills', d.purchase.bills),
      stat('Purchase Net', d.purchase.net, true)),
    h('div', { class: 'section-title' }, 'Stock'),
    h('div', { class: 'grid cols-4' },
      stat('Items', d.items),
      stat('Stock Lines', d.stockLines),
      stat('Stock Value', d.stockValue, true),
      h('div', { class: 'card' }, h('h3', {}, 'Quick actions'),
        h('div', { class: 'list-menu' },
          h('button', { onclick: () => ctx.navigate('sale') }, '＋ New Sale'),
          h('button', { onclick: () => ctx.navigate('reports:sales') }, 'Sales Reports'),
          h('button', { onclick: () => ctx.navigate('items') }, 'Item Master'))))
  );
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

// --------------------------------------------------------------- admin
async function paramsScreen(ctx) {
  const p = await api.parameters();
  const skip = new Set(['companycode']);
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
  return h('div', { class: 'card' }, h('h3', {}, 'Parameters'), f);
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
  const f = form([
    [['Year Range', input('yearrange', '2025-2026')], ['Start Date', input('startdate', '2025-04-01', 'date')]],
    [['End Date', input('enddate', '2026-03-31', 'date')], ['Year Code', input('yearcode', '2025')]],
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
async function posSale(ctx) {
  const meta = ctx.state.meta || {};
  const items = await api.items();
  const saleTypes = meta.saleTypes || ['FL', 'CS', 'NAI', 'CASH', 'CREDIT', 'RETURN'];
  const lineEd = repeater([
    { name: 'itemcode', label: 'Item', options: items.map((i) => ({ value: i.itemcode, label: `${i.itemcode} · ${i.itemname}` })) },
    { name: 'qnty', label: 'Qty', type: 'number', value: '1' },
    { name: 'rate', label: 'Rate', type: 'number' },
  ], [{ qnty: '1' }]);
  const total = h('span', { class: 'pill' }, 'Total ₹0.00');
  const recalc = () => {
    const sum = lineEd.values().reduce((s, l) => s + (Number(l.qnty) || 0) * (Number(l.rate) || 0), 0);
    total.textContent = `Total ₹${money(sum)}`;
  };
  const f = h('form', { class: 'form' },
    h('div', { class: 'row three' },
      field('Store / Counter', input('storecode', 'CNT1')),
      field('Sale Type', select('salestype', saleTypes, 'FL')),
      field('Party Ledger', input('ledgercode', 'C00001'))),
    h('div', { class: 'row three' },
      field('Date', input('trndate', '2024-04-06', 'date')),
      field('Bill Discount', input('discount', '0', 'number')),
      h('div', {})),
    h('div', { class: 'section-title' }, 'Items'),
    lineEd.node,
    h('div', { class: 'actions' }, total, h('button', { class: 'btn primary', type: 'submit' }, 'Save Sale')));
  f.addEventListener('input', recalc);
  recalc();
  f.addEventListener('submit', async (e) => {
    e.preventDefault();
    const data = formValues(f);
    const lines = lineEd.values().map((l) => ({ itemcode: l.itemcode, qnty: Number(l.qnty) || 1, rate: Number(l.rate) || 0 }));
    if (!lines.length) return ctx.toast('Add at least one item', 'error');
    try {
      const r = await api.createSale({ ...data, discount: Number(data.discount) || 0, lines, userId: ctx.state.user.usercode });
      ctx.openModal('Sale Saved', h('div', { class: 'grid cols-2' },
        statCard('Bill No', r.trnno), statCard('Gross', money(r.amount)),
        statCard('Discount', money(r.discamount)), statCard('Net', money(r.netamount))));
      ctx.toast('Sale saved', 'ok');
    } catch (ex) { ctx.toast(ex.message, 'error'); }
  });
  return h('div', { class: 'card' }, h('h3', {}, 'Sale – POS'), f);
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
      field('Date', input('trndate', '2024-04-05', 'date')),
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
      field('Date', input('vchdate', '2024-04-07', 'date')),
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
      [['Date', input('vchdate', '2024-04-07', 'date')]],
      [['From Ledger', input('from_ledger', 'L00001')], ['To Ledger', input('to_ledger', 'L00002')]],
      [['Amount', input('amount', '0', 'number')]],
    ], async (d) => { try { await api.createVoucher({ type, ...d, amount: Number(d.amount) || 0 }); ctx.toast('Contra posted', 'ok'); } catch (e) { ctx.toast(e.message, 'error'); } });
  } else {
    f = form([
      [['Date', input('vchdate', '2024-04-07', 'date')]],
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
    [['Date', input('trndate', '2024-04-07', 'date')], ['Party Ledger', input('ledcode', 'C00001')]],
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
    [['Quantity', input('qnty', '0', 'number')], ['Date', input('trndate', '2024-04-01', 'date')]],
  ], async (d) => { try { await api.addOpeningStock({ ...d, qnty: Number(d.qnty) || 0 }); ctx.toast('Opening stock saved', 'ok'); } catch (e) { ctx.toast(e.message, 'error'); } });
  return h('div', { class: 'card' }, h('h3', {}, 'Opening Stock'), f);
}

async function stockTransferScreen(ctx) {
  const lineEd = repeater([
    { name: 'itemcode', label: 'Item', value: '' },
    { name: 'qnty', label: 'Qty', type: 'number', value: '1' },
  ], [{ qnty: '1' }]);
  const f = h('form', { class: 'form' },
    h('div', { class: 'row three' }, field('From', input('from', 'MAIN')), field('To', input('to', 'CNT1')), field('Date', input('trndate', '2024-04-08', 'date'))),
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
    h('div', { class: 'row' }, field('Store', input('shopcode', 'MAIN')), field('Date', input('trndate', '2024-04-08', 'date'))),
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
    h('div', { class: 'row three' }, field('From', input('from', 'MAIN')), field('To', input('to', 'CNT1')), field('Date', input('trndate', '2024-04-06', 'date'))),
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
    [['Date', input('trndate', '2024-04-06', 'date')], ['Transfer No', input('trnno', '')]],
    [['Amount', input('amount', '0', 'number')]],
  ], async (d) => { try { await api.createTransferReceive({ ...d, amount: Number(d.amount) || 0 }); ctx.toast('Transfer receive saved', 'ok'); ctx.navigate(ctx.state.route); } catch (e) { ctx.toast(e.message, 'error'); } });
  const rows = await api.transferReceives();
  return h('div', {}, h('div', { class: 'card' }, h('h3', {}, 'Transfer Receive'), f),
    h('div', { class: 'section-title' }, 'Recent'),
    table([{ key: 'trnno', title: 'No' }, { key: 'trndate', title: 'Date' }, { key: 'from', title: 'From' }, { key: 'to', title: 'To' }, { key: 'amount', title: 'Amount', money: true, num: true }], rows));
}

async function onAccountScreen(ctx) {
  const f = form([
    [['Date', input('trndate', '2024-04-06', 'date')], ['Ledger', input('ledcode', 'C00001')]],
    [['Amount', input('amount', '0', 'number')], ['Narration', input('narration', 'On A/C adjustment')]],
  ], async (d) => { try { await api.onAccount({ ...d, amount: Number(d.amount) || 0 }); ctx.toast('Adjustment saved', 'ok'); } catch (e) { ctx.toast(e.message, 'error'); } });
  return h('div', { class: 'card' }, h('h3', {}, 'On A/C Adjustment'), f);
}

async function accountReplaceScreen(ctx) {
  const f = form([
    [['Date', input('trndate', '2024-04-06', 'date')]],
    [['From Ledger', input('from_ledcode', 'C00001')], ['To Ledger', input('to_ledcode', 'C00002')]],
    [['Amount', input('amount', '0', 'number')]],
  ], async (d) => { try { await api.accountReplace({ ...d, amount: Number(d.amount) || 0 }); ctx.toast('Replace saved', 'ok'); } catch (e) { ctx.toast(e.message, 'error'); } });
  return h('div', { class: 'card' }, h('h3', {}, 'A/C Replace'), f);
}

async function fineScreen(ctx) {
  const f = form([
    [['Date', input('trndate', '2024-04-06', 'date')], ['Shop', input('shopcode', 'MAIN')]],
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
    [['Date', input('trndate', '2024-04-06', 'date')], ['Reference', input('reference', '')]],
  ], async (d) => { try { await api.addReconciliation({ ...d, amount: Number(d.amount) || 0 }); ctx.toast('BRS entry saved', 'ok'); ctx.navigate(ctx.state.route); } catch (e) { ctx.toast(e.message, 'error'); } });
  const t = table([
    { key: 'slno', title: 'Sl' }, { key: 'ledcode', title: 'Ledger' }, { key: 'amount', title: 'Amount', money: true, num: true },
    { key: 'trndate', title: 'Date' }, { key: 'cleareddate', title: 'Cleared', badge: (r) => (r.cleareddate ? 'ok' : 'warn') },
  ], rows);
  [...t.querySelector('tbody').rows].forEach((tr, i) => {
    tr.append(h('td', {}, rows[i].cleareddate ? '—' : h('button', {
      class: 'btn small', onclick: async () => { try { await api.clearReconciliation(rows[i].slno, '2024-04-30'); ctx.toast('Cleared', 'ok'); ctx.navigate(ctx.state.route); } catch (e) { ctx.toast(e.message, 'error'); } },
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
    [['Date', input('trndate', '2024-04-06', 'date')], ['Amount', input('amount', '0', 'number')]],
    [['UPI Ref', input('refno', '')], ['Party', input('ledcode', 'C00001')]],
  ], async (d) => { try { await api.recordUpi({ ...d, amount: Number(d.amount) || 0 }); ctx.toast('UPI recorded', 'ok'); ctx.navigate(ctx.state.route); } catch (e) { ctx.toast(e.message, 'error'); } });
  const rows = await api.upi();
  return h('div', {}, h('div', { class: 'card' }, h('h3', {}, 'QRCode / UPI Payment'), f),
    h('div', { class: 'section-title' }, 'Recent'),
    table([{ key: 'trnno', title: 'No' }, { key: 'trndate', title: 'Date' }, { key: 'amount', title: 'Amount', money: true, num: true }, { key: 'refno', title: 'Ref' }], rows));
}

async function deliveryScreen(ctx) {
  const f = form([
    [['Date', input('trndate', '2024-04-06', 'date')], ['Customer', input('custcode', 'CUST1')]],
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
    form([[['Backup file path', input('src', '/data/backup/madhushala.db')]]], async () => {
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
  const f = form([[['Date', input('trndate', '2024-04-06', 'date')], ['Bottles', input('bottles', '0', 'number')]], [['ML', input('ml', '0', 'number')], ['Remarks', input('remarks', '')]]],
    async (d) => { try { await api.addPml(d); ctx.toast('PML entry saved', 'ok'); ctx.navigate(ctx.state.route); } catch (e) { ctx.toast(e.message, 'error'); } });
  return h('div', {}, h('div', { class: 'card' }, h('h3', {}, 'PML Print and Reports'), f),
    h('div', { class: 'section-title' }, 'Entries'), table([{ key: 'slno', title: 'Sl' }, { key: 'trndate', title: 'Date' }, { key: 'bottles', title: 'Bottles', num: true }, { key: 'ml', title: 'ML', num: true }, { key: 'remarks', title: 'Remarks' }], rows));
}

function cleanPrintScreen(ctx) {
  return h('div', { class: 'card' }, h('h3', {}, 'Clean Print Garbage'),
    h('p', { class: 'muted' }, 'Removes orphaned print records for the current company.'),
    h('div', { class: 'actions' }, h('button', { class: 'btn danger', onclick: async () => { try { await api.cleanPrint(); ctx.toast('Print garbage cleaned', 'ok'); } catch (e) { ctx.toast(e.message, 'error'); } } }, 'Run Cleanup')));
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
  const cfgForm = form([
    [['Broker URL', input('brokerurl', cfg.brokerurl || 'https://broker.madhushala.cloud')],
     ['Tally company', input('tallycompany', cfg.tallycompany || '')]],
    [['Agent ID', input('agentid', cfg.agentid || '')],
     ['Company GUID', input('companyguid', cfg.companyguid || '')]],
    [['Agent token', input('agenttoken', cfg.agenttoken || '')],
     ['Enabled', enabled]],
  ], async (d) => {
    await api.saveTallyConfig({ ...d, enabled: enabled.checked });
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
  const cbDate = input('date', new Date().toISOString().slice(0, 10), 'date');
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

// --------------------------------------------------------------- GST
async function gstScreen(ctx) {
  const [cfg, rates, returns, logs] = await Promise.all([
    api.gstConfig(), api.gstRates(), api.gstReturns(), api.gstLogs(),
  ]);

  const period = new Date().toISOString().slice(0, 7);
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
  return h('div', { class: 'card' }, h('h3', {}, 'About Madhushala Pro'),
    h('p', {}, 'Bar & Liquor POS — modern rebuild.'),
    h('p', { class: 'muted' }, 'Frontend: Tauri (Web + Rust). Backend: Node.js + SQLite. All business flows from the legacy Windows app are covered.'));
}
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
      h('li', {}, '90-table schema, full report catalog (119 reports)'),
      h('li', {}, 'GST: item rates, per-line capture, GSTR-1 / GSTR-3B returns and filing'),
      h('li', {}, 'Docker Compose for backend + frontend')));
}

// --------------------------------------------------------------- router
export async function screens(route, ctx) {
  if (route === 'dashboard') return dashboard(ctx);
  if (route === 'sysadmin') return systemAdmin(ctx);
  if (route === 'params') return paramsScreen(ctx);
  if (route === 'users') return usersScreen(ctx);
  if (route === 'rights') return usersScreen(ctx);
  if (route === 'new-year') return newYearScreen(ctx);
  if (route === 'excise-audit') return exciseAuditScreen(ctx);
  if (route === 'deleted-bills') return deletedBillsScreen(ctx);
  if (route === 'items') return itemsScreen(ctx);
  if (route === 'schemes') return schemesScreen(ctx);
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
  if (route === 'gst') return gstScreen(ctx);
  if (route === 'messages') return messagesScreen(ctx);
  if (route === 'pml') return pmlScreen(ctx);
  if (route === 'clean-print') return cleanPrintScreen(ctx);
  if (route === 'sync') return syncScreen(ctx);
  if (route === 'about') return aboutScreen();
  if (route === 'help') return helpScreen();
  if (route === 'updates') return updatesScreen();
  return h('div', { class: 'card' }, h('h3', {}, 'Not found'), h('p', { class: 'muted' }, `No screen for route "${route}".`));
}
