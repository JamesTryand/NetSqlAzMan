CREATE TABLE [dbo].[netsqlazman_StoreGroupMembersTable]
(
[StoreGroupMemberId] [int] NOT NULL IDENTITY(1, 1),
[StoreGroupId] [int] NOT NULL,
[objectSid] [varbinary] (85) NOT NULL,
[WhereDefined] [tinyint] NOT NULL,
[IsMember] [bit] NOT NULL
) ON [PRIMARY]


