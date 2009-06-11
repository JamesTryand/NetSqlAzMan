CREATE TABLE [dbo].[netsqlazman_ItemAttributesTable]
(
[ItemAttributeId] [int] NOT NULL IDENTITY(1, 1),
[ItemId] [int] NOT NULL,
[AttributeKey] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
[AttributeValue] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NOT NULL
) ON [PRIMARY]


