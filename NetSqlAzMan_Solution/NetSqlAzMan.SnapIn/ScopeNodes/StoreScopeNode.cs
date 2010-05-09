using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.ManagementConsole.Advanced;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.SnapIn.Forms;
using NetSqlAzMan.SnapIn.ListViews;
using NetSqlAzMan.SnapIn.Printing;
using MMC = Microsoft.ManagementConsole;

namespace NetSqlAzMan.SnapIn.ScopeNodes
{
    public class StoreScopeNode : ScopeNodeBase
    {
        protected IAzManStore store;

        public StoreScopeNode(IAzManStore store)
        {
            //Prepare Node
            this.store = store;
            // Create a message view for the Store node.
            MMC.MmcListViewDescription lvdItems = new MMC.MmcListViewDescription();
            lvdItems.DisplayName = Globalization.MultilanguageResource.GetString("Folder_Msg100");
            lvdItems.ViewType = typeof(StoreGroupsAndApplicationsListView);
            lvdItems.Options = MMC.MmcListViewOptions.AllowUserInitiatedModeChanges;
            this.ViewDescriptions.Clear();
            this.ViewDescriptions.Add(lvdItems);
            this.ViewDescriptions.DefaultIndex = 0;
            this.Children.Clear();
            this.RenderStoreScopeNode();
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

        public IAzManStore Store
        {
            get
            {
                return this.store;
            }
        }

        protected void RenderStoreScopeNode()
        {
            //Prepare node
            string fixedserverrole;
            if (this.store.IAmAdmin) fixedserverrole = "Admin";
            else if (this.store.IAmManager) fixedserverrole = "Manager";
            else if (this.store.IAmUser) fixedserverrole = "User";
            else fixedserverrole = "Reader";
            this.DisplayName = String.Format("{0} ({1})", this.store.Name, fixedserverrole);
            this.SubItemDisplayNames.Clear();
            this.SubItemDisplayNames.AddRange(
                new string[] {
                                store.Description,
                                store.StoreId.ToString()});
            this.ImageIndex = ImageIndexes.StoreImgIdx;
            this.SelectedImageIndex = ImageIndexes.StoreImgIdx;
            //Assign Tag
            this.Tag = store;
            //Enable standard verbs
            if (this.store.IAmAdmin)
                this.EnabledStandardVerbs = MMC.StandardVerbs.Refresh | MMC.StandardVerbs.Delete;
            else
                this.EnabledStandardVerbs = MMC.StandardVerbs.Refresh;
            //Add custom actions
            //Store Properties - MMC.SyncAction
            this.ActionsPaneItems.Clear();
            MMC.SyncAction storePropertiesAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_Msg470"), Globalization.MultilanguageResource.GetString("Menu_Tit470"));
            this.ActionsPaneItems.Add(storePropertiesAction);
            storePropertiesAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(storePropertiesAction_Triggered);
            //Line MMC.SyncAction
            MMC.ActionSeparator lineAction2 = new MMC.ActionSeparator();
            this.ActionsPaneItems.Add(lineAction2);
            //Create New Application - MMC.SyncAction
            MMC.SyncAction createNewApplicationAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_Msg480"), Globalization.MultilanguageResource.GetString("Menu_Tit480"));
            if (!this.store.IAmManager)
                createNewApplicationAction.Enabled = false;
            createNewApplicationAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(newApplicationAction_Triggered);
            this.ActionsPaneItems.Add(createNewApplicationAction);
            //Line MMC.SyncAction
            MMC.ActionSeparator lineAction3 = new MMC.ActionSeparator();
            this.ActionsPaneItems.Add(lineAction3);
            //Items Hierarchy View - MMC.SyncAction
            MMC.SyncAction ItemsHVAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_ItemsHierarchicalView"), Globalization.MultilanguageResource.GetString("Menu_ItemsHierarchicalViewDescription"));
            ItemsHVAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(ItemsHVAction_Triggered);
            this.ActionsPaneItems.Add(ItemsHVAction);
            //Line MMC.SyncAction
            MMC.ActionSeparator lineAction4 = new MMC.ActionSeparator();
            this.ActionsPaneItems.Add(lineAction4);
            //Report Group
            MMC.ActionGroup reportAction = new MMC.ActionGroup(Globalization.MultilanguageResource.GetString("rptTitle"), Globalization.MultilanguageResource.GetString("rptDescription"));
            this.ActionsPaneItems.Add(reportAction);
            //Items Hierarchy Report - MMC.SyncAction
            MMC.SyncAction ItemsHReportAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("rptMsg10"), Globalization.MultilanguageResource.GetString("rptTit10"));
            ItemsHReportAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(ItemsHReportAction_Triggered);
            reportAction.Items.Add(ItemsHReportAction);
            //Authorizations Report - MMC.SyncAction
            MMC.SyncAction AuthorizationsReportAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("rptMsg20"), Globalization.MultilanguageResource.GetString("rptTit20"));
            AuthorizationsReportAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(AuthorizationsReportAction_Triggered);
            reportAction.Items.Add(AuthorizationsReportAction);
            //Effective Permissions Report - MMC.SyncAction
            MMC.SyncAction EffectivePermissionsReportAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("rptMsg30"), Globalization.MultilanguageResource.GetString("rptTit30"));
            EffectivePermissionsReportAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(EffectivePermissionsReportAction_Triggered);
            reportAction.Items.Add(EffectivePermissionsReportAction);
            //Line MMC.SyncAction
            MMC.ActionSeparator lineAction5 = new MMC.ActionSeparator();
            this.ActionsPaneItems.Add(lineAction5);
            //Import - MMC.SyncAction
            MMC.SyncAction importAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_Msg490"), Globalization.MultilanguageResource.GetString("Menu_Tit490"));
            if (!this.store.IAmManager)
                importAction.Enabled = false;
            importAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(importAction_Triggered);
            this.ActionsPaneItems.Add(importAction);
            //Export - MMC.SyncAction
            MMC.SyncAction exportAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_Msg500"), Globalization.MultilanguageResource.GetString("Menu_Tit500"));
            exportAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(exportAction_Triggered);
            this.ActionsPaneItems.Add(exportAction);
        }

        void ItemsHReportAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            frmPrint frm = new frmPrint();
            ptItemsHierarchy rep = new ptItemsHierarchy();
            rep.Applications = this.store.GetApplications();
            frm.document = rep;
            this.SnapIn.Console.ShowDialog(frm);
        }

        void AuthorizationsReportAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            frmPrint frm = new frmPrint();
            ptItemAuthorizations rep = new ptItemAuthorizations();
            rep.Applications = this.store.GetApplications();
            frm.document = rep;
            this.SnapIn.Console.ShowDialog(frm);
        }

        void EffectivePermissionsReportAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            frmPrint frm = new frmPrint();
            ptEffectivePermissions rep = new ptEffectivePermissions();
            rep.Applications = this.store.GetApplications();
            frm.document = rep;
            this.SnapIn.Console.ShowDialog(frm);
        }
        void ItemsHVAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            frmItemsHierarchyView frm = new frmItemsHierarchyView();
            frm.applications = this.store.GetApplications();
            this.SnapIn.Console.ShowDialog(frm);
        }

        protected override void Render()
        {
            //Prepare Node
            this.RenderStoreScopeNode();
            //Children
            this.Children.Clear();
            //Adding fixed children
            this.Children.Add(new StoreGroupsScopeNode(this.store));
            IAzManApplication[] applications = this.store.GetApplications();
            List<ApplicationScopeNode> list = new List<ApplicationScopeNode>();
            for (int i = 0; i < applications.Length; i++)
            {
                list.Add(new ApplicationScopeNode(applications[i]));
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
                frm.importIntoObject = this.store;
                frm.fileName = openFileDialog.FileName;
                this.SnapIn.Console.ShowDialog(frm);
                this.Render();
            }
        }

        void exportAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            frmExportOptions frm = new frmExportOptions();
            DialogResult dr = this.SnapIn.Console.ShowDialog(frm);
            if (dr == DialogResult.OK)
            {
                frmExport frmwait = new frmExport();
                frmwait.ShowDialog(e, frm.fileName, new IAzManExport[] { this.store }, frm.includeSecurityObjects, frm.includeDBUsers, frm.includeAuthorizations, this.store.Storage);
            }
        }

        private void storePropertiesAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            frmStoreProperties frm = new frmStoreProperties();
            frm.Text += " - " + this.store.Name;
            frm.store = this.store;
            frm.storage = this.store.Storage;

            DialogResult dr = this.SnapIn.Console.ShowDialog(frm);
            if (dr == DialogResult.OK)
            {
                this.RenderStoreScopeNode();
            }
        }

        protected override void OnDelete(MMC.SyncStatus status)
        {
            MessageBoxParameters msg = new MessageBoxParameters();
            msg.Caption = Globalization.MultilanguageResource.GetString("Menu_Msg510");
            msg.Text = String.Format(Globalization.MultilanguageResource.GetString("Menu_Msg520")+"\r\n'{0}'", this.store.Name);
            msg.Icon = MessageBoxIcon.Question;
            msg.Buttons = MessageBoxButtons.YesNo;
            msg.DefaultButton = MessageBoxDefaultButton.Button2;
            DialogResult dr = this.SnapIn.Console.ShowDialog(msg);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    this.store.Delete();
                    this.Parent.Children.Remove(this);
                }
                catch (Exception ex)
                {
                    this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("StoreScopeNode_Msg10"));
                }
            }
        }

        private void newApplicationAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            frmApplicationProperties frm = new frmApplicationProperties();
            frm.store = this.store;
            DialogResult dr = this.SnapIn.Console.ShowDialog(frm);
            if (dr == DialogResult.OK)
            {
                this.Children.Add(new ApplicationScopeNode(frm.application));
            }
        }
    }
}
