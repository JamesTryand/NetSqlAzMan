CREATE FUNCTION [dbo].[Applications] ()
RETURNS TABLE
AS
RETURN
	SELECT * FROM dbo.[netsqlazman_ApplicationsTable]
	WHERE dbo.CheckApplicationPermissions(ApplicationId, 0) = 1


