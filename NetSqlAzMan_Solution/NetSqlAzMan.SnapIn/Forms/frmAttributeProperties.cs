using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.SnapIn.DirectoryServices.ADObjectPicker;
using NetSqlAzMan.SnapIn.DirectoryServices;
using NetSqlAzMan;

namespace NetSqlAzMan.SnapIn.Forms
{
    public partial class frmAttributeProperties : frmBase
    {
        internal string key;
        internal string value;

        public frmAttributeProperties()
        {
            InitializeComponent();
        }

        private void frmAuthorizationAttributeProperties_Load(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
            if (!String.IsNullOrEmpty(this.key))
            {
                this.txtKey.Text = this.key;
                this.txtValue.Text = this.value;
                this.txtKey.Enabled = false;
                this.txtValue.Focus();
            }
            else
            {
                this.txtKey.Text = String.Empty;
                this.txtValue.Text = String.Empty;
                this.txtKey.Focus();
            }
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

        private void frmAuthorizationAttributeProperties_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.None)
                this.DialogResult = DialogResult.Cancel;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.key = this.txtKey.Text.Trim();
            this.value = this.txtValue.Text.Trim();
            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}