using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using NetSqlAzMan;
using NetSqlAzMan.Interfaces;
using NetSqlAzManWebConsole.Objects;

namespace NetSqlAzManWebConsole
{
    public partial class dlgApplicationGroupProperties : dlgPage
    {
        protected internal IAzManStorage storage = null;
        protected internal IAzManApplication application = null;
        protected internal IAzManApplicationGroup applicationGroup = null;
        private GenericMemberCollection MembersToAdd;
        private GenericMemberCollection MembersToRemove;
        private GenericMemberCollection NonMembersToAdd;
        private GenericMemberCollection NonMembersToRemove;
        private bool modified;
        private ListView lsvMembers;
        private ListView lsvNonMembers;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.showOkCancelApply();
            this.setOkHandler(new EventHandler(this.btnOk_Click));
            this.setApplyHandler(new EventHandler(this.btnApply_Click));
        }

        private void saveSessionVariables()
        {
            this.Session["modified"] = this.modified;
            this.Session["lsvMembers"] = this.lsvMembers;
            this.Session["lsvNonMembers"] = this.lsvNonMembers;
            this.Session["MembersToAdd"] = this.MembersToAdd;
            this.Session["MembersToRemove"] = this.MembersToRemove;
            this.Session["NonMembersToAdd"] = this.NonMembersToAdd;
            this.Session["NonMembersToRemove"] = this.NonMembersToRemove;
            this.Session["ADObjectType"] = ADObjectType.UsersAndGroups;
            this.Session["group"] = this.Session["applicationGroup"] = this.applicationGroup;
            this.Session["storeGroup"] = null;
        }

