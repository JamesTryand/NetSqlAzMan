using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Principal;
using System.Windows.Forms;
using NetSqlAzMan;
//using CheckAccessNamespace;
using NetSqlAzMan.Cache;
using NetSqlAzMan.ENS;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.SnapIn.DirectoryServices.ADObjectPicker;
using NetSqlAzMan.SnapIn.Forms;

namespace NetSqlAzMan_WinTest
{
    public partial class Form1 : Form
    {
        private IAzManSid currentOwnerSid = new SqlAzManSID(System.Security.Principal.WindowsIdentity.GetCurrent().User.Value);
        public Form1()
        {
            InitializeComponent();
        }

        private void btnStoreManipulate_Click(object sender, EventArgs e)
        {
            IAzManStorage storage = new SqlAzManStorage("data source=eidosis4-afr;Initial Catalog=NetSqlAzManStorage;Integrated Security = SSPI;");
            storage.BeginTransaction(AzManIsolationLevel.ReadUncommitted);
            IAzManStore store1 = storage.CreateStore("Store di prova 3", "descrizione");

            IAzManStoreGroup storeGroup = store1.CreateStoreGroup(SqlAzManSID.NewSqlAzManSid(), "Store Group 2", "sg1 des", null, GroupType.Basic);
            storeGroup.CreateStoreGroupMember(SqlAzManSID.NewSqlAzManSid(), WhereDefined.LDAP, true);
            IAzManStoreGroupMember[] storeGroupMembers = storeGroup.GetStoreGroupMembers();
            store1.CreateApplication("Application 3", "description of store 3");
            IAzManApplication app = store1.GetApplication("Application 3");
            IAzManApplicationGroup appGroup = app.CreateApplicationGroup(SqlAzManSID.NewSqlAzManSid(), "Application Group 3", "ag3 des", null, GroupType.Basic);
            appGroup.CreateApplicationGroupMember(SqlAzManSID.NewSqlAzManSid(), WhereDefined.LDAP, false);
            IAzManApplicationGroupMember[] appGroupMembers = appGroup.GetApplicationGroupMembers();
            IAzManItem item1 = app.CreateItem("Responsabile UO", "descrizione", ItemType.Role);
            IAzManItem item11 = app.CreateItem("Modifica", "mod des", ItemType.Task);
            IAzManItem item111 = app.CreateItem("Salva", "salva descr", ItemType.Operation);
            item1.AddMember(item11);
            item11.AddMember(item111);
            IAzManAuthorization auth111 = item111.CreateAuthorization(this.currentOwnerSid, WhereDefined.LDAP, SqlAzManSID.NewSqlAzManSid(), WhereDefined.Store, AuthorizationType.AllowWithDelegation, DateTime.Now, null);
            IAzManAttribute<IAzManAuthorization> attr111 = auth111.CreateAttribute("UO", "SS20S");
            storage.CommitTransaction();
        }

        private void btnItemManipulate_Click(object sender, EventArgs e)
        {
            try
            {
                IAzManStorage storage = new SqlAzManStorage("data source=.;Initial Catalog=NetSqlAzManStorage;Integrated Security = SSPI;");
                IAzManStore store = storage.GetStore("Store Stress Test");
                //IAzManStoreGroup storage = store.GetStoreGroup("Store Group 1");
                IAzManItem item = store.GetApplication("Application0").GetItem("Operation0");
                string bizRule =
                                        @"using System;
using System.Security.Principal;
using System.IO;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using NetSqlAzMan;
using NetSqlAzMan.Interfaces;

namespace Prova.BizRules
{
    public sealed class BizRule : IAzManBizRule
    {
        public BizRule()
        { }

        public bool Execute(Hashtable contextParameters, IAzManSid identity, IAzManItem ownerItem, ref AuthorizationType authorizationType)
        {
            //my comments
            //Assign authorizationType to some AuthorizationType value to force CheckAccess result for this item.
            return true;
        }
    }
}
";
//@"Imports System
//Imports System.Security.Principal
//Imports System.IO
//Imports System.Data
//Imports System.Data.SqlClient
//Imports System.Collections
//Imports System.Collections.Specialized
//Imports System.Collections.Generic
//Imports System.Text
//Imports NetSqlAzMan
//Imports NetSqlAzMan.Interfaces
//
//Namespace MyApplication.BizRules
//    Public NotInheritable Class BizRule : Implements IAzManBizRule
//        Public Sub New()
//        End Sub
//
//        Public Overloads Function Execute(ByVal contextParameters As Hashtable, ByVal identity As WindowsIdentity, ByVal ownerItem As IAzManItem) As Boolean _
//            Implements IAzManBizRule.Execute
//            Return True
//        End Function
//    End Class
//End Namespace
//
//";
                item.ReloadBizRule(bizRule, NetSqlAzMan.BizRuleSourceLanguage.CSharp);
                Assembly ass = item.LoadBizRuleAssembly();

                //AuthorizationType authorizationType = storage.CheckAccess(System.Security.Principal.WindowsIdentity.GetCurrent(), DateTime.Now);
                //MessageBox.Show(authorizationType.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                ADObject[] res = NetSqlAzMan.SnapIn.DirectoryServices.DirectoryServicesUtils.ADObjectPickerShowDialog(
                    this, true, false, true);
                if (res != null)
                {
                    foreach (ADObject o in res)
                    {
                        NTAccount acc = new NTAccount(String.IsNullOrEmpty(o.UPN.Trim()) ? o.Name : o.UPN);
                        SecurityIdentifier sid = (SecurityIdentifier)acc.Translate(typeof(SecurityIdentifier));
                        MessageBox.Show(acc.Value + " " + sid.Value);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + "\r\nStackTrace:\r\n" + ex.StackTrace);
            }
        }

