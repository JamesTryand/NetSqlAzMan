CREATE PROCEDURE [dbo].[netsqlazman_AuthorizationInsert]
(
	@ItemId int,
	@ownerSid varbinary(85),
	@ownerSidWhereDefined tinyint,
	@objectSid varbinary(85),
	@objectSidWhereDefined tinyint,
	@AuthorizationType tinyint,
	@ValidFrom datetime,
	@ValidTo datetime,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ItemId FROM dbo.[netsqlazman_Items]() WHERE ItemId = @ItemId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1
BEGIN
	INSERT INTO [dbo].[netsqlazman_AuthorizationsTable] ([ItemId], [ownerSid], [ownerSidWhereDefined], [objectSid], [objectSidWhereDefined], [AuthorizationType], [ValidFrom], [ValidTo]) VALUES (@ItemId, @ownerSid, @ownerSidWhereDefined, @objectSid, @objectSidWhereDefined, @AuthorizationType, @ValidFrom, @ValidTo)
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Application permission denied.', 16, 1)


