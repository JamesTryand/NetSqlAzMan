using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace NetSqlAzMan.Interfaces
{
    /// <summary>
    /// Is the Item Type categorization for Items. An itemName can be of one of these types.
    /// </summary>
    [DataContract(Namespace="http://NetSqlAzMan/ServiceModel")]
    public enum ItemType
    {
        /// <summary>
        /// A Role itemName can contain: Roles, Tasks, Operations.
        /// </summary>
        /// <remarks>Administrative purpose only. Do not use in the Applications.</remarks>
        [EnumMember()]
        Role,
        /// <summary>
        /// A Task itemName can contain: Tasks, Operations.
        /// </summary>
        /// <remarks>Administrative purpose only. Do not use in the Applications.</remarks>
        [EnumMember()]
        Task,
        /// <summary>
        /// An Operation can contain: Operations.
        /// </summary>
        /// <remarks>Administrative and Developer purpose. Invoke Operations CheckAccess in the Applications.</remarks>
        [EnumMember()]
        Operation
    }
}
