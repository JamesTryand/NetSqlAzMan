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
    /// AzMan Authorization Exception class.
    /// </summary>
    [DataContract(Namespace = "http://NetSqlAzMan/ServiceModel", IsReference = true)]
    public partial class SqlAzManAuthorizationException : SqlAzManExceptionBase
    {
        private IAzManAuthorization authorization;

        /// <summary>
        /// Gets the name of the authorization.
        /// </summary>
        /// <value>The name of the store.</value>
        public IAzManAuthorization Authorization
        {
            get { return authorization; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AzManStoreException"/> class.
        /// </summary>
        /// <param name="authorization">The authorization.</param>
        /// <param name="message">The message.</param>
        public SqlAzManAuthorizationException(IAzManAuthorization authorization, string message)
            : this(authorization, message, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AzManStoreException"/> class.
        /// </summary>
        /// <param name="authorization">The authorization.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public SqlAzManAuthorizationException(IAzManAuthorization authorization, string message, Exception innerException)
            : base(message, innerException)
        {
            this.authorization = authorization;
        }
    }
}
