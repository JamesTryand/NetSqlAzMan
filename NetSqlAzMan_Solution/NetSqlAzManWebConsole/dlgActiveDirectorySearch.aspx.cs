using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Principal;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace NetSqlAzManWebConsole
{
    public partial class dlgActiveDirectorySearch : dlgPage
    {
        //[PreEmptive.Attributes.Feature("NetSqlAzMan WebConsole: Active Directory Search")]
        protected void Page_Init(object sender, EventArgs e)
        {
            this.setImage("ActiveDirectory.gif");
            this.setOkHandler(new EventHandler(this.btnOk_Click));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                this.Text = "Active Directory Search";
                this.Title = this.Text;
                this.Description = this.Text;
                if (!Page.IsPostBack)
                    this.RefreshActiveDirectoryObjectsList();
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
                List<ADObject> selectedADObjects = new List<ADObject>();
                foreach (GridViewRow gvr in this.gvLDAP.Rows)
                {
                    bool selected = ((CheckBox)gvr.Cells[0].FindControl("chkSelect")).Checked;
                    if (selected)
                    {
                        ADObject ado = new ADObject();
                        string accountName = gvr.Cells[1].Text.Trim(); //SAMAccountName
                        if (String.IsNullOrEmpty(accountName))
                            accountName = gvr.Cells[2].Text.Trim(); //Name
                        ado.Name = accountName;
                        ado.ClassName = gvr.Cells[3].Text;
                        ado.internalSid = new SecurityIdentifier(gvr.Cells[4].Text);
                        ado.ADSPath = gvr.Cells[5].Text;
                        selectedADObjects.Add(ado);
                    }
                }
                if (selectedADObjects.Count == 0)
                    selectedADObjects = null;
                this.Session["selectedADObjectsFromList"] = selectedADObjects;
                this.closeWindow(true);
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }

        private void RefreshActiveDirectoryObjectsList()
        {
            this.gvLDAP.DataSource = (DataView)this.Application["Active Directory List"];
            this.gvLDAP.DataBind();
        }
    }
}