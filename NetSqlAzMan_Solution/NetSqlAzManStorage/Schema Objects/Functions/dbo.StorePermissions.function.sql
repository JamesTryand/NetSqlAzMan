CREATE FUNCTION [dbo].[StorePermissions]()
RETURNS TABLE 
AS  
RETURN
	SELECT     dbo.[netsqlazman_StorePermissionsTable].*
	FROM         dbo.[netsqlazman_StorePermissionsTable] INNER JOIN
	                      dbo.Stores() Stores ON dbo.[netsqlazman_StorePermissionsTable].StoreId = Stores.StoreId


