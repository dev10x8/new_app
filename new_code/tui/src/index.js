#!/usr/bin/env node
import { c, clear } from './lib/ansi.js';
import { header, table, kv, message, ask as askRaw, closeInput, EOF } from './lib/widgets.js';
import { api } from './lib/api.js';
import { MENU } from './lib/menu.js';

const state = { user: null };

// Wrap input helpers so that end-of-input (piped/scripted runs) exits cleanly.
async function ask(question, opts) {
  const r = await askRaw(question, opts);
  if (r === EOF) { closeInput(); clear(); process.exit(0); }
  return r;
}
async function pause(text) { return ask(`\n${c.dim}${text || 'Press Enter to continue...'}${c.reset}`); }
async function menu(title, options) {
  clear();
  process.stdout.write(header(title));
  options.forEach((o, i) => process.stdout.write(`  ${c.bold}${String(i + 1).padStart(2)}${c.reset}. ${o.label}\n`));
  process.stdout.write(`  ${c.bold} 0${c.reset}. ${c.dim}Back${c.reset}\n`);
  const ans = await ask(`\n ${c.cyan}Select>${c.reset} `);
  const n = Number(ans);
  if (!n || n < 1 || n > options.length) return null;
  return options[n - 1].value;
}

// ------------------------------------------------------------------ login
async function loginScreen() {
  clear();
  process.stdout.write(header('Login'));
  process.stdout.write(kv([
    ['User Code', 'A00001 (default)'],
    ['Password', 'admin'],
  ]));
  process.stdout.write(`\n ${c.dim}Tip: use Tools > Change Password or Admin > User to manage PINs.${c.reset}\n`);
  const usercode = (await ask(`\n ${c.cyan}User Code${c.reset} [A00001]: `)) || 'A00001';
  const password = await ask(` ${c.cyan}Password${c.reset} [admin]: `, { mask: true }) || 'admin';
  try {
    const r = await api.login(usercode, password);
    state.user = r.user;
    return true;
  } catch (err) {
    process.stdout.write(message(`Login failed: ${err.message}`, 'error'));
    await pause();
    return false;
  }
}

// ------------------------------------------------------------------ main shell
async function mainShell() {
  while (state.user) {
    clear();
    process.stdout.write(header('Main', `Company 1  |  Year 2024  |  User ${state.user.username} (${state.user.accesslevel})`));
    process.stdout.write('\n');
    MENU.forEach((node, i) => {
      process.stdout.write(`  ${c.bold}${String(i + 1).padStart(2)}${c.reset}. ${c.cyan}${node.title}${c.reset}\n`);
    });
    process.stdout.write(`\n  ${c.bold} 0${c.reset}. ${c.dim}Log Out / Exit${c.reset}\n`);
    const ans = await ask(`\n ${c.cyan}Select menu>${c.reset} `);
    const n = Number(ans);
    if (!n || n < 1 || n > MENU.length) {
      if (String(ans).trim() === '0') return;
      continue;
    }
    await nodeScreen(MENU[n - 1]);
  }
}

async function nodeScreen(node) {
  const options = node.children.map((ch) => ({ label: ch.title, value: ch.action }));
  const action = await menu(node.title, options);
  if (action) await dispatch(action);
}

// ------------------------------------------------------------------ router
async function dispatch(action) {
  const [kind, arg] = action.includes(':') ? action.split(':') : [action, undefined];
  try {
    switch (kind) {
      case 'logout': state.user = null; return;
      case 'exit': state.user = null; closeInput(); process.exit(0);
      case 'about': return about();
      case 'help': return helpScreen();
      case 'updates': return updatesScreen();
      case 'sysadmin': return systemAdminScreen();
      case 'params': return showParams();
      case 'users': return usersScreen();
      case 'rights': return rightsScreen();
      case 'new-year': return newYearScreen();
      case 'excise-audit': return exciseAuditScreen();
      case 'deleted-bills': return deletedBillsScreen();
      case 'master': return masterScreen(arg);
      case 'schemes': return schemesScreen();
      case 'items': return itemsScreen();
      case 'sale': return posSaleScreen();
      case 'purchase': return purchaseScreen();
      case 'transfer-sale': return transferSaleScreen();
      case 'transfer-receive': return transferReceiveScreen();
      case 'voucher': return voucherScreen(arg);
      case 'note': return noteScreen(arg);
      case 'on-account': return onAccountScreen();
      case 'account-replace': return accountReplaceScreen();
      case 'fine': return fineScreen();
      case 'opening-stock': return openingStockScreen();
      case 'stock-transfer': return stockTransferScreen();
      case 'breakage': return breakageScreen();
      case 'brs': return brsScreen();
      case 'upi': return upiScreen();
      case 'delivery': return deliveryScreen();
      case 'reports': return reportsGroupScreen(arg);
      case 'report': return reportScreen(arg, arg);
      case 'dashboard': return dashboardScreen();
      case 'dash': return dashboardGeneric(arg, arg === 'stock' ? 'Stock Status' : 'Purchase Status');
      case 'calc': return calcScreen();
      case 'change-password': return changePasswordScreen();
      case 'backup': return backupScreen();
      case 'restore': return restoreScreen();
      case 'shortcuts': return shortcutsScreen();
      case 'tally': return tallyScreen();
      case 'messages': return messagesScreen();
      case 'pml': return pmlScreen();
      case 'clean-print': return cleanPrintScreen();
      case 'sync': return syncScreen();
      default: return info('Not implemented', `Action "${action}" is not wired yet.`);
    }
  } catch (err) {
    process.stdout.write(message(`Error: ${err.message}`, 'error'));
    await pause();
  }
}

