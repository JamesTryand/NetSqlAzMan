--
-- Script To Delete dbo.ItemsHierarchyTriggerr Trigger In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Deleting dbo.ItemsHierarchyTriggerr Trigger'
GO

   DROP TRIGGER [dbo].[ItemsHierarchyTriggerr]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.ItemsHierarchyTriggerr Trigger Deleted Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Delete dbo.ItemsHierarchyTriggerr Trigger'
END
GO

--
-- Script To Update dbo.StoresTable Table In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.StoresTable Table'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO


IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysindexes WHERE name = N'Stores_Name_Unique_Index')
      DROP INDEX [dbo].[StoresTable].[Stores_Name_Unique_Index]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[StoresTable]
      ALTER COLUMN [Description] [nvarchar] (1024) NOT NULL
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[StoresTable]
      ALTER COLUMN [Name] [nvarchar] (255) NOT NULL
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   CREATE UNIQUE INDEX [Stores_Name_Unique_Index] ON [dbo].[StoresTable] ([Name])
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.StoresTable Table Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.StoresTable Table'
END
GO

--
-- Script To Update dbo.ApplicationsTable Table In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.ApplicationsTable Table'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO


IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysindexes WHERE name = N'Applications_StoreId_Name_Unique_Index')
      DROP INDEX [dbo].[ApplicationsTable].[Applications_StoreId_Name_Unique_Index]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysindexes WHERE name = N'IX_Applications')
      DROP INDEX [dbo].[ApplicationsTable].[IX_Applications]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[ApplicationsTable]
      ALTER COLUMN [Description] [nvarchar] (1024) NOT NULL
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[ApplicationsTable]
      ALTER COLUMN [Name] [nvarchar] (255) NOT NULL
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   CREATE UNIQUE INDEX [Applications_StoreId_Name_Unique_Index] ON [dbo].[ApplicationsTable] ([Name], [StoreId])
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   CREATE INDEX [IX_Applications] ON [dbo].[ApplicationsTable] ([ApplicationId], [Name])
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.ApplicationsTable Table Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.ApplicationsTable Table'
END
GO

--
-- Script To Update dbo.ApplicationAttributesTable Table In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.ApplicationAttributesTable Table'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO


IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysindexes WHERE name = N'ApplicationAttributes_AuhorizationId_AttributeKey_Unique_Index')
      DROP INDEX [dbo].[ApplicationAttributesTable].[ApplicationAttributes_AuhorizationId_AttributeKey_Unique_Index]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysindexes WHERE name = N'IX_ApplicationAttributes')
      DROP INDEX [dbo].[ApplicationAttributesTable].[IX_ApplicationAttributes]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[ApplicationAttributesTable]
      ALTER COLUMN [AttributeKey] [nvarchar] (255) NOT NULL
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[ApplicationAttributesTable]
      ALTER COLUMN [AttributeValue] [nvarchar] (4000) NOT NULL
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   CREATE UNIQUE INDEX [ApplicationAttributes_AuhorizationId_AttributeKey_Unique_Index] ON [dbo].[ApplicationAttributesTable] ([ApplicationId], [AttributeKey])
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   CREATE INDEX [IX_ApplicationAttributes] ON [dbo].[ApplicationAttributesTable] ([ApplicationId], [AttributeKey])
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.ApplicationAttributesTable Table Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.ApplicationAttributesTable Table'
END
GO

--
-- Script To Update dbo.ApplicationGroupsTable Table In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.ApplicationGroupsTable Table'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO


IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysindexes WHERE name = N'ApplicationGroups_ApplicationId_Name_Unique_Index')
      DROP INDEX [dbo].[ApplicationGroupsTable].[ApplicationGroups_ApplicationId_Name_Unique_Index]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysindexes WHERE name = N'IX_ApplicationGroups')
      DROP INDEX [dbo].[ApplicationGroupsTable].[IX_ApplicationGroups]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[ApplicationGroupsTable]
      ALTER COLUMN [Description] [nvarchar] (1024) NOT NULL
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[ApplicationGroupsTable]
      ALTER COLUMN [LDapQuery] [nvarchar] (4000) NULL
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[ApplicationGroupsTable]
      ALTER COLUMN [Name] [nvarchar] (255) NOT NULL
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   CREATE UNIQUE INDEX [ApplicationGroups_ApplicationId_Name_Unique_Index] ON [dbo].[ApplicationGroupsTable] ([ApplicationId], [Name])
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   CREATE INDEX [IX_ApplicationGroups] ON [dbo].[ApplicationGroupsTable] ([ApplicationId], [Name])
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.ApplicationGroupsTable Table Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.ApplicationGroupsTable Table'
END
GO

--
-- Script To Update dbo.ApplicationPermissionsTable Table In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.ApplicationPermissionsTable Table'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO


IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysindexes WHERE name = N'IX_ApplicationPermissions_1')
      DROP INDEX [dbo].[ApplicationPermissionsTable].[IX_ApplicationPermissions_1]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[ApplicationPermissionsTable]
      ALTER COLUMN [SqlUserOrRole] [nvarchar] (128) NOT NULL
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   CREATE INDEX [IX_ApplicationPermissions_1] ON [dbo].[ApplicationPermissionsTable] ([ApplicationId], [SqlUserOrRole], [NetSqlAzManFixedServerRole])
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.ApplicationPermissionsTable Table Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.ApplicationPermissionsTable Table'
END
GO

--
-- Script To Update dbo.AuthorizationAttributesTable Table In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.AuthorizationAttributesTable Table'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO


IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysindexes WHERE name = N'AuthorizationAttributes_AuhorizationId_AttributeKey_Unique_Index')
      DROP INDEX [dbo].[AuthorizationAttributesTable].[AuthorizationAttributes_AuhorizationId_AttributeKey_Unique_Index]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysindexes WHERE name = N'IX_AuthorizationAttributes')
      DROP INDEX [dbo].[AuthorizationAttributesTable].[IX_AuthorizationAttributes]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[AuthorizationAttributesTable]
      ALTER COLUMN [AttributeKey] [nvarchar] (255) NOT NULL
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[AuthorizationAttributesTable]
      ALTER COLUMN [AttributeValue] [nvarchar] (4000) NOT NULL
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   CREATE UNIQUE INDEX [AuthorizationAttributes_AuhorizationId_AttributeKey_Unique_Index] ON [dbo].[AuthorizationAttributesTable] ([AuthorizationId], [AttributeKey])
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   CREATE INDEX [IX_AuthorizationAttributes] ON [dbo].[AuthorizationAttributesTable] ([AuthorizationId], [AttributeKey])
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.AuthorizationAttributesTable Table Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.AuthorizationAttributesTable Table'
END
GO

--
-- Script To Update dbo.ItemsTable Table In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.ItemsTable Table'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO


IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysindexes WHERE name = N'Items_ApplicationId_Name_Unique_Index')
      DROP INDEX [dbo].[ItemsTable].[Items_ApplicationId_Name_Unique_Index]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysindexes WHERE name = N'IX_Items')
      DROP INDEX [dbo].[ItemsTable].[IX_Items]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[ItemsTable]
      ALTER COLUMN [Description] [nvarchar] (1024) NOT NULL
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[ItemsTable]
      ALTER COLUMN [Name] [nvarchar] (255) NOT NULL
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   CREATE UNIQUE INDEX [Items_ApplicationId_Name_Unique_Index] ON [dbo].[ItemsTable] ([Name], [ApplicationId])
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   CREATE INDEX [IX_Items] ON [dbo].[ItemsTable] ([ApplicationId], [Name])
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.ItemsTable Table Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.ItemsTable Table'
END
GO

--
-- Script To Update dbo.ItemAttributesTable Table In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.ItemAttributesTable Table'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO


IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysindexes WHERE name = N'ItemAttributes_AuhorizationId_AttributeKey_Unique_Index')
      DROP INDEX [dbo].[ItemAttributesTable].[ItemAttributes_AuhorizationId_AttributeKey_Unique_Index]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysindexes WHERE name = N'IX_ItemAttributes')
      DROP INDEX [dbo].[ItemAttributesTable].[IX_ItemAttributes]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[ItemAttributesTable]
      ALTER COLUMN [AttributeKey] [nvarchar] (255) NOT NULL
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[ItemAttributesTable]
      ALTER COLUMN [AttributeValue] [nvarchar] (4000) NOT NULL
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   CREATE UNIQUE INDEX [ItemAttributes_AuhorizationId_AttributeKey_Unique_Index] ON [dbo].[ItemAttributesTable] ([ItemId], [AttributeKey])
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   CREATE INDEX [IX_ItemAttributes] ON [dbo].[ItemAttributesTable] ([ItemId], [AttributeKey])
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.ItemAttributesTable Table Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.ItemAttributesTable Table'
END
GO

--
-- Script To Delete dbo.Log Table In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Deleting dbo.Log Table'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO


IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   DROP TABLE [dbo].[Log]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.Log Table Deleted Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Delete dbo.Log Table'
END
GO

--
-- Script To Create dbo.LogTable Table In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Creating dbo.LogTable Table'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO

CREATE TABLE [dbo].[LogTable] (
   [LogId] [int] IDENTITY (1, 1) NOT NULL,
   [LogDateTime] [datetime] NOT NULL,
   [WindowsIdentity] [nvarchar] (255) NOT NULL,
   [SqlIdentity] [nvarchar] (128) NULL CONSTRAINT [DF_Log_SqlIdentity] DEFAULT (suser_sname()),
   [MachineName] [nvarchar] (255) NOT NULL,
   [InstanceGuid] [uniqueidentifier] NOT NULL,
   [TransactionGuid] [uniqueidentifier] NULL,
   [OperationCounter] [int] NOT NULL,
   [ENSType] [nvarchar] (255) NOT NULL,
   [ENSDescription] [nvarchar] (4000) NOT NULL,
   [LogType] [char] (1) NOT NULL
)
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[LogTable] ADD CONSTRAINT [PK_Log] PRIMARY KEY NONCLUSTERED ([LogId])
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   CREATE INDEX [IX_Log] ON [dbo].[LogTable] ([WindowsIdentity])
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   CREATE INDEX [IX_Log_1] ON [dbo].[LogTable] ([SqlIdentity])
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   CREATE CLUSTERED INDEX [IX_Log_2] ON [dbo].[LogTable] ([LogDateTime] DESC, [InstanceGuid], [OperationCounter] DESC)
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[LogTable] ADD CONSTRAINT [CK_Log] CHECK ([LogType] = 'I' or [LogType] = 'W' or [LogType] = 'E')
GO
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   GRANT Insert ON [LogTable] TO [NetSqlAzMan_Readers]
GO
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   GRANT Select ON [LogTable] TO [NetSqlAzMan_Readers]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.LogTable Table Added Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Add dbo.LogTable Table'
END
GO

