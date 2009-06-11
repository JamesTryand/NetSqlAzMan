CREATE PROCEDURE dbo.[netsqlazman_BuildUserPermissionCache](@STORENAME nvarchar(255), @APPLICATIONNAME nvarchar(255))
AS 
-- Hierarchy
SET NOCOUNT ON
SELECT     Items.Name AS ItemName, Items_1.Name AS ParentItemName
FROM         dbo.Items() Items_1 INNER JOIN
                      dbo.ItemsHierarchy() ItemsHierarchy ON Items_1.ItemId = ItemsHierarchy.MemberOfItemId RIGHT OUTER JOIN
                      dbo.Applications() Applications INNER JOIN
                      dbo.Stores() Stores ON Applications.StoreId = Stores.StoreId INNER JOIN
                      dbo.Items() Items ON Applications.ApplicationId = Items.ApplicationId ON ItemsHierarchy.ItemId = Items.ItemId
WHERE     (Stores.Name = @STORENAME) AND (Applications.Name = @APPLICATIONNAME)

-- Item Authorizations
SELECT DISTINCT Items.Name AS ItemName, Authorizations.ValidFrom, Authorizations.ValidTo
FROM         dbo.Authorizations() Authorizations INNER JOIN
                      dbo.Items() Items ON Authorizations.ItemId = Items.ItemId INNER JOIN
                      dbo.Stores() Stores INNER JOIN
                      dbo.Applications() Applications ON Stores.StoreId = Applications.StoreId ON Items.ApplicationId = Applications.ApplicationId
WHERE     (Authorizations.AuthorizationType <> 0) AND (Stores.Name = @STORENAME) AND (Applications.Name = @APPLICATIONNAME)
UNION
SELECT DISTINCT Items.Name AS ItemName, NULL ValidFrom, NULL ValidTo
FROM         dbo.Items() Items INNER JOIN
                      dbo.Stores() Stores INNER JOIN
                      dbo.Applications() Applications ON Stores.StoreId = Applications.StoreId ON Items.ApplicationId = Applications.ApplicationId
WHERE     (Stores.Name = @STORENAME) AND (Applications.Name = @APPLICATIONNAME) AND Items.BizRuleId IS NOT NULL
SET NOCOUNT OFF


