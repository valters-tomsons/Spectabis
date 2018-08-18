namespace PCSX2_Spectabis
{
    partial class addGameForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(addGameForm));
            this.addGameButton = new MaterialSkin.Controls.MaterialFlatButton();
            this.gameName = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.artLabel = new MaterialSkin.Controls.MaterialLabel();
            this.materialLabel2 = new MaterialSkin.Controls.MaterialLabel();
            this.gamePath = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.materialRaisedButton1 = new MaterialSkin.Controls.MaterialRaisedButton();
            this.autoArt = new MaterialSkin.Controls.MaterialRadioButton();
            this.customArt = new MaterialSkin.Controls.MaterialRadioButton();
            this.titleLabel = new MaterialSkin.Controls.MaterialLabel();
            this.titleName = new MaterialSkin.Controls.MaterialSingleLineTextField();
            this.SuspendLayout();
            // 
            // addGameButton
            // 
            this.addGameButton.AutoSize = true;
            this.addGameButton.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.addGameButton.Depth = 0;
            this.addGameButton.Icon = null;
            this.addGameButton.Location = new System.Drawing.Point(306, 236);
            this.addGameButton.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.addGameButton.MouseState = MaterialSkin.MouseState.HOVER;
            this.addGameButton.Name = "addGameButton";
            this.addGameButton.Primary = false;
            this.addGameButton.Size = new System.Drawing.Size(90, 36);
            this.addGameButton.TabIndex = 1;
            this.addGameButton.Text = "Add Game";
            this.addGameButton.UseVisualStyleBackColor = true;
            this.addGameButton.Click += new System.EventHandler(this.addGameButton_Click);
            // 
            // gameName
            // 
            this.gameName.Depth = 0;
            this.gameName.Hint = "";
            this.gameName.Location = new System.Drawing.Point(11, 157);
            this.gameName.MaxLength = 32767;
            this.gameName.MouseState = MaterialSkin.MouseState.HOVER;
            this.gameName.Name = "gameName";
            this.gameName.PasswordChar = '\0';
            this.gameName.SelectedText = "";
            this.gameName.SelectionLength = 0;
            this.gameName.SelectionStart = 0;
            this.gameName.Size = new System.Drawing.Size(376, 23);
            this.gameName.TabIndex = 2;
            this.gameName.TabStop = false;
            this.gameName.UseSystemPasswordChar = false;
            // 
            // artLabel
            // 
            this.artLabel.AutoSize = true;
            this.artLabel.Depth = 0;
            this.artLabel.Font = new System.Drawing.Font("Roboto", 11F);
            this.artLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.artLabel.Location = new System.Drawing.Point(12, 136);
            this.artLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.artLabel.Name = "artLabel";
            this.artLabel.Size = new System.Drawing.Size(92, 19);
            this.artLabel.TabIndex = 3;
            this.artLabel.Text = "Game Name";
            // 
            // materialLabel2
            // 
            this.materialLabel2.AutoSize = true;
            this.materialLabel2.Depth = 0;
            this.materialLabel2.Font = new System.Drawing.Font("Roboto", 11F);
            this.materialLabel2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialLabel2.Location = new System.Drawing.Point(12, 81);
            this.materialLabel2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel2.Name = "materialLabel2";
            this.materialLabel2.Size = new System.Drawing.Size(123, 19);
            this.materialLabel2.TabIndex = 4;
            this.materialLabel2.Text = "Path to game file";
            // 
            // gamePath
            // 
            this.gamePath.Depth = 0;
            this.gamePath.Hint = "";
            this.gamePath.Location = new System.Drawing.Point(16, 106);
            this.gamePath.MaxLength = 32767;
            this.gamePath.MouseState = MaterialSkin.MouseState.HOVER;
            this.gamePath.Name = "gamePath";
            this.gamePath.PasswordChar = '\0';
            this.gamePath.SelectedText = "";
            this.gamePath.SelectionLength = 0;
            this.gamePath.SelectionStart = 0;
            this.gamePath.Size = new System.Drawing.Size(273, 23);
            this.gamePath.TabIndex = 5;
            this.gamePath.TabStop = false;
            this.gamePath.UseSystemPasswordChar = false;
            // 
            // materialRaisedButton1
            // 
            this.materialRaisedButton1.AutoSize = true;
            this.materialRaisedButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.materialRaisedButton1.Depth = 0;
            this.materialRaisedButton1.Icon = null;
            this.materialRaisedButton1.Location = new System.Drawing.Point(300, 106);
            this.materialRaisedButton1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialRaisedButton1.Name = "materialRaisedButton1";
            this.materialRaisedButton1.Primary = true;
            this.materialRaisedButton1.Size = new System.Drawing.Size(76, 36);
            this.materialRaisedButton1.TabIndex = 6;
            this.materialRaisedButton1.Text = "Browse";
            this.materialRaisedButton1.UseVisualStyleBackColor = true;
            this.materialRaisedButton1.Click += new System.EventHandler(this.materialRaisedButton1_Click);
            // 
            // autoArt
            // 
            this.autoArt.AutoSize = true;
            this.autoArt.Checked = true;
            this.autoArt.Depth = 0;
            this.autoArt.Font = new System.Drawing.Font("Roboto", 10F);
            this.autoArt.Location = new System.Drawing.Point(16, 215);
            this.autoArt.Margin = new System.Windows.Forms.Padding(0);
            this.autoArt.MouseLocation = new System.Drawing.Point(-1, -1);
            this.autoArt.MouseState = MaterialSkin.MouseState.HOVER;
            this.autoArt.Name = "autoArt";
            this.autoArt.Ripple = true;
            this.autoArt.Size = new System.Drawing.Size(107, 30);
            this.autoArt.TabIndex = 8;
            this.autoArt.TabStop = true;
            this.autoArt.Text = "Auto Box Art";
            this.autoArt.UseVisualStyleBackColor = true;
            // 
            // customArt
            // 
            this.customArt.AutoSize = true;
            this.customArt.Depth = 0;
            this.customArt.Font = new System.Drawing.Font("Roboto", 10F);
            this.customArt.Location = new System.Drawing.Point(16, 245);
            this.customArt.Margin = new System.Windows.Forms.Padding(0);
            this.customArt.MouseLocation = new System.Drawing.Point(-1, -1);
            this.customArt.MouseState = MaterialSkin.MouseState.HOVER;
            this.customArt.Name = "customArt";
            this.customArt.Ripple = true;
            this.customArt.Size = new System.Drawing.Size(126, 30);
            this.customArt.TabIndex = 9;
            this.customArt.Text = "Custom Box Art";
            this.customArt.UseVisualStyleBackColor = true;
            this.customArt.CheckedChanged += new System.EventHandler(this.customArt_CheckedChanged);
            // 
            // titleLabel
            // 
            this.titleLabel.AutoSize = true;
            this.titleLabel.Depth = 0;
            this.titleLabel.Font = new System.Drawing.Font("Roboto", 11F);
            this.titleLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.titleLabel.Location = new System.Drawing.Point(12, 188);
            this.titleLabel.MouseState = MaterialSkin.MouseState.HOVER;
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(92, 19);
            this.titleLabel.TabIndex = 10;
            this.titleLabel.Text = "Game Name";
            this.titleLabel.Visible = false;
            // 
            // titleName
            // 
            this.titleName.Depth = 0;
            this.titleName.Hint = "";
            this.titleName.Location = new System.Drawing.Point(110, 188);
            this.titleName.MaxLength = 32767;
            this.titleName.MouseState = MaterialSkin.MouseState.HOVER;
            this.titleName.Name = "titleName";
            this.titleName.PasswordChar = '\0';
            this.titleName.SelectedText = "";
            this.titleName.SelectionLength = 0;
            this.titleName.SelectionStart = 0;
            this.titleName.Size = new System.Drawing.Size(277, 23);
            this.titleName.TabIndex = 11;
            this.titleName.TabStop = false;
            this.titleName.UseSystemPasswordChar = false;
            this.titleName.Visible = false;
            // 
            // addGameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 287);
            this.Controls.Add(this.titleName);
            this.Controls.Add(this.titleLabel);
            this.Controls.Add(this.customArt);
            this.Controls.Add(this.autoArt);
            this.Controls.Add(this.materialRaisedButton1);
            this.Controls.Add(this.gamePath);
            this.Controls.Add(this.materialLabel2);
            this.Controls.Add(this.artLabel);
            this.Controls.Add(this.gameName);
            this.Controls.Add(this.addGameButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "addGameForm";
            this.Sizable = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add a Game";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MaterialSkin.Controls.MaterialFlatButton addGameButton;
        private MaterialSkin.Controls.MaterialSingleLineTextField gameName;
        private MaterialSkin.Controls.MaterialLabel artLabel;
        private MaterialSkin.Controls.MaterialLabel materialLabel2;
        private MaterialSkin.Controls.MaterialSingleLineTextField gamePath;
        private MaterialSkin.Controls.MaterialRaisedButton materialRaisedButton1;
        private MaterialSkin.Controls.MaterialRadioButton autoArt;
        private MaterialSkin.Controls.MaterialRadioButton customArt;
        private MaterialSkin.Controls.MaterialLabel titleLabel;
        private MaterialSkin.Controls.MaterialSingleLineTextField titleName;
    }
}