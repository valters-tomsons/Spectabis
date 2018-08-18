namespace PCSX2_Spectabis
{
    partial class MainWindow
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.isoPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.SettingsButton = new System.Windows.Forms.PictureBox();
            this.addGameButton = new MaterialSkin.Controls.MaterialRaisedButton();
            this.mainTimer = new System.Windows.Forms.Timer(this.components);
            this.contextMenu = new MaterialSkin.Controls.MaterialContextMenuStrip();
            this.emulatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configureToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AddDirectoryButton = new MaterialSkin.Controls.MaterialRaisedButton();
            this.refreshBtn = new MaterialSkin.Controls.MaterialFlatButton();
            this.currentTask = new MaterialSkin.Controls.MaterialLabel();
            this.refreshArt = new System.Windows.Forms.Timer(this.components);
            this.taskList = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.SettingsButton)).BeginInit();
            this.contextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // isoPanel
            // 
            this.isoPanel.AllowDrop = true;
            this.isoPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.isoPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.isoPanel.Location = new System.Drawing.Point(12, 114);
            this.isoPanel.Name = "isoPanel";
            this.isoPanel.Size = new System.Drawing.Size(989, 603);
            this.isoPanel.TabIndex = 0;
            this.isoPanel.TabStop = true;
            this.isoPanel.DragDrop += new System.Windows.Forms.DragEventHandler(this.isoPanel_DragDrop);
            this.isoPanel.DragEnter += new System.Windows.Forms.DragEventHandler(this.isoPanel_DragEnter);
            // 
            // SettingsButton
            // 
            this.SettingsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.SettingsButton.BackColor = System.Drawing.Color.Transparent;
            this.SettingsButton.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("SettingsButton.BackgroundImage")));
            this.SettingsButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SettingsButton.Location = new System.Drawing.Point(965, 32);
            this.SettingsButton.Name = "SettingsButton";
            this.SettingsButton.Size = new System.Drawing.Size(25, 25);
            this.SettingsButton.TabIndex = 1;
            this.SettingsButton.TabStop = false;
            this.SettingsButton.Click += new System.EventHandler(this.SettingsButton_Click);
            // 
            // addGameButton
            // 
            this.addGameButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addGameButton.AutoSize = true;
            this.addGameButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.addGameButton.Depth = 0;
            this.addGameButton.Icon = null;
            this.addGameButton.Location = new System.Drawing.Point(900, 72);
            this.addGameButton.MouseState = MaterialSkin.MouseState.HOVER;
            this.addGameButton.Name = "addGameButton";
            this.addGameButton.Primary = true;
            this.addGameButton.Size = new System.Drawing.Size(90, 36);
            this.addGameButton.TabIndex = 2;
            this.addGameButton.Text = "Add Game";
            this.addGameButton.UseVisualStyleBackColor = true;
            this.addGameButton.Click += new System.EventHandler(this.addGameButton_Click);
            // 
            // mainTimer
            // 
            this.mainTimer.Enabled = true;
            this.mainTimer.Tick += new System.EventHandler(this.mainTimer_Tick);
            // 
            // contextMenu
            // 
            this.contextMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.contextMenu.Depth = 0;
            this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.emulatorToolStripMenuItem,
            this.configureToolStripMenuItem,
            this.removeToolStripMenuItem});
            this.contextMenu.MouseState = MaterialSkin.MouseState.HOVER;
            this.contextMenu.Name = "materialContextMenuStrip1";
            this.contextMenu.Size = new System.Drawing.Size(201, 70);
            // 
            // emulatorToolStripMenuItem
            // 
            this.emulatorToolStripMenuItem.Name = "emulatorToolStripMenuItem";
            this.emulatorToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.emulatorToolStripMenuItem.Text = "Emulator Settings";
            this.emulatorToolStripMenuItem.Click += new System.EventHandler(this.emulatorToolStripMenuItem_Click);
            // 
            // configureToolStripMenuItem
            // 
            this.configureToolStripMenuItem.Name = "configureToolStripMenuItem";
            this.configureToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.configureToolStripMenuItem.Text = "Launcher Configuration";
            this.configureToolStripMenuItem.Click += new System.EventHandler(this.configureToolStripMenuItem_Click);
            // 
            // removeToolStripMenuItem
            // 
            this.removeToolStripMenuItem.Name = "removeToolStripMenuItem";
            this.removeToolStripMenuItem.Size = new System.Drawing.Size(200, 22);
            this.removeToolStripMenuItem.Text = "Remove";
            this.removeToolStripMenuItem.Click += new System.EventHandler(this.removeToolStripMenuItem_Click);
            // 
            // AddDirectoryButton
            // 
            this.AddDirectoryButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AddDirectoryButton.AutoSize = true;
            this.AddDirectoryButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.AddDirectoryButton.Depth = 0;
            this.AddDirectoryButton.Icon = null;
            this.AddDirectoryButton.Location = new System.Drawing.Point(770, 72);
            this.AddDirectoryButton.MouseState = MaterialSkin.MouseState.HOVER;
            this.AddDirectoryButton.Name = "AddDirectoryButton";
            this.AddDirectoryButton.Primary = true;
            this.AddDirectoryButton.Size = new System.Drawing.Size(124, 36);
            this.AddDirectoryButton.TabIndex = 3;
            this.AddDirectoryButton.Text = "Add Directory";
            this.AddDirectoryButton.UseVisualStyleBackColor = true;
            this.AddDirectoryButton.Click += new System.EventHandler(this.AddDirectoryButton_Click);
            // 
            // refreshBtn
            // 
            this.refreshBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.refreshBtn.AutoSize = true;
            this.refreshBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.refreshBtn.Depth = 0;
            this.refreshBtn.Icon = null;
            this.refreshBtn.Location = new System.Drawing.Point(732, 72);
            this.refreshBtn.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.refreshBtn.MouseState = MaterialSkin.MouseState.HOVER;
            this.refreshBtn.Name = "refreshBtn";
            this.refreshBtn.Primary = false;
            this.refreshBtn.Size = new System.Drawing.Size(31, 36);
            this.refreshBtn.TabIndex = 4;
            this.refreshBtn.Text = "↺";
            this.refreshBtn.UseVisualStyleBackColor = true;
            this.refreshBtn.Click += new System.EventHandler(this.refreshBtn_Click);
            // 
            // currentTask
            // 
            this.currentTask.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.currentTask.AutoSize = true;
            this.currentTask.Depth = 0;
            this.currentTask.Font = new System.Drawing.Font("Roboto", 11F);
            this.currentTask.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.currentTask.Location = new System.Drawing.Point(6, 695);
            this.currentTask.MouseState = MaterialSkin.MouseState.HOVER;
            this.currentTask.Name = "currentTask";
            this.currentTask.Size = new System.Drawing.Size(0, 19);
            this.currentTask.TabIndex = 5;
            // 
            // refreshArt
            // 
            this.refreshArt.Enabled = true;
            this.refreshArt.Interval = 300;
            this.refreshArt.Tick += new System.EventHandler(this.refreshArt_Tick);
            // 
            // taskList
            // 
            this.taskList.Enabled = true;
            this.taskList.Interval = 2000;
            this.taskList.Tick += new System.EventHandler(this.taskList_Tick);
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1003, 720);
            this.Controls.Add(this.currentTask);
            this.Controls.Add(this.refreshBtn);
            this.Controls.Add(this.AddDirectoryButton);
            this.Controls.Add(this.addGameButton);
            this.Controls.Add(this.SettingsButton);
            this.Controls.Add(this.isoPanel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1003, 150);
            this.Name = "MainWindow";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Spectabis";
            ((System.ComponentModel.ISupportInitialize)(this.SettingsButton)).EndInit();
            this.contextMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox SettingsButton;
        private MaterialSkin.Controls.MaterialRaisedButton addGameButton;
        private System.Windows.Forms.Timer mainTimer;
        public System.Windows.Forms.FlowLayoutPanel isoPanel;
        private MaterialSkin.Controls.MaterialContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem removeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configureToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem emulatorToolStripMenuItem;
        private MaterialSkin.Controls.MaterialRaisedButton AddDirectoryButton;
        private MaterialSkin.Controls.MaterialFlatButton refreshBtn;
        private MaterialSkin.Controls.MaterialLabel currentTask;
        private System.Windows.Forms.Timer refreshArt;
        private System.Windows.Forms.Timer taskList;
    }
}

