CREATE FUNCTION [dbo].[netsqlazman_StoreGroupMembers] ()
RETURNS TABLE
AS
RETURN
	SELECT     dbo.[netsqlazman_StoreGroupMembersTable].*
	FROM         dbo.[netsqlazman_StoreGroupMembersTable] INNER JOIN
	                      dbo.[netsqlazman_StoreGroups]() StoreGroups ON dbo.[netsqlazman_StoreGroupMembersTable].StoreGroupId = StoreGroups.StoreGroupId


