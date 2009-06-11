CREATE VIEW [dbo].[netsqlazman_DatabaseUsers]
AS
SELECT     *
FROM         dbo.GetDBUsers(NULL, NULL, DEFAULT, DEFAULT) GetDBUsers


