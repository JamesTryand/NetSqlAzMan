using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;

namespace NetSqlAzMan.SnapIn.Forms
{
    /// <summary>
    /// Base class for all Windows Forms
    /// </summary>
    public class frmBase : Form
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:frmBase"/> class.
        /// </summary>
        public frmBase() : this(true)
        {
            
        }

        public frmBase(bool localized)
        {
            if (localized)
            {
                this.Load += new EventHandler(frmBase_Load);
            }
        }


        /// <summary>
        /// Handles the Load event of the frmBase control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="T:System.EventArgs"/> instance containing the event data.</param>
        private void frmBase_Load(object sender, EventArgs e)
        {
            NetSqlAzMan.SnapIn.Globalization.ResourcesManager.ManageResource(this);
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // frmBase
            // 
            this.ClientSize = new System.Drawing.Size(292, 273);
            this.Name = "frmBase";
            this.ResumeLayout(false);

        }
    }
}
