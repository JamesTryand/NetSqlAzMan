namespace NetSqlAzMan.SnapIn.Forms
{
    partial class frmInvalidateWCFCacheService
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtWCFCacheServiceEndPoint = new System.Windows.Forms.TextBox();
            this.btnInvalidateCache = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(153, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "WCF Cache Service EndPoint:";
            // 
            // txtWCFCacheServiceEndPoint
            // 
            this.txtWCFCacheServiceEndPoint.Location = new System.Drawing.Point(171, 12);
            this.txtWCFCacheServiceEndPoint.MaxLength = 4000;
            this.txtWCFCacheServiceEndPoint.Name = "txtWCFCacheServiceEndPoint";
            this.txtWCFCacheServiceEndPoint.Size = new System.Drawing.Size(385, 20);
            this.txtWCFCacheServiceEndPoint.TabIndex = 0;
            this.txtWCFCacheServiceEndPoint.Text = "net.tcp://localhost:8000/NetSqlAzMan.Cache.Service/CacheService/";
            // 
            // btnInvalidateCache
            // 
            this.btnInvalidateCache.Location = new System.Drawing.Point(351, 38);
            this.btnInvalidateCache.Name = "btnInvalidateCache";
            this.btnInvalidateCache.Size = new System.Drawing.Size(124, 23);
            this.btnInvalidateCache.TabIndex = 2;
            this.btnInvalidateCache.Text = "&InvalidateCache()";
            this.btnInvalidateCache.UseVisualStyleBackColor = true;
            this.btnInvalidateCache.Click += new System.EventHandler(this.btnInvalidateCache_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(12, 266);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(544, 15);
            this.lblStatus.TabIndex = 38;
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(481, 38);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 39;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // frmInvalidateWCFCacheService
            // 
            this.AcceptButton = this.btnInvalidateCache;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(568, 77);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.txtWCFCacheServiceEndPoint);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.btnInvalidateCache);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmInvalidateWCFCacheService";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Invalidate WCF Cache Service";
            this.Load += new System.EventHandler(this.frmImportFromAzMan_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmImportFromAzMan_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtWCFCacheServiceEndPoint;
        private System.Windows.Forms.Button btnInvalidateCache;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.ErrorProvider errorProvider1;

    }
}