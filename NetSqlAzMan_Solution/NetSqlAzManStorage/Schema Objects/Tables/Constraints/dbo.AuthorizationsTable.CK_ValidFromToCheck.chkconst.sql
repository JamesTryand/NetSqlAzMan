ALTER TABLE [dbo].[netsqlazman_AuthorizationsTable] ADD CONSTRAINT [CK_ValidFromToCheck] CHECK (([ValidFrom] is null or [ValidTo] is null or [ValidFrom] <= [ValidTo]))


