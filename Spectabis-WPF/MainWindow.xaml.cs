using MahApps.Metro.Controls;
using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace Spectabis_WPF
{
    public partial class MainWindow : MetroWindow
    {

        public string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        public MainWindow()
        {
            InitializeComponent();

            //Saves settings between versions
            Properties.Settings.Default.Upgrade();


            //Version
            Debug.WriteLine(Assembly.GetExecutingAssembly().GetName().Version);
            Title = "Spectabis " + Assembly.GetExecutingAssembly().GetName().Version;

            //Advanced options ini
            if (File.Exists(BaseDirectory + @"\advanced.ini"))
            {

                //Read values
                var advancedIni = new IniFile(BaseDirectory + @"\advanced.ini");
                int _framerate = Convert.ToInt16(advancedIni.Read("timelineFramerate", "Renderer"));

                //Timeline Framerate
                Timeline.DesiredFrameRateProperty.OverrideMetadata(typeof(Timeline), new FrameworkPropertyMetadata { DefaultValue = _framerate });
            }


            //Sets nightmode from variable
            new PaletteHelper().SetLightDark(Properties.Settings.Default.nightMode);

            //If emuDir is not set, launch first time setup
            if (Properties.Settings.Default.emuDir == "null")
            {
                Frame FirstSetup = new Frame();
                FirstSetup.Source = new Uri("FirstTimeSetup.xaml", UriKind.Relative);
            }



        }

        //Shows & hides overlay, when appropriate
        private void MenuToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            sideMenu.Visibility = Visibility.Visible;
            Overlay(true);
        }

        private void MenuToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            sideMenu.Visibility = Visibility.Collapsed;
            Overlay(false);
        }

        //Show or hide black overlay
        public void Overlay(bool _show)
        {
            if (_show == true)
            {
                overlay.Opacity = .5;
                overlay.IsEnabled = true;
                overlay.IsHitTestVisible = true;
            }
            else
            {
                overlay.Opacity = 0;
                overlay.IsEnabled = false;
                overlay.IsHitTestVisible = false;
                MenuToggleButton.IsChecked = false;
            }
        }

        //Menu - Library Button
        private void Menu_Library_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Source = new Uri("Library.xaml", UriKind.Relative);
            MainWindow_Header.Text = "Library";
            Overlay(false);

        }

        //Menu - Settings Button
        private void Menu_Settings_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Source = new Uri("Settings.xaml", UriKind.Relative);
            MainWindow_Header.Text = "Settings";
            Overlay(false);
        }

        private void Menu_Credits_Click(object sender, RoutedEventArgs e)
        {

        }

        public void Open_AddGame()
        {
            mainFrame.Source = new Uri("AddGame.xaml", UriKind.Relative);
            MainWindow_Header.Text = "Add Game";
        }

        public void Open_Library()
        {
            mainFrame.Source = new Uri("Library.xaml", UriKind.Relative);
            MainWindow_Header.Text = "Library";
        }

        //Open settings sidewindow
        //Bool true, to show - false to hide
        public void Open_Settings(bool e, [Optional] string _name)
        {
            if(e == true)
            {
                string _cfgDir = BaseDirectory + @"\resources\configs\" + _name;

                //Reads the values from Spectabis ini
                var gameIni = new IniFile(_cfgDir + @"\spectabis.ini");
                var _nogui = gameIni.Read("nogui", "Spectabis");
                var _fullscreen = gameIni.Read("fullscreen", "Spectabis");
                var _fullboot = gameIni.Read("fullboot", "Spectabis");
                var _nohacks = gameIni.Read("nohacks", "Spectabis");
                var _isodir = gameIni.Read("isoDirectory", "Spectabis");

                //Sets the values from spectabis ini
                if (_nogui == "1") { nogui.IsChecked = true; } else { nogui.IsChecked = false; }
                if (_fullscreen == "1") { fullscreen.IsChecked = true; } else {fullscreen.IsChecked = false; }
                if (_fullboot == "1") { fullboot.IsChecked = true; } else { fullboot.IsChecked = false; }
                if (_nohacks == "1") { nohacks.IsChecked = true; } else { nohacks.IsChecked = false; }

                //Reads PCSX2_vm file
                var vmIni = new IniFile(_cfgDir + @"\PCSX2_vm.ini");
                var _widescreen = vmIni.Read("EnableWideScreenPatches", "EmuCore");

                //Sets the values from PCSX2_vm ini
                if (_widescreen == "enabled") { widescreen.IsChecked = true; } else { widescreen.IsChecked = false; }

                //GDSX file mipmap hack
                var gsdxIni = new IniFile(_cfgDir + @"\GSdx.ini");
                var _mipmaphack = gsdxIni.Read("UserHacks_mipmap", "Settings");
                if (_mipmaphack == "1") { hwmipmap.IsChecked = true; } else { hwmipmap.IsChecked = false; }


                //Reads the PCSX2_ui ini file
                var uiIni = new IniFile(_cfgDir + @"\PCSX2_ui.ini");
                var _zoom = uiIni.Read("Zoom", "GSWindow");
                var _aspectratio = uiIni.Read("AspectRatio", "GSWindow");

                //Read aspect ratio
                //Create a list of all the aspect ratios and add them to aspectratio combobox
                List<string> aspectRatios = new List<string>();
                aspectRatios.Add("Letterbox");
                aspectRatios.Add("Widescreen");
                aspectRatios.Add("Stretched");
                aspectratio.ItemsSource = aspectRatios;

                if (_aspectratio == "4:3")
                {
                    aspectratio.SelectedIndex = 0;
                }
                else if (_aspectratio == "16:9")
                {
                    aspectratio.SelectedIndex = 1;
                }
                else
                {
                    aspectratio.SelectedIndex = 2;
                }

                //Set zoom level to textbox
                zoom.Text = _zoom;


                //Show the panel and overlay
                Overlay(true);
                GameSettings.Opacity = 1;
                GameSettings.IsHitTestVisible = true;

                //Set image and header text for the game
                Header_title.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_name);
                GameSettings_Header.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(BaseDirectory + @"\resources\configs\" + _name + @"\art.jpg"));
            }
            else
            {
                //Hide panel
                Overlay(false);
                GameSettings.Opacity = 0;
                GameSettings.IsHitTestVisible = false;
            }
        }

        //Close Game Settings button click
        private void CloseSettings_Button(object sender, RoutedEventArgs e)
        {
            //Save settings, take name from header text
            SaveGameSettings(Header_title.Text);

            //Hide panel
            Overlay(false);
            GameSettings.Opacity = 0;
            GameSettings.IsHitTestVisible = false;
        }

        //Change boxart
        private void ChangeArt_Button(object sender, RoutedEventArgs e)
        {
            //
        }

        private void SaveGameSettings(string _name)
        {
            //Create instances for every ini file to save
            var gameIni = new IniFile(AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + _name + @"\spectabis.ini");
            var uiIni = new IniFile(AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + _name + @"\PCSX2_ui.ini");
            var vmIni = new IniFile(AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + _name + @"\PCSX2_vm.ini");
            var gsdxIni = new IniFile(AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + _name + @"\GSdx.ini");

            //Emulation Settings - written to spectabis ini
            if (nogui.IsChecked == true)
            {
                gameIni.Write("nogui", "1", "Spectabis");
            }
            else
            {
                gameIni.Write("nogui", "0", "Spectabis");
            }

            if (fullscreen.IsChecked == true)
            {
                gameIni.Write("fullscreen", "1", "Spectabis");
            }
            else
            {
                gameIni.Write("fullscreen", "0", "Spectabis");
            }

            if (fullboot.IsChecked == true)
            {
                gameIni.Write("fullboot", "1", "Spectabis");
            }
            else
            {
                gameIni.Write("fullboot", "0", "Spectabis");
            }

            if (nohacks.IsChecked == true)
            {
                gameIni.Write("nohacks", "1", "Spectabis");
            }
            else
            {
                gameIni.Write("nohacks", "0", "Spectabis");
            }

            //Widescreen patch - written to pcsx2_vm
            if (widescreen.IsChecked == true)
            {
                vmIni.Write("EnableWideScreenPatches", "enabled", "EmuCore");
            }
            else
            {
                vmIni.Write("EnableWideScreenPatches", "disabled", "EmuCore");
            }

            //Mipmap hack - written to gsdx.ini
            if (hwmipmap.IsChecked == true)
            {
                gsdxIni.Write("UserHacks_mipmap", "1", "Settings");
                gsdxIni.Write("UserHacks", "1", "Settings");
            }
            else
            {
                gsdxIni.Write("UserHacks_mipmap", "0", "Settings");
            }

            //Aspect ratio - written to PCSX2_ui ini
            if(aspectratio.SelectedIndex == 0)
            {
                uiIni.Write("AspectRatio", "4:3", "GSWindow");
            }
            else if(aspectratio.SelectedIndex == 1)
            {
                uiIni.Write("AspectRatio", "16:9", "GSWindow");
            }
            else
            {
                uiIni.Write("AspectRatio", "Stretch", "GSWindow");
            }

            //Zoom level - writeen to PCSX2-ui ini
            uiIni.Write("Zoom", zoom.Text, "GSWindow");
        }

        //Search PCSX2 wiki button
        private void SearchWiki_Button(object sender, RoutedEventArgs e)
        {
            //Take the header title and replace spaces with + sign
            string _query = Header_title.Text;
            _query = _query.Replace(" ", "+");

            //Open up PCSX2 wiki
            Process.Start(@"http://wiki.pcsx2.net/index.php?search=" + _query);
        }
    }
}