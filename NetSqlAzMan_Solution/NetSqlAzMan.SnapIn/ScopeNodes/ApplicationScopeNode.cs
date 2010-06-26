using System;
using System.Windows.Forms;
using Microsoft.ManagementConsole.Advanced;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.SnapIn.Forms;
using NetSqlAzMan.SnapIn.Printing;
using MMC = Microsoft.ManagementConsole;

namespace NetSqlAzMan.SnapIn.ScopeNodes
{
    public class ApplicationScopeNode : ScopeNodeBase
    {
        protected IAzManApplication application;
        public ApplicationScopeNode(IAzManApplication application)
        {
            this.application = application;
            this.Render();
        }

        protected override void OnExpand(MMC.AsyncStatus status)
        {
            status.ReportProgress(50, 100, Globalization.MultilanguageResource.GetString("Expanding_Msg10"));
            this.Render();
            status.Complete("Done.", true);
            base.OnExpand(status);
        }

        protected override bool OnExpandFromLoad(MMC.SyncStatus status)
        {
            status.ReportProgress(50, 100, Globalization.MultilanguageResource.GetString("Expanding_Msg10"));
            this.Render();
            status.Complete(Globalization.MultilanguageResource.GetString("Done_Msg10"), true);
            return base.OnExpandFromLoad(status);
        }

        public IAzManApplication Application
        {
            get
            {
                return this.application;
            }
        }

