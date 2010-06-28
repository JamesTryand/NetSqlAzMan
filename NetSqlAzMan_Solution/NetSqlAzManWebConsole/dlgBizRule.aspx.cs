using System;
using System.Web.UI;
using NetSqlAzMan;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzManWebConsole
{
    public partial class dlgBizRule : dlgPage
    {
        internal IAzManItem item;
        //[PreEmptive.Attributes.Feature("NetSqlAzMan WebConsole: Business Rule")]
        protected void Page_Init(object sender, EventArgs e)
        {
            this.setImage("BizRule.gif");
            this.showCloseOnly();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.item = (IAzManItem)this.Session["selectedObject"];
            this.Text = "Biz Rule - " + this.item.Name;
            this.Description = this.Text;
            this.Title = this.Text;
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(this.item.BizRuleSource))
                {
                    this.txtSourceCode.Text = this.item.BizRuleSource;
                }
                else
                {
                    this.txtSourceCode.Text = "(Choose a .NET language and press 'New Biz Rule' button to compose a new Business Rule, then press 'Reload rule into Store' to persist changes and compile Biz Rule.)";
                }
                if (this.item.BizRuleSourceLanguage.HasValue)
                {
                    if (this.item.BizRuleSourceLanguage.Value == BizRuleSourceLanguage.VBNet)
                    {
                        this.rbVBNet.Checked = true;
                    }
                    else
                    {
                        this.rbCSharp.Checked = true;
                    }
                }
                else
                {
                    this.rbCSharp.Checked = true;
                }
                this.txtSourceCode.Focus();
                if (!this.item.Application.IAmManager)
                    this.txtSourceCode.Enabled = this.rbCSharp.Enabled = this.rbVBNet.Enabled = this.btnReloadBizRule.Enabled = this.btnClearBizRule.Enabled = this.btnNewFromTemplate.Enabled = false;
                this.txtSourceCode.Focus();
            }
        }

        protected void btnReloadBizRule_Click(object sender, EventArgs e)
        {
            try
            {
                this.item.ReloadBizRule(this.txtSourceCode.Text, this.rbCSharp.Checked ? BizRuleSourceLanguage.CSharp : BizRuleSourceLanguage.VBNet);
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message);
            }
        }

        protected void btnClearBizRule_Click(object sender, EventArgs e)
        {
            try
            {
                this.txtSourceCode.Text = String.Empty;
                this.item.ClearBizRule();
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

        protected void btnNewFromTemplate_Click(object sender, EventArgs e)
        {
            string source = String.Empty;
            if (this.rbCSharp.Checked)
            {
                #region Get from BizRuleCSharpTemplate.cs
                source =
@"using System;
using System.Security.Principal;
using System.IO;
using System.Data;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text;
using NetSqlAzMan;
using NetSqlAzMan.Interfaces;

namespace {0}.BizRules
{
    public sealed class BizRule : IAzManBizRule
    {
        public BizRule()
        { }

        public bool Execute(Hashtable contextParameters, IAzManSid identity, IAzManItem ownerItem, ref AuthorizationType ForcedCheckAccessResult)
        {
            //Insert your code here
            return true;
        }
    }
}
";
                #endregion Get from BizRuleCSharpTemplate.cs
            }
            else
            {
                #region Get from BizRuleVBNetTemplate.vb
                source =
@"Imports System
Imports System.Security.Principal
Imports System.IO
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections
Imports System.Collections.Specialized
Imports System.Collections.Generic
Imports System.Text
Imports NetSqlAzMan
Imports NetSqlAzMan.Interfaces

Namespace {0}.BizRules
    Public NotInheritable Class BizRule : Implements IAzManBizRule
        Public Sub New()
        End Sub

        Public Overloads Function Execute(ByVal contextParameters As Hashtable, ByVal identity As IAzManSid, ByVal ownerItem As IAzManItem, ByRef ForcedCheckAccessResult As AuthorizationType) As Boolean _
            Implements IAzManBizRule.Execute
            'Insert your code here
            Return True
        End Function
    End Class
End Namespace
";
                #endregion Get from BizRuleVBNetTemplate.vb
            }
            this.txtSourceCode.Text = source.Replace("{0}", this.TransformToVariable("", this.item.Application.Name, false));
        }
    }
}
