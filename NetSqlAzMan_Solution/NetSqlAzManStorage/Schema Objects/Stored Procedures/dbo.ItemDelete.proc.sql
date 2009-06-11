CREATE PROCEDURE [dbo].[netsqlazman_ItemDelete]
(
	@ItemId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ItemId FROM dbo.[netsqlazman_Items]() WHERE ItemId = @ItemId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1
	DELETE FROM [dbo].[netsqlazman_ItemsTable] WHERE [ItemId] = @ItemId AND [ApplicationId] = @ApplicationId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)