        protected void RenderApplication()
        {
            //Prepare Node
            string fixedserverrole;
            if (this.application.IAmAdmin) fixedserverrole = "Admin";
            else if (this.application.IAmManager) fixedserverrole = "Manager";
            else if (this.application.IAmUser) fixedserverrole = "User";
            else fixedserverrole = "Reader";
            this.DisplayName = String.Format("{0} ({1})", this.application.Name, fixedserverrole);
            this.SubItemDisplayNames.Clear();
            this.SubItemDisplayNames.AddRange(
                new string[] {
                                application.Description,
                                application.ApplicationId.ToString()});

            this.ImageIndex = ImageIndexes.ApplicationImgIdx;
            this.SelectedImageIndex = ImageIndexes.ApplicationImgIdx;
            //Assign Tag
            this.Tag = application;
            //Enable standard verbs
            if (this.application.Store.IAmManager)
                this.EnabledStandardVerbs = MMC.StandardVerbs.Refresh | MMC.StandardVerbs.Delete;
            else
                this.EnabledStandardVerbs = MMC.StandardVerbs.Refresh;
            //Add custom actions
            this.ActionsPaneItems.Clear();
            //Application Properties - MMC.SyncAction
            MMC.SyncAction applicationPropertiesAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_Msg230"), Globalization.MultilanguageResource.GetString("Menu_Tit230"));
            applicationPropertiesAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(applicationPropertiesAction_Triggered);
            this.ActionsPaneItems.Add(applicationPropertiesAction);
            //Line MMC.SyncAction
            MMC.ActionSeparator lineAction1 = new MMC.ActionSeparator();
            this.ActionsPaneItems.Add(lineAction1);
            //Items Hierarchy View - MMC.SyncAction
            MMC.SyncAction ItemsHVAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_ItemsHierarchicalView"), Globalization.MultilanguageResource.GetString("Menu_ItemsHierarchicalViewDescription"));
            ItemsHVAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(ItemsHVAction_Triggered);
            this.ActionsPaneItems.Add(ItemsHVAction);
            //Line MMC.SyncAction
            MMC.ActionSeparator lineAction3 = new MMC.ActionSeparator();
            this.ActionsPaneItems.Add(lineAction3);
            if (this.application.Store.Storage.Mode == NetSqlAzManMode.Developer)
            {
                //Generate CheckAccessHelper - MMC.SyncAction
                MMC.SyncAction gcahAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_Msg240"), Globalization.MultilanguageResource.GetString("Menu_Tit240"));
                gcahAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(gcahAction_Triggered);
                this.ActionsPaneItems.Add(gcahAction);
                //CheckAccessTest - MMC.SyncAction
                MMC.SyncAction checkAccessTestAction = new MMC.SyncAction("Check Access Test", "Check Access Test");
                checkAccessTestAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(checkAccessTestAction_Triggered);
                this.ActionsPaneItems.Add(checkAccessTestAction);
                //Line MMC.SyncAction
                MMC.ActionSeparator lineAction4 = new MMC.ActionSeparator();
                this.ActionsPaneItems.Add(lineAction4);
            }
            //Report Group
            MMC.ActionGroup reportAction = new MMC.ActionGroup(Globalization.MultilanguageResource.GetString("rptTitle"), Globalization.MultilanguageResource.GetString("rptDescription"));
            this.ActionsPaneItems.Add(reportAction);
            //Items Hierarchy Report - MMC.SyncAction
            MMC.SyncAction ItemsHReportAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("rptMsg10"), Globalization.MultilanguageResource.GetString("rptTit10"));
            ItemsHReportAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(ItemsHReportAction_Triggered);
            reportAction.Items.Add(ItemsHReportAction);
            //Authorizations Report - MMC.SyncAction
            MMC.SyncAction AuthorizationsReportAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("rptMsg20"), Globalization.MultilanguageResource.GetString("rptTit20"));
            AuthorizationsReportAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(AuthorizationsReportAction_Triggered);
            reportAction.Items.Add(AuthorizationsReportAction);
            //Authorizations Report - MMC.SyncAction
            MMC.SyncAction EffectivePermissionsReportAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("rptMsg30"), Globalization.MultilanguageResource.GetString("rptTit30"));
            EffectivePermissionsReportAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(EffectivePermissionsReportAction_Triggered);
            reportAction.Items.Add(EffectivePermissionsReportAction);
            //Line MMC.SyncAction
            MMC.ActionSeparator lineAction5 = new MMC.ActionSeparator();
            this.ActionsPaneItems.Add(lineAction5);
            //Import - MMC.SyncAction
            MMC.SyncAction importAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_Msg250"), Globalization.MultilanguageResource.GetString("Menu_Tit250"));
            if (!this.application.IAmManager)
                importAction.Enabled = false;
            importAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(importAction_Triggered);
            this.ActionsPaneItems.Add(importAction);
            //Export - MMC.SyncAction
            MMC.SyncAction exportAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_Msg260"), Globalization.MultilanguageResource.GetString("Menu_Tit260"));
            exportAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(exportAction_Triggered);
            this.ActionsPaneItems.Add(exportAction);
            //Adding fixed children
            this.Children.Clear();
            this.Children.AddRange(
                new MMC.ScopeNode[] { new ApplicationGroupsScopeNode(this.application),
                                  new ItemDefinitionsScopeNode(this.application),
                                  new ItemAuthorizationsScopeNode(this.application) });
            /*System.Windows.Forms.Application.DoEvents();*/
        }

        void ItemsHReportAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            frmPrint frm = new frmPrint();
            ptItemsHierarchy rep = new ptItemsHierarchy();
            rep.Applications = new IAzManApplication[] { this.application };
            frm.document = rep;
            this.SnapIn.Console.ShowDialog(frm);
        }

        void AuthorizationsReportAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            frmPrint frm = new frmPrint();
            ptItemAuthorizations rep = new ptItemAuthorizations();
            rep.Applications = new IAzManApplication[] { this.application };
            frm.document = rep;
            this.SnapIn.Console.ShowDialog(frm);
        }
        void EffectivePermissionsReportAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            frmPrint frm = new frmPrint();
            ptEffectivePermissions rep = new ptEffectivePermissions();
            rep.Applications = new IAzManApplication[] { this.application };
            frm.document = rep;
            this.SnapIn.Console.ShowDialog(frm);
        }

        void checkAccessTestAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            frmCheckAccessTest frm = new frmCheckAccessTest();
            frm.application = this.application;
            this.SnapIn.Console.ShowDialog(frm);
        }

        void gcahAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            frmGenerateCheckAccessHelper frm = new frmGenerateCheckAccessHelper();
            frm.application = this.application;
            this.SnapIn.Console.ShowDialog(frm);
        }

        void ItemsHVAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            frmItemsHierarchyView frm = new frmItemsHierarchyView();
            frm.applications = new IAzManApplication[] { this.application };
            this.SnapIn.Console.ShowDialog(frm);
        }

        internal void internalRender()
        {
            this.Render();
        }

        protected override void Render()
        {
            //Prepare Node
            this.RenderApplication();
        }
        protected override void OnRefresh(MMC.AsyncStatus status)
        {
            base.OnRefresh(status);
            this.Render();
        }

        void importAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = "xml";
            openFileDialog.FileName = "NetSqlAzMan.xml";
            openFileDialog.Filter = "Xml files|*.xml|All files|*.*";
            openFileDialog.SupportMultiDottedExtensions = true;
            openFileDialog.Title = Globalization.MultilanguageResource.GetString("ApplicationGroupsScopeNode_Msg10");
            DialogResult dr = this.SnapIn.Console.ShowDialog(openFileDialog);
            if (dr == DialogResult.OK)
            {
                frmImportOptions frm = new frmImportOptions();
                frm.importIntoObject = this.application;
                frm.fileName = openFileDialog.FileName;
                this.SnapIn.Console.ShowDialog(frm);
                this.Render();
            }
        }

        void exportAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            frmExportOptions frm = new frmExportOptions();
            DialogResult dr = this.SnapIn.Console.ShowDialog(frm);
            if (dr == DialogResult.OK)
            {
                frmExport frmwait = new frmExport();
                frmwait.ShowDialog(e, frm.fileName, new IAzManExport[] { this.application }, frm.includeSecurityObjects, frm.includeDBUsers, frm.includeAuthorizations, this.application.Store.Storage);
            }
        }

        private void applicationPropertiesAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            frmApplicationProperties frm = new frmApplicationProperties();
            frm.Text += " - " + this.application.Name;
            frm.application = this.application;
            frm.store = this.application.Store;

            DialogResult dr = this.SnapIn.Console.ShowDialog(frm);
            if (dr == DialogResult.OK)
            {
                this.RenderApplication();
            }
        }

        protected override void OnDelete(MMC.SyncStatus status)
        {
            MessageBoxParameters msg = new MessageBoxParameters();
            msg.Caption = Globalization.MultilanguageResource.GetString("Menu_Msg270");
            msg.Text = String.Format(Globalization.MultilanguageResource.GetString("Menu_Msg280")+"\r\n'{0}'", this.application.Name);
            msg.Icon = MessageBoxIcon.Question;
            msg.Buttons = MessageBoxButtons.YesNo;
            msg.DefaultButton = MessageBoxDefaultButton.Button2;
            DialogResult dr = this.SnapIn.Console.ShowDialog(msg);
            if (dr == DialogResult.Yes)
            {
                try
                {
                    this.application.Delete();
                    this.Parent.Children.Remove(this);
                }
                catch (Exception ex)
                {
                    this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("Menu_Msg290"));
                }
            }
        }
    }
}
