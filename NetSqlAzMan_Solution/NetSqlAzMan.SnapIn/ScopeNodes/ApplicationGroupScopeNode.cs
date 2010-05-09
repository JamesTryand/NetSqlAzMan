using System;
using System.Windows.Forms;
using Microsoft.ManagementConsole.Advanced;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.SnapIn.Forms;
using NetSqlAzMan.SnapIn.ListViews;
using MMC = Microsoft.ManagementConsole;

namespace NetSqlAzMan.SnapIn.ScopeNodes
{
    public class ApplicationGroupScopeNode : ScopeNodeBase
    {
        protected IAzManApplicationGroup applicationGroup;
        
        public ApplicationGroupScopeNode(IAzManApplicationGroup applicationGroup) : base(true)
        {
            this.applicationGroup = applicationGroup;
            // Create a message view for the Application Group node.
            MMC.MmcListViewDescription lvlApplicationGroup = new MMC.MmcListViewDescription();
            lvlApplicationGroup.DisplayName = Globalization.MultilanguageResource.GetString("ListView_Msg10");
            lvlApplicationGroup.ViewType = typeof(ApplicationGroupListView);
            lvlApplicationGroup.Options = MMC.MmcListViewOptions.AllowUserInitiatedModeChanges;
            this.ViewDescriptions.Clear();
            this.ViewDescriptions.Add(lvlApplicationGroup);
            this.ViewDescriptions.DefaultIndex = 0;
            this.RenderApplicationGroupScopeNode();
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
        
        public IAzManApplicationGroup ApplicationGroup
        {
            get 
            { 
                return this.applicationGroup; 
            }
        }

        protected void RenderApplicationGroupScopeNode()
        {
            this.DisplayName = this.applicationGroup.Name;
            this.SubItemDisplayNames.Clear();
            this.SubItemDisplayNames.AddRange(
                new string[] {
                                this.applicationGroup.Description,
                                this.applicationGroup.GroupType.ToString(),
                                this.applicationGroup.SID.StringValue});
            this.ImageIndex = this.applicationGroup.GroupType == GroupType.Basic ? ImageIndexes.ApplicationGroupBasicImgIdx : ImageIndexes.ApplicationGroupLDAPImgIdx;
            this.SelectedImageIndex = this.ImageIndex;
            //Assign Tag
            this.Tag = this.applicationGroup;
            //Enable standard verbs
            if (this.applicationGroup.Application.IAmManager)
                this.EnabledStandardVerbs = MMC.StandardVerbs.Refresh | MMC.StandardVerbs.Delete;
            else
                this.EnabledStandardVerbs = MMC.StandardVerbs.Refresh;
            //Add custom actions
            //MMC.SyncAction - Application Group Properties
            this.ActionsPaneItems.Clear();
            MMC.SyncAction applicationGroupPropertiesAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_Msg160"), Globalization.MultilanguageResource.GetString("Menu_Tit160"));
            applicationGroupPropertiesAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(applicationGroupPropertiesAction_Triggered);
            this.ActionsPaneItems.Add(applicationGroupPropertiesAction);
            //Line MMC.SyncAction
            MMC.ActionSeparator lineAction1 = new MMC.ActionSeparator();
            this.ActionsPaneItems.Add(lineAction1);
            //Line MMC.SyncAction
            MMC.ActionSeparator lineAction2 = new MMC.ActionSeparator();
            this.ActionsPaneItems.Add(lineAction2);
            //Export - MMC.SyncAction
            MMC.SyncAction exportAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_Msg170"), Globalization.MultilanguageResource.GetString("Menu_Tit170"));
            exportAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(exportAction_Triggered);
            this.ActionsPaneItems.Add(exportAction);
            /*System.Windows.Forms.Application.DoEvents();*/
        }

        protected override void Render()
        {
            //Prepare Node
            this.RenderApplicationGroupScopeNode();
        }

        protected override void OnRefresh(MMC.AsyncStatus status)
        {
            status.Title = Globalization.MultilanguageResource.GetString("Refreshing_Msg10");
            base.OnRefresh(status);
            this.Render();
            status.Complete(Globalization.MultilanguageResource.GetString("RefreshComplete_Msg10"), true);
        }

        protected override void OnDelete(MMC.SyncStatus status)
        {
            MessageBoxParameters mbp = new MessageBoxParameters();
            mbp.Buttons = MessageBoxButtons.YesNo;
            mbp.Caption = Globalization.MultilanguageResource.GetString("Menu_Msg180");
            mbp.DefaultButton = MessageBoxDefaultButton.Button2;
            mbp.Icon = MessageBoxIcon.Question;
            mbp.Text = String.Format(Globalization.MultilanguageResource.GetString("Menu_Msg190")+"\r\n'{0}'", this.applicationGroup.Name);
            DialogResult dr = this.SnapIn.Console.ShowDialog(mbp);
            /*Application.DoEvents();*/
            if (dr == DialogResult.Yes)
            {
                try
                {
                    this.applicationGroup.Delete();
                    this.Parent.Children.Remove(this);
                    /*Application.DoEvents();*/
                }
                catch (Exception ex)
                {
                    this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("Menu_Tit200"));
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
                frmwait.ShowDialog(e, frm.fileName, new IAzManExport[] { this.applicationGroup } , frm.includeSecurityObjects, frm.includeDBUsers, frm.includeAuthorizations, this.applicationGroup.Application.Store.Storage);
            }
        }

        private void applicationGroupPropertiesAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            try
            {
                frmApplicationGroupsProperties frm = new frmApplicationGroupsProperties();
                frm.Text += " - " + this.applicationGroup.Name;
                frm.applicationGroup = this.applicationGroup;
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
            catch (Exception ex)
            {
                this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("ApplicationGroupScopeNode_Msg10"));
            }
        }
    }
}