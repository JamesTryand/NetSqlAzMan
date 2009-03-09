using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.SnapIn.DirectoryServices;
using NetSqlAzMan;
using NetSqlAzMan.SnapIn.ListViews;
using MMC = Microsoft.ManagementConsole;
using Microsoft.ManagementConsole.Advanced;
using NetSqlAzMan.SnapIn.Forms;

namespace NetSqlAzMan.SnapIn.ScopeNodes
{
    public class OperationDefinitionsScopeNode : ScopeNodeBase
    {
        protected IAzManApplication application;

        public OperationDefinitionsScopeNode(IAzManApplication application)
            : base(!application.HasItems(ItemType.Operation))
        {
            this.application = application;
            // Create a message view for the Store Groups node.
            MMC.MmcListViewDescription lvdOperations = new MMC.MmcListViewDescription();
            lvdOperations.DisplayName = Globalization.MultilanguageResource.GetString("Folder_Msg50");
            lvdOperations.ViewType = typeof(ItemDefinitionsListView);
            lvdOperations.Options = MMC.MmcListViewOptions.AllowUserInitiatedModeChanges;
            this.ViewDescriptions.Clear();
            this.ViewDescriptions.Add(lvdOperations);
            this.ViewDescriptions.DefaultIndex = 0;
            this.RenderOperationDefinitions();
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

        protected void RenderOperationDefinitions()
        {
            //Prepare Node
            this.DisplayName = Globalization.MultilanguageResource.GetString("Folder_Msg50");
            this.SubItemDisplayNames.Clear();
            this.SubItemDisplayNames.Add(Globalization.MultilanguageResource.GetString("Folder_Tit50"));
            this.ImageIndex = ImageIndexes.ItemsImgIdx;
            this.SelectedImageIndex = ImageIndexes.ItemsImgIdx;
            //Assign Tag
            this.Tag = application;
            //Enable standard verbs
            this.EnabledStandardVerbs = MMC.StandardVerbs.Refresh;
            //Add custom actions
            this.ActionsPaneItems.Clear();
            //New Item - MMC.SyncAction
            MMC.SyncAction newOperationAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_Msg320"), Globalization.MultilanguageResource.GetString("Menu_Tit320"));
            if (!this.application.IAmManager)
                newOperationAction.Enabled = false;
            newOperationAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(newOperationDefinitionAction_Triggered);
            this.ActionsPaneItems.Add(newOperationAction);
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

        protected override void  Render()
        {
            //Prepare Node
            this.RenderOperationDefinitions();
            //Children
            this.Children.Clear();
            IAzManItem[] itemDefinitions = this.application.GetItems(ItemType.Operation);
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

        void newOperationDefinitionAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            MMC.ScopeNode itemNode = (MMC.ScopeNode)sender;
            frmItemProperties frm = new frmItemProperties();
            frm.application = this.application;
            frm.item = null;
            frm.itemType = ItemType.Operation;
            DialogResult dr = this.SnapIn.Console.ShowDialog(frm);
            if (dr == DialogResult.OK)
            {
                this.Children.Add(new ItemDefinitionScopeNode(frm.item));
                //Add relative child in Item Authorizations if opened
                try
                {
                    if (
                        this.Parent != null
                        &&
                        this.Parent.Parent != null
                        &&
                        this.Parent.Parent.Children.Count >= 3
                        &&
                        this.Parent.Parent.Children[2].Children.Count >= 3
                        )
                    {
                        MMC.ScopeNode itemDefinitionsScopeNode = this.Parent;
                        OperationAuthorizationsScopeNode itemAuthorizationsScopeNode = (itemDefinitionsScopeNode.Parent.Children[2].Children[2]) as OperationAuthorizationsScopeNode;
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
