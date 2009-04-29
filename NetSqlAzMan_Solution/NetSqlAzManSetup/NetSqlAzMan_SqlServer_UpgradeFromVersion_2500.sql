--
-- Script To Delete dbo.ItemsHierarchyTriggerr Trigger In ..NetSqlAzMan_2500
-- Generated sabato, aprile 12, 2008, at 02.35 PM
--
-- Please backup ..NetSqlAzMan_2500 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Deleting dbo.ItemsHierarchyTriggerr Trigger'
GO

   DROP TRIGGER [dbo].[ItemsHierarchyTriggerr]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.ItemsHierarchyTriggerr Trigger Deleted Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Delete dbo.ItemsHierarchyTriggerr Trigger'
END
GO

--
-- Script To Delete dbo.Log Table In ..NetSqlAzMan_2500
-- Generated sabato, aprile 12, 2008, at 02.35 PM
--
-- Please backup ..NetSqlAzMan_2500 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Deleting dbo.Log Table'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO


IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   DROP TABLE [dbo].[Log]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.Log Table Deleted Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Delete dbo.Log Table'
END
GO

--
-- Script To Create dbo.LogTable Table In ..NetSqlAzMan_2500
-- Generated sabato, aprile 12, 2008, at 02.35 PM
--
-- Please backup ..NetSqlAzMan_2500 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Creating dbo.LogTable Table'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO

CREATE TABLE [dbo].[LogTable] (
   [LogId] [int] IDENTITY (1, 1) NOT NULL,
   [LogDateTime] [datetime] NOT NULL,
   [WindowsIdentity] [nvarchar] (255) NOT NULL,
   [SqlIdentity] [nvarchar] (128) NULL CONSTRAINT [DF_Log_SqlIdentity] DEFAULT (suser_sname()),
   [MachineName] [nvarchar] (255) NOT NULL,
   [InstanceGuid] [uniqueidentifier] NOT NULL,
   [TransactionGuid] [uniqueidentifier] NULL,
   [OperationCounter] [int] NOT NULL,
   [ENSType] [nvarchar] (255) NOT NULL,
   [ENSDescription] [nvarchar] (4000) NOT NULL,
   [LogType] [char] (1) NOT NULL
)
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[LogTable] ADD CONSTRAINT [PK_Log] PRIMARY KEY NONCLUSTERED ([LogId])
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   CREATE INDEX [IX_Log] ON [dbo].[LogTable] ([WindowsIdentity])
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   CREATE INDEX [IX_Log_1] ON [dbo].[LogTable] ([SqlIdentity])
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   CREATE CLUSTERED INDEX [IX_Log_2] ON [dbo].[LogTable] ([LogDateTime] DESC, [InstanceGuid], [OperationCounter] DESC)
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[LogTable] ADD CONSTRAINT [CK_Log] CHECK ([LogType] = 'I' or [LogType] = 'W' or [LogType] = 'E')
GO
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   GRANT Insert ON [LogTable] TO [NetSqlAzMan_Readers]
GO
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   GRANT Select ON [LogTable] TO [NetSqlAzMan_Readers]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.LogTable Table Added Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Add dbo.LogTable Table'
END
GO

--
-- Script To Update dbo.NetSqlAzMan_DBVersion Function In ..NetSqlAzMan_2500
-- Generated sabato, aprile 12, 2008, at 02.35 PM
--
-- Please backup ..NetSqlAzMan_2500 before executing this script
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
	return ''3.5.4.1''
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
-- Script To Update dbo.GetStoreGroupSidMembers Procedure In ..NetSqlAzMan_2500
-- Generated sabato, aprile 12, 2008, at 02.35 PM
--
-- Please backup ..NetSqlAzMan_2500 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.GetStoreGroupSidMembers Procedure'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO


