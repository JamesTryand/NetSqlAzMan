using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.IO;
using System.DirectoryServices;
using System.Diagnostics;


namespace NetSqlAzManWebConsole
{
    [RunInstaller(true)]
    public partial class WebConsoleInstaller : System.Configuration.Install.Installer
    {
        public WebConsoleInstaller()
        {
            InitializeComponent();
        }

        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
        }

        protected override void OnAfterInstall(IDictionary savedState)
        {
            String targetSite = Context.Parameters["TargetSite"]; ///LM/W3SVC/1
            String targetVDir = Context.Parameters["TargetVDir"];
            String runtimeFolder = System.Runtime.InteropServices.RuntimeEnvironment.GetRuntimeDirectory();
            //"%windir%\Microsoft.NET\Framework\v4.0.30319\aspnet_regiis.exe -s W3SVC/" & virtualSite & "/Root/" & virtualDir
            Process registerClientScriptProcess = new Process();
            registerClientScriptProcess.StartInfo.FileName = Path.Combine(runtimeFolder, "aspnet_regiis.exe");
            registerClientScriptProcess.StartInfo.Arguments = String.Format("-s {0}/{1}/Root/{2}", targetSite.Split('/')[2], targetSite.Split('/')[3], targetVDir);
            registerClientScriptProcess.StartInfo.CreateNoWindow = true;
            registerClientScriptProcess.StartInfo.UseShellExecute = false;
            registerClientScriptProcess.Start();
            registerClientScriptProcess.WaitForExit();
            using (DirectoryEntry iisEntry = new DirectoryEntry(String.Format("IIS://localhost/{0}/{1}", targetSite.Split('/')[2], targetSite.Split('/')[3])))
            {
                PropertyValueCollection pvc = iisEntry.Properties["ServerBindings"];
                string ipAddress = String.Empty;
                string tcpIpPort = String.Empty;
                if (pvc.Count > 0)
                {
                    // Format is IPAddress:Port:HostHeader
                    string[] Bits = pvc[0].ToString().Split(':');
                    ipAddress = Bits[0];
                    tcpIpPort = Bits[1];
                }
                if (String.IsNullOrWhiteSpace(ipAddress))
                    ipAddress = "localhost";
                if (String.IsNullOrWhiteSpace(tcpIpPort))
                    tcpIpPort = "80";

                String programsFilesFolder = Context.Parameters["ProgramFilesFolder"];
                String linkPath = Path.Combine(programsFilesFolder, @".NET Sql Authorization Manager\Web Console\.NET Sql Authorization Manager - Web Console.url");
                using (StreamWriter sw = new StreamWriter(linkPath, false))
                {
                    sw.WriteLine("[InternetShortcut]");
                    sw.WriteLine(String.Format("URL=http://{0}:{1}/{2}/Default.aspx", ipAddress, tcpIpPort, targetVDir));
                    sw.WriteLine("IDList=");
                    sw.WriteLine("HotKey=0");
                    sw.WriteLine("[{000214A0-0000-0000-C000-000000000046}]");
                    sw.WriteLine("Prop3=19,2");
                }
            }
            base.OnAfterInstall(savedState);
        }

        public override void Uninstall(IDictionary savedState)
        {
            String programsFilesFolder = Context.Parameters["ProgramFilesFolder"];
            String linkPath = Path.Combine(programsFilesFolder, @".NET Sql Authorization Manager\Web Console\.NET Sql Authorization Manager - Web Console.url");
            if (File.Exists(linkPath))
                File.Delete(linkPath);
            base.Uninstall(savedState);
        }
    }
}
