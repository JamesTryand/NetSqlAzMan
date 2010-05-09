using System;
using System.Windows.Forms;
using NetSqlAzMan.SnapIn.Printing;

namespace NetSqlAzMan.SnapIn.Forms
{
    public partial class frmPrint : frmBase
    {
        internal PrintDocumentBase document;
        public frmPrint()
        {
            InitializeComponent();
        }

        private void frmPrint_Load(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.None;
            if (this.document != null)
                this.changeDocument();
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

        private void frmPrint_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult == DialogResult.None)
                this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Gets or sets the document.
        /// </summary>
        /// <value>The document.</value>
        public PrintDocumentBase Document
        {
            get
            {
                return this.document;
            }
            set
            {
                this.document = value;
                this.changeDocument();
            }
        }

        private void changeDocument()
        {
            this.pageSetupDialog1.Document = this.document;
            this.printPreviewDialog1.Document = this.document;
            this.printDialog1.Document = this.document;
            this.picIcon.Image = this.document.TopIcon;
            this.lblTitle.Text = this.document.Title;
        }

        private void btnPageSetup_Click(object sender, EventArgs e)
        {
            try
            {
                this.HourGlass(true);
                this.pageSetupDialog1.ShowDialog(this);
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("frmPrint.Text"));
            }
            finally
            {
                this.HourGlass(false);
            }
        }

        private void btnPreview_Click(object sender, EventArgs e)
        {
            try
            {
                this.HourGlass(true);
                this.printPreviewDialog1.ShowDialog(this);
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("frmPrint.Text"));
            }
            finally
            {
                this.HourGlass(false);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                this.HourGlass(true);
                DialogResult dr = this.printDialog1.ShowDialog(this);
                if (dr == DialogResult.OK)
                {
                    this.document.Print();
                }
            }
            catch (Exception ex)
            {
                this.ShowError(ex.Message, Globalization.MultilanguageResource.GetString("frmPrint.Text"));
            }
            finally
            {
                this.HourGlass(false);
            }
        }
    }
}