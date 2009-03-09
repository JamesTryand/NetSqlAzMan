using System;
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
    public partial class dlgApplicationPermissions : dlgPage
    {
        protected internal IAzManStorage storage = null;
        protected internal IAzManApplication application = null;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.setImage("Logins.bmp");
            this.setOkHandler(new EventHandler(this.btnOk_Click));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.storage = this.Session["storage"] as IAzManStorage;
            this.application = this.Session["selectedObject"] as IAzManApplication;
            this.Text = String.Format("Application Permissions: {0}", this.application.Name);
            this.Title = this.Text;
            this.Description = "Application Permissions";
            if (!Page.IsPostBack)
                this.RefreshApplicationPermissions();
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                this.storage.OpenConnection();
                this.storage.BeginTransaction(AzManIsolationLevel.ReadUncommitted);
                //Managers
                KeyValuePair<string, bool>[] managers = this.application.GetManagers();
                foreach (ListItem sqlLogin in this.chkManagers.Items)
                {
                    if (sqlLogin.Selected)
                    {
                        if (!this.findLogin(managers, sqlLogin.Text))
                            this.application.GrantAccessAsManager(sqlLogin.Text);
                    }
                    else
                    {
                        if (this.findLogin(managers, sqlLogin.Text))
                            this.application.RevokeAccessAsManager(sqlLogin.Text);
                    }
                }
                //Users
                KeyValuePair<string, bool>[] users = this.application.GetUsers();
                foreach (ListItem sqlLogin in this.chkUsers.Items)
                {
                    if (sqlLogin.Selected)
                    {
                        if (!this.findLogin(users, sqlLogin.Text))
                            this.application.GrantAccessAsUser(sqlLogin.Text);
                    }
                    else
                    {
                        if (this.findLogin(users, sqlLogin.Text))
                            this.application.RevokeAccessAsUser(sqlLogin.Text);
                    }
                }
                //Readers
                KeyValuePair<string, bool>[] readers = this.application.GetReaders();
                foreach (ListItem sqlLogin in this.chkReaders.Items)
                {
                    if (sqlLogin.Selected)
                    {
                        if (!this.findLogin(readers, sqlLogin.Text))
                            this.application.GrantAccessAsReader(sqlLogin.Text);
                    }
                    else
                    {
                        if (this.findLogin(readers, sqlLogin.Text))
                            this.application.RevokeAccessAsReader(sqlLogin.Text);
                    }
                }
                this.storage.CommitTransaction();
                this.closeWindow(false);
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
            finally
            {
                this.storage.CloseConnection();
            }
        }

        private bool findLogin(KeyValuePair<string, bool>[] logins, string login)
        {
            foreach (KeyValuePair<string, bool> l in logins)
            {
                if (l.Value && String.Compare(l.Key, login, true) == 0)
                {
                    return true;
                }
            }
            return false;
        }

        private void RefreshApplicationPermissions()
        {
            this.chkManagers.Items.Clear();
            foreach (KeyValuePair<string, bool> kvp in this.application.GetManagers())
            {
                ListItem li = new ListItem(kvp.Key);
                li.Selected = kvp.Value;
                this.chkManagers.Items.Add(li);
            }
            this.chkUsers.Items.Clear();
            foreach (KeyValuePair<string, bool> kvp in this.application.GetUsers())
            {
                ListItem li = new ListItem(kvp.Key);
                li.Selected = kvp.Value;
                this.chkUsers.Items.Add(li);
            }
            this.chkReaders.Items.Clear();
            foreach (KeyValuePair<string, bool> kvp in this.application.GetReaders())
            {
                ListItem li = new ListItem(kvp.Key);
                li.Selected = kvp.Value;
                this.chkReaders.Items.Add(li);
            }
            if (!this.application.IAmManager)
                this.chkManagers.Enabled = this.chkUsers.Enabled = this.chkReaders.Enabled = this.btnOk.Enabled = false;
            this.chkManagers.Enabled = this.application.IAmAdmin;
        }
    }
}