import { existsSync } from 'node:fs';
import { dirname, join, resolve } from 'node:path';
import { fileURLToPath } from 'node:url';

// Directory of this module. Source runs as ESM (`import.meta.url`); a bundled
// single-executable runs as CommonJS where `import.meta.url` is empty and
// `__dirname` is the executable's directory.
let sourceDir;
try {
  sourceDir = dirname(fileURLToPath(import.meta.url));
} catch {
  sourceDir = typeof __dirname !== 'undefined' ? __dirname : process.cwd();
}

const hasDb = (dir) => existsSync(join(dir, 'db', 'schema.sql'));

// Read-only resources that mirror backend/src (SQL schema/seed, the bundled
// demo licence). Overridable by the shell; otherwise resolved relative to this
// module so a manually launched exe finds them beside itself.
function resolveResources() {
  if (process.env.VANIJLIQUOR_RESOURCES) return process.env.VANIJLIQUOR_RESOURCES;
  if (hasDb(sourceDir)) return sourceDir; // source layout: backend/src
  return join(sourceDir, 'resources');    // packaged: <exe dir>/resources
}

// Datasets that mirror backend/data (the state master lists).
function resolveData(resources) {
  if (process.env.VANIJLIQUOR_DATA) return process.env.VANIJLIQUOR_DATA;
  if (existsSync(join(resources, 'data'))) return join(resources, 'data');
  return resolve(sourceDir, '..', 'data'); // source layout: backend/data
}

export const resourcesDir = resolveResources();
export const dataDir = resolveData(resourcesDir);

export { sourceDir };
