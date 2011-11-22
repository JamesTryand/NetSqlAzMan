using System;
using System.Collections.Generic;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzManWebConsole
{
    public partial class dlgStoreGroupsList : dlgPage
    {
        protected internal IAzManStorage storage = null;
        internal IAzManStore store;
        internal IAzManStoreGroup storeGroup;
        internal IAzManStoreGroup[] selectedStoreGroups;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.setImage("StoreApplicationGroup_32x32.gif");
            this.setOkHandler(new EventHandler(this.btnOk_Click));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.storage = this.Session["storage"] as IAzManStorage;
            this.storeGroup = this.Session["storeGroup"] as IAzManStoreGroup;
            this.store = this.Session["store"] as IAzManStore;
            this.Text = "Store Groups List";
            this.Description = this.Text;
            this.Title = this.Text;
            if (!Page.IsPostBack)
            {
                this.RefreshStoreList();
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                List<IAzManStoreGroup> selectedStoreGroups = new List<IAzManStoreGroup>();
                foreach (GridViewRow gvr in this.gvStoreGroups.Rows)
                {
                    if (((CheckBox)gvr.FindControl("chkSelect")).Checked)
                    {
                        selectedStoreGroups.Add(this.store.GetStoreGroup(HttpUtility.HtmlDecode(gvr.Cells[1].Text)));
                    }
                }
                this.selectedStoreGroups = selectedStoreGroups.ToArray();
                this.Session["selectedStoreGroups"] = this.selectedStoreGroups;
                this.closeWindow(true);
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }

        private void RefreshStoreList()
        {
            DataTable dtStoreGroups = new DataTable("Store Groups");
            dtStoreGroups.Columns.Add("Select", typeof(bool));
            dtStoreGroups.Columns.Add("Name", typeof(string));
            dtStoreGroups.Columns.Add("Description", typeof(string));
            dtStoreGroups.Columns.Add("Type", typeof(string));
            IAzManStoreGroup[] storeGroups = this.store.GetStoreGroups();
            foreach (IAzManStoreGroup storeGroup in storeGroups)
            {
                //Show all sids rather than owner, if owner is a Store Group
                if ((this.storeGroup == null) || (this.storeGroup != null && storeGroup.SID.StringValue != this.storeGroup.SID.StringValue))
                {
                    DataRow dr = dtStoreGroups.NewRow();
                    dr["Select"] = false;
                    dr["Name"] = storeGroup.Name;
                    dr["Description"] = storeGroup.Description;
                    dr["Type"] = storeGroup.GroupType == GroupType.Basic ? "Basic group" : "LDAP Query group";
                    dtStoreGroups.Rows.Add(dr);
                }
            }
            this.gvStoreGroups.DataSource = dtStoreGroups;
            this.gvStoreGroups.DataBind();
        }
    }
}
