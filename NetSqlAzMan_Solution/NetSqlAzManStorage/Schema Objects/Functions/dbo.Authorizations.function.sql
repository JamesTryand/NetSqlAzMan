CREATE FUNCTION [dbo].[netsqlazman_Authorizations]()
RETURNS TABLE
AS
RETURN
	SELECT     dbo.[netsqlazman_AuthorizationsTable].*
	FROM         dbo.[netsqlazman_AuthorizationsTable] INNER JOIN
	                      dbo.[netsqlazman_Items]() Items ON dbo.[netsqlazman_AuthorizationsTable].ItemId = Items.ItemId


