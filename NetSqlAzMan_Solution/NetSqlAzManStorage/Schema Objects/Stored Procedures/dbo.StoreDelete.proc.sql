CREATE PROCEDURE [dbo].[netsqlazman_StoreDelete]
(
	@Original_StoreId int
)
AS
IF EXISTS(Select StoreId FROM dbo.[netsqlazman_Stores]() WHERE StoreId = @Original_StoreId) AND dbo.[netsqlazman_CheckStorePermissions](@Original_StoreId, 2) = 1
	DELETE FROM [dbo].[netsqlazman_StoresTable] WHERE [StoreId] = @Original_StoreId
ELSE
	RAISERROR ('Store permission denied.', 16, 1)


