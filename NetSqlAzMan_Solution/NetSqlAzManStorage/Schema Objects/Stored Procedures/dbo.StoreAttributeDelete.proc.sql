CREATE PROCEDURE [dbo].[netsqlazman_StoreAttributeDelete]
(
	@StoreId int,
	@StoreAttributeId int
)
AS
IF EXISTS(Select StoreAttributeId FROM dbo.[netsqlazman_StoreAttributes]() WHERE StoreAttributeId = @StoreAttributeId) AND dbo.[netsqlazman_CheckStorePermissions](@StoreId, 2) = 1
	DELETE FROM [dbo].[netsqlazman_StoreAttributesTable] WHERE [StoreAttributeId] = @StoreAttributeId AND [StoreId] = @StoreId
ELSE
	RAISERROR ('Store permission denied.', 16, 1)


