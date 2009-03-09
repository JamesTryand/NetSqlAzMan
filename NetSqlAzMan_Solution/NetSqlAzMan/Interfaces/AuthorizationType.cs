using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace NetSqlAzMan.Interfaces
{
    /// <summary>
    /// Authorization Type
    /// </summary>
    [DataContract(Namespace="http://NetSqlAzMan/ServiceModel")]
    public enum AuthorizationType : byte
    {
        /// <summary>
        /// Neutral.
        /// </summary>
        [EnumMember()]
        Neutral = 0,
        /// <summary>
        /// Allow.
        /// </summary>
        [EnumMember()]
        Allow = 1,
        /// <summary>
        /// Deny.
        /// </summary>
        [EnumMember()]
        Deny = 2,
        /// <summary>
        /// Allow with delegation
        /// </summary>
        [EnumMember()]
        AllowWithDelegation = 3
        
    }
}
