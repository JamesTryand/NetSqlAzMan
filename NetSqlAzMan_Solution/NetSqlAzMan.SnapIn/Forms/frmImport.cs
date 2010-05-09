using System;
using System.Windows.Forms;
using System.Xml;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.SnapIn.ScopeNodes;

namespace NetSqlAzMan.SnapIn.Forms
{
    public partial class frmImport : frmBase
    {
        public frmImport()
        {
            InitializeComponent();
        }

        private void frmImport_Load(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
            this.lblTitle.Text = Globalization.MultilanguageResource.GetString("frmImport_lblTitle.Text");
            NetSqlAzMan.SnapIn.Globalization.ResourcesManager.CollectResources(this);
            /*Application.DoEvents();*/
        }

        internal void HourGlass(bool switchOn)
        {
            this.Cursor = switchOn ? Cursors.WaitCursor : Cursors.Arrow;
            /*Application.DoEvents();*/
        }

        private void frmImport_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.None)
            {
                this.DialogResult = DialogResult.None;
                e.Cancel = true;
            }
        }

        public DialogResult ShowDialog(IWin32Window owner, string fileName, object importIntoObject, bool chkUsersAndGroups, bool chkDBUsers, bool chkAuthorizations, SqlAzManMergeOptions mergeOptions)
        {
            this.DialogResult = DialogResult.None;
            this.TopMost = true;
            this.Show(owner);
            /*Application.DoEvents();*/
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);
            XmlNode xmlStartNode;
            if (this.checkScopeNodePosition(doc, ref importIntoObject, out xmlStartNode))
            {
                IAzManStorage storage = this.getStorageReference(importIntoObject);
                try
                {
                    storage.BeginTransaction(AzManIsolationLevel.ReadUncommitted);
                    ((IAzManImport)importIntoObject).ImportChildren(xmlStartNode, chkUsersAndGroups, chkDBUsers, chkAuthorizations, mergeOptions);
                    storage.CommitTransaction();
                    this.Hide();
                    return this.DialogResult = DialogResult.OK;
                }
                catch
                {
                    storage.RollBackTransaction();
                    this.DialogResult = DialogResult.Cancel;
                    this.Hide();
                    throw;
                }
            }
            else
            {
                return this.DialogResult = DialogResult.Cancel;
            }
        }

        /// <summary>
        /// Checks the scope node position.
        /// </summary>
        /// <param name="doc">The doc.</param>
        /// <param name="objectForImport">The object for import.</param>
        /// <param name="xmlStartNode">The XML start node.</param>
        /// <returns></returns>
        private bool checkScopeNodePosition(XmlDocument doc, ref object objectForImport, out XmlNode xmlStartNode)
        {
            string firstChildNodeNameMustBe;
            string firstChildNodeNameMustBeOr = String.Empty;
            if (objectForImport as IAzManStorage != null)
                firstChildNodeNameMustBe = "Store";
            else if (objectForImport as IAzManStore != null)
            {
                firstChildNodeNameMustBe = "Application";
                firstChildNodeNameMustBeOr = "StoreGroups";
            }
            else if (objectForImport as StoreGroupsScopeNode != null)
            {
                firstChildNodeNameMustBe = "StoreGroup";
                firstChildNodeNameMustBeOr = "StoreGroups";
                objectForImport = ((StoreGroupsScopeNode)objectForImport).Store;
            }
            else if (objectForImport as IAzManApplication != null)
            {
                firstChildNodeNameMustBe = "Item";
                firstChildNodeNameMustBeOr = "ApplicationGroups";
            }
            else if (objectForImport as ApplicationGroupsScopeNode != null)
            {
                firstChildNodeNameMustBe = "ApplicationGroup";
                firstChildNodeNameMustBeOr = "ApplicationGroups";
                objectForImport = ((ApplicationGroupsScopeNode)objectForImport).Application;
            }
            else if (objectForImport as ItemDefinitionsScopeNode != null)
            {
                firstChildNodeNameMustBe = "Item";
                firstChildNodeNameMustBeOr = "Items";
                objectForImport = ((ItemDefinitionsScopeNode)objectForImport).Application;
            }
            else if (objectForImport as ItemDefinitionScopeNode != null)
            {
                firstChildNodeNameMustBe = "Item";
                firstChildNodeNameMustBeOr = "Items";
                objectForImport = ((ItemDefinitionScopeNode)objectForImport).Item;
            }
            else throw new ArgumentException("objectForImport type not supported.");

            if (doc["NetSqlAzMan"] == null || (doc["NetSqlAzMan"][firstChildNodeNameMustBe] == null && doc["NetSqlAzMan"][firstChildNodeNameMustBeOr] == null))
            {
                throw new System.Xml.Schema.XmlSchemaValidationException(Globalization.MultilanguageResource.GetString("frmImport_Msg10"));
            }
            else
            {
                xmlStartNode = doc["NetSqlAzMan"];
                return true;
            }
        }

        private IAzManStorage getStorageReference(object objectForImport)
        {
            if (objectForImport as IAzManStorage != null)
                return (IAzManStorage)objectForImport;
            else if (objectForImport as IAzManStore != null)
                return ((IAzManStore)objectForImport).Storage;
            else if (objectForImport as StoreGroupsScopeNode != null)
                return ((StoreGroupsScopeNode)objectForImport).Store.Storage;
            else if (objectForImport as IAzManApplication != null)
                return ((IAzManApplication)objectForImport).Store.Storage;
            else if (objectForImport as ApplicationGroupsScopeNode != null)
                return ((ApplicationGroupsScopeNode)objectForImport).Application.Store.Storage;
            else if (objectForImport as ItemDefinitionsScopeNode != null)
                return ((ItemDefinitionsScopeNode)objectForImport).Application.Store.Storage;
            else if (objectForImport as ItemDefinitionScopeNode != null)
                return ((ItemDefinitionScopeNode)objectForImport).Item.Application.Store.Storage;
            else throw new ArgumentException(Globalization.MultilanguageResource.GetString("frmImport_Msg20"));
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