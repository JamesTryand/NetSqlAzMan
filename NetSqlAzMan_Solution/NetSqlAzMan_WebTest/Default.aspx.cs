using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security.Principal;
using Microsoft.Interop.Security.AzRoles;
using NetSqlAzMan;
using NetSqlAzMan.Interfaces;
using System.Threading;

public partial class _Default : System.Web.UI.Page 
{
    private const string AzManStorePath = @"msxml://c:\Store Test.xml";
    //private const string AzManStorePath = @"msldap://localhost:389/CN=AzMan, DC=AdamPartition";
    private const string NetSqlAzManStorePath = "data source=.;Initial Catalog=NetSqlAzManStorage;user id=sa;password=;";
    
    protected void Page_Load(object sender, EventArgs e)
    {
        string storeName = ((NetSqlAzMan.Providers.NetSqlAzManRoleProvider)Roles.Provider).Store.Name;
        //Dim storeName as String = Ctype(NetSqlAzMan.Providers.NetSqlAzManRoleProvider, Roles.Provider).Store.Name
        this.lblIM.Text = WindowsIdentity.GetCurrent().Name;
    }
    protected void btnAzManTestCheckAccess_Click(object sender, EventArgs e)
    {
        DateTime dtStart = DateTime.Now;
        int count = int.Parse(this.txtAzManTestCheckAccessCount.Text);
        if (!this.chkAzManMultiThread.Checked)
        {
            for (int i = 0; i < count; i++)
            {
                this.lblAzManCheckAccess.Text = this.AzManTestCheckAccess().ToString();
            }
        }
        else
        {
            Thread[] tt = new Thread[count];
            for (int i = 0; i < count; i++)
            {
                
                Thread t = new Thread(new ThreadStart(this.AzManTestCheckAccessMultiThread));
                t.Start();
                tt[i] = t;
            }
            for (int i = 0; i < count; i++)
            {
                tt[i].Join();
            }
        }
        DateTime dtEnd = DateTime.Now;
        double millisecons = ((TimeSpan)dtEnd.Subtract(dtStart)).TotalMilliseconds;
        this.txtAzManCheckAccessResults.Text = millisecons.ToString();
    }

    private void AzManTestCheckAccessMultiThread()
    {
        this.AzManTestCheckAccess();
    }

    private void NetSqlAzManCheckAccessMultiThread()
    {
        this.NetSqlAzManTestCheckAccess();
    }

    private void NetSqlAzManDirectCheckAccessMultiThread()
    {
        this.NetSqlAzManTestDirectCheckAccess();
    }

    private bool AzManTestCheckAccess()
    {
        WindowsIdentity identity = this.Request.LogonUserIdentity;
        string applicationName = "Application Test";
        string[] operations = new string[] { this.txtOperation.Text };
        HybridDictionary businessRuleParameters = new HybridDictionary();
        AzAuthorizationStoreClass store = new AzAuthorizationStoreClass();
        store.Initialize(0, AzManStorePath, null);
        IAzApplication azApp = store.OpenApplication(applicationName, null);
        IAzClientContext clientCtx = azApp.InitializeClientContextFromToken((UInt64)identity.Token, null);
        // costruisce il vettore dei valori e dei delle regole di business
        Object[] names = new Object[0];
        Object[] values = new Object[0];
        Object[] operationIds = new Object[operations.Length];
        for (Int32 index = 0; index < operations.Length; index++)
        {
            operationIds[index] = azApp.OpenOperation(operations[index], null).OperationID;
        }
        Object[] internalScopes = new Object[1];
        Object[] result = (Object[])clientCtx.AccessCheck("AuditString", internalScopes, operationIds, names, values, null, null, null);
        foreach (Int32 accessAllowed in result)
        {
            if (accessAllowed != 0)
            {
                return false;
            }
        }
        return true;
    }

    protected void btnNetSqlAzManTestCheckAccess_Click(object sender, EventArgs e)
    {
        DateTime dtStart = DateTime.Now;
        int count = int.Parse(this.txtNetSqlAzManTestCheckAccessCount.Text);
        if (!this.chkNetSqlAzManMultiThread.Checked)
        {
            for (int i = 0; i < count; i++)
            {
                this.lblNetSqlAzManCheckAccess.Text = this.NetSqlAzManTestCheckAccess().ToString();
            }
        }
        else
        {
            Thread[] tt = new Thread[count];
            for (int i = 0; i < count; i++)
            {
                Thread t = new Thread(new ThreadStart(this.NetSqlAzManCheckAccessMultiThread));
                t.Start();
                tt[i] = t;
            }
            for (int i = 0; i < count; i++)
            {
                tt[i].Join();
            }
        }
        DateTime dtEnd = DateTime.Now;
        double millisecons = ((TimeSpan)dtEnd.Subtract(dtStart)).TotalMilliseconds;
        this.txtNetSqlAzManCheckAccessResults.Text = millisecons.ToString();
    }

    private AuthorizationType NetSqlAzManTestCheckAccess()
    {
        WindowsIdentity userIdentity = this.Request.LogonUserIdentity;
        IAzManStorage storage = new SqlAzManStorage(NetSqlAzManStorePath);
        IAzManItem item = storage["Store Test"]["Application Test"][this.txtItem.Text];
        return item.CheckAccess(userIdentity, DateTime.Now);
    }

    private AuthorizationType NetSqlAzManTestDirectCheckAccess()
    {
        WindowsIdentity userIdentity = this.Request.LogonUserIdentity;
        IAzManStorage storage = new SqlAzManStorage(NetSqlAzManStorePath);
        return storage.CheckAccess("Store Test", "Application Test", this.txtDirectItem.Text, userIdentity, DateTime.Now, true);
    }

    protected void btnNetSqlAzManTestDirectCheckAccess_Click(object sender, EventArgs e)
    {
        DateTime dtStart = DateTime.Now;
        int count = int.Parse(this.txtNetSqlAzManTestDirectCheckAccessCount.Text);
        if (!this.chkNetSqlAzManDirectMultiThread.Checked)
        {
            for (int i = 0; i < count; i++)
            {
                this.lblNetSqlAzManDirectCheckAccess.Text = this.NetSqlAzManTestDirectCheckAccess().ToString();
            }
        }
        else
        {
            Thread[] tt = new Thread[count];
            for (int i = 0; i < count; i++)
            {
                Thread t = new Thread(new ThreadStart(this.NetSqlAzManDirectCheckAccessMultiThread));
                t.Start();
                tt[i] = t;
            }
            for (int i = 0; i < count; i++)
            {
                tt[i].Join();
            }
        }
        DateTime dtEnd = DateTime.Now;
        double millisecons = ((TimeSpan)dtEnd.Subtract(dtStart)).TotalMilliseconds;
        this.txtNetSqlAzManDirectCheckAccessResults.Text = millisecons.ToString();
    }

    protected void btnCheckAccess_Click(object sender, EventArgs e)
    {
        //Application0.Security.CheckAccessHelper chk = new Application0.Security.CheckAccessHelper("data source=.;Initial Catalog=NetSqlAzManStorage;Integrated Security=SSPI", this.Request.LogonUserIdentity);
        //SqlAzManItem.ClearBizRuleAssemblyCache();
        //this.txtCheckAccessResults.Text = chk.CheckAccess(Application0.Security.CheckAccessHelper.Operation.Operation0).ToString();
    }
}
