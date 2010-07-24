using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using NetSqlAzMan;
using NetSqlAzMan.Interfaces;
using NetSqlAzManWebConsole.Objects;

namespace NetSqlAzManWebConsole
{
    public partial class dlgStoreGroupProperties : dlgPage
    {
        protected internal IAzManStorage storage = null;
        protected internal IAzManStore store = null;
        protected internal IAzManStoreGroup storeGroup = null;
        private GenericMemberCollection MembersToAdd;
        private GenericMemberCollection MembersToRemove;
        private GenericMemberCollection NonMembersToAdd;
        private GenericMemberCollection NonMembersToRemove;
        private bool modified;
        private ListView lsvMembers;
        private ListView lsvNonMembers;
        [PreEmptive.Attributes.Feature("NetSqlAzMan WebConsole: Store Group Properties")]
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
            this.Session["group"] = this.Session["storeGroup"] = this.storeGroup;
            this.Session["applicationGroup"] = null;
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
            if (this.Session["selectedObject"] as IAzManStore != null)
            {
                this.store = this.Session["selectedObject"] as IAzManStore;
            }
            if (this.Session["selectedObject"] as IAzManStoreGroup != null)
            {
                this.storeGroup = this.Session["selectedObject"] as IAzManStoreGroup;
                this.Session["storeGroup"] = this.storeGroup;
                this.Session["store"] = this.storeGroup.Store;
            }
            this.Text = "Store Group properties" + (this.storeGroup != null ? ": " + this.storeGroup.Name : String.Empty);
            this.Description = "Store Group properties";
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
                this.txtName.Text = this.storeGroup.Name;
                this.txtDescription.Text = this.storeGroup.Description;
                this.txtGroupType.Text = (this.storeGroup.GroupType == GroupType.Basic ? "Basic group" : "LDAP Query group");

                if (this.storeGroup.GroupType == GroupType.Basic)
                {
                    this.btnMembersAddStoreGroup.Enabled = this.btnNonMembersAddStoreGroup.Enabled = this.storeGroup.Store.HasStoreGroups();
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
                this.RefreshStoreGroupProperties();
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
                this.Session["FindNodeText"] = this.storeGroup.Name;
                this.closeWindow(true);
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }

        protected void mnuTab_MenuItemClick(object sender, System.Web.UI.WebControls.MenuEventArgs e)
        {
            this.mnuTab.MenuItemClick += new System.Web.UI.WebControls.MenuEventHandler(mnuTab_MenuItemClick);
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

        private ListViewItem CreateLDapListViewItem(IAzManStoreGroupMember member)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.Tag = member;
            string displayName = String.Empty;
            member.GetMemberInfo(out displayName);
            lvi.Text = displayName;
            lvi.SubItems.Add("Active Directory");
            lvi.SubItems.Add(member.SID.StringValue);
            return lvi;
        }

        private ListViewItem CreateDBListViewItem(IAzManStoreGroupMember member)
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
                case "Store": lvi.SubItems.Add("Store"); break;
                case "Application": lvi.SubItems.Add("Application"); break;
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
                if (objectSid != null)
                {
                    if (member.sid.StringValue == objectSid)
                    {
                        toRemove.Add(lvi);
                        //lvi.Remove();
                        return;
                    }
                }
            }

            ListView.Remove(lsv, toRemove);
        }

        private void RefreshStoreGroupProperties()
        {
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
                if (!this.storeGroup.Store.IAmManager)
                {
                    this.txtName.Enabled = this.txtDescription.Enabled = this.btnOk.Enabled =
                        this.btnMembersAddStoreGroup.Enabled = this.btnMembersAddDBUsers.Enabled = this.btnMembersAddWindowsUsersAndGroups.Enabled =
                        this.lsvMembers.Enabled = this.lsvNonMembers.Enabled =
                        this.btnNonMembersAddStoreGroup.Enabled = this.btnNonMembersAddDBUsers.Enabled = this.btnNonMembersAddWindowsUsersAndGroup.Enabled = false;
                    this.dgMembers.Columns[0].Visible = false;
                    this.dgNonMembers.Columns[0].Visible = false;
                }
            }
            else //Ldap Query
            {
                this.txtGroupType.Text = "LDAP Query";
                this.txtLDapQuery.Text = this.storeGroup.LDAPQuery;
                if (!this.storeGroup.Store.IAmManager)
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
                    lvi.Selected = false;
                }
            }
            this.RefreshStoreGroupProperties();
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
                //Store Group Properties
                this.storeGroup.Store.Storage.BeginTransaction(AzManIsolationLevel.ReadUncommitted);
                this.storeGroup.Rename(this.txtName.Text.Trim());
                this.storeGroup.Update(this.txtDescription.Text.Trim(), this.storeGroup.GroupType);
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
                    if (!this.MembersToAdd.ContainsByObjectSid(sg.SID.StringValue) && !this.FindMember(this.storeGroup.GetStoreGroupMembers(), sg.SID))
                    {
                        this.MembersToAdd.Add(new GenericMember(sg.Name, sg.SID, WhereDefined.Store));
                        this.modified = true;
                    }
                }
            }
            this.RefreshStoreGroupProperties();
        }

        protected void btnMembersAddWindowsUsersAndGroups_Click(object sender, EventArgs e)
        {
            ADObject[] res = ((List<ADObject>)this.Session["selectedADObjects"]).ToArray();
            this.Session["selectedADObjects"] = null;
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
        }

        protected void btnMembersAddDBUsers_Click(object sender, EventArgs e)
        {
            IAzManDBUser[] selectedDBUsers = this.Session["selectedDBUsers"] as IAzManDBUser[];
            this.Session["selectedDBUsers"] = null;
            foreach (IAzManDBUser dbUser in selectedDBUsers)
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

        protected void btnNonMembersAddDBUsers_Click(object sender, EventArgs e)
        {
            IAzManDBUser[] selectedDBUsers = this.Session["selectedDBUsers"] as IAzManDBUser[];
            this.Session["selectedDBUsers"] = null;
            foreach (IAzManDBUser dbUser in selectedDBUsers)
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

        protected void btnNonMembersAddStoreGroup_Click(object sender, EventArgs e)
        {
            IAzManStoreGroup[] selectedStoreGroups = this.Session["selectedStoreGroups"] as IAzManStoreGroup[];
            this.Session["selectedStoreGroups"] = null;
            foreach (IAzManStoreGroup sg in selectedStoreGroups)
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

        protected void btnNonMembersAddWindowsUsersAndGroup_Click(object sender, EventArgs e)
        {
            ADObject[] res = ((List<ADObject>)this.Session["selectedADObjects"]).ToArray();
            this.Session["selectedADObjects"] = null;
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
                    lvi.Selected = false;
                }
            }
            this.RefreshStoreGroupProperties();
        }
    }
}
