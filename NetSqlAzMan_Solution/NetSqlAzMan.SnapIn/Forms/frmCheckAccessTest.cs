using System;
using System.Collections.Generic;
using System.Drawing;
using System.Security.Principal;
using System.Windows.Forms;
using NetSqlAzMan.Cache;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.SnapIn.DirectoryServices;
using NetSqlAzMan.SnapIn.DirectoryServices.ADObjectPicker;

namespace NetSqlAzMan.SnapIn.Forms
{
    public partial class frmCheckAccessTest : frmBase
    {
        public IAzManStorage storage = null;
        public IAzManApplication application=null;
        private WindowsIdentity wid = null;
        private IAzManDBUser dbuser = null;
        private Cache.UserPermissionCache cache = null;
        private Cache.StorageCache storageCache = null;
        [PreEmptive.Attributes.Feature("NetSqlAzMan MMC SnapIn: Check Access Test")]
        public frmCheckAccessTest()
        {
            InitializeComponent();
        }

        internal void frmCheckAccessTest_Load(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
            ImageList clonedImageList = new ImageList();
            foreach (Image image in this.imageListItemHierarchyView.Images)
            {
                clonedImageList.Images.Add((Image)image.Clone());
            }
            this.checkAccessTestTreeView.ImageList = clonedImageList;
            /*Application.DoEvents();*/
            NetSqlAzMan.SnapIn.Globalization.ResourcesManager.CollectResources(this);
            this.lblUPNUser.Text = "(otheruser@domain.ext)";
            this.wid = ((System.Threading.Thread.CurrentPrincipal.Identity as WindowsIdentity) ?? WindowsIdentity.GetCurrent());
            NTAccount nta = (NTAccount)this.wid.User.Translate(typeof(NTAccount));
            string currentUpnName = nta.Value;
            if (currentUpnName.IndexOf('\\') != -1)
            {
                currentUpnName = currentUpnName.Substring(currentUpnName.IndexOf('\\') + 1);
            }
            this.lblMessage.Text = "...";
            this.btnBrowseWindowsUser.Text = "...";
            this.btnBrowseDBUser.Text = "...";
            this.chkCache.Text = "UserPermissionCache";
            this.chkStorageCache.Text = "StorageCache";
            this.groupBox1.Text = " " + Globalization.MultilanguageResource.GetString("frmCheckAccessTest.Text") + " ";
            this.dtValidFor.Value = DateTime.Now;
            this.rbCheckedChanged();
            this.txtWindowsUser.Text = currentUpnName;
            this.FormValidate();
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

        private void frmCheckAccessTest_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.None)
                this.DialogResult = DialogResult.Cancel;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void RefreshItemsHierarchy()
        {
            frmItemsHierarchyView ihv = new frmItemsHierarchyView();
            this.DialogResult = DialogResult.None;
            ImageList clonedImageList = new ImageList();
            foreach (Image image in ihv.imageListItemHierarchyView.Images)
            {
                clonedImageList.Images.Add((Image)image.Clone());
            }
            this.checkAccessTestTreeView.ImageList = clonedImageList;
            ihv.applications = new IAzManApplication[] { this.application };
            ihv.buildApplicationsTreeView();
            this.checkAccessTestTreeView.Nodes.Clear();
            this.checkAccessTestTreeView.Nodes.Add((TreeNode)ihv.itemsHierarchyTreeView.Nodes[0].Clone());
            this.checkAccessTestTreeView.ExpandAll();
            ihv.Dispose();
        }

