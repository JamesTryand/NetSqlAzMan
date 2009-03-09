using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;

namespace NetSqlAzMan.Interfaces
{
    /// <summary>
    /// Type of member
    /// </summary>
    [DataContract(Namespace="http://NetSqlAzMan/ServiceModel")]
    public enum MemberType
    {
        /// <summary>
        /// StoreGroup type
        /// </summary>
        [EnumMember()]
        StoreGroup,
        /// <summary>
        /// ApplicationGroup type
        /// </summary>
        [EnumMember()]
        ApplicationGroup,
        /// <summary>
        /// WindowsNTGroup type
        /// </summary>
        [EnumMember()]
        WindowsNTGroup,
        /// <summary>
        /// WindowsNTUser type
        /// </summary>
        [EnumMember()]
        WindowsNTUser,
        /// <summary>
        /// AnonymousSID type
        /// </summary>
        [EnumMember()]
        AnonymousSID,
        /// <summary>
        /// DatabaseUser type
        /// </summary>
        [EnumMember()]
        DatabaseUser
    }
}
