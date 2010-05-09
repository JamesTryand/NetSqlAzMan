using System;
using System.Windows.Forms;
using Microsoft.ManagementConsole.Advanced;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.SnapIn.Forms;
using NetSqlAzMan.SnapIn.ScopeNodes;
using MMC = Microsoft.ManagementConsole;

namespace NetSqlAzMan.SnapIn.ListViews
{
    public class ApplicationGroupsListView : ListViewBase
    {
        public ApplicationGroupsListView()
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
            this.Columns[0].Title = Globalization.MultilanguageResource.GetString("ColumnHeader_ApplicationGroupName");
            this.Columns[0].SetWidth(200);
            this.Columns.AddRange(
                new MMC.MmcListViewColumn[] {
                    new MMC.MmcListViewColumn(Globalization.MultilanguageResource.GetString("ColumnHeader_Description"), 300),
                    new MMC.MmcListViewColumn(Globalization.MultilanguageResource.GetString("ColumnHeader_GroupType"), 100),
                    new MMC.MmcListViewColumn(Globalization.MultilanguageResource.GetString("ColumnHeader_ObjectSID"),300)});
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
                MMC.SyncAction exportAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_Msg10"), Globalization.MultilanguageResource.GetString("Menu_Tit10"));
                exportAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(exportAction_Triggered);
                this.SelectionData.ActionsPaneItems.Add(exportAction);
                //MMC.SyncAction - Delete Stores
                MMC.SyncAction deleteApplicationGroupsAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_Msg20"), Globalization.MultilanguageResource.GetString("Menu_Tit20"));
                if (!(((ApplicationGroupScopeNode)this.SelectedNodes[0]).ApplicationGroup.Application.IAmManager))
                    deleteApplicationGroupsAction.Enabled = false;
                deleteApplicationGroupsAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(deleteApplicationGroupsAction_Triggered);
                this.SelectionData.ActionsPaneItems.Add(deleteApplicationGroupsAction);
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
                for (int i=0;i<this.SelectedNodes.Count;i++)
                {
                    objectsToExport[i] = ((ApplicationGroupScopeNode)this.SelectedNodes[i]).ApplicationGroup;
                }
                frmwait.ShowDialog(e, frm.fileName, objectsToExport, frm.includeSecurityObjects, frm.includeDBUsers, frm.includeAuthorizations, ((ApplicationGroupScopeNode)this.SelectedNodes[0]).ApplicationGroup.Application.Store.Storage);
            }
        }

        void deleteApplicationGroupsAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            MessageBoxParameters mbp = new MessageBoxParameters();
            mbp.Buttons = MessageBoxButtons.YesNo;
            mbp.Caption = e.Action.DisplayName;
            mbp.DefaultButton = MessageBoxDefaultButton.Button2;
            mbp.Icon = MessageBoxIcon.Question;
            mbp.Text = String.Format(Globalization.MultilanguageResource.GetString("ApplicationGroupsListView_Msg10"));
            DialogResult dr = this.SnapIn.Console.ShowDialog(mbp);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    foreach (ApplicationGroupScopeNode applicationGroupScopeNode in this.SelectedNodes)
                    {
                        applicationGroupScopeNode.ApplicationGroup.Delete();
                        this.ScopeNode.Children.Remove(applicationGroupScopeNode);
                    }
                }
                catch (Exception ex)
                {
                    this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("ApplicationGroupsListView_Msg20"));
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
