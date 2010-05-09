using System;
using System.Linq;
using System.Security.Principal;
using System.Windows.Forms;
using Microsoft.Interop.Security.AzRoles;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.SnapIn.DirectoryServices;

namespace NetSqlAzMan.SnapIn.Forms
{
    public partial class frmImportFromAzMan : frmBase
    {
        internal IAzManStorage storage;
        private IAzManSid currentOwnerSid = new SqlAzManSID(((System.Threading.Thread.CurrentPrincipal.Identity as WindowsIdentity) ?? WindowsIdentity.GetCurrent()).User.Value);
        private WhereDefined currentOwnerSidWhereDefined;
        public frmImportFromAzMan()
        {
            InitializeComponent();

            try
            {
                string memberName;
                bool isLocal;
                DirectoryServicesUtils.GetMemberInfo(this.currentOwnerSid.StringValue, out memberName, out isLocal);
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

        private void frmImportFromAzMan_Load(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
            this.ValidateForm();
            NetSqlAzMan.SnapIn.Globalization.ResourcesManager.CollectResources(this);
        }

        protected void HourGlass(bool switchOn)
        {
            this.Cursor = switchOn ? Cursors.WaitCursor : Cursors.Arrow;
            /*Application.DoEvents();*/
        }

        protected void ShowError(string text, string caption)
        {
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        protected void ShowInfo(string text, string caption)
        {
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected void ShowWarning(string text, string caption)
        {
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void frmImportFromAzMan_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.None)
                this.DialogResult = DialogResult.Cancel;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (!this.ValidateForm())
                return;
            try
            {
                this.txtAzManStorePath.ReadOnly = this.txtNetSqlAzManStoreName.ReadOnly = true;
                this.btnImport.Enabled = this.btnClose.Enabled = false;
                this.picImporting.Visible = true;
                this.HourGlass(true);
                this.ImportFromAzMan(this.txtAzManStorePath.Text, this.txtNetSqlAzManStoreName.Text);
                this.DialogResult = DialogResult.OK;
            }
            catch (System.Runtime.InteropServices.COMException cex)
            {
                this.HourGlass(false);
                this.ShowError(cex.Message + "\r\n\r\n" + Globalization.MultilanguageResource.GetString("frmImportFromAzMan_Msg10"), Globalization.MultilanguageResource.GetString("frmImportFromAzMan_Tit10"));
            }
            catch (Exception ex)
            {
                this.HourGlass(false);
                this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("frmImportFromAzMan_Tit10"));
            }
            finally
            {
                this.txtAzManStorePath.ReadOnly = this.txtNetSqlAzManStoreName.ReadOnly = false;
                this.btnImport.Enabled = this.btnClose.Enabled = true;
                this.picImporting.Visible = false;
            }
        }

        private bool ValidateForm()
        {
            bool isValid = true;
            if (isValid && String.IsNullOrEmpty(this.txtAzManStorePath.Text.Trim()))
            {
                this.errorProvider1.SetError(this.txtAzManStorePath, Globalization.MultilanguageResource.GetString("frmImportFromAzMan_Msg20"));
                isValid = false;
            }
            else
            {
                this.errorProvider1.SetError(this.txtAzManStorePath, String.Empty);
            }
            if (isValid && !this.txtAzManStorePath.Text.Trim().ToLower().StartsWith("msxml://") && !this.txtAzManStorePath.Text.Trim().ToLower().StartsWith("msldap://") && !this.txtAzManStorePath.Text.Trim().ToLower().StartsWith("mssql://"))
            {
                this.errorProvider1.SetError(this.txtAzManStorePath, Globalization.MultilanguageResource.GetString("frmImportFromAzMan_Msg30"));
                isValid = false;
            }
            else
            {
                this.errorProvider1.SetError(this.txtAzManStorePath, String.Empty);
            }
            if (isValid && String.IsNullOrEmpty(this.txtNetSqlAzManStoreName.Text.Trim()))
            {
                this.errorProvider1.SetError(this.txtNetSqlAzManStoreName, Globalization.MultilanguageResource.GetString("frmImportFromAzMan_Msg40"));
                isValid = false;
            }
            else
            {
                this.errorProvider1.SetError(this.txtNetSqlAzManStoreName, String.Empty);
            }
            return this.btnImport.Enabled = isValid;
        }

        private void txtAzManStorePath_TextChanged(object sender, EventArgs e)
        {
            this.ValidateForm();
        }

        private void txtNetSqlAzManStoreName_TextChanged(object sender, EventArgs e)
        {
            this.ValidateForm();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void SetMessage(string message)
        {
            this.lblStatus.Text = message;
            /*Application.DoEvents();*/
        }

        private void ImportFromAzMan(string azManStorePath, string netSqlAzManStoreName)
        {
            Microsoft.Interop.Security.AzRoles.AzAuthorizationStore azstore = null;
            try
            {
                this.SetMessage(Globalization.MultilanguageResource.GetString("frmImportFromAzMan_Msg50"));
                this.storage.BeginTransaction(AzManIsolationLevel.ReadUncommitted);
                string storeDescription = String.Format(Globalization.MultilanguageResource.GetString("frmImportFromAzMan_Msg60") +" ({0}) - {1}", azManStorePath, DateTime.Now.ToString());
                IAzManStore store = this.storage.CreateStore(netSqlAzManStoreName, storeDescription);
                azstore = new AzAuthorizationStoreClass();
                this.SetMessage(String.Format(Globalization.MultilanguageResource.GetString("frmImportFromAzMan_Msg70") +" '{0}'", azManStorePath));
                azstore.Initialize(2, azManStorePath, null);
                #region Store Groups
                //Store Groups
                foreach (IAzApplicationGroup azStoreGroup in azstore.ApplicationGroups)
                {
                    //Store Groups Definition
                    this.SetMessage(String.Format("Store Group: '{0}'", azStoreGroup.Name));
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
                        this.SetMessage(String.Format("Store Group: '{0}' Members", storeGroup.Name));
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
                        this.SetMessage(String.Format("Store Group: '{0}' Non-Members", storeGroup.Name));
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
                        this.SetMessage(String.Format("Store Group: '{0}' Windows account Members", storeGroup.Name));
                        object[] azStoreGroupWindowsMembers = azStoreGroup.Members as object[];
                        if (azStoreGroupWindowsMembers != null)
                        {
                            foreach (string azStoreWindowsMember in azStoreGroupWindowsMembers)
                            {
                                IAzManSid sid = new SqlAzManSID(azStoreWindowsMember);

                                string memberName;
                                bool isLocal;
                                DirectoryServicesUtils.GetMemberInfo(sid.StringValue, out memberName, out isLocal);
                                storeGroup.CreateStoreGroupMember(sid, isLocal ? WhereDefined.Local : WhereDefined.LDAP, true);
                            }
                        }
                        //Store Group NonMembers - Windows NT Account
                        this.SetMessage(String.Format("Store Group: '{0}' Windows account Non-Members", storeGroup.Name));
                        object[] azStoreGroupWindowsNonMembers = azStoreGroup.NonMembers as object[];
                        if (azStoreGroupWindowsNonMembers != null)
                        {
                            foreach (string azStoreWindowsNonMember in azStoreGroupWindowsNonMembers)
                            {
                                IAzManSid sid = new SqlAzManSID(azStoreWindowsNonMember);
                                string memberName;
                                bool isLocal;
                                DirectoryServicesUtils.GetMemberInfo(sid.StringValue, out memberName, out isLocal);
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
                    this.SetMessage(String.Format("Application: '{0}'", azApplication.Name));
                    IAzManApplication application = store.CreateApplication(azApplication.Name, azApplication.Description);
                    #region Application Groups
                    //Store Groups
                    foreach (IAzApplicationGroup azApplicationGroup in azApplication.ApplicationGroups)
                    {
                        //Application Groups Definition
                        this.SetMessage(String.Format("Application Group: '{0}'", azApplicationGroup.Name));
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
                            this.SetMessage(String.Format("Application Group: '{0}' Members", applicationGroup.Name));
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
                            this.SetMessage(String.Format("Application Group: '{0}' Non-Members", applicationGroup.Name));
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
                            this.SetMessage(String.Format("Application Group: '{0}' Windows account Members", applicationGroup.Name));
                            object[] azApplicationGroupWindowsMembers = azApplicationGroup.Members as object[];
                            if (azApplicationGroupWindowsMembers != null)
                            {
                                foreach (string azApplicationWindowsMember in azApplicationGroupWindowsMembers)
                                {
                                    IAzManSid sid = new SqlAzManSID(azApplicationWindowsMember);
                                    string memberName;
                                    bool isLocal;
                                    DirectoryServicesUtils.GetMemberInfo(sid.StringValue, out memberName, out isLocal);
                                    applicationGroup.CreateApplicationGroupMember(sid, isLocal ? WhereDefined.Local : WhereDefined.LDAP, true);
                                }
                            }
                            //Application Group NonMembers - Windows NT Account
                            this.SetMessage(String.Format("Application Group: '{0}' Windows account Non-Members", applicationGroup.Name));
                            object[] azApplicationGroupWindowsNonMembers = azApplicationGroup.NonMembers as object[];
                            if (azApplicationGroupWindowsNonMembers != null)
                            {
                                foreach (string azApplicationWindowsNonMember in azApplicationGroupWindowsNonMembers)
                                {
                                    IAzManSid sid = new SqlAzManSID(azApplicationWindowsNonMember);
                                    string memberName;
                                    bool isLocal;
                                    DirectoryServicesUtils.GetMemberInfo(sid.StringValue, out memberName, out isLocal);
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
                                this.SetMessage(String.Format("Role: '{0}'", azTask.Name));
                                IAzManItem item = application.CreateItem(azTask.Name, azTask.Description, ItemType.Role);
                            }
                            else
                            {
                                this.SetMessage(String.Format("Task: '{0}'", azTask.Name));
                                IAzManItem item = application.CreateItem(azTask.Name, azTask.Description, ItemType.Task);
                            }
                        }
                    }
                    IAzOperations operations = azApplication.Operations as IAzOperations;
                    if (operations != null)
                    {
                        foreach (IAzOperation azOperation in operations)
                        {
                            this.SetMessage(String.Format("Operation: '{0}'", azOperation.Name));
                            application.CreateItem(azOperation.Name, azOperation.Description, ItemType.Operation);
                        }
                    }
                    //Build Item Hierarchy
                    if (tasks != null)
                    {

                        foreach (IAzTask azTask in tasks)
                        {
                            this.SetMessage(String.Format("Task: '{0}'", azTask.Name));
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
                                    this.SetMessage(String.Format("Role: '{0}'", azTask.Name));
                                    IAzManItem item = application.CreateItem(azTask.Name, azTask.Description, ItemType.Role);
                                }
                                else
                                {
                                    this.SetMessage(String.Format("Task: '{0}'", azTask.Name));
                                    IAzManItem item = application.CreateItem(azTask.Name, azTask.Description, ItemType.Task);
                                }
                            }
                        }
                        //Build Item Hierarchy
                        if (tasksOfScope != null)
                        {

                            foreach (IAzTask azTask in tasksOfScope)
                            {
                                this.SetMessage(String.Format("Task: '{0}'", azTask.Name));
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
                            DirectoryServicesUtils.GetMemberInfo(sid.StringValue, out memberName, out isLocal);
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
                                DirectoryServicesUtils.GetMemberInfo(sid.StringValue, out memberName, out isLocal);
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
                this.SetMessage(Globalization.MultilanguageResource.GetString("frmImportFromAzMan_Msg80"));
                if (storage.TransactionInProgress)
                    storage.CommitTransaction();
            }
            catch
            {
                if (storage.TransactionInProgress)
                {
                    this.SetMessage(Globalization.MultilanguageResource.GetString("frmImportFromAzMan_Msg90"));
                    storage.RollBackTransaction();
                }
                throw;
            }
            finally
            {
                if (azstore != null)
                {
                    this.SetMessage(Globalization.MultilanguageResource.GetString("frmImportFromAzMan_Msg100"));
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(azstore);
                    azstore = null;
                }
                this.SetMessage(Globalization.MultilanguageResource.GetString("Done_Msg10"));
            }
        }

        private void SetHirearchy(IAzScope azScope, IAzApplication azApplication, string taskName, IAzManApplication application)
        {
            IAzTask azTask = null;
            if (azScope == null)
                azTask = azApplication.OpenTask(taskName, null);
            else
                azTask = azScope.OpenTask(taskName, null);

            if (azTask != null)
            {
                IAzManItem item = application.GetItem(taskName);
                //SubTasks
                object[] azSubTasks = azTask.Tasks as object[];
                if (azSubTasks != null)
                {
                    foreach (string azSubTask in azSubTasks)
                    {
                        IAzManItem subItem = application.GetItem(azSubTask);
                        var members = item.GetMembers();
                        if (members == null || members.Where(t => t.ItemId == subItem.ItemId).Count() == 0)
                            item.AddMember(subItem);
                        this.SetHirearchy(azScope, azApplication, azSubTask, application);
                    }
                }
                //SubOperations
                object[] azSubOperations = azTask.Operations as object[];
                if (azSubOperations != null)
                {
                    foreach (string azSubOperation in azSubOperations)
                    {
                        IAzManItem subItem = application.GetItem(azSubOperation);
                        var members = item.GetMembers();
                        if (members == null || members.Where(t => t.ItemId == subItem.ItemId).Count() == 0)
                            item.AddMember(subItem);
                    }
                }
            }
        }
    }
}