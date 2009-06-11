/* 
   @ROLEID = { 0 READERS, 1 USERS, 2 MANAGERS}
*/
CREATE FUNCTION [dbo].[netsqlazman_CheckApplicationPermissions](@APPLICATIONID int, @ROLEID tinyint)
RETURNS bit
AS
BEGIN
DECLARE @RESULT bit
IF @APPLICATIONID IS NULL OR @ROLEID IS NULL
	SET @RESULT = 0	
ELSE
BEGIN
	IF EXISTS (
		SELECT     dbo.[netsqlazman_ApplicationPermissionsTable].ApplicationId
		FROM         dbo.[netsqlazman_ApplicationsTable] INNER JOIN
		                      dbo.[netsqlazman_StoresTable] ON dbo.[netsqlazman_ApplicationsTable].StoreId = dbo.[netsqlazman_StoresTable].StoreId LEFT OUTER JOIN
		                      dbo.[netsqlazman_StorePermissionsTable] ON dbo.[netsqlazman_StoresTable].StoreId = dbo.[netsqlazman_StorePermissionsTable].StoreId LEFT OUTER JOIN
		                      dbo.[netsqlazman_ApplicationPermissionsTable] ON dbo.[netsqlazman_ApplicationsTable].ApplicationId = dbo.[netsqlazman_ApplicationPermissionsTable].ApplicationId
		WHERE
		IS_MEMBER('db_owner')=1 OR IS_MEMBER('NetSqlAzMan_Administrators')=1 OR 
		(@ROLEID = 0 AND IS_MEMBER('NetSqlAzMan_Readers')=1 OR 
		@ROLEID = 1 AND IS_MEMBER('NetSqlAzMan_Users')=1 OR 
		@ROLEID = 2 AND IS_MEMBER('NetSqlAzMan_Managers')=1) AND
		(
		(dbo.[netsqlazman_ApplicationPermissionsTable].ApplicationId = @APPLICATIONID AND dbo.[netsqlazman_ApplicationPermissionsTable].NetSqlAzManFixedServerRole >= @ROLEID AND 
		(SUSER_SNAME(SUSER_SID())=[netsqlazman_ApplicationPermissionsTable].SqlUserOrRole AND [netsqlazman_ApplicationPermissionsTable].IsSqlRole = 0
		OR IS_MEMBER([netsqlazman_ApplicationPermissionsTable].SqlUserOrRole)=1 AND [netsqlazman_ApplicationPermissionsTable].IsSqlRole = 1)) OR
	
		dbo.[netsqlazman_ApplicationsTable].ApplicationId = @APPLICATIONID AND 
		(dbo.[netsqlazman_StorePermissionsTable].StoreId = dbo.[netsqlazman_ApplicationsTable].StoreId AND dbo.[netsqlazman_StorePermissionsTable].NetSqlAzManFixedServerRole >= @ROLEID AND 
		(SUSER_SNAME(SUSER_SID())=[netsqlazman_StorePermissionsTable].SqlUserOrRole AND [netsqlazman_StorePermissionsTable].IsSqlRole = 0 OR
		IS_MEMBER([netsqlazman_StorePermissionsTable].SqlUserOrRole)=1 AND [netsqlazman_StorePermissionsTable].IsSqlRole = 1))

))
	
	SET @RESULT = 1
	ELSE
	SET @RESULT = 0
END
RETURN @RESULT
END


