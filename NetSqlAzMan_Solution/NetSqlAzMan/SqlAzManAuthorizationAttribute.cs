using System;
using System.Data.SqlTypes;
using System.Xml;
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
    /// Authorization Attribute
    /// </summary>
    [Serializable()]
    [DataContract(Namespace = "http://NetSqlAzMan/ServiceModel", IsReference = true)]
    public sealed partial class SqlAzManAuthorizationAttribute : SqlAzManAttribute<IAzManAuthorization>
    {
        #region Constructors
        internal SqlAzManAuthorizationAttribute(NetSqlAzManStorageDataContext db, IAzManAuthorization owner, int attributeId, string key, string value, SqlAzManENS ens)
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
                this.db.AuthorizationAttributeUpdate(key, value, this.AttributeId, this.Owner.Item.Application.ApplicationId);
                this.raiseAttributeUpdated(this, oldKey, oldValue);
            }
        }

        /// <summary>
        /// Deletes this instance.
        /// </summary>
        public override void Delete()
        {
            this.db.AuthorizationAttributeDelete(this.attributeId, this.owner.Item.Application.ApplicationId);
            this.raiseAttributeDeleted(this.owner, this.key);
        }

        #endregion IAzManAttribute Members
    }
}
