// Small DOM + formatting helpers used by the screens.

// The server owns "today". The client never reads its own clock for business
// dates, so an altered system date on the till cannot shift defaults. The value
// is set at boot from GET /api/meta (falling back to the local clock only if the
// server is unreachable, so the UI still renders).
let serverToday = null;
export function setServerToday(date) {
  if (date) serverToday = date;
}
export function today() {
  return serverToday || new Date().toISOString().slice(0, 10);
}

export function h(tag, props = {}, ...children) {
  const node = document.createElement(tag);
  for (const [k, v] of Object.entries(props || {})) {
    if (v == null || v === false) continue;
    if (k === 'class') node.className = v;
    else if (k === 'html') node.innerHTML = v;
    else if (k === 'text') node.textContent = v;
    else if (k === 'dataset') Object.assign(node.dataset, v);
    else if (k.startsWith('on') && typeof v === 'function') node.addEventListener(k.slice(2).toLowerCase(), v);
    else if (k === 'value') node.value = v;
    else if (v === true) node.setAttribute(k, '');
    else node.setAttribute(k, v);
  }
  append(node, children);
  return node;
}

function append(node, children) {
  for (const c of children.flat(Infinity)) {
    if (c == null || c === false) continue;
    node.append(c.nodeType ? c : document.createTextNode(String(c)));
  }
}

export const money = (n) => (Number(n) || 0).toLocaleString('en-IN', { minimumFractionDigits: 2, maximumFractionDigits: 2 });
export const num = (n) => (n == null ? '' : Number(n).toLocaleString('en-IN'));

const SVG_NS = 'http://www.w3.org/2000/svg';

/** Inline sparkline for a series of numbers (no chart dependency). */
export function sparkline(values, { width = 100, height = 30 } = {}) {
  const nums = (values || []).map((v) => Number(v) || 0);
  const svg = document.createElementNS(SVG_NS, 'svg');
  svg.setAttribute('class', 'spark');
  svg.setAttribute('viewBox', `0 0 ${width} ${height}`);
  svg.setAttribute('preserveAspectRatio', 'none');
  svg.setAttribute('aria-hidden', 'true');
  if (nums.length >= 2) {
    const max = Math.max(...nums, 1);
    const min = Math.min(...nums, 0);
    const span = max - min || 1;
    const pad = 3;
    const x = (i) => (i / (nums.length - 1)) * width;
    const y = (v) => pad + (1 - (v - min) / span) * (height - pad * 2);
    const line = nums.map((v, i) => `${x(i).toFixed(1)},${y(v).toFixed(1)}`).join(' ');
    const area = document.createElementNS(SVG_NS, 'polygon');
    area.setAttribute('points', `${x(0)},${height} ${line} ${x(nums.length - 1)},${height}`);
    area.setAttribute('fill', 'currentColor');
    area.setAttribute('opacity', '.12');
    const poly = document.createElementNS(SVG_NS, 'polyline');
    poly.setAttribute('points', line);
    poly.setAttribute('fill', 'none');
    poly.setAttribute('stroke', 'currentColor');
    poly.setAttribute('stroke-width', '2');
    poly.setAttribute('stroke-linejoin', 'round');
    poly.setAttribute('stroke-linecap', 'round');
    svg.append(area, poly);
  }
  return svg;
}

/** Trend label ("+12.5%" / "-20%" / "no change") from two figures. */
export function delta(prev, next) {
  const p = Number(prev) || 0;
  const n = Number(next) || 0;
  if (p === 0 && n === 0) return { text: 'no change', cls: 'flat', dir: 0 };
  if (p === 0) return { text: 'new', cls: '', dir: 1 };
  const pct = ((n - p) / Math.abs(p)) * 100;
  if (Math.abs(pct) < 0.05) return { text: '0.0%', cls: 'flat', dir: 0 };
  return {
    text: `${pct > 0 ? '+' : ''}${pct.toFixed(1)}%`,
    cls: pct > 0 ? '' : 'down',
    dir: pct > 0 ? 1 : -1,
  };
}

export function table(columns, rows) {
  if (!rows || !rows.length) return h('div', { class: 'table-wrap' }, h('div', { class: 'empty' }, 'No records found.'));
  const thead = h('thead', {}, h('tr', {}, ...columns.map((c) => h('th', { class: c.num ? 'num' : '' }, c.title))));
  const tbody = h('tbody', {}, ...rows.map((r) => h('tr', {}, ...columns.map((c) => {
    let v = typeof c.value === 'function' ? c.value(r) : r[c.key];
    if (c.money) v = money(v);
    else if (c.num) v = v == null ? '' : num(v);
    else v = v == null || v === '' ? '—' : String(v);
    if (c.badge) return h('td', {}, h('span', { class: `badge ${c.badge(r)}` }, v));
    return h('td', { class: c.num ? 'num' : '' }, v);
  }))));
  return h('div', { class: 'table-wrap' }, h('table', {}, thead, tbody));
}

export function field(label, input) {
  return h('label', { class: 'field' }, label, input);
}

export function input(name, value = '', type = 'text') {
  return h('input', { name, value, type });
}

export function select(name, options, value) {
  const sel = h('select', { name });
  for (const o of options) {
    const opt = h('option', { value: o.value ?? o }, o.label ?? o);
    if (String(o.value ?? o) === String(value)) opt.selected = true;
    sel.append(opt);
  }
  return sel;
}

export function form(fields, onSubmit) {
  const f = h('form', { class: 'form' });
  for (const row of fields) {
    const r = h('div', { class: `row${row.length === 3 ? ' three' : ''}` });
    for (const [label, ctrl] of row) r.append(field(label, ctrl));
    f.append(r);
  }
  const submitBtn = h('button', { class: 'btn primary', type: 'submit' }, 'Save');
  f.append(h('div', { class: 'actions' }, submitBtn));
  f.addEventListener('submit', async (e) => {
    e.preventDefault();
    const data = Object.fromEntries(new FormData(f).entries());
    submitBtn.disabled = true;
    try { await onSubmit(data); } finally { submitBtn.disabled = false; }
  });
  return f;
}

export function loading() {
  return h('div', { class: 'loading' }, h('span', { class: 'spinner' }), 'Loading…');
}

/** A KPI card: label, value, an optional trend pill and an optional sparkline. */
export function kpi({ label, value, money: isMoney = false, trend, series }) {
  const card = h('div', { class: 'card stat' },
    h('div', { class: 'k' },
      h('span', {}, label),
      trend ? h('span', { class: `delta ${trend.cls}`.trim() }, trend.text) : null),
    h('div', { class: `v${isMoney ? ' money' : ''}` }, isMoney ? money(value) : num(value)));
  if (series && series.length >= 2) card.append(sparkline(series));
  return card;
}
