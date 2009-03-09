using System;
using System.Security.Principal;
using System.Collections.Generic;
using System.Text;
using NetSqlAzMan.ENS;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace NetSqlAzMan.Interfaces
{
    /// <summary>
    /// Interfaces interface for all AzMan Stores
    /// </summary>
    [ServiceContract(Namespace="http://NetSqlAzMan/ServiceModel", SessionMode=SessionMode.Required)]
    public interface IAzManStore : IAzManSecurable, IAzManExport, IAzManImport, IDisposable
    {
        /// <summary>
        /// Gets the store id.
        /// </summary>
        /// <value>The store id.</value>
        [DataMember]
        int StoreId { get; } //da implementare esplicitamente
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        [DataMember]
        string Name { get; }
        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        [DataMember]
        string Description { get; }
        /// <summary>
        /// Updates store info with the specified store description and LDap path.
        /// </summary>
        /// <param name="storeDescription">The store description.</param>
        [OperationContract]
        void Update(string storeDescription);
        /// <summary>
        /// Renames the specified new store name.
        /// </summary>
        /// <param name="newStoreName">New name of the store.</param>
        [OperationContract]
        void Rename(string newStoreName);
        /// <summary>
        /// Deletes current Store.
        /// </summary>
        [OperationContract]
        void Delete();
        /// <summary>
        /// Creates the application.
        /// </summary>
        /// <param name="applicationName">Name of the application.</param>
        /// <param name="applicationDescription">The application description.</param>
        /// <returns></returns>
        [OperationContract]
        IAzManApplication CreateApplication(string applicationName, string applicationDescription);
        /// <summary>
        /// Opens the application.
        /// </summary>
        /// <param name="applicationName">Name of the application.</param>
        /// <returns></returns>
        [OperationContract]
        IAzManApplication GetApplication(string applicationName);
        /// <summary>
        /// Gets the <see cref="T:IAzManApplication"/> with the specified application name.
        /// </summary>
        /// <value></value>
        [DataMember]
        IAzManApplication this[string applicationName] { get; }
        /// <summary>
        /// Gets the applications.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IAzManApplication[] GetApplications();
        /// <summary>
        /// Gets the applications.
        /// </summary>
        /// <value>The applications.</value>
        [DataMember]
        Dictionary<string, IAzManApplication> Applications { get; }
        /// <summary>
        /// Gets the store groups.
        /// </summary>
        /// <value>The store groups.</value>
        [DataMember]
        Dictionary<string, IAzManStoreGroup> StoreGroups { get; }
        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>The attributes.</value>
        [DataMember]
        Dictionary<string, IAzManAttribute<IAzManStore>> Attributes { get; }
        /// <summary>
        /// Creates the store group.
        /// </summary>
        /// <param name="sid">The object owner.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="lDapQuery">The ldap query.</param>
        /// <param name="groupType">Type of the group.</param>
        /// <returns></returns>
        [OperationContract]
        IAzManStoreGroup CreateStoreGroup(IAzManSid sid, string name, string description, string lDapQuery, GroupType groupType);
        /// <summary>
        /// Determines whether [has store groups].
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [has store groups]; otherwise, <c>false</c>.
        /// </returns>
        [OperationContract]
        bool HasStoreGroups();
        /// <summary>
        /// Gets the store groups.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IAzManStoreGroup[] GetStoreGroups();
        /// <summary>
        /// Gets the store group.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        [OperationContract]
        IAzManStoreGroup GetStoreGroup(string name);
        /// <summary>
        /// Gets the store group.
        /// </summary>
        /// <param name="sid">The object owner.</param>
        /// <returns></returns>
        [OperationContract]
        IAzManStoreGroup GetStoreGroup(IAzManSid sid);
        /// <summary>
        /// Gets the storage.
        /// </summary>
        /// <value>The storage.</value>
        [DataMember]
        IAzManStorage Storage { get; }
        /// <summary>
        /// Finds the DB user.
        /// </summary>
        /// <param name="customSid">The custom sid.</param>
        /// <returns></returns>
        [OperationContract]
        IAzManDBUser GetDBUser(IAzManSid customSid);
        /// <summary>
        /// Finds the DB user.
        /// </summary>
        /// <param name="userName">The custom sid.</param>
        /// <returns></returns>
        [OperationContract]
        IAzManDBUser GetDBUser(string userName);
        /// <summary>
        /// Gets the DB users.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IAzManDBUser[] GetDBUsers();
        /// <summary>
        /// Creates an attribute.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        [OperationContract]
        IAzManAttribute<IAzManStore> CreateAttribute(string key, string value);
        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IAzManAttribute<IAzManStore>[] GetAttributes();
        /// <summary>
        /// Gets the attribute.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        [OperationContract]
        IAzManAttribute<IAzManStore> GetAttribute(string key);
        /// <summary>
        /// Checks the Store access [FOR Windows Users ONLY].
        /// </summary>
        /// <param name="windowsIdentity">The windows identity. System.Security.Principal.WindowsIdentity.GetCurrent() for Windows Applications and (WindowsIdentity)HttpContext.Current.User.Identity or Page.Request.LogonUserIdentity for ASP.NET Applications.</param>
        /// <param name="validFor">The valid for.</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns>[true] for access allowd, [false] otherwise.</returns>
        [OperationContract]
        bool CheckStoreAccess(WindowsIdentity windowsIdentity, DateTime validFor, params KeyValuePair<string, object>[] contextParameters);
        /// <summary>
        /// Checks the Store access [FOR DB Users ONLY].
        /// </summary>
        /// <param name="dbUser">The db user.</param>
        /// <param name="validFor">The valid for.</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns>[true] for access allowd, [false] otherwise.</returns>
        [OperationContract]
        bool CheckStoreAccess(IAzManDBUser dbUser, DateTime validFor, params KeyValuePair<string, object>[] contextParameters);
        /// <summary>
        /// Occurs after an Attribute object has been Created.
        /// </summary>
        event AttributeCreatedDelegate<IAzManStore> StoreAttributeCreated;
        /// <summary>
        /// Occurs after a SqlAzManStore object has been Deleted.
        /// </summary>
        event StoreDeletedDelegate StoreDeleted;
        /// <summary>
        /// Occurs after a SqlAzManStore object has been Updated.
        /// </summary>
        event StoreUpdatedDelegate StoreUpdated;
        /// <summary>
        /// Occurs after a SqlAzManStore object has been Renamed.
        /// </summary>
        event StoreRenamedDelegate StoreRenamed;
        /// <summary>
        /// Occurs after an Application object has been Created.
        /// </summary>
        event ApplicationCreatedDelegate ApplicationCreated;
        /// <summary>
        /// Occurs after a StoreGroup object has been Created.
        /// </summary>
        event StoreGroupCreatedDelegate StoreGroupCreated;
        /// <summary>
        /// Occurs after an Application object has been Opened.
        /// </summary>
        event ApplicationOpenedDelegate ApplicationOpened;
        /// <summary>
        /// Occurs after a SQL Login is Granted on the Store.
        /// </summary>
        event StorePermissionGrantedDelegate StorePermissionGranted;
        /// <summary>
        /// Occurs after a SQL Login is Revoked on the Store.
        /// </summary>
        event StorePermissionRevokedDelegate StorePermissionRevoked;
    }
}
