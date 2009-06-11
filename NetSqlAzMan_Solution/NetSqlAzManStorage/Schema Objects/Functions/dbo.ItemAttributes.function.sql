CREATE FUNCTION [dbo].[ItemAttributes] ()
RETURNS TABLE
AS
RETURN
	SELECT     dbo.[netsqlazman_ItemAttributesTable].*
	FROM         dbo.[netsqlazman_ItemAttributesTable] INNER JOIN
	                      dbo.Items() Items ON dbo.[netsqlazman_ItemAttributesTable].ItemId = Items.ItemId


