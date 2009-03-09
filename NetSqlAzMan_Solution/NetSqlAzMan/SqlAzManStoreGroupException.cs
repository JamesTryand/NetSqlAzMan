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
    /// AzMan Store Group Exception class.
    /// </summary>
    [DataContract(Namespace = "http://NetSqlAzMan/ServiceModel", IsReference = true)]
    public partial class SqlAzManStoreGroupException : SqlAzManExceptionBase
    {
        private IAzManStoreGroup storeGroup;
        /// <summary>
        /// Gets the store.
        /// </summary>
        /// <value>The store.</value>
        public IAzManStoreGroup StoreGroup
        {
            get { return storeGroup; }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SqlAzManStoreGroupException"/> class.
        /// </summary>
        /// <param name="storeGroup">The store group.</param>
        /// <param name="message">The message.</param>
        public SqlAzManStoreGroupException(IAzManStoreGroup storeGroup, string message)
            : this(storeGroup, message, null)
        {
            
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SqlAzManStoreGroupException"/> class.
        /// </summary>
        /// <param name="storeGroup">The store group.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public SqlAzManStoreGroupException(IAzManStoreGroup storeGroup, string message, Exception innerException)
            : base(message, innerException)
        {
            this.storeGroup = storeGroup;
        }

    }
}
