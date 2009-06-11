ALTER TABLE [dbo].[netsqlazman_AuthorizationsTable] ADD CONSTRAINT [CK_AuthorizationTypeCheck] CHECK (([AuthorizationType] >= 0 and [AuthorizationType] <= 3))


