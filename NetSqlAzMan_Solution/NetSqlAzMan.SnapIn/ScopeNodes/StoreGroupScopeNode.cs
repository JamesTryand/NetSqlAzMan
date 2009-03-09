using System;
using System.Xml;
using System.IO;
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
using NetSqlAzMan.SnapIn.ListViews;
using NetSqlAzMan.SnapIn.Forms;

namespace NetSqlAzMan.SnapIn.ScopeNodes
{
    public class StoreGroupScopeNode : ScopeNodeBase
    {
        protected IAzManStoreGroup storeGroup;

        public StoreGroupScopeNode(IAzManStoreGroup storeGroup)
            : base(true)
        {
            this.storeGroup = storeGroup;
            // Create a message view for the Store Group node.
            MMC.MmcListViewDescription lvlStoreGroup = new MMC.MmcListViewDescription();
            lvlStoreGroup.DisplayName = Globalization.MultilanguageResource.GetString("ListView_Msg150");
            lvlStoreGroup.ViewType = typeof(StoreGroupListView);
            lvlStoreGroup.Options = MMC.MmcListViewOptions.AllowUserInitiatedModeChanges;
            this.ViewDescriptions.Clear();
            this.ViewDescriptions.Add(lvlStoreGroup);
            this.ViewDescriptions.DefaultIndex = 0;
            this.RenderStoreGroupScopeNode();
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

        public IAzManStoreGroup StoreGroup
        {
            get
            {
                return this.storeGroup;
            }
        }

        protected void RenderStoreGroupScopeNode()
        {
            //Prepare node
            this.DisplayName = this.storeGroup.Name;
            this.SubItemDisplayNames.Clear();
            this.SubItemDisplayNames.AddRange(
                new string[] {
                                this.storeGroup.Description,
                                this.storeGroup.GroupType.ToString(),
                                this.storeGroup.SID.StringValue});
            this.ImageIndex = this.storeGroup.GroupType == GroupType.Basic ? ImageIndexes.StoreGroupBasicImgIdx : ImageIndexes.StoreGroupLDAPImgIdx;
            this.SelectedImageIndex = this.ImageIndex;
            //Assign Tag
            this.Tag = storeGroup;
            //Enable standard verbs
            if (this.storeGroup.Store.IAmManager)
                this.EnabledStandardVerbs = MMC.StandardVerbs.Refresh | MMC.StandardVerbs.Delete;
            else
                this.EnabledStandardVerbs = MMC.StandardVerbs.Refresh;
            //Add custom actions
            this.ActionsPaneItems.Clear();
            //MMC.SyncAction - Store Group Properties
            MMC.SyncAction storeGroupPropertiesAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_Msg400"), Globalization.MultilanguageResource.GetString("Menu_Tit400"));
            storeGroupPropertiesAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(storeGroupPropertiesAction_Triggered);
            this.ActionsPaneItems.Add(storeGroupPropertiesAction);
            //Line MMC.SyncAction
            MMC.ActionSeparator lineAction1 = new MMC.ActionSeparator();
            this.ActionsPaneItems.Add(lineAction1);
            //Export - MMC.SyncAction
            MMC.SyncAction exportAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_Msg410"), Globalization.MultilanguageResource.GetString("Menu_Tit410"));
            exportAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(exportAction_Triggered);
            this.ActionsPaneItems.Add(exportAction);
            /*System.Windows.Forms.Application.DoEvents();*/
        }

        protected override void Render()
        {
            //Prepare node
            this.RenderStoreGroupScopeNode();
        }

        protected override void OnRefresh(MMC.AsyncStatus status)
        {
            status.Title = Globalization.MultilanguageResource.GetString("Refreshing_Msg10");
            base.OnRefresh(status);
            this.Render();
            status.Complete(Globalization.MultilanguageResource.GetString("RefreshComplete_Msg10"), true);
        }

        void exportAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            frmExportOptions frm = new frmExportOptions();
            DialogResult dr = this.SnapIn.Console.ShowDialog(frm);
            if (dr == DialogResult.OK)
            {
                frmExport frmwait = new frmExport();
                frmwait.ShowDialog(e, frm.fileName, new IAzManExport[] { this.storeGroup }, frm.includeSecurityObjects, frm.includeDBUsers, frm.includeAuthorizations,this.storeGroup.Store.Storage);
            }
        }

        private void storeGroupPropertiesAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            try
            {
                frmStoreGroupsProperties frm = new frmStoreGroupsProperties();
                frm.Text += " - " + this.storeGroup.Name;
                frm.storeGroup = this.storeGroup;
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
                this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("StoreGroupScopeNode_Msg10"));
            }
        }

        protected override void OnDelete(MMC.SyncStatus status)
        {
            MessageBoxParameters mbp = new MessageBoxParameters();
            mbp.Buttons = MessageBoxButtons.YesNo;
            mbp.Caption = Globalization.MultilanguageResource.GetString("Menu_Msg420");
            mbp.DefaultButton = MessageBoxDefaultButton.Button2;
            mbp.Icon = MessageBoxIcon.Question;
            mbp.Text = String.Format(Globalization.MultilanguageResource.GetString("Menu_Msg430")+"\r\n'{0}'", this.storeGroup.Name);
            DialogResult dr = this.SnapIn.Console.ShowDialog(mbp);
            /*Application.DoEvents();*/
            if (dr == DialogResult.Yes)
            {
                try
                {
                    this.storeGroup.Delete();
                    this.Parent.Children.Remove(this);
                    /*Application.DoEvents();*/
                }
                catch (Exception ex)
                {
                    this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("StoreGroupScopeNode_Msg20"));
                }
            }
        }
    }
}
