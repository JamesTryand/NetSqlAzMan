using System;
using NetSqlAzMan;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.LINQ;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace NetSqlAzMan.ENS
{
    /// <summary>
    /// SqlAzMan Event Notification System.
    /// </summary>
    [Serializable()]
    [DataContract]
    public class SqlAzManENS : IAzManENS
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlAzManENS"/> class.
        /// </summary>
        internal SqlAzManENS()
        { 
            
        }
        #endregion Constructors
        #region Application Events
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
        /// Occurs after a SQL Login is Granted on the Application.
        /// </summary>
        public event ApplicationPermissionGrantedDelegate ApplicationPermissionGranted;
        /// <summary>
        /// Occurs after a SQL Login is Revoked on the Application.
        /// </summary>
        public event ApplicationPermissionRevokedDelegate ApplicationPermissionRevoked;
        #endregion Application Events
        #region ApplicationGroup Events
        /// <summary>
        /// Occurs after a SqlAzManApplicationGroup object has been Deleted.
        /// </summary>
        public event ApplicationGroupDeletedDelegate ApplicationGroupDeleted;
        /// <summary>
        /// Occurs after a SqlAzManApplicationGroup LDAPQuery has been Updated.
        /// </summary>
        public event ApplicationGroupLDAPQueryUpdatedDelegate ApplicationGroupLDAPQueryUpdated;
        /// <summary>
        /// Occurs after a SqlAzManApplicationGroup object has been Updated.
        /// </summary>
        public event ApplicationGroupUpdatedDelegate ApplicationGroupUpdated;
        /// <summary>
        /// Occurs after a SqlAzManApplicationGroup object has been Renamed.
        /// </summary>
        public event ApplicationGroupRenamedDelegate ApplicationGroupRenamed;
        /// <summary>
        /// Occurs after an ApplicationGroupMember object has been Created.
        /// </summary>
        public event ApplicationGroupMemberCreatedDelegate ApplicationGroupMemberCreated;
        #endregion ApplicationGroup Events
        #region StoreGroup Events
        /// <summary>
        /// Occurs after a SqlAzManStoreGroup object has been Deleted.
        /// </summary>
        public event StoreGroupDeletedDelegate StoreGroupDeleted;
        /// <summary>
        /// Occurs after a SqlAzManStoreGroup LDAPQuery has been Updated.
        /// </summary>
        public event StoreGroupLDAPQueryUpdatedDelegate StoreGroupLDAPQueryUpdated;
        /// <summary>
        /// Occurs after a SqlAzManStoreGroup object has been Updated.
        /// </summary>
        public event StoreGroupUpdatedDelegate StoreGroupUpdated;
        /// <summary>
        /// Occurs after a SqlAzManStoreGroup object has been Renamed.
        /// </summary>
        public event StoreGroupRenamedDelegate StoreGroupRenamed;
        /// <summary>
        /// Occurs after an StoreGroupMember object has been Created.
        /// </summary>
        public event StoreGroupMemberCreatedDelegate StoreGroupMemberCreated;
        #endregion StoreGroup Events
        #region Authorization Events
        /// <summary>
        /// Occurs after a SqlAzManAuthorization object has been Deleted.
        /// </summary>
        public event AuthorizationDeletedDelegate AuthorizationDeleted;
        /// <summary>
        /// Occurs after a SqlAzManAuthorization object has been Updated.
        /// </summary>
        public event AuthorizationUpdatedDelegate AuthorizationUpdated;
        #endregion Authorization Events
        #region Attribute<OWNER> Events
        /// <summary>
        /// Occurs after an Attribute object has been Created.
        /// </summary>
        public event AttributeCreatedDelegate<IAzManStore> StoreAttributeCreated;
        /// <summary>
        /// Occurs after an Attribute object has been Deleted.
        /// </summary>
        public event AttributeDeletedDelegate<IAzManStore> StoreAttributeDeleted;
        /// <summary>
        /// Occurs after an Attribute object has been Updated.
        /// </summary>
        public event AttributeUpdatedDelegate<IAzManStore> StoreAttributeUpdated;
        /// <summary>
        /// Occurs after an Attribute object has been Created.
        /// </summary>
        public event AttributeCreatedDelegate<IAzManApplication> ApplicationAttributeCreated;
        /// <summary>
        /// Occurs after an Attribute object has been Deleted.
        /// </summary>
        public event AttributeDeletedDelegate<IAzManApplication> ApplicationAttributeDeleted;
        /// <summary>
        /// Occurs after an Attribute object has been Updated.
        /// </summary>
        public event AttributeUpdatedDelegate<IAzManApplication> ApplicationAttributeUpdated;
        /// <summary>
        /// Occurs after an Attribute object has been Created.
        /// </summary>
        public event AttributeCreatedDelegate<IAzManItem> ItemAttributeCreated;
        /// <summary>
        /// Occurs after an Attribute object has been Deleted.
        /// </summary>
        public event AttributeDeletedDelegate<IAzManItem> ItemAttributeDeleted;
        /// <summary>
        /// Occurs after an Attribute object has been Updated.
        /// </summary>
        public event AttributeUpdatedDelegate<IAzManItem> ItemAttributeUpdated;
        /// <summary>
        /// Occurs after an Attribute object has been Created.
        /// </summary>
        public event AttributeCreatedDelegate<IAzManAuthorization> AuthorizationAttributeCreated;
        /// <summary>
        /// Occurs after an Attribute object has been Deleted.
        /// </summary>
        public event AttributeDeletedDelegate<IAzManAuthorization> AuthorizationAttributeDeleted;
        /// <summary>
        /// Occurs after an Attribute object has been Updated.
        /// </summary>
        public event AttributeUpdatedDelegate<IAzManAuthorization> AuthorizationAttributeUpdated;
        #endregion Attribute<OWNER> Events
        #region ApplicationGroupMember Events
        /// <summary>
        /// Occurs after a SqlApplicationGroupMember object has been Deleted.
        /// </summary>
        public event ApplicationGroupMemberDeletedDelegate ApplicationGroupMemberDeleted;
        #endregion ApplicationGroupMember Events
        #region StoreGroupMember Events
        /// <summary>
        /// Occurs after a SqlStoreGroupMember object has been Deleted.
        /// </summary>
        public event StoreGroupMemberDeletedDelegate StoreGroupMemberDeleted;
        #endregion StoreGroupMember Events
        #region Item Events
        /// <summary>
        /// Occurs after a SqlAzManItem object has been Deleted.
        /// </summary>
        public event ItemDeletedDelegate ItemDeleted;
        /// <summary>
        /// Occurs after a SqlAzManItem object has been Updated.
        /// </summary>
        public event ItemUpdatedDelegate ItemUpdated;
        /// <summary>
        /// Occurs after a SqlAzManItem BizRule has been Updated.
        /// </summary>
        public event BizRuleUpdatedDelegate BizRuleUpdated;
        /// <summary>
        /// Occurs after a SqlAzManItem object has been Renamed.
        /// </summary>
        public event ItemRenamedDelegate ItemRenamed;
        /// <summary>
        /// Occurs after an Authorization object has been Created.
        /// </summary>
        public event AuthorizationCreatedDelegate AuthorizationCreated;
        /// <summary>
        /// Occurs after a Delegate has been Created.
        /// </summary>
        public event DelegateCreatedDelegate DelegateCreated;
        /// <summary>
        /// Occurs after a Delegate has been Deleted.
        /// </summary>
        public event DelegateDeletedDelegate DelegateDeleted;
        /// <summary>
        /// Occurs after an Item object has been Added as a member Item.
        /// </summary>
        public event MemberAddedDelegate MemberAdded;
        /// <summary>
        /// Occurs after an Item object has been Removed as a member Item.
        /// </summary>
        public event MemberRemovedDelegate MemberRemoved;
        #endregion Item Events
        #region Storage Events
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
        /// Occurs after NetSqlAzManMode has ben changed.
        /// </summary>
        public event NetSqlAzManModeChangeDelegate NetSqlAzManModeChanged;
        #endregion Storage Events
        #region Store Events
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
        /// Occurs after a SQL Login is Granted on the Store.
        /// </summary>
        public event StorePermissionGrantedDelegate StorePermissionGranted;
        /// <summary>
        /// Occurs after a SQL Login is Revoked on the Store.
        /// </summary>
        public event StorePermissionRevokedDelegate StorePermissionRevoked;
        #endregion Store Events
        #region AddPublisher Methods
        /// <summary>
        /// Adds an IAzManApplication publisher.
        /// </summary>
        /// <param name="publisher">The application.</param>
        internal void AddPublisher(IAzManApplication publisher)
        {
            publisher.ApplicationAttributeCreated += new AttributeCreatedDelegate<IAzManApplication>(delegate(IAzManApplication owner, IAzManAttribute<IAzManApplication> attributeCreated) { if (this.ApplicationAttributeCreated != null) this.ApplicationAttributeCreated(owner, attributeCreated); }); 
            publisher.ApplicationGroupCreated += new ApplicationGroupCreatedDelegate(delegate(IAzManApplication application, IAzManApplicationGroup applicationGroupcreated) { if (this.ApplicationGroupCreated != null) this.ApplicationGroupCreated(application, applicationGroupcreated); });
            publisher.ApplicationDeleted += new ApplicationDeletedDelegate(delegate(IAzManStore ownerStore, string applicationName) { if (this.ApplicationDeleted != null) this.ApplicationDeleted(ownerStore, applicationName); });
            publisher.ItemCreated+=new ItemCreatedDelegate(delegate(IAzManApplication application, IAzManItem itemCreated) { if (this.ItemCreated!=null) this.ItemCreated(application, itemCreated); });
            publisher.ApplicationRenamed += new ApplicationRenamedDelegate(delegate(IAzManApplication application, string oldName) { if (this.ApplicationRenamed != null) this.ApplicationRenamed(application, oldName); });
            publisher.ApplicationUpdated += new ApplicationUpdatedDelegate(delegate(IAzManApplication application, string oldDescription) { if (this.ApplicationUpdated != null) this.ApplicationUpdated(application, oldDescription); } );
            publisher.ApplicationPermissionGranted += new ApplicationPermissionGrantedDelegate(delegate(IAzManApplication application, string sqlLogin, string role) { if (this.ApplicationPermissionGranted != null) this.ApplicationPermissionGranted(application, sqlLogin, role); });
            publisher.ApplicationPermissionRevoked += new ApplicationPermissionRevokedDelegate(delegate(IAzManApplication application, string sqlLogin, string role) { if (this.ApplicationPermissionRevoked != null) this.ApplicationPermissionRevoked(application, sqlLogin, role); });
        }

        /// <summary>
        /// Adds an IAzManApplicationGroup publisher.
        /// </summary>
        /// <param name="publisher">The applicationGroup.</param>
        internal void AddPublisher(IAzManApplicationGroup publisher)
        {
            publisher.ApplicationGroupMemberCreated += new ApplicationGroupMemberCreatedDelegate(delegate(IAzManApplicationGroup applicationGroup, IAzManApplicationGroupMember applicationGroupMembercreated) { if (this.ApplicationGroupMemberCreated != null) this.ApplicationGroupMemberCreated(applicationGroup, applicationGroupMembercreated); });
            publisher.ApplicationGroupDeleted += new ApplicationGroupDeletedDelegate(delegate(IAzManApplication ownerApplication, string applicationGroupName) { if (this.ApplicationGroupDeleted != null) this.ApplicationGroupDeleted(ownerApplication, applicationGroupName); });
            publisher.ApplicationGroupLDAPQueryUpdated += new ApplicationGroupLDAPQueryUpdatedDelegate(delegate(IAzManApplicationGroup applicationGroup, string oldLDapQuery) { if (this.ApplicationGroupLDAPQueryUpdated != null) this.ApplicationGroupLDAPQueryUpdated(applicationGroup, oldLDapQuery); });
            publisher.ApplicationGroupRenamed += new ApplicationGroupRenamedDelegate(delegate(IAzManApplicationGroup applicationGroup, string oldName) { if (this.ApplicationGroupRenamed != null) this.ApplicationGroupRenamed(applicationGroup, oldName); });
            publisher.ApplicationGroupUpdated += new ApplicationGroupUpdatedDelegate(delegate(IAzManApplicationGroup applicationGroup, IAzManSid oldSid, string oldDescription, GroupType oldGroupType) { if (this.ApplicationGroupUpdated != null) this.ApplicationGroupUpdated(applicationGroup, oldSid, oldDescription, oldGroupType); });
        }

        /// <summary>
        /// Adds an IAzManStoreGroup publisher.
        /// </summary>
        /// <param name="publisher">The storeGroup.</param>
        internal void AddPublisher(IAzManStoreGroup publisher)
        {
            publisher.StoreGroupMemberCreated += new StoreGroupMemberCreatedDelegate(delegate(IAzManStoreGroup storeGroup, IAzManStoreGroupMember storeGroupMembercreated) { if (this.StoreGroupMemberCreated != null) this.StoreGroupMemberCreated(storeGroup, storeGroupMembercreated); });
            publisher.StoreGroupDeleted += new StoreGroupDeletedDelegate(delegate(IAzManStore ownerStore, string storeGroupName) { if (this.StoreGroupDeleted != null) this.StoreGroupDeleted(ownerStore, storeGroupName); });
            publisher.StoreGroupLDAPQueryUpdated += new StoreGroupLDAPQueryUpdatedDelegate(delegate(IAzManStoreGroup storeGroup, string oldLDapQuery) { if (this.StoreGroupLDAPQueryUpdated != null) this.StoreGroupLDAPQueryUpdated(storeGroup, oldLDapQuery); });
            publisher.StoreGroupRenamed += new StoreGroupRenamedDelegate(delegate(IAzManStoreGroup storeGroup, string oldName) { if (this.StoreGroupRenamed != null) this.StoreGroupRenamed(storeGroup, oldName); });
            publisher.StoreGroupUpdated += new StoreGroupUpdatedDelegate(delegate(IAzManStoreGroup storeGroup, IAzManSid oldSid, string oldDescription, GroupType oldGroupType) { if (this.StoreGroupUpdated != null) this.StoreGroupUpdated(storeGroup, oldSid, oldDescription, oldGroupType); });
        }

        /// <summary>
        /// Adds an IAzManAuthorization publisher.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        internal void AddPublisher(IAzManAuthorization publisher)
        {
            publisher.AuthorizationAttributeCreated += new AttributeCreatedDelegate<IAzManAuthorization>(delegate(IAzManAuthorization owner, IAzManAttribute<IAzManAuthorization> attributeCreated) { if (this.AuthorizationAttributeCreated != null) this.AuthorizationAttributeCreated(owner, attributeCreated); }); publisher.AuthorizationDeleted += new AuthorizationDeletedDelegate(delegate(IAzManItem ownerItem, IAzManSid owner, IAzManSid sid) { if (this.AuthorizationDeleted != null) this.AuthorizationDeleted(ownerItem, owner, sid); });
            publisher.AuthorizationUpdated += new AuthorizationUpdatedDelegate(delegate(IAzManAuthorization authorization, IAzManSid oldOwner, WhereDefined oldOwnerSidWhereDefined, IAzManSid oldSid, WhereDefined oldSidWhereDefined, AuthorizationType oldAuthorizationType, DateTime? oldValidFrom, DateTime? oldValidTo) { if (this.AuthorizationUpdated != null) this.AuthorizationUpdated(authorization, oldOwner, oldOwnerSidWhereDefined, oldSid, oldSidWhereDefined, oldAuthorizationType, oldValidFrom, oldValidTo); });
        }

        /// <summary>
        /// Adds an IAzManAttribute publisher.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        internal void AddPublisher(IAzManAttribute<IAzManAuthorization> publisher)
        {
            publisher.AttributeDeleted += new AttributeDeletedDelegate<IAzManAuthorization>(delegate(IAzManAuthorization owner, string key) { if (this.AuthorizationAttributeDeleted != null) this.AuthorizationAttributeDeleted(owner, key); });
            publisher.AttributeUpdated += new AttributeUpdatedDelegate<IAzManAuthorization>(delegate(IAzManAttribute<IAzManAuthorization> attribute, string oldKey, string oldValue) { if (this.AuthorizationAttributeUpdated != null) this.AuthorizationAttributeUpdated(attribute, oldKey, oldValue); });
        }

        /// <summary>
        /// Adds an IAzManAttribute publisher.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        internal void AddPublisher(IAzManAttribute<IAzManStore> publisher)
        {
            publisher.AttributeDeleted += new AttributeDeletedDelegate<IAzManStore>(delegate(IAzManStore owner, string key) { if (this.StoreAttributeDeleted != null) this.StoreAttributeDeleted(owner, key); });
            publisher.AttributeUpdated += new AttributeUpdatedDelegate<IAzManStore>(delegate(IAzManAttribute<IAzManStore> attribute, string oldKey, string oldValue) { if (this.StoreAttributeUpdated != null) this.StoreAttributeUpdated(attribute, oldKey, oldValue); });
        }

        /// <summary>
        /// Adds an IAzManAttribute publisher.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        internal void AddPublisher(IAzManAttribute<IAzManApplication> publisher)
        {
            publisher.AttributeDeleted += new AttributeDeletedDelegate<IAzManApplication>(delegate(IAzManApplication owner, string key) { if (this.ApplicationAttributeDeleted != null) this.ApplicationAttributeDeleted(owner, key); });
            publisher.AttributeUpdated += new AttributeUpdatedDelegate<IAzManApplication>(delegate(IAzManAttribute<IAzManApplication> attribute, string oldKey, string oldValue) { if (this.ApplicationAttributeUpdated != null) this.ApplicationAttributeUpdated(attribute, oldKey, oldValue); });
        }

        /// <summary>
        /// Adds an IAzManAttribute publisher.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        internal void AddPublisher(IAzManAttribute<IAzManItem> publisher)
        {
            publisher.AttributeDeleted += new AttributeDeletedDelegate<IAzManItem>(delegate(IAzManItem owner, string key) { if (this.ItemAttributeDeleted != null) this.ItemAttributeDeleted(owner, key); });
            publisher.AttributeUpdated += new AttributeUpdatedDelegate<IAzManItem>(delegate(IAzManAttribute<IAzManItem> attribute, string oldKey, string oldValue) { if (this.ItemAttributeUpdated != null) this.ItemAttributeUpdated(attribute, oldKey, oldValue); });
        }

        /// <summary>
        /// Adds an IAzManApplicationGroupMember publisher.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        internal void AddPublisher(IAzManApplicationGroupMember publisher)
        {
            publisher.ApplicationGroupMemberDeleted += new ApplicationGroupMemberDeletedDelegate(delegate(IAzManApplicationGroup ownerApplicationGroup, IAzManSid sid) { if (this.ApplicationGroupMemberDeleted != null) this.ApplicationGroupMemberDeleted(ownerApplicationGroup, sid); });
        }

        /// <summary>
        /// Adds an IAzManStoreGroupMember publisher.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        internal void AddPublisher(IAzManStoreGroupMember publisher)
        {
            publisher.StoreGroupMemberDeleted += new StoreGroupMemberDeletedDelegate(delegate(IAzManStoreGroup ownerStoreGroup, IAzManSid sid) { if (this.StoreGroupMemberDeleted != null) this.StoreGroupMemberDeleted(ownerStoreGroup, sid); });
        }

        /// <summary>
        /// Adds the IAzManItem publisher.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        internal void AddPublisher(IAzManItem publisher)
        {
            publisher.ItemAttributeCreated += new AttributeCreatedDelegate<IAzManItem>(delegate(IAzManItem owner, IAzManAttribute<IAzManItem> attributeCreated) { if (this.ItemAttributeCreated != null) this.ItemAttributeCreated(owner, attributeCreated); }); publisher.AuthorizationCreated += new AuthorizationCreatedDelegate(delegate(IAzManItem item, IAzManAuthorization authorizationCreated) { if (this.AuthorizationCreated != null) this.AuthorizationCreated(item, authorizationCreated); });
            publisher.DelegateCreated += new DelegateCreatedDelegate(delegate(IAzManItem item, IAzManAuthorization delegationCreated) { if (this.DelegateCreated != null) this.DelegateCreated(item, delegationCreated); });
            publisher.DelegateDeleted += new DelegateDeletedDelegate(delegate(IAzManItem item, IAzManSid delegatingUserSid, IAzManSid delegatedUserSid, RestrictedAuthorizationType authorizationType) { if (this.DelegateDeleted != null) this.DelegateDeleted(item, delegatingUserSid, delegatedUserSid, authorizationType); });
            publisher.ItemDeleted += new ItemDeletedDelegate(delegate(IAzManApplication applicationContainer, string itemName, ItemType itemType) { if (this.ItemDeleted != null) this.ItemDeleted(applicationContainer, itemName, itemType); });
            publisher.ItemRenamed += new ItemRenamedDelegate(delegate(IAzManItem item, string oldName) { if (this.ItemRenamed!=null) this.ItemRenamed(item, oldName); });
            publisher.ItemUpdated += new ItemUpdatedDelegate(delegate(IAzManItem item, string oldDescription) { if (this.ItemUpdated!=null) this.ItemUpdated(item, oldDescription); });
            publisher.BizRuleUpdated += new BizRuleUpdatedDelegate(delegate(IAzManItem item, string oldBizRule) { if (this.BizRuleUpdated != null) this.BizRuleUpdated(item, oldBizRule); });
            publisher.MemberAdded += new MemberAddedDelegate(delegate(IAzManItem item, IAzManItem member) { if (this.MemberAdded != null) this.MemberAdded(item, member); });
            publisher.MemberRemoved += new MemberRemovedDelegate(delegate(IAzManItem item, IAzManItem member) { if (this.MemberRemoved != null) this.MemberRemoved(item, member); });
        }

        /// <summary>
        /// Adds the IAzManStorage publisher.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        internal void AddPublisher(IAzManStorage publisher)
        {
            publisher.StoreOpened += new StoreOpenedDelegate(delegate(IAzManStore store) { if (this.StoreOpened != null) this.StoreOpened(store); });
            publisher.StoreCreated += new StoreCreatedDelegate(delegate(IAzManStore storeCreated) { if (this.StoreCreated != null) this.StoreCreated(storeCreated); });
            publisher.TransactionBeggined += new TransactionBeginnedDelegate(delegate() { if (this.TransactionBeggined != null) this.TransactionBeggined(); });
            publisher.TransactionTerminated += new TransactionTerminatedDelegate(delegate(bool success) { if (this.TransactionTerminated != null) this.TransactionTerminated(success); });
            publisher.NetSqlAzManModeChanged += new NetSqlAzManModeChangeDelegate(delegate(NetSqlAzManMode oldMode, NetSqlAzManMode newMode) { if (this.NetSqlAzManModeChanged != null) this.NetSqlAzManModeChanged(oldMode, newMode); });
        }

        /// <summary>
        /// Adds the IAzManStore publisher.
        /// </summary>
        /// <param name="publisher">The publisher.</param>
        internal void AddPublisher(IAzManStore publisher)
        {
            publisher.StoreAttributeCreated += new AttributeCreatedDelegate<IAzManStore>(delegate(IAzManStore owner, IAzManAttribute<IAzManStore> attributeCreated) { if (this.StoreAttributeCreated != null) this.StoreAttributeCreated(owner, attributeCreated); });
            publisher.ApplicationOpened+=new ApplicationOpenedDelegate(delegate(IAzManApplication application) { if (this.ApplicationOpened!=null) this.ApplicationOpened(application); });
            publisher.ApplicationCreated += new ApplicationCreatedDelegate(delegate(IAzManStore store, IAzManApplication applicationCreated) { if (this.ApplicationCreated != null) this.ApplicationCreated(store, applicationCreated); });
            publisher.StoreDeleted += new StoreDeletedDelegate(delegate(IAzManStorage ownerStorage, string storeName) { if (this.StoreDeleted != null) this.StoreDeleted(ownerStorage, storeName); });
            publisher.StoreGroupCreated += new StoreGroupCreatedDelegate(delegate(IAzManStore store, IAzManStoreGroup storeGroupCreated) { if (this.StoreGroupCreated != null) this.StoreGroupCreated(store, storeGroupCreated); });
            publisher.StoreRenamed += new StoreRenamedDelegate(delegate(IAzManStore store, string oldName) { if (this.StoreRenamed != null) this.StoreRenamed(store, oldName); });
            publisher.StoreUpdated += new StoreUpdatedDelegate(delegate(IAzManStore store, string oldDescription) { if (this.StoreUpdated != null) this.StoreUpdated(store, oldDescription); });
            publisher.StorePermissionGranted += new StorePermissionGrantedDelegate(delegate(IAzManStore store, string sqlLogin, string role) { if (this.StorePermissionGranted != null) this.StorePermissionGranted(store, sqlLogin, role); });
            publisher.StorePermissionRevoked += new StorePermissionRevokedDelegate(delegate(IAzManStore store, string sqlLogin, string role) { if (this.StorePermissionRevoked != null) this.StorePermissionRevoked(store, sqlLogin, role); });
        }
        #endregion AddPublisher Methods
    }
}
