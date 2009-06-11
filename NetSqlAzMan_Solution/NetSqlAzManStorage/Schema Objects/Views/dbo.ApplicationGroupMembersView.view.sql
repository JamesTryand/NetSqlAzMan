CREATE VIEW [dbo].[netsqlazman_ApplicationGroupMembersView]
AS
SELECT     Stores.StoreId, Applications.ApplicationId, ApplicationGroupMembers.ApplicationGroupMemberId, ApplicationGroupMembers.ApplicationGroupId, 
                      ApplicationGroups.Name AS ApplicationGroup, dbo.GetNameFromSid(Stores.Name, Applications.Name, ApplicationGroupMembers.objectSid, 
                      ApplicationGroupMembers.WhereDefined) AS Name, ApplicationGroupMembers.objectSid, 
                      CASE WhereDefined WHEN 0 THEN 'Store' WHEN 1 THEN 'Application' WHEN 2 THEN 'LDap' WHEN 3 THEN 'Local' WHEN 4 THEN 'DATABASE' END AS WhereDefined,
                       CASE IsMember WHEN 1 THEN 'Member' WHEN 0 THEN 'Non-Member' END AS MemberType
FROM         dbo.[netsqlazman_ApplicationGroupMembers]() ApplicationGroupMembers INNER JOIN
                      dbo.[netsqlazman_ApplicationGroups]() ApplicationGroups ON ApplicationGroupMembers.ApplicationGroupId = ApplicationGroups.ApplicationGroupId INNER JOIN
                      dbo.Applications() Applications ON ApplicationGroups.ApplicationId = Applications.ApplicationId INNER JOIN
                      dbo.Stores() Stores ON Applications.StoreId = Stores.StoreId


