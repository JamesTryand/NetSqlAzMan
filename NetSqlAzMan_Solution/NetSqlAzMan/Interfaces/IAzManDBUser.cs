using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace NetSqlAzMan.Interfaces
{
    /// <summary>
    /// Common Interface for All Database Custom Users
    /// </summary>
    [ServiceContract(Namespace="http://NetSqlAzMan/ServiceModel", SessionMode=SessionMode.Required)]
    public interface IAzManDBUser
    {
        /// <summary>
        /// Custom Unique identifier of the DB User
        /// </summary>
        [DataMember]
        IAzManSid CustomSid { get; }
        /// <summary>
        /// Username of the DB User
        /// </summary>
        [DataMember]
        string UserName { get; }
        /// <summary>
        /// Gets the custom columns.
        /// </summary>
        /// <value>The custom columns.</value>
        [DataMember]
        Dictionary<string, object> CustomColumns { get; }
    }
}
