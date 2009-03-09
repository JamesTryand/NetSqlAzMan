using System;
using System.DirectoryServices;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.SnapIn.DirectoryServices;
using NetSqlAzMan;

namespace NetSqlAzMan.SnapIn.Forms
{
    public partial class frmActiveDirectoryObjectsList : frmBase
    {
        internal SearchResultCollection searchResultCollection;
        public frmActiveDirectoryObjectsList()
        {
            InitializeComponent();
        }

        private void frmActiveDirectoryObjectsList_Load(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
            //PorkAround: http://lab.msdn.microsoft.com/ProductFeedback/viewFeedback.aspx?feedbackId=FDBK49664
            ImageList clonedImageList = new ImageList();
            foreach (Image image in this.imageList1.Images)
            {
                clonedImageList.Images.Add((Image)image.Clone());
            }
            this.lsvObjectsSid.SmallImageList = clonedImageList;
            //PorkAround End
            /*Application.DoEvents();*/
            this.RefreshActiveDirectoryObjectsList();
            NetSqlAzMan.SnapIn.Globalization.ResourcesManager.CollectResources(this);
        }

        private void RefreshActiveDirectoryObjectsList()
        {
            this.HourGlass(true);
            this.lsvObjectsSid.Items.Clear();
            if (this.searchResultCollection != null)
            {
                foreach (SearchResult sr in this.searchResultCollection)
                {
                    DirectoryEntry de = sr.GetDirectoryEntry();
                    ListViewItem lvi = new ListViewItem();
                    lvi.Tag = sr;
                    lvi.Text = (string)de.Properties["sAMAccountName"][0];
                    lvi.SubItems.Add((string)de.InvokeGet("displayname"));
                    lvi.SubItems.Add(de.SchemaClassName);
                    lvi.SubItems.Add(new SqlAzManSID((byte[])de.Properties["objectSid"].Value).StringValue);
                    this.lsvObjectsSid.Items.Add(lvi);
                }
            }
            this.HourGlass(false);
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

        private void frmActiveDirectoryObjectsList_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.None)
                this.DialogResult = DialogResult.Cancel;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }
    }
}