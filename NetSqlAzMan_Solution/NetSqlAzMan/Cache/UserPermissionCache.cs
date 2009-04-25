using System;
using System.Threading;
using System.Security.Principal;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.LINQ;
using System.Linq;
using NetSqlAzMan.ENS;
using NetSqlAzMan.DirectoryServices;

namespace NetSqlAzMan.Cache
{
    /// <summary>
    /// UserPermissionCache class.
    /// </summary>
    [Serializable()]
    public sealed class UserPermissionCache
    {
        #region Fields
        private IAzManStorage storage;
        private string storeName;
        private string applicationName;
        private WindowsIdentity windowsIdentity;
        private IAzManDBUser dbUser;
        private ItemCheckAccessResult[] checkAccessTimeSlice;
        private KeyValuePair<string, object>[] contextParameters;
        private Dictionary<string, List<KeyValuePair<string, string>>> itemAttributes;
        private bool retrieveAttributes;
        private string[] items;
        private DataTable dtHierarchy;
        [NonSerialized()]
        private IEnumerable<BuildUserPermissionCacheResult2> dtAuthorizations;
        #endregion Fields
        #region Constructors
        private UserPermissionCache(IAzManStorage storage, string storeName, string applicationName, bool retrieveAttributes, params KeyValuePair<string, object>[] contextParameters)
        {
            this.storage = storage;
            this.storeName = storeName;
            this.applicationName = applicationName;
            this.contextParameters = contextParameters;
            this.retrieveAttributes = retrieveAttributes;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="UserPermissionCache"/> class.
        /// </summary>
        /// <param name="storage">The storage.</param>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="applicationName">Name of the application.</param>
        /// <param name="windowsIdentity">The windows identity.</param>
        /// <param name="retrieveAttributes">if set to <c>true</c> [retrieve attributes].</param>
        /// <param name="multiThreadBuild">if set to <c>true</c> [multi thread build].</param>
        /// <param name="contextParameters">The context parameters.</param>
        public UserPermissionCache(IAzManStorage storage, string storeName, string applicationName, WindowsIdentity windowsIdentity, bool retrieveAttributes, bool multiThreadBuild, params KeyValuePair<string, object>[] contextParameters)
            : this(storage, storeName, applicationName, retrieveAttributes, contextParameters)
        {
            this.windowsIdentity = windowsIdentity;
            if (multiThreadBuild)
                this.buildApplicationCacheMultiThread();
            else
                this.buildApplicationCache();
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="UserPermissionCache"/> class.
        /// </summary>
        /// <param name="storage">The storage.</param>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="applicationName">Name of the application.</param>
        /// <param name="dbUser">The db user.</param>
        /// <param name="retrieveAttributes">if set to <c>true</c> [retrieve attributes].</param>
        /// <param name="multiThreadBuild">if set to <c>true</c> [multi thread build].</param>
        /// <param name="contextParameters">The context parameters.</param>
        public UserPermissionCache(IAzManStorage storage, string storeName, string applicationName, IAzManDBUser dbUser, bool retrieveAttributes, bool multiThreadBuild, params KeyValuePair<string, object>[] contextParameters)
            : this(storage, storeName, applicationName, retrieveAttributes, contextParameters)
        {
            this.dbUser = dbUser;
            if (multiThreadBuild)
                this.buildApplicationCacheMultiThread();
            else
                this.buildApplicationCache();
        }
        #endregion Constructors
        #region Properties
        /// <summary>
        /// Gets Item names.
        /// </summary>
        /// <value>The items.</value>
        public string[] Items
        {
            get
            {
                return this.items;
            }
        }
        /// <summary>
        /// Gets the item attributes.
        /// </summary>
        /// <value>The item attributes.</value>
        public Dictionary<string, List<KeyValuePair<string, string>>> ItemAttributes
        {
            get { return this.itemAttributes; }
        }
        #endregion Properties
        #region Methods
        private void buildApplicationCacheMultiThread()
        {
            try
            {
                DateTime globalNow = DateTime.Now;
                this.storage.OpenConnection();
                this.collectPermissionData();
                List<ItemCheckAccessResult> results = new List<ItemCheckAccessResult>();
                IAzManSid sid = this.windowsIdentity != null ? new SqlAzManSID(this.windowsIdentity.User) : this.dbUser.CustomSid;
                List<ManualResetEvent> waitHandles = new List<ManualResetEvent>();
                Hashtable allResult = new Hashtable();
                int index = 0;
                Exception lastException = null;

                foreach (var drAuthorization in this.dtAuthorizations)
                {
                    
                    ManualResetEvent waitHandle = new ManualResetEvent(false);
                    waitHandles.Add(waitHandle);
                    //New Thread Pool
                    ThreadPool.QueueUserWorkItem(new WaitCallback(
                        delegate(object o)
                        {
                            IAzManStorage clonedStorage = new SqlAzManStorage(((SqlAzManStorage)this.storage).db.Connection.ConnectionString);
                            int localIndex = (int)((object[])o)[0];
                            ManualResetEvent localWaitHandle = (ManualResetEvent)((object[])o)[1];
                            BuildUserPermissionCacheResult2 localAuth = (BuildUserPermissionCacheResult2)((object[])o)[2];
                            DateTime now = (DateTime)((object[])o)[3];
                            string itemName = localAuth.ItemName;
                            try
                            {
                                clonedStorage.OpenConnection();
                                ItemCheckAccessResult result = new ItemCheckAccessResult(itemName);
                                result.ValidFrom = localAuth.ValidFrom.HasValue ? localAuth.ValidFrom.Value : DateTime.MinValue;
                                result.ValidTo = localAuth.ValidTo.HasValue? localAuth.ValidTo.Value : DateTime.MaxValue;
                                List<KeyValuePair<string, string>> attributes = null;
                                DateTime validFor = localAuth.ValidFrom.HasValue ? localAuth.ValidFrom.Value : now;
                                if (this.windowsIdentity != null)
                                {
                                    if (this.retrieveAttributes)
                                        result.AuthorizationType = clonedStorage.CheckAccess(this.storeName, this.applicationName, itemName, this.windowsIdentity, validFor, false, out attributes, this.contextParameters);
                                    else
                                        result.AuthorizationType = clonedStorage.CheckAccess(this.storeName, this.applicationName, itemName, this.windowsIdentity, validFor, false, this.contextParameters);

                                }
                                else if (this.dbUser != null)
                                {
                                    if (this.retrieveAttributes)
                                        result.AuthorizationType = clonedStorage.CheckAccess(this.storeName, this.applicationName, itemName, this.dbUser, validFor, false, out attributes, this.contextParameters);
                                    else
                                        result.AuthorizationType = clonedStorage.CheckAccess(this.storeName, this.applicationName, itemName, this.dbUser, validFor, false, this.contextParameters);
                                }
                                result.Attributes = attributes;
                                //Thread safety
                                lock (allResult.SyncRoot)
                                {
                                    allResult.Add(localIndex, new object[] { itemName, result });
                                }
                            }
                            catch (Exception ex)
                            {
                                lastException = ex;
                            }
                            finally
                            {
                                clonedStorage.CloseConnection();
                                localWaitHandle.Set();
                            }
                        }), new object[] { index, waitHandle, drAuthorization, globalNow });
                    index++;
                }
                if (lastException != null)
                    throw lastException;
                int count = index;
                //Wait for all threads: http://www.devnewsgroups.net/group/microsoft.public.dotnet.framework/topic28609.aspx
                if (Thread.CurrentThread.GetApartmentState() == ApartmentState.STA)
                {
                    // WaitAll for multiple handles on an STA thread is not supported.
                    // ...so wait on each handle individually.
                    foreach (ManualResetEvent myWaitHandle in waitHandles)
                    {
                        myWaitHandle.WaitOne();
                    }
                }
                else
                {
                    WaitHandle.WaitAll(waitHandles.ToArray());
                }
                //Extends all results
                index = 0;
                for (int i=0;i<count;i++)
                {
                    object[] values = (object[])allResult[index++];
                    string itemName = (string)((object[])values)[0];
                    ItemCheckAccessResult result = (ItemCheckAccessResult)((object[])values)[1];
                    results.Add(result);
                    this.extendResultToMembers(itemName, result, results);
                }
                this.checkAccessTimeSlice = results.ToArray();
            }
            finally
            {
                this.storage.CloseConnection();
            }
        }
        private void collectPermissionData()
        {
            var data = ((SqlAzManStorage)this.storage).db.BuildUserPermissionCache(this.storeName, this.applicationName);
            var hierarchy = (IEnumerable<BuildUserPermissionCacheResult1>)data.GetResult<BuildUserPermissionCacheResult1>();
            this.dtAuthorizations = (IEnumerable<BuildUserPermissionCacheResult2>)data.GetResult<BuildUserPermissionCacheResult2>().ToList();
            this.dtHierarchy = new DataTable();
            this.dtHierarchy.Columns.Add("ItemName", typeof(string));
            this.dtHierarchy.Columns.Add("ParentItemName", typeof(string));
            StringCollection items = new StringCollection();
            foreach (var dr in hierarchy)
            {
                DataRow row = this.dtHierarchy.NewRow();
                row["ItemName"] = dr.ItemName;
                row["ParentItemName"] = dr.ParentItemName;
                this.dtHierarchy.Rows.Add(row);
                string itemName = dr.ItemName;
                if (!items.Contains(itemName))
                {
                    items.Add(itemName);
                }
            }
            this.dtHierarchy.AcceptChanges();
            this.items = new string[items.Count];
            items.CopyTo(this.items, 0);
            //Item Owned Attributes
            this.itemAttributes = new Dictionary<string, List<KeyValuePair<string, string>>>();
            foreach (var item in this.storage[this.storeName][this.applicationName].Items)
            { 
                var ownedAttributes = (from t in item.Value.Attributes.Values select new KeyValuePair<string, string>(t.Key, t.Value)).ToList();
                this.itemAttributes.Add(item.Key, ownedAttributes);
            }
        }
        private void buildApplicationCache()
        {
            try
            {
                DateTime now = DateTime.Now;
                this.storage.OpenConnection();
                this.collectPermissionData();
                List<ItemCheckAccessResult> results = new List<ItemCheckAccessResult>();
                IAzManSid sid = this.windowsIdentity!=null ? new SqlAzManSID(this.windowsIdentity.User) : this.dbUser.CustomSid;
                int index = 0;
                foreach (var drAuthorization in this.dtAuthorizations)
                {
                    string itemName = drAuthorization.ItemName;
                    ItemCheckAccessResult result = new ItemCheckAccessResult(itemName);
                    result.ValidFrom = drAuthorization.ValidFrom.HasValue ? drAuthorization.ValidFrom.Value : DateTime.MinValue;
                    result.ValidTo = drAuthorization.ValidTo.HasValue ? drAuthorization.ValidTo.Value : DateTime.MaxValue;
                    List<KeyValuePair<string, string>> attributes = null;
                    DateTime validFor = drAuthorization.ValidFrom.HasValue ? drAuthorization.ValidFrom.Value : now;
                    if (this.windowsIdentity != null)
                    {
                        if (this.retrieveAttributes)
                            result.AuthorizationType = this.storage.CheckAccess(this.storeName, this.applicationName, itemName, this.windowsIdentity, validFor, false,  out attributes, this.contextParameters);
                        else
                            result.AuthorizationType = this.storage.CheckAccess(this.storeName, this.applicationName, itemName, this.windowsIdentity, validFor, false, this.contextParameters);
                    }
                    else if (this.dbUser != null)
                    {
                        if (this.retrieveAttributes)
                            result.AuthorizationType = this.storage.CheckAccess(this.storeName, this.applicationName, itemName, this.dbUser, validFor, false, out attributes, this.contextParameters);
                        else
                            result.AuthorizationType = this.storage.CheckAccess(this.storeName, this.applicationName, itemName, this.dbUser, validFor, false, this.contextParameters);
                    }
                    result.Attributes = attributes;
                    results.Add(result);
                    this.extendResultToMembers(itemName, result, results);
                    index++;
                }
                this.checkAccessTimeSlice = results.ToArray();
            }
            finally
            {
                this.storage.CloseConnection();
            }
        }
        private void extendResultToMembers(string itemName, ItemCheckAccessResult result, List<ItemCheckAccessResult> results)
        {
            foreach (DataRow member in this.dtHierarchy.Select(String.Format("ParentItemName='{0}'", itemName.Replace("'", "''"))))
            {
                string memberName = (string)member["ItemName"];
                ItemCheckAccessResult memberItemCheckAccessResult = result.ClonedForItem(memberName);
                results.Add(memberItemCheckAccessResult);
                if (result.AuthorizationType == AuthorizationType.Allow || result.AuthorizationType == AuthorizationType.AllowWithDelegation)
                { 
                    //Add my attributes to my members
                    var itemAttributes = result.Attributes;
                    foreach (var itemAttribute in itemAttributes)
                    {
                        if (!memberItemCheckAccessResult.Attributes.Contains(itemAttribute))
                        {
                            memberItemCheckAccessResult.Attributes.Add(itemAttribute);
                        }
                    }
                }
                this.extendResultToMembers(memberName, result, results);
            }
        }
        /// <summary>
        /// Checks the access.
        /// </summary>
        /// <param name="itemName">Name of the operation.</param>
        /// <param name="validFor">The valid for.</param>
        /// <param name="attributes">The attributes.</param>
        /// <returns></returns>
        public AuthorizationType CheckAccess(string itemName, DateTime validFor, out List<KeyValuePair<string, string>> attributes)
        {
            AuthorizationType result = AuthorizationType.Neutral;
            attributes = new List<KeyValuePair<string,string>>();
            bool allowBecomesAllowWithDelegation = false;
            foreach (ItemCheckAccessResult ca in this.checkAccessTimeSlice)
            {
                if (String.Compare(ca.ItemName,itemName, true)==0)
                {
                    if (validFor >= ca.ValidFrom && validFor <= ca.ValidTo)
                    {
                        if (ca.AuthorizationType == AuthorizationType.AllowWithDelegation && !ca.Inherited)
                            allowBecomesAllowWithDelegation = true;
                        result = SqlAzManItem.mergeAuthorizations(result, ca.AuthorizationType);
                        //Inherited Attributes
                        if (this.retrieveAttributes)
                        {
                            foreach (KeyValuePair<string, string> kvp in ca.Attributes)
                            {
                                if (!attributes.Contains(new KeyValuePair<string, string>(kvp.Key, kvp.Value)))
                                {
                                    attributes.Add(kvp);
                                }
                            }
                        }
                        //Owner Attributes
                        if (this.retrieveAttributes && (result == AuthorizationType.AllowWithDelegation || result == AuthorizationType.Allow))
                        {
                            foreach (KeyValuePair<string, string> kvp in this.itemAttributes[itemName])
                            {
                                if (!attributes.Contains(new KeyValuePair<string, string>(kvp.Key, kvp.Value)))
                                {
                                    attributes.Add(kvp);
                                }
                            }
                        }
                    }
                }
            }
            if (result == AuthorizationType.Allow && allowBecomesAllowWithDelegation)
                result = AuthorizationType.AllowWithDelegation;
            return result;
        }
        /// <summary>
        /// Checks the access.
        /// </summary>
        /// <param name="itemName">Name of the itemName.</param>
        /// <param name="validFor">The valid for.</param>
        /// <returns></returns>
        public AuthorizationType CheckAccess(string itemName, DateTime validFor)
        {
            List<KeyValuePair<string, string>> attributes;
            return this.CheckAccess(itemName, validFor, out attributes);
        }
        #endregion Methods
    }
}