async function info(title, text) {
  clear();
  process.stdout.write(header(title));
  process.stdout.write(`\n ${text}\n`);
  await pause();
}

// ------------------------------------------------------------------ masters
async function masterScreen(name) {
  while (true) {
    const rows = await api.masters(name);
    clear();
    process.stdout.write(header(`Masters > ${name}`, `${rows.length} record(s)`));
    const cols = rows.length ? Object.keys(rows[0]).slice(0, 8).map((k) => ({ key: k, title: k })) : [];
    process.stdout.write(table(rows, cols));
    const choice = await menu('', [
      { label: 'Add', value: 'add' },
      { label: 'Delete', value: 'del' },
    ]);
    if (choice === 'add') await addMaster(name);
    else if (choice === 'del') await deleteMaster(name);
    else return;
  }
}

async function addMaster(name) {
  clear();
  process.stdout.write(header(`Masters > ${name} > Add`));
  process.stdout.write(`\n ${c.dim}Enter key=value pairs, comma separated. Example: companycode=1,categorycode=C9,categoryname=Gin${c.reset}\n`);
  const ans = await ask(`\n ${c.cyan}Values>${c.reset} `);
  const values = parsePairs(ans);
  await api.createMaster(name, values);
  process.stdout.write(message('Created.', 'ok'));
  await pause();
}

async function deleteMaster(name) {
  const key = await ask(`\n ${c.cyan}Delete key value(s) (comma separated keys)>${c.reset} `);
  const parts = key.split(',').map((s) => s.trim()).filter(Boolean);
  const rows = await api.masters(name);
  if (!rows.length) return;
  const pkCols = Object.keys(rows[0]).filter((k) => /code|companycode|name/i.test(k)).slice(0, parts.length || 1);
  const keys = {};
  (parts.length ? parts : ['']).forEach((v, i) => { keys[pkCols[i]] = v; });
  await api.deleteMaster(name, keys);
  process.stdout.write(message('Deleted.', 'ok'));
  await pause();
}

function parsePairs(s) {
  const out = {};
  for (const part of String(s).split(',')) {
    const [k, ...rest] = part.split('=');
    if (!k || rest.length === 0) continue;
    out[k.trim()] = rest.join('=').trim();
  }
  return out;
}

// ------------------------------------------------------------------ items
async function itemsScreen() {
  const rows = await api.items();
  clear();
  process.stdout.write(header('Item Master', `${rows.length} item(s)`));
  process.stdout.write(table(rows, [
    { key: 'itemcode', title: 'Code' },
    { key: 'itemname', title: 'Name' },
    { key: 'categoryname', title: 'Category' },
    { key: 'kflname', title: 'KFL' },
    { key: 'ml', title: 'ML', num: true, width: 4 },
    { key: 'packing', title: 'Pack', num: true, width: 5 },
    { key: 'salesrate', title: 'Sale', money: true, width: 8 },
    { key: 'purchaseRate', title: 'Purch', money: true, width: 8 },
    { key: 'barcode', title: 'Barcode' },
  ]));
  await pause();
}

// ------------------------------------------------------------------ POS sale
async function posSaleScreen() {
  clear();
  process.stdout.write(header('Transactions > Sale - POS'));
  const storecode = (await ask(`\n ${c.cyan}Counter/Store${c.reset} [CNT1]: `)) || 'CNT1';
  const salestype = (await ask(` ${c.cyan}Sale Type${c.reset} (FL/CS/NAI/RETURN) [FL]: `)) || 'FL';
  const ledgercode = (await ask(` ${c.cyan}Party ledger${c.reset} [C00001 walk-in]: `)) || 'C00001';
  const trndate = (await ask(` ${c.cyan}Date${c.reset} [2024-04-06]: `)) || '2024-04-06';
  const lines = [];
  process.stdout.write(`\n ${c.dim}Add items by barcode or code. Blank code finishes.${c.reset}\n`);
  while (true) {
    const term = await ask(`  ${c.cyan}Item${c.reset} (code/barcode, blank=done): `);
    if (!term.trim()) break;
    let item;
    try { item = await api.findItem(term.trim()); } catch { item = null; }
    if (!item) { process.stdout.write(message('Item not found.', 'error')); continue; }
    const qty = Number(await ask(`  Qty [1]: `)) || 1;
    const rate = Number(await ask(`  Rate [${item.salesrate}]: `)) || item.salesrate;
    lines.push({ itemcode: item.itemcode, qnty: qty, rate });
    process.stdout.write(message(`Added ${item.itemname} x${qty} @ ${rate}`, 'ok'));
  }
  if (lines.length === 0) return;
  const discount = Number(await ask(` ${c.cyan}Bill discount${c.reset} [0]: `)) || 0;
  const result = await api.createSale({ storecode, salestype, ledgercode, trndate, discount, userId: state.user.usercode, lines });
  clear();
  process.stdout.write(header('Sale Saved'));
  process.stdout.write(kv([
    ['Bill No', result.trnno],
    ['Gross', result.amount.toFixed(2)],
    ['Discount', result.discamount.toFixed(2)],
    ['Net', result.netamount.toFixed(2)],
    ['Lines', String(result.lines)],
  ]));
  await pause();
}

