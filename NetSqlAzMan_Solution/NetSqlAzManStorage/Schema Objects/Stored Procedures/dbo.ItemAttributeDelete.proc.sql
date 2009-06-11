CREATE PROCEDURE [dbo].[netsqlazman_ItemAttributeDelete]
(
	@ItemAttributeId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ItemAttributeId FROM dbo.ItemAttributes() WHERE ItemAttributeId = @ItemAttributeId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
	DELETE FROM [dbo].[netsqlazman_ItemAttributesTable] WHERE [ItemAttributeId] = @ItemAttributeId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)


