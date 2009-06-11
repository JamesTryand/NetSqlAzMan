CREATE FUNCTION [dbo].[netsqlazman_ApplicationAttributes] ()
RETURNS TABLE
AS
RETURN 
	SELECT     dbo.[netsqlazman_ApplicationAttributesTable].*
	FROM         dbo.[netsqlazman_ApplicationAttributesTable] INNER JOIN
	                      dbo.Applications() Applications ON dbo.[netsqlazman_ApplicationAttributesTable].ApplicationId = Applications.ApplicationId


