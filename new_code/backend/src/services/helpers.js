import { get } from '../db/index.js';

/** Round money to 2 decimals. */
export function money(n) {
  return Math.round((Number(n) + Number.EPSILON) * 100) / 100;
}

/** Current timestamp as 'YYYY-MM-DD HH:MM:SS'. */
export function now() {
  const d = new Date();
  const p = (x) => String(x).padStart(2, '0');
  return `${d.getFullYear()}-${p(d.getMonth() + 1)}-${p(d.getDate())} ${p(d.getHours())}:${p(d.getMinutes())}:${p(d.getSeconds())}`;
}

/** Today as 'YYYY-MM-DD'. */
export function today() {
  return now().slice(0, 10);
}

/** Pad a code to width (codes are fixed-length varchar in the legacy DB). */
export function pad(code, width) {
  return String(code).padStart(width, '0');
}

/**
 * Next transaction number for an integer-keyed table within a company/year.
 * Mirrors the legacy trnidmst / per-document counter behaviour.
 */
export function nextTrnNo(db, table, company, year, column = 'trnno') {
  const row = get(
    db,
    `SELECT COALESCE(MAX(CAST([${column}] AS INTEGER)), 0) AS n
       FROM [${table}]
      WHERE companycode = ? AND yearcode = ?`,
    [company, year],
  );
  return (row?.n || 0) + 1;
}

/**
 * Next bill number for sales. Bills render as <prefix><n> where the prefix is
 * derived from the sale type (FL / CS / NAI), matching the legacy app.
 */
export function nextBillNo(db, company, year, salestype) {
  const prefix = billPrefix(salestype);
  const row = get(
    db,
    `SELECT trnno FROM salesbillmain
      WHERE companycode = ? AND yearcode = ? AND trnno LIKE ?
      ORDER BY CAST(substr(trnno, ?) AS INTEGER) DESC LIMIT 1`,
    [company, year, `${prefix}%`, prefix.length + 1],
  );
  const last = row ? Number(String(row.trnno).slice(prefix.length)) || 0 : 0;
  return `${prefix}${String(last + 1).padStart(5, '0')}`;
}

export function billPrefix(salestype) {
  const t = String(salestype || '').toUpperCase();
  if (t.includes('NAI')) return 'NAI';
  if (t.includes('CS') || t.includes('CASH') || t.includes('CREDIT')) return 'CS';
  return 'FL';
}

/** Next sequence value for a (company, year, trnid) document type. */
export function nextTrnId(db, company, year, trnType) {
  const row = get(
    db,
    'SELECT COALESCE(MAX(trnid), 0) AS n FROM trnidmst WHERE companycode = ? AND yearcode = ?',
    [company, year],
  );
  return (row?.n || 0) + 1;
}
