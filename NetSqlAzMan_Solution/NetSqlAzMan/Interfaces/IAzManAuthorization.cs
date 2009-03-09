using System;
using System.Security.Principal;
using NetSqlAzMan.ENS;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace NetSqlAzMan.Interfaces
{
    /// <summary>
    /// Interfaces interface for all AzManAuthorizations
    /// </summary>
    [ServiceContract(Namespace="http://NetSqlAzMan/ServiceModel", SessionMode=SessionMode.Required)]
    public interface IAzManAuthorization : IAzManExport, IAzManImport, IDisposable
    {
        /// <summary>
        /// Gets the authorization id.
        /// </summary>
        /// <value>The authorization id.</value>
        [DataMember]
        int AuthorizationId { get; } //da implementare esplicitamente
        /// <summary>
        /// Gets the itemName.
        /// </summary>
        /// <value>The itemName.</value>
        [DataMember]
        IAzManItem Item { get; }
        /// <summary>
        /// Gets the Owner.
        /// </summary>
        /// <value>The Owner object owner.</value>
        [DataMember]
        IAzManSid Owner { get; }
        /// <summary>
        /// Gets the where is defined the Owner.
        /// </summary>
        /// <value>The object owner where defined.</value>
        [DataMember]
        WhereDefined OwnerSidWhereDefined { get; }
        /// <summary>
        /// Gets the object owner.
        /// </summary>
        /// <value>The object owner.</value>
        [DataMember]
        IAzManSid SID { get; }
        /// <summary>
        /// Gets the object owner where defined.
        /// </summary>
        /// <value>The object owner where defined.</value>
        [DataMember]
        WhereDefined SidWhereDefined { get; }
        /// <summary>
        /// Gets the type of the authorization.
        /// </summary>
        /// <value>The type of the authorization.</value>
        [DataMember]
        AuthorizationType AuthorizationType { get; }
        /// <summary>
        /// Gets the valid from.
        /// </summary>
        /// <value>The valid from.</value>
        [DataMember]
        Nullable<DateTime> ValidFrom { get; }
        /// <summary>
        /// Gets the valid to.
        /// </summary>
        /// <value>The valid to.</value>
        [DataMember]
        Nullable<DateTime> ValidTo { get; }
        /// <summary>
        /// Updates the specified authorization type.
        /// </summary>
        /// <param name="Owner">The owner.</param>
        /// <param name="sid">The sid.</param>
        /// <param name="sidWhereDefined">The sid where defined.</param>
        /// <param name="authorizationType">Type of the authorization.</param>
        /// <param name="validFrom">The valid from.</param>
        /// <param name="validTo">The valid to.</param>
        [OperationContract]
        void Update(IAzManSid Owner, IAzManSid sid, WhereDefined sidWhereDefined, AuthorizationType authorizationType, Nullable<DateTime> validFrom, Nullable<DateTime> validTo);
        /// <summary>
        /// Deletes this instance.
        /// </summary>
        [OperationContract]
        void Delete();
        /// <summary>
        /// Creates an attribute.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        [OperationContract]
        IAzManAttribute<IAzManAuthorization> CreateAttribute(string key, string value);
        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IAzManAttribute<IAzManAuthorization>[] GetAttributes();
        /// <summary>
        /// Gets the attributes.
        /// </summary>
        /// <value>The attributes.</value>
        [DataMember]
        Dictionary<string, IAzManAttribute<IAzManAuthorization>> Attributes { get; }
        /// <summary>
        /// Gets the attribute.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        [OperationContract]
        IAzManAttribute<IAzManAuthorization> GetAttribute(string key);
        /// <summary>
        /// Occurs after an Attribute object has been Created.
        /// </summary>
        event AttributeCreatedDelegate<IAzManAuthorization> AuthorizationAttributeCreated;
        /// <summary>
        /// Gets the member info.
        /// </summary>
        /// <param name="displayName">Display Name of the member.</param>
        /// <returns></returns>
        [OperationContract]
        MemberType GetMemberInfo(out string displayName);
        /// <summary>
        /// Gets the owner info.
        /// </summary>
        /// <param name="displayName">Display Name of the owner.</param>
        /// <returns></returns>
        [OperationContract]
        MemberType GetOwnerInfo(out string displayName);
        /// <summary>
        /// Occurs after a SqlAzManAuthorization object has been Deleted.
        /// </summary>
        event AuthorizationDeletedDelegate AuthorizationDeleted;
        /// <summary>
        /// Occurs after a SqlAzManAuthorization object has been Updated.
        /// </summary>
        event AuthorizationUpdatedDelegate AuthorizationUpdated;
    }
}
