CREATE PROCEDURE [dbo].[netsqlazman_ApplicationGroupUpdate]
(
	@objectSid varbinary(85),
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@LDapQuery nvarchar(4000),
	@GroupType tinyint,
	@Original_ApplicationGroupId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ApplicationGroupId FROM dbo.[netsqlazman_ApplicationGroups]() WHERE ApplicationGroupId = @Original_ApplicationGroupId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
	UPDATE [dbo].[netsqlazman_ApplicationGroupsTable] SET [objectSid] = @objectSid, [Name] = @Name, [Description] = @Description, [LDapQuery] = @LDapQuery, [GroupType] = @GroupType WHERE [ApplicationGroupId] = @Original_ApplicationGroupId AND [ApplicationId] = @ApplicationId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)


