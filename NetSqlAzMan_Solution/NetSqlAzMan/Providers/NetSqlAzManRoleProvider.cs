using System;
using System.Web;
using System.Web.Configuration;
using System.Web.Util;
using System.Security;
using System.Security.Principal;
using System.Security.Permissions;
using System.Globalization;
using System.Runtime.Serialization;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Text;
using System.Configuration.Provider;
using System.Configuration;
using System.Data.OleDb;
using System.Reflection;
using System.Web.Hosting;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Web.Security;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.LINQ;
using System.Linq;
using NetSqlAzMan.ENS;
using NetSqlAzMan.DirectoryServices;

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
        private IAzManStorage storage;
        private IAzManStore store;
        private IAzManApplication application;
        private string userLookupType;
        private string defaultDomain;
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
        /// <summary>
        /// Gets the store.
        /// </summary>
        /// <value>The store.</value>
        public IAzManStore Store
        {
            get
            {
                return this.store;
            }
        }
        /// <summary>
        /// Gets the application.
        /// </summary>
        /// <value>The application.</value>
        public IAzManApplication Application
        {
            get
            {
                return this.application;
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
        /// Gets or sets the name of the application to store and retrieve role information for.
        /// </summary>
        /// <value></value>
        /// <returns>The name of the application to store and retrieve role information for.</returns>
        public override string ApplicationName
        {
            get
            {
                return this.application.Name;
            }
            set
            {
                this.application = this.store.GetApplication(value);
            }
        }

        /// <summary>
        /// Gets or sets the User Lookup Type. (LDAP or DB)
        /// </summary>
        public string UserLookupType
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
        public string DefaultDomain
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
            if (config["userLookupType"] != "LDAP" && config["userLookupType"]!="DB")
                throw new ArgumentNullException("userLookupType", "userLookupType invalid parameter. Possible values: \"LDAP\", \"DB\".");
            if (config["defaultDomain"] == null)
                throw new ArgumentNullException("defaultDomain", "defaultDomain Name parameter required.");
            base.Initialize(name, config);
            this.storage = new SqlAzManStorage(System.Configuration.ConfigurationManager.ConnectionStrings[config["connectionStringName"]].ConnectionString);
            this.store = this.storage[config["storeName"]];
            this.application = this.store[config["applicationName"]];
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
            try
            {
                this.storage.OpenConnection();
                this.storage.BeginTransaction();
                foreach (string roleName in roleNames)
                {
                    IAzManItem role = this.application.GetItem(roleName);
                    if (role.ItemType != ItemType.Role)
                        throw new ArgumentException(String.Format("{0} must be a Role.", roleName));

                    foreach (string username in usernames)
                    {
                        IAzManSid owner = new SqlAzManSID(WindowsIdentity.GetCurrent().User);
                        WhereDefined whereDefined = WhereDefined.LDAP;
                        if (this.userLookupType=="LDAP")
                        {
                            IAzManSid sid = new SqlAzManSID(((SecurityIdentifier)(new NTAccount(this.getFQUN(username)).Translate(typeof(SecurityIdentifier)))));
                            role.CreateAuthorization(owner, whereDefined, sid, WhereDefined.LDAP, AuthorizationType.Allow, null, null);
                        }
                        else
                        {
                            IAzManSid sid = this.application.GetDBUser(username).CustomSid;
                            role.CreateAuthorization(owner, whereDefined, sid, WhereDefined.Database, AuthorizationType.Allow, null, null);
                        }
                    }
                }
                this.storage.CommitTransaction();
            }
            catch
            {
                this.storage.RollBackTransaction();
                throw;
            }
            finally
            {
                this.storage.CloseConnection();
            }
        }

        /// <summary>
        /// Adds a new role to the data source for the configured applicationName.
        /// </summary>
        /// <param name="roleName">The name of the role to create.</param>
        public override void CreateRole(string roleName)
        {
            this.application.CreateItem(roleName, String.Empty, ItemType.Role);
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
            IAzManItem role = this.application[roleName];
            if (role == null)
                throw new ArgumentNullException("roleName");
            if (roleName.Trim() == String.Empty)
                throw new ArgumentException("roleName parameter cannot be empty.");
            if (role.ItemType != ItemType.Role)
                throw new ArgumentException(String.Format("{0} must be a Role.", roleName), "roleName");
            if (throwOnPopulatedRole && this.application[roleName].GetMembers().Length > 0)
            {
                throw new ProviderException(String.Format("{0} has one or more members and cannot be deleted.", roleName));
            }
            role.Delete();
            return true;
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
            List<string> roles = new List<string>();
            foreach (IAzManItem role in this.application.GetItems(ItemType.Role))
            {
                roles.Add(role.Name);
            }
            return roles.ToArray();
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
            IAzManItem[] allRoles = this.application.GetItems(ItemType.Role);
            List<string> roles = new List<string>();
            if (this.userLookupType=="LDAP")
            {
                WindowsIdentity wid = null;
                if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.User.Identity as WindowsIdentity != null && String.Compare(this.getFQUN(username), System.Web.HttpContext.Current.User.Identity.Name, true) == 0)
                {
                    wid = (WindowsIdentity)System.Web.HttpContext.Current.User.Identity;
                }
                else if (String.Compare(WindowsIdentity.GetCurrent().Name, this.getFQUN(username), true) == 0)
                {
                    wid = WindowsIdentity.GetCurrent();
                }
                if (wid == null)
                {
                    wid = new WindowsIdentity(this.getUpn(this.getFQUN(username))); //Kerberos Protocol Transition: Works in W2K3 native domain only 
                }
                foreach (IAzManItem role in allRoles)
                {
                    AuthorizationType auth = role.CheckAccess(wid, DateTime.Now);
                    if (auth == AuthorizationType.Allow || auth == AuthorizationType.AllowWithDelegation)
                    {
                        roles.Add(role.Name);
                    }
                }
            }
            else
            {
                IAzManDBUser dbUser = this.application.GetDBUser(username);
                if (dbUser == null)
                    throw new Exception(String.Format("DBUser '{0}' not found.", username));
                IAzManSid sid = dbUser.CustomSid;
                foreach (IAzManItem role in allRoles)
                {
                    AuthorizationType auth = role.CheckAccess(new SqlAzManDBUser(sid, username), DateTime.Now);
                    if (auth == AuthorizationType.Allow || auth == AuthorizationType.AllowWithDelegation)
                    {
                        roles.Add(role.Name);
                    }
                }
            }
            return roles.ToArray();
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
            IAzManItem role = this.application[roleName];
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
                        users.Add(this.application.GetDBUser(auth.SID).UserName);
                    }
                }
            }
            return users.ToArray();
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
            IAzManItem role = this.application[roleName];
            if (role.ItemType != ItemType.Role)
                throw new ArgumentException(String.Format("{0} must be a Role.", roleName), "roleName");

            if (this.userLookupType=="LDAP")
            {
                WindowsIdentity wid = null;
                if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.User.Identity as WindowsIdentity != null && String.Compare(this.getFQUN(username), System.Web.HttpContext.Current.User.Identity.Name, true) == 0)
                {
                    wid = (WindowsIdentity)System.Web.HttpContext.Current.User.Identity;
                }
                else if (String.Compare(WindowsIdentity.GetCurrent().Name, this.getFQUN(username), true) == 0)
                {
                    wid = WindowsIdentity.GetCurrent();
                }
                if (wid == null)
                {
                    wid = new WindowsIdentity(this.getUpn(this.getFQUN(username))); //Kerberos Protocol Transition: Works in W2K3 native domain only 
                }
                AuthorizationType auth = role.CheckAccess(wid, DateTime.Now);
                return auth == AuthorizationType.Allow || auth == AuthorizationType.AllowWithDelegation;
            }
            else
            {
                IAzManDBUser dbUser = this.application.GetDBUser(username);
                AuthorizationType auth = role.CheckAccess(dbUser, DateTime.Now);
                return auth == AuthorizationType.Allow || auth == AuthorizationType.AllowWithDelegation;
            }
        }

        /// <summary>
        /// Removes the specified user names from the specified roles for the configured applicationName.
        /// </summary>
        /// <param name="usernames">A string array of user names to be removed from the specified roles.</param>
        /// <param name="roleNames">A string array of role names to remove the specified user names from.</param>
        public override void RemoveUsersFromRoles(string[] usernames, string[] roleNames)
        {
            try
            {
                this.storage.OpenConnection();
                this.storage.BeginTransaction();
                foreach (string roleName in roleNames)
                {
                    IAzManItem role = this.application.GetItem(roleName);
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
                this.storage.CommitTransaction();
            }
            catch
            {
                this.storage.RollBackTransaction();
                throw;
            }
            finally
            {
                this.storage.CloseConnection();
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
            IAzManItem role = this.application[roleName];
            return (role != null && role.ItemType == ItemType.Role);
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
        #endregion Methods
    }
}
