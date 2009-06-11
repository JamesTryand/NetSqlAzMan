CREATE FUNCTION [dbo].[StoreGroups] ()
RETURNS TABLE
AS
RETURN
	SELECT     dbo.[netsqlazman_StoreGroupsTable].*
	FROM         dbo.Stores() Stores INNER JOIN
	                      dbo.[netsqlazman_StoreGroupsTable] ON Stores.StoreId = dbo.[netsqlazman_StoreGroupsTable].StoreId


