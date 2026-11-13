# Backend Code Protection — Design (Node.js anti-reverse-engineering)

Status: **design / not yet implemented**. The single-executable backend
(esbuild + Node SEA + postject, shipped in v1.0.7) is in place; the hardening
steps below are planned for a later release.
Scope: how to make the shipped desktop backend harder to read and copy, what
each option really buys, and the recommended implementation order. Applies to
`new_code/tauri/scripts/build_backend.mjs` and the backend under
`new_code/backend/`.

---

## 1. Problem

`C:\Program Files\VanijLiquor\runtime\backend` originally shipped readable
`.js`, exposing the application logic and business flow. v1.0.7 removed the
loose files by compiling the backend into one executable
(`runtime/backend/backend.exe`). That is a real improvement, but the bundled
JavaScript is still embedded verbatim inside the exe (Node SEA stores the blob
as plain data), so `strings` / blob extraction recovers it with modest effort.
This document records the options for going further and the order to apply
them.

## 2. Threat model (be honest about the ceiling)

No client-side technique makes shipped code *safe* from reverse engineering.
The realistic goal is to raise the cost from "anyone opens the file" to
"a motivated, skilled person needs real time and tooling". The only true
protection is code the customer never receives (server-side logic).

We care about two distinct assets, and they behave differently:

- **Code structure** (function/variable names, control flow) — hides well with
  obfuscation.
- **Data model and query logic** (route paths, SQL table/column names, JSON
  field names such as `usercode`, `trial_started_at`, config keys) — cannot be
  renamed, because they are string literals the app looks up dynamically. For a
  POS these strings are most of the "business flow" a competitor would want.

## 3. Options

ProGuard has no 1:1 JavaScript equivalent: Java compiles to a structured
bytecode that can be rewritten deterministically, whereas JavaScript ships as
text and runs in an interpreter, so a JS "obfuscator" is a source-to-source
transform. The ProGuard jobs map roughly as:

| ProGuard job | JavaScript equivalent |
|---|---|
| Shrink (dead-code removal) | esbuild / terser tree-shaking (already used) |
| Optimize | terser `compress`, Google Closure advanced mode |
| Obfuscate (rename) | javascript-obfuscator, terser `mangle`, JSDefender, Jscrambler |
| Preverify | not applicable |

### 3.1 Tooling

**Free / open source**

- **javascript-obfuscator** (npm) — closest free analog to ProGuard's obfuscate
  step. Identifier renaming, string-array encoding (base64/rc4), string
  splitting, **control-flow flattening**, dead-code injection, self-defending,
  debug protection. Materially stronger than `minify` alone.
- **terser** — minify + `mangle`; essentially what esbuild `minify` already does.
- **Google Closure Compiler** — aggressive optimize + rename.
- **Bytenode** — compiles JS to V8 bytecode (`.jsc`), the closest thing to a
  "compiled" target. Caveats: locked to the exact Node/V8 build, and bytecode is
  disassemblable — it hides source but is not encryption.

**Commercial** (where the real ProGuard siblings live)

- **PreEmptive JSDefender** — the JS sibling of their Java (DashO) and .NET
  (Dotfuscator) protectors; direct ProGuard analog.
- **Jscrambler** — transformations + self-defending + code locks (domain/expiry
  binding) + runtime anti-tampering/anti-debugging. Enterprise pricing.
- **Jshaman**, **ProtectJS** — paid services in the same space.

**Packers** (applied to the finished exe, not the source): VMProtect, Themida,
Enigma — stronger, but can trip antivirus.

### 3.2 Non-JS alternatives

- **Rust rewrite.** Compiles to native code, so no source is embedded — a
  genuine step up from the SEA blob. But native decompilers (Ghidra, IDA,
  Binary Ninja) recover Rust logic with effort; string literals and SQL remain
  readable via `strings`; licensing branches are still patchable. It changes
  "readable in minutes" to "readable in hours–days", which is obfuscation, not
  safety. A full Node→Rust port of 30+ service modules, 135 tests, SQLite
  access, licensing and reports is a multi-month project with real regression
  risk — hard to justify on IP grounds alone. (Legitimate reasons to consider
  Rust separately: single small native binary instead of an ~87 MB Node-based
  exe, no bundled runtime, performance.)
