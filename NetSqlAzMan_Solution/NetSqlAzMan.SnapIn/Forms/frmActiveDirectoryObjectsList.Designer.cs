namespace NetSqlAzMan.SnapIn.Forms
{
    partial class frmActiveDirectoryObjectsList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmActiveDirectoryObjectsList));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btnClose = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblInfo = new System.Windows.Forms.Label();
            this.lsvObjectsSid = new System.Windows.Forms.ListView();
            this.sAMAccountNameColumnHeader = new System.Windows.Forms.ColumnHeader(0);
            this.NameColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.objectClassColumnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.ObjectSidcolumnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.label5 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "WindowsQueryLDAPGroup_16x16.ico");
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(602, 367);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "&Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(17, 359);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(664, 1);
            this.panel1.TabIndex = 27;
            // 
            // lblInfo
            // 
            this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInfo.Location = new System.Drawing.Point(53, 343);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(624, 13);
            this.lblInfo.TabIndex = 28;
            this.lblInfo.Text = "LDap Query Result";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lsvObjectsSid
            // 
            this.lsvObjectsSid.AllowColumnReorder = true;
            this.lsvObjectsSid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lsvObjectsSid.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.sAMAccountNameColumnHeader,
            this.NameColumnHeader,
            this.objectClassColumnHeader1,
            this.ObjectSidcolumnHeader1});
            this.lsvObjectsSid.HideSelection = false;
            this.lsvObjectsSid.LargeImageList = this.imageList1;
            this.lsvObjectsSid.Location = new System.Drawing.Point(53, 25);
            this.lsvObjectsSid.MultiSelect = false;
            this.lsvObjectsSid.Name = "lsvObjectsSid";
            this.lsvObjectsSid.ShowGroups = false;
            this.lsvObjectsSid.Size = new System.Drawing.Size(624, 300);
            this.lsvObjectsSid.SmallImageList = this.imageList1;
            this.lsvObjectsSid.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.lsvObjectsSid.TabIndex = 0;
            this.lsvObjectsSid.UseCompatibleStateImageBehavior = false;
            this.lsvObjectsSid.View = System.Windows.Forms.View.Details;
            // 
            // sAMAccountNameColumnHeader
            // 
            this.sAMAccountNameColumnHeader.Text = "sAMAccountName";
            this.sAMAccountNameColumnHeader.Width = 200;
            // 
            // NameColumnHeader
            // 
            this.NameColumnHeader.Text = "Name";
            this.NameColumnHeader.Width = 200;
            // 
            // objectClassColumnHeader1
            // 
            this.objectClassColumnHeader1.Text = "objectClass";
            this.objectClassColumnHeader1.Width = 100;
            // 
            // ObjectSidcolumnHeader1
            // 
            this.ObjectSidcolumnHeader1.Text = "objectSid";
            this.ObjectSidcolumnHeader1.Width = 300;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(50, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(97, 13);
            this.label5.TabIndex = 31;
            this.label5.Text = "LDap Query Result";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::NetSqlAzMan.SnapIn.Properties.Resources.WindowsQueryLDAPGroup_32x32;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 33;
            this.pictureBox1.TabStop = false;
            // 
            // frmActiveDirectoryObjectsList
            // 
            this.AcceptButton = this.btnClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(691, 402);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lsvObjectsSid);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(187, 217);
            this.Name = "frmActiveDirectoryObjectsList";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = " LDap Query Result";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmActiveDirectoryObjectsList_FormClosing);
            this.Load += new System.EventHandler(this.frmActiveDirectoryObjectsList_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ListView lsvObjectsSid;
        private System.Windows.Forms.ColumnHeader sAMAccountNameColumnHeader;
        private System.Windows.Forms.ColumnHeader NameColumnHeader;
        private System.Windows.Forms.ColumnHeader objectClassColumnHeader1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ColumnHeader ObjectSidcolumnHeader1;

    }
}