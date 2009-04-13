using System;
using System.IO;
using System.Xml;
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
using NetSqlAzMan;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzManWebConsole
{
    public partial class dlgDBUsersList : dlgPage
    {
        protected internal IAzManStorage storage = null;
        internal IAzManApplication application;
        internal IAzManStore store;
        internal IAzManDBUser[] selectedDBUsers;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.setImage("DBUser_32x32.gif");
            this.setOkHandler(new EventHandler(this.btnOk_Click));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.storage = this.Session["storage"] as IAzManStorage;
            if (this.Session["storeGroup"] as IAzManStoreGroup!=null)
                this.store = ((IAzManStoreGroup)this.Session["storeGroup"]).Store;
            if (this.Session["application"] as IAzManApplication != null)
            {
                this.application = (IAzManApplication)this.Session["application"];
                this.store = this.application.Store;
            }
            this.Text = "DB Users List";
            this.Description = this.Text;
            this.Title = this.Text;
            if (!Page.IsPostBack)
            {
                //Filtering
                this.cmbFieldName.Items.Clear();
                this.cmbOperator.Items.Clear();
                this.cmbOperator.Items.AddRange(
                    new[] {
                    new ListItem("Is"), 
                    new ListItem("Is not"), 
                    new ListItem("Starts with"), 
                    new ListItem("Ends with"), 
                    new ListItem("Does not start with"), 
                    new ListItem("Does not end with"),
                    new ListItem("Contains"), 
                    new ListItem("Does not contain")
                });
                this.cmbOperator.SelectedIndex = 0;
                this.txtFieldValue.Text = String.Empty;

                this.RefreshDBUsersList();
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                List<IAzManDBUser> selectedDBUsers = new List<IAzManDBUser>();
                foreach (GridViewRow gvr in this.gvDBUsers.Rows)
                {
                    if (((CheckBox)gvr.FindControl("chkSelect")).Checked)
                    {
                        selectedDBUsers.Add(this.store.Storage.GetDBUser(HttpUtility.HtmlDecode(gvr.Cells[1].Text)));
                    }
                }
                this.selectedDBUsers = selectedDBUsers.ToArray();
                this.Session["selectedDBUsers"] = this.selectedDBUsers;
                this.closeWindow(true);
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }

        private void RefreshDBUsersList()
        {
            DataTable dtDBUsers = new DataTable("Database");
            dtDBUsers.Columns.Add("Select", typeof(bool));
            dtDBUsers.Columns.Add("Name", typeof(string));
            dtDBUsers.Columns.Add("SID", typeof(string));
            dtDBUsers.Columns.Add("Type", typeof(string));
            IAzManDBUser[] dbUsers = null;
            if (this.application != null)
                dbUsers = this.application.GetDBUsers();
            else if (this.store != null)
                dbUsers = this.store.GetDBUsers();
            else
                throw new System.InvalidOperationException("Store or Application must be not null. Please contact Developer team.");
            if (dbUsers.Length > 0)
            {
                if (!this.Page.IsPostBack)
                {
                    this.cmbFieldName.Items.Add("Name");
                    this.cmbFieldName.Items.Add("Custom Sid");
                }
                foreach (var customColumn in dbUsers[0].CustomColumns)
                {
                    dtDBUsers.Columns.Add(customColumn.Key, typeof(string));
                    if (!this.Page.IsPostBack)
                    {
                        BoundField boundField = new BoundField();
                        boundField.DataField = customColumn.Key;
                        boundField.HeaderText = customColumn.Key;
                        this.gvDBUsers.Columns.Add(boundField);
                        this.cmbFieldName.Items.Add(customColumn.Key);
                    }
                }
                if (!this.Page.IsPostBack)
                {
                    this.cmbFieldName.SelectedIndex = 0;
                }
            }
            foreach (IAzManDBUser dbUser in dbUsers)
            {
                if (this.isInFilter(dbUser, this.cmbFieldName.SelectedItem.ToString(), this.cmbOperator.SelectedItem.ToString(), this.txtFieldValue.Text))
                {
                    DataRow dr = dtDBUsers.NewRow();
                    dr["Select"] = false;
                    dr["Name"] = dbUser.UserName;
                    dr["SID"] = dbUser.CustomSid.StringValue;
                    dr["Type"] = "Database User";
                    foreach (var customColumn in dbUser.CustomColumns)
                    {
                        if (customColumn.Value != null && customColumn.Value != DBNull.Value)
                        {
                            dr[customColumn.Key] = customColumn.Value.ToString();
                        }
                        else
                        {
                            dr[customColumn.Key] = String.Empty;
                        }
                    }
                    dtDBUsers.Rows.Add(dr);
                }
            }
            this.gvDBUsers.DataSource = dtDBUsers;
            this.gvDBUsers.DataBind();
        }

        private bool isInFilter(IAzManDBUser dbuser, string fieldName, string filterOperator, string fieldValue)
        {
            if (fieldValue.Trim() == String.Empty)
                return true;
            //Left Value
            string leftValue = String.Empty;
            if (String.Compare(fieldName, "Name", true) == 0)
                leftValue = dbuser.UserName;
            else if (String.Compare(fieldName, "Custom Sid", true) == 0)
                leftValue = dbuser.CustomSid.StringValue;
            else
            {
                foreach (var customColumn in dbuser.CustomColumns)
                {
                    if (String.Compare(fieldName, customColumn.Key, true) == 0)
                    {
                        leftValue = customColumn.Value.ToString();
                        break;
                    }
                }
            }
            //Right Value
            string rightValue = fieldValue.Trim();
            //Operator
            if (filterOperator == "Is" && String.Compare(leftValue, rightValue, true) == 0) return true;
            if (filterOperator == "Is not" && String.Compare(leftValue, rightValue, true) != 0) return true;
            if (filterOperator == "Starts with" && leftValue.StartsWith(rightValue, StringComparison.CurrentCultureIgnoreCase)) return true;
            if (filterOperator == "Ends with" && leftValue.EndsWith(rightValue, StringComparison.CurrentCultureIgnoreCase)) return true;
            if (filterOperator == "Does not start with" && !leftValue.StartsWith(rightValue, StringComparison.CurrentCultureIgnoreCase)) return true;
            if (filterOperator == "Does not end with" && !leftValue.EndsWith(rightValue, StringComparison.CurrentCultureIgnoreCase)) return true;
            if (filterOperator == "Contains" && leftValue.IndexOf(rightValue, StringComparison.CurrentCultureIgnoreCase) != -1) return true;
            if (filterOperator == "Does not contain" && leftValue.IndexOf(rightValue, StringComparison.CurrentCultureIgnoreCase) == -1) return true;
            //otherwise
            return false;
        }


        protected void Filters_Changed(object sender, EventArgs e)
        {
            if (this.txtFieldValue.Text.Trim() != String.Empty)
                this.RefreshDBUsersList();
        }
    }
}
