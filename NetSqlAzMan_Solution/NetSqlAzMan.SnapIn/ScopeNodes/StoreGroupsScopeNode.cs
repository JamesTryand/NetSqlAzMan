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
using MMC = Microsoft.ManagementConsole;
using Microsoft.ManagementConsole.Advanced;
using NetSqlAzMan.SnapIn.ListViews;
using NetSqlAzMan.SnapIn.Forms;

namespace NetSqlAzMan.SnapIn.ScopeNodes
{
    public class StoreGroupsScopeNode : ScopeNodeBase
    {
        protected IAzManStore store;

        public StoreGroupsScopeNode(IAzManStore store) : base(!store.HasStoreGroups())
        {
            this.store = store;
            // Create a message view for the Store Groups node.
            MMC.MmcListViewDescription lvdStoreGroups = new MMC.MmcListViewDescription();
            lvdStoreGroups.DisplayName = Globalization.MultilanguageResource.GetString("Folder_Msg90");
            lvdStoreGroups.ViewType = typeof(StoreGroupsListView);
            lvdStoreGroups.Options = MMC.MmcListViewOptions.AllowUserInitiatedModeChanges;
            this.ViewDescriptions.Clear();
            this.ViewDescriptions.Add(lvdStoreGroups);
            this.ViewDescriptions.DefaultIndex = 0;
            this.RenderStoreGroups();
        }

        protected override void OnExpand(MMC.AsyncStatus status)
        {
            status.ReportProgress(50, 100, Globalization.MultilanguageResource.GetString("Refreshing_Msg10"));
            this.Render();
            status.Complete(Globalization.MultilanguageResource.GetString("Done_Msg10"), true);
            base.OnExpand(status);
        }

        protected override bool OnExpandFromLoad(MMC.SyncStatus status)
        {
            status.ReportProgress(50, 100, Globalization.MultilanguageResource.GetString("Refreshing_Msg10"));
            this.Render();
            status.Complete(Globalization.MultilanguageResource.GetString("Done_Msg10"), true);
            return base.OnExpandFromLoad(status);
        }

        public IAzManStore Store
        {
            get
            {
                return this.store;
            }
        }

        protected void RenderStoreGroups()
        {
            //Prepare Node
            this.DisplayName = Globalization.MultilanguageResource.GetString("Folder_Msg90");
            this.SubItemDisplayNames.Clear();
            this.SubItemDisplayNames.Add(Globalization.MultilanguageResource.GetString("Folder_Tit90"));
            this.ImageIndex = ImageIndexes.StoreGroupsImgIdx;
            this.SelectedImageIndex = ImageIndexes.StoreGroupsImgIdx;
            //Assign Tag
            this.Tag = store;
            //Enable standard verbs
            this.EnabledStandardVerbs = MMC.StandardVerbs.Refresh;
            //Add custom actions
            this.ActionsPaneItems.Clear();
            //New Store Group - MMC.SyncAction
            MMC.SyncAction newStoreGroupAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_Msg440"), Globalization.MultilanguageResource.GetString("Menu_Tit440"));
            if (!this.store.IAmManager)
                newStoreGroupAction.Enabled = false;
            newStoreGroupAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(newStoreGroupAction_Triggered);
            this.ActionsPaneItems.Add(newStoreGroupAction);
            //Line MMC.SyncAction
            MMC.ActionSeparator lineAction1 = new MMC.ActionSeparator();
            this.ActionsPaneItems.Add(lineAction1);
            //Import - MMC.SyncAction
            MMC.SyncAction importAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_Msg450"), Globalization.MultilanguageResource.GetString("Menu_Msg460"));
            if (!this.store.IAmManager)
                importAction.Enabled = false;
            importAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(importAction_Triggered);
            this.ActionsPaneItems.Add(importAction);
        }

        protected override void Render()
        {
            //Prepare node
            this.RenderStoreGroups();
            //Children
            this.Children.Clear();
            IAzManStoreGroup[] storeGroups = this.store.GetStoreGroups();
            List<StoreGroupScopeNode> list = new List<StoreGroupScopeNode>();
            for (int i = 0; i < storeGroups.Length; i++)
            {
                list.Add(new StoreGroupScopeNode(storeGroups[i]));
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

        void importAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = "xml";
            openFileDialog.FileName = "NetSqlAzMan.xml";
            openFileDialog.Filter = "Xml files|*.xml|All files|*.*";
            openFileDialog.SupportMultiDottedExtensions = true;
            openFileDialog.Title = Globalization.MultilanguageResource.GetString("ApplicationGroupsScopeNode_Msg10");
            DialogResult dr = this.SnapIn.Console.ShowDialog(openFileDialog);
            if (dr == DialogResult.OK)
            {
                frmImportOptions frm = new frmImportOptions();
                frm.importIntoObject = this;
                frm.fileName = openFileDialog.FileName;
                this.SnapIn.Console.ShowDialog(frm);
                this.Render();
            }
        }

        private void newStoreGroupAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            frmNewStoreGroup frm = new frmNewStoreGroup();
            frm.store = this.store;
            DialogResult dr = this.SnapIn.Console.ShowDialog(frm);
            if (dr == DialogResult.OK)
            {
                this.Children.Add(new StoreGroupScopeNode(frm.storeGroup));
            }
        }
    }
}
