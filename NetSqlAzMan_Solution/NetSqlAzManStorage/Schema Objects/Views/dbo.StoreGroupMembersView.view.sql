CREATE VIEW [dbo].[netsqlazman_StoreGroupMembersView]
AS
SELECT     StoreGroupMembers.StoreGroupMemberId, StoreGroupMembers.StoreGroupId, StoreGroups.Name AS StoreGroup, dbo.[netsqlazman_GetNameFromSid](Stores.Name, NULL, 
                      StoreGroupMembers.objectSid, StoreGroupMembers.WhereDefined) AS Name, StoreGroupMembers.objectSid, 
                      CASE WhereDefined WHEN 0 THEN 'Store' WHEN 1 THEN 'Application' WHEN 2 THEN 'LDap' WHEN 3 THEN 'Local' WHEN 4 THEN 'DATABASE' END AS WhereDefined,
                       CASE IsMember WHEN 1 THEN 'Member' WHEN 0 THEN 'Non-Member' END AS MemberType
FROM         dbo.[netsqlazman_StoreGroupMembers]() StoreGroupMembers INNER JOIN
                      dbo.[netsqlazman_StoreGroups]() StoreGroups ON StoreGroupMembers.StoreGroupId = StoreGroups.StoreGroupId INNER JOIN
                      dbo.[netsqlazman_Stores]() Stores ON StoreGroups.StoreId = Stores.StoreId


