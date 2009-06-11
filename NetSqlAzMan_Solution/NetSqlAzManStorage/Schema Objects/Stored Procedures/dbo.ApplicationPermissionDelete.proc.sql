CREATE PROCEDURE [dbo].[netsqlazman_ApplicationPermissionDelete]
(
	@ApplicationPermissionId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ApplicationId FROM dbo.[netsqlazman_Applications]() WHERE ApplicationId = @ApplicationId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1
	DELETE FROM dbo.[netsqlazman_ApplicationPermissionsTable] WHERE ApplicationPermissionId = @ApplicationPermissionId AND ApplicationId = @ApplicationId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)


