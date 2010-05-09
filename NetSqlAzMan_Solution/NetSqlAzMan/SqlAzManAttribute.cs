using System;
using System.Runtime.Serialization;
using System.Xml;
using NetSqlAzMan.ENS;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.LINQ;

namespace NetSqlAzMan
{
    /// <summary>
    /// Interfaces interface for all Attributes
    /// </summary>
    [Serializable()]
    [DataContract(Namespace = "http://NetSqlAzMan/ServiceModel", IsReference = true)]
    public abstract partial class SqlAzManAttribute<OWNER> : IAzManAttribute<OWNER>
    {
        #region Fields
        /// <summary>
        /// NetSqlAzManStorageDataContext object reference
        /// </summary>
        [NonSerialized()]
        protected NetSqlAzManStorageDataContext db;
        /// <summary>
        /// Attribute Id
        /// </summary>
        protected int attributeId;
        /// <summary>
        /// Attribute Owner
        /// </summary>
        protected OWNER owner;
        /// <summary>
        /// Attribute Key
        /// </summary>
        protected string key;
        /// <summary>
        /// Attribute Value
        /// </summary>
        protected string value;
        /// <summary>
        /// Event Notification System
        /// </summary>
        [NonSerialized()]
        protected SqlAzManENS ens;
        #endregion Fields
        #region Events
        /// <summary>
        /// Occurs after an Attribute object has been Deleted.
        /// </summary>
        public event AttributeDeletedDelegate<OWNER> AttributeDeleted;
        /// <summary>
        /// Occurs after an Attribute object has been Updated.
        /// </summary>
        public event AttributeUpdatedDelegate<OWNER> AttributeUpdated;
        #endregion Events
        #region Protected Event Raisers
        /// <summary>
        /// Raises the attribute deleted.
        /// </summary>
        /// <param name="owner">The owner.</param>
        /// <param name="key">The key.</param>
        protected void raiseAttributeDeleted(OWNER owner, string key)
        {
            if (this.AttributeDeleted != null)
                this.AttributeDeleted(owner, key);
        }
        /// <summary>
        /// Raises the attribute updated.
        /// </summary>
        /// <param name="attribute">The attribute.</param>
        /// <param name="oldKey">The old key.</param>
        /// <param name="oldValue">The old value.</param>
        protected void raiseAttributeUpdated(IAzManAttribute<OWNER> attribute, string oldKey, string oldValue)
        {
            if (this.AttributeUpdated != null)
                this.AttributeUpdated(attribute, oldKey, oldValue);
        }
        #endregion Protected Event Raisers
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SqlAzManAttribute&lt;OWNER&gt;"/> class.
        /// </summary>
        /// <param name="db">The db.</param>
        /// <param name="owner">The owner.</param>
        /// <param name="attributeId">The attribute id.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="ens">The ens.</param>
        protected internal SqlAzManAttribute(NetSqlAzManStorageDataContext db, OWNER owner, int attributeId, string key, string value, SqlAzManENS ens)
        {
            this.db = db;
            this.owner = owner;
            this.attributeId = attributeId;
            this.key = key;
            this.value = value;
            this.ens = ens;
        }
        #endregion Constructors
        #region IAzManAttribute Members

        /// <summary>
        /// Gets the attribute id.
        /// </summary>
        /// <value>The attribute id.</value>
        public int AttributeId
        {
            get
            {
                return this.attributeId;
            }
        }

        /// <summary>
        /// Gets the Owner.
        /// </summary>
        /// <value>The Owner.</value>
        public OWNER Owner
        {
            get
            {
                return this.owner;
            }
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        public string Key
        {
            get
            {
                return this.key;
            }
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        public string Value
        {
            get
            {
                return this.value;
            }
        }

        /// <summary>
        /// Updates the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public abstract void Update(string key, string value);

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        public abstract void Delete();
        #endregion IAzManAttribute Members
        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            
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
            xmlWriter.WriteStartElement("Attribute");
            xmlWriter.WriteAttributeString("Key", this.key);
            xmlWriter.WriteAttributeString("Value", this.value);
            xmlWriter.WriteEndElement();
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
            return String.Format("Attribute ID: {0}\r\nKey: {1}\r\nValue: {2}",
                this.attributeId, this.key, this.value);
        }
        #endregion Object Members
    }
}
