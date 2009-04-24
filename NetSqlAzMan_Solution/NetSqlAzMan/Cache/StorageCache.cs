using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using NetSqlAzMan;
using NetSqlAzMan.LINQ;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.ENS;

namespace NetSqlAzMan.Cache
{
    /// <summary>
    /// Storage Cache class able to cache all Storage data without querying the DB Storage.
    /// </summary>
    [Serializable()]
    public sealed class StorageCache
    {
        #region Fields
        private SqlAzManStorage storage;
        private Hashtable ldapQueryResults;
        #endregion Fields
        #region Properties
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        public string ConnectionString
        {
            get
            {
                return this.storage.ConnectionString;
            }
            set
            {
                this.storage = new SqlAzManStorage(value);
                this.storage.db.ObjectTrackingEnabled = false;
            }
        }
        /// <summary>
        /// Gets the storage.
        /// </summary>
        /// <value>The storage.</value>
        public SqlAzManStorage Storage
        {
            get
            {
                return this.storage;
            }
            set
            {
                this.storage = value;
            }
        }
        #endregion Properties
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="StorageCache"/> class.
        /// </summary>
        public StorageCache()
        {
            this.ldapQueryResults = new Hashtable();
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="StorageCache"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public StorageCache(string connectionString) : this()
        {
            this.ConnectionString = connectionString;
        }
        #endregion Constructors
        #region Private Methods
        private IEnumerable<IAzManSid> getStoreGroupSidMembers(IAzManStore store, bool isMember, IAzManSid groupObjectSid)
        {
            IEnumerable<IAzManSid> result = new IAzManSid[0];
            var storeGroup = (from sg in store.StoreGroups.Values
                              where sg.SID.StringValue == groupObjectSid.StringValue
                              select sg).First();


            //BASIC GROUP
            if (storeGroup.GroupType == GroupType.Basic)
            {
                //Windows SIDs
                var membersResult = from sgm in storeGroup.Members.Values
                                    where sgm.StoreGroup.StoreGroupId == storeGroup.StoreGroupId &&
                                    sgm.IsMember == isMember &&
                                    ((this.storage.Mode == NetSqlAzManMode.Administrator && (sgm.WhereDefined == WhereDefined.LDAP || sgm.WhereDefined == WhereDefined.Database)) ||
                                     (this.storage.Mode == NetSqlAzManMode.Developer && sgm.WhereDefined >= WhereDefined.LDAP && sgm.WhereDefined <= WhereDefined.Database))
                                    select sgm.SID;
                result = result.Union(membersResult);

                //Sub Store Groups
                var subMembers = from sgm in storeGroup.Members.Values
                                 where sgm.StoreGroup.StoreGroupId == storeGroup.StoreGroupId &&
                                 sgm.IsMember == isMember &&
                                 sgm.WhereDefined == WhereDefined.Store
                                 select sgm;
                foreach (var subMember in subMembers)
                {
                    //recursive call
                    bool nonMemberType;
                    if (isMember)
                    {
                        if (subMember.IsMember == false)
                            nonMemberType = false;
                        else
                            nonMemberType = true;
                    }
                    else
                    {
                        if (subMember.IsMember == false)
                            nonMemberType = true;
                        else
                            nonMemberType = false;
                    }
                    var subMembersResult = this.getStoreGroupSidMembers(store, nonMemberType, subMember.SID);
                    result = result.Union(subMembersResult);
                }
                return result;
            }
            else if (storeGroup.GroupType == GroupType.LDapQuery && isMember == true)
            {
                return this.getCachedLDAPQueryResults(storeGroup);
            }
            else
            {
                //Empty result
                return result;
            }
        }
        private IEnumerable<IAzManSid> getApplicationGroupSidMembers(IAzManApplication application, bool isMember, IAzManSid groupObjectSid)
        {
            var applicationGroup = (from ag in application.ApplicationGroups.Values
                                    where ag.SID.StringValue == groupObjectSid.StringValue
                                    select ag).First();

            IEnumerable<IAzManSid> result = new IAzManSid[0];

            //Store Group members
            var membersResult = from agm in applicationGroup.Members.Values
                                where agm.ApplicationGroup.ApplicationGroupId == applicationGroup.ApplicationGroupId &&
                                agm.IsMember == isMember &&
                                ((this.storage.Mode == NetSqlAzManMode.Administrator && (agm.WhereDefined == WhereDefined.LDAP || agm.WhereDefined == WhereDefined.Database)) ||
                                 (this.storage.Mode == NetSqlAzManMode.Developer && agm.WhereDefined >= WhereDefined.LDAP && agm.WhereDefined <= WhereDefined.Database))
                                select agm.SID;
            result = result.Union(membersResult);

            //BASIC GROUP
            if (applicationGroup.GroupType == GroupType.Basic)
            {
                //Sub Store Groups
                var subMembers = from agm in applicationGroup.Members.Values
                                 where agm.ApplicationGroup.ApplicationGroupId == applicationGroup.ApplicationGroupId &&
                                 agm.IsMember == isMember &&
                                 agm.WhereDefined == WhereDefined.Store
                                 select agm;

                foreach (var subMember in subMembers)
                {
                    //recursive call
                    bool nonMemberType;
                    if (isMember)
                    {
                        if (subMember.IsMember == false)
                            nonMemberType = false;
                        else
                            nonMemberType = true;
                    }
                    else
                    {
                        if (subMember.IsMember == false)
                            nonMemberType = true;
                        else
                            nonMemberType = false;
                    }
                    var subMembersResult = this.getStoreGroupSidMembers(application.Store, nonMemberType, subMember.SID);
                    return result = result.Union(subMembersResult);
                }
                //Sub Application Groups
                var subMembers2 = from agm in applicationGroup.Members.Values
                                  where agm.ApplicationGroup.ApplicationGroupId == applicationGroup.ApplicationGroupId &&
                                  agm.IsMember == isMember &&
                                  agm.WhereDefined == WhereDefined.Application
                                  select agm;

                foreach (var subMember in subMembers2)
                {
                    //recursive call
                    bool nonMemberType;
                    if (isMember)
                    {
                        if (subMember.IsMember == false)
                            nonMemberType = false;
                        else
                            nonMemberType = true;
                    }
                    else
                    {
                        if (subMember.IsMember == false)
                            nonMemberType = true;
                        else
                            nonMemberType = false;
                    }
                    var subMembersResult = this.getStoreGroupSidMembers(application.Store, nonMemberType, subMember.SID);
                    result = result.Union(subMembersResult);
                }
                return result;
            }
            else if (applicationGroup.GroupType == GroupType.LDapQuery && isMember == true)
            {
                //LDAP Group
                return this.getCachedLDAPQueryResults(applicationGroup);
            }
            else
            {
                //Empty result
                return new IAzManSid[0];
            }
        }
        private IAzManSid[] getCachedLDAPQueryResults(IAzManStoreGroup storeGroup)
        {
            string key = "storeGroup " + storeGroup.StoreGroupId.ToString();
            if (!this.ldapQueryResults.ContainsKey(key))
            {
                //LDAP Group
                var ldapQueryResult = storeGroup.ExecuteLDAPQuery();
                IAzManSid[] membersResults = new IAzManSid[ldapQueryResult.Count];
                for (int i = 0; i < ldapQueryResult.Count; i++)
                {
                    membersResults[i] = new SqlAzManSID((byte[])ldapQueryResult[i].Properties["objectSid"][0]);
                }
                this.ldapQueryResults.Add(key, membersResults);
            }
            return (IAzManSid[])this.ldapQueryResults[key];
        }
        private IAzManSid[] getCachedLDAPQueryResults(IAzManApplicationGroup applicationGroup)
        {
            string key = "applicationGroup " + applicationGroup.ApplicationGroupId.ToString();
            if (!this.ldapQueryResults.ContainsKey(key))
            {
                //LDAP Group
                var ldapQueryResult = applicationGroup.ExecuteLDAPQuery();
                IAzManSid[] membersResults = new IAzManSid[ldapQueryResult.Count];
                for (int i = 0; i < ldapQueryResult.Count; i++)
                {
                    membersResults[i] = new SqlAzManSID((byte[])ldapQueryResult[i].Properties["objectSid"][0]);
                }
                this.ldapQueryResults.Add(key, membersResults);
            }
            return (IAzManSid[])this.ldapQueryResults[key];
        }
        #endregion Private Methods
        #region Public Methods
        /// <summary>
        /// Build a cache version of the Storage.
        /// </summary>
        public void BuildStorageCache()
        {
            this.BuildStorageCache(null, null);
        }
        /// <summary>
        /// Build a cache version of the Storage.
        /// </summary>
        /// <param name="storeNameFilter">The store name filter.</param>
        public void BuildStorageCache(string storeNameFilter)
        {
            this.BuildStorageCache(storeNameFilter, null);
        }
        /// <summary>
        /// Build a cache version of the Storage.
        /// </summary>
        /// <param name="storeNameFilter">The store name filter.</param>
        /// <param name="applicationNameFilter">The application name filter.</param>
        public void BuildStorageCache(string storeNameFilter, string applicationNameFilter)
        {
            System.Diagnostics.Debug.WriteLine(String.Format("Cache Building started at {0}", DateTime.Now));
            SqlAzManStorage newStorage = new SqlAzManStorage(this.storage.ConnectionString);
            SqlAzManItem.ClearBizRuleAssemblyCache();
            var db = newStorage.db;
            //Just iterate over all collections to cache all values
            newStorage.OpenConnection();
            try
            {
                if (!String.IsNullOrEmpty(storeNameFilter))
                    storeNameFilter = storeNameFilter.Trim();
                if (!String.IsNullOrEmpty(applicationNameFilter))
                    applicationNameFilter = applicationNameFilter.Trim();
                var filteredStores = from s in db.Stores()
                                     where 
                                     !String.IsNullOrEmpty(storeNameFilter) && s.Name.Contains(storeNameFilter)
                                     ||
                                     String.IsNullOrEmpty(storeNameFilter)
                                     select s;

                if (filteredStores.Count() == 0)
                    throw new ArgumentOutOfRangeException("storeNameFilter");

                var filteredApplications = from s in filteredStores
                                           join a in db.Applications() on s.StoreId equals a.StoreId
                                           where
                                           !String.IsNullOrEmpty(applicationNameFilter) && a.Name.Contains(applicationNameFilter)
                                           ||
                                           String.IsNullOrEmpty(applicationNameFilter)
                                           select a;

                if (filteredApplications.Count()==0)
                    throw new ArgumentOutOfRangeException("applicationNameFilter");

                //All data
                SqlAzManENS ens = (SqlAzManENS)newStorage.ENS;
                var allStores = (from s in filteredStores orderby s.Name select s).ToList();;
                var allStoreAttributes = (from sa in db.StoreAttributes() orderby sa.AttributeKey select sa).ToList();
                var allStoreGroups = (from sg in db.StoreGroups() orderby sg.Name select sg).ToList();
                var allStoreGroupMembers = (from sgm in db.StoreGroupMembers() select sgm).ToList();
                var allApplications = (from a in filteredApplications orderby a.Name select a).ToList();
                var allApplicationAttributes = (from aa in db.ApplicationAttributes() orderby aa.AttributeKey select aa).ToList();
                var allApplicationGroups = (from ag in db.ApplicationGroups() orderby ag.Name select ag).ToList();
                var allApplicationGroupMembers = (from agm in db.ApplicationGroupMembers() select agm).ToList();
                var allItems = (from i in db.Items() orderby i.Name select i).ToList();
                var allItemsHierarchy = (from ih in db.ItemsHierarchyView select ih).ToList();
                var allItemAttributes = (from ia in db.ItemAttributes() select ia).ToList();
                var allBizRules = (from br in db.BizRules() select br).ToList();
                var allAuthorizations = (from a in db.Authorizations() select a).ToList();
                var allAuthorizationAttributes = (from aa in db.AuthorizationAttributes() orderby aa.AttributeKey select aa).ToList();

                //Stores
                var stores = allStores.ToDictionary<StoresResult, string, IAzManStore>(sr => sr.Name, sr =>
                {
                    byte netsqlazmanFixedServerRole = 0;
                    if (newStorage.IAmAdmin)
                    {
                        netsqlazmanFixedServerRole = 3;
                    }
                    else
                    {
                        var res1 = db.CheckStorePermissions(sr.StoreId, 2);
                        var res2 = db.CheckStorePermissions(sr.StoreId, 1);
                        if (res1.HasValue && res1.Value)
                            netsqlazmanFixedServerRole = 2;
                        else if (res2.HasValue && res2.Value)
                            netsqlazmanFixedServerRole = 1;
                    }
                    return new SqlAzManStore(db, newStorage, sr.StoreId.Value, sr.Name, sr.Description, netsqlazmanFixedServerRole, ens);
                });
                newStorage.stores = stores;
                foreach (IAzManStore store in newStorage.Stores.Values)
                {
                    //Store Groups
                    var storeGroups = allStoreGroups.Where(sgr=>sgr.StoreId == store.StoreId).ToDictionary<StoreGroupsResult, string, IAzManStoreGroup>(sgr => sgr.Name, sgr =>
                    {
                        return new SqlAzManStoreGroup(db, store, sgr.StoreGroupId.Value, new SqlAzManSID(sgr.ObjectSid.ToArray()), sgr.Name, sgr.Description, sgr.LDapQuery, (GroupType)sgr.GroupType.Value, ens);
                    });
                    ((SqlAzManStore)store).storeGroups = (from sgv in storeGroups.Values
                                                         where sgv.Store.StoreId == store.StoreId
                                                         select sgv).ToDictionary(sg=>sg.Name);
                    foreach (IAzManStoreGroup storeGroup in store.StoreGroups.Values)
                    {
                        //Store Groups Members
                        if (storeGroup.GroupType == GroupType.Basic)
                        {
                            var storeGroupMembers = allStoreGroupMembers.Where(sgm => sgm.StoreGroupId == storeGroup.StoreGroupId).ToDictionary<StoreGroupMembersResult, IAzManSid, IAzManStoreGroupMember>(sgm => new SqlAzManSID(sgm.ObjectSid.ToArray(),(WhereDefined)sgm.WhereDefined.Value == WhereDefined.Database), sgm =>
                                {
                                    return new SqlAzManStoreGroupMember(db, storeGroup, sgm.StoreGroupMemberId.Value, new SqlAzManSID(sgm.ObjectSid.ToArray(), (WhereDefined)sgm.WhereDefined.Value == WhereDefined.Database), (WhereDefined)sgm.WhereDefined.Value, sgm.IsMember.Value, ens);
                                });
                            ((SqlAzManStoreGroup)storeGroup).members = storeGroupMembers;
                            //foreach (IAzManStoreGroupMember storeGroupMember in storeGroup.Members.Values)
                            //{

                            //}
                        }
                        else if (storeGroup.GroupType == GroupType.LDapQuery)
                        {
                            this.getCachedLDAPQueryResults(storeGroup);
                        }

                    }
                    //Store Attributes
                    var storeAttributes = allStoreAttributes.Where(sa => sa.StoreId == store.StoreId).ToDictionary<StoreAttributesResult, string, IAzManAttribute<IAzManStore>>(sa => sa.AttributeKey, sa =>
                        {
                            return new SqlAzManStoreAttribute(db, store, sa.StoreAttributeId.Value, sa.AttributeKey, sa.AttributeValue, ens);
                        });
                    ((SqlAzManStore)store).attributes = storeAttributes;
                    //foreach (IAzManAttribute<IAzManStore> storeAttribute in store.Attributes.Values)
                    //{

                    //}
                    //Applications
                    var applications = allApplications.Where(a => a.StoreId == store.StoreId).ToDictionary<ApplicationsResult, string, IAzManApplication>(a => a.Name, a =>
                        {
                            byte netsqlazmanFixedServerRole = 0;
                            if (store.IAmAdmin)
                            {
                                netsqlazmanFixedServerRole = 3;
                            }
                            else
                            {
                                var r1 = db.CheckApplicationPermissions(a.ApplicationId.Value, 2);
                                var r2 = db.CheckApplicationPermissions(a.ApplicationId.Value, 1);
                                if (r1.HasValue && r1.Value)
                                    netsqlazmanFixedServerRole = 2;
                                else if (r2.HasValue && r2.Value)
                                    netsqlazmanFixedServerRole = 1;
                            }
                            return new SqlAzManApplication(db, store, a.ApplicationId.Value, a.Name, a.Description, netsqlazmanFixedServerRole, ens);
                        });
                    ((SqlAzManStore)store).applications = applications;
                    foreach (IAzManApplication application in store.Applications.Values)
                    {
                        //Application Groups
                        var applicationGroups = allApplicationGroups.Where(ag => ag.ApplicationId == application.ApplicationId).ToDictionary<ApplicationGroupsResult, string, IAzManApplicationGroup>(ag => ag.Name, ag =>
                            {
                                return new SqlAzManApplicationGroup(db, application, ag.ApplicationGroupId.Value, new SqlAzManSID(ag.ObjectSid.ToArray()), ag.Name, ag.Description, ag.LDapQuery, (GroupType)ag.GroupType, ens);
                            });
                        ((SqlAzManApplication)application).applicationGroups = applicationGroups;
                        foreach (IAzManApplicationGroup applicationGroup in application.ApplicationGroups.Values)
                        {
                            if (applicationGroup.GroupType == GroupType.Basic)
                            {
                                //Application Group Members
                                var applicationGroupMembers = allApplicationGroupMembers.Where(agm => agm.ApplicationGroupId == applicationGroup.ApplicationGroupId).ToDictionary<ApplicationGroupMembersResult, IAzManSid, IAzManApplicationGroupMember>(agm => new SqlAzManSID(agm.ObjectSid.ToArray(), (WhereDefined)agm.WhereDefined.Value == WhereDefined.Database), agm =>
                                {
                                    return new SqlAzManApplicationGroupMember(db, applicationGroup, agm.ApplicationGroupMemberId.Value, new SqlAzManSID(agm.ObjectSid.ToArray(), (WhereDefined)agm.WhereDefined.Value == WhereDefined.Database), (WhereDefined)agm.WhereDefined.Value, agm.IsMember.Value, ens);
                                });
                                ((SqlAzManApplicationGroup)applicationGroup).members = applicationGroupMembers;
                                //foreach (IAzManApplicationGroupMember applicationGroupMember in applicationGroup.Members.Values)
                                //{

                                //}
                            }
                            else if (applicationGroup.GroupType == GroupType.LDapQuery)
                            {
                                this.getCachedLDAPQueryResults(applicationGroup);
                            }
                        }
                        //Application Attributes
                        var applicationAttributes = allApplicationAttributes.Where(sa => sa.ApplicationId == application.ApplicationId).ToDictionary<ApplicationAttributesResult, string, IAzManAttribute<IAzManApplication>>(sa => sa.AttributeKey, sa =>
                        {
                            return new SqlAzManApplicationAttribute(db, application, sa.ApplicationAttributeId.Value, sa.AttributeKey, sa.AttributeValue, ens);
                        });
                        ((SqlAzManApplication)application).attributes = applicationAttributes;
                        //foreach (IAzManAttribute<IAzManApplication> applicationAttribute in application.Attributes.Values)
                        //{

                        //}
                        //Item
                        var items = allItems.Where(i => i.ApplicationId == application.ApplicationId).ToDictionary<ItemsResult, string, IAzManItem>(i => i.Name, i =>
                            {
                                BizRulesResult bizRule = null;
                                if (i.BizRuleId.HasValue)
                                {
                                bizRule = (from br in allBizRules
                                              where br.BizRuleId == i.BizRuleId
                                              select br).First();
                                }
                                return new SqlAzManItem(db, application, i.ItemId.Value, i.Name, i.Description, (ItemType)i.ItemType, bizRule != null ? bizRule.BizRuleSource : String.Empty, bizRule != null ? (BizRuleSourceLanguage)bizRule.BizRuleLanguage.Value : default(BizRuleSourceLanguage), ens);
                            });
                        ((SqlAzManApplication)application).items = items;
                        var itemsOfApplication = from i in allItems
                                                 where i.ApplicationId == application.ApplicationId
                                                 select i;
                        var itemsHierarchy = from ih in allItemsHierarchy
                                             where ih.ApplicationId == application.ApplicationId
                                             select ih;
                        foreach (IAzManItem item in application.Items.Values)
                        {
                            var itemAttributes = allItemAttributes.Where(sa => sa.ItemId == item.ItemId).ToDictionary<ItemAttributesResult, string, IAzManAttribute<IAzManItem>>(sa => sa.AttributeKey, sa =>
                            {
                                return new SqlAzManItemAttribute(db, item, sa.ItemAttributeId.Value, sa.AttributeKey, sa.AttributeValue, ens);
                            });
                            ((SqlAzManItem)item).attributes = itemAttributes;
                            //Item Attributes
                            //foreach (IAzManAttribute<IAzManItem> itemAttribute in item.Attributes.Values)
                            //{

                            //}
                            
                            var memberItemsId = from ihv in itemsHierarchy
                                               where ihv.ItemId == item.ItemId
                                               select ihv.MemberItemId;
                            var itemMembers = (from i in itemsOfApplication
                                               where memberItemsId.Contains(i.ItemId.Value)
                                               select i).ToDictionary<ItemsResult, string, IAzManItem>(i => i.Name, i =>
                                                  {
                                                      BizRulesResult bizRule = null;
                                                      if (i.BizRuleId.HasValue)
                                                      {
                                                          bizRule = (from br in allBizRules
                                                                     where br.BizRuleId == i.BizRuleId
                                                                     select br).First();
                                                      }
                                                      return new SqlAzManItem(db, application, i.ItemId.Value, i.Name, i.Description, (ItemType)i.ItemType, bizRule != null ? bizRule.BizRuleSource : String.Empty, bizRule != null ? (BizRuleSourceLanguage)bizRule.BizRuleLanguage.Value : default(BizRuleSourceLanguage), ens);
                                                  });
                            ((SqlAzManItem)item).members = itemMembers;
                            //foreach (IAzManItem member in item.Members.Values)
                            //{ 
                            
                            //}

                            //Authorizations
                            var authorizations = allAuthorizations.Where(a => a.ItemId == item.ItemId).ToDictionary<AuthorizationsResult, int, IAzManAuthorization>(a => a.AuthorizationId.Value, a =>
                                {
                                    //return new SqlAzManAuthorization(db, item, a.AuthorizationId.Value, new SqlAzManSID(a.OwnerSid.ToArray()), (WhereDefined)a.OwnerSidWhereDefined, new SqlAzManSID(a.ObjectSid.ToArray()), (WhereDefined)a.ObjectSidWhereDefined, (AuthorizationType)a.AuthorizationType, a.ValidFrom, a.ValidTo, ens);
                                    //Thanks to: K.Overmars - koelvin@hotmail.com (http://sourceforge.net/tracker/index.php?func=detail&aid=1939219&group_id=165814&atid=836877)
                                    return new SqlAzManAuthorization(db, item, a.AuthorizationId.Value, new SqlAzManSID(a.OwnerSid.ToArray(), a.OwnerSidWhereDefined == (byte)(WhereDefined.Database)), (WhereDefined)a.OwnerSidWhereDefined, new SqlAzManSID(a.ObjectSid.ToArray(), a.ObjectSidWhereDefined == (byte)(WhereDefined.Database)), (WhereDefined)a.ObjectSidWhereDefined, (AuthorizationType)a.AuthorizationType, a.ValidFrom, a.ValidTo, ens);

                                }).Values.ToList();
                            ((SqlAzManItem)item).authorizations = authorizations;
                            foreach (IAzManAuthorization authorization in item.Authorizations)
                            {
                                //Authorization Attributes
                                var authorizationAttributes = allAuthorizationAttributes.Where(sa => sa.AuthorizationId == authorization.AuthorizationId).ToDictionary<AuthorizationAttributesResult, string, IAzManAttribute<IAzManAuthorization>>(sa => sa.AttributeKey, sa =>
                                {
                                    return new SqlAzManAuthorizationAttribute(db, authorization, sa.AuthorizationAttributeId.Value, sa.AttributeKey, sa.AttributeValue, ens);
                                });
                                ((SqlAzManAuthorization)authorization).attributes = authorizationAttributes;
                                //foreach (IAzManAttribute<IAzManAuthorization> authorizationAttribute in authorization.Attributes.Values)
                                //{

                                //}
                            }
                            //Biz Rules
                            if (!String.IsNullOrEmpty(item.BizRuleSource))
                                item.LoadBizRuleAssembly();
                        }
                    }
                }
                lock (this)
                {
                    this.storage = newStorage;
                }
            }
            finally
            {
                newStorage.CloseConnection();
            }
        }

        /// <summary>
        /// Checks the access [WINDOWS USERS ONLY].
        /// </summary>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="applicationName">Name of the application.</param>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="userSSid">The user S sid.</param>
        /// <param name="groupsSSid">The groups S sid.</param>
        /// <param name="validFor">The valid for.</param>
        /// <param name="operationsOnly">if set to <c>true</c> [operations only].</param>
        /// <param name="attributes">The attributes.</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns></returns>
        public AuthorizationType CheckAccess(string storeName, string applicationName, string itemName, string userSSid, string[] groupsSSid, DateTime validFor, bool operationsOnly, out List<KeyValuePair<string, string>> attributes, params KeyValuePair<string, object>[] contextParameters)
        {
            return this.internalCheckAccess(storeName, applicationName, itemName, userSSid, groupsSSid, validFor, operationsOnly, true, out attributes, contextParameters);
        }

        /// <summary>
        /// Checks the access [WINDOWS USERS ONLY].
        /// </summary>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="applicationName">Name of the application.</param>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="userSSid">The user S sid.</param>
        /// <param name="groupsSSid">The groups S sid.</param>
        /// <param name="validFor">The valid for.</param>
        /// <param name="operationsOnly">if set to <c>true</c> [operations only].</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns></returns>
        public AuthorizationType CheckAccess(string storeName, string applicationName, string itemName, string userSSid, string[] groupsSSid, DateTime validFor, bool operationsOnly, params KeyValuePair<string, object>[] contextParameters)
        {
            List<KeyValuePair<string, string>> attributes;
            return this.internalCheckAccess(storeName, applicationName, itemName, userSSid, groupsSSid, validFor, operationsOnly, false, out attributes, contextParameters);
        }

        /// <summary>
        /// Checks the access [DB USERS ONLY].
        /// </summary>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="applicationName">Name of the application.</param>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="DBuserSSid">The D buser S sid.</param>
        /// <param name="validFor">The valid for.</param>
        /// <param name="operationsOnly">if set to <c>true</c> [operations only].</param>
        /// <param name="attributes">The attributes.</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns></returns>
        public AuthorizationType CheckAccess(string storeName, string applicationName, string itemName, string DBuserSSid, DateTime validFor, bool operationsOnly, out List<KeyValuePair<string, string>> attributes, params KeyValuePair<string, object>[] contextParameters)
        {
            return this.internalCheckAccess(storeName, applicationName, itemName, DBuserSSid, new string[0], validFor, operationsOnly, true, out attributes, contextParameters);
        }

        /// <summary>
        /// Checks the access [DB USERS ONLY].
        /// </summary>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="applicationName">Name of the application.</param>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="DBuserSSid">The D buser S sid.</param>
        /// <param name="validFor">The valid for.</param>
        /// <param name="operationsOnly">if set to <c>true</c> [operations only].</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns></returns>
        public AuthorizationType CheckAccess(string storeName, string applicationName, string itemName, string DBuserSSid, DateTime validFor, bool operationsOnly, params KeyValuePair<string, object>[] contextParameters)
        {
            List<KeyValuePair<string, string>> attributes;
            return this.internalCheckAccess(storeName, applicationName, itemName, DBuserSSid, new string[0], validFor, operationsOnly, false, out attributes, contextParameters);
        }

        internal AuthorizationType internalCheckAccess(string storeName, string applicationName, string itemName, string userSSid, string[] groupsSSid, DateTime validFor, bool operationsOnly, bool retrieveAttributes, out List<KeyValuePair<string, string>> attributes, params KeyValuePair<string, object>[] contextParameters)
        {
            AuthorizationType authorizationType = AuthorizationType.Neutral;
            attributes = new List<KeyValuePair<string, string>>();
            #region NAMES VALIDATION
            storeName = storeName.Trim();
            applicationName = applicationName.Trim();
            itemName = itemName.Trim();
            var store = (from s in this.storage.Stores.Values
                         where String.Compare(s.Name, storeName, true) == 0
                         select s).FirstOrDefault();
            if (store == null) throw new ArgumentOutOfRangeException("storeName", "Store not found or Store permission denied.");
            var application = (from a in store.Applications.Values
                               where String.Compare(a.Name, applicationName, true) == 0
                               select a).FirstOrDefault();
            if (application == null) throw new ArgumentOutOfRangeException("applicationName", "Application not found or Application permission denied.");
            var item = (from a in application.Items.Values
                        where String.Compare(a.Name, itemName, true) == 0
                        select a).FirstOrDefault();
            if (item == null) throw new ArgumentOutOfRangeException("itemName", "Item not found.");
            #endregion NAMES VALIDATION
            #region RECURSIVE CALL
            var parentItems = from it in application.Items.Values
                              from m in it.Members.Values
                              where m.ItemId == item.ItemId
                              select it;
            foreach (var parentItem in parentItems)
            {
                List<KeyValuePair<string, string>> localAttributes;
                AuthorizationType parentAuthorizationType = this.internalCheckAccess(storeName, applicationName, parentItem.Name, userSSid, groupsSSid, validFor, operationsOnly, retrieveAttributes, out localAttributes, contextParameters);
                if (retrieveAttributes && (parentAuthorizationType == AuthorizationType.Allow || parentAuthorizationType == AuthorizationType.AllowWithDelegation))
                    attributes.AddRange(localAttributes);
                authorizationType = SqlAzManItem.mergeAuthorizations(authorizationType, parentAuthorizationType);
            }
            if (authorizationType == AuthorizationType.AllowWithDelegation)
                authorizationType = AuthorizationType.Allow; //AllowWithDelegation becomes Just Allow (if comes from parents)
            #endregion RECURSIVE CALL
            #region BIZ RULE CHECK
            if (!String.IsNullOrEmpty(item.BizRuleSource))
            {
                try
                {
                    AuthorizationType forcedCheckAccessResult = authorizationType;
                    Hashtable ctxParameters = new Hashtable();
                    if (contextParameters != null)
                    {
                        foreach (KeyValuePair<string, object> kv in contextParameters)
                        {
                            ctxParameters.Add(kv.Key, kv.Value);
                        }
                    }
                    bool bizRuleResult = this.storage.executeBizRule(item, new SqlAzManSID(userSSid), ctxParameters, ref forcedCheckAccessResult);
                    if (bizRuleResult == true)
                    {
                        authorizationType = SqlAzManItem.mergeAuthorizations(authorizationType, forcedCheckAccessResult);
                    }
                    else
                    {
                        return authorizationType;
                    }
                }
                catch (Exception ex)
                {
                    string msg = String.Format("Business Rule Error:{0}\r\nItem Name:{1}, Application Name: {2}, Store Name: {3}", ex.Message, itemName, application.Name, storeName);
                    throw new Exception(msg, ex);
                }
            }
            #endregion BIZ RULE CHECK
            #region CHECK ACCESS ON ITEM
            //Store Attributes
            if (retrieveAttributes)
            {
                foreach (IAzManAttribute<IAzManStore> storeAttribute in item.Application.Store.Attributes.Values)
                {
                    KeyValuePair<string, string> attr = new KeyValuePair<string, string>(storeAttribute.Key, storeAttribute.Value);
                    if (!attributes.Contains(attr))
                        attributes.Add(attr);
                }
            }
            //Application Attributes
            if (retrieveAttributes)
            {
                foreach (IAzManAttribute<IAzManApplication> applicationAttribute in item.Application.Attributes.Values)
                {
                    KeyValuePair<string, string> attr = new KeyValuePair<string, string>(applicationAttribute.Key, applicationAttribute.Value);
                    if (!attributes.Contains(attr))
                        attributes.Add(attr);
                }
            }
            //Item Attributes
            if (retrieveAttributes)
            {
                foreach (IAzManAttribute<IAzManItem> itemAttribute in item.Attributes.Values)
                {
                    attributes.Add(new KeyValuePair<string, string>(itemAttribute.Key, itemAttribute.Value));
                }
            }
            //memo: WhereDefined can be:0 - Store; 1 - Application; 2 - LDAP; 3 - Local; 4 - Database
            var authz = from a in item.Authorizations
                        where a.Item.ItemId == item.ItemId &&
                        a.SID.StringValue == userSSid &&
                        (a.ValidFrom == null && a.ValidTo == null ||
                        validFor >= a.ValidFrom && a.ValidTo == null ||
                        validFor <= a.ValidTo && a.ValidFrom == null ||
                        validFor >= a.ValidFrom && validFor <= a.ValidTo) &&
                        a.AuthorizationType != AuthorizationType.Neutral &&
                        ((this.storage.Mode == NetSqlAzManMode.Administrator && (a.SidWhereDefined == WhereDefined.LDAP || a.SidWhereDefined == WhereDefined.Database)) ||
                        (this.storage.Mode == NetSqlAzManMode.Developer && a.SidWhereDefined >= WhereDefined.LDAP && a.SidWhereDefined <= WhereDefined.Database))
                        select a;
            foreach (var auth in authz)
            {
                authorizationType = SqlAzManItem.mergeAuthorizations(authorizationType, auth.AuthorizationType);
                //Authorization Attributes
                if (retrieveAttributes)
                {
                    foreach (IAzManAttribute<IAzManAuthorization> authorizationAttribute in auth.Attributes.Values)
                    {
                        attributes.Add(new KeyValuePair<string, string>(authorizationAttribute.Key, authorizationAttribute.Value));
                    }
                }
            }
            #endregion CHECK ACCESS ON ITEM
            #region CHECK ACCESS FOR USER GROUPS AUTHORIZATIONS
            authz = from a in item.Authorizations
                    from g in groupsSSid
                    where String.Compare(a.Item.Name, itemName, true) == 0 &&
                    g == a.SID.StringValue &&
                    (a.ValidFrom == null && a.ValidTo == null ||
                    validFor >= a.ValidFrom.Value && a.ValidTo == null ||
                    validFor <= a.ValidTo.Value && a.ValidFrom == null ||
                    validFor >= a.ValidFrom && validFor <= a.ValidTo.Value) &&
                    a.AuthorizationType != AuthorizationType.Neutral &&
                    ((this.storage.Mode == NetSqlAzManMode.Administrator && (a.SidWhereDefined == WhereDefined.LDAP || a.SidWhereDefined == WhereDefined.Database)) ||
                    (this.storage.Mode == NetSqlAzManMode.Developer && a.SidWhereDefined >= WhereDefined.LDAP && a.SidWhereDefined <= WhereDefined.Database))
                    select a;
            foreach (var auth in authz)
            {
                authorizationType = SqlAzManItem.mergeAuthorizations(authorizationType, auth.AuthorizationType);
                //Authorization Attributes
                if (retrieveAttributes)
                {
                    foreach (IAzManAttribute<IAzManAuthorization> authorizationAttribute in auth.Attributes.Values)
                    {
                        attributes.Add(new KeyValuePair<string, string>(authorizationAttribute.Key, authorizationAttribute.Value));
                    }
                }
            }
            #endregion CHECK ACCESS FOR USER GROUPS AUTHORIZATIONS
            #region CHECK ACCESS FOR STORE/APPLICATION GROUPS AUTHORIZATIONS
            bool isMember = true;
            authz = from a in item.Authorizations
                    where String.Compare(a.Item.Name, itemName, true) == 0 &&
                    (a.SidWhereDefined == WhereDefined.Store || a.SidWhereDefined == WhereDefined.Application) &&
                    (a.ValidFrom == null && a.ValidTo == null ||
                    validFor >= a.ValidFrom.Value && a.ValidTo == null ||
                    validFor <= a.ValidTo.Value && a.ValidFrom == null ||
                    validFor >= a.ValidFrom && validFor <= a.ValidTo.Value) &&
                    a.AuthorizationType != AuthorizationType.Neutral
                    select a;

            foreach (var auth in authz)
            {
                isMember = true;
                //store group members
                if (auth.SidWhereDefined == WhereDefined.Store)
                {
                    //check if user is a non-member
                    //non members
                    var nonMembers = this.getStoreGroupSidMembers(store, false, auth.SID);
                    if (nonMembers.Count(m => m.StringValue == userSSid) > 0
                        ||
                        (from m in nonMembers
                         from g in groupsSSid
                         where m.StringValue == g
                         select g).Count() > 0)
                    {
                        isMember = false;
                    }
                    if (isMember == true)
                    {
                        //members
                        var members = this.getStoreGroupSidMembers(store, true, auth.SID);
                        if (members.Count(m => m.StringValue == userSSid) > 0
                            ||
                            (from m in members
                             from g in groupsSSid
                             where m.StringValue == g
                             select g).Count() > 0)
                        {
                            isMember = true;
                        }
                        else
                        {
                            isMember = false;
                        }
                    }
                    //if a member ... get authorization
                    if (isMember == true)
                    {
                        authorizationType = SqlAzManItem.mergeAuthorizations(authorizationType, auth.AuthorizationType);
                        if (retrieveAttributes)
                        {
                            foreach (var attr in auth.Attributes.Values)
                            {
                                attributes.Add(new KeyValuePair<string, string>(attr.Key, attr.Value));
                            }
                        }
                    }
                }
                else if (auth.SidWhereDefined == WhereDefined.Application)
                {
                    //application group members
                    var nonMembers = this.getApplicationGroupSidMembers(auth.Item.Application, false, auth.SID);
                    if (nonMembers.Count(m => m.StringValue == userSSid) > 0
                        ||
                        (from m in nonMembers
                         from g in groupsSSid
                         where m.StringValue == g
                         select g).Count() > 0)
                    {
                        isMember = false;
                    }
                    if (isMember == true)
                    {
                        //members
                        var members = this.getApplicationGroupSidMembers(auth.Item.Application, true, auth.SID);
                        if (members.Count(m => m.StringValue == userSSid) > 0
                            ||
                            (from m in members
                             from g in groupsSSid
                             where m.StringValue == g
                             select g).Count() > 0)
                        {
                            isMember = true;
                        }
                        else
                        {
                            isMember = false;
                        }
                    }
                    //if a member ... get authorization
                    if (isMember == true)
                    {
                        authorizationType = SqlAzManItem.mergeAuthorizations(authorizationType, auth.AuthorizationType);
                        //Authorization Attributes
                        if (retrieveAttributes)
                        {
                            foreach (var attr in auth.Attributes.Values)
                            {
                                attributes.Add(new KeyValuePair<string, string>(attr.Key, attr.Value));
                            }
                        }
                    }
                }
            }
            #endregion CHECK ACCESS FOR STORE/APPLICATION GROUPS AUTHORIZATIONS

            return authorizationType;
        }
        #endregion Public Methods
    }
}
