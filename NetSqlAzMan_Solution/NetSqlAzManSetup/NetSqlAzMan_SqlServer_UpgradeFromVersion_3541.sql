--
-- Script To Update dbo.NetSqlAzMan_DBVersion Function In ..NetSqlAzMan_3521
-- Generated sabato, marzo 28, 2009, at 11.38 AM
--
-- Please backup ..NetSqlAzMan_3521 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.NetSqlAzMan_DBVersion Function'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO

REVOKE Execute ON [NetSqlAzMan_DBVersion] TO [NetSqlAzMan_Readers]
GO

SET QUOTED_IDENTIFIER OFF
GO

SET ANSI_NULLS OFF
GO

exec('ALTER FUNCTION [dbo].[NetSqlAzMan_DBVersion] ()  
RETURNS nvarchar(200) AS  
BEGIN 
	return ''3.5.4.4''
END')
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   GRANT Execute ON [NetSqlAzMan_DBVersion] TO [NetSqlAzMan_Readers]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.NetSqlAzMan_DBVersion Function Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.NetSqlAzMan_DBVersion Function'
END
GO
