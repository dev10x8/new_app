// Entry point for the single-executable backend bundle.
//
// The bundled executable runs as the API server by default. Extra arguments
// dispatch to the recovery commands (`reset-pin`, `reset-db`) so a locked-out
// shop can recover without any readable JavaScript on disk.
import { runRecovery } from './recovery.js';

const sub = process.argv[2];
if (sub) {
  const code = runRecovery(sub, process.argv.slice(3));
  if (code !== null) process.exit(code);
}

// Start the API server. Imported dynamically, not statically: server.js opens
// the database at module load, and `reset-db` deletes it - on Windows an open
// SQLite file cannot be unlinked, so recovery must run before any DB open.
import('./server.js').catch((err) => {
  console.error(err);
  process.exit(1);
});
