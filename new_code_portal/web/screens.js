import { api } from './api.js';
import {
  h, money, rupees, date, statusBadge, card, stat, table,
  checkboxList, checkedValues, downloadJson, loading,
} from './ui.js';

// Each screen is `(ctx) => Node | Promise<Node>`.
const screensMap = {
  dashboard, businesses, business, licenses, onboard, plans, modules, issue, verify, download, key, audit,
};

export async function screens(route, ctx) {
  const fn = screensMap[route];
  if (!fn) return h('div', { class: 'card' }, h('h3', {}, 'Not found'), h('p', { class: 'muted' }, `No screen for “${route}”.`));
  return fn(ctx);
}

// ---------------------------------------------------------------- dashboard
async function dashboard(ctx) {
  const d = await api.dashboard();
  const b = d.businesses;
  const l = d.licenses;
  return h('div', {},
    h('div', { class: 'grid cols-4' },
      stat('Businesses', b.total, { hint: `${b.activeBusinesses} active` }),
      stat('Licences', l.issued + l.active, { hint: `${l.active} active · ${l.issued} issued` }),
      stat('Activations', l.activations),
      stat('MRR (active)', b.mrr, { money: true, hint: 'monthly recurring' })),
    h('div', { class: 'grid cols-2', style: 'margin-top:16px' },
      card('Quick actions', h('div', { class: 'list-menu' },
        h('button', { onclick: () => ctx.navigate('onboard') }, '＋ Onboard a business'),
        h('button', { onclick: () => ctx.navigate('issue') }, '⎙ Issue a licence'),
        h('button', { onclick: () => ctx.navigate('verify') }, '✓ Activate / verify'),
        h('button', { onclick: () => ctx.navigate('download') }, '⭳ Download the app'))),
      card('Licence health', h('div', { class: 'stat-list' },
        row('Issued', l.issued), row('Active', l.active), row('Revoked', l.revoked),
        row('In grace', l.inGrace ?? 0),
        row('Expiring ≤ 30 days', l.expiringSoon),
        row('Seats in use', l.seatsUsed ?? 0),
        row('Signing key', l.usingProductionKey ? 'production' : 'development')))),
    h('div', { class: 'section-title' }, 'Businesses by plan'),
    planBars(b.byPlan),
    h('div', { class: 'section-title' }, 'Recent activity'),
    auditTable(d.recent));
}

function row(k, v) {
  return h('div', { class: 'kv' }, h('span', { class: 'k' }, k), h('span', { class: 'v' }, String(v)));
}

function planBars(byPlan = []) {
  if (!byPlan.length) return h('div', { class: 'card' }, h('div', { class: 'empty' }, 'No businesses yet.'));
  const max = Math.max(...byPlan.map((p) => p.n));
  return h('div', { class: 'card' }, h('div', { class: 'bars' },
    ...byPlan.map((p) => h('div', { class: 'bar-row' },
      h('span', { class: 'bar-label' }, p.plan_code),
      h('span', { class: 'bar', style: `width:${Math.round((p.n / max) * 100)}%` }),
      h('span', { class: 'bar-value' }, String(p.n))))));
}

// ---------------------------------------------------------------- businesses
async function businesses(ctx) {
  const state = { q: '', status: '' };
  const host = h('div', {});
  const render = async () => {
    host.replaceChildren(loading());
    const res = await api.businesses({ q: state.q, status: state.status });
    host.replaceChildren(
      h('div', { class: 'toolbar' },
        h('label', { class: 'field' }, 'Search',
          h('input', { value: state.q, placeholder: 'name, code or email', oninput: (e) => { state.q = e.target.value; } })),
        h('label', { class: 'field' }, 'Status',
          sel(['', 'draft', 'onboarding', 'active', 'suspended'], state.status, (v) => { state.status = v; void render(); })),
        h('button', { class: 'btn', onclick: () => void render() }, 'Search'),
        h('div', { class: 'spacer' }),
        h('button', { class: 'btn primary', onclick: () => ctx.navigate('onboard') }, '＋ New business')),
      h('p', { class: 'muted small' }, `${res.total} business(es)`),
      h('div', { class: 'lic-cards' }, ...res.rows.map((r) => h('div', { class: 'card lic' },
        h('div', { class: 'lic-head' }, h('strong', {}, r.legalname), statusBadge(r.status)),
        h('div', { class: 'muted small' }, `${r.business_code} · ${r.city || '—'} · ${r.plan_code}/${r.period_code}`),
        h('div', { class: 'muted small' }, `${r.email || ''} ${r.phone || ''}`),
        h('div', { class: 'actions' },
          h('button', { class: 'btn small', onclick: () => { ctx.state.businessId = r.id; ctx.navigate('business'); } }, 'Open'))))));
  };
  await render();
  return host;
}

