CREATE PROCEDURE [dbo].[netsqlazman_DirectCheckAccess] (@STORENAME nvarchar(255), @APPLICATIONNAME nvarchar(255), @ITEMNAME nvarchar(255), @OPERATIONSONLY BIT, @TOKEN IMAGE, @USERGROUPSCOUNT INT, @VALIDFOR DATETIME, @LDAPPATH nvarchar(4000), @AUTHORIZATION_TYPE TINYINT OUTPUT, @RETRIEVEATTRIBUTES BIT) 
AS
--Memo: 0 - Role; 1 - Task; 2 - Operation
SET NOCOUNT ON
DECLARE @STOREID int
DECLARE @ApplicationId int
DECLARE @ITEMID INT

-- CHECK STORE EXISTANCE/PERMISSIONS
Select @STOREID = StoreId FROM dbo.[netsqlazman_Stores]() WHERE Name = @STORENAME
IF @STOREID IS NULL
	BEGIN
	RAISERROR ('Store not found or Store permission denied.', 16, 1)
	RETURN 1
	END
-- CHECK APPLICATION EXISTANCE/PERMISSIONS
Select @ApplicationId = ApplicationId FROM dbo.[netsqlazman_Applications]() WHERE Name = @APPLICATIONNAME And StoreId = @STOREID
IF @ApplicationId IS NULL
	BEGIN
	RAISERROR ('Application not found or Application permission denied.', 16, 1)
	RETURN 1
	END

SELECT @ITEMID = Items.ItemId
	FROM         dbo.[netsqlazman_Applications]() Applications INNER JOIN
	                      dbo.[netsqlazman_Items]() Items ON Applications.ApplicationId = Items.ApplicationId INNER JOIN
	                      dbo.[netsqlazman_Stores]() Stores ON Applications.StoreId = Stores.StoreId
	WHERE     (Stores.StoreId = @STOREID) AND (Applications.ApplicationId = @ApplicationId) AND (Items.Name = @ITEMNAME) AND (@OPERATIONSONLY = 1 AND Items.ItemType=2 OR @OPERATIONSONLY = 0)
IF @ITEMID IS NULL
	BEGIN
	RAISERROR ('Item not found.', 16, 1)
	RETURN 1
	END
-- PREPARE RESULTSET FOR BIZ RULE CHECKING
CREATE TABLE #PARTIAL_RESULTS_TABLE 
	(   
		[ItemId] [int] NOT NULL ,
		[ItemName] [nvarchar] (255)  NOT NULL ,
		[ItemType] [tinyint] NOT NULL,
		[AuthorizationType] TINYINT,
		[BizRuleId] [int] NULL ,
		[BizRuleSource] TEXT,
		[BizRuleLanguage] TINYINT
	)
-- PREPARE RESULTSET FOR ATTRIBUTES
IF @RETRIEVEATTRIBUTES = 1
BEGIN
	CREATE TABLE #ATTRIBUTES_TABLE 
	(   
		[AttributeKey] [nvarchar] (255)  NOT NULL,
		[AttributeValue] [nvarchar] (4000)  NOT NULL,
		[ItemId] INT NULL
	)
--------------------------------------------------------------------------------
-- GET STORE AND APPLICATION ATTRIBUTES
--------------------------------------------------------------------------------
	INSERT INTO #ATTRIBUTES_TABLE SELECT AttributeKey, AttributeValue, NULL FROM dbo.[netsqlazman_StoreAttributesTable] StoreAttributes INNER JOIN dbo.[netsqlazman_StoresTable] Stores ON StoreAttributes.StoreId = Stores.StoreId WHERE Stores.StoreId = @STOREID
	INSERT INTO #ATTRIBUTES_TABLE SELECT AttributeKey, AttributeValue, NULL FROM dbo.[netsqlazman_ApplicationAttributesTable] ApplicationAttributes INNER JOIN dbo.[netsqlazman_ApplicationsTable] Applications ON ApplicationAttributes.ApplicationId = Applications.ApplicationId WHERE Applications.ApplicationId = @ApplicationId
END
--------------------------------------------------------------------------------
DECLARE @USERSID varbinary(85)
DECLARE @I INT
DECLARE @INDEX INT
DECLARE @APP VARBINARY(85)
DECLARE @SETTINGVALUE nvarchar(255)
DECLARE @NETSQLAZMANMODE bit

SELECT @SETTINGVALUE = SettingValue FROM dbo.[netsqlazman_Settings] WHERE SettingName = 'Mode'
IF @SETTINGVALUE = 'Developer' 
BEGIN
	SET @NETSQLAZMANMODE = 1 
END
ELSE 
BEGIN
	SET @NETSQLAZMANMODE = 0
END

CREATE TABLE #USERGROUPS (objectSid VARBINARY(85))
-- Get User Sid
IF @USERGROUPSCOUNT>0
BEGIN
	SET @USERSID = SUBSTRING(@TOKEN,1,85)
	SET @I = CHARINDEX(0x01, @USERSID)
	SET @USERSID = SUBSTRING(@USERSID, @I, 85-@I+1)
	-- Get User Groups Sid
	SET @I = 0
	WHILE (@I<@USERGROUPSCOUNT)
	BEGIN
		SET @APP = SUBSTRING(@TOKEN,(@I+1)*85+1,85) --GET USER GROUP TOKEN PORTION
		SET @INDEX = CHARINDEX(0x01, @APP) -- FIND TOKEN START (0x01)
		SET @APP = SUBSTRING(@APP, @INDEX, 85-@INDEX+1) -- EXTRACT USER GROUP SID
		INSERT INTO #USERGROUPS (objectSid) VALUES (@APP)
		SET @I = @I + 1
	END
END
ELSE
BEGIN
	SET @USERSID = @TOKEN
END

EXEC dbo.[netsqlazman_CheckAccess] @ITEMID, @USERSID, @VALIDFOR, @LDAPPATH, @AUTHORIZATION_TYPE OUTPUT, @NETSQLAZMANMODE, @RETRIEVEATTRIBUTES
SELECT * FROM #PARTIAL_RESULTS_TABLE
IF @RETRIEVEATTRIBUTES = 1
	SELECT * FROM #ATTRIBUTES_TABLE
DROP TABLE #PARTIAL_RESULTS_TABLE
IF @RETRIEVEATTRIBUTES = 1
	DROP TABLE #ATTRIBUTES_TABLE
DROP TABLE #USERGROUPS
SET NOCOUNT OFF


