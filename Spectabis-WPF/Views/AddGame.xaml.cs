using System.Windows;
using System.Windows.Controls;
using Spectabis_WPF.Domain;
using Ookii.Dialogs.Wpf;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Windows.Media.Animation;
using System;

namespace Spectabis_WPF.Views
{
    /// <summary>
    /// Interaction logic for AddGame.xaml
    /// </summary>
    public partial class AddGame : Page
    {
        public AddGame()
        {
            InitializeComponent();
        }

        private void ISOBrowser_Button_Click(object sender, RoutedEventArgs e)
        {
            VistaOpenFileDialog ISODialog = new VistaOpenFileDialog();
            ISODialog.Title = "Navigate to game file!";

            //Catch, when file filter is empty for future reference
            var empty = ISODialog.Filter;

            foreach (var type in SupportedGames.GameFiles)
            {
                //Forge a file dialog Filter Entry
                string fileType = type.ToLower();
                string FilterEntry = $"{fileType} Files (*.{fileType})|*.{fileType}";

                if (ISODialog.Filter == empty)
                {
                    //If filter has no entries, simply add one
                    ISODialog.Filter = FilterEntry;
                }
                else
                {
                    //If filter has entries, check if current entry already exists
                    if (ISODialog.Filter.ToString().Contains(fileType) == false)
                    {
                        //If current entry doesn't exist, forge an entry and add it to filter
                        ISODialog.Filter = ISODialog.Filter + "|" + FilterEntry;
                    }
                }
            
            }
            if (ISODialog.ShowDialog().Value == true)
            {
                var serial = GetSerial.GetSerialNumber(ISODialog.FileName);
                var title = GetGameName.GetName(serial);
                var file = ISODialog.FileName;

                //If title was not gotten, set it from file
                if(title == "UNKNOWN")
                {
                    title = Path.GetFileNameWithoutExtension(file);
                }

                GameProfile.Create(null, file, title);

                //Change initial button and text
                ISOBrowser_Text.Text = $"{title} was added!";
                ISOBrowser_Button.IsEnabled = false;

                FadeButton();
                SlideText();
                
            }
        }

        private void FadeButton()
        {
            //Fade button out
            DoubleAnimation da = new DoubleAnimation();
            da.From = 1;
            da.To = 0;
            da.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            ISOBrowser_Button.BeginAnimation(Button.OpacityProperty, da);
        }

        private void SlideText()
        {

        }
    }
}
