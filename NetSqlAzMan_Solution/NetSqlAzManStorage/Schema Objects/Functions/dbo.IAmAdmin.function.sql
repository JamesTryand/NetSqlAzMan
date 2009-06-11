CREATE FUNCTION [dbo].[IAmAdmin] ()  
RETURNS bit AS  
BEGIN 
DECLARE @result bit
IF IS_MEMBER('db_owner')=1 OR IS_MEMBER('NetSqlAzMan_Administrators')=1
	SET @result = 1
ELSE
	SET @result = 0
RETURN @result
END


