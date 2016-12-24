using System.Linq;
using System.Threading.Tasks;

namespace Spectabis_WPF.Scraping {
	public class GiantBomb : IScrape { // todo i can't test it

		public async Task<GameModel> Scrape(string fileName, string gameId){
			var client = new GiantBombApi.GiantBombRestClient("MOTHER, FUCKER!"); // Todo get the API key
			var res = (await client.SearchForAllGamesAsync(gameId)).ToArray();
			if (res.Any() == false) return null;


			var gameResult= res.First();
			string imageFile = null;
			if (gameResult.Image != null)
				imageFile = await WebRequest.GetImage(gameId, gameResult.Image.MediumUrl);

			return new GameModel{
				Title = gameResult.Name,
				Serial = gameId,
				Description = gameResult.Description,
				ImagePath = imageFile,
				//Location = "",
				MetacriticScore = gameResult.OriginalGameRating,
				//Notes = "",
				Publisher = string.Join(", ", gameResult.Publishers),
				Region = string.Join(", ", gameResult.Releases.Select(p=>p.Region)),
				ReleaseDate = (
					from release in gameResult.Releases
					let releaseDate = release.ReleaseDate
					where releaseDate != null
					select (System.DateTime)releaseDate
				).Min(),
				Compatibility = -1
			};
		}

	}
}
