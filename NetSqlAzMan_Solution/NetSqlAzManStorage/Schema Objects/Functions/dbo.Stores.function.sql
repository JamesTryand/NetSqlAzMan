CREATE FUNCTION [dbo].[Stores] ()
RETURNS TABLE 
AS
RETURN
	SELECT dbo.[netsqlazman_StoresTable].* FROM dbo.[netsqlazman_StoresTable]
	WHERE dbo.CheckStorePermissions([netsqlazman_StoresTable].StoreId, 0) = 1


