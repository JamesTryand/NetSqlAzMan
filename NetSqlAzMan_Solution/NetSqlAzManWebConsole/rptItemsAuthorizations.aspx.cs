using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzManWebConsole
{
    public partial class rptItemsAuthorizations : dlgPage
    {
        protected internal IAzManStorage storage = null;
        internal IAzManApplication[] applications = null;
        //[PreEmptive.Attributes.Feature("NetSqlAzMan WebConsole: Report Items Authorizations")]
        protected void Page_Init(object sender, EventArgs e)
        {
            this.setImage("ItemAuthorization_32x32.gif");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.storage = this.Session["storage"] as IAzManStorage;
            if (this.Session["selectedObject"] as IAzManStore!=null)
            {
                IAzManStore store = ((IAzManStore)this.Session["selectedObject"]);
                this.applications = new IAzManApplication[store.Applications.Count];
                store.Applications.Values.CopyTo(this.applications, 0);
            }
            else
            {
                
                this.applications = new IAzManApplication[] { (IAzManApplication)this.Session["selectedObject"] };
            }
            this.Text = "Items Authorizations";
            this.Description = String.Empty;
            this.Title = this.Text;
            this.reportMode(true);
            string nowaitpanel = this.Request["nowaitpanel"];
            if (String.IsNullOrEmpty(nowaitpanel))
            {
                if (!Page.IsPostBack)
                {
                    this.showWaitPanelNow(this.pnlWait, this.itemsHierachyPanel);
                    this.RegisterEndClientScript("window.location='rptItemsAuthorizations.aspx?nowaitpanel=true';");
                }
            }
            else if (nowaitpanel == "true")
            {
                this.itemsHierachyPanel.Visible = true;
                this.pnlWait.Visible = false;
                if (!Page.IsPostBack)
                {
                    this.buildApplicationsTreeView();
                    this.itemsHierarchyTreeView.ExpandAll();
                }
            }
        }

        internal bool findNode(TreeNode startingNode, string text)
        {
            foreach (TreeNode childNode in startingNode.ChildNodes)
            {
                if (childNode.Text.Equals(text))
                    return true;
                if (this.findNode(childNode, text))
                    return true;
            }
            return false;
        }

        internal TreeNode findTreeNode(TreeNode startingNode, string text)
        {
            if (String.Compare(startingNode.Text, text, true) == 0)
                return startingNode;
            foreach (TreeNode childNode in startingNode.ChildNodes)
            {
                TreeNode result = this.findTreeNode(childNode, text);
                if (result != null)
                    return result;
            }
            return null;
        }

        internal protected void buildApplicationsTreeView()
        {
            if (this.applications != null && this.applications.Length > 0)
            {
                this.storage.OpenConnection();
                this.itemsHierarchyTreeView.Nodes.Clear();
                TreeNode root = new TreeNode("NetSqlAzMan", "NetSqlAzMan", this.getImageUrl("NetSqlAzMan_16x16.gif"));
                root.ToolTip = ".NET Sql Authorization Manager";
                this.itemsHierarchyTreeView.Nodes.Add(root);
                TreeNode storeNode = new TreeNode(this.applications[0].Store.Name, this.applications[0].Store.Name, this.getImageUrl("Store_16x16.gif"));
                root.ChildNodes.Add(storeNode);
                root.Expand();
                storeNode.Expand();
                storeNode.ToolTip = this.applications[0].Store.Description;
                for (int i = 0; i < this.applications.Length; i++)
                {
                    this.add(storeNode, this.applications[i]);
                    storeNode.Expand();
                }
                this.storage.CloseConnection();
                root.Expand();
                storeNode.Expand();
            }
        }

        private void add(TreeNode parent, IAzManApplication app)
        {
            TreeNode node = new TreeNode(app.Name, app.Name, this.getImageUrl("Application_16x16.gif"));
            node.ToolTip = app.Description;
            parent.ChildNodes.Add(node);
            node.Expand();
            foreach (IAzManItem item in app.Items.Values)
            {
                if (item.ItemType == ItemType.Role)
                {
                    if (item.ItemsWhereIAmAMember.Count == 0) this.AddRole(node, item, node);
                }
            }
            foreach (IAzManItem item in app.Items.Values)
            {
                if (item.ItemType == ItemType.Task)
                {
                    if (item.ItemsWhereIAmAMember.Count == 0) this.AddTask(node, item, node);
                }
            }
            if (app.Store.Storage.Mode == NetSqlAzManMode.Developer)
            {
                foreach (IAzManItem item in app.Items.Values)
                {
                    if (item.ItemType == ItemType.Operation)
                    {
                        if (item.ItemsWhereIAmAMember.Count == 0) this.AddOperation(node, item, node);
                    }
                }
            }
            node.Collapse();
        }

        private void AddRole(TreeNode parent, IAzManItem item, TreeNode applicationNode)
        {
            TreeNode node = new TreeNode(item.Name, item.Name, this.getImageUrl("Role_16x16.gif"));
            node.ToolTip = item.Description;
            parent.ChildNodes.Add(node);
            this.AddAuthorizations(node, item);
            foreach (IAzManItem subItem in item.Members.Values)
            {
                if (subItem.ItemType == ItemType.Role)
                {
                    this.AddRole(node, subItem, applicationNode);
                }
            }
            foreach (IAzManItem subItem in item.Members.Values)
            {
                if (subItem.ItemType == ItemType.Task)
                {
                    this.AddTask(node, subItem, applicationNode);
                }
            }
            if (item.Application.Store.Storage.Mode == NetSqlAzManMode.Developer)
            {
                foreach (IAzManItem subItem in item.Members.Values)
                {
                    if (subItem.ItemType == ItemType.Operation)
                    {
                        this.AddOperation(node, subItem, applicationNode);
                    }
                }
            }
            node.Collapse();
        }

        private void AddTask(TreeNode parent, IAzManItem item, TreeNode applicationNode)
        {
            TreeNode node = new TreeNode(item.Name, item.Name, this.getImageUrl("Task_16x16.gif"));
            node.ToolTip = item.Description;
            parent.ChildNodes.Add(node);
            this.AddAuthorizations(node, item);
            foreach (IAzManItem subItem in item.Members.Values)
            {
                if (subItem.ItemType == ItemType.Task)
                {
                    this.AddTask(node, subItem, applicationNode);
                }
            }
            if (item.Application.Store.Storage.Mode == NetSqlAzManMode.Developer)
            {
                foreach (IAzManItem subItem in item.Members.Values)
                {
                    if (subItem.ItemType == ItemType.Operation)
                    {
                        this.AddOperation(node, subItem, applicationNode);
                    }
                }
            }
            node.Collapse();
        }
        private void AddOperation(TreeNode parent, IAzManItem item, TreeNode applicationNode)
        {
            TreeNode node = new TreeNode(item.Name, item.Name, this.getImageUrl("Operation_16x16.gif"));
            node.ToolTip = item.Description;
            parent.ChildNodes.Add(node);
            this.AddAuthorizations(node, item);
            foreach (IAzManItem subItem in item.Members.Values)
            {
                this.AddOperation(node, subItem, applicationNode);
            }
            node.Collapse();
        }


        private void AddAuthorizations(TreeNode parent, IAzManItem item)
        {
            bool stop = false;
            AuthorizationType authType = AuthorizationType.AllowWithDelegation;
            IAzManAuthorization[] authz = new IAzManAuthorization[item.Authorizations.Count];
            item.Authorizations.CopyTo(authz, 0); ;
            if (authz.Length > 0)
            {
                while (!stop)
                {
                    string sAuthz = String.Empty;
                    string imageType = String.Empty;
                    string sAuthType = String.Empty;
                    switch (authType)
                    {
                        case AuthorizationType.AllowWithDelegation: sAuthType = "Allow With Delegation"; imageType = "AllowForDelegation.bmp"; break;
                        case AuthorizationType.Allow: sAuthType = "Allow"; imageType = "Allow.bmp"; break;
                        case AuthorizationType.Deny: sAuthType = "Deny"; imageType = "Deny.bmp"; break;
                        case AuthorizationType.Neutral: sAuthType = "Neutral"; imageType = "Neutral.bmp"; break;
                    }
                    foreach (IAzManAuthorization auth in authz)
                    {
                        if (auth.AuthorizationType == authType)
                        {
                            string displayName = String.Empty;
                            MemberType mt = auth.GetMemberInfo(out displayName);
                            sAuthz += displayName + ", ";
                        }
                    }
                    if (sAuthz.EndsWith(", ")) sAuthz = sAuthz.Remove(sAuthz.Length - 2);
                    if (!String.IsNullOrEmpty(sAuthz))
                    {
                        TreeNode imageNode = new TreeNode(sAuthType, sAuthType, this.getImageUrl(imageType));
                        parent.ChildNodes.Add(imageNode);
                        TreeNode authzNode = new TreeNode(sAuthz, sAuthz);
                        imageNode.ChildNodes.Add(authzNode);
                    }
                    switch (authType)
                    {
                        case AuthorizationType.AllowWithDelegation: authType = AuthorizationType.Allow; break;
                        case AuthorizationType.Allow: authType = AuthorizationType.Deny; break;
                        case AuthorizationType.Deny: authType = AuthorizationType.Neutral; break;
                        case AuthorizationType.Neutral: stop = true; break;
                    }
                    if (stop) break;
                }
            }
        }

    }
}
