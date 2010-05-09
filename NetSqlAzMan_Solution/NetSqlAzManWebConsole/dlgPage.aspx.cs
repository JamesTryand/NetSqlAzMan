using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using NetSqlAzMan.Interfaces;


namespace NetSqlAzManWebConsole
{
    public partial class dlgPage : ThemePage
    {
        protected string menuItem;


        protected new void Page_PreInit(object sender, EventArgs e)
        {
            base.Page_PreInit(sender, e);
            #pragma warning disable 618
            this.Page.SmartNavigation = false;
            #pragma warning restore 618
        }

        private string getApplicationPath()
        {
            string result = this.Request.Url.AbsoluteUri;
            result = result.Substring(0, result.IndexOf(".aspx"));
            result = result.Substring(0, result.LastIndexOf('/'));
            return result;
        }

        protected internal void setImage(string imagePath)
        {
            ((ModalDialog)this.Master).imgIcon.Width = new Unit(32);
            ((ModalDialog)this.Master).imgIcon.Height = new Unit(32);
            ((ModalDialog)this.Master).imgIcon.ImageUrl = this.getApplicationPath() + "/images/" + imagePath;
        }

        protected string getImageUrl(string imagePath)
        {
            return this.getApplicationPath() + "/images/" + imagePath;
        }

        protected internal Button btnApply
        {
            get
            {
                return ((ModalDialog)this.Master).internalBtnApply;
            }
        }

        protected internal Button btnOk
        {
            get
            {
                return ((ModalDialog)this.Master).internalBtnOk;
            }
        }

        protected internal Button btnCancel
        {
            get
            {
                return ((ModalDialog)this.Master).internalBtnCancel;
            }
        }

        protected internal void setOkHandler(EventHandler handler)
        {
            ((ModalDialog)this.Master).internalBtnOk.Click += handler;
        }

        protected internal void setApplyHandler(EventHandler handler)
        {
            ((ModalDialog)this.Master).internalBtnApply.Click += handler;
        }

        private void showMessage(messageType type, string text)
        {
            string typePrefix = String.Empty;
            switch (type)
            {
                case messageType.Info: typePrefix = "Information:\r\n"; break;
                case messageType.Warning: typePrefix = "Warning:\r\n"; break;
                case messageType.Error: typePrefix = "Error:\r\n"; break;
            }
            this.Page.ClientScript.RegisterStartupScript(typeof(string), "postBackAlert", String.Format("window.alert('{0}');", Utility.QuoteJScriptString(typePrefix + text, false)), true);
        }

        protected internal void ShowInfo(string message)
        {
            this.showMessage(messageType.Info, message);
        }

        protected internal void ShowWarning(string message)
        {
            this.showMessage(messageType.Warning, message);
        }

        protected internal void ShowError(string message)
        {
            this.showMessage(messageType.Error, message);
        }

        protected internal void closeWindow(bool doPostBack)
        {
            this.Page.ClientScript.RegisterStartupScript(typeof(string), "close", "closewindow("+ (doPostBack ? "true" : "false") +");", true);
        }

        protected string Text
        {
            get
            {
                return this.Page.Title;
            }
            set
            {
                this.Page.Title = value;
            }
        }

        protected string Description
        {
            get
            {
                return ((ModalDialog)this.Master).lblDescription.Text;
            }
            set
            {
                ((ModalDialog)this.Master).lblDescription.Text = value;
            }
        }


        protected void showCloseOnly()
        {
            ((ModalDialog)this.Master).internalBtnOk.Visible = false;
            ((ModalDialog)this.Master).internalBtnCancel.Text = "Close";
        }

        protected void showOkCancelApply()
        {
            ((ModalDialog)this.Master).internalBtnOk.Visible = true;
            ((ModalDialog)this.Master).internalBtnCancel.Visible = true;
            ((ModalDialog)this.Master).internalBtnApply.Visible = true;
        }

        protected void showWaitPanelOnSubmit(Panel waitPanel, Panel panelToHide)
        {
            this.Page.ClientScript.RegisterOnSubmitStatement(typeof(string), "showWait", "if (typeof(ValidatorOnSubmit) == \"function\" && ValidatorOnSubmit() == false) return false;showHidePanel('" + waitPanel.ClientID + "', true);showHidePanel('" + panelToHide.ClientID + "', false);document.body.style.cursor = 'wait';");
        }

