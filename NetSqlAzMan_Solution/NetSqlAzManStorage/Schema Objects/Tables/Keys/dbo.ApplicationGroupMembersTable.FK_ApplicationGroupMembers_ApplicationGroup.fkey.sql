ALTER TABLE [dbo].[netsqlazman_ApplicationGroupMembersTable] ADD
CONSTRAINT [FK_ApplicationGroupMembers_ApplicationGroup] FOREIGN KEY ([ApplicationGroupId]) REFERENCES [dbo].[netsqlazman_ApplicationGroupsTable] ([ApplicationGroupId]) ON DELETE CASCADE


