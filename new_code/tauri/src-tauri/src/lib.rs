// Library entry point for the Tauri shell.
//
// The shell has no business logic: it starts the bundled Node.js backend
// (`runtime/backend/src/server.js` via the bundled `runtime/node`), waits for
// the API on 127.0.0.1:7100 to accept connections, and stops the child when the
// window closes. Everything else lives in the backend, so the desktop app and
// the browser app run the exact same frontend and API.

use std::net::TcpStream;
use std::path::PathBuf;
use std::process::{Child, Command, Stdio};
use std::sync::Mutex;
use std::time::Duration;

use tauri::path::BaseDirectory;
use tauri::{Manager, RunEvent};

const BACKEND_ADDR: &str = "127.0.0.1:7100";

struct BackendProcess(Mutex<Option<Child>>);

/// Resolve a bundled resource, trying the Windows and Unix names.
fn resource(app: &tauri::AppHandle, names: &[&str]) -> Option<PathBuf> {
    for name in names {
        if let Ok(p) = app.path().resolve(name, BaseDirectory::Resource) {
            if p.exists() {
                return Some(p);
            }
        }
    }
    None
}

/// Start the bundled backend. Returns `None` in development, where the backend
/// is run separately and the resource directory has no staged runtime.
fn spawn_backend(app: &tauri::AppHandle) -> Option<Child> {
    let node = resource(app, &["runtime/node.exe", "runtime/node"])?;
    let server = resource(app, &["runtime/backend/src/server.js"])?;

    // Keep the database in the per-user app data directory so it survives
    // upgrades and does not live inside the (read-only) install directory.
    let data_dir = app
        .path()
        .app_data_dir()
        .unwrap_or_else(|_| std::env::temp_dir().join("vanijliquor"));
    let _ = std::fs::create_dir_all(&data_dir);
    let db_file = data_dir.join("vanijliquor.db");

    let (host, port) = BACKEND_ADDR.split_once(':').unwrap_or(("127.0.0.1", "7100"));
    Command::new(node)
        .arg(server)
        .env("VANIJLIQUOR_DB", db_file)
        .env("HOST", host)
        .env("PORT", port)
        .stdin(Stdio::null())
        .stdout(Stdio::null())
        .stderr(Stdio::null())
        .spawn()
        .ok()
}

/// Wait until the backend accepts TCP connections, so the first login does not
/// race the server's start-up.
fn wait_for_backend(tries: u32) -> bool {
    for _ in 0..tries {
        if TcpStream::connect(BACKEND_ADDR).is_ok() {
            return true;
        }
        std::thread::sleep(Duration::from_millis(250));
    }
    false
}

#[cfg_attr(mobile, tauri::mobile_entry_point)]
pub fn run() {
    tauri::Builder::default()
        .setup(|app| {
            let handle = app.handle().clone();
            if let Some(child) = spawn_backend(&handle) {
                if !wait_for_backend(60) {
                    eprintln!("backend did not become ready on {BACKEND_ADDR}");
                }
                app.manage(BackendProcess(Mutex::new(Some(child))));
            } else {
                eprintln!("no bundled backend found; expecting one on {BACKEND_ADDR}");
                app.manage(BackendProcess(Mutex::new(None)));
            }
            Ok(())
        })
        .build(tauri::generate_context!())
        .expect("error while building VanijLiquor")
        .run(|app, event| {
            if let RunEvent::Exit = event {
                if let Some(state) = app.try_state::<BackendProcess>() {
                    if let Some(mut child) = state.0.lock().unwrap().take() {
                        let _ = child.kill();
                        let _ = child.wait();
                    }
                }
            }
        });
}
