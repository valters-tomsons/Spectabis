using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;

namespace Spectabis_WPF.Views
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
            SearchBar.IsChecked = Properties.Settings.Default.searchbar;
            Tooltips.IsChecked = Properties.Settings.Default.tooltips;
            checkUpdates.IsChecked = Properties.Settings.Default.checkupdates;
            TitleAsFile.IsChecked = Properties.Settings.Default.titleAsFile;

            //Console.WriteLine("GiantBomb API Key: " + Properties.Settings.Default.APIKey_GiantBomb);
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
            Properties.Settings.Default.searchbar = Convert.ToBoolean(SearchBar.IsChecked);
            Properties.Settings.Default.tooltips = Convert.ToBoolean(Tooltips.IsChecked);
            Properties.Settings.Default.checkupdates = Convert.ToBoolean(checkUpdates.IsChecked);
            Properties.Settings.Default.titleAsFile = Convert.ToBoolean(TitleAsFile.IsChecked);

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
            Console.WriteLine("Settings Saved");

            //Load Nightmode
            new PaletteHelper().SetLightDark(Properties.Settings.Default.nightMode);

            ShowAPISelection();
        }

        //Save Settings when checkbox is clicked
        private void checkbox_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();
        }

        //Show giantbomb warning dialog
        //if bool is true, hide the warning
        private void ShowGiantBombWarning([Optional]bool hide)
        {
            GiantBombWarning.Visibility = Visibility.Visible;
            WarningContent.Visibility = Visibility.Visible;
            GiantBombWarning.ShowDialog(WarningContent);
            if(hide == true)
            {
                GiantBombWarning.Visibility = Visibility.Collapsed;
                WarningContent.Visibility = Visibility.Collapsed;
            }
        }

        //giantbomb warning dialog "yes button"
        private void Giantbomb_Yes(object sender, RoutedEventArgs e)
        {
            ShowGiantBombWarning(true);
            Process.Start("http://www.giantbomb.com/api/");
        }

        //giantbomb warning dialog "no button"
        private void Giantbomb_No(object sender, RoutedEventArgs e)
        {
            ShowGiantBombWarning(true);
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
                    showApiSettings(false);
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
                showApiSettings(false);
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
                    ShowGiantBombWarning();
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
            Console.WriteLine(Api_Box.Text);
            SaveSettings();
        }

        //PCSX2 directory button 
        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            Ookii.Dialogs.Wpf.VistaFolderBrowserDialog BrowserDialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            BrowserDialog.Description = "Select PCSX2 Directory";
            BrowserDialog.UseDescriptionForTitle = true;

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
