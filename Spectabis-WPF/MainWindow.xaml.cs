using System;
using System.Windows;
using System.Windows.Controls;

namespace Spectabis_WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            //If emuDir is not set, launch first time setup
            if(Properties.Settings.Default.emuDir == "null")
            {
                Frame FirstSetup = new Frame();
                FirstSetup.Source = new Uri("FirstTimeSetup.xaml", UriKind.Relative);
            }

        }

        //Shows & hides overlay, when appropriate
        private void MenuToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            Overlay(true);
        }

        private void MenuToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            Overlay(false);
        }

        //Show or hide black overlay
        private void Overlay(bool _show)
        {
            if(_show == true)
            {
                sideMenu.Visibility = Visibility.Visible;
                overlay.Opacity = .5;
                overlay.IsEnabled = true;
                overlay.IsHitTestVisible = true;
            }
            else
            {
                sideMenu.Visibility = Visibility.Collapsed;
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
            Overlay(false);

        }

        //Menu - Settings Button
        private void Menu_Settings_Click(object sender, RoutedEventArgs e)
        {
            mainFrame.Source = new Uri("Settings.xaml", UriKind.Relative);
            Overlay(false);
        }
    }
}