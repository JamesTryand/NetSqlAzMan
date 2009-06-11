CREATE PROCEDURE [dbo].[netsqlazman_StoreGroupMemberInsert]
(
	@StoreId int,
	@StoreGroupId int,
	@objectSid varbinary(85),
	@WhereDefined tinyint,
	@IsMember bit
)
AS
IF EXISTS(SELECT StoreGroupId FROM dbo.[netsqlazman_StoreGroups]() WHERE StoreGroupId = @StoreGroupId) AND dbo.[netsqlazman_CheckStorePermissions](@StoreId, 2) = 1
BEGIN
	INSERT INTO [dbo].[netsqlazman_StoreGroupMembersTable] ([StoreGroupId], [objectSid], [WhereDefined], [IsMember]) VALUES (@StoreGroupId, @objectSid, @WhereDefined, @IsMember)
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Store permission denied.', 16, 1)


