using System;
using System.Windows.Forms;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using MMC = Microsoft.ManagementConsole;
using Microsoft.ManagementConsole.Advanced;
using NetSqlAzMan;
using NetSqlAzMan.Logging;
using NetSqlAzMan.ENS;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.SnapIn.Forms;
using NetSqlAzMan.SnapIn.ListViews;
using NetSqlAzMan.SnapIn.ScopeNodes;
using System.Configuration;
using System.Data.SqlClient;

namespace NetSqlAzMan.SnapIn
{
    [MMC.SnapInSettings("{C8466485-78DA-473a-B864-3FE1A9A6F781}",
        DisplayName = ".NET SQL Authorization Manager",
        Description = "Management Console for .NET SQL Authorization Manager Ver. 3.6.0.6. The .NET Sql Authorization Manager allows you to set item-based permissions for Authorization Manager-enabled .NET applications.",
        Vendor = "Andrea Ferendeles - http://netsqlazman.codeplex.com")]
    [MMC.SnapInAbout("NetSqlAzMan.SnapIn.resources.dll",
         ApplicationBaseRelative = true,
         IconId = 106,
         LargeFolderBitmapId = 103,
         SmallFolderBitmapId = 103,
         SmallFolderSelectedBitmapId = 103
        )]
    public class NetSqlAzManSnapIn : Microsoft.ManagementConsole.SnapIn
    {
        protected internal string dataSource;
        protected internal string initialCatalog;
        protected internal string security;
        protected internal string userId;
        protected internal string password;
        protected internal string otherSettings;
        protected IAzManStorage storage;
        private bool loadingCustomData;
        delegate void splashScreenDelegate();
        private frmSplash splash;
        private static volatile bool splashScreenShowed = false;
        
        public NetSqlAzManSnapIn()
        {
            if (NetSqlAzMan.SnapIn.Utilities.ConsoleUtilities.commandLineArgumentOn("DebugMode"))
                MessageBox.Show("NetSqlAzMan Debug Mode. Attach to the process now.", "Debug Mode", MessageBoxButtons.OK, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1);
            this.loadingCustomData = false;
            this.Tag = this;
            ImageIndexes.LoadImages(this.SmallImages, this.LargeImages);
            this.dataSource = String.Empty;
            this.initialCatalog = String.Empty;
            this.security = String.Empty;
            this.userId = String.Empty;
            this.password = String.Empty;
            this.otherSettings = String.Empty;
            this.storage = null;
            //Catch Console Errors to Application Event Log
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
        }

        void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            new NetSqlAzMan.Logging.LoggingUtility().WriteError(this.storage, "Untrapper Console Error.\r\n" + e.Exception.Message + "\r\nat:\r\n" + e.Exception.StackTrace);
            MessageBox.Show("Untrapped Console Error.\r\nPlease review Application Event Log and send me error details at mail address:\r\naferende@hotmail.com.\r\nThanks for your collaboration.\r\n\r\nError details:\r\n" + e.Exception.Message + "\r\n\r\nat:\r\n\r\n" + e.Exception.StackTrace, "Console Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
        }

        [PreEmptive.Attributes.Setup(CustomEndpoint = "so-s.info/PreEmptive.Web.Services.Messaging/MessagingServiceV2.asmx")]
        [PreEmptive.Attributes.Teardown()]
        [PreEmptive.Attributes.Feature("Console Initialize", EventType=PreEmptive.Attributes.FeatureEventTypes.Start)]
        protected override void OnInitialize()
        {
            this.splash = new frmSplash();
            this.splash.Visible = false;
            if (!NetSqlAzManSnapIn.splashScreenShowed)
            {
                this.showSplashScreen();
            }

            base.OnInitialize();
            this.RootNode = new StorageScopeNode();

            if (this.splash.Visible)
            {
                new System.Threading.Thread(new System.Threading.ThreadStart(
                    delegate()
                    {
                        System.Threading.Thread.Sleep(2500);
                        if (!this.loadingCustomData)
                        {
                            this.splash.Close();
                            this.splash.Dispose();
                        }
                        Application.DoEvents();
                    })).Start();
            }
        }

