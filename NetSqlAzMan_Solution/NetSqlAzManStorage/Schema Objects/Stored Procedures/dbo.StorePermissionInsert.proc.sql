CREATE PROCEDURE [dbo].[netsqlazman_StorePermissionInsert]
(
	@StoreId int,
	@SqlUserOrRole nvarchar(128),
	@IsSqlRole bit,
	@NetSqlAzManFixedServerRole tinyint
)
AS
IF EXISTS(SELECT StoreId FROM dbo.Stores() WHERE StoreId = @StoreId) AND dbo.CheckStorePermissions(@StoreId, 2) = 1
BEGIN
	INSERT INTO dbo.[netsqlazman_StorePermissionsTable] (StoreId, SqlUserOrRole, IsSqlRole, NetSqlAzManFixedServerRole) VALUES (@StoreId, @SqlUserOrRole, @IsSqlRole, @NetSqlAzManFixedServerRole)
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Store permission denied.', 16, 1)


