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
    public partial class frmStoreGroupsProperties : frmBase
    {
        internal IAzManStoreGroup storeGroup;
        private GenericMemberCollection MembersToAdd;
        private GenericMemberCollection MembersToRemove;
        private GenericMemberCollection NonMembersToAdd;
        private GenericMemberCollection NonMembersToRemove;
        private bool modified;
        private bool firstShow;

        public frmStoreGroupsProperties()
        {
            InitializeComponent();
            this.firstShow = true;
            this.MembersToAdd = new GenericMemberCollection();
            this.MembersToRemove = new GenericMemberCollection();
            this.NonMembersToAdd = new GenericMemberCollection();
            this.NonMembersToRemove = new GenericMemberCollection();
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

        private void frmGroupsProperties_Load(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
            //PorkAround: http://lab.msdn.microsoft.com/ProductFeedback/viewFeedback.aspx?feedbackId=FDBK49664
            ImageList clonedImageList = new ImageList();
            foreach (Image image in this.imageList1.Images)
            {
                clonedImageList.Images.Add((Image)image.Clone());
            }
            this.lsvMembers.SmallImageList = clonedImageList;
            this.lsvNonMembers.SmallImageList = clonedImageList;
            //PorkAround End
            this.txtName.Text = this.storeGroup.Name;
            this.txtDescription.Text = this.storeGroup.Description;
            this.txtGroupType.Text = (this.storeGroup.GroupType == GroupType.Basic ? Globalization.MultilanguageResource.GetString("frmApplicationGroupsList_Msg10") : Globalization.MultilanguageResource.GetString("frmApplicationGroupsList_Msg20"));
           
            if (this.storeGroup.GroupType==GroupType.Basic)
            {
                this.btnMembersAddStoreGroup.Enabled = this.btnNonMembersAddStoreGroup.Enabled = this.storeGroup.Store.HasStoreGroups();
                if (this.tabControl1.TabPages.Contains(this.tabLdapQuery))
                    this.tabControl1.TabPages.Remove(this.tabLdapQuery);
                this.lsvMembers.Items.Clear();
                this.lsvNonMembers.Items.Clear();
                this.picImage.Image = this.picBasic.Image;
                this.PreSortListView(this.lsvMembers);
                this.PreSortListView(this.lsvNonMembers);
            }
            else
            {
                if (this.tabControl1.Contains(this.tabMembers))
                    this.tabControl1.TabPages.Remove(this.tabMembers);
                if (this.tabControl1.Contains(this.tabNonMembers))
                    this.tabControl1.TabPages.Remove(this.tabNonMembers);
                this.picImage.Image = this.picLDap.Image;
            }
            this.RefreshStoreGroupProperties();
            this.modified = false;
            this.btnApply.Enabled = false;
            this.txtName.Focus();
            this.txtName.SelectAll();
            NetSqlAzMan.SnapIn.Globalization.ResourcesManager.CollectResources(this);
        }

        private ListViewItem CreateStoreListViewItem(IAzManStoreGroup member)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.Tag = member;
            lvi.Text = member.Name;
            lvi.SubItems.Add(Globalization.MultilanguageResource.GetString("WhereDefined_Store"));
            return lvi;
        }

        private ListViewItem CreateLDapListViewItem(IAzManStoreGroupMember member)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.Tag = member;
            string displayName;
            member.GetMemberInfo(out displayName);
            lvi.Text = displayName;
            lvi.SubItems.Add(Globalization.MultilanguageResource.GetString("WhereDefined_LDAP"));
            return lvi;
        }

        private ListViewItem CreateDBListViewItem(IAzManStoreGroupMember member)
        {
            ListViewItem lvi = new ListViewItem();
            if (member != null)
            {
                lvi.Tag = member;
                string displayName;
                member.GetMemberInfo(out displayName);
                lvi.Text = displayName;
                lvi.SubItems.Add(Globalization.MultilanguageResource.GetString("WhereDefined_DB"));
            }
            return lvi;
        }

        private ListViewItem CreateListViewItem(GenericMember member)
        {
            ListViewItem lvi = new ListViewItem();
            if (member != null)
            {
                lvi.Tag = member;
                lvi.Text = member.Name;
                switch (member.WhereDefined.ToString())
                {
                    case "LDAP": lvi.SubItems.Add(Globalization.MultilanguageResource.GetString("WhereDefined_LDAP")); break;
                    case "Local": lvi.SubItems.Add(Globalization.MultilanguageResource.GetString("WhereDefined_Local")); break;
                    case "Database": lvi.SubItems.Add(Globalization.MultilanguageResource.GetString("WhereDefined_DB")); break;
                    case "Store": lvi.SubItems.Add(Globalization.MultilanguageResource.GetString("WhereDefined_Store")); break;
                    case "Application": lvi.SubItems.Add(Globalization.MultilanguageResource.GetString("WhereDefined_Application")); break;
                }
            }
            return lvi;
        }

        private void RemoveListViewItem(ref ListView lsv, GenericMember member)
        {
            foreach (ListViewItem lvi in lsv.Items)
            {
                string objectSid = null;
                if ((lvi.Tag as GenericMember)!=null)
                    objectSid = ((GenericMember)lvi.Tag).sid.StringValue;
                else if ((lvi.Tag as IAzManStoreGroup)!=null)
                    objectSid = ((IAzManStoreGroup)lvi.Tag).SID.StringValue;
                else if ((lvi.Tag as IAzManStoreGroupMember)!=null)
                    objectSid = ((IAzManStoreGroupMember)lvi.Tag).SID.StringValue;
                if (objectSid != null)
                {
                    if (member.sid.StringValue==objectSid)
                    {
                        lvi.Remove();
                        return;
                    }
                }
            }
        }

        private void RefreshStoreGroupProperties()
        {
            this.HourGlass(true);
            if (this.storeGroup.GroupType == GroupType.Basic)
            {
                //Members
                //Add committed sids 
                this.lsvMembers.Items.Clear();
                IAzManStoreGroupMember[] storeGroupMembers = this.storeGroup.GetStoreGroupMembers();
                foreach (IAzManStoreGroupMember storeGroupMember in storeGroupMembers)
                {
                    //Store Groups
                    if (storeGroupMember.WhereDefined == WhereDefined.Store)
                        this.lsvMembers.Items.Add(this.CreateStoreListViewItem(storeGroupMember.StoreGroup.Store.GetStoreGroup(storeGroupMember.SID)));
                    //Ldap Object
                    if (storeGroupMember.WhereDefined == WhereDefined.LDAP || storeGroupMember.WhereDefined == WhereDefined.Local)
                    {
                        this.lsvMembers.Items.Add(this.CreateLDapListViewItem(storeGroupMember));
                    }
                    //DB Users
                    if (storeGroupMember.WhereDefined == WhereDefined.Database)
                        this.lsvMembers.Items.Add(this.CreateDBListViewItem(storeGroupMember));

                }
                //Add uncommitted sids 
                foreach (GenericMember member in this.MembersToAdd)
                {
                    this.lsvMembers.Items.Add(this.CreateListViewItem(member));
                }
                //Remove uncommitted sids
                foreach (GenericMember member in this.MembersToRemove)
                {
                    this.RemoveListViewItem(ref this.lsvMembers, member);
                }
                
                //Non-Members
                //Add committed non-sids 
                this.lsvNonMembers.Items.Clear();
                IAzManStoreGroupMember[] storeGroupNonMembers = this.storeGroup.GetStoreGroupNonMembers();
                foreach (IAzManStoreGroupMember storeGroupNonMember in storeGroupNonMembers)
                {
                    //Store Groups
                    if (storeGroupNonMember.WhereDefined == WhereDefined.Store)
                        this.lsvNonMembers.Items.Add(this.CreateStoreListViewItem(storeGroupNonMember.StoreGroup.Store.GetStoreGroup(storeGroupNonMember.SID)));
                    //Ldap Object
                    if (storeGroupNonMember.WhereDefined == WhereDefined.LDAP || storeGroupNonMember.WhereDefined == WhereDefined.Local)
                        this.lsvNonMembers.Items.Add(this.CreateLDapListViewItem(storeGroupNonMember));
                    //DB Users
                    if (storeGroupNonMember.WhereDefined == WhereDefined.Database)
                        this.lsvNonMembers.Items.Add(this.CreateDBListViewItem(storeGroupNonMember));
                }
                //Add uncommitted non-sids 
                foreach (GenericMember nonmember in this.NonMembersToAdd)
                {
                    this.lsvNonMembers.Items.Add(this.CreateListViewItem(nonmember));
                }
                //Remove uncommitted non-sids
                foreach (GenericMember nonmember in this.NonMembersToRemove)
                {
                    this.RemoveListViewItem(ref this.lsvNonMembers, nonmember);
                }
                this.btnApply.Enabled = this.modified;
                if (!this.storeGroup.Store.IAmManager)
                    this.txtName.Enabled = this.txtDescription.Enabled = this.btnOK.Enabled = this.btnApply.Enabled =
                        this.btnMembersAddStoreGroup.Enabled = this.btnMembersAddDBUsers.Enabled = this.btnMembersAddWindowsUsersAndGroups.Enabled =
                        this.btnMembersRemove.Enabled =
                        this.lsvMembers.Enabled = this.lsvNonMembers.Enabled =
                        this.btnNonMembersAddStoreGroup.Enabled = this.btnNonMembersAddDBUsers.Enabled = this.btnNonMembersAddWindowsUsersAndGroup.Enabled =
                        this.btnNonMembersRemove.Enabled = false;
            }
            else //Ldap Query
            {
                this.txtGroupType.Text = Globalization.MultilanguageResource.GetString("frmApplicationGroupsProperties_Msg10");
                this.txtLDapQuery.Text = this.storeGroup.LDAPQuery;
                this.btnApply.Enabled = this.modified;
                if (!this.storeGroup.Store.IAmManager)
                    this.txtName.Enabled = this.txtDescription.Enabled = this.txtLDapQuery.Enabled = this.btnOK.Enabled = this.btnApply.Enabled = false;
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

        private void frmGroupsProperties_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.None)
                this.DialogResult = DialogResult.Cancel;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (this.txtName.Text.Trim().Length > 0)
            {
                this.btnOK.Enabled = true;
                this.errorProvider1.SetError(this.txtName, String.Empty);
            }
            else
            {
                this.btnOK.Enabled = false;
                this.errorProvider1.SetError(this.txtName, Globalization.MultilanguageResource.GetString("frmStoreGroupsProperties_Msg10"));
            }
            this.modified = true;
            this.btnApply.Enabled = true;
        }

        private bool FindMember(IAzManStoreGroupMember[] members, IAzManSid sid)
        {
            foreach (IAzManStoreGroupMember m in members)
            {
                if (m.SID.StringValue == sid.StringValue)
                    return true;
            }
            return false;
        }

        private bool FindMember(GenericMemberCollection members, IAzManSid sid)
        {
            foreach (GenericMember m in members)
            {
                if (m.sid.StringValue == sid.StringValue)
                    return true;
            }
            return false;
        }

        private void btnMembersAddStoreGroups_Click(object sender, EventArgs e)
        {
            frmStoreGroupsList frm = new frmStoreGroupsList();
            frm.store = this.storeGroup.Store;
            frm.storeGroup = this.storeGroup;
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                foreach (IAzManStoreGroup sg in frm.selectedStoreGroups)
                {
                    if (!this.MembersToRemove.Remove(sg.SID.StringValue))
                    {
                        if (!this.MembersToAdd.ContainsByObjectSid(sg.SID.StringValue) && !this.FindMember(this.storeGroup.GetStoreGroupMembers(), sg.SID))
                        {
                            this.MembersToAdd.Add(new GenericMember(sg.Name, sg.SID, WhereDefined.Store));
                            this.modified = true;
                        }
                    }
                }
                this.RefreshStoreGroupProperties();
            }
            this.HourGlass(false);
        }

        private void btnMembersAddWindowsUsersAndGroups_Click(object sender, EventArgs e)
        {
            try
            {
                this.HourGlass(true);
                ADObject[] res = DirectoryServicesUtils.ADObjectPickerShowDialog(this, this.storeGroup.Store.Storage.Mode==NetSqlAzManMode.Developer);
                /*Application.DoEvents();*/
                if (res != null)
                {
                    foreach (ADObject o in res)
                    {
                        if (!this.MembersToRemove.Remove(o.Sid) && !this.FindMember(this.storeGroup.GetStoreGroupMembers(), new SqlAzManSID(o.Sid)) && !this.FindMember(this.MembersToAdd, new SqlAzManSID(o.Sid)))
                        {
                            WhereDefined wd = WhereDefined.LDAP;
                            if (!o.ADSPath.StartsWith("LDAP"))
                                wd = WhereDefined.Local;
                            this.MembersToAdd.Add(new GenericMember(o.Name, new SqlAzManSID(o.Sid), wd));
                            this.modified = true;
                        }
                    }
                    this.RefreshStoreGroupProperties();
                }
                this.HourGlass(false);
            }
            catch (Exception ex)
            {
                this.HourGlass(false);
                MessageBox.Show(ex.Message);
            }
        }

        private void btnMembersRemove_Click(object sender, EventArgs e)
        {
            this.HourGlass(true);
            foreach (ListViewItem lvi in this.lsvMembers.SelectedItems)
            {
                if ((lvi.Tag as IAzManStoreGroup) != null)
                {
                    IAzManStoreGroup lviTag = (IAzManStoreGroup)(lvi.Tag);
                    this.MembersToRemove.Add(new GenericMember(lviTag.Name, lviTag.SID, WhereDefined.Store));
                    this.modified = true;
                }
                else if ((lvi.Tag as IAzManStoreGroupMember) != null)
                {
                    IAzManStoreGroupMember lviTag = (IAzManStoreGroupMember)(lvi.Tag);
                    this.MembersToRemove.Add(new GenericMember(lviTag.SID.StringValue, lviTag.SID, WhereDefined.LDAP));
                    this.modified = true;
                }
                else if ((lvi.Tag as GenericMember) != null)
                {
                    GenericMember lviTag = (GenericMember)(lvi.Tag);
                    if (this.MembersToAdd.Remove(lviTag.sid.StringValue))
                        this.modified = true;
                }
            }
            this.RefreshStoreGroupProperties();
            if (this.lsvMembers.Items.Count == 0)
                this.btnMembersRemove.Enabled = false;
            this.HourGlass(false);
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                this.HourGlass(true);
                this.CommitChanges();
                this.btnApply.Enabled = false;
                this.HourGlass(false);
            }
            catch (Exception ex)
            {
                this.HourGlass(false);
                this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("UpdateError_Msg10"));
            }
        }

        private void CommitChanges()
        {
            try
            {
                if (!this.modified)
                    return;
                //Store Group Properties
                this.storeGroup.Store.Storage.BeginTransaction(AzManIsolationLevel.ReadUncommitted);
                this.storeGroup.Rename(this.txtName.Text.Trim());
                this.storeGroup.Update(this.storeGroup.SID, this.txtDescription.Text.Trim(), this.storeGroup.GroupType);
                if (this.storeGroup.GroupType == GroupType.Basic)
                {
                    //Members
                    //Members To Add
                    foreach (GenericMember member in this.MembersToAdd)
                    {
                        this.storeGroup.CreateStoreGroupMember(member.sid, member.WhereDefined, true);
                    }
                    //Members To Remove
                    foreach (GenericMember member in this.MembersToRemove)
                    {
                        this.storeGroup.GetStoreGroupMember(member.sid).Delete();
                    }
                    //Non Members
                    //Non Members To Add
                    foreach (GenericMember nonMember in this.NonMembersToAdd)
                    {
                        this.storeGroup.CreateStoreGroupMember(nonMember.sid, nonMember.WhereDefined, false);
                    }
                    //Non Members To Remove
                    foreach (GenericMember nonMember in this.NonMembersToRemove)
                    {
                        this.storeGroup.GetStoreGroupMember(nonMember.sid).Delete();
                    }
                    this.MembersToAdd.Clear();
                    this.MembersToRemove.Clear();
                    this.NonMembersToAdd.Clear();
                    this.NonMembersToRemove.Clear();
                    this.modified = false;
                }
                else
                {
                    this.storeGroup.UpdateLDapQuery(this.txtLDapQuery.Text.Trim());
                }
                this.storeGroup.Store.Storage.CommitTransaction();
            }
            catch
            {
                this.storeGroup.Store.Storage.RollBackTransaction();
                throw;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                this.HourGlass(true);
                this.CommitChanges();
                this.btnApply.Enabled = false;
                this.DialogResult = DialogResult.OK;
                this.HourGlass(false);
            }
            catch (Exception ex)
            {
                this.HourGlass(false);
                this.DialogResult = DialogResult.None;
                this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("UpdateError_Msg10"));
            }
        }

        private void txtDescription_TextChanged(object sender, EventArgs e)
        {
            this.modified = true;
            this.btnApply.Enabled = true;
        }

        private void btnNonMembersAddStoreGroup_Click(object sender, EventArgs e)
        {
            this.HourGlass(true);
            frmStoreGroupsList frm = new frmStoreGroupsList();
            frm.store = this.storeGroup.Store;
            frm.storeGroup = this.storeGroup;
            DialogResult dr = frm.ShowDialog(this);
            this.HourGlass(true);
            if (dr == DialogResult.OK)
            {
                foreach (IAzManStoreGroup sg in frm.selectedStoreGroups)
                {
                    if (!this.NonMembersToRemove.Remove(sg.SID.StringValue))
                    {
                        if (!this.NonMembersToAdd.ContainsByObjectSid(sg.SID.StringValue) && !this.FindMember(this.storeGroup.GetStoreGroupNonMembers(), sg.SID))
                        {
                            this.NonMembersToAdd.Add(new GenericMember(sg.Name, sg.SID, WhereDefined.Store));
                            this.modified = true;
                        }
                    }
                }
                this.RefreshStoreGroupProperties();
            }
            this.HourGlass(false);
        }

        private void btnNonMembersAddWindowsUsersAndGroup_Click(object sender, EventArgs e)
        {
            this.HourGlass(true);
            ADObject[] res = DirectoryServicesUtils.ADObjectPickerShowDialog(this, this.storeGroup.Store.Storage.Mode == NetSqlAzManMode.Developer);
            if (res != null)
            {
                foreach (ADObject o in res)
                {
                    if (!this.NonMembersToRemove.Remove(o.Sid) && !this.FindMember(this.storeGroup.GetStoreGroupNonMembers(), new SqlAzManSID(o.Sid)) && !this.FindMember(this.NonMembersToAdd, new SqlAzManSID(o.Sid)))
                    {
                        WhereDefined wd = WhereDefined.LDAP;
                        if (!o.ADSPath.StartsWith("LDAP"))
                            wd = WhereDefined.Local;
                        this.NonMembersToAdd.Add(new GenericMember(o.Name, new SqlAzManSID(o.Sid), wd));
                        this.modified = true;
                    }
                }
                this.RefreshStoreGroupProperties();
            }
            this.HourGlass(false);
        }

        private void btnNonMembersRemove_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem lvi in this.lsvNonMembers.SelectedItems)
            {
                if ((lvi.Tag as IAzManStoreGroup) != null)
                {
                    IAzManStoreGroup lviTag = (IAzManStoreGroup)lvi.Tag;
                    this.NonMembersToRemove.Add(new GenericMember(lviTag.Name, lviTag.SID, WhereDefined.Store));
                    this.modified = true;
                }
                else if ((lvi.Tag as IAzManStoreGroupMember) != null)
                {
                    IAzManStoreGroupMember lviTag = (IAzManStoreGroupMember)lvi.Tag;
                    this.NonMembersToRemove.Add(new GenericMember(lviTag.SID.StringValue, lviTag.SID, WhereDefined.LDAP));
                    this.modified = true;
                }
                else if ((lvi.Tag as GenericMember) != null)
                {
                    GenericMember lviTag = (GenericMember)lvi.Tag;
                    if (this.NonMembersToAdd.Remove(lviTag.sid.StringValue))
                        this.modified = true;
                }
            }
            this.RefreshStoreGroupProperties();
            if (this.lsvNonMembers.Items.Count == 0)
                this.btnNonMembersRemove.Enabled = false;
            this.HourGlass(false);
        }

        private void lsvMembers_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (this.lsvMembers.CheckedItems.Count == 1 && e.CurrentValue == CheckState.Checked && e.NewValue == CheckState.Unchecked)
                this.btnMembersRemove.Enabled = false;
            else
                this.btnMembersRemove.Enabled = true && this.storeGroup.Store.IAmManager;
        }

        private void lsvNonMembers_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (this.lsvNonMembers.CheckedItems.Count == 1 && e.CurrentValue == CheckState.Checked && e.NewValue == CheckState.Unchecked)
                this.btnNonMembersRemove.Enabled = false;
            else
                this.btnNonMembersRemove.Enabled = true && this.storeGroup.Store.IAmManager;

        }

        private void txtLDapQuery_TextChanged(object sender, EventArgs e)
        {
            this.btnApply.Enabled = true;
            this.modified = true;
        }

        private void lsvMembers_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            this.btnMembersRemove.Enabled = this.lsvMembers.SelectedItems.Count > 0 && this.storeGroup.Store.IAmManager;
        }

        private void lsvNonMembers_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            this.btnNonMembersRemove.Enabled = this.lsvNonMembers.SelectedItems.Count > 0 && this.storeGroup.Store.IAmManager;
        }

        private void btnTestLDapQuery_Click(object sender, EventArgs e)
        {
            try
            {
                this.HourGlass(true);
                frmActiveDirectoryObjectsList frm = new frmActiveDirectoryObjectsList();
                frm.Text = Globalization.MultilanguageResource.GetString("frmApplicationGroupsProperties_Msg20") + this.storeGroup.Name;
                frm.searchResultCollection = this.storeGroup.ExecuteLDAPQuery(this.txtLDapQuery.Text);
                frm.ShowDialog(this);
                this.HourGlass(false);
            }
            catch (Exception ex)
            {
                this.HourGlass(false);
                this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("frmApplicationGroupsProperties_Tit30"));
            }
        }

        private void frmStoreGroupsProperties_Activated(object sender, EventArgs e)
        {
            if (this.firstShow)
            {
                this.txtName.Focus();
                this.txtName.SelectAll();
                this.firstShow = false;
            }
        }

        private void lsvMembers_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            this.SortListView(this.lsvMembers);
        }

        private void lsvNonMembers_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            this.SortListView(this.lsvNonMembers);
        }

        private void btnMembersAddDBUsers_Click(object sender, EventArgs e)
        {
            this.HourGlass(true);
            frmDBUsersList frm = new frmDBUsersList();
            frm.store = this.storeGroup.Store;
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                foreach (IAzManDBUser dbUser in frm.selectedDBUsers)
                {
                    if (!this.MembersToRemove.Remove(dbUser.CustomSid.StringValue))
                    {
                        if (!this.MembersToAdd.ContainsByObjectSid(dbUser.CustomSid.StringValue) && !this.FindMember(this.storeGroup.GetStoreGroupMembers(), dbUser.CustomSid))
                        {
                            this.MembersToAdd.Add(new GenericMember(dbUser.UserName, dbUser.CustomSid, WhereDefined.Database));
                            this.modified = true;
                        }
                    }
                }
                this.RefreshStoreGroupProperties();
            }
            this.HourGlass(false);
        }

        private void btnNonMembersAddDBUsers_Click(object sender, EventArgs e)
        {
            this.HourGlass(true);
            frmDBUsersList frm = new frmDBUsersList();
            frm.store = this.storeGroup.Store;
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                foreach (IAzManDBUser dbUser in frm.selectedDBUsers)
                {
                    if (!this.NonMembersToRemove.Remove(dbUser.CustomSid.StringValue))
                    {
                        if (!this.NonMembersToAdd.ContainsByObjectSid(dbUser.CustomSid.StringValue) && !this.FindMember(this.storeGroup.GetStoreGroupNonMembers(), dbUser.CustomSid))
                        {
                            this.NonMembersToAdd.Add(new GenericMember(dbUser.UserName, dbUser.CustomSid, WhereDefined.Database));
                            this.modified = true;
                        }
                    }
                }
                this.RefreshStoreGroupProperties();
            }
            this.HourGlass(false);
        }
    }
}