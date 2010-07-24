using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzMan.SnapIn.Forms
{
    public partial class frmApplicationPermissions : frmBase
    {
        public IAzManApplication application;
        [PreEmptive.Attributes.Feature("NetSqlAzMan MMC SnapIn: Application Permissions")]
        public frmApplicationPermissions()
        {
            InitializeComponent();
        }

        private void frmApplicationPermissions_Load(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
            this.RefreshApplicationPermissions();
            /*Application.DoEvents();*/
            NetSqlAzMan.SnapIn.Globalization.ResourcesManager.CollectResources(this);
            this.Text = NetSqlAzMan.SnapIn.Globalization.MultilanguageResource.GetString("frmApplicationPermissions.Text") + " - " + this.application.Name;
            this.lblManagers.Text = "Application Managers";
            this.lblUsers.Text = "Application Users";
            this.lblReaders.Text = "Application Readers";
        }

        private void RefreshApplicationPermissions()
        {
            this.HourGlass(true);
            this.chkManagers.Items.Clear();
            //Store Managers
            foreach (KeyValuePair<string, bool> kvp in this.application.Store.GetManagers())
            {
                if (kvp.Value)
                    this.chkManagers.Items.Add(String.Format("{0} ({1})", kvp.Key, this.application.Store.Name),CheckState.Indeterminate);
            }
            //Managers
            foreach (KeyValuePair<string, bool> kvp in this.application.GetManagers())
            {
                this.chkManagers.Items.Add(kvp.Key, kvp.Value);
            }
            this.chkUsers.Items.Clear();
            //Store Users
            foreach (KeyValuePair<string, bool> kvp in this.application.Store.GetUsers())
            {
                if (kvp.Value)
                    this.chkUsers.Items.Add(String.Format("{0} ({1})", kvp.Key, this.application.Store.Name), CheckState.Indeterminate);
            }
            //Users
            foreach (KeyValuePair<string, bool> kvp in this.application.GetUsers())
            {
                this.chkUsers.Items.Add(kvp.Key, kvp.Value);
            }
            this.chkReaders.Items.Clear();
            //Store Readers
            foreach (KeyValuePair<string, bool> kvp in this.application.Store.GetReaders())
            {
                if (kvp.Value)
                    this.chkReaders.Items.Add(String.Format("{0} ({1})", kvp.Key, this.application.Store.Name), CheckState.Indeterminate);
            }
            //Readers
            foreach (KeyValuePair<string, bool> kvp in this.application.GetReaders())
            {
                this.chkReaders.Items.Add(kvp.Key, kvp.Value);
            }
            if (!this.application.IAmManager)
                this.chkManagers.Enabled = this.chkUsers.Enabled = this.chkReaders.Enabled = this.btnOK.Enabled = false;
            this.chkManagers.Enabled = this.application.Store.IAmManager;
            this.HourGlass(false);
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

        private void frmApplicationPermissions_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.None)
                this.DialogResult = DialogResult.Cancel;
        }

        private bool findLogin(KeyValuePair<string, bool>[] logins, string login)
        {
            foreach (KeyValuePair<string, bool> l in logins)
            {
                if (l.Value && String.Compare(l.Key, login, true) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                this.HourGlass(true);
                this.application.Store.Storage.OpenConnection();
                this.application.Store.Storage.BeginTransaction(AzManIsolationLevel.ReadUncommitted);
                //Managers
                KeyValuePair<string, bool>[] managers = this.application.GetManagers();
                for (int i = 0; i < this.chkManagers.Items.Count;i++)
                {
                    if (this.chkManagers.GetItemCheckState(i) != CheckState.Indeterminate)
                    {
                        string sqlLogin = (string)this.chkManagers.Items[i];

                        if (this.chkManagers.CheckedItems.Contains(sqlLogin))
                        {
                            if (!this.findLogin(managers, sqlLogin))
                                this.application.GrantAccessAsManager(sqlLogin);
                        }
                        else
                        {
                            if (this.findLogin(managers, sqlLogin))
                                this.application.RevokeAccessAsManager(sqlLogin);
                        }
                    }
                }
                //Users
                KeyValuePair<string, bool>[] users = this.application.GetUsers();
                for (int i=0; i<this.chkUsers.Items.Count;i++)
                {
                    if (this.chkUsers.GetItemCheckState(i) != CheckState.Indeterminate)
                    {
                        string sqlLogin = (string)this.chkUsers.Items[i];

                        if (this.chkUsers.CheckedItems.Contains(sqlLogin))
                        {
                            if (!this.findLogin(users, sqlLogin))
                                this.application.GrantAccessAsUser(sqlLogin);
                        }
                        else
                        {
                            if (this.findLogin(users, sqlLogin))
                                this.application.RevokeAccessAsUser(sqlLogin);
                        }
                    }
                }
                //Readers
                KeyValuePair<string, bool>[] readers = this.application.GetReaders();
                for (int i=0; i<this.chkReaders.Items.Count;i++)
                {
                    if (this.chkReaders.GetItemCheckState(i) != CheckState.Indeterminate)
                    {
                        string sqlLogin = (string)this.chkReaders.Items[i];

                        if (this.chkReaders.CheckedItems.Contains(sqlLogin))
                        {
                            if (!this.findLogin(readers, sqlLogin))
                                this.application.GrantAccessAsReader(sqlLogin);
                        }
                        else
                        {
                            if (this.findLogin(readers, sqlLogin))
                                this.application.RevokeAccessAsReader(sqlLogin);
                        }
                    }
                }
                this.application.Store.Storage.CommitTransaction();
                this.DialogResult = DialogResult.OK;
                this.HourGlass(false);
            }
            catch (Exception ex)
            {
                this.application.Store.Storage.RollBackTransaction();
                this.HourGlass(false);
                this.DialogResult = DialogResult.None;
                this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("UpdateError_Msg10"));
            }
            finally
            {
                this.application.Store.Storage.CloseConnection();
            }

        }

        private void chkLogins_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.CurrentValue == CheckState.Indeterminate)
                e.NewValue = CheckState.Indeterminate;
        }
    }
}