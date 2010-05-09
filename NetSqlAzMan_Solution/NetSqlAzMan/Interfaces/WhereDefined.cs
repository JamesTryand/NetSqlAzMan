using System.Runtime.Serialization;

namespace NetSqlAzMan.Interfaces
{
    /// <summary>
    /// Position of where objects are defined
    /// </summary>
    [DataContract(Namespace="http://NetSqlAzMan/ServiceModel")]
    public enum WhereDefined : byte
    {
        /// <summary>
        /// Defined at store-level
        /// </summary>
        [EnumMember()]
        Store = 0,
        /// <summary>
        /// Defined at application-level
        /// </summary>
        [EnumMember()]
        Application = 1,
        /// <summary>
        /// Defined at LDAP-level
        /// </summary>
        [EnumMember()]
        LDAP = 2,
        /// <summary>
        /// Defined on Local machine
        /// </summary>
        [EnumMember()]
        Local = 3,
        /// <summary>
        /// Defined on a Database
        /// </summary>
        [EnumMember()]
        Database = 4,
    }
}
