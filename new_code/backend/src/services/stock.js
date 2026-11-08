import { all, get, run } from '../db/index.js';
import { money } from './helpers.js';

/**
 * Current stock for an item in a store, computed from the movement tables:
 *   opening + purchases - sales + sales-returns - transfers-out + transfers-in - breakage
 * This replaces the legacy SP_SetStockAll materialisation.
 */
export function stockOnHand(db, { company, year, storecode = null, itemcode = null }) {
  const params = [company, year];
  let storeFilter = '';
  let itemFilter = '';
  if (storecode) { storeFilter = ' AND storecode = ?'; params.push(storecode); }
  if (itemcode) { itemFilter = ' AND itemcode = ?'; params.push(itemcode); }

  const sql = `
    WITH movements AS (
      SELECT companycode, yearcode, shopcode AS storecode, itemcode,
             CAST(COALESCE(qnty,0) AS INTEGER) AS qty
        FROM openingstockmst
       WHERE companycode = ? AND yearcode = ?

      UNION ALL
      SELECT pm.companycode, pm.yearcode, pm.shopcode, pd.itemcode, CAST(COALESCE(pd.itemquantity,0) AS INTEGER)
        FROM purchasemain pm
        JOIN purchasedetail pd
          ON pd.companycode = pm.companycode AND pd.yearcode = pm.yearcode AND pd.trnno = pm.trnno
       WHERE pm.companycode = ? AND pm.yearcode = ? AND pm.ptype = 'PURCHASE'

      UNION ALL
      SELECT pm.companycode, pm.yearcode, pm.shopcode, pd.itemcode, -CAST(COALESCE(pd.itemquantity,0) AS INTEGER)
        FROM purchasemain pm
        JOIN purchasedetail pd
          ON pd.companycode = pm.companycode AND pd.yearcode = pm.yearcode AND pd.trnno = pm.trnno
       WHERE pm.companycode = ? AND pm.yearcode = ? AND pm.ptype = 'PURCHASE RETURN'

      UNION ALL
      SELECT sm.companycode, sm.yearcode, sm.storecode, sd.itemcode, -CAST(COALESCE(sd.qnty,0) AS INTEGER)
        FROM salesbillmain sm
        JOIN salesbilldetail sd
          ON sd.companycode = sm.companycode AND sd.yearcode = sm.yearcode AND sd.trnno = sm.trnno
       WHERE sm.companycode = ? AND sm.yearcode = ? AND (sm.salestype IS NULL OR sm.salestype <> 'RETURN')

      UNION ALL
      SELECT sm.companycode, sm.yearcode, sm.storecode, sd.itemcode, CAST(COALESCE(sd.qnty,0) AS INTEGER)
        FROM salesbillmain sm
        JOIN salesbilldetail sd
          ON sd.companycode = sm.companycode AND sd.yearcode = sm.yearcode AND sd.trnno = sm.trnno
       WHERE sm.companycode = ? AND sm.yearcode = ? AND sm.salestype = 'RETURN'

      UNION ALL
      SELECT tm.CompanyCode, tm.yearcode, tm.shopcode_frm, td.ItemCode, -CAST(COALESCE(td.qnty,0) AS INTEGER)
        FROM stk_transfer_main tm
        JOIN stk_transfer_detail td
          ON td.CompanyCode = tm.CompanyCode AND td.yearcode = tm.yearcode AND td.trnno = tm.TrnNo
       WHERE tm.CompanyCode = ? AND tm.yearcode = ?

      UNION ALL
      SELECT tm.CompanyCode, tm.yearcode, tm.shopcode_to, td.ItemCode, CAST(COALESCE(td.qnty,0) AS INTEGER)
        FROM stk_transfer_main tm
        JOIN stk_transfer_detail td
          ON td.CompanyCode = tm.CompanyCode AND td.yearcode = tm.yearcode AND td.trnno = tm.TrnNo
       WHERE tm.CompanyCode = ? AND tm.yearcode = ?

      UNION ALL
      SELECT bm.CompanyCode, bm.YearCode, bm.shopcode, bd.ItemCode, -CAST(COALESCE(bd.quantity,0) AS INTEGER)
        FROM breakagemain bm
        JOIN breakagedetail bd
          ON bd.CompanyCode = bm.CompanyCode AND bd.yearcode = bm.YearCode AND bd.trnno = bm.TrnNo
       WHERE bm.CompanyCode = ? AND bm.YearCode = ?
    )
    SELECT m.storecode, m.itemcode, i.itemname, i.ml, i.packing,
           SUM(m.qty) AS qty,
           SUM(m.qty) * COALESCE(i.salesrate, 0) AS value
      FROM movements m
      LEFT JOIN itemmst i ON i.companycode = ? AND i.itemcode = m.itemcode
     WHERE 1=1 ${storeFilter} ${itemFilter}
     GROUP BY m.storecode, m.itemcode, i.itemname, i.ml, i.packing
     ORDER BY m.storecode, m.itemcode`;

  // The CTE references (company, year) eight times; build the param list in order.
  const cteParams = [];
  for (let i = 0; i < 8; i++) cteParams.push(company, year);
  const finalParams = [...cteParams, company, ...params.slice(2)];
  return all(db, sql, finalParams);
}

