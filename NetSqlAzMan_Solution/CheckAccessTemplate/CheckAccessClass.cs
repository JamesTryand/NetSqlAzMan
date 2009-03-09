namespace CheckAccessNamespace
{
    using System;
    using System.Security.Principal;
    using System.Collections.Generic;
    using System.Text;
    using NetSqlAzMan;
    using NetSqlAzMan.Interfaces;

    /// <summary>
    /// NetSqlAzMan Check Access Helper Class for 'ApplicazioneSogeiSia' Application 
    /// </summary>
    public partial class NetSqlAzManHelper
    {
        #region Constants
        /// <summary>
        /// Store Name
        /// </summary>
        protected const string STORENAME = "NETSQLAZMANSTORE";
        /// <summary>
        /// Application Name
        /// </summary>
        protected const string APPLICATIONNAME = "APPLICAZIONESIA";
        #endregion Constants
        #region Fields
        /// <summary>
        /// NetSqlAzMan Storage
        /// </summary>
        protected IAzManStorage storage;
        /// <summary>
        /// User Identity
        /// </summary>
        protected WindowsIdentity windowsIdentity;
        #endregion Fields
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="T:NetSqlAzManHelper"/> class.
        /// </summary>
        /// <param name="storageConnectionString">The storage connection string.</param>
        /// <param name="windowsIdentity">The windows identity.</param>
        public NetSqlAzManHelper(string storageConnectionString, WindowsIdentity windowsIdentity)
        {
            this.storage = new SqlAzManStorage(storageConnectionString);
            this.windowsIdentity = windowsIdentity;
        }
        #endregion Constructors
        #region Enums
        /// <summary>
        /// Role Enumeration
        /// </summary>
        public enum Role
        {
            /// <summary>
            /// Role Amministratore
            /// </summary>
            Amministratore,
            /// <summary>
            /// Role ResponsabileUo
            /// </summary>
            ResponsabileUo
        }
        /// <summary>
        /// Task Enumeration
        /// </summary>
        public enum Task
        {
            /// <summary>
            /// Task Attività Deleganti
            /// </summary>
            Attività_Deleganti
        }
        /// <summary>
        /// Operation Enum
        /// </summary>
        public enum Operation
        {
            /// <summary>
            /// Operation Delega
            /// </summary>
            Delega
        }
        #endregion Enums
        #region Properties
        /// <summary>
        /// Gets the storage.
        /// </summary>
        /// <value>The storage.</value>
        public IAzManStorage Storage
        {
            get
            {
                return this.storage;
            }
        }
        #endregion Properties
        #region Methods
        /// <summary>
        /// Opens the connection.
        /// </summary>
        public void OpenConnection()
        {
            this.storage.OpenConnection();
        }
        /// <summary>
        /// Closes the connection.
        /// </summary>
        public void CloseConnection()
        {
            this.storage.CloseConnection();
        }
        /// <summary>
        /// Retrieve Item name from a Role Enum.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns>The Role Name</returns>
        public string ItemName(Role role) 
        {
            switch (role)
            {
                case Role.Amministratore : return "AMMINISTRATORE";
                case Role.ResponsabileUo : return "RESPONSABILEUO";
                default:
                    throw new ArgumentException("Unknow Role name", "role");
            }
        }
        /// <summary>
        /// Retrieve Item name from a Task Enum.
        /// </summary>
        /// <param name="task">The Task</param>
        /// <returns>The Task Name</returns>
        public string ItemName(Task task)
        {
            if (task == Task.Attività_Deleganti) return "Attività Deleganti";
            else throw new ArgumentException("Unknow Task name", "task");
        }
        /// <summary>
        /// Retrieve Item name from an Operation Enum.
        /// </summary>
        /// <param name="operation">The Operation</param>
        /// <returns>The Operation name</returns>
        public string ItemName(Operation operation)
        {
            switch (operation)
            {
                case Operation.Delega: return "Delega";
                default:
                    throw new ArgumentException("Unknow Operation name", "operation");
            }
        }
        /// <summary>
        /// Checks the access.
        /// </summary>
        /// <param name="itemName">The Item Name.</param>
        /// <param name="operationsOnly">if set to <c>true</c> checks the access for operations only.</param>
        /// <returns>The Authorization Type [AllowWithDelegation, Allow, Deny, Neutral]</returns>
        protected AuthorizationType CheckAccess(string itemName, bool operationsOnly)
        {
            return this.storage.CheckAccess(NetSqlAzManHelper.STORENAME, NetSqlAzManHelper.APPLICATIONNAME, itemName, this.windowsIdentity, DateTime.Now, operationsOnly);
        }

        /// <summary>
        /// Gets the type of the authorization.
        /// </summary>
        /// <param name="role">The Role.</param>
        /// <returns>The Authorization Type [AllowWithDelegation, Allow, Deny, Neutral]</returns>
        public AuthorizationType GetAuthorizationType(Role role)
        {
            return this.CheckAccess(this.ItemName(role), false);
        }
        /// <summary>
        /// Gets the type of the authorization.
        /// </summary>
        /// <param name="task">The Task.</param>
        /// <returns>The Authorization Type [AllowWithDelegation, Allow, Deny, Neutral]</returns>
        public AuthorizationType GetAuthorizationType(Task task)
        {
            return this.CheckAccess(this.ItemName(task), false);
        }
        /// <summary>
        /// Gets the type of the authorization.
        /// </summary>
        /// <param name="operation">The Operation.</param>
        /// <returns>The Authorization Type [AllowWithDelegation, Allow, Deny, Neutral]</returns>
        public AuthorizationType GetAuthorizationType(Operation operation)
        {
            return this.CheckAccess(this.ItemName(operation), false);
        }

        /// <summary>
        /// Checks the access.
        /// </summary>
        /// <param name="role">The Role.</param>
        /// <returns>True for Access Granted, False for Access Denied</returns>
        public bool CheckAccess(Role role)
        {
            AuthorizationType result = this.GetAuthorizationType(role);
            return (result == AuthorizationType.AllowWithDelegation || result == AuthorizationType.Allow);
        }
        /// <summary>
        /// Checks the access.
        /// </summary>
        /// <param name="task">The Task.</param>
        /// <returns>True for Access Granted, False for Access Denied</returns>
        public bool CheckAccess(Task task)
        {
            AuthorizationType result = this.GetAuthorizationType(task);
            return (result == AuthorizationType.AllowWithDelegation || result == AuthorizationType.Allow);
        }
        /// <summary>
        /// Checks the access.
        /// </summary>
        /// <param name="operation">The Operation.</param>
        /// <returns>True for Access Granted, False for Access Denied</returns>
        public bool CheckAccess(Operation operation)
        {
            AuthorizationType result = this.GetAuthorizationType(operation);
            return (result == AuthorizationType.AllowWithDelegation || result == AuthorizationType.Allow);
        }
        #endregion Methods
    }
}
