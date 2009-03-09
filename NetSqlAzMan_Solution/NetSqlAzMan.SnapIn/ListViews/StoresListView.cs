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

namespace NetSqlAzMan.SnapIn.ListViews
{
    public class StoresListView : ListViewBase
    {
        public StoresListView()
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
            this.Columns[0].Title = Globalization.MultilanguageResource.GetString("ColumnHeader_StoreName");
            this.Columns[0].SetWidth(200);
            this.Columns.AddRange(
                new MMC.MmcListViewColumn[] {
                    new MMC.MmcListViewColumn(Globalization.MultilanguageResource.GetString("ColumnHeader_Description"),300),
                    new MMC.MmcListViewColumn(Globalization.MultilanguageResource.GetString("ColumnHeader_StoreID"), 100)});
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
            //Multiple delete
            if (this.SelectedNodes.Count > 1)
            {
                //Prepare actions
                this.SelectionData.ActionsPaneItems.Clear();
                this.SelectionData.Update(this.SelectedNodes, true, null, null);
                //Export - MMC.SyncAction
                MMC.SyncAction exportAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_Msg130"), Globalization.MultilanguageResource.GetString("Menu_Tit130"));
                exportAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(exportAction_Triggered);
                this.SelectionData.ActionsPaneItems.Add(exportAction);
                //MMC.SyncAction - Delete Stores
                MMC.SyncAction deleteStoresAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_Msg140"), Globalization.MultilanguageResource.GetString("Menu_Tit140"));
                bool canDelete = true;
                foreach (StoreScopeNode ssn in this.SelectedNodes)
                {
                    if (!ssn.Store.IAmAdmin)
                    {
                        canDelete = false;
                        break;
                    }
                }
                deleteStoresAction.Enabled = canDelete;
                deleteStoresAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(deleteStoresAction_Triggered);
                this.SelectionData.ActionsPaneItems.Add(deleteStoresAction);
            }
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
                    objectsToExport[i] = ((StoreScopeNode)this.SelectedNodes[i]).Store;
                }
                frmwait.ShowDialog(e, frm.fileName, objectsToExport, frm.includeSecurityObjects, frm.includeDBUsers, frm.includeAuthorizations, ((StoreScopeNode)this.SelectedNodes[0]).Store.Storage);
            }
        }

        void deleteStoresAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            MessageBoxParameters mbp = new MessageBoxParameters();
            mbp.Buttons = MessageBoxButtons.YesNo;
            mbp.Caption = e.Action.DisplayName;
            mbp.DefaultButton = MessageBoxDefaultButton.Button2;
            mbp.Icon = MessageBoxIcon.Question;
            mbp.Text = String.Format(Globalization.MultilanguageResource.GetString("StoresListView_Msg10"));
            DialogResult dr = this.SnapIn.Console.ShowDialog(mbp);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    foreach (StoreScopeNode storeScopeNode in this.SelectedNodes)
                    {
                        storeScopeNode.Store.Delete();
                        this.ScopeNode.Children.Remove(storeScopeNode);
                    }
                }
                catch (Exception ex)
                {
                    this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("Menu_Tit150"));
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
