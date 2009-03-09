using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NetSqlAzMan.LINQ
{
    /// <summary>
    /// BuildUserPermissionCacheResult2 LINQ class.
    /// </summary>
    [Serializable()]
	public partial class BuildUserPermissionCacheResult2 : ISerializable
	{
        #region ISerializable Members

        /// <summary>
        /// Initializes a new instance of the <see cref="BuildUserPermissionCacheResult2"/> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        public BuildUserPermissionCacheResult2(SerializationInfo info, StreamingContext context)
        {
            this._ItemName = info.GetString("_ItemName");
            this._ValidFrom = (System.Nullable<System.DateTime>)info.GetValue("_ValidFrom", typeof(System.Nullable<System.DateTime>));
            this._ValidTo = (System.Nullable<System.DateTime>)info.GetValue("_ValidTo", typeof(System.Nullable<System.DateTime>));
        }

        /// <summary>
        /// Populates a <see cref="T:System.Runtime.Serialization.SerializationInfo"/> with the data needed to serialize the target object.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> to populate with data.</param>
        /// <param name="context">The destination (see <see cref="T:System.Runtime.Serialization.StreamingContext"/>) for this serialization.</param>
        /// <exception cref="T:System.Security.SecurityException">The caller does not have the required permission. </exception>
        public void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            info.AddValue("_ItemName", this._ItemName);
            info.AddValue("_ValidFrom", this._ValidFrom);
            info.AddValue("_ValidTo", this._ValidTo);
        }

        #endregion
    }
}
