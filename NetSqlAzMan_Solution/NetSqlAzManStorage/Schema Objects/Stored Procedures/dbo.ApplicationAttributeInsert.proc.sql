CREATE PROCEDURE [dbo].[netsqlazman_ApplicationAttributeInsert]
(
	@ApplicationId int,
	@AttributeKey nvarchar(255),
	@AttributeValue nvarchar(4000)
)
AS
IF EXISTS(SELECT ApplicationId FROM dbo.Applications() WHERE ApplicationId = @ApplicationId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
BEGIN
	INSERT INTO [dbo].[netsqlazman_ApplicationAttributesTable] ([ApplicationId], [AttributeKey], [AttributeValue]) VALUES (@ApplicationId, @AttributeKey, @AttributeValue)
	RETURN SCOPE_IDENTITY()
END
ELSE
BEGIN
	RAISERROR ('Application Permission denied.', 16, 1)
END


