// Tauri desktop shell for VanijLiquor.
//
// The UI (../web) is ordinary HTML/CSS/JS and talks to the Node.js backend at
// 127.0.0.1:7100. This crate only provides the native window; all business
// logic lives in the backend, so the desktop app and the browser app run the
// exact same frontend.

// Prevent an extra console window on Windows in release.
#![cfg_attr(not(debug_assertions), windows_subsystem = "windows")]

fn main() {
    vanijliquor_lib::run()
}
