ALTER TABLE [dbo].[netsqlazman_StoreAttributesTable] ADD
CONSTRAINT [FK_StoreAttributes_Stores] FOREIGN KEY ([StoreId]) REFERENCES [dbo].[netsqlazman_StoresTable] ([StoreId]) ON DELETE CASCADE


