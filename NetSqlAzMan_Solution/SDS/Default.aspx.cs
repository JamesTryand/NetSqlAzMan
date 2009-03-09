using System;
using System.Security.Principal;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using NetSqlAzMan;
using NetSqlAzMan.Interfaces;

public partial class _Default : System.Web.UI.Page 
{
    private WindowsIdentity identity;
    private IAzManApplication application;

    protected void Page_Load(object sender, EventArgs e)
    {
        IAzManStorage storage = new SqlAzManStorage(ConfigurationManager.ConnectionStrings["NetSqlAzManStorage"].ConnectionString);
        this.application = storage[ConfigurationManager.AppSettings["StoreName"]][ConfigurationManager.AppSettings["ApplicationName"]];
        //Get user Identity
        this.identity = this.Request.LogonUserIdentity;
        this.lblIAM.Text = this.identity.Name;
        //Print DateTime
        this.lblDateTime.Text = DateTime.Now.ToString();
        //Check Access on Items
        this.application.Store.Storage.OpenConnection();
        this.btnBudgetCheck.Enabled = this.checkAccessHelper("Controllo del Budget");
        this.btnCustomerRelationshipManagement.Enabled = this.checkAccessHelper("Relazioni con i Clienti");
        this.btnConstraintCheck.Enabled = this.checkAccessHelper("Controllo dei Vincoli");
        this.btnTimesheetCheck.Enabled = this.checkAccessHelper("Approvazione del TimeSheet");
        this.btnTimesheetCompile.Enabled = this.checkAccessHelper("Compilazione del Timesheet");
        this.btnDevelopment.Enabled = this.checkAccessHelper("Sviluppo");
        //Can delegate ?
        NTAccount delegatedNTAccount = new NTAccount("ProductManager1");
        SecurityIdentifier delegatedSid = (SecurityIdentifier)delegatedNTAccount.Translate(typeof(SecurityIdentifier));
        bool canDelegate = this.checkAccessForDelegationHelper("Controllo del Budget");
        bool alreadyDelegate = this.application["Controllo del Budget"].GetAuthorizations(new SqlAzManSID(this.identity.User), new SqlAzManSID(delegatedSid)).Length > 0;
        this.btnDelegateForBudgetCheck.Enabled = canDelegate && !alreadyDelegate;
        this.btnUndelegate.Enabled = canDelegate && alreadyDelegate;
        //Attributes
        IAzManAuthorization[] auths = this.application["Controllo del Budget"].GetAuthorizationsOfMember(new SqlAzManSID(this.identity.User));
        string toolTip = String.Empty;
        foreach (IAzManAuthorization auth in auths)
        {
            IAzManAttribute<IAzManAuthorization>[] attribs = auth.GetAttributes();
            foreach (IAzManAttribute<IAzManAuthorization> attrib in attribs)
            {
                toolTip += String.Format("{0} - {1}\r\n", attrib.Key, attrib.Value);
            }
        }
        this.btnBudgetCheck.ToolTip = toolTip;
        this.application.Store.Storage.CloseConnection();
    }

    private bool checkAccessHelper(string itemName)
    {
        AuthorizationType auth = this.application[itemName].CheckAccess(this.identity, DateTime.Now);
        return auth == AuthorizationType.Allow || auth == AuthorizationType.AllowWithDelegation;
    }

    private bool checkAccessForDelegationHelper(string itemName)
    {
        AuthorizationType auth = this.application[itemName].CheckAccess(this.identity, DateTime.Now);
        return auth == AuthorizationType.AllowWithDelegation;
    }

    protected void btnDelegateForBudgetCheck_Click(object sender, EventArgs e)
    {
        NTAccount delegatedNTAccount = new NTAccount("ProductManager1");
        SecurityIdentifier delegatedSid = (SecurityIdentifier)delegatedNTAccount.Translate(typeof(SecurityIdentifier));
        this.application.Store.Storage.OpenConnection();
        this.application.Store.Storage.BeginTransaction(AzManIsolationLevel.ReadUncommitted);
        IAzManAuthorization delegateAuthorization = this.application["Controllo del Budget"].CreateDelegateAuthorization(this.identity, new SqlAzManSID(delegatedSid), RestrictedAuthorizationType.Allow, null, null);
        delegateAuthorization.CreateAttribute("SomeBusinessAttribute", "Business profile data");
        this.application.Store.Storage.CommitTransaction();
        this.application.Store.Storage.CloseConnection();
        this.btnDelegateForBudgetCheck.Enabled = false;
        this.btnUndelegate.Enabled = true;
    }
    protected void btnUndelegate_Click(object sender, EventArgs e)
    {
        NTAccount delegatedNTAccount = new NTAccount("ProductManager1");
        SecurityIdentifier delegatedSid = (SecurityIdentifier)delegatedNTAccount.Translate(typeof(SecurityIdentifier));
        IAzManItem item = this.application["Controllo del Budget"];
        item.DeleteDelegateAuthorization(this.identity, new SqlAzManSID(delegatedSid), RestrictedAuthorizationType.Allow);
        this.btnDelegateForBudgetCheck.Enabled = true;
        this.btnUndelegate.Enabled = false;
    }
}
