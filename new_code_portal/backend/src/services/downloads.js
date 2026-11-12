import { config } from '../config.js';

/**
 * Distribution metadata for the portal's "Download the app" screen.
 *
 * The portal does not host the installers itself — they are built by the
 * `package-windows` GitHub workflow in `../new_code` and attached to a GitHub
 * release. This service turns the configured repository / tag / override base
 * URL into concrete download links, plus the licence-server (this portal) URLs
 * a customer needs to activate the app.
 */

/** Filename convention used by the packaging workflow. */
export function installerNames(version = config.appVersion) {
  return {
    exe: `VanijLiquor_${version}_x64-setup.exe`,
    msi: `VanijLiquor_${version}_x64_en-US.msi`,
  };
}

/** Base URL the installers are served from. */
export function downloadBase() {
  if (config.downloadBaseUrl) return config.downloadBaseUrl.replace(/\/+$/, '');
  const tag = config.releaseTag ? `download/${config.releaseTag}` : 'releases/latest/download';
  return `https://github.com/${config.releaseRepo}/${tag}`;
}

/** The portal's own base URL (its licensing / activation server). */
export function serverBase() {
  return (config.licenseServerUrl || config.publicUrl || '').replace(/\/+$/, '');
}

/**
 * The distribution bundle: installer links, the licence server endpoints, and
 * the machine-fingerprint helper scripts. `origin` is the browser's current
 * origin, used as the server URL fallback when none is configured.
 */
export function distribution(origin = '') {
  const base = downloadBase();
  const names = installerNames();
  const server = serverBase() || String(origin || '').replace(/\/+$/, '');
  return {
    app_version: config.appVersion,
    release: {
      repo: config.releaseRepo,
      tag: config.releaseTag || null,
      base_url: base,
      override: Boolean(config.downloadBaseUrl),
    },
    installers: [
      { id: 'exe', label: 'Windows installer (.exe)', platform: 'windows', url: `${base}/${names.exe}`, filename: names.exe },
      { id: 'msi', label: 'Windows installer (.msi)', platform: 'windows', url: `${base}/${names.msi}`, filename: names.msi },
    ],
    license_server: {
      url: server,
      endpoints: {
        health: `${server}/api/health`,
        issue: `${server}/api/licenses`,
        activate: `${server}/api/licenses/:id/activate`,
        verify: `${server}/api/licenses/:id/verify`,
        status: `${server}/api/licenses/:id/status`,
        license_key: `${server}/api/license-key`,
      },
    },
    helpers: [
      { id: 'ps1', label: 'Fingerprint helper (PowerShell)', url: './downloads/machine-fingerprint.ps1', filename: 'machine-fingerprint.ps1' },
      { id: 'sh', label: 'Fingerprint helper (shell)', url: './downloads/machine-fingerprint.sh', filename: 'machine-fingerprint.sh' },
    ],
    steps: [
      'Download and run the Windows installer on the counter PC.',
      'Launch VanijLiquor; it shows a machine fingerprint (or run the helper script).',
      'Paste the fingerprint under “Issue Licence” and download the .vlic licence file.',
      `Import the .vlic in the app — it verifies offline and unlocks the modules.`,
    ],
  };
}
