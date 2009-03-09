using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace NetSqlAzMan.Interfaces
{
    /// <summary>
    /// Represents a Security IDentifier (SID)
    /// </summary>
    [ServiceContract(Namespace="http://NetSqlAzMan/ServiceModel", SessionMode=SessionMode.Required)]
    public interface IAzManSid
    {
        /// <summary>
        /// Gets the binary value.
        /// </summary>
        /// <value>The binary value.</value>
        [DataMember]
        byte[] BinaryValue { get; }
        /// <summary>
        /// Gets the string value.
        /// </summary>
        /// <value>The string value.</value>
        [DataMember]
        string StringValue { get; }
    }
}
