using System;
using System.Data.SqlClient;
using System.Security.Principal;
using System.DirectoryServices;
using System.Xml;
using System.Data.SqlTypes;
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.LINQ;
using System.Linq;
using NetSqlAzMan.ENS;
using NetSqlAzMan.DirectoryServices;
using System.Runtime.Serialization;

namespace NetSqlAzMan
{
    /// <summary>
    /// SqlAzMan Store Group class.
    /// </summary>
    [Serializable()]
    [DataContract(Namespace = "http://NetSqlAzMan/ServiceModel", IsReference = true)]
    public sealed partial class SqlAzManStoreGroup : IAzManStoreGroup
    {
        #region Fields
        [NonSerialized()]
        private NetSqlAzManStorageDataContext db;
        private IAzManStore store;
        private int storeGroupId;
        private IAzManSid sid;
        private string name;
        private string description;
        private string lDapQuery;
        private GroupType groupType;
        [NonSerialized()]
        private SqlAzManENS ens;
        internal Dictionary<IAzManSid, IAzManStoreGroupMember> members;
        #endregion Fields
        #region Events
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
        #endregion Events
        #region Private Event Raisers
        private void raiseStoreGroupDeleted(IAzManStore ownerStore, string storeGroupName)
        {
            if (this.StoreGroupDeleted != null)
                this.StoreGroupDeleted(ownerStore, storeGroupName);
        }
        private void raiseStoreGroupLDAPQueryUpdated(IAzManStoreGroup storeGroup, string oldLDapQuery)
        {
            if (this.StoreGroupLDAPQueryUpdated != null)
                this.StoreGroupLDAPQueryUpdated(storeGroup, oldLDapQuery);
        }
        private void raiseStoreGroupUpdated(IAzManStoreGroup storeGroup, IAzManSid oldSid, string oldDescription, GroupType oldGroupType)
        {
            if (this.StoreGroupUpdated != null)
                this.StoreGroupUpdated(storeGroup, oldSid, oldDescription, oldGroupType);
        }
        private void raiseStoreGroupRenamed(IAzManStoreGroup storeGroup, string oldName)
        {
            if (this.StoreGroupRenamed != null)
                this.StoreGroupRenamed(storeGroup, oldName);
        }
        private void raiseStoreGroupMemberCreated(IAzManStoreGroup storeGroup, IAzManStoreGroupMember memberCreated)
        {
            if (this.StoreGroupMemberCreated != null)
                this.StoreGroupMemberCreated(storeGroup, memberCreated);
        }
        #endregion Private Event Raisers
        #region Constructors
        internal SqlAzManStoreGroup(NetSqlAzManStorageDataContext db, IAzManStore store, int storeGroupId, IAzManSid sid, string name, string description, string lDapQuery, GroupType groupType, SqlAzManENS ens)
        {
            this.db = db;
            this.store = store;
            this.storeGroupId = storeGroupId;
            this.sid = sid;
            this.name = name;
            this.description = description;
            this.lDapQuery = String.IsNullOrEmpty(lDapQuery) ? String.Empty : lDapQuery;
            this.groupType = groupType;
            this.ens = ens;
        }
        #endregion Constructors
        #region IAzManStoreGroup Members
        /// <summary>
        /// Gets the members.
        /// </summary>
        /// <value>The members.</value>
        public Dictionary<IAzManSid, IAzManStoreGroupMember> Members
        {
            get
            {
                if (this.members == null)
                {
                    this.members = new Dictionary<IAzManSid, IAzManStoreGroupMember>();
                    foreach (IAzManStoreGroupMember m in this.GetStoreGroupAllMembers())
                    {
                        this.members.Add(m.SID, m);
                    }
                }
                return this.members;
            }
        }
        /// <summary>
        /// Gets the store group id.
        /// </summary>
        /// <value>The store group id.</value>
        public int StoreGroupId
        {
            get
            {
                return this.storeGroupId;
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
        /// Gets the object owner.
        /// </summary>
        /// <value>The object owner.</value>
        public IAzManSid SID
        {
            get
            {
                return this.sid;
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
        /// Gets the LDAP query.
        /// </summary>
        /// <value>The LDAP query.</value>
        public string LDAPQuery
        {
            get
            {
                return this.lDapQuery;
            }
        }

        /// <summary>
        /// Gets the type of the group.
        /// </summary>
        /// <value>The type of the group.</value>
        public GroupType GroupType
        {
            get
            {
                return this.groupType;
            }
        }

        /// <summary>
        /// Updates the specified object owner.
        /// </summary>
        /// <param name="sid">The object owner.</param>
        /// <param name="description">The description.</param>
        /// <param name="groupType">Type of the group.</param>
        public void Update(IAzManSid sid, string description, GroupType groupType)
        {
            if (this.sid.StringValue != sid.StringValue || this.description != description || this.groupType != groupType)
            {
                IAzManSid oldSid = new SqlAzManSID(this.sid.StringValue);
                string oldDescription = this.description;
                GroupType oldGroupType = this.groupType;
                this.db.StoreGroupUpdate(this.Store.StoreId, sid.BinaryValue, this.name, description, this.LDAPQuery, (byte)(groupType), this.storeGroupId);
                this.sid = sid;
                this.description = description;
                this.groupType = groupType;
                this.raiseStoreGroupUpdated(this, oldSid, oldDescription, oldGroupType);
            }
        }

        /// <summary>
        /// Updates the L dap query.
        /// </summary>
        /// <param name="newLdapQuery">The new ldap query.</param>
        public void UpdateLDapQuery(string newLdapQuery)
        {
            if (this.lDapQuery != newLdapQuery)
            {
                if (DirectoryServicesUtils.TestLDAPQuery(newLdapQuery))
                {
                    string oldLDapQuery = this.lDapQuery;
                    this.db.StoreGroupUpdate(this.store.StoreId, this.SID.BinaryValue, this.name, this.description, newLdapQuery, (byte)this.groupType, this.storeGroupId);
                    this.lDapQuery = newLdapQuery;
                    this.raiseStoreGroupLDAPQueryUpdated(this, oldLDapQuery);
                }
                else
                {
                    throw new ArgumentException("LDAP Query syntax error or unavailable Domain.", "newLdapQuery");
                }
            }
        }

        /// <summary>
        /// Renames the specified new name.
        /// </summary>
        /// <param name="newName">The new name.</param>
        public void Rename(string newName)
        {
            try
            {
                if (this.name != newName)
                {
                    string oldName = this.name;
                    this.db.StoreGroupUpdate(this.store.StoreId, this.SID.BinaryValue, newName, this.description, this.LDAPQuery, (byte)this.groupType, this.storeGroupId);
                    this.name = newName;
                    this.raiseStoreGroupRenamed(this, oldName);
                }
            }
            catch (System.Data.SqlClient.SqlException sqlex)
            {
                if (sqlex.Number == 2601) //Index Duplicate Error
                    throw SqlAzManException.StoreGroupDuplicateException(newName, this.store, sqlex);
                else
                    throw SqlAzManException.GenericException(sqlex);
            }
        }

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        public void Delete()
        {
            this.db.StoreGroupDelete(this.storeGroupId, this.store.StoreId);
            this.raiseStoreGroupDeleted(this.store, this.name);
        }

        private bool detectLoop(IAzManStoreGroup storeGroupToAdd)
        {
            if (storeGroupToAdd.GroupType == GroupType.LDapQuery)
                return false;
            bool loopDetected = false;
            IAzManStoreGroupMember[] membersOfStoreGroupToAdd = storeGroupToAdd.GetStoreGroupAllMembers();
            foreach (IAzManStoreGroupMember member in membersOfStoreGroupToAdd)
            {
                if (member.WhereDefined == WhereDefined.Store)
                {
                    IAzManStoreGroup storeGroupMember = this.store.GetStoreGroup(member.SID);
                    if (storeGroupMember.SID.StringValue == this.sid.StringValue)
                    {
                        return true;
                    }
                    else
                    {
                        if (this.detectLoop(storeGroupMember))
                        {
                            loopDetected = true;
                        }
                    }
                }
            }
            return loopDetected;
        }

        /// <summary>
        /// Creates the store group member.
        /// </summary>
        /// <param name="sid">The object owner.</param>
        /// <param name="whereDefined">Where member is defined.</param>
        /// <param name="isMember">if set to <c>true</c> [is member].</param>
        /// <returns></returns>
        public IAzManStoreGroupMember CreateStoreGroupMember(IAzManSid sid, WhereDefined whereDefined, bool isMember)
        {
            if (this.groupType != GroupType.Basic)
                throw new InvalidOperationException("Method not supported for LDAP Groups");
            
            if (this.store.Storage.Mode == NetSqlAzManMode.Administrator && whereDefined == WhereDefined.Local)
            {
                throw new SqlAzManException("Cannot create Store Group members defined on local in Administrator Mode");
            }
            //Loop detection
            if (whereDefined == WhereDefined.Store)
            {
                IAzManStoreGroup storeGroupToAdd = this.store.GetStoreGroup(sid);
                if (this.detectLoop(storeGroupToAdd))
                    throw new SqlAzManException(String.Format("Cannot add '{0}'. A loop has been detected.", storeGroupToAdd.Name));
            }
            int retV = this.db.StoreGroupMemberInsert(this.store.StoreId, this.storeGroupId, sid.BinaryValue, (byte)whereDefined, isMember);
            IAzManStoreGroupMember result = new SqlAzManStoreGroupMember(this.db, this, retV, sid, whereDefined, isMember, this.ens);
            this.raiseStoreGroupMemberCreated(this, result);
            if (this.ens != null) this.ens.AddPublisher(result);
            return result;
        }
        /// <summary>
        /// Gets the store group members.
        /// </summary>
        /// <returns></returns>
        public IAzManStoreGroupMember[] GetStoreGroupAllMembers()
        {
            if (this.groupType != GroupType.Basic)
                throw new InvalidOperationException("Method not supported for LDAP Groups");
            var sgm = from f in this.db.StoreGroupMembers()
                      where 
                      (this.store.Storage.Mode == NetSqlAzManMode.Administrator && f.WhereDefined != (byte)WhereDefined.Local
                      ||
                      this.store.Storage.Mode == NetSqlAzManMode.Developer) &&
                      f.StoreGroupId == this.storeGroupId
                      select f;
            int index = 0;
            IAzManStoreGroupMember[] storeGroupAllMembers = new SqlAzManStoreGroupMember[sgm.Count()];
            foreach (var row in sgm)
            {
                storeGroupAllMembers[index] = new SqlAzManStoreGroupMember(this.db, this, row.StoreGroupMemberId.Value, new SqlAzManSID(row.ObjectSid.ToArray(), row.WhereDefined == (byte)(WhereDefined.Database)), (WhereDefined)row.WhereDefined, row.IsMember.Value, this.ens);
                if (this.ens != null) this.ens.AddPublisher(storeGroupAllMembers[index]);
                index++;
            }
            return storeGroupAllMembers;
        }

        /// <summary>
        /// Gets the store group members.
        /// </summary>
        /// <returns></returns>
        public IAzManStoreGroupMember[] GetStoreGroupMembers()
        {
            if (this.groupType != GroupType.Basic)
                throw new InvalidOperationException("Method not supported for LDAP Groups");
            var sgm = from f in this.db.StoreGroupMembers()
                      where 
                      (
                      this.store.Storage.Mode == NetSqlAzManMode.Administrator && f.WhereDefined != (byte)WhereDefined.Local
                      ||
                      this.store.Storage.Mode == NetSqlAzManMode.Developer) &&
                      f.StoreGroupId == this.storeGroupId && f.IsMember == true
                      select f;
            int index = 0;
            IAzManStoreGroupMember[] storeGroupMembers = new SqlAzManStoreGroupMember[sgm.Count()];
            foreach (var row in sgm)
            {
                storeGroupMembers[index] = new SqlAzManStoreGroupMember(this.db, this, row.StoreGroupMemberId.Value, new SqlAzManSID(row.ObjectSid.ToArray(), row.WhereDefined == (byte)(WhereDefined.Database)), (WhereDefined)row.WhereDefined, row.IsMember.Value, this.ens);
                if (this.ens != null) this.ens.AddPublisher(storeGroupMembers[index]);
                index++;
            }
            return storeGroupMembers;
        }

        /// <summary>
        /// Gets the store group member.
        /// </summary>
        /// <param name="sid">The object owner.</param>
        /// <returns></returns>
        public IAzManStoreGroupMember GetStoreGroupMember(IAzManSid sid)
        {
            if (this.groupType != GroupType.Basic)
                throw new InvalidOperationException("Method not supported for LDAP Groups");
            StoreGroupMembersResult sgm;
            if ((sgm = (from t in this.db.StoreGroupMembers() where t.StoreGroupId == this.storeGroupId && t.ObjectSid == sid.BinaryValue select t).FirstOrDefault())!=null)
            {
                if (this.store.Storage.Mode == NetSqlAzManMode.Administrator && sgm.WhereDefined == (byte)WhereDefined.Local)
                {
                    return null;
                }
                else
                {
                    IAzManStoreGroupMember result = new SqlAzManStoreGroupMember(this.db, this, sgm.StoreGroupMemberId.Value, new SqlAzManSID(sgm.ObjectSid.ToArray(), sgm.WhereDefined == (byte)(WhereDefined.Database)), (WhereDefined)(sgm.WhereDefined), sgm.IsMember.Value, this.ens);
                    if (this.ens != null) this.ens.AddPublisher(result);
                    return result;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the store group non members.
        /// </summary>
        /// <returns></returns>
        public IAzManStoreGroupMember[] GetStoreGroupNonMembers()
        {
            if (this.groupType != GroupType.Basic)
                throw new InvalidOperationException("Method not supported for LDAP Groups");
            var sgnm = from f in this.db.StoreGroupMembers()
                       where 
                       (
                       this.store.Storage.Mode == NetSqlAzManMode.Administrator && f.WhereDefined != (byte)WhereDefined.Local
                       ||
                       this.store.Storage.Mode == NetSqlAzManMode.Developer)
                       &&
                       f.StoreGroupId == this.storeGroupId && f.IsMember == false
                       select f;
            int index = 0;
            IAzManStoreGroupMember[] storeGroupNonMembers = new SqlAzManStoreGroupMember[sgnm.Count()];
            foreach (var row in sgnm)
            {
                storeGroupNonMembers[index] = new SqlAzManStoreGroupMember(this.db, this, row.StoreGroupMemberId.Value, new SqlAzManSID(row.ObjectSid.ToArray(), row.WhereDefined == (byte)(WhereDefined.Database)), (WhereDefined)row.WhereDefined, row.IsMember.Value, this.ens);
                if (this.ens != null) this.ens.AddPublisher(storeGroupNonMembers[index]);
                index++;
            }
            return storeGroupNonMembers;
        }
        /// <summary>
        /// Determines whether [is in group] [the specified windows identity].
        /// </summary>
        /// <param name="windowsIdentity">The windows identity.</param>
        /// <returns>
        /// 	<c>true</c> if [is in group] [the specified windows identity]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsInGroup(WindowsIdentity windowsIdentity)
        {
            if (windowsIdentity == null)
                throw new ArgumentNullException("windowsIdentity");
            List<byte> token = new List<byte>();
            int userGroupsCount = windowsIdentity.Groups.Count;
            if (userGroupsCount > 0)
            {
                token.AddRange(SqlAzManItem.getSqlBinarySid(windowsIdentity.User));
                foreach (SecurityIdentifier userGroupSid in windowsIdentity.Groups)
                {
                    token.AddRange(SqlAzManItem.getSqlBinarySid(userGroupSid));
                }
            }
            else
            {
                byte[] bSid = new byte[windowsIdentity.User.BinaryLength];
                windowsIdentity.User.GetBinaryForm(bSid, 0);
                token.AddRange(bSid);
            }
            return this.isAMemberOfGroup(false, this.sid.BinaryValue, this.Store.Storage.Mode == NetSqlAzManMode.Developer, DirectoryServicesUtils.rootDsePath, token.ToArray(), userGroupsCount);
        }
        /// <summary>
        /// Determines whether [is in group] [the specified windows identity].
        /// </summary>
        /// <param name="dbUser">The db user.</param>
        /// <returns>
        /// 	<c>true</c> if [is in group] [the specified windows identity]; otherwise, <c>false</c>.
        /// </returns>
        public bool IsInGroup(IAzManDBUser dbUser)
        {
            if (dbUser == null)
                throw new ArgumentNullException("dbUser");
            return this.isAMemberOfGroup(false, this.sid.BinaryValue, this.Store.Storage.Mode == NetSqlAzManMode.Developer, DirectoryServicesUtils.rootDsePath, dbUser.CustomSid.BinaryValue, 0);
        }
        /// <summary>
        /// Determines whether [is A member of group] [the specified group type].
        /// </summary>
        /// <param name="groupType">if set to <c>true</c> [group type].</param>
        /// <param name="GroupSid">The group sid.</param>
        /// <param name="netSqlAzManMode">if set to <c>true</c> [net SQL az man mode].</param>
        /// <param name="rootDsePath">The root dse path.</param>
        /// <param name="token">The token.</param>
        /// <param name="userGroupsCount">The user groups count.</param>
        /// <returns>
        /// 	<c>true</c> if [is A member of group] [the specified group type]; otherwise, <c>false</c>.
        /// </returns>
        internal bool isAMemberOfGroup(bool groupType, byte[] GroupSid, bool netSqlAzManMode, string rootDsePath, byte[] token, int userGroupsCount)
        {
            SqlConnection conn = new SqlConnection(this.store.Storage.ConnectionString);
            conn.Open();
            bool result = false;
            try
            {
                System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand("dbo.IsAMemberOfGroup", conn);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@GROUPTYPE", groupType);
                cmd.Parameters.AddWithValue("@GROUPOBJECTSID", GroupSid);
                cmd.Parameters.AddWithValue("@NETSQLAZMANMODE", netSqlAzManMode);
                cmd.Parameters.AddWithValue("@LDAPPATH", rootDsePath);
                cmd.Parameters.AddWithValue("@TOKEN", token);
                cmd.Parameters.AddWithValue("@USERGROUPSCOUNT", userGroupsCount);
                result = (bool)cmd.ExecuteScalar();
            }
            finally
            {
                conn.Close();
            }
            return result;
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
            xmlWriter.WriteStartElement("StoreGroup");
            xmlWriter.WriteAttributeString("Name", this.name);
            xmlWriter.WriteAttributeString("Description", this.description);
            xmlWriter.WriteAttributeString("Sid", this.sid.StringValue);
            xmlWriter.WriteAttributeString("LDAPQuery", this.lDapQuery);
            xmlWriter.WriteAttributeString("GroupType", this.groupType.ToString());
            if (this.groupType == GroupType.Basic)
            {
                xmlWriter.WriteStartElement("StoreGroupMembers");
                foreach (IAzManStoreGroupMember member in this.GetStoreGroupAllMembers())
                {
                    if (
                        includeDBUsers && member.WhereDefined == WhereDefined.Database
                        ||
                        includeWindowsUsersAndGroups && member.WhereDefined == WhereDefined.LDAP
                        ||
                        includeWindowsUsersAndGroups && member.WhereDefined == WhereDefined.Local && this.store.Storage.Mode == NetSqlAzManMode.Developer
                        ||
                        member.WhereDefined == WhereDefined.Store
                        )
                    {
                        ((IAzManExport)member).Export(xmlWriter, includeWindowsUsersAndGroups, includeDBUsers, includeAuthorizations, ownerOfExport);
                    }
                }
                xmlWriter.WriteEndElement();
            }
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
            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                if (node.Name == "StoreGroupMembers")
                {
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        if (childNode.Name == "StoreGroupMember")
                        { 
                            WhereDefined whereDefined;
                            switch (childNode.Attributes["WhereDefined"].Value)
                            {
                                case "Application": whereDefined = WhereDefined.Application;break;
                                case "LDAP": whereDefined = WhereDefined.LDAP;break;
                                case "Local": whereDefined = WhereDefined.Local;break;
                                case "Store": whereDefined = WhereDefined.Store;break;
                                case "Database": whereDefined = WhereDefined.Database; break;
                                default:
                                    throw new System.Xml.Schema.XmlSchemaValidationException("WhereDefined attribute valid");
                            }
                            bool isMember = false;
                            if (childNode.Attributes["IsMember"].Value=="True")
                            {
                                isMember = true;
                            }
                            if (includeWindowsUsersAndGroups
                                ||
                                !includeWindowsUsersAndGroups && whereDefined != WhereDefined.LDAP
                                &&
                                whereDefined != WhereDefined.Local
                                &&
                                this.Store.Storage.Mode != NetSqlAzManMode.Developer
                                ||
                                includeDBUsers && whereDefined == WhereDefined.Database
                                || 
                                whereDefined==WhereDefined.Store
                                || 
                                whereDefined == WhereDefined.Application)
                            {
                                IAzManSid sid = new SqlAzManSID(childNode.Attributes["Sid"].Value, whereDefined == WhereDefined.Database);
                                if (this.Members.Where(m => m.Key.StringValue == sid.StringValue).Count() == 0)
                                    this.CreateStoreGroupMember(sid, whereDefined, isMember);
                            }
                        }
                    }
                }
                else if (node.Name == "StoreGroupMember")
                {
                    WhereDefined whereDefined;
                    switch (node.Attributes["WhereDefined"].Value)
                    {
                        case "Application": whereDefined = WhereDefined.Application; break;
                        case "LDAP": whereDefined = WhereDefined.LDAP; break;
                        case "Local": whereDefined = WhereDefined.Local; break;
                        case "Store": whereDefined = WhereDefined.Store; break;
                        default:
                            throw new System.Xml.Schema.XmlSchemaValidationException("WhereDefined attribute not valid.");
                    }
                    bool isMember = false;
                    if (node.Attributes["IsMember"].Value == "True")
                    {
                        isMember = true;
                    }
                    if (includeWindowsUsersAndGroups
                                ||
                                !includeWindowsUsersAndGroups && whereDefined != WhereDefined.LDAP
                                &&
                                whereDefined != WhereDefined.Local
                                &&
                                this.Store.Storage.Mode != NetSqlAzManMode.Developer
                                ||
                                whereDefined == WhereDefined.Store
                                ||
                                whereDefined == WhereDefined.Application)
                    {
                        IAzManSid sid = new SqlAzManSID(node.Attributes["Sid"].Value, whereDefined == WhereDefined.Database);
                        if (this.Members.Where(m => m.Key.StringValue == sid.StringValue).Count() == 0)
                        this.CreateStoreGroupMember(sid, whereDefined, isMember);
                    }
                }
            }
        }

        /// <summary>
        /// Executes the LDAP query.
        /// </summary>
        /// <returns></returns>
        public SearchResultCollection ExecuteLDAPQuery()
        {
            return this.ExecuteLDAPQuery(this.lDapQuery);
        }

        /// <summary>
        /// Executes the LDAP query.
        /// </summary>
        /// <param name="testLDapQuery">The test L dap query.</param>
        /// <returns></returns>
        public SearchResultCollection ExecuteLDAPQuery(string testLDapQuery)
        {
            if (this.groupType != GroupType.LDapQuery)
                throw new InvalidOperationException("Method not supported for Basic Groups");
            if (!String.IsNullOrEmpty(testLDapQuery.Trim()))
            {
                string rootdse = DirectoryServicesUtils.GetRootDSEPart(testLDapQuery);
                string ldapquery = DirectoryServicesUtils.GetLDAPQueryPart(testLDapQuery);
                string query = String.Empty;
                if (rootdse == null)
                    query = String.Format("(&(!(objectClass=computer))(&(|(objectClass=user)(objectClass=group)))({0}))", ldapquery);
                else
                    query = String.Format("[RootDSE:{0}](&(!(objectClass=computer))(&(|(objectClass=user)(objectClass=group)))({1}))", rootdse, ldapquery);
                return DirectoryServices.DirectoryServicesUtils.ExecuteLDAPQuery(query);
            }
            else
            {
                return null;
            }
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
            return String.Format("Store Group ID: {0}\r\nSID: {1}\r\nName: {2}\r\nDescription: {3}\r\nGroup Type: {4}\r\nLDAP Query: {5}",
                this.storeGroupId, this.sid, this.name, this.description, this.groupType, this.lDapQuery);
        }
        #endregion Object Members
    }
}
