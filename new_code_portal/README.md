# VanijLiquor Customer Portal

A self-service portal for VanijLiquor customers: onboard a business, choose a
plan and modules, get a price quote, issue a machine-bound licence, and activate
or verify it on the counter PC.

This is a separate app from the POS rebuild in `../new_code`. It reuses the same
stack and conventions (Node.js + `node:sqlite` + a framework-free web UI).

## Stack

- **Backend**: Node.js (`node:http`) + SQLite (`node:sqlite`). No external
  runtime dependencies — Node >= 22.5 only.
- **Web**: framework-free SPA (HTML/CSS/JS) served by `web/server.js`, which
  also proxies `/api` to the backend. This is a browser web app; it does not
  ship a desktop shell.

## Layout

| Path | Contents |
| --- | --- |
| `backend/src/server.js` | HTTP server and routes |
| `backend/src/db/` | `schema.sql`, `seed.sql` (plans, modules, add-ons) |
| `backend/src/services/` | `onboarding`, `catalogue`, `licensing`, `license` (Ed25519 signing), `fingerprint`, `audit` |
| `backend/test/` | Test suite (41 tests) |
| `web/` | SPA: `menu.js` (routes), `screens.js`, `api.js`, `ui.js`, `app.js`, `styles.css` |
| `web/downloads/` | Machine-fingerprint helper scripts (PowerShell + shell) |
| `scripts/` | `start.js`, `seed_demo.js`, `capture_screenshots.js` |
| `docs/` | User guide PDF, `build_user_guide.py` and `docs/screenshots/` |

## Run

```bash
npm start          # backend on 127.0.0.1:7200 + web on 0.0.0.0:8081
npm run seed-demo  # reset DB and load a demo dataset
npm test           # backend test suite
npm run reset-db
npm run gen-license-key
npm run screenshots  # capture docs/screenshots/*.png from the running portal
npm run guides       # build docs/VanijLiquor_Customer_Portal_User_Guide.pdf
```

## Documentation

The user guide — `docs/VanijLiquor_Customer_Portal_User_Guide.pdf` — walks through
every portal feature with screenshots. It is generated from the app's own sources:

1. `npm start` and (optionally) `npm run seed-demo` to load the demo dataset.
2. `npm run screenshots` — drives headless Chromium over CDP to capture
   `docs/screenshots/*.png` (no npm dependencies).
3. `npm run guides` — builds the PDF with `docs/build_user_guide.py`
   (needs `reportlab` and `Pillow`).

## Licensing model

1. Onboard the business and pick a plan, term and modules (`Onboard`).
2. On the counter PC, run the app or the fingerprint helper to produce
   `machine-fingerprint.json`.
3. Paste the fingerprint under `Issue Licence` to mint a `.vlic` file.
4. Import the `.vlic` in the app — it verifies offline and unlocks the modules.

Licences are signed with an Ed25519 key. The VanijLiquor POS app pins the
public key and verifies offline. **Set `PORTAL_LICENSE_KEY` for production**;
without it the portal uses a development signing key and the UI flags it as
non-production.

The licence binds to the machine fingerprint (MAC, CPU id, disk serial,
hostname). Activation on a different machine is refused
(`Licence is bound to a different machine`).

### Soft enforcement (grace, offline, seats)

Every licence carries three tolerances, all baked into the signed payload so
the POS app can honour them with no network:

- **Grace window** (`grace_days`, default 15) — after `valid_until` the licence
  still works for this many days, reported as the state `grace`, so a renewal
  that lands a little late does not stop a paying shop. Past it the licence is
  `expired` and refused.
- **Offline limit** (`offline_limit_days`, default 30) — days the app may run
  without a check-in before it drops to limited *educational* mode (Tally-style)
  rather than refusing to run.
- **Seats** (`seat_limit`, default 1) — how many distinct machines the licence
  may bind.

Moving a licence to a new machine is an explicit, audited **surrender**: it
releases every binding (`POST /api/licenses/:id/surrender`) so the next
activation re-binds. Set the defaults with `PORTAL_GRACE_DAYS`,
`PORTAL_OFFLINE_LIMIT_DAYS` and `PORTAL_SEAT_LIMIT`, or per licence on issue.

States returned by `GET /api/licenses/:id/status`: `valid`, `grace`, `expired`,
`revoked`, plus an `offline` block (`online` or `educational`).

## API

Catalogue: `GET /api/health`, `/api/meta`, `/api/catalogue`, `/api/plans`,
`/api/periods`, `/api/modules`, `/api/addons`, `/api/comparison`.

Quoting and onboarding: `POST /api/quote`, `POST /api/businesses`,
`GET /api/businesses`, `GET|PUT /api/businesses/:id`,
`POST|DELETE /api/businesses/:id/modules`.

Licensing: `POST /api/licenses`, `GET /api/licenses`, `GET /api/licenses/:id`,
`GET /api/licenses/:id/file`, `POST /api/licenses/:id/activate`,
`POST /api/licenses/:id/verify`, `POST /api/licenses/:id/surrender`,
`GET /api/licenses/:id/status`, `POST /api/licenses/:id/revoke`,
`POST /api/licenses/:id/resign`, `GET /api/licenses/:id/activations`,
`GET /api/license-key`.

Portal: `GET /api/dashboard`, `GET /api/audit`.
