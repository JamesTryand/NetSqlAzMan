using System;
using System.Runtime.Serialization;
using NetSqlAzMan.ENS;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.LINQ;

namespace NetSqlAzMan
{
    /// <summary>
    /// Store Attribute
    /// </summary>
    [Serializable()]
    [DataContract(Namespace = "http://NetSqlAzMan/ServiceModel", IsReference = true)]
    public sealed partial class SqlAzManStoreAttribute : SqlAzManAttribute<IAzManStore>
    {
        #region Constructors
        internal SqlAzManStoreAttribute(NetSqlAzManStorageDataContext db, IAzManStore owner, int attributeId, string key, string value, SqlAzManENS ens)
            : base(db, owner, attributeId, key, value, ens)
        {

        }
        #endregion Constructors
        #region IAzManAttribute Members
        /// <summary>
        /// Updates the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public override void Update(string key, string value)
        {
            if (this.key != key || this.value != value)
            {
                string oldKey = this.key;
                string oldValue = this.value;
                this.db.StoreAttributeUpdate(this.Owner.StoreId, key, value, this.attributeId);
                this.key = key;
                this.value = value;
                this.raiseAttributeUpdated(this, oldKey, oldValue);
            }
        }

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        public override void Delete()
        {
            this.db.StoreAttributeDelete(this.owner.StoreId, this.attributeId);
            this.raiseAttributeDeleted(this.owner, this.key);
        }

        #endregion IAzManAttribute Members
    }
}
