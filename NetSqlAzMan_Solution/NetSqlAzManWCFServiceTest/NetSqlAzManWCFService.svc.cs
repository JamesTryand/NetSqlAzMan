using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan;

namespace NetSqlAzManWCFServiceTest
{
    // NOTE: If you change the class name "NetSqlAzManWCFService" here, you must also update the reference to "NetSqlAzManWCFService" in Web.config.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, UseSynchronizationContext=true)]
    public class NetSqlAzManWCFService : INetSqlAzManWCFService
    {
        public SqlAzManStorage CreateStorageInstance(string connectionString)
        {
            SqlAzManStorage result = new SqlAzManStorage(connectionString);
            return result;
        }
    }
}
