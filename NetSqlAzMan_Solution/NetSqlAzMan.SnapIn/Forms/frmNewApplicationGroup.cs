using System;
using System.Windows.Forms;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzMan.SnapIn.Forms
{
    public partial class frmNewApplicationGroup : frmBase
    {
        internal IAzManApplication application;
        internal IAzManApplicationGroup applicationGroup = null;

        public frmNewApplicationGroup()
        {
            InitializeComponent();
        }

        private void frmNewApplicationGroup_Load(object sender, EventArgs e)
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

        private void frmNewApplicationGroup_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.None)
                this.DialogResult = DialogResult.Cancel;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(this.txtName.Text.Trim()))
            {
                this.btnOk.Enabled = false;
                this.errorProvider1.SetError(this.txtName, Globalization.MultilanguageResource.GetString("frmNewApplicationGroup_Msg10"));
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
                this.applicationGroup = this.application.CreateApplicationGroup(SqlAzManSID.NewSqlAzManSid(), this.txtName.Text.Trim(), this.txtDescription.Text.Trim(), String.Empty, (this.rbtBasic.Checked ? GroupType.Basic : GroupType.LDapQuery));
                this.HourGlass(false);
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                this.HourGlass(false);
                this.DialogResult = DialogResult.None;
                this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("frmNewApplicationGroup_Msg20"));
            }
        }
    }
}