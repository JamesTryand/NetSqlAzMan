CREATE PROCEDURE [dbo].[netsqlazman_ApplicationGroupInsert]
(
	@ApplicationId int,
	@objectSid varbinary(85),
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@LDapQuery nvarchar(4000),
	@GroupType tinyint
)
AS
IF EXISTS(SELECT ApplicationId FROM dbo.Applications() WHERE ApplicationId = @ApplicationId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
BEGIN
	INSERT INTO [dbo].[netsqlazman_ApplicationGroupsTable] ([ApplicationId], [objectSid], [Name], [Description], [LDapQuery], [GroupType]) VALUES (@ApplicationId, @objectSid, @Name, @Description, @LDapQuery, @GroupType)
	RETURN SCOPE_IDENTITY()
END
ELSE	
	RAISERROR ('Application permission denied.', 16, 1)


