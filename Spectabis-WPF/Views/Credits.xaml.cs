using System;
using System.Windows;
using System.Windows.Controls;

namespace Spectabis_WPF.Views
{
    /// <summary>
    /// Interaction logic for Credits.xaml
    /// </summary>
    public partial class Credits : Page
    {
        public Credits()
        {
            InitializeComponent();
        }

		void Link_Click(object sender, RoutedEventArgs args) {
			string url = Convert.ToString(((MaterialDesignThemes.Wpf.Card)sender).Tag);
			System.Diagnostics.Process.Start(url);
		}
    }
}
