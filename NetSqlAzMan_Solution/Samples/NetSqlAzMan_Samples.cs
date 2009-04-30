/* -------------------
 * NetSqlAzMan Samples
 * -------------------
 * Andrea Ferendeles
 * aferende@hotmail.com
 * http://netsqlazman.codeplex.com
 * 
 * 1) Install first NetSqlAzMan.msi
 * 2) Create a new Sql database: "NetSqlAzManStorage"
 * 3) Execute Sql Script (installation folder) on this sql database
 * 4) Launch NetSqlAzMan Console: start - run - "netsqlazman.msc"
 * 5) Create a Store called "My Store"
 * 6) Under "My Store", create an Application called "My Application"
 * 7) Under "My Application" - Item Definitions, create an Operation called "My Operation"
 * 8) Under Item Authorization - "My Operation", assign yourself (Windows Account) a set an "Allow" permission
 * 
 */

using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;
//*************************************************
//TODO: Add a reference to NetSqlAzMan.dll assembly
//*************************************************
using NetSqlAzMan;
using NetSqlAzMan.Interfaces;
//*************************************************

namespace NetSqlAzMan_CSharp_Samples
{
    /// <summary>
    /// NetSqlAzMan C# Samples
    /// </summary>
    public partial class NetSqlAzMan_Samples
    {
        /// <summary>
        /// Check Access from your Application [FOR Windows Users ONLY].
        /// </summary>
        /// <param name="userIdentity">Windows User Identity.</param>
        private void CheckAccessPermissionsForWindowsUsers(WindowsIdentity userIdentity, bool useCache)
        {
            // USER MUST BE A MEMBER OF SQL DATABASE ROLE: NetSqlAzMan_Readers

            //Sql Storage connection string
            string sqlConnectionString = "data source=(local);initial catalog=NetSqlAzManStorage;user id=netsqlazmanuser;password=password";
            //Create an instance of SqlAzManStorage class
            IAzManStorage storage = new SqlAzManStorage(sqlConnectionString);
            //To Pass current user identity:
            //WindowsIdentity.GetCurrent() -> for Windows Applications 
            //this.Request.LogonUserIdentity -> for ASP.NET Applications
            List<KeyValuePair<string, string>> attributes;
            AuthorizationType auth;
            if (useCache)
            {
                //Build the cache Only one time per session/application/user
                NetSqlAzMan.Cache.UserPermissionCache cache = new NetSqlAzMan.Cache.UserPermissionCache(storage, "My Store", "My Application", userIdentity, true, true);
                //Then Check Access
                auth = cache.CheckAccess("My Operation", DateTime.Now,out attributes);
            }
            else
            {
                auth = storage.CheckAccess("My Store", "My Application", "My Operation", userIdentity, DateTime.Now, true, out attributes);
            }
            switch (auth)
            { 
                case AuthorizationType.AllowWithDelegation:
                    //Yes, I can ... and I can delegate
                    break;
                case AuthorizationType.Allow:
                    //Yes, I can
                    break;
                case AuthorizationType.Neutral:
                case AuthorizationType.Deny:
                    //No, I cannot
                    break;
            }
            //Do something with attributes found
        }

        /// <summary>
        /// Check Access from your Application [FOR Windows Users ONLY].
        /// </summary>
        /// <param name="dbUserName">DB Username</param>
        private void CheckAccessPermissionsForDBUsers(string dbUserName)
        {
            // REMBER: 
            // Modify dbo.GetDBUsers Table-Function to customize DB User list.
            // USER MUST BE A MEMBER OF SQL DATABASE ROLE: NetSqlAzMan_Readers
            //Sql Storage connection string
            string sqlConnectionString = "data source=(local);initial catalog=NetSqlAzManStorage;user id=netsqlazmanuser;password=password";
            //Create an instance of SqlAzManStorage class
            IAzManStorage storage = new SqlAzManStorage(sqlConnectionString);
            //Retrieve DB User identity from dbo.GetDBUsers Table-Function
            IAzManDBUser dbUser = storage.GetDBUser(dbUserName);
            AuthorizationType auth = storage.CheckAccess("My Store", "My Application", "My Operation", dbUser, DateTime.Now, true);
            switch (auth)
            {
                case AuthorizationType.AllowWithDelegation:
                    //Yes, I can ... and I can delegate
                    break;
                case AuthorizationType.Allow:
                    //Yes, I can
                    break;
                case AuthorizationType.Neutral:
                case AuthorizationType.Deny:
                    //No, I cannot
                    break;
            }
        }

