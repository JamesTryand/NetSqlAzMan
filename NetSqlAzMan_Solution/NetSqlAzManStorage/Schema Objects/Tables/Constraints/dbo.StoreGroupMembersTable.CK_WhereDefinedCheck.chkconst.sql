ALTER TABLE [dbo].[netsqlazman_StoreGroupMembersTable] ADD CONSTRAINT [CK_WhereDefinedCheck] CHECK (([WhereDefined] = 0 or [WhereDefined] >= 2 and [WhereDefined] <= 4))


