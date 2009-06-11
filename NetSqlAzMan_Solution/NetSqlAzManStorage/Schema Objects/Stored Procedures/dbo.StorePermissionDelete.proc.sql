CREATE PROCEDURE [dbo].[netsqlazman_StorePermissionDelete]
(
	@StorePermissionId int,
	@StoreId int
)
AS
IF EXISTS(SELECT StoreId FROM dbo.Stores() WHERE StoreId = @StoreId) AND dbo.CheckStorePermissions(@StoreId, 2) = 1
	DELETE FROM dbo.[netsqlazman_StorePermissionsTable] WHERE StorePermissionId = @StorePermissionId AND StoreId = @StoreId
ELSE
	RAISERROR ('Store permission denied.', 16, 1)


