# Madhushala Pro — Developer Guide

How to set up the environment, decompile the application, and find or reset the
admin login PIN.

See also:
- `PRODUCT_FEATURES.md` — menus, submenus and business flows.
- `decode/README.md` — what is in the decompiled `decode/` tree.

> These instructions are for maintaining and recovering a system you own or are
> authorised to service. They modify the application's local database.

---

## 1. Repository layout

```
.
├── Debug/                       # the shipped, runnable application
│   ├── Madhushala Pro.exe       # main binary (Git LFS pointer in the repo)
│   ├── *.dll                    # app + third-party assemblies
│   └── Resources/
│       ├── barmanager.mdf       # live database (SQL Server data file)
│       ├── barmanager_demo.mdf  # demo database
│       └── *.ldf                # transaction logs
├── decode/                      # decompiled C# (generated; see README there)
├── dev/                         # this developer tooling
│   ├── developer_guide.md
│   ├── docker-compose.yml       # decompile toolchain in Docker
│   ├── docker/
│   │   └── Dockerfile           # the toolchain image
│   ├── scripts/
│   │   ├── setup_decompile_tools.sh
│   │   ├── decompile.sh
│   │   ├── mdf_users.py
│   │   └── admin_pin.sh
│   └── sql/
│       └── reset_admin_pin.sql
└── PRODUCT_FEATURES.md
```

The app is a .NET Framework 4.0 VB.NET WinForms program; the database is
Microsoft SQL Server.

---

## 2. Decompiling the application

### 2.1 Toolchain

Install the tools (see `dev/scripts/setup_decompile_tools.sh`):

| Tool | Why | Install |
| --- | --- | --- |
| .NET SDK 8 | runs `ilspycmd` | `dotnet-install.sh --channel 8.0` |
| `ilspycmd` (ILSpy 8.2+) | command-line decompiler | `dotnet tool install -g ilspycmd` |
| `git-lfs` | the main EXE is stored in LFS | `git lfs install` |

The tooling used to produce the current `decode/` tree:

```
dotnet 8.0.427
ilspycmd 8.2.0.7535  (ICSharpCode.Decompiler 8.2.0.7535)
```

Make sure `$HOME/.dotnet` and `$HOME/.dotnet/tools` are on `PATH`.

### 2.2 Fetch the real binaries (LFS)

`Debug/Madhushala Pro.exe` is tracked with Git LFS; a fresh clone gets a
~134-byte text pointer, not the 181 MB binary. Fetch it first:

```bash
git lfs install
git lfs pull                 # or: git lfs fetch && git lfs checkout
ls -l "Debug/Madhushala Pro.exe"   # should be ~181 MB, not 134 bytes
```

A 134-byte file starting with `version https://git-lfs.github.com/spec/v1` is a
pointer, not the binary.

### 2.3 Run the decompiler

Decompile everything under `Debug/` into `decode/`:

```bash
dev/scripts/decompile.sh
```

Decompile specific assemblies:

```bash
dev/scripts/decompile.sh "Debug/Madhushala Pro.exe" "Debug/MadhushalaDataSync.dll"
```

Under the hood the script runs, per assembly:

```bash
ilspycmd -o "decode/<name>" "Debug/<name>"
# -> decode/<name>/<name>.decompiled.cs
```

Useful `ilspycmd` options if you want a compilable project instead of a single
file:

```bash
# one source file per type, namespaces as nested folders, plus a .csproj
ilspycmd -p --nested-directories -o decode/proj "Debug/Madhushala Pro.exe"

# decompile just one type (namespaces are e.g. Madhushala_Ultimate)
ilspycmd -t "Madhushala_Ultimate.frmLoginNew" "Debug/Madhushala Pro.exe"

# list types in an assembly
ilspycmd -l c "Debug/NumberToWord.dll"
```

Notes:
- `ilspycmd` needs the assembly's dependencies to resolve types. If you see
  unresolved-type warnings, pass `-r Debug` so it can find the sibling DLLs:
  `ilspycmd -r Debug -o decode/x "Debug/x.dll"`.
- `chromedriver.exe` and the `*.vshost.exe` shims are native/unmanaged and
  cannot be decompiled.

### 2.4 Manual / no-script route

If you prefer to do it by hand:

```bash
mkdir -p decode
for f in Debug/*.dll Debug/*.exe; do
  [ "$(basename "$f")" = "chromedriver.exe" ] && continue
  case "$f" in *.vshost.exe) continue;; esac
  ilspycmd -o "decode/$(basename "$f")" "$f"
done
```

### 2.5 Where to look in the decompiled source

The main app decompiles to one large file:
`decode/Madhushala Pro.exe/Madhushala Pro.decompiled.cs` (~1M lines). Use grep:

```bash
cd "decode/Madhushala Pro.exe"
grep -n "class frm_main"        Madhushala*.cs
grep -n "tvMenuClick"           Madhushala*.cs
grep -n "ValidatePasswordAndLogin" Madhushala*.cs
```

