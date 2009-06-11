CREATE TABLE [dbo].[UsersDemo]
(
[UserID] [int] NOT NULL IDENTITY(1, 1),
[UserName] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
[Password] [varbinary] (50) NULL,
[FullName] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
[OtherFields] [nvarchar] (255) COLLATE Latin1_General_CI_AS NULL
) ON [PRIMARY]


