CREATE PROCEDURE [dbo].[netsqlazman_DeleteDelegate](@AUTHORIZATIONID INT, @OWNERSID VARBINARY(85))
AS
DECLARE @ApplicationId int
SELECT @ApplicationId = Items.ApplicationId FROM dbo.[netsqlazman_Items]() Items INNER JOIN dbo.[netsqlazman_Authorizations]() Authorizations ON Items.ItemId = Authorizations.ItemId WHERE Authorizations.AuthorizationId = @AUTHORIZATIONID
IF @ApplicationId IS NOT NULL AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 1) = 1
	DELETE FROM dbo.[netsqlazman_AuthorizationsTable] WHERE AuthorizationId = @AUTHORIZATIONID AND ownerSid = @OWNERSID
ELSE
	RAISERROR ('Item NOT Found or Application permission denied.', 16, 1)


