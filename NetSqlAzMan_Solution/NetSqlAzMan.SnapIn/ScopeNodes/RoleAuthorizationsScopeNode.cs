using System.Collections.Generic;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.SnapIn.ListViews;
using MMC = Microsoft.ManagementConsole;

namespace NetSqlAzMan.SnapIn.ScopeNodes
{
    public class RoleAuthorizationsScopeNode : ScopeNodeBase
    {
        protected IAzManApplication application;

        public RoleAuthorizationsScopeNode(IAzManApplication application)
            : base(!application.HasItems(ItemType.Role))
        {
            this.application = application;
            // Create a message view for Role node.
            MMC.MmcListViewDescription lvdStoreGroups = new MMC.MmcListViewDescription();
            lvdStoreGroups.DisplayName = Globalization.MultilanguageResource.GetString("Folder_Msg60");
            lvdStoreGroups.ViewType = typeof(ItemDefinitionsListView);
            lvdStoreGroups.Options = MMC.MmcListViewOptions.AllowUserInitiatedModeChanges;
            this.ViewDescriptions.Clear();
            this.ViewDescriptions.Add(lvdStoreGroups);
            this.ViewDescriptions.DefaultIndex = 0;
            this.RenderAuthorizations();
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

        public void RenderAuthorizations()
        {
            //Prepare Node
            this.DisplayName = Globalization.MultilanguageResource.GetString("Folder_Msg60");
            this.SubItemDisplayNames.Clear();
            this.SubItemDisplayNames.Add(Globalization.MultilanguageResource.GetString("Folder_Tit60"));
            this.ImageIndex = ImageIndexes.AuthorizationsImgIdx;
            this.SelectedImageIndex = ImageIndexes.AuthorizationsImgIdx;
            //Assign Tag
            this.Tag = this.application;
            //Enable standard verbs
            this.EnabledStandardVerbs = MMC.StandardVerbs.Refresh;
            //Add custom actions
            this.ActionsPaneItems.Clear();
            /*System.Windows.Forms.Application.DoEvents();*/
        }

        public IAzManApplication Application
        {
            get
            {
                return this.application;
            }
        }
        
        protected override void Render()
        {
            //Prepare Node
            this.RenderAuthorizations();
            //Children
            this.Children.Clear();
            IAzManItem[] items = this.application.GetItems(ItemType.Role);
            List<ItemAuthorizationScopeNode> list = new List<ItemAuthorizationScopeNode>();
            for (int i = 0; i < items.Length; i++)
            {
                list.Add(new ItemAuthorizationScopeNode(items[i]));
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
    }
}
