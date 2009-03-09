using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Security.Principal;
using NetSqlAzMan;
using NetSqlAzMan.Interfaces;
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