        private void loadSessionVariables()
        {
            this.lsvMembers = this.Session["lsvMembers"] as ListView;
            this.lsvNonMembers = this.Session["lsvNonMembers"] as ListView;
            this.MembersToAdd = this.Session["MembersToAdd"] as GenericMemberCollection;
            this.MembersToRemove = this.Session["MembersToRemove"] as GenericMemberCollection;
            this.NonMembersToAdd = this.Session["NonMembersToAdd"] as GenericMemberCollection;
            this.NonMembersToRemove = this.Session["NonMembersToRemove"] as GenericMemberCollection;
            this.modified = (bool)this.Session["modified"];
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.storage = this.Session["storage"] as IAzManStorage;
            if (this.Session["selectedObject"] as IAzManApplication != null)
            {
                this.application = this.Session["selectedObject"] as IAzManApplication;
                this.Session["application"] = this.application;
            }
            if (this.Session["selectedObject"] as IAzManApplicationGroup != null)
            {
                this.applicationGroup = this.Session["selectedObject"] as IAzManApplicationGroup;
                this.Session["applicationGroup"] = this.applicationGroup;
                this.Session["store"] = this.applicationGroup.Application.Store;
                this.Session["application"] = this.applicationGroup.Application;
            }
            this.Text = "Application Group properties" + (this.applicationGroup != null ? ": " + this.applicationGroup.Name : String.Empty);
            this.Description = "Application Group properties";
            this.Title = this.Text;
            if (!Page.IsPostBack)
            {
                this.MembersToAdd = new GenericMemberCollection();
                this.MembersToRemove = new GenericMemberCollection();
                this.NonMembersToAdd = new GenericMemberCollection();
                this.NonMembersToRemove = new GenericMemberCollection();
                this.lsvMembers = new ListView();
                this.lsvNonMembers = new ListView();
                this.mnuTab.Items[1].Selected = true; //0 is blank
                this.mnuTab_MenuItemClick(this, new System.Web.UI.WebControls.MenuEventArgs(this.mnuTab.Items[1]));
                this.txtName.Text = this.applicationGroup.Name;
                this.txtDescription.Text = this.applicationGroup.Description;
                this.txtGroupType.Text = (this.applicationGroup.GroupType == GroupType.Basic ? "Basic group" : "LDAP Query group");

                if (this.applicationGroup.GroupType == GroupType.Basic)
                {
                    this.btnMembersAddApplicationGroup.Enabled = this.btnNonMembersAddApplicationGroup.Enabled = this.applicationGroup.Application.HasApplicationGroups();
                    this.mnuTab.Items.RemoveAt(3);
                    this.mnuTab.Items.RemoveAt(2);
                    this.lsvMembers.Items.Clear();
                    this.lsvNonMembers.Items.Clear();
                    this.setImage("StoreApplicationGroup_32x32.gif");
                }
                else
                {
                    this.mnuTab.Items.RemoveAt(7);
                    this.mnuTab.Items.RemoveAt(6);
                    this.mnuTab.Items.RemoveAt(5);
                    this.mnuTab.Items.RemoveAt(4);
                    this.setImage("WindowsQueryLDAPGroup_32x32.gif");
                }
                this.RefreshApplicationGroupProperties();
                this.saveSessionVariables();
                this.modified = false;
                this.txtName.Focus();
            }
            else
            {
                this.loadSessionVariables();
                if (this.Session["selectedStoreGroups"] != null)
                {
                    if (this.mnuTab.SelectedValue == "Members")
                        this.btnMembersAddStoreGroups_Click(this, EventArgs.Empty);
                    else if (this.mnuTab.SelectedValue == "Non Members")
                        this.btnNonMembersAddStoreGroup_Click(this, EventArgs.Empty);
                }
                if (this.Session["selectedApplicationGroups"] != null)
                {
                    if (this.mnuTab.SelectedValue == "Members")
                        this.btnMembersAddApplicationGroups_Click(this, EventArgs.Empty);
                    else if (this.mnuTab.SelectedValue == "Non Members")
                        this.btnNonMembersAddApplicationGroup_Click(this, EventArgs.Empty);
                }
                if (this.Session["selectedDBUsers"] != null)
                {
                    if (this.mnuTab.SelectedValue == "Members")
                        this.btnMembersAddDBUsers_Click(this, EventArgs.Empty);
                    else if (this.mnuTab.SelectedValue == "Non Members")
                        this.btnNonMembersAddDBUsers_Click(this, EventArgs.Empty);
                }
                if (this.Session["selectedADObjects"] != null)
                {
                    if (this.mnuTab.SelectedValue == "Members")
                        this.btnMembersAddWindowsUsersAndGroups_Click(this, EventArgs.Empty);
                    else if (this.mnuTab.SelectedValue == "Non Members")
                        this.btnNonMembersAddWindowsUsersAndGroup_Click(this, EventArgs.Empty);
                }
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                this.CommitChanges();
                this.Session["FindNodeText"] = this.applicationGroup.Name;
                this.closeWindow(true);
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }

        protected void mnuTab_MenuItemClick(object sender, System.Web.UI.WebControls.MenuEventArgs e)
        {
            switch (e.Item.Value)
            {
                case "General": this.MultiView1.ActiveViewIndex = 0; break;
                case "LDAP Query": this.MultiView1.ActiveViewIndex = 1; break;
                case "Members": this.MultiView1.ActiveViewIndex = 2; break;
                case "Non Members": this.MultiView1.ActiveViewIndex = 3; break;
            }
        }

        private void renderListViews()
        {
            //Members
            DataTable dtMembers = new DataTable("Members");
            dtMembers.Columns.Add("Name", typeof(string));
            dtMembers.Columns.Add("WhereDefined", typeof(string));
            dtMembers.Columns.Add("SID", typeof(string));
            foreach (ListViewItem lvi in this.lsvMembers.Items)
            {
                DataRow dr = dtMembers.NewRow();
                dr["Name"] = lvi.Text;
                dr["WhereDefined"] = lvi.SubItems[0].Text;
                dr["SID"] = lvi.SubItems[1].Text;
                dtMembers.Rows.Add(dr);
            }
            this.dgMembers.DataSource = dtMembers;
            this.dgMembers.DataBind();
            //Non-Members
            DataTable dtNonMembers = new DataTable("NonMembers");
            dtNonMembers.Columns.Add("Name", typeof(string));
            dtNonMembers.Columns.Add("WhereDefined", typeof(string));
            dtNonMembers.Columns.Add("SID", typeof(string));
            foreach (ListViewItem lvi in this.lsvNonMembers.Items)
            {
                DataRow dr = dtNonMembers.NewRow();
                dr["Name"] = lvi.Text;
                dr["WhereDefined"] = lvi.SubItems[0].Text;
                dr["SID"] = lvi.SubItems[1].Text;
                dtNonMembers.Rows.Add(dr);
            }
            this.dgNonMembers.DataSource = dtNonMembers;
            this.dgNonMembers.DataBind();
        }

        private ListViewItem CreateStoreListViewItem(IAzManStoreGroup member)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.Tag = member;
            lvi.Text = member.Name;
            lvi.SubItems.Add("Store");
            lvi.SubItems.Add(member.SID.StringValue);
            return lvi;
        }

