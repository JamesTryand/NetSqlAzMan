using System.Collections.Generic;
using System.DirectoryServices;
using System.Runtime.Serialization;
using System.Security.Principal;
using System.ServiceModel;
using NetSqlAzMan.ENS;

namespace NetSqlAzMan.Interfaces
{
    /// <summary>
    /// Interfaces interface for all Application Groups.
    /// </summary>
    [ServiceContract(Namespace="http://NetSqlAzMan/ServiceModel", SessionMode=SessionMode.Required)]
    public interface IAzManApplicationGroup : IAzManExport, IAzManImport 
    {
        /// <summary>
        /// Gets the application group id.
        /// </summary>
        /// <value>The application group id.</value>
        [DataMember]
        int ApplicationGroupId { get; } //da implementare esplicitamente
        /// <summary>
        /// Gets the application.
        /// </summary>
        /// <value>The application.</value>
        [DataMember]
        IAzManApplication Application { get; }
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
        /// <param name="description">The description.</param>
        /// <param name="groupType">Type of the group.</param>
        [OperationContract]
        void Update(string description, GroupType groupType);
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
        /// Creates the application group member.
        /// </summary>
        /// <param name="sid">The object owner.</param>
        /// <param name="whereDefined">Where member is defined.</param>
        /// <param name="isMember">if set to <c>true</c> [is member].</param>
        /// <returns></returns>
        [OperationContract]
        IAzManApplicationGroupMember CreateApplicationGroupMember(IAzManSid sid, WhereDefined whereDefined, bool isMember);
        /// <summary>
        /// Gets the application group members.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IAzManApplicationGroupMember[] GetApplicationGroupMembers();
        /// <summary>
        /// Gets the members.
        /// </summary>
        /// <value>The members.</value>
        [DataMember]
        Dictionary<IAzManSid, IAzManApplicationGroupMember> Members { get; }
        /// <summary>
        /// Gets the application group non members.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IAzManApplicationGroupMember[] GetApplicationGroupNonMembers();
        /// <summary>
        /// Gets the A application group all members.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        IAzManApplicationGroupMember[] GetApplicationGroupAllMembers();
        /// <summary>
        /// Gets the application group member.
        /// </summary>
        /// <param name="sid">The object owner.</param>
        /// <returns></returns>
        [OperationContract]
        IAzManApplicationGroupMember GetApplicationGroupMember(IAzManSid sid);
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
        /// Occurs after a SqlAzManApplicationGroup object has been Deleted.
        /// </summary>
        event ApplicationGroupDeletedDelegate ApplicationGroupDeleted;
        /// <summary>
        /// Occurs after a SqlAzManApplicationGroup LDAPQuery has been Updated.
        /// </summary>
        event ApplicationGroupLDAPQueryUpdatedDelegate ApplicationGroupLDAPQueryUpdated;
        /// <summary>
        /// Occurs after a SqlAzManApplicationGroup object has been Updated.
        /// </summary>
        event ApplicationGroupUpdatedDelegate ApplicationGroupUpdated;
        /// <summary>
        /// Occurs after a SqlAzManApplicationGroup object has been Renamed.
        /// </summary>
        event ApplicationGroupRenamedDelegate ApplicationGroupRenamed;
        /// <summary>
        /// Occurs after an ApplicationGroupMember object has been Created.
        /// </summary>
        event ApplicationGroupMemberCreatedDelegate ApplicationGroupMemberCreated;
        
    }
}
