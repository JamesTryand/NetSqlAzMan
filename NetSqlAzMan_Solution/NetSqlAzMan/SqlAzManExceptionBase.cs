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
    /// AzMan Exception Base class.
    /// </summary>
    [DataContract(Namespace = "http://NetSqlAzMan/ServiceModel", IsReference = true)]
    public abstract class SqlAzManExceptionBase : Exception
    {
        internal SqlAzManExceptionBase(string message)
            : this(message, null)
        { 
        
        }

        internal SqlAzManExceptionBase(string message, Exception innerException) : base(message, innerException)
        { 
        
        }
    }
}
