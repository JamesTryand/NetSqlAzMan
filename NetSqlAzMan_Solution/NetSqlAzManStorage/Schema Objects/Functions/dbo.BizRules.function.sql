CREATE FUNCTION [dbo].[BizRules]()
RETURNS TABLE
AS
RETURN
	SELECT     dbo.[netsqlazman_BizRulesTable].*
	FROM         dbo.[netsqlazman_BizRulesTable] INNER JOIN
	                      dbo.Items() Items ON dbo.[netsqlazman_BizRulesTable].BizRuleId = Items.BizRuleId


