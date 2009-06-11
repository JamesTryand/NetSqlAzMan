CREATE FUNCTION [dbo].[Items] ()
RETURNS TABLE
AS
RETURN
	SELECT     dbo.[netsqlazman_ItemsTable].*
	FROM         dbo.[netsqlazman_ItemsTable] INNER JOIN
	                      dbo.Applications() Applications ON dbo.[netsqlazman_ItemsTable].ApplicationId = Applications.ApplicationId


