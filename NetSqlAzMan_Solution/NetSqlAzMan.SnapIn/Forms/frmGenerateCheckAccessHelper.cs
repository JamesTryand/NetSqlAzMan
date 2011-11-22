using System;
using System.CodeDom;
using System.Windows.Forms;
using NetSqlAzMan.Interfaces;

namespace NetSqlAzMan.SnapIn.Forms
{
    public partial class frmGenerateCheckAccessHelper : frmBase
    {
        internal IAzManApplication application;

        public frmGenerateCheckAccessHelper()
        {
            InitializeComponent();
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

        private void frmNetSqlAzManBase_Load(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
            this.txtNamespace.Text = this.TransformToVariable("",this.application.Name,false) + ".Security";
            this.btnGenerate_Click(this, EventArgs.Empty);
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

        private void frmNetSqlAzManBase_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.None)
                this.DialogResult = DialogResult.Cancel;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                this.HourGlass(true);
                this.txtSourceCode.Text = String.Empty;
                this.btnGenerate.Enabled = false;
                CodeCompileUnit ccu1 = NetSqlAzMan.CodeDom.CodeDomGenerator.GenerateCheckAccessHelperClass(this.txtNamespace.Text, this.txtClassName.Text, this.chkAllowRoles.Checked, this.chkAllowTasks.Checked, this.application, this.rbCSharp.Checked ? NetSqlAzMan.CodeDom.Language.CSharp : NetSqlAzMan.CodeDom.Language.VB);
                CodeCompileUnit ccu2 = NetSqlAzMan.CodeDom.CodeDomGenerator.GenerateItemConstants(this.txtNamespace.Text, this.chkAllowRoles.Checked, this.chkAllowTasks.Checked, this.application, this.rbCSharp.Checked ? NetSqlAzMan.CodeDom.Language.CSharp : NetSqlAzMan.CodeDom.Language.VB);
                this.txtSourceCode.Text =
                    NetSqlAzMan.CodeDom.CodeDomGenerator.GenerateSourceCode(ccu1, this.rbCSharp.Checked ? NetSqlAzMan.CodeDom.Language.CSharp : NetSqlAzMan.CodeDom.Language.VB)
                    + "\r\n" +
                    NetSqlAzMan.CodeDom.CodeDomGenerator.GenerateSourceCode(ccu2, this.rbCSharp.Checked ? NetSqlAzMan.CodeDom.Language.CSharp : NetSqlAzMan.CodeDom.Language.VB);
            }
            catch (Exception ex)
            {
                this.HourGlass(false);
                this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("frmGenerateCheckAccessHelper_Msg10"));
            }
            finally
            {
                this.HourGlass(false);
                this.btnGenerate.Enabled = true;
            }
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            Clipboard.SetData(System.Windows.Forms.DataFormats.Text, this.txtSourceCode.Text);
        }

        private void FormValidate()
        {
            bool isValid = true;
            if (this.txtClassName.Text.Trim() == String.Empty)
            {
                this.errorProvider1.SetError(this.txtClassName, Globalization.MultilanguageResource.GetString("frmGenerateCheckAccessHelper_Msg20"));
                isValid = false;
            }
            else
            {
                this.errorProvider1.SetError(this.txtClassName, String.Empty);
            }
            if (this.txtNamespace.Text.Trim() == String.Empty)
            {
                this.errorProvider1.SetError(this.txtNamespace, Globalization.MultilanguageResource.GetString("frmGenerateCheckAccessHelper_Msg30"));
                isValid = false;
            }
            else
            {
                this.errorProvider1.SetError(this.txtNamespace, String.Empty);
            }
            this.btnGenerate.Enabled = isValid;
        }

        private void txtClassName_TextChanged(object sender, EventArgs e)
        {
            this.FormValidate();
        }

        private void txtNamespace_TextChanged(object sender, EventArgs e)
        {
            this.FormValidate();
        }
    }
}