// ------------------------------------------------------------------ purchase
async function purchaseScreen() {
  clear();
  process.stdout.write(header('Transactions > Purchase'));
  const trndate = (await ask(`\n ${c.cyan}Date${c.reset} [2024-04-05]: `)) || '2024-04-05';
  const suppliercode = (await ask(` ${c.cyan}Supplier ledger${c.reset} [S00001]: `)) || 'S00001';
  const docno = (await ask(` ${c.cyan}Supplier invoice no${c.reset} [INV-001]: `)) || 'INV-001';
  const lines = [];
  while (true) {
    const term = await ask(`\n  ${c.cyan}Item${c.reset} (code/barcode, blank=done): `);
    if (!term.trim()) break;
    let item;
    try { item = await api.findItem(term.trim()); } catch { item = null; }
    if (!item) { process.stdout.write(message('Item not found.', 'error')); continue; }
    const qty = Number(await ask(`  Qty [1]: `)) || 1;
    const rate = Number(await ask(`  Rate [${item.purchaseRate}]: `)) || item.purchaseRate;
    const mrp = Number(await ask(`  MRP [${item.salesrate}]: `)) || item.salesrate;
    const batchno = (await ask(`  Batch no [B1]: `)) || 'B1';
    lines.push({ itemcode: item.itemcode, itemquantity: qty, itemrate: rate, itemmrp: mrp, batchno, itemfreeqnty: 0 });
    process.stdout.write(message(`Added ${item.itemname} x${qty} @ ${rate}`, 'ok'));
  }
  if (lines.length === 0) return;
  const result = await api.createPurchase({ trndate, suppliercode, purchaseacccode: 'L00004', shopcode: 'MAIN', docno, lines });
  clear();
  process.stdout.write(header('Purchase Saved'));
  process.stdout.write(kv([
    ['Trn No', String(result.trnno)],
    ['Gross', result.totamount.toFixed(2)],
    ['Tax/Other', result.tottaxOth.toFixed(2)],
    ['Net', result.totnetamt.toFixed(2)],
  ]));
  await pause();
}

// ------------------------------------------------------------------ vouchers
async function voucherScreen(type) {
  clear();
  process.stdout.write(header(`Transactions > ${type} Entry`));
  if (type === 'JOURNAL') return journalScreen();
  if (type === 'CONTRA') return contraScreen();
  const vchdate = (await ask(`\n ${c.cyan}Date${c.reset} [2024-04-07]: `)) || '2024-04-07';
  const head_account = (await ask(` ${c.cyan}Head account${c.reset} [L00002 Bank]: `)) || 'L00002';
  const client_account = (await ask(` ${c.cyan}Client account${c.reset} [${type === 'PAYMENT' ? 'S00001' : 'C00001'}]: `)) || (type === 'PAYMENT' ? 'S00001' : 'C00001');
  const amount_paid = Number(await ask(` ${c.cyan}Amount${c.reset}: `)) || 0;
  const narration = (await ask(` ${c.cyan}Narration${c.reset}: `)) || `${type} entry`;
  const result = await api.createVoucher({ type, vchdate, head_account, client_account, amount_paid, narration });
  process.stdout.write(message(`Voucher ${result.vchno} saved.`, 'ok'));
  await pause();
}

async function contraScreen() {
  const vchdate = (await ask(`\n ${c.cyan}Date${c.reset} [2024-04-07]: `)) || '2024-04-07';
  const from_ledger = (await ask(` ${c.cyan}From ledger${c.reset} [L00001 Cash]: `)) || 'L00001';
  const to_ledger = (await ask(` ${c.cyan}To ledger${c.reset} [L00002 Bank]: `)) || 'L00002';
  const amount = Number(await ask(` ${c.cyan}Amount${c.reset}: `)) || 0;
  await api.createVoucher({ type: 'CONTRA', vchdate, from_ledger, to_ledger, amount });
  process.stdout.write(message('Contra posted.', 'ok'));
  await pause();
}

async function journalScreen() {
  const vchdate = (await ask(`\n ${c.cyan}Date${c.reset} [2024-04-07]: `)) || '2024-04-07';
  const lines = [];
  while (true) {
    const ledcode = await ask(`  ${c.cyan}Ledger${c.reset} (blank=done): `);
    if (!ledcode.trim()) break;
    const debit = Number(await ask(`  Debit [0]: `)) || 0;
    const credit = Number(await ask(`  Credit [0]: `)) || 0;
    lines.push({ ledcode: ledcode.trim(), debit, credit });
  }
  if (lines.length === 0) return;
  const result = await api.createVoucher({ type: 'JOURNAL', vchdate, lines });
  process.stdout.write(message(`Journal ${result.trnno} posted.`, 'ok'));
  await pause();
}

// ------------------------------------------------------------------ stock ops
async function openingStockScreen() {
  clear();
  process.stdout.write(header('Transactions > Opening Stock'));
  const shopcode = (await ask(`\n ${c.cyan}Store${c.reset} [MAIN]: `)) || 'MAIN';
  const itemcode = await ask(` ${c.cyan}Item code${c.reset}: `);
  const qnty = Number(await ask(` ${c.cyan}Quantity${c.reset}: `)) || 0;
  const trndate = (await ask(` ${c.cyan}Date${c.reset} [2024-04-01]: `)) || '2024-04-01';
  await api.addOpeningStock({ shopcode, itemcode: itemcode.trim(), qnty, trndate });
  process.stdout.write(message('Opening stock saved.', 'ok'));
  await pause();
}

async function stockTransferScreen() {
  clear();
  process.stdout.write(header('Transactions > Stock Transfer'));
  const from = (await ask(`\n ${c.cyan}From store${c.reset} [MAIN]: `)) || 'MAIN';
  const to = (await ask(` ${c.cyan}To store${c.reset} [CNT1]: `)) || 'CNT1';
  const itemcode = (await ask(` ${c.cyan}Item code${c.reset}: `)).trim();
  const qnty = Number(await ask(` ${c.cyan}Quantity${c.reset}: `)) || 0;
  const trndate = (await ask(` ${c.cyan}Date${c.reset} [2024-04-08]: `)) || '2024-04-08';
  const result = await api.stockTransfer({ from, to, trndate, lines: [{ itemcode, qnty }] });
  process.stdout.write(message(`Transfer ${result.trnno} saved.`, 'ok'));
  await pause();
}

