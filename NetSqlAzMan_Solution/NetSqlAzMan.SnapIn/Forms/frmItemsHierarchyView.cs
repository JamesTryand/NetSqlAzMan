using System;
using System.Drawing;
using System.Windows.Forms;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzMan.SnapIn.Forms
{
    public partial class frmItemsHierarchyView : frmBase
    {
        internal IAzManApplication[] applications=null;
        private bool firstShow = true;
        private bool closeRequest = false;

        public frmItemsHierarchyView()
        {
            InitializeComponent();
        }

        internal void frmNetSqlAzManBase_Load(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
            ImageList clonedImageList = new ImageList();
            foreach (Image image in this.imageListItemHierarchyView.Images)
            {
                clonedImageList.Images.Add((Image)image.Clone());
            }
            if (this.applications != null && this.applications.Length > 0)
            { 
                //Refresh items cache 
                foreach (IAzManApplication app in this.applications)
                {
                    if (app as SqlAzManApplication != null)
                    {
                        ((SqlAzManApplication)app).GetItems();
                    }
                }
            }
            this.itemsHierarchyTreeView.ImageList = clonedImageList;
            /*Application.DoEvents();*/
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

        private void frmNetSqlAzManBase_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.None)
                this.DialogResult = DialogResult.Cancel;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.closeRequest = true;
            this.DialogResult = DialogResult.OK;
        }

        internal bool findNode(TreeNode startingNode, string text)
        {
            foreach (TreeNode childNode in startingNode.Nodes)
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
            foreach (TreeNode childNode in startingNode.Nodes)
            {
                TreeNode result = this.findTreeNode(childNode, text);
                if (result != null)
                    return result;
            }
            return null;
        }

        internal void buildApplicationsTreeView()
        {
            if (this.applications != null && this.applications.Length > 0)
            {
                IAzManStorage storage = this.applications[0].Store.Storage;
                storage.OpenConnection();
                this.itemsHierarchyTreeView.Nodes.Clear();
                TreeNode root = new TreeNode("NetSqlAzMan", 0, 0);
                root.ToolTipText = ".NET Sql Authorization Manager";
                this.itemsHierarchyTreeView.Nodes.Add(root);
                TreeNode storeNode = new TreeNode(this.applications[0].Store.Name, 1, 1);
                root.Nodes.Add(storeNode); 
                root.Expand();
                storeNode.Expand();
                /*Application.DoEvents();*/
                storeNode.ToolTipText = this.applications[0].Store.Description;
                this.pb.Value = 0;
                this.pb.Minimum = 0;
                this.pb.Maximum = this.applications.Length - 1;
                this.pb.Visible = true;
                this.lblStatus.Visible = true;
                for (int i = 0; i < this.applications.Length; i++)
                {
                    if (this.closeRequest)
                    {
                        return;
                    }
                    this.pb.Value = i;
                    /*Application.DoEvents();*/
                    this.add(storeNode, this.applications[i]);
                    storeNode.Expand();
                    /*Application.DoEvents();*/
                }
                storage.CloseConnection();
                root.ExpandAll();
                this.pb.Visible = false;
                this.lblStatus.Visible = false;
            }
        }

        private void add(TreeNode parent, IAzManApplication app)
        {
            TreeNode node = new TreeNode(app.Name, 2, 2);
            node.ToolTipText = app.Description;
            node.Tag = app;
            parent.Nodes.Add(node);
            node.Expand();
            foreach (IAzManItem item in app.Items.Values)
            {
                if (item.ItemType == ItemType.Role)
                {
                    if (item.ItemsWhereIAmAMember.Count== 0) this.AddRole(node, item, node);
                    /*Application.DoEvents();*/
                }
            }
            foreach (IAzManItem item in app.Items.Values)
            {
                if (item.ItemType == ItemType.Task)
                {
                    if (item.ItemsWhereIAmAMember.Count == 0) this.AddTask(node, item, node);
                    /*Application.DoEvents();*/
                }
            }
            foreach (IAzManItem item in app.Items.Values)
            {
                if (item.ItemType == ItemType.Operation)
                {
                    if (item.ItemsWhereIAmAMember.Count == 0) this.AddOperation(node, item, node);
                    /*Application.DoEvents();*/
                }
            }
            node.Collapse();
        }

        private void AddRole(TreeNode parent, IAzManItem item, TreeNode applicationNode)
        {
            TreeNode node = new TreeNode(item.Name, 3, 3);
            node.ToolTipText = item.Description;
            node.Tag = item;
            parent.Nodes.Add(node);
            foreach (IAzManItem subItem in item.Members.Values)
            {
                if (subItem.ItemType == ItemType.Role)
                {
                    this.AddRole(node, subItem, applicationNode);
                    node.Expand();
                    /*Application.DoEvents();*/
                }
            }
            foreach (IAzManItem subItem in item.Members.Values)
            {
                if (subItem.ItemType == ItemType.Task)
                {
                    this.AddTask(node, subItem, applicationNode);
                    node.Expand();
                    /*Application.DoEvents();*/
                }
            }
            foreach (IAzManItem subItem in item.Members.Values)
            {
                if (subItem.ItemType == ItemType.Operation)
                {
                    this.AddOperation(node, subItem, applicationNode);
                    node.Expand();
                    /*Application.DoEvents();*/
                }
            }
        }

        private void AddTask(TreeNode parent, IAzManItem item, TreeNode applicationNode)
        {
            TreeNode node = new TreeNode(item.Name, 4, 4);
            node.ToolTipText = item.Description;
            node.Tag = item;
            parent.Nodes.Add(node);
            foreach (IAzManItem subItem in item.Members.Values)
            {
                if (subItem.ItemType == ItemType.Task)
                {
                    this.AddTask(node, subItem, applicationNode);
                    node.Expand();
                    /*Application.DoEvents();*/
                }
            }
            foreach (IAzManItem subItem in item.Members.Values)
            {
                if (subItem.ItemType == ItemType.Operation)
                {
                    this.AddOperation(node, subItem, applicationNode);
                    node.Expand();
                    /*Application.DoEvents();*/
                }
            }
        }
        private void AddOperation(TreeNode parent, IAzManItem item, TreeNode applicationNode)
        {
            TreeNode node = new TreeNode(item.Name, 5, 5);
            node.ToolTipText = item.Description;
            node.Tag = item;
            parent.Nodes.Add(node);
            foreach (IAzManItem subItem in item.Members.Values)
            {
                this.AddOperation(node, subItem, applicationNode);
                node.Expand();
                /*Application.DoEvents();*/
            }
        }

        private void frmItemsHierarchyView_Activated(object sender, EventArgs e)
        {
            if (this.firstShow)
            {
                this.firstShow = false;
                try
                {
                    this.buildApplicationsTreeView();
                }
                catch (Exception ex)
                {
                    this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("frmItemsHierarchyView_Msg10"));
                    this.DialogResult = DialogResult.Cancel;
                    this.Close();
                }
            }
        }
    }
}