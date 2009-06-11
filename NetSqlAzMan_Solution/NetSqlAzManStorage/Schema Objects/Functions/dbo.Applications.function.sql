CREATE FUNCTION [dbo].[netsqlazman_Applications] ()
RETURNS TABLE
AS
RETURN
	SELECT * FROM dbo.[netsqlazman_ApplicationsTable]
	WHERE dbo.[netsqlazman_CheckApplicationPermissions](ApplicationId, 0) = 1


