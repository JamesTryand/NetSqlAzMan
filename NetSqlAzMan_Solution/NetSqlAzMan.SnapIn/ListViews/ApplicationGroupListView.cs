using System.Collections.Generic;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.SnapIn.ScopeNodes;
using MMC = Microsoft.ManagementConsole;

namespace NetSqlAzMan.SnapIn.ListViews
{
    public class ApplicationGroupListView : ListViewBase
    {
        public ApplicationGroupListView()
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
            if (((ApplicationGroupScopeNode)this.ScopeNode).ApplicationGroup.GroupType == GroupType.Basic)
            {
                this.Columns[0].Title = Globalization.MultilanguageResource.GetString("ColumnHeader_Name");
                this.Columns[0].SetWidth(200);
                this.Columns.AddRange(
                    new MMC.MmcListViewColumn[] {
                    new MMC.MmcListViewColumn(Globalization.MultilanguageResource.GetString("ColumnHeader_WhereDefined"),100),
                    new MMC.MmcListViewColumn(Globalization.MultilanguageResource.GetString("ColumnHeader_MemberNonMember"), 150),
                    new MMC.MmcListViewColumn("Sid", 300)});
            }
            else
            {
                this.Columns[0].Title = Globalization.MultilanguageResource.GetString("ColumnHeader_Name");
                this.Columns[0].SetWidth(200);
                this.Columns.AddRange(
                    new MMC.MmcListViewColumn[] {
                    new MMC.MmcListViewColumn(Globalization.MultilanguageResource.GetString("ColumnHeader_Description"),250),
                    new MMC.MmcListViewColumn(Globalization.MultilanguageResource.GetString("ColumnHeader_LDAPQuery"), 400)});

            }
            this.Mode = MMC.MmcListViewMode.Report;
            this.Refresh();
            ApplicationGroupScopeNode appSN = this.ScopeNode as ApplicationGroupScopeNode;
            if (appSN != null)
                appSN.ScopeNodeChanged += new ScopeNodeChangedHandler(ApplicationNodeListView_ScopeNodeChanged);
            status.Complete(Globalization.MultilanguageResource.GetString("RefreshComplete_Msg10"), true);
            /*Application.DoEvents();*/
        }

        void ApplicationNodeListView_ScopeNodeChanged()
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
            if (((ApplicationGroupScopeNode)this.ScopeNode).ApplicationGroup.GroupType == GroupType.Basic)
            {
                IAzManApplicationGroupMember[] allMembers = ((ApplicationGroupScopeNode)this.ScopeNode).ApplicationGroup.GetApplicationGroupAllMembers();
                foreach (IAzManApplicationGroupMember member in allMembers)
                {
                    string sMemberNonMember = member.IsMember ? Globalization.MultilanguageResource.GetString("Domain_Member") : Globalization.MultilanguageResource.GetString("Domain_NonMember");
                    MMC.ResultNode resultNode = new MMC.ResultNode();
                    string displayName;
                    MemberType memberType = member.GetMemberInfo(out displayName);
                    resultNode.DisplayName = displayName;
                    switch (memberType)
                    {
                        case MemberType.AnonymousSID: resultNode.ImageIndex = ImageIndexes.SidNotFoundImgIdx; break;
                        case MemberType.ApplicationGroup: resultNode.ImageIndex = ImageIndexes.ApplicationGroupBasicImgIdx; break;
                        case MemberType.StoreGroup: resultNode.ImageIndex = ImageIndexes.StoreGroupBasicImgIdx; break;
                        case MemberType.WindowsNTGroup: resultNode.ImageIndex = ImageIndexes.WindowsGroupImgIdx; break;
                        case MemberType.WindowsNTUser: resultNode.ImageIndex = ImageIndexes.WindowsUserImgIdx; break;
                        case MemberType.DatabaseUser: resultNode.ImageIndex = ImageIndexes.DatabaseUserImgIdx; break;
                    }
                    resultNode.SubItemDisplayNames.AddRange(
                        new string[] {
                        member.WhereDefined.ToString(),
                        sMemberNonMember,
                        member.SID.StringValue});
                    list.Add(resultNode);
                }
            }
            else
            {
                MMC.ResultNode resultNode = new MMC.ResultNode();
                resultNode.ImageIndex = ImageIndexes.ApplicationGroupLDAPImgIdx;
                resultNode.DisplayName = ((ApplicationGroupScopeNode)this.ScopeNode).ApplicationGroup.Name;
                resultNode.SubItemDisplayNames.AddRange(
                    new string[] 
                    { ((ApplicationGroupScopeNode)this.ScopeNode).ApplicationGroup.Description,
                    ((ApplicationGroupScopeNode)this.ScopeNode).ApplicationGroup.LDAPQuery});
                list.Add(resultNode);
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
