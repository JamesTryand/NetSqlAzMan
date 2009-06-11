CREATE PROCEDURE [dbo].[netsqlazman_ClearBizRule]
(
	@ItemId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ItemId FROM dbo.Items() WHERE ItemId = @ItemId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
	UPDATE [dbo].[netsqlazman_ItemsTable] SET BizRuleId = NULL WHERE [ItemId] = @ItemId AND [ApplicationId] = @ApplicationId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)