Key symbols (line numbers shift between builds):

| Symbol | Purpose |
| --- | --- |
| `frm_main` | main shell; owns the `tv_menu` TreeView |
| `tvMenuClick` | dispatches a menu node to a form or report |
| `frmload` / `frm_Form1.Setcount` | opens the generic entry form in a mode |
| `frmLoginNew` / `ValidatePasswordAndLogin` | login and PIN check |
| `frm_login` | older login form |
| `frm_countersales` | POS billing |
| `frm_purchase` | purchase entry |

---

## 3. Decompiling with Docker (reproducible toolchain)

`dev/docker-compose.yml` builds the entire toolchain into one image and runs the
same scripts with the repository mounted at `/work`. Use this when you do not
want to install .NET/Mono on the host, or you want a pinned, repeatable setup.

### 3.1 What the image contains

| Tool | Version | Purpose |
| --- | --- | --- |
| .NET SDK | 8.0 | runs `ilspycmd` |
| `ilspycmd` | 8.2.0.7535 | .NET decompiler (matches the committed `decode/`) |
| Mono / `monodis` | 6.8 | disassembly fallback |
| Python 3 | 3.11 | `dev/scripts/mdf_users.py` |
| git + git-lfs | — | fetch the LFS-stored main EXE |
| `file`, `unzip` | — | inspection helpers |

### 3.2 Prerequisites

The Docker daemon must be running. In a container environment:

```bash
sudo dockerd > /tmp/docker.log 2>&1 &
```

`ilspycmd` 8.2 targets .NET 6; the image sets `DOTNET_ROLL_FORWARD=LatestMajor`
so it runs on the .NET 8 runtime. The tool is installed into `/usr/local/bin`,
so it is on `PATH` in every shell.

### 3.3 Decompile (one-shot)

From the repository root:

```bash
docker compose -f dev/docker-compose.yml run --rm decompiler
```

This runs `git lfs pull` (best effort) then `dev/scripts/decompile.sh`, writing
`decode/<assembly>/<assembly>.decompiled.cs` on the host.

### 3.4 Interactive shell

```bash
docker compose -f dev/docker-compose.yml run --rm shell
# inside:
ilspycmd --version
dev/scripts/decompile.sh
dev/scripts/admin_pin.sh find
monodis "Debug/NumberToWord.dll" | head
```

### 3.5 Optional: SQL Server for end-to-end verification

A SQL Server 2022 service (profile `sql`) is provided to attach the shipped
`.MDF` and confirm a reset with the engine itself:

```bash
MSSQL_SA_PASSWORD='Str0ng!Passw0rd' \
  docker compose -f dev/docker-compose.yml --profile sql up -d mssql
```

`Debug/Resources/` is mounted read-only at `/resources`. Attach a copy and check
it (adjust the `FILENAME` paths to the copied files):

```bash
docker exec <mssql-container> /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P 'Str0ng!Passw0rd' -C \
  -Q "CREATE DATABASE barmanager ON
        (FILENAME='/var/opt/mssql/data/barmanager.mdf'),
        (FILENAME='/var/opt/mssql/data/barmanager_log.ldf') FOR ATTACH"

docker exec <mssql-container> /opt/mssql-tools18/bin/sqlcmd \
  -S localhost -U sa -P 'Str0ng!Passw0rd' -C -d barmanager \
  -Q "DBCC CHECKDB (barmanager) WITH NO_INFOMSGS;
      SELECT usercode,username,accesslevel,password FROM users WHERE usercode='A00001'"
```

Tear down with:

```bash
docker compose -f dev/docker-compose.yml --profile sql down
```

> `docker compose run` may not return in some sandboxed environments even
> though the command runs and prints output; the container can be stopped with
> `docker rm -f`. This is a compose-CLI quirk, not an issue with this file.

---

## 4. Finding the existing admin PIN

The login "PIN" is the plain-text `users.password` column. There is no hashing
and no separate numeric PIN column (the `pin` columns elsewhere belong to
address tables).

### 4.1 From a running / attached database (recommended)

```sql
SELECT usercode, username, accesslevel, password
FROM   users
WHERE  accesslevel = 'Admin';       -- admin row is usercode = 'A00001'
```

Or list every user:

```sql
SELECT usercode, username, accesslevel, password FROM users;
```

### 4.2 Offline, straight from the .MDF file

When SQL Server is not running (or you cannot attach), read the PIN directly:

```bash
dev/scripts/admin_pin.sh find
# or point at specific files:
dev/scripts/admin_pin.sh find Debug/Resources/barmanager.mdf
```

Example output:

```
== Debug/Resources/barmanager.mdf
   page 267: usercode='A00001' username='Admin' accesslevel='Admin' password='12345'
   page checksum valid: True
```

The underlying tool is `dev/scripts/mdf_users.py`, which locates the `A00001`
row in the data page, decodes its variable-length columns
(`usercode`, `username`, `accesslevel`, `password`) and prints them.

