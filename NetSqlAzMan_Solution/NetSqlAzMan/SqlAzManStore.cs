using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Data.SqlTypes;
using System.Text;
using System.Security.Principal;
using System.Collections.Generic;
using System.Collections.Specialized;
using NetSqlAzMan.ENS;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.LINQ;
using System.Linq;
using NetSqlAzMan.Utilities;
using System.Runtime.Serialization;

namespace NetSqlAzMan
{
    /// <summary>
    /// Represents an AzManStore stored on Sql Server.
    /// </summary>
    [Serializable()]
    [DataContract(Namespace = "http://NetSqlAzMan/ServiceModel", IsReference = true)]
    public sealed partial class SqlAzManStore : IAzManStore
    {
        #region Fields
        [NonSerialized()]
        private NetSqlAzManStorageDataContext db;
        private int storeId;
        private string name;
        private string description;
        private IAzManStorage storage;
        private byte[] currentSid;
        private byte netsqlazmanFixedServerRole;
        [NonSerialized()]
        private SqlAzManENS ens;
        internal Dictionary<string, IAzManApplication> applications;
        internal Dictionary<string, IAzManAttribute<IAzManStore>> attributes;
        internal Dictionary<string, IAzManStoreGroup> storeGroups;
        #endregion Fields
        #region Events
        /// <summary>
        /// Occurs after a SqlAzManStore object has been Deleted.
        /// </summary>
        public event StoreDeletedDelegate StoreDeleted;
        /// <summary>
        /// Occurs after a SqlAzManStore object has been Updated.
        /// </summary>
        public event StoreUpdatedDelegate StoreUpdated;
        /// <summary>
        /// Occurs after a SqlAzManStore object has been Renamed.
        /// </summary>
        public event StoreRenamedDelegate StoreRenamed;
        /// <summary>
        /// Occurs after an Application object has been Created.
        /// </summary>
        public event ApplicationCreatedDelegate ApplicationCreated;
        /// <summary>
        /// Occurs after a StoreGroup object has been Created.
        /// </summary>
        public event StoreGroupCreatedDelegate StoreGroupCreated;
        /// <summary>
        /// Occurs after an Application object has been Opened.
        /// </summary>
        public event ApplicationOpenedDelegate ApplicationOpened;
        /// <summary>
        /// Occurs after an Attribute object has been Created.
        /// </summary>
        public event AttributeCreatedDelegate<IAzManStore> StoreAttributeCreated;
        /// <summary>
        /// Occurs after a SQL Login is Granted on the Store.
        /// </summary>
        public event StorePermissionGrantedDelegate StorePermissionGranted;
        /// <summary>
        /// Occurs after a SQL Login is Revoked on the Store.
        /// </summary>
        public event StorePermissionRevokedDelegate StorePermissionRevoked;
        #endregion Events
        #region Private Event Raisers
        private void raiseStoreDeleted(IAzManStorage ownerStorage, string storeName)
        {
            if (this.StoreDeleted != null)
                this.StoreDeleted(ownerStorage, storeName);
        }
        private void raiseStoreUpdated(IAzManStore store, string oldDescription)
        {
            if (this.StoreUpdated != null)
                this.StoreUpdated(store, oldDescription);
        }
        private void raiseStoreRenamed(IAzManStore store, string oldName)
        {
            if (this.StoreRenamed != null)
                this.StoreRenamed(store, oldName);
        }
        private void raiseApplicationCreated(IAzManStore store, IAzManApplication applicationCreated)
        {
            if (this.ApplicationCreated != null)
                this.ApplicationCreated(store, applicationCreated);
        }
        private void raiseStoreGroupCreated(IAzManStore store, IAzManStoreGroup storeGroupCreated)
        {
            if (this.StoreGroupCreated != null)
                this.StoreGroupCreated(store, storeGroupCreated);
        }
        private void raiseApplicationOpened(IAzManApplication application)
        {
            if (this.ApplicationOpened != null)
                this.ApplicationOpened(application);
        }
        private void raiseStoreAttributeCreated(IAzManStore owner, IAzManAttribute<IAzManStore> attributeCreated)
        {
            if (this.StoreAttributeCreated != null)
                this.StoreAttributeCreated(owner, attributeCreated);
        }
        private void raiseStorePermissionGranted(IAzManStore owner, string sqlLogin, string role)
        {
            if (this.StorePermissionGranted != null)
                this.StorePermissionGranted(owner, sqlLogin, role);
        }
        private void raiseStorePermissionRevoked(IAzManStore owner, string sqlLogin, string role)
        {
            if (this.StorePermissionRevoked != null)
                this.StorePermissionRevoked(owner, sqlLogin, role);
        }
        #endregion Private Event Raisers
        #region Constructors
        internal SqlAzManStore(NetSqlAzManStorageDataContext db, IAzManStorage storage, int storeId, string name, string description, byte netsqlazmanFixedServerRole, SqlAzManENS ens)
        {
            this.db = db;
            this.storage = storage;
            this.storeId = storeId;
            this.name = name;
            this.description = description;
            this.currentSid = new SqlAzManSID(((System.Threading.Thread.CurrentPrincipal.Identity as WindowsIdentity) ?? WindowsIdentity.GetCurrent()).User).BinaryValue;
            this.netsqlazmanFixedServerRole = netsqlazmanFixedServerRole;
            this.ens = ens;
        }
        #endregion Constructors
        #region IAzManStore Members
        /// <summary>
        /// Gets the store groups.
        /// </summary>
        /// <value>The store groups.</value>
        public Dictionary<string, IAzManStoreGroup> StoreGroups
        {
            get
            {
                if (this.storeGroups == null)
                {
                    this.storeGroups = new Dictionary<string, IAzManStoreGroup>();
                    foreach (IAzManStoreGroup s in this.GetStoreGroups())
                    {
                        this.storeGroups.Add(s.Name, s);
                    }
                }
                return this.storeGroups;
            }
        }
        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>The attributes.</value>
        public Dictionary<string, IAzManAttribute<IAzManStore>> Attributes
        {
            get
            {
                if (this.attributes == null)
                {
                    this.attributes = new Dictionary<string, IAzManAttribute<IAzManStore>>();
                    foreach (IAzManAttribute<IAzManStore> i in this.GetAttributes())
                    {
                        this.attributes.Add(i.Key, i);
                    }
                }
                return this.attributes;
            }
        }
        /// <summary>
        /// Gets the applications.
        /// </summary>
        /// <value>The applications.</value>
        public Dictionary<string, IAzManApplication> Applications
        {
            get
            {
                if (this.applications == null)
                {
                    this.applications = new Dictionary<string, IAzManApplication>();
                    foreach (IAzManApplication a in this.GetApplications())
                    {
                        this.applications.Add(a.Name, a);
                    }
                }
                return this.applications;
            }
        }
        /// <summary>
        /// Gets the storage.
        /// </summary>
        /// <value>The storage.</value>
        public IAzManStorage Storage
        {
            get
            {
                return this.storage;
            }
        }

