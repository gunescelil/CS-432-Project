namespace CS432ProjectAuthenticationServer
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
            this.tbPortNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnStartServer = new System.Windows.Forms.Button();
            this.rtbMonitor = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.tbServerPrivateKeyPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lbConnectedUsers = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbMainRepository = new System.Windows.Forms.TextBox();
            this.btnChooseMainRepository = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // tbPortNumber
            // 
            this.tbPortNumber.Location = new System.Drawing.Point(26, 434);
            this.tbPortNumber.Name = "tbPortNumber";
            this.tbPortNumber.Size = new System.Drawing.Size(114, 22);
            this.tbPortNumber.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 404);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Port";
            // 
            // btnStartServer
            // 
            this.btnStartServer.Location = new System.Drawing.Point(337, 426);
            this.btnStartServer.Name = "btnStartServer";
            this.btnStartServer.Size = new System.Drawing.Size(114, 30);
            this.btnStartServer.TabIndex = 2;
            this.btnStartServer.Text = "Start Server";
            this.btnStartServer.UseVisualStyleBackColor = true;
            this.btnStartServer.Click += new System.EventHandler(this.btnStartServer_Click);
            // 
            // rtbMonitor
            // 
            this.rtbMonitor.Location = new System.Drawing.Point(476, 40);
            this.rtbMonitor.Name = "rtbMonitor";
            this.rtbMonitor.Size = new System.Drawing.Size(436, 420);
            this.rtbMonitor.TabIndex = 3;
            this.rtbMonitor.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(473, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Monitor";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(337, 290);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(114, 30);
            this.btnBrowse.TabIndex = 5;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // tbServerPrivateKeyPath
            // 
            this.tbServerPrivateKeyPath.Location = new System.Drawing.Point(26, 298);
            this.tbServerPrivateKeyPath.Name = "tbServerPrivateKeyPath";
            this.tbServerPrivateKeyPath.Size = new System.Drawing.Size(275, 22);
            this.tbServerPrivateKeyPath.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 269);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(159, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "Server Private Key Path";
            // 
            // lbConnectedUsers
            // 
            this.lbConnectedUsers.FormattingEnabled = true;
            this.lbConnectedUsers.ItemHeight = 16;
            this.lbConnectedUsers.Location = new System.Drawing.Point(26, 42);
            this.lbConnectedUsers.Name = "lbConnectedUsers";
            this.lbConnectedUsers.Size = new System.Drawing.Size(425, 212);
            this.lbConnectedUsers.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 17);
            this.label4.TabIndex = 9;
            this.label4.Text = "Connected Users";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 336);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(175, 17);
            this.label5.TabIndex = 10;
            this.label5.Text = "Main Repository Of Server";
            // 
            // tbMainRepository
            // 
            this.tbMainRepository.Location = new System.Drawing.Point(26, 370);
            this.tbMainRepository.Name = "tbMainRepository";
            this.tbMainRepository.Size = new System.Drawing.Size(275, 22);
            this.tbMainRepository.TabIndex = 11;
            // 
            // btnChooseMainRepository
            // 
            this.btnChooseMainRepository.Location = new System.Drawing.Point(337, 362);
            this.btnChooseMainRepository.Name = "btnChooseMainRepository";
            this.btnChooseMainRepository.Size = new System.Drawing.Size(114, 30);
            this.btnChooseMainRepository.TabIndex = 12;
            this.btnChooseMainRepository.Text = "Browse";
            this.btnChooseMainRepository.UseVisualStyleBackColor = true;
            this.btnChooseMainRepository.Click += new System.EventHandler(this.btnChooseMainRepository_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(934, 485);
            this.Controls.Add(this.btnChooseMainRepository);
            this.Controls.Add(this.tbMainRepository);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.lbConnectedUsers);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbServerPrivateKeyPath);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.rtbMonitor);
            this.Controls.Add(this.btnStartServer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbPortNumber);
            this.Name = "Form1";
            this.Text = "Authentication Server";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbPortNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnStartServer;
        private System.Windows.Forms.RichTextBox rtbMonitor;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox tbServerPrivateKeyPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox lbConnectedUsers;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbMainRepository;
        private System.Windows.Forms.Button btnChooseMainRepository;
    }
}

