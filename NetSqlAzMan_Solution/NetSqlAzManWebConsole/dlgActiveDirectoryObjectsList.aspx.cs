using System;
using System.Security.Principal;
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
using System.DirectoryServices;
using NetSqlAzMan;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzManWebConsole
{
    public partial class dlgActiveDirectoryObjectsList : dlgPage
    {
        private SearchResultCollection searchResultCollection;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.setImage("WindowsQueryLDAPGroup_32x32.gif");
            this.showCloseOnly();
            this.setOkHandler(new EventHandler(this.btnOk_Click));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            string ldapQuery = HttpUtility.HtmlDecode(this.Request["LDAPQuery"]);
            try
            {
                try
                {
                    this.searchResultCollection = DirectoryServicesWebUtils.ExecuteLDAPQuery(ldapQuery);
                }
                catch (System.Runtime.InteropServices.COMException cex)
                {
                    //http://brennan.offwhite.net/blog/2005/07/22/firefox-authentication-with-ntlm/
                    this.ShowError("NTLM Authentication failed.\r\nIf you are using a browser like FireFox type 'about:config' in the url and add to the 'network.negotiate-auth.trusted-uris' the <IIS-Server-Name>.\r\nFinally Close and re-open your browser.\r\n\r\nError:\r\n" + cex.Message);
                }
                this.Text = String.Format("LDap Query Result: {0}", Utility.QuoteJScriptString(ldapQuery, false));
                this.Title = this.Text;
                this.Description = this.Text;
                string nowaitpanel = this.Request["nowaitpanel"];
                if (String.IsNullOrEmpty(nowaitpanel))
                {
                    if (!Page.IsPostBack)
                    {
                        this.showWaitPanelNow(this.pnlWait, this.LDAPQueryResultPanel);
                        this.RegisterEndClientScript("window.location='" + this.Request.RawUrl + "&nowaitpanel=true'");
                    }
                }
                else if (nowaitpanel == "true")
                {
                    this.LDAPQueryResultPanel.Visible = true;
                    this.pnlWait.Visible = false;
                    if (!Page.IsPostBack)
                    {
                        this.RefreshActiveDirectoryObjectsList();
                    }
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                this.closeWindow(false);
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }

        private void RefreshActiveDirectoryObjectsList()
        {
            DataTable dtADList = new DataTable("Active Directory Objects List");
            dtADList.Columns.Add("sAMAccountName", typeof(string));
            dtADList.Columns.Add("Name", typeof(string));
            dtADList.Columns.Add("objectClass", typeof(string));
            dtADList.Columns.Add("objectSid", typeof(string));
            if (this.searchResultCollection != null)
            {
                foreach (SearchResult sr in this.searchResultCollection)
                {
                    DirectoryEntry de = sr.GetDirectoryEntry();
                    DataRow dr = dtADList.NewRow();
                    dr["sAMAccountName"] = (string)de.Properties["sAMAccountName"][0];
                    dr["Name"] = (string)de.InvokeGet("displayname");
                    dr["objectClass"] = de.SchemaClassName;
                    dr["objectSid"] = new SqlAzManSID((byte[])de.Properties["objectSid"].Value).StringValue;
                    dtADList.Rows.Add(dr);
                }
            }
            this.gvLDAPQueryResults.DataSource = dtADList;
            this.gvLDAPQueryResults.DataBind();
            this.EmptyGridFix(this.gvLDAPQueryResults);
        }
    }
}