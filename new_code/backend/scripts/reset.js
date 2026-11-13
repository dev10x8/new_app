#!/usr/bin/env node
// Drop the database file (and WAL/SHM sidecars) so the next open reseeds it.
// The packaged desktop app exposes the same command as `backend.exe reset-db`.
import { resetDbCommand } from '../src/recovery.js';

process.exit(resetDbCommand());
