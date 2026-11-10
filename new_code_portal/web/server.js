#!/usr/bin/env node
// Static file server for the web frontend + a transparent /api proxy to the
// backend. Zero dependencies. It serves the portal UI and keeps it on the same
// origin as the backend, so the browser needs no CORS setup.
import http from 'node:http';
import { readFile, stat } from 'node:fs/promises';
import { extname, join, normalize, resolve } from 'node:path';
import { fileURLToPath } from 'node:url';

const here = resolve(fileURLToPath(import.meta.url), '..');
const PORT = Number(process.env.PORT || 8080);
const HOST = process.env.HOST || '0.0.0.0';
const BACKEND = process.env.API_URL || 'http://127.0.0.1:7100';

const TYPES = {
  '.html': 'text/html; charset=utf-8',
  '.js': 'text/javascript; charset=utf-8',
  '.css': 'text/css; charset=utf-8',
  '.json': 'application/json; charset=utf-8',
  '.svg': 'image/svg+xml',
  '.ico': 'image/x-icon',
  '.png': 'image/png',
  '.woff2': 'font/woff2',
};

async function serveStatic(req, res) {
  let path = decodeURIComponent(new URL(req.url, 'http://x').pathname);
  if (path === '/') path = '/index.html';
  const file = normalize(join(here, path));
  if (!file.startsWith(here)) { res.writeHead(403).end('Forbidden'); return; }
  try {
    const buf = await readFile(file);
    res.writeHead(200, { 'content-type': TYPES[extname(file)] || 'application/octet-stream', 'cache-control': 'no-cache' });
    res.end(buf);
  } catch {
    // SPA fallback
    try {
      const buf = await readFile(join(here, 'index.html'));
      res.writeHead(200, { 'content-type': TYPES['.html'] }).end(buf);
    } catch { res.writeHead(404).end('Not found'); }
  }
}

async function proxy(req, res) {
  const chunks = [];
  for await (const c of req) chunks.push(c);
  const body = Buffer.concat(chunks);
  const headers = { ...req.headers, host: new URL(BACKEND).host };
  try {
    const upstream = await fetch(BACKEND + req.url, { method: req.method, headers, body: body.length ? body : undefined });
    res.writeHead(upstream.status, {
      'content-type': upstream.headers.get('content-type') || 'application/json',
      'access-control-allow-origin': '*',
      'access-control-allow-headers': '*',
      'access-control-allow-methods': '*',
    });
    res.end(Buffer.from(await upstream.arrayBuffer()));
  } catch (err) {
    res.writeHead(502, { 'content-type': 'application/json' });
    res.end(JSON.stringify({ ok: false, error: `Backend unreachable: ${err.message}` }));
  }
}

const server = http.createServer((req, res) => {
  if (req.method === 'OPTIONS') {
    res.writeHead(204, { 'access-control-allow-origin': '*', 'access-control-allow-headers': '*', 'access-control-allow-methods': '*' });
    res.end();
    return;
  }
  if (req.url.startsWith('/api/')) return void proxy(req, res);
  return void serveStatic(req, res);
});

server.listen(PORT, HOST, () => {
  console.log(`web frontend on http://${HOST}:${PORT}  (proxying /api -> ${BACKEND})`);
});

// Fail fast if the backend URL is malformed.
stat(here).catch(() => {});
