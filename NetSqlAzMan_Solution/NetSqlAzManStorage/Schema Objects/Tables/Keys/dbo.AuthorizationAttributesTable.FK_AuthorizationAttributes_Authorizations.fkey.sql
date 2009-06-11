ALTER TABLE [dbo].[netsqlazman_AuthorizationAttributesTable] ADD
CONSTRAINT [FK_AuthorizationAttributes_Authorizations] FOREIGN KEY ([AuthorizationId]) REFERENCES [dbo].[netsqlazman_AuthorizationsTable] ([AuthorizationId]) ON DELETE CASCADE


