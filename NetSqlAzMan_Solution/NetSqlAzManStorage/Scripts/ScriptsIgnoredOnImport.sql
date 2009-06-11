
IF NOT EXISTS (SELECT * FROM master.dbo.syslogins WHERE loginname = N'BUILTIN\Administrators')
exec sp_grantlogin N'BUILTIN\Administrators'

GO
