namespace NetSqlAzMan.SnapIn.Forms
{
    partial class frmStoreGroupsProperties
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStoreGroupsProperties));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabGeneral = new System.Windows.Forms.TabPage();
            this.picLDap = new System.Windows.Forms.PictureBox();
            this.picBasic = new System.Windows.Forms.PictureBox();
            this.picImage = new System.Windows.Forms.PictureBox();
            this.txtGroupType = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabLdapQuery = new System.Windows.Forms.TabPage();
            this.btnTestLDapQuery = new System.Windows.Forms.Button();
            this.txtLDapQuery = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tabMembers = new System.Windows.Forms.TabPage();
            this.btnMembersAddDBUsers = new System.Windows.Forms.Button();
            this.lsvMembers = new System.Windows.Forms.ListView();
            this.nameColumnHeader = new System.Windows.Forms.ColumnHeader(0);
            this.whereDefinedColumnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.btnMembersRemove = new System.Windows.Forms.Button();
            this.btnMembersAddWindowsUsersAndGroups = new System.Windows.Forms.Button();
            this.btnMembersAddStoreGroup = new System.Windows.Forms.Button();
            this.tabNonMembers = new System.Windows.Forms.TabPage();
            this.btnNonMembersAddDBUsers = new System.Windows.Forms.Button();
            this.lsvNonMembers = new System.Windows.Forms.ListView();
            this.columnNameHeader1 = new System.Windows.Forms.ColumnHeader(0);
            this.columnWhereDefinedHeader2 = new System.Windows.Forms.ColumnHeader();
            this.label6 = new System.Windows.Forms.Label();
            this.btnNonMembersRemove = new System.Windows.Forms.Button();
            this.btnNonMembersAddWindowsUsersAndGroup = new System.Windows.Forms.Button();
            this.btnNonMembersAddStoreGroup = new System.Windows.Forms.Button();
            this.lblInfo = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnApply = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabGeneral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLDap)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBasic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
            this.tabLdapQuery.SuspendLayout();
            this.tabMembers.SuspendLayout();
            this.tabNonMembers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabGeneral);
            this.tabControl1.Controls.Add(this.tabLdapQuery);
            this.tabControl1.Controls.Add(this.tabMembers);
            this.tabControl1.Controls.Add(this.tabNonMembers);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(571, 288);
            this.tabControl1.TabIndex = 5;
            // 
            // tabGeneral
            // 
            this.tabGeneral.Controls.Add(this.picLDap);
            this.tabGeneral.Controls.Add(this.picBasic);
            this.tabGeneral.Controls.Add(this.picImage);
            this.tabGeneral.Controls.Add(this.txtGroupType);
            this.tabGeneral.Controls.Add(this.label3);
            this.tabGeneral.Controls.Add(this.txtDescription);
            this.tabGeneral.Controls.Add(this.label2);
            this.tabGeneral.Controls.Add(this.txtName);
            this.tabGeneral.Controls.Add(this.label1);
            this.tabGeneral.Location = new System.Drawing.Point(4, 22);
            this.tabGeneral.Name = "tabGeneral";
            this.tabGeneral.Padding = new System.Windows.Forms.Padding(3);
            this.tabGeneral.Size = new System.Drawing.Size(563, 262);
            this.tabGeneral.TabIndex = 0;
            this.tabGeneral.Text = "General";
            this.tabGeneral.UseVisualStyleBackColor = true;
            // 
            // picLDap
            // 
            this.picLDap.Image = global::NetSqlAzMan.SnapIn.Properties.Resources.WindowsQueryLDAPGroup_32x32;
            this.picLDap.Location = new System.Drawing.Point(6, 97);
            this.picLDap.Name = "picLDap";
            this.picLDap.Size = new System.Drawing.Size(32, 32);
            this.picLDap.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picLDap.TabIndex = 38;
            this.picLDap.TabStop = false;
            this.picLDap.Visible = false;
            // 
            // picBasic
            // 
            this.picBasic.Image = global::NetSqlAzMan.SnapIn.Properties.Resources.StoreApplicationGroup_32x32;
            this.picBasic.Location = new System.Drawing.Point(6, 59);
            this.picBasic.Name = "picBasic";
            this.picBasic.Size = new System.Drawing.Size(32, 32);
            this.picBasic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picBasic.TabIndex = 37;
            this.picBasic.TabStop = false;
            this.picBasic.Visible = false;
            // 
            // picImage
            // 
            this.picImage.Image = global::NetSqlAzMan.SnapIn.Properties.Resources.StoreApplicationGroup_32x32;
            this.picImage.Location = new System.Drawing.Point(6, 8);
            this.picImage.Name = "picImage";
            this.picImage.Size = new System.Drawing.Size(32, 32);
            this.picImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picImage.TabIndex = 36;
            this.picImage.TabStop = false;
            // 
            // txtGroupType
            // 
            this.txtGroupType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtGroupType.Enabled = false;
            this.txtGroupType.Location = new System.Drawing.Point(47, 223);
            this.txtGroupType.MaxLength = 255;
            this.txtGroupType.Name = "txtGroupType";
            this.txtGroupType.Size = new System.Drawing.Size(510, 20);
            this.txtGroupType.TabIndex = 2;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(44, 207);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 32;
            this.label3.Text = "Group Type:";
            // 
            // txtDescription
            // 
            this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescription.Location = new System.Drawing.Point(47, 63);
            this.txtDescription.MaxLength = 1024;
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescription.Size = new System.Drawing.Size(510, 141);
            this.txtDescription.TabIndex = 1;
            this.txtDescription.TextChanged += new System.EventHandler(this.txtDescription_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(44, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 31;
            this.label2.Text = "Description:";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(47, 24);
            this.txtName.MaxLength = 255;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(510, 20);
            this.txtName.TabIndex = 0;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 30;
            this.label1.Text = "Name:";
            // 
            // tabLdapQuery
            // 
            this.tabLdapQuery.Controls.Add(this.btnTestLDapQuery);
            this.tabLdapQuery.Controls.Add(this.txtLDapQuery);
            this.tabLdapQuery.Controls.Add(this.label4);
            this.tabLdapQuery.Location = new System.Drawing.Point(4, 22);
            this.tabLdapQuery.Name = "tabLdapQuery";
            this.tabLdapQuery.Size = new System.Drawing.Size(563, 262);
            this.tabLdapQuery.TabIndex = 3;
            this.tabLdapQuery.Text = "LDAP Query";
            this.tabLdapQuery.UseVisualStyleBackColor = true;
            // 
            // btnTestLDapQuery
            // 
            this.btnTestLDapQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnTestLDapQuery.Location = new System.Drawing.Point(417, 236);
            this.btnTestLDapQuery.Name = "btnTestLDapQuery";
            this.btnTestLDapQuery.Size = new System.Drawing.Size(143, 23);
            this.btnTestLDapQuery.TabIndex = 34;
            this.btnTestLDapQuery.Text = "Execute &LDAP Query";
            this.btnTestLDapQuery.UseVisualStyleBackColor = true;
            this.btnTestLDapQuery.Click += new System.EventHandler(this.btnTestLDapQuery_Click);
            // 
            // txtLDapQuery
            // 
            this.txtLDapQuery.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLDapQuery.Location = new System.Drawing.Point(6, 25);
            this.txtLDapQuery.MaxLength = 4096;
            this.txtLDapQuery.Multiline = true;
            this.txtLDapQuery.Name = "txtLDapQuery";
            this.txtLDapQuery.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtLDapQuery.Size = new System.Drawing.Size(554, 205);
            this.txtLDapQuery.TabIndex = 6;
            this.txtLDapQuery.TextChanged += new System.EventHandler(this.txtLDapQuery_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(255, 13);
            this.label4.TabIndex = 33;
            this.label4.Text = "The &LDAP Query that defines members of this group.";
            // 
            // tabMembers
            // 
            this.tabMembers.Controls.Add(this.btnMembersAddDBUsers);
            this.tabMembers.Controls.Add(this.lsvMembers);
            this.tabMembers.Controls.Add(this.label5);
            this.tabMembers.Controls.Add(this.btnMembersRemove);
            this.tabMembers.Controls.Add(this.btnMembersAddWindowsUsersAndGroups);
            this.tabMembers.Controls.Add(this.btnMembersAddStoreGroup);
            this.tabMembers.Location = new System.Drawing.Point(4, 22);
            this.tabMembers.Name = "tabMembers";
            this.tabMembers.Padding = new System.Windows.Forms.Padding(3);
            this.tabMembers.Size = new System.Drawing.Size(563, 262);
            this.tabMembers.TabIndex = 1;
            this.tabMembers.Text = "Members";
            this.tabMembers.UseVisualStyleBackColor = true;
            // 
            // btnMembersAddDBUsers
            // 
            this.btnMembersAddDBUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMembersAddDBUsers.Location = new System.Drawing.Point(252, 206);
            this.btnMembersAddDBUsers.Name = "btnMembersAddDBUsers";
            this.btnMembersAddDBUsers.Size = new System.Drawing.Size(184, 23);
            this.btnMembersAddDBUsers.TabIndex = 31;
            this.btnMembersAddDBUsers.Text = "Add &DB Users";
            this.btnMembersAddDBUsers.UseVisualStyleBackColor = true;
            this.btnMembersAddDBUsers.Click += new System.EventHandler(this.btnMembersAddDBUsers_Click);
            // 
            // lsvMembers
            // 
            this.lsvMembers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lsvMembers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumnHeader,
            this.whereDefinedColumnHeader1});
            this.lsvMembers.FullRowSelect = true;
            this.lsvMembers.HideSelection = false;
            this.lsvMembers.LargeImageList = this.imageList1;
            this.lsvMembers.Location = new System.Drawing.Point(9, 20);
            this.lsvMembers.Name = "lsvMembers";
            this.lsvMembers.ShowGroups = false;
            this.lsvMembers.Size = new System.Drawing.Size(550, 180);
            this.lsvMembers.SmallImageList = this.imageList1;
            this.lsvMembers.TabIndex = 7;
            this.lsvMembers.UseCompatibleStateImageBehavior = false;
            this.lsvMembers.View = System.Windows.Forms.View.Details;
            this.lsvMembers.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lsvMembers_ItemCheck);
            this.lsvMembers.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lsvMembers_ColumnClick);
            this.lsvMembers.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lsvMembers_ItemSelectionChanged);
            // 
            // nameColumnHeader
            // 
            this.nameColumnHeader.Text = "Name";
            this.nameColumnHeader.Width = 267;
            // 
            // whereDefinedColumnHeader1
            // 
            this.whereDefinedColumnHeader1.Text = "Where defined";
            this.whereDefinedColumnHeader1.Width = 150;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Group-Basic.ico");
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 4);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(240, 13);
            this.label5.TabIndex = 29;
            this.label5.Text = "Users and Groups that are members of this group.";
            // 
            // btnMembersRemove
            // 
            this.btnMembersRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMembersRemove.Enabled = false;
            this.btnMembersRemove.Location = new System.Drawing.Point(446, 236);
            this.btnMembersRemove.Name = "btnMembersRemove";
            this.btnMembersRemove.Size = new System.Drawing.Size(111, 23);
            this.btnMembersRemove.TabIndex = 10;
            this.btnMembersRemove.Text = "&Remove";
            this.btnMembersRemove.UseVisualStyleBackColor = true;
            this.btnMembersRemove.Click += new System.EventHandler(this.btnMembersRemove_Click);
            // 
            // btnMembersAddWindowsUsersAndGroups
            // 
            this.btnMembersAddWindowsUsersAndGroups.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMembersAddWindowsUsersAndGroups.Location = new System.Drawing.Point(9, 235);
            this.btnMembersAddWindowsUsersAndGroups.Name = "btnMembersAddWindowsUsersAndGroups";
            this.btnMembersAddWindowsUsersAndGroups.Size = new System.Drawing.Size(237, 23);
            this.btnMembersAddWindowsUsersAndGroups.TabIndex = 9;
            this.btnMembersAddWindowsUsersAndGroups.Text = "Add &Windows Users and Groups";
            this.btnMembersAddWindowsUsersAndGroups.UseVisualStyleBackColor = true;
            this.btnMembersAddWindowsUsersAndGroups.Click += new System.EventHandler(this.btnMembersAddWindowsUsersAndGroups_Click);
            // 
            // btnMembersAddStoreGroup
            // 
            this.btnMembersAddStoreGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnMembersAddStoreGroup.Enabled = false;
            this.btnMembersAddStoreGroup.Location = new System.Drawing.Point(9, 206);
            this.btnMembersAddStoreGroup.Name = "btnMembersAddStoreGroup";
            this.btnMembersAddStoreGroup.Size = new System.Drawing.Size(237, 23);
            this.btnMembersAddStoreGroup.TabIndex = 8;
            this.btnMembersAddStoreGroup.Text = "Add &Store Groups";
            this.btnMembersAddStoreGroup.UseVisualStyleBackColor = true;
            this.btnMembersAddStoreGroup.Click += new System.EventHandler(this.btnMembersAddStoreGroups_Click);
            // 
            // tabNonMembers
            // 
            this.tabNonMembers.Controls.Add(this.btnNonMembersAddDBUsers);
            this.tabNonMembers.Controls.Add(this.lsvNonMembers);
            this.tabNonMembers.Controls.Add(this.label6);
            this.tabNonMembers.Controls.Add(this.btnNonMembersRemove);
            this.tabNonMembers.Controls.Add(this.btnNonMembersAddWindowsUsersAndGroup);
            this.tabNonMembers.Controls.Add(this.btnNonMembersAddStoreGroup);
            this.tabNonMembers.Location = new System.Drawing.Point(4, 22);
            this.tabNonMembers.Name = "tabNonMembers";
            this.tabNonMembers.Size = new System.Drawing.Size(563, 262);
            this.tabNonMembers.TabIndex = 2;
            this.tabNonMembers.Text = "Non Members";
            this.tabNonMembers.UseVisualStyleBackColor = true;
            // 
            // btnNonMembersAddDBUsers
            // 
            this.btnNonMembersAddDBUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNonMembersAddDBUsers.Location = new System.Drawing.Point(252, 204);
            this.btnNonMembersAddDBUsers.Name = "btnNonMembersAddDBUsers";
            this.btnNonMembersAddDBUsers.Size = new System.Drawing.Size(184, 23);
            this.btnNonMembersAddDBUsers.TabIndex = 35;
            this.btnNonMembersAddDBUsers.Text = "Add &DB Users";
            this.btnNonMembersAddDBUsers.UseVisualStyleBackColor = true;
            this.btnNonMembersAddDBUsers.Click += new System.EventHandler(this.btnNonMembersAddDBUsers_Click);
            // 
            // lsvNonMembers
            // 
            this.lsvNonMembers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lsvNonMembers.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnNameHeader1,
            this.columnWhereDefinedHeader2});
            this.lsvNonMembers.FullRowSelect = true;
            this.lsvNonMembers.HideSelection = false;
            this.lsvNonMembers.LargeImageList = this.imageList1;
            this.lsvNonMembers.Location = new System.Drawing.Point(9, 20);
            this.lsvNonMembers.MultiSelect = false;
            this.lsvNonMembers.Name = "lsvNonMembers";
            this.lsvNonMembers.ShowGroups = false;
            this.lsvNonMembers.Size = new System.Drawing.Size(551, 178);
            this.lsvNonMembers.SmallImageList = this.imageList1;
            this.lsvNonMembers.TabIndex = 11;
            this.lsvNonMembers.UseCompatibleStateImageBehavior = false;
            this.lsvNonMembers.View = System.Windows.Forms.View.Details;
            this.lsvNonMembers.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lsvNonMembers_ItemCheck);
            this.lsvNonMembers.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lsvNonMembers_ColumnClick);
            this.lsvNonMembers.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lsvNonMembers_ItemSelectionChanged);
            // 
            // columnNameHeader1
            // 
            this.columnNameHeader1.Text = "Name";
            this.columnNameHeader1.Width = 200;
            // 
            // columnWhereDefinedHeader2
            // 
            this.columnWhereDefinedHeader2.Text = "Where defined";
            this.columnWhereDefinedHeader2.Width = 150;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 4);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(252, 13);
            this.label6.TabIndex = 34;
            this.label6.Text = "Users and Groups that are excluded from this group.";
            // 
            // btnNonMembersRemove
            // 
            this.btnNonMembersRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNonMembersRemove.Enabled = false;
            this.btnNonMembersRemove.Location = new System.Drawing.Point(449, 236);
            this.btnNonMembersRemove.Name = "btnNonMembersRemove";
            this.btnNonMembersRemove.Size = new System.Drawing.Size(111, 23);
            this.btnNonMembersRemove.TabIndex = 14;
            this.btnNonMembersRemove.Text = "&Remove";
            this.btnNonMembersRemove.UseVisualStyleBackColor = true;
            this.btnNonMembersRemove.Click += new System.EventHandler(this.btnNonMembersRemove_Click);
            // 
            // btnNonMembersAddWindowsUsersAndGroup
            // 
            this.btnNonMembersAddWindowsUsersAndGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNonMembersAddWindowsUsersAndGroup.Location = new System.Drawing.Point(9, 233);
            this.btnNonMembersAddWindowsUsersAndGroup.Name = "btnNonMembersAddWindowsUsersAndGroup";
            this.btnNonMembersAddWindowsUsersAndGroup.Size = new System.Drawing.Size(237, 23);
            this.btnNonMembersAddWindowsUsersAndGroup.TabIndex = 13;
            this.btnNonMembersAddWindowsUsersAndGroup.Text = "Add &Windows Users and Groups";
            this.btnNonMembersAddWindowsUsersAndGroup.UseVisualStyleBackColor = true;
            this.btnNonMembersAddWindowsUsersAndGroup.Click += new System.EventHandler(this.btnNonMembersAddWindowsUsersAndGroup_Click);
            // 
            // btnNonMembersAddStoreGroup
            // 
            this.btnNonMembersAddStoreGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnNonMembersAddStoreGroup.Enabled = false;
            this.btnNonMembersAddStoreGroup.Location = new System.Drawing.Point(9, 204);
            this.btnNonMembersAddStoreGroup.Name = "btnNonMembersAddStoreGroup";
            this.btnNonMembersAddStoreGroup.Size = new System.Drawing.Size(237, 23);
            this.btnNonMembersAddStoreGroup.TabIndex = 12;
            this.btnNonMembersAddStoreGroup.Text = "Add &Store Groups";
            this.btnNonMembersAddStoreGroup.UseVisualStyleBackColor = true;
            this.btnNonMembersAddStoreGroup.Click += new System.EventHandler(this.btnNonMembersAddStoreGroup_Click);
            // 
            // lblInfo
            // 
            this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInfo.Location = new System.Drawing.Point(12, 303);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(567, 13);
            this.lblInfo.TabIndex = 28;
            this.lblInfo.Text = "Store Group properties.";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(17, 319);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(568, 1);
            this.panel1.TabIndex = 27;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(346, 327);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(427, 327);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 29;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.Enabled = false;
            this.btnApply.Location = new System.Drawing.Point(508, 327);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 30;
            this.btnApply.Text = "&Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // frmStoreGroupsProperties
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(595, 362);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(466, 356);
            this.Name = "frmStoreGroupsProperties";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " Store Group properties";
            this.Activated += new System.EventHandler(this.frmStoreGroupsProperties_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmGroupsProperties_FormClosing);
            this.Load += new System.EventHandler(this.frmGroupsProperties_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabGeneral.ResumeLayout(false);
            this.tabGeneral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picLDap)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picBasic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
            this.tabLdapQuery.ResumeLayout(false);
            this.tabLdapQuery.PerformLayout();
            this.tabMembers.ResumeLayout(false);
            this.tabMembers.PerformLayout();
            this.tabNonMembers.ResumeLayout(false);
            this.tabNonMembers.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabGeneral;
        private System.Windows.Forms.TabPage tabMembers;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.TabPage tabNonMembers;
        private System.Windows.Forms.TextBox txtGroupType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabLdapQuery;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.TextBox txtLDapQuery;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnMembersRemove;
        private System.Windows.Forms.Button btnMembersAddWindowsUsersAndGroups;
        private System.Windows.Forms.Button btnMembersAddStoreGroup;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ListView lsvMembers;
        private System.Windows.Forms.ColumnHeader nameColumnHeader;
        private System.Windows.Forms.ColumnHeader whereDefinedColumnHeader1;
        private System.Windows.Forms.ListView lsvNonMembers;
        private System.Windows.Forms.ColumnHeader columnNameHeader1;
        private System.Windows.Forms.ColumnHeader columnWhereDefinedHeader2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnNonMembersRemove;
        private System.Windows.Forms.Button btnNonMembersAddWindowsUsersAndGroup;
        private System.Windows.Forms.Button btnNonMembersAddStoreGroup;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.PictureBox picLDap;
        private System.Windows.Forms.PictureBox picBasic;
        private System.Windows.Forms.PictureBox picImage;
        private System.Windows.Forms.Button btnTestLDapQuery;
        private System.Windows.Forms.Button btnMembersAddDBUsers;
        private System.Windows.Forms.Button btnNonMembersAddDBUsers;

    }
}