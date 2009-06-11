ALTER TABLE [dbo].[netsqlazman_AuthorizationsTable] ADD
CONSTRAINT [FK_Authorizations_Items] FOREIGN KEY ([ItemId]) REFERENCES [dbo].[netsqlazman_ItemsTable] ([ItemId]) ON DELETE CASCADE


