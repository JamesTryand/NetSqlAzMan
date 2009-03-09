using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.SnapIn.DirectoryServices;
using NetSqlAzMan;
using MMC = Microsoft.ManagementConsole;
using Microsoft.ManagementConsole.Advanced;
using NetSqlAzMan.SnapIn.Forms;
using NetSqlAzMan.SnapIn.ListViews;

namespace NetSqlAzMan.SnapIn.ScopeNodes
{
    public class ItemAuthorizationsScopeNode : ScopeNodeBase
    {
        protected IAzManApplication application;

        public ItemAuthorizationsScopeNode(IAzManApplication application) : base()
        {
            this.application = application;
            // Create a message view for the Store Groups node.
            MMC.MmcListViewDescription lvdStoreGroups = new MMC.MmcListViewDescription();
            lvdStoreGroups.DisplayName = Globalization.MultilanguageResource.GetString("Folder_Msg20");
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
            this.DisplayName = Globalization.MultilanguageResource.GetString("Folder_Msg20");
            this.SubItemDisplayNames.Clear();
            this.SubItemDisplayNames.Add(Globalization.MultilanguageResource.GetString("Folder_Tit20"));
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
            //Operation Definitions visibile only in Developer Mode.
            if (this.application.Store.Storage.Mode == NetSqlAzManMode.Administrator)
            {
                this.Children.AddRange(
                    new MMC.ScopeNode[] { new RoleAuthorizationsScopeNode(this.application),
                                  new TaskAuthorizationsScopeNode(this.application)});
            }
            else
            {
                this.Children.AddRange(
                        new MMC.ScopeNode[] { new RoleAuthorizationsScopeNode(this.application),
                                  new TaskAuthorizationsScopeNode(this.application),
                                  new OperationAuthorizationsScopeNode(this.application)});
            }
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
