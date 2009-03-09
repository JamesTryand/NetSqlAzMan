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
    /// AzMan Store Exception class.
    /// </summary>
    [DataContract(Namespace = "http://NetSqlAzMan/ServiceModel", IsReference = true)]
    public partial class SqlAzManStoreException : SqlAzManExceptionBase
    {
        private IAzManStore store;

        /// <summary>
        /// Gets the name of the store.
        /// </summary>
        /// <value>The name of the store.</value>
        public IAzManStore Store
        {
            get { return store; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AzManStoreException"/> class.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="message">The message.</param>
        public SqlAzManStoreException(IAzManStore store, string message)
            : this(store, message, null)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AzManStoreException"/> class.
        /// </summary>
        /// <param name="store">The store.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public SqlAzManStoreException(IAzManStore store, string message, Exception innerException)
            : base(message, innerException)
        {
            this.store = store;
        }

    }
}
