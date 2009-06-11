CREATE TRIGGER [dbo].[ApplicationGroupDeleteTrigger] ON [dbo].[netsqlazman_ApplicationGroupsTable] 
FOR DELETE 
AS
DECLARE @DELETEDOBJECTSID int
DECLARE applicationgroups_cur CURSOR FAST_FORWARD FOR SELECT objectSid FROM deleted
OPEN applicationgroups_cur
FETCH NEXT from applicationgroups_cur INTO @DELETEDOBJECTSID
WHILE @@FETCH_STATUS = 0
BEGIN
	DELETE FROM dbo.[netsqlazman_ApplicationGroupMembersTable] WHERE objectSid = @DELETEDOBJECTSID AND WhereDefined = 1
	DELETE FROM dbo.[netsqlazman_AuthorizationsTable] WHERE objectSid = @DELETEDOBJECTSID AND objectSidWhereDefined = 1
	FETCH NEXT from applicationgroups_cur INTO @DELETEDOBJECTSID
END
CLOSE applicationgroups_cur
DEALLOCATE applicationgroups_cur


