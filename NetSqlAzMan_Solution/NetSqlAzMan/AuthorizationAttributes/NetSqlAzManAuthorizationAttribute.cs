using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;
using NetSqlAzMan.Interfaces;
using System.Reflection;
using System.Collections;
using NetSqlAzMan.Cache;

namespace NetSqlAzMan
{
    /// <summary>
    /// Authorization Attribute class.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false)]
    public partial class NetSqlAzManAuthorizationAttribute : System.Attribute
    {
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="NetSqlAzManAuthorizationAttribute"/> class.
        /// </summary>
        public NetSqlAzManAuthorizationAttribute(string itemName, string propertyName, object propertyValue)
        {
            this.ContextParameters = null;
            this.OperationsOnly = true;
            this.ValidFor = null;
            this.ItemName = itemName;
            this.PropertyName = propertyName;
            this.PropertyValue = propertyValue;
        }
        #endregion Constructors
        #region Properties
        /// <summary>
        /// Gets or sets the name of the item.
        /// </summary>
        /// <value>The name of the item.</value>
        public string ItemName { get; set; }
        /// <summary>
        /// Gets or sets the name of the property.
        /// </summary>
        /// <value>The name of the property.</value>
        public string PropertyName { get; set; }
        /// <summary>
        /// Gets or sets the property value.
        /// </summary>
        /// <value>The property value.</value>
        public object PropertyValue  { get; set; }
        /// <summary>
        /// Gets or sets the context parameters.
        /// </summary>
        /// <value>The context parameters.</value>
        public KeyValuePair<string, object>[] ContextParameters { get; set; }
        /// <summary>
        /// Gets or sets the valid for.
        /// </summary>
        /// <value>The valid for.</value>
        public DateTime? ValidFor { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [operations only].
        /// </summary>
        /// <value><c>true</c> if [operations only]; otherwise, <c>false</c>.</value>
        public bool OperationsOnly { get; set; }
        #endregion Properties
        #region Private Methods
        /// <summary>
        /// Determines whether the specified control name has access.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="controlName">Name of the control.</param>
        /// <param name="itemName">Name of the item.</param>
        /// <returns>
        /// 	<c>true</c> if the specified control name has access; otherwise, <c>false</c>.
        /// </returns>
        protected internal bool HasAccess(NetSqlAzManAuthorizationContext context, string controlName, string itemName)
        {
            if (!String.IsNullOrEmpty(context._storageConnectionString))
            {
                if (context.StorageCache != null)
                {
                    //Storage Cache
                    AuthorizationType auth = AuthorizationType.Neutral;
                    if (context._windowIdentity != null)
                        auth = context.StorageCache.CheckAccess(context.StoreName, context.ApplicationName, itemName, context._windowIdentity.GetUserBinarySSid(), context._windowIdentity.GetGroupsBinarySSid(), ValidFor.HasValue ? ValidFor.Value : DateTime.Now, OperationsOnly, ContextParameters);
                    else if (context._dbuserIdentity != null)
                        auth = context.StorageCache.CheckAccess(context.StoreName, context.ApplicationName, itemName, context._dbuserIdentity.CustomSid.StringValue, ValidFor.HasValue ? ValidFor.Value : DateTime.Now, OperationsOnly, ContextParameters);
                    return (auth == AuthorizationType.AllowWithDelegation) || (auth == AuthorizationType.Allow);
                    
                }
                else
                {
                    //Direct Access
                    using (SqlAzManStorage storage = new SqlAzManStorage(context._storageConnectionString))
                    {
                        AuthorizationType auth = AuthorizationType.Neutral;
                        if (context._windowIdentity != null)
                            auth = storage.CheckAccess(context.StoreName, context.ApplicationName, itemName, context._windowIdentity, ValidFor.HasValue ? ValidFor.Value : DateTime.Now, OperationsOnly, ContextParameters);
                        else if (context._dbuserIdentity != null)
                            auth = storage.CheckAccess(context.StoreName, context.ApplicationName, itemName, context._dbuserIdentity, ValidFor.HasValue ? ValidFor.Value : DateTime.Now, OperationsOnly, ContextParameters);
                        return (auth == AuthorizationType.AllowWithDelegation) || (auth == AuthorizationType.Allow);
                    }
                }
            }
            else
            {
                throw new InvalidOperationException("NetSqlAzMan Storage connection string and NetSqlAzMan WCF Cache Service url cannot be both null");
            }
        }
        #endregion Private Methods
    }
}