async function breakageScreen() {
  clear();
  process.stdout.write(header('Transactions > Breakage'));
  const shopcode = (await ask(`\n ${c.cyan}Store${c.reset} [MAIN]: `)) || 'MAIN';
  const itemcode = (await ask(` ${c.cyan}Item code${c.reset}: `)).trim();
  const quantity = Number(await ask(` ${c.cyan}Quantity${c.reset}: `)) || 0;
  const trndate = (await ask(` ${c.cyan}Date${c.reset} [2024-04-08]: `)) || '2024-04-08';
  const result = await api.breakage({ shopcode, trndate, lines: [{ itemcode, quantity }] });
  process.stdout.write(message(`Breakage ${result.trnno} saved.`, 'ok'));
  await pause();
}

// ------------------------------------------------------------------ reports
async function reportsGroupScreen(group) {
  const groups = await api.reportGroups();
  const list = groups[group] || [];
  const options = list.map((r) => ({ label: r.title, value: r.key }));
  const key = await menu(`Reports > ${group}`, options);
  if (key) await reportScreen(key, list.find((r) => r.key === key)?.title || key);
}

async function reportScreen(key, title) {
  const from = await ask(`\n ${c.cyan}From date${c.reset} (YYYY-MM-DD, blank=all): `);
  const to = await ask(` ${c.cyan}To date${c.reset} (YYYY-MM-DD, blank=all): `);
  const q = [];
  if (from.trim()) q.push(`from=${from.trim()}`);
  if (to.trim()) q.push(`to=${to.trim()}`);
  const qs = q.length ? `?${q.join('&')}` : '';
  let rows;
  try {
    rows = await api.catalogReport(key, qs);
  } catch {
    rows = await api.report(key, qs);
  }
  clear();
  process.stdout.write(header(`Reports > ${title}`, `${rows.length} row(s)`));
  const cols = rows.length ? Object.keys(rows[0]).map((k) => ({
    key: k, title: k,
    money: /amount|value|debit|credit|balance|net|gross|discount|profit|cost/i.test(k),
  })) : [];
  process.stdout.write(table(rows, cols));
  await pause();
}

async function dashboardScreen() {
  const d = await api.dashboard();
  clear();
  process.stdout.write(header('Dash Board > Sales Status'));
  process.stdout.write(kv([
    ['Sales bills', String(d.sales.bills)],
    ['Sales net', Number(d.sales.net).toFixed(2)],
    ['Purchase bills', String(d.purchase.bills)],
    ['Purchase net', Number(d.purchase.net).toFixed(2)],
    ['Items', String(d.items)],
    ['Stock value', d.stockValue.toFixed(2)],
    ['Stock lines', String(d.stockLines)],
  ]));
  await pause();
}

// ------------------------------------------------------------------ admin
async function usersScreen() {
  while (true) {
    const rows = await api.users();
    clear();
    process.stdout.write(header('Admin > User'));
    process.stdout.write(table(rows, [
      { key: 'usercode', title: 'Code' },
      { key: 'username', title: 'Name' },
      { key: 'accesslevel', title: 'Access' },
    ]));
    const choice = await menu('', [
      { label: 'Add user', value: 'add' },
      { label: 'Reset PIN', value: 'pin' },
    ]);
    if (choice === 'add') {
      const usercode = await ask(`\n ${c.cyan}User code${c.reset}: `);
      const username = await ask(` ${c.cyan}Name${c.reset}: `);
      const accesslevel = (await ask(` ${c.cyan}Access${c.reset} (Admin/User) [User]: `)) || 'User';
      const password = await ask(` ${c.cyan}PIN/Password${c.reset}: `, { mask: true });
      await api.createUser({ usercode: usercode.trim(), username: username.trim(), accesslevel, password });
      process.stdout.write(message('User created.', 'ok'));
      await pause();
    } else if (choice === 'pin') {
      const usercode = await ask(`\n ${c.cyan}User code${c.reset}: `);
      const pin = await ask(` ${c.cyan}New PIN${c.reset}: `, { mask: true });
      await api.resetPin(usercode.trim(), pin);
      process.stdout.write(message('PIN reset.', 'ok'));
      await pause();
    } else return;
  }
}

async function rightsScreen() {
  const usercode = (await ask(`\n ${c.cyan}User code${c.reset} [A00001]: `)) || 'A00001';
  const module = (await ask(` ${c.cyan}Module${c.reset} (MASTERS/TRANSACTIONS/REPORTS) [TRANSACTIONS]: `)) || 'TRANSACTIONS';
  const rights = {};
  for (const p of ['view', 'add', 'edit', 'delete']) {
    rights['per' + p[0]] = (await ask(`  ${p} (Y/N) [N]: `)).toUpperCase() === 'Y' ? 'Y' : 'N';
  }
  await api.setRights(usercode.trim(), module.trim().toUpperCase(), rights);
  process.stdout.write(message('Rights saved.', 'ok'));
  await pause();
}

async function changePasswordScreen() {
  const usercode = (await ask(`\n ${c.cyan}User code${c.reset} [${state.user.usercode}]: `)) || state.user.usercode;
  const pin = await ask(` ${c.cyan}New PIN${c.reset}: `, { mask: true });
  await api.resetPin(usercode.trim(), pin);
  process.stdout.write(message('PIN updated.', 'ok'));
  await pause();
}

