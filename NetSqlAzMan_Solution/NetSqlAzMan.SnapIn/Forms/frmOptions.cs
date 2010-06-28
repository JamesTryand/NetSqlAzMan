using System;
using System.Windows.Forms;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzMan.SnapIn.Forms
{
    public partial class frmOptions : frmBase
    {
        internal NetSqlAzManMode mode;
        internal IAzManStorage storage;
        internal bool logErrors;
        internal bool logWarnings;
        internal bool logInformations;
        internal bool logOnEventLog;
        internal bool logOnDb;

        //[PreEmptive.Attributes.Feature("NetSqlAzMan MMC SnapIn: Options")]
        public frmOptions()
        {
            InitializeComponent();
        }

        private void frmOptions_Load(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
            this.rbAdministrator.Checked = (this.mode == NetSqlAzManMode.Administrator);
            this.rbDeveloper.Checked = (this.mode == NetSqlAzManMode.Developer);
            this.chkLogOnEventLog.Checked = this.logOnEventLog;
            this.chkLogOnDb.Checked = this.logOnDb;
            this.chkErrors.Checked = this.logErrors;
            this.chkWarnings.Checked = this.logWarnings;
            this.chkInformations.Checked = this.logInformations;
            NetSqlAzMan.SnapIn.Globalization.ResourcesManager.CollectResources(this);
            this.chkLogType_CheckedChanged(this, EventArgs.Empty);
            if (!this.storage.IAmAdmin)
                this.rbDeveloper.Enabled = this.rbAdministrator.Enabled = this.chkLogOnEventLog.Enabled = this.chkLogOnDb.Enabled = this.chkErrors.Enabled = this.chkWarnings.Enabled = this.chkInformations.Enabled = this.btnOk.Enabled = false;
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

        private void frmOptions_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.None)
                this.DialogResult = DialogResult.Cancel;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (this.rbAdministrator.Checked)
            {
                this.mode = NetSqlAzManMode.Administrator;
            }
            else
            {
                this.mode = NetSqlAzManMode.Developer;
            }
            this.logErrors = this.chkErrors.Checked && (this.chkLogOnEventLog.Checked || this.chkLogOnDb.Checked);
            this.logWarnings = this.chkWarnings.Checked && (this.chkLogOnEventLog.Checked || this.chkLogOnDb.Checked);
            this.logInformations = this.chkInformations.Checked && (this.chkLogOnEventLog.Checked || this.chkLogOnDb.Checked);
            this.logOnEventLog = this.chkLogOnEventLog.Checked;
            this.logOnDb = this.chkLogOnDb.Checked;
            this.DialogResult = DialogResult.OK;
        }

        private void chkLogType_CheckedChanged(object sender, EventArgs e)
        {
            this.chkErrors.Enabled = this.chkWarnings.Enabled = this.chkInformations.Enabled = (this.chkLogOnEventLog.Checked || this.chkLogOnDb.Checked) && this.chkLogOnEventLog.Enabled && this.chkLogOnDb.Enabled;
        }
    }
}