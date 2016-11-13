using MaterialDesignThemes.Wpf;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Spectabis_WPF
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {

        public Settings()
        {
            InitializeComponent();

            //Loads values
            doubleclick.IsChecked = Properties.Settings.Default.doubleClick;
            showTitles.IsChecked = Properties.Settings.Default.showTitle;
            nightMode.IsChecked = Properties.Settings.Default.nightMode;
            AutoScrapping.IsChecked = Properties.Settings.Default.autoBoxart;

            Debug.WriteLine("GiantBomb API Key: " + Properties.Settings.Default.APIKey_GiantBomb);
            Api_Box.Text = Properties.Settings.Default.APIKey_GiantBomb;

            emudir_text.Text = Properties.Settings.Default.emuDir;



            ShowAPISelection();
        }

        //Saves Settings
        private void SaveSettings()
        {
            //Sets values to save
            Properties.Settings.Default.doubleClick = Convert.ToBoolean(doubleclick.IsChecked);
            Properties.Settings.Default.showTitle = Convert.ToBoolean(showTitles.IsChecked);
            Properties.Settings.Default.nightMode = Convert.ToBoolean(nightMode.IsChecked);
            Properties.Settings.Default.autoBoxart = Convert.ToBoolean(AutoScrapping.IsChecked);

            Properties.Settings.Default.APIKey_GiantBomb = Api_Box.Text;

            if(thegamesdb_radio.IsChecked == true)
            {
                Properties.Settings.Default.artDB = "TheGamesDB";
            }
            else if(giantbomb_radio.IsChecked == true)
            {
                Properties.Settings.Default.artDB = "GiantBomb";
            }

            //Save settings
            Properties.Settings.Default.Save();
            Debug.WriteLine("Settings Saved");

            //Load Nightmode
            new PaletteHelper().SetLightDark(Properties.Settings.Default.nightMode);

            ShowAPISelection();
        }

        //Save Settings when checkbox is clicked
        private void checkbox_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();
        }

        //Save PCSX2 directory button
        private void Save_Directory(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(emudir_text.Text))
            {
                if (File.Exists(emudir_text.Text + @"/pcsx2.exe"))
                {
                    Properties.Settings.Default.emuDir = emudir_text.Text;
                    Properties.Settings.Default.Save();

                    PushSnackbar("Emulator directory saved");
                }
                else
                {
                    PushSnackbar("Directory must contain PCSX2.exe");
                }
            }
            else
            {
                PushSnackbar("Directory does not exist");
            }
        }

        //Push snackbar function
        public void PushSnackbar(string message)
        {
            var messageQueue = Snackbar.MessageQueue;

            //the message queue can be called from any thread
            Task.Factory.StartNew(() => messageQueue.Enqueue(message));
        }

        //Shows the gamedb selection panel, if autoscrapping is enabled
        private void ShowAPISelection()
        {
            
            if (AutoScrapping.IsChecked == true)
            {
                Database_API_Stackpanel.IsEnabled = true;
                Database_API_Stackpanel.Visibility = Visibility.Visible;

                showApiSettings(false);

                //See which database API is selected and set radio button
                if (Properties.Settings.Default.artDB == "TheGamesDB")
                {
                    thegamesdb_radio.IsChecked = true;
                }
                else if (Properties.Settings.Default.artDB == "GiantBomb")
                {
                    giantbomb_radio.IsChecked = true;
                    showApiSettings(true);
                }

            }
            else
            {
                Database_API_Stackpanel.IsEnabled = false;
                Database_API_Stackpanel.Visibility = Visibility.Collapsed;
            }
        }

        //Click on API radio buttons
        private void artDB_click(object sender, RoutedEventArgs e)
        {
            //Checks, which radiobutton was clicked
            var clickedRadio = (RadioButton)sender;
            //If giantbomb_was clicked
            if(clickedRadio.Name == "giantbomb_radio")
            {
                //Proceed only, if GiantBomb wasn't checked before clicking
                if(Properties.Settings.Default.artDB != "GiantBomb")
                {
                    if(MessageBox.Show(@"GiantBomb API is faster and reliable. Sadly, you must register and get an API key from GiantBomb. Register now?", "It's better, trust me.", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        Process.Start(@"http://www.giantbomb.com/api/");
                    }
                }
            }

            SaveSettings();
        }

        //Show API Settings, use bool to show=true or hide=false
        private void showApiSettings(bool e)
        {
            if(e == true)
            {
                //Show Api key textbox
                APISettings_Panel.IsEnabled = true;
                APISettings_Panel.Visibility = Visibility.Visible;
            }
            else
            {
                //Show Api key textbox
                APISettings_Panel.IsEnabled = false;
                APISettings_Panel.Visibility = Visibility.Collapsed;
            }
        }

        //Save API key when textbox loses focus
        private void Api_Box_LostFocus(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(Api_Box.Text);
            SaveSettings();
        }

        //PCSX2 directory button 
        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            Ookii.Dialogs.Wpf.VistaFolderBrowserDialog BrowserDialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();

            //Create a return point, if selected path is invalid
            ShowDialog:
            //Show the dialog and check, if directory contains pcsx2.exe
            var BrowserResult = BrowserDialog.ShowDialog();
            //If OK was clicked...
            if(BrowserResult == true)
            {
                if (File.Exists(BrowserDialog.SelectedPath + @"/pcsx2.exe") == false)
                {
                    //If directory isn't PCSX2's, fall back to beginning
                    PushSnackbar("Invalid Emulator Directory");
                    goto ShowDialog;
                }
                //Set emudir textbox to location of selected directory
                emudir_text.Text = BrowserDialog.SelectedPath;
            }
        }
    }
}