async function showParams() {
  const params = await api.parameters();
  clear();
  process.stdout.write(header('Admin > Company Parameters'));
  process.stdout.write(kv(Object.entries(params).slice(0, 12).map(([k, v]) => [k, String(v ?? '')])));
  const edit = (await ask(`\n ${c.cyan}Edit a parameter?${c.reset} (key=value, blank=no): `)).trim();
  if (edit) {
    const [k, ...rest] = edit.split('=');
    await api.updateParameters({ [k.trim()]: rest.join('=').trim() });
    process.stdout.write(message('Parameter updated.', 'ok'));
    await pause();
  }
}

async function newYearScreen() {
  clear();
  process.stdout.write(header('Admin > New Year'));
  const years = await api.years();
  process.stdout.write(table(years, Object.keys(years[0] || {}).map((k) => ({ key: k, title: k }))));
  const fromYear = (await ask(`\n ${c.cyan}Carry forward from year${c.reset} [2024]: `)) || '2024';
  const yearcode = (await ask(` ${c.cyan}New year code${c.reset} [2025]: `)) || '2025';
  const stdate = (await ask(` ${c.cyan}Start date${c.reset} [2025-04-01]: `)) || '2025-04-01';
  const enddate = (await ask(` ${c.cyan}End date${c.reset} [2026-03-31]: `)) || '2026-03-31';
  const r = await api.createNewYear({ fromYear, yearcode, stdate, enddate });
  process.stdout.write(message(`Year ${r.yearcode} created. Ledgers carried: ${r.carriedLedgers}, stock: ${r.carriedStock}.`, 'ok'));
  await pause();
}

async function deletedBillsScreen() {
  const rows = await api.deletedBills();
  clear();
  process.stdout.write(header('Admin > Deleted Bill', `${rows.length} archived`));
  process.stdout.write(table(rows, [
    { key: 'trnno', title: 'Bill' }, { key: 'trndate', title: 'Date' },
    { key: 'ledgercode', title: 'Party' }, { key: 'netamount', title: 'Net', money: true },
    { key: 'deletedon', title: 'Deleted On' },
  ]));
  await pause();
}

async function exciseAuditScreen() {
  const rows = await api.exciseAudit();
  clear();
  process.stdout.write(header('Admin > Excise Audit', `${rows.length} item(s)`));
  process.stdout.write(table(rows, [
    { key: 'itemcode', title: 'Item' }, { key: 'excisecode', title: 'Excise' },
    { key: 'excisename', title: 'Excise Name' }, { key: 'ml', title: 'ML' },
    { key: 'strength', title: 'Strength' }, { key: 'opening', title: 'Opening' },
  ]));
  await pause();
}

async function systemAdminScreen() {
  const info2 = await api.systemInfo();
  clear();
  process.stdout.write(header('Main > System Admin'));
  process.stdout.write(kv([
    ['Version', info2.version],
    ['Updated', String(info2.updatedat)],
    ['Deployment mode', info2.deploymentMode],
    ['POS mode', String(info2.posMode)],
    ['Batch sale', String(info2.batchSale)],
    ['Negative sale', String(info2.negativeSale)],
    ['SMS enabled', String(info2.smsOn)],
    ['Data sync', String(info2.dataSync)],
  ]));
  await pause();
}

// ------------------------------------------------------------------ schemes
async function schemesScreen() {
  while (true) {
    const choice = await menu('Masters > Scheme / Offer', [
      { label: 'Schemes', value: 'scheme' },
      { label: 'Scheme Details', value: 'schemedetail' },
      { label: 'Offers', value: 'offer' },
    ]);
    if (!choice) return;
    await masterScreen(choice);
  }
}

// ------------------------------------------------------------------ transfers
async function transferSaleScreen() {
  clear();
  process.stdout.write(header('Transactions > Counter / Transfer Sale'));
  const trndate = (await ask(`\n ${c.cyan}Date${c.reset} [2024-04-06]: `)) || '2024-04-06';
  const from = (await ask(` ${c.cyan}From store${c.reset} [MAIN]: `)) || 'MAIN';
  const to = (await ask(` ${c.cyan}To store${c.reset} [CNT1]: `)) || 'CNT1';
  const lines = [];
  while (true) {
    const term = await ask(`\n  ${c.cyan}Item${c.reset} (code/barcode, blank=done): `);
    if (!term.trim()) break;
    let item;
    try { item = await api.findItem(term.trim()); } catch { item = null; }
    if (!item) { process.stdout.write(message('Item not found.', 'error')); continue; }
    const qnty = Number(await ask(`  Qty [1]: `)) || 1;
    lines.push({ itemcode: item.itemcode, qnty });
    process.stdout.write(message(`Added ${item.itemname} x${qnty}`, 'ok'));
  }
  if (lines.length === 0) return;
  const r = await api.createTransferSale({ trndate, from, to, lines });
  process.stdout.write(message(`Transfer ${r.trnno} saved (amount ${r.amount}).`, 'ok'));
  await pause();
}

async function transferReceiveScreen() {
  clear();
  process.stdout.write(header('Transactions > Transfer Receive'));
  const trndate = (await ask(`\n ${c.cyan}Date${c.reset} [2024-04-06]: `)) || '2024-04-06';
  const from = (await ask(` ${c.cyan}From store${c.reset} [MAIN]: `)) || 'MAIN';
  const to = (await ask(` ${c.cyan}To store${c.reset} [CNT1]: `)) || 'CNT1';
  const lines = [];
  while (true) {
    const itemcode = await ask(`\n  ${c.cyan}Item${c.reset} (code, blank=done): `);
    if (!itemcode.trim()) break;
    const qnty = Number(await ask(`  Qty [1]: `)) || 1;
    lines.push({ itemcode: itemcode.trim(), qnty });
  }
  if (lines.length === 0) return;
  const r = await api.createTransferReceive({ trndate, from, to, lines });
  process.stdout.write(message(`Transfer receive ${r.trnno} saved.`, 'ok'));
  await pause();
}

