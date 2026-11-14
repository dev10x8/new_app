import { api } from './api.js';
import { NAV, ROUTE_TITLES, ROUTE_GROUP, groupRoute, isGroupRoute, groupOfRoute } from './menu.js';
import { h, setServerToday } from './ui.js';
import { navIcon, groupIcon, svgIcon } from './icons.js';
import { getTheme, applyTheme, toggleTheme } from './theme.js';
import { screens } from './screens.js';

// Navigation modes:
//   'expand' — Option A: clicking a sidebar group expands its submenu inline.
//   'cards'  — Option B: the sidebar lists groups only; clicking one opens a
//              launcher of small boxes in the main panel.
// The mode is an Admin parameter (`companyparamtr.nav_mode`), served in
// /api/meta. A `?nav=cards` (or `?nav=expand`) URL query overrides it for a
// single session so the two options can be previewed without changing config.
const NAV_MODES = ['expand', 'cards'];
const urlNavMode = () => {
  const v = new URLSearchParams(location.search).get('nav');
  return NAV_MODES.includes(v) ? v : null;
};
const cachedNavMode = () => {
  const v = localStorage.getItem('navMode');
  return NAV_MODES.includes(v) ? v : null;
};

const state = {
  user: null, meta: null, route: 'dashboard', license: null, open: null,
  mode: urlNavMode() || cachedNavMode() || 'expand',
};

const el = (id) => document.getElementById(id);

// -------------------------------------------------------------- toast
let toastTimer;
export function toast(msg, kind = '') {
  const t = el('toast');
  t.textContent = msg;
  t.className = `toast ${kind}`.trim();
  t.hidden = false;
  clearTimeout(toastTimer);
  toastTimer = setTimeout(() => { t.hidden = true; }, 3200);
}

// -------------------------------------------------------------- modal
export function openModal(title, bodyNode) {
  el('modal-title').textContent = title;
  const body = el('modal-body');
  body.replaceChildren(bodyNode);
  el('modal').hidden = false;
}
export function closeModal() { el('modal').hidden = true; }

// -------------------------------------------------------------- context
/** Switch navigation mode at runtime (from Admin → Parameters or a `?nav=` link). */
export function applyNavMode(mode) {
  if (!NAV_MODES.includes(mode)) return;
  state.mode = mode;
  localStorage.setItem('navMode', mode);
  state.open = mode === 'expand' ? (ROUTE_GROUP[state.route] || null) : null;
  renderNav();
  // Leaving a cards launcher for expand mode: show the group's first item so the
  // main panel never shows a launcher the sidebar can no longer produce.
  if (mode === 'expand' && isGroupRoute(state.route)) {
    const g = groupOfRoute(state.route);
    if (g) { navigate(g.items[0].route); return; }
  }
  void renderRoute();
}

const ctx = {
  state,
  toast,
  openModal,
  closeModal,
  navigate,
  setPage,
  renderNav,
  applyNavMode,
};

// -------------------------------------------------------------- routing
export function navigate(route) {
  state.route = route;
  // In expand mode keep the owning group open so the active item is visible.
  // In cards mode the sidebar has no open group; the launcher lives in main.
  state.open = state.mode === 'expand' ? (ROUTE_GROUP[route] || null) : null;
  renderNav();
  void renderRoute();
  document.querySelector('.sidebar')?.classList.remove('open');
}

export function setPage(node) {
  const page = el('page');
  page.replaceChildren(node);
  page.scrollTop = 0;
  window.scrollTo({ top: 0 });
}

let renderToken = 0;
async function renderRoute() {
  const route = state.route;
  const token = ++renderToken;
  const grp = groupOfRoute(route);
  el('page-title').textContent = grp ? grp.title : (ROUTE_TITLES[route] || 'VanijLiquor');
  setPage(h('div', { class: 'loading' }, h('span', { class: 'spinner' }), 'Loading…'));
  try {
    const node = await screens(route, ctx);
    if (token !== renderToken) return; // superseded by a newer navigation
    if (node) setPage(node);
  } catch (err) {
    if (token !== renderToken) return;
    setPage(h('div', { class: 'card' },
      h('h3', {}, 'Something went wrong'),
      h('p', { class: 'error' }, err.message),
      h('div', { class: 'actions' }, h('button', { class: 'btn', onclick: () => renderRoute() }, 'Retry'))));
  }
}

