CREATE VIEW [dbo].[netsqlazman_ApplicationAttributesView]
AS
SELECT     [netsqlazman_Applications].ApplicationId, [netsqlazman_Applications].StoreId, [netsqlazman_Applications].Name, [netsqlazman_Applications].Description, ApplicationAttributes.ApplicationAttributeId, 
                      ApplicationAttributes.AttributeKey, ApplicationAttributes.AttributeValue
FROM         dbo.[netsqlazman_Applications]() [netsqlazman_Applications] INNER JOIN
                      dbo.[netsqlazman_ApplicationAttributes]() ApplicationAttributes ON [netsqlazman_Applications].ApplicationId = ApplicationAttributes.ApplicationId


