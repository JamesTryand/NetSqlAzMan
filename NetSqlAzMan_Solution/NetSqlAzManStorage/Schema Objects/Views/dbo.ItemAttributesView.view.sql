CREATE VIEW [dbo].[netsqlazman_ItemAttributesView]
AS
SELECT     Items.ItemId, Items.ApplicationId, Items.Name, Items.Description, 
                      CASE Items.ItemType WHEN 0 THEN 'Role' WHEN 1 THEN 'Task' WHEN 2 THEN 'Operation' END AS ItemType, ItemAttributes.ItemAttributeId, 
                      ItemAttributes.AttributeKey, ItemAttributes.AttributeValue
FROM         dbo.Items() Items INNER JOIN
                      dbo.ItemAttributes() ItemAttributes ON Items.ItemId = ItemAttributes.ItemId


