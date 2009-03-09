using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NetSqlAzMan;

namespace NetSqlAzManWCFServiceWinTest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void btnGetStorage_Click(object sender, EventArgs e)
        {
            using (NetSqlAzManSR.NetSqlAzManWCFServiceClient c = new NetSqlAzManWCFServiceWinTest.NetSqlAzManSR.NetSqlAzManWCFServiceClient())
            {
                c.Open();
                SqlAzManStorage storage = (SqlAzManStorage)c.CreateStorageInstance("data source=.;Initial Catalog=NetSqlAzManStorage;user id=sa;password=");
                var store = storage.GetStore("Eidos");
                store.CreateApplication("Prova", "");
                var apps = store.Applications;
            }
        }
    }
}
