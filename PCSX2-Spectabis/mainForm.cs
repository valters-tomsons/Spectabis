using System;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using MaterialSkin;
using MaterialSkin.Controls;
using System.Net;
using System.Drawing;
using System.Collections.Generic;
using SevenZipExtractor;
using System.Linq;
using System.ComponentModel;
using System.Threading;
using TheGamesDBAPI;
using System.Threading.Tasks;

namespace PCSX2_Spectabis
{
    public partial class MainWindow : MaterialForm
    {

        //Basic Variables
        private static string emuDir;
        private static string addgamesDir;
        private static string gameserial;

        //Lists
        public static List<string> gamelist = new List<string>();
        public List<string> regionList = new List<string>();

        //Scrapping Threads
        public BackgroundWorker artScrapper = new BackgroundWorker();
        private AutoResetEvent _resetEvent = new AutoResetEvent(false);

        //Async game art scrapping variables
        List<Tuple<String, String>> taskQueue = new List<Tuple<String, String>>();
        public BackgroundWorker QueueThread = new BackgroundWorker();
        

        //Delegate setup for addGameForm
        public delegate void UpdateUiDelegate(string _img, string _isoDir, string _title);
        public event UpdateUiDelegate UpdateUiEvent;

        //Last clicked game
        public PictureBox lastGame;

        //First Time Setup
        public PictureBox welcomeBg = new PictureBox();
        public MaterialFlatButton welcomedirbtn = new MaterialFlatButton();

        


        public MainWindow()
        {
            InitializeComponent();

            //Initialize The Colorscheme
            var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);

            //Loads colorscheme
            if (Properties.Settings.Default.nightMode == true)
            {
                materialSkinManager.Theme = MaterialSkinManager.Themes.DARK;
            }
            else
            {
                materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            }

            //Load saved colorscheme
            if(Properties.Settings.Default.colorscheme == "bluegrey")
            {
                materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);
            }
            else if(Properties.Settings.Default.colorscheme == "pink")
            {
                materialSkinManager.ColorScheme = new ColorScheme(Primary.Pink800, Primary.Pink900, Primary.Pink500, Accent.Pink200, TextShade.WHITE);
            }

            //Loads saved settings
            emuDir = Properties.Settings.Default.EmuDir;
            addgamesDir = Properties.Settings.Default.gamesDir;

            //Initilization
            isoPanel.AutoScroll = true;
            isoPanel.AllowDrop = true;
            UpdateUiEvent += new UpdateUiDelegate(addIso);
            artScrapper.WorkerSupportsCancellation = true;

            QueueThread.WorkerSupportsCancellation = true;
            QueueThread.WorkerReportsProgress = true;
            QueueThread.DoWork += QueueThread_DoWork;
            //QueueThread.RunWorkerCompleted += QueueThread_DoWork;

            //Loads last window size
            if ((Properties.Settings.Default.lastSize == "null") == false)
            {
                string _size = Properties.Settings.Default.lastSize;
                _size = _size.Replace("{Width=", String.Empty);
                _size = _size.Replace("}", String.Empty);
                _size = _size.Replace(" Height=", String.Empty);
                this.Width = Convert.ToInt32(_size.Split(',')[0]);
                this.Height = Convert.ToInt32(_size.Split(',')[1]);
            }


            //Creates required directories
            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\");
            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"\resources\logs\");

            //If blacklist file doesn't exist, create one
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\resources\logs\blacklist.txt") == false)
            {
                FileStream newblacklist = null;
                newblacklist = File.Create(AppDomain.CurrentDomain.BaseDirectory + @"\resources\logs\blacklist.txt");
                newblacklist.Close();
            }

            //Adds items to region list
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

