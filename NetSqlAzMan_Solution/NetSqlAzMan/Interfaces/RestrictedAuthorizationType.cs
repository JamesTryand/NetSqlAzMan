using System.Runtime.Serialization;

namespace NetSqlAzMan.Interfaces
{
    /// <summary>
    /// Restricted Authorization Type
    /// </summary>
    [DataContract(Namespace="http://NetSqlAzMan/ServiceModel")]
    public enum RestrictedAuthorizationType : byte
    {
        /// <summary>
        /// Allow.
        /// </summary>
        [EnumMember()]
        Allow = 1,
        /// <summary>
        /// Deny.
        /// </summary>
        [EnumMember()]
        Deny = 2        
    }
}
