using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Cache;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Animation;
using MahApps.Metro.Controls;
using MaterialDesignThemes.Wpf;
using Spectabis_WPF.Domain;
using System.Windows.Threading;
using System.Diagnostics;
using System.Threading;

namespace Spectabis_WPF.Views
{
    public partial class MainWindow : MetroWindow
    {
        public static string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;
    
        //Side panel width value
        public static readonly double PanelWidth = 700;

        //Stopwatch to keep track of playtime
        private Stopwatch SessionPlaytime = new Stopwatch();
        private DispatcherTimer updatePlaytimeUI = new DispatcherTimer();

        public DiscordRPC DiscordRpc;

        public MainWindow()
        {
            InitializeComponent();

            //Create resources folder
            Directory.CreateDirectory($"{BaseDirectory}//resources//_temp");

            //Saves settings between versions
            Properties.Settings.Default.Upgrade();

            CatchCommandLineArguments();

            updatePlaytimeUI.Tick += updatePlaytimeUI_Tick;

            //Version
            Console.WriteLine(Assembly.GetExecutingAssembly().GetName().Version);

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
                FirstSetupFrame.Visibility = Visibility.Visible;
            }

            SetPrimary(Properties.Settings.Default.swatch);

			//Copy spinner.gif to temporary files
			{
				var dir = BaseDirectory + "\\resources\\_temp";
				if (Directory.Exists(dir) == false)
					Directory.CreateDirectory(dir);
				Properties.Resources.spinner.Save(dir + "\\spinner.gif");
			}

            //Open game library page
            mainFrame.Source = new Uri("Library.xaml", UriKind.Relative);

            GameSettings.Width = PanelWidth;

            //Check if it's april fool's day
            if ((DateTime.Now.Month == 4) && (DateTime.Now.Day == 1) && (Properties.Settings.Default.aprilfooled == false))
            {
                AprilFools_Grid.Visibility = Visibility.Visible;
            }

            DiscordRpc = new DiscordRPC();
            DiscordRpc.UpdatePresence("Menus");
        }

        //DLLs for console window
        [DllImport("Kernel32")]
        private static extern void AllocConsole();
        [DllImport("Kernel32")]
        private static extern bool FreeConsole();

        //Set primary color scheme
        private void SetPrimary(string swatch)
        {
            Console.WriteLine("Setting PrimaryColor to " + swatch);
            new PaletteHelper().ReplacePrimaryColor(swatch);
        }

        private void CatchCommandLineArguments()
        {
            //Make alist of all arguments
            List<string> arguments = new List<string>(Environment.GetCommandLineArgs());
            bool findProfile = false;

            foreach(string arg in arguments)
            {
                //Open Console Window
                if (arg == "-console")
                {
                    AllocConsole();
                    Thread.Sleep(10);
                    Console.WriteLine("Opening debug console");
                    Console.WriteLine($"Current Build: {Assembly.GetExecutingAssembly().GetName().Version}");
                }
                //Force first time setup
                else if (arg == "-firsttime")
                {
                    Console.WriteLine("Forcing first time setup");
                    FirstSetupFrame.Visibility = Visibility.Visible;
                }
                //Launch a game profile
                else if(arg == "-profile")
                {
                    Console.WriteLine("Looking for a game profile to launch...");
                    findProfile = true;
                }
                //If profile needs to be found, then do that
                else if(findProfile == true)
                {
                    //If there's a game profile with given argument
                    if (Directory.Exists($"{BaseDirectory}resources\\configs\\{arg}"))
                    {
                        //A given game name is valid
                        Console.WriteLine("Launching " + arg);

                        //Launch game
                        Domain.LaunchPCSX2.LaunchGame(arg);
                    }
                    else
                    {
                        //If game profile does not exist, then don't look for another one
                        Console.WriteLine("Not a valid game profile name!");
                        findProfile = false;
                    }
                }
            }
        }

        //Hides first time setup frame
        public void HideFirsttimeSetup()
        {
            FirstSetupFrame.Visibility = Visibility.Collapsed;
        }

