CREATE PROCEDURE [dbo].[netsqlazman_RevokeStoreAccess] (
	@StoreId int,
	@SqlUserOrRole sysname,
	@NetSqlAzManFixedServerRole tinyint)
AS
IF EXISTS(SELECT StoreId FROM dbo.[netsqlazman_StoresTable] WHERE StoreId = @StoreId) AND (dbo.[netsqlazman_CheckStorePermissions](@StoreId, 2) = 1 AND @NetSqlAzManFixedServerRole BETWEEN 0 AND 1 OR (IS_MEMBER('db_owner')=1 OR IS_MEMBER('NetSqlAzMan_Administrators')=1) AND @NetSqlAzManFixedServerRole = 2)
BEGIN
	IF EXISTS(SELECT * FROM dbo.[netsqlazman_StorePermissionsTable] WHERE StoreId = @StoreId AND SqlUserOrRole = @SqlUserOrRole AND NetSqlAzManFixedServerRole = @NetSqlAzManFixedServerRole)
		DELETE FROM dbo.[netsqlazman_StorePermissionsTable] WHERE StoreId = @StoreId AND SqlUserOrRole = @SqlUserOrRole AND NetSqlAzManFixedServerRole = @NetSqlAzManFixedServerRole
	ELSE
		RAISERROR ('Permission not found. Revoke Store Access ignored.', -1, -1)
END
ELSE
	RAISERROR ('Store NOT Found or Store permission denied.', 16, 1)


