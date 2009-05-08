using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using MMC = Microsoft.ManagementConsole;
using Microsoft.ManagementConsole.Advanced;
using NetSqlAzMan;
using NetSqlAzMan.SnapIn.ScopeNodes;
using NetSqlAzMan.SnapIn.Forms;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.SnapIn.Printing;

namespace NetSqlAzMan.SnapIn.ListViews
{
    public class StoreGroupsAndApplicationsListView : ListViewBase
    {
        public StoreGroupsAndApplicationsListView()
        { 
        
        }

        protected override void OnInitialize(MMC.AsyncStatus status)
        {
            base.OnInitialize(status);
            status.Title = Globalization.MultilanguageResource.GetString("Refreshing_Msg10");
            status.ReportProgress(50, 100, Globalization.MultilanguageResource.GetString("Refreshing_Msg10"));
            /*Application.DoEvents();*/
            base.OnInitialize(status);
            this.Columns.Clear();
            this.Columns[0].Title = Globalization.MultilanguageResource.GetString("ColumnHeader_ApplicationName");
            this.Columns[0].SetWidth(200);
            this.Columns.AddRange(
                new MMC.MmcListViewColumn[] {
                    new MMC.MmcListViewColumn(Globalization.MultilanguageResource.GetString("ColumnHeader_Description"),300),
                    new MMC.MmcListViewColumn(Globalization.MultilanguageResource.GetString("ColumnHeader_ApplicationID"), 100)});
            this.Mode = MMC.MmcListViewMode.Report;
            status.Complete(Globalization.MultilanguageResource.GetString("RefreshComplete_Msg10"), true);
            /*Application.DoEvents();*/
        }

        protected override void Refresh()
        {

        }

