using System;
using System.Security.Principal;
using System.Xml;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.LINQ;
using System.Linq;
using NetSqlAzMan.ENS;
using NetSqlAzMan.DirectoryServices;
using NetSqlAzMan.Utilities;
using System.Runtime.Serialization;

namespace NetSqlAzMan
{
    /// <summary>
    /// Represents an AzManApplication stored on Sql Server.
    /// </summary>
    [Serializable()]
    [DataContract(Namespace = "http://NetSqlAzMan/ServiceModel", IsReference = true)]
    public sealed partial class SqlAzManApplication : IAzManApplication
    {
        #region Fields
        [NonSerialized()]
        private NetSqlAzManStorageDataContext db;
        private int applicationId;
        private IAzManStore store;
        private string name;
        private string description;
        private byte netsqlazmanFixedServerRole;
        [NonSerialized()]
        private SqlAzManENS ens;
        internal Dictionary<string, IAzManItem> items;
        internal Dictionary<string, IAzManApplicationGroup> applicationGroups;
        internal Dictionary<string, IAzManAttribute<IAzManApplication>> attributes;
        #endregion Fields
        #region Events
        /// <summary>
        /// Occurs after a SqlAzManApplication object has been Deleted.
        /// </summary>
        public event ApplicationDeletedDelegate ApplicationDeleted;
        /// <summary>
        /// Occurs after a SqlAzManApplication object has been Updated.
        /// </summary>
        public event ApplicationUpdatedDelegate ApplicationUpdated;
        /// <summary>
        /// Occurs after a SqlAzManApplication object has been Renamed.
        /// </summary>
        public event ApplicationRenamedDelegate ApplicationRenamed;
        /// <summary>
        /// Occurs after an Application Group object has been Created.
        /// </summary>
        public event ApplicationGroupCreatedDelegate ApplicationGroupCreated;
        /// <summary>
        /// Occurs after an Item object has been Created.
        /// </summary>
        public event ItemCreatedDelegate ItemCreated;
        /// <summary>
        /// Occurs after an Attribute object has been Created.
        /// </summary>
        public event AttributeCreatedDelegate<IAzManApplication> ApplicationAttributeCreated;
        /// <summary>
        /// Occurs after a SQL Login is Granted on the Application.
        /// </summary>
        public event ApplicationPermissionGrantedDelegate ApplicationPermissionGranted;
        /// <summary>
        /// Occurs after a SQL Login is Revoked on the Application.
        /// </summary>
        public event ApplicationPermissionRevokedDelegate ApplicationPermissionRevoked;
        #endregion Events
        #region Private Event Raisers
        private void raiseDeleted(IAzManStore ownerStore, string applicationName)
        {
            if (this.ApplicationDeleted != null)
                this.ApplicationDeleted(ownerStore, applicationName);
        }
        private void raiseUpdated(IAzManApplication application, string oldDescription)
        {
            if (this.ApplicationUpdated != null)
                this.ApplicationUpdated(application, oldDescription);
        }
        private void raiseRenamed(IAzManApplication application, string oldName)
        {
            if (this.ApplicationRenamed != null)
                this.ApplicationRenamed(application, oldName);
        }
        private void raiseApplicationGroupCreated(IAzManApplication application, IAzManApplicationGroup applicationGroupCreated)
        {
            if (this.ApplicationGroupCreated != null)
                this.ApplicationGroupCreated(application, applicationGroupCreated);
        }
        private void raiseItemCreated(IAzManApplication application, IAzManItem itemCreated)
        {
            if (this.ItemCreated != null)
                this.ItemCreated(application, itemCreated);
        }
        private void raiseApplicationAttributeCreated(IAzManApplication owner, IAzManAttribute<IAzManApplication> attributeCreated)
        {
            if (this.ApplicationAttributeCreated != null)
                this.ApplicationAttributeCreated(owner, attributeCreated);
        }
        private void raiseApplicationPermissionGranted(IAzManApplication owner, string sqlLogin, string role)
        {
            if (this.ApplicationPermissionGranted != null)
                this.ApplicationPermissionGranted(owner, sqlLogin, role);
        }
        private void raiseApplicationPermissionRevoked(IAzManApplication owner, string sqlLogin, string role)
        {
            if (this.ApplicationPermissionRevoked != null)
                this.ApplicationPermissionRevoked(owner, sqlLogin, role);
        }
        #endregion Private Event Raisers
        #region Constructors
        internal SqlAzManApplication(NetSqlAzManStorageDataContext db, IAzManStore store, int applicationId, string name, string description, byte netsqlazmanFixedServerRole, SqlAzManENS ens)
        {
            this.db = db;
            this.applicationId = applicationId;
            this.store = store;
            this.name = name;
            this.description = description;
            this.netsqlazmanFixedServerRole = netsqlazmanFixedServerRole;
            this.ens = ens;
        }
        #endregion Constructors
        #region IAzManApplication Members
        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>The attributes.</value>
        public Dictionary<string, IAzManAttribute<IAzManApplication>> Attributes
        {
            get
            {
                if (this.attributes == null)
                {
                    this.attributes = new Dictionary<string, IAzManAttribute<IAzManApplication>>();
                    foreach (IAzManAttribute<IAzManApplication> i in this.GetAttributes())
                    {
                        this.attributes.Add(i.Key, i);
                    }
                }
                return this.attributes;
            }
        }
        /// <summary>
        /// Gets the application groups.
        /// </summary>
        /// <value>The application groups.</value>
        public Dictionary<string, IAzManApplicationGroup> ApplicationGroups
        {
            get
            {
                if (this.applicationGroups == null)
                {
                    this.applicationGroups = new Dictionary<string, IAzManApplicationGroup>();
                    foreach (IAzManApplicationGroup a in this.GetApplicationGroups())
                    {
                        this.applicationGroups.Add(a.Name, a);
                    }
                }
                return this.applicationGroups;
            }
        }
        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>The items.</value>
        public Dictionary<string, IAzManItem> Items
        {
            get
            {
                if (this.items == null)
                {
                    this.GetItems();
                }
                return this.items;
            }
        }
        /// <summary>
        /// Gets the application id.
        /// </summary>
        /// <value>The application id.</value>
        int IAzManApplication.ApplicationId
        {
            get
            {
                return this.applicationId;
            }
        }

