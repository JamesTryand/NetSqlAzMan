--
-- Please backup ..NetSqlAzManStorage before executing this script
--


PRINT 'Creating dbo User'
GO

exec sp_revokedbaccess N''
GO

if not exists (select * from master.dbo.syslogins where loginname = N'sa')
   exec sp_addlogin N'sa'
GO

exec sp_grantdbaccess N'sa', N'dbo'
GO

PRINT 'dbo User Updated Successfully'
GO

--
-- Script To Update dbo.NetSqlAzMan_DBVersion Function In ..NetSqlAzMan
--
-- Please backup ... NetSqlAzManStorage before executing this script
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
	return ''3.5.3.0''
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

/****** Object:  StoredProcedure [dbo].[ExecuteLDAPQuery]    Script Date: 03/24/2009 22:02:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[ExecuteLDAPQuery](@LDAPPATH NVARCHAR(4000), @LDAPQUERY NVARCHAR(4000), @members_cur CURSOR VARYING OUTPUT)
AS
-- REMEMBER !!!
-- BEFORE executing ExecuteLDAPQuery procedure ... a Linked Server named 'ADSI' must be added:
-- --sp_addlinkedserver 'ADSI', 'Active Directory Service Interfaces', 'ADSDSOObject', 'adsdatasource'
CREATE TABLE #temp (objectSid VARBINARY(85))
IF @LDAPQUERY IS NULL OR RTRIM(LTRIM(@LDAPQUERY))='' OR @LDAPPATH IS NULL OR RTRIM(LTRIM(@LDAPPATH))=''
BEGIN
SET @members_cur = CURSOR STATIC FORWARD_ONLY FOR SELECT * FROM #temp
OPEN @members_cur
DROP TABLE #temp
RETURN
END
SET @LDAPPATH = REPLACE(@LDAPPATH, N'''', N'''''')
SET @LDAPQUERY = REPLACE(@LDAPQUERY, N'''', N'''''')
DECLARE @QUERY nvarchar(4000)
DECLARE @LDAPROOTDSEPART nvarchar(4000)
DECLARE @LDAPQUERYPART nvarchar(4000)
SET @LDAPROOTDSEPART = LTRIM(@LDAPQUERY)
IF CHARINDEX('[RootDSE:', @LDAPROOTDSEPART)=1
BEGIN
	SET @LDAPROOTDSEPART = SUBSTRING(@LDAPROOTDSEPART, 10, CHARINDEX(']', @LDAPROOTDSEPART)-10)
	SET @LDAPQUERYPART = SUBSTRING(@LDAPQUERY, CHARINDEX( ']', @LDAPQUERY)+1, 4000)
END
ELSE
BEGIN
	SET @LDAPROOTDSEPART = @LDAPPATH
	SET @LDAPQUERYPART = @LDAPQUERY
END
SET @QUERY = CHAR(39) + '<' + 'LDAP://'+ @LDAPROOTDSEPART + '>;(&(!(objectClass=computer))(&(|(objectClass=user)(objectClass=group)))' + @LDAPQUERYPART + ');objectSid;subtree' + CHAR(39) 
DECLARE @OPENQUERY nvarchar(4000)
SET @OPENQUERY = 'SELECT * FROM OPENQUERY(ADSI, ' + @QUERY + ')'
INSERT INTO #temp EXEC (@OPENQUERY)
SET @members_cur = CURSOR STATIC FORWARD_ONLY FOR SELECT * FROM #temp
OPEN @members_cur
DROP TABLE #temp

GO