function statusKind(s) {
  return { active: 'ok', issued: 'warn', onboarding: 'warn', draft: '', suspended: 'danger', revoked: 'danger' }[s] ?? '';
}

function sel(options, value, onchange) {
  const s = h('select', { onchange: (e) => onchange(e.target.value) });
  for (const o of options) {
    const opt = h('option', { value: o }, o || 'All');
    if (String(o) === String(value)) opt.selected = true;
    s.append(opt);
  }
  return s;
}

// ---------------------------------------------------------------- business detail
async function business(ctx) {
  const id = ctx.state.businessId;
  if (!id) return h('div', { class: 'card' }, h('p', {}, 'No business selected.'));
  const b = await api.business(id);
  const cat = ctx.state.catalogue;
  const moduleNames = new Map(cat.modules.map((m) => [m.code, m.name]));
  const licTable = await licencesFor(ctx, b.id);

  return h('div', {},
    h('div', { class: 'grid cols-2' },
      card('Business', h('div', { class: 'stat-list' },
        row('Code', b.business_code), row('Legal name', b.legalname), row('Trade name', b.tradename || '—'),
        row('Owner', b.ownername || '—'), row('Email', b.email || '—'), row('Phone', b.phone || '—'),
        row('GSTIN', b.gstin || '—'), row('City / State', `${b.city || '—'} / ${b.state || '—'}`),
        row('Type', b.businesstype || '—'), row('Outlets', b.outlets),
        h('div', { class: 'kv' }, h('span', { class: 'k' }, 'Status'), statusBadge(b.status)))),
      card('Plan & entitlement', h('div', {},
        h('div', { class: 'stat-list' },
          row('Plan', b.plan_code), row('Period', b.period_code),
          row('Entitled modules', b.entitlements.length)),
        h('div', { class: 'section-title' }, 'Modules'),
        h('div', { class: 'chips' }, ...b.entitlements.map((c) => h('span', { class: 'chip' }, moduleNames.get(c) || c))),
        h('div', { class: 'actions' },
          h('button', { class: 'btn primary', onclick: () => { ctx.state.businessId = b.id; ctx.navigate('issue'); } }, 'Issue licence'),
          h('button', { class: 'btn', onclick: () => { ctx.state.businessId = b.id; ctx.navigate('modules'); } }, 'Edit modules')))),
    ),
    h('div', { class: 'section-title' }, 'Licences'),
    licTable);
}

async function licencesFor(ctx, businessId) {
  const rows = await api.licenses({ business_id: businessId });
  return table([
    { title: 'Licence', key: 'license_id' },
    { title: 'Plan', key: 'plan_code' },
    { title: 'Status', value: (r) => r.status, badge: (r) => statusKind(r.status) },
    { title: 'Valid until', value: (r) => date(r.valid_until) },
    { title: 'Activations', value: (r) => r.activations.length, num: true },
  ], rows);
}

// ---------------------------------------------------------------- licences
async function licenses(ctx) {
  const rows = await api.licenses({});
  return h('div', {},
    h('div', { class: 'toolbar' },
      h('button', { class: 'btn primary', onclick: () => ctx.navigate('issue') }, '＋ Issue licence'),
      h('button', { class: 'btn', onclick: () => ctx.navigate('verify') }, '✓ Activate / verify')),
    table([
      { title: 'Licence', key: 'license_id' },
      { title: 'Business', key: 'business_code' },
      { title: 'Plan', key: 'plan_code' },
      { title: 'Status', value: (r) => r.status, badge: (r) => statusKind(r.status) },
      { title: 'State', value: (r) => r.state, badge: (r) => statusKind(r.state) },
      { title: 'Machine', value: (r) => `${r.machine_hash.slice(0, 12)}…` },
      { title: 'Valid until', value: (r) => date(r.valid_until) },
      { title: 'Seats', value: (r) => `${r.activations.filter((a) => !a.released_at).length}/${r.seat_limit}` },
    ], rows),
    detailList(ctx, rows));
}

function detailList(ctx, list) {
  return h('div', { class: 'lic-cards' }, ...list.map((r) => h('div', { class: 'card lic' },
    h('div', { class: 'lic-head' }, h('strong', {}, r.license_id), statusBadge(r.state)),
    h('div', { class: 'muted small' }, `${r.business_code} · ${r.plan_code} · until ${date(r.valid_until)}`),
    h('div', { class: 'actions' },
      h('button', { class: 'btn small', onclick: () => openLicence(ctx, r.license_id) }, 'View'),
      h('a', { class: 'btn small', href: api.licenseFileUrl(r.license_id), download: `${r.license_id}.vlic` }, 'Download')))));
}

