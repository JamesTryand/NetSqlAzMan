CREATE PROCEDURE [dbo].[netsqlazman_RevokeApplicationAccess] (
	@ApplicationId int,
	@SqlUserOrRole sysname,
	@NetSqlAzManFixedServerRole tinyint)
AS
DECLARE @StoreId int
SET @StoreId = (SELECT StoreId FROM dbo.[netsqlazman_Applications]() WHERE ApplicationId = @ApplicationId)
IF EXISTS(SELECT ApplicationId FROM dbo.[netsqlazman_ApplicationsTable] WHERE ApplicationId = @ApplicationId) AND (dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1 AND @NetSqlAzManFixedServerRole BETWEEN 0 AND 1 OR dbo.[netsqlazman_CheckStorePermissions](@StoreId, 2) = 1 AND @NetSqlAzManFixedServerRole = 2)
BEGIN
	IF EXISTS(SELECT * FROM dbo.[netsqlazman_ApplicationPermissionsTable] WHERE ApplicationId = @ApplicationId AND SqlUserOrRole = @SqlUserOrRole AND NetSqlAzManFixedServerRole = @NetSqlAzManFixedServerRole)
		DELETE FROM dbo.[netsqlazman_ApplicationPermissionsTable] WHERE ApplicationId = @ApplicationId AND SqlUserOrRole = @SqlUserOrRole AND NetSqlAzManFixedServerRole = @NetSqlAzManFixedServerRole
	ELSE
		RAISERROR ('Permission not found. Revoke Application Access ignored.', -1, -1)
END
ELSE
	RAISERROR ('Application NOT Found or Application permission denied.', 16, 1)


