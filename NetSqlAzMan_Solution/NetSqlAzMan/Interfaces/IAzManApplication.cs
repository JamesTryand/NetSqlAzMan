using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security.Principal;
using System.ServiceModel;
using NetSqlAzMan.ENS;

namespace NetSqlAzMan.Interfaces
{
    /// <summary>
    /// Interfaces Interface for all AzManApplications
    /// </summary>
    [ServiceContract(Namespace="http://NetSqlAzMan/ServiceModel", SessionMode=SessionMode.Required)]
    public interface IAzManApplication : IAzManSecurable, IAzManExport, IAzManImport, IDisposable
    {
        /// <summary>
        /// Gets the application id.
        /// </summary>
        /// <value>The application id.</value>
        [DataMember]
        int ApplicationId { get; } //da implementare esplicitamente
        /// <summary>
        /// Gets the store.
        /// </summary>
        /// <value>The store.</value>
        [DataMember]
        IAzManStore Store { get; }
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
        /// Updates the specified application description.
        /// </summary>
        /// <param name="applicationDescription">The application description.</param>
        [OperationContract]
        void Update(string applicationDescription);
        /// <summary>
        /// Renames application name with the specified new application name.
        /// </summary>
        /// <param name="newApplicationName">New name of the application.</param>
        [OperationContract]
        void Rename(string newApplicationName);
        /// <summary>
        /// Deletes this application.
        /// </summary>
        [OperationContract]
        void Delete();
        /// <summary>
        /// Creates the itemName.
        /// </summary>
        /// <param name="itemName">Name of the itemName.</param>
        /// <param name="itemDescription">The itemName description.</param>
        /// <param name="itemType">Type of the itemName.</param>
        /// <returns></returns>
        [OperationContract]
        IAzManItem CreateItem(string itemName, string itemDescription, ItemType itemType);
        /// <summary>
        /// Gets the itemName.
        /// </summary>
        /// <param name="itemName">Name of the itemName.</param>
        /// <returns></returns>
        [OperationContract]
        IAzManItem GetItem(string itemName);
        /// <summary>
        /// Gets the <see cref="T:IAzManItem"/> with the specified itemName name.
        /// </summary>
        /// <value></value>
        [DataMember]
        IAzManItem this[string itemName] { get;}
        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IAzManItem[] GetItems();
        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>The items.</value>
        [DataMember]
        Dictionary<string, IAzManItem> Items { get; }
        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>The attributes.</value>
        [DataMember]
        Dictionary<string, IAzManAttribute<IAzManApplication>> Attributes { get; }
        /// <summary>
        /// Gets the application groups.
        /// </summary>
        /// <value>The application groups.</value>
        [DataMember]
        Dictionary<string, IAzManApplicationGroup> ApplicationGroups { get; }
        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <param name="itemType">Type of the itemName.</param>
        /// <returns></returns>
        [OperationContract]
        IAzManItem[] GetItems(ItemType itemType);
        /// <summary>
        /// Determines whether [has child items].
        /// </summary>
        /// <param name="itemType">Type of the itemName.</param>
        /// <returns>
        /// 	<c>true</c> if [has child items]; otherwise, <c>false</c>.
        /// </returns>
        [OperationContract]
        bool HasItems(ItemType itemType);
        /// <summary>
        /// Creates the application group.
        /// </summary>
        /// <param name="sid">The object owner.</param>
        /// <param name="name">The name.</param>
        /// <param name="description">The description.</param>
        /// <param name="lDapQuery">The ldap query.</param>
        /// <param name="groupType">Type of the group.</param>
        /// <returns></returns>
        [OperationContract]
        IAzManApplicationGroup CreateApplicationGroup(IAzManSid sid, string name, string description, string lDapQuery, GroupType groupType);
        /// <summary>
        /// Determines whether [has application groups].
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [has application groups]; otherwise, <c>false</c>.
        /// </returns>
        [OperationContract]
        bool HasApplicationGroups();
        /// <summary>
        /// Gets the application groups.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IAzManApplicationGroup[] GetApplicationGroups();
        /// <summary>
        /// Gets the application group.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        [OperationContract]
        IAzManApplicationGroup GetApplicationGroup(string name);
        /// <summary>
        /// Gets the application group.
        /// </summary>
        /// <param name="sid">The object owner.</param>
        /// <returns></returns>
        [OperationContract]
        IAzManApplicationGroup GetApplicationGroup(IAzManSid sid);
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
        IAzManAttribute<IAzManApplication> CreateAttribute(string key, string value);
        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IAzManAttribute<IAzManApplication>[] GetAttributes();
        /// <summary>
        /// Gets the attribute.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        [OperationContract]
        IAzManAttribute<IAzManApplication> GetAttribute(string key);
        /// <summary>
        /// Checks the Application access [FOR Windows Users ONLY].
        /// </summary>
        /// <param name="windowsIdentity">The windows identity. System.Security.Principal.WindowsIdentity.GetCurrent() for Windows Applications and (WindowsIdentity)HttpContext.Current.User.Identity or Page.Request.LogonUserIdentity for ASP.NET Applications.</param>
        /// <param name="validFor">The valid for.</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns>[true] for access allowd, [false] otherwise.</returns>
        [OperationContract]
        bool CheckApplicationAccess(WindowsIdentity windowsIdentity, DateTime validFor, params KeyValuePair<string, object>[] contextParameters);
        /// <summary>
        /// Checks the Application access [FOR DB Users ONLY].
        /// </summary>
        /// <param name="dbUser">The db user.</param>
        /// <param name="validFor">The valid for.</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns>[true] for access allowd, [false] otherwise.</returns>
        [OperationContract]
        bool CheckApplicationAccess(IAzManDBUser dbUser, DateTime validFor, params KeyValuePair<string, object>[] contextParameters);
        /// <summary>
        /// Occurs after an Attribute object has been Created.
        /// </summary>
        event AttributeCreatedDelegate<IAzManApplication> ApplicationAttributeCreated;
        /// <summary>
        /// Occurs after a SqlAzManApplication object has been Deleted.
        /// </summary>
        event ApplicationDeletedDelegate ApplicationDeleted;
        /// <summary>
        /// Occurs after a SqlAzManApplication object has been Updated.
        /// </summary>
        event ApplicationUpdatedDelegate ApplicationUpdated;
        /// <summary>
        /// Occurs after a SqlAzManApplication object has been Renamed.
        /// </summary>
        event ApplicationRenamedDelegate ApplicationRenamed;
        /// <summary>
        /// Occurs after an Application Group object has been Created.
        /// </summary>
        event ApplicationGroupCreatedDelegate ApplicationGroupCreated;
        /// <summary>
        /// Occurs after an Item object has been Created.
        /// </summary>
        event ItemCreatedDelegate ItemCreated;
        /// <summary>
        /// Occurs after a SQL Login is Granted on the Application.
        /// </summary>
        event ApplicationPermissionGrantedDelegate ApplicationPermissionGranted;
        /// <summary>
        /// Occurs after a SQL Login is Revoked on the Application.
        /// </summary>
        event ApplicationPermissionRevokedDelegate ApplicationPermissionRevoked;
    }
}
