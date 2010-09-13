/*************************************************************************************/
/* NetSqlAzMan - .NET SQL Authorization Manager - http://netsqlazman.codeplex.com    */
/*************************************************************************************/
/* Microsoft Public License (Ms-PL) - Andrea Ferendeles - aferende@hotmail.com       */
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
GO

/****** Object:  Role [NetSqlAzMan_Readers]    Script Date: 06/11/2009 17:45:30 ******/
EXEC dbo.sp_addrole @rolename = N'NetSqlAzMan_Readers'
GO
/****** Object:  Role [NetSqlAzMan_Users]    Script Date: 06/11/2009 17:45:30 ******/
EXEC dbo.sp_addrole @rolename = N'NetSqlAzMan_Users'
GO
/****** Object:  Role [NetSqlAzMan_Managers]    Script Date: 06/11/2009 17:45:30 ******/
EXEC dbo.sp_addrole @rolename = N'NetSqlAzMan_Managers'
GO
/****** Object:  Role [NetSqlAzMan_Administrators]    Script Date: 06/11/2009 17:45:30 ******/
EXEC dbo.sp_addrole @rolename = N'NetSqlAzMan_Administrators'
GO
/****** Object:  User [BUILTIN\Administrators]    Script Date: 06/11/2009 17:45:30 ******/
EXEC dbo.sp_grantdbaccess @loginame = N'BUILTIN\Administrators', @name_in_db = N'BUILTIN\Administrators'
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_ExecuteLDAPQuery]    Script Date: 06/11/2009 17:45:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_ExecuteLDAPQuery](@LDAPPATH NVARCHAR(4000), @LDAPQUERY NVARCHAR(4000), @members_cur CURSOR VARYING OUTPUT)
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
/****** Object:  Table [dbo].[netsqlazman_BizRulesTable]    Script Date: 06/11/2009 17:45:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[netsqlazman_BizRulesTable](
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
/****** Object:  Table [dbo].[UsersDemo]    Script Date: 06/11/2009 17:45:32 ******/
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
SET ANSI_PADDING ON
GO
/****** Object:  UserDefinedFunction [dbo].[netsqlazman_DBVersion]    Script Date: 06/11/2009 17:45:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[netsqlazman_DBVersion] ()  
RETURNS nvarchar(200) AS  
BEGIN 
	return '3.6.0.x'
END
GO
/****** Object:  Table [dbo].[netsqlazman_ItemsHierarchyTable]    Script Date: 06/11/2009 17:45:31 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[netsqlazman_ItemsHierarchyTable](
	[ItemId] [int] NOT NULL,
	[MemberOfItemId] [int] NOT NULL,
 CONSTRAINT [PK_ItemsHierarchy] PRIMARY KEY CLUSTERED 
(
	[ItemId] ASC,
	[MemberOfItemId] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ItemsHierarchy] ON [dbo].[netsqlazman_ItemsHierarchyTable] 
(
	[ItemId] ASC
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ItemsHierarchy_1] ON [dbo].[netsqlazman_ItemsHierarchyTable] 
(
	[MemberOfItemId] ASC
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[netsqlazman_LogTable]    Script Date: 06/11/2009 17:45:32 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[netsqlazman_LogTable](
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
SET ANSI_PADDING ON
GO
CREATE CLUSTERED INDEX [IX_Log_2] ON [dbo].[netsqlazman_LogTable] 
(
	[LogDateTime] DESC,
	[InstanceGuid] ASC,
	[OperationCounter] DESC
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Log] ON [dbo].[netsqlazman_LogTable] 
(
	[WindowsIdentity] ASC
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Log_1] ON [dbo].[netsqlazman_LogTable] 
(
	[SqlIdentity] ASC
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[netsqlazman_Settings]    Script Date: 06/11/2009 17:45:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[netsqlazman_Settings](
	[SettingName] [nvarchar](255) NOT NULL,
	[SettingValue] [nvarchar](255) NOT NULL,
 CONSTRAINT [PK_Settings] PRIMARY KEY CLUSTERED 
(
	[SettingName] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[netsqlazman_MergeAuthorizations]    Script Date: 06/11/2009 17:45:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[netsqlazman_MergeAuthorizations](@AUTH1 tinyint, @AUTH2 tinyint)
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
/****** Object:  UserDefinedFunction [dbo].[netsqlazman_IAmAdmin]    Script Date: 06/11/2009 17:45:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[netsqlazman_IAmAdmin] ()  
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
/****** Object:  StoredProcedure [dbo].[netsqlazman_helplogins]    Script Date: 06/11/2009 17:45:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
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
GO
/****** Object:  Table [dbo].[netsqlazman_StoresTable]    Script Date: 06/11/2009 17:45:33 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[netsqlazman_StoresTable](
	[StoreId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](1024) NOT NULL,
 CONSTRAINT [PK_Stores] PRIMARY KEY CLUSTERED 
(
	[StoreId] ASC
) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [Stores_Name_Unique_Index] ON [dbo].[netsqlazman_StoresTable] 
(
	[Name] ASC
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[netsqlazman_AuthorizationAttributesTable]    Script Date: 06/11/2009 17:45:35 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[netsqlazman_AuthorizationAttributesTable](
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
CREATE UNIQUE NONCLUSTERED INDEX [AuthorizationAttributes_AuhorizationId_AttributeKey_Unique_Index] ON [dbo].[netsqlazman_AuthorizationAttributesTable] 
(
	[AuthorizationId] ASC,
	[AttributeKey] ASC
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_AuthorizationAttributes] ON [dbo].[netsqlazman_AuthorizationAttributesTable] 
(
	[AuthorizationId] ASC,
	[AttributeKey] ASC
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[netsqlazman_ApplicationGroupMembersTable]    Script Date: 06/11/2009 17:45:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[netsqlazman_ApplicationGroupMembersTable](
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
SET ANSI_PADDING ON
GO
CREATE UNIQUE NONCLUSTERED INDEX [ApplicationGroupMembers_ApplicationGroupId_ObjectSid_IsMember_Unique_Index] ON [dbo].[netsqlazman_ApplicationGroupMembersTable] 
(
	[ApplicationGroupId] ASC,
	[objectSid] ASC,
	[IsMember] ASC
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ApplicationGroupMembers] ON [dbo].[netsqlazman_ApplicationGroupMembersTable] 
(
	[ApplicationGroupId] ASC,
	[objectSid] ASC
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[netsqlazman_ItemsTable]    Script Date: 06/11/2009 17:45:36 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[netsqlazman_ItemsTable](
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
CREATE UNIQUE NONCLUSTERED INDEX [Items_ApplicationId_Name_Unique_Index] ON [dbo].[netsqlazman_ItemsTable] 
(
	[Name] ASC,
	[ApplicationId] ASC
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Items] ON [dbo].[netsqlazman_ItemsTable] 
(
	[ApplicationId] ASC,
	[Name] ASC
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[netsqlazman_ApplicationGroupsTable]    Script Date: 06/11/2009 17:45:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[netsqlazman_ApplicationGroupsTable](
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
SET ANSI_PADDING ON
GO
CREATE UNIQUE NONCLUSTERED INDEX [ApplicationGroups_ApplicationId_Name_Unique_Index] ON [dbo].[netsqlazman_ApplicationGroupsTable] 
(
	[ApplicationId] ASC,
	[Name] ASC
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ApplicationGroups] ON [dbo].[netsqlazman_ApplicationGroupsTable] 
(
	[ApplicationId] ASC,
	[Name] ASC
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ApplicationGroups_1] ON [dbo].[netsqlazman_ApplicationGroupsTable] 
(
	[objectSid] ASC
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[netsqlazman_ApplicationAttributesTable]    Script Date: 06/11/2009 17:45:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[netsqlazman_ApplicationAttributesTable](
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
CREATE UNIQUE NONCLUSTERED INDEX [ApplicationAttributes_AuhorizationId_AttributeKey_Unique_Index] ON [dbo].[netsqlazman_ApplicationAttributesTable] 
(
	[ApplicationId] ASC,
	[AttributeKey] ASC
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ApplicationAttributes] ON [dbo].[netsqlazman_ApplicationAttributesTable] 
(
	[ApplicationId] ASC,
	[AttributeKey] ASC
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[netsqlazman_ApplicationPermissionsTable]    Script Date: 06/11/2009 17:45:37 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[netsqlazman_ApplicationPermissionsTable](
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
CREATE NONCLUSTERED INDEX [IX_ApplicationPermissions] ON [dbo].[netsqlazman_ApplicationPermissionsTable] 
(
	[ApplicationId] ASC
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ApplicationPermissions_1] ON [dbo].[netsqlazman_ApplicationPermissionsTable] 
(
	[ApplicationId] ASC,
	[SqlUserOrRole] ASC,
	[NetSqlAzManFixedServerRole] ASC
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[netsqlazman_StoreGroupMembersTable]    Script Date: 06/11/2009 17:45:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[netsqlazman_StoreGroupMembersTable](
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
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_StoreGroupMembers] ON [dbo].[netsqlazman_StoreGroupMembersTable] 
(
	[StoreGroupId] ASC,
	[objectSid] ASC
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [StoreGroupMembers_StoreGroupId_ObjectSid_IsMember_Unique_Index] ON [dbo].[netsqlazman_StoreGroupMembersTable] 
(
	[StoreGroupId] ASC,
	[objectSid] ASC,
	[IsMember] ASC
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[netsqlazman_ApplicationsTable]    Script Date: 06/11/2009 17:45:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[netsqlazman_ApplicationsTable](
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
CREATE UNIQUE NONCLUSTERED INDEX [Applications_StoreId_Name_Unique_Index] ON [dbo].[netsqlazman_ApplicationsTable] 
(
	[Name] ASC,
	[StoreId] ASC
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Applications] ON [dbo].[netsqlazman_ApplicationsTable] 
(
	[ApplicationId] ASC,
	[Name] ASC
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[netsqlazman_StoreAttributesTable]    Script Date: 06/11/2009 17:45:38 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[netsqlazman_StoreAttributesTable](
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
CREATE NONCLUSTERED INDEX [IX_StoreAttributes] ON [dbo].[netsqlazman_StoreAttributesTable] 
(
	[StoreId] ASC,
	[AttributeKey] ASC
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [StoreAttributes_AuhorizationId_AttributeKey_Unique_Index] ON [dbo].[netsqlazman_StoreAttributesTable] 
(
	[StoreId] ASC,
	[AttributeKey] ASC
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[netsqlazman_StorePermissionsTable]    Script Date: 06/11/2009 17:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[netsqlazman_StorePermissionsTable](
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
CREATE NONCLUSTERED INDEX [IX_StorePermissions] ON [dbo].[netsqlazman_StorePermissionsTable] 
(
	[StoreId] ASC
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_StorePermissions_1] ON [dbo].[netsqlazman_StorePermissionsTable] 
(
	[StoreId] ASC,
	[SqlUserOrRole] ASC,
	[NetSqlAzManFixedServerRole] ASC
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[netsqlazman_StoreGroupsTable]    Script Date: 06/11/2009 17:45:39 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[netsqlazman_StoreGroupsTable](
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
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_StoreGroups] ON [dbo].[netsqlazman_StoreGroupsTable] 
(
	[StoreId] ASC,
	[objectSid] ASC
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [StoreGroups_StoreId_Name_Unique_Index] ON [dbo].[netsqlazman_StoreGroupsTable] 
(
	[StoreId] ASC,
	[Name] ASC
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[netsqlazman_AuthorizationsTable]    Script Date: 06/11/2009 17:45:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[netsqlazman_AuthorizationsTable](
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
SET ANSI_PADDING ON
GO
CREATE NONCLUSTERED INDEX [IX_Authorizations] ON [dbo].[netsqlazman_AuthorizationsTable] 
(
	[ItemId] ASC,
	[objectSid] ASC
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_Authorizations_1] ON [dbo].[netsqlazman_AuthorizationsTable] 
(
	[ItemId] ASC,
	[objectSid] ASC,
	[objectSidWhereDefined] ASC,
	[AuthorizationType] ASC,
	[ValidFrom] ASC,
	[ValidTo] ASC
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[netsqlazman_ItemAttributesTable]    Script Date: 06/11/2009 17:45:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[netsqlazman_ItemAttributesTable](
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
CREATE UNIQUE NONCLUSTERED INDEX [ItemAttributes_AuhorizationId_AttributeKey_Unique_Index] ON [dbo].[netsqlazman_ItemAttributesTable] 
(
	[ItemId] ASC,
	[AttributeKey] ASC
) ON [PRIMARY]
GO
CREATE NONCLUSTERED INDEX [IX_ItemAttributes] ON [dbo].[netsqlazman_ItemAttributesTable] 
(
	[ItemId] ASC,
	[AttributeKey] ASC
) ON [PRIMARY]
GO
/****** Object:  Trigger [ApplicationGroupDeleteTrigger]    Script Date: 06/11/2009 17:46:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[ApplicationGroupDeleteTrigger] ON [dbo].[netsqlazman_ApplicationGroupsTable] 
FOR DELETE 
AS
DECLARE @DELETEDOBJECTSID int
DECLARE applicationgroups_cur CURSOR FAST_FORWARD FOR SELECT objectSid FROM deleted
OPEN applicationgroups_cur
FETCH NEXT from applicationgroups_cur INTO @DELETEDOBJECTSID
WHILE @@FETCH_STATUS = 0
BEGIN
	DELETE FROM dbo.[netsqlazman_ApplicationGroupMembersTable] WHERE objectSid = @DELETEDOBJECTSID AND WhereDefined = 1
	DELETE FROM dbo.[netsqlazman_AuthorizationsTable] WHERE objectSid = @DELETEDOBJECTSID AND objectSidWhereDefined = 1
	FETCH NEXT from applicationgroups_cur INTO @DELETEDOBJECTSID
END
CLOSE applicationgroups_cur
DEALLOCATE applicationgroups_cur
GO
/****** Object:  Trigger [StoreGroupDeleteTrigger]    Script Date: 06/11/2009 17:46:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[StoreGroupDeleteTrigger] ON [dbo].[netsqlazman_StoreGroupsTable] 
FOR DELETE 
AS
DECLARE @DELETEDOBJECTSID int
DECLARE storegroups_cur CURSOR FAST_FORWARD FOR SELECT objectSid FROM deleted
OPEN storegroups_cur
FETCH NEXT from storegroups_cur INTO @DELETEDOBJECTSID
WHILE @@FETCH_STATUS = 0
BEGIN
	DELETE FROM dbo.[netsqlazman_StoreGroupMembersTable] WHERE objectSid = @DELETEDOBJECTSID AND WhereDefined = 0
	DELETE FROM dbo.[netsqlazman_ApplicationGroupMembersTable] WHERE objectSid = @DELETEDOBJECTSID AND WhereDefined = 0
	DELETE FROM dbo.[netsqlazman_AuthorizationsTable] WHERE objectSid = @DELETEDOBJECTSID AND objectSidWhereDefined = 0
	FETCH NEXT from storegroups_cur INTO @DELETEDOBJECTSID
END
CLOSE storegroups_cur
DEALLOCATE storegroups_cur
GO
/****** Object:  UserDefinedFunction [dbo].[netsqlazman_CheckApplicationPermissions]    Script Date: 06/11/2009 17:45:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* 
   @ROLEID = { 0 READERS, 1 USERS, 2 MANAGERS}
*/
CREATE FUNCTION [dbo].[netsqlazman_CheckApplicationPermissions](@ApplicationId int, @ROLEID tinyint)
RETURNS bit
AS
BEGIN
DECLARE @RESULT bit
IF @ApplicationId IS NULL OR @ROLEID IS NULL
	SET @RESULT = 0	
