using System;
using System.Web.UI;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzManWebConsole
{
    public partial class dlgAuditing : dlgPage
    {
        protected internal IAzManStorage storage = null;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.setImage("SqlAudit.gif");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.storage = this.Session["storage"] as IAzManStorage;
            this.showCloseOnly();
            if (!Page.IsPostBack)
            {
                SqlAudit.SqlAuditSettings settings = new SqlAudit.SqlAuditSettings();
                this.Description = "SqlAudit ver. " + settings.GetType().Assembly.GetName().Version.ToString();
            }
            this.Text = "Auditing";
            this.showWaitPanelOnSubmit(this.pnlWait, this.pnlAudit);
        }

        private void generate(SqlAudit.ScriptAction scriptAction)
        {
            try
            {
                this.txtDDLScript.Text = SQLAudit.StorageAuditGenerator.GenerateAuditScript(this.storage.ConnectionString, scriptAction, null);
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }

        protected void imgGenerate_Click(object sender, ImageClickEventArgs e)
        {
            if (this.tsbCreateTablesTriggerAndViews.Checked)
                this.generate(SqlAudit.ScriptAction.CreateAllAuditTablesTriggersAndViews);
            else if (this.tspDropTablesTriggersAndViews.Checked)
                this.generate(SqlAudit.ScriptAction.DropAllAuditTablesTriggersAndViews);
            else if (tsbCreateAuditTriggersOnly.Checked)
                this.generate(SqlAudit.ScriptAction.CreateTriggersOnly);
            else if (tsbDropAuditTriggersOnly.Checked)
                this.generate(SqlAudit.ScriptAction.DropTriggersOnly);
        }
    }
}
