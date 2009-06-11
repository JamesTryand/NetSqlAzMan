CREATE PROCEDURE [dbo].[netsqlazman_StoreInsert]
(
	@Name nvarchar(255),
	@Description nvarchar(1024)
)
AS
INSERT INTO [dbo].[netsqlazman_StoresTable] ([Name], [Description]) VALUES (@Name, @Description);
RETURN SCOPE_IDENTITY()


