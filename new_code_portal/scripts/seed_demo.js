#!/usr/bin/env node
// Seed a handful of demo businesses + licences so the portal has something to
// show. Idempotent-ish: it resets the DB first unless PORTAL_KEEP=1.
//
//   node scripts/seed_demo.js
//   PORTAL_KEEP=1 node scripts/seed_demo.js   # keep existing rows
import { existsSync, rmSync } from 'node:fs';
import { dirname, resolve } from 'node:path';
import { fileURLToPath } from 'node:url';
import { config } from '../backend/src/config.js';
import { openDatabase } from '../backend/src/db/index.js';
import * as onboarding from '../backend/src/services/onboarding.js';
import * as licensing from '../backend/src/services/licensing.js';

const here = dirname(fileURLToPath(import.meta.url));
if (!process.env.PORTAL_KEEP) {
  for (const suffix of ['', '-wal', '-shm']) {
    const f = resolve(here, '..', 'data', 'portal.db') + suffix;
    if (existsSync(f)) rmSync(f);
  }
}

const db = openDatabase(config.dbFile);

const demo = [
  { legalname: 'Shree Wines & Co', tradename: 'Shree Wines', ownername: 'Ravi Kulkarni', email: 'ravi@shree.example', phone: '9876543210', gstin: '27ABCDE1234F1Z5', city: 'Pune', state: 'Maharashtra', statecode: '27', businesstype: 'retail', plan_code: 'premium', period_code: 'threeYears', modules: ['PARCHA', 'EXCISE', 'TALLY'], machine: { mac: '00:1A:2B:3C:4D:5E', cpu: 'BFEBFBFF000306A9', disk: 'WD-1234ABCD', hostname: 'counter-01' } },
  { legalname: 'Madhushala Bar & Restaurant', tradename: 'Madhushala', ownername: 'S. Nair', email: 'owner@madhushala.example', phone: '9820011223', gstin: '29XYZAB6789K1Z2', city: 'Bengaluru', state: 'Karnataka', statecode: '29', businesstype: 'bar', plan_code: 'standard', period_code: 'oneYear', modules: ['WHATSAPP_SUPPORT'], machine: { mac: '3C:5A:B4:11:22:33', cpu: 'BFEBFBFF000806EC', disk: 'SSD-9F8E7D', hostname: 'bar-pos' } },
  { legalname: 'Ganesh Liquor Mart', tradename: 'Ganesh Liquor', ownername: 'P. Das', email: 'ganesh@mart.example', phone: '9830012345', gstin: '19PQRSX1234L1Z9', city: 'Kolkata', state: 'West Bengal', statecode: '19', businesstype: 'wholesale', plan_code: 'basic', period_code: 'monthly', modules: [], machine: { mac: 'B8:27:EB:00:11:22', cpu: 'BFEBFBFF000506E3', disk: 'NVME-7788AA', hostname: 'ganesh-pc' } },
];

for (const d of demo) {
  const { modules = [], machine, ...biz } = d;
  const b = onboarding.create(db, biz, 'seed');
  if (modules.length) onboarding.setModules(db, b.id, modules, 'seed');
  const lic = licensing.issue(db, { business_id: b.id, machine }, 'seed');
  licensing.activate(db, lic.license_id, machine, { ip: '127.0.0.1' }, 'seed');
  console.log(`seeded ${b.business_code}  ${lic.license_id}  (${biz.plan_code})`);
}

// A fourth business that illustrates the grace window: its term ended a few days
// ago, so it still runs but reports "grace" with a renewal nudge.
const graceBiz = onboarding.create(db, {
  legalname: 'Krishna Wines', tradename: 'Krishna', ownername: 'M. Iyer', email: 'krishna@wines.example',
  city: 'Nashik', state: 'Maharashtra', statecode: '27', businesstype: 'retail',
  plan_code: 'standard', period_code: 'monthly',
}, 'seed');
const graceMachine = { mac: 'D4:BE:D9:77:88:99', cpu: 'BFEBFBFF000306C3', disk: 'SSD-GRACE01', hostname: 'krishna-pc' };
const graceLic = licensing.issue(db, { business_id: graceBiz.id, machine: graceMachine, months: 1 }, 'seed');
licensing.activate(db, graceLic.license_id, graceMachine, { ip: '127.0.0.1' }, 'seed');
// A monthly term that ended 3 days ago: backdate both ends so the range reads
// sensibly (a licence issued ~1 month ago, now just past its term).
db.prepare("UPDATE licenses SET valid_from=date('now','-33 day'), valid_until=date('now','-3 day') WHERE license_id=?").run(graceLic.license_id);
console.log(`seeded ${graceBiz.business_code}  ${graceLic.license_id}  (grace window demo)`);

// A fifth business on a 3-seat licence with two machines bound, to show seats.
const multiBiz = onboarding.create(db, {
  legalname: 'Sunrise Wine Chain', tradename: 'Sunrise', ownername: 'A. Bose', email: 'sunrise@chain.example',
  city: 'Nagpur', state: 'Maharashtra', statecode: '27', businesstype: 'wholesale',
  plan_code: 'premium', period_code: 'oneYear',
}, 'seed');
const seatA = { mac: '00:11:22:33:44:55', cpu: 'BFEBFBFF000806EA', disk: 'NVME-SEAT01', hostname: 'sunrise-hq' };
const seatB = { mac: '66:77:88:99:AA:BB', cpu: 'BFEBFBFF000806EA', disk: 'NVME-SEAT02', hostname: 'sunrise-branch' };
const multiLic = licensing.issue(db, { business_id: multiBiz.id, machine: seatA, seat_limit: 3 }, 'seed');
licensing.activate(db, multiLic.license_id, seatA, { ip: '127.0.0.1' }, 'seed');
licensing.activate(db, multiLic.license_id, seatB, { ip: '127.0.0.1' }, 'seed');
console.log(`seeded ${multiBiz.business_code}  ${multiLic.license_id}  (3 seats, 2 bound)`);

const stats = onboarding.stats(db);
console.log(`\nDone: ${stats.total} businesses, ${stats.licenses} licences, MRR ₹${stats.mrr}`);
db.close();
