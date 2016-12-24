using System;
using System.Linq;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Media.Imaging;

namespace Spectabis_WPF.Views {
	public enum AddGameFormState{
		Waiting,
		Downloading,
		Completed,
		Failed
	}

	public partial class AddGame {
		public AddGame() {
			InitializeComponent();
			AutoBoxart.IsEnabled = Properties.Settings.Default.autoBoxart;
		}

		public static readonly DependencyProperty FormStateProperty = DependencyProperty.Register(
			"FormState", typeof (AddGameFormState), typeof (AddGame), new PropertyMetadata(default(AddGameFormState)));

		public AddGameFormState FormState
		{
			get { return (AddGameFormState) GetValue(FormStateProperty); }
			set { SetValue(FormStateProperty, value); }
		}

		public static readonly DependencyProperty GameArtSourceProperty = DependencyProperty.Register(
			"GameArtSource", typeof(BitmapImage), typeof(AddGame),
			new PropertyMetadata(default(BitmapImage)));

		public BitmapImage GameArtSource
		{
			get { return (BitmapImage)GetValue(GameArtSourceProperty); }
			set { SetValue(GameArtSourceProperty, value); }
		}

		private static readonly string[] DiscExtensions = { "iso", "gz", "cso", "bin", "nrg", "img" };
		private static readonly string[] ImageExtensions = { "jpeg", "jpg", "png" };

		private async void SelectGame_Click(object sender, RoutedEventArgs e) { // todo locks up application after "Downloading"
			var fileName = Dialog("Select Game", DiscExtensions);
			if (fileName == null) return;

			GameNameTextBox.IsEnabled = false;
			FormState = AddGameFormState.Downloading;

			var gameSerial = await SerialFinder.ExtractSerial(fileName);

			var gameInfo = await new Scraping.TheGamesDb().Scrape(System.IO.Path.GetFileNameWithoutExtension(fileName), gameSerial);

			if (gameInfo == null){
				FormState = AddGameFormState.Failed; // todo untested
				return;
			}

			FormState = AddGameFormState.Completed;
			GameArtSource = new BitmapImage(new Uri(gameInfo.ImagePath));
			GameNameTextBox.IsEnabled = true;

			GameNameTextBox.Text = gameInfo.Title;

			GameInfoTextBlock.Text = 
				"\r\nTitle: "			+ gameInfo.Title 			+
				"\r\nSerial: "			+ gameInfo.Serial 			+
			//	"\r\nDescription: "		+ gameInfo.Description 		+
				"\r\nImagePath: "		+ gameInfo.ImagePath 		+
				"\r\nLocation: "		+ gameInfo.Location 		+
				"\r\nMetacriticScore: " + gameInfo.MetacriticScore 	+
				"\r\nNotes: "			+ gameInfo.Notes 			+
				"\r\nPublisher: "		+ gameInfo.Publisher 		+
				"\r\nRegion: "			+ gameInfo.Region 			+
				"\r\nReleaseDate: "		+ gameInfo.ReleaseDate 		+
				"\r\nCompatibility: "	+ gameInfo.Compatibility 	+
				"\r\nTimePlayed: "		+ gameInfo.TimePlayed		;

			CompleteButton.IsEnabled = true;
		}

		private void SelectBoxart_Click(object sender, RoutedEventArgs e) {
			
		}

		private void Complete_Click(object sender, RoutedEventArgs e) {
			((MainWindow)Application.Current.MainWindow).reloadLibrary();
		}

		/////////////////////////////////////
		/////////// Utils ///////////////////
		private static string CreateFileFilter(IEnumerable<string> arr) {
			return string.Join("|", arr.Select(p => string.Format("{0} File|*.{1}", p.ToUpper(), p)));
		}

		public static string Dialog(string title, string[] filter, string initialDir = null) {
			var dialog = new Ookii.Dialogs.Wpf.VistaOpenFileDialog {
				Filter = CreateFileFilter(filter),
				InitialDirectory = initialDir,
				Multiselect = false
			};

			var res = dialog.ShowDialog();
			if (res != null && res.Value)
				return dialog.FileName;
			return null;
		}

	}
}
