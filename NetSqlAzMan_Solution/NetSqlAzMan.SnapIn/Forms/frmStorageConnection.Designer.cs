namespace NetSqlAzMan.SnapIn.Forms
{
    partial class frmStorageConnection
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStorageConnection));
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbDataSources = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.rbIntegrated = new System.Windows.Forms.RadioButton();
            this.rbSql = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUserId = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbDatabases = new System.Windows.Forms.ComboBox();
            this.btnRefreshDatabases = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnTestConnection = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblInfo = new System.Windows.Forms.Label();
            this.btnRefreshDataSources = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtOtherSettings = new System.Windows.Forms.TextBox();
            this.picImage = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(295, 204);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 9;
            this.btnOk.Text = "&OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(376, 204);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 10;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Data Source:";
            // 
            // cmbDataSources
            // 
            this.cmbDataSources.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbDataSources.FormattingEnabled = true;
            this.cmbDataSources.Location = new System.Drawing.Point(126, 6);
            this.cmbDataSources.MaxLength = 255;
            this.cmbDataSources.Name = "cmbDataSources";
            this.cmbDataSources.Size = new System.Drawing.Size(215, 21);
            this.cmbDataSources.TabIndex = 0;
            this.cmbDataSources.SelectedIndexChanged += new System.EventHandler(this.cmbDataSource_SelectedIndexChanged);
            this.cmbDataSources.TextChanged += new System.EventHandler(this.cmbDataSource_TextChanged);
            this.cmbDataSources.DropDown += new System.EventHandler(this.cmbDataSource_DropDown);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Authentication:";
            // 
            // rbIntegrated
            // 
            this.rbIntegrated.AutoSize = true;
            this.rbIntegrated.Location = new System.Drawing.Point(126, 33);
            this.rbIntegrated.Name = "rbIntegrated";
            this.rbIntegrated.Size = new System.Drawing.Size(73, 17);
            this.rbIntegrated.TabIndex = 2;
            this.rbIntegrated.Text = "Integrated";
            this.rbIntegrated.UseVisualStyleBackColor = true;
            this.rbIntegrated.CheckedChanged += new System.EventHandler(this.rbAuthentication_CheckedChanged);
            // 
            // rbSql
            // 
            this.rbSql.AutoSize = true;
            this.rbSql.Location = new System.Drawing.Point(216, 33);
            this.rbSql.Name = "rbSql";
            this.rbSql.Size = new System.Drawing.Size(40, 17);
            this.rbSql.TabIndex = 3;
            this.rbSql.Text = "Sql";
            this.rbSql.UseVisualStyleBackColor = true;
            this.rbSql.CheckedChanged += new System.EventHandler(this.rbAuthentication_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "User Id:";
            // 
            // txtUserId
            // 
            this.txtUserId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUserId.Location = new System.Drawing.Point(126, 56);
            this.txtUserId.MaxLength = 255;
            this.txtUserId.Name = "txtUserId";
            this.txtUserId.Size = new System.Drawing.Size(313, 20);
            this.txtUserId.TabIndex = 4;
            this.txtUserId.TextChanged += new System.EventHandler(this.txtUserId_TextChanged);
            // 
            // txtPassword
            // 
            this.txtPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPassword.Location = new System.Drawing.Point(126, 82);
            this.txtPassword.MaxLength = 255;
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(313, 20);
            this.txtPassword.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 85);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Password:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 111);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(93, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "NetSqlAzMan DB:";
            // 
            // cmbDatabases
            // 
            this.cmbDatabases.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbDatabases.FormattingEnabled = true;
            this.cmbDatabases.Location = new System.Drawing.Point(126, 108);
            this.cmbDatabases.MaxLength = 255;
            this.cmbDatabases.Name = "cmbDatabases";
            this.cmbDatabases.Size = new System.Drawing.Size(215, 21);
            this.cmbDatabases.TabIndex = 6;
            this.cmbDatabases.SelectedIndexChanged += new System.EventHandler(this.cmbDatabases_SelectedIndexChanged);
            this.cmbDatabases.TextChanged += new System.EventHandler(this.cmbDatabases_TextChanged);
            this.cmbDatabases.DropDown += new System.EventHandler(this.cmbDatabases_DropDown);
            // 
            // btnRefreshDatabases
            // 
            this.btnRefreshDatabases.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshDatabases.Location = new System.Drawing.Point(364, 106);
            this.btnRefreshDatabases.Name = "btnRefreshDatabases";
            this.btnRefreshDatabases.Size = new System.Drawing.Size(75, 23);
            this.btnRefreshDatabases.TabIndex = 7;
            this.btnRefreshDatabases.Text = "Re&fresh";
            this.btnRefreshDatabases.UseVisualStyleBackColor = true;
            this.btnRefreshDatabases.Click += new System.EventHandler(this.btnRefreshDatabases_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // btnTestConnection
            // 
            this.btnTestConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnTestConnection.Location = new System.Drawing.Point(15, 204);
            this.btnTestConnection.Name = "btnTestConnection";
            this.btnTestConnection.Size = new System.Drawing.Size(75, 23);
            this.btnTestConnection.TabIndex = 8;
            this.btnTestConnection.Text = "&Test";
            this.btnTestConnection.UseVisualStyleBackColor = true;
            this.btnTestConnection.Click += new System.EventHandler(this.btnTestConnection_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(15, 196);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(436, 1);
            this.panel1.TabIndex = 15;
            // 
            // lblInfo
            // 
            this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInfo.Location = new System.Drawing.Point(57, 175);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(394, 15);
            this.lblInfo.TabIndex = 16;
            this.lblInfo.Text = "Manage Authorization Manager Storage connection info.";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // btnRefreshDataSources
            // 
            this.btnRefreshDataSources.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRefreshDataSources.Location = new System.Drawing.Point(364, 4);
            this.btnRefreshDataSources.Name = "btnRefreshDataSources";
            this.btnRefreshDataSources.Size = new System.Drawing.Size(75, 23);
            this.btnRefreshDataSources.TabIndex = 1;
            this.btnRefreshDataSources.Text = "&Refresh";
            this.btnRefreshDataSources.UseVisualStyleBackColor = true;
            this.btnRefreshDataSources.Click += new System.EventHandler(this.btnRefreshDataSources_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 138);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 13);
            this.label6.TabIndex = 17;
            this.label6.Text = "Other Settings:";
            // 
            // txtOtherSettings
            // 
            this.txtOtherSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOtherSettings.Location = new System.Drawing.Point(126, 135);
            this.txtOtherSettings.MaxLength = 255;
            this.txtOtherSettings.Name = "txtOtherSettings";
            this.txtOtherSettings.Size = new System.Drawing.Size(313, 20);
            this.txtOtherSettings.TabIndex = 18;
            this.txtOtherSettings.TextChanged += new System.EventHandler(this.txtOtherSettings_TextChanged);
            // 
            // picImage
            // 
            this.picImage.Image = ((System.Drawing.Image)(resources.GetObject("picImage.Image")));
            this.picImage.Location = new System.Drawing.Point(15, 158);
            this.picImage.Name = "picImage";
            this.picImage.Size = new System.Drawing.Size(32, 32);
            this.picImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picImage.TabIndex = 34;
            this.picImage.TabStop = false;
            // 
            // frmStorageConnection
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 239);
            this.Controls.Add(this.picImage);
            this.Controls.Add(this.txtOtherSettings);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnRefreshDataSources);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnTestConnection);
            this.Controls.Add(this.btnRefreshDatabases);
            this.Controls.Add(this.cmbDatabases);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtUserId);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.rbSql);
            this.Controls.Add(this.rbIntegrated);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbDataSources);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmStorageConnection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " Storage Connection";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmStorageManagement_FormClosing);
            this.Load += new System.EventHandler(this.frmStorageManagement_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbDataSources;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbIntegrated;
        private System.Windows.Forms.RadioButton rbSql;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtUserId;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbDatabases;
        private System.Windows.Forms.Button btnRefreshDatabases;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Button btnTestConnection;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Button btnRefreshDataSources;
        private System.Windows.Forms.TextBox txtOtherSettings;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.PictureBox picImage;

    }
}