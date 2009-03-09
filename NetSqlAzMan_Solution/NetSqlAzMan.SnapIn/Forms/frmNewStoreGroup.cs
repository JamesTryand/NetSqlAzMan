using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan;

namespace NetSqlAzMan.SnapIn.Forms
{
    public partial class frmNewStoreGroup : frmBase
    {
        internal IAzManStore store;
        internal IAzManStoreGroup storeGroup = null;
        public frmNewStoreGroup()
        {
            InitializeComponent();
        }

        private void frmNewStoreGroup_Load(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
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

        private void frmNewStoreGroup_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.None)
                this.DialogResult = DialogResult.Cancel;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.txtName.Text.Trim()))
            {
                this.btnOk.Enabled = false;
                this.errorProvider1.SetError(this.txtName, Globalization.MultilanguageResource.GetString("frmNewStoreGroup_Msg10"));
            }
            else
            {
                this.btnOk.Enabled = true;
                this.errorProvider1.SetError(this.txtName, String.Empty);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.HourGlass(true);
            try
            {
                this.storeGroup = this.store.CreateStoreGroup(SqlAzManSID.NewSqlAzManSid(), this.txtName.Text.Trim(), this.txtDescription.Text.Trim(), String.Empty, (this.rbtBasic.Checked ? GroupType.Basic : GroupType.LDapQuery));
                this.HourGlass(false);
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                this.HourGlass(false);
                this.DialogResult = DialogResult.None;
                this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("frmNewStoreGroup_Msg20"));
            }
        }
    }
}