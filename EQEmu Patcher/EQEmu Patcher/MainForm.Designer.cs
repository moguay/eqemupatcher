namespace EQEmu_Patcher
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.txtList = new System.Windows.Forms.TextBox();
            this.btnStart = new System.Windows.Forms.Button();
            this.btnCheck = new System.Windows.Forms.Button();
            this.chkAutoPlay = new System.Windows.Forms.CheckBox();
            this.chkAutoPatch = new System.Windows.Forms.CheckBox();
            this.chkAsyncPatch = new System.Windows.Forms.CheckBox();
            this.chkLogPatch = new System.Windows.Forms.CheckBox();
            this.splashLogo = new System.Windows.Forms.PictureBox();
            this.labelSpeed = new System.Windows.Forms.Label();
            this.labelDownloaded = new System.Windows.Forms.Label();
            this.labelPerc = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splashLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(10, 489);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(768, 14);
            this.progressBar.TabIndex = 0;
            // 
            // txtList
            // 
            this.txtList.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtList.HideSelection = false;
            this.txtList.Location = new System.Drawing.Point(10, 330);
            this.txtList.Multiline = true;
            this.txtList.Name = "txtList";
            this.txtList.ReadOnly = true;
            this.txtList.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtList.Size = new System.Drawing.Size(768, 65);
            this.txtList.TabIndex = 1;
            // 
            // btnStart
            // 
            this.btnStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStart.Location = new System.Drawing.Point(683, 402);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(95, 52);
            this.btnStart.TabIndex = 3;
            this.btnStart.Text = "Play";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Visible = false;
            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // btnCheck
            // 
            this.btnCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCheck.Location = new System.Drawing.Point(10, 402);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Size = new System.Drawing.Size(95, 52);
            this.btnCheck.TabIndex = 6;
            this.btnCheck.Text = "Patch";
            this.btnCheck.UseVisualStyleBackColor = true;
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // chkAutoPlay
            // 
            this.chkAutoPlay.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkAutoPlay.AutoSize = true;
            this.chkAutoPlay.BackColor = System.Drawing.Color.Transparent;
            this.chkAutoPlay.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutoPlay.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkAutoPlay.Location = new System.Drawing.Point(236, 402);
            this.chkAutoPlay.Name = "chkAutoPlay";
            this.chkAutoPlay.Size = new System.Drawing.Size(80, 17);
            this.chkAutoPlay.TabIndex = 7;
            this.chkAutoPlay.Text = "Auto Play";
            this.chkAutoPlay.UseVisualStyleBackColor = false;
            this.chkAutoPlay.Visible = false;
            this.chkAutoPlay.CheckedChanged += new System.EventHandler(this.chkAutoPlay_CheckedChanged);
            // 
            // chkAutoPatch
            // 
            this.chkAutoPatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkAutoPatch.AutoSize = true;
            this.chkAutoPatch.BackColor = System.Drawing.Color.Transparent;
            this.chkAutoPatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutoPatch.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkAutoPatch.Location = new System.Drawing.Point(111, 402);
            this.chkAutoPatch.Name = "chkAutoPatch";
            this.chkAutoPatch.Size = new System.Drawing.Size(89, 17);
            this.chkAutoPatch.TabIndex = 8;
            this.chkAutoPatch.Text = "Auto Patch";
            this.chkAutoPatch.UseVisualStyleBackColor = false;
            this.chkAutoPatch.CheckedChanged += new System.EventHandler(this.chkAutoPatch_CheckedChanged);
            // 
            // chkAsyncPatch
            // 
            this.chkAsyncPatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkAsyncPatch.AutoSize = true;
            this.chkAsyncPatch.BackColor = System.Drawing.Color.Transparent;
            this.chkAsyncPatch.Checked = true;
            this.chkAsyncPatch.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAsyncPatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAsyncPatch.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkAsyncPatch.Location = new System.Drawing.Point(111, 421);
            this.chkAsyncPatch.Name = "chkAsyncPatch";
            this.chkAsyncPatch.Size = new System.Drawing.Size(120, 17);
            this.chkAsyncPatch.TabIndex = 9;
            this.chkAsyncPatch.Text = "Async Download";
            this.chkAsyncPatch.UseVisualStyleBackColor = false;
            this.chkAsyncPatch.CheckedChanged += new System.EventHandler(this.chkAsyncPatch_CheckedChanged);
            // 
            // chkLogPatch
            // 
            this.chkLogPatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkLogPatch.AutoSize = true;
            this.chkLogPatch.BackColor = System.Drawing.Color.Transparent;
            this.chkLogPatch.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkLogPatch.ForeColor = System.Drawing.SystemColors.ControlText;
            this.chkLogPatch.Location = new System.Drawing.Point(111, 439);
            this.chkLogPatch.Name = "chkLogPatch";
            this.chkLogPatch.Size = new System.Drawing.Size(47, 17);
            this.chkLogPatch.TabIndex = 10;
            this.chkLogPatch.Text = "Log";
            this.chkLogPatch.UseVisualStyleBackColor = false;
            this.chkLogPatch.CheckedChanged += new System.EventHandler(this.chkLogPatch_CheckedChanged);
            // 
            // splashLogo
            // 
            this.splashLogo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splashLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.splashLogo.Image = global::EQEmu_Patcher.Properties.Resources.rof;
            this.splashLogo.Location = new System.Drawing.Point(578, 153);
            this.splashLogo.Margin = new System.Windows.Forms.Padding(0);
            this.splashLogo.MinimumSize = new System.Drawing.Size(200, 225);
            this.splashLogo.Name = "splashLogo";
            this.splashLogo.Size = new System.Drawing.Size(200, 225);
            this.splashLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.splashLogo.TabIndex = 4;
            this.splashLogo.TabStop = false;
            // 
            // labelSpeed
            // 
            this.labelSpeed.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelSpeed.AutoSize = true;
            this.labelSpeed.BackColor = System.Drawing.Color.Transparent;
            this.labelSpeed.ForeColor = System.Drawing.Color.White;
            this.labelSpeed.Location = new System.Drawing.Point(597, 473);
            this.labelSpeed.Name = "labelSpeed";
            this.labelSpeed.Size = new System.Drawing.Size(10, 13);
            this.labelSpeed.TabIndex = 13;
            this.labelSpeed.Text = "-";
            // 
            // labelDownloaded
            // 
            this.labelDownloaded.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.labelDownloaded.AutoSize = true;
            this.labelDownloaded.BackColor = System.Drawing.Color.Transparent;
            this.labelDownloaded.ForeColor = System.Drawing.SystemColors.Control;
            this.labelDownloaded.Location = new System.Drawing.Point(689, 472);
            this.labelDownloaded.Name = "labelDownloaded";
            this.labelDownloaded.Size = new System.Drawing.Size(10, 13);
            this.labelDownloaded.TabIndex = 15;
            this.labelDownloaded.Text = "-";
            // 
            // labelPerc
            // 
            this.labelPerc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.labelPerc.BackColor = System.Drawing.Color.Transparent;
            this.labelPerc.ForeColor = System.Drawing.Color.White;
            this.labelPerc.Location = new System.Drawing.Point(103, 470);
            this.labelPerc.Margin = new System.Windows.Forms.Padding(3);
            this.labelPerc.Name = "labelPerc";
            this.labelPerc.Size = new System.Drawing.Size(583, 18);
            this.labelPerc.TabIndex = 14;
            this.labelPerc.Text = "0%";
            this.labelPerc.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 71.99999F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(10, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(768, 100);
            this.label1.TabIndex = 16;
            this.label1.Text = "EQ ProFusion";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::EQEmu_Patcher.Properties.Resources.d2p6dw3_9d08ddc6_d072_4605_9386_6ac12b2178cb;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(784, 511);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.labelSpeed);
            this.Controls.Add(this.labelDownloaded);
            this.Controls.Add(this.labelPerc);
            this.Controls.Add(this.chkLogPatch);
            this.Controls.Add(this.chkAsyncPatch);
            this.Controls.Add(this.chkAutoPatch);
            this.Controls.Add(this.chkAutoPlay);
            this.Controls.Add(this.txtList);
            this.Controls.Add(this.btnStart);
            this.Controls.Add(this.btnCheck);
            this.Controls.Add(this.splashLogo);
            this.Controls.Add(this.progressBar);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(800, 500);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ProFusion Downloader";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.Shown += new System.EventHandler(this.MainForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.splashLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TextBox txtList;
        private System.Windows.Forms.Button btnStart;
        private System.Windows.Forms.PictureBox splashLogo;
        private System.Windows.Forms.Button btnCheck;
        private System.Windows.Forms.CheckBox chkAutoPlay;
        private System.Windows.Forms.CheckBox chkAutoPatch;
        private System.Windows.Forms.CheckBox chkAsyncPatch;
        private System.Windows.Forms.CheckBox chkLogPatch;
        private System.Windows.Forms.Label labelSpeed;
        private System.Windows.Forms.Label labelDownloaded;
        private System.Windows.Forms.Label labelPerc;
        private System.Windows.Forms.Label label1;
    }
}

