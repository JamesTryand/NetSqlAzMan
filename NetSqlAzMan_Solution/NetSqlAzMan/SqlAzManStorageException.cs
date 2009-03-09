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
    /// AzMan Storage Exception class.
    /// </summary>
    [DataContract(Namespace = "http://NetSqlAzMan/ServiceModel", IsReference = true)]
    public partial class SqlAzManStorageException : SqlAzManExceptionBase
    {
        private IAzManStorage storage;

        /// <summary>
        /// Gets the name of the storage.
        /// </summary>
        /// <value>The name of the storage.</value>
        public IAzManStorage Storage
        {
            get { return storage; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AzManStorageException"/> class.
        /// </summary>
        /// <param name="storage">The storage.</param>
        /// <param name="message">The message.</param>
        public SqlAzManStorageException(IAzManStorage storage, string message)
            : this(storage, message, null)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AzManStorageException"/> class.
        /// </summary>
        /// <param name="storage">The storage.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public SqlAzManStorageException(IAzManStorage storage, string message, Exception innerException)
            : base(message, innerException)
        {
            this.storage = storage;
        }

    }
}
