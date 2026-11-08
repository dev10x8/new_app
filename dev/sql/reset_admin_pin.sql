-- reset_admin_pin.sql
--
-- Reset the Madhushala Pro admin login PIN when the database is (or can be)
-- attached to a running SQL Server instance. This is the simplest and safest
-- method: SQL Server maintains the page checksums for you.
--
-- The login "PIN" is the plain-text `users.password` value. The admin account
-- is usercode = 'A00001', accesslevel = 'Admin'.
--
-- Usage (sqlcmd):
--   sqlcmd -S <server> -U <user> -P <password> -d barmanager -i reset_admin_pin.sql
-- or paste into SSMS and run against the `barmanager` database.
--
-- Change the value of @NewPin below, then run the whole script.

USE barmanager;
GO

DECLARE @NewPin      varchar(50) = '12345';
DECLARE @UserCode    varchar(6)  = 'A00001';

-- 1) Show the current admin account and PIN before changing anything.
SELECT usercode, username, accesslevel, password
FROM   users
WHERE  usercode = @UserCode;

-- 2) Reset the PIN.
UPDATE users
SET    password = @NewPin
WHERE  usercode = @UserCode;

-- 3) Verify.
SELECT usercode, username, accesslevel, password
FROM   users
WHERE  usercode = @UserCode;

-- 4) (Optional) Confirm the database is structurally healthy.
DBCC CHECKDB (barmanager) WITH NO_INFOMSGS;
GO
