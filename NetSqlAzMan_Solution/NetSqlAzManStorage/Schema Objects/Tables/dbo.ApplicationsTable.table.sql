CREATE TABLE [dbo].[netsqlazman_ApplicationsTable]
(
[ApplicationId] [int] NOT NULL IDENTITY(1, 1),
[StoreId] [int] NOT NULL,
[Name] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
[Description] [nvarchar] (1024) COLLATE Latin1_General_CI_AS NOT NULL
) ON [PRIMARY]


