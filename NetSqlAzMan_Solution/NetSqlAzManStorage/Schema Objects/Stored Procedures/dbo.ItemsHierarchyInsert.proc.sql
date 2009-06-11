CREATE PROCEDURE [dbo].[netsqlazman_ItemsHierarchyInsert]
(
	@ItemId int,
	@MemberOfItemId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ItemId FROM dbo.Items() WHERE ItemId = @ItemId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
BEGIN
	INSERT INTO [dbo].[netsqlazman_ItemsHierarchyTable] ([ItemId], [MemberOfItemId]) VALUES (@ItemId, @MemberOfItemId)
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Application permission denied.', 16, 1)


