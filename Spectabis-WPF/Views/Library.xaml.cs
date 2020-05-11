using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Cache;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MahApps.Metro.Controls;
using MaterialDesignThemes.Wpf;
using SharpDX.XInput;
using System.Windows.Media.Animation;
using System.Management;
using Spectabis_WPF.Domain;
using System.Runtime.InteropServices;

namespace Spectabis_WPF.Views
{
    public partial class Library : Page
    {

        //Spectabis Variables
        public static string emuDir => Properties.Settings.Default.emuDir;

	    private string GameConfigs;
        private string BaseDirectory = App.BaseDirectory;

        //Temporary variable for rightclick funcionality
        private Image clickedBoxArt;

        //Async game art scrapping variables
        List<string> taskQueue = new List<string>();
        private BackgroundWorker QueueThread = new BackgroundWorker();

        //Scraping Threads
        private BackgroundWorker artScraper = new BackgroundWorker();
        private AutoResetEvent _resetEvent = new AutoResetEvent(false);

        //Current xInput controller
        private Controller xController;

        //Controller input listener thread
        private BackgroundWorker xListener = new BackgroundWorker();

        //Events for USB device detection
        private ManagementEventWatcher mwe_deletion;
        private ManagementEventWatcher mwe_creation;

        private List<string> LoadedISOs = new List<string>();

        //Make alist of all arguments
        public static List<string> arguments = new List<string>(Environment.GetCommandLineArgs());

        //PCSX2 Process
        private Process PCSX = new Process();

        public Library()
        {
            InitializeComponent();

            Console.WriteLine("Opening Library...");

            CheckForUpdates();

            //Where game profile folders are saved
            GameConfigs = BaseDirectory + @"\resources\configs\";

            var advancedIni = new IniFile(BaseDirectory + @"\advanced.ini");
            var _enableXInput = advancedIni.Read("EnableXinput", "Input");
            if (_enableXInput == "false")
            {
                Console.WriteLine("Disabling XInput...");
                arguments.Add("-ignorexinput");
            }

            //Starts the TaskQueue timer
            System.Windows.Threading.DispatcherTimer taskList = new System.Windows.Threading.DispatcherTimer();
            taskList.Tick += taskList_Tick;
            taskList.Interval = new TimeSpan(0, 0, 3);
            taskList.Start();

            //QueueThread Initialization
            artScraper.WorkerSupportsCancellation = true;
            QueueThread.WorkerSupportsCancellation = true;
            QueueThread.WorkerReportsProgress = true;
            QueueThread.DoWork += QueueThread_DoWork;

            if (arguments.Contains("-ignorexinput") == false)
            {
                Console.WriteLine("xInput Initialization");

                //xInput Initialization
                getCurrentController();

                //xInput listener
                xListener.DoWork += xListener_DoWork;

                //detect new USB device
                WqlEventQuery q_creation = new WqlEventQuery();
                q_creation.EventClassName = "__InstanceCreationEvent";
                q_creation.WithinInterval = new TimeSpan(0, 0, 2);
                q_creation.Condition = @"TargetInstance ISA 'Win32_USBControllerdevice'";
                mwe_creation = new ManagementEventWatcher(q_creation);
                mwe_creation.EventArrived += new EventArrivedEventHandler(USBEventArrived);
                mwe_creation.Start();

                //detect USB device deletion
                WqlEventQuery q_deletion = new WqlEventQuery();
                q_deletion.EventClassName = "__InstanceDeletionEvent";
                q_deletion.WithinInterval = new TimeSpan(0, 0, 2);
                q_deletion.Condition = @"TargetInstance ISA 'Win32_USBControllerdevice'  ";
                mwe_deletion = new ManagementEventWatcher(q_deletion);
                mwe_deletion.EventArrived += new EventArrivedEventHandler(USBEventArrived);
                mwe_deletion.Start();
            }

            //Hide searchbar
            if (Properties.Settings.Default.searchbar == false)
            {
                SearchPanel.Visibility = Visibility.Collapsed;
            }

            //Set popup buttons visible
            PopButtonHitTest(true);

            //Load game profiles
            reloadGames();

            //List all loaded games
            EnumerateISOs();

            ScanGameDirectory();

            //Set appropriate menu icon for global controller settings
            setGlobalControllerIcon(Properties.Settings.Default.globalController);
        }

        private void CheckForUpdates()
        {
            Console.WriteLine("Checking for updates...");
            if (Properties.Settings.Default.checkupdates)
            {
                if (UpdateCheck.isNewUpdate())
                {
                    try
                    {
                        //Push snackbar
                        this.Invoke(new Action(() => PushSnackbar("A new update is available!")));
                    }
                    catch
                    {
                        Console.WriteLine("Couldn't push update notification");
                    }

                }
            }

        }

