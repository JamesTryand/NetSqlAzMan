using System;
using System.IO;
using System.Diagnostics;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using MMC = Microsoft.ManagementConsole;


namespace NetSqlAzMan.SnapIn
{
    [RunInstaller(true), System.ComponentModel.DesignTimeVisible(false)]
    public class NetSqlAzManSnapInInstaller : MMC.SnapInInstaller
    {
        private void InitializeComponent()
        {

        }
    }
}
