CREATE VIEW [dbo].[netsqlazman_BizRuleView]
AS
SELECT     Items.ItemId, Items.ApplicationId, Items.Name, Items.Description, Items.ItemType, BizRules.BizRuleSource, BizRules.BizRuleLanguage, 
                      BizRules.CompiledAssembly
FROM         dbo.Items() Items INNER JOIN
                      dbo.BizRules() BizRules ON Items.BizRuleId = BizRules.BizRuleId


