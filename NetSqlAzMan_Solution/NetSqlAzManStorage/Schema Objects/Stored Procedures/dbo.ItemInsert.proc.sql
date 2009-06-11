CREATE PROCEDURE [dbo].[netsqlazman_ItemInsert]
(
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@ItemType tinyint,
	@BizRuleId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ApplicationId FROM dbo.Applications() WHERE ApplicationId = @ApplicationId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
BEGIN
	INSERT INTO [dbo].[netsqlazman_ItemsTable] ([ApplicationId], [Name], [Description], [ItemType], [BizRuleId]) VALUES (@ApplicationId, @Name, @Description, @ItemType, @BizRuleId)
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Application permission denied.', 16, 1)


