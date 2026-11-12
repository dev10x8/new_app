// Library entry point for the Tauri shell.
//
// The shell has no business logic: it starts the bundled Node.js backend
// (`runtime/backend/src/server.js` via the bundled `runtime/node`), waits for
// the API on 127.0.0.1:7100 to accept connections, and stops the child when the
// window closes. Everything else lives in the backend, so the desktop app and
// the browser app run the exact same frontend and API.
//
// A packaged window has no console, so every step is written to a log file and
// exposed to the frontend through the `backend_diagnostics` command. The log is
// written to several locations so a line survives even when one is unwritable.

use std::io::Write;
use std::net::TcpStream;
use std::path::{Path, PathBuf};
use std::process::{Child, Command, Stdio};
use std::sync::Mutex;
use std::time::Duration;

use tauri::path::BaseDirectory;
use tauri::{Manager, RunEvent};

const BACKEND_ADDR: &str = "127.0.0.1:7100";

struct BackendProcess(Mutex<Option<Child>>);

/// Everything the frontend needs to explain a failed start.
struct ShellState {
    data_dir: PathBuf,
    log_paths: Vec<PathBuf>,
    ready: Mutex<bool>,
}

/// A log sink that survives the resource/DB problems it exists to diagnose: it
/// writes to the data dir, next to the executable, and the temp dir.
struct Log {
    paths: Vec<PathBuf>,
}

impl Log {
    fn open(data_dir: &Path, exe_dir: Option<&Path>) -> Log {
        let mut paths = vec![data_dir.join("backend.log")];
        if let Some(dir) = exe_dir {
            paths.push(dir.join("backend.log"));
        }
        paths.push(std::env::temp_dir().join("vanijliquor-backend.log"));
        for p in &paths {
            if let Some(parent) = p.parent() {
                let _ = std::fs::create_dir_all(parent);
            }
            if let Ok(mut f) = std::fs::OpenOptions::new().create(true).append(true).open(p) {
                let _ = writeln!(f, "--- log opened ({}) ---", now());
            }
        }
        Log { paths }
    }

    fn line(&self, line: &str) {
        for p in &self.paths {
            if let Ok(mut f) = std::fs::OpenOptions::new().create(true).append(true).open(p) {
                let _ = writeln!(f, "[{}] {}", now(), line);
            }
        }
    }

    /// A fresh handle for the child's stdout/stderr (one per stream).
    fn child_output(&self) -> Stdio {
        for p in &self.paths {
            if let Ok(f) = std::fs::OpenOptions::new().create(true).append(true).open(p) {
                return Stdio::from(f);
            }
        }
        Stdio::null()
    }
}

fn now() -> u64 {
    std::time::SystemTime::now()
        .duration_since(std::time::UNIX_EPOCH)
        .map(|d| d.as_secs())
        .unwrap_or(0)
}

/// Pick a directory we can actually write to for the database and logs.
///
/// Local (non-roaming) storage comes first on purpose: `%APPDATA%` is a roaming
/// profile that is frequently a slow network share on managed machines, and
/// SQLite/WAL on a network share can hang for minutes or fail outright.
fn data_dir(app: &tauri::AppHandle) -> PathBuf {
    let mut candidates: Vec<PathBuf> = Vec::new();
    if let Ok(p) = app.path().app_local_data_dir() {
        candidates.push(p);
    }
    if let Some(base) = std::env::var_os("LOCALAPPDATA") {
        candidates.push(PathBuf::from(base).join("VanijLiquor"));
    }
    if let Ok(p) = app.path().app_data_dir() {
        candidates.push(p);
    }
    if let Some(base) = std::env::var_os("APPDATA") {
        candidates.push(PathBuf::from(base).join("VanijLiquor"));
    }
    candidates.push(std::env::temp_dir().join("VanijLiquor"));

    for dir in &candidates {
        if std::fs::create_dir_all(dir).is_ok() && is_writable(dir) {
            return dir.clone();
        }
    }
    std::env::temp_dir().join("VanijLiquor")
}

fn is_writable(dir: &Path) -> bool {
    let probe = dir.join(".write-probe");
    match std::fs::File::create(&probe) {
        Ok(_) => {
            let _ = std::fs::remove_file(&probe);
            true
        }
        Err(_) => false,
    }
}

/// Resolve a bundled resource, trying the Tauri resource dir first and then the
/// executable's own directory.
fn resource(app: &tauri::AppHandle, log: &Log, names: &[&str]) -> Option<PathBuf> {
    let exe_dir = std::env::current_exe()
        .ok()
        .and_then(|p| p.parent().map(Path::to_path_buf));
    for name in names {
        if let Ok(p) = app.path().resolve(name, BaseDirectory::Resource) {
            log.line(&format!("resource {name}: {} (exists={})", p.display(), p.exists()));
            if p.exists() {
                return Some(p);
            }
        } else {
            log.line(&format!("resource {name}: resolve failed"));
        }
        if let Some(dir) = &exe_dir {
            let p = dir.join(name);
            if p.exists() {
                log.line(&format!("resource {name}: fell back to {}", p.display()));
                return Some(p);
            }
        }
    }
    None
}

