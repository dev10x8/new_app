import test from 'node:test';
import assert from 'node:assert/strict';
import { mkdtempSync } from 'node:fs';
import { tmpdir } from 'node:os';
import { join } from 'node:path';

// The desktop app's webview is cross-origin to the backend (tauri://localhost,
// or http://tauri.localhost on Windows, vs http://127.0.0.1:7100), so the
// server must answer CORS preflights and tag responses. Boot the real server on
// an ephemeral port with a throwaway database and exercise it with fetch().
const dir = mkdtempSync(join(tmpdir(), 'vanijliquor-cors-'));
process.env.HOST = '127.0.0.1';
process.env.PORT = '0';
process.env.VANIJLIQUOR_DB = join(dir, 'test.db');

const { server } = await import('../src/server.js');
if (!server.listening) await new Promise((r) => server.once('listening', r));
const base = `http://127.0.0.1:${server.address().port}`;
test.after(() => server.close());

test('CORS preflight is answered for the desktop webview', async () => {
  const res = await fetch(`${base}/api/health`, { method: 'OPTIONS' });
  assert.equal(res.status, 204);
  assert.equal(res.headers.get('access-control-allow-origin'), '*');
  assert.equal(res.headers.get('access-control-allow-methods'), '*');
  assert.equal(res.headers.get('access-control-allow-private-network'), 'true');
});

test('API responses carry the CORS origin header', async () => {
  const res = await fetch(`${base}/api/health`);
  assert.equal(res.status, 200);
  assert.equal(res.headers.get('access-control-allow-origin'), '*');
  assert.equal(res.headers.get('access-control-allow-private-network'), 'true');
  const body = await res.json();
  assert.equal(body.ok, true);
});

// The exact desktop flow: the webview preflights the JSON POST, then sends it
// with the tauri.localhost origin. This is what "Failed to fetch" on the login
// screen pointed at.
test('login succeeds cross-origin from the desktop webview', async () => {
  const origin = 'http://tauri.localhost';
  const pre = await fetch(`${base}/api/login`, {
    method: 'OPTIONS',
    headers: { origin, 'access-control-request-method': 'POST', 'access-control-request-headers': 'content-type' },
  });
  assert.equal(pre.status, 204);
  assert.equal(pre.headers.get('access-control-allow-origin'), '*');

  const res = await fetch(`${base}/api/login`, {
    method: 'POST',
    headers: { origin, 'content-type': 'application/json' },
    body: JSON.stringify({ usercode: 'A00001', password: 'admin' }),
  });
  assert.equal(res.status, 200);
  assert.equal(res.headers.get('access-control-allow-origin'), '*');
  const body = await res.json();
  assert.equal(body.ok, true);
  assert.equal(body.data.user.usercode, 'A00001');
});