        protected override void OnSelectionChanged(MMC.SyncStatus status)
        {
            base.OnSelectionChanged(status);
            if (this.SelectedNodes.Count > 1)
            {
                //Prepare actions
                this.SelectionData.ActionsPaneItems.Clear();
                bool allApplications = true;
                foreach (MMC.ScopeNode scopeNode in this.SelectedNodes)
                {
                    if (scopeNode as ApplicationScopeNode==null)
                    {
                        allApplications = false;
                    }
                }
                if (allApplications)
                {
                    this.SelectionData.Update(this.SelectedNodes, true, null, null);
                    //Items Hierarchy View - MMC.SyncAction
                    MMC.SyncAction ItemsHVAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_ItemsHierarchicalView"), Globalization.MultilanguageResource.GetString("Menu_ItemsHierarchicalViewDescription"));
                    ItemsHVAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(ItemsHVAction_Triggered);
                    this.SelectionData.ActionsPaneItems.Add(ItemsHVAction);
                    //Line MMC.SyncAction
                    MMC.ActionSeparator lineAction1 = new MMC.ActionSeparator();
                    this.SelectionData.ActionsPaneItems.Add(lineAction1);
                    //Report Group
                    MMC.ActionGroup reportAction = new MMC.ActionGroup(Globalization.MultilanguageResource.GetString("rptTitle"), Globalization.MultilanguageResource.GetString("rptDescription"));
                    this.SelectionData.ActionsPaneItems.Add(reportAction);
                    //Items Hierarchy Report - MMC.SyncAction
                    MMC.SyncAction ItemsHReportAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("rptMsg10"), Globalization.MultilanguageResource.GetString("rptTit10"));
                    ItemsHReportAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(ItemsHReportAction_Triggered);
                    reportAction.Items.Add(ItemsHReportAction);
                    //Authorizations Report - MMC.SyncAction
                    MMC.SyncAction AuthorizationsReportAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("rptMsg20"), Globalization.MultilanguageResource.GetString("rptTit20"));
                    AuthorizationsReportAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(AuthorizationsReportAction_Triggered);
                    reportAction.Items.Add(AuthorizationsReportAction);
                    //Effective Permissions Report Report - MMC.SyncAction
                    MMC.SyncAction EffectivePermissionsReportAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("rptMsg30"), Globalization.MultilanguageResource.GetString("rptTit30"));
                    EffectivePermissionsReportAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(EffectivePermissionsReportAction_Triggered);
                    reportAction.Items.Add(EffectivePermissionsReportAction);
                    //Line MMC.SyncAction
                    MMC.ActionSeparator lineAction5 = new MMC.ActionSeparator();
                    this.SelectionData.ActionsPaneItems.Add(lineAction5);
                    //Export - MMC.SyncAction
                    MMC.SyncAction exportAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_Msg80"), Globalization.MultilanguageResource.GetString("Menu_Tit80"));
                    exportAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(exportAction_Triggered);
                    this.SelectionData.ActionsPaneItems.Add(exportAction);
                    //Line MMC.SyncAction
                    MMC.ActionSeparator lineAction2 = new MMC.ActionSeparator();
                    this.SelectionData.ActionsPaneItems.Add(lineAction2);
                    //MMC.SyncAction - Delete Applications
                    MMC.SyncAction deleteApplicationsAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_Msg90"), Globalization.MultilanguageResource.GetString("Menu_Tit90"));
                    if (!(((ApplicationScopeNode)this.SelectedNodes[0]).Application.Store.IAmManager))
                        deleteApplicationsAction.Enabled = false;
                    deleteApplicationsAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(deleteApplicationsAction_Triggered);
                    this.SelectionData.ActionsPaneItems.Add(deleteApplicationsAction);
                }
            }
        }

        void ItemsHReportAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            IAzManApplication[] applications = new IAzManApplication[this.SelectedNodes.Count];
            int index = 0;
            foreach (ApplicationScopeNode applicationScopeNode in this.SelectedNodes)
            {
                applications[index++] = applicationScopeNode.Application;
            }
            frmPrint frm = new frmPrint();
            ptItemsHierarchy rep = new ptItemsHierarchy();
            rep.Applications = applications;
            frm.document = rep;
            this.SnapIn.Console.ShowDialog(frm);
        }

        void AuthorizationsReportAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            IAzManApplication[] applications = new IAzManApplication[this.SelectedNodes.Count];
            int index = 0;
            foreach (ApplicationScopeNode applicationScopeNode in this.SelectedNodes)
            {
                applications[index++] = applicationScopeNode.Application;
            }
            frmPrint frm = new frmPrint();
            ptItemAuthorizations rep = new ptItemAuthorizations();
            rep.Applications = applications;
            frm.document = rep;
            this.SnapIn.Console.ShowDialog(frm);
        }
        void EffectivePermissionsReportAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            IAzManApplication[] applications = new IAzManApplication[this.SelectedNodes.Count];
            int index = 0;
            foreach (ApplicationScopeNode applicationScopeNode in this.SelectedNodes)
            {
                applications[index++] = applicationScopeNode.Application;
            }
            frmPrint frm = new frmPrint();
            ptEffectivePermissions rep = new ptEffectivePermissions();
            rep.Applications = applications;
            frm.document = rep;
            this.SnapIn.Console.ShowDialog(frm);
        }
        void ItemsHVAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            IAzManApplication[] applications = new IAzManApplication[this.SelectedNodes.Count];
            int index = 0;
            foreach (ApplicationScopeNode applicationScopeNode in this.SelectedNodes)
            {
                applications[index++] = applicationScopeNode.Application;
            }
            frmItemsHierarchyView frm = new frmItemsHierarchyView();
            frm.applications = applications;
            this.SnapIn.Console.ShowDialog(frm);
        }

        void exportAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            frmExportOptions frm = new frmExportOptions();
            DialogResult dr = this.SnapIn.Console.ShowDialog(frm);
            if (dr == DialogResult.OK)
            {
                frmExport frmwait = new frmExport();
                IAzManExport[] objectsToExport = new IAzManExport[this.SelectedNodes.Count];
                for (int i = 0; i < this.SelectedNodes.Count; i++)
                {
                    objectsToExport[i] = ((ApplicationScopeNode)this.SelectedNodes[i]).Application;
                }
                frmwait.ShowDialog(e, frm.fileName, objectsToExport, frm.includeSecurityObjects, frm.includeDBUsers, frm.includeAuthorizations, ((ApplicationScopeNode)this.SelectedNodes[0]).Application.Store.Storage);
            }
        }

        void deleteApplicationsAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            MessageBoxParameters mbp = new MessageBoxParameters();
            mbp.Buttons = MessageBoxButtons.YesNo;
            mbp.Caption = e.Action.Description;
            mbp.DefaultButton = MessageBoxDefaultButton.Button2;
            mbp.Icon = MessageBoxIcon.Question;
            mbp.Text = String.Format(Globalization.MultilanguageResource.GetString("Menu_Msg100"));
            DialogResult dr = this.SnapIn.Console.ShowDialog(mbp);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    foreach (ApplicationScopeNode applicationScopeNode in this.SelectedNodes)
                    {
                        applicationScopeNode.Application.Delete();
                        this.ScopeNode.Children.Remove(applicationScopeNode);
                    }
                }
                catch (Exception ex)
                {
                    this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("StoreGroupsAndApplicationsListView_Msg10"));
                }
            }
        }

        protected override void OnRefresh(MMC.AsyncStatus status)
        {
            base.OnRefresh(status);
            status.ReportProgress(50, 100, Globalization.MultilanguageResource.GetString("Refreshing_Msg10"));
            //Children
            this.Refresh();
            status.Complete(Globalization.MultilanguageResource.GetString("RefreshComplete_Msg10"), true);
            /*Application.DoEvents();*/
        }
    }
}