        /// <summary>
        /// Gets the store id.
        /// </summary>
        /// <value>The store id.</value>
        int IAzManStore.StoreId
        {
            get 
            {
                
                return this.storeId;
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
        /// Updates store info with the specified store description and LDap path.
        /// </summary>
        /// <param name="storeDescription">The store description.</param>
        public void Update(string storeDescription)
        {
            if (this.description != storeDescription)
            {
                string oldDescription = this.description;
                this.db.StoreUpdate(this.name, storeDescription, this.storeId);
                this.description = storeDescription;
                this.raiseStoreUpdated(this, oldDescription);
            }
        }

        /// <summary>
        /// Renames the specified new store name.
        /// </summary>
        /// <param name="newStoreName">New name of the store.</param>
        public void Rename(string newStoreName)
        {
            try
            {
                if (this.name != newStoreName)
                {
                    string oldName = this.name;
                    this.db.StoreUpdate(newStoreName, this.description, this.storeId);
                    this.name = newStoreName;
                    this.raiseStoreRenamed(this, oldName);
                }
            }
            catch (System.Data.SqlClient.SqlException sqlex)
            {
                if (sqlex.Number == 2601) //Index Duplicate Error
                    throw SqlAzManException.StoreDuplicateException(newStoreName, sqlex);
                else
                    throw SqlAzManException.GenericException(sqlex);
            }
        }

        /// <summary>
        /// Deletes current Store.
        /// </summary>
        public void Delete()
        {
            this.db.StoreDelete(this.storeId);
            this.raiseStoreDeleted(this.storage, this.name);
        }

        /// <summary>
        /// Creates the specified application name.
        /// </summary>
        /// <param name="applicationName">Name of the application.</param>
        /// <param name="applicationDescription">The application description.</param>
        /// <returns></returns>
        public IAzManApplication CreateApplication(string applicationName, string applicationDescription)
        {
            try
            {
                int applicationId = this.db.ApplicationInsert(this.storeId, applicationName, applicationDescription);
                IAzManApplication applicationCreated = new SqlAzManApplication(this.db, this, applicationId, applicationName, applicationDescription, this.netsqlazmanFixedServerRole, this.ens);
                this.raiseApplicationCreated(this, applicationCreated);
                if (this.ens!=null)
                    this.ens.AddPublisher(applicationCreated);
                this.applications = null; //Force cache refresh
                return applicationCreated;
            }
            catch (System.Data.SqlClient.SqlException sqlex)
            {
                if (sqlex.Number == 2601) //Index Duplicate Error
                    throw SqlAzManException.ApplicationDuplicateException(applicationName, this, sqlex);
                else
                    throw SqlAzManException.GenericException(sqlex);
            }
        }

        /// <summary>
        /// Opens the application.
        /// </summary>
        /// <param name="applicationName">Name of the application.</param>
        /// <returns></returns>
        public IAzManApplication GetApplication(string applicationName)
        {
            ApplicationsResult app;
            if ((app = (from t in this.db.Applications() where t.StoreId == this.storeId && t.Name == applicationName select t).FirstOrDefault()) != null)
            {
                byte netsqlazmanFixedServerRole = 0;
                if (this.IAmAdmin)
                {
                    netsqlazmanFixedServerRole = 3;
                }
                else
                {
                    var r1 = this.db.CheckApplicationPermissions(app.ApplicationId, 2);
                    var r2 = this.db.CheckApplicationPermissions(app.ApplicationId, 1);
                    if (r1.HasValue && r1.Value)
                        netsqlazmanFixedServerRole = 2;
                    else if (r2.HasValue && r2.Value)
                        netsqlazmanFixedServerRole = 1;
                }
                IAzManApplication application = new SqlAzManApplication(this.db, this, app.ApplicationId.Value, applicationName, app.Description, netsqlazmanFixedServerRole, this.ens);
                this.raiseApplicationOpened(application);
                this.ens.AddPublisher(application);
                return application;
            }
            else
            {
                throw SqlAzManException.ApplicationNotFoundException(applicationName, this, null);
            }
        }

        /// <summary>
        /// Gets the applications.
        /// </summary>
        /// <returns></returns>
        public IAzManApplication[] GetApplications()
        {
            var ds = from tf in this.db.Applications()
                      where tf.StoreId == this.storeId
                      orderby tf.Name
                      select tf;
            List<IAzManApplication> applications = new List<IAzManApplication>();
            foreach (var row in ds)
            {
                byte netsqlazmanFixedServerRole = 0;
                if (this.IAmAdmin)
                {
                    netsqlazmanFixedServerRole = 3;
                }
                else
                {
                    var r1 = this.db.CheckApplicationPermissions(row.ApplicationId.Value, 2);
                    var r2 = this.db.CheckApplicationPermissions(row.ApplicationId.Value, 1);
                    if (r1.HasValue && r1.Value)
                        netsqlazmanFixedServerRole = 2;
                    else if (r2.HasValue && r2.Value)
                        netsqlazmanFixedServerRole = 1;
                }
                IAzManApplication app = new SqlAzManApplication(this.db, this, row.ApplicationId.Value, row.Name, row.Description, netsqlazmanFixedServerRole, this.ens);
                applications.Add(app);
                this.raiseApplicationOpened(app);
                if (this.ens!=null)
                    this.ens.AddPublisher(app);
            }
            return applications.ToArray();
        }
        /// <summary>
        /// Creates the store group.
        /// </summary>
        /// <param name="sid">The object owner.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="lDapQuery">The ldap query.</param>
        /// <param name="groupType">Type of the group.</param>
        /// <returns></returns>
        public IAzManStoreGroup CreateStoreGroup(IAzManSid sid, string name, string description, string lDapQuery, GroupType groupType)
        {
            try
            {
                if (DirectoryServices.DirectoryServicesUtils.TestLDAPQuery(lDapQuery))
                {
                    this.db.StoreGroupInsert(this.storeId, sid.BinaryValue, name, description, lDapQuery, (byte)groupType);
                    IAzManStoreGroup result = this.GetStoreGroup(name);
                    this.raiseStoreGroupCreated(this, result);
                    this.ens.AddPublisher(result);
                    this.storeGroups = null; //Force cache refresh
                    return result;
                }
                else
                {
                    throw new ArgumentException("LDAP Query syntax error or unavailable Domain.", "lDapQuery");
                }
            }
            catch (System.Data.SqlClient.SqlException sqlex)
            {
                if (sqlex.Number == 2601) //Index Duplicate Error
                    throw SqlAzManException.StoreGroupDuplicateException(name, this, sqlex);
                else
                    throw SqlAzManException.GenericException(sqlex);
            }
        }

        /// <summary>
        /// Determines whether [has store groups].
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [has store groups]; otherwise, <c>false</c>.
        /// </returns>
        public bool HasStoreGroups()
        {
            return this.db.StoreGroups().Any(p=>p.StoreId == this.storeId);
        }

        /// <summary>
        /// Gets the store groups.
        /// </summary>
        /// <returns></returns>
        public IAzManStoreGroup[] GetStoreGroups()
        {
            var ds = from tf in this.db.StoreGroups()
                     where tf.StoreId == this.storeId
                     orderby tf.Name
                     select tf;
            int index = 0;
            IAzManStoreGroup[] storeGroups = new SqlAzManStoreGroup[ds.Count()];
            foreach (var row in ds)
            {
                storeGroups[index] = new SqlAzManStoreGroup(this.db, this, row.StoreGroupId.Value, new SqlAzManSID(row.ObjectSid.ToArray()), row.Name, row.Description, row.LDapQuery, (GroupType)row.GroupType.Value, this.ens);
                this.ens.AddPublisher(storeGroups[index]);
                index++;
            }
            return storeGroups;
        }

        /// <summary>
        /// Gets the store group.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public IAzManStoreGroup GetStoreGroup(string name)
        {
            StoreGroupsResult sgr;
            if ((sgr = (from tf in this.db.StoreGroups() where tf.Name == name && tf.StoreId == this.storeId select tf).FirstOrDefault()) != null)
            {
                int storeGroupid = sgr.StoreGroupId.Value;
                IAzManSid objectSid = new SqlAzManSID(sgr.ObjectSid.ToArray());
                string description = sgr.Description;
                string lDapQuery = sgr.LDapQuery;
                GroupType groupType = (GroupType)sgr.GroupType.Value;
                IAzManStoreGroup result = new SqlAzManStoreGroup(this.db, this, storeGroupid, objectSid, name, description, lDapQuery , groupType, this.ens);
                this.ens.AddPublisher(result);
                return result;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Gets the store group.
        /// </summary>
        /// <param name="sid">The object owner.</param>
        /// <returns></returns>
        public IAzManStoreGroup GetStoreGroup(IAzManSid sid)
        {
            StoreGroupsResult sgr;
            if ((sgr = (from t in this.db.StoreGroups() where t.ObjectSid == sid.BinaryValue && t.StoreId == this.storeId select t).FirstOrDefault()) != null)
            {
                IAzManStoreGroup result = this.GetStoreGroup(sgr.Name);
                this.ens.AddPublisher(result);
                return result;
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Gets the <see cref="T:IAzManApplication"/> with the specified application name.
        /// </summary>
        /// <value></value>
        public IAzManApplication this[string applicationName]
        {
            get { return this.GetApplication(applicationName); }
        }
        /// <summary>
        /// Gets the store attributes.
        /// </summary>
        /// <returns></returns>
        public IAzManAttribute<IAzManStore>[] GetAttributes()
        {
            IAzManAttribute<IAzManStore>[] attributes;
            var ds = from tf in this.db.StoreAttributes()
                     where tf.StoreId == this.storeId
                     select tf;
            attributes = new SqlAzManStoreAttribute[ds.Count()];
            int index = 0;
            foreach (var row in ds)
            {
                attributes[index] = new SqlAzManStoreAttribute(this.db, this, row.StoreAttributeId.Value, row.AttributeKey, row.AttributeValue, this.ens);
                this.ens.AddPublisher(attributes[index]);
                index++;
            }
            return attributes;
        }

        /// <summary>
        /// Gets the store attribute.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public IAzManAttribute<IAzManStore> GetAttribute(string key)
        {
            StoreAttributesResult sar;
            if ((sar = (from t in this.db.StoreAttributes() where t.StoreId == this.storeId && t.AttributeKey == key select t).FirstOrDefault()) != null)
            {
                IAzManAttribute<IAzManStore> result = new SqlAzManStoreAttribute(this.db, this, sar.StoreAttributeId.Value, sar.AttributeKey, sar.AttributeValue, this.ens);
                this.ens.AddPublisher(result);
                return result;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Creates a store attribute.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public IAzManAttribute<IAzManStore> CreateAttribute(string key, string value)
        {
            try
            {
                int storeAttributeId = this.db.StoreAttributeInsert(this.storeId, key, value);
                IAzManAttribute<IAzManStore> result = new SqlAzManStoreAttribute(this.db, this, storeAttributeId, key, value, this.ens);
                this.raiseStoreAttributeCreated(this, result);
                this.ens.AddPublisher(result);
                return result;
            }
            catch (System.Data.SqlClient.SqlException sqlex)
            {
                if (sqlex.Number == 2601) //Index Duplicate Error
                    throw SqlAzManException.AttributeDuplicateException(key, this, sqlex);
                else
                    throw SqlAzManException.GenericException(sqlex);
            }
        }
        /// <summary>
        /// Checks the Store access. [FOR Windows Users ONLY].
        /// </summary>
        /// <param name="windowsIdentity">The windows identity.</param>
        /// <param name="validFor">The valid for.</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns></returns>
        public bool CheckStoreAccess(WindowsIdentity windowsIdentity, DateTime validFor, params KeyValuePair<string, object>[] contextParameters)
        {
            foreach (IAzManApplication application in this.GetApplications())
            {
                if (application.CheckApplicationAccess(windowsIdentity, validFor, contextParameters) == true)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Checks the Store access [FOR DB Users ONLY].
        /// </summary>
        /// <param name="dbUser">The db user.</param>
        /// <param name="validFor">The valid for.</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns>
        /// [true] for access allowd, [false] otherwise.
        /// </returns>
        public bool CheckStoreAccess(IAzManDBUser dbUser, DateTime validFor, params KeyValuePair<string, object>[] contextParameters)
        {
            foreach (IAzManApplication application in this.GetApplications())
            {
                if (application.CheckApplicationAccess(dbUser, validFor, contextParameters) == true)
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
        public void Export(System.Xml.XmlWriter xmlWriter, bool includeWindowsUsersAndGroups, bool includeDBUsers, bool includeAuthorizations, object ownerOfExport)
        {
            System.Windows.Forms.Application.DoEvents();
            xmlWriter.WriteStartElement("Store");
            xmlWriter.WriteAttributeString("Name", this.name);
            xmlWriter.WriteAttributeString("Description", this.description);
            //Attributes
            xmlWriter.WriteStartElement("Attributes");
            foreach (IAzManAttribute<IAzManStore> attribute in this.GetAttributes())
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
            //Store Groups
            xmlWriter.WriteStartElement("StoreGroups");
            foreach (IAzManStoreGroup storeGroup in this.GetStoreGroups())
            {
                storeGroup.Export(xmlWriter, includeWindowsUsersAndGroups, includeDBUsers, includeAuthorizations, ownerOfExport);
            }
            xmlWriter.WriteEndElement();
            xmlWriter.WriteStartElement("Applications");
            //Applications
            foreach (IAzManApplication application in this.GetApplications())
            {
                application.Export(xmlWriter, includeWindowsUsersAndGroups, includeDBUsers, includeAuthorizations, ownerOfExport);
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
            //Create Store Groups
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
                                IAzManAttribute<IAzManStore> newStoreAttribute = this.CreateAttribute(childNode.Attributes["Key"].Value, childNode.Attributes["Value"].Value);
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
                        IAzManAttribute<IAzManStore> newStoreAttribute = this.CreateAttribute(node.Attributes["Key"].Value, node.Attributes["Value"].Value);
                    }
                    else
                    {
                        this.Attributes[node.Attributes["Key"].Value].Update(node.Attributes["Key"].Value, node.Attributes["Value"].Value);
                    }

                }
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
                            if (this.StoreGroups.ContainsKey(childNode.Attributes["Name"].Value))
                            {
                                storeGroup = this.StoreGroups[childNode.Attributes["Name"].Value];
                                sid = storeGroup.SID.StringValue;
                                //Change Store Group SID
                                MergeUtilities.changeSid(childNode.OwnerDocument.DocumentElement, childNode.Attributes["Sid"].Value, sid);
                            }
                            else
                            {
                                sid = SqlAzManSID.NewSqlAzManSid().StringValue;
                                storeGroup = this.CreateStoreGroup(new SqlAzManSID(sid), childNode.Attributes["Name"].Value, childNode.Attributes["Description"].Value, childNode.Attributes["LDAPQuery"].Value, groupType);
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
                    if (this.StoreGroups.ContainsKey(node.Attributes["Name"].Value))
                    {
                        storeGroup = this.StoreGroups[node.Attributes["Name"].Value];
                        sid = storeGroup.SID.StringValue;
                        //Change Store Group SID
                        MergeUtilities.changeSid(node.OwnerDocument.DocumentElement, node.Attributes["Sid"].Value, sid);
                    }
                    else
                    {
                        sid = SqlAzManSID.NewSqlAzManSid().StringValue;
                        storeGroup = this.CreateStoreGroup(new SqlAzManSID(sid), node.Attributes["Name"].Value, node.Attributes["Description"].Value, node.Attributes["LDAPQuery"].Value, groupType);
                        //Change Store Group SID
                        MergeUtilities.changeSid(node.OwnerDocument.DocumentElement, node.Attributes["Sid"].Value, sid);
                    }
                    //newStoreGroup.ImportChildren(node, includeWindowsUsersAndGroups, includeAuthorizations);
                }
            }
            //Create Applications & Store Group Members
            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                System.Windows.Forms.Application.DoEvents();
                if (node.Name == "Applications")
                {
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        if (childNode.Name == "Application")
                        {
                            IAzManApplication newApplication = 
                                this.Applications.ContainsKey(childNode.Attributes["Name"].Value) ?
                                this.Applications[childNode.Attributes["Name"].Value] :
                                this.CreateApplication(childNode.Attributes["Name"].Value, childNode.Attributes["Description"].Value);
                            newApplication.ImportChildren(childNode, includeWindowsUsersAndGroups, includeDBUsers, includeAuthorizations, mergeOptions);
                        }
                    }
                }
                else if (node.Name == "Application")
                {
                    IAzManApplication newApplication =
                        this.Applications.ContainsKey(node.Attributes["Name"].Value) ?
                        this.Applications[node.Attributes["Name"].Value] :
                        this.CreateApplication(node.Attributes["Name"].Value, node.Attributes["Description"].Value);
                    newApplication.ImportChildren(node, includeWindowsUsersAndGroups, includeDBUsers, includeAuthorizations, mergeOptions);
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
                                IAzManStoreGroup newStoreGroup = this.StoreGroups[childNode.Attributes["Name"].Value];
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
                        IAzManStoreGroup newStoreGroup = this.StoreGroups[node.Attributes["Name"].Value];
                        newStoreGroup.ImportChildren(node, includeWindowsUsersAndGroups, includeDBUsers, includeAuthorizations, mergeOptions);
                    }
                }
            }
            this.applications = null; //Force refresh
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
            return String.Format("Store ID: {0}\r\nName: {1}\r\nDescription: {2}",
                this.storeId, this.name, this.description);
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
            var dtDBUsers = this.db.GetDBUsersEx(this.name, null, customSid.BinaryValue, null);
            IAzManDBUser result;
            if (dtDBUsers.Rows.Count == 0)
            {
                result = null;
            }
            else
            {
                result = new SqlAzManDBUser(dtDBUsers.Rows[0]);
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
            var dtDBUsers = this.db.GetDBUsersEx(this.name, null, null, userName);
            IAzManDBUser result;
            if (dtDBUsers.Rows.Count == 0)
            {
                result = null;
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
            
            var dtDBUsers = this.db.GetDBUsersEx(this.name, null, null, null);
            IAzManDBUser[] result = new IAzManDBUser[dtDBUsers.Rows.Count];
            int i = 0;
            foreach (DataRow row in dtDBUsers.Rows)
            {
                result[i++] = new SqlAzManDBUser(row);
            }
            return result;
        }
        #endregion DB Users
        #region IAzManSecurable Members
        private KeyValuePair<string, bool>[] getUsers(byte netsqlazmanfixedserverrole)
        {
            List<KeyValuePair<string, bool>> result = new List<KeyValuePair<string, bool>>();
            string roleName = "NetSqlAzMan_Readers";
            if (netsqlazmanfixedserverrole == 1)
                roleName = "NetSqlAzMan_Users";
            else if (netsqlazmanfixedserverrole == 2)
                roleName = "NetSqlAzMan_Managers";
            SqlCommand cmd = (SqlCommand)this.db.Connection.CreateCommand();
            cmd.Transaction = (SqlTransaction)this.db.Transaction;
            cmd.CommandText = "dbo.helplogins";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@rolename", roleName);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable logins = new DataTable();
            da.Fill(logins);
            var permissions = from t in this.db.StorePermissionsTable
                              where t.StoreId == this.storeId && t.NetSqlAzManFixedServerRole == netsqlazmanfixedserverrole
                              select t;
            foreach (DataRow drLogins in logins.Rows)
            {
                string login = (string)drLogins[0];
                bool isGranted = false;
                foreach (var drPermission in permissions)
                {
                    if (String.Compare(drPermission.SqlUserOrRole , login, true) == 0)
                    {
                        isGranted = true;
                        break;
                    }
                }
                result.Add(new KeyValuePair<string, bool>(login, isGranted));
            }
            return result.ToArray();
        }
        private void grantAccess(string sqllogin, byte netsqlazmanfixedserverrole)
        {
            this.db.GrantStoreAccess(this.storeId, sqllogin, netsqlazmanfixedserverrole);
        }
        private void revokeAccess(string sqllogin, byte netsqlazmanfixedserverrole)
        {
            this.db.RevokeStoreAccess(this.storeId, sqllogin, netsqlazmanfixedserverrole);
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
            this.raiseStorePermissionGranted(this, sqlLogin, "Manager");
        }
        /// <summary>
        /// Grants the access as user.
        /// </summary>
        /// <param name="sqlLogin">The SQL login.</param>
        public void GrantAccessAsUser(string sqlLogin)
        {
            this.grantAccess(sqlLogin, 1);
            this.raiseStorePermissionGranted(this, sqlLogin, "User");
        }
        /// <summary>
        /// Grants the access as reader.
        /// </summary>
        /// <param name="sqlLogin">The SQL login.</param>
        public void GrantAccessAsReader(string sqlLogin)
        {
            this.grantAccess(sqlLogin, 0);
            this.raiseStorePermissionGranted(this, sqlLogin, "Reader");
        }
        /// <summary>
        /// Revokes the access as manager.
        /// </summary>
        /// <param name="sqlLogin">The SQL login.</param>
        public void RevokeAccessAsManager(string sqlLogin)
        {
            this.revokeAccess(sqlLogin, 2);
            this.raiseStorePermissionRevoked(this, sqlLogin, "Manager");
        }
        /// <summary>
        /// Revokes the access as user.
        /// </summary>
        /// <param name="sqlLogin">The SQL login.</param>
        public void RevokeAccessAsUser(string sqlLogin)
        {
            this.revokeAccess(sqlLogin, 1);
            this.raiseStorePermissionRevoked(this, sqlLogin, "User");
        }
        /// <summary>
        /// Revokes the access as reader.
        /// </summary>
        /// <param name="sqlLogin">The SQL login.</param>
        public void RevokeAccessAsReader(string sqlLogin)
        {
            this.revokeAccess(sqlLogin, 0);
            this.raiseStorePermissionRevoked(this, sqlLogin, "Reader");
        }
        /// <summary>
        /// Gets a value indicating whether [I am admin].
        /// </summary>
        /// <value><c>true</c> if [I am admin]; otherwise, <c>false</c>.</value>
        public bool IAmAdmin
        {
            get
            {
                return this.storage.IAmAdmin;
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
