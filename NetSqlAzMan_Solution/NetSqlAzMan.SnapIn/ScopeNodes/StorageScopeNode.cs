using System;
using System.Xml;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
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
using System.Configuration;

namespace NetSqlAzMan.SnapIn.ScopeNodes
{
    [MMC.NodeType("{CF845FEF-6D93-4de6-98A1-D3C31C3C1E0D}", Description = ".NET Sql Authorization Manager Storage")]
    public class StorageScopeNode : ScopeNodeBase
    {
        protected internal string dataSource;
        protected internal string initialCatalog;
        protected internal string security;
        protected internal string userId;
        protected internal string password;
        protected internal string otherSettings;
        protected internal IAzManStorage storage;
        protected MMC.SyncAction LanguageEnglishAction = null;
        protected MMC.SyncAction LanguageItalianAction = null;
        protected MMC.SyncAction LanguageSpanishAction = null;
        protected MMC.SyncAction LanguageAlbanianAction = null;
        protected MMC.SyncAction LanguageRussianAction = null;
        protected string selectedLanguage = "";
        private static volatile bool alreadyCheckedForUpdate = false;

        public StorageScopeNode()
            : this(String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, String.Empty, null)
        {
            this.RenderInitialMessageViewDescription();

            // Create a message view for the Storage node.
            MMC.MmcListViewDescription lvd = new MMC.MmcListViewDescription();
            lvd.DisplayName = Globalization.MultilanguageResource.GetString("Folder_Msg80");
            lvd.ViewType = typeof(StoresListView);
            lvd.Options = MMC.MmcListViewOptions.AllowUserInitiatedModeChanges;
            this.ViewDescriptions.Add(lvd);
            this.ViewDescriptions.DefaultIndex = 0;
        }

        public IAzManStorage Storage
        {
            get
            {
                return this.storage;
            }
        }

        public StorageScopeNode(
            string dataSource,
            string initialCatalog,
            string security,
            string userId,
            string password,
            string otherSettings,
        IAzManStorage storage)
        {
            try
            {
                this.dataSource = dataSource;
                this.initialCatalog = initialCatalog;
                this.security = security;
                this.userId = userId;
                this.password = password;
                this.otherSettings = otherSettings;
                this.storage = storage;

                //Async check for Tdo update
                if (!NetSqlAzMan.SnapIn.Utilities.ConsoleUtilities.commandLineArgumentOn("NoCheckForUpdate"))
                    System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(this.CheckForUpdateSync));
                //System.Threading.Thread asyncCheckForUpdate = new System.Threading.Thread(new System.Threading.ThreadStart(this.CheckForUpdateSync));
                //asyncCheckForUpdate.Start();
                this.RenderStorageScopeNode();
            }
            catch (Exception ex)
            {
                // set up the update authorizationType pane when scope node selected
                MMC.MessageViewDescription mvd = new MMC.MessageViewDescription();
                mvd.DisplayName = Globalization.MultilanguageResource.GetString("MMC_Msg20");
                mvd.BodyText = ex.Message;
                mvd.IconId = MMC.MessageViewIcon.Error;

                // attach the view and set it as the default to show
                this.ViewDescriptions.Clear();
                this.ViewDescriptions.Add(mvd);
                this.ViewDescriptions.DefaultIndex = 0;
                new NetSqlAzMan.Logging.LoggingUtility().WriteError(this.storage, ex.Message);
            }
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

