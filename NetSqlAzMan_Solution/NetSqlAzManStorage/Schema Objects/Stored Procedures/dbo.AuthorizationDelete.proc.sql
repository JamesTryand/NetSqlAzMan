CREATE PROCEDURE [dbo].[netsqlazman_AuthorizationDelete]
(
	@AuthorizationId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT AuthorizationId FROM dbo.Authorizations() WHERE AuthorizationId = @AuthorizationId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
	DELETE FROM [dbo].[netsqlazman_AuthorizationsTable] WHERE [AuthorizationId] = @AuthorizationId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)


