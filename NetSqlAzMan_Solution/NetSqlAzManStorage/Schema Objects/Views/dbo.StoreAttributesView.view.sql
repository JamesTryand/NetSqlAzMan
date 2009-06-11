CREATE VIEW [dbo].[netsqlazman_StoreAttributesView]
AS
SELECT     Stores.StoreId, Stores.Name, Stores.Description, StoreAttributes.StoreAttributeId, StoreAttributes.AttributeKey, StoreAttributes.AttributeValue
FROM         dbo.Stores() Stores INNER JOIN
                      dbo.StoreAttributes() StoreAttributes ON Stores.StoreId = StoreAttributes.StoreId


