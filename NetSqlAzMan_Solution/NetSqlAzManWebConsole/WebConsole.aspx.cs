using System;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NetSqlAzMan;
using NetSqlAzMan.Interfaces;
using NetSqlAzManWebConsole.Objects;

namespace NetSqlAzManWebConsole
{
    public partial class WebConsole : ThemePage
    {
        #region Fields
        private string imageUrlPath;
        private int detailsRowIndex;
        #endregion Fields
        #region Properties
        internal IAzManStorage Storage
        {
            get
            {
                if (this.Session["storage"] == null)
                {
                    Response.Redirect("StorageConnection.aspx");
                }
                return (IAzManStorage)this.Session["storage"];
            }
        }

        internal object SelectedObject
        {
            get
            {
                return this.Session["selectedObject"];
            }
            set
            {
                this.Session["selectedObject"] = value;
            }
        }
        #endregion Properties
        #region Handlers
        protected void Page_Init(object sender, EventArgs e)
        {
            this.imageUrlPath = this.getApplicationPath() + "/images/";
            switch (this.Theme)
            {
                case "Default": this.imgLogo2.ImageUrl = "~/Images/logo.gif"; break;
                case "Green": this.imgLogo2.ImageUrl = "~/Images/logo_1.gif"; break;
                case "Yellow": this.imgLogo2.ImageUrl = "~/Images/logo_2.gif"; break;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.Page.ClientScript.RegisterOnSubmitStatement(typeof(string), "waitCursorOnSubmit", "doHourglass();");
                if (!this.Page.IsPostBack)
                {
                    this.renderTreeRoot();
                    this.renderHeaderInfo();
                    this.renderFooterInfo();
                }
                //e.g.: Store Properties from Store Node
                if (this.Session["FindNodeText"] != null)
                { 
                    this.tv.SelectedNode.Parent.Select();
                    this.tv.SelectedNode.ChildNodes.Clear();
                    this.tv_TreeNodeExpanded(this, new TreeNodeEventArgs(this.tv.SelectedNode));
                    this.findChildTextNode(this.tv.SelectedNode, (string)this.Session["FindNodeText"]);
                    this.Session["FindNodeText"] = null;
                }
                //e.g.: New Store from Storage Node
                if (this.Session["FindChildNodeText"] != null)
                {
                    this.tv.SelectedNode.ChildNodes.Clear();
                    this.tv_TreeNodeExpanded(this, new TreeNodeEventArgs(this.tv.SelectedNode));
                    this.findChildTextNode(this.tv.SelectedNode, (string)this.Session["FindChildNodeText"]);
                    this.Session["FindChildNodeText"] = null;
                }
                if ((bool)this.Session["RefreshParentNode"] == true && this.tv.SelectedNode.Parent != null)
                {
                    this.Session["RefreshParentNode"] = false;
                    this.tv.SelectedNode.Parent.Select();
                    this.tv.SelectedNode.ChildNodes.Clear();
                    this.tv_SelectedNodeChanged(this, EventArgs.Empty);
                    this.tv_TreeNodeExpanded(this, new TreeNodeEventArgs(this.tv.SelectedNode));
                }
                if ((bool)this.Session["RefreshNode"] == true && this.tv.SelectedNode != null)
                {
                    this.Session["RefreshNode"] = false;
                    this.tv.SelectedNode.Select();
                    this.tv.SelectedNode.ChildNodes.Clear();
                    this.tv_SelectedNodeChanged(this, EventArgs.Empty);
                    this.tv_TreeNodeExpanded(this, new TreeNodeEventArgs(this.tv.SelectedNode));
                }
                if ((bool)this.Session["RefreshTree"] == true)
                {
                    this.Session["RefreshTree"] = false;
                    Response.Redirect("WebConsole.aspx");
                }
                if (this.Session["DownloadContent"] != null)
                {
                    byte[] content = (byte[])this.Session["DownloadContent"];
                    this.Session["DownloadContent"] = null;
                    this.sendFileToTheBrowser("NetSqlAzMan.xml", content);
                }
                this.showDetails();
                //Check for Update
                if (this.Session["updateMessage"] != null)
                {
                    string msg = (string)this.Session["updateMessage"];
                    this.Session["updateMessage"] = null;
                    this.Session["updateWarning"] = true;
                    this.ShowInfo(msg);
                }
                if ((bool)this.Session["updateWarning"])
                {
                    this.lblUpdateWarning.Visible = true;
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }
        protected void tv_TreeNodeExpanded(object sender, TreeNodeEventArgs e)
        {
            try
            {
                #region TreeNodeExpanded
                if (e.Node == null)
                    return;
                string nodeValue = e.Node.Value;
                e.Node.ChildNodes.Clear();
                //Storage
                if (nodeValue.StartsWith("Storage|"))
                {
                    foreach (IAzManStore store in this.Storage.GetStores())
                    {
                        string fixedserverrole;
                        if (store.IAmAdmin) fixedserverrole = "Admin";
                        else if (store.IAmManager) fixedserverrole = "Manager";
                        else if (store.IAmUser) fixedserverrole = "User";
                        else fixedserverrole = "Reader";
                        string DisplayName = String.Format("{0} ({1})", store.Name, fixedserverrole);
                        TreeNode tn = this.newTreeNode(DisplayName, "Store|" + Utility.PipeEncode(store.Name) + "|" + nodeValue, e.Node, "Store_16x16.gif");
                    }
                }
                //Stores
                else if (nodeValue.StartsWith("Store|"))
                {
                    this.newTreeNode("Store Groups", "Store Groups|" + nodeValue, e.Node, "Folder_16x16.gif");
                    foreach (IAzManApplication app in this.Storage[this.getName(nodeValue, 1)].GetApplications())
                    {
                        string fixedserverrole;
                        if (app.IAmAdmin) fixedserverrole = "Admin";
                        else if (app.IAmManager) fixedserverrole = "Manager";
                        else if (app.IAmUser) fixedserverrole = "User";
                        else fixedserverrole = "Reader";
                        string DisplayName = String.Format("{0} ({1})", app.Name, fixedserverrole);
                        this.newTreeNode(DisplayName, "Application|" + Utility.PipeEncode(app.Name) + "|" + nodeValue, e.Node, "Application_16x16.gif");
                    }
                }
                //Store Groups
                else if (nodeValue.StartsWith("Store Groups|"))
                {
                    foreach (IAzManStoreGroup sg in this.Storage[this.getName(nodeValue, 2)].GetStoreGroups())
                    {
                        this.newTreeNode(sg.Name, "Store Group|" + Utility.PipeEncode(sg.Name) + "|" + nodeValue, e.Node, sg.GroupType == GroupType.Basic ? "StoreApplicationGroup_16x16.gif" : "WindowsQueryLDAPGroup_16x16.gif");
                    }
                }
                //Applications
                else if (nodeValue.StartsWith("Application|"))
                {
                    this.newTreeNode("Application Groups", "Application Groups|" + nodeValue, e.Node, "Folder_16x16.gif");
                    this.newTreeNode("Item Definitions", "Item Definitions|" + nodeValue, e.Node, "Folder_16x16.gif");
                    this.newTreeNode("Item Authorizations", "Item Authorizations|" + nodeValue, e.Node, "Folder_16x16.gif");
                }
                //Application Groups
                else if (nodeValue.StartsWith("Application Groups|"))
                {
                    foreach (IAzManApplicationGroup ag in this.Storage[this.getName(nodeValue, 4)][this.getName(nodeValue, 2)].GetApplicationGroups())
                    {
                        this.newTreeNode(ag.Name, "Application Group|" + Utility.PipeEncode(ag.Name) + "|" + nodeValue, e.Node, ag.GroupType == GroupType.Basic ? "StoreApplicationGroup_16x16.gif" : "WindowsQueryLDAPGroup_16x16.gif");
                    }
                }
                //Item Definitions
                else if (nodeValue.StartsWith("Item Definitions|"))
                {
                    this.newTreeNode("Role Definitions", "Role Definitions|" + nodeValue, e.Node, "Folder_16x16.gif");
                    this.newTreeNode("Task Definitions", "Task Definitions|" + nodeValue, e.Node, "Folder_16x16.gif");
                    if (this.Storage.Mode == NetSqlAzManMode.Developer)
                        this.newTreeNode("Operation Definitions", "Operation Definitions|" + nodeValue, e.Node, "Folder_16x16.gif");
                }
                //Item Authorizations
                else if (nodeValue.StartsWith("Item Authorizations|"))
                {
                    this.newTreeNode("Role Authorizations", "Role Authorizations|" + nodeValue, e.Node, "Folder_16x16.gif");
                    this.newTreeNode("Task Authorizations", "Task Authorizations|" + nodeValue, e.Node, "Folder_16x16.gif");
                    if (this.Storage.Mode == NetSqlAzManMode.Developer)
                        this.newTreeNode("Operation Authorizations", "Operation Authorizations|" + nodeValue, e.Node, "Folder_16x16.gif");
                }
                //Role Definitions
                else if (nodeValue.StartsWith("Role Definitions|"))
                {
                    foreach (IAzManItem item in this.Storage[this.getName(nodeValue, 5)][this.getName(nodeValue, 3)].GetItems(ItemType.Role))
                    {
                        this.newTreeNode(item.Name, "Role Definition|" + Utility.PipeEncode(item.Name) + "|" + nodeValue, e.Node, "Role_16x16.gif");
                    }
                }
                //Task Definitions
                else if (nodeValue.StartsWith("Task Definitions|"))
                {
                    foreach (IAzManItem item in this.Storage[this.getName(nodeValue, 5)][this.getName(nodeValue, 3)].GetItems(ItemType.Task))
                    {
                        this.newTreeNode(item.Name, "Task Definition|" + Utility.PipeEncode(item.Name) + "|" + nodeValue, e.Node, "Task_16x16.gif");
                    }
                }
                //Operation Definitions
                else if (nodeValue.StartsWith("Operation Definitions|"))
                {
                    foreach (IAzManItem item in this.Storage[this.getName(nodeValue, 5)][this.getName(nodeValue, 3)].GetItems(ItemType.Operation))
                    {
                        this.newTreeNode(item.Name, "Operation Definition|" + Utility.PipeEncode(item.Name) + "|" + nodeValue, e.Node, "Operation_16x16.gif");
                    }
                }
                //Role Authorizations
                else if (nodeValue.StartsWith("Role Authorizations|"))
                {
                    foreach (IAzManItem item in this.Storage[this.getName(nodeValue, 5)][this.getName(nodeValue, 3)].GetItems(ItemType.Role))
                    {
                        this.newTreeNode(item.Name, "Role Authorization|" + Utility.PipeEncode(item.Name) + "|" + nodeValue, e.Node, "Role_16x16.gif");
                    }
                }
                //Task Authorizations
                else if (nodeValue.StartsWith("Task Authorizations|"))
                {
                    foreach (IAzManItem item in this.Storage[this.getName(nodeValue, 5)][this.getName(nodeValue, 3)].GetItems(ItemType.Task))
                    {
                        this.newTreeNode(item.Name, "Task Authorization|" + Utility.PipeEncode(item.Name) + "|" + nodeValue, e.Node, "Task_16x16.gif");
                    }
                }
                //Operation Authorizations
                else if (nodeValue.StartsWith("Operation Authorizations|"))
                {
                    foreach (IAzManItem item in this.Storage[this.getName(nodeValue, 5)][this.getName(nodeValue, 3)].GetItems(ItemType.Operation))
                    {
                        this.newTreeNode(item.Name, "Operation Authorization|" + Utility.PipeEncode(item.Name) + "|" + nodeValue, e.Node, "Operation_16x16.gif");
                    }
                }
                #endregion TreeNodeExpanded
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }

        protected void lnkLogoff_Click(object sender, EventArgs e)
        {
            this.Session["storage"] = null;
            this.Session.Abandon();
            Response.Redirect("StorageConnection.aspx");
        }

        protected void lnkReload_Click(object sender, EventArgs e)
        {
            Session["storage"] = new SqlAzManStorage(((IAzManStorage)this.Session["storage"]).ConnectionString);
            Response.Redirect("WebConsole.aspx");
        }

        protected void tv_SelectedNodeChanged(object sender, EventArgs e)
        {
            try
            {
                #region SelectedNodeChanged
                TreeNode selectedNode = this.tv.SelectedNode;
                string nodeValue = selectedNode.Value;
                if (selectedNode == null)
                {
                    this.SelectedObject = null;
                    return;
                }
                //Storage
                if (nodeValue.StartsWith("Storage|"))
                {
                    this.SelectedObject = this.Storage;
                }
                //Stores
                else if (nodeValue.StartsWith("Store|"))
                {
                    this.SelectedObject = this.Storage[this.getName(nodeValue, 1)];
                }
                //Store Groups
                else if (nodeValue.StartsWith("Store Groups|"))
                {
                    this.SelectedObject = this.Storage[this.getName(nodeValue, 2)];
                }
                //Store Group
                else if (nodeValue.StartsWith("Store Group|"))
                {
                    this.SelectedObject = this.Storage[this.getName(nodeValue, 4)].GetStoreGroup(this.getName(nodeValue, 1));
                }
                //Applications
                else if (nodeValue.StartsWith("Application|"))
                {
                    this.SelectedObject = this.Storage[this.getName(nodeValue, 3)][this.getName(nodeValue, 1)];
                }
                //Application Groups
                else if (nodeValue.StartsWith("Application Groups|"))
                {
                    this.SelectedObject = this.Storage[this.getName(nodeValue, 4)][this.getName(nodeValue, 2)];
                }
                //Application Group
                else if (nodeValue.StartsWith("Application Group|"))
                {
                    this.SelectedObject = this.Storage[this.getName(nodeValue, 6)][this.getName(nodeValue, 4)].GetApplicationGroup(this.getName(nodeValue, 1));
                }
                //Item Definitions
                else if (nodeValue.StartsWith("Item Definitions|"))
                {
                    this.SelectedObject = this.Storage[this.getName(nodeValue, 4)][this.getName(nodeValue, 2)];
                }
                //Item Authorizations
                else if (nodeValue.StartsWith("Item Authorizations|"))
                {
                    this.SelectedObject = this.Storage[this.getName(nodeValue, 4)][this.getName(nodeValue, 2)];
                }
                //Role Definitions
                else if (nodeValue.StartsWith("Role Definitions|"))
                {
                    this.SelectedObject = this.Storage[this.getName(nodeValue, 5)][this.getName(nodeValue, 3)];
                }
                //Task Definitions
                else if (nodeValue.StartsWith("Task Definitions|"))
                {
                    this.SelectedObject = this.Storage[this.getName(nodeValue, 5)][this.getName(nodeValue, 3)];
                }
                //Operation Definitions
                else if (nodeValue.StartsWith("Operation Definitions|"))
                {
                    this.SelectedObject = this.Storage[this.getName(nodeValue, 5)][this.getName(nodeValue, 3)];
                }
                //Role Authorizations
                else if (nodeValue.StartsWith("Role Authorizations|"))
                {
                    this.SelectedObject = this.Storage[this.getName(nodeValue, 5)][this.getName(nodeValue, 3)];
                }
                //Task Authorizations
                else if (nodeValue.StartsWith("Task Authorizations|"))
                {
                    this.SelectedObject = this.Storage[this.getName(nodeValue, 5)][this.getName(nodeValue, 3)];
                }
                //Operation Authorizations
                else if (nodeValue.StartsWith("Operation Authorizations|"))
                {
                    this.SelectedObject = this.Storage[this.getName(nodeValue, 5)][this.getName(nodeValue, 3)];
                }
                //Role Definition
                else if (nodeValue.StartsWith("Role Definition|"))
                {
                    this.SelectedObject = this.Storage[this.getName(nodeValue, 7)][this.getName(nodeValue, 5)][this.getName(nodeValue, 1)];
                }
                //Task Definition
                else if (nodeValue.StartsWith("Task Definition|"))
                {
                    this.SelectedObject = this.Storage[this.getName(nodeValue, 7)][this.getName(nodeValue, 5)][this.getName(nodeValue, 1)];
                }
                //Operation Definition
                else if (nodeValue.StartsWith("Operation Definition|"))
                {
                    this.SelectedObject = this.Storage[this.getName(nodeValue, 7)][this.getName(nodeValue, 5)][this.getName(nodeValue, 1)];
                }
                //Role Authorization
                else if (nodeValue.StartsWith("Role Authorization|"))
                {
                    this.SelectedObject = this.Storage[this.getName(nodeValue, 7)][this.getName(nodeValue, 5)][this.getName(nodeValue, 1)];
                }
                //Task Authorization
                else if (nodeValue.StartsWith("Task Authorization|"))
                {
                    this.SelectedObject = this.Storage[this.getName(nodeValue, 7)][this.getName(nodeValue, 5)][this.getName(nodeValue, 1)];
                }
                //Operation Authorization
                else if (nodeValue.StartsWith("Operation Authorization|"))
                {
                    this.SelectedObject = this.Storage[this.getName(nodeValue, 7)][this.getName(nodeValue, 5)][this.getName(nodeValue, 1)];
                }
                if (sender != null)
                    selectedNode.Expand(); //Don't expand while collapsing
                this.showDetails();
                this.breadcrumbsLiteral.Text = this.buildBreadcrumbs(this.tv.SelectedNode);
                #endregion SelectedNodeChanged
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }

        protected void mnuAction_MenuItemClick(object sender, MenuEventArgs e)
        {
            try
            {
                if (e.Item.Text == "Storage connection")
                    Response.Redirect("StorageConnection.aspx");
                else if (e.Item.Text == "Refresh")
                    this.tv_TreeNodeExpanded(this, new TreeNodeEventArgs(this.tv.SelectedNode));
                else if (e.Item.Text == "Delete")
                    this.deleteSelectedObject();
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }
        protected override void Render(HtmlTextWriter writer)
        {
            PostBackOptions options = this.GetbtnDummyPostOptions();
            if (options != null)
            {
                options.PerformValidation = true;
                Page.ClientScript.RegisterForEventValidation(options);
                litPostBack.Text = Page.ClientScript.GetPostBackEventReference(options);
            }
            base.Render(writer);
        }
        #endregion
        #region Methods
        private void renderFooterInfo()
        {
            this.lblWebConsoleVersion.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
            this.lblNetSqlAzManVersion.Text = this.Storage.GetType().Assembly.GetName().Version.ToString();
            this.lblNetSqlAzManStorageVersion.Text = this.Storage.DatabaseVesion;
        }
        private void renderHeaderInfo()
        {
            try
            {
                SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder(this.Storage.ConnectionString);
                if (scsb.IntegratedSecurity)
                    this.lblUser.Text = this.Request.LogonUserIdentity.Name;
                else
                    this.lblUser.Text = scsb.UserID;
                this.lblDomain.Text = DirectoryServicesWebUtils.FriendlyDomainToLdapDomain(System.Environment.UserDomainName, System.Environment.UserDomainName);
                this.lblStorage.Text = String.Format("{0}\\{1}", scsb.DataSource.Trim().ToUpper(), scsb.InitialCatalog.Trim());
                this.lblMode.Text = this.Storage.Mode.ToString() + "&nbsp;&nbsp;";
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }
        private string getApplicationPath()
        {
            string result = this.Request.Url.AbsoluteUri;
            result = result.Substring(0, result.LastIndexOf('/'));
            return result;
        }
        private PostBackOptions GetbtnDummyPostOptions()
        {
            return new PostBackOptions(this.btnDummyToPostBack);
        }
        private void deleteSelectedObject()
        {
            try
            {
                if (this.SelectedObject as IAzManStore != null) ((IAzManStore)this.SelectedObject).Delete();
                else if (this.SelectedObject as IAzManStoreGroup != null) ((IAzManStoreGroup)this.SelectedObject).Delete();
                else if (this.SelectedObject as IAzManApplication != null) ((IAzManApplication)this.SelectedObject).Delete();
                else if (this.SelectedObject as IAzManApplicationGroup != null) ((IAzManApplicationGroup)this.SelectedObject).Delete();
                else if (this.SelectedObject as IAzManItem != null)
                {
                    ((IAzManItem)this.SelectedObject).Delete();
                    //Refresh entire application
                    this.tv.SelectedNode.Parent.Parent.Parent.Select();
                    this.tv.SelectedNode.ChildNodes.Clear();
                    this.tv_SelectedNodeChanged(this, EventArgs.Empty);
                    this.tv_TreeNodeExpanded(this, new TreeNodeEventArgs(this.tv.SelectedNode));
                    this.showDetails();
                    return;
                }
                else
                {
                    throw new NotImplementedException("Delete not implemented for " + this.SelectedObject.ToString() + " object.");
                }
                //Refresh Parent TreeNode
                this.tv.SelectedNode.Parent.Select();
                this.tv.SelectedNode.ChildNodes.Clear();
                this.tv_SelectedNodeChanged(this, EventArgs.Empty);
                this.tv_TreeNodeExpanded(this, new TreeNodeEventArgs(this.tv.SelectedNode));
                this.showDetails();
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }
        private void findChildTextNode(TreeNode startingNode, string textToSearch)
        {
            foreach (TreeNode childNode in startingNode.ChildNodes)
            {
                if (String.Compare(childNode.Text, textToSearch, true) == 0)
                {
                    childNode.Select();
                    this.tv_SelectedNodeChanged(this, EventArgs.Empty);
                    this.tv_TreeNodeExpanded(this, new TreeNodeEventArgs(childNode));
                    return;
                }
            }
            throw new InvalidOperationException(String.Format("Unable to find a node with text: '{0}'", textToSearch));
        }
        private void showDetails()
        {
            try
            {
                #region showDetails
                TreeNode selectedNode = this.tv.SelectedNode;
                string nodeValue = selectedNode.Value;
                this.mnuAction.Items.Clear();
                if (selectedNode == null)
                    return;
                //Storage
                if (nodeValue.StartsWith("Storage|"))
                {
                    this.createDetailsTable("Store name", "Description", "Store ID");
                    foreach (IAzManStore store in this.Storage.GetStores())
                    {
                        this.setDetailsTableRow(selectedNode, "Store_16x16.gif", store.Name, store.Description, store.StoreId.ToString());
                    }
                    this.addMenuItems(
                        new MenuInfo("Storage connection", "Get or Set Sql Storage connection settings"),
                        new MenuInfo("Options", ".NET Sql Authorization Manager options", new MenuInfo("Mode and Logging", "NetSqlAzMan Mode and Logging"), new MenuInfo("Auditing", "NetSqlAzMan autiding with SQLAudit")),
                        new MenuInfo("-"),
                        new MenuInfo("Invalidate WCF Cache Service", "Invoke the InvalidateCache() method on the WCF Cache Service"),
                        new MenuInfo("-"),
                        new MenuInfo("New Store", "Create a new Store in this Storage", this.Storage.IAmAdmin),
                        new MenuInfo("-"),
                        new MenuInfo("Import Stores", "Import .NET Sql Authorization Manager Stores data", this.Storage.IAmAdmin),
                        new MenuInfo("Export Stores", "Export .NET Sql Authorization Manager Stores data"),
                        new MenuInfo("Import Store from AzMan", "Import .NET Sql Authorization Manager Stores data from Microsoft Authorization Manager(MS AzMan)", this.Storage.IAmAdmin),
                        new MenuInfo("-"),
                        new MenuInfo("Refresh", "Refresh")
                        );
                }
                //Stores
                else if (nodeValue.StartsWith("Store|"))
                {
                    this.createDetailsTable("Application name", "Description", "Application ID");
                    this.setDetailsTableRow(selectedNode, "Folder_16x16.gif", "Store Groups", "Store Groups container", String.Empty);
                    IAzManStore store = this.Storage[this.getName(nodeValue, 1)];
                    foreach (IAzManApplication app in store.GetApplications())
                    {
                        this.setDetailsTableRow(selectedNode, "Application_16x16.gif", app.Name, app.Description, app.ApplicationId.ToString());
                    }

                    this.addMenuItems(
                        new MenuInfo("Store Properties", "Update Store Properties"),
                        new MenuInfo("-"),
                        new MenuInfo("New Application", "Create a new Application in the selected Store", store.IAmManager),
                        new MenuInfo("-"),
                        new MenuInfo("Items Hierarchical View", "Show a Hierarchical view of the selected Store"),
                        new MenuInfo("-"),
                        new MenuInfo("Reports", "Reports", 
                            new MenuInfo("Items Hierarchy", "Items Hierarchy Report", "rptItemsHierarchy.aspx"), 
                            new MenuInfo("Items Authorizations", "Items Authorizations Report", "rptItemsAuthorizations.aspx"),
                            new MenuInfo("Effective Permissions", "Effective Permissions Report", "rptEffectivePermissions.aspx")),
                        new MenuInfo("-"),
                        new MenuInfo("Import Store Groups/Application", "Import .NET Sql Authorization Manager Store Groups/Application data", store.IAmManager),
                        new MenuInfo("Export Store", "Export .NET Sql Authorization Manager Store data"),
                        new MenuInfo("-"),
                        new MenuInfo("Delete", "Delete selected Store", store.IAmManager, this.confirmClient("Delete selected Store ?", "mnuAction", "Delete")),
                        new MenuInfo("Refresh", "Refresh"));
                }
                //Store Groups
                else if (nodeValue.StartsWith("Store Groups|"))
                {
                    this.createDetailsTable("Store Group name", "Description", "Group Type", "SID");
                    IAzManStore store = this.Storage[this.getName(nodeValue, 2)];
                    foreach (IAzManStoreGroup sg in store.GetStoreGroups())
                    {
                        if (sg.GroupType == GroupType.Basic)
                            this.setDetailsTableRow(selectedNode, "StoreApplicationGroup_16x16.gif", sg.Name, sg.Description, sg.GroupType.ToString(), sg.SID.StringValue);
                        else
                            this.setDetailsTableRow(selectedNode, "WindowsQueryLDAPGroup_16x16.gif", sg.Name, sg.Description, sg.GroupType.ToString(), sg.SID.StringValue);
                    }
                    this.addMenuItems(
                        new MenuInfo("New Store Group", "Create a new Store Group", store.IAmManager),
                        new MenuInfo("-"),
                        new MenuInfo("Import Store Groups", "Import .NET Sql Authorization Manager Store Groups data", store.IAmManager),
                        new MenuInfo("-"),
                        new MenuInfo("Refresh", "Refresh"));
                }
                //Store Group
                else if (nodeValue.StartsWith("Store Group|"))
                {
                    //Basic
                    IAzManStoreGroup group = this.Storage[this.getName(nodeValue, 4)].GetStoreGroup(this.getName(nodeValue, 1));
                    if (group.GroupType == GroupType.Basic)
                    {
                        this.createDetailsTable("Member", "Where defined", "Member / Non Member", "SID");
                        foreach (IAzManStoreGroupMember member in group.GetStoreGroupAllMembers())
                        {
                            string displayName = String.Empty;
                            MemberType memberType = MemberType.AnonymousSID;
                            memberType = member.GetMemberInfo(out displayName);
                            string image;
                            switch (memberType)
                            {
                                case MemberType.ApplicationGroup: image = "StoreApplicationGroup_16x16.gif"; break;
                                case MemberType.StoreGroup: image = "StoreApplicationGroup_16x16.gif"; break;
                                case MemberType.WindowsNTGroup: image = "WindowsBasicGroup_16x16.gif"; break;
                                case MemberType.WindowsNTUser: image = "WindowsUser_16x16.gif"; break;
                                case MemberType.DatabaseUser: image = "DBUser_16x16.gif"; break;
                                case MemberType.AnonymousSID:
                                default:
                                    image = "SIDNotFound_16x16.gif"; break;
                            }
                            this.setDetailsTableRow(selectedNode, image, displayName, member.WhereDefined.ToString(), member.IsMember ? "Member" : "Non Member", member.SID.StringValue);
                        }
                    }
                    //LDAP
                    else
                    {
                        this.createDetailsTable("Name", "Description", "LDAP Query");
                        this.setDetailsTableRow(selectedNode, "WindowsQueryLDAPGroup_16x16.gif", group.Name, group.Description, group.LDAPQuery);
                    }
                    this.addMenuItems(
                        new MenuInfo("Store Group Properties", "Manage Store Group Properties"),
                        new MenuInfo("-"),
                        new MenuInfo("Export Store Group", "Export .NET Sql Authorization Manager Store Group data"),
                        new MenuInfo("-"),
                        new MenuInfo("Delete", "Delete selected Store Group", group.Store.IAmManager, this.confirmClient("Delete selected Store Group ?", "mnuAction", "Delete")),
                        new MenuInfo("Refresh", "Refresh"));
                }
                //Applications
                else if (nodeValue.StartsWith("Application|"))
                {
                    IAzManApplication application = this.Storage[this.getName(nodeValue, 3)][this.getName(nodeValue, 1)];
                    this.createDetailsTable("Name");
                    this.setDetailsTableRow(selectedNode, "Folder_16x16.gif", "Application Groups");
                    this.setDetailsTableRow(selectedNode, "Folder_16x16.gif", "Item Definitions");
                    this.setDetailsTableRow(selectedNode, "Folder_16x16.gif", "Item Authorizations");
                    if (this.Storage.Mode == NetSqlAzManMode.Developer)
                    {
                        this.addMenuItems(
                            new MenuInfo("Application Properties", "Update Application Properties"),
                            new MenuInfo("-"),
                            new MenuInfo("Items Hierarchical View", "Show a Hierarchical view of the selected Application"),
                            new MenuInfo("-"),
                            new MenuInfo("Generate CheckAccessHelper", "Generate CheckAccessHelper source code class (C#/VB.NET)"),
                            new MenuInfo("Check Access Test", "Check Access Test"),
                            new MenuInfo("-"),
                            new MenuInfo("Reports", "Reports", 
                                new MenuInfo("Items Hierarchy", "Items Hierarchy Report", "rptItemsHierarchy.aspx"), 
                                new MenuInfo("Items Authorizations", "Items Authorizations Report", "rptItemsAuthorizations.aspx"),
                                new MenuInfo("Effective Permissions", "Effective Permissions Report", "rptEffectivePermissions.aspx")),
                            new MenuInfo("-"),
                            new MenuInfo("Import Application Groups/Items", "Import .NET Sql Authorization Manager Application Groups/Items data", application.IAmManager),
                            new MenuInfo("Export Application", "Export .NET Sql Authorization Manager Application data"),
                            new MenuInfo("-"),
                            new MenuInfo("Delete", "Delete selected Application", application.IAmManager, this.confirmClient("Delete selected Application ?", "mnuAction", "Delete")),
                            new MenuInfo("Refresh", "Refresh"));
                    }
                    else
                    {
                        this.addMenuItems(
                                new MenuInfo("Application Properties", "Update Application Properties"),
                                new MenuInfo("-"),
                                new MenuInfo("Items Hierarchical View", "Show a Hierarchical view of the selected Application"),
                                new MenuInfo("-"),
                                new MenuInfo("Reports", "Reports", 
                                    new MenuInfo("Items Hierarchy", "Items Hierarchy Report", "rptItemsHierarchy.aspx"), 
                                    new MenuInfo("Items Authorizations", "Items Authorizations Report", "rptItemsAuthorizations.aspx"),
                                    new MenuInfo("Effective Permissions", "Effective Permissions Report", "rptEffectivePermissions.aspx")),
                                new MenuInfo("-"),
                                new MenuInfo("Import Application Groups/Items", "Import .NET Sql Authorization Manager Application Groups/Items data", application.IAmManager),
                                new MenuInfo("Export Application", "Export .NET Sql Authorization Manager Application data"),
                                new MenuInfo("-"),
                                new MenuInfo("Delete", "Delete selected Application", application.IAmManager, this.confirmClient("Delete selected Application ?", "mnuAction", "Delete")),
                                new MenuInfo("Refresh", "Refresh"));
                    }
                }
                //Application Groups
                else if (nodeValue.StartsWith("Application Groups|"))
                {
                    this.createDetailsTable("Application Group name", "Description", "Group Type", "SID");
                    IAzManApplication application = this.Storage[this.getName(nodeValue, 4)][this.getName(nodeValue, 2)];
                    foreach (IAzManApplicationGroup ag in application.GetApplicationGroups())
                    {
                        if (ag.GroupType == GroupType.Basic)
                            this.setDetailsTableRow(selectedNode, "StoreApplicationGroup_16x16.gif", ag.Name, ag.Description, ag.GroupType.ToString(), ag.SID.StringValue);
                        else
                            this.setDetailsTableRow(selectedNode, "WindowsQueryLDAPGroup_16x16.gif", ag.Name, ag.Description, ag.GroupType.ToString(), ag.SID.StringValue);
                    }

                    this.addMenuItems(
                        new MenuInfo("New Application Group", "Create a new Application Group", application.IAmManager),
                        new MenuInfo("-"),
                        new MenuInfo("Import Application Groups", "Import .NET Sql Authorization Manager Application Groups data", application.IAmManager),
                        new MenuInfo("-"),
                        new MenuInfo("Refresh", "Refresh"));
                }
                //Application Group
                else if (nodeValue.StartsWith("Application Group|"))
                {
                    //Basic
                    IAzManApplicationGroup group = this.Storage[this.getName(nodeValue, 6)][this.getName(nodeValue, 4)].GetApplicationGroup(this.getName(nodeValue, 1));
                    if (group.GroupType == GroupType.Basic)
                    {
                        this.createDetailsTable("Member", "Where defined", "Member / Non Member", "SID");
                        foreach (IAzManApplicationGroupMember member in group.GetApplicationGroupAllMembers())
                        {
                            string displayName;
                            MemberType memberType = member.GetMemberInfo(out displayName);
                            string image;
                            switch (memberType)
                            {
                                case MemberType.ApplicationGroup: image = "StoreApplicationGroup_16x16.gif"; break;
                                case MemberType.StoreGroup: image = "StoreApplicationGroup_16x16.gif"; break;
                                case MemberType.WindowsNTGroup: image = "WindowsBasicGroup_16x16.gif"; break;
                                case MemberType.WindowsNTUser: image = "WindowsUser_16x16.gif"; break;
                                case MemberType.DatabaseUser: image = "DBUser_16x16.gif"; break;
                                case MemberType.AnonymousSID:
                                default:
                                    image = "SIDNotFound_16x16.gif"; break;
                            }
                            this.setDetailsTableRow(selectedNode, image, displayName, member.WhereDefined.ToString(), member.IsMember ? "Member" : "Non Member", member.SID.StringValue);
                        }
                    }
                    //LDAP
                    else
                    {
                        this.createDetailsTable("Name", "Description", "LDAP Query");
                        this.setDetailsTableRow(selectedNode, "WindowsQueryLDAPGroup_16x16.gif", group.Name, group.Description, group.LDAPQuery);
                    }
                    this.addMenuItems(
                        new MenuInfo("Application Group Properties", "Manage Application Group Properties"),
                        new MenuInfo("-"),
                        new MenuInfo("Export Application Group", "Export .NET Sql Authorization Manager Application Group data"),
                        new MenuInfo("-"),
                        new MenuInfo("Delete", "Delete selected Application Group", group.Application.IAmManager, this.confirmClient("Delete selected Application Group ?", "mnuAction", "Delete")),
                        new MenuInfo("Refresh", "Refresh"));
                }
                //Item Definitions
                else if (nodeValue.StartsWith("Item Definitions|"))
                {
                    IAzManApplication application = this.Storage[this.getName(nodeValue, 4)][this.getName(nodeValue, 2)];
                    this.createDetailsTable("Name", "Description");
                    this.setDetailsTableRow(selectedNode, "Folder_16x16.gif", "Role Definitions", "Role Definitions container");
                    this.setDetailsTableRow(selectedNode, "Folder_16x16.gif", "Task Definitions", "Task Definitions container");
                    this.setDetailsTableRow(selectedNode, "Folder_16x16.gif", "Operation Definitions", "Operation Definitions container");
                    this.addMenuItems(
                        new MenuInfo("Import Items", "Export .NET Sql Authorization Manager Items data", application.IAmManager),
                        new MenuInfo("Export Items", "Export .NET Sql Authorization Manager Items data"),
                        new MenuInfo("-"),
                        new MenuInfo("Refresh", "Refresh"));

                }
                //Item Authorizations
                else if (nodeValue.StartsWith("Item Authorizations|"))
                {
                    this.createDetailsTable("Name", "Description");
                    this.setDetailsTableRow(selectedNode, "Folder_16x16.gif", "Role Authorizations", "Role Authorizations container");
                    this.setDetailsTableRow(selectedNode, "Folder_16x16.gif", "Task Authorizations", "Task Authorizations container");
                    this.setDetailsTableRow(selectedNode, "Folder_16x16.gif", "Operation Authorizations", "Operation Authorizations container");
                    this.addMenuItems(new MenuInfo("Refresh", "Refresh"));
                }
                //Role Definitions
                else if (nodeValue.StartsWith("Role Definitions|"))
                {
                    this.createDetailsTable("Role Name", "Description", "Role ID");
                    IAzManApplication application = this.Storage[this.getName(nodeValue, 5)][this.getName(nodeValue, 3)];
                    foreach (IAzManItem item in application.GetItems(ItemType.Role))
                    {
                        this.setDetailsTableRow(selectedNode, "Role_16x16.gif", item.Name, item.Description, item.ItemId.ToString());
                    }
                    this.addMenuItems(
                        new MenuInfo("New Role", "Create a new Role", application.IAmManager),
                        new MenuInfo("-"),
                        new MenuInfo("Refresh", "Refresh"));
                }
                //Role Definition
                else if (nodeValue.StartsWith("Role Definition|"))
                {
                    this.createDetailsTable("Member Name", "Member Type", "Member Description");
                    IAzManItem item = this.Storage[this.getName(nodeValue, 7)][this.getName(nodeValue, 5)][this.getName(nodeValue, 1)];
                    foreach (IAzManItem member in item.GetMembers())
                    {
                        string image = String.Empty;
                        switch (member.ItemType)
                        {
                            case ItemType.Role: image = "Role_16x16.gif"; break;
                            case ItemType.Task: image = "Task_16x16.gif"; break;
                            case ItemType.Operation: image = "Operation_16x16.gif"; break;
                        }
                        if (!(member.ItemType == ItemType.Operation && this.Storage.Mode == NetSqlAzManMode.Administrator))
                        {
                            this.setDetailsTableRow(selectedNode, image, member.Name, member.ItemType.ToString(), member.Description);
                        }
                    }
                    this.addMenuItems(
                        new MenuInfo("Role Properties", "Manage Role properties"),
                        new MenuInfo("-"),
                        new MenuInfo("Delete", "Delete selected Role", item.Application.IAmManager, this.confirmClient("Delete selected Role ?", "mnuAction", "Delete")),
                        new MenuInfo("Refresh", "Refresh"));
                }
                //Task Definitions
                else if (nodeValue.StartsWith("Task Definitions|"))
                {
                    this.createDetailsTable("Task Name", "Description", "Task ID");
                    IAzManApplication application = this.Storage[this.getName(nodeValue, 5)][this.getName(nodeValue, 3)];
                    foreach (IAzManItem item in application.GetItems(ItemType.Task))
                    {
                        this.setDetailsTableRow(selectedNode, "Task_16x16.gif", item.Name, item.Description, item.ItemId.ToString());
                    }
                    this.addMenuItems(
                        new MenuInfo("New Task", "Create a new Task", application.IAmManager),
                        new MenuInfo("-"),
                        new MenuInfo("Refresh", "Refresh"));
                }
                //Task Definition
                else if (nodeValue.StartsWith("Task Definition|"))
                {
                    this.createDetailsTable("Member Name", "Member Type", "Member Description");
                    IAzManItem item = this.Storage[this.getName(nodeValue, 7)][this.getName(nodeValue, 5)][this.getName(nodeValue, 1)];
                    foreach (IAzManItem member in item.GetMembers())
                    {
                        string image = String.Empty;
                        switch (member.ItemType)
                        {
                            case ItemType.Role: image = "Role_16x16.gif"; break;
                            case ItemType.Task: image = "Task_16x16.gif"; break;
                            case ItemType.Operation: image = "Operation_16x16.gif"; break;
                        }
                        if (!(member.ItemType == ItemType.Operation && this.Storage.Mode == NetSqlAzManMode.Administrator))
                        {
                            this.setDetailsTableRow(selectedNode, image, member.Name, member.ItemType.ToString(), member.Description);
                        }

                    }
                    this.addMenuItems(
                        new MenuInfo("Task Properties", "Manage Task properties"),
                        new MenuInfo("-"),
                        new MenuInfo("Delete", "Delete selected Task", item.Application.IAmManager, this.confirmClient("Delete selected Task ?", "mnuAction", "Delete")),
                        new MenuInfo("Refresh", "Refresh"));
                }
                //Operation Definitions
                else if (nodeValue.StartsWith("Operation Definitions|"))
                {
                    this.createDetailsTable("Operation Name", "Description", "Operation ID");
                    IAzManApplication application = this.Storage[this.getName(nodeValue, 5)][this.getName(nodeValue, 3)];
                    foreach (IAzManItem item in application.GetItems(ItemType.Operation))
                    {
                        this.setDetailsTableRow(selectedNode, "Operation_16x16.gif", item.Name, item.Description, item.ItemId.ToString());
                    }
                    this.addMenuItems(
                        new MenuInfo("New Operation", "Create a new Operation", application.IAmManager),
                        new MenuInfo("-"),
                        new MenuInfo("Refresh", "Refresh"));
                }
                //Operation Definition
                else if (nodeValue.StartsWith("Operation Definition|"))
                {
                    this.createDetailsTable("Member Name", "Member Type", "Member Description");
                    IAzManItem item = this.Storage[this.getName(nodeValue, 7)][this.getName(nodeValue, 5)][this.getName(nodeValue, 1)];
                    foreach (IAzManItem member in item.GetMembers())
                    {
                        string image = String.Empty;
                        switch (member.ItemType)
                        {
                            case ItemType.Role: image = "Role_16x16.gif"; break;
                            case ItemType.Task: image = "Task_16x16.gif"; break;
                            case ItemType.Operation: image = "Operation_16x16.gif"; break;
                        }
                        this.setDetailsTableRow(selectedNode, image, member.Name, member.ItemType.ToString(), member.Description);
                    }
                    this.addMenuItems(
                        new MenuInfo("Operation Properties", "Manage Operation properties"),
                        new MenuInfo("-"),
                        new MenuInfo("Delete", "Delete selected Operation", item.Application.IAmManager, this.confirmClient("Delete selected Operation ?", "mnuAction", "Delete")),
                        new MenuInfo("Refresh", "Refresh"));
                }
                //Role Authorizations
                else if (nodeValue.StartsWith("Role Authorizations|"))
                {
                    this.createDetailsTable("Role Name", "Description", "Role ID");
                    IAzManApplication application = this.Storage[this.getName(nodeValue, 5)][this.getName(nodeValue, 3)];
                    foreach (IAzManItem item in application.GetItems(ItemType.Role))
                    {
                        this.setDetailsTableRow(selectedNode, "Role_16x16.gif", item.Name, item.Description, item.ItemId.ToString());
                    }
                    this.addMenuItems(
                        new MenuInfo("Refresh", "Refresh"));
                }
                //Role Authorization
                else if (nodeValue.StartsWith("Role Authorization|"))
                {
                    this.createDetailsTable("Name", "Authorization Type", "Where Defined", "Owner", "Valid From", "Valid To", "SID");
                    IAzManItem item = this.Storage[this.getName(nodeValue, 7)][this.getName(nodeValue, 5)][this.getName(nodeValue, 1)];
                    foreach (IAzManAuthorization authorization in item.GetAuthorizations())
                    {
                        string sAuthType;
                        switch (authorization.AuthorizationType)
                        {
                            default:
                            case AuthorizationType.Neutral: sAuthType = "Neutral"; break;
                            case AuthorizationType.Allow: sAuthType = "Allow"; break;
                            case AuthorizationType.AllowWithDelegation: sAuthType = "Allow With Delegation"; break;
                            case AuthorizationType.Deny: sAuthType = "Deny"; break;
                        }
                        string displayName;
                        MemberType memberType = authorization.GetMemberInfo(out displayName);
                        string ownerName;
                        MemberType ownerType = authorization.GetOwnerInfo(out ownerName);
                        string image = String.Empty;
                        switch (memberType)
                        {
                            case MemberType.AnonymousSID: image = "SIDNotFound_16x16.gif"; break;
                            case MemberType.ApplicationGroup:
                                if (item.Application.GetApplicationGroup(authorization.SID).GroupType == GroupType.Basic)
                                    image = "StoreApplicationGroup_16x16.gif";
                                else
                                    image = "WindowsQueryLDAPGroup_16x16.gif";
                                break;
                            case MemberType.StoreGroup:
                                if (item.Application.Store.GetStoreGroup(authorization.SID).GroupType == GroupType.Basic)
                                    image = "StoreApplicationGroup_16x16.gif";
                                else
                                    image = "WindowsQueryLDAPGroup_16x16.gif";
                                break;
                            case MemberType.WindowsNTGroup: image = "WindowsBasicGroup_16x16.gif"; break;
                            case MemberType.WindowsNTUser: image = "WindowsUser_16x16.gif"; break;
                            case MemberType.DatabaseUser: image = "DBUser_16x16.gif"; break;
                        }
                        string sidWDS = String.Empty;
                        switch (authorization.SidWhereDefined.ToString())
                        {
                            case "LDAP": sidWDS = "Active Directory"; break;
                            case "Local": sidWDS = "Local Computer"; break;
                            case "Database": sidWDS = "Database"; break;
                            case "Store": sidWDS = "Store"; break;
                            case "Application": sidWDS = "Application"; break;
                        }
                        if (!(authorization.SidWhereDefined == WhereDefined.Local && this.Storage.Mode == NetSqlAzManMode.Administrator))
                        {
                            this.setDetailsTableRow(selectedNode, image, displayName, sAuthType, sidWDS, ownerName,
                                (authorization.ValidFrom.HasValue ? authorization.ValidFrom.Value.ToString() : String.Empty),
                                (authorization.ValidTo.HasValue ? authorization.ValidTo.Value.ToString() : String.Empty),
                                authorization.SID.StringValue);
                        }
                    }
                    this.addMenuItems(
                        new MenuInfo("Manage Authorizations", "Manage Item Authorizations"),
                        new MenuInfo("-"),
                        new MenuInfo("Refresh", "Refresh"));
                }
                //Task Authorizations
                else if (nodeValue.StartsWith("Task Authorizations|"))
                {
                    this.createDetailsTable("Task Name", "Description", "Task ID");
                    IAzManApplication application = this.Storage[this.getName(nodeValue, 5)][this.getName(nodeValue, 3)];
                    foreach (IAzManItem item in application.GetItems(ItemType.Task))
                    {
                        this.setDetailsTableRow(selectedNode, "Task_16x16.gif", item.Name, item.Description, item.ItemId.ToString());
                    }
                    this.addMenuItems(
                        new MenuInfo("Refresh", "Refresh"));
                }
                //Task Authorization
                else if (nodeValue.StartsWith("Task Authorization|"))
                {
                    this.createDetailsTable("Name", "Authorization Type", "Where Defined", "Owner", "Valid From", "Valid To", "SID");
                    IAzManItem item = this.Storage[this.getName(nodeValue, 7)][this.getName(nodeValue, 5)][this.getName(nodeValue, 1)];
                    foreach (IAzManAuthorization authorization in item.GetAuthorizations())
                    {
                        string sAuthType;
                        switch (authorization.AuthorizationType)
                        {
                            default:
                            case AuthorizationType.Neutral: sAuthType = "Neutral"; break;
                            case AuthorizationType.Allow: sAuthType = "Allow"; break;
                            case AuthorizationType.AllowWithDelegation: sAuthType = "Allow With Delegation"; break;
                            case AuthorizationType.Deny: sAuthType = "Deny"; break;
                        }
                        string displayName;
                        MemberType memberType = authorization.GetMemberInfo(out displayName);
                        string ownerName;
                        MemberType ownerType = authorization.GetOwnerInfo(out ownerName);
                        string image = String.Empty;
                        switch (memberType)
                        {
                            case MemberType.AnonymousSID: image = "SIDNotFound_16x16.gif"; break;
                            case MemberType.ApplicationGroup:
                                if (item.Application.GetApplicationGroup(authorization.SID).GroupType == GroupType.Basic)
                                    image = "StoreApplicationGroup_16x16.gif";
                                else
                                    image = "WindowsQueryLDAPGroup_16x16.gif";
                                break;
                            case MemberType.StoreGroup:
                                if (item.Application.Store.GetStoreGroup(authorization.SID).GroupType == GroupType.Basic)
                                    image = "StoreApplicationGroup_16x16.gif";
                                else
                                    image = "WindowsQueryLDAPGroup_16x16.gif";
                                break;
                            case MemberType.WindowsNTGroup: image = "WindowsBasicGroup_16x16.gif"; break;
                            case MemberType.WindowsNTUser: image = "WindowsUser_16x16.gif"; break;
                            case MemberType.DatabaseUser: image = "DBUser_16x16.gif"; break;
                        }
                        string sidWDS = String.Empty;
                        switch (authorization.SidWhereDefined.ToString())
                        {
                            case "LDAP": sidWDS = "Active Directory"; break;
                            case "Local": sidWDS = "Local Computer"; break;
                            case "Database": sidWDS = "Database"; break;
                            case "Store": sidWDS = "Store"; break;
                            case "Application": sidWDS = "Application"; break;
                        }
                        if (!(authorization.SidWhereDefined == WhereDefined.Local && this.Storage.Mode == NetSqlAzManMode.Administrator))
                        {
                            this.setDetailsTableRow(selectedNode, image, displayName, sAuthType, sidWDS, ownerName,
                                (authorization.ValidFrom.HasValue ? authorization.ValidFrom.Value.ToString() : String.Empty),
                                (authorization.ValidTo.HasValue ? authorization.ValidTo.Value.ToString() : String.Empty),
                                authorization.SID.StringValue);
                        }
                    }
                    this.addMenuItems(
                        new MenuInfo("Manage Authorizations", "Manage Item Authorizations"),
                        new MenuInfo("-"),
                        new MenuInfo("Refresh", "Refresh"));
                }
                //Operation Authorizations
                else if (nodeValue.StartsWith("Operation Authorizations|"))
                {
                    this.createDetailsTable("Operation Name", "Description", "Operation ID");
                    IAzManApplication application = this.Storage[this.getName(nodeValue, 5)][this.getName(nodeValue, 3)];
                    foreach (IAzManItem item in application.GetItems(ItemType.Operation))
                    {
                        this.setDetailsTableRow(selectedNode, "Operation_16x16.gif", item.Name, item.Description, item.ItemId.ToString());
                    }
                    this.addMenuItems(
                        new MenuInfo("Refresh", "Refresh"));
                }
                //Operation Authorization
                else if (nodeValue.StartsWith("Operation Authorization|"))
                {
                    this.createDetailsTable("Name", "Authorization Type", "Where Defined", "Owner", "Valid From", "Valid To", "SID");
                    IAzManItem item = this.Storage[this.getName(nodeValue, 7)][this.getName(nodeValue, 5)][this.getName(nodeValue, 1)];
                    foreach (IAzManAuthorization authorization in item.GetAuthorizations())
                    {
                        string sAuthType;
                        switch (authorization.AuthorizationType)
                        {
                            default:
                            case AuthorizationType.Neutral: sAuthType = "Neutral"; break;
                            case AuthorizationType.Allow: sAuthType = "Allow"; break;
                            case AuthorizationType.AllowWithDelegation: sAuthType = "Allow With Delegation"; break;
                            case AuthorizationType.Deny: sAuthType = "Deny"; break;
                        }
                        string displayName;
                        MemberType memberType = authorization.GetMemberInfo(out displayName);
                        string ownerName;
                        MemberType ownerType = authorization.GetOwnerInfo(out ownerName);
                        string image = String.Empty;
                        switch (memberType)
                        {
                            case MemberType.AnonymousSID: image = "SIDNotFound_16x16.gif"; break;
                            case MemberType.ApplicationGroup:
                                if (item.Application.GetApplicationGroup(authorization.SID).GroupType == GroupType.Basic)
                                    image = "StoreApplicationGroup_16x16.gif";
                                else
                                    image = "WindowsQueryLDAPGroup_16x16.gif";
                                break;
                            case MemberType.StoreGroup:
                                if (item.Application.Store.GetStoreGroup(authorization.SID).GroupType == GroupType.Basic)
                                    image = "StoreApplicationGroup_16x16.gif";
                                else
                                    image = "WindowsQueryLDAPGroup_16x16.gif";
                                break;
                            case MemberType.WindowsNTGroup: image = "WindowsBasicGroup_16x16.gif"; break;
                            case MemberType.WindowsNTUser: image = "WindowsUser_16x16.gif"; break;
                            case MemberType.DatabaseUser: image = "DBUser_16x16.gif"; break;
                        }
                        string sidWDS = String.Empty;
                        switch (authorization.SidWhereDefined.ToString())
                        {
                            case "LDAP": sidWDS = "Active Directory"; break;
                            case "Local": sidWDS = "Local Computer"; break;
                            case "Database": sidWDS = "Database"; break;
                            case "Store": sidWDS = "Store"; break;
                            case "Application": sidWDS = "Application"; break;
                        }
                        this.setDetailsTableRow(selectedNode, image, displayName, sAuthType, sidWDS, ownerName,
                            (authorization.ValidFrom.HasValue ? authorization.ValidFrom.Value.ToString() : String.Empty),
                            (authorization.ValidTo.HasValue ? authorization.ValidTo.Value.ToString() : String.Empty),
                            authorization.SID.StringValue);
                    }
                    this.addMenuItems(
                        new MenuInfo("Manage Authorizations", "Manage Item Authorizations"),
                        new MenuInfo("-"),
                        new MenuInfo("Refresh", "Refresh"));
                }
                #endregion showDetails
            }
            catch (NullReferenceException)
            {
                this.ShowError("The selected object could have been deleted.");
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }
        private void renderTreeRoot()
        {
            try
            {
                //Root
                string displayName = ".NET SQL Authorization Manager";
                string connectedUserName = "?";
                SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder(this.Storage.ConnectionString);
                if (csb.IntegratedSecurity)
                    connectedUserName = this.Request.LogonUserIdentity.Name;
                else
                    connectedUserName = csb.UserID.Trim();
                if (!String.IsNullOrEmpty(csb.DataSource))
                    displayName += String.Format(" ({0}\\{1} - {2})", csb.DataSource.Trim().ToUpper(), csb.InitialCatalog.Trim(), connectedUserName);
                this.tv.Nodes.Clear();
                TreeNode root = this.newTreeNode(displayName, "Storage|", null, "NetSqlAzMan_16x16.gif");
                root.ToolTip = displayName;
                root.Selected = true;
                root.Expand();
                this.SelectedObject = this.Storage;
                this.breadcrumbsLiteral.Text = this.buildBreadcrumbs(root);
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }
        private string getName(string nameWithPipe, int index)
        {
            char pipeSubst = Convert.ToChar(1);
            if (nameWithPipe.Contains("||"))
            {
                nameWithPipe = nameWithPipe.Replace("||", pipeSubst.ToString());
            }
            if (nameWithPipe.Contains("|"))
            {
                string result = nameWithPipe.Split('|')[index];
                result = result.Replace(pipeSubst, '|');
                return result;
            }
            else
            {
                return nameWithPipe.Replace(pipeSubst, '|');
            }
        }
        private TreeNode newTreeNode(string text, string value, TreeNode parentNode, string image)
        {
            string imageUrl = this.imageUrlPath + image;
            TreeNode tn = new TreeNode(HttpUtility.HtmlEncode(text), value, imageUrl);
            tn.ToolTip = text;
            if (
                value.StartsWith("Store Group|") ||
                value.StartsWith("Application Group|") ||
                value.StartsWith("Role Definition|") ||
                value.StartsWith("Task Definition|") ||
                value.StartsWith("Operation Definition|") ||
                value.StartsWith("Role Authorization|") ||
                value.StartsWith("Task Authorization|") ||
                value.StartsWith("Operation Authorization|"))
            {
                tn.PopulateOnDemand = false;
                tn.Expanded = true;
            }
            else
            {
                tn.PopulateOnDemand = true;
                tn.Expanded = false;
            }
            if (parentNode != null)
            {
                parentNode.ChildNodes.Add(tn);
            }
            else
            {
                this.tv.Nodes.Add(tn);
            }
            return tn;
        }
        protected string getImageUrl(string imagePath)
        {
            return this.getApplicationPath() + "/images/" + imagePath;
        }
        private void setMenuItemChild(MenuItemCollection mic, params MenuInfo[] menuItems)
        {
            try
            {
                mic.Clear();
                foreach (MenuInfo menuItem in menuItems)
                {
                    if (menuItem.Text != "-")
                    {
                        MenuItem mi = new MenuItem(menuItem.Text, menuItem.Text);
                        mi.Enabled = menuItem.Enabled;
                        if (
                            String.IsNullOrEmpty(menuItem.NavigateUrl)
                            && menuItem.Text != "Storage connection"
                            && menuItem.Text != "Refresh"
                            && menuItem.SubMenus.Count == 0
                            )
                        {
                            mi.NavigateUrl = String.Format("javascript:openDialog('ModalDialogHandler.aspx?MenuItem={0}&MenuValue={1}');", mi.Text, mi.Value);
                        }
                        else if (!String.IsNullOrEmpty(menuItem.NavigateUrl))
                        {
                            mi.NavigateUrl = menuItem.NavigateUrl;
                            if (!mi.NavigateUrl.StartsWith("javascript:", StringComparison.CurrentCultureIgnoreCase))
                                mi.Target = "_blank";
                        }
                        mi.ToolTip = menuItem.ToolTip;
                        switch (menuItem.Text)
                        {
                            case "Storage connection": mi.ImageUrl = this.getImageUrl("db.gif"); break;
                            case "Options": mi.ImageUrl = this.getImageUrl("Options_16x16.gif"); break;
                            case "Invalidate WCF Cache Service": mi.ImageUrl = this.getImageUrl("Cache_16x16.gif"); break;
                            case "New Store":
                            case "New Application":
                            case "New Store Group":
                            case "New Application Group":
                            case "New Role":
                            case "New Task":
                            case "New Operation":
                                mi.ImageUrl = this.getImageUrl("new.gif"); break;
                            case "Import Store from AzMan":
                                mi.ImageUrl = this.getImageUrl("azman.gif"); break;
                            case "Import Stores":
                            case "Import Store Groups/Application":
                            case "Import Application Groups/Items":
                            case "Import Store Groups":
                            case "Import Application Groups":
                            case "Import Items":
                                mi.ImageUrl = this.getImageUrl("import.gif"); break;
                            case "Export Stores":
                            case "Export Store":
                            case "Export Application":
                            case "Export Store Group":
                            case "Export Application Group":
                            case "Export Items":
                                mi.ImageUrl = this.getImageUrl("export.gif"); break;
                            case "Refresh": mi.ImageUrl = this.getImageUrl("refresh.gif"); break;
                            case "Store Properties": mi.ImageUrl = this.getImageUrl("Store_16x16.gif"); break;
                            case "Items Hierarchical View":
                            case "Items Hierarchy":
                                mi.ImageUrl = this.getImageUrl("Hierarchy_16x16.gif"); break;
                            case "Reports": mi.ImageUrl = this.getImageUrl("print.gif"); break;
                            case "Delete": mi.ImageUrl = this.getImageUrl("delete.gif"); break;
                            case "Items Authorizations": mi.ImageUrl = this.getImageUrl("ItemAuthorization_16x16.gif"); break;
                            case "Mode and Logging": mi.ImageUrl = this.getImageUrl("NetSqlAzMan_16x16.gif"); break;
                            case "Auditing": mi.ImageUrl = this.getImageUrl("SqlAudit_16x16.gif"); break;
                            case "Application Properties": mi.ImageUrl = this.getImageUrl("Application_16x16.gif"); break;
                            case "Generate CheckAccessHelper": mi.ImageUrl = this.getImageUrl("vsnet_16x16.gif"); break;
                            case "Check Access Test": mi.ImageUrl = this.getImageUrl("CheckAccessTest_16x16.gif"); break;
                            case "Application Group Properties":
                            case "Store Group Properties": mi.ImageUrl = this.getImageUrl("StoreApplicationGroup_16x16.gif"); break;
                            case "Role Properties": mi.ImageUrl = this.getImageUrl("Role_16x16.gif"); break;
                            case "Task Properties": mi.ImageUrl = this.getImageUrl("Task_16x16.gif"); break;
                            case "Operation Properties": mi.ImageUrl = this.getImageUrl("Operation_16x16.gif"); break;
                            case "Effective Permissions": mi.ImageUrl = this.getImageUrl("EffectivePermissions_16x16.gif"); break;
                            case "Manage Authorizations": mi.ImageUrl = this.getImageUrl("ItemAuthorization_16x16.gif"); break;
                            default: mi.ImageUrl = this.getImageUrl("blank.gif"); break;
                        }
                        mic.Add(mi);
                        this.setMenuItemChild(mi.ChildItems, menuItem.SubMenus.ToArray());
                    }
                    else
                    {
                        mic[mic.Count - 1].SeparatorImageUrl = "Images/Separator.jpg";
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }
        private void addMenuItems(params MenuInfo[] menuItems)
        {
            this.setMenuItemChild(this.mnuAction.Items, menuItems);
        }
        private void createDetailsTable(params string[] columnHeaders)
        {
            this.detailsRowIndex = -1;
            this.detailsTable.Rows.Clear();
            //Header
            TableRow header = new TableRow();
            this.detailsTable.Rows.Add(header);
            header.CssClass = "detailsTableHeader";
            //Headers
            for (int c = 0; c < columnHeaders.Length; c++)
            {
                TableCell cell = new TableCell();
                cell.Wrap = false;
                header.Cells.Add(cell);
                Label lbl = new Label();
                lbl.Text = columnHeaders[c];
                cell.Controls.Add(lbl);
            }
            // <hr />
            TableRow rowSep = new TableRow();
            this.detailsTable.Rows.Add(rowSep);
            TableCell cellSep = new TableCell();
            cellSep.ColumnSpan = columnHeaders.Length;
            rowSep.Cells.Add(cellSep);
            //Literal lit = new Literal();
            //lit.Text = "<hr />";
            //cellSep.Controls.Add(lit);

        }
        private void setDetailsTableRow(TreeNode containerNode, params string[] values)
        {
            try
            {
                //Details
                TableRow row = new TableRow();
                this.detailsRowIndex++;
                row.Attributes.Add("onmouseover", "this.className='detailsTableRowonMouseOver'");
                row.Attributes.Add("onmouseout", "this.className='detailsTableRowonMouseOut'");
                row.CssClass = "detailsTableRow";
                bool hasChildren = containerNode.ChildNodes.Count > this.detailsRowIndex;
                string postBackUrl = "#";
                if (hasChildren)
                {
                    TreeNode childNode = containerNode.ChildNodes[this.detailsRowIndex];
                    string internalValuePath = (string)childNode.GetType().InvokeMember("InternalValuePath", System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, null, childNode, null);
                    postBackUrl = this.Page.ClientScript.GetPostBackClientHyperlink(this.tv, "s" + internalValuePath, true);
                }
                this.detailsTable.Rows.Add(row);
                for (int c = 1; c < values.Length; c++)
                {
                    object value = values[c];
                    TableCell cell = new TableCell();
                    cell.CssClass = "detailsTableCell";
                    cell.Wrap = false;
                    row.Cells.Add(cell);
                    HyperLink hyperLink = new HyperLink();
                    hyperLink.CssClass = "detailHyperlink";
                    hyperLink.NavigateUrl = postBackUrl;
                    if (hasChildren)
                        cell.Controls.Add(hyperLink);

                    if (c == 1)
                    {
                        Image img = new Image();
                        string toolTip = "Logo";
                        string imageUrl = this.imageUrlPath + (string)values[0];
                        switch ((string)values[0])
                        {
                            case "Application_16x16.gif": toolTip = "Application"; break;
                            case "DBUser_16x16.gif": toolTip = "Database User"; break;
                            case "Folder_16x16.gif": toolTip = "Container"; break;
                            case "Operation_16x16.gif": toolTip = "Operation"; break;
                            case "Role_16x16.gif": toolTip = "Role"; break;
                            case "Task_16x16.gif": toolTip = "Task"; break;
                            case "SIDNotFound_16x16.gif": toolTip = "SID not found"; break;
                            case "Store_16x16.gif": toolTip = "Store"; break;
                            case "StoreApplicationGroup_16x16.gif": toolTip = "Group"; break;
                            case "WindowsBasicGroup_16x16.gif": toolTip = "Windows Basic Group"; break;
                            case "WindowsQueryLDAPGroup_16x16.gif": toolTip = "Windows LDAP Group"; break;
                            case "WindowsUser_16x16.gif": toolTip = "Windows User"; break;
                        }
                        img.AlternateText = toolTip;
                        img.ToolTip = toolTip;
                        img.ImageUrl = imageUrl;
                        if (hasChildren)
                            hyperLink.Controls.Add(img);
                        else
                            cell.Controls.Add(img);
                        Label lblSpace = new Label();
                        lblSpace.Text = "&nbsp;";
                        if (hasChildren)
                            hyperLink.Controls.Add(lblSpace);
                        else
                            cell.Controls.Add(lblSpace);
                    }
                    Label lbl = new Label();
                    lbl.Text = HttpUtility.HtmlEncode(values[c]);
                    lbl.ToolTip = values[c];
                    if (hasChildren)
                        hyperLink.Controls.Add(lbl);
                    else
                        cell.Controls.Add(lbl);
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }
        private string javascriptString(string text)
        {
            return Utility.QuoteJScriptString(text, false);
        }
        private string confirmClient(string text, string name, string value)
        {
            return String.Format("javascript:if (confirmClient('{0}')) __doPostBack('{1}','{2}');", this.javascriptString(text), this.javascriptString(name), this.javascriptString(value));
        }
        private void showMessage(messageType type, string text)
        {
            string typePrefix = String.Empty;
            switch (type)
            {
                case messageType.Info: typePrefix = "Information:\r\n"; break;
                case messageType.Warning: typePrefix = "Warning:\r\n"; break;
                case messageType.Error: typePrefix = "Error:\r\n"; break;
            }
            this.Page.ClientScript.RegisterStartupScript(typeof(string), "postBackAlert", String.Format("window.alert('{0}');", Utility.QuoteJScriptString(typePrefix + text, false)), true);
        }
        private void ShowInfo(string message)
        {
            this.showMessage(messageType.Info, message);
        }
        private void ShowWarning(string message)
        {
            this.showMessage(messageType.Warning, message);
        }
        private void ShowError(string message)
        {
            this.showMessage(messageType.Error, message);
        }
        private void sendFileToTheBrowser(string filename, byte[] content)
        {
            Response.Clear();
            Response.Buffer = false;
            Response.ContentType = "application/octet-stream";
            Response.AddHeader("Content-Disposition", String.Format("attachment;filename={0}", filename));
            Response.BinaryWrite(content);
            Response.End();
        }
        protected string buildBreadcrumbs(TreeNode startingNode)
        {
            if (startingNode == null)
                return "<span class=\"breadCrumLabel\">Path</span>";
            string internalValuePath = (string)startingNode.GetType().InvokeMember("InternalValuePath", System.Reflection.BindingFlags.Default | System.Reflection.BindingFlags.GetProperty | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance, null, startingNode, null);
            string postBackUrl = this.Page.ClientScript.GetPostBackClientHyperlink(this.tv, "s" + internalValuePath, true);
            string breadcrumSeparatorImageUrl = this.getImageUrl("breadcrumseparator.gif");
            string separator = String.Format("<span class=\"breadCrumSeparator\">&nbsp;<img src=\"{0}\" alt=\">>\" title=\">>\" /> &nbsp;</span>", breadcrumSeparatorImageUrl);
            string nodeText = startingNode.Parent != null ? startingNode.Text : "Storage";
            string breadCrum;
            if (startingNode == this.tv.SelectedNode)
            {
                string template = "<span class=\"breadCrumLast\" title=\"{1}\"><img src=\"{0}\" alt=\"{1}\" title=\"{1}\" />&nbsp;{2}</span>&nbsp;";
                breadCrum = String.Format(template, startingNode.ImageUrl, Utility.QuoteJScriptString(startingNode.ToolTip, false), Utility.QuoteJScriptString(nodeText, false));
            }
            else
            {
                string template = "<a href=\"{0}\" title=\"{1}\" class=\"breadCrum\"><img src=\"{2}\" alt=\"{3}\" title=\"{3}\" />&nbsp;{4}</a>&nbsp;";
                breadCrum = String.Format(template, postBackUrl, Utility.QuoteJScriptString(startingNode.ToolTip, false), startingNode.ImageUrl, Utility.QuoteJScriptString(startingNode.ToolTip, false), Utility.QuoteJScriptString(nodeText, false));
            }
            string result = this.buildBreadcrumbs(startingNode.Parent) + separator + breadCrum;
            if (result.StartsWith(separator))
                result = result.Substring(separator.Length);
            return result;
        }
        

        private void writeThemeCookie(string theme)
        {
            HttpCookie c = new HttpCookie("NetSqlAzManWebConsole-Theme");
            c.Expires = DateTime.Now.AddYears(1000);
            c["Theme"] = theme;
            HttpCookie encodedCookie = HttpSecureCookie.Encode(c);
            this.Response.Cookies.Add(encodedCookie);
        }

        #endregion Methods
        #region Themes
        protected void DefaultTheme_Click(object sender, EventArgs e)
        {
            this.writeThemeCookie("Default");
            Response.Redirect("~/WebConsole.aspx", false);
        }

        protected void GreenTheme_Click(object sender, EventArgs e)
        {
            this.writeThemeCookie("Green");
            Response.Redirect("~/WebConsole.aspx", false);
        }

        protected void YellowTheme_Click(object sender, EventArgs e)
        {
            this.writeThemeCookie("Yellow");
            Response.Redirect("~/WebConsole.aspx", false);
        }
        #endregion Themes
    }
}
