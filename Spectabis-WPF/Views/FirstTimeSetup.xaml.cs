using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace Spectabis_WPF.Views
{
    /// <summary>
    /// Interaction logic for FirstTimeSetup.xaml
    /// </summary>
    public partial class FirstTimeSetup : Page
    {
        public FirstTimeSetup()
        {
            InitializeComponent();
        }

        //Keeps track of setup steps
        public int StepCounter = 0;

        //Primary button click
        private void PrimaryButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            //Select emulator directory step
            if(StepCounter == 0)
            {
                Ookii.Dialogs.Wpf.VistaOpenFileDialog emuBrowser = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();
                emuBrowser.Filter = "PCSX2 Executable | *.exe|Executable|*.exe";

                //Show dialog
                var browserResult = emuBrowser.ShowDialog();
                if (browserResult.Value == true)
                {
                    string _result = emuBrowser.FileName;

                    if (_result.ToLower().Contains("pcsx2") == false || _result.ToLower().EndsWith(".exe") == false)
                    {
                        MessageBox.Show("Invalid file.");
                        return;
                    }

                    //Save selected path, *don't* remove pcsx2.exe from string
                    Properties.Settings.Default.emuDir = _result;
                    Properties.Settings.Default.Save();

                    //Increment Step count
                    StepCounter += 1;


                    // disabled
                    if (false) {
                        //Set up next step
                        BigLogo.Visibility = Visibility.Collapsed;
                        DownloadLabel.Visibility = Visibility.Collapsed;

                        MainLabel.Content = "Select Boxart searching method";
                        TGDB_Description.Visibility = Visibility.Visible;

                        PrimaryButton.Content = "Continue";
                        API_RadioPanel.Visibility = Visibility.Visible;
                        return;
                    }
                }
            }

            //Select API step
            //// Cyberfoxhax<08-July-2018>: disable this step entirely 
            if(false)
            //if(StepCounter == 1)
            {
                //If Giantbomb is selected, check if API key is entered
                if (GB_Radio.IsChecked == true)
                {
                    if(ApiBox.Text.Length != 40)
                    {
                        MessageBox.Show("API Key not valid lenght");
                        return;
                    }


                    //Save settings
                    Properties.Settings.Default.artDB = "GiantBomb";
                    Properties.Settings.Default.APIKey_GiantBomb = ApiBox.Text;
                    Properties.Settings.Default.Save();

                }
                else if (TGDB_Radio.IsChecked == true)
                {
                    //Save settings
                    Properties.Settings.Default.artDB = "TheGamesDB";
                    Properties.Settings.Default.Save();
                }
            }

            //Hide this page and go to Spectabis
            ((MainWindow)Application.Current.MainWindow).HideFirsttimeSetup();
        }

        //Hyperlink in Giantbomb API description
        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("http://giantbomb.com/api/");
        }

        //Radio buttons for API Selection
        private void APIClick(object sender, RoutedEventArgs e)
        {
            GB_Description.Visibility = Visibility.Collapsed;
            TGDB_Description.Visibility = Visibility.Collapsed;

            ApiBox.Visibility = Visibility.Collapsed;

            //Detect radio button and enable things and stuff
            if (GB_Radio.IsChecked == true)
            {
                GB_Description.Visibility = Visibility.Visible;
                ApiBox.Visibility = Visibility.Visible;
                if(ApiBox.Text.Length != 40)
                {
                    PrimaryButton.IsEnabled = false;
                }
                else
                {
                    PrimaryButton.IsEnabled = true;
                }
                
            }
            else if(TGDB_Radio.IsChecked == true)
            {
                TGDB_Description.Visibility = Visibility.Visible;
                PrimaryButton.IsEnabled = true;
            }
        }

        //"Get PCSX2" at bottom label
        private void Label_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Process.Start("http://buildbot.orphis.net/pcsx2/index.php/");
        }

        //Disable Button when lenght is not 40
        private void ApiBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (ApiBox.Text.Length == 40)
            {
                PrimaryButton.IsEnabled = true;
            }
            else
            {
                PrimaryButton.IsEnabled = false;
            }
        }

        //Underline PCSX2 label on hover
        private void Label_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DownloadLabel.TextDecorations = TextDecorations.Underline;
            DownloadLabel.FontStyle = FontStyles.Italic;
        }

        private void Label_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            DownloadLabel.TextDecorations = null;
            DownloadLabel.FontStyle = FontStyles.Normal;
        }
    }
}