        private void btnACL_Click(object sender, EventArgs e)
        {
            try
            {
                
                
                //string[] users = DirectoryServicesUtils.GetAllDomainUsers();


                //IAzManStorage storage = new SqlAzManStorage("data source=.;Initial Catalog=NetSqlAzManStorage;Integrated Security = SSPI;");
                //IAzManDBUser dbUser1 = storage.GetDBUser(new SqlAzManSID(this.GetBytesFromInt32(1), true));
                //IAzManDBUser dbUser2 = storage.GetDBUser(new SqlAzManSID(this.GetBytesFromInt32(2), true));
                //AuthorizationType auth1 = storage.CheckAccess("Eidos", "DB Persone", "Accesso", dbUser1, DateTime.Now, false);
                //AuthorizationType auth2 = storage.CheckAccess("Eidos", "DB Persone", "Accesso", dbUser1, DateTime.Now, false);
                //string cs = "data source=.\\sql2005;Initial Catalog=NetSqlAzManStorage;Integrated Security=SSPI";
                string cs = "data source=.;Initial Catalog=NetSqlAzManStorage;Integrated Security=SSPI";
                var ctx = new[] { new KeyValuePair<string, object>("Value1", "111"), new KeyValuePair<string, object>("Value2", "222") };
                IAzManStorage storage = new SqlAzManStorage(cs);
                //DateTime dt = new DateTime(2009, 05, 01);
                //AuthorizationType authz = storage.CheckAccess("Eidos", "DB Persone", "Super utente senza dati retributivi", WindowsIdentity.GetCurrent(), dt, false);

                //authz = upcTest.CheckAccess("Super utente senza dati retributivi", dt);
                //MessageBox.Show(authz.ToString());
                DateTime t1, t2;
                ////return;
                t1 = DateTime.Now;
                StorageCache sc = new StorageCache(cs);
                sc.BuildStorageCache();
                //t2 = DateTime.Now;
                ////MessageBox.Show((t2 - t1).TotalMilliseconds.ToString());
                //t1 = DateTime.Now;
                //UserPermissionCache uupc = new UserPermissionCache(storage, "Eidos", "DB Persone", WindowsIdentity.GetCurrent(), true, true);
                t2 = DateTime.Now;
                //MessageBox.Show((t2 - t1).TotalMilliseconds.ToString());
                //return;
                //t1 = DateTime.Now;
                //UserPermissionCache upcTest = new UserPermissionCache(storage, "Eidos", "DB Persone", WindowsIdentity.GetCurrent(), true, true, ctx);
                //t2 = DateTime.Now;
                //MessageBox.Show((t2 - t1).TotalMilliseconds.ToString());

                //t1 = DateTime.Now;
                //for (int i = 0; i < 1000; i++)
                //{
                //    upcTest.CheckAccess("Accesso", DateTime.Now);
                //}
                //t2 = DateTime.Now;
                //MessageBox.Show((t2 - t1).TotalMilliseconds.ToString());

                string ssid = WindowsIdentity.GetCurrent().GetUserBinarySSid();
                string[] gsid = WindowsIdentity.GetCurrent().GetGroupsBinarySSid();
                

                //t1 = DateTime.Now;
                //for (int i = 0; i < 1000; i++)
                //{
                //    sc.CheckAccess("Eidos", "DB Persone", "Gestore", ssid, gsid, DateTime.Now, false);
                //}
                //t2 = DateTime.Now;
                //MessageBox.Show((t2 - t1).TotalMilliseconds.ToString());

                //sr.CacheServiceClient csc = new NetSqlAzMan_WinTest.sr.CacheServiceClient();
                //csc.Open();
                //t1 = DateTime.Now;
                ////for (int i = 0; i < 1000; i++)
                ////{
                ////var aauu = csc.CheckAccessForWindowsUsersWithoutAttributesRetrieve("ZZEntDataSvcs", "CommissionFeeTax", "Editor", ssid, gsid, DateTime.Now, false, null);
                //var aauu = sc.CheckAccess("Eidos", "DB Persone", "Accesso", ssid, gsid, DateTime.Now, false, null);
                //    //csc.GetAuthorizedItemsForWindowsUsers("Eidos", "DB Persone", ssid, gsid, DateTime.Now, null);
                ////}
                //t2 = DateTime.Now;
                ////MessageBox.Show((t2 - t1).TotalMilliseconds.ToString());
                //csc.Close();


                //t1 = DateTime.Now;
                //for (int i = 0; i < 1000; i++)
                //{
                //    storage.CheckAccess("Eidos", "DB Persone", "Gestore", WindowsIdentity.GetCurrent(), DateTime.Now, false);
                //}
                //t2 = DateTime.Now;
                //MessageBox.Show((t2 - t1).TotalMilliseconds.ToString());
                //return;

                //DateTime dt = DateTime.Now;
                //foreach (string user in users)
                //{
                //    WindowsIdentity win = new WindowsIdentity(user);
                //    sc.CheckAccess("Eidos", "DB Persone", "Gestore", win.GetUserBinarySSid(), win.GetGroupsBinarySSid(), DateTime.Now, false);
                //}
                //TimeSpan ts = DateTime.Now.Subtract(dt);
                //var seconds = ts.TotalSeconds;

                //
                //upcTest.CheckAccess("Accesso", DateTime.Now);

                List<KeyValuePair<string, string>> attributes1;
                List<KeyValuePair<string, string>> attributes2;
                List<KeyValuePair<string, string>> attributes3;
                int h;

                foreach (var store in storage.Stores)
                {
                    foreach (var application in store.Value.Applications)
                    {
                        UserPermissionCache upc = new UserPermissionCache(storage, store.Value.Name, application.Value.Name, WindowsIdentity.GetCurrent(), true, false, ctx);
                        foreach (var item in application.Value.Items)
                        {
                            this.textBox1.Text += String.Format("Store: {0}\tApplication: {1}\tItem: {2}\r\n", store.Key, application.Key, item.Key);
                            AuthorizationType auth1 = sc.CheckAccess(store.Value.Name, application.Value.Name, item.Value.Name, WindowsIdentity.GetCurrent().GetUserBinarySSid(), WindowsIdentity.GetCurrent().GetGroupsBinarySSid(), DateTime.Now, false, out attributes1, ctx);
                            AuthorizationType auth2 = storage.CheckAccess(store.Value.Name, application.Value.Name, item.Value.Name, WindowsIdentity.GetCurrent(), DateTime.Now, false, out attributes2, ctx);
                            AuthorizationType auth3 = upc.CheckAccess(item.Value.Name, DateTime.Now, out attributes3);
                            if (item.Key == "Method1")
                                h = 9;
                            this.detectedDifferences(auth1, attributes1, auth2, attributes2);
                            this.detectedDifferences(auth2, attributes2, auth3, attributes3);
                            this.detectedDifferences(auth1, attributes1, auth3, attributes3);

                        }
                    }
                }
                MessageBox.Show("Done");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void detectedDifferences(
            AuthorizationType auth1,
            List<KeyValuePair<string, string>> attrs1,
            AuthorizationType auth2,
            List<KeyValuePair<string, string>> attrs2)
        {
            if (auth1 != auth2)
                throw new Exception("Auth1 <> Auth2");
            if (attrs1.Count == attrs2.Count)
            {
                var t = (from t1 in attrs1
                         join t2 in attrs2 on new { K = t1.Key, V = t1.Value } equals new { K = t2.Key, V = t2.Value }
                         select t1).Count();
                if (t != attrs2.Count)
                {
                    throw new Exception("attrs1 <> attrs2");
                }
            }
            else
            {
                throw new Exception("attrs1 <> attrs2");
            
            }
        }

        private byte[] GetBytesFromInt32(int n)
        {
            byte[] result = BitConverter.GetBytes(n);
            if (BitConverter.IsLittleEndian)
                Array.Reverse(result);
            return result;
        }

        private void callBack(IAsyncResult ar)
        {
            MessageBox.Show("callback " + (string)ar.AsyncState);
        }

        private void btnEventHandling_Click(object sender, EventArgs e)
        {
            
            //SqlAzManENS.ApplicationUpdated += new ApplicationUpdatedDelegate(SqlAzManENS_ApplicationUpdated);
            IAzManStorage storage = new SqlAzManStorage("data source=.;Initial Catalog=NetSqlAzManStorage;Integrated Security = SSPI;");
            IAzManStore store = storage.GetStore("Store Stress Test");
            IAzManApplication application = store.GetApplication("Application0");
            application.ApplicationUpdated += new NetSqlAzMan.ENS.ApplicationUpdatedDelegate(application_Updated);
            application.Update("New Description");
            MessageBox.Show("Descrizione dell'Applicazione modificata !");
        }

        void SqlAzManENS_ApplicationUpdated(IAzManApplication application, string oldDescription)
        {
            MessageBox.Show("SqlAzManENS_ApplicationUpdated Application " + application.Name + " ha cambiato descrizione da " + oldDescription + " a " + application.Description);
        }

        void application_Updated(IAzManApplication application, string oldDescription)
        {
            MessageBox.Show("application_Updated Application " + application.Name + " ha cambiato descrizione da " + oldDescription + " a " + application.Description);
        }

        private void btnImportFromAzMan_Click(object sender, EventArgs e)
        {
            //try
            //{
            //    frmImportFromAzMan frm = new frmImportFromAzMan();
            //    IAzManStorage storage = new SqlAzManStorage("data source=.\\sql2005;Initial Catalog=NetSqlAzManStorage;Integrated Security = SSPI;");
            //    frm.storage = storage;
            //    DialogResult dr = frm.ShowDialog(this);
            //    if (dr == DialogResult.OK)
            //    {
            //        MessageBox.Show("Done !");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message, "Import From AzMan failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //IAzManStorage storage = new SqlAzManStorage("data source=eidos-egcc1\\sql2005;Initial Catalog=NetSqlAzManStore;Integrated Security = SSPI;");
            ////storage.CreateStore("Prova", "").CreateApplication("App 1", "").CreateItem("Task 1","");
            //IAzManItem storage = storage["Prova"]["App 1"]["Task 1"];

            //frmItemProperties frm = new frmItemProperties();
            //frm.Text += " - " + storage.Name;
            //frm.store = storage.Application;
            //frm.storage = storage;
            //DialogResult dr = frm.ShowDialog(this);
            ////MessageBox.Show(dr.ToString());
            
        }

        private void btnTestImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.DefaultExt = "xml";
            openFileDialog.FileName = "NetSqlAzMan.xml";
            openFileDialog.Filter = "Xml files|*.xml|All files|*.*";
            openFileDialog.SupportMultiDottedExtensions = true;
            openFileDialog.Title = "Import from ...";
            DialogResult dr = openFileDialog.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                frmImportOptions frm = new frmImportOptions();
                IAzManStorage storage = new SqlAzManStorage("data source=.;Initial Catalog=NetSqlAzManStorage;Integrated Security = SSPI;");
                IAzManStore store = storage.GetStore("Eidos");
                frm.importIntoObject = store;
                frm.fileName = openFileDialog.FileName;
                frm.ShowDialog();
            }
        }

