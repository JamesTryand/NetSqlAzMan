namespace NetSqlAzMan.SnapIn.Forms
{
    partial class frmSQLAudit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSQLAudit));
            this.txtDDLScript = new System.Windows.Forms.TextBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.lnkSQLAudit = new System.Windows.Forms.LinkLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbGenerateScript = new System.Windows.Forms.ToolStripSplitButton();
            this.tsbCreateTablesTriggerAndViews = new System.Windows.Forms.ToolStripMenuItem();
            this.tspDropTablesTriggersAndViews = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbCreateAuditTriggersOnly = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbDropAuditTriggersOnly = new System.Windows.Forms.ToolStripMenuItem();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.lblSQLAuditDescription = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtDDLScript
            // 
            this.txtDDLScript.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDDLScript.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtDDLScript.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDDLScript.ForeColor = System.Drawing.SystemColors.ControlText;
            this.txtDDLScript.Location = new System.Drawing.Point(65, 57);
            this.txtDDLScript.MaxLength = 4096;
            this.txtDDLScript.Multiline = true;
            this.txtDDLScript.Name = "txtDDLScript";
            this.txtDDLScript.ReadOnly = true;
            this.txtDDLScript.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDDLScript.Size = new System.Drawing.Size(874, 435);
            this.txtDDLScript.TabIndex = 0;
            this.txtDDLScript.TabStop = false;
            this.txtDDLScript.WordWrap = false;
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitle.Location = new System.Drawing.Point(62, 9);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(877, 45);
            this.lblTitle.TabIndex = 49;
            this.lblTitle.Text = "Generate T-SQL Script to enable Auditing on NetSqlAzMan Storage.";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(8, 9);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 48);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 47;
            this.pictureBox1.TabStop = false;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(16, 537);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(923, 1);
            this.panel1.TabIndex = 44;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(864, 545);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lnkSQLAudit
            // 
            this.lnkSQLAudit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lnkSQLAudit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lnkSQLAudit.Location = new System.Drawing.Point(788, 521);
            this.lnkSQLAudit.Name = "lnkSQLAudit";
            this.lnkSQLAudit.Size = new System.Drawing.Size(151, 13);
            this.lnkSQLAudit.TabIndex = 50;
            this.lnkSQLAudit.TabStop = true;
            this.lnkSQLAudit.Text = "http://sqlaudit.sourceforge.net";
            this.lnkSQLAudit.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lnkSQLAudit.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lnkSQLAudit_LinkClicked);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbGenerateScript,
            this.progressBar});
            this.toolStrip1.Location = new System.Drawing.Point(65, 496);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(177, 25);
            this.toolStrip1.TabIndex = 51;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbGenerateScript
            // 
            this.tsbGenerateScript.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbGenerateScript.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbCreateTablesTriggerAndViews,
            this.tspDropTablesTriggersAndViews,
            this.tsbCreateAuditTriggersOnly,
            this.tsbDropAuditTriggersOnly});
            this.tsbGenerateScript.Image = ((System.Drawing.Image)(resources.GetObject("tsbGenerateScript.Image")));
            this.tsbGenerateScript.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbGenerateScript.Name = "tsbGenerateScript";
            this.tsbGenerateScript.Size = new System.Drawing.Size(32, 22);
            this.tsbGenerateScript.Text = "Script Generation MMC.SyncAction";
            // 
            // tsbCreateTablesTriggerAndViews
            // 
            this.tsbCreateTablesTriggerAndViews.Image = ((System.Drawing.Image)(resources.GetObject("tsbCreateTablesTriggerAndViews.Image")));
            this.tsbCreateTablesTriggerAndViews.Name = "tsbCreateTablesTriggerAndViews";
            this.tsbCreateTablesTriggerAndViews.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D1)));
            this.tsbCreateTablesTriggerAndViews.Size = new System.Drawing.Size(317, 22);
            this.tsbCreateTablesTriggerAndViews.Text = "Create Audit Tables, Trigger and Views";
            this.tsbCreateTablesTriggerAndViews.Click += new System.EventHandler(this.tsbCreateTablesTriggerAndViews_Click);
            // 
            // tspDropTablesTriggersAndViews
            // 
            this.tspDropTablesTriggersAndViews.Image = ((System.Drawing.Image)(resources.GetObject("tspDropTablesTriggersAndViews.Image")));
            this.tspDropTablesTriggersAndViews.Name = "tspDropTablesTriggersAndViews";
            this.tspDropTablesTriggersAndViews.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D2)));
            this.tspDropTablesTriggersAndViews.Size = new System.Drawing.Size(317, 22);
            this.tspDropTablesTriggersAndViews.Text = "Drop Audit Tables, Triggers and Views";
            this.tspDropTablesTriggersAndViews.Click += new System.EventHandler(this.tspDropTablesTriggersAndViews_Click);
            // 
            // tsbCreateAuditTriggersOnly
            // 
            this.tsbCreateAuditTriggersOnly.Image = ((System.Drawing.Image)(resources.GetObject("tsbCreateAuditTriggersOnly.Image")));
            this.tsbCreateAuditTriggersOnly.Name = "tsbCreateAuditTriggersOnly";
            this.tsbCreateAuditTriggersOnly.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D3)));
            this.tsbCreateAuditTriggersOnly.Size = new System.Drawing.Size(317, 22);
            this.tsbCreateAuditTriggersOnly.Text = "Create Audit Triggers only";
            this.tsbCreateAuditTriggersOnly.Click += new System.EventHandler(this.tsbCreateAuditTriggersOnly_Click);
            // 
            // tsbDropAuditTriggersOnly
            // 
            this.tsbDropAuditTriggersOnly.Image = ((System.Drawing.Image)(resources.GetObject("tsbDropAuditTriggersOnly.Image")));
            this.tsbDropAuditTriggersOnly.Name = "tsbDropAuditTriggersOnly";
            this.tsbDropAuditTriggersOnly.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D4)));
            this.tsbDropAuditTriggersOnly.Size = new System.Drawing.Size(317, 22);
            this.tsbDropAuditTriggersOnly.Text = "Drop Audit Triggers only";
            this.tsbDropAuditTriggersOnly.Click += new System.EventHandler(this.tsbDropAuditTriggersOnly_Click);
            // 
            // progressBar
            // 
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(100, 22);
            // 
            // lblSQLAuditDescription
            // 
            this.lblSQLAuditDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSQLAuditDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSQLAuditDescription.Location = new System.Drawing.Point(214, 496);
            this.lblSQLAuditDescription.Name = "lblSQLAuditDescription";
            this.lblSQLAuditDescription.Size = new System.Drawing.Size(725, 25);
            this.lblSQLAuditDescription.TabIndex = 52;
            this.lblSQLAuditDescription.Text = "SQLAudit is open source project (Ms-PL) by Andrea Ferendeles.";
            this.lblSQLAuditDescription.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // frmSQLAudit
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(949, 580);
            this.Controls.Add(this.lblSQLAuditDescription);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.lnkSQLAudit);
            this.Controls.Add(this.txtDDLScript);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnClose);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "frmSQLAudit";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Auditing";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmSQLAudit_FormClosing);
            this.Load += new System.EventHandler(this.frmSQLAudit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtDDLScript;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.LinkLabel lnkSQLAudit;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSplitButton tsbGenerateScript;
        private System.Windows.Forms.ToolStripMenuItem tsbCreateTablesTriggerAndViews;
        private System.Windows.Forms.ToolStripMenuItem tspDropTablesTriggersAndViews;
        private System.Windows.Forms.ToolStripMenuItem tsbCreateAuditTriggersOnly;
        private System.Windows.Forms.ToolStripMenuItem tsbDropAuditTriggersOnly;
        private System.Windows.Forms.Label lblSQLAuditDescription;
        private System.Windows.Forms.ToolStripProgressBar progressBar;


    }
}