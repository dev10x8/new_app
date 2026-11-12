import { all, get, run, tx } from '../db/index.js';
import { money, now } from './helpers.js';
import { businessDate } from './clock.js';

/** Read company parameters. */
export function getParameters(db, company) {
  return get(db, 'SELECT * FROM companyparamtr WHERE companycode=?', [company]) || {};
}

/** Update company parameters (only known columns are written). */
export function updateParameters(db, company, values) {
  const cur = get(db, 'SELECT * FROM companyparamtr WHERE companycode=?', [company]);
  if (!cur) throw new Error('Company parameters not found');
  const allowed = Object.keys(cur).filter((k) => k !== 'companycode' && values[k] !== undefined);
  if (allowed.length === 0) return cur;
  run(db,
    `UPDATE companyparamtr SET ${allowed.map((c) => `[${c}] = ?`).join(', ')} WHERE companycode = ?`,
    [...allowed.map((c) => values[c]), company]);
  return getParameters(db, company);
}

/**
 * New Year: create a new financial year and carry forward opening balances.
 * Mirrors frm_yearcreation: opening ledger balances and opening stock are
 * copied from the closing position of the previous year.
 */
export function createNewYear(db, { company, yearcode, yearrange, stdate, enddate, fromYear }) {
  return tx(db, () => {
    run(db, 'INSERT INTO yearmst (yearcode, yearrange, stdate, enddate, companycode) VALUES (?,?,?,?,?)',
      [yearcode, yearrange || yearcode, stdate, enddate, company]);

    let carriedLedgers = 0;
    let carriedStock = 0;

    if (fromYear) {
      // ledger closing balances -> opening balances of the new year
      const balances = all(db,
        `SELECT l.ledcode,
                COALESCE(SUM(j.debit),0) - COALESCE(SUM(j.credit),0) AS balance
           FROM ledger l
           LEFT JOIN journaldetail j
             ON j.ledcode=l.ledcode AND j.companycode=l.companycode AND j.yearcode=?
          WHERE l.companycode=?
          GROUP BY l.ledcode`,
        [fromYear, company]);
      for (const b of balances) {
        if (money(b.balance) === 0) continue;
        run(db,
          `INSERT INTO ledger_opening (companycode, yearcode, ledcode, debit, credit, type, stdate)
           VALUES (?,?,?,?,?,?,?)`,
          [company, yearcode, b.ledcode, b.balance > 0 ? money(b.balance) : 0, b.balance < 0 ? money(-b.balance) : 0, 'OPENING', stdate]);
        carriedLedgers += 1;
      }

      // opening stock -> carried forward as opening stock of the new year
      const stock = all(db,
        `SELECT shopcode, itemcode, box, loose, qnty FROM openingstockmst WHERE companycode=? AND yearcode=?`,
        [company, fromYear]);
      for (const s of stock) {
        run(db,
          `INSERT OR IGNORE INTO openingstockmst (shopcode, companycode, itemcode, loose, box, qnty, yearcode, trndate)
           VALUES (?,?,?,?,?,?,?,?)`,
          [s.shopcode, company, s.itemcode, s.loose, s.box, s.qnty, yearcode, stdate]);
        carriedStock += 1;
      }
    }
    return { yearcode, carriedLedgers, carriedStock };
  });
}

export function listYears(db, company) {
  return all(db, 'SELECT * FROM yearmst WHERE companycode=? ORDER BY yearcode', [company]);
}

/** System admin / deployment info. */
export function systemInfo(db, company) {
  const version = get(db, 'SELECT version, updatedat FROM Version ORDER BY id DESC LIMIT 1');
  const params = get(db, 'SELECT * FROM companyparamtr WHERE companycode=?', [company]) || {};
  const company1 = get(db, 'SELECT * FROM companymast WHERE companycode=?', [company]) || {};
  const clockInfo = businessDate(db, company);
  return {
    version: version?.version || 'unknown',
    updatedat: version?.updatedat || null,
    deploymentMode: params.print_mode || 'POS',
    company: company1.companyname || '',
    posMode: params.actposmode === 'Y',
    batchSale: params.batchSale === 1,
    negativeSale: params.negativeSale === 1,
    smsOn: params.sms_on === 1,
    dataSync: params.backup_upload === 1,
    now: now(),
    serverDate: clockInfo.date,
    clockRolledBack: clockInfo.rolledBack,
    clockSkewSeconds: clockInfo.skewSeconds,
    lastSeen: clockInfo.lastseen,
  };
}

/** Excise audit: per-item excise mapping and closing stock for audit. */
export function exciseAudit(db, company, year) {
  return all(db,
    `SELECT e.itemcode, e.excisecode, e.excisename, e.ml, e.strength,
            i.itemname,
            COALESCE(o.qnty,0) AS opening
       FROM ExciseItemMaster e
       LEFT JOIN itemmst i ON i.companycode=e.companycode AND i.itemcode=e.itemcode
       LEFT JOIN openingstockmst o ON o.companycode=e.companycode AND o.yearcode=? AND o.itemcode=e.itemcode
      WHERE e.companycode=?
      ORDER BY e.itemcode`,
    [year, company]);
}
