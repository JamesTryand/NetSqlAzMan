CREATE FUNCTION [dbo].[AuthorizationAttributes] ()
RETURNS TABLE
AS
RETURN
	SELECT     dbo.[netsqlazman_AuthorizationAttributesTable].*
	FROM         dbo.[netsqlazman_AuthorizationAttributesTable] INNER JOIN
	                      dbo.Authorizations() Authorizations ON dbo.[netsqlazman_AuthorizationAttributesTable].AuthorizationId = Authorizations.AuthorizationId


