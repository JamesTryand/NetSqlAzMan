//Remove comment To Debug MMC SnapIn
//#define MMCDEBUG 

using System;
using System.Reflection;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Security.Principal;
using System.Runtime.Serialization;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.LINQ;
using System.Linq;
using NetSqlAzMan.ENS;
using NetSqlAzMan.Logging;
using NetSqlAzMan.DirectoryServices;
using System.ServiceModel;

namespace NetSqlAzMan
{
    /// <summary>
    /// AzMan Storage Class.
    /// </summary>
    [Serializable()]
    [DataContract(Namespace = "http://NetSqlAzMan/ServiceModel", IsReference = true)]
    public sealed partial class SqlAzManStorage : IAzManStorage
    {
        #region Delegates
        /// <summary>
        /// Delegate for Async Check Access [FOR Windows Users ONLY].
        /// </summary>
        /// <param name="StoreName">Name of the store.</param>
        /// <param name="ApplicationName">Name of the application.</param>
        /// <param name="ItemName">Name of the itemName.</param>
        /// <param name="windowsIdentity">The windows identity.</param>
        /// <param name="ValidFor">The valid for.</param>
        /// <param name="OperationsOnly">if set to <c>true</c> [operations only].</param>
        /// <param name="attributes">The Attributes readed.</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns>AuthorizationType</returns>
        public delegate AuthorizationType AsyncCheckAccess(string StoreName, string ApplicationName, string ItemName, WindowsIdentity windowsIdentity, DateTime ValidFor, bool OperationsOnly, out List<KeyValuePair<string, string>> attributes, params KeyValuePair<string, object>[] contextParameters);
        /// <summary>
        /// Delegate for Async Check Access [FOR DB Users ONLY].
        /// </summary>
        /// <param name="StoreName">Name of the store.</param>
        /// <param name="ApplicationName">Name of the application.</param>
        /// <param name="ItemName">Name of the itemName.</param>
        /// <param name="dbUser">The db user.</param>
        /// <param name="ValidFor">The valid for.</param>
        /// <param name="OperationsOnly">if set to <c>true</c> [operations only].</param>
        /// <param name="attributes">The Attributes readed.</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns>AuthorizationType</returns>
        public delegate AuthorizationType AsyncCheckAccessForDBUsers(string StoreName, string ApplicationName, string ItemName, IAzManDBUser dbUser, DateTime ValidFor, bool OperationsOnly, out List<KeyValuePair<string, string>> attributes, params KeyValuePair<string, object>[] contextParameters);
        #endregion Delegates
        #region Sub-Classes
        internal class ItemNode
        {
            internal HybridDictionary parentItemNodes;
            internal int itemId;
            internal string itemName;
            internal ItemType itemType;
            internal AuthorizationType authorizationType;
            internal int? bizRuleId;
            internal string bizRuleSource;
            internal byte? bizRuleLanguage;
            internal IAzManItem azmanItem;

