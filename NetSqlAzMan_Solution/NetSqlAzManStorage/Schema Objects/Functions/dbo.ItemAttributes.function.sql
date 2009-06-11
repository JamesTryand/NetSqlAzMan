CREATE FUNCTION [dbo].[netsqlazman_ItemAttributes] ()
RETURNS TABLE
AS
RETURN
	SELECT     dbo.[netsqlazman_ItemAttributesTable].*
	FROM         dbo.[netsqlazman_ItemAttributesTable] INNER JOIN
	                      dbo.[netsqlazman_Items]() Items ON dbo.[netsqlazman_ItemAttributesTable].ItemId = Items.ItemId


