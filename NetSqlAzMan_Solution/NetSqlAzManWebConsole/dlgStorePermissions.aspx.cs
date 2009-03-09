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
    public partial class dlgStorePermissions : dlgPage
    {
        protected internal IAzManStorage storage = null;
        protected internal IAzManStore store = null;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.setImage("Logins.bmp");
            this.setOkHandler(new EventHandler(this.btnOk_Click));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.storage = this.Session["storage"] as IAzManStorage;
            this.store = this.Session["selectedObject"] as IAzManStore;
            this.Text = String.Format("Store Permissions: {0}", this.store.Name);
            this.Title = this.Text;
            this.Description = "Store Permissions";
            if (!Page.IsPostBack)
                this.RefreshStorePermissions();
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                this.storage.OpenConnection();
                this.storage.BeginTransaction(AzManIsolationLevel.ReadUncommitted);
                //Managers
                KeyValuePair<string, bool>[] managers = this.store.GetManagers();
                foreach (ListItem sqlLogin in this.chkManagers.Items)
                {
                    if (sqlLogin.Selected)
                    {
                        if (!this.findLogin(managers, sqlLogin.Text))
                            this.store.GrantAccessAsManager(sqlLogin.Text);
                    }
                    else
                    {
                        if (this.findLogin(managers, sqlLogin.Text))
                            this.store.RevokeAccessAsManager(sqlLogin.Text);
                    }
                }
                //Users
                KeyValuePair<string, bool>[] users = this.store.GetUsers();
                foreach (ListItem sqlLogin in this.chkUsers.Items)
                {
                    if (sqlLogin.Selected)
                    {
                        if (!this.findLogin(users, sqlLogin.Text))
                            this.store.GrantAccessAsUser(sqlLogin.Text);
                    }
                    else
                    {
                        if (this.findLogin(users, sqlLogin.Text))
                            this.store.RevokeAccessAsUser(sqlLogin.Text);
                    }
                }
                //Readers
                KeyValuePair<string, bool>[] readers = this.store.GetReaders();
                foreach (ListItem sqlLogin in this.chkReaders.Items)
                {
                    if (sqlLogin.Selected)
                    {
                        if (!this.findLogin(readers, sqlLogin.Text))
                            this.store.GrantAccessAsReader(sqlLogin.Text);
                    }
                    else
                    {
                        if (this.findLogin(readers, sqlLogin.Text))
                            this.store.RevokeAccessAsReader(sqlLogin.Text);
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

        private void RefreshStorePermissions()
        {
            this.chkManagers.Items.Clear();
            foreach (KeyValuePair<string, bool> kvp in this.store.GetManagers())
            {
                ListItem li = new ListItem(kvp.Key);
                li.Selected = kvp.Value;
                this.chkManagers.Items.Add(li);
            }
            this.chkUsers.Items.Clear();
            foreach (KeyValuePair<string, bool> kvp in this.store.GetUsers())
            {
                ListItem li = new ListItem(kvp.Key);
                li.Selected = kvp.Value;
                this.chkUsers.Items.Add(li);
            }
            this.chkReaders.Items.Clear();
            foreach (KeyValuePair<string, bool> kvp in this.store.GetReaders())
            {
                ListItem li = new ListItem(kvp.Key);
                li.Selected = kvp.Value;
                this.chkReaders.Items.Add(li);
            }
            if (!this.store.IAmManager)
                this.chkManagers.Enabled = this.chkUsers.Enabled = this.chkReaders.Enabled = this.btnOk.Enabled = false;
            this.chkManagers.Enabled = this.store.IAmAdmin;
        }
    }
}