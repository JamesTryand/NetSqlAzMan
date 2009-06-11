
GRANT SELECT ON  [dbo].[ApplicationAttributes] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[ApplicationGroupMembers] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[ApplicationGroups] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[ApplicationPermissions] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[Applications] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[AuthorizationAttributes] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[Authorizations] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[BizRules] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON  [dbo].[CheckApplicationPermissions] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON  [dbo].[CheckStorePermissions] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[GetDBUsers] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON  [dbo].[IAmAdmin] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[ItemAttributes] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[Items] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[ItemsHierarchy] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON  [dbo].[NetSqlAzMan_DBVersion] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[StoreAttributes] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[StoreGroupMembers] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[StoreGroups] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[StorePermissions] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[Stores] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[ApplicationPermissionsTable] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[LogTable] TO [NetSqlAzMan_Readers]
GO
GRANT INSERT ON  [dbo].[LogTable] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[Settings] TO [NetSqlAzMan_Readers]
GO
GRANT INSERT ON  [dbo].[Settings] TO [NetSqlAzMan_Administrators]
GO
GRANT DELETE ON  [dbo].[Settings] TO [NetSqlAzMan_Administrators]
GO
GRANT UPDATE ON  [dbo].[Settings] TO [NetSqlAzMan_Administrators]
GO
GRANT SELECT ON  [dbo].[StorePermissionsTable] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[UsersDemo] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[ApplicationAttributesView] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[ApplicationGroupMembersView] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[ApplicationsView] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[AuthorizationAttributesView] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[AuthorizationView] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[BizRuleView] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[DatabaseUsers] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[ItemAttributesView] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[ItemsHierarchyView] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[StoreAttributesView] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[StoreGroupMembersView] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON  [dbo].[ApplicationAttributeDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[ApplicationAttributeInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[ApplicationAttributeUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[ApplicationDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[ApplicationGroupDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[ApplicationGroupInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[ApplicationGroupMemberDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[ApplicationGroupMemberInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[ApplicationGroupMemberUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[ApplicationGroupUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[ApplicationInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[ApplicationPermissionDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[ApplicationPermissionInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[ApplicationUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[AuthorizationAttributeDelete] TO [NetSqlAzMan_Users]
GO
GRANT EXECUTE ON  [dbo].[AuthorizationAttributeInsert] TO [NetSqlAzMan_Users]
GO
GRANT EXECUTE ON  [dbo].[AuthorizationAttributeUpdate] TO [NetSqlAzMan_Users]
GO
GRANT EXECUTE ON  [dbo].[AuthorizationDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[AuthorizationInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[AuthorizationUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[BizRuleDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[BizRuleInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[BizRuleUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[BuildUserPermissionCache] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON  [dbo].[ClearBizRule] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[CreateDelegate] TO [NetSqlAzMan_Users]
GO
GRANT EXECUTE ON  [dbo].[DeleteDelegate] TO [NetSqlAzMan_Users]
GO
GRANT EXECUTE ON  [dbo].[DirectCheckAccess] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON  [dbo].[GrantApplicationAccess] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[GrantStoreAccess] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[helplogins] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON  [dbo].[IsAMemberOfGroup] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON  [dbo].[ItemAttributeDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[ItemAttributeInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[ItemAttributeUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[ItemDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[ItemInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[ItemsHierarchyDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[ItemsHierarchyInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[ItemUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[ReloadBizRule] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[RevokeApplicationAccess] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[RevokeStoreAccess] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[StoreAttributeDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[StoreAttributeInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[StoreAttributeUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[StoreGroupDelete] TO [NetSqlAzMan_Administrators]
GO
GRANT EXECUTE ON  [dbo].[StoreGroupInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[StoreGroupMemberDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[StoreGroupMemberInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[StoreGroupMemberUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[StoreGroupUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[StoreInsert] TO [NetSqlAzMan_Administrators]
GO
GRANT EXECUTE ON  [dbo].[StorePermissionDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[StorePermissionInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[StoreUpdate] TO [NetSqlAzMan_Managers]

GO
