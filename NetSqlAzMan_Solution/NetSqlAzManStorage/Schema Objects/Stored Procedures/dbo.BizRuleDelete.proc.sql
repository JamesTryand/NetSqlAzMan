CREATE PROCEDURE [dbo].[netsqlazman_BizRuleDelete]
(
	@BizRuleId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT BizRuleId FROM dbo.[netsqlazman_BizRulesTable] WHERE BizRuleId = @BizRuleId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
	DELETE FROM [dbo].[netsqlazman_BizRulesTable] WHERE [BizRuleId] = @BizRuleId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)


