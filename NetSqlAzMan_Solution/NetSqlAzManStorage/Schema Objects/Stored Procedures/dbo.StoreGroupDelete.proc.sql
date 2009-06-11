CREATE PROCEDURE [dbo].[netsqlazman_StoreGroupDelete]
(
	@Original_StoreGroupId int,
	@StoreId int
)
AS
IF dbo.[netsqlazman_CheckStorePermissions](@StoreId, 2) = 1
	DELETE FROM [dbo].[netsqlazman_StoreGroupsTable] WHERE [StoreGroupId] = @Original_StoreGroupId AND [StoreId] = @StoreId
ELSE
	RAISERROR ('Store permission denied.', 16, 1)


