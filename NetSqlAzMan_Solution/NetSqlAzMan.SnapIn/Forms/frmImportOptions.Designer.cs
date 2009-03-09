namespace NetSqlAzMan.SnapIn.Forms
{
    partial class frmImportOptions
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
            this.chkUsersAndGroups = new System.Windows.Forms.CheckBox();
            this.chkAuthorizations = new System.Windows.Forms.CheckBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnNext = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.chkDBUsers = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkCreatesNewItemAuthorizations = new System.Windows.Forms.CheckBox();
            this.chkDeleteMissingItemAuthorizations = new System.Windows.Forms.CheckBox();
            this.chkOverwritesItemAuthorizations = new System.Windows.Forms.CheckBox();
            this.chkCreatesNewItems = new System.Windows.Forms.CheckBox();
            this.chkDeleteMissingItems = new System.Windows.Forms.CheckBox();
            this.chkOverwritesExistingItems = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkUsersAndGroups
            // 
            this.chkUsersAndGroups.AutoSize = true;
            this.chkUsersAndGroups.Location = new System.Drawing.Point(10, 42);
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
            this.chkAuthorizations.Location = new System.Drawing.Point(10, 19);
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
            this.btnCancel.Location = new System.Drawing.Point(290, 330);
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
            this.lblInfo.Location = new System.Drawing.Point(35, 307);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(412, 13);
            this.lblInfo.TabIndex = 33;
            this.lblInfo.Text = "Import options";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(15, 323);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(431, 1);
            this.panel1.TabIndex = 32;
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.Location = new System.Drawing.Point(371, 330);
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
            this.lblTitle.Text = "Choose one or more Import options:";
            // 
            // chkDBUsers
            // 
            this.chkDBUsers.AutoSize = true;
            this.chkDBUsers.Location = new System.Drawing.Point(10, 65);
            this.chkDBUsers.Name = "chkDBUsers";
            this.chkDBUsers.Size = new System.Drawing.Size(140, 17);
            this.chkDBUsers.TabIndex = 2;
            this.chkDBUsers.Text = "Include Database Users";
            this.chkDBUsers.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.chkAuthorizations);
            this.groupBox1.Controls.Add(this.chkDBUsers);
            this.groupBox1.Controls.Add(this.chkUsersAndGroups);
            this.groupBox1.Location = new System.Drawing.Point(15, 33);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(431, 96);
            this.groupBox1.TabIndex = 35;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Import options";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.chkCreatesNewItemAuthorizations);
            this.groupBox2.Controls.Add(this.chkDeleteMissingItemAuthorizations);
            this.groupBox2.Controls.Add(this.chkOverwritesItemAuthorizations);
            this.groupBox2.Controls.Add(this.chkCreatesNewItems);
            this.groupBox2.Controls.Add(this.chkDeleteMissingItems);
            this.groupBox2.Controls.Add(this.chkOverwritesExistingItems);
            this.groupBox2.Location = new System.Drawing.Point(17, 135);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(431, 163);
            this.groupBox2.TabIndex = 36;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Merge options";
            // 
            // chkCreatesNewItemAuthorizations
            // 
            this.chkCreatesNewItemAuthorizations.AutoSize = true;
            this.chkCreatesNewItemAuthorizations.Checked = true;
            this.chkCreatesNewItemAuthorizations.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCreatesNewItemAuthorizations.Location = new System.Drawing.Point(10, 88);
            this.chkCreatesNewItemAuthorizations.Name = "chkCreatesNewItemAuthorizations";
            this.chkCreatesNewItemAuthorizations.Size = new System.Drawing.Size(176, 17);
            this.chkCreatesNewItemAuthorizations.TabIndex = 3;
            this.chkCreatesNewItemAuthorizations.Text = "Creates new Item authorizations";
            this.chkCreatesNewItemAuthorizations.UseVisualStyleBackColor = true;
            // 
            // chkDeleteMissingItemAuthorizations
            // 
            this.chkDeleteMissingItemAuthorizations.AutoSize = true;
            this.chkDeleteMissingItemAuthorizations.Location = new System.Drawing.Point(10, 134);
            this.chkDeleteMissingItemAuthorizations.Name = "chkDeleteMissingItemAuthorizations";
            this.chkDeleteMissingItemAuthorizations.Size = new System.Drawing.Size(185, 17);
            this.chkDeleteMissingItemAuthorizations.TabIndex = 5;
            this.chkDeleteMissingItemAuthorizations.Text = "Delete missing Item authorizations";
            this.chkDeleteMissingItemAuthorizations.UseVisualStyleBackColor = true;
            // 
            // chkOverwritesItemAuthorizations
            // 
            this.chkOverwritesItemAuthorizations.AutoSize = true;
            this.chkOverwritesItemAuthorizations.Location = new System.Drawing.Point(10, 111);
            this.chkOverwritesItemAuthorizations.Name = "chkOverwritesItemAuthorizations";
            this.chkOverwritesItemAuthorizations.Size = new System.Drawing.Size(167, 17);
            this.chkOverwritesItemAuthorizations.TabIndex = 4;
            this.chkOverwritesItemAuthorizations.Text = "Overwrites Item authorizations";
            this.chkOverwritesItemAuthorizations.UseVisualStyleBackColor = true;
            // 
            // chkCreatesNewItems
            // 
            this.chkCreatesNewItems.AutoSize = true;
            this.chkCreatesNewItems.Checked = true;
            this.chkCreatesNewItems.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCreatesNewItems.Location = new System.Drawing.Point(10, 19);
            this.chkCreatesNewItems.Name = "chkCreatesNewItems";
            this.chkCreatesNewItems.Size = new System.Drawing.Size(113, 17);
            this.chkCreatesNewItems.TabIndex = 0;
            this.chkCreatesNewItems.Text = "Creates new Items";
            this.chkCreatesNewItems.UseVisualStyleBackColor = true;
            // 
            // chkDeleteMissingItems
            // 
            this.chkDeleteMissingItems.AutoSize = true;
            this.chkDeleteMissingItems.Location = new System.Drawing.Point(10, 65);
            this.chkDeleteMissingItems.Name = "chkDeleteMissingItems";
            this.chkDeleteMissingItems.Size = new System.Drawing.Size(122, 17);
            this.chkDeleteMissingItems.TabIndex = 2;
            this.chkDeleteMissingItems.Text = "Delete missing Items";
            this.chkDeleteMissingItems.UseVisualStyleBackColor = true;
            // 
            // chkOverwritesExistingItems
            // 
            this.chkOverwritesExistingItems.AutoSize = true;
            this.chkOverwritesExistingItems.Location = new System.Drawing.Point(10, 42);
            this.chkOverwritesExistingItems.Name = "chkOverwritesExistingItems";
            this.chkOverwritesExistingItems.Size = new System.Drawing.Size(142, 17);
            this.chkOverwritesExistingItems.TabIndex = 1;
            this.chkOverwritesExistingItems.Text = "Overwrites existing Items";
            this.chkOverwritesExistingItems.UseVisualStyleBackColor = true;
            // 
            // frmImportOptions
            // 
            this.AcceptButton = this.btnNext;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(460, 366);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnNext);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmImportOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " .NET Sql Authorization Manager Import Options";
            this.Load += new System.EventHandler(this.frmImportOptions_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmImportOptions_FormClosing);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox chkUsersAndGroups;
        private System.Windows.Forms.CheckBox chkAuthorizations;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.CheckBox chkDBUsers;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkCreatesNewItemAuthorizations;
        private System.Windows.Forms.CheckBox chkDeleteMissingItemAuthorizations;
        private System.Windows.Forms.CheckBox chkOverwritesItemAuthorizations;
        private System.Windows.Forms.CheckBox chkCreatesNewItems;
        private System.Windows.Forms.CheckBox chkDeleteMissingItems;
        private System.Windows.Forms.CheckBox chkOverwritesExistingItems;

    }
}