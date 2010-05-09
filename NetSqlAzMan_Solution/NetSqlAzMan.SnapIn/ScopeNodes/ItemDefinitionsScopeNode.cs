using System.Windows.Forms;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.SnapIn.Forms;
using NetSqlAzMan.SnapIn.ListViews;
using MMC = Microsoft.ManagementConsole;

namespace NetSqlAzMan.SnapIn.ScopeNodes
{
    public class ItemDefinitionsScopeNode : ScopeNodeBase
    {
        protected IAzManApplication application;

        public ItemDefinitionsScopeNode(IAzManApplication application) : base()
        {
            this.application = application;
            // Create a message view for the Store Groups node.
            MMC.MmcListViewDescription lvdStoreGroups = new MMC.MmcListViewDescription();
            lvdStoreGroups.DisplayName = Globalization.MultilanguageResource.GetString("Folder_Msg30");
            lvdStoreGroups.ViewType = typeof(ItemDefinitionsListView);
            lvdStoreGroups.Options = MMC.MmcListViewOptions.AllowUserInitiatedModeChanges;
            this.ViewDescriptions.Clear();
            this.ViewDescriptions.Add(lvdStoreGroups);
            this.ViewDescriptions.DefaultIndex = 0;
            this.RenderItemDefinitions();
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

        protected void RenderItemDefinitions()
        {
            //Prepare Node
            this.DisplayName = Globalization.MultilanguageResource.GetString("Folder_Msg30");
            this.SubItemDisplayNames.Clear();
            this.SubItemDisplayNames.Add(Globalization.MultilanguageResource.GetString("Folder_Tit30"));
            this.ImageIndex = ImageIndexes.ItemsImgIdx;
            this.SelectedImageIndex = ImageIndexes.ItemsImgIdx;
            //Assign Tag
            this.Tag = application;
            //Enable standard verbs
            this.EnabledStandardVerbs = MMC.StandardVerbs.Refresh;
            //Add custom actions
            this.ActionsPaneItems.Clear();
            //Line MMC.SyncAction
            MMC.ActionSeparator lineAction1 = new MMC.ActionSeparator();
            this.ActionsPaneItems.Add(lineAction1);
            //Import - MMC.SyncAction
            MMC.SyncAction importAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_Msg300"), Globalization.MultilanguageResource.GetString("Menu_Tit300"));
            if (!this.application.IAmManager)
                importAction.Enabled = false;
            importAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(importAction_Triggered);
            this.ActionsPaneItems.Add(importAction);
            //Export - MMC.SyncAction
            MMC.SyncAction exportAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_Msg310"), Globalization.MultilanguageResource.GetString("Menu_Tit310"));
            exportAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(exportAction_Triggered);
            this.ActionsPaneItems.Add(exportAction);
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
            this.RenderItemDefinitions();
            //Children
            this.Children.Clear();
            //Operation Definitions visibile only in Developer Mode.
            if (this.application.Store.Storage.Mode == NetSqlAzManMode.Administrator)
            {
                this.Children.AddRange(
                    new MMC.ScopeNode[] { new RoleDefinitionsScopeNode(this.application), 
                                  new TaskDefinitionsScopeNode(this.application)});
            }
            else
            {
                this.Children.AddRange(
                        new MMC.ScopeNode[] { new RoleDefinitionsScopeNode(this.application), 
                                  new TaskDefinitionsScopeNode(this.application),
                                  new OperationDefinitionsScopeNode(this.application)});
            }
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
                ((ApplicationScopeNode)this.Parent).internalRender();
            }
        }

        void exportAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            frmExportOptions frm = new frmExportOptions();
            DialogResult dr = this.SnapIn.Console.ShowDialog(frm);
            if (dr == DialogResult.OK)
            {
                frmExport frmwait = new frmExport();
                IAzManItem[] itemToExport = this.application.GetItems();
                IAzManExport[] toExport = new IAzManExport[itemToExport.Length];
                for (int i = 0; i < itemToExport.Length; i++)
                {
                    toExport[i] = itemToExport[i];
                }
                frmwait.ShowDialog(e, frm.fileName, toExport, frm.includeSecurityObjects, frm.includeDBUsers, frm.includeAuthorizations, this.application.Store.Storage);
            }
        }
    }
}