            //Integrity Checks
            if (emuDir == "null")
            {
                FirstTimeSetup(true);
            }
            else
            {
                copyDLL();

                //Searches game folders in resources directory
                string[] gamesDir = Directory.GetDirectories(AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\");
                foreach (string dir in gamesDir)
                {
                    //Removes symbols from game title
                    string _title = dir;
                    string _name = dir.Remove(0, dir.LastIndexOf(System.IO.Path.DirectorySeparatorChar) + 1);
                    _name = _name.Trim(new Char[] { ' ', '*', '.', '\\', '/' });
                    //_name = _name.Remove(0, dir.LastIndexOf(System.IO.Path.DirectorySeparatorChar) + 1);

                    if (File.Exists(_title + @"\art.jpg"))
                    {
                        var gameIni = new IniFile(_title + @"\spectabis.ini");
                        var _isoDir = gameIni.Read("isoDirectory", "Spectabis");

                        if (File.Exists(_isoDir))
                        {
                            //Creates a group for game tile
                            FlowLayoutPanel gameGroupBox = new FlowLayoutPanel();
                            gameGroupBox.FlowDirection = FlowDirection.TopDown;
                            gameGroupBox.AutoSize = true;
                            gameGroupBox.WrapContents = true;
                            gameGroupBox.Name = _name;
                            //Adds file to game file list
                            gamelist.Add(_isoDir);

                            //Creates a game picture
                            PictureBox gameBox = new PictureBox();

                            gameBox.Height = 200;
                            gameBox.Width = 150;
                            gameBox.SizeMode = PictureBoxSizeMode.StretchImage;
                            gameBox.ImageLocation = _title + @"\art.jpg";
                            //isoPanel.Controls.Add(gameBox);
                            gameBox.MouseDown += gameBox_Click;
                            gameBox.Tag = _isoDir;
                            gameBox.Name = _name;
                            gameGroupBox.Controls.Add(gameBox);

                            //If showtitle is selected, create a label object
                            if (Properties.Settings.Default.showtitle == true)
                            {
                                MaterialLabel gamelabel = new MaterialLabel();
                                gamelabel.Text = _name;
                                gamelabel.Width = 180;
                                gameGroupBox.Controls.Add(gamelabel);
                            }

                            isoPanel.Controls.Add(gameGroupBox);

                            Debug.WriteLine(_name + " has been added");
                        }
                    }

                }

            }

            //Empty list cannot be foreached
            gamelist.Add("null");

            //Every startup look for new .iso files
            if (addgamesDir != "null")
            {
                scanDir();
            }

        }

        private static void copyDLL()
        {
            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"\plugins\");
            //Checks and copies needed plugin files from emulator directory, if they exist

            if(File.Exists(emuDir + @"\plugins\LilyPad.dll"))
            {
                File.Copy(emuDir + @"\plugins\LilyPad.dll", AppDomain.CurrentDomain.BaseDirectory + @"\plugins\LilyPad.dll", true);
            }
            
            if(File.Exists(emuDir + @"\plugins\GSdx32-SSE2.dll"))
            {
                File.Copy(emuDir + @"\plugins\GSdx32-SSE2.dll", AppDomain.CurrentDomain.BaseDirectory + @"\plugins\GSdx32-SSE2.dll", true);
            }

            if(File.Exists(emuDir + @"\plugins\Spu2-X.dll"))
            {
                File.Copy(emuDir + @"\plugins\Spu2-X.dll", AppDomain.CurrentDomain.BaseDirectory + @"\plugins\Spu2-X.dll", true);
            }
        }

