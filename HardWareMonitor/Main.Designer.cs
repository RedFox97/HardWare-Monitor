namespace HardWareMonitor
{
    partial class Main
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.status = new System.Windows.Forms.Label();
            this.IPAddr = new System.Windows.Forms.TextBox();
            this.connectButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.AppIcon = new System.Windows.Forms.NotifyIcon(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 89);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(165, 24);
            this.label1.TabIndex = 0;
            this.label1.Text = "ESP IP Address:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 15.75F);
            this.label2.Location = new System.Drawing.Point(31, 138);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 24);
            this.label2.TabIndex = 1;
            this.label2.Text = "Status:";
            // 
            // status
            // 
            this.status.AutoSize = true;
            this.status.ForeColor = System.Drawing.Color.Red;
            this.status.Location = new System.Drawing.Point(250, 138);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(138, 24);
            this.status.TabIndex = 3;
            this.status.Text = "Disconnected";
            // 
            // IPAddr
            // 
            this.IPAddr.Location = new System.Drawing.Point(252, 86);
            this.IPAddr.Name = "IPAddr";
            this.IPAddr.Size = new System.Drawing.Size(195, 32);
            this.IPAddr.TabIndex = 4;
            // 
            // connectButton
            // 
            this.connectButton.Location = new System.Drawing.Point(252, 182);
            this.connectButton.Name = "connectButton";
            this.connectButton.Size = new System.Drawing.Size(184, 42);
            this.connectButton.TabIndex = 5;
            this.connectButton.Text = "Connect";
            this.connectButton.UseVisualStyleBackColor = true;
            this.connectButton.Click += new System.EventHandler(this.connectButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::HardWareMonitor.Properties.Resources.title;
            this.pictureBox1.Location = new System.Drawing.Point(32, 26);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(416, 40);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // timer
            // 
            this.timer.Interval = 2000;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // AppIcon
            // 
            this.AppIcon.BalloonTipText = "Hardware LCD Monitor";
            this.AppIcon.BalloonTipTitle = "Hardware LCD Monitor";
            this.AppIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("AppIcon.Icon")));
            this.AppIcon.Text = "notifyIcon";
            this.AppIcon.Visible = true;
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(480, 250);
            this.Controls.Add(this.connectButton);
            this.Controls.Add(this.IPAddr);
            this.Controls.Add(this.status);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 15.75F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MaximizeBox = false;
            this.Name = "Main";
            this.Padding = new System.Windows.Forms.Padding(40, 111, 40, 37);
            this.Resizable = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label status;
        private System.Windows.Forms.TextBox IPAddr;
        private System.Windows.Forms.Button connectButton;
        private System.Windows.Forms.Timer timer;
        private System.Windows.Forms.NotifyIcon AppIcon;
    }
}

