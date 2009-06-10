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
    /// Interfaces interface for all AzMan Storage
    /// </summary>
    [ServiceContract(Namespace="http://NetSqlAzMan/ServiceModel", SessionMode=SessionMode.Required)]
    public interface IAzManStorage : IAzManExport, IAzManImport, IAzManTransactable, IDisposable
    {
        /// <summary>
        /// Opens the connection.
        /// </summary>
        [OperationContract]
        void OpenConnection();
        /// <summary>
        /// Closes the connection.
        /// </summary>
        [OperationContract]
        void CloseConnection();
        /// <summary>
        /// Gets the database vesion.
        /// </summary>
        /// <value>The database vesion.</value>
        [DataMember]
        string DatabaseVesion { get; }
        /// <summary>
        /// Gets or sets the storage time out.
        /// </summary>
        /// <value>The storage time out.</value>
        [DataMember]
        int StorageTimeOut { get; set; }
        /// <summary>
        /// Creates the specified store name.
        /// </summary>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="storeDescription">The store description.</param>
        [OperationContract]
        IAzManStore CreateStore(string storeName, string storeDescription);
        /// <summary>
        /// Opens the specified store name.
        /// </summary>
        /// <param name="storeName">Name of the store.</param>
        /// <returns></returns>
        [OperationContract]
        IAzManStore GetStore(string storeName);
        /// <summary>
        /// Gets the <see cref="T:IAzManStore"/> with the specified store name.
        /// </summary>
        /// <value></value>
        [DataMember]
        IAzManStore this[string storeName] { get; }
        /// <summary>
        /// Gets the stores.
        /// </summary>
        /// <value>The stores.</value>
        [DataMember]
        Dictionary<string, IAzManStore> Stores { get; }
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        [DataMember]
        string ConnectionString { get; set; }
        /// <summary>
        /// Determines whether this instance has stores.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance has stores; otherwise, <c>false</c>.
        /// </returns>
        [OperationContract]
        bool HasStores();
        /// <summary>
        /// Gets the stores.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IAzManStore[] GetStores();
        /// <summary>
        /// Finds the DB user.
        /// </summary>
        /// <param name="customSid">The custom sid.</param>
        /// <returns></returns>
        [OperationContract(Name="GetDBUserBySID")]
        IAzManDBUser GetDBUser(IAzManSid customSid);
        /// <summary>
        /// Finds the DB user.
        /// </summary>
        /// <param name="userName">The custom sid.</param>
        /// <returns></returns>
        [OperationContract(Name = "GetDBUserByName")]
        IAzManDBUser GetDBUser(string userName);
        /// <summary>
        /// Gets the DB users.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IAzManDBUser[] GetDBUsers();
        /// <summary>
        /// Gets the DB users.
        /// </summary>
        /// <value>The DB users.</value>
        [DataMember]
        Dictionary<string, IAzManDBUser> DBUsers { get; }
        /// <summary>
        /// Gets the mode.
        /// </summary>
        /// <value>The mode.</value>
        [DataMember]
        NetSqlAzManMode Mode { get; set; }
        /// <summary>
        /// Gets the ENS (Event Notification System).
        /// </summary>
        /// <value>The ENS.</value>
        [DataMember]
        IAzManENS ENS { get; }
        /// <summary>
        /// Gets or sets the log stream.
        /// </summary>
        /// <value>The log stream.</value>
        [DataMember]
        System.IO.TextWriter LogStream { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [log errors].
        /// </summary>
        /// <value><c>true</c> if [log errors]; otherwise, <c>false</c>.</value>
        [DataMember]
        bool LogErrors { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [log warnings].
        /// </summary>
        /// <value><c>true</c> if [log warnings]; otherwise, <c>false</c>.</value>
        [DataMember]
        bool LogWarnings { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [log informations].
        /// </summary>
        /// <value><c>true</c> if [log informations]; otherwise, <c>false</c>.</value>
        [DataMember]
        bool LogInformations { get; set; }
        /// <summary>
        /// Gets a value indicating whether I am a NetSqlAzMan_Administrators member.
        /// </summary>
        /// <value><c>true</c> if [I am admin]; otherwise, <c>false</c>.</value>
        [DataMember]
        bool IAmAdmin { get; }
        /// <summary>
        /// Gets a value indicating whether [log on event log].
        /// </summary>
        /// <value><c>true</c> if [log on event log]; otherwise, <c>false</c>.</value>
        [DataMember]
        bool LogOnEventLog { get; set; }
        /// <summary>
        /// Gets a value indicating whether [log on db].
        /// </summary>
        /// <value><c>true</c> if [log on db]; otherwise, <c>false</c>.</value>
        [DataMember]
        bool LogOnDb { get; set; }
        /// <summary>
        /// Checks the access [FOR Windows Users ONLY].
        /// </summary>
        /// <param name="StoreName">Name of the store.</param>
        /// <param name="ApplicationName">Name of the application.</param>
        /// <param name="ItemName">Name of the itemName.</param>
        /// <param name="windowsIdentity">The windows identity.</param>
        /// <param name="ValidFor">The valid for.</param>
        /// <param name="OperationsOnly">if set to <c>true</c> [operations only].</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns>AuthorizationType</returns>
        [OperationContract(Name = "CheckAccessForWindowsUsersWithoutAttributesRetrieve")]
        AuthorizationType CheckAccess(string StoreName, string ApplicationName, string ItemName, WindowsIdentity windowsIdentity, DateTime ValidFor, bool OperationsOnly, params KeyValuePair<string, object>[] contextParameters);
        /// <summary>
        /// Checks the access [FOR Windows Users ONLY].
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
        [OperationContract(Name = "CheckAccessForWindowsUsersWithAttributesRetrieve")]
        AuthorizationType CheckAccess(string StoreName, string ApplicationName, string ItemName, WindowsIdentity windowsIdentity, DateTime ValidFor, bool OperationsOnly, out List<KeyValuePair<string, string>> attributes, params KeyValuePair<string, object>[] contextParameters);
        /// <summary>
        /// Checks the access [FOR DB Users ONLY].
        /// </summary>
        /// <param name="StoreName">Name of the store.</param>
        /// <param name="ApplicationName">Name of the application.</param>
        /// <param name="ItemName">Name of the itemName.</param>
        /// <param name="dbUser">The db user.</param>
        /// <param name="ValidFor">The valid for.</param>
        /// <param name="OperationsOnly">if set to <c>true</c> [operations only].</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns>AuthorizationType</returns>
        [OperationContract(Name = "CheckAccessForDatabaseUsersWithoutAttributesRetrieve")]
        AuthorizationType CheckAccess(string StoreName, string ApplicationName, string ItemName, IAzManDBUser dbUser, DateTime ValidFor, bool OperationsOnly, params KeyValuePair<string, object>[] contextParameters);
        /// <summary>
        /// Checks the access [FOR DB Users ONLY].
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
        [OperationContract(Name="CheckAccessForDatabaseUsersWithAttributesRetrieve")]
        AuthorizationType CheckAccess(string StoreName, string ApplicationName, string ItemName, IAzManDBUser dbUser, DateTime ValidFor, bool OperationsOnly, out List<KeyValuePair<string, string>> attributes, params KeyValuePair<string, object>[] contextParameters);
        /// <summary>
        /// Checks the access in async way [FOR Windows Users ONLY].
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
        [OperationContract(Name = "BeginCheckAccessForWindowsUsersWithoutAttributesRetrieve")]
        IAsyncResult BeginCheckAccess(string StoreName, string ApplicationName, string ItemName, WindowsIdentity windowsIdentity, DateTime ValidFor, bool OperationsOnly, AsyncCallback callBack, object stateObject, params KeyValuePair<string, object>[] contextParameters);
        /// <summary>
        /// Checks the access in async way [FOR Windows Users ONLY].
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
        [OperationContract(Name = "BeginCheckAccessForWindowsUsersWithAttributesRetrieve")]
        IAsyncResult BeginCheckAccess(string StoreName, string ApplicationName, string ItemName, WindowsIdentity windowsIdentity, DateTime ValidFor, bool OperationsOnly, AsyncCallback callBack, object stateObject, out List<KeyValuePair<string, string>> attributes, params KeyValuePair<string, object>[] contextParameters);
        /// <summary>
        /// Checks the access in async way [FOR DB Users ONLY].
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
        [OperationContract(Name = "BeginCheckAccessForDatabaseUsersWithoutAttributesRetrieve")]
        IAsyncResult BeginCheckAccess(string StoreName, string ApplicationName, string ItemName, IAzManDBUser dbUser, DateTime ValidFor, bool OperationsOnly, AsyncCallback callBack, object stateObject, params KeyValuePair<string, object>[] contextParameters);
        /// <summary>
        /// Checks the access in async way [FOR DB Users ONLY].
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
        [OperationContract(Name = "BeginCheckAccessForDatabaseUsersWithAttributesRetrieve")]
        IAsyncResult BeginCheckAccess(string StoreName, string ApplicationName, string ItemName, IAzManDBUser dbUser, DateTime ValidFor, bool OperationsOnly, AsyncCallback callBack, object stateObject, out List<KeyValuePair<string, string>> attributes, params KeyValuePair<string, object>[] contextParameters);
        /// <summary>
        /// Ends the check access [FOR Windows Users ONLY].
        /// </summary>
        /// <param name="asyncResult">The async authorizationType.</param>
        /// <returns>AuthorizationType</returns>
        //[OperationContract(Name = "EndCheckAccessForWindowsUsersWithoutAttributesRetrieve")]
        AuthorizationType EndCheckAccess(IAsyncResult asyncResult);
        /// <summary>
        /// Ends the check access [FOR Windows Users ONLY].
        /// </summary>
        /// <param name="asyncResult">The async authorizationType.</param>
        /// <param name="attributes">The attributes readed.</param>
        /// <returns>AuthorizationType</returns>
        //[OperationContract(Name = "EndCheckAccessForWindowsUsersWithoutAttributesRetrieve")]
        AuthorizationType EndCheckAccess(IAsyncResult asyncResult, out List<KeyValuePair<string, string>> attributes);
        /// <summary>
        /// Ends the check access [FOR DB Users ONLY].
        /// </summary>
        /// <param name="asyncResult">The async authorizationType.</param>
        /// <returns>AuthorizationType</returns>
        //[OperationContract(Name = "EndCheckAccessForDatabaseUsersWithoutAttributesRetrieve")]
        AuthorizationType EndCheckAccessForDBUsers(IAsyncResult asyncResult);
        /// <summary>
        /// Ends the check access [FOR DB Users ONLY].
        /// </summary>
        /// <param name="asyncResult">The async authorizationType.</param>
        /// <param name="attributes">The attributes readed.</param>
        /// <returns>AuthorizationType</returns>
        //[OperationContract(Name = "EndCheckAccessForWindowsUsersWithAttributesRetrieve")]
        AuthorizationType EndCheckAccessForDBUsers(IAsyncResult asyncResult, out List<KeyValuePair<string, string>> attributes);
        /// <summary>
        /// Occurs after a Store object has been Created.
        /// </summary>
        event StoreCreatedDelegate StoreCreated;
        /// <summary>
        /// Occurs after a Store object has been Opened.
        /// </summary>
        event StoreOpenedDelegate StoreOpened;
        /// <summary>
        /// Occurs after a Storage Transaction has benn initiated.
        /// </summary>
        event TransactionBeginnedDelegate TransactionBeginned;
        /// <summary>
        /// Occurs after a Storage Transaction has benn terminated.
        /// </summary>
        event TransactionTerminatedDelegate TransactionTerminated;
        /// <summary>
        /// Occurs after NetSqlAzMan Mode has been changed.
        /// </summary>
        event NetSqlAzManModeChangeDelegate NetSqlAzManModeChanged;
    }
}