### 4.3 How the offline reader works

1. A SQL Server page is 8192 bytes; the 96-byte header is followed by the row
   data, and the slot array sits at the end of the page.
2. The four variable columns are stored contiguously as
   `usercode(6) + username + accesslevel + password`.
3. The reader finds the `A00001` marker, reads the little-endian end-offsets
   that precede the row's data region, and slices each column from them. This
   means it works for any username/accesslevel/password length.

---

## 5. Resetting the admin PIN

Pick whichever method matches your situation.

### 5.1 Attached database (simplest, safest) — SQL

Use `dev/sql/reset_admin_pin.sql` (SQL Server maintains the page checksums):

```bash
sqlcmd -S <server> -U <user> -P <password> -d barmanager -i dev/sql/reset_admin_pin.sql
```

Or run the statements directly:

```sql
UPDATE users SET password = '12345' WHERE usercode = 'A00001';
```

Any PIN length works here.

### 5.2 Offline .MDF edit (no SQL Server needed)

```bash
# same length as the current PIN (shipped PINs are 5 chars)
dev/scripts/admin_pin.sh reset 12345
# specific files:
dev/scripts/admin_pin.sh reset 12345 Debug/Resources/barmanager.mdf
```

The script:
1. copies the file to `<name>.bak.<timestamp>` first,
2. rewrites the `password` bytes in place,
3. recomputes the page checksum at offset 60,
4. re-reads the file and asserts the checksum is valid.

Output:

```
barmanager.mdf: PIN set to 12345, checksum ok
```

Because the column offsets are stored as end-offsets inside the row, the
offline method requires the new PIN to be the **same length** as the old one.
For a different length, attach the database and use the SQL method (5.1).

### 5.3 Log in

After resetting, start Madhushala Pro and log in as:

- User: **Admin** (usercode `A00001`)
- PIN: the value you set

---

## 6. The MDF page checksum (why the offline edit works)

SQL Server stores a 32-bit checksum at byte offset 60 of every 8192-byte page.
Editing page bytes without recomputing it makes the file look corrupt — the
engine refuses to attach it, or `DBCC CHECKDB` reports errors. The algorithm
used by these MDFs is the classic XOR variant:

```
PAGE_SIZE = 8192, SECTORS = 16, ELEMENTS = 128, SEED = 15

checksum(page):
    dwords = 2048 little-endian uint32 words of the page
    acc = XOR of dwords[0..127] with index 15 skipped   # index 15 = the checksum slot
    ck  = rotate_left(acc, 15)
    for s in 1..15:
        acc = XOR of dwords[s*128 .. s*128+127]
        ck ^= rotate_left(acc, 15 - s)
    return ck
```

This is implemented in `dev/scripts/mdf_users.py` (`page_checksum`). It was
validated against every allocated page in the shipped databases (all match
except two empty pages whose stored checksum is 0).

> After an offline edit, always confirm with the engine when possible:
> `DBCC CHECKDB (barmanager) WITH NO_INFOMSGS;`

---

## 7. Verification checklist

Offline edit:
- [ ] `dev/scripts/admin_pin.sh find` shows the new PIN.
- [ ] `page checksum valid: True`.
- [ ] If SQL Server is available: attach, `DBCC CHECKDB` clean,
      `SELECT password FROM users WHERE usercode='A00001'` returns the new PIN.
- [ ] Log in as `Admin` with the new PIN.

Decompilation:
- [ ] `git lfs pull` produced the full-size `Debug/Madhushala Pro.exe`.
- [ ] `dev/scripts/decompile.sh` regenerated `decode/<assembly>/…decompiled.cs`.
- [ ] `grep -n "class frm_main" "decode/Madhushala Pro.exe/"*.cs` finds the shell.

---

## 8. Troubleshooting

| Symptom | Cause / fix |
| --- | --- |
| `ilspycmd: command not found` | add `$HOME/.dotnet/tools` to `PATH`, or `dotnet tool install -g ilspycmd` |
| `Debug/Madhushala Pro.exe` is 134 bytes | LFS pointer — run `git lfs pull` |
| `ilspycmd` emits unresolved types | pass `-r Debug` so sibling DLLs resolve |
| Offline reset says length mismatch | use the SQL method (5.1) for a different-length PIN |
| Database won't attach after an offline edit | checksum not updated; restore the `.bak` copy and re-run `admin_pin.sh reset` |
| `admin_pin.sh find` finds no user rows | wrong file, or a build whose row layout differs — inspect with `mdf_users.py` or use the SQL method |
| Docker: `Cannot connect to the Docker daemon` | start it: `sudo dockerd > /tmp/docker.log 2>&1 &` |
| Docker: `ilspycmd` exits with "Framework ... 6.0.0 not found" | ensure `DOTNET_ROLL_FORWARD=LatestMajor` (set in the image) |
| Docker: `docker compose run` never returns | sandbox compose-CLI quirk; the task has run — stop it with `docker rm -f <container>` |
