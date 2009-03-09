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
    public partial class SqlAzManItemException : SqlAzManExceptionBase
    {
        private IAzManItem item;

        /// <summary>
        /// Gets the name of the itemName.
        /// </summary>
        /// <value>The name of the store.</value>
        public IAzManItem Item
        {
            get { return item; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AzManStoreException"/> class.
        /// </summary>
        /// <param name="item">The itemName.</param>
        /// <param name="message">The message.</param>
        public SqlAzManItemException(IAzManItem item, string message)
            : this(item, message, null)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:AzManStoreException"/> class.
        /// </summary>
        /// <param name="item">The itemName.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public SqlAzManItemException(IAzManItem item, string message, Exception innerException)
            : base(message, innerException)
        {
            this.item = item;
        }

    }
}
