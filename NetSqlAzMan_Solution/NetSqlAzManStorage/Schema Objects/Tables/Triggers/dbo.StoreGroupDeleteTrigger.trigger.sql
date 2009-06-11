CREATE TRIGGER [dbo].[StoreGroupDeleteTrigger] ON [dbo].[netsqlazman_StoreGroupsTable] 
FOR DELETE 
AS
DECLARE @DELETEDOBJECTSID int
DECLARE storegroups_cur CURSOR FAST_FORWARD FOR SELECT objectSid FROM deleted
OPEN storegroups_cur
FETCH NEXT from storegroups_cur INTO @DELETEDOBJECTSID
WHILE @@FETCH_STATUS = 0
BEGIN
	DELETE FROM dbo.[netsqlazman_StoreGroupMembersTable] WHERE objectSid = @DELETEDOBJECTSID AND WhereDefined = 0
	DELETE FROM dbo.[netsqlazman_ApplicationGroupMembersTable] WHERE objectSid = @DELETEDOBJECTSID AND WhereDefined = 0
	DELETE FROM dbo.[netsqlazman_AuthorizationsTable] WHERE objectSid = @DELETEDOBJECTSID AND objectSidWhereDefined = 0
	FETCH NEXT from storegroups_cur INTO @DELETEDOBJECTSID
END
CLOSE storegroups_cur
DEALLOCATE storegroups_cur


