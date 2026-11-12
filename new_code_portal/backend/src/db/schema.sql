-- VanijLiquor Customer Portal schema (SQLite).
--
-- Two halves:
--   * Catalogue  - plans, billing periods, modules, add-ons and the plan
--                  comparison matrix (seeded from the vendor's public plans).
--   * Tenancy    - businesses (the shop owners who onboard), their chosen
--                  modules, issued licences, machine activations, users and an
--                  audit log.
--
-- Amounts are whole rupees (the vendor's plans are integer-priced).

-- ------------------------------------------------------------- catalogue
CREATE TABLE IF NOT EXISTS plans (
  code        TEXT PRIMARY KEY,
  name        TEXT NOT NULL,
  tagline     TEXT NOT NULL DEFAULT '',
  monthly     INTEGER NOT NULL DEFAULT 0,
  popular     INTEGER NOT NULL DEFAULT 0,
  highlights  TEXT NOT NULL DEFAULT '[]',   -- JSON array of short bullet strings
  sort        INTEGER NOT NULL DEFAULT 0
);

CREATE TABLE IF NOT EXISTS billing_periods (
  code   TEXT PRIMARY KEY,          -- monthly | oneYear | twoYears | threeYears
  label  TEXT NOT NULL,             -- Month | 1 Year | 2 Years | 3 Years
  months INTEGER NOT NULL,
  sort   INTEGER NOT NULL DEFAULT 0
);

CREATE TABLE IF NOT EXISTS plan_prices (
  plan_code   TEXT NOT NULL,
  period_code TEXT NOT NULL,
  amount      INTEGER NOT NULL,
  PRIMARY KEY (plan_code, period_code)
);

CREATE TABLE IF NOT EXISTS modules (
  code        TEXT PRIMARY KEY,
  name        TEXT NOT NULL,
  category    TEXT NOT NULL DEFAULT '',
  description TEXT NOT NULL DEFAULT '',
  addon_price INTEGER NOT NULL DEFAULT 0,
  sort        INTEGER NOT NULL DEFAULT 0
);

-- Which modules a plan includes. `included` is 1 when the plan bundles the
-- module and 0 when it is only available as a paid add-on at that tier.
CREATE TABLE IF NOT EXISTS plan_modules (
  plan_code   TEXT NOT NULL,
  module_code TEXT NOT NULL,
  included    INTEGER NOT NULL DEFAULT 0,
  PRIMARY KEY (plan_code, module_code)
);

CREATE TABLE IF NOT EXISTS addons (
  code     TEXT PRIMARY KEY,
  name     TEXT NOT NULL,
  detail   TEXT NOT NULL DEFAULT '',
  price    INTEGER NOT NULL DEFAULT 0,
  period   TEXT NOT NULL DEFAULT 'Yearly',
  sort     INTEGER NOT NULL DEFAULT 0
);

CREATE TABLE IF NOT EXISTS comparison (
  id        INTEGER PRIMARY KEY AUTOINCREMENT,
  category  TEXT NOT NULL,
  feature   TEXT NOT NULL,
  basic     INTEGER NOT NULL DEFAULT 0,
  standard  INTEGER NOT NULL DEFAULT 0,
  premium   INTEGER NOT NULL DEFAULT 0,
  sort      INTEGER NOT NULL DEFAULT 0
);

-- ------------------------------------------------------------- tenancy
CREATE TABLE IF NOT EXISTS businesses (
  id             INTEGER PRIMARY KEY AUTOINCREMENT,
  business_code  TEXT NOT NULL UNIQUE,      -- e.g. VL-2026-0001
  legalname      TEXT NOT NULL DEFAULT '',
  tradename      TEXT NOT NULL DEFAULT '',
  ownername      TEXT NOT NULL DEFAULT '',
  email          TEXT NOT NULL DEFAULT '',
  phone          TEXT NOT NULL DEFAULT '',
  gstin          TEXT NOT NULL DEFAULT '',
  panno          TEXT NOT NULL DEFAULT '',
  address1       TEXT NOT NULL DEFAULT '',
  city           TEXT NOT NULL DEFAULT '',
  district       TEXT NOT NULL DEFAULT '',
  state          TEXT NOT NULL DEFAULT '',
  statecode      TEXT NOT NULL DEFAULT '',
  pincode        TEXT NOT NULL DEFAULT '',
  businesstype   TEXT NOT NULL DEFAULT '',   -- retail | wholesale | bar | club
  outlets        INTEGER NOT NULL DEFAULT 1,
  plan_code      TEXT NOT NULL DEFAULT 'basic',
  period_code    TEXT NOT NULL DEFAULT 'monthly',
  status         TEXT NOT NULL DEFAULT 'draft', -- draft | onboarding | active | suspended
  onboarding_step INTEGER NOT NULL DEFAULT 1,
  createdat      TEXT NOT NULL DEFAULT (datetime('now')),
  updatedat      TEXT NOT NULL DEFAULT (datetime('now'))
);

CREATE TABLE IF NOT EXISTS business_modules (
  business_id INTEGER NOT NULL,
  module_code TEXT NOT NULL,
  status      TEXT NOT NULL DEFAULT 'active', -- active | trial | off
  PRIMARY KEY (business_id, module_code)
);

CREATE TABLE IF NOT EXISTS licenses (
  id              INTEGER PRIMARY KEY AUTOINCREMENT,
  license_id      TEXT NOT NULL UNIQUE,       -- e.g. VL-LIC-2F9A1C
  business_id     INTEGER NOT NULL,
  plan_code       TEXT NOT NULL,
  period_code     TEXT NOT NULL,
  status          TEXT NOT NULL DEFAULT 'issued', -- issued | active | revoked | expired
  machine_hash    TEXT NOT NULL,              -- SHA-256 of the normalised fingerprint
  machine_detail  TEXT NOT NULL DEFAULT '{}', -- JSON: mac, cpu, disk, hostname, os
  entitlements    TEXT NOT NULL DEFAULT '[]', -- JSON: module codes unlocked
  valid_from      TEXT NOT NULL,
  valid_until     TEXT NOT NULL,
  issued_at       TEXT NOT NULL DEFAULT (datetime('now')),
  payload         TEXT NOT NULL,              -- canonical JSON that was signed
  signature       TEXT NOT NULL,              -- Ed25519 signature (base64)
  public_key      TEXT NOT NULL DEFAULT '',   -- Ed25519 public key (base64) for offline verify
  -- Grace window after expiry, and the offline (no check-in) tolerance before the
  -- client drops to limited "educational" mode. Both are baked into the signed
  -- payload so the desktop can enforce them with no network.
  grace_days         INTEGER NOT NULL DEFAULT 0,
  offline_limit_days INTEGER NOT NULL DEFAULT 0,
  -- Seats: how many distinct machines this licence may bind. The first
  -- activation pins the machine (bound_machine_hash); moving it is an explicit
  -- surrender + reactivate, not a silent re-bind.
  seat_limit         INTEGER NOT NULL DEFAULT 1,
  bound_machine_hash TEXT NOT NULL DEFAULT '',
  surrendered_at     TEXT NOT NULL DEFAULT ''
);

CREATE TABLE IF NOT EXISTS activations (
  id           INTEGER PRIMARY KEY AUTOINCREMENT,
  license_id   TEXT NOT NULL,
  machine_hash TEXT NOT NULL,
  activated_at TEXT NOT NULL DEFAULT (datetime('now')),
  released_at  TEXT NOT NULL DEFAULT '',      -- set when the licence is surrendered
  ip           TEXT NOT NULL DEFAULT '',
  detail       TEXT NOT NULL DEFAULT ''
);

CREATE TABLE IF NOT EXISTS portal_users (
  id           INTEGER PRIMARY KEY AUTOINCREMENT,
  email        TEXT NOT NULL UNIQUE,
  name         TEXT NOT NULL DEFAULT '',
  password     TEXT NOT NULL,
  salt         TEXT NOT NULL,
  business_id  INTEGER,
  createdat    TEXT NOT NULL DEFAULT (datetime('now'))
);

CREATE TABLE IF NOT EXISTS audit_log (
  id        INTEGER PRIMARY KEY AUTOINCREMENT,
  at        TEXT NOT NULL DEFAULT (datetime('now')),
  actor     TEXT NOT NULL DEFAULT '',
  action    TEXT NOT NULL,
  entity    TEXT NOT NULL DEFAULT '',
  detail    TEXT NOT NULL DEFAULT ''
);

CREATE TABLE IF NOT EXISTS portal_meta (
  key   TEXT PRIMARY KEY,
  value TEXT NOT NULL
);
