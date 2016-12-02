using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using MaterialDesignColors;
using MahApps;

namespace Spectabis_WPF
{
    /// <summary>
    /// Interaction logic for Themes.xaml
    /// </summary>
    public partial class Themes : Page
    {
        //list of all swatches
        public IEnumerable<Swatch> Swatches { get; }

        public Themes()
        {
            InitializeComponent();

            //make a list of all available swatches
            Swatches = new SwatchesProvider().Swatches;
        }

        //Set Primary color method
        private void SetPrimary(Swatch swatch)
        {
            new PaletteHelper().ReplacePrimaryColor(swatch);
            
        }


        private void button_Click(object sender, RoutedEventArgs e)
        {
            
            foreach (Swatch swatch in Swatches)
            {
                SetPrimary(swatch);
                break;
            }
            //SetPrimary();
        }
    }
}
