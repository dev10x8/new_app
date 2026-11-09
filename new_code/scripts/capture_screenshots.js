#!/usr/bin/env node
// Capture full-page screenshots of every Madhushala Pro screen for the user
// guide, by driving headless Chromium over the DevTools Protocol (CDP).
//
// No npm dependencies: uses Node's built-in global WebSocket.
//
// Usage:
//   node scripts/capture_screenshots.js            # all screens
//   node scripts/capture_screenshots.js dashboard  # one route
//
// Requires the app running: backend on 127.0.0.1:7100, web on 127.0.0.1:8080.

import { spawn } from 'node:child_process';
import { mkdirSync, writeFileSync, rmSync } from 'node:fs';
import { dirname, resolve } from 'node:path';
import { fileURLToPath } from 'node:url';

const here = dirname(fileURLToPath(import.meta.url));
const OUT_DIR = resolve(here, '..', 'docs', 'screenshots');
const WEB = process.env.WEB_URL || 'http://127.0.0.1:8080';
const DEBUG_PORT = Number(process.env.CDP_PORT || 9222);
const PROFILE = '/tmp/madhushala-cdp-profile';
const VIEWPORT = { width: 1440, height: 960 };

// route id -> screenshot file stem. Order is the order the guide presents them.
const SCREENS = [
  ['login', '__login__'],
  ['dashboard', 'dashboard'],
  ['sysadmin', 'system-admin'],
  ['master:company', 'master-company'],
  ['params', 'admin-parameters'],
  ['users', 'admin-users'],
  ['rights', 'admin-user-rights'],
  ['new-year', 'admin-new-year'],
  ['excise-audit', 'admin-excise-audit'],
  ['deleted-bills', 'admin-deleted-bills'],
  ['master:acgroup', 'master-acgroup'],
  ['master:acount', 'master-acount'],
  ['master:ledger', 'master-ledger'],
  ['master:category', 'master-category'],
  ['master:kfl', 'master-kfl'],
  ['master:manufacturer', 'master-manufacturer'],
  ['master:strength', 'master-strength'],
  ['master:itemgroupml', 'master-itemgroupml'],
  ['items', 'items'],
  ['master:rateinfo', 'master-rateinfo'],
  ['master:storage', 'master-storage'],
  ['master:counter', 'master-counter'],
  ['master:customer', 'master-customer'],
  ['master:area', 'master-area'],
  ['master:taxscheme', 'master-taxscheme'],
  ['schemes', 'schemes'],
  ['master:exciseitem', 'master-exciseitem'],
  ['master:reporttagmain', 'master-reporttag'],
  ['master:naiunit', 'master-naiunit'],
  ['master:naigroup', 'master-naigroup'],
  ['master:naisubgroup', 'master-naisubgroup'],
  ['master:naiitem', 'master-naiitem'],
  ['sale', 'sale-pos'],
  ['sales-register', 'sales-register'],
  ['purchase', 'purchase'],
  ['transfer-sale', 'transfer-sale'],
  ['transfer-receive', 'transfer-receive'],
  ['voucher:PAYMENT', 'voucher-payment'],
  ['voucher:RECEIPT', 'voucher-receipt'],
  ['voucher:CONTRA', 'voucher-contra'],
  ['voucher:JOURNAL', 'voucher-journal'],
  ['note:DEBIT', 'note-debit'],
  ['note:CREDIT', 'note-credit'],
  ['on-account', 'on-account'],
  ['account-replace', 'account-replace'],
  ['fine', 'fine'],
  ['opening-stock', 'opening-stock'],
  ['stock-transfer', 'stock-transfer'],
  ['breakage', 'breakage'],
  ['brs', 'brs'],
  ['upi', 'upi'],
  ['delivery', 'delivery'],
  ['reports:sales', 'reports-sales'],
  ['reports:stock', 'reports-stock'],
  ['reports:purchase', 'reports-purchase'],
  ['reports:accounts', 'reports-accounts'],
  ['reports:mis', 'reports-mis'],
  ['reports:breakage', 'reports-breakage'],
  ['reports:transfer', 'reports-transfer'],
  ['reports:excise', 'reports-excise'],
  ['dash:stock', 'dash-stock'],
  ['dash:purchase', 'dash-purchase'],
  ['dash:scheme', 'dash-scheme'],
  ['calc', 'tool-calculator'],
  ['change-password', 'tool-change-password'],
  ['backup', 'tool-backup'],
  ['restore', 'tool-restore'],
  ['tally', 'tool-tally-export'],
  ['tally-sync', 'tool-tally-sync'],
  ['excise-sync', 'tool-excise-sync'],
  ['messages', 'tool-messages'],
  ['pml', 'tool-pml'],
  ['clean-print', 'tool-clean-print'],
  ['sync', 'tool-data-sync'],
  ['about', 'help-about'],
  ['help', 'help-ask'],
  ['updates', 'help-updates'],
];

const only = process.argv.slice(2);
const want = only.length ? new Set(only) : null;

// --------------------------------------------------------------- CDP client
class CDP {
  constructor(ws) { this.ws = ws; this.id = 0; this.pending = new Map(); this.handlers = new Map();
    ws.addEventListener('message', (ev) => {
      const msg = JSON.parse(ev.data);
      if (msg.id && this.pending.has(msg.id)) {
        const { resolve, reject } = this.pending.get(msg.id);
        this.pending.delete(msg.id);
        msg.error ? reject(new Error(JSON.stringify(msg.error))) : resolve(msg.result);
      } else if (msg.method && this.handlers.has(msg.method)) {
        this.handlers.get(msg.method)(msg.params);
      }
    });
  }
  send(method, params = {}) {
    const id = ++this.id;
    return new Promise((resolve, reject) => {
      this.pending.set(id, { resolve, reject });
      this.ws.send(JSON.stringify({ id, method, params }));
    });
  }
  once(method) {
    return new Promise((resolve) => this.handlers.set(method, (p) => { this.handlers.delete(method); resolve(p); }));
  }
}

