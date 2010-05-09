using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzManWebConsole
{
    public partial class dlgItemsList : dlgPage
    {
        internal IAzManApplication application;
        internal IAzManItem item;
        internal ItemType itemType;
        internal IAzManItem[] selectedItems;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.setOkHandler(new EventHandler(this.btnOk_Click));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.item = (IAzManItem)this.Session["item"];
            this.itemType = (ItemType)this.Session["itemType"];
            this.application = this.item.Application;

            switch (this.itemType)
            {
                case ItemType.Role:
                    this.setImage("Role_32x32.gif");
                    this.Text = "Roles list";
                    break;
                case ItemType.Task:
                    this.setImage("Task_32x32.gif");
                    this.Text = "Tasks list";
                    break;
                case ItemType.Operation:
                    this.setImage("Operation_32x32.gif");
                    this.Text = "Operations list";
                    break;
            }
            this.Description = this.Text;
            this.Title = this.Text;
            if (!Page.IsPostBack)
            {
                this.RefreshItemsList();
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                List<IAzManItem> selectedItems = new List<IAzManItem>();
                foreach (GridViewRow gvr in this.gvItemsList.Rows)
                {
                    if (((CheckBox)gvr.FindControl("chkSelect")).Checked)
                    {
                        selectedItems.Add(this.application.GetItem(HttpUtility.HtmlDecode(gvr.Cells[1].Text)));
                    }
                }
                this.selectedItems = selectedItems.ToArray();
                this.Session["selectedItems"] = this.selectedItems;
                this.closeWindow(true);
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }

        private void RefreshItemsList()
        {
            DataTable dtItemsList = new DataTable("Items List");
            dtItemsList.Columns.Add("Select", typeof(bool));
            dtItemsList.Columns.Add("Name", typeof(string));
            dtItemsList.Columns.Add("Description", typeof(string));
            IAzManItem[] members = this.application.GetItems(this.itemType);
            foreach (IAzManItem member in members)
            {
                //Show all sids rather than owner, if owner is a Store Group
                if ((this.item == null) || (this.item != null && member.Name != this.item.Name))
                {
                    DataRow dr = dtItemsList.NewRow();
                    dr["Select"] = false;
                    dr["Name"] = member.Name;
                    dr["Description"] = member.Description;
                    dtItemsList.Rows.Add(dr);
                }
            }
            this.gvItemsList.DataSource = dtItemsList;
            this.gvItemsList.DataBind();
        }
    }
}
