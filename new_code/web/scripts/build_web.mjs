#!/usr/bin/env node
// Copy the static frontend into dist/ so Tauri can bundle it (frontendDist).
// The Docker/browser server serves the same files straight from this folder.
import { mkdir, copyFile, readdir, rm } from 'node:fs/promises';
import { resolve } from 'node:path';
import { fileURLToPath } from 'node:url';

const here = resolve(fileURLToPath(import.meta.url), '..', '..');
const out = resolve(here, 'dist');

await rm(out, { recursive: true, force: true });
await mkdir(out, { recursive: true });

const files = (await readdir(here)).filter((f) => /\.(html|js|css)$/.test(f));
for (const f of files) await copyFile(resolve(here, f), resolve(out, f));

console.log(`built web -> ${out} (${files.length} files)`);
