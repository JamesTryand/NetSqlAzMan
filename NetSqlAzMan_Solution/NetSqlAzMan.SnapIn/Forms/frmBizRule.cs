using System;
using System.Windows.Forms;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzMan.SnapIn.Forms
{
    public partial class frmBizRule : frmBase
    {
        internal IAzManItem item;
        //[PreEmptive.Attributes.Feature("NetSqlAzMan MMC SnapIn: Business Rule")]
        public frmBizRule()
        {
            InitializeComponent();
        }

        private void frmBizRule_Load(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
            if (!String.IsNullOrEmpty(this.item.BizRuleSource))
            {
                this.txtSourceCode.Text = this.item.BizRuleSource;
            }
            else
            {
                this.txtSourceCode.Text = Globalization.MultilanguageResource.GetString("frmBizRule_Msg10");
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
            this.txtSourceCode.SelectionStart = 0;
            this.txtSourceCode.SelectionLength = 0;
            this.txtSourceCode.Focus();
            if (!this.item.Application.IAmManager)
                this.txtSourceCode.Enabled = this.rbCSharp.Enabled = this.rbVBNet.Enabled = this.btnReloadBizRule.Enabled = this.btnClearBizRule.Enabled = this.btnNewFromTemplate.Enabled = false;
            NetSqlAzMan.SnapIn.Globalization.ResourcesManager.CollectResources(this);
        }

        protected void HourGlass(bool switchOn)
        {
            this.Cursor = switchOn ? Cursors.WaitCursor : Cursors.Arrow;
            /*Application.DoEvents();*/
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

        private void frmBizRule_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.None)
            {
                this.DialogResult = DialogResult.Cancel;
                e.Cancel = true;
            }
        }

        private void btnReloadBizRule_Click(object sender, EventArgs e)
        {
            try
            {
                this.DialogResult = DialogResult.None;
                this.HourGlass(true);
                this.item.ReloadBizRule(this.txtSourceCode.Text, this.rbCSharp.Checked ? BizRuleSourceLanguage.CSharp : BizRuleSourceLanguage.VBNet);
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("frmBizRule_Msg20"));
            }
            finally
            {
                this.HourGlass(false);
            }
        }

        private void btnClearBizRule_Click(object sender, EventArgs e)
        {
            try
            {
                this.HourGlass(true);
                this.txtSourceCode.Text = String.Empty;
                this.item.ClearBizRule();
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("frmBizRule_Msg20"));
            }
            finally
            {
                this.HourGlass(false);
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

        private void btnNewFromTemplate_Click(object sender, EventArgs e)
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

        public bool Execute(Hashtable contextParameters, IAzManSid identity, IAzManItem ownerItem, ref AuthorizationType authorizationType)
        {
            //" + Globalization.MultilanguageResource.GetString("frmBizRule_Msg30") + @"
            //Assign authorizationType to some AuthorizationType value to force CheckAccess result for this item.
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

        Public Overloads Function Execute(ByVal contextParameters As Hashtable, ByVal identity As IAzManSid, ByVal ownerItem As IAzManItem, ByRef authorization as AuthorizationType) As Boolean _
            Implements IAzManBizRule.Execute
            '" + Globalization.MultilanguageResource.GetString("frmBizRule_Msg30")+ @"
            'Assign authorizationType to some AuthorizationType value to force CheckAccess result for this item.
            Return True
        End Function
    End Class
End Namespace
";
                #endregion Get from BizRuleVBNetTemplate.vb
            }
            this.txtSourceCode.Text = source.Replace("{0}",this.TransformToVariable("",this.item.Application.Name,false));
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            NetSqlAzMan.SqlAzManItem.ClearBizRuleAssemblyCache();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}