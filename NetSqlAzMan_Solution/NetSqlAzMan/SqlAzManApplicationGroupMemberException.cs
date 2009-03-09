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
    /// AzMan Application Group Member Exception class.
    /// </summary>
    [DataContract(Namespace = "http://NetSqlAzMan/ServiceModel", IsReference = true)]
    public partial class SqlAzManApplicationGroupMemberException : SqlAzManExceptionBase
    {
        private IAzManApplicationGroupMember applicationGroupMember;
        /// <summary>
        /// Gets the application group member.
        /// </summary>
        /// <value>The application group member.</value>
        public IAzManApplicationGroupMember ApplicationGroupMember
        {
            get { return applicationGroupMember; }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SqlAzManApplicationGroupException"/> class.
        /// </summary>
        /// <param name="applicationGroupMember">The application group member.</param>
        /// <param name="message">The message.</param>
        public SqlAzManApplicationGroupMemberException(IAzManApplicationGroupMember applicationGroupMember, string message)
            : this(applicationGroupMember, message, null)
        {
            
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:SqlAzManApplicationGroupException"/> class.
        /// </summary>
        /// <param name="applicationGroupMember">The application group member.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public SqlAzManApplicationGroupMemberException(IAzManApplicationGroupMember applicationGroupMember, string message, Exception innerException)
            : base(message, innerException)
        {
            this.applicationGroupMember = applicationGroupMember;
        }

    }
}
