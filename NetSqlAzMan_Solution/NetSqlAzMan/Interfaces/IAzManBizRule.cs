using System;
using System.Security.Principal;
using System.IO;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace NetSqlAzMan.Interfaces
{
    /// <summary>
    /// Public Interface for All NetSqlAzMan Business Rules
    /// </summary>
    [ServiceContract(Namespace="http://NetSqlAzMan/ServiceModel", SessionMode=SessionMode.Required)]
    public interface IAzManBizRule
    {
        /// <summary>
        /// Executes the specified Business Rule.
        /// </summary>
        /// <param name="contextParameters">The context parameters.</param>
        /// <param name="identity">The identity.</param>
        /// <param name="ownerItem">The owner itemName.</param>
        /// <param name="ForcedCheckAccessResult">The ForcedCheckAccessResult property sets a value that indicates whether the Business Rule (BizRule) forces CheckAccess result to some value.</param>
        /// <returns>True or False</returns>
        [OperationContract]
        bool Execute(Hashtable contextParameters, IAzManSid identity, IAzManItem ownerItem, ref AuthorizationType ForcedCheckAccessResult);
    }
}
