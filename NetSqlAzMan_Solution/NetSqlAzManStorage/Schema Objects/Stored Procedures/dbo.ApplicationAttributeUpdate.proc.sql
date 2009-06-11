CREATE PROCEDURE [dbo].[netsqlazman_ApplicationAttributeUpdate]
(
	@ApplicationId int,
	@AttributeKey nvarchar(255),
	@AttributeValue nvarchar(4000),
	@Original_ApplicationAttributeId int
)
AS
IF EXISTS(SELECT ApplicationAttributeId FROM dbo.[netsqlazman_ApplicationAttributes]() WHERE ApplicationAttributeId = @Original_ApplicationAttributeId) AND dbo.[netsqlazman_CheckApplicationPermissions](@ApplicationId, 2) = 1
	UPDATE [dbo].[netsqlazman_ApplicationAttributesTable] SET [AttributeKey] = @AttributeKey, [AttributeValue] = @AttributeValue WHERE [ApplicationAttributeId] = @Original_ApplicationAttributeId AND [ApplicationId] = @ApplicationId
ELSE
	RAISERROR ('Applicaction Permission denied.', 16, 1)


