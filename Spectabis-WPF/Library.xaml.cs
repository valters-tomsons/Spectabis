using System;
using System.Diagnostics;
using System.IO;
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

        public Library()
        {
            InitializeComponent();

            //DEV VALUES
            GameConfigs = @"D:\Program Files (x86)\Spectabis\resources\configs";
            emuDir = @"D:\Program Files (x86)\PCSX2";

            reloadGames();
        }


        //MouseDown event on boxArt image
        private void boxArt_Click(object sender, MouseButtonEventArgs e)
        {
            Image clickedBoxArt = (Image)sender;
            Debug.WriteLine(clickedBoxArt.Tag + " - clicked");

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
                //code
            }



        }

        //Rescans the game config directory and adds them to gamePanel
        private void reloadGames()
        {
            //Makes a collection of game folders from game config directory
            string[] _gamesdir = Directory.GetDirectories(GameConfigs);

            //Loops through each folder in game config directory
            foreach (string game in _gamesdir)
            {
                if (File.Exists(game + @"\Spectabis.ini"))
                {
                    //Sets _gameName to name of the folder
                    string _gameName = game.Remove(0, game.LastIndexOf(System.IO.Path.DirectorySeparatorChar) + 1);

                    Debug.WriteLine("adding to gamePanel - " + _gameName);

                    //Creates an image object
                    Image boxArt = new Image();
                    boxArt.Source = new ImageSourceConverter().ConvertFromString(game + @"\art.jpg") as ImageSource;
                    boxArt.Height = 200;
                    boxArt.Width = 150;
                    boxArt.MouseDown += boxArt_Click;
                    boxArt.Tag = _gameName;

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
    }
}