        private void btnBrowseWindowsUser_Click(object sender, EventArgs e)
        {
            string userName = String.Empty;
            try
            {
                this.rbWindowsUser.Checked = true;
                ADObject[] res = DirectoryServicesUtils.ADObjectPickerShowDialog(this, false, true, false);
                /*Application.DoEvents();*/
                if (res != null)
                {
                    if (res.Length > 1)
                    {
                        this.ShowError(Globalization.MultilanguageResource.GetString("frmCheckAccessTest_Msg20"), Globalization.MultilanguageResource.GetString("frmCheckAccessTest_Msg10"));
                    }
                    if (res.Length == 1)
                    {
                        userName = res[0].UPN;
                        this.wid = new WindowsIdentity(userName);
                        this.txtWindowsUser.Text = userName;
                        this.dbuser = null;
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("frmCheckAccessTest_Msg10"));
            }
            finally
            {
                this.FormValidate();
            }
        }

        private void rbWindowsUser_CheckedChanged(object sender, EventArgs e)
        {
            this.rbCheckedChanged();
        }

        private void rbCheckedChanged()
        {
            if (this.rbWindowsUser.Checked)
            {
                this.txtWindowsUser.ReadOnly = false;
                this.txtWindowsUser.BackColor = SystemColors.Window;
                this.txtDBUser.BackColor = SystemColors.Control;
                this.txtWindowsUser.Text = String.Empty;
            }
            else
            {
                this.txtWindowsUser.ReadOnly = true;
                this.txtWindowsUser.BackColor = SystemColors.Control;
                this.txtDBUser.BackColor = SystemColors.Window;
            }
            this.txtWindowsUser.Text = String.Empty;
            this.txtDBUser.Text = String.Empty;
            this.cache = null;
            this.wid = null;
            this.dbuser = null;
        }

        private void rbDBUser_CheckedChanged(object sender, EventArgs e)
        {
            this.rbCheckedChanged();
        }

        private void FormValidate()
        {
            this.btnCheckAccessTest.Enabled = this.txtWindowsUser.Text.Length + this.txtDBUser.Text.Length > 0;
        }

        private void btnCheckAccessTest_Click(object sender, EventArgs e)
        {
            try
            {
                NTAccount nta = (NTAccount)(((System.Threading.Thread.CurrentPrincipal.Identity as WindowsIdentity) ?? WindowsIdentity.GetCurrent()).User.Translate(typeof(NTAccount)));
                string currentUpnName = nta.Value;
                if (currentUpnName.IndexOf('\\') != -1)
                {
                    currentUpnName = currentUpnName.Substring(currentUpnName.IndexOf('\\') + 1);
                }
                if (this.rbWindowsUser.Checked && !String.IsNullOrEmpty(this.txtWindowsUser.Text))
                {
                    this.dbuser = null;
                    if (this.txtWindowsUser.Text == currentUpnName)
                    {
                        //Current Windows User
                        this.wid = ((System.Threading.Thread.CurrentPrincipal.Identity as WindowsIdentity) ?? WindowsIdentity.GetCurrent());
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
                this.HourGlass(true);
                this.txtDetails.Text = String.Empty;
                this.btnCheckAccessTest.Enabled = this.btnClose.Enabled = 
                    this.btnBrowseWindowsUser.Enabled = this.btnBrowseDBUser.Enabled =
                    this.rbWindowsUser.Enabled = this.rbDBUser.Enabled = false;
                this.WriteLineDetailMessage(Globalization.MultilanguageResource.GetString("frmCheckAccessTest_Msg30") + " " + DateTime.Now.ToString());
                this.WriteLineDetailMessage(String.Empty);
                this.WriteIdentityDetails();
                this.WriteLineDetailMessage(String.Empty);
                this.WriteDetailMessage(Globalization.MultilanguageResource.GetString("frmCheckAccessTest_Msg40"));
                this.RefreshItemsHierarchy();
                Application.DoEvents();
                this.WriteLineDetailMessage(Globalization.MultilanguageResource.GetString("Done_Msg10"));
                this.WriteLineDetailMessage(String.Empty);
                TreeNode applicationTreeNode = this.checkAccessTestTreeView.Nodes[0].Nodes[0].Nodes[0];
                foreach (TreeNode itemTreeNode in applicationTreeNode.Nodes)
                {
                    this.checkAccessTest(itemTreeNode);
                }
                this.WriteLineDetailMessage(String.Empty);
                this.WriteLineDetailMessage(Globalization.MultilanguageResource.GetString("frmCheckAccessTest_Msg50") + " " + DateTime.Now.ToString());
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("frmCheckAccessTest_Msg10"));
            }
            finally
            {
                this.btnCheckAccessTest.Enabled = this.btnClose.Enabled = 
                    this.btnBrowseWindowsUser.Enabled = this.btnBrowseDBUser.Enabled =
                    this.rbWindowsUser.Enabled = this.rbDBUser.Enabled = true;
                this.HourGlass(false);
            }
        }

        private void WriteIdentityDetails()
        {
            if (this.wid!=null)
            {
                this.WriteLineDetailMessage(Globalization.MultilanguageResource.GetString("frmCheckAccessTest_Msg60"));
                SecurityIdentifier sid = this.wid.User;
                NTAccount nta = (NTAccount)sid.Translate(typeof(NTAccount));
                this.WriteLineDetailMessage(String.Format("{0}: {1} {2}: {3}", Globalization.MultilanguageResource.GetString("ColumnHeader_Name"), nta.Value, Globalization.MultilanguageResource.GetString("ColumnHeader_ObjectSID"), sid.Value));
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
                        this.WriteLineDetailMessage(String.Format("{0}) {1} - ({2})", (++index), ex.Message.Replace("\r\n"," "), sidA.Value));
                    }
                }
                this.WriteLineDetailMessage(String.Empty);
                this.WriteLineDetailMessage(Globalization.MultilanguageResource.GetString("frmCheckAccessTest_Msg100"));
                foreach (IAzManStoreGroup storeGroup in this.application.Store.GetStoreGroups())
                {
                    if (storeGroup.IsInGroup(this.wid))
                    {
                        this.WriteLineDetailMessage(storeGroup.Name);
                    }
                }
                this.WriteLineDetailMessage(String.Empty);
                this.WriteLineDetailMessage(Globalization.MultilanguageResource.GetString("frmCheckAccessTest_Msg110"));
                foreach (IAzManApplicationGroup applicationGroup in this.application.GetApplicationGroups())
                {
                    if (applicationGroup.IsInGroup(this.wid))
                    {
                        this.WriteLineDetailMessage(applicationGroup.Name);
                    }
                }
            }
            else if (this.dbuser!=null)
            {
                this.WriteLineDetailMessage(Globalization.MultilanguageResource.GetString("frmCheckAccessTest_Msg60"));
                this.WriteLineDetailMessage(String.Format("{0}: {1}\t{2}: {3}", Globalization.MultilanguageResource.GetString("ColumnHeader_Name"), this.dbuser.UserName, Globalization.MultilanguageResource.GetString("frmDBUsersList_lsvDBUsers_1.Text"), this.dbuser.CustomSid.StringValue));
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
            switch (tn.ImageIndex)
            {
                case 3: sItemType = Globalization.MultilanguageResource.GetString("Domain_Role"); break;
                case 4: sItemType = Globalization.MultilanguageResource.GetString("Domain_Task"); break;
                case 5: sItemType = Globalization.MultilanguageResource.GetString("Domain_Operation"); break;
            }
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
                    this.cache = new NetSqlAzMan.Cache.UserPermissionCache(this.application.Store.Storage, this.application.Store.Name, this.application.Name, this.wid, true, true);
                }
                else if (this.dbuser != null)
                {
                    this.cache = new NetSqlAzMan.Cache.UserPermissionCache(this.application.Store.Storage, this.application.Store.Name, this.application.Name, this.dbuser, true, true);
                }
                chkEnd = DateTime.Now;
                elapsedTime = (TimeSpan)chkEnd.Subtract(chkStart);
                this.WriteLineDetailMessage(String.Format("[{0} mls.]\r\n", elapsedTime.TotalMilliseconds));
            }
            else if (this.chkStorageCache.Checked && this.storageCache == null)
            {
                this.WriteDetailMessage("Building StorageCache ...");
                this.storageCache = new NetSqlAzMan.Cache.StorageCache(this.application.Store.Storage.ConnectionString);
                this.storageCache.BuildStorageCache(this.application.Store.Name, application.Name);
                chkEnd = DateTime.Now;
                elapsedTime = (TimeSpan)chkEnd.Subtract(chkStart);
                this.WriteLineDetailMessage(String.Format("[{0} mls.]\r\n", elapsedTime.TotalMilliseconds));
            }
            chkStart = DateTime.Now;
            elapsedTime = TimeSpan.Zero;
            this.WriteDetailMessage(String.Format("{0} {1} '{2}' ... ", Globalization.MultilanguageResource.GetString("frmCheckAccessTest_Msg70"), sItemType, tn.Text));
            try
            {
                if (this.wid != null)
                {
                    if (this.chkCache.Checked)
                    {
                        auth = this.cache.CheckAccess(tn.Text, this.dtValidFor.Checked ? this.dtValidFor.Value : DateTime.Now, out attributes);
                    }
                    else if (this.chkStorageCache.Checked)
                    { 
                        IAzManItem item = (IAzManItem)tn.Tag;
                        auth = this.storageCache.CheckAccess(item.Application.Store.Name, item.Application.Name, item.Name, this.wid.GetUserBinarySSid(), this.wid.GetGroupsBinarySSid(), this.dtValidFor.Checked ? this.dtValidFor.Value : DateTime.Now, false, out attributes);
                    }
                    else
                    {
                        auth = this.application.Store.Storage.CheckAccess(this.application.Store.Name, this.application.Name, tn.Text, this.wid, this.dtValidFor.Checked ? this.dtValidFor.Value : DateTime.Now, false, out attributes);
                    }
                }
                else if (this.dbuser != null)
                {
                    if (this.chkCache.Checked)
                    {
                        auth = this.cache.CheckAccess(tn.Text, this.dtValidFor.Checked ? this.dtValidFor.Value : DateTime.Now, out attributes);
                    }
                    else if (this.chkStorageCache.Checked)
                    {
                        IAzManItem item = (IAzManItem)tn.Tag;
                        auth = this.storageCache.CheckAccess(item.Application.Store.Name, item.Application.Name, item.Name, this.dbuser.CustomSid.StringValue,this.dtValidFor.Checked ? this.dtValidFor.Value : DateTime.Now, false);
                    }
                    else
                    {
                        auth = this.application.Store.Storage.CheckAccess(this.application.Store.Name, this.application.Name, tn.Text, this.dbuser, this.dtValidFor.Checked ? this.dtValidFor.Value : DateTime.Now, false, out attributes);
                    }
                }
                chkEnd = DateTime.Now;
                elapsedTime = (TimeSpan)chkEnd.Subtract(chkStart);
                sAuth = Globalization.MultilanguageResource.GetString("Domain_Neutral");
                switch (auth)
                {
                    case AuthorizationType.AllowWithDelegation:
                        sAuth = Globalization.MultilanguageResource.GetString("Domain_AllowWithDelegation");
                        tn.BackColor = Color.SkyBlue;
                        break;
                    case AuthorizationType.Allow:
                        sAuth = Globalization.MultilanguageResource.GetString("Domain_Allow");
                        tn.BackColor = Color.LightGreen;
                        break;
                    case AuthorizationType.Deny:
                        sAuth = Globalization.MultilanguageResource.GetString("Domain_Deny");
                        tn.BackColor = Color.LightSalmon;
                        break;
                    case AuthorizationType.Neutral:
                        sAuth = Globalization.MultilanguageResource.GetString("Domain_Neutral");
                        tn.BackColor = Color.Gainsboro;
                        break;
                }
                this.WriteLineDetailMessage(String.Format("{0} [{1} mls.]", sAuth, elapsedTime.TotalMilliseconds));
                if (attributes!=null && attributes.Count > 0)
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
                sAuth = Globalization.MultilanguageResource.GetString("frmCheckAccessTest_Msg10");
                this.WriteLineDetailMessage(String.Format("{0} [{1} mls.]", ex.Message, elapsedTime.TotalMilliseconds));
            }
            tn.Text = String.Format("{0} - ({1})", tn.Text, sAuth.ToUpper());
            
