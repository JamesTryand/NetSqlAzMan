CREATE VIEW [dbo].[netsqlazman_ApplicationAttributesView]
AS
SELECT     Applications.ApplicationId, Applications.StoreId, Applications.Name, Applications.Description, ApplicationAttributes.ApplicationAttributeId, 
                      ApplicationAttributes.AttributeKey, ApplicationAttributes.AttributeValue
FROM         dbo.Applications() Applications INNER JOIN
                      dbo.[netsqlazman_ApplicationAttributes]() ApplicationAttributes ON Applications.ApplicationId = ApplicationAttributes.ApplicationId


