using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Web.UI;
using NetSqlAzMan.Interfaces;
using NetSqlAzManWebConsole.Objects;


namespace NetSqlAzManWebConsole
{
    public partial class dlgItemProperties : dlgPage
    {
        internal IAzManApplication application;
        internal IAzManItem item;
        internal ItemType itemType = ItemType.Operation;
        private StringCollection MembersToAdd;
        private StringCollection MembersToRemove;
        private ListView lsvRoles;
        private ListView lsvTasks;
        private ListView lsvOperations;
        private bool modified;
        //[PreEmptive.Attributes.Feature("NetSqlAzMan WebConsole: Item Properties")]
        protected void Page_Init(object sender, EventArgs e)
        {
            this.showOkCancelApply();
            this.setOkHandler(new EventHandler(this.btnOk_Click));
            this.setApplyHandler(new EventHandler(this.btnApply_Click));
        }

        private void saveSessionVariables()
        {
            this.Session["modified"] = this.modified;
            this.Session["MembersToAdd"] = this.MembersToAdd;
            this.Session["MembersToRemove"] = this.MembersToRemove;
            this.Session["lsvRoles"] = this.lsvRoles;
            this.Session["lsvTasks"] = this.lsvTasks;
            this.Session["lsvOperations"] = this.lsvOperations;
        }

        private void loadSessionVariables()
        {
            if (this.Session["modified"] != null)
                this.modified = (bool)this.Session["modified"];
            else
                this.modified = false;
            this.MembersToAdd = this.Session["MembersToAdd"] as StringCollection;
            this.MembersToRemove = this.Session["MembersToRemove"] as StringCollection;
            this.lsvRoles = this.Session["lsvRoles"] as ListView;
            this.lsvTasks = this.Session["lsvTasks"] as ListView;
            this.lsvOperations = this.Session["lsvOperations"] as ListView;
            if (this.Session["selectedObject"] as IAzManApplication != null)
            {
                this.application = (IAzManApplication)this.Session["selectedObject"];
                this.item = null;
            }
            else if (this.Session["selectedObject"] as IAzManItem != null)
            {
                this.item = (IAzManItem)this.Session["selectedObject"];
                this.application = this.item.Application;
            }
            this.menuItem = Request["MenuItem"];
            switch (this.menuItem)
            {
                case "New Role":
                case "Role Properties":
                    this.itemType = ItemType.Role; break;
                case "New Task":
                case "Task Properties":
                    this.itemType = ItemType.Task; break;
                case "New Operation":
                case "Role Operation":
                    this.itemType = ItemType.Operation; break;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.loadSessionVariables();
                this.MultiView1.ActiveViewIndex = 0;
                this.modified = false;
                this.MembersToAdd = new StringCollection();
                this.MembersToRemove = new StringCollection();
                this.lsvRoles = new ListView();
                this.lsvTasks = new ListView();
                this.lsvOperations = new ListView();
                this.saveSessionVariables();
                if (this.item != null)
                {
                    if (this.item.Application.Store.Storage.Mode == NetSqlAzManMode.Administrator)
                    {
                        this.mnuTab.Items.RemoveAt(7); // Operations
                        this.mnuTab.Items.RemoveAt(6);
                    }
                    this.btnAttributes.Enabled = true;
                    this.btnBizRule.Enabled = true;
                    switch (this.item.ItemType)
                    {
                        case ItemType.Role:
                            this.Text = "Role Properties";
                            this.mnuTab.Items[1].Text = "Role Definition";
                            this.setImage("Role_32x32.gif");
                            break;
                        case ItemType.Task:
                            this.mnuTab.Items.RemoveAt(4); // Roles
                            this.mnuTab.Items.RemoveAt(3);
                            this.Text = "Task Properties";
                            this.mnuTab.Items[1].Text = "Task Definition";
                            this.setImage("Task_32x32.gif");
                            break;
                        case ItemType.Operation:
                            this.mnuTab.Items.RemoveAt(6); // Tasks
                            this.mnuTab.Items.RemoveAt(5);
                            this.mnuTab.Items.RemoveAt(4); // Roles
                            this.mnuTab.Items.RemoveAt(3);
                            this.Text = "Operation Properties";
                            this.mnuTab.Items[1].Text = "Operation Definition";
                            this.setImage("Operation_32x32.gif");
                            break;
                    }
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
                            this.Text = "New Role";
                            this.mnuTab.Items[1].Text = "Role Definition";
                            this.setImage("Role_32x32.gif");
                            break;
                        case ItemType.Task:
                            this.Text = "New Task";
                            this.mnuTab.Items[1].Text = "Task Definition";
                            this.setImage("Task_32x32.gif");
                            break;
                        case ItemType.Operation:
                            this.Text = "New Operation";
                            this.mnuTab.Items[1].Text = "Operation Definition";
                            this.setImage("Operation_32x32.gif");
                            break;
                    }
                    this.mnuTab.Items.RemoveAt(7); // Operations
                    this.mnuTab.Items.RemoveAt(6);
                    this.mnuTab.Items.RemoveAt(5); // Tasks
                    this.mnuTab.Items.RemoveAt(4);
                    this.mnuTab.Items.RemoveAt(3); // Roles
                    this.mnuTab.Items.RemoveAt(2);
                }
                this.Description = this.Text;
                this.RefreshItems();
                this.modified = false;
                this.mnuTab.Items[1].Selected = true;
                this.txtName.Focus();
            }
            else
            {
                this.loadSessionVariables();
                if (this.Session["selectedItems"] != null)
                {
                    if (this.mnuTab.SelectedValue == "Roles")
                        this.btnAddRole_Click(this, EventArgs.Empty);
                    else if (this.mnuTab.SelectedValue == "Tasks")
                        this.btnAddTask_Click(this, EventArgs.Empty);
                    else if (this.mnuTab.SelectedValue == "Operations")
                        this.btnAddOperation_Click(this, EventArgs.Empty);
                }
            }
            this.Title = this.Description;
            this.Session["item"] = this.item;
        }