// ------------------------------------------------------------------ notes
async function noteScreen(type) {
  clear();
  process.stdout.write(header(`Transactions > ${type === 'DEBIT' ? 'Debit' : 'Credit'} Note`));
  const trndate = (await ask(`\n ${c.cyan}Date${c.reset} [2024-04-07]: `)) || '2024-04-07';
  const partycode = (await ask(` ${c.cyan}Party ledger${c.reset} [S00001]: `)) || 'S00001';
  const headaccount = (await ask(` ${c.cyan}Against account${c.reset} [L00004]: `)) || 'L00004';
  const amount = Number(await ask(` ${c.cyan}Amount${c.reset}: `)) || 0;
  const narration = (await ask(` ${c.cyan}Narration${c.reset}: `)) || `${type} note`;
  const r = await api.createNote({ trndate, type, partycode, headaccount, amount, narration });
  process.stdout.write(message(`${type} note ${r.noteno} saved (${r.amount}).`, 'ok'));
  await pause();
}

async function onAccountScreen() {
  clear();
  process.stdout.write(header('Transactions > On A/C Adjustment'));
  const trndate = (await ask(`\n ${c.cyan}Date${c.reset} [2024-04-07]: `)) || '2024-04-07';
  const partycode = (await ask(` ${c.cyan}Party ledger${c.reset} [S00001]: `)) || 'S00001';
  const against_ledger = (await ask(` ${c.cyan}Against ledger${c.reset} [L00003]: `)) || 'L00003';
  const amount = Number(await ask(` ${c.cyan}Amount${c.reset}: `)) || 0;
  const r = await api.onAccount({ trndate, partycode, against_ledger, amount });
  process.stdout.write(message(`On a/c adjustment ${r.trnno} posted.`, 'ok'));
  await pause();
}

async function accountReplaceScreen() {
  clear();
  process.stdout.write(header('Transactions > A/C Replace'));
  const trndate = (await ask(`\n ${c.cyan}Date${c.reset} [2024-04-07]: `)) || '2024-04-07';
  const from_ledger = (await ask(` ${c.cyan}From ledger${c.reset} [L00003]: `)) || 'L00003';
  const to_ledger = (await ask(` ${c.cyan}To ledger${c.reset} [L00004]: `)) || 'L00004';
  const amount = Number(await ask(` ${c.cyan}Amount${c.reset}: `)) || 0;
  const r = await api.accountReplace({ trndate, from_ledger, to_ledger, amount });
  process.stdout.write(message(`A/C replace ${r.trnno} posted.`, 'ok'));
  await pause();
}

async function fineScreen() {
  clear();
  process.stdout.write(header('Transactions > Excise Fine'));
  const trndate = (await ask(`\n ${c.cyan}Date${c.reset} [2024-04-07]: `)) || '2024-04-07';
  const amount = Number(await ask(` ${c.cyan}Fine amount${c.reset}: `)) || 0;
  const narration = (await ask(` ${c.cyan}Narration${c.reset}: `)) || 'Excise fine';
  const r = await api.createFine({ trndate, amount, narration });
  process.stdout.write(message(`Fine ${r.trnno} posted (${r.amount}).`, 'ok'));
  await pause();
}

// ------------------------------------------------------------------ BRS
async function brsScreen() {
  while (true) {
    const rows = await api.reconciliation();
    clear();
    process.stdout.write(header('Transactions > BRS Entry', `${rows.length} entr(ies)`));
    process.stdout.write(table(rows, [
      { key: 'slno', title: '#' }, { key: 'ledcode', title: 'Bank' }, { key: 'trndate', title: 'Date' },
      { key: 'chqno', title: 'Chq No' }, { key: 'amount', title: 'Amount', money: true },
      { key: 'cleared', title: 'Cleared' },
    ]));
    const choice = await menu('', [
      { label: 'Add entry', value: 'add' },
      { label: 'Mark cleared', value: 'clear' },
    ]);
    if (choice === 'add') {
      const ledcode = (await ask(`\n ${c.cyan}Bank ledger${c.reset} [L00002]: `)) || 'L00002';
      const trndate = (await ask(` ${c.cyan}Date${c.reset} [2024-04-07]: `)) || '2024-04-07';
      const chqno = await ask(` ${c.cyan}Cheque no${c.reset}: `);
      const amount = Number(await ask(` ${c.cyan}Amount${c.reset}: `)) || 0;
      await api.addReconciliation({ ledcode, trndate, chqno, amount });
      process.stdout.write(message('BRS entry added.', 'ok'));
      await pause();
    } else if (choice === 'clear') {
      const slno = Number(await ask(`\n ${c.cyan}Sl no${c.reset}: `));
      const cleareddate = (await ask(` ${c.cyan}Cleared date${c.reset} [2024-04-08]: `)) || '2024-04-08';
      await api.clearReconciliation(slno, cleareddate);
      process.stdout.write(message('Marked cleared.', 'ok'));
      await pause();
    } else return;
  }
}

