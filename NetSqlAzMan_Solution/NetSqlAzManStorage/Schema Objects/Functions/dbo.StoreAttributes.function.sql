CREATE FUNCTION [dbo].[StoreAttributes] ()
RETURNS TABLE
AS
RETURN
	SELECT     dbo.[netsqlazman_StoreAttributesTable].*
	FROM         dbo.[netsqlazman_StoreAttributesTable] INNER JOIN
	                      dbo.Stores() Stores ON dbo.[netsqlazman_StoreAttributesTable].StoreId = Stores.StoreId