        /// <summary>
        /// Navigate through NetSqlAzMan DOM (Document Object Model)
        /// </summary>
        private void NetSqlAzMan_DOM_Sample()
        {
            // USER MUST BE A MEMBER OF SQL DATABASE ROLE: NetSqlAzMan_Readers

            //Sql Storage connection string
            string sqlConnectionString = "data source=(local);initial catalog=NetSqlAzManStorage;user id=netsqlazmanuser;password=password";
            //Create an instance of SqlAzManStorage class
            IAzManStorage storage = new SqlAzManStorage(sqlConnectionString);
            IAzManStore mystore = storage.GetStore("My Store"); //or storage["My Store"]
            IAzManApplication myapp = mystore.GetApplication("My Application");
            IAzManItem myop = myapp.GetItem("My Operation");
            IAzManAuthorization[] auths = myop.GetAuthorizations();
            foreach (IAzManAuthorization auth in auths)
            {
                IAzManAttribute<IAzManAuthorization>[] attrs = auth.GetAttributes();
                foreach (IAzManAttribute<IAzManAuthorization> attr in attrs)
                {
                    string attrKey = attr.Key;
                    string attrValue = attr.Value;
                    //do something
                }
            }
        }

        /// <summary>
        /// Create an Authorization Delegate
        /// </summary>
        private void CreateDelegate()
        {
            // USER MUST BE A MEMBER OF SQL DATABASE ROLE: NetSqlAzMan_Users

            //Sql Storage connection string
            string sqlConnectionString = "data source=(local);initial catalog=NetSqlAzManStorage;user id=netsqlazmanuser;password=password";
            //Create an instance of SqlAzManStorage class
            IAzManStorage storage = new SqlAzManStorage(sqlConnectionString);
            IAzManStore mystore = storage.GetStore("My Store"); //or storage["My Store"]
            IAzManApplication myapp = mystore.GetApplication("My Application");
            IAzManItem myop = myapp.GetItem("My Operation");
            //Retrieve current user identity (delegating user)
            WindowsIdentity userIdentity = ((System.Threading.Thread.CurrentPrincipal.Identity as WindowsIdentity) ?? WindowsIdentity.GetCurrent()); //for Windows Applications 
            //WindowsIdentity userIdentity = this.Request.LogonUserIdentity; //for ASP.NET Applications
            //Retrieve delegate user Login
            NTAccount delegateUserLogin = new NTAccount("DOMAIN","delegateuseraccount");
            //Retrieve delegate user SID
            SecurityIdentifier delegateSID = (SecurityIdentifier)delegateUserLogin.Translate(typeof(SecurityIdentifier));
            IAzManSid delegateNetSqlAzManSID = new SqlAzManSID(delegateSID);
            //Estabilish delegate authorization (only Allow or Deny)
            RestrictedAuthorizationType delegateAuthorization = RestrictedAuthorizationType.Allow;
            //Create delegate
            IAzManAuthorization del = myop.CreateDelegateAuthorization(userIdentity, delegateNetSqlAzManSID, delegateAuthorization, new DateTime(2006, 1, 1, 0,0,0), new DateTime(2006, 12, 31, 23, 59, 59));
            //Set custom Attribute on Authorization Delegate
            del.CreateAttribute("MyCustomInfoKey", "MyCustomInfoValue");
        }

