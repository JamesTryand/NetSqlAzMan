using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NetSqlAzMan;
using NetSqlAzManWebConsole.Objects;

namespace NetSqlAzManWebConsole
{
    public partial class StorageConnection : ThemePage
    {
        [PreEmptive.Attributes.Feature("NetSqlAzMan WebConsole: Storage Connection")]
        protected void Page_Init(object sender, EventArgs e)
        { 
        
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.cmbDataSources.Items.Add(this.defaultDataSource);
                this.cmbDatabases.Items.Add(ConfigurationManager.AppSettings["Default Initial Catalog"]);
                this.rbAuthentication_CheckedChanged(this, EventArgs.Empty);
                this.readCookies();
                this.cmbDataSources.Focus();
            }
        }

        private string defaultDataSource
        {
            get
            {
                if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["Default Data Source"]))
                {
                    return Environment.MachineName;
                }
                else
                {
                    return ConfigurationManager.AppSettings["Default Data Source"];
                }
            }
        }

        private string[] GetDatabases(string connectionString)
        {
            SqlConnection sqlConnection = null;
            string[] databasesArray;
            StringCollection databasesCollection = new StringCollection();
            try
            {
                sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                string sqlversion = sqlConnection.ServerVersion;
                sqlversion = sqlversion.Split('.')[0]; //major version
                int intsqlversion = Convert.ToInt32(sqlversion);
                string sql = String.Empty;
                if (intsqlversion >= 9)
                    sql = "use master; select name from sys.databases order by name";
                else
                    sql = "use master; select name from sysdatabases order by name";
                SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
                while (sqlDataReader.Read())
                {
                    databasesCollection.Add(sqlDataReader["name"].ToString());
                }
                sqlDataReader.Close();
                databasesArray = new string[databasesCollection.Count];
                databasesCollection.CopyTo(databasesArray, 0);
                return databasesArray;
            }
            finally
            {
                if (sqlConnection != null && sqlConnection.State == ConnectionState.Open)
                    sqlConnection.Close();
            }

        }

        private string[] GetSqlDataSources()
        {
            while (this.Application["SqlDataSources"] == null)
            {
                System.Threading.Thread.Sleep(100);
            }
            return ((List<string>)this.Application["SqlDataSources"]).ToArray();
        }

        protected void btnRefreshDataSource_Click(object sender, EventArgs e)
        {
            try
            {
                this.RefreshDataSource();
                this.lblMessage.Text = String.Empty;
            }
            catch (Exception ex)
            {
                this.lblMessage.Text = ex.Message;
            }
        }

        private void RefreshDataSource()
        {
            string selectedText = this.cmbDataSources.Text;
            this.cmbDataSources.Items.Clear();
            int selectedIndex = -1;
            int index = -1;
            foreach (string server in this.GetSqlDataSources())
            {
                index++;
                this.cmbDataSources.Items.Add(server);
                if (String.Compare(selectedText, server, true) == 0)
                {
                    selectedIndex = index;
                }
            }
            this.cmbDataSources.SelectedIndex = selectedIndex;
        }

        protected void rbAuthentication_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbIntegrated.Checked)
            {
                this.txtUserId.Text = this.Request.LogonUserIdentity.Name;
                this.txtPassword.Text = String.Empty;
                this.txtUserId.ReadOnly = true;
                this.txtPassword.ReadOnly = true;
                this.RequiredUserIdValidator.Enabled = false;
            }
            else if (this.rbSql.Checked)
            {
                this.txtUserId.ReadOnly = false;
                this.txtPassword.ReadOnly = false;
                this.txtUserId.Text = String.Empty;
                this.RequiredUserIdValidator.Enabled = true;
                this.txtUserId.Focus();
            }
            this.cmbDatabases.Items.Clear();
            this.cmbDatabases.Items.Add(ConfigurationManager.AppSettings["Default Initial Catalog"]);
        }

        private void RefreshDatabases()
        {
            string currentdb = this.cmbDatabases.Text;
            this.cmbDatabases.Items.Clear();
            SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder();
            scsb.DataSource = this.cmbDataSources.Text;
            scsb.IntegratedSecurity = this.rbIntegrated.Checked;
            if (!scsb.IntegratedSecurity)
            {
                scsb.UserID = this.txtUserId.Text;
                scsb.Password = this.txtPassword.Text;
            }
            scsb.InitialCatalog = this.cmbDatabases.Text;
            string[] databases = this.GetDatabases(scsb.ConnectionString);
            foreach (string database in databases)
            {
                ListItem li = new ListItem(database);
                if (database.StartsWith(ConfigurationManager.AppSettings["Default Initial Catalog"], StringComparison.CurrentCultureIgnoreCase))
                {
                    li.Selected = true;
                }
                this.cmbDatabases.Items.Add(li);
            }
        }

        protected void btnRefreshDatabases_Click(object sender, EventArgs e)
        {
            try
            {
                this.RefreshDatabases();
                this.lblMessage.Text = String.Empty;
            }
            catch (Exception ex)
            {
                this.lblMessage.Text = ex.Message;
            }
        }

        private void TestConnection(string connectionString)
        {
            SqlAzManStorage.VerifyStorageDB(connectionString);
            SqlAzManStorage storage = new SqlAzManStorage(connectionString);
            string dbVersion = storage.DatabaseVesion;
            string netsqlazmanRunTimeVersion = storage.GetType().Assembly.GetName().Version.ToString();
            if (!String.Equals(netsqlazmanRunTimeVersion.Substring(0, 6), dbVersion.Substring(0, 6), StringComparison.InvariantCultureIgnoreCase))
            {
                this.Page.ClientScript.RegisterStartupScript(typeof(string), "postBackAlert",
                    String.Format("window.alert('{0}');", String.Format("Warning: Version mismatch !!!\\r\\nNetSqlAzMan Run-Time Version: {0}\r\nNetSqlAzMan Database Version: {1}", netsqlazmanRunTimeVersion, dbVersion)), true);
            }
        }

        protected void btnTestConnection_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder();
                scsb.DataSource = this.cmbDataSources.Text;
                scsb.IntegratedSecurity = this.rbIntegrated.Checked;
                if (!scsb.IntegratedSecurity)
                {
                    scsb.UserID = this.txtUserId.Text;
                    scsb.Password = this.txtPassword.Text;
                }
                scsb.InitialCatalog = this.cmbDatabases.Text;
                string connectionString = scsb.ConnectionString + ";" + this.txtOtherSettings.Text;
                this.TestConnection(connectionString);
                this.lblMessage.Text = "Connection Test Success !";
            }
            catch (Exception ex)
            {
                this.lblMessage.Text = ex.Message;
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            SqlConnectionStringBuilder scsb = new SqlConnectionStringBuilder();
            scsb.DataSource = this.cmbDataSources.Text;
            scsb.IntegratedSecurity = this.rbIntegrated.Checked;
            if (!scsb.IntegratedSecurity)
            {
                scsb.UserID = this.txtUserId.Text;
                scsb.Password = this.txtPassword.Text;
            }
            scsb.InitialCatalog = this.cmbDatabases.Text;
            string connectionString = scsb.ConnectionString + ";" + this.txtOtherSettings.Text;
            try
            {
                this.TestConnection(connectionString);
                this.Session["storage"] = new SqlAzManStorage(connectionString);
            }
            catch (Exception ex)
            {
                this.lblMessage.Text = ex.Message;
                return;
            }
            this.writeCookies();
            Response.Redirect("WebConsole.aspx");
        }

        protected void btnAddDataSource_Click(object sender, EventArgs e)
        {
            ListItem li = new ListItem(this.txtDataSource.Text);
            this.cmbDataSources.Items.Add(li);
            this.cmbDataSources.SelectedIndex = this.cmbDataSources.Items.Count - 1;
            this.txtDataSource.Text = String.Empty;
        }

        private void readCookies()
        {
            HttpCookie cookie = this.Request.Cookies["NetSqlAzManWebConsole"];
            if (cookie != null)
            {
                HttpCookie c;
                try
                {
                    c = HttpSecureCookie.Decode(cookie);
                }
                catch
                { 
                    //Cookie tampered
                    c = null;
                }
                if (c != null)
                {
                    bool remember = false;
                    if (bool.TryParse(c["RememberConnectionInfo"], out remember) && remember)
                    {
                        ListItem li = null;
                        foreach (ListItem li2 in this.cmbDataSources.Items)
                        {
                            if (String.Compare(li2.Text.Trim(), c["DataSource"].Trim(), true) == 0)
                            {
                                li = li2;
                            }
                        }
                        if (li == null)
                        {
                            li = new ListItem(c["DataSource"]);
                            this.cmbDataSources.Items.Add(li);
                        }
                        li.Selected = true;
                        this.rbIntegrated.Checked = bool.Parse(c["IntegratedAuthentication"]);
                        this.rbSql.Checked = !this.rbIntegrated.Checked;
                        this.rbAuthentication_CheckedChanged(this, EventArgs.Empty);
                        if (!this.rbIntegrated.Checked)
                        {
                            this.txtUserId.Text = c["UserId"];
                            this.txtPassword.Text = c["Password"];
                            this.txtPassword.Focus();
                        }
                        li = null;
                        foreach (ListItem li2 in this.cmbDatabases.Items)
                        {
                            if (String.Compare(li2.Text.Trim(), c["InitialCatalog"].Trim(), true) == 0)
                            {
                                li = li2;
                            }
                        }
                        if (li == null)
                        {
                            li = new ListItem(c["InitialCatalog"]);
                            this.cmbDatabases.Items.Add(li);
                        }
                        li.Selected = true;
                        this.txtOtherSettings.Text = c["OtherSettings"];
                        this.chkRemember.Checked = true;
                    }
                }
            }
        }

        private void writeCookies()
        {
            if (this.chkRemember.Checked)
            {
                HttpCookie c = new HttpCookie("NetSqlAzManWebConsole");
                c.Expires = DateTime.Now.AddDays(15);
                c["DataSource"] = this.cmbDataSources.SelectedValue;
                c["IntegratedAuthentication"] = this.rbIntegrated.Checked.ToString();
                c["UserId"] = this.txtUserId.Text;
                c["Password"] = this.txtPassword.Text;
                c["InitialCatalog"] = this.cmbDatabases.SelectedValue;
                c["OtherSettings"] = this.txtOtherSettings.Text;
                c["RememberConnectionInfo"] = this.chkRemember.Checked.ToString();
                HttpCookie encodedCookie = HttpSecureCookie.Encode(c);
                this.Response.Cookies.Add(encodedCookie);
            }
            else
            {
                HttpCookie c = this.Response.Cookies["NetSqlAzManWebConsole"];
                c.Expires = DateTime.Now.AddDays(-2);
            }
        }

        /// <summary>Re-populate the 'Password' field on postback</summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Password_PreRender(object sender, System.EventArgs e)
        {
            this.txtPassword.Attributes["value"] = this.txtPassword.Text;
        }
    }
}
