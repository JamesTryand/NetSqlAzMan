CREATE TABLE [dbo].[netsqlazman_ApplicationGroupsTable]
(
[ApplicationGroupId] [int] NOT NULL IDENTITY(1, 1),
[ApplicationId] [int] NOT NULL,
[objectSid] [varbinary] (85) NOT NULL,
[Name] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
[Description] [nvarchar] (1024) COLLATE Latin1_General_CI_AS NOT NULL,
[LDapQuery] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NULL,
[GroupType] [tinyint] NOT NULL
) ON [PRIMARY]


