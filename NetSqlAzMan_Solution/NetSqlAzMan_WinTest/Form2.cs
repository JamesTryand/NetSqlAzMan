using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetSqlAzMan;
using System.Security.Principal;

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

            //Optionally you can intercept events before and after the Access Check
            //ctx.BeforeCheckAccess += new BeforeCheckAccessHandler(NetSqlAzManAuthorizationContext_BeforeCheckAccess);
            //ctx.AfterCheckAccess += new AfterCheckAccessHandler(NetSqlAzManAuthorizationContext_AfterCheckAccess);

            //If using the Storage Cache … you can also invalidate the cache 
            //ctx.InvalidateCache();

            ctx.CheckSecurity(this);
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
