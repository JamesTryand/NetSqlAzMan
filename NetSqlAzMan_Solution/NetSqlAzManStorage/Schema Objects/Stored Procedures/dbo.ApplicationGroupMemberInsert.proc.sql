CREATE PROCEDURE [dbo].[netsqlazman_ApplicationGroupMemberInsert]
(
	@ApplicationGroupId int,
	@objectSid varbinary(85),
	@WhereDefined tinyint,
	@IsMember bit,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ApplicationGroupId FROM dbo.[netsqlazman_ApplicationGroups]() WHERE ApplicationGroupId = @ApplicationGroupId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
BEGIN
	INSERT INTO [dbo].[netsqlazman_ApplicationGroupMembersTable] ([ApplicationGroupId], [objectSid], [WhereDefined], [IsMember]) VALUES (@ApplicationGroupId, @objectSid, @WhereDefined, @IsMember)
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Application permission denied.', 16, 1)


