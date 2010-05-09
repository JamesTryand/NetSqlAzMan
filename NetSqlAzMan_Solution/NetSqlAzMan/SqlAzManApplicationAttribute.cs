using System;
using System.Runtime.Serialization;
using NetSqlAzMan.ENS;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.LINQ;

namespace NetSqlAzMan
{
    /// <summary>
    /// Application Attribute
    /// </summary>
    [Serializable()]
    [DataContract(Namespace = "http://NetSqlAzMan/ServiceModel", IsReference = true)]
    public sealed partial class SqlAzManApplicationAttribute : SqlAzManAttribute<IAzManApplication>
    {
        #region Constructors
        internal SqlAzManApplicationAttribute(NetSqlAzManStorageDataContext db, IAzManApplication owner, int attributeId, string key, string value, SqlAzManENS ens)
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
                this.db.ApplicationAttributeUpdate(this.owner.ApplicationId, key, value, this.attributeId);
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
            this.db.ApplicationAttributeDelete(this.owner.ApplicationId, this.attributeId);
            this.raiseAttributeDeleted(this.owner, this.key);
        }

        #endregion IAzManAttribute Members
    }
}
