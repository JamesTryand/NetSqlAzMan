CREATE PROCEDURE [dbo].[netsqlazman_StoreUpdate]
(
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@Original_StoreId int
)
AS
IF EXISTS(Select StoreId FROM dbo.[netsqlazman_Stores]() WHERE StoreId = @Original_StoreId) AND dbo.[netsqlazman_CheckStorePermissions](@Original_StoreId, 2) = 1
	UPDATE [dbo].[netsqlazman_StoresTable] SET [Name] = @Name, [Description] = @Description WHERE [StoreId] = @Original_StoreId
ELSE
	RAISERROR ('Store permission denied.', 16, 1)


