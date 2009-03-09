using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

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
