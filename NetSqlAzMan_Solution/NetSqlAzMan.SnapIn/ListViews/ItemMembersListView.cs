using System.Collections.Generic;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.SnapIn.ScopeNodes;
using MMC = Microsoft.ManagementConsole;

namespace NetSqlAzMan.SnapIn.ListViews
{
    public class ItemMembersListView : ListViewBase
    {
        public ItemMembersListView()
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
            this.Columns[0].Title = Globalization.MultilanguageResource.GetString("ColumnHeader_MemberName");
            this.Columns[0].SetWidth(200);
            this.Columns.AddRange(
                new MMC.MmcListViewColumn[] {
                new MMC.MmcListViewColumn(Globalization.MultilanguageResource.GetString("ColumnHeader_Type"),100),
                new MMC.MmcListViewColumn(Globalization.MultilanguageResource.GetString("ColumnHeader_Description"),300),
                new MMC.MmcListViewColumn(Globalization.MultilanguageResource.GetString("ColumnHeader_ItemID"), 100)});
            this.Mode = MMC.MmcListViewMode.Report;
            this.Refresh();
            ItemDefinitionScopeNode idSN = this.ScopeNode as ItemDefinitionScopeNode;
            if (idSN != null)
                idSN.ScopeNodeChanged += new ScopeNodeChangedHandler(ItemMembersListView_ScopeNodeChanged);
            status.Complete(Globalization.MultilanguageResource.GetString("RefreshComplete_Msg10"), true);
            /*Application.DoEvents();*/
        }

        void ItemMembersListView_ScopeNodeChanged()
        {
            if (this.isALive)
            {
                this.Refresh();
                this.SelectScopeNode(this.ScopeNode);
            }
        }

        protected override void Refresh()
        {
            this.ResultNodes.Clear();
            List<MMC.ResultNode> list = new List<MMC.ResultNode>();
            IAzManItem parentItem = ((ItemDefinitionScopeNode)this.ScopeNode).Item;
            IAzManItem[] allMembers = parentItem.GetMembers();
            foreach (IAzManItem member in allMembers)
            {
                if (member.ItemType != ItemType.Operation
                    ||
                    member.ItemType == ItemType.Operation && member.Application.Store.Storage.Mode == NetSqlAzManMode.Developer)
                {
                    MMC.ResultNode resultNode = new MMC.ResultNode();
                    resultNode.DisplayName = member.Name;
                    switch (member.ItemType)
                    {
                        case ItemType.Role:
                            resultNode.ImageIndex = ImageIndexes.RoleImgIdx;
                            break;
                        case ItemType.Task:
                            resultNode.ImageIndex = ImageIndexes.TaskImgIdx;
                            break;
                        case ItemType.Operation:
                            resultNode.ImageIndex = ImageIndexes.OperationImgIdx;
                            break;
                    }
                    resultNode.SubItemDisplayNames.AddRange(
                        new string[] {
                        member.ItemType.ToString(),
                        member.Description,
                        member.ItemId.ToString()});
                    list.Add(resultNode);
                }
            }
            this.ResultNodes.AddRange(list.ToArray());
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
