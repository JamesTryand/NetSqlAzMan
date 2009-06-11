ALTER TABLE [dbo].[netsqlazman_ItemAttributesTable] ADD
CONSTRAINT [FK_ItemAttributes_Items] FOREIGN KEY ([ItemId]) REFERENCES [dbo].[netsqlazman_ItemsTable] ([ItemId]) ON DELETE CASCADE


