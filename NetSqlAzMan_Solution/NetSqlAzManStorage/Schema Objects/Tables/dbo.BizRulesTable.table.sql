CREATE TABLE [dbo].[netsqlazman_BizRulesTable]
(
[BizRuleId] [int] NOT NULL IDENTITY(1, 1),
[BizRuleSource] [text] COLLATE Latin1_General_CI_AS NOT NULL,
[BizRuleLanguage] [tinyint] NOT NULL,
[CompiledAssembly] [image] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]


