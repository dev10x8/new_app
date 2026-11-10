// Small DOM + formatting helpers used by the screens.

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

// ------------------------------------------------------------ portal helpers

/** Whole-rupee formatter, e.g. ₹1,499. */
export const rupees = (n) => `₹${(Number(n) || 0).toLocaleString('en-IN', { maximumFractionDigits: 0 })}`;

/** Short date from an ISO string (YYYY-MM-DD or a full timestamp). */
export const date = (s) => (s ? String(s).slice(0, 10) : '—');

/** A coloured status badge. */
export function statusBadge(status) {
  const kind = {
    active: 'ok', issued: 'warn', onboarding: 'warn', draft: '',
    suspended: 'danger', revoked: 'danger', expired: 'danger', off: '',
  }[status] ?? '';
  return h('span', { class: `badge ${kind}`.trim() }, status || '—');
}

/** A clickable card used across dashboards and lists. */
export function card(title, body, opts = {}) {
  const c = h('div', { class: `card${opts.class ? ' ' + opts.class : ''}` },
    title ? h('h3', {}, title) : null, body);
  if (opts.onclick) { c.style.cursor = 'pointer'; c.addEventListener('click', opts.onclick); }
  return c;
}

/** A single statistic tile. */
export function stat(label, value, opts = {}) {
  return h('div', { class: 'card stat' },
    h('span', { class: 'k' }, label),
    h('span', { class: `v${opts.money ? ' money' : ''}` }, value),
    opts.hint ? h('span', { class: 'muted small' }, opts.hint) : null);
}

/** Checkbox list for multi-select (e.g. modules). Returns the wrapper node. */
export function checkboxList(name, options, selected = []) {
  const set = new Set(selected);
  const wrap = h('div', { class: 'check-list' });
  for (const o of options) {
    const value = o.value ?? o;
    const id = `${name}-${value}`;
    const cb = h('input', { type: 'checkbox', name, value, id });
    if (set.has(String(value))) cb.checked = true;
    wrap.append(h('label', { class: 'check', for: id }, cb,
      h('span', {}, h('strong', {}, o.label ?? value), o.hint ? h('span', { class: 'muted small' }, ` — ${o.hint}`) : null)));
  }
  return wrap;
}

/** Read the checked values of a checkbox list. */
export function checkedValues(root, name) {
  return [...root.querySelectorAll(`input[name="${name}"]:checked`)].map((el) => el.value);
}

/** Download a JSON object as a file in the browser. */
export function downloadJson(filename, obj) {
  const blob = new Blob([JSON.stringify(obj, null, 2)], { type: 'application/json' });
  const url = URL.createObjectURL(blob);
  const a = h('a', { href: url, download: filename });
  document.body.append(a);
  a.click();
  a.remove();
  setTimeout(() => URL.revokeObjectURL(url), 1000);
}
