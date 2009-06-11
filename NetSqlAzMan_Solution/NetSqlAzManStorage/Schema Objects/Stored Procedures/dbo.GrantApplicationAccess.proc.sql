CREATE PROCEDURE [dbo].[netsqlazman_GrantApplicationAccess] (
	@ApplicationId int,
	@SqlUserOrRole sysname,
	@NetSqlAzManFixedServerRole tinyint)
AS
DECLARE @StoreId int
SET @StoreId = (SELECT StoreId FROM dbo.Applications() WHERE ApplicationId = @ApplicationId)
IF EXISTS(SELECT ApplicationId FROM dbo.[netsqlazman_ApplicationsTable] WHERE ApplicationId = @ApplicationId) AND (dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1 AND @NetSqlAzManFixedServerRole BETWEEN 0 AND 1 OR dbo.CheckStorePermissions(@StoreId, 2) = 1 AND @NetSqlAzManFixedServerRole = 2)
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


