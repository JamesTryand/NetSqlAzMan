CREATE PROCEDURE [dbo].[netsqlazman_AuthorizationUpdate]
(
	@ItemId int,
	@ownerSid varbinary(85),
	@ownerSidWhereDefined tinyint,
	@objectSid varbinary(85),
	@objectSidWhereDefined tinyint,
	@AuthorizationType tinyint,
	@ValidFrom datetime,
	@ValidTo datetime,
	@Original_AuthorizationId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT AuthorizationId FROM dbo.[netsqlazman_Authorizations]() WHERE AuthorizationId = @Original_AuthorizationId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1
	UPDATE [dbo].[netsqlazman_AuthorizationsTable] SET [ownerSid] = @ownerSid, [ownerSidWhereDefined] = @ownerSidWhereDefined, [objectSid] = @objectSid, [objectSidWhereDefined] = @objectSidWhereDefined, [AuthorizationType] = @AuthorizationType, [ValidFrom] = @ValidFrom, [ValidTo] = @ValidTo WHERE [AuthorizationId] = @Original_AuthorizationId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)


