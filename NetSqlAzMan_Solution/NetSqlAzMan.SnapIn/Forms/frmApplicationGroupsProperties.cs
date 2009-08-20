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
    public partial class frmApplicationGroupsProperties : frmBase
    {
        internal IAzManApplicationGroup applicationGroup;
        private GenericMemberCollection MembersToAdd;
        private GenericMemberCollection MembersToRemove;
        private GenericMemberCollection NonMembersToAdd;
        private GenericMemberCollection NonMembersToRemove;
        private bool modified;
        private bool firstShow;


        public frmApplicationGroupsProperties()
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
            this.txtName.Text = this.applicationGroup.Name;
            this.txtDescription.Text = this.applicationGroup.Description;
            this.txtGroupType.Text = (this.applicationGroup.GroupType == GroupType.Basic ? Globalization.MultilanguageResource.GetString("frmApplicationGroupsList_Msg10") : Globalization.MultilanguageResource.GetString("frmApplicationGroupsList_Msg20"));
            if (this.applicationGroup.GroupType==GroupType.Basic)
            {
                this.btnMembersAddStoreGroups.Enabled = this.btnNonMembersAddStoreGroups.Enabled = this.applicationGroup.Application.Store.HasStoreGroups();
                this.btnMembersAddApplicationGroups.Enabled = this.btnNonMembersAddApplicationGroup.Enabled = this.applicationGroup.Application.HasApplicationGroups();
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
            this.RefreshApplicationGroupProperties();
            this.modified = false;
            this.btnApply.Enabled = false;
            //PorkAround: http://lab.msdn.microsoft.com/ProductFeedback/viewFeedback.aspx?feedbackId=FDBK49664
            ImageList clonedImageList = new ImageList();
            foreach (Image image in this.imageList1.Images)
            {
                clonedImageList.Images.Add((Image)image.Clone());
            }
            this.lsvMembers.SmallImageList = clonedImageList;
            this.lsvNonMembers.SmallImageList = clonedImageList;
            //PorkAround End
            /*Application.DoEvents();*/
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

        private ListViewItem CreateApplicationListViewItem(IAzManApplicationGroup member)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.Tag = member;
            lvi.Text = member.Name;
            lvi.SubItems.Add(Globalization.MultilanguageResource.GetString("WhereDefined_Application"));
            return lvi;
        }


        private ListViewItem CreateLDapListViewItem(IAzManApplicationGroupMember member)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.Tag = member;
            string displayName;
            member.GetMemberInfo(out displayName);
            lvi.Text = displayName;
            lvi.SubItems.Add(Globalization.MultilanguageResource.GetString("WhereDefined_LDAP"));
            return lvi;
        }

        private ListViewItem CreateDBListViewItem(IAzManApplicationGroupMember member)
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
                else if ((lvi.Tag as IAzManStoreGroup) != null)
                    objectSid = ((IAzManStoreGroup)lvi.Tag).SID.StringValue;
                else if ((lvi.Tag as IAzManApplicationGroup)!=null)
                    objectSid = ((IAzManApplicationGroup)lvi.Tag).SID.StringValue;
                else if ((lvi.Tag as IAzManApplicationGroupMember)!=null)
                    objectSid = ((IAzManApplicationGroupMember)lvi.Tag).SID.StringValue;
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

        private void RefreshApplicationGroupProperties()
        {
            this.HourGlass(true);
            if (this.applicationGroup.GroupType == GroupType.Basic)
            {
                //Members
                //Add committed sids 
                this.lsvMembers.Items.Clear();
                IAzManApplicationGroupMember[] applicationGroupMembers = this.applicationGroup.GetApplicationGroupMembers();
                foreach (IAzManApplicationGroupMember applicationGroupMember in applicationGroupMembers)
                {
                    //Store Groups
                    if (applicationGroupMember.WhereDefined == WhereDefined.Store)
                        this.lsvMembers.Items.Add(this.CreateStoreListViewItem(applicationGroupMember.ApplicationGroup.Application.Store.GetStoreGroup(applicationGroupMember.SID)));
                    //Application Groups
                    if (applicationGroupMember.WhereDefined == WhereDefined.Application)
                        this.lsvMembers.Items.Add(this.CreateApplicationListViewItem(applicationGroupMember.ApplicationGroup.Application.GetApplicationGroup(applicationGroupMember.SID)));
                    //Ldap Object
                    if (applicationGroupMember.WhereDefined == WhereDefined.LDAP || applicationGroupMember.WhereDefined == WhereDefined.Local)
                        this.lsvMembers.Items.Add(this.CreateLDapListViewItem(applicationGroupMember));
                    //DB Users
                    if (applicationGroupMember.WhereDefined == WhereDefined.Database)
                        this.lsvMembers.Items.Add(this.CreateDBListViewItem(applicationGroupMember));
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
                IAzManApplicationGroupMember[] applicationGroupNonMembers = this.applicationGroup.GetApplicationGroupNonMembers();
                foreach (IAzManApplicationGroupMember applicationGroupNonMember in applicationGroupNonMembers)
                {
                    //Store Groups
                    if (applicationGroupNonMember.WhereDefined == WhereDefined.Store)
                        this.lsvNonMembers.Items.Add(this.CreateStoreListViewItem(applicationGroupNonMember.ApplicationGroup.Application.Store.GetStoreGroup(applicationGroupNonMember.SID)));
                    //Application Groups
                    if (applicationGroupNonMember.WhereDefined == WhereDefined.Application)
                        this.lsvNonMembers.Items.Add(this.CreateApplicationListViewItem(applicationGroupNonMember.ApplicationGroup.Application.GetApplicationGroup(applicationGroupNonMember.SID)));
                    //Ldap Object
                    if (applicationGroupNonMember.WhereDefined == WhereDefined.LDAP || applicationGroupNonMember.WhereDefined == WhereDefined.Local)
                        this.lsvNonMembers.Items.Add(this.CreateLDapListViewItem(applicationGroupNonMember));
                    ///DB Users
                    if (applicationGroupNonMember.WhereDefined == WhereDefined.Database)
                        this.lsvNonMembers.Items.Add(this.CreateDBListViewItem(applicationGroupNonMember));
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
                if (!this.applicationGroup.Application.IAmManager)
                    this.txtName.Enabled = this.txtDescription.Enabled = this.btnOK.Enabled = this.btnApply.Enabled =
                        this.btnMembersAddApplicationGroups.Enabled =
                        this.btnMembersAddStoreGroups.Enabled = this.btnMembersAddDBUsers.Enabled = this.btnMembersAddWindowsUsersAndGroups.Enabled =
                        this.btnMembersRemove.Enabled =
                        this.btnNonMembersAddApplicationGroup.Enabled =
                        this.btnNonMembersAddStoreGroups.Enabled = this.btnNonMembersAddDBUsers.Enabled = this.btnNonMembersAddWindowsUsersAndGroup.Enabled =
                        this.btnNonMembersRemove.Enabled = false;
            }
            else //Ldap Query
            {
                this.txtGroupType.Text = Globalization.MultilanguageResource.GetString("frmApplicationGroupsProperties_Msg10");
                this.txtLDapQuery.Text = this.applicationGroup.LDAPQuery;
                this.btnApply.Enabled = this.modified;
                if (!this.applicationGroup.Application.IAmManager)
                    this.txtName.Enabled = this.txtDescription.Enabled = this.txtLDapQuery.Enabled = this.btnOK.Enabled = this.btnApply.Enabled = 
                        this.lsvMembers.Enabled = this.lsvNonMembers.Enabled = false;
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
                this.errorProvider1.SetError(this.txtName, Globalization.MultilanguageResource.GetString("frmApplicationGroupsProperties_Tit20"));
            }
            this.modified = true;
            this.btnApply.Enabled = true;
        }

        private bool FindMember(IAzManApplicationGroupMember[] members, string objectSid)
        {
            foreach (IAzManApplicationGroupMember m in members)
            {
                if (m.SID.StringValue == objectSid)
                    return true;
            }
            return false;
        }

        private bool FindMember(GenericMemberCollection members, string objectSid)
        {
            foreach (GenericMember m in members)
            {
                if (m.sid.StringValue == objectSid)
                    return true;
            }
            return false;
        }


        private void btnMembersAddApplicationGroups_Click(object sender, EventArgs e)
        {
            frmApplicationGroupsList frm = new frmApplicationGroupsList();
            frm.application = this.applicationGroup.Application;
            frm.applicationGroup = this.applicationGroup;
            DialogResult dr = frm.ShowDialog(this);
            /*Application.DoEvents();*/
            frm.Dispose();
            /*Application.DoEvents();*/
            if (dr == DialogResult.OK)
            {
                foreach (IAzManApplicationGroup sg in frm.selectedApplicationGroups)
                {
                    if (!this.MembersToRemove.Remove(sg.SID.StringValue))
                    {
                        if (!this.MembersToAdd.ContainsByObjectSid(sg.SID.StringValue) && !this.FindMember(this.applicationGroup.GetApplicationGroupMembers(), sg.SID.StringValue))
                        {
                            this.MembersToAdd.Add(new GenericMember(sg.Name, sg.SID, WhereDefined.Application));
                            this.modified = true;
                        }
                    }
                }
                this.RefreshApplicationGroupProperties();
            }
            this.HourGlass(false);
        }

        private void btnMembersAddWindowsUsersAndGroups_Click(object sender, EventArgs e)
        {
            try
            {
                this.HourGlass(true);
                ADObject[] res = DirectoryServicesUtils.ADObjectPickerShowDialog(this, this.applicationGroup.Application.Store.Storage.Mode==NetSqlAzManMode.Developer);
                /*Application.DoEvents();*/
                if (res != null)
                {
                    foreach (ADObject o in res)
                    {
                        if (!this.MembersToRemove.Remove(o.Sid) && !this.FindMember(this.applicationGroup.GetApplicationGroupMembers(), o.Sid) && !this.FindMember(this.MembersToAdd, o.Sid))
                        {
                            WhereDefined wd = WhereDefined.LDAP;
                            if (!o.ADSPath.StartsWith("LDAP"))
                                wd = WhereDefined.Local;
                            this.MembersToAdd.Add(new GenericMember(o.Name, new SqlAzManSID(o.Sid), wd));
                            this.modified = true;
                        }
                    }
                    this.RefreshApplicationGroupProperties();
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
                else if ((lvi.Tag as IAzManApplicationGroup) != null)
                {
                    IAzManApplicationGroup lviTag = (IAzManApplicationGroup)(lvi.Tag);
                    this.MembersToRemove.Add(new GenericMember(lviTag.Name, lviTag.SID, WhereDefined.Application));
                    this.modified = true;
                }
                else if ((lvi.Tag as IAzManApplicationGroupMember) != null)
                {
                    IAzManApplicationGroupMember lviTag = (IAzManApplicationGroupMember)(lvi.Tag);
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
            this.RefreshApplicationGroupProperties();
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
                //Application Group Properties
                this.applicationGroup.Application.Store.Storage.BeginTransaction(AzManIsolationLevel.ReadUncommitted);
                this.applicationGroup.Rename(this.txtName.Text.Trim());
                this.applicationGroup.Update(this.applicationGroup.SID, this.txtDescription.Text.Trim(), this.applicationGroup.GroupType);
                if (this.applicationGroup.GroupType == GroupType.Basic)
                {
                    //Members
                    //Members To Add
                    foreach (GenericMember member in this.MembersToAdd)
                    {
                        this.applicationGroup.CreateApplicationGroupMember(member.sid, member.WhereDefined, true);
                    }
                    //Members To Remove
                    foreach (GenericMember member in this.MembersToRemove)
                    {
                        this.applicationGroup.GetApplicationGroupMember(member.sid).Delete();
                    }
                    //Non Members
                    //Non Members To Add
                    foreach (GenericMember nonMember in this.NonMembersToAdd)
                    {
                        this.applicationGroup.CreateApplicationGroupMember(nonMember.sid, nonMember.WhereDefined, false);
                    }
                    //Non Members To Remove
                    foreach (GenericMember nonMember in this.NonMembersToRemove)
                    {
                        this.applicationGroup.GetApplicationGroupMember(nonMember.sid).Delete();
                    }
                    this.MembersToAdd.Clear();
                    this.MembersToRemove.Clear();
                    this.NonMembersToAdd.Clear();
                    this.NonMembersToRemove.Clear();
                    this.modified = false;
                }
                else
                {
                    this.applicationGroup.UpdateLDapQuery(this.txtLDapQuery.Text.Trim());
                }
                this.applicationGroup.Application.Store.Storage.CommitTransaction();
            }
            catch
            {
                this.applicationGroup.Application.Store.Storage.RollBackTransaction();
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

        private void btnNonMembersAddApplicationGroup_Click(object sender, EventArgs e)
        {
            this.HourGlass(true);
            frmApplicationGroupsList frm = new frmApplicationGroupsList();
            frm.application = this.applicationGroup.Application;
            frm.applicationGroup = this.applicationGroup;
            DialogResult dr = frm.ShowDialog(this);
            this.HourGlass(true);
            if (dr == DialogResult.OK)
            {
                foreach (IAzManApplicationGroup sg in frm.selectedApplicationGroups)
                {
                    if (!this.NonMembersToRemove.Remove(sg.SID.StringValue))
                    {
                        if (!this.NonMembersToAdd.ContainsByObjectSid(sg.SID.StringValue) && !this.FindMember(this.applicationGroup.GetApplicationGroupNonMembers(), sg.SID.StringValue))
                        {
                            this.NonMembersToAdd.Add(new GenericMember(sg.Name, sg.SID, WhereDefined.Application));
                            this.modified = true;
                        }
                    }
                }
                this.RefreshApplicationGroupProperties();
            }
            this.HourGlass(false);
        }

        private void btnNonMembersAddWindowsUsersAndGroup_Click(object sender, EventArgs e)
        {
            this.HourGlass(true);
            ADObject[] res = DirectoryServicesUtils.ADObjectPickerShowDialog(this, this.applicationGroup.Application.Store.Storage.Mode == NetSqlAzManMode.Developer);
            if (res != null)
            {
                foreach (ADObject o in res)
                {
                    if (!this.NonMembersToRemove.Remove(o.Sid) && !this.FindMember(this.applicationGroup.GetApplicationGroupNonMembers(), o.Sid) && !this.FindMember(this.NonMembersToAdd, o.Sid))
                    {
                        WhereDefined wd = WhereDefined.LDAP;
                        if (!o.ADSPath.StartsWith("LDAP"))
                            wd = WhereDefined.Local;
                        this.NonMembersToAdd.Add(new GenericMember(o.Name, new SqlAzManSID(o.Sid), wd));
                        this.modified = true;
                    }
                }
                this.RefreshApplicationGroupProperties();
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
                else if ((lvi.Tag as IAzManApplicationGroup) != null)
                {
                    IAzManApplicationGroup lviTag = (IAzManApplicationGroup)lvi.Tag;
                    this.NonMembersToRemove.Add(new GenericMember(lviTag.Name, lviTag.SID, WhereDefined.Application));
                    this.modified = true;
                }
                else if ((lvi.Tag as IAzManApplicationGroupMember) != null)
                {
                    IAzManApplicationGroupMember lviTag = (IAzManApplicationGroupMember)lvi.Tag;
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
            this.RefreshApplicationGroupProperties();
            if (this.lsvNonMembers.Items.Count == 0)
                this.btnNonMembersRemove.Enabled = false;
            this.HourGlass(false);
        }

        private void lsvMembers_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (this.lsvMembers.CheckedItems.Count == 1 && e.CurrentValue == CheckState.Checked && e.NewValue == CheckState.Unchecked)
                this.btnMembersRemove.Enabled = false;
            else
                this.btnMembersRemove.Enabled = true && this.applicationGroup.Application.IAmManager;
        }

        private void lsvNonMembers_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (this.lsvNonMembers.CheckedItems.Count == 1 && e.CurrentValue == CheckState.Checked && e.NewValue == CheckState.Unchecked)
                this.btnNonMembersRemove.Enabled = false;
            else
                this.btnNonMembersRemove.Enabled = true && this.applicationGroup.Application.IAmManager;

        }

        private void btnMembersAddStoreGroups_Click(object sender, EventArgs e)
        {
            frmStoreGroupsList frm = new frmStoreGroupsList();
            frm.store = this.applicationGroup.Application.Store;
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                foreach (IAzManStoreGroup sg in frm.selectedStoreGroups)
                {
                    if (!this.MembersToRemove.Remove(sg.SID.StringValue))
                    {
                        if (!this.MembersToAdd.ContainsByObjectSid(sg.SID.StringValue) && !this.FindMember(this.applicationGroup.GetApplicationGroupMembers(), sg.SID.StringValue))
                        {
                            this.MembersToAdd.Add(new GenericMember(sg.Name, sg.SID, WhereDefined.Store));
                            this.modified = true;
                        }
                    }
                }
                this.RefreshApplicationGroupProperties();
            }
            this.HourGlass(false);
        }

        private void btnNonMembersAddStoreGroups_Click(object sender, EventArgs e)
        {
            this.HourGlass(true);
            frmStoreGroupsList frm = new frmStoreGroupsList();
            frm.store = this.applicationGroup.Application.Store;
            DialogResult dr = frm.ShowDialog(this);
            this.HourGlass(true);
            if (dr == DialogResult.OK)
            {
                foreach (IAzManStoreGroup sg in frm.selectedStoreGroups)
                {
                    if (!this.NonMembersToRemove.Remove(sg.SID.StringValue))
                    {
                        if (!this.NonMembersToAdd.ContainsByObjectSid(sg.SID.StringValue) && !this.FindMember(this.applicationGroup.GetApplicationGroupNonMembers(), sg.SID.StringValue))
                        {
                            this.NonMembersToAdd.Add(new GenericMember(sg.Name, sg.SID, WhereDefined.Store));
                            this.modified = true;
                        }
                    }
                }
                this.RefreshApplicationGroupProperties();
            }
            this.HourGlass(false);
        }

        private void txtLDapQuery_TextChanged(object sender, EventArgs e)
        {
            this.btnApply.Enabled = true;
            this.modified = true;
        }

        private void lsvMembers_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            this.btnMembersRemove.Enabled = this.lsvMembers.SelectedItems.Count > 0 && this.applicationGroup.Application.IAmManager;
        }

        private void lsvNonMembers_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            this.btnNonMembersRemove.Enabled = this.lsvNonMembers.SelectedItems.Count > 0 && this.applicationGroup.Application.IAmManager;
        }

        private void btnTestLDapQuery_Click(object sender, EventArgs e)
        {
            try
            {
                this.HourGlass(true);
                frmActiveDirectoryObjectsList frm = new frmActiveDirectoryObjectsList();
                frm.Text = Globalization.MultilanguageResource.GetString("frmApplicationGroupsProperties_Msg20") + this.applicationGroup.Name;
                frm.searchResultCollection = this.applicationGroup.ExecuteLDAPQuery(this.txtLDapQuery.Text);
                frm.ShowDialog(this);
                this.HourGlass(false);
            }
            catch (Exception ex)
            {
                this.HourGlass(false);
                this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("frmApplicationGroupsProperties_Tit30"));
            }
        }

        private void frmApplicationGroupsProperties_Activated(object sender, EventArgs e)
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
            frm.application = this.applicationGroup.Application;
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                foreach (IAzManDBUser dbUser in frm.selectedDBUsers)
                {
                    if (!this.MembersToRemove.Remove(dbUser.CustomSid.StringValue))
                    {
                        if (!this.MembersToAdd.ContainsByObjectSid(dbUser.CustomSid.StringValue) && !this.FindMember(this.applicationGroup.GetApplicationGroupMembers(), dbUser.CustomSid.StringValue))
                        {
                            this.MembersToAdd.Add(new GenericMember(dbUser.UserName, dbUser.CustomSid, WhereDefined.Database));
                            this.modified = true;
                        }
                    }
                }
                this.RefreshApplicationGroupProperties();
            }
            this.HourGlass(false);
        }

        private void btnNonMembersAddDBUsers_Click(object sender, EventArgs e)
        {
            this.HourGlass(true);
            frmDBUsersList frm = new frmDBUsersList();
            frm.application = this.applicationGroup.Application;
            DialogResult dr = frm.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                foreach (IAzManDBUser dbUser in frm.selectedDBUsers)
                {
                    if (!this.NonMembersToRemove.Remove(dbUser.CustomSid.StringValue))
                    {
                        if (!this.NonMembersToAdd.ContainsByObjectSid(dbUser.CustomSid.StringValue) && !this.FindMember(this.applicationGroup.GetApplicationGroupNonMembers(), dbUser.CustomSid.StringValue))
                        {
                            this.NonMembersToAdd.Add(new GenericMember(dbUser.UserName, dbUser.CustomSid, WhereDefined.Database));
                            this.modified = true;
                        }
                    }
                }
                this.RefreshApplicationGroupProperties();
            }
            this.HourGlass(false);
        }
    }
}