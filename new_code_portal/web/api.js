// Thin HTTP client for the portal backend.
//
// The UI is served by web/server.js on the same origin as the backend (it
// proxies /api), so the base is empty by default. Overridable with
// window.__API_BASE__ when the frontend is served from a different host.

const BASE = (typeof window !== 'undefined' && window.__API_BASE__) || '';

async function req(method, path, body) {
  const opts = { method, headers: {} };
  if (body !== undefined) {
    opts.headers['content-type'] = 'application/json';
    opts.body = JSON.stringify(body);
  }
  const res = await fetch(BASE + path, opts);
  const json = await res.json().catch(() => ({ ok: false, error: 'Invalid JSON from server' }));
  if (!json.ok) throw new Error(json.error || `HTTP ${res.status}`);
  return json.data;
}

const qs = (o = {}) => {
  const p = new URLSearchParams();
  for (const [k, v] of Object.entries(o)) if (v !== undefined && v !== null && v !== '') p.set(k, v);
  const s = p.toString();
  return s ? `?${s}` : '';
};

export const api = {
  base: BASE,
  health: () => req('GET', '/api/health'),
  meta: () => req('GET', '/api/meta'),

  catalogue: () => req('GET', '/api/catalogue'),
  plans: () => req('GET', '/api/plans'),
  periods: () => req('GET', '/api/periods'),
  modules: () => req('GET', '/api/modules'),
  addons: () => req('GET', '/api/addons'),
  comparison: () => req('GET', '/api/comparison'),
  quote: (q) => req('POST', '/api/quote', q),

  businesses: (filter = {}) => req('GET', `/api/businesses${qs(filter)}`),
  business: (id) => req('GET', `/api/businesses/${id}`),
  createBusiness: (b) => req('POST', '/api/businesses', b),
  updateBusiness: (id, b) => req('PUT', `/api/businesses/${id}`, b),
  setModules: (id, modules) => req('POST', `/api/businesses/${id}/modules`, { modules }),
  addModule: (id, code) => req('POST', `/api/businesses/${id}/modules/${code}`),
  removeModule: (id, code) => req('DELETE', `/api/businesses/${id}/modules/${code}`),

  licenses: (filter = {}) => req('GET', `/api/licenses${qs(filter)}`),
  license: (id) => req('GET', `/api/licenses/${id}`),
  issueLicense: (payload) => req('POST', '/api/licenses', payload),
  activate: (id, machine) => req('POST', `/api/licenses/${id}/activate`, { machine }),
  verifyLicense: (id, machine) => req('POST', `/api/licenses/${id}/verify`, { machine }),
  revoke: (id) => req('POST', `/api/licenses/${id}/revoke`),
  resign: (id) => req('POST', `/api/licenses/${id}/resign`),
  licenseKey: () => req('GET', '/api/license-key'),
  licenseFileUrl: (id) => `${BASE}/api/licenses/${id}/file`,

  dashboard: () => req('GET', '/api/dashboard'),
  audit: (limit = 100) => req('GET', `/api/audit${qs({ limit })}`),
};

export default api;
