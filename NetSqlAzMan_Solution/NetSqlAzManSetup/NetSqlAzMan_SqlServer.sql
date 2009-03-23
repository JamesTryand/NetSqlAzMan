/*************************************************************************************/
/* ATTENTION: REMEMBER TO CREATE A DATABASE FIRST (Tipical: NetSqlAzManStorage) !!!  */
/*            THIS SCRIPT DOES NOT CREATE DATABASE !!!!                              */
/*************************************************************************************/

/** ADD ADSI LINKED SERVER PROVIDER **/
    -- CHECK IF SERVER ALREADY EXISTS
    if not exists (select * from master.dbo.sysservers where srvname = 'ADSI')
    begin
     exec sp_addlinkedserver 'ADSI', 'Active Directory Service Interfaces', 'ADSDSOObject', 'adsdatasource'
     /** REMEMBER: change security context credentials for this linked server to allow ADSI provider to estabilish a connection with your DOMAIN **/
    end
GO

USE [NetSqlAzManStorage]

EXEC dbo.sp_addrole @rolename = N'NetSqlAzMan_Readers'
GO
EXEC dbo.sp_addrole @rolename = N'NetSqlAzMan_Users'
GO
EXEC dbo.sp_addrole @rolename = N'NetSqlAzMan_Managers'
GO
EXEC dbo.sp_addrole @rolename = N'NetSqlAzMan_Administrators'
GO
EXEC dbo.sp_grantdbaccess @loginame = N'BUILTIN\Administrators', @name_in_db = N'BUILTIN\Administrators'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[StoresTable](
	[StoreId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](1024) NOT NULL,
 CONSTRAINT [PK_Stores] PRIMARY KEY CLUSTERED 
(
	[StoreId] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE UNIQUE NONCLUSTERED INDEX [Stores_Name_Unique_Index] ON [dbo].[StoresTable] 
(
	[Name] ASC
) ON [PRIMARY]
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE FUNCTION [dbo].[NetSqlAzMan_DBVersion] ()  
RETURNS nvarchar(200) AS  
BEGIN 
	return '3.5.2.1'
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE FUNCTION [dbo].[IAmAdmin] ()  
RETURNS bit AS  
BEGIN 
DECLARE @result bit
IF IS_MEMBER('db_owner')=1 OR IS_MEMBER('NetSqlAzMan_Administrators')=1
	SET @result = 1
ELSE
	SET @result = 0
RETURN @result
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[MergeAuthorizations](@AUTH1 tinyint, @AUTH2 tinyint)
RETURNS tinyint
AS
BEGIN
-- 0 Neutral 1 Allow 2 Deny 3 AllowWithDelegation
DECLARE @RESULT tinyint
IF @AUTH1 IS NULL 
BEGIN
	SET @RESULT = @AUTH2
END
ELSE 
IF @AUTH2 IS NULL 
BEGIN
SET @RESULT = @AUTH1
END
ELSE
BEGIN
	IF @AUTH1 = 2 SET @AUTH1 = 4 -- DENY WINS
	ELSE
	IF @AUTH2 = 2 SET @AUTH2 = 4 -- DENY WINS
	IF @AUTH1 >= @AUTH2
                	SET @RESULT = @AUTH1
	ELSE
	IF @AUTH1 < @AUTH2
		SET @RESULT = @AUTH2
	IF @RESULT = 4 SET @RESULT = 2
END
RETURN @RESULT
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BizRulesTable](
	[BizRuleId] [int] IDENTITY(1,1) NOT NULL,
	[BizRuleSource] [text] NOT NULL,
	[BizRuleLanguage] [tinyint] NOT NULL,
	[CompiledAssembly] [image] NOT NULL,
 CONSTRAINT [PK_BizRules] PRIMARY KEY CLUSTERED 
(
	[BizRuleId] ASC
) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ItemsHierarchyTable](
	[ItemId] [int] NOT NULL,
	[MemberOfItemId] [int] NOT NULL,
 CONSTRAINT [PK_ItemsHierarchy] PRIMARY KEY CLUSTERED 
(
	[ItemId] ASC,
	[MemberOfItemId] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ItemsHierarchy] ON [dbo].[ItemsHierarchyTable] 
(
	[ItemId] ASC
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ItemsHierarchy_1] ON [dbo].[ItemsHierarchyTable] 
(
	[MemberOfItemId] ASC
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Settings](
	[SettingName] [nvarchar](255) NOT NULL,
	[SettingValue] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_Settings] PRIMARY KEY CLUSTERED 
(
	[SettingName] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[LogTable](
	[LogId] [int] IDENTITY(1,1) NOT NULL,
	[LogDateTime] [datetime] NOT NULL,
	[WindowsIdentity] [nvarchar](255) NOT NULL,
	[SqlIdentity] [nvarchar](128) NULL CONSTRAINT [DF_Log_SqlIdentity]  DEFAULT (suser_sname()),
	[MachineName] [nvarchar](255) NOT NULL,
	[InstanceGuid] [uniqueidentifier] NOT NULL,
	[TransactionGuid] [uniqueidentifier] NULL,
	[OperationCounter] [int] NOT NULL,
	[ENSType] [nvarchar](255) NOT NULL,
	[ENSDescription] [nvarchar](4000) NOT NULL,
	[LogType] [char](1) NOT NULL,
 CONSTRAINT [PK_Log] PRIMARY KEY NONCLUSTERED 
(
	[LogId] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE CLUSTERED INDEX [IX_Log_2] ON [dbo].[LogTable] 
(
	[LogDateTime] DESC,
	[InstanceGuid] ASC,
	[OperationCounter] DESC
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Log] ON [dbo].[LogTable] 
(
	[WindowsIdentity] ASC
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Log_1] ON [dbo].[LogTable] 
(
	[SqlIdentity] ASC
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UsersDemo](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](255) NOT NULL,
	[Password] [varbinary](50) NULL,
	[FullName] [nvarchar](255) NOT NULL,
	[OtherFields] [nvarchar](255) NULL,
 CONSTRAINT [PK_UsersDemo] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* 
    NetSqlAzMan GetDBUsers TABLE Function
    ************************************************************************
    Creation Date: August, 23  2006
    Purpose: Retrieve from a DB a list of custom Users (DBUserSid, DBUserName)
    Author: Andrea Ferendeles 
    Revision: 1.0.0.0
    Updated by: <put here your name>
    Parameters: 
	use: 
		1)     SELECT * FROM dbo.GetDBUsers(<storename>, <applicationname>, NULL, NULL)            -- to retrieve all DB Users
		2)     SELECT * FROM dbo.GetDBUsers(<storename>, <applicationname>, <customsid>, NULL)  -- to retrieve DB User with specified <customsid>
		3)     SELECT * FROM dbo.GetDBUsers(<storename>, <applicationname>, NULL, <username>)  -- to retrieve DB User with specified <username>

    Remarks: 
	- Update this Function with your CUSTOM CODE
	- Returned DBUserSid must be unique
	- Returned DBUserName must be unique
*/
CREATE FUNCTION [dbo].[GetDBUsers] (@StoreName nvarchar(255), @ApplicationName nvarchar(255), @DBUserSid VARBINARY(85) = NULL, @DBUserName nvarchar(255) = NULL)  
RETURNS TABLE 
AS  
RETURN 
	SELECT TOP 100 PERCENT CONVERT(VARBINARY(85), UserID) AS DBUserSid, UserName AS DBUserName FROM dbo.UsersDemo
	WHERE 
		(@DBUserSid IS NOT NULL AND CONVERT(VARBINARY(85), UserID) = @DBUserSid OR @DBUserSid  IS NULL)
		AND
		(@DBUserName IS NOT NULL AND UserName = @DBUserName OR @DBUserName IS NULL)
	ORDER BY UserName
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- THIS CODE IS JUST FOR AN EXAMPLE: comment this section and customize "INSERT HERE YOUR CUSTOM T-SQL" section below
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
GO
SET ANSI_PADDING OFF
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[DatabaseUsers]
AS
SELECT     *
FROM         dbo.GetDBUsers(NULL, NULL, DEFAULT, DEFAULT) GetDBUsers
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ExecuteLDAPQuery](@LDAPPATH NVARCHAR(4000), @LDAPQUERY NVARCHAR(4000), @members_cur CURSOR VARYING OUTPUT)
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
SET @QUERY = CHAR(39) + '<' + 'LDAP://'+ @LDAPPATH + '>;(&(!(objectClass=computer))(&(|(objectClass=user)(objectClass=group)))' + @LDAPQUERY + ');objectSid;subtree' + CHAR(39) 
DECLARE @OPENQUERY nvarchar(4000)
SET @OPENQUERY = 'SELECT * FROM OPENQUERY(ADSI, ' + @QUERY + ')'
INSERT INTO #temp EXEC (@OPENQUERY)
SET @members_cur = CURSOR STATIC FORWARD_ONLY FOR SELECT * FROM #temp
OPEN @members_cur
DROP TABLE #temp
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[IsAMemberOfGroup](@GROUPTYPE bit, @GROUPOBJECTSID VARBINARY(85), @NETSQLAZMANMODE bit, @LDAPPATH nvarchar(4000), @TOKEN IMAGE, @USERGROUPSCOUNT INT)  
AS  
DECLARE @member_cur CURSOR
DECLARE @memberSid VARBINARY(85)
DECLARE @USERSID VARBINARY(85)
DECLARE @USERGROUPS TABLE(objectSid VARBINARY(85))
DECLARE @I INT
DECLARE @INDEX INT
DECLARE @APP VARBINARY(85)
DECLARE @COUNT int

-- Get User Sid
IF @USERGROUPSCOUNT>0
BEGIN
	SET @USERSID = SUBSTRING(@TOKEN,1,85)
	SET @I = CHARINDEX(0x01, @USERSID)
	SET @USERSID = SUBSTRING(@USERSID, @I, 85-@I+1)
	-- Get User Groups Sid
	SET @I = 0
	WHILE (@I<@USERGROUPSCOUNT)
	BEGIN
		SET @APP = SUBSTRING(@TOKEN,(@I+1)*85+1,85) --GET USER GROUP TOKEN PORTION
		SET @INDEX = CHARINDEX(0x01, @APP) -- FIND TOKEN START (0x01)
		SET @APP = SUBSTRING(@APP, @INDEX, 85-@INDEX+1) -- EXTRACT USER GROUP SID
		INSERT INTO @USERGROUPS (objectSid) VALUES (@APP)
		SET @I = @I + 1
	END
END
ELSE
BEGIN
	SET @USERSID = @TOKEN
END

-- CHECK IF IS A NON-MEMBER
IF @GROUPTYPE = 0 -- STORE GROUP
	EXEC dbo.GetStoreGroupSidMembers 0, @GROUPOBJECTSID, @NETSQLAZMANMODE, @LDAPPATH, @member_cur OUTPUT
ELSE -- APPLICATON GROUP
	EXEC dbo.GetApplicationGroupSidMembers 0, @GROUPOBJECTSID, @NETSQLAZMANMODE, @LDAPPATH, @member_cur OUTPUT

FETCH NEXT FROM @member_cur INTO @memberSid
WHILE @@FETCH_STATUS = 0
BEGIN
	IF @memberSid = @USERSID
	BEGIN
		CLOSE @member_cur
		DEALLOCATE @member_cur
		SELECT CONVERT(bit, 0) -- true
		RETURN
	END		
	SELECT @COUNT =  COUNT(*)  FROM @USERGROUPS WHERE objectSid = @memberSid
	IF @COUNT>0
	BEGIN
		CLOSE @member_cur
		DEALLOCATE @member_cur
		SELECT CONVERT(bit, 0) -- true
		RETURN
	END		
	FETCH NEXT FROM @member_cur INTO @memberSid
END
CLOSE @member_cur
DEALLOCATE @member_cur

-- CHECK IF IS A MEMBER
IF @GROUPTYPE = 0 -- STORE GROUP
	EXEC dbo.GetStoreGroupSidMembers 1, @GROUPOBJECTSID, @NETSQLAZMANMODE, @LDAPPATH, @member_cur OUTPUT
ELSE -- APPLICATON GROUP
	EXEC dbo.GetApplicationGroupSidMembers 1, @GROUPOBJECTSID, @NETSQLAZMANMODE, @LDAPPATH, @member_cur OUTPUT

FETCH NEXT FROM @member_cur INTO @memberSid
WHILE @@FETCH_STATUS = 0
BEGIN
	IF @memberSid = @USERSID
	BEGIN
		CLOSE @member_cur
		DEALLOCATE @member_cur
		SELECT CONVERT(bit,1) -- true
		RETURN
	END		
	SELECT @COUNT =  COUNT(*)  FROM @USERGROUPS WHERE objectSid = @memberSid
	IF @COUNT>0
	BEGIN
		CLOSE @member_cur
		DEALLOCATE @member_cur
		SELECT CONVERT(bit, 1) -- true
		RETURN
	END		
	FETCH NEXT FROM @member_cur INTO @memberSid
END
CLOSE @member_cur
DEALLOCATE @member_cur

SELECT CONVERT(bit, 0) -- true
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[helplogins](@rolename nvarchar(128))
AS

CREATE TABLE #temptable (
	[DBRole] sysname NOT NULL ,
	[MemberName] sysname NOT NULL ,
	[MemberSID] varbinary(85) NULL
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
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ApplicationGroupMembersTable](
	[ApplicationGroupMemberId] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationGroupId] [int] NOT NULL,
	[objectSid] [varbinary](85) NOT NULL,
	[WhereDefined] [tinyint] NOT NULL,
	[IsMember] [bit] NOT NULL,
 CONSTRAINT [PK_GroupMembers] PRIMARY KEY CLUSTERED 
