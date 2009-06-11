CREATE VIEW [dbo].[netsqlazman_StoreAttributesView]
AS
SELECT     [netsqlazman_Stores].StoreId, [netsqlazman_Stores].Name, [netsqlazman_Stores].Description, [netsqlazman_StoreAttributes].StoreAttributeId, [netsqlazman_StoreAttributes].AttributeKey, [netsqlazman_StoreAttributes].AttributeValue
FROM         dbo.[netsqlazman_Stores]() [netsqlazman_Stores] INNER JOIN
                      dbo.[netsqlazman_StoreAttributes]() [netsqlazman_StoreAttributes] ON [netsqlazman_Stores].StoreId = [netsqlazman_StoreAttributes].StoreId


