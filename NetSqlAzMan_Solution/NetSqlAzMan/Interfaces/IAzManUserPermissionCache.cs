using System;
using System.Collections.Generic;

namespace NetSqlAzMan.Interfaces
{
    /// <summary>
    /// IAzManUserPermissionCache Interface.
    /// </summary>
    public interface IAzManUserPermissionCache
    {
        /// <summary>
        /// Checks the access.
        /// </summary>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="validFor">The valid for.</param>
        /// <param name="attributes">The attributes.</param>
        /// <returns></returns>
        AuthorizationType CheckAccess(string itemName, DateTime validFor, out System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>> attributes);
        /// <summary>
        /// Checks the access.
        /// </summary>
        /// <param name="itemName">Name of the item.</param>
        /// <param name="validFor">The valid for.</param>
        /// <returns></returns>
        AuthorizationType CheckAccess(string itemName, DateTime validFor);
        /// <summary>
        /// Gets the item attributes.
        /// </summary>
        /// <value>The item attributes.</value>
        Dictionary<string, List<KeyValuePair<string, string>>> ItemAttributes { get; }
        /// <summary>
        /// Gets the items.
        /// </summary>
        /// <value>The items.</value>
        string[] Items { get; }
    }
}
