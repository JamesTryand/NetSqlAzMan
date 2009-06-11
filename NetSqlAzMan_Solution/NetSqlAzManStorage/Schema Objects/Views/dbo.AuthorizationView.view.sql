CREATE VIEW [dbo].[netsqlazman_AuthorizationView]
AS
SELECT     [netsqlazman_Authorizations].AuthorizationId, [netsqlazman_Authorizations].ItemId, dbo.[netsqlazman_GetNameFromSid]([netsqlazman_Stores].Name, [netsqlazman_Applications].Name, [netsqlazman_Authorizations].ownerSid, 
                      [netsqlazman_Authorizations].ownerSidWhereDefined) AS Owner, dbo.[netsqlazman_GetNameFromSid]([netsqlazman_Stores].Name, [netsqlazman_Applications].Name, [netsqlazman_Authorizations].objectSid, 
                      [netsqlazman_Authorizations].objectSidWhereDefined) AS Name, [netsqlazman_Authorizations].objectSid, 
                      CASE objectSidWhereDefined WHEN 0 THEN 'Store' WHEN 1 THEN 'Application' WHEN 2 THEN 'LDAP' WHEN 3 THEN 'Local' WHEN 4 THEN 'DATABASE' END AS SidWhereDefined,
                       CASE AuthorizationType WHEN 0 THEN 'NEUTRAL' WHEN 1 THEN 'ALLOW' WHEN 2 THEN 'DENY' WHEN 3 THEN 'ALLOWWITHDELEGATION' END AS AuthorizationType,
                       [netsqlazman_Authorizations].ValidFrom, [netsqlazman_Authorizations].ValidTo
FROM         dbo.[netsqlazman_Authorizations]() [netsqlazman_Authorizations] INNER JOIN
                      dbo.[netsqlazman_Items]() [netsqlazman_Items] ON [netsqlazman_Authorizations].ItemId = [netsqlazman_Items].ItemId INNER JOIN
                      dbo.[netsqlazman_Applications]() [netsqlazman_Applications] ON [netsqlazman_Items].ApplicationId = [netsqlazman_Applications].ApplicationId INNER JOIN
                      dbo.[netsqlazman_Stores]() [netsqlazman_Stores] ON [netsqlazman_Applications].StoreId = [netsqlazman_Stores].StoreId


