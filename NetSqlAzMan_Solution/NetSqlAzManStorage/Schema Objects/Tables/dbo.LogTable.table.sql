CREATE TABLE [dbo].[netsqlazman_LogTable]
(
[LogId] [int] NOT NULL IDENTITY(1, 1),
[LogDateTime] [datetime] NOT NULL,
[WindowsIdentity] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
[SqlIdentity] [nvarchar] (128) COLLATE Latin1_General_CI_AS NULL,
[MachineName] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
[InstanceGuid] [uniqueidentifier] NOT NULL,
[TransactionGuid] [uniqueidentifier] NULL,
[OperationCounter] [int] NOT NULL,
[ENSType] [nvarchar] (255) COLLATE Latin1_General_CI_AS NOT NULL,
[ENSDescription] [nvarchar] (4000) COLLATE Latin1_General_CI_AS NOT NULL,
[LogType] [char] (1) COLLATE Latin1_General_CI_AS NOT NULL
) ON [PRIMARY]


