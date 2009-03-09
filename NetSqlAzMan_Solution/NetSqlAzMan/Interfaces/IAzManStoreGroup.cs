using System;
using System.Security.Principal;
using System.DirectoryServices;
using System.Collections.Generic;
using System.Text;
using NetSqlAzMan.ENS;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace NetSqlAzMan.Interfaces
{
    /// <summary>
    /// Interfaces interface for all Store Groups.
    /// </summary>
    [ServiceContract(Namespace="http://NetSqlAzMan/ServiceModel", SessionMode=SessionMode.Required)]
    public interface IAzManStoreGroup : IAzManExport, IAzManImport
    {
        /// <summary>
        /// Gets the store group id.
        /// </summary>
        /// <value>The store group id.</value>
        [DataMember]
        int StoreGroupId { get; } //da implementare esplicitamente
        /// <summary>
        /// Gets the store.
        /// </summary>
        /// <value>The store.</value>
        [DataMember]
        IAzManStore Store { get; }
        /// <summary>
        /// Gets the object owner.
        /// </summary>
        /// <value>The object owner.</value>
        [DataMember]
        IAzManSid SID { get; }
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>The name.</value>
        [DataMember]
        string Name { get; }
        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
        [DataMember]
        string Description { get; }
        /// <summary>
        /// Gets the LDAP query.
        /// </summary>
        /// <value>The LDAP query.</value>
        [DataMember]
        string LDAPQuery { get; }
        /// <summary>
        /// Gets the type of the group.
        /// </summary>
        /// <value>The type of the group.</value>
        [DataMember]
        GroupType GroupType { get; }
        /// <summary>
        /// Updates the specified object owner.
        /// </summary>
        /// <param name="sid">The object owner.</param>
        /// <param name="description">The description.</param>
        /// <param name="groupType">Type of the group.</param>
        [OperationContract]
        void Update(IAzManSid sid, string description, GroupType groupType);
        /// <summary>
        /// Updates the L dap query.
        /// </summary>
        /// <param name="newLdapQuery">The new ldap query.</param>
        [OperationContract]
        void UpdateLDapQuery(string newLdapQuery);
        /// <summary>
        /// Renames the specified new name.
        /// </summary>
        /// <param name="newName">The new name.</param>
        [OperationContract]
        void Rename(string newName);
        /// <summary>
        /// Deletes this instance.
        /// </summary>
        [OperationContract]
        void Delete();
        /// <summary>
        /// Creates the store group member.
        /// </summary>
        /// <param name="sid">The object owner.</param>
        /// <param name="whereDefined">Where member is defined.</param>
        /// <param name="isMember">if set to <c>true</c> [is member].</param>
        /// <returns></returns>
        [OperationContract]
        IAzManStoreGroupMember CreateStoreGroupMember(IAzManSid sid, WhereDefined whereDefined, bool isMember);
        /// <summary>
        /// Gets the store group members.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IAzManStoreGroupMember[] GetStoreGroupMembers();
        /// <summary>
        /// Gets the store group non members.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IAzManStoreGroupMember[] GetStoreGroupNonMembers();
        /// <summary>
        /// Gets the store group all members.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IAzManStoreGroupMember[] GetStoreGroupAllMembers();
        /// <summary>
        /// Gets the members.
        /// </summary>
        /// <value>The members.</value>
        [DataMember]
        Dictionary<IAzManSid, IAzManStoreGroupMember> Members { get; }
        /// <summary>
        /// Determines whether [is in group] [the specified windows identity].
        /// </summary>
        /// <param name="windowsIdentity">The windows identity.</param>
        /// <returns>
        /// 	<c>true</c> if [is in group] [the specified windows identity]; otherwise, <c>false</c>.
        /// </returns>
        [OperationContract]
        bool IsInGroup(WindowsIdentity windowsIdentity);
        /// <summary>
        /// Determines whether [is in group] [the specified windows identity].
        /// </summary>
        /// <param name="dbUser">The db user.</param>
        /// <returns>
        /// 	<c>true</c> if [is in group] [the specified windows identity]; otherwise, <c>false</c>.
        /// </returns>
        [OperationContract]
        bool IsInGroup(IAzManDBUser dbUser);
        /// <summary>
        /// Gets the store group member.
        /// </summary>
        /// <param name="sid">The object owner.</param>
        /// <returns></returns>
        [OperationContract]
        IAzManStoreGroupMember GetStoreGroupMember(IAzManSid sid);
        /// <summary>
        /// Executes the LDAP query.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        SearchResultCollection ExecuteLDAPQuery();
        /// <summary>
        /// Executes the LDAP query.
        /// </summary>
        /// <param name="testLDapQuery">The test LDap query.</param>
        /// <returns></returns>
        [OperationContract]
        SearchResultCollection ExecuteLDAPQuery(string testLDapQuery);
        /// <summary>
        /// Occurs after a SqlAzManStoreGroup object has been Deleted.
        /// </summary>
        event StoreGroupDeletedDelegate StoreGroupDeleted;
        /// <summary>
        /// Occurs after a SqlAzManStoreGroup LDAPQuery has been Updated.
        /// </summary>
        event StoreGroupLDAPQueryUpdatedDelegate StoreGroupLDAPQueryUpdated;
        /// <summary>
        /// Occurs after a SqlAzManStoreGroup object has been Updated.
        /// </summary>
        event StoreGroupUpdatedDelegate StoreGroupUpdated;
        /// <summary>
        /// Occurs after a SqlAzManStoreGroup object has been Renamed.
        /// </summary>
        event StoreGroupRenamedDelegate StoreGroupRenamed;
        /// <summary>
        /// Occurs after an StoreGroupMember object has been Created.
        /// </summary>
        event StoreGroupMemberCreatedDelegate StoreGroupMemberCreated;
    }
}
