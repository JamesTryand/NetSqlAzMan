CREATE VIEW [dbo].[netsqlazman_ApplicationsView]
AS
SELECT     Stores.StoreId, Stores.Name AS StoreName, Stores.Description AS StoreDescription, Applications.ApplicationId, Applications.Name AS ApplicationName, 
                      Applications.Description AS ApplicationDescription
FROM         dbo.Applications() Applications INNER JOIN
                      dbo.Stores() Stores ON Applications.StoreId = Stores.StoreId


