using System;
using System.CodeDom;
using System.Web.UI;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzManWebConsole
{
    public partial class dlgGenerateCheckAccessHelper : dlgPage
    {
        protected internal IAzManStorage storage = null;
        internal IAzManApplication application;
        [PreEmptive.Attributes.Feature("NetSqlAzMan WebConsole: Generate Check Access Helper")]
        protected void Page_Init(object sender, EventArgs e)
        {
            this.setImage("NetSqlAzMan_32x32.gif");
            this.setOkHandler(new EventHandler(this.btnOk_Click));
            this.showCloseOnly();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.storage = this.Session["storage"] as IAzManStorage;
            this.application = this.Session["selectedObject"] as IAzManApplication;
            this.Text = "Generate CheckAccessHelper class";
            this.Description = this.Text;
            this.Title = this.Text;
            if (!Page.IsPostBack)
            {
                this.btnCopy.Attributes["onclick"] = String.Format("javascript: copyToClipBoard('{0}');", this.txtSourceCode.UniqueID);
                this.txtNamespace.Text = this.TransformToVariable("", this.application.Name, false) + ".Security";
                this.btnGenerate_Click(this, EventArgs.Empty);
                this.txtClassName.Focus();
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            try
            {
                this.closeWindow(false);
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }

        private string TransformToVariable(string prefix, string name, bool toupper)
        {
            string ris = "";
            if (String.IsNullOrEmpty(name)) return String.Empty;
            char[] nc = name.ToCharArray();
            for (int i = 0; i < nc.Length; i++)
            {
                if (!char.IsLetterOrDigit(nc[i]) && (nc[i] != '_'))
                {
                    if (nc[i] != '[')
                    {
                        ris += "_";
                    }
                }
                else
                {
                    ris += nc[i].ToString();
                }
            }
            if (toupper)
            {
                ris = ris.ToUpper();
            }
            if (!char.IsLetter(ris.ToCharArray()[0]))
            {
                ris = "_" + ris;

            }
            ris = ris.Trim('_');
            ris = prefix + ris;
            return ris;
        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                this.txtSourceCode.Text = String.Empty;
                CodeCompileUnit ccu1 = NetSqlAzMan.CodeDom.CodeDomGenerator.GenerateCheckAccessHelperClass(this.txtNamespace.Text, this.txtClassName.Text, this.chkAllowRoles.Checked, this.chkAllowTasks.Checked, this.application, this.rbCSharp.Checked ? NetSqlAzMan.CodeDom.Language.CSharp : NetSqlAzMan.CodeDom.Language.VB);
                CodeCompileUnit ccu2 = NetSqlAzMan.CodeDom.CodeDomGenerator.GenerateItemConstants(this.txtNamespace.Text, this.chkAllowRoles.Checked, this.chkAllowTasks.Checked, this.application, this.rbCSharp.Checked ? NetSqlAzMan.CodeDom.Language.CSharp : NetSqlAzMan.CodeDom.Language.VB);
                this.txtSourceCode.Text =
                    NetSqlAzMan.CodeDom.CodeDomGenerator.GenerateSourceCode(ccu1, this.rbCSharp.Checked ? NetSqlAzMan.CodeDom.Language.CSharp : NetSqlAzMan.CodeDom.Language.VB)
                    + "\r\n" +
                    NetSqlAzMan.CodeDom.CodeDomGenerator.GenerateSourceCode(ccu2, this.rbCSharp.Checked ? NetSqlAzMan.CodeDom.Language.CSharp : NetSqlAzMan.CodeDom.Language.VB);
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }
    }
}
