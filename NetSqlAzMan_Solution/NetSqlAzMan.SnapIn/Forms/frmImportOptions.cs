using System;
using System.Windows.Forms;

namespace NetSqlAzMan.SnapIn.Forms
{
    public partial class frmImportOptions : frmBase
    {
        public object importIntoObject;
        public string fileName;

        //[PreEmptive.Attributes.Feature("NetSqlAzMan MMC SnapIn: Import Options")]
        public frmImportOptions()
        {
            InitializeComponent();
        }

        private void frmImportOptions_Load(object sender, EventArgs e)
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

        private void frmImportOptions_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.None)
                this.DialogResult = DialogResult.Cancel;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
            frmImport frm = new frmImport();
            try
            {
                this.Close();
                SqlAzManMergeOptions mergeOptions = SqlAzManMergeOptions.NoMerge;
                if (this.chkCreatesNewItems.Checked) mergeOptions |= SqlAzManMergeOptions.CreatesNewItems;
                if (this.chkOverwritesExistingItems.Checked) mergeOptions |= SqlAzManMergeOptions.OverwritesExistingItems;
                if (this.chkDeleteMissingItems.Checked) mergeOptions |= SqlAzManMergeOptions.DeleteMissingItems;
                if (this.chkCreatesNewItemAuthorizations.Checked) mergeOptions |= SqlAzManMergeOptions.CreatesNewItemAuthorizations;
                if (this.chkOverwritesItemAuthorizations.Checked) mergeOptions |= SqlAzManMergeOptions.OverwritesExistingItemAuthorization;
                if (this.chkDeleteMissingItemAuthorizations.Checked) mergeOptions |= SqlAzManMergeOptions.DeleteMissingItemAuthorizations;

                frm.ShowDialog(this, fileName, importIntoObject, this.chkUsersAndGroups.Checked, this.chkDBUsers.Checked, this.chkAuthorizations.Checked, mergeOptions);
                /*Application.DoEvents();*/
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("frmImportOptions_Msg10"));
            }
            finally
            {
                this.DialogResult = DialogResult.OK;
            }

        }
    }
}