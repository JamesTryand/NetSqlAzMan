ALTER TABLE [dbo].[netsqlazman_ApplicationAttributesTable] ADD
CONSTRAINT [FK_ApplicationAttributes_Applications] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[netsqlazman_ApplicationsTable] ([ApplicationId]) ON DELETE CASCADE


