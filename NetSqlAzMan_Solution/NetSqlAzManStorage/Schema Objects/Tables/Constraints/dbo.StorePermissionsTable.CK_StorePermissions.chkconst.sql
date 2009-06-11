ALTER TABLE [dbo].[netsqlazman_StorePermissionsTable] ADD CONSTRAINT [CK_StorePermissions] CHECK (([NetSqlAzManFixedServerRole] >= 0 and [NetSqlAzManFixedServerRole] <= 2))


