using System.Windows;
using System.Windows.Controls;
using Spectabis_WPF.Domain;
using System.Linq;
using System;
using System.Collections.Generic;

namespace Spectabis_WPF.Views
{
    /// <summary>
    /// Interaction logic for GameDiscovery.xaml
    /// </summary>
    public partial class GameDiscovery : Page
    {
        private List<string> GameList = new List<string>((((MainWindow)Application.Current.MainWindow).NewGamesInDirectory));
        private List<string> gamesToAdd = new List<string>();

        public GameDiscovery()
        {
            InitializeComponent();
            GameListView.SelectionChanged += (sender, e) => GameListView_SelectionChanged();
            MakeData();
        }

        //When selecting an item, flip its checkbox and deselect item visually
        private void GameListView_SelectionChanged()
        {
            foreach (StackPanel panel in GameListView.SelectedItems)
            {
                foreach (CheckBox box in panel.Children)
                {
                    box.IsChecked = (box.IsChecked == true) ? false : true;
                    GameListView.SelectedIndex = -1;
                    break;
                }
                break;
            }
        }

        //Create an entry in list for each game
        private void MakeData()
        {
            //go through each game file and make an entry for the GameList list panel
            foreach(var game in GameList)
            {
                //if file is a supported game format
                if(SupportedGames.GameFiles.Any(s => game.EndsWith(s)))
                {
                    StackPanel panel = new StackPanel();
                    panel.Orientation = Orientation.Horizontal;

                    CheckBox check = new CheckBox();
                    check.Margin = new Thickness(0, 0, 10, 0);
                    panel.Children.Add(check);

                    TextBlock text = new TextBlock();
                    text.Text = game;
                    panel.Children.Add(text);

                    GameListView.Items.Add(panel);
                }
            }
        }

        private void AddSelected_Click(object sender, RoutedEventArgs e)
        {
            foreach(StackPanel panel in GameListView.Items)
            {
                bool isChecked = false;

                foreach (var obj in panel.Children)
                {
                    if (obj.GetType().ToString() == "System.Windows.Controls.CheckBox")
                    {
                        CheckBox check = (CheckBox)obj;
                        if(check.IsChecked == true)
                        {
                            isChecked = true;
                        }
                    }

                    if(obj.GetType().ToString() == "System.Windows.Controls.TextBlock")
                    {
                        if(isChecked)
                        {
                            TextBlock text = (TextBlock)obj;
                            gamesToAdd.Add(text.Text);
                        }
                    }
                }
            }
            CreateProfiles();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            Back();
        }

        private void CreateProfiles()
        {
            foreach(string game in gamesToAdd)
            {
                GameProfile.Create(null, game, GetGameName.GetName(game));
            }

            Console.WriteLine("Game profiles created!");
            Back();
        }

        private void Back()
        {
            ((MainWindow)Application.Current.MainWindow).Open_Library();
        }
    }
}
