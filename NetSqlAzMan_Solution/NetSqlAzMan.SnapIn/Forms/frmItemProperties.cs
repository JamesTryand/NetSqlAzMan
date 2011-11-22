using System;
using System.Collections.Specialized;
using System.Drawing;
using System.Windows.Forms;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzMan.SnapIn.Forms
{
    public partial class frmItemProperties : frmBase
    {
        internal IAzManApplication application;
        internal IAzManItem item;
        internal ItemType itemType = ItemType.Operation;
        private StringCollection MembersToAdd;
        private StringCollection MembersToRemove;
        private bool modified;
        private bool firstFocus = false;

        public frmItemProperties()
        {
            InitializeComponent();
            /*Application.DoEvents();*/
            this.modified = false;
            this.MembersToAdd = new StringCollection();
            this.MembersToRemove = new StringCollection();
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

        private void frmItemProperties_Load(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
            //PorkAround: http://lab.msdn.microsoft.com/ProductFeedback/viewFeedback.aspx?feedbackId=FDBK49664
            ImageList clonedImageList1 = new ImageList();
            foreach (Image image in this.imageList1.Images)
            {
                clonedImageList1.Images.Add((Image)image.Clone());
            }
            this.lsvRoles.SmallImageList = clonedImageList1;
            ImageList clonedImageList2 = new ImageList();
            foreach (Image image in this.imageList2.Images)
            {
                clonedImageList2.Images.Add((Image)image.Clone());
            }
            this.lsvTasks.SmallImageList = clonedImageList2;
            ImageList clonedImageList3 = new ImageList();
            foreach (Image image in this.imageList3.Images)
            {
                clonedImageList3.Images.Add((Image)image.Clone());
            }
            this.lsvOperations.SmallImageList = clonedImageList3;
            //PorkAround End
            //Item Properties
            if (this.item != null)
            {
                this.btnAttributes.Enabled = true;
                this.btnBizRule.Enabled = true;
                this.PreSortListView(this.lsvRoles);
                this.PreSortListView(this.lsvTasks);
                this.PreSortListView(this.lsvOperations);
                this.tabControl1.TabPages.Remove(this.TabTasks);
                this.tabControl1.TabPages.Remove(this.TabOperations);
                switch (this.item.ItemType)
                {
                    case ItemType.Role:
                        if (!this.tabControl1.TabPages.Contains(this.TabRoles)) this.tabControl1.TabPages.Add(this.TabRoles);
                        if (!this.tabControl1.TabPages.Contains(this.TabTasks)) this.tabControl1.TabPages.Add(this.TabTasks);
                        if (!this.tabControl1.TabPages.Contains(this.TabOperations)) this.tabControl1.TabPages.Add(this.TabOperations);
                        this.Text = this.lblInfo.Text = Globalization.MultilanguageResource.GetString("frmItemProperties_Msg10");
                        this.tabItemDefinition.Text = Globalization.MultilanguageResource.GetString("frmItemProperties_Msg20");
                        this.picImage.Image = this.picRole.Image;
                        break;
                    case ItemType.Task:
                        if (!this.tabControl1.TabPages.Contains(this.TabTasks)) this.tabControl1.TabPages.Add(this.TabTasks);
                        if (!this.tabControl1.TabPages.Contains(this.TabOperations)) this.tabControl1.TabPages.Add(this.TabOperations);
                        this.tabControl1.TabPages.Remove(this.TabRoles);
                        this.Text = this.lblInfo.Text = Globalization.MultilanguageResource.GetString("frmItemProperties_Msg30");
                        this.tabItemDefinition.Text = Globalization.MultilanguageResource.GetString("frmItemProperties_Msg40");
                        this.picImage.Image = this.picTask.Image;
                        break;
                    case ItemType.Operation:
                        if (!this.tabControl1.TabPages.Contains(this.TabOperations)) this.tabControl1.TabPages.Add(this.TabOperations);
                        this.tabControl1.TabPages.Remove(this.TabRoles);
                        this.tabControl1.TabPages.Remove(this.TabTasks);
                        this.Text = this.lblInfo.Text = Globalization.MultilanguageResource.GetString("frmItemProperties_Msg50");
                        this.tabItemDefinition.Text = Globalization.MultilanguageResource.GetString("frmItemProperties_Msg60");
                        this.picImage.Image = this.picOperation.Image;
                        break;
                }
                if (this.item.Application.Store.Storage.Mode == NetSqlAzManMode.Administrator)
                    this.tabControl1.TabPages.Remove(this.TabOperations);
                this.Text += " - " + this.item.Name;
                this.txtName.Text = this.item.Name;
                this.txtDescription.Text = this.item.Description;
            }
            else
            {
                this.btnAttributes.Enabled = false;
                this.btnBizRule.Enabled = false;
                switch (this.itemType)
                {
                    case ItemType.Role:
                        this.Text = Globalization.MultilanguageResource.GetString("frmItemProperties_Msg70");
                        this.lblInfo.Text = Globalization.MultilanguageResource.GetString("frmItemProperties_Msg80");
                        this.tabItemDefinition.Text = Globalization.MultilanguageResource.GetString("frmItemProperties_Msg20");
                        this.picImage.Image = this.picRole.Image;
                        break;
                    case ItemType.Task:
                        this.Text = Globalization.MultilanguageResource.GetString("frmItemProperties_Msg90");
                        this.lblInfo.Text = Globalization.MultilanguageResource.GetString("frmItemProperties_Msg100");
                        this.tabItemDefinition.Text = Globalization.MultilanguageResource.GetString("frmItemProperties_Msg40");
                        this.picImage.Image = this.picTask.Image;
                        break;
                    case ItemType.Operation:
                        this.Text = Globalization.MultilanguageResource.GetString("frmItemProperties_Msg110");
                        this.lblInfo.Text = Globalization.MultilanguageResource.GetString("frmItemProperties_Msg120");
                        this.tabItemDefinition.Text = Globalization.MultilanguageResource.GetString("frmItemProperties_Msg60");
                        this.picImage.Image = this.picOperation.Image;
                        break;
                }
                this.tabControl1.TabPages.Remove(this.TabRoles);
                this.tabControl1.TabPages.Remove(this.TabTasks);
                this.tabControl1.TabPages.Remove(this.TabOperations);
            }
            this.RefreshItems();
            this.modified = false;
            this.btnApply.Enabled = false;
            this.txtName.Focus();
            this.txtName.SelectAll();
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

        private void frmItemProperties_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.None)
                this.DialogResult = DialogResult.Cancel;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            this.modified = true;
            if (String.IsNullOrEmpty(this.txtName.Text.Trim()))
            {
                this.btnOk.Enabled = this.btnApply.Enabled = false;
                this.errorProvider1.SetError(this.txtName, Globalization.MultilanguageResource.GetString("frmItemProperties_Msg130"));
            }
            else
            {
                this.btnOk.Enabled = this.btnApply.Enabled = true;
                this.errorProvider1.SetError(this.txtName, String.Empty);
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                this.HourGlass(true);
                this.commitChanges();
                this.btnApply.Enabled = false;
                this.HourGlass(false);
                this.DialogResult = DialogResult.OK;
                this.HourGlass(false);
            }
            catch (Exception ex)
            {
                this.HourGlass(false);
                this.DialogResult = DialogResult.None;
                if (this.item == null)
                {
                    this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("frmItemProperties_Msg140"));
                }
                if (this.item != null)
                {
                    this.item.Application.Store.Storage.RollBackTransaction();
                    this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("frmItemProperties_Msg150"));
                }
            }
        }

        private void commitChanges()
        {
            try
            {
                if (this.item == null)
                {
                    this.item = this.application.CreateItem(this.txtName.Text.Trim(), this.txtDescription.Text.Trim(), this.itemType);
                    this.frmItemProperties_Load(this, EventArgs.Empty);
                }
                else
                {
                    this.item.Application.Store.Storage.BeginTransaction(AzManIsolationLevel.ReadUncommitted);
                    this.item.Rename(this.txtName.Text.Trim());
                    this.item.Update(this.txtDescription.Text.Trim());
                    //Members
                    //Members To Add
                    foreach (string member in this.MembersToAdd)
                    {
                        IAzManItem item = this.item.Application.GetItem(member);
                        this.item.AddMember(item);
                    }
                    //Members To Remove
                    foreach (string member in this.MembersToRemove)
                    {
                        IAzManItem item = this.item.Application.GetItem(member);
                        this.item.RemoveMember(item);
                    }
                    this.MembersToAdd.Clear();
                    this.MembersToRemove.Clear();
                    this.modified = false;
                    this.item.Application.Store.Storage.CommitTransaction();
                }
                this.HourGlass(false);
            }
            catch
            {
                this.HourGlass(false);
                if (this.item!=null && this.item.Application.Store.Storage.TransactionInProgress)
                    this.item.Application.Store.Storage.RollBackTransaction();
                throw;
            }
        }

        private bool FindMember(IAzManItem[] members, string name)
        {
            foreach (IAzManItem m in members)
            {
                if (m.Name == name)
                    return true;
            }
            return false;
        }

        private bool FindMember(GenericMemberCollection members, string name)
        {
            foreach (GenericMember m in members)
            {
                if (m.Name == name)
                    return true;
            }
            return false;
        }

        private void btnAddOperation_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
            frmItemsList frm = new frmItemsList();
            frm.application = this.item.Application;
            frm.item = this.item;
            frm.itemType = ItemType.Operation;
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                foreach (IAzManItem item in frm.selectedItems)
                {
                    if (!this.MembersToRemove.Contains(item.Name))
                    {
                        if (!this.MembersToAdd.Contains(item.Name) && !this.FindMember(this.item.GetMembers(), item.Name))
                        {
                            this.MembersToAdd.Add(item.Name);
                            this.modified = true;
                        }
                    }
                    else
                    {
                        this.MembersToRemove.Remove(item.Name);
                    }
                }
                this.RefreshItems();
            }
            this.HourGlass(false);
        }

        private void btnRemoveOperation_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
            this.HourGlass(true);
            foreach (ListViewItem lvi in this.lsvOperations.CheckedItems)
            {
                if ((lvi.Tag as IAzManItem) != null)
                {
                    IAzManItem lviTag = (IAzManItem)(lvi.Tag);
                    this.MembersToRemove.Add(lviTag.Name);
                    this.modified = true;
                }
                else if ((lvi.Tag as GenericMember) != null)
                {
                    GenericMember lviTag = (GenericMember)(lvi.Tag);
                    if (this.MembersToAdd.Contains(lviTag.Name))
                    {
                        this.MembersToAdd.Remove(lviTag.Name);
                        this.modified = true;
                    }
                }
            }
            this.RefreshItems();
            if (this.lsvOperations.Items.Count == 0 || this.lsvOperations.CheckedItems.Count == 0)
                this.btnRemoveOperation.Enabled = false;
            this.HourGlass(false);
        }

        private void lsvOperations_Check(object sender, ItemCheckEventArgs e)
        {
            if (this.lsvOperations.CheckedItems.Count == 1 && e.CurrentValue == CheckState.Checked && e.NewValue == CheckState.Unchecked)
                this.btnRemoveOperation.Enabled = false;
            else
                this.btnRemoveOperation.Enabled = true;
        }

        private ListViewItem CreateListViewItem(string member)
        {
            ListViewItem lvi = new ListViewItem();
            IAzManItem item = this.item.Application.GetItem(member);
            lvi.Tag = new GenericMember(item.Name, item.Description);
            lvi.Text = member;
            lvi.ImageIndex = 0;
            lvi.SubItems.Add(item.Description);
            return lvi;
        }

        private ListViewItem CreateListViewItem(IAzManItem member)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.Tag = member;
            lvi.Text = member.Name;
            lvi.ImageIndex = 0;
            lvi.SubItems.Add(member.Description);
            return lvi;
        }

        private void RemoveListViewItem(ref ListView lsv, string member)
        {
            foreach (ListViewItem lvi in lsv.Items)
            {
                if (lvi.Text == member)
                {
                    lvi.Remove();
                    return;
                }
            }
        }

        private void RefreshItems()
        {
            this.HourGlass(true);
            if (this.item != null) // Item Properties
            {
                //Members
                //Add committed sids 
                this.lsvRoles.Items.Clear();
                this.lsvTasks.Items.Clear();
                this.lsvOperations.Items.Clear();
                IAzManItem[] members = this.item.GetMembers();
                foreach (IAzManItem member in members)
                {
                    switch (member.ItemType)
                    { 
                        case ItemType.Role:
                            this.lsvRoles.Items.Add(this.CreateListViewItem(member));
                            break;
                        case ItemType.Task:
                            this.lsvTasks.Items.Add(this.CreateListViewItem(member));
                            break;
                        case ItemType.Operation:
                            this.lsvOperations.Items.Add(this.CreateListViewItem(member));
                            break;
                    }
                }
                //Add uncommitted sids 
                foreach (string member in this.MembersToAdd)
                {
                    IAzManItem m = this.application.GetItem(member);
                    switch (m.ItemType)
                    {
                        case ItemType.Role:
                            this.lsvRoles.Items.Add(this.CreateListViewItem(member));
                            break;
                        case ItemType.Task:
                            this.lsvTasks.Items.Add(this.CreateListViewItem(member));
                            break;
                        case ItemType.Operation:
                            this.lsvOperations.Items.Add(this.CreateListViewItem(member));
                            break;
                    }
                }
                //Remove uncommitted sids
                foreach (string member in this.MembersToRemove)
                {
                    IAzManItem m = this.application.GetItem(member);
                    switch (m.ItemType)
                    {
                        case ItemType.Role:
                            this.RemoveListViewItem(ref this.lsvRoles, member);
                            break;
                        case ItemType.Task:
                            this.RemoveListViewItem(ref this.lsvTasks, member);
                            break;
                        case ItemType.Operation:
                            this.RemoveListViewItem(ref this.lsvOperations, member);
                            break;
                    }
                }
            }
            this.btnApply.Enabled = this.modified;
            if (!this.application.IAmManager)
                this.txtName.Enabled = this.txtDescription.Enabled = this.btnOk.Enabled = this.btnApply.Enabled =
                    this.lsvRoles.Enabled = this.lsvTasks.Enabled = this.lsvOperations.Enabled = 
                    this.btnAddRole.Enabled = this.btnAddTask.Enabled = this.btnAddOperation.Enabled =
                    this.btnRemoveRole.Enabled = this.btnRemoveTask.Enabled = this.btnRemoveOperation.Enabled = false;
            this.HourGlass(false);
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
            try
            {
                this.HourGlass(true);
                this.commitChanges();
                this.btnApply.Enabled = false;
                this.btnAttributes.Enabled = true;
                this.HourGlass(false);
            }
            catch (Exception ex)
            {
                this.HourGlass(false);
                if (this.item == null)
                {
                    this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("frmItemProperties_Msg140"));
                }
                if (this.item != null)
                {
                    this.item.Application.Store.Storage.RollBackTransaction();
                    this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("frmItemProperties_Msg150"));
                }
            }
        }

        private void frmItemProperties_Activated(object sender, EventArgs e)
        {
            if (!this.firstFocus)
            {
                this.txtName.Focus();
                this.firstFocus = true;
            }
        }

        private void lsvTasks_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (this.lsvTasks.CheckedItems.Count == 1 && e.CurrentValue == CheckState.Checked && e.NewValue == CheckState.Unchecked)
                this.btnRemoveTask.Enabled = false;
            else
                this.btnRemoveTask.Enabled = true && this.item.Application.IAmManager;
        }

        private void lsvRoles_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (this.lsvRoles.CheckedItems.Count == 1 && e.CurrentValue == CheckState.Checked && e.NewValue == CheckState.Unchecked)
                this.btnRemoveRole.Enabled = false;
            else
                this.btnRemoveRole.Enabled = true && this.item.Application.IAmManager;
        }

        private void btnAddRole_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
            frmItemsList frm = new frmItemsList();
            frm.application = this.item.Application;
            frm.item = this.item;
            frm.itemType = ItemType.Role;
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                foreach (IAzManItem item in frm.selectedItems)
                {
                    if (!this.MembersToRemove.Contains(item.Name))
                    {
                        if (!this.MembersToAdd.Contains(item.Name) && !this.FindMember(this.item.GetMembers(), item.Name))
                        {
                            this.MembersToAdd.Add(item.Name);
                            this.modified = true;
                        }
                    }
                    else
                    {
                        this.MembersToRemove.Remove(item.Name);
                    }
                }
                this.RefreshItems();
            }
            this.HourGlass(false);
        }

        private void btnAddTask_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
            frmItemsList frm = new frmItemsList();
            frm.application = this.item.Application;
            frm.item = this.item;
            frm.itemType = ItemType.Task;
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                foreach (IAzManItem item in frm.selectedItems)
                {
                    if (!this.MembersToRemove.Contains(item.Name))
                    {
                        if (!this.MembersToAdd.Contains(item.Name) && !this.FindMember(this.item.GetMembers(), item.Name))
                        {
                            this.MembersToAdd.Add(item.Name);
                            this.modified = true;
                        }
                    }
                    else
                    {
                        this.MembersToRemove.Remove(item.Name);
                    }
                }
                this.RefreshItems();
            }
            this.HourGlass(false);
        }

        private void btnRemoveRole_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
            this.HourGlass(true);
            foreach (ListViewItem lvi in this.lsvRoles.CheckedItems)
            {
                if ((lvi.Tag as IAzManItem) != null)
                {
                    IAzManItem lviTag = (IAzManItem)(lvi.Tag);
                    this.MembersToRemove.Add(lviTag.Name);
                    this.modified = true;
                }
                else if ((lvi.Tag as GenericMember) != null)
                {
                    GenericMember lviTag = (GenericMember)(lvi.Tag);
                    if (this.MembersToAdd.Contains(lviTag.Name))
                    {
                        this.MembersToAdd.Remove(lviTag.Name);
                        this.modified = true;
                    }
                }
            }
            this.RefreshItems();
            if (this.lsvRoles.Items.Count == 0 || this.lsvRoles.CheckedItems.Count == 0)
                this.btnRemoveRole.Enabled = false;
            this.HourGlass(false);
        }

        private void btnRemoveTask_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
            this.HourGlass(true);
            foreach (ListViewItem lvi in this.lsvTasks.CheckedItems)
            {
                if ((lvi.Tag as IAzManItem) != null)
                {
                    IAzManItem lviTag = (IAzManItem)(lvi.Tag);
                    this.MembersToRemove.Add(lviTag.Name);
                    this.modified = true;
                }
                else if ((lvi.Tag as GenericMember) != null)
                {
                    GenericMember lviTag = (GenericMember)(lvi.Tag);
                    if (this.MembersToAdd.Contains(lviTag.Name))
                    {
                        this.MembersToAdd.Remove(lviTag.Name);
                        this.modified = true;
                    }
                }
            }
            this.RefreshItems();
            if (this.lsvTasks.Items.Count == 0 || this.lsvTasks.CheckedItems.Count == 0)
                this.btnRemoveTask.Enabled = false;
            this.HourGlass(false);
        }

        private void lsvRoles_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            this.SortListView(this.lsvRoles);
        }

        private void lsvTasks_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            this.SortListView(this.lsvTasks);
        }

        private void lsvOperations_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            this.SortListView(this.lsvOperations);
        }

        private void btnAttributes_Click(object sender, EventArgs e)
        {
            try
            {
                frmItemAttributes frm = new frmItemAttributes();
                frm.Text += " - " + this.item.Name;
                frm.item = this.item;
                frm.ShowDialog(this);
                /*Application.DoEvents();*/
                frm.Dispose();
                /*Application.DoEvents();*/
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("frmItemProperties_Msg160"));
            }
        }

        private void btnBizRule_Click(object sender, EventArgs e)
        {
            try
            {
                frmBizRule frm = new frmBizRule();
                frm.Text = Globalization.MultilanguageResource.GetString("frmItemProperties_Msg170") + this.item.Name;
                frm.item = this.item;
                frm.ShowDialog(this);
                /*Application.DoEvents();*/
                frm.Dispose();
                /*Application.DoEvents();*/
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("frmItemProperties_Msg180"));
            }
        }
    }
}