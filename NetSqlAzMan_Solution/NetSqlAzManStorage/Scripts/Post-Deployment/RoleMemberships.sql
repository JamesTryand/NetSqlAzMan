
-- Project upgrade has moved this code to 'Upgraded.rolememberships.sql'.
-- EXEC sp_addrolemember N'NetSqlAzMan_Administrators', N'BUILTIN\Administrators'

GO
-- Project upgrade has moved this code to 'Upgraded.rolememberships.sql'.
-- EXEC sp_addrolemember N'NetSqlAzMan_Managers', N'NetSqlAzMan_Administrators'

GO
-- Project upgrade has moved this code to 'Upgraded.rolememberships.sql'.
-- EXEC sp_addrolemember N'NetSqlAzMan_Managers', N'BUILTIN\Administrators'

GO
-- Project upgrade has moved this code to 'Upgraded.rolememberships.sql'.
-- EXEC sp_addrolemember N'NetSqlAzMan_Users', N'NetSqlAzMan_Managers'

GO
-- Project upgrade has moved this code to 'Upgraded.rolememberships.sql'.
-- EXEC sp_addrolemember N'NetSqlAzMan_Users', N'NetSqlAzMan_Administrators'

GO
-- Project upgrade has moved this code to 'Upgraded.rolememberships.sql'.
-- EXEC sp_addrolemember N'NetSqlAzMan_Users', N'BUILTIN\Administrators'

GO
-- Project upgrade has moved this code to 'Upgraded.rolememberships.sql'.
-- EXEC sp_addrolemember N'NetSqlAzMan_Readers', N'NetSqlAzMan_Users'

GO
-- Project upgrade has moved this code to 'Upgraded.rolememberships.sql'.
-- EXEC sp_addrolemember N'NetSqlAzMan_Readers', N'NetSqlAzMan_Managers'

GO
-- Project upgrade has moved this code to 'Upgraded.rolememberships.sql'.
-- EXEC sp_addrolemember N'NetSqlAzMan_Readers', N'NetSqlAzMan_Administrators'

GO
-- Project upgrade has moved this code to 'Upgraded.rolememberships.sql'.
-- EXEC sp_addrolemember N'NetSqlAzMan_Readers', N'BUILTIN\Administrators'

GO
