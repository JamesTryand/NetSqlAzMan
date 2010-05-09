using System;
using System.Runtime.Serialization;

namespace NetSqlAzMan
{
    /// <summary>
    /// SqlAzMan Merge Options (Import)
    /// </summary>
    [Flags()]
    [DataContract(Namespace="http://NetSqlAzMan/ServiceModel", IsReference = true)]
    public enum SqlAzManMergeOptions
    {
        /// <summary>
        /// No merge
        /// </summary>
        [EnumMember]
        NoMerge = 0,
        /// <summary>
        /// Create New Items
        /// </summary>
        [EnumMember]
        CreatesNewItems = 1,
        /// <summary>
        /// Overwrites Existing Items
        /// </summary>
        [EnumMember]
        OverwritesExistingItems = 2,
        /// <summary>
        /// Delete Missing Items 
        /// </summary>
        [EnumMember]
        DeleteMissingItems = 4,
        /// <summary>
        /// Creates New Item Authorizations  
        /// </summary>
        [EnumMember]
        CreatesNewItemAuthorizations = 8,
        /// <summary>
        /// Overwrites Existing Item Authorization
        /// </summary>
        [EnumMember]
        OverwritesExistingItemAuthorization = 16,
        /// <summary>
        /// Delete Missing Item Authorizations
        /// </summary>
        [EnumMember]
        DeleteMissingItemAuthorizations = 32
    }
}
