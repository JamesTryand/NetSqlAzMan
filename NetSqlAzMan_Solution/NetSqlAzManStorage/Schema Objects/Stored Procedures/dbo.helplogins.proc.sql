CREATE PROCEDURE [dbo].[netsqlazman_helplogins](@rolename nvarchar(128))
AS

CREATE TABLE #temptable (
	[DBRole] sysname NOT NULL ,
	[MemberName] sysname NOT NULL ,
	[MemberSid] varbinary(85) NULL
	)

IF @rolename = 'NetSqlAzMan_Managers'
BEGIN
	INSERT INTO #temptable EXEC sp_helprolemember 'NetSqlAzMan_Managers'
END

IF @rolename = 'NetSqlAzMan_Users' 
BEGIN
	INSERT INTO #temptable EXEC sp_helprolemember 'NetSqlAzMan_Managers'
	INSERT INTO #temptable EXEC sp_helprolemember 'NetSqlAzMan_Users'
END

IF @rolename = 'NetSqlAzMan_Readers' 
BEGIN
	INSERT INTO #temptable EXEC sp_helprolemember 'NetSqlAzMan_Managers'
	INSERT INTO #temptable EXEC sp_helprolemember 'NetSqlAzMan_Users'
	INSERT INTO #temptable EXEC sp_helprolemember 'NetSqlAzMan_Readers'
END

SELECT DISTINCT SUSER_SNAME(MemberSid) SqlUserOrRole, CASE MemberSid WHEN NULL THEN 1 ELSE 0 END AS IsSqlRole
FROM #temptable
WHERE MemberName NOT IN ('NetSqlAzMan_Administrators', 'NetSqlAzMan_Managers', 'NetSqlAzMan_Users', 'NetSqlAzMan_Readers')
AND SUSER_SNAME(MemberSid) IS NOT NULL
ORDER BY SUSER_SNAME(MemberSid)

DROP TABLE #temptable


