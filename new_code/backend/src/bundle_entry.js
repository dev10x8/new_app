// Entry point for the single-executable backend bundle.
//
// The bundled executable runs as the API server by default. Extra arguments
// dispatch to the recovery commands (`reset-pin`, `reset-db`) so a locked-out
// shop can recover without any readable JavaScript on disk.
import { runRecovery } from './recovery.js';
import './server.js';

const name = process.argv[2];
if (name) {
  const code = runRecovery(name, process.argv.slice(3));
  if (code !== null) process.exit(code);
}
