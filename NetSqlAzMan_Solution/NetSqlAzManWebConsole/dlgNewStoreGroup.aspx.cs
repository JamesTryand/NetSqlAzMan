using System;
using System.Web.UI;
using NetSqlAzMan;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzManWebConsole
{
    public partial class dlgNewStoreGroup : dlgPage
    {
        protected internal IAzManStorage storage = null;
        protected internal IAzManStore store = null;
        [PreEmptive.Attributes.Feature("NetSqlAzMan WebConsole: New Store Group")]
        protected void Page_Init(object sender, EventArgs e)
        {
            this.setImage("StoreApplicationGroup_32x32.gif");
            this.setOkHandler(new EventHandler(this.btnOk_Click));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.storage = this.Session["storage"] as IAzManStorage;
            this.store = this.Session["selectedObject"] as IAzManStore;
            if (!Page.IsPostBack)
            {
                this.txtName.Focus();
                this.Text = "New Store Group";
                this.Title = this.Text;
                this.Description = this.Text;
                this.txtName.Focus();
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                IAzManStoreGroup storeGroup = this.store.CreateStoreGroup(SqlAzManSID.NewSqlAzManSid(), this.txtName.Text.Trim(), this.txtDescription.Text.Trim(), String.Empty, (this.rbtBasic.Checked ? GroupType.Basic : GroupType.LDapQuery));
                this.Session["FindChildNodeText"] = storeGroup.Name;
                this.closeWindow(true);
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }
    }
}
