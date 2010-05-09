using System;
using System.Windows.Forms;

namespace NetSqlAzMan.SnapIn.Forms
{
    public partial class frmNetSqlAzManBase : frmBase
    {
        public frmNetSqlAzManBase()
        {
            InitializeComponent();
        }

        private void frmNetSqlAzManBase_Load(object sender, EventArgs e)
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

        private void frmNetSqlAzManBase_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.None)
                this.DialogResult = DialogResult.Cancel;
        }
    }
}