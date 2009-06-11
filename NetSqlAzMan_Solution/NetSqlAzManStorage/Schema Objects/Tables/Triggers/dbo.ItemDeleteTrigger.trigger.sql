CREATE TRIGGER [dbo].[ItemDeleteTrigger] ON [dbo].[netsqlazman_ItemsTable] 
FOR DELETE 
AS
DECLARE @DELETEDITEMID int
DECLARE @BIZRULEID int
DECLARE items_cur CURSOR FAST_FORWARD FOR SELECT ItemId, BizRuleId FROM deleted
OPEN items_cur
FETCH NEXT from items_cur INTO @DELETEDITEMID, @BIZRULEID
WHILE @@FETCH_STATUS = 0
BEGIN
	DELETE FROM dbo.[netsqlazman_ItemsHierarchyTable] WHERE ItemId = @DELETEDITEMID OR MemberOfItemId = @DELETEDITEMID
	IF @BIZRULEID IS NOT NULL
		DELETE FROM dbo.[netsqlazman_BizRulesTable] WHERE BizRuleId = @BIZRULEID
	FETCH NEXT from items_cur INTO @DELETEDITEMID, @BIZRULEID
END
CLOSE items_cur
DEALLOCATE items_cur