// -------------------------------------------------------------- sidebar
// 'expand' mode (Option A): the sidebar shows group headers; clicking one
// toggles its items inline (one open at a time); the group owning the current
// route opens on navigation so the active item is always visible.
// 'cards' mode (Option B): the sidebar shows group headers only; clicking one
// opens the group's launcher in the main panel.
function renderNav() {
  const nav = el('sidebar-nav');
  if (!state.user) { nav.replaceChildren(); return; }
  nav.dataset.mode = state.mode;
  const activeGroup = state.mode === 'cards'
    ? (groupOfRoute(state.route)?.title || null)
    : (ROUTE_GROUP[state.route] || null);

  nav.replaceChildren(...NAV.map((g) => {
    const open = state.mode === 'expand' && state.open === g.title;
    const active = g.title === activeGroup;
    const head = h('button', {
      class: 'nav-group-head',
      'aria-expanded': String(open),
      onclick: () => (state.mode === 'cards'
        ? navigate(groupRoute(g.title))
        : (state.open = open ? null : g.title, renderNav())),
    }, groupIcon(g.title), h('span', { class: 'nav-group-title' }, g.title),
      state.mode === 'expand'
        ? h('span', { class: 'nav-chevron' }, svgIcon('chevron', 14))
        : h('span', { class: 'nav-chevron' }, svgIcon('chevron-right', 14)));

    if (state.mode === 'cards') {
      return h('div', { class: `nav-group${active ? ' active' : ''}` }, head);
    }
    const items = h('div', { class: 'nav-items' },
      h('div', { class: 'nav-items-inner' },
        ...g.items.map((it) => h('button', {
          class: `nav-item${it.route === state.route ? ' active' : ''}`,
          dataset: { route: it.route },
          onclick: () => navigate(it.route),
        }, navIcon(it.route, g.title), h('span', {}, it.title)))));
    return h('div', { class: `nav-group${open ? ' open' : ''}${active ? ' active' : ''}` }, head, items);
  }));
}

// -------------------------------------------------------------- auth
async function doLogin(usercode, password) {
  const r = await api.login(usercode, password);
  state.user = r.user;
  localStorage.setItem('user', JSON.stringify(r.user));
  await boot();
}

export async function logout() {
  state.user = null;
  localStorage.removeItem('user');
  el('app-view').hidden = true;
  el('login-view').hidden = false;
  el('login-error').hidden = true;
}

async function boot() {
  el('login-view').hidden = true;
  el('app-view').hidden = false;
  el('who').textContent = `${state.user.username} · ${state.user.accesslevel}`;
  try { state.meta = await api.meta(); } catch { state.meta = null; }
  // Adopt the company's configured nav mode unless a `?nav=` URL override is set.
  if (!urlNavMode() && NAV_MODES.includes(state.meta?.navMode)) {
    state.mode = state.meta.navMode;
    localStorage.setItem('navMode', state.mode);
  }
  setServerToday(state.meta?.today);
  el('company-pill').textContent = `Company ${state.meta?.company ?? 1}`;
  el('year-pill').textContent = `Year ${state.meta?.year ?? 2024}`;
  el('clock-warning').hidden = !state.meta?.clockRolledBack;
  state.open = state.mode === 'expand' ? (ROUTE_GROUP[state.route] || null) : null;
  renderNav();
  // First run: activate the bundled 90-day demo (bound to this machine) and show
  // the licence screen so the customer sees the term and can import a real
  // licence or read the fingerprint. The demo is one-time per machine, so a
  // machine whose trial was already used is taken to the licence screen instead
  // of being silently re-activated.
  let firstRun = false;
  try {
    state.license = await api.license();
    if (state.license && !state.license.present) {
      firstRun = true;
      if (!state.license.trial_started) {
        state.license = await api.activateDemo();
      }
    }
  } catch { state.license = null; }
  renderLicenseBanner();
  renderLicencePill();
  state.route = firstRun ? 'license' : 'dashboard';
  await renderRoute();
}

