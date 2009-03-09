using System;
using System.Collections.Generic;
using System.Text;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.LINQ;
using System.Linq;
using System.Runtime.Serialization;

namespace NetSqlAzMan
{
    /// <summary>
    /// AzMan Store Group Member Exception class.
    /// </summary>
    [DataContract(Namespace = "http://NetSqlAzMan/ServiceModel", IsReference = true)]
    public partial class SqlAzManStoreGroupMemberException : SqlAzManExceptionBase
    {
        private IAzManStoreGroupMember storeGroupMember;
        /// <summary>
        /// Gets the store.
        /// </summary>
        /// <value>The store.</value>
        public IAzManStoreGroupMember StoreGroupMember
        {
            get { return storeGroupMember; }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SqlAzManStoreGroupException"/> class.
        /// </summary>
        /// <param name="storeGroupMember">The store group member.</param>
        /// <param name="message">The message.</param>
        public SqlAzManStoreGroupMemberException(IAzManStoreGroupMember storeGroupMember, string message)
            : this(storeGroupMember, message, null)
        {
            
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SqlAzManStoreGroupException"/> class.
        /// </summary>
        /// <param name="storeGroupMember">The store group member.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public SqlAzManStoreGroupMemberException(IAzManStoreGroupMember storeGroupMember, string message, Exception innerException)
            : base(message, innerException)
        {
            this.storeGroupMember = storeGroupMember;
        }

    }
}
