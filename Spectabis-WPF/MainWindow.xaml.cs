using MahApps.Metro.Controls;
using MaterialDesignThemes.Wpf;
using System;
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
                //Show the panel and overlay
                Overlay(true);
                GameSettings.Opacity = 1;
                GameSettings.IsHitTestVisible = true;

                //Set image and header text for the game
                Header_title.Content = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(_name);
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
            //probably some code to save stuff

            //Hide panel
            Overlay(false);
            GameSettings.Opacity = 0;
            GameSettings.IsHitTestVisible = false;
        }
    }
}