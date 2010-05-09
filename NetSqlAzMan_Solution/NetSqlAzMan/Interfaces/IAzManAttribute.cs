using System;
using System.Runtime.Serialization;
using System.ServiceModel;
using NetSqlAzMan.ENS;

namespace NetSqlAzMan.Interfaces
{
    /// <summary>
    /// Interfaces interface for all Attributes
    /// </summary>
    [ServiceContract(Namespace="http://NetSqlAzMan/ServiceModel", SessionMode=SessionMode.Required)]
    public interface IAzManAttribute<OWNER> : IAzManExport, IDisposable
    {
        /// <summary>
        /// Gets the authorization attribute id.
        /// </summary>
        /// <value>The authorization attribute id.</value>
        [DataMember]
        int AttributeId { get; }
        /// <summary>
        /// Gets the Owner.
        /// </summary>
        /// <value>The parent.</value>
        [DataMember]
        OWNER Owner { get; }
        /// <summary>
        /// Gets the key.
        /// </summary>
        /// <value>The key.</value>
        [DataMember]
        string Key { get; }
        /// <summary>
        /// Gets the value.
        /// </summary>
        /// <value>The value.</value>
        [DataMember]
        string Value { get; }
        /// <summary>
        /// Updates the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        [OperationContract]
        void Update(string key, string value);
        /// <summary>
        /// Deletes this instance.
        /// </summary>
        [OperationContract]
        void Delete();
        /// <summary>
        /// Occurs after an Attribute object has been Deleted.
        /// </summary>
        event AttributeDeletedDelegate<OWNER> AttributeDeleted;
        /// <summary>
        /// Occurs after an Attribute object has been Updated.
        /// </summary>
        event AttributeUpdatedDelegate<OWNER> AttributeUpdated;
    }
}
