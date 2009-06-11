CREATE FUNCTION [dbo].[ApplicationPermissions]()
RETURNS TABLE 
AS  
RETURN
	SELECT     dbo.[netsqlazman_ApplicationPermissionsTable].*
	FROM         dbo.[netsqlazman_ApplicationPermissionsTable] INNER JOIN
	                      dbo.Applications() Applications ON dbo.[netsqlazman_ApplicationPermissionsTable].ApplicationId = Applications.ApplicationId


