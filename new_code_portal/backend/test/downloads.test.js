import test from 'node:test';
import assert from 'node:assert/strict';
import { execFileSync } from 'node:child_process';
import { fileURLToPath } from 'node:url';
import { dirname, join } from 'node:path';
import * as downloads from '../src/services/downloads.js';

const here = dirname(fileURLToPath(import.meta.url));
const root = join(here, '..');

/** Run a snippet in a child process with a clean env + the given overrides. */
function withEnv(env, code) {
  const out = execFileSync(process.execPath, ['--input-type=module', '-e', code], {
    cwd: root,
    env: { PATH: process.env.PATH, ...env },
    encoding: 'utf8',
  });
  return JSON.parse(out);
}

test('installer names follow the packaging workflow convention', () => {
  const names = downloads.installerNames('2.3.4');
  assert.equal(names.exe, 'VanijLiquor_2.3.4_x64-setup.exe');
  assert.equal(names.msi, 'VanijLiquor_2.3.4_x64_en-US.msi');
});

test('distribution exposes exe + msi installers, a licence server and helpers', () => {
  const d = downloads.distribution('https://portal.example.com');
  assert.deepEqual(d.installers.map((i) => i.id), ['exe', 'msi']);
  for (const it of d.installers) {
    assert.equal(it.platform, 'windows');
    assert.match(it.url, /^https?:\/\//);
    assert.ok(it.url.endsWith(it.filename));
  }
  assert.ok(d.license_server.url.startsWith('http'));
  assert.ok(d.license_server.endpoints.activate.includes('/api/licenses/'));
  assert.equal(d.helpers.length, 2);
  assert.equal(d.steps.length, 4);
  assert.ok(d.app_version);
});

test('installer URLs default to the repository latest-release path', () => {
  const d = withEnv({}, `
    import { distribution } from './src/services/downloads.js';
    console.log(JSON.stringify(distribution('')));
  `);
  assert.equal(d.release.tag, null);
  assert.equal(d.release.base_url, 'https://github.com/dev10x8/new_app/releases/latest/download');
  assert.ok(d.installers[0].url.endsWith('/VanijLiquor_1.0.0_x64-setup.exe'));
});

test('PORTAL_RELEASE_TAG pins installer links to a tagged release', () => {
  const d = withEnv({ PORTAL_RELEASE_TAG: 'v1.2.0', PORTAL_APP_VERSION: '1.2.0' }, `
    import { distribution } from './src/services/downloads.js';
    console.log(JSON.stringify(distribution('')));
  `);
  assert.equal(d.release.tag, 'v1.2.0');
  assert.equal(d.release.base_url, 'https://github.com/dev10x8/new_app/download/v1.2.0');
  assert.ok(d.installers[0].url.includes('VanijLiquor_1.2.0_x64-setup.exe'));
});

test('PORTAL_DOWNLOAD_BASE_URL overrides the GitHub release URL', () => {
  const d = withEnv({ PORTAL_DOWNLOAD_BASE_URL: 'https://cdn.example.com/vl/' }, `
    import { distribution } from './src/services/downloads.js';
    console.log(JSON.stringify(distribution('')));
  `);
  assert.equal(d.release.override, true);
  assert.equal(d.release.base_url, 'https://cdn.example.com/vl');
  assert.equal(d.installers[0].url, 'https://cdn.example.com/vl/VanijLiquor_1.0.0_x64-setup.exe');
});

test('the licence-server URL prefers the configured server, else the origin', () => {
  const configured = withEnv({ PORTAL_LICENSE_SERVER_URL: 'https://lic.example.com/' }, `
    import { distribution } from './src/services/downloads.js';
    console.log(JSON.stringify(distribution('http://localhost:8081')));
  `);
  assert.equal(configured.license_server.url, 'https://lic.example.com');
  assert.equal(configured.license_server.endpoints.health, 'https://lic.example.com/api/health');

  const fallback = downloads.distribution('http://127.0.0.1:8081');
  assert.equal(fallback.license_server.url, 'http://127.0.0.1:8081');
});
