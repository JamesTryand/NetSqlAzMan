CREATE PROCEDURE [dbo].[netsqlazman_BuildUserPermissionCache](@STORENAME nvarchar(255), @APPLICATIONNAME nvarchar(255))
AS 
-- Hierarchy
SET NOCOUNT ON
SELECT     Items.Name AS ItemName, Items_1.Name AS ParentItemName
FROM         dbo.[netsqlazman_Items]() Items_1 INNER JOIN
                      dbo.[netsqlazman_ItemsHierarchy]() ItemsHierarchy ON Items_1.ItemId = ItemsHierarchy.MemberOfItemId RIGHT OUTER JOIN
                      dbo.[netsqlazman_Applications]() Applications INNER JOIN
                      dbo.[netsqlazman_Stores]() Stores ON Applications.StoreId = Stores.StoreId INNER JOIN
                      dbo.[netsqlazman_Items]() Items ON Applications.ApplicationId = Items.ApplicationId ON ItemsHierarchy.ItemId = Items.ItemId
WHERE     (Stores.Name = @STORENAME) AND (Applications.Name = @APPLICATIONNAME)

-- Item Authorizations
SELECT DISTINCT Items.Name AS ItemName, Authorizations.ValidFrom, Authorizations.ValidTo
FROM         dbo.[netsqlazman_Authorizations]() Authorizations INNER JOIN
                      dbo.[netsqlazman_Items]() Items ON Authorizations.ItemId = Items.ItemId INNER JOIN
                      dbo.[netsqlazman_Stores]() Stores INNER JOIN
                      dbo.[netsqlazman_Applications]() Applications ON Stores.StoreId = Applications.StoreId ON Items.ApplicationId = Applications.ApplicationId
WHERE     (Authorizations.AuthorizationType <> 0) AND (Stores.Name = @STORENAME) AND (Applications.Name = @APPLICATIONNAME)
UNION
SELECT DISTINCT Items.Name AS ItemName, NULL ValidFrom, NULL ValidTo
FROM         dbo.[netsqlazman_Items]() Items INNER JOIN
                      dbo.[netsqlazman_Stores]() Stores INNER JOIN
                      dbo.[netsqlazman_Applications]() Applications ON Stores.StoreId = Applications.StoreId ON Items.ApplicationId = Applications.ApplicationId
WHERE     (Stores.Name = @STORENAME) AND (Applications.Name = @APPLICATIONNAME) AND Items.BizRuleId IS NOT NULL
SET NOCOUNT OFF