        private void showSplashScreen()
        {
            NetSqlAzManSnapIn.splashScreenShowed = true;
            if (NetSqlAzMan.SnapIn.Utilities.ConsoleUtilities.commandLineArgumentOn("NoSplashScreen"))
                return;
            this.splash.TopMost = true;
            this.splash.Show();
            this.splash.Visible = true;
            Application.DoEvents();
        }

        /// <summary>
        /// Load in any saved data
        /// </summary>
        /// <param name="status">asynchronous status for updating the console</param>
        /// <param name="persistenceData">binary data stored in the console file</param>
        protected override void OnLoadCustomData(MMC.AsyncStatus status, byte[] persistenceData)
        {
            try
            {
                this.loadingCustomData = true;
                /*Application.DoEvents();*/
                base.OnLoadCustomData(status, persistenceData);
                string allSettings = Encoding.Unicode.GetString(persistenceData);
                string connectionSettingsString = allSettings.Substring(0, allSettings.LastIndexOf('\n') + 1);
                string savedLanguage = "en";
                if (allSettings.Split('\n').Length >= 7)
                {
                    if (!String.IsNullOrEmpty(allSettings.Split('\n')[6]))
                    {
                        savedLanguage = allSettings.Split('\n')[6];
                    }
                }
                if (!String.IsNullOrEmpty(savedLanguage))
                {
                    Globalization.MultilanguageResource.SetCulture(Globalization.MultilanguageResource.cultureName(savedLanguage));
                }
                // saved name? then set snap-in to the name
                if (string.IsNullOrEmpty(connectionSettingsString) || connectionSettingsString == new String('\n', 6))
                {
                    if (this.splash != null && this.splash.Visible)
                    {
                        new System.Threading.Thread(new System.Threading.ThreadStart(
                            delegate()
                            {
                                System.Threading.Thread.Sleep(1500);
                                this.splash.Close();
                                this.splash.Dispose();
                                Application.DoEvents();
                                this.splash = null;
                            })).Start();
                    }
                    frmStorageConnection frm = new frmStorageConnection();
                    DialogResult dr = this.Console.ShowDialog(frm);
                    if (dr == DialogResult.OK)
                    {
                        this.dataSource = frm.dataSource;
                        this.initialCatalog = frm.initialCatalog;
                        this.security = frm.security;
                        this.otherSettings = frm.otherSettings;
                        this.userId = frm.userId;
                        this.password = frm.password;
                        this.storage = new SqlAzManStorage(frmStorageConnection.ConstructConnectionString(this.dataSource, this.initialCatalog, !(this.security == "Sql"), this.userId, this.password, this.otherSettings));
                        this.UpdateRootNode();
                    }
                    else
                    {
                        this.storage = null;
                    }
                }
                else
                {
                    try
                    {
                        this.dataSource = connectionSettingsString.Split('\n')[0];
                        this.initialCatalog = connectionSettingsString.Split('\n')[1];
                        this.security = connectionSettingsString.Split('\n')[2];
                        this.otherSettings = connectionSettingsString.Split('\n')[3];
                        this.userId = connectionSettingsString.Split('\n')[4];
                        this.password = connectionSettingsString.Split('\n')[5];
                        this.storage = new SqlAzManStorage(frmStorageConnection.ConstructConnectionString(this.dataSource, this.initialCatalog, !(this.security == "Sql"), this.userId, this.password, this.otherSettings));
                        this.UpdateRootNode();
                    }
                    catch (SqlException)
                    {
                        frmStorageConnection frm = new frmStorageConnection();
                        DialogResult dr = this.Console.ShowDialog(frm);
                        if (dr == DialogResult.OK)
                        {
                            this.dataSource = frm.dataSource;
                            this.initialCatalog = frm.initialCatalog;
                            this.security = frm.security;
                            this.otherSettings = frm.otherSettings;
                            this.userId = frm.userId;
                            this.password = frm.password;
                            this.storage = new SqlAzManStorage(frmStorageConnection.ConstructConnectionString(this.dataSource, this.initialCatalog, !(this.security == "Sql"), this.userId, this.password, this.otherSettings));
                            this.UpdateRootNode();
                        }
                        else
                        {
                            this.storage = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowError(String.Format(Globalization.MultilanguageResource.GetString("MMC_Msg30"), ex.Message), Globalization.MultilanguageResource.GetString("MMC_Tit30"));
            }
            finally
            {
                if (this.splash!=null && this.splash.Visible)
                {
                    new System.Threading.Thread(new System.Threading.ThreadStart(
                        delegate()
                        {
                            System.Threading.Thread.Sleep(2500);
                            this.splash.Close();
                            this.splash.Dispose();
                            Application.DoEvents();
                        })).Start();
                }
            }
        }

        private void UpdateRootNode()
        {
            StorageScopeNode ssn = ((StorageScopeNode)this.RootNode);
            do
            {
                try
                {
                    ssn.storage = this.storage;
                    ssn.dataSource = this.dataSource;
                    ssn.initialCatalog = this.initialCatalog;
                    ssn.security = this.security;
                    ssn.otherSettings = this.otherSettings;
                    ssn.userId = this.userId;
                    ssn.password = this.password;
                    ssn.internalRender();
                }
                catch (SqlException)
                {
                    this.storage = null;
                    ssn.storage = null;
                }
                if (ssn.storage == null)
                {
                    if (this.splash != null && this.splash.Visible)
                    {
                        System.Threading.Thread.Sleep(1000);
                        this.splash.Close();
                        this.splash.Dispose();
                        Application.DoEvents();
                    }
                    frmStorageConnection frm = new frmStorageConnection();
                    DialogResult dr = this.Console.ShowDialog(frm);
                    if (dr == DialogResult.OK)
                    {
                        this.dataSource = frm.dataSource;
                        this.initialCatalog = frm.initialCatalog;
                        this.security = frm.security;
                        this.otherSettings = frm.otherSettings;
                        this.userId = frm.userId;
                        this.password = frm.password;
                        this.storage = new SqlAzManStorage(frmStorageConnection.ConstructConnectionString(this.dataSource, this.initialCatalog, !(this.security == "Sql"), this.userId, this.password, this.otherSettings));
                    }
                    else
                    {
                        return;
                    }
                }
            } while (ssn.storage == null);
        }

        /// <summary>
        /// If snapIn 'IsModified', then save data
        /// </summary>
        /// <param name="status">status for updating the console</param>
        /// <returns>binary data to be stored in the console file</returns>
        [PreEmptive.Attributes.Feature("Console Initialize", EventType = PreEmptive.Attributes.FeatureEventTypes.Stop)]
        protected override byte[] OnSaveCustomData(MMC.SyncStatus status)
        {
            return Encoding.Unicode.GetBytes(
                String.Join("\n", new string[] 
                { 
                    this.dataSource, this.initialCatalog, this.security, this.otherSettings, this.userId, this.password, Globalization.MultilanguageResource.GetCurrentCulture()
                }));
        }

        protected void internalShowDialog(string text, string caption, MessageBoxIcon icon)
        {
            try
            {
                MessageBoxParameters mbp = new MessageBoxParameters();
                mbp.Buttons = MessageBoxButtons.OK;
                mbp.Caption = caption;
                mbp.Text = text;
                mbp.Icon = icon;
                this.Console.ShowDialog(mbp);
            }
            catch (Exception ex)
            {
                // set up the update authorizationType pane when scope node selected
                MMC.MessageViewDescription mvd = new MMC.MessageViewDescription();
                mvd.DisplayName = caption;
                mvd.BodyText = String.Format("{0}\r\n\r\n{1}", text, ex.Message);
                switch (icon)
                {
                    case MessageBoxIcon.Warning: mvd.IconId = MMC.MessageViewIcon.Warning; break;
                    case MessageBoxIcon.Information: mvd.IconId = MMC.MessageViewIcon.Information; break;
                    case MessageBoxIcon.Question: mvd.IconId = MMC.MessageViewIcon.Question; break;
                    case MessageBoxIcon.None: mvd.IconId = MMC.MessageViewIcon.None; break;
                    default: mvd.IconId = MMC.MessageViewIcon.Error; break;
                }
                // attach the view and set it as the default to show
                this.RootNode.ViewDescriptions.Add(mvd);
            }
        }

        protected void ShowError(string text, string caption)
        {
            this.internalShowDialog(text, caption, MessageBoxIcon.Error);
        }

        protected void ShowInfo(string text, string caption)
        {
            this.internalShowDialog(text, caption, MessageBoxIcon.Information);
        }

        protected void ShowWarning(string text, string caption)
        {
            this.internalShowDialog(text, caption, MessageBoxIcon.Warning);
        }
    }
}