        //scan directory for new isos function
        private void scanDir()
        {
            if (Directory.Exists(addgamesDir) == false)
            {
                return;
            }
            foreach (string iso in Directory.GetFiles(addgamesDir + @"\"))
            {

                //iso file name
                string _isoname = iso.Replace(addgamesDir + @"\", String.Empty);

                //Checks if found file is already in game library
                if (gamelist.Contains(iso) == false)
                {
                    //Checks if apropriate file
                    if (_isoname.EndsWith(".iso") || _isoname.EndsWith(".gz") || _isoname.EndsWith(".cso") || _isoname.EndsWith(".bin"))
                    {
                        //if found iso exists in blacklist
                        if (File.ReadAllText(AppDomain.CurrentDomain.BaseDirectory + @"\resources\logs\blacklist.txt").Contains(iso) == false)
                        {
                            //Add game dialog box
                            DialogResult addGame = MessageBox.Show("Do you want to add " + _isoname + "?", "New game found!", MessageBoxButtons.YesNo);
                            if (addGame == DialogResult.Yes)
                            {
                                //adding a game code here
                                string _imgsdir = AppDomain.CurrentDomain.BaseDirectory + @"\resources\images\defbox.gif";

                                //Automatic Game Name for image files, except .cso and bin/cue
                                if ((iso.EndsWith(".cso") == false) && (iso.EndsWith(".bin") == false) && (iso.EndsWith(".gz") == false))
                                {
                                    string _filename;


                                    //Gets the game serial number from file
                                    using (ArchiveFile archiveFile = new ArchiveFile(iso))
                                    {
                                        foreach (Entry entry in archiveFile.Entries)
                                        {
                                            //If any file in archive starts with a regional number, save it in variable

                                            _filename = new string(entry.FileName.Take(4).ToArray());
                                            if (regionList.Contains(_filename))
                                            {
                                                gameserial = entry.FileName.Replace(".", String.Empty);
                                                gameserial = gameserial.Replace("_", "-");

                                                Console.WriteLine("Serial = " + gameserial);
                                            }
                                        }
                                    }

                                    //Gets game name from pcsx2 game db
                                    using (var reader = new StreamReader(Properties.Settings.Default.EmuDir + @"\GameIndex.dbf"))
                                    {
                                        bool serialFound = false;
                                        while (!reader.EndOfStream)
                                        {
                                            var line = reader.ReadLine();
                                            if (line.Contains("Serial = " + gameserial))
                                            {
                                                serialFound = true;
                                            }
                                            else if (serialFound == true)
                                            {
                                                _isoname = line.Replace("Name   = ", String.Empty);
                                                break;
                                            }
                                        }
                                    }

                                    if (Properties.Settings.Default.autoArt == true)
                                    {
                                        Debug.WriteLine("Adding " + _isoname + " to taskQueue!");
                                        taskQueue.Add(new Tuple<String, String>(_isoname, _imgsdir));
                                    }




                                }

                                addIso(_imgsdir, iso, _isoname);

                            }
                            else
                            {
                                //Adds iso path to blacklist
                                System.IO.StreamWriter blacklist = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"\resources\logs\blacklist.txt", true);
                                blacklist.WriteLine(iso);
                                blacklist.Close();
                            }
                        }
                    }
                }
            }
        }


        //Save Settings function
        private static void saveSettings()
        {
            Properties.Settings.Default.EmuDir = emuDir;
            Properties.Settings.Default.gamesDir = addgamesDir;
            Properties.Settings.Default.Save();
        }


        //Opens emulator directory selection dialog
        public static void SelectDir()
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
                    }
                    else
                    {
                        MessageBox.Show("Not a valid emulator directory");
                        goto SelectDir; //retries FolderBrowserDialog
                    }
                }
                else
                {
                    Application.Exit();
                }
            }
        }


        //Main Timer
        private void mainTimer_Tick(object sender, EventArgs e)
        {
            Properties.Settings.Default.lastSize = Convert.ToString(this.Size);
            emuDir = Properties.Settings.Default.EmuDir;
            saveSettings();
        }


        //First Time Setup, should be called only once
        public void FirstTimeSetup(bool _show)
        {

            if (_show == true)
            {
                //Color bgCol = ColorTranslator.FromHtml("#0277BD");
                //welcomeBg.BackColor = bgCol;

                Size = new Size(1122, 720);

                welcomeBg.Visible = true;

                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\resources\images\bg1.png"))
                {
                    welcomeBg.ImageLocation = AppDomain.CurrentDomain.BaseDirectory + @"\resources\images\bg1.png";
                }
                else
                {
                    MessageBox.Show(@"Cannot find \resources\images\bg1.png");
                }

                welcomeBg.Height = this.ClientSize.Height;
                welcomeBg.Width = this.ClientSize.Width;
                Controls.Add(welcomeBg);
                welcomeBg.BringToFront();

                //Welcome Screen selectdir button
                welcomedirbtn.Visible = true;
                welcomedirbtn.Height = 50;
                welcomedirbtn.Width = 140;
                welcomedirbtn.Anchor = AnchorStyles.None;
                welcomedirbtn.Text = "Browse";
                Controls.Add(welcomedirbtn);
                welcomedirbtn.BringToFront();
                welcomedirbtn.MouseDown += welcomedirbtn_click;

                //Centers the button
                welcomedirbtn.Location = new Point((this.ClientSize.Width / 2) - (welcomedirbtn.Width / 2), (this.ClientSize.Height / 2) + (welcomedirbtn.Height / 2));
            }
            else
            {

                welcomeBg.Visible = false;
                welcomedirbtn.Visible = false;
            }
        }

        //First Time Setup "OK" button
        private void welcomedirbtn_click(object sender, EventArgs e)
        {
            SelectDir();
            FirstTimeSetup(false);
            copyDLL();
        }


        //Settings (gears) button
        private void SettingsButton_Click(object sender, EventArgs e)
        {
            //Opens settings form
            settingsForm obj2 = new settingsForm();
            obj2.RefToForm1 = this;

            //Hides this form
            this.Visible = false;

            //Shows settings form
            obj2.Show();
        }


        //Add Game Button
        private void addGameButton_Click(object sender, EventArgs e)
        {
            //Creates a delegate addGameForm
            addGameForm addgame = new addGameForm();
            addgame.ControlCreator = UpdateUiEvent;
            addgame.Show();

        }


        //Add Iso function
        public void addIso(string _img, string _isoDir, string _title)
        {
            //Item properties
            FlowLayoutPanel gameGroupBox = new FlowLayoutPanel();
            gameGroupBox.FlowDirection = FlowDirection.TopDown;
            gameGroupBox.AutoSize = true;
            gameGroupBox.WrapContents = true;
            gameGroupBox.Name = _title;

            PictureBox gameBox = new PictureBox();

            gameBox.Height = 200;
            gameBox.Width = 150;
            gameBox.SizeMode = PictureBoxSizeMode.StretchImage;

            _title = _title.Replace(@"/", string.Empty);
            _title = _title.Replace(@"\", string.Empty);
            _title = _title.Replace(@":", string.Empty);

            //Path to iso from mainForm
            string selfPath = Path.GetDirectoryName(Environment.GetCommandLineArgs()[0]);


            //If boxart exists in folder, then set it in isoPanel
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + _title + @"\art.jpg"))
            {
                gameBox.ImageLocation = AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + _title + @"\art.jpg";
            }
            else
            {
                gameBox.ImageLocation = _img;
            }

            Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + _title);

            //copies pcsx2 inis to added game

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
                    MessageBox.Show("Cannot find default PCSX2 configuration at " + emuDir + @"\inis\");
                    MessageBox.Show("Neither in " + Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\PCSX2\inis");
                }

            }



            gameBox.Name = _title;
            gameBox.MouseDown += gameBox_Click;
            gameBox.Tag = _isoDir;
            gamelist.Add(_isoDir);
            gameGroupBox.Controls.Add(gameBox);

            //If showtitle is selected, create a label object
            if (Properties.Settings.Default.showtitle == true)
            {
                MaterialLabel gamelabel = new MaterialLabel();
                gamelabel.Text = _title;
                gamelabel.Width = 180;
                gameGroupBox.Controls.Add(gamelabel);
            }

            isoPanel.Controls.Add(gameGroupBox);




            Debug.WriteLine("creating a folder at - " + AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + _title);


            using (WebClient client = new WebClient())
            {
                try
                {
                    client.DownloadFile(_img, AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + _title + @"\art.jpg");
                }
                catch
                {
                    //throw;
                    MessageBox.Show("Image not available, none set.");
                }
            }

            var gameIni = new IniFile(AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + _title + @"\spectabis.ini");
            gameIni.Write("isoDirectory", _isoDir, "Spectabis");
            gameIni.Write("nogui", "0", "Spectabis");
            gameIni.Write("fullscreen", "0", "Spectabis");
            gameIni.Write("fullboot", "0", "Spectabis");

            //MessageBox.Show("Please, configure the game");
            //string cfgDir = AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + _title;
            //Process.Start(emuDir + @"\pcsx2.exe", "--cfgpath \"" + cfgDir + "\"");
        }


        //Clicking on game
        private void gameBox_Click(object sender, MouseEventArgs e)
        {

            PictureBox clickedPictureBox = (PictureBox)sender;
            Debug.WriteLine(clickedPictureBox.Name + " - clicked");

            //Saves last picturebox to a variable
            lastGame = (PictureBox)sender;

            //Refresh the image
            clickedPictureBox.ImageLocation = AppDomain.CurrentDomain.BaseDirectory + @"resources\configs\" + clickedPictureBox.Name + @"\art.jpg";

            //Refresh isoDirectory
            string _cfgDir = AppDomain.CurrentDomain.BaseDirectory + @"resources\configs\" + clickedPictureBox.Name;
            var _gameIni = new IniFile(_cfgDir + @"\spectabis.ini");
            var _isoDir = _gameIni.Read("isoDirectory", "Spectabis");
            clickedPictureBox.Tag = _isoDir;




            //Check, if click was left mouse
            if (e.Button == MouseButtons.Left)
            {
                //Checks, if double click
                if (e.Clicks == 2)
                {
                    //If doubleclick setting is false, then stop
                    //This prevents launching multiple instances of game
                    if (Properties.Settings.Default.doubleclick == false)
                    {
                        return;
                    }
                }

                //If single click, with doubleclick setting, then stop
                if (e.Clicks == 1)
                {
                    if (Properties.Settings.Default.doubleclick == true)
                    {
                        return;
                    }
                }

                //Checks, if game file still exists
                if (File.Exists((string)clickedPictureBox.Tag))
                {
                    //Starts the game, if exists
                    string isoDir = (string)clickedPictureBox.Tag;
                    string cfgDir = AppDomain.CurrentDomain.BaseDirectory + @"resources\configs\" + clickedPictureBox.Name;

                    var gameIni = new IniFile(cfgDir + @"\spectabis.ini");
                    var _nogui = gameIni.Read("nogui", "Spectabis");
                    var _fullscreen = gameIni.Read("fullscreen", "Spectabis");
                    var _fullboot = gameIni.Read("fullboot", "Spectabis");
                    var _nohacks = gameIni.Read("nohacks", "Spectabis");


                    string _launchargs = "";

                    if (_nogui == "1")
                    {
                        _launchargs = "--nogui ";
                    }

                    if (_fullscreen == "1")
                    {
                        _launchargs = _launchargs + "--fullscreen ";
                    }

                    if (_fullboot == "1")
                    {
                        _launchargs = _launchargs + "--fullboot ";
                    }

                    if (_nohacks == "1")
                    {
                        _launchargs = _launchargs + "--nohacks ";
                    }

                    Debug.WriteLine(clickedPictureBox.Name + " launched with commandlines:  " + _launchargs);
                    Debug.WriteLine(clickedPictureBox.Name + " launched from: " + isoDir);
                    Debug.WriteLine(emuDir + @"\pcsx2.exe", "" + _launchargs + "\"" + isoDir + "\" --cfgpath \"" + cfgDir + "\"");

                    Process.Start(emuDir + @"\pcsx2.exe", "" + _launchargs + "\"" + isoDir + "\" --cfgpath \"" + cfgDir + "\"");
                    return;
                }
                else
                {
                    MessageBox.Show("Huh, the game doesn't exist", ":(");
                }
            }

            //Check, if click was right mouse
            if (e.Button == MouseButtons.Right)
            {
                //Displays context menu
                contextMenu.Show(Cursor.Position.X, Cursor.Position.Y);
            }
        }


        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Deletes last picturebox in isoPanel
            Directory.Delete(AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + lastGame.Name, true);

            var isoPanelList = isoPanel.Controls.OfType<Control>();
            foreach (var child in isoPanelList)
            {
                if (child.Name == lastGame.Name)
                {
                    isoPanel.Controls.Remove(child);
                }
            }

            //Removes the game front gamelist
            string cfgDir = AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + lastGame.Name;
            var gameIni = new IniFile(cfgDir + @"\spectabis.ini");
            var _gamedir = gameIni.Read("IsoDirectory", "Spectabis");
            gamelist.Remove(_gamedir);

            lastGame = null;
        }


        private void configureToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.lastGameEdit = lastGame.Name;
            Properties.Settings.Default.Save();

            //Opens game settings form
            gameSettings gameSettings = new gameSettings();
            gameSettings.RefToForm2 = this;

            //Hides this form
            this.Visible = false;

            //Shows game settings form
            gameSettings.Show();
        }


        private void emulatorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string cfgDir = AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + lastGame.Name;
            Process.Start(emuDir + @"\pcsx2.exe", "--cfgpath \"" + cfgDir + "\"");
        }


        private void AddDirectoryButton_Click(object sender, EventArgs e)
        {
            //Shows the notice, if another directory is already set
            if (addgamesDir != "null")
            {
                MessageBox.Show("Proceeding will overwrite your current active game directory.");
            }

            //Shows the folder browser dialog
            using (FolderBrowserDialog fbd = new FolderBrowserDialog() { Description = "Select where your game files are located." })
            {
                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    addgamesDir = fbd.SelectedPath;
                    scanDir();
                }
            }
        }

        //refresh game list button
        private void refreshBtn_Click(object sender, EventArgs e)
        {
            scanDir();
        }

        //Automatic box art scanner method
        private void doArtScrapping(string _isoname, string _imgsdir)
        {
            try
            {
                string _databaseurl = "http://thegamesdb.net/";

                Debug.WriteLine("Starting ArtScrapping for " + _isoname);

                //WebRequest.Create(_databaseurl).GetResponse();
                string _title;
                this.Invoke((MethodInvoker)delegate 
                {
                    currentTask.Text = "Searching box art for " + _isoname + "...";
                });

                foreach (GameSearchResult game in GamesDB.GetGames(_isoname, "Sony Playstation 2"))
                {

                    //Gets game's database ID
                    Game newGame = GamesDB.GetGame(game.ID);
                    //Trim title
                    _title = game.Title.Replace(":", "");
                    _title = game.Title.Replace(@"/", "");
                    _title = game.Title.Replace(@".", "");
                    //Sets image
                    _imgsdir = "http://thegamesdb.net/banners/" + newGame.Images.BoxartFront.Path;

                    Debug.WriteLine(_isoname + " box art found!");

                    if(_isoname.Contains(_title))
                    {
                        this.Invoke((MethodInvoker)delegate {
                            currentTask.Text = "Found box art for " + _isoname + "...";
                        });
                    }
                    else
                    {
                        this.Invoke((MethodInvoker)delegate {
                            currentTask.Text = "TheGamesDB screwed up again and didn't return the proper art...";
                        });
                        Thread.Sleep(1000);
                    }
                   

                    //Stops at the first game
                    break;
                }

                //Downloads the image
                using (WebClient client = new WebClient())
                {
                    try
                    {
                        this.Invoke((MethodInvoker)delegate {
                            currentTask.Text = "Downloading box art for " + _isoname + "...";
                        });

                        client.DownloadFile(_imgsdir, AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + _isoname + @"\art.jpg");

                        this.Invoke((MethodInvoker)delegate {
                            currentTask.Text = "Box art for " + _isoname + " downloaded!";
                        });

                        //Lists all games in isoPanel
                        List<Control> listGames = isoPanel.Controls.Cast<Control>().ToList();

                        foreach (Control gametile in listGames)
                        {
                            if (gametile.Name == _isoname)
                            {
                                this.Invoke((MethodInvoker)delegate {
                                    currentTask.Text = "Setting box art for  " + _isoname;
                                });

                                PictureBox currentPicturebox = (PictureBox)gametile;
                                currentPicturebox.ImageLocation = AppDomain.CurrentDomain.BaseDirectory + @"\resources\configs\" + _isoname + @"\art.jpg";
                            }
                        }

                    }
                    catch
                    {
                        Debug.WriteLine("Could not download the image!");
                        this.Invoke((MethodInvoker)delegate {
                            currentTask.Text = "Could not download box art for " + _isoname;
                        });
                    }

                    this.Invoke((MethodInvoker)delegate {
                        currentTask.Text = String.Empty;
                    });
                    _resetEvent.Set();
                }


            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                this.Invoke((MethodInvoker)delegate {
                    currentTask.Text = "TheGamesDB is unreachable!";
                });
                return;
            }
        }

        //file drag effect
        private void isoPanel_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.All;
        }

        //file drop functionality
        private void isoPanel_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop, false);
            string _filename;
            string _isoname;
            string _imgsdir = AppDomain.CurrentDomain.BaseDirectory + @"\resources\images\defbox.gif";

            foreach (string file in files)
            {
                if((file.EndsWith(".iso") == false) && (file.EndsWith(".cso") == false) && (file.EndsWith(".mdf") == false) && (file.EndsWith(".gz") == false) && (file.EndsWith(".bin") == false))
                {
                    currentTask.Text = file + " is not a valid file!";
                    return;
                }

                _isoname = Path.GetFileNameWithoutExtension(file);

                if ((file.EndsWith(".bin") == false) && (file.EndsWith(".cso") == false) && (file.EndsWith(".gz") == false))
                {
                    //Gets the game serial number from file
                    using (ArchiveFile archiveFile = new ArchiveFile(file))
                    {
                        foreach (Entry entry in archiveFile.Entries)
                        {
                            //If any file in archive starts with a regional number, save it in variable

                            _filename = new string(entry.FileName.Take(4).ToArray());
                            if (regionList.Contains(_filename))
                            {
                                gameserial = entry.FileName.Replace(".", String.Empty);
                                gameserial = gameserial.Replace("_", "-");

                                Console.WriteLine("Serial = " + gameserial);
                            }
                        }
                    }

                    Debug.WriteLine("Getting gamename from PCSX2DB");

                    //Gets game name from pcsx2 game db
                    using (var reader = new StreamReader(Properties.Settings.Default.EmuDir + @"\GameIndex.dbf"))
                    {
                        bool serialFound = false;
                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            if (line.Contains("Serial = " + gameserial))
                            {
                                serialFound = true;
                            }
                            else if (serialFound == true)
                            {
                                _isoname = line.Replace("Name   = ", String.Empty);
                                Debug.WriteLine(_isoname);
                                break;
                            }
                        }
                    }

                    Debug.WriteLine("Preparing to add " + _isoname + " to taskQueue");

                    //if drag&drop auto art is enabled, search for art
                    if(Properties.Settings.Default.dropautoart == true)
                    {
                        //Art scrapper run in another thread
                        //Pings gamesDB and downloads box art cover

                        //Task DRAGartscrapper = new Task(() => doArtScrapping(_isoname, _imgsdir));
                        //DRAGartscrapper.Start();

                        //Adds the game to taskQueue list
                        Debug.WriteLine("Adding " + _isoname + " to taskQueue!");
                        taskQueue.Add(new Tuple<String, String>(_isoname, _imgsdir));
                    }

                }

                addIso(_imgsdir, file, _isoname);

            }
        }

        //Refreshes box art timer
        private void refreshArt_Tick(object sender, EventArgs e)
        {
            //loops all controls in isopanel
            foreach (Control child in isoPanel.Controls)
            {
                //loops through the items in found control
                foreach (var art in child.Controls)
                {
                    //if found item is an image
                    if (Convert.ToString(art.GetType()) == "System.Windows.Forms.PictureBox")
                    {               
                        //set the image     
                        PictureBox artbox = (PictureBox)art;
                        artbox.ImageLocation = AppDomain.CurrentDomain.BaseDirectory + @"resources\configs\" + child.Name + @"\art.jpg";
                    }
                    
                }
            }
        }

        //timer for async task list
        private void taskList_Tick(object sender, EventArgs e)
        {
            //Checks if taskQueue isn't empty
            Debug.WriteLine("Checking, if taskList has any values");
            if(taskQueue.Any())
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
            //loop all values in taskQueue list
            foreach (var task in taskQueue)
            {
                string _isoname = task.Item1;
                string _imgsdir = task.Item2;

                //Removes the game from taskQueue list
                taskQueue.Remove(task);

                //does artscrapping on another thread with values
                doArtScrapping(_isoname, _imgsdir);

                //stops at first value
                break;
            }
        }

        //Menu Button
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            PictureBox menuBackground = new PictureBox();
            menuBackground.BackColor = Color.White;
            menuBackground.Location = new Point(this.Height, 0);
            menuBackground.Width = 200;
            menuBackground.Height = this.Height;
        }
    }
}