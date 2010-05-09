using System;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzMan.ENS
{
    #region Application Delegates
    /// <summary>
    /// Delegate for Application Deleted events.
    /// </summary>
    /// <param name="ownerStore">The owner store.</param>
    /// <param name="applicationName">Name of the application.</param>
    public delegate void ApplicationDeletedDelegate(IAzManStore ownerStore, string applicationName);
    /// <summary>
    /// Delegate for Application Renamed events.
    /// </summary>
    /// <param name="application">The application.</param>
    /// <param name="oldName">The old name.</param>
    public delegate void ApplicationRenamedDelegate(IAzManApplication application, string oldName);
    /// <summary>
    /// Delegate for Application Updated events.
    /// </summary>
    /// <param name="application">The application.</param>
    /// <param name="oldDescription">The old description.</param>
    public delegate void ApplicationUpdatedDelegate(IAzManApplication application, string oldDescription);
    /// <summary>
    /// Delegate for ApplicationGroupCreated events.
    /// </summary>
    /// <param name="application">The application.</param>
    /// <param name="applicationGroupCreated">The application group created.</param>
    public delegate void ApplicationGroupCreatedDelegate(IAzManApplication application, IAzManApplicationGroup applicationGroupCreated);
    /// <summary>
    /// Delegate for ItemCreated events.
    /// </summary>
    /// <param name="application">The application.</param>
    /// <param name="itemCreated">The itemName created.</param>
    public delegate void ItemCreatedDelegate(IAzManApplication application, IAzManItem itemCreated);
    /// <summary>
    /// Delegate for ApplicationPermissionGranted events
    /// </summary>
    /// <param name="application">The Application.</param>
    /// <param name="sqlLogin">The SQL Login.</param>
    /// <param name="role">The NetSqlAzMan Role.</param>
    public delegate void ApplicationPermissionGrantedDelegate(IAzManApplication application, string sqlLogin, string role);
    /// <summary>
    /// Delegate for ApplicationPermissionRevoked events
    /// </summary>
    /// <param name="application">The Application</param>
    /// <param name="sqlLogin">The SQL Login</param>
    /// <param name="role">The NetSqlAzMan Role.</param>
    public delegate void ApplicationPermissionRevokedDelegate(IAzManApplication application, string sqlLogin, string role);
    #endregion Application Delegates
    #region ApplicationGroup Delegates
    /// <summary>
    /// Delegate for ApplicationGroup Deleted events.
    /// </summary>
    /// <param name="ownerApplication">The owner application.</param>
    /// <param name="applicationGroupName">Name of the applicationGroup.</param>
    public delegate void ApplicationGroupDeletedDelegate(IAzManApplication ownerApplication, string applicationGroupName);
    /// <summary>
    /// Delegate for ApplicationGroup Renamed events.
    /// </summary>
    /// <param name="applicationGroup">The applicationGroup.</param>
    /// <param name="oldName">The old name.</param>
    public delegate void ApplicationGroupRenamedDelegate(IAzManApplicationGroup applicationGroup, string oldName);
    /// <summary>
    /// Delegate for ApplicationGroup Updated events.
    /// </summary>
    /// <param name="applicationGroup">Application Group updated</param>
    /// <param name="oldSid">The old SID</param>
    /// <param name="oldDescription">The old Description</param>
    /// <param name="oldGroupType">The old Group Type</param>
    public delegate void ApplicationGroupUpdatedDelegate(IAzManApplicationGroup applicationGroup, IAzManSid oldSid, string oldDescription, GroupType oldGroupType);
    /// <summary>
    /// Delegate for ApplicationGroup LDAPQueryUpdated events.
    /// </summary>
    /// <param name="applicationGroup">Application Group updated</param>
    /// <param name="oldLDapQuery">The old LDapQuery</param>
    public delegate void ApplicationGroupLDAPQueryUpdatedDelegate(IAzManApplicationGroup applicationGroup, string oldLDapQuery);
    /// <summary>
    /// Delegate for ItemCreated events.
    /// </summary>
    /// <param name="applicationGroup">The applicationGroup.</param>
    /// <param name="memberCreated">The member created.</param>
    public delegate void ApplicationGroupMemberCreatedDelegate(IAzManApplicationGroup applicationGroup, IAzManApplicationGroupMember memberCreated);
    #endregion ApplicationGroup Delegates
    #region StoreGroup Delegates
    /// <summary>
    /// Delegate for StoreGroup Deleted events.
    /// </summary>
    /// <param name="ownerStore">The owner Store.</param>
    /// <param name="storeGroupName">Name of the storeGroup.</param>
    public delegate void StoreGroupDeletedDelegate(IAzManStore ownerStore, string storeGroupName);
    /// <summary>
    /// Delegate for StoreGroup Renamed events.
    /// </summary>
    /// <param name="storeGroup">The storeGroup.</param>
    /// <param name="oldName">The old name.</param>
    public delegate void StoreGroupRenamedDelegate(IAzManStoreGroup storeGroup, string oldName);
    /// <summary>
    /// Delegate for StoreGroup Updated events.
    /// </summary>
    /// <param name="storeGroup">Application Group updated</param>
    /// <param name="oldSid">The old SID</param>
    /// <param name="oldDescription">The old Description</param>
    /// <param name="oldGroupType">The old Group Type</param>
    public delegate void StoreGroupUpdatedDelegate(IAzManStoreGroup storeGroup, IAzManSid oldSid, string oldDescription, GroupType oldGroupType);
    /// <summary>
    /// Delegate for StoreGroup LDAPQueryUpdated events.
    /// </summary>
    /// <param name="storeGroup">Application Group updated</param>
    /// <param name="oldLDapQuery">The old LDapQuery</param>
    public delegate void StoreGroupLDAPQueryUpdatedDelegate(IAzManStoreGroup storeGroup, string oldLDapQuery);
    /// <summary>
    /// Delegate for ItemCreated events.
    /// </summary>
    /// <param name="storeGroup">The storeGroup.</param>
    /// <param name="memberCreated">The member created.</param>
    public delegate void StoreGroupMemberCreatedDelegate(IAzManStoreGroup storeGroup, IAzManStoreGroupMember memberCreated);
    #endregion StoreGroup Delegates
    #region Authorization Delegates
    /// <summary>
    /// Delegate for Authorization Deleted events.
    /// </summary>
    /// <param name="ownerItem">The owner Item.</param>
    /// <param name="owner">Owner Sid of the authorization.</param>
    /// <param name="sid">Sid of the authorization.</param>
    public delegate void AuthorizationDeletedDelegate(IAzManItem ownerItem, IAzManSid owner, IAzManSid sid);
    /// <summary>
    /// Delegate for Authorization Updated events.
    /// </summary>
    /// <param name="authorization">The authorization updated object.</param>
    /// <param name="oldOwner">The old Owner.</param>
    /// <param name="oldOwnerSidWhereDefined">The old OwnerSidWhereDefined.</param>
    /// <param name="oldSid">The old Sid.</param>
    /// <param name="oldSidWhereDefined">The old SidWhereDefined.</param>
    /// <param name="oldAuthorizationType">The old AuthorizationType.</param>
    /// <param name="oldValidFrom">The old ValidFrom.</param>
    /// <param name="oldValidTo">The old ValidTo.</param>
    public delegate void AuthorizationUpdatedDelegate(IAzManAuthorization authorization, IAzManSid oldOwner, WhereDefined oldOwnerSidWhereDefined, IAzManSid oldSid, WhereDefined oldSidWhereDefined, AuthorizationType oldAuthorizationType, DateTime? oldValidFrom, DateTime? oldValidTo);
    #endregion Authorization Delegates
    #region Attribute<OWNER> Delegates
    /// <summary>
    /// Delegate for AttributeCreated events.
    /// </summary>
    /// <param name="owner">The Owner.</param>
    /// <param name="attributeCreated">The Attribute created.</param>
    public delegate void AttributeCreatedDelegate<OWNER>(OWNER owner, IAzManAttribute<OWNER> attributeCreated);
    /// <summary>
    /// Delegate for AttributeDeleted events.
    /// </summary>
    /// <param name="owner">The owner Authorization.</param>
    /// <param name="key">The key of the AuthorizationAttribute.</param>
    public delegate void AttributeDeletedDelegate<OWNER>(OWNER owner, string key);
    /// <summary>
    /// Delegate for AttributeUpdated events.
    /// </summary>
    /// <param name="attribute">The attribute object.</param>
    /// <param name="oldKey">The old key.</param>
    /// <param name="oldValue">The old value.</param>
    public delegate void AttributeUpdatedDelegate<OWNER>(IAzManAttribute<OWNER> attribute, string oldKey, string oldValue);
    #endregion Attribute<OWNER> Delegates
    #region ApplicationGroupMember Delegates
    /// <summary>
    /// Delegate for ApplicationGroupMember Deleted events.
    /// </summary>
    /// <param name="ownerApplicationGroup">The owner Application Group.</param>
    /// <param name="sid">The owner of the ApplicationGroupMember.</param>
    public delegate void ApplicationGroupMemberDeletedDelegate(IAzManApplicationGroup ownerApplicationGroup, IAzManSid sid);
    #endregion ApplicationGroupMember Delegates
    #region StoreGroupMember Delegates
    /// <summary>
    /// Delegate for StoreGroupMember Deleted events.
    /// </summary>
    /// <param name="ownerStoreGroup">The owner Store Group.</param>
    /// <param name="sid">The owner of the StoreGroupMember.</param>
    public delegate void StoreGroupMemberDeletedDelegate(IAzManStoreGroup ownerStoreGroup, IAzManSid sid);
    #endregion StoreGroupMember Delegates
    #region Item Delegates
    /// <summary>
    /// Delegate for Item Deleted events.
    /// </summary>
    /// <param name="applicationContainer">The Application Container.</param>
    /// <param name="itemName">Name of the itemName.</param>
    /// <param name="itemType">Type of the itemName.</param>
    public delegate void ItemDeletedDelegate(IAzManApplication applicationContainer, string itemName, ItemType itemType);
    /// <summary>
    /// Delegate for Item Renamed events.
    /// </summary>
    /// <param name="item">The itemName.</param>
    /// <param name="oldName">The old name.</param>
    public delegate void ItemRenamedDelegate(IAzManItem item, string oldName);
    /// <summary>
    /// Delegate for Item Updated events.
    /// </summary>
    /// <param name="item">The itemName.</param>
    /// <param name="oldDescription">The old description.</param>
    public delegate void ItemUpdatedDelegate(IAzManItem item, string oldDescription);
    /// <summary>
    /// Delegate for Biz Rule Updated events.
    /// </summary>
    /// <param name="item">The itemName.</param>
    /// <param name="oldBizRule">The old Biz Rule.</param>
    public delegate void BizRuleUpdatedDelegate(IAzManItem item, string oldBizRule);
    /// <summary>
    /// Delegate for AuthorizationCreated events.
    /// </summary>
    /// <param name="item">The itemName.</param>
    /// <param name="authorizationCreated">The Authorization created.</param>
    public delegate void AuthorizationCreatedDelegate(IAzManItem item, IAzManAuthorization authorizationCreated);
    /// <summary>
    /// Delegate for DelegationCreated events.
    /// </summary>
    /// <param name="item">The itemName.</param>
    /// <param name="delegationCreated">The Delegation created.</param>
    public delegate void DelegateCreatedDelegate(IAzManItem item, IAzManAuthorization delegationCreated);
    /// <summary>
    /// Delegate for DelegationDeleted events.
    /// </summary>
    /// <param name="item">The itemName.</param>
    /// <param name="delegatingUserSid">The delegating User.</param>
    /// <param name="delegatedUserSid">The delegated User.</param>
    /// <param name="authorizationType">The authorization Type.</param>
    public delegate void DelegateDeletedDelegate(IAzManItem item, IAzManSid delegatingUserSid, IAzManSid delegatedUserSid, RestrictedAuthorizationType authorizationType);
    /// <summary>
    /// Delegate for MemberAdded events.
    /// </summary>
    /// <param name="item">The itemName.</param>
    /// <param name="memberAdded">The member added.</param>
    public delegate void MemberAddedDelegate(IAzManItem item, IAzManItem memberAdded);
    /// <summary>
    /// Delegate for MemberRemoved events.
    /// </summary>
    /// <param name="item">The itemName.</param>
    /// <param name="memberRemoved">The member removed.</param>
    public delegate void MemberRemovedDelegate(IAzManItem item, IAzManItem memberRemoved);
    #endregion Item Delegates
    #region Storage Delegates
    /// <summary>
    /// Delegate for StoreCreated events.
    /// </summary>
    /// <param name="storeCreated">The Store created.</param>
    public delegate void StoreCreatedDelegate(IAzManStore storeCreated);
    /// <summary>
    /// Delegate for StoreOpened events.
    /// </summary>
    /// <param name="store">The Store opened.</param>
    public delegate void StoreOpenedDelegate(IAzManStore store);
    /// <summary>
    /// Delegate for Begin Transaction Events.
    /// </summary>
    public delegate void TransactionBeginnedDelegate();
    /// <summary>
    /// Delegate for Commit/Rollback Transaction Events.
    /// </summary>
    /// <param name="success"></param>
    public delegate void TransactionTerminatedDelegate(bool success);
    /// <summary>
    /// Delegate for NetSqlAzMan Mode Changed Events.
    /// </summary>
    public delegate void NetSqlAzManModeChangeDelegate(NetSqlAzManMode oldMode, NetSqlAzManMode newMode);
    #endregion Storage Delegates
    #region Store Delegates
    /// <summary>
    /// Delegate for Store Deleted events.
    /// </summary>
    /// <param name="ownerStorage">The owner Storage.</param>
    /// <param name="storeName">Name of the store.</param>
    public delegate void StoreDeletedDelegate(IAzManStorage ownerStorage, string storeName);
    /// <summary>
    /// Delegate for Store Renamed events.
    /// </summary>
    /// <param name="store">The store.</param>
    /// <param name="oldName">The old name.</param>
    public delegate void StoreRenamedDelegate(IAzManStore store, string oldName);
    /// <summary>
    /// Delegate for Store Updated events.
    /// </summary>
    /// <param name="store">The store.</param>
    /// <param name="oldDescription">The old description.</param>
    public delegate void StoreUpdatedDelegate(IAzManStore store, string oldDescription);
    /// <summary>
    /// Delegate for StoreChildStoreCreated events.
    /// </summary>
    /// <param name="store">The store.</param>
    /// <param name="applicationCreated">The Application created.</param>
    public delegate void ApplicationCreatedDelegate(IAzManStore store, IAzManApplication applicationCreated);
    /// <summary>
    /// Delegate for StoreCreated events.
    /// </summary>
    /// <param name="store">The store.</param>
    /// <param name="storeGroupCreated">The StoreGroup created.</param>
    public delegate void StoreGroupCreatedDelegate(IAzManStore store, IAzManStoreGroup storeGroupCreated);
    /// <summary>
    /// Delegate for ApplicationOpened events.
    /// </summary>
    /// <param name="application">The Application opened.</param>
    public delegate void ApplicationOpenedDelegate(IAzManApplication application);
    /// <summary>
    /// Delegate for StorePermissionGranted events
    /// </summary>
    /// <param name="store">The Store</param>
    /// <param name="sqlLogin">The SQL Login</param>
    /// <param name="role">The NetSqlAzMan Role.</param>
    public delegate void StorePermissionGrantedDelegate(IAzManStore store, string sqlLogin, string role);
    /// <summary>
    /// Delegate for StorePermissionRevoked events
    /// </summary>
    /// <param name="store">The Store</param>
    /// <param name="sqlLogin">The SQL Login</param>
    /// <param name="role">The NetSqlAzMan Role.</param>
    public delegate void StorePermissionRevokedDelegate(IAzManStore store, string sqlLogin, string role);
    #endregion Store Delegates
}
