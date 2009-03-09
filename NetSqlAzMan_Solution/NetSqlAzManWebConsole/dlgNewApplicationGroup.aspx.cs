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
using NetSqlAzMan;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzManWebConsole
{
    public partial class dlgNewApplicationGroup : dlgPage
    {
        protected internal IAzManStorage storage = null;
        protected internal IAzManApplication application = null;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.setImage("StoreApplicationGroup_32x32.gif");
            this.setOkHandler(new EventHandler(this.btnOk_Click));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.storage = this.Session["storage"] as IAzManStorage;
            this.application = this.Session["selectedObject"] as IAzManApplication;
            if (!Page.IsPostBack)
            {
                this.txtName.Focus();
                this.Text = "New Application Group";
                this.Title = this.Text;
                this.Description = this.Text;
                this.txtName.Focus();
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                IAzManApplicationGroup applicationGroup = this.application.CreateApplicationGroup(SqlAzManSID.NewSqlAzManSid(), this.txtName.Text.Trim(), this.txtDescription.Text.Trim(), String.Empty, (this.rbtBasic.Checked ? GroupType.Basic : GroupType.LDapQuery));
                this.Session["FindChildNodeText"] = applicationGroup.Name;
                this.closeWindow(true);
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }
    }
}
