# Decoded (decompiled) sources — Madhushala Pro

Decompiled from the .NET assemblies under `Debug/` using `ilspycmd` (ILSpy 8.2).
Each subfolder holds the decompiled C# for one binary. These are reference
sources only; the running application uses the binaries in `Debug/`.

`Madhushala Pro.exe` is stored with Git LFS (see `.gitattributes`), so the
tracked file is a pointer. The decompilation below was produced from the full
181 MB binary.

## Contents

| Folder | Source binary | Notes |
| --- | --- | --- |
| `Madhushala Pro.exe/` | Madhushala Pro.exe | Main WinForms app (~1.0M lines of C#) |
| `ABMSQL.DataAccess.dll/` | ABMSQL.DataAccess.dll | Data-access layer |
| `MadhushalaDataSync.dll/` | MadhushalaDataSync.dll | Cloud sync |
| `NumberToWord.dll/` | NumberToWord.dll | Amount-to-words helper |
| `AutoDataSync.exe/` | AutoDataSync.exe | Sync helper |
| `Madhushala Downloader.exe/` | Madhushala Downloader.exe | Updater/downloader |
| `Downloader Madhushala Pro.exe/` | Downloader Madhushala Pro.exe | Updater/downloader |
| `ExciseSalesUploadOld.exe/` | ExciseSalesUploadOld.exe | Excise upload |
| `ABMQueryScreen*.dll/` | ABMQueryScreen*.dll | Query screens (4 variants) |
| `LatestEXE/` | Madhushala Ultimate_{32Bit,64Bit,AnyCPU}.exe | Packaged release builds |
| `LatestEXEOld/` | Madhushala Ultimate_{32Bit,64Bit}.exe | Previous release builds |
| others | third-party libs | NPOI, iTextSharp, Newtonsoft, Selenium, BouncyCastle, etc. |

Not decompiled (non-managed): `chromedriver.exe`, the `*.vshost.exe` shims.

## Admin login / PIN mechanism

The "PIN" used on the login screen is the user's `password` in the database
`users` table. It is stored and compared as plain text (no hashing).

- Login form: `frmLoginNew` (`PerformLogin`, `ValidatePasswordAndLogin`) and the
  older `frm_login`.
- Validation: `select usercode,username,accesslevel,password from users` then
  `enteredPassword == password`. If `accesslevel == "Admin"` the session is
  marked admin.
- Admin row: `usercode=A00001`, `username=Admin`, `accesslevel=Admin`,
  `password=admin`.
- There is no separate numeric PIN column anywhere in the schema; the `pin`
  columns that exist belong to address tables (`customerDetails`,
  `MasterAccountsLedger`, `companymast`).

### Database files

`Debug/Resources/barmanager.mdf` (live) and `barmanager_demo.mdf` (demo), plus
their `_log.ldf`. The app attaches `barmanager.mdf` from its startup path.

### Reset performed

The admin password was reset from `admin` to `12345` in both
`barmanager.mdf` and `barmanager_demo.mdf` by editing the single matching page
and recomputing the SQL Server page checksum (offset 60, XOR-based algorithm:
16 sectors x 128 dwords, rotate-left 15 for sector 0 skipping dword 15, then
rotate-left `15-s` XOR for each subsequent sector). Verified by attaching both
databases to SQL Server 2022: `DBCC CHECKDB` clean, and
`SELECT password FROM users` returns `12345`.
