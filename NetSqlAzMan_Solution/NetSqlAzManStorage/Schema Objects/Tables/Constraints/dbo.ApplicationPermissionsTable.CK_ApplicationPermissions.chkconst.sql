ALTER TABLE [dbo].[netsqlazman_ApplicationPermissionsTable] ADD CONSTRAINT [CK_ApplicationPermissions] CHECK (([NetSqlAzManFixedServerRole] >= 0 and [NetSqlAzManFixedServerRole] <= 2))


