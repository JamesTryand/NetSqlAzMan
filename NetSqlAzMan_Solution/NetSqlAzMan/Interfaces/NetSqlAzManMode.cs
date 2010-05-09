using System.Runtime.Serialization;

namespace NetSqlAzMan.Interfaces
{
    /// <summary>
    /// .Net Sql Authorization Manager Mode
    /// </summary>
    [DataContract(Namespace="http://NetSqlAzMan/ServiceModel")]
    public enum NetSqlAzManMode
    {
        /// <summary>
        /// Administrator Mode
        /// </summary>
        [EnumMember()]
        Administrator = 0,
        /// <summary>
        /// Developer Mode
        /// </summary>
        [EnumMember()]
        Developer = 1
    }
}
