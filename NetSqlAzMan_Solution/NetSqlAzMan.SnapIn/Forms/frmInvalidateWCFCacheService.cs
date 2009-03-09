using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan;
using NetSqlAzMan.SnapIn.DirectoryServices;
using System.DirectoryServices;
using Microsoft.Interop.Security.AzRoles;

namespace NetSqlAzMan.SnapIn.Forms
{
    public partial class frmInvalidateWCFCacheService : frmBase
    {
        public frmInvalidateWCFCacheService()
        {
            InitializeComponent();
        }

        private void frmImportFromAzMan_Load(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
            NetSqlAzMan.SnapIn.Globalization.ResourcesManager.CollectResources(this);
            using (wcf.CacheServiceClient csc = new NetSqlAzMan.SnapIn.wcf.CacheServiceClient())
            {
                this.txtWCFCacheServiceEndPoint.Text = csc.Endpoint.Address.ToString();
            }
        }

        protected void HourGlass(bool switchOn)
        {
            this.Cursor = switchOn ? Cursors.WaitCursor : Cursors.Arrow;
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

        private void frmImportFromAzMan_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.None)
                this.DialogResult = DialogResult.Cancel;
        }

        private void btnInvalidateCache_Click(object sender, EventArgs e)
        {
            try
            {
                using (wcf.CacheServiceClient csc = new NetSqlAzMan.SnapIn.wcf.CacheServiceClient())
                {
                    csc.Endpoint.Address = new System.ServiceModel.EndpointAddress(this.txtWCFCacheServiceEndPoint.Text);
                    csc.Open();
                    csc.InvalidateCache();
                }
                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                this.HourGlass(false);
                this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("frmImportFromAzMan_Tit10"));
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}