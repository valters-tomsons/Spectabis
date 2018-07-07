using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Spectabis_WPF.Domain.Scraping.Api {
    public class GiantBombApi : IScraperApi {

        private const string DefaultApiKey = "be7ee5819d83b10c66965a1688b280c2edfc3156";
        public static string ApiKey { get {
                var config = Properties.Settings.Default.APIKey_GiantBomb;
                if (string.IsNullOrEmpty(config))
                    return DefaultApiKey;
                return config;
        } }

        public GameInfoModel GetDataFromApi(string title) {
            //Variables
            var giantBomb = new global::GiantBombApi.GiantBombRestClient(ApiKey);

            //list for game results
            var resultGame = new List<global::GiantBombApi.Model.Game>();

            // unused
            //var PlatformFilter = new Dictionary<string, object>() { { "platform", "PlayStation 2" } };

            //Search the DB for a game ID
            try {
                resultGame = giantBomb.SearchForGames(title).ToList();
            }
            catch {
                Console.WriteLine("Failed to connect to Giantbomb");
                return null;
            }

            try {
                //loops through each game in resultGame list
                foreach (global::GiantBombApi.Model.Game game in resultGame) {
                    //Gets game ID and makes a list of platforms it's available for
                    var finalGame = giantBomb.GetGame(game.Id);
                    List<global::GiantBombApi.Model.Platform> platforms = new List<global::GiantBombApi.Model.Platform>(finalGame.Platforms);

                    //If game platform list contains "PlayStation 2", then start scraping
                    foreach (var gamePlatform in platforms) {
                        if (gamePlatform.Name == "PlayStation 2") {
                            string imgUrl = finalGame.Image.SmallUrl;

                            ScrapeArt.SaveImageFromUrl(title, imgUrl);

                            return new GameInfoModel {
                                Id = finalGame.Id,
                                Title = finalGame.Name,
                                InfoSource = ScrapeSource.GiantBomb,
                                ThumbnailUrl = imgUrl
                            };
                        }
                    }
                }
            }
            catch {
                Console.WriteLine("Failed to connect to Giantbomb");
            }
            return null;
        }
    }
}
