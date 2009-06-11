CREATE PROCEDURE [dbo].[netsqlazman_ApplicationInsert]
(
	@StoreId int,
	@Name nvarchar(255),
	@Description nvarchar(1024)
)
AS
IF EXISTS(SELECT StoreId FROM dbo.[netsqlazman_Stores]() WHERE StoreId = @StoreId) AND dbo.[netsqlazman_CheckStorePermissions](@StoreId, 2) = 1
BEGIN
	INSERT INTO [dbo].[netsqlazman_ApplicationsTable] ([StoreId], [Name], [Description]) VALUES (@StoreId, @Name, @Description);
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Store permission denied.', 16, 1)