        private void btnTestDomain_Click(object sender, EventArgs e)
        {
            //MessageBox.Show(DirectoryServicesUtils.rootDsePath);
        }

        /// <summary>
        /// Flags that indicate the scope types described by this structure. You can combine multiple scope types if all specified scopes use the same settings. 
        /// </summary>
        internal class DSOP_SCOPE_TYPE_FLAGS
        {
            internal const uint DSOP_SCOPE_TYPE_TARGET_COMPUTER = 0x00000001;
            internal const uint DSOP_SCOPE_TYPE_UPLEVEL_JOINED_DOMAIN = 0x00000002;
            internal const uint DSOP_SCOPE_TYPE_DOWNLEVEL_JOINED_DOMAIN = 0x00000004;
            internal const uint DSOP_SCOPE_TYPE_ENTERPRISE_DOMAIN = 0x00000008;
            internal const uint DSOP_SCOPE_TYPE_GLOBAL_CATALOG = 0x00000010;
            internal const uint DSOP_SCOPE_TYPE_EXTERNAL_UPLEVEL_DOMAIN = 0x00000020;
            internal const uint DSOP_SCOPE_TYPE_EXTERNAL_DOWNLEVEL_DOMAIN = 0x00000040;
            internal const uint DSOP_SCOPE_TYPE_WORKGROUP = 0x00000080;
            internal const uint DSOP_SCOPE_TYPE_USER_ENTERED_UPLEVEL_SCOPE = 0x00000100;
            internal const uint DSOP_SCOPE_TYPE_USER_ENTERED_DOWNLEVEL_SCOPE = 0x00000200;
        }

