using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheGamesDBAPI;

namespace Spectabis_WPF.Domain.Scraping.Api {
    public class TheGamesDbApi : IScraperApi {
        public GameInfoModel GetDataFromApi(string title) {
            title = new [] { "/", @"\", ":" }.Aggregate(title, (acc,source)=>acc.Replace(source, string.Empty));

            try {
                //Search all games with given title and filter only PS2 titles
                foreach (GameSearchResult game in GamesDB.GetGames(title, "Sony Playstation 2")) {

                    //Gets game's database ID
                    Game newGame = GamesDB.GetGame(game.ID);

                    //Creates a link for image url
                    var imgUrl = "http://thegamesdb.net/banners/" + newGame.Images.BoxartFront.Path;

                    //Downloads the image
                    ScrapeArt.SaveImageFromUrl(title, imgUrl);

                    return new GameInfoModel {
                        Id = game.ID,
                        Title = game.Title,
                        InfoSource = ScrapeSource.TheGamesDB,
                        ThumbnailUrl = imgUrl
                    };
                }
            }
            catch {
                Console.WriteLine("Failed to connect to TheGamesDB");
            }

            return null;
        }
    }
}
