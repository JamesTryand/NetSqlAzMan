CREATE PROCEDURE [dbo].[netsqlazman_AuthorizationAttributeDelete]
(
	@AuthorizationAttributeId int,
	@ApplicationId int
)
AS
IF  EXISTS(SELECT AuthorizationAttributeId FROM dbo.[netsqlazman_AuthorizationAttributes]() WHERE AuthorizationAttributeId = @AuthorizationAttributeId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 1) = 1
	DELETE FROM [dbo].[netsqlazman_AuthorizationAttributesTable] WHERE [AuthorizationAttributeId] = @AuthorizationAttributeId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)


