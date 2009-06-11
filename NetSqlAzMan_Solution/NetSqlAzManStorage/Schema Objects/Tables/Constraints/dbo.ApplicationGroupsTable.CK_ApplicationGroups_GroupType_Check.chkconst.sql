ALTER TABLE [dbo].[netsqlazman_ApplicationGroupsTable] ADD CONSTRAINT [CK_ApplicationGroups_GroupType_Check] CHECK (([GroupType] >= 0 and [GroupType] <= 1))


