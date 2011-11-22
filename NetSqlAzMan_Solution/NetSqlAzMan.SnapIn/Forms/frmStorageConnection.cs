using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace NetSqlAzMan.SnapIn.Forms
{
    public partial class frmStorageConnection : frmBase
    {
        protected internal string dataSource;
        protected internal string initialCatalog;
        protected internal string security;
        protected internal string userId;
        protected internal string password;
        protected internal string otherSettings;

        public frmStorageConnection()
        {
            InitializeComponent();
        }
        protected void HourGlass(bool switchOn)
        {
            this.Cursor = switchOn ? Cursors.WaitCursor : Cursors.Arrow;
            /*Application.DoEvents();*/
        }

        protected void ShowError(string text, string caption)
        {
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        protected void ShowInfo(string text, string caption)
        {
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        protected void ShowWarning(string text, string caption)
        {
            MessageBox.Show(text, caption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void frmStorageManagement_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.None)
                this.DialogResult = DialogResult.Cancel;
        }

        internal static string ConstructConnectionString(
            string dataSource,
            string initialCatalog,
            bool integratedSecurity,
            string userId,
            string password,
            string otherSettings)
        {
            return String.Format("Data Source={0};Initial Catalog={1};{2};{3};{4};{5}",
                dataSource,
                initialCatalog,
                (integratedSecurity ? "Integrated Security=SSPI" : String.Empty),
                (integratedSecurity ? String.Empty : String.Format("User Id={0}", userId)),
                (integratedSecurity ? String.Empty : String.Format("Password={0}", password)),
                otherSettings).TrimEnd(';');
        }

        public static string[] GetDatabases(string connectionString, int? commandTimeOut)
        {
            SqlConnection sqlConnection = null;
            string[] databasesArray;
            StringCollection databasesCollection = new StringCollection();
            try
            {
                sqlConnection = new SqlConnection(connectionString.TrimEnd(';') + ";Connection Timeout=" + commandTimeOut.ToString());
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
                if (commandTimeOut.HasValue)
                    sqlCommand.CommandTimeout = commandTimeOut.Value;
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

        public static string[] GetSqlDataSources()
        {
            DataTable dt = System.Data.Sql.SqlDataSourceEnumerator.Instance.GetDataSources();
            string[] servers = new string[dt.Rows.Count];
            int count = 0;
            foreach (DataRow dr in dt.Rows)
            {
                string ServerName = dr["ServerName"].ToString().Trim().ToUpper();
                string InstanceName = null;

                if (dr["InstanceName"] != null && dr["InstanceName"] != DBNull.Value)
                    InstanceName = dr["InstanceName"].ToString().Trim().ToUpper();
                servers[count] = ServerName;
                if (!String.IsNullOrEmpty(InstanceName))
                    servers[count] += "\\" + InstanceName;
                count++;
            }
            Array.Sort<string>(servers);
            return servers;
        }

        private void frmStorageManagement_Load(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
            this.HourGlass(true);
            this.cmbDataSources.Text = this.dataSource;
            if (String.Compare(this.security, "Sql", true)==0)
            {
                this.rbIntegrated.Checked = false;
                this.rbSql.Checked = true;
                this.txtUserId.Text = this.userId;
                this.txtUserId.Enabled = true;
                this.txtPassword.Text = this.password;
                this.txtPassword.Enabled = true;
            }
            else
            {
                this.rbIntegrated.Checked = true;
                this.rbSql.Checked = false;
                this.txtUserId.Text = String.Empty;
                this.txtUserId.Enabled = false;
                this.txtPassword.Text = String.Empty;
                this.txtPassword.Enabled = false;
            }
            this.cmbDatabases.Text = this.initialCatalog;
            this.txtOtherSettings.Text = this.otherSettings;
            this.HourGlass(false);
            NetSqlAzMan.SnapIn.Globalization.ResourcesManager.CollectResources(this);
        }



        private void cmbDataSource_DropDown(object sender, EventArgs e)
        {
            if (this.cmbDataSources.Items.Count == 0)
            {
                this.RefreshDataSource();
            }
        }

        private void RefreshDataSource()
        {
            this.HourGlass(true);
            string selectedText = this.cmbDataSources.Text;
            this.cmbDataSources.Items.Clear();
            int selectedIndex = -1;
            int index=-1;
            foreach (string server in frmStorageConnection.GetSqlDataSources())
            {
                index++;
                this.cmbDataSources.Items.Add(server);
                if (String.Compare(selectedText, server, true)==0)
                {
                    selectedIndex = index;
                }
            }
            this.cmbDataSources.SelectedIndex = selectedIndex;
            this.HourGlass(false);
        }

        private void btnRefreshDataSources_Click(object sender, EventArgs e)
        {
            this.RefreshDataSource();
            this.ValidateForm();
        }

        private void rbAuthentication_CheckedChanged(object sender, EventArgs e)
        {
            if (this.rbIntegrated.Checked)
            {
                this.txtUserId.Text = String.Empty;
                this.txtPassword.Text = String.Empty;
                this.txtUserId.Enabled = false;
                this.txtPassword.Enabled = false;
            }
            else if (this.rbSql.Checked)
            {
                this.txtUserId.Enabled = true;
                this.txtPassword.Enabled = true;
                this.txtUserId.Focus();
            }
            this.ValidateForm();
        }

        private void RefreshDatabases()
        {
            try
            {
                string currentdb = this.cmbDatabases.Text;
                this.HourGlass(true);
                this.cmbDatabases.Items.Clear();
                string[] databases = frmStorageConnection.GetDatabases(
                    frmStorageConnection.ConstructConnectionString(this.cmbDataSources.Text, "master", this.rbIntegrated.Checked, this.txtUserId.Text, this.txtPassword.Text, this.txtOtherSettings.Text), new int?()
                    );
                foreach (string database in databases)
                {
                    this.cmbDatabases.Items.Add(database);
                }
                this.cmbDatabases.Text = currentdb;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Globalization.MultilanguageResource.GetString("frmStorageConnection_Msg10"), MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
            finally
            {
                this.Cursor = Cursors.Arrow;
            }
        }

        private void btnRefreshDatabases_Click(object sender, EventArgs e)
        {
            this.RefreshDatabases();
            this.ValidateForm();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            string connectionString = frmStorageConnection.ConstructConnectionString(this.cmbDataSources.Text, this.cmbDatabases.Text, this.rbIntegrated.Checked, this.txtUserId.Text, this.txtPassword.Text, this.txtOtherSettings.Text);
            try
            {
                frmStorageConnection.TestConnection(connectionString);
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message+"\r\nConnection String:\r\n"+connectionString, "Invalid Connection Settings");
                return;
            }
            this.HourGlass(true);
            this.dataSource = this.cmbDataSources.Text;
            this.initialCatalog = this.cmbDatabases.Text;
            this.security = (this.rbIntegrated.Checked ? "Integrated" : "Sql");
            this.userId = this.txtUserId.Text;
            this.password = this.txtPassword.Text;
            this.otherSettings = this.txtOtherSettings.Text;
            this.DialogResult = DialogResult.OK;
            this.HourGlass(false);
        }

        internal static void TestConnection(string connectionString)
        {
            SqlAzManStorage.VerifyStorageDB(connectionString);
            SqlAzManStorage storage = new SqlAzManStorage(connectionString);
            string dbVersion = storage.DatabaseVesion;
            string netsqlazmanRunTimeVersion = storage.GetType().Assembly.GetName().Version.ToString();
            if (!String.Equals(netsqlazmanRunTimeVersion.Substring(0,6), dbVersion.Substring(0,6), StringComparison.InvariantCultureIgnoreCase))
            {
                MessageBox.Show(String.Format("NetSqlAzMan Run-Time Version: {0}\r\nNetSqlAzMan Database Version: {1}", netsqlazmanRunTimeVersion, dbVersion), "Warning: Version mismatch",MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnTestConnection_Click(object sender, EventArgs e)
        {
            try
            {
                string connectionString = frmStorageConnection.ConstructConnectionString(this.cmbDataSources.Text, this.cmbDatabases.Text, this.rbIntegrated.Checked, this.txtUserId.Text, this.txtPassword.Text, this.txtOtherSettings.Text);
                frmStorageConnection.TestConnection(connectionString);
                this.ShowInfo(Globalization.MultilanguageResource.GetString("frmStorageConnection_Msg20"), Globalization.MultilanguageResource.GetString("frmStorageConnection_Tit20"));
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message + "\r\n" + Globalization.MultilanguageResource.GetString("frmStorageConnection_Msg30"), Globalization.MultilanguageResource.GetString("frmStorageConnection_Tit30"));
            }
        }

        private void cmbDataSource_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ValidateForm();
        }

        private void cmbDatabases_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ValidateForm();
        }

        private void cmbDataSource_TextChanged(object sender, EventArgs e)
        {
            this.cmbDatabases.Items.Clear();
            this.ValidateForm();
        }

        

        private void cmbDatabases_DropDown(object sender, EventArgs e)
        {
            if (this.cmbDatabases.Items.Count == 0)
            {
                this.RefreshDatabases();
            }
        }

        private bool ValidateForm()
        {
            bool isValid = true;
            this.errorProvider1.Clear();
            if (String.IsNullOrEmpty(this.cmbDataSources.Text))
            {
                isValid = false;
                this.errorProvider1.SetError(this.cmbDataSources, Globalization.MultilanguageResource.GetString("frmStorageConnection_Msg40"));
            }
            if (String.IsNullOrEmpty(this.cmbDatabases.Text))
            {
                isValid = false;
                this.errorProvider1.SetError(this.cmbDatabases, Globalization.MultilanguageResource.GetString("frmStorageConnection_Msg50"));
            }
            if (this.rbSql.Checked && String.IsNullOrEmpty(this.txtUserId.Text.Trim()))
            {
                isValid = false;
                this.errorProvider1.SetError(this.txtUserId, "User Id required.");
            }
            this.btnOk.Enabled = this.btnTestConnection.Enabled = isValid;
            return isValid;
        }

        private void txtUserId_TextChanged(object sender, EventArgs e)
        {
            this.ValidateForm();
        }

        private void cmbDatabases_TextChanged(object sender, EventArgs e)
        {
            this.ValidateForm();
        }

        private void txtOtherSettings_TextChanged(object sender, EventArgs e)
        {
            this.ValidateForm();
        }
    }
}