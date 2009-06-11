CREATE PROCEDURE [dbo].[netsqlazman_AuthorizationAttributeUpdate]
(
	@AttributeKey nvarchar(255),
	@AttributeValue nvarchar(4000),
	@Original_AuthorizationAttributeId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT AuthorizationAttributeId FROM dbo.[netsqlazman_AuthorizationAttributes]() WHERE AuthorizationAttributeId = @Original_AuthorizationAttributeId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 1) = 1
	UPDATE [dbo].[netsqlazman_AuthorizationAttributesTable] SET [AttributeKey] = @AttributeKey, [AttributeValue] = @AttributeValue WHERE [AuthorizationAttributeId] = @Original_AuthorizationAttributeId
ELSE
	RAISERROR ('Application permission denied.', 16 ,1)


