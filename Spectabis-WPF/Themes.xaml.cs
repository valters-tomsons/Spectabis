using System.Windows;
using System.Windows.Controls;
using MahApps;
using MaterialDesignThemes;
using MahApps.Metro;
using MahApps.Metro.Controls;
using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using MaterialDesignColors;
using System.Diagnostics;

namespace Spectabis_WPF
{
    /// <summary>
    /// Interaction logic for Themes.xaml
    /// </summary>
    public partial class Themes : Page
    { 
        public Themes()
        {
            InitializeComponent();
        }

        private void SetPrimary(Swatch swatch)
        {
            new PaletteHelper().ReplacePrimaryColor(swatch);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //SetPrimary("BlueGrey700Primary");
        }
    }
}
