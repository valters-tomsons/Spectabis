using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
            
            emudir_text.Text = Properties.Settings.Default.emuDir;
        }

        //Saves Settings
        private void SaveSettings()
        {
            //Sets values to save
            Properties.Settings.Default.doubleClick = Convert.ToBoolean(doubleclick.IsChecked);
            Properties.Settings.Default.showTitle = Convert.ToBoolean(showTitles.IsChecked);
            Properties.Settings.Default.nightMode = Convert.ToBoolean(nightMode.IsChecked);
            Properties.Settings.Default.autoBoxart = Convert.ToBoolean(AutoScrapping.IsChecked);

            //Save settings
            Properties.Settings.Default.Save();
            Debug.WriteLine("Settings Saved");

            //Load Nightmode
            new PaletteHelper().SetLightDark(Properties.Settings.Default.nightMode);
        }

        //Save Settings when checkbox is clicked
        private void checkbox_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();
        }

        //Save PCSX2 directory button
        private void Save_Directory(object sender, RoutedEventArgs e)
        {
            if(Directory.Exists(emudir_text.Text))
            {
                if(File.Exists(emudir_text.Text + @"/pcsx2.exe"))
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
    }
}
