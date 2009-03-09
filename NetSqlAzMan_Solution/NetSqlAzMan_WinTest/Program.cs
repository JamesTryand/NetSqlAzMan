using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace NetSqlAzMan_WinTest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the store.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}