// ------------------------------------------------------------------ UPI
async function upiScreen() {
  while (true) {
    const rows = await api.upi();
    clear();
    process.stdout.write(header('Transactions > QRCode / UPI Payment', `${rows.length} record(s)`));
    process.stdout.write(table(rows, [
      { key: 'trnno', title: 'Bill' }, { key: 'trndate', title: 'Date' },
      { key: 'amount', title: 'Amount', money: true }, { key: 'upiid', title: 'UPI ID' },
      { key: 'status', title: 'Status' },
    ]));
    const choice = await menu('', [{ label: 'Record payment', value: 'add' }]);
    if (choice !== 'add') return;
    const trnno = await ask(`\n ${c.cyan}Bill no${c.reset}: `);
    const trndate = (await ask(` ${c.cyan}Date${c.reset} [2024-04-07]: `)) || '2024-04-07';
    const amount = Number(await ask(` ${c.cyan}Amount${c.reset}: `)) || 0;
    const upiid = await ask(` ${c.cyan}UPI ID${c.reset}: `);
    const status = (await ask(` ${c.cyan}Status${c.reset} (PENDING/SUCCESS/FAILED) [SUCCESS]: `)) || 'SUCCESS';
    await api.recordUpi({ trnno: trnno.trim(), trndate, amount, upiid: upiid.trim(), status });
    process.stdout.write(message('UPI payment recorded.', 'ok'));
    await pause();
  }
}

// ------------------------------------------------------------------ delivery
async function deliveryScreen() {
  while (true) {
    const rows = await api.deliveries();
    clear();
    process.stdout.write(header('Transactions > Home Delivery', `${rows.length} order(s)`));
    process.stdout.write(table(rows, [
      { key: 'orderno', title: 'Order' }, { key: 'trndate', title: 'Date' },
      { key: 'custcode', title: 'Customer' }, { key: 'phone', title: 'Phone' },
      { key: 'amount', title: 'Amount', money: true }, { key: 'status', title: 'Status' },
    ]));
    const choice = await menu('', [
      { label: 'New delivery', value: 'add' },
      { label: 'Update status', value: 'status' },
    ]);
    if (choice === 'add') {
      const trndate = (await ask(`\n ${c.cyan}Date${c.reset} [2024-04-07]: `)) || '2024-04-07';
      const custcode = (await ask(` ${c.cyan}Customer code${c.reset} [CU0002]: `)) || 'CU0002';
      const address = await ask(` ${c.cyan}Address${c.reset}: `);
      const phone = await ask(` ${c.cyan}Phone${c.reset}: `);
      const amount = Number(await ask(` ${c.cyan}Amount${c.reset}: `)) || 0;
      const r = await api.createDelivery({ trndate, custcode, address, phone, amount });
      process.stdout.write(message(`Delivery ${r.orderno} created.`, 'ok'));
      await pause();
    } else if (choice === 'status') {
      const orderno = await ask(`\n ${c.cyan}Order no${c.reset}: `);
      const status = (await ask(` ${c.cyan}Status${c.reset} (OPEN/DELIVERED/CANCELLED) [DELIVERED]: `)) || 'DELIVERED';
      await api.setDeliveryStatus(orderno.trim(), status);
      process.stdout.write(message('Status updated.', 'ok'));
      await pause();
    } else return;
  }
}

// ------------------------------------------------------------------ dashboards
async function dashboardGeneric(group, title) {
  const rows = await api.catalogReport(group === 'stock' ? 'stock.items_closing' : 'purchase.bill_detail');
  clear();
  process.stdout.write(header(`Dash Board > ${title}`, `${rows.length} row(s)`));
  const cols = rows.length ? Object.keys(rows[0]).slice(0, 7).map((k) => ({
    key: k, title: k, money: /amount|value|net|gross/i.test(k),
  })) : [];
  process.stdout.write(table(rows, cols));
  await pause();
}

async function dashboardSchemeScreen() {
  const schemes = await api.catalogReport('mis.scheme');
  const stock = await api.catalogReport('stock.value_closing');
  const beer = stock.filter((r) => /beer/i.test(r.itemname || ''));
  clear();
  process.stdout.write(header('Dash Board > Scheme / Spirit-Beer / CS Figures'));
  process.stdout.write(kv([
    ['Active schemes', String(schemes.length)],
    ['Beer lines', String(beer.length)],
    ['Beer stock value', beer.reduce((s, r) => s + (r.value || 0), 0).toFixed(2)],
    ['CS (counter sales) lines', String(stock.length)],
  ]));
  await pause();
}

// ------------------------------------------------------------------ tools
async function calcScreen() {
  clear();
  process.stdout.write(header('Tools > Calculator'));
  const a = await ask(`\n ${c.cyan}A${c.reset}: `);
  const op = (await ask(` ${c.cyan}Operator${c.reset} (+ - * /): `)).trim();
  const b = await ask(` ${c.cyan}B${c.reset}: `);
  const r = await api.calc(a, op, b);
  process.stdout.write(message(`= ${r.result}`, 'ok'));
  await pause();
}

async function backupScreen() {
  clear();
  process.stdout.write(header('Tools > Data Back-Up'));
  const dest = (await ask(`\n ${c.cyan}Backup file path${c.reset} [./data/backup.db]: `)) || './data/backup.db';
  const r = await api.backup(dest);
  process.stdout.write(message(`Backed up to ${r.dest} (${r.size} bytes).`, 'ok'));
  await pause();
}

async function restoreScreen() {
  await info('Tools > Data Restore',
    'Restore replaces data/madhushala.db with a backup file. For the new stack, stop the backend, copy the backup over data/madhushala.db, and restart.');
}

async function shortcutsScreen() {
  await info('Tools > Shortcut Panel',
    'Keyboard shortcuts: type the menu number then Enter; 0 goes back. POS: enter item code/barcode, blank line finishes the bill.');
}