        protected void showWaitPanelNow(Panel waitPanel, Panel panelToHide)
        {
            this.RegisterEndClientScript("showHidePanel('" + waitPanel.ClientID + "', true);showHidePanel('" + panelToHide.ClientID + "', false);document.body.style.cursor = 'wait';");
        }

        protected void RegisterEndClientScript(string text)
        {
            Literal lit = ((ModalDialog)this.Page.Master).litEnd;
            string litStart = "<script type=\"text/javascript\" language=\"javascript\">";
            string litEnd = "</script>";
            if (lit.Text.StartsWith(litStart))
                lit.Text = lit.Text.Remove(0, litStart.Length);
            if (lit.Text.EndsWith(litEnd))
                lit.Text = lit.Text.Substring(0, lit.Text.Length-litEnd.Length);
            lit.Text += text+"\r\n";
            lit.Text = litStart+lit.Text+litEnd;
        }

        protected DataTable attributesToDataTable<T>(IAzManAttribute<T>[] attributes)
        {
            DataTable dt = new DataTable("Attributes");
            DataColumn dcKey = dt.Columns.Add("Key", typeof(string));
            DataColumn dcValue = dt.Columns.Add("Value", typeof(string));
            dt.Constraints.Add("PK_Key", dcKey, true);
            foreach (IAzManAttribute<T> attr in attributes)
            {
                DataRow dr = dt.NewRow();
                dr["Key"] = HttpUtility.HtmlEncode(attr.Key.Trim());
                dr["Value"] = HttpUtility.HtmlEncode(attr.Value.Trim());
                dt.Rows.Add(dr);
            }
            return dt;
        }

        protected DataTable itemsToDataTable(IAzManItem[] items)
        {
            DataTable dt = new DataTable("Items");
            dt.Columns.Add("Name", typeof(string));
            dt.Columns.Add("Description", typeof(string));
            foreach (IAzManItem item in items)
            {
                DataRow dr = dt.NewRow();
                dr["Name"] = HttpUtility.HtmlEncode(item.Name.Trim());
                dr["Value"] = HttpUtility.HtmlEncode(item.Description.Trim());
                dt.Rows.Add(dr);
            }
            return dt;
        }

        //http://www.codeproject.com/useritems/Fix_empty_GridView_issue.asp
        protected void EmptyGridFix(GridView grdView)
        {
            // normally executes after a grid load method
            if (grdView.Rows.Count == 0 && grdView.DataSource != null)
            {
                DataTable dt = null;
                // need to clone sources otherwise it will be indirectly adding to 
                // the original source
                if (grdView.DataSource is DataSet)
                    dt = ((DataSet)grdView.DataSource).Tables[0].Clone();
                else if (grdView.DataSource is DataTable)
                    dt = ((DataTable)grdView.DataSource).Clone();
                if (dt == null)
                    return;
                DataRow blankRow = dt.NewRow();
                foreach (DataColumn dc in blankRow.Table.Columns)
                {
                    if (!dc.AllowDBNull)
                    {
                        if (dc.DataType == typeof(Int32))
                        {
                            blankRow[dc] = 0;
                        }
                        else
                        {
                            blankRow[dc] = String.Empty;
                        }
                    }
                        
                }
                dt.Rows.Add(blankRow); // add empty row
                grdView.DataSource = dt;
                grdView.DataBind();

                // hide row
                grdView.Rows[0].Visible = false;
                grdView.Rows[0].Controls.Clear();
            }

            // normally executes at all postbacks
            if (grdView.Rows.Count == 1 &&
                grdView.DataSource == null)
            {
                bool bIsGridEmpty = true;

                // check first row that all cells empty
                for (int i = 0; i < grdView.Rows[0].Cells.Count; i++)
                {
                    if (grdView.Rows[0].Cells[i].Text != string.Empty)
                    {
                        bIsGridEmpty = false;
                    }
                }
                // hide row
                if (bIsGridEmpty)
                {
                    grdView.Rows[0].Visible = false;
                    grdView.Rows[0].Controls.Clear();
                }
            }
        }
        protected void reportMode(bool on)
        {
            this.btnOk.Visible = this.btnCancel.Visible = !on;
        }
    }
}
