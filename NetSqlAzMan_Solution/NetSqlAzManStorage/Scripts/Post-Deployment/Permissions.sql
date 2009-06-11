
GRANT SELECT ON  [dbo].[netsqlazman_ApplicationAttributes] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[netsqlazman_ApplicationGroupMembers] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[netsqlazman_ApplicationGroups] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[netsqlazman_ApplicationPermissions] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[netsqlazman_Applications] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[netsqlazman_AuthorizationAttributes] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[netsqlazman_Authorizations] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[netsqlazman_BizRules] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_CheckApplicationPermissions] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_CheckStorePermissions] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[netsqlazman_GetDBUsers] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_IAmAdmin] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[netsqlazman_ItemAttributes] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[netsqlazman_Items] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[netsqlazman_ItemsHierarchy] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON  [dbo].[NetSqlAzMan_DBVersion] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[netsqlazman_StoreAttributes] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[netsqlazman_StoreGroupMembers] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[netsqlazman_StoreGroups] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[netsqlazman_StorePermissions] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[netsqlazman_Stores] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[netsqlazman_ApplicationPermissionsTable] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[netsqlazman_LogTable] TO [NetSqlAzMan_Readers]
GO
GRANT INSERT ON  [dbo].[netsqlazman_LogTable] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[netsqlazman_Settings] TO [NetSqlAzMan_Readers]
GO
GRANT INSERT ON  [dbo].[netsqlazman_Settings] TO [NetSqlAzMan_Administrators]
GO
GRANT DELETE ON  [dbo].[netsqlazman_Settings] TO [NetSqlAzMan_Administrators]
GO
GRANT UPDATE ON  [dbo].[netsqlazman_Settings] TO [NetSqlAzMan_Administrators]
GO
GRANT SELECT ON  [dbo].[netsqlazman_StorePermissionsTable] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[UsersDemo] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[netsqlazman_ApplicationAttributesView] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[netsqlazman_ApplicationGroupMembersView] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[netsqlazman_ApplicationsView] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[netsqlazman_AuthorizationAttributesView] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[netsqlazman_AuthorizationView] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[netsqlazman_BizRuleView] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[netsqlazman_DatabaseUsers] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[netsqlazman_ItemAttributesView] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[netsqlazman_ItemsHierarchyView] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[netsqlazman_StoreAttributesView] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON  [dbo].[netsqlazman_StoreGroupMembersView] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_ApplicationAttributeDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_ApplicationAttributeInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_ApplicationAttributeUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_ApplicationDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_ApplicationGroupDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_ApplicationGroupInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_ApplicationGroupMemberDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_ApplicationGroupMemberInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_ApplicationGroupMemberUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_ApplicationGroupUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_ApplicationInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_ApplicationPermissionDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_ApplicationPermissionInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_ApplicationUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_AuthorizationAttributeDelete] TO [NetSqlAzMan_Users]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_AuthorizationAttributeInsert] TO [NetSqlAzMan_Users]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_AuthorizationAttributeUpdate] TO [NetSqlAzMan_Users]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_AuthorizationDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_AuthorizationInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_AuthorizationUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_BizRuleDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_BizRuleInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_BizRuleUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_BuildUserPermissionCache] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_ClearBizRule] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_CreateDelegate] TO [NetSqlAzMan_Users]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_DeleteDelegate] TO [NetSqlAzMan_Users]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_DirectCheckAccess] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_GrantApplicationAccess] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_GrantStoreAccess] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_helplogins] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_IsAMemberOfGroup] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_ItemAttributeDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_ItemAttributeInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_ItemAttributeUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_ItemDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_ItemInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_ItemsHierarchyDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_ItemsHierarchyInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_ItemUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_ReloadBizRule] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_RevokeApplicationAccess] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_RevokeStoreAccess] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_StoreAttributeDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_StoreAttributeInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_StoreAttributeUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_StoreGroupDelete] TO [NetSqlAzMan_Administrators]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_StoreGroupInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_StoreGroupMemberDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_StoreGroupMemberInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_StoreGroupMemberUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_StoreGroupUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_StoreInsert] TO [NetSqlAzMan_Administrators]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_StorePermissionDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_StorePermissionInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON  [dbo].[netsqlazman_StoreUpdate] TO [NetSqlAzMan_Managers]
GO

/* ********************** */
/* ONLY FOR SQL 2005/2008 */
/* ********************** */
IF CHARINDEX('Microsoft SQL Server 2005', REPLACE(@@VERSION,'  ', ' '))=1 
   OR 
   CHARINDEX('Microsoft SQL Server 2008', REPLACE(@@VERSION,'  ', ' '))=1
   BEGIN
        EXEC sp_executesql N'GRANT VIEW DEFINITION TO [NetSqlAzMan_Readers]' -- ALLOW NetSqlAzMan_Readers TO SEE ALL OTHER Logins 
-- http://www.microsoft.com/technet/technetmag/issues/2006/01/ProtectMetaData/?topics=y
END
/* ********************** */
