using System;
using System.Drawing;
using System.Windows.Forms;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzMan.SnapIn.Forms
{
    public partial class frmItemsList : frmBase
    {
        internal IAzManApplication application;
        internal IAzManItem item;
        internal ItemType itemType;
        internal IAzManItem[] selectedItems;

        public frmItemsList()
        {
            InitializeComponent();
            this.selectedItems = null;
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

        private void frmItemsList_Load(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
            //PorkAround: http://lab.msdn.microsoft.com/ProductFeedback/viewFeedback.aspx?feedbackId=FDBK49664
            ImageList clonedImageList1 = new ImageList();
            foreach (Image image in this.imageList1.Images)
            {
                clonedImageList1.Images.Add((Image)image.Clone());
            }
            ImageList clonedImageList2 = new ImageList();
            foreach (Image image in this.imageList2.Images)
            {
                clonedImageList2.Images.Add((Image)image.Clone());
            }
            ImageList clonedImageList3 = new ImageList();
            foreach (Image image in this.imageList3.Images)
            {
                clonedImageList3.Images.Add((Image)image.Clone());
            }
            //PorkAround End
            this.PreSortListView(this.lsvItems);
            switch (this.itemType)
            {
                case ItemType.Role: 
                    this.picImage.Image = this.picRole.Image;
                    this.lblInfo1.Text = this.lblInfo2.Text = Globalization.MultilanguageResource.GetString("frmItemsList_Msg10");
                    this.lsvItems.SmallImageList = clonedImageList1;
                    break;
                case ItemType.Task: 
                    this.picImage.Image = this.picTask.Image;
                    this.lblInfo1.Text = this.lblInfo2.Text = Globalization.MultilanguageResource.GetString("frmItemsList_Msg20");
                    this.lsvItems.SmallImageList = clonedImageList2;
                    break;
                case ItemType.Operation: 
                    this.picImage.Image = this.picOperation.Image;
                    this.lblInfo1.Text = this.lblInfo2.Text = Globalization.MultilanguageResource.GetString("frmItemsList_Msg30");
                    this.lsvItems.SmallImageList = clonedImageList3;
                    break;
            }
            this.Text = this.lblInfo1.Text;
            NetSqlAzMan.SnapIn.Globalization.ResourcesManager.CollectResources(this);
        }

        private void RefreshItemsList()
        {
            this.HourGlass(true);
            this.lsvItems.Items.Clear();
            IAzManItem[] members = this.application.GetItems(this.itemType);
            foreach (IAzManItem member in members)
            {
                //Show all sids rather than owner
                if ((this.item==null) || (this.item != null && member.Name != this.item.Name))
                {
                    ListViewItem lvi = new ListViewItem();
                    lvi.Tag = member; 
                    lvi.UseItemStyleForSubItems = false;
                    lvi.Text = member.Name;
                    lvi.SubItems.Add(member.Description);
                    lvi.ImageIndex = 0;
                    this.lsvItems.Items.Add(lvi);
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

        private void frmItemsList_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.None)
                this.DialogResult = DialogResult.Cancel;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            this.HourGlass(true);
            this.selectedItems = new IAzManItem[this.lsvItems.CheckedItems.Count];
            int index = 0;
            foreach (ListViewItem lvi in this.lsvItems.CheckedItems)
            {
                this.selectedItems[index++] = (IAzManItem)lvi.Tag;
            }
            this.DialogResult = DialogResult.OK;
            this.HourGlass(false);
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.selectedItems = new IAzManItem[0];
            this.DialogResult = DialogResult.Cancel;
        }

        private void frmItemsList_Activated(object sender, EventArgs e)
        {
            if (this.lsvItems.CheckedItems.Count==0)
                this.RefreshItemsList();
        }

        private void lsvItems_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            this.SortListView(this.lsvItems);
        }
    }
}