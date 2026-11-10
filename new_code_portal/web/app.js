import { api } from './api.js';
import { NAV, ROUTE_TITLES } from './menu.js';
import { h } from './ui.js';
import { screens } from './screens.js';

const state = { meta: null, catalogue: null, route: 'dashboard' };
const el = (id) => document.getElementById(id);

// -------------------------------------------------------------- toast
let toastTimer;
export function toast(msg, kind = '') {
  const t = el('toast');
  t.textContent = msg;
  t.className = `toast ${kind}`.trim();
  t.hidden = false;
  clearTimeout(toastTimer);
  toastTimer = setTimeout(() => { t.hidden = true; }, 3600);
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
const ctx = { state, toast, openModal, closeModal, navigate, setPage };

// -------------------------------------------------------------- routing
export function navigate(route) {
  state.route = route;
  closeModal();
  if (location.hash.slice(1) !== route) location.hash = route;
  void renderRoute();
  document.querySelectorAll('.nav-item').forEach((b) => {
    b.classList.toggle('active', b.dataset.route === route);
  });
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
  el('page-title').textContent = ROUTE_TITLES[route] || 'VanijLiquor Portal';
  setPage(h('div', { class: 'loading' }, h('span', { class: 'spinner' }), 'Loading…'));
  try {
    const node = await screens(route, ctx);
    if (token !== renderToken) return;
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
function buildNav() {
  const nav = el('sidebar-nav');
  nav.replaceChildren(...NAV.map((g) => h('div', { class: 'nav-group' },
    h('div', { class: 'nav-group-title' }, g.title),
    ...g.items.map((it) => h('button', {
      class: `nav-item${it.route === state.route ? ' active' : ''}`,
      dataset: { route: it.route },
      onclick: () => navigate(it.route),
    }, it.title)))));
}

// -------------------------------------------------------------- boot
async function boot() {
  el('app-view').hidden = false;
  try {
    state.meta = await api.meta();
    state.catalogue = await api.catalogue();
  } catch (err) {
    toast(`Backend unreachable: ${err.message}`, 'error');
  }
  buildNav();
  const hash = location.hash.slice(1);
  const known = NAV.flatMap((g) => g.items.map((i) => i.route));
  navigate(known.includes(hash) ? hash : 'dashboard');
}

window.addEventListener('hashchange', () => {
  const route = location.hash.slice(1);
  if (route && route !== state.route) navigate(route);
});

el('menu-toggle')?.addEventListener('click', () => el('sidebar-nav')?.parentElement?.classList.toggle('open'));
el('modal-close')?.addEventListener('click', closeModal);

void boot();
