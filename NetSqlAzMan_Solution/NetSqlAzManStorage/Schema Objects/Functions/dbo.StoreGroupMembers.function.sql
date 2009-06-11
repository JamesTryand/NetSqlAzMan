CREATE FUNCTION [dbo].[StoreGroupMembers] ()
RETURNS TABLE
AS
RETURN
	SELECT     dbo.[netsqlazman_StoreGroupMembersTable].*
	FROM         dbo.[netsqlazman_StoreGroupMembersTable] INNER JOIN
	                      dbo.StoreGroups() StoreGroups ON dbo.[netsqlazman_StoreGroupMembersTable].StoreGroupId = StoreGroups.StoreGroupId