exec('ALTER PROCEDURE [dbo].[GetStoreGroupSidMembers](@ISMEMBER BIT, @GROUPOBJECTSID VARBINARY(85), @NETSQLAZMANMODE bit, @LDAPPATH nvarchar(4000), @member_cur CURSOR VARYING OUTPUT)
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
OPEN @member_cur')
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.GetStoreGroupSidMembers Procedure Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.GetStoreGroupSidMembers Procedure'
END
GO

--
-- Script To Update dbo.GetApplicationGroupSidMembers Procedure In ..NetSqlAzMan_2500
-- Generated sabato, aprile 12, 2008, at 02.35 PM
--
-- Please backup ..NetSqlAzMan_2500 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.GetApplicationGroupSidMembers Procedure'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO


exec('ALTER PROCEDURE [dbo].[GetApplicationGroupSidMembers](@ISMEMBER BIT, @GROUPOBJECTSID VARBINARY(85), @NETSQLAZMANMODE bit, @LDAPPATH nvarchar(4000), @member_cur CURSOR VARYING OUTPUT)
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
OPEN @member_cur')
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.GetApplicationGroupSidMembers Procedure Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.GetApplicationGroupSidMembers Procedure'
END
GO

--
-- Script To Update dbo.CheckAccess Procedure In ..NetSqlAzMan_2500
-- Generated sabato, aprile 12, 2008, at 02.35 PM
--
-- Please backup ..NetSqlAzMan_2500 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.CheckAccess Procedure'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO


exec('ALTER PROCEDURE [dbo].[CheckAccess] (@ITEMID INT, @USERSID VARBINARY(85), @VALIDFOR DATETIME, @LDAPPATH nvarchar(4000), @AUTHORIZATION_TYPE TINYINT OUTPUT, @NETSQLAZMANMODE BIT, @RETRIEVEATTRIBUTES BIT) 
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
	SELECT AuthorizationType, AuthorizationID
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
			INSERT INTO #ATTRIBUTES_TABLE SELECT AttributeKey, AttributeValue, NULL FROM dbo.AuthorizationAttributesTable WHERE AuthorizationID = @PKID
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
			INSERT INTO #ATTRIBUTES_TABLE SELECT AttributeKey, AttributeValue, NULL FROM dbo.AuthorizationAttributesTable WHERE AuthorizationID = @PKID
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
FOR 	SELECT objectSid, objectSidWhereDefined, AuthorizationType, AuthorizationID FROM dbo.AuthorizationsTable
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
--store groups members of type ''non-member''
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
			INSERT INTO #ATTRIBUTES_TABLE SELECT AttributeKey, AttributeValue, NULL FROM dbo.AuthorizationAttributesTable WHERE AuthorizationID = @PKID
	END
END
	ELSE
IF @GROUPWHEREDEFINED = 1 -- application group members
BEGIN
	--application groups members of type ''non-member''
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
			INSERT INTO #ATTRIBUTES_TABLE SELECT AttributeKey, AttributeValue, NULL FROM dbo.AuthorizationAttributesTable WHERE AuthorizationID = @PKID
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
SET NOCOUNT OFF')
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.CheckAccess Procedure Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.CheckAccess Procedure'
END
GO

--
-- Script To Create dbo.ItemsHierarchyTrigger Trigger In ..NetSqlAzMan_2500
-- Generated sabato, aprile 12, 2008, at 02.35 PM
--
-- Please backup ..NetSqlAzMan_2500 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Creating dbo.ItemsHierarchyTrigger Trigger'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO


SET QUOTED_IDENTIFIER OFF
GO

exec('CREATE TRIGGER [dbo].[ItemsHierarchyTrigger] ON [dbo].[ItemsHierarchyTable] 
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
	  RAISERROR (''ItemId NOT FOUND into dbo.ItemsTable'', 16, 1)
	  ROLLBACK TRANSACTION
	 END
	
	IF UPDATE(MemberOfItemId) AND NOT EXISTS (SELECT ItemId FROM dbo.ItemsTable WHERE ItemsTable.ItemId = @INSERTEDMEMBEROFITEMID)
	 BEGIN
	  RAISERROR (''MemberOfItemId NOT FOUND into dbo.ItemsTable'', 16, 1)
	  ROLLBACK TRANSACTION
	 END
	FETCH NEXT from itemhierarchy_cur INTO @INSERTEDITEMID, @INSERTEDMEMBEROFITEMID
END
CLOSE itemhierarchy_cur
DEALLOCATE itemhierarchy_cur')
GO

SET QUOTED_IDENTIFIER ON
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.ItemsHierarchyTrigger Trigger Added Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Add dbo.ItemsHierarchyTrigger Trigger'
END
GO