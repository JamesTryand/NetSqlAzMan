CREATE PROCEDURE [dbo].[netsqlazman_StoreGroupInsert]
(
	@StoreId int,
	@objectSid varbinary(85),
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@LDapQuery nvarchar(4000),
	@GroupType tinyint
)
AS
IF dbo.[netsqlazman_CheckStorePermissions](@StoreId, 2) = 1
BEGIN
	INSERT INTO [dbo].[netsqlazman_StoreGroupsTable] ([StoreId], [objectSid], [Name], [Description], [LDapQuery], [GroupType]) VALUES (@StoreId, @objectSid, @Name, @Description, @LDapQuery, @GroupType);
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR ('Store permission denied.', 16, 1)


