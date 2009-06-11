CREATE VIEW [dbo].[netsqlazman_AuthorizationAttributesView]
AS
SELECT     dbo.[netsqlazman_AuthorizationView].AuthorizationId, dbo.[netsqlazman_AuthorizationView].ItemId, dbo.[netsqlazman_AuthorizationView].Owner, dbo.[netsqlazman_AuthorizationView].Name, dbo.[netsqlazman_AuthorizationView].objectSid, 
                      dbo.[netsqlazman_AuthorizationView].SidWhereDefined, dbo.[netsqlazman_AuthorizationView].AuthorizationType, dbo.[netsqlazman_AuthorizationView].ValidFrom, dbo.[netsqlazman_AuthorizationView].ValidTo, 
                      AuthorizationAttributes.AuthorizationAttributeId, AuthorizationAttributes.AttributeKey, AuthorizationAttributes.AttributeValue
FROM         dbo.[netsqlazman_AuthorizationView] INNER JOIN
                      dbo.AuthorizationAttributes() AuthorizationAttributes ON dbo.[netsqlazman_AuthorizationView].AuthorizationId = AuthorizationAttributes.AuthorizationId