        /// <summary>
        /// Flags that indicate the format used to return ADsPath for objects selected from this scope. The flScope sid can also indicate the initial scope displayed in the Look in drop-down list. 
        /// </summary>
        internal class DSOP_SCOPE_INIT_INFO_FLAGS
        {
            internal const uint DSOP_SCOPE_FLAG_STARTING_SCOPE = 0x00000001;
            internal const uint DSOP_SCOPE_FLAG_WANT_PROVIDER_WINNT = 0x00000002;
            internal const uint DSOP_SCOPE_FLAG_WANT_PROVIDER_LDAP = 0x00000004;
            internal const uint DSOP_SCOPE_FLAG_WANT_PROVIDER_GC = 0x00000008;
            internal const uint DSOP_SCOPE_FLAG_WANT_SID_PATH = 0x00000010;
            internal const uint DSOP_SCOPE_FLAG_WANT_DOWNLEVEL_BUILTIN_PATH = 0x00000020;
            internal const uint DSOP_SCOPE_FLAG_DEFAULT_FILTER_USERS = 0x00000040;
            internal const uint DSOP_SCOPE_FLAG_DEFAULT_FILTER_GROUPS = 0x00000080;
            internal const uint DSOP_SCOPE_FLAG_DEFAULT_FILTER_COMPUTERS = 0x00000100;
            internal const uint DSOP_SCOPE_FLAG_DEFAULT_FILTER_CONTACTS = 0x00000200;
        }

        /// <summary>
        /// Filter flags to use for an up-level scope, regardless of whether it is a mixed or native mode domain. 
        /// </summary>
        internal class DSOP_FILTER_FLAGS_FLAGS
        {
            internal const uint DSOP_FILTER_INCLUDE_ADVANCED_VIEW = 0x00000001;
            internal const uint DSOP_FILTER_USERS = 0x00000002;
            internal const uint DSOP_FILTER_BUILTIN_GROUPS = 0x00000004;
            internal const uint DSOP_FILTER_WELL_KNOWN_PRINCIPALS = 0x00000008;
            internal const uint DSOP_FILTER_UNIVERSAL_GROUPS_DL = 0x00000010;
            internal const uint DSOP_FILTER_UNIVERSAL_GROUPS_SE = 0x00000020;
            internal const uint DSOP_FILTER_GLOBAL_GROUPS_DL = 0x00000040;
            internal const uint DSOP_FILTER_GLOBAL_GROUPS_SE = 0x00000080;
            internal const uint DSOP_FILTER_DOMAIN_LOCAL_GROUPS_DL = 0x00000100;
            internal const uint DSOP_FILTER_DOMAIN_LOCAL_GROUPS_SE = 0x00000200;
            internal const uint DSOP_FILTER_CONTACTS = 0x00000400;
            internal const uint DSOP_FILTER_COMPUTERS = 0x00000800;
        }

        /// <summary>
        /// Contains the filter flags to use for down-level scopes
        /// </summary>
        internal class DSOP_DOWNLEVEL_FLAGS
        {
            internal const uint DSOP_DOWNLEVEL_FILTER_USERS = 0x80000001;
            internal const uint DSOP_DOWNLEVEL_FILTER_LOCAL_GROUPS = 0x80000002;
            internal const uint DSOP_DOWNLEVEL_FILTER_GLOBAL_GROUPS = 0x80000004;
            internal const uint DSOP_DOWNLEVEL_FILTER_COMPUTERS = 0x80000008;
            internal const uint DSOP_DOWNLEVEL_FILTER_WORLD = 0x80000010;
            internal const uint DSOP_DOWNLEVEL_FILTER_AUTHENTICATED_USER = 0x80000020;
            internal const uint DSOP_DOWNLEVEL_FILTER_ANONYMOUS = 0x80000040;
            internal const uint DSOP_DOWNLEVEL_FILTER_BATCH = 0x80000080;
            internal const uint DSOP_DOWNLEVEL_FILTER_CREATOR_OWNER = 0x80000100;
            internal const uint DSOP_DOWNLEVEL_FILTER_CREATOR_GROUP = 0x80000200;
            internal const uint DSOP_DOWNLEVEL_FILTER_DIALUP = 0x80000400;
            internal const uint DSOP_DOWNLEVEL_FILTER_INTERACTIVE = 0x80000800;
            internal const uint DSOP_DOWNLEVEL_FILTER_NETWORK = 0x80001000;
            internal const uint DSOP_DOWNLEVEL_FILTER_SERVICE = 0x80002000;
            internal const uint DSOP_DOWNLEVEL_FILTER_SYSTEM = 0x80004000;
            internal const uint DSOP_DOWNLEVEL_FILTER_EXCLUDE_BUILTIN_GROUPS = 0x80008000;
            internal const uint DSOP_DOWNLEVEL_FILTER_TERMINAL_SERVER = 0x80010000;
            internal const uint DSOP_DOWNLEVEL_FILTER_ALL_WELLKNOWN_SIDS = 0x80020000;
            internal const uint DSOP_DOWNLEVEL_FILTER_LOCAL_SERVICE = 0x80040000;
            internal const uint DSOP_DOWNLEVEL_FILTER_NETWORK_SERVICE = 0x80080000;
            internal const uint DSOP_DOWNLEVEL_FILTER_REMOTE_LOGON = 0x80100000;
        }

