import { mkdtempSync } from 'node:fs';
import { tmpdir } from 'node:os';
import { join } from 'node:path';
import { openDatabase } from '../src/db/index.js';

/** Open a fresh, seeded portal database in a throwaway temp directory. */
export function freshDb() {
  const dir = mkdtempSync(join(tmpdir(), 'portal-test-'));
  return openDatabase(join(dir, 'portal.db'));
}

/** Representative machine fingerprints for tests (universally-administered MACs). */
export const machineA = { mac: '00:1A:2B:3C:4D:5E', cpu: 'BFEBFBFF000306A9', disk: 'WD-1234ABCD', hostname: 'counter-01' };
export const machineB = { mac: '3C:5A:B4:11:22:33', cpu: 'BFEBFBFF000306A9', disk: 'WD-1234ABCD', hostname: 'counter-02' };

/** Create a business quickly. */
export function makeBusiness(db, overrides = {}) {
  return import('../src/services/onboarding.js').then((m) => m.create(db, {
    legalname: 'Test Wines', email: 't@t.example', plan_code: 'standard', period_code: 'oneYear', ...overrides,
  }, 'test'));
}
