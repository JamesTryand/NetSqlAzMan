CREATE PROCEDURE [dbo].[netsqlazman_StoreGroupMemberDelete]
(
	@StoreId int,
	@StoreGroupMemberId int
)
AS
IF EXISTS(SELECT StoreGroupMemberId FROM dbo.StoreGroupMembers() WHERE StoreGroupMemberId = @StoreGroupMemberId) AND dbo.CheckStorePermissions(@StoreId, 2) = 1
	DELETE FROM [dbo].[netsqlazman_StoreGroupMembersTable] WHERE [StoreGroupMemberId] = @StoreGroupMemberId
ELSE
	RAISERROR ('Store permission denied.', 16, 1)