// ---------------------------------------------------------------- onboard wizard
async function onboard(ctx) {
  const cat = ctx.state.catalogue || await api.catalogue();
  const draft = { legalname: '', tradename: '', ownername: '', email: '', phone: '', gstin: '', panno: '',
    address1: '', city: '', district: '', state: '', statecode: '', pincode: '',
    businesstype: 'retail', outlets: 1, plan_code: 'standard', period_code: 'monthly', modules: [] };
  let step = 0;
  const steps = ['Business', 'Plan', 'Modules', 'Review'];
  const host = h('div', { class: 'card' });
  const body = h('div', {});
  const stepBar = h('div', { class: 'steps' });

  const draw = () => {
    stepBar.replaceChildren(...steps.map((s, i) => h('div', { class: `step${i === step ? ' on' : ''}${i < step ? ' done' : ''}` },
      h('span', { class: 'num' }, String(i + 1)), s)));
    body.replaceChildren([businessStep, planStep, modulesStep, reviewStep][step]());
  };

  const next = () => { step = Math.min(steps.length - 1, step + 1); draw(); };
  const prev = () => { step = Math.max(0, step - 1); draw(); };

  const nav = (extra) => h('div', { class: 'actions' },
    step > 0 ? h('button', { class: 'btn', onclick: prev }, 'Back') : null,
    extra,
    step < steps.length - 1 ? h('button', { class: 'btn primary', onclick: next }, 'Continue') : null);

  function businessStep() {
    const f = h('div', { class: 'form' });
    const row = (...cells) => h('div', { class: 'row' }, ...cells);
    const field = (label, name, opts = {}) => h('label', { class: 'field' }, label,
      h('input', { name, value: draft[name] ?? '', placeholder: opts.ph || '', oninput: (e) => { draft[name] = e.target.value; } }));
    f.append(
      row(field('Legal name *', 'legalname'), field('Trade name', 'tradename')),
      row(field('Owner name', 'ownername'), field('Email *', 'email', { ph: 'owner@shop.in' })),
      row(field('Phone *', 'phone'), field('GSTIN', 'gstin')),
      row(field('PAN', 'panno'), h('label', { class: 'field' }, 'Business type',
        sel(['retail', 'wholesale', 'bar', 'club'], draft.businesstype, (v) => { draft.businesstype = v; }))),
      row(field('Address', 'address1'), field('City', 'city')),
      row(field('District', 'district'), field('State', 'state')),
      row(field('State code', 'statecode', { ph: 'e.g. 27' }), field('PIN', 'pincode')),
      row(h('label', { class: 'field' }, 'Outlets',
        h('input', { type: 'number', min: 1, value: draft.outlets, oninput: (e) => { draft.outlets = Number(e.target.value) || 1; } }))),
      nav());
    return f;
  }

  function planStep() {
    const wrap = h('div', {});
    const render = () => {
      wrap.replaceChildren(
        h('p', { class: 'muted' }, 'Choose a plan and billing term.'),
        h('div', { class: 'plan-grid' }, ...cat.plans.map((p) => h('div', {
          class: `plan${draft.plan_code === p.code ? ' selected' : ''}${p.popular ? ' popular' : ''}`,
          onclick: () => { draft.plan_code = p.code; render(); },
        },
          p.popular ? h('span', { class: 'ribbon' }, 'Most popular') : null,
          h('h3', {}, p.name),
          h('p', { class: 'muted small' }, p.tagline),
          h('div', { class: 'price' }, rupees(p.monthly), h('span', { class: 'muted small' }, '/month'))))),
        h('div', { class: 'section-title' }, 'Billing term'),
        h('div', { class: 'segmented' }, ...cat.periods.map((per) => h('button', {
          class: `seg${draft.period_code === per.code ? ' on' : ''}`,
          onclick: () => { draft.period_code = per.code; render(); },
        }, per.label))),
        quoteBox(cat, draft),
        nav());
    };
    render();
    return wrap;
  }

  function modulesStep() {
    const wrap = h('div', {});
    const bundled = new Set(cat.plans.find((p) => p.code === draft.plan_code)?.modules || []);
    const options = cat.modules.map((m) => ({
      value: m.code,
      label: `${m.name}${bundled.has(m.code) ? '  ✓ included' : ''}`,
      hint: bundled.has(m.code) ? m.category : `${m.category}${m.addon_price ? ` · +${rupees(m.addon_price)}` : ''}`,
    }));
    wrap.append(
      h('p', { class: 'muted' }, 'Tick the modules this shop should be entitled to. Items already in the plan are pre-marked; extras are quoted as add-ons.'),
      checkboxList('modules', options, [...bundled, ...draft.modules]),
      nav());
    wrap.addEventListener('change', () => { draft.modules = checkedValues(wrap, 'modules'); });
    draft.modules = [...bundled, ...draft.modules];
    return wrap;
  }

  function reviewStep() {
    const q = quoteFor(cat, draft);
    return h('div', {},
      h('div', { class: 'grid cols-2' },
        card('Business', h('div', { class: 'stat-list' },
          row('Legal name', draft.legalname || '—'), row('Owner', draft.ownername || '—'),
          row('Email', draft.email || '—'), row('Phone', draft.phone || '—'),
          row('GSTIN', draft.gstin || '—'), row('City', draft.city || '—'))),
        card('Subscription', h('div', { class: 'stat-list' },
          row('Plan', `${draft.plan_code} (${draft.period_code})`),
          row('Base', rupees(q.base)), row('Add-ons', rupees(q.addons_total)),
          row('GST 18%', rupees(q.gst)), row('Total', rupees(q.total)))),
      ),
      h('div', { class: 'actions' },
        h('button', { class: 'btn', onclick: prev }, 'Back'),
        h('button', { class: 'btn primary', onclick: submit }, 'Create business')),
    );
  }

  async function submit() {
    if (!draft.legalname) { ctx.toast('Legal name is required', 'error'); step = 0; draw(); return; }
    try {
      const b = await api.createBusiness({ ...draft, modules: undefined });
      await api.setModules(b.id, draft.modules || []);
      ctx.toast(`Created ${b.business_code}`, 'ok');
      ctx.state.businessId = b.id;
      ctx.navigate('business');
    } catch (err) { ctx.toast(err.message, 'error'); }
  }

  host.append(h('h3', {}, 'Onboard a new business'), stepBar, body);
  draw();
  return host;
}

