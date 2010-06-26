using System;
using System.Web.UI.WebControls;

namespace NetSqlAzManWebConsole
{
    public partial class dlgInvalidateWCFCacheService : dlgPage
    {
        [PreEmptive.Attributes.Feature("NetSqlAzMan WebConsole: Invalidate WCF Cache")]
        protected void Page_Init(object sender, EventArgs e)
        {
            this.setImage("Cache_32x32.gif");
            this.setOkHandler(new EventHandler(this.btnOk_Click));
            this.btnOk.Width = new Unit("150px");
            this.btnOk.Text = "InvalidateCache()";
            this.btnCancel.Text = "Close";
            using (wcf.CacheServiceClient csc = new NetSqlAzManWebConsole.wcf.CacheServiceClient())
            {
                this.TextBox1.Text = csc.Endpoint.Address.ToString();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.menuItem = Request["MenuItem"];
            this.Text = "Invalidate WCF Cache Service";
            this.Description = this.menuItem;
            this.Title = this.Text;
            this.showWaitPanelOnSubmit(this.pnlWait, this.pnlExport);
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                using (wcf.CacheServiceClient csc = new NetSqlAzManWebConsole.wcf.CacheServiceClient())
                {
                    csc.Endpoint.Address = new System.ServiceModel.EndpointAddress(this.TextBox1.Text);
                    csc.Open();
                    csc.InvalidateCache();
                    base.closeWindow(false);
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }
    }
}
