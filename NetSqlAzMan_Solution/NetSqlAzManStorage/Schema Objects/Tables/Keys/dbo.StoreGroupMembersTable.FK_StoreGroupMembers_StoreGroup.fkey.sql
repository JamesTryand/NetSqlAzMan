ALTER TABLE [dbo].[netsqlazman_StoreGroupMembersTable] ADD
CONSTRAINT [FK_StoreGroupMembers_StoreGroup] FOREIGN KEY ([StoreGroupId]) REFERENCES [dbo].[netsqlazman_StoreGroupsTable] ([StoreGroupId]) ON DELETE CASCADE


