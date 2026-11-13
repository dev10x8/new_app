#!/usr/bin/env node
// Compile the backend into a single self-contained executable so an installed
// app ships no readable JavaScript for its business logic.
//
//   node scripts/build_backend.mjs
//
// Pipeline: esbuild bundles backend/src/bundle_entry.js (all our modules, no
// third-party deps) into one CommonJS file, Node's SEA turns that into a blob
// and postject injects it into a copy of the Node binary. The result is
// `runtime/backend/backend.exe` — a real executable that serves the API and
// also answers recovery subcommands (`reset-pin`, `reset-db`).
//
// Resources the code reads at runtime (SQL schema/seed, the demo licence, the
// state-master datasets) cannot live inside the bundle, so they are copied to
// `runtime/backend/resources/` and located via VANIJLIQUOR_RESOURCES /
// VANIJLIQUOR_DATA (set by the desktop shell, or defaulted here).
import { cp, mkdir, rm, copyFile, writeFile, chmod } from 'node:fs/promises';
import { existsSync } from 'node:fs';
import { dirname, join, resolve } from 'node:path';
import { fileURLToPath } from 'node:url';
import { spawnSync } from 'node:child_process';

const here = dirname(fileURLToPath(import.meta.url));
const tauriDir = resolve(here, '..');
const backendDir = join(tauriDir, '..', 'backend');
const runtimeDir = join(tauriDir, 'src-tauri', 'runtime');
const outDir = join(runtimeDir, 'backend');
const resourcesDir = join(outDir, 'resources');
const exeName = process.platform === 'win32' ? 'backend.exe' : 'backend';
const exePath = join(outDir, exeName);

// Node's SEA sentinel; must match the fuse compiled into the Node binary.
const SENTINEL = 'NODE_SEA_FUSE_fce680ab2cc467b6e072b8b5df1996b2';

async function esbuildBundle(entry, outfile) {
  let esbuild;
  try {
    ({ build: esbuild } = await import('esbuild'));
  } catch {
    throw new Error('esbuild is required to build the backend exe (npm install).');
  }
  await esbuild({
    entryPoints: [entry],
    outfile,
    bundle: true,
    platform: 'node',
    format: 'cjs',
    target: 'node22',
    // The SEA blob is embedded verbatim, so minify to strip names, comments and
    // whitespace: `strings backend.exe` then yields a dense blob rather than
    // readable business logic. This is obfuscation, not encryption - a
    // determined reverse-engineer can still recover the code.
    minify: true,
    legalComments: 'none',
    // Our code imports only node: builtins and relative modules; anything else
    // would be a surprise third-party dependency we do not want inlined.
    logLevel: 'warning',
  });
}

function seaBlob(script, blobPath) {
  const config = join(outDir, 'sea-config.json');
  return writeFile(config, JSON.stringify({
    main: script,
    output: blobPath,
    disableExperimentalSEAWarning: true,
  }, null, 2)).then(() => {
    const r = spawnSync(process.execPath, ['--experimental-sea-config', config], { stdio: 'inherit' });
    if (r.status !== 0) throw new Error('node --experimental-sea-config failed');
  });
}

async function inject(exe, blob) {
  let injectFn;
  try {
    ({ inject: injectFn } = await import('postject'));
  } catch {
    throw new Error('postject is required to inject the SEA blob (npm install).');
  }
  await injectFn(exe, 'NODE_SEA_BLOB', await (await import('node:fs/promises')).readFile(blob), {
    sentinelFuse: SENTINEL,
    machoSegmentName: 'NODE_SEA',
  });
}

export async function buildBackend() {
  if (process.execPath.includes('node_modules')) {
    throw new Error(`Refusing to bundle a shim Node (${process.execPath}); run with a real Node binary.`);
  }

  await rm(outDir, { recursive: true, force: true });
  await mkdir(outDir, { recursive: true });

  const bundle = join(outDir, 'backend.cjs');
  await esbuildBundle(join(backendDir, 'src', 'bundle_entry.js'), bundle);

  const blob = join(outDir, 'sea-prep.blob');
  await seaBlob(bundle, blob);

  await copyFile(process.execPath, exePath);
  await chmod(exePath, 0o755);
  await inject(exePath, blob);

  // Resources read at runtime, laid out as `resourcesDir` expects.
  await mkdir(join(resourcesDir, 'db'), { recursive: true });
  await mkdir(join(resourcesDir, 'license'), { recursive: true });
  for (const f of ['schema.sql', 'schema_ext.sql', 'seed.sql', 'seed_ext.sql']) {
    await copyFile(join(backendDir, 'src', 'db', f), join(resourcesDir, 'db', f));
  }
  await copyFile(join(backendDir, 'src', 'license', 'demo.vlic'), join(resourcesDir, 'license', 'demo.vlic'));
  const masters = join(backendDir, 'data', 'state-masters');
  if (existsSync(masters)) await cp(masters, join(resourcesDir, 'data', 'state-masters'), { recursive: true });

  // Intermediate build artefacts are not shipped.
  await rm(bundle, { force: true });
  await rm(blob, { force: true });
  await rm(join(outDir, 'sea-config.json'), { force: true });

  console.log(`backend exe -> ${exePath}`);
  console.log(`resources   -> ${resourcesDir}`);
  return exePath;
}

if (import.meta.url === `file://${process.argv[1]}`) {
  buildBackend().catch((err) => { console.error(err.message); process.exit(1); });
}
