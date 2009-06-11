ALTER TABLE [dbo].[netsqlazman_ApplicationsTable] ADD
CONSTRAINT [FK_Applications_Stores] FOREIGN KEY ([StoreId]) REFERENCES [dbo].[netsqlazman_StoresTable] ([StoreId]) ON DELETE CASCADE