function quoteFor(cat, draft) {
  const plan = cat.plans.find((p) => p.code === draft.plan_code) || cat.plans[0];
  const period = cat.periods.find((p) => p.code === draft.period_code) || cat.periods[0];
  const base = plan.prices[period.code] ?? plan.monthly;
  const bundled = new Set(plan.modules);
  const selected = draft.modules || [];
  const extra = cat.modules.filter((m) => selected.includes(m.code) && !bundled.has(m.code) && m.addon_price > 0);
  const addons_total = extra.reduce((s, m) => s + m.addon_price, 0);
  const subtotal = base + addons_total;
  const gst = Math.round(subtotal * 0.18);
  return { plan, period, base, addons_total, subtotal, gst, total: subtotal + gst };
}

function quoteBox(cat, draft) {
  const q = quoteFor(cat, draft);
  return h('div', { class: 'card quote' },
    h('div', { class: 'stat-list' },
      row('Base price', rupees(q.base)),
      row('Add-ons', rupees(q.addons_total)),
      row('GST 18%', rupees(q.gst)),
      h('div', { class: 'kv total' }, h('span', { class: 'k' }, 'Total'), h('span', { class: 'v' }, rupees(q.total)))));
}

// ---------------------------------------------------------------- plans
async function plans(ctx) {
  const cat = ctx.state.catalogue || await api.catalogue();
  let period = 'monthly';
  const host = h('div', {});
  const render = () => host.replaceChildren(
    h('div', { class: 'segmented' }, ...cat.periods.map((per) => h('button', {
      class: `seg${period === per.code ? ' on' : ''}`,
      onclick: () => { period = per.code; render(); },
    }, per.label))),
    h('div', { class: 'plan-grid' }, ...cat.plans.map((p) => h('div', { class: `plan${p.popular ? ' popular' : ''}` },
      p.popular ? h('span', { class: 'ribbon' }, 'Most popular') : null,
      h('h3', {}, p.name),
      h('p', { class: 'muted small' }, p.tagline),
      h('div', { class: 'price' }, rupees(p.prices[period] ?? p.monthly),
        h('span', { class: 'muted small' }, period === 'monthly' ? '/month' : ` / ${label(period)}`)),
      h('ul', { class: 'ticks' }, ...p.highlights.map((x) => h('li', {}, x))),
      h('button', { class: 'btn primary', onclick: () => ctx.navigate('onboard') }, 'Choose ' + p.name)))),
    h('div', { class: 'section-title' }, 'Compare plans'),
    comparisonTable(cat.comparison),
    h('div', { class: 'section-title' }, 'Add-ons'),
    addonTable(cat.addons));
  render();
  return host;
}

