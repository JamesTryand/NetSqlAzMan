CREATE PROCEDURE [dbo].[netsqlazman_ItemAttributeUpdate]
(
	@AttributeKey nvarchar(255),
	@AttributeValue nvarchar(4000),
	@Original_ItemAttributeId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ItemAttributeId FROM dbo.[netsqlazman_ItemAttributes]() WHERE ItemAttributeId = @Original_ItemAttributeId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1
	UPDATE [dbo].[netsqlazman_ItemAttributesTable] SET [AttributeKey] = @AttributeKey, [AttributeValue] = @AttributeValue WHERE [ItemAttributeId] = @Original_ItemAttributeId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)


