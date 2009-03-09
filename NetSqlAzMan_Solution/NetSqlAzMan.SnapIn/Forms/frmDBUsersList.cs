using System;
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
    public partial class frmDBUsersList : frmBase
    {
        internal IAzManStore store;
        internal IAzManApplication application;
        internal IAzManDBUser[] selectedDBUsers;
        public frmDBUsersList()
        {
            InitializeComponent();
            this.selectedDBUsers = null;
            this.store = null;
            this.application = null;
        }

        private void frmDBUsersList_Load(object sender, EventArgs e)
        {
            this.HourGlass(true);
            this.DialogResult = DialogResult.None;
            this.RefreshApplicationList();
            this.PreSortListView(this.lsvDBUsers);
            //PorkAround: http://lab.msdn.microsoft.com/ProductFeedback/viewFeedback.aspx?feedbackId=FDBK49664
            ImageList clonedImageList = new ImageList();
            foreach (Image image in this.imageList1.Images)
            {
                clonedImageList.Images.Add((Image)image.Clone());
            }
            this.lsvDBUsers.SmallImageList = clonedImageList;
            //PorkAround End
            /*Application.DoEvents();*/
            NetSqlAzMan.SnapIn.Globalization.ResourcesManager.CollectResources(this);
        }

        private void PreSortListView(ListView lv)
        {
            lv.Sorting = SortOrder.Ascending;
        }
        private void SortListView(ListView lv)
        {
            if (lv.Sorting == SortOrder.Ascending)
                lv.Sorting = SortOrder.Descending;
            else
                lv.Sorting = SortOrder.Ascending;
            lv.Sort();
        }

        private void RefreshApplicationList()
        {
            this.HourGlass(true);
            this.lsvDBUsers.Items.Clear();
            IAzManDBUser[] dbUsers = null;
            if (this.store != null)
                dbUsers = this.store.GetDBUsers();
            else if (this.application != null)
                dbUsers = this.application.GetDBUsers();
            else
                throw new System.InvalidOperationException(Globalization.MultilanguageResource.GetString("frmDBUsersList_Msg10"));
            foreach (IAzManDBUser dbUser in dbUsers)
            {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Tag = dbUser;
                    lvi.ImageIndex = 0;
                    lvi.Text = dbUser.UserName;
                    lvi.SubItems.Add(dbUser.CustomSid.StringValue);
                    lvi.SubItems.Add(Globalization.MultilanguageResource.GetString("frmDBUsersList_Msg20"));
                    this.lsvDBUsers.Items.Add(lvi);
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

        private void frmDBUsersList_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.None)
                this.DialogResult = DialogResult.Cancel;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.HourGlass(true);
            this.selectedDBUsers = new IAzManDBUser[this.lsvDBUsers.CheckedItems.Count];
            int index = 0;
            foreach (ListViewItem lvi in this.lsvDBUsers.CheckedItems)
            {
                this.selectedDBUsers[index++] = (IAzManDBUser)lvi.Tag;
            }
            this.DialogResult = DialogResult.OK;
            this.HourGlass(false);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.selectedDBUsers = new IAzManDBUser[0];
        }

        private void lsvDBUsers_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            this.SortListView(this.lsvDBUsers);
        }

        private void frmDBUsersList_Activated(object sender, EventArgs e)
        {
            this.HourGlass(false);
        }
    }
}