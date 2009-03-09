using System;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using MMC = Microsoft.ManagementConsole;
using Microsoft.ManagementConsole.Advanced;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan;

namespace NetSqlAzMan.SnapIn.Forms
{
    public partial class frmExport : frmBase
    {
        IAzManStorage storage;
        public frmExport()
        {
            InitializeComponent();
        }

        private void frmExport_Load(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
            NetSqlAzMan.SnapIn.Globalization.ResourcesManager.CollectResources(this);
        }

        internal void HourGlass(bool switchOn)
        {
            this.Cursor = switchOn ? Cursors.WaitCursor : Cursors.Arrow;
            /*Application.DoEvents();*/
        }

        private void frmExport_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.None)
            {
                this.DialogResult = DialogResult.None;
                e.Cancel = true;
            }
        }

        public DialogResult ShowDialog(MMC.SyncActionEventArgs e, string fileName, IAzManExport[] objectsToExport, bool includeSecurityObjects, bool includeDBUsers, bool includeAuthorizations, IAzManStorage storage)
        {
            this.storage = storage;
            XmlWriter xw=null;
            try
            {
                this.Show();
                this.Activate();
                this.Focus();
                /*Application.DoEvents();*/
                if (e!=null)
                    e.Status.ReportProgress(50, 100, Globalization.MultilanguageResource.GetString("frmExport_Msg10"));
                xw = XmlWriter.Create(fileName);
                this.BeginExport(xw);
                foreach (IAzManExport objectToExport in objectsToExport)
                {
                    objectToExport.Export(xw, includeSecurityObjects, includeDBUsers, includeAuthorizations, objectToExport);
                    /*Application.DoEvents();*/
                }
                this.EndExport(xw);
                if (e != null)
                    e.Status.Complete(Globalization.MultilanguageResource.GetString("frmExport_Msg20"), true);
                return this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("frmExport_Msg30"));
                e.Status.Complete(Globalization.MultilanguageResource.GetString("frmExport_Msg20"), false);
                return this.DialogResult = DialogResult.Cancel;
            }
            finally
            {
                if (xw != null)
                {
                    xw.Flush();
                    xw.Close();
                }
                this.Hide();
            }
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
    }
}