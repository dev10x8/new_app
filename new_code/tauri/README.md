# VanijLiquor — desktop shell (Tauri: Web + Rust)

This folder turns the web UI in [`../web`](../web) into a native desktop
application using [Tauri](https://tauri.app) v2. The UI is ordinary
HTML/CSS/JS; Tauri only provides the native window (the Rust layer). There is
**no Rust business logic** — the app runs the same Node.js + SQLite backend on
`http://127.0.0.1:7100` that the browser build uses. In a packaged build the
shell starts that backend itself from a bundled Node runtime (see
"Self-contained desktop app").

```
tauri/
  package.json              # @tauri-apps/cli
  scripts/
    stage_runtime.mjs       # copy node + backend into src-tauri/runtime/
  src-tauri/
    Cargo.toml              # Rust crate (vanijliquor)
    build.rs
    tauri.conf.json         # window, bundle, frontendDist = ../web/dist
    capabilities/default.json
    icons/                  # app icons (regenerate with `tauri icon`)
    runtime/                # staged at build time (git-ignored): node + backend
    src/main.rs             # thin entry point
    src/lib.rs              # tauri::Builder, runs the webview, starts the backend
```

## Why a separate frontend server?

The frontend is served the same way in both builds:

* **Browser / Docker** — `web/server.js` serves the static files and proxies
  `/api` to the backend. No CORS, no extra config.
* **Desktop (Tauri)** — `tauri dev` runs the same `web/server.js` for the dev
  window; `tauri build` bundles `web/dist` into the binary and the UI calls
  `127.0.0.1:7100` directly.

So the browser UI and the desktop UI are byte-for-byte identical.

## Prerequisites

* Rust (stable) — `rustup` — and the Tauri system dependencies for your OS
  (on Linux: `webkit2gtk-4.1`, `libgtk-3-dev`, `libayatana-appindicator3-dev`,
  `librsvg2-dev`; on Windows: WebView2, which ships with Windows 10+).
* Node.js ≥ 20 (to run the frontend server / build script).

See <https://tauri.app/start/prerequisites/>.

## Run (development)

```bash
# 1) start the backend (from the repo root)
cd new_code/backend && npm start        # http://127.0.0.1:7100

# 2) start the desktop app
cd new_code/tauri
npm install
npm run dev                             # opens the native window
```

## Build a release bundle

```bash
cd new_code/tauri
npm install
npm run build        # installers in src-tauri/target/release/bundle/
```

`npm run build` first runs `scripts/stage_runtime.mjs` (which copies a Node
binary and the backend source into `src-tauri/runtime/`) and
`web/scripts/build_web.mjs` (which copies the frontend into `web/dist`), via the
`beforeBuildCommand`, then compiles the Rust shell and packages an installer for
the current OS.

## Self-contained desktop app

The packaged app ships its own Node.js runtime and backend, so the customer
does not install Node:

- `scripts/stage_runtime.mjs` copies the running Node binary to
  `src-tauri/runtime/node` (`.exe` on Windows) and the backend source to
  `src-tauri/runtime/backend/`.
- `tauri.conf.json` bundles `runtime/` as a Tauri resource.
- On launch, the Rust shell (`src-tauri/src/lib.rs`) spawns
  `runtime/node runtime/backend/src/server.js`, waits for `127.0.0.1:7100`, and
  kills the child when the window closes. The backend keeps its database in the
  per-user app-data directory, so it survives upgrades.
- In development the runtime is not staged, so the shell expects the backend to
  be started separately (see "Run (development)").

The webview loads from `tauri://localhost` (Windows: `http://tauri.localhost`)
and calls `http://127.0.0.1:7100`, which is cross-origin — the backend sends CORS
headers (loopback-only, so `*` is safe).

## Build for Windows

Windows installers (NSIS `.exe` and WiX `.msi`) are built on a Windows machine
or in CI — Tauri cannot cross-compile a Windows bundle from Linux/macOS without
a full Windows toolchain.

**In CI (recommended).** Push a tag and the `Package Windows app` workflow
(`.github/workflows/package-windows.yml`) builds both installers, uploads them as
workflow artifacts, and attaches them to a GitHub release as:

```
VanijLiquor_<version>_x64-setup.exe      # NSIS installer (recommended)
VanijLiquor_<version>_x64_en-US.msi      # MSI
```

The `<version>` must match `version` in `src-tauri/tauri.conf.json` and the
portal's `PORTAL_APP_VERSION` / `PORTAL_RELEASE_TAG`, which is how the portal's
"Download the app" screen builds the download URLs.

```bash
# from the repo root, on main
git tag v1.0.0
git push origin v1.0.0
```

You can also run the workflow from the Actions tab (`workflow_dispatch`),
optionally passing a tag to publish a release.

**Locally, on Windows.** Install Rust (MSVC toolchain) and the WebView2 runtime
(ships with Windows 10+), then:

```bash
cd new_code/tauri
npm install
npm run build -- --target x86_64-pc-windows-msvc
```

## Regenerating icons

The icons in `src-tauri/icons` are placeholders. Replace them with your own:

```bash
npx tauri icon path/to/logo.png
```
