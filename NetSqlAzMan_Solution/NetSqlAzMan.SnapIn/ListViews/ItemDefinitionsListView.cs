using System;
using System.Windows.Forms;
using Microsoft.ManagementConsole.Advanced;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.SnapIn.Forms;
using NetSqlAzMan.SnapIn.ScopeNodes;
using MMC = Microsoft.ManagementConsole;

namespace NetSqlAzMan.SnapIn.ListViews
{
    public class ItemDefinitionsListView : ListViewBase
    {
        public ItemDefinitionsListView()
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
            this.Columns[0].Title = Globalization.MultilanguageResource.GetString("ColumnHeader_ItemName");
            this.Columns[0].SetWidth(200);
            this.Columns.AddRange(
                new MMC.MmcListViewColumn[] {
                new MMC.MmcListViewColumn(Globalization.MultilanguageResource.GetString("ColumnHeader_Description"),300),
                new MMC.MmcListViewColumn(Globalization.MultilanguageResource.GetString("ColumnHeader_ItemID"), 100)});
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
                if (this.SelectedNodes[0] as RoleDefinitionsScopeNode != null ||
                    this.SelectedNodes[0] as TaskDefinitionsScopeNode != null ||
                    this.SelectedNodes[0] as OperationDefinitionsScopeNode != null ||
                    this.SelectedNodes[0] as RoleAuthorizationsScopeNode != null ||
                    this.SelectedNodes[0] as TaskAuthorizationsScopeNode != null ||
                    this.SelectedNodes[0] as OperationAuthorizationsScopeNode != null)
                {
                    return;
                }
                else
                {
                    //Export - MMC.SyncAction
                    MMC.SyncAction exportAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_Msg60"), Globalization.MultilanguageResource.GetString("Menu_Tit60"));
                    exportAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(exportAction_Triggered);
                    this.SelectionData.ActionsPaneItems.Add(exportAction);
                    //MMC.SyncAction - Delete Stores
                    MMC.SyncAction deleteItemDefinitionsAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_Msg70"), Globalization.MultilanguageResource.GetString("Menu_Tit70"));

                    if (this.SelectedNodes[0] as ItemDefinitionScopeNode != null && !(((ItemDefinitionScopeNode)this.SelectedNodes[0]).Item.Application.IAmManager) ||
                        this.SelectedNodes[0] as ItemAuthorizationScopeNode != null && !(((ItemAuthorizationScopeNode)this.SelectedNodes[0]).Item.Application.IAmManager))
                        deleteItemDefinitionsAction.Enabled = false;
                    deleteItemDefinitionsAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(deleteItemDefinitionsAction_Triggered);
                    this.SelectionData.ActionsPaneItems.Add(deleteItemDefinitionsAction);
                }
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
                    objectsToExport[i] = ((ItemDefinitionScopeNode)this.SelectedNodes[i]).Item;
                }
                frmwait.ShowDialog(e, frm.fileName, objectsToExport, frm.includeSecurityObjects, frm.includeDBUsers, frm.includeAuthorizations, ((ItemDefinitionScopeNode)this.SelectedNodes[0]).Item.Application.Store.Storage);
            }
        }


        void deleteItemDefinitionsAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            MessageBoxParameters mbp = new MessageBoxParameters();
            mbp.Buttons = MessageBoxButtons.YesNo;
            mbp.Caption = e.Action.DisplayName;
            mbp.DefaultButton = MessageBoxDefaultButton.Button2;
            mbp.Icon = MessageBoxIcon.Question;
            mbp.Text = Globalization.MultilanguageResource.GetString("ItemDefinitionsListView_Msg10");
            DialogResult dr = this.SnapIn.Console.ShowDialog(mbp);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    foreach (ItemDefinitionScopeNode itemDefinitionScopeNode in this.SelectedNodes)
                    {
                        itemDefinitionScopeNode.Item.Delete();
                        //Remove relative child in Item Authorizations
                        MMC.ScopeNode itemDefinitionsScopeNode = itemDefinitionScopeNode.Parent;
                        while (itemDefinitionsScopeNode as ItemDefinitionsScopeNode == null)
                        {
                            itemDefinitionsScopeNode = itemDefinitionsScopeNode.Parent; //go up one level
                        }
                        ItemAuthorizationsScopeNode itemAuthorizationsScopeNode = (ItemAuthorizationsScopeNode)itemDefinitionsScopeNode.Parent.Children[2];
                        foreach (MMC.ScopeNode itemAuthorizationsScopeNodeChild in itemAuthorizationsScopeNode.Children)
                        {
                            bool founded = false;
                            foreach (ItemAuthorizationScopeNode itemAuthorizationScopeNode in itemAuthorizationsScopeNodeChild.Children)
                            {
                                if (itemDefinitionScopeNode.Item.Name == itemAuthorizationScopeNode.Item.Name)
                                {
                                    itemAuthorizationScopeNode.Parent.Children.Remove(itemAuthorizationScopeNode);
                                    founded = true;
                                    break;
                                }
                            }
                            if (founded) break;
                        }
                        this.ScopeNode.Children.Remove(itemDefinitionScopeNode);
                    }
                }
                catch (Exception ex)
                {
                    this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("ItemDefinitionsListView_Tit20"));
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
