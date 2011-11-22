using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzManWebConsole
{
    public partial class dlgExport : dlgPage
    {
        protected internal IAzManStorage storage = null;

        protected void Page_Init(object sender, EventArgs e)
        {
            this.setImage("NetSqlAzMan_32x32.gif");
            this.setOkHandler(new EventHandler(this.btnOk_Click));
            this.btnOk.Text = "Export";
            this.btnCancel.Text = "Close";
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.storage = this.Session["storage"] as IAzManStorage;
            this.menuItem = Request["MenuItem"];
            this.Text = ".NET Sql Authorization Manager Export";
            this.Description = this.menuItem;
            this.Title = this.Text;
            this.showWaitPanelOnSubmit(this.pnlWait, this.pnlExport);
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                List<IAzManExport> objectsToExport = new List<IAzManExport>();
                object selectedObject = this.Session["selectedObject"];
                if (this.menuItem == "Export Stores")
                    objectsToExport.AddRange(this.storage.GetStores());
                else if (this.menuItem == "Export Items")
                    objectsToExport.AddRange(((IAzManApplication)selectedObject).GetItems());
                else
                    objectsToExport.Add((IAzManExport)selectedObject); 
                byte[] result = this.doExport(objectsToExport.ToArray(), this.chkUsersAndGroups.Checked, this.chkDBUsers.Checked, this.chkAuthorizations.Checked);
                this.Session["DownloadContent"] = result;
                this.closeWindow(true);
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }

        public byte[] doExport(IAzManExport[] objectsToExport, bool includeItemAuthorizations, bool includeDBUsers, bool includeAuthorizations)
        {
            MemoryStream ms = new MemoryStream();
            XmlWriter xw = null;
            xw = XmlWriter.Create(ms);
            this.BeginExport(xw);
            foreach (IAzManExport objectToExport in objectsToExport)
            {
                objectToExport.Export(xw, includeItemAuthorizations, includeDBUsers, includeAuthorizations, objectToExport);
            }
            this.EndExport(xw);
            xw.Flush();
            xw.Close();
            return ms.ToArray();
        }

        protected void BeginExport(XmlWriter xmlWriter)
        {
            xmlWriter.WriteComment("*************************************");
            xmlWriter.WriteComment(".NET SQL Authorization Manager (Ms-PL)");
            xmlWriter.WriteComment("*************************************");
            xmlWriter.WriteComment("http://netsqlazman.codeplex.com");
            xmlWriter.WriteComment("Andrea Ferendeles");
            xmlWriter.WriteComment("*************************************");
            xmlWriter.WriteComment(String.Format("Creation Date: {0}", DateTime.Now.ToString()));
            xmlWriter.WriteComment(String.Format("NetSqlAzMan Run-Time version: {0}", this.storage.GetType().Assembly.GetName().Version.ToString()));
            xmlWriter.WriteComment(String.Format("NetSqlAzMan Database version: {0}", this.storage.DatabaseVesion));
            xmlWriter.WriteComment("*************************************");
            xmlWriter.WriteStartElement("NetSqlAzMan");
        }

        protected void EndExport(XmlWriter xmlWriter)
        {
            xmlWriter.WriteEndElement();
            xmlWriter.WriteComment("*************************************");
            xmlWriter.WriteComment(".NET SQL Authorization Manager (Ms-PL)");
            xmlWriter.WriteComment("*************************************");
        }
    }
}
