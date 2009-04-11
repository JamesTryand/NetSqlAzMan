using System;
using System.Xml;
using System.DirectoryServices;
using System.Data.SqlTypes;
using System.Text;
using System.Collections.Generic;
using System.Collections.Specialized;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.LINQ;
using System.Linq;
using NetSqlAzMan.ENS;
using System.Runtime.Serialization;

namespace NetSqlAzMan
{
    /// <summary>
    /// SqlAzMan Store Group Member class.
    /// </summary>
    [Serializable()]
    [DataContract(Namespace = "http://NetSqlAzMan/ServiceModel", IsReference = true)]
    public sealed partial class SqlAzManStoreGroupMember : IAzManStoreGroupMember, IAzManExport
    {
        #region Fields
        [NonSerialized()]
        private NetSqlAzManStorageDataContext db;
        private int storeGroupMemberId;
        private IAzManStoreGroup storeGroup;
        private IAzManSid sid;
        private WhereDefined whereDefined;
        private bool isMember;
        [NonSerialized()]
        private SqlAzManENS ens;
        #endregion Fields
        #region Events
        /// <summary>
        /// Occurs after a SqlStoreGroupMember object has been Deleted.
        /// </summary>
        public event StoreGroupMemberDeletedDelegate StoreGroupMemberDeleted;
        #endregion Events
        #region Private Event Raisers
        private void raiseStoreGroupMemberDeleted(IAzManStoreGroup ownerStoreGroup, IAzManSid sid)
        {
            if (this.StoreGroupMemberDeleted != null)
                this.StoreGroupMemberDeleted(ownerStoreGroup, sid);
        }
        #endregion Private Event Raisers
        #region Constructors
        internal SqlAzManStoreGroupMember(NetSqlAzManStorageDataContext db, IAzManStoreGroup storeGroup, int storeGroupMemberId, IAzManSid sid, WhereDefined whereDefined, bool isMember, SqlAzManENS ens)
        {
            this.db = db;
            this.storeGroup = storeGroup;
            this.storeGroupMemberId = storeGroupMemberId;
            this.sid = sid;
            this.whereDefined = whereDefined;
            this.isMember = isMember;
            this.ens = ens;
        }
        #endregion Constructors
        #region IAzManStoreGroupMember Members

        /// <summary>
        /// Gets the store group member id.
        /// </summary>
        /// <value>The store group member id.</value>
        public int StoreGroupMemberId
        {
            get
            {
                return this.storeGroupMemberId;
            }
        }

        /// <summary>
        /// Gets the store group.
        /// </summary>
        /// <value>The store group.</value>
        public IAzManStoreGroup StoreGroup
        {
            get
            {
                return this.storeGroup;
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
        /// Gets the member info.
        /// </summary>
        /// <param name="displayName">Name of the display.</param>
        /// <returns></returns>
        public MemberType GetMemberInfo(out string displayName)
        {
            try
            {
                switch (this.whereDefined)
                {
                    case WhereDefined.Store:
                        displayName = this.storeGroup.Store.GetStoreGroup(this.sid).Name;
                        return MemberType.StoreGroup;
                    case WhereDefined.LDAP:
                        bool isAnLdapGroup;
                        bool isLocal;
                        DirectoryServices.DirectoryServicesUtils.GetMemberInfo(this.sid.StringValue, out displayName, out isAnLdapGroup, out isLocal);
                        return isAnLdapGroup ? MemberType.WindowsNTGroup : MemberType.WindowsNTUser;
                    case WhereDefined.Local:
                        bool isALocalGroup;
                        bool isLocal2;
                        DirectoryServices.DirectoryServicesUtils.GetMemberInfo(this.sid.StringValue, out displayName, out isALocalGroup, out isLocal2);
                        return isALocalGroup ? MemberType.WindowsNTGroup : MemberType.WindowsNTUser;
                    case WhereDefined.Database:
                        displayName = this.storeGroup.Store.GetDBUser(this.sid).UserName;
                        return MemberType.DatabaseUser;
                }
                displayName = this.sid.StringValue;
                return MemberType.AnonymousSID;
            }
            catch
            {
                displayName = this.sid.StringValue;
                return MemberType.AnonymousSID;
            }
        }

        /// <summary>
        /// Gets where member is defined.
        /// </summary>
        /// <value>The where defined.</value>
        public WhereDefined WhereDefined
        {
            get
            {
                return this.whereDefined;
            }
        }
        /// <summary>
        /// Gets a value indicating whether this instance is member.
        /// </summary>
        /// <value><c>true</c> if this instance is member; otherwise, <c>false</c>.</value>
        public bool IsMember
        {
            get
            {
                return this.isMember;
            }
        }
        /// <summary>
        /// Deletes this instance.
        /// </summary>
        public void Delete()
        {
            this.db.StoreGroupMemberDelete(this.storeGroup.Store.StoreId, this.storeGroupMemberId);
            this.raiseStoreGroupMemberDeleted(this.storeGroup, this.sid);
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
            xmlWriter.WriteStartElement("StoreGroupMember");
            xmlWriter.WriteAttributeString("Sid", this.sid.StringValue);
            xmlWriter.WriteAttributeString("WhereDefined", this.whereDefined.ToString());
            xmlWriter.WriteAttributeString("IsMember", this.isMember.ToString());
            xmlWriter.WriteEndElement();
        }

        /// <summary>
        /// Imports the specified XML reader.
        /// </summary>
        /// <param name="xmlNode">The XML node.</param>
        /// <param name="includeWindowsUsersAndGroups">if set to <c>true</c> [include windows users and groups].</param>
        /// <param name="includeAuthorizations">if set to <c>true</c> [include authorizations].</param>
        /// <returns></returns>
        public void ImportChildren(XmlNode xmlNode, bool includeWindowsUsersAndGroups, bool includeAuthorizations)
        {
            throw new Exception("The method or operation is not implemented.");
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
            return String.Format("Store Group Member ID: {0}\r\nSID: {1}\r\nWhere Defined: {2}\r\nIs Member: {3}",
                this.storeGroupMemberId, this.sid, this.whereDefined, this.isMember);
        }
        #endregion Object Members
    }
}