        private void btnTestADShowDialog2_Click(object sender, EventArgs e)
        {
            //bool showLocalUsersAndGroups = true;

            //// Initialize 1st search scope			

            //uint flType =
            //    DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_DOWNLEVEL_JOINED_DOMAIN |
            //    DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_ENTERPRISE_DOMAIN |
            //    DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_EXTERNAL_DOWNLEVEL_DOMAIN |
            //    DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_EXTERNAL_UPLEVEL_DOMAIN |
            //    DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_GLOBAL_CATALOG |
            //    //DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_TARGET_COMPUTER |
            //    DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_UPLEVEL_JOINED_DOMAIN |
            //    DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_USER_ENTERED_DOWNLEVEL_SCOPE |
            //    DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_USER_ENTERED_UPLEVEL_SCOPE |
            //    DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_WORKGROUP;

            //if (showLocalUsersAndGroups)
            //    flType = flType | DSOP_SCOPE_TYPE_FLAGS.DSOP_SCOPE_TYPE_TARGET_COMPUTER;

            //uint flScope =
            //    DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_DEFAULT_FILTER_USERS |
            //    DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_DEFAULT_FILTER_GROUPS |
            //    DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_STARTING_SCOPE |
            //    DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_WANT_PROVIDER_LDAP |
            //    DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_WANT_DOWNLEVEL_BUILTIN_PATH |
            //    //DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_WANT_SID_PATH |
            //    DSOP_SCOPE_INIT_INFO_FLAGS.DSOP_SCOPE_FLAG_WANT_PROVIDER_WINNT; // Starting !?;
            

            //uint flBothModes =
            //    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_BUILTIN_GROUPS |
            //    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_DOMAIN_LOCAL_GROUPS_DL |
            //    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_DOMAIN_LOCAL_GROUPS_SE |
            //    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_GLOBAL_GROUPS_DL |
            //    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_GLOBAL_GROUPS_SE |
            //    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_INCLUDE_ADVANCED_VIEW |
            //    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_UNIVERSAL_GROUPS_DL |
            //    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_UNIVERSAL_GROUPS_SE |
            //    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_USERS | 
            //    DSOP_FILTER_FLAGS_FLAGS.DSOP_FILTER_WELL_KNOWN_PRINCIPALS; // +1 = advanced view, Check MSDN for the available options

            //uint flDownlevel =
            //    DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_ALL_WELLKNOWN_SIDS |
            //    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_ANONYMOUS |
            //    DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_AUTHENTICATED_USER |
            //    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_BATCH |
            //    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_CREATOR_GROUP |
            //    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_CREATOR_OWNER |
            //    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_DIALUP |
            //    DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_GLOBAL_GROUPS |
            //    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_INTERACTIVE |
            //    DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_LOCAL_GROUPS |
            //    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_LOCAL_SERVICE |
            //    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_NETWORK |
            //    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_NETWORK_SERVICE |
            //    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_REMOTE_LOGON |
            //    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_SERVICE |
            //    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_SYSTEM |
            //    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_TERMINAL_SERVER |
            //    DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_USERS;
            //    //DSOP_DOWNLEVEL_FLAGS.DSOP_DOWNLEVEL_FILTER_WORLD;

            //ADObjectPickerClass cadObjectPicker = new ADObjectPickerClass();
            //cadObjectPicker.ScopeTypeFlags = flType;
            //cadObjectPicker.ScopeFlags = flScope;
            //cadObjectPicker.UplevelFilterFlags_Both = flBothModes;
            //cadObjectPicker.DownLevelFilterFlags = flDownlevel;
            //cadObjectPicker.InvokeDialog(this.Handle.ToInt32());
            //ADObjectColl authorizationType = (ADObjectColl)cadObjectPicker.ADObjectsColl;
            //for (uint j = 1; j<=authorizationType.Count; j++)
            //{
            //    int i = (int)j;
            //    ADObjectInfo info = (ADObjectInfo)authorizationType.Item(i);
            //    ADObject ad = new ADObject();
            //    ad.ADSPath = info.ADPath;
            //    ad.ClassName = info.Class;
            //    ad.Name = info.Name;
            //    ad.UPN = info.UPN;
            //    NTAccount Account = new NTAccount("Administrator");
            //    SecurityIdentifier Sid = (SecurityIdentifier)
            //      Account.Translate(typeof(SecurityIdentifier));
               
            //    string s = String.Format("Name: {0} Class: {1} ADPath: {2} Sid: {3}", info.Name, info.Class, info.ADPath, Sid.Value);
            //    MessageBox.Show(s);
            }

        private void btnCheckAccessTemplate_Click(object sender, EventArgs e)
        {
            string cs = "data source=eidosis4-afr;Initial Catalog=NetSqlAzManStorage;Integrated Security = SSPI;";
            My_Application.Security.CheckAccessHelper helper = new My_Application.Security.CheckAccessHelper(cs, WindowsIdentity.GetCurrent());
            helper.OpenConnection();
            bool result = helper.CheckAccess(My_Application.Security.CheckAccessHelper.Operation.Op_1);
            helper.CloseConnection();
            //Use result for your biz
            if (result == true)
            {
                //Allow or AllowWithDelegation
            }
            else
            { 
                //Deny or Neutral
            }
        }

        private void btnGenerateCheckAccessHelper_Click(object sender, EventArgs e)
        {
            IAzManStorage storage = new SqlAzManStorage("data source=eidosis4-afr;Initial Catalog=NetSqlAzManStorage;Integrated Security = SSPI;");
            IAzManApplication application = storage["Eidos"]["DB Persone"];
            CodeCompileUnit ccu = NetSqlAzMan.CodeDom.CodeDomGenerator.GenerateItemConstants("MyApplication.NetSqlHelper", true, true, application, NetSqlAzMan.CodeDom.Language.CSharp);
            string code = NetSqlAzMan.CodeDom.CodeDomGenerator.GenerateSourceCode(ccu, NetSqlAzMan.CodeDom.Language.CSharp);
            this.textBox1.Text = code;
        }

        private void btnDBGetUsers_Click(object sender, EventArgs e)
        {
            this.textBox1.Text = String.Empty;
            IAzManStorage storage = new SqlAzManStorage("data source=eidosis4-afr;Initial Catalog=NetSqlAzManStorage;Integrated Security = SSPI;");
            IAzManStore store = storage["My Store"];

            IAzManDBUser dbu = store.GetDBUser(store.GetStoreGroup("sg1").SID);
            //IAzManDBUser[] dbUsers = store. app.GetDBUsers();
            //foreach (IAzManDBUser dbU in dbUsers)
            //{

            //    this.textBox1.Text += String.Format("Sid: {0} - Name: {1}\r\n", dbU.CustomSid.StringValue, dbU.UserName);
            //}
            ////MessageBox.Show(app.GetDBUser("andrea").CustomSid.StringValue);
            //WindowsIdentity win = WindowsIdentity.GetCurrent();
            ////app.GetItem("ResponsabileUO").CreateAuthorization(new SqlAzManSID(win.User), WhereDefined.LDAP, app.GetDBUser("a.ferendeles").CustomSid, WhereDefined.Database, AuthorizationType.AllowWithDelegation, null, null);
            ////AuthorizationType auth = storage.CheckAccess("NetSqlAzManStore", "ApplicazioneSia", "Delega", storage.GetDBUser("a.ferendeles"), DateTime.Now, true);
            ////MessageBox.Show(auth.ToString());
        }

        private void btnCheckAccessTest_Click(object sender, EventArgs e)
        {
            //IAzManStorage storage = new SqlAzManStorage("data source=.;Initial Catalog=NetSqlAzManStorage;Integrated Security = SSPI;");
            //List<KeyValuePair<string, string>> attributes;
            //var auth = storage.CheckAccess("AET Authorization Store", "Trading Hub", "Create Deal", WindowsIdentity.GetCurrent(), DateTime.Now, false, out attributes);
            //return;
            frmCheckAccessTest frm = new frmCheckAccessTest();
            IAzManStorage storage = new SqlAzManStorage("data source=.\\sql2005;Initial Catalog=NetSqlAzManStorage;Integrated Security = SSPI;");
            IAzManStore store = storage.GetStore("Sistel-1Sez");
            frm.application = store.GetApplication("Perseo.net");
            frm.ShowDialog();
        }

