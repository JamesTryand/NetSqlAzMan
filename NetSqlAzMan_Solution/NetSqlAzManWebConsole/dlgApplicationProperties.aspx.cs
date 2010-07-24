using System;
using System.Web.UI;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzManWebConsole
{
    public partial class dlgApplicationProperties : dlgPage
    {
        protected internal IAzManStorage storage = null;
        protected internal IAzManStore store = null;
        protected internal IAzManApplication application = null;
        [PreEmptive.Attributes.Feature("NetSqlAzMan WebConsole: Application Properties")]
        protected void Page_Init(object sender, EventArgs e)
        {
            this.setImage("Application_32x32.gif");
            this.setOkHandler(new EventHandler(this.btnOk_Click));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.storage = this.Session["storage"] as IAzManStorage;
            if (this.Session["selectedObject"] as IAzManStore != null)
            {
                this.store = this.Session["selectedObject"] as IAzManStore;
            }
            if (this.Session["selectedObject"] as IAzManApplication != null)
            {
                this.application = this.Session["selectedObject"] as IAzManApplication;
            }
            if (!Page.IsPostBack)
            {
                if (this.application != null)
                {
                    this.btnPermissions.Enabled = true;
                    this.btnAttributes.Enabled = true;
                    this.txtName.Text = this.application.Name;
                    this.txtDescription.Text = this.application.Description;
                    this.txtName.Focus();
                    this.Text = "Application Properties - " + this.application.Name;
                    this.Title = this.Text;
                    this.Description = "Application Properties";
                    if (!this.application.IAmManager)
                        this.txtName.Enabled = this.txtDescription.Enabled = this.btnOk.Enabled = false;
                }
                else
                {
                    this.btnPermissions.Enabled = false;
                    this.btnAttributes.Enabled = false;
                    this.Description = "Create a New Application";
                    this.Text = "New Application";
                    this.Title = this.Text;
                }
                this.txtName.Focus();
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                //Create
                if (this.application == null)
                {
                    this.application = this.store.CreateApplication(this.txtName.Text, this.txtDescription.Text);
                    string suffix = String.Empty;
                    if (this.application.IAmAdmin) suffix = " (Admin)";
                    else if (this.application.IAmManager) suffix = " (Manager)";
                    else if (this.application.IAmUser) suffix = " (User)";
                    else if (this.application.IAmReader) suffix = " (Reader)";
                    this.Session["FindChildNodeText"] = this.application.Name + suffix;
                    this.closeWindow(true);
                }
                else
                {
                    this.storage.BeginTransaction(AzManIsolationLevel.ReadUncommitted);
                    this.application.Rename(this.txtName.Text.Trim());
                    this.application.Update(this.txtDescription.Text.Trim());
                    this.storage.CommitTransaction();
                    string suffix = String.Empty;
                    if (this.application.IAmAdmin) suffix = " (Admin)";
                    else if (this.application.IAmManager) suffix = " (Manager)";
                    else if (this.application.IAmUser) suffix = " (User)";
                    else if (this.application.IAmReader) suffix = " (Reader)";
                    this.Session["FindNodeText"] = this.application.Name + suffix;
                    this.closeWindow(true);
                }
            }
            catch (Exception ex)
            {
                if (this.application != null)
                    this.storage.RollBackTransaction();
                this.ShowError(ex.Message);
            }
        }
    }
}