            tn.EnsureVisible();
            Application.DoEvents();

            foreach (TreeNode tnChild in tn.Nodes)
            {
                this.checkAccessTest(tnChild);
            }
        }

        private void WriteDetailMessage(string message)
        {
            this.txtDetails.Text += message;
            this.lblMessage.Text = message.Replace("\r\n", "");
            this.txtDetails.SelectionStart = this.txtDetails.Text.Length;
            this.txtDetails.ScrollToCaret();
            /*Application.DoEvents();*/
        }

        private void WriteLineDetailMessage(string message)
        {
            this.WriteDetailMessage(message + "\r\n");
        }

        private void btnBrowseDBUser_Click(object sender, EventArgs e)
        {
            try
            {
                this.rbDBUser.Checked = true;
                frmDBUsersList frm = new frmDBUsersList();
                frm.application = this.application;
                DialogResult dr = frm.ShowDialog(this);
                if (dr == DialogResult.OK)
                {
                    if (frm.selectedDBUsers.Length > 1)
                    {
                        this.ShowError(Globalization.MultilanguageResource.GetString("frmCheckAccessTest_Msg80"), Globalization.MultilanguageResource.GetString("frmCheckAccessTest_Msg10"));
                    }
                    if (frm.selectedDBUsers.Length == 1)
                    {
                        this.wid = null;
                        this.dbuser = frm.selectedDBUsers[0];
                        this.txtDBUser.Text = this.dbuser.UserName;
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("frmCheckAccessTest_Msg10"));
            }
            finally
            {
                this.FormValidate();
            }
        }

        private void txtWindowsUser_TextChanged(object sender, EventArgs e)
        {
            this.FormValidate();
        }

        private void chkCache_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkCache.Checked && this.chkStorageCache.Checked)
                this.chkStorageCache.Checked = false;
            this.cache = null;
        }

        private void chkStorageCache_CheckedChanged(object sender, EventArgs e)
        {
            if (this.chkStorageCache.Checked && this.chkCache.Checked)
                this.chkCache.Checked = false;
            this.cache = null;
        }
    }
}
