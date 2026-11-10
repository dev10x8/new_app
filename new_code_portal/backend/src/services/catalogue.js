import { all, get } from '../db/index.js';

/** Parse a JSON text column, tolerating NULL / bad data. */
const json = (s, fallback) => {
  if (s == null || s === '') return fallback;
  try { return JSON.parse(s); } catch { return fallback; }
};

/** Billing periods, cheapest commitment first. */
export function periods(db) {
  return all(db, 'SELECT code, label, months FROM billing_periods ORDER BY sort');
}

/** All plans with their per-period prices and bundled module codes. */
export function plans(db) {
  const rows = all(db, 'SELECT * FROM plans ORDER BY sort');
  return rows.map((p) => ({
    code: p.code,
    name: p.name,
    tagline: p.tagline,
    monthly: p.monthly,
    popular: !!p.popular,
    highlights: json(p.highlights, []),
    prices: priceMap(db, p.code),
    modules: includedModules(db, p.code),
  }));
}

function priceMap(db, planCode) {
  const out = {};
  for (const r of all(db, 'SELECT period_code, amount FROM plan_prices WHERE plan_code=?', [planCode])) {
    out[r.period_code] = r.amount;
  }
  return out;
}

function includedModules(db, planCode) {
  return all(db, 'SELECT module_code FROM plan_modules WHERE plan_code=? AND included=1 ORDER BY module_code', [planCode])
    .map((r) => r.module_code);
}

/** The full module catalogue. */
export function modules(db) {
  return all(db, 'SELECT code, name, category, description, addon_price FROM modules ORDER BY sort');
}

/** Add-on catalogue. */
export function addons(db) {
  return all(db, 'SELECT code, name, detail, price, period FROM addons ORDER BY sort');
}

/** Plan comparison matrix grouped by category. */
export function comparison(db) {
  const rows = all(db, 'SELECT category, feature, basic, standard, premium FROM comparison ORDER BY category, sort, id');
  const groups = [];
  const byCat = new Map();
  for (const r of rows) {
    if (!byCat.has(r.category)) { byCat.set(r.category, { category: r.category, features: [] }); groups.push(byCat.get(r.category)); }
    byCat.get(r.category).features.push({
      name: r.feature,
      basic: !!r.basic, standard: !!r.standard, premium: !!r.premium,
    });
  }
  return groups;
}

/** Everything the marketing/selection UI needs, in one payload. */
export function catalogue(db) {
  return {
    plans: plans(db),
    periods: periods(db),
    modules: modules(db),
    addons: addons(db),
    comparison: comparison(db),
  };
}

/** Plan price for a period, falling back to the monthly rate if unset. */
export function planPrice(db, planCode, periodCode) {
  const row = get(db, 'SELECT amount FROM plan_prices WHERE plan_code=? AND period_code=?', [planCode, periodCode]);
  if (row) return row.amount;
  const p = get(db, 'SELECT monthly FROM plans WHERE code=?', [planCode]);
  return p ? p.monthly : 0;
}
