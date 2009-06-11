ALTER TABLE [dbo].[netsqlazman_ItemsTable] ADD
CONSTRAINT [FK_Items_BizRules] FOREIGN KEY ([BizRuleId]) REFERENCES [dbo].[netsqlazman_BizRulesTable] ([BizRuleId])


