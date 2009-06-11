CREATE VIEW [dbo].[netsqlazman_ApplicationGroupMembersView]
AS
SELECT     [netsqlazman_Stores].StoreId, [netsqlazman_Applications].ApplicationId, [netsqlazman_ApplicationGroupMembers].ApplicationGroupMemberId, [netsqlazman_ApplicationGroupMembers].ApplicationGroupId, 
                      [netsqlazman_ApplicationGroups].Name AS ApplicationGroup, dbo.[netsqlazman_GetNameFromSid]([netsqlazman_Stores].Name, [netsqlazman_Applications].Name, [netsqlazman_ApplicationGroupMembers].objectSid, 
                      [netsqlazman_ApplicationGroupMembers].WhereDefined) AS Name, [netsqlazman_ApplicationGroupMembers].objectSid, 
                      CASE WhereDefined WHEN 0 THEN 'Store' WHEN 1 THEN 'Application' WHEN 2 THEN 'LDap' WHEN 3 THEN 'Local' WHEN 4 THEN 'DATABASE' END AS WhereDefined,
                       CASE IsMember WHEN 1 THEN 'Member' WHEN 0 THEN 'Non-Member' END AS MemberType
FROM         dbo.[netsqlazman_ApplicationGroupMembers]() [netsqlazman_ApplicationGroupMembers] INNER JOIN
                      dbo.[netsqlazman_ApplicationGroups]() [netsqlazman_ApplicationGroups] ON [netsqlazman_ApplicationGroupMembers].ApplicationGroupId = [netsqlazman_ApplicationGroups].ApplicationGroupId INNER JOIN
                      dbo.[netsqlazman_Applications]() [netsqlazman_Applications] ON [netsqlazman_ApplicationGroups].ApplicationId = [netsqlazman_Applications].ApplicationId INNER JOIN
                      dbo.[netsqlazman_Stores]() [netsqlazman_Stores] ON [netsqlazman_Applications].StoreId = [netsqlazman_Stores].StoreId


