using System;

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
