CREATE PROCEDURE [dbo].[netsqlazman_ItemAttributeInsert]
(
	@ItemId int,
	@AttributeKey nvarchar(255),
	@AttributeValue nvarchar(4000),
	@ApplicationId int
)
AS
IF EXISTS(SELECT ItemId FROM dbo.[netsqlazman_Items]() WHERE ItemId = @ItemId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1
BEGIN
	INSERT INTO [dbo].[netsqlazman_ItemAttributesTable] ([ItemId], [AttributeKey], [AttributeValue]) VALUES (@ItemId, @AttributeKey, @AttributeValue)
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Application permission denied.', 16, 1)


