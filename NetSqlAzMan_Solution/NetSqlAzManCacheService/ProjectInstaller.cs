using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration.Install;


namespace NetSqlAzMan.Cache.Service
{
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        public ProjectInstaller()
        {
            InitializeComponent();
        }

        //When you call installUtil to install your service you need specify /servicename="Name of your service" in command line. 
        //http://www.codeproject.com/KB/dotnet/MultipleInstNetWinService.aspx?print=true
        public override void Install(IDictionary stateSaver)
        {
            try
            {
                if (this.Context.Parameters.ContainsKey("servicename"))
                    this.serviceInstaller1.ServiceName = this.Context.Parameters["servicename"];
                if (this.Context.Parameters.ContainsKey("username") && this.Context.Parameters.ContainsKey("password"))
                {
                    if (!String.IsNullOrEmpty(this.Context.Parameters["username"]) && !String.IsNullOrEmpty(this.Context.Parameters["password"]))
                    {
                        this.serviceProcessInstaller1.Username = this.Context.Parameters["username"];
                        this.serviceProcessInstaller1.Password = this.Context.Parameters["password"];
                        return;
                    }
                }
                //Else pop-up for service credentials
                this.serviceProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.User;
                this.Context.Parameters["username"] = String.Empty;
                this.Context.Parameters["password"] = String.Empty;
                this.serviceProcessInstaller1.Username = String.Empty;
                this.serviceProcessInstaller1.Password = String.Empty;
            }
            finally
            {
                base.Install(stateSaver);
            }
        }

        public override void Uninstall(IDictionary savedState)
        {
            if (this.Context.Parameters["servicename"] != null)
                this.serviceInstaller1.ServiceName = this.Context.Parameters["servicename"];

            base.Uninstall(savedState);
        }
    }
}
