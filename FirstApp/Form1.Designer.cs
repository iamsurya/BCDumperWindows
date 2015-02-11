namespace FirstApp
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
            this.components = new System.ComponentModel.Container();
            System.IO.Ports.SerialPort serialPort1;
            this.lblStat = new System.Windows.Forms.Label();
            this.lblTXPercent = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.lblFileName = new System.Windows.Forms.Label();
            this.BtnDump = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblRX = new System.Windows.Forms.Label();
            this.lblToRX = new System.Windows.Forms.Label();
            this.lblETA = new System.Windows.Forms.Label();
            this.CmbPorts = new System.Windows.Forms.ComboBox();
            this.BtnListPorts = new System.Windows.Forms.Button();
            this.btnTimeSync = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.SuspendLayout();
            // 
            // lblStat
            // 
            this.lblStat.AutoSize = true;
            this.lblStat.ForeColor = System.Drawing.Color.Red;
            this.lblStat.Location = new System.Drawing.Point(190, 173);
            this.lblStat.Name = "lblStat";
            this.lblStat.Size = new System.Drawing.Size(43, 13);
            this.lblStat.TabIndex = 0;
            this.lblStat.Text = "Waiting";
            // 
            // lblTXPercent
            // 
            this.lblTXPercent.AutoSize = true;
            this.lblTXPercent.Location = new System.Drawing.Point(190, 201);
            this.lblTXPercent.Name = "lblTXPercent";
            this.lblTXPercent.Size = new System.Drawing.Size(21, 13);
            this.lblTXPercent.TabIndex = 1;
            this.lblTXPercent.Text = "0%";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Location = new System.Drawing.Point(190, 229);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(51, 13);
            this.lblFileName.TabIndex = 2;
            this.lblFileName.Text = "FileName";
            this.lblFileName.Click += new System.EventHandler(this.lblFileName_Click);
            // 
            // BtnDump
            // 
            this.BtnDump.Location = new System.Drawing.Point(49, 117);
            this.BtnDump.Name = "BtnDump";
            this.BtnDump.Size = new System.Drawing.Size(75, 23);
            this.BtnDump.TabIndex = 3;
            this.BtnDump.Text = "Dump Data";
            this.BtnDump.UseVisualStyleBackColor = true;
            this.BtnDump.Click += new System.EventHandler(this.BtnDump_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(211, 116);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(134, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Close File and Serial Port";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnStopDump);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(49, 77);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(322, 23);
            this.progressBar1.TabIndex = 5;
            // 
            // lblRX
            // 
            this.lblRX.AutoSize = true;
            this.lblRX.Location = new System.Drawing.Point(311, 173);
            this.lblRX.Name = "lblRX";
            this.lblRX.Size = new System.Drawing.Size(51, 13);
            this.lblRX.TabIndex = 6;
            this.lblRX.Text = "RX Bytes";
            // 
            // lblToRX
            // 
            this.lblToRX.AutoSize = true;
            this.lblToRX.Location = new System.Drawing.Point(311, 201);
            this.lblToRX.Name = "lblToRX";
            this.lblToRX.Size = new System.Drawing.Size(60, 13);
            this.lblToRX.TabIndex = 7;
            this.lblToRX.Text = "Total Bytes";
            // 
            // lblETA
            // 
            this.lblETA.AutoSize = true;
            this.lblETA.Location = new System.Drawing.Point(311, 229);
            this.lblETA.Name = "lblETA";
            this.lblETA.Size = new System.Drawing.Size(83, 13);
            this.lblETA.TabIndex = 8;
            this.lblETA.Text = "Time Remaining";
            // 
            // CmbPorts
            // 
            this.CmbPorts.FormattingEnabled = true;
            this.CmbPorts.Location = new System.Drawing.Point(83, 25);
            this.CmbPorts.Name = "CmbPorts";
            this.CmbPorts.Size = new System.Drawing.Size(54, 21);
            this.CmbPorts.TabIndex = 9;
            // 
            // BtnListPorts
            // 
            this.BtnListPorts.Location = new System.Drawing.Point(270, 25);
            this.BtnListPorts.Name = "BtnListPorts";
            this.BtnListPorts.Size = new System.Drawing.Size(75, 23);
            this.BtnListPorts.TabIndex = 10;
            this.BtnListPorts.Text = "Detect Ports";
            this.BtnListPorts.UseVisualStyleBackColor = true;
            this.BtnListPorts.Click += new System.EventHandler(this.BtnListPorts_Click);
            // 
            // btnTimeSync
            // 
            this.btnTimeSync.Location = new System.Drawing.Point(49, 260);
            this.btnTimeSync.Name = "btnTimeSync";
            this.btnTimeSync.Size = new System.Drawing.Size(75, 23);
            this.btnTimeSync.TabIndex = 11;
            this.btnTimeSync.Text = "Sync Time";
            this.btnTimeSync.UseVisualStyleBackColor = true;
            this.btnTimeSync.Click += new System.EventHandler(this.btnTimeSync_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 173);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Status";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 201);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 13);
            this.label2.TabIndex = 13;
            this.label2.Text = "Transfer Percentage";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(434, 295);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnTimeSync);
            this.Controls.Add(this.BtnListPorts);
            this.Controls.Add(this.CmbPorts);
            this.Controls.Add(this.lblETA);
            this.Controls.Add(this.lblToRX);
            this.Controls.Add(this.lblRX);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.BtnDump);
            this.Controls.Add(this.lblFileName);
            this.Controls.Add(this.lblTXPercent);
            this.Controls.Add(this.lblStat);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblStat;
        private System.Windows.Forms.Label lblTXPercent;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.Button BtnDump;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblRX;
        private System.Windows.Forms.Label lblToRX;
        private System.Windows.Forms.Label lblETA;
        private System.Windows.Forms.ComboBox CmbPorts;
        private System.Windows.Forms.Button BtnListPorts;
        private System.Windows.Forms.Button btnTimeSync;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}

