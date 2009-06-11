CREATE TABLE [dbo].[netsqlazman_ApplicationPermissionsTable]
(
[ApplicationPermissionId] [int] NOT NULL IDENTITY(1, 1),
[ApplicationId] [int] NOT NULL,
[SqlUserOrRole] [nvarchar] (128) COLLATE Latin1_General_CI_AS NOT NULL,
[IsSqlRole] [bit] NOT NULL,
[NetSqlAzManFixedServerRole] [tinyint] NOT NULL
) ON [PRIMARY]


