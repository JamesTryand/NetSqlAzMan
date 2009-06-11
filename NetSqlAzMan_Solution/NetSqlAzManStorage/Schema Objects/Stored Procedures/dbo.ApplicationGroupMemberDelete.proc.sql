CREATE PROCEDURE [dbo].[netsqlazman_ApplicationGroupMemberDelete]
(
	@ApplicationGroupMemberId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ApplicationGroupMemberId FROM dbo.[netsqlazman_ApplicationGroupMembers]() WHERE ApplicationGroupMemberId = @ApplicationGroupMemberId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1
	DELETE FROM [dbo].[netsqlazman_ApplicationGroupMembersTable] WHERE [ApplicationGroupMemberId] = @ApplicationGroupMemberId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)


