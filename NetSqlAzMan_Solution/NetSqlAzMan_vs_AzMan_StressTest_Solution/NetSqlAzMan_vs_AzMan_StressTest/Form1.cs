using System;
using System.Threading;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Security.Principal;
using Microsoft.Interop.Security.AzRoles;
using NetSqlAzMan;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzMan_vs_AzMan_StressTest
{
    public partial class frmTest : Form
    {
        private DateTime dtStart;
        private DateTime dtEnd;
        public frmTest()
        {
            InitializeComponent();
        }

        private void StartTimer()
        {
            this.dtStart = DateTime.Now;
            this.txtStart.Text = this.dtStart.ToString();
            Application.DoEvents();
        }

        private void StopTimer(TextBox txtElapsed)
        {
            this.dtEnd = DateTime.Now;
            this.txtStop.Text = this.dtEnd.ToString();
            TimeSpan ts = this.dtEnd.Subtract(this.dtStart);
            txtElapsed.Text = ts.TotalMilliseconds.ToString();
            Application.DoEvents();
        }

        private void Clessidra(bool accesa)
        {
            this.Cursor = accesa ? Cursors.WaitCursor : Cursors.Arrow;
            Application.DoEvents();
        }

        private void btnCreaAzMan_Click(object sender, EventArgs e)
        {
            this.CreaStrutturaSuAzMan(this.txtAzManStorePath.Text, int.Parse(this.txtUnita.Text));
        }


        private void btnCreaNetSqlAzMan_Click(object sender, EventArgs e)
        {
            this.CreaStrutturaSuNetSqlAzMan(this.txtNetSqlAzManConnectionString.Text, int.Parse(this.txtUnita.Text));
        }

        private void btnTestAzMan_Click(object sender, EventArgs e)
        {
            this.Clessidra(true);
            this.StartTimer();
            int n = int.Parse(this.txtAzManThreads.Text);
            int max = int.Parse(this.txtUnita.Text);
            this.pb.Maximum = n - 1;
            if (!this.chkAzManMultiThread.Checked)
            {
                for (int i = 0; i < n; i++)
                {
                    this.TestSuAzMan(this.txtAzManStorePath.Text, max);
                    this.pb.Value = i;
                    Application.DoEvents();
                }
            }
            else
            {
                Thread[] tt = new Thread[n];
                //Threads Prepare
                for (int i = 0; i < n; i++)
                {
                    tt[i] = new Thread(new ThreadStart(this.TestSuAzManForThread));
                    tt[i].Start();
                    this.pb.Value = i;
                }
                //Threads Consume
                for (int i = 0; i < n; i++)
                {
                    tt[i].Join();
                    this.pb.Value = i;
                }
                //Threads Destroy
                for (int i = 0; i < n; i++)
                {
                    tt[i] = null;
                    this.pb.Value = i;
                }
            }
            this.StopTimer(this.txtAzManElapsed);
            this.Clessidra(false);
        }

        private void TestSuAzManForThread()
        {
            int max = 20; // int.Parse(this.txtUnita.Text);
            //this.TestSuAzMan(this.txtAzManStorePath.Text, max);
            this.TestSuAzMan(@"msldap://localhost:389/CN=AzMan, CN=EIDOSIS4-AFRAdam", max);
        }

        private void TestSuNetSqlAzManForThread()
        {
            int max = int.Parse(this.txtUnita.Text);
            this.TestSuNetSqlAzMan(this.txtNetSqlAzManConnectionString.Text, max);
        }

        private void btnTestNetSqlAzMan_Click(object sender, EventArgs e)
        {
            this.Clessidra(true);
            this.StartTimer();
            int n = int.Parse(this.txtNetSqlAzManThreads.Text);
            int max = int.Parse(this.txtUnita.Text);
            this.pb.Maximum = n - 1;
            if (!this.chkNetSqlAzManMultiThread.Checked)
            {
                if (!this.chkCache.Checked)
                {
                    for (int i = 0; i < n; i++)
                    {
                        this.TestSuNetSqlAzMan(this.txtNetSqlAzManConnectionString.Text, max);
                        this.pb.Value = i;
                        Application.DoEvents();
                    }
                }
                else
                {
                    WindowsIdentity id = WindowsIdentity.GetCurrent();
                    IAzManStorage storage = new SqlAzManStorage(this.txtNetSqlAzManConnectionString.Text);
                    storage.OpenConnection();
                    int rnd = new Random().Next(max);
                    NetSqlAzMan.Cache.UserPermissionCache c = new NetSqlAzMan.Cache.UserPermissionCache(storage, "Store Stress Test", "Application" + rnd.ToString(), id, true, true);
                    for (int i = 0; i < n; i++)
                    {
                        List<KeyValuePair<string, string>> attr;
                        AuthorizationType res = c.CheckAccess("Operation" + rnd.ToString(), DateTime.Now, out attr);
                        this.pb.Value = i;
                        Application.DoEvents();
                    }
                    storage.CloseConnection();
                    storage.Dispose();
                }
            }
            else
            {
                Thread[] tt = new Thread[n];
                //Threads Prepare
                for (int i = 0; i < n; i++)
                {
                    tt[i] = new Thread(new ThreadStart(this.TestSuNetSqlAzManForThread));
                    tt[i].Start();
                    this.pb.Value = i;
                }
                //Threads Consume
                for (int i = 0; i < n; i++)
                {
                    tt[i].Join();
                    this.pb.Value = i;
                }
                //Threads Destroy
                for (int i = 0; i < n; i++)
                {
                    tt[i] = null;
                    this.pb.Value = i;
                }
            }
            this.StopTimer(this.txtNetSqlAzManElapsed);
            this.Clessidra(false);
        }

        private void CreaStrutturaSuAzMan(string azManStorePath, int n)
        {
            this.Clessidra(true);
            this.StartTimer();
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            NTAccount userName = new NTAccount(id.Name);
            IAzAuthorizationStore store = new AzAuthorizationStoreClass();
            store.Initialize(0, azManStorePath, null);
            object o = null;
            this.pb.Maximum = n - 1;
            for (int a = 0; a < n; a++)
            {
                IAzApplication app = store.CreateApplication("Application" + a.ToString(), null);
                app.Submit(0, null);
                this.pb.Value = a;
                Application.DoEvents();
                //IAzClientContext ctx = app.InitializeClientContextFromToken((UInt64)id.Token, null);
                for (int i = 0; i < n; i++)
                {
                    IAzOperation op = app.CreateOperation("Operation" + i.ToString(), o);
                    op.OperationID = i + 1;
                    op.Submit(0, null);
                    IAzTask task = app.CreateTask("Task" + i.ToString(), null);
                    task.AddOperation(op.Name, null);
                    task.Submit(0, null);
                    IAzTask roleTask = app.CreateTask("Role" + i.ToString(), null);
                    roleTask.IsRoleDefinition = 1;
                    roleTask.AddTask("Task" + i.ToString(), null);
                    roleTask.Submit(0, null);
                    IAzRole role = app.CreateRole("Role" + i.ToString(), null);
                    role.AddTask("Role" + i.ToString(), null);
                    role.AddMember(id.User.Value, null); //add current user
                    role.Submit(0, null);
                }
            }
            this.StopTimer(this.txtAzManElapsed);
            this.Clessidra(false);
        }

        private void CreaStrutturaSuNetSqlAzMan(string connectionString, int n)
        {
            this.Clessidra(true);
            this.StartTimer();
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            IAzManStorage storage = new SqlAzManStorage(connectionString);
            storage.ENS.AuthorizationCreated += new NetSqlAzMan.ENS.AuthorizationCreatedDelegate(ENS_AuthorizationCreated);
            try
            {
                IAzManStore s = storage["Store Stress Test"];
                if (s != null)
                    s.Delete();
            }
            catch { }
            storage.OpenConnection();
            storage.BeginTransaction(AzManIsolationLevel.ReadUncommitted);
            IAzManStore store = storage.CreateStore("Store Stress Test", String.Empty);
            this.pb.Maximum = n - 1;
            for (int a = 0; a < n; a++)
            {
                IAzManApplication app = store.CreateApplication("Application" + a.ToString(), String.Empty);
                this.pb.Value = a;
                Application.DoEvents();
                for (int i = 0; i < n; i++)
                {
                    IAzManItem role = app.CreateItem("Role" + i.ToString(), String.Empty, ItemType.Role);
                    IAzManItem task = app.CreateItem("Task" + i.ToString(), String.Empty, ItemType.Task);
                    IAzManItem op = app.CreateItem("Operation" + i.ToString(), String.Empty, ItemType.Operation);
                    role.AddMember(task);
                    task.AddMember(op);
                    role.CreateAuthorization(new SqlAzManSID(id.User), WhereDefined.LDAP, new SqlAzManSID(id.User), WhereDefined.LDAP, AuthorizationType.Allow, null, null); //add current Windows user
                    //role.CreateAuthorization(new SqlAzManSID(id.User), WhereDefined.LDAP, new SqlAzManSID(storage.GetDBUser("Andrea").CustomSid.BinaryValue, true), WhereDefined.Database, AuthorizationType.Allow, null, null); //add Andrea DB User
                }
            }
            //storage.RollBackTransaction();
            storage.CommitTransaction();
            storage.CloseConnection();
            this.StopTimer(this.txtNetSqlAzManElapsed);
            this.Clessidra(false);
        }

        void ENS_AuthorizationCreated(IAzManItem item, IAzManAuthorization authorizationCreated)
        {
            System.Diagnostics.Debug.WriteLine(authorizationCreated.Item.Name);
        }

        private void TestSuAzMan(string azManStorePath, int max)
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            IAzAuthorizationStore store = new AzAuthorizationStoreClass();
            store.Initialize(0, azManStorePath, null);
            int rnd = 0; // new Random().Next(max);
            IAzApplication app = store.OpenApplication("Application" + rnd.ToString(), null);
            IAzClientContext ctx = app.InitializeClientContextFromToken((ulong)id.Token.ToInt64(), null);
            string opName = "Operation" + rnd.ToString();
            IAzOperation op = app.OpenOperation(opName, null);
            object[] parameterNames = new object[1] { "chiave" };
            object[] parameterValues = new object[1] { "valore" };
            object[] oRes = (object[])ctx.AccessCheck("Test", null, new object[] { op.OperationID }, parameterNames, parameterValues, null, null, null);
            foreach (int accessAllowed in oRes)
            {
                if (accessAllowed != 0)
                {
                    break;
                }
            }
            store.CloseApplication("Application" + rnd.ToString(), 0);
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(op);
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(app);
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(store);
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(ctx);
            op = null;
            ctx = null;
            app = null;
            store = null;
        }

        private void TestSuNetSqlAzMan(string connectionString, int max)
        {
            WindowsIdentity id = WindowsIdentity.GetCurrent();
            int rnd = new Random().Next(max);
            IAzManStorage storage = new SqlAzManStorage(connectionString);
            storage.OpenConnection();
            AuthorizationType res = storage.CheckAccess("Store Stress Test", "Application" + rnd.ToString(), "Operation" + rnd.ToString(), id, DateTime.Now, true, new KeyValuePair<string, object>("chiave","valore"));
            //AuthorizationType res = storage.CheckAccess("Store Stress Test", "Application" + rnd.ToString(), "Operation" + rnd.ToString(), storage.GetDBUser("Andrea"), DateTime.Now, true, new KeyValuePair<string, object>("chiave", "valore"));
            storage.CloseConnection();
            storage.Dispose();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            NetSqlAzMan.SnapIn.frmSplash frm = new NetSqlAzMan.SnapIn.frmSplash();
            frm.Show();

            //IAzManStorage storage = new SqlAzManStorage(this.txtNetSqlAzManConnectionString.Text);
            //IAzManStore[] stores = storage.GetStores();
            //MessageBox.Show(storage.LogErrors.ToString());
            //MessageBox.Show(storage.LogWarnings.ToString());
            //MessageBox.Show(storage.LogInformations.ToString());
            //MessageBox.Show(storage.Mode.ToString());
        }




    }
}