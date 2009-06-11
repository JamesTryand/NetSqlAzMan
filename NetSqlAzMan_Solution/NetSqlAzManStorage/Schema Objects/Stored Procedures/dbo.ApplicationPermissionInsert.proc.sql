CREATE PROCEDURE [dbo].[netsqlazman_ApplicationPermissionInsert]
(
	@ApplicationId int,
	@SqlUserOrRole nvarchar(128),
	@IsSqlRole bit,
	@NetSqlAzManFixedServerRole tinyint
)
AS
IF EXISTS(SELECT ApplicationId FROM dbo.Applications() WHERE ApplicationId = @ApplicationId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
BEGIN
	INSERT INTO dbo.[netsqlazman_ApplicationPermissionsTable] (ApplicationId, SqlUserOrRole, IsSqlRole, NetSqlAzManFixedServerRole) VALUES (@ApplicationId, @SqlUserOrRole, @IsSqlRole, @NetSqlAzManFixedServerRole)
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Application permission denied.', 16, 1)


