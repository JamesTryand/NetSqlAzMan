CREATE TABLE [dbo].[netsqlazman_ApplicationGroupMembersTable]
(
[ApplicationGroupMemberId] [int] NOT NULL IDENTITY(1, 1),
[ApplicationGroupId] [int] NOT NULL,
[objectSid] [varbinary] (85) NOT NULL,
[WhereDefined] [tinyint] NOT NULL,
[IsMember] [bit] NOT NULL
) ON [PRIMARY]


