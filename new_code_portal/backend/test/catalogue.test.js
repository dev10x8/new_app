import test from 'node:test';
import assert from 'node:assert/strict';
import { freshDb } from './helpers.mjs';
import * as catalogue from '../src/services/catalogue.js';

test('catalogue seeds the three plans with the vendor prices', () => {
  const db = freshDb();
  const plans = catalogue.plans(db);
  assert.equal(plans.length, 3);
  const byCode = Object.fromEntries(plans.map((p) => [p.code, p]));
  assert.equal(byCode.basic.monthly, 499);
  assert.equal(byCode.standard.monthly, 1199);
  assert.equal(byCode.premium.monthly, 1499);
  assert.equal(byCode.basic.prices.oneYear, 5599);
  assert.equal(byCode.standard.prices.threeYears, 29999);
  assert.equal(byCode.premium.prices.twoYears, 29999);
  assert.equal(byCode.standard.popular, true);
});

test('billing periods are ordered shortest first', () => {
  const db = freshDb();
  const periods = catalogue.periods(db);
  assert.deepEqual(periods.map((p) => p.code), ['monthly', 'oneYear', 'twoYears', 'threeYears']);
});

test('plan module bundles grow from basic to premium', () => {
  const db = freshDb();
  const byCode = Object.fromEntries(catalogue.plans(db).map((p) => [p.code, p]));
  assert.ok(byCode.basic.modules.includes('EXCISE'));
  assert.ok(!byCode.basic.modules.includes('TALLY'));
  assert.ok(byCode.standard.modules.includes('CLOUD'));
  assert.ok(!byCode.standard.modules.includes('TALLY'));
  assert.ok(byCode.premium.modules.includes('TALLY'));
  assert.ok(byCode.premium.modules.includes('AI_ASSISTANT'));
  assert.ok(byCode.premium.modules.length > byCode.standard.modules.length);
  assert.ok(byCode.standard.modules.length > byCode.basic.modules.length);
});

test('comparison matrix mirrors the published features', () => {
  const db = freshDb();
  const groups = catalogue.comparison(db);
  const flat = groups.flatMap((g) => g.features.map((f) => [f.name, f]));
  const get = (name) => flat.find(([n]) => n === name)[1];
  assert.equal(get('Offline Usage').basic, true);
  assert.equal(get('AMC Support').basic, false);
  assert.equal(get('AMC Support').standard, true);
  assert.equal(get('Tally Integration').standard, false);
  assert.equal(get('Tally Integration').premium, true);
});

test('planPrice falls back to the monthly rate for an unknown period', () => {
  const db = freshDb();
  assert.equal(catalogue.planPrice(db, 'basic', 'oneYear'), 5599);
  assert.equal(catalogue.planPrice(db, 'basic', 'nonsense'), 499);
});

test('add-ons are listed with their prices', () => {
  const db = freshDb();
  const addons = catalogue.addons(db);
  const byCode = Object.fromEntries(addons.map((a) => [a.code, a]));
  assert.equal(byCode.cloud.price, 7999);
  assert.equal(byCode.tally.price, 5999);
  assert.equal(byCode.epos.price, 3999);
});
