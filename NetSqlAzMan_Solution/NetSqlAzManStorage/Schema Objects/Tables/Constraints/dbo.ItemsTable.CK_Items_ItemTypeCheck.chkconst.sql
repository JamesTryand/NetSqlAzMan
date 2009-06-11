ALTER TABLE [dbo].[netsqlazman_ItemsTable] ADD CONSTRAINT [CK_Items_ItemTypeCheck] CHECK (([ItemType] >= 0 and [ItemType] <= 2))