        //MouseDown event on boxArt image
        private void boxArt_Click(object sender, MouseButtonEventArgs e)
        {
            clickedBoxArt = (Image)sender;
            Console.WriteLine(Convert.ToString(clickedBoxArt.Tag) + " - clicked");

            //Get isoDir from Spectabis.ini
            //string _cfgDir = BaseDirectory + @"resources\configs\" + clickedBoxArt.Tag;
            string _cfgDir = GameConfigs + clickedBoxArt.Tag;

            var _gameIni = new IniFile(_cfgDir + @"\spectabis.ini");
            var _isoDir = _gameIni.Read("isoDirectory", "Spectabis");

            if (File.Exists(emuDir) == false)
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

                        Console.WriteLine($"{_launchargs} {_isoDir} --cfgpath={_cfgDir}");

                        //Copy global controller settings
                        Console.WriteLine($"CopyGlobalProfile({clickedBoxArt.Tag.ToString()})");
                        GameProfile.CopyGlobalProfile(clickedBoxArt.Tag.ToString());

                        //Paths in PCSX2 command arguments have to be in quotes...
                        const string quote = "\"";

                        //PCSX2 Process
                        PCSX.StartInfo.FileName = emuDir;
                        PCSX.StartInfo.Arguments = $"{_launchargs} {quote}{_isoDir}{quote} --cfgpath={quote}{_cfgDir}{quote}";

                        PCSX.EnableRaisingEvents = true;
                        PCSX.Exited += new EventHandler(PCSX_Exited);

                        PCSX.Start();

                        //Elevate Process
                        PCSX.PriorityClass = ProcessPriorityClass.AboveNormal;

                        //Minimize Window
                        this.Invoke(new Action(() => ((MainWindow)Application.Current.MainWindow).MainWindow_Minimize()));

                        //Set running game text
                        this.Invoke(new Action(() => ((MainWindow)Application.Current.MainWindow).SetRunningGame(clickedBoxArt.Tag.ToString())));

                        BlockInput(true);

                    }
                    else
                    {
                        Console.WriteLine(_isoDir + " does not exist!");
                    }
                }
            }

            //If left click
            if (e.XButton1 == e.LeftButton)
            {
                OpenContext();
            }
        }

        //Kill PCSX2 process
        public void ForceStop()
        {
            try
            {
                PCSX.Kill();
                BlockInput(false);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        //Block Spectabis while PCSX2 is running
        private void BlockInput(bool e)
        {
            this.Invoke(new Action(() => ((MainWindow)Application.Current.MainWindow).BlockInput(e)));
        }

        private void PCSX_Exited(object sender, EventArgs e)
        {
            //Bring Spectabis to front
            BlockInput(false);
            this.Invoke(new Action(() => ((MainWindow)Application.Current.MainWindow).MainWindow_Maximize()));
        }

        private void OpenContext()
        {
            //Creates a ContextMenu
            ContextMenu gameContext = new ContextMenu();

            //Emulator Settings menu button
            MenuItem PCSX2config = new MenuItem();
            PCSX2config.Header = "Configure in PCSX2";
            PCSX2config.Click += PCSX2ConfigureGame_Click;

            //Refetch Cover art
            MenuItem RedownloadArt = new MenuItem();
            RedownloadArt.Header = "Refetch Boxart";
            RedownloadArt.Click += RedownloadArt_Click;

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
            gameContext.Items.Add(RedownloadArt);
            gameContext.Items.Add(PCSX2config);
            gameContext.Items.Add(RemoveGame);


            //Open context menu
            gameContext.IsOpen = true;
        }

        //Context Menu PCSX2 button
        private void PCSX2ConfigureGame_Click(object sender, RoutedEventArgs e)
        {
            //Title of the last clicked game
            string _title = Convert.ToString(clickedBoxArt.Tag);

            //Start PCSX2 only with --cfgpath
            string _cfgDir = GameConfigs + @"/" + clickedBoxArt.Tag;
            Process.Start(emuDir, " --cfgpath=\"" + _cfgDir + "\"");

        }

        //Context Menu Redownload boxart
        private void RedownloadArt_Click(object sender, RoutedEventArgs e)
        {
            //Title of the last clicked game
            string _title = Convert.ToString(clickedBoxArt.Tag);

            //Add game to art download queue
            taskQueue.Add(_title);
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

            //Reads the game's iso file and adds it to blacklist
            IniFile SpectabisINI = new IniFile(GameConfigs + @"\" + _title + @"\spectabis.ini");
            AddToBlacklist(SpectabisINI.Read("isoDirectory", "Spectabis"));

            clickedBoxArt.Source = null;
            UpdateLayout();

            gamePanel.Children.Remove(clickedBoxArt);

            //Delete profile folder
            if (Directory.Exists(GameConfigs + @"/" + clickedBoxArt.Tag))
            {
                try
                {
                    Directory.Delete(GameConfigs + @"/" + clickedBoxArt.Tag, true);

                }
                catch
                {
                    PushSnackbar("Failed to delete game files!");
                }

            }

            //Reload game list
            reloadGames();
        }

        //boxArt Image for game tiles
        private Image CreateBoxArtResource(string game)
        {
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

            artSource.DecodePixelHeight = 200;
            artSource.DecodePixelWidth = 150;

            //Closes the filestream
            artSource.EndInit();

            //sets boxArt source to created bitmap
            boxArt.Source = artSource;

            boxArt.Height = 200;
            boxArt.Width = 150;

            //Creates a gap between tiles
            //There is an issue, when scaling another object when referencing this object's size, the gap is added to the size
            //To counter this, use ActualSize
            boxArt.Margin = new Thickness(10, 0, 0, 10);

            boxArt.Stretch = Stretch.Fill;
            boxArt.MouseDown += boxArt_Click;
            boxArt.Focusable = true;

            return boxArt;
        }

        //gameTile grid object
        private Grid CreateGameTileGridResource()
        {
            Grid gameTile = new Grid();

            gameTile.MouseEnter += new MouseEventHandler(gameTile_MouseEnter);
            gameTile.MouseLeave += new MouseEventHandler(gameTile_MouseLeave);

            return gameTile;
        }

        //gameTile overlay for hover
        private System.Windows.Shapes.Rectangle CreateTitleOverlayResource()
        {
            System.Windows.Shapes.Rectangle overlay = new System.Windows.Shapes.Rectangle();

            overlay.Fill = CurrentPrimary();
            overlay.Opacity = 0.8;
            overlay.Visibility = Visibility.Collapsed;
            overlay.IsHitTestVisible = false;

            return overlay;
        }

        //game title in game tiles
        private TextBlock CreateTextBlockResource()
        {
            TextBlock gameTitle = new TextBlock();

            gameTitle.Name = "title";
            gameTitle.HorizontalAlignment = HorizontalAlignment.Center;
            gameTitle.VerticalAlignment = VerticalAlignment.Bottom;
            gameTitle.Width = 150;
            gameTitle.FontSize = 16;
            gameTitle.Foreground = new SolidColorBrush(Colors.White);
            gameTitle.Margin = new Thickness(0, 0, 0, 30);
            gameTitle.TextAlignment = TextAlignment.Center;
            gameTitle.TextWrapping = TextWrapping.Wrap;
            gameTitle.Visibility = Visibility.Visible;
            gameTitle.FontFamily = new FontFamily("Roboto Light");

            return gameTitle;
        }

        private TextBlock CreatePlayTimeResource()
        {
            TextBlock block = new TextBlock();

            block.Name = "playtime";
            block.VerticalAlignment = VerticalAlignment.Top;
            block.HorizontalAlignment = HorizontalAlignment.Center;
            block.FontSize = 16;
            block.Margin = new Thickness(0, 10, 0, 30);
            block.Foreground = new SolidColorBrush(Colors.White);
            block.Width = 150;
            block.TextAlignment = TextAlignment.Center;
            block.Visibility = Visibility.Visible;
            block.FontFamily = new FontFamily("Roboto Light");

            return block;
        }

        //Rescans the game config directory and adds them to gamePanel
        public void reloadGames(string query = "")
        {
            //Removes all games from list
            gamePanel.Children.Clear();

            //Checks, if any games are added to the library
            bool gamesExist = false;

            //If command line argument "-ignoreporfiles", then do nothing
            if (arguments.Contains("-ignoreprofiles"))
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
                    //Loads only games that contain query string
                    if (game.ToLower().Contains(query.ToLower()))

                        if (File.Exists(game + @"\Spectabis.ini"))
                        {
                            //Sets _gameName to name of the folder
                            string _gameName = game.Remove(0, game.LastIndexOf(Path.DirectorySeparatorChar) + 1);

                            //Creates an image object
                            Image boxArt = CreateBoxArtResource(game);
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

                            //Set tooltip, if enabled
                            if (Properties.Settings.Default.tooltips)
                            {
                                boxArt.ToolTip = _gameName;
                            }

                            //Define the grid for game tiles

                            //If showtitle is selected
                            if (Properties.Settings.Default.showTitle == true)
                            {
                                Grid gameTile = CreateGameTileGridResource();

                                //Center boxart image
                                boxArt.HorizontalAlignment = HorizontalAlignment.Center;
                                gameTile.Children.Add(boxArt);

                                //Create the colored rectangle
                                System.Windows.Shapes.Rectangle overlay = CreateTitleOverlayResource();
                                gameTile.Children.Add(overlay);

                                //Create a textblock for game title
                                TextBlock gameTitle = CreateTextBlockResource();
                                gameTile.Children.Add(gameTitle);

                                //Create playtime text
                                TextBlock playTime = CreatePlayTimeResource();
                                gameTile.Children.Add(playTime);

                                //Add created tile to game panel
                                gamePanel.Children.Add(gameTile);
                            }
                            else
                            {
                                //Adds the object to gamePanel
                                Grid gameTile = new Grid();

                                boxArt.HorizontalAlignment = HorizontalAlignment.Center;
                                gameTile.Children.Add(boxArt);

                                //Add created tile to game panel
                                gamePanel.Children.Add(gameTile);
                            }
                        }
                }

                //Show "drag&drop" hint accordingly
                if (gamesExist == false)
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

        //Discover a game 
        public void refreshTile(string game){
            var boxArt = gamePanel.Children
                .OfType<Grid>()
                .SelectMany(p => p.Children.OfType<Image>())
                .FirstOrDefault(p=>(string)p.Tag == game);

            if (boxArt == null)
                return;

            if (string.IsNullOrEmpty(game)) {
                boxArt.Source = null;
                return;
            }
            //Creates a bitmap stream
            var artSource = new System.Windows.Media.Imaging.BitmapImage();

            artSource.BeginInit();

            //Fixes the caching issues, where cached copy would just hang around and bother me for two days
            artSource.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.None;
            artSource.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            artSource.CreateOptions = System.Windows.Media.Imaging.BitmapCreateOptions.IgnoreImageCache;

            artSource.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
            artSource.UriSource = new Uri(@"resources\configs\" + game + @"\art.jpg", UriKind.RelativeOrAbsolute);

            artSource.EndInit();

            WpfAnimatedGif.ImageBehavior.SetAnimatedSource(boxArt, artSource);
        }

        public void renameTile(string _old, string _new)
        {
            var boxArt = gamePanel.Children
                .OfType<Grid>()
                .SelectMany(p => p.Children.OfType<Image>())
                .FirstOrDefault(p => (string)p.Tag == _old);

            if (boxArt == null)
                throw new Exception();
            
            boxArt.Tag = _new;
        }

        //Create a new game tile in gamePanel
        public void CreateTile(string game)
        {
            Image boxArt = CreateBoxArtResource($"resources\\configs\\{game}");
            boxArt.Tag = game;

            if (Properties.Settings.Default.showTitle == true)
            {
                Grid gameTile = CreateGameTileGridResource();

                //Center boxart image
                boxArt.HorizontalAlignment = HorizontalAlignment.Center;
                gameTile.Children.Add(boxArt);

                //Create the colored rectangle
                System.Windows.Shapes.Rectangle overlay = CreateTitleOverlayResource();
                gameTile.Children.Add(overlay);

                //Create a textblock for game title
                TextBlock gameTitle = CreateTextBlockResource();
                gameTile.Children.Add(gameTitle);

                //Create playtime text
                TextBlock playTime = CreatePlayTimeResource();
                gameTile.Children.Add(playTime);

                //Add created tile to game panel
                gamePanel.Children.Add(gameTile);
            }
            else
            {
                //Adds the object to gamePanel
                Grid gameTile = new Grid();

                boxArt.HorizontalAlignment = HorizontalAlignment.Center;
                gameTile.Children.Add(boxArt);

                //Add created tile to game panel
                gamePanel.Children.Add(gameTile);
            }

        }

        //Get primary color from current palette
        public SolidColorBrush CurrentPrimary()
        {
            PaletteHelper PaletteQuery = new PaletteHelper();
            Palette currentPalette = PaletteQuery.QueryPalette();
            SolidColorBrush brush = new SolidColorBrush(currentPalette.PrimarySwatch.PrimaryHues.ElementAt(7).Color);
            return brush;
        }

        //Mouse enter for game tile
        private void gameTile_MouseEnter(object sender, MouseEventArgs e)
        {
            var gameTile = (Grid)sender;
            string gameName = null;
            double actualWidth = 0;
            double actualHeight = 0;

            //All objects in gametile Tile
            foreach (object child in gameTile.Children)
            {
                //Get the image tag and game name
                if (child.GetType().ToString() == "System.Windows.Controls.Image")
                {
                    var control = (Image)child;

                    //Get the tag(game name) and size of the boxart
                    gameName = control.Tag.ToString();
                    actualWidth = control.ActualWidth;
                    actualHeight = control.ActualHeight;
                }

                //Show the color overlay
                if (child.GetType().ToString() == "System.Windows.Shapes.Rectangle")
                {
                    var control = (System.Windows.Shapes.Rectangle)child;
                    control.Visibility = Visibility.Visible;

                    //fix the size issue created by margin
                    control.Width = actualWidth;
                    control.Height = actualHeight;

                    control.HorizontalAlignment = HorizontalAlignment.Right;
                    control.VerticalAlignment = VerticalAlignment.Top;
                }

                //Show the game title
                if (child.GetType().ToString() == "System.Windows.Controls.TextBlock")
                {
                    var control = (TextBlock)child;
                    if(control.Name == "title")
                    {
                        control.Text = gameName;
                        control.Visibility = Visibility.Visible;

                        //fix the size issue created by margin
                        control.HorizontalAlignment = HorizontalAlignment.Right;
                    }
                }

                //Show the game title
                if (child.GetType().ToString() == "System.Windows.Controls.TextBlock")
                {
                    var control = (TextBlock)child;

                    if (control.Name == "playtime")
                    {
                        if(Properties.Settings.Default.playtime == true)
                        {
                            IniFile spectabis = new IniFile($"{GameConfigs}//{gameName}//spectabis.ini");
                            string minutes = spectabis.Read("playtime", "Spectabis");
                            if (minutes != "")
                            {
                                control.Text = minutes + " minutes";
                                control.Visibility = Visibility.Visible;

                                //fix the size issue created by margin
                                control.HorizontalAlignment = HorizontalAlignment.Right;
                            }
                        }
                    }
                }
            }
        }

        //Mouse leave for game tile
        private void gameTile_MouseLeave(object sender, MouseEventArgs e)
        {
            var gameTile = (Grid)sender;

            //All objects in gametile Tile
            foreach (object child in gameTile.Children)
            {
                //color overlay
                if (child.GetType().ToString() == "System.Windows.Shapes.Rectangle")
                {
                    var control = (System.Windows.Shapes.Rectangle)child;
                    control.Visibility = Visibility.Collapsed;
                }

                //game title
                if (child.GetType().ToString() == "System.Windows.Controls.TextBlock")
                {
                    var control = (TextBlock)child;
                    control.Visibility = Visibility.Collapsed;
                }
            }
        }

        //Push snackbar function
        public void PushSnackbar(string message)
        {
            var messageQueue = SnackBar.MessageQueue;

            //the message queue can be called from any thread
            Task.Factory.StartNew(() => messageQueue.Enqueue(message));
        }

        //Push when a new game in directory is detected
        private void PushDirectoryDialog(string game)
        {
            TextBlock text = new TextBlock();
            text.FontFamily = new FontFamily("Roboto Light");
            text.Text = $"Would you like to add \"{GetGameName.GetName(game)}\" ?";
            text.TextWrapping = TextWrapping.Wrap;
            text.VerticalAlignment = VerticalAlignment.Center;
            text.Margin = new Thickness(0, 0, 10, 0);

            Button YesButton = new Button();
            YesButton.Content = "Yes";
            YesButton.Margin = new Thickness(0, 0, 10, 0);
            YesButton.Click += DirectoryDialog_Click;

            Button NoButton = new Button();
            NoButton.Content = "No";
            NoButton.Click += DirectoryDialog_Click;

            //Set file path as tag, so it can be accessed by Dialog_Click
            YesButton.Tag = game;
            NoButton.Tag = game;

            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.Children.Add(text);
            panel.Children.Add(YesButton);
            panel.Children.Add(NoButton);

            var messageQueue = SnackBar.MessageQueue;
            Task.Factory.StartNew(() => messageQueue.Enqueue(panel));
        }

        //Directory Snackbar notification Yes/No buttons
        private void DirectoryDialog_Click(object sender, EventArgs e)
        {
            //Get the game file from button tag
            Button button = (Button)sender;
            string game = button.Tag.ToString();

            //Hide Snackbar
            SnackBar.IsActive = false;

            //Yes Button
            if (button.Content.ToString() == "Yes")
            {
                Console.WriteLine("Adding " + game);

                if (Properties.Settings.Default.titleAsFile)
                {
                    AddGame(null, game, Path.GetFileNameWithoutExtension(game));
                }
                else
                {
                    AddGame(null, game, GetGameName.GetName(game));
                }
            }

            //No Button
            else if (button.Content.ToString() == "No")
            {
                Console.WriteLine("Blacklisting " + game);

                //Add game to blacklist file
                AddToBlacklist(game);
            }

        }

        public List<string> NewGamesInDirectory;

        //Push a snackbar when there's a huge number of new games in a directory
        private void PushMultipleDirectoryDialog(int count, List<string> list)
        {
            NewGamesInDirectory = list;

            TextBlock text = new TextBlock();
            text.FontFamily = new FontFamily("Roboto Light");
            text.Text = $"There are {count} new games in your directory";
            text.TextWrapping = TextWrapping.Wrap;
            text.VerticalAlignment = VerticalAlignment.Center;
            text.Margin = new Thickness(0, 0, 10, 0);

            Button OpenAll = new Button();
            OpenAll.Content = "Show";
            OpenAll.Click += MultipleDirectory_Click;
            OpenAll.Margin = new Thickness(0, 0, 10, 0);

            Button Dismiss = new Button();
            Dismiss.Content = "Dismiss";
            Dismiss.Click += MultipleDirectory_Click;

            StackPanel panel = new StackPanel();
            panel.Orientation = Orientation.Horizontal;
            panel.Children.Add(text);
            panel.Children.Add(OpenAll);
            panel.Children.Add(Dismiss);

            var messageQueue = SnackBar.MessageQueue;
            Task.Factory.StartNew(() => messageQueue.Enqueue(panel));
        }

        private void MultipleDirectory_Click(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            SnackBar.IsActive = false;

            if (button.Content.ToString() == "Show")
            {
                //Navigate to Game Discovery page
                ((MainWindow)Application.Current.MainWindow).Open_GameDiscovery();
            }
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
                if (SupportedGames.GameFiles.Any(s => file.EndsWith(s)))
                {
                    //If file supports name scrapping
                    if (SupportedGames.ScrappingFiles.Any(s => file.EndsWith(s)))
                    {
                        if(Properties.Settings.Default.titleAsFile)
                        {
                            AddGame(null, file, Path.GetFileNameWithoutExtension(file));
                        }
                        else
                        {
                            AddGame(null, file, GetGameName.GetName(file));
                        }
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
            Console.WriteLine($"Title: {_title}");
            _title = _title.ToSanitizedString();
            Console.WriteLine($"Sanitized: {_title}");

            //Checks, if the game profile already exists
            if (Directory.Exists(BaseDirectory + @"\resources\configs\" + _title))
            {
                if (File.Exists(BaseDirectory + @"\resources\configs\" + _title + @"\Spectabis.ini"))
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
                    Console.WriteLine(inifile + " found!");
                    if (File.Exists(BaseDirectory + @"\resources\configs\" + _title + @"\" + Path.GetFileName(inifile)) == false)
                    {
                        string _destinationPath = Path.Combine(BaseDirectory + @"\resources\configs\" + _title + @"\" + Path.GetFileName(inifile));
                        File.Copy(inifile, _destinationPath);
                    }
                }
            }
            else
            {
                Console.WriteLine($"{emuDir}\\inis\\ not found!");
                Console.WriteLine("Trying " + Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\PCSX2\inis");

                //looks for pcsx2 inis in documents folder
                if (Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\PCSX2\inis"))
                {
                    string[] inisDirDoc = Directory.GetFiles((Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\PCSX2\inis"));
                    foreach (string inifile in inisDirDoc)
                    {
                        if (File.Exists(BaseDirectory + @"\resources\configs\" + _title + @"\" + Path.GetFileName(inifile)) == false)
                        {
                            string _destinationPath = Path.Combine(BaseDirectory + @"\resources\configs\" + _title + @"\" + Path.GetFileName(inifile));
                            File.Copy(inifile, _destinationPath);
                        }
                    }
                }

                //if no inis are found, warning is shown
                else
                {
                    Console.WriteLine("Cannot find default PCSX2 configuration");
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

            try
            {
                File.Copy(BaseDirectory + @"\resources\_temp\art.jpg", BaseDirectory + @"\resources\configs\" + _title + @"\art.jpg", true);
            }
            catch
            {
                Console.WriteLine("Failed to copy temporal art file...");
            }
            finally
            {
                //If game boxart location is null, then try scrapping
                if (_img == null)
                {
                    //Add game title to automatic scrapping tasklist
                    if (Properties.Settings.Default.autoBoxart == true)
                    {
                        Console.WriteLine("Adding " + _title + " to taskQueue!");
                        taskQueue.Add(_title);
                    }
                }

                //Add game to gamePanel
                CreateTile(_title);
            }
        }

        //Plus Button
        private void PlusButton_Click(object sender, RoutedEventArgs e)
        {

            //Invokes mainWindow class which navigates to AddGame.xaml
            ((MainWindow)Application.Current.MainWindow).Open_AddGame();
        }

        //"Add Directory" button
        private void Directory_Click(object sender, RoutedEventArgs e)
        {
            var DirectoryDialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            DirectoryDialog.Description = "Add Game Directory";
            DirectoryDialog.UseDescriptionForTitle = true;

            var DialogResult = DirectoryDialog.ShowDialog();

            if (DialogResult.Value == true)
            {
                Properties.Settings.Default.gameDirectory = DirectoryDialog.SelectedPath;
                Properties.Settings.Default.Save();
                reloadGames();

                Console.WriteLine(DirectoryDialog.SelectedPath + " set as directory!");
            }
            else
            {
                Properties.Settings.Default.gameDirectory = null;
                Properties.Settings.Default.Save();
                PushSnackbar("Game directory folder has been removed!");
            }

            ClearBlacklist();


        }

        //"Global Controller" button left click
        private void GlobalController_Click(object sender, RoutedEventArgs e)
        {
            //Icon is off, click to turn on
            if(GlobalController_Icon.Kind == PackIconKind.XboxControllerOff)
            {
                Console.WriteLine("Turning on Global Controller settings");
                GameProfile.CreateGlobalController();
                setGlobalControllerIcon(true);
                Properties.Settings.Default.globalController = true;
            }
            else if (GlobalController_Icon.Kind == PackIconKind.XboxController)
            {
                Console.WriteLine("Turning off Global Controller settings");
                setGlobalControllerIcon(false);
                Properties.Settings.Default.globalController = false;
            }

            Properties.Settings.Default.Save();
            Console.WriteLine("Settings saved!");
        }

        //Set Controller settings option icon
        private void setGlobalControllerIcon(bool e)
        {
            if(e == true)
            {
                GlobalController_Icon.Kind = PackIconKind.XboxController;
                GlobalController_Button.ToolTip = "Disable Global Controller Profile" + System.Environment.NewLine + "Right click to configure";
            }
            else
            {
                GlobalController_Icon.Kind = PackIconKind.XboxControllerOff;
                GlobalController_Button.ToolTip = "Enable Global Controller Profile";
            }
        }

        [DllImport(@"\plugins\LilyPad.dll")]
        static private extern void PADconfigure();

        //Configuration must be closed so .dll is not in use
        [DllImport(@"\plugins\LilyPad.dll")]
        static private extern void PADclose();

        //"Global Controller" button right click
        private void GlobalController_RightClick(object sender, RoutedEventArgs e)
        {
            if(Properties.Settings.Default.globalController)
            {
                string globalProfile = BaseDirectory + @"resources\configs\#global_controller\LilyPad.ini";
                string inisTemp = BaseDirectory + @"inis\LilyPad.ini";

                Console.WriteLine("Opening Global Controller settings");
                Console.WriteLine("globalProfile = " + globalProfile);
                Console.WriteLine("inisTemp = " + inisTemp);

                GameProfile.CreateGlobalController();

                Directory.CreateDirectory(BaseDirectory + "inis");
                File.Copy(globalProfile, inisTemp, true);

                //Calls the DLL configuration function which is already imported in MainWindow
                PADconfigure();

                //Calls the configration close function which is already imported in MainWindow
                PADclose();

                File.Copy(inisTemp, globalProfile, true);
                File.Delete(inisTemp);
                Directory.Delete(BaseDirectory + @"inis\", true);

                Console.WriteLine("Global settings saved to: " + globalProfile);
            }
        }

        //timer for async boxart task list
        private void taskList_Tick(object sender, EventArgs e)
        {
            //Checks if taskQueue isn't empty
            if (taskQueue.Any())
            {
                //Checks, if QueueThread is already busy
                Console.WriteLine("Checking if QueueThread is busy.");
                if (QueueThread.IsBusy == false)
                {
                    //If not busy, run the QueueThread
                    Console.WriteLine("QueueThread is not busy, starting it");
                    QueueThread.RunWorkerAsync();
                    //Thread artScrapper = new Thread(() => doTaskQueue());
                }
            }
        }

        //QueueThread Work //async task list tick function
        private void QueueThread_DoWork(object sender, DoWorkEventArgs e)
        {
            Console.WriteLine("QueueThread_DoWork");
            //loop all values in taskQueue list
            foreach (var task in taskQueue)
            {
                string _isoname = task;
                Console.WriteLine("QueueThread_DoWork - " + _isoname);
                //Removes the game from taskQueue list
                taskQueue.Remove(task);

                //does artscrapping on another thread with values
                doArtScraping(_isoname);

                //stops at first value
                break;
            }
        }

        //Automatic box art scanner method
        private void doArtScraping(string title)
        {
            //Calls the method, which sets loading boxart
            this.Invoke(() => SetLoadingStateForImage(title));

            var scraper = new ScrapeArt(title);
            var result = scraper.Result;

            this.Invoke(() => {
                if (result == null)
                    PushSnackbar("Couldn't get the game, sorry");
                refreshTile(title);
            });
        }

        //"loading boxart" overlay for games which are currently downloading boxart
        public void SetLoadingStateForImage(string _tagName)
        {
            //Location of loading gif
            string LoadingPlaceholder = BaseDirectory + @"resources\_temp\spinner.gif";

            //All objects in gamePanel
            foreach (Grid gameTile in gamePanel.Children)
            {
                foreach (var obj in gameTile.Children)
                {
                    //Find the Image boxart
                    if (obj.GetType().ToString() == "System.Windows.Controls.Image")
                    {
                        Image boxArt = (Image)obj;
                        //If gamebox is the same as requested, change it
                        if (boxArt.Tag.ToString() == _tagName)
                        {
                            //set source to loading image
                            System.Windows.Media.Imaging.BitmapImage artSource = new System.Windows.Media.Imaging.BitmapImage();
                            //Opens the filestream
                            artSource.BeginInit();
                            artSource.UriSource = new Uri(LoadingPlaceholder);
                            artSource.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                            artSource.EndInit();

                            WpfAnimatedGif.ImageBehavior.SetAnimatedSource(boxArt, artSource);
                            return;
                        }
                    }
                }
            }

        }

        //Returns a list of all games loaded in spectabis
        public void EnumerateISOs()
        {
            List<string> gameList = new List<string>();

            //Get all directories in Spectabis config folder
            string[] _gamesdir = Directory.GetDirectories(GameConfigs);

            //Scan each game for Spectabis.ini
            foreach (var game in _gamesdir)
            {
                if (File.Exists(game + @"\spectabis.ini"))
                {
                    IniFile SpectabisIni = new IniFile(game + @"\spectabis.ini");
                    var isoDir = SpectabisIni.Read("isoDirectory", "Spectabis");
                    gameList.Add(isoDir);
                }
            }

            LoadedISOs = gameList;
        }

        private void ScanGameDirectory()
        {
            if (Properties.Settings.Default.gameDirectory != null)
            {
                //If game directory doesn't exist, stop and remove it from variable
                if (Directory.Exists(Properties.Settings.Default.gameDirectory) == false)
                {
                    PushSnackbar("Game Directory doesn't exist anymore!");
                    Properties.Settings.Default.gameDirectory = null;
                    Properties.Settings.Default.Save();
                }

                //List of all files that don't contain already loaded files
                if (Properties.Settings.Default.gameDirectory == null)
                    return;

                var _fileList = Directory.GetFiles(Properties.Settings.Default.gameDirectory);
                Console.WriteLine(_fileList.Count() + " files found!");

                int _count = _fileList.Except(LoadedISOs).ToList().Count;
                
                Console.WriteLine($"{_count} new files found!");

                //Count after which games will be moved to "Game Discovery" page
                int TooManyFiles = 3;

                if (_count > TooManyFiles)
                {
                    PushMultipleDirectoryDialog(_count, _fileList.Except(LoadedISOs).ToList());
                }
                else
                {
                    //Go through each file
                    foreach (var file in _fileList)
                    {
                        //Check, if file type is in supported file types
                        if (SupportedGames.GameFiles.Any(s => file.EndsWith(s)))
                        {
                            //Check if file is already loaded in Spectabis
                            List<string> IsoList = LoadedISOs;
                            if (IsoList.Contains(file) == false)
                            {
                                Console.WriteLine(file + " is not loaded, prompting to add!");

                                //Checks, if file is in blacklist file
                                if (IsGameBlacklisted(file) == false)
                                {
                                    //Show a Yes/No message box
                                    //If "Yes" then add the game, if not, add it to blacklist
                                    this.Invoke(new Action(() => PushDirectoryDialog(file)));
                                }
                            }
                            else
                            {
                                Console.WriteLine(file + " is already loaded, skipping.");
                            }
                        }
                    }
                }
            }
        }

        private void AddToBlacklist(string _file)
        {
            //Create a folder and blacklist.text if it doesn't exist
            Directory.CreateDirectory(BaseDirectory + @"\resources\logs\");
            if (File.Exists(BaseDirectory + @"\resources\logs\blacklist.txt") == false)
            {
                var newFile = File.Create(BaseDirectory + @"\resources\logs\blacklist.txt");
                newFile.Close();
            }

            //Add a line to blacklist
            StreamWriter blacklistFile = new StreamWriter(BaseDirectory + @"\resources\logs\blacklist.txt", append: true);
            blacklistFile.WriteLine(_file);
            blacklistFile.Close();
        }

        private bool IsGameBlacklisted(string _file)
        {
            //Create a folder and blacklist.text if it doesn't exist
            Directory.CreateDirectory(BaseDirectory + @"\resources\logs\");
            if (File.Exists(BaseDirectory + @"\resources\logs\blacklist.txt") == false)
            {
                var newFile = File.Create(BaseDirectory + @"\resources\logs\blacklist.txt");
                newFile.Close();
            }

            StreamReader blacklistFile = new StreamReader(BaseDirectory + @"\resources\logs\blacklist.txt");
            if (blacklistFile.ReadToEnd().Contains(_file))
            {
                blacklistFile.Close();
                return true;
            }
            else
            {
                blacklistFile.Close();
                return false;
            }
        }


        //Delete blacklist file and create a blank file
        private void ClearBlacklist()
        {
            if(File.Exists(BaseDirectory + @"\resources\logs\blacklist.txt"))
            {
                File.Delete(BaseDirectory + @"\resources\logs\blacklist.txt");
            }
            Directory.CreateDirectory($"{BaseDirectory}\\resources\\logs\\");
            File.Create($"{BaseDirectory}\\resources\\logs\\blacklist.txt");
        }

        //Controls "Plus" Button popup button visiblity
        void PopButtonHitTest(bool e)
        {
            if (e == true)
            {
                PopupStackPanel.Visibility = Visibility.Visible;
            }
            else
            {
                PopupStackPanel.Visibility = Visibility.Collapsed;
            }
        }

        //Detect when USB devices change
        private void USBEventArrived(object sender, EventArrivedEventArgs e)
        {
            getCurrentController();
        }

        //Gets the currently connected controller
        public void getCurrentController()
        {
            //Checks, if controller is connected before detecting a new controller
            bool wasConnected = false;
            if (xController != null)
            {
                wasConnected = true;
            }

            //currentXInputDevice.cs
            currentXInputDevice getDevice = new currentXInputDevice();
            xController = getDevice.getActiveController();

            if (File.Exists(BaseDirectory + @"\x360ce.ini"))
            {
                Console.WriteLine("X360CE.ini found, be sure to use xinput1_4.dll 32-bit version");
            }

            //Show controller message, only when appropriate
            if (xController != null)
            {
                if (wasConnected == false)
                {
                    //When new a controller is detected
                    setControllerState(1);

                    Console.WriteLine("Starting xListener thread!");
                    xListener.RunWorkerAsync();
                }
            }
            else
            {
                Console.WriteLine("No controllers detected");
                if (wasConnected == true)
                {
                    //When controller is unplugged
                    setControllerState(2);
                }
            }

        }

        //Sets controller state label text
        private void setControllerState(int i)
        {
            string statusText = null;

            if (i == 1)
            {
                statusText = "Controller Detected";
            }
            else if (i == 2)
            {
                statusText = "Controller Unplugged";
            }

            //Invoke Dispatcher, in case multiple USB devices are added at the same time
            Dispatcher.BeginInvoke(new Action(() =>
            {
                //Set text from status
                ControllerStatus.Content = statusText;

                //Play fade-out animation
                DoubleAnimation da = new DoubleAnimation();
                da.Duration = TimeSpan.FromMilliseconds(1500);
                da.From = 1;
                da.To = 0;

                ControllerStatus.BeginAnimation(Label.OpacityProperty, da);
            }));
        }

        //Controller input listener thread
        private void xListener_DoWork(object sender, DoWorkEventArgs e)
        {
            Console.WriteLine("xListener Started");

            currentXInputDevice xDevice = new currentXInputDevice();
            var previousState = xController.GetState();

            Console.WriteLine(xDevice.getActiveController().ToString());

            while (xController.IsConnected)
            {
                var buttons = xController.GetState().Gamepad.Buttons;

                //Check for buttons here!
                if (xDevice.getPressedButton(buttons) != "None")
                {
                    Console.WriteLine(xDevice.getPressedButton(buttons));
                }

                Thread.Sleep(100);
            }

            Console.WriteLine("Disposing of xListener thread!");
        }

        //Search bar key event
        private void SearchBar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                //Cancel search and reload full library
                reloadGames();
                SearchBar.Text = null;
                MoveFocus(e);
                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                //Search and load only games with query
                reloadGames(SearchBar.Text);
                MoveFocus(e);
                e.Handled = true;
            }
        }

        //SearchBar "click"
        private void SearchBar_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchBar.Text.Length != 0)
            {
                reloadGames();
                SearchBar.Text = null;
            }
        }

        //Remove focus from textbox
        private void MoveFocus(KeyEventArgs e)
        {
            //http://stackoverflow.com/questions/8203329/moving-to-next-control-on-enter-keypress-in-wpf

            FocusNavigationDirection focusDirection = FocusNavigationDirection.Next;
            TraversalRequest request = new TraversalRequest(focusDirection);
            UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

            // Change keyboard focus.
            if (elementWithFocus != null)
            {
                if (elementWithFocus.MoveFocus(request)) e.Handled = true;
            }
        }
    }
}
