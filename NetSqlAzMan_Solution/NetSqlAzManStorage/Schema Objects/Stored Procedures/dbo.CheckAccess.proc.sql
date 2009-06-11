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
	FROM dbo.[netsqlazman_AuthorizationsTable] Authorizations INNER JOIN #USERGROUPS usergroups
	ON Authorizations.objectSid = #USERGROUPS.objectSid WHERE 
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
	     EXISTS(SELECT * FROM @GROUPSIDMEMBERS groupsidmembers INNER JOIN #USERGROUPS usergroups ON groupsidmembers.objectSid = #USERGROUPS.objectSid)
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
		     EXISTS (SELECT * FROM @GROUPSIDMEMBERS groupsidmembers INNER JOIN #USERGROUPS usergroups ON groupsidmembers.objectSid = #USERGROUPS.ObjectSId)
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
	     EXISTS (SELECT* FROM @GROUPSIDMEMBERS groupsidmembers INNER JOIN #USERGROUPS usergroups ON groupsidmembers.objectSid = #USERGROUPS.objectSid)
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
		     EXISTS (SELECT * FROM @GROUPSIDMEMBERS groupsidmembers INNER JOIN #USERGROUPS usergroups ON groupsidmembers.objectSid = #USERGROUPS.objectSid)
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


