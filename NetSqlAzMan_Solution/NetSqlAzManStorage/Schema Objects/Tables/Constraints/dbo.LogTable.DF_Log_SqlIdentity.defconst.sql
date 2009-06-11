ALTER TABLE [dbo].[netsqlazman_LogTable] ADD CONSTRAINT [DF_Log_SqlIdentity] DEFAULT (suser_sname()) FOR [SqlIdentity]


