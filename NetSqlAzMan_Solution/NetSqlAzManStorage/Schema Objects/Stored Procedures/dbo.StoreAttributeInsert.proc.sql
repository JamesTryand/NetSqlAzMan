CREATE PROCEDURE [dbo].[netsqlazman_StoreAttributeInsert]
(
	@StoreId int,
	@AttributeKey nvarchar(255),
	@AttributeValue nvarchar(4000)
)
AS
IF EXISTS(Select StoreId FROM dbo.Stores() WHERE StoreId = @StoreId) AND dbo.CheckStorePermissions(@StoreId, 2) = 1
BEGIN
	INSERT INTO [dbo].[netsqlazman_StoreAttributesTable] ([StoreId], [AttributeKey], [AttributeValue]) VALUES (@StoreId, @AttributeKey, @AttributeValue);
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Store permission denied.', 16, 1)


