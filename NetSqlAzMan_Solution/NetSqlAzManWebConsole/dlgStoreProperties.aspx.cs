using System;
using System.Web.UI;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzManWebConsole
{
    public partial class dlgStoreProperties : dlgPage
    {
        protected internal IAzManStorage storage = null;
        protected internal IAzManStore store = null;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.setImage("Store_32x32.gif");
            this.setOkHandler(new EventHandler(this.btnOk_Click));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.storage = this.Session["storage"] as IAzManStorage;
            if (this.Session["selectedObject"] as IAzManStore != null)
            {
                this.store = this.Session["selectedObject"] as IAzManStore;
            }
            if (!Page.IsPostBack)
            {
                if (this.store != null)
                {
                    this.btnPermissions.Enabled = true;
                    this.btnAttributes.Enabled = true;
                    this.txtName.Text = this.store.Name;
                    this.txtDescription.Text = this.store.Description;
                    this.txtName.Focus();
                    this.Text = "Store Properties - " + this.store.Name;
                    this.Title = this.Text;
                    this.Description = "Store Properties";
                    if (!this.store.IAmManager)
                        this.txtName.Enabled = this.txtDescription.Enabled = this.btnOk.Enabled = false;
                }
                else
                {
                    this.btnPermissions.Enabled = false;
                    this.btnAttributes.Enabled = false;
                    this.Description = "Create a New Store";
                    this.Text = "New Store";
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
                if (this.store == null)
                {
                    this.store = this.storage.CreateStore(this.txtName.Text, this.txtDescription.Text);
                    string suffix = String.Empty;
                    if (this.store.IAmAdmin) suffix = " (Admin)";
                    else if (this.store.IAmManager) suffix = " (Manager)";
                    else if (this.store.IAmUser) suffix = " (User)";
                    else if (this.store.IAmReader) suffix = " (Reader)";
                    this.Session["FindChildNodeText"] = this.store.Name+suffix;
                    this.closeWindow(true);
                }
                else
                {
                    this.storage.BeginTransaction(AzManIsolationLevel.ReadUncommitted);
                    this.store.Rename(this.txtName.Text.Trim());
                    this.store.Update(this.txtDescription.Text.Trim());
                    this.storage.CommitTransaction();
                    string suffix = String.Empty;
                    if (this.store.IAmAdmin) suffix = " (Admin)";
                    else if (this.store.IAmManager) suffix = " (Manager)";
                    else if (this.store.IAmUser) suffix = " (User)";
                    else if (this.store.IAmReader) suffix = " (Reader)";
                    this.Session["FindNodeText"] = this.store.Name+suffix;
                    this.closeWindow(true);
                }
            }
            catch (Exception ex)
            {
                if (this.store != null)
                    this.storage.RollBackTransaction();
                this.ShowError(ex.Message);
            }
        }
    }
}
