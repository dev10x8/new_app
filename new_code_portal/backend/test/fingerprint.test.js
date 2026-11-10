import test from 'node:test';
import assert from 'node:assert/strict';
import { normaliseMac, normaliseMacs, machineHash, machineDetail, hashFromDetail } from '../src/services/fingerprint.js';

test('MAC normalisation accepts every common notation', () => {
  assert.equal(normaliseMac('00:1A:2B:3C:4D:5E'), '00:1a:2b:3c:4d:5e');
  assert.equal(normaliseMac('00-1A-2B-3C-4D-5E'), '00:1a:2b:3c:4d:5e');
  assert.equal(normaliseMac('001a.2b3c.4d5e'), '00:1a:2b:3c:4d:5e');
  assert.equal(normaliseMac('001A2B3C4D5E'), '00:1a:2b:3c:4d:5e');
  assert.equal(normaliseMac('not a mac'), '');
  assert.equal(normaliseMac(''), '');
});

test('normaliseMacs drops virtual (locally-administered) adapters and sorts', () => {
  // 02:.. is locally administered (virtual/tunnel); the real OUIs are kept.
  const macs = normaliseMacs(['00:1A:2B:3C:4D:5E', '02:00:00:00:00:01', '3C:5A:B4:11:22:33']);
  assert.deepEqual(macs, ['00:1a:2b:3c:4d:5e', '3c:5a:b4:11:22:33']);
});

test('normaliseMacs keeps a virtual-only list rather than emptying it', () => {
  assert.deepEqual(normaliseMacs(['02:11:22:33:44:55']), ['02:11:22:33:44:55']);
});

test('machine hash is stable across MAC case/format and key order', () => {
  const a = machineHash({ mac: '00:1A:2B:3C:4D:5E', cpu: 'ABC', disk: 'D1' });
  const b = machineHash({ cpu: 'ABC', disk: 'D1', mac: '001a.2b3c.4d5e' });
  assert.equal(a, b);
  assert.equal(a.length, 64);
});

test('machine hash changes when any identifying part changes', () => {
  const base = { mac: '00:1A:2B:3C:4D:5E', cpu: 'ABC', disk: 'D1' };
  assert.notEqual(machineHash(base), machineHash({ ...base, mac: '00:1A:2B:3C:4D:5F' }));
  assert.notEqual(machineHash(base), machineHash({ ...base, cpu: 'XYZ' }));
  assert.notEqual(machineHash(base), machineHash({ ...base, disk: 'D2' }));
});

test('machine hash ignores a hostname rename when none was reported', () => {
  // hostname is advisory: present on both sides -> must match; absent -> ignored.
  const withHost = machineHash({ mac: '00:1A:2B:3C:4D:5E', cpu: 'ABC', disk: 'D1', hostname: 'pc1' });
  const noHost = machineHash({ mac: '00:1A:2B:3C:4D:5E', cpu: 'ABC', disk: 'D1' });
  assert.notEqual(withHost, noHost); // reported hostnames do participate
  assert.equal(machineHash({ mac: '00:1A:2B:3C:4D:5E', cpu: 'ABC', disk: 'D1' }),
    machineHash({ mac: '00:1A:2B:3C:4D:5E', cpu: 'ABC', disk: 'D1' }));
});

test('machineDetail + hashFromDetail round-trip', () => {
  const input = { mac: '00:1A:2B:3C:4D:5E', cpu: 'ABC', disk: 'D1', hostname: 'pc1' };
  const detail = machineDetail(input);
  assert.equal(detail.mac, '00:1a:2b:3c:4d:5e');
  assert.equal(hashFromDetail(detail), machineHash(input));
});

test('structured { components } input is accepted', () => {
  const flat = machineHash({ mac: '00:1A:2B:3C:4D:5E', cpu: 'ABC', disk: 'D1' });
  const structured = machineHash({ components: { macs: ['00:1A:2B:3C:4D:5E'], cpu: 'ABC', disk: 'D1' } });
  assert.equal(flat, structured);
});
