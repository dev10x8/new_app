import readline from 'node:readline';
import { c, clear, size, padEnd, padStart, center, vlen } from './ansi.js';

// ---------- rendering ----------

export function header(title, subtitle = '') {
  const { cols } = size();
  const bar = padEnd(` ${c.bold}${c.bgCyan}\x1b[30m Madhushala Pro ${c.reset}${c.cyan} ${title}${c.reset}`, cols);
  const sub = subtitle ? `\n${c.dim} ${subtitle}${c.reset}` : '';
  return bar + sub + '\n';
}

export function footer(keys) {
  const { cols } = size();
  const line = keys.map(([k, v]) => `${c.reverse} ${k} ${c.reset} ${v}`).join('  ');
  return `\n${c.dim}${'─'.repeat(cols)}${c.reset}\n ${line}`;
}

/** Render an array of objects as an aligned table. */
export function table(rows, columns) {
  if (!rows || rows.length === 0) return `${c.dim} (no rows)${c.reset}\n`;
  const widths = columns.map((col) =>
    Math.max(vlen(col.title), ...rows.map((r) => vlen(fmt(r[col.key], col)))));
  const head = columns.map((col, i) => padEnd(`${c.bold}${col.title}${c.reset}`, widths[i])).join('  ');
  const sep = columns.map((_, i) => '─'.repeat(widths[i])).join('  ');
  const body = rows.map((r) =>
    columns.map((col, i) => padEnd(fmt(r[col.key], col), widths[i])).join('  ')).join('\n');
  return `${head}\n${c.dim}${sep}${c.reset}\n${body}\n`;
}

function fmt(v, col) {
  if (v === null || v === undefined) return '';
  if (col.money) return padStart(Number(v).toFixed(2), col.width || 0);
  if (col.num) return padStart(String(v), col.width || 0);
  const s = String(v);
  return col.width ? s.slice(0, col.width) : s;
}

export function kv(pairs) {
  const w = Math.max(...pairs.map(([k]) => vlen(k)));
  return pairs.map(([k, v]) => ` ${c.cyan}${padEnd(k, w)}${c.reset} : ${v}`).join('\n') + '\n';
}

export function message(text, kind = 'info') {
  const color = kind === 'error' ? c.red : kind === 'ok' ? c.green : c.yellow;
  return `\n ${color}${text}${c.reset}\n`;
}

// ---------- input ----------

let rl = null;
const lineQueue = [];
const waiters = [];
let closed = false;

export function initInput() {
  if (rl || closed) return rl;
  rl = readline.createInterface({ input: process.stdin, output: process.stdout, terminal: false });
  rl.on('line', (line) => {
    if (waiters.length) waiters.shift()(line);
    else lineQueue.push(line);
  });
  rl.on('close', () => {
    closed = true;
    while (waiters.length) waiters.shift()(null);
  });
  return rl;
}

export function closeInput() {
  if (rl) { rl.close(); rl = null; }
}

export const EOF = Symbol('EOF');

function nextLine() {
  if (lineQueue.length) return Promise.resolve(lineQueue.shift());
  if (closed) return Promise.resolve(EOF);
  return new Promise((resolve) => waiters.push(resolve));
}

export function ask(question, { mask = false } = {}) {
  process.stdout.write(question);
  initInput();
  if (mask && process.stdin.isTTY) {
    // masked prompt on a real terminal
    return new Promise((resolve) => {
      const stdin = process.stdin;
      stdin.resume();
      stdin.setRawMode?.(true);
      let buf = '';
      const onData = (ch) => {
        for (const chr of ch.toString()) {
          if (chr === '\r' || chr === '\n') {
            stdin.setRawMode?.(false);
            stdin.removeListener('data', onData);
            process.stdout.write('\n');
            return resolve(buf);
          }
          if (chr === '\x03') { process.exit(0); }
          if (chr === '\x7f' || chr === '\b') { if (buf) { buf = buf.slice(0, -1); process.stdout.write('\b \b'); } continue; }
          if (chr >= ' ') { buf += chr; process.stdout.write('*'); }
        }
      };
      stdin.on('data', onData);
    });
  }
  return nextLine();
}

export async function pause(text = 'Press Enter to continue...') {
  await ask(`\n${c.dim}${text}${c.reset}`);
}

export async function menu(title, options) {
  // Simple numbered menu; options = [{label, value}]
  clear();
  process.stdout.write(header(title));
  options.forEach((o, i) => {
    process.stdout.write(`  ${c.bold}${String(i + 1).padStart(2)}${c.reset}. ${o.label}\n`);
  });
  process.stdout.write(`  ${c.bold} 0${c.reset}. ${c.dim}Back${c.reset}\n`);
  const ans = await ask(`\n ${c.cyan}Select>${c.reset} `);
  const n = Number(ans);
  if (!n || n < 1 || n > options.length) return null;
  return options[n - 1].value;
}
