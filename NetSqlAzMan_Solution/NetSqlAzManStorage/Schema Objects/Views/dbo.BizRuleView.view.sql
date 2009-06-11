CREATE VIEW [dbo].[netsqlazman_BizRuleView]
AS
SELECT     [netsqlazman_Items].ItemId, [netsqlazman_Items].ApplicationId, [netsqlazman_Items].Name, [netsqlazman_Items].Description, [netsqlazman_Items].ItemType, [netsqlazman_BizRules].BizRuleSource, [netsqlazman_BizRules].BizRuleLanguage, 
                      [netsqlazman_BizRules].CompiledAssembly
FROM         dbo.[netsqlazman_Items]() [netsqlazman_Items] INNER JOIN
                      dbo.[netsqlazman_BizRules]() [netsqlazman_BizRules] ON [netsqlazman_Items].BizRuleId = [netsqlazman_BizRules].BizRuleId


