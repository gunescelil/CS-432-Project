namespace CS432TermProject
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
            this.tbUserName = new System.Windows.Forms.TextBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.tbAuthenticationServerIP = new System.Windows.Forms.TextBox();
            this.tbAuthenticationServerPort = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnConnect = new System.Windows.Forms.Button();
            this.rtbMonitor = new System.Windows.Forms.RichTextBox();
            this.monitorLabel = new System.Windows.Forms.Label();
            this.tbAServerPubKeyFile = new System.Windows.Forms.TextBox();
            this.btnBrowseAServerPublicKey = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnBrowseUserKeyPairFile = new System.Windows.Forms.Button();
            this.tbUserKeyPairFile = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // tbUserName
            // 
            this.tbUserName.Location = new System.Drawing.Point(22, 29);
            this.tbUserName.Name = "tbUserName";
            this.tbUserName.Size = new System.Drawing.Size(136, 22);
            this.tbUserName.TabIndex = 0;
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(22, 89);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(136, 22);
            this.tbPassword.TabIndex = 1;
            // 
            // tbAuthenticationServerIP
            // 
            this.tbAuthenticationServerIP.Location = new System.Drawing.Point(204, 29);
            this.tbAuthenticationServerIP.Name = "tbAuthenticationServerIP";
            this.tbAuthenticationServerIP.Size = new System.Drawing.Size(225, 22);
            this.tbAuthenticationServerIP.TabIndex = 2;
            // 
            // tbAuthenticationServerPort
            // 
            this.tbAuthenticationServerPort.Location = new System.Drawing.Point(204, 89);
            this.tbAuthenticationServerPort.Name = "tbAuthenticationServerPort";
            this.tbAuthenticationServerPort.Size = new System.Drawing.Size(225, 22);
            this.tbAuthenticationServerPort.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Username";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Password";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(201, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 17);
            this.label3.TabIndex = 6;
            this.label3.Text = "Server IP";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(201, 69);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 17);
            this.label4.TabIndex = 7;
            this.label4.Text = "Port Number";
            // 
            // btnConnect
            // 
            this.btnConnect.Location = new System.Drawing.Point(22, 138);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(136, 32);
            this.btnConnect.TabIndex = 8;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Click += new System.EventHandler(this.Connect_Click);
            // 
            // rtbMonitor
            // 
            this.rtbMonitor.Location = new System.Drawing.Point(475, 29);
            this.rtbMonitor.Name = "rtbMonitor";
            this.rtbMonitor.Size = new System.Drawing.Size(374, 107);
            this.rtbMonitor.TabIndex = 9;
            this.rtbMonitor.Text = "";
            // 
            // monitorLabel
            // 
            this.monitorLabel.AutoSize = true;
            this.monitorLabel.Location = new System.Drawing.Point(472, 9);
            this.monitorLabel.Name = "monitorLabel";
            this.monitorLabel.Size = new System.Drawing.Size(55, 17);
            this.monitorLabel.TabIndex = 10;
            this.monitorLabel.Text = "Monitor";
            // 
            // tbAServerPubKeyFile
            // 
            this.tbAServerPubKeyFile.Location = new System.Drawing.Point(22, 331);
            this.tbAServerPubKeyFile.Name = "tbAServerPubKeyFile";
            this.tbAServerPubKeyFile.Size = new System.Drawing.Size(314, 22);
            this.tbAServerPubKeyFile.TabIndex = 11;
            // 
            // btnBrowseAServerPublicKey
            // 
            this.btnBrowseAServerPublicKey.Location = new System.Drawing.Point(371, 331);
            this.btnBrowseAServerPublicKey.Name = "btnBrowseAServerPublicKey";
            this.btnBrowseAServerPublicKey.Size = new System.Drawing.Size(136, 32);
            this.btnBrowseAServerPublicKey.TabIndex = 12;
            this.btnBrowseAServerPublicKey.Text = "Browse";
            this.btnBrowseAServerPublicKey.UseVisualStyleBackColor = true;
            this.btnBrowseAServerPublicKey.Click += new System.EventHandler(this.btnBrowseAServerPublicKey_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 302);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(240, 17);
            this.label5.TabIndex = 13;
            this.label5.Text = "Authentication Server Public Key File";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 223);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(121, 17);
            this.label6.TabIndex = 16;
            this.label6.Text = "User Key Pair File";
            // 
            // btnBrowseUserKeyPairFile
            // 
            this.btnBrowseUserKeyPairFile.Location = new System.Drawing.Point(371, 252);
            this.btnBrowseUserKeyPairFile.Name = "btnBrowseUserKeyPairFile";
            this.btnBrowseUserKeyPairFile.Size = new System.Drawing.Size(136, 32);
            this.btnBrowseUserKeyPairFile.TabIndex = 15;
            this.btnBrowseUserKeyPairFile.Text = "Browse";
            this.btnBrowseUserKeyPairFile.UseVisualStyleBackColor = true;
            this.btnBrowseUserKeyPairFile.Click += new System.EventHandler(this.btnBrowseUserKeyPairFile_Click);
            // 
            // tbUserKeyPairFile
            // 
            this.tbUserKeyPairFile.Location = new System.Drawing.Point(22, 252);
            this.tbUserKeyPairFile.Name = "tbUserKeyPairFile";
            this.tbUserKeyPairFile.Size = new System.Drawing.Size(314, 22);
            this.tbUserKeyPairFile.TabIndex = 14;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(922, 389);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.btnBrowseUserKeyPairFile);
            this.Controls.Add(this.tbUserKeyPairFile);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.btnBrowseAServerPublicKey);
            this.Controls.Add(this.tbAServerPubKeyFile);
            this.Controls.Add(this.monitorLabel);
            this.Controls.Add(this.rtbMonitor);
            this.Controls.Add(this.btnConnect);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbAuthenticationServerPort);
            this.Controls.Add(this.tbAuthenticationServerIP);
            this.Controls.Add(this.tbPassword);
            this.Controls.Add(this.tbUserName);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbUserName;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.TextBox tbAuthenticationServerIP;
        private System.Windows.Forms.TextBox tbAuthenticationServerPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.RichTextBox rtbMonitor;
        private System.Windows.Forms.Label monitorLabel;
        private System.Windows.Forms.TextBox tbAServerPubKeyFile;
        private System.Windows.Forms.Button btnBrowseAServerPublicKey;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnBrowseUserKeyPairFile;
        private System.Windows.Forms.TextBox tbUserKeyPairFile;
    }
}

