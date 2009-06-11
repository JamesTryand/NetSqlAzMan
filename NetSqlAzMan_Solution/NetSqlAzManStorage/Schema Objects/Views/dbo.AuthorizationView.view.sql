CREATE VIEW [dbo].[netsqlazman_AuthorizationView]
AS
SELECT     Authorizations.AuthorizationId, Authorizations.ItemId, dbo.GetNameFromSid(Stores.Name, Applications.Name, Authorizations.ownerSid, 
                      Authorizations.ownerSidWhereDefined) AS Owner, dbo.GetNameFromSid(Stores.Name, Applications.Name, Authorizations.objectSid, 
                      Authorizations.objectSidWhereDefined) AS Name, Authorizations.objectSid, 
                      CASE objectSidWhereDefined WHEN 0 THEN 'Store' WHEN 1 THEN 'Application' WHEN 2 THEN 'LDAP' WHEN 3 THEN 'Local' WHEN 4 THEN 'DATABASE' END AS SidWhereDefined,
                       CASE AuthorizationType WHEN 0 THEN 'NEUTRAL' WHEN 1 THEN 'ALLOW' WHEN 2 THEN 'DENY' WHEN 3 THEN 'ALLOWWITHDELEGATION' END AS AuthorizationType,
                       Authorizations.ValidFrom, Authorizations.ValidTo
FROM         dbo.Authorizations() Authorizations INNER JOIN
                      dbo.Items() Items ON Authorizations.ItemId = Items.ItemId INNER JOIN
                      dbo.Applications() Applications ON Items.ApplicationId = Applications.ApplicationId INNER JOIN
                      dbo.Stores() Stores ON Applications.StoreId = Stores.StoreId


