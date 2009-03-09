using System;
using System.IO;
using System.Xml;
using System.Security.Principal;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using NetSqlAzMan;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.Cache;

namespace NetSqlAzManWebConsole
{
    public partial class dlgCheckAccessTest : dlgPage
    {
        internal IAzManApplication application = null;
        private WindowsIdentity wid = null;
        private IAzManDBUser dbuser = null;
        private UserPermissionCache cache = null;
        private StorageCache storageCache = null;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.setImage("CheckAccessTest.gif");
            this.showCloseOnly();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.application = this.Session["selectedObject"] as IAzManApplication;
            this.Session["application"] = this.application;
            this.Text = "Check Access Test";
            this.Description = this.Text;
            this.Title = this.Text;
            this.showWaitPanelOnSubmit(this.pnlWait, this.pnlCheckAccessTest);
            if (!Page.IsPostBack)
            {
                this.wid = this.Request.LogonUserIdentity;
                NTAccount nta = (NTAccount)this.wid.User.Translate(typeof(NTAccount));
                string currentUpnName = nta.Value;
                if (currentUpnName.IndexOf('\\') != -1)
                {
                    currentUpnName = currentUpnName.Substring(currentUpnName.IndexOf('\\') + 1);
                }
                this.dtValidFor.Text = DateTime.Now.ToString();
                this.rbCheckedChanged();
                this.txtWindowsUser.Text = currentUpnName;
                this.txtWindowsUser.Focus();
            }
            if (this.Session["selectedDBUsers"] != null)
                this.btnBrowseDBUser_Click(this, EventArgs.Empty);
            DateTime odt;
            if (!DateTime.TryParse(this.dtValidFor.Text, out odt))
            {
                this.ShowError("Valid For must be a valid Date.");
                this.dtValidFor.Focus();
                return;
            }
        }

        private void RefreshItemsHierarchy()
        {
            this.itemsHierarchyTreeView.Nodes.Clear();
            this.buildApplicationsTreeView();
            this.itemsHierarchyTreeView.ExpandAll();
        }

        //private void btnBrowseWindowsUser_Click(object sender, EventArgs e)
        //{
        //    string userName = String.Empty;
        //    try
        //    {
        //        this.rbWindowsUser.Checked = true;
        //        ADObject[] res = DirectoryServicesWebUtils.ADObjectPickerShowDialog(this.Handle, false, true, false);
        //        Application.DoEvents();
        //        if (res != null)
        //        {
        //            if (res.Length > 1)
        //            {
        //                this.ShowError(Globalization.MultilanguageResource.GetString("frmCheckAccessTest_Msg20"), Globalization.MultilanguageResource.GetString("frmCheckAccessTest_Msg10"));
        //            }
        //            if (res.Length == 1)
        //            {
        //                userName = res[0].UPN;
        //                this.wid = new WindowsIdentity(userName);
        //                this.txtWindowsUser.Text = userName;
        //                this.dbuser = null;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("frmCheckAccessTest_Msg10"));
        //    }
        //}

        protected void rbWindowsUser_CheckedChanged(object sender, EventArgs e)
        {
            this.rbCheckedChanged();
            this.wid = this.Request.LogonUserIdentity;
            NTAccount nta = (NTAccount)this.wid.User.Translate(typeof(NTAccount));
            string currentUpnName = nta.Value;
            if (currentUpnName.IndexOf('\\') != -1)
            {
                currentUpnName = currentUpnName.Substring(currentUpnName.IndexOf('\\') + 1);
            }
            this.txtWindowsUser.Text = currentUpnName;
        }

        private void rbCheckedChanged()
        {
            if (this.rbWindowsUser.Checked)
            {
                this.txtWindowsUser.Enabled = true;
                this.txtDBUser.Enabled = false;
            }
            else
            {
                this.txtDBUser.Enabled = false;
                this.txtWindowsUser.Enabled = false;
            }
            this.txtWindowsUser.Text = String.Empty;
            this.txtDBUser.Text = String.Empty;
            this.cache = null;
            this.wid = null;
            this.dbuser = null;
        }

        protected void rbDBUser_CheckedChanged(object sender, EventArgs e)
        {
            this.rbCheckedChanged();
        }


