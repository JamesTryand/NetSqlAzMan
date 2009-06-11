CREATE PROCEDURE [dbo].[netsqlazman_ReloadBizRule]
(
	@ItemId int,
	@BizRuleId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ItemId FROM dbo.[netsqlazman_Items]() WHERE ItemId = @ItemId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1
	UPDATE [dbo].[netsqlazman_ItemsTable] SET BizRuleId = @BizRuleId WHERE [ItemId] = @ItemId AND [ApplicationId] = @ApplicationId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)


