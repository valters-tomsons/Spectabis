using System;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using CsQuery.ExtensionMethods;
using MaterialDesignThemes.Wpf;
using Spectabis_WPF.Domain;
using Spectabis_WPF.Domain.Scraping;
using Button = System.Windows.Controls.Button;

namespace Spectabis_WPF.Views
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
            nightMode.IsChecked = Properties.Settings.Default.nightMode;
            AutoScrapping.IsChecked = Properties.Settings.Default.autoBoxart;
            SearchBar.IsChecked = Properties.Settings.Default.searchbar;
            Tooltips.IsChecked = Properties.Settings.Default.tooltips;
            checkUpdates.IsChecked = Properties.Settings.Default.checkupdates;
            TitleAsFile.IsChecked = Properties.Settings.Default.titleAsFile;
            Playtime.IsChecked = Properties.Settings.Default.playtime;

			PopulateApiList();

			emudir_text.Text = Properties.Settings.Default.emuDir;
        }

	    private void PopulateApiList() {
		    var getProp = new Func<string, PropertyInfo>(p => typeof(Properties.Settings).GetProperty(p));
		    var apiList = new[]{
				    new ApiItem{ ApiName = "Giant Bomb" , PropertyInfo = getProp(nameof(Properties.Settings.APIKey_GiantBomb)) , ScraperApi = ScrapeArt.Scrapers.Values.OfType<Domain.Scraping.Api.GiantBombApi>().First() },
				    new ApiItem{ ApiName = "TheGamesDB" , PropertyInfo = getProp(nameof(Properties.Settings.APIKey_TheGamesDb)), ScraperApi = ScrapeArt.Scrapers.Values.OfType<Domain.Scraping.Api.TheGamesDbApi>().First() },
				    new ApiItem{ ApiName = "IGDB"       , PropertyInfo = getProp(nameof(Properties.Settings.APIKey_IGDB))      , ScraperApi = ScrapeArt.Scrapers.Values.OfType<Domain.Scraping.Api.IGDBApi>().First() },
				    new ApiItem{ ApiName = "Moby Games" , PropertyInfo = getProp(nameof(Properties.Settings.APIKey_MobyGames)) , ScraperApi = ScrapeArt.Scrapers.Values.OfType<Domain.Scraping.Api.MobyGamesApi>().First() },
				    new ApiItem{ ApiName = "TheGamesDb Open", IsFreeApi = true, ScraperApi = ScrapeArt.Scrapers.Values.OfType<Domain.Scraping.Api.TheGamesDbHtml>().First() }
			    }
				.OrderBy(p=>ScrapeArt.Scrapers.First(pp=>pp.Value == p.ScraperApi).Key)
				.Select((p, i) => {
				    p.Id = i;
				    return p;
			    })
			    .ToArray();

		    var order = new[] {
				    Properties.Settings.Default.APIUserSequence,
				    Properties.Settings.Default.APIAutoSequence
			    }
			    .First(p => string.IsNullOrEmpty(p) == false)
			    .Split(',')
			    .Select(int.Parse)
			    .ToArray();

			apiList = apiList.Select((p, i) => apiList[order[i]]).ToArray();

		    ApiKeysListView.ItemsSource = null;
		    ApiKeysListView.Items.Clear();
			ApiKeysListView.ItemsSource = apiList.ToArray();
		}

        //Saves Settings
        private void SaveSettings()
        {
            //Sets values to save
            Properties.Settings.Default.doubleClick = Convert.ToBoolean(doubleclick.IsChecked);
            Properties.Settings.Default.showTitle = Convert.ToBoolean(showTitles.IsChecked);
            Properties.Settings.Default.nightMode = Convert.ToBoolean(nightMode.IsChecked);
            Properties.Settings.Default.autoBoxart = Convert.ToBoolean(AutoScrapping.IsChecked);
            Properties.Settings.Default.searchbar = Convert.ToBoolean(SearchBar.IsChecked);
            Properties.Settings.Default.tooltips = Convert.ToBoolean(Tooltips.IsChecked);
            Properties.Settings.Default.checkupdates = Convert.ToBoolean(checkUpdates.IsChecked);
            Properties.Settings.Default.titleAsFile = Convert.ToBoolean(TitleAsFile.IsChecked);
            Properties.Settings.Default.playtime = Convert.ToBoolean(Playtime.IsChecked);

            //Save settings
            Properties.Settings.Default.Save();
            Console.WriteLine("Settings Saved");

            //Load Nightmode
            new PaletteHelper().SetLightDark(Properties.Settings.Default.nightMode);
        }

	    private void ApiItemUpClick(object sender, RoutedEventArgs e) {
		    var button = sender as Button;
			if (button == null)
				return;

		    var item = button.DataContext as ApiItem;
		    if (item == null)
			    return;

		    var index = ApiKeysListView.Items.IndexOf(item);
		    if (index == 0 || index == -1)
			    return;

		    var items = ApiKeysListView.ItemsSource as ApiItem[];
		    if (items == null)
			    return;
		    ApiKeysListView.ItemsSource = null;

		    items[index] = items[index - 1];
		    items[index - 1] = item;

			ApiKeysListView.Items.Clear();
			ApiKeysListView.ItemsSource = items;

		    Properties.Settings.Default.APIUserSequence = string.Join(",",items.Select(p => p.Id));
		    Properties.Settings.Default.Save();
		}

	    private void ApiItemDownClick(object sender, RoutedEventArgs e) {
			var button = sender as Button;
		    if (button == null)
			    return;

		    var item = button.DataContext as ApiItem;
		    if (item == null)
			    return;

		    var index = ApiKeysListView.Items.IndexOf(item);
		    if (index == ApiKeysListView.Items.Count - 1 || index == -1)
			    return;

		    var items = ApiKeysListView.ItemsSource as ApiItem[];
		    if (items == null)
			    return;
		    ApiKeysListView.ItemsSource = null;

			items[index] = items[index + 1];
		    items[index + 1] = item;

			ApiKeysListView.Items.Clear();
		    ApiKeysListView.ItemsSource = items;

		    Properties.Settings.Default.APIUserSequence = string.Join(",", items.Select(p => p.Id));
		    Properties.Settings.Default.Save();
		}

	    private void ResetPrioritiesClick(object sender, RoutedEventArgs e) {
		    Properties.Settings.Default.APIUserSequence = null;
		    Properties.Settings.Default.Save();
		    PopulateApiList();
		}

		//Save Settings when checkbox is clicked
		private void checkbox_Click(object sender, RoutedEventArgs e)
        {
            SaveSettings();
        }

        //Save PCSX2 directory button
        private void Save_Directory(object sender, RoutedEventArgs e)
        {
            if (Directory.Exists(emudir_text.Text))
            {
                if (File.Exists(emudir_text.Text))
                {
                    Properties.Settings.Default.emuDir = emudir_text.Text;
                    Properties.Settings.Default.Save();

                    PushSnackbar("Emulator directory saved");
                }
                else
                {
                    PushSnackbar("Directory must contain PCSX2.exe");
                }
            }
            else
            {
                PushSnackbar("Directory does not exist");
            }
        }

        //Push snackbar function
        public void PushSnackbar(string message)
        {
            var messageQueue = Snackbar.MessageQueue;

            //the message queue can be called from any thread
            Task.Factory.StartNew(() => messageQueue.Enqueue(message));
        }

        //PCSX2 directory button 
        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            Ookii.Dialogs.Wpf.VistaFolderBrowserDialog BrowserDialog = new Ookii.Dialogs.Wpf.VistaFolderBrowserDialog();
            BrowserDialog.Description = "Select PCSX2 Directory";
            BrowserDialog.UseDescriptionForTitle = true;

            //Create a return point, if selected path is invalid
            ShowDialog:
            //Show the dialog and check, if directory contains pcsx2.exe
            var BrowserResult = BrowserDialog.ShowDialog();
            //If OK was clicked...
            if(BrowserResult == true)
            {
                var filesInFolder = Directory.GetFiles(BrowserDialog.SelectedPath);

                if (filesInFolder.Any(p=>p.Contains("pcsx2") && p.EndsWith(".exe")) == false){
                    //If directory isn't PCSX2's, fall back to beginning
                    PushSnackbar("Invalid Emulator Directory");
                    goto ShowDialog;
                }
                //Set emudir textbox to location of selected directory
                emudir_text.Text = BrowserDialog.SelectedPath;
            }
        }
    }
}
