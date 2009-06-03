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
    public partial class dlgActiveDirectoryObjectPickUp : dlgPage
    {
        private List<ADObject> selectedAdObjects;
        private ADObject adoToResolve = null;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.setImage("WindowsQueryLDAPGroup_32x32.gif");
            this.showCloseOnly();
            this.setOkHandler(new EventHandler(this.btnOk_Click));
            this.btnOk.Text = "Cancel";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.selectedAdObjects = (List<ADObject>)this.Session["selectedADObjects"];
            foreach (ADObject ado in this.selectedAdObjects)
            {
                if (ado.state != ADObjectState.Resolved)
                {
                    this.adoToResolve = ado;
                    break;
                }
            }
            if (!Page.IsPostBack)
            {
                if (adoToResolve.state == ADObjectState.NotFound)
                {
                    this.lblMessage.Text = "Unknown Windows User/Group:";
                    this.Text = "Unable to find: " + adoToResolve.Name;
                    this.txtUnknow.Text = HttpUtility.HtmlEncode(this.adoToResolve.Name);
                    this.txtUnknow.Focus();
                }
                else if (adoToResolve.state == ADObjectState.Multiple)
                {
                    this.lblMessage.Text = "Ambiguous name:";
                    this.Text = "Ambiguous name: " + HttpUtility.HtmlEncode(this.adoToResolve.Name);
                    this.txtUnknow.Text = HttpUtility.HtmlEncode(this.adoToResolve.Name);
                    this.RefreshActiveDirectoryObjectsList();
                }
                this.Title = this.Text;
                this.Description = this.Text;
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                this.Session["selectedADObjects"] = null;
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
            dtADList.Columns.Add("Name", typeof(string));
            dtADList.Columns.Add("UPN", typeof(string));
            dtADList.Columns.Add("objectClass", typeof(string));
            dtADList.Columns.Add("ADSPath", typeof(string));
            dtADList.Columns.Add("Sid", typeof(string));
            List<ADObject> proposedADObjects = (List<ADObject>)this.Session["proposedADObjects"];
            foreach (ADObject proposal in proposedADObjects)
            {
                DataRow dr = dtADList.NewRow();
                dr["Name"] = proposal.Name;
                dr["UPN"] = proposal.UPN;
                dr["objectClass"] = proposal.ClassName;
                dr["ADSPath"] = proposal.ADSPath;
                dr["Sid"] = proposal.Sid;
                dtADList.Rows.Add(dr);
            }
            this.gvLDAPQueryResults.DataSource = dtADList;
            this.gvLDAPQueryResults.DataBind();
        }

        protected void gvLDAPQueryResults_SelectedIndexChanging(object sender, GridViewSelectEventArgs e)
        {
            List<ADObject> proposedADObjects = (List<ADObject>)this.Session["proposedADObjects"];
            string sid = this.gvLDAPQueryResults.Rows[e.NewSelectedIndex].Cells[5].Text;
            foreach (ADObject ado in proposedADObjects)
            {
                if (ado.Sid == sid)
                {
                    this.adoToResolve.Name = ado.Name;
                    if (String.IsNullOrEmpty(ado.Name))
                    {
                        this.adoToResolve.Name = sid;
                    }
                    this.adoToResolve.ClassName = ado.ClassName;
                    this.adoToResolve.ADSPath = ado.ADSPath;
                    this.adoToResolve.UPN = ado.UPN;
                    this.adoToResolve.state = ADObjectState.Resolved;
                    break;
                }
            }
            bool allResolved = true;
            foreach (ADObject ado in this.selectedAdObjects)
            {
                if (ado.state != ADObjectState.Resolved)
                {
                    allResolved = false;
                }
            } 
            this.callBackToTheOpener(allResolved);
            this.closeWindow(true);
        }

        protected void btnRemove_Click(object sender, EventArgs e)
        {
            this.selectedAdObjects.Remove(this.adoToResolve);
            bool allResolved = true;
            foreach (ADObject ado in this.selectedAdObjects)
            {
                if (ado.state != ADObjectState.Resolved)
                {
                    allResolved = false;
                }
            }
            this.callBackToTheOpener(allResolved);
            this.closeWindow(true);
        }

        private void callBackToTheOpener(bool allResolved)
        {
            this.RegisterEndClientScript("window.opener.allResolved=" + allResolved.ToString()+";");
        }

        protected void btnChange_Click(object sender, EventArgs e)
        {
            this.adoToResolve.Name = this.txtUnknow.Text;
            this.adoToResolve.state = ADObjectState.Resolved;
            bool allResolved = true;
            foreach (ADObject ado in this.selectedAdObjects)
            {
                if (ado.state != ADObjectState.Resolved)
                {
                    allResolved = false;
                }
            }
            this.callBackToTheOpener(allResolved);
            this.closeWindow(true);
        }
    }
}