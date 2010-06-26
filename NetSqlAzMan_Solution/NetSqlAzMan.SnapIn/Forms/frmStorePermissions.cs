using System;
using System.Collections.Generic;
using System.Windows.Forms;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzMan.SnapIn.Forms
{
    public partial class frmStorePermissions : frmBase
    {
        public IAzManStore store;

        [PreEmptive.Attributes.Feature("NetSqlAzMan MMC SnapIn: Store Permissions")]
        public frmStorePermissions()
        {
            InitializeComponent();
        }

        private void frmStorePermissions_Load(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
            this.RefreshStorePermissions();
            /*Application.DoEvents();*/
            NetSqlAzMan.SnapIn.Globalization.ResourcesManager.CollectResources(this);
            this.Text = NetSqlAzMan.SnapIn.Globalization.MultilanguageResource.GetString("frmStorePermissions.Text") + " - " + this.store.Name;
            this.lblManagers.Text = "Store Managers";
            this.lblUsers.Text = "Store Users";
            this.lblReaders.Text = "Store Readers";
        }

        private void RefreshStorePermissions()
        {
            this.HourGlass(true);
            this.chkManagers.Items.Clear();
            foreach (KeyValuePair<string, bool> kvp in this.store.GetManagers())
            {
                this.chkManagers.Items.Add(kvp.Key, kvp.Value);
            }
            this.chkUsers.Items.Clear();
            foreach (KeyValuePair<string, bool> kvp in this.store.GetUsers())
            {
                this.chkUsers.Items.Add(kvp.Key, kvp.Value);
            }
            this.chkReaders.Items.Clear();
            foreach (KeyValuePair<string, bool> kvp in this.store.GetReaders())
            {
                this.chkReaders.Items.Add(kvp.Key, kvp.Value);
            }
            if (!this.store.IAmManager)
                this.chkManagers.Enabled = this.chkUsers.Enabled = this.chkReaders.Enabled = this.btnOK.Enabled = false ;
            this.chkManagers.Enabled = this.store.IAmAdmin;
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

        private void frmStorePermissions_FormClosing(object sender, FormClosingEventArgs e)
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
                this.store.Storage.OpenConnection();
                this.store.Storage.BeginTransaction(AzManIsolationLevel.ReadUncommitted);
                //Managers
                KeyValuePair<string, bool>[] managers = this.store.GetManagers();
                foreach (string sqlLogin in this.chkManagers.Items)
                {
                    if (this.chkManagers.CheckedItems.Contains(sqlLogin))
                    {
                        if (!this.findLogin(managers, sqlLogin))
                            this.store.GrantAccessAsManager(sqlLogin);
                    }
                    else
                    {
                        if (this.findLogin(managers, sqlLogin))
                            this.store.RevokeAccessAsManager(sqlLogin);
                    }
                }
                //Users
                KeyValuePair<string, bool>[] users = this.store.GetUsers();
                foreach (string sqlLogin in this.chkUsers.Items)
                {
                    if (this.chkUsers.CheckedItems.Contains(sqlLogin))
                    {
                        if (!this.findLogin(users, sqlLogin))
                            this.store.GrantAccessAsUser(sqlLogin);
                    }
                    else
                    {
                        if (this.findLogin(users, sqlLogin))
                            this.store.RevokeAccessAsUser(sqlLogin);
                    }
                }
                //Readers
                KeyValuePair<string, bool>[] readers = this.store.GetReaders();
                foreach (string sqlLogin in this.chkReaders.Items)
                {
                    if (this.chkReaders.CheckedItems.Contains(sqlLogin))
                    {
                        if (!this.findLogin(readers, sqlLogin))
                            this.store.GrantAccessAsReader(sqlLogin);
                    }
                    else
                    {
                        if (this.findLogin(readers, sqlLogin))
                            this.store.RevokeAccessAsReader(sqlLogin);
                    }
                }
                this.store.Storage.CommitTransaction();
                this.DialogResult = DialogResult.OK;
                this.HourGlass(false);
            }
            catch (Exception ex)
            {
                this.store.Storage.RollBackTransaction();
                this.HourGlass(false);
                this.DialogResult = DialogResult.None;
                this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("UpdateError_Msg10"));
            }
            finally
            {
                this.store.Storage.CloseConnection();
            }

        }
    }
}