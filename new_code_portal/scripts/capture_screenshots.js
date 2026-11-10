#!/usr/bin/env node
// Capture full-page screenshots of every VanijLiquor Customer Portal screen
// for the user guide, by driving headless Chromium over the DevTools Protocol
// (CDP). No npm dependencies: uses Node's built-in global WebSocket.
//
// Usage:
//   node scripts/capture_screenshots.js                  # all screens
//   node scripts/capture_screenshots.js dashboard plans  # only these routes
//
// Requires the portal running: backend on 127.0.0.1:7200, web on 127.0.0.1:8081
// (npm start), with the demo dataset loaded (npm run seed-demo).

import { spawn } from 'node:child_process';
import { mkdirSync, writeFileSync, rmSync } from 'node:fs';
import { dirname, resolve } from 'node:path';
import { fileURLToPath } from 'node:url';

const here = dirname(fileURLToPath(import.meta.url));
const OUT_DIR = resolve(here, '..', 'docs', 'screenshots');
const WEB = process.env.WEB_URL || 'http://127.0.0.1:8081';
const API = process.env.API_URL || 'http://127.0.0.1:7200';
const DEBUG_PORT = Number(process.env.CDP_PORT || 9223);
const PROFILE = '/tmp/vanijliquor-portal-cdp-profile';
const VIEWPORT = { width: 1440, height: 960 };
const SIDEBAR_W = 248;

// route id -> screenshot file stem, in the order the guide presents them.
const SCREENS = [
  ['dashboard', 'dashboard'],
  ['businesses', 'businesses'],
  ['licenses', 'licences'],
  ['onboard', 'onboard-step1-business'],
  ['plans', 'plans'],
  ['modules', 'modules'],
  ['issue', 'issue'],
  ['verify', 'verify'],
  ['download', 'download'],
  ['key', 'licence-key'],
  ['audit', 'audit'],
];

const only = process.argv.slice(2);
const want = only.length ? new Set(only) : null;

// Stable demo machine fingerprints used when minting / redeeming a licence.
const DEMO_MACHINE = {
  mac: '00:1A:2B:3C:4D:5E', cpu: 'BFEBFBFF000306A9', disk: 'WD-1234ABCD',
  hostname: 'counter-01', os: 'Windows 11',
};
const OTHER_MACHINE = {
  mac: 'AA:BB:CC:DD:EE:FF', cpu: 'BFEBFBFF000306A9', disk: 'WD-1234ABCD',
  hostname: 'counter-01', os: 'Windows 11',
};

