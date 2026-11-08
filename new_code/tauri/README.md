# Madhushala Pro — desktop shell (Tauri: Web + Rust)

This folder turns the web UI in [`../web`](../web) into a native desktop
application using [Tauri](https://tauri.app) v2. The UI is ordinary
HTML/CSS/JS; Tauri only provides the native window (the Rust layer). There is
**no Rust business logic** — the app talks to the same Node.js + SQLite backend
on `http://127.0.0.1:7100` that the browser build uses.

```
tauri/
  package.json              # @tauri-apps/cli
  src-tauri/
    Cargo.toml              # Rust crate (madhushala-pro)
    build.rs
    tauri.conf.json         # window, bundle, frontendDist = ../web/dist
    capabilities/default.json
    icons/                  # app icons (regenerate with `tauri icon`)
    src/main.rs             # thin entry point
    src/lib.rs              # tauri::Builder, runs the webview
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

`npm run build` first runs `web/scripts/build_web.mjs` (via the
`beforeBuildCommand`) to copy the frontend into `web/dist`, then compiles the
Rust shell and packages an installer for the current OS.

## Regenerating icons

The icons in `src-tauri/icons` are placeholders. Replace them with your own:

```bash
npx tauri icon path/to/logo.png
```
