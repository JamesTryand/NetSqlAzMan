using System;

namespace NetSqlAzManWebConsole
{
    public partial class ModalDialogHandler : ThemePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            string url = String.Empty;
            string menuItem = this.Request["MenuItem"];
            string menuValue = this.Request["MenuValue"];
            switch (menuItem)
            {
                case "New Store":
                case "Store Properties":
                    url = "dlgStoreProperties.aspx"; break;
                case "Mode and Logging": url = "dlgOptions.aspx"; break;
                case "Auditing": url = "dlgAuditing.aspx"; break;
                //WCF Cache Service
                case "Invalidate WCF Cache Service": url = "dlgInvalidateWCFCacheService.aspx"; break;
                //Import
                case "Import Stores":
                case "Import Store Groups/Application":
                case "Import Store Groups":
                case "Import Application Groups/Items":
                case "Import Application Groups":
                case "Import Items":
                    url = "dlgImport.aspx";
                    break;
                //Export
                case "Export Stores":
                case "Export Store":
                case "Export Store Group":
                case "Export Application":
                case "Export Application Group":
                case "Export Items":
                    url = "dlgExport.aspx"; break;
                case "Import Store from AzMan": url = "dlgImportFromAzMan.aspx"; break;
                case "New Application":
                case "Application Properties": url = "dlgApplicationProperties.aspx"; break;
                case "Items Hierarchical View": url = "dlgItemsHierarchyView.aspx"; break;
                case "Store Group Properties": url = "dlgStoreGroupProperties.aspx"; break;
                case "Application Group Properties": url = "dlgApplicationGroupProperties.aspx"; break;
                case "New Store Group": url = "dlgNewStoreGroup.aspx"; break;
                case "New Application Group": url = "dlgNewApplicationGroup.aspx"; break;
                case "Generate CheckAccessHelper": url = "dlgGenerateCheckAccessHelper.aspx"; break;
                case "Check Access Test": url = "dlgCheckAccessTest.aspx"; break;
                case "New Role":
                case "New Task":
                case "New Operation":
                case "Role Properties":
                case "Task Properties":
                case "Operation Properties":
                    url = "dlgItemProperties.aspx"; break;
                case "Manage Authorizations":
                    url = "dlgItemAuthorizations.aspx"; break;
                default:
                    throw new NotImplementedException(String.Format("{0} Not implemented.", menuItem));
            }
            Response.Redirect(url + "?MenuItem=" + menuItem, true);
        }
    }
}