            internal ItemNode(int itemId, string itemName, ItemType itemType, AuthorizationType authorizationType, int? bizRuleId, string bizRuleSource, byte? bizRuleType, IAzManItem azmanItem)
            {
                this.parentItemNodes = new HybridDictionary();
                this.itemId = itemId;
                this.itemName = itemName;
                this.itemType = itemType;
                this.authorizationType = authorizationType;
                this.bizRuleId = bizRuleId;
                this.bizRuleSource = bizRuleSource;
                this.bizRuleLanguage = bizRuleType;
                this.azmanItem = azmanItem;
            }
        }
        #endregion Sub-Classes
        #region Fields
        [NonSerialized()]
        internal NetSqlAzManStorageDataContext db;
        private string connectionString;
        private NetSqlAzManMode? mode;
        private bool? logErrors = null;
        private bool? logWarnings = null;
        private bool? logInformations = null;
        private bool? logOnEventLog = null;
        private bool? logOnDb = null;
        private ConnectionState backupConnectionState = ConnectionState.Closed;
        private bool userTransaction;
        private AsyncCheckAccess asyncCheckAccess = null;
        private AsyncCheckAccessForDBUsers asyncCheckAccessForDBUsers = null;
        private bool? iamadmin;
        //[NonSerialized()]
        private SqlAzManENS ens;
        internal Guid transactionGuid = Guid.Empty;
        internal Guid instanceGuid = Guid.Empty;
        internal int operationCounter = 0;
        [NonSerialized()]
        internal Logging.LoggingUtility logging;
        internal Dictionary<string, IAzManStore> stores;
        private Dictionary<string, IAzManDBUser> dbUsers;
        #endregion Fields
        #region Static Members
        /// <summary>
        /// Gets or sets the log stream.
        /// </summary>
        /// <value>The log stream.</value>
        public System.IO.TextWriter LogStream
        {
            get
            {
                return this.db.Log;
            }
            set
            {
                this.db.Log = value;
            }
        }
        /// <summary>
        /// Gets or sets the root dse path.
        /// </summary>
        /// <value>The root DSE path.</value>
        public static string RootDSEPath
        {
            get
            {
                return DirectoryServicesUtils.rootDsePath;
            }
            set
            {
                DirectoryServicesUtils.rootDsePath = value;
            }
        }
        #endregion Static Members
        #region Events
        /// <summary>
        /// Occurs after a Store object has been Created.
        /// </summary>
        public event StoreCreatedDelegate StoreCreated;
        /// <summary>
        /// Occurs after a Store object has been Opened.
        /// </summary>
        public event StoreOpenedDelegate StoreOpened;
        /// <summary>
        /// Occurs after a Storage Transaction has benn initiated.
        /// </summary>
        public event TransactionBeginnedDelegate TransactionBeggined;
        /// <summary>
        /// Occurs after a Storage Transaction has benn terminated.
        /// </summary>
        public event TransactionTerminatedDelegate TransactionTerminated;
        /// <summary>
        /// Occurs after NetSqlAzMan Mode has been changed.
        /// </summary>
        public event NetSqlAzManModeChangeDelegate NetSqlAzManModeChanged;
        #endregion Events
        #region Private Event Raisers
        private void raiseStoreCreated(IAzManStore storeCreated)
        {
            if (this.StoreCreated != null)
                this.StoreCreated(storeCreated);
        }
        private void raiseStoreOpened(IAzManStore store)
        {
            if (this.StoreOpened != null)
                this.StoreOpened(store);
        }
        private void raiseTransactionBeginned()
        {
            if (this.TransactionBeggined != null)
                this.TransactionBeggined();
        }
        private void raiseTransactionTerminated(bool success)
        {
            if (this.TransactionTerminated != null)
                this.TransactionTerminated(success);
        }
        private void raiseNetSqlAzManModeChanged(NetSqlAzManMode oldMode, NetSqlAzManMode newMode)
        {
            if (this.NetSqlAzManModeChanged != null)
                this.NetSqlAzManModeChanged(oldMode, newMode);
        }
        #endregion Private Event Raisers
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlAzManStorage"/> class.
        /// </summary>
        [System.Runtime.Serialization.OnDeserialized()]
        private void OnDeserialized(StreamingContext context)
        {
            this.db = new NetSqlAzManStorageDataContext(this.ConnectionString);
        }
        [System.Runtime.Serialization.OnSerializing()]
        private void OnSerializing(StreamingContext context)
        {
            NetSqlAzManMode mode = this.Mode; //force mode reading
        }

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SqlAzManStore"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public SqlAzManStorage(string connectionString)
        {
            this.userTransaction = false;
#if MMCDEBUG
            System.Windows.Forms.MessageBox.Show("This message is for Debug only. Attach now to the process 'MMC.EXE'.", "Debug", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
#endif
            this.instanceGuid = Guid.NewGuid();
            this.logging = new LoggingUtility();
            //ENS Subscriptions
            this.ens = new SqlAzManENS();
            this.SubscribeToENS();
            this.ConnectionString = connectionString;
        }
        #endregion Constructors
        #region Private Members
        internal bool internalBeginTransaction(IsolationLevel isolationLevel)
        {
            if (!this.userTransaction)
            {
                if (this.db.Transaction == null)
                {
                    if (this.db.Connection.State == ConnectionState.Closed)
                    {
                        this.db.Connection.Open();
                    }
                    this.db.Transaction = this.db.Connection.BeginTransaction(isolationLevel);
                    this.raiseTransactionBeginned();
                    return true;
                }
            }
            return false;
        }
        internal void internalCommitTransaction()
        {
            if (!this.userTransaction)
            {
                if (this.db.Transaction != null)
                {
                    this.db.Transaction.Commit();
                    this.raiseTransactionTerminated(true);
                    this.db.Connection.Close();
                }
            }
        }
        internal void internalRollBackTransaction()
        {
            if (!this.userTransaction)
            {
                if (this.db.Transaction != null)
                {
                    this.raiseTransactionTerminated(false);
                    this.db.Transaction.Rollback();
                    this.db.Connection.Close();
                }
            }
        }
        #endregion Private Members
        #region IAzManStorage Members
        /// <summary>
        /// Gets or sets the storage time out.
        /// </summary>
        /// <value>The storage time out.</value>
        public int StorageTimeOut
        {
            get
            {
                return this.db.CommandTimeout;
            }
            set 
            {
                this.db.CommandTimeout = value;
            }
        }
        /// <summary>
        /// Gets the database vesion.
        /// </summary>
        /// <value>The database vesion.</value>
        public string DatabaseVesion
        {
            get
            {
                return this.db.NetSqlAzMan_DBVersion();
            }
        }
        /// <summary>
        /// Gets the DB users.
        /// </summary>
        /// <value>The DB users.</value>
        public Dictionary<string, IAzManDBUser> DBUsers
        {
            get
            {
                if (this.dbUsers == null)
                {
                    this.dbUsers = new Dictionary<string, IAzManDBUser>();
                    foreach (IAzManDBUser d in this.GetDBUsers())
                    {
                        this.dbUsers.Add(d.UserName, d);
                    }
                }
                return this.dbUsers;
            }
        }
        /// <summary>
        /// Gets the stores.
        /// </summary>
        /// <value>The stores.</value>
        public Dictionary<string, IAzManStore> Stores
        {
            get
            {
                if (this.stores == null)
                {
                    this.stores = new Dictionary<string, IAzManStore>();
                    foreach (IAzManStore s in this.GetStores())
                    {
                        this.stores.Add(s.Name, s);
                    }
                }
                return this.stores;
            }
        }
        /// <summary>
        /// Opens the connection.
        /// </summary>
        public void OpenConnection()
        {
            this.backupConnectionState = this.db.Connection.State;
            if (this.backupConnectionState != ConnectionState.Open)
            {
                this.db.Connection.Open();
            }
        }
        /// <summary>
        /// Closes the connection.
        /// </summary>
        public void CloseConnection()
        {
            if (this.db.Connection.State == ConnectionState.Open && this.backupConnectionState == ConnectionState.Closed)
            {
                this.db.Connection.Close();
            }
        }
        /// <summary>
        /// Verifies if the database is a valid NetSqlAzMan Storage DB.
        /// </summary>
        public static void VerifyStorageDB(string connectionString)
        {
            using (NetSqlAzManStorageDataContext db = new NetSqlAzManStorageDataContext(connectionString))
            {
                db.AuthorizationAttributes().Any();
            }
        }
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        [DataMember]
        public string ConnectionString
        {
            get
            {
                return this.connectionString;
            }
            set
            {
                SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder(value);
                if (!scsb.IntegratedSecurity)
                    scsb.PersistSecurityInfo = true;
                this.connectionString = scsb.ToString();
                this.db = new NetSqlAzManStorageDataContext(this.connectionString);
                this.db.CommandTimeout = this.StorageTimeOut;
            }
        }

        internal SqlConnection connection
        {
            get
            {
                return (SqlConnection)this.db.Connection;
            }
        }
        /// <summary>
        /// Opens the specified store name.
        /// </summary>
        /// <param name="storeName">Name of the store.</param>
        /// <returns></returns>
        public IAzManStore GetStore(string storeName)
        {
            StoresResult sr;
            if ((sr = (from t in this.db.Stores() where t.Name == storeName select t).FirstOrDefault())!=null)
            {
                int storeId = sr.StoreId.Value;
                string name = sr.Name;
                string description = sr.Description;
                byte netsqlazmanFixedServerRole = 0;
                if (this.IAmAdmin)
                {
                    netsqlazmanFixedServerRole = 3;
                }
                else
                {
                    var res1 = this.db.CheckStorePermissions(storeId, 2);
                    var res2 = this.db.CheckStorePermissions(storeId, 1);
                    if (res1.HasValue && res1.Value)
                        netsqlazmanFixedServerRole = 2;
                    else if (res2.HasValue && res2.Value)
                        netsqlazmanFixedServerRole = 1;
                }
                IAzManStore result = new SqlAzManStore(this.db, this, storeId, name, description, netsqlazmanFixedServerRole, this.ens);
                this.raiseStoreOpened(result);
                if (this.ens!=null)
                    this.ens.AddPublisher(result);
                return result;
            }
            else
            {
                throw SqlAzManException.StoreNotFoundException(storeName,null);
            }
        }

        /// <summary>
        /// Gets the <see cref="T:IAzManStore"/> with the specified store name.
        /// </summary>
        /// <value></value>
        public IAzManStore this[string storeName]
        {
            get
            {
                return this.GetStore(storeName);
            }
        }

        /// <summary>
        /// Determines whether this instance has stores.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance has stores; otherwise, <c>false</c>.
        /// </returns>
        public bool HasStores()
        {
            return this.db.Stores().Any();
        }

        /// <summary>
        /// Gets the stores.
        /// </summary>
        /// <returns></returns>
        public IAzManStore[] GetStores()
        {
            IAzManStore[] stores;
            var s = (from tf in this.db.Stores()
                    orderby tf.Name
                    select tf).ToList();
            stores = new SqlAzManStore[s.Count];
            int index = 0;
            foreach (var row in s)
            {
                byte netsqlazmanFixedServerRole = 0;
                if (this.IAmAdmin)
                {
                    netsqlazmanFixedServerRole = 3;
                }
                else
                {
                    var res1 = this.db.CheckStorePermissions(row.StoreId, 2);
                    var res2 = this.db.CheckStorePermissions(row.StoreId, 1);
                    if (res1.HasValue && res1.Value)
                        netsqlazmanFixedServerRole = 2;
                    else if (res2.HasValue && res2.Value)
                        netsqlazmanFixedServerRole = 1;
                }
                stores[index] = new SqlAzManStore(this.db, this, row.StoreId.Value, row.Name, row.Description, netsqlazmanFixedServerRole, this.ens);
                this.raiseStoreOpened(stores[index]);
                if (this.ens!=null)
                    this.ens.AddPublisher(stores[index]);
                index++;
            }
            return stores;
        }
        /// <summary>
        /// Creates the specified store name.
        /// </summary>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="storeDescription">The store description.</param>
        /// <returns></returns>
        public IAzManStore CreateStore(string storeName, string storeDescription)
        {
            try
            {
                this.db.StoreInsert(storeName, storeDescription);
                IAzManStore result = this.GetStore(storeName);
                this.raiseStoreCreated(result);
                this.stores = null; //Force cache refresh
                return result;
            }
            catch (System.Data.SqlClient.SqlException sqlex)
            {
                if (sqlex.Number == 2601) //Index Duplicate Error
                    throw SqlAzManException.StoreDuplicateException(storeName, sqlex);
                else
                    throw SqlAzManException.GenericException(sqlex);
            }
        }
        /// <summary>
        /// Checks the access [FOR Windows Users only].
        /// </summary>
        /// <param name="StoreName">Name of the store.</param>
        /// <param name="ApplicationName">Name of the application.</param>
        /// <param name="ItemName">Name of the itemName.</param>
        /// <param name="windowsIdentity">The windows identity.</param>
        /// <param name="ValidFor">The valid for.</param>
        /// <param name="OperationsOnly">if set to <c>true</c> [operations only].</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns>AuthorizationType</returns>
        public AuthorizationType CheckAccess(string StoreName, string ApplicationName, string ItemName, WindowsIdentity windowsIdentity, DateTime ValidFor, bool OperationsOnly, params KeyValuePair<string, object>[] contextParameters)
        {
            List<KeyValuePair<string, string>> attributes;
            return this.internalCheckAccess(StoreName, ApplicationName, ItemName, windowsIdentity, ValidFor, OperationsOnly, false, out attributes, contextParameters);
        }

        /// <summary>
        /// Checks the access [FOR Windows Users only].
        /// </summary>
        /// <param name="StoreName">Name of the store.</param>
        /// <param name="ApplicationName">Name of the application.</param>
        /// <param name="ItemName">Name of the itemName.</param>
        /// <param name="windowsIdentity">The windows identity.</param>
        /// <param name="ValidFor">The valid for.</param>
        /// <param name="OperationsOnly">if set to <c>true</c> [operations only].</param>
        /// <param name="attributes">The attributes readed.</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns>AuthorizationType</returns>
        public AuthorizationType CheckAccess(string StoreName, string ApplicationName, string ItemName, WindowsIdentity windowsIdentity, DateTime ValidFor, bool OperationsOnly, out List<KeyValuePair<string, string>> attributes, params KeyValuePair<string, object>[] contextParameters)
        {
            return this.internalCheckAccess(StoreName, ApplicationName, ItemName, windowsIdentity, ValidFor, OperationsOnly, true, out attributes, contextParameters);
        }

        private AuthorizationType internalCheckAccess(string StoreName, string ApplicationName, string ItemName, WindowsIdentity windowsIdentity, DateTime ValidFor, bool OperationsOnly, bool retrieveAttributes, out List<KeyValuePair<string, string>> attributes, params KeyValuePair<string, object>[] contextParameters)
        {
            List<byte> token = new List<byte>();
            int userGroupsCount = windowsIdentity.Groups.Count;
            if (userGroupsCount > 0)
            {
                token.AddRange(SqlAzManItem.getSqlBinarySid(windowsIdentity.User));
                foreach (SecurityIdentifier userGroupSid in windowsIdentity.Groups)
                {
                    token.AddRange(SqlAzManItem.getSqlBinarySid(userGroupSid));
                }
            }
            else
            {
                byte[] bSid = new byte[windowsIdentity.User.BinaryLength];
                windowsIdentity.User.GetBinaryForm(bSid, 0);
                token.AddRange(bSid);
            }
            SqlConnection conn = new SqlConnection(this.db.Connection.ConnectionString);
            conn.Open();
            DataSet checkAccessResults = new DataSet();
            System.Data.SqlClient.SqlCommand cmd = new SqlCommand("DirectCheckAccess", conn);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@STORENAME", StoreName);
            cmd.Parameters.AddWithValue("@APPLICATIONNAME", ApplicationName);
            cmd.Parameters.AddWithValue("@ITEMNAME", ItemName);
            cmd.Parameters.AddWithValue("@OPERATIONSONLY", OperationsOnly);
            cmd.Parameters.AddWithValue("@TOKEN", token.ToArray());
            cmd.Parameters.AddWithValue("@USERGROUPSCOUNT", (object)userGroupsCount);
            cmd.Parameters.AddWithValue("@VALIDFOR", ValidFor);
            cmd.Parameters.AddWithValue("@LDAPPATH", DirectoryServicesUtils.rootDsePath);
            cmd.Parameters.AddWithValue("@RETRIEVEATTRIBUTES", retrieveAttributes);
            System.Data.SqlClient.SqlParameter pAuthorizationType = new System.Data.SqlClient.SqlParameter("@AUTHORIZATION_TYPE", (object)0);
            pAuthorizationType.Direction = System.Data.ParameterDirection.InputOutput;
            cmd.Parameters.Add(pAuthorizationType);
            //BizRule Check CallBack
            SqlDataAdapter checkAccessPartialResultsDataAdapter = new SqlDataAdapter(cmd);
            try
            {
                checkAccessPartialResultsDataAdapter.Fill(checkAccessResults);
            }
            catch (SqlException sqlex)
            {
                if (sqlex.Message.StartsWith("Store not found", StringComparison.CurrentCultureIgnoreCase))
                {
                    throw SqlAzManException.StoreNotFoundException(StoreName, sqlex);
                }
                else if (sqlex.Message.StartsWith("Application not found", StringComparison.CurrentCultureIgnoreCase))
                {
                    throw SqlAzManException.ApplicationNotFoundException(ApplicationName, StoreName, sqlex);
                }
                else if (sqlex.Message.StartsWith("Item not found", StringComparison.CurrentCultureIgnoreCase))
                {
                    throw SqlAzManException.ItemNotFoundException(ItemName, StoreName, ApplicationName, sqlex);
                }
                else
                {
                    throw SqlAzManException.GenericException(sqlex);
                }
            }
            finally
            {
                conn.Close();
            }
            DataTable checkAccessPartialResultsDataTable = checkAccessResults.Tables[0];
            DataTable checkAccessAttributesDataTable = (retrieveAttributes ? checkAccessResults.Tables[1] : new DataTable());
            AuthorizationType result;
            if (checkAccessPartialResultsDataTable.Select("BizRuleId IS NOT NULL").Length == 0)
            {
                //No business rules to check ... just return check access authorizationType
                result = (AuthorizationType)((int)pAuthorizationType.Value);
            }
            else
            {
                //Transform DataRows into a Hierarchical Tree of Node(s)
                IAzManApplication application = this[StoreName][ApplicationName];
                ItemNode itemNode = this.buildTreeOfNodes(application, checkAccessPartialResultsDataTable, ItemName);
                //Execute Biz Rules and Cut Nodes that returns false
                Hashtable ctxParameters = new Hashtable();
                if (contextParameters != null)
                {
                    foreach (KeyValuePair<string, object> kv in contextParameters)
                    {
                        ctxParameters.Add(kv.Key, kv.Value);
                    }
                }
                itemNode = this.executeBizRules(itemNode, new SqlAzManSID(windowsIdentity.User), ctxParameters);
                //Compute final CheckAccess authorizationType
                result = this.computeCheckAccessResult(itemNode, ref checkAccessAttributesDataTable);
                if (result == AuthorizationType.Deny || result == AuthorizationType.Neutral)
                {
                    checkAccessAttributesDataTable = new DataTable(); //no attributes
                }
            }
            //Populate Attributes authorizationType
            if (retrieveAttributes)
            {
                attributes = new List<KeyValuePair<string, string>>(checkAccessAttributesDataTable.Rows.Count);
                foreach (DataRow dr in checkAccessAttributesDataTable.Rows)
                {
                    KeyValuePair<string, string> kvp = new KeyValuePair<string, string>((string)dr[0], (string)dr[1]);
                    if (!attributes.Contains(kvp))
                    {
                        if (dr["ItemId"] == DBNull.Value)
                        {
                            if (result == AuthorizationType.Allow || result == AuthorizationType.AllowWithDelegation)
                                attributes.Add(kvp);
                        }
                        else if (dr["ItemId"]!=DBNull.Value)
                        {
                            int itemId = (int)dr["ItemId"];
                            DataRow[] drItems = checkAccessPartialResultsDataTable.Select("ItemId=" + itemId.ToString());
                            if (drItems.Length>0)
                            {
                                DataRow drItem = drItems[0];
                                AuthorizationType auth = (AuthorizationType)drItem["AuthorizationType"];
                                if (auth== AuthorizationType.Allow || auth == AuthorizationType.AllowWithDelegation)
                                {
                                    attributes.Add(kvp);
                                }
                                else if (auth == AuthorizationType.Neutral)
                                {
                                    AuthorizationType authParent = this.getParentResult(drItem, checkAccessPartialResultsDataTable, this.Stores[StoreName][ApplicationName].Items.Values.ToArray());
                                    if (authParent == AuthorizationType.Allow || authParent == AuthorizationType.AllowWithDelegation)
                                    {
                                        attributes.Add(kvp);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                attributes = new List<KeyValuePair<string, string>>();
            }
            return result;
        }

        private AuthorizationType getParentResult(DataRow drItem, DataTable checkAccessPartialResultsDataTable, IAzManItem[] items)
        {
            AuthorizationType result = AuthorizationType.Neutral;
            DataRow[] drItems = checkAccessPartialResultsDataTable.Select("ItemId=" + drItem["ItemId"].ToString());
            if (drItems.Length > 0)
            {
                result = (AuthorizationType)drItems[0]["AuthorizationType"];
                string itemName = (string)drItem["ItemName"];
                foreach (var item in items)
                {
                    foreach (var member in item.Members.Values)
                    {
                        if (member.Name == itemName)
                        {
                            DataRow[] drParentItems = checkAccessPartialResultsDataTable.Select("ItemId="+item.ItemId.ToString());
                            if (drParentItems.Length>0)
                            {
                                DataRow drParentItem = drParentItems[0];
                                result = SqlAzManItem.mergeAuthorizations(result, this.getParentResult(drParentItem, checkAccessPartialResultsDataTable, items));
                            }
                            break;
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Checks the access [FOR DB Users only].
        /// </summary>
        /// <param name="StoreName">Name of the store.</param>
        /// <param name="ApplicationName">Name of the application.</param>
        /// <param name="ItemName">Name of the itemName.</param>
        /// <param name="dbUser">The db user.</param>
        /// <param name="ValidFor">The valid for.</param>
        /// <param name="OperationsOnly">if set to <c>true</c> [operations only].</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns>AuthorizationType</returns>
        public AuthorizationType CheckAccess(string StoreName, string ApplicationName, string ItemName, IAzManDBUser dbUser, DateTime ValidFor, bool OperationsOnly, params KeyValuePair<string, object>[] contextParameters)
        {
            List<KeyValuePair<string, string>> attributes;
            return this.internalCheckAccess(StoreName, ApplicationName, ItemName, dbUser, ValidFor, OperationsOnly, false, out attributes, contextParameters);
        }

        /// <summary>
        /// Checks the access [FOR DB Users only].
        /// </summary>
        /// <param name="StoreName">Name of the store.</param>
        /// <param name="ApplicationName">Name of the application.</param>
        /// <param name="ItemName">Name of the itemName.</param>
        /// <param name="dbUser">The db user.</param>
        /// <param name="ValidFor">The valid for.</param>
        /// <param name="OperationsOnly">if set to <c>true</c> [operations only].</param>
        /// <param name="attributes">The attributes readed.</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns>AuthorizationType</returns>
        public AuthorizationType CheckAccess(string StoreName, string ApplicationName, string ItemName, IAzManDBUser dbUser, DateTime ValidFor, bool OperationsOnly, out List<KeyValuePair<string, string>> attributes, params KeyValuePair<string, object>[] contextParameters)
        {
            return this.internalCheckAccess(StoreName, ApplicationName, ItemName, dbUser, ValidFor, OperationsOnly, true, out attributes, contextParameters);
        }

        private AuthorizationType internalCheckAccess(string StoreName, string ApplicationName, string ItemName, IAzManDBUser dbUser, DateTime ValidFor, bool OperationsOnly, bool retrieveAttributes, out List<KeyValuePair<string, string>> attributes, params KeyValuePair<string, object>[] contextParameters)
        {
            int userGroupsCount = 0;
            SqlConnection conn = new SqlConnection(this.db.Connection.ConnectionString);
            conn.Open();
            System.Data.SqlClient.SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = "DirectCheckAccess";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@STORENAME", StoreName);
            cmd.Parameters.AddWithValue("@APPLICATIONNAME", ApplicationName);
            cmd.Parameters.AddWithValue("@ITEMNAME", ItemName);
            cmd.Parameters.AddWithValue("@OPERATIONSONLY", OperationsOnly);
            cmd.Parameters.AddWithValue("@TOKEN", dbUser.CustomSid.BinaryValue);
            cmd.Parameters.AddWithValue("@USERGROUPSCOUNT", (object)userGroupsCount);
            cmd.Parameters.AddWithValue("@VALIDFOR", ValidFor);
            cmd.Parameters.AddWithValue("@LDAPPATH", DirectoryServicesUtils.rootDsePath);
            cmd.Parameters.AddWithValue("@RETRIEVEATTRIBUTES", retrieveAttributes);
            System.Data.SqlClient.SqlParameter pAuthorizationType = new System.Data.SqlClient.SqlParameter("@AUTHORIZATION_TYPE", (object)0);
            pAuthorizationType.Direction = System.Data.ParameterDirection.InputOutput;
            cmd.Parameters.Add(pAuthorizationType);
            //BizRule Check CallBack
            SqlDataAdapter checkAccessPartialResultsDataAdapter = new SqlDataAdapter(cmd);
            DataSet checkAccessResults = new DataSet();
            try
            {
                checkAccessPartialResultsDataAdapter.Fill(checkAccessResults);
            }
            catch (SqlException sqlex)
            {
                if (sqlex.Message.StartsWith("Store not found", StringComparison.CurrentCultureIgnoreCase))
                {
                    throw SqlAzManException.StoreNotFoundException(StoreName, sqlex);
                }
                else if (sqlex.Message.StartsWith("Application not found", StringComparison.CurrentCultureIgnoreCase))
                {
                    throw SqlAzManException.ApplicationNotFoundException(ApplicationName, StoreName, sqlex);
                }
                else if (sqlex.Message.StartsWith("Item not found", StringComparison.CurrentCultureIgnoreCase))
                {
                    throw SqlAzManException.ItemNotFoundException(ItemName, StoreName, ApplicationName, sqlex);
                }
                else
                {
                    throw SqlAzManException.GenericException(sqlex);
                }
            }
            finally
            {
                conn.Close();
            }
            DataTable checkAccessPartialResultsDataTable = checkAccessResults.Tables[0];
            DataTable checkAccessAttributesDataTable = (retrieveAttributes ? checkAccessResults.Tables[1] : new DataTable());
            AuthorizationType result;
            if (checkAccessPartialResultsDataTable.Select("BizRuleId IS NOT NULL").Length == 0)
            {
                //No business rules to check ... just return check access authorizationType
                result = (AuthorizationType)((int)pAuthorizationType.Value);
            }
            else
            {
                //Transform DataRows into a Hierarchical Tree of Node(s)
                IAzManApplication application = this[StoreName][ApplicationName];
                ItemNode itemNode = this.buildTreeOfNodes(application, checkAccessPartialResultsDataTable, ItemName);
                //Execute Biz Rules and Cut Nodes that returns false
                Hashtable ctxParameters = new Hashtable();
                if (contextParameters != null)
                {
                    foreach (KeyValuePair<string, object> kv in contextParameters)
                    {
                        ctxParameters.Add(kv.Key, kv.Value);
                    }
                }
                itemNode = this.executeBizRules(itemNode, dbUser.CustomSid, ctxParameters);
                //Compute final CheckAccess authorizationType
                result = this.computeCheckAccessResult(itemNode, ref checkAccessAttributesDataTable);
                if (result == AuthorizationType.Deny || result == AuthorizationType.Neutral)
                {
                    checkAccessAttributesDataTable = new DataTable(); //no attributes
                }
            }
            if (retrieveAttributes)
            {
                //Populate Attributes authorizationType
                attributes = new List<KeyValuePair<string, string>>(checkAccessAttributesDataTable.Rows.Count);
                foreach (DataRow dr in checkAccessAttributesDataTable.Rows)
                {
                    KeyValuePair<string, string> kvp = new KeyValuePair<string, string>((string)dr[0], (string)dr[1]);
                    if (!attributes.Contains(kvp))
                    {
                        attributes.Add(kvp);
                    }
                }
            }
            else
            {
                attributes = new List<KeyValuePair<string,string>>();
            }
            return result;
        }

        internal bool executeBizRule(IAzManItem item, IAzManSid identity, Hashtable contextParameters, ref AuthorizationType forcedCheckAccessResult)
        {
            try
            {
                Assembly assembly = item.LoadBizRuleAssembly();
                Type bizRuleType = null;
                foreach (Type t in assembly.GetTypes())
                {
                    Type[] interfaces = t.FindInterfaces(new TypeFilter(
                        delegate(Type typeObj, Object criteriaObj)
                        {
                            if (typeObj.ToString() == criteriaObj.ToString())
                                return true;
                            else
                                return false;
                        }), typeof(NetSqlAzMan.Interfaces.IAzManBizRule).FullName);
                    if (interfaces.Length > 0)
                    {
                        bizRuleType = t;
                        break;
                    }
                }
                IAzManBizRule bizRule = (IAzManBizRule)bizRuleType.InvokeMember("cctor", BindingFlags.CreateInstance, null, bizRuleType, null);
                return bizRule.Execute(contextParameters, identity, item, ref forcedCheckAccessResult);
            }
            catch (Exception ex)
            {
                string msg = String.Format("Business Rule Error:{0}\r\nItem Name:{1}, Application Name: {2}, Store Name: {3}", ex.Message, item.Name, item.Application.Name, item.Application.Store.Name);
                throw new SqlAzManException(msg, ex);
            }
        }

        private AuthorizationType computeCheckAccessResult(ItemNode itemNode, ref DataTable checkAccessAttributes)
        {
            AuthorizationType result;
            if (itemNode == null)
            {
                result = AuthorizationType.Neutral;
            }
            else
            {
                result = itemNode.authorizationType;
                foreach (ItemNode parentItemNode in itemNode.parentItemNodes.Values)
                {
                    AuthorizationType resultFromParent = this.computeCheckAccessResult(parentItemNode, ref checkAccessAttributes);
                    if (resultFromParent == AuthorizationType.AllowWithDelegation)
                    {
                        resultFromParent = AuthorizationType.Allow;
                    }
                    result = SqlAzManItem.mergeAuthorizations(result, resultFromParent);
                    //Purge Attributes of Neutral/Deny Items
                    if (result == AuthorizationType.Neutral || result == AuthorizationType.Deny)
                    {
                        foreach (DataRow dr in checkAccessAttributes.Rows)
                        {
                            if (dr["ItemId"] != DBNull.Value && (int)dr["ItemId"] == itemNode.itemId)
                            {
                                dr.Delete();
                            }
                        }
                        checkAccessAttributes.AcceptChanges();
                    }
                }
            }
            return result;
        }

        internal ItemNode executeBizRules(ItemNode itemNode, IAzManSid identity, Hashtable contextParameters)
        {
            bool bizRuleResult = true;
            if (itemNode.bizRuleId.HasValue)
            {
                try
                {
                    AuthorizationType forcedCheckAccessResult = itemNode.authorizationType;
                    bizRuleResult = this.executeBizRule(itemNode.azmanItem, identity, contextParameters, ref forcedCheckAccessResult);
                    if (bizRuleResult)
                    {
                        itemNode.authorizationType = forcedCheckAccessResult;
                    }
                }
                catch (Exception ex)
                {
                    string msg = String.Format("Business Rule Error:{0}\r\nItem Name:{1}, Application Name: {2}, Store Name: {3}", ex.Message, itemNode.azmanItem.Name, itemNode.azmanItem.Application.Name, itemNode.azmanItem.Application.Store.Name);
                    throw new SqlAzManException(msg, ex);
                }
            }
            //Cut if false
            if (bizRuleResult == false)
            {
                itemNode.authorizationType = AuthorizationType.Neutral;
            }
            string[] keyNames = new string[itemNode.parentItemNodes.Keys.Count];
            itemNode.parentItemNodes.Keys.CopyTo(keyNames, 0);
            foreach (string keyName in keyNames)
            {
                ItemNode parentItemNode = (ItemNode)itemNode.parentItemNodes[keyName];
                if (this.executeBizRules(parentItemNode, identity, contextParameters) == null)
                {
                    itemNode.parentItemNodes.Remove(keyName);
                }
            }
            return itemNode;
        }

        private ItemNode buildTreeOfNodes(IAzManApplication application, DataTable checkAccessPartialResultsDataTable, string itemName)
        {
            DataRow dr = checkAccessPartialResultsDataTable.Select(String.Format("ItemName='{0}'", itemName.Replace("'", "''")))[0];
            int? bizRuleId = null;
            string bizRuleSource = null;
            byte? bizRuleLanguage = null;
            if (dr["BizRuleId"] != DBNull.Value)
            {
                bizRuleId = (int)dr["BizRuleId"];
                bizRuleSource = (string)dr["BizRuleSource"];
                bizRuleLanguage = (byte)dr["BizRuleLanguage"];
            }
            IAzManItem item = application.GetItem((string)dr["ItemName"]);
            ItemNode node = new ItemNode((int)dr["ItemId"], (string)dr["ItemName"], (ItemType)((byte)dr["ItemType"]), (AuthorizationType)((byte)dr["AuthorizationType"]), bizRuleId, bizRuleSource, bizRuleLanguage, item);
            IAzManItem[] parentItems = item.GetItemsWhereIAmAMember();
            foreach (IAzManItem parentItem in parentItems)
            {
                ItemNode parentItemNode = this.buildTreeOfNodes(application, checkAccessPartialResultsDataTable, parentItem.Name);
                node.parentItemNodes.Add(parentItem.Name, parentItemNode);
            }
            return node;
        }

        /// <summary>
        /// Checks the access in async way [ONLY FOR Windows Users].
        /// </summary>
        /// <param name="StoreName">Name of the store.</param>
        /// <param name="ApplicationName">Name of the application.</param>
        /// <param name="ItemName">Name of the itemName.</param>
        /// <param name="windowsIdentity">The windows identity. System.Security.Principal.WindowsIdentity.GetCurrent() for Windows Applications and (WindowsIdentity)HttpContext.Current.User.Identity or Page.Request.LogonUserIdentity for ASP.NET Applications.</param>
        /// <param name="ValidFor">The valid for.</param>
        /// <param name="OperationsOnly">if set to <c>true</c> [operations only].</param>
        /// <param name="callBack">The call back.</param>
        /// <param name="stateObject">The state object.</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns>AuthorizationType</returns>
        /// <remarks>
        /// 	<para>Remeber to: </para>
        /// 	<para>1) add "Asynchronous Processing=true" in the Storage Connection String</para>
        /// 	<para>2) Storage Connection must be manually opened and closed.</para>
        /// </remarks>
        public IAsyncResult BeginCheckAccess(string StoreName, string ApplicationName, string ItemName, WindowsIdentity windowsIdentity, DateTime ValidFor, bool OperationsOnly, AsyncCallback callBack, object stateObject, params KeyValuePair<string, object>[] contextParameters)
        {
            List<KeyValuePair<string, string>> attributes;
            return this.BeginCheckAccess(StoreName, ApplicationName, ItemName, windowsIdentity, ValidFor, OperationsOnly, callBack, stateObject, out attributes, contextParameters);
        }

        /// <summary>
        /// Checks the access in async way [ONLY FOR Windows Users].
        /// </summary>
        /// <param name="StoreName">Name of the store.</param>
        /// <param name="ApplicationName">Name of the application.</param>
        /// <param name="ItemName">Name of the itemName.</param>
        /// <param name="windowsIdentity">The windows identity. System.Security.Principal.WindowsIdentity.GetCurrent() for Windows Applications and (WindowsIdentity)HttpContext.Current.User.Identity or Page.Request.LogonUserIdentity for ASP.NET Applications.</param>
        /// <param name="ValidFor">The valid for.</param>
        /// <param name="OperationsOnly">if set to <c>true</c> [operations only].</param>
        /// <param name="callBack">The call back.</param>
        /// <param name="stateObject">The state object.</param>
        /// <param name="attributes">The attributes readed.</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns>AuthorizationType</returns>
        /// <remarks>
        /// 	<para>Remeber to: </para>
        /// 	<para>1) add "Asynchronous Processing=true" in the Storage Connection String</para>
        /// 	<para>2) Storage Connection must be manually opened and closed.</para>
        /// </remarks>
        public IAsyncResult BeginCheckAccess(string StoreName, string ApplicationName, string ItemName, WindowsIdentity windowsIdentity, DateTime ValidFor, bool OperationsOnly, AsyncCallback callBack, object stateObject, out List<KeyValuePair<string, string>> attributes, params KeyValuePair<string, object>[] contextParameters)
        {
            this.asyncCheckAccess = new AsyncCheckAccess(this.CheckAccess);
            return this.asyncCheckAccess.BeginInvoke(StoreName, ApplicationName, ItemName, windowsIdentity, ValidFor, OperationsOnly, out attributes, contextParameters, callBack, stateObject);
        }

        /// <summary>
        /// Checks the access in async way [ONLY FOR DB Users].
        /// </summary>
        /// <param name="StoreName">Name of the store.</param>
        /// <param name="ApplicationName">Name of the application.</param>
        /// <param name="ItemName">Name of the itemName.</param>
        /// <param name="dbUser">The db user.</param>
        /// <param name="ValidFor">The valid for.</param>
        /// <param name="OperationsOnly">if set to <c>true</c> [operations only].</param>
        /// <param name="callBack">The call back.</param>
        /// <param name="stateObject">The state object.</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns>AuthorizationType</returns>
        /// <remarks>
        /// 	<para>Remeber to: </para>
        /// 	<para>1) add "Asynchronous Processing=true" in the Storage Connection String</para>
        /// 	<para>2) Storage Connection must be manually opened and closed.</para>
        /// </remarks>
        public IAsyncResult BeginCheckAccess(string StoreName, string ApplicationName, string ItemName, IAzManDBUser dbUser, DateTime ValidFor, bool OperationsOnly, AsyncCallback callBack, object stateObject, params KeyValuePair<string, object>[] contextParameters)
        {
            List<KeyValuePair<string, string>> attributes;
            return this.BeginCheckAccess(StoreName, ApplicationName, ItemName, dbUser, ValidFor, OperationsOnly, callBack, stateObject, out attributes, contextParameters);
        }

        /// <summary>
        /// Checks the access in async way [ONLY FOR DB Users].
        /// </summary>
        /// <param name="StoreName">Name of the store.</param>
        /// <param name="ApplicationName">Name of the application.</param>
        /// <param name="ItemName">Name of the itemName.</param>
        /// <param name="dbUser">The db user.</param>
        /// <param name="ValidFor">The valid for.</param>
        /// <param name="OperationsOnly">if set to <c>true</c> [operations only].</param>
        /// <param name="callBack">The call back.</param>
        /// <param name="stateObject">The state object.</param>
        /// <param name="attributes">The attributes readed.</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns>AuthorizationType</returns>
        /// <remarks>
        /// 	<para>Remeber to: </para>
        /// 	<para>1) add "Asynchronous Processing=true" in the Storage Connection String</para>
        /// 	<para>2) Storage Connection must be manually opened and closed.</para>
        /// </remarks>
        public IAsyncResult BeginCheckAccess(string StoreName, string ApplicationName, string ItemName, IAzManDBUser dbUser, DateTime ValidFor, bool OperationsOnly, AsyncCallback callBack, object stateObject, out List<KeyValuePair<string, string>> attributes, params KeyValuePair<string, object>[] contextParameters)
        {
            this.asyncCheckAccessForDBUsers = new AsyncCheckAccessForDBUsers(this.CheckAccess);
            return this.asyncCheckAccessForDBUsers.BeginInvoke(StoreName, ApplicationName, ItemName, dbUser, ValidFor, OperationsOnly, out attributes, contextParameters, callBack, stateObject);
        }

        /// <summary>
        /// Ends the check access [FOR Windows Users ONLY].
        /// </summary>
        /// <param name="asyncResult">The async authorizationType.</param>
        /// <returns>AuthorizationType</returns>
        public AuthorizationType EndCheckAccess(IAsyncResult asyncResult)
        {
            List<KeyValuePair<string, string>> attributes;
            return this.asyncCheckAccess.EndInvoke(out attributes, asyncResult);
        }

        /// <summary>
        /// Ends the check access [FOR Windows Users ONLY].
        /// </summary>
        /// <param name="asyncResult">The async authorizationType.</param>
        /// <param name="attributes">The attributes readed.</param>
        /// <returns>AuthorizationType</returns>
        public AuthorizationType EndCheckAccess(IAsyncResult asyncResult, out List<KeyValuePair<string, string>> attributes)
        {
            return this.asyncCheckAccess.EndInvoke(out attributes, asyncResult);
        }
        /// <summary>
        /// Ends the check access [FOR DB Users ONLY].
        /// </summary>
        /// <param name="asyncResult">The async authorizationType.</param>
        /// <returns>AuthorizationType</returns>
        public AuthorizationType EndCheckAccessForDBUsers(IAsyncResult asyncResult)
        {
            List<KeyValuePair<string, string>> attributes;
            return this.asyncCheckAccessForDBUsers.EndInvoke(out attributes, asyncResult);
        }
        /// <summary>
        /// Ends the check access [FOR DB Users ONLY].
        /// </summary>
        /// <param name="asyncResult">The async authorizationType.</param>
        /// <param name="attributes">The attributes readed.</param>
        /// <returns>AuthorizationType</returns>
        public AuthorizationType EndCheckAccessForDBUsers(IAsyncResult asyncResult, out List<KeyValuePair<string, string>> attributes)
        {
            return this.asyncCheckAccessForDBUsers.EndInvoke(out attributes, asyncResult);
        }
        /// <summary>
        /// Gets a value indicating whether I am a NetSqlAzMan_Administrators member.
        /// </summary>
        /// <value><c>true</c> if [I am admin]; otherwise, <c>false</c>.</value>
        public bool IAmAdmin
        {
            get
            {
                if (!this.iamadmin.HasValue)
                {
                    this.iamadmin = this.db.IAmAdmin().Value;
                }
                return this.iamadmin.Value;
            }
        }
        #endregion IAzManStorage Members
        #region IAzManTransactable Members

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        public void BeginTransaction()
        {
            this.BeginTransaction(AzManIsolationLevel.ReadCommitted);
        }

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <param name="isolationLevel">The isolation level.</param>
        public void BeginTransaction(AzManIsolationLevel isolationLevel)
        {
            System.Data.IsolationLevel isoLevel;
            switch (isolationLevel)
            {
                case AzManIsolationLevel.ReadCommitted: isoLevel = System.Data.IsolationLevel.ReadCommitted; break;
                case AzManIsolationLevel.ReadUncommitted: isoLevel = System.Data.IsolationLevel.ReadUncommitted; break;
                case AzManIsolationLevel.RepeatableRead: isoLevel = System.Data.IsolationLevel.RepeatableRead; break;
                case AzManIsolationLevel.Snapshot: isoLevel = System.Data.IsolationLevel.Snapshot; break;
                default: isoLevel = System.Data.IsolationLevel.ReadCommitted; break;
            }
            this.OpenConnection();
            this.db.Transaction = this.db.Connection.BeginTransaction(isoLevel);
            this.transactionGuid = Guid.NewGuid();
            this.raiseTransactionBeginned();
            this.userTransaction = true;
        }

        /// <summary>
        /// Commits the transaction.
        /// </summary>
        public void CommitTransaction()
        {
            this.db.Transaction.Commit();
            this.raiseTransactionTerminated(true);
            this.transactionGuid = Guid.Empty;
            this.CloseConnection();
            this.userTransaction = false;
        }

        /// <summary>
        /// Rollback the transaction.
        /// </summary>
        public void RollBackTransaction()
        {
            if (this.TransactionInProgress)
            {
                this.db.Transaction.Rollback();
            }
            this.raiseTransactionTerminated(false);
            this.transactionGuid = Guid.Empty;
            this.CloseConnection();
            this.userTransaction = false;
        }

        /// <summary>
        /// Gets a value indicating whether [transaction in progress].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [transaction in progress]; otherwise, <c>false</c>.
        /// </value>
        public bool TransactionInProgress
        {
            get
            {
                if (this.db != null && this.db.Transaction != null && this.db.Transaction.Connection != null)
                     return true;
                else
                    return false;
            }
        }
        #endregion IAzManTransactable Members
        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.db.Dispose();
        }

        /// <summary>
        /// Gets the mode.
        /// </summary>
        /// <value>The mode.</value>
        public NetSqlAzManMode Mode
        {
            get
            {
                if (!this.mode.HasValue)
                {
                    //Mode
                    Settings s;
                    if ((s = (from t in this.db.Settings where t.SettingName == "Mode" select t).FirstOrDefault()) != null)
                    {
                        this.mode = (s.SettingValue == "Developer" ? NetSqlAzManMode.Developer : NetSqlAzManMode.Administrator);
                    }
                    else
                    {
                        this.mode = NetSqlAzManMode.Developer;
                        if (this.IAmAdmin) this.Mode = NetSqlAzManMode.Developer;
                    }
                }
                return this.mode.Value;
            }
            set
            {
                Settings s = null;
                if ((s = (from t in this.db.Settings where t.SettingName == "Mode" select t).FirstOrDefault()) != null)
                {
                    s.SettingValue = value.ToString();
                    this.db.SubmitChanges();
                    this.raiseNetSqlAzManModeChanged(this.mode.Value, value);
                }
                else
                {
                    this.db.Settings.InsertOnSubmit(new Settings { SettingName = "Mode", SettingValue = value.ToString() });
                    this.db.SubmitChanges();
                }
                this.mode = value;
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether [log errors].
        /// </summary>
        /// <value><c>true</c> if [log errors]; otherwise, <c>false</c>.</value>
        public bool LogErrors
        {
            get
            {
                if (!this.logErrors.HasValue)
                {
                    //Log Errors
                    Settings s;
                    if ((s = (from t in this.db.Settings where t.SettingName == "LogErrors" select t).FirstOrDefault()) != null)
                    {
                        this.logErrors = (s.SettingValue == "True");
                    }
                    else
                    {
                        this.logErrors = false;
                        if (this.IAmAdmin) this.LogErrors = false;
                    }

                }
                return this.logErrors.Value;
            }
            set
            {
                Settings s;
                if ((s = (from t in this.db.Settings where t.SettingName == "LogErrors" select t).FirstOrDefault()) != null)
                {
                    s.SettingValue = value ? "True" : "False";
                    this.db.SubmitChanges();
                }
                else
                {
                    this.db.Settings.InsertOnSubmit(new Settings() { SettingName = "LogErrors", SettingValue = value ? "True" : "False" });
                    this.db.SubmitChanges();
                }
                this.logErrors = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [log Warnings].
        /// </summary>
        /// <value><c>true</c> if [log Warnings]; otherwise, <c>false</c>.</value>
        public bool LogWarnings
        {
            get
            {
                if (!this.logWarnings.HasValue)
                {
                    //Log Warnings
                    Settings s;
                    if ((s = (from t in this.db.Settings where t.SettingName == "LogWarnings" select t).FirstOrDefault()) != null)
                    {
                        this.logWarnings = (s.SettingValue == "True");
                    }
                    else
                    {
                        this.logWarnings = false;
                        if (this.IAmAdmin) this.LogWarnings = false;
                    }
                }
                return this.logWarnings.Value;
            }
            set
            {
                Settings s;
                if ((s = (from t in this.db.Settings where t.SettingName == "LogWarnings" select t).FirstOrDefault()) != null)
                {
                    s.SettingValue = value ? "True" : "False";
                    this.db.SubmitChanges();
                }
                else
                {
                    this.db.Settings.InsertOnSubmit(new Settings() { SettingName = "LogWarnings", SettingValue = value ? "True" : "False" });
                    this.db.SubmitChanges();
                }
                this.logWarnings = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [log Informations].
        /// </summary>
        /// <value><c>true</c> if [log Informations]; otherwise, <c>false</c>.</value>
        public bool LogInformations
        {
            get
            {
                if (!this.logInformations.HasValue)
                {
                    //Log Informations
                    Settings s;
                    if ((s = (from t in this.db.Settings where t.SettingName == "LogInformations" select t).FirstOrDefault()) != null)
                    {
                        this.logInformations = (s.SettingValue == "True");
                    }
                    else
                    {
                        this.logInformations = false;
                        if (this.IAmAdmin) this.LogInformations = false;
                    }
                }
                return this.logInformations.Value;
            }
            set
            {
                Settings s;
                if ((s = (from t in this.db.Settings where t.SettingName == "LogInformations" select t).FirstOrDefault()) != null)
                {
                    s.SettingValue = value ? "True" : "False";
                    this.db.SubmitChanges();
                }
                else
                {
                    this.db.Settings.InsertOnSubmit(new Settings() { SettingName = "LogInformations", SettingValue = value ? "True" : "False" });
                    this.db.SubmitChanges();
                }
                this.logInformations = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [log on event log].
        /// </summary>
        /// <value><c>true</c> if [log on event log]; otherwise, <c>false</c>.</value>
        public bool LogOnEventLog
        {
            get
            {
                if (!this.logOnEventLog.HasValue)
                {
                    //Log On Event Log
                    Settings s;
                    if ((s = (from t in this.db.Settings where t.SettingName == "LogOnEventLog" select t).FirstOrDefault()) != null)
                    {
                        this.logOnEventLog = (s.SettingValue == "True");
                    }
                    else
                    {
                        this.logOnEventLog = false;
                        if (this.IAmAdmin) this.LogOnEventLog = false;
                    }
                }
                return this.logOnEventLog.Value;
            }
            set
            {
                Settings s;
                if ((s = (from t in this.db.Settings where t.SettingName == "LogOnEventLog" select t).FirstOrDefault()) != null)
                {
                    s.SettingValue = value ? "True" : "False";
                    this.db.SubmitChanges();
                }
                else
                {
                    this.db.Settings.InsertOnSubmit(new Settings() { SettingName = "LogOnEventLog", SettingValue = value ? "True" : "False" });
                    this.db.SubmitChanges();
                }
                this.logOnEventLog = value;
            }
        }
        /// <summary>
        /// Gets or sets a value indicating whether [log on db].
        /// </summary>
        /// <value><c>true</c> if [log on db]; otherwise, <c>false</c>.</value>
        public bool LogOnDb
        {
            get
            {
                if (!this.logOnDb.HasValue)
                {
                    //Log On Event Log
                    Settings s;
                    if ((s = (from t in this.db.Settings where t.SettingName == "LogOnDb" select t).FirstOrDefault()) != null)
                    {
                        this.logOnDb = (s.SettingValue == "True");
                    }
                    else
                    {
                        this.logOnDb = false;
                        if (this.IAmAdmin) this.LogOnDb = false;
                    }
                }
                return this.logOnDb.Value;
            }
            set
            {
                Settings s;
                if ((s = (from t in this.db.Settings where t.SettingName == "LogOnDb" select t).FirstOrDefault()) != null)
                {
                    s.SettingValue = value ? "True" : "False";
                    this.db.SubmitChanges();
                }
                else
                {
                    this.db.Settings.InsertOnSubmit(new Settings() { SettingName = "LogOnDb", SettingValue = value ? "True" : "False" });
                    this.db.SubmitChanges();
                }
                this.logOnDb = value;
            }
        }
        /// <summary>
        /// Gets the ENS (Event Notification System).
        /// </summary>
        /// <value>The ENS.</value>
        public IAzManENS ENS
        {
            get
            {
                return this.ens;
            }
        }
        #endregion
        #region IAzManImportExport Members

        /// <summary>
        /// Exports the specified XML writer.
        /// </summary>
        /// <param name="xmlWriter">The XML writer.</param>
        /// <param name="includeWindowsUsersAndGroups">if set to <c>true</c> [include windows users and groups].</param>
        /// <param name="includeDBUsers">if set to <c>true</c> [include DB users].</param>
        /// <param name="includeAuthorizations">if set to <c>true</c> [include authorizations].</param>
        /// <param name="ownerOfExport">The owner of export.</param>
        public void Export(System.Xml.XmlWriter xmlWriter, bool includeWindowsUsersAndGroups, bool includeDBUsers, bool includeAuthorizations, object ownerOfExport)
        {
            foreach (IAzManStore store in this.GetStores())
            {
                store.Export(xmlWriter, includeWindowsUsersAndGroups, includeDBUsers, includeAuthorizations, ownerOfExport);
            }
        }

        /// <summary>
        /// Imports the specified XML reader.
        /// </summary>
        /// <param name="xmlNode">The XML node.</param>
        /// <param name="includeWindowsUsersAndGroups">if set to <c>true</c> [include windows users and groups].</param>
        /// <param name="includeDBUsers">if set to <c>true</c> [include DB users].</param>
        /// <param name="includeAuthorizations">if set to <c>true</c> [include authorizations].</param>
        /// <param name="mergeOptions">The merge options.</param>
        public void ImportChildren(XmlNode xmlNode, bool includeWindowsUsersAndGroups, bool includeDBUsers, bool includeAuthorizations, SqlAzManMergeOptions mergeOptions)
        {
            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                if (node.Name == "Store")
                {
                    IAzManStore newStore = this.Stores.ContainsKey(node.Attributes["Name"].Value) ? this.Stores[node.Attributes["Name"].Value] : this.CreateStore(node.Attributes["Name"].Value, node.Attributes["Description"].Value);
                    newStore.ImportChildren(node, includeWindowsUsersAndGroups, includeDBUsers, includeAuthorizations, mergeOptions);
                }
            }
            this.stores = null; //Force refresh
        }

        #endregion
        #region DB Users
        /// <summary>
        /// Finds the DB user.
        /// </summary>
        /// <param name="customSid">The custom sid.</param>
        /// <returns></returns>
        public IAzManDBUser GetDBUser(IAzManSid customSid)
        {
            var dtDBUsers = this.db.GetDBUsers(null, null, customSid.BinaryValue, null);
            IAzManDBUser result;
            if (dtDBUsers.Count() == 0)
            {
                throw SqlAzManException.DBUserNotFoundException(customSid.StringValue, null);
            }
            else
            {
                result = new SqlAzManDBUser(new SqlAzManSID(dtDBUsers.First().DBUserSid.ToArray(), true), dtDBUsers.First().DBUserName);
            }
            return result;
        }
        /// <summary>
        /// Finds the DB user.
        /// </summary>
        /// <param name="userName">The custom sid.</param>
        /// <returns></returns>
        public IAzManDBUser GetDBUser(string userName)
        {
            var dtDBUsers = this.db.GetDBUsersEx(null, null, null, userName);
            IAzManDBUser result;
            if (dtDBUsers.Rows.Count == 0)
            {
                throw SqlAzManException.DBUserNotFoundException(userName, null);
            }
            else
            {
                result = new SqlAzManDBUser(dtDBUsers.Rows[0]);
            }
            return result;
        }
        /// <summary>
        /// Gets the DB users.
        /// </summary>
        /// <returns></returns>
        public IAzManDBUser[] GetDBUsers()
        {
            var dtDBUsers = this.db.GetDBUsersEx(null, null, null, null);
            IAzManDBUser[] result = new IAzManDBUser[dtDBUsers.Rows.Count];
            for (int i = 0; i < dtDBUsers.Rows.Count; i++)
            {
                result[i] = new SqlAzManDBUser(dtDBUsers.Rows[i]);
            }
            return result;
        }
        #endregion DB Users
        #region ENS Subscription
        private void SubscribeToENS()
        {
            if (this.ens != null)
            {
                this.ens.AddPublisher(this);
                this.ens.ApplicationAttributeCreated += new AttributeCreatedDelegate<IAzManApplication>(this.SqlAzManENS_ApplicationAttributeCreated);
                this.ens.ApplicationAttributeDeleted += new AttributeDeletedDelegate<IAzManApplication>(this.SqlAzManENS_ApplicationAttributeDeleted);
                this.ens.ApplicationAttributeUpdated += new AttributeUpdatedDelegate<IAzManApplication>(this.SqlAzManENS_ApplicationAttributeUpdated);
                this.ens.ApplicationCreated += new ApplicationCreatedDelegate(this.SqlAzManENS_ApplicationCreated);
                this.ens.ApplicationDeleted += new ApplicationDeletedDelegate(this.SqlAzManENS_ApplicationDeleted);
                this.ens.ApplicationGroupCreated += new ApplicationGroupCreatedDelegate(this.SqlAzManENS_ApplicationGroupCreated);
                this.ens.ApplicationGroupDeleted += new ApplicationGroupDeletedDelegate(this.SqlAzManENS_ApplicationGroupDeleted);
                this.ens.ApplicationGroupLDAPQueryUpdated += new ApplicationGroupLDAPQueryUpdatedDelegate(this.SqlAzManENS_ApplicationGroupLDAPQueryUpdated);
                this.ens.ApplicationGroupMemberCreated += new ApplicationGroupMemberCreatedDelegate(this.SqlAzManENS_ApplicationGroupMemberCreated);
                this.ens.ApplicationGroupMemberDeleted += new ApplicationGroupMemberDeletedDelegate(this.SqlAzManENS_ApplicationGroupMemberDeleted);
                this.ens.ApplicationGroupRenamed += new ApplicationGroupRenamedDelegate(this.SqlAzManENS_ApplicationGroupRenamed);
                this.ens.ApplicationGroupUpdated += new ApplicationGroupUpdatedDelegate(this.SqlAzManENS_ApplicationGroupUpdated);
                this.ens.ApplicationOpened += new ApplicationOpenedDelegate(this.SqlAzManENS_ApplicationOpened);
                this.ens.ApplicationRenamed += new ApplicationRenamedDelegate(this.SqlAzManENS_ApplicationRenamed);
                this.ens.ApplicationUpdated += new ApplicationUpdatedDelegate(this.SqlAzManENS_ApplicationUpdated);
                this.ens.AuthorizationAttributeCreated += new AttributeCreatedDelegate<IAzManAuthorization>(this.SqlAzManENS_AuthorizationAttributeCreated);
                this.ens.AuthorizationAttributeDeleted += new AttributeDeletedDelegate<IAzManAuthorization>(this.SqlAzManENS_AuthorizationAttributeDeleted);
                this.ens.AuthorizationAttributeUpdated += new AttributeUpdatedDelegate<IAzManAuthorization>(this.SqlAzManENS_AuthorizationAttributeUpdated);
                this.ens.AuthorizationCreated += new AuthorizationCreatedDelegate(this.SqlAzManENS_AuthorizationCreated);
                this.ens.AuthorizationDeleted += new AuthorizationDeletedDelegate(this.SqlAzManENS_AuthorizationDeleted);
                this.ens.AuthorizationUpdated += new AuthorizationUpdatedDelegate(this.SqlAzManENS_AuthorizationUpdated);
                this.ens.ItemAttributeCreated += new AttributeCreatedDelegate<IAzManItem>(this.SqlAzManENS_ItemAttributeCreated);
                this.ens.ItemAttributeDeleted += new AttributeDeletedDelegate<IAzManItem>(this.SqlAzManENS_ItemAttributeDeleted);
                this.ens.ItemAttributeUpdated += new AttributeUpdatedDelegate<IAzManItem>(this.SqlAzManENS_ItemAttributeUpdated);
                this.ens.ItemCreated += new ItemCreatedDelegate(this.SqlAzManENS_ItemCreated);
                this.ens.ItemDeleted += new ItemDeletedDelegate(this.SqlAzManENS_ItemDeleted);
                this.ens.ItemRenamed += new ItemRenamedDelegate(this.SqlAzManENS_ItemRenamed);
                this.ens.ItemUpdated += new ItemUpdatedDelegate(this.SqlAzManENS_ItemUpdated);
                this.ens.BizRuleUpdated += new BizRuleUpdatedDelegate(this.SqlAzManENS_BizRuleUpdated);
                this.ens.MemberAdded += new MemberAddedDelegate(this.SqlAzManENS_MemberAdded);
                this.ens.MemberRemoved += new MemberRemovedDelegate(this.SqlAzManENS_MemberRemoved);
                this.ens.StoreAttributeCreated += new AttributeCreatedDelegate<IAzManStore>(this.SqlAzManENS_StoreAttributeCreated);
                this.ens.StoreAttributeDeleted += new AttributeDeletedDelegate<IAzManStore>(this.SqlAzManENS_StoreAttributeDeleted);
                this.ens.StoreAttributeUpdated += new AttributeUpdatedDelegate<IAzManStore>(this.SqlAzManENS_StoreAttributeUpdated);
                this.ens.StoreCreated += new StoreCreatedDelegate(this.SqlAzManENS_StoreCreated);
                this.ens.StoreDeleted += new StoreDeletedDelegate(this.SqlAzManENS_StoreDeleted);
                this.ens.StoreGroupCreated += new StoreGroupCreatedDelegate(this.SqlAzManENS_StoreGroupCreated);
                this.ens.StoreGroupDeleted += new StoreGroupDeletedDelegate(this.SqlAzManENS_StoreGroupDeleted);
                this.ens.StoreGroupLDAPQueryUpdated += new StoreGroupLDAPQueryUpdatedDelegate(this.SqlAzManENS_StoreGroupLDAPQueryUpdated);
                this.ens.StoreGroupMemberCreated += new StoreGroupMemberCreatedDelegate(this.SqlAzManENS_StoreGroupMemberCreated);
                this.ens.StoreGroupMemberDeleted += new StoreGroupMemberDeletedDelegate(this.SqlAzManENS_StoreGroupMemberDeleted);
                this.ens.StoreGroupRenamed += new StoreGroupRenamedDelegate(this.SqlAzManENS_StoreGroupRenamed);
                this.ens.StoreGroupUpdated += new StoreGroupUpdatedDelegate(this.SqlAzManENS_StoreGroupUpdated);
                this.ens.StoreOpened += new StoreOpenedDelegate(this.SqlAzManENS_StoreOpened);
                this.ens.StoreRenamed += new StoreRenamedDelegate(this.SqlAzManENS_StoreRenamed);
                this.ens.StoreUpdated += new StoreUpdatedDelegate(this.SqlAzManENS_StoreUpdated);
                this.ens.TransactionBeggined += new TransactionBeginnedDelegate(this.SqlAzManENS_TransactionBeggined);
                this.ens.TransactionTerminated += new TransactionTerminatedDelegate(this.SqlAzManENS_TransactionTerminated);
                this.ens.NetSqlAzManModeChanged += new NetSqlAzManModeChangeDelegate(this.SqlAzManENS_NetSqlAzManModeChanged);
                this.ens.StorePermissionGranted += new StorePermissionGrantedDelegate(this.SqlAzManENS_StorePermissionGranted);
                this.ens.StorePermissionRevoked += new StorePermissionRevokedDelegate(this.SqlAzManENS_StorePermissionRevoked);
                this.ens.ApplicationPermissionGranted += new ApplicationPermissionGrantedDelegate(this.SqlAzManENS_ApplicationPermissionGranted);
                this.ens.ApplicationPermissionRevoked += new ApplicationPermissionRevokedDelegate(this.SqlAzManENS_ApplicationPermissionRevoked);
            }
        }

        void SqlAzManENS_ApplicationPermissionRevoked(IAzManApplication application, string sqlLogin, string role)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nApplication: {1}\r\nSql Login: {2}\r\nRole: {3}", "ApplicationPermissionRevoked", application.ToString(), sqlLogin, role));
        }

        void SqlAzManENS_ApplicationPermissionGranted(IAzManApplication application, string sqlLogin, string role)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nApplication: {1}\r\nSql Login: {2}\r\nRole: {3}", "ApplicationPermissionGranted", application.ToString(), sqlLogin, role));
        }

        void SqlAzManENS_StorePermissionRevoked(IAzManStore store, string sqlLogin, string role)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nStore: {1}\r\nSql Login: {2}\r\nRole: {3}", "StorePermissionRevoked", store.ToString(), sqlLogin, role));
        }

        void SqlAzManENS_StorePermissionGranted(IAzManStore store, string sqlLogin, string role)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nStore: {1}\r\nSql Login: {2}\r\nRole: {3}", "StorePermissionGranted", store.ToString(), sqlLogin, role));
        }

        void SqlAzManENS_ItemAttributeDeleted(IAzManItem owner, string key)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nItem: {1}\r\nAttribute deleted Key: {2}\r\n", "ItemAttributeDeleted", owner.ToString(), key));
        }

        void SqlAzManENS_StoreAttributeDeleted(IAzManStore owner, string key)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nStore: {1}\r\nAttribute deleted Key: {2}\r\n", "StoreAttributeDeleted", owner.ToString(), key));
        }

        void SqlAzManENS_ApplicationAttributeDeleted(IAzManApplication owner, string key)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nApplication: {1}\r\nAttribute deleted Key: {2}\r\n", "ApplicationAttributeDeleted", owner.ToString(), key));
        }

        void SqlAzManENS_ItemAttributeCreated(IAzManItem owner, IAzManAttribute<IAzManItem> attributeCreated)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nItem: {1}\r\nItem Attribute Created: {2}\r\n", "ItemAttributeCreated", owner.ToString(), attributeCreated.ToString()));
        }

        void SqlAzManENS_StoreAttributeCreated(IAzManStore owner, IAzManAttribute<IAzManStore> attributeCreated)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nStore: {1}\r\nStore Attribute Created: {2}\r\n", "StoreAttributeCreated", owner.ToString(), attributeCreated.ToString()));
        }

        void SqlAzManENS_ApplicationAttributeCreated(IAzManApplication owner, IAzManAttribute<IAzManApplication> attributeCreated)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nApplication: {1}\r\nApplication Attribute Created: {2}\r\n", "ApplicationAttributeCreated", owner.ToString(), attributeCreated.ToString()));
        }

        void SqlAzManENS_NetSqlAzManModeChanged(NetSqlAzManMode oldMode, NetSqlAzManMode newMode)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nOld Mode: {1}\r\nNew Mode: {2}\r\n", "NetSqlAzManModeChanged", oldMode, newMode));
        }

        void SqlAzManENS_TransactionTerminated(bool success)
        {
            if (success)
                logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\n", "TransactionTerminated with Success"));
            else
                logging.WriteWarning(this, String.Format("ENS Event: {0}\r\n\r\n", "TransactionTerminated with Failure"));
        }

        void SqlAzManENS_TransactionBeggined()
        {
            this.transactionGuid = Guid.NewGuid();
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\n", "TransactionBeggined"));
        }

        void SqlAzManENS_StoreUpdated(IAzManStore store, string oldDescription)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nStore: {1}\r\nOld Description: {2}\r\n", "StoreUpdated", store.ToString(), oldDescription));
        }

        void SqlAzManENS_StoreRenamed(IAzManStore store, string oldName)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nStore: {1}\r\nOld Name: {2}\r\n", "StoreRenamed", store.ToString(), oldName));
        }

        void SqlAzManENS_StoreOpened(IAzManStore store)
        {
            //logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nStore: {1}\r\n\r\n", "StoreOpened", store.ToString()));
        }

        void SqlAzManENS_StoreGroupUpdated(IAzManStoreGroup storeGroup, IAzManSid oldSid, string oldDescription, GroupType oldGroupType)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nStore Group: {1}\r\nOld SID: {2}\r\nOld Description: {3}\r\nOld Group Type: {4}\r\n", "StoreGroupUpdated", storeGroup.ToString(), oldSid, oldDescription, oldGroupType));
        }

        void SqlAzManENS_StoreGroupRenamed(IAzManStoreGroup storeGroup, string oldName)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nStore Group: {1}\r\nOld Name: {2}\r\n", "StoreGroupRenamed", storeGroup.ToString(), oldName));
        }

        void SqlAzManENS_StoreGroupMemberDeleted(IAzManStoreGroup ownerStoreGroup, IAzManSid sid)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nStore Group: {1}\r\nSID: {2}\r\n", "StoreGroupMemberDeleted", ownerStoreGroup.ToString(), sid));
        }

        void SqlAzManENS_StoreGroupMemberCreated(IAzManStoreGroup storeGroup, IAzManStoreGroupMember memberCreated)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nStore Group: {1}\r\nMember Created: {2}\r\n", "StoreGroupMemberCreated", storeGroup.ToString(), memberCreated.ToString()));
        }

        void SqlAzManENS_StoreGroupLDAPQueryUpdated(IAzManStoreGroup storeGroup, string oldLDapQuery)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nStore Group: {1}\r\nOld LDAP Query: {2}\r\n", "StoreGroupLDAPQueryUpdated", storeGroup.ToString(), oldLDapQuery));
        }

