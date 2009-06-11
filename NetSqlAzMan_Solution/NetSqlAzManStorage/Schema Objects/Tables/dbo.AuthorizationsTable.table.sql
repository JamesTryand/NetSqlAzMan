CREATE TABLE [dbo].[netsqlazman_AuthorizationsTable]
(
[AuthorizationId] [int] NOT NULL IDENTITY(1, 1),
[ItemId] [int] NOT NULL,
[ownerSid] [varbinary] (85) NOT NULL,
[ownerSidWhereDefined] [tinyint] NOT NULL,
[objectSid] [varbinary] (85) NOT NULL,
[objectSidWhereDefined] [tinyint] NOT NULL,
[AuthorizationType] [tinyint] NOT NULL,
[ValidFrom] [datetime] NULL,
[ValidTo] [datetime] NULL
) ON [PRIMARY]


