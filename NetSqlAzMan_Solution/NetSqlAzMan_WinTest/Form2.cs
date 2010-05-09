using System;
using System.Security.Principal;
using System.Windows.Forms;
using NetSqlAzMan;

namespace NetSqlAzMan_WinTest
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            NetSqlAzManAuthorizationContext ctx = new NetSqlAzManAuthorizationContext(
                "data source=(local);Initial Catalog=NetSqlAzManStorage;User id=sa;password=",
                "Eidos",
                "DB Persone",
                WindowsIdentity.GetCurrent(),
                true);

            var auth = ctx.StorageCache.CheckAccess("Eidos", "DB Persone", "Gestore", ctx.Storage.GetDBUser("John").CustomSid.StringValue, DateTime.Now, false);
            MessageBox.Show(auth.ToString());
            //Optionally you can intercept events before and after the Access Check
            //ctx.BeforeCheckAccess += new BeforeCheckAccessHandler(NetSqlAzManAuthorizationContext_BeforeCheckAccess);
            //ctx.AfterCheckAccess += new AfterCheckAccessHandler(NetSqlAzManAuthorizationContext_AfterCheckAccess);

            //If using the Storage Cache … you can also invalidate the cache 
            //ctx.InvalidateCache();

            ctx.CheckSecurity(this);
            auth = ctx.Storage.CheckAccess("Eidos", "DB Persone", "Gestore", WindowsIdentity.GetCurrent(), DateTime.Now, false);
            MessageBox.Show(auth.ToString());
        }

        //void NetSqlAzManAuthorizationContext_AfterCheckAccess(NetSqlAzManAuthorizationContext context, NetSqlAzManAuthorizationAttribute attribute, ref bool partialResult)
        //{
        //    //Do something before checking the access
        //}

        //void NetSqlAzManAuthorizationContext_BeforeCheckAccess(NetSqlAzManAuthorizationContext context, NetSqlAzManAuthorizationAttribute attribute)
        //{
        //    //Do something after access check
        //}
    }
}
