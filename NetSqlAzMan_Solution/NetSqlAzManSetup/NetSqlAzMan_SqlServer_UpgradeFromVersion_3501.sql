--
-- Script To Update dbo.NetSqlAzMan_DBVersion Function In ..NetSqlAzMan_3501
-- Generated sabato, marzo 28, 2009, at 11.35 AM
--
-- Please backup ..NetSqlAzMan_3501 before executing this script
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
	return ''3.5.4.0''
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

--
-- Script To Update dbo.ExecuteLDAPQuery Procedure In ..NetSqlAzMan_3501
-- Generated sabato, marzo 28, 2009, at 11.35 AM
--
-- Please backup ..NetSqlAzMan_3501 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.ExecuteLDAPQuery Procedure'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO


exec('ALTER PROCEDURE [dbo].[ExecuteLDAPQuery](@LDAPPATH NVARCHAR(4000), @LDAPQUERY NVARCHAR(4000), @members_cur CURSOR VARYING OUTPUT)
AS
-- REMEMBER !!!
-- BEFORE executing ExecuteLDAPQuery procedure ... a Linked Server named ''ADSI'' must be added:
-- --sp_addlinkedserver ''ADSI'', ''Active Directory Service Interfaces'', ''ADSDSOObject'', ''adsdatasource''
CREATE TABLE #temp (objectSid VARBINARY(85))
IF @LDAPQUERY IS NULL OR RTRIM(LTRIM(@LDAPQUERY))='''' OR @LDAPPATH IS NULL OR RTRIM(LTRIM(@LDAPPATH))=''''
BEGIN
SET @members_cur = CURSOR STATIC FORWARD_ONLY FOR SELECT * FROM #temp
OPEN @members_cur
DROP TABLE #temp
RETURN
END
SET @LDAPPATH = REPLACE(@LDAPPATH, N'''''''', N'''''''''''')
SET @LDAPQUERY = REPLACE(@LDAPQUERY, N'''''''', N'''''''''''')
DECLARE @QUERY nvarchar(4000)
DECLARE @LDAPROOTDSEPART nvarchar(4000)
DECLARE @LDAPQUERYPART nvarchar(4000)
SET @LDAPROOTDSEPART = LTRIM(@LDAPQUERY)
IF CHARINDEX(''[RootDSE:'', @LDAPROOTDSEPART)=1
BEGIN
	SET @LDAPROOTDSEPART = SUBSTRING(@LDAPROOTDSEPART, 10, CHARINDEX('']'', @LDAPROOTDSEPART)-10)
	SET @LDAPQUERYPART = SUBSTRING(@LDAPQUERY, CHARINDEX( '']'', @LDAPQUERY)+1, 4000)
END
ELSE
BEGIN
	SET @LDAPROOTDSEPART = @LDAPPATH
	SET @LDAPQUERYPART = @LDAPQUERY
END
SET @QUERY = CHAR(39) + ''<'' + ''LDAP://''+ @LDAPROOTDSEPART + ''>;(&(!(objectClass=computer))(&(|(objectClass=user)(objectClass=group)))'' + @LDAPQUERYPART + '');objectSid;subtree'' + CHAR(39) 
DECLARE @OPENQUERY nvarchar(4000)
SET @OPENQUERY = ''SELECT * FROM OPENQUERY(ADSI, '' + @QUERY + '')''
INSERT INTO #temp EXEC (@OPENQUERY)
SET @members_cur = CURSOR STATIC FORWARD_ONLY FOR SELECT * FROM #temp
OPEN @members_cur
DROP TABLE #temp')
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.ExecuteLDAPQuery Procedure Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.ExecuteLDAPQuery Procedure'
END
GO

--
-- Script To Update dbo.helplogins Procedure In ..NetSqlAzMan_3501
-- Generated sabato, marzo 28, 2009, at 11.35 AM
--
-- Please backup ..NetSqlAzMan_3501 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.helplogins Procedure'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO

REVOKE Execute ON [helplogins] TO [NetSqlAzMan_Readers]
GO

SET QUOTED_IDENTIFIER OFF
GO

SET ANSI_NULLS OFF
GO

exec('ALTER PROCEDURE [dbo].[helplogins](@rolename nvarchar(128))
AS

CREATE TABLE #temptable (
	[DBRole] sysname NOT NULL ,
	[MemberName] sysname NOT NULL ,
	[MemberSID] varbinary(85) NULL
	)

IF @rolename = ''NetSqlAzMan_Managers''
BEGIN
	INSERT INTO #temptable EXEC sp_helprolemember ''NetSqlAzMan_Managers''
END

IF @rolename = ''NetSqlAzMan_Users'' 
BEGIN
	INSERT INTO #temptable EXEC sp_helprolemember ''NetSqlAzMan_Managers''
	INSERT INTO #temptable EXEC sp_helprolemember ''NetSqlAzMan_Users''
END

IF @rolename = ''NetSqlAzMan_Readers'' 
BEGIN
	INSERT INTO #temptable EXEC sp_helprolemember ''NetSqlAzMan_Managers''
	INSERT INTO #temptable EXEC sp_helprolemember ''NetSqlAzMan_Users''
	INSERT INTO #temptable EXEC sp_helprolemember ''NetSqlAzMan_Readers''
END

SELECT DISTINCT SUSER_SNAME(MemberSid) SqlUserOrRole, CASE MemberSid WHEN NULL THEN 1 ELSE 0 END AS IsSqlRole
FROM #temptable
WHERE MemberName NOT IN (''NetSqlAzMan_Administrators'', ''NetSqlAzMan_Managers'', ''NetSqlAzMan_Users'', ''NetSqlAzMan_Readers'')
AND SUSER_SNAME(MemberSid) IS NOT NULL
ORDER BY SUSER_SNAME(MemberSid)

DROP TABLE #temptable')
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   GRANT Execute ON [helplogins] TO [NetSqlAzMan_Readers]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.helplogins Procedure Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.helplogins Procedure'
END
GO