using System;
using System.Security.Principal;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using NetSqlAzMan;
using NetSqlAzMan.Cache;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzMan_WebTest2
{
    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Label1.Text = WindowsIdentity.GetCurrent().Name;
            NetSqlAzManAuthorizationContext ctx = new NetSqlAzManAuthorizationContext(
                "data source=(local);Initial Catalog=NetSqlAzManStorage;User id=sa;password=",
                "Eidos",
                "DB Persone",
                this.Request.LogonUserIdentity,
                false);
            ctx.CheckSecurity(this);
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            IAzManStorage storage = new SqlAzManStorage("data source=(local);Initial Catalog=NetSqlAzManStorage;user id=sa;password=");
            for (int i = 0; i < 30; i++)
            {
                //SqlAzManItem.ClearBizRuleAssemblyCache();

                this.TextBox1.Text = storage.CheckAccess("Store Stress Test", "Application0", "Role0", this.Request.LogonUserIdentity, DateTime.Now, false).ToString();
                this.TextBox1.Text += storage.CheckAccess("Store Stress Test", "Application0", "Operation0", this.Request.LogonUserIdentity, DateTime.Now, false).ToString();
            }
            //Application0.Security.CheckAccessHelper chk = new Application0.Security.CheckAccessHelper("data source=.;Initial Catalog=NetSqlAzManStorage;Integrated Security=SSPI", this.Request.LogonUserIdentity);
            
//            this.TextBox1.Text = chk.CheckAccess(Application0.Security.CheckAccessHelper.Operation.Operation0).ToString();
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            IAzManStorage storage = new SqlAzManStorage("data source=(local);Initial Catalog=NetSqlAzManStorage;user id=sa;password=");
            IAzManDBUser andrea = storage.GetDBUser("Andrea");
            UserPermissionCache cache = new UserPermissionCache(storage, "Store Stress Test", "Application0", andrea, true, true);
            Session["cache"] = cache;
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            UserPermissionCache cache = (UserPermissionCache)(Session["cache"]);
            System.Collections.Generic.List<System.Collections.Generic.KeyValuePair<string, string>> attrs;
            AuthorizationType a = cache.CheckAccess("Operation0", DateTime.Now, out attrs );
            this.TextBox1.Text = a.ToString();
        }
    }
}
