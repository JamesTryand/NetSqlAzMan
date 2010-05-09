using SqlAudit;

namespace NetSqlAzMan.SnapIn.SQLAudit
{
    internal static class StorageAuditGenerator
    {
        internal static string GenerateAuditScript(string storageConnectionString, ScriptAction scriptAction, ProgressChangeDelegate progressChangeDelegate)
        {
            //Set Audit Settings
            SqlAuditSettings myAuditSettings = new SqlAuditSettings(storageConnectionString);

            //Choose Script MMC.SyncAction
            myAuditSettings.ScriptAction = scriptAction;

            //Select Audit Columns
            myAuditSettings.AuditColumns = AuditColumns.ApplicationName | AuditColumns.AuditDateTime | AuditColumns.HostId | AuditColumns.HostName | AuditColumns.UserName | AuditColumns.UserSID;

            //Choose Triggers MMC.SyncAction
            myAuditSettings.TriggersActions = TriggerActions.Insert | TriggerActions.Update | TriggerActions.Delete;

            //Choose Indexes MMC.SyncAction
            myAuditSettings.IndexesAction = IndexesAction.SameIndexesAsSourceTable;

            //Assign naming properties
            myAuditSettings.AuditTablesSuffix = "_Audit";
            myAuditSettings.AuditTriggersSuffix = "_TriggerForAudit";

            //Define File Groups
            myAuditSettings.TablesFileGroupName = "[PRIMARY]";
            myAuditSettings.IndexesFileGroupName = "[PRIMARY]";

            //Use SqlAuditDiscovery Class to get Table Schemas.
            SqlAuditDiscovery auditDiscoveryUtility = new SqlAuditDiscovery(storageConnectionString);

            //Get TableInfo Collection
            TableInfoCollection myTables = auditDiscoveryUtility.GetAllTableInfo(myAuditSettings);

            //*** Tables Customization ***//
            myTables.Remove(new SchemaTablePair("dbo", "UsersDemo"));
            myTables.Remove(new SchemaTablePair("dbo", "netsqlazman_LogTable"));

            

            //Generate Audit Script
            SqlAuditGenerator generator = new SqlAuditGenerator(myAuditSettings);
            generator.ProgressChange+=progressChangeDelegate;
            return generator.GenerateDDLScriptForAuditTables(myTables);
        }
    }
}
