// Library entry point for the Tauri shell.
//
// The shell has no business logic: it starts the bundled Node.js backend
// (`runtime/backend/src/server.js` via the bundled `runtime/node`), waits for
// the API on 127.0.0.1:7100 to accept connections, and stops the child when the
// window closes. Everything else lives in the backend, so the desktop app and
// the browser app run the exact same frontend and API.

use std::io::Write;
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

/// Per-user directory holding the database and the shell's log file.
fn data_dir(app: &tauri::AppHandle) -> PathBuf {
    app.path()
        .app_data_dir()
        .unwrap_or_else(|_| std::env::temp_dir().join("vanijliquor"))
}

/// Start the bundled backend. Returns `None` in development, where the backend
/// is run separately and the resource directory has no staged runtime.
fn spawn_backend(app: &tauri::AppHandle) -> Option<Child> {
    let node = resource(app, &["runtime/node.exe", "runtime/node"])?;
    let server = resource(app, &["runtime/backend/src/server.js"])?;

    // Keep the database in the per-user app data directory so it survives
    // upgrades and does not live inside the (read-only) install directory.
    let dir = data_dir(app);
    let _ = std::fs::create_dir_all(&dir);
    let db_file = dir.join("vanijliquor.db");
    let log_file = dir.join("backend.log");

    let (host, port) = BACKEND_ADDR.split_once(':').unwrap_or(("127.0.0.1", "7100"));

    // A packaged window has no console, so send the child's output to a log
    // file next to the database; it is the only way to see why a start-up
    // failed on a user's machine.
    log_line(&log_file, &format!("--- launch: node={} server={}", node.display(), server.display()));

    let mut cmd = Command::new(&node);
    cmd.arg(&server)
        .env("VANIJLIQUOR_DB", &db_file)
        .env("HOST", host)
        .env("PORT", port)
        .stdin(Stdio::null());
    match std::fs::OpenOptions::new().create(true).append(true).open(&log_file) {
        Ok(file) => {
            let err = file.try_clone().ok();
            cmd.stdout(Stdio::from(file));
            cmd.stderr(match err {
                Some(f) => Stdio::from(f),
                None => Stdio::null(),
            });
        }
        Err(_) => {
            cmd.stdout(Stdio::null());
            cmd.stderr(Stdio::null());
        }
    }

    match cmd.spawn() {
        Ok(child) => Some(child),
        Err(e) => {
            log_line(&log_file, &format!("failed to start backend: {e}"));
            None
        }
    }
}

/// Append a line to the shell's log file, ignoring any I/O error.
fn log_line(path: &std::path::Path, line: &str) {
    if let Ok(mut f) = std::fs::OpenOptions::new().create(true).append(true).open(path) {
        let _ = writeln!(f, "{line}");
    }
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
            let log_file = data_dir(&handle).join("backend.log");
            if let Some(child) = spawn_backend(&handle) {
                // Short block so the window appears promptly; the frontend also
                // retries while the backend finishes starting.
                if wait_for_backend(20) {
                    log_line(&log_file, &format!("backend ready on {BACKEND_ADDR}"));
                } else {
                    log_line(&log_file, &format!("backend did not become ready on {BACKEND_ADDR}"));
                }
                app.manage(BackendProcess(Mutex::new(Some(child))));
            } else {
                log_line(&log_file, "no bundled backend found");
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
