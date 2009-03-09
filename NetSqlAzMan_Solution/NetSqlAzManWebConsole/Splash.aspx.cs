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
    public partial class Splash : ThemePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (this.Request["Redirect"] != null && this.Request["Redirect"] == "true")
            {
                Response.Redirect("StorageConnection.aspx");
            }

        }
    }
}