        protected void btnCheckAccessTest_Click(object sender, EventArgs e)
        {
            try
            {
                NTAccount nta = (NTAccount)(this.Request.LogonUserIdentity.User.Translate(typeof(NTAccount)));
                string currentUpnName = nta.Value;
                if (currentUpnName.IndexOf('\\') != -1)
                {
                    currentUpnName = currentUpnName.Substring(currentUpnName.IndexOf('\\') + 1);
                }
                if (String.IsNullOrEmpty(this.txtWindowsUser.Text) && this.rbWindowsUser.Checked)
                {
                    this.txtWindowsUser.Text = currentUpnName;
                }
                if (this.rbWindowsUser.Checked)
                {
                    this.dbuser = null;
                    if (this.txtWindowsUser.Text == currentUpnName)
                    {
                        //Current Windows User
                        this.wid = this.Request.LogonUserIdentity;
                    }
                    else
                    {
                        //Kerberos Protocol Transition
                        this.wid = new WindowsIdentity(this.txtWindowsUser.Text);
                    }
                }
                else if (this.rbDBUser.Checked && !String.IsNullOrEmpty(this.txtDBUser.Text))
                {
                    this.wid = null;
                    this.dbuser = this.application.GetDBUser(this.txtDBUser.Text);
                }
                this.txtDetails.Text = String.Empty;
                this.WriteLineDetailMessage("Check Access Test started at " + DateTime.Now.ToString());
                this.WriteLineDetailMessage(String.Empty);
                this.WriteIdentityDetails();
                this.WriteLineDetailMessage(String.Empty);
                this.WriteDetailMessage("Building Items Hierarchy ...");
                this.RefreshItemsHierarchy();
                this.WriteLineDetailMessage("Done.");
                this.WriteLineDetailMessage(String.Empty);
                TreeNode applicationTreeNode = this.itemsHierarchyTreeView.Nodes[0].ChildNodes[0].ChildNodes[0];
                foreach (TreeNode itemTreeNode in applicationTreeNode.ChildNodes)
                {
                    this.checkAccessTest(itemTreeNode);
                }
                this.WriteLineDetailMessage(String.Empty);
                this.WriteLineDetailMessage("Check Access Test finished at " + DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }

        private void WriteIdentityDetails()
        {
            if (this.wid != null)
            {
                this.WriteLineDetailMessage("Identity Details:");
                SecurityIdentifier sid = this.wid.User;
                NTAccount nta = (NTAccount)sid.Translate(typeof(NTAccount));
                this.WriteLineDetailMessage(String.Format("{0}: {1} {2}: {3}", "Name", nta.Value, "Object SID", sid.Value));
                this.WriteLineDetailMessage(String.Format("{0}: {1}", "Groups", this.wid.Groups.Count));
                int index = 0;
                foreach (SecurityIdentifier groupSid in this.wid.Groups)
                {

                    SecurityIdentifier sidA = groupSid;
                    try
                    {
                        NTAccount ntGA = (NTAccount)sidA.Translate(typeof(NTAccount));
                        this.WriteLineDetailMessage(String.Format("{0}) {1} - ({2})", (++index), ntGA.Value, sidA.Value));
                    }
                    catch (Exception ex)
                    {
                        this.WriteLineDetailMessage(String.Format("{0}) {1} - ({2})", (++index), ex.Message.Replace("\r\n", " "), sidA.Value));
                    }
                }
                this.WriteLineDetailMessage(String.Empty);
                this.WriteLineDetailMessage("Member of these Store Groups:");
                foreach (IAzManStoreGroup storeGroup in this.application.Store.GetStoreGroups())
                {
                    if (storeGroup.IsInGroup(this.wid))
                    {
                        this.WriteLineDetailMessage(storeGroup.Name);
                    }
                }
                this.WriteLineDetailMessage(String.Empty);
                this.WriteLineDetailMessage("Member of these Application Groups:");
                foreach (IAzManApplicationGroup applicationGroup in this.application.GetApplicationGroups())
                {
                    if (applicationGroup.IsInGroup(this.wid))
                    {
                        this.WriteLineDetailMessage(applicationGroup.Name);
                    }
                }
            }
            else if (this.dbuser != null)
            {
                this.WriteLineDetailMessage("Identity Details:");
                this.WriteLineDetailMessage(String.Format("{0}: {1}\t{2}: {3}", "Name", this.dbuser.UserName, "Custom Sid", this.dbuser.CustomSid.StringValue));
                this.WriteLineDetailMessage("Member of these Store Groups:");
                foreach (IAzManStoreGroup storeGroup in this.application.Store.GetStoreGroups())
                {
                    if (storeGroup.IsInGroup(this.dbuser))
                    {
                        this.WriteLineDetailMessage(storeGroup.Name);
                    }
                }
                this.WriteLineDetailMessage("Member of these Application Groups:");
                foreach (IAzManApplicationGroup applicationGroup in this.application.GetApplicationGroups())
                {
                    if (applicationGroup.IsInGroup(this.dbuser))
                    {
                        this.WriteLineDetailMessage(applicationGroup.Name);
                    }
                }
            }
        }

        private void checkAccessTest(TreeNode tn)
        {
            string sItemType = String.Empty;
            if (tn.ImageUrl.EndsWith("Role_16x16.gif"))
                sItemType = "Role";
            else if (tn.ImageUrl.EndsWith("Task_16x16.gif"))
                sItemType = "Task";
            else
                sItemType = "Operation";
            AuthorizationType auth = AuthorizationType.Neutral;
            string sAuth = String.Empty;
            DateTime chkStart = DateTime.Now;
            TimeSpan elapsedTime = TimeSpan.Zero;
            DateTime chkEnd = DateTime.Now;
            List<KeyValuePair<string, string>> attributes = null;
            //Cache Build
            if (this.chkCache.Checked && this.cache == null)
            {
                this.WriteDetailMessage("Building UserPermissionCache ...");
                if (this.wid != null)
                {
                    this.cache = new NetSqlAzMan.Cache.UserPermissionCache(this.application.Store.Storage, this.application.Store.Name, this.application.Name, this.wid, true, false);
                }
                else if (this.dbuser != null)
                {
                    this.cache = new NetSqlAzMan.Cache.UserPermissionCache(this.application.Store.Storage, this.application.Store.Name, this.application.Name, this.dbuser, true, false);
                }
                chkEnd = DateTime.Now;
                elapsedTime = (TimeSpan)chkEnd.Subtract(chkStart);
                this.WriteLineDetailMessage(String.Format("[{0} mls.]\r\n", elapsedTime.TotalMilliseconds));
            }
            else if (this.chkStorageCache.Checked && this.storageCache == null)
            {
                this.WriteDetailMessage("Building StorageCache ...");
                this.storageCache = new NetSqlAzMan.Cache.StorageCache(this.application.Store.Storage.ConnectionString);
                this.storageCache.BuildStorageCache(this.application.Store.Name, this.application.Name);
                chkEnd = DateTime.Now;
                elapsedTime = (TimeSpan)chkEnd.Subtract(chkStart);
                this.WriteLineDetailMessage(String.Format("[{0} mls.]\r\n", elapsedTime.TotalMilliseconds));
            }
            chkStart = DateTime.Now;
            elapsedTime = TimeSpan.Zero;
            this.WriteDetailMessage(String.Format("{0} {1} '{2}' ... ", "Check Access Test on", sItemType, tn.Text));
            try
            {
                if (this.wid != null)
                {
                    if (this.chkCache.Checked)
                    {
                        auth = this.cache.CheckAccess(tn.Text, !String.IsNullOrEmpty(this.dtValidFor.Text) ? Convert.ToDateTime(this.dtValidFor.Text) : DateTime.Now, out attributes);
                    }
                    else if (this.chkStorageCache.Checked)
                    {
                        auth = this.storageCache.CheckAccess(this.application.Store.Name, this.application.Name, tn.Text, this.wid.GetUserBinarySSid(), this.wid.GetGroupsBinarySSid(), !String.IsNullOrEmpty(this.dtValidFor.Text) ? Convert.ToDateTime(this.dtValidFor.Text) : DateTime.Now, false, out attributes);
                    }
                    else
                    {
                        auth = this.application.Store.Storage.CheckAccess(this.application.Store.Name, this.application.Name, tn.Text, this.wid, !String.IsNullOrEmpty(this.dtValidFor.Text) ? Convert.ToDateTime(this.dtValidFor.Text) : DateTime.Now, false, out attributes);
                    }
                }
                else if (this.dbuser != null)
                {
                    if (this.chkCache.Checked)
                    {
                        auth = this.cache.CheckAccess(tn.Text, !String.IsNullOrEmpty(this.dtValidFor.Text) ? Convert.ToDateTime(this.dtValidFor.Text) : DateTime.Now, out attributes);
                    }
                    else if (this.chkStorageCache.Checked)
                    {
                        auth = this.storageCache.CheckAccess(this.application.Store.Name, this.application.Name, tn.Text, this.dbuser.CustomSid.StringValue, !String.IsNullOrEmpty(this.dtValidFor.Text) ? Convert.ToDateTime(this.dtValidFor.Text) : DateTime.Now, false, out attributes);
                    }
                    else
                    {
                        auth = this.application.Store.Storage.CheckAccess(this.application.Store.Name, this.application.Name, tn.Text, this.dbuser, !String.IsNullOrEmpty(this.dtValidFor.Text) ? Convert.ToDateTime(this.dtValidFor.Text) : DateTime.Now, false, out attributes);
                    }
                }
                chkEnd = DateTime.Now;
                elapsedTime = (TimeSpan)chkEnd.Subtract(chkStart);
                sAuth = "Neutral";
                switch (auth)
                {
                    case AuthorizationType.AllowWithDelegation:
                        sAuth = "Allow with Delegation";
                        break;
                    case AuthorizationType.Allow:
                        sAuth = "Allow";
                        break;
                    case AuthorizationType.Deny:
                        sAuth = "Deny";
                        break;
                    case AuthorizationType.Neutral:
                        sAuth = "Neutral";
                        break;
                }
                tn.ToolTip = sAuth;
                this.WriteLineDetailMessage(String.Format("{0} [{1} mls.]", sAuth, elapsedTime.TotalMilliseconds));
                if (attributes != null && attributes.Count > 0)
                {
                    this.WriteLineDetailMessage(String.Format(" {0} attribute(s) found:", attributes.Count));
                    int attributeIndex = 0;
                    foreach (KeyValuePair<string, string> attr in attributes)
                    {
                        this.WriteLineDetailMessage(String.Format("  {0}) Key: {1} Value: {2}", ++attributeIndex, attr.Key, attr.Value));
                    }
                }
            }
            catch (Exception ex)
            {
                sAuth = "Check Access Test Error";
                this.WriteLineDetailMessage(String.Format("{0} [{1} mls.]", ex.Message, elapsedTime.TotalMilliseconds));
            }
            tn.Text = String.Format("{0} - ({1})", tn.Text, sAuth.ToUpper());
            foreach (TreeNode tnChild in tn.ChildNodes)
            {
                this.checkAccessTest(tnChild);
            }
        }

        private void WriteDetailMessage(string message)
        {
            this.txtDetails.Text += message;
            this.lblMessage.Text = message.Replace("\r\n", "");
        }

        private void WriteLineDetailMessage(string message)
        {
            this.WriteDetailMessage(message + "\r\n");
        }

        protected void btnBrowseDBUser_Click(object sender, EventArgs e)
        {
            try
            {
                IAzManDBUser[] selectedDBUsers = (IAzManDBUser[])this.Session["selectedDBUsers"];
                if (selectedDBUsers.Length > 1)
                {
                    this.ShowError("Please choose only one DB User");
                }
                if (selectedDBUsers.Length == 1)
                {
                    this.wid = null;
                    this.dbuser = selectedDBUsers[0];
                    this.txtDBUser.Text = this.dbuser.UserName;
                }
                this.rbDBUser.Checked = true;
                this.txtWindowsUser.Text = String.Empty;
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
            finally
            {
                this.Session["selectedDBUsers"] = null;
            }
        }

        #region Build Items Hierarchy

        internal bool findNode(TreeNode startingNode, string text)
        {
            foreach (TreeNode childNode in startingNode.ChildNodes)
            {
                if (childNode.Text.Equals(text))
                    return true;
                if (this.findNode(childNode, text))
                    return true;
            }
            return false;
        }

        internal TreeNode findTreeNode(TreeNode startingNode, string text)
        {
            if (String.Compare(startingNode.Text, text, true) == 0)
                return startingNode;
            foreach (TreeNode childNode in startingNode.ChildNodes)
            {
                TreeNode result = this.findTreeNode(childNode, text);
                if (result != null)
                    return result;
            }
            return null;
        }

        internal protected void buildApplicationsTreeView()
        {
            this.application.Store.Storage.OpenConnection();
            this.itemsHierarchyTreeView.Nodes.Clear();
            TreeNode root = new TreeNode("NetSqlAzMan", "NetSqlAzMan", this.getImageUrl("NetSqlAzMan_16x16.gif"));
            root.ToolTip = ".NET Sql Authorization Manager";
            this.itemsHierarchyTreeView.Nodes.Add(root);
            TreeNode storeNode = new TreeNode(this.application.Store.Name, this.application.Store.Name, this.getImageUrl("Store_16x16.gif"));
            root.ChildNodes.Add(storeNode);
            root.Expand();
            storeNode.Expand();
            storeNode.ToolTip = this.application.Store.Description;
            this.add(storeNode, this.application);
            storeNode.Expand();
            this.application.Store.Storage.CloseConnection();
            root.Expand();
            storeNode.Expand();
        }

        private void add(TreeNode parent, IAzManApplication app)
        {
            TreeNode node = new TreeNode(app.Name, app.Name, this.getImageUrl("Application_16x16.gif"));
            node.ToolTip = app.Description;
            parent.ChildNodes.Add(node);
            node.Expand();
            foreach (IAzManItem item in app.Items.Values)
            {
                if (item.ItemType == ItemType.Role)
                {
                    if (item.ItemsWhereIAmAMember.Count == 0) this.AddRole(node, item, node);
                }
            }
            foreach (IAzManItem item in app.Items.Values)
            {
                if (item.ItemType == ItemType.Task)
                {
                    if (item.ItemsWhereIAmAMember.Count == 0) this.AddTask(node, item, node);
                }
            }
            foreach (IAzManItem item in app.Items.Values)
            {
                if (item.ItemType == ItemType.Operation)
                {
                    if (item.ItemsWhereIAmAMember.Count == 0) this.AddOperation(node, item, node);
                }
            }
            node.Collapse();
        }

        private void AddRole(TreeNode parent, IAzManItem item, TreeNode applicationNode)
        {
            TreeNode node = new TreeNode(item.Name, item.Name, this.getImageUrl("Role_16x16.gif"));
            node.ToolTip = item.Description;
            parent.ChildNodes.Add(node);
            foreach (IAzManItem subItem in item.Members.Values)
            {
                if (subItem.ItemType == ItemType.Role)
                {
                    this.AddRole(node, subItem, applicationNode);
                }
            }
            foreach (IAzManItem subItem in item.Members.Values)
            {
                if (subItem.ItemType == ItemType.Task)
                {
                    this.AddTask(node, subItem, applicationNode);
                }
            }
            foreach (IAzManItem subItem in item.Members.Values)
            {
                if (subItem.ItemType == ItemType.Operation)
                {
                    this.AddOperation(node, subItem, applicationNode);
                }
            }
            node.Collapse();
        }

        private void AddTask(TreeNode parent, IAzManItem item, TreeNode applicationNode)
        {
            TreeNode node = new TreeNode(item.Name, item.Name, this.getImageUrl("Task_16x16.gif"));
            node.ToolTip = item.Description;
            parent.ChildNodes.Add(node);
            foreach (IAzManItem subItem in item.Members.Values)
            {
                if (subItem.ItemType == ItemType.Task)
                {
                    this.AddTask(node, subItem, applicationNode);
                }
            }
            foreach (IAzManItem subItem in item.Members.Values)
            {
                if (subItem.ItemType == ItemType.Operation)
                {
                    this.AddOperation(node, subItem, applicationNode);
                }
            }
            node.Collapse();
        }
        private void AddOperation(TreeNode parent, IAzManItem item, TreeNode applicationNode)
        {
            TreeNode node = new TreeNode(item.Name, item.Name, this.getImageUrl("Operation_16x16.gif"));
            node.ToolTip = item.Description;
            parent.ChildNodes.Add(node);
            foreach (IAzManItem subItem in item.Members.Values)
            {
                this.AddOperation(node, subItem, applicationNode);
            }
            node.Collapse();
        }
        #endregion Build Items Hierarchy

        protected void chkCache_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkCache.Checked && this.chkStorageCache.Checked)
                this.chkStorageCache.Checked = false;
            this.cache = null;
        }

        protected void chkStorageCache_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkStorageCache.Checked && this.chkCache.Checked)
                this.chkCache.Checked = false;
            this.cache = null;
        }
    }
}
