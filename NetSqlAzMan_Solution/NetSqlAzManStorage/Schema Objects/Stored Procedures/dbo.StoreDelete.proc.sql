CREATE PROCEDURE [dbo].[netsqlazman_StoreDelete]
(
	@Original_StoreId int
)
AS
IF EXISTS(Select StoreId FROM dbo.Stores() WHERE StoreId = @Original_StoreId) AND dbo.CheckStorePermissions(@Original_StoreId, 2) = 1
	DELETE FROM [dbo].[netsqlazman_StoresTable] WHERE [StoreId] = @Original_StoreId
ELSE
	RAISERROR ('Store permission denied.', 16, 1)


