CREATE VIEW [dbo].[netsqlazman_ItemAttributesView]
AS
SELECT     [netsqlazman_Items].ItemId, [netsqlazman_Items].ApplicationId, [netsqlazman_Items].Name, [netsqlazman_Items].Description, 
                      CASE [netsqlazman_Items].ItemType WHEN 0 THEN 'Role' WHEN 1 THEN 'Task' WHEN 2 THEN 'Operation' END AS ItemType, [netsqlazman_ItemAttributes].ItemAttributeId, 
                      [netsqlazman_ItemAttributes].AttributeKey, [netsqlazman_ItemAttributes].AttributeValue
FROM         dbo.[netsqlazman_Items]() [netsqlazman_Items] INNER JOIN
                      dbo.[netsqlazman_ItemAttributes]() [netsqlazman_ItemAttributes] ON [netsqlazman_Items].ItemId = [netsqlazman_ItemAttributes].ItemId


