# UI Polish — Design (VanijLiquor desktop)

Status: **Tier 1 implemented**. Tiers 2–3 are designed here, not yet built.
Scope: modernise the vanilla-JS frontend in `new_code/web/` to the dark,
zinc-surfaced, warm-accent look of the shadcn/ui `dashboard-01` block (the
`create-tauri-ui` demo), without changing the stack.

---

## 1. The reference

The target is the shadcn/ui `dashboard-01` block in dark mode — the demo output
of `agmmnn/tauri-ui` (a scaffolding CLI that runs `shadcn add dashboard-01`;
there is no bespoke design system to adopt). Measured from the reference:

- **Layered dark greys, not pure black**: canvas `#09090b`, sidebar/titlebar
  `#111113`, raised cards `#141416` / `#18181b`, 1px hairlines `#27272a`.
  Depth comes from border + surface steps, not drop shadows.
- **Text ramp**: headings/metrics `#fafafa`, secondary `#a1a1aa`, muted
  `#71717a`.
- **One warm accent**, used sparingly (active nav, one highlighted bar, a badge).
- **Primary CTA**: near-white pill (`#fafafa` on `#09090b`) — or the warm accent.
- **Structure**: ~240px sidebar (brand → quick action → grouped nav → sticky user
  footer) + fluid main (header → KPI grid → input/chart cards → tabs) + an
  optional right drawer.
- **Details**: ~10px radius, pill trend badges (`+12.5%`, `-20%`) with a
  directional icon, `tabular-nums` for figures, monospace for technical values,
  Inter/Geist for UI text.

`new_code/web/` already has the right skeleton (CSS-variable tokens, dark
sidebar, sticky topbar, `.card`, `.stat`, tables, modal, toast), so this is a
restyle + component polish, not a rebuild.

## 2. Constraints

- **Keep the vanilla stack.** The app is plain JS + a custom `build_web.mjs`,
  zero runtime deps — ideal for Tauri (small, fast). Adopting shadcn *literally*
  means React + Tailwind + a Vite build; that is a rearchitecture for a cosmetic
  goal and is deliberately out of scope. We port the *look and tokens*, not the
  stack.
- **No fabricated data.** KPI deltas and sparklines must come from real
  endpoints; where the backend has no figure, the UI shows none rather than a
  placeholder.
- **Desktop-first.** Minimum comfortable width ~1024px; degrade gracefully to
  the existing mobile sidebar behaviour.

## 3. Tier 1 — quick wins (no new dependencies)

Doable entirely inside the current codebase. This is the work in progress.

1. **Theme tokens (dark default + light)** — rewrite the `:root` block to the
   dark zinc palette above, add a `[data-theme="light"]` override, and a toggle
   persisted in `localStorage`. Keep the existing token *names* so the ~260
   lines of current CSS keep working.
2. **Warm accent** — brand switches from teal to amber (`#e0b45f`) with a dark
   `--brand-ink` for text on the accent; teal is retired.
3. **Sidebar** — inline-SVG icon per nav item (route → icon map with group and
   generic fallbacks), grouped section labels (already present), avatar footer.
4. **KPI cards** — value + trend pill (delta vs previous period) + inline-SVG
   sparkline, fed by a small `/api/dashboard` extension (real 7-day series).
5. **Tables** — status pills (existing `.badge` variants), right-aligned
   `tabular-nums`, row hover.
6. **Focus + motion** — a consistent `:focus-visible` ring and subtle
   transitions; respect `prefers-reduced-motion`.

## 4. Tier 2 — component layer

- **Primitives** — button/input/select/dialog/toast/dropdown/tabs styled once
  (hand-rolled CSS or shadcn-style copy-in), replacing ad-hoc markup.
- **⌘K command palette** — jump to any screen; `menu.js` already enumerates
  routes, so this is cheap and high-value for a POS with ~80 screens.
- **Charts** — only if wanted: uPlot (~40 KB) or hand-rolled SVG. Avoid heavy
  chart deps.
- **Right-side drawer** — a contextual inspector (e.g. bill detail, item
  history) mirroring the reference's debug drawer, for detail-without-leaving.
- **Empty / loading / error states** — consistent skeletons and messages.

## 5. Tier 3 — architecture (deliberate, separate decision)

Adopting **shadcn/ui literally requires React + Tailwind + Vite**. That is a
frontend rewrite of ~3,200 lines. Worth it only if the team wants React for its
own sake (component ecosystem, hiring, testing). It is *not* required for the
visual result — Tier 1 gets ~80% of the look at ~5% of the cost. Keep it as a
separate, deliberate migration, not a side effect of a restyle.

## 6. Verification

- `npm --prefix new_code/web run build` — `dist/` regenerates (Tauri bundles it).
- `node --test` in `new_code/backend` — 135 tests stay green (the dashboard
  extension adds one).
- The Windows CI smoke test is unaffected (frontend is embedded by Tauri; the
  backend exe is unchanged apart from the dashboard query).
- Visual: compare against the reference and the approved mockup.
