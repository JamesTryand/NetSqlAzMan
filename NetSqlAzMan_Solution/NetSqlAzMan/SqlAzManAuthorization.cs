using System;
using System.Security.Principal;
using System.Data;
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
    /// Represents an AzManAuthorization stored on Sql Server.
    /// </summary>
    [Serializable()]
    [DataContract(Namespace = "http://NetSqlAzMan/ServiceModel", IsReference = true)]
    public sealed partial class SqlAzManAuthorization : IAzManAuthorization
    {
        #region Fields
        [NonSerialized()]
        private NetSqlAzManStorageDataContext db;
        private int authorizationId;
        private IAzManItem item;
        private IAzManSid owner;
        private IAzManSid sid;
        private WhereDefined ownerSidWhereDefined;
        private WhereDefined sidWhereDefined;
        private AuthorizationType authorizationType;
        private DateTime? validFrom;
        private DateTime? validTo;
        [NonSerialized()]
        private SqlAzManENS ens;
        internal Dictionary<string, IAzManAttribute<IAzManAuthorization>> attributes;
        #endregion Fields
        #region Events
        /// <summary>
        /// Occurs after a SqlAzManAuthorization object has been Deleted.
        /// </summary>
        public event AuthorizationDeletedDelegate AuthorizationDeleted;
        /// <summary>
        /// Occurs after a SqlAzManAuthorization object has been Updated.
        /// </summary>
        public event AuthorizationUpdatedDelegate AuthorizationUpdated;
        /// <summary>
        /// Occurs after an Attribute object has been Created.
        /// </summary>
        public event AttributeCreatedDelegate<IAzManAuthorization> AuthorizationAttributeCreated;
        #endregion Events
        #region Private Event Raisers
        private void raiseAuthorizationDeleted(IAzManItem ownerItem, IAzManSid owner, IAzManSid sid)
        {
            if (this.AuthorizationDeleted != null)
                this.AuthorizationDeleted(ownerItem, owner, sid);
        }
        private void raiseAuthorizationUpdated(IAzManAuthorization authorization, IAzManSid oldOwner, WhereDefined oldOwnerSidWhereDefined, IAzManSid oldSid, WhereDefined oldSidWhereDefined, AuthorizationType oldAuthorizationType, DateTime? oldValidFrom, DateTime? oldValidTo)
        {
            if (this.AuthorizationUpdated != null)
                this.AuthorizationUpdated(authorization, oldOwner, oldOwnerSidWhereDefined, oldSid, oldSidWhereDefined, oldAuthorizationType, oldValidFrom, oldValidTo); 
        }
        private void raiseAuthorizationAttributeCreated(IAzManAuthorization owner, IAzManAttribute<IAzManAuthorization> attributeCreated)
        {
            if (this.AuthorizationAttributeCreated != null)
                this.AuthorizationAttributeCreated(owner, attributeCreated);
        }
        #endregion Private Event Raisers
        #region Constructors
        internal SqlAzManAuthorization(NetSqlAzManStorageDataContext db, IAzManItem item, int authorizationId, IAzManSid owner, WhereDefined ownerSidWhereDefined, IAzManSid sid, WhereDefined objectSidWhereDefined, AuthorizationType authorizationType, DateTime? validFrom, DateTime? validTo, SqlAzManENS ens)
        {
            this.db = db;
            this.authorizationId = authorizationId;
            this.item = item;
            this.owner = owner;
            this.ownerSidWhereDefined = ownerSidWhereDefined;
            this.sid = sid;
            this.sidWhereDefined = objectSidWhereDefined;
            this.authorizationType = authorizationType;
            this.validFrom = validFrom;
            this.validTo = validTo;
            this.ens = ens;
        }
        #endregion Constructors
        #region IAzManAuthorization Members
        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>The attributes.</value>
        public Dictionary<string, IAzManAttribute<IAzManAuthorization>> Attributes
        {
            get
            {
                if (this.attributes == null)
                {
                    this.attributes = new Dictionary<string, IAzManAttribute<IAzManAuthorization>>();
                    foreach (IAzManAttribute<IAzManAuthorization> a in this.GetAttributes())
                    {
                        this.attributes.Add(a.Key, a);
                    }
                }
                return this.attributes;
            }
        }
        /// <summary>
        /// Gets the authorization id.
        /// </summary>
        /// <value>The authorization id.</value>
        int IAzManAuthorization.AuthorizationId
        {
            get
            {
                return this.authorizationId;
            }
        }

        /// <summary>
        /// Gets the itemName.
        /// </summary>
        /// <value>The itemName.</value>
        public IAzManItem Item
        {
            get
            {
                return this.item;
            }
        }

        /// <summary>
        /// Gets the Owner owner.
        /// </summary>
        /// <value>The Owner owner.</value>
        public IAzManSid Owner
        {
            get
            {
                return this.owner;
            }
        }

        /// <summary>
        /// Gets the Member owner.
        /// </summary>
        /// <value>The Member owner.</value>
        public IAzManSid SID
        {
            get
            {
                return this.sid;
            }
        }

        /// <summary>
        /// Gets the object owner where defined.
        /// </summary>
        /// <value>The object owner where defined.</value>
        public WhereDefined SidWhereDefined
        {
            get
            {
                return this.sidWhereDefined;
            }
        }

        /// <summary>
        /// Gets the object owner Sid where defined.
        /// </summary>
        /// <value>The object owner where defined.</value>
        public WhereDefined OwnerSidWhereDefined
        {
            get
            {
                return this.ownerSidWhereDefined;
            }
        }

        /// <summary>
        /// Gets the type of the authorization.
        /// </summary>
        /// <value>The type of the authorization.</value>
        public AuthorizationType AuthorizationType
        {
            get
            {
                return this.authorizationType;
            }
        }

        /// <summary>
        /// Gets the valid from.
        /// </summary>
        /// <value>The valid from.</value>
        public DateTime? ValidFrom
        {
            get
            {
                return this.validFrom;
            }
        }

        /// <summary>
        /// Gets the valid to.
        /// </summary>
        /// <value>The valid to.</value>
        public DateTime? ValidTo
        {
            get
            {
                return this.validTo;
            }
        }

        /// <summary>
        /// Updates the specified authorization type.
        /// </summary>
        /// <param name="owner">The owner Sid.</param>
        /// <param name="sid">The member Sid.</param>
        /// <param name="sidWhereDefined">The object owner where defined.</param>
        /// <param name="authorizationType">Type of the authorization.</param>
        /// <param name="validFrom">The valid from.</param>
        /// <param name="validTo">The valid to.</param>
        public void Update(IAzManSid owner, IAzManSid sid, WhereDefined sidWhereDefined, AuthorizationType authorizationType, DateTime? validFrom, DateTime? validTo)
        {
            if (this.owner.StringValue != owner.StringValue || this.sid.StringValue != sid.StringValue || this.sidWhereDefined != sidWhereDefined || this.authorizationType != authorizationType || this.validFrom != validFrom || this.validTo != validTo)
            {
                //DateTime range check
                if (validFrom.HasValue && validTo.HasValue)
                {
                    if (validFrom.Value > validTo.Value)
                        throw new InvalidOperationException("ValidFrom cannot be greater then ValidTo if supplied.");
                }
                SqlAzManSID oldOwner = new SqlAzManSID(this.owner.StringValue, this.ownerSidWhereDefined == WhereDefined.Database);
                SqlAzManSID oldSid = new SqlAzManSID(this.sid.StringValue, this.sidWhereDefined == WhereDefined.Database);
                WhereDefined oldOwnerSidWhereDefined = this.ownerSidWhereDefined;
                WhereDefined oldSidWhereDefined = this.SidWhereDefined;
                AuthorizationType oldAuthorizationType = this.AuthorizationType;
                DateTime? oldValidFrom = this.validFrom;
                DateTime? oldValidTo = this.validTo;
                string memberName;
                bool isLocal;
                DirectoryServicesUtils.GetMemberInfo(owner.StringValue, out memberName, out isLocal);
                WhereDefined ownerSidWhereDefined = isLocal ? WhereDefined.Local : WhereDefined.LDAP;
                this.db.AuthorizationUpdate(this.item.ItemId, owner.BinaryValue, (byte)ownerSidWhereDefined, sid.BinaryValue, (byte)sidWhereDefined, (byte)authorizationType, (validFrom.HasValue ? validFrom.Value : new DateTime?() ), (validTo.HasValue ? validTo.Value : new DateTime?() ), this.authorizationId, this.item.Application.ApplicationId);
                this.owner = new SqlAzManSID(owner.BinaryValue);
                this.ownerSidWhereDefined = ownerSidWhereDefined;
                this.sid = sid;
                this.sidWhereDefined = sidWhereDefined;
                this.authorizationType = authorizationType;
                this.validFrom = validFrom;
                this.validTo = validTo;
                this.raiseAuthorizationUpdated(this, oldOwner, oldOwnerSidWhereDefined, oldSid, oldSidWhereDefined, oldAuthorizationType, oldValidFrom, oldValidTo);
            }
        }

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        public void Delete()
        {
            this.db.AuthorizationDelete(this.authorizationId, this.item.Application.ApplicationId);
            this.raiseAuthorizationDeleted(this.item, this.owner, this.sid);
        }

        /// <summary>
        /// Gets the authorization attributes.
        /// </summary>
        /// <returns></returns>
        public IAzManAttribute<IAzManAuthorization>[] GetAttributes()
        {
            IAzManAttribute<IAzManAuthorization>[] attributes;
            var attrs = (from a in this.db.AuthorizationAttributes()
                        where a.AuthorizationId == this.authorizationId
                        select a).ToList();

            attributes = new SqlAzManAttribute<IAzManAuthorization>[attrs.Count];
            int index = 0;
            foreach (var row in attrs)
            {
                attributes[index] = new SqlAzManAuthorizationAttribute(this.db, this, row.AuthorizationAttributeId.Value, row.AttributeKey, row.AttributeValue, this.ens);
                if (this.ens != null) this.ens.AddPublisher(attributes[index]);
                index++;
            }
            return attributes;
        }

        /// <summary>
        /// Gets the authorization attribute.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public IAzManAttribute<IAzManAuthorization> GetAttribute(string key)
        {
            AuthorizationAttributesResult attr;
            if ((attr = (from t in this.db.AuthorizationAttributes() where t.AuthorizationId == this.authorizationId && t.AttributeKey == key select t).FirstOrDefault()) != null)
            {
                IAzManAttribute<IAzManAuthorization> result = new SqlAzManAuthorizationAttribute(this.db, this, attr.AuthorizationAttributeId.Value, attr.AttributeKey, attr.AttributeValue, this.ens);
                if (this.ens != null) this.ens.AddPublisher(result);
                return result;
            }
            else
            {
                throw SqlAzManException.AttributeNotFoundException(key, this, null);
            }
        }

        /// <summary>
        /// Creates an authorization attribute.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public IAzManAttribute<IAzManAuthorization> CreateAttribute(string key, string value)
        {
            try
            {
                //db.tAuthorizationattributes.Insert(this.authorizationId, key, value);
                int authorizationAttributeId = 0;
                authorizationAttributeId = this.db.AuthorizationAttributeInsert(this.authorizationId, key, value, this.item.Application.ApplicationId);
                this.db.SubmitChanges();
                IAzManAttribute<IAzManAuthorization> result = new SqlAzManAuthorizationAttribute(this.db, this, authorizationAttributeId, key, value, this.ens);
                this.raiseAuthorizationAttributeCreated(this, result);
                if (this.ens != null) this.ens.AddPublisher(result);
                return result;
            }
            catch (System.Data.SqlClient.SqlException sqlex)
            {
                if (sqlex.Number == 2601) //Index Duplicate Error
                    throw SqlAzManException.AttributeDuplicateException(key, this, sqlex);
                else
                    throw SqlAzManException.GenericException(sqlex);
            }
        }
        #endregion
        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            
        }
        /// <summary>
        /// Gets the member info.
        /// </summary>
        /// <param name="displayName">Display name of the member.</param>
        /// <returns></returns>
        public MemberType GetMemberInfo(out string displayName)
        {
            switch (this.SidWhereDefined)
            {
                case WhereDefined.Store:
                    displayName = this.item.Application.Store.GetStoreGroup(this.sid).Name;
                    return MemberType.StoreGroup;
                case WhereDefined.Application:
                    displayName = this.item.Application.GetApplicationGroup(this.sid).Name;
                    return MemberType.ApplicationGroup;
                case WhereDefined.LDAP:
                    bool isAnLdapGroup;
                    bool isLocal;
                    DirectoryServicesUtils.GetMemberInfo(this.sid.StringValue, out displayName, out isAnLdapGroup, out isLocal);
                    if (this.sid.StringValue.Equals(displayName))
                        return MemberType.AnonymousSID;
                    return isAnLdapGroup ? MemberType.WindowsNTGroup : MemberType.WindowsNTUser;
                case WhereDefined.Local:
                    if (this.item.Application.Store.Storage.Mode == NetSqlAzManMode.Administrator)
                        break;
                    bool isALocalGroup;
                    bool isLocal2;
                    DirectoryServicesUtils.GetMemberInfo(this.sid.StringValue, out displayName, out isALocalGroup, out isLocal2);
                    if (this.sid.StringValue.Equals(displayName))
                        return MemberType.AnonymousSID;
                    return isALocalGroup ? MemberType.WindowsNTGroup : MemberType.WindowsNTUser;
                case WhereDefined.Database:
                    displayName = this.item.Application.GetDBUser(this.sid).UserName;
                    if (displayName == this.sid.StringValue)
                        displayName = this.item.Application.Store.GetDBUser(this.sid).UserName;
                    if (displayName == this.sid.StringValue)
                        displayName = this.item.Application.Store.Storage.GetDBUser(this.sid).UserName;
                    return MemberType.DatabaseUser;
            }
            displayName = this.sid.StringValue;
            return MemberType.AnonymousSID;
        }
        /// <summary>
        /// Gets the owner info.
        /// </summary>
        /// <param name="displayName">Display name of the Owner.</param>
        /// <returns></returns>
        public MemberType GetOwnerInfo(out string displayName)
        {
            if (System.Security.Principal.WindowsIdentity.GetCurrent().User.Value == this.owner.StringValue)
            {
                displayName = ((System.Threading.Thread.CurrentPrincipal.Identity as WindowsIdentity) ?? WindowsIdentity.GetCurrent()).Name;
                return MemberType.WindowsNTUser;
            }
            if (this.ownerSidWhereDefined == WhereDefined.Database)
            {
                displayName = this.item.Application.GetDBUser(this.owner).UserName;
                return MemberType.DatabaseUser;
            }
            bool isAGroup;
            bool isLocal;
            DirectoryServicesUtils.GetMemberInfo(this.owner.StringValue, out displayName, out isAGroup, out isLocal);
            if (this.item.Application.Store.Storage.Mode == NetSqlAzManMode.Administrator && isLocal)
            {
                displayName = this.owner.StringValue;
                return MemberType.AnonymousSID;
            }
            return isAGroup ? MemberType.WindowsNTGroup : MemberType.WindowsNTUser;
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
        public void Export(XmlWriter xmlWriter, bool includeWindowsUsersAndGroups, bool includeDBUsers, bool includeAuthorizations, object ownerOfExport)
        {
            if (includeAuthorizations)
            {
                if (
                    includeDBUsers && this.sidWhereDefined == WhereDefined.Database
                    ||
                    includeWindowsUsersAndGroups && this.sidWhereDefined == WhereDefined.LDAP
                    ||
                    includeWindowsUsersAndGroups && this.sidWhereDefined == WhereDefined.Local && this.item.Application.Store.Storage.Mode == NetSqlAzManMode.Developer
                    ||
                    this.sidWhereDefined == WhereDefined.Store && (ownerOfExport as IAzManStorage != null || ownerOfExport as IAzManStore != null || ownerOfExport as IAzManApplication != null)
                    ||
                    this.sidWhereDefined == WhereDefined.Application && (ownerOfExport as IAzManStorage != null || ownerOfExport as IAzManStore != null || ownerOfExport as IAzManApplication != null)
                )
                {
                    xmlWriter.WriteStartElement("Authorization");
                    xmlWriter.WriteAttributeString("Owner", this.owner.StringValue);
                    xmlWriter.WriteAttributeString("OwnerSidWhereDefined", this.OwnerSidWhereDefined.ToString());
                    xmlWriter.WriteAttributeString("Sid", this.sid.StringValue);
                    xmlWriter.WriteAttributeString("SidWhereDefined", this.SidWhereDefined.ToString());
                    xmlWriter.WriteAttributeString("AuthorizationType", this.AuthorizationType.ToString());
                    if (this.validFrom.HasValue)
                        xmlWriter.WriteAttributeString("ValidFrom", this.validFrom.Value.ToString());
                    else
                        xmlWriter.WriteAttributeString("ValidFrom", "Null");
                    if (this.validTo.HasValue)
                        xmlWriter.WriteAttributeString("ValidTo", this.ValidTo.Value.ToString());
                    else
                        xmlWriter.WriteAttributeString("ValidTo", "Null");
                    //Attributes
                    xmlWriter.WriteStartElement("Attributes");
                    foreach (IAzManAttribute<IAzManAuthorization> authorizationAttribute in this.GetAttributes())
                    {
                        ((IAzManExport)authorizationAttribute).Export(xmlWriter, includeWindowsUsersAndGroups, includeDBUsers, includeAuthorizations, ownerOfExport);
                    }
                    xmlWriter.WriteEndElement();
                    xmlWriter.WriteEndElement();
                }
            }
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
                if (node.Name == "Attributes")
                {
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        if (childNode.Name == "Attribute")
                        {
                            IAzManAttribute<IAzManAuthorization> newAuthorizationAttribute = this.CreateAttribute(childNode.Attributes["Key"].Value, childNode.Attributes["Value"].Value);
                        }
                    }
                }
                else if (node.Name == "Attribute")
                {
                    IAzManAttribute<IAzManAuthorization> newAuthorizationAttribute = this.CreateAttribute(node.Attributes["Key"].Value, node.Attributes["Value"].Value);
                }

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
            return String.Format("Authorization ID: {0}\r\nSID: {1}\r\nWhere Defined: {2}\r\nOwner SID: {3}\r\nOwner Where Defined: {4}\r\nAuthorization Type: {5}\r\nValid From: {6}\r\nValid To: {7}",
                this.authorizationId, this.sid, this.sidWhereDefined, this.owner, this.ownerSidWhereDefined, this.authorizationType, this.validFrom.HasValue ? this.validFrom.Value.ToString() : "", this.validTo.HasValue ? this.validTo.Value.ToString() : "");
        }
        #endregion Object Members
    }
}
