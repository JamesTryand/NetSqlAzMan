using System.ServiceModel;
using NetSqlAzMan;

namespace NetSqlAzManWCFServiceTest
{
    // NOTE: If you change the interface name "INetSqlAzManWCFService" here, you must also update the reference to "INetSqlAzManWCFService" in Web.config.
    [ServiceContract(SessionMode = SessionMode.Required)]
    public interface INetSqlAzManWCFService
    {
        [OperationContract]
        SqlAzManStorage CreateStorageInstance(string connectionString);
    }
}