- **Server-side logic.** The only approach that actually protects the code:
  move licensing enforcement, plan/module gating, and proprietary calculations
  behind the portal API and keep only UI/glue in the desktop app. Code you do
  not ship cannot be stolen. Fits the existing model — the portal is already
  authoritative for licences.

## 4. Recommended approach

Ordered by value per unit of effort:

1. **Obfuscate the existing bundle (free, no architecture change).** Insert
   javascript-obfuscator into the current pipeline between esbuild and SEA:

   ```
   esbuild bundle  ->  javascript-obfuscator  ->  SEA blob  ->  postject  ->  backend.exe
   ```

   Gains control-flow flattening, string-array encoding and dead code on top of
   today's minify. This is the planned "later" work (section 5).
2. **Move the crown jewels server-side.** Identify the modules worth protecting
   (licensing/term enforcement, plan and module gating, any proprietary
   report/calculation) and move them behind the portal, with the client
   verifying against it. Highest ceiling; independent of step 1.
3. **Optional stronger client hardening.** Bytenode bytecode (free but fragile,
   Node-version-locked) or a commercial protector (JSDefender / Jscrambler /
   a packer) if the budget exists.
4. **Protect the data, not just the code.** The SQLite schema and data are the
   most easily cloned assets; licensing already addresses this partly — treat
   the DB and the customer relationship as the real moat.
5. **Rust rewrite** only if the architecture/performance case justifies it on
   its own merits; do not do it for IP protection alone.

## 5. Implementation plan — javascript-obfuscator (step 1)

Contained change; no path or shell changes required.

1. Add `javascript-obfuscator` to `new_code/tauri/package.json` devDependencies.
2. In `new_code/tauri/scripts/build_backend.mjs`, after the esbuild step and
   before writing the SEA blob, run the bundle through javascript-obfuscator
   with a conservative preset:
   - `compact: true`, `controlFlowFlattening: true` (moderate threshold),
     `stringArray: true`, `stringArrayEncoding: ['base64']`,
     `stringArrayThreshold` moderate, `deadCodeInjection` low,
     `selfDefending: true`, `disableConsoleOutput: false` (the shell tees
     stdout/stderr to `backend.log`; keep logging).
   - Keep `identifierNamesGenerator` deterministic (e.g. `hexadecimal` with a
     fixed `seed`) so builds are reproducible.
   - **Do not** enable `debugProtection`/`selfDefending` blindly: verify the
     SEA/postject-wrapped exe still boots, and that startup time stays
     acceptable.
3. Keep the existing fallback: if the obfuscation step throws, fall back to the
   plain (minified) bundle so the build never breaks.
4. Verify (section 6) before shipping.

Known constraints to design around:

- String literals (routes, SQL, JSON field names, config keys) are not renamed —
  expect them to remain visible. Consider moving the most sensitive ones
  server-side (section 4, step 2) rather than relying on encoding.
- Obfuscation increases bundle size and startup cost; measure both.
- Some obfuscation transforms are themselves reversible; treat the result as
  "harder to read", not "protected".

## 6. Verification

- `cd new_code/backend && node --test` — 135/135 still pass (source unchanged).
- Rebuild the exe via `node new_code/tauri/scripts/build_backend.mjs`; confirm it
  serves `/api/license`, logs in, and handles `reset-pin` / `reset-db`.
- The CI smoke test in `.github/workflows/package-windows.yml`
  ("Smoke-test the bundled backend exe") must stay green — it runs the real
  Windows exe, so it is the gate for any obfuscation regression.
- Compare `strings backend.exe | grep -c '<known identifier>'` and exe size /
  cold-start time before and after, to confirm the obfuscation actually took
  effect and did not regress boot.

## 7. Honest summary

Every client-side step here — the current SEA exe, javascript-obfuscator,
Bytenode, a commercial protector, or a Rust rewrite — is obfuscation: it raises
the effort bar, it does not make the code unrecoverable. Only moving the
sensitive logic behind the portal removes the exposure. Recommended sequence:
obfuscate now (cheap), then migrate the genuinely valuable logic server-side.
