CREATE VIEW [dbo].[netsqlazman_ApplicationsView]
AS
SELECT     [netsqlazman_Stores].StoreId, [netsqlazman_Stores].Name AS StoreName, [netsqlazman_Stores].Description AS StoreDescription, [netsqlazman_Applications].ApplicationId, [netsqlazman_Applications].Name AS ApplicationName, 
                      [netsqlazman_Applications].Description AS ApplicationDescription
FROM         dbo.[netsqlazman_Applications]() [netsqlazman_Applications] INNER JOIN
                      dbo.[netsqlazman_Stores]() [netsqlazman_Stores] ON [netsqlazman_Applications].StoreId = [netsqlazman_Stores].StoreId


