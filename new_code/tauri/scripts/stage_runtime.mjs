#!/usr/bin/env node
// Stage the runtime the packaged desktop app launches at startup.
//
// The Tauri shell has no business logic: on launch it spawns the backend, which
// serves the API on 127.0.0.1:7100. This script prepares `src-tauri/runtime/`,
// which `tauri.conf.json` bundles as a resource.
//
//   node scripts/stage_runtime.mjs
//
// Two layouts:
//   * Windows release build - `backend/backend.exe`, a single self-contained
//     executable (scripts/build_backend.mjs: esbuild + Node SEA). The install
//     ships NO readable JavaScript for the backend, only SQL/JSON data files.
//   * Anywhere else (macOS/Linux dev, or when the exe cannot be built) - the
//     backend source plus the Node binary, as before.
import { cp, mkdir, rm, access, chmod } from 'node:fs/promises';
import { dirname, join, resolve } from 'node:path';
import { fileURLToPath } from 'node:url';
import { buildBackend } from './build_backend.mjs';

const here = dirname(fileURLToPath(import.meta.url));
const tauriDir = resolve(here, '..');
const backendSrc = join(tauriDir, '..', 'backend');
const runtimeDir = join(tauriDir, 'src-tauri', 'runtime');
const runtimeBackend = join(runtimeDir, 'backend');
const nodeName = process.platform === 'win32' ? 'node.exe' : 'node';
const nodeDest = join(runtimeDir, nodeName);

const exists = (p) => access(p).then(() => true, () => false);

if (process.execPath.includes('node_modules')) {
  console.error(`Refusing to bundle a shim Node (${process.execPath}).`);
  console.error('Run this script with a real Node binary (e.g. via actions/setup-node).');
  process.exit(1);
}

await mkdir(runtimeDir, { recursive: true });

async function stageFromSource() {
  await rm(runtimeBackend, { recursive: true, force: true });
  await mkdir(runtimeBackend, { recursive: true });
  await cp(join(backendSrc, 'src'), join(runtimeBackend, 'src'), { recursive: true });
  await cp(join(backendSrc, 'scripts'), join(runtimeBackend, 'scripts'), { recursive: true });
  await cp(join(backendSrc, 'package.json'), join(runtimeBackend, 'package.json'));
  if (!(await exists(nodeDest))) {
    await cp(process.execPath, nodeDest);
    await chmod(nodeDest, 0o755);
  }
  console.log(`runtime ready at ${runtimeDir} (source layout)`);
  console.log(`  ${nodeName}  <- ${process.execPath}`);
  console.log(`  backend/  <- ${join(backendSrc, 'src')}`);
}

if (process.platform === 'win32') {
  try {
    await buildBackend();
    console.log(`runtime ready at ${runtimeDir} (bundled executable)`);
  } catch (err) {
    console.error(`backend exe build failed (${err.message}); falling back to source layout.`);
    await stageFromSource();
  }
} else {
  await stageFromSource();
}
