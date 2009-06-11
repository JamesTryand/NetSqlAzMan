CREATE FUNCTION [dbo].[netsqlazman_StorePermissions]()
RETURNS TABLE 
AS  
RETURN
	SELECT     dbo.[netsqlazman_StorePermissionsTable].*
	FROM         dbo.[netsqlazman_StorePermissionsTable] INNER JOIN
	                      dbo.[netsqlazman_Stores]() Stores ON dbo.[netsqlazman_StorePermissionsTable].StoreId = Stores.StoreId


