using SevenZip;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Spectabis_WPF
{
    public partial class Library : Page
    {

        //Spectabis Variables
        public string emuDir = Properties.Settings.Default.emuDir;
        public string GameConfigs;
        public string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory;

        //Temporary variable for rightclick funcionality
        public Image clickedBoxArt;

        //Lists
        public List<string> regionList = new List<string>();
        public List<string> supportedGameFiles = new List<string>();
        public List<string> supportedScrappingFiles = new List<string>();

        public Library()
        {
            InitializeComponent();

            //Where game profile folders are saved
            GameConfigs = BaseDirectory + @"\resources\configs\";

            //Adds supported game image files to a list
            supportedGameFiles.Add("iso");
            supportedGameFiles.Add("bin");
            supportedGameFiles.Add("cso");
            supportedGameFiles.Add("gz");

            //Adds supported files for artScrapping to a list
            supportedScrappingFiles.Add("iso");

            //Adds known items to region a list
            regionList.Add("SLUS");
            regionList.Add("SCUS");
            regionList.Add("SCES");
            regionList.Add("SLES");
            regionList.Add("SCPS");
            regionList.Add("SLPS");
            regionList.Add("SLPM");
            regionList.Add("PSRM");
            regionList.Add("SCED");
            regionList.Add("SLPM");
            regionList.Add("SIPS");


            reloadGames();
        }


        //MouseDown event on boxArt image
        private void boxArt_Click(object sender, MouseButtonEventArgs e)
        {
            clickedBoxArt = (Image)sender;
            Debug.WriteLine(Convert.ToString(clickedBoxArt.Tag) + " - clicked");

            //Get isoDir from Spectabis.ini
            //string _cfgDir = BaseDirectory + @"resources\configs\" + clickedBoxArt.Tag;
            string _cfgDir = GameConfigs + @"/" + clickedBoxArt.Tag;

            var _gameIni = new IniFile(_cfgDir + @"\spectabis.ini");
            var _isoDir = _gameIni.Read("isoDirectory", "Spectabis");

            //Save needed click count to variable
            int _ClickCount;
            if(Properties.Settings.Default.doubleClick == true)
            {
                _ClickCount = 2;
            }
            else
            {
                _ClickCount = 1;
            }

            //If right click
            if(e.XButton1 == e.RightButton)
            {
                //Checks for click count
                if(e.ClickCount == _ClickCount)
                {
                    if(File.Exists(_isoDir))
                    {
                        //If game file exists, launch

                        //Launch arguments
                        var _nogui = _gameIni.Read("nogui", "Spectabis");
                        var _fullscreen = _gameIni.Read("fullscreen", "Spectabis");
                        var _fullboot = _gameIni.Read("fullboot", "Spectabis");
                        var _nohacks = _gameIni.Read("nohacks", "Spectabis");

                        string _launchargs = "";

                        if (_nogui == "1") { _launchargs = "--nogui "; }
                        if (_fullscreen == "1") { _launchargs = _launchargs + "--fullscreen "; }
                        if (_fullboot == "1") { _launchargs = _launchargs + "--fullboot "; }
                        if (_nohacks == "1") { _launchargs = _launchargs + "--nohacks "; }

                        Debug.WriteLine(clickedBoxArt.Tag + " launched with commandlines:  " + _launchargs);
                        Debug.WriteLine(clickedBoxArt.Tag + " launched from: " + _isoDir);
                        Debug.WriteLine(emuDir + @"\pcsx2.exe", "" + _launchargs + "\"" + _isoDir + "\" --cfgpath \"" + _cfgDir + "\"");

                        Process.Start(emuDir + @"\pcsx2.exe", "" + _launchargs + "\"" + _isoDir + "\" --cfgpath \"" + _cfgDir + "\"");

                    }
                    else
                    {
                        Debug.WriteLine(_isoDir + " does not exist!");
                    }
                }
            }

            //If left click
            if(e.XButton1 == e.LeftButton)
            {
                //Creates a ContextMenu
                ContextMenu gameContext = new ContextMenu();

                //Emulator Settings menu button
                MenuItem PCSX2config = new MenuItem();
                PCSX2config.Header = "Configure in PCSX2";
                PCSX2config.Click += PCSX2ConfigureGame_Click;

                //Spectabis Config menu button
                MenuItem SpectabisConfig = new MenuItem();
                SpectabisConfig.Header = "Game Configuration";
                SpectabisConfig.Click += SpectabisConfig_Click;

                //Remove game menu button
                MenuItem RemoveGame = new MenuItem();
                RemoveGame.Header = "Remove Game";
                RemoveGame.Click += RemoveGame_Click;
               
                //Add buttons to context menu
                gameContext.Items.Add(SpectabisConfig);
                gameContext.Items.Add(PCSX2config);
                gameContext.Items.Add(RemoveGame);

                //Open context menu
                gameContext.IsOpen = true;
            }



        }


        //Context Menu PCSX2 button
        private void PCSX2ConfigureGame_Click(object sender, RoutedEventArgs e)
        {
            //Title of the last clicked game
            string _title = Convert.ToString(clickedBoxArt.Tag);

            //Start PCSX2 only with --cfgpath
            string _cfgDir = GameConfigs + @"/" + clickedBoxArt.Tag;
            Process.Start(emuDir + @"\pcsx2.exe", " --cfgpath \"" + _cfgDir + "\"");

        }

        //Context Menu Settings button
        private void SpectabisConfig_Click(object sender, RoutedEventArgs e)
        {
            //Title of the last clicked game
            string _title = Convert.ToString(clickedBoxArt.Tag);
        }

        //Context Menu Remove button
        private void RemoveGame_Click(object sender, RoutedEventArgs e)
        {
            //Title of the last clicked game
            string _title = Convert.ToString(clickedBoxArt.Tag);
            gamePanel.Children.Remove(clickedBoxArt);

            if(Directory.Exists(GameConfigs + @"/" + clickedBoxArt.Tag))
            {
                Directory.Delete(GameConfigs + @"/" + clickedBoxArt.Tag, true);
            }
        }

        //Rescans the game config directory and adds them to gamePanel
        private void reloadGames()
        {
            if (Directory.Exists(GameConfigs))
            {
                //Makes a collection of game folders from game config directory
                string[] _gamesdir = Directory.GetDirectories(GameConfigs);

                //Loops through each folder in game config directory
                foreach (string game in _gamesdir)
                {
                    if (File.Exists(game + @"\Spectabis.ini"))
                    {
                        //Sets _gameName to name of the folder
                        string _gameName = game.Remove(0, game.LastIndexOf(Path.DirectorySeparatorChar) + 1);

                        Debug.WriteLine("adding to gamePanel - " + _gameName);

                        //Creates an image object
                        Image boxArt = new Image();

                        //Creates a bitmap stream
                        System.Windows.Media.Imaging.BitmapImage artSource = new System.Windows.Media.Imaging.BitmapImage();
                        //Opens the filestream
                        artSource.BeginInit();
                        artSource.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                        artSource.UriSource = new Uri(game + @"\art.jpg");
                        //Closes the filestream
                        artSource.EndInit();

                        //sets boxArt source to created bitmap
                        boxArt.Source = artSource;

                        boxArt.Height = 200;
                        boxArt.Width = 150;
                        boxArt.MouseDown += boxArt_Click;
                        boxArt.Tag = _gameName;

                        //Set BitmapScalingMode from advanced.ini if set
                        //Forgive me
                        if (File.Exists(BaseDirectory + @"\advanced.ini"))
                        {
                            var advancedIni = new IniFile(BaseDirectory + @"\advanced.ini");

                            var _BitmapScalingMode = advancedIni.Read("BitmapScalingMode", "Renderer");

                            if(_BitmapScalingMode == "BitmapScalingMode.HighQuality")
                            {
                                RenderOptions.SetBitmapScalingMode(boxArt, BitmapScalingMode.HighQuality);
                            }
                            else if(_BitmapScalingMode == "BitmapScalingMode.LowQuality")
                            {
                                RenderOptions.SetBitmapScalingMode(boxArt, BitmapScalingMode.LowQuality);
                            }
                            else if (_BitmapScalingMode == "BitmapScalingMode.Linear")
                            {
                                RenderOptions.SetBitmapScalingMode(boxArt, BitmapScalingMode.Linear);
                            }
                            else if (_BitmapScalingMode == "BitmapScalingMode.NearestNeighbor")
                            {
                                RenderOptions.SetBitmapScalingMode(boxArt, BitmapScalingMode.NearestNeighbor);
                            }
                            else if (_BitmapScalingMode == "BitmapScalingMode.Fant")
                            {
                                RenderOptions.SetBitmapScalingMode(boxArt, BitmapScalingMode.Fant);
                            }
                        }

                        //If showtitle is selected
                        if (Properties.Settings.Default.showTitle == true)
                        {
                            GroupBox gameTile = new GroupBox();
                            gameTile.Content = boxArt;
                            gameTile.Header = _gameName;
                            gamePanel.Children.Add(gameTile);
                        }
                        else
                        {
                            //Adds the object to gamePanel
                            gamePanel.Children.Add(boxArt);
                        }
                    }
                }
            }

            Directory.CreateDirectory(GameConfigs);

        }

        //Push snackbar function
        public void PushSnackbar(string message)
        {
            var messageQueue = Snackbar.MessageQueue;

            //the message queue can be called from any thread
            Task.Factory.StartNew(() => messageQueue.Enqueue(message));
        }

        //Dragging file effect
        private void Grid_DragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.All;
        }

        //Drag and drop functionality
        private void Grid_Drop(object sender, System.Windows.DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);

            foreach (string file in files)
            {
                //If file is a valid game file
                if(supportedGameFiles.Any(s => file.EndsWith(s)))
                {
                    //If file supports name scrapping
                    if(supportedScrappingFiles.Any(s => file.EndsWith(s)))
                    {
                        string SerialNumber = GetSerialNumber(file);
                        string GameName = GetGameName(SerialNumber);
                        AddGame(null, file, GameName);
                    }
                    else
                    {
                        PushSnackbar("This filetype doesn't support automatic boxart!");
                    }

                    AddGame(null, file, Path.GetFileNameWithoutExtension(file));
                }
                else
                {
                    PushSnackbar("Unsupported file!");
                }
            }

        }

        //Add game method
        //_img = null if no game image
        public void AddGame(string _img, string _isoDir, string _title)
        {
            //sanitize game's title for folder creation
            _title = _title.Replace(@"/", string.Empty);
            _title = _title.Replace(@"\", string.Empty);
            _title = _title.Replace(@":", string.Empty);
            _title = _title.Replace(@"|", string.Empty);
            _title = _title.Replace(@"*", string.Empty);
            _title = _title.Replace(@"<", string.Empty);
            _title = _title.Replace(@">", string.Empty);

            Directory.CreateDirectory(BaseDirectory + @"\resources\configs\" + _title);

            //Copies existing ini files from PCSX2
            //looks for inis in pcsx2 directory
            if (Directory.Exists(emuDir + @"\inis\"))
            {
                string[] inisDir = Directory.GetFiles(emuDir + @"\inis\");
                foreach (string inifile in inisDir)
                {
                    Debug.WriteLine(inifile + " found!");
                    if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + _title + @"\" + Path.GetFileName(inifile)) == false)
                    {
                        string _destinationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + _title + @"\" + Path.GetFileName(inifile));
                        File.Copy(inifile, _destinationPath);
                    }
                }
            }
            else
            {

                //looks for pcsx2 inis in documents folder
                if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\PCSX2\inis"))
                {
                    string[] inisDirDoc = Directory.GetFiles((Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\PCSX2\inis"));
                    foreach (string inifile in inisDirDoc)
                    {
                        if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + _title + @"\" + Path.GetFileName(inifile)) == false)
                        {
                            string _destinationPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + _title + @"\" + Path.GetFileName(inifile));
                            File.Copy(inifile, _destinationPath);
                        }
                    }
                }

                //if no inis are found, warning is shown
                else
                {
                    PushSnackbar("Cannot find default PCSX2 configuration");
                }

            }

            //Create a blank Spectabis.ini file
            var gameIni = new IniFile(BaseDirectory + @"\resources\configs\" + _title + @"\spectabis.ini");
            gameIni.Write("isoDirectory", _isoDir, "Spectabis");
            gameIni.Write("nogui", "0", "Spectabis");
            gameIni.Write("fullscreen", "0", "Spectabis");
            gameIni.Write("fullboot", "0", "Spectabis");
            gameIni.Write("nohacks", "0", "Spectabis");

            //Downloads the image !!
            using (WebClient client = new WebClient())
            {
                try
                {
                    client.DownloadFile(_img, BaseDirectory + @"\resources\configs\" + _title + @"\art.jpg");
                }
                catch
                {
                    Properties.Resources.tempArt.Save(BaseDirectory + @"\resources\configs\" + _title + @"\art.jpg");
                    PushSnackbar("Image not available");
                }
            }

            //Removes all games from list
            gamePanel.Children.Clear();

            //Reloads games
            reloadGames();

        }

        //Get serial number for then given file
        public string GetSerialNumber(string _isoDir)
        {
            string _filename;
            string gameserial = "NULL";

            //Checks, if process is 32-bit or 64
            if (IntPtr.Size == 4)
            {
                //32-bit
                SevenZipBase.SetLibraryPath(BaseDirectory + @"lib\7z-x86.dll");
            }
            else if (IntPtr.Size == 8)
            {
                //64-bit
                SevenZipBase.SetLibraryPath(BaseDirectory + @"lib\7z-x64.dll");
            }

            //Opens the archive
            using (SevenZipExtractor archivedFile = new SevenZipExtractor(_isoDir))
            {
                //loops throught each file name
                foreach (var file in archivedFile.ArchiveFileData)
                {
                    _filename = new string(file.FileName.Take(4).ToArray());
                    //If filename contains region code...
                    if (regionList.Contains(_filename))
                    {
                        //Return forged serial number
                        gameserial = file.FileName.Replace(".", String.Empty);
                        gameserial = gameserial.Replace("_", "-");
                        return gameserial;
                    }
                }
                return gameserial;
            }
        }

        //Returns a game name, using PCSX2 database file
        public string GetGameName(string _gameserial)
        {
            string GameIndex = emuDir + @"\GameIndex.dbf";
            string GameName = "UNKNOWN";

            //Reads the GameIndex file by line
            using (var reader = new StreamReader(GameIndex))
            {
                
                bool serialFound = false;
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();

                    //Forges a GameIndex.dbf entry
                    //If forged line appears in GameIndex.dbf stop and read the next line
                    if (line.Contains("Serial = " + _gameserial))
                    {
                        serialFound = true;
                    }
                    //The next line which contains name associated with gameserial
                    else if (serialFound == true)
                    {
                        //Cleans the data
                        GameName = line.Replace("Name   = ", String.Empty);
                        return GameName;
                    }
                }
                return GameName;
            }
        }

        //Plus Button
        private void PlusButton_CLick(object sender, RoutedEventArgs e)
        {
            //AddGame(null, @"D:\Program Files (x86)\PCSX2\ICO\softc.iso", @"example");

            //Invokes mainWindow class which navigates to AddGame.xaml
            ((MainWindow)Application.Current.MainWindow).Open_AddGame();
        }
    }
}
