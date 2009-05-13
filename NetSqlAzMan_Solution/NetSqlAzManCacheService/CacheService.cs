using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using NetSqlAzMan;
using NetSqlAzMan.Interfaces;
using System.Security.Principal;
using System.Configuration;

namespace NetSqlAzMan.Cache.Service
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.Single)]
    public sealed class CacheService : ICacheService
    {
        internal static StorageCache storageCache;
        internal volatile static bool buildingCache;
        public CacheService()
        {
            CacheService.buildingCache = false;
        }

        internal static void startStorageBuildCache()
        {
            CacheService.startStorageBuildCache(ConfigurationManager.AppSettings["StoreNameFilter"], ConfigurationManager.AppSettings["ApplicationNameFilter"]);
        }
        
        internal static void startStorageBuildCache(string storeName)
        {
            CacheService.startStorageBuildCache(storeName, String.Empty);
        }

        internal static void startStorageBuildCache(string storeName, string applicationName)
        {
            //Design Feature 1: If Not already building cache ... ignore new InvalidateCache
            if (!CacheService.buildingCache)
            {
                CacheService.buildingCache = true;
                WindowsCacheService.writeEvent(String.Format("Invalidate Cache invoked from user '{0}'. Store: '{1}' - Application: '{2}'.", ((System.Threading.Thread.CurrentPrincipal.Identity as WindowsIdentity) ?? WindowsIdentity.GetCurrent()).Name, storeName ?? String.Empty, applicationName ?? String.Empty), System.Diagnostics.EventLogEntryType.Information);

                //Design Feature 2: Async Cache Building
                System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(o =>
                    {
                        try
                        {
                            StorageCache sc = new StorageCache(Properties.Settings.Default.NetSqlAzManStorageCacheConnectionString);
                            sc.BuildStorageCache(storeName, applicationName);
                            if (CacheService.storageCache != null)
                            {
                                //Design Feature 3: When Build ... replace the old cache with new one
                                //3.1) This means that while building ... User can invoke CheckAccess on the OLD cache
                                //3.2) Replacement is thread safe
                                lock (CacheService.storageCache)
                                {
                                    CacheService.storageCache = sc;
                                }
                            }
                            else
                            {
                                CacheService.storageCache = sc;
                            }
                            WindowsCacheService.writeEvent("Cache Built.", System.Diagnostics.EventLogEntryType.Information);
                        }
                        catch (Exception ex)
                        {
                            WindowsCacheService.writeEvent(String.Format("Cache building error:\r\n{0}\r\n\r\nStack Track:\r\n{1}", ex.Message, ex.StackTrace), System.Diagnostics.EventLogEntryType.Error);
                        }
                        finally
                        {
                            CacheService.buildingCache = false;
                        }
                    }
                        ));
            }
            else
            {
                WindowsCacheService.writeEvent("Invalidate Cache invoked while building cache. Command ignored.", System.Diagnostics.EventLogEntryType.Warning);
            }
        }
        #region ICacheService Members

        public void InvalidateCache()
        {
            Debug.WriteLine("InvalidateCache called.");
            CacheService.startStorageBuildCache();
        }

        public void InvalidateStoreCache(string storeName)
        {
            Debug.WriteLine(String.Format("InvalidateStoreCache called for Store '{0}'.", storeName));
            CacheService.startStorageBuildCache(storeName);
        }

        public void InvalidateStoreApplicationCache(string storeName, string applicationName)
        {
            Debug.WriteLine(String.Format("InvalidateStoreApplicationCache called for Store '{0}' - Application '{1}'.", storeName, applicationName));
            CacheService.startStorageBuildCache(storeName, applicationName);
        }

        public AuthorizationType CheckAccess(string storeName, string applicationName, string itemName, string userSSid, string[] groupsSSid, DateTime validFor, bool operationsOnly, out List<KeyValuePair<string, string>> attributes, params KeyValuePair<string, object>[] contextParameters)
        {
            //Debug.WriteLine("CheckAccessForWindowsUsersWithAttributesRetrieve called.");
            //WindowsCacheService.writeEvent(String.Format("CheckAccessForWindowsUsersWithAttributesRetrieve called for\r\nUser:\r\n{0}\r\nGroups:{1}", userSSid, String.Join("\r\n", groupsSSid)), EventLogEntryType.Information);
            return CacheService.storageCache.CheckAccess(storeName, applicationName, itemName, userSSid, groupsSSid, validFor, operationsOnly, out attributes, contextParameters);
        }

        public AuthorizationType CheckAccess(string storeName, string applicationName, string itemName, string userSSid, string[] groupsSSid, DateTime validFor, bool operationsOnly, params KeyValuePair<string, object>[] contextParameters)
        {
            //Debug.WriteLine("CheckAccessForWindowsUsersWithoutAttributesRetrieve called.");
            //WindowsCacheService.writeEvent(String.Format("CheckAccessForWindowsUsersWithoutAttributesRetrieve called for\r\nUser:\r\n{0}\r\nGroups:{1}", userSSid, String.Join("\r\n", groupsSSid)), EventLogEntryType.Information);
            return CacheService.storageCache.CheckAccess(storeName, applicationName, itemName, userSSid, groupsSSid, validFor, operationsOnly, contextParameters);
        }

        public AuthorizationType CheckAccess(string storeName, string applicationName, string itemName, string DBuserSSid, DateTime validFor, bool operationsOnly, out List<KeyValuePair<string, string>> attributes, params KeyValuePair<string, object>[] contextParameters)
        {
            //Debug.WriteLine("CheckAccessForDatabaseUsersWithAttributesRetrieve called.");
            return CacheService.storageCache.CheckAccess(storeName, applicationName, itemName, DBuserSSid, validFor, operationsOnly, out attributes, contextParameters);
        }

        public AuthorizationType CheckAccess(string storeName, string applicationName, string itemName, string DBuserSSid, DateTime validFor, bool operationsOnly, params KeyValuePair<string, object>[] contextParameters)
        {
            //Debug.WriteLine("CheckAccessForDatabaseUsersWithoutAttributesRetrieve called.");
            return CacheService.storageCache.CheckAccess(storeName, applicationName, itemName, DBuserSSid, validFor, operationsOnly, contextParameters);
        }

        public string[] GetItemNames(string storeName, string applicationName, ItemType type)
        { 
            List<string> items = new List<string>();
            foreach (var item in CacheService.storageCache.Storage[storeName][applicationName].Items)
            {
                items.Add(item.Value.Name);
            }
            return items.ToArray();
        }

        public KeyValuePair<string, ItemType>[] GetAllItems(string storeName, string applicationName)
        {
            List<KeyValuePair<string, ItemType>> items = new List<KeyValuePair<string, ItemType>>();
            foreach (var item in CacheService.storageCache.Storage[storeName][applicationName].Items)
            {
                items.Add(new KeyValuePair<string, ItemType>(item.Value.Name, item.Value.ItemType));
            }
            return items.ToArray();
        }
        #endregion
    }
}
