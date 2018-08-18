using MaterialSkin;
using MaterialSkin.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PCSX2_Spectabis
{
    public partial class gameSettings : MaterialForm
    {
        private readonly MaterialSkinManager materialSkinManager;
        public string currentGame = Properties.Settings.Default.lastGameEdit;
        public string emuDir = Properties.Settings.Default.EmuDir;
        OpenFileDialog browseImg = new OpenFileDialog();
        

        //Form Reference
        public Form RefToForm2 { get; set; }

        public gameSettings()
        {
            InitializeComponent();

            //Visual stuff
            boxArt.SizeMode = PictureBoxSizeMode.StretchImage;
            Text = "Editing - " + currentGame;
            if(File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + currentGame + @"\art.jpg"))
            {
                boxArt.ImageLocation = AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + currentGame + @"\art.jpg";
            }

            //Defines configration file location
            string cfgDir = AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + currentGame;


            //Reads the spectabis ini file 
            var gameIni = new IniFile(cfgDir + @"\spectabis.ini");
            var _nogui = gameIni.Read("nogui", "Spectabis");
            var _fullscreen = gameIni.Read("fullscreen", "Spectabis");
            var _fullboot = gameIni.Read("fullboot", "Spectabis");
            var _nohacks = gameIni.Read("nohacks", "Spectabis");
            var _isodir = gameIni.Read("isoDirectory", "Spectabis");

            //Sets the values from spectabis ini
            if (_nogui == "1") {nogui.Checked = true;}
            if (_fullscreen == "1") {fullscreen.Checked = true;}
            if (_fullboot == "1") {fullboot.Checked = true;}
            if (_nohacks == "1") {nohacks.Checked = true;}
            isoDirBox.Text = _isodir;


            //Reads the PCSX2_ui ini file
            var uiIni = new IniFile(cfgDir + @"\PCSX2_ui.ini");
            var _zoom = uiIni.Read("Zoom", "GSWindow");
            var _aspectratio = uiIni.Read("AspectRatio", "GSWindow");

            //Sets the values from PCSX2_ui ini
            zoom.Text = _zoom;
            if (_aspectratio == "4:3"){
                aspectratio.Text = "Letterbox";
            }
            else if (_aspectratio == "16:9"){
                aspectratio.Text = "Widescreen";
            }
            else{
                aspectratio.Text = "Stretched";
            }


            //Reads RCSX2_vm file
            var vmIni = new IniFile(cfgDir + @"\PCSX2_vm.ini");
            var _widescreen = vmIni.Read("EnableWideScreenPatches", "EmuCore");

            //Sets the values from PCSX2_vm ini
            if (_widescreen == "enabled") { widescreen.Checked = true;}

            //GDSX file mipmap hack
            var gsdxIni = new IniFile(cfgDir + @"\GSdx.ini");
            var _mipmaphack = gsdxIni.Read("UserHacks_mipmap", "Settings");
            if (_mipmaphack == "1"){ hwmipmap.Checked = true; } else { hwmipmap.Checked = false; }


            // Initialize MaterialSkinManager
            materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);

        }

        //On form closing
        protected override void OnClosing(CancelEventArgs e)
        {

            //Defines ini files
            var gameIni = new IniFile(AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + currentGame + @"\spectabis.ini");
            var uiIni = new IniFile(AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + currentGame + @"\PCSX2_ui.ini");
            var vmIni = new IniFile(AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + currentGame + @"\PCSX2_vm.ini");
            var gsdxIni = new IniFile(AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + currentGame + @"\GSdx.ini");

            //Writes zoom level to pcsx2_ui
            uiIni.Write("Zoom", zoom.Text, "GSWindow");

            //Aspect Ratio - written to pcsx2_ui
            if (aspectratio.Text == "Letterbox")
            {
                uiIni.Write("AspectRatio","4:3","GSWindow");
            }
            else if (aspectratio.Text == "Widescreen")
            {
                uiIni.Write("AspectRatio", "16:9", "GSWindow");
            }
            else
            {
                uiIni.Write("AspectRatio", "Stretch", "GSWindow");
            }

            //Emulation Settings - written to spectabis ini
            if (nogui.Checked == true)
            {
                gameIni.Write("nogui", "1", "Spectabis");
            }
            else
            {
                gameIni.Write("nogui", "0", "Spectabis");
            }

            if (fullscreen.Checked == true)
            {
                gameIni.Write("fullscreen", "1", "Spectabis");
            }
            else
            {
                gameIni.Write("fullscreen", "0", "Spectabis");
            }

            if(fullboot.Checked == true)
            {
                gameIni.Write("fullboot", "1", "Spectabis");
            }
            else
            {
                gameIni.Write("fullboot", "0", "Spectabis");
            }

            if(nohacks.Checked == true)
            {
                gameIni.Write("nohacks", "1", "Spectabis");
            }
            else
            {
                gameIni.Write("nohacks", "0", "Spectabis");
            }

            gameIni.Write("isoDirectory", isoDirBox.Text, "Spectabis");


            //Widescreen patch - written to pcsx2_vm
            if (widescreen.Checked == true)
            {
                vmIni.Write("EnableWideScreenPatches", "enabled", "EmuCore");
            }
            else
            {
                vmIni.Write("EnableWideScreenPatches", "disabled", "EmuCore");
            }


            //Mipmap hack - written to gsdx.ini
            if (hwmipmap.Checked == true)
            {
                gsdxIni.Write("UserHacks_mipmap", "1", "Settings");
                gsdxIni.Write("UserHacks", "1", "Settings");
            }
            else
            {
                gsdxIni.Write("UserHacks_mipmap", "0", "Settings");
            }

            //Show mainForm
            this.RefToForm2.Show();
        }

        private void chgimg_Click(object sender, EventArgs e)
        {
            browseImg.Filter = "JPEG image (.jpg)|*.jpg|PNG image (.png)|*.png";

            if (browseImg.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + currentGame + @"\art.jpg"))
                {
                    File.Delete(AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + currentGame + @"\art.jpg");
                }

                boxArt.ImageLocation = browseImg.FileName;

                using (WebClient client = new WebClient())
                {
                    try
                    {
                        client.DownloadFile(browseImg.FileName, AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + currentGame + @"\art.jpg");
                    }
                    catch
                    {
                        throw;
                    }
                }
            }

        }




        //Calls the LilyPad.dll copied from PCSX2 directory
        //It has no inputs, but writes/reads the ini files where .exe is located at folder /inis/
        //Calls the PADconfigure when controller_btn is clicked
        [DllImport(@"\plugins\LilyPad.dll")]
        static public extern void PADconfigure();

        //Configuration must be closed so .dll is not in use
        [DllImport(@"\plugins\LilyPad.dll")]
        static public extern void PADclose();

        private void controller_btn_Click(object sender, EventArgs e)
        {
            //Copy the existing .ini file for editing if it exists
            if(File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + currentGame + @"\LilyPad.ini"))
            {
                //Creates inis folder and copies it from game profile folder
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"inis");
                File.Copy(AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + currentGame + @"\LilyPad.ini", AppDomain.CurrentDomain.BaseDirectory + @"inis\LilyPad.ini", true);
            }

            //Calls the DLL configuration function
            PADconfigure();

            //Calls the configration close function
            PADclose();

            //Copies the modified file into the game profile & deletes the created folder
            File.Copy(AppDomain.CurrentDomain.BaseDirectory + @"inis\LilyPad.ini", AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + currentGame + @"\LilyPad.ini", true);
            Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + @"inis", true);
        }

        //Imports GPUconfigure from GSdx plugin
        //All GSdx plugins have same settings, by the looks of it
        [DllImport(@"\plugins\GSdx32-SSE2.dll")]
        static public extern void GSconfigure();

        [DllImport(@"\plugins\GSdx32-SSE2.dll")]
        static public extern void GSclose();

        private void video_btn_Click(object sender, EventArgs e)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + currentGame + @"\GSdx.ini"))
            {
                //Creates inis folder and copies it from game profile folder
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"inis");
                File.Copy(AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + currentGame + @"\GSdx.ini", AppDomain.CurrentDomain.BaseDirectory + @"inis\GSdx.ini", true);
            }

            //GPUConfigure(); - Only software mode was available
            GSconfigure();
            GSclose();

            File.Copy(AppDomain.CurrentDomain.BaseDirectory + @"inis\GSdx.ini", AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + currentGame + @"\GSdx.ini", true);
            Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + @"inis", true);
        }


        [DllImport(@"\plugins\Spu2-X.dll")]
        static public extern void SPU2configure();

        [DllImport(@"\plugins\Spu2-X.dll")]
        static public extern void SPU2close();

        private void audio_btn_Click(object sender, EventArgs e)
        {
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + currentGame + @"\SPU2-X.ini"))
            {
                //Creates inis folder and copies it from game profile folder
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"inis");
                File.Copy(AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + currentGame + @"\SPU2-X.ini", AppDomain.CurrentDomain.BaseDirectory + @"inis\SPU2-X.ini", true);
            }

            SPU2configure();
            SPU2close();

            File.Copy(AppDomain.CurrentDomain.BaseDirectory + @"inis\SPU2-X.ini", AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + currentGame + @"\SPU2-X.ini", true);
            Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + @"inis", true);
        }

        OpenFileDialog browseIsoDialog = new OpenFileDialog();

        //Browse Iso button
        private void browseIso_Click(object sender, EventArgs e)
        {
            //File Filter
            browseIsoDialog.Filter = "ISO image (.iso)|*.iso|Media Descriptor File (.mdf)|*.mdf|Image File (.img)|*.img|Compressed ISO (.cso)|*.cso|gzip archive (.gz)|*.gz";

            if (browseIsoDialog.ShowDialog() == DialogResult.OK)
            {
                //Sets path into textbox
                isoDirBox.Text = browseIsoDialog.FileName;
            }
        }

        //GSDX mipmap hack warning
        private void hwmipmap_CheckedChanged(object sender, EventArgs e)
        {
            if(hwmipmap.Checked == true)
            {
                MessageBox.Show("Please be aware this experimental hack is not perfect and may even crash your emulator. Also, this hack is only available to latest PCSX2 development builds.", "USE AT YOUR OWN RISK!");
            }
        }
    }
}
