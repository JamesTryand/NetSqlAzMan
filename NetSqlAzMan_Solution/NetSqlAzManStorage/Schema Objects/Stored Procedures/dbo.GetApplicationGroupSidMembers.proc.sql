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


