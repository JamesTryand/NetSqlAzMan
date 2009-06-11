-- =============================================
-- Author:		Andrea Ferendeles
-- Create date: 13/04/2006
-- Description:	Get Name From Sid
-- =============================================
CREATE FUNCTION [dbo].[GetNameFromSid] (@StoreName nvarchar(255), @ApplicationName nvarchar(255), @Sid varbinary(85), @SidWhereDefined tinyint)
RETURNS nvarchar(255)
AS
BEGIN

DECLARE @Name nvarchar(255)
SET @Name = NULL

IF (@SidWhereDefined=0) --Store
BEGIN
SET @Name = (SELECT TOP 1 Name FROM dbo.StoreGroups() WHERE objectSid = @Sid)
END
ELSE IF (@SidWhereDefined=1) --Application 
BEGIN
SET @Name = (SELECT TOP 1 Name FROM dbo.[netsqlazman_ApplicationGroups]() WHERE objectSid = @Sid)
END
ELSE IF (@SidWhereDefined=2 OR @SidWhereDefined=3) --LDAP or LOCAL
BEGIN
SET @Name = (SELECT Suser_Sname(@Sid))
END
ELSE IF (@SidWhereDefined=4) --Database
BEGIN
SET @Name = (SELECT DBUserName FROM dbo.GetDBUsers(@StoreName, @ApplicationName, @Sid, NULL))
END
IF (@Name IS NULL)
BEGIN
	SET @Name = @Sid
END
RETURN @Name
END


