using System;
using System.Xml;
using NetSqlAzMan;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzManWebConsole
{
    public partial class dlgImport : dlgPage
    {
        protected internal IAzManStorage storage = null;
        //[PreEmptive.Attributes.Feature("NetSqlAzMan WebConsole: XML Import")]
        protected void Page_Init(object sender, EventArgs e)
        {
            this.setImage("NetSqlAzMan_32x32.gif");
            this.setOkHandler(new EventHandler(this.btnOk_Click));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.storage = this.Session["storage"] as IAzManStorage;
            this.menuItem = Request["MenuItem"];
            this.Description = ".NET Sql Authorization Manager Import";
            this.Text = this.menuItem;
            this.Description = this.Text;
            this.Title = this.Text;
            this.showWaitPanelOnSubmit(this.pnlWait, this.pnlImport);
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                SqlAzManMergeOptions mergeOptions = SqlAzManMergeOptions.NoMerge;
                if (this.chkCreatesNewItems.Checked) mergeOptions |= SqlAzManMergeOptions.CreatesNewItems;
                if (this.chkOverwritesExistingItems.Checked) mergeOptions |= SqlAzManMergeOptions.OverwritesExistingItems;
                if (this.chkDeleteMissingItems.Checked) mergeOptions |= SqlAzManMergeOptions.DeleteMissingItems;
                if (this.chkCreatesNewItemAuthorizations.Checked) mergeOptions |= SqlAzManMergeOptions.CreatesNewItemAuthorizations;
                if (this.chkOverwritesItemAuthorizations.Checked) mergeOptions |= SqlAzManMergeOptions.OverwritesExistingItemAuthorization;
                if (this.chkDeleteMissingItemAuthorizations.Checked) mergeOptions |= SqlAzManMergeOptions.DeleteMissingItemAuthorizations;
                this.doImport(this.Session["selectedObject"], this.chkUsersAndGroups.Checked, this.chkDBUsers.Checked, this.chkAuthorizations.Checked, mergeOptions);
                this.closeWindow(true);
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }

        public void doImport(object importIntoObject, bool chkUsersAndGroups, bool chkDBUsers, bool chkAuthorizations, SqlAzManMergeOptions mergeOptions)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(this.FileUpload1.PostedFile.InputStream);
            XmlNode xmlStartNode;
            if (this.checkScopeNodePosition(doc, ref importIntoObject, out xmlStartNode))
            {
                try
                {
                    this.storage.BeginTransaction(AzManIsolationLevel.ReadUncommitted);
                    ((IAzManImport)importIntoObject).ImportChildren(xmlStartNode, chkUsersAndGroups, chkDBUsers, chkAuthorizations, mergeOptions);
                    this.storage.CommitTransaction();
                    this.closeWindow(true);
                }
                catch
                {
                    this.storage.RollBackTransaction();
                    throw;
                }
            }
            else
            {
                this.closeWindow(false);
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
            string firstChildNodeNameMustBe=null;
            string firstChildNodeNameMustBeOr = String.Empty;
            if (objectForImport as IAzManStorage != null)
                firstChildNodeNameMustBe = "Store";
            if (objectForImport as IAzManStore != null)
            {
                firstChildNodeNameMustBe = "Application";
                firstChildNodeNameMustBeOr = "StoreGroups";
            }
            if (objectForImport as IAzManStore != null && this.menuItem == "Import Store Groups")
            {
                firstChildNodeNameMustBe = "StoreGroup";
                firstChildNodeNameMustBeOr = "StoreGroups";
            }
            if (objectForImport as IAzManApplication != null)
            {
                firstChildNodeNameMustBe = "Item";
                firstChildNodeNameMustBeOr = "ApplicationGroups";
            }
            if (objectForImport as IAzManApplication != null  && this.menuItem == "Import Application Groups")
            {
                firstChildNodeNameMustBe = "ApplicationGroup";
                firstChildNodeNameMustBeOr = "ApplicationGroups";
            }
            if (objectForImport as IAzManApplication != null && this.menuItem == "Import Items")
            {
                firstChildNodeNameMustBe = "Item";
                firstChildNodeNameMustBeOr = "Items";
            }
            else if (String.IsNullOrEmpty(firstChildNodeNameMustBe)) throw new ArgumentException("objectForImport type not supported.");

            if (doc["NetSqlAzMan"] == null || (doc["NetSqlAzMan"][firstChildNodeNameMustBe] == null && doc["NetSqlAzMan"][firstChildNodeNameMustBeOr] == null))
            {
                throw new System.Xml.Schema.XmlSchemaValidationException("The Xml file is not valid or wrong import position.");
            }
            else
            {
                xmlStartNode = doc["NetSqlAzMan"];
                return true;
            }
        }
    }
}
