CREATE TABLE [dbo].[netsqlazman_ApplicationAttributesTable]
(
[ApplicationAttributeId] [int] NOT NULL IDENTITY(1, 1),
[ApplicationId] [int] NOT NULL,
[AttributeKey] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
[AttributeValue] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NOT NULL
) ON [PRIMARY]


