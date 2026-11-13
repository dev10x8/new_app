#!/usr/bin/env node
// Reset a user's PIN without touching any other data (dev convenience).
//
//   npm run reset-admin-pin              # defaults to A00001 / 1234
//   npm run reset-admin-pin A00001 9876  # explicit user + PIN
//   node scripts/reset-pin.js --list
//
// The packaged desktop app exposes the same command as `backend.exe reset-pin`;
// both share src/recovery.js.
import { resetPinCommand } from '../src/recovery.js';

process.exit(resetPinCommand(process.argv.slice(2)));
