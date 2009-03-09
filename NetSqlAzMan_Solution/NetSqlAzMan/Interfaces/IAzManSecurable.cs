using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace NetSqlAzMan.Interfaces
{
    /// <summary>
    /// Common interface for all NetSqlAzMan securable objects.
    /// </summary>
    [ServiceContract(Namespace="http://NetSqlAzMan/ServiceModel", SessionMode=SessionMode.Required)]
    public interface IAzManSecurable
    {
        /// <summary>
        /// Gets the managers.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        KeyValuePair<string, bool>[] GetManagers();
        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        KeyValuePair<string, bool>[] GetUsers();
        /// <summary>
        /// Gets the readers.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        KeyValuePair<string, bool>[] GetReaders();
        /// <summary>
        /// Grants the access as manager.
        /// </summary>
        /// <param name="sqlLogin">The SQL login.</param>
        [OperationContract]
        void GrantAccessAsManager(string sqlLogin);
        /// <summary>
        /// Grants the access as user.
        /// </summary>
        /// <param name="sqlLogin">The SQL login.</param>
        [OperationContract]
        void GrantAccessAsUser(string sqlLogin);
        /// <summary>
        /// Grants the access as reader.
        /// </summary>
        /// <param name="sqlLogin">The SQL login.</param>
        [OperationContract]
        void GrantAccessAsReader(string sqlLogin);
        /// <summary>
        /// Revokes the access as manager.
        /// </summary>
        /// <param name="sqlLogin">The SQL login.</param>
        [OperationContract]
        void RevokeAccessAsManager(string sqlLogin);
        /// <summary>
        /// Revokes the access as user.
        /// </summary>
        /// <param name="sqlLogin">The SQL login.</param>
        [OperationContract]
        void RevokeAccessAsUser(string sqlLogin);
        /// <summary>
        /// Revokes the access as reader.
        /// </summary>
        /// <param name="sqlLogin">The SQL login.</param>
        [OperationContract]
        void RevokeAccessAsReader(string sqlLogin);
        /// <summary>
        /// Gets a value indicating whether [I am admin].
        /// </summary>
        /// <value><c>true</c> if [I am admin]; otherwise, <c>false</c>.</value>
        [DataMember]
        bool IAmAdmin { get; }
        /// <summary>
        /// Gets a value indicating whether [I am manager].
        /// </summary>
        /// <value><c>true</c> if [I am manager]; otherwise, <c>false</c>.</value>
        [DataMember]
        bool IAmManager { get; }
        /// <summary>
        /// Gets a value indicating whether [I am user].
        /// </summary>
        /// <value><c>true</c> if [I am user]; otherwise, <c>false</c>.</value>
        [DataMember]
        bool IAmUser { get; }
        /// <summary>
        /// Gets a value indicating whether [I am reader].
        /// </summary>
        /// <value><c>true</c> if [I am reader]; otherwise, <c>false</c>.</value>
        [DataMember]
        bool IAmReader { get; }
    }
}