// --------------------------------------------------------------- CDP client
class CDP {
  constructor(ws) {
    this.ws = ws; this.id = 0; this.pending = new Map(); this.handlers = new Map();
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
const httpJson = async (url) => (await fetch(url)).json();

async function main() {
  mkdirSync(OUT_DIR, { recursive: true });
  rmSync(PROFILE, { recursive: true, force: true });

  try { await httpJson(`${API}/api/health`); }
  catch { throw new Error(`Portal backend not reachable at ${API}. Run: npm start`); }

  const chrome = spawn('chromium', [
    '--headless=new', '--no-sandbox', '--disable-gpu', '--disable-dev-shm-usage',
    '--hide-scrollbars', `--remote-debugging-port=${DEBUG_PORT}`,
    `--user-data-dir=${PROFILE}`, `--window-size=${VIEWPORT.width},${VIEWPORT.height}`, 'about:blank',
  ], { stdio: 'ignore' });
  const cleanup = () => { try { chrome.kill('SIGKILL'); } catch { /* gone */ } };
  process.on('exit', cleanup);

  let targets;
  for (let i = 0; i < 60; i++) {
    try { targets = await (await fetch(`http://127.0.0.1:${DEBUG_PORT}/json`)).json(); if (targets.length) break; }
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

  const evaluate = async (expression) => {
    const r = await cdp.send('Runtime.evaluate', { expression, awaitPromise: true, returnByValue: true });
    if (r.exceptionDetails) throw new Error(r.exceptionDetails.text);
    return r.result.value;
  };
  const goto = async (url) => {
    const loaded = cdp.once('Page.loadEventFired');
    await cdp.send('Page.navigate', { url });
    await loaded;
    await sleep(700);
  };
  const goRoute = async (route) => { await evaluate(`location.hash = ${JSON.stringify(route)}; true;`); await sleep(900); };
  const screenshot = async (file, { shell = false, full = true } = {}) => {
    const height = full
      ? await evaluate('Math.max(document.body.scrollHeight, document.documentElement.scrollHeight, window.innerHeight)')
      : VIEWPORT.height;
    const clip = { x: shell ? 0 : SIDEBAR_W, y: 0, width: shell ? VIEWPORT.width : VIEWPORT.width - SIDEBAR_W,
      height: Math.min(height, 6000), scale: 1 };
    const { data } = await cdp.send('Page.captureScreenshot', { format: 'png', captureBeyondViewport: true, clip });
    writeFileSync(resolve(OUT_DIR, `${file}.png`), Buffer.from(data, 'base64'));
  };
  const clickText = (label) => evaluate(`(() => {
    const b = [...document.querySelectorAll('button')].find((x) => x.textContent.trim() === ${JSON.stringify(label)});
    if (!b) return false; b.click(); return true;
  })()`);
  const fillFingerprint = (machine) => evaluate(`(() => {
    const fp = ${JSON.stringify(machine)};
    document.querySelectorAll('input[data-fp]').forEach((el) => {
      const k = el.dataset.fp; if (fp[k] != null) { el.value = fp[k]; el.dispatchEvent(new Event('input', { bubbles: true })); }
    });
    return true;
  })()`);
  // /api/businesses returns { data: { rows: [...] } }; /api/licenses returns { data: [...] }.
  const listFrom = (res) => (Array.isArray(res.data) ? res.data : (res.data?.rows || res.rows || []));
  const firstBusinessId = async () => {
    const rows = listFrom(await httpJson(`${API}/api/businesses`));
    return rows[0] ? rows[0].id : null;
  };

  const saved = [];
  const record = (stem) => { saved.push(stem); console.log('captured', stem); };

  await goto(`${WEB}/`);

  // ---- plain routes -----------------------------------------------------
  for (const [route, stem] of SCREENS) {
    if (want && !want.has(route) && !want.has(stem)) continue;
    try {
      await goRoute(route);
      await screenshot(stem);
      if (route === 'dashboard') await screenshot('shell-dashboard', { shell: true, full: false });
      record(stem);
    } catch (e) { console.error('FAILED', stem, e.message); }
  }

  // ---- business detail (open the first card) ----------------------------
  if (!want || want.has('business') || want.has('business-detail')) {
    try {
      const id = await firstBusinessId();
      if (!id) throw new Error('no businesses (run npm run seed-demo)');
      await goRoute('businesses');
      const ok = await evaluate(`(() => {
        const b = [...document.querySelectorAll('button')].find((x) => x.textContent.trim() === 'Open');
        if (!b) return false; b.click(); return true;
      })()`);
      if (!ok) throw new Error('no "Open" button on Businesses');
      await sleep(900);
      await screenshot('business-detail');
      record('business-detail');
    } catch (e) { console.error('FAILED business-detail', e.message); }
  }

  // ---- onboard wizard: one shot per step --------------------------------
  if (!want || ['onboard', 'onboard-step2-plan', 'onboard-step3-modules', 'onboard-step4-review'].some((k) => want.has(k))) {
    try {
      await goRoute('onboard');
      await evaluate(`(() => {
        const set = (name, v) => { const el = document.querySelector('input[name="'+name+'"]'); if (el) el.value = v; };
        set('legalname','Krishna Wines'); set('tradename','Krishna');
        set('ownername','A. Sharma'); set('email','owner@krishna.example'); set('phone','9800011122');
        set('gstin','27ABCDE1234F1Z5'); set('city','Pune'); set('state','Maharashtra');
        return true;
      })()`);
      await clickText('Continue'); await sleep(700);
      if (!want || want.has('onboard-step2-plan')) { await screenshot('onboard-step2-plan'); record('onboard-step2-plan'); }
      await clickText('Continue'); await sleep(700);
      if (!want || want.has('onboard-step3-modules')) { await screenshot('onboard-step3-modules'); record('onboard-step3-modules'); }
      await clickText('Continue'); await sleep(700);
      if (!want || want.has('onboard-step4-review')) { await screenshot('onboard-step4-review'); record('onboard-step4-review'); }
    } catch (e) { console.error('FAILED onboard wizard', e.message); }
  }

  // ---- issue licence + the issued modal ---------------------------------
  if (!want || want.has('issue') || want.has('issue-modal')) {
    try {
      await goRoute('issue');
      await evaluate(`(() => { const el = document.querySelector('input[name="months"]'); if (el) { el.value = '12'; el.dispatchEvent(new Event('input', { bubbles: true })); } return true; })()`);
      await fillFingerprint(DEMO_MACHINE);
      await sleep(300);
      if (!want || want.has('issue')) { await screenshot('issue'); record('issue'); }
      if (await clickText('Issue licence')) {
        await sleep(1100);
        if (!want || want.has('issue-modal')) { await screenshot('issue-modal', { full: false }); record('issue-modal'); }
        await evaluate(`document.getElementById('modal-close')?.click(); true;`);
        await sleep(300);
      }
    } catch (e) { console.error('FAILED issue', e.message); }
  }

  // ---- activate / verify: success and the anti-clone refusal ------------
  if (!want || ['verify', 'verify-success', 'verify-denied'].some((k) => want.has(k))) {
    try {
      const rows = listFrom(await httpJson(`${API}/api/licenses`));
      // Prefer the seeded licence bound to DEMO_MACHINE so "verify" shows a match.
      const wantMac = DEMO_MACHINE.mac.toLowerCase();
      const lic = rows.find((r) => (r.machine_detail?.mac || '').toLowerCase() === wantMac)
        || rows.find((r) => r.machine_hash) || rows[0];
      if (!lic) throw new Error('no licences (run npm run seed-demo)');
      const fill = (id, machine) => evaluate(`(() => {
        const idEl = document.querySelector('.form input');
        if (idEl) { idEl.value = ${JSON.stringify(id)}; idEl.dispatchEvent(new Event('input', { bubbles: true })); }
        return true;
      })()`).then(() => fillFingerprint(machine));

      await goRoute('verify');
      await fill(lic.license_id, DEMO_MACHINE);
      await clickText('Verify only'); await sleep(900);
      if (!want || want.has('verify-success')) { await screenshot('verify-success'); record('verify-success'); }

      await goRoute('verify');
      await fill(lic.license_id, OTHER_MACHINE);
      await clickText('Activate on this machine'); await sleep(900);
      if (!want || want.has('verify-denied')) { await screenshot('verify-denied'); record('verify-denied'); }
    } catch (e) { console.error('FAILED verify', e.message); }
  }

  ws.close();
  cleanup();
  console.log(`\n${saved.length} screenshots -> ${OUT_DIR}`);
}

main().catch((e) => { console.error(e.message || e); process.exit(1); });
