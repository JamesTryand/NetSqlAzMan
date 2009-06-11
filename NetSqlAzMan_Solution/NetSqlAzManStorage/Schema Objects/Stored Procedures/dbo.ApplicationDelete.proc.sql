CREATE PROCEDURE [dbo].[netsqlazman_ApplicationDelete]
(
	@StoreId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ApplicationId FROM dbo.[netsqlazman_Applications]() WHERE ApplicationId = @ApplicationId) AND dbo.[netsqlazman_CheckStorePermissions](@StoreId, 2) = 1
	DELETE FROM [dbo].[netsqlazman_ApplicationsTable] WHERE [ApplicationId] = @ApplicationId AND [StoreId] = @StoreId
ELSE
	RAISERROR ('Store permission denied', 16, 1)


