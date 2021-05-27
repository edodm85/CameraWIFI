namespace PhoneTCPClientExample
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTCPStatus = new System.Windows.Forms.Label();
            this.textBoxPort = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxIpAddress = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonTCPDisconnect = new System.Windows.Forms.Button();
            this.buttonTCPConnect = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonAcqNewImage = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(754, 455);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.labelTCPStatus);
            this.panel1.Controls.Add(this.textBoxPort);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.textBoxIpAddress);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.buttonTCPDisconnect);
            this.panel1.Controls.Add(this.buttonTCPConnect);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(748, 59);
            this.panel1.TabIndex = 0;
            // 
            // labelTCPStatus
            // 
            this.labelTCPStatus.AutoSize = true;
            this.labelTCPStatus.Location = new System.Drawing.Point(561, 23);
            this.labelTCPStatus.Name = "labelTCPStatus";
            this.labelTCPStatus.Size = new System.Drawing.Size(40, 13);
            this.labelTCPStatus.TabIndex = 6;
            this.labelTCPStatus.Text = "Status:";
            // 
            // textBoxPort
            // 
            this.textBoxPort.Location = new System.Drawing.Point(311, 36);
            this.textBoxPort.Name = "textBoxPort";
            this.textBoxPort.Size = new System.Drawing.Size(244, 20);
            this.textBoxPort.TabIndex = 5;
            this.textBoxPort.Text = "2000";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(270, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "PORT:";
            // 
            // textBoxIpAddress
            // 
            this.textBoxIpAddress.Location = new System.Drawing.Point(311, 10);
            this.textBoxIpAddress.Name = "textBoxIpAddress";
            this.textBoxIpAddress.Size = new System.Drawing.Size(244, 20);
            this.textBoxIpAddress.TabIndex = 3;
            this.textBoxIpAddress.Text = "192.168.153.100";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(270, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(20, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "IP:";
            // 
            // buttonTCPDisconnect
            // 
            this.buttonTCPDisconnect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonTCPDisconnect.Location = new System.Drawing.Point(121, 3);
            this.buttonTCPDisconnect.Name = "buttonTCPDisconnect";
            this.buttonTCPDisconnect.Size = new System.Drawing.Size(112, 53);
            this.buttonTCPDisconnect.TabIndex = 1;
            this.buttonTCPDisconnect.Text = "DISCONNECT";
            this.buttonTCPDisconnect.UseVisualStyleBackColor = true;
            this.buttonTCPDisconnect.Click += new System.EventHandler(this.buttonTCPDisconnect_Click);
            // 
            // buttonTCPConnect
            // 
            this.buttonTCPConnect.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonTCPConnect.Location = new System.Drawing.Point(3, 3);
            this.buttonTCPConnect.Name = "buttonTCPConnect";
            this.buttonTCPConnect.Size = new System.Drawing.Size(112, 53);
            this.buttonTCPConnect.TabIndex = 0;
            this.buttonTCPConnect.Text = "CONNECT";
            this.buttonTCPConnect.UseVisualStyleBackColor = true;
            this.buttonTCPConnect.Click += new System.EventHandler(this.buttonTCPConnect_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.buttonAcqNewImage);
            this.panel2.Controls.Add(this.pictureBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 68);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(748, 384);
            this.panel2.TabIndex = 1;
            // 
            // buttonAcqNewImage
            // 
            this.buttonAcqNewImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonAcqNewImage.Location = new System.Drawing.Point(561, 3);
            this.buttonAcqNewImage.Name = "buttonAcqNewImage";
            this.buttonAcqNewImage.Size = new System.Drawing.Size(140, 62);
            this.buttonAcqNewImage.TabIndex = 1;
            this.buttonAcqNewImage.Text = "SNAP";
            this.buttonAcqNewImage.UseVisualStyleBackColor = true;
            this.buttonAcqNewImage.Click += new System.EventHandler(this.buttonAcqNewImage_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(552, 378);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(754, 455);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Form1";
            this.Text = "PhoneTCPClient Example v1.3";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button buttonTCPConnect;
        private System.Windows.Forms.Button buttonTCPDisconnect;
        private System.Windows.Forms.TextBox textBoxPort;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxIpAddress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelTCPStatus;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonAcqNewImage;
    }
}

