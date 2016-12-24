using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spectabis_WPF.Scraping {
	public class TheGamesDb : IScrape {

		public async Task<GameModel> Scrape(string fileName, string gameId){
			var gamesResult = await Task.Run(() => TheGamesDBAPI.GamesDB.GetGames(fileName, "Sony Playstation 2"));
			var firstResult = gamesResult.FirstOrDefault();
			if (firstResult == null) return null;
			var gameResult = await Task.Run(() => TheGamesDBAPI.GamesDB.GetGame(firstResult));

			var imageUrl = gameResult.Images.BoxartFront.Path;
			string image = null;
			if (imageUrl != null)
				image = await WebRequest.GetImage(gameId, TheGamesDBAPI.GamesDB.BaseImgURL + imageUrl);

			DateTime outDate;
			if (DateTime.TryParse(gameResult.ReleaseDate, out outDate) == false)
				outDate = default(DateTime);

			return new GameModel{
				Title = gameResult.Title,
				Serial = gameId,
				Description = gameResult.Overview,
				ImagePath = image,
				//Location = "",
				MetacriticScore = gameResult.Rating,
				//Notes = "",
				Publisher = gameResult.Publisher,
				//Region = "",
				ReleaseDate = outDate,
				Compatibility = -1
			};
		}

	}
}