const label = (code) => ({ monthly: 'month', oneYear: 'year', twoYears: '2 years', threeYears: '3 years' }[code] || code);

function comparisonTable(groups) {
  const cols = [{ title: 'Feature', key: 'name' }, { title: 'Basic', value: (r) => tick(r.basic) },
    { title: 'Standard', value: (r) => tick(r.standard) }, { title: 'Premium', value: (r) => tick(r.premium) }];
  const wrap = h('div', {});
  for (const g of groups) {
    wrap.append(h('div', { class: 'section-title small' }, g.category), table(cols, g.features));
  }
  return wrap;
}
const tick = (v) => (v ? '✓' : '—');

function addonTable(addons) {
  return table([
    { title: 'Add-on', key: 'name' },
    { title: 'Detail', key: 'detail' },
    { title: 'Price', value: (r) => rupees(r.price) },
    { title: 'Period', key: 'period' },
  ], addons);
}

// ---------------------------------------------------------------- modules
async function modules(ctx) {
  const cat = ctx.state.catalogue || await api.catalogue();
  const businessId = ctx.state.businessId;
  if (businessId) return moduleEditor(ctx, cat, businessId);

  const byCat = new Map();
  for (const m of cat.modules) { if (!byCat.has(m.category)) byCat.set(m.category, []); byCat.get(m.category).push(m); }
  return h('div', {},
    h('p', { class: 'muted' }, 'The full module catalogue. Bundles are defined per plan; extras are quoted as add-ons. Open a business to change what it is entitled to.'),
    ...[...byCat.entries()].flatMap(([c, list]) => [
      h('div', { class: 'section-title' }, c),
      h('div', { class: 'grid cols-3' }, ...list.map((m) => card(m.name,
        h('div', {},
          h('p', { class: 'muted small' }, m.description),
          m.addon_price ? h('span', { class: 'chip' }, `Add-on ${rupees(m.addon_price)}`) : h('span', { class: 'chip muted' }, 'Bundled'))))),
    ]));
}

async function moduleEditor(ctx, cat, businessId) {
  const b = await api.business(businessId);
  const chosen = new Set(b.entitlements);
  const bundled = new Set(cat.plans.find((p) => p.code === b.plan_code)?.modules || []);
  const wrap = h('div', {});
  const options = cat.modules.map((m) => ({
    value: m.code,
    label: `${m.name}${bundled.has(m.code) ? '  ✓ in plan' : ''}`,
    hint: bundled.has(m.code) ? m.category : `${m.category}${m.addon_price ? ` · +${rupees(m.addon_price)}` : ''}`,
  }));
  const list = checkboxList('modules', options, [...chosen]);
  const save = h('button', { class: 'btn primary' }, 'Save modules');
  save.addEventListener('click', async () => {
    save.disabled = true;
    try {
      await api.setModules(b.id, checkedValues(list, 'modules'));
      ctx.toast('Modules updated', 'ok');
      ctx.navigate('business');
    } catch (err) { ctx.toast(err.message, 'error'); } finally { save.disabled = false; }
  });
  wrap.append(
    h('div', { class: 'card' },
      h('h3', {}, `Modules for ${b.legalname}`),
      h('p', { class: 'muted small' }, `Plan ${b.plan_code} · ${b.business_code}`),
      list,
      h('div', { class: 'actions' }, save)));
  return wrap;
}

