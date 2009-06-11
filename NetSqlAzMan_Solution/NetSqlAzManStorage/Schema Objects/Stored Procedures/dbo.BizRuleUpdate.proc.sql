CREATE PROCEDURE [dbo].[netsqlazman_BizRuleUpdate]
(
	@BizRuleSource text,
	@BizRuleLanguage tinyint,
	@CompiledAssembly image,
	@Original_BizRuleId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT BizRuleId FROM dbo.BizRules() WHERE BizRuleId = @Original_BizRuleId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
	UPDATE [dbo].[netsqlazman_BizRulesTable] SET [BizRuleSource] = @BizRuleSource, [BizRuleLanguage] = @BizRuleLanguage, [CompiledAssembly] = @CompiledAssembly WHERE [BizRuleId] = @Original_BizRuleId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)


