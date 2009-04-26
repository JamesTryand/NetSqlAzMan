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
    /// SqlAzMan Application Group class.
    /// </summary>
    [Serializable()]
    [DataContract(Namespace = "http://NetSqlAzMan/ServiceModel", IsReference = true)]
    public sealed partial class SqlAzManApplicationGroup : IAzManApplicationGroup, IAzManExport
    {
        #region Fields
        [NonSerialized()]
        private NetSqlAzManStorageDataContext db;
        private IAzManApplication application;
        private int applicationGroupId;
        private IAzManSid sid;
        private string name;
        private string description;
        private string lDapQuery;
        private GroupType groupType;
        [NonSerialized()]
        private SqlAzManENS ens;
        internal Dictionary<IAzManSid, IAzManApplicationGroupMember> members;
        #endregion Fields
        #region Events
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
        #endregion Events
        #region Private Event Raisers
        private void raiseApplicationGroupDeleted(IAzManApplication ownerApplication, string applicationGroupName)
        {
            if (this.ApplicationGroupDeleted != null)
                this.ApplicationGroupDeleted(ownerApplication, applicationGroupName);
        }
        private void raiseApplicationGroupLDAPQueryUpdated(IAzManApplicationGroup applicationGroup, string oldLDapQuery)
        {
            if (this.ApplicationGroupLDAPQueryUpdated != null)
                this.ApplicationGroupLDAPQueryUpdated(applicationGroup, oldLDapQuery);
        }
        private void raiseApplicationGroupUpdated(IAzManApplicationGroup applicationGroup, IAzManSid oldSid, string oldDescription, GroupType oldGroupType)
        {
            if (this.ApplicationGroupUpdated != null)
                this.ApplicationGroupUpdated(applicationGroup, oldSid, oldDescription, oldGroupType);
        }
        private void raiseApplicationGroupRenamed(IAzManApplicationGroup applicationGroup, string oldName)
        {
            if (this.ApplicationGroupRenamed != null)
                this.ApplicationGroupRenamed(applicationGroup, oldName);
        }
        private void raiseApplicationGroupMemberCreated(IAzManApplicationGroup applicationGroup, IAzManApplicationGroupMember memberCreated)
        {
            if (this.ApplicationGroupMemberCreated != null)
                this.ApplicationGroupMemberCreated(applicationGroup, memberCreated);
        }
        #endregion Private Event Raisers
        #region Constructors
        internal SqlAzManApplicationGroup(NetSqlAzManStorageDataContext db, IAzManApplication application, int applicationGroupId, IAzManSid sid, string name, string description, string lDapQuery, GroupType groupType, SqlAzManENS ens)
        {
            this.db = db;
            this.application = application;
            this.applicationGroupId = applicationGroupId;
            this.sid = sid;
            this.name = name;
            this.description = description;
            this.lDapQuery = String.IsNullOrEmpty(lDapQuery) ? String.Empty : lDapQuery;
            this.groupType = groupType;
            this.ens = ens;
        }
        #endregion Constructors
        #region IAzManApplicationGroup Members
        /// <summary>
        /// Gets the members.
        /// </summary>
        /// <value>The members.</value>
        public Dictionary<IAzManSid, IAzManApplicationGroupMember> Members
        {
            get
            {
                if (this.members == null)
                {
                    this.members = new Dictionary<IAzManSid, IAzManApplicationGroupMember>();
                    foreach (IAzManApplicationGroupMember m in this.GetApplicationGroupAllMembers())
                    {
                        this.members.Add(m.SID, m);
                    }
                }
                return this.members;
            }
        }
        /// <summary>
        /// Gets the application group id.
        /// </summary>
        /// <value>The application group id.</value>
        public int ApplicationGroupId
        {
            get
            {
                return this.applicationGroupId;
            }
        }

        /// <summary>
        /// Gets the application.
        /// </summary>
        /// <value>The application.</value>
        public IAzManApplication Application
        {
            get
            {
                return this.application;
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
                this.db.ApplicationGroupUpdate(sid.BinaryValue, this.name, description, this.lDapQuery, (byte)(groupType), this.applicationGroupId, this.application.ApplicationId);
                IAzManSid oldSid = new SqlAzManSID(this.sid.StringValue);
                string oldDescription = this.description;
                GroupType oldGroupType = this.groupType;
                this.sid = sid;
                this.description = description;
                this.groupType = groupType;
                this.raiseApplicationGroupUpdated(this, oldSid, oldDescription, oldGroupType);
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
                    this.db.ApplicationGroupUpdate(this.sid.BinaryValue, this.name, this.description, newLdapQuery, (byte)this.groupType, this.applicationGroupId, this.application.ApplicationId);
                    string oldLDapQuery = this.lDapQuery;
                    this.lDapQuery = newLdapQuery;
                    this.raiseApplicationGroupLDAPQueryUpdated(this, oldLDapQuery);
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
            if (this.name != newName)
            {
                string oldName = this.name;
                try
                {
                    this.db.ApplicationGroupUpdate(this.sid.BinaryValue, newName, this.description, this.lDapQuery, (byte)this.groupType, this.applicationGroupId, this.application.ApplicationId);
                    this.name = newName;
                    this.raiseApplicationGroupRenamed(this, oldName);
                }
                catch (System.Data.SqlClient.SqlException sqlex)
                {
                    if (sqlex.Number == 2601) //Index Duplicate Error
                        throw SqlAzManException.ApplicationGroupDuplicateException(newName, this.application, sqlex);
                    else
                        throw SqlAzManException.GenericException(sqlex);
                }
            }
        }

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        public void Delete()
        {
            this.db.ApplicationGroupDelete(this.applicationGroupId, this.application.ApplicationId);
            this.raiseApplicationGroupDeleted(this.application, this.name);
        }
        private bool detectLoop(IAzManApplicationGroup applicationGroupToAdd)
        {
            if (applicationGroupToAdd.GroupType == GroupType.LDapQuery)
                return false;
            bool loopDetected = false;
            IAzManApplicationGroupMember[] membersOfApplicationGroupToAdd = applicationGroupToAdd.GetApplicationGroupAllMembers();
            foreach (IAzManApplicationGroupMember member in membersOfApplicationGroupToAdd)
            {
                if (member.WhereDefined == WhereDefined.Application)
                {
                    IAzManApplicationGroup applicationGroupMember = this.application.GetApplicationGroup(member.SID);
                    if (applicationGroupMember.SID.StringValue == this.sid.StringValue)
                    {
                        return true;
                    }
                    else
                    {
                        if (this.detectLoop(applicationGroupMember))
                        {
                            loopDetected = true;
                        }
                    }
                }
            }
            return loopDetected;
        }

        /// <summary>
        /// Creates the application group member.
        /// </summary>
        /// <param name="sid">The object owner.</param>
        /// <param name="whereDefined">The where defined.</param>
        /// <param name="isMember">if set to <c>true</c> [is member].</param>
        public IAzManApplicationGroupMember CreateApplicationGroupMember(IAzManSid sid, WhereDefined whereDefined, bool isMember)
        {
            if (this.groupType != GroupType.Basic)
                throw new InvalidOperationException("Method not supported for LDAP Groups");
            
            if (this.application.Store.Storage.Mode == NetSqlAzManMode.Administrator && whereDefined == WhereDefined.Local)
            {
                throw new SqlAzManException("Cannot create Application Group members defined on local in Administrator Mode");
            }
            //Loop detection
            if (whereDefined == WhereDefined.Application)
            {
                IAzManApplicationGroup applicationGroupToAdd = this.application.GetApplicationGroup(sid);
                if (this.detectLoop(applicationGroupToAdd))
                    throw new SqlAzManException(String.Format("Cannot add '{0}'. A loop has been detected.", applicationGroupToAdd.Name));
            }
            int retV = this.db.ApplicationGroupMemberInsert(this.applicationGroupId, sid.BinaryValue, (byte)whereDefined, isMember, this.application.ApplicationId);
            IAzManApplicationGroupMember result = new SqlAzManApplicationGroupMember(this.db, this, retV, sid, whereDefined, isMember, this.ens);
            this.raiseApplicationGroupMemberCreated(this, result);
            this.ens.AddPublisher(result);
            return result;
        }
        /// <summary>
        /// Gets the store group members.
        /// </summary>
        /// <returns></returns>
        public IAzManApplicationGroupMember[] GetApplicationGroupMembers()
        {
            if (this.groupType != GroupType.Basic)
                throw new InvalidOperationException("Method not supported for LDAP Groups");
            var agm = from f in this.db.ApplicationGroupMembers()
                      where
                      (
                      this.application.Store.Storage.Mode == NetSqlAzManMode.Administrator && f.WhereDefined != (byte)WhereDefined.Local
                      ||
                      this.application.Store.Storage.Mode == NetSqlAzManMode.Developer)
                      &&
                      f.ApplicationGroupId == this.applicationGroupId && f.IsMember == true
                      select f;
            
            int index = 0;
            IAzManApplicationGroupMember[] applicationGroupMembers = new SqlAzManApplicationGroupMember[agm.Count()];
            foreach (var row in agm)
            {
                applicationGroupMembers[index] = new SqlAzManApplicationGroupMember(this.db, this, row.ApplicationGroupMemberId.Value, new SqlAzManSID(row.ObjectSid.ToArray(), row.WhereDefined == (byte)(WhereDefined.Database)), (WhereDefined)row.WhereDefined, row.IsMember.Value, this.ens);
                this.ens.AddPublisher(applicationGroupMembers[index]);
                index++;
            }
            return applicationGroupMembers;
        }

        /// <summary>
        /// Gets the application group member.
        /// </summary>
        /// <param name="sid">The object owner.</param>
        /// <returns></returns>
        public IAzManApplicationGroupMember GetApplicationGroupMember(IAzManSid sid)
        {
            if (this.groupType != GroupType.Basic)
                throw new InvalidOperationException("Method not supported for LDAP Groups");
            ApplicationGroupMembersResult agm;
            if ((agm = (from t in this.db.ApplicationGroupMembers() where t.ApplicationGroupId == this.applicationGroupId && t.ObjectSid == sid.BinaryValue select t).FirstOrDefault())!=null)
            {
                if (this.application.Store.Storage.Mode == NetSqlAzManMode.Administrator && agm.WhereDefined == (byte)WhereDefined.Local)
                {
                    return null;
                }
                else
                {
                    IAzManApplicationGroupMember result = new SqlAzManApplicationGroupMember(this.db, this, agm.ApplicationGroupMemberId.Value, new SqlAzManSID(agm.ObjectSid.ToArray(), agm.WhereDefined == (byte)(WhereDefined.Database)), (WhereDefined)(agm.WhereDefined), agm.IsMember.Value, this.ens);
                    this.ens.AddPublisher(result);
                    return result;
                }
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the store group members.
        /// </summary>
        /// <returns></returns>
        public IAzManApplicationGroupMember[] GetApplicationGroupNonMembers()
        {
            if (this.groupType != GroupType.Basic)
                throw new InvalidOperationException("Method not supported for LDAP Groups");
            var agnm = from f in this.db.ApplicationGroupMembers()
                       where
                       (this.application.Store.Storage.Mode == NetSqlAzManMode.Administrator && f.WhereDefined != (byte)WhereDefined.Local
                       ||
                       this.application.Store.Storage.Mode == NetSqlAzManMode.Developer)
                       &&
                       f.ApplicationGroupId == this.applicationGroupId && f.IsMember == false
                       select f;
            int index = 0;
            IAzManApplicationGroupMember[] applicationGroupNonMembers = new SqlAzManApplicationGroupMember[agnm.Count()];
            foreach (var row in agnm)
            {
                applicationGroupNonMembers[index] = new SqlAzManApplicationGroupMember(this.db, this, row.ApplicationGroupMemberId.Value, new SqlAzManSID(row.ObjectSid.ToArray(), row.WhereDefined == (byte)(WhereDefined.Database)), (WhereDefined)row.WhereDefined, row.IsMember.Value, this.ens);
                this.ens.AddPublisher(applicationGroupNonMembers[index]);
                index++;
            }
            return applicationGroupNonMembers;
        }

        /// <summary>
        /// Gets the application group all members.
        /// </summary>
        /// <returns></returns>
        public IAzManApplicationGroupMember[] GetApplicationGroupAllMembers()
        {
            if (this.groupType != GroupType.Basic)
                throw new InvalidOperationException("Method not supported for LDAP Groups");
            var agam = from f in this.db.ApplicationGroupMembers()
                       where 
                       (this.application.Store.Storage.Mode == NetSqlAzManMode.Administrator && f.WhereDefined != (byte)WhereDefined.Local
                       ||
                       this.application.Store.Storage.Mode != NetSqlAzManMode.Administrator)
                       &&
                       f.ApplicationGroupId == this.applicationGroupId
                       select f;
            int index = 0;
            IAzManApplicationGroupMember[] applicationGroupAllMembers = new SqlAzManApplicationGroupMember[agam.Count()];
            foreach (var row in agam)
            {
                applicationGroupAllMembers[index] = new SqlAzManApplicationGroupMember(this.db, this, row.ApplicationGroupMemberId.Value, new SqlAzManSID(row.ObjectSid.ToArray(), row.WhereDefined == (byte)(WhereDefined.Database)), (WhereDefined)row.WhereDefined, row.IsMember.Value, this.ens);
                this.ens.AddPublisher(applicationGroupAllMembers[index]);
                index++;
            }
            return applicationGroupAllMembers;
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
            return this.isAMemberOfGroup(true, this.sid.BinaryValue, this.application.Store.Storage.Mode == NetSqlAzManMode.Developer, DirectoryServicesUtils.rootDsePath, token.ToArray(), userGroupsCount);
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
            return this.isAMemberOfGroup(true, this.sid.BinaryValue, this.application.Store.Storage.Mode == NetSqlAzManMode.Developer, DirectoryServicesUtils.rootDsePath, dbUser.CustomSid.BinaryValue, 0);
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
            SqlConnection conn = new SqlConnection(this.db.Connection.ConnectionString);
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
            xmlWriter.WriteStartElement("ApplicationGroup");
            xmlWriter.WriteAttributeString("Name", this.name);
            xmlWriter.WriteAttributeString("Description", this.description);
            xmlWriter.WriteAttributeString("Sid", this.sid.StringValue);
            xmlWriter.WriteAttributeString("LDAPQuery", this.lDapQuery);
            xmlWriter.WriteAttributeString("GroupType", this.groupType.ToString());
            if (this.groupType == GroupType.Basic)
            {
                xmlWriter.WriteStartElement("ApplicationGroupMembers");
                foreach (IAzManApplicationGroupMember member in this.GetApplicationGroupAllMembers())
                {
                    if (
                        includeWindowsUsersAndGroups && member.WhereDefined == WhereDefined.LDAP
                        ||
                        includeWindowsUsersAndGroups && member.WhereDefined == WhereDefined.Local && this.application.Store.Storage.Mode == NetSqlAzManMode.Developer
                        ||
                        includeDBUsers && member.WhereDefined == WhereDefined.Database
                        ||
                        member.WhereDefined == WhereDefined.Application
                        ||
                        member.WhereDefined == WhereDefined.Store && (ownerOfExport as IAzManStorage != null || ownerOfExport as IAzManStore != null)
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
                if (node.Name == "ApplicationGroupMembers")
                {
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        if (childNode.Name == "ApplicationGroupMember")
                        {
                            WhereDefined whereDefined;
                            switch (childNode.Attributes["WhereDefined"].Value)
                            {
                                case "Application": whereDefined = WhereDefined.Application; break;
                                case "LDAP": whereDefined = WhereDefined.LDAP; break;
                                case "Local": whereDefined = WhereDefined.Local; break;
                                case "Store": whereDefined = WhereDefined.Store; break;
                                case "Database": whereDefined = WhereDefined.Database; break;
                                default:
                                    throw new System.Xml.Schema.XmlSchemaValidationException("WhereDefined attribute valid");
                            }
                            bool isMember = false;
                            if (childNode.Attributes["IsMember"].Value == "True")
                            {
                                isMember = true;
                            }
                            if (includeWindowsUsersAndGroups
                                ||
                                !includeWindowsUsersAndGroups && whereDefined != WhereDefined.LDAP
                                &&
                                whereDefined != WhereDefined.Local
                                &&
                                this.application.Store.Storage.Mode != NetSqlAzManMode.Developer
                                ||
                                includeDBUsers && whereDefined == WhereDefined.Database
                                ||
                                whereDefined == WhereDefined.Store
                                ||
                                whereDefined == WhereDefined.Application)
                            {
                                IAzManSid sid = new SqlAzManSID(childNode.Attributes["Sid"].Value, whereDefined == WhereDefined.Database);
                                if (this.Members.Where(m => m.Key.StringValue == sid.StringValue).Count() == 0)
                                this.CreateApplicationGroupMember(sid, whereDefined, isMember);
                            }
                        }
                    }
                }
                else if (node.Name == "ApplicationGroupMember")
                {
                    WhereDefined whereDefined;
                    switch (node.Attributes["WhereDefined"].Value)
                    {
                        case "Application": whereDefined = WhereDefined.Application; break;
                        case "LDAP": whereDefined = WhereDefined.LDAP; break;
                        case "Local": whereDefined = WhereDefined.Local; break;
                        case "Store": whereDefined = WhereDefined.Store; break;
                        case "Database": whereDefined = WhereDefined.Database; break;
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
                        this.application.Store.Storage.Mode != NetSqlAzManMode.Developer
                        ||
                        includeDBUsers && whereDefined == WhereDefined.Database
                        ||
                        whereDefined == WhereDefined.Store
                        ||
                        whereDefined == WhereDefined.Application)
                    {
                        IAzManSid sid = new SqlAzManSID(node.Attributes["Sid"].Value, whereDefined == WhereDefined.Database);
                        if (this.Members.Where(m => m.Key.StringValue == sid.StringValue).Count() == 0)
                            this.CreateApplicationGroupMember(sid, whereDefined, isMember);
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
                if (rootdse==null)
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
            return String.Format("Application Group ID: {0}\r\nSID: {1}\r\nName: {2}\r\nDescription: {3}\r\nGroup Type: {4}\r\nLDAP Query: {5}", 
                this.applicationGroupId, this.sid,this.name, this.description, this.groupType, this.lDapQuery);
        }
        #endregion Object Members
    }
}
