using Spectabis_WPF.Domain;
using System;
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

        private Process _wizardProcess;

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

                    Properties.Settings.Default.emuDir = _result;

                    BigLogo.Visibility = Visibility.Collapsed;
                    DownloadLabel.Visibility = Visibility.Collapsed;

                    SecondaryButton.Visibility = Visibility.Visible;

                    MainLabel.Content = "PCSX2 First Time Setup";

                    Subtitle.Content = "It is recommended to finish first time wizard to create a base configuration";
                    Subtitle.Visibility = Visibility.Visible;

                    PrimaryButton.Content = "Begin";

                    StepCounter += 1;
                    return;
                }
            }

            if(StepCounter == 1)
            {
                PrimaryButton.Visibility = Visibility.Collapsed;
                SecondaryButton.Content = "Finish";

                MainLabel.Content = "Finish!";
                Subtitle.Content = "Click below when finished with the wizard";

                _wizardProcess = LaunchPCSX2.CreateFirstTimeWizard();
                _wizardProcess.Start();

                StepCounter += 1;
                return;
            }
        }

        private void SecondaryButton_Click(object sender, RoutedEventArgs e)
        {
            if(_wizardProcess != null)
            {
                try
                {
                    _wizardProcess.Kill();
                }
                catch
                {
                    Console.WriteLine("Failed to kill wizard process");
                }
            }

            Properties.Settings.Default.Save();
            ((MainWindow)Application.Current.MainWindow).HideFirsttimeSetup();
        }

        //"Get PCSX2" at bottom label
        private void Label_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Process.Start("https://pcsx2.net/download/releases/windows/category/40-windows.html");
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
