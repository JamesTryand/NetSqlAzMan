using System;
using System.Configuration;

namespace NetSqlAzManWebConsole
{
    public partial class Default : ThemePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (ConfigurationManager.AppSettings["ShowSplashScreen"]=="true")
                Response.Redirect("Splash.aspx");
            else
                Response.Redirect("StorageConnection.aspx");
        }
    }
}
