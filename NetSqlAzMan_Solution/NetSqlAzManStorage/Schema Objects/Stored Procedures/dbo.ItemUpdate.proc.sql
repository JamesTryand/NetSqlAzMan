CREATE PROCEDURE [dbo].[netsqlazman_ItemUpdate]
(
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@ItemType tinyint,
	@Original_ItemId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ItemId FROM dbo.Items() WHERE ItemId = @Original_ItemId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
	UPDATE [dbo].[netsqlazman_ItemsTable] SET [Name] = @Name, [Description] = @Description, [ItemType] = @ItemType WHERE [ItemId] = @Original_ItemId AND [ApplicationId] = @ApplicationId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)


