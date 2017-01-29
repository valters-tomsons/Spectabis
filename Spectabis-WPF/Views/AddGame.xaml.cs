using System.Windows;
using System.Windows.Controls;
using Spectabis_WPF.Domain;
using Ookii.Dialogs.Wpf;
using System.IO;
using System.Windows.Media.Animation;
using System;
using System.Net.Cache;
using System.Diagnostics;
using System.Windows.Input;
using System.Windows.Media;

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
            PartTwo_Grid.Opacity = 0;
        }

        private string title;
        private string BaseDirectory = MainWindow.BaseDirectory;

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
                title = GetGameName.GetName(serial);
                var file = ISODialog.FileName;

                //If title was not extracted, set it from file
                if(title == "UNKNOWN")
                {
                    title = Path.GetFileNameWithoutExtension(file);
                }

                title = GameProfile.Create(null, file, title);

                //Change initial button and text
                ISOBrowser_Text.Text = $"{title} was added!";
                ISOBrowser_Button.IsEnabled = false;

                FadeButton();
                FadeText();
                FadeGrid();

                Name_Textbox.Text = title;

                ScrapeArt scraper = new ScrapeArt(title);
                RefreshBox();
            }
        }

        private void RefreshBox()
        {
            System.Windows.Media.Imaging.BitmapImage artSource = new System.Windows.Media.Imaging.BitmapImage();

            artSource.BeginInit();

            //Fixes the caching issues, where cached copy would just hang around and bother me for two days
            artSource.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.None;
            artSource.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            artSource.CreateOptions = System.Windows.Media.Imaging.BitmapCreateOptions.IgnoreImageCache;

            artSource.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
            artSource.UriSource = new Uri(BaseDirectory + @"resources\configs\" + title + @"\art.jpg", UriKind.RelativeOrAbsolute);
            //Closes the filestream
            artSource.EndInit();

            BoxArt.Source = artSource;
        }

        //Change boxart
        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            VistaOpenFileDialog artBrowser = new VistaOpenFileDialog();
            artBrowser.Filter = "JPEG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png";
            artBrowser.Title = "Browse to a new boxart image";

            if(artBrowser.ShowDialog().Value == true)
            {
                File.Copy(artBrowser.FileName, BaseDirectory + @"resources\configs\"+ title +@"\art.jpg", true);

                Uri _img = new Uri(BaseDirectory + @"resources\configs\" + title + @"\art.jpg");
                CacheImage(_img);
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(Name_Textbox.Text);
            Debug.WriteLine(BaseDirectory + @"\resources\configs\" + Name_Textbox.Text);
            GameProfile.Delete(Name_Textbox.Text);
            ((MainWindow)Application.Current.MainWindow).Open_Library();

        }

        private void CacheImage(Uri _img)
        {
            //Creates a bitmap stream
            System.Windows.Media.Imaging.BitmapImage artSource = new System.Windows.Media.Imaging.BitmapImage();
            //Opens the filestream
            artSource.BeginInit();

            //Fixes the caching issues, where cached copy would just hang around and bother me for two days
            artSource.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.None;
            artSource.UriCachePolicy = new RequestCachePolicy(RequestCacheLevel.BypassCache);
            artSource.CreateOptions = System.Windows.Media.Imaging.BitmapCreateOptions.IgnoreImageCache;

            artSource.CacheOption = System.Windows.Media.Imaging.BitmapCacheOption.OnLoad;
            artSource.UriSource = _img;
            //Closes the filestream
            artSource.EndInit();

            //sets boxArt source to created bitmap
            BoxArt.Source = artSource;
        }

        //Fade browse button out
        private void FadeButton()
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 1;
            da.To = 0;
            da.Duration = new Duration(TimeSpan.FromSeconds(0.5));
            ISOBrowser_Button.BeginAnimation(Button.OpacityProperty, da);
        }

        //Fade browse text out
        private void FadeText()
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 1;
            da.To = 0;
            da.Duration = new Duration(TimeSpan.FromSeconds(1.5));
            ISOBrowser_Text.BeginAnimation(TextBlock.OpacityProperty, da);
        }

        //Fade-in Step two grid
        private void FadeGrid()
        {
            DoubleAnimation da = new DoubleAnimation();
            da.From = 0;
            da.To = 1;
            da.Duration = new Duration(TimeSpan.FromSeconds(1));
            PartTwo_Grid.BeginAnimation(Grid.OpacityProperty, da);
        }

        //Game title textbox
        private void Name_Textbox_LostFocus(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(title);
            if (title != Name_Textbox.Text)
            {
                title = GameProfile.Rename(title, Name_Textbox.Text);
                Name_Textbox.Text = title;
                Debug.WriteLine(title);
            }
        }

        private void Name_Textbox_KeyDown(object sender, KeyEventArgs e)
        {
            //Cancel title change
            if(e.Key == Key.Escape)
            {
                Name_Textbox.Text = title;
                MoveFocus(e);
                e.Handled = true;
            }

            //Accept title change
            if(e.Key == Key.Enter)
            {
                title = Name_Textbox.Text;
                MoveFocus(e);
                e.Handled = true;
            }
        }

        //Remove focus from textbox
        private void MoveFocus(KeyEventArgs e)
        {
            //http://stackoverflow.com/questions/8203329/moving-to-next-control-on-enter-keypress-in-wpf

            FocusNavigationDirection focusDirection = FocusNavigationDirection.Next;
            TraversalRequest request = new TraversalRequest(focusDirection);
            UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;

            // Change keyboard focus.
            if (elementWithFocus != null)
            {
                if (elementWithFocus.MoveFocus(request)) e.Handled = true;
            }

        }
    }
}