/// Start the bundled backend. Returns `None` when the staged runtime is absent
/// (e.g. running in development against a separately started backend).
fn spawn_backend(app: &tauri::AppHandle, log: &Log) -> Option<Child> {
    let node = resource(app, log, &["runtime/node.exe", "runtime/node"])?;
    let server = resource(app, log, &["runtime/backend/src/server.js"])?;

    let dir = data_dir(app);
    let db_file = dir.join("vanijliquor.db");
    log.line(&format!("data dir: {} (exists={})", dir.display(), dir.exists()));
    log.line(&format!("node: {}", node.display()));
    log.line(&format!("server: {}", server.display()));

    let (host, port) = BACKEND_ADDR.split_once(':').unwrap_or(("127.0.0.1", "7100"));

    // Run from the resource dir so any relative path the backend resolves lands
    // inside the install, not wherever the shortcut started us.
    if let Some(work) = server.parent().and_then(|p| p.parent()).and_then(|p| p.parent()) {
        log.line(&format!("working dir: {}", work.display()));
    }

    let mut cmd = Command::new(&node);
    cmd.arg(&server)
        .env("VANIJLIQUOR_DB", &db_file)
        .env("HOST", host)
        .env("PORT", port)
        .stdin(Stdio::null())
        .stdout(log.child_output())
        .stderr(log.child_output());
    if let Some(work) = server.parent().and_then(|p| p.parent()).and_then(|p| p.parent()) {
        cmd.current_dir(work);
    }

    match cmd.spawn() {
        Ok(child) => {
            log.line(&format!("backend spawned, pid {}", child.id()));
            Some(child)
        }
        Err(e) => {
            log.line(&format!("failed to start backend: {e}"));
            None
        }
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

/// Report where the app keeps its data and the tail of the start-up log, so the
/// frontend can show it when the backend is unreachable.
#[tauri::command]
fn backend_diagnostics(state: tauri::State<ShellState>) -> String {
    let mut out = format!("ready: {}\n", *state.ready.lock().unwrap());
    out.push_str(&format!("data dir: {}\n", state.data_dir.display()));
    for p in &state.log_paths {
        out.push_str(&format!("log: {}\n", p.display()));
    }
    if let Some(p) = state.log_paths.first() {
        if let Ok(text) = std::fs::read_to_string(p) {
            out.push_str("--- log tail ---\n");
            let lines: Vec<&str> = text.lines().collect();
            let start = lines.len().saturating_sub(50);
            for line in &lines[start..] {
                out.push_str(line);
                out.push('\n');
            }
        }
    }
    out
}

#[cfg_attr(mobile, tauri::mobile_entry_point)]
pub fn run() {
    tauri::Builder::default()
        .invoke_handler(tauri::generate_handler![backend_diagnostics])
        .setup(|app| {
            let handle = app.handle().clone();
            let exe_dir = std::env::current_exe()
                .ok()
                .and_then(|p| p.parent().map(Path::to_path_buf));
            let dir = data_dir(&handle);
            let log = Log::open(&dir, exe_dir.as_deref());

            log.line("=== VanijLiquor starting ===");
            log.line(&format!(
                "exe dir: {}",
                exe_dir.as_ref().map(|p| p.display().to_string()).unwrap_or_default()
            ));
            log.line(&format!(
                "cwd: {}",
                std::env::current_dir().map(|p| p.display().to_string()).unwrap_or_default()
            ));

            match spawn_backend(&handle, &log) {
                Some(child) => {
                    log.line("backend process started");
                    app.manage(BackendProcess(Mutex::new(Some(child))));
                }
                None => {
                    log.line("no bundled backend found; expecting one on 127.0.0.1:7100");
                    app.manage(BackendProcess(Mutex::new(None)));
                }
            }

            app.manage(ShellState {
                data_dir: dir,
                log_paths: log.paths.clone(),
                ready: Mutex::new(false),
            });

            // Probe readiness off the main thread so the window is never held
            // back by a slow or unreachable backend; the frontend retries too.
            let probe_handle = handle.clone();
            std::thread::spawn(move || {
                if wait_for_backend(60) {
                    if let Some(state) = probe_handle.try_state::<ShellState>() {
                        if let Ok(mut r) = state.ready.lock() {
                            *r = true;
                        }
                    }
                }
            });
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
