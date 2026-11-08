#!/usr/bin/env node
// Start the backend, wait for it to become healthy, then launch the TUI.
import { spawn } from 'node:child_process';
import { dirname, join } from 'node:path';
import { fileURLToPath } from 'node:url';

const here = dirname(fileURLToPath(import.meta.url));
const root = join(here, '..');
const backend = spawn(process.execPath, [join(root, 'backend', 'src', 'server.js')], { stdio: 'inherit' });
const api = process.env.API_URL || 'http://127.0.0.1:7100';

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

const healthy = await waitHealthy();
if (!healthy) {
  console.error(`Backend did not become healthy at ${api}`);
  backend.kill();
  process.exit(1);
}

const tui = spawn(process.execPath, [join(root, 'tui', 'src', 'index.js')], { stdio: 'inherit', env: process.env });

function shutdown() { backend.kill(); tui.kill(); }
tui.on('exit', (code) => { shutdown(); process.exit(code || 0); });
process.on('SIGINT', shutdown);
process.on('SIGTERM', shutdown);