        protected void RenderStorageScopeNode()
        {
            //Prepare Node
            this.DisplayName = ".NET SQL Authorization Manager";
            string connectedUserName = "?";
            if (this.storage != null)
            {
                SqlConnectionStringBuilder csb = new SqlConnectionStringBuilder(this.storage.ConnectionString);
                if (csb.IntegratedSecurity)
                    connectedUserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.Trim();
                else
                    connectedUserName = csb.UserID.Trim();
            }
            if (!String.IsNullOrEmpty(this.dataSource))
                this.DisplayName += String.Format(" ({0}\\{1} - {2})", this.dataSource.Trim().ToUpper(), this.initialCatalog.Trim(), connectedUserName);
            this.SubItemDisplayNames.Clear();
            this.ImageIndex = ImageIndexes.NetSqlAzManImgIdx;
            this.SelectedImageIndex = ImageIndexes.NetSqlAzManImgIdx;
            //Assign Tag
            this.Tag = storage;
            //Enable standard verbs
            this.EnabledStandardVerbs = MMC.StandardVerbs.Refresh;
            //Add custom actions
            this.ActionsPaneItems.Clear();
            //Sql Store Connection String - MMC.SyncAction
            MMC.SyncAction sqlStoreConnectionStringAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_Msg340"), Globalization.MultilanguageResource.GetString("Menu_Tit340"));
            this.ActionsPaneItems.Add(sqlStoreConnectionStringAction);
            sqlStoreConnectionStringAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(this.sqlStoreConnectionStringAction_Triggered);
            //Language - MMC.SyncAction
            MMC.ActionGroup LanguageActionGroup = new MMC.ActionGroup(Globalization.MultilanguageResource.GetString("Language_Msg10"), Globalization.MultilanguageResource.GetString("Language_Tit10"));
            this.LanguageEnglishAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Language_English"), Globalization.MultilanguageResource.GetString("Language_EnglishDescription"));
            this.LanguageItalianAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Language_Italian"), Globalization.MultilanguageResource.GetString("Language_ItalianDescription"));
            this.LanguageSpanishAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Language_Spanish"), Globalization.MultilanguageResource.GetString("Language_SpanishDescription"));
            this.LanguageAlbanianAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Language_Albanian"), Globalization.MultilanguageResource.GetString("Language_AlbanianDescription"));
            this.LanguageRussianAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Language_Russian"), Globalization.MultilanguageResource.GetString("Language_RussianDescription"));

            this.LanguageEnglishAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(LanguageEnglishAction_Triggered);
            this.LanguageItalianAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(LanguageItalianAction_Triggered);
            this.LanguageSpanishAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(LanguageSpanishAction_Triggered);
            this.LanguageAlbanianAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(LanguageAlbanianAction_Triggered);
            this.LanguageRussianAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(LanguageRussianAction_Triggered);

            LanguageActionGroup.Items.Add(this.LanguageEnglishAction);
            LanguageActionGroup.Items.Add(this.LanguageAlbanianAction);
            LanguageActionGroup.Items.Add(this.LanguageItalianAction);
            LanguageActionGroup.Items.Add(this.LanguageSpanishAction);
            LanguageActionGroup.Items.Add(this.LanguageRussianAction);

            this.ActionsPaneItems.Add(LanguageActionGroup);

            string selectedCulture = Globalization.MultilanguageResource.cultureName(Globalization.MultilanguageResource.GetCurrentCulture());
            if (selectedCulture == "Italian")
            {
                this.LanguageItalianAction_Triggered(this, null);
                this.LanguageItalianAction.Bulleted = true;
            }
            else if (selectedCulture == "Spanish")
            {
                this.LanguageSpanishAction_Triggered(this, null);
                this.LanguageSpanishAction.Bulleted = true;
            }
            else if (selectedCulture == "Albanian")
            {
                this.LanguageAlbanianAction_Triggered(this, null);
                this.LanguageAlbanianAction.Bulleted = true;
            }
            else if (selectedCulture == "Russian")
            {
                this.LanguageRussianAction_Triggered(this, null);
                this.LanguageRussianAction.Bulleted = true;
            }
            else
            {
                this.LanguageEnglishAction_Triggered(this, null);
                this.LanguageEnglishAction.Bulleted = true;
            }

            if (this.storage != null)
            {
                if (this.ViewDescriptions.Count > 1)
                    this.ViewDescriptions.RemoveAt(0);
                //Options - MMC.SyncAction
                MMC.ActionGroup optionsGroupAction = new MMC.ActionGroup(Globalization.MultilanguageResource.GetString("Menu_Msg350"), Globalization.MultilanguageResource.GetString("Menu_Tit350"));
                optionsGroupAction.ImageIndex = ImageIndexes.mnuConnectionSettingsImgIdx;

                //Mode & Logging
                MMC.SyncAction modeAndLoggingAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_Msg550"), Globalization.MultilanguageResource.GetString("Menu_Tit550"));
                modeAndLoggingAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(modeAndLoggingAction_Triggered);
                optionsGroupAction.Items.Add(modeAndLoggingAction);

                //Auditing
                MMC.SyncAction auditingAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_Msg560"), Globalization.MultilanguageResource.GetString("Menu_Tit560"));
                auditingAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(auditingAction_Triggered);
                optionsGroupAction.Items.Add(auditingAction);

                this.ActionsPaneItems.Add(optionsGroupAction);

                //Line MMC.SyncAction
                MMC.ActionSeparator lineAction1 = new MMC.ActionSeparator();
                this.ActionsPaneItems.Add(lineAction1);

                //Invalidate WCF Cache Service
                MMC.SyncAction invalidateWCFCacheServiceAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_Msg570"), Globalization.MultilanguageResource.GetString("Menu_Tit570"));
                invalidateWCFCacheServiceAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(this.invalidateWCFCacheServiceAction_Triggered);
                this.ActionsPaneItems.Add(invalidateWCFCacheServiceAction);


                //Line MMC.SyncAction
                MMC.ActionSeparator lineAction11 = new MMC.ActionSeparator();
                this.ActionsPaneItems.Add(lineAction11);
                //New Store - MMC.SyncAction
                MMC.SyncAction createNewStoreAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_Msg360"), Globalization.MultilanguageResource.GetString("Menu_Tit360"));
                if (!this.storage.IAmAdmin)
                    createNewStoreAction.Enabled = false;
                createNewStoreAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(this.newStoreAction_Triggered);
                this.ActionsPaneItems.Add(createNewStoreAction);
                //Line MMC.SyncAction
                MMC.ActionSeparator lineAction2 = new MMC.ActionSeparator();
                this.ActionsPaneItems.Add(lineAction2);
                //Import - MMC.SyncAction
                MMC.SyncAction importAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_Msg370"), Globalization.MultilanguageResource.GetString("Menu_Tit370"));
                if (!this.storage.IAmAdmin)
                    importAction.Enabled = false;
                importAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(importAction_Triggered);
                this.ActionsPaneItems.Add(importAction);
                //Export - MMC.SyncAction
                MMC.SyncAction exportAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_Msg380"), Globalization.MultilanguageResource.GetString("Menu_Tit380"));
                exportAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(exportAction_Triggered);
                this.ActionsPaneItems.Add(exportAction);
                //Import From Microsoft Authorization Manager - MMC.SyncAction
                MMC.SyncAction importAzManAction = new MMC.SyncAction(Globalization.MultilanguageResource.GetString("Menu_Msg390"), Globalization.MultilanguageResource.GetString("Menu_Tit390"));
                if (!this.storage.IAmAdmin)
                    importAzManAction.Enabled = false;
                importAzManAction.Triggered += new MMC.SyncAction.SyncActionEventHandler(importAzManAction_Triggered);
                this.ActionsPaneItems.Add(importAzManAction);
            }
        }

