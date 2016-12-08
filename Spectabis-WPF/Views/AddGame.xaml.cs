using System;
using System.Net.Cache;
using System.Windows;
using System.Windows.Controls;

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

            if(Properties.Settings.Default.autoBoxart == true)
            {
                AutoBoxart.IsEnabled = true;
            }
            else
            {
                AutoBoxart.IsEnabled = false;
            }
        }

        string imageSource;

        //Browse game
        private void SelectGame_Click(object sender, RoutedEventArgs e)
        {
            Ookii.Dialogs.Wpf.VistaOpenFileDialog gameBrowser = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();
            gameBrowser.Filter = "ISO File|*.iso|GZ File|*.gz|CSO File|*.cso|BIN File|*.bin";

            var gameResult = gameBrowser.ShowDialog();
            if(gameResult.Value == true)
            {
                CompleteButton.IsEnabled = true;
            }

        }

        //Select boxart Image button
        private void SelectBoxart_Click(object sender, RoutedEventArgs e)
        {
            Ookii.Dialogs.Wpf.VistaOpenFileDialog imageBrowser = new Ookii.Dialogs.Wpf.VistaOpenFileDialog();
            imageBrowser.Filter = "JPEG Image|*.jpg|PNG Image|*.png";

            //Open dialog
            var imageResult = imageBrowser.ShowDialog();
            if(imageResult.Value == true)
            {
                imageSource = imageBrowser.FileName;

                //Reload the image in boxart preview
                System.Windows.Media.Imaging.BitmapImage artSource = new System.Windows.Media.Imaging.BitmapImage();
                artSource.BeginInit();
                artSource.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.None;
                artSource.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
                artSource.CreateOptions = System.Windows.Media.Imaging.BitmapCreateOptions.IgnoreImageCache;
                artSource.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
                artSource.UriSource = new Uri(imageSource);
                artSource.EndInit();

                BoxPreview.Source = artSource;
            }
        }

        //Add game to library and create a profile
        private void Complete_Click(object sender, RoutedEventArgs e)
        {


            
            //Invokes mainWindow class which reloads game library
            ((MainWindow)Application.Current.MainWindow).reloadLibrary();
        }
    }
}