        private void btnIsAMemberOf_Click(object sender, EventArgs e)
        {
        }

        private void btnIHV_Click(object sender, EventArgs e)
        {
            //IAzManStorage storage = new SqlAzManStorage("data source=.;Initial Catalog=NetSqlAzManStorage;Integrated Security = SSPI;");
            //IAzManStore store = storage.GetStore("Store Stress Test");
            //NetSqlAzMan.SnapIn.Printing.ptItemAuthorizations doc = new NetSqlAzMan.SnapIn.Printing.ptItemAuthorizations();
            //doc.Applications = new IAzManApplication[] { 
            //    store.GetApplication("Application0")
            //    //,store.GetApplication("Application1"),
            //    //store.GetApplication("Application2"),
            //    //store.GetApplication("Application3"),
            //    //store.GetApplication("Application4")
            //};
            //frmPrint frm = new frmPrint();
            //frm.Document = doc;
            //frm.ShowDialog(this);

            IAzManStorage storage = new SqlAzManStorage("data source=.;Initial Catalog=NetSqlAzManStorage;Integrated Security = SSPI;");
            IAzManStore store = storage.GetStore("Eidos");
            NetSqlAzMan.SnapIn.Printing.ptEffectivePermissions doc = new NetSqlAzMan.SnapIn.Printing.ptEffectivePermissions();
            doc.Applications = new IAzManApplication[] { 
                store.GetApplication("DB Persone")
                //,store.GetApplication("Application1"),
                //store.GetApplication("Application2"),
                //store.GetApplication("Application3"),
                //store.GetApplication("Application4")
            };
            frmPrint frm = new frmPrint();
            frm.Document = doc;
            frm.ShowDialog(this);
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            IAzManStorage storage = new SqlAzManStorage("data source=.;Initial Catalog=NetSqlAzManStorage;Integrated Security = SSPI;");
            IAzManStore store = storage.GetStore("Store Stress Test");
            foreach (KeyValuePair<string, bool> kvp in store.GetManagers())
            {
                MessageBox.Show(String.Format("Manager: {0} IsSqlRole: {1}", kvp.Key, kvp.Value));
            }
            foreach (KeyValuePair<string, bool> kvp in store.GetUsers())
            {
                MessageBox.Show(String.Format("User: {0} IsSqlRole: {1}", kvp.Key, kvp.Value));
            }
            foreach (KeyValuePair<string, bool> kvp in store.GetReaders())
            {
                MessageBox.Show(String.Format("Reader: {0} IsSqlRole: {1}", kvp.Key, kvp.Value));
            }
            IAzManApplication app = store["Application1"];
            foreach (KeyValuePair<string, bool> kvp in app.GetManagers())
            {
                MessageBox.Show(String.Format("Manager: {0} IsSqlRole: {1}", kvp.Key, kvp.Value));
            }
            foreach (KeyValuePair<string, bool> kvp in app.GetUsers())
            {
                MessageBox.Show(String.Format("User: {0} IsSqlRole: {1}", kvp.Key, kvp.Value));
            }
            foreach (KeyValuePair<string, bool> kvp in app.GetReaders())
            {
                MessageBox.Show(String.Format("Reader: {0} IsSqlRole: {1}", kvp.Key, kvp.Value));
            }
        }

        private void btnCacheTest_Click(object sender, EventArgs e)
        {
            NetSqlAzMan.Cache.StorageCache sc = new StorageCache("data source=.;Initial Catalog=NetSqlAzManStorage;Integrated Security = SSPI;");
            sc.BuildStorageCache("Eidos; Olsa", "Web Portal; db persone");
            //DateTime dtStart = DateTime.Now;
            //IAzManStorage storage = new SqlAzManStorage("data source=.;Initial Catalog=NetSqlAzManStorage;Integrated Security = SSPI;");
            //NetSqlAzMan.Cache.UserPermissionCache userPermissionCache = new NetSqlAzMan.Cache.UserPermissionCache(storage, "Italferr", "CartaDeiServizi", WindowsIdentity.GetCurrent(), true, true);
            //AuthorizationType auth = userPermissionCache.CheckAccess("My Operation", DateTime.Now);
            //MessageBox.Show(((TimeSpan)(DateTime.Now-dtStart)).TotalMilliseconds.ToString());
        }

        private void btnCheckStoreAccess_Click(object sender, EventArgs e)
        {
            IAzManStorage storage = new SqlAzManStorage("data source=.;Initial Catalog=NetSqlAzManStorage;Integrated Security = SSPI;");
            foreach (IAzManStore store in storage.GetStores())
            {
                //MessageBox.Show(String.Format("Store: {0} - Access: {1}", store.Name, store.CheckStoreAccess(WindowsIdentity.GetCurrent(), DateTime.Now)));
                store.CheckStoreAccess(WindowsIdentity.GetCurrent(), DateTime.Now);
                foreach (IAzManApplication application in store.GetApplications())
                {
                    //MessageBox.Show(String.Format("Application: {0} - Access: {1}", application.Name, application.CheckApplicationAccess(WindowsIdentity.GetCurrent(), DateTime.Now)));
                    application.CheckApplicationAccess(WindowsIdentity.GetCurrent(), DateTime.Now);
                }
            }
        }

        private void btnCacheServiceTest_Click(object sender, EventArgs e)
        {
            List<TimeSpan> times = new List<TimeSpan>();
            List<System.Threading.Thread> threads = new List<System.Threading.Thread>();
            int max = 100;
            for (int i = 0; i < max; i++)
            {
                System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(
                () =>
                {
                    int k = i;
                    Random r = new Random();
                    //System.Threading.Thread.Sleep(r.Next(300));
                    DateTime now = DateTime.Now;
                    sr.CacheServiceClient c = new NetSqlAzMan_WinTest.sr.CacheServiceClient();
                    c.Open();
                    KeyValuePair<string, string>[] attributes;
                    string user = WindowsIdentity.GetCurrent().GetUserBinarySSid();
                    string[] groups = WindowsIdentity.GetCurrent().GetGroupsBinarySSid();
                    AuthorizationType auth = c.CheckAccessForWindowsUsersWithAttributesRetrieve(out attributes, "Italferr", "CartaDeiServizi", "Visualizza Dettagli Richiesta", user, groups, DateTime.Now, false, null);
                    c.Close();
                    TimeSpan ts = DateTime.Now.Subtract(now);
                    if (auth != AuthorizationType.Allow)
                    {
                        throw new Exception("Error");
                    }
                    times.Add(ts);
                }));
                threads.Add(t);
                t.Start();
            }
            foreach (System.Threading.Thread t in threads)
            {
                t.Join();
            }
            this.textBox1.Text = String.Empty;
            int cc = 0;
            foreach (TimeSpan ts in times)
            {
                this.textBox1.Text += String.Format("{0}) {1}\r\n",++cc, ts);
            }
            MessageBox.Show("Done");
            
