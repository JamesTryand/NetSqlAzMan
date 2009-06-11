CREATE PROCEDURE [dbo].[netsqlazman_ApplicationAttributeDelete]
(
	@ApplicationId int,
	@ApplicationAttributeId int
)
AS
IF EXISTS(SELECT ApplicationAttributeId FROM dbo.[netsqlazman_ApplicationAttributes]() WHERE ApplicationAttributeId = @ApplicationAttributeId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1
	DELETE FROM [dbo].[netsqlazman_ApplicationAttributesTable] WHERE [ApplicationAttributeId] = @ApplicationAttributeId AND [ApplicationId] = @ApplicationId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)


