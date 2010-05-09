using System.Collections.Generic;
using System.Windows.Forms;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.SnapIn.Forms;
using NetSqlAzMan.SnapIn.ListViews;
using MMC = Microsoft.ManagementConsole;

namespace NetSqlAzMan.SnapIn.ScopeNodes
{
    public class RoleDefinitionsScopeNode : ScopeNodeBase
    {
        protected IAzManApplication application;

        public RoleDefinitionsScopeNode(IAzManApplication application)
            : base(!application.HasItems(ItemType.Role))
        {
            this.application = application;
            // Create a message view for the Store Groups node.
            MMC.MmcListViewDescription lvdRoles = new MMC.MmcListViewDescription();
            lvdRoles.DisplayName = Globalization.MultilanguageResource.GetString("Folder_Msg70");
            lvdRoles.ViewType = typeof(ItemDefinitionsListView);
            lvdRoles.Options = MMC.MmcListViewOptions.AllowUserInitiatedModeChanges;
            this.ViewDescriptions.Clear();
            this.ViewDescriptions.Add(lvdRoles);
            this.ViewDescriptions.DefaultIndex = 0;
            this.RenderRoleDefinitions();
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

        protected void RenderRoleDefinitions()
        {
            //Prepare Node
            this.DisplayName = Globalization.MultilanguageResource.GetString("Folder_Msg70");
            this.SubItemDisplayNames.Clear();
            this.SubItemDisplayNames.Add(Globalization.MultilanguageResource.GetString("Folder_Tit70"));
            this.ImageIndex = ImageIndexes.ItemsImgIdx;
            this.SelectedImageIndex = ImageIndexes.ItemsImgIdx;
            //Assign Tag
            this.Tag = application;
            //Enable standard verbs
            this.EnabledStandardVerbs = MMC.StandardVerbs.Refresh;
            //Add custom actions
            this.ActionsPaneItems.Clear();
            //New Item - MMC.SyncAction
            MMC.SyncAction newRoleAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_Msg330"), Globalization.MultilanguageResource.GetString("Menu_Tit330"));
            if (!this.application.IAmManager)
                newRoleAction.Enabled = false;
            newRoleAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(newRoleDefinitionAction_Triggered);
            this.ActionsPaneItems.Add(newRoleAction);
        }

        public IAzManApplication Application
        {
            get
            {
                return this.application;
            }
        }

        internal void internalRender()
        {
            this.Render();
        }

        protected override void Render()
        {
            //Prepare Node
            this.RenderRoleDefinitions();
            //Children
            this.Children.Clear();
            IAzManItem[] itemDefinitions = this.application.GetItems(ItemType.Role);
            List<ItemDefinitionScopeNode> list = new List<ItemDefinitionScopeNode>();
            for (int i = 0; i < itemDefinitions.Length; i++)
            {
                list.Add(new ItemDefinitionScopeNode(itemDefinitions[i]));
            }
            this.Children.AddRange(list.ToArray());
            /*System.Windows.Forms.Application.DoEvents();*/
        }
        

        protected override void OnRefresh(MMC.AsyncStatus status)
        {
            status.Title = Globalization.MultilanguageResource.GetString("Refreshing_Msg10");
            base.OnRefresh(status);
            this.Render();
            status.Complete(Globalization.MultilanguageResource.GetString("RefreshComplete_Msg10"), true);
        }

        void newRoleDefinitionAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            MMC.ScopeNode itemNode = (MMC.ScopeNode)sender;
            frmItemProperties frm = new frmItemProperties();
            frm.application = this.application;
            frm.item = null;
            frm.itemType = ItemType.Role;
            DialogResult dr = this.SnapIn.Console.ShowDialog(frm);
            if (dr == DialogResult.OK)
            {
                this.Children.Add(new ItemDefinitionScopeNode(frm.item));
                //Add relative child in Item Authorizations if Opened
                try
                {
                    if (
                        this.Parent != null
                        &&
                        this.Parent.Parent != null
                        &&
                        this.Parent.Parent.Children.Count >= 3
                        &&
                        this.Parent.Parent.Children[2].Children.Count >= 1)
                    {
                        MMC.ScopeNode itemDefinitionsScopeNode = this.Parent;
                        RoleAuthorizationsScopeNode itemAuthorizationsScopeNode = (itemDefinitionsScopeNode.Parent.Children[2].Children[0]) as RoleAuthorizationsScopeNode;
                        if (itemAuthorizationsScopeNode!=null)
                            itemAuthorizationsScopeNode.Children.Add(new ItemAuthorizationScopeNode(frm.item));
                    }
                }
                catch { }
                /*System.Windows.Forms.Application.DoEvents();*/
            }
        }
    }
}
