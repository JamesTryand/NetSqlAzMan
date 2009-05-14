using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using NetSqlAzMan;
using NetSqlAzMan.Interfaces;
using System.Security.Principal;

namespace NetSqlAzMan.Cache.Service
{
    // NOTE: If you change the interface name "ICacheService" here, you must also update the reference to "ICacheService" in App.config.
    [ServiceContract]
    public interface ICacheService
    {
        [OperationContract(Name = "CheckAccessForWindowsUsersWithAttributesRetrieve")]
        AuthorizationType CheckAccess(string storeName, string applicationName, string itemName, string userSSid, string[] groupsSSid, DateTime validFor, bool operationsOnly, out List<KeyValuePair<string, string>> attributes, params KeyValuePair<string, object>[] contextParameters);
        [OperationContract(Name = "CheckAccessForWindowsUsersWithoutAttributesRetrieve")]
        AuthorizationType CheckAccess(string storeName, string applicationName, string itemName, string userSSid, string[] groupsSSid, DateTime validFor, bool operationsOnly, params KeyValuePair<string, object>[] contextParameters);
        [OperationContract(Name = "CheckAccessForDatabaseUsersWithAttributesRetrieve")]
        AuthorizationType CheckAccess(string storeName, string applicationName, string itemName, string DBuserSSid, DateTime validFor, bool operationsOnly, out List<KeyValuePair<string, string>> attributes, params KeyValuePair<string, object>[] contextParameters);
        [OperationContract(Name = "CheckAccessForDatabaseUsersWithoutAttributesRetrieve")]
        AuthorizationType CheckAccess(string storeName, string applicationName, string itemName, string DBuserSSid, DateTime validFor, bool operationsOnly, params KeyValuePair<string, object>[] contextParameters);
        [OperationContract(Name = "InvalidateCache")]
        void InvalidateCache();
        [OperationContract(Name = "InvalidateCacheOnServicePartners")]
        void InvalidateCache(bool invalidateCacheOnServicePartners);
        //[OperationContract(Name = "InvalidateStoreCache")]
        void InvalidateStoreCache(string storeName);
        //[OperationContract(Name = "InvalidateStoreApplicationCache")]
        void InvalidateStoreApplicationCache(string storeName, string applicationName);
        [OperationContract()]
        string[] GetItemNames(string storeName, string applicationName, ItemType type);
        [OperationContract()]
        KeyValuePair<string, ItemType>[] GetAllItems(string storeName, string applicationName);
    }
}