// ---------------------------------------------------------------- issue licence
async function issue(ctx) {
  const cat = ctx.state.catalogue || await api.catalogue();
  const bizList = await api.businesses({});
  if (!bizList.rows.length) {
    return h('div', { class: 'card' }, h('h3', {}, 'No businesses yet'),
      h('p', { class: 'muted' }, 'Onboard a business first.'),
      h('button', { class: 'btn primary', onclick: () => ctx.navigate('onboard') }, '＋ Onboard a business'));
  }
  let businessId = ctx.state.businessId || bizList.rows[0].id;
  const fp = fingerprintForm();

  const form = h('div', { class: 'form' },
    h('div', { class: 'row' },
      h('label', { class: 'field' }, 'Business',
        sel(bizList.rows.map((b) => String(b.id)), String(businessId), (v) => { businessId = Number(v); refreshEntitlements(); })),
      h('label', { class: 'field' }, 'Term (months, optional)',
        h('input', { name: 'months', placeholder: 'default from plan', inputmode: 'numeric' }))),
    h('div', { class: 'section-title' }, 'Machine fingerprint'),
    h('p', { class: 'muted small' }, 'Paste the machine-fingerprint.json produced by the installer, or type the values. The licence binds to this machine only.'),
    fp.node,
    h('div', { class: 'actions' }, h('button', { class: 'btn primary', onclick: submit }, 'Issue licence')));

  const ent = h('div', { class: 'chips' });
  const refreshEntitlements = async () => {
    const b = await api.business(businessId);
    ent.replaceChildren(...b.entitlements.map((c) => h('span', { class: 'chip' }, c)));
  };
  void refreshEntitlements();

  async function submit() {
    const machine = fp.get();
    if (!machine.mac && !machine.cpu && !machine.disk) { ctx.toast('Enter at least a MAC, CPU or disk id', 'error'); return; }
    const months = Number(form.querySelector('input[name=months]').value) || undefined;
    try {
      const lic = await api.issueLicense({ business_id: businessId, machine, months });
      ctx.toast(`Issued ${lic.license_id}`, 'ok');
      ctx.openModal('Licence issued', licenceView(ctx, lic));
    } catch (err) { ctx.toast(err.message, 'error'); }
  }

  return h('div', {},
    card('Issue a machine-bound licence', form),
    h('div', { class: 'section-title' }, 'Entitled modules for this business'),
    ent);
}

function fingerprintForm(initial = {}) {
  const vals = { mac: '', cpu: '', disk: '', hostname: '', os: '', ...initial };
  const field = (label, key, ph) => h('label', { class: 'field' }, label,
    h('input', { value: vals[key] || '', placeholder: ph || '', oninput: (e) => { vals[key] = e.target.value; } }));
  const jsonBox = h('textarea', {
    rows: 3, placeholder: '{ "mac": "AA:BB:..", "cpu": "...", "disk": "...", "hostname": "..." }',
    oninput: (e) => {
      try {
        const o = JSON.parse(e.target.value);
        Object.assign(vals, o);
        for (const k of ['mac', 'cpu', 'disk', 'hostname', 'os']) {
          const el = node.querySelector(`input[data-fp="${k}"]`);
          if (el && o[k] != null) el.value = o[k];
        }
        e.target.classList.remove('error-border');
      } catch { if (e.target.value.trim()) e.target.classList.add('error-border'); }
    },
  });
  const node = h('div', {},
    h('div', { class: 'row' }, field('MAC address', 'mac', '00:1A:2B:3C:4D:5E'), field('CPU id', 'cpu', 'BFEBFBFF000306A9')),
    h('div', { class: 'row' }, field('System disk serial', 'disk', 'WD-1234ABCD'), field('Hostname', 'hostname', 'counter-pc')),
    h('div', { class: 'row' }, field('OS', 'os', 'Windows 11')),
    h('label', { class: 'field' }, 'Paste machine-fingerprint.json', jsonBox));
  // tag the inputs so the JSON paste can fill them
  ['mac', 'cpu', 'disk', 'hostname', 'os'].forEach((k, i) => {
    node.querySelectorAll('input')[i].dataset.fp = k;
  });
  return { node, get: () => ({ mac: vals.mac, cpu: vals.cpu, disk: vals.disk, hostname: vals.hostname, os: vals.os }) };
}

function licenceView(ctx, lic) {
  const seatsUsed = lic.activations.filter((a) => !a.released_at).length;
  const stateLine = {
    valid: 'Valid', grace: `In grace — ${lic.grace_remaining} day(s) left before expiry`,
    expired: 'Expired', revoked: 'Revoked',
  }[lic.state] || lic.state;

  const surrender = h('button', { class: 'btn' }, '↺ Surrender');
  surrender.addEventListener('click', async () => {
    if (!window.confirm(`Surrender ${lic.license_id}? Every machine binding is released so the licence can be activated on a new machine.`)) return;
    surrender.disabled = true;
    try {
      await api.surrender(lic.license_id, 'surrender from portal');
      ctx.toast('Licence surrendered — ready to re-activate on a new machine', 'ok');
      ctx.closeModal?.();
      ctx.navigate('licenses');
    } catch (err) { ctx.toast(err.message, 'error'); surrender.disabled = false; }
  });

  return h('div', {},
    h('div', { class: 'stat-list' },
      row('Licence', lic.license_id), row('Business', lic.business_code),
      row('Plan', lic.plan_code),
      h('div', { class: 'kv' }, h('span', { class: 'k' }, 'State'), statusBadge(lic.state)),
      row('State detail', stateLine),
      row('Valid', `${date(lic.valid_from)} → ${date(lic.valid_until)}`),
      row('Grace after expiry', `${lic.grace_days} days`),
      row('Offline limit', `${lic.offline_limit_days} days (then educational mode)`),
      row('Seats', `${seatsUsed} of ${lic.seat_limit} in use`),
      row('Bound machine', lic.bound_machine_hash ? `${lic.bound_machine_hash.slice(0, 16)}…` : 'unbound (surrendered)'),
      row('Machine hash', lic.machine_hash)),
    h('div', { class: 'section-title' }, 'Entitlements'),
    h('div', { class: 'chips' }, ...lic.entitlements.map((c) => h('span', { class: 'chip' }, c))),
    h('div', { class: 'section-title' }, 'Activations'),
    activationsTable(lic.activations),
    h('div', { class: 'actions' },
      h('a', { class: 'btn primary', href: api.licenseFileUrl(lic.license_id), download: `${lic.license_id}.vlic` }, '⭳ Download licence file'),
      h('button', { class: 'btn', onclick: () => { downloadJson(`${lic.license_id}.vlic`, lic.payload); } }, 'Download payload only'),
      surrender));
}