/** Opening stock entry (per store / batch / item). */
export function addOpeningStock(db, o) {
  run(
    db,
    `INSERT INTO openingstockmst (shopcode, companycode, itemcode, loose, box, qnty, yearcode, trndate)
     VALUES (?,?,?,?,?,?,?,?)
     ON CONFLICT(companycode, yearcode, shopcode, itemcode)
     DO UPDATE SET loose=excluded.loose, box=excluded.box, qnty=excluded.qnty, trndate=excluded.trndate`,
    [o.shopcode, o.company, o.itemcode, o.loose || 0, o.box || 0, o.qnty || 0, o.year, o.trndate],
  );
  return { ok: true };
}

/** Stock transfer between stores. */
export function stockTransfer(db, t) {
  run(
    db,
    `INSERT INTO stk_transfer_main (CompanyCode, YearCode, TrnNo, TrnDate, shopname_from, shopname_to, shopcode_frm, shopcode_to, trnid)
     VALUES (?,?,?,?,?,?,?,?,?)`,
    [t.company, t.year, t.trnno, t.trndate, t.fromName || null, t.toName || null, t.from, t.to, t.trnid || 0],
  );
  for (const line of t.lines) {
    run(
      db,
      `INSERT INTO stk_transfer_detail (CompanyCode, ItemCode, box, loose, qnty, trnno, trndate, yearcode, batchDate, batchNo)
       VALUES (?,?,?,?,?,?,?,?,?,?)`,
      [t.company, line.itemcode, line.box || 0, line.loose || 0, line.qnty || 0, t.trnno, t.trndate, t.year,
       line.batchDate || t.trndate, line.batchNo || '0'],
    );
  }
  return { trnno: t.trnno };
}

/** Breakage entry. */
export function addBreakage(db, b) {
  run(
    db,
    `INSERT INTO breakagemain (CompanyCode, YearCode, TrnNo, Narration, shopcode, trndate, ledgercode, billno, TP_pass_no, trnid)
     VALUES (?,?,?,?,?,?,?,?,?,?)`,
    [b.company, b.year, b.trnno, b.narration || null, b.shopcode, b.trndate, b.ledgercode || null, b.billno || null, b.tppassno || null, b.trnid || 0],
  );
  for (const line of b.lines) {
    run(
      db,
      `INSERT INTO breakagedetail (CompanyCode, trnno, ItemCode, Receivd, yearcode, loose, box, quantity, trndate, batchDate, batchNo)
       VALUES (?,?,?,?,?,?,?,?,?,?,?)`,
      [b.company, b.trnno, line.itemcode, line.receivd || 'False', b.year, line.loose || 0, line.box || 0,
       line.quantity || 0, b.trndate, line.batchDate || b.trndate, line.batchNo || '0'],
    );
  }
  return { trnno: b.trnno };
}
