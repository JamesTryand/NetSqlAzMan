namespace NetSqlAzMan.SnapIn.Forms
{
    partial class frmOptions
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
            this.label1 = new System.Windows.Forms.Label();
            this.rbAdministrator = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.rbDeveloper = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblEventLog = new System.Windows.Forms.Label();
            this.chkErrors = new System.Windows.Forms.CheckBox();
            this.chkWarnings = new System.Windows.Forms.CheckBox();
            this.chkInformations = new System.Windows.Forms.CheckBox();
            this.chkLogOnEventLog = new System.Windows.Forms.CheckBox();
            this.chkLogOnDb = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(62, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(227, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = ".NET Sql Authorization Manager mode:";
            // 
            // rbAdministrator
            // 
            this.rbAdministrator.AutoSize = true;
            this.rbAdministrator.Checked = true;
            this.rbAdministrator.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbAdministrator.Location = new System.Drawing.Point(77, 36);
            this.rbAdministrator.Name = "rbAdministrator";
            this.rbAdministrator.Size = new System.Drawing.Size(136, 17);
            this.rbAdministrator.TabIndex = 0;
            this.rbAdministrator.TabStop = true;
            this.rbAdministrator.Text = "&Administrator mode:";
            this.rbAdministrator.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(97, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "MMC:";
            // 
            // rbDeveloper
            // 
            this.rbDeveloper.AutoSize = true;
            this.rbDeveloper.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rbDeveloper.Location = new System.Drawing.Point(77, 173);
            this.rbDeveloper.Name = "rbDeveloper";
            this.rbDeveloper.Size = new System.Drawing.Size(121, 17);
            this.rbDeveloper.TabIndex = 1;
            this.rbDeveloper.Text = "&Developer mode:";
            this.rbDeveloper.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(174, 266);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(258, 60);
            this.label3.TabIndex = 4;
            this.label3.Text = "In Developer mode, local Windows Users/Groups and Domain User/Groups authorizatio" +
                "ns are NOT skipped by CheckAccess methods.";
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(281, 422);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 7;
            this.btnOk.Text = "&OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(362, 422);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(14, 415);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(423, 1);
            this.panel1.TabIndex = 41;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(174, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(258, 58);
            this.label4.TabIndex = 42;
            this.label4.Text = "In Administrator mode, local Windows Users/Groups and Operations are not visible." +
                "";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(174, 126);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(258, 44);
            this.label5.TabIndex = 44;
            this.label5.Text = "In Administrator mode, local Windows Users/Groups authorizations are skipped by C" +
                "heckAccess methods.";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(97, 126);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 20);
            this.label6.TabIndex = 43;
            this.label6.Text = "Run-Time:";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(97, 266);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(71, 20);
            this.label8.TabIndex = 47;
            this.label8.Text = "Run-Time:";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(97, 203);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(43, 17);
            this.label10.TabIndex = 45;
            this.label10.Text = "MMC:";
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(174, 203);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(258, 54);
            this.label9.TabIndex = 46;
            this.label9.Text = "In Developer mode, users can see Domain/Local Windows Users/Groups and full contr" +
                "ol on Operations.";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::NetSqlAzMan.SnapIn.Properties.Resources.Options_32x32;
            this.pictureBox1.Location = new System.Drawing.Point(12, 9);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 48;
            this.pictureBox1.TabStop = false;
            // 
            // lblEventLog
            // 
            this.lblEventLog.AutoSize = true;
            this.lblEventLog.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEventLog.Location = new System.Drawing.Point(62, 328);
            this.lblEventLog.Name = "lblEventLog";
            this.lblEventLog.Size = new System.Drawing.Size(69, 13);
            this.lblEventLog.TabIndex = 49;
            this.lblEventLog.Text = "Event Log:";
            // 
            // chkErrors
            // 
            this.chkErrors.AutoSize = true;
            this.chkErrors.Location = new System.Drawing.Point(100, 359);
            this.chkErrors.Name = "chkErrors";
            this.chkErrors.Size = new System.Drawing.Size(53, 17);
            this.chkErrors.TabIndex = 4;
            this.chkErrors.Text = "&Errors";
            this.chkErrors.UseVisualStyleBackColor = true;
            // 
            // chkWarnings
            // 
            this.chkWarnings.AutoSize = true;
            this.chkWarnings.Location = new System.Drawing.Point(218, 359);
            this.chkWarnings.Name = "chkWarnings";
            this.chkWarnings.Size = new System.Drawing.Size(71, 17);
            this.chkWarnings.TabIndex = 5;
            this.chkWarnings.Text = "&Warnings";
            this.chkWarnings.UseVisualStyleBackColor = true;
            // 
            // chkInformations
            // 
            this.chkInformations.AutoSize = true;
            this.chkInformations.Location = new System.Drawing.Point(349, 359);
            this.chkInformations.Name = "chkInformations";
            this.chkInformations.Size = new System.Drawing.Size(83, 17);
            this.chkInformations.TabIndex = 6;
            this.chkInformations.Text = "&Informations";
            this.chkInformations.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.chkInformations.UseVisualStyleBackColor = true;
            // 
            // chkLogOnEventLog
            // 
            this.chkLogOnEventLog.AutoSize = true;
            this.chkLogOnEventLog.Location = new System.Drawing.Point(177, 327);
            this.chkLogOnEventLog.Name = "chkLogOnEventLog";
            this.chkLogOnEventLog.Size = new System.Drawing.Size(116, 17);
            this.chkLogOnEventLog.TabIndex = 2;
            this.chkLogOnEventLog.Text = "Log into E&vent Log";
            this.chkLogOnEventLog.UseVisualStyleBackColor = true;
            this.chkLogOnEventLog.CheckedChanged += new System.EventHandler(this.chkLogType_CheckedChanged);
            // 
            // chkLogOnDb
            // 
            this.chkLogOnDb.AutoSize = true;
            this.chkLogOnDb.Location = new System.Drawing.Point(294, 328);
            this.chkLogOnDb.Name = "chkLogOnDb";
            this.chkLogOnDb.Size = new System.Drawing.Size(113, 17);
            this.chkLogOnDb.TabIndex = 3;
            this.chkLogOnDb.Text = "Log into &Database";
            this.chkLogOnDb.UseVisualStyleBackColor = true;
            this.chkLogOnDb.CheckedChanged += new System.EventHandler(this.chkLogType_CheckedChanged);
            // 
            // frmOptions
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(449, 455);
            this.Controls.Add(this.chkLogOnDb);
            this.Controls.Add(this.chkLogOnEventLog);
            this.Controls.Add(this.chkInformations);
            this.Controls.Add(this.chkWarnings);
            this.Controls.Add(this.chkErrors);
            this.Controls.Add(this.lblEventLog);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.rbDeveloper);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rbAdministrator);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmOptions";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " NetSqlAzMan Options";
            this.Load += new System.EventHandler(this.frmOptions_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblEventLog;
        internal System.Windows.Forms.RadioButton rbAdministrator;
        internal System.Windows.Forms.RadioButton rbDeveloper;
        internal System.Windows.Forms.Button btnOk;
        internal System.Windows.Forms.CheckBox chkErrors;
        internal System.Windows.Forms.CheckBox chkWarnings;
        internal System.Windows.Forms.CheckBox chkInformations;
        internal System.Windows.Forms.CheckBox chkLogOnEventLog;
        internal System.Windows.Forms.CheckBox chkLogOnDb;

    }
}