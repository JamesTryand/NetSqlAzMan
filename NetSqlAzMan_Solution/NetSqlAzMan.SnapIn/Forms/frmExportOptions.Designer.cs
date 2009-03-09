namespace NetSqlAzMan.SnapIn.Forms
{
    partial class frmExportOptions
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.chkUsersAndGroups = new System.Windows.Forms.CheckBox();
            this.chkAuthorizations = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnNext = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.chkDBUsers = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "xml";
            this.saveFileDialog1.Filter = "xml files|*.xml|All files|*.*";
            this.saveFileDialog1.SupportMultiDottedExtensions = true;
            this.saveFileDialog1.Title = "Export to ...";
            // 
            // chkUsersAndGroups
            // 
            this.chkUsersAndGroups.AutoSize = true;
            this.chkUsersAndGroups.Location = new System.Drawing.Point(35, 56);
            this.chkUsersAndGroups.Name = "chkUsersAndGroups";
            this.chkUsersAndGroups.Size = new System.Drawing.Size(183, 17);
            this.chkUsersAndGroups.TabIndex = 1;
            this.chkUsersAndGroups.Text = "Include Windows Users / Groups";
            this.chkUsersAndGroups.UseVisualStyleBackColor = true;
            // 
            // chkAuthorizations
            // 
            this.chkAuthorizations.AutoSize = true;
            this.chkAuthorizations.Checked = true;
            this.chkAuthorizations.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAuthorizations.Location = new System.Drawing.Point(35, 33);
            this.chkAuthorizations.Name = "chkAuthorizations";
            this.chkAuthorizations.Size = new System.Drawing.Size(153, 17);
            this.chkAuthorizations.TabIndex = 0;
            this.chkAuthorizations.Text = "Include Item Authorizations";
            this.chkAuthorizations.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(290, 138);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInfo.Location = new System.Drawing.Point(15, 114);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(433, 14);
            this.lblInfo.TabIndex = 33;
            this.lblInfo.Text = "Export options";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(15, 131);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(431, 1);
            this.panel1.TabIndex = 32;
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.Location = new System.Drawing.Point(371, 138);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 4;
            this.btnNext.Text = "&Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(12, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(186, 21);
            this.lblTitle.TabIndex = 34;
            this.lblTitle.Text = "Choose one or more Export options:";
            // 
            // chkDBUsers
            // 
            this.chkDBUsers.AutoSize = true;
            this.chkDBUsers.Location = new System.Drawing.Point(35, 79);
            this.chkDBUsers.Name = "chkDBUsers";
            this.chkDBUsers.Size = new System.Drawing.Size(140, 17);
            this.chkDBUsers.TabIndex = 2;
            this.chkDBUsers.Text = "Include Database Users";
            this.chkDBUsers.UseVisualStyleBackColor = true;
            // 
            // frmExportOptions
            // 
            this.AcceptButton = this.btnNext;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(460, 174);
            this.Controls.Add(this.chkDBUsers);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.chkAuthorizations);
            this.Controls.Add(this.chkUsersAndGroups);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmExportOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " .NET Sql Authorization Manager Export Options";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmExport_FormClosing);
            this.Load += new System.EventHandler(this.frmExport_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.CheckBox chkUsersAndGroups;
        private System.Windows.Forms.CheckBox chkAuthorizations;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.CheckBox chkDBUsers;

    }
}