--
-- Script To Update dbo.Settings Table In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.Settings Table'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO


IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysobjects WHERE name = N'PK_Settings')
      ALTER TABLE [dbo].[Settings] DROP CONSTRAINT [PK_Settings]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Settings]
      ALTER COLUMN [SettingName] [nvarchar] (255) NOT NULL
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Settings]
      ALTER COLUMN [SettingValue] [nvarchar] (255) NOT NULL
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[Settings] ADD CONSTRAINT [PK_Settings] PRIMARY KEY CLUSTERED ([SettingName])
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.Settings Table Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.Settings Table'
END
GO

--
-- Script To Update dbo.StoreAttributesTable Table In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.StoreAttributesTable Table'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO


IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysindexes WHERE name = N'IX_StoreAttributes')
      DROP INDEX [dbo].[StoreAttributesTable].[IX_StoreAttributes]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysindexes WHERE name = N'StoreAttributes_AuhorizationId_AttributeKey_Unique_Index')
      DROP INDEX [dbo].[StoreAttributesTable].[StoreAttributes_AuhorizationId_AttributeKey_Unique_Index]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[StoreAttributesTable]
      ALTER COLUMN [AttributeKey] [nvarchar] (255) NOT NULL
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[StoreAttributesTable]
      ALTER COLUMN [AttributeValue] [nvarchar] (4000) NOT NULL
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   CREATE INDEX [IX_StoreAttributes] ON [dbo].[StoreAttributesTable] ([StoreId], [AttributeKey])
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   CREATE UNIQUE INDEX [StoreAttributes_AuhorizationId_AttributeKey_Unique_Index] ON [dbo].[StoreAttributesTable] ([StoreId], [AttributeKey])
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.StoreAttributesTable Table Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.StoreAttributesTable Table'
END
GO

--
-- Script To Update dbo.StoreGroupsTable Table In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.StoreGroupsTable Table'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO


IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysindexes WHERE name = N'StoreGroups_StoreId_Name_Unique_Index')
      DROP INDEX [dbo].[StoreGroupsTable].[StoreGroups_StoreId_Name_Unique_Index]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[StoreGroupsTable]
      ALTER COLUMN [Description] [nvarchar] (1024) NOT NULL
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[StoreGroupsTable]
      ALTER COLUMN [LDapQuery] [nvarchar] (4000) NULL
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[StoreGroupsTable]
      ALTER COLUMN [Name] [nvarchar] (255) NOT NULL
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   CREATE UNIQUE INDEX [StoreGroups_StoreId_Name_Unique_Index] ON [dbo].[StoreGroupsTable] ([StoreId], [Name])
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.StoreGroupsTable Table Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.StoreGroupsTable Table'
END
GO

--
-- Script To Update dbo.StorePermissionsTable Table In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.StorePermissionsTable Table'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO


IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   IF EXISTS (SELECT name FROM sysindexes WHERE name = N'IX_StorePermissions_1')
      DROP INDEX [dbo].[StorePermissionsTable].[IX_StorePermissions_1]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[StorePermissionsTable]
      ALTER COLUMN [SqlUserOrRole] [nvarchar] (128) NOT NULL
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   CREATE INDEX [IX_StorePermissions_1] ON [dbo].[StorePermissionsTable] ([StoreId], [SqlUserOrRole], [NetSqlAzManFixedServerRole])
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.StorePermissionsTable Table Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.StorePermissionsTable Table'
END
GO

--
-- Script To Update dbo.UsersDemo Table In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.UsersDemo Table'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO


IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[UsersDemo]
      ALTER COLUMN [FullName] [nvarchar] (255) NOT NULL
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[UsersDemo]
      ALTER COLUMN [OtherFields] [nvarchar] (255) NULL
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   ALTER TABLE [dbo].[UsersDemo]
      ALTER COLUMN [UserName] [nvarchar] (255) NOT NULL
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.UsersDemo Table Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.UsersDemo Table'
END
GO

--
-- Script To Update dbo.GetDBUsers Function In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.GetDBUsers Function'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO

REVOKE Select ON [GetDBUsers] TO [NetSqlAzMan_Readers]
GO

exec('/* 
    NetSqlAzMan GetDBUsers TABLE Function
    ************************************************************************
    Creation Date: August, 23  2006
    Purpose: Retrieve from a DB a list of custom Users (DBUserSid, DBUserName)
    Author: Andrea Ferendeles 
    Revision: 1.0.0.0
    Updated by: <put here your name>
    Parameters: 
	use: 
		1)     SELECT * FROM dbo.GetDBUsers(<storename>, <applicationname>, NULL, NULL)            -- to retrieve all DB Users
		2)     SELECT * FROM dbo.GetDBUsers(<storename>, <applicationname>, <customsid>, NULL)  -- to retrieve DB User with specified <customsid>
		3)     SELECT * FROM dbo.GetDBUsers(<storename>, <applicationname>, NULL, <username>)  -- to retrieve DB User with specified <username>

    Remarks: 
	- Update this Function with your CUSTOM CODE
	- Returned DBUserSid must be unique
	- Returned DBUserName must be unique
*/
ALTER FUNCTION [dbo].[GetDBUsers] (@StoreName nvarchar(255), @ApplicationName nvarchar(255), @DBUserSid VARBINARY(85) = NULL, @DBUserName nvarchar(255) = NULL)  
RETURNS TABLE 
AS  
RETURN 
	SELECT TOP 100 PERCENT CONVERT(VARBINARY(85), UserID) AS DBUserSid, UserName AS DBUserName FROM dbo.UsersDemo
	WHERE 
		(@DBUserSid IS NOT NULL AND CONVERT(VARBINARY(85), UserID) = @DBUserSid OR @DBUserSid  IS NULL)
		AND
		(@DBUserName IS NOT NULL AND UserName = @DBUserName OR @DBUserName IS NULL)
	ORDER BY UserName
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- THIS CODE IS JUST FOR AN EXAMPLE: comment this section and customize "INSERT HERE YOUR CUSTOM T-SQL" section below
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------')
GO
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   GRANT Select ON [GetDBUsers] TO [NetSqlAzMan_Readers]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.GetDBUsers Function Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.GetDBUsers Function'
END
GO

--
-- Script To Update dbo.GetNameFromSid Function In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.GetNameFromSid Function'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO


