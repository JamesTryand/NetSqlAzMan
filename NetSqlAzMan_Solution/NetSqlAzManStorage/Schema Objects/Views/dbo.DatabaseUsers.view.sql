CREATE VIEW [dbo].[netsqlazman_DatabaseUsers]
AS
SELECT     *
FROM         dbo.[netsqlazman_GetDBUsers](NULL, NULL, DEFAULT, DEFAULT) GetDBUsers


