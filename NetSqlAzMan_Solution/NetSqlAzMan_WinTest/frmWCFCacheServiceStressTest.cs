using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Security.Principal;
using NetSqlAzMan;
using NetSqlAzMan.Cache;
using NetSqlAzMan.Interfaces;
using System.Diagnostics;

namespace NetSqlAzMan_WinTest
{
    public partial class frmWCFCacheServiceStressTest : Form
    {
        private volatile int errors = 0;
        public frmWCFCacheServiceStressTest()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int max = 1000;
            this.errors = 0;
            this.progressBar1.Minimum = 0;
            this.progressBar1.Maximum = max - 1;
            for (int i=0;i<max;i++)
            {
                Thread t = new Thread(new ThreadStart(
                    delegate()
                    {
                        using (sr.CacheServiceClient csc = new NetSqlAzMan_WinTest.sr.CacheServiceClient())
                        {
                            try
                            {
                            csc.Open();
                            KeyValuePair<string, string>[] attrs = null;
                            
                                AuthorizationType auth = csc.CheckAccessForWindowsUsersWithoutAttributesRetrieve("Eidos", "DB Persone",
                                    "Gestore", WindowsIdentity.GetCurrent().GetUserBinarySSid(), WindowsIdentity.GetCurrent().GetGroupsBinarySSid(),
                                    DateTime.Now, false, null);
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.Message);
                                if (ex.InnerException!=null)
                                    MessageBox.Show(ex.InnerException.Message);
                                this.errors++;
                            }
                        }
                        Thread.Sleep(new Random().Next(300));
                    }));
                this.progressBar1.Value = i;
                Application.DoEvents();
                t.Start();
                Thread.Sleep(new Random().Next(100));
            }
            MessageBox.Show(this.errors.ToString());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using (sr.CacheServiceClient csc = new NetSqlAzMan_WinTest.sr.CacheServiceClient())
            {
                csc.Open();
                csc.InvalidateCache();
            }
        }
    }
}
