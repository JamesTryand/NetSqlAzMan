ALTER TABLE [dbo].[netsqlazman_StoreGroupsTable] ADD
CONSTRAINT [FK_StoreGroups_Stores] FOREIGN KEY ([StoreId]) REFERENCES [dbo].[netsqlazman_StoresTable] ([StoreId]) ON DELETE CASCADE