function activationsTable(rows) {
  if (!rows.length) return h('div', { class: 'empty' }, 'Not yet activated.');
  return table([
    { title: 'Machine', value: (r) => `${r.machine_hash.slice(0, 16)}…` },
    { title: 'Activated', value: (r) => r.activated_at },
    { title: 'Released', value: (r) => (r.released_at ? r.released_at : '—') },
    { title: 'IP', value: (r) => r.ip || '—' },
  ], rows);
}

// ---------------------------------------------------------------- verify / activate
async function verify(ctx) {
  const fp = fingerprintForm();
  const resultBox = h('div', {});
  const idInput = h('input', { placeholder: 'VL-LIC-XXXXXX' });
  const offlineInput = h('input', { name: 'offline', placeholder: '0', inputmode: 'numeric' });

  async function run(action) {
    const id = idInput.value.trim();
    if (!id) { ctx.toast('Enter a licence id', 'error'); return; }
    const days = Number(offlineInput.value) || 0;
    resultBox.replaceChildren(loading());
    try {
      const r = action === 'activate'
        ? await api.activate(id, fp.get())
        : await api.verifyLicense(id, fp.get(), days);
      resultBox.replaceChildren(action === 'activate' ? activateView(r) : verifyView(r));
    } catch (err) { resultBox.replaceChildren(h('div', { class: 'card' }, h('p', { class: 'error' }, err.message))); }
  }

  return h('div', {},
    card('Activate or verify a licence', h('div', { class: 'form' },
      h('div', { class: 'row' },
        h('label', { class: 'field' }, 'Licence id', idInput),
        h('label', { class: 'field' }, 'Days since last check-in (offline test)', offlineInput)),
      h('div', { class: 'section-title' }, 'This machine'),
      fp.node,
      h('div', { class: 'actions' },
        h('button', { class: 'btn primary', onclick: () => run('activate') }, 'Activate on this machine'),
        h('button', { class: 'btn', onclick: () => run('verify') }, 'Verify only')))),
    h('div', { class: 'section-title' }, 'Result'),
    resultBox);
}

function verifyView(r) {
  if (!r.ok && r.reason === 'not-found') return h('div', { class: 'card' }, h('p', { class: 'error' }, 'Licence not found.'));
  const verdictClass = r.severity === 'hard' ? 'bad' : (r.severity === 'warn' ? 'warn' : 'ok');
  const verdictText = r.ok
    ? (r.state === 'grace' ? '⚠ Licence in grace period' : (r.offline_state === 'educational' ? '⚠ Limited (educational) mode' : '✓ Licence valid'))
    : '✗ Licence not valid';
  return h('div', { class: 'card' },
    h('div', { class: 'verdict ' + verdictClass }, verdictText),
    h('div', { class: 'stat-list' },
      row('Licence', r.license_id), row('Business', r.business_code), row('Plan', r.plan),
      row('Status', r.status),
      h('div', { class: 'kv' }, h('span', { class: 'k' }, 'State'), statusBadge(r.state)),
      row('Signature', r.signature_ok ? 'valid' : 'INVALID'),
      row('Days remaining', r.days_remaining),
      row('Grace remaining', `${r.grace_remaining} of ${r.grace_days} days`),
      row('Offline limit', `${r.offline_limit_days} days`),
      h('div', { class: 'kv' }, h('span', { class: 'k' }, 'Offline state'), statusBadge(r.offline_state)),
      row('Seats', r.seats),
      row('Machine match', r.machine_ok == null ? 'not checked' : (r.machine_ok ? 'yes' : 'NO'))),
    h('div', { class: 'chips' }, ...r.entitlements.map((c) => h('span', { class: 'chip' }, c))));
}