ELSE
BEGIN
	IF EXISTS (
		SELECT     dbo.[netsqlazman_ApplicationPermissionsTable].ApplicationId
		FROM         dbo.[netsqlazman_ApplicationsTable] INNER JOIN
		                      dbo.[netsqlazman_StoresTable] ON dbo.[netsqlazman_ApplicationsTable].StoreId = dbo.[netsqlazman_StoresTable].StoreId LEFT OUTER JOIN
		                      dbo.[netsqlazman_StorePermissionsTable] ON dbo.[netsqlazman_StoresTable].StoreId = dbo.[netsqlazman_StorePermissionsTable].StoreId LEFT OUTER JOIN
		                      dbo.[netsqlazman_ApplicationPermissionsTable] ON dbo.[netsqlazman_ApplicationsTable].ApplicationId = dbo.[netsqlazman_ApplicationPermissionsTable].ApplicationId
		WHERE
		IS_MEMBER('db_owner')=1 OR IS_MEMBER('NetSqlAzMan_Administrators')=1 OR 
		(@ROLEID = 0 AND IS_MEMBER('NetSqlAzMan_Readers')=1 OR 
		@ROLEID = 1 AND IS_MEMBER('NetSqlAzMan_Users')=1 OR 
		@ROLEID = 2 AND IS_MEMBER('NetSqlAzMan_Managers')=1) AND
		(
		(dbo.[netsqlazman_ApplicationPermissionsTable].ApplicationId = @ApplicationId AND dbo.[netsqlazman_ApplicationPermissionsTable].NetSqlAzManFixedServerRole >= @ROLEID AND 
		(SUSER_SNAME(SUSER_SID())=[netsqlazman_ApplicationPermissionsTable].SqlUserOrRole AND [netsqlazman_ApplicationPermissionsTable].IsSqlRole = 0
		OR IS_MEMBER([netsqlazman_ApplicationPermissionsTable].SqlUserOrRole)=1 AND [netsqlazman_ApplicationPermissionsTable].IsSqlRole = 1)) OR
	
		dbo.[netsqlazman_ApplicationsTable].ApplicationId = @ApplicationId AND 
		(dbo.[netsqlazman_StorePermissionsTable].StoreId = dbo.[netsqlazman_ApplicationsTable].StoreId AND dbo.[netsqlazman_StorePermissionsTable].NetSqlAzManFixedServerRole >= @ROLEID AND 
		(SUSER_SNAME(SUSER_SID())=[netsqlazman_StorePermissionsTable].SqlUserOrRole AND [netsqlazman_StorePermissionsTable].IsSqlRole = 0 OR
		IS_MEMBER([netsqlazman_StorePermissionsTable].SqlUserOrRole)=1 AND [netsqlazman_StorePermissionsTable].IsSqlRole = 1))

))
	
	SET @RESULT = 1
	ELSE
	SET @RESULT = 0
END
RETURN @RESULT
END
GO
/****** Object:  UserDefinedFunction [dbo].[netsqlazman_CheckStorePermissions]    Script Date: 06/11/2009 17:45:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/* 
   @ROLEID = { 0 READERS, 1 USERS, 2 MANAGERS}
*/
CREATE FUNCTION [dbo].[netsqlazman_CheckStorePermissions](@STOREID int, @ROLEID tinyint)
RETURNS bit
AS
BEGIN
DECLARE @RESULT bit
IF @STOREID IS NULL OR @ROLEID IS NULL
	SET @RESULT = 0	
ELSE
BEGIN
	IF EXISTS (
		SELECT     dbo.[netsqlazman_StorePermissionsTable].StoreId
		FROM         dbo.[netsqlazman_ApplicationsTable] RIGHT OUTER JOIN
		                      dbo.[netsqlazman_StoresTable] ON dbo.[netsqlazman_ApplicationsTable].StoreId = dbo.[netsqlazman_StoresTable].StoreId LEFT OUTER JOIN
		                      dbo.[netsqlazman_StorePermissionsTable] ON dbo.[netsqlazman_StoresTable].StoreId = dbo.[netsqlazman_StorePermissionsTable].StoreId LEFT OUTER JOIN
		                      dbo.[netsqlazman_ApplicationPermissionsTable] ON dbo.[netsqlazman_ApplicationsTable].ApplicationId = dbo.[netsqlazman_ApplicationPermissionsTable].ApplicationId
		WHERE 
		IS_MEMBER('db_owner')=1 OR IS_MEMBER('NetSqlAzMan_Administrators')=1 OR 
		(@ROLEID = 0 AND IS_MEMBER('NetSqlAzMan_Readers')=1 OR 
		@ROLEID = 1 AND IS_MEMBER('NetSqlAzMan_Users')=1 OR 
		@ROLEID = 2 AND IS_MEMBER('NetSqlAzMan_Managers')=1) AND
		(
		(dbo.[netsqlazman_StorePermissionsTable].StoreId = @STOREID AND dbo.[netsqlazman_StorePermissionsTable].NetSqlAzManFixedServerRole >= @ROLEID AND 
		(SUSER_SNAME(SUSER_SID())=[netsqlazman_StorePermissionsTable].SqlUserOrRole AND [netsqlazman_StorePermissionsTable].IsSqlRole = 0 OR
		IS_MEMBER([netsqlazman_StorePermissionsTable].SqlUserOrRole)=1 AND [netsqlazman_StorePermissionsTable].IsSqlRole = 1)) OR
	
		(@ROLEID = 0 AND dbo.[netsqlazman_StoresTable].StoreId = @STOREID AND dbo.[netsqlazman_ApplicationPermissionsTable].NetSqlAzManFixedServerRole >= @ROLEID AND 
		(SUSER_SNAME(SUSER_SID())=[netsqlazman_ApplicationPermissionsTable].SqlUserOrRole AND [netsqlazman_ApplicationPermissionsTable].IsSqlRole = 0
		OR IS_MEMBER([netsqlazman_ApplicationPermissionsTable].SqlUserOrRole)=1 AND [netsqlazman_ApplicationPermissionsTable].IsSqlRole = 1))))
	SET @RESULT = 1
	ELSE
	SET @RESULT = 0
