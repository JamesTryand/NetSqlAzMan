using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzManWebConsole
{
    public partial class dlgApplicationAttributes : dlgPage
    {
        protected internal IAzManStorage storage = null;
        protected internal IAzManApplication application = null;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.setImage("AuthorizationAttribute_32x32.gif");
            this.showCloseOnly();
            this.setOkHandler(new EventHandler(this.btnOk_Click));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.storage = this.Session["storage"] as IAzManStorage;
            this.application = this.Session["selectedObject"] as IAzManApplication;
            this.Text = String.Format("Application Attributes: {0}", this.application.Name);
            this.Title = this.Text;
            this.Description = this.Text;
            if (!Page.IsPostBack)
            {
                this.bindGridView();
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                //this.updateAttributes((DataTable)this.gvAttributes.DataSource, this.application);
                this.closeWindow(false);
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }

        private void bindGridView()
        {
            this.gvAttributes.DataSource = this.attributesToDataTable<IAzManApplication>(this.application.GetAttributes());
            this.gvAttributes.DataBind();
            if (!this.application.IAmManager)
            {
                this.gvAttributes.Columns[0].Visible = this.gvAttributes.Columns[1].Visible = false;
                this.gvAttributes.ShowFooter = false;
            }
            if (this.gvAttributes.EditIndex != -1)
            {
                if (this.gvAttributes.FooterRow != null)
                    ((ImageButton)this.gvAttributes.FooterRow.FindControl("imgNew")).Visible = false;
            }
            else
            {
                if (this.gvAttributes.FooterRow != null)
                    ((ImageButton)this.gvAttributes.FooterRow.FindControl("imgNew")).Visible = this.application.IAmManager;
            }
            this.EmptyGridFix(this.gvAttributes);
        }

        protected void gvAttributes_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                string key;
                if (this.gvAttributes.Rows[e.RowIndex].FindControl("lblKey") != null)
                    key = HttpUtility.HtmlDecode(((Label)((this.gvAttributes.Rows[e.RowIndex].FindControl("lblKey")))).Text);
                else
                {
                    key = HttpUtility.HtmlDecode(this.gvAttributes.Rows[0].Cells[2].Text).Trim();
                    this.gvAttributes.EditIndex = -1;
                    this.gvAttributes.Columns[2].Visible = false;
                }
                this.application.GetAttribute(key).Delete();
                this.bindGridView();
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }

        protected void gvAttributes_RowEditing(object sender, GridViewEditEventArgs e)
        {
            this.gvAttributes.Columns[2].Visible = true;
            this.gvAttributes.EditIndex = e.NewEditIndex;
            this.bindGridView();
        }

        protected void gvAttributes_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            try
            {
                string oldKey = HttpUtility.HtmlDecode(this.gvAttributes.Rows[this.gvAttributes.EditIndex].Cells[2].Text).Trim();
                string key = HttpUtility.HtmlDecode(((TextBox)((this.gvAttributes.Rows[this.gvAttributes.EditIndex].FindControl("txtKey")))).Text).Trim();
                string newValue = HttpUtility.HtmlDecode(((TextBox)((this.gvAttributes.Rows[this.gvAttributes.EditIndex].FindControl("txtValue")))).Text).Trim();
                this.gvAttributes.EditIndex = -1;
                this.application.GetAttribute(oldKey).Update(key, newValue);
                this.gvAttributes.Columns[2].Visible = false;
                this.bindGridView();
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }

        protected void gvAttributes_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            this.gvAttributes.EditIndex = -1;
            this.gvAttributes.Columns[2].Visible = false;
            ((ImageButton)this.gvAttributes.FooterRow.FindControl("imgNew")).Visible = this.application.IAmManager;
            this.bindGridView();
        }

        protected void imgNew_Click(object sender, ImageClickEventArgs e)
        {
            ((ImageButton)this.gvAttributes.FooterRow.FindControl("imgNew")).Visible = false;
            ((ImageButton)this.gvAttributes.FooterRow.FindControl("imgOk")).Visible = true;
            ((ImageButton)this.gvAttributes.FooterRow.FindControl("imgCancel")).Visible = true;
            ((TextBox)this.gvAttributes.FooterRow.FindControl("txtNewKey")).Visible = true;
            ((TextBox)this.gvAttributes.FooterRow.FindControl("txtNewValue")).Visible = true;
        }

        protected void imgCancel_Click(object sender, ImageClickEventArgs e)
        {
            ((ImageButton)this.gvAttributes.FooterRow.Cells[0].FindControl("imgNew")).Visible = true;
            ((ImageButton)this.gvAttributes.FooterRow.Cells[0].FindControl("imgOk")).Visible = false;
            ((ImageButton)this.gvAttributes.FooterRow.Cells[0].FindControl("imgCancel")).Visible = false;
            ((TextBox)this.gvAttributes.FooterRow.Cells[0].FindControl("txtNewKey")).Visible = false;
            ((TextBox)this.gvAttributes.FooterRow.Cells[0].FindControl("txtNewValue")).Visible = false;
            this.bindGridView();
        }

        protected void imgOk_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string key = HttpUtility.HtmlEncode(((TextBox)this.gvAttributes.FooterRow.Cells[0].FindControl("txtNewKey")).Text).Trim();
                string value = HttpUtility.HtmlEncode(((TextBox)this.gvAttributes.FooterRow.Cells[0].FindControl("txtNewValue")).Text).Trim();
                this.application.CreateAttribute(key, value);
                this.bindGridView();
                ((ImageButton)this.gvAttributes.FooterRow.Cells[0].FindControl("imgNew")).Visible = true;
                ((ImageButton)this.gvAttributes.FooterRow.Cells[0].FindControl("imgOk")).Visible = false;
                ((ImageButton)this.gvAttributes.FooterRow.Cells[0].FindControl("imgCancel")).Visible = false;
                ((TextBox)this.gvAttributes.FooterRow.Cells[0].FindControl("txtNewKey")).Visible = false;
                ((TextBox)this.gvAttributes.FooterRow.Cells[0].FindControl("txtNewValue")).Visible = false;
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }
    }
}