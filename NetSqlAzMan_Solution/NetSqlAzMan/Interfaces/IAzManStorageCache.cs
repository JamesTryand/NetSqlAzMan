using System;
using NetSqlAzMan.Cache;

namespace NetSqlAzMan.Interfaces
{
    /// <summary>
    /// IAzManStorageCache Interface
    /// </summary>
    public interface IAzManStorageCache
    {
        /// <summary>
        /// Builds the storage cache.
        /// </summary>
        void BuildStorageCache();
        /// <summary>
        /// Builds the storage cache.
        /// </summary>
        /// <param name="storeNameFilter">The store name filter.</param>
        /// <param name="applicationNameFilter">The application name filter.</param>
        void BuildStorageCache(string storeNameFilter, string applicationNameFilter);
        /// <summary>
        /// Builds the storage cache.
        /// </summary>
        /// <param name="storeNameFilter">The store name filter.</param>
        void BuildStorageCache(string storeNameFilter);
        /// <summary>
        /// Checks the access.
        /// </summary>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="applicationName">Name of the application.</param>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="userSSid">The user S sid.</param>
        /// <param name="groupsSSid">The groups S sid.</param>
        /// <param name="validFor">The valid for.</param>
        /// <param name="operationsOnly">if set to <c>true</c> [operations only].</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns></returns>
        AuthorizationType CheckAccess(string storeName, string applicationName, string itemName, string userSSid, string[] groupsSSid, DateTime validFor, bool operationsOnly, params System.Collections.Generic.KeyValuePair<string, object>[] contextParameters);
        /// <summary>
        /// Checks the access.
        /// </summary>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="applicationName">Name of the application.</param>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="DBuserSSid">The D buser S sid.</param>
        /// <param name="validFor">The valid for.</param>
        /// <param name="operationsOnly">if set to <c>true</c> [operations only].</param>
        /// <param name="attributes">The attributes.</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns></returns>
        AuthorizationType CheckAccess(string storeName, string applicationName, string itemName, string DBuserSSid, DateTime validFor, bool operationsOnly, out System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>> attributes, params System.Collections.Generic.KeyValuePair<string, object>[] contextParameters);
        /// <summary>
        /// Checks the access.
        /// </summary>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="applicationName">Name of the application.</param>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="DBuserSSid">The D buser S sid.</param>
        /// <param name="validFor">The valid for.</param>
        /// <param name="operationsOnly">if set to <c>true</c> [operations only].</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns></returns>
        AuthorizationType CheckAccess(string storeName, string applicationName, string itemName, string DBuserSSid, DateTime validFor, bool operationsOnly, params System.Collections.Generic.KeyValuePair<string, object>[] contextParameters);
        /// <summary>
        /// Checks the access.
        /// </summary>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="applicationName">Name of the application.</param>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="userSSid">The user S sid.</param>
        /// <param name="groupsSSid">The groups S sid.</param>
        /// <param name="validFor">The valid for.</param>
        /// <param name="operationsOnly">if set to <c>true</c> [operations only].</param>
        /// <param name="attributes">The attributes.</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns></returns>
        AuthorizationType CheckAccess(string storeName, string applicationName, string itemName, string userSSid, string[] groupsSSid, DateTime validFor, bool operationsOnly, out System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>> attributes, params System.Collections.Generic.KeyValuePair<string, object>[] contextParameters);
        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value>The connection string.</value>
        string ConnectionString { get; set; }
        /// <summary>
        /// Gets the authorized items.
        /// </summary>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="applicationName">Name of the application.</param>
        /// <param name="DBuserSSid">The D buser S sid.</param>
        /// <param name="validFor">The valid for.</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns></returns>
        AuthorizedItem[] GetAuthorizedItems(string storeName, string applicationName, string DBuserSSid, DateTime validFor, params System.Collections.Generic.KeyValuePair<string, object>[] contextParameters);
        /// <summary>
        /// Gets the authorized items.
        /// </summary>
        /// <param name="storeName">Name of the store.</param>
        /// <param name="applicationName">Name of the application.</param>
        /// <param name="userSSid">The user S sid.</param>
        /// <param name="groupsSSid">The groups S sid.</param>
        /// <param name="validFor">The valid for.</param>
        /// <param name="contextParameters">The context parameters.</param>
        /// <returns></returns>
        AuthorizedItem[] GetAuthorizedItems(string storeName, string applicationName, string userSSid, string[] groupsSSid, DateTime validFor, params System.Collections.Generic.KeyValuePair<string, object>[] contextParameters);
        /// <summary>
        /// Gets or sets the storage.
        /// </summary>
        /// <value>The storage.</value>
        SqlAzManStorage Storage { get; set; }
    }
}
