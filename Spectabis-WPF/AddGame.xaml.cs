using System;
using System.Collections.Generic;
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
    /// Interaction logic for AddGame.xaml
    /// </summary>
    public partial class AddGame : Page
    {
        public AddGame()
        {
            InitializeComponent();
        }

        //Back button
        private void backbutton_Click(object sender, RoutedEventArgs e)
        {
            //Invokes MainWindow method, navigates to Library
            ((MainWindow)Application.Current.MainWindow).Open_Library();
        }

        private void autoboxart_checkbox_Click(object sender, RoutedEventArgs e)
        {
            if(autoboxart_checkbox.IsChecked == true)
            {
                Manual_Browse.IsEnabled = false;
                Manual_Directory.IsEnabled = false;
                Manual_Browse.Opacity = 0;
                Manual_Directory.Opacity = 0;
            }
            else
            {
                Manual_Browse.IsEnabled = true;
                Manual_Directory.IsEnabled = true;
                Manual_Browse.Opacity = 100;
                Manual_Directory.Opacity = 100;
            }
        }
    }
}
