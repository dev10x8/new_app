# Sidebar navigation — two design options

Status: **both options implemented**; switchable at runtime from the sidebar
footer button ("Cards menu" / "Expand menu"). The choice is saved in
`localStorage` under `navMode`.

Scope: the vanilla-JS frontend in `new_code/web/`. One route table
(`web/menu.js`) drives both options; screens live in `web/screens.js`. No new
dependencies.

---

## Shared model

`web/menu.js` exports the navigation tree `NAV` — an ordered list of groups,
each with items (`{ title, route }`). Group order is:

```
Main · Masters · Transactions · Reports · Dash Board · Tools · Help · Admin
```

`System`/`Help`/`Admin` sit last; `Main` first. Two derived maps keep the menu
in sync with the router:

- `ROUTE_TITLES` — route → item title (page heading).
- `ROUTE_GROUP` — route → owning group title (so a screen can highlight/open
  its group). A route must not be shared by two items, or the active group is
  ambiguous — `Dash Board → Sales Status` therefore has its own `dash:sales`
  route rather than reusing `dashboard`.

The sidebar always lists **group headers only** — never a flat list of every
submenu. The two options differ in *where the submenus appear*.

---

## Option A — inline expand (default)

The submenu expands inside the sidebar, directly under the group header.

- Clicking a group header toggles its items inline (one group open at a time;
  opening another closes the previous).
- The group owning the current route opens automatically on navigation, so the
  active item is always visible.
- A gap separates the header from its first item; the panel animates open/closed
  with `grid-template-rows: 0fr → 1fr`, and the chevron rotates.
- While collapsed, the items are `visibility: hidden` so they are skipped by the
  keyboard tab order, not merely clipped.

Route ids: item routes directly (e.g. `items`). No extra routes.

**Pros** — everything in one column; no context switch; familiar
tree-menu feel; the main panel shows only the working screen.

**Cons** — a long group (Masters has 27 items) pushes the headers below it out
of view and the sidebar scrolls; deep menus make the sidebar tall.

---

## Option B — cards in the main panel

The sidebar stays a short, fixed list of group headers. Clicking one opens a
**launcher** in the main panel: the group's items as small clickable boxes
(cards). Clicking a box opens that item's screen.

- The launcher route is `group:<title>` (e.g. `group:Masters`), handled by
  `groupLauncher()` in `screens.js`. `isGroupRoute()` / `groupOfRoute()` in
  `menu.js` recognise it.
- Each card shows the item's icon, its title, and a chevron that fades in on
  hover; the card for the current route is highlighted.
- A header block (group icon + title + item count) titles the launcher.
- The sidebar highlights the group currently open in the main panel.

**Pros** — the sidebar is short and stable no matter how many items a group
has; items are large, labelled targets; good for touch/POS use and for
discoverability.

**Cons** — two clicks to reach a screen (group, then item); adds a landing
screen the operator must step through; more main-panel surface to style.

---

## Switching

The sidebar footer button toggles between the two modes and persists the choice.
Switching to *expand* while a `group:<title>` launcher route is open navigates to
that group's first item so the main panel never shows a launcher the sidebar can
no longer produce.

---

## Notes / possible follow-ups

- The mode toggle is a temporary in-app switch to compare the options; once a
  direction is chosen, one mode can become the fixed design (and the toggle
  removed, or moved to Admin → Parameters).
- Option B could grow a "recent" row or a search box above the cards.
- Neither option changes routing semantics for screens; both are pure shell.
