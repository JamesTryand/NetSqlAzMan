namespace NetSqlAzMan_WinPerformanceTest
{
    partial class Form1
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
            this.btnAzManTestCheckAccess = new System.Windows.Forms.Button();
            this.chkAzManMultiThread = new System.Windows.Forms.CheckBox();
            this.txtAzManTestCheckAccessCount = new System.Windows.Forms.TextBox();
            this.txtOperation = new System.Windows.Forms.TextBox();
            this.txtAzManCheckAccessResults = new System.Windows.Forms.TextBox();
            this.lblAzManCheckAccess = new System.Windows.Forms.Label();
            this.lblNetSqlAzManCheckAccess = new System.Windows.Forms.Label();
            this.txtNetSqlAzManCheckAccessResults = new System.Windows.Forms.TextBox();
            this.txtItem = new System.Windows.Forms.TextBox();
            this.txtNetSqlAzManTestCheckAccessCount = new System.Windows.Forms.TextBox();
            this.chkNetSqlAzManMultiThread = new System.Windows.Forms.CheckBox();
            this.btnNetSqlAzManTestCheckAccess = new System.Windows.Forms.Button();
            this.lblIM = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnAzManTestCheckAccess
            // 
            this.btnAzManTestCheckAccess.Location = new System.Drawing.Point(115, 69);
            this.btnAzManTestCheckAccess.Name = "btnAzManTestCheckAccess";
            this.btnAzManTestCheckAccess.Size = new System.Drawing.Size(178, 23);
            this.btnAzManTestCheckAccess.TabIndex = 0;
            this.btnAzManTestCheckAccess.Text = "AzMan Test Check Access";
            this.btnAzManTestCheckAccess.UseVisualStyleBackColor = true;
            this.btnAzManTestCheckAccess.Click += new System.EventHandler(this.btnAzManTestCheckAccess_Click);
            // 
            // chkAzManMultiThread
            // 
            this.chkAzManMultiThread.AutoSize = true;
            this.chkAzManMultiThread.Location = new System.Drawing.Point(405, 75);
            this.chkAzManMultiThread.Name = "chkAzManMultiThread";
            this.chkAzManMultiThread.Size = new System.Drawing.Size(85, 17);
            this.chkAzManMultiThread.TabIndex = 2;
            this.chkAzManMultiThread.Text = "Multi Thread";
            this.chkAzManMultiThread.UseVisualStyleBackColor = true;
            // 
            // txtAzManTestCheckAccessCount
            // 
            this.txtAzManTestCheckAccessCount.Location = new System.Drawing.Point(9, 71);
            this.txtAzManTestCheckAccessCount.Name = "txtAzManTestCheckAccessCount";
            this.txtAzManTestCheckAccessCount.Size = new System.Drawing.Size(100, 20);
            this.txtAzManTestCheckAccessCount.TabIndex = 4;
            this.txtAzManTestCheckAccessCount.Text = "1";
            // 
            // txtOperation
            // 
            this.txtOperation.Location = new System.Drawing.Point(299, 72);
            this.txtOperation.Name = "txtOperation";
            this.txtOperation.Size = new System.Drawing.Size(100, 20);
            this.txtOperation.TabIndex = 5;
            this.txtOperation.Text = "Operation Test";
            // 
            // txtAzManCheckAccessResults
            // 
            this.txtAzManCheckAccessResults.Location = new System.Drawing.Point(9, 97);
            this.txtAzManCheckAccessResults.Name = "txtAzManCheckAccessResults";
            this.txtAzManCheckAccessResults.Size = new System.Drawing.Size(100, 20);
            this.txtAzManCheckAccessResults.TabIndex = 6;
            // 
            // lblAzManCheckAccess
            // 
            this.lblAzManCheckAccess.AutoSize = true;
            this.lblAzManCheckAccess.Location = new System.Drawing.Point(115, 104);
            this.lblAzManCheckAccess.Name = "lblAzManCheckAccess";
            this.lblAzManCheckAccess.Size = new System.Drawing.Size(38, 13);
            this.lblAzManCheckAccess.TabIndex = 7;
            this.lblAzManCheckAccess.Text = "(result)";
            // 
            // lblNetSqlAzManCheckAccess
            // 
            this.lblNetSqlAzManCheckAccess.AutoSize = true;
            this.lblNetSqlAzManCheckAccess.Location = new System.Drawing.Point(115, 189);
            this.lblNetSqlAzManCheckAccess.Name = "lblNetSqlAzManCheckAccess";
            this.lblNetSqlAzManCheckAccess.Size = new System.Drawing.Size(38, 13);
            this.lblNetSqlAzManCheckAccess.TabIndex = 13;
            this.lblNetSqlAzManCheckAccess.Text = "(result)";
            // 
            // txtNetSqlAzManCheckAccessResults
            // 
            this.txtNetSqlAzManCheckAccessResults.Location = new System.Drawing.Point(9, 182);
            this.txtNetSqlAzManCheckAccessResults.Name = "txtNetSqlAzManCheckAccessResults";
            this.txtNetSqlAzManCheckAccessResults.Size = new System.Drawing.Size(100, 20);
            this.txtNetSqlAzManCheckAccessResults.TabIndex = 12;
            // 
            // txtItem
            // 
            this.txtItem.Location = new System.Drawing.Point(299, 157);
            this.txtItem.Name = "txtItem";
            this.txtItem.Size = new System.Drawing.Size(100, 20);
            this.txtItem.TabIndex = 11;
            this.txtItem.Text = "Item Test";
            // 
            // txtNetSqlAzManTestCheckAccessCount
            // 
            this.txtNetSqlAzManTestCheckAccessCount.Location = new System.Drawing.Point(9, 156);
            this.txtNetSqlAzManTestCheckAccessCount.Name = "txtNetSqlAzManTestCheckAccessCount";
            this.txtNetSqlAzManTestCheckAccessCount.Size = new System.Drawing.Size(100, 20);
            this.txtNetSqlAzManTestCheckAccessCount.TabIndex = 10;
            this.txtNetSqlAzManTestCheckAccessCount.Text = "1";
            // 
            // chkNetSqlAzManMultiThread
            // 
            this.chkNetSqlAzManMultiThread.AutoSize = true;
            this.chkNetSqlAzManMultiThread.Location = new System.Drawing.Point(405, 160);
            this.chkNetSqlAzManMultiThread.Name = "chkNetSqlAzManMultiThread";
            this.chkNetSqlAzManMultiThread.Size = new System.Drawing.Size(85, 17);
            this.chkNetSqlAzManMultiThread.TabIndex = 9;
            this.chkNetSqlAzManMultiThread.Text = "Multi Thread";
            this.chkNetSqlAzManMultiThread.UseVisualStyleBackColor = true;
            // 
            // btnNetSqlAzManTestCheckAccess
            // 
            this.btnNetSqlAzManTestCheckAccess.Location = new System.Drawing.Point(115, 154);
            this.btnNetSqlAzManTestCheckAccess.Name = "btnNetSqlAzManTestCheckAccess";
            this.btnNetSqlAzManTestCheckAccess.Size = new System.Drawing.Size(178, 23);
            this.btnNetSqlAzManTestCheckAccess.TabIndex = 8;
            this.btnNetSqlAzManTestCheckAccess.Text = "NetSqlAzMan Test Check Access";
            this.btnNetSqlAzManTestCheckAccess.UseVisualStyleBackColor = true;
            this.btnNetSqlAzManTestCheckAccess.Click += new System.EventHandler(this.btnNetSqlAzManTestCheckAccess_Click);
            // 
            // lblIM
            // 
            this.lblIM.AutoSize = true;
            this.lblIM.Location = new System.Drawing.Point(12, 9);
            this.lblIM.Name = "lblIM";
            this.lblIM.Size = new System.Drawing.Size(35, 13);
            this.lblIM.TabIndex = 14;
            this.lblIM.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 238);
            this.Controls.Add(this.lblIM);
            this.Controls.Add(this.lblNetSqlAzManCheckAccess);
            this.Controls.Add(this.txtNetSqlAzManCheckAccessResults);
            this.Controls.Add(this.txtItem);
            this.Controls.Add(this.txtNetSqlAzManTestCheckAccessCount);
            this.Controls.Add(this.chkNetSqlAzManMultiThread);
            this.Controls.Add(this.btnNetSqlAzManTestCheckAccess);
            this.Controls.Add(this.lblAzManCheckAccess);
            this.Controls.Add(this.txtAzManCheckAccessResults);
            this.Controls.Add(this.txtOperation);
            this.Controls.Add(this.txtAzManTestCheckAccessCount);
            this.Controls.Add(this.chkAzManMultiThread);
            this.Controls.Add(this.btnAzManTestCheckAccess);
            this.Name = "Form1";
            this.Text = "AzMan vs NetSqlAzMan (Performance & Scalability Test)";
            this.Load += new System.EventHandler(this.Page_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAzManTestCheckAccess;
        private System.Windows.Forms.CheckBox chkAzManMultiThread;
        private System.Windows.Forms.TextBox txtAzManTestCheckAccessCount;
        private System.Windows.Forms.TextBox txtOperation;
        private System.Windows.Forms.TextBox txtAzManCheckAccessResults;
        private System.Windows.Forms.Label lblAzManCheckAccess;
        private System.Windows.Forms.Label lblNetSqlAzManCheckAccess;
        private System.Windows.Forms.TextBox txtNetSqlAzManCheckAccessResults;
        private System.Windows.Forms.TextBox txtItem;
        private System.Windows.Forms.TextBox txtNetSqlAzManTestCheckAccessCount;
        private System.Windows.Forms.CheckBox chkNetSqlAzManMultiThread;
        private System.Windows.Forms.Button btnNetSqlAzManTestCheckAccess;
        private System.Windows.Forms.Label lblIM;
    }
}

