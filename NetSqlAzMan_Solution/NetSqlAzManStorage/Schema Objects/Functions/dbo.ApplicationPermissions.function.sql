CREATE FUNCTION [dbo].[netsqlazman_ApplicationPermissions]()
RETURNS TABLE 
AS  
RETURN
	SELECT     dbo.[netsqlazman_ApplicationPermissionsTable].*
	FROM         dbo.[netsqlazman_ApplicationPermissionsTable] INNER JOIN
	                      dbo.[netsqlazman_Applications]() Applications ON dbo.[netsqlazman_ApplicationPermissionsTable].ApplicationId = Applications.ApplicationId


