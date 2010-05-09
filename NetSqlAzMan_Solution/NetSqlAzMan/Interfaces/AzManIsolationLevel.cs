using System.Runtime.Serialization;

namespace NetSqlAzMan.Interfaces
{
    /// <summary>
    /// Specifies the transaction locking behavior for AzMan store operations. 
    /// </summary>
    [DataContract(Namespace="http://NetSqlAzMan/ServiceModel")]
    public enum AzManIsolationLevel
    {
        /// <summary>
        /// Shared locks are held while the data is being read to avoid dirty reads, but the data can be changed before the end of the transaction, resulting in non-repeatable reads or phantom data. 
        /// </summary>
        [EnumMember()]
        ReadCommitted,
        /// <summary>
        /// A dirty read is possible, meaning that no shared locks are issued and no exclusive locks are honored. 
        /// </summary>
        [EnumMember()]
        ReadUncommitted,
        /// <summary>
        /// Locks are placed on all data that is used in a query, preventing other users from updating the data. Prevents non-repeatable reads but phantom rows are still possible. 
        /// </summary>
        [EnumMember()]
        RepeatableRead,
        /// <summary>
        /// Reduces blocking by storing a version of data that one application can read while another is modifying the same data. Indicates that from one transaction you cannot see changes made in other transactions, even if you requery. 
        /// </summary>
        [EnumMember()]
        Snapshot
    }
}
