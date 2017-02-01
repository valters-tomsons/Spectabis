using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MaterialDesignColors;
using MaterialDesignThemes.Wpf;

namespace Spectabis_WPF.Views
{
    /// <summary>
    /// Interaction logic for Themes.xaml
    /// </summary>
    public partial class Themes : Page
    {

        public IEnumerable<Swatch> Swatches { get; private set; }

        public Themes()
        {
            InitializeComponent();

            //make a list of all available swatches
            Swatches = new SwatchesProvider().Swatches;
            Console.WriteLine("Enumerating swatches...");

            //Load swatches
            LoadSwatches();
        }

        //Set Primary color method
        private void SetPrimary(string swatch)
        {
            Console.WriteLine("Setting PrimaryColor to " + swatch);
            new PaletteHelper().ReplacePrimaryColor(swatch);
        }

        //Load Swatches in WrapPanel
        private void LoadSwatches()
        {
            //Create a card for each swatch in swatch list
            foreach(Swatch swatch in Swatches)
            {
                CreateCard(swatch);
            }
        }

        //Create a swatch card and add it to wrappanel
        private void CreateCard(Swatch swatch)
        {
            string tag = swatch.Name;

            //Create a container card
            Card cardContainer = new Card();
            cardContainer.Height = 86;
            cardContainer.Width = 180;
            cardContainer.Style = Resources["CardStyle"] as Style;

            //Place a StackPanel inside card
            StackPanel cardPanel = new StackPanel();
            cardPanel.Orientation = Orientation.Vertical;
            cardContainer.Content = cardPanel;

            //Create a label inside Stackpanel which contains name of the swatch
            Label swatchName = CreateLabel(swatch, cardContainer.Width);
            cardPanel.Children.Add(swatchName);

            //Create a button
            Button ChangeButton = CreateButton(swatch);
            cardPanel.Children.Add(ChangeButton);

            //Add the card to wrappanel
            SwatchPanel.Children.Add(cardContainer);

        }

        //Create a Label
        private Label CreateLabel(Swatch swatch, double Width)
        {
            //Create a label inside Stackpanel which contains name of the swatch
            Label swatchName = new Label();
            swatchName.Content = swatch.Name;
            swatchName.Width = Width;
            //Show, that the bluegrey is the default color
            if (swatch.Name == "bluegrey")
            {
                swatchName.Content = swatch.Name + " (default)";
            }
            swatchName.HorizontalContentAlignment = HorizontalAlignment.Center;
            return swatchName;
        }

        //Create A Button
        private Button CreateButton(Swatch swatch)
        {
            Button ChangeButton = new Button();
            ChangeButton.Margin = new Thickness(15, 0, 15, 0);
            ChangeButton.Tag = swatch.Name;

            //Create a stackpanel inside button
            StackPanel content = new StackPanel();
            content.Orientation = Orientation.Horizontal;
            ChangeButton.Content = content;

            //Add an icon to button's content
            PackIcon icon = CreateIcon();
            content.Children.Add(icon);

            //Add text to button's content
            TextBlock text = new TextBlock();
            text.Text = " Apply";
            content.Children.Add(text);

            //Set the color of the button from hue in swatch
            Hue hue = swatch.PrimaryHues.ElementAt(7);
            SolidColorBrush brush = new SolidColorBrush(swatch.PrimaryHues.ElementAt(7).Color); //Selects seventh hue in selected swatch
            ChangeButton.Foreground = new SolidColorBrush(Colors.White); //Sets icon and text color to white in all situations
            ChangeButton.Background = brush;
            ChangeButton.BorderBrush = brush;

            ChangeButton.Click += ChangeButton_Click;

            return ChangeButton;
        }

        private PackIcon CreateIcon()
        {
            PackIcon icon = new PackIcon();
            icon.Kind = PackIconKind.Palette;
            return icon;
        }

        //Button created inside swatch card click event
        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            //Get sender button's tag
            string SwatchName = Convert.ToString(((Button)sender).Tag);

            // Save swatch to settings
            Properties.Settings.Default.swatch = SwatchName;
            Properties.Settings.Default.Save();

            //Create a card from button's tag
            SetPrimary(SwatchName);

            Console.WriteLine("Swatch saved - " + Properties.Settings.Default.swatch);
        }
    }
}
