#!/usr/bin/env node
// Stage the runtime the packaged desktop app launches at startup.
//
// The Tauri shell has no business logic: on launch it spawns a Node process
// running the bundled backend (`src/server.js`), which serves the API on
// 127.0.0.1:7100. This script copies that Node binary and the backend source
// into `src-tauri/runtime/`, which `tauri.conf.json` bundles as a resource.
//
//   node scripts/stage_runtime.mjs
//
// The Node binary comes from the running interpreter (a real binary, which is
// what CI's setup-node provides). The backend has no external dependencies
// (`node:sqlite` is built in), so its source is copied verbatim.
import { cp, mkdir, rm, access, chmod } from 'node:fs/promises';
import { dirname, join, resolve } from 'node:path';
import { fileURLToPath } from 'node:url';

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

await rm(runtimeBackend, { recursive: true, force: true });
await mkdir(runtimeBackend, { recursive: true });
await cp(join(backendSrc, 'src'), join(runtimeBackend, 'src'), { recursive: true });
// scripts/ ships so an installed app can recover a forgotten PIN
// (`node scripts/reset-pin.js`) and rebuild schema without a source checkout.
await cp(join(backendSrc, 'scripts'), join(runtimeBackend, 'scripts'), { recursive: true });
await cp(join(backendSrc, 'package.json'), join(runtimeBackend, 'package.json'));

if (!(await exists(nodeDest))) {
  await cp(process.execPath, nodeDest);
  await chmod(nodeDest, 0o755);
}

console.log(`runtime ready at ${runtimeDir}`);
console.log(`  ${nodeName}  <- ${process.execPath}`);
console.log(`  backend/  <- ${join(backendSrc, 'src')}`);
