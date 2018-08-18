namespace PCSX2_Spectabis
{
    partial class gameSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(gameSettings));
            this.boxArt = new System.Windows.Forms.PictureBox();
            this.fullscreen = new MaterialSkin.Controls.MaterialCheckBox();
            this.nogui = new MaterialSkin.Controls.MaterialCheckBox();
            this.fullboot = new MaterialSkin.Controls.MaterialCheckBox();
            this.chgimg = new MaterialSkin.Controls.MaterialRaisedButton();
            this.nohacks = new MaterialSkin.Controls.MaterialCheckBox();
            this.controller_btn = new MaterialSkin.Controls.MaterialFlatButton();
            this.video_btn = new MaterialSkin.Controls.MaterialFlatButton();
            this.audio_btn = new MaterialSkin.Controls.MaterialFlatButton();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.zoom = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.materialLabel2 = new MaterialSkin.Controls.MaterialLabel();
            this.aspectratio = new System.Windows.Forms.ComboBox();
            this.materialLabel3 = new MaterialSkin.Controls.MaterialLabel();
            this.isoDirBox = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.browseIso = new MaterialSkin.Controls.MaterialRaisedButton();
            this.widescreen = new MaterialSkin.Controls.MaterialCheckBox();
            this.hwmipmap = new MaterialSkin.Controls.MaterialCheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.boxArt)).BeginInit();
            this.SuspendLayout();
            // 
            // boxArt
            // 
            this.boxArt.Location = new System.Drawing.Point(12, 78);
            this.boxArt.Name = "boxArt";
            this.boxArt.Size = new System.Drawing.Size(150, 200);
            this.boxArt.TabIndex = 0;
            this.boxArt.TabStop = false;
            // 
            // fullscreen
            // 
            this.fullscreen.AutoSize = true;
            this.fullscreen.Depth = 0;
            this.fullscreen.Font = new System.Drawing.Font("Roboto", 10F);
            this.fullscreen.Location = new System.Drawing.Point(175, 78);
            this.fullscreen.Margin = new System.Windows.Forms.Padding(0);
            this.fullscreen.MouseLocation = new System.Drawing.Point(-1, -1);
            this.fullscreen.MouseState = MaterialSkin.MouseState.HOVER;
            this.fullscreen.Name = "fullscreen";
            this.fullscreen.Ripple = true;
            this.fullscreen.Size = new System.Drawing.Size(94, 30);
            this.fullscreen.TabIndex = 1;
            this.fullscreen.Text = "Fullscreen";
            this.fullscreen.UseVisualStyleBackColor = true;
            // 
            // nogui
            // 
            this.nogui.AutoSize = true;
            this.nogui.Depth = 0;
            this.nogui.Font = new System.Drawing.Font("Roboto", 10F);
            this.nogui.Location = new System.Drawing.Point(175, 108);
            this.nogui.Margin = new System.Windows.Forms.Padding(0);
            this.nogui.MouseLocation = new System.Drawing.Point(-1, -1);
            this.nogui.MouseState = MaterialSkin.MouseState.HOVER;
            this.nogui.Name = "nogui";
            this.nogui.Ripple = true;
            this.nogui.Size = new System.Drawing.Size(168, 30);
            this.nogui.TabIndex = 2;
            this.nogui.Text = "No Graphical Interface";
            this.nogui.UseVisualStyleBackColor = true;
            // 
            // fullboot
            // 
            this.fullboot.AutoSize = true;
            this.fullboot.Depth = 0;
            this.fullboot.Font = new System.Drawing.Font("Roboto", 10F);
            this.fullboot.Location = new System.Drawing.Point(175, 138);
            this.fullboot.Margin = new System.Windows.Forms.Padding(0);
            this.fullboot.MouseLocation = new System.Drawing.Point(-1, -1);
            this.fullboot.MouseState = MaterialSkin.MouseState.HOVER;
            this.fullboot.Name = "fullboot";
            this.fullboot.Ripple = true;
            this.fullboot.Size = new System.Drawing.Size(84, 30);
            this.fullboot.TabIndex = 3;
            this.fullboot.Text = "Full Boot";
            this.fullboot.UseVisualStyleBackColor = true;
            // 
            // chgimg
            // 
            this.chgimg.AutoSize = true;
            this.chgimg.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.chgimg.Depth = 0;
            this.chgimg.Icon = null;
            this.chgimg.Location = new System.Drawing.Point(12, 284);
            this.chgimg.MouseState = MaterialSkin.MouseState.HOVER;
            this.chgimg.Name = "chgimg";
            this.chgimg.Primary = true;
            this.chgimg.Size = new System.Drawing.Size(76, 36);
            this.chgimg.TabIndex = 4;
            this.chgimg.Text = "Change";
            this.chgimg.UseVisualStyleBackColor = true;
            this.chgimg.Click += new System.EventHandler(this.chgimg_Click);
            // 
            // nohacks
            // 
            this.nohacks.AutoSize = true;
            this.nohacks.Depth = 0;
            this.nohacks.Font = new System.Drawing.Font("Roboto", 10F);
            this.nohacks.Location = new System.Drawing.Point(175, 168);
            this.nohacks.Margin = new System.Windows.Forms.Padding(0);
            this.nohacks.MouseLocation = new System.Drawing.Point(-1, -1);
            this.nohacks.MouseState = MaterialSkin.MouseState.HOVER;
            this.nohacks.Name = "nohacks";
            this.nohacks.Ripple = true;
            this.nohacks.Size = new System.Drawing.Size(154, 30);
            this.nohacks.TabIndex = 5;
            this.nohacks.Text = "Disable Speedhacks";
            this.nohacks.UseVisualStyleBackColor = true;
            // 
            // controller_btn
            // 
            this.controller_btn.AutoSize = true;
            this.controller_btn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.controller_btn.Depth = 0;
            this.controller_btn.Icon = null;
            this.controller_btn.Location = new System.Drawing.Point(603, 369);
            this.controller_btn.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.controller_btn.MouseState = MaterialSkin.MouseState.HOVER;
            this.controller_btn.Name = "controller_btn";
            this.controller_btn.Primary = false;
            this.controller_btn.Size = new System.Drawing.Size(184, 36);
            this.controller_btn.TabIndex = 6;
            this.controller_btn.Text = "Configure Controller";
            this.controller_btn.UseVisualStyleBackColor = true;
            this.controller_btn.Click += new System.EventHandler(this.controller_btn_Click);
            // 
            // video_btn
            // 
            this.video_btn.AutoSize = true;
            this.video_btn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.video_btn.Depth = 0;
            this.video_btn.Icon = null;
            this.video_btn.Location = new System.Drawing.Point(649, 273);
            this.video_btn.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.video_btn.MouseState = MaterialSkin.MouseState.HOVER;
            this.video_btn.Name = "video_btn";
            this.video_btn.Primary = false;
            this.video_btn.Size = new System.Drawing.Size(138, 36);
            this.video_btn.TabIndex = 7;
            this.video_btn.Text = "Configure Video";
            this.video_btn.UseVisualStyleBackColor = true;
            this.video_btn.Click += new System.EventHandler(this.video_btn_Click);
            // 
            // audio_btn
            // 
            this.audio_btn.AutoSize = true;
            this.audio_btn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.audio_btn.Depth = 0;
            this.audio_btn.Icon = null;
            this.audio_btn.Location = new System.Drawing.Point(648, 321);
            this.audio_btn.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.audio_btn.MouseState = MaterialSkin.MouseState.HOVER;
            this.audio_btn.Name = "audio_btn";
            this.audio_btn.Primary = false;
            this.audio_btn.Size = new System.Drawing.Size(139, 36);
            this.audio_btn.TabIndex = 8;
            this.audio_btn.Text = "Configure Audio";
            this.audio_btn.UseVisualStyleBackColor = true;
            this.audio_btn.Click += new System.EventHandler(this.audio_btn_Click);
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel1.Location = new System.Drawing.Point(599, 89);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(78, 19);
            this.materialLabel1.TabIndex = 9;
            this.materialLabel1.Text = "Zoom (%):";
            // 
            // zoom
            // 
            this.zoom.Depth = 0;
            this.zoom.Hint = "";
            this.zoom.Location = new System.Drawing.Point(683, 89);
            this.zoom.MaxLength = 32767;
            this.zoom.MouseState = MaterialSkin.MouseState.HOVER;
            this.zoom.Name = "zoom";
            this.zoom.PasswordChar = '\0';
            this.zoom.SelectedText = "";
            this.zoom.SelectionLength = 0;
            this.zoom.SelectionStart = 0;
            this.zoom.Size = new System.Drawing.Size(104, 23);
            this.zoom.TabIndex = 10;
            this.zoom.TabStop = false;
            this.zoom.Text = "100.00";
            this.zoom.UseSystemPasswordChar = false;
            // 
            // materialLabel2
            // 
            this.materialLabel2.AutoSize = true;
            this.materialLabel2.Depth = 0;
            this.materialLabel2.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel2.Location = new System.Drawing.Point(582, 119);
            this.materialLabel2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel2.Name = "materialLabel2";
            this.materialLabel2.Size = new System.Drawing.Size(95, 19);
            this.materialLabel2.TabIndex = 11;
            this.materialLabel2.Text = "Aspect Ratio";
            // 
            // aspectratio
            // 
            this.aspectratio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.aspectratio.FormattingEnabled = true;
            this.aspectratio.Items.AddRange(new object[] {
            "Letterbox",
            "Widescreen",
            "Stretched"});
            this.aspectratio.Location = new System.Drawing.Point(683, 119);
            this.aspectratio.Name = "aspectratio";
            this.aspectratio.Size = new System.Drawing.Size(104, 21);
            this.aspectratio.TabIndex = 12;
            // 
            // materialLabel3
            // 
            this.materialLabel3.AutoSize = true;
            this.materialLabel3.Depth = 0;
            this.materialLabel3.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel3.Location = new System.Drawing.Point(13, 343);
            this.materialLabel3.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel3.Name = "materialLabel3";
            this.materialLabel3.Size = new System.Drawing.Size(141, 19);
            this.materialLabel3.TabIndex = 13;
            this.materialLabel3.Text = "Game File Directory";
            // 
            // isoDirBox
            // 
            this.isoDirBox.Depth = 0;
            this.isoDirBox.Enabled = false;
            this.isoDirBox.Hint = "";
            this.isoDirBox.Location = new System.Drawing.Point(17, 369);
            this.isoDirBox.MaxLength = 32767;
            this.isoDirBox.MouseState = MaterialSkin.MouseState.HOVER;
            this.isoDirBox.Name = "isoDirBox";
            this.isoDirBox.PasswordChar = '\0';
            this.isoDirBox.SelectedText = "";
            this.isoDirBox.SelectionLength = 0;
            this.isoDirBox.SelectionStart = 0;
            this.isoDirBox.Size = new System.Drawing.Size(312, 23);
            this.isoDirBox.TabIndex = 14;
            this.isoDirBox.TabStop = false;
            this.isoDirBox.Text = "null";
            this.isoDirBox.UseSystemPasswordChar = false;
            // 
            // browseIso
            // 
            this.browseIso.AutoSize = true;
            this.browseIso.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.browseIso.Depth = 0;
            this.browseIso.Icon = null;
            this.browseIso.Location = new System.Drawing.Point(335, 362);
            this.browseIso.MouseState = MaterialSkin.MouseState.HOVER;
            this.browseIso.Name = "browseIso";
            this.browseIso.Primary = true;
            this.browseIso.Size = new System.Drawing.Size(76, 36);
            this.browseIso.TabIndex = 15;
            this.browseIso.Text = "Browse";
            this.browseIso.UseVisualStyleBackColor = true;
            this.browseIso.Click += new System.EventHandler(this.browseIso_Click);
            // 
            // widescreen
            // 
            this.widescreen.AutoSize = true;
            this.widescreen.Depth = 0;
            this.widescreen.Font = new System.Drawing.Font("Roboto", 10F);
            this.widescreen.Location = new System.Drawing.Point(175, 198);
            this.widescreen.Margin = new System.Windows.Forms.Padding(0);
            this.widescreen.MouseLocation = new System.Drawing.Point(-1, -1);
            this.widescreen.MouseState = MaterialSkin.MouseState.HOVER;
            this.widescreen.Name = "widescreen";
            this.widescreen.Ripple = true;
            this.widescreen.Size = new System.Drawing.Size(156, 30);
            this.widescreen.TabIndex = 16;
            this.widescreen.Text = "Widescreen Patches";
            this.widescreen.UseVisualStyleBackColor = true;
            // 
            // hwmipmap
            // 
            this.hwmipmap.AutoSize = true;
            this.hwmipmap.Checked = true;
            this.hwmipmap.CheckState = System.Windows.Forms.CheckState.Checked;
            this.hwmipmap.Depth = 0;
            this.hwmipmap.Font = new System.Drawing.Font("Roboto", 10F);
            this.hwmipmap.Location = new System.Drawing.Point(175, 228);
            this.hwmipmap.Margin = new System.Windows.Forms.Padding(0);
            this.hwmipmap.MouseLocation = new System.Drawing.Point(-1, -1);
            this.hwmipmap.MouseState = MaterialSkin.MouseState.HOVER;
            this.hwmipmap.Name = "hwmipmap";
            this.hwmipmap.Ripple = true;
            this.hwmipmap.Size = new System.Drawing.Size(185, 30);
            this.hwmipmap.TabIndex = 17;
            this.hwmipmap.Text = "Enable HW Mipmap Hack";
            this.hwmipmap.UseVisualStyleBackColor = true;
            this.hwmipmap.CheckedChanged += new System.EventHandler(this.hwmipmap_CheckedChanged);
            // 
            // gameSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 420);
            this.Controls.Add(this.hwmipmap);
            this.Controls.Add(this.widescreen);
            this.Controls.Add(this.browseIso);
            this.Controls.Add(this.isoDirBox);
            this.Controls.Add(this.materialLabel3);
            this.Controls.Add(this.aspectratio);
            this.Controls.Add(this.materialLabel2);
            this.Controls.Add(this.zoom);
            this.Controls.Add(this.materialLabel1);
            this.Controls.Add(this.audio_btn);
            this.Controls.Add(this.video_btn);
            this.Controls.Add(this.controller_btn);
            this.Controls.Add(this.nohacks);
            this.Controls.Add(this.chgimg);
            this.Controls.Add(this.fullboot);
            this.Controls.Add(this.nogui);
            this.Controls.Add(this.fullscreen);
            this.Controls.Add(this.boxArt);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "gameSettings";
            this.Sizable = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "null";
            ((System.ComponentModel.ISupportInitialize)(this.boxArt)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox boxArt;
        private MaterialSkin.Controls.MaterialCheckBox fullscreen;
        private MaterialSkin.Controls.MaterialCheckBox nogui;
        private MaterialSkin.Controls.MaterialCheckBox fullboot;
        private MaterialSkin.Controls.MaterialRaisedButton chgimg;
        private MaterialSkin.Controls.MaterialCheckBox nohacks;
        private MaterialSkin.Controls.MaterialFlatButton controller_btn;
        private MaterialSkin.Controls.MaterialFlatButton video_btn;
        private MaterialSkin.Controls.MaterialFlatButton audio_btn;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private MaterialSkin.Controls.MaterialSingleLineTextField zoom;
        private MaterialSkin.Controls.MaterialLabel materialLabel2;
        private System.Windows.Forms.ComboBox aspectratio;
        private MaterialSkin.Controls.MaterialLabel materialLabel3;
        private MaterialSkin.Controls.MaterialSingleLineTextField isoDirBox;
        private MaterialSkin.Controls.MaterialRaisedButton browseIso;
        private MaterialSkin.Controls.MaterialCheckBox widescreen;
        private MaterialSkin.Controls.MaterialCheckBox hwmipmap;
    }
}