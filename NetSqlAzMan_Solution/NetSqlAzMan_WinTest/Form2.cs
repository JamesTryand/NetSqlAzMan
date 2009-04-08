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
                WindowsIdentity.GetCurrent());

            ctx.BeforeCheckAccess += new BeforeCheckAccessHandler(NetSqlAzManAuthorizationContext_BeforeCheckAccess);
            ctx.AfterCheckAccess += new AfterCheckAccessHandler(NetSqlAzManAuthorizationContext_AfterCheckAccess);
            ctx.CheckSecurity(this);

        }

        void NetSqlAzManAuthorizationContext_AfterCheckAccess(NetSqlAzManAuthorizationContext context, NetSqlAzManAuthorizationAttribute attribute, ref bool partialResult)
        {
            
        }

        void NetSqlAzManAuthorizationContext_BeforeCheckAccess(NetSqlAzManAuthorizationContext context, NetSqlAzManAuthorizationAttribute attribute)
        {
            //throw new NotImplementedException();
        }
    }
}
