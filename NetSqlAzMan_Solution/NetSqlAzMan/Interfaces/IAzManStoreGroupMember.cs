using System.Runtime.Serialization;
using System.ServiceModel;
using NetSqlAzMan.ENS;

namespace NetSqlAzMan.Interfaces
{
    /// <summary>
    /// Interfaces interface for all Store Group Members.
    /// </summary>
    [ServiceContract(Namespace="http://NetSqlAzMan/ServiceModel", SessionMode=SessionMode.Required)]
    public interface IAzManStoreGroupMember
    {
        /// <summary>
        /// Gets the store group member id.
        /// </summary>
        /// <value>The store group member id.</value>
        [DataMember]
        int StoreGroupMemberId { get; } //da implementare esplicitamente
        /// <summary>
        /// Gets the store group.
        /// </summary>
        /// <value>The store group.</value>
        [DataMember]
        IAzManStoreGroup StoreGroup { get; }
        /// <summary>
        /// Gets the object owner.
        /// </summary>
        /// <value>The object owner.</value>
        [DataMember]
        IAzManSid SID { get; }
        /// <summary>
        /// Gets where member is defined.
        /// </summary>
        /// <value>The where defined.</value>
        [DataMember]
        WhereDefined WhereDefined { get; }
        /// <summary>
        /// Gets a value indicating whether this instance is member.
        /// </summary>
        /// <value><c>true</c> if this instance is member; otherwise, <c>false</c>.</value>
        [DataMember]
        bool IsMember { get; }
        /// <summary>
        /// Deletes this instance.
        /// </summary>
        [OperationContract]
        void Delete();
        /// <summary>
        /// Gets the member info.
        /// </summary>
        /// <param name="displayName">Name of the display.</param>
        /// <returns></returns>
        [OperationContract]
        MemberType GetMemberInfo(out string displayName);
        /// <summary>
        /// Occurs after a SqlStoreGroupMember object has been Deleted.
        /// </summary>
        event StoreGroupMemberDeletedDelegate StoreGroupMemberDeleted;
    }
}