        private void RenderInitialMessageViewDescription()
        {
            // set up the update authorizationType pane when scope node selected
            MMC.MessageViewDescription mvd = new MMC.MessageViewDescription();
            mvd.DisplayName = Globalization.MultilanguageResource.GetString("MMC_Msg10");
            mvd.BodyText = Globalization.MultilanguageResource.GetString("MMC_Tit10");
            mvd.IconId = MMC.MessageViewIcon.Information;

            // attach the view and set it as the default to show
            this.ViewDescriptions.Add(mvd);
            this.ViewDescriptions.DefaultIndex = 0;
        }

        void LanguageSpanishAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            if (this.selectedLanguage != "Spanish")
            {
                this.selectedLanguage = "Spanish";
                Globalization.MultilanguageResource.SetCulture(this.selectedLanguage);
                this.Render();
                this.SnapIn.IsModified = true;
            }
        }

        void LanguageItalianAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            if (this.selectedLanguage != "Italian")
            {
                this.selectedLanguage = "Italian";
                Globalization.MultilanguageResource.SetCulture(this.selectedLanguage);
                this.Render();
                this.SnapIn.IsModified = true;
            }
        }

        void LanguageAlbanianAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            if (this.selectedLanguage != "Albanian")
            {
                this.selectedLanguage = "Albanian";
                Globalization.MultilanguageResource.SetCulture(this.selectedLanguage);
                this.Render();
                this.SnapIn.IsModified = true;
            }
        }

        void LanguageRussianAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            if (this.selectedLanguage != "Russian")
            {
                this.selectedLanguage = "Russian";
                Globalization.MultilanguageResource.SetCulture(this.selectedLanguage);
                this.Render();
                this.SnapIn.IsModified = true;
            }
        }

        void LanguageEnglishAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            if (this.selectedLanguage != "English")
            {
                this.selectedLanguage = "English";
                Globalization.MultilanguageResource.SetCulture(this.selectedLanguage);
                this.Render();
                this.SnapIn.IsModified = true;
            }
        }

        internal void internalRender()
        {
            this.Render();
        }

        protected override void Render()
        {
            //Prepare Node
            this.RenderStorageScopeNode();
            //Children
            this.Children.Clear();
            try
            {
                if (this.storage != null)
                {
                    this.storage.OpenConnection();
                    IAzManStore[] stores = this.storage.GetStores();
                    List<StoreScopeNode> list = new List<StoreScopeNode>();
                    for (int i = 0; i < stores.Length; i++)
                    {
                        list.Add(new StoreScopeNode(stores[i]));
                    }
                    this.Children.AddRange(list.ToArray());
                }
            }
            finally
            {
                if (this.storage != null)
                    this.storage.CloseConnection();
            }
        }

        void importAzManAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            try
            {
                frmImportFromAzMan frm = new frmImportFromAzMan();
                frm.storage = this.storage;
                DialogResult dr = this.SnapIn.Console.ShowDialog(frm);
                if (dr == DialogResult.OK)
                {
                    this.Render();
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("StorageScopeNode_Msg10"));
            }
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
                frm.importIntoObject = this.storage;
                frm.fileName = openFileDialog.FileName;
                this.SnapIn.Console.ShowDialog(frm);
                this.Render();
            }
        }

        private void CheckForUpdateSync(object stateObject)
        {
            try
            {
                if (StorageScopeNode.alreadyCheckedForUpdate)
                    return;
                StorageScopeNode.alreadyCheckedForUpdate = true;
                //Get ws update url from http://netsqlazman.sourceforge.net/wsNetSqlAzManUpdateUrl.txt
                System.Threading.Thread.Sleep(3000);
                System.Net.WebRequest req = System.Net.WebRequest.Create("http://netsqlazman.sourceforge.net/wsNetSqlAzManUpdateUrl.txt");
                req.Proxy = System.Net.WebRequest.GetSystemWebProxy();
                req.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
                System.Net.WebResponse resp = req.GetResponse();
                System.IO.Stream stream = resp.GetResponseStream();
                System.IO.StreamReader tr = new StreamReader(stream);
                string wsUrl = tr.ReadToEnd();
                tr.Close();
                stream.Close();
                //Call Update Providers Service
                wsUpdate.NetSqlAzManUpdateService ws = new wsUpdate.NetSqlAzManUpdateService();
                ws.Proxy = System.Net.WebRequest.GetSystemWebProxy();
                ws.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
                ws.Url = wsUrl;
                string clientVersionId = typeof(NetSqlAzMan.SqlAzManStorage).Assembly.GetName().Version.ToString().Trim();
                string[] environmentInfo = new string[] {
                "OS Version: "+Environment.OSVersion,
                "CLR Version: " + Environment.Version.ToString(),
                "Processor Count: "+Environment.ProcessorCount.ToString(),
                "Machine: " + Environment.MachineName};

                string[] results = ws.CheckForUpdate(environmentInfo, clientVersionId);
                string serverVersionId = results[0].Trim();
                string downloadUrl = results[1].Trim();
                if (String.Compare(serverVersionId, clientVersionId, true) > 0)
                {
                    System.Console.Beep();
                    DialogResult dr = MessageBox.Show(String.Format(Globalization.MultilanguageResource.GetString("StorageScopeNode_Msg20"), clientVersionId, serverVersionId, downloadUrl), Globalization.MultilanguageResource.GetString("StorageScopeNode_Tit20"), MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    if (dr == DialogResult.Yes)
                    {
                        System.Diagnostics.Process p = new System.Diagnostics.Process();
                        p.StartInfo.FileName = "iexplore.exe";
                        p.StartInfo.Arguments = downloadUrl;
                        p.Start();
                    }
                }
            }
            catch { }
        }

        void exportAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            frmExportOptions frm = new frmExportOptions();
            DialogResult dr = this.SnapIn.Console.ShowDialog(frm);
            if (dr == DialogResult.OK)
            {
                frmExport frmwait = new frmExport();
                frmwait.ShowDialog(e, frm.fileName, this.storage.GetStores(), frm.includeSecurityObjects, frm.includeDBUsers, frm.includeAuthorizations, this.storage);
            }
        }

        void auditingAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            frmSQLAudit frm = new frmSQLAudit();
            frm.storage = this.storage;
            this.SnapIn.Console.ShowDialog(frm);
        }

        void modeAndLoggingAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            try
            {
                frmOptions frm = new frmOptions();
                frm.storage = this.storage;
                frm.mode = this.storage.Mode;
                frm.logOnEventLog = this.storage.LogOnEventLog;
                frm.logOnDb = this.storage.LogOnDb;
                frm.logErrors = this.storage.LogErrors;
                frm.logWarnings = this.storage.LogWarnings;
                frm.logInformations = this.storage.LogInformations;
                DialogResult dr = this.SnapIn.Console.ShowDialog(frm);
                if (dr == DialogResult.OK)
                {
                    this.storage.Mode = frm.mode;
                    this.storage.LogOnEventLog = frm.logOnEventLog;
                    this.storage.LogOnDb = frm.logOnDb;
                    this.storage.LogErrors = frm.logErrors;
                    this.storage.LogWarnings = frm.logWarnings;
                    this.storage.LogInformations = frm.logInformations;
                    this.storage = new SqlAzManStorage(frmStorageConnection.ConstructConnectionString(this.dataSource, this.initialCatalog, !(this.security == "Sql"), this.userId, this.password, this.otherSettings));
                    this.SnapIn.IsModified = true;
                    this.Render();
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message + "\r\n" + Globalization.MultilanguageResource.GetString("StorageScopeNode_Msg30"), Globalization.MultilanguageResource.GetString("StorageScopeNode_Tit30"));
            }
        }

        protected override void OnRefresh(MMC.AsyncStatus status)
        {
            NetSqlAzMan.SnapIn.Globalization.ResourcesManager.WriteResources();
            status.Title = Globalization.MultilanguageResource.GetString("Refreshing_Msg10");
            base.OnRefresh(status);
            this.Render();
            status.Complete(Globalization.MultilanguageResource.GetString("RefreshComplete_Msg10"), true);

        }

        protected internal void sqlStoreConnectionStringAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            frmStorageConnection frm = new frmStorageConnection();
            frm.dataSource = this.dataSource;
            frm.initialCatalog = this.initialCatalog;
            frm.security = this.security;
            frm.otherSettings = this.otherSettings;
            frm.userId = this.userId;
            frm.password = this.password;
            DialogResult dr = this.SnapIn.Console.ShowDialog(frm);
            if (dr == DialogResult.OK)
            {
                this.dataSource = frm.dataSource;
                this.initialCatalog = frm.initialCatalog;
                this.security = frm.security;
                this.otherSettings = frm.otherSettings;
                this.userId = frm.userId;
                this.password = frm.password;
                this.storage = new SqlAzManStorage(frmStorageConnection.ConstructConnectionString(this.dataSource, this.initialCatalog, !(this.security == "Sql"), this.userId, this.password, this.otherSettings));
                this.Render();
                ((NetSqlAzManSnapIn)this.SnapIn).dataSource = this.dataSource;
                ((NetSqlAzManSnapIn)this.SnapIn).initialCatalog = this.initialCatalog;
                ((NetSqlAzManSnapIn)this.SnapIn).security = this.security;
                ((NetSqlAzManSnapIn)this.SnapIn).otherSettings = this.otherSettings;
                ((NetSqlAzManSnapIn)this.SnapIn).userId = this.userId;
                ((NetSqlAzManSnapIn)this.SnapIn).password = this.password;
                this.SnapIn.IsModified = true;
            }
        }

        private void newStoreAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            frmStoreProperties frm = new frmStoreProperties();
            frm.storage = this.storage;
            DialogResult dr = this.SnapIn.Console.ShowDialog(frm);
            if (dr == DialogResult.OK)
            {
                this.Children.Add(new StoreScopeNode(frm.store));
            }
        }

        private void invalidateWCFCacheServiceAction_Triggered(object sender, MMC.SyncActionEventArgs e)
        {
            frmInvalidateWCFCacheService frm = new frmInvalidateWCFCacheService();
            this.SnapIn.Console.ShowDialog(frm);
        }
    }
}