(
	[ApplicationGroupMemberId] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE UNIQUE NONCLUSTERED INDEX [ApplicationGroupMembers_ApplicationGroupId_ObjectSid_IsMember_Unique_Index] ON [dbo].[ApplicationGroupMembersTable] 
(
	[ApplicationGroupId] ASC,
	[objectSid] ASC,
	[IsMember] ASC
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ApplicationGroupMembers] ON [dbo].[ApplicationGroupMembersTable] 
(
	[ApplicationGroupId] ASC,
	[objectSid] ASC
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AuthorizationsTable](
	[AuthorizationId] [int] IDENTITY(1,1) NOT NULL,
	[ItemId] [int] NOT NULL,
	[ownerSid] [varbinary](85) NOT NULL,
	[ownerSidWhereDefined] [tinyint] NOT NULL,
	[objectSid] [varbinary](85) NOT NULL,
	[objectSidWhereDefined] [tinyint] NOT NULL,
	[AuthorizationType] [tinyint] NOT NULL,
	[ValidFrom] [datetime] NULL,
	[ValidTo] [datetime] NULL,
 CONSTRAINT [PK_Authorizations] PRIMARY KEY CLUSTERED 
(
	[AuthorizationId] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE NONCLUSTERED INDEX [IX_Authorizations] ON [dbo].[AuthorizationsTable] 
(
	[ItemId] ASC,
	[objectSid] ASC
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Authorizations_1] ON [dbo].[AuthorizationsTable] 
(
	[ItemId] ASC,
	[objectSid] ASC,
	[objectSidWhereDefined] ASC,
	[AuthorizationType] ASC,
	[ValidFrom] ASC,
	[ValidTo] ASC
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ItemAttributesTable](
	[ItemAttributeId] [int] IDENTITY(1,1) NOT NULL,
	[ItemId] [int] NOT NULL,
	[AttributeKey] [nvarchar](255) NOT NULL,
	[AttributeValue] [nvarchar](4000) NOT NULL,
 CONSTRAINT [PK_ItemAttributes] PRIMARY KEY CLUSTERED 
(
	[ItemAttributeId] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE UNIQUE NONCLUSTERED INDEX [ItemAttributes_AuhorizationId_AttributeKey_Unique_Index] ON [dbo].[ItemAttributesTable] 
(
	[ItemId] ASC,
	[AttributeKey] ASC
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ItemAttributes] ON [dbo].[ItemAttributesTable] 
(
	[ItemId] ASC,
	[AttributeKey] ASC
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AuthorizationAttributesTable](
	[AuthorizationAttributeId] [int] IDENTITY(1,1) NOT NULL,
	[AuthorizationId] [int] NOT NULL,
	[AttributeKey] [nvarchar](255) NOT NULL,
	[AttributeValue] [nvarchar](4000) NOT NULL,
 CONSTRAINT [PK_AuthorizationAttributes] PRIMARY KEY CLUSTERED 
(
	[AuthorizationAttributeId] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE UNIQUE NONCLUSTERED INDEX [AuthorizationAttributes_AuhorizationId_AttributeKey_Unique_Index] ON [dbo].[AuthorizationAttributesTable] 
(
	[AuthorizationId] ASC,
	[AttributeKey] ASC
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_AuthorizationAttributes] ON [dbo].[AuthorizationAttributesTable] 
(
	[AuthorizationId] ASC,
	[AttributeKey] ASC
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ItemsTable](
	[ItemId] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationId] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](1024) NOT NULL,
	[ItemType] [tinyint] NOT NULL,
	[BizRuleId] [int] NULL,
 CONSTRAINT [PK_Items] PRIMARY KEY CLUSTERED 
(
	[ItemId] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE UNIQUE NONCLUSTERED INDEX [Items_ApplicationId_Name_Unique_Index] ON [dbo].[ItemsTable] 
(
	[Name] ASC,
	[ApplicationId] ASC
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Items] ON [dbo].[ItemsTable] 
(
	[ApplicationId] ASC,
	[Name] ASC
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[StorePermissionsTable](
	[StorePermissionId] [int] IDENTITY(1,1) NOT NULL,
	[StoreId] [int] NOT NULL,
	[SqlUserOrRole] [nvarchar](128) NOT NULL,
	[IsSqlRole] [bit] NOT NULL,
	[NetSqlAzManFixedServerRole] [tinyint] NOT NULL,
 CONSTRAINT [PK_StorePermissions] PRIMARY KEY CLUSTERED 
(
	[StorePermissionId] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE NONCLUSTERED INDEX [IX_StorePermissions] ON [dbo].[StorePermissionsTable] 
(
	[StoreId] ASC
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_StorePermissions_1] ON [dbo].[StorePermissionsTable] 
(
	[StoreId] ASC,
	[SqlUserOrRole] ASC,
	[NetSqlAzManFixedServerRole] ASC
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ApplicationsTable](
	[ApplicationId] [int] IDENTITY(1,1) NOT NULL,
	[StoreId] [int] NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](1024) NOT NULL,
 CONSTRAINT [PK_Applications] PRIMARY KEY CLUSTERED 
(
	[ApplicationId] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE UNIQUE NONCLUSTERED INDEX [Applications_StoreId_Name_Unique_Index] ON [dbo].[ApplicationsTable] 
(
	[Name] ASC,
	[StoreId] ASC
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Applications] ON [dbo].[ApplicationsTable] 
(
	[ApplicationId] ASC,
	[Name] ASC
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[StoreAttributesTable](
	[StoreAttributeId] [int] IDENTITY(1,1) NOT NULL,
	[StoreId] [int] NOT NULL,
	[AttributeKey] [nvarchar](255) NOT NULL,
	[AttributeValue] [nvarchar](4000) NOT NULL,
 CONSTRAINT [PK_StoreAttributes] PRIMARY KEY CLUSTERED 
(
	[StoreAttributeId] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE NONCLUSTERED INDEX [IX_StoreAttributes] ON [dbo].[StoreAttributesTable] 
(
	[StoreId] ASC,
	[AttributeKey] ASC
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [StoreAttributes_AuhorizationId_AttributeKey_Unique_Index] ON [dbo].[StoreAttributesTable] 
(
	[StoreId] ASC,
	[AttributeKey] ASC
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[StoreGroupsTable](
	[StoreGroupId] [int] IDENTITY(1,1) NOT NULL,
	[StoreId] [int] NOT NULL,
	[objectSid] [varbinary](85) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](1024) NOT NULL,
	[LDapQuery] [nvarchar](4000) NULL,
	[GroupType] [tinyint] NOT NULL,
 CONSTRAINT [PK_StoreGroups] PRIMARY KEY CLUSTERED 
(
	[StoreGroupId] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE NONCLUSTERED INDEX [IX_StoreGroups] ON [dbo].[StoreGroupsTable] 
(
	[StoreId] ASC,
	[objectSid] ASC
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [StoreGroups_StoreId_Name_Unique_Index] ON [dbo].[StoreGroupsTable] 
(
	[StoreId] ASC,
	[Name] ASC
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ApplicationAttributesTable](
	[ApplicationAttributeId] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationId] [int] NOT NULL,
	[AttributeKey] [nvarchar](255) NOT NULL,
	[AttributeValue] [nvarchar](4000) NOT NULL,
 CONSTRAINT [PK_ApplicationAttributes] PRIMARY KEY CLUSTERED 
(
	[ApplicationAttributeId] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE UNIQUE NONCLUSTERED INDEX [ApplicationAttributes_AuhorizationId_AttributeKey_Unique_Index] ON [dbo].[ApplicationAttributesTable] 
(
	[ApplicationId] ASC,
	[AttributeKey] ASC
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ApplicationAttributes] ON [dbo].[ApplicationAttributesTable] 
(
	[ApplicationId] ASC,
	[AttributeKey] ASC
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ApplicationGroupsTable](
	[ApplicationGroupId] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationId] [int] NOT NULL,
	[objectSid] [varbinary](85) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](1024) NOT NULL,
	[LDapQuery] [nvarchar](4000) NULL,
	[GroupType] [tinyint] NOT NULL,
 CONSTRAINT [PK_Groups] PRIMARY KEY CLUSTERED 
(
	[ApplicationGroupId] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE UNIQUE NONCLUSTERED INDEX [ApplicationGroups_ApplicationId_Name_Unique_Index] ON [dbo].[ApplicationGroupsTable] 
(
	[ApplicationId] ASC,
	[Name] ASC
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ApplicationGroups] ON [dbo].[ApplicationGroupsTable] 
(
	[ApplicationId] ASC,
	[Name] ASC
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ApplicationGroups_1] ON [dbo].[ApplicationGroupsTable] 
(
	[objectSid] ASC
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ApplicationPermissionsTable](
	[ApplicationPermissionId] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationId] [int] NOT NULL,
	[SqlUserOrRole] [nvarchar](128) NOT NULL,
	[IsSqlRole] [bit] NOT NULL,
	[NetSqlAzManFixedServerRole] [tinyint] NOT NULL,
 CONSTRAINT [PK_ApplicationPermissions] PRIMARY KEY CLUSTERED 
(
	[ApplicationPermissionId] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE NONCLUSTERED INDEX [IX_ApplicationPermissions] ON [dbo].[ApplicationPermissionsTable] 
(
	[ApplicationId] ASC
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ApplicationPermissions_1] ON [dbo].[ApplicationPermissionsTable] 
(
	[ApplicationId] ASC,
	[SqlUserOrRole] ASC,
	[NetSqlAzManFixedServerRole] ASC
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[StoreGroupMembersTable](
	[StoreGroupMemberId] [int] IDENTITY(1,1) NOT NULL,
	[StoreGroupId] [int] NOT NULL,
	[objectSid] [varbinary](85) NOT NULL,
	[WhereDefined] [tinyint] NOT NULL,
	[IsMember] [bit] NOT NULL,
 CONSTRAINT [PK_StoreGroupMembers] PRIMARY KEY CLUSTERED 
(
	[StoreGroupMemberId] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
CREATE NONCLUSTERED INDEX [IX_StoreGroupMembers] ON [dbo].[StoreGroupMembersTable] 
(
	[StoreGroupId] ASC,
	[objectSid] ASC
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [StoreGroupMembers_StoreGroupId_ObjectSid_IsMember_Unique_Index] ON [dbo].[StoreGroupMembersTable] 
(
	[StoreGroupId] ASC,
	[objectSid] ASC,
	[IsMember] ASC
) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
/* 
   @ROLEID = { 0 READERS, 1 USERS, 2 MANAGERS}
*/
CREATE FUNCTION [dbo].[CheckStorePermissions](@STOREID int, @ROLEID tinyint)
RETURNS bit
AS
BEGIN
DECLARE @RESULT bit
IF @STOREID IS NULL OR @ROLEID IS NULL
	SET @RESULT = 0	
ELSE
BEGIN
	IF EXISTS (
		SELECT     dbo.StorePermissionsTable.StoreId
		FROM         dbo.ApplicationsTable RIGHT OUTER JOIN
		                      dbo.StoresTable ON dbo.ApplicationsTable.StoreId = dbo.StoresTable.StoreId LEFT OUTER JOIN
		                      dbo.StorePermissionsTable ON dbo.StoresTable.StoreId = dbo.StorePermissionsTable.StoreId LEFT OUTER JOIN
		                      dbo.ApplicationPermissionsTable ON dbo.ApplicationsTable.ApplicationId = dbo.ApplicationPermissionsTable.ApplicationId
		WHERE 
		IS_MEMBER('db_owner')=1 OR IS_MEMBER('NetSqlAzMan_Administrators')=1 OR 
		(@ROLEID = 0 AND IS_MEMBER('NetSqlAzMan_Readers')=1 OR 
		@ROLEID = 1 AND IS_MEMBER('NetSqlAzMan_Users')=1 OR 
		@ROLEID = 2 AND IS_MEMBER('NetSqlAzMan_Managers')=1) AND
		(
		(dbo.StorePermissionsTable.StoreId = @STOREID AND dbo.StorePermissionsTable.NetSqlAzManFixedServerRole >= @ROLEID AND 
		(SUSER_SNAME(SUSER_SID())=StorePermissionsTable.SqlUserOrRole AND StorePermissionsTable.IsSqlRole = 0 OR
		IS_MEMBER(StorePermissionsTable.SqlUserOrRole)=1 AND StorePermissionsTable.IsSqlRole = 1)) OR
	
		(@ROLEID = 0 AND dbo.StoresTable.StoreId = @STOREID AND dbo.ApplicationPermissionsTable.NetSqlAzManFixedServerRole >= @ROLEID AND 
		(SUSER_SNAME(SUSER_SID())=ApplicationPermissionsTable.SqlUserOrRole AND ApplicationPermissionsTable.IsSqlRole = 0
		OR IS_MEMBER(ApplicationPermissionsTable.SqlUserOrRole)=1 AND ApplicationPermissionsTable.IsSqlRole = 1))))
	SET @RESULT = 1
	ELSE
	SET @RESULT = 0
END
RETURN @RESULT
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
/* 
   @ROLEID = { 0 READERS, 1 USERS, 2 MANAGERS}
*/
CREATE FUNCTION [dbo].[CheckApplicationPermissions](@APPLICATIONID int, @ROLEID tinyint)
RETURNS bit
AS
BEGIN
DECLARE @RESULT bit
IF @APPLICATIONID IS NULL OR @ROLEID IS NULL
	SET @RESULT = 0	
ELSE
BEGIN
	IF EXISTS (
		SELECT     dbo.ApplicationPermissionsTable.ApplicationId
		FROM         dbo.ApplicationsTable INNER JOIN
		                      dbo.StoresTable ON dbo.ApplicationsTable.StoreId = dbo.StoresTable.StoreId LEFT OUTER JOIN
		                      dbo.StorePermissionsTable ON dbo.StoresTable.StoreId = dbo.StorePermissionsTable.StoreId LEFT OUTER JOIN
		                      dbo.ApplicationPermissionsTable ON dbo.ApplicationsTable.ApplicationId = dbo.ApplicationPermissionsTable.ApplicationId
		WHERE
		IS_MEMBER('db_owner')=1 OR IS_MEMBER('NetSqlAzMan_Administrators')=1 OR 
		(@ROLEID = 0 AND IS_MEMBER('NetSqlAzMan_Readers')=1 OR 
		@ROLEID = 1 AND IS_MEMBER('NetSqlAzMan_Users')=1 OR 
		@ROLEID = 2 AND IS_MEMBER('NetSqlAzMan_Managers')=1) AND
		(
		(dbo.ApplicationPermissionsTable.ApplicationId = @APPLICATIONID AND dbo.ApplicationPermissionsTable.NetSqlAzManFixedServerRole >= @ROLEID AND 
		(SUSER_SNAME(SUSER_SID())=ApplicationPermissionsTable.SqlUserOrRole AND ApplicationPermissionsTable.IsSqlRole = 0
		OR IS_MEMBER(ApplicationPermissionsTable.SqlUserOrRole)=1 AND ApplicationPermissionsTable.IsSqlRole = 1)) OR
	
		dbo.ApplicationsTable.ApplicationId = @APPLICATIONID AND 
		(dbo.StorePermissionsTable.StoreId = dbo.ApplicationsTable.StoreId AND dbo.StorePermissionsTable.NetSqlAzManFixedServerRole >= @ROLEID AND 
		(SUSER_SNAME(SUSER_SID())=StorePermissionsTable.SqlUserOrRole AND StorePermissionsTable.IsSqlRole = 0 OR
		IS_MEMBER(StorePermissionsTable.SqlUserOrRole)=1 AND StorePermissionsTable.IsSqlRole = 1))

))
	
	SET @RESULT = 1
	ELSE
	SET @RESULT = 0
END
RETURN @RESULT
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[RevokeStoreAccess] (
	@StoreId int,
	@SqlUserOrRole sysname,
	@NetSqlAzManFixedServerRole tinyint)
AS
IF EXISTS(SELECT StoreId FROM dbo.StoresTable WHERE StoreId = @StoreId) AND (dbo.CheckStorePermissions(@StoreId, 2) = 1 AND @NetSqlAzManFixedServerRole BETWEEN 0 AND 1 OR (IS_MEMBER('db_owner')=1 OR IS_MEMBER('NetSqlAzMan_Administrators')=1) AND @NetSqlAzManFixedServerRole = 2)
BEGIN
	IF EXISTS(SELECT * FROM dbo.StorePermissionsTable WHERE StoreId = @StoreId AND SqlUserOrRole = @SqlUserOrRole AND NetSqlAzManFixedServerRole = @NetSqlAzManFixedServerRole)
		DELETE FROM dbo.StorePermissionsTable WHERE StoreId = @StoreId AND SqlUserOrRole = @SqlUserOrRole AND NetSqlAzManFixedServerRole = @NetSqlAzManFixedServerRole
	ELSE
		RAISERROR ('Permission not found. Revoke Store Access ignored.', -1, -1)
END
ELSE
	RAISERROR ('Store NOT Found or Store permission denied.', 16, 1)
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[GrantStoreAccess] (
	@StoreId int,
	@SqlUserOrRole sysname,
	@NetSqlAzManFixedServerRole tinyint)
AS
IF EXISTS(SELECT StoreId FROM dbo.StoresTable WHERE StoreId = @StoreId) AND (dbo.CheckStorePermissions(@StoreId, 2) = 1 AND @NetSqlAzManFixedServerRole BETWEEN 0 AND 1 OR (IS_MEMBER('db_owner')=1 OR IS_MEMBER('NetSqlAzMan_Administrators')=1) AND @NetSqlAzManFixedServerRole = 2)
BEGIN
	DECLARE @MEMBERUID int
	IF NOT (@NetSqlAzManFixedServerRole BETWEEN 0 AND 2)
	BEGIN
		RAISERROR ('NetSqlAzManFixedServerRole must be 0, 1 or 2 (Reader, User, Manager).', 16, 1)
		RETURN -1
	END
	 -- CHECK MEMBER NAME (ATTEMPT ADDING IMPLICIT ROW FOR NT NAME) --
	DECLARE @IsSqlRoleInt int
	DECLARE @IsNtGroupInt bit
	DECLARE @IsSqlRole bit
	SELECT @MEMBERUID = uid, @IsSqlRoleInt = issqlrole, @IsNtGroupInt = isntgroup  from sysusers where sid = SUSER_SID(@SqlUserOrRole) and isaliased = 0
	IF @IsSqlRoleInt = 1 OR @IsNtGroupInt = 1
		SET @IsSqlRole = 1
	ELSE
		SET @IsSqlRole = 0
	IF @MEMBERUID IS NULL
	BEGIN
		RAISERROR ('Sql User/Role Not Found. Grant Store Access ignored.', -1, -1)
		RETURN 0
	END
	IF EXISTS(SELECT * FROM dbo.StorePermissionsTable WHERE StoreId = @StoreId AND SqlUserOrRole = @SqlUserOrRole AND NetSqlAzManFixedServerRole = @NetSqlAzManFixedServerRole)
		BEGIN
		RAISERROR ('NetSqlAzManFixedServerRole updated.', -1, -1)
		RETURN 0
		END
	ELSE
		BEGIN
		INSERT INTO dbo.StorePermissionsTable (StoreId, SqlUserOrRole, IsSqlRole, NetSqlAzManFixedServerRole) VALUES (@StoreId, @SqlUserOrRole, @IsSqlRole, @NetSqlAzManFixedServerRole)
		RETURN SCOPE_IDENTITY()
		END
END
ELSE
	RAISERROR ('Store NOT Found or Store permission denied.', 16, 1)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE TRIGGER [dbo].[ApplicationGroupDeleteTrigger] ON [dbo].[ApplicationGroupsTable] 
FOR DELETE 
AS
DECLARE @DELETEDOBJECTSID int
DECLARE applicationgroups_cur CURSOR FAST_FORWARD FOR SELECT objectSid FROM deleted
OPEN applicationgroups_cur
FETCH NEXT from applicationgroups_cur INTO @DELETEDOBJECTSID
WHILE @@FETCH_STATUS = 0
BEGIN
	DELETE FROM dbo.ApplicationGroupMembersTable WHERE objectSid = @DELETEDOBJECTSID AND WhereDefined = 1
	DELETE FROM dbo.AuthorizationsTable WHERE objectSid = @DELETEDOBJECTSID AND objectSidWhereDefined = 1
	FETCH NEXT from applicationgroups_cur INTO @DELETEDOBJECTSID
END
CLOSE applicationgroups_cur
DEALLOCATE applicationgroups_cur
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE TRIGGER [dbo].[ItemsHierarchyTrigger] ON [dbo].[ItemsHierarchyTable] 
FOR INSERT, UPDATE
AS
DECLARE @INSERTEDITEMID int
DECLARE @INSERTEDMEMBEROFITEMID int

DECLARE itemhierarchy_cur CURSOR FAST_FORWARD FOR SELECT ItemId, MemberOfItemId FROM inserted
OPEN itemhierarchy_cur
FETCH NEXT from itemhierarchy_cur INTO @INSERTEDITEMID, @INSERTEDMEMBEROFITEMID
WHILE @@FETCH_STATUS = 0
BEGIN
	IF UPDATE(ItemId) AND NOT EXISTS (SELECT ItemId FROM dbo.ItemsTable WHERE ItemsTable.ItemId = @INSERTEDITEMID) 
	 BEGIN
	  RAISERROR ('ItemId NOT FOUND into dbo.ItemsTable', 16, 1)
	  ROLLBACK TRANSACTION
	 END
	
	IF UPDATE(MemberOfItemId) AND NOT EXISTS (SELECT ItemId FROM dbo.ItemsTable WHERE ItemsTable.ItemId = @INSERTEDMEMBEROFITEMID)
	 BEGIN
	  RAISERROR ('MemberOfItemId NOT FOUND into dbo.ItemsTable', 16, 1)
	  ROLLBACK TRANSACTION
	 END
	FETCH NEXT from itemhierarchy_cur INTO @INSERTEDITEMID, @INSERTEDMEMBEROFITEMID
END
CLOSE itemhierarchy_cur
DEALLOCATE itemhierarchy_cur
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[ItemDeleteTrigger] ON [dbo].[ItemsTable] 
FOR DELETE 
AS
DECLARE @DELETEDITEMID int
DECLARE @BIZRULEID int
DECLARE items_cur CURSOR FAST_FORWARD FOR SELECT ItemId, BizRuleId FROM deleted
OPEN items_cur
FETCH NEXT from items_cur INTO @DELETEDITEMID, @BIZRULEID
WHILE @@FETCH_STATUS = 0
BEGIN
	DELETE FROM dbo.ItemsHierarchyTable WHERE ItemId = @DELETEDITEMID OR MemberOfItemId = @DELETEDITEMID
	IF @BIZRULEID IS NOT NULL
		DELETE FROM dbo.BizRulesTable WHERE BizRuleId = @BIZRULEID
	FETCH NEXT from items_cur INTO @DELETEDITEMID, @BIZRULEID
END
CLOSE items_cur
DEALLOCATE items_cur
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE TRIGGER [dbo].[StoreGroupDeleteTrigger] ON [dbo].[StoreGroupsTable] 
FOR DELETE 
AS
DECLARE @DELETEDOBJECTSID int
DECLARE storegroups_cur CURSOR FAST_FORWARD FOR SELECT objectSid FROM deleted
OPEN storegroups_cur
FETCH NEXT from storegroups_cur INTO @DELETEDOBJECTSID
WHILE @@FETCH_STATUS = 0
BEGIN
	DELETE FROM dbo.StoreGroupMembersTable WHERE objectSid = @DELETEDOBJECTSID AND WhereDefined = 0
	DELETE FROM dbo.ApplicationGroupMembersTable WHERE objectSid = @DELETEDOBJECTSID AND WhereDefined = 0
	DELETE FROM dbo.AuthorizationsTable WHERE objectSid = @DELETEDOBJECTSID AND objectSidWhereDefined = 0
	FETCH NEXT from storegroups_cur INTO @DELETEDOBJECTSID
END
CLOSE storegroups_cur
DEALLOCATE storegroups_cur
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BizRuleDelete]
(
	@BizRuleId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT BizRuleId FROM dbo.BizRulesTable WHERE BizRuleId = @BizRuleId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
	DELETE FROM [dbo].[BizRulesTable] WHERE [BizRuleId] = @BizRuleId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BizRuleInsert]
(
	@BizRuleSource text,
	@BizRuleLanguage tinyint,
	@CompiledAssembly image
)
AS
INSERT INTO [dbo].[BizRulesTable] ([BizRuleSource], [BizRuleLanguage], [CompiledAssembly]) VALUES (@BizRuleSource, @BizRuleLanguage, @CompiledAssembly);
RETURN SCOPE_IDENTITY()
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE FUNCTION [dbo].[Stores] ()
RETURNS TABLE 
AS
RETURN
	SELECT dbo.StoresTable.* FROM dbo.StoresTable
	WHERE dbo.CheckStorePermissions(StoresTable.StoreId, 0) = 1
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[StoreInsert]
(
	@Name nvarchar(255),
	@Description nvarchar(1024)
)
AS
INSERT INTO [dbo].[StoresTable] ([Name], [Description]) VALUES (@Name, @Description);
RETURN SCOPE_IDENTITY()
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE FUNCTION [dbo].[Applications] ()
RETURNS TABLE
AS
RETURN
	SELECT * FROM dbo.ApplicationsTable
	WHERE dbo.CheckApplicationPermissions(ApplicationId, 0) = 1
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[StoreGroupDelete]
(
	@Original_StoreGroupId int,
	@StoreId int
)
AS
IF dbo.CheckStorePermissions(@StoreId, 2) = 1
	DELETE FROM [dbo].[StoreGroupsTable] WHERE [StoreGroupId] = @Original_StoreGroupId AND [StoreId] = @StoreId
ELSE
	RAISERROR ('Store permission denied.', 16, 1)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[StoreGroupInsert]
(
	@StoreId int,
	@objectSid varbinary(85),
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@LDapQuery nvarchar(4000),
	@GroupType tinyint
)
AS
IF dbo.CheckStorePermissions(@StoreId, 2) = 1
BEGIN
	INSERT INTO [dbo].[StoreGroupsTable] ([StoreId], [objectSid], [Name], [Description], [LDapQuery], [GroupType]) VALUES (@StoreId, @objectSid, @Name, @Description, @LDapQuery, @GroupType);
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Store permission denied.', 16, 1)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[StoreGroupUpdate]
(
	@StoreId int,
	@objectSid varbinary(85),
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@LDapQuery nvarchar(4000),
	@GroupType tinyint,
	@Original_StoreGroupId int
)
AS
IF dbo.CheckStorePermissions(@StoreId, 2) = 1
	UPDATE [dbo].[StoreGroupsTable] SET [objectSid] = @objectSid, [Name] = @Name, [Description] = @Description, [LDapQuery] = @LDapQuery, [GroupType] = @GroupType WHERE [StoreGroupId] = @Original_StoreGroupId AND StoreId = @StoreId
ELSE
	RAISERROR ('Store permission denied.', 16, 1)
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[StorePermissionDelete]
(
	@StorePermissionId int,
	@StoreId int
)
AS
IF EXISTS(SELECT StoreId FROM dbo.Stores() WHERE StoreId = @StoreId) AND dbo.CheckStorePermissions(@StoreId, 2) = 1
	DELETE FROM dbo.StorePermissionsTable WHERE StorePermissionId = @StorePermissionId AND StoreId = @StoreId
ELSE
	RAISERROR ('Store permission denied.', 16, 1)
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[StorePermissionInsert]
(
	@StoreId int,
	@SqlUserOrRole nvarchar(128),
	@IsSqlRole bit,
	@NetSqlAzManFixedServerRole tinyint
)
AS
IF EXISTS(SELECT StoreId FROM dbo.Stores() WHERE StoreId = @StoreId) AND dbo.CheckStorePermissions(@StoreId, 2) = 1
BEGIN
	INSERT INTO dbo.StorePermissionsTable (StoreId, SqlUserOrRole, IsSqlRole, NetSqlAzManFixedServerRole) VALUES (@StoreId, @SqlUserOrRole, @IsSqlRole, @NetSqlAzManFixedServerRole)
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Store permission denied.', 16, 1)
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE FUNCTION [dbo].[StorePermissions]()
RETURNS TABLE 
AS  
RETURN
	SELECT     dbo.StorePermissionsTable.*
	FROM         dbo.StorePermissionsTable INNER JOIN
	                      dbo.Stores() Stores ON dbo.StorePermissionsTable.StoreId = Stores.StoreId
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE FUNCTION [dbo].[ApplicationAttributes] ()
RETURNS TABLE
AS
RETURN 
	SELECT     dbo.ApplicationAttributesTable.*
	FROM         dbo.ApplicationAttributesTable INNER JOIN
	                      dbo.Applications() Applications ON dbo.ApplicationAttributesTable.ApplicationId = Applications.ApplicationId
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ApplicationAttributeInsert]
(
	@ApplicationId int,
	@AttributeKey nvarchar(255),
	@AttributeValue nvarchar(4000)
)
AS
IF EXISTS(SELECT ApplicationId FROM dbo.Applications() WHERE ApplicationId = @ApplicationId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
BEGIN
	INSERT INTO [dbo].[ApplicationAttributesTable] ([ApplicationId], [AttributeKey], [AttributeValue]) VALUES (@ApplicationId, @AttributeKey, @AttributeValue)
	RETURN SCOPE_IDENTITY()
END
ELSE
BEGIN
	RAISERROR ('Application Permission denied.', 16, 1)
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ApplicationGroupInsert]
(
	@ApplicationId int,
	@objectSid varbinary(85),
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@LDapQuery nvarchar(4000),
	@GroupType tinyint
)
AS
IF EXISTS(SELECT ApplicationId FROM dbo.Applications() WHERE ApplicationId = @ApplicationId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
BEGIN
	INSERT INTO [dbo].[ApplicationGroupsTable] ([ApplicationId], [objectSid], [Name], [Description], [LDapQuery], [GroupType]) VALUES (@ApplicationId, @objectSid, @Name, @Description, @LDapQuery, @GroupType)
	RETURN SCOPE_IDENTITY()
END
ELSE	
	RAISERROR ('Application permission denied.', 16, 1)
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE FUNCTION [dbo].[ApplicationGroups] ()
RETURNS TABLE
AS
RETURN
	SELECT     dbo.ApplicationGroupsTable.*
	FROM         dbo.ApplicationGroupsTable INNER JOIN
	                      dbo.Applications() Applications ON dbo.ApplicationGroupsTable.ApplicationId = Applications.ApplicationId
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE FUNCTION [dbo].[ApplicationPermissions]()
RETURNS TABLE 
AS  
RETURN
	SELECT     dbo.ApplicationPermissionsTable.*
	FROM         dbo.ApplicationPermissionsTable INNER JOIN
	                      dbo.Applications() Applications ON dbo.ApplicationPermissionsTable.ApplicationId = Applications.ApplicationId
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[GrantApplicationAccess] (
	@ApplicationId int,
	@SqlUserOrRole sysname,
	@NetSqlAzManFixedServerRole tinyint)
AS
DECLARE @StoreId int
SET @StoreId = (SELECT StoreId FROM dbo.Applications() WHERE ApplicationId = @ApplicationId)
IF EXISTS(SELECT ApplicationId FROM dbo.ApplicationsTable WHERE ApplicationId = @ApplicationId) AND (dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1 AND @NetSqlAzManFixedServerRole BETWEEN 0 AND 1 OR dbo.CheckStorePermissions(@StoreId, 2) = 1 AND @NetSqlAzManFixedServerRole = 2)
BEGIN
	DECLARE @MEMBERUID int
	IF NOT (@NetSqlAzManFixedServerRole BETWEEN 0 AND 2)
	BEGIN
		RAISERROR ('NetSqlAzManFixedServerRole must be 0, 1 or 2 (Reader, User, Manager).', 16, 1)
		RETURN -1
	END
	 -- CHECK MEMBER NAME (ATTEMPT ADDING IMPLICIT ROW FOR NT NAME) --
	DECLARE @IsSqlRoleInt int
	DECLARE @IsNtGroupInt bit
	DECLARE @IsSqlRole bit
	SELECT @MEMBERUID = uid, @IsSqlRoleInt = issqlrole, @IsNtGroupInt = isntgroup  from sysusers where sid = SUSER_SID(@SqlUserOrRole) and isaliased = 0
	IF @IsSqlRoleInt = 1 OR @IsNtGroupInt = 1
		SET @IsSqlRole = 1
	ELSE
		SET @IsSqlRole = 0
	IF @MEMBERUID IS NULL
	BEGIN
		RAISERROR ('Sql User/Role Not Found. Grant Store Access ignored.', -1, -1)
		RETURN 0
	END
	IF EXISTS(SELECT * FROM dbo.ApplicationPermissionsTable WHERE ApplicationId = @ApplicationId AND SqlUserOrRole = @SqlUserOrRole AND NetSqlAzManFixedServerRole = @NetSqlAzManFixedServerRole)
		BEGIN
		RAISERROR ('NetSqlAzManFixedServerRole updated.', -1, -1)
		RETURN 0
		END
	ELSE
		BEGIN
		INSERT INTO dbo.ApplicationPermissionsTable (ApplicationId, SqlUserOrRole, IsSqlRole, NetSqlAzManFixedServerRole) VALUES (@ApplicationId, @SqlUserOrRole, @IsSqlRole, @NetSqlAzManFixedServerRole)
		RETURN SCOPE_IDENTITY()
		END
END
ELSE
	RAISERROR ('Application NOT Found or Application permission denied.', 16, 1)
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[RevokeApplicationAccess] (
	@ApplicationId int,
	@SqlUserOrRole sysname,
	@NetSqlAzManFixedServerRole tinyint)
AS
DECLARE @StoreId int
SET @StoreId = (SELECT StoreId FROM dbo.Applications() WHERE ApplicationId = @ApplicationId)
IF EXISTS(SELECT ApplicationId FROM dbo.ApplicationsTable WHERE ApplicationId = @ApplicationId) AND (dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1 AND @NetSqlAzManFixedServerRole BETWEEN 0 AND 1 OR dbo.CheckStorePermissions(@StoreId, 2) = 1 AND @NetSqlAzManFixedServerRole = 2)
BEGIN
	IF EXISTS(SELECT * FROM dbo.ApplicationPermissionsTable WHERE ApplicationId = @ApplicationId AND SqlUserOrRole = @SqlUserOrRole AND NetSqlAzManFixedServerRole = @NetSqlAzManFixedServerRole)
		DELETE FROM dbo.ApplicationPermissionsTable WHERE ApplicationId = @ApplicationId AND SqlUserOrRole = @SqlUserOrRole AND NetSqlAzManFixedServerRole = @NetSqlAzManFixedServerRole
	ELSE
		RAISERROR ('Permission not found. Revoke Application Access ignored.', -1, -1)
END
ELSE
	RAISERROR ('Application NOT Found or Application permission denied.', 16, 1)
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[ApplicationPermissionDelete]
(
	@ApplicationPermissionId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ApplicationId FROM dbo.Applications() WHERE ApplicationId = @ApplicationId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
	DELETE FROM dbo.ApplicationPermissionsTable WHERE ApplicationPermissionId = @ApplicationPermissionId AND ApplicationId = @ApplicationId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[ApplicationPermissionInsert]
(
	@ApplicationId int,
	@SqlUserOrRole nvarchar(128),
	@IsSqlRole bit,
	@NetSqlAzManFixedServerRole tinyint
)
AS
IF EXISTS(SELECT ApplicationId FROM dbo.Applications() WHERE ApplicationId = @ApplicationId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
BEGIN
	INSERT INTO dbo.ApplicationPermissionsTable (ApplicationId, SqlUserOrRole, IsSqlRole, NetSqlAzManFixedServerRole) VALUES (@ApplicationId, @SqlUserOrRole, @IsSqlRole, @NetSqlAzManFixedServerRole)
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ItemInsert]
(
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@ItemType tinyint,
	@BizRuleId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ApplicationId FROM dbo.Applications() WHERE ApplicationId = @ApplicationId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
BEGIN
	INSERT INTO [dbo].[ItemsTable] ([ApplicationId], [Name], [Description], [ItemType], [BizRuleId]) VALUES (@ApplicationId, @Name, @Description, @ItemType, @BizRuleId)
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE FUNCTION [dbo].[Items] ()
RETURNS TABLE
AS
RETURN
	SELECT     dbo.ItemsTable.*
	FROM         dbo.ItemsTable INNER JOIN
	                      dbo.Applications() Applications ON dbo.ItemsTable.ApplicationId = Applications.ApplicationId
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE FUNCTION [dbo].[StoreAttributes] ()
RETURNS TABLE
AS
RETURN
	SELECT     dbo.StoreAttributesTable.*
	FROM         dbo.StoreAttributesTable INNER JOIN
	                      dbo.Stores() Stores ON dbo.StoreAttributesTable.StoreId = Stores.StoreId
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE FUNCTION [dbo].[StoreGroups] ()
RETURNS TABLE
AS
RETURN
	SELECT     dbo.StoreGroupsTable.*
	FROM         dbo.Stores() Stores INNER JOIN
	                      dbo.StoreGroupsTable ON Stores.StoreId = dbo.StoreGroupsTable.StoreId
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[StoreUpdate]
(
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@Original_StoreId int
)
AS
IF EXISTS(Select StoreId FROM dbo.Stores() WHERE StoreId = @Original_StoreId) AND dbo.CheckStorePermissions(@Original_StoreId, 2) = 1
	UPDATE [dbo].[StoresTable] SET [Name] = @Name, [Description] = @Description WHERE [StoreId] = @Original_StoreId
ELSE
	RAISERROR ('Store permission denied.', 16, 1)
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[ApplicationInsert]
(
	@StoreId int,
	@Name nvarchar(255),
	@Description nvarchar(1024)
)
AS
IF EXISTS(SELECT StoreId FROM dbo.Stores() WHERE StoreId = @StoreId) AND dbo.CheckStorePermissions(@StoreId, 2) = 1
BEGIN
	INSERT INTO [dbo].[ApplicationsTable] ([StoreId], [Name], [Description]) VALUES (@StoreId, @Name, @Description);
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Store permission denied.', 16, 1)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[StoreAttributeInsert]
(
	@StoreId int,
	@AttributeKey nvarchar(255),
	@AttributeValue nvarchar(4000)
)
AS
IF EXISTS(Select StoreId FROM dbo.Stores() WHERE StoreId = @StoreId) AND dbo.CheckStorePermissions(@StoreId, 2) = 1
BEGIN
	INSERT INTO [dbo].[StoreAttributesTable] ([StoreId], [AttributeKey], [AttributeValue]) VALUES (@StoreId, @AttributeKey, @AttributeValue);
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Store permission denied.', 16, 1)
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[StoreDelete]
(
	@Original_StoreId int
)
AS
IF EXISTS(Select StoreId FROM dbo.Stores() WHERE StoreId = @Original_StoreId) AND dbo.CheckStorePermissions(@Original_StoreId, 2) = 1
	DELETE FROM [dbo].[StoresTable] WHERE [StoreId] = @Original_StoreId
ELSE
	RAISERROR ('Store permission denied.', 16, 1)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ApplicationsView]
AS
SELECT     Stores.StoreId, Stores.Name AS StoreName, Stores.Description AS StoreDescription, Applications.ApplicationId, Applications.Name AS ApplicationName, 
                      Applications.Description AS ApplicationDescription
FROM         dbo.Applications() Applications INNER JOIN
                      dbo.Stores() Stores ON Applications.StoreId = Stores.StoreId
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[ApplicationUpdate]
(
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@Original_ApplicationId int
)
AS
IF EXISTS(SELECT ApplicationId FROM dbo.Applications() WHERE ApplicationId = @Original_ApplicationId) AND dbo.CheckApplicationPermissions(@Original_ApplicationId, 2) = 1
	UPDATE [dbo].[ApplicationsTable] SET [Name] = @Name, [Description] = @Description WHERE [ApplicationId] = @Original_ApplicationId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[ApplicationDelete]
(
	@StoreId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ApplicationId FROM dbo.Applications() WHERE ApplicationId = @ApplicationId) AND dbo.CheckStorePermissions(@StoreId, 2) = 1
	DELETE FROM [dbo].[ApplicationsTable] WHERE [ApplicationId] = @ApplicationId AND [StoreId] = @StoreId
ELSE
	RAISERROR ('Store permission denied', 16, 1)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ApplicationAttributeUpdate]
(
	@ApplicationId int,
	@AttributeKey nvarchar(255),
	@AttributeValue nvarchar(4000),
	@Original_ApplicationAttributeId int
)
AS
IF EXISTS(SELECT ApplicationAttributeId FROM dbo.ApplicationAttributes() WHERE ApplicationAttributeId = @Original_ApplicationAttributeId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
	UPDATE [dbo].[ApplicationAttributesTable] SET [AttributeKey] = @AttributeKey, [AttributeValue] = @AttributeValue WHERE [ApplicationAttributeId] = @Original_ApplicationAttributeId AND [ApplicationId] = @ApplicationId
ELSE
	RAISERROR ('Applicaction Permission denied.', 16, 1)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ApplicationAttributeDelete]
(
	@ApplicationId int,
	@ApplicationAttributeId int
)
AS
IF EXISTS(SELECT ApplicationAttributeId FROM dbo.ApplicationAttributes() WHERE ApplicationAttributeId = @ApplicationAttributeId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
	DELETE FROM [dbo].[ApplicationAttributesTable] WHERE [ApplicationAttributeId] = @ApplicationAttributeId AND [ApplicationId] = @ApplicationId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ApplicationGroupDelete]
(
	@ApplicationGroupId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ApplicationGroupId FROM dbo.ApplicationGroups() WHERE ApplicationGroupId = @ApplicationGroupId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
	DELETE FROM [dbo].[ApplicationGroupsTable] WHERE [ApplicationGroupId] = @ApplicationGroupId AND [ApplicationId] = @ApplicationId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ApplicationGroupUpdate]
(
	@objectSid varbinary(85),
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@LDapQuery nvarchar(4000),
	@GroupType tinyint,
	@Original_ApplicationGroupId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ApplicationGroupId FROM dbo.ApplicationGroups() WHERE ApplicationGroupId = @Original_ApplicationGroupId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
	UPDATE [dbo].[ApplicationGroupsTable] SET [objectSid] = @objectSid, [Name] = @Name, [Description] = @Description, [LDapQuery] = @LDapQuery, [GroupType] = @GroupType WHERE [ApplicationGroupId] = @Original_ApplicationGroupId AND [ApplicationId] = @ApplicationId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[ClearBizRule]
(
	@ItemId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ItemId FROM dbo.Items() WHERE ItemId = @ItemId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
	UPDATE [dbo].[ItemsTable] SET BizRuleId = NULL WHERE [ItemId] = @ItemId AND [ApplicationId] = @ApplicationId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ItemUpdate]
(
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@ItemType tinyint,
	@Original_ItemId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ItemId FROM dbo.Items() WHERE ItemId = @Original_ItemId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
	UPDATE [dbo].[ItemsTable] SET [Name] = @Name, [Description] = @Description, [ItemType] = @ItemType WHERE [ItemId] = @Original_ItemId AND [ApplicationId] = @ApplicationId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ItemDelete]
(
	@ItemId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ItemId FROM dbo.Items() WHERE ItemId = @ItemId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
	DELETE FROM [dbo].[ItemsTable] WHERE [ItemId] = @ItemId AND [ApplicationId] = @ApplicationId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE PROCEDURE [dbo].[ReloadBizRule]
(
	@ItemId int,
	@BizRuleId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ItemId FROM dbo.Items() WHERE ItemId = @ItemId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
	UPDATE [dbo].[ItemsTable] SET BizRuleId = @BizRuleId WHERE [ItemId] = @ItemId AND [ApplicationId] = @ApplicationId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[StoreGroupMemberInsert]
(
	@StoreId int,
	@StoreGroupId int,
	@objectSid varbinary(85),
	@WhereDefined tinyint,
	@IsMember bit
)
AS
IF EXISTS(SELECT StoreGroupId FROM dbo.StoreGroups() WHERE StoreGroupId = @StoreGroupId) AND dbo.CheckStorePermissions(@StoreId, 2) = 1
BEGIN
	INSERT INTO [dbo].[StoreGroupMembersTable] ([StoreGroupId], [objectSid], [WhereDefined], [IsMember]) VALUES (@StoreGroupId, @objectSid, @WhereDefined, @IsMember)
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Store permission denied.', 16, 1)
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE FUNCTION [dbo].[StoreGroupMembers] ()
RETURNS TABLE
AS
RETURN
	SELECT     dbo.StoreGroupMembersTable.*
	FROM         dbo.StoreGroupMembersTable INNER JOIN
	                      dbo.StoreGroups() StoreGroups ON dbo.StoreGroupMembersTable.StoreGroupId = StoreGroups.StoreGroupId
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetStoreGroupSidMembers](@ISMEMBER BIT, @GROUPOBJECTSID VARBINARY(85), @NETSQLAZMANMODE bit, @LDAPPATH nvarchar(4000), @member_cur CURSOR VARYING OUTPUT)
AS
DECLARE @RESULT TABLE (objectSid VARBINARY(85))
DECLARE @GROUPID INT
DECLARE @GROUPTYPE TINYINT
DECLARE @LDAPQUERY nvarchar(4000)
DECLARE @sub_members_cur CURSOR
DECLARE @OBJECTSID VARBINARY(85)
SELECT @GROUPID = StoreGroupId, @GROUPTYPE = GroupType, @LDAPQUERY = LDapQuery FROM dbo.StoreGroups() WHERE objectSid = @GROUPOBJECTSID
IF @GROUPTYPE = 0 -- BASIC
BEGIN
	--memo: WhereDefined can be:0 - Store; 1 - Application; 2 - LDAP; 3 - Local; 4 - Database
	-- Windows SIDs
	INSERT INTO @RESULT (objectSid) 
	SELECT objectSid 
	FROM dbo.StoreGroupMembersTable
	WHERE 
	StoreGroupId = @GROUPID AND IsMember = @ISMEMBER AND
	((@NETSQLAZMANMODE = 0 AND (WhereDefined = 2 OR WhereDefined = 4)) OR (@NETSQLAZMANMODE = 1 AND WhereDefined BETWEEN 2 AND 4))
	-- Store Groups Members
	DECLARE @MemberObjectSid VARBINARY(85)
	DECLARE @MemberType bit
	DECLARE @NotMemberType bit
	DECLARE nested_Store_groups_cur CURSOR LOCAL FAST_FORWARD FOR
		SELECT objectSid, IsMember FROM dbo.StoreGroupMembersTable WHERE StoreGroupId = @GROUPID AND WhereDefined = 0
	
	OPEN nested_Store_groups_cur
	FETCH NEXT FROM nested_Store_groups_cur INTO @MemberObjectSid, @MemberType
	WHILE @@FETCH_STATUS = 0
	BEGIN
	        -- recursive call
		IF @ISMEMBER = 1
		BEGIN
			IF @MemberType = 0 
				SET @NotMemberType = 0
			ELSE
				SET @NotMemberType = 1
		END
		ELSE
		BEGIN
			IF @MemberType = 0 
				SET @NotMemberType = 1
			ELSE
				SET @NotMemberType = 0
		END
		EXEC dbo.GetStoreGroupSidMembers @NotMemberType, @MemberObjectSid, @NETSQLAZMANMODE, @LDAPPATH, @sub_members_cur OUTPUT
		FETCH NEXT FROM @sub_members_cur INTO @OBJECTSID
		WHILE @@FETCH_STATUS=0
		BEGIN
			INSERT INTO @RESULT VALUES (@OBJECTSID)
			FETCH NEXT FROM @sub_members_cur INTO @OBJECTSID
		END		
		CLOSE @sub_members_cur
		DEALLOCATE @sub_members_cur	

		FETCH NEXT FROM nested_Store_groups_cur INTO @MemberObjectSid, @MemberType
	END
	CLOSE nested_Store_groups_cur
	DEALLOCATE nested_Store_groups_cur
END
ELSE IF @GROUPTYPE = 1 AND @ISMEMBER = 1 -- LDAP QUERY
BEGIN
	EXEC dbo.ExecuteLDAPQuery @LDAPPATH, @LDAPQUERY, @sub_members_cur OUTPUT
	FETCH NEXT FROM @sub_members_cur INTO @OBJECTSID
	WHILE @@FETCH_STATUS=0
	BEGIN
		INSERT INTO @RESULT (objectSid) VALUES (@OBJECTSID)
		FETCH NEXT FROM @sub_members_cur INTO @OBJECTSID
	END
	CLOSE @sub_members_cur
	DEALLOCATE @sub_members_cur
END
SET @member_cur = CURSOR STATIC FORWARD_ONLY FOR SELECT * FROM @RESULT
OPEN @member_cur

GO

SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE FUNCTION [dbo].[ApplicationGroupMembers] ()
RETURNS TABLE
AS
RETURN
	SELECT     dbo.ApplicationGroupMembersTable.*
	FROM         dbo.ApplicationGroups() ApplicationGroups INNER JOIN
	                      dbo.ApplicationGroupMembersTable ON ApplicationGroups.ApplicationGroupId = dbo.ApplicationGroupMembersTable.ApplicationGroupId
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ApplicationGroupMemberInsert]
(
	@ApplicationGroupId int,
	@objectSid varbinary(85),
	@WhereDefined tinyint,
	@IsMember bit,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ApplicationGroupId FROM dbo.ApplicationGroups() WHERE ApplicationGroupId = @ApplicationGroupId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
BEGIN
	INSERT INTO [dbo].[ApplicationGroupMembersTable] ([ApplicationGroupId], [objectSid], [WhereDefined], [IsMember]) VALUES (@ApplicationGroupId, @objectSid, @WhereDefined, @IsMember)
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AuthorizationInsert]
(
	@ItemId int,
	@ownerSid varbinary(85),
	@ownerSidWhereDefined tinyint,
	@objectSid varbinary(85),
	@objectSidWhereDefined tinyint,
	@AuthorizationType tinyint,
	@ValidFrom datetime,
	@ValidTo datetime,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ItemId FROM dbo.Items() WHERE ItemId = @ItemId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
BEGIN
	INSERT INTO [dbo].[AuthorizationsTable] ([ItemId], [ownerSid], [ownerSidWhereDefined], [objectSid], [objectSidWhereDefined], [AuthorizationType], [ValidFrom], [ValidTo]) VALUES (@ItemId, @ownerSid, @ownerSidWhereDefined, @objectSid, @objectSidWhereDefined, @AuthorizationType, @ValidFrom, @ValidTo)
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Object:  Stored Procedure dbo.CreateDelegate    Script Date: 19/05/2006 19.11.19 ******/
CREATE PROCEDURE [dbo].[CreateDelegate](@ITEMID INT, @OWNERSID VARBINARY(85), @OWNERSIDWHEREDEFINED TINYINT, @DELEGATEDUSERSID VARBINARY(85), @SIDWHEREDEFINED TINYINT, @AUTHORIZATIONTYPE TINYINT, @VALIDFROM DATETIME, @VALIDTO DATETIME, @AUTHORIZATIONID INT OUTPUT)
AS
DECLARE @APPLICATIONID int
SELECT @APPLICATIONID = ApplicationId FROM dbo.Items() WHERE ItemId = @ItemId
IF @APPLICATIONID IS NOT NULL AND dbo.CheckApplicationPermissions(@ApplicationId, 1) = 1
BEGIN
	INSERT INTO dbo.AuthorizationsTable (ItemId, ownerSid, ownerSidWhereDefined, objectSid, objectSidWhereDefined, AuthorizationType, ValidFrom, ValidTo)
		VALUES (@ITEMID, @OWNERSID, @OWNERSIDWHEREDEFINED, @DELEGATEDUSERSID, @SIDWHEREDEFINED, @AUTHORIZATIONTYPE, @VALIDFROM, @VALIDTO)
	SET @AUTHORIZATIONID = SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Item NOT Found or Application permission denied.', 16, 1)
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE FUNCTION [dbo].[Authorizations] ()
RETURNS TABLE
AS
RETURN
	SELECT     dbo.AuthorizationsTable.*
	FROM         dbo.AuthorizationsTable INNER JOIN
	                      dbo.Items() Items ON dbo.AuthorizationsTable.ItemId = Items.ItemId
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE FUNCTION [dbo].[ItemAttributes] ()
RETURNS TABLE
AS
RETURN
	SELECT     dbo.ItemAttributesTable.*
	FROM         dbo.ItemAttributesTable INNER JOIN
	                      dbo.Items() Items ON dbo.ItemAttributesTable.ItemId = Items.ItemId
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ItemAttributeInsert]
(
	@ItemId int,
	@AttributeKey nvarchar(255),
	@AttributeValue nvarchar(4000),
	@ApplicationId int
)
AS
IF EXISTS(SELECT ItemId FROM dbo.Items() WHERE ItemId = @ItemId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
BEGIN
	INSERT INTO [dbo].[ItemAttributesTable] ([ItemId], [AttributeKey], [AttributeValue]) VALUES (@ItemId, @AttributeKey, @AttributeValue)
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[StoreAttributesView]
AS
SELECT     Stores.StoreId, Stores.Name, Stores.Description, StoreAttributes.StoreAttributeId, StoreAttributes.AttributeKey, StoreAttributes.AttributeValue
FROM         dbo.Stores() Stores INNER JOIN
                      dbo.StoreAttributes() StoreAttributes ON Stores.StoreId = StoreAttributes.StoreId
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ApplicationAttributesView]
AS
SELECT     Applications.ApplicationId, Applications.StoreId, Applications.Name, Applications.Description, ApplicationAttributes.ApplicationAttributeId, 
                      ApplicationAttributes.AttributeKey, ApplicationAttributes.AttributeValue
FROM         dbo.Applications() Applications INNER JOIN
                      dbo.ApplicationAttributes() ApplicationAttributes ON Applications.ApplicationId = ApplicationAttributes.ApplicationId
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[StoreAttributeDelete]
(
	@StoreId int,
	@StoreAttributeId int
)
AS
IF EXISTS(Select StoreAttributeId FROM dbo.StoreAttributes() WHERE StoreAttributeId = @StoreAttributeId) AND dbo.CheckStorePermissions(@StoreId, 2) = 1
	DELETE FROM [dbo].[StoreAttributesTable] WHERE [StoreAttributeId] = @StoreAttributeId AND [StoreId] = @StoreId
ELSE
	RAISERROR ('Store permission denied.', 16, 1)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[StoreAttributeUpdate]
(
	@StoreId int,
	@AttributeKey nvarchar(255),
	@AttributeValue nvarchar(4000),
	@Original_StoreAttributeId int
)
AS
IF EXISTS(Select StoreAttributeId FROM dbo.StoreAttributes() WHERE StoreAttributeId = @Original_StoreAttributeId) AND dbo.CheckStorePermissions(@StoreId, 2) = 1
	UPDATE [dbo].[StoreAttributesTable] SET [AttributeKey] = @AttributeKey, [AttributeValue] = @AttributeValue WHERE [StoreAttributeId] = @Original_StoreAttributeId AND [StoreId] = @StoreId 
ELSE
	RAISERROR ('Store permission denied.', 16, 1)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Andrea Ferendeles
-- Create date: 13/04/2006
-- Description:	Get Name From Sid
-- =============================================
CREATE FUNCTION [dbo].[GetNameFromSid] (@StoreName nvarchar(255), @ApplicationName nvarchar(255), @Sid varbinary(85), @SidWhereDefined tinyint)
RETURNS nvarchar(255)
AS
BEGIN

DECLARE @Name nvarchar(255)
SET @Name = NULL

IF (@SidWhereDefined=0) --Store
BEGIN
SET @Name = (SELECT TOP 1 Name FROM dbo.StoreGroups() WHERE objectSid = @Sid)
END
ELSE IF (@SidWhereDefined=1) --Application 
BEGIN
SET @Name = (SELECT TOP 1 Name FROM dbo.ApplicationGroups() WHERE objectSid = @Sid)
END
ELSE IF (@SidWhereDefined=2 OR @SidWhereDefined=3) --LDAP or LOCAL
BEGIN
SET @Name = (SELECT Suser_Sname(@Sid))
END
ELSE IF (@SidWhereDefined=4) --Database
BEGIN
SET @Name = (SELECT DBUserName FROM dbo.GetDBUsers(@StoreName, @ApplicationName, @Sid, NULL))
END
IF (@Name IS NULL)
BEGIN
	SET @Name = @Sid
END
RETURN @Name
END
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE FUNCTION [dbo].[ItemsHierarchy] ()
RETURNS TABLE
AS
RETURN
	SELECT     dbo.ItemsHierarchyTable.*
	FROM         dbo.ItemsHierarchyTable INNER JOIN
	                      dbo.Items() Items ON dbo.ItemsHierarchyTable.ItemId = Items.ItemId
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[BizRules]()
RETURNS TABLE
AS
RETURN
	SELECT     dbo.BizRulesTable.*
	FROM         dbo.BizRulesTable INNER JOIN
	                      dbo.Items() Items ON dbo.BizRulesTable.BizRuleId = Items.BizRuleId
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ItemsHierarchyInsert]
(
	@ItemId int,
	@MemberOfItemId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ItemId FROM dbo.Items() WHERE ItemId = @ItemId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
BEGIN
	INSERT INTO [dbo].[ItemsHierarchyTable] ([ItemId], [MemberOfItemId]) VALUES (@ItemId, @MemberOfItemId)
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ItemsHierarchyDelete]
(
	@ItemId int,
	@MemberOfItemId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ItemId FROM dbo.Items() WHERE ItemId = @ItemId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
	DELETE FROM [dbo].[ItemsHierarchyTable] WHERE [ItemId] = @ItemId AND [MemberOfItemId] = @MemberOfItemId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[GetApplicationGroupSidMembers](@ISMEMBER BIT, @GROUPOBJECTSID VARBINARY(85), @NETSQLAZMANMODE bit, @LDAPPATH nvarchar(4000), @member_cur CURSOR VARYING OUTPUT)
AS
DECLARE @RESULT TABLE (objectSid VARBINARY(85))
DECLARE @GROUPID INT
DECLARE @GROUPTYPE TINYINT
DECLARE @LDAPQUERY nvarchar(4000)
DECLARE @sub_members_cur CURSOR
DECLARE @OBJECTSID VARBINARY(85)
SELECT @GROUPID = ApplicationGroupId, @GROUPTYPE = GroupType, @LDAPQUERY = LDapQuery FROM ApplicationGroupsTable WHERE objectSid = @GROUPOBJECTSID
IF @GROUPTYPE = 0 -- BASIC
BEGIN
	--memo: WhereDefined can be:0 - Store; 1 - Application; 2 - LDAP; 3 - Local; 4 - Database
	-- Windows SIDs
	INSERT INTO @RESULT (objectSid) 
	SELECT objectSid 
	FROM dbo.ApplicationGroupMembersTable
	WHERE 
	ApplicationGroupId = @GROUPID AND IsMember = @ISMEMBER AND
	((@NETSQLAZMANMODE = 0 AND (WhereDefined = 2 OR WhereDefined = 4)) OR (@NETSQLAZMANMODE = 1 AND WhereDefined BETWEEN 2 AND 4))
	-- Store Groups Members
	DECLARE @MemberObjectSid VARBINARY(85)
	DECLARE @MemberType bit
	DECLARE @NotMemberType bit
	DECLARE nested_Store_groups_cur CURSOR LOCAL FAST_FORWARD FOR
		SELECT objectSid, IsMember FROM dbo.ApplicationGroupMembersTable WHERE ApplicationGroupId = @GROUPID AND WhereDefined = 0
	
	OPEN nested_Store_groups_cur
	FETCH NEXT FROM nested_Store_groups_cur INTO @MemberObjectSid, @MemberType
	WHILE @@FETCH_STATUS = 0
	BEGIN
	        -- recursive call
		IF @ISMEMBER = 1
		BEGIN
			IF @MemberType = 0 
				SET @NotMemberType = 0
			ELSE
				SET @NotMemberType = 1
		END
		ELSE
		BEGIN
			IF @MemberType = 0 
				SET @NotMemberType = 1
			ELSE
				SET @NotMemberType = 0
		END
		EXEC dbo.GetStoreGroupSidMembers @NotMemberType, @MemberObjectSid, @NETSQLAZMANMODE, @LDAPPATH, @sub_members_cur OUTPUT
		FETCH NEXT FROM @sub_members_cur INTO @OBJECTSID
		WHILE @@FETCH_STATUS=0
		BEGIN
			INSERT INTO @RESULT VALUES (@OBJECTSID)
			FETCH NEXT FROM @sub_members_cur INTO @OBJECTSID
		END		
		CLOSE @sub_members_cur
		DEALLOCATE @sub_members_cur			

		FETCH NEXT FROM nested_Store_groups_cur INTO @MemberObjectSid, @MemberType
	END
	CLOSE nested_Store_groups_cur
	DEALLOCATE nested_Store_groups_cur
	
	-- Application Groups Members
	DECLARE nested_Application_groups_cur CURSOR LOCAL FAST_FORWARD FOR
		SELECT objectSid, IsMember FROM dbo.ApplicationGroupMembersTable WHERE ApplicationGroupId = @GROUPID AND WhereDefined = 1
	
	OPEN nested_Application_groups_cur
	FETCH NEXT FROM nested_Application_groups_cur INTO @MemberObjectSid, @MemberType
	WHILE @@FETCH_STATUS = 0
	BEGIN
	        -- recursive call
		IF @ISMEMBER = 1
		BEGIN
			IF @MemberType = 0 
				SET @NotMemberType = 0
			ELSE
				SET @NotMemberType = 1
		END
		ELSE
		BEGIN
			IF @MemberType = 0 
				SET @NotMemberType = 1
			ELSE
				SET @NotMemberType = 0
		END
		EXEC dbo.GetApplicationGroupSidMembers @NotMemberType, @MemberObjectSid, @NETSQLAZMANMODE, @LDAPPATH, @sub_members_cur OUTPUT
		FETCH NEXT FROM @sub_members_cur INTO @OBJECTSID
		WHILE @@FETCH_STATUS=0
		BEGIN
			INSERT INTO @RESULT VALUES (@OBJECTSID)
			FETCH NEXT FROM @sub_members_cur INTO @OBJECTSID
		END		
		CLOSE @sub_members_cur
		DEALLOCATE @sub_members_cur	

		FETCH NEXT FROM nested_Application_groups_cur INTO @MemberObjectSid, @MemberType
	END
	CLOSE nested_Application_groups_cur
	DEALLOCATE nested_Application_groups_cur
	END
ELSE IF @GROUPTYPE = 1 AND @ISMEMBER = 1 -- LDAP QUERY
BEGIN
	EXEC dbo.ExecuteLDAPQuery @LDAPPATH, @LDAPQUERY, @sub_members_cur OUTPUT
	FETCH NEXT FROM @sub_members_cur INTO @OBJECTSID
	WHILE @@FETCH_STATUS=0
	BEGIN
		INSERT INTO @RESULT (objectSid) VALUES (@OBJECTSID)
		FETCH NEXT FROM @sub_members_cur INTO @OBJECTSID
	END
	CLOSE @sub_members_cur
	DEALLOCATE @sub_members_cur
END
SET @member_cur = CURSOR STATIC FORWARD_ONLY FOR SELECT * FROM @RESULT
OPEN @member_cur

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[StoreGroupMemberDelete]
(
	@StoreId int,
	@StoreGroupMemberId int
)
AS
IF EXISTS(SELECT StoreGroupMemberId FROM dbo.StoreGroupMembers() WHERE StoreGroupMemberId = @StoreGroupMemberId) AND dbo.CheckStorePermissions(@StoreId, 2) = 1
	DELETE FROM [dbo].[StoreGroupMembersTable] WHERE [StoreGroupMemberId] = @StoreGroupMemberId
ELSE
	RAISERROR ('Store permission denied.', 16, 1)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[StoreGroupMemberUpdate]
(
	@StoreId int,
	@StoreGroupId int,
	@objectSid varbinary(85),
	@WhereDefined tinyint,
	@IsMember bit,
	@Original_StoreGroupMemberId int
)
AS
IF EXISTS(SELECT StoreGroupMemberId FROM dbo.StoreGroupMembers() WHERE StoreGroupMemberId = @Original_StoreGroupMemberId) AND dbo.CheckStorePermissions(@StoreId, 2) = 1
	UPDATE [dbo].[StoreGroupMembersTable] SET [StoreGroupId] = @StoreGroupId, [objectSid] = @objectSid, [WhereDefined] = @WhereDefined, [IsMember] = @IsMember WHERE [StoreGroupMemberId] = @Original_StoreGroupMemberId
ELSE
	RAISERROR ('Store permission denied.', 16, 1)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ApplicationGroupMemberDelete]
(
	@ApplicationGroupMemberId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ApplicationGroupMemberId FROM dbo.ApplicationGroupMembers() WHERE ApplicationGroupMemberId = @ApplicationGroupMemberId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
	DELETE FROM [dbo].[ApplicationGroupMembersTable] WHERE [ApplicationGroupMemberId] = @ApplicationGroupMemberId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ApplicationGroupMemberUpdate]
(
	@ApplicationGroupId int,
	@objectSid varbinary(85),
	@WhereDefined tinyint,
	@IsMember bit,
	@Original_ApplicationGroupMemberId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ApplicationGroupMemberId FROM dbo.ApplicationGroupMembers() WHERE ApplicationGroupMemberId = @Original_ApplicationGroupMemberId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
	UPDATE [dbo].[ApplicationGroupMembersTable] SET [objectSid] = @objectSid, [WhereDefined] = @WhereDefined, [IsMember] = @IsMember WHERE [ApplicationGroupMemberId] = @Original_ApplicationGroupMemberId
ELSE	
	RAISERROR ('Application permission denied.', 16, 1)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AuthorizationDelete]
(
	@AuthorizationId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT AuthorizationId FROM dbo.Authorizations() WHERE AuthorizationId = @AuthorizationId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
	DELETE FROM [dbo].[AuthorizationsTable] WHERE [AuthorizationId] = @AuthorizationId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AuthorizationUpdate]
(
	@ItemId int,
	@ownerSid varbinary(85),
	@ownerSidWhereDefined tinyint,
	@objectSid varbinary(85),
	@objectSidWhereDefined tinyint,
	@AuthorizationType tinyint,
	@ValidFrom datetime,
	@ValidTo datetime,
	@Original_AuthorizationId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT AuthorizationId FROM dbo.Authorizations() WHERE AuthorizationId = @Original_AuthorizationId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
	UPDATE [dbo].[AuthorizationsTable] SET [ownerSid] = @ownerSid, [ownerSidWhereDefined] = @ownerSidWhereDefined, [objectSid] = @objectSid, [objectSidWhereDefined] = @objectSidWhereDefined, [AuthorizationType] = @AuthorizationType, [ValidFrom] = @ValidFrom, [ValidTo] = @ValidTo WHERE [AuthorizationId] = @Original_AuthorizationId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DeleteDelegate](@AUTHORIZATIONID INT, @OWNERSID VARBINARY(85))
AS
DECLARE @APPLICATIONID int
SELECT @APPLICATIONID = Items.ApplicationId FROM dbo.Items() Items INNER JOIN dbo.Authorizations() Authorizations ON Items.ItemId = Authorizations.ItemId WHERE Authorizations.AuthorizationId = @AUTHORIZATIONID
IF @APPLICATIONID IS NOT NULL AND dbo.CheckApplicationPermissions(@ApplicationId, 1) = 1
	DELETE FROM dbo.AuthorizationsTable WHERE AuthorizationId = @AUTHORIZATIONID AND ownerSid = @OWNERSID
ELSE
	RAISERROR ('Item NOT Found or Application permission denied.', 16, 1)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ItemAttributeDelete]
(
	@ItemAttributeId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ItemAttributeId FROM dbo.ItemAttributes() WHERE ItemAttributeId = @ItemAttributeId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
	DELETE FROM [dbo].[ItemAttributesTable] WHERE [ItemAttributeId] = @ItemAttributeId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[ItemAttributeUpdate]
(
	@AttributeKey nvarchar(255),
	@AttributeValue nvarchar(4000),
	@Original_ItemAttributeId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ItemAttributeId FROM dbo.ItemAttributes() WHERE ItemAttributeId = @Original_ItemAttributeId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
	UPDATE [dbo].[ItemAttributesTable] SET [AttributeKey] = @AttributeKey, [AttributeValue] = @AttributeValue WHERE [ItemAttributeId] = @Original_ItemAttributeId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
SET ANSI_NULLS OFF
GO
SET QUOTED_IDENTIFIER OFF
GO
CREATE FUNCTION [dbo].[AuthorizationAttributes] ()
RETURNS TABLE
AS
RETURN
	SELECT     dbo.AuthorizationAttributesTable.*
	FROM         dbo.AuthorizationAttributesTable INNER JOIN
	                      dbo.Authorizations() Authorizations ON dbo.AuthorizationAttributesTable.AuthorizationId = Authorizations.AuthorizationId
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AuthorizationAttributeInsert]
(
	@AuthorizationId int,
	@AttributeKey nvarchar(255),
	@AttributeValue nvarchar(4000),
	@ApplicationId int
)
AS
IF EXISTS(SELECT AuthorizationId FROM dbo.Authorizations() WHERE AuthorizationId = @AuthorizationId) AND dbo.CheckApplicationPermissions(@ApplicationId, 1) = 1
BEGIN
	INSERT INTO [dbo].[AuthorizationAttributesTable] ([AuthorizationId], [AttributeKey], [AttributeValue]) VALUES (@AuthorizationId, @AttributeKey, @AttributeValue)
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[StoreGroupMembersView]
AS
SELECT     StoreGroupMembers.StoreGroupMemberId, StoreGroupMembers.StoreGroupId, StoreGroups.Name AS StoreGroup, dbo.GetNameFromSid(Stores.Name, NULL, 
                      StoreGroupMembers.objectSid, StoreGroupMembers.WhereDefined) AS Name, StoreGroupMembers.objectSid, 
                      CASE WhereDefined WHEN 0 THEN 'Store' WHEN 1 THEN 'Application' WHEN 2 THEN 'LDap' WHEN 3 THEN 'Local' WHEN 4 THEN 'DATABASE' END AS WhereDefined,
                       CASE IsMember WHEN 1 THEN 'Member' WHEN 0 THEN 'Non-Member' END AS MemberType
FROM         dbo.StoreGroupMembers() StoreGroupMembers INNER JOIN
                      dbo.StoreGroups() StoreGroups ON StoreGroupMembers.StoreGroupId = StoreGroups.StoreGroupId INNER JOIN
                      dbo.Stores() Stores ON StoreGroups.StoreId = Stores.StoreId
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ApplicationGroupMembersView]
AS
SELECT     Stores.StoreId, Applications.ApplicationId, ApplicationGroupMembers.ApplicationGroupMemberId, ApplicationGroupMembers.ApplicationGroupId, 
                      ApplicationGroups.Name AS ApplicationGroup, dbo.GetNameFromSid(Stores.Name, Applications.Name, ApplicationGroupMembers.objectSid, 
                      ApplicationGroupMembers.WhereDefined) AS Name, ApplicationGroupMembers.objectSid, 
                      CASE WhereDefined WHEN 0 THEN 'Store' WHEN 1 THEN 'Application' WHEN 2 THEN 'LDap' WHEN 3 THEN 'Local' WHEN 4 THEN 'DATABASE' END AS WhereDefined,
                       CASE IsMember WHEN 1 THEN 'Member' WHEN 0 THEN 'Non-Member' END AS MemberType
FROM         dbo.ApplicationGroupMembers() ApplicationGroupMembers INNER JOIN
                      dbo.ApplicationGroups() ApplicationGroups ON ApplicationGroupMembers.ApplicationGroupId = ApplicationGroups.ApplicationGroupId INNER JOIN
                      dbo.Applications() Applications ON ApplicationGroups.ApplicationId = Applications.ApplicationId INNER JOIN
                      dbo.Stores() Stores ON Applications.StoreId = Stores.StoreId
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[AuthorizationView]
AS
SELECT     Authorizations.AuthorizationId, Authorizations.ItemId, dbo.GetNameFromSid(Stores.Name, Applications.Name, Authorizations.ownerSid, 
                      Authorizations.ownerSidWhereDefined) AS Owner, dbo.GetNameFromSid(Stores.Name, Applications.Name, Authorizations.objectSid, 
                      Authorizations.objectSidWhereDefined) AS Name, Authorizations.objectSid, 
                      CASE objectSidWhereDefined WHEN 0 THEN 'Store' WHEN 1 THEN 'Application' WHEN 2 THEN 'LDAP' WHEN 3 THEN 'Local' WHEN 4 THEN 'DATABASE' END AS SidWhereDefined,
                       CASE AuthorizationType WHEN 0 THEN 'NEUTRAL' WHEN 1 THEN 'ALLOW' WHEN 2 THEN 'DENY' WHEN 3 THEN 'ALLOWWITHDELEGATION' END AS AuthorizationType,
                       Authorizations.ValidFrom, Authorizations.ValidTo
FROM         dbo.Authorizations() Authorizations INNER JOIN
                      dbo.Items() Items ON Authorizations.ItemId = Items.ItemId INNER JOIN
                      dbo.Applications() Applications ON Items.ApplicationId = Applications.ApplicationId INNER JOIN
                      dbo.Stores() Stores ON Applications.StoreId = Stores.StoreId
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ItemsHierarchyView]
AS
SELECT     Items.ItemId, Items.ApplicationId, Items.Name, Items.Description, 
                      CASE Items.ItemType WHEN 0 THEN 'Role' WHEN 1 THEN 'Task' WHEN 2 THEN 'Operation' END AS ItemType, Items_1.ItemId AS MemberItemId, 
                      Items_1.ApplicationId AS MemberApplicationId, Items_1.Name AS MemberName, Items_1.Description AS MemberDescription, 
                      CASE Items_1.ItemType WHEN 0 THEN 'Role' WHEN 1 THEN 'Task' WHEN 2 THEN 'Operation' END AS MemberType
FROM         dbo.Items() Items_1 INNER JOIN
                      dbo.ItemsHierarchy() ItemsHierarchy ON Items_1.ItemId = ItemsHierarchy.ItemId INNER JOIN
                      dbo.Items() Items ON ItemsHierarchy.MemberOfItemId = Items.ItemId INNER JOIN
                      dbo.Applications() Applications ON Items.ApplicationId = Applications.ApplicationId
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[BizRuleView]
AS
SELECT     Items.ItemId, Items.ApplicationId, Items.Name, Items.Description, Items.ItemType, BizRules.BizRuleSource, BizRules.BizRuleLanguage, 
                      BizRules.CompiledAssembly
FROM         dbo.Items() Items INNER JOIN
                      dbo.BizRules() BizRules ON Items.BizRuleId = BizRules.BizRuleId
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[ItemAttributesView]
AS
SELECT     Items.ItemId, Items.ApplicationId, Items.Name, Items.Description, 
                      CASE Items.ItemType WHEN 0 THEN 'Role' WHEN 1 THEN 'Task' WHEN 2 THEN 'Operation' END AS ItemType, ItemAttributes.ItemAttributeId, 
                      ItemAttributes.AttributeKey, ItemAttributes.AttributeValue
FROM         dbo.Items() Items INNER JOIN
                      dbo.ItemAttributes() ItemAttributes ON Items.ItemId = ItemAttributes.ItemId
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[BizRuleUpdate]
(
	@BizRuleSource text,
	@BizRuleLanguage tinyint,
	@CompiledAssembly image,
	@Original_BizRuleId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT BizRuleId FROM dbo.BizRules() WHERE BizRuleId = @Original_BizRuleId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
	UPDATE [dbo].[BizRulesTable] SET [BizRuleSource] = @BizRuleSource, [BizRuleLanguage] = @BizRuleLanguage, [CompiledAssembly] = @CompiledAssembly WHERE [BizRuleId] = @Original_BizRuleId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[CheckAccess] (@ITEMID INT, @USERSID VARBINARY(85), @VALIDFOR DATETIME, @LDAPPATH nvarchar(4000), @AUTHORIZATION_TYPE TINYINT OUTPUT, @NETSQLAZMANMODE BIT, @RETRIEVEATTRIBUTES BIT) 
AS
---------------------------------------------------
-- VARIABLES DECLARATION
-- 0 - Neutral; 1 - Allow; 2 - Deny; 3 - AllowWithDelegation
SET NOCOUNT ON
DECLARE @PARENTITEMID INT
DECLARE @PKID INT
DECLARE @PARENTRESULT TINYINT
DECLARE @APP VARBINARY(85)
DECLARE @members_cur CURSOR
DECLARE @OBJECTSID VARBINARY(85)
DECLARE @ITEM_AUTHORIZATION_TYPE TINYINT
---------------------------------------------------
-- INITIALIZE VARIABLES
SET @ITEM_AUTHORIZATION_TYPE = 0 -- Neutral
SET @AUTHORIZATION_TYPE = 0 -- Neutral
------------------------------------------------------
-- CHECK ACCESS ON PARENTS
-- Get Items Where Item is A Member
DECLARE ItemsWhereIAmAMember_cur CURSOR LOCAL FAST_FORWARD READ_ONLY FOR SELECT MemberOfItemId FROM dbo.ItemsHierarchyTable WHERE ItemId = @ITEMID
OPEN ItemsWhereIAmAMember_cur
FETCH NEXT FROM ItemsWhereIAmAMember_cur INTO @PARENTITEMID
WHILE @@FETCH_STATUS = 0
BEGIN
	-- Recursive Call
	EXEC dbo.CheckAccess @PARENTITEMID, @USERSID, @VALIDFOR, @LDAPPATH, @PARENTRESULT OUTPUT, @NETSQLAZMANMODE, @RETRIEVEATTRIBUTES
	SELECT @AUTHORIZATION_TYPE = dbo.MergeAuthorizations(@AUTHORIZATION_TYPE, @PARENTRESULT)
	FETCH NEXT FROM ItemsWhereIAmAMember_cur INTO @PARENTITEMID
END
CLOSE ItemsWhereIAmAMember_cur
DEALLOCATE ItemsWhereIAmAMember_cur

IF @AUTHORIZATION_TYPE = 3 
BEGIN
	SET @AUTHORIZATION_TYPE = 1 -- AllowWithDelegation becomes Just Allow (if comes from parents)
END
---------------------------------------------
-- GET ITEM ATTRIBUTES
---------------------------------------------
IF @RETRIEVEATTRIBUTES = 1
	INSERT INTO #ATTRIBUTES_TABLE SELECT AttributeKey, AttributeValue, @ITEMID FROM dbo.ItemAttributesTable WHERE ItemId = @ITEMID
---------------------------------------------
-- CHECK ACCESS ON ITEM
-- AuthorizationType can be:  0 - Neutral; 1 - Allow; 2 - Deny; 3 - AllowWithDelegation
-- objectSidWhereDefined can be:0 - Store; 1 - Application; 2 - LDAP; 3 - Local; 4 - Database
DECLARE @PARTIAL_RESULT TINYINT
--CHECK ACCESS FOR USER AUTHORIZATIONS
DECLARE checkaccessonitem_cur CURSOR  LOCAL FAST_FORWARD READ_ONLY FOR 
	SELECT AuthorizationType, AuthorizationId
	FROM dbo.AuthorizationsTable WHERE 
	ItemId = @ITEMID AND
	objectSid = @USERSID AND
	(ValidFrom IS NULL AND ValidTo IS NULL OR
	@VALIDFOR >= ValidFrom  AND ValidTo IS NULL OR
	@VALIDFOR <= ValidTo AND ValidFrom IS NULL OR
	@VALIDFOR BETWEEN ValidFrom AND ValidTo) AND
        AuthorizationType<>0 AND
	((@NETSQLAZMANMODE = 0 AND (objectSidWhereDefined=2 OR objectSidWhereDefined=4)) OR (@NETSQLAZMANMODE = 1 AND objectSidWhereDefined BETWEEN 2 AND 4)) -- if Mode = Administrator SKIP CHECK for local Authorizations

OPEN checkaccessonitem_cur
FETCH NEXT FROM checkaccessonitem_cur INTO @PARTIAL_RESULT, @PKID
WHILE @@FETCH_STATUS = 0
BEGIN
	--CHECK FOR DENY
	IF @PARTIAL_RESULT IS NOT NULL
	BEGIN
		SELECT @AUTHORIZATION_TYPE = dbo.MergeAuthorizations(@AUTHORIZATION_TYPE, @PARTIAL_RESULT)
		SELECT @ITEM_AUTHORIZATION_TYPE  = dbo.MergeAuthorizations(@ITEM_AUTHORIZATION_TYPE, @PARTIAL_RESULT)
		IF @RETRIEVEATTRIBUTES = 1 
			INSERT INTO #ATTRIBUTES_TABLE SELECT AttributeKey, AttributeValue, NULL FROM dbo.AuthorizationAttributesTable WHERE AuthorizationId = @PKID
	END
	ELSE
	BEGIN
		SET @PARTIAL_RESULT = 0 -- NEUTRAL
	END
	FETCH NEXT FROM checkaccessonitem_cur INTO @PARTIAL_RESULT, @PKID
END

CLOSE checkaccessonitem_cur
DEALLOCATE checkaccessonitem_cur

--CHECK ACCESS FOR USER GROUPS AUTHORIZATIONS
DECLARE usergroupsauthz_cur CURSOR LOCAL FAST_FORWARD READ_ONLY FOR 
	SELECT AuthorizationType, AuthorizationID
	FROM dbo.AuthorizationsTable Authorizations INNER JOIN #USERGROUPS usergroups
	ON Authorizations.objectSid = usergroups.objectSid WHERE 
	ItemId = @ITEMID AND
	(ValidFrom IS NULL AND ValidTo IS NULL OR
	@VALIDFOR >= ValidFrom  AND ValidTo IS NULL OR
	@VALIDFOR <= ValidTo AND ValidFrom IS NULL OR
	@VALIDFOR BETWEEN ValidFrom AND ValidTo) AND
        AuthorizationType<>0 AND
	((@NETSQLAZMANMODE = 0 AND (objectSidWhereDefined=2 OR objectSidWhereDefined=4)) OR (@NETSQLAZMANMODE = 1 AND objectSidWhereDefined BETWEEN 2 AND 4)) -- if Mode = Administrator SKIP CHECK for local Authorizations

OPEN usergroupsauthz_cur
FETCH NEXT FROM usergroupsauthz_cur INTO @PARTIAL_RESULT, @PKID
WHILE @@FETCH_STATUS = 0
BEGIN
	IF @PARTIAL_RESULT IS NOT NULL
	BEGIN
		SELECT @AUTHORIZATION_TYPE = dbo.MergeAuthorizations(@AUTHORIZATION_TYPE, @PARTIAL_RESULT)
		SELECT @ITEM_AUTHORIZATION_TYPE = dbo.MergeAuthorizations(@ITEM_AUTHORIZATION_TYPE, @PARTIAL_RESULT)
		IF @RETRIEVEATTRIBUTES = 1
			INSERT INTO #ATTRIBUTES_TABLE SELECT AttributeKey, AttributeValue, NULL FROM dbo.AuthorizationAttributesTable WHERE AuthorizationId = @PKID
	END
	ELSE
	BEGIN
		SET @PARTIAL_RESULT = 0 -- NEUTRAL
	END
	FETCH NEXT FROM usergroupsauthz_cur INTO @PARTIAL_RESULT, @PKID
END

CLOSE usergroupsauthz_cur
DEALLOCATE usergroupsauthz_cur

--CHECK ACCESS FOR STORE/APPLICATION GROUPS AUTHORIZATIONS
DECLARE @GROUPOBJECTSID VARBINARY(85)
DECLARE @GROUPWHEREDEFINED TINYINT
DECLARE @GROUPSIDMEMBERS table (objectSid VARBINARY(85))
DECLARE @ISMEMBER BIT
SET @ISMEMBER = 1
DECLARE groups_authorizations_cur CURSOR LOCAL FAST_FORWARD READ_ONLY 
FOR 	SELECT objectSid, objectSidWhereDefined, AuthorizationType, AuthorizationId FROM dbo.AuthorizationsTable
	WHERE ItemId = @ITEMID AND objectSidWhereDefined BETWEEN 0 AND 1 AND
        AuthorizationType<>0 AND
	(ValidFrom IS NULL AND ValidTo IS NULL OR
	@VALIDFOR >= ValidFrom  AND ValidTo IS NULL OR
	@VALIDFOR <= ValidTo AND ValidFrom IS NULL OR
	@VALIDFOR BETWEEN ValidFrom AND ValidTo)

OPEN groups_authorizations_cur
FETCH NEXT FROM groups_authorizations_cur INTO @GROUPOBJECTSID, @GROUPWHEREDEFINED, @PARTIAL_RESULT, @PKID
WHILE @@FETCH_STATUS=0
BEGIN
SET @ISMEMBER = 1
--check if user is a non-member
IF @GROUPWHEREDEFINED = 0 -- store group members
BEGIN
--store groups members of type 'non-member'
	DELETE FROM @GROUPSIDMEMBERS

	EXEC dbo.GetStoreGroupSidMembers 0, @GROUPOBJECTSID, @NETSQLAZMANMODE, @LDAPPATH, @members_cur OUTPUT -- non-members
	FETCH NEXT FROM @members_cur INTO @OBJECTSID
	WHILE @@FETCH_STATUS=0
	BEGIN
		INSERT INTO @GROUPSIDMEMBERS VALUES (@OBJECTSID)
		FETCH NEXT FROM @members_cur INTO @OBJECTSID
	END
	CLOSE @members_cur
	DEALLOCATE @members_cur

	IF EXISTS(SELECT * FROM @GROUPSIDMEMBERS WHERE objectSid = @USERSID) OR
	     EXISTS(SELECT * FROM @GROUPSIDMEMBERS groupsidmembers INNER JOIN #USERGROUPS usergroups ON groupsidmembers.objectSid = usergroups.objectSid)
	BEGIN
	-- user is a non-member
	SET @ISMEMBER = 0
	END
	IF @ISMEMBER = 1
	BEGIN
		DELETE FROM @GROUPSIDMEMBERS

		EXEC dbo.GetStoreGroupSidMembers 1, @GROUPOBJECTSID, @NETSQLAZMANMODE, @LDAPPATH, @members_cur OUTPUT -- members
		FETCH NEXT FROM @members_cur INTO @OBJECTSID
		WHILE @@FETCH_STATUS=0
		BEGIN
			INSERT INTO @GROUPSIDMEMBERS VALUES (@OBJECTSID)
			FETCH NEXT FROM @members_cur INTO @OBJECTSID
		END
		CLOSE @members_cur
		DEALLOCATE @members_cur

		IF EXISTS (SELECT * FROM @GROUPSIDMEMBERS WHERE objectSid = @USERSID) OR
		     EXISTS (SELECT * FROM @GROUPSIDMEMBERS groupsidmembers INNER JOIN #USERGROUPS usergroups ON groupsidmembers.objectSid = usergroups.ObjectSId)
		BEGIN
		-- user is a member
		SET @ISMEMBER = 1
		END
		ELSE
		BEGIN
		-- user is not present
		SET @ISMEMBER = 0
		END
	END
	-- if a member ... get authorization
	IF @ISMEMBER = 1
	BEGIN
		SET @AUTHORIZATION_TYPE = (SELECT dbo.MergeAuthorizations(@AUTHORIZATION_TYPE, @PARTIAL_RESULT))
		SET @ITEM_AUTHORIZATION_TYPE = (SELECT dbo.MergeAuthorizations(@ITEM_AUTHORIZATION_TYPE, @PARTIAL_RESULT))
		IF @PKID IS NOT NULL AND @RETRIEVEATTRIBUTES = 1
			INSERT INTO #ATTRIBUTES_TABLE SELECT AttributeKey, AttributeValue, NULL FROM dbo.AuthorizationAttributesTable WHERE AuthorizationId = @PKID
	END
END
	ELSE
IF @GROUPWHEREDEFINED = 1 -- application group members
BEGIN
	--application groups members of type 'non-member'
	DELETE FROM @GROUPSIDMEMBERS

	EXEC dbo.GetApplicationGroupSidMembers 0, @GROUPOBJECTSID, @NETSQLAZMANMODE, @LDAPPATH, @members_cur OUTPUT -- non-members
	FETCH NEXT FROM @members_cur INTO @OBJECTSID
	WHILE @@FETCH_STATUS=0
	BEGIN
		INSERT INTO @GROUPSIDMEMBERS VALUES (@OBJECTSID)
		FETCH NEXT FROM @members_cur INTO @OBJECTSID
	END
	CLOSE @members_cur
	DEALLOCATE @members_cur

	IF EXISTS(SELECT * FROM @GROUPSIDMEMBERS WHERE objectSid = @USERSID) OR
	     EXISTS (SELECT* FROM @GROUPSIDMEMBERS groupsidmembers INNER JOIN #USERGROUPS usergroups ON groupsidmembers.objectSid = usergroups.objectSid)
	BEGIN	-- user is a non-member
	SET @ISMEMBER = 0
	END
	IF @ISMEMBER = 1 
	BEGIN
		DELETE FROM @GROUPSIDMEMBERS

		EXEC dbo.GetApplicationGroupSidMembers 1, @GROUPOBJECTSID, @NETSQLAZMANMODE, @LDAPPATH, @members_cur OUTPUT -- members
		FETCH NEXT FROM @members_cur INTO @OBJECTSID
		WHILE @@FETCH_STATUS=0
		BEGIN
			INSERT INTO @GROUPSIDMEMBERS VALUES (@OBJECTSID)
			FETCH NEXT FROM @members_cur INTO @OBJECTSID
		END
		CLOSE @members_cur
		DEALLOCATE @members_cur

		IF EXISTS(SELECT * FROM @GROUPSIDMEMBERS WHERE objectSid = @USERSID) OR
		     EXISTS (SELECT * FROM @GROUPSIDMEMBERS groupsidmembers INNER JOIN #USERGROUPS usergroups ON groupsidmembers.objectSid = usergroups.objectSid)
		BEGIN
		-- user is a member
		SET @ISMEMBER = 1
		END
		ELSE
		BEGIN
		-- user is not present
		SET @ISMEMBER = 0
		END
	END
	-- if a member ... get authorization
	IF @ISMEMBER = 1
	BEGIN
		SET @AUTHORIZATION_TYPE = (SELECT dbo.MergeAuthorizations(@AUTHORIZATION_TYPE, @PARTIAL_RESULT))
		SET @ITEM_AUTHORIZATION_TYPE = (SELECT dbo.MergeAuthorizations(@ITEM_AUTHORIZATION_TYPE, @PARTIAL_RESULT))
		IF @PKID IS NOT NULL AND @RETRIEVEATTRIBUTES = 1 
			INSERT INTO #ATTRIBUTES_TABLE SELECT AttributeKey, AttributeValue, NULL FROM dbo.AuthorizationAttributesTable WHERE AuthorizationId = @PKID
	END
END
	FETCH NEXT FROM groups_authorizations_cur INTO @GROUPOBJECTSID, @GROUPWHEREDEFINED, @PARTIAL_RESULT, @PKID
END
CLOSE groups_authorizations_cur
DEALLOCATE groups_authorizations_cur

-- PREPARE RESULTSET FOR BIZ RULE CHECKING
----------------------------------------------------------------------------------------
INSERT INTO #PARTIAL_RESULTS_TABLE 
SELECT     Items.ItemId, Items.Name, Items.ItemType, @ITEM_AUTHORIZATION_TYPE,BizRules.BizRuleId, BizRules.BizRuleSource, BizRules.BizRuleLanguage
FROM         dbo.ItemsTable Items LEFT OUTER JOIN
                      dbo.BizRulesTable BizRules ON Items.BizRuleId = BizRules.BizRuleId WHERE Items.ItemId = @ITEMID
SET NOCOUNT OFF

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AuthorizationAttributeUpdate]
(
	@AttributeKey nvarchar(255),
	@AttributeValue nvarchar(4000),
	@Original_AuthorizationAttributeId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT AuthorizationAttributeId FROM dbo.AuthorizationAttributes() WHERE AuthorizationAttributeId = @Original_AuthorizationAttributeId) AND dbo.CheckApplicationPermissions(@ApplicationId, 1) = 1
	UPDATE [dbo].[AuthorizationAttributesTable] SET [AttributeKey] = @AttributeKey, [AttributeValue] = @AttributeValue WHERE [AuthorizationAttributeId] = @Original_AuthorizationAttributeId
ELSE
	RAISERROR ('Application permission denied.', 16 ,1)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[AuthorizationAttributeDelete]
(
	@AuthorizationAttributeId int,
	@ApplicationId int
)
AS
IF  EXISTS(SELECT AuthorizationAttributeId FROM dbo.AuthorizationAttributes() WHERE AuthorizationAttributeId = @AuthorizationAttributeId) AND dbo.CheckApplicationPermissions(@ApplicationId, 1) = 1
	DELETE FROM [dbo].[AuthorizationAttributesTable] WHERE [AuthorizationAttributeId] = @AuthorizationAttributeId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[AuthorizationAttributesView]
AS
SELECT     dbo.AuthorizationView.AuthorizationId, dbo.AuthorizationView.ItemId, dbo.AuthorizationView.Owner, dbo.AuthorizationView.Name, dbo.AuthorizationView.objectSid, 
                      dbo.AuthorizationView.SidWhereDefined, dbo.AuthorizationView.AuthorizationType, dbo.AuthorizationView.ValidFrom, dbo.AuthorizationView.ValidTo, 
                      AuthorizationAttributes.AuthorizationAttributeId, AuthorizationAttributes.AttributeKey, AuthorizationAttributes.AttributeValue
FROM         dbo.AuthorizationView INNER JOIN
                      dbo.AuthorizationAttributes() AuthorizationAttributes ON dbo.AuthorizationView.AuthorizationId = AuthorizationAttributes.AuthorizationId
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[DirectCheckAccess] (@STORENAME nvarchar(255), @APPLICATIONNAME nvarchar(255), @ITEMNAME nvarchar(255), @OPERATIONSONLY BIT, @TOKEN IMAGE, @USERGROUPSCOUNT INT, @VALIDFOR DATETIME, @LDAPPATH nvarchar(4000), @AUTHORIZATION_TYPE TINYINT OUTPUT, @RETRIEVEATTRIBUTES BIT) 
AS
--Memo: 0 - Role; 1 - Task; 2 - Operation
SET NOCOUNT ON
DECLARE @STOREID int
DECLARE @APPLICATIONID int
DECLARE @ITEMID INT

-- CHECK STORE EXISTANCE/PERMISSIONS
Select @STOREID = StoreId FROM dbo.Stores() WHERE Name = @STORENAME
IF @STOREID IS NULL
	BEGIN
	RAISERROR ('Store not found or Store permission denied.', 16, 1)
	RETURN 1
	END
-- CHECK APPLICATION EXISTANCE/PERMISSIONS
Select @APPLICATIONID = ApplicationId FROM dbo.Applications() WHERE Name = @APPLICATIONNAME And StoreId = @STOREID
IF @APPLICATIONID IS NULL
	BEGIN
	RAISERROR ('Application not found or Application permission denied.', 16, 1)
	RETURN 1
	END

SELECT @ITEMID = Items.ItemId
	FROM         dbo.Applications() Applications INNER JOIN
	                      dbo.Items() Items ON Applications.ApplicationId = Items.ApplicationId INNER JOIN
	                      dbo.Stores() Stores ON Applications.StoreId = Stores.StoreId
	WHERE     (Stores.StoreId = @STOREID) AND (Applications.ApplicationId = @APPLICATIONID) AND (Items.Name = @ITEMNAME) AND (@OPERATIONSONLY = 1 AND Items.ItemType=2 OR @OPERATIONSONLY = 0)
IF @ITEMID IS NULL
	BEGIN
	RAISERROR ('Item not found.', 16, 1)
	RETURN 1
	END
-- PREPARE RESULTSET FOR BIZ RULE CHECKING
CREATE TABLE #PARTIAL_RESULTS_TABLE 
	(   
		[ItemId] [int] NOT NULL ,
		[ItemName] [nvarchar] (255)  NOT NULL ,
		[ItemType] [tinyint] NOT NULL,
		[AuthorizationType] TINYINT,
		[BizRuleId] [int] NULL ,
		[BizRuleSource] TEXT,
		[BizRuleLanguage] TINYINT
	)
-- PREPARE RESULTSET FOR ATTRIBUTES
IF @RETRIEVEATTRIBUTES = 1
BEGIN
	CREATE TABLE #ATTRIBUTES_TABLE 
	(   
		[AttributeKey] [nvarchar] (255)  NOT NULL,
		[AttributeValue] [nvarchar] (4000)  NOT NULL,
		[ItemId] INT NULL
	)
--------------------------------------------------------------------------------
-- GET STORE AND APPLICATION ATTRIBUTES
--------------------------------------------------------------------------------
	INSERT INTO #ATTRIBUTES_TABLE SELECT AttributeKey, AttributeValue, NULL FROM dbo.StoreAttributesTable StoreAttributes INNER JOIN dbo.StoresTable Stores ON StoreAttributes.StoreId = Stores.StoreId WHERE Stores.StoreId = @STOREID
	INSERT INTO #ATTRIBUTES_TABLE SELECT AttributeKey, AttributeValue, NULL FROM dbo.ApplicationAttributesTable ApplicationAttributes INNER JOIN dbo.ApplicationsTable Applications ON ApplicationAttributes.ApplicationId = Applications.ApplicationId WHERE Applications.ApplicationId = @APPLICATIONID
END
--------------------------------------------------------------------------------
DECLARE @USERSID varbinary(85)
DECLARE @I INT
DECLARE @INDEX INT
DECLARE @APP VARBINARY(85)
DECLARE @SETTINGVALUE nvarchar(255)
DECLARE @NETSQLAZMANMODE bit

SELECT @SETTINGVALUE = SettingValue FROM dbo.Settings WHERE SettingName = 'Mode'
IF @SETTINGVALUE = 'Developer' 
BEGIN
	SET @NETSQLAZMANMODE = 1 
END
ELSE 
BEGIN
	SET @NETSQLAZMANMODE = 0
END

CREATE TABLE #USERGROUPS (objectSid VARBINARY(85))
-- Get User Sid
IF @USERGROUPSCOUNT>0
BEGIN
	SET @USERSID = SUBSTRING(@TOKEN,1,85)
	SET @I = CHARINDEX(0x01, @USERSID)
	SET @USERSID = SUBSTRING(@USERSID, @I, 85-@I+1)
	-- Get User Groups Sid
	SET @I = 0
	WHILE (@I<@USERGROUPSCOUNT)
	BEGIN
		SET @APP = SUBSTRING(@TOKEN,(@I+1)*85+1,85) --GET USER GROUP TOKEN PORTION
		SET @INDEX = CHARINDEX(0x01, @APP) -- FIND TOKEN START (0x01)
		SET @APP = SUBSTRING(@APP, @INDEX, 85-@INDEX+1) -- EXTRACT USER GROUP SID
		INSERT INTO #USERGROUPS (objectSid) VALUES (@APP)
		SET @I = @I + 1
	END
END
ELSE
BEGIN
	SET @USERSID = @TOKEN
END

EXEC dbo.CheckAccess @ITEMID, @USERSID, @VALIDFOR, @LDAPPATH, @AUTHORIZATION_TYPE OUTPUT, @NETSQLAZMANMODE, @RETRIEVEATTRIBUTES
SELECT * FROM #PARTIAL_RESULTS_TABLE
IF @RETRIEVEATTRIBUTES = 1
	SELECT * FROM #ATTRIBUTES_TABLE
DROP TABLE #PARTIAL_RESULTS_TABLE
IF @RETRIEVEATTRIBUTES = 1
	DROP TABLE #ATTRIBUTES_TABLE
DROP TABLE #USERGROUPS
SET NOCOUNT OFF
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE dbo.BuildUserPermissionCache(@STORENAME nvarchar(255), @APPLICATIONNAME nvarchar(255))
AS 
-- Hierarchy
SET NOCOUNT ON
SELECT     Items.Name AS ItemName, Items_1.Name AS ParentItemName
FROM         dbo.Items() Items_1 INNER JOIN
                      dbo.ItemsHierarchy() ItemsHierarchy ON Items_1.ItemId = ItemsHierarchy.MemberOfItemId RIGHT OUTER JOIN
                      dbo.Applications() Applications INNER JOIN
                      dbo.Stores() Stores ON Applications.StoreId = Stores.StoreId INNER JOIN
                      dbo.Items() Items ON Applications.ApplicationId = Items.ApplicationId ON ItemsHierarchy.ItemId = Items.ItemId
WHERE     (Stores.Name = @STORENAME) AND (Applications.Name = @APPLICATIONNAME)

-- Item Authorizations
SELECT DISTINCT Items.Name AS ItemName, Authorizations.ValidFrom, Authorizations.ValidTo
FROM         dbo.Authorizations() Authorizations INNER JOIN
                      dbo.Items() Items ON Authorizations.ItemId = Items.ItemId INNER JOIN
                      dbo.Stores() Stores INNER JOIN
                      dbo.Applications() Applications ON Stores.StoreId = Applications.StoreId ON Items.ApplicationId = Applications.ApplicationId
WHERE     (Authorizations.AuthorizationType <> 0) AND (Stores.Name = @STORENAME) AND (Applications.Name = @APPLICATIONNAME)
UNION
SELECT DISTINCT Items.Name AS ItemName, NULL ValidFrom, NULL ValidTo
FROM         dbo.Items() Items INNER JOIN
                      dbo.Stores() Stores INNER JOIN
                      dbo.Applications() Applications ON Stores.StoreId = Applications.StoreId ON Items.ApplicationId = Applications.ApplicationId
WHERE     (Stores.Name = @STORENAME) AND (Applications.Name = @APPLICATIONNAME) AND Items.BizRuleId IS NOT NULL
SET NOCOUNT OFF
GO


SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

GRANT  EXECUTE  ON [dbo].[BuildUserPermissionCache]  TO [NetSqlAzMan_Readers]
GO



ALTER TABLE [dbo].[Settings]  WITH CHECK ADD  CONSTRAINT [CK_Settings] CHECK  (([SettingName] = 'Mode' and ([SettingValue] = 'Developer' or [SettingValue] = 'Administrator') or [SettingName] = 'LogErrors' and ([SettingValue] = 'True' or [SettingValue] = 'False') or [SettingName] = 'LogWarnings' and ([SettingValue] = 'True' or [SettingValue] = 'False') or [SettingName] = 'LogInformations' and ([SettingValue] = 'True' or [SettingValue] = 'False') or [SettingName] = 'LogOnEventLog' and ([SettingValue] = 'True' or [SettingValue] = 'False') or [SettingName] = 'LogOnDb' and ([SettingValue] = 'True' or [SettingValue] = 'False')))
GO
ALTER TABLE [dbo].[Settings] CHECK CONSTRAINT [CK_Settings]
GO
ALTER TABLE [dbo].[LogTable]  WITH CHECK ADD  CONSTRAINT [CK_Log] CHECK  (([LogType] = 'I' or [LogType] = 'W' or [LogType] = 'E'))
GO
ALTER TABLE [dbo].[LogTable] CHECK CONSTRAINT [CK_Log]
GO
ALTER TABLE [dbo].[ApplicationGroupMembersTable]  WITH CHECK ADD  CONSTRAINT [CK_WhereDefinedNotValid] CHECK  (([WhereDefined] >= 0 and [WhereDefined] <= 4))
GO
ALTER TABLE [dbo].[ApplicationGroupMembersTable] CHECK CONSTRAINT [CK_WhereDefinedNotValid]
GO
ALTER TABLE [dbo].[AuthorizationsTable]  WITH CHECK ADD  CONSTRAINT [CK_AuthorizationTypeCheck] CHECK  (([AuthorizationType] >= 0 and [AuthorizationType] <= 3))
GO
ALTER TABLE [dbo].[AuthorizationsTable] CHECK CONSTRAINT [CK_AuthorizationTypeCheck]
GO
ALTER TABLE [dbo].[AuthorizationsTable]  WITH CHECK ADD  CONSTRAINT [CK_objectSidWhereDefinedCheck] CHECK  (([objectSidWhereDefined] >= 0 and [objectSidWhereDefined] <= 4))
GO
ALTER TABLE [dbo].[AuthorizationsTable] CHECK CONSTRAINT [CK_objectSidWhereDefinedCheck]
GO
ALTER TABLE [dbo].[AuthorizationsTable]  WITH CHECK ADD  CONSTRAINT [CK_ownerSidWhereDefined] CHECK  (([ownerSidWhereDefined] >= 2 and [ownerSidWhereDefined] <= 4))
GO
ALTER TABLE [dbo].[AuthorizationsTable] CHECK CONSTRAINT [CK_ownerSidWhereDefined]
GO
ALTER TABLE [dbo].[AuthorizationsTable]  WITH CHECK ADD  CONSTRAINT [CK_ValidFromToCheck] CHECK  (([ValidFrom] is null or [ValidTo] is null or [ValidFrom] <= [ValidTo]))
GO
ALTER TABLE [dbo].[AuthorizationsTable] CHECK CONSTRAINT [CK_ValidFromToCheck]
GO
ALTER TABLE [dbo].[ItemsTable]  WITH CHECK ADD  CONSTRAINT [CK_Items_ItemTypeCheck] CHECK  (([ItemType] >= 0 and [ItemType] <= 2))
GO
ALTER TABLE [dbo].[ItemsTable] CHECK CONSTRAINT [CK_Items_ItemTypeCheck]
GO
ALTER TABLE [dbo].[StorePermissionsTable]  WITH CHECK ADD  CONSTRAINT [CK_StorePermissions] CHECK  (([NetSqlAzManFixedServerRole] >= 0 and [NetSqlAzManFixedServerRole] <= 2))
GO
ALTER TABLE [dbo].[StorePermissionsTable] CHECK CONSTRAINT [CK_StorePermissions]
GO
ALTER TABLE [dbo].[StoreGroupsTable]  WITH CHECK ADD  CONSTRAINT [CK_StoreGroups_GroupType_Check] CHECK  (([GroupType] >= 0 and [GroupType] <= 1))
GO
ALTER TABLE [dbo].[StoreGroupsTable] CHECK CONSTRAINT [CK_StoreGroups_GroupType_Check]
GO
ALTER TABLE [dbo].[ApplicationGroupsTable]  WITH CHECK ADD  CONSTRAINT [CK_ApplicationGroups_GroupType_Check] CHECK  (([GroupType] >= 0 and [GroupType] <= 1))
GO
ALTER TABLE [dbo].[ApplicationGroupsTable] CHECK CONSTRAINT [CK_ApplicationGroups_GroupType_Check]
GO
ALTER TABLE [dbo].[ApplicationPermissionsTable]  WITH CHECK ADD  CONSTRAINT [CK_ApplicationPermissions] CHECK  (([NetSqlAzManFixedServerRole] >= 0 and [NetSqlAzManFixedServerRole] <= 2))
GO
ALTER TABLE [dbo].[ApplicationPermissionsTable] CHECK CONSTRAINT [CK_ApplicationPermissions]
GO
ALTER TABLE [dbo].[StoreGroupMembersTable]  WITH CHECK ADD  CONSTRAINT [CK_WhereDefinedCheck] CHECK  (([WhereDefined] = 0 or [WhereDefined] >= 2 and [WhereDefined] <= 4))
GO
ALTER TABLE [dbo].[StoreGroupMembersTable] CHECK CONSTRAINT [CK_WhereDefinedCheck]
GO
ALTER TABLE [dbo].[ApplicationGroupMembersTable]  WITH CHECK ADD  CONSTRAINT [FK_ApplicationGroupMembers_ApplicationGroup] FOREIGN KEY([ApplicationGroupId])
REFERENCES [dbo].[ApplicationGroupsTable] ([ApplicationGroupId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ApplicationGroupMembersTable] CHECK CONSTRAINT [FK_ApplicationGroupMembers_ApplicationGroup]
GO
ALTER TABLE [dbo].[ItemAttributesTable]  WITH CHECK ADD  CONSTRAINT [FK_ItemAttributes_Items] FOREIGN KEY([ItemId])
REFERENCES [dbo].[ItemsTable] ([ItemId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ItemAttributesTable] CHECK CONSTRAINT [FK_ItemAttributes_Items]
GO
ALTER TABLE [dbo].[AuthorizationsTable]  WITH CHECK ADD  CONSTRAINT [FK_Authorizations_Items] FOREIGN KEY([ItemId])
REFERENCES [dbo].[ItemsTable] ([ItemId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AuthorizationsTable] CHECK CONSTRAINT [FK_Authorizations_Items]
GO
ALTER TABLE [dbo].[AuthorizationAttributesTable]  WITH CHECK ADD  CONSTRAINT [FK_AuthorizationAttributes_Authorizations] FOREIGN KEY([AuthorizationId])
REFERENCES [dbo].[AuthorizationsTable] ([AuthorizationId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[AuthorizationAttributesTable] CHECK CONSTRAINT [FK_AuthorizationAttributes_Authorizations]
GO
ALTER TABLE [dbo].[ItemsTable]  WITH CHECK ADD  CONSTRAINT [FK_Items_Applications] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[ApplicationsTable] ([ApplicationId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ItemsTable] CHECK CONSTRAINT [FK_Items_Applications]
GO
ALTER TABLE [dbo].[ItemsTable]  WITH CHECK ADD  CONSTRAINT [FK_Items_BizRules] FOREIGN KEY([BizRuleId])
REFERENCES [dbo].[BizRulesTable] ([BizRuleId])
GO
ALTER TABLE [dbo].[ItemsTable] CHECK CONSTRAINT [FK_Items_BizRules]
GO
ALTER TABLE [dbo].[StorePermissionsTable]  WITH CHECK ADD  CONSTRAINT [FK_StorePermissions_StoresTable] FOREIGN KEY([StoreId])
REFERENCES [dbo].[StoresTable] ([StoreId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StorePermissionsTable] CHECK CONSTRAINT [FK_StorePermissions_StoresTable]
GO
ALTER TABLE [dbo].[StoreAttributesTable]  WITH CHECK ADD  CONSTRAINT [FK_StoreAttributes_Stores] FOREIGN KEY([StoreId])
REFERENCES [dbo].[StoresTable] ([StoreId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StoreAttributesTable] CHECK CONSTRAINT [FK_StoreAttributes_Stores]
GO
ALTER TABLE [dbo].[ApplicationsTable]  WITH CHECK ADD  CONSTRAINT [FK_Applications_Stores] FOREIGN KEY([StoreId])
REFERENCES [dbo].[StoresTable] ([StoreId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ApplicationsTable] CHECK CONSTRAINT [FK_Applications_Stores]
GO
ALTER TABLE [dbo].[StoreGroupsTable]  WITH CHECK ADD  CONSTRAINT [FK_StoreGroups_Stores] FOREIGN KEY([StoreId])
REFERENCES [dbo].[StoresTable] ([StoreId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StoreGroupsTable] CHECK CONSTRAINT [FK_StoreGroups_Stores]
GO
ALTER TABLE [dbo].[ApplicationAttributesTable]  WITH CHECK ADD  CONSTRAINT [FK_ApplicationAttributes_Applications] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[ApplicationsTable] ([ApplicationId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ApplicationAttributesTable] CHECK CONSTRAINT [FK_ApplicationAttributes_Applications]
GO
ALTER TABLE [dbo].[ApplicationGroupsTable]  WITH CHECK ADD  CONSTRAINT [FK_ApplicationGroups_Applications] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[ApplicationsTable] ([ApplicationId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ApplicationGroupsTable] CHECK CONSTRAINT [FK_ApplicationGroups_Applications]
GO
ALTER TABLE [dbo].[ApplicationPermissionsTable]  WITH CHECK ADD  CONSTRAINT [FK_ApplicationPermissions_ApplicationsTable] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[ApplicationsTable] ([ApplicationId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ApplicationPermissionsTable] CHECK CONSTRAINT [FK_ApplicationPermissions_ApplicationsTable]
GO
ALTER TABLE [dbo].[StoreGroupMembersTable]  WITH CHECK ADD  CONSTRAINT [FK_StoreGroupMembers_StoreGroup] FOREIGN KEY([StoreGroupId])
REFERENCES [dbo].[StoreGroupsTable] ([StoreGroupId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[StoreGroupMembersTable] CHECK CONSTRAINT [FK_StoreGroupMembers_StoreGroup]
GO
EXEC dbo.sp_addrolemember @rolename=N'NetSqlAzMan_Readers', @membername=N'BUILTIN\Administrators'
GO
EXEC dbo.sp_addrolemember @rolename=N'NetSqlAzMan_Readers', @membername=N'NetSqlAzMan_Administrators'
GO
EXEC dbo.sp_addrolemember @rolename=N'NetSqlAzMan_Readers', @membername=N'NetSqlAzMan_Users'
GO
EXEC dbo.sp_addrolemember @rolename=N'NetSqlAzMan_Readers', @membername=N'NetSqlAzMan_Managers'
GO
EXEC dbo.sp_addrolemember @rolename=N'NetSqlAzMan_Users', @membername=N'BUILTIN\Administrators'
GO
EXEC dbo.sp_addrolemember @rolename=N'NetSqlAzMan_Users', @membername=N'NetSqlAzMan_Administrators'
GO
EXEC dbo.sp_addrolemember @rolename=N'NetSqlAzMan_Users', @membername=N'NetSqlAzMan_Managers'
GO
EXEC dbo.sp_addrolemember @rolename=N'NetSqlAzMan_Managers', @membername=N'BUILTIN\Administrators'
GO
EXEC dbo.sp_addrolemember @rolename=N'NetSqlAzMan_Managers', @membername=N'NetSqlAzMan_Administrators'
GO
EXEC dbo.sp_addrolemember @rolename=N'NetSqlAzMan_Administrators', @membername=N'BUILTIN\Administrators'
GO
GRANT EXECUTE ON [dbo].[NetSqlAzMan_DBVersion] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[IAmAdmin] TO [NetSqlAzMan_Readers]
GO
GRANT INSERT ON [dbo].[Settings] TO [NetSqlAzMan_Administrators]
GO
GRANT DELETE ON [dbo].[Settings] TO [NetSqlAzMan_Administrators]
GO
GRANT UPDATE ON [dbo].[Settings] TO [NetSqlAzMan_Administrators]
GO
GRANT SELECT ON [dbo].[Settings] TO [NetSqlAzMan_Readers]
GO
GRANT UPDATE ON [dbo].[Settings] ([SettingName]) TO [NetSqlAzMan_Administrators]
GO
GRANT SELECT ON [dbo].[Settings] ([SettingName]) TO [NetSqlAzMan_Readers]
GO
GRANT UPDATE ON [dbo].[Settings] ([SettingValue]) TO [NetSqlAzMan_Administrators]
GO
GRANT SELECT ON [dbo].[Settings] ([SettingValue]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[LogTable] TO [NetSqlAzMan_Readers]
GO
GRANT INSERT ON [dbo].[LogTable] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[LogTable] ([LogId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[LogTable] ([LogDateTime]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[LogTable] ([WindowsIdentity]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[LogTable] ([SqlIdentity]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[LogTable] ([MachineName]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[LogTable] ([InstanceGuid]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[LogTable] ([TransactionGuid]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[LogTable] ([OperationCounter]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[LogTable] ([ENSType]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[LogTable] ([ENSDescription]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[LogTable] ([LogType]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[UsersDemo] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[UsersDemo] ([UserID]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[UsersDemo] ([UserName]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[UsersDemo] ([Password]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[UsersDemo] ([FullName]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[UsersDemo] ([OtherFields]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[DatabaseUsers] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[DatabaseUsers] ([DBUserSid]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[DatabaseUsers] ([DBUserName]) TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[IsAMemberOfGroup] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[helplogins] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StorePermissionsTable] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StorePermissionsTable] ([StorePermissionId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StorePermissionsTable] ([StoreId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StorePermissionsTable] ([SqlUserOrRole]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StorePermissionsTable] ([IsSqlRole]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StorePermissionsTable] ([NetSqlAzManFixedServerRole]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationPermissionsTable] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationPermissionsTable] ([ApplicationPermissionId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationPermissionsTable] ([ApplicationId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationPermissionsTable] ([SqlUserOrRole]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationPermissionsTable] ([IsSqlRole]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationPermissionsTable] ([NetSqlAzManFixedServerRole]) TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[RevokeStoreAccess] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[GrantStoreAccess] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[CheckStorePermissions] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[CheckApplicationPermissions] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[GetDBUsers] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[GetDBUsers] ([DBUserSid]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[GetDBUsers] ([DBUserName]) TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[BizRuleDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[BizRuleInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[StoreInsert] TO [NetSqlAzMan_Administrators]
GO
GRANT SELECT ON [dbo].[Stores] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[Stores] ([StoreId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[Stores] ([Name]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[Stores] ([Description]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[Applications] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[Applications] ([ApplicationId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[Applications] ([StoreId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[Applications] ([Name]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[Applications] ([Description]) TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[StoreGroupDelete] TO [NetSqlAzMan_Administrators]
GO
GRANT EXECUTE ON [dbo].[StoreGroupInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[StoreGroupUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[StorePermissionDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[StorePermissionInsert] TO [NetSqlAzMan_Managers]
GO
GRANT SELECT ON [dbo].[StorePermissions] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StorePermissions] ([StorePermissionId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StorePermissions] ([StoreId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StorePermissions] ([SqlUserOrRole]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StorePermissions] ([IsSqlRole]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StorePermissions] ([NetSqlAzManFixedServerRole]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationAttributes] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationAttributes] ([ApplicationAttributeId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationAttributes] ([ApplicationId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationAttributes] ([AttributeKey]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationAttributes] ([AttributeValue]) TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[ApplicationAttributeInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[ApplicationGroupInsert] TO [NetSqlAzMan_Managers]
GO
GRANT SELECT ON [dbo].[ApplicationGroups] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationGroups] ([ApplicationGroupId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationGroups] ([ApplicationId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationGroups] ([objectSid]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationGroups] ([Name]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationGroups] ([Description]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationGroups] ([LDapQuery]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationGroups] ([GroupType]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationPermissions] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationPermissions] ([ApplicationPermissionId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationPermissions] ([ApplicationId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationPermissions] ([SqlUserOrRole]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationPermissions] ([IsSqlRole]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationPermissions] ([NetSqlAzManFixedServerRole]) TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[RevokeApplicationAccess] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[GrantApplicationAccess] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[ApplicationPermissionInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[ApplicationPermissionDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[ItemInsert] TO [NetSqlAzMan_Managers]
GO
GRANT SELECT ON [dbo].[Items] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[Items] ([ItemId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[Items] ([ApplicationId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[Items] ([Name]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[Items] ([Description]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[Items] ([ItemType]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[Items] ([BizRuleId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StoreGroups] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StoreGroups] ([StoreGroupId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StoreGroups] ([StoreId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StoreGroups] ([objectSid]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StoreGroups] ([Name]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StoreGroups] ([Description]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StoreGroups] ([LDapQuery]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StoreGroups] ([GroupType]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StoreAttributes] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StoreAttributes] ([StoreAttributeId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StoreAttributes] ([StoreId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StoreAttributes] ([AttributeKey]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StoreAttributes] ([AttributeValue]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationsView] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationsView] ([StoreId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationsView] ([StoreName]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationsView] ([StoreDescription]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationsView] ([ApplicationId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationsView] ([ApplicationName]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationsView] ([ApplicationDescription]) TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[StoreUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[StoreAttributeInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[ApplicationInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[ApplicationDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[ApplicationUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[ApplicationAttributeUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[ApplicationAttributeDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[ApplicationGroupDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[ApplicationGroupUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[ReloadBizRule] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[ClearBizRule] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[ItemDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[ItemUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT SELECT ON [dbo].[StoreGroupMembers] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StoreGroupMembers] ([StoreGroupMemberId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StoreGroupMembers] ([StoreGroupId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StoreGroupMembers] ([objectSid]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StoreGroupMembers] ([WhereDefined]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StoreGroupMembers] ([IsMember]) TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[StoreGroupMemberInsert] TO [NetSqlAzMan_Managers]
GO
GRANT SELECT ON [dbo].[ApplicationGroupMembers] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationGroupMembers] ([ApplicationGroupMemberId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationGroupMembers] ([ApplicationGroupId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationGroupMembers] ([objectSid]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationGroupMembers] ([WhereDefined]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationGroupMembers] ([IsMember]) TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[ApplicationGroupMemberInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[AuthorizationInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[CreateDelegate] TO [NetSqlAzMan_Users]
GO
GRANT SELECT ON [dbo].[Authorizations] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[Authorizations] ([AuthorizationId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[Authorizations] ([ItemId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[Authorizations] ([ownerSid]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[Authorizations] ([ownerSidWhereDefined]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[Authorizations] ([objectSid]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[Authorizations] ([objectSidWhereDefined]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[Authorizations] ([AuthorizationType]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[Authorizations] ([ValidFrom]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[Authorizations] ([ValidTo]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ItemAttributes] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ItemAttributes] ([ItemAttributeId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ItemAttributes] ([ItemId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ItemAttributes] ([AttributeKey]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ItemAttributes] ([AttributeValue]) TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[ItemAttributeInsert] TO [NetSqlAzMan_Managers]
GO
GRANT SELECT ON [dbo].[StoreAttributesView] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StoreAttributesView] ([StoreId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StoreAttributesView] ([Name]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StoreAttributesView] ([Description]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StoreAttributesView] ([StoreAttributeId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StoreAttributesView] ([AttributeKey]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StoreAttributesView] ([AttributeValue]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationAttributesView] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationAttributesView] ([ApplicationId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationAttributesView] ([StoreId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationAttributesView] ([Name]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationAttributesView] ([Description]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationAttributesView] ([ApplicationAttributeId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationAttributesView] ([AttributeKey]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationAttributesView] ([AttributeValue]) TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[StoreAttributeDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[StoreAttributeUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[ItemsHierarchyDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[ItemsHierarchyInsert] TO [NetSqlAzMan_Managers]
GO
GRANT SELECT ON [dbo].[BizRules] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[BizRules] ([BizRuleId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[BizRules] ([BizRuleSource]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[BizRules] ([BizRuleLanguage]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[BizRules] ([CompiledAssembly]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ItemsHierarchy] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ItemsHierarchy] ([ItemId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ItemsHierarchy] ([MemberOfItemId]) TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[StoreGroupMemberDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[StoreGroupMemberUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[ApplicationGroupMemberDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[ApplicationGroupMemberUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[AuthorizationUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[DeleteDelegate] TO [NetSqlAzMan_Users]
GO
GRANT EXECUTE ON [dbo].[AuthorizationDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[ItemAttributeDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[ItemAttributeUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT SELECT ON [dbo].[AuthorizationAttributes] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[AuthorizationAttributes] ([AuthorizationAttributeId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[AuthorizationAttributes] ([AuthorizationId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[AuthorizationAttributes] ([AttributeKey]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[AuthorizationAttributes] ([AttributeValue]) TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[AuthorizationAttributeInsert] TO [NetSqlAzMan_Users]
GO
GRANT SELECT ON [dbo].[AuthorizationView] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[AuthorizationView] ([AuthorizationId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[AuthorizationView] ([ItemId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[AuthorizationView] ([Owner]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[AuthorizationView] ([Name]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[AuthorizationView] ([objectSid]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[AuthorizationView] ([SidWhereDefined]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[AuthorizationView] ([AuthorizationType]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[AuthorizationView] ([ValidFrom]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[AuthorizationView] ([ValidTo]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StoreGroupMembersView] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StoreGroupMembersView] ([StoreGroupMemberId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StoreGroupMembersView] ([StoreGroupId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StoreGroupMembersView] ([StoreGroup]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StoreGroupMembersView] ([Name]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StoreGroupMembersView] ([objectSid]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StoreGroupMembersView] ([WhereDefined]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[StoreGroupMembersView] ([MemberType]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationGroupMembersView] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationGroupMembersView] ([StoreId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationGroupMembersView] ([ApplicationId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationGroupMembersView] ([ApplicationGroupMemberId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationGroupMembersView] ([ApplicationGroupId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationGroupMembersView] ([ApplicationGroup]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationGroupMembersView] ([Name]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationGroupMembersView] ([objectSid]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationGroupMembersView] ([WhereDefined]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ApplicationGroupMembersView] ([MemberType]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ItemsHierarchyView] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ItemsHierarchyView] ([ItemId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ItemsHierarchyView] ([ApplicationId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ItemsHierarchyView] ([Name]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ItemsHierarchyView] ([Description]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ItemsHierarchyView] ([ItemType]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ItemsHierarchyView] ([MemberItemId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ItemsHierarchyView] ([MemberApplicationId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ItemsHierarchyView] ([MemberName]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ItemsHierarchyView] ([MemberDescription]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ItemsHierarchyView] ([MemberType]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[BizRuleView] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[BizRuleView] ([ItemId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[BizRuleView] ([ApplicationId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[BizRuleView] ([Name]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[BizRuleView] ([Description]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[BizRuleView] ([ItemType]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[BizRuleView] ([BizRuleSource]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[BizRuleView] ([BizRuleLanguage]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[BizRuleView] ([CompiledAssembly]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ItemAttributesView] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ItemAttributesView] ([ItemId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ItemAttributesView] ([ApplicationId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ItemAttributesView] ([Name]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ItemAttributesView] ([Description]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ItemAttributesView] ([ItemType]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ItemAttributesView] ([ItemAttributeId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ItemAttributesView] ([AttributeKey]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[ItemAttributesView] ([AttributeValue]) TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[BizRuleUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[AuthorizationAttributeDelete] TO [NetSqlAzMan_Users]
GO
GRANT EXECUTE ON [dbo].[AuthorizationAttributeUpdate] TO [NetSqlAzMan_Users]
GO
GRANT SELECT ON [dbo].[AuthorizationAttributesView] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[AuthorizationAttributesView] ([AuthorizationId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[AuthorizationAttributesView] ([ItemId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[AuthorizationAttributesView] ([Owner]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[AuthorizationAttributesView] ([Name]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[AuthorizationAttributesView] ([objectSid]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[AuthorizationAttributesView] ([SidWhereDefined]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[AuthorizationAttributesView] ([AuthorizationType]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[AuthorizationAttributesView] ([ValidFrom]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[AuthorizationAttributesView] ([ValidTo]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[AuthorizationAttributesView] ([AuthorizationAttributeId]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[AuthorizationAttributesView] ([AttributeKey]) TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[AuthorizationAttributesView] ([AttributeValue]) TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[DirectCheckAccess] TO [NetSqlAzMan_Readers]
GO

/* ***************** */
/* ONLY FOR SQL 2005 */
/* ***************** */
IF CHARINDEX('Microsoft SQL Server 2005', REPLACE(@@VERSION,'  ', ' '))=1 BEGIN
        EXEC sp_executesql N'GRANT VIEW DEFINITION TO [NetSqlAzMan_Readers]' -- ALLOW NetSqlAzMan_Readers TO SEE ALL OTHER Logins 
-- http://www.microsoft.com/technet/technetmag/issues/2006/01/ProtectMetaData/?topics=y
END
/* ***************** */
