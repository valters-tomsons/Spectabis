using System;
using System.ComponentModel;
using System.Windows.Forms;
using MaterialSkin;
using MaterialSkin.Controls;
using System.IO;
using System.Diagnostics;
using System.Drawing;

namespace PCSX2_Spectabis
{
    public partial class settingsForm : MaterialForm
    {
        private readonly MaterialSkinManager materialSkinManager;
        //Form Reference
        public Form RefToForm1 { get; set; }

        public string emuDir = Properties.Settings.Default.EmuDir;

        public settingsForm()
        {
            InitializeComponent();

            emuDir = Properties.Settings.Default.EmuDir;

            // Initialize MaterialSkinManager
            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);


            if (Properties.Settings.Default.nightMode == true)
            {
                materialCheckBox1.Checked = true;
            }

            if (Properties.Settings.Default.doubleclick == true)
            {
                materialCheckBox2.Checked = true;
            }

            if(Properties.Settings.Default.showtitle == true)
            {
                showTitle.Checked = true;
            }

            if(Properties.Settings.Default.autoArt == true)
            {
                allowAutoDownload.Checked = true;
            }

            if(Properties.Settings.Default.dropautoart == true)
            {
                allowDropBoxArt.Checked = true;
            }

        }

        //On form closing
        protected override void OnClosing(CancelEventArgs e)
        {
            //Saves Settings
            Properties.Settings.Default.EmuDir = emuDir;
            Properties.Settings.Default.doubleclick = materialCheckBox2.Checked;
            Properties.Settings.Default.showtitle = showTitle.Checked;
            Properties.Settings.Default.autoArt = allowAutoDownload.Checked;
            Properties.Settings.Default.dropautoart = allowDropBoxArt.Checked;
            Properties.Settings.Default.Save();

            //Show mainForm
            this.RefToForm1.Show();
        }

        private void settingsForm_Load(object sender, EventArgs e)
        {
            emulatordir.Text = emuDir;
        }

        //Change Directory Button
        private void dirButton_Click(object sender, EventArgs e)
        {
            SelectDir:
            //Opens Folder Browser Dialog
            using (FolderBrowserDialog fbd = new FolderBrowserDialog() { Description = "Navigate to where PCSX2.exe is located." })
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    emuDir = fbd.SelectedPath;
                    if (File.Exists(emuDir + "/pcsx2.exe"))
                    {
                        Properties.Settings.Default.EmuDir = emuDir;
                        Properties.Settings.Default.Save();
                        emulatordir.Text = Properties.Settings.Default.EmuDir;
                    }
                    else
                    {
                        MessageBox.Show("Not a valid emulator directory");
                        goto SelectDir; //retries FolderBrowserDialog
                    }
                }
            }
        }

        //Night Mode Checkbox
        private void materialCheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(materialCheckBox1.Checked == true)
            {
                materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
                Properties.Settings.Default.nightMode = true;
                Properties.Settings.Default.Save();
            }
            else
            {
                materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
                Properties.Settings.Default.nightMode = false;
                Properties.Settings.Default.Save();
            }
        }

        //Manage game blacklist button
        private void blacklistBtn_Click(object sender, EventArgs e)
        {
            Process.Start(AppDomain.CurrentDomain.BaseDirectory + @"\resources\logs\blacklist.txt");
        }

        private void askBeforeDwnl_CheckedChanged(object sender, EventArgs e)
        {
            //throw new NotImplementedException();
        }


        //Show title checkbox, restart if checked
        private void showTitle_Click(object sender, EventArgs e)
        {
            if(showTitle.Checked == true)
            {
                DialogResult dialogResult = MessageBox.Show("Spectabis needs to be restarted in order to enable this.", "Terminate Now?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    ActiveForm.Close();
                    Application.Exit();
                }
                else if (dialogResult == DialogResult.No)
                {
                    showTitle.Checked = false;
                    return;
                }
            }

            if (showTitle.Checked == false)
            {
                DialogResult dialogResult = MessageBox.Show("Spectabis needs to be restarted in order to enable this.", "Terminate Now?", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    ActiveForm.Close();
                    Application.Exit();
                }
                else if (dialogResult == DialogResult.No)
                {
                    showTitle.Checked = true;
                    return;
                }
            }

        }

        //Click on colorscheme
        private void colorschemeBox_Click(object sender, EventArgs e)
        {
            colorschemelist.Show(Cursor.Position.X, Cursor.Position.Y);
        }
    }
}
