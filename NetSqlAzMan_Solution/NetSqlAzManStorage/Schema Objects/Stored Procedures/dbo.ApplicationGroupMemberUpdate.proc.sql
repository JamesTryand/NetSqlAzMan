CREATE PROCEDURE [dbo].[netsqlazman_ApplicationGroupMemberUpdate]
(
	@ApplicationGroupId int,
	@objectSid varbinary(85),
	@WhereDefined tinyint,
	@IsMember bit,
	@Original_ApplicationGroupMemberId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ApplicationGroupMemberId FROM dbo.[netsqlazman_ApplicationGroupMembers]() WHERE ApplicationGroupMemberId = @Original_ApplicationGroupMemberId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1
	UPDATE [dbo].[netsqlazman_ApplicationGroupMembersTable] SET [objectSid] = @objectSid, [WhereDefined] = @WhereDefined, [IsMember] = @IsMember WHERE [ApplicationGroupMemberId] = @Original_ApplicationGroupMemberId
ELSE	
	RAISERROR ('Application permission denied.', 16, 1)


