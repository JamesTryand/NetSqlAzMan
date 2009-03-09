using System;
using System.IO;
using System.Xml;
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
    public partial class dlgDBUsersList : dlgPage
    {
        protected internal IAzManStorage storage = null;
        internal IAzManApplication application;
        internal IAzManStore store;
        internal IAzManDBUser[] selectedDBUsers;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.setImage("DBUser_32x32.gif");
            this.setOkHandler(new EventHandler(this.btnOk_Click));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.storage = this.Session["storage"] as IAzManStorage;
            if (this.Session["storeGroup"] as IAzManStoreGroup!=null)
                this.store = ((IAzManStoreGroup)this.Session["storeGroup"]).Store;
            if (this.Session["application"] as IAzManApplication != null)
            {
                this.application = (IAzManApplication)this.Session["application"];
                this.store = this.application.Store;
            }
            this.Text = "DB Users List";
            this.Description = this.Text;
            this.Title = this.Text;
            if (!Page.IsPostBack)
            {
                this.RefreshStoreList();
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                List<IAzManDBUser> selectedDBUsers = new List<IAzManDBUser>();
                foreach (GridViewRow gvr in this.gvDBUsers.Rows)
                {
                    if (((CheckBox)gvr.FindControl("chkSelect")).Checked)
                    {
                        selectedDBUsers.Add(this.store.Storage.GetDBUser(HttpUtility.HtmlDecode(gvr.Cells[1].Text)));
                    }
                }
                this.selectedDBUsers = selectedDBUsers.ToArray();
                this.Session["selectedDBUsers"] = this.selectedDBUsers;
                this.closeWindow(true);
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }

        private void RefreshStoreList()
        {
            DataTable dtDBUsers = new DataTable("Database");
            dtDBUsers.Columns.Add("Select", typeof(bool));
            dtDBUsers.Columns.Add("Name", typeof(string));
            dtDBUsers.Columns.Add("SID", typeof(string));
            dtDBUsers.Columns.Add("Type", typeof(string));
            IAzManDBUser[] dbUsers = null;
            if (this.application != null)
                dbUsers = this.application.GetDBUsers();
            else if (this.store != null)
                dbUsers = this.store.GetDBUsers();
            else
                throw new System.InvalidOperationException("Store or Application must be not null. Please contact Developer team.");

            foreach (IAzManDBUser dbUser in dbUsers)
            {
                DataRow dr = dtDBUsers.NewRow();
                dr["Select"] = false;
                dr["Name"] = dbUser.UserName;
                dr["SID"] = dbUser.CustomSid.StringValue;
                dr["Type"] = "Database User";
                dtDBUsers.Rows.Add(dr);
            }
            this.gvDBUsers.DataSource = dtDBUsers;
            this.gvDBUsers.DataBind();
        }
    }
}
