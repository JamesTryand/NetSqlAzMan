CREATE PROCEDURE [dbo].[netsqlazman_ApplicationUpdate]
(
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@Original_ApplicationId int
)
AS
IF EXISTS(SELECT ApplicationId FROM dbo.Applications() WHERE ApplicationId = @Original_ApplicationId) AND dbo.CheckApplicationPermissions(@Original_ApplicationId, 2) = 1
	UPDATE [dbo].[netsqlazman_ApplicationsTable] SET [Name] = @Name, [Description] = @Description WHERE [ApplicationId] = @Original_ApplicationId
ELSE
	RAISERROR ('Application permission denied.', 16, 1)