/** Persistent licence banner (hidden when the licence is healthy). */
function renderLicenseBanner() {
  const b = el('license-banner');
  const lic = state.license;
  if (!lic || lic.severity === 'ok') { b.hidden = true; return; }
  const msg = lic.state === 'none'
    ? (lic.trial_expired
      ? 'The 90-day demo has ended and no licence is active — import a licence to post transactions.'
      : lic.trial_started
        ? `Demo already used on this machine (${lic.trial_days_remaining} day(s) left) — import a licence to continue after it ends.`
        : 'No licence is active — start the demo or import a licence to post transactions.')
    : lic.state === 'grace' ? `Licence past its term; running in grace (${lic.grace_remaining} day(s) left).`
      : lic.state === 'invalid' ? 'Licence failed verification. Import a valid licence.'
        : 'Licence expired — posting is disabled until you renew.';
  b.className = `license-banner${lic.severity === 'warn' ? ' warn' : ''}`;
  b.replaceChildren(
    h('span', {}, msg),
    h('button', { class: 'btn small', onclick: () => navigate('license') }, 'Manage licence'));
  b.hidden = false;
}

/** Compact licence state in the top bar (hidden when the licence is healthy). */
function renderLicencePill() {
  const p = el('licence-pill');
  const lic = state.license;
  if (!lic || lic.severity === 'ok') { p.hidden = true; return; }
  const text = lic.state === 'none'
    ? (lic.trial_started ? `Demo · ${lic.trial_days_remaining}d left` : 'No licence')
    : lic.state === 'grace' ? `Grace · ${lic.grace_remaining}d left`
      : lic.state === 'invalid' ? 'Licence invalid' : 'Licence expired';
  p.textContent = text;
  p.hidden = false;
}

// -------------------------------------------------------------- wiring
/** Ask the desktop shell where it put the DB and for the start-up log tail. */
async function shellDiagnostics() {
  const invoke = window.__TAURI__?.core?.invoke || window.__TAURI__?.invoke;
  if (!invoke) return null;
  try { return await invoke('backend_diagnostics'); } catch { return null; }
}

function showDiagnostics(text) {
  document.querySelectorAll('.diag').forEach((n) => n.remove());
  if (!text) return;
  const pre = document.createElement('pre');
  pre.className = 'diag';
  pre.textContent = text;
  el('login-form').appendChild(pre);
}

function init() {
  applyTheme(getTheme());
  const themeBtn = el('theme-toggle');
  const paintThemeBtn = () => themeBtn.replaceChildren(svgIcon(getTheme() === 'dark' ? 'sun' : 'moon', 16));
  paintThemeBtn();
  themeBtn.addEventListener('click', () => { toggleTheme(); paintThemeBtn(); });

  el('login-form').addEventListener('submit', async (e) => {
    e.preventDefault();
    const err = el('login-error');
    err.hidden = true;
    try {
      await doLogin(el('login-usercode').value.trim() || 'A00001', el('login-password').value || 'admin');
    } catch (ex) {
      const unreachable = /fetch/i.test(ex.message);
      err.textContent = unreachable
        ? `Cannot reach the local server at ${api.base}. It may still be starting — wait a moment and try again.`
        : ex.message;
      err.hidden = false;
      if (unreachable) showDiagnostics(await shellDiagnostics());
    }
  });
  el('logout-btn').addEventListener('click', logout);
  el('menu-toggle').addEventListener('click', () => document.querySelector('.sidebar').classList.toggle('open'));
  el('modal-close').addEventListener('click', closeModal);
  el('modal').addEventListener('click', (e) => { if (e.target === el('modal')) closeModal(); });
  document.addEventListener('keydown', (e) => { if (e.key === 'Escape') closeModal(); });

  // Restore a previous session if the API is reachable.
  const saved = localStorage.getItem('user');
  if (saved) {
    try {
      state.user = JSON.parse(saved);
      api.health().then(boot).catch(() => { state.user = null; localStorage.removeItem('user'); });
      return;
    } catch { localStorage.removeItem('user'); }
  }
}

init();
