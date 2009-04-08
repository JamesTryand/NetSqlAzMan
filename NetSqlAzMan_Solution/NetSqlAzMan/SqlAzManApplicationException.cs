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
    /// AzMan Application Exception class.
    /// </summary>
    [DataContract(Namespace = "http://NetSqlAzMan/ServiceModel", IsReference = true)]
    public partial class SqlAzManApplicationException : SqlAzManExceptionBase
    {   
        private IAzManApplication application;

        /// <summary>
        /// Gets the name of the application.
        /// </summary>
        /// <value>The name of the store.</value>
        public IAzManApplication Application
        {
            get { return application; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AzManStoreException"/> class.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="message">The message.</param>
        public SqlAzManApplicationException(IAzManApplication application, string message)
            : this(application, message, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AzManStoreException"/> class.
        /// </summary>
        /// <param name="application">The application.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public SqlAzManApplicationException(IAzManApplication application, string message, Exception innerException)
            : base(message, innerException)
        {
            this.application = application;
        }
    }
}
