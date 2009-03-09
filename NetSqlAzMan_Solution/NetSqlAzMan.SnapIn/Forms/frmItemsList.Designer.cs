namespace NetSqlAzMan.SnapIn.Forms
{
    partial class frmItemsList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmItemsList));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblInfo2 = new System.Windows.Forms.Label();
            this.lsvItems = new System.Windows.Forms.ListView();
            this.nameColumnHeader = new System.Windows.Forms.ColumnHeader(0);
            this.descriptionColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.lblInfo1 = new System.Windows.Forms.Label();
            this.picOperation = new System.Windows.Forms.PictureBox();
            this.picTask = new System.Windows.Forms.PictureBox();
            this.picRole = new System.Windows.Forms.PictureBox();
            this.picImage = new System.Windows.Forms.PictureBox();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.imageList3 = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOperation)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTask)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRole)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Role_16x16.ico");
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Location = new System.Drawing.Point(394, 300);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "&OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(475, 300);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(17, 292);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(533, 1);
            this.panel1.TabIndex = 27;
            // 
            // lblInfo2
            // 
            this.lblInfo2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInfo2.Location = new System.Drawing.Point(53, 276);
            this.lblInfo2.Name = "lblInfo2";
            this.lblInfo2.Size = new System.Drawing.Size(493, 13);
            this.lblInfo2.TabIndex = 28;
            this.lblInfo2.Text = "Items list.";
            this.lblInfo2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lsvItems
            // 
            this.lsvItems.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lsvItems.CheckBoxes = true;
            this.lsvItems.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumnHeader,
            this.descriptionColumnHeader});
            this.lsvItems.FullRowSelect = true;
            this.lsvItems.HideSelection = false;
            this.lsvItems.Location = new System.Drawing.Point(53, 25);
            this.lsvItems.MultiSelect = false;
            this.lsvItems.Name = "lsvItems";
            this.lsvItems.ShowGroups = false;
            this.lsvItems.Size = new System.Drawing.Size(493, 233);
            this.lsvItems.TabIndex = 0;
            this.lsvItems.UseCompatibleStateImageBehavior = false;
            this.lsvItems.View = System.Windows.Forms.View.Details;
            this.lsvItems.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lsvItems_ColumnClick);
            // 
            // nameColumnHeader
            // 
            this.nameColumnHeader.Text = "Name";
            this.nameColumnHeader.Width = 250;
            // 
            // descriptionColumnHeader
            // 
            this.descriptionColumnHeader.Text = "Description";
            this.descriptionColumnHeader.Width = 300;
            // 
            // lblInfo1
            // 
            this.lblInfo1.AutoSize = true;
            this.lblInfo1.Location = new System.Drawing.Point(50, 9);
            this.lblInfo1.Name = "lblInfo1";
            this.lblInfo1.Size = new System.Drawing.Size(47, 13);
            this.lblInfo1.TabIndex = 31;
            this.lblInfo1.Text = "Items list";
            // 
            // picOperation
            // 
            this.picOperation.Image = global::NetSqlAzMan.SnapIn.Properties.Resources.Operation_32x32;
            this.picOperation.Location = new System.Drawing.Point(12, 126);
            this.picOperation.Name = "picOperation";
            this.picOperation.Size = new System.Drawing.Size(32, 32);
            this.picOperation.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picOperation.TabIndex = 52;
            this.picOperation.TabStop = false;
            this.picOperation.Visible = false;
            // 
            // picTask
            // 
            this.picTask.Image = global::NetSqlAzMan.SnapIn.Properties.Resources.Task_32x32;
            this.picTask.Location = new System.Drawing.Point(12, 88);
            this.picTask.Name = "picTask";
            this.picTask.Size = new System.Drawing.Size(32, 32);
            this.picTask.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picTask.TabIndex = 51;
            this.picTask.TabStop = false;
            this.picTask.Visible = false;
            // 
            // picRole
            // 
            this.picRole.Image = global::NetSqlAzMan.SnapIn.Properties.Resources.Role_32x32;
            this.picRole.Location = new System.Drawing.Point(12, 50);
            this.picRole.Name = "picRole";
            this.picRole.Size = new System.Drawing.Size(32, 32);
            this.picRole.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picRole.TabIndex = 50;
            this.picRole.TabStop = false;
            this.picRole.Visible = false;
            // 
            // picImage
            // 
            this.picImage.Location = new System.Drawing.Point(12, 12);
            this.picImage.Name = "picImage";
            this.picImage.Size = new System.Drawing.Size(32, 32);
            this.picImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picImage.TabIndex = 49;
            this.picImage.TabStop = false;
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "Task_16x16.ico");
            // 
            // imageList3
            // 
            this.imageList3.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList3.ImageStream")));
            this.imageList3.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList3.Images.SetKeyName(0, "Operation_16x16.ico");
            // 
            // frmItemsList
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(560, 335);
            this.Controls.Add(this.picOperation);
            this.Controls.Add(this.picTask);
            this.Controls.Add(this.picRole);
            this.Controls.Add(this.picImage);
            this.Controls.Add(this.lsvItems);
            this.Controls.Add(this.lblInfo1);
            this.Controls.Add(this.lblInfo2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(187, 217);
            this.Name = "frmItemsList";
            this.Text = " Items list";
            this.Activated += new System.EventHandler(this.frmItemsList_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmItemsList_FormClosing);
            this.Load += new System.EventHandler(this.frmItemsList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOperation)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTask)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picRole)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ListView lsvItems;
        private System.Windows.Forms.ColumnHeader nameColumnHeader;
        private System.Windows.Forms.ColumnHeader descriptionColumnHeader;
        private System.Windows.Forms.Label lblInfo1;
        private System.Windows.Forms.Label lblInfo2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.PictureBox picOperation;
        private System.Windows.Forms.PictureBox picTask;
        private System.Windows.Forms.PictureBox picRole;
        private System.Windows.Forms.PictureBox picImage;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.ImageList imageList3;

    }
}