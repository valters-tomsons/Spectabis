namespace PCSX2_Spectabis
{
    partial class settingsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(settingsForm));
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.emulatordir = new MaterialSkin.Controls.MaterialLabel();
            this.dirButton = new MaterialSkin.Controls.MaterialRaisedButton();
            this.materialCheckBox1 = new MaterialSkin.Controls.MaterialCheckBox();
            this.materialCheckBox2 = new MaterialSkin.Controls.MaterialCheckBox();
            this.materialLabel2 = new MaterialSkin.Controls.MaterialLabel();
            this.blacklistBtn = new MaterialSkin.Controls.MaterialRaisedButton();
            this.showTitle = new MaterialSkin.Controls.MaterialCheckBox();
            this.allowAutoDownload = new MaterialSkin.Controls.MaterialCheckBox();
            this.colorschemelist = new MaterialSkin.Controls.MaterialContextMenuStrip();
            this.blueGreyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pinkToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allowDropBoxArt = new MaterialSkin.Controls.MaterialCheckBox();
            this.colorschemelist.SuspendLayout();
            this.SuspendLayout();
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel1.Location = new System.Drawing.Point(12, 97);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(135, 19);
            this.materialLabel1.TabIndex = 0;
            this.materialLabel1.Text = "Emulator Directory";
            // 
            // emulatordir
            // 
            this.emulatordir.AutoSize = true;
            this.emulatordir.Depth = 0;
            this.emulatordir.Font = new System.Drawing.Font("Roboto", 11F);
            this.emulatordir.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.emulatordir.Location = new System.Drawing.Point(13, 122);
            this.emulatordir.MouseState = MaterialSkin.MouseState.HOVER;
            this.emulatordir.Name = "emulatordir";
            this.emulatordir.Size = new System.Drawing.Size(160, 19);
            this.emulatordir.TabIndex = 2;
            this.emulatordir.Text = "C:/shouldnt/show/this";
            // 
            // dirButton
            // 
            this.dirButton.AutoSize = true;
            this.dirButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.dirButton.Depth = 0;
            this.dirButton.Icon = null;
            this.dirButton.Location = new System.Drawing.Point(277, 105);
            this.dirButton.MouseState = MaterialSkin.MouseState.HOVER;
            this.dirButton.Name = "dirButton";
            this.dirButton.Primary = true;
            this.dirButton.Size = new System.Drawing.Size(76, 36);
            this.dirButton.TabIndex = 3;
            this.dirButton.Text = "Change";
            this.dirButton.UseVisualStyleBackColor = true;
            this.dirButton.Click += new System.EventHandler(this.dirButton_Click);
            // 
            // materialCheckBox1
            // 
            this.materialCheckBox1.AutoSize = true;
            this.materialCheckBox1.Depth = 0;
            this.materialCheckBox1.Font = new System.Drawing.Font("Roboto", 10F);
            this.materialCheckBox1.Location = new System.Drawing.Point(17, 252);
            this.materialCheckBox1.Margin = new System.Windows.Forms.Padding(0);
            this.materialCheckBox1.MouseLocation = new System.Drawing.Point(-1, -1);
            this.materialCheckBox1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCheckBox1.Name = "materialCheckBox1";
            this.materialCheckBox1.Ripple = true;
            this.materialCheckBox1.Size = new System.Drawing.Size(102, 30);
            this.materialCheckBox1.TabIndex = 4;
            this.materialCheckBox1.Text = "Night Mode";
            this.materialCheckBox1.UseVisualStyleBackColor = true;
            this.materialCheckBox1.CheckedChanged += new System.EventHandler(this.materialCheckBox1_CheckedChanged);
            // 
            // materialCheckBox2
            // 
            this.materialCheckBox2.AutoSize = true;
            this.materialCheckBox2.Depth = 0;
            this.materialCheckBox2.Font = new System.Drawing.Font("Roboto", 10F);
            this.materialCheckBox2.Location = new System.Drawing.Point(17, 282);
            this.materialCheckBox2.Margin = new System.Windows.Forms.Padding(0);
            this.materialCheckBox2.MouseLocation = new System.Drawing.Point(-1, -1);
            this.materialCheckBox2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCheckBox2.Name = "materialCheckBox2";
            this.materialCheckBox2.Ripple = true;
            this.materialCheckBox2.Size = new System.Drawing.Size(172, 30);
            this.materialCheckBox2.TabIndex = 5;
            this.materialCheckBox2.Text = "Double-Click to Launch";
            this.materialCheckBox2.UseVisualStyleBackColor = true;
            // 
            // materialLabel2
            // 
            this.materialLabel2.AutoSize = true;
            this.materialLabel2.Depth = 0;
            this.materialLabel2.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel2.Location = new System.Drawing.Point(13, 185);
            this.materialLabel2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel2.Name = "materialLabel2";
            this.materialLabel2.Size = new System.Drawing.Size(206, 19);
            this.materialLabel2.TabIndex = 7;
            this.materialLabel2.Text = "Auto directory game blacklist";
            // 
            // blacklistBtn
            // 
            this.blacklistBtn.AutoSize = true;
            this.blacklistBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.blacklistBtn.Depth = 0;
            this.blacklistBtn.Icon = null;
            this.blacklistBtn.Location = new System.Drawing.Point(277, 177);
            this.blacklistBtn.MouseState = MaterialSkin.MouseState.HOVER;
            this.blacklistBtn.Name = "blacklistBtn";
            this.blacklistBtn.Primary = true;
            this.blacklistBtn.Size = new System.Drawing.Size(78, 36);
            this.blacklistBtn.TabIndex = 8;
            this.blacklistBtn.Text = "Manage";
            this.blacklistBtn.UseVisualStyleBackColor = true;
            this.blacklistBtn.Click += new System.EventHandler(this.blacklistBtn_Click);
            // 
            // showTitle
            // 
            this.showTitle.AutoSize = true;
            this.showTitle.Depth = 0;
            this.showTitle.Font = new System.Drawing.Font("Roboto", 10F);
            this.showTitle.Location = new System.Drawing.Point(17, 312);
            this.showTitle.Margin = new System.Windows.Forms.Padding(0);
            this.showTitle.MouseLocation = new System.Drawing.Point(-1, -1);
            this.showTitle.MouseState = MaterialSkin.MouseState.HOVER;
            this.showTitle.Name = "showTitle";
            this.showTitle.Ripple = true;
            this.showTitle.Size = new System.Drawing.Size(244, 30);
            this.showTitle.TabIndex = 10;
            this.showTitle.Text = "Show game title (Restart Required)";
            this.showTitle.UseVisualStyleBackColor = true;
            this.showTitle.Click += new System.EventHandler(this.showTitle_Click);
            // 
            // allowAutoDownload
            // 
            this.allowAutoDownload.AutoSize = true;
            this.allowAutoDownload.Depth = 0;
            this.allowAutoDownload.Font = new System.Drawing.Font("Roboto", 10F);
            this.allowAutoDownload.Location = new System.Drawing.Point(17, 342);
            this.allowAutoDownload.Margin = new System.Windows.Forms.Padding(0);
            this.allowAutoDownload.MouseLocation = new System.Drawing.Point(-1, -1);
            this.allowAutoDownload.MouseState = MaterialSkin.MouseState.HOVER;
            this.allowAutoDownload.Name = "allowAutoDownload";
            this.allowAutoDownload.Ripple = true;
            this.allowAutoDownload.Size = new System.Drawing.Size(262, 30);
            this.allowAutoDownload.TabIndex = 11;
            this.allowAutoDownload.Text = "Download game box art automatically";
            this.allowAutoDownload.UseVisualStyleBackColor = true;
            // 
            // colorschemelist
            // 
            this.colorschemelist.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.colorschemelist.Depth = 0;
            this.colorschemelist.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.blueGreyToolStripMenuItem,
            this.pinkToolStripMenuItem});
            this.colorschemelist.MouseState = MaterialSkin.MouseState.HOVER;
            this.colorschemelist.Name = "colorshemelist";
            this.colorschemelist.Size = new System.Drawing.Size(125, 48);
            // 
            // blueGreyToolStripMenuItem
            // 
            this.blueGreyToolStripMenuItem.Name = "blueGreyToolStripMenuItem";
            this.blueGreyToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.blueGreyToolStripMenuItem.Text = "Blue Grey";
            // 
            // pinkToolStripMenuItem
            // 
            this.pinkToolStripMenuItem.Name = "pinkToolStripMenuItem";
            this.pinkToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.pinkToolStripMenuItem.Text = "Pink";
            // 
            // allowDropBoxArt
            // 
            this.allowDropBoxArt.AutoSize = true;
            this.allowDropBoxArt.Depth = 0;
            this.allowDropBoxArt.Font = new System.Drawing.Font("Roboto", 10F);
            this.allowDropBoxArt.Location = new System.Drawing.Point(17, 372);
            this.allowDropBoxArt.Margin = new System.Windows.Forms.Padding(0);
            this.allowDropBoxArt.MouseLocation = new System.Drawing.Point(-1, -1);
            this.allowDropBoxArt.MouseState = MaterialSkin.MouseState.HOVER;
            this.allowDropBoxArt.Name = "allowDropBoxArt";
            this.allowDropBoxArt.Ripple = true;
            this.allowDropBoxArt.Size = new System.Drawing.Size(289, 30);
            this.allowDropBoxArt.TabIndex = 14;
            this.allowDropBoxArt.Text = "Allow auto box art when using drag & drop";
            this.allowDropBoxArt.UseVisualStyleBackColor = true;
            // 
            // settingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 605);
            this.Controls.Add(this.allowDropBoxArt);
            this.Controls.Add(this.allowAutoDownload);
            this.Controls.Add(this.showTitle);
            this.Controls.Add(this.blacklistBtn);
            this.Controls.Add(this.materialLabel2);
            this.Controls.Add(this.materialCheckBox2);
            this.Controls.Add(this.materialCheckBox1);
            this.Controls.Add(this.dirButton);
            this.Controls.Add(this.emulatordir);
            this.Controls.Add(this.materialLabel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "settingsForm";
            this.Sizable = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.settingsForm_Load);
            this.colorschemelist.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private MaterialSkin.Controls.MaterialLabel emulatordir;
        private MaterialSkin.Controls.MaterialRaisedButton dirButton;
        private MaterialSkin.Controls.MaterialCheckBox materialCheckBox1;
        private MaterialSkin.Controls.MaterialCheckBox materialCheckBox2;
        private MaterialSkin.Controls.MaterialLabel materialLabel2;
        private MaterialSkin.Controls.MaterialRaisedButton blacklistBtn;
        private MaterialSkin.Controls.MaterialCheckBox showTitle;
        private MaterialSkin.Controls.MaterialCheckBox allowAutoDownload;
        private MaterialSkin.Controls.MaterialContextMenuStrip colorschemelist;
        private System.Windows.Forms.ToolStripMenuItem blueGreyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pinkToolStripMenuItem;
        private MaterialSkin.Controls.MaterialCheckBox allowDropBoxArt;
    }
}