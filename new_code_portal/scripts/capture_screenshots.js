#!/usr/bin/env node
// Capture full-page screenshots of the portal screens by driving headless
// Chromium over the DevTools Protocol (CDP). No npm dependencies.
//
// Usage:
//   node scripts/capture_screenshots.js            # all screens
//   node scripts/capture_screenshots.js dashboard  # one route
//
// Requires the portal running: backend on 127.0.0.1:7200, web on 127.0.0.1:8081.

import { spawn } from 'node:child_process';
import { mkdirSync, writeFileSync, rmSync } from 'node:fs';
import { dirname, resolve } from 'node:path';
import { fileURLToPath } from 'node:url';

const here = dirname(fileURLToPath(import.meta.url));
const OUT_DIR = resolve(here, '..', 'docs', 'screenshots');
const WEB = process.env.WEB_URL || 'http://127.0.0.1:8081';
const DEBUG_PORT = Number(process.env.CDP_PORT || 9223);
const PROFILE = '/tmp/vanijliquor-portal-cdp';
const VIEWPORT = { width: 1440, height: 960 };

const SCREENS = [
  ['dashboard', 'dashboard'],
  ['businesses', 'businesses'],
  ['onboard', 'onboard-wizard'],
  ['plans', 'plans-pricing'],
  ['modules', 'modules-addons'],
  ['licenses', 'licences'],
  ['issue', 'issue-licence'],
  ['verify', 'activate-verify'],
  ['download', 'download-app'],
  ['key', 'licence-key'],
  ['audit', 'audit-log'],
];

const only = process.argv.slice(2);
const want = only.length ? new Set(only) : null;

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

async function main() {
  mkdirSync(OUT_DIR, { recursive: true });
  rmSync(PROFILE, { recursive: true, force: true });

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
  const screenshot = async (file, { shell = false } = {}) => {
    const height = await evaluate('Math.max(document.body.scrollHeight, document.documentElement.scrollHeight, window.innerHeight)');
    const clip = { x: shell ? 0 : 248, y: 0, width: shell ? VIEWPORT.width : VIEWPORT.width - 248, height: Math.min(height, 6000), scale: 1 };
    const { data } = await cdp.send('Page.captureScreenshot', { format: 'png', captureBeyondViewport: true, clip });
    writeFileSync(resolve(OUT_DIR, `${file}.png`), Buffer.from(data, 'base64'));
  };

  const saved = [];
  await goto(`${WEB}/`);
  for (const [route, stem] of SCREENS) {
    if (want && !want.has(route) && !want.has(stem)) continue;
    try {
      const ok = await evaluate(`(() => {
        const item = document.querySelector('.nav-item[data-route=' + JSON.stringify(${JSON.stringify(route)}) + ']');
        if (!item) return false; item.click(); return true;
      })()`);
      if (!ok) throw new Error(`no sidebar item for ${route}`);
      await sleep(800);
      await screenshot(stem);
      if (route === 'dashboard') await screenshot('shell-dashboard', { shell: true });
      saved.push(stem);
      console.log('captured', stem);
    } catch (e) { console.error('FAILED', stem, e.message); }
  }

  ws.close();
  cleanup();
  console.log(`\n${saved.length} screenshots -> ${OUT_DIR}`);
}

main().catch((e) => { console.error(e); process.exit(1); });