END
RETURN @RESULT
END
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_BizRuleInsert]    Script Date: 06/11/2009 17:45:40 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_BizRuleInsert]
(
	@BizRuleSource text,
	@BizRuleLanguage tinyint,
	@CompiledAssembly image
)
AS
INSERT INTO [dbo].[netsqlazman_BizRulesTable] ([BizRuleSource], [BizRuleLanguage], [CompiledAssembly]) VALUES (@BizRuleSource, @BizRuleLanguage, @CompiledAssembly);
RETURN SCOPE_IDENTITY()
GO
/****** Object:  Trigger [ItemDeleteTrigger]    Script Date: 06/11/2009 17:46:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[ItemDeleteTrigger] ON [dbo].[netsqlazman_ItemsTable] 
FOR DELETE 
AS
DECLARE @DELETEDITEMID int
DECLARE @BIZRULEID int
DECLARE items_cur CURSOR FAST_FORWARD FOR SELECT ItemId, BizRuleId FROM deleted
OPEN items_cur
FETCH NEXT from items_cur INTO @DELETEDITEMID, @BIZRULEID
WHILE @@FETCH_STATUS = 0
BEGIN
	DELETE FROM dbo.[netsqlazman_ItemsHierarchyTable] WHERE ItemId = @DELETEDITEMID OR MemberOfItemId = @DELETEDITEMID
	IF @BIZRULEID IS NOT NULL
		DELETE FROM dbo.[netsqlazman_BizRulesTable] WHERE BizRuleId = @BIZRULEID
	FETCH NEXT from items_cur INTO @DELETEDITEMID, @BIZRULEID
END
CLOSE items_cur
DEALLOCATE items_cur
GO
/****** Object:  UserDefinedFunction [dbo].[netsqlazman_GetDBUsers]    Script Date: 06/11/2009 17:45:41 ******/
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
CREATE FUNCTION [dbo].[netsqlazman_GetDBUsers] (@StoreName nvarchar(255), @ApplicationName nvarchar(255), @DBUserSid VARBINARY(85) = NULL, @DBUserName nvarchar(255) = NULL)  
RETURNS TABLE 
AS  
RETURN 
	SELECT TOP 100 PERCENT CONVERT(VARBINARY(85), UserID) AS DBUserSid, UserName AS DBUserName, FullName, OtherFields FROM dbo.UsersDemo
	WHERE 
		(@DBUserSid IS NOT NULL AND CONVERT(VARBINARY(85), UserID) = @DBUserSid OR @DBUserSid  IS NULL)
		AND
		(@DBUserName IS NOT NULL AND UserName = @DBUserName OR @DBUserName IS NULL)
	ORDER BY UserName
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- THIS CODE IS JUST FOR AN EXAMPLE: comment this section and customize "INSERT HERE YOUR CUSTOM T-SQL" section below
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
GO
/****** Object:  Trigger [ItemsHierarchyTrigger]    Script Date: 06/11/2009 17:46:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TRIGGER [dbo].[ItemsHierarchyTrigger] ON [dbo].[netsqlazman_ItemsHierarchyTable] 
FOR INSERT, UPDATE
AS
DECLARE @INSERTEDITEMID int
DECLARE @INSERTEDMEMBEROFITEMID int

DECLARE itemhierarchy_cur CURSOR FAST_FORWARD FOR SELECT ItemId, MemberOfItemId FROM inserted
OPEN itemhierarchy_cur
FETCH NEXT from itemhierarchy_cur INTO @INSERTEDITEMID, @INSERTEDMEMBEROFITEMID
WHILE @@FETCH_STATUS = 0
BEGIN
	IF UPDATE(ItemId) AND NOT EXISTS (SELECT ItemId FROM dbo.[netsqlazman_ItemsTable] WHERE [netsqlazman_ItemsTable].ItemId = @INSERTEDITEMID) 
	 BEGIN
	  RAISERROR ('ItemId NOT FOUND into dbo.ItemsTable', 16, 1)
	  ROLLBACK TRANSACTION
	 END
	
	IF UPDATE(MemberOfItemId) AND NOT EXISTS (SELECT ItemId FROM dbo.[netsqlazman_ItemsTable] WHERE [netsqlazman_ItemsTable].ItemId = @INSERTEDMEMBEROFITEMID)
	 BEGIN
	  RAISERROR ('MemberOfItemId NOT FOUND into dbo.ItemsTable', 16, 1)
	  ROLLBACK TRANSACTION
	 END
	FETCH NEXT from itemhierarchy_cur INTO @INSERTEDITEMID, @INSERTEDMEMBEROFITEMID
END
CLOSE itemhierarchy_cur
DEALLOCATE itemhierarchy_cur
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_StoreInsert]    Script Date: 06/11/2009 17:45:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_StoreInsert]
(
	@Name nvarchar(255),
	@Description nvarchar(1024)
)
AS
INSERT INTO [dbo].[netsqlazman_StoresTable] ([Name], [Description]) VALUES (@Name, @Description);
RETURN SCOPE_IDENTITY()
GO
/****** Object:  UserDefinedFunction [dbo].[netsqlazman_Applications]    Script Date: 06/11/2009 17:45:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[netsqlazman_Applications] ()
RETURNS TABLE
AS
RETURN
	SELECT * FROM dbo.[netsqlazman_ApplicationsTable]
	WHERE dbo.[netsqlazman_CheckApplicationPermissions](ApplicationId, 0) = 1
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_GrantStoreAccess]    Script Date: 06/11/2009 17:45:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_GrantStoreAccess] (
	@StoreId int,
	@SqlUserOrRole sysname,
	@NetSqlAzManFixedServerRole tinyint)
AS
IF EXISTS(SELECT StoreId FROM dbo.[netsqlazman_StoresTable] WHERE StoreId = @StoreId) AND (dbo.[netsqlazman_CheckStorePermissions](@StoreId, 2) = 1 AND @NetSqlAzManFixedServerRole BETWEEN 0 AND 1 OR (IS_MEMBER('db_owner')=1 OR IS_MEMBER('NetSqlAzMan_Administrators')=1) AND @NetSqlAzManFixedServerRole = 2)
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
	IF EXISTS(SELECT * FROM dbo.[netsqlazman_StorePermissionsTable] WHERE StoreId = @StoreId AND SqlUserOrRole = @SqlUserOrRole AND NetSqlAzManFixedServerRole = @NetSqlAzManFixedServerRole)
		BEGIN
		RAISERROR ('NetSqlAzManFixedServerRole updated.', -1, -1)
		RETURN 0
		END
	ELSE
		BEGIN
		INSERT INTO dbo.[netsqlazman_StorePermissionsTable] (StoreId, SqlUserOrRole, IsSqlRole, NetSqlAzManFixedServerRole) VALUES (@StoreId, @SqlUserOrRole, @IsSqlRole, @NetSqlAzManFixedServerRole)
		RETURN SCOPE_IDENTITY()
		END
END
ELSE
	RAISERROR ('Store NOT Found or Store permission denied.', 16, 1)
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_RevokeStoreAccess]    Script Date: 06/11/2009 17:45:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_RevokeStoreAccess] (
	@StoreId int,
	@SqlUserOrRole sysname,
	@NetSqlAzManFixedServerRole tinyint)
AS
IF EXISTS(SELECT StoreId FROM dbo.[netsqlazman_StoresTable] WHERE StoreId = @StoreId) AND (dbo.[netsqlazman_CheckStorePermissions](@StoreId, 2) = 1 AND @NetSqlAzManFixedServerRole BETWEEN 0 AND 1 OR (IS_MEMBER('db_owner')=1 OR IS_MEMBER('NetSqlAzMan_Administrators')=1) AND @NetSqlAzManFixedServerRole = 2)
BEGIN
	IF EXISTS(SELECT * FROM dbo.[netsqlazman_StorePermissionsTable] WHERE StoreId = @StoreId AND SqlUserOrRole = @SqlUserOrRole AND NetSqlAzManFixedServerRole = @NetSqlAzManFixedServerRole)
		DELETE FROM dbo.[netsqlazman_StorePermissionsTable] WHERE StoreId = @StoreId AND SqlUserOrRole = @SqlUserOrRole AND NetSqlAzManFixedServerRole = @NetSqlAzManFixedServerRole
	ELSE
		RAISERROR ('Permission not found. Revoke Store Access ignored.', -1, -1)
END
ELSE
	RAISERROR ('Store NOT Found or Store permission denied.', 16, 1)
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_BizRuleDelete]    Script Date: 06/11/2009 17:45:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_BizRuleDelete]
(
	@BizRuleId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT BizRuleId FROM dbo.[netsqlazman_BizRulesTable] WHERE BizRuleId = @BizRuleId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1
	DELETE FROM [dbo].[netsqlazman_BizRulesTable] WHERE [BizRuleId] = @BizRuleId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
/****** Object:  View [dbo].[netsqlazman_DatabaseUsers]    Script Date: 06/11/2009 17:45:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[netsqlazman_DatabaseUsers]
AS
SELECT     *
FROM         dbo.[netsqlazman_GetDBUsers](NULL, NULL, DEFAULT, DEFAULT) GetDBUsers
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_StoreGroupDelete]    Script Date: 06/11/2009 17:45:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_StoreGroupDelete]
(
	@Original_StoreGroupId int,
	@StoreId int
)
AS
IF dbo.[netsqlazman_CheckStorePermissions](@StoreId, 2) = 1
	DELETE FROM [dbo].[netsqlazman_StoreGroupsTable] WHERE [StoreGroupId] = @Original_StoreGroupId AND [StoreId] = @StoreId
ELSE
	RAISERROR ('Store permission denied.', 16, 1)
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_StoreGroupInsert]    Script Date: 06/11/2009 17:45:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_StoreGroupInsert]
(
	@StoreId int,
	@objectSid varbinary(85),
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@LDapQuery nvarchar(4000),
	@GroupType tinyint
)
AS
IF dbo.[netsqlazman_CheckStorePermissions](@StoreId, 2) = 1
BEGIN
	INSERT INTO [dbo].[netsqlazman_StoreGroupsTable] ([StoreId], [objectSid], [Name], [Description], [LDapQuery], [GroupType]) VALUES (@StoreId, @objectSid, @Name, @Description, @LDapQuery, @GroupType);
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Store permission denied.', 16, 1)
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_StoreGroupUpdate]    Script Date: 06/11/2009 17:45:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_StoreGroupUpdate]
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
IF dbo.[netsqlazman_CheckStorePermissions](@StoreId, 2) = 1
	UPDATE [dbo].[netsqlazman_StoreGroupsTable] SET [objectSid] = @objectSid, [Name] = @Name, [Description] = @Description, [LDapQuery] = @LDapQuery, [GroupType] = @GroupType WHERE [StoreGroupId] = @Original_StoreGroupId AND StoreId = @StoreId
ELSE
	RAISERROR ('Store permission denied.', 16, 1)
GO
/****** Object:  UserDefinedFunction [dbo].[netsqlazman_Stores]    Script Date: 06/11/2009 17:45:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[netsqlazman_Stores] ()
RETURNS TABLE 
AS
RETURN
	SELECT dbo.[netsqlazman_StoresTable].* FROM dbo.[netsqlazman_StoresTable]
	WHERE dbo.[netsqlazman_CheckStorePermissions]([netsqlazman_StoresTable].StoreId, 0) = 1
GO
/****** Object:  UserDefinedFunction [dbo].[netsqlazman_ApplicationGroups]    Script Date: 06/11/2009 17:45:44 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[netsqlazman_ApplicationGroups] ()
RETURNS TABLE
AS
RETURN
	SELECT     dbo.[netsqlazman_ApplicationGroupsTable].*
	FROM         dbo.[netsqlazman_ApplicationGroupsTable] INNER JOIN
	                      dbo.[netsqlazman_Applications]() Applications ON dbo.[netsqlazman_ApplicationGroupsTable].ApplicationId = Applications.ApplicationId
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_ApplicationGroupInsert]    Script Date: 06/11/2009 17:45:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_ApplicationGroupInsert]
(
	@ApplicationId int,
	@objectSid varbinary(85),
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@LDapQuery nvarchar(4000),
	@GroupType tinyint
)
AS
IF EXISTS(SELECT ApplicationId FROM dbo.[netsqlazman_Applications]() WHERE ApplicationId = @ApplicationId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1
BEGIN
	INSERT INTO [dbo].[netsqlazman_ApplicationGroupsTable] ([ApplicationId], [objectSid], [Name], [Description], [LDapQuery], [GroupType]) VALUES (@ApplicationId, @objectSid, @Name, @Description, @LDapQuery, @GroupType)
	RETURN SCOPE_IDENTITY()
END
ELSE	
	RAISERROR ('Application permission denied.', 16, 1)
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_ApplicationPermissionInsert]    Script Date: 06/11/2009 17:45:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_ApplicationPermissionInsert]
(
	@ApplicationId int,
	@SqlUserOrRole nvarchar(128),
	@IsSqlRole bit,
	@NetSqlAzManFixedServerRole tinyint
)
AS
IF EXISTS(SELECT ApplicationId FROM dbo.[netsqlazman_Applications]() WHERE ApplicationId = @ApplicationId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1
BEGIN
	INSERT INTO dbo.[netsqlazman_ApplicationPermissionsTable] (ApplicationId, SqlUserOrRole, IsSqlRole, NetSqlAzManFixedServerRole) VALUES (@ApplicationId, @SqlUserOrRole, @IsSqlRole, @NetSqlAzManFixedServerRole)
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_RevokeApplicationAccess]    Script Date: 06/11/2009 17:45:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_RevokeApplicationAccess] (
	@ApplicationId int,
	@SqlUserOrRole sysname,
	@NetSqlAzManFixedServerRole tinyint)
AS
DECLARE @StoreId int
SET @StoreId = (SELECT StoreId FROM dbo.[netsqlazman_Applications]() WHERE ApplicationId = @ApplicationId)
IF EXISTS(SELECT ApplicationId FROM dbo.[netsqlazman_ApplicationsTable] WHERE ApplicationId = @ApplicationId) AND (dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1 AND @NetSqlAzManFixedServerRole BETWEEN 0 AND 1 OR dbo.[netsqlazman_CheckStorePermissions](@StoreId, 2) = 1 AND @NetSqlAzManFixedServerRole = 2)
BEGIN
	IF EXISTS(SELECT * FROM dbo.[netsqlazman_ApplicationPermissionsTable] WHERE ApplicationId = @ApplicationId AND SqlUserOrRole = @SqlUserOrRole AND NetSqlAzManFixedServerRole = @NetSqlAzManFixedServerRole)
		DELETE FROM dbo.[netsqlazman_ApplicationPermissionsTable] WHERE ApplicationId = @ApplicationId AND SqlUserOrRole = @SqlUserOrRole AND NetSqlAzManFixedServerRole = @NetSqlAzManFixedServerRole
	ELSE
		RAISERROR ('Permission not found. Revoke Application Access ignored.', -1, -1)
END
ELSE
	RAISERROR ('Application NOT Found or Application permission denied.', 16, 1)
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_ApplicationPermissionDelete]    Script Date: 06/11/2009 17:45:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_ApplicationPermissionDelete]
(
	@ApplicationPermissionId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ApplicationId FROM dbo.[netsqlazman_Applications]() WHERE ApplicationId = @ApplicationId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1
	DELETE FROM dbo.[netsqlazman_ApplicationPermissionsTable] WHERE ApplicationPermissionId = @ApplicationPermissionId AND ApplicationId = @ApplicationId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_GrantApplicationAccess]    Script Date: 06/11/2009 17:45:45 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_GrantApplicationAccess] (
	@ApplicationId int,
	@SqlUserOrRole sysname,
	@NetSqlAzManFixedServerRole tinyint)
AS
DECLARE @StoreId int
SET @StoreId = (SELECT StoreId FROM dbo.[netsqlazman_Applications]() WHERE ApplicationId = @ApplicationId)
IF EXISTS(SELECT ApplicationId FROM dbo.[netsqlazman_ApplicationsTable] WHERE ApplicationId = @ApplicationId) AND (dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1 AND @NetSqlAzManFixedServerRole BETWEEN 0 AND 1 OR dbo.[netsqlazman_CheckStorePermissions](@StoreId, 2) = 1 AND @NetSqlAzManFixedServerRole = 2)
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
	IF EXISTS(SELECT * FROM dbo.[netsqlazman_ApplicationPermissionsTable] WHERE ApplicationId = @ApplicationId AND SqlUserOrRole = @SqlUserOrRole AND NetSqlAzManFixedServerRole = @NetSqlAzManFixedServerRole)
		BEGIN
		RAISERROR ('NetSqlAzManFixedServerRole updated.', -1, -1)
		RETURN 0
		END
	ELSE
		BEGIN
		INSERT INTO dbo.[netsqlazman_ApplicationPermissionsTable] (ApplicationId, SqlUserOrRole, IsSqlRole, NetSqlAzManFixedServerRole) VALUES (@ApplicationId, @SqlUserOrRole, @IsSqlRole, @NetSqlAzManFixedServerRole)
		RETURN SCOPE_IDENTITY()
		END
END
ELSE
	RAISERROR ('Application NOT Found or Application permission denied.', 16, 1)
GO
/****** Object:  UserDefinedFunction [dbo].[netsqlazman_ApplicationPermissions]    Script Date: 06/11/2009 17:45:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[netsqlazman_ApplicationPermissions]()
RETURNS TABLE 
AS  
RETURN
	SELECT     dbo.[netsqlazman_ApplicationPermissionsTable].*
	FROM         dbo.[netsqlazman_ApplicationPermissionsTable] INNER JOIN
	                      dbo.[netsqlazman_Applications]() Applications ON dbo.[netsqlazman_ApplicationPermissionsTable].ApplicationId = Applications.ApplicationId
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_ApplicationDelete]    Script Date: 06/11/2009 17:45:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_ApplicationDelete]
(
	@StoreId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ApplicationId FROM dbo.[netsqlazman_Applications]() WHERE ApplicationId = @ApplicationId) AND dbo.[netsqlazman_CheckStorePermissions](@StoreId, 2) = 1
	DELETE FROM [dbo].[netsqlazman_ApplicationsTable] WHERE [ApplicationId] = @ApplicationId AND [StoreId] = @StoreId
ELSE
	RAISERROR ('Store permission denied', 16, 1)
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_ApplicationInsert]    Script Date: 06/11/2009 17:45:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_ApplicationInsert]
(
	@StoreId int,
	@Name nvarchar(255),
	@Description nvarchar(1024)
)
AS
IF EXISTS(SELECT StoreId FROM dbo.[netsqlazman_Stores]() WHERE StoreId = @StoreId) AND dbo.[netsqlazman_CheckStorePermissions](@StoreId, 2) = 1
BEGIN
	INSERT INTO [dbo].[netsqlazman_ApplicationsTable] ([StoreId], [Name], [Description]) VALUES (@StoreId, @Name, @Description);
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Store permission denied.', 16, 1)
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_ApplicationUpdate]    Script Date: 06/11/2009 17:45:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_ApplicationUpdate]
(
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@Original_ApplicationId int
)
AS
IF EXISTS(SELECT ApplicationId FROM dbo.[netsqlazman_Applications]() WHERE ApplicationId = @Original_ApplicationId) AND dbo.[netsqlazman_CheckApplicationPermissions](@Original_ApplicationId, 2) = 1
	UPDATE [dbo].[netsqlazman_ApplicationsTable] SET [Name] = @Name, [Description] = @Description WHERE [ApplicationId] = @Original_ApplicationId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_StoreAttributeInsert]    Script Date: 06/11/2009 17:45:46 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_StoreAttributeInsert]
(
	@StoreId int,
	@AttributeKey nvarchar(255),
	@AttributeValue nvarchar(4000)
)
AS
IF EXISTS(Select StoreId FROM dbo.[netsqlazman_Stores]() WHERE StoreId = @StoreId) AND dbo.[netsqlazman_CheckStorePermissions](@StoreId, 2) = 1
BEGIN
	INSERT INTO [dbo].[netsqlazman_StoreAttributesTable] ([StoreId], [AttributeKey], [AttributeValue]) VALUES (@StoreId, @AttributeKey, @AttributeValue);
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Store permission denied.', 16, 1)
GO
/****** Object:  UserDefinedFunction [dbo].[netsqlazman_StoreAttributes]    Script Date: 06/11/2009 17:45:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[netsqlazman_StoreAttributes] ()
RETURNS TABLE
AS
RETURN
	SELECT     dbo.[netsqlazman_StoreAttributesTable].*
	FROM         dbo.[netsqlazman_StoreAttributesTable] INNER JOIN
	                      dbo.[netsqlazman_Stores]() Stores ON dbo.[netsqlazman_StoreAttributesTable].StoreId = Stores.StoreId
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_StorePermissionInsert]    Script Date: 06/11/2009 17:45:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_StorePermissionInsert]
(
	@StoreId int,
	@SqlUserOrRole nvarchar(128),
	@IsSqlRole bit,
	@NetSqlAzManFixedServerRole tinyint
)
AS
IF EXISTS(SELECT StoreId FROM dbo.[netsqlazman_Stores]() WHERE StoreId = @StoreId) AND dbo.[netsqlazman_CheckStorePermissions](@StoreId, 2) = 1
BEGIN
	INSERT INTO dbo.[netsqlazman_StorePermissionsTable] (StoreId, SqlUserOrRole, IsSqlRole, NetSqlAzManFixedServerRole) VALUES (@StoreId, @SqlUserOrRole, @IsSqlRole, @NetSqlAzManFixedServerRole)
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Store permission denied.', 16, 1)
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_StorePermissionDelete]    Script Date: 06/11/2009 17:45:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_StorePermissionDelete]
(
	@StorePermissionId int,
	@StoreId int
)
AS
IF EXISTS(SELECT StoreId FROM dbo.[netsqlazman_Stores]() WHERE StoreId = @StoreId) AND dbo.[netsqlazman_CheckStorePermissions](@StoreId, 2) = 1
	DELETE FROM dbo.[netsqlazman_StorePermissionsTable] WHERE StorePermissionId = @StorePermissionId AND StoreId = @StoreId
ELSE
	RAISERROR ('Store permission denied.', 16, 1)
GO
/****** Object:  UserDefinedFunction [dbo].[netsqlazman_StorePermissions]    Script Date: 06/11/2009 17:45:47 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[netsqlazman_StorePermissions]()
RETURNS TABLE 
AS  
RETURN
	SELECT     dbo.[netsqlazman_StorePermissionsTable].*
	FROM         dbo.[netsqlazman_StorePermissionsTable] INNER JOIN
	                      dbo.[netsqlazman_Stores]() Stores ON dbo.[netsqlazman_StorePermissionsTable].StoreId = Stores.StoreId
GO
/****** Object:  UserDefinedFunction [dbo].[netsqlazman_ApplicationAttributes]    Script Date: 06/11/2009 17:45:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[netsqlazman_ApplicationAttributes] ()
RETURNS TABLE
AS
RETURN 
	SELECT     dbo.[netsqlazman_ApplicationAttributesTable].*
	FROM         dbo.[netsqlazman_ApplicationAttributesTable] INNER JOIN
	                      dbo.[netsqlazman_Applications]() Applications ON dbo.[netsqlazman_ApplicationAttributesTable].ApplicationId = Applications.ApplicationId
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_ApplicationAttributeInsert]    Script Date: 06/11/2009 17:45:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_ApplicationAttributeInsert]
(
	@ApplicationId int,
	@AttributeKey nvarchar(255),
	@AttributeValue nvarchar(4000)
)
AS
IF EXISTS(SELECT ApplicationId FROM dbo.[netsqlazman_Applications]() WHERE ApplicationId = @ApplicationId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1
BEGIN
	INSERT INTO [dbo].[netsqlazman_ApplicationAttributesTable] ([ApplicationId], [AttributeKey], [AttributeValue]) VALUES (@ApplicationId, @AttributeKey, @AttributeValue)
	RETURN SCOPE_IDENTITY()
END
ELSE
BEGIN
	RAISERROR ('Application Permission denied.', 16, 1)
END
GO
/****** Object:  UserDefinedFunction [dbo].[netsqlazman_StoreGroups]    Script Date: 06/11/2009 17:45:48 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[netsqlazman_StoreGroups] ()
RETURNS TABLE
AS
RETURN
	SELECT     dbo.[netsqlazman_StoreGroupsTable].*
	FROM         dbo.[netsqlazman_Stores]() Stores INNER JOIN
	                      dbo.[netsqlazman_StoreGroupsTable] ON Stores.StoreId = dbo.[netsqlazman_StoreGroupsTable].StoreId
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_ItemInsert]    Script Date: 06/11/2009 17:45:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_ItemInsert]
(
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@ItemType tinyint,
	@BizRuleId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ApplicationId FROM dbo.[netsqlazman_Applications]() WHERE ApplicationId = @ApplicationId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1
BEGIN
	INSERT INTO [dbo].[netsqlazman_ItemsTable] ([ApplicationId], [Name], [Description], [ItemType], [BizRuleId]) VALUES (@ApplicationId, @Name, @Description, @ItemType, @BizRuleId)
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_StoreUpdate]    Script Date: 06/11/2009 17:45:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_StoreUpdate]
(
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@Original_StoreId int
)
AS
IF EXISTS(Select StoreId FROM dbo.[netsqlazman_Stores]() WHERE StoreId = @Original_StoreId) AND dbo.[netsqlazman_CheckStorePermissions](@Original_StoreId, 2) = 1
	UPDATE [dbo].[netsqlazman_StoresTable] SET [Name] = @Name, [Description] = @Description WHERE [StoreId] = @Original_StoreId
ELSE
	RAISERROR ('Store permission denied.', 16, 1)
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_StoreDelete]    Script Date: 06/11/2009 17:45:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_StoreDelete]
(
	@Original_StoreId int
)
AS
IF EXISTS(Select StoreId FROM dbo.[netsqlazman_Stores]() WHERE StoreId = @Original_StoreId) AND dbo.[netsqlazman_CheckStorePermissions](@Original_StoreId, 2) = 1
	DELETE FROM [dbo].[netsqlazman_StoresTable] WHERE [StoreId] = @Original_StoreId
ELSE
	RAISERROR ('Store permission denied.', 16, 1)
GO
/****** Object:  View [dbo].[netsqlazman_ApplicationsView]    Script Date: 06/11/2009 17:45:49 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[netsqlazman_ApplicationsView]
AS
SELECT     [netsqlazman_Stores].StoreId, [netsqlazman_Stores].Name AS StoreName, [netsqlazman_Stores].Description AS StoreDescription, [netsqlazman_Applications].ApplicationId, [netsqlazman_Applications].Name AS ApplicationName, 
                      [netsqlazman_Applications].Description AS ApplicationDescription
FROM         dbo.[netsqlazman_Applications]() [netsqlazman_Applications] INNER JOIN
                      dbo.[netsqlazman_Stores]() [netsqlazman_Stores] ON [netsqlazman_Applications].StoreId = [netsqlazman_Stores].StoreId
GO
/****** Object:  UserDefinedFunction [dbo].[netsqlazman_Items]    Script Date: 06/11/2009 17:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[netsqlazman_Items] ()
RETURNS TABLE
AS
RETURN
	SELECT     dbo.[netsqlazman_ItemsTable].*
	FROM         dbo.[netsqlazman_ItemsTable] INNER JOIN
	                      dbo.[netsqlazman_Applications]() Applications ON dbo.[netsqlazman_ItemsTable].ApplicationId = Applications.ApplicationId
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_CreateDelegate]    Script Date: 06/11/2009 17:45:50 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/****** Object:  Stored Procedure dbo.CreateDelegate    Script Date: 19/05/2006 19.11.19 ******/
CREATE PROCEDURE [dbo].[netsqlazman_CreateDelegate](@ITEMID INT, @OWNERSID VARBINARY(85), @OWNERSIDWHEREDEFINED TINYINT, @DELEGATEDUSERSID VARBINARY(85), @SIDWHEREDEFINED TINYINT, @AUTHORIZATIONTYPE TINYINT, @VALIDFROM DATETIME, @VALIDTO DATETIME, @AUTHORIZATIONID INT OUTPUT)
AS
DECLARE @ApplicationId int
SELECT @ApplicationId = ApplicationId FROM dbo.[netsqlazman_Items]() WHERE ItemId = @ItemId
IF @ApplicationId IS NOT NULL AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 1) = 1
BEGIN
	INSERT INTO dbo.[netsqlazman_AuthorizationsTable] (ItemId, ownerSid, ownerSidWhereDefined, objectSid, objectSidWhereDefined, AuthorizationType, ValidFrom, ValidTo)
		VALUES (@ITEMID, @OWNERSID, @OWNERSIDWHEREDEFINED, @DELEGATEDUSERSID, @SIDWHEREDEFINED, @AUTHORIZATIONTYPE, @VALIDFROM, @VALIDTO)
	SET @AUTHORIZATIONID = SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Item NOT Found or Application permission denied.', 16, 1)
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_AuthorizationInsert]    Script Date: 06/11/2009 17:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_AuthorizationInsert]
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
IF EXISTS(SELECT ItemId FROM dbo.[netsqlazman_Items]() WHERE ItemId = @ItemId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1
BEGIN
	INSERT INTO [dbo].[netsqlazman_AuthorizationsTable] ([ItemId], [ownerSid], [ownerSidWhereDefined], [objectSid], [objectSidWhereDefined], [AuthorizationType], [ValidFrom], [ValidTo]) VALUES (@ItemId, @ownerSid, @ownerSidWhereDefined, @objectSid, @objectSidWhereDefined, @AuthorizationType, @ValidFrom, @ValidTo)
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
/****** Object:  UserDefinedFunction [dbo].[netsqlazman_Authorizations]    Script Date: 06/11/2009 17:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[netsqlazman_Authorizations]()
RETURNS TABLE
AS
RETURN
	SELECT     dbo.[netsqlazman_AuthorizationsTable].*
	FROM         dbo.[netsqlazman_AuthorizationsTable] INNER JOIN
	                      dbo.[netsqlazman_Items]() Items ON dbo.[netsqlazman_AuthorizationsTable].ItemId = Items.ItemId
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_GetStoreGroupSidMembers]    Script Date: 06/11/2009 17:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_GetStoreGroupSidMembers](@ISMEMBER BIT, @GROUPOBJECTSID VARBINARY(85), @NETSQLAZMANMODE bit, @LDAPPATH nvarchar(4000), @member_cur CURSOR VARYING OUTPUT)
AS
DECLARE @RESULT TABLE (objectSid VARBINARY(85))
DECLARE @GROUPID INT
DECLARE @GROUPTYPE TINYINT
DECLARE @LDAPQUERY nvarchar(4000)
DECLARE @sub_members_cur CURSOR
DECLARE @OBJECTSID VARBINARY(85)
SELECT @GROUPID = StoreGroupId, @GROUPTYPE = GroupType, @LDAPQUERY = LDapQuery FROM dbo.[netsqlazman_StoreGroups]() WHERE objectSid = @GROUPOBJECTSID
IF @GROUPTYPE = 0 -- BASIC
BEGIN
	--memo: WhereDefined can be:0 - Store; 1 - Application; 2 - LDAP; 3 - Local; 4 - Database
	-- Windows SIDs
	INSERT INTO @RESULT (objectSid) 
	SELECT objectSid 
	FROM dbo.[netsqlazman_StoreGroupMembersTable]
	WHERE 
	StoreGroupId = @GROUPID AND IsMember = @ISMEMBER AND
	((@NETSQLAZMANMODE = 0 AND (WhereDefined = 2 OR WhereDefined = 4)) OR (@NETSQLAZMANMODE = 1 AND WhereDefined BETWEEN 2 AND 4))
	-- Store Groups Members
	DECLARE @MemberObjectSid VARBINARY(85)
	DECLARE @MemberType bit
	DECLARE @NotMemberType bit
	DECLARE nested_Store_groups_cur CURSOR LOCAL FAST_FORWARD FOR
		SELECT objectSid, IsMember FROM dbo.[netsqlazman_StoreGroupMembersTable] WHERE StoreGroupId = @GROUPID AND WhereDefined = 0
	
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
		EXEC dbo.[netsqlazman_GetStoreGroupSidMembers] @NotMemberType, @MemberObjectSid, @NETSQLAZMANMODE, @LDAPPATH, @sub_members_cur OUTPUT
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
	EXEC dbo.[netsqlazman_ExecuteLDAPQuery] @LDAPPATH, @LDAPQUERY, @sub_members_cur OUTPUT
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
/****** Object:  StoredProcedure [dbo].[netsqlazman_ApplicationGroupDelete]    Script Date: 06/11/2009 17:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_ApplicationGroupDelete]
(
	@ApplicationGroupId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ApplicationGroupId FROM dbo.[netsqlazman_ApplicationGroups]() WHERE ApplicationGroupId = @ApplicationGroupId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1
	DELETE FROM [dbo].[netsqlazman_ApplicationGroupsTable] WHERE [ApplicationGroupId] = @ApplicationGroupId AND [ApplicationId] = @ApplicationId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_ApplicationGroupUpdate]    Script Date: 06/11/2009 17:45:51 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_ApplicationGroupUpdate]
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
IF EXISTS(SELECT ApplicationGroupId FROM dbo.[netsqlazman_ApplicationGroups]() WHERE ApplicationGroupId = @Original_ApplicationGroupId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1
	UPDATE [dbo].[netsqlazman_ApplicationGroupsTable] SET [objectSid] = @objectSid, [Name] = @Name, [Description] = @Description, [LDapQuery] = @LDapQuery, [GroupType] = @GroupType WHERE [ApplicationGroupId] = @Original_ApplicationGroupId AND [ApplicationId] = @ApplicationId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_ItemAttributeInsert]    Script Date: 06/11/2009 17:45:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_ItemAttributeInsert]
(
	@ItemId int,
	@AttributeKey nvarchar(255),
	@AttributeValue nvarchar(4000),
	@ApplicationId int
)
AS
IF EXISTS(SELECT ItemId FROM dbo.[netsqlazman_Items]() WHERE ItemId = @ItemId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1
BEGIN
	INSERT INTO [dbo].[netsqlazman_ItemAttributesTable] ([ItemId], [AttributeKey], [AttributeValue]) VALUES (@ItemId, @AttributeKey, @AttributeValue)
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
/****** Object:  UserDefinedFunction [dbo].[netsqlazman_ItemAttributes]    Script Date: 06/11/2009 17:45:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[netsqlazman_ItemAttributes] ()
RETURNS TABLE
AS
RETURN
	SELECT     dbo.[netsqlazman_ItemAttributesTable].*
	FROM         dbo.[netsqlazman_ItemAttributesTable] INNER JOIN
	                      dbo.[netsqlazman_Items]() Items ON dbo.[netsqlazman_ItemAttributesTable].ItemId = Items.ItemId
GO
/****** Object:  UserDefinedFunction [dbo].[netsqlazman_StoreGroupMembers]    Script Date: 06/11/2009 17:45:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[netsqlazman_StoreGroupMembers] ()
RETURNS TABLE
AS
RETURN
	SELECT     dbo.[netsqlazman_StoreGroupMembersTable].*
	FROM         dbo.[netsqlazman_StoreGroupMembersTable] INNER JOIN
	                      dbo.[netsqlazman_StoreGroups]() StoreGroups ON dbo.[netsqlazman_StoreGroupMembersTable].StoreGroupId = StoreGroups.StoreGroupId
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_StoreGroupMemberInsert]    Script Date: 06/11/2009 17:45:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_StoreGroupMemberInsert]
(
	@StoreId int,
	@StoreGroupId int,
	@objectSid varbinary(85),
	@WhereDefined tinyint,
	@IsMember bit
)
AS
IF EXISTS(SELECT StoreGroupId FROM dbo.[netsqlazman_StoreGroups]() WHERE StoreGroupId = @StoreGroupId) AND dbo.[netsqlazman_CheckStorePermissions](@StoreId, 2) = 1
BEGIN
	INSERT INTO [dbo].[netsqlazman_StoreGroupMembersTable] ([StoreGroupId], [objectSid], [WhereDefined], [IsMember]) VALUES (@StoreGroupId, @objectSid, @WhereDefined, @IsMember)
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Store permission denied.', 16, 1)
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_StoreAttributeUpdate]    Script Date: 06/11/2009 17:45:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_StoreAttributeUpdate]
(
	@StoreId int,
	@AttributeKey nvarchar(255),
	@AttributeValue nvarchar(4000),
	@Original_StoreAttributeId int
)
AS
IF EXISTS(Select StoreAttributeId FROM dbo.[netsqlazman_StoreAttributes]() WHERE StoreAttributeId = @Original_StoreAttributeId) AND dbo.[netsqlazman_CheckStorePermissions](@StoreId, 2) = 1
	UPDATE [dbo].[netsqlazman_StoreAttributesTable] SET [AttributeKey] = @AttributeKey, [AttributeValue] = @AttributeValue WHERE [StoreAttributeId] = @Original_StoreAttributeId AND [StoreId] = @StoreId 
ELSE
	RAISERROR ('Store permission denied.', 16, 1)
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_StoreAttributeDelete]    Script Date: 06/11/2009 17:45:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_StoreAttributeDelete]
(
	@StoreId int,
	@StoreAttributeId int
)
AS
IF EXISTS(Select StoreAttributeId FROM dbo.[netsqlazman_StoreAttributes]() WHERE StoreAttributeId = @StoreAttributeId) AND dbo.[netsqlazman_CheckStorePermissions](@StoreId, 2) = 1
	DELETE FROM [dbo].[netsqlazman_StoreAttributesTable] WHERE [StoreAttributeId] = @StoreAttributeId AND [StoreId] = @StoreId
ELSE
	RAISERROR ('Store permission denied.', 16, 1)
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_ApplicationAttributeDelete]    Script Date: 06/11/2009 17:45:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_ApplicationAttributeDelete]
(
	@ApplicationId int,
	@ApplicationAttributeId int
)
AS
IF EXISTS(SELECT ApplicationAttributeId FROM dbo.[netsqlazman_ApplicationAttributes]() WHERE ApplicationAttributeId = @ApplicationAttributeId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1
	DELETE FROM [dbo].[netsqlazman_ApplicationAttributesTable] WHERE [ApplicationAttributeId] = @ApplicationAttributeId AND [ApplicationId] = @ApplicationId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_ApplicationAttributeUpdate]    Script Date: 06/11/2009 17:45:53 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_ApplicationAttributeUpdate]
(
	@ApplicationId int,
	@AttributeKey nvarchar(255),
	@AttributeValue nvarchar(4000),
	@Original_ApplicationAttributeId int
)
AS
IF EXISTS(SELECT ApplicationAttributeId FROM dbo.[netsqlazman_ApplicationAttributes]() WHERE ApplicationAttributeId = @Original_ApplicationAttributeId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1
	UPDATE [dbo].[netsqlazman_ApplicationAttributesTable] SET [AttributeKey] = @AttributeKey, [AttributeValue] = @AttributeValue WHERE [ApplicationAttributeId] = @Original_ApplicationAttributeId AND [ApplicationId] = @ApplicationId
ELSE
	RAISERROR ('Applicaction Permission denied.', 16, 1)
GO
/****** Object:  UserDefinedFunction [dbo].[netsqlazman_BizRules]    Script Date: 06/11/2009 17:45:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[netsqlazman_BizRules]()
RETURNS TABLE
AS
RETURN
	SELECT     dbo.[netsqlazman_BizRulesTable].*
	FROM         dbo.[netsqlazman_BizRulesTable] INNER JOIN
	                      dbo.[netsqlazman_Items]() Items ON dbo.[netsqlazman_BizRulesTable].BizRuleId = Items.BizRuleId
GO
/****** Object:  UserDefinedFunction [dbo].[netsqlazman_GetNameFromSid]    Script Date: 06/11/2009 17:45:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Andrea Ferendeles
-- Create date: 13/04/2006
-- Description:	Get Name From Sid
-- =============================================
CREATE FUNCTION [dbo].[netsqlazman_GetNameFromSid] (@StoreName nvarchar(255), @ApplicationName nvarchar(255), @sid varbinary(85), @SidWhereDefined tinyint)
RETURNS nvarchar(255)
AS
BEGIN

DECLARE @Name nvarchar(255)
SET @Name = NULL

IF (@SidWhereDefined=0) --Store
BEGIN
SET @Name = (SELECT TOP 1 Name FROM dbo.[netsqlazman_StoreGroups]() WHERE objectSid = @sid)
END
ELSE IF (@SidWhereDefined=1) --Application 
BEGIN
SET @Name = (SELECT TOP 1 Name FROM dbo.[netsqlazman_ApplicationGroups]() WHERE objectSid = @sid)
END
ELSE IF (@SidWhereDefined=2 OR @SidWhereDefined=3) --LDAP or LOCAL
BEGIN
SET @Name = (SELECT Suser_Sname(@sid))
END
ELSE IF (@SidWhereDefined=4) --Database
BEGIN
SET @Name = (SELECT DBUserName FROM dbo.[netsqlazman_GetDBUsers](@StoreName, @ApplicationName, @sid, NULL))
END
IF (@Name IS NULL)
BEGIN
	SET @Name = @sid
END
RETURN @Name
END
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_ItemsHierarchyInsert]    Script Date: 06/11/2009 17:45:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_ItemsHierarchyInsert]
(
	@ItemId int,
	@MemberOfItemId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ItemId FROM dbo.[netsqlazman_Items]() WHERE ItemId = @ItemId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1
BEGIN
	INSERT INTO [dbo].[netsqlazman_ItemsHierarchyTable] ([ItemId], [MemberOfItemId]) VALUES (@ItemId, @MemberOfItemId)
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_ItemsHierarchyDelete]    Script Date: 06/11/2009 17:45:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_ItemsHierarchyDelete]
(
	@ItemId int,
	@MemberOfItemId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ItemId FROM dbo.[netsqlazman_Items]() WHERE ItemId = @ItemId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1
	DELETE FROM [dbo].[netsqlazman_ItemsHierarchyTable] WHERE [ItemId] = @ItemId AND [MemberOfItemId] = @MemberOfItemId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
/****** Object:  UserDefinedFunction [dbo].[netsqlazman_ItemsHierarchy]    Script Date: 06/11/2009 17:45:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[netsqlazman_ItemsHierarchy] ()
RETURNS TABLE
AS
RETURN
	SELECT     dbo.[netsqlazman_ItemsHierarchyTable].*
	FROM         dbo.[netsqlazman_ItemsHierarchyTable] INNER JOIN
	                      dbo.[netsqlazman_Items]() Items ON dbo.[netsqlazman_ItemsHierarchyTable].ItemId = Items.ItemId
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_ReloadBizRule]    Script Date: 06/11/2009 17:45:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_ReloadBizRule]
(
	@ItemId int,
	@BizRuleId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ItemId FROM dbo.[netsqlazman_Items]() WHERE ItemId = @ItemId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1
	UPDATE [dbo].[netsqlazman_ItemsTable] SET BizRuleId = @BizRuleId WHERE [ItemId] = @ItemId AND [ApplicationId] = @ApplicationId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_ClearBizRule]    Script Date: 06/11/2009 17:45:54 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_ClearBizRule]
(
	@ItemId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ItemId FROM dbo.[netsqlazman_Items]() WHERE ItemId = @ItemId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1
	UPDATE [dbo].[netsqlazman_ItemsTable] SET BizRuleId = NULL WHERE [ItemId] = @ItemId AND [ApplicationId] = @ApplicationId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_ApplicationGroupMemberInsert]    Script Date: 06/11/2009 17:45:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_ApplicationGroupMemberInsert]
(
	@ApplicationGroupId int,
	@objectSid varbinary(85),
	@WhereDefined tinyint,
	@IsMember bit,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ApplicationGroupId FROM dbo.[netsqlazman_ApplicationGroups]() WHERE ApplicationGroupId = @ApplicationGroupId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1
BEGIN
	INSERT INTO [dbo].[netsqlazman_ApplicationGroupMembersTable] ([ApplicationGroupId], [objectSid], [WhereDefined], [IsMember]) VALUES (@ApplicationGroupId, @objectSid, @WhereDefined, @IsMember)
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_ItemUpdate]    Script Date: 06/11/2009 17:45:55 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_ItemUpdate]
(
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@ItemType tinyint,
	@Original_ItemId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ItemId FROM dbo.[netsqlazman_Items]() WHERE ItemId = @Original_ItemId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1
	UPDATE [dbo].[netsqlazman_ItemsTable] SET [Name] = @Name, [Description] = @Description, [ItemType] = @ItemType WHERE [ItemId] = @Original_ItemId AND [ApplicationId] = @ApplicationId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_ItemDelete]    Script Date: 06/11/2009 17:45:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_ItemDelete]
(
	@ItemId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ItemId FROM dbo.[netsqlazman_Items]() WHERE ItemId = @ItemId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1
	DELETE FROM [dbo].[netsqlazman_ItemsTable] WHERE [ItemId] = @ItemId AND [ApplicationId] = @ApplicationId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
/****** Object:  View [dbo].[netsqlazman_StoreAttributesView]    Script Date: 06/11/2009 17:45:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[netsqlazman_StoreAttributesView]
AS
SELECT     [netsqlazman_Stores].StoreId, [netsqlazman_Stores].Name, [netsqlazman_Stores].Description, [netsqlazman_StoreAttributes].StoreAttributeId, [netsqlazman_StoreAttributes].AttributeKey, [netsqlazman_StoreAttributes].AttributeValue
FROM         dbo.[netsqlazman_Stores]() [netsqlazman_Stores] INNER JOIN
                      dbo.[netsqlazman_StoreAttributes]() [netsqlazman_StoreAttributes] ON [netsqlazman_Stores].StoreId = [netsqlazman_StoreAttributes].StoreId
GO
/****** Object:  View [dbo].[netsqlazman_ApplicationAttributesView]    Script Date: 06/11/2009 17:45:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[netsqlazman_ApplicationAttributesView]
AS
SELECT     [netsqlazman_Applications].ApplicationId, [netsqlazman_Applications].StoreId, [netsqlazman_Applications].Name, [netsqlazman_Applications].Description, ApplicationAttributes.ApplicationAttributeId, 
                      ApplicationAttributes.AttributeKey, ApplicationAttributes.AttributeValue
FROM         dbo.[netsqlazman_Applications]() [netsqlazman_Applications] INNER JOIN
                      dbo.[netsqlazman_ApplicationAttributes]() ApplicationAttributes ON [netsqlazman_Applications].ApplicationId = ApplicationAttributes.ApplicationId
GO
/****** Object:  UserDefinedFunction [dbo].[netsqlazman_ApplicationGroupMembers]    Script Date: 06/11/2009 17:45:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[netsqlazman_ApplicationGroupMembers] ()
RETURNS TABLE
AS
RETURN
	SELECT     dbo.[netsqlazman_ApplicationGroupMembersTable].*
	FROM         dbo.[netsqlazman_ApplicationGroups]() ApplicationGroups INNER JOIN
	                      dbo.[netsqlazman_ApplicationGroupMembersTable] ON ApplicationGroups.ApplicationGroupId = dbo.[netsqlazman_ApplicationGroupMembersTable].ApplicationGroupId
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_DeleteDelegate]    Script Date: 06/11/2009 17:45:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_DeleteDelegate](@AUTHORIZATIONID INT, @OWNERSID VARBINARY(85))
AS
DECLARE @ApplicationId int
SELECT @ApplicationId = Items.ApplicationId FROM dbo.[netsqlazman_Items]() Items INNER JOIN dbo.[netsqlazman_Authorizations]() Authorizations ON Items.ItemId = Authorizations.ItemId WHERE Authorizations.AuthorizationId = @AUTHORIZATIONID
IF @ApplicationId IS NOT NULL AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 1) = 1
	DELETE FROM dbo.[netsqlazman_AuthorizationsTable] WHERE AuthorizationId = @AUTHORIZATIONID AND ownerSid = @OWNERSID
ELSE
	RAISERROR ('Item NOT Found or Application permission denied.', 16, 1)
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_AuthorizationUpdate]    Script Date: 06/11/2009 17:45:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_AuthorizationUpdate]
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
IF EXISTS(SELECT AuthorizationId FROM dbo.[netsqlazman_Authorizations]() WHERE AuthorizationId = @Original_AuthorizationId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1
	UPDATE [dbo].[netsqlazman_AuthorizationsTable] SET [ownerSid] = @ownerSid, [ownerSidWhereDefined] = @ownerSidWhereDefined, [objectSid] = @objectSid, [objectSidWhereDefined] = @objectSidWhereDefined, [AuthorizationType] = @AuthorizationType, [ValidFrom] = @ValidFrom, [ValidTo] = @ValidTo WHERE [AuthorizationId] = @Original_AuthorizationId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_AuthorizationDelete]    Script Date: 06/11/2009 17:45:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_AuthorizationDelete]
(
	@AuthorizationId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT AuthorizationId FROM dbo.[netsqlazman_Authorizations]() WHERE AuthorizationId = @AuthorizationId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1
	DELETE FROM [dbo].[netsqlazman_AuthorizationsTable] WHERE [AuthorizationId] = @AuthorizationId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_GetApplicationGroupSidMembers]    Script Date: 06/11/2009 17:45:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_GetApplicationGroupSidMembers](@ISMEMBER BIT, @GROUPOBJECTSID VARBINARY(85), @NETSQLAZMANMODE bit, @LDAPPATH nvarchar(4000), @member_cur CURSOR VARYING OUTPUT)
AS
DECLARE @RESULT TABLE (objectSid VARBINARY(85))
DECLARE @GROUPID INT
DECLARE @GROUPTYPE TINYINT
DECLARE @LDAPQUERY nvarchar(4000)
DECLARE @sub_members_cur CURSOR
DECLARE @OBJECTSID VARBINARY(85)
SELECT @GROUPID = ApplicationGroupId, @GROUPTYPE = GroupType, @LDAPQUERY = LDapQuery FROM [netsqlazman_ApplicationGroupsTable] WHERE objectSid = @GROUPOBJECTSID
IF @GROUPTYPE = 0 -- BASIC
BEGIN
	--memo: WhereDefined can be:0 - Store; 1 - Application; 2 - LDAP; 3 - Local; 4 - Database
	-- Windows SIDs
	INSERT INTO @RESULT (objectSid) 
	SELECT objectSid 
	FROM dbo.[netsqlazman_ApplicationGroupMembersTable]
	WHERE 
	ApplicationGroupId = @GROUPID AND IsMember = @ISMEMBER AND
	((@NETSQLAZMANMODE = 0 AND (WhereDefined = 2 OR WhereDefined = 4)) OR (@NETSQLAZMANMODE = 1 AND WhereDefined BETWEEN 2 AND 4))
	-- Store Groups Members
	DECLARE @MemberObjectSid VARBINARY(85)
	DECLARE @MemberType bit
	DECLARE @NotMemberType bit
	DECLARE nested_Store_groups_cur CURSOR LOCAL FAST_FORWARD FOR
		SELECT objectSid, IsMember FROM dbo.[netsqlazman_ApplicationGroupMembersTable] WHERE ApplicationGroupId = @GROUPID AND WhereDefined = 0
	
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
		EXEC dbo.[netsqlazman_GetStoreGroupSidMembers] @NotMemberType, @MemberObjectSid, @NETSQLAZMANMODE, @LDAPPATH, @sub_members_cur OUTPUT
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
		SELECT objectSid, IsMember FROM dbo.[netsqlazman_ApplicationGroupMembersTable] WHERE ApplicationGroupId = @GROUPID AND WhereDefined = 1
	
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
		EXEC dbo.[netsqlazman_GetApplicationGroupSidMembers] @NotMemberType, @MemberObjectSid, @NETSQLAZMANMODE, @LDAPPATH, @sub_members_cur OUTPUT
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
	EXEC dbo.[netsqlazman_ExecuteLDAPQuery] @LDAPPATH, @LDAPQUERY, @sub_members_cur OUTPUT
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
/****** Object:  StoredProcedure [dbo].[netsqlazman_ItemAttributeUpdate]    Script Date: 06/11/2009 17:45:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_ItemAttributeUpdate]
(
	@AttributeKey nvarchar(255),
	@AttributeValue nvarchar(4000),
	@Original_ItemAttributeId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ItemAttributeId FROM dbo.[netsqlazman_ItemAttributes]() WHERE ItemAttributeId = @Original_ItemAttributeId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1
	UPDATE [dbo].[netsqlazman_ItemAttributesTable] SET [AttributeKey] = @AttributeKey, [AttributeValue] = @AttributeValue WHERE [ItemAttributeId] = @Original_ItemAttributeId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_ItemAttributeDelete]    Script Date: 06/11/2009 17:45:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_ItemAttributeDelete]
(
	@ItemAttributeId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ItemAttributeId FROM dbo.[netsqlazman_ItemAttributes]() WHERE ItemAttributeId = @ItemAttributeId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1
	DELETE FROM [dbo].[netsqlazman_ItemAttributesTable] WHERE [ItemAttributeId] = @ItemAttributeId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_StoreGroupMemberDelete]    Script Date: 06/11/2009 17:45:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_StoreGroupMemberDelete]
(
	@StoreId int,
	@StoreGroupMemberId int
)
AS
IF EXISTS(SELECT StoreGroupMemberId FROM dbo.[netsqlazman_StoreGroupMembers]() WHERE StoreGroupMemberId = @StoreGroupMemberId) AND dbo.[netsqlazman_CheckStorePermissions](@StoreId, 2) = 1
	DELETE FROM [dbo].[netsqlazman_StoreGroupMembersTable] WHERE [StoreGroupMemberId] = @StoreGroupMemberId
ELSE
	RAISERROR ('Store permission denied.', 16, 1)
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_StoreGroupMemberUpdate]    Script Date: 06/11/2009 17:45:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_StoreGroupMemberUpdate]
(
	@StoreId int,
	@StoreGroupId int,
	@objectSid varbinary(85),
	@WhereDefined tinyint,
	@IsMember bit,
	@Original_StoreGroupMemberId int
)
AS
IF EXISTS(SELECT StoreGroupMemberId FROM dbo.[netsqlazman_StoreGroupMembers]() WHERE StoreGroupMemberId = @Original_StoreGroupMemberId) AND dbo.[netsqlazman_CheckStorePermissions](@StoreId, 2) = 1
	UPDATE [dbo].[netsqlazman_StoreGroupMembersTable] SET [StoreGroupId] = @StoreGroupId, [objectSid] = @objectSid, [WhereDefined] = @WhereDefined, [IsMember] = @IsMember WHERE [StoreGroupMemberId] = @Original_StoreGroupMemberId
ELSE
	RAISERROR ('Store permission denied.', 16, 1)
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_BizRuleUpdate]    Script Date: 06/11/2009 17:45:58 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_BizRuleUpdate]
(
	@BizRuleSource text,
	@BizRuleLanguage tinyint,
	@CompiledAssembly image,
	@Original_BizRuleId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT BizRuleId FROM dbo.[netsqlazman_BizRules]() WHERE BizRuleId = @Original_BizRuleId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1
	UPDATE [dbo].[netsqlazman_BizRulesTable] SET [BizRuleSource] = @BizRuleSource, [BizRuleLanguage] = @BizRuleLanguage, [CompiledAssembly] = @CompiledAssembly WHERE [BizRuleId] = @Original_BizRuleId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
/****** Object:  UserDefinedFunction [dbo].[netsqlazman_AuthorizationAttributes]    Script Date: 06/11/2009 17:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[netsqlazman_AuthorizationAttributes] ()
RETURNS TABLE
AS
RETURN
	SELECT     dbo.[netsqlazman_AuthorizationAttributesTable].*
	FROM         dbo.[netsqlazman_AuthorizationAttributesTable] INNER JOIN
	                      dbo.[netsqlazman_Authorizations]() as Authorizations ON dbo.[netsqlazman_AuthorizationAttributesTable].AuthorizationId = Authorizations.AuthorizationId
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_AuthorizationAttributeInsert]    Script Date: 06/11/2009 17:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_AuthorizationAttributeInsert]
(
	@AuthorizationId int,
	@AttributeKey nvarchar(255),
	@AttributeValue nvarchar(4000),
	@ApplicationId int
)
AS
IF EXISTS(SELECT AuthorizationId FROM dbo.[netsqlazman_Authorizations]() WHERE AuthorizationId = @AuthorizationId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 1) = 1
BEGIN
	INSERT INTO [dbo].[netsqlazman_AuthorizationAttributesTable] ([AuthorizationId], [AttributeKey], [AttributeValue]) VALUES (@AuthorizationId, @AttributeKey, @AttributeValue)
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_ApplicationGroupMemberDelete]    Script Date: 06/11/2009 17:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_ApplicationGroupMemberDelete]
(
	@ApplicationGroupMemberId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ApplicationGroupMemberId FROM dbo.[netsqlazman_ApplicationGroupMembers]() WHERE ApplicationGroupMemberId = @ApplicationGroupMemberId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1
	DELETE FROM [dbo].[netsqlazman_ApplicationGroupMembersTable] WHERE [ApplicationGroupMemberId] = @ApplicationGroupMemberId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_ApplicationGroupMemberUpdate]    Script Date: 06/11/2009 17:45:59 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_ApplicationGroupMemberUpdate]
(
	@ApplicationGroupId int,
	@objectSid varbinary(85),
	@WhereDefined tinyint,
	@IsMember bit,
	@Original_ApplicationGroupMemberId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ApplicationGroupMemberId FROM dbo.[netsqlazman_ApplicationGroupMembers]() WHERE ApplicationGroupMemberId = @Original_ApplicationGroupMemberId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1
	UPDATE [dbo].[netsqlazman_ApplicationGroupMembersTable] SET [objectSid] = @objectSid, [WhereDefined] = @WhereDefined, [IsMember] = @IsMember WHERE [ApplicationGroupMemberId] = @Original_ApplicationGroupMemberId
ELSE	
	RAISERROR ('Application permission denied.', 16, 1)
GO
/****** Object:  View [dbo].[netsqlazman_StoreGroupMembersView]    Script Date: 06/11/2009 17:46:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[netsqlazman_StoreGroupMembersView]
AS
SELECT     StoreGroupMembers.StoreGroupMemberId, StoreGroupMembers.StoreGroupId, StoreGroups.Name AS StoreGroup, dbo.[netsqlazman_GetNameFromSid](Stores.Name, NULL, 
                      StoreGroupMembers.objectSid, StoreGroupMembers.WhereDefined) AS Name, StoreGroupMembers.objectSid, 
                      CASE WhereDefined WHEN 0 THEN 'Store' WHEN 1 THEN 'Application' WHEN 2 THEN 'LDap' WHEN 3 THEN 'Local' WHEN 4 THEN 'DATABASE' END AS WhereDefined,
                       CASE IsMember WHEN 1 THEN 'Member' WHEN 0 THEN 'Non-Member' END AS MemberType
FROM         dbo.[netsqlazman_StoreGroupMembers]() StoreGroupMembers INNER JOIN
                      dbo.[netsqlazman_StoreGroups]() StoreGroups ON StoreGroupMembers.StoreGroupId = StoreGroups.StoreGroupId INNER JOIN
                      dbo.[netsqlazman_Stores]() Stores ON StoreGroups.StoreId = Stores.StoreId
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_BuildUserPermissionCache]    Script Date: 06/11/2009 17:46:00 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_BuildUserPermissionCache](@STORENAME nvarchar(255), @APPLICATIONNAME nvarchar(255))
AS 
-- Hierarchy
SET NOCOUNT ON
SELECT     Items.Name AS ItemName, Items_1.Name AS ParentItemName
FROM         dbo.[netsqlazman_Items]() Items_1 INNER JOIN
                      dbo.[netsqlazman_ItemsHierarchy]() ItemsHierarchy ON Items_1.ItemId = ItemsHierarchy.MemberOfItemId RIGHT OUTER JOIN
                      dbo.[netsqlazman_Applications]() Applications INNER JOIN
                      dbo.[netsqlazman_Stores]() Stores ON Applications.StoreId = Stores.StoreId INNER JOIN
                      dbo.[netsqlazman_Items]() Items ON Applications.ApplicationId = Items.ApplicationId ON ItemsHierarchy.ItemId = Items.ItemId
WHERE     (Stores.Name = @STORENAME) AND (Applications.Name = @APPLICATIONNAME)

-- Item Authorizations
SELECT DISTINCT Items.Name AS ItemName, Authorizations.ValidFrom, Authorizations.ValidTo
FROM         dbo.[netsqlazman_Authorizations]() Authorizations INNER JOIN
                      dbo.[netsqlazman_Items]() Items ON Authorizations.ItemId = Items.ItemId INNER JOIN
                      dbo.[netsqlazman_Stores]() Stores INNER JOIN
                      dbo.[netsqlazman_Applications]() Applications ON Stores.StoreId = Applications.StoreId ON Items.ApplicationId = Applications.ApplicationId
WHERE     (Authorizations.AuthorizationType <> 0) AND (Stores.Name = @STORENAME) AND (Applications.Name = @APPLICATIONNAME)
UNION
SELECT DISTINCT Items.Name AS ItemName, NULL ValidFrom, NULL ValidTo
FROM         dbo.[netsqlazman_Items]() Items INNER JOIN
                      dbo.[netsqlazman_Stores]() Stores INNER JOIN
                      dbo.[netsqlazman_Applications]() Applications ON Stores.StoreId = Applications.StoreId ON Items.ApplicationId = Applications.ApplicationId
WHERE     (Stores.Name = @STORENAME) AND (Applications.Name = @APPLICATIONNAME) AND Items.BizRuleId IS NOT NULL
SET NOCOUNT OFF
GO
/****** Object:  View [dbo].[netsqlazman_AuthorizationView]    Script Date: 06/11/2009 17:46:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[netsqlazman_AuthorizationView]
AS
SELECT     [netsqlazman_Authorizations].AuthorizationId, [netsqlazman_Authorizations].ItemId, dbo.[netsqlazman_GetNameFromSid]([netsqlazman_Stores].Name, [netsqlazman_Applications].Name, [netsqlazman_Authorizations].ownerSid, 
                      [netsqlazman_Authorizations].ownerSidWhereDefined) AS Owner, dbo.[netsqlazman_GetNameFromSid]([netsqlazman_Stores].Name, [netsqlazman_Applications].Name, [netsqlazman_Authorizations].objectSid, 
                      [netsqlazman_Authorizations].objectSidWhereDefined) AS Name, [netsqlazman_Authorizations].objectSid, 
                      CASE objectSidWhereDefined WHEN 0 THEN 'Store' WHEN 1 THEN 'Application' WHEN 2 THEN 'LDAP' WHEN 3 THEN 'Local' WHEN 4 THEN 'DATABASE' END AS SidWhereDefined,
                       CASE AuthorizationType WHEN 0 THEN 'NEUTRAL' WHEN 1 THEN 'ALLOW' WHEN 2 THEN 'DENY' WHEN 3 THEN 'ALLOWWITHDELEGATION' END AS AuthorizationType,
                       [netsqlazman_Authorizations].ValidFrom, [netsqlazman_Authorizations].ValidTo
FROM         dbo.[netsqlazman_Authorizations]() [netsqlazman_Authorizations] INNER JOIN
                      dbo.[netsqlazman_Items]() [netsqlazman_Items] ON [netsqlazman_Authorizations].ItemId = [netsqlazman_Items].ItemId INNER JOIN
                      dbo.[netsqlazman_Applications]() [netsqlazman_Applications] ON [netsqlazman_Items].ApplicationId = [netsqlazman_Applications].ApplicationId INNER JOIN
                      dbo.[netsqlazman_Stores]() [netsqlazman_Stores] ON [netsqlazman_Applications].StoreId = [netsqlazman_Stores].StoreId
GO
/****** Object:  View [dbo].[netsqlazman_ApplicationGroupMembersView]    Script Date: 06/11/2009 17:46:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[netsqlazman_ApplicationGroupMembersView]
AS
SELECT     [netsqlazman_Stores].StoreId, [netsqlazman_Applications].ApplicationId, [netsqlazman_ApplicationGroupMembers].ApplicationGroupMemberId, [netsqlazman_ApplicationGroupMembers].ApplicationGroupId, 
                      [netsqlazman_ApplicationGroups].Name AS ApplicationGroup, dbo.[netsqlazman_GetNameFromSid]([netsqlazman_Stores].Name, [netsqlazman_Applications].Name, [netsqlazman_ApplicationGroupMembers].objectSid, 
                      [netsqlazman_ApplicationGroupMembers].WhereDefined) AS Name, [netsqlazman_ApplicationGroupMembers].objectSid, 
                      CASE WhereDefined WHEN 0 THEN 'Store' WHEN 1 THEN 'Application' WHEN 2 THEN 'LDap' WHEN 3 THEN 'Local' WHEN 4 THEN 'DATABASE' END AS WhereDefined,
                       CASE IsMember WHEN 1 THEN 'Member' WHEN 0 THEN 'Non-Member' END AS MemberType
FROM         dbo.[netsqlazman_ApplicationGroupMembers]() [netsqlazman_ApplicationGroupMembers] INNER JOIN
                      dbo.[netsqlazman_ApplicationGroups]() [netsqlazman_ApplicationGroups] ON [netsqlazman_ApplicationGroupMembers].ApplicationGroupId = [netsqlazman_ApplicationGroups].ApplicationGroupId INNER JOIN
                      dbo.[netsqlazman_Applications]() [netsqlazman_Applications] ON [netsqlazman_ApplicationGroups].ApplicationId = [netsqlazman_Applications].ApplicationId INNER JOIN
                      dbo.[netsqlazman_Stores]() [netsqlazman_Stores] ON [netsqlazman_Applications].StoreId = [netsqlazman_Stores].StoreId
GO
/****** Object:  View [dbo].[netsqlazman_ItemsHierarchyView]    Script Date: 06/11/2009 17:46:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[netsqlazman_ItemsHierarchyView]
AS
SELECT     [netsqlazman_Items].ItemId, [netsqlazman_Items].ApplicationId, [netsqlazman_Items].Name, [netsqlazman_Items].Description, 
                      CASE [netsqlazman_Items].ItemType WHEN 0 THEN 'Role' WHEN 1 THEN 'Task' WHEN 2 THEN 'Operation' END AS ItemType, Items_1.ItemId AS MemberItemId, 
                      Items_1.ApplicationId AS MemberApplicationId, Items_1.Name AS MemberName, Items_1.Description AS MemberDescription, 
                      CASE Items_1.ItemType WHEN 0 THEN 'Role' WHEN 1 THEN 'Task' WHEN 2 THEN 'Operation' END AS MemberType
FROM         dbo.[netsqlazman_Items]() Items_1 INNER JOIN
                      dbo.[netsqlazman_ItemsHierarchy]() [netsqlazman_ItemsHierarchy] ON Items_1.ItemId = [netsqlazman_ItemsHierarchy].ItemId INNER JOIN
                      dbo.[netsqlazman_Items]() [netsqlazman_Items] ON [netsqlazman_ItemsHierarchy].MemberOfItemId = [netsqlazman_Items].ItemId INNER JOIN
                      dbo.[netsqlazman_Applications]() [netsqlazman_Applications] ON [netsqlazman_Items].ApplicationId = [netsqlazman_Applications].ApplicationId
GO
/****** Object:  View [dbo].[netsqlazman_BizRuleView]    Script Date: 06/11/2009 17:46:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[netsqlazman_BizRuleView]
AS
SELECT     [netsqlazman_Items].ItemId, [netsqlazman_Items].ApplicationId, [netsqlazman_Items].Name, [netsqlazman_Items].Description, [netsqlazman_Items].ItemType, [netsqlazman_BizRules].BizRuleSource, [netsqlazman_BizRules].BizRuleLanguage, 
                      [netsqlazman_BizRules].CompiledAssembly
FROM         dbo.[netsqlazman_Items]() [netsqlazman_Items] INNER JOIN
                      dbo.[netsqlazman_BizRules]() [netsqlazman_BizRules] ON [netsqlazman_Items].BizRuleId = [netsqlazman_BizRules].BizRuleId
GO
/****** Object:  View [dbo].[netsqlazman_ItemAttributesView]    Script Date: 06/11/2009 17:46:03 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[netsqlazman_ItemAttributesView]
AS
SELECT     [netsqlazman_Items].ItemId, [netsqlazman_Items].ApplicationId, [netsqlazman_Items].Name, [netsqlazman_Items].Description, 
                      CASE [netsqlazman_Items].ItemType WHEN 0 THEN 'Role' WHEN 1 THEN 'Task' WHEN 2 THEN 'Operation' END AS ItemType, [netsqlazman_ItemAttributes].ItemAttributeId, 
                      [netsqlazman_ItemAttributes].AttributeKey, [netsqlazman_ItemAttributes].AttributeValue
FROM         dbo.[netsqlazman_Items]() [netsqlazman_Items] INNER JOIN
                      dbo.[netsqlazman_ItemAttributes]() [netsqlazman_ItemAttributes] ON [netsqlazman_Items].ItemId = [netsqlazman_ItemAttributes].ItemId
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_CheckAccess]    Script Date: 06/11/2009 17:46:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_CheckAccess] (@ITEMID INT, @USERSID VARBINARY(85), @VALIDFOR DATETIME, @LDAPPATH nvarchar(4000), @AUTHORIZATION_TYPE TINYINT OUTPUT, @NETSQLAZMANMODE BIT, @RETRIEVEATTRIBUTES BIT) 
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
DECLARE ItemsWhereIAmAMember_cur CURSOR LOCAL FAST_FORWARD READ_ONLY FOR SELECT MemberOfItemId FROM dbo.[netsqlazman_ItemsHierarchyTable] WHERE ItemId = @ITEMID
OPEN ItemsWhereIAmAMember_cur
FETCH NEXT FROM ItemsWhereIAmAMember_cur INTO @PARENTITEMID
WHILE @@FETCH_STATUS = 0
BEGIN
	-- Recursive Call
	EXEC dbo.[netsqlazman_CheckAccess] @PARENTITEMID, @USERSID, @VALIDFOR, @LDAPPATH, @PARENTRESULT OUTPUT, @NETSQLAZMANMODE, @RETRIEVEATTRIBUTES
	SELECT @AUTHORIZATION_TYPE = dbo.[netsqlazman_MergeAuthorizations](@AUTHORIZATION_TYPE, @PARENTRESULT)
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
	INSERT INTO #ATTRIBUTES_TABLE SELECT AttributeKey, AttributeValue, @ITEMID FROM dbo.[netsqlazman_ItemAttributesTable] WHERE ItemId = @ITEMID
---------------------------------------------
-- CHECK ACCESS ON ITEM
-- AuthorizationType can be:  0 - Neutral; 1 - Allow; 2 - Deny; 3 - AllowWithDelegation
-- objectSidWhereDefined can be:0 - Store; 1 - Application; 2 - LDAP; 3 - Local; 4 - Database
DECLARE @PARTIAL_RESULT TINYINT
--CHECK ACCESS FOR USER AUTHORIZATIONS
DECLARE checkaccessonitem_cur CURSOR  LOCAL FAST_FORWARD READ_ONLY FOR 
	SELECT AuthorizationType, AuthorizationId
	FROM dbo.[netsqlazman_AuthorizationsTable] WHERE 
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
		SELECT @AUTHORIZATION_TYPE = dbo.[netsqlazman_MergeAuthorizations](@AUTHORIZATION_TYPE, @PARTIAL_RESULT)
		SELECT @ITEM_AUTHORIZATION_TYPE  = dbo.[netsqlazman_MergeAuthorizations](@ITEM_AUTHORIZATION_TYPE, @PARTIAL_RESULT)
		IF @RETRIEVEATTRIBUTES = 1 
			INSERT INTO #ATTRIBUTES_TABLE SELECT AttributeKey, AttributeValue, NULL FROM dbo.[netsqlazman_AuthorizationAttributesTable] WHERE AuthorizationId = @PKID
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
	FROM dbo.[netsqlazman_AuthorizationsTable] Authorizations INNER JOIN #USERGROUPS as usergroups
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
		SELECT @AUTHORIZATION_TYPE = dbo.[netsqlazman_MergeAuthorizations](@AUTHORIZATION_TYPE, @PARTIAL_RESULT)
		SELECT @ITEM_AUTHORIZATION_TYPE = dbo.[netsqlazman_MergeAuthorizations](@ITEM_AUTHORIZATION_TYPE, @PARTIAL_RESULT)
		IF @RETRIEVEATTRIBUTES = 1
			INSERT INTO #ATTRIBUTES_TABLE SELECT AttributeKey, AttributeValue, NULL FROM dbo.[netsqlazman_AuthorizationAttributesTable] WHERE AuthorizationId = @PKID
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
FOR 	SELECT objectSid, objectSidWhereDefined, AuthorizationType, AuthorizationId FROM dbo.[netsqlazman_AuthorizationsTable]
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

	EXEC dbo.[netsqlazman_GetStoreGroupSidMembers] 0, @GROUPOBJECTSID, @NETSQLAZMANMODE, @LDAPPATH, @members_cur OUTPUT -- non-members
	FETCH NEXT FROM @members_cur INTO @OBJECTSID
	WHILE @@FETCH_STATUS=0
	BEGIN
		INSERT INTO @GROUPSIDMEMBERS VALUES (@OBJECTSID)
		FETCH NEXT FROM @members_cur INTO @OBJECTSID
	END
	CLOSE @members_cur
	DEALLOCATE @members_cur

	IF EXISTS(SELECT * FROM @GROUPSIDMEMBERS WHERE objectSid = @USERSID) OR
	     EXISTS(SELECT * FROM @GROUPSIDMEMBERS groupsidmembers INNER JOIN #USERGROUPS as usergroups ON groupsidmembers.objectSid = usergroups.objectSid)
	BEGIN
	-- user is a non-member
	SET @ISMEMBER = 0
	END
	IF @ISMEMBER = 1
	BEGIN
		DELETE FROM @GROUPSIDMEMBERS

		EXEC dbo.[netsqlazman_GetStoreGroupSidMembers] 1, @GROUPOBJECTSID, @NETSQLAZMANMODE, @LDAPPATH, @members_cur OUTPUT -- members
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
		SET @AUTHORIZATION_TYPE = (SELECT dbo.[netsqlazman_MergeAuthorizations](@AUTHORIZATION_TYPE, @PARTIAL_RESULT))
		SET @ITEM_AUTHORIZATION_TYPE = (SELECT dbo.[netsqlazman_MergeAuthorizations](@ITEM_AUTHORIZATION_TYPE, @PARTIAL_RESULT))
		IF @PKID IS NOT NULL AND @RETRIEVEATTRIBUTES = 1
			INSERT INTO #ATTRIBUTES_TABLE SELECT AttributeKey, AttributeValue, NULL FROM dbo.[netsqlazman_AuthorizationAttributesTable] WHERE AuthorizationId = @PKID
	END
END
	ELSE
IF @GROUPWHEREDEFINED = 1 -- application group members
BEGIN
	--application groups members of type 'non-member'
	DELETE FROM @GROUPSIDMEMBERS

	EXEC dbo.[netsqlazman_GetApplicationGroupSidMembers] 0, @GROUPOBJECTSID, @NETSQLAZMANMODE, @LDAPPATH, @members_cur OUTPUT -- non-members
	FETCH NEXT FROM @members_cur INTO @OBJECTSID
	WHILE @@FETCH_STATUS=0
	BEGIN
		INSERT INTO @GROUPSIDMEMBERS VALUES (@OBJECTSID)
		FETCH NEXT FROM @members_cur INTO @OBJECTSID
	END
	CLOSE @members_cur
	DEALLOCATE @members_cur

	IF EXISTS(SELECT * FROM @GROUPSIDMEMBERS WHERE objectSid = @USERSID) OR
	     EXISTS (SELECT* FROM @GROUPSIDMEMBERS groupsidmembers INNER JOIN #USERGROUPS as usergroups ON groupsidmembers.objectSid = usergroups.objectSid)
	BEGIN	-- user is a non-member
	SET @ISMEMBER = 0
	END
	IF @ISMEMBER = 1 
	BEGIN
		DELETE FROM @GROUPSIDMEMBERS

		EXEC dbo.[netsqlazman_GetApplicationGroupSidMembers] 1, @GROUPOBJECTSID, @NETSQLAZMANMODE, @LDAPPATH, @members_cur OUTPUT -- members
		FETCH NEXT FROM @members_cur INTO @OBJECTSID
		WHILE @@FETCH_STATUS=0
		BEGIN
			INSERT INTO @GROUPSIDMEMBERS VALUES (@OBJECTSID)
			FETCH NEXT FROM @members_cur INTO @OBJECTSID
		END
		CLOSE @members_cur
		DEALLOCATE @members_cur

		IF EXISTS(SELECT * FROM @GROUPSIDMEMBERS WHERE objectSid = @USERSID) OR
		     EXISTS (SELECT * FROM @GROUPSIDMEMBERS groupsidmembers INNER JOIN #USERGROUPS as usergroups ON groupsidmembers.objectSid = usergroups.objectSid)
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
		SET @AUTHORIZATION_TYPE = (SELECT dbo.[netsqlazman_MergeAuthorizations](@AUTHORIZATION_TYPE, @PARTIAL_RESULT))
		SET @ITEM_AUTHORIZATION_TYPE = (SELECT dbo.[netsqlazman_MergeAuthorizations](@ITEM_AUTHORIZATION_TYPE, @PARTIAL_RESULT))
		IF @PKID IS NOT NULL AND @RETRIEVEATTRIBUTES = 1 
			INSERT INTO #ATTRIBUTES_TABLE SELECT AttributeKey, AttributeValue, NULL FROM dbo.[netsqlazman_AuthorizationAttributesTable] WHERE AuthorizationId = @PKID
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
FROM         dbo.[netsqlazman_ItemsTable] Items LEFT OUTER JOIN
                      dbo.[netsqlazman_BizRulesTable] BizRules ON Items.BizRuleId = BizRules.BizRuleId WHERE Items.ItemId = @ITEMID
SET NOCOUNT OFF
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_AuthorizationAttributeDelete]    Script Date: 06/11/2009 17:46:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_AuthorizationAttributeDelete]
(
	@AuthorizationAttributeId int,
	@ApplicationId int
)
AS
IF  EXISTS(SELECT AuthorizationAttributeId FROM dbo.[netsqlazman_AuthorizationAttributes]() WHERE AuthorizationAttributeId = @AuthorizationAttributeId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 1) = 1
	DELETE FROM [dbo].[netsqlazman_AuthorizationAttributesTable] WHERE [AuthorizationAttributeId] = @AuthorizationAttributeId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_AuthorizationAttributeUpdate]    Script Date: 06/11/2009 17:46:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_AuthorizationAttributeUpdate]
(
	@AttributeKey nvarchar(255),
	@AttributeValue nvarchar(4000),
	@Original_AuthorizationAttributeId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT AuthorizationAttributeId FROM dbo.[netsqlazman_AuthorizationAttributes]() WHERE AuthorizationAttributeId = @Original_AuthorizationAttributeId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 1) = 1
	UPDATE [dbo].[netsqlazman_AuthorizationAttributesTable] SET [AttributeKey] = @AttributeKey, [AttributeValue] = @AttributeValue WHERE [AuthorizationAttributeId] = @Original_AuthorizationAttributeId
ELSE
	RAISERROR ('Application permission denied.', 16 ,1)
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_IsAMemberOfGroup]    Script Date: 06/11/2009 17:46:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_IsAMemberOfGroup](@GROUPTYPE bit, @GROUPOBJECTSID VARBINARY(85), @NETSQLAZMANMODE bit, @LDAPPATH nvarchar(4000), @TOKEN IMAGE, @USERGROUPSCOUNT INT)  
AS  
DECLARE @member_cur CURSOR
DECLARE @MemberSid VARBINARY(85)
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
	EXEC dbo.[netsqlazman_GetStoreGroupSidMembers] 0, @GROUPOBJECTSID, @NETSQLAZMANMODE, @LDAPPATH, @member_cur OUTPUT
ELSE -- APPLICATON GROUP
	EXEC dbo.[netsqlazman_GetApplicationGroupSidMembers] 0, @GROUPOBJECTSID, @NETSQLAZMANMODE, @LDAPPATH, @member_cur OUTPUT

FETCH NEXT FROM @member_cur INTO @MemberSid
WHILE @@FETCH_STATUS = 0
BEGIN
	IF @MemberSid = @USERSID
	BEGIN
		CLOSE @member_cur
		DEALLOCATE @member_cur
		SELECT CONVERT(bit, 0) -- true
		RETURN
	END		
	SELECT @COUNT =  COUNT(*)  FROM @USERGROUPS WHERE objectSid = @MemberSid
	IF @COUNT>0
	BEGIN
		CLOSE @member_cur
		DEALLOCATE @member_cur
		SELECT CONVERT(bit, 0) -- true
		RETURN
	END		
	FETCH NEXT FROM @member_cur INTO @MemberSid
END
CLOSE @member_cur
DEALLOCATE @member_cur

-- CHECK IF IS A MEMBER
IF @GROUPTYPE = 0 -- STORE GROUP
	EXEC dbo.[netsqlazman_GetStoreGroupSidMembers] 1, @GROUPOBJECTSID, @NETSQLAZMANMODE, @LDAPPATH, @member_cur OUTPUT
ELSE -- APPLICATON GROUP
	EXEC dbo.[netsqlazman_GetApplicationGroupSidMembers] 1, @GROUPOBJECTSID, @NETSQLAZMANMODE, @LDAPPATH, @member_cur OUTPUT

FETCH NEXT FROM @member_cur INTO @MemberSid
WHILE @@FETCH_STATUS = 0
BEGIN
	IF @MemberSid = @USERSID
	BEGIN
		CLOSE @member_cur
		DEALLOCATE @member_cur
		SELECT CONVERT(bit,1) -- true
		RETURN
	END		
	SELECT @COUNT =  COUNT(*)  FROM @USERGROUPS WHERE objectSid = @MemberSid
	IF @COUNT>0
	BEGIN
		CLOSE @member_cur
		DEALLOCATE @member_cur
		SELECT CONVERT(bit, 1) -- true
		RETURN
	END		
	FETCH NEXT FROM @member_cur INTO @MemberSid
END
CLOSE @member_cur
DEALLOCATE @member_cur

SELECT CONVERT(bit, 0) -- true
GO
/****** Object:  View [dbo].[netsqlazman_AuthorizationAttributesView]    Script Date: 06/11/2009 17:46:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[netsqlazman_AuthorizationAttributesView]
AS
SELECT     dbo.[netsqlazman_AuthorizationView].AuthorizationId, dbo.[netsqlazman_AuthorizationView].ItemId, dbo.[netsqlazman_AuthorizationView].Owner, dbo.[netsqlazman_AuthorizationView].Name, dbo.[netsqlazman_AuthorizationView].objectSid, 
                      dbo.[netsqlazman_AuthorizationView].SidWhereDefined, dbo.[netsqlazman_AuthorizationView].AuthorizationType, dbo.[netsqlazman_AuthorizationView].ValidFrom, dbo.[netsqlazman_AuthorizationView].ValidTo, 
                      [netsqlazman_AuthorizationAttributes].AuthorizationAttributeId, [netsqlazman_AuthorizationAttributes].AttributeKey, [netsqlazman_AuthorizationAttributes].AttributeValue
FROM         dbo.[netsqlazman_AuthorizationView] INNER JOIN
                      dbo.[netsqlazman_AuthorizationAttributes]() [netsqlazman_AuthorizationAttributes] ON dbo.[netsqlazman_AuthorizationView].AuthorizationId = [netsqlazman_AuthorizationAttributes].AuthorizationId
GO
/****** Object:  StoredProcedure [dbo].[netsqlazman_DirectCheckAccess]    Script Date: 06/11/2009 17:46:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[netsqlazman_DirectCheckAccess] (@STORENAME nvarchar(255), @APPLICATIONNAME nvarchar(255), @ITEMNAME nvarchar(255), @OPERATIONSONLY BIT, @TOKEN IMAGE, @USERGROUPSCOUNT INT, @VALIDFOR DATETIME, @LDAPPATH nvarchar(4000), @AUTHORIZATION_TYPE TINYINT OUTPUT, @RETRIEVEATTRIBUTES BIT) 
AS
--Memo: 0 - Role; 1 - Task; 2 - Operation
SET NOCOUNT ON
DECLARE @STOREID int
DECLARE @ApplicationId int
DECLARE @ITEMID INT

-- CHECK STORE EXISTANCE/PERMISSIONS
Select @STOREID = StoreId FROM dbo.[netsqlazman_Stores]() WHERE Name = @STORENAME
IF @STOREID IS NULL
	BEGIN
	RAISERROR ('Store not found or Store permission denied.', 16, 1)
	RETURN 1
	END
-- CHECK APPLICATION EXISTANCE/PERMISSIONS
Select @ApplicationId = ApplicationId FROM dbo.[netsqlazman_Applications]() WHERE Name = @APPLICATIONNAME And StoreId = @STOREID
IF @ApplicationId IS NULL
	BEGIN
	RAISERROR ('Application not found or Application permission denied.', 16, 1)
	RETURN 1
	END

SELECT @ITEMID = Items.ItemId
	FROM         dbo.[netsqlazman_Applications]() Applications INNER JOIN
	                      dbo.[netsqlazman_Items]() Items ON Applications.ApplicationId = Items.ApplicationId INNER JOIN
	                      dbo.[netsqlazman_Stores]() Stores ON Applications.StoreId = Stores.StoreId
	WHERE     (Stores.StoreId = @STOREID) AND (Applications.ApplicationId = @ApplicationId) AND (Items.Name = @ITEMNAME) AND (@OPERATIONSONLY = 1 AND Items.ItemType=2 OR @OPERATIONSONLY = 0)
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
	INSERT INTO #ATTRIBUTES_TABLE SELECT AttributeKey, AttributeValue, NULL FROM dbo.[netsqlazman_StoreAttributesTable] StoreAttributes INNER JOIN dbo.[netsqlazman_StoresTable] Stores ON StoreAttributes.StoreId = Stores.StoreId WHERE Stores.StoreId = @STOREID
	INSERT INTO #ATTRIBUTES_TABLE SELECT AttributeKey, AttributeValue, NULL FROM dbo.[netsqlazman_ApplicationAttributesTable] ApplicationAttributes INNER JOIN dbo.[netsqlazman_ApplicationsTable] Applications ON ApplicationAttributes.ApplicationId = Applications.ApplicationId WHERE Applications.ApplicationId = @ApplicationId
END
--------------------------------------------------------------------------------
DECLARE @USERSID varbinary(85)
DECLARE @I INT
DECLARE @INDEX INT
DECLARE @APP VARBINARY(85)
DECLARE @SETTINGVALUE nvarchar(255)
DECLARE @NETSQLAZMANMODE bit

SELECT @SETTINGVALUE = SettingValue FROM dbo.[netsqlazman_Settings] WHERE SettingName = 'Mode'
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

EXEC dbo.[netsqlazman_CheckAccess] @ITEMID, @USERSID, @VALIDFOR, @LDAPPATH, @AUTHORIZATION_TYPE OUTPUT, @NETSQLAZMANMODE, @RETRIEVEATTRIBUTES
SELECT * FROM #PARTIAL_RESULTS_TABLE
IF @RETRIEVEATTRIBUTES = 1
	SELECT * FROM #ATTRIBUTES_TABLE
DROP TABLE #PARTIAL_RESULTS_TABLE
IF @RETRIEVEATTRIBUTES = 1
	DROP TABLE #ATTRIBUTES_TABLE
DROP TABLE #USERGROUPS
SET NOCOUNT OFF
GO
/****** Object:  Check [CK_Log]    Script Date: 06/11/2009 17:45:32 ******/
ALTER TABLE [dbo].[netsqlazman_LogTable]  WITH CHECK ADD  CONSTRAINT [CK_Log] CHECK  (([LogType]='I' OR [LogType]='W' OR [LogType]='E'))
GO
ALTER TABLE [dbo].[netsqlazman_LogTable] CHECK CONSTRAINT [CK_Log]
GO
/****** Object:  Check [CK_Settings]    Script Date: 06/11/2009 17:45:33 ******/
ALTER TABLE [dbo].[netsqlazman_Settings]  WITH CHECK ADD  CONSTRAINT [CK_Settings] CHECK  (([SettingName]='Mode' AND ([SettingValue]='Developer' OR [SettingValue]='Administrator') OR [SettingName]='LogErrors' AND ([SettingValue]='True' OR [SettingValue]='False') OR [SettingName]='LogWarnings' AND ([SettingValue]='True' OR [SettingValue]='False') OR [SettingName]='LogInformations' AND ([SettingValue]='True' OR [SettingValue]='False') OR [SettingName]='LogOnEventLog' AND ([SettingValue]='True' OR [SettingValue]='False') OR [SettingName]='LogOnDb' AND ([SettingValue]='True' OR [SettingValue]='False')))
GO
ALTER TABLE [dbo].[netsqlazman_Settings] CHECK CONSTRAINT [CK_Settings]
GO
/****** Object:  Check [CK_WhereDefinedNotValid]    Script Date: 06/11/2009 17:45:36 ******/
ALTER TABLE [dbo].[netsqlazman_ApplicationGroupMembersTable]  WITH CHECK ADD  CONSTRAINT [CK_WhereDefinedNotValid] CHECK  (([WhereDefined]>=(0) AND [WhereDefined]<=(4)))
GO
ALTER TABLE [dbo].[netsqlazman_ApplicationGroupMembersTable] CHECK CONSTRAINT [CK_WhereDefinedNotValid]
GO
/****** Object:  Check [CK_Items_ItemTypeCheck]    Script Date: 06/11/2009 17:45:36 ******/
ALTER TABLE [dbo].[netsqlazman_ItemsTable]  WITH CHECK ADD  CONSTRAINT [CK_Items_ItemTypeCheck] CHECK  (([ItemType]>=(0) AND [ItemType]<=(2)))
GO
ALTER TABLE [dbo].[netsqlazman_ItemsTable] CHECK CONSTRAINT [CK_Items_ItemTypeCheck]
GO
/****** Object:  Check [CK_ApplicationGroups_GroupType_Check]    Script Date: 06/11/2009 17:45:37 ******/
ALTER TABLE [dbo].[netsqlazman_ApplicationGroupsTable]  WITH CHECK ADD  CONSTRAINT [CK_ApplicationGroups_GroupType_Check] CHECK  (([GroupType]>=(0) AND [GroupType]<=(1)))
GO
ALTER TABLE [dbo].[netsqlazman_ApplicationGroupsTable] CHECK CONSTRAINT [CK_ApplicationGroups_GroupType_Check]
GO
/****** Object:  Check [CK_ApplicationPermissions]    Script Date: 06/11/2009 17:45:37 ******/
ALTER TABLE [dbo].[netsqlazman_ApplicationPermissionsTable]  WITH CHECK ADD  CONSTRAINT [CK_ApplicationPermissions] CHECK  (([NetSqlAzManFixedServerRole]>=(0) AND [NetSqlAzManFixedServerRole]<=(2)))
GO
ALTER TABLE [dbo].[netsqlazman_ApplicationPermissionsTable] CHECK CONSTRAINT [CK_ApplicationPermissions]
GO
/****** Object:  Check [CK_WhereDefinedCheck]    Script Date: 06/11/2009 17:45:38 ******/
ALTER TABLE [dbo].[netsqlazman_StoreGroupMembersTable]  WITH CHECK ADD  CONSTRAINT [CK_WhereDefinedCheck] CHECK  (([WhereDefined]=(0) OR [WhereDefined]>=(2) AND [WhereDefined]<=(4)))
GO
ALTER TABLE [dbo].[netsqlazman_StoreGroupMembersTable] CHECK CONSTRAINT [CK_WhereDefinedCheck]
GO
/****** Object:  Check [CK_StorePermissions]    Script Date: 06/11/2009 17:45:39 ******/
ALTER TABLE [dbo].[netsqlazman_StorePermissionsTable]  WITH CHECK ADD  CONSTRAINT [CK_StorePermissions] CHECK  (([NetSqlAzManFixedServerRole]>=(0) AND [NetSqlAzManFixedServerRole]<=(2)))
GO
ALTER TABLE [dbo].[netsqlazman_StorePermissionsTable] CHECK CONSTRAINT [CK_StorePermissions]
GO
/****** Object:  Check [CK_StoreGroups_GroupType_Check]    Script Date: 06/11/2009 17:45:39 ******/
ALTER TABLE [dbo].[netsqlazman_StoreGroupsTable]  WITH CHECK ADD  CONSTRAINT [CK_StoreGroups_GroupType_Check] CHECK  (([GroupType]>=(0) AND [GroupType]<=(1)))
GO
ALTER TABLE [dbo].[netsqlazman_StoreGroupsTable] CHECK CONSTRAINT [CK_StoreGroups_GroupType_Check]
GO
/****** Object:  Check [CK_AuthorizationTypeCheck]    Script Date: 06/11/2009 17:45:40 ******/
ALTER TABLE [dbo].[netsqlazman_AuthorizationsTable]  WITH CHECK ADD  CONSTRAINT [CK_AuthorizationTypeCheck] CHECK  (([AuthorizationType]>=(0) AND [AuthorizationType]<=(3)))
GO
ALTER TABLE [dbo].[netsqlazman_AuthorizationsTable] CHECK CONSTRAINT [CK_AuthorizationTypeCheck]
GO
/****** Object:  Check [CK_objectSidWhereDefinedCheck]    Script Date: 06/11/2009 17:45:40 ******/
ALTER TABLE [dbo].[netsqlazman_AuthorizationsTable]  WITH CHECK ADD  CONSTRAINT [CK_objectSidWhereDefinedCheck] CHECK  (([objectSidWhereDefined]>=(0) AND [objectSidWhereDefined]<=(4)))
GO
ALTER TABLE [dbo].[netsqlazman_AuthorizationsTable] CHECK CONSTRAINT [CK_objectSidWhereDefinedCheck]
GO
/****** Object:  Check [CK_ownerSidWhereDefined]    Script Date: 06/11/2009 17:45:40 ******/
ALTER TABLE [dbo].[netsqlazman_AuthorizationsTable]  WITH CHECK ADD  CONSTRAINT [CK_ownerSidWhereDefined] CHECK  (([ownerSidWhereDefined]>=(2) AND [ownerSidWhereDefined]<=(4)))
GO
ALTER TABLE [dbo].[netsqlazman_AuthorizationsTable] CHECK CONSTRAINT [CK_ownerSidWhereDefined]
GO
/****** Object:  Check [CK_ValidFromToCheck]    Script Date: 06/11/2009 17:45:40 ******/
ALTER TABLE [dbo].[netsqlazman_AuthorizationsTable]  WITH CHECK ADD  CONSTRAINT [CK_ValidFromToCheck] CHECK  (([ValidFrom] IS NULL OR [ValidTo] IS NULL OR [ValidFrom]<=[ValidTo]))
GO
ALTER TABLE [dbo].[netsqlazman_AuthorizationsTable] CHECK CONSTRAINT [CK_ValidFromToCheck]
GO
/****** Object:  ForeignKey [FK_AuthorizationAttributes_Authorizations]    Script Date: 06/11/2009 17:45:35 ******/
ALTER TABLE [dbo].[netsqlazman_AuthorizationAttributesTable]  WITH CHECK ADD  CONSTRAINT [FK_AuthorizationAttributes_Authorizations] FOREIGN KEY([AuthorizationId])
REFERENCES [dbo].[netsqlazman_AuthorizationsTable] ([AuthorizationId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[netsqlazman_AuthorizationAttributesTable] CHECK CONSTRAINT [FK_AuthorizationAttributes_Authorizations]
GO
/****** Object:  ForeignKey [FK_ApplicationGroupMembers_ApplicationGroup]    Script Date: 06/11/2009 17:45:36 ******/
ALTER TABLE [dbo].[netsqlazman_ApplicationGroupMembersTable]  WITH CHECK ADD  CONSTRAINT [FK_ApplicationGroupMembers_ApplicationGroup] FOREIGN KEY([ApplicationGroupId])
REFERENCES [dbo].[netsqlazman_ApplicationGroupsTable] ([ApplicationGroupId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[netsqlazman_ApplicationGroupMembersTable] CHECK CONSTRAINT [FK_ApplicationGroupMembers_ApplicationGroup]
GO
/****** Object:  ForeignKey [FK_Items_Applications]    Script Date: 06/11/2009 17:45:36 ******/
ALTER TABLE [dbo].[netsqlazman_ItemsTable]  WITH CHECK ADD  CONSTRAINT [FK_Items_Applications] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[netsqlazman_ApplicationsTable] ([ApplicationId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[netsqlazman_ItemsTable] CHECK CONSTRAINT [FK_Items_Applications]
GO
/****** Object:  ForeignKey [FK_Items_BizRules]    Script Date: 06/11/2009 17:45:36 ******/
ALTER TABLE [dbo].[netsqlazman_ItemsTable]  WITH CHECK ADD  CONSTRAINT [FK_Items_BizRules] FOREIGN KEY([BizRuleId])
REFERENCES [dbo].[netsqlazman_BizRulesTable] ([BizRuleId])
GO
ALTER TABLE [dbo].[netsqlazman_ItemsTable] CHECK CONSTRAINT [FK_Items_BizRules]
GO
/****** Object:  ForeignKey [FK_ApplicationGroups_Applications]    Script Date: 06/11/2009 17:45:37 ******/
ALTER TABLE [dbo].[netsqlazman_ApplicationGroupsTable]  WITH CHECK ADD  CONSTRAINT [FK_ApplicationGroups_Applications] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[netsqlazman_ApplicationsTable] ([ApplicationId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[netsqlazman_ApplicationGroupsTable] CHECK CONSTRAINT [FK_ApplicationGroups_Applications]
GO
/****** Object:  ForeignKey [FK_ApplicationAttributes_Applications]    Script Date: 06/11/2009 17:45:37 ******/
ALTER TABLE [dbo].[netsqlazman_ApplicationAttributesTable]  WITH CHECK ADD  CONSTRAINT [FK_ApplicationAttributes_Applications] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[netsqlazman_ApplicationsTable] ([ApplicationId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[netsqlazman_ApplicationAttributesTable] CHECK CONSTRAINT [FK_ApplicationAttributes_Applications]
GO
/****** Object:  ForeignKey [FK_ApplicationPermissions_ApplicationsTable]    Script Date: 06/11/2009 17:45:37 ******/
ALTER TABLE [dbo].[netsqlazman_ApplicationPermissionsTable]  WITH CHECK ADD  CONSTRAINT [FK_ApplicationPermissions_ApplicationsTable] FOREIGN KEY([ApplicationId])
REFERENCES [dbo].[netsqlazman_ApplicationsTable] ([ApplicationId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[netsqlazman_ApplicationPermissionsTable] CHECK CONSTRAINT [FK_ApplicationPermissions_ApplicationsTable]
GO
/****** Object:  ForeignKey [FK_StoreGroupMembers_StoreGroup]    Script Date: 06/11/2009 17:45:38 ******/
ALTER TABLE [dbo].[netsqlazman_StoreGroupMembersTable]  WITH CHECK ADD  CONSTRAINT [FK_StoreGroupMembers_StoreGroup] FOREIGN KEY([StoreGroupId])
REFERENCES [dbo].[netsqlazman_StoreGroupsTable] ([StoreGroupId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[netsqlazman_StoreGroupMembersTable] CHECK CONSTRAINT [FK_StoreGroupMembers_StoreGroup]
GO
/****** Object:  ForeignKey [FK_Applications_Stores]    Script Date: 06/11/2009 17:45:38 ******/
ALTER TABLE [dbo].[netsqlazman_ApplicationsTable]  WITH CHECK ADD  CONSTRAINT [FK_Applications_Stores] FOREIGN KEY([StoreId])
REFERENCES [dbo].[netsqlazman_StoresTable] ([StoreId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[netsqlazman_ApplicationsTable] CHECK CONSTRAINT [FK_Applications_Stores]
GO
/****** Object:  ForeignKey [FK_StoreAttributes_Stores]    Script Date: 06/11/2009 17:45:38 ******/
ALTER TABLE [dbo].[netsqlazman_StoreAttributesTable]  WITH CHECK ADD  CONSTRAINT [FK_StoreAttributes_Stores] FOREIGN KEY([StoreId])
REFERENCES [dbo].[netsqlazman_StoresTable] ([StoreId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[netsqlazman_StoreAttributesTable] CHECK CONSTRAINT [FK_StoreAttributes_Stores]
GO
/****** Object:  ForeignKey [FK_StorePermissions_StoresTable]    Script Date: 06/11/2009 17:45:39 ******/
ALTER TABLE [dbo].[netsqlazman_StorePermissionsTable]  WITH CHECK ADD  CONSTRAINT [FK_StorePermissions_StoresTable] FOREIGN KEY([StoreId])
REFERENCES [dbo].[netsqlazman_StoresTable] ([StoreId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[netsqlazman_StorePermissionsTable] CHECK CONSTRAINT [FK_StorePermissions_StoresTable]
GO
/****** Object:  ForeignKey [FK_StoreGroups_Stores]    Script Date: 06/11/2009 17:45:39 ******/
ALTER TABLE [dbo].[netsqlazman_StoreGroupsTable]  WITH CHECK ADD  CONSTRAINT [FK_StoreGroups_Stores] FOREIGN KEY([StoreId])
REFERENCES [dbo].[netsqlazman_StoresTable] ([StoreId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[netsqlazman_StoreGroupsTable] CHECK CONSTRAINT [FK_StoreGroups_Stores]
GO
/****** Object:  ForeignKey [FK_Authorizations_Items]    Script Date: 06/11/2009 17:45:40 ******/
ALTER TABLE [dbo].[netsqlazman_AuthorizationsTable]  WITH CHECK ADD  CONSTRAINT [FK_Authorizations_Items] FOREIGN KEY([ItemId])
REFERENCES [dbo].[netsqlazman_ItemsTable] ([ItemId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[netsqlazman_AuthorizationsTable] CHECK CONSTRAINT [FK_Authorizations_Items]
GO
/****** Object:  ForeignKey [FK_ItemAttributes_Items]    Script Date: 06/11/2009 17:45:40 ******/
ALTER TABLE [dbo].[netsqlazman_ItemAttributesTable]  WITH CHECK ADD  CONSTRAINT [FK_ItemAttributes_Items] FOREIGN KEY([ItemId])
REFERENCES [dbo].[netsqlazman_ItemsTable] ([ItemId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[netsqlazman_ItemAttributesTable] CHECK CONSTRAINT [FK_ItemAttributes_Items]
GO
EXEC dbo.sp_addrolemember @rolename=N'NetSqlAzMan_Readers', @membername=N'BUILTIN\Administrators'
GO
EXEC dbo.sp_addrolemember @rolename=N'NetSqlAzMan_Readers', @membername=N'NetSqlAzMan_Administrators'
GO
EXEC dbo.sp_addrolemember @rolename=N'NetSqlAzMan_Readers', @membername=N'NetSqlAzMan_Managers'
GO
EXEC dbo.sp_addrolemember @rolename=N'NetSqlAzMan_Readers', @membername=N'NetSqlAzMan_Users'
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
GRANT EXECUTE ON [dbo].[netsqlazman_DBVersion] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[UsersDemo] TO [NetSqlAzMan_Readers]
GO
GRANT INSERT ON [dbo].[netsqlazman_LogTable] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[netsqlazman_LogTable] TO [NetSqlAzMan_Readers]
GO
GRANT DELETE ON [dbo].[netsqlazman_Settings] TO [NetSqlAzMan_Administrators]
GO
GRANT INSERT ON [dbo].[netsqlazman_Settings] TO [NetSqlAzMan_Administrators]
GO
GRANT UPDATE ON [dbo].[netsqlazman_Settings] TO [NetSqlAzMan_Administrators]
GO
GRANT SELECT ON [dbo].[netsqlazman_Settings] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_IAmAdmin] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_helplogins] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[netsqlazman_ApplicationPermissionsTable] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[netsqlazman_StorePermissionsTable] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_CheckApplicationPermissions] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_CheckStorePermissions] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_BizRuleInsert] TO [NetSqlAzMan_Managers]
GO
GRANT SELECT ON [dbo].[netsqlazman_GetDBUsers] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_StoreInsert] TO [NetSqlAzMan_Administrators]
GO
GRANT SELECT ON [dbo].[netsqlazman_Applications] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_GrantStoreAccess] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_RevokeStoreAccess] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_BizRuleDelete] TO [NetSqlAzMan_Managers]
GO
GRANT SELECT ON [dbo].[netsqlazman_DatabaseUsers] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_StoreGroupInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_StoreGroupUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_StoreGroupDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_StoreDelete] TO [NetSqlAzMan_Managers]
GO
GRANT SELECT ON [dbo].[netsqlazman_Stores] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[netsqlazman_ApplicationGroups] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_ApplicationGroupInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_ApplicationPermissionDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_RevokeApplicationAccess] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_ApplicationPermissionInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_GrantApplicationAccess] TO [NetSqlAzMan_Managers]
GO
GRANT SELECT ON [dbo].[netsqlazman_ApplicationPermissions] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_ApplicationDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_ApplicationUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_ApplicationInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_StoreAttributeInsert] TO [NetSqlAzMan_Managers]
GO
GRANT SELECT ON [dbo].[netsqlazman_StoreAttributes] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_StorePermissionDelete] TO [NetSqlAzMan_Managers]
GO
GRANT SELECT ON [dbo].[netsqlazman_StorePermissions] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_StorePermissionInsert] TO [NetSqlAzMan_Managers]
GO
GRANT SELECT ON [dbo].[netsqlazman_ApplicationAttributes] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_ApplicationAttributeInsert] TO [NetSqlAzMan_Managers]
GO
GRANT SELECT ON [dbo].[netsqlazman_StoreGroups] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_ItemInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_StoreUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT SELECT ON [dbo].[netsqlazman_ApplicationsView] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[netsqlazman_Items] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_CreateDelegate] TO [NetSqlAzMan_Users]
GO
GRANT SELECT ON [dbo].[netsqlazman_Authorizations] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_AuthorizationInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_ApplicationGroupDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_ApplicationGroupUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_ItemAttributeInsert] TO [NetSqlAzMan_Managers]
GO
GRANT SELECT ON [dbo].[netsqlazman_ItemAttributes] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[netsqlazman_StoreGroupMembers] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_StoreGroupMemberInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_StoreAttributeUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_StoreAttributeDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_ApplicationAttributeDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_ApplicationAttributeUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT SELECT ON [dbo].[netsqlazman_BizRules] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_ItemsHierarchyDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_ItemsHierarchyInsert] TO [NetSqlAzMan_Managers]
GO
GRANT SELECT ON [dbo].[netsqlazman_ItemsHierarchy] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_ClearBizRule] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_ItemUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_ApplicationGroupMemberInsert] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_ReloadBizRule] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_ItemDelete] TO [NetSqlAzMan_Managers]
GO
GRANT SELECT ON [dbo].[netsqlazman_StoreAttributesView] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[netsqlazman_ApplicationAttributesView] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[netsqlazman_ApplicationGroupMembers] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_AuthorizationUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_DeleteDelegate] TO [NetSqlAzMan_Users]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_AuthorizationDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_ItemAttributeUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_ItemAttributeDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_StoreGroupMemberUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_StoreGroupMemberDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_BizRuleUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT SELECT ON [dbo].[netsqlazman_AuthorizationAttributes] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_AuthorizationAttributeInsert] TO [NetSqlAzMan_Users]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_ApplicationGroupMemberDelete] TO [NetSqlAzMan_Managers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_ApplicationGroupMemberUpdate] TO [NetSqlAzMan_Managers]
GO
GRANT SELECT ON [dbo].[netsqlazman_StoreGroupMembersView] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_BuildUserPermissionCache] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[netsqlazman_AuthorizationView] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[netsqlazman_ApplicationGroupMembersView] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[netsqlazman_ItemsHierarchyView] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[netsqlazman_ItemAttributesView] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[netsqlazman_BizRuleView] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_AuthorizationAttributeDelete] TO [NetSqlAzMan_Users]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_AuthorizationAttributeUpdate] TO [NetSqlAzMan_Users]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_IsAMemberOfGroup] TO [NetSqlAzMan_Readers]
GO
GRANT SELECT ON [dbo].[netsqlazman_AuthorizationAttributesView] TO [NetSqlAzMan_Readers]
GO
GRANT EXECUTE ON [dbo].[netsqlazman_DirectCheckAccess] TO [NetSqlAzMan_Readers]
GO


/* ********************** */
/* ONLY FOR SQL 2005/2008 */
/* ********************** */
IF CHARINDEX('Microsoft SQL Server 2005', REPLACE(@@VERSION,'  ', ' '))=1 
   OR 
   CHARINDEX('Microsoft SQL Server 2008', REPLACE(@@VERSION,'  ', ' '))=1
   BEGIN
        EXEC sp_executesql N'GRANT VIEW DEFINITION TO [NetSqlAzMan_Readers]' -- ALLOW NetSqlAzMan_Readers TO SEE ALL OTHER Logins 
-- http://www.microsoft.com/technet/technetmag/issues/2006/01/ProtectMetaData/?topics=y
END
/* ********************** */

