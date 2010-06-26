using System;
using System.Windows.Forms;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzMan.SnapIn.Forms
{
    public partial class frmStoreProperties : frmBase
    {
        internal IAzManStorage storage = null;
        internal IAzManStore store = null;

        [PreEmptive.Attributes.Feature("NetSqlAzMan MMC SnapIn: Store Properties")]
        public frmStoreProperties()
        {
            InitializeComponent();
        }

        private void frmCreateStore_Load(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
            if (this.store != null)
            {
                this.btnPermissions.Enabled = true;
                this.btnAttributes.Enabled = true;
                
                this.txtName.Text = this.store.Name;
                this.txtDescription.Text = this.store.Description;
                this.txtName.SelectAll();
                if (!this.store.IAmManager)
                    this.txtName.Enabled = this.txtDescription.Enabled = this.btnOk.Enabled = false;
            }
            else
            {
                this.btnPermissions.Enabled = false;
                this.btnAttributes.Enabled = false;
            }
            NetSqlAzMan.SnapIn.Globalization.ResourcesManager.CollectResources(this);
            if (this.store != null)
            {
                this.Text = Globalization.MultilanguageResource.GetString("frmStoreProperties_Msg10");
            }
            else
            {
                this.Text = Globalization.MultilanguageResource.GetString("frmStoreProperties_Msg20");
            }
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

        private void frmCreateStore_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.None)
                this.DialogResult = DialogResult.Cancel;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            this.ValidateForm();
        }

        private void ValidateForm()
        {
            bool isValid = true;
            if (this.txtName.Text.Trim().Length == 0)
            {
                isValid = false;
                this.errorProvider1.SetError(this.txtName, Globalization.MultilanguageResource.GetString("frmStoreProperties_Msg30"));
            }
            else
            {
                this.errorProvider1.SetError(this.txtName, String.Empty);
            }
            this.btnOk.Enabled = isValid;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                this.HourGlass(true);
                //Create
                if (this.store == null)
                {
                    this.store = this.storage.CreateStore(this.txtName.Text, this.txtDescription.Text);
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    this.storage.BeginTransaction(AzManIsolationLevel.ReadUncommitted);
                    this.store.Rename(this.txtName.Text.Trim());
                    this.store.Update(this.txtDescription.Text.Trim());
                    this.storage.CommitTransaction();
                    this.DialogResult = DialogResult.OK;
                }
                this.HourGlass(false);
            }
            catch (Exception ex)
            {
                this.HourGlass(false);
                if (this.store != null)
                    this.storage.RollBackTransaction();
                this.DialogResult = DialogResult.None;
                this.ShowError(ex.Message, this.store == null ? Globalization.MultilanguageResource.GetString("frmStoreProperties_Msg40") : Globalization.MultilanguageResource.GetString("frmStoreProperties_Msg50"));
            }
        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {
            this.ValidateForm();
        }

        private void txtLDapPath_TextChanged(object sender, EventArgs e)
        {
            this.ValidateForm();
        }

        private void btnAttributes_Click(object sender, EventArgs e)
        {
            try
            {
                frmStoreAttributes frm = new frmStoreAttributes();
                frm.Text += " - " + this.store.Name;
                frm.store = this.store;
                frm.ShowDialog(this);
                /*Application.DoEvents();*/
                frm.Dispose();
                /*Application.DoEvents();*/
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("frmStoreProperties_Msg60"));
            }
        }

        private void btnPermissions_Click(object sender, EventArgs e)
        {
            try
            {
                frmStorePermissions frm = new frmStorePermissions();
                frm.Text += " - " + this.store.Name;
                frm.store = this.store;
                frm.ShowDialog(this);
                /*Application.DoEvents();*/
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("frmApplicationProperties_Msg40"));
            }
        }
    }
}