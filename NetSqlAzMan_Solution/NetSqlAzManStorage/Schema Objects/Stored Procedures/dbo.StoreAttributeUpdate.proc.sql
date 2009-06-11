CREATE PROCEDURE [dbo].[netsqlazman_StoreAttributeUpdate]
(
	@StoreId int,
	@AttributeKey nvarchar(255),
	@AttributeValue nvarchar(4000),
	@Original_StoreAttributeId int
)
AS
IF EXISTS(Select StoreAttributeId FROM dbo.StoreAttributes() WHERE StoreAttributeId = @Original_StoreAttributeId) AND dbo.CheckStorePermissions(@StoreId, 2) = 1
	UPDATE [dbo].[netsqlazman_StoreAttributesTable] SET [AttributeKey] = @AttributeKey, [AttributeValue] = @AttributeValue WHERE [StoreAttributeId] = @Original_StoreAttributeId AND [StoreId] = @StoreId 
ELSE
	RAISERROR ('Store permission denied.', 16, 1)