async function tallyScreen() {
  const kind = (await ask(`\n ${c.cyan}Export${c.reset} (SALES/PURCHASE) [SALES]: `)).trim().toUpperCase() || 'SALES';
  const lines = await api.tally(kind);
  clear();
  process.stdout.write(header(`Tools > Export To Tally (${kind})`, `${lines.length} voucher(s)`));
  process.stdout.write(lines.slice(0, 40).join('\n') + '\n');
  await pause();
}

async function messagesScreen() {
  while (true) {
    const rows = await api.messages();
    clear();
    process.stdout.write(header('Tools > SMS / WhatsApp', `${rows.length} message(s)`));
    process.stdout.write(table(rows, [
      { key: 'channel', title: 'Channel' }, { key: 'recipient', title: 'To' },
      { key: 'message', title: 'Message' }, { key: 'sentat', title: 'Sent' },
    ]));
    const choice = await menu('', [{ label: 'Send message', value: 'add' }]);
    if (choice !== 'add') return;
    const channel = (await ask(`\n ${c.cyan}Channel${c.reset} (SMS/WHATSAPP) [SMS]: `)) || 'SMS';
    const recipient = await ask(` ${c.cyan}Recipient${c.reset}: `);
    const message = await ask(` ${c.cyan}Message${c.reset}: `);
    await api.sendMessage({ channel, recipient, message });
    process.stdout.write(message('Message logged.', 'ok'));
    await pause();
  }
}

async function pmlScreen() {
  while (true) {
    const rows = await api.pml();
    clear();
    process.stdout.write(header('Tools > PML Print and Reports', `${rows.length} entr(ies)`));
    process.stdout.write(table(rows, [
      { key: 'slno', title: '#' }, { key: 'trndate', title: 'Date' },
      { key: 'itemcode', title: 'Item' }, { key: 'ml', title: 'ML' }, { key: 'bottles', title: 'Bottles' },
    ]));
    const choice = await menu('', [{ label: 'Add PML entry', value: 'add' }]);
    if (choice !== 'add') return;
    const trndate = (await ask(`\n ${c.cyan}Date${c.reset} [2024-04-07]: `)) || '2024-04-07';
    const itemcode = await ask(` ${c.cyan}Item code${c.reset}: `);
    const ml = Number(await ask(` ${c.cyan}ML${c.reset}: `)) || 0;
    const bottles = Number(await ask(` ${c.cyan}Bottles${c.reset}: `)) || 0;
    const r = await api.addPml({ trndate, itemcode: itemcode.trim(), ml, bottles });
    process.stdout.write(message(`PML entry added. Totals: ${r.totml} ML / ${r.totbottles} bottles.`, 'ok'));
    await pause();
  }
}

async function cleanPrintScreen() {
  const r = await api.cleanPrint();
  clear();
  process.stdout.write(header('Tools > Clean Print Garbage'));
  process.stdout.write(message(`Cleared ${r.messagesCleared} message(s) and ${r.auditCleared} audit row(s).`, 'ok'));
  await pause();
}

async function syncScreen() {
  while (true) {
    const rows = await api.syncStatus();
    clear();
    process.stdout.write(header('Tools > Data Sync', `${rows.length} job(s)`));
    process.stdout.write(table(rows, [
      { key: 'entity', title: 'Entity' }, { key: 'direction', title: 'Direction' },
      { key: 'records', title: 'Records' }, { key: 'status', title: 'Status' },
      { key: 'lastrun', title: 'Last Run' },
    ]));
    const choice = await menu('', [
      { label: 'Upload SALES', value: 'UPLOAD:SALES' },
      { label: 'Upload PURCHASE', value: 'UPLOAD:PURCHASE' },
      { label: 'Download MASTER', value: 'DOWNLOAD:MASTER' },
    ]);
    if (!choice) return;
    const [direction, entity] = choice.split(':');
    const r = await api.sync(entity, direction);
    process.stdout.write(message(`Sync ${direction} ${entity}: ${r.records} record(s).`, 'ok'));
    await pause();
  }
}

// ------------------------------------------------------------------ help
async function about() {
  const info2 = await api.systemInfo();
  clear();
  process.stdout.write(header('Help > About Madhushala'));
  process.stdout.write(kv([
    ['Product', 'Madhushala Pro (new stack)'],
    ['Version', info2.version],
    ['Company', info2.company],
    ['Deployment', info2.deploymentMode],
    ['POS mode', String(info2.posMode)],
    ['User', `${state.user.username} (${state.user.accesslevel})`],
  ]));
  await pause();
}

async function helpScreen() {
  await info('Help > Ask for Help',
    'Madhushala Pro rebuilt on Node.js + SQLite with a TUI. See new_code/README.md for the full menu mapping and API reference.');
}

async function updatesScreen() {
  const info2 = await api.systemInfo();
  clear();
  process.stdout.write(header('Help > List of Updates'));
  process.stdout.write(kv([
    ['New stack', 'TUI + Node.js backend + SQLite'],
    ['Report catalog', '113 reports across 8 groups'],
    ['Build', info2.version],
    ['Updated', String(info2.updatedat)],
  ]));
  await pause();
}

// ------------------------------------------------------------------ boot
async function main() {
  try {
    await api.health();
  } catch {
    clear();
    process.stdout.write(header('Backend unavailable'));
    process.stdout.write(message(`Cannot reach ${api.base}. Start the backend: npm --prefix backend start`, 'error'));
    process.exit(1);
  }
  let ok = false;
  while (!ok) ok = await loginScreen();
  await mainShell();
  closeInput();
  clear();
  process.stdout.write(header('Goodbye'));
}

process.on('SIGINT', () => { closeInput(); clear(); process.stdout.write('\n'); process.exit(0); });

main();
