CREATE PROCEDURE [dbo].[netsqlazman_AuthorizationAttributeInsert]
(
	@AuthorizationId int,
	@AttributeKey nvarchar(255),
	@AttributeValue nvarchar(4000),
	@ApplicationId int
)
AS
IF EXISTS(SELECT AuthorizationId FROM dbo.Authorizations() WHERE AuthorizationId = @AuthorizationId) AND dbo.CheckApplicationPermissions(@ApplicationId, 1) = 1
BEGIN
	INSERT INTO [dbo].[netsqlazman_AuthorizationAttributesTable] ([AuthorizationId], [AttributeKey], [AttributeValue]) VALUES (@AuthorizationId, @AttributeKey, @AttributeValue)
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Application permission denied.', 16, 1)


