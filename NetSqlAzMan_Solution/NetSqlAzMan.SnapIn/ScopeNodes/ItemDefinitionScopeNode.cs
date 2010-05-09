using System;
using System.Windows.Forms;
using Microsoft.ManagementConsole.Advanced;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.SnapIn.Forms;
using NetSqlAzMan.SnapIn.ListViews;
using MMC = Microsoft.ManagementConsole;

namespace NetSqlAzMan.SnapIn.ScopeNodes
{
    public class ItemDefinitionScopeNode : ScopeNodeBase
    {
        protected IAzManItem item;
        public ItemDefinitionScopeNode(IAzManItem item) : base(true)
        {
            this.item = item;
            // Create a message view for the Store node.
            MMC.MmcListViewDescription lvdItems = new MMC.MmcListViewDescription();
            switch (this.item.ItemType)
            {
                case ItemType.Role:
                    lvdItems.DisplayName = Globalization.MultilanguageResource.GetString("ListView_Msg30");
                    break;
                case ItemType.Task:
                    lvdItems.DisplayName = Globalization.MultilanguageResource.GetString("ListView_Msg40");
                    break;
                case ItemType.Operation:
                    lvdItems.DisplayName = Globalization.MultilanguageResource.GetString("ListView_Msg50");
                    break;
            }
            lvdItems.ViewType = typeof(ItemMembersListView);
            lvdItems.Options = MMC.MmcListViewOptions.AllowUserInitiatedModeChanges;
            this.ViewDescriptions.Clear();
            this.ViewDescriptions.Add(lvdItems);
            this.ViewDescriptions.DefaultIndex = 0;
            this.RenderItemDefinitionScopeNode();
        }

        protected override void OnExpand(MMC.AsyncStatus status)
        {
            status.ReportProgress(50, 100, Globalization.MultilanguageResource.GetString("Expanding_Msg10"));
            this.Render();
            status.Complete(Globalization.MultilanguageResource.GetString("Done_Msg10"), true);
            base.OnExpand(status);
        }

        protected override bool OnExpandFromLoad(MMC.SyncStatus status)
        {
            status.ReportProgress(50, 100, Globalization.MultilanguageResource.GetString("Expanding_Msg10"));
            this.Render();
            status.Complete(Globalization.MultilanguageResource.GetString("Done_Msg10"), true);
            return base.OnExpandFromLoad(status);
        }

        public IAzManItem Item
        {
            get
            {
                return this.item;
            }
        }

