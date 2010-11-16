namespace NetSqlAzMan_vs_AzMan_StressTest
{
    partial class frmTest
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
            this.lblCarico = new System.Windows.Forms.Label();
            this.txtUnita = new System.Windows.Forms.TextBox();
            this.txtAzManStorePath = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNetSqlAzManConnectionString = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCreaAzMan = new System.Windows.Forms.Button();
            this.btnCreaNetSqlAzMan = new System.Windows.Forms.Button();
            this.btnTestAzMan = new System.Windows.Forms.Button();
            this.btnTestNetSqlAzMan = new System.Windows.Forms.Button();
            this.pb = new System.Windows.Forms.ProgressBar();
            this.chkAzManMultiThread = new System.Windows.Forms.CheckBox();
            this.chkNetSqlAzManMultiThread = new System.Windows.Forms.CheckBox();
            this.txtAzManThreads = new System.Windows.Forms.TextBox();
            this.txtNetSqlAzManThreads = new System.Windows.Forms.TextBox();
            this.txtStart = new System.Windows.Forms.TextBox();
            this.txtStop = new System.Windows.Forms.TextBox();
            this.txtAzManElapsed = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtNetSqlAzManElapsed = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.chkCache = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblCarico
            // 
            this.lblCarico.Location = new System.Drawing.Point(12, 70);
            this.lblCarico.Name = "lblCarico";
            this.lblCarico.Size = new System.Drawing.Size(166, 23);
            this.lblCarico.TabIndex = 0;
            this.lblCarico.Text = "Unità di Carico: ";
            // 
            // txtUnita
            // 
            this.txtUnita.Location = new System.Drawing.Point(184, 67);
            this.txtUnita.Name = "txtUnita";
            this.txtUnita.Size = new System.Drawing.Size(60, 20);
            this.txtUnita.TabIndex = 1;
            this.txtUnita.Text = "20";
            this.txtUnita.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtAzManStorePath
            // 
            this.txtAzManStorePath.Location = new System.Drawing.Point(184, 6);
            this.txtAzManStorePath.Name = "txtAzManStorePath";
            this.txtAzManStorePath.Size = new System.Drawing.Size(466, 20);
            this.txtAzManStorePath.TabIndex = 3;
            this.txtAzManStorePath.Text = "mssql://Driver={SQL Server};Server={EIDOS-NBAFR};/AzManDB/AzManStore";
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(166, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "AzMan Path:";
            // 
            // txtNetSqlAzManConnectionString
            // 
            this.txtNetSqlAzManConnectionString.Location = new System.Drawing.Point(184, 32);
            this.txtNetSqlAzManConnectionString.Name = "txtNetSqlAzManConnectionString";
            this.txtNetSqlAzManConnectionString.Size = new System.Drawing.Size(466, 20);
            this.txtNetSqlAzManConnectionString.TabIndex = 5;
            this.txtNetSqlAzManConnectionString.Text = "data source=EIDOS-NBAFR;Initial Catalog=NetSqlAzManStorage;Integrated Security=T" +
                "rue;";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(166, 23);
            this.label2.TabIndex = 4;
            this.label2.Text = "NetSqlAzMan connections string:";
            // 
            // btnCreaAzMan
            // 
            this.btnCreaAzMan.Location = new System.Drawing.Point(15, 96);
            this.btnCreaAzMan.Name = "btnCreaAzMan";
            this.btnCreaAzMan.Size = new System.Drawing.Size(186, 23);
            this.btnCreaAzMan.TabIndex = 6;
            this.btnCreaAzMan.Text = "Crea struttura su AzMan";
            this.btnCreaAzMan.UseVisualStyleBackColor = true;
            this.btnCreaAzMan.Click += new System.EventHandler(this.btnCreaAzMan_Click);
            // 
            // btnCreaNetSqlAzMan
            // 
            this.btnCreaNetSqlAzMan.Location = new System.Drawing.Point(15, 125);
            this.btnCreaNetSqlAzMan.Name = "btnCreaNetSqlAzMan";
            this.btnCreaNetSqlAzMan.Size = new System.Drawing.Size(186, 23);
            this.btnCreaNetSqlAzMan.TabIndex = 7;
            this.btnCreaNetSqlAzMan.Text = "Crea struttura su NetSqlAzMan";
            this.btnCreaNetSqlAzMan.UseVisualStyleBackColor = true;
            this.btnCreaNetSqlAzMan.Click += new System.EventHandler(this.btnCreaNetSqlAzMan_Click);
            // 
            // btnTestAzMan
            // 
            this.btnTestAzMan.Location = new System.Drawing.Point(98, 177);
            this.btnTestAzMan.Name = "btnTestAzMan";
            this.btnTestAzMan.Size = new System.Drawing.Size(186, 23);
            this.btnTestAzMan.TabIndex = 8;
            this.btnTestAzMan.Text = "Test AzMan";
            this.btnTestAzMan.UseVisualStyleBackColor = true;
            this.btnTestAzMan.Click += new System.EventHandler(this.btnTestAzMan_Click);
            // 
            // btnTestNetSqlAzMan
            // 
            this.btnTestNetSqlAzMan.Location = new System.Drawing.Point(98, 206);
            this.btnTestNetSqlAzMan.Name = "btnTestNetSqlAzMan";
            this.btnTestNetSqlAzMan.Size = new System.Drawing.Size(186, 23);
            this.btnTestNetSqlAzMan.TabIndex = 9;
            this.btnTestNetSqlAzMan.Text = "Test NetSqlAzMan";
            this.btnTestNetSqlAzMan.UseVisualStyleBackColor = true;
            this.btnTestNetSqlAzMan.Click += new System.EventHandler(this.btnTestNetSqlAzMan_Click);
            // 
            // pb
            // 
            this.pb.Location = new System.Drawing.Point(12, 277);
            this.pb.Name = "pb";
            this.pb.Size = new System.Drawing.Size(638, 23);
            this.pb.TabIndex = 10;
            // 
            // chkAzManMultiThread
            // 
            this.chkAzManMultiThread.AutoSize = true;
            this.chkAzManMultiThread.Location = new System.Drawing.Point(303, 177);
            this.chkAzManMultiThread.Name = "chkAzManMultiThread";
            this.chkAzManMultiThread.Size = new System.Drawing.Size(85, 17);
            this.chkAzManMultiThread.TabIndex = 11;
            this.chkAzManMultiThread.Text = "Multi Thread";
            this.chkAzManMultiThread.UseVisualStyleBackColor = true;
            // 
            // chkNetSqlAzManMultiThread
            // 
            this.chkNetSqlAzManMultiThread.AutoSize = true;
            this.chkNetSqlAzManMultiThread.Location = new System.Drawing.Point(303, 206);
            this.chkNetSqlAzManMultiThread.Name = "chkNetSqlAzManMultiThread";
            this.chkNetSqlAzManMultiThread.Size = new System.Drawing.Size(85, 17);
            this.chkNetSqlAzManMultiThread.TabIndex = 12;
            this.chkNetSqlAzManMultiThread.Text = "Multi Thread";
            this.chkNetSqlAzManMultiThread.UseVisualStyleBackColor = true;
            // 
            // txtAzManThreads
            // 
            this.txtAzManThreads.Location = new System.Drawing.Point(406, 174);
            this.txtAzManThreads.Name = "txtAzManThreads";
            this.txtAzManThreads.Size = new System.Drawing.Size(59, 20);
            this.txtAzManThreads.TabIndex = 13;
            this.txtAzManThreads.Text = "100";
            this.txtAzManThreads.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtNetSqlAzManThreads
            // 
            this.txtNetSqlAzManThreads.Location = new System.Drawing.Point(406, 203);
            this.txtNetSqlAzManThreads.Name = "txtNetSqlAzManThreads";
            this.txtNetSqlAzManThreads.Size = new System.Drawing.Size(59, 20);
            this.txtNetSqlAzManThreads.TabIndex = 14;
            this.txtNetSqlAzManThreads.Text = "100";
            this.txtNetSqlAzManThreads.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtStart
            // 
            this.txtStart.Location = new System.Drawing.Point(15, 251);
            this.txtStart.Name = "txtStart";
            this.txtStart.Size = new System.Drawing.Size(124, 20);
            this.txtStart.TabIndex = 15;
            this.txtStart.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtStop
            // 
            this.txtStop.Location = new System.Drawing.Point(526, 251);
            this.txtStop.Name = "txtStop";
            this.txtStop.Size = new System.Drawing.Size(124, 20);
            this.txtStop.TabIndex = 16;
            this.txtStop.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtAzManElapsed
            // 
            this.txtAzManElapsed.Location = new System.Drawing.Point(526, 174);
            this.txtAzManElapsed.Name = "txtAzManElapsed";
            this.txtAzManElapsed.Size = new System.Drawing.Size(124, 20);
            this.txtAzManElapsed.TabIndex = 17;
            this.txtAzManElapsed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 232);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 16);
            this.label3.TabIndex = 18;
            this.label3.Text = "Start Time:";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(523, 155);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 16);
            this.label4.TabIndex = 19;
            this.label4.Text = "Elapsed Time:";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(523, 232);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(127, 16);
            this.label5.TabIndex = 20;
            this.label5.Text = "End Time:";
            // 
            // txtNetSqlAzManElapsed
            // 
            this.txtNetSqlAzManElapsed.Location = new System.Drawing.Point(526, 200);
            this.txtNetSqlAzManElapsed.Name = "txtNetSqlAzManElapsed";
            this.txtNetSqlAzManElapsed.Size = new System.Drawing.Size(124, 20);
            this.txtNetSqlAzManElapsed.TabIndex = 21;
            this.txtNetSqlAzManElapsed.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(253, 103);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 22;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // chkCache
            // 
            this.chkCache.AutoSize = true;
            this.chkCache.Location = new System.Drawing.Point(303, 229);
            this.chkCache.Name = "chkCache";
            this.chkCache.Size = new System.Drawing.Size(79, 17);
            this.chkCache.TabIndex = 23;
            this.chkCache.Text = "Use Cache";
            this.chkCache.UseVisualStyleBackColor = true;
            // 
            // frmTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(662, 312);
            this.Controls.Add(this.chkCache);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtNetSqlAzManElapsed);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtAzManElapsed);
            this.Controls.Add(this.txtStop);
            this.Controls.Add(this.txtStart);
            this.Controls.Add(this.txtNetSqlAzManThreads);
            this.Controls.Add(this.txtAzManThreads);
            this.Controls.Add(this.chkNetSqlAzManMultiThread);
            this.Controls.Add(this.chkAzManMultiThread);
            this.Controls.Add(this.pb);
            this.Controls.Add(this.btnTestNetSqlAzMan);
            this.Controls.Add(this.btnTestAzMan);
            this.Controls.Add(this.btnCreaNetSqlAzMan);
            this.Controls.Add(this.btnCreaAzMan);
            this.Controls.Add(this.txtNetSqlAzManConnectionString);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtAzManStorePath);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtUnita);
            this.Controls.Add(this.lblCarico);
            this.Name = "frmTest";
            this.Text = "NetSqlAzMan vs AzMan - Stress Test";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblCarico;
        private System.Windows.Forms.TextBox txtUnita;
        private System.Windows.Forms.TextBox txtAzManStorePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNetSqlAzManConnectionString;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCreaAzMan;
        private System.Windows.Forms.Button btnCreaNetSqlAzMan;
        private System.Windows.Forms.Button btnTestAzMan;
        private System.Windows.Forms.Button btnTestNetSqlAzMan;
        private System.Windows.Forms.ProgressBar pb;
        private System.Windows.Forms.CheckBox chkAzManMultiThread;
        private System.Windows.Forms.CheckBox chkNetSqlAzManMultiThread;
        private System.Windows.Forms.TextBox txtAzManThreads;
        private System.Windows.Forms.TextBox txtNetSqlAzManThreads;
        private System.Windows.Forms.TextBox txtStart;
        private System.Windows.Forms.TextBox txtStop;
        private System.Windows.Forms.TextBox txtAzManElapsed;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtNetSqlAzManElapsed;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox chkCache;
    }
}

