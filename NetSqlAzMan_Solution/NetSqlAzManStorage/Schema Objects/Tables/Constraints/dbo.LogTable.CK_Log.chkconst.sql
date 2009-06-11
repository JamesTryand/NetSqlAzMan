ALTER TABLE [dbo].[netsqlazman_LogTable] ADD CONSTRAINT [CK_Log] CHECK (([LogType] = 'I' or [LogType] = 'W' or [LogType] = 'E'))


