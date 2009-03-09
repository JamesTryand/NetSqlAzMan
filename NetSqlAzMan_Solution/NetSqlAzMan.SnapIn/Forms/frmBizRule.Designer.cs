namespace NetSqlAzMan.SnapIn.Forms
{
    partial class frmBizRule
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBizRule));
            this.txtSourceCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.lblInfo = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.rbVBNet = new System.Windows.Forms.RadioButton();
            this.rbCSharp = new System.Windows.Forms.RadioButton();
            this.btnReloadBizRule = new System.Windows.Forms.Button();
            this.btnClearBizRule = new System.Windows.Forms.Button();
            this.btnNewFromTemplate = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSourceCode
            // 
            this.txtSourceCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSourceCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.txtSourceCode.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSourceCode.ForeColor = System.Drawing.Color.Yellow;
            this.txtSourceCode.Location = new System.Drawing.Point(49, 49);
            this.txtSourceCode.MaxLength = 4096;
            this.txtSourceCode.Multiline = true;
            this.txtSourceCode.Name = "txtSourceCode";
            this.txtSourceCode.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSourceCode.Size = new System.Drawing.Size(681, 314);
            this.txtSourceCode.TabIndex = 0;
            this.txtSourceCode.TabStop = false;
            this.txtSourceCode.WordWrap = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(46, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 49;
            this.label2.Text = "Source code:";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::NetSqlAzMan.SnapIn.Properties.Resources.BizRule;
            this.pictureBox1.Location = new System.Drawing.Point(8, 9);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 47;
            this.pictureBox1.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(46, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(46, 13);
            this.label7.TabIndex = 46;
            this.label7.Text = "Biz Rule";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // lblInfo
            // 
            this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInfo.Location = new System.Drawing.Point(49, 464);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(681, 13);
            this.lblInfo.TabIndex = 45;
            this.lblInfo.Text = "Biz Rule";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(16, 480);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(716, 1);
            this.panel1.TabIndex = 44;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(657, 488);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(46, 366);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 50;
            this.label1.Text = "Source type:";
            // 
            // rbVBNet
            // 
            this.rbVBNet.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rbVBNet.AutoSize = true;
            this.rbVBNet.Location = new System.Drawing.Point(63, 405);
            this.rbVBNet.Name = "rbVBNet";
            this.rbVBNet.Size = new System.Drawing.Size(64, 17);
            this.rbVBNet.TabIndex = 2;
            this.rbVBNet.Text = "&VB.NET";
            this.rbVBNet.UseVisualStyleBackColor = true;
            // 
            // rbCSharp
            // 
            this.rbCSharp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.rbCSharp.AutoSize = true;
            this.rbCSharp.Checked = true;
            this.rbCSharp.Location = new System.Drawing.Point(63, 382);
            this.rbCSharp.Name = "rbCSharp";
            this.rbCSharp.Size = new System.Drawing.Size(39, 17);
            this.rbCSharp.TabIndex = 1;
            this.rbCSharp.TabStop = true;
            this.rbCSharp.Text = "&C#";
            this.rbCSharp.UseVisualStyleBackColor = true;
            // 
            // btnReloadBizRule
            // 
            this.btnReloadBizRule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnReloadBizRule.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnReloadBizRule.Location = new System.Drawing.Point(49, 428);
            this.btnReloadBizRule.Name = "btnReloadBizRule";
            this.btnReloadBizRule.Size = new System.Drawing.Size(165, 23);
            this.btnReloadBizRule.TabIndex = 3;
            this.btnReloadBizRule.Text = "&Reload rule into Store";
            this.btnReloadBizRule.UseVisualStyleBackColor = true;
            this.btnReloadBizRule.Click += new System.EventHandler(this.btnReloadBizRule_Click);
            // 
            // btnClearBizRule
            // 
            this.btnClearBizRule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClearBizRule.Location = new System.Drawing.Point(220, 428);
            this.btnClearBizRule.Name = "btnClearBizRule";
            this.btnClearBizRule.Size = new System.Drawing.Size(165, 23);
            this.btnClearBizRule.TabIndex = 4;
            this.btnClearBizRule.Text = "&Clear rule from Store";
            this.btnClearBizRule.UseVisualStyleBackColor = true;
            this.btnClearBizRule.Click += new System.EventHandler(this.btnClearBizRule_Click);
            // 
            // btnNewFromTemplate
            // 
            this.btnNewFromTemplate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNewFromTemplate.Location = new System.Drawing.Point(391, 428);
            this.btnNewFromTemplate.Name = "btnNewFromTemplate";
            this.btnNewFromTemplate.Size = new System.Drawing.Size(165, 23);
            this.btnNewFromTemplate.TabIndex = 5;
            this.btnNewFromTemplate.Text = "&New Biz Rule";
            this.btnNewFromTemplate.UseVisualStyleBackColor = true;
            this.btnNewFromTemplate.Click += new System.EventHandler(this.btnNewFromTemplate_Click);
            // 
            // frmBizRule
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(742, 523);
            this.Controls.Add(this.btnNewFromTemplate);
            this.Controls.Add(this.btnReloadBizRule);
            this.Controls.Add(this.btnClearBizRule);
            this.Controls.Add(this.rbCSharp);
            this.Controls.Add(this.rbVBNet);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSourceCode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnClose);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "frmBizRule";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Biz Rule";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmBizRule_FormClosing);
            this.Load += new System.EventHandler(this.frmBizRule_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSourceCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.RadioButton rbCSharp;
        private System.Windows.Forms.RadioButton rbVBNet;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnReloadBizRule;
        private System.Windows.Forms.Button btnClearBizRule;
        private System.Windows.Forms.Button btnNewFromTemplate;


    }
}