        void SqlAzManENS_StoreGroupDeleted(IAzManStore ownerStore, string storeGroupName)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nStore: {1}\r\nStore Group Name: {2}\r\n", "StoreGroupDeleted", ownerStore.ToString(), storeGroupName));
        }

        void SqlAzManENS_StoreGroupCreated(IAzManStore store, IAzManStoreGroup storeGroupCreated)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nStore: {1}\r\nStore Group Created: {2}\r\n", "StoreGroupCreated", store.ToString(), storeGroupCreated.ToString()));
        }

        void SqlAzManENS_StoreDeleted(IAzManStorage ownerStorage, string storeName)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nStore: {1}\r\n", "StoreDeleted", storeName));
        }

        void SqlAzManENS_StoreCreated(IAzManStore storeCreated)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nStore Created: {1}\r\n", "StoreCreated", storeCreated.ToString()));
        }

        void SqlAzManENS_MemberRemoved(IAzManItem item, IAzManItem memberRemoved)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nItem: {1}\r\nMember Removed: {2}\r\n", "MemberRemoved", item.ToString(), memberRemoved.ToString()));
        }

        void SqlAzManENS_MemberAdded(IAzManItem item, IAzManItem memberAdded)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nItem: {1}\r\nMember Added: {2}\r\n", "MemberAdded", item.ToString(), memberAdded.ToString()));
        }

        void SqlAzManENS_ItemUpdated(IAzManItem item, string oldDescription)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nItem: {1}\r\nOld Description: {2}\r\n", "ItemUpdated", item.ToString(), oldDescription));
        }

        void SqlAzManENS_BizRuleUpdated(IAzManItem item, string oldBizRule)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nItem: {1}\r\nOld Biz Rule: {2}\r\n", "BizRuleUpdated", item.ToString(), oldBizRule));
        }

        void SqlAzManENS_ItemRenamed(IAzManItem item, string oldName)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nItem: {1}\r\nOld Name: {2}\r\n", "ItemRenamed", item.ToString(), oldName));
        }

        void SqlAzManENS_ItemDeleted(IAzManApplication applicationContainer, string itemName, ItemType itemType)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nApplication Container: {1}\r\nItem Name: {2}\r\nItem Type: {3}\r\n", "ItemDeleted", applicationContainer.ToString(), itemName, itemType.ToString()));
        }

        void SqlAzManENS_ItemCreated(IAzManApplication application, IAzManItem itemCreated)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nApplication: {1}\r\nItem Created: {2}\r\n", "ItemCreated", application.ToString(), itemCreated.ToString()));
        }

        void SqlAzManENS_AuthorizationUpdated(IAzManAuthorization authorization, IAzManSid oldOwner, WhereDefined oldOwnerSidWhereDefined, IAzManSid oldSid, WhereDefined oldSidWhereDefined, AuthorizationType oldAuthorizationType, DateTime? oldValidFrom, DateTime? oldValidTo)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nAuthorization: {1}\r\nOld Owner SID: {2}\r\nOld Owner SID Where Defined: {3}\r\nOld SID: {4}\r\nOld SID Where Defined: {5}\r\nOld Authorization Type: {6}\r\nOld Valid From: {7}\r\nOld Valid To: {8}\r\n",
                "AuthorizationUpdated", authorization.ToString(), oldOwner.ToString(), oldOwnerSidWhereDefined, oldSid.ToString(), oldSidWhereDefined, oldAuthorizationType, (oldValidFrom.HasValue ? oldValidFrom.Value.ToString() : ""), (oldValidTo.HasValue ? oldValidTo.Value.ToString() : "")));
        }

        void SqlAzManENS_AuthorizationDeleted(IAzManItem ownerItem, IAzManSid owner, IAzManSid sid)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nOwner Item: {1}\r\nOwner SID: {2}\r\nSID: {3}\r\n", "AuthorizationDeleted", ownerItem.ToString(), owner.ToString(), sid.ToString()));
        }

        void SqlAzManENS_AuthorizationCreated(IAzManItem item, IAzManAuthorization authorizationCreated)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nItem: {1}\r\nAuthorization Created: {2}\r\n", "AuthorizationCreated", item.ToString(), authorizationCreated.ToString()));
        }

        void SqlAzManENS_AuthorizationAttributeUpdated(IAzManAttribute<IAzManAuthorization> attribute, string oldKey, string oldValue)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nAuthorization Attribute: {1}\r\nOld Key: {2}\r\nOld Value: {3}\r\n", "AuthorizationAttributeUpdated", attribute.ToString(), oldKey, oldValue));
        }

        void SqlAzManENS_StoreAttributeUpdated(IAzManAttribute<IAzManStore> attribute, string oldKey, string oldValue)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nStore Attribute: {1}\r\nOld Key: {2}\r\nOld Value: {3}\r\n", "StoreAttributeUpdated", attribute.ToString(), oldKey, oldValue));
        }

        void SqlAzManENS_ApplicationAttributeUpdated(IAzManAttribute<IAzManApplication> attribute, string oldKey, string oldValue)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nApplication Attribute: {1}\r\nOld Key: {2}\r\nOld Value: {3}\r\n", "ApplicationAttributeUpdated", attribute.ToString(), oldKey, oldValue));
        }

        void SqlAzManENS_ItemAttributeUpdated(IAzManAttribute<IAzManItem> attribute, string oldKey, string oldValue)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nItem Attribute: {1}\r\nOld Key: {2}\r\nOld Value: {3}\r\n", "ItemAttributeUpdated", attribute.ToString(), oldKey, oldValue));
        }

        void SqlAzManENS_AuthorizationAttributeDeleted(IAzManAuthorization owner, string key)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nAuthorization: {1}\r\nAttribute deleted Key: {2}\r\n", "AuthorizationAttributeDeleted", owner.ToString(), key));
        }

        void SqlAzManENS_AuthorizationAttributeCreated(IAzManAuthorization owner, IAzManAttribute<IAzManAuthorization> attributeCreated)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nAuthorization: {1}\r\nAuthorization Attribute Created: {2}\r\n", "AuthorizationAttributeCreated", owner.ToString(), attributeCreated.ToString()));
        }

        void SqlAzManENS_ApplicationUpdated(IAzManApplication application, string oldDescription)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nApplication: {1}\r\nOld Description: {2}\r\n", "ApplicationUpdated", application.ToString(), oldDescription));
        }

        void SqlAzManENS_ApplicationRenamed(IAzManApplication application, string oldName)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nApplication: {1}\r\nOld Name: {2}\r\n", "ApplicationRenamed", application.ToString(), oldName));
        }

        void SqlAzManENS_ApplicationOpened(IAzManApplication application)
        {
            //logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nApplication: {1}\r\n\r\n", "ApplicationOpened", application.ToString()));
        }

        void SqlAzManENS_ApplicationGroupUpdated(IAzManApplicationGroup applicationGroup, IAzManSid oldSid, string oldDescription, GroupType oldGroupType)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nApplication Group: {1}\r\nOld SID: {2}\r\nOld Description: {3}\r\nOld Group Type: {4}\r\n", "ApplicationGroupUpdated", applicationGroup.ToString(), oldSid, oldDescription, oldGroupType));
        }

        void SqlAzManENS_ApplicationGroupRenamed(IAzManApplicationGroup applicationGroup, string oldName)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nApplication Group: {1}\r\nOld Name: {2}\r\n", "ApplicationGroupRenamed(", applicationGroup.ToString(), oldName));
        }

        void SqlAzManENS_ApplicationGroupMemberDeleted(IAzManApplicationGroup ownerApplicationGroup, IAzManSid sid)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nApplication Group: {1}\r\nSID: {2}\r\n", "ApplicationGroupMemberDeleted", ownerApplicationGroup.ToString(), sid));
        }

        void SqlAzManENS_ApplicationGroupMemberCreated(IAzManApplicationGroup applicationGroup, IAzManApplicationGroupMember memberCreated)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nApplication Group: {1}\r\nMember Created: {2}\r\n", "ApplicationGroupMemberCreated", applicationGroup.ToString(), memberCreated.ToString()));
        }

        void SqlAzManENS_ApplicationGroupLDAPQueryUpdated(IAzManApplicationGroup applicationGroup, string oldLDapQuery)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nApplication Group: {1}\r\nOld LDAP Query: {2}\r\n", "ApplicationGroupLDAPQueryUpdated", applicationGroup.ToString(), oldLDapQuery));
        }

        void SqlAzManENS_ApplicationGroupDeleted(IAzManApplication ownerApplication, string applicationGroupName)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nApplication: {1}\r\nApplication Group Name: {2}\r\n", "ApplicationGroupDeleted", ownerApplication.ToString(), applicationGroupName));
        }

        void SqlAzManENS_ApplicationGroupCreated(IAzManApplication application, IAzManApplicationGroup applicationGroupCreated)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nApplication: {1}\r\nApplication Group Created: {2}\r\n", "ApplicationGroupCreated", application.ToString(), applicationGroupCreated.ToString()));
        }

        void SqlAzManENS_ApplicationDeleted(IAzManStore ownerStore, string applicationName)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nOwner Store: {1}\r\nApplication Name: {2}\r\n", "ApplicationDeleted", ownerStore.ToString(), applicationName));
        }

        void SqlAzManENS_ApplicationCreated(IAzManStore store, IAzManApplication applicationCreated)
        {
            logging.WriteInfo(this, String.Format("ENS Event: {0}\r\n\r\nOwner Store: {1}\r\nApplication Created: {2}\r\n", "ApplicationCreated", store.ToString(), applicationCreated));
        }
        #endregion ENS Subscription
    }
}
