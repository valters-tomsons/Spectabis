using System;
using System.Net.Configuration;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using BigPicture.Model;
using BigPicture.Domain;

namespace BigPicture
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();

            DiscoverGames dg = new DiscoverGames();
            List<GameProfile> games = dg.AllGames();

            Console.WriteLine(SpectabisSettings.EmuDir);
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