            //DateTime now = DateTime.Now;
            //NetSqlAzMan.Cache.StorageCache sc = new StorageCache("data source=.;initial catalog=NetSqlAzManStorage;user id=sa;password=");
            //sc.BuildStorageCache("Italferr", "CartaDeiServizi");
            //WindowsIdentity wid = WindowsIdentity.GetCurrent();
            //string user = wid.GetUserBinarySSid(); //using NetSqlAzMan.Cache needed
            //string[] groups = wid.GetGroupsBinarySSid(); //using NetSqlAzMan.Cache needed
            //AuthorizationType au = sc.CheckAccess("Italferr", "CartaDeiServizi", "Approvatore Sistemi Informativi", user, groups, DateTime.Now, false);
            //MessageBox.Show(au.ToString());
            //return;
            
            //int max = 100;


            //sr.CacheServiceClient client = new NetSqlAzMan_WinTest.sr.CacheServiceClient();
            //for (int i = 0; i < max; i++)
            //{
            //    KeyValuePair<string, string>[] attributes;
            //    auth = client.CheckAccessForWindowsUsersWithAttributesRetrieve(out attributes, "Italferr", "CartaDeiServizi", "Visualizza Dettagli Richiesta", user, groups, DateTime.Now, false, null);
            //}
            //client.Close();
            //MessageBox.Show(auth.ToString());
            //TimeSpan ts = DateTime.Now.Subtract(now);
            //MessageBox.Show(String.Format("Total: {0} - Single: {1}", ts.TotalMilliseconds, ts.TotalMilliseconds / (double)max));

            //NetSqlAzMan.Cache.StorageCache sc = new NetSqlAzMan.Cache.StorageCache("data source=.;initial catalog=NetSqlAzManStorage;user id=sa;password=");
            //sc.BuildStorageCache();
            //WindowsIdentity wid = WindowsIdentity.GetCurrent();
            //List<KeyValuePair<string, string>> attributes;
            ////AuthorizationType auth = sc.CheckAccess("ItalferR", "CartadeiServizi", "Visualizza dettagli Richiesta ", wid.User, wid.Groups, DateTime.Now, false, out attributes);
            //DateTime now = DateTime.Now;
            //int max = 10000;
            //string user = wid.GetUserBinarySSid();
            //string[] groups = wid.GetGroupsBinarySSid();
            //for (int i = 0; i < max; i++)
            //{
            //    AuthorizationType auth = sc.CheckAccess("Italferr", "CartaDeiServizi", "visualizza dettagli richiesta", user, groups, DateTime.Now, false, out attributes);
            //}
            //TimeSpan ts = DateTime.Now.Subtract(now);
            //MessageBox.Show(String.Format("Total: {0} - Single: {1}",ts.TotalMilliseconds, ts.TotalMilliseconds/(double)max));
        }

        /// <summary>
        /// Create an Authorization Delegate
        /// </summary>
        private void AddDBUserToRole(string dbUserName, string roleName)
        {
            //Sql Storage connection string
            string sqlConnectionString = "data source=(local);initial catalog=NetSqlAzManStorage;user id=sa;password=password";
            //Create an instance of SqlAzManStorage class
            using (IAzManStorage storage = new SqlAzManStorage(sqlConnectionString))
            {
                storage.OpenConnection();
                IAzManStore mystore = storage.GetStore("My Store"); //or storage["My Store"]
                IAzManApplication myapp = mystore.GetApplication("My Application");
                IAzManItem myRole = myapp.GetItem(roleName);
                //Retrieve DB user identity
                IAzManDBUser dbUser = storage.GetDBUser(dbUserName);
                //Add DB "My Db User" to "My Role" role.
                IAzManAuthorization auth = myRole.CreateAuthorization(new SqlAzManSID(WindowsIdentity.GetCurrent().User), WhereDefined.LDAP, dbUser.CustomSid, WhereDefined.Database, AuthorizationType.Allow, null, null);
                //Optional: add authorization attribute
                //auth.CreateAttribute("attribute key", "attribute value");
                storage.CloseConnection();
            }
        }

        private void btnSerializeUserPermissionCache_Click(object sender, EventArgs e)
        {
            string sqlConnectionString = "data source=(local);initial catalog=NetSqlAzManStorage;user id=sa;password=";
            StorageCache sc = new StorageCache(sqlConnectionString);
            sc.BuildStorageCache();

            BinaryFormatter xSer = new BinaryFormatter();
            FileStream fs = File.Create("c:\\ser.xml");
            xSer.Serialize(fs, sc);
            fs.Close();

            fs = File.Open("c:\\ser.xml", FileMode.Open);
            StorageCache sc2 = (StorageCache)xSer.Deserialize(fs);
            fs.Close();
            AuthorizationType result = sc2.CheckAccess("Italferr", "CartaDeiServizi", "Visualizza Richiesta RAC", WindowsIdentity.GetCurrent().GetUserBinarySSid(), DateTime.Now, false);
            MessageBox.Show(result.ToString());

            //string sqlConnectionString = "data source=(local);initial catalog=NetSqlAzManStorage;user id=sa;password=";
            //IAzManStorage storage = new SqlAzManStorage(sqlConnectionString);
            //UserPermissionCache upc = new UserPermissionCache(storage, "Italferr", "CartaDeiServizi", WindowsIdentity.GetCurrent(), true, true);

            //BinaryFormatter xSer = new BinaryFormatter();
            //FileStream fs = File.Create("c:\\ser.xml");
            //xSer.Serialize(fs, upc);
            //fs.Close();

            //fs = File.Open("c:\\ser.xml", FileMode.Open);
            //UserPermissionCache upc2 = (UserPermissionCache)xSer.Deserialize(fs);
            //fs.Close();
            //AuthorizationType result = upc2.CheckAccess("Visualizza Richiesta RAC", DateTime.Now);
            //MessageBox.Show(result.ToString());

        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            IAzManStorage storage = new SqlAzManStorage("data source=.;Initial Catalog=NetSqlAzManStorage;user id=sa;password=");
            IAzManApplication app = storage.GetStore("Eidos").GetApplication("Feedback");

            frmExportOptions frm = new frmExportOptions();

            frmExport frmwait = new frmExport();
            frmwait.ShowDialog(null, "c:\\netsqlazman.xml", new IAzManExport[] { app }, true, false, true, app.Store.Storage);
        }

