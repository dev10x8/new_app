#!/usr/bin/env node
// Start the portal backend, wait for it to become healthy, then launch the web
// UI and open it in the default browser.
import { spawn } from 'node:child_process';
import { dirname, join } from 'node:path';
import { fileURLToPath } from 'node:url';

const here = dirname(fileURLToPath(import.meta.url));
const root = join(here, '..');
const api = process.env.API_URL || 'http://127.0.0.1:7200';
const webPort = process.env.WEB_PORT || '8081';
const webUrl = `http://127.0.0.1:${webPort}`;

const backend = spawn(process.execPath, [join(root, 'backend', 'src', 'server.js')], { stdio: 'inherit' });

async function waitHealthy(tries = 40) {
  for (let i = 0; i < tries; i++) {
    try {
      const res = await fetch(`${api}/api/health`);
      if (res.ok) return true;
    } catch { /* not up yet */ }
    await new Promise((r) => setTimeout(r, 250));
  }
  return false;
}

if (!await waitHealthy()) {
  console.error(`Backend did not become healthy at ${api}`);
  backend.kill();
  process.exit(1);
}

const web = spawn(process.execPath, [join(root, 'web', 'server.js')], {
  stdio: 'inherit',
  env: { ...process.env, PORT: webPort, API_URL: api },
});

console.log(`\nVanijLiquor Customer Portal: ${webUrl}\n`);
openBrowser(webUrl);

function openBrowser(url) {
  const cmd = process.platform === 'win32' ? 'start'
    : process.platform === 'darwin' ? 'open' : 'xdg-open';
  try { spawn(cmd, [url], { stdio: 'ignore', detached: true, shell: process.platform === 'win32' }).unref(); }
  catch { /* headless - just print the URL */ }
}

function shutdown() { backend.kill(); web.kill(); }
web.on('exit', (code) => { shutdown(); process.exit(code || 0); });
process.on('SIGINT', shutdown);
process.on('SIGTERM', shutdown);
