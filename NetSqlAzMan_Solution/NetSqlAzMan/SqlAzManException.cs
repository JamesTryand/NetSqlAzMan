using System;
using System.Runtime.Serialization;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzMan
{
    /// <summary>
    /// SqlAzMan Exception base class
    /// </summary>
    /// <remarks>
    /// Design Guidelines for Exceptions: http://msdn.microsoft.com/en-us/library/ms229014(VS.80).aspx
    /// For further details Check Data property.
    /// </remarks>
    [Serializable()]
    public class SqlAzManException : System.Exception, ISerializable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlAzManException"/> class.
        /// </summary>
        public SqlAzManException()
            : base()
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlAzManException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public SqlAzManException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlAzManException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public SqlAzManException(string message, Exception innerException)
            : base(message, innerException)
        { }

        // This constructor is needed for serialization.
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlAzManException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo"/> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext"/> that contains contextual information about the source or destination.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// The <paramref name="info"/> parameter is null.
        /// </exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">
        /// The class name is null or <see cref="P:System.Exception.HResult"/> is zero (0).
        /// </exception>
        protected SqlAzManException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            
        }
        #region static members 
        internal static SqlAzManException GenericException(Exception innerException)
        {
            return new SqlAzManException("NetSqlAzMan Generic Error", innerException);
        }
        internal static SqlAzManException GenericException(string message)
        {
            return new SqlAzManException(message);
        }
        internal static SqlAzManException GenericException(string message, Exception innerException)
        {
            return new SqlAzManException(message, innerException);
        }
        internal static SqlAzManException StoreNotFoundException(string storeName, Exception innerException)
        {
            SqlAzManException ex =  new SqlAzManException(String.Format("Store '{0}' not found.", storeName), innerException);
            addParameter(ex, "Store name", storeName);
            return ex;
        }
        internal static SqlAzManException StoreDuplicateException(string storeName, Exception innerException)
        {
            SqlAzManException ex =  new SqlAzManException(String.Format("A Store with the same name already exists: '{0}'.", storeName), innerException);
            addParameter(ex, "Store name", storeName);
            return ex;
        }
        internal static SqlAzManException StoreGroupNotFoundException(string group, IAzManStore store, Exception innerException)
        {
            SqlAzManException ex = new SqlAzManException(String.Format("Store Group '{0}' not found. Store '{1}'.", group, store.Name), innerException);
            addParameter(ex, "Store Group", group);
            addParameter(ex, store);
            return ex;
        }
        internal static SqlAzManException StoreGroupDuplicateException(string storeGroupName, IAzManStore store, Exception innerException)
        {
            SqlAzManException ex =  new SqlAzManException(String.Format("A Store Group with the same name already exists: '{0}'. Store '{1}'.", storeGroupName, store.Name), innerException);
            addParameter(ex, "Store Group name", storeGroupName);
            addParameter(ex, store);
            return ex;
        }
        internal static SqlAzManException ApplicationNotFoundException(string applicationName, IAzManStore store, Exception innerException)
        {
            SqlAzManException ex =  new SqlAzManException(String.Format("Application '{0}' not found. Store '{1}'.", applicationName, store.Name), innerException);
            addParameter(ex, "Application name", applicationName);
            addParameter(ex, store);
            return ex;
        }
        internal static SqlAzManException ApplicationNotFoundException(string applicationName, string storeName, Exception innerException)
        {
            SqlAzManException ex =  new SqlAzManException(String.Format("Application '{0}' not found. Store '{1}'.", applicationName, storeName), innerException);
            addParameter(ex, "Application name", applicationName);
            addParameter(ex, "Store name", storeName);
            return ex;
        }
        internal static SqlAzManException ApplicationDuplicateException(string applicationName, IAzManStore store, Exception innerException)
        {
            SqlAzManException ex =  new SqlAzManException(String.Format("An Application with the same name already exists: '{0}'. Store '{1}'.", applicationName, store.Name), innerException);
            addParameter(ex, "Application name", applicationName);
            addParameter(ex, store);
            return ex;
        }
        internal static SqlAzManException ApplicationGroupNotFoundException(string group, IAzManApplication application, Exception innerException)
        {
            SqlAzManException ex = new SqlAzManException(String.Format("Application Group '{0}' not found. Store '{1}', Application '{2}'.", group, application.Store.Name, application.Name), innerException);
            addParameter(ex, "Application Group", group);
            addParameter(ex, application);
            return ex;
        }
        internal static SqlAzManException ApplicationGroupMemberNotFoundException(string memberName, IAzManApplicationGroup applicationGroup, Exception innerException)
        {
            SqlAzManException ex = new SqlAzManException(String.Format("Application Group Member '{0}' not found. Store '{1}', Application '{2}', Application Group '{3}'.", memberName, applicationGroup.Application.Store.Name, applicationGroup.Application.Name, applicationGroup.Name), innerException);
            addParameter(ex, "Store name", applicationGroup.Application.Store.Name);
            addParameter(ex, "Application name", applicationGroup.Application.Name);
            addParameter(ex, "Application Group name", applicationGroup.Name);
            addParameter(ex, "Member name", memberName);
            return ex;
        }
        internal static SqlAzManException StoreGroupMemberNotFoundException(string memberName, IAzManStoreGroup storeGroup, Exception innerException)
        {
            SqlAzManException ex = new SqlAzManException(String.Format("Store Group Member '{0}' not found. Store '{1}', Store Group '{2}'.", memberName, storeGroup.Store.Name, storeGroup.Name), innerException);
            addParameter(ex, "Store name", storeGroup.Store.Name);
            addParameter(ex, "Store Group name", storeGroup.Name);
            addParameter(ex, "Member name", memberName);
            return ex;
        }
        internal static SqlAzManException ApplicationGroupDuplicateException(string applicationGroupName, IAzManApplication application, Exception innerException)
        {
            SqlAzManException ex =  new SqlAzManException(String.Format("An Application Group with the same name already exists: '{0}'. Store '{1}', Application '{2}'.", applicationGroupName, application.Store.Name, application.Name), innerException);
            addParameter(ex, "Application Group name", applicationGroupName);
            addParameter(ex, application);
            return ex;
        }
        internal static SqlAzManException AttributeNotFoundException(string key, IAzManAuthorization authorization, Exception innerException)
        {
            SqlAzManException ex = new SqlAzManException(String.Format("Attribute '{0}' not found. Store '{1}', Application '{2}', Item '{3}', Authorization SID '{4}'.",
                key, authorization.Item.Application.Store.Name, authorization.Item.Application.Name, authorization.Item.Name, authorization.SID.StringValue), innerException);
            addParameter(ex, "Store name", authorization.Item.Application.Store.Name);
            addParameter(ex, "Application name", authorization.Item.Application.Name);
            addParameter(ex, "Item name", authorization.Item.Name);
            addParameter(ex, "Authorization SID", authorization.SID.StringValue);
            
            return ex;
        }
        internal static SqlAzManException AttributeNotFoundException(string key, string storeName, string applicationName, Exception innerException)
        {
            SqlAzManException ex = new SqlAzManException(String.Format("Attribute '{0}' not found. Store '{1}', Application '{2}'.", key, storeName, applicationName), innerException);
            addParameter(ex, "Store name", storeName);
            addParameter(ex, "Application name", applicationName);
            addParameter(ex, "Attribute key", key);
            return ex;
        }
        internal static SqlAzManException AttributeNotFoundException(string key, string storeName, string applicationName, string itemName, Exception innerException)
        {
            SqlAzManException ex = new SqlAzManException(String.Format("Attribute '{0}' not found. Store '{1}', Application '{2}', Item '{3}'.", key, storeName, applicationName, itemName), innerException);
            addParameter(ex, "Store name", storeName);
            addParameter(ex, "Application name", applicationName);
            addParameter(ex, "Item name", itemName);
            addParameter(ex, "Attribute key", key);
            return ex;
        }
        internal static SqlAzManException AttributeNotFoundException(string key, string storeName, Exception innerException)
        {
            SqlAzManException ex = new SqlAzManException(String.Format("Attribute '{0}' not found. Store '{1}'.", key, storeName), innerException);
            addParameter(ex, "Store name", storeName);
            addParameter(ex, "Attribute key", key);
            return ex;
        }
        internal static SqlAzManException AttributeDuplicateException(string attributeKey, IAzManApplication application, Exception innerException)
        {
            SqlAzManException ex =  new SqlAzManException(String.Format("An Attribute with the same key name already exists: '{0}'. Store '{1}', Application '{2}'.", attributeKey, application.Store.Name, application.Name), innerException);
            addParameter(ex, "Attribute key", attributeKey);
            addParameter(ex, application);
            return ex;
        }
        internal static SqlAzManException AttributeDuplicateException(string attributeKey, IAzManStore store, Exception innerException)
        {
            SqlAzManException ex =  new SqlAzManException(String.Format("An Attribute with the same key name already exists: '{0}'. Store '{1}'.", attributeKey, store.Name), innerException);
            addParameter(ex, "Attribute key", attributeKey);
            addParameter(ex, store);
            return ex;
        }
        internal static SqlAzManException AttributeDuplicateException(string attributeKey, IAzManAuthorization authorization, Exception innerException)
        {
            SqlAzManException ex =  new SqlAzManException(String.Format("An Attribute with the same key name already exists: '{0}'. Store '{1}', Application '{2}', Item '{3}', Authorization Id '{4}'.", attributeKey, authorization.Item.Application.Store.Name, authorization.Item.Application.Name, authorization.Item.Name, authorization.AuthorizationId), innerException);
            addParameter(ex, "Attribute key", attributeKey);
            addParameter(ex, authorization);
            return ex;
        }
        internal static SqlAzManException AttributeDuplicateException(string attributeKey, IAzManItem item, Exception innerException)
        {
            SqlAzManException ex =  new SqlAzManException(String.Format("An Attribute with the same key name already exists: '{0}'. Store '{1}', Application '{2}', Item '{3}'.", attributeKey, item.Application.Store.Name, item.Application.Name, item.Name), innerException);
            addParameter(ex, "Attribute key", attributeKey);
            addParameter(ex, item);
            return ex;
        }
        internal static SqlAzManException UserNotFoundException(string user, Exception innerException)
        {
            SqlAzManException ex = new SqlAzManException(String.Format("User '{0}' not found.", user), innerException);
            addParameter(ex, "User", user);
            return ex;
        }
        internal static SqlAzManException ItemNotFoundException(string itemName, IAzManApplication application, Exception innerException)
        {
            SqlAzManException ex =  new SqlAzManException(String.Format("Item '{0}' not found. Store '{1}', Application '{2}'.", itemName, application.Store.Name, application.Name), innerException);
            addParameter(ex, "Item name", itemName);
            addParameter(ex, application);
            return ex;
        }
        internal static SqlAzManException AuthorizationNotFoundException(int authorizationId, IAzManItem item, Exception innerException)
        {
            SqlAzManException ex = new SqlAzManException(String.Format("Authorization {0} not found. Store '{1}', Application '{2}', Item '{3}'.", authorizationId, item.Application.Store.Name, item.Application.Name, item.Name), innerException);
            addParameter(ex, "Store name", item.Application.Store.Name);
            addParameter(ex, "Application name", item.Application.Name);
            addParameter(ex, "Item name", item.Name);
            addParameter(ex, "Authorization id", authorizationId);
            return ex;
        }
        internal static SqlAzManException DBUserNotFoundException(string dbUserName, IAzManApplication application, Exception innerException)
        {
            SqlAzManException ex = new SqlAzManException(String.Format("DB User '{0}' not found. Store '{1}', Application '{2}'.", dbUserName, application.Store.Name, application.Name), innerException);
            addParameter(ex, "DB User name", dbUserName);
            addParameter(ex, application);
            return ex;
        }
        internal static SqlAzManException DBUserNotFoundException(string dbUserName, IAzManStore store, Exception innerException)
        {
            SqlAzManException ex = new SqlAzManException(String.Format("DB User '{0}' not found. Store '{1}'.", dbUserName, store.Name), innerException);
            addParameter(ex, "DB User name", dbUserName);
            addParameter(ex, store);
            return ex;
        }
        internal static SqlAzManException DBUserNotFoundException(string dbUserName, Exception innerException)
        {
            SqlAzManException ex = new SqlAzManException(String.Format("DB User '{0}' not found.", dbUserName), innerException);
            addParameter(ex, "DB User name", dbUserName);
            return ex;
        }
        internal static SqlAzManException ItemNotFoundException(string itemName, string storeName, string applicationName, Exception innerException)
        {
            SqlAzManException ex =  new SqlAzManException(String.Format("Item '{0}' not found. Store '{1}', Application '{2}'.", itemName, storeName, applicationName), innerException);
            addParameter(ex, "Store name", storeName);
            addParameter(ex, "Application name", applicationName);
            addParameter(ex, "Item name", itemName);
            return ex;
        }
        internal static SqlAzManException ItemDuplicateException(string itemName, IAzManApplication application, Exception innerException)
        {
            SqlAzManException ex =  new SqlAzManException(String.Format("An Item with the same name already exists: '{0}'. Store '{1}'. Application '{2}'.", itemName, application.Store.Name, application.Name), innerException);
            addParameter(ex, "Item name", itemName);
            addParameter(ex, "Item name", application);
            return ex;
        }
        internal static SqlAzManException BizRuleException(IAzManItem item, Exception innerException)
        {
            SqlAzManException ex =  new SqlAzManException(String.Format("BizRule Error. Store '{0}', Application '{1}', Item '{2}'.", item.Application.Store.Name, item.Application.Name, item.Name), innerException);
            addParameter(ex, item);
            return ex;
        }
        private static void addParameter(SqlAzManException ex, IAzManStore store)
        {
            addParameter(ex, "Store name", store.Name);
        }
        private static void addParameter(SqlAzManException ex, IAzManApplication application)
        {
            addParameter(ex, "Application name", application.Name);
        }
        private static void addParameter(SqlAzManException ex, IAzManItem item)
        {
            addParameter(ex, "Item name", item.Name);
        }
        private static void addParameter(SqlAzManException ex, IAzManAuthorization auth)
        {
            addParameter(ex, "Authorization Id", auth.AuthorizationId);
        }
        private static void addParameter(SqlAzManException ex, string parameterName, object parameterValue)
        {
            ex.Data.Add(parameterName, parameterValue);
        }
        #endregion static members
    }
}
