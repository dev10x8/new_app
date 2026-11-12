import { get, run } from '../db/index.js';
import { config } from '../config.js';

const pad = (x) => String(x).padStart(2, '0');

/**
 * Date integrity. The process clock is read in exactly one place (here) so the
 * API, the UI and every stored date agree on what "now" is. Two protections sit
 * on top of the raw clock:
 *
 *   * a high-water mark (tbl_ClockGuard.lastseen) records the newest time the
 *     app has ever seen, so a system clock moved *backwards* is detected;
 *   * posting validates the transaction date against the server date, so a bill
 *     cannot land in the future or inside a day that has been closed.
 *
 * The aim is tamper-*evident*: on a machine the operator controls the clock can
 * still be changed, but the change is recorded and surfaced, not silent.
 */

/** Current system timestamp as 'YYYY-MM-DD HH:MM:SS' (local time). */
export function now() {
  const d = new Date();
  return `${d.getFullYear()}-${pad(d.getMonth() + 1)}-${pad(d.getDate())} `
    + `${pad(d.getHours())}:${pad(d.getMinutes())}:${pad(d.getSeconds())}`;
}

/** Today as 'YYYY-MM-DD'. */
export function today() {
  return now().slice(0, 10);
}

/** Whole days from `a` to `b` (b - a). Both are 'YYYY-MM-DD'. */
export function daysBetween(a, b) {
  return Math.round((Date.parse(`${b}T00:00:00`) - Date.parse(`${a}T00:00:00`)) / 86400000);
}

const toMs = (stamp) => Date.parse(String(stamp).replace(' ', 'T'));

/**
 * Newest closed date for a table, or null. Tolerates a schema where the close
 * column has not been created yet (it is added lazily on first day-close).
 */
function lastClosedDate(db, table, column, company, year) {
  const cols = db.prepare(`PRAGMA table_info([${table}])`).all();
  if (!cols.some((c) => c.name === column)) return null;
  const row = get(
    db,
    `SELECT MAX(${column}) AS last FROM [${table}]
      WHERE companycode = ? AND yearcode = ? AND ${column} IS NOT NULL AND ${column} <> ''`,
    [company, year],
  );
  return row?.last || null;
}

/**
 * Compare the current clock with the stored high-water mark and advance it.
 * When the clock is behind the mark by more than the tolerance it is treated as
 * rolled back, and the *effective* date falls back to the mark so a rolled-back
 * clock cannot make the app believe it is an earlier day. The rollback is
 * recorded in the audit log (tamper-evident).
 */
export function guardClock(db, company) {
  const current = now();
  const row = get(db, 'SELECT lastseen FROM tbl_ClockGuard WHERE companycode = ?', [company]);
  const lastseen = row?.lastseen || null;

  let skewSeconds = 0;
  let rolledBack = false;
  if (lastseen) {
    skewSeconds = Math.round((toMs(lastseen) - toMs(current)) / 1000);
    rolledBack = skewSeconds > config.clockSkewToleranceSeconds;
  }

  // Effective time never moves backwards: trust whichever is later.
  const effective = lastseen && rolledBack ? lastseen : current;

  if (!lastseen || effective > lastseen) {
    run(
      db,
      `INSERT INTO tbl_ClockGuard (companycode, lastseen, updatedat) VALUES (?,?,?)
       ON CONFLICT(companycode) DO UPDATE SET lastseen = excluded.lastseen, updatedat = excluded.updatedat`,
      [company, effective, current],
    );
  }

  if (rolledBack) {
    // Record once per distinct rollback (same skew and same rolled-back time),
    // so repeated reads while the clock stays wrong don't spam the audit log.
    const already = get(
      db,
      `SELECT id FROM TBL_Audit
        WHERE action = 'CLOCK_ROLLBACK' AND companycode = ? AND refkey = ? AND auditat = ?
        LIMIT 1`,
      [company, String(skewSeconds), current],
    );
    if (!already) {
      run(
        db,
        'INSERT INTO TBL_Audit (companycode, tablename, action, refkey, auditat) VALUES (?,?,?,?,?)',
        [company, 'tbl_ClockGuard', 'CLOCK_ROLLBACK', String(skewSeconds), current],
      );
    }
  }

  return {
    datetime: effective,
    date: effective.slice(0, 10),
    system_datetime: current,
    lastseen,
    rolledBack,
    skewSeconds,
  };
}

/** The authoritative business date plus the clock-guard verdict. */
export function businessDate(db, company) {
  return guardClock(db, company);
}

/**
 * Validate a transaction date before it is stored. Returns the resolved date
 * and any advisory flags; throws (status 422) for dates that must not be
 * posted.
 *
 *   * future  - later than the server date + futureToleranceDays -> rejected
 *   * closed  - inside a day already closed for the table (day-close lock) -> rejected
 *   * back-dated beyond backdateToleranceDays -> allowed but flagged
 *
 * A missing date resolves to the server date, so a bill never inherits a stale
 * value from a client with a wrong clock.
 */
export function assertPostableDate(db, { company, year, date, lockTable = null, lockColumn = 'dayclose' } = {}) {
  const guard = guardClock(db, company);
  const resolved = String(date || guard.date).slice(0, 10);

  if (!/^\d{4}-\d{2}-\d{2}$/.test(resolved)) {
    const err = new Error(`Invalid transaction date '${date}' (expected YYYY-MM-DD)`);
    err.status = 422;
    err.code = 'INVALID_DATE';
    throw err;
  }

  if (daysBetween(guard.date, resolved) > config.futureToleranceDays) {
    const err = new Error(`Transaction date ${resolved} is in the future (server date ${guard.date})`);
    err.status = 422;
    err.code = 'FUTURE_DATE';
    throw err;
  }

  if (lockTable) {
    const last = lastClosedDate(db, lockTable, lockColumn, company, year);
    if (last && resolved <= last) {
      const err = new Error(`Transaction date ${resolved} falls in a closed period (closed through ${last})`);
      err.status = 422;
      err.code = 'PERIOD_CLOSED';
      throw err;
    }
  }

  const backDays = Math.max(0, daysBetween(resolved, guard.date));
  return {
    date: resolved,
    server_date: guard.date,
    backdated: backDays > config.backdateToleranceDays,
    backdated_days: backDays,
    clock_rolled_back: guard.rolledBack,
    clock_skew_seconds: guard.skewSeconds,
  };
}
