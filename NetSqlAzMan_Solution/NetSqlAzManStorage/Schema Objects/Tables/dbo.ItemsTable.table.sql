CREATE TABLE [dbo].[netsqlazman_ItemsTable]
(
[ItemId] [int] NOT NULL IDENTITY(1, 1),
[ApplicationId] [int] NOT NULL,
[Name] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
[Description] [nvarchar] (1024) COLLATE Latin1_General_CI_AS NOT NULL,
[ItemType] [tinyint] NOT NULL,
[BizRuleId] [int] NULL
) ON [PRIMARY]


