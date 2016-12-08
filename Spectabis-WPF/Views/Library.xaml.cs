using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro.Controls;
using SevenZip;
using TheGamesDBAPI;

namespace Spectabis_WPF.Views
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

        //Async game art scrapping variables
        List<string> taskQueue = new List<string>();
        public BackgroundWorker QueueThread = new BackgroundWorker();

        //Scrapping Threads
        public BackgroundWorker artScrapper = new BackgroundWorker();
        private AutoResetEvent _resetEvent = new AutoResetEvent(false);

        //Make alist of all arguments
        public List<string> arguments = new List<string>(Environment.GetCommandLineArgs());

        public Library()
        {
            InitializeComponent();

            //Where game profile folders are saved
            GameConfigs = BaseDirectory + @"\resources\configs\";

            //Adds supported game image files to a list
            //Case sensitive, for some reason
            supportedGameFiles.Add("iso");
            supportedGameFiles.Add("bin");
            supportedGameFiles.Add("cso");
            supportedGameFiles.Add("gz");

            supportedGameFiles.Add("ISO");
            supportedGameFiles.Add("BIN");
            supportedGameFiles.Add("CSO");
            supportedGameFiles.Add("GZ");

            //Adds supported files for artScrapping to a list
            //Case sensitive, for some reason
            supportedScrappingFiles.Add("iso");
            supportedScrappingFiles.Add("ISO");

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

            //Starts the TaskQueue timer
            System.Windows.Threading.DispatcherTimer taskList = new System.Windows.Threading.DispatcherTimer();
            taskList.Tick += taskList_Tick;
            taskList.Interval = new TimeSpan(0, 0, 3);
            taskList.Start();

            //QueueThread Initialization
            artScrapper.WorkerSupportsCancellation = true;
            QueueThread.WorkerSupportsCancellation = true;
            QueueThread.WorkerReportsProgress = true;
            QueueThread.DoWork += QueueThread_DoWork;

            //Load game profiles
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

            if (File.Exists(emuDir + @"\pcsx2.exe") == false)
            {
                PushSnackbar("PCSX2 installation corrupt");
            }

            if (File.Exists(_isoDir) == false)
            {
                PushSnackbar("Game doesn't exist anymore");
            }

            //Save needed click count to variable
            int _ClickCount;
            if (Properties.Settings.Default.doubleClick == true)
            {
                _ClickCount = 2;
            }
            else
            {
                _ClickCount = 1;
            }

            //If right click
            if (e.XButton1 == e.RightButton)
            {
                //Checks for click count
                if (e.ClickCount == _ClickCount)
                {
                    if (File.Exists(_isoDir))
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
            if (e.XButton1 == e.LeftButton)
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
            string _name = Convert.ToString(clickedBoxArt.Tag);
            ((MainWindow)Application.Current.MainWindow).Open_Settings(true, _name);

        }

        //Context Menu Remove button
        private void RemoveGame_Click(object sender, RoutedEventArgs e)
        {
            //Title of the last clicked game
            string _title = Convert.ToString(clickedBoxArt.Tag);

            clickedBoxArt.Source = null;
            UpdateLayout();

            gamePanel.Children.Remove(clickedBoxArt);

            //Delete profile folder
            if (Directory.Exists(GameConfigs + @"/" + clickedBoxArt.Tag))
            {
                Directory.Delete(GameConfigs + @"/" + clickedBoxArt.Tag, true);
            }

            //Reload game list
            reloadGames();
        }

        //Rescans the game config directory and adds them to gamePanel
        public void reloadGames()
        {

            //Removes all games from list
            gamePanel.Children.Clear();

            //Checks, if any games are added to the library
            bool gamesExist = false;

            //If command line argument "-ignoreporfiles", then do nothing
            if(arguments.Contains("-ignoreprofiles"))
            {
                return;
            }

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

                        //Fixes the caching issues, where cached copy would just hang around and bother me for two days
                        artSource.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.None;
                        artSource.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
                        artSource.CreateOptions = System.Windows.Media.Imaging.BitmapCreateOptions.IgnoreImageCache;

                        artSource.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                        artSource.UriSource = new Uri(game + @"\art.jpg", UriKind.RelativeOrAbsolute);
                        //Closes the filestream
                        artSource.EndInit();

                        //sets boxArt source to created bitmap
                        boxArt.Source = artSource;

                        boxArt.Height = 200;
                        boxArt.Width = 150;

                        //Creates a gap between tiles
                        boxArt.Margin = new Thickness(10,0,0,0);

                        boxArt.Stretch = Stretch.Fill;
                        boxArt.MouseDown += boxArt_Click;
                        boxArt.Tag = _gameName;

                        //Set BitmapScalingMode from advanced.ini if set
                        //Forgive me
                        if (File.Exists(BaseDirectory + @"\advanced.ini"))
                        {
                            var advancedIni = new IniFile(BaseDirectory + @"\advanced.ini");

                            var _BitmapScalingMode = advancedIni.Read("BitmapScalingMode", "Renderer");

                            if (_BitmapScalingMode == "BitmapScalingMode.HighQuality")
                            {
                                RenderOptions.SetBitmapScalingMode(boxArt, BitmapScalingMode.HighQuality);
                            }
                            else if (_BitmapScalingMode == "BitmapScalingMode.LowQuality")
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

                        gamesExist = true;

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

                //Show "drag&drop" hint accordingly
                if(gamesExist == false)
                {
                    NoGameLabel.Visibility = Visibility.Visible;
                }
                else
                {
                    NoGameLabel.Visibility = Visibility.Collapsed;
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
                if (supportedGameFiles.Any(s => file.EndsWith(s)))
                {
                    //If file supports name scrapping
                    if (supportedScrappingFiles.Any(s => file.EndsWith(s)))
                    {
                        string SerialNumber = GetSerialNumber(file);
                        string GameName = GetGameName(SerialNumber);
                        AddGame(null, file, GameName);
                    }
                    else
                    {
                        PushSnackbar("This filetype doesn't support automatic boxart!");
                        AddGame(null, file, Path.GetFileNameWithoutExtension(file));
                    }
                }
                else
                {
                    PushSnackbar("Unsupported file!");
                }
            }

        }

        //Add game method //_img = null if no game image
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

            //Checks, if the game profile already exists
            if(Directory.Exists(BaseDirectory + @"\resources\configs\" + _title))
            {
                if(File.Exists(BaseDirectory + @"\resources\configs\" + _title + @"\Spectabis.ini"))
                {
                    PushSnackbar("Game Profile already exists!");
                    return;
                }
            }

            //Create a folder for game profile
            Directory.CreateDirectory(BaseDirectory + @"\resources\configs\" + _title);

            //Copies existing ini files from PCSX2
            //looks for inis in pcsx2 directory
            if (Directory.Exists(emuDir + @"\inis\"))
            {
                string[] inisDir = Directory.GetFiles(emuDir + @"\inis\");
                foreach (string inifile in inisDir)
                {
                    Debug.WriteLine(inifile + " found!");
                    if (File.Exists(BaseDirectory + @"\resources\configs\" + _title + @"\" + Path.GetFileName(inifile)) == false)
                    {
                        string _destinationPath = Path.Combine(BaseDirectory + @"\resources\configs\" + _title + @"\" + Path.GetFileName(inifile));
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
                        if (File.Exists(BaseDirectory + @"\resources\configs\" + _title + @"\" + Path.GetFileName(inifile)) == false)
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

            //Copy tempart from resources and filestream it to game profile
            Properties.Resources.tempArt.Save(BaseDirectory + @"\resources\_temp\art.jpg");
            File.Copy(BaseDirectory + @"\resources\_temp\art.jpg", BaseDirectory + @"\resources\configs\" + _title + @"\art.jpg", true);

            //If game boxart location is null, then try scrapping
            if (_img == null)
            {
                //Add game title to automatic scrapping tasklist
                if (Properties.Settings.Default.autoBoxart == true)
                {
                    Debug.WriteLine("Adding " + _title + " to taskQueue!");
                    taskQueue.Add(_title);
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

        //timer for async task list
        private void taskList_Tick(object sender, EventArgs e)
        {
            //Checks if taskQueue isn't empty
            if (taskQueue.Any())
            {
                //Checks, if QueueThread is already busy
                Debug.WriteLine("Checking if QueueThread is busy.");
                if (QueueThread.IsBusy == false)
                {
                    //If not busy, run the QueueThread
                    Debug.WriteLine("QueueThread is not busy, starting it");
                    QueueThread.RunWorkerAsync();
                    //Thread artScrapper = new Thread(() => doTaskQueue());
                }
            }
        }

        //QueueThread Work //async task list tick function
        private void QueueThread_DoWork(object sender, DoWorkEventArgs e)
        {
            Debug.WriteLine("QueueThread_DoWork");
            //loop all values in taskQueue list
            foreach (var task in taskQueue)
            {
                string _isoname = task;
                Debug.WriteLine("QueueThread_DoWork - " + _isoname);
                //Removes the game from taskQueue list
                taskQueue.Remove(task);

                //does artscrapping on another thread with values
                doArtScrapping(_isoname);

                //stops at first value
                break;
            }
        }

        //Automatic box art scanner method
        private void doArtScrapping(string _name)
        {
            //Calls the method, which sets loading boxart
            //Find out, which method is needed and use it
            if (Properties.Settings.Default.showTitle == false)
            {
                this.Invoke(new Action(() => SetLoadingStateForImage(_name, 1)));
                //SetLoadingStateForImage
            }
            else if (Properties.Settings.Default.showTitle == true)
            {

            }

            //TheGamesDB API Scrapping
            if (Properties.Settings.Default.artDB == "TheGamesDB")
            {
                Debug.WriteLine("Using TheGamesDB API");
                //PushSnackbar("Downloading boxart for " + _name);
                try
                {
                    Debug.WriteLine("Starting ArtScrapping for " + _name);

                    //WebRequest.Create(_databaseurl).GetResponse();
                    string _title;
                    string _imgdir;

                    foreach (GameSearchResult game in GamesDB.GetGames(_name, "Sony Playstation 2"))
                    {

                        //Gets game's database ID
                        Game newGame = GamesDB.GetGame(game.ID);

                        _title = _name.Replace(@"/", string.Empty);
                        _title = _title.Replace(@"\", string.Empty);
                        _title = _title.Replace(@":", string.Empty);


                        //Sets image to variable
                        _imgdir = "http://thegamesdb.net/banners/" + newGame.Images.BoxartFront.Path;

                        //Downloads the image
                        using (WebClient client = new WebClient())
                        {
                            try
                            {
                                client.DownloadFile(_imgdir, BaseDirectory + @"\resources\_temp\" + _name + ".jpg");
                                File.Copy(BaseDirectory + @"\resources\_temp\" + _name + ".jpg", BaseDirectory + @"\resources\configs\" + _name + @"\art.jpg", true);

                                //Reload game library
                                this.Invoke(new Action(() => gamePanel.Children.Clear()));
                                this.Invoke(new Action(() => reloadGames()));

                                Debug.WriteLine("Downloaded boxart for " + _name);
                            }
                            catch
                            {
                                this.Invoke(new Action(() => PushSnackbar("Failed to connect to TheGamesDB")));
                            }
                        }

                        //Stops at the first game
                        break;

                    }
                }
                catch
                {
                    this.Invoke(new Action(() => PushSnackbar("Failed to connect to TheGamesDB")));
                }
            }

            //GiantBomb API
            if (Properties.Settings.Default.artDB == "GiantBomb")
            {
                //Variables
                string ApiKey = Properties.Settings.Default.APIKey_GiantBomb;
                var giantBomb = new GiantBomb.Api.Resources.GiantBombRestClient(ApiKey);

                //list for game results
                List<GiantBomb.Api.Model.Game> resultGame = new List<GiantBomb.Api.Model.Game>();

                var PlatformFilter = new Dictionary<string, object>() { { "platform", "PlayStation 2" } };


                //Search for game in DB, get its id, then get the image url
                try
                {
                    resultGame = giantBomb.SearchForGames(_name).ToList();
                    Thread.Sleep(1000);
                }
                catch
                {
                    this.Invoke(new Action(() => PushSnackbar("Failed to connect to GiantBomb. Is the API key valid?")));

                    //Reload game library
                    this.Invoke(new Action(() => gamePanel.Children.Clear()));
                    this.Invoke(new Action(() => reloadGames()));
                    return;
                }

                GiantBomb.Api.Model.Game FinalGame;

                try
                {
                    //loops through each game in resultGame list
                    foreach (GiantBomb.Api.Model.Game game in resultGame)
                    {
                        //Gets game ID and makes a list of platforms it's available for
                        FinalGame = giantBomb.GetGame(game.Id);
                        List<GiantBomb.Api.Model.Platform> platforms = new List<GiantBomb.Api.Model.Platform>(FinalGame.Platforms);

                        //If game platform list contains "PlayStation 2", then start scrapping
                        foreach (var gamePlatform in platforms)
                        {
                            if(gamePlatform.Name == "PlayStation 2")
                            {
                                string _imgdir = FinalGame.Image.SmallUrl;

                                Debug.WriteLine("Using GiantBomb API");
                                Debug.WriteLine("ApiKey = " + ApiKey);
                                Debug.WriteLine("Game ID: " + resultGame.First().Id);
                                Debug.WriteLine(_imgdir);

                                //Downloads the image
                                using (WebClient client = new WebClient())
                                {
                                    //GiantBomb throws 403 if user-agent is less than 5 characters
                                    client.Headers.Add("user-agent", "PCSX2 Spectabis frontend");

                                    try
                                    {
                                        client.DownloadFile(_imgdir, BaseDirectory + @"\resources\_temp\" + _name + ".jpg");
                                        File.Copy(BaseDirectory + @"\resources\_temp\" + _name + ".jpg", BaseDirectory + @"\resources\configs\" + _name + @"\art.jpg", true);

                                        //Reload game library
                                        this.Invoke(new Action(() => gamePanel.Children.Clear()));
                                        this.Invoke(new Action(() => reloadGames()));
                                        return;
                                    }
                                    catch
                                    {
                                        this.Invoke(new Action(() => PushSnackbar("Failed to download the image, check your internet connection.")));

                                        //Reload game library
                                        this.Invoke(new Action(() => gamePanel.Children.Clear()));
                                        this.Invoke(new Action(() => reloadGames()));
                                        return;
                                    }
                                }
                            }
                        }
                    }

                }
                catch
                {
                    this.Invoke(new Action(() => PushSnackbar("Couldn't get the game, sorry")));
                    //Reload game library
                    this.Invoke(new Action(() => gamePanel.Children.Clear()));
                    this.Invoke(new Action(() => reloadGames()));
                }
            }

        }

        //Two methods to set loading image, sorry
        //Finds and sets it, by finding an object with appropriate tag
        //Use these accordingly, by finding value of Properties.Settings.Default.showTitle!
        // _phase is used to differenciate between progress

        //Use only, if using Image style boxart ("not showing game titles")
        public void SetLoadingStateForImage(string _tagName, int _phase)
        {
            //Sets image source of game to loading
            foreach (Image game in gamePanel.Children)
            {
                if (Convert.ToString(game.Tag) == _tagName)
                {
                    //set source to loading image
                    System.Windows.Media.Imaging.BitmapImage artSource = new System.Windows.Media.Imaging.BitmapImage();
                    //Opens the filestream
                    artSource.BeginInit();

                    //Fixes the caching issues, where cached copy would just hang around and bother me for two days
                    artSource.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.None;
                    artSource.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
                    artSource.CreateOptions = System.Windows.Media.Imaging.BitmapCreateOptions.IgnoreImageCache;
                    artSource.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;

                    //shouldn't do this, but I can
                    if(_phase == 1)
                    {
                        artSource.UriSource = new Uri(@"http://smashinghub.com/wp-content/uploads/2014/08/cool-loading-animated-gif-8.gif");
                    }
                    else if(_phase == 2)
                    {
                        //
                    }
                    else if (_phase == 3)
                    {
                        //
                    }
                    else if(_phase ==4)
                    {
                        //
                    }
                    
                    //Closes the filestream
                    artSource.EndInit();

                    game.Source = artSource;
                }
            }

        }



        //Use only, if using Groupbox style boxart ("showing game titles")
        public void SetLoadingStateForGroups()
        {

            //some code will probably come here in time

        }


    }
}
