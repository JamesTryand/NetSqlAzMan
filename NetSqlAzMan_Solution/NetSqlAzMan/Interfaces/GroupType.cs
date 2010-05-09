using System.Runtime.Serialization;

namespace NetSqlAzMan.Interfaces
{
    /// <summary>
    /// Type of Group for Store Groups and Application Groups
    /// </summary>
    [DataContract(Namespace="http://NetSqlAzMan/ServiceModel")]
    public enum GroupType
    {
        /// <summary>
        /// Basic group
        /// </summary>
        [EnumMember()]
        Basic = 0,
        /// <summary>
        /// Dynamic Group (LDAP query)
        /// </summary>
        [EnumMember()]
        LDapQuery = 1
    }
}
