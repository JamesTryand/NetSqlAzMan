CREATE FUNCTION [dbo].[netsqlazman_ItemsHierarchy] ()
RETURNS TABLE
AS
RETURN
	SELECT     dbo.[netsqlazman_ItemsHierarchyTable].*
	FROM         dbo.[netsqlazman_ItemsHierarchyTable] INNER JOIN
	                      dbo.[netsqlazman_Items]() Items ON dbo.[netsqlazman_ItemsHierarchyTable].ItemId = Items.ItemId


