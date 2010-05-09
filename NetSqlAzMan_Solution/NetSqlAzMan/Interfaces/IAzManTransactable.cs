using System.Runtime.Serialization;
using System.ServiceModel;

namespace NetSqlAzMan.Interfaces
{
    /// <summary>
    /// Interfaces interface for all AzMan Transactable objects
    /// </summary>
    [ServiceContract(Namespace="http://NetSqlAzMan/ServiceModel", SessionMode=SessionMode.Required)]
    public interface IAzManTransactable
    {
        /// <summary>
        /// Begins the transaction.
        /// </summary>
        [OperationContract]
        void BeginTransaction();
        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <param name="isolationLevel">The isolation level.</param>
        [OperationContract(Name="BeginTransactionWithIsolationLevel")]
        void BeginTransaction(AzManIsolationLevel isolationLevel);
        /// <summary>
        /// Commits the transaction.
        /// </summary>
        [OperationContract]
        void CommitTransaction();
        /// <summary>
        /// Rollback the transaction.
        /// </summary>
        [OperationContract]
        void RollBackTransaction();
        /// <summary>
        /// Gets a value indicating whether [transaction in progress].
        /// </summary>
        /// <value>
        /// 	<c>true</c> if [transaction in progress]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        bool TransactionInProgress { get; }
    }
}
