namespace NetSqlAzMan.SnapIn.Forms
{
    partial class frmStorePermissions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStorePermissions));
            this.lblInfo = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblDescription = new System.Windows.Forms.Label();
            this.picReaders = new System.Windows.Forms.PictureBox();
            this.picUsers = new System.Windows.Forms.PictureBox();
            this.picManagers = new System.Windows.Forms.PictureBox();
            this.chkManagers = new System.Windows.Forms.CheckedListBox();
            this.chkUsers = new System.Windows.Forms.CheckedListBox();
            this.chkReaders = new System.Windows.Forms.CheckedListBox();
            this.lblReaders = new System.Windows.Forms.Label();
            this.lblUsers = new System.Windows.Forms.Label();
            this.lblManagers = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picReaders)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picUsers)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picManagers)).BeginInit();
            this.SuspendLayout();
            // 
            // lblInfo
            // 
            this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInfo.Location = new System.Drawing.Point(16, 370);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(713, 13);
            this.lblInfo.TabIndex = 28;
            this.lblInfo.Text = "Store Permissions.";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(17, 386);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(712, 1);
            this.panel1.TabIndex = 27;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(570, 394);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(651, 394);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblDescription
            // 
            this.lblDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDescription.ForeColor = System.Drawing.Color.Blue;
            this.lblDescription.Location = new System.Drawing.Point(12, 9);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(714, 48);
            this.lblDescription.TabIndex = 50;
            this.lblDescription.Text = resources.GetString("lblDescription.Text");
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picReaders
            // 
            this.picReaders.Image = ((System.Drawing.Image)(resources.GetObject("picReaders.Image")));
            this.picReaders.Location = new System.Drawing.Point(492, 60);
            this.picReaders.Name = "picReaders";
            this.picReaders.Size = new System.Drawing.Size(48, 48);
            this.picReaders.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picReaders.TabIndex = 59;
            this.picReaders.TabStop = false;
            // 
            // picUsers
            // 
            this.picUsers.Image = ((System.Drawing.Image)(resources.GetObject("picUsers.Image")));
            this.picUsers.Location = new System.Drawing.Point(252, 60);
            this.picUsers.Name = "picUsers";
            this.picUsers.Size = new System.Drawing.Size(48, 48);
            this.picUsers.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picUsers.TabIndex = 58;
            this.picUsers.TabStop = false;
            // 
            // picManagers
            // 
            this.picManagers.Image = ((System.Drawing.Image)(resources.GetObject("picManagers.Image")));
            this.picManagers.Location = new System.Drawing.Point(12, 60);
            this.picManagers.Name = "picManagers";
            this.picManagers.Size = new System.Drawing.Size(48, 48);
            this.picManagers.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picManagers.TabIndex = 57;
            this.picManagers.TabStop = false;
            // 
            // chkManagers
            // 
            this.chkManagers.CheckOnClick = true;
            this.chkManagers.FormattingEnabled = true;
            this.chkManagers.HorizontalScrollbar = true;
            this.chkManagers.Location = new System.Drawing.Point(66, 60);
            this.chkManagers.Name = "chkManagers";
            this.chkManagers.ScrollAlwaysVisible = true;
            this.chkManagers.Size = new System.Drawing.Size(180, 289);
            this.chkManagers.TabIndex = 0;
            // 
            // chkUsers
            // 
            this.chkUsers.CheckOnClick = true;
            this.chkUsers.FormattingEnabled = true;
            this.chkUsers.HorizontalScrollbar = true;
            this.chkUsers.Location = new System.Drawing.Point(306, 60);
            this.chkUsers.Name = "chkUsers";
            this.chkUsers.ScrollAlwaysVisible = true;
            this.chkUsers.Size = new System.Drawing.Size(180, 289);
            this.chkUsers.TabIndex = 1;
            // 
            // chkReaders
            // 
            this.chkReaders.CheckOnClick = true;
            this.chkReaders.FormattingEnabled = true;
            this.chkReaders.HorizontalScrollbar = true;
            this.chkReaders.Location = new System.Drawing.Point(546, 60);
            this.chkReaders.Name = "chkReaders";
            this.chkReaders.ScrollAlwaysVisible = true;
            this.chkReaders.Size = new System.Drawing.Size(180, 289);
            this.chkReaders.TabIndex = 2;
            // 
            // lblReaders
            // 
            this.lblReaders.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReaders.ForeColor = System.Drawing.Color.Red;
            this.lblReaders.Location = new System.Drawing.Point(546, 352);
            this.lblReaders.Name = "lblReaders";
            this.lblReaders.Size = new System.Drawing.Size(180, 18);
            this.lblReaders.TabIndex = 65;
            this.lblReaders.Text = "Store Readers";
            this.lblReaders.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblUsers
            // 
            this.lblUsers.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUsers.ForeColor = System.Drawing.Color.Green;
            this.lblUsers.Location = new System.Drawing.Point(306, 352);
            this.lblUsers.Name = "lblUsers";
            this.lblUsers.Size = new System.Drawing.Size(180, 18);
            this.lblUsers.TabIndex = 64;
            this.lblUsers.Text = "Store Users";
            this.lblUsers.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblManagers
            // 
            this.lblManagers.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblManagers.ForeColor = System.Drawing.Color.Blue;
            this.lblManagers.Location = new System.Drawing.Point(63, 352);
            this.lblManagers.Name = "lblManagers";
            this.lblManagers.Size = new System.Drawing.Size(183, 18);
            this.lblManagers.TabIndex = 63;
            this.lblManagers.Text = "Store Managers";
            this.lblManagers.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // frmStorePermissions
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(739, 429);
            this.Controls.Add(this.lblReaders);
            this.Controls.Add(this.lblUsers);
            this.Controls.Add(this.lblManagers);
            this.Controls.Add(this.chkReaders);
            this.Controls.Add(this.chkUsers);
            this.Controls.Add(this.chkManagers);
            this.Controls.Add(this.picReaders);
            this.Controls.Add(this.picUsers);
            this.Controls.Add(this.picManagers);
            this.Controls.Add(this.lblDescription);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MinimumSize = new System.Drawing.Size(466, 356);
            this.Name = "frmStorePermissions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " Store Permissions";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmStorePermissions_FormClosing);
            this.Load += new System.EventHandler(this.frmStorePermissions_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picReaders)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picUsers)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picManagers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.PictureBox picReaders;
        private System.Windows.Forms.PictureBox picUsers;
        private System.Windows.Forms.PictureBox picManagers;
        private System.Windows.Forms.CheckedListBox chkManagers;
        private System.Windows.Forms.CheckedListBox chkUsers;
        private System.Windows.Forms.CheckedListBox chkReaders;
        private System.Windows.Forms.Label lblReaders;
        private System.Windows.Forms.Label lblUsers;
        private System.Windows.Forms.Label lblManagers;

    }
}