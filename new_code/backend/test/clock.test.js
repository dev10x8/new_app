import { test } from 'node:test';
import assert from 'node:assert/strict';
import { mkdtempSync, rmSync } from 'node:fs';
import { tmpdir } from 'node:os';
import { join } from 'node:path';
import { openDatabase } from '../src/db/index.js';
import * as clock from '../src/services/clock.js';
import * as sales from '../src/services/sales.js';
import * as purchase from '../src/services/purchase.js';
import * as vouchers from '../src/services/vouchers.js';
import * as transactions from '../src/services/transactions.js';

const COMPANY = '1';
const YEAR = '2024';

function freshDb() {
  const dir = mkdtempSync(join(tmpdir(), 'vanijliquor-clock-'));
  const file = join(dir, 'test.db');
  const db = openDatabase(file);
  return { db, file, dir };
}

/** Run `fn` with the process clock pinned to `iso`, then restore it. */
function withClock(iso, fn) {
  const RealDate = Date;
  class FakeDate extends RealDate {
    constructor(...a) { super(...(a.length ? a : [iso])); }
    static now() { return RealDate.parse(iso); }
  }
  globalThis.Date = FakeDate;
  try { return fn(); } finally { globalThis.Date = RealDate; }
}

test('businessDate reports the system date and advances the high-water mark', () => {
  const { db, dir } = freshDb();
  try {
    const first = withClock('2026-05-01T09:00:00Z', () => clock.businessDate(db, COMPANY));
    assert.equal(first.date, '2026-05-01');
    assert.equal(first.rolledBack, false);
    assert.equal(first.lastseen, null);

    // A later day advances the mark and is not a rollback.
    const second = withClock('2026-05-03T09:00:00Z', () => clock.businessDate(db, COMPANY));
    assert.equal(second.date, '2026-05-03');
    assert.equal(second.rolledBack, false);
    assert.equal(second.lastseen.slice(0, 10), '2026-05-01');
  } finally {
    db.close(); rmSync(dir, { recursive: true, force: true });
  }
});

test('a clock rolled back to a past date is detected and does not move the business date back', () => {
  const { db, dir } = freshDb();
  try {
    withClock('2026-05-10T12:00:00Z', () => clock.businessDate(db, COMPANY));
    const rolled = withClock('2023-06-15T10:30:00Z', () => clock.businessDate(db, COMPANY));
    assert.equal(rolled.rolledBack, true, 'a clock behind the mark must be flagged');
    assert.equal(rolled.date, '2026-05-10', 'business date must not fall back to the rolled-back date');
    assert.ok(rolled.skewSeconds > 0);

    // The rollback is recorded (tamper-evident).
    const audit = db.prepare("SELECT * FROM TBL_Audit WHERE action='CLOCK_ROLLBACK'").all();
    assert.equal(audit.length, 1);
  } finally {
    db.close(); rmSync(dir, { recursive: true, force: true });
  }
});

test('small clock drift within tolerance is not treated as a rollback', () => {
  const { db, dir } = freshDb();
  try {
    withClock('2026-05-10T12:00:00Z', () => clock.businessDate(db, COMPANY));
    const drift = withClock('2026-05-10T11:59:30Z', () => clock.businessDate(db, COMPANY));
    assert.equal(drift.rolledBack, false);
  } finally {
    db.close(); rmSync(dir, { recursive: true, force: true });
  }
});

test('a missing transaction date defaults to the server date, not the client value', () => {
  const { db, dir } = freshDb();
  try {
    withClock('2026-05-10T12:00:00Z', () => {
      const r = sales.createSale(db, {
        company: COMPANY, year: YEAR, storecode: 'MAIN', salestype: 'FL', ledgercode: 'C00001',
        lines: [{ itemcode: 'I00001', qnty: 1, rate: 250 }],
      });
      assert.equal(r.date, '2026-05-10');
      const row = db.prepare('SELECT trndate FROM salesbillmain ORDER BY rowid DESC LIMIT 1').get();
      assert.equal(row.trndate, '2026-05-10');
    });
  } finally {
    db.close(); rmSync(dir, { recursive: true, force: true });
  }
});

test('a future-dated sale is rejected', () => {
  const { db, dir } = freshDb();
  try {
    withClock('2026-05-10T12:00:00Z', () => {
      assert.throws(() => sales.createSale(db, {
        company: COMPANY, year: YEAR, storecode: 'MAIN', salestype: 'FL', ledgercode: 'C00001',
        trndate: '2026-05-20',
        lines: [{ itemcode: 'I00001', qnty: 1, rate: 250 }],
      }), (err) => err.code === 'FUTURE_DATE' && err.status === 422);
    });
  } finally {
    db.close(); rmSync(dir, { recursive: true, force: true });
  }
});

