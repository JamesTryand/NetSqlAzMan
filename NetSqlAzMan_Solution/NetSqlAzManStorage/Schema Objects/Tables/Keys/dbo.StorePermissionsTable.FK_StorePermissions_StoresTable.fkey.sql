ALTER TABLE [dbo].[netsqlazman_StorePermissionsTable] ADD
CONSTRAINT [FK_StorePermissions_StoresTable] FOREIGN KEY ([StoreId]) REFERENCES [dbo].[netsqlazman_StoresTable] ([StoreId]) ON DELETE CASCADE


