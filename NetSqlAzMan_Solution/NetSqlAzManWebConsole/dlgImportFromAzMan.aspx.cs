using System;
using System.IO;
using Microsoft.Interop.Security.AzRoles;
using NetSqlAzMan;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzManWebConsole
{
    public partial class dlgImportFromAzMan : dlgPage
    {
        protected internal IAzManStorage storage = null;
        private IAzManSid currentOwnerSid;
        private WhereDefined currentOwnerSidWhereDefined;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.setImage("NetSqlAzMan_32x32.gif");
            this.setOkHandler(new EventHandler(this.btnOk_Click));
            this.currentOwnerSid = new SqlAzManSID(this.Request.LogonUserIdentity.User);
            try
            {
                string memberName;
                bool isLocal;
                DirectoryServicesWebUtils.GetMemberInfo(this.currentOwnerSid.StringValue, out memberName, out isLocal);
                if (!isLocal)
                {
                    this.currentOwnerSidWhereDefined = WhereDefined.LDAP;
                }
                else
                {
                    this.currentOwnerSidWhereDefined = WhereDefined.Local;
                }
            }
            catch
            {
                this.currentOwnerSidWhereDefined = WhereDefined.LDAP;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.storage = this.Session["storage"] as IAzManStorage;
            this.menuItem = Request["MenuItem"];
            this.Text = "Import Store from Microsoft Authorization Manager (AzMan)";
            this.Description = this.Text;
            this.Title = this.Text;
            this.rbtStoreFile_CheckedChanged(this, EventArgs.Empty);
            this.showWaitPanelOnSubmit(this.pnlWait, this.pnlImportFromAzMan);
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.txtAzManStorePath.Text.Trim().StartsWith("msxml://", StringComparison.CurrentCultureIgnoreCase))
                {
                    throw new InvalidOperationException("MS AzMan Network Path cannot be a server path. Use instead Xml file as source type and upload the specified Xml file.");
                }
                if (this.rbtStorePath.Checked && (!this.txtAzManStorePath.Text.Trim().StartsWith("msldap://", StringComparison.CurrentCultureIgnoreCase) && !this.txtAzManStorePath.Text.Trim().StartsWith("mssql://", StringComparison.CurrentCultureIgnoreCase)))
                {
                    throw new InvalidOperationException("MS AzMan unsupported provider type. Network path must start with MSLDAP:// or MSSQL://.");
                }
                this.ImportFromAzMan(this.txtAzManStorePath.Text, this.txtNetSqlAzManStoreName.Text);
                this.closeWindow(true);
            }
            catch (System.Runtime.InteropServices.COMException cex)
            {
                this.ShowError(cex.Message + "\\r\\n" + "Azroles.dll could not be found. Please install on IIS server Microsoft Authorization Manager components and try again");
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }

        protected void rbtStoreFile_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbtStoreFile.Checked)
            {
                this.lblStoreFile.Visible = this.FileUpload1.Visible = this.rfvFile.Visible = true;
                this.lblStorePath.Visible = this.txtAzManStorePath.Visible = this.rfvPath.Visible = this.lblDescription1.Visible = this.lblDescription2.Visible = false;
            }
            else if (this.rbtStorePath.Checked)
            {
                this.lblStorePath.Visible = this.txtAzManStorePath.Visible = this.rfvPath.Visible = this.lblDescription1.Visible = this.lblDescription2.Visible = true;
                this.lblStoreFile.Visible = this.FileUpload1.Visible = this.rfvFile.Visible = false;
            }
        }

        private void ImportFromAzMan(string azManStorePath, string netSqlAzManStoreName)
        {
            Microsoft.Interop.Security.AzRoles.AzAuthorizationStore azstore = null;
            string tempFileName = Path.Combine(Environment.GetEnvironmentVariable("temp", EnvironmentVariableTarget.Machine), String.Format("AzMan{0}.xml", Guid.NewGuid()));
            try
            {
                this.storage.BeginTransaction(AzManIsolationLevel.ReadUncommitted);
                string storeDescription = String.Format("Store imported from AzMan Store:" + " ({0}) - {1}", azManStorePath, DateTime.Now.ToString());
                IAzManStore store = this.storage.CreateStore(netSqlAzManStoreName, storeDescription);
                azstore = new AzAuthorizationStoreClass();
                if (this.rbtStoreFile.Checked)
                {
                    this.FileUpload1.SaveAs(tempFileName);
                    azManStorePath = String.Format("msxml://{0}", tempFileName);
                }
                azstore.Initialize(2, azManStorePath, null);
                #region Store Groups
                //Store Groups
                foreach (IAzApplicationGroup azStoreGroup in azstore.ApplicationGroups)
                {
                    //Store Groups Definition
                    if (azStoreGroup.Type == (int)tagAZ_PROP_CONSTANTS.AZ_GROUPTYPE_BASIC)
                    {
                        //Basic
                        store.CreateStoreGroup(SqlAzManSID.NewSqlAzManSid(), azStoreGroup.Name, azStoreGroup.Description, String.Empty, GroupType.Basic);
                    }
                    else if (azStoreGroup.Type == (int)tagAZ_PROP_CONSTANTS.AZ_GROUPTYPE_LDAP_QUERY)
                    {
                        //LDap
                        store.CreateStoreGroup(SqlAzManSID.NewSqlAzManSid(), azStoreGroup.Name, azStoreGroup.Description, azStoreGroup.LdapQuery, GroupType.LDapQuery);
                    }
                }
                //Store Groups Members
                foreach (IAzApplicationGroup azStoreGroup in azstore.ApplicationGroups)
                {
                    if (azStoreGroup.Type == (int)tagAZ_PROP_CONSTANTS.AZ_GROUPTYPE_BASIC)
                    {
                        //Basic
                        IAzManStoreGroup storeGroup = store.GetStoreGroup(azStoreGroup.Name);
                        //Store Group Members - Members Store Group
                        object[] azStoreGroupMembers = azStoreGroup.AppMembers as object[];
                        if (azStoreGroupMembers != null)
                        {
                            foreach (string azStoreGroupMember in azStoreGroupMembers)
                            {
                                IAzManStoreGroup member = store.GetStoreGroup(azStoreGroupMember);
                                storeGroup.CreateStoreGroupMember(member.SID, WhereDefined.Store, true);
                            }
                        }
                        //Store Group Non-Members - Non-Members Store Group
                        object[] azStoreGroupNonMembers = azStoreGroup.AppNonMembers as object[];
                        if (azStoreGroupNonMembers != null)
                        {
                            foreach (string azStoreGroupNonMember in azStoreGroupNonMembers)
                            {
                                IAzManStoreGroup nonMember = store.GetStoreGroup(azStoreGroupNonMember);
                                storeGroup.CreateStoreGroupMember(nonMember.SID, WhereDefined.Store, false);
                            }
                        }
                        //Store Group Members - Windows NT Account
                        object[] azStoreGroupWindowsMembers = azStoreGroup.Members as object[];
                        if (azStoreGroupWindowsMembers != null)
                        {
                            foreach (string azStoreWindowsMember in azStoreGroupWindowsMembers)
                            {
                                IAzManSid sid = new SqlAzManSID(azStoreWindowsMember);

                                string memberName;
                                bool isLocal;
                                DirectoryServicesWebUtils.GetMemberInfo(sid.StringValue, out memberName, out isLocal);
                                storeGroup.CreateStoreGroupMember(sid, isLocal ? WhereDefined.Local : WhereDefined.LDAP, true);
                            }
                        }
                        //Store Group NonMembers - Windows NT Account
                        object[] azStoreGroupWindowsNonMembers = azStoreGroup.NonMembers as object[];
                        if (azStoreGroupWindowsNonMembers != null)
                        {
                            foreach (string azStoreWindowsNonMember in azStoreGroupWindowsNonMembers)
                            {
                                IAzManSid sid = new SqlAzManSID(azStoreWindowsNonMember);
                                string memberName;
                                bool isLocal;
                                DirectoryServicesWebUtils.GetMemberInfo(sid.StringValue, out memberName, out isLocal);
                                storeGroup.CreateStoreGroupMember(sid, isLocal ? WhereDefined.Local : WhereDefined.LDAP, false);
                            }
                        }
                    }
                }
                #endregion Store Groups
                #region Applications
                //Applications
                foreach (IAzApplication azApplication in azstore.Applications)
                {
                    IAzManApplication application = store.CreateApplication(azApplication.Name, azApplication.Description);
                    #region Application Groups
                    //Store Groups
                    foreach (IAzApplicationGroup azApplicationGroup in azApplication.ApplicationGroups)
                    {
                        //Application Groups Definition
                        if (azApplicationGroup.Type == (int)tagAZ_PROP_CONSTANTS.AZ_GROUPTYPE_BASIC)
                        {
                            //Basic
                            application.CreateApplicationGroup(SqlAzManSID.NewSqlAzManSid(), azApplicationGroup.Name, azApplicationGroup.Description, String.Empty, GroupType.Basic);
                        }
                        else if (azApplicationGroup.Type == (int)tagAZ_PROP_CONSTANTS.AZ_GROUPTYPE_LDAP_QUERY)
                        {
                            //LDap
                            application.CreateApplicationGroup(SqlAzManSID.NewSqlAzManSid(), azApplicationGroup.Name, azApplicationGroup.Description, azApplicationGroup.LdapQuery, GroupType.LDapQuery);
                        }
                    }
                    //Application Groups Members
                    foreach (IAzApplicationGroup azApplicationGroup in azApplication.ApplicationGroups)
                    {
                        if (azApplicationGroup.Type == (int)tagAZ_PROP_CONSTANTS.AZ_GROUPTYPE_BASIC)
                        {
                            //Basic
                            IAzManApplicationGroup applicationGroup = application.GetApplicationGroup(azApplicationGroup.Name);
                            //Application Group Members - Members Group
                            object[] azStoreGroupMembers = azApplicationGroup.AppMembers as object[];
                            if (azStoreGroupMembers != null)
                            {
                                foreach (string azGroupMember in azStoreGroupMembers)
                                {
                                    IAzManStoreGroup storemember;
                                    try
                                    {
                                        storemember = store.GetStoreGroup(azGroupMember);
                                    }
                                    catch (SqlAzManException)
                                    {
                                        storemember = null;
                                    }
                                    IAzManApplicationGroup appmember;
                                    try
                                    {
                                        appmember = application.GetApplicationGroup(azGroupMember);
                                    }
                                    catch (SqlAzManException)
                                    {
                                        appmember = null;
                                    }
                                    if (storemember != null)
                                        applicationGroup.CreateApplicationGroupMember(storemember.SID, WhereDefined.Store, true);
                                    else
                                        applicationGroup.CreateApplicationGroupMember(appmember.SID, WhereDefined.Application, true);
                                }
                            }
                            //Application Group Non-Members - Non-Members Group
                            object[] azStoreGroupNonMembers = azApplicationGroup.AppNonMembers as object[];
                            if (azStoreGroupNonMembers != null)
                            {
                                foreach (string azGroupNonMember in azStoreGroupNonMembers)
                                {
                                    IAzManStoreGroup storenonMember;
                                    try
                                    {
                                        storenonMember = store.GetStoreGroup(azGroupNonMember);
                                    }
                                    catch (SqlAzManException)
                                    {
                                        storenonMember = null;
                                    }
                                    IAzManApplicationGroup appnonMember;
                                    try
                                    {
                                        appnonMember = application.GetApplicationGroup(azGroupNonMember);
                                    }
                                    catch (SqlAzManException)
                                    {
                                        appnonMember = null;
                                    }
                                    if (storenonMember != null)
                                        applicationGroup.CreateApplicationGroupMember(storenonMember.SID, WhereDefined.Store, false);
                                    else
                                        applicationGroup.CreateApplicationGroupMember(appnonMember.SID, WhereDefined.Application, false);
                                }
                            }
                            //Application Group Members - Windows NT Account
                            object[] azApplicationGroupWindowsMembers = azApplicationGroup.Members as object[];
                            if (azApplicationGroupWindowsMembers != null)
                            {
                                foreach (string azApplicationWindowsMember in azApplicationGroupWindowsMembers)
                                {
                                    IAzManSid sid = new SqlAzManSID(azApplicationWindowsMember);
                                    string memberName;
                                    bool isLocal;
                                    DirectoryServicesWebUtils.GetMemberInfo(sid.StringValue, out memberName, out isLocal);
                                    applicationGroup.CreateApplicationGroupMember(sid, isLocal ? WhereDefined.Local : WhereDefined.LDAP, true);
                                }
                            }
                            //Application Group NonMembers - Windows NT Account
                            object[] azApplicationGroupWindowsNonMembers = azApplicationGroup.NonMembers as object[];
                            if (azApplicationGroupWindowsNonMembers != null)
                            {
                                foreach (string azApplicationWindowsNonMember in azApplicationGroupWindowsNonMembers)
                                {
                                    IAzManSid sid = new SqlAzManSID(azApplicationWindowsNonMember);
                                    string memberName;
                                    bool isLocal;
                                    DirectoryServicesWebUtils.GetMemberInfo(sid.StringValue, out memberName, out isLocal);
                                    applicationGroup.CreateApplicationGroupMember(sid, isLocal ? WhereDefined.Local : WhereDefined.LDAP, false);
                                }
                            }
                        }
                    }
                    #endregion Application Groups
                    //Without Scopes
                    IAzTasks tasks = azApplication.Tasks as IAzTasks;
                    if (tasks != null)
                    {
                        foreach (IAzTask azTask in tasks)
                        {
                            if (azTask.IsRoleDefinition == 1)
                            {
                                IAzManItem item = application.CreateItem(azTask.Name, azTask.Description, ItemType.Role);
                            }
                            else
                            {
                                IAzManItem item = application.CreateItem(azTask.Name, azTask.Description, ItemType.Task);
                            }
                        }
                    }
                    IAzOperations operations = azApplication.Operations as IAzOperations;
                    if (operations != null)
                    {
                        foreach (IAzOperation azOperation in operations)
                        {
                            application.CreateItem(azOperation.Name, azOperation.Description, ItemType.Operation);
                        }
                    }
                    //Build Item Hierarchy
                    if (tasks != null)
                    {

                        foreach (IAzTask azTask in tasks)
                        {
                            this.SetHirearchy(null, azApplication, azTask.Name, application);
                        }
                    }
                    //Scopes
                    foreach (IAzScope azScope in azApplication.Scopes)
                    {
                        azApplication.OpenScope(azScope.Name, null);
                        IAzTasks tasksOfScope = azScope.Tasks as IAzTasks;
                        if (tasksOfScope != null)
                        {
                            foreach (IAzTask azTask in tasksOfScope)
                            {
                                if (azTask.IsRoleDefinition == 1)
                                {
                                    IAzManItem item = application.CreateItem(azTask.Name, azTask.Description, ItemType.Role);
                                }
                                else
                                {
                                    IAzManItem item = application.CreateItem(azTask.Name, azTask.Description, ItemType.Task);
                                }
                            }
                        }
                        //Build Item Hierarchy
                        if (tasksOfScope != null)
                        {

                            foreach (IAzTask azTask in tasksOfScope)
                            {
                                this.SetHirearchy(azScope, azApplication, azTask.Name, application);
                            }
                        }
                    }
                    //Authorizations on Roles without Scopes
                    AuthorizationType defaultAuthorization = AuthorizationType.AllowWithDelegation;
                    IAzRoles azRoles = azApplication.Roles;
                    foreach (IAzRole azRole in azRoles)
                    {
                        IAzManItem item;
                        try
                        {
                            item = application.GetItem(azRole.Name);
                        }
                        catch (SqlAzManException)
                        {
                            item = null;
                        }
                        if (item == null)
                            item = application.CreateItem(azRole.Name, azRole.Description, ItemType.Role);
                        //Store & Application Groups Authorizations
                        foreach (string member in (object[])azRole.AppMembers)
                        {
                            IAzManStoreGroup storeGroup;
                            try
                            {
                                storeGroup = application.Store.GetStoreGroup(member);
                            }
                            catch (SqlAzManException)
                            {
                                storeGroup = null;
                            }
                            IAzManApplicationGroup applicationGroup;
                            try
                            {
                                applicationGroup = application.GetApplicationGroup(member);
                            }
                            catch (SqlAzManException)
                            {
                                applicationGroup = null;
                            }
                            if (storeGroup != null)
                                item.CreateAuthorization(this.currentOwnerSid, this.currentOwnerSidWhereDefined, storeGroup.SID, WhereDefined.Store, defaultAuthorization, null, null);
                            else if (applicationGroup != null)
                                item.CreateAuthorization(this.currentOwnerSid, this.currentOwnerSidWhereDefined, applicationGroup.SID, WhereDefined.Application, defaultAuthorization, null, null);
                        }
                        //Windows Users & Groups Authorizations
                        foreach (string sSid in (object[])azRole.Members)
                        {
                            IAzManSid sid = new SqlAzManSID(sSid);
                            string memberName;
                            bool isLocal;
                            DirectoryServicesWebUtils.GetMemberInfo(sid.StringValue, out memberName, out isLocal);
                            item.CreateAuthorization(this.currentOwnerSid, this.currentOwnerSidWhereDefined, sid, isLocal ? WhereDefined.Local : WhereDefined.LDAP, defaultAuthorization, null, null);
                        }
                    }
                    //Authorizations on Roles with Scopes
                    foreach (IAzScope azScope in azApplication.Scopes)
                    {
                        IAzRoles azRolesWithScopes = azScope.Roles;
                        foreach (IAzRole azRole in azRolesWithScopes)
                        {
                            IAzManItem item;
                            try
                            {
                                item = application.GetItem(azRole.Name);
                            }
                            catch (SqlAzManException)
                            {
                                item = null;
                            }
                            if (item == null)
                                item = application.CreateItem(azRole.Name, azRole.Description, ItemType.Role);
                            //Store & Application Groups Authorizations
                            foreach (string member in (object[])azRole.AppMembers)
                            {
                                IAzManStoreGroup storeGroup;
                                try
                                {
                                    storeGroup = application.Store.GetStoreGroup(member);
                                }
                                catch (SqlAzManException)
                                {
                                    storeGroup = null;
                                }
                                IAzManApplicationGroup applicationGroup;
                                try
                                {
                                    applicationGroup = application.GetApplicationGroup(member);
                                }
                                catch (SqlAzManException)
                                {
                                    applicationGroup = null;
                                }
                                if (storeGroup != null)
                                    item.CreateAuthorization(this.currentOwnerSid, this.currentOwnerSidWhereDefined, storeGroup.SID, WhereDefined.Store, defaultAuthorization, null, null);
                                else if (applicationGroup != null)
                                    item.CreateAuthorization(this.currentOwnerSid, this.currentOwnerSidWhereDefined, applicationGroup.SID, WhereDefined.Application, defaultAuthorization, null, null);
                            }
                            //Windows Users & Groups Authorizations
                            foreach (string sSid in (object[])azRole.Members)
                            {
                                IAzManSid sid = new SqlAzManSID(sSid);
                                string memberName;
                                bool isLocal;
                                DirectoryServicesWebUtils.GetMemberInfo(sid.StringValue, out memberName, out isLocal);
                                item.CreateAuthorization(this.currentOwnerSid, this.currentOwnerSidWhereDefined, sid, isLocal ? WhereDefined.Local : WhereDefined.LDAP, defaultAuthorization, null, null);
                            }
                        }
                    }
                    //try
                    //{
                    //    azstore.CloseApplication(azApplication.Name, 0);
                    //}
                    //catch 
                    //{ 
                    //    //PorkAround: COM Is a mistery  
                    //}
                }
                #endregion Applications
                if (storage.TransactionInProgress)
                    storage.CommitTransaction();
            }
            catch
            {
                if (storage.TransactionInProgress)
                {
                    storage.RollBackTransaction();
                }
                throw;
            }
            finally
            {
                if (azstore != null)
                {
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(azstore);
                    File.Delete(tempFileName);
                    azstore = null;
                }
            }
        }

        private void SetHirearchy(IAzScope azScope, IAzApplication azApplication, string taskName, IAzManApplication application)
        {
            IAzTask azTask = null;
            if (azScope == null)
                azTask = azApplication.OpenTask(taskName, null);
            else
                azTask = azScope.OpenTask(taskName, null);

            IAzManItem item = application.GetItem(taskName);
            if (azTask != null)
            {
                //SubTasks
                object[] azSubTasks = azTask.Tasks as object[];
                if (azSubTasks != null)
                {
                    foreach (string azSubTask in azSubTasks)
                    {
                        IAzManItem subItem = application.GetItem(azSubTask);
                        item.AddMember(subItem);
                        //this.SetHirearchy(azScope, azApplication, azSubTask, application);
                    }
                }
                //SubOperations
                object[] azSubOperations = azTask.Operations as object[];
                if (azSubOperations != null)
                {
                    foreach (string azSubOperation in azSubOperations)
                    {
                        IAzManItem subItem = application.GetItem(azSubOperation);
                        item.AddMember(subItem);
                    }
                }
            }
        }
    }
}