test('posting into a closed day is rejected', () => {
  const { db, dir } = freshDb();
  try {
    withClock('2026-05-10T12:00:00Z', () => {
      sales.createSale(db, {
        company: COMPANY, year: YEAR, storecode: 'MAIN', salestype: 'FL', ledgercode: 'C00001',
        trndate: '2026-05-08',
        lines: [{ itemcode: 'I00001', qnty: 1, rate: 250 }],
      });
      sales.dayClose(db, COMPANY, YEAR, { from: '2026-05-01', to: '2026-05-09' });
      assert.throws(() => sales.createSale(db, {
        company: COMPANY, year: YEAR, storecode: 'MAIN', salestype: 'FL', ledgercode: 'C00001',
        trndate: '2026-05-08',
        lines: [{ itemcode: 'I00001', qnty: 1, rate: 250 }],
      }), (err) => err.code === 'PERIOD_CLOSED');
    });
  } finally {
    db.close(); rmSync(dir, { recursive: true, force: true });
  }
});

test('back-dating beyond tolerance is allowed but flagged', () => {
  const { db, dir } = freshDb();
  try {
    withClock('2026-05-10T12:00:00Z', () => {
      const r = sales.createSale(db, {
        company: COMPANY, year: YEAR, storecode: 'MAIN', salestype: 'FL', ledgercode: 'C00001',
        trndate: '2026-05-01',
        lines: [{ itemcode: 'I00001', qnty: 1, rate: 250 }],
      });
      assert.equal(r.date, '2026-05-01');
      assert.match(r.date_warning, /back-dated by 9 day/);
    });
  } finally {
    db.close(); rmSync(dir, { recursive: true, force: true });
  }
});

test('an invalid transaction date is rejected', () => {
  const { db, dir } = freshDb();
  try {
    assert.throws(() => clock.assertPostableDate(db, { company: COMPANY, year: YEAR, date: '10-05-2026' }),
      (err) => err.code === 'INVALID_DATE');
  } finally {
    db.close(); rmSync(dir, { recursive: true, force: true });
  }
});

test('vouchers reject a future date too', () => {
  const { db, dir } = freshDb();
  try {
    withClock('2026-05-10T12:00:00Z', () => {
      assert.throws(() => vouchers.createJournal(db, {
        company: COMPANY, year: YEAR, vchdate: '2026-06-01',
        lines: [{ ledcode: 'L00001', debit: 100 }, { ledcode: 'L00003', credit: 100 }],
      }), (err) => err.code === 'FUTURE_DATE');
    });
  } finally {
    db.close(); rmSync(dir, { recursive: true, force: true });
  }
});

test('purchase defaults a missing date to the server date and rejects a future one', () => {
  const { db, dir } = freshDb();
  try {
    withClock('2026-05-10T12:00:00Z', () => {
      purchase.createPurchase(db, {
        company: COMPANY, year: YEAR, ptype: 'PURCHASE',
        suppliercode: 'S00001', purchaseacccode: 'L00004', shopcode: 'MAIN', docno: 'INV-1',
        lines: [{ itemcode: 'I00001', batchno: 'B1', itemrate: 200, itemquantity: 10, itemmrp: 250 }],
      });
      const row = db.prepare('SELECT trndate FROM purchasemain ORDER BY rowid DESC LIMIT 1').get();
      assert.equal(row.trndate, '2026-05-10');

      assert.throws(() => purchase.createPurchase(db, {
        company: COMPANY, year: YEAR, trndate: '2026-06-01', ptype: 'PURCHASE',
        suppliercode: 'S00001', purchaseacccode: 'L00004', shopcode: 'MAIN', docno: 'INV-2',
        lines: [{ itemcode: 'I00001', batchno: 'B1', itemrate: 200, itemquantity: 5, itemmrp: 250 }],
      }), (err) => err.code === 'FUTURE_DATE');
    });
  } finally {
    db.close(); rmSync(dir, { recursive: true, force: true });
  }
});

test('stock movements and notes inherit the server date', () => {
  const { db, dir } = freshDb();
  try {
    withClock('2026-05-10T12:00:00Z', () => {
      transactions.createNote(db, {
        company: COMPANY, year: YEAR, type: 'DEBIT', partycode: 'S00001', headaccount: 'L00004',
        amount: 500, lines: [{ ledcode: 'L00004', amount: 500 }],
      });
      const note = db.prepare('SELECT trndate FROM notes_main ORDER BY rowid DESC LIMIT 1').get();
      assert.equal(note.trndate, '2026-05-10');

      assert.throws(() => transactions.createTransferSale(db, {
        company: COMPANY, year: YEAR, trndate: '2026-06-01', from: 'MAIN', to: 'CNT1',
        lines: [{ itemcode: 'I00001', qnty: 1, rate: 100 }],
      }), (err) => err.code === 'FUTURE_DATE');
    });
  } finally {
    db.close(); rmSync(dir, { recursive: true, force: true });
  }
});

test('a sustained rollback is recorded once, not on every read', () => {
  const { db, dir } = freshDb();
  try {
    withClock('2026-05-10T12:00:00Z', () => clock.businessDate(db, COMPANY));
    withClock('2023-06-15T10:30:00Z', () => {
      clock.businessDate(db, COMPANY);
      clock.businessDate(db, COMPANY);
      clock.businessDate(db, COMPANY);
    });
    const audit = db.prepare("SELECT * FROM TBL_Audit WHERE action='CLOCK_ROLLBACK'").all();
    assert.equal(audit.length, 1, 'repeated reads during one rollback must not spam the audit log');
  } finally {
    db.close(); rmSync(dir, { recursive: true, force: true });
  }
});
