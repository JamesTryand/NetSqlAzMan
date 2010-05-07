/****** Object:  Stored Procedure dbo.CreateDelegate    Script Date: 19/05/2006 19.11.19 ******/
CREATE PROCEDURE [dbo].[netsqlazman_CreateDelegate](@ITEMID INT, @OWNERSID VARBINARY(85), @OWNERSIDWHEREDEFINED TINYINT, @DELEGATEDUSERSID VARBINARY(85), @SIDWHEREDEFINED TINYINT, @AUTHORIZATIONTYPE TINYINT, @VALIDFROM DATETIME, @VALIDTO DATETIME, @AUTHORIZATIONID INT OUTPUT)
AS
DECLARE @ApplicationId int
SELECT @ApplicationId = ApplicationId FROM dbo.[netsqlazman_Items]() WHERE ItemId = @ITEMID
IF @ApplicationId IS NOT NULL AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 1) = 1
BEGIN
	INSERT INTO dbo.[netsqlazman_AuthorizationsTable] (ItemId, ownerSid, ownerSidWhereDefined, objectSid, objectSidWhereDefined, AuthorizationType, ValidFrom, ValidTo)
		VALUES (@ITEMID, @OWNERSID, @OWNERSIDWHEREDEFINED, @DELEGATEDUSERSID, @SIDWHEREDEFINED, @AUTHORIZATIONTYPE, @VALIDFROM, @VALIDTO)
	SET @AUTHORIZATIONID = SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Item NOT Found or Application permission denied.', 16, 1)