const sleep = (ms) => new Promise((r) => setTimeout(r, ms));

async function httpJson(url) {
  const res = await fetch(url);
  return res.json();
}

async function main() {
  mkdirSync(OUT_DIR, { recursive: true });
  rmSync(PROFILE, { recursive: true, force: true });

  const chrome = spawn('chromium', [
    '--headless=new',
    '--no-sandbox',
    '--disable-gpu',
    '--disable-dev-shm-usage',
    '--hide-scrollbars',
    `--remote-debugging-port=${DEBUG_PORT}`,
    `--user-data-dir=${PROFILE}`,
    `--window-size=${VIEWPORT.width},${VIEWPORT.height}`,
    'about:blank',
  ], { stdio: 'ignore' });

  const cleanup = () => { try { chrome.kill('SIGKILL'); } catch {} };
  process.on('exit', cleanup);

  // wait for the debugger endpoint
  let targets;
  for (let i = 0; i < 60; i++) {
    try { targets = await httpJson(`http://127.0.0.1:${DEBUG_PORT}/json`); if (targets.length) break; }
    catch { /* not up yet */ }
    await sleep(250);
  }
  const page = targets.find((t) => t.type === 'page');
  const ws = new WebSocket(page.webSocketDebuggerUrl);
  await new Promise((res, rej) => { ws.addEventListener('open', res); ws.addEventListener('error', rej); });
  const cdp = new CDP(ws);

  await cdp.send('Page.enable');
  await cdp.send('Runtime.enable');
  await cdp.send('Emulation.setDeviceMetricsOverride', {
    width: VIEWPORT.width, height: VIEWPORT.height, deviceScaleFactor: 2, mobile: false,
  });

  async function evaluate(expression, awaitPromise = true) {
    const r = await cdp.send('Runtime.evaluate', { expression, awaitPromise, returnByValue: true });
    if (r.exceptionDetails) throw new Error(r.exceptionDetails.text + ' ' + JSON.stringify(r.exceptionDetails.exception?.description || ''));
    return r.result.value;
  }

  async function goto(url) {
    const loaded = cdp.once('Page.loadEventFired');
    await cdp.send('Page.navigate', { url });
    await loaded;
    await sleep(400);
  }

  // Log in once so localStorage holds the session; screens are then rendered by
  // clicking the real sidebar items (no reloads needed - it is a SPA).
  async function loginAndBoot() {
    await goto(`${WEB}/`);
    await evaluate(`(async () => {
      const r = await fetch('/api/login', { method:'POST', headers:{'content-type':'application/json'},
        body: JSON.stringify({ usercode:'A00001', password:'admin' }) }).then(r=>r.json());
      localStorage.setItem('user', JSON.stringify(r.data.user));
      return true;
    })()`);
    await evaluate(`location.reload(); true;`);
    await sleep(900);
  }

  const SIDEBAR_W = 248;

  // Capture the page. For app screens we clip to the content column (right of
  // the sidebar) so the ~80-item menu does not inflate every image; `shell`
  // captures the whole shell including the menu.
  async function screenshot(file, { full = true, shell = false } = {}) {
    const height = full
      ? await evaluate('Math.max(document.body.scrollHeight, document.documentElement.scrollHeight, window.innerHeight)')
      : VIEWPORT.height;
    const clip = shell
      ? { x: 0, y: 0, width: VIEWPORT.width, height: Math.min(height, 6000), scale: 1 }
      : { x: SIDEBAR_W, y: 0, width: VIEWPORT.width - SIDEBAR_W, height: Math.min(height, 6000), scale: 1 };
    const { data } = await cdp.send('Page.captureScreenshot', { format: 'png', captureBeyondViewport: true, clip });
    writeFileSync(resolve(OUT_DIR, `${file}.png`), Buffer.from(data, 'base64'));
    return true;
  }

  const saved = [];
  await loginAndBoot();
  for (const [route, stem] of SCREENS) {
    if (want && !want.has(route) && !want.has(stem)) continue;
    try {
      if (route === 'login') {
        await evaluate(`localStorage.removeItem('user'); location.reload(); true;`);
        await sleep(800);
        await screenshot(stem, { full: false, shell: true });
        await loginAndBoot();
      } else {
        const clicked = await evaluate(`(() => {
          const item = document.querySelector('.nav-item[data-route=' + JSON.stringify(${JSON.stringify(route)}) + ']');
          if (!item) return false;
          item.click();
          return true;
        })()`);
        if (!clicked) throw new Error(`no sidebar item for route ${route}`);
        await sleep(800);
        await screenshot(stem);
        // a whole-shell shot (menu + page) for the dashboard
        if (route === 'dashboard') await screenshot('shell-dashboard', { full: false, shell: true });
      }
      saved.push(stem);
      console.log('captured', stem);
    } catch (e) {
      console.error('FAILED', stem, e.message);
    }
  }

  ws.close();
  cleanup();
  console.log(`\n${saved.length} screenshots -> ${OUT_DIR}`);
}

main().catch((e) => { console.error(e); process.exit(1); });
