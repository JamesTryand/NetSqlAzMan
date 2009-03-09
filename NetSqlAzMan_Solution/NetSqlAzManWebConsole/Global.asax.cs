using System;
using System.DirectoryServices;
using System.IO;
using System.Diagnostics;
using System.Data;
using System.Configuration;
using System.Collections.Generic;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using NetSqlAzMan;
using NetSqlAzMan.Interfaces;


namespace NetSqlAzManWebConsole
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {
            List<string> sqlDataSources = new List<string>();
            if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["RootDSEPath"]))
                SqlAzManStorage.RootDSEPath = ConfigurationManager.AppSettings["RootDSEPath"];
            NetSqlAzMan.DirectoryServices.DirectoryServicesUtils.SetActiveDirectoryLookUpCredential(
                ConfigurationManager.AppSettings["Active Directory LookUp Username"],
                ConfigurationManager.AppSettings["Active Directory LookUp Password"]);
            //Populate SQL Data Sources
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(
                delegate(object o)
                {
                    try
                    {
                        sqlDataSources.AddRange(this.GetSqlDataSources());
                        this.Application.Lock();
                        this.Application["SqlDataSources"] = sqlDataSources;
                        this.Application.UnLock();
                    }
                    catch
                    {
                        this.Application.Lock();
                        this.Application["SqlDataSources"] = null;
                        this.Application.UnLock();
                    }
                }
                ));
            //Populate Active Directory Users & Groups
            System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(
                delegate(object o)
                {
                    try
                    {
                        DirectoryEntry root = Utility.NewDirectoryEntry("LDAP://" + SqlAzManStorage.RootDSEPath);
                        DirectorySearcher deSearch = new DirectorySearcher(root);
                        deSearch.Filter = "(|(&(objectClass=user)(objectCategory=person))(objectClass=group))";
                        SearchResultCollection searchResultCollection = deSearch.FindAll();
                        DataTable dtADList = null;
                        dtADList = new DataTable("Active Directory Objects List");
                        dtADList.Columns.Add("sAMAccountName", typeof(string));
                        dtADList.Columns.Add("Name", typeof(string));
                        dtADList.Columns.Add("objectClass", typeof(string));
                        dtADList.Columns.Add("objectSid", typeof(string));
                        dtADList.Columns.Add("ADSPath", typeof(string));
                        if (searchResultCollection != null)
                        {
                            foreach (SearchResult sr in searchResultCollection)
                            {
                                DirectoryEntry de = sr.GetDirectoryEntry();
                                DataRow dr = dtADList.NewRow();
                                dr["sAMAccountName"] = (string)de.Properties["sAMAccountName"][0];
                                dr["Name"] = (string)de.InvokeGet("displayname");
                                dr["objectClass"] = de.SchemaClassName;
                                dr["objectSid"] = new SqlAzManSID((byte[])de.Properties["objectSid"].Value).StringValue;
                                dr["ADSPath"] = de.Path;
                                dtADList.Rows.Add(dr);
                            }
                        }
                        DataView dv = dtADList.DefaultView;
                        dv.Sort = "sAMAccountName";
                        this.Application.Lock();
                        this.Application["Active Directory List"] = dv;
                        this.Application.UnLock();
                    }
                    catch
                    {
                        this.Application.Lock();
                        this.Application["Active Directory List"] = null;
                        this.Application.UnLock();
                    }
                }));
        }

        protected void Application_End(object sender, EventArgs e)
        {
            this.Application["SqlDataSources"] = null;
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            this.Session["storage"] = null;
            this.Session["selectedObject"] = null;
            this.Session["RefreshTree"] = false;
            this.Session["RefreshNode"] = false;
            this.Session["RefreshParentNode"] = false;
            this.Session["FindNodeText"] = null;
            this.Session["FindChildNodeText"] = null;
            this.Session["updateMessage"] = null;
            this.Session["updateWarning"] = false;
            if (ConfigurationManager.AppSettings["Check For Update"] == "true")
            {
                //Check for Update
                System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(
                    delegate(object session)
                    {
                        try
                        {
                            //Get ws update url from http://netsqlazman.sourceforge.net/wsNetSqlAzManWebConsoleUpdateUrl.txt
                            System.Net.WebRequest req = System.Net.WebRequest.Create("http://netsqlazman.sourceforge.net/wsNetSqlAzManWebConsoleUpdateUrl.txt");
                            req.Proxy = System.Net.WebRequest.GetSystemWebProxy();
                            req.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
                            System.Net.WebResponse resp = req.GetResponse();
                            System.IO.Stream stream = resp.GetResponseStream();
                            System.IO.StreamReader tr = new StreamReader(stream);
                            string wsUrl = tr.ReadToEnd();
                            tr.Close();
                            stream.Close();
                            //Call Update Providers Service
                            wsUpdate.NetSqlAzManWebConsoleUpdateService ws = new wsUpdate.NetSqlAzManWebConsoleUpdateService();
                            ws.Proxy = System.Net.WebRequest.GetSystemWebProxy();
                            ws.Proxy.Credentials = System.Net.CredentialCache.DefaultCredentials;
                            ws.Url = wsUrl;
                            string clientVersionId = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                            string[] environmentInfo = new string[] {
                "OS Version: "+Environment.OSVersion,
                "CLR Version: " + Environment.Version.ToString(),
                "Processor Count: "+Environment.ProcessorCount.ToString(),
                "Machine: " + Environment.MachineName,
                "NetSqlAzMan: "+ typeof(NetSqlAzMan.SqlAzManStorage).Assembly.GetName().Version.ToString().Trim()};

                            string[] results = ws.CheckForUpdate(environmentInfo, clientVersionId);
                            string serverVersionId = results[0].Trim();
                            string downloadUrl = results[1].Trim();
                            if (String.Compare(serverVersionId, clientVersionId, true) > 0)
                            {
                                string msg = "A new NetSqlAzMan Web Console version is available.\r\n\r\nYour version: {0}\r\nNew version: {1}\r\nDownload from: {2}.";
                                ((HttpSessionState)session)["updateMessage"] = String.Format(msg, clientVersionId, serverVersionId, downloadUrl);
                            }
                        }
                        catch { }
                    }
                    ), this.Session);
            }
        }

        protected void Session_End(object sender, EventArgs e)
        {
            if (this.Session["storage"] != null)
            {
                ((IAzManStorage)this.Session["storage"]).Dispose();
                this.Session["storage"] = null;
            }
            this.Session["selectedObject"] = null;
        }

        private string[] GetSqlDataSources()
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

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception objErr = Server.GetLastError();
            string err = "<b>Error Caught in Application_Error event</b><hr><br>" +
                    "<br><b>Error in: </b>" + Request.Url.ToString() +
                    "<br><b>Error Message: </b>" + objErr.Message.ToString() +
                    "<br><b>Stack Trace:</b><br>" +
                    Server.GetLastError().ToString();
            EventLog.WriteEntry("NetSqlAzManWebConsole", err, EventLogEntryType.Error);
            //Server.ClearError();
        }
    }
}