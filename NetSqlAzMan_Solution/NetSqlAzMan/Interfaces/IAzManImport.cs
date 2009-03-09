using System;
using System.Xml;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace NetSqlAzMan.Interfaces
{
    /// <summary>
    /// Common interface for all NetSqlAzMan importable/exportable objects
    /// </summary>
    [ServiceContract(Namespace="http://NetSqlAzMan/ServiceModel", SessionMode=SessionMode.Required)]
    [XmlSerializerFormat]
    public interface IAzManImport
    {
        /// <summary>
        /// Imports the specified XML reader.
        /// </summary>
        /// <param name="xmlNode">The XML node.</param>
        /// <param name="includeWindowsUsersAndGroups">if set to <c>true</c> [include windows users and groups].</param>
        /// <param name="includeDBUsers">if set to <c>true</c> [include DB users].</param>
        /// <param name="includeAuthorizations">if set to <c>true</c> [include authorizations].</param>
        /// <param name="mergeOptions">The merge options.</param>
        [OperationContract]
        void ImportChildren(XmlNode xmlNode, bool includeWindowsUsersAndGroups, bool includeDBUsers, bool includeAuthorizations, SqlAzManMergeOptions mergeOptions);
    }
}
