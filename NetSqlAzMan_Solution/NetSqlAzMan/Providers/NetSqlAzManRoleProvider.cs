using System;
using System.Web;
using System.Security.Principal;
using System.Security.Permissions;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Configuration.Provider;
using System.Web.Security;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.Cache;
using System.Linq;

namespace NetSqlAzMan.Providers
{
    /// <summary>
    /// ASP.NET Role Provider for .NET Sql Authorization Manager.
    /// </summary>
    [AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    public class NetSqlAzManRoleProvider : RoleProvider
    {
        #region Fields
        /// <summary>
        /// The Storage Cache
        /// </summary>
        protected StorageCache storageCache;
        private String storeName;
        private String applicationName;
        private Boolean useWCFCacheService;
        private static Object locker = new Object();
        private volatile static bool buildingCache = false;
        /// <summary>
        /// The user lookup type.
        /// </summary>
        protected string userLookupType;
        /// <summary>
        /// The default Domain.
        /// </summary>
        protected string defaultDomain;
        #endregion Fields
        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="T:NetSqlAzManRoleProvider"/> class.
        /// </summary>
        public NetSqlAzManRoleProvider()
        {

        }
        #endregion Constructors
        #region Properties
        /// <summary>
        /// Gets the DB user.
        /// </summary>
        /// <param name="dbUserName">Name of the db user.</param>
        /// <returns></returns>
        /// <remarks>Thread-Safe</remarks>
        public virtual IAzManDBUser GetDBUser(string dbUserName)
        {
            using (IAzManStorage storage = new SqlAzManStorage(this.storageCache.ConnectionString))
            {
                IAzManApplication application = storage[this.storeName][this.applicationName];
                return application.GetDBUser(dbUserName);
            }
        }
        /// <summary>
        /// Gets the store.
        /// </summary>
        /// <value>The store.</value>
        public virtual String StoreName
        {
            get
            {
                return this.storeName;
            }
        }

        /// <summary>
        /// Gets a value indicating whether [use WCF cache service].
        /// </summary>
        /// <value><c>true</c> if [use WCF cache service]; otherwise, <c>false</c>.</value>
        public virtual Boolean UseWCFCacheService
        {
            get
            {
                return this.useWCFCacheService;
            }
        }
        /// <summary>
        /// Gets or sets the name of the application to store and retrieve role information for.
        /// </summary>
        /// <value></value>
        /// <returns>The name of the application to store and retrieve role information for.</returns>
        public override String ApplicationName
        {
            get
            {
                return this.applicationName;
            }
            set
            {
                this.applicationName = value;
            }
        }
        /// <summary>
        /// Gets the friendly name used to refer to the provider during configuration.
        /// </summary>
        /// <value></value>
        /// <returns>The friendly name used to refer to the provider during configuration.</returns>
        public override string Name
        {
            get
            {
                return "NetSqlAzManRoleProvider";
            }
        }

        /// <summary>
        /// Gets a brief, friendly description suitable for display in administrative tools or other user interfaces (UIs).
        /// </summary>
        /// <value></value>
        /// <returns>A brief, friendly description suitable for display in administrative tools or other UIs.</returns>
        public override string Description
        {
            get
            {
                return "Role Provider for .NET Sql Authorization Manager - http://netsqlazman.codeplex.com - Andrea Ferendeles";
            }
        }

        /// <summary>
        /// Gets or sets the User Lookup Type. (LDAP or DB)
        /// </summary>
        public virtual string UserLookupType
        {
            get
            {
                return this.userLookupType;
            }
            set
            {
                this.userLookupType = value;
            }
        }
        /// <summary>
        /// Gets or sets the Default Domain. (Valid only for UserLookupType = "LDAP")
        /// </summary>
        public virtual string DefaultDomain
        {
            get
            {
                return this.defaultDomain;
            }
            set
            {
                if (!value.EndsWith("\\"))
                {
                    value += "\\";
                }
                this.defaultDomain = value;
            }
        }
        #endregion Properties
        #region Methods
        /// <summary>
        /// Initializes the provider.
        /// </summary>
        /// <param name="name">The friendly name of the provider.</param>
        /// <param name="config">A collection of the name/value pairs representing the provider-specific attributes specified in the configuration for this provider.</param>
        /// <exception cref="T:System.ArgumentNullException">The name of the provider is null.</exception>
        /// <exception cref="T:System.InvalidOperationException">An attempt is made to call <see cref="M:System.Configuration.Provider.ProviderBase.Initialize(System.String,System.Collections.Specialized.NameValueCollection)"></see> on a provider after the provider has already been initialized.</exception>
        /// <exception cref="T:System.ArgumentException">The name of the provider has a length of zero.</exception>
        public override void Initialize(string name, NameValueCollection config)
        {
            if (config["connectionStringName"] == null)
                throw new ArgumentNullException("connectionStringName", "Connection String parameter required.");
            if (System.Configuration.ConfigurationManager.ConnectionStrings[config["connectionStringName"]] == null)
                throw new ApplicationException(String.Format("Connection String name=\"{0}\" not found.", config["connectionStringName"]));
            if (config["storeName"] == null)
                throw new ArgumentNullException("storeName", "Store Name parameter required.");
            if (config["applicationName"] == null)
                throw new ArgumentNullException("applicationName", "Application Name parameter required.");
            if (config["userLookupType"] == null)
                throw new ArgumentNullException("userLookupType", "userLookupType Name parameter required.");
            if (config["userLookupType"] != "LDAP" && config["userLookupType"] != "DB")
                throw new ArgumentNullException("userLookupType", "userLookupType invalid parameter. Possible values: \"LDAP\", \"DB\".");
            if (config["defaultDomain"] == null)
                throw new ArgumentNullException("defaultDomain", "defaultDomain Name parameter required.");
            base.Initialize(name, config);
            this.storeName = config["storeName"];
            this.applicationName = config["applicationName"];
            this.useWCFCacheService = config["UseWCFCacheService"] == null ? false : Boolean.Parse(config["UseWCFCacheService"]);
            this.storageCache = new StorageCache(System.Configuration.ConfigurationManager.ConnectionStrings[config["connectionStringName"]].ConnectionString);
            this.InvalidateCache(true);
            this.UserLookupType = config["userLookupType"];
            this.DefaultDomain = config["defaultDomain"];
        }
        /// <summary>
        /// Adds the specified user names to the specified roles for the configured applicationName.
        /// </summary>
        /// <param name="usernames">A string array of user names to be added to the specified roles.</param>
        /// <param name="roleNames">A string array of the role names to add the specified user names to.</param>
        public override void AddUsersToRoles(string[] usernames, string[] roleNames)
        {
            using (IAzManStorage storage = new SqlAzManStorage(this.storageCache.ConnectionString))
            {
                try
                {
                    storage.OpenConnection();
                    storage.BeginTransaction();
                    IAzManApplication application = storage[this.storeName][this.applicationName];
                    foreach (string roleName in roleNames)
                    {
                        IAzManItem role = application.GetItem(roleName);
                        if (role.ItemType != ItemType.Role)
                            throw new ArgumentException(String.Format("{0} must be a Role.", roleName));

                        foreach (string username in usernames)
                        {
                            IAzManSid owner = new SqlAzManSID(((System.Threading.Thread.CurrentPrincipal.Identity as WindowsIdentity) ?? WindowsIdentity.GetCurrent()).User);
                            WhereDefined whereDefined = WhereDefined.LDAP;
                            if (this.userLookupType == "LDAP")
                            {
                                string fqun = this.getFQUN(username);
                                NTAccount ntaccount = new NTAccount(fqun);
                                if (ntaccount == null)
                                    throw SqlAzManException.UserNotFoundException(username, null);
                                IAzManSid sid = new SqlAzManSID(((SecurityIdentifier)(ntaccount.Translate(typeof(SecurityIdentifier)))));
                                if (sid == null)
                                    throw SqlAzManException.UserNotFoundException(username, null);
                                role.CreateAuthorization(owner, whereDefined, sid, WhereDefined.LDAP, AuthorizationType.Allow, null, null);
                            }
                            else
                            {
                                var dbuser = application.GetDBUser(username);
                                IAzManSid sid = dbuser.CustomSid;
                                role.CreateAuthorization(owner, whereDefined, sid, WhereDefined.Database, AuthorizationType.Allow, null, null);
                            }
                        }
                    }
                    storage.CommitTransaction();
                    //Rebuild StorageCache
                    this.InvalidateCache(false);
                }
                catch
                {
                    storage.RollBackTransaction();
                    throw;
                }
                finally
                {
                    storage.CloseConnection();
                }
            }
        }

        /// <summary>
        /// Adds a new role to the data source for the configured applicationName.
        /// </summary>
        /// <param name="roleName">The name of the role to create.</param>
        public override void CreateRole(string roleName)
        {
            using (IAzManStorage storage = new SqlAzManStorage(this.storageCache.ConnectionString))
            {
                IAzManApplication application = storage[this.storeName][this.applicationName];
                application.CreateItem(roleName, String.Empty, ItemType.Role);
            }
            //Rebuild StorageCache
            this.InvalidateCache(false);
        }

        /// <summary>
        /// Removes a role from the data source for the configured applicationName.
        /// </summary>
        /// <param name="roleName">The name of the role to delete.</param>
        /// <param name="throwOnPopulatedRole">If true, throw an exception if roleName has one or more members and do not delete roleName.</param>
        /// <returns>
        /// true if the role was successfully deleted; otherwise, false.
        /// </returns>
        public override bool DeleteRole(string roleName, bool throwOnPopulatedRole)
        {
            using (IAzManStorage storage = new SqlAzManStorage(this.storageCache.ConnectionString))
            {
                IAzManApplication application = storage[this.storeName][this.applicationName];
                IAzManItem role = application[roleName];
                if (role == null)
                    throw new ArgumentNullException("roleName");
                if (roleName.Trim() == String.Empty)
                    throw new ArgumentException("roleName parameter cannot be empty.");
                if (role.ItemType != ItemType.Role)
                    throw new ArgumentException(String.Format("{0} must be a Role.", roleName), "roleName");
                if (throwOnPopulatedRole && application[roleName].GetMembers().Length > 0)
                {
                    throw new ProviderException(String.Format("{0} has one or more members and cannot be deleted.", roleName));
                }
                role.Delete();
                //Rebuild StorageCache
                this.InvalidateCache(false);
                return true;
            }
        }

        /// <summary>
        /// Gets an array of user names in a role where the user name contains the specified user name to match.
        /// </summary>
        /// <param name="roleName">The role to search in.</param>
        /// <param name="usernameToMatch">The user name to search for.</param>
        /// <returns>
        /// A string array containing the names of all the users where the user name matches usernameToMatch and the user is a member of the specified role.
        /// </returns>
        public override string[] FindUsersInRole(string roleName, string usernameToMatch)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Gets a list of all the roles for the configured applicationName.
        /// </summary>
        /// <returns>
        /// A string array containing the names of all the roles stored in the data source for the configured applicationName.
        /// </returns>
        public override string[] GetAllRoles()
        {
            if (!this.useWCFCacheService)
                return (from t in this.storageCache.GetAuthorizedItems(this.storeName, this.applicationName, WindowsIdentity.GetCurrent().GetUserBinarySSid(), new string[0], DateTime.Now)
                        where t.Type == ItemType.Role
                        select t.Name).ToArray();
            else
            {
                using (NetSqlAzManWCFCacheService.CacheServiceClient csc = new NetSqlAzManWCFCacheService.CacheServiceClient())
                {
                    try
                    {
                        return (from t in csc.GetAuthorizedItemsForWindowsUsers(this.storeName, this.applicationName, WindowsIdentity.GetCurrent().GetUserBinarySSid(), WindowsIdentity.GetCurrent().GetGroupsBinarySSid(), DateTime.Now, null)
                                where t.Type == NetSqlAzManWCFCacheService.ItemType.Role
                                select t.Name).ToArray();
                    }
                    finally
                    {
                        ((IDisposable)csc).Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// Gets a list of the roles that a specified user is in for the configured applicationName.
        /// </summary>
        /// <param name="username">The user to return a list of roles for.</param>
        /// <returns>
        /// A string array containing the names of all the roles that the specified user is in for the configured applicationName.
        /// </returns>
        public override string[] GetRolesForUser(string username)
        {
            if (this.userLookupType == "LDAP")
            {
                WindowsIdentity wid = null;
                if (String.Compare(((System.Threading.Thread.CurrentPrincipal.Identity as WindowsIdentity) ?? WindowsIdentity.GetCurrent()).Name, this.getFQUN(username), true) == 0)
                {
                    wid = ((System.Threading.Thread.CurrentPrincipal.Identity as WindowsIdentity) ?? WindowsIdentity.GetCurrent());
                }
                if (wid == null)
                {
                    wid = new WindowsIdentity(this.getUpn(this.getFQUN(username))); //Kerberos Protocol Transition: Works in W2K3 native domain only 
                }

                if (!this.useWCFCacheService)
                    return (from t in this.storageCache.GetAuthorizedItems(this.storeName, this.applicationName, wid.GetUserBinarySSid(), wid.GetGroupsBinarySSid(), DateTime.Now)
                            where t.Type == ItemType.Role && (t.Authorization == AuthorizationType.Allow || t.Authorization == AuthorizationType.AllowWithDelegation)
                            select t.Name).ToArray();
                else
                {
                    using (NetSqlAzManWCFCacheService.CacheServiceClient csc = new NetSqlAzManWCFCacheService.CacheServiceClient())
                    {
                        try
                        {
                            return (from t in csc.GetAuthorizedItemsForWindowsUsers(this.storeName, this.applicationName, wid.GetUserBinarySSid(), wid.GetGroupsBinarySSid(), DateTime.Now, null)
                                    where t.Type == NetSqlAzManWCFCacheService.ItemType.Role && (t.Authorization == NetSqlAzManWCFCacheService.AuthorizationType.Allow || t.Authorization == NetSqlAzManWCFCacheService.AuthorizationType.AllowWithDelegation)
                                    select t.Name).ToArray();
                        }
                        finally
                        {
                            ((IDisposable)csc).Dispose();
                        }
                    }
                }
            }
            else
            {
                using (IAzManStorage storage = new SqlAzManStorage(this.storageCache.ConnectionString))
                {
                    IAzManApplication application = storage[this.storeName][this.applicationName];
                    IAzManDBUser dbUser = application.GetDBUser(username);
                    if (dbUser == null)
                        throw SqlAzManException.DBUserNotFoundException(username, null);
                    IAzManSid sid = dbUser.CustomSid;

                    if (!this.useWCFCacheService)
                        return (from t in this.storageCache.GetAuthorizedItems(this.storeName, this.applicationName, sid.StringValue, DateTime.Now)
                                where t.Type == ItemType.Role && (t.Authorization == AuthorizationType.Allow || t.Authorization == AuthorizationType.AllowWithDelegation)
                                select t.Name).ToArray();
                    else
                    {
                        using (NetSqlAzManWCFCacheService.CacheServiceClient csc = new NetSqlAzManWCFCacheService.CacheServiceClient())
                        {
                            try
                            {
                                return (from t in csc.GetAuthorizedItemsForDatabaseUsers(this.storeName, this.applicationName, sid.StringValue, DateTime.Now, null)
                                        where t.Type == NetSqlAzManWCFCacheService.ItemType.Role && (t.Authorization == NetSqlAzManWCFCacheService.AuthorizationType.Allow || t.Authorization == NetSqlAzManWCFCacheService.AuthorizationType.AllowWithDelegation)
                                        select t.Name).ToArray();
                            }
                            finally
                            {
                                ((IDisposable)csc).Dispose();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets a list of users in the specified role for the configured applicationName.
        /// </summary>
        /// <param name="roleName">The name of the role to get the list of users for.</param>
        /// <returns>
        /// A string array containing the names of all the users who are members of the specified role for the configured applicationName.
        /// </returns>
        public override string[] GetUsersInRole(string roleName)
        {
            using (IAzManStorage storage = new SqlAzManStorage(this.storageCache.ConnectionString))
            {
                IAzManApplication application = storage[this.storeName][this.applicationName];
                IAzManItem role = application[roleName];
                if (role.ItemType != ItemType.Role)
                    throw new ArgumentException(String.Format("{0} must be a Role.", roleName), "roleName");

                IAzManAuthorization[] authz = role.GetAuthorizations();
                List<string> users = new List<string>();
                foreach (IAzManAuthorization auth in authz)
                {
                    if (auth.AuthorizationType == AuthorizationType.Allow
                        ||
                        auth.AuthorizationType == AuthorizationType.AllowWithDelegation)
                    {
                        if (auth.SidWhereDefined == WhereDefined.Local || auth.SidWhereDefined == WhereDefined.LDAP)
                        {
                            string displayName;
                            auth.GetMemberInfo(out displayName);
                            users.Add(displayName);
                        }
                        else if (auth.SidWhereDefined == WhereDefined.Database)
                        {
                            users.Add(application.GetDBUser(auth.SID).UserName);
                        }
                    }
                }
                return users.ToArray();
            }
        }

        /// <summary>
        /// Gets a value indicating whether the specified user is in the specified role for the configured applicationName.
        /// </summary>
        /// <param name="username">The user name to search for.</param>
        /// <param name="roleName">The role to search in.</param>
        /// <returns>
        /// true if the specified user is in the specified role for the configured applicationName; otherwise, false.
        /// </returns>
        public override bool IsUserInRole(string username, string roleName)
        {
            if (this.userLookupType == "LDAP")
            {
                WindowsIdentity wid = null;
                if (String.Compare(((System.Threading.Thread.CurrentPrincipal.Identity as WindowsIdentity) ?? WindowsIdentity.GetCurrent()).Name, this.getFQUN(username), true) == 0)
                {
                    wid = ((System.Threading.Thread.CurrentPrincipal.Identity as WindowsIdentity) ?? WindowsIdentity.GetCurrent());
                }
                if (wid == null)
                {
                    wid = new WindowsIdentity(this.getUpn(this.getFQUN(username))); //Kerberos Protocol Transition: Works in W2K3 native domain only 
                }

                if (!this.useWCFCacheService)
                    return (from t in this.storageCache.GetAuthorizedItems(this.storeName, this.applicationName, wid.GetUserBinarySSid(), wid.GetGroupsBinarySSid(), DateTime.Now)
                            where
                            t.Type == ItemType.Role
                            &&
                            (t.Authorization == AuthorizationType.Allow || t.Authorization == AuthorizationType.AllowWithDelegation)
                            &&
                            t.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)
                            select t).Count() > 0;
                else
                {
                    using (NetSqlAzManWCFCacheService.CacheServiceClient csc = new NetSqlAzManWCFCacheService.CacheServiceClient())
                    {
                        try
                        {
                            return (from t in csc.GetAuthorizedItemsForWindowsUsers(this.storeName, this.applicationName, wid.GetUserBinarySSid(), wid.GetGroupsBinarySSid(), DateTime.Now, null)
                                    where
                                    t.Type == NetSqlAzManWCFCacheService.ItemType.Role
                                    &&
                                    (t.Authorization == NetSqlAzManWCFCacheService.AuthorizationType.Allow || t.Authorization == NetSqlAzManWCFCacheService.AuthorizationType.AllowWithDelegation)
                                    &&
                                    t.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)
                                    select t).Count() > 0;
                        }
                        finally
                        {
                            ((IDisposable)csc).Dispose();
                        }
                    }
                }
            }
            else
            {
                using (IAzManStorage storage = new SqlAzManStorage(this.storageCache.ConnectionString))
                {
                    IAzManApplication application = storage[this.storeName][this.applicationName];
                    IAzManDBUser dbUser = application.GetDBUser(username);

                    if (!this.useWCFCacheService)
                        return (from t in this.storageCache.GetAuthorizedItems(this.storeName, this.applicationName, dbUser.CustomSid.StringValue, DateTime.Now)
                                where
                                t.Type == ItemType.Role
                                &&
                                (t.Authorization == AuthorizationType.Allow || t.Authorization == AuthorizationType.AllowWithDelegation)
                                &&
                                t.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)
                                select t).Count() > 0;
                    else
                    {
                        using (NetSqlAzManWCFCacheService.CacheServiceClient csc = new NetSqlAzManWCFCacheService.CacheServiceClient())
                        {
                            try
                            {
                                return (from t in csc.GetAuthorizedItemsForDatabaseUsers(this.storeName, this.applicationName, dbUser.CustomSid.StringValue, DateTime.Now, null)
                                        where
                                        t.Type == NetSqlAzManWCFCacheService.ItemType.Role
                                        &&
                                        (t.Authorization == NetSqlAzManWCFCacheService.AuthorizationType.Allow || t.Authorization == NetSqlAzManWCFCacheService.AuthorizationType.AllowWithDelegation)
                                        &&
                                        t.Name.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)
                                        select t).Count() > 0;
                            }
                            finally
                            {
                                ((IDisposable)csc).Dispose();
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Removes the specified user names from the specified roles for the configured applicationName.
        /// </summary>
        /// <param name="usernames">A string array of user names to be removed from the specified roles.</param>
        /// <param name="roleNames">A string array of role names to remove the specified user names from.</param>
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            using (IAzManStorage storage = new SqlAzManStorage(this.storageCache.ConnectionString))
            {
                try
                {
                    storage.OpenConnection();
                    storage.BeginTransaction();
                    IAzManApplication application = storage[this.storeName][this.applicationName];
                    foreach (string roleName in roleNames)
                    {
                        IAzManItem role = application.GetItem(roleName);
                        if (role.ItemType != ItemType.Role)
                            throw new ArgumentException(String.Format("{0} must be a Role.", roleName));
                        foreach (IAzManAuthorization auth in role.GetAuthorizations())
                        {
                            string displayName;
                            auth.GetMemberInfo(out displayName);
                            foreach (string username in usernames)
                            {
                                if (String.Compare(this.getFQUN(username), displayName, true) == 0)
                                {
                                    auth.Delete();
                                }
                            }
                        }
                    }
                    storage.CommitTransaction();
                    //Rebuild StorageCache
                    this.InvalidateCache(false);
                }
                catch
                {
                    storage.RollBackTransaction();
                    throw;
                }
                finally
                {
                    storage.CloseConnection();
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether the specified role name already exists in the role data source for the configured applicationName.
        /// </summary>
        /// <param name="roleName">The name of the role to search for in the data source.</param>
        /// <returns>
        /// true if the role name already exists in the data source for the configured applicationName; otherwise, false.
        /// </returns>
        public override bool RoleExists(string roleName)
        {
            return (from t in this.GetAllRoles()
                    where t.Equals(roleName, StringComparison.CurrentCultureIgnoreCase)
                    select t).Count() > 0;
        }
        /// <summary>
        /// This code takes the user name supplied in the login form, constructs a UPN in the format userName@domainName, and passes the UPN to the WindowsIdentity constructor. This constructor uses the Kerberos S4U extension to obtain a Windows token for the user.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        /// <remarks>http://msdn2.microsoft.com/en-us/library/ms998355.aspx</remarks>
        private string getUpn(string userName)
        {
            // Obtain the user name (obtained from forms authentication)
            string identity = userName;

            // Convert the user name from domainName\userName format to 
            // userName@domainName format if necessary
            int slash = identity.IndexOf("\\");
            if (slash > 0)
            {
                string domain = identity.Substring(0, slash);
                string user = identity.Substring(slash + 1);
                identity = user + "@" + domain;
            }
            else
            {
                identity = userName + "@" + Environment.UserDomainName;
            }
            return identity;
        }

        private string getFQUN(string userName)
        {
            if (this.userLookupType == "LDAP")
            {
                if (!userName.Contains("\\"))
                {
                    return this.defaultDomain + userName;
                }
                else
                {
                    return userName;
                }
            }
            else
            {
                return userName;
            }
        }
        /// <summary>
        /// Gets the storage.
        /// </summary>
        /// <returns></returns>
        public IAzManStorage GetStorage()
        {
            return new SqlAzManStorage(this.storageCache.ConnectionString);
        }
        /// <summary>
        /// Gets the store.
        /// </summary>
        /// <returns></returns>
        public IAzManStore GetStore()
        {
            return this.GetStorage()[this.storeName];
        }
        /// <summary>
        /// Gets the application.
        /// </summary>
        /// <returns></returns>
        public IAzManApplication GetApplication()
        {
            return this.GetStore()[this.applicationName];
        }
        /// <summary>
        /// Invalidates the cache.
        /// </summary>
        /// <param name="waitForCacheBuiltCompletition">if set to <c>true</c> [wait for cache built completition].</param>
        public void InvalidateCache(bool waitForCacheBuiltCompletition)
        {
            if (!waitForCacheBuiltCompletition && NetSqlAzManRoleProvider.buildingCache)
            {
                //Ignore build cache requests while building
                return;
            }
            else
            {
                lock (NetSqlAzManRoleProvider.locker)
                {
                    try
                    {
                        NetSqlAzManRoleProvider.buildingCache = true;
                        if (!this.useWCFCacheService)
                        {
                            this.storageCache.BuildStorageCache(this.storeName, this.applicationName);
                        }
                        else
                        {
                            using (NetSqlAzManWCFCacheService.CacheServiceClient csc = new NetSqlAzManWCFCacheService.CacheServiceClient())
                            {
                                try
                                {
                                    csc.InvalidateCache();
                                }
                                finally
                                {
                                    ((IDisposable)csc).Dispose();
                                }
                            }
                        }
                    }
                    finally
                    {
                        NetSqlAzManRoleProvider.buildingCache = false;
                    }
                }
            }
        }
        #endregion Methods
    }
}
