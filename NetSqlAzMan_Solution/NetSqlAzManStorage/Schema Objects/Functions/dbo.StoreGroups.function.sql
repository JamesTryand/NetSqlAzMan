CREATE FUNCTION [dbo].[netsqlazman_StoreGroups] ()
RETURNS TABLE
AS
RETURN
	SELECT     dbo.[netsqlazman_StoreGroupsTable].*
	FROM         dbo.[netsqlazman_Stores]() Stores INNER JOIN
	                      dbo.[netsqlazman_StoreGroupsTable] ON Stores.StoreId = dbo.[netsqlazman_StoreGroupsTable].StoreId


