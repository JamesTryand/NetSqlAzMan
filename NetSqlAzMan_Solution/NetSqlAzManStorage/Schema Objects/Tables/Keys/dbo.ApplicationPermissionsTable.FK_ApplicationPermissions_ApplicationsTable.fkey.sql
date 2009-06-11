ALTER TABLE [dbo].[netsqlazman_ApplicationPermissionsTable] ADD
CONSTRAINT [FK_ApplicationPermissions_ApplicationsTable] FOREIGN KEY ([ApplicationId]) REFERENCES [dbo].[netsqlazman_ApplicationsTable] ([ApplicationId]) ON DELETE CASCADE


