using System.Windows;
using System.Windows.Controls;

namespace Spectabis_WPF
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
                emuBrowser.Filter = "PCSX2 Executable | PCSX2.exe";

                //Show dialog
                var browserResult = emuBrowser.ShowDialog();
                if (browserResult.Value == true)
                {
                    string _result = emuBrowser.FileName;

                    //If filename contains ".exe", just in case
                    if (_result.Contains(".exe") == false)
                    {
                        MessageBox.Show("Invalid file.");
                        return;
                    }

                    //Save selected path, remove pcsx2.exe from string
                    Properties.Settings.Default.emuDir = _result.Replace("pcsx2.exe", string.Empty);
                    Properties.Settings.Default.Save();

                    //Increment Step count
                    StepCounter = +1;

                    //Set up next step text

                    //Hide this page and go to Spectabis
                    ((MainWindow)Application.Current.MainWindow).HideFirsttimeSetup();

                }
            }


            
        }
    }
}
