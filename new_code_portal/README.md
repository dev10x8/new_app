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
| `backend/test/` | Test suite (33 tests) |
| `web/` | SPA: `menu.js` (routes), `screens.js`, `api.js`, `ui.js`, `app.js`, `styles.css` |
| `web/downloads/` | Machine-fingerprint helper scripts (PowerShell + shell) |
| `scripts/` | `start.js`, `seed_demo.js`, `capture_screenshots.js` |

## Run

```bash
npm start          # backend on 127.0.0.1:7200 + web on 0.0.0.0:8081
npm run seed-demo  # reset DB and load a demo dataset
npm test           # backend test suite
npm run reset-db
npm run gen-license-key
```

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

## API

Catalogue: `GET /api/health`, `/api/meta`, `/api/catalogue`, `/api/plans`,
`/api/periods`, `/api/modules`, `/api/addons`, `/api/comparison`.

Quoting and onboarding: `POST /api/quote`, `POST /api/businesses`,
`GET /api/businesses`, `GET|PUT /api/businesses/:id`,
`POST|DELETE /api/businesses/:id/modules`.

Licensing: `POST /api/licenses`, `GET /api/licenses`, `GET /api/licenses/:id`,
`GET /api/licenses/:id/file`, `POST /api/licenses/:id/activate`,
`POST /api/licenses/:id/verify`, `POST /api/licenses/:id/revoke`,
`POST /api/licenses/:id/resign`, `GET /api/licenses/:id/activations`,
`GET /api/license-key`.

Portal: `GET /api/dashboard`, `GET /api/audit`.
