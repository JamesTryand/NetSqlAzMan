CREATE FUNCTION [dbo].[netsqlazman_Stores] ()
RETURNS TABLE 
AS
RETURN
	SELECT dbo.[netsqlazman_StoresTable].* FROM dbo.[netsqlazman_StoresTable]
	WHERE dbo.[netsqlazman_CheckStorePermissions]([netsqlazman_StoresTable].StoreId, 0) = 1


