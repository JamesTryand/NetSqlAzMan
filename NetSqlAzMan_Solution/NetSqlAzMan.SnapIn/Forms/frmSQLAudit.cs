using System;
using System.Windows.Forms;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzMan.SnapIn.Forms
{
    public partial class frmSQLAudit : frmBase
    {
        internal IAzManStorage storage;

        [PreEmptive.Attributes.Feature("NetSqlAzMan MMC SnapIn: SQL Audit")]
        public frmSQLAudit() : base(false)
        {
            InitializeComponent();
        }

        private void frmSQLAudit_Load(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
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

        private void frmSQLAudit_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.None)
            {
                this.DialogResult = DialogResult.Cancel;
                e.Cancel = true;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void ProgressChange(int value, int maximum)
        {
            this.progressBar.Maximum = maximum;
            this.progressBar.Value = value;
            /*Application.DoEvents();*/
        }

        private void generate(SqlAudit.ScriptAction scriptAction)
        {
            try
            {
                this.HourGlass(true);
                this.txtDDLScript.Clear();
                /*Application.DoEvents();*/
                this.txtDDLScript.Text = SQLAudit.StorageAuditGenerator.GenerateAuditScript(this.storage.ConnectionString, scriptAction, new SqlAudit.ProgressChangeDelegate(this.ProgressChange));
                this.txtDDLScript.SelectionStart = 0;
                this.txtDDLScript.ScrollToCaret();
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message, ex.Source);
            }
            finally
            {
                this.HourGlass(false);
            }
        }

        private void tsbCreateTablesTriggerAndViews_Click(object sender, EventArgs e)
        {
            this.generate(SqlAudit.ScriptAction.CreateAllAuditTablesTriggersAndViews);
        }

        private void tspDropTablesTriggersAndViews_Click(object sender, EventArgs e)
        {
            this.generate(SqlAudit.ScriptAction.DropAllAuditTablesTriggersAndViews);
        }

        private void tsbCreateAuditTriggersOnly_Click(object sender, EventArgs e)
        {
            this.generate(SqlAudit.ScriptAction.CreateTriggersOnly);
        }

        private void tsbDropAuditTriggersOnly_Click(object sender, EventArgs e)
        {
            this.generate(SqlAudit.ScriptAction.DropTriggersOnly);
        }

        private void lnkSQLAudit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(this.lnkSQLAudit.Text);
        }
    }
}