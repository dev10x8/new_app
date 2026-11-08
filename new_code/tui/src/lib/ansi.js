// Minimal ANSI helpers for the TUI (no external dependencies).

export const ESC = '\x1b[';
export const c = {
  reset: '\x1b[0m',
  bold: '\x1b[1m',
  dim: '\x1b[2m',
  reverse: '\x1b[7m',
  red: '\x1b[31m',
  green: '\x1b[32m',
  yellow: '\x1b[33m',
  blue: '\x1b[34m',
  magenta: '\x1b[35m',
  cyan: '\x1b[36m',
  white: '\x1b[37m',
  bgBlue: '\x1b[44m',
  bgCyan: '\x1b[46m',
};

export function clear() { process.stdout.write(`${ESC}2J${ESC}H`); }
export function move(row, col) { process.stdout.write(`${ESC}${row};${col}H`); }
export function hideCursor() { process.stdout.write(`${ESC}?25l`); }
export function showCursor() { process.stdout.write(`${ESC}?25h`); }

export function size() {
  const cols = process.stdout.columns || 80;
  const rows = process.stdout.rows || 24;
  return { cols, rows };
}

/** Visible length ignoring ANSI escape codes. */
export function vlen(s) {
  return s.replace(/\x1b\[[0-9;]*m/g, '').length;
}

export function padEnd(s, width) {
  const n = vlen(s);
  return n >= width ? s : s + ' '.repeat(width - n);
}

export function padStart(s, width) {
  const n = vlen(s);
  return n >= width ? s : ' '.repeat(width - n) + s;
}

export function center(s, width) {
  const n = vlen(s);
  if (n >= width) return s;
  const left = Math.floor((width - n) / 2);
  return ' '.repeat(left) + s;
}
