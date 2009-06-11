ALTER TABLE [dbo].[netsqlazman_ApplicationGroupsTable] ADD
CONSTRAINT [FK_ApplicationGroups_Applications] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[netsqlazman_ApplicationsTable] ([ApplicationId]) ON DELETE CASCADE


