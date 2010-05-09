using System.Windows.Forms;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.SnapIn.Forms;
using NetSqlAzMan.SnapIn.ListViews;
using MMC = Microsoft.ManagementConsole;

namespace NetSqlAzMan.SnapIn.ScopeNodes
{
    public class ItemAuthorizationScopeNode : ScopeNodeBase
    {
        protected IAzManItem item;
        public ItemAuthorizationScopeNode(IAzManItem item) : base(true)
        {
            this.item = item;
            // Create a message view for the Item node.
            MMC.MmcListViewDescription lvdItems = new MMC.MmcListViewDescription();
            lvdItems.DisplayName = Globalization.MultilanguageResource.GetString("Menu_Msg30");
            lvdItems.ViewType = typeof(AuthorizationsListView);
            lvdItems.Options = MMC.MmcListViewOptions.AllowUserInitiatedModeChanges;
            this.ViewDescriptions.Clear();
            this.ViewDescriptions.Add(lvdItems);
            this.ViewDescriptions.DefaultIndex = 0;
            this.RenderItemAuthorizationScopeNode();
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

        public IAzManItem Item
        {
            get
            {
                return this.item;
            }
            internal set
            {
                this.item = value;
            }
        }

        protected internal void RenderItemAuthorizationScopeNode()
        {
            //Prepare Node
            this.DisplayName = this.item.Name;
            this.SubItemDisplayNames.Clear();
            this.SubItemDisplayNames.AddRange(
                new string[] {
                                item.Description,
                                item.ItemId.ToString()});
            switch (this.item.ItemType)
            { 
                case ItemType.Role:
                    this.ImageIndex = this.SelectedImageIndex = ImageIndexes.RoleImgIdx;
                    break;
                case ItemType.Task:
                    this.ImageIndex = this.SelectedImageIndex = ImageIndexes.TaskImgIdx;
                    break;
                case ItemType.Operation:
                    this.ImageIndex = this.SelectedImageIndex = ImageIndexes.OperationImgIdx;
                    break;
            }
            //Assign Tag
            this.Tag = item;
            //Enable standard verbs
            this.EnabledStandardVerbs = MMC.StandardVerbs.Refresh;
            //Add custom actions
            this.ActionsPaneItems.Clear(); 
            //Item Authorization - MMC.SyncAction
            MMC.SyncAction manageAuthorizationsAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_Msg540"), Globalization.MultilanguageResource.GetString("Menu_Tit540"));
            this.ActionsPaneItems.Add(manageAuthorizationsAction);
            manageAuthorizationsAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(manageAuthorizationsAction_Triggered);
            /*System.Windows.Forms.Application.DoEvents();*/
        }

        protected override void OnRefresh(MMC.AsyncStatus status)
        {
            status.Title = Globalization.MultilanguageResource.GetString("Refreshing_Msg10");
            base.OnRefresh(status);
            this.Render();
            status.Complete(Globalization.MultilanguageResource.GetString("RefreshComplete_Msg10"), true);
        }

        protected override void Render()
        {
            //Prepare Node
            this.RenderItemAuthorizationScopeNode();
        }

        void manageAuthorizationsAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            frmItemAuthorizations frm = new frmItemAuthorizations();
            frm.Text += " - " + this.item.Name;
            frm.item = this.item;

            DialogResult dr = this.SnapIn.Console.ShowDialog(frm);
            /*Application.DoEvents();*/
            frm.Dispose();
            /*Application.DoEvents();*/
            if (dr == DialogResult.OK)
            {
                this.Render();
                try
                {
                    this.NotifyChanged();
                    /*Application.DoEvents();*/
                }
                catch { }
            }
        }
    }
}
