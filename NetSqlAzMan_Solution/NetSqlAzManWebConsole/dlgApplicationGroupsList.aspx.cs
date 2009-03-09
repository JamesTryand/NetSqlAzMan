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
    public partial class dlgApplicationGroupsList : dlgPage
    {
        protected internal IAzManStorage storage = null;
        internal IAzManApplication application;
        internal IAzManApplicationGroup applicationGroup;
        internal IAzManApplicationGroup[] selectedApplicationGroups;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.setImage("StoreApplicationGroup_32x32.gif");
            this.setOkHandler(new EventHandler(this.btnOk_Click));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.storage = this.Session["storage"] as IAzManStorage;
            this.applicationGroup = this.Session["applicationGroup"] as IAzManApplicationGroup;
            this.application = this.Session["application"] as IAzManApplication;
            this.Text = "Application Groups List";
            this.Description = this.Text;
            this.Title = this.Text;
            if (!Page.IsPostBack)
            {
                this.RefreshApplicationList();
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                List<IAzManApplicationGroup> selectedApplicationGroups = new List<IAzManApplicationGroup>();
                foreach (GridViewRow gvr in this.gvApplicationGroups.Rows)
                {
                    if (((CheckBox)gvr.FindControl("chkSelect")).Checked)
                    {
                        selectedApplicationGroups.Add(this.application.GetApplicationGroup(HttpUtility.HtmlDecode(gvr.Cells[1].Text)));
                    }
                }
                this.selectedApplicationGroups = selectedApplicationGroups.ToArray();
                this.Session["selectedApplicationGroups"] = this.selectedApplicationGroups;
                this.closeWindow(true);
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }

        private void RefreshApplicationList()
        {
            DataTable dtApplicationGroups = new DataTable("Application Groups");
            dtApplicationGroups.Columns.Add("Select", typeof(bool));
            dtApplicationGroups.Columns.Add("Name", typeof(string));
            dtApplicationGroups.Columns.Add("Description", typeof(string));
            dtApplicationGroups.Columns.Add("Type", typeof(string));
            IAzManApplicationGroup[] applicationGroups = this.application.GetApplicationGroups();
            foreach (IAzManApplicationGroup applicationGroup in applicationGroups)
            {
                //Show all sids rather than owner, if owner is a Application Group
                if ((this.applicationGroup == null) || (this.applicationGroup != null && applicationGroup.SID.StringValue != this.applicationGroup.SID.StringValue))
                {
                    DataRow dr = dtApplicationGroups.NewRow();
                    dr["Select"] = false;
                    dr["Name"] = applicationGroup.Name;
                    dr["Description"] = applicationGroup.Description;
                    dr["Type"] = applicationGroup.GroupType == GroupType.Basic ? "Basic group" : "LDAP Query group";
                    dtApplicationGroups.Rows.Add(dr);
                }
            }
            this.gvApplicationGroups.DataSource = dtApplicationGroups;
            this.gvApplicationGroups.DataBind();
        }
    }
}
