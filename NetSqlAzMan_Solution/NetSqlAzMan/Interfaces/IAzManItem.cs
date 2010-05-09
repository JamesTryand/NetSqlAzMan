using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Principal;
using System.ServiceModel;
using NetSqlAzMan.ENS;

namespace NetSqlAzMan.Interfaces
{
    /// <summary>
    /// Interface for all AzMan Items
    /// </summary>
    [ServiceContract(Namespace="http://NetSqlAzMan/ServiceModel", SessionMode=SessionMode.Required)]
    public interface IAzManItem : IAzManExport, IAzManImport, IDisposable
    {
        /// <summary>
        /// Gets the itemName id.
        /// </summary>
        /// <value>The itemName id.</value>
        [DataMember]
        int ItemId { get; }
        /// <summary>
        /// Gets the application.
        /// </summary>
        /// <value>The application.</value>
        [DataMember]
        IAzManApplication Application { get; }
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        [DataMember]
        string Name { get; }
        /// <summary>
        /// Gets the biz rule.
        /// </summary>
        /// <value>The biz rule.</value>
        [DataMember]
        string BizRuleSource { get; }
        /// <summary>
        /// Gets the biz rule script language.
        /// </summary>
        /// <value>The biz rule script language.</value>
        [DataMember]
        BizRuleSourceLanguage? BizRuleSourceLanguage { get; }
        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        [DataMember]
        string Description { get; }
        /// <summary>
        /// Is the type of the Item (Role, Task or Operation).
        /// </summary>
        [DataMember]
        ItemType ItemType { get; }
        /// <summary>
        /// Updates the specified application description.
        /// </summary>
        /// <param name="newItemDescription">The new itemName description.</param>
        [OperationContract]
        void Update(string newItemDescription);
        /// <summary>
        /// Renames the specified application with a new application name.
        /// </summary>
        /// <param name="newItemName">New name of the itemName.</param>
        [OperationContract]
        void Rename(string newItemName);
        /// <summary>
        /// Deletes this Item.
        /// </summary>
        [OperationContract]
        void Delete();
        /// <summary>
        /// Determines whether this instance has members.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if this instance has members; otherwise, <c>false</c>.
        /// </returns>
        [OperationContract]
        bool HasMembers();
        /// <summary>
        /// Creates the authorization.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="ownerSidWhereDefined">The owner sid where defined.</param>
        /// <param name="sid">The object owner.</param>
        /// <param name="sidWhereDefined">The object owner where defined.</param>
        /// <param name="authorizationType">Type of the authorization.</param>
        /// <param name="validFrom">The valid from.</param>
        /// <param name="validTo">The valid to.</param>
        /// <returns></returns>
        [OperationContract]
        IAzManAuthorization CreateAuthorization(IAzManSid owner, WhereDefined ownerSidWhereDefined, IAzManSid sid, WhereDefined sidWhereDefined, AuthorizationType authorizationType, Nullable<DateTime> validFrom, Nullable<DateTime> validTo);
        /// <summary>
        /// Gets the members.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IAzManItem[] GetMembers();
        /// <summary>
        /// Gets the Items where I'am a member.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IAzManItem[] GetItemsWhereIAmAMember();
        /// <summary>
        /// Adds the member.
        /// </summary>
        /// <param name="member">The member.</param>
        [OperationContract]
        void AddMember(IAzManItem member);
        /// <summary>
        /// Removes the member.
        /// </summary>
        /// <param name="member">The member.</param>
        [OperationContract]
        void RemoveMember(IAzManItem member);
        /// <summary>
        /// Gets the members.
        /// </summary>
        /// <value>The members.</value>
        [DataMember]
        Dictionary<string, IAzManItem> Members { get; }
        /// <summary>
        /// Gets the items where I am A member.
        /// </summary>
        /// <value>The items where I am A member.</value>
        [DataMember]
        Dictionary<string, IAzManItem> ItemsWhereIAmAMember { get; }
        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>The attributes.</value>
        [DataMember]
        Dictionary<string, IAzManAttribute<IAzManItem>> Attributes { get; }
        /// <summary>
        /// Gets the authorizations.
        /// </summary>
        /// <value>The authorizations.</value>
        [DataMember]
        System.Collections.ObjectModel.ReadOnlyCollection<IAzManAuthorization> Authorizations { get; }
        /// <summary>
        /// Gets the authorizations.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IAzManAuthorization[] GetAuthorizations();
        /// <summary>
        /// Gets the authorizations.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        [OperationContract]
        IAzManAuthorization[] GetAuthorizations(AuthorizationType type);
        /// <summary>
        /// Gets the authorizations by SID.
        /// </summary>
        /// <param name="sid">The member owner.</param>
        /// <returns></returns>
        [OperationContract]
        IAzManAuthorization[] GetAuthorizationsOfMember(IAzManSid sid);
        /// <summary>
        /// Gets the authorizations by Owner SID.
        /// </summary>
        /// <param name="owner">The Owner owner.</param>
        /// <returns></returns>
        [OperationContract]
        IAzManAuthorization[] GetAuthorizationsOfOwner(IAzManSid owner);
        /// <summary>
        /// Gets the authorizations by Owner SID and Member SID.
        /// </summary>
        /// <param name="owner">The Owner owner.</param>
        /// <param name="sid">The member owner.</param>
        /// <returns></returns>
        [OperationContract]
        IAzManAuthorization[] GetAuthorizations(IAzManSid owner, IAzManSid sid);
        /// <summary>
        /// Gets the <see cref="T:IAzManAuthorization[]"/> with the specified owner.
        /// </summary>
        /// <value></value>
        [DataMember]
        IAzManAuthorization[] this[IAzManSid owner, IAzManSid sid] { get; }
        /// <summary>
        /// Gets the authorization.
        /// </summary>
        /// <param name="authorizationId">The authorization id.</param>
        /// <returns></returns>
        [OperationContract]
        IAzManAuthorization GetAuthorization(int authorizationId);
        /// <summary>
        /// Checks the access [FOR Windows Users ONLY].
        /// </summary>
        /// <param name="windowsIdentity">The windows identity. System.Security.Principal.WindowsIdentity.GetCurrent() for Windows Applications and (WindowsIdentity)HttpContext.Current.User.Identity or Page.Request.LogonUserIdentity for ASP.NET Applications.</param>
        /// <param name="validFor">The valid for.</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns>AuthorizationType</returns>
        [OperationContract]
        AuthorizationType CheckAccess(WindowsIdentity windowsIdentity, DateTime validFor, params KeyValuePair<string, object>[] contextParameters);
        /// <summary>
        /// Checks the access [FOR Windows Users ONLY].
        /// </summary>
        /// <param name="windowsIdentity">The windows identity. System.Security.Principal.WindowsIdentity.GetCurrent() for Windows Applications and (WindowsIdentity)HttpContext.Current.User.Identity or Page.Request.LogonUserIdentity for ASP.NET Applications.</param>
        /// <param name="validFor">The valid for.</param>
        /// <param name="attributes">The attributes readed.</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns>AuthorizationType</returns>
        [OperationContract]
        AuthorizationType CheckAccess(WindowsIdentity windowsIdentity, DateTime validFor, out List<KeyValuePair<string, string>> attributes, params KeyValuePair<string, object>[] contextParameters);
        /// <summary>
        /// Checks the access [FOR DB Users ONLY].
        /// </summary>
        /// <param name="dbUser">The db user.</param>
        /// <param name="validFor">The valid for.</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns>AuthorizationType</returns>
        [OperationContract]
        AuthorizationType CheckAccess(IAzManDBUser dbUser, DateTime validFor, params KeyValuePair<string, object>[] contextParameters);
        /// <summary>
        /// Checks the access [FOR DB Users ONLY].
        /// </summary>
        /// <param name="dbUser">The db user.</param>
        /// <param name="validFor">The valid for.</param>
        /// <param name="attributes">The attributes readed.</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns>AuthorizationType</returns>
        [OperationContract]
        AuthorizationType CheckAccess(IAzManDBUser dbUser, DateTime validFor, out List<KeyValuePair<string, string>> attributes, params KeyValuePair<string, object>[] contextParameters);
        /// <summary>
        /// Checks the access in async way [FOR Windows Users ONLY].
        /// </summary>
        /// <param name="windowsIdentity">The windows identity. System.Security.Principal.WindowsIdentity.GetCurrent() for Windows Applications and (WindowsIdentity)HttpContext.Current.User.Identity or Page.Request.LogonUserIdentity for ASP.NET Applications.</param>
        /// <param name="validFor">The valid for.</param>
        /// <param name="callBack">The call back.</param>
        /// <param name="stateObject">The state object.</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns>AuthorizationType</returns>
        /// <remarks>
        /// 	<para>Remeber to: </para>
        /// 	<para>1) add "Asynchronous Processing=true" in the Storage Connection String</para>
        /// 	<para>2) Storage Connection must be manually opened and closed.</para>
        /// </remarks>
        [OperationContract]
        IAsyncResult BeginCheckAccess(WindowsIdentity windowsIdentity, DateTime validFor, AsyncCallback callBack, object stateObject, params KeyValuePair<string, object>[] contextParameters);
        /// <summary>
        /// Checks the access in async way [FOR Windows Users ONLY].
        /// </summary>
        /// <param name="windowsIdentity">The windows identity. System.Security.Principal.WindowsIdentity.GetCurrent() for Windows Applications and (WindowsIdentity)HttpContext.Current.User.Identity or Page.Request.LogonUserIdentity for ASP.NET Applications.</param>
        /// <param name="validFor">The valid for.</param>
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
        [OperationContract]
        IAsyncResult BeginCheckAccess(WindowsIdentity windowsIdentity, DateTime validFor, AsyncCallback callBack, object stateObject, out List<KeyValuePair<string, string>> attributes, params KeyValuePair<string, object>[] contextParameters);
        /// <summary>
        /// Checks the access in async way [FOR DB Users ONLY].
        /// </summary>
        /// <param name="dbUser">The db user.</param>
        /// <param name="validFor">The valid for.</param>
        /// <param name="callBack">The call back.</param>
        /// <param name="stateObject">The state object.</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns>AuthorizationType</returns>
        /// <remarks>
        /// 	<para>Remeber to: </para>
        /// 	<para>1) add "Asynchronous Processing=true" in the Storage Connection String</para>
        /// 	<para>2) Storage Connection must be manually opened and closed.</para>
        /// </remarks>
        [OperationContract]
        IAsyncResult BeginCheckAccess(IAzManDBUser dbUser, DateTime validFor, AsyncCallback callBack, object stateObject, params KeyValuePair<string, object>[] contextParameters);
        /// <summary>
        /// Checks the access in async way [FOR DB Users ONLY].
        /// </summary>
        /// <param name="dbUser">The db user.</param>
        /// <param name="validFor">The valid for.</param>
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
        [OperationContract]
        IAsyncResult BeginCheckAccess(IAzManDBUser dbUser, DateTime validFor, AsyncCallback callBack, object stateObject, out List<KeyValuePair<string, string>> attributes, params KeyValuePair<string, object>[] contextParameters);
        /// <summary>
        /// Ends the check access [FOR Windows Users ONLY].
        /// </summary>
        /// <param name="asyncResult">The async authorizationType.</param>
        /// <returns>AuthorizationType</returns>
        [OperationContract]
        AuthorizationType EndCheckAccess(IAsyncResult asyncResult);
        /// <summary>
        /// Ends the check access [FOR Windows Users ONLY].
        /// </summary>
        /// <param name="asyncResult">The async authorizationType.</param>
        /// <param name="attributes">The attributes readed.</param>
        /// <returns>AuthorizationType</returns>
        [OperationContract]
        AuthorizationType EndCheckAccess(IAsyncResult asyncResult, out List<KeyValuePair<string, string>> attributes);
        /// <summary>
        /// Ends the check access [FOR DB Users ONLY].
        /// </summary>
        /// <param name="asyncResult">The async authorizationType.</param>
        /// <returns>AuthorizationType</returns>
        [OperationContract]
        AuthorizationType EndCheckAccessForDBUsers(IAsyncResult asyncResult);
        /// <summary>
        /// Ends the check access [FOR DB Users ONLY].
        /// </summary>
        /// <param name="asyncResult">The async authorizationType.</param>
        /// <param name="attributes">The attributes readed.</param>
        /// <returns>AuthorizationType</returns>
        [OperationContract]
        AuthorizationType EndCheckAccessForDBUsers(IAsyncResult asyncResult, out List<KeyValuePair<string, string>> attributes);
        /// <summary>
        /// Creates the delegation [Windows Users].
        /// </summary>
        /// <param name="delegatingUser">The delegating user.</param>
        /// <param name="delegateUser">The delegate user.</param>
        /// <param name="authorizationType">Type of the authorization.</param>
        /// <param name="validFrom">The valid from.</param>
        /// <param name="validTo">The valid to.</param>
        /// <returns>IAzManAuthorization</returns>
        [OperationContract]
        IAzManAuthorization CreateDelegateAuthorization(WindowsIdentity delegatingUser, IAzManSid delegateUser, RestrictedAuthorizationType authorizationType, DateTime? validFrom, DateTime? validTo);
        /// <summary>
        /// Creates the delegation [DB Users].
        /// </summary>
        /// <param name="delegatingUser">The delegating user.</param>
        /// <param name="delegateUser">The delegate user.</param>
        /// <param name="authorizationType">Type of the authorization.</param>
        /// <param name="validFrom">The valid from.</param>
        /// <param name="validTo">The valid to.</param>
        /// <returns>IAzManAuthorization</returns>
        [OperationContract]
        IAzManAuthorization CreateDelegateAuthorization(IAzManDBUser delegatingUser, IAzManSid delegateUser, RestrictedAuthorizationType authorizationType, DateTime? validFrom, DateTime? validTo);
        /// <summary>
        /// Removes the delegate [Windows Users].
        /// </summary>
        /// <param name="delegatingUser">The delegating user.</param>
        /// <param name="delegateUser">The delegate user.</param>
        /// <param name="authorizationType">Type of the authorization.</param>
        [OperationContract]
        void DeleteDelegateAuthorization(WindowsIdentity delegatingUser, IAzManSid delegateUser, RestrictedAuthorizationType authorizationType);
        /// <summary>
        /// Removes the delegate [DB Users].
        /// </summary>
        /// <param name="delegatingUser">The delegating user.</param>
        /// <param name="delegateUser">The delegate user.</param>
        /// <param name="authorizationType">Type of the authorization.</param>
        [OperationContract]
        void DeleteDelegateAuthorization(IAzManDBUser delegatingUser, IAzManSid delegateUser, RestrictedAuthorizationType authorizationType);
        /// <summary>
        /// Creates an attribute.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        [OperationContract]
        IAzManAttribute<IAzManItem> CreateAttribute(string key, string value);
        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IAzManAttribute<IAzManItem>[] GetAttributes();
        /// <summary>
        /// Gets the attribute.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        [OperationContract]
        IAzManAttribute<IAzManItem> GetAttribute(string key);
        /// <summary>
        /// Reloads the biz rule.
        /// </summary>
        /// <param name="bizRule">The biz rule.</param>
        /// <param name="bizRuleScriptLanguage">The biz rule script language.</param>
        [OperationContract]
        void ReloadBizRule(string bizRule, BizRuleSourceLanguage bizRuleScriptLanguage);
        /// <summary>
        /// Clears the biz rule.
        /// </summary>
        [OperationContract]
        void ClearBizRule();
        /// <summary>
        /// Loads the biz rule compiled assembly.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        Assembly LoadBizRuleAssembly();
        /// <summary>
        /// Occurs after an Attribute object has been Created.
        /// </summary>
        event AttributeCreatedDelegate<IAzManItem> ItemAttributeCreated;
        /// <summary>
        /// Occurs after a SqlAzManItem object has been Deleted.
        /// </summary>
        event ItemDeletedDelegate ItemDeleted;
        /// <summary>
        /// Occurs after a SqlAzManItem object has been Updated.
        /// </summary>
        event ItemUpdatedDelegate ItemUpdated;
        /// <summary>
        /// Occurs after a SqlAzManItem Biz Rule has been Updated.
        /// </summary>
        event BizRuleUpdatedDelegate BizRuleUpdated;
        /// <summary>
        /// Occurs after a SqlAzManItem object has been Renamed.
        /// </summary>
        event ItemRenamedDelegate ItemRenamed;
        /// <summary>
        /// Occurs after an Authorization object has been Created.
        /// </summary>
        event AuthorizationCreatedDelegate AuthorizationCreated;
        /// <summary>
        /// Occurs after a Delegated has been Created.
        /// </summary>
        event DelegateCreatedDelegate DelegateCreated;
        /// <summary>
        /// Occurs after a Delegate has been Deleted.
        /// </summary>
        event DelegateDeletedDelegate DelegateDeleted;
        /// <summary>
        /// Occurs after an Item object has been Added as a member Item.
        /// </summary>
        event MemberAddedDelegate MemberAdded;
        /// <summary>
        /// Occurs after an Item object has been Removed as a member Item.
        /// </summary>
        event MemberRemovedDelegate MemberRemoved;
    }
}
