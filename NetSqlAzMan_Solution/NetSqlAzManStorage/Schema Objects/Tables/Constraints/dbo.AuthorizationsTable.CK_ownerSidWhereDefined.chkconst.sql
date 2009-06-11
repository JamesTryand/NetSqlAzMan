ALTER TABLE [dbo].[netsqlazman_AuthorizationsTable] ADD CONSTRAINT [CK_ownerSidWhereDefined] CHECK (([ownerSidWhereDefined] >= 2 and [ownerSidWhereDefined] <= 4))


