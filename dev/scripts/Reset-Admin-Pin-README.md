# Madhushala Pro - find or reset the Admin PIN (Windows)

Companion notes for `Reset-MadhushalaAdminPin.ps1`.

## What the "PIN" actually is

In Madhushala Pro the login PIN is the plain-text `password` column of the
`users` table. The administrator is:

```
usercode = A00001    username = Admin    accesslevel = Admin
```

The login form runs this query and grants admin rights when the row's
`username` is `Admin`:

```sql
select * from users where usercode='A00001' and password='<typed PIN>'
```

So the value to type in the Password box is exactly `users.password`.
The app ships with the PIN `admin` (5 characters).

## Where the data lives

The desktop app talks to a SQL Server (Express) database named `barmanager`.
The database file ships next to the app:

```
<app folder>\Debug\Resources\barmanager.mdf        <- live database
<app folder>\Debug\Resources\barmanager_demo.mdf   <- demo database
```

The SQL Server instance and credentials are in `Debug\Madhushala Pro.exe.config`
(`server_name`, `db`, `User`, `Password`). The instance name may also be in
`Debug\ServerName.txt` (for example `DESKTOP-XXXXXXX\SQLEXPRESS`).

## Requirements

- Windows PowerShell 5.1 (built in) or PowerShell 7+.
- Read/write access to the `.mdf` (run PowerShell as Administrator if the app is
  under `Program Files`).
- For the online method only: access to the SQL Server instance.

## Method 1 - offline (works even when SQL Server is stopped)

Best when the app is closed. It edits the `.mdf` directly and fixes the SQL
Server page checksum so the file stays valid.

1. Close Madhushala Pro (and SQL Server Express, if it is running) so the
   `.mdf` is not locked.
2. Open PowerShell and go to the folder that holds the script, for example:

   ```powershell
   cd "C:\Madhushala Pro\dev\scripts"
   ```

3. If the shell blocks the script, allow it for this session:

   ```powershell
   Set-ExecutionPolicy -Scope Process -ExecutionPolicy Bypass
   ```

4. Find the current PIN:

   ```powershell
   .\Reset-MadhushalaAdminPin.ps1 -Action Find
   ```

   If the database is not found automatically, point at it:

   ```powershell
   .\Reset-MadhushalaAdminPin.ps1 -Action Find -Mdf "C:\Madhushala Pro\Debug\Resources\barmanager.mdf"
   ```

5. Reset the PIN (the new PIN must be the same length as the old one - the
   shipped PIN is 5 characters):

   ```powershell
   .\Reset-MadhushalaAdminPin.ps1 -Action Reset -NewPin 12345
   ```

   A timestamped backup (`.mdf.bak.<stamp>`) is written next to the file before
   it is changed. The script re-reads the file afterwards and confirms both the
   new PIN and a valid page checksum.

## Method 2 - online (when SQL Server is reachable)

Best when the database is attached and in use. It runs an `UPDATE` through SQL
Server, so the new PIN can be any length (up to 10 characters).

```powershell
.\Reset-MadhushalaAdminPin.ps1 -Action Reset -NewPin 9876 -Online -Server ".\SQLEXPRESS"
```

If SQL Server is not reachable the script says so and falls back to the offline
method automatically.

## Log in after the reset

1. Start `Madhushala Pro.exe` (or the desktop shortcut).
2. On the sign-in screen enter:

   | Field    | Value                     |
   | -------- | ------------------------- |
   | User ID  | `A00001`                  |
   | Password | the PIN you just set      |

3. Click the login / OK button. You are signed in as the administrator.

If the login is rejected, check that you used the same database the app uses
(`barmanager`), and that Madhushala Pro was fully closed before an offline
reset.

## Quick reference

| Task                    | Command                                                            |
| ----------------------- | ------------------------------------------------------------------ |
| Find the PIN            | `.\Reset-MadhushalaAdminPin.ps1 -Action Find`                       |
| Reset (same length)     | `.\Reset-MadhushalaAdminPin.ps1 -Action Reset -NewPin 12345`        |
| Reset via SQL Server    | `.\Reset-MadhushalaAdminPin.ps1 -Action Reset -NewPin 9876 -Online` |
| Target a specific file  | add `-Mdf "C:\...\barmanager.mdf"`                                  |
| Reset another user      | add `-UserCode A00002`                                              |
| Skip the backup copy    | add `-NoBackup`                                                     |

## Notes and limits

- Offline reset keeps the PIN the same length because the stored column width
  is fixed in the row; use the online method (or the SQL in
  `dev/sql/reset_admin_pin.sql`) to change the length.
- The PIN is stored in plain text, exactly as in the legacy application. This
  is a legacy design; hashing is recommended before any real deployment.
- Always keep the `.mdf` backup until you have logged in successfully.
