using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

public partial class NetSqlAzManRoleProviderTest : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        NetSqlAzMan.Providers.NetSqlAzManRoleProvider p = new NetSqlAzMan.Providers.NetSqlAzManRoleProvider();
        System.Collections.Specialized.NameValueCollection nvc = new System.Collections.Specialized.NameValueCollection();
        nvc.Add("connectionStringName", "NetSqlAzManConnectionString");
        nvc.Add("storeName", "Store Stress Test");
        nvc.Add("applicationName","Application0");
        nvc.Add("userLookupType", "LDAP");
        nvc.Add("defaultDomain", "EIDOS");
        p.Initialize("NetSqlAzManRoleProvider", nvc);
        //p.AddUsersToRoles(
        //    new string[] { "EIDOS\\A.ferendeles", "EIDOSIS4-AFR\\Administrator" } ,
        //    new string[] { "Role1", "Role2" });
        string s = p.ApplicationName;
        p.CreateRole("Ruolo di Prova");
        p.DeleteRole("Ruolo di prova", true);
        s = p.Description;
        //string[] roles = p.FindUsersInRole("Role0", "eidos\\a.ferendeles"); //NOT IMPLEMENTED
        string[] roles = p.GetAllRoles();
        roles = p.GetRolesForUser("EIDOS\\a.ferendeles");
        roles = p.GetRolesForUser("a.ferendeles");
        roles = p.GetRolesForUser("EIDOSIS4-AFR\\Administrator");
        string[] users = p.GetUsersInRole("Role5");
        bool isInRole = p.IsUserInRole("a.ferendeles", "Role9");
        p.RemoveUsersFromRoles(
            new string[] { "A.ferendeles", "EIDOSIS4-AFR\\Administrator" } ,
            new string[] { "Role1", "Role2" });
        bool roleExists = p.RoleExists("saaa");
    }
}
