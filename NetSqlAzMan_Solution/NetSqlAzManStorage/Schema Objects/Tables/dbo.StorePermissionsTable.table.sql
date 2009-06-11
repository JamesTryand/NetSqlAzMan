CREATE TABLE [dbo].[netsqlazman_StorePermissionsTable]
(
[StorePermissionId] [int] NOT NULL IDENTITY(1, 1),
[StoreId] [int] NOT NULL,
[SqlUserOrRole] [nvarchar] (128) COLLATE Latin1_General_CI_AS NOT NULL,
[IsSqlRole] [bit] NOT NULL,
[NetSqlAzManFixedServerRole] [tinyint] NOT NULL
) ON [PRIMARY]


