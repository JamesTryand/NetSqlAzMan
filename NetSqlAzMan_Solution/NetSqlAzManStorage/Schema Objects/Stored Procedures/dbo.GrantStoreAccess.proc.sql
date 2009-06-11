CREATE PROCEDURE [dbo].[netsqlazman_GrantStoreAccess] (
	@StoreId int,
	@SqlUserOrRole sysname,
	@NetSqlAzManFixedServerRole tinyint)
AS
IF EXISTS(SELECT StoreId FROM dbo.[netsqlazman_StoresTable] WHERE StoreId = @StoreId) AND (dbo.CheckStorePermissions(@StoreId, 2) = 1 AND @NetSqlAzManFixedServerRole BETWEEN 0 AND 1 OR (IS_MEMBER('db_owner')=1 OR IS_MEMBER('NetSqlAzMan_Administrators')=1) AND @NetSqlAzManFixedServerRole = 2)
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


