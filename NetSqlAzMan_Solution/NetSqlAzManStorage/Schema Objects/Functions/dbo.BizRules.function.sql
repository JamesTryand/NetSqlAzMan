CREATE FUNCTION [dbo].[netsqlazman_BizRules]()
RETURNS TABLE
AS
RETURN
	SELECT     dbo.[netsqlazman_BizRulesTable].*
	FROM         dbo.[netsqlazman_BizRulesTable] INNER JOIN
	                      dbo.[netsqlazman_Items]() Items ON dbo.[netsqlazman_BizRulesTable].BizRuleId = Items.BizRuleId


