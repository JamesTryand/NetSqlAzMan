using System;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.LINQ;
using System.Linq;
using System.ServiceModel;

namespace NetSqlAzMan.ENS
{
    /// <summary>
    /// Interface for ENS (Event Notification System).
    /// </summary>
    [ServiceContract]
    public interface IAzManENS
    {
        #region Application Events
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
        #endregion Application Events
        #region ApplicationGroup Events
        /// <summary>
        /// Occurs after a SqlAzManApplicationGroup object has been Deleted.
        /// </summary>
        event ApplicationGroupDeletedDelegate ApplicationGroupDeleted;
        /// <summary>
        /// Occurs after a SqlAzManApplicationGroup LDAPQuery has been Updated.
        /// </summary>
        event ApplicationGroupLDAPQueryUpdatedDelegate ApplicationGroupLDAPQueryUpdated;
        /// <summary>
        /// Occurs after a SqlAzManApplicationGroup object has been Updated.
        /// </summary>
        event ApplicationGroupUpdatedDelegate ApplicationGroupUpdated;
        /// <summary>
        /// Occurs after a SqlAzManApplicationGroup object has been Renamed.
        /// </summary>
        event ApplicationGroupRenamedDelegate ApplicationGroupRenamed;
        /// <summary>
        /// Occurs after an ApplicationGroupMember object has been Created.
        /// </summary>
        event ApplicationGroupMemberCreatedDelegate ApplicationGroupMemberCreated;
        #endregion ApplicationGroup Events
        #region StoreGroup Events
        /// <summary>
        /// Occurs after a SqlAzManStoreGroup object has been Deleted.
        /// </summary>
        event StoreGroupDeletedDelegate StoreGroupDeleted;
        /// <summary>
        /// Occurs after a SqlAzManStoreGroup LDAPQuery has been Updated.
        /// </summary>
        event StoreGroupLDAPQueryUpdatedDelegate StoreGroupLDAPQueryUpdated;
        /// <summary>
        /// Occurs after a SqlAzManStoreGroup object has been Updated.
        /// </summary>
        event StoreGroupUpdatedDelegate StoreGroupUpdated;
        /// <summary>
        /// Occurs after a SqlAzManStoreGroup object has been Renamed.
        /// </summary>
        event StoreGroupRenamedDelegate StoreGroupRenamed;
        /// <summary>
        /// Occurs after an StoreGroupMember object has been Created.
        /// </summary>
        event StoreGroupMemberCreatedDelegate StoreGroupMemberCreated;
        #endregion StoreGroup Events
        #region Authorization Events
        /// <summary>
        /// Occurs after a SqlAzManAuthorization object has been Deleted.
        /// </summary>
        event AuthorizationDeletedDelegate AuthorizationDeleted;
        /// <summary>
        /// Occurs after a SqlAzManAuthorization object has been Updated.
        /// </summary>
        event AuthorizationUpdatedDelegate AuthorizationUpdated;
        #endregion Authorization Events
        #region Attribute<OWNER> Events
        /// <summary>
        /// Occurs after an Attribute object has been Created.
        /// </summary>
        event AttributeCreatedDelegate<IAzManStore> StoreAttributeCreated;
        /// <summary>
        /// Occurs after an Attribute object has been Deleted.
        /// </summary>
        event AttributeDeletedDelegate<IAzManStore> StoreAttributeDeleted;
        /// <summary>
        /// Occurs after an Attribute object has been Updated.
        /// </summary>
        event AttributeUpdatedDelegate<IAzManStore> StoreAttributeUpdated;
        /// <summary>
        /// Occurs after an Attribute object has been Created.
        /// </summary>
        event AttributeCreatedDelegate<IAzManApplication> ApplicationAttributeCreated;
        /// <summary>
        /// Occurs after an Attribute object has been Deleted.
        /// </summary>
        event AttributeDeletedDelegate<IAzManApplication> ApplicationAttributeDeleted;
        /// <summary>
        /// Occurs after an Attribute object has been Updated.
        /// </summary>
        event AttributeUpdatedDelegate<IAzManApplication> ApplicationAttributeUpdated;
        /// <summary>
        /// Occurs after an Attribute object has been Created.
        /// </summary>
        event AttributeCreatedDelegate<IAzManItem> ItemAttributeCreated;
        /// <summary>
        /// Occurs after an Attribute object has been Deleted.
        /// </summary>
        event AttributeDeletedDelegate<IAzManItem> ItemAttributeDeleted;
        /// <summary>
        /// Occurs after an Attribute object has been Updated.
        /// </summary>
        event AttributeUpdatedDelegate<IAzManItem> ItemAttributeUpdated;
        /// <summary>
        /// Occurs after an Attribute object has been Created.
        /// </summary>
        event AttributeCreatedDelegate<IAzManAuthorization> AuthorizationAttributeCreated;
        /// <summary>
        /// Occurs after an Attribute object has been Deleted.
        /// </summary>
        event AttributeDeletedDelegate<IAzManAuthorization> AuthorizationAttributeDeleted;
        /// <summary>
        /// Occurs after an Attribute object has been Updated.
        /// </summary>
        event AttributeUpdatedDelegate<IAzManAuthorization> AuthorizationAttributeUpdated;
        #endregion Attribute<OWNER> Events
        #region ApplicationGroupMember Events
        /// <summary>
        /// Occurs after a SqlApplicationGroupMember object has been Deleted.
        /// </summary>
        event ApplicationGroupMemberDeletedDelegate ApplicationGroupMemberDeleted;
        #endregion ApplicationGroupMember Events
        #region StoreGroupMember Events
        /// <summary>
        /// Occurs after a SqlStoreGroupMember object has been Deleted.
        /// </summary>
        event StoreGroupMemberDeletedDelegate StoreGroupMemberDeleted;
        #endregion StoreGroupMember Events
        #region Item Events
        /// <summary>
        /// Occurs after a SqlAzManItem object has been Deleted.
        /// </summary>
        event ItemDeletedDelegate ItemDeleted;
        /// <summary>
        /// Occurs after a SqlAzManItem object has been Updated.
        /// </summary>
        event ItemUpdatedDelegate ItemUpdated;
        /// <summary>
        /// Occurs after a SqlAzManItem BizRule has been Updated.
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
        /// Occurs after a Delegate has been Created.
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
        #endregion Item Events
        #region Storage Events
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
        event TransactionBeginnedDelegate TransactionBeggined;
        /// <summary>
        /// Occurs after a Storage Transaction has benn terminated.
        /// </summary>
        event TransactionTerminatedDelegate TransactionTerminated;
        /// <summary>
        /// Occurs after NetSqlAzManMode has ben changed.
        /// </summary>
        event NetSqlAzManModeChangeDelegate NetSqlAzManModeChanged;
        #endregion Storage Events
        #region Store Events
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
        #endregion Store Events
    }
}