        //Shows & hides overlay, when appropriate
        private void MenuToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            if(GameSettings.Visibility == Visibility.Visible)
            {
                MenuToggleButton.IsChecked = false;
                return;
            }
            sideMenu.Visibility = Visibility.Visible;
            Overlay(true);
        }

        private void MenuToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            if (GameSettings.Visibility == Visibility.Visible)
            {
                MenuToggleButton.IsChecked = false;
                return;
            }
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

            Console.WriteLine(this.Width + " x " + this.Height);

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
            mainFrame.Source = new Uri("Credits.xaml", UriKind.Relative);
            MainWindow_Header.Text = "Credits";
            Overlay(false);
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

        public List<string> NewGamesInDirectory;

        public void Open_GameDiscovery()
        {
            NewGamesInDirectory = ((Library)mainFrame.Content).NewGamesInDirectory;

            mainFrame.Source = new Uri("GameDiscovery.xaml", UriKind.Relative);
            MainWindow_Header.Text = "Game Discovery";
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
                var _shaderfx = gsdxIni.Read("shaderfx", "Settings");

                if (_mipmaphack == "1") { hwmipmap.IsChecked = true; } else { hwmipmap.IsChecked = false; }
                if (_shaderfx == "1") { Shader_Checkbox.IsChecked = true; Shader_Button.IsEnabled = true; } else { Shader_Checkbox.IsChecked = false; Shader_Button.IsEnabled = false; }


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

                //Disable Input settings button if "Global Controller Profile" is enabled
                if(Properties.Settings.Default.globalController)
                {
                    InputSettings_Button.IsEnabled = false;
                }
                else
                {
                    InputSettings_Button.IsEnabled = true;
                }

                //Show the panel and overlay
                Overlay(true);
                GameSettings.Visibility = Visibility.Visible;
                SlideInPanelAnimation();

                //Set image and header text for the game
                Header_title.Text = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_name);

                //Creates a bitmap stream
                var artSource = new System.Windows.Media.Imaging.BitmapImage();

                artSource.BeginInit();

                //Fixes the caching issues, where cached copy would just hang around and bother me for two days
                artSource.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.None;
                artSource.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
                artSource.CreateOptions = System.Windows.Media.Imaging.BitmapCreateOptions.IgnoreImageCache;

                artSource.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                artSource.UriSource = new Uri(@"resources\configs\" + _name + @"\art.jpg", UriKind.RelativeOrAbsolute);

                artSource.EndInit();
                GameSettings_Header.Source = artSource;
            }
            else
            {
                //Hide panel
                Overlay(false);
                SlideOutPanelAnimation();
            }
        }

        public static readonly Duration PanelSlideTime = TimeSpan.FromMilliseconds(120);

        //Side panel sliding in animation, must be triggered after visiblity change
        public void SlideInPanelAnimation()
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = PanelWidth;
            da.Duration = PanelSlideTime;
            GameSettings.BeginAnimation(System.Windows.Controls.Grid.WidthProperty, da);
            
        }

        //Side Panel sliding out animation
        public void SlideOutPanelAnimation()
        {
            DoubleAnimation da = new DoubleAnimation();
            da.Completed += new EventHandler(SlideOutPanelAnimation_Finished);
            da.From = PanelWidth;
            da.To = 0;
            da.Duration = PanelSlideTime;
            GameSettings.BeginAnimation(System.Windows.Controls.Grid.WidthProperty, da);
        }

        //When sliding out animated has finished, hide the panel, because things rely on the panel's visiblity
        private void SlideOutPanelAnimation_Finished(object sender, EventArgs e)
        {
            GameSettings.Visibility = Visibility.Collapsed;
        }

        //Close Game Settings button click
        private void CloseSettings_Button(object sender, RoutedEventArgs e)
        {
            //Save settings, take name from header text
            SaveGameSettings(Header_title.Text);

            //Hide panel
            Overlay(false);
            Open_Settings(false);
        }

        //Change boxart
        private void ChangeArt_Button(object sender, RoutedEventArgs e)
        {
            Ookii.Dialogs.Wpf.VistaOpenFileDialog artBrowser = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();
            artBrowser.Filter = "JPEG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png";
            artBrowser.Title = "Browse to a new boxart image";

            var browserResult = artBrowser.ShowDialog();
            if(browserResult == true)
            {
                string _file = artBrowser.FileName;
                string _game = Header_title.Text;

                //Url files don't get filtered, so let's just not break the game profile and stop, if 
                //selected file is indeed a url file
                if(_file.Contains(".url"))
                {
                    Console.WriteLine("File was URL, returning.");
                    return;
                }

                //Replace the boxart image
                //refreshTile(null);
                GameSettings_Header.Source = null;
                File.Copy(_file, BaseDirectory + @"\resources\configs\" + _game + @"\art.jpg", true);

                //Reload the image in header
                var artSource = new System.Windows.Media.Imaging.BitmapImage();
                artSource.BeginInit();
                artSource.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.None;
                artSource.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
                artSource.CreateOptions = System.Windows.Media.Imaging.BitmapCreateOptions.IgnoreImageCache;

                artSource.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                artSource.UriSource = new Uri(BaseDirectory + @"\resources\configs\" + _game + @"\art.jpg");
                artSource.EndInit();
                GameSettings_Header.Source = artSource;

                refreshTile(_game);
            }
        }

        public void reloadLibrary()
        {
            mainFrame.NavigationService.Refresh();
        }

        public void refreshTile(string game)
        {
            this.Invoke(new Action(() => ((Library)mainFrame.Content).refreshTile(game)));
        }

        private void SaveGameSettings(string _name)
        {
            //Create instances for every ini file to save
            var gameIni = new IniFile(BaseDirectory + @"\resources\configs\" + _name + @"\spectabis.ini");
            var uiIni = new IniFile(BaseDirectory + @"\resources\configs\" + _name + @"\PCSX2_ui.ini");
            var vmIni = new IniFile(BaseDirectory + @"\resources\configs\" + _name + @"\PCSX2_vm.ini");
            var gsdxIni = new IniFile(BaseDirectory + @"\resources\configs\" + _name + @"\GSdx.ini");

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

            //Shader status - written to gsdx.ini
            if (Shader_Checkbox.IsChecked == true)
            {
                gsdxIni.Write("shaderfx", "1", "Settings");
                CopyShaders(_name);
                WriteGSdxFX(_name);
            }
            else
            {
                gsdxIni.Write("shaderfx", "0", "Settings");
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
            _query = _query.Replace(" - ", ":+");
            _query = _query.Replace(" ", "+");
            _query = _query.Replace("++",":+");

            //Open up PCSX2 wiki
            Process.Start(@"http://wiki.pcsx2.net/index.php?search=" + _query);
        }

        //shader config button
        private void ShaderButton_Click(object sender, RoutedEventArgs e)
        {
            string currentGame = Header_title.Text;
            string GSdxfx = BaseDirectory + @"resources\configs\" + currentGame + @"\GSdx.fx";
            string GSdxfxini = BaseDirectory + @"resources\configs\" + currentGame + @"\GSdx_FX_Settings.ini";

            if (File.Exists(BaseDirectory + @"resources\configs\" + currentGame + @"\GSdx.fx") == false)
            {
                CopyShaders(currentGame);
            }

            WriteGSdxFX(currentGame);

            //open shader config file
            Process.Start(GSdxfxini);
        }

        //Disable & enable configure shader button according to checkbox
        private void Shader_Checkbox_Click(object sender, RoutedEventArgs e)
        {
            if(Shader_Checkbox.IsChecked == true)
            {
                Shader_Button.IsEnabled = true;
            }
            else
            {
                Shader_Button.IsEnabled = false;
            }
        }

        //copy shader files from emulator directory to game profile
        private void CopyShaders(string currentGame)
        {
            string GSdxfx = BaseDirectory + @"resources\configs\" + currentGame + @"\GSdx.fx";
            string GSdxfxini = BaseDirectory + @"resources\configs\" + currentGame + @"\GSdx_FX_Settings.ini";

            if(File.Exists(GSdxfx) == false)
            {
                var dir = Path.GetDirectoryName(Properties.Settings.Default.emuDir);
                File.Copy(dir + @"\shaders\GSdx.fx", GSdxfx);
                File.Copy(dir + @"\shaders\GSdx_FX_Settings.ini", GSdxfxini);
            }
        }

        //Write shader file locations to GSdx ini
        private void WriteGSdxFX(string currentGame)
        {
            string GSdxfx = BaseDirectory + @"resources\configs\" + currentGame + @"\GSdx.fx";
            string GSdxfxini = BaseDirectory + @"resources\configs\" + currentGame + @"\GSdx_FX_Settings.ini";

            var GSdx = new IniFile(BaseDirectory + @"\resources\configs\" + currentGame + @"\GSdx.ini");
            GSdx.Write("shaderfx_glsl", GSdxfx, "Settings");
            GSdx.Write("shaderfx_conf", GSdxfxini, "Settings");
            Console.WriteLine("Shader files written to GSdx.ini");
        }

        //Imports GPUconfigure from GSdx plugin
        //All GSdx plugins have same settings, by the looks of it
        //It has no inputs, but writes/reads the ini files where .exe is located at folder /inis/
        [DllImport(@"\plugins\GSdx32-SSE2.dll")]
        static private extern void GSconfigure();

        //Configuration must be closed so .dll is not in use
        [DllImport(@"\plugins\GSdx32-SSE2.dll")]
        static private extern void GSclose();

        //Video Settings button
        private void VideoSettings_click(object sender, RoutedEventArgs e)
        {
            //Set currentgame from header title text
            string currentGame = Header_title.Text;

            if (File.Exists(BaseDirectory + @"\resources\configs\" + currentGame + @"\GSdx.ini"))
            {
                //Creates inis folder and copies it from game profile folder
                Directory.CreateDirectory(BaseDirectory + @"inis");
                File.Copy(BaseDirectory + @"\resources\configs\" + currentGame + @"\GSdx.ini", BaseDirectory + @"inis\GSdx.ini", true);
            }
            else
            {
                Directory.CreateDirectory(BaseDirectory + @"inis");
                File.Create(BaseDirectory + @"inis\GSdx.ini");
            }

            //GPUConfigure(); - Only software mode was available
            GSconfigure();
            GSclose();

            File.Copy(BaseDirectory + @"inis\GSdx.ini", BaseDirectory + @"\resources\configs\" + currentGame + @"\GSdx.ini", true);
            Directory.Delete(BaseDirectory + @"inis", true);
        }


        [DllImport(@"\plugins\Spu2-X.dll")]
        static private extern void SPU2configure();

        [DllImport(@"\plugins\Spu2-X.dll")]
        static private extern void SPU2close();

        //Audio Settings button
        private void AudioSettings_click(object sender, RoutedEventArgs e)
        {
            string currentGame = Header_title.Text;

            if (File.Exists(BaseDirectory + @"\resources\configs\" + currentGame + @"\SPU2-X.ini"))
            {
                //Creates inis folder and copies it from game profile folder
                Directory.CreateDirectory(BaseDirectory + @"inis");
                File.Copy(BaseDirectory + @"\resources\configs\" + currentGame + @"\SPU2-X.ini", BaseDirectory + @"inis\SPU2-X.ini", true);
            }
            else
            {
                Directory.CreateDirectory(BaseDirectory + @"inis");
                File.Create(BaseDirectory + @"inis\SPU2-X.ini");
            }

            SPU2configure();
            SPU2close();

            File.Copy(BaseDirectory + @"inis\SPU2-X.ini", BaseDirectory + @"\resources\configs\" + currentGame + @"\SPU2-X.ini", true);
            Directory.Delete(BaseDirectory + @"inis", true);
        }


        [DllImport(@"\plugins\LilyPad.dll")]
        static private extern void PADconfigure();

        //Configuration must be closed so .dll is not in use
        [DllImport(@"\plugins\LilyPad.dll")]
        static private extern void PADclose();

        private void InputSettings_click(object sender, RoutedEventArgs e)
        {
            string currentGame = Header_title.Text;

            //Copy the existing .ini file for editing if it exists
            if (File.Exists(BaseDirectory + @"\resources\configs\" + currentGame + @"\LilyPad.ini"))
            {
                //Creates inis folder and copies it from game profile folder
                Directory.CreateDirectory(BaseDirectory + @"inis");
                File.Copy(BaseDirectory + @"\resources\configs\" + currentGame + @"\LilyPad.ini", BaseDirectory + @"inis\LilyPad.ini", true);
            }
            else
            {
                Directory.CreateDirectory(BaseDirectory + @"inis");
                File.Create(BaseDirectory + @"inis\LilyPad.ini");
            }

            Console.WriteLine("Loading " + BaseDirectory + @"\resources\configs\" + currentGame + @"\LilyPad.ini");

            //Calls the DLL configuration function
            PADconfigure();

            //Calls the configration close function
            PADclose();

            //Copies the modified file into the game profile & deletes the created folder
            File.Copy(BaseDirectory + @"inis\LilyPad.ini", BaseDirectory + @"\resources\configs\" + currentGame + @"\LilyPad.ini", true);
            Directory.Delete(BaseDirectory + @"inis", true);

            Console.WriteLine("Saving to " + BaseDirectory + @"\resources\configs\" + currentGame + @"\LilyPad.ini");
        }

        //Public timer, because it needs to stop itself
        public DispatcherTimer timeTimer = new DispatcherTimer();

        //Because labels don't support Click, i'll have to do it on my own
        private void Header_title_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Console.WriteLine("Clicked on game header!");

            int TimeBetweenClicks = System.Windows.Forms.SystemInformation.DoubleClickTime;

            //If the timer is not yet stopped, that means it's a double click
            //If the timer is not running, it's not a double click
            if (timeTimer.IsEnabled)
            {
                //Hide the real title label
                Header_title.Visibility = Visibility.Collapsed;

                //Set text from label and make the textbox visible
                TitleEditBox.Text = Header_title.Text;
                TitleEditBox.Visibility = Visibility.Visible;

                //Set focus to this textbox
                TitleEditBox.Focus();
                TitleEditBox.CaretIndex = TitleEditBox.Text.Length;
            }
            else
            {
                //Starts the timer, with system double click time as interval (500ms for me)
                timeTimer.Interval = new TimeSpan(0, 0, 0, 0, TimeBetweenClicks);
                timeTimer.Tick += timeTimer_Tick;
                timeTimer.Start();

                Console.WriteLine("Started timer - after ms" + TimeBetweenClicks);
            }
        }

        //Stop double-click timer on first tick
        private void timeTimer_Tick(object sender, EventArgs e)
        {
            Console.WriteLine("Tick!");
            timeTimer.Stop();
        }

        //When created textbox loeses focus, show the real label and don't save changes
        private void TitleEditBox_LostFocus(object sender, RoutedEventArgs e)
        {
            //Hide the textbox
            TitleEditBox.Visibility = Visibility.Collapsed;

            //Show real label
            Header_title.Visibility = Visibility.Visible;
        }

        //Catch created textbox key presses
        private void TitleEditBox_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            List<string> chars = new List<string>();
            //Remove unsupported characters
            {
                var tempText = TitleEditBox.Text;
                foreach (var illegal in new[] { "/", "\\", ":", "|", "*", "<", ">" })
                    tempText = tempText.Replace(illegal, string.Empty);
                TitleEditBox.Text = tempText;
            }

            Console.WriteLine(e.Key.ToString());

            //This is the only way to save the name
            if (e.Key == Key.Enter)
            {

                //Save old name to a variable
                string _oldName = Header_title.Text;
                string _newName;

                //Hide the textbox
                TitleEditBox.Visibility = Visibility.Collapsed;

                //Pass text to label
                Header_title.Text = TitleEditBox.Text;

                //Show real label
                Header_title.Visibility = Visibility.Visible;

                //Save new name to the variable
                _newName = Header_title.Text;

                //Check, if old directory exists
                if(Directory.Exists(BaseDirectory + @"\resources\configs\" + _oldName))
                {
                    try
                    {
                        //Move old folder to new folder
                        GameProfile.Rename(_oldName, _newName);
                        
                        //Rename game tile
                        renameTile(_oldName, _newName);

                        Console.WriteLine($"Renamed profile '{_oldName}' to '{_newName}'");
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine("Couldn't rename the folder");
                        Console.WriteLine(ex);
                    }
                }
            }

            if (e.Key == Key.Escape)
            {
                //Hide the textbox
                TitleEditBox.Visibility = Visibility.Collapsed;

                //Show real label
                Header_title.Visibility = Visibility.Visible;
            }
        }

        private void renameTile(string _old, string _new)
        {
            this.Invoke(new Action(() => ((Library)mainFrame.Content).renameTile(_old, _new)));
        }

        private void Menu_Themes_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Source = new Uri("Themes.xaml", UriKind.Relative);
            MainWindow_Header.Text = "Color Themes";
            Overlay(false);
        }

        //When Window closes
        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Clear Temp files
            ClearTemp();
        }

        //Clears _temp folder
        private void ClearTemp()
        {
            string[] _tempdir = Directory.GetFiles(BaseDirectory + @"\resources\_temp\");
            foreach(var file in _tempdir)
            {
                File.Delete(file);
            }
        }

        //Minimize Spectabis
        public void MainWindow_Minimize()
        {
            this.Invoke(new Action(() => WindowState = WindowState.Minimized));
        }

        //Bring Spectabis to front
        public void MainWindow_Maximize()
        {
            this.Invoke(new Action(() => WindowState = WindowState.Normal));
        }

        public void SetRunningGame(string e)
        {
            RunningGame.Text = e;
            CurrentGame = e;
        }

        public string CurrentGame = null;

        //Block Spectabis while PCSX2 is running
        public void BlockInput(bool e)
        {
            if(e)
            {
                //Spectabis input is blocked and game is running
                Block.Visibility = Visibility.Visible;

                //Start Playtime session tracker
                SessionPlaytime.Reset();
                SessionPlaytime.Start();

                //Timer that updates playtime in UI
                updatePlaytimeUI.Interval = TimeSpan.FromSeconds(60);
                updatePlaytimeUI.Start();

                DiscordRpc.UpdatePresence(CurrentGame);

            }
            else
            {
                //Default Rich Presence
                DiscordRpc.UpdatePresence("Menus");

                //Input is not blocked and game is not running
                Block.Visibility = Visibility.Collapsed;

                //Stop playtime session tracker
                if(SessionPlaytime.IsRunning)
                {
                    SessionPlaytime.Stop();
                    updatePlaytimeUI.Stop();
                    Console.WriteLine("Session Lenght: " + SessionPlaytime.Elapsed.Minutes);
                    Console.WriteLine($"SessionTimer Working: {SessionPlaytime.IsRunning}");
                }
            }
        }

        //Update session timer in UI
        private void updatePlaytimeUI_Tick(object sender, EventArgs e)
        {
            if(SessionPlaytime.Elapsed.Minutes == 1)
            {
                this.Invoke(new Action(() => SessionLenght.Text = $"Current Session: {SessionPlaytime.Elapsed.Minutes} minute"));
            }
            else
            {
                this.Invoke(new Action(() => SessionLenght.Text = $"Current Session: {SessionPlaytime.Elapsed.Minutes} minutes"));
            }

            //As this timer updates every minute, playtime in file gets updated also
            Playtime.AddPlaytime(CurrentGame, TimeSpan.FromMinutes(1));

            Console.WriteLine($"Updating UI with TimerTimer: {SessionPlaytime.Elapsed} minutes, adding 1 minute to file");
        }


        //Force Stop PCSX2 button
        private void ForceStop_Click(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(new Action(() => ((Library)mainFrame.Content).ForceStop()));
        }

        private void AprilFools_Button(object sender, RoutedEventArgs e)
        {
            //Fade out
            DoubleAnimation da = new DoubleAnimation();
            da.From = 1;
            da.To = 0;
            da.Duration = TimeSpan.FromSeconds(1.5);
            AprilFools_Grid.BeginAnimation(System.Windows.Controls.Grid.OpacityProperty, da);

            AprilFools_Grid.IsHitTestVisible = false;

            Dispatcher.Invoke(new Action(() => ((Library)mainFrame.Content).PushSnackbar("Happy April Fools' Day! Pre-order 'Horse Armor DLC' now! ")));

            //Don't show this message again
            Properties.Settings.Default.aprilfooled = true;
            Properties.Settings.Default.Save();
        }
    }
}