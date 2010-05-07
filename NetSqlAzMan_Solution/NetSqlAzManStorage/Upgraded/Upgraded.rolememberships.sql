--EXEC sp_addrolemember N'NetSqlAzMan_Administrators', N'BUILTIN\Administrators'
--GO
--EXEC sp_addrolemember N'NetSqlAzMan_Managers', N'BUILTIN\Administrators'
--GO
EXEC sp_addrolemember N'NetSqlAzMan_Managers', N'NetSqlAzMan_Administrators'
GO
--EXEC sp_addrolemember N'NetSqlAzMan_Users', N'BUILTIN\Administrators'
--GO
--EXEC sp_addrolemember N'NetSqlAzMan_Users', N'NetSqlAzMan_Administrators'
--GO
EXEC sp_addrolemember N'NetSqlAzMan_Users', N'NetSqlAzMan_Managers'
GO
--EXEC sp_addrolemember N'NetSqlAzMan_Readers', N'BUILTIN\Administrators'
--GO
EXEC sp_addrolemember N'NetSqlAzMan_Readers', N'NetSqlAzMan_Administrators'
GO
EXEC sp_addrolemember N'NetSqlAzMan_Readers', N'NetSqlAzMan_Managers'
GO
EXEC sp_addrolemember N'NetSqlAzMan_Readers', N'NetSqlAzMan_Users'