        protected void RenderItemDefinitionScopeNode()
        {
            //Prepare Node
            this.DisplayName = item.Name;
            this.SubItemDisplayNames.Clear();
            this.SubItemDisplayNames.AddRange(
                new string[] {
                                item.Description,
                                item.ItemId.ToString()});
            switch (this.item.ItemType)
            { 
                case ItemType.Role:
                    this.ImageIndex = this.SelectedImageIndex = ImageIndexes.RoleImgIdx;
                    break;
                case ItemType.Task:
                    this.ImageIndex = this.SelectedImageIndex = ImageIndexes.TaskImgIdx;
                    break;
                case ItemType.Operation:
                    this.ImageIndex = this.SelectedImageIndex = ImageIndexes.OperationImgIdx;
                    break;
            }
            //Assign Tag
            this.Tag = item;
            //Enable standard verbs
            if (this.item.Application.IAmManager)
                this.EnabledStandardVerbs = MMC.StandardVerbs.Refresh | MMC.StandardVerbs.Delete;
            else
                this.EnabledStandardVerbs = MMC.StandardVerbs.Refresh;
            //Add custom actions
            this.ActionsPaneItems.Clear();
            //Item Properties - MMC.SyncAction
            string itemTypeName = String.Empty;
            MMC.SyncAction itemDefinitionPropertiesAction = null;
            switch (this.item.ItemType)
            {
                case ItemType.Role:
                    itemDefinitionPropertiesAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("ListView_Msg60"), Globalization.MultilanguageResource.GetString("ListView_Tit60"));
                    break;
                case ItemType.Task:
                    itemDefinitionPropertiesAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("ListView_Msg70"), Globalization.MultilanguageResource.GetString("ListView_Tit70"));
                    break;
                case ItemType.Operation:
                    itemDefinitionPropertiesAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("ListView_Msg80"), Globalization.MultilanguageResource.GetString("ListView_Tit80"));
                    break;
            }
            itemDefinitionPropertiesAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(itemDefinitionPropertiesAction_Triggered);
            this.ActionsPaneItems.Add(itemDefinitionPropertiesAction);
        }

        protected override void Render()
        {
            //Prepare Node
            this.RenderItemDefinitionScopeNode();
            /*System.Windows.Forms.Application.DoEvents();*/
        }

        protected override void OnDelete(MMC.SyncStatus status)
        {
            MessageBoxParameters msg = new MessageBoxParameters();
            switch (this.item.ItemType)
            {
                case ItemType.Role:
                    msg.Caption = Globalization.MultilanguageResource.GetString("ListView_Msg90");
                    msg.Text = String.Format("{0}\r\n{1}", Globalization.MultilanguageResource.GetString("ListView_Msg100"), this.item.Name);
                    break;
                case ItemType.Task:
                    msg.Caption = Globalization.MultilanguageResource.GetString("ListView_Msg110");
                    msg.Text = String.Format("{0}\r\n{1}", Globalization.MultilanguageResource.GetString("ListView_Msg120"), this.item.Name);
                    break;
                case ItemType.Operation:
                    msg.Caption = Globalization.MultilanguageResource.GetString("ListView_Msg130");
                    msg.Text = String.Format("{0}\r\n{1}", Globalization.MultilanguageResource.GetString("ListView_Msg140"), this.item.Name);
                    break;
            }
            msg.Icon = MessageBoxIcon.Question;
            msg.Buttons = MessageBoxButtons.YesNo;
            msg.DefaultButton = MessageBoxDefaultButton.Button2;
            DialogResult dr = this.SnapIn.Console.ShowDialog(msg);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    this.item.Delete();
                    try
                    {
                        //Remove relative child and all its children in Item Authorizations
                        MMC.ScopeNode itemDefinitionsScopeNode = this.Parent.Parent;
                        MMC.ScopeNode itemAuthorizationsScopeNode = itemDefinitionsScopeNode.Parent.Children[2];
                        switch (this.item.ItemType)
                        {
                            case ItemType.Role:
                                if (itemAuthorizationsScopeNode.Children.Count >= 1)
                                    itemAuthorizationsScopeNode = itemAuthorizationsScopeNode.Children[0];
                                else
                                    return;
                                break;
                            case ItemType.Task:
                                if (itemAuthorizationsScopeNode.Children.Count >= 2)
                                    itemAuthorizationsScopeNode = itemAuthorizationsScopeNode.Children[1];
                                else
                                    return;
                                break;
                            case ItemType.Operation:
                                if (itemAuthorizationsScopeNode.Children.Count >= 3)
                                    itemAuthorizationsScopeNode = itemAuthorizationsScopeNode.Children[2];
                                else
                                    return;
                                break;
                        }
                        this.RemoveItemAuthorizationScopeNode(itemAuthorizationsScopeNode, this);
                    }
                    catch { }
                }
                catch (Exception ex)
                {
                    this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("DeleteError_Msg10"));
                }
                finally
                {
                    this.Parent.Children.Remove(this);
                }

            }
        }

        private void RemoveItemAuthorizationScopeNode(MMC.ScopeNode itemAuthorizationsScopeNode, ItemDefinitionScopeNode itemDefinitionToRemove)
        {
            foreach (ItemAuthorizationScopeNode itemAuthorizationScopeNode in itemAuthorizationsScopeNode.Children)
            {
                if (itemAuthorizationScopeNode.Item.Name == itemDefinitionToRemove.Item.Name)
                {
                    itemAuthorizationsScopeNode.Children.Remove(itemAuthorizationScopeNode);
                    break;
                }
            }
        }

        void itemDefinitionPropertiesAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            frmItemProperties frm = new frmItemProperties();
            frm.Text += " - " + this.item.Name;
            frm.application = this.item.Application;
            frm.item = this.item;
            frm.itemType = this.item.ItemType;
            string oldItemName = this.item.Name;
            DialogResult dr = this.SnapIn.Console.ShowDialog(frm);
            /*Application.DoEvents();*/
            frm.Dispose();
            /*Application.DoEvents();*/
            if (dr == DialogResult.OK)
            {
                this.Render();
                try
                {
                    try
                    {
                        this.NotifyChanged();
                        /*Application.DoEvents();*/
                    }
                    catch { }
                    //Update relative child in Item Authorizations
                    ItemDefinitionsScopeNode itemDefinitionsScopeNode = (ItemDefinitionsScopeNode)this.Parent.Parent;
                    MMC.ScopeNode itemAuthorizationsScopeNode = itemDefinitionsScopeNode.Parent.Children[2];
                    switch (this.item.ItemType)
                    {
                        case ItemType.Role:
                            if (itemAuthorizationsScopeNode != null && itemAuthorizationsScopeNode.Children.Count >= 1)
                                itemAuthorizationsScopeNode = itemAuthorizationsScopeNode.Children[0];
                            else
                                return;
                            break;
                        case ItemType.Task:
                            if (itemAuthorizationsScopeNode != null && itemAuthorizationsScopeNode.Children.Count >= 2)
                                itemAuthorizationsScopeNode = itemAuthorizationsScopeNode.Children[1];
                            else
                                return;
                            break;
                        case ItemType.Operation:
                            if (itemAuthorizationsScopeNode != null && itemAuthorizationsScopeNode.Children.Count >= 3)
                                itemAuthorizationsScopeNode = itemAuthorizationsScopeNode.Children[2];
                            else
                                return;
                            break;
                    }
                    foreach (ItemAuthorizationScopeNode itemAuthorizationScopeNode in itemAuthorizationsScopeNode.Children)
                    {
                        if (oldItemName == itemAuthorizationScopeNode.Item.Name)
                        {
                            itemAuthorizationScopeNode.Item = this.item;
                            itemAuthorizationScopeNode.RenderItemAuthorizationScopeNode();
                            break;
                        }
                    }
                }
                catch { }
                /*Application.DoEvents();*/
            }
        }

        protected override void OnRefresh(MMC.AsyncStatus status)
        {
            status.Title = Globalization.MultilanguageResource.GetString("Refreshing_Msg10");
            base.OnRefresh(status);
            this.Render();
            status.Complete(Globalization.MultilanguageResource.GetString("RefreshComplete_Msg10"), true);
        }
    }
}
