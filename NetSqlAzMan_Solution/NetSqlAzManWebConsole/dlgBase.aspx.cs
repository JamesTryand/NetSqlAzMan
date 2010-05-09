using System;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzManWebConsole
{
    public partial class dlgBase : dlgPage
    {
        protected internal IAzManStorage storage = null;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.setImage("NetSqlAzMan_32x32.gif");
            this.setOkHandler(new EventHandler(this.btnOk_Click));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.storage = this.Session["storage"] as IAzManStorage;
            this.menuItem = Request["MenuItem"];
            this.Text = ".NET Sql Authorization Manager Web Console";
            this.Description = this.Text;
            this.Title = this.Text;
            //this.showWaitPanelOnSubmit(this.pnlWait, this.pnlXXX);
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                this.closeWindow(true);
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }
    }
}
