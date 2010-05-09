using System.Collections.Generic;
using System.Windows.Forms;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.SnapIn.Forms;
using NetSqlAzMan.SnapIn.ListViews;
using MMC = Microsoft.ManagementConsole;

namespace NetSqlAzMan.SnapIn.ScopeNodes
{
    public class ApplicationGroupsScopeNode : ScopeNodeBase
    {
        protected IAzManApplication application;

        public ApplicationGroupsScopeNode(IAzManApplication application) : base(!application.HasApplicationGroups())
        {
            this.application = application;
            // Create a message view for the Application Groups node.
            MMC.MmcListViewDescription lvdApplicationGroups = new MMC.MmcListViewDescription();
            lvdApplicationGroups.DisplayName = Globalization.MultilanguageResource.GetString("ListView_Msg20");
            lvdApplicationGroups.ViewType = typeof(ApplicationGroupsListView);
            lvdApplicationGroups.Options = MMC.MmcListViewOptions.AllowUserInitiatedModeChanges;
            this.ViewDescriptions.Clear();
            this.ViewDescriptions.Add(lvdApplicationGroups);
            this.ViewDescriptions.DefaultIndex = 0;
            this.RenderApplicationGroups();
        }

        protected override void OnExpand(MMC.AsyncStatus status)
        {
            status.ReportProgress(50, 100, Globalization.MultilanguageResource.GetString("Expanding_Msg10"));
            this.Render();
            status.Complete("Done.", true);
            base.OnExpand(status);
        }

        protected override bool OnExpandFromLoad(MMC.SyncStatus status)
        {
            status.ReportProgress(50, 100, Globalization.MultilanguageResource.GetString("Expanding_Msg10"));
            this.Render();
            status.Complete(Globalization.MultilanguageResource.GetString("Done_Msg10"), true);
            return base.OnExpandFromLoad(status);
        }

        public IAzManApplication Application
        {
            get
            {
                return this.application;
            }
        }

        protected void RenderApplicationGroups()
        {
        //Prepare Node
            this.DisplayName = Globalization.MultilanguageResource.GetString("ListView_Msg20");
            this.SubItemDisplayNames.Clear();
            this.SubItemDisplayNames.Add(Globalization.MultilanguageResource.GetString("Folder_Msg10"));
            this.ImageIndex = ImageIndexes.ApplicationGroupsImgIdx;
            this.SelectedImageIndex = ImageIndexes.ApplicationGroupsImgIdx;
            //Assign Tag
            this.Tag = application;
            //Enable standard verbs
            this.EnabledStandardVerbs = MMC.StandardVerbs.Refresh;
            //Add custom actions
            this.ActionsPaneItems.Clear();
            //New Application Group - MMC.SyncAction
            MMC.SyncAction newApplicationGroupAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_Msg210"), Globalization.MultilanguageResource.GetString("Menu_Tit210"));
            if (!this.application.IAmManager)
                newApplicationGroupAction.Enabled = false;
            newApplicationGroupAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(newApplicationGroupAction_Triggered);
            this.ActionsPaneItems.Add(newApplicationGroupAction);
            //Line MMC.SyncAction
            MMC.ActionSeparator lineAction1 = new MMC.ActionSeparator();
            this.ActionsPaneItems.Add(lineAction1);
            //Import - MMC.SyncAction
            MMC.SyncAction importAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_Msg220"), Globalization.MultilanguageResource.GetString("Menu_Tit220"));
            if (!this.application.IAmManager)
                importAction.Enabled = false;
            importAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(importAction_Triggered);
            this.ActionsPaneItems.Add(importAction);
             /*System.Windows.Forms.Application.DoEvents();*/
        }
        
        protected override void Render()
        {
            //Prepare Node
            this.RenderApplicationGroups();
            //Children
            this.Children.Clear();
            IAzManApplicationGroup[] applicationGroups = this.application.GetApplicationGroups();
            List<ApplicationGroupScopeNode> list = new List<ApplicationGroupScopeNode>();
            for (int i = 0; i < applicationGroups.Length; i++)
            {
                list.Add(new ApplicationGroupScopeNode(applicationGroups[i]));
            }
            this.Children.AddRange(list.ToArray());
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

        private void newApplicationGroupAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            frmNewApplicationGroup frm = new frmNewApplicationGroup();
            frm.application = this.application;
            DialogResult dr = this.SnapIn.Console.ShowDialog(frm);
            if (dr == DialogResult.OK)
            {
                this.Children.Add(new ApplicationGroupScopeNode(frm.applicationGroup));
            }
        }
    }
}
