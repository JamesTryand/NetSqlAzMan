CREATE PROCEDURE [dbo].[netsqlazman_StoreGroupMemberUpdate]
(
	@StoreId int,
	@StoreGroupId int,
	@objectSid varbinary(85),
	@WhereDefined tinyint,
	@IsMember bit,
	@Original_StoreGroupMemberId int
)
AS
IF EXISTS(SELECT StoreGroupMemberId FROM dbo.StoreGroupMembers() WHERE StoreGroupMemberId = @Original_StoreGroupMemberId) AND dbo.CheckStorePermissions(@StoreId, 2) = 1
	UPDATE [dbo].[netsqlazman_StoreGroupMembersTable] SET [StoreGroupId] = @StoreGroupId, [objectSid] = @objectSid, [WhereDefined] = @WhereDefined, [IsMember] = @IsMember WHERE [StoreGroupMemberId] = @Original_StoreGroupMemberId
ELSE
	RAISERROR ('Store permission denied.', 16, 1)