        /// <summary>
        /// Gets the store.
        /// </summary>
        /// <value>The store.</value>
        public IAzManStore Store
        {
            get
            {
                return this.store;
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        public string Name
        {
            get
            {
                return this.name;
            }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get
            {
                return this.description;
            }
        }
        /// <summary>
        /// Updates the specified application description.
        /// </summary>
        /// <param name="newApplicationDescription">The new application description.</param>
        public void Update(string newApplicationDescription)
        {
            if (this.description != newApplicationDescription)
            {
                string oldDescription = this.description;
                this.db.ApplicationUpdate(this.name, newApplicationDescription, this.applicationId);
                this.description = newApplicationDescription;
                this.raiseUpdated(this, oldDescription);
            }
        }

        /// <summary>
        /// Renames application name with the specified new application name.
        /// </summary>
        /// <param name="newApplicationName">New name of the application.</param>
        public void Rename(string newApplicationName)
        {
            if (this.name != newApplicationName)
            {
                string oldName = this.name;
                try
                {
                    this.db.ApplicationUpdate(newApplicationName, this.description, this.applicationId);
                    this.name = newApplicationName;
                    this.raiseRenamed(this, oldName);
                }
                catch (System.Data.SqlClient.SqlException sqlex)
                {
                    if (sqlex.Number == 2601) //Index Duplicate Error
                        throw new SqlAzManApplicationException(this, "An Application with the same name already exists.");
                    else
                        throw sqlex;
                }
            }
        }

        /// <summary>
        /// Deletes this application.
        /// </summary>
        public void Delete()
        {
            this.db.ApplicationDelete(this.store.StoreId, this.applicationId);
            this.raiseDeleted(this.store, this.name);
        }
        /// <summary>
        /// Creates the itemName.
        /// </summary>
        /// <param name="itemName">Name of the itemName.</param>
        /// <param name="itemDescription">The itemName description.</param>
        /// <param name="itemType">Type of the itemName.</param>
        /// <returns></returns>
        public IAzManItem CreateItem(string itemName, string itemDescription, ItemType itemType)
        {
            try
            {
                int id = this.db.ItemInsert(itemName, itemDescription, (byte)itemType, null, this.applicationId);
                IAzManItem itemCreated = new SqlAzManItem(this.db, this, id, itemName, itemDescription, itemType, String.Empty, null, this.ens);
                this.raiseItemCreated(this, itemCreated);
                this.ens.AddPublisher(itemCreated);
                //Update cached items
                if (this.items!=null && !this.items.ContainsKey(itemCreated.Name))
                    this.items.Add(itemCreated.Name, itemCreated);
                return itemCreated;
            }
            catch (System.Data.SqlClient.SqlException sqlex)
            {
                if (sqlex.Number == 2601) //Index Duplicate Error
                    throw new SqlAzManApplicationException(this, "An Item with the same name already exists.");
                else
                    throw sqlex;
            }
        }

        /// <summary>
        /// Gets the <see cref="T:IAzManItem"/> with the specified itemName name.
        /// </summary>
        /// <value></value>
        public IAzManItem GetItem(string itemName)
        {
            ItemsResult items;
            if ((items = (from t in this.db.Items() where t.Name == itemName && t.ApplicationId == this.applicationId select t).FirstOrDefault())!=null)
            {
                int itemId = items.ItemId.Value;
                string name = items.Name;
                string description = items.Description;
                ItemType itemType = (ItemType)items.ItemType.Value;
                string bizRule = String.Empty;
                NetSqlAzMan.BizRuleSourceLanguage? bizRuleScriptLanguage = null;
                if (items.BizRuleId.HasValue)
                {
                    var bizrule = (from br in this.db.BizRules()
                                  where br.BizRuleId == items.BizRuleId.Value
                                  select br).First();
                    bizRule = bizrule.BizRuleSource;
                    bizRuleScriptLanguage = (NetSqlAzMan.BizRuleSourceLanguage)bizrule.BizRuleLanguage.Value;
                }
                IAzManItem result = new SqlAzManItem(this.db, this, itemId, name, description, itemType, bizRule, bizRuleScriptLanguage, this.ens);
                this.ens.AddPublisher(result);
                return result;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// Determines whether [has child items].
        /// </summary>
        /// <param name="itemType">Type of the itemName.</param>
        /// <returns>
        /// 	<c>true</c> if [has child items]; otherwise, <c>false</c>.
        /// </returns>
        public bool HasItems(ItemType itemType)
        {

            return this.db.Items().Any(p=>p.ApplicationId == this.applicationId && p.ItemType.Value == (byte)itemType);
        }

        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <returns></returns>
        public IAzManItem[] GetItems()
        {
            IAzManItem[] items;
            var ds = from tf in this.db.Items()
                      where tf.ApplicationId == this.applicationId
                      orderby tf.Name
                      select tf;
            items = new SqlAzManItem[ds.Count()];
            int index = 0;
            this.items = new Dictionary<string, IAzManItem>();
            foreach (var row in ds)
            {
                string bizRule = String.Empty;
                NetSqlAzMan.BizRuleSourceLanguage? bizRuleScriptLanguage = null;
                if (row.BizRuleId.HasValue)
                {
                    var bizrule = (from tf in this.db.BizRules()
                                   where tf.BizRuleId == row.BizRuleId
                                   select tf).First();
                    bizRule = bizrule.BizRuleSource;
                    bizRuleScriptLanguage = (NetSqlAzMan.BizRuleSourceLanguage)bizrule.BizRuleLanguage.Value;
                }
                items[index] = new SqlAzManItem(this.db, this, row.ItemId.Value, row.Name, row.Description, (ItemType)row.ItemType.Value, bizRule, bizRuleScriptLanguage, this.ens);
                this.items.Add(items[index].Name, items[index]);
                this.ens.AddPublisher(items[index]);
                index++;
            }
            //Members
            var dt = from v in this.db.ItemsHierarchyView
                     where v.ApplicationId == this.applicationId
                     select v;
            foreach (IAzManItem item in this.items.Values)
            {
                ((SqlAzManItem)item).members = new Dictionary<string, IAzManItem>();
                foreach (var row in dt.Where<ItemsHierarchyView>(p => p.Name == item.Name))
                {
                    IAzManItem member = this.items[row.MemberName];
                    ((SqlAzManItem)item).members.Add(member.Name, member);
                    //Items Where Im a member
                    if (((SqlAzManItem)member).itemsWhereIAmAMember==null)
                    {
                        ((SqlAzManItem)member).itemsWhereIAmAMember = new Dictionary<string,IAzManItem>();
                    }
                    if (!(((SqlAzManItem)member).itemsWhereIAmAMember.ContainsKey(item.Name)))
                    {
                        ((SqlAzManItem)member).itemsWhereIAmAMember.Add(item.Name, item);
                    }
                }
            }
            return items;
        }
        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <param name="itemType">Type of the itemName.</param>
        /// <returns></returns>
        public IAzManItem[] GetItems(ItemType itemType)
        {
            IAzManItem[] items;
            var ds = from tf in this.db.Items()
                     where tf.ApplicationId == this.applicationId && tf.ItemType.Value == (byte)itemType
                     orderby tf.Name
                     select tf;
            items = new SqlAzManItem[ds.Count()];
            int index = 0;
            foreach (var row in ds)
            {
                string bizRule = String.Empty;
                NetSqlAzMan.BizRuleSourceLanguage? bizRuleScriptLanguage = null;
                if (row.BizRuleId.HasValue)
                {
                    var bizrule = (from tf in this.db.BizRules()
                                  where tf.BizRuleId == row.BizRuleId.Value
                                  select tf).First();
                    bizRule = bizrule.BizRuleSource;
                    bizRuleScriptLanguage = (NetSqlAzMan.BizRuleSourceLanguage)bizrule.BizRuleLanguage.Value;
                }
                items[index] = new SqlAzManItem(this.db, this, row.ItemId.Value, row.Name, row.Description, (ItemType)row.ItemType, bizRule, bizRuleScriptLanguage, this.ens);
                this.ens.AddPublisher(items[index]);
                index++;
            }
            return items;
        }
        /// <summary>
        /// Creates the application group.
        /// </summary>
        /// <param name="sid">The object owner.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="lDapQuery">The ldap query.</param>
        /// <param name="groupType">Type of the group.</param>
        /// <returns></returns>
        public IAzManApplicationGroup CreateApplicationGroup(IAzManSid sid, string name, string description, string lDapQuery, GroupType groupType)
        {
           try
            {
                if (DirectoryServices.DirectoryServicesUtils.TestLDAPQuery(lDapQuery))
                {
                    this.db.ApplicationGroupInsert(this.applicationId, sid.BinaryValue, name, description, lDapQuery, (byte)groupType);
                    IAzManApplicationGroup applicationGroupCreated = this.GetApplicationGroup(name);
                    this.raiseApplicationGroupCreated(this, applicationGroupCreated);
                    this.ens.AddPublisher(applicationGroupCreated);
                    this.applicationGroups = null; //Force cache refresh
                    return applicationGroupCreated;
                }
                else
                {
                    throw new ArgumentException("LDAP Query syntax error or unavailable Domain.", "lDapQuery");
                }
            }
            catch (System.Data.SqlClient.SqlException sqlex)
            {
                if (sqlex.Number == 2601) //Index Duplicate Error
                    throw new SqlAzManApplicationException(this, "An Application Group with the same name already exists.");
                else
                    throw sqlex;
            }
        }

        /// <summary>
        /// Determines whether [has application groups].
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [has application groups]; otherwise, <c>false</c>.
        /// </returns>
        public bool HasApplicationGroups()
        {

            return this.db.ApplicationGroups().Any(p=>p.ApplicationId == this.applicationId);
        }

        /// <summary>
        /// Gets the application groups.
        /// </summary>
        /// <returns></returns>
        public IAzManApplicationGroup[] GetApplicationGroups()
        {
            var ds = from tf in this.db.ApplicationGroups()
                     where tf.ApplicationId == this.applicationId
                     orderby tf.Name
                     select tf;
            int index = 0;
            IAzManApplicationGroup[] applicationGroups = new SqlAzManApplicationGroup[ds.Count()];
            foreach (var row in ds)
            {
                applicationGroups[index] = new SqlAzManApplicationGroup(this.db, this, row.ApplicationGroupId.Value, new SqlAzManSID(row.ObjectSid.ToArray()), row.Name, row.Description, row.LDapQuery, (GroupType)row.GroupType.Value, this.ens);
                this.ens.AddPublisher(applicationGroups[index]);
                index++;
            }
            return applicationGroups;
        }

        /// <summary>
        /// Gets the application group.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public IAzManApplicationGroup GetApplicationGroup(string name)
        {
            ApplicationGroupsResult agr;
            if ((agr = (from t in this.db.ApplicationGroups() where t.Name == name && t.ApplicationId == this.applicationId select t).FirstOrDefault())!=null)
            {
                int applicationGroupid = agr.ApplicationGroupId.Value;
                IAzManSid objectSid = new SqlAzManSID(agr.ObjectSid.ToArray());
                string description = agr.Description;
                string lDapQuery = agr.LDapQuery;
                GroupType groupType = (GroupType)agr.GroupType.Value;

                IAzManApplicationGroup result = new SqlAzManApplicationGroup(this.db, this, applicationGroupid, objectSid, name, description, lDapQuery, groupType, this.ens);
                this.ens.AddPublisher(result);
                return result;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Gets the application group.
        /// </summary>
        /// <param name="sid">The object owner.</param>
        /// <returns></returns>
        public IAzManApplicationGroup GetApplicationGroup(IAzManSid sid)
        {
            ApplicationGroupsResult agr = null;
            if ((agr = (from t in this.db.ApplicationGroups() where t.ObjectSid == sid.BinaryValue && t.ApplicationId == this.applicationId select t).FirstOrDefault())!=null)
            {
                IAzManApplicationGroup result = this.GetApplicationGroup(agr.Name);
                this.ens.AddPublisher(result);
                return result;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Gets the application attributes.
        /// </summary>
        /// <returns></returns>
        public IAzManAttribute<IAzManApplication>[] GetAttributes()
        {

            IAzManAttribute<IAzManApplication>[] attributes;
            var ds = from tf in this.db.ApplicationAttributes()
                     where tf.ApplicationId == this.applicationId
                     select tf;
            attributes = new SqlAzManApplicationAttribute[ds.Count()];
            int index = 0;
            foreach (var row in ds)
            {
                attributes[index] = new SqlAzManApplicationAttribute(this.db, this, row.ApplicationAttributeId.Value, row.AttributeKey, row.AttributeValue, this.ens);
                this.ens.AddPublisher(attributes[index]);
                index++;
            }
            return attributes;
        }

        /// <summary>
        /// Gets the application attribute.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public IAzManAttribute<IAzManApplication> GetAttribute(string key)
        {
            ApplicationAttributesResult aa;
            if ((aa = (from t in this.db.ApplicationAttributes() where t.ApplicationId == this.applicationId && t.AttributeKey == key select t).FirstOrDefault())!=null)
            {
                IAzManAttribute<IAzManApplication> result = new SqlAzManApplicationAttribute(this.db, this, aa.ApplicationAttributeId.Value, aa.AttributeKey, aa.AttributeValue, this.ens);
                this.ens.AddPublisher(result);
                return result;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Creates an application attribute.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public IAzManAttribute<IAzManApplication> CreateAttribute(string key, string value)
        {
            try
            {
                int applicationAttributeId = this.db.ApplicationAttributeInsert(this.applicationId, key, value);
                IAzManAttribute<IAzManApplication> result = new SqlAzManApplicationAttribute(this.db, this, applicationAttributeId, key, value, this.ens);
                this.raiseApplicationAttributeCreated(this, result);
                this.ens.AddPublisher(result);
                return result;
            }
            catch (System.Data.SqlClient.SqlException sqlex)
            {
                if (sqlex.Number == 2601) //Index Duplicate Error
                    throw new SqlAzManApplicationException(this, "An Application Attribute with the same Key name already exists.");
                else
                    throw sqlex;
            }
        }
        /// <summary>
        /// Checks the Application access [FOR Windows Users ONLY].
        /// </summary>
        /// <param name="windowsIdentity">The windows identity. System.Security.Principal.WindowsIdentity.GetCurrent() for Windows Applications and (WindowsIdentity)HttpContext.Current.User.Identity or Page.Request.LogonUserIdentity for ASP.NET Applications.</param>
        /// <param name="validFor">The valid for.</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns>
        /// [true] for access allowd, [false] otherwise.
        /// </returns>
        public bool CheckApplicationAccess(WindowsIdentity windowsIdentity, DateTime validFor, params KeyValuePair<string, object>[] contextParameters)
        {
            foreach (IAzManItem item in this.GetItems())
            {
                AuthorizationType auth = item.CheckAccess(windowsIdentity, validFor, contextParameters);
                if (auth == AuthorizationType.AllowWithDelegation || auth == AuthorizationType.Allow)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// Checks the Application access [FOR DB Users ONLY].
        /// </summary>
        /// <param name="dbUser">The db user.</param>
        /// <param name="validFor">The valid for.</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns>
        /// [true] for access allowd, [false] otherwise.
        /// </returns>
        public bool CheckApplicationAccess(IAzManDBUser dbUser, DateTime validFor, params KeyValuePair<string, object>[] contextParameters)
        {
            foreach (IAzManItem item in this.GetItems())
            {
                AuthorizationType auth = item.CheckAccess(dbUser, validFor, contextParameters);
                if (auth == AuthorizationType.AllowWithDelegation || auth == AuthorizationType.Allow)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {

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
        public void Export(XmlWriter xmlWriter, bool includeWindowsUsersAndGroups, bool includeDBUsers, bool includeAuthorizations, object ownerOfExport)
        {
            System.Windows.Forms.Application.DoEvents();
            xmlWriter.WriteStartElement("Application");
            xmlWriter.WriteAttributeString("Name", this.name);
            xmlWriter.WriteAttributeString("Description", this.description);
            //Attributes
            xmlWriter.WriteStartElement("Attributes");
            foreach (IAzManAttribute<IAzManApplication> attribute in this.GetAttributes())
            {
                ((IAzManExport)attribute).Export(xmlWriter, includeWindowsUsersAndGroups, includeDBUsers, includeAuthorizations, ownerOfExport);
            }
            xmlWriter.WriteEndElement();
            //Permissions
            xmlWriter.WriteStartElement("Permissions");
            //Managers
            xmlWriter.WriteStartElement("Managers");
            foreach (KeyValuePair<string, bool> kvp in this.GetManagers())
            {
                if (kvp.Value == true)
                {
                    xmlWriter.WriteStartElement("Manager");
                    xmlWriter.WriteAttributeString("SqlUserOrRole", kvp.Key);
                    xmlWriter.WriteEndElement();
                }
            }
            xmlWriter.WriteEndElement();
            //Users
            xmlWriter.WriteStartElement("Users");
            foreach (KeyValuePair<string, bool> kvp in this.GetUsers())
            {
                if (kvp.Value == true)
                {
                    xmlWriter.WriteStartElement("User");
                    xmlWriter.WriteAttributeString("SqlUserOrRole", kvp.Key);
                    xmlWriter.WriteEndElement();
                }
            }
            xmlWriter.WriteEndElement();
            //Readers
            xmlWriter.WriteStartElement("Readers");
            foreach (KeyValuePair<string, bool> kvp in this.GetReaders())
            {
                if (kvp.Value == true)
                {
                    xmlWriter.WriteStartElement("Reader");
                    xmlWriter.WriteAttributeString("SqlUserOrRole", kvp.Key);
                    xmlWriter.WriteEndElement();
                }
            }
            xmlWriter.WriteEndElement();

            xmlWriter.WriteEndElement();

            if (ownerOfExport as IAzManApplication != null)
            {
                //Export here Store Groups only if I'm exporting an Application only
                xmlWriter.WriteStartElement("StoreGroups");
                foreach (IAzManStoreGroup storeGroup in this.Store.GetStoreGroups())
                {
                    storeGroup.Export(xmlWriter, includeWindowsUsersAndGroups, includeDBUsers, includeAuthorizations, ownerOfExport);
                }
                xmlWriter.WriteEndElement();
            }

            //Application Groups
            xmlWriter.WriteStartElement("ApplicationGroups");
            foreach (IAzManApplicationGroup applicationGroup in this.GetApplicationGroups())
            {
                ((IAzManExport)applicationGroup).Export(xmlWriter, includeWindowsUsersAndGroups, includeDBUsers, includeAuthorizations, ownerOfExport);
            }
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("Items");
            foreach (IAzManItem item in this.GetItems())
            {
                ((IAzManExport)item).Export(xmlWriter, includeWindowsUsersAndGroups, includeDBUsers, includeAuthorizations, ownerOfExport);
            }
            xmlWriter.WriteEndElement();
            xmlWriter.WriteEndElement();
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
            List<string> importedItemNames = new List<string>();
            //First ... Create Application Groups
            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                if (node.Name == "Attributes")
                {
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        if (childNode.Name == "Attribute")
                        {
                            if (!this.Attributes.ContainsKey(childNode.Attributes["Key"].Value))
                            {
                                IAzManAttribute<IAzManApplication> newApplicationAttribute = this.CreateAttribute(childNode.Attributes["Key"].Value, childNode.Attributes["Value"].Value);
                            }
                            else
                            {
                                this.Attributes[childNode.Attributes["Key"].Value].Update(childNode.Attributes["Key"].Value, childNode.Attributes["Value"].Value);
                            }
                        }
                    }
                }
                else if (node.Name == "Attribute")
                {
                    if (!this.Attributes.ContainsKey(node.Attributes["Key"].Value))
                    {
                        IAzManAttribute<IAzManApplication> newApplicationAttribute = this.CreateAttribute(node.Attributes["Key"].Value, node.Attributes["Value"].Value);
                    }
                    else
                    {
                        this.Attributes[node.Attributes["Key"].Value].Update(node.Attributes["Key"].Value, node.Attributes["Value"].Value);
                    }
                }
                System.Windows.Forms.Application.DoEvents();
                if (node.Name == "Permissions")
                {
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        if (childNode.Name == "Managers")
                        {
                            foreach (XmlNode childChildNode in childNode.ChildNodes)
                            {
                                if (childChildNode.Name == "Manager")
                                    this.GrantAccessAsManager(childChildNode.Attributes["SqlUserOrRole"].Value);
                            }
                        }
                        else if (childNode.Name == "Users")
                        {
                            foreach (XmlNode childChildNode in childNode.ChildNodes)
                            {
                                if (childChildNode.Name == "User")
                                    this.GrantAccessAsUser(childChildNode.Attributes["SqlUserOrRole"].Value);
                            }
                        }
                        else if (childNode.Name == "Readers")
                        {
                            foreach (XmlNode childChildNode in childNode.ChildNodes)
                            {
                                if (childChildNode.Name == "Reader")
                                    this.GrantAccessAsReader(childChildNode.Attributes["SqlUserOrRole"].Value);
                            }
                        }
                    }
                }
                if (node.Name == "StoreGroups")
                {
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        if (childNode.Name == "StoreGroup")
                        {
                            GroupType groupType = GroupType.Basic;
                            if (childNode.Attributes["GroupType"].Value == GroupType.LDapQuery.ToString())
                                groupType = GroupType.LDapQuery;


                            IAzManStoreGroup storeGroup = null;
                            string sid = null;
                            if (this.Store.StoreGroups.ContainsKey(childNode.Attributes["Name"].Value))
                            {
                                storeGroup = this.Store.StoreGroups[childNode.Attributes["Name"].Value];
                                sid = storeGroup.SID.StringValue;
                                //Change Store Group SID
                                MergeUtilities.changeSid(childNode.OwnerDocument.DocumentElement, childNode.Attributes["Sid"].Value, sid);
                            }
                            else
                            {
                                sid = SqlAzManSID.NewSqlAzManSid().StringValue;
                                storeGroup = this.Store.CreateStoreGroup(new SqlAzManSID(sid), childNode.Attributes["Name"].Value, childNode.Attributes["Description"].Value, childNode.Attributes["LDAPQuery"].Value, groupType);
                                //Change Store Group SID
                                MergeUtilities.changeSid(childNode.OwnerDocument.DocumentElement, childNode.Attributes["Sid"].Value, sid);
                            }
                            //newStoreGroup.ImportChildren(childNode, includeWindowsUsersAndGroups, includeAuthorizations);
                        }
                    }
                }
                else if (node.Name == "StoreGroup")
                {
                    GroupType groupType = GroupType.Basic;
                    if (node.Attributes["GroupType"].Value == GroupType.LDapQuery.ToString())
                        groupType = GroupType.LDapQuery;

                    IAzManStoreGroup storeGroup = null;
                    string sid = null;
                    if (this.Store.StoreGroups.ContainsKey(node.Attributes["Name"].Value))
                    {
                        storeGroup = this.Store.StoreGroups[node.Attributes["Name"].Value];
                        sid = storeGroup.SID.StringValue;
                        //Change Store Group SID
                        MergeUtilities.changeSid(node.OwnerDocument.DocumentElement, node.Attributes["Sid"].Value, sid);
                    }
                    else
                    {
                        sid = SqlAzManSID.NewSqlAzManSid().StringValue;
                        storeGroup = this.Store.CreateStoreGroup(new SqlAzManSID(sid), node.Attributes["Name"].Value, node.Attributes["Description"].Value, node.Attributes["LDAPQuery"].Value, groupType);
                        //Change Store Group SID
                        MergeUtilities.changeSid(node.OwnerDocument.DocumentElement, node.Attributes["Sid"].Value, sid);
                    }
                    //newStoreGroup.ImportChildren(node, includeWindowsUsersAndGroups, includeAuthorizations);
                }
                if (node.Name == "ApplicationGroups")
                {
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        if (childNode.Name == "ApplicationGroup")
                        {
                            GroupType groupType = GroupType.Basic;
                            if (childNode.Attributes["GroupType"].Value == GroupType.LDapQuery.ToString())
                                groupType = GroupType.LDapQuery;

                            IAzManApplicationGroup applicationGroup = null;
                            string sid = null;
                            if (this.ApplicationGroups.ContainsKey(childNode.Attributes["Name"].Value))
                            {
                                applicationGroup = this.ApplicationGroups[childNode.Attributes["Name"].Value];
                                sid = applicationGroup.SID.StringValue;
                                //Change Application Group SID
                                MergeUtilities.changeSid(childNode.OwnerDocument.DocumentElement, childNode.Attributes["Sid"].Value, sid);
                            }
                            else
                            {
                                sid = SqlAzManSID.NewSqlAzManSid().StringValue;
                                applicationGroup = this.CreateApplicationGroup(new SqlAzManSID(sid), childNode.Attributes["Name"].Value, childNode.Attributes["Description"].Value, childNode.Attributes["LDAPQuery"].Value, groupType);
                                //Change Application Group SID
                                MergeUtilities.changeSid(childNode.OwnerDocument.DocumentElement, childNode.Attributes["Sid"].Value, sid);
                            }
                            //newApplicationGroup.ImportChildren(childNode, includeWindowsUsersAndGroups, includeAuthorizations);
                        }
                    }
                }
                else if (node.Name == "ApplicationGroup")
                {
                    GroupType groupType = GroupType.Basic;
                    if (node.Attributes["GroupType"].Value == GroupType.LDapQuery.ToString())
                        groupType = GroupType.LDapQuery;

                    IAzManApplicationGroup applicationGroup = null;
                    string sid = null;
                    if (this.ApplicationGroups.ContainsKey(node.Attributes["Name"].Value))
                    {
                        applicationGroup = this.ApplicationGroups[node.Attributes["Name"].Value];
                        sid = applicationGroup.SID.StringValue;
                        //Change Application Group SID
                        MergeUtilities.changeSid(node.OwnerDocument.DocumentElement, node.Attributes["Sid"].Value, sid);
                    }
                    else
                    {
                        sid = SqlAzManSID.NewSqlAzManSid().StringValue;
                        applicationGroup = this.CreateApplicationGroup(new SqlAzManSID(sid), node.Attributes["Name"].Value, node.Attributes["Description"].Value, node.Attributes["LDAPQuery"].Value, groupType);
                        //Change Application Group SID
                        MergeUtilities.changeSid(node.OwnerDocument.DocumentElement, node.Attributes["Sid"].Value, sid);
                    }
                    //newApplicationGroup.ImportChildren(node, includeWindowsUsersAndGroups, includeAuthorizations);
                }
            }
            //Then ... Create Items & Application Groups members
            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                System.Windows.Forms.Application.DoEvents();
                if (node.Name == "Item")
                {
                    ItemType itemType;
                    switch (node.Attributes["ItemType"].Value)
                    {
                        case "Role": itemType = ItemType.Role; break;
                        case "Task": itemType = ItemType.Task; break;
                        case "Operation": itemType = ItemType.Operation; break;
                        default:
                            throw new InvalidOperationException("Invalid ItemType on xml node: " + node.InnerXml);
                    }
                    IAzManItem item = this.Items.ContainsKey(node.Attributes["Name"].Value) ? this.Items[node.Attributes["Name"].Value] : null;
                    if (item == null && MergeUtilities.IsOn(mergeOptions, SqlAzManMergeOptions.CreatesNewItems))
                    {
                        this.CreateItem(node.Attributes["Name"].Value, node.Attributes["Description"].Value, itemType);
                        importedItemNames.Add(node.Attributes["Name"].Value);
                    }
                    else if (MergeUtilities.IsOn(mergeOptions, SqlAzManMergeOptions.OverwritesExistingItems))
                    {
                        if (item!=null)
                            item.Delete();
                        this.CreateItem(node.Attributes["Name"].Value, node.Attributes["Description"].Value, itemType);
                        importedItemNames.Add(node.Attributes["Name"].Value);
                    }
                    //Overwrites existing authorizations
                    if (item != null && MergeUtilities.IsOn(mergeOptions, SqlAzManMergeOptions.OverwritesExistingItemAuthorization))
                    {
                        foreach (var auth in item.GetAuthorizations())
                        {
                            auth.Delete();
                        }
                    }
                }
                else if (node.Name == "Items")
                {
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        if (childNode.Name == "Item")
                        {
                            ItemType childItemType;
                            switch (childNode.Attributes["ItemType"].Value)
                            {
                                case "Role": childItemType = ItemType.Role; break;
                                case "Task": childItemType = ItemType.Task; break;
                                case "Operation": childItemType = ItemType.Operation; break;
                                default:
                                    throw new InvalidOperationException("Invalid ItemType on xml node: " + childNode.InnerXml);
                            }
                            IAzManItem newItem = this.Items.ContainsKey(childNode.Attributes["Name"].Value) ? this.Items[childNode.Attributes["Name"].Value] : null;
                            if (newItem == null && MergeUtilities.IsOn(mergeOptions, SqlAzManMergeOptions.CreatesNewItems))
                            {
                                this.CreateItem(childNode.Attributes["Name"].Value, childNode.Attributes["Description"].Value, childItemType);
                                importedItemNames.Add(childNode.Attributes["Name"].Value);
                            }
                            else if (MergeUtilities.IsOn(mergeOptions, SqlAzManMergeOptions.OverwritesExistingItems))
                            {
                                if (newItem != null)
                                {
                                    //If overwrite ... clear attributes, bizrules, members
                                    newItem.Update(childNode.Attributes["Description"].Value);
                                    foreach (var attr in newItem.GetAttributes())
                                    {
                                        attr.Delete();
                                    }
                                    newItem.ClearBizRule();
                                    foreach (var member in newItem.GetMembers())
                                    {
                                        newItem.RemoveMember(member);
                                    }
                                }
                                else
                                {
                                    this.CreateItem(childNode.Attributes["Name"].Value, childNode.Attributes["Description"].Value, childItemType);
                                }
                                importedItemNames.Add(childNode.Attributes["Name"].Value);
                            }
                            //Overwrites existing authorizations
                            if (newItem != null && MergeUtilities.IsOn(mergeOptions, SqlAzManMergeOptions.OverwritesExistingItemAuthorization))
                            {
                                foreach (var auth in newItem.GetAuthorizations())
                                {
                                    auth.Delete();
                                }
                            }
                        }
                    }
                }
                else if (node.Name == "ApplicationGroups")
                {
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        if (childNode.Name == "ApplicationGroup")
                        {
                            GroupType groupType = GroupType.Basic;
                            if (childNode.Attributes["GroupType"].Value == GroupType.LDapQuery.ToString())
                                groupType = GroupType.LDapQuery;
                            if (groupType == GroupType.Basic)
                            {
                                IAzManApplicationGroup newApplicationGroup = this.GetApplicationGroup(childNode.Attributes["Name"].Value);
                                newApplicationGroup.ImportChildren(childNode, includeWindowsUsersAndGroups, includeDBUsers, includeAuthorizations, mergeOptions);
                            }
                        }
                    }
                }
                else if (node.Name == "ApplicationGroup")
                {
                    GroupType groupType = GroupType.Basic;
                    if (node.Attributes["GroupType"].Value == GroupType.LDapQuery.ToString())
                        groupType = GroupType.LDapQuery;
                    if (groupType == GroupType.Basic)
                    {
                        IAzManApplicationGroup newApplicationGroup = this.GetApplicationGroup(node.Attributes["Name"].Value);
                        newApplicationGroup.ImportChildren(node, includeWindowsUsersAndGroups, includeDBUsers, includeAuthorizations, mergeOptions);
                    }
                }
                else if (node.Name == "StoreGroups")
                {
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        if (childNode.Name == "StoreGroup")
                        {
                            GroupType groupType = GroupType.Basic;
                            if (childNode.Attributes["GroupType"].Value == GroupType.LDapQuery.ToString())
                                groupType = GroupType.LDapQuery;
                            if (groupType == GroupType.Basic)
                            {
                                IAzManStoreGroup newStoreGroup = this.Store.GetStoreGroup(childNode.Attributes["Name"].Value);
                                newStoreGroup.ImportChildren(childNode, includeWindowsUsersAndGroups, includeDBUsers, includeAuthorizations, mergeOptions);
                            }
                        }
                    }
                }
                else if (node.Name == "StoreGroup")
                {
                    GroupType groupType = GroupType.Basic;
                    if (node.Attributes["GroupType"].Value == GroupType.LDapQuery.ToString())
                        groupType = GroupType.LDapQuery;
                    if (groupType == GroupType.Basic)
                    {
                        IAzManStoreGroup newStoreGroup = this.Store.GetStoreGroup(new SqlAzManSID(node.Attributes["Name"].Value));
                        newStoreGroup.ImportChildren(node, includeWindowsUsersAndGroups, includeDBUsers, includeAuthorizations, mergeOptions);
                    }
                }
            }

            //Then ... Create Item Members 
            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                if (node.Name == "Item")
                {
                    IAzManItem item = this.GetItem(node.Attributes["Name"].Value);
                    if (item!=null)
                        item.ImportChildren(node, includeWindowsUsersAndGroups, includeDBUsers, includeAuthorizations, mergeOptions);
                }
                else if (node.Name == "Items")
                {
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        if (childNode.Name == "Item")
                        {
                            IAzManItem item = this.GetItem(childNode.Attributes["Name"].Value);
                            if (item!=null)
                                item.ImportChildren(childNode, includeWindowsUsersAndGroups, includeDBUsers, includeAuthorizations, mergeOptions);
                        }
                    }
                }
            }

            //Delete missing items
            if (MergeUtilities.IsOn(mergeOptions, SqlAzManMergeOptions.DeleteMissingItems))
            {
                foreach (var it in this.GetItems())
                {
                    bool exists = false;
                    foreach (var iit in importedItemNames)
                    {
                        if (String.Compare(iit, it.Name, true)==0)
                        {
                            exists = true;
                            break;
                        }
                    }
                    if (!exists)
                        it.Delete();
                }
            }
            this.items = null; //Force refresh
        }
        /// <summary>
        /// Gets the <see cref="T:IAzManItem"/> with the specified itemName name.
        /// </summary>
        /// <value></value>
        public IAzManItem this[string itemName]
        {
            get { return this.GetItem(itemName); }
        }
        #endregion
        #region Object Members
        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
        public override string ToString()
        {
            return String.Format("Application ID: {0}\r\nName: {1}\r\nDescription: {2}", this.applicationId, this.name, this.description);
        }
        #endregion Object Members
        #region DB Users
        /// <summary>
        /// Finds the DB user.
        /// </summary>
        /// <param name="customSid">The custom sid.</param>
        /// <returns></returns>
        public IAzManDBUser GetDBUser(IAzManSid customSid)
        {
            var dtDBUsers = this.db.GetDBUsers(this.store.Name, this.name, customSid.BinaryValue, null);
            IAzManDBUser result;
            if (dtDBUsers.Count() == 0)
            {
                //result = new SqlAzManDBUser(new SqlAzManSID(customSid.BinaryValue, true), customSid.StringValue);
                result = null;
            }
            else
            {
                result = new SqlAzManDBUser(new SqlAzManSID(dtDBUsers.First().DBUserSid.ToArray(),true), dtDBUsers.First().DBUserName);
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
            var dtDBUsers = this.db.GetDBUsers(this.store.Name, this.name, null, userName);
            IAzManDBUser result;
            if (dtDBUsers.Count() == 0)
            {
                result = null;
            }
            else
            {
                result = new SqlAzManDBUser(new SqlAzManSID(dtDBUsers.First().DBUserSid.ToArray(),true), dtDBUsers.First().DBUserName);
            }
            return result;
        }
        /// <summary>
        /// Gets the DB users.
        /// </summary>
        /// <returns></returns>
        public IAzManDBUser[] GetDBUsers()
        {
            var dtDBUsers = this.db.GetDBUsers(this.store.Name, this.name, null, null);
            IAzManDBUser[] result = new IAzManDBUser[dtDBUsers.Count()];
            int i = 0;
            foreach (var row in dtDBUsers)
            {
                result[i++] = new SqlAzManDBUser(new SqlAzManSID(row.DBUserSid.ToArray(),true), row.DBUserName);
            }
            return result;
        }
        #endregion DB Users
        #region IAzManSecurable Members
        private KeyValuePair<string, bool>[] getUsers(byte netsqlazmanfixedserverrole)
        {
            List<KeyValuePair<string, bool>> result = new List<KeyValuePair<string, bool>>();
            string roleName = "NetSqlAzMan_Readers";
            if (netsqlazmanfixedserverrole==1) 
                roleName = "NetSqlAzMan_Users";
            else if (netsqlazmanfixedserverrole==2) 
                roleName = "NetSqlAzMan_Managers";
            SqlCommand cmdHelpLogins = (SqlCommand)this.db.Connection.CreateCommand();
            cmdHelpLogins.Transaction = (SqlTransaction)this.db.Transaction;
            cmdHelpLogins.CommandText = "dbo.helplogins";
            cmdHelpLogins.CommandType = CommandType.StoredProcedure;
            cmdHelpLogins.Parameters.AddWithValue("@rolename", roleName);
            SqlDataAdapter da = new SqlDataAdapter(cmdHelpLogins);
            DataTable logins = new DataTable();
            da.Fill(logins);
            var permissions = from t in this.db.ApplicationPermissionsTable
                              where t.ApplicationId == this.applicationId && t.NetSqlAzManFixedServerRole == netsqlazmanfixedserverrole
                              select t;
            foreach (DataRow drLogins in logins.Rows)
            {
                string login = (string)drLogins[0];
                bool isGranted = false;
                foreach (var drPermission in permissions)
                {
                    if (String.Compare(drPermission.SqlUserOrRole, login, true) == 0)
                    {
                        isGranted = true;
                        break;
                    }
                }
                result.Add(new KeyValuePair<string,bool>(login, isGranted));
            }
            return result.ToArray();
        }
        private void grantAccess(string sqllogin, byte netsqlazmanfixedserverrole)
        {
            this.db.GrantApplicationAccess(this.applicationId, sqllogin, netsqlazmanfixedserverrole);
        }
        private void revokeAccess(string sqllogin, byte netsqlazmanfixedserverrole)
        {
            this.db.RevokeApplicationAccess(this.applicationId, sqllogin, netsqlazmanfixedserverrole);
        }
        /// <summary>
        /// Gets the managers.
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<string, bool>[] GetManagers()
        {
            return this.getUsers(2);
        }
        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<string, bool>[] GetUsers()
        {
            return this.getUsers(1);
        }
        /// <summary>
        /// Gets the readers.
        /// </summary>
        /// <returns></returns>
        public KeyValuePair<string, bool>[] GetReaders()
        {
            return this.getUsers(0);
        }
        /// <summary>
        /// Grants the access as manager.
        /// </summary>
        /// <param name="sqlLogin">The SQL login.</param>
        public void GrantAccessAsManager(string sqlLogin)
        {
            this.grantAccess(sqlLogin, 2);
            this.raiseApplicationPermissionGranted(this, sqlLogin, "Manager");
        }
        /// <summary>
        /// Grants the access as user.
        /// </summary>
        /// <param name="sqlLogin">The SQL login.</param>
        public void GrantAccessAsUser(string sqlLogin)
        {
            this.grantAccess(sqlLogin, 1);
            this.raiseApplicationPermissionGranted(this, sqlLogin, "User");
        }
        /// <summary>
        /// Grants the access as reader.
        /// </summary>
        /// <param name="sqlLogin">The SQL login.</param>
        public void GrantAccessAsReader(string sqlLogin)
        {
            this.grantAccess(sqlLogin, 0);
            this.raiseApplicationPermissionGranted(this, sqlLogin, "Reader");
        }
        /// <summary>
        /// Revokes the access as manager.
        /// </summary>
        /// <param name="sqlLogin">The SQL login.</param>
        public void RevokeAccessAsManager(string sqlLogin)
        {
            this.revokeAccess(sqlLogin, 2);
            this.raiseApplicationPermissionRevoked(this, sqlLogin, "Manager");
        }
        /// <summary>
        /// Revokes the access as user.
        /// </summary>
        /// <param name="sqlLogin">The SQL login.</param>
        public void RevokeAccessAsUser(string sqlLogin)
        {
            this.revokeAccess(sqlLogin, 1);
            this.raiseApplicationPermissionRevoked(this, sqlLogin, "User");
        }
        /// <summary>
        /// Revokes the access as reader.
        /// </summary>
        /// <param name="sqlLogin">The SQL login.</param>
        public void RevokeAccessAsReader(string sqlLogin)
        {
            this.revokeAccess(sqlLogin, 0);
            this.raiseApplicationPermissionRevoked(this, sqlLogin, "Reader");
        }
        /// <summary>
        /// Gets a value indicating whether [I am admin].
        /// </summary>
        /// <value><c>true</c> if [I am admin]; otherwise, <c>false</c>.</value>
        public bool IAmAdmin
        {
            get
            {
                return this.store.IAmAdmin;
            }
        }
        /// <summary>
        /// Gets a value indicating whether [I am manager].
        /// </summary>
        /// <value><c>true</c> if [I am manager]; otherwise, <c>false</c>.</value>
        public bool IAmManager
        {
            get
            {
                return this.netsqlazmanFixedServerRole >= 2;
            }
        }
        /// <summary>
        /// Gets a value indicating whether [I am user].
        /// </summary>
        /// <value><c>true</c> if [I am user]; otherwise, <c>false</c>.</value>
        public bool IAmUser
        {
            get
            {
                return this.netsqlazmanFixedServerRole >= 1;
            }
        }
        /// <summary>
        /// Gets a value indicating whether [I am reader].
        /// </summary>
        /// <value><c>true</c> if [I am reader]; otherwise, <c>false</c>.</value>
        public bool IAmReader
        {
            get
            {
                return this.netsqlazmanFixedServerRole >= 0;
            }
        }
        #endregion IAzManSecurable Members
    }
}
