using System;
using System.Drawing;
using System.Windows.Forms;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzMan.SnapIn.Forms
{
    public partial class frmStoreGroupsList : frmBase
    {
        internal IAzManStore store;
        internal IAzManStoreGroup storeGroup;
        internal IAzManStoreGroup[] selectedStoreGroups;

        public frmStoreGroupsList()
        {
            InitializeComponent();
            this.selectedStoreGroups = null;
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

        private void frmStoreGroupsList_Load(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
            this.RefreshStoreList();
            this.PreSortListView(this.lsvStoreGroups);
            //PorkAround: http://lab.msdn.microsoft.com/ProductFeedback/viewFeedback.aspx?feedbackId=FDBK49664
            ImageList clonedImageList = new ImageList();
            foreach (Image image in this.imageList1.Images)
            {
                clonedImageList.Images.Add((Image)image.Clone());
            }
            this.lsvStoreGroups.SmallImageList = clonedImageList;
            //PorkAround End
            NetSqlAzMan.SnapIn.Globalization.ResourcesManager.CollectResources(this);
        }

        private void RefreshStoreList()
        {
            this.HourGlass(true);
            this.lsvStoreGroups.Items.Clear();
            IAzManStoreGroup[] storeGroups = this.store.GetStoreGroups();
            foreach (IAzManStoreGroup storeGroup in storeGroups)
            {
                //Show all sids rather than owner, if owner is a Store Group
                if ((this.storeGroup==null) || (this.storeGroup != null && storeGroup.SID.StringValue != this.storeGroup.SID.StringValue))
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Tag = storeGroup;
                    lvi.ImageIndex = 0;
                    lvi.Text = storeGroup.Name;
                    lvi.SubItems.Add(storeGroup.Description);
                    lvi.SubItems.Add(storeGroup.GroupType == GroupType.Basic ? Globalization.MultilanguageResource.GetString("frmApplicationGroupsList_Msg10") : Globalization.MultilanguageResource.GetString("frmApplicationGroupsList_Msg20"));
                    this.lsvStoreGroups.Items.Add(lvi);
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

        private void frmStoreGroupsList_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.None)
                this.DialogResult = DialogResult.Cancel;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.HourGlass(true);
            this.selectedStoreGroups = new IAzManStoreGroup[this.lsvStoreGroups.CheckedItems.Count];
            int index = 0;
            foreach (ListViewItem lvi in this.lsvStoreGroups.CheckedItems)
            {
                this.selectedStoreGroups[index++] = (IAzManStoreGroup)lvi.Tag;
            }
            this.DialogResult = DialogResult.OK;
            this.HourGlass(false);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.selectedStoreGroups = new IAzManStoreGroup[0];
        }

        private void lsvStoreGroups_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            this.SortListView(this.lsvStoreGroups);
        }


    }
}