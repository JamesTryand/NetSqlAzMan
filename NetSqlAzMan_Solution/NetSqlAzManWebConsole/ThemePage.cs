using System;
using System.Web;
using NetSqlAzManWebConsole.Objects;

namespace NetSqlAzManWebConsole
{
    public class ThemePage : System.Web.UI.Page
    {
        protected void Page_PreInit(object sender, EventArgs e)
        {
            this.Theme = this.readThemeCookie();
        }

        private string readThemeCookie()
        {
            HttpCookie cookie = this.Request.Cookies["NetSqlAzManWebConsole-Theme"];
            if (cookie != null)
            {
                HttpCookie c;
                try
                {
                    c = HttpSecureCookie.Decode(cookie);
                }
                catch
                {
                    //Cookie tampered
                    return "Default";
                }
                if (c != null)
                {
                    return c["Theme"];
                }
            }
            return "Default";
        }
    }
}
