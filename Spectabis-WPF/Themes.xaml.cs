using System.Windows;
using System.Windows.Controls;
using MaterialDesignThemes.Wpf;
using System.Collections.Generic;
using MaterialDesignColors;
using MahApps;
using System;
using System.Diagnostics;

namespace Spectabis_WPF
{
    /// <summary>
    /// Interaction logic for Themes.xaml
    /// </summary>
    public partial class Themes : Page
    {

        public IEnumerable<Swatch> Swatches { get; }

        public Themes()
        {
            InitializeComponent();

            //make a list of all available swatches
            Swatches = new SwatchesProvider().Swatches;
            Debug.WriteLine("Enumerating swatches...");

            //Load swatches
            LoadSwatches();
        }

        //Set Primary color method
        private void SetPrimary(string swatch)
        {
            Debug.WriteLine("Setting PrimaryColor to " + swatch);
            new PaletteHelper().ReplacePrimaryColor(swatch);
        }

        //Load Swatches in WrapPanel
        private void LoadSwatches()
        {
            //Create a card for each swatch in swatch list
            foreach(Swatch swatch in Swatches)
            {
                CreateCard(swatch.Name);
            }
        }

        //Create a swatch card and add it to wrappanel
        private void CreateCard(string tag)
        {
            //Create a container card
            Card cardContainer = new Card();
            cardContainer.Height = 86;
            cardContainer.Width = 200;
            cardContainer.Style = Resources["CardStyle"] as Style;

            //Place a StackPanel inside card
            StackPanel cardPanel = new StackPanel();
            cardPanel.Orientation = Orientation.Vertical;
            cardContainer.Content = cardPanel;

            //Create a label inside Stackpanel which contains name of the swatch
            Label swatchName = new Label();
            swatchName.Content = tag;
            swatchName.Width = cardContainer.Width;
            //Show, that the bluegrey is the default color
            if (tag == "bluegrey")
            {
                swatchName.Content = tag + "(default)";
            }
            swatchName.HorizontalContentAlignment = HorizontalAlignment.Center;
            cardPanel.Children.Add(swatchName);

            //Create a button inside StackPanel which has tag value of swatch name
            Button ChangeButton = new Button();
            ChangeButton.Margin = new Thickness(15,0,15,0);
            ChangeButton.Content = "Apply";
            ChangeButton.Tag = tag;
            ChangeButton.Click += ChangeButton_Click;
            cardPanel.Children.Add(ChangeButton);

            //Add the container card to wrappanel
            SwatchPanel.Children.Add(cardContainer);

        }

        //Button inside Swatch card
        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            //Get sender button's tag
            string SwatchName = Convert.ToString(((Button)sender).Tag);

            //Create a card from button's tag
            SetPrimary(SwatchName);
        }
    }
}
