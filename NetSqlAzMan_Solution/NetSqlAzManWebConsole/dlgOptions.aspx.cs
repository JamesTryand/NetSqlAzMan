using System;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzManWebConsole
{
    public partial class dlgOptions : dlgPage
    {
        internal IAzManStorage storage;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.setImage("Options_32x32.gif");
            this.setOkHandler(new EventHandler(this.btnOk_Click));
            this.storage = this.Session["storage"] as IAzManStorage;
            this.rbAdministrator.Checked = (this.storage.Mode == NetSqlAzManMode.Administrator);
            this.rbDeveloper.Checked = (this.storage.Mode == NetSqlAzManMode.Developer);
            this.chkLogOnEventLog.Checked = this.storage.LogOnEventLog;
            this.chkLogOnDb.Checked = this.storage.LogOnDb;
            this.chkErrors.Checked = this.storage.LogErrors;
            this.chkWarnings.Checked = this.storage.LogWarnings;
            this.chkInformations.Checked = this.storage.LogInformations;
            this.chkLogType_CheckedChanged(this, EventArgs.Empty);
            this.Text = ".NET Sql Authorization Manager options";
            this.Description = "Manage .NET Sql Authorization Manager options";
            if (!this.storage.IAmAdmin)
                this.rbDeveloper.Enabled = this.rbAdministrator.Enabled = this.chkLogOnEventLog.Enabled = this.chkLogOnDb.Enabled = this.chkErrors.Enabled = this.chkWarnings.Enabled = this.chkInformations.Enabled = this.btnOk.Enabled = false;

        }

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (this.rbAdministrator.Checked)
            {
                this.storage.Mode = NetSqlAzManMode.Administrator;
            }
            else
            {
                this.storage.Mode = NetSqlAzManMode.Developer;
            }
            this.storage.LogErrors = this.chkErrors.Checked && (this.chkLogOnEventLog.Checked || this.chkLogOnDb.Checked);
            this.storage.LogWarnings = this.chkWarnings.Checked && (this.chkLogOnEventLog.Checked || this.chkLogOnDb.Checked);
            this.storage.LogInformations = this.chkInformations.Checked && (this.chkLogOnEventLog.Checked || this.chkLogOnDb.Checked);
            this.storage.LogOnEventLog = this.chkLogOnEventLog.Checked;
            this.storage.LogOnDb = this.chkLogOnDb.Checked;
            this.Session["RefreshTree"] = true;
            this.closeWindow(true);
        }

        protected void chkLogType_CheckedChanged(object sender, EventArgs e)
        {
            this.chkErrors.Enabled = this.chkWarnings.Enabled = this.chkInformations.Enabled = (this.chkLogOnEventLog.Checked || this.chkLogOnDb.Checked) && this.chkLogOnEventLog.Enabled && this.chkLogOnDb.Enabled;
        }
    }
}
