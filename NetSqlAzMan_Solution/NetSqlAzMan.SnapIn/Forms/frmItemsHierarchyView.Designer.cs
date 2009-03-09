namespace NetSqlAzMan.SnapIn.Forms
{
    partial class frmItemsHierarchyView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmItemsHierarchyView));
            this.imageListItemHierarchyView = new System.Windows.Forms.ImageList(this.components);
            this.picImage = new System.Windows.Forms.PictureBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblInfo = new System.Windows.Forms.Label();
            this.itemsHierarchyTreeView = new System.Windows.Forms.TreeView();
            this.pb = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.printPreviewDialog1 = new System.Windows.Forms.PrintPreviewDialog();
            this.pageSetupDialog1 = new System.Windows.Forms.PageSetupDialog();
            this.printDialog1 = new System.Windows.Forms.PrintDialog();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
            this.SuspendLayout();
            // 
            // imageListItemHierarchyView
            // 
            this.imageListItemHierarchyView.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListItemHierarchyView.ImageStream")));
            this.imageListItemHierarchyView.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListItemHierarchyView.Images.SetKeyName(0, "NetSqlAzMan_16x16.ico");
            this.imageListItemHierarchyView.Images.SetKeyName(1, "Store_16x16.ico");
            this.imageListItemHierarchyView.Images.SetKeyName(2, "Application_16x16.ico");
            this.imageListItemHierarchyView.Images.SetKeyName(3, "Role_16x16.ico");
            this.imageListItemHierarchyView.Images.SetKeyName(4, "Task_16x16.ico");
            this.imageListItemHierarchyView.Images.SetKeyName(5, "Operation_16x16.ico");
            // 
            // picImage
            // 
            this.picImage.Image = global::NetSqlAzMan.SnapIn.Properties.Resources.Hierarchy_32x32;
            this.picImage.Location = new System.Drawing.Point(12, 12);
            this.picImage.Name = "picImage";
            this.picImage.Size = new System.Drawing.Size(32, 32);
            this.picImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picImage.TabIndex = 51;
            this.picImage.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(509, 392);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 46;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(12, 385);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(572, 1);
            this.panel1.TabIndex = 49;
            // 
            // lblInfo
            // 
            this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInfo.Location = new System.Drawing.Point(144, 366);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(440, 14);
            this.lblInfo.TabIndex = 50;
            this.lblInfo.Text = "Items Hierarchy View.";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // itemsHierarchyTreeView
            // 
            this.itemsHierarchyTreeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.itemsHierarchyTreeView.FullRowSelect = true;
            this.itemsHierarchyTreeView.ImageIndex = 5;
            this.itemsHierarchyTreeView.ImageList = this.imageListItemHierarchyView;
            this.itemsHierarchyTreeView.Location = new System.Drawing.Point(50, 12);
            this.itemsHierarchyTreeView.Name = "itemsHierarchyTreeView";
            this.itemsHierarchyTreeView.SelectedImageIndex = 0;
            this.itemsHierarchyTreeView.ShowNodeToolTips = true;
            this.itemsHierarchyTreeView.Size = new System.Drawing.Size(534, 351);
            this.itemsHierarchyTreeView.TabIndex = 52;
            // 
            // pb
            // 
            this.pb.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pb.Location = new System.Drawing.Point(12, 392);
            this.pb.Name = "pb";
            this.pb.Size = new System.Drawing.Size(491, 23);
            this.pb.TabIndex = 53;
            this.pb.Visible = false;
            // 
            // lblStatus
            // 
            this.lblStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.Location = new System.Drawing.Point(12, 367);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(126, 13);
            this.lblStatus.TabIndex = 54;
            this.lblStatus.Text = "Building Hierarchy ...";
            // 
            // printPreviewDialog1
            // 
            this.printPreviewDialog1.AutoScrollMargin = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.AutoScrollMinSize = new System.Drawing.Size(0, 0);
            this.printPreviewDialog1.ClientSize = new System.Drawing.Size(400, 300);
            this.printPreviewDialog1.Enabled = true;
            this.printPreviewDialog1.Icon = ((System.Drawing.Icon)(resources.GetObject("printPreviewDialog1.Icon")));
            this.printPreviewDialog1.Name = "printPreviewDialog1";
            this.printPreviewDialog1.Visible = false;
            // 
            // printDialog1
            // 
            this.printDialog1.UseEXDialog = true;
            // 
            // frmItemsHierarchyView
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(597, 425);
            this.Controls.Add(this.lblStatus);
            this.Controls.Add(this.pb);
            this.Controls.Add(this.itemsHierarchyTreeView);
            this.Controls.Add(this.picImage);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblInfo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.Name = "frmItemsHierarchyView";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " Items Hierarchy View";
            this.Activated += new System.EventHandler(this.frmItemsHierarchyView_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmNetSqlAzManBase_FormClosing);
            this.Load += new System.EventHandler(this.frmNetSqlAzManBase_Load);
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox picImage;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.ProgressBar pb;
        private System.Windows.Forms.Label lblStatus;
        internal System.Windows.Forms.TreeView itemsHierarchyTreeView;
        internal System.Windows.Forms.ImageList imageListItemHierarchyView;
        private System.Windows.Forms.PrintPreviewDialog printPreviewDialog1;
        private System.Windows.Forms.PageSetupDialog pageSetupDialog1;
        private System.Windows.Forms.PrintDialog printDialog1;
    }
}