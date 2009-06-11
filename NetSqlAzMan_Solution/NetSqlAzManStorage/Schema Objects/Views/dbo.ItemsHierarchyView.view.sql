CREATE VIEW [dbo].[netsqlazman_ItemsHierarchyView]
AS
SELECT     [netsqlazman_Items].ItemId, [netsqlazman_Items].ApplicationId, [netsqlazman_Items].Name, [netsqlazman_Items].Description, 
                      CASE [netsqlazman_Items].ItemType WHEN 0 THEN 'Role' WHEN 1 THEN 'Task' WHEN 2 THEN 'Operation' END AS ItemType, Items_1.ItemId AS MemberItemId, 
                      Items_1.ApplicationId AS MemberApplicationId, Items_1.Name AS MemberName, Items_1.Description AS MemberDescription, 
                      CASE Items_1.ItemType WHEN 0 THEN 'Role' WHEN 1 THEN 'Task' WHEN 2 THEN 'Operation' END AS MemberType
FROM         dbo.[netsqlazman_Items]() Items_1 INNER JOIN
                      dbo.[netsqlazman_ItemsHierarchy]() [netsqlazman_ItemsHierarchy] ON Items_1.ItemId = [netsqlazman_ItemsHierarchy].ItemId INNER JOIN
                      dbo.[netsqlazman_Items]() [netsqlazman_Items] ON [netsqlazman_ItemsHierarchy].MemberOfItemId = [netsqlazman_Items].ItemId INNER JOIN
                      dbo.[netsqlazman_Applications]() [netsqlazman_Applications] ON [netsqlazman_Items].ApplicationId = [netsqlazman_Applications].ApplicationId