        private ListViewItem CreateApplicationListViewItem(IAzManApplicationGroup member)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.Tag = member;
            lvi.Text = member.Name;
            lvi.SubItems.Add("Application");
            lvi.SubItems.Add(member.SID.StringValue);
            return lvi;
        }

        private ListViewItem CreateLDapListViewItem(IAzManApplicationGroupMember member)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.Tag = member;
            string displayName=String.Empty;
            member.GetMemberInfo(out displayName);
            lvi.Text = displayName;
            lvi.SubItems.Add("Active Directory");
            lvi.SubItems.Add(member.SID.StringValue);
            return lvi;
        }

        private ListViewItem CreateDBListViewItem(IAzManApplicationGroupMember member)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.Tag = member;
            string displayName;
            member.GetMemberInfo(out displayName);
            lvi.Text = displayName;
            lvi.SubItems.Add("DB User");
            lvi.SubItems.Add(member.SID.StringValue);
            return lvi;
        }

        private ListViewItem CreateListViewItem(GenericMember member)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.Tag = member;
            lvi.Text = member.Name;
            switch (member.WhereDefined.ToString())
            {
                case "LDAP": lvi.SubItems.Add("Active Directory"); break;
                case "Local": lvi.SubItems.Add("Local"); break;
                case "Database": lvi.SubItems.Add("DB User"); break;
                case "Application": lvi.SubItems.Add("Application"); break;
                case "Store": lvi.SubItems.Add("Store"); break;
            }
            lvi.SubItems.Add(member.sid.StringValue);
            return lvi;
        }

        private void RemoveListViewItem(ref ListView lsv, GenericMember member)
        {
            List<ListViewItem> toRemove = new List<ListViewItem>();
            foreach (ListViewItem lvi in lsv.Items)
            {
                string objectSid = null;
                if ((lvi.Tag as GenericMember) != null)
                    objectSid = ((GenericMember)lvi.Tag).sid.StringValue;
                else if ((lvi.Tag as IAzManStoreGroup) != null)
                    objectSid = ((IAzManStoreGroup)lvi.Tag).SID.StringValue;
                else if ((lvi.Tag as IAzManStoreGroupMember) != null)
                    objectSid = ((IAzManStoreGroupMember)lvi.Tag).SID.StringValue;
                else if ((lvi.Tag as IAzManApplicationGroup) != null)
                    objectSid = ((IAzManApplicationGroup)lvi.Tag).SID.StringValue;
                else if ((lvi.Tag as IAzManApplicationGroupMember) != null)
                    objectSid = ((IAzManApplicationGroupMember)lvi.Tag).SID.StringValue;
                if (objectSid != null)
                {
                    if (member.sid.StringValue == objectSid)
                    {
                        //lvi.Remove();
                        toRemove.Add(lvi);
                        return;
                    }
                }
            }
            ListView.Remove(lsv, toRemove);
        }

        private void RefreshApplicationGroupProperties()
        {
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
                    {
                        this.lsvMembers.Items.Add(this.CreateLDapListViewItem(applicationGroupMember));
                    }
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
                    //DB Users
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
                if (!this.applicationGroup.Application.IAmManager)
                {
                    this.txtName.Enabled = this.txtDescription.Enabled = this.btnOk.Enabled =
                        this.btnMembersAddStoreGroup.Enabled = this.btnMembersAddApplicationGroup.Enabled = this.btnMembersAddDBUsers.Enabled = this.btnMembersAddWindowsUsersAndGroups.Enabled =
                        this.lsvMembers.Enabled = this.lsvNonMembers.Enabled =
                        this.btnNonMembersAddStoreGroup.Enabled = this.btnNonMembersAddApplicationGroup.Enabled = this.btnNonMembersAddDBUsers.Enabled = this.btnNonMembersAddWindowsUsersAndGroup.Enabled = false;
                    this.dgMembers.Columns[0].Visible = false;
                    this.dgNonMembers.Columns[0].Visible = false;
                }
            }
            else //Ldap Query
            {
                this.txtGroupType.Text = "LDAP Query";
                this.txtLDapQuery.Text = this.applicationGroup.LDAPQuery;
                if (!this.applicationGroup.Application.IAmManager)
                    this.txtName.Enabled = this.txtDescription.Enabled = this.txtLDapQuery.Enabled = this.btnOk.Enabled = false;
            }
            this.saveSessionVariables();
            this.renderListViews();
        }

        protected void txtName_TextChanged(object sender, EventArgs e)
        {
            this.modified = true;
            this.saveSessionVariables();
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

        private bool FindMember(IAzManApplicationGroupMember[] members, IAzManSid sid)
        {
            foreach (IAzManApplicationGroupMember m in members)
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

        protected void btnMembersRemove_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.dgMembers.Rows.Count; i++)
            {
                if (((System.Web.UI.WebControls.CheckBox)this.dgMembers.Rows[i].FindControl("chkSelect")).Checked)
                {
                    string sid = this.dgMembers.Rows[i].Cells[3].Text;
                    foreach (ListViewItem lvi in this.lsvMembers.Items)
                    {
                        if (lvi.SubItems[1].Text == sid)
                        {
                            lvi.Selected = true;
                            break;
                        }
                    }
                }
            }
            foreach (ListViewItem lvi in this.lsvMembers.Items)
            {
                if (lvi.Selected)
                {
                    if ((lvi.Tag as IAzManStoreGroup) != null)
                    {
                        IAzManStoreGroup lviTag = (IAzManStoreGroup)(lvi.Tag);
                        this.MembersToRemove.Add(new GenericMember(lviTag.Name, lviTag.SID, WhereDefined.Store));
                        this.modified = true;
                    }
                    if ((lvi.Tag as IAzManApplicationGroup) != null)
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
                    lvi.Selected = false;
                }
            }
            this.RefreshApplicationGroupProperties();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                this.CommitChanges();
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
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
                this.applicationGroup.Update(this.txtDescription.Text.Trim(), this.applicationGroup.GroupType);
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

        protected void txtDescription_TextChanged(object sender, EventArgs e)
        {
            this.modified = true;
            this.saveSessionVariables();
        }

        protected void txtLDapQuery_TextChanged(object sender, EventArgs e)
        {
            this.modified = true;
            this.saveSessionVariables();
        }

        protected void btnMembersAddStoreGroups_Click(object sender, EventArgs e)
        {
            IAzManStoreGroup[] selectedStoreGroups = this.Session["selectedStoreGroups"] as IAzManStoreGroup[];
            this.Session["selectedStoreGroups"] = null;
            foreach (IAzManStoreGroup sg in selectedStoreGroups)
            {
                if (!this.MembersToRemove.Remove(sg.SID.StringValue))
                {
                    if (!this.MembersToAdd.ContainsByObjectSid(sg.SID.StringValue) && !this.FindMember(this.applicationGroup.GetApplicationGroupMembers(), sg.SID))
                    {
                        this.MembersToAdd.Add(new GenericMember(sg.Name, sg.SID, WhereDefined.Store));
                        this.modified = true;
                    }
                }
            }
            this.RefreshApplicationGroupProperties();
        }


        protected void btnMembersAddApplicationGroups_Click(object sender, EventArgs e)
        {
            IAzManApplicationGroup[] selectedApplicationGroups = this.Session["selectedApplicationGroups"] as IAzManApplicationGroup[];
            this.Session["selectedApplicationGroups"] = null;
            foreach (IAzManApplicationGroup sg in selectedApplicationGroups)
            {
                if (!this.MembersToRemove.Remove(sg.SID.StringValue))
                {
                    if (!this.MembersToAdd.ContainsByObjectSid(sg.SID.StringValue) && !this.FindMember(this.applicationGroup.GetApplicationGroupMembers(), sg.SID))
                    {
                        this.MembersToAdd.Add(new GenericMember(sg.Name, sg.SID, WhereDefined.Application));
                        this.modified = true;
                    }
                }
            }
            this.RefreshApplicationGroupProperties();
        }

        protected void btnMembersAddWindowsUsersAndGroups_Click(object sender, EventArgs e)
        {
            ADObject[] res = ((List<ADObject>)this.Session["selectedADObjects"]).ToArray();
            this.Session["selectedADObjects"] = null;
            if (res != null)
            {
                foreach (ADObject o in res)
                {
                    if (!this.MembersToRemove.Remove(o.Sid) && !this.FindMember(this.applicationGroup.GetApplicationGroupMembers(), new SqlAzManSID(o.Sid)) && !this.FindMember(this.MembersToAdd, new SqlAzManSID(o.Sid)))
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
        }

        protected void btnMembersAddDBUsers_Click(object sender, EventArgs e)
        {
            IAzManDBUser[] selectedDBUsers = this.Session["selectedDBUsers"] as IAzManDBUser[];
            this.Session["selectedDBUsers"] = null;
            foreach (IAzManDBUser dbUser in selectedDBUsers)
            {
                if (!this.MembersToRemove.Remove(dbUser.CustomSid.StringValue))
                {
                    if (!this.MembersToAdd.ContainsByObjectSid(dbUser.CustomSid.StringValue) && !this.FindMember(this.applicationGroup.GetApplicationGroupMembers(), dbUser.CustomSid))
                    {
                        this.MembersToAdd.Add(new GenericMember(dbUser.UserName, dbUser.CustomSid, WhereDefined.Database));
                        this.modified = true;
                    }
                }
            }
            this.RefreshApplicationGroupProperties();
        }

        protected void btnNonMembersAddDBUsers_Click(object sender, EventArgs e)
        {
            IAzManDBUser[] selectedDBUsers = this.Session["selectedDBUsers"] as IAzManDBUser[];
            this.Session["selectedDBUsers"] = null;
            foreach (IAzManDBUser dbUser in selectedDBUsers)
            {
                if (!this.NonMembersToRemove.Remove(dbUser.CustomSid.StringValue))
                {
                    if (!this.NonMembersToAdd.ContainsByObjectSid(dbUser.CustomSid.StringValue) && !this.FindMember(this.applicationGroup.GetApplicationGroupNonMembers(), dbUser.CustomSid))
                    {
                        this.NonMembersToAdd.Add(new GenericMember(dbUser.UserName, dbUser.CustomSid, WhereDefined.Database));
                        this.modified = true;
                    }
                }
            }
            this.RefreshApplicationGroupProperties();
        }

        protected void btnNonMembersAddStoreGroup_Click(object sender, EventArgs e)
        {
            IAzManStoreGroup[] selectedStoreGroups = this.Session["selectedStoreGroups"] as IAzManStoreGroup[];
            this.Session["selectedStoreGroups"] = null;
            foreach (IAzManStoreGroup sg in selectedStoreGroups)
            {
                if (!this.NonMembersToRemove.Remove(sg.SID.StringValue))
                {
                    if (!this.NonMembersToAdd.ContainsByObjectSid(sg.SID.StringValue) && !this.FindMember(this.applicationGroup.GetApplicationGroupNonMembers(), sg.SID))
                    {
                        this.NonMembersToAdd.Add(new GenericMember(sg.Name, sg.SID, WhereDefined.Store));
                        this.modified = true;
                    }
                }
            }
            this.RefreshApplicationGroupProperties();
        }

        protected void btnNonMembersAddApplicationGroup_Click(object sender, EventArgs e)
        {
            IAzManApplicationGroup[] selectedApplicationGroups = this.Session["selectedApplicationGroups"] as IAzManApplicationGroup[];
            this.Session["selectedApplicationGroups"] = null;
            foreach (IAzManApplicationGroup sg in selectedApplicationGroups)
            {
                if (!this.NonMembersToRemove.Remove(sg.SID.StringValue))
                {
                    if (!this.NonMembersToAdd.ContainsByObjectSid(sg.SID.StringValue) && !this.FindMember(this.applicationGroup.GetApplicationGroupNonMembers(), sg.SID))
                    {
                        this.NonMembersToAdd.Add(new GenericMember(sg.Name, sg.SID, WhereDefined.Application));
                        this.modified = true;
                    }
                }
            }
            this.RefreshApplicationGroupProperties();
        }

        protected void btnNonMembersAddWindowsUsersAndGroup_Click(object sender, EventArgs e)
        {
            ADObject[] res = ((List<ADObject>)this.Session["selectedADObjects"]).ToArray();
            this.Session["selectedADObjects"] = null;
            if (res != null)
            {
                foreach (ADObject o in res)
                {
                    if (!this.NonMembersToRemove.Remove(o.Sid) && !this.FindMember(this.applicationGroup.GetApplicationGroupNonMembers(), new SqlAzManSID(o.Sid)) && !this.FindMember(this.NonMembersToAdd, new SqlAzManSID(o.Sid)))
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
        }

        protected void btnNonMembersRemove_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.dgNonMembers.Rows.Count; i++)
            {
                if (((System.Web.UI.WebControls.CheckBox)this.dgNonMembers.Rows[i].FindControl("chkSelect")).Checked)
                {
                    string sid = this.dgNonMembers.Rows[i].Cells[3].Text;
                    foreach (ListViewItem lvi in this.lsvNonMembers.Items)
                    {
                        if (lvi.SubItems[1].Text == sid)
                        {
                            lvi.Selected = true;
                            break;
                        }
                    }
                }
            }
            foreach (ListViewItem lvi in this.lsvNonMembers.Items)
            {
                if (lvi.Selected)
                {
                    if ((lvi.Tag as IAzManStoreGroup) != null)
                    {
                        IAzManStoreGroup lviTag = (IAzManStoreGroup)lvi.Tag;
                        this.NonMembersToRemove.Add(new GenericMember(lviTag.Name, lviTag.SID, WhereDefined.Store));
                        this.modified = true;
                    }
                    if ((lvi.Tag as IAzManApplicationGroup) != null)
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
                    lvi.Selected = false;
                }
            }
            this.RefreshApplicationGroupProperties();
        }
    }
}
