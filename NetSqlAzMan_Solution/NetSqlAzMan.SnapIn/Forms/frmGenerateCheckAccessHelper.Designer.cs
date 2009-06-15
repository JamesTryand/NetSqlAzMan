namespace NetSqlAzMan.SnapIn.Forms
{
    partial class frmGenerateCheckAccessHelper
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGenerateCheckAccessHelper));
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.chkAllowRoles = new System.Windows.Forms.CheckBox();
            this.chkAllowTasks = new System.Windows.Forms.CheckBox();
            this.chkAllowOperations = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtClassName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.rbCSharp = new System.Windows.Forms.RadioButton();
            this.rbVBNet = new System.Windows.Forms.RadioButton();
            this.txtNamespace = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.gbSourceCode = new System.Windows.Forms.GroupBox();
            this.btnCopy = new System.Windows.Forms.Button();
            this.txtSourceCode = new System.Windows.Forms.TextBox();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.gbSourceCode.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Location = new System.Drawing.Point(483, 34);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(208, 20);
            this.lblTitle.TabIndex = 41;
            this.lblTitle.Text = "Choose one or more generation options:";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(638, 533);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 8;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInfo.Location = new System.Drawing.Point(18, 510);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(695, 13);
            this.lblInfo.TabIndex = 40;
            this.lblInfo.Text = "Generate options";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(17, 526);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(696, 1);
            this.panel1.TabIndex = 39;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerate.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerate.Location = new System.Drawing.Point(610, 340);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(85, 23);
            this.btnGenerate.TabIndex = 7;
            this.btnGenerate.Text = "&Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // chkAllowRoles
            // 
            this.chkAllowRoles.AutoSize = true;
            this.chkAllowRoles.Location = new System.Drawing.Point(498, 57);
            this.chkAllowRoles.Name = "chkAllowRoles";
            this.chkAllowRoles.Size = new System.Drawing.Size(148, 17);
            this.chkAllowRoles.TabIndex = 2;
            this.chkAllowRoles.Text = "Allow Role Check Access";
            this.chkAllowRoles.UseVisualStyleBackColor = true;
            // 
            // chkAllowTasks
            // 
            this.chkAllowTasks.AutoSize = true;
            this.chkAllowTasks.Location = new System.Drawing.Point(498, 80);
            this.chkAllowTasks.Name = "chkAllowTasks";
            this.chkAllowTasks.Size = new System.Drawing.Size(150, 17);
            this.chkAllowTasks.TabIndex = 3;
            this.chkAllowTasks.Text = "Allow Task Check Access";
            this.chkAllowTasks.UseVisualStyleBackColor = true;
            // 
            // chkAllowOperations
            // 
            this.chkAllowOperations.AutoSize = true;
            this.chkAllowOperations.Checked = true;
            this.chkAllowOperations.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllowOperations.Enabled = false;
            this.chkAllowOperations.Location = new System.Drawing.Point(498, 103);
            this.chkAllowOperations.Name = "chkAllowOperations";
            this.chkAllowOperations.Size = new System.Drawing.Size(172, 17);
            this.chkAllowOperations.TabIndex = 4;
            this.chkAllowOperations.Text = "Allow Operation Check Access";
            this.chkAllowOperations.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 43;
            this.label1.Text = "Class Name:";
            // 
            // txtClassName
            // 
            this.txtClassName.Location = new System.Drawing.Point(229, 34);
            this.txtClassName.Name = "txtClassName";
            this.txtClassName.Size = new System.Drawing.Size(248, 20);
            this.txtClassName.TabIndex = 0;
            this.txtClassName.Text = "CheckAccessHelper";
            this.txtClassName.TextChanged += new System.EventHandler(this.txtClassName_TextChanged);
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(14, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(123, 21);
            this.label2.TabIndex = 45;
            this.label2.Text = "Source Code Language:";
            // 
            // rbCSharp
            // 
            this.rbCSharp.AutoSize = true;
            this.rbCSharp.Checked = true;
            this.rbCSharp.Location = new System.Drawing.Point(228, 97);
            this.rbCSharp.Name = "rbCSharp";
            this.rbCSharp.Size = new System.Drawing.Size(39, 17);
            this.rbCSharp.TabIndex = 5;
            this.rbCSharp.TabStop = true;
            this.rbCSharp.Text = "C#";
            this.rbCSharp.UseVisualStyleBackColor = true;
            // 
            // rbVBNet
            // 
            this.rbVBNet.AutoSize = true;
            this.rbVBNet.Location = new System.Drawing.Point(283, 97);
            this.rbVBNet.Name = "rbVBNet";
            this.rbVBNet.Size = new System.Drawing.Size(39, 17);
            this.rbVBNet.TabIndex = 6;
            this.rbVBNet.Text = "VB";
            this.rbVBNet.UseVisualStyleBackColor = true;
            // 
            // txtNamespace
            // 
            this.txtNamespace.Location = new System.Drawing.Point(229, 60);
            this.txtNamespace.Name = "txtNamespace";
            this.txtNamespace.Size = new System.Drawing.Size(248, 20);
            this.txtNamespace.TabIndex = 1;
            this.txtNamespace.Text = "[ApplicationName].Security";
            this.txtNamespace.TextChanged += new System.EventHandler(this.txtNamespace_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 48;
            this.label3.Text = "Namespace:";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(15, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(433, 15);
            this.label4.TabIndex = 50;
            this.label4.Text = "Generate CheckAccessHelper Class:";
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Location = new System.Drawing.Point(18, 25);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(696, 1);
            this.panel2.TabIndex = 51;
            // 
            // gbSourceCode
            // 
            this.gbSourceCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbSourceCode.Controls.Add(this.btnCopy);
            this.gbSourceCode.Controls.Add(this.txtSourceCode);
            this.gbSourceCode.Controls.Add(this.btnGenerate);
            this.gbSourceCode.Location = new System.Drawing.Point(12, 126);
            this.gbSourceCode.Name = "gbSourceCode";
            this.gbSourceCode.Size = new System.Drawing.Size(701, 369);
            this.gbSourceCode.TabIndex = 52;
            this.gbSourceCode.TabStop = false;
            this.gbSourceCode.Text = "Source Code";
            // 
            // btnCopy
            // 
            this.btnCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopy.Location = new System.Drawing.Point(525, 340);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(75, 23);
            this.btnCopy.TabIndex = 2;
            this.btnCopy.Text = "Copy &All";
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // txtSourceCode
            // 
            this.txtSourceCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSourceCode.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.txtSourceCode.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSourceCode.ForeColor = System.Drawing.Color.Yellow;
            this.txtSourceCode.Location = new System.Drawing.Point(6, 19);
            this.txtSourceCode.Multiline = true;
            this.txtSourceCode.Name = "txtSourceCode";
            this.txtSourceCode.ReadOnly = true;
            this.txtSourceCode.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSourceCode.Size = new System.Drawing.Size(689, 315);
            this.txtSourceCode.TabIndex = 1;
            this.txtSourceCode.WordWrap = false;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // frmGenerateCheckAccessHelper
            // 
            this.AcceptButton = this.btnGenerate;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(725, 568);
            this.Controls.Add(this.gbSourceCode);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtNamespace);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.rbVBNet);
            this.Controls.Add(this.rbCSharp);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtClassName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkAllowOperations);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.chkAllowRoles);
            this.Controls.Add(this.chkAllowTasks);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "frmGenerateCheckAccessHelper";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Generate CheckAccessHelper Class";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmNetSqlAzManBase_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmNetSqlAzManBase_FormClosing);
            this.gbSourceCode.ResumeLayout(false);
            this.gbSourceCode.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.CheckBox chkAllowRoles;
        private System.Windows.Forms.CheckBox chkAllowTasks;
        private System.Windows.Forms.CheckBox chkAllowOperations;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtClassName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbCSharp;
        private System.Windows.Forms.RadioButton rbVBNet;
        private System.Windows.Forms.TextBox txtNamespace;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox gbSourceCode;
        private System.Windows.Forms.TextBox txtSourceCode;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.ErrorProvider errorProvider1;


    }
}