        private void btnCreateItemsFromAFolder_Click(object sender, EventArgs e)
        {
            using (IAzManStorage storage = new SqlAzManStorage("Data Source=(local);Initial Catalog=NetSqlAzManStorage;Integrated Security=SSPI;"))
            {
                storage.OpenConnection();
                storage.BeginTransaction();
                var a = storage["Eidos"]["DB Persone"]["Gestore"].GetMembers();

                

            }
            this.CreateItemsFromAFolder(
                "Data Source=(local);Initial Catalog=NetSqlAzManStorage;Integrated Security=SSPI;",
                "My Store",
                "My Application",
                @"D:\Documenti\EIDOS\ICP\EIDOS.ApplicazioniAziendali\EIDOS.ApplicazioniAziendali.DBPersone.Web",
                "*.aspx",
                ItemType.Task);
        }

        public void CreateItemsFromAFolder(
            string storageConnectionString, 
            string storeName,
            string applicationName,
            string folderPath, 
            string searchPattern,
            ItemType itemType)
        {
            using (IAzManStorage storage = new SqlAzManStorage(storageConnectionString))
            {
                storage.OpenConnection();
                storage.BeginTransaction();
                try
                {
                    IAzManApplication app = storage.GetStore(storeName).GetApplication(applicationName);
                    DirectoryInfo di = new DirectoryInfo(folderPath);
                    foreach (FileInfo fi in di.GetFiles(searchPattern))
                    {
                        //Use some recursive function to get subfolder files
                        app.CreateItem(fi.Name, String.Empty, itemType);
                    }
                    storage.CommitTransaction();
                }
                catch
                {
                    storage.RollBackTransaction();
                }
            }
        }

        private void btnStorageCacheAuthorizedItems_Click(object sender, EventArgs e)
        {
            string cs = "data source=.;Initial Catalog=NetSqlAzManStorage;user id=testuser;password=;";
            var ctx = new[] { new KeyValuePair<string, object>("Value1", "111"), new KeyValuePair<string, object>("Value2", "222") };
            IAzManStorage storage = new SqlAzManStorage(cs);
            IAzManApplication app = storage["Eidos"]["DB Persone"];
            var res = app.GetItem("Gestore");
            MessageBox.Show(res.Members.Count.ToString());
            //string ssid = WindowsIdentity.GetCurrent().GetUserBinarySSid();
            //string[] gsid = WindowsIdentity.GetCurrent().GetGroupsBinarySSid();
            //DateTime t1, t2;

            //StorageCache sc = new StorageCache(cs);
            //sc.BuildStorageCache("Eidos");
            //t1 = DateTime.Now;
            //for (int i = 0; i < 1000; i++)
            //{
            //    AuthorizedItem[] result = sc.GetAuthorizedItems("Eidos", "DB Persone", ssid, gsid, DateTime.Now, ctx);
            //}
            //t2 = DateTime.Now;
            //double ms = t2.Subtract(t1).TotalMilliseconds;
            //MessageBox.Show(String.Format("Done in {0} ms", ms));
        }

        /// <summary>
        /// Handles the Click event of the btnCreateALotOfItems control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void btnCreateALotOfItems_Click(object sender, EventArgs e)
        {
            string cs = "data source=.\\sql2005;initial catalog=NetSqlAzManStorage;Integrated Security=SSPI;";
            IAzManStorage storage = new SqlAzManStorage(cs);
            storage.OpenConnection();
            storage.BeginTransaction();
            IAzManStore store = storage.CreateStore("Test2", String.Empty);
            IAzManApplication app = store.CreateApplication("Test", String.Empty);
            storage.ENS.AuthorizationCreated+= new AuthorizationCreatedDelegate(ens_AuthorizationCreated);
            
            //Create 1 MLN Items
            for (int r = 0; r < 100; r++)
            {
                IAzManItem role = app.CreateItem("Role " + r.ToString(), "", ItemType.Role);

                IAzManAuthorization auth = role.CreateAuthorization(new SqlAzManSID(WindowsIdentity.GetCurrent().User), WhereDefined.Local,
                    new SqlAzManSID(WindowsIdentity.GetCurrent().User), WhereDefined.Local, AuthorizationType.Allow, null, null);
                Debug.WriteLine("Role "+ r.ToString()); 
                auth.CreateAttribute("key", "value");
                for (int t = 0; t < 100; t++)
                {
                    IAzManItem task = app.CreateItem("Task " + t.ToString() + " of Role " + r.ToString(), "", ItemType.Task);
                    role.AddMember(task);
                    for (int o = 0; o < 100; o++)
                    {
                        IAzManItem op = app.CreateItem("Operation " + o.ToString() + " of Task " + t.ToString() + " of Role " + r.ToString() , "", ItemType.Operation);
                        task.AddMember(op);
                    }
                }   
            }
            storage.CommitTransaction();
            storage.CloseConnection();
        }

        void ens_AuthorizationCreated(IAzManItem item, IAzManAuthorization authorizationCreated)
        {
            MessageBox.Show("created");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //var sc = new NetSqlAzMan.Cache.StorageCache("data source=.\\sql2005;Initial Catalog=NetSqlAzManStorage;Integrated Security=SSPI");
            //sc.BuildStorageCache();
            //List<KeyValuePair<String, String>> attributes;
            //var au = sc.CheckAccess("XXX",
            //    "Application", "X",
            //    WindowsIdentity.GetCurrent().GetUserBinarySSid(),
            //    WindowsIdentity.GetCurrent().GetGroupsBinarySSid(), 
            //    DateTime.Now, false, out attributes);


            //var au2 = sc.GetAuthorizedItems("XXXX",
            //    "X", 
            //    WindowsIdentity.GetCurrent().GetUserBinarySSid(),
            //    WindowsIdentity.GetCurrent().GetGroupsBinarySSid(),
            //    DateTime.Now);
        }
    }
}