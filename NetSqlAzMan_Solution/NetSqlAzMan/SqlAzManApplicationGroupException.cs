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
    /// AzMan Application Group Exception class.
    /// </summary>
    [DataContract(Namespace = "http://NetSqlAzMan/ServiceModel", IsReference = true)]
    public partial class SqlAzManApplicationGroupException : SqlAzManExceptionBase
    {
        private IAzManApplicationGroup applicationGroup;
        /// <summary>
        /// Gets the application.
        /// </summary>
        /// <value>The application.</value>
        public IAzManApplicationGroup ApplicationGroup
        {
            get { return applicationGroup; }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SqlAzManApplicationGroupException"/> class.
        /// </summary>
        /// <param name="applicationGroup">The application group.</param>
        /// <param name="message">The message.</param>
        public SqlAzManApplicationGroupException(IAzManApplicationGroup applicationGroup, string message)
            : this(applicationGroup, message, null)
        {
            
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SqlAzManApplicationGroupException"/> class.
        /// </summary>
        /// <param name="applicationGroup">The application group.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public SqlAzManApplicationGroupException(IAzManApplicationGroup applicationGroup, string message, Exception innerException)
            : base(message, innerException)
        {
            this.applicationGroup = applicationGroup;
        }

    }
}