        private void renderListViews()
        {
            //Roles
            DataTable dtRoles = new DataTable("Roles");
            dtRoles.Columns.Add("Name", typeof(string));
            dtRoles.Columns.Add("Description", typeof(string));
            dtRoles.Columns.Add("ItemId", typeof(int));
            foreach (ListViewItem lvi in this.lsvRoles.Items)
            {
                DataRow dr = dtRoles.NewRow();
                dr["Name"] = lvi.Text;
                dr["Description"] = lvi.SubItems[0].Text;
                dr["ItemId"] = int.Parse(lvi.SubItems[1].Text);
                dtRoles.Rows.Add(dr);
            }
            this.dgRoles.DataSource = dtRoles;
            this.dgRoles.DataBind();
            //Tasks
            DataTable dtTasks = new DataTable("Tasks");
            dtTasks.Columns.Add("Name", typeof(string));
            dtTasks.Columns.Add("Description", typeof(string));
            dtTasks.Columns.Add("ItemId", typeof(int));
            foreach (ListViewItem lvi in this.lsvTasks.Items)
            {
                DataRow dr = dtTasks.NewRow();
                dr["Name"] = lvi.Text;
                dr["Description"] = lvi.SubItems[0].Text;
                dr["ItemId"] = int.Parse(lvi.SubItems[1].Text);
                dtTasks.Rows.Add(dr);
            }
            this.dgTasks.DataSource = dtTasks;
            this.dgTasks.DataBind();
            //Operations
            DataTable dtOperations = new DataTable("Operations");
            dtOperations.Columns.Add("Name", typeof(string));
            dtOperations.Columns.Add("Description", typeof(string));
            dtOperations.Columns.Add("ItemId", typeof(int));
            foreach (ListViewItem lvi in this.lsvOperations.Items)
            {
                DataRow dr = dtOperations.NewRow();
                dr["Name"] = lvi.Text;
                dr["Description"] = lvi.SubItems[0].Text;
                dr["ItemId"] = int.Parse(lvi.SubItems[1].Text);
                dtOperations.Rows.Add(dr);
            }
            this.dgOperations.DataSource = dtOperations;
            this.dgOperations.DataBind();
            
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.item != null)
                {
                    if (this.Session["FindChildNodeText"] == null)
                    {
                        this.Session["FindNodeText"] = this.txtName.Text;
                    }
                    else
                    {
                        this.Session["FindChildNodeText"] = this.txtName.Text;
                        this.Session["FindNodeText"] = null;
                    }
                }
                else
                {
                    this.Session["FindChildNodeText"] = this.txtName.Text;
                }
                this.commitChanges();
                //Refresh Application
                this.closeWindow(true);
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
                this.Session["RefreshParentNode"] = false;
                this.Session["RefreshNode"] = false; 
            }
        }

        protected void mnuTab_MenuItemClick(object sender, System.Web.UI.WebControls.MenuEventArgs e)
        {
            switch (e.Item.Text)
            {
                case "Role Definition":
                case "Task Definition":
                case "Operation Definition": 
                    this.MultiView1.ActiveViewIndex = 0;
                    this.txtName.Focus();
                    break;
                case "Roles": this.MultiView1.ActiveViewIndex = 1; this.Session["itemType"] = ItemType.Role; break;
                case "Tasks": this.MultiView1.ActiveViewIndex = 2; this.Session["itemType"] = ItemType.Task; break;
                case "Operations": this.MultiView1.ActiveViewIndex = 3; this.Session["itemType"] = ItemType.Operation; break;
            }
        }

        protected void btnApply_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.item != null)
                    this.Session["FindNodeText"] = this.txtName.Text;
                else
                    this.Session["FindChildNodeText"] = this.txtName.Text;
                this.commitChanges();
                Response.Redirect("dlgItemProperties.aspx");
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }

        private void commitChanges()
        {
            try
            {
                if (this.item == null)
                {
                    this.item = this.application.CreateItem(this.txtName.Text.Trim(), this.txtDescription.Text.Trim(), this.itemType);
                    this.Session["selectedObject"] = this.item;
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
            }
            catch
            {
                if (this.item != null && this.item.Application.Store.Storage.TransactionInProgress)
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
            IAzManItem[] selectedItems = (IAzManItem[])this.Session["selectedItems"];
            this.Session["selectedItems"] = null;
            foreach (IAzManItem item in selectedItems)
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

        protected void btnRemoveOperation_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.dgOperations.Rows.Count; i++)
            {
                if (((System.Web.UI.WebControls.CheckBox)this.dgOperations.Rows[i].FindControl("chkSelect")).Checked)
                {
                    string itemId = this.dgOperations.Rows[i].Cells[3].Text;
                    foreach (ListViewItem lvi in this.lsvOperations.Items)
                    {
                        if (lvi.SubItems[1].Text == itemId)
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
                    }
                }
            }
            this.RefreshItems();
        }

        private ListViewItem CreateListViewItem(string member, int itemId)
        {
            ListViewItem lvi = new ListViewItem();
            IAzManItem item = this.item.Application.GetItem(member);
            lvi.Tag = new GenericMember(item.Name, item.Description);
            lvi.Text = member;
            lvi.SubItems.Add(item.Description);
            lvi.SubItems.Add(itemId.ToString());
            return lvi;
        }

        private ListViewItem CreateListViewItem(IAzManItem member)
        {
            ListViewItem lvi = new ListViewItem();
            lvi.Tag = member;
            lvi.Text = member.Name;
            lvi.SubItems.Add(member.Description);
            lvi.SubItems.Add(member.ItemId.ToString());
            return lvi;
        }

        private void RemoveListViewItem(ref ListView lsv, string member)
        {
            foreach (ListViewItem lvi in lsv.Items)
            {
                if (lvi.Text == member)
                {
                    List<ListViewItem> toRemove = new List<ListViewItem>();
                    toRemove.Add(lvi);
                    ListView.Remove(lsv, toRemove);
                    //lvi.Remove();
                    return;
                }
            }
        }

        private void RefreshItems()
        {
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
                            this.lsvRoles.Items.Add(this.CreateListViewItem(member, m.ItemId));
                            break;
                        case ItemType.Task:
                            this.lsvTasks.Items.Add(this.CreateListViewItem(member, m.ItemId));
                            break;
                        case ItemType.Operation:
                            this.lsvOperations.Items.Add(this.CreateListViewItem(member, m.ItemId));
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
            if (!this.application.IAmManager)
            {
                this.txtName.Enabled = this.txtDescription.Enabled = this.btnOk.Enabled = this.btnApply.Enabled =
                    this.btnAddRole.Enabled = this.btnAddTask.Enabled = this.btnAddOperation.Enabled =
                    this.btnRemoveRole.Enabled = this.btnRemoveTask.Enabled = this.btnRemoveOperation.Enabled = false;
            }
            this.saveSessionVariables();
            this.renderListViews();
        }

        private void btnAddRole_Click(object sender, EventArgs e)
        {
            IAzManItem[] selectedItems = (IAzManItem[])this.Session["selectedItems"];
            this.Session["selectedItems"] = null;
            foreach (IAzManItem item in selectedItems)
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

        private void btnAddTask_Click(object sender, EventArgs e)
        {
            IAzManItem[] selectedItems = (IAzManItem[])this.Session["selectedItems"];
            this.Session["selectedItems"] = null;
            foreach (IAzManItem item in selectedItems)
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

        protected void btnRemoveRole_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.dgRoles.Rows.Count; i++)
            {
                if (((System.Web.UI.WebControls.CheckBox)this.dgRoles.Rows[i].FindControl("chkSelect")).Checked)
                {
                    string itemId = this.dgRoles.Rows[i].Cells[3].Text;
                    foreach (ListViewItem lvi in this.lsvRoles.Items)
                    {
                        if (lvi.SubItems[1].Text == itemId)
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
                    }
                }
            }
            this.RefreshItems();
        }

        protected void btnRemoveTask_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.dgTasks.Rows.Count; i++)
            {
                if (((System.Web.UI.WebControls.CheckBox)this.dgTasks.Rows[i].FindControl("chkSelect")).Checked)
                {
                    string itemId = this.dgTasks.Rows[i].Cells[3].Text;
                    foreach (ListViewItem lvi in this.lsvTasks.Items)
                    {
                        if (lvi.SubItems[1].Text == itemId)
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
                    }
                }
            }
            this.RefreshItems();
        }

        protected void txtName_TextChanged(object sender, EventArgs e)
        {
            this.modified = true;
        }

        protected void txtDescription_TextChanged(object sender, EventArgs e)
        {
            this.modified = true;
        }
    }
}