        /// <summary>
        /// Remove Authorization Delegate
        /// </summary>
        private void RemoveDelegate()
        {
            // USER MUST BE A MEMBER OF SQL DATABASE ROLE: NetSqlAzMan_Users

            //Sql Storage connection string
            string sqlConnectionString = "data source=(local);initial catalog=NetSqlAzManStorage;user id=netsqlazmanuser;password=password";
            //Create an instance of SqlAzManStorage class
            IAzManStorage storage = new SqlAzManStorage(sqlConnectionString);
            IAzManStore mystore = storage.GetStore("My Store"); //or storage["My Store"]
            IAzManApplication myapp = mystore.GetApplication("My Application");
            IAzManItem myop = myapp.GetItem("My Operation");
            //Retrieve current user identity (delegating user)
            WindowsIdentity userIdentity = ((System.Threading.Thread.CurrentPrincipal.Identity as WindowsIdentity) ?? WindowsIdentity.GetCurrent()); //for Windows Applications 
            //WindowsIdentity userIdentity = this.Request.LogonUserIdentity; //for ASP.NET Applications
            //Retrieve delegate user Login
            NTAccount delegateUserLogin = new NTAccount("DOMAIN", "delegateuseraccount");
            //Retrieve delegate user SID
            SecurityIdentifier delegateSID = (SecurityIdentifier)delegateUserLogin.Translate(typeof(SecurityIdentifier));
            IAzManSid delegateNetSqlAzManSID = new SqlAzManSID(delegateSID);
            //Estabilish delegate authorization (only Allow or Deny)
            RestrictedAuthorizationType delegateAuthorization = RestrictedAuthorizationType.Allow;
            //Remove delegate and all custom attributes
            myop.DeleteDelegateAuthorization(userIdentity, delegateNetSqlAzManSID, delegateAuthorization);
        }
        /// <summary>
        /// Create a Full Storage through .NET code
        /// </summary>
        private void CreateFullStorage()
        {
            // USER MUST BE A MEMBER OF SQL DATABASE ROLE: NetSqlAzMan_Administrators

            //Sql Storage connection string
            string sqlConnectionString = "data source=(local);initial catalog=NetSqlAzManStorage;user id=netsqlazmanuser;password=password";
            //Create an instance of SqlAzManStorage class
            IAzManStorage storage = new SqlAzManStorage(sqlConnectionString);
            //Open Storage Connection
            storage.OpenConnection();
            //Begin a new Transaction
            storage.BeginTransaction(AzManIsolationLevel.ReadUncommitted);
            //Create a new Store
            IAzManStore newStore = storage.CreateStore("My Store", "Store description");
            //Create a new Basic StoreGroup
            IAzManStoreGroup newStoreGroup = newStore.CreateStoreGroup(SqlAzManSID.NewSqlAzManSid(), "My Store Group", "Store Group Description", String.Empty, GroupType.Basic);
            //Retrieve current user SID
            IAzManSid mySid = new SqlAzManSID(((System.Threading.Thread.CurrentPrincipal.Identity as WindowsIdentity) ?? WindowsIdentity.GetCurrent()).User);
            //Add myself as sid of "My Store Group"
            IAzManStoreGroupMember storeGroupMember = newStoreGroup.CreateStoreGroupMember(mySid, WhereDefined.Local, true);
            //Create a new Application
            IAzManApplication newApp = newStore.CreateApplication("New Application", "Application description");
            //Create a new Role
            IAzManItem newRole = newApp.CreateItem("New Role", "Role description", ItemType.Role);
            //Create a new Task
            IAzManItem newTask = newApp.CreateItem("New Task", "Task description", ItemType.Task);
            //Create a new Operation
            IAzManItem newOp = newApp.CreateItem("New Operation", "Operation description", ItemType.Operation);
            //Add "New Operation" as a sid of "New Task"
            newTask.AddMember(newOp);
            //Add "New Task" as a sid of "New Role"
            newRole.AddMember(newTask);
            //Create an authorization for myself on "New Role"
            IAzManAuthorization auth = newRole.CreateAuthorization(mySid, WhereDefined.Local, mySid, WhereDefined.Local, AuthorizationType.AllowWithDelegation, null, null);
            //Create a custom attribute
            IAzManAttribute<IAzManAuthorization> attr = auth.CreateAttribute("New Key", "New Value");
            //Create an authorization for DB User "Andrea" on "New Role"
            IAzManAuthorization auth2 = newRole.CreateAuthorization(mySid, WhereDefined.Local, storage.GetDBUser("Andrea").CustomSid, WhereDefined.Local, AuthorizationType.AllowWithDelegation, null, null);
            //Commit transaction
            storage.CommitTransaction();
            //Close connection
            storage.CloseConnection();
        }
    }
}
