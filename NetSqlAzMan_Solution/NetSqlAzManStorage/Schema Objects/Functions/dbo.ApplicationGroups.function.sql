CREATE FUNCTION [dbo].[netsqlazman_ApplicationGroups] ()
RETURNS TABLE
AS
RETURN
	SELECT     dbo.[netsqlazman_ApplicationGroupsTable].*
	FROM         dbo.[netsqlazman_ApplicationGroupsTable] INNER JOIN
	                      dbo.[netsqlazman_Applications]() Applications ON dbo.[netsqlazman_ApplicationGroupsTable].ApplicationId = Applications.ApplicationId


