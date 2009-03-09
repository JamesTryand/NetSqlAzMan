namespace NetSqlAzMan.SnapIn.Forms
{
    partial class frmItemAuthorizations
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmItemAuthorizations));
            this.btnAddStoreGroups = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.btnRemove = new System.Windows.Forms.Button();
            this.btnAddWindowsUsersAndGroups = new System.Windows.Forms.Button();
            this.btnAddApplicationGroups = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.dgAuthorizations = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.allowWithDelegationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allowToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.denyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.neutralToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.picImage = new System.Windows.Forms.PictureBox();
            this.btnAttributes = new System.Windows.Forms.Button();
            this.btnAddDBUsers = new System.Windows.Forms.Button();
            this.imageListItemHierarchyView = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgAuthorizations)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
            this.SuspendLayout();
            // 
            // btnAddStoreGroups
            // 
            this.btnAddStoreGroups.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddStoreGroups.Enabled = false;
            this.btnAddStoreGroups.Location = new System.Drawing.Point(53, 398);
            this.btnAddStoreGroups.Name = "btnAddStoreGroups";
            this.btnAddStoreGroups.Size = new System.Drawing.Size(250, 23);
            this.btnAddStoreGroups.TabIndex = 2;
            this.btnAddStoreGroups.Text = "Add &Store Groups";
            this.btnAddStoreGroups.UseVisualStyleBackColor = true;
            this.btnAddStoreGroups.Click += new System.EventHandler(this.btnAddStoreGroups_Click);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Group-Basic.ico");
            this.imageList1.Images.SetKeyName(1, "Allow.bmp");
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(50, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 13);
            this.label5.TabIndex = 35;
            this.label5.Text = "Item Authorizations";
            // 
            // btnRemove
            // 
            this.btnRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemove.Enabled = false;
            this.btnRemove.Location = new System.Drawing.Point(661, 369);
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(88, 23);
            this.btnRemove.TabIndex = 4;
            this.btnRemove.Text = "&Remove";
            this.btnRemove.UseVisualStyleBackColor = true;
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // btnAddWindowsUsersAndGroups
            // 
            this.btnAddWindowsUsersAndGroups.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddWindowsUsersAndGroups.Location = new System.Drawing.Point(309, 398);
            this.btnAddWindowsUsersAndGroups.Name = "btnAddWindowsUsersAndGroups";
            this.btnAddWindowsUsersAndGroups.Size = new System.Drawing.Size(247, 23);
            this.btnAddWindowsUsersAndGroups.TabIndex = 3;
            this.btnAddWindowsUsersAndGroups.Text = "Add &Windows Users and Groups";
            this.btnAddWindowsUsersAndGroups.UseVisualStyleBackColor = true;
            this.btnAddWindowsUsersAndGroups.Click += new System.EventHandler(this.btnAddWindowsUsersAndGroups_Click);
            // 
            // btnAddApplicationGroups
            // 
            this.btnAddApplicationGroups.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddApplicationGroups.Enabled = false;
            this.btnAddApplicationGroups.Location = new System.Drawing.Point(53, 427);
            this.btnAddApplicationGroups.Name = "btnAddApplicationGroups";
            this.btnAddApplicationGroups.Size = new System.Drawing.Size(250, 23);
            this.btnAddApplicationGroups.TabIndex = 1;
            this.btnAddApplicationGroups.Text = "Add &Application Groups";
            this.btnAddApplicationGroups.UseVisualStyleBackColor = true;
            this.btnAddApplicationGroups.Click += new System.EventHandler(this.btnAddApplicationGroups_Click);
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Enabled = false;
            this.btnApply.Location = new System.Drawing.Point(670, 489);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 8;
            this.btnApply.Text = "&Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(589, 489);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInfo.Location = new System.Drawing.Point(53, 461);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(692, 17);
            this.lblInfo.TabIndex = 39;
            this.lblInfo.Text = "Item Authorizations.";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(12, 481);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(733, 1);
            this.panel1.TabIndex = 38;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(508, 489);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 6;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // dgAuthorizations
            // 
            this.dgAuthorizations.AllowUserToAddRows = false;
            this.dgAuthorizations.AllowUserToDeleteRows = false;
            this.dgAuthorizations.AllowUserToOrderColumns = true;
            this.dgAuthorizations.AllowUserToResizeRows = false;
            this.dgAuthorizations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgAuthorizations.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgAuthorizations.BackgroundColor = System.Drawing.Color.White;
            this.dgAuthorizations.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dgAuthorizations.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgAuthorizations.Location = new System.Drawing.Point(53, 25);
            this.dgAuthorizations.Name = "dgAuthorizations";
            this.dgAuthorizations.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgAuthorizations.Size = new System.Drawing.Size(696, 338);
            this.dgAuthorizations.TabIndex = 0;
            this.dgAuthorizations.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgAuthorizations_CellClick);
            this.dgAuthorizations.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgAuthorizations_CellEndEdit);
            this.dgAuthorizations.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgAuthorizations_DataError);
            this.dgAuthorizations.SelectionChanged += new System.EventHandler(this.dgAuthorizations_SelectionChanged);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.allowWithDelegationToolStripMenuItem,
            this.allowToolStripMenuItem,
            this.denyToolStripMenuItem,
            this.neutralToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(188, 92);
            // 
            // allowWithDelegationToolStripMenuItem
            // 
            this.allowWithDelegationToolStripMenuItem.Image = global::NetSqlAzMan.SnapIn.Properties.Resources.AllowForDelegation;
            this.allowWithDelegationToolStripMenuItem.Name = "allowWithDelegationToolStripMenuItem";
            this.allowWithDelegationToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.allowWithDelegationToolStripMenuItem.Text = "Allow with Delegation";
            this.allowWithDelegationToolStripMenuItem.Click += new System.EventHandler(this.allowWithDelegationToolStripMenuItem_Click);
            // 
            // allowToolStripMenuItem
            // 
            this.allowToolStripMenuItem.Image = global::NetSqlAzMan.SnapIn.Properties.Resources.Allow;
            this.allowToolStripMenuItem.Name = "allowToolStripMenuItem";
            this.allowToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.allowToolStripMenuItem.Text = "Allow";
            this.allowToolStripMenuItem.Click += new System.EventHandler(this.allowToolStripMenuItem_Click);
            // 
            // denyToolStripMenuItem
            // 
            this.denyToolStripMenuItem.Image = global::NetSqlAzMan.SnapIn.Properties.Resources.Deny;
            this.denyToolStripMenuItem.Name = "denyToolStripMenuItem";
            this.denyToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.denyToolStripMenuItem.Text = "Deny";
            this.denyToolStripMenuItem.Click += new System.EventHandler(this.denyToolStripMenuItem_Click);
            // 
            // neutralToolStripMenuItem
            // 
            this.neutralToolStripMenuItem.Image = global::NetSqlAzMan.SnapIn.Properties.Resources.Neutral;
            this.neutralToolStripMenuItem.Name = "neutralToolStripMenuItem";
            this.neutralToolStripMenuItem.Size = new System.Drawing.Size(187, 22);
            this.neutralToolStripMenuItem.Text = "Neutral";
            this.neutralToolStripMenuItem.Click += new System.EventHandler(this.neutralToolStripMenuItem_Click);
            // 
            // picImage
            // 
            this.picImage.Image = global::NetSqlAzMan.SnapIn.Properties.Resources.ItemAuthorization_32x32;
            this.picImage.Location = new System.Drawing.Point(12, 12);
            this.picImage.Name = "picImage";
            this.picImage.Size = new System.Drawing.Size(32, 32);
            this.picImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picImage.TabIndex = 44;
            this.picImage.TabStop = false;
            // 
            // btnAttributes
            // 
            this.btnAttributes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAttributes.Location = new System.Drawing.Point(567, 369);
            this.btnAttributes.Name = "btnAttributes";
            this.btnAttributes.Size = new System.Drawing.Size(88, 23);
            this.btnAttributes.TabIndex = 5;
            this.btnAttributes.Text = "A&ttributes";
            this.btnAttributes.UseVisualStyleBackColor = true;
            this.btnAttributes.Click += new System.EventHandler(this.btnAttributes_Click);
            // 
            // btnAddDBUsers
            // 
            this.btnAddDBUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddDBUsers.Location = new System.Drawing.Point(309, 427);
            this.btnAddDBUsers.Name = "btnAddDBUsers";
            this.btnAddDBUsers.Size = new System.Drawing.Size(247, 23);
            this.btnAddDBUsers.TabIndex = 45;
            this.btnAddDBUsers.Text = "Add &DB Users";
            this.btnAddDBUsers.UseVisualStyleBackColor = true;
            this.btnAddDBUsers.Click += new System.EventHandler(this.btnAddDBUsers_Click);
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
            // frmItemAuthorizations
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(761, 524);
            this.Controls.Add(this.picImage);
            this.Controls.Add(this.dgAuthorizations);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAddDBUsers);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnAddStoreGroups);
            this.Controls.Add(this.btnAddApplicationGroups);
            this.Controls.Add(this.btnAddWindowsUsersAndGroups);
            this.Controls.Add(this.btnRemove);
            this.Controls.Add(this.btnAttributes);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(637, 200);
            this.Name = "frmItemAuthorizations";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " Authorizations";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAuthorizations_FormClosing);
            this.Load += new System.EventHandler(this.frmAuthorizations_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgAuthorizations)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAddStoreGroups;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnRemove;
        private System.Windows.Forms.Button btnAddWindowsUsersAndGroups;
        private System.Windows.Forms.Button btnAddApplicationGroups;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.DataGridView dgAuthorizations;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem allowWithDelegationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allowToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem denyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem neutralToolStripMenuItem;
        private System.Windows.Forms.PictureBox picImage;
        private System.Windows.Forms.Button btnAttributes;
        private System.Windows.Forms.Button btnAddDBUsers;
        private System.Windows.Forms.ImageList imageListItemHierarchyView;
    }
}