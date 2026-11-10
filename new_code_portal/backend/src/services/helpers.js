/** ISO-ish timestamp used across the portal (UTC, second precision). */
export function now() {
  return new Date().toISOString().slice(0, 19).replace('T', ' ');
}

/** Today's date (YYYY-MM-DD, UTC). */
export function today() {
  return new Date().toISOString().slice(0, 10);
}

/** Add months to a YYYY-MM-DD date, clamping the day to the month length. */
export function addMonths(dateStr, months) {
  const [y, m, d] = dateStr.split('-').map(Number);
  const base = new Date(Date.UTC(y, m - 1 + Number(months), 1));
  const lastDay = new Date(Date.UTC(base.getUTCFullYear(), base.getUTCMonth() + 1, 0)).getUTCDate();
  base.setUTCDate(Math.min(d, lastDay));
  return base.toISOString().slice(0, 10);
}

/** Days from a to b (b - a), as a whole number. */
export function daysBetween(a, b) {
  return Math.round((Date.parse(b) - Date.parse(a)) / 86400000);
}

/** URL-safe random id segment. */
export function token(n = 6) {
  const chars = 'ABCDEFGHJKLMNPQRSTUVWXYZ23456789';
  let out = '';
  for (let i = 0; i < n; i++) out += chars[Math.floor(Math.random() * chars.length)];
  return out;
}
