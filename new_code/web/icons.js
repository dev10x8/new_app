// Inline stroke icons for the sidebar. Kept dependency-free: a single <path>
// per icon on a 24x24 grid, stroked with currentColor so the active/inactive
// nav colours apply automatically.

const NS = 'http://www.w3.org/2000/svg';

const PATHS = {
  dot: 'M12 12h.01',
  dashboard: 'M3 12h7V3H3zM14 21h7v-9h-7zM14 3h7v6h-7zM3 21h7v-6H3z',
  settings: 'M12 15a3 3 0 1 0 0-6 3 3 0 0 0 0 6zM19.4 15a1.7 1.7 0 0 0 .3 1.9l.1.1a2 2 0 1 1-2.8 2.8l-.1-.1a1.7 1.7 0 0 0-1.9-.3 1.7 1.7 0 0 0-1 1.5V21a2 2 0 1 1-4 0v-.1A1.7 1.7 0 0 0 8.9 19a1.7 1.7 0 0 0-1.9.3l-.1.1a2 2 0 1 1-2.8-2.8l.1-.1a1.7 1.7 0 0 0 .3-1.9 1.7 1.7 0 0 0-1.5-1H3a2 2 0 1 1 0-4h.1A1.7 1.7 0 0 0 5 8.9a1.7 1.7 0 0 0-.3-1.9l-.1-.1a2 2 0 1 1 2.8-2.8l.1.1a1.7 1.7 0 0 0 1.9.3H9.5a1.7 1.7 0 0 0 1-1.5V3a2 2 0 1 1 4 0v.1a1.7 1.7 0 0 0 1 1.5 1.7 1.7 0 0 0 1.9-.3l.1-.1a2 2 0 1 1 2.8 2.8l-.1.1a1.7 1.7 0 0 0-.3 1.9v.1a1.7 1.7 0 0 0 1.5 1H21a2 2 0 1 1 0 4h-.1a1.7 1.7 0 0 0-1.5 1z',
  box: 'M3 7l9-4 9 4-9 4zM3 7v10l9 4 9-4V7',
  receipt: 'M5 3h14v18l-3-2-2 2-2-2-2 2-2-2-3 2zM9 8h6M9 12h6',
  chart: 'M4 20V10M10 20V4M16 20v-7M22 20H2',
  wrench: 'M14.7 6.3a4 4 0 0 1-5.4 5.4L4 17l3 3 5.3-5.3a4 4 0 0 1 5.4-5.4l-3 3-2-2z',
  help: 'M12 22a10 10 0 1 0 0-20 10 10 0 0 0 0 20zM9.5 9a2.5 2.5 0 1 1 3.5 2.3c-.8.4-1 .9-1 1.7M12 17h.01',
  users: 'M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2M9 11a4 4 0 1 0 0-8 4 4 0 0 0 0 8zM22 21v-2a4 4 0 0 0-3-3.9M16 3.1a4 4 0 0 1 0 7.8',
  tag: 'M20.6 13.4 12 22l-9-9V4h9zM7 7h.01',
  truck: 'M1 3h15v13H1zM16 8h4l3 3v5h-7zM5.5 21a2 2 0 1 0 0-4 2 2 0 0 0 0 4zM18.5 21a2 2 0 1 0 0-4 2 2 0 0 0 0 4z',
  bank: 'M3 21h18M4 21V9l8-6 8 6v12M9 21v-6h6v6',
  file: 'M6 2h9l5 5v15H6zM15 2v5h5',
  store: 'M3 9l1.5-6h15L21 9M4 9v11h16V9M9 20v-6h6v6',
  calculator: 'M6 2h12v20H6zM9 6h6M9 11h.01M12 11h.01M15 11h.01M9 15h.01M12 15h.01M15 15h.01',
  cloud: 'M17 18a4 4 0 0 0 0-8 6 6 0 0 0-11.6 2A3.5 3.5 0 0 0 6 18z',
  shield: 'M12 3l8 4v5c0 5-3.5 8-8 9-4.5-1-8-4-8-9V7z',
  grid: 'M4 4h7v7H4zM13 4h7v7h-7zM4 13h7v7H4zM13 13h7v7h-7z',
  sliders: 'M4 21v-7M4 10V3M12 21v-9M12 8V3M20 21v-5M20 12V3M1 14h6M9 8h6M17 16h6',
  key: 'M21 2l-2 2M15.5 8.5 19 5l-2-2-3.5 3.5M15.5 8.5a5 5 0 1 1-7 7 5 5 0 0 1 7-7z',
  bell: 'M18 8a6 6 0 0 0-12 0c0 7-3 9-3 9h18s-3-2-3-9M13.7 21a2 2 0 0 1-3.4 0',
  db: 'M12 8c4.4 0 8-1.3 8-3s-3.6-3-8-3-8 1.3-8 3 3.6 3 8 3zM4 5v14c0 1.7 3.6 3 8 3s8-1.3 8-3V5M4 12c0 1.7 3.6 3 8 3s8-1.3 8-3',
  moon: 'M21 12.8A9 9 0 1 1 11.2 3a7 7 0 0 0 9.8 9.8z',
  sun: 'M12 17a5 5 0 1 0 0-10 5 5 0 0 0 0 10zM12 1v2M12 21v2M4.2 4.2l1.4 1.4M18.4 18.4l1.4 1.4M1 12h2M21 12h2M4.2 19.8l1.4-1.4M18.4 5.6l1.4-1.4',
  search: 'M11 19a8 8 0 1 0 0-16 8 8 0 0 0 0 16zM21 21l-4.3-4.3',
  chevron: 'M6 9l6 6 6-6',
  plus: 'M12 5v14M5 12h14',
  menu: 'M4 6h16M4 12h16M4 18h16',
  close: 'M6 6l12 12M18 6 6 18',
  logout: 'M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4M16 17l5-5-5-5M21 12H9',
};