exec('-- =============================================
-- Author:		Andrea Ferendeles
-- Create date: 13/04/2006
-- Description:	Get Name From Sid
-- =============================================
ALTER FUNCTION [dbo].[GetNameFromSid] (@StoreName nvarchar(255), @ApplicationName nvarchar(255), @Sid varbinary(85), @SidWhereDefined tinyint)
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
SET @Name = (SELECT TOP 1 Name FROM dbo.ApplicationGroups() WHERE objectSid = @Sid)
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
END')
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.GetNameFromSid Function Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.GetNameFromSid Function'
END
GO

--
-- Script To Update dbo.NetSqlAzMan_DBVersion Function In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.NetSqlAzMan_DBVersion Function'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO

REVOKE Execute ON [NetSqlAzMan_DBVersion] TO [NetSqlAzMan_Readers]
GO

SET QUOTED_IDENTIFIER OFF
GO

SET ANSI_NULLS OFF
GO

exec('ALTER FUNCTION [dbo].[NetSqlAzMan_DBVersion] ()  
RETURNS nvarchar(200) AS  
BEGIN 
	return ''3.5.3.0''
END')
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   GRANT Execute ON [NetSqlAzMan_DBVersion] TO [NetSqlAzMan_Readers]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.NetSqlAzMan_DBVersion Function Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.NetSqlAzMan_DBVersion Function'
END
GO

--
-- Script To Update dbo.ApplicationAttributeInsert Procedure In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.ApplicationAttributeInsert Procedure'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO

REVOKE Execute ON [ApplicationAttributeInsert] TO [NetSqlAzMan_Managers]
GO

exec('ALTER PROCEDURE [dbo].[ApplicationAttributeInsert]
(
	@ApplicationId int,
	@AttributeKey nvarchar(255),
	@AttributeValue nvarchar(4000)
)
AS
IF EXISTS(SELECT ApplicationId FROM dbo.Applications() WHERE ApplicationId = @ApplicationId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
BEGIN
	INSERT INTO [dbo].[ApplicationAttributesTable] ([ApplicationId], [AttributeKey], [AttributeValue]) VALUES (@ApplicationId, @AttributeKey, @AttributeValue)
	RETURN SCOPE_IDENTITY()
END
ELSE
BEGIN
	RAISERROR (''Application Permission denied.'', 16, 1)
END')
GO
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   GRANT Execute ON [ApplicationAttributeInsert] TO [NetSqlAzMan_Managers]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.ApplicationAttributeInsert Procedure Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.ApplicationAttributeInsert Procedure'
END
GO

--
-- Script To Update dbo.ApplicationAttributeUpdate Procedure In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.ApplicationAttributeUpdate Procedure'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO

REVOKE Execute ON [ApplicationAttributeUpdate] TO [NetSqlAzMan_Managers]
GO

exec('ALTER PROCEDURE [dbo].[ApplicationAttributeUpdate]
(
	@ApplicationId int,
	@AttributeKey nvarchar(255),
	@AttributeValue nvarchar(4000),
	@Original_ApplicationAttributeId int
)
AS
IF EXISTS(SELECT ApplicationAttributeId FROM dbo.ApplicationAttributes() WHERE ApplicationAttributeId = @Original_ApplicationAttributeId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
	UPDATE [dbo].[ApplicationAttributesTable] SET [AttributeKey] = @AttributeKey, [AttributeValue] = @AttributeValue WHERE [ApplicationAttributeId] = @Original_ApplicationAttributeId AND [ApplicationId] = @ApplicationId
ELSE
	RAISERROR (''Applicaction Permission denied.'', 16, 1)')
GO
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   GRANT Execute ON [ApplicationAttributeUpdate] TO [NetSqlAzMan_Managers]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.ApplicationAttributeUpdate Procedure Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.ApplicationAttributeUpdate Procedure'
END
GO

--
-- Script To Update dbo.ApplicationGroupInsert Procedure In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.ApplicationGroupInsert Procedure'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO

REVOKE Execute ON [ApplicationGroupInsert] TO [NetSqlAzMan_Managers]
GO

exec('ALTER PROCEDURE [dbo].[ApplicationGroupInsert]
(
	@ApplicationId int,
	@objectSid varbinary(85),
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@LDapQuery nvarchar(4000),
	@GroupType tinyint
)
AS
IF EXISTS(SELECT ApplicationId FROM dbo.Applications() WHERE ApplicationId = @ApplicationId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
BEGIN
	INSERT INTO [dbo].[ApplicationGroupsTable] ([ApplicationId], [objectSid], [Name], [Description], [LDapQuery], [GroupType]) VALUES (@ApplicationId, @objectSid, @Name, @Description, @LDapQuery, @GroupType)
	RETURN SCOPE_IDENTITY()
END
ELSE	
	RAISERROR (''Application permission denied.'', 16, 1)')
GO
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   GRANT Execute ON [ApplicationGroupInsert] TO [NetSqlAzMan_Managers]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.ApplicationGroupInsert Procedure Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.ApplicationGroupInsert Procedure'
END
GO

--
-- Script To Update dbo.ApplicationGroupUpdate Procedure In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.ApplicationGroupUpdate Procedure'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO

REVOKE Execute ON [ApplicationGroupUpdate] TO [NetSqlAzMan_Managers]
GO

exec('ALTER PROCEDURE [dbo].[ApplicationGroupUpdate]
(
	@objectSid varbinary(85),
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@LDapQuery nvarchar(4000),
	@GroupType tinyint,
	@Original_ApplicationGroupId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ApplicationGroupId FROM dbo.ApplicationGroups() WHERE ApplicationGroupId = @Original_ApplicationGroupId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
	UPDATE [dbo].[ApplicationGroupsTable] SET [objectSid] = @objectSid, [Name] = @Name, [Description] = @Description, [LDapQuery] = @LDapQuery, [GroupType] = @GroupType WHERE [ApplicationGroupId] = @Original_ApplicationGroupId AND [ApplicationId] = @ApplicationId
ELSE
	RAISERROR (''Application permission denied.'', 16, 1)')
GO
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   GRANT Execute ON [ApplicationGroupUpdate] TO [NetSqlAzMan_Managers]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.ApplicationGroupUpdate Procedure Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.ApplicationGroupUpdate Procedure'
END
GO

--
-- Script To Update dbo.ApplicationInsert Procedure In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.ApplicationInsert Procedure'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO

REVOKE Execute ON [ApplicationInsert] TO [NetSqlAzMan_Managers]
GO

SET QUOTED_IDENTIFIER OFF
GO

SET ANSI_NULLS OFF
GO

exec('ALTER PROCEDURE [dbo].[ApplicationInsert]
(
	@StoreId int,
	@Name nvarchar(255),
	@Description nvarchar(1024)
)
AS
IF EXISTS(SELECT StoreId FROM dbo.Stores() WHERE StoreId = @StoreId) AND dbo.CheckStorePermissions(@StoreId, 2) = 1
BEGIN
	INSERT INTO [dbo].[ApplicationsTable] ([StoreId], [Name], [Description]) VALUES (@StoreId, @Name, @Description);
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR (''Store permission denied.'', 16, 1)')
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   GRANT Execute ON [ApplicationInsert] TO [NetSqlAzMan_Managers]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.ApplicationInsert Procedure Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.ApplicationInsert Procedure'
END
GO

--
-- Script To Update dbo.ApplicationPermissionInsert Procedure In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.ApplicationPermissionInsert Procedure'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO

REVOKE Execute ON [ApplicationPermissionInsert] TO [NetSqlAzMan_Managers]
GO

SET QUOTED_IDENTIFIER OFF
GO

SET ANSI_NULLS OFF
GO

exec('ALTER PROCEDURE [dbo].[ApplicationPermissionInsert]
(
	@ApplicationId int,
	@SqlUserOrRole nvarchar(128),
	@IsSqlRole bit,
	@NetSqlAzManFixedServerRole tinyint
)
AS
IF EXISTS(SELECT ApplicationId FROM dbo.Applications() WHERE ApplicationId = @ApplicationId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
BEGIN
	INSERT INTO dbo.ApplicationPermissionsTable (ApplicationId, SqlUserOrRole, IsSqlRole, NetSqlAzManFixedServerRole) VALUES (@ApplicationId, @SqlUserOrRole, @IsSqlRole, @NetSqlAzManFixedServerRole)
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR (''Application permission denied.'', 16, 1)')
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   GRANT Execute ON [ApplicationPermissionInsert] TO [NetSqlAzMan_Managers]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.ApplicationPermissionInsert Procedure Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.ApplicationPermissionInsert Procedure'
END
GO

--
-- Script To Update dbo.ApplicationUpdate Procedure In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.ApplicationUpdate Procedure'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO

REVOKE Execute ON [ApplicationUpdate] TO [NetSqlAzMan_Managers]
GO

SET QUOTED_IDENTIFIER OFF
GO

SET ANSI_NULLS OFF
GO

exec('ALTER PROCEDURE [dbo].[ApplicationUpdate]
(
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@Original_ApplicationId int
)
AS
IF EXISTS(SELECT ApplicationId FROM dbo.Applications() WHERE ApplicationId = @Original_ApplicationId) AND dbo.CheckApplicationPermissions(@Original_ApplicationId, 2) = 1
	UPDATE [dbo].[ApplicationsTable] SET [Name] = @Name, [Description] = @Description WHERE [ApplicationId] = @Original_ApplicationId
ELSE
	RAISERROR (''Application permission denied.'', 16, 1)')
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   GRANT Execute ON [ApplicationUpdate] TO [NetSqlAzMan_Managers]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.ApplicationUpdate Procedure Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.ApplicationUpdate Procedure'
END
GO

--
-- Script To Update dbo.AuthorizationAttributeInsert Procedure In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.AuthorizationAttributeInsert Procedure'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO

REVOKE Execute ON [AuthorizationAttributeInsert] TO [NetSqlAzMan_Users]
GO

exec('ALTER PROCEDURE [dbo].[AuthorizationAttributeInsert]
(
	@AuthorizationId int,
	@AttributeKey nvarchar(255),
	@AttributeValue nvarchar(4000),
	@ApplicationId int
)
AS
IF EXISTS(SELECT AuthorizationId FROM dbo.Authorizations() WHERE AuthorizationId = @AuthorizationId) AND dbo.CheckApplicationPermissions(@ApplicationId, 1) = 1
BEGIN
	INSERT INTO [dbo].[AuthorizationAttributesTable] ([AuthorizationId], [AttributeKey], [AttributeValue]) VALUES (@AuthorizationId, @AttributeKey, @AttributeValue)
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR (''Application permission denied.'', 16, 1)')
GO
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   GRANT Execute ON [AuthorizationAttributeInsert] TO [NetSqlAzMan_Users]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.AuthorizationAttributeInsert Procedure Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.AuthorizationAttributeInsert Procedure'
END
GO

--
-- Script To Update dbo.AuthorizationAttributeUpdate Procedure In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.AuthorizationAttributeUpdate Procedure'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO

REVOKE Execute ON [AuthorizationAttributeUpdate] TO [NetSqlAzMan_Users]
GO

exec('ALTER PROCEDURE [dbo].[AuthorizationAttributeUpdate]
(
	@AttributeKey nvarchar(255),
	@AttributeValue nvarchar(4000),
	@Original_AuthorizationAttributeId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT AuthorizationAttributeId FROM dbo.AuthorizationAttributes() WHERE AuthorizationAttributeId = @Original_AuthorizationAttributeId) AND dbo.CheckApplicationPermissions(@ApplicationId, 1) = 1
	UPDATE [dbo].[AuthorizationAttributesTable] SET [AttributeKey] = @AttributeKey, [AttributeValue] = @AttributeValue WHERE [AuthorizationAttributeId] = @Original_AuthorizationAttributeId
ELSE
	RAISERROR (''Application permission denied.'', 16 ,1)')
GO
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   GRANT Execute ON [AuthorizationAttributeUpdate] TO [NetSqlAzMan_Users]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.AuthorizationAttributeUpdate Procedure Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.AuthorizationAttributeUpdate Procedure'
END
GO

--
-- Script To Update dbo.BuildUserPermissionCache Procedure In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.BuildUserPermissionCache Procedure'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO

REVOKE Execute ON [BuildUserPermissionCache] TO [NetSqlAzMan_Readers]
GO

SET QUOTED_IDENTIFIER OFF
GO

SET ANSI_NULLS OFF
GO

exec('ALTER PROCEDURE dbo.BuildUserPermissionCache(@STORENAME nvarchar(255), @APPLICATIONNAME nvarchar(255))
AS 
-- Hierarchy
SET NOCOUNT ON
SELECT     Items.Name AS ItemName, Items_1.Name AS ParentItemName
FROM         dbo.Items() Items_1 INNER JOIN
                      dbo.ItemsHierarchy() ItemsHierarchy ON Items_1.ItemId = ItemsHierarchy.MemberOfItemId RIGHT OUTER JOIN
                      dbo.Applications() Applications INNER JOIN
                      dbo.Stores() Stores ON Applications.StoreId = Stores.StoreId INNER JOIN
                      dbo.Items() Items ON Applications.ApplicationId = Items.ApplicationId ON ItemsHierarchy.ItemId = Items.ItemId
WHERE     (Stores.Name = @STORENAME) AND (Applications.Name = @APPLICATIONNAME)

-- Item Authorizations
SELECT DISTINCT Items.Name AS ItemName, Authorizations.ValidFrom, Authorizations.ValidTo
FROM         dbo.Authorizations() Authorizations INNER JOIN
                      dbo.Items() Items ON Authorizations.ItemId = Items.ItemId INNER JOIN
                      dbo.Stores() Stores INNER JOIN
                      dbo.Applications() Applications ON Stores.StoreId = Applications.StoreId ON Items.ApplicationId = Applications.ApplicationId
WHERE     (Authorizations.AuthorizationType <> 0) AND (Stores.Name = @STORENAME) AND (Applications.Name = @APPLICATIONNAME)
UNION
SELECT DISTINCT Items.Name AS ItemName, NULL ValidFrom, NULL ValidTo
FROM         dbo.Items() Items INNER JOIN
                      dbo.Stores() Stores INNER JOIN
                      dbo.Applications() Applications ON Stores.StoreId = Applications.StoreId ON Items.ApplicationId = Applications.ApplicationId
WHERE     (Stores.Name = @STORENAME) AND (Applications.Name = @APPLICATIONNAME) AND Items.BizRuleId IS NOT NULL
SET NOCOUNT OFF')
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   GRANT Execute ON [BuildUserPermissionCache] TO [NetSqlAzMan_Readers]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.BuildUserPermissionCache Procedure Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.BuildUserPermissionCache Procedure'
END
GO

--
-- Script To Update dbo.ExecuteLDAPQuery Procedure In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.ExecuteLDAPQuery Procedure'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO


exec('ALTER PROCEDURE [dbo].[ExecuteLDAPQuery](@LDAPPATH NVARCHAR(4000), @LDAPQUERY NVARCHAR(4000), @members_cur CURSOR VARYING OUTPUT)
AS
-- REMEMBER !!!
-- BEFORE executing ExecuteLDAPQuery procedure ... a Linked Server named ''ADSI'' must be added:
-- --sp_addlinkedserver ''ADSI'', ''Active Directory Service Interfaces'', ''ADSDSOObject'', ''adsdatasource''
CREATE TABLE #temp (objectSid VARBINARY(85))
IF @LDAPQUERY IS NULL OR RTRIM(LTRIM(@LDAPQUERY))='''' OR @LDAPPATH IS NULL OR RTRIM(LTRIM(@LDAPPATH))=''''
BEGIN
SET @members_cur = CURSOR STATIC FORWARD_ONLY FOR SELECT * FROM #temp
OPEN @members_cur
DROP TABLE #temp
RETURN
END
SET @LDAPPATH = REPLACE(@LDAPPATH, N'''''''', N'''''''''''')
SET @LDAPQUERY = REPLACE(@LDAPQUERY, N'''''''', N'''''''''''')
DECLARE @QUERY nvarchar(4000)
SET @QUERY = CHAR(39) + ''<'' + ''LDAP://''+ @LDAPPATH + ''>;(&(!(objectClass=computer))(&(|(objectClass=user)(objectClass=group)))'' + @LDAPQUERY + '');objectSid;subtree'' + CHAR(39) 
DECLARE @OPENQUERY nvarchar(4000)
SET @OPENQUERY = ''SELECT * FROM OPENQUERY(ADSI, '' + @QUERY + '')''
INSERT INTO #temp EXEC (@OPENQUERY)
SET @members_cur = CURSOR STATIC FORWARD_ONLY FOR SELECT * FROM #temp
OPEN @members_cur
DROP TABLE #temp')
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.ExecuteLDAPQuery Procedure Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.ExecuteLDAPQuery Procedure'
END
GO

--
-- Script To Update dbo.GetStoreGroupSidMembers Procedure In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.GetStoreGroupSidMembers Procedure'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO


exec('ALTER PROCEDURE [dbo].[GetStoreGroupSidMembers](@ISMEMBER BIT, @GROUPOBJECTSID VARBINARY(85), @NETSQLAZMANMODE bit, @LDAPPATH nvarchar(4000), @member_cur CURSOR VARYING OUTPUT)
AS
DECLARE @RESULT TABLE (objectSid VARBINARY(85))
DECLARE @GROUPID INT
DECLARE @GROUPTYPE TINYINT
DECLARE @LDAPQUERY nvarchar(4000)
DECLARE @sub_members_cur CURSOR
DECLARE @OBJECTSID VARBINARY(85)
SELECT @GROUPID = StoreGroupId, @GROUPTYPE = GroupType, @LDAPQUERY = LDapQuery FROM dbo.StoreGroups() WHERE objectSid = @GROUPOBJECTSID
IF @GROUPTYPE = 0 -- BASIC
BEGIN
	--memo: WhereDefined can be:0 - Store; 1 - Application; 2 - LDAP; 3 - Local; 4 - Database
	-- Windows SIDs
	INSERT INTO @RESULT (objectSid) 
	SELECT objectSid 
	FROM dbo.StoreGroupMembersTable
	WHERE 
	StoreGroupId = @GROUPID AND IsMember = @ISMEMBER AND
	((@NETSQLAZMANMODE = 0 AND (WhereDefined = 2 OR WhereDefined = 4)) OR (@NETSQLAZMANMODE = 1 AND WhereDefined BETWEEN 2 AND 4))
	-- Store Groups Members
	DECLARE @MemberObjectSid VARBINARY(85)
	DECLARE @MemberType bit
	DECLARE @NotMemberType bit
	DECLARE nested_Store_groups_cur CURSOR LOCAL FAST_FORWARD FOR
		SELECT objectSid, IsMember FROM dbo.StoreGroupMembersTable WHERE StoreGroupId = @GROUPID AND WhereDefined = 0
	
	OPEN nested_Store_groups_cur
	FETCH NEXT FROM nested_Store_groups_cur INTO @MemberObjectSid, @MemberType
	WHILE @@FETCH_STATUS = 0
	BEGIN
	        -- recursive call
		IF @ISMEMBER = 1
		BEGIN
			IF @MemberType = 0 
				SET @NotMemberType = 0
			ELSE
				SET @NotMemberType = 1
		END
		ELSE
		BEGIN
			IF @MemberType = 0 
				SET @NotMemberType = 1
			ELSE
				SET @NotMemberType = 0
		END
		EXEC dbo.GetStoreGroupSidMembers @NotMemberType, @MemberObjectSid, @NETSQLAZMANMODE, @LDAPPATH, @sub_members_cur OUTPUT
		FETCH NEXT FROM @sub_members_cur INTO @OBJECTSID
		WHILE @@FETCH_STATUS=0
		BEGIN
			INSERT INTO @RESULT VALUES (@OBJECTSID)
			FETCH NEXT FROM @sub_members_cur INTO @OBJECTSID
		END		
		CLOSE @sub_members_cur
		DEALLOCATE @sub_members_cur	

		FETCH NEXT FROM nested_Store_groups_cur INTO @MemberObjectSid, @MemberType
	END
	CLOSE nested_Store_groups_cur
	DEALLOCATE nested_Store_groups_cur
END
ELSE IF @GROUPTYPE = 1 AND @ISMEMBER = 1 -- LDAP QUERY
BEGIN
	EXEC dbo.ExecuteLDAPQuery @LDAPPATH, @LDAPQUERY, @sub_members_cur OUTPUT
	FETCH NEXT FROM @sub_members_cur INTO @OBJECTSID
	WHILE @@FETCH_STATUS=0
	BEGIN
		INSERT INTO @RESULT (objectSid) VALUES (@OBJECTSID)
		FETCH NEXT FROM @sub_members_cur INTO @OBJECTSID
	END
	CLOSE @sub_members_cur
	DEALLOCATE @sub_members_cur
END
SET @member_cur = CURSOR STATIC FORWARD_ONLY FOR SELECT * FROM @RESULT
OPEN @member_cur')
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.GetStoreGroupSidMembers Procedure Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.GetStoreGroupSidMembers Procedure'
END
GO

--
-- Script To Update dbo.GetApplicationGroupSidMembers Procedure In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.GetApplicationGroupSidMembers Procedure'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO


exec('ALTER PROCEDURE [dbo].[GetApplicationGroupSidMembers](@ISMEMBER BIT, @GROUPOBJECTSID VARBINARY(85), @NETSQLAZMANMODE bit, @LDAPPATH nvarchar(4000), @member_cur CURSOR VARYING OUTPUT)
AS
DECLARE @RESULT TABLE (objectSid VARBINARY(85))
DECLARE @GROUPID INT
DECLARE @GROUPTYPE TINYINT
DECLARE @LDAPQUERY nvarchar(4000)
DECLARE @sub_members_cur CURSOR
DECLARE @OBJECTSID VARBINARY(85)
SELECT @GROUPID = ApplicationGroupId, @GROUPTYPE = GroupType, @LDAPQUERY = LDapQuery FROM ApplicationGroupsTable WHERE objectSid = @GROUPOBJECTSID
IF @GROUPTYPE = 0 -- BASIC
BEGIN
	--memo: WhereDefined can be:0 - Store; 1 - Application; 2 - LDAP; 3 - Local; 4 - Database
	-- Windows SIDs
	INSERT INTO @RESULT (objectSid) 
	SELECT objectSid 
	FROM dbo.ApplicationGroupMembersTable
	WHERE 
	ApplicationGroupId = @GROUPID AND IsMember = @ISMEMBER AND
	((@NETSQLAZMANMODE = 0 AND (WhereDefined = 2 OR WhereDefined = 4)) OR (@NETSQLAZMANMODE = 1 AND WhereDefined BETWEEN 2 AND 4))
	-- Store Groups Members
	DECLARE @MemberObjectSid VARBINARY(85)
	DECLARE @MemberType bit
	DECLARE @NotMemberType bit
	DECLARE nested_Store_groups_cur CURSOR LOCAL FAST_FORWARD FOR
		SELECT objectSid, IsMember FROM dbo.ApplicationGroupMembersTable WHERE ApplicationGroupId = @GROUPID AND WhereDefined = 0
	
	OPEN nested_Store_groups_cur
	FETCH NEXT FROM nested_Store_groups_cur INTO @MemberObjectSid, @MemberType
	WHILE @@FETCH_STATUS = 0
	BEGIN
	        -- recursive call
		IF @ISMEMBER = 1
		BEGIN
			IF @MemberType = 0 
				SET @NotMemberType = 0
			ELSE
				SET @NotMemberType = 1
		END
		ELSE
		BEGIN
			IF @MemberType = 0 
				SET @NotMemberType = 1
			ELSE
				SET @NotMemberType = 0
		END
		EXEC dbo.GetStoreGroupSidMembers @NotMemberType, @MemberObjectSid, @NETSQLAZMANMODE, @LDAPPATH, @sub_members_cur OUTPUT
		FETCH NEXT FROM @sub_members_cur INTO @OBJECTSID
		WHILE @@FETCH_STATUS=0
		BEGIN
			INSERT INTO @RESULT VALUES (@OBJECTSID)
			FETCH NEXT FROM @sub_members_cur INTO @OBJECTSID
		END		
		CLOSE @sub_members_cur
		DEALLOCATE @sub_members_cur			

		FETCH NEXT FROM nested_Store_groups_cur INTO @MemberObjectSid, @MemberType
	END
	CLOSE nested_Store_groups_cur
	DEALLOCATE nested_Store_groups_cur
	
	-- Application Groups Members
	DECLARE nested_Application_groups_cur CURSOR LOCAL FAST_FORWARD FOR
		SELECT objectSid, IsMember FROM dbo.ApplicationGroupMembersTable WHERE ApplicationGroupId = @GROUPID AND WhereDefined = 1
	
	OPEN nested_Application_groups_cur
	FETCH NEXT FROM nested_Application_groups_cur INTO @MemberObjectSid, @MemberType
	WHILE @@FETCH_STATUS = 0
	BEGIN
	        -- recursive call
		IF @ISMEMBER = 1
		BEGIN
			IF @MemberType = 0 
				SET @NotMemberType = 0
			ELSE
				SET @NotMemberType = 1
		END
		ELSE
		BEGIN
			IF @MemberType = 0 
				SET @NotMemberType = 1
			ELSE
				SET @NotMemberType = 0
		END
		EXEC dbo.GetApplicationGroupSidMembers @NotMemberType, @MemberObjectSid, @NETSQLAZMANMODE, @LDAPPATH, @sub_members_cur OUTPUT
		FETCH NEXT FROM @sub_members_cur INTO @OBJECTSID
		WHILE @@FETCH_STATUS=0
		BEGIN
			INSERT INTO @RESULT VALUES (@OBJECTSID)
			FETCH NEXT FROM @sub_members_cur INTO @OBJECTSID
		END		
		CLOSE @sub_members_cur
		DEALLOCATE @sub_members_cur	

		FETCH NEXT FROM nested_Application_groups_cur INTO @MemberObjectSid, @MemberType
	END
	CLOSE nested_Application_groups_cur
	DEALLOCATE nested_Application_groups_cur
	END
ELSE IF @GROUPTYPE = 1 AND @ISMEMBER = 1 -- LDAP QUERY
BEGIN
	EXEC dbo.ExecuteLDAPQuery @LDAPPATH, @LDAPQUERY, @sub_members_cur OUTPUT
	FETCH NEXT FROM @sub_members_cur INTO @OBJECTSID
	WHILE @@FETCH_STATUS=0
	BEGIN
		INSERT INTO @RESULT (objectSid) VALUES (@OBJECTSID)
		FETCH NEXT FROM @sub_members_cur INTO @OBJECTSID
	END
	CLOSE @sub_members_cur
	DEALLOCATE @sub_members_cur
END
SET @member_cur = CURSOR STATIC FORWARD_ONLY FOR SELECT * FROM @RESULT
OPEN @member_cur')
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.GetApplicationGroupSidMembers Procedure Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.GetApplicationGroupSidMembers Procedure'
END
GO

--
-- Script To Update dbo.CheckAccess Procedure In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.CheckAccess Procedure'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO


exec('ALTER PROCEDURE [dbo].[CheckAccess] (@ITEMID INT, @USERSID VARBINARY(85), @VALIDFOR DATETIME, @LDAPPATH nvarchar(4000), @AUTHORIZATION_TYPE TINYINT OUTPUT, @NETSQLAZMANMODE BIT, @RETRIEVEATTRIBUTES BIT) 
AS
---------------------------------------------------
-- VARIABLES DECLARATION
-- 0 - Neutral; 1 - Allow; 2 - Deny; 3 - AllowWithDelegation
SET NOCOUNT ON
DECLARE @PARENTITEMID INT
DECLARE @PKID INT
DECLARE @PARENTRESULT TINYINT
DECLARE @APP VARBINARY(85)
DECLARE @members_cur CURSOR
DECLARE @OBJECTSID VARBINARY(85)
DECLARE @ITEM_AUTHORIZATION_TYPE TINYINT
---------------------------------------------------
-- INITIALIZE VARIABLES
SET @ITEM_AUTHORIZATION_TYPE = 0 -- Neutral
SET @AUTHORIZATION_TYPE = 0 -- Neutral
------------------------------------------------------
-- CHECK ACCESS ON PARENTS
-- Get Items Where Item is A Member
DECLARE ItemsWhereIAmAMember_cur CURSOR LOCAL FAST_FORWARD READ_ONLY FOR SELECT MemberOfItemId FROM dbo.ItemsHierarchyTable WHERE ItemId = @ITEMID
OPEN ItemsWhereIAmAMember_cur
FETCH NEXT FROM ItemsWhereIAmAMember_cur INTO @PARENTITEMID
WHILE @@FETCH_STATUS = 0
BEGIN
	-- Recursive Call
	EXEC dbo.CheckAccess @PARENTITEMID, @USERSID, @VALIDFOR, @LDAPPATH, @PARENTRESULT OUTPUT, @NETSQLAZMANMODE, @RETRIEVEATTRIBUTES
	SELECT @AUTHORIZATION_TYPE = dbo.MergeAuthorizations(@AUTHORIZATION_TYPE, @PARENTRESULT)
	FETCH NEXT FROM ItemsWhereIAmAMember_cur INTO @PARENTITEMID
END
CLOSE ItemsWhereIAmAMember_cur
DEALLOCATE ItemsWhereIAmAMember_cur

IF @AUTHORIZATION_TYPE = 3 
BEGIN
	SET @AUTHORIZATION_TYPE = 1 -- AllowWithDelegation becomes Just Allow (if comes from parents)
END
---------------------------------------------
-- GET ITEM ATTRIBUTES
---------------------------------------------
IF @RETRIEVEATTRIBUTES = 1
	INSERT INTO #ATTRIBUTES_TABLE SELECT AttributeKey, AttributeValue, @ITEMID FROM dbo.ItemAttributesTable WHERE ItemId = @ITEMID
---------------------------------------------
-- CHECK ACCESS ON ITEM
-- AuthorizationType can be:  0 - Neutral; 1 - Allow; 2 - Deny; 3 - AllowWithDelegation
-- objectSidWhereDefined can be:0 - Store; 1 - Application; 2 - LDAP; 3 - Local; 4 - Database
DECLARE @PARTIAL_RESULT TINYINT
--CHECK ACCESS FOR USER AUTHORIZATIONS
DECLARE checkaccessonitem_cur CURSOR  LOCAL FAST_FORWARD READ_ONLY FOR 
	SELECT AuthorizationType, AuthorizationID
	FROM dbo.AuthorizationsTable WHERE 
	ItemId = @ITEMID AND
	objectSid = @USERSID AND
	(ValidFrom IS NULL AND ValidTo IS NULL OR
	@VALIDFOR >= ValidFrom  AND ValidTo IS NULL OR
	@VALIDFOR <= ValidTo AND ValidFrom IS NULL OR
	@VALIDFOR BETWEEN ValidFrom AND ValidTo) AND
        AuthorizationType<>0 AND
	((@NETSQLAZMANMODE = 0 AND (objectSidWhereDefined=2 OR objectSidWhereDefined=4)) OR (@NETSQLAZMANMODE = 1 AND objectSidWhereDefined BETWEEN 2 AND 4)) -- if Mode = Administrator SKIP CHECK for local Authorizations

OPEN checkaccessonitem_cur
FETCH NEXT FROM checkaccessonitem_cur INTO @PARTIAL_RESULT, @PKID
WHILE @@FETCH_STATUS = 0
BEGIN
	--CHECK FOR DENY
	IF @PARTIAL_RESULT IS NOT NULL
	BEGIN
		SELECT @AUTHORIZATION_TYPE = dbo.MergeAuthorizations(@AUTHORIZATION_TYPE, @PARTIAL_RESULT)
		SELECT @ITEM_AUTHORIZATION_TYPE  = dbo.MergeAuthorizations(@ITEM_AUTHORIZATION_TYPE, @PARTIAL_RESULT)
		IF @RETRIEVEATTRIBUTES = 1 
			INSERT INTO #ATTRIBUTES_TABLE SELECT AttributeKey, AttributeValue, NULL FROM dbo.AuthorizationAttributesTable WHERE AuthorizationID = @PKID
	END
	ELSE
	BEGIN
		SET @PARTIAL_RESULT = 0 -- NEUTRAL
	END
	FETCH NEXT FROM checkaccessonitem_cur INTO @PARTIAL_RESULT, @PKID
END

CLOSE checkaccessonitem_cur
DEALLOCATE checkaccessonitem_cur

--CHECK ACCESS FOR USER GROUPS AUTHORIZATIONS
DECLARE usergroupsauthz_cur CURSOR LOCAL FAST_FORWARD READ_ONLY FOR 
	SELECT AuthorizationType, AuthorizationID
	FROM dbo.AuthorizationsTable Authorizations INNER JOIN #USERGROUPS usergroups
	ON Authorizations.objectSid = usergroups.objectSid WHERE 
	ItemId = @ITEMID AND
	(ValidFrom IS NULL AND ValidTo IS NULL OR
	@VALIDFOR >= ValidFrom  AND ValidTo IS NULL OR
	@VALIDFOR <= ValidTo AND ValidFrom IS NULL OR
	@VALIDFOR BETWEEN ValidFrom AND ValidTo) AND
        AuthorizationType<>0 AND
	((@NETSQLAZMANMODE = 0 AND (objectSidWhereDefined=2 OR objectSidWhereDefined=4)) OR (@NETSQLAZMANMODE = 1 AND objectSidWhereDefined BETWEEN 2 AND 4)) -- if Mode = Administrator SKIP CHECK for local Authorizations

OPEN usergroupsauthz_cur
FETCH NEXT FROM usergroupsauthz_cur INTO @PARTIAL_RESULT, @PKID
WHILE @@FETCH_STATUS = 0
BEGIN
	IF @PARTIAL_RESULT IS NOT NULL
	BEGIN
		SELECT @AUTHORIZATION_TYPE = dbo.MergeAuthorizations(@AUTHORIZATION_TYPE, @PARTIAL_RESULT)
		SELECT @ITEM_AUTHORIZATION_TYPE = dbo.MergeAuthorizations(@ITEM_AUTHORIZATION_TYPE, @PARTIAL_RESULT)
		IF @RETRIEVEATTRIBUTES = 1
			INSERT INTO #ATTRIBUTES_TABLE SELECT AttributeKey, AttributeValue, NULL FROM dbo.AuthorizationAttributesTable WHERE AuthorizationID = @PKID
	END
	ELSE
	BEGIN
		SET @PARTIAL_RESULT = 0 -- NEUTRAL
	END
	FETCH NEXT FROM usergroupsauthz_cur INTO @PARTIAL_RESULT, @PKID
END

CLOSE usergroupsauthz_cur
DEALLOCATE usergroupsauthz_cur

--CHECK ACCESS FOR STORE/APPLICATION GROUPS AUTHORIZATIONS
DECLARE @GROUPOBJECTSID VARBINARY(85)
DECLARE @GROUPWHEREDEFINED TINYINT
DECLARE @GROUPSIDMEMBERS table (objectSid VARBINARY(85))
DECLARE @ISMEMBER BIT
SET @ISMEMBER = 1
DECLARE groups_authorizations_cur CURSOR LOCAL FAST_FORWARD READ_ONLY 
FOR 	SELECT objectSid, objectSidWhereDefined, AuthorizationType, AuthorizationID FROM dbo.AuthorizationsTable
	WHERE ItemId = @ITEMID AND objectSidWhereDefined BETWEEN 0 AND 1 AND
        AuthorizationType<>0 AND
	(ValidFrom IS NULL AND ValidTo IS NULL OR
	@VALIDFOR >= ValidFrom  AND ValidTo IS NULL OR
	@VALIDFOR <= ValidTo AND ValidFrom IS NULL OR
	@VALIDFOR BETWEEN ValidFrom AND ValidTo)

OPEN groups_authorizations_cur
FETCH NEXT FROM groups_authorizations_cur INTO @GROUPOBJECTSID, @GROUPWHEREDEFINED, @PARTIAL_RESULT, @PKID
WHILE @@FETCH_STATUS=0
BEGIN
SET @ISMEMBER = 1
--check if user is a non-member
IF @GROUPWHEREDEFINED = 0 -- store group members
BEGIN
--store groups members of type ''non-member''
	DELETE FROM @GROUPSIDMEMBERS

	EXEC dbo.GetStoreGroupSidMembers 0, @GROUPOBJECTSID, @NETSQLAZMANMODE, @LDAPPATH, @members_cur OUTPUT -- non-members
	FETCH NEXT FROM @members_cur INTO @OBJECTSID
	WHILE @@FETCH_STATUS=0
	BEGIN
		INSERT INTO @GROUPSIDMEMBERS VALUES (@OBJECTSID)
		FETCH NEXT FROM @members_cur INTO @OBJECTSID
	END
	CLOSE @members_cur
	DEALLOCATE @members_cur

	IF EXISTS(SELECT * FROM @GROUPSIDMEMBERS WHERE objectSid = @USERSID) OR
	     EXISTS(SELECT * FROM @GROUPSIDMEMBERS groupsidmembers INNER JOIN #USERGROUPS usergroups ON groupsidmembers.objectSid = usergroups.objectSid)
	BEGIN
	-- user is a non-member
	SET @ISMEMBER = 0
	END
	IF @ISMEMBER = 1
	BEGIN
		DELETE FROM @GROUPSIDMEMBERS

		EXEC dbo.GetStoreGroupSidMembers 1, @GROUPOBJECTSID, @NETSQLAZMANMODE, @LDAPPATH, @members_cur OUTPUT -- members
		FETCH NEXT FROM @members_cur INTO @OBJECTSID
		WHILE @@FETCH_STATUS=0
		BEGIN
			INSERT INTO @GROUPSIDMEMBERS VALUES (@OBJECTSID)
			FETCH NEXT FROM @members_cur INTO @OBJECTSID
		END
		CLOSE @members_cur
		DEALLOCATE @members_cur

		IF EXISTS (SELECT * FROM @GROUPSIDMEMBERS WHERE objectSid = @USERSID) OR
		     EXISTS (SELECT * FROM @GROUPSIDMEMBERS groupsidmembers INNER JOIN #USERGROUPS usergroups ON groupsidmembers.objectSid = usergroups.ObjectSId)
		BEGIN
		-- user is a member
		SET @ISMEMBER = 1
		END
		ELSE
		BEGIN
		-- user is not present
		SET @ISMEMBER = 0
		END
	END
	-- if a member ... get authorization
	IF @ISMEMBER = 1
	BEGIN
		SET @AUTHORIZATION_TYPE = (SELECT dbo.MergeAuthorizations(@AUTHORIZATION_TYPE, @PARTIAL_RESULT))
		SET @ITEM_AUTHORIZATION_TYPE = (SELECT dbo.MergeAuthorizations(@ITEM_AUTHORIZATION_TYPE, @PARTIAL_RESULT))
		IF @PKID IS NOT NULL AND @RETRIEVEATTRIBUTES = 1
			INSERT INTO #ATTRIBUTES_TABLE SELECT AttributeKey, AttributeValue, NULL FROM dbo.AuthorizationAttributesTable WHERE AuthorizationID = @PKID
	END
END
	ELSE
IF @GROUPWHEREDEFINED = 1 -- application group members
BEGIN
	--application groups members of type ''non-member''
	DELETE FROM @GROUPSIDMEMBERS

	EXEC dbo.GetApplicationGroupSidMembers 0, @GROUPOBJECTSID, @NETSQLAZMANMODE, @LDAPPATH, @members_cur OUTPUT -- non-members
	FETCH NEXT FROM @members_cur INTO @OBJECTSID
	WHILE @@FETCH_STATUS=0
	BEGIN
		INSERT INTO @GROUPSIDMEMBERS VALUES (@OBJECTSID)
		FETCH NEXT FROM @members_cur INTO @OBJECTSID
	END
	CLOSE @members_cur
	DEALLOCATE @members_cur

	IF EXISTS(SELECT * FROM @GROUPSIDMEMBERS WHERE objectSid = @USERSID) OR
	     EXISTS (SELECT* FROM @GROUPSIDMEMBERS groupsidmembers INNER JOIN #USERGROUPS usergroups ON groupsidmembers.objectSid = usergroups.objectSid)
	BEGIN	-- user is a non-member
	SET @ISMEMBER = 0
	END
	IF @ISMEMBER = 1 
	BEGIN
		DELETE FROM @GROUPSIDMEMBERS

		EXEC dbo.GetApplicationGroupSidMembers 1, @GROUPOBJECTSID, @NETSQLAZMANMODE, @LDAPPATH, @members_cur OUTPUT -- members
		FETCH NEXT FROM @members_cur INTO @OBJECTSID
		WHILE @@FETCH_STATUS=0
		BEGIN
			INSERT INTO @GROUPSIDMEMBERS VALUES (@OBJECTSID)
			FETCH NEXT FROM @members_cur INTO @OBJECTSID
		END
		CLOSE @members_cur
		DEALLOCATE @members_cur

		IF EXISTS(SELECT * FROM @GROUPSIDMEMBERS WHERE objectSid = @USERSID) OR
		     EXISTS (SELECT * FROM @GROUPSIDMEMBERS groupsidmembers INNER JOIN #USERGROUPS usergroups ON groupsidmembers.objectSid = usergroups.objectSid)
		BEGIN
		-- user is a member
		SET @ISMEMBER = 1
		END
		ELSE
		BEGIN
		-- user is not present
		SET @ISMEMBER = 0
		END
	END
	-- if a member ... get authorization
	IF @ISMEMBER = 1
	BEGIN
		SET @AUTHORIZATION_TYPE = (SELECT dbo.MergeAuthorizations(@AUTHORIZATION_TYPE, @PARTIAL_RESULT))
		SET @ITEM_AUTHORIZATION_TYPE = (SELECT dbo.MergeAuthorizations(@ITEM_AUTHORIZATION_TYPE, @PARTIAL_RESULT))
		IF @PKID IS NOT NULL AND @RETRIEVEATTRIBUTES = 1 
			INSERT INTO #ATTRIBUTES_TABLE SELECT AttributeKey, AttributeValue, NULL FROM dbo.AuthorizationAttributesTable WHERE AuthorizationID = @PKID
	END
END
	FETCH NEXT FROM groups_authorizations_cur INTO @GROUPOBJECTSID, @GROUPWHEREDEFINED, @PARTIAL_RESULT, @PKID
END
CLOSE groups_authorizations_cur
DEALLOCATE groups_authorizations_cur

-- PREPARE RESULTSET FOR BIZ RULE CHECKING
----------------------------------------------------------------------------------------
INSERT INTO #PARTIAL_RESULTS_TABLE 
SELECT     Items.ItemId, Items.Name, Items.ItemType, @ITEM_AUTHORIZATION_TYPE,BizRules.BizRuleId, BizRules.BizRuleSource, BizRules.BizRuleLanguage
FROM         dbo.ItemsTable Items LEFT OUTER JOIN
                      dbo.BizRulesTable BizRules ON Items.BizRuleId = BizRules.BizRuleId WHERE Items.ItemId = @ITEMID
SET NOCOUNT OFF')
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.CheckAccess Procedure Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.CheckAccess Procedure'
END
GO

--
-- Script To Update dbo.DirectCheckAccess Procedure In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.DirectCheckAccess Procedure'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO

REVOKE Execute ON [DirectCheckAccess] TO [NetSqlAzMan_Readers]
GO

exec('ALTER PROCEDURE [dbo].[DirectCheckAccess] (@STORENAME nvarchar(255), @APPLICATIONNAME nvarchar(255), @ITEMNAME nvarchar(255), @OPERATIONSONLY BIT, @TOKEN IMAGE, @USERGROUPSCOUNT INT, @VALIDFOR DATETIME, @LDAPPATH nvarchar(4000), @AUTHORIZATION_TYPE TINYINT OUTPUT, @RETRIEVEATTRIBUTES BIT) 
AS
--Memo: 0 - Role; 1 - Task; 2 - Operation
SET NOCOUNT ON
DECLARE @STOREID int
DECLARE @APPLICATIONID int
DECLARE @ITEMID INT

-- CHECK STORE EXISTANCE/PERMISSIONS
Select @STOREID = StoreId FROM dbo.Stores() WHERE Name = @STORENAME
IF @STOREID IS NULL
	BEGIN
	RAISERROR (''Store not found or Store permission denied.'', 16, 1)
	RETURN 1
	END
-- CHECK APPLICATION EXISTANCE/PERMISSIONS
Select @APPLICATIONID = ApplicationId FROM dbo.Applications() WHERE Name = @APPLICATIONNAME And StoreId = @STOREID
IF @APPLICATIONID IS NULL
	BEGIN
	RAISERROR (''Application not found or Application permission denied.'', 16, 1)
	RETURN 1
	END

SELECT @ITEMID = Items.ItemId
	FROM         dbo.Applications() Applications INNER JOIN
	                      dbo.Items() Items ON Applications.ApplicationId = Items.ApplicationId INNER JOIN
	                      dbo.Stores() Stores ON Applications.StoreId = Stores.StoreId
	WHERE     (Stores.StoreId = @STOREID) AND (Applications.ApplicationId = @APPLICATIONID) AND (Items.Name = @ITEMNAME) AND (@OPERATIONSONLY = 1 AND Items.ItemType=2 OR @OPERATIONSONLY = 0)
IF @ITEMID IS NULL
	BEGIN
	RAISERROR (''Item not found.'', 16, 1)
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
	INSERT INTO #ATTRIBUTES_TABLE SELECT AttributeKey, AttributeValue, NULL FROM dbo.StoreAttributesTable StoreAttributes INNER JOIN dbo.StoresTable Stores ON StoreAttributes.StoreId = Stores.StoreId WHERE Stores.StoreId = @STOREID
	INSERT INTO #ATTRIBUTES_TABLE SELECT AttributeKey, AttributeValue, NULL FROM dbo.ApplicationAttributesTable ApplicationAttributes INNER JOIN dbo.ApplicationsTable Applications ON ApplicationAttributes.ApplicationId = Applications.ApplicationId WHERE Applications.ApplicationId = @APPLICATIONID
END
--------------------------------------------------------------------------------
DECLARE @USERSID varbinary(85)
DECLARE @I INT
DECLARE @INDEX INT
DECLARE @APP VARBINARY(85)
DECLARE @SETTINGVALUE nvarchar(255)
DECLARE @NETSQLAZMANMODE bit

SELECT @SETTINGVALUE = SettingValue FROM dbo.Settings WHERE SettingName = ''Mode''
IF @SETTINGVALUE = ''Developer'' 
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

EXEC dbo.CheckAccess @ITEMID, @USERSID, @VALIDFOR, @LDAPPATH, @AUTHORIZATION_TYPE OUTPUT, @NETSQLAZMANMODE, @RETRIEVEATTRIBUTES
SELECT * FROM #PARTIAL_RESULTS_TABLE
IF @RETRIEVEATTRIBUTES = 1
	SELECT * FROM #ATTRIBUTES_TABLE
DROP TABLE #PARTIAL_RESULTS_TABLE
IF @RETRIEVEATTRIBUTES = 1
	DROP TABLE #ATTRIBUTES_TABLE
DROP TABLE #USERGROUPS
SET NOCOUNT OFF')
GO
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   GRANT Execute ON [DirectCheckAccess] TO [NetSqlAzMan_Readers]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.DirectCheckAccess Procedure Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.DirectCheckAccess Procedure'
END
GO

--
-- Script To Update dbo.IsAMemberOfGroup Procedure In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.IsAMemberOfGroup Procedure'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO

REVOKE Execute ON [IsAMemberOfGroup] TO [NetSqlAzMan_Readers]
GO

exec('ALTER PROCEDURE [dbo].[IsAMemberOfGroup](@GROUPTYPE bit, @GROUPOBJECTSID VARBINARY(85), @NETSQLAZMANMODE bit, @LDAPPATH nvarchar(4000), @TOKEN IMAGE, @USERGROUPSCOUNT INT)  
AS  
DECLARE @member_cur CURSOR
DECLARE @memberSid VARBINARY(85)
DECLARE @USERSID VARBINARY(85)
DECLARE @USERGROUPS TABLE(objectSid VARBINARY(85))
DECLARE @I INT
DECLARE @INDEX INT
DECLARE @APP VARBINARY(85)
DECLARE @COUNT int

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
		INSERT INTO @USERGROUPS (objectSid) VALUES (@APP)
		SET @I = @I + 1
	END
END
ELSE
BEGIN
	SET @USERSID = @TOKEN
END

-- CHECK IF IS A NON-MEMBER
IF @GROUPTYPE = 0 -- STORE GROUP
	EXEC dbo.GetStoreGroupSidMembers 0, @GROUPOBJECTSID, @NETSQLAZMANMODE, @LDAPPATH, @member_cur OUTPUT
ELSE -- APPLICATON GROUP
	EXEC dbo.GetApplicationGroupSidMembers 0, @GROUPOBJECTSID, @NETSQLAZMANMODE, @LDAPPATH, @member_cur OUTPUT

FETCH NEXT FROM @member_cur INTO @memberSid
WHILE @@FETCH_STATUS = 0
BEGIN
	IF @memberSid = @USERSID
	BEGIN
		CLOSE @member_cur
		DEALLOCATE @member_cur
		SELECT CONVERT(bit, 0) -- true
		RETURN
	END		
	SELECT @COUNT =  COUNT(*)  FROM @USERGROUPS WHERE objectSid = @memberSid
	IF @COUNT>0
	BEGIN
		CLOSE @member_cur
		DEALLOCATE @member_cur
		SELECT CONVERT(bit, 0) -- true
		RETURN
	END		
	FETCH NEXT FROM @member_cur INTO @memberSid
END
CLOSE @member_cur
DEALLOCATE @member_cur

-- CHECK IF IS A MEMBER
IF @GROUPTYPE = 0 -- STORE GROUP
	EXEC dbo.GetStoreGroupSidMembers 1, @GROUPOBJECTSID, @NETSQLAZMANMODE, @LDAPPATH, @member_cur OUTPUT
ELSE -- APPLICATON GROUP
	EXEC dbo.GetApplicationGroupSidMembers 1, @GROUPOBJECTSID, @NETSQLAZMANMODE, @LDAPPATH, @member_cur OUTPUT

FETCH NEXT FROM @member_cur INTO @memberSid
WHILE @@FETCH_STATUS = 0
BEGIN
	IF @memberSid = @USERSID
	BEGIN
		CLOSE @member_cur
		DEALLOCATE @member_cur
		SELECT CONVERT(bit,1) -- true
		RETURN
	END		
	SELECT @COUNT =  COUNT(*)  FROM @USERGROUPS WHERE objectSid = @memberSid
	IF @COUNT>0
	BEGIN
		CLOSE @member_cur
		DEALLOCATE @member_cur
		SELECT CONVERT(bit, 1) -- true
		RETURN
	END		
	FETCH NEXT FROM @member_cur INTO @memberSid
END
CLOSE @member_cur
DEALLOCATE @member_cur

SELECT CONVERT(bit, 0) -- true')
GO
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   GRANT Execute ON [IsAMemberOfGroup] TO [NetSqlAzMan_Readers]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.IsAMemberOfGroup Procedure Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.IsAMemberOfGroup Procedure'
END
GO

--
-- Script To Update dbo.ItemAttributeInsert Procedure In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.ItemAttributeInsert Procedure'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO

REVOKE Execute ON [ItemAttributeInsert] TO [NetSqlAzMan_Managers]
GO

exec('ALTER PROCEDURE [dbo].[ItemAttributeInsert]
(
	@ItemId int,
	@AttributeKey nvarchar(255),
	@AttributeValue nvarchar(4000),
	@ApplicationId int
)
AS
IF EXISTS(SELECT ItemId FROM dbo.Items() WHERE ItemId = @ItemId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
BEGIN
	INSERT INTO [dbo].[ItemAttributesTable] ([ItemId], [AttributeKey], [AttributeValue]) VALUES (@ItemId, @AttributeKey, @AttributeValue)
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR (''Application permission denied.'', 16, 1)')
GO
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   GRANT Execute ON [ItemAttributeInsert] TO [NetSqlAzMan_Managers]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.ItemAttributeInsert Procedure Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.ItemAttributeInsert Procedure'
END
GO

--
-- Script To Update dbo.ItemAttributeUpdate Procedure In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.ItemAttributeUpdate Procedure'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO

REVOKE Execute ON [ItemAttributeUpdate] TO [NetSqlAzMan_Managers]
GO

exec('ALTER PROCEDURE [dbo].[ItemAttributeUpdate]
(
	@AttributeKey nvarchar(255),
	@AttributeValue nvarchar(4000),
	@Original_ItemAttributeId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ItemAttributeId FROM dbo.ItemAttributes() WHERE ItemAttributeId = @Original_ItemAttributeId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
	UPDATE [dbo].[ItemAttributesTable] SET [AttributeKey] = @AttributeKey, [AttributeValue] = @AttributeValue WHERE [ItemAttributeId] = @Original_ItemAttributeId
ELSE
	RAISERROR (''Application permission denied.'', 16, 1)')
GO
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   GRANT Execute ON [ItemAttributeUpdate] TO [NetSqlAzMan_Managers]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.ItemAttributeUpdate Procedure Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.ItemAttributeUpdate Procedure'
END
GO

--
-- Script To Update dbo.ItemInsert Procedure In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.ItemInsert Procedure'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO

REVOKE Execute ON [ItemInsert] TO [NetSqlAzMan_Managers]
GO

exec('ALTER PROCEDURE [dbo].[ItemInsert]
(
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@ItemType tinyint,
	@BizRuleId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ApplicationId FROM dbo.Applications() WHERE ApplicationId = @ApplicationId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
BEGIN
	INSERT INTO [dbo].[ItemsTable] ([ApplicationId], [Name], [Description], [ItemType], [BizRuleId]) VALUES (@ApplicationId, @Name, @Description, @ItemType, @BizRuleId)
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR (''Application permission denied.'', 16, 1)')
GO
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   GRANT Execute ON [ItemInsert] TO [NetSqlAzMan_Managers]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.ItemInsert Procedure Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.ItemInsert Procedure'
END
GO

--
-- Script To Update dbo.ItemUpdate Procedure In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.ItemUpdate Procedure'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO

REVOKE Execute ON [ItemUpdate] TO [NetSqlAzMan_Managers]
GO

exec('ALTER PROCEDURE [dbo].[ItemUpdate]
(
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@ItemType tinyint,
	@Original_ItemId int,
	@ApplicationId int
)
AS
IF EXISTS(SELECT ItemId FROM dbo.Items() WHERE ItemId = @Original_ItemId) AND dbo.CheckApplicationPermissions(@ApplicationId, 2) = 1
	UPDATE [dbo].[ItemsTable] SET [Name] = @Name, [Description] = @Description, [ItemType] = @ItemType WHERE [ItemId] = @Original_ItemId AND [ApplicationId] = @ApplicationId
ELSE
	RAISERROR (''Application permission denied.'', 16, 1)')
GO
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   GRANT Execute ON [ItemUpdate] TO [NetSqlAzMan_Managers]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.ItemUpdate Procedure Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.ItemUpdate Procedure'
END
GO

--
-- Script To Update dbo.StoreAttributeInsert Procedure In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.StoreAttributeInsert Procedure'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO

REVOKE Execute ON [StoreAttributeInsert] TO [NetSqlAzMan_Managers]
GO

exec('ALTER PROCEDURE [dbo].[StoreAttributeInsert]
(
	@StoreId int,
	@AttributeKey nvarchar(255),
	@AttributeValue nvarchar(4000)
)
AS
IF EXISTS(Select StoreId FROM dbo.Stores() WHERE StoreId = @StoreId) AND dbo.CheckStorePermissions(@StoreId, 2) = 1
BEGIN
	INSERT INTO [dbo].[StoreAttributesTable] ([StoreId], [AttributeKey], [AttributeValue]) VALUES (@StoreId, @AttributeKey, @AttributeValue);
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR (''Store permission denied.'', 16, 1)')
GO
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   GRANT Execute ON [StoreAttributeInsert] TO [NetSqlAzMan_Managers]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.StoreAttributeInsert Procedure Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.StoreAttributeInsert Procedure'
END
GO

--
-- Script To Update dbo.StoreAttributeUpdate Procedure In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.StoreAttributeUpdate Procedure'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO

REVOKE Execute ON [StoreAttributeUpdate] TO [NetSqlAzMan_Managers]
GO

exec('ALTER PROCEDURE [dbo].[StoreAttributeUpdate]
(
	@StoreId int,
	@AttributeKey nvarchar(255),
	@AttributeValue nvarchar(4000),
	@Original_StoreAttributeId int
)
AS
IF EXISTS(Select StoreAttributeId FROM dbo.StoreAttributes() WHERE StoreAttributeId = @Original_StoreAttributeId) AND dbo.CheckStorePermissions(@StoreId, 2) = 1
	UPDATE [dbo].[StoreAttributesTable] SET [AttributeKey] = @AttributeKey, [AttributeValue] = @AttributeValue WHERE [StoreAttributeId] = @Original_StoreAttributeId AND [StoreId] = @StoreId 
ELSE
	RAISERROR (''Store permission denied.'', 16, 1)')
GO
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   GRANT Execute ON [StoreAttributeUpdate] TO [NetSqlAzMan_Managers]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.StoreAttributeUpdate Procedure Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.StoreAttributeUpdate Procedure'
END
GO

--
-- Script To Update dbo.StoreGroupInsert Procedure In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.StoreGroupInsert Procedure'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO

REVOKE Execute ON [StoreGroupInsert] TO [NetSqlAzMan_Managers]
GO

exec('ALTER PROCEDURE [dbo].[StoreGroupInsert]
(
	@StoreId int,
	@objectSid varbinary(85),
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@LDapQuery nvarchar(4000),
	@GroupType tinyint
)
AS
IF dbo.CheckStorePermissions(@StoreId, 2) = 1
BEGIN
	INSERT INTO [dbo].[StoreGroupsTable] ([StoreId], [objectSid], [Name], [Description], [LDapQuery], [GroupType]) VALUES (@StoreId, @objectSid, @Name, @Description, @LDapQuery, @GroupType);
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR (''Store permission denied.'', 16, 1)')
GO
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   GRANT Execute ON [StoreGroupInsert] TO [NetSqlAzMan_Managers]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.StoreGroupInsert Procedure Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.StoreGroupInsert Procedure'
END
GO

--
-- Script To Update dbo.StoreGroupUpdate Procedure In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.StoreGroupUpdate Procedure'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO

REVOKE Execute ON [StoreGroupUpdate] TO [NetSqlAzMan_Managers]
GO

exec('ALTER PROCEDURE [dbo].[StoreGroupUpdate]
(
	@StoreId int,
	@objectSid varbinary(85),
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@LDapQuery nvarchar(4000),
	@GroupType tinyint,
	@Original_StoreGroupId int
)
AS
IF dbo.CheckStorePermissions(@StoreId, 2) = 1
	UPDATE [dbo].[StoreGroupsTable] SET [objectSid] = @objectSid, [Name] = @Name, [Description] = @Description, [LDapQuery] = @LDapQuery, [GroupType] = @GroupType WHERE [StoreGroupId] = @Original_StoreGroupId AND StoreId = @StoreId
ELSE
	RAISERROR (''Store permission denied.'', 16, 1)')
GO
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   GRANT Execute ON [StoreGroupUpdate] TO [NetSqlAzMan_Managers]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.StoreGroupUpdate Procedure Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.StoreGroupUpdate Procedure'
END
GO

--
-- Script To Update dbo.StoreInsert Procedure In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.StoreInsert Procedure'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO

REVOKE Execute ON [StoreInsert] TO [NetSqlAzMan_Administrators]
GO

SET QUOTED_IDENTIFIER OFF
GO

SET ANSI_NULLS OFF
GO

exec('ALTER PROCEDURE [dbo].[StoreInsert]
(
	@Name nvarchar(255),
	@Description nvarchar(1024)
)
AS
INSERT INTO [dbo].[StoresTable] ([Name], [Description]) VALUES (@Name, @Description);
RETURN SCOPE_IDENTITY()')
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   GRANT Execute ON [StoreInsert] TO [NetSqlAzMan_Administrators]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.StoreInsert Procedure Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.StoreInsert Procedure'
END
GO

--
-- Script To Update dbo.StorePermissionInsert Procedure In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.StorePermissionInsert Procedure'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO

REVOKE Execute ON [StorePermissionInsert] TO [NetSqlAzMan_Managers]
GO

SET QUOTED_IDENTIFIER OFF
GO

SET ANSI_NULLS OFF
GO

exec('ALTER PROCEDURE [dbo].[StorePermissionInsert]
(
	@StoreId int,
	@SqlUserOrRole nvarchar(128),
	@IsSqlRole bit,
	@NetSqlAzManFixedServerRole tinyint
)
AS
IF EXISTS(SELECT StoreId FROM dbo.Stores() WHERE StoreId = @StoreId) AND dbo.CheckStorePermissions(@StoreId, 2) = 1
BEGIN
	INSERT INTO dbo.StorePermissionsTable (StoreId, SqlUserOrRole, IsSqlRole, NetSqlAzManFixedServerRole) VALUES (@StoreId, @SqlUserOrRole, @IsSqlRole, @NetSqlAzManFixedServerRole)
	RETURN SCOPE_IDENTITY()
END
ELSE
	RAISERROR (''Store permission denied.'', 16, 1)')
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   GRANT Execute ON [StorePermissionInsert] TO [NetSqlAzMan_Managers]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.StorePermissionInsert Procedure Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.StorePermissionInsert Procedure'
END
GO

--
-- Script To Update dbo.StoreUpdate Procedure In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Updating dbo.StoreUpdate Procedure'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO

REVOKE Execute ON [StoreUpdate] TO [NetSqlAzMan_Managers]
GO

SET QUOTED_IDENTIFIER OFF
GO

SET ANSI_NULLS OFF
GO

exec('ALTER PROCEDURE [dbo].[StoreUpdate]
(
	@Name nvarchar(255),
	@Description nvarchar(1024),
	@Original_StoreId int
)
AS
IF EXISTS(Select StoreId FROM dbo.Stores() WHERE StoreId = @Original_StoreId) AND dbo.CheckStorePermissions(@Original_StoreId, 2) = 1
	UPDATE [dbo].[StoresTable] SET [Name] = @Name, [Description] = @Description WHERE [StoreId] = @Original_StoreId
ELSE
	RAISERROR (''Store permission denied.'', 16, 1)')
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
   GRANT Execute ON [StoreUpdate] TO [NetSqlAzMan_Managers]
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.StoreUpdate Procedure Updated Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Update dbo.StoreUpdate Procedure'
END
GO

--
-- Script To Create dbo.ItemsHierarchyTrigger Trigger In ..NetSqlAzMan_2450
-- Generated sabato, aprile 12, 2008, at 02.34 PM
--
-- Please backup ..NetSqlAzMan_2450 before executing this script
--


BEGIN TRANSACTION
GO
SET TRANSACTION ISOLATION LEVEL SERIALIZABLE
GO

PRINT 'Creating dbo.ItemsHierarchyTrigger Trigger'
GO

SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, QUOTED_IDENTIFIER, CONCAT_NULL_YIELDS_NULL ON
GO

SET NUMERIC_ROUNDABORT OFF
GO


SET QUOTED_IDENTIFIER OFF
GO

exec('CREATE TRIGGER [dbo].[ItemsHierarchyTrigger] ON [dbo].[ItemsHierarchyTable] 
FOR INSERT, UPDATE
AS
DECLARE @INSERTEDITEMID int
DECLARE @INSERTEDMEMBEROFITEMID int

DECLARE itemhierarchy_cur CURSOR FAST_FORWARD FOR SELECT ItemId, MemberOfItemId FROM inserted
OPEN itemhierarchy_cur
FETCH NEXT from itemhierarchy_cur INTO @INSERTEDITEMID, @INSERTEDMEMBEROFITEMID
WHILE @@FETCH_STATUS = 0
BEGIN
	IF UPDATE(ItemId) AND NOT EXISTS (SELECT ItemId FROM dbo.ItemsTable WHERE ItemsTable.ItemId = @INSERTEDITEMID) 
	 BEGIN
	  RAISERROR (''ItemId NOT FOUND into dbo.ItemsTable'', 16, 1)
	  ROLLBACK TRANSACTION
	 END
	
	IF UPDATE(MemberOfItemId) AND NOT EXISTS (SELECT ItemId FROM dbo.ItemsTable WHERE ItemsTable.ItemId = @INSERTEDMEMBEROFITEMID)
	 BEGIN
	  RAISERROR (''MemberOfItemId NOT FOUND into dbo.ItemsTable'', 16, 1)
	  ROLLBACK TRANSACTION
	 END
	FETCH NEXT from itemhierarchy_cur INTO @INSERTEDITEMID, @INSERTEDMEMBEROFITEMID
END
CLOSE itemhierarchy_cur
DEALLOCATE itemhierarchy_cur')
GO

SET QUOTED_IDENTIFIER ON
GO

IF @@ERROR <> 0
   IF @@TRANCOUNT = 1 ROLLBACK TRANSACTION
GO

IF @@TRANCOUNT = 1
BEGIN
   PRINT 'dbo.ItemsHierarchyTrigger Trigger Added Successfully'
   COMMIT TRANSACTION
END ELSE
BEGIN
   PRINT 'Failed To Add dbo.ItemsHierarchyTrigger Trigger'
END
GO