namespace NetSqlAzMan.SnapIn.Forms
{
    partial class frmItemProperties
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmItemProperties));
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblInfo = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.picImage = new System.Windows.Forms.PictureBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabItemDefinition = new System.Windows.Forms.TabPage();
            this.btnBizRule = new System.Windows.Forms.Button();
            this.btnAttributes = new System.Windows.Forms.Button();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TabRoles = new System.Windows.Forms.TabPage();
            this.btnAddRole = new System.Windows.Forms.Button();
            this.btnRemoveRole = new System.Windows.Forms.Button();
            this.lsvRoles = new System.Windows.Forms.ListView();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader(0);
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.TabTasks = new System.Windows.Forms.TabPage();
            this.btnAddTask = new System.Windows.Forms.Button();
            this.btnRemoveTask = new System.Windows.Forms.Button();
            this.lsvTasks = new System.Windows.Forms.ListView();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader(0);
            this.columnHeader4 = new System.Windows.Forms.ColumnHeader();
            this.imageList2 = new System.Windows.Forms.ImageList(this.components);
            this.TabOperations = new System.Windows.Forms.TabPage();
            this.btnAddOperation = new System.Windows.Forms.Button();
            this.btnRemoveOperation = new System.Windows.Forms.Button();
            this.lsvOperations = new System.Windows.Forms.ListView();
            this.nameColumnHeader = new System.Windows.Forms.ColumnHeader(0);
            this.descriptionColumnHeader = new System.Windows.Forms.ColumnHeader();
            this.imageList3 = new System.Windows.Forms.ImageList(this.components);
            this.btnApply = new System.Windows.Forms.Button();
            this.picRole = new System.Windows.Forms.PictureBox();
            this.picTask = new System.Windows.Forms.PictureBox();
            this.picOperation = new System.Windows.Forms.PictureBox();
            this.imageListItemHierarchyView = new System.Windows.Forms.ImageList(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabItemDefinition.SuspendLayout();
            this.TabRoles.SuspendLayout();
            this.TabTasks.SuspendLayout();
            this.TabOperations.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picRole)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTask)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOperation)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Enabled = false;
            this.btnOk.Location = new System.Drawing.Point(349, 378);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "&OK";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(430, 377);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Location = new System.Drawing.Point(18, 370);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(567, 1);
            this.panel1.TabIndex = 37;
            // 
            // lblInfo
            // 
            this.lblInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblInfo.Location = new System.Drawing.Point(50, 351);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(536, 13);
            this.lblInfo.TabIndex = 38;
            this.lblInfo.Text = "Create a new Item.";
            this.lblInfo.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // picImage
            // 
            this.picImage.Location = new System.Drawing.Point(12, 10);
            this.picImage.Name = "picImage";
            this.picImage.Size = new System.Drawing.Size(32, 32);
            this.picImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picImage.TabIndex = 45;
            this.picImage.TabStop = false;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabItemDefinition);
            this.tabControl1.Controls.Add(this.TabRoles);
            this.tabControl1.Controls.Add(this.TabTasks);
            this.tabControl1.Controls.Add(this.TabOperations);
            this.tabControl1.Location = new System.Drawing.Point(50, 10);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(536, 338);
            this.tabControl1.TabIndex = 8;
            this.tabControl1.TabStop = false;
            // 
            // tabItemDefinition
            // 
            this.tabItemDefinition.Controls.Add(this.btnBizRule);
            this.tabItemDefinition.Controls.Add(this.btnAttributes);
            this.tabItemDefinition.Controls.Add(this.txtDescription);
            this.tabItemDefinition.Controls.Add(this.label2);
            this.tabItemDefinition.Controls.Add(this.txtName);
            this.tabItemDefinition.Controls.Add(this.label1);
            this.tabItemDefinition.Location = new System.Drawing.Point(4, 22);
            this.tabItemDefinition.Name = "tabItemDefinition";
            this.tabItemDefinition.Padding = new System.Windows.Forms.Padding(3);
            this.tabItemDefinition.Size = new System.Drawing.Size(528, 312);
            this.tabItemDefinition.TabIndex = 45;
            this.tabItemDefinition.Text = "Item Definition";
            this.tabItemDefinition.UseVisualStyleBackColor = true;
            // 
            // btnBizRule
            // 
            this.btnBizRule.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBizRule.Location = new System.Drawing.Point(324, 283);
            this.btnBizRule.Name = "btnBizRule";
            this.btnBizRule.Size = new System.Drawing.Size(192, 23);
            this.btnBizRule.TabIndex = 46;
            this.btnBizRule.Text = "&Biz Rule";
            this.btnBizRule.UseVisualStyleBackColor = true;
            this.btnBizRule.Click += new System.EventHandler(this.btnBizRule_Click);
            // 
            // btnAttributes
            // 
            this.btnAttributes.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAttributes.Location = new System.Drawing.Point(9, 283);
            this.btnAttributes.Name = "btnAttributes";
            this.btnAttributes.Size = new System.Drawing.Size(198, 23);
            this.btnAttributes.TabIndex = 45;
            this.btnAttributes.Text = "A&ttributes";
            this.btnAttributes.UseVisualStyleBackColor = true;
            this.btnAttributes.Click += new System.EventHandler(this.btnAttributes_Click);
            // 
            // txtDescription
            // 
            this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescription.Location = new System.Drawing.Point(9, 64);
            this.txtDescription.MaxLength = 1024;
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtDescription.Size = new System.Drawing.Size(507, 213);
            this.txtDescription.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 13);
            this.label2.TabIndex = 44;
            this.label2.Text = "Description:";
            // 
            // txtName
            // 
            this.txtName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtName.Location = new System.Drawing.Point(9, 25);
            this.txtName.MaxLength = 255;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(486, 20);
            this.txtName.TabIndex = 0;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 43;
            this.label1.Text = "Name:";
            // 
            // TabRoles
            // 
            this.TabRoles.Controls.Add(this.btnAddRole);
            this.TabRoles.Controls.Add(this.btnRemoveRole);
            this.TabRoles.Controls.Add(this.lsvRoles);
            this.TabRoles.Location = new System.Drawing.Point(4, 22);
            this.TabRoles.Name = "TabRoles";
            this.TabRoles.Size = new System.Drawing.Size(528, 312);
            this.TabRoles.TabIndex = 47;
            this.TabRoles.Text = "Roles";
            this.TabRoles.UseVisualStyleBackColor = true;
            // 
            // btnAddRole
            // 
            this.btnAddRole.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddRole.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAddRole.Location = new System.Drawing.Point(360, 275);
            this.btnAddRole.Name = "btnAddRole";
            this.btnAddRole.Size = new System.Drawing.Size(75, 23);
            this.btnAddRole.TabIndex = 9;
            this.btnAddRole.Text = "A&dd";
            this.btnAddRole.UseVisualStyleBackColor = true;
            this.btnAddRole.Click += new System.EventHandler(this.btnAddRole_Click);
            // 
            // btnRemoveRole
            // 
            this.btnRemoveRole.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveRole.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnRemoveRole.Enabled = false;
            this.btnRemoveRole.Location = new System.Drawing.Point(441, 275);
            this.btnRemoveRole.Name = "btnRemoveRole";
            this.btnRemoveRole.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveRole.TabIndex = 10;
            this.btnRemoveRole.Text = "&Remove";
            this.btnRemoveRole.UseVisualStyleBackColor = true;
            this.btnRemoveRole.Click += new System.EventHandler(this.btnRemoveRole_Click);
            // 
            // lsvRoles
            // 
            this.lsvRoles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lsvRoles.CheckBoxes = true;
            this.lsvRoles.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lsvRoles.FullRowSelect = true;
            this.lsvRoles.HideSelection = false;
            this.lsvRoles.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lsvRoles.Location = new System.Drawing.Point(6, 6);
            this.lsvRoles.MultiSelect = false;
            this.lsvRoles.Name = "lsvRoles";
            this.lsvRoles.ShowGroups = false;
            this.lsvRoles.Size = new System.Drawing.Size(510, 263);
            this.lsvRoles.SmallImageList = this.imageList1;
            this.lsvRoles.TabIndex = 8;
            this.lsvRoles.UseCompatibleStateImageBehavior = false;
            this.lsvRoles.View = System.Windows.Forms.View.Details;
            this.lsvRoles.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lsvRoles_ItemCheck);
            this.lsvRoles.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lsvRoles_ColumnClick);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Name";
            this.columnHeader1.Width = 200;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Description";
            this.columnHeader2.Width = 300;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "Role_16x16.ico");
            // 
            // TabTasks
            // 
            this.TabTasks.Controls.Add(this.btnAddTask);
            this.TabTasks.Controls.Add(this.btnRemoveTask);
            this.TabTasks.Controls.Add(this.lsvTasks);
            this.TabTasks.Location = new System.Drawing.Point(4, 22);
            this.TabTasks.Name = "TabTasks";
            this.TabTasks.Size = new System.Drawing.Size(528, 312);
            this.TabTasks.TabIndex = 46;
            this.TabTasks.Text = "Tasks";
            this.TabTasks.UseVisualStyleBackColor = true;
            // 
            // btnAddTask
            // 
            this.btnAddTask.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddTask.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAddTask.Location = new System.Drawing.Point(360, 275);
            this.btnAddTask.Name = "btnAddTask";
            this.btnAddTask.Size = new System.Drawing.Size(75, 23);
            this.btnAddTask.TabIndex = 9;
            this.btnAddTask.Text = "A&dd";
            this.btnAddTask.UseVisualStyleBackColor = true;
            this.btnAddTask.Click += new System.EventHandler(this.btnAddTask_Click);
            // 
            // btnRemoveTask
            // 
            this.btnRemoveTask.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveTask.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnRemoveTask.Enabled = false;
            this.btnRemoveTask.Location = new System.Drawing.Point(441, 275);
            this.btnRemoveTask.Name = "btnRemoveTask";
            this.btnRemoveTask.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveTask.TabIndex = 10;
            this.btnRemoveTask.Text = "&Remove";
            this.btnRemoveTask.UseVisualStyleBackColor = true;
            this.btnRemoveTask.Click += new System.EventHandler(this.btnRemoveTask_Click);
            // 
            // lsvTasks
            // 
            this.lsvTasks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lsvTasks.CheckBoxes = true;
            this.lsvTasks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader3,
            this.columnHeader4});
            this.lsvTasks.FullRowSelect = true;
            this.lsvTasks.HideSelection = false;
            this.lsvTasks.Location = new System.Drawing.Point(6, 6);
            this.lsvTasks.MultiSelect = false;
            this.lsvTasks.Name = "lsvTasks";
            this.lsvTasks.ShowGroups = false;
            this.lsvTasks.Size = new System.Drawing.Size(510, 263);
            this.lsvTasks.SmallImageList = this.imageList2;
            this.lsvTasks.TabIndex = 8;
            this.lsvTasks.UseCompatibleStateImageBehavior = false;
            this.lsvTasks.View = System.Windows.Forms.View.Details;
            this.lsvTasks.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lsvTasks_ItemCheck);
            this.lsvTasks.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lsvTasks_ColumnClick);
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Name";
            this.columnHeader3.Width = 200;
            // 
            // columnHeader4
            // 
            this.columnHeader4.Text = "Description";
            this.columnHeader4.Width = 300;
            // 
            // imageList2
            // 
            this.imageList2.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList2.ImageStream")));
            this.imageList2.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList2.Images.SetKeyName(0, "Task_16x16.ico");
            // 
            // TabOperations
            // 
            this.TabOperations.Controls.Add(this.btnAddOperation);
            this.TabOperations.Controls.Add(this.btnRemoveOperation);
            this.TabOperations.Controls.Add(this.lsvOperations);
            this.TabOperations.Location = new System.Drawing.Point(4, 22);
            this.TabOperations.Name = "TabOperations";
            this.TabOperations.Padding = new System.Windows.Forms.Padding(3);
            this.TabOperations.Size = new System.Drawing.Size(528, 312);
            this.TabOperations.TabIndex = 1;
            this.TabOperations.Text = "Operations";
            this.TabOperations.UseVisualStyleBackColor = true;
            // 
            // btnAddOperation
            // 
            this.btnAddOperation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddOperation.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAddOperation.Location = new System.Drawing.Point(360, 275);
            this.btnAddOperation.Name = "btnAddOperation";
            this.btnAddOperation.Size = new System.Drawing.Size(75, 23);
            this.btnAddOperation.TabIndex = 6;
            this.btnAddOperation.Text = "A&dd";
            this.btnAddOperation.UseVisualStyleBackColor = true;
            this.btnAddOperation.Click += new System.EventHandler(this.btnAddOperation_Click);
            // 
            // btnRemoveOperation
            // 
            this.btnRemoveOperation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveOperation.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnRemoveOperation.Enabled = false;
            this.btnRemoveOperation.Location = new System.Drawing.Point(441, 275);
            this.btnRemoveOperation.Name = "btnRemoveOperation";
            this.btnRemoveOperation.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveOperation.TabIndex = 7;
            this.btnRemoveOperation.Text = "&Remove";
            this.btnRemoveOperation.UseVisualStyleBackColor = true;
            this.btnRemoveOperation.Click += new System.EventHandler(this.btnRemoveOperation_Click);
            // 
            // lsvOperations
            // 
            this.lsvOperations.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lsvOperations.CheckBoxes = true;
            this.lsvOperations.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.nameColumnHeader,
            this.descriptionColumnHeader});
            this.lsvOperations.FullRowSelect = true;
            this.lsvOperations.HideSelection = false;
            this.lsvOperations.Location = new System.Drawing.Point(6, 6);
            this.lsvOperations.MultiSelect = false;
            this.lsvOperations.Name = "lsvOperations";
            this.lsvOperations.ShowGroups = false;
            this.lsvOperations.Size = new System.Drawing.Size(510, 263);
            this.lsvOperations.SmallImageList = this.imageList3;
            this.lsvOperations.TabIndex = 5;
            this.lsvOperations.UseCompatibleStateImageBehavior = false;
            this.lsvOperations.View = System.Windows.Forms.View.Details;
            this.lsvOperations.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.lsvOperations_Check);
            this.lsvOperations.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.lsvOperations_ColumnClick);
            // 
            // nameColumnHeader
            // 
            this.nameColumnHeader.Text = "Name";
            this.nameColumnHeader.Width = 200;
            // 
            // descriptionColumnHeader
            // 
            this.descriptionColumnHeader.Text = "Description";
            this.descriptionColumnHeader.Width = 300;
            // 
            // imageList3
            // 
            this.imageList3.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList3.ImageStream")));
            this.imageList3.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList3.Images.SetKeyName(0, "Operation_16x16.ico");
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnApply.Enabled = false;
            this.btnApply.Location = new System.Drawing.Point(511, 377);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 4;
            this.btnApply.Text = "&Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // picRole
            // 
            this.picRole.Image = global::NetSqlAzMan.SnapIn.Properties.Resources.Role_32x32;
            this.picRole.Location = new System.Drawing.Point(12, 48);
            this.picRole.Name = "picRole";
            this.picRole.Size = new System.Drawing.Size(32, 32);
            this.picRole.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picRole.TabIndex = 46;
            this.picRole.TabStop = false;
            this.picRole.Visible = false;
            // 
            // picTask
            // 
            this.picTask.Image = global::NetSqlAzMan.SnapIn.Properties.Resources.Task_32x32;
            this.picTask.Location = new System.Drawing.Point(12, 86);
            this.picTask.Name = "picTask";
            this.picTask.Size = new System.Drawing.Size(32, 32);
            this.picTask.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picTask.TabIndex = 47;
            this.picTask.TabStop = false;
            this.picTask.Visible = false;
            // 
            // picOperation
            // 
            this.picOperation.Image = global::NetSqlAzMan.SnapIn.Properties.Resources.Operation_32x32;
            this.picOperation.Location = new System.Drawing.Point(12, 124);
            this.picOperation.Name = "picOperation";
            this.picOperation.Size = new System.Drawing.Size(32, 32);
            this.picOperation.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.picOperation.TabIndex = 48;
            this.picOperation.TabStop = false;
            this.picOperation.Visible = false;
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
            // frmItemProperties
            // 
            this.AcceptButton = this.btnOk;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(598, 408);
            this.Controls.Add(this.picOperation);
            this.Controls.Add(this.picTask);
            this.Controls.Add(this.picRole);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.picImage);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.lblInfo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.MinimumSize = new System.Drawing.Size(185, 255);
            this.Name = "frmItemProperties";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = " New Item";
            this.Activated += new System.EventHandler(this.frmItemProperties_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmItemProperties_FormClosing);
            this.Load += new System.EventHandler(this.frmItemProperties_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabItemDefinition.ResumeLayout(false);
            this.tabItemDefinition.PerformLayout();
            this.TabRoles.ResumeLayout(false);
            this.TabTasks.ResumeLayout(false);
            this.TabOperations.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picRole)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picTask)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picOperation)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.PictureBox picImage;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabItemDefinition;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage TabOperations;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ListView lsvOperations;
        private System.Windows.Forms.ColumnHeader nameColumnHeader;
        private System.Windows.Forms.ColumnHeader descriptionColumnHeader;
        private System.Windows.Forms.Button btnAddOperation;
        private System.Windows.Forms.Button btnRemoveOperation;
        private System.Windows.Forms.PictureBox picOperation;
        private System.Windows.Forms.PictureBox picTask;
        private System.Windows.Forms.PictureBox picRole;
        private System.Windows.Forms.TabPage TabTasks;
        private System.Windows.Forms.TabPage TabRoles;
        private System.Windows.Forms.Button btnAddRole;
        private System.Windows.Forms.Button btnRemoveRole;
        private System.Windows.Forms.ListView lsvRoles;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.Button btnAddTask;
        private System.Windows.Forms.Button btnRemoveTask;
        private System.Windows.Forms.ListView lsvTasks;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ColumnHeader columnHeader4;
        private System.Windows.Forms.ImageList imageList2;
        private System.Windows.Forms.ImageList imageList3;
        private System.Windows.Forms.Button btnAttributes;
        private System.Windows.Forms.Button btnBizRule;
        private System.Windows.Forms.ImageList imageListItemHierarchyView;

    }
}