/** Routes with a bespoke icon; everything else falls back to its group. */
const ROUTE_ICON = {
  dashboard: 'dashboard',
  'dash:sales': 'dashboard',
  sysadmin: 'sliders',
  params: 'sliders',
  users: 'users',
  rights: 'key',
  'new-year': 'file',
  'excise-audit': 'shield',
  'deleted-bills': 'receipt',
  items: 'box',
  inventory: 'db',
  'state-master': 'grid',
  'bulk-import': 'cloud',
  'pack-sizes': 'box',
  schemes: 'tag',
  sale: 'receipt',
  purchase: 'truck',
  'sales-register': 'file',
  'parcha-import': 'file',
  'transfer-sale': 'truck',
  'transfer-receive': 'truck',
  'opening-stock': 'db',
  'stock-transfer': 'truck',
  'account-replace': 'sliders',
  upi: 'bank',
  delivery: 'truck',
  'change-password': 'key',
  backup: 'db',
  restore: 'db',
  migration: 'db',
  tally: 'file',
  'tally-sync': 'cloud',
  'excise-sync': 'cloud',
  'form-m22': 'file',
  gst: 'file',
  messages: 'bell',
  sync: 'cloud',
  license: 'key',
  about: 'help',
  help: 'help',
  updates: 'file',
  calc: 'calculator',
  'dash:stock': 'db',
  'dash:purchase': 'truck',
  'dash:scheme': 'tag',
  'landed-cost': 'calculator',
  'clean-print': 'wrench',
  pml: 'file',
  brs: 'bank',
  fine: 'receipt',
  breakage: 'box',
  'on-account': 'bank',
};

const GROUP_ICON = {
  Main: 'dashboard',
  Admin: 'sliders',
  Masters: 'box',
  Transactions: 'receipt',
  Reports: 'chart',
  'Dash Board': 'dashboard',
  Tools: 'wrench',
  Help: 'help',
};

/** An inline SVG icon element (`name` falls back to a neutral dot). */
export function svgIcon(name, size = 16) {
  const svg = document.createElementNS(NS, 'svg');
  svg.setAttribute('viewBox', '0 0 24 24');
  svg.setAttribute('fill', 'none');
  svg.setAttribute('stroke', 'currentColor');
  svg.setAttribute('stroke-width', '2');
  svg.setAttribute('stroke-linecap', 'round');
  svg.setAttribute('stroke-linejoin', 'round');
  svg.setAttribute('width', size);
  svg.setAttribute('height', size);
  svg.setAttribute('aria-hidden', 'true');
  const path = document.createElementNS(NS, 'path');
  path.setAttribute('d', PATHS[name] || PATHS.dot);
  svg.append(path);
  return svg;
}

/** The icon for a nav entry, resolved from its route then its group. */
export function navIcon(route, group) {
  return svgIcon(ROUTE_ICON[route] || GROUP_ICON[group] || 'dot');
}

/** The icon for a nav group header. */
export function groupIcon(group) {
  return svgIcon(GROUP_ICON[group] || 'dot');
}
