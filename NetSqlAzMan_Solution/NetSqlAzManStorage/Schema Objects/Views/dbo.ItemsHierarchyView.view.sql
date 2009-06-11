CREATE VIEW [dbo].[netsqlazman_ItemsHierarchyView]
AS
SELECT     Items.ItemId, Items.ApplicationId, Items.Name, Items.Description, 
                      CASE Items.ItemType WHEN 0 THEN 'Role' WHEN 1 THEN 'Task' WHEN 2 THEN 'Operation' END AS ItemType, Items_1.ItemId AS MemberItemId, 
                      Items_1.ApplicationId AS MemberApplicationId, Items_1.Name AS MemberName, Items_1.Description AS MemberDescription, 
                      CASE Items_1.ItemType WHEN 0 THEN 'Role' WHEN 1 THEN 'Task' WHEN 2 THEN 'Operation' END AS MemberType
FROM         dbo.Items() Items_1 INNER JOIN
                      dbo.ItemsHierarchy() ItemsHierarchy ON Items_1.ItemId = ItemsHierarchy.ItemId INNER JOIN
                      dbo.Items() Items ON ItemsHierarchy.MemberOfItemId = Items.ItemId INNER JOIN
                      dbo.Applications() Applications ON Items.ApplicationId = Applications.ApplicationId