function activateView(lic) {
  const seatsUsed = lic.activations.filter((a) => !a.released_at).length;
  return h('div', { class: 'card' },
    h('div', { class: 'verdict ok' }, '✓ Activated on this machine'),
    h('div', { class: 'stat-list' },
      row('Licence', lic.license_id), row('Business', lic.business_code), row('Status', lic.status),
      row('Valid until', date(lic.valid_until)),
      row('Seats', `${seatsUsed} of ${lic.seat_limit} in use`)));
}

// ---------------------------------------------------------------- download
async function download(ctx) {
  const d = await api.downloads();

  const installerLinks = h('div', { class: 'list-menu' },
    ...d.installers.map((it, i) => h('a', {
      class: i === 0 ? 'btn primary' : 'btn', href: it.url, download: it.filename,
    }, `⭳ ${it.label}`)));

  const serverUrl = d.license_server.url || location.origin;
  const serverLinks = h('div', { class: 'list-menu' },
    h('a', { class: 'btn', href: serverUrl, target: '_blank', rel: 'noopener' }, '↗ Open licence server'),
    h('a', { class: 'btn', href: d.license_server.endpoints.license_key, target: '_blank', rel: 'noopener' }, '↗ Licence signing key'),
    h('a', { class: 'btn', href: d.license_server.endpoints.health, target: '_blank', rel: 'noopener' }, '↗ Server health'));

  const steps = h('ol', { class: 'steps-list' }, ...d.steps.map((s) => h('li', {}, s)));

  const meta = h('div', { class: 'stat-list' },
    row('App version', d.app_version),
    row('Release', d.release.tag || 'latest'),
    row('Repository', d.release.repo),
    row('Licence server', serverUrl || '—'));

  return h('div', {},
    card('Download VanijLiquor for Windows', h('div', {},
      h('p', { class: 'muted' }, 'Download and run the installer on the counter PC. The app writes a machine-fingerprint.json you paste here to issue a licence.'),
      installerLinks,
      h('p', { class: 'muted small' }, `Installer links point at the ${d.release.tag ? `release ${d.release.tag}` : 'latest release'} of ${d.release.repo}.`))),
    card('Licence server', h('div', {},
      h('p', { class: 'muted' }, 'The desktop app activates and verifies licences against this portal. Share these links with the customer if they install on another PC.'),
      h('div', { class: 'kv' }, h('span', { class: 'k' }, 'Server URL'), h('span', { class: 'v' }, serverUrl || '—')),
      serverLinks)),
    card('Fingerprint helpers', h('div', {},
      h('p', { class: 'muted small' }, 'Optional — run on the counter PC if the app is not yet installed.'),
      h('div', { class: 'list-menu' },
        ...d.helpers.map((hp) => h('a', { class: 'btn', href: hp.url, download: hp.filename }, `⭳ ${hp.label}`))))),
    card('How onboarding works', steps),
    card('Package details', meta));
}

// ---------------------------------------------------------------- licence key
async function key(ctx) {
  const k = await api.licenseKey();
  return h('div', {},
    card('Licence signing key', h('div', {},
      h('p', { class: 'muted' }, 'The desktop app pins this public key and verifies licences offline. Keep the private seed (PORTAL_LICENSE_KEY) secret and on the server only.'),
      k.production
        ? h('span', { class: 'badge ok' }, 'production key in use')
        : h('span', { class: 'badge danger' }, 'development key — set PORTAL_LICENSE_KEY'),
      h('div', { class: 'section-title' }, 'Public key (SPKI, base64)'),
      h('pre', { class: 'code-block' }, k.public_key))));
}

// ---------------------------------------------------------------- audit
async function audit(ctx) {
  const rows = await api.audit(200);
  return auditTable(rows);
}

function auditTable(rows) {
  return table([
    { title: 'When', value: (r) => r.at },
    { title: 'Actor', key: 'actor' },
    { title: 'Action', key: 'action' },
    { title: 'Entity', key: 'entity' },
    { title: 'Detail', value: (r) => r.detail },
  ], rows);
}

function openLicence(ctx, id) {
  api.license(id).then((lic) => ctx.openModal(`Licence ${id}`, licenceView(ctx, lic)))
    .catch((err) => ctx.toast(err.message, 'error'));
}
