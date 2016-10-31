using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
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
        }

        //Saves Settings
        private void SaveSettings()
        {
            //Sets values to save
            Properties.Settings.Default.doubleClick = Convert.ToBoolean(doubleclick.IsChecked);
            Properties.Settings.Default.showTitle = Convert.ToBoolean(showTitles.IsChecked);

            //Save settings
            Properties.Settings.Default.Save();
            Debug.WriteLine("Settings Saved");
        }

        //Save Settings every time something is changed!!!

        private void doubleclick_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();
        }

        private void showTitles_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();
        